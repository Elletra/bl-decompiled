datablock AudioProfile (vehicleExplosionSound)
{
	fileName = "base/data/sound/vehicleExplosion.wav";
	description = AudioDefault3d;
	preload = 1;
};

datablock ParticleData (vehicleExplosionParticle)
{
	dragCoefficient = 3;
	windCoefficient = 0;
	gravityCoefficient = -0.5;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 1900;
	lifetimeVarianceMS = 300;
	spinSpeed = 10;
	spinRandomMin = -50.0;
	spinRandomMax = 50;
	useInvAlpha = 1;
	animateTexture = 0;

	textureName = "base/data/particles/cloud";

	colors[0] = "0.9 0.3 0.2 0.9";
	colors[1] = "0.0 0.0 0.0 0.0";

	sizes[0] = 4;
	sizes[1] = 10;

	times[0] = 0;
	times[1] = 1;
};

datablock ParticleEmitterData (vehicleExplosionEmitter)
{
	ejectionPeriodMS = 7;
	periodVarianceMS = 0;
	lifetimeMS = 21;
	ejectionVelocity = 8;
	velocityVariance = 1;
	ejectionOffset = 0;
	thetaMin = 0;
	thetaMax = 90;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;

	emitterNode = TenthEmitterNode;
	particles = vehicleExplosionParticle;

	uiName = "Vehicle Explosion";
};

datablock ParticleData (vehicleExplosionParticle2)
{
	dragCoefficient = 0.1;
	windCoefficient = 0;
	gravityCoefficient = 2;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 1000;
	lifetimeVarianceMS = 500;
	spinSpeed = 10;
	spinRandomMin = -50.0;
	spinRandomMax = 50;
	useInvAlpha = 1;
	animateTexture = 0;

	textureName = "base/data/particles/chunk";

	colors[0] = "1.0 1.0 0.0 1.0";
	colors[1] = "1.0 0.0 0.0 0.0";

	sizes[0] = 0.5;
	sizes[1] = 0.5;

	times[0] = 0;
	times[1] = 1;
};

datablock ParticleEmitterData (vehicleExplosionEmitter2)
{
	ejectionPeriodMS = 1;
	periodVarianceMS = 0;
	lifetimeMS = 7;
	ejectionVelocity = 15;
	velocityVariance = 5;
	ejectionOffset = 0;
	thetaMin = 0;
	thetaMax = 90;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;

	particles = vehicleExplosionParticle2;
	emitterNode = TenthEmitterNode;
	
	uiName = "Vehicle Explosion 2";
};

datablock ExplosionData (vehicleExplosion)
{
	lifetimeMS = 150;
	soundProfile = vehicleExplosionSound;
	emitter[0] = vehicleExplosionEmitter;
	emitter[1] = vehicleExplosionEmitter2;
	faceViewer = 1;
	explosionScale = "1 1 1";
	shakeCamera = 1;
	camShakeFreq = "7.0 8.0 7.0";
	camShakeAmp = "1.0 1.0 1.0";
	camShakeDuration = 0.5;
	camShakeRadius = 15;
	lightStartRadius = 14;
	lightEndRadius = 3;
	lightStartColor = "0.9 0.3 0.1";
	lightEndColor = "0 0 0";
	impulseRadius = 10;
	impulseForce = 500;
	radiusDamage = 30;
	damageRadius = 3.5;
};

datablock ProjectileData (vehicleExplosionProjectile)
{
	directDamage = 0;
	radiusDamage = 0;
	damageRadius = 0;

	Explosion = vehicleExplosion;

	DirectDamageType = $DamageType::VehicleExplosion;
	RadiusDamageType = $DamageType::VehicleExplosion;

	explodeOnDeath = 1;
	armingDelay = 0;
	lifetime = 0;

	uiName = "Vehicle Explosion";
};
