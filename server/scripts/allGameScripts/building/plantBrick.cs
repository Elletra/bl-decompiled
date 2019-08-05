function fxDTSBrickData::onPlant ( %this, %obj )
{
	// Your code here
}

function fxDTSBrick::onPlant ( %obj )
{
	%obj.getDataBlock().onPlant(%obj);
}

function fxDTSBrick::plantedTrustCheck ( %obj )
{
	%obj.chainTrustCheckDown (0);
}


function serverCmdPlantBrick ( %client )
{
	if ( $Game::MissionCleaningUp )
	{
		return 0;
	}


	%player = %client.Player;
	%tempBrick = %player.tempBrick;

	if ( !isObject(%player) )
	{
		return;
	}


	%player.playThread (3, plant);
	%mg = %client.miniGame;

	if ( isObject(%mg) )
	{
		if ( !%mg.EnableBuilding )
		{
			return 0;
		}
	}


	if ( getBrickCount() >= getBrickLimit() )
	{
		messageClient (%client, 'MsgPlantError_Limit');
		return 0;
	}

	if ( !%client.isAdmin  &&  !%client.isSuperAdmin )
	{
		if ( $Server::MaxBricksPerSecond > 0 )
		{
			%currTime = getSimTime();

			if ( %client.bpsTime + 1000 < %currTime )
			{
				%client.bpsCount = 0;
				%client.bpsTime = %currTime;
			}

			if ( %client.bpsCount >= $Server::MaxBricksPerSecond )
			{
				return 0;
			}
		}
	}

	if ( !isObject(%tempBrick) )
	{
		return 0;
	}


	%tempBrickTrans = %tempBrick.getTransform();
	%tempBrickPos = getWords (%tempBrickTrans, 0, 2);

	%brickData = %tempBrick.getDataBlock();

	if ( %brickData.brickSizeX > %brickData.brickSizeY )
	{
		%brickRadius = %brickData.brickSizeX;
	}
	else
	{
		%brickRadius = %brickData.brickSizeY;
	}


	%brickRadius = %brickRadius * 0.5 / 2;

	if ( $Pref::Server::TooFarDistance == 0  ||  $Pref::Server::TooFarDistance $= "" )
	{
		$Pref::Server::TooFarDistance = 50;
	}

	$Pref::Server::TooFarDistance = mClampF ($Pref::Server::TooFarDistance, 20, 99999);

	if ( VectorDist ( %tempBrickPos, %client.Player.getPosition() )  >  $Pref::Server::TooFarDistance + %brickRadius )
	{
		messageClient (%client, 'MsgPlantError_TooFar');
		return 0;
	}


	%plantBrick = new fxDTSBrick ()
	{
		dataBlock = %tempBrick.getDataBlock();
		position = %tempBrickTrans;
		isPlanted = 1;
	};
	%client.brickGroup.add (%plantBrick);

	%plantBrick.setTransform (%tempBrickTrans);
	%plantBrick.setColor ( %tempBrick.getColorID() );
	%plantBrick.setPrint ( %tempBrick.getPrintID() );
	%plantBrick.client = %client;

	%plantErrorCode = %plantBrick.plant();

	if ( !%plantBrick.isColliding() )
	{
		%plantBrick.dontCollideAfterTrust = 1;
	}

	%plantBrick.setColliding (0);


	if ( %plantErrorCode == 0 )
	{
		if ( !$Server::LAN )
		{
			if ( %plantBrick.getNumDownBricks() )
			{
				%plantBrick.stackBL_ID = %plantBrick.getDownBrick(0).stackBL_ID;
			}
			else if ( %plantBrick.getNumUpBricks() )
			{
				%plantBrick.stackBL_ID = %plantBrick.getUpBrick(0).stackBL_ID;
			}
			else
			{
				%plantBrick.stackBL_ID = %client.getBLID();
			}

			if ( %plantBrick.stackBL_ID <= 0 )
			{
				%plant.stackBL_ID = %client.getBLID();
			}
		}

		%client.undoStack.push (%plantBrick  TAB "PLANT");


		if ( $Server::LAN )
		{
			%plantBrick.trustCheckFinished();
		}
		else
		{
			%plantBrick.PlantedTrustCheck();
		}

		ServerPlay3D ( brickPlantSound, %plantBrick.getTransform() );


		if ( $Pref::Server::RandomBrickColor == 1 )
		{
			%randColor = getRandom (5);

			if ( %randColor == 0 )
			{
				%player.tempBrick.setColor (0);
			}
			else if ( %randColor == 1 )
			{
				%player.tempBrick.setColor (1);
			}
			else if ( %randColor == 2 )
			{
				%player.tempBrick.setColor (3);
			}
			else if ( %randColor == 3 )
			{
				%player.tempBrick.setColor (4);
			}
			else if ( %randColor == 4 )
			{
				%player.tempBrick.setColor (5);
			}
			else if ( %randColor == 5 )
			{
				%player.tempBrick.setColor (7);
			}
		}
		else
		{
			%player.tempBrick.setColor (%client.currentColor);
		}

		%client.bpsCount++;
	}
	else if ( %plantErrorCode == 1 )
	{
		%plantBrick.delete();
		messageClient (%client, 'MsgPlantError_Overlap');
	}
	else if ( %plantErrorCode == 2 )
	{
		%plantBrick.delete();
		messageClient (%client, 'MsgPlantError_Float');
	}
	else if ( %plantErrorCode == 3 )
	{
		%plantBrick.delete();
		messageClient (%client, 'MsgPlantError_Stuck');
	}
	else if ( %plantErrorCode == 4 )
	{
		%plantBrick.delete();
		messageClient (%client, 'MsgPlantError_Unstable');
	}
	else if ( %plantErrorCode == 5 )
	{
		%plantBrick.delete();
		messageClient (%client, 'MsgPlantError_Buried');
	}
	else
	{
		%plantBrick.delete();
		messageClient (%client, 'MsgPlantError_Forbidden');
	}


	if ( getBrickCount() <= 100  &&  getRayTracerProgress() <= -1  &&  
		 getRayTracerProgress() < 0  &&  $Server::LAN == 0  &&  doesAllowConnections() )
	{
		startRaytracer();
	}

	return %plantBrick;
}
