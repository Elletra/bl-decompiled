datablock AudioProfile (ArmorMoveBubblesSound)
{
	fileName = "base/data/sound/underWater1.wav";
	description = AudioCloseLooping3d;
	preload = 1;
};

datablock ParticleData (PlayerBubbleParticle)
{
	dragCoefficient = 0;
	gravityCoefficient = 1;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 1200;
	lifetimeVarianceMS = 400;
	useInvAlpha = 0;

	textureName = "base/data/particles/bubble";

	colors[0] = "0.7 0.8 1.0 0.8";
	colors[1] = "0.7 0.8 1.0 0.8";
	colors[2] = "0.7 0.8 1.0 0.0";

	sizes[0] = 0.2;
	sizes[1] = 0.2;
	sizes[2] = 0.2;

	times[0] = 0;
	times[1] = 0.5;
	times[2] = 1;
};

datablock ParticleEmitterData (PlayerBubbleEmitter)
{
	ejectionPeriodMS = 1;
	periodVarianceMS = 0;
	ejectionVelocity = 7;
	ejectionOffset = 0.4;
	velocityVariance = 3;
	thetaMin = 0;
	thetaMax = 30;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;

	particles = PlayerBubbleParticle;

	useEmitterColors = 1;
	useInvAlpha = 1;

	uiName = "Player Bubbles";
};
