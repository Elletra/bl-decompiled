datablock ParticleData (VehicleBurnParticle)
{
	textureName = "base/data/particles/cloud";
	
	dragCoefficient = 0;
	gravityCoefficient = -1.0;
	inheritedVelFactor = 0;
	windCoefficient = 0;
	constantAcceleration = 3;
	lifetimeMS = 1200;
	lifetimeVarianceMS = 100;
	spinSpeed = 0;
	spinRandomMin = -90.0;
	spinRandomMax = 90;
	useInvAlpha = 0;

	colors[0] = "1   1   0.3 0.0";
	colors[1] = "1   1   0.3 1.0";
	colors[2] = "0.6 0.0 0.0 0.0";

	sizes[0] = 0;
	sizes[1] = 2;
	sizes[2] = 1;

	times[0] = 0;
	times[1] = 0.2;
	times[2] = 1;
};

datablock ParticleEmitterData (VehicleBurnEmitter)
{
	ejectionPeriodMS = 14;
	periodVarianceMS = 4;
	ejectionVelocity = 0;
	ejectionOffset = 1;
	velocityVariance = 0;
	thetaMin = 30;
	thetaMax = 90;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;

	particles = VehicleBurnParticle;

	uiName = "Vehicle Fire";
};
