datablock ParticleData (AdminWandExplosionParticle)
{
	dragCoefficient = 14;
	gravityCoefficient = 0;
	inheritedVelFactor = 0.2;
	constantAcceleration = 0;
	lifetimeMS = 500;
	lifetimeVarianceMS = 300;
	spinSpeed = 0;
	spinRandomMin = -150.0;
	spinRandomMax = 150;
	textureName = "~/data/particles/cloud";

	colors[0] = "0.9 0.9 0.0 0.9";
	colors[1] = "0.9 0.4 0.0 0.9";
	colors[2] = "0.9 0.0 0.0 0.0";

	sizes[0] = 0;
	sizes[1] = 0.7;
	sizes[2] = 0.2;

	times[0] = 0;
	times[1] = 0.2;
	times[2] = 1;
};

datablock ParticleEmitterData (AdminWandExplosionEmitter)
{
	lifetimeMS = 50;
	ejectionPeriodMS = 1;
	periodVarianceMS = 0;
	ejectionVelocity = 15;
	velocityVariance = 4;
	ejectionOffset = 0.05;
	thetaMin = 0;
	thetaMax = 120;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = "AdminWandExplosionParticle";
};

datablock ExplosionData (AdminWandExplosion)
{
	lifetimeMS = 500;
	emitter[0] = AdminWandExplosionEmitter;
	soundProfile = wandHitSound;
	faceViewer = 1;
	explosionScale = "1 1 1";
	shakeCamera = 1;
	camShakeFreq = "2.0 2.0 2.0";
	camShakeAmp = "1.0 1.0 1.0";
	camShakeDuration = 0.5;
	camShakeRadius = 10;
	lightStartRadius = 3;
	lightEndRadius = 1;
	lightStartColor = "00.9 0.6 0.0";
	lightEndColor = "0 0 0";
	radiusDamage = 0;
	damageRadius = 0;
	impulseRadius = 0;
	impulseForce = 0;
};

datablock ProjectileData (AdminWandProjectile) 
{
	directDamage = 0;
	impactImpulse = 0;
	verticalImpulse = 0;
	Explosion = AdminWandExplosion;
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

datablock ParticleData (AdminWandParticleA)
{
	textureName = "~/data/particles/cloud";
	dragCoefficient = 0;
	gravityCoefficient = -0.5;
	inheritedVelFactor = 0;
	windCoefficient = 0;
	constantAcceleration = 0;
	lifetimeMS = 600;
	lifetimeVarianceMS = 100;
	spinSpeed = 0;
	spinRandomMin = -90.0;
	spinRandomMax = 90;
	useInvAlpha = 0;

	colors[0] = "0.9 0.4 0.0 1.0";
	colors[1] = "0.9 0.0 0.0 0.0";

	sizes[0] = 0.3;
	sizes[1] = 0.1;

	times[0] = 0;
	times[1] = 1;
};

datablock ParticleEmitterData (AdminWandEmitterA)
{
	ejectionPeriodMS = 4;
	periodVarianceMS = 2;
	ejectionVelocity = 0;
	ejectionOffset = 0.1;
	velocityVariance = 0;
	thetaMin = 0;
	thetaMax = 180;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = AdminWandParticleA;
};

datablock ParticleData (AdminWandParticleB)
{
	textureName = "~/data/particles/cloud";
	dragCoefficient = 0;
	gravityCoefficient = -0.5;
	inheritedVelFactor = 0;
	windCoefficient = 0;
	constantAcceleration = 0;
	lifetimeMS = 600;
	lifetimeVarianceMS = 100;
	spinSpeed = 0;
	spinRandomMin = -90.0;
	spinRandomMax = 90;
	useInvAlpha = 0;

	colors[0] = "1.0 1.0 0.0 1.0";
	colors[1] = "0.9 0.4 0.0 0.0";

	sizes[0] = 0.3;
	sizes[1] = 0.1;

	times[0] = 0;
	times[1] = 1;
};

datablock ParticleEmitterData (AdminWandEmitterB)
{
	ejectionPeriodMS = 4;
	periodVarianceMS = 2;
	ejectionVelocity = 0;
	ejectionOffset = 0.1;
	velocityVariance = 0;
	thetaMin = 0;
	thetaMax = 180;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = AdminWandParticleB;
};

datablock ShapeBaseImageData (AdminWandImage)
{
	shapeFile = "~/data/shapes/wand.dts";
	emap = 0;
	mountPoint = 0;
	eyeOffset = "0.7 1.2 -0.25";
	correctMuzzleVector = 0;
	className = "WeaponImage";
	Item = "";
	ammo = " ";
	Projectile = AdminWandProjectile;
	projectileType = Projectile;
	melee = 1;
	doRetraction = 0;
	armReady = 1;
	showBricks = 1;
	doColorShift = 1;
	colorShiftColor = "1 0 0 1";

	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0;
	stateTransitionOnTimeout[0] = "Ready";

	stateName[1] = "Ready";
	stateTransitionOnTriggerDown[1] = "PreFire";
	stateAllowImageChange[1] = 1;
	stateEmitter[1] = AdminWandEmitterA;
	stateEmitterTime[1] = 0.15;
	stateEmitterNode[1] = "muzzlePoint";
	stateTransitionOnTimeout[1] = "Ready";
	stateTimoutValue[1] = 0.1;

	stateName[2] = "PreFire";
	stateScript[2] = "onPreFire";
	stateAllowImageChange[2] = 0;
	stateTimeoutValue[2] = 0.1;
	stateTransitionOnTimeout[2] = "Fire";

	stateName[3] = "Fire";
	stateTransitionOnTimeout[3] = "CheckFire";
	stateTimeoutValue[3] = 0.2;
	stateFire[3] = 1;
	stateAllowImageChange[3] = 1;
	stateSequence[3] = "Fire";
	stateScript[3] = "onFire";
	stateWaitForTimeout[3] = 1;
	stateEmitter[3] = AdminWandEmitterB;
	stateEmitterTime[3] = 0.4;
	stateEmitterNode[3] = "blah";

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

function AdminWandImage::onPreFire ( %this, %obj, %slot )
{
	%obj.playThread (2, armattack);
}

function AdminWandImage::onStopFire ( %this, %obj, %slot )
{
	%obj.playThread(2, root);
}

function AdminWandImage::onFire ( %this, %player, %slot )
{
	%start = %player.getEyePoint();

	%muzzleVec  = %player.getMuzzleVector (%slot);
	%muzzleVecZ = getWord (%muzzleVec, 2);

	if ( %muzzleVecZ < -0.9 )
	{
		%range = 5.5;
	}
	else
	{
		%range = 5;
	}

	%range = 500;

	%vec = VectorScale ( %muzzleVec, %range * getWord ( %player.getScale(), 2 ) );
	%end = VectorAdd (%start, %vec);

	%mask = $TypeMasks::StaticObjectType | $TypeMasks::FxBrickAlwaysObjectType | 
	        $TypeMasks::PlayerObjectType | $TypeMasks::VehicleObjectType | $TypeMasks::StaticShapeObjectType;

	if ( %player.isMounted() )
	{
		%exempt = %player.getObjectMount();
	}
	else
	{
		%exempt = %player;
	}

	%raycast = containerRayCast (%start, %end, %mask, %exempt);

	if ( !%raycast )
	{
		return;
	}


	%hitObj = getWord (%raycast, 0);
	%hitPos = getWords (%raycast, 1, 3);
	%hitNormal = getWords (%raycast, 4, 6);

	%projectilePos = VectorSub ( %hitPos, VectorScale ( %player.getEyeVector(), 0.25 ) );

	%p = new Projectile ()
	{
		dataBlock = AdminWandProjectile;

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

function AdminWandImage::onHitObject ( %this, %player, %slot, %hitObj, %hitPos, %hitNormal )
{
	%client = %player.client;

	if ( !isObject(%client) )
	{
		return;
	}

	if ( !%client.isAdmin )
	{
		return;
	}

	if ( %hitObj.getType() & $TypeMasks::FxBrickAlwaysObjectType )
	{
		%brickGroup = %hitObj.getGroup();

		if ( %brickGroup )
		{
			if ( %brickGroup.bl_id == 888888  &&  !%client.destroyPublicBricks )
			{
				return;
			}
		}

		$CurrBrickKiller = %client;
		%hitObj.killBrick();
	}
	else if ( %hitObj.getType() & $TypeMasks::PlayerObjectType )
	{
		%vel = VectorNormalize ( %player.getEyeVector() );
		%vel = VectorAdd (%vel, "0 0 1");
		%vel = VectorNormalize (%vel);
		%vel = VectorScale (%vel, 20);

		%hitObj.setVelocity (%vel);
	}
}


function serverCmdMagicWand ( %client )
{
	if ( !%client.isAdmin )
	{
		return;
	}


	%player = %client.Player;

	if ( !isObject(%player) )
	{
		return;
	}


	%player.updateArm (AdminWandImage);
	%player.mountImage (AdminWandImage, 0);

	if ( %client.destroyPublicBricks )
	{
		%client.bottomPrint ("\c6Destroy public bricks is \c0ON \c6-  use /dpb to toggle", 4);
	}
	else
	{
		%client.bottomPrint ("\c6Destroy public bricks is \c2OFF \c6-  use /dpb to toggle", 4);
	}
}
