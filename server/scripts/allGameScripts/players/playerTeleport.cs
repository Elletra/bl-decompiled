datablock ParticleData (PlayerTeleportParticleA)
{
	textureName = "~/data/particles/dot";

	dragCoefficient = 0;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	windCoefficient = 0;
	constantAcceleration = 0;
	lifetimeMS = 100;
	lifetimeVarianceMS = 99;
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

datablock ParticleEmitterData (PlayerTeleportEmitterA)
{
	ejectionPeriodMS = 1;
	periodVarianceMS = 0;
	ejectionVelocity = 10;
	ejectionOffset = 0.1;
	velocityVariance = 0;
	thetaMin = 0;
	thetaMax = 180;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;

	particles = PlayerTeleportParticleA;

	doFalloff = 0;
	doDetail = 0;

	uiName = "Player Teleport A";
};

datablock ExplosionData (PlayerTeleportExplosion)
{
	lifetimeMS = 150;
	soundProfile = "";

	particleEmitter = "";
	particleDensity = 230;
	particleRadius = 2;

	emitter[0] = PlayerTeleportEmitterA;

	faceViewer = 1;
	explosionScale = "1 1 1";

	shakeCamera = 0;
	camShakeFreq = "7.0 8.0 7.0";
	camShakeAmp = "1.0 1.0 1.0";
	camShakeDuration = 0.5;
	camShakeRadius = 15;

	lightStartRadius = 0;
	lightEndRadius = 0;
	lightStartColor = "0.45 0.3 0.1";
	lightEndColor = "0 0 0";

	impulseRadius = 0;
	impulseForce = 0;

	radiusDamage = 0;
	damageRadius = 0;
};

datablock ProjectileData (playerTeleportProjectile)
{
	directDamage = 0;
	radiusDamage = 0;
	damageRadius = 0;
	Explosion = PlayerTeleportExplosion;
	explodeOnDeath = 1;
	armingDelay = 0;
	lifetime = 10;
	uiName = "Player Teleport";
};

datablock ParticleData (playerTeleportParticleB)
{
	dragCoefficient = 0.1;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 1000;
	lifetimeVarianceMS = 500;
	useInvAlpha = 0;
	
	textureName = "base/data/particles/dot";

	colors[0] = "0.6 0.0 0.0 0.0";
	colors[1] = "1   1   0.3 1.0";
	colors[2] = "0.6 0.0 0.0 0.0";

	sizes[0] = 0.4;
	sizes[1] = 0.2;
	sizes[2] = 0;

	times[0] = 0;
	times[1] = 0.2;
	times[2] = 1;
};

datablock ParticleEmitterData (playerTeleportEmitterB)
{
	ejectionPeriodMS = 35;
	periodVarianceMS = 0;
	ejectionVelocity = 1.5;
	ejectionOffset = 0.75;
	velocityVariance = 0.49;
	thetaMin = 0;
	thetaMax = 180;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = playerTeleportParticleB;
	uiName = "Player Teleport B";
};

datablock ShapeBaseImageData (PlayerTeleportImage)
{
	shapeFile = "base/data/shapes/empty.dts";
	emap = 0;
	mountPoint = $BackSlot;

	stateName[0] = "Ready";
	stateTransitionOnTimeout[0] = "FireA";
	stateTimeoutValue[0] = 0.01;

	stateName[1] = "FireA";
	stateTransitionOnTimeout[1] = "Done";
	stateWaitForTimeout[1] = True;
	stateTimeoutValue[1] = 3;
	stateEmitter[1] = playerTeleportEmitterB;
	stateEmitterTime[1] = 3;

	stateName[2] = "Done";
	stateScript[2] = "onDone";
};

function PlayerTeleportImage::onDone ( %this, %obj, %slot )
{
	%obj.unmountImage (%slot);
}
