datablock ParticleData (CameraParticleA)
{
	textureName = "~/data/particles/dot";
	
	dragCoefficient = 0;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	windCoefficient = 0;
	constantAcceleration = 0;
	lifetimeMS = 200;
	lifetimeVarianceMS = 0;
	spinSpeed = 0;
	spinRandomMin = -90.0;
	spinRandomMax = 90;
	useInvAlpha = 0;

	colors[0] = "0.6 0.0 0.0 0.0";
	colors[1] = "1   1   0.3 1.0";
	colors[2] = "0.6 0.0 0.0 0.0";

	sizes[0] = 1.5;
	sizes[1] = 1.5;
	sizes[2] = 0;

	times[0] = 0;
	times[1] = 0.5;
	times[2] = 1;
};

datablock ParticleEmitterData (CameraEmitterA)
{
	ejectionPeriodMS = 1;
	periodVarianceMS = 0;
	ejectionVelocity = 0;
	ejectionOffset = 0.1;
	velocityVariance = 0;
	thetaMin = 0;
	thetaMax = 180;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = CameraParticleA;
	doFalloff = 0;
	doDetail = 0;
	pointEmitterNode = FifthEmitterNode;
};

datablock ShapeBaseImageData (cameraImage)
{
	shapeFile = "~/data/shapes/empty.dts";
	emap = 0;
	mountPoint = 0;
	offset = "0 0 0";
	doRetraction = 0;
	firstPersonParticles = 0;

	stateName[0] = "Ready";
	stateTransitionOnTimeout[0] = "FireA";
	stateTimeoutValue[0] = 0.01;

	stateName[1] = "FireA";
	stateTransitionOnTimeout[1] = "FireB";
	stateWaitForTimeout[1] = True;
	stateTimeoutValue[1] = 0.05;
	stateEmitter[1] = CameraEmitterA;
	stateEmitterTime[1] = 0.05;

	stateName[2] = "FireB";
	stateTransitionOnTimeout[2] = "FireA";
	stateWaitForTimeout[2] = True;
	stateTimeoutValue[2] = 0.05;
	stateEmitter[2] = CameraEmitterA;
	stateEmitterTime[2] = 0.05;
};
