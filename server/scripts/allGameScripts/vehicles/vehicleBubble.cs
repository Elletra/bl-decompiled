datablock ParticleData (vehicleBubbleParticle)
{
	dragCoefficient = 0;
	gravityCoefficient = -0.5;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 400;
	lifetimeVarianceMS = 100;
	useInvAlpha = 0;

	textureName = "base/data/particles/cloud";

	colors[0] = "0.7 0.8 1.0 0.4";
	colors[1] = "0.7 0.8 1.0 0.4";
	colors[2] = "0.7 0.8 1.0 0.0";

	sizes[0] = 0.1;
	sizes[1] = 0.3;
	sizes[2] = 0.3;

	times[0] = 0;
	times[1] = 0.5;
	times[2] = 1;
};

datablock ParticleEmitterData (vehicleBubbleEmitter)
{
	ejectionPeriodMS = 1;
	periodVarianceMS = 0;
	ejectionVelocity = 2;
	ejectionOffset = 1.5;
	velocityVariance = 0.5;
	thetaMin = 0;
	thetaMax = 80;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvances = 0;
	
	particles = vehicleBubbleParticle;
	emitterNode = FifthEmitterNode;

	uiName = "Vehicle Bubbles";
};
