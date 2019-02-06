datablock AudioProfile (wandHitSound)
{
	fileName = "~/data/sound/wandHit.wav";
	description = AudioClosest3d;
	preload = 1;
};

datablock ParticleData (wandExplosionParticle)
{
	dragCoefficient = 10;
	gravityCoefficient = 0;
	inheritedVelFactor = 0.2;
	constantAcceleration = 0;
	lifetimeMS = 500;
	lifetimeVarianceMS = 300;
	spinSpeed = 0;
	spinRandomMin = -150.0;
	spinRandomMax = 150;
	textureName = "~/data/particles/star1";

	colors[0] = "0.0 0.7 0.9 0.9";
	colors[1] = "0.0 0.7 0.9 0.9";
	colors[1] = "0.0 0.0 0.9 0.0";

	sizes[0] = 0;
	sizes[1] = 0.7;
	sizes[2] = 0.2;

	times[0] = 0;
	times[1] = 0.2;
	times[2] = 1;
};

datablock ParticleEmitterData (wandExplosionEmitter)
{
	lifetimeMS = 50;
	ejectionPeriodMS = 1;
	periodVarianceMS = 0;
	ejectionVelocity = 15;
	velocityVariance = 4;
	ejectionOffset = 0.25;
	thetaMin = 0;
	thetaMax = 120;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = "wandExplosionParticle";
};

datablock ExplosionData (wandExplosion)
{
	lifetimeMS = 500;
	emitter[0] = wandExplosionEmitter;
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
	lightStartColor = "00.0 0.6 0.9";
	lightEndColor = "0 0 0";
};

datablock ProjectileData (wandProjectile)
{
	directDamage = 0;
	radiusDamage = 0;
	damageRadius = 0.5;
	Explosion = wandExplosion;
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

datablock ItemData (WandItem)
{
	category = "Weapon";
	className = "Tool";
	shapeFile = "~/data/shapes/wand.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = 1;
	uiName = "Wand";
	iconName = "base/client/ui/itemIcons/wand";
	image = WandImage;
	canDrop = 1;
};

function WandItem::onUse ( %this, %player, %invPosition )
{
	%client = %player.client;

	if ( isObject(%client) )
	{
		%mg = %client.miniGame;

		if ( isObject(%mg) )
		{
			if ( !%mg.EnableWand )
			{
				return;
			}
		}
	}

	Weapon::onUse (%this, %player, %invPosition);
}

datablock ParticleData (WandParticleA)
{
	textureName = "~/data/particles/star1";
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

	colors[0] = "0.0   0   1.0 1.0";
	colors[1] = "0.0 0.0 0.6 0.0";

	sizes[0] = 0.3;
	sizes[1] = 0.1;

	times[0] = 0;
	times[1] = 1;
};

datablock ParticleEmitterData (WandEmitterA)
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
	particles = WandParticleA;
};

datablock ParticleData (WandParticleB)
{
	textureName = "~/data/particles/star1";
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

	colors[0] = "0.3   1   1.0 1.0";
	colors[1] = "0.0 0.0 0.6 0.0";

	sizes[0] = 0.3;
	sizes[1] = 0.1;

	times[0] = 0;
	times[1] = 1;
};

datablock ParticleEmitterData (WandEmitterB)
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
	particles = WandParticleB;
};

datablock ShapeBaseImageData (WandImage)
{
	shapeFile = "~/data/shapes/wand.dts";
	emap = 0;
	mountPoint = 0;
	eyeOffset = "0.7 1.2 -0.25";
	correctMuzzleVector = 0;
	className = "WeaponImage";
	Item = WandItem;
	ammo = " ";
	Projectile = wandProjectile;
	projectileType = Projectile;
	melee = 1;
	doRetraction = 0;
	armReady = 1;
	showBricks = 1;
	doColorShift = 1;
	colorShiftColor = "1 1 1 1";

	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0;
	stateTransitionOnTimeout[0] = "Ready";
	stateName[1] = "Ready";
	stateTransitionOnTriggerDown[1] = "PreFire";
	stateAllowImageChange[1] = 1;
	stateEmitter[1] = WandEmitterA;
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
	stateEmitter[3] = WandEmitterB;
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

function WandImage::onPreFire ( %this, %obj, %slot )
{
	%obj.playThread (2, armattack);
}

function WandImage::onStopFire ( %this, %obj, %slot )
{
	%obj.playThread (2, root);
}

function WandImage::onFire ( %this, %player, %slot )
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

	%mask = $TypeMasks::StaticObjectType | $TypeMasks::FxBrickAlwaysObjectType | $TypeMasks::PlayerObjectType | 
			$TypeMasks::VehicleObjectType | $TypeMasks::StaticShapeObjectType;
	
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

	%p = new Projectile()
	{
		dataBlock = wandProjectile;

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

function WandImage::onHitObject ( %this, %player, %slot, %hitObj, %hitPos, %hitNormal )
{
	%client = %player.client;

	if ( !isObject(%client) )
	{
		return;
	}


	if ( %hitObj.getType() & $TypeMasks::FxBrickAlwaysObjectType )
	{
		if ( getTrustLevel(%player, %hitObj) < $TrustLevel::Wand )
		{
			if ( %hitObj.stackBL_ID $= ""  ||  %hitObj.stackBL_ID != %client.getBLID() )
			{
				%client.sendTrustFailureMessage ( %hitObj.getGroup() );
				return;
			}
		}

		%hitObj.onToolBreak(%client);
		$CurrBrickKiller = %client;
		%hitObj.killBrick();
	}
	else if ( %hitObj.getType() & $TypeMasks::PlayerObjectType )
	{
		if ( miniGameCanDamage (%client, %hitObj) != 1 )
		{
			if ( miniGameCanDamage(%client, %hitObj) == 0 )
			{
				commandToClient (%client, 'centerPrint', %hitObj.client.getPlayerName()  @ " is in a different minigame.", 1);
				return;
			}
			else if ( getTrustLevel(%player, %hitObj) < $TrustLevel::Wand )
			{
				%client.sendTrustFailureMessage (%hitObj.client.brickGroup);
				return;
			}
		}

		%hitObj.setVelocity ("0 0 15");
	}
}


function serverCmdWand ( %client )
{
	%mg = %client.miniGame;

	if ( isObject(%mg) )
	{
		if ( !%mg.EnableWand )
		{
			return;
		}
	}


	%player = %client.Player;

	if ( !isObject(%player) )
	{
		return;
	}

	%player.updateArm (WandImage);
	%player.mountImage (WandImage, 0);
}
