datablock ParticleData (chromePaintExplosionParticle)
{
	dragCoefficient = 2;
	gravityCoefficient = 0.5;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 600;
	lifetimeVarianceMS = 400;
	textureName = "~/data/particles/bubble";
	useInvAlpha = 1;
	spinSpeed = 100;
	spinRandomMin = -50.0;
	spinRandomMax = 50;

	colors[0] = "0.9 0.9 1 1.00";
	colors[1] = "1 1 1 1.00";
	colors[2] = "0.5 0.5 0.5 0.000";

	sizes[0] = 0.2;
	sizes[1] = 0.3;
	sizes[2] = 0;

	times[0] = 0;
	times[1] = 0.7;
	times[2] = 1;
};

datablock ParticleData (chromePaintDropletParticle)
{
	dragCoefficient = 0;
	gravityCoefficient = 1;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 800;
	lifetimeVarianceMS = 200;
	textureName = "~/data/particles/bubble";
	useInvAlpha = 0;
	spinSpeed = 100;
	spinRandomMin = -50.0;
	spinRandomMax = 50;

	colors[0] = "1 1 1 0.500";
	colors[1] = "0.000 0.317 0.745 0.000";

	sizes[0] = 0.1;
	sizes[1] = 0;
};

datablock ParticleEmitterData (chromePaintExplosionEmitter)
{
	lifetimeMS = 100;
	ejectionPeriodMS = 40;
	periodVarianceMS = 0;
	ejectionVelocity = 3.5;
	velocityVariance = 0;
	ejectionOffset = 0;
	thetaMin = 35;
	thetaMax = 85;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = "chromePaintExplosionParticle";
};

datablock ParticleEmitterData (chromePaintDropletEmitter)
{
	lifetimeMS = 100;
	ejectionPeriodMS = 40;
	periodVarianceMS = 0;
	ejectionVelocity = 5;
	velocityVariance = 0;
	ejectionOffset = 0;
	thetaMin = 0;
	thetaMax = 90;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = "chromePaintDropletParticle";
};

datablock ExplosionData (chromePaintExplosion)
{
	lifetimeMS = 150;
	emitter[0] = chromePaintExplosionEmitter;
	emitter[1] = chromePaintDropletEmitter;
	faceViewer = 0;
	shakeCamera = 0;
};

datablock ProjectileData (chromePaintProjectile)
{
	className = paintProjectile;
	Explosion = chromePaintExplosion;
	muzzleVelocity = 20;
	velInheritFactor = 0;
	armingDelay = 0;
	lifetime = 525;
	fadeDelay = 0;
	bounceElasticity = 0;
	bounceFriction = 0;
	isBallistic = 0;
	gravityMod = 0;
	hasLight = 0;
	lightRadius = 3;
	lightColor = "0 0 0.5";
};

datablock ParticleData (chromePaintParticle)
{
	dragCoefficient = 0;
	windCoefficient = 0;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 420;
	lifetimeVarianceMS = 75;
	spinSpeed = 0;
	spinRandomMin = -50.0;
	spinRandomMax = 50;
	useInvAlpha = 1;
	animateTexture = 0;
	textureName = "~/data/particles/bubble";

	colors[0] = "1 1 1 1.00";
	colors[1] = "1 1 1 1.00";
	colors[2] = "0.5 0.5 0.5 0.000";

	sizes[0] = 0.1;
	sizes[1] = 0.5;
	sizes[2] = 1.2;

	times[0] = 0;
	times[1] = 0.9;
	times[2] = 1;
};

datablock ParticleEmitterData (chromePaintEmitter)
{
	ejectionPeriodMS = 10;
	periodVarianceMS = 0;
	ejectionVelocity = 20;
	velocityVariance = 0;
	ejectionOffset = 0;
	thetaMin = 0;
	thetaMax = 5;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = chromePaintParticle;
};

datablock ShapeBaseImageData (chromeSprayCanImage)
{
	shapeFile = "~/data/shapes/spraycan.dts";
	emap = 0;
	mountPoint = 0;
	offset = "0 0 0";
	eyeOffset = "0.7 1.0 -0.6";
	correctMuzzleVector = 0;
	className = "WeaponImage";
	Item = sprayCan;
	ammo = " ";
	Projectile = chromePaintProjectile;
	projectileType = Projectile;
	doColorShift = 1;
	colorShiftColor = "0.5 0.5 0.5 1.0";
	melee = 1;
	doRetraction = 0;
	armReady = 1;

	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0.5;
	stateTransitionOnTimeout[0] = "CapOff";
	stateTransitionOnTriggerDown[0] = "Fire";
	stateWaitForTimeout[0] = 0;
	stateSound[0] = sprayActivateSound;
	stateSequence[0] = "Shake";

	stateName[4] = "CapOff";
	stateSequence[4] = "capOff";
	stateTimeoutValue[4] = 0.2;
	stateTransitionOnTriggerDown[4] = "fire";
	stateWaitForTimeout[4] = 0;
	stateTransitionOnTimeout[4] = "Ready";

	stateName[1] = "Ready";
	stateTransitionOnTriggerDown[1] = "fire";
	stateAllowImageChange[1] = 1;

	stateName[2] = "Fire";
	stateScript[2] = "onFire";
	stateFire[2] = 1;
	stateAllowImageChange[2] = 1;
	stateTimeoutValue[2] = 0.04;
	stateTransitionOnTimeout[2] = "Fire";
	stateTransitionOnTriggerUp[2] = "StopFire";
	stateEmitter[2] = chromePaintEmitter;
	stateEmitterTime[2] = 0.06;
	stateEmitterNode[2] = "muzzleNode";
	stateSound[2] = sprayFireSound;
	stateSequence[2] = "Fire";

	stateName[3] = "StopFire";
	stateTransitionOnTimeout[3] = 0;
	stateSequence[3] = "StopFire";
	stateTransitionOnTimeout[3] = "Ready";
};

function chromePaintProjectile::onCollision ( %this, %obj, %col, %fade, %pos, %normal )
{
	if ( %col.getClassName() $= "fxDTSBrick" )
	{
		if ( %col.colorFxID != 2 )
		{
			%brickGroup = %col.getGroup();
			%client = %obj.client;

			if ( !isObject(%client) )
			{
				return;
			}

			if ( %client.brickGroup != %brickGroup )
			{
				%trustLevel = %brickGroup.Trust[ %client.getBLID() ];

				if ( %trustLevel < $TrustLevel::FXPaint )
				{
					%client.sendTrustFailureMessage (%brickGroup);
					return;
				}
			}

			if ( isObject(%obj.client) )
			{
				%obj.client.undoStack.push (%col  TAB "COLORFX" TAB  %col.colorFxID);
			}

			%col.setColorFX(2);
		}
	}
}
