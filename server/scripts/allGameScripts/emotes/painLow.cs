datablock ParticleData (PainLowParticle)
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

	colors[0] = "0.0 1.0 0 0";
	colors[1] = "0.0 1 0 0.8";
	colors[2] = "0.0 0.0 0 0";

	sizes[0] = 1;
	sizes[1] = 1;
	sizes[2] = 1;

	times[0] = 0;
	times[1] = 0.2;
	times[2] = 1;
};

datablock ParticleEmitterData (PainLowEmitter)
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

	particles = PainLowParticle;

	uiName = "Pain - Low";
};

datablock ShapeBaseImageData (PainLowImage)
{
	shapeFile = "base/data/shapes/empty.dts";
	emap = 0;
	mountPoint = $HeadSlot;

	stateName[0] = "Ready";
	stateTransitionOnTimeout[0] = "FireA";
	stateTimeoutValue[0] = 0.01;

	stateName[1] = "FireA";
	stateTransitionOnTimeout[1] = "Done";
	stateWaitForTimeout[1] = True;
	stateEmitter[1] = PainLowEmitter;
	stateEmitterTime[1] = 0.35;
	stateTimeoutValue[1] = 0.35;

	stateName[2] = "Done";
	stateScript[2] = "onDone";
};


function PainLowImage::onDone ( %this, %obj, %slot )
{
	%obj.unmountImage (%slot);
}
