datablock AudioProfile (hammerHitSound)
{
	fileName = "~/data/sound/hammerHit.wav";
	description = AudioClosest3d;
	preload = 1;
};

datablock ParticleData (hammerSparkParticle)
{
	dragCoefficient = 4;
	gravityCoefficient = 1;
	inheritedVelFactor = 0.2;
	constantAcceleration = 0;
	lifetimeMS = 400;
	lifetimeVarianceMS = 300;
	textureName = "~/data/particles/star1";
	useInvAlpha = 0;
	spinSpeed = 150;
	spinRandomMin = -150.0;
	spinRandomMax = 150;

	colors[0] = "1.0 1.0 0.0 0.0";
	colors[1] = "1.0 1.0 0.0 0.5";
	colors[2] = "1.0 1.0 0.0 0.0";

	sizes[0] = 0.15;
	sizes[1] = 0.05;
	sizes[2] = 0;

	times[0] = 0.1;
	times[1] = 0.5;
	times[2] = 1;

	useInvAlpha = 0;
};

datablock ParticleEmitterData (hammerSparkEmitter)
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
	particles = hammerSparkParticle;
};

datablock ParticleData (hammerExplosionParticle)
{
	dragCoefficient = 10;
	gravityCoefficient = -0.15;
	inheritedVelFactor = 0.2;
	constantAcceleration = 0;
	lifetimeMS = 500;
	lifetimeVarianceMS = 300;
	textureName = "~/data/particles/cloud";
	spinSpeed = 50;
	spinRandomMin = -50.0;
	spinRandomMax = 50;

	colors[0] = "1.0 1.0 1.0 0.25";
	colors[1] = "0.0 0.0 0.0 0.0";

	sizes[0] = 0.5;
	sizes[1] = 1;

	useInvAlpha = 0;
};

datablock ParticleEmitterData (hammerExplosionEmitter)
{
	lifetimeMS = 50;
	ejectionPeriodMS = 3;
	periodVarianceMS = 0;
	ejectionVelocity = 10;
	velocityVariance = 1;
	ejectionOffset = 0;
	thetaMin = 80;
	thetaMax = 85;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = hammerExplosionParticle;
};

datablock ExplosionData (hammerExplosion)
{
	lifetimeMS = 400;
	emitter[0] = hammerExplosionEmitter;
	emitter[1] = hammerSparkEmitter;
	faceViewer = 1;
	explosionScale = "1 1 1";
	shakeCamera = 1;
	camShakeFreq = "20.0 22.0 20.0";
	camShakeAmp = "0.50 0.50 0.50";
	camShakeDuration = 0.5;
	camShakeRadius = 10;
	lightStartRadius = 2;
	lightEndRadius = 1;
	lightStartColor = "0.6 0.6 0.0";
	lightEndColor = "0 0 0";
};

AddDamageType ("HammerDirect", '<bitmap:base/client/ui/ci/hammer> %1', '%2 <bitmap:base/client/ui/ci/hammer> %1', 0, 1);

datablock ProjectileData (hammerProjectile)
{
	directDamage = 10;
	radiusDamage = 0;
	damageRadius = 0;
	Explosion = hammerExplosion;
	DirectDamageType = $DamageType::HammerDirect;
	RadiusDamageType = $DamageType::HammerDirect;
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

datablock ItemData (hammerItem)
{
	category = "Tools";
	className = "Weapon";
	shapeFile = "~/data/shapes/Hammer.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = 1;
	uiName = "Hammer ";
	iconName = "base/client/ui/itemIcons/Hammer";
	doColorShift = 1;
	colorShiftColor = "0.200 0.200 0.200 1.000";
	image = hammerImage;
	canDrop = 1;
};

datablock ShapeBaseImageData (hammerImage)
{
	shapeFile = "~/data/shapes/Hammer.dts";
	emap = 1;
	mountPoint = 0;
	offset = "0 0 0";
	eyeOffset = "0.7 1.2 -0.15";
	correctMuzzleVector = 0;
	className = "WeaponImage";
	Item = hammerItem;
	ammo = " ";
	Projectile = hammerProjectile;
	projectileType = Projectile;
	melee = 1;
	doRetraction = 0;
	armReady = 1;
	showBricks = 1;
	doColorShift = 1;
	colorShiftColor = "0.200 0.200 0.200 1.000";

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
	stateTimeoutValue[3] = 0.2;
	stateFire[3] = 1;
	stateAllowImageChange[3] = 1;
	stateSequence[3] = "Fire";
	stateScript[3] = "onFire";
	stateWaitForTimeout[3] = 1;
	stateSequence[3] = "Fire";

	stateName[4] = "CheckFire";
	stateTransitionOnTriggerUp[4] = "StopFire";
	stateTransitionOnTriggerDown[4] = "Fire";
	stateName[5] = "StopFire";
	stateTransitionOnTimeout[5] = "Ready";
	stateTimeoutValue[5] = 0.2;
	stateAllowImageChange[5] = 1;
	stateWaitForTimeout[5] = 1;
	stateSequence[5] = "StopFire";
	stateScript[5] = "onStopFire";
};

function hammerImage::onPreFire ( %this, %obj, %slot )
{
	%obj.playThread (2, armattack);
}

function hammerImage::onStopFire ( %this, %obj, %slot )
{
	%obj.playThread (2, root);
}

function hammerImage::onFire ( %this, %player, %slot )
{
	%start = %player.getEyePoint();

	%muzzleVec = %player.getMuzzleVector (%slot);
	%muzzleVecZ = getWord (%muzzleVec, 2);

	if ( %muzzleVecZ < -0.9 )
	{
		%range = 5.5;
	}
	else
	{
		%range = 5;
	}


	%vec = VectorScale ( %muzzleVec, %range * getWord(%player.getScale(), 2) );
	%end = VectorAdd (%start, %vec);

	%mask = $TypeMasks::FxBrickAlwaysObjectType | $TypeMasks::PlayerObjectType | $TypeMasks::VehicleObjectType | 
			$TypeMasks::StaticShapeObjectType | $TypeMasks::StaticObjectType;
	
	if ( %player.isMounted() )
	{
		%raycast = containerRayCast ( %start, %end, %mask, %player, %player.getObjectMount() );
	}
	else
	{
		%raycast = containerRayCast (%start, %end, %mask, %player);
	}

	if ( !%raycast )
	{
		return;
	}


	%hitObj = getWord (%raycast, 0);
	%hitPos = getWords (%raycast, 1, 3);
	%hitNormal = getWords (%raycast, 4, 6);

	%projectilePos = VectorSub ( %hitPos, VectorScale(%player.getEyeVector(), 0.25) );

	%p = new Projectile()
	{
		dataBlock = hammerProjectile;

		initialVelocity = %hitNormal;
		initialPosition = %projectilePos;

		sourceObject = %player;
		sourceSlot = %slot;

		client = %player.client;
	};
	%p.setScale ( %player.getScale() );
	MissionCleanup.add (%p);

	%this.onHitObject (%player, %slot, %hitObj, %hitPos, %hitNormal);
}

function hammerImage::onHitObject ( %this, %player, %slot, %hitObj, %hitPos, %hitNormal )
{
	%client = %player.client;

	ServerPlay3D (hammerHitSound, %hitPos);

	if ( !isObject(%client) )
	{
		return;
	}


	if ( %hitObj.getType() & $TypeMasks::FxBrickAlwaysObjectType )
	{
		if ( !isObject(%client) )
		{
			return;
		}

		if ( !%hitObj.willCauseChainKill() )
		{
			if ( getTrustLevel(%player, %hitObj) < $TrustLevel::Hammer )
			{
				if ( %hitObj.stackBL_ID $= ""  ||  %hitObj.stackBL_ID != %client.getBLID() )
				{
					%client.sendTrustFailureMessage ( %hitObj.getGroup() );
					return;
				}
			}

			%hitObj.onToolBreak (%client);
			$CurrBrickKiller = %client;
			%hitObj.killBrick();
		}
	}
	else if ( %hitObj.getClassName() $= "Player" )
	{
		if ( miniGameCanDamage(%client, %hitObj) == 1 )
		{
			%hitObj.Damage (%player, %hitPos, hammerProjectile.directDamage, $DamageType::HammerDirect);
		}
	}
	else
	{
		if ( %hitObj.getClassName() $= "WheeledVehicle"  ||  %hitObj.getClassName() $= "HoverVehicle"  ||  
			 %hitObj.getClassName() $= "FlyingVehicle" )
		{
			%mount = %player;

			for ( %i = 0;  %i < 100;  %i++ )
			{
				if ( %mount == %hitObj )
				{
					return;
				}

				if ( %mount.isMounted() )
				{
					%mount = %mount.getObjectMount();
				}
			}


			%doFlip = false;

			if ( isObject(%hitObj.spawnBrick) )
			{
				%vehicleOwner = findClientByBL_ID ( %hitObj.spawnBrick.getGroup().bl_id );
			}
			else
			{
				%vehicleOwner = 0;
			}

			if ( isObject(%vehicleOwner) )
			{
				if ( getTrustLevel(%player, %hitObj) >= $TrustLevel::VehicleTurnover )
				{
					%doFlip = true;
				}
			}
			else
			{
				%doFlip = true;
			}

			if ( miniGameCanDamage(%player, %hitObj) == 1 )
			{
				%doFlip = true;
			}

			if ( miniGameCanDamage(%player, %hitObj) == 0 )
			{
				%doFlip = false;
			}

			if ( %doFlip )
			{
				%impulse = VectorNormalize (%vec);
				%impulse = VectorAdd (%impulse, "0 0 1");
				%impulse = VectorNormalize (%impulse);

				%force = %hitObj.getDataBlock().mass * 5;

				%impulse = VectorScale(%impulse, %force);
				%hitObj.applyImpulse(%hitPos, %impulse);
			}
		}
	}
}


function serverCmdUseHammer ( %client )
{
	if ( %client.isAdmin )
	{
		if ( ClientGroup.getCount() > 1  &&  !%client.canHammer )
		{
			messageClient (%client, '', '\c3You cannot use the Hammer yet');
			return;
		}
	}


	%player = %client.player;

	%mountPoint = %this.image.mountPoint;
	%mountedImage = %player.getMountedImage (%mountPoint);

	if ( %mountedImage )
	{
		if ( %mountedImage == hammerImage.getId() )
		{
			%player.unmountImage (%mountPoint);

			messageClient (%player.client, 'MsgHilightInv', '', -1);
			%player.currWeaponSlot = -1;
		}
		else
		{
			%player.mountImage (hammerImage, %mountPoint);

			messageClient(%player.client, 'MsgHilightInv', '', -1);
			%player.currWeaponSlot = -1;
		}
	}
	else
	{
		%player.mountImage (hammerImage, %mountPoint);

		messageClient(%player.client, 'MsgHilightInv', '', -1);
		%player.currWeaponSlot = -1;
	}
}
