$Game::MinMountTime = 2000;


function Armor::onCollision ( %this, %obj, %col, %vec, %speed )
{
	if ( %obj.getState() $= "Dead" )
	{
		return;
	}

	if ( %col.getDamagePercent() >= 1.0 )
	{
		return;
	}


	%colClassName = %col.getClassName();

	if ( %colClassName $= "Item" )
	{
		%client = %obj.client;
		%colData = %col.getDataBlock();

		for ( %i = 0;  %i < %this.maxTools;  %i++ )
		{
			if ( %obj.tool[%i] == %colData )
			{
				return;
			}
		}

		%obj.pickup (%col);
	}
	else if ( %colClassName $= "Player"  ||  %colClassName $= "AIPlayer" )
	{
		if ( %col.getDataBlock().canRide  &&  %this.rideAble  &&  %this.nummountpoints > 0 )
		{
			if ( getSimTime() - %col.lastMountTime <= $Game::MinMountTime )
			{
				return;
			}

			%colZpos = getWord (%col.getPosition(), 2);
			%objZpos = getWord (%obj.getPosition(), 2);

			if ( %colZpos <= %objZpos + 0.2 )
			{
				return;
			}

			%canUse = false;

			if ( isObject(%obj.spawnBrick) )
			{
				%vehicleOwner = findClientByBL_ID (%obj.spawnBrick.getGroup().bl_id);
			}

			if ( isObject(%vehicleOwner) )
			{
				if ( getTrustLevel(%col, %obj) >= $TrustLevel::RideVehicle )
				{
					%canUse = true;
				}
			}
			else
			{
				%canUse = true;
			}

			if ( miniGameCanUse(%col, %obj) == 1 )
			{
				%canUse = true;
			}

			if ( miniGameCanUse(%col, %obj) == 0 )
			{
				%canUse = false;
			}


			if ( !%canUse )
			{
				if ( !isObject(%obj.spawnBrick) )
				{
					return;
				}

				%ownerName = %obj.spawnBrick.getGroup().name;
				%msg = %ownerName  @ " does not trust you enough to do that";

				if ( $lastError == $LastError::Trust )
				{
					%msg = %ownerName  @ " does not trust you enough to ride.";
				}
				else if ( $lastError == $LastError::MiniGameDifferent )
				{
					if ( isObject(%col.client.miniGame) )
					{
						%msg = "This vehicle is not part of the mini-game.";
					}
					else
					{
						%msg = "This vehicle is part of a mini-game.";
					}
				}
				else if ( $lastError == $LastError::MiniGameNotYours )
				{
					%msg = "You do not own this vehicle.";
				}
				else if ( $lastError == $LastError::NotInMiniGame )
				{
					%msg = "This vehicle is not part of the mini-game.";
				}

				commandToClient (%col.client, 'centerPrint', %msg, 1);
				return;
			}


			for ( %i = 0;  %i < %this.nummountpoints;  %i++ )
			{
				if ( %this.mountNode[%i] $= "" )
				{
					%mountNode = %i;
				}
				else
				{
					%mountNode = %this.mountNode[%i];
				}


				%blockingObj = %obj.getMountNodeObject (%mountNode);

				if ( isObject(%blockingObj) )
				{
					if ( %blockingObj.getDataBlock().rideAble  &&  !%blockingObj.getMountedObject(0) )
					{
						%blockingObj.mountObject (%col, 0);

						if ( %blockingObj.getControllingClient() == 0 )
						{
							%col.setControlObject (%blockingObj);
						}

						%col.setTransform ("0 0 0 0 0 1 0");
						%col.setActionThread (root, 0);
					}
					else
					{
						%obj.mountObject (%col, %mountNode);
						%col.setActionThread (root, 0);

						if ( %i == 0 )
						{
							if ( %obj.isHoleBot )
							{
								if ( %obj.controlOnMount )
								{
									%col.setControlObject (%obj);
								}
							}
							else if ( %obj.getControllingClient() == 0 )
							{
								%col.setControlObject (%obj);
							}

							if ( isObject(%obj.spawnBrick) )
							{
								%obj.lastControllingClient = %col;
							}
						}

						break;
					}
				}
			}
		}
	}
}
