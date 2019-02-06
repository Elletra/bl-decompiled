function serverCmdRotateBrick ( %client, %dir )
{
	%dir = mFloor (%dir);

	if ( %dir == 0 )
	{
		return;
	}


	%player = %client.Player;
	%tempBrick = %player.tempBrick;

	if ( !isObject(%player) )
	{
		return;
	}


	if ( %dir > 0 )
	{
		%player.playThread (3, rotCW);
	}
	else
	{
		%player.playThread (3, rotCCW);
	}


	if ( !isObject(%tempBrick) )
	{
		return;
	}


	%brickTrans = %tempBrick.getTransform();

	%x = getWord (%brickTrans, 0);
	%y = getWord (%brickTrans, 1);
	%z = getWord (%brickTrans, 2);

	%brickAngle = getWord (%brickTrans, 6);
	%vectorDir = getWord (%brickTrans, 5);

	%forwardVec = %player.getForwardVector();
	%forwardX = getWord (%forwardVec, 0);
	%forwardY = getWord (%forwardVec, 1);


	if ( %tempBrick.angleID % 2 == 0 )
	{
		%shiftX = 0.25;
		%shiftY = 0.25;
	}
	else
	{
		%shiftX = -0.25;
		%shiftY = -0.25;
	}

	if ( %tempBrick.getDataBlock().brickSizeX % 2 == %tempBrick.getDataBlock().brickSizeY % 2 )
	{
		%shiftX = 0;
		%shiftY = 0;
	}


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
				%y -= %shiftY;
				%x -= %shiftX;
			}
		}
	}
	else if ( mAbs(%forwardX) > mAbs(%forwardY) )
	{
		%x += %shiftX;
		%y -= %shiftY;
	}
	else
	{
		if ( %forwardY > 0 )
		{
			%x += %shiftX;
		}
		else
		{
			%y -= %shiftY;
			%x -= %shiftX;
		}
	}


	if ( %vectorDir == -1 )
	{
		%brickAngle += $pi;
	}

	%brickAngle /= $piOver2;
	%brickAngle = mFloor (%brickAngle + 0.1);
	%brickAngle += %dir;


	if ( %brickAngle > 4 )
	{
		%brickAngle -= 4;
	}

	if ( %brickAngle <= 0 )
	{
		%brickAngle += 4;
	}

	%tempBrick.setTransform (%x SPC %y SPC %z  @ " 0 0 1 " @  %brickAngle * $piOver2);
	return;


	if ( %dir == 1 )
	{
		if ( %brickAngle == 1 )
		{
			shift (%tempBrick, 0, 0.5, 0);
		}
		else if ( %brickAngle == 2 )
		{
			shift (%tempBrick, 0.5, 0, 0);
		}
		else if ( %brickAngle == 3 )
		{
			shift (%tempBrick, 0, -0.5, 0);
		}
		else if ( %brickAngle == 4 )
		{
			shift (%tempBrick, -0.5, 0, 0);
		}
	}
	else
	{
		if ( %brickAngle == 1 )
		{
			shift (%tempBrick, -0.5, 0, 0);
		}
		else if ( %brickAngle == 2 )
		{
			shift (%tempBrick, 0, 0.5, 0);
		}
		else if ( %brickAngle == 3 )
		{
			shift (%tempBrick, 0.5, 0, 0);
		}
		else if ( %brickAngle == 4 )
		{
			shift (%tempBrick, 0, -0.5, 0);
		}
	}
}
