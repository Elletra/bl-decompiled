function SimGroup::chainBlink ( %group, %idx, %count, %firstPass, %timeBetween )
{
	%idx = mFloor (%idx);

	if ( %idx == 0  &&  %firstPass == 1 )
	{
		if ( %group.isChainBlinking )
		{
			return;
		}
		else
		{
			%group.isChainBlinking = 1;
		}
	}


	for ( %i = 0;  %i < 3;  %i++ )
	{
		if ( %idx < %group.getCount() )
		{
			%obj = %group.getObject (%idx);
		}
		else
		{
			if ( %count != 0 )
			{
				%count--;
				%group.schedule (%timeBetween, chainBlink, 0, %count - 1.0, 0, %timeBetween);
			}
			else
			{
				%group.isChainBlinking = 0;
			}

			return;
		}

		if ( %obj.getClassName() $= "FxDtsBrick" )
		{
			if ( %obj.isPlanted() )
			{
				if ( %firstPass  ||  %obj.oldColor $= "" )
				{
					%obj.oldColor = %obj.getColorID();
					%obj.oldColorFX = %obj.getColorFxID();
					%obj.setColorFX (3);
				}

				if ( %count == 0 )
				{
					%obj.setColor (%obj.oldColor);
					%obj.setColorFX (%obj.oldColorFX);
					%obj.oldColor = "";
					%obj.oldColorFX = "";
				}
				else
				{
					%x = %count % 2;

					if ( %x == 0 )
					{
						echo ("hilight 0");
						%obj.setColor ($HilightColor);
					}
					else if ( %x == 1 )
					{
						%obj.setColor ($HilightColor);
					}
					else
					{
						echo ("wtf should not happen");  // WTF ?!?!?!
					}
				}
			}
		}

		%idx++;
	}

	%group.schedule (1, chainBlink, %idx, %count, %firstPass, %timeBetween);
}


function serverCmdHilightBrickGroup ( %client, %bl_id )
{
	if ( !%client.isAdmin )
	{
		return;
	}

	%bl_id = mFloor (%bl_id);
	%group = "BrickGroup_" @  %bl_id;

	if ( !isObject(%group) )
	{
		return;
	}

	if ( %group.getClassName() !$= "SimGroup" )
	{
		error ("ERROR: ServerCmdClearBrickGroup() - " @  %group  @ " is not a SimGroup!");
		MessageAll ('', "ERROR: ServerCmdClearBrickGroup() - " @  %group  @ " is not a SimGroup!");
		return;
	}

	%bestColor = 0;
	%bestScore = 0;
	%worstColor = 0;
	%worstScore = 0;

	for ( %i = 0;  %i < $maxSprayColors;  %i++ )
	{
		%rgba = getColorIDTable (%i);

		%r = getWord(%rgba, 0);
		%g = getWord(%rgba, 1);
		%b = getWord(%rgba, 2);
		%a = getWord(%rgba, 3);

		%score = %r + %g + %b + 10 * %a;
		%lowScore = 1 - %r + 1 - %g + 1 - %b + %a * 10;

		if ( %lowScore > %worstScore )
		{
			%worstColor = %i;
			%worstScore = %lowScore;
		}

		if ( %score > %bestScore )
		{
			%bestColor = %i;
			%bestScore = %score;
		}
	}


	$HilightColor = %bestColor;
	$LowlightColor = %worstColor;

	if ( %group.getCount() > 10000 )
	{
		%time = 3000;
	}
	else if ( %group.getCount() > 4000 )
	{
		%time = 2000;
	}
	else if ( %group.getCount() > 2000 )
	{
		%time = 1500;
	}
	else
	{
		%time = 1000;
	}

	%group.chainBlink (0, 1, 1, %time);
}
