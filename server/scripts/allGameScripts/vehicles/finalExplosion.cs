datablock ParticleData (vehicleFinalExplosionParticle)
{
	dragCoefficient = 1;
	windCoefficient = 0;
	gravityCoefficient = -0.5;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 1900;
	lifetimeVarianceMS = 1000;
	spinSpeed = 10;
	spinRandomMin = -50.0;
	spinRandomMax = 50;
	useInvAlpha = 1;
	animateTexture = 0;
	
	textureName = "base/data/particles/cloud";

	colors[0] = "0.0 0.0 0.0 0.5";
	colors[1] = "0.0 0.0 0.0 1.0";
	colors[2] = "0.0 0.0 0.0 0.0";

	sizes[0] = 5;
	sizes[1] = 10;
	sizes[2] = 5;

	times[0] = 0;
	times[1] = 0.1;
	times[2] = 1;
};

datablock ParticleEmitterData (vehicleFinalExplosionEmitter)
{
	ejectionPeriodMS = 7;
	periodVarianceMS = 0;
	lifetimeMS = 21;
	ejectionVelocity = 8;
	velocityVariance = 1;
	ejectionOffset = 0;
	thetaMin = 10;
	thetaMax = 40;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;

	particles = vehicleFinalExplosionParticle;
	emitterNode = TwentiethEmitterNode;

	uiName = "Vehicle Final Explosion 1";
};

datablock ParticleData (vehicleFinalExplosionParticle2)
{
	dragCoefficient = 3;
	windCoefficient = 0;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 1000;
	lifetimeVarianceMS = 500;
	spinSpeed = 10;
	spinRandomMin = -500.0;
	spinRandomMax = 500;
	useInvAlpha = 0;
	animateTexture = 0;

	textureName = "base/data/particles/cloud";

	colors[0] = "1.0 0.5 0.0 1.0";
	colors[1] = "1.0 0.0 0.0 0.0";

	sizes[0] = 1.5;
	sizes[1] = 2.5;

	times[0] = 0;
	times[1] = 1;
};

datablock ParticleEmitterData (vehicleFinalExplosionEmitter2)
{
	ejectionPeriodMS = 1;
	periodVarianceMS = 0;
	lifetimeMS = 15;
	ejectionVelocity = 30;
	velocityVariance = 5;
	ejectionOffset = 0;
	thetaMin = 85;
	thetaMax = 90;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;

	particles = vehicleFinalExplosionParticle2;
	emitterNode = TenthEmitterNode;

	uiName = "Vehicle Final Explosion 2";
};

datablock ParticleData (vehicleFinalExplosionParticle3)
{
	dragCoefficient = 13;
	windCoefficient = 0;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 100;
	lifetimeVarianceMS = 50;
	spinSpeed = 10;
	spinRandomMin = -500.0;
	spinRandomMax = 500;
	useInvAlpha = 0;
	animateTexture = 0;

	textureName = "base/data/particles/star1";

	colors[0] = "1.0 0.5 0.0 1.0";
	colors[1] = "1.0 0.0 0.0 0.0";

	sizes[0] = 15;
	sizes[1] = 0.5;

	times[0] = 0;
	times[1] = 1;
};

datablock ParticleEmitterData (vehicleFinalExplosionEmitter3)
{
	ejectionPeriodMS = 1;
	periodVarianceMS = 0;
	lifetimeMS = 15;
	ejectionVelocity = 30;
	velocityVariance = 5;
	ejectionOffset = 0;
	thetaMin = 85;
	thetaMax = 90;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;

	particles = vehicleFinalExplosionParticle3;
	emitterNode = TenthEmitterNode;

	uiName = "Vehicle Final Explosion 3";
};

datablock ExplosionData (vehicleFinalExplosion)
{
	lifetimeMS = 150;
	soundProfile = vehicleExplosionSound;
	emitter[0] = vehicleFinalExplosionEmitter3;
	emitter[1] = vehicleFinalExplosionEmitter2;
	particleEmitter = vehicleFinalExplosionEmitter;
	particleDensity = 20;
	particleRadius = 1;
	faceViewer = 1;
	explosionScale = "1 1 1";
	shakeCamera = 1;
	camShakeFreq = "7.0 8.0 7.0";
	camShakeAmp = "10.0 10.0 10.0";
	camShakeDuration = 0.75;
	camShakeRadius = 15;
	lightStartRadius = 0;
	lightEndRadius = 20;
	lightStartColor = "0.45 0.3 0.1";
	lightEndColor = "0 0 0";
	impulseRadius = 15;
	impulseForce = 1000;
	impulseVertical = 2000;
	radiusDamage = 30;
	damageRadius = 8;
	playerBurnTime = 5000;
};

datablock ProjectileData (vehicleFinalExplosionProjectile)
{
	directDamage = 0;
	radiusDamage = 0;
	damageRadius = 0;
	Explosion = vehicleFinalExplosion;
	DirectDamageType = $DamageType::VehicleExplosion;
	RadiusDamageType = $DamageType::VehicleExplosion;
	explodeOnDeath = 1;
	armingDelay = 0;
	lifetime = 0;

	uiName = "Vehicle Final Explosion";
};


function Vehicle::finalExplosion ( %obj )
{
	%data = %obj.getDataBlock();

	while ( %obj.getMountedObjectCount() > 0 )
	{
		%obj.getMountedObject(0).unmount();
	}

	%pos = VectorAdd (%obj.getPosition(), "0 0" SPC %data.finalExplosionOffset);

	if ( isObject(%obj.lastDamageClient) )
	{
		%client = %obj.lastDamageClient;
	}
	else
	{
		%client = %obj.spawnBrick.getGroup().client;
	}

	if ( %data.finalExplosionScale $= "" )
	{
		%data.finalExplosionScale = 1;
	}

	%data.finalExplosionScale = mClampF (%data.finalExplosionScale, 0.2, 5);
	%finalScale = getWord (%obj.getScale(), 2) * %data.finalExplosionScale;

	if ( isObject(%data.finalExplosionProjectile) )
	{
		%projectileData = %data.finalExplosionProjectile;
	}
	else
	{
		%projectileData = vehicleFinalExplosionProjectile;
	}

	%p = new Projectile()
	{
		dataBlock = %projectileData;

		initialPosition = %pos;

		sourceClient = %client;
		sourceObject = %obj.spawnBrick;
	};

	%p.setScale (%finalScale SPC %finalScale SPC %finalScale);
	%p.upVector = %obj.getUpVector();
	%p.client = %client;

	MissionCleanup.add (%p);
	%obj.delete();
}
