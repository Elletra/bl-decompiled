function WheeledVehicleData::onCollision ( %this, %obj, %col, %vec, %speed )
{
	if ( %obj.getDamageState() $= "Dead" )
	{
		return;
	}

	if ( %col.getDamagePercent() >= 1.0 )
	{
		return;
	}

	%runOver = false;

	if ( isObject(%obj.client) )
	{
		if ( %col.client == %obj.client )
		{
			return;
		}
	}

	%canUse = false;

	if ( isObject(%obj.spawnBrick) )
	{
		%vehicleOwner = findClientByBL_ID (%obj.spawnBrick.getGroup().bl_id);
	}
	else
	{
		%vehicleOwner = 0;
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

	if ( !miniGameCanUse(%col, %obj) )
	{
		%canUse = false;
	}

	if ( miniGameCanDamage(%col, %obj) == 1 )
	{
		%canDamage = true;
	}
	else
	{
		%canDamage = false;
	}

	%minSpeed = mClampF (%this.minRunOverSpeed, $Game::DefaultMinRunOverSpeed, 999);

	if ( !isObject ( %obj.getControllingObject() ) )
	{
		%minSpeed += 2;
	}

	%relativeSpeed = VectorLen ( VectorSub ( %obj.getVelocity(),  %col.getVelocity() ) );

	if ( %col.getDataBlock().canRide  &&  %this.rideAble  &&  %this.nummountpoints > 0 )
	{
		if ( getSimTime() - %col.lastMountTime > $Game::MinMountTime )
		{
			%colZpos = getWord (%col.getPosition(), 2);
			%objZpos = getWord (%obj.getPosition(), 2);

			if ( %colZpos > %objZpos + 0.2 )
			{
				if ( %canUse )
				{
					for ( %i = 0;  %i < %this.nummountpoints;  %i++ )
					{
						%blockingObj = %obj.getMountNodeObject (%i);

						if ( isObject(%blockingObj) )
						{
							if ( %blockingObj.getDataBlock().rideAble )
							{
								if ( !%blockingObj.getMountedObject(0) )
								{
									%blockingObj.mountObject (%col, 0);

									if ( %blockingObj.getControllingClient() == 0 )
									{
										%col.setControlObject (%blockingObj);
									}
								}
							}
						}
						else
						{
							%obj.mountObject (%col, %i);

							if ( %i == 0 )
							{
								if ( %obj.getControllingClient() == 0 )
								{
									%col.setControlObject (%obj);
								}
							}

							break;
						}
					}
				}
				else
				{
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
					%runOver = true;
				}
			}
			else
			{
				%runOver = true;
			}
		}
	}
	else
	{
		%runOver = true;
	}


	if ( %canDamage )
	{
		if ( %runOver  &&  %col.getType() & $TypeMasks::PlayerObjectType )
		{
			%vehicleSpeed = VectorLen ( %obj.getVelocity() );

			if ( %vehicleSpeed > %minSpeed )
			{
				%damageScale = %this.runOverDamageScale;

				if ( %damageScale $= "" )
				{
					%damageScale = $Game::DefaultRunOverDamageScale;
				}

				%damageType = %this.damageType;

				if ( %damageType $= "" )
				{
					%damageType = $DamageType::Vehicle;
				}

				%damageAmt = %vehicleSpeed * %damageScale;

				%col.Damage (%obj, %pos, %damageAmt, %damageType);
			}
		}

		%pushScale = %this.runOverPushScale;

		if ( %pushScale $= "" )
		{
			%pushScale = $Game::DefaultRunOverPushScale;
		}

		%pushVec = %obj.getVelocity();
		%pushVec = VectorScale (%pushVec, %pushScale);

		%col.setVelocity (%pushVec);
	}
}
