datablock AudioProfile (blankaBallExplosionSound)
{
	fileName = "base/data/sound/northImpact.wav";
	description = AudioClose3d;
	preload = 0;
};

datablock ParticleData (blankaBallExplosionParticle)
{
	dragCoefficient = 3;
	windCoefficient = 0;
	gravityCoefficient = 0.5;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 900;
	lifetimeVarianceMS = 300;
	spinSpeed = 10;
	spinRandomMin = -50.0;
	spinRandomMax = 50;
	useInvAlpha = 1;
	animateTexture = 0;

	textureName = "base/data/particles/cloud";

	colors[0] = "1 1 1 0.9";
	colors[1] = "1 1 1 0.0";

	sizes[0] = 1;
	sizes[1] = 2;

	times[0] = 0;
	times[1] = 1;
};

datablock ParticleEmitterData (blankaBallExplosionEmitter)
{
	ejectionPeriodMS = 7;
	periodVarianceMS = 0;
	lifetimeMS = 21;
	ejectionVelocity = 1;
	velocityVariance = 0;
	ejectionOffset = 0;
	thetaMin = 0;
	thetaMax = 90;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = blankaBallExplosionParticle;
	emitterNode = TenthEmitterNode;
};

datablock ParticleData (blankaBallExplosionParticle2)
{
	dragCoefficient = 0.1;
	windCoefficient = 0;
	gravityCoefficient = 0.5;
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

	colors[0] = "1 1 1 1.0";
	colors[1] = "1 1 1 0.0";

	sizes[0] = 0.5;
	sizes[1] = 0.5;

	times[0] = 0;
	times[1] = 1;
};

datablock ParticleEmitterData (blankaBallExplosionEmitter2)
{
	ejectionPeriodMS = 1;
	periodVarianceMS = 0;
	lifetimeMS = 7;
	ejectionVelocity = 2;
	velocityVariance = 0;
	ejectionOffset = 0;
	thetaMin = 0;
	thetaMax = 90;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	useEmitterColors = 1;
	particles = blankaBallExplosionParticle2;
	emitterNode = HalfEmitterNode;
};

datablock ExplosionData (blankaBallExplosion)
{
	lifetimeMS = 150;
	soundProfile = "";
	emitter[0] = blankaBallExplosionEmitter;
	emitter[1] = blankaBallExplosionEmitter2;
	faceViewer = 1;
	explosionScale = "1 1 1";
	shakeCamera = 1;
	camShakeFreq = "0 0 0";
	camShakeAmp = "0 0 0";
	camShakeDuration = 0;
	camShakeRadius = 0;
	lightStartRadius = 0;
	lightEndRadius = 0;
	lightStartColor = "0.45 0.3 0.1";
	lightEndColor = "0 0 0";
	impulseRadius = 0;
	impulseForce = 2000;
	radiusDamage = 0;
	damageRadius = 0;
};
