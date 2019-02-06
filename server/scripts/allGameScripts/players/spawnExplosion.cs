datablock AudioProfile (spawnExplosionSound)
{
	fileName = "~/data/sound/spawn.wav";
	description = AudioClose3d;
	preload = 0;
};

datablock ParticleData (spawnExplosionParticle1)
{
	dragCoefficient = 0;
	windCoefficient = 0;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 400;
	lifetimeVarianceMS = 399;
	spinSpeed = 10;
	spinRandomMin = -50.0;
	spinRandomMax = 50;
	useInvAlpha = 1;
	animateTexture = 0;
	
	textureName = "~/data/particles/cloud";

	colors[0] = "1 0 1 0.5";
	colors[1] = "0 1 0 1 0.5";
	colors[2] = "1 0 1 0.0";

	sizes[0] = 0;
	sizes[1] = 4;
	sizes[2] = 4;

	times[0] = 0;
	times[1] = 0.1;
	times[2] = 1;
};

datablock ParticleEmitterData (spawnExplosionEmitter1)
{
	ejectionPeriodMS = 4;
	periodVarianceMS = 0;
	lifetimeMS = 21;
	ejectionVelocity = 5;
	velocityVariance = 1;
	ejectionOffset = 0;
	thetaMin = 0;
	thetaMax = 120;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;

	particles = spawnExplosionParticle1;
};

datablock ParticleData (spawnExplosionParticle2)
{
	dragCoefficient = 0.1;
	windCoefficient = 0;
	gravityCoefficient = 2;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 1000;
	lifetimeVarianceMS = 400;
	spinSpeed = 10;
	spinRandomMin = -50.0;
	spinRandomMax = 50;
	useInvAlpha = 0;
	animateTexture = 0;

	textureName = "~/data/particles/star1";

	colors[0] = "1.0 0.0 0.0 1.0";
	colors[1] = "0.0 0.0 0.0 0.0";

	sizes[0] = 0.25;
	sizes[1] = 0.25;

	times[0] = 0;
	times[1] = 1;
};

datablock ParticleEmitterData (spawnExplosionEmitter2)
{
	ejectionPeriodMS = 1;
	periodVarianceMS = 0;
	lifetimeMS = 7;
	ejectionVelocity = 0;
	velocityVariance = 0;
	ejectionOffset = 2;
	thetaMin = 0;
	thetaMax = 180;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;

	particles = spawnExplosionParticle2;
};

datablock ExplosionData (spawnExplosion)
{
	lifetimeMS = 150;
	soundProfile = spawnExplosionSound;

	emitter[0] = spawnExplosionEmitter1;
	emitter[1] = spawnExplosionEmitter2;

	faceViewer = 1;
	explosionScale = "1 1 1";

	shakeCamera = 0;
	camShakeFreq = "7.0 8.0 7.0";
	camShakeAmp = "1.0 1.0 1.0";
	camShakeDuration = 0.5;
	camShakeRadius = 15;

	hasLight = 0;
	lightStartRadius = 4;
	lightEndRadius = 3;
	lightStartColor = "0.45 0.3 0.1";
	lightEndColor = "0 0 0";
};

datablock ProjectileData (spawnProjectile)
{
	directDamage = 0;
	radiusDamage = 0;
	damageRadius = 0;

	Explosion = spawnExplosion;

	muzzleVelocity = 50;
	velInheritFactor = 1;
	explodeOnDeath = 1;
	armingDelay = 1000;
	lifetime = 10;
	fadeDelay = 10;
	bounceElasticity = 0;
	bounceFriction = 0;
	isBallistic = 1;
	gravityMod = 0.5;

	hasLight = 0;
	lightRadius = 3;
	lightColor = "0 0 0.5";

	uiName = "Player Spawn";
};
