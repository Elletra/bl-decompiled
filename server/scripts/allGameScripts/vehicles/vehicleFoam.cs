datablock ParticleData (vehicleFoamParticle)
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

	colors[0] = "0.7 0.8 1.0 0.20";
	colors[1] = "0.7 0.8 1.0 0.20";
	colors[2] = "0.7 0.8 1.0 0.00";

	sizes[0] = 1.2;
	sizes[1] = 1.4;
	sizes[2] = 2.6;

	times[0] = 0;
	times[1] = 0.5;
	times[2] = 1;
};

datablock ParticleEmitterData (vehicleFoamEmitter)
{
	ejectionPeriodMS = 20;
	periodVarianceMS = 0;
	ejectionVelocity = 3;
	velocityVariance = 1;
	ejectionOffset = 0.75;
	thetaMin = 85;
	thetaMax = 85;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvances = 0;

	particles = vehicleFoamParticle;
	emitterNode = GenericEmitterNode;

	uiName = "Vehicle Foam";
};

datablock ParticleData (vehicleFoamDropletsParticle)
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

	sizes[0] = 0.8;
	sizes[1] = 0.3;
	sizes[2] = 0;

	times[0] = 0;
	times[1] = 0.5;
	times[2] = 1;
};

datablock ParticleEmitterData (vehicleFoamDropletsEmitter)
{
	ejectionPeriodMS = 7;
	periodVarianceMS = 0;
	ejectionVelocity = 2;
	velocityVariance = 1;
	ejectionOffset = 1;
	thetaMin = 60;
	thetaMax = 80;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvances = 0;
	orientParticles = 1;

	particles = vehicleFoamDropletsParticle;
	emitterNode = GenericEmitterNode;

	uiName = "Vehicle Foam Droplets";
};
