datablock ParticleData (jelloPaintExplosionParticle)
{
	dragCoefficient = 0;
	gravityCoefficient = 0.8;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 600;
	lifetimeVarianceMS = 400;
	textureName = "~/data/particles/chunk";
	useInvAlpha = 0;
	spinSpeed = 100;
	spinRandomMin = -500.0;
	spinRandomMax = 500;

	colors[0] = "1 0 0 0.50";
	colors[1] = "1 0 0 0.50";
	colors[2] = "1 0 0 0.00";

	sizes[0] = 0.3;
	sizes[1] = 0.3;
	sizes[2] = 0.3;

	times[0] = 0;
	times[1] = 0.5;
	times[2] = 1;
};

datablock ParticleData (jelloPaintDropletParticle)
{
	dragCoefficient = 0;
	gravityCoefficient = 1;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 800;
	lifetimeVarianceMS = 200;
	textureName = "~/data/particles/chunk";
	useInvAlpha = 0;
	spinSpeed = 100;
	spinRandomMin = -50.0;
	spinRandomMax = 50;

	colors[0] = "1 0 0 1.0";
	colors[1] = "1 0 0 1.0";

	sizes[0] = 0.1;
	sizes[1] = 0;
};

datablock ParticleEmitterData (jelloPaintExplosionEmitter)
{
	lifetimeMS = 100;
	ejectionPeriodMS = 40;
	periodVarianceMS = 0;
	ejectionVelocity = 0.5;
	velocityVariance = 0;
	ejectionOffset = 0.4;
	thetaMin = 35;
	thetaMax = 85;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = "jelloPaintExplosionParticle";
};

datablock ParticleEmitterData (jelloPaintDropletEmitter)
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
	particles = "jelloPaintDropletParticle";
};

datablock ExplosionData (jelloPaintExplosion)
{
	lifetimeMS = 100;
	emitter[0] = jelloPaintExplosionEmitter;
	emitter[1] = jelloPaintDropletEmitter;
	faceViewer = 0;
	shakeCamera = 0;
};

datablock ProjectileData (jelloPaintProjectile)
{
	className = paintProjectile;
	Explosion = jelloPaintExplosion;
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

datablock ParticleData (jelloPaintParticle)
{
	dragCoefficient = 0;
	windCoefficient = 0;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 420;
	lifetimeVarianceMS = 75;
	spinSpeed = 0;
	spinRandomMin = -550.0;
	spinRandomMax = 550;
	useInvAlpha = 0;
	animateTexture = 0;
	textureName = "~/data/particles/chunk";

	colors[0] = "0.5 0 0 0.750";
	colors[1] = "0.5 0 0 0.750";
	colors[2] = "1 0 0 0.000";

	sizes[0] = 0.1;
	sizes[1] = 0.5;
	sizes[2] = 0.5;

	times[0] = 0;
	times[1] = 0.3;
	times[2] = 1;
};

datablock ParticleEmitterData (jelloPaintEmitter)
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
	particles = jelloPaintParticle;
};

datablock ShapeBaseImageData (jelloSprayCanImage)
{
	shapeFile = "~/data/shapes/transspraycan.dts";
	emap = 0;
	mountPoint = 0;
	offset = "0 0 0";
	eyeOffset = "0.7 1.0 -0.6";
	correctMuzzleVector = 0;
	className = "WeaponImage";
	Item = sprayCan;
	ammo = " ";
	Projectile = jelloPaintProjectile;
	projectileType = Projectile;
	doColorShift = 1;
	colorShiftColor = "0.5 0 0 0.7";
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
	stateEmitter[2] = jelloPaintEmitter;
	stateEmitterTime[2] = 0.06;
	stateEmitterNode[2] = "muzzleNode";
	stateSound[2] = sprayFireSound;
	stateSequence[2] = "Fire";

	stateName[3] = "StopFire";
	stateTransitionOnTimeout[3] = 0;
	stateSequence[3] = "StopFire";
	stateTransitionOnTimeout[3] = "Ready";
};

function jelloPaintProjectile::onCollision ( %this, %obj, %col, %fade, %pos, %normal )
{
	if ( %col.getClassName() $= "fxDTSBrick" )
	{
		if ( %col.shapeFxID != 1 )
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
				%obj.client.undoStack.push (%col  TAB "SHAPEFX" TAB  %col.shapeFxID);
			}

			%col.setShapeFX (1);
		}
	}
}
