exec ("./ntObjectName.cs");
exec ("./extendedInfo.cs");
exec ("./setWrenchData.cs");
exec ("./sendWrenchData.cs");
exec ("./sendSoundData.cs");
exec ("./createUINameTable.cs");
exec ("./vehicleSpawn/vehicleSpawn.cs");


datablock AudioProfile (wrenchHitSound)
{
	fileName = "~/data/sound/wrenchHit.wav";
	description = AudioClose3d;
	preload = 1;
};

datablock AudioProfile (wrenchMissSound)
{
	fileName = "~/data/sound/wrenchMiss.wav";
	description = AudioClose3d;
	preload = 1;
};

datablock ParticleData (wrenchSparkParticle)
{
	dragCoefficient = 4;
	gravityCoefficient = 1;
	inheritedVelFactor = 0.2;
	constantAcceleration = 0;
	lifetimeMS = 400;
	lifetimeVarianceMS = 300;
	textureName = "~/data/particles/chunk";
	useInvAlpha = 0;
	spinSpeed = 150;
	spinRandomMin = -150.0;
	spinRandomMax = 150;

	colors[0] = "0.2 0.1 0.0 1.0";
	colors[1] = "0.0 0.0 0.0 0.5";
	colors[2] = "0.0 0.0 0.0 0.0";

	sizes[0] = 0.15;
	sizes[1] = 0.05;
	sizes[2] = 0;

	times[0] = 0.1;
	times[1] = 0.5;
	times[2] = 1;

	useInvAlpha = 1;
};

datablock ParticleEmitterData (wrenchSparkEmitter)
{
	lifetimeMS = 50;
	ejectionPeriodMS = 3;
	periodVarianceMS = 0;
	ejectionVelocity = 5;
	velocityVariance = 3;
	ejectionOffset = 0.5;
	thetaMin = 0;
	thetaMax = 90;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = wrenchSparkParticle;
};

datablock ParticleData (wrenchExplosionParticle)
{
	dragCoefficient = 2;
	gravityCoefficient = 0.5;
	inheritedVelFactor = 0.2;
	constantAcceleration = 0;
	lifetimeMS = 500;
	lifetimeVarianceMS = 300;
	textureName = "~/data/particles/nut";
	spinSpeed = 50;
	spinRandomMin = 300;
	spinRandomMax = 650;

	colors[0] = "1.0 1.0 0.0 1.0";
	colors[1] = "1.0 0.0 0.0 0.0";

	sizes[0] = 0.3;
	sizes[1] = 0.3;

	useInvAlpha = 0;
};

datablock ParticleEmitterData (wrenchExplosionEmitter)
{
	lifetimeMS = 50;
	ejectionPeriodMS = 3;
	periodVarianceMS = 0;
	ejectionVelocity = 2;
	velocityVariance = 1;
	ejectionOffset = 0.2;
	thetaMin = 0;
	thetaMax = 180;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = wrenchExplosionParticle;
};

datablock ExplosionData (wrenchExplosion)
{
	lifetimeMS = 400;
	emitter[0] = wrenchExplosionEmitter;
	emitter[1] = wrenchSparkEmitter;
	faceViewer = 1;
	explosionScale = "1 1 1";
	shakeCamera = 0;
	camShakeFreq = "20.0 22.0 20.0";
	camShakeAmp = "1.0 1.0 1.0";
	camShakeDuration = 0.5;
	camShakeRadius = 10;
	lightStartRadius = 2;
	lightEndRadius = 1;
	lightStartColor = "1.0 0.5 0.0";
	lightEndColor = "0 0 0";
};

datablock ProjectileData (wrenchProjectile)
{
	directDamage = 10;
	radiusDamage = 0;
	damageRadius = 0;
	Explosion = wrenchExplosion;
	muzzleVelocity = 50;
	velInheritFactor = 1;
	armingDelay = 0;
	lifetime = 0;
	fadeDelay = 70;
	bounceElasticity = 0;
	bounceFriction = 0;
	isBallistic = 0;
	gravityMod = 0;
	explodeOnDeath = 1;
	hasLight = 0;
	lightRadius = 3;
	lightColor = "0 0 0.5";
};

datablock ItemData (WrenchItem)
{
	category = "Tools";
	className = "Weapon";
	shapeFile = "~/data/shapes/wrench.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = 1;
	uiName = "wrench";
	iconName = "base/client/ui/itemIcons/wrench";
	doColorShift = 1;
	colorShiftColor = "0.471 0.471 0.471 1.000";
	image = WrenchImage;
	canDrop = 1;
};

datablock ShapeBaseImageData (WrenchImage)
{
	shapeFile = "~/data/shapes/wrench.dts";
	emap = 1;
	mountPoint = 0;
	offset = "0 0 0";
	eyeOffset = "0.7 1.2 -0.15";
	correctMuzzleVector = 0;
	className = "WeaponImage";
	Item = WrenchItem;
	ammo = " ";
	Projectile = wrenchProjectile;
	projectileType = Projectile;
	melee = 1;
	doRetraction = 0;
	armReady = 1;
	showBricks = 1;
	doColorShift = 1;
	colorShiftColor = "0.471 0.471 0.471 1.000";

	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0;
	stateTransitionOnTimeout[0] = "Ready";

	stateName[1] = "Ready";
	stateTransitionOnTriggerDown[1] = "PreFire";
	stateAllowImageChange[1] = 1;

	stateName[2] = "PreFire";
	stateScript[2] = "onPreFire";
	stateAllowImageChange[2] = 1;
	stateTimeoutValue[2] = 0.01;
	stateTransitionOnTimeout[2] = "Fire";

	stateName[3] = "Fire";
	stateTransitionOnTimeout[3] = "CheckFire";
	stateTimeoutValue[3] = 0.5;
	stateFire[3] = 1;
	stateAllowImageChange[3] = 1;
	stateSequence[3] = "Fire";
	stateScript[3] = "onFire";
	stateWaitForTimeout[3] = 1;
	stateSequence[3] = "Fire";

	stateName[4] = "CheckFire";
	stateTransitionOnTriggerUp[4] = "StopFire";

	stateName[5] = "StopFire";
	stateTransitionOnTimeout[5] = "Ready";
	stateTimeoutValue[5] = 0.01;
	stateAllowImageChange[5] = 1;
	stateWaitForTimeout[5] = 1;
	stateSequence[5] = "StopFire";
	stateScript[5] = "onStopFire";
};

function WrenchImage::onPreFire ( %this, %obj, %slot )
{
	%obj.playThread (2, wrench);
}

function WrenchImage::onStopFire ( %this, %obj, %slot )
{
	%obj.playThread (2, root);
}

function WrenchImage::onFire ( %this, %player, %slot )
{
	%start = %player.getEyePoint();
	%vec = VectorScale ( %player.getMuzzleVector(%slot),  10 * getWord(%player.getScale(),  2) );
	%end = VectorAdd (%start, %vec);

	%mask = $TypeMasks::StaticObjectType | $TypeMasks::FxBrickAlwaysObjectType;

	%raycast = containerRayCast (%start, %end, %mask);

	if ( !%raycast )
	{
		return;
	}


	%hitObj = getWord (%raycast, 0);
	%hitPos = getWords (%raycast, 1, 3);
	%hitNormal = getWords (%raycast, 4, 6);

	%projectilePos = VectorSub ( %hitPos, VectorScale ( %player.getEyeVector(), 0.25 ) );

	%p = new Projectile()
	{
		dataBlock = wrenchProjectile;

		initialVelocity = "0 0 0";
		initialPosition = %projectilePos;

		sourceObject = %player;
		sourceSlot = %slot;

		client = %player.client;
	};
	%p.setScale ( %player.getScale() );
	MissionCleanup.add (%p);

	%this.onHitObject (%player, %slot, %hitObj, %hitPos, %hitNormal);
}

function WrenchImage::onHitObject ( %this, %player, %slot, %hitObj, %hitPos, %hitNormal )
{
	%client = %player.client;

	if ( %hitObj.getType() & $TypeMasks::FxBrickAlwaysObjectType )
	{
		%adminOverride = false;

		if ( getTrustLevel(%player, %hitObj) < $TrustLevel::Wrench )
		{
			if ( %client.isAdmin )
			{
				%adminOverride = true;
			}
			else
			{
				%client.sendTrustFailureMessage ( %hitObj.getGroup() );
				ServerPlay3D (wrenchMissSound, %hitPos);

				return;
			}
		}

		if ( !$Server::LAN )
		{
			if ( isObject(%client.miniGame)  &&  isObject(%brickGroup.client)  &&  isObject(%brickGroup.client.miniGame) )
			{
				if ( %brickGroup.client.miniGame != %client.miniGame )
				{
					commandToClient (%client, 'centerPrint', %brickGroup.name  @ " is not in the minigame.", 1);
					ServerPlay3D (wrenchMissSound, %hitPos);

					return;
				}
			}
			else if ( isObject(%brickGroup.client)  &&  isObject(%brickGroup.client.miniGame) )
			{
				if ( %brickGroup.client.miniGame.UseAllPlayersBricks  ||  %brickGroup.client.miniGame.owner == %brickGroup.client )
				{
					commandToClient (%client, 'centerPrint', %brickGroup.name  @ "\'s bricks are in a minigame right now.", 1);
					ServerPlay3D (wrenchMissSound, %hitPos);

					return;
				}
			}
		}


		if ( %adminOverride )
		{
			%client.wrenchBrick = "";
			%client.adminWrenchBrick = %hitObj;
		}
		else
		{
			%client.wrenchBrick = %hitObj;
			%client.adminWrenchBrick = "";
		}


		if ( %client.brickGroup == %hitObj.getGroup() )
		{
			%allowNamedTargets = true;
		}
		else
		{
			%allowNamedTargets = false;
		}

		if ( isObject(%hitObj.client) )
		{
			%lanHeading = %hitObj.client.getPlayerName();
		}


		%netHeading = %hitObj.getGroup().name  @ " - (BL_ID: " @  %hitObj.getGroup().bl_id  @ ")";

		if ( %hitObj.getDataBlock().specialBrickType $= "Sound" )
		{
			if ( $Server::LAN )
			{
				commandToClient (%client, 'openWrenchSoundDlg', %lanHeading, %allowNamedTargets, 
					%adminOverride, $Server::WrenchEventsAdminOnly);
			}
			else
			{
				commandToClient (%client, 'openWrenchSoundDlg', %netHeading, %allowNamedTargets, 
					%adminOverride, $Server::WrenchEventsAdminOnly);
			}

			%hitObj.sendWrenchSoundData (%client);
		}
		else if ( %hitObj.getDataBlock().specialBrickType $= "VehicleSpawn" )
		{
			if ( $Server::LAN )
			{
				commandToClient (%client, 'openWrenchVehicleSpawnDlg', %lanHeading, %allowNamedTargets, 
					%adminOverride, $Server::WrenchEventsAdminOnly);
			}
			else
			{
				commandToClient (%client, 'openWrenchVehicleSpawnDlg', %netHeading, %allowNamedTargets, 
					%adminOverride, $Server::WrenchEventsAdminOnly);
			}

			%hitObj.sendWrenchVehicleSpawnData (%client);
		}
		else if ( %hitObj.getDataBlock().specialBrickType $= "BotSpawn" )
		{
			if ( $Server::LAN )
			{
				commandToClient (%client, 'openWrenchBotSpawnDlg', %lanHeading, %allowNamedTargets, 
					%adminOverride, $Server::WrenchEventsAdminOnly);
			}
			else
			{
				commandToClient (%client, 'openWrenchBotSpawnDlg', %netHeading, %allowNamedTargets, 
					%adminOverride, $Server::WrenchEventsAdminOnly);
			}

			%hitObj.sendWrenchBotSpawnData (%client);
		}
		else
		{
			if ( $Server::LAN )
			{
				commandToClient (%client, 'openWrenchDlg', %lanHeading, %allowNamedTargets, 
					%adminOverride, $Server::WrenchEventsAdminOnly);
			}
			else
			{
				commandToClient (%client, 'openWrenchDlg', %netHeading, %allowNamedTargets, 
					%adminOverride, $Server::WrenchEventsAdminOnly);
			}

			%hitObj.sendWrenchData (%client);
		}

		ServerPlay3D (wrenchHitSound, %hitPos);
		return;
	}
	else
	{
		ServerPlay3D (wrenchMissSound, %hitPos);
	}
}
