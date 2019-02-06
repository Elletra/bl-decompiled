function shift ( %obj, %x, %y, %z )
{
	%trans = %obj.getTransform();

	%transX = getWord (%trans, 0);
	%transY = getWord (%trans, 1);
	%transZ = getWord (%trans, 2);
	%transQuat = getWords (%trans, 3, 6);

	%obj.setTransform (%transX + %x  @ " " @  %transY + %y  @ " " @  %transZ + %z  @ " " @  %transQuat);
}


function serverCmdShiftBrick ( %client, %x, %y, %z )
{
	%x = mFloor (%x);
	%y = mFloor (%y);
	%z = mFloor (%z);

	%player = %client.Player;
	%tempBrick = %player.tempBrick;
	%controlObj = %client.getControlObject();


	if ( !isObject(%controlObj) )
	{
		return;
	}

	if ( isObject(%player) )
	{
		if ( %z > 0 )
		{
			%player.playThread (3, shiftUp);
		}
		else if ( %z < 0 )
		{
			%player.playThread (3, shiftDown);
		}
		else if ( %y > 0 )
		{
			%player.playThread (3, shiftLeft);
		}
		else if ( %y < 0 )
		{
			%player.playThread (3, shiftRight);
		}
		else if ( %x > 0 )
		{
			%player.playThread (3, shiftAway);
		}
		else if ( %x < 0 )
		{
			%player.playThread (3, shiftTO);
		}
	}

	if ( !isObject(%tempBrick) )
	{
		return;
	}


	if ( %tempBrick )
	{
		%forwardVec = %controlObj.getForwardVector();

		%forwardX = getWord (%forwardVec, 0);
		%forwardY = getWord (%forwardVec, 1);
		%forwardZ = getWord (%forwardVec, 2);

		if ( %forwardZ == -1 )
		{
			%forwardVec = %controlObj.getUpVector();
			%forwardX = getWord (%forwardVec, 0);
			%forwardY = getWord (%forwardVec, 1);
		}

		if ( %forwardX > 0 )
		{
			if ( %forwardX <= mAbs(%forwardY) )
			{
				if ( %forwardY > 0 )
				{
					%newY = %x;
					%newX = -1 * %y;
					%x = %newX;
					%y = %newY;
				}
				else
				{
					%newY = -1 * %x;
					%newX = 1 * %y;
					%x = %newX;
					%y = %newY;
				}
			}
		}
		else if ( mAbs(%forwardX) > mAbs(%forwardY) )
		{
			%x *= -1;
			%y *= -1;
		}
		else
		{
			if ( %forwardY > 0 )
			{
				%newY = %x;
				%newX = -1 * %y;
				%x = %newX;
				%y = %newY;
			}
			else
			{
				%newY = -1 * %x;
				%newX = 1 * %y;
				%x = %newX;
				%y = %newY;
			}
		}

		%x *= 0.5;
		%y *= 0.5;
		%z *= 0.2;

		shift (%tempBrick, %x, %y, %z);
	}
}

function serverCmdSuperShiftBrick ( %client, %x, %y, %z )
{
	%x = mFloor (%x);
	%y = mFloor (%y);
	%z = mFloor (%z);

	%player = %client.Player;
	%tempBrick = %player.tempBrick;
	%controlObj = %client.getControlObject();

	if ( !isObject(%controlObj) )
	{
		return;
	}


	if ( isObject(%player) )
	{
		if ( %z > 0 )
		{
			%player.playThread (3, shiftUp);
		}
		else if ( %z < 0 )
		{
			%player.playThread (3, shiftDown);
		}
		else if ( %y > 0 )
		{
			%player.playThread (3, shiftLeft);
		}
		else if ( %y < 0 )
		{
			%player.playThread (3, shiftRight);
		}
		else if ( %x > 0 )
		{
			%player.playThread (3, shiftAway);
		}
		else if ( %x < 0 )
		{
			%player.playThread (3, shiftTO);
		}
	}


	if ( !isObject(%tempBrick) )
	{
		return;
	}

	if ( %tempBrick )
	{
		%forwardVec = %controlObj.getForwardVector();
		%forwardX = getWord (%forwardVec, 0);
		%forwardY = getWord (%forwardVec, 1);
		%forwardZ = getWord (%forwardVec, 2);


		if ( %forwardZ == -1 )
		{
			%forwardVec = %controlObj.getUpVector();
			%forwardX = getWord (%forwardVec, 0);
			%forwardY = getWord (%forwardVec, 1);
		}

		if ( %forwardX > 0 )
		{
			if ( %forwardX <= mAbs(%forwardY) )
			{
				if ( %forwardY > 0 )
				{
					%newY = %x;
					%newX = -1 * %y;
					%x = %newX;
					%y = %newY;
				}
				else
				{
					%newY = -1 * %x;
					%newX = 1 * %y;
					%x = %newX;
					%y = %newY;
				}
			}
		}
		else if ( mAbs(%forwardX) > mAbs(%forwardY) )
		{
			%x *= -1;
			%y *= -1;
		}
		else
		{
			if ( %forwardY > 0 )
			{
				%newY = %x;
				%newX = -1 * %y;
				%x = %newX;
				%y = %newY;
			}
			else
			{
				%newY = -1 * %x;
				%newX = 1 * %y;
				%x = %newX;
				%y = %newY;
			}
		}


		%data = %tempBrick.getDataBlock();

		if ( %tempBrick.angleID == 0  ||  %tempBrick.angleID == 2 )
		{
			%x *= %data.brickSizeX;
			%y *= %data.brickSizeY;
			%z *= %data.brickSizeZ;
		}
		else if ( %tempBrick.angleID == 1  ||  %tempBrick.angleID == 3 )
		{
			%x *= %data.brickSizeY;
			%y *= %data.brickSizeX;
			%z *= %data.brickSizeZ;
		}

		%x *= 0.5;
		%y *= 0.5;
		%z *= 0.2;

		shift (%tempBrick, %x, %y, %z);
	}
}
