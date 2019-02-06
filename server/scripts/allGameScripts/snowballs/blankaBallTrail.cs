datablock ParticleData (blankaBallTrailParticle)
{
	dragCoefficient = 3;
	windCoefficient = 0;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 300;
	lifetimeVarianceMS = 0;
	spinSpeed = 10;
	spinRandomMin = -50.0;
	spinRandomMax = 50;
	useInvAlpha = 1;
	animateTexture = 0;
	textureName = "base/data/particles/chunk";

	colors[0] = "1 1 1 0.3";
	colors[1] = "1 1 1 0.2";
	colors[2] = "1 1 1 0.0";

	sizes[0] = 0.3;
	sizes[1] = 0.12;
	sizes[2] = 0.05;

	times[0] = 0;
	times[1] = 0.5;
	times[2] = 1;
};

datablock ParticleEmitterData (blankaBallTrailEmitter)
{
	ejectionPeriodMS = 25;
	periodVarianceMS = 10;
	ejectionVelocity = 2;
	velocityVariance = 2;
	ejectionOffset = 0;
	phiReferenceVel = 0;
	phiVariance = 360;
	thetaMin = 0;
	thetaMax = 90;
	particles = blankaBallTrailParticle;
	useEmitterColors = 1;
};
