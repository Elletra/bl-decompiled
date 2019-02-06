datablock ParticleData (PlayerFoamParticle)
{
	dragCoefficient = 0;
	gravityCoefficient = 1;
	inheritedVelFactor = 0.5;
	constantAcceleration = 0;
	lifetimeMS = 400;
	lifetimeVarianceMS = 100;
	useInvAlpha = 0;
	spinRandomMin = -90.0;
	spinRandomMax = 500;
	
	textureName = "base/data/particles/bubble";

	colors[0] = "0.7 0.8 1.0 1.0";
	colors[1] = "0.7 0.8 1.0 0.85";
	colors[2] = "0.7 0.8 1.0 0.00";

	sizes[0] = 0.1;
	sizes[1] = 0.1;
	sizes[2] = 0.1;

	times[0] = 0;
	times[1] = 0.5;
	times[2] = 1;
};

datablock ParticleEmitterData (PlayerFoamEmitter)
{
	ejectionPeriodMS = 10;
	periodVarianceMS = 0;
	ejectionVelocity = 3;
	velocityVariance = 1;
	ejectionOffset = 0.4;
	thetaMin = 0;
	thetaMax = 90;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;

	particles = PlayerFoamParticle;
	useEmitterColors = 1;

	uiName = "Player Foam";
};

datablock ParticleData (PlayerFoamDropletsParticle)
{
	dragCoefficient = 0;
	gravityCoefficient = 1;
	inheritedVelFactor = 0.5;
	constantAcceleration = -0.0;
	lifetimeMS = 500;
	lifetimeVarianceMS = 0;

	textureName = "base/data/particles/bubble";

	colors[0] = "0.7 0.8 1.0 1.0";
	colors[1] = "0.7 0.8 1.0 0.85";
	colors[2] = "0.7 0.8 1.0 0.0";

	sizes[0] = 0.15;
	sizes[1] = 0.15;
	sizes[2] = 0.15;

	times[0] = 0;
	times[1] = 0.8;
	times[2] = 1;
};

datablock ParticleEmitterData (PlayerFoamDropletsEmitter)
{
	ejectionPeriodMS = 7;
	periodVarianceMS = 0;
	ejectionVelocity = 2;
	velocityVariance = 1;
	ejectionOffset = 0.4;
	thetaMin = 40;
	thetaMax = 70;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	orientParticles = 0;
	particles = PlayerFoamDropletsParticle;
	useEmitterColors = 1;
};
