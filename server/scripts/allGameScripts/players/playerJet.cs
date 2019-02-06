datablock ParticleData (playerJetParticle)
{
	dragCoefficient = 3;
	windCoefficient = 0;
	gravityCoefficient = 0;
	inheritedVelFactor = 1;
	constantAcceleration = 0;
	lifetimeMS = 130;
	lifetimeVarianceMS = 10;
	spinSpeed = 500;
	spinRandomMin = -150.0;
	spinRandomMax = 150;
	useInvAlpha = 0;
	animateTexture = 0;
	
	textureName = "base/data/particles/cloud";

	colors[0] = "0 0 1 1.0";
	colors[1] = "1 0.5 0 1.0";
	colors[2] = "1 0 0 0.000";

	sizes[0] = 0.6;
	sizes[1] = 0.4;
	sizes[2] = 0.7;

	times[0] = 0;
	times[1] = 0.1;
	times[2] = 1;
};

datablock ParticleEmitterData (playerJetEmitter)
{
	ejectionPeriodMS = 6;
	periodVarianceMS = 0;
	ejectionVelocity = 5;
	velocityVariance = 0;
	ejectionOffset = 0;
	thetaMin = 0;
	thetaMax = 0;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = playerJetParticle;
};

datablock ParticleData (playerJetGroundParticle)
{
	dragCoefficient = 3;
	windCoefficient = 0;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 500;
	lifetimeVarianceMS = 100;
	spinSpeed = 500;
	spinRandomMin = -150.0;
	spinRandomMax = 150;
	useInvAlpha = 1;
	animateTexture = 0;

	textureName = "base/data/particles/cloud";

	colors[0] = "1 1 1 0.00";
	colors[1] = "0.5 0.5 0.5 0.250";
	colors[2] = "0.8 0.8 1.0 0.000";

	sizes[0] = 0.4;
	sizes[1] = 0.4;
	sizes[2] = 1.5;

	times[0] = 0;
	times[1] = 0.3;
	times[2] = 1;
};

datablock ParticleEmitterData (playerJetGroundEmitter)
{
	ejectionPeriodMS = 14;
	periodVarianceMS = 0;
	ejectionVelocity = 4;
	velocityVariance = 0;
	ejectionOffset = 0;
	thetaMin = 85;
	thetaMax = 90;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = playerJetGroundParticle;
};
