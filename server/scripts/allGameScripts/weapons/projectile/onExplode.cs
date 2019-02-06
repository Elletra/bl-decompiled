// FIXME: this method is a fucking disaster -- pls fix

// Seriously, like, it doesn't work at all.  Don't use this for anything.


function ProjectileData::onExplode ( %this, %obj, %pos )
{
	warn ("ProjectileData::onExplode is broken and doesn't work!");  // FIXME: ADDED DEBUG LINE
	return;                                                          // FIXME: ADDED DEBUG LINE


	%totalExplodedBricks = 0;
	%client = %obj.client;

	if ( isObject(%obj.sourceObject)  &&  %obj.sourceObject.isBot )
	{
		%client = %obj.sourceObject;
	}

	if ( !isObject(%client) )
	{
		%client = %obj.sourceObject;
	}

	if ( !isObject(%client) )
	{
		return %totalExplodedBricks;
	}


	%clientBLID = getBL_IDFromObject (%obj);

	if ( isObject(%client.miniGame) )
	{
		if ( getSimTime() - %client.lastF8Time < 3000 )
		{
			return %totalExplodedBricks;
		}
	}


	%mg = %client.miniGame;

	if ( isObject(%mg) )
	{
		%respawnTime = %mg.BrickRespawnTime;
	}
	else
	{
		%respawnTime = mClampF ($Server::BrickRespawnTime, 1000, 60000);

		%explosion = %this.Explosion;
		%scale = getWord (%obj.getScale(), 2);
		%explodeBricks = 1;

		if ( isObject(%mg) )
		{
			if ( !%mg.BrickDamage)
			{
				%explodeBricks = 0;
			}
		}

		if ( %this.brickExplosionRadius > 0  &&  %explodeBricks )
		{
			%count = 0;

			%mask = $TypeMasks::FxBrickAlwaysObjectType;

			%radius = %this.brickExplosionRadius * %scale;
			%maxVolume = %this.brickExplosionMaxVolume * %scale;
			%maxFloatingVolume = %this.brickExplosionMaxVolumeFloating * %scale;

			%explosionForce = %this.brickExplosionForce;
			%explosionRadius = %this.brickExplosionRadius * %scale;

			$CurrBrickKiller = %client;

			initContainerRadiusSearch (%pos, %radius, %mask);

			// FIXME: I have no idea how accurately this while loop was decompiled
			// The variable seems to get reassigned inside this while loop for another container search
			// inside /another/ while loop???

			// I don't think this is right...

			while ( ( %searchObj = containerSearchNext() )  !=  0 )
			{
				if ( %searchObj.canExplode(%maxVolume, %maxFloatingVolume)  &&  $Server::LAN )
				{
					if ( isObject(%mg) )
					{
						if ( !%mg.BrickDamage )
						{
							// :thinking:
						}
						else
						{
							// who knows?
						}
					}
					else if ( %searchObj.getGroup().bl_id == 888888 )
					{
						if ( isObject(%mg) )
						{
							if ( %mg.getId() != $DefaultMiniGame.getId() )
							{
								%respawnTime = mClampF (%mg.BrickRespawnTime, 1000, $Server::BrickRespawnTime);
							}
						}
					}
					else
					{
						if ( isObject(%mg)  &&  miniGameCanDamage(%client, %searchObj) == 1  &&  isObject(%obj.sourceObject) )
						{
							if ( %searchObj.getGroup().bl_id == %clientBLID  ||  %searchObj.getGroup() == %obj.sourceObject.getGroup() )
							{
								if ( %searchObj.getGroup().bl_id == %clientBLID )
								{
									if ( %searchObj.numEvents > 0 )
									{
										%oldQuota = getCurrentQuotaObject();
										clearCurrentQuotaObject();

										%searchObj.schedule (10, onBlownUp, %client, %obj.sourceObject);

										if ( isObject(%oldQuota) )
										{
											setCurrentQuotaObject (%oldQuota);
										}
									}

									if ( %count == 0 )
									{
										startNewBrickExplosion (%pos, %explosionForce, %explosionRadius, %respawnTime);
									}

									addBrickToExplosion (%searchObj);

									%totalExplodedBricks++;
									%count++;

									if ( %count > 100 )
									{
										sendBrickExplosion();
										%count = 0;
									}

									// FIXME: ???
									// %searchObj = containerSearchNext();
								}

								if ( %count > 0 )
								{
									sendBrickExplosion();
								}
							}

							if ( %explosion.damageRadius <= 0  ||  %explosion.radiusDamage <= 0  &&  
								(%explosion.impulse  &&  %explosion.impulseVertical <= 0  ||  %explosion.impulseRadius <= 0) )
							{
								return %totalExplodedBricks;
							}


							%damageRadius  = %explosion.damageRadius  * %scale;
							%impulseRadius = %explosion.impulseRadius * %scale;
							%radiusDamage  = %explosion.radiusDamage  * %scale;

							%impulseForce    = %explosion.impulseForce    * %scale;
							%impulseVertical = %explosion.impulseVertical * %scale;

							if ( %damageRadius > %impulseRadius )
							{
								%radius = %explosion.damageRadius;
							}
							else
							{
								%radius = %explosion.impulseRadius;
								%mask = $TypeMasks::PlayerObjectType | $TypeMasks::VehicleObjectType | $TypeMasks::CorpseObjectType;
								
								initContainerRadiusSearch (%pos, %radius, %mask);

								// FIXME: See what I mean?  %searchObj gets reassigned down here
								// Like what the fuck

								while ( ( %searchObj = containerSearchNext() ) != 0)
								{
									if ( isObject(%mg) )
									{
										if ( !%mg.SelfDamage )
										{
											if ( %searchObj.client == %client )
											{
												// God this whole function is a mess
											}
											else
											{
												// I have no idea what's going on lol
											}
										}

										%searchObj = getWord (%searchObj, 0);

										if ( %searchObj.getType() & $TypeMasks::PlayerObjectType | $TypeMasks::CorpseObjectType )
										{
											%searchPos = %searchObj.getHackPosition();
										}
										else
										{
											%searchPos = %searchObj.getWorldBoxCenter();
											%dist = VectorDist (%searchPos, %pos);

											%damageDistFactor = 1 - %dist / %damageRadius * %dist / %damageRadius;
											%impulseDistFactor = 1 - %dist / %impulseRadius * %dist / %impulseRadius;

											%yourStuffOverride = 0;

											if ( !isObject(%client.miniGame) )
											{
												if ( isObject(%searchObj.spawnBrick) )
												{
													if ( %searchObj.spawnBrick.getGroup().bl_id == %clientBLID  ||  
														 %searchObj.spawnBrick.getGroup().bl_id == 888888 )
													{
														%yourStuffOverride = 1;
													}
												}
											}

											if ( miniGameCanDamage(%client, %searchObj) == 1  ||  %yourStuffOverride )
											{
												%this.radiusDamage (%obj, %searchObj, %damageDistFactor, %pos, %radiusDamage);
												%this.radiusImpulse (%obj, %searchObj, %impulseDistFactor, %pos, %impulseForce, %impulseVertical);
											}
										}

										return %totalExplodedBricks;
									}
								}
							}
						}
					}
				}
			}
		}
	}
}
