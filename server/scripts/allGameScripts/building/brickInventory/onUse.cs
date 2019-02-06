function fxDTSBrickData::onUse ( %this, %player, %InvSlot )
{
	if ( !isObject(%player) )
	{
		return;
	}

	%player.updateArm (%player.getDataBlock().brickImage);
	%player.mountImage (%player.getDataBlock().brickImage, 0);

	%client = %player.client;
	%client.currInv = %InvSlot;

	// FIXME???
	%InvSlot = -1;

	if ( -1 )  // FIXME???
	{
		%client.instantUseData = %this;
	}
	else
	{
		%client.instantUseData = 0;
	}

	if ( isObject(%player.tempBrick) )
	{
		%oldDB = %player.tempBrick.getDataBlock();

		if ( %player.tempBrick.angleID == 0 )
		{
			%oldXSize = %oldDB.brickSizeX;
			%oldYSize = %oldDB.brickSizeY;
			%newXSize = %this.brickSizeX;
			%newYSize = %this.brickSizeY;
		}
		else if ( %player.tempBrick.angleID == 2 )
		{
			%oldXSize = %oldDB.brickSizeX;
			%oldYSize = %oldDB.brickSizeY;
			%newXSize = %this.brickSizeX;
			%newYSize = %this.brickSizeY;
		}
		else if ( %player.tempBrick.angleID == 1 )
		{
			%oldXSize = %oldDB.brickSizeY;
			%oldYSize = %oldDB.brickSizeX;
			%newXSize = %this.brickSizeY;
			%newYSize = %this.brickSizeX;
		}
		else if ( %player.tempBrick.angleID == 3 )
		{
			%oldXSize = %oldDB.brickSizeY;
			%oldYSize = %oldDB.brickSizeX;
			%newXSize = %this.brickSizeY;
			%newYSize = %this.brickSizeX;
		}


		if ( %oldXSize % 2 > %newXSize % 2 )
		{
			%shiftX = 0.2;
		}
		else if ( %oldXSize % 2 < %newXSize % 2 )
		{
			%shiftX = -0.2;
		}
		else
		{
			%shiftX = 0;
		}

		if ( %oldYSize % 2 > %newYSize % 2 )
		{
			%shiftY = 0.2;
		}
		else if ( %oldYSize % 2 < %newYSize % 2 )
		{
			%shiftY = -0.2;
		}
		else
		{
			%shiftY = 0;
		}


		%player.tempBrick.setDatablock (%this);
		%trans = %player.tempBrick.getTransform();

		%x = getWord (%trans, 0);
		%y = getWord (%trans, 1);
		%z = getWord (%trans, 2);

		%rot = getWords (%trans, 3, 6);

		%forwardVec = %player.getForwardVector();

		%forwardX = getWord (%forwardVec, 0);
		%forwardY = getWord (%forwardVec, 1);

		if ( %forwardX > 0 )
		{
			if ( %forwardX <= mAbs(%forwardY) )
			{
				if ( %forwardY > 0 )
				{
					%x += %shiftX;
				}
				else
				{
					%y += %shiftY;
				}
			}
		}
		else if ( mAbs(%forwardX) > mAbs(%forwardY) )
		{
			%x += %shiftX;
			%y += %shiftY;
		}
		else if ( %forwardY > 0 )
		{
			%x += %shiftX;
		}
		else
		{
			%y += %shiftY;
		}

		%player.tempBrick.setTransform (%x SPC %y SPC %z SPC %rot);


		%aspectRatio = %this.printAspectRatio;

		if ( %aspectRatio !$= "" )
		{
			if ( %client.lastPrint[%aspectRatio] $= "" )
			{
				%player.tempBrick.setPrint ( $printNameTable["letters/A"] );
			}
			else
			{
				%player.tempBrick.setPrint ( %client.lastPrint[%aspectRatio] );
			}
		}
	}
}
