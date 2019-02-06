datablock ParticleData (PainMidParticle)
{
	dragCoefficient = 2;
	gravityCoefficient = 1;
	inheritedVelFactor = 0;
	windCoefficient = 0;
	constantAcceleration = 0;
	lifetimeMS = 1500;
	lifetimeVarianceMS = 100;
	useInvAlpha = 1;
	spinSpeed = 10;
	spinRandomMin = -150.0;
	spinRandomMax = 150;
	textureName = "~/data/particles/pain";

	colors[0] = "1.0 1.0 0 0.0";
	colors[1] = "1.0 1.0 0 0.8";
	colors[2] = "0.5 0.0 0 0.0";

	sizes[0] = 1.8;
	sizes[1] = 1.5;
	sizes[2] = 1.5;

	times[0] = 0;
	times[1] = 0.2;
	times[2] = 1;
};

datablock ParticleEmitterData (PainMidEmitter)
{
	ejectionPeriodMS = 65;
	periodVarianceMS = 0;
	ejectionVelocity = 3;
	ejectionOffset = 0.5;
	velocityVariance = 0;
	thetaMin = 0;
	thetaMax = 180;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;

	particles = PainMidParticle;

	uiName = "Pain - Mid";
};

datablock ShapeBaseImageData (PainMidImage : PainLowImage)
{
	stateEmitter[1] = PainMidEmitter;
	stateEmitterTime[1] = 0.35;
	stateTimeoutValue[1] = 0.35;
};


function PainMidImage::onDone ( %this, %obj, %slot )
{
	%obj.unmountImage (%slot);
}
