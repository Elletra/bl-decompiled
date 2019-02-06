datablock ParticleData (vehicleSplashParticle)
{
	dragCoefficient = 1;
	gravityCoefficient = 0.2;
	inheritedVelFactor = 0.2;
	constantAcceleration = -0.0;
	lifetimeMS = 600;
	lifetimeVarianceMS = 0;

	textureName = "base/data/particles/cloud";

	colors[0] = "0.7 0.8 1.0 1.0";
	colors[1] = "0.7 0.8 1.0 0.5";
	colors[2] = "0.7 0.8 1.0 0.0";

	sizes[0] = 0.5;
	sizes[1] = 0.5;
	sizes[2] = 0.5;

	times[0] = 0;
	times[1] = 0.5;
	times[2] = 1;
};

datablock ParticleEmitterData (vehicleSplashEmitter)
{
	ejectionPeriodMS = 1;
	periodVarianceMS = 0;
	ejectionVelocity = 3;
	velocityVariance = 1;
	ejectionOffset = 0;
	thetaMin = 60;
	thetaMax = 80;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvances = 0;
	orientParticles = 1;
	lifetimeMS = 100;

	particles = vehicleSplashParticle;
	emitterNode = TenthEmitterNode;
	
	uiName = "Vehicle Splash";
};

datablock ParticleData (vehicleSplashMist)
{
	dragCoefficient = 2;
	gravityCoefficient = -0.05;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 400;
	lifetimeVarianceMS = 100;
	useInvAlpha = 0;
	spinRandomMin = -90.0;
	spinRandomMax = 500;
	
	textureName = "base/data/particles/cloud";

	colors[0] = "0.7 0.8 1.0 1.0";
	colors[1] = "0.7 0.8 1.0 0.5";
	colors[2] = "0.7 0.8 1.0 0.0";

	sizes[0] = 2.5;
	sizes[1] = 2.5;
	sizes[2] = 5;

	times[0] = 0;
	times[1] = 0.5;
	times[2] = 1;
};

datablock ParticleEmitterData (vehicleSplashMistEmitter)
{
	ejectionPeriodMS = 5;
	periodVarianceMS = 0;
	ejectionVelocity = 3;
	velocityVariance = 2;
	ejectionOffset = 1;
	thetaMin = 85;
	thetaMax = 85;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvances = 0;
	lifetimeMS = 250;

	particles = vehicleSplashMist;
	emitterNode = FifthEmitterNode;

	uiName = "Vehicle Splash Mist";
};
