function colorMatch ( %colorA, %colorB )
{
	for ( %i = 0;  %i < 4;  %i++ )
	{
		if ( mAbs ( getWord(%colorA, %i) - getWord(%colorB, %i) )  >  0.005 )
		{
			return false;
		}
	}

	return true;
}

function serverCmdSetColorMethod ( %client, %val )
{
	if ( !%client.isAdmin )
	{
		return;
	}

	if ( $LoadingBricks_Client != %client )
	{
		return;
	}

	if ( $Pref::Server::AllowColorLoading )
	{
		$LoadingBricks_ColorMethod = %val;
	}
	else
	{
		$LoadingBricks_ColorMethod = 3;
	}
}

function ServerLoadSaveFile_ProcessColorData ()
{
	%colorCount = -1;

	for ( %i = 0;  %i < 64;  %i++ )
	{
		if ( getWord ( getColorIDTable(%i), 3 ) > 0.001 )
		{
			%colorCount++;
		}
	}

	if ( $LoadingBricks_ColorMethod != 0 )
	{
		if ( $LoadingBricks_ColorMethod == 1 )
		{
			%divCount = 0;

			for ( %i = 0;  %i < 16;  %i++ )
			{
				if ( getSprayCanDivisionSlot(%i) != 0 )
				{
					%divCount++;
				}
				else
				{
					break;
				}
			}
		}
		else if ( $LoadingBricks_ColorMethod == 2 )
		{
			%colorCount = -1;
			%divCount = -1;
		}
	}

	for ( %i = 0;  %i < 64;  %i++ )
	{
		%color = $Server_LoadFileObj.readLine();

		%red = getWord (%color, 0);
		%green = getWord (%color, 1);
		%blue = getWord (%color, 2);
		%alpha = getWord (%color, 3);

		if ( $LoadingBricks_ColorMethod == 0 )
		{
			if ( %alpha >= 0.0001 )
			{
				%match = 0;
				%j = 0;

				while ( %j < 64 )
				{
					if ( colorMatch ( getColorIDTable(%j),  %color ) )
					{
						$colorTranslation[%i] = %j;
						%match = 1;
					}
					else
					{
						// FIXME: is this supposed to be like this?
						%j++;
					}
				}

				if ( %match == 0 )
				{
					error ("ERROR: ServerLoadSaveFile_ProcessColorData() - color method 0 specified but match not found for color " @  
						%color);
				}
			}
		}
		else if ( $LoadingBricks_ColorMethod == 1 )
		{
			if ( %alpha >= 0.0001 )
			{
				%match = 0;
				%j = 0;

				while ( %j < 64 )
				{
					if ( colorMatch ( getColorIDTable(%j),  %color ) )
					{
						$colorTranslation[%i] = %j;
						%match = 1;
					}
					else
					{
						%j++;
					}
				}

				if ( %match == 0 )
				{
					%colorCount++;

					setSprayCanColor ( %colorCount + 1, %color );
					$colorTranslation[%i] = %colorCount;
				}
			}
		}
		else if ( $LoadingBricks_ColorMethod == 2 )
		{
			%colorCount++;

			setSprayCanColor (%colorCount + 1, %color);
			$colorTranslation[%i] = %i;
		}
		else if ( $LoadingBricks_ColorMethod == 3 )
		{
			if (%alpha >= 0.0001)
			{
				%minDiff = 99999;
				%matchIdx = -1;

				for ( %j = 0;  %j < 64; %j++ )
				{
					%checkColor = getColorIDTable (%j);

					%checkRed = getWord (%checkColor, 0);
					%checkGreen = getWord (%checkColor, 1);
					%checkBlue = getWord (%checkColor, 2);
					%checkAlpha = getWord (%checkColor, 3);

					%diff = 0;
					%diff += mAbs ( mAbs(%checkRed) - mAbs(%red) );
					%diff += mAbs ( mAbs(%checkGreen) - mAbs(%green) );
					%diff += mAbs ( mAbs(%checkBlue) - mAbs(%blue) );

					if ( %checkAlpha > 0.99  &&  %alpha < 0.99  ||  (%checkAlpha < 0.99  &&  %alpha > 0.99) )
					{
						%diff += 1000;
					}
					else
					{
						%diff += ( mAbs ( mAbs(%checkAlpha) - mAbs(%alpha) ) ) * 0.5;
					}

					if (%diff < %minDiff)
					{
						%minDiff = %diff;
						%matchIdx = %j;
					}
				}

				if ( %matchIdx == -1 )
				{
					error ("ERROR - LoadBricks() - Nearest match failed - wtf.");
				}
				else
				{
					$colorTranslation[%i] = %matchIdx;
				}
			}
		}
	}

	if ( $LoadingBricks_ColorMethod == 1 )
	{
		echo ("  setting spraycan division at ", %divCount, " ", %colorCount);
		setSprayCanDivision (%divCount, %colorCount, "File");
	}

	if ( $LoadingBricks_ColorMethod != 0  &&  $LoadingBricks_ColorMethod != 3 )
	{
		$maxSprayColors = %colorCount;

		for ( %clientIndex = 0;  %clientIndex < ClientGroup.getCount();  %clientIndex++ )
		{
			%cl = ClientGroup.getObject (%clientIndex);

			%cl.transmitStaticBrickData();
			%cl.transmitDataBlocks (1);

			commandToClient (%cl, 'PlayGui_LoadPaint');
		}
	}
}
