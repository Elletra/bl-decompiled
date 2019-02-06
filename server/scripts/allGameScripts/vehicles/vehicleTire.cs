datablock ParticleData (VehicleTireParticle)
{
	textureName = "base/data/particles/chunk";

	dragCoefficient = 0;
	gravityCoefficient = 2;
	windCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 800;
	lifetimeVarianceMS = 300;

	colors[0] = "0 0 0 1";
	colors[1] = "0 0 0 0";

	sizes[0] = 0.25;
	sizes[1] = 0;

	useInvAlpha = 1;
};

datablock ParticleEmitterData (VehicleTireEmitter)
{
	ejectionPeriodMS = 3;
	periodVarianceMS = 0;
	ejectionVelocity = 5;
	velocityVariance = 3;
	ejectionOffset = 0.1;
	thetaMin = 10;
	thetaMax = 30;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvances = 0;

	particles = VehicleTireParticle;
	
	uiName = "Vehicle Tire";
};
