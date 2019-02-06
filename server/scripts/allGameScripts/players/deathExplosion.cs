datablock AudioProfile (deathExplosionSound)
{
	fileName = "~/data/sound/bodyRemove.wav";
	description = AudioClose3d;
	preload = 0;
};

datablock ParticleData (deathExplosionParticle1)
{
	dragCoefficient = 3;
	windCoefficient = 0;
	gravityCoefficient = -1.5;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 900;
	lifetimeVarianceMS = 800;
	spinSpeed = 10;
	spinRandomMin = -50.0;
	spinRandomMax = 50;
	useInvAlpha = 1;
	animateTexture = 0;

	textureName = "~/data/particles/cloud";

	colors[0] = "1 1 1 0.5";
	colors[1] = "1 1 1 0.5";
	colors[2] = "1 1 1 0.0";

	sizes[0] = 2;
	sizes[1] = 6;
	sizes[2] = 4;

	times[0] = 0;
	times[1] = 0.1;
	times[2] = 1;
};

datablock ParticleEmitterData (deathExplosionEmitter1)
{
	ejectionPeriodMS = 3;
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

	particles = deathExplosionParticle1;
};

datablock ParticleData (deathExplosionParticle2)
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
	useInvAlpha = 0;
	animateTexture = 0;

	textureName = "~/data/particles/chunk";

	colors[0] = "1.0 1.0 0.0 1.0";
	colors[1] = "0.0 0.0 0.0 0.0";

	sizes[0] = 0.25;
	sizes[1] = 0.25;

	times[0] = 0;
	times[1] = 1;
};

datablock ParticleEmitterData (deathExplosionEmitter2)
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

	particles = deathExplosionParticle2;
};

datablock ExplosionData (deathExplosion)
{
	lifetimeMS = 150;
	soundProfile = deathExplosionSound;

	emitter[0] = deathExplosionEmitter1;
	emitter[1] = deathExplosionEmitter2;

	faceViewer = 1;
	explosionScale = "1 1 1";
	shakeCamera = 0;
	camShakeFreq = "7.0 8.0 7.0";
	camShakeAmp = "1.0 1.0 1.0";
	camShakeDuration = 0.5;
	camShakeRadius = 15;
	lightStartRadius = 4;
	lightEndRadius = 3;
	lightStartColor = "0.45 0.3 0.1";
	lightEndColor = "0 0 0";
};

datablock ProjectileData (deathProjectile)
{
	projectileShapeName = "~/data/shapes/empty.dts";

	directDamage = 0;
	radiusDamage = 0;
	damageRadius = 0;
	
	Explosion = deathExplosion;
	muzzleVelocity = 50;
	velInheritFactor = 1;
	explodeOnDeath = 1;
	armingDelay = 0;
	lifetime = 10;
	fadeDelay = 10;
	bounceElasticity = 0;
	bounceFriction = 0;
	isBallistic = 1;
	gravityMod = 0.5;

	hasLight = 0;
	lightRadius = 3;
	lightColor = "0 0 0.5";

	uiName = "Player Death";
};
