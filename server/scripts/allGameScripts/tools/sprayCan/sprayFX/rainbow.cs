datablock ParticleData (rainbowPaintExplosionParticle)
{
	dragCoefficient = 3;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 600;
	lifetimeVarianceMS = 400;
	textureName = "~/data/particles/chunk";
	useInvAlpha = 0;
	spinSpeed = 100;
	spinRandomMin = -500.0;
	spinRandomMax = 500;

	colors[0] = "1 0 0 0.750";
	colors[1] = "0 1 0 0.750";
	colors[2] = "0 0 1 0.750";
	colors[3] = "0 0 1 0.00";

	sizes[0] = 0.3;
	sizes[1] = 0.3;
	sizes[2] = 0.3;
	sizes[3] = 0.3;

	times[0] = 0;
	times[1] = 0.45;
	times[2] = 0.9;
	times[3] = 1;
};

datablock ParticleData (rainbowPaintDropletParticle)
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

	colors[0] = "1 0 0 0.750";
	colors[1] = "0 1 0 0.750";
	colors[2] = "0 0 1 0.750";
	colors[3] = "0 0 1 0.00";

	sizes[0] = 0.1;
	sizes[1] = 0.1;
	sizes[2] = 0.1;
	sizes[3] = 0;

	times[0] = 0;
	times[1] = 0.45;
	times[2] = 0.9;
	times[3] = 1;
};

datablock ParticleEmitterData (rainbowPaintExplosionEmitter)
{
	lifetimeMS = 100;
	ejectionPeriodMS = 40;
	periodVarianceMS = 0;
	ejectionVelocity = 1.5;
	velocityVariance = 0;
	ejectionOffset = 0.4;
	thetaMin = 35;
	thetaMax = 85;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = "rainbowPaintExplosionParticle";
};

datablock ParticleEmitterData (rainbowPaintDropletEmitter)
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
	particles = "rainbowPaintDropletParticle";
};

datablock ExplosionData (rainbowPaintExplosion)
{
	lifetimeMS = 100;
	emitter[0] = rainbowPaintExplosionEmitter;
	emitter[1] = rainbowPaintDropletEmitter;
	faceViewer = 0;
	shakeCamera = 0;
};

datablock ProjectileData (rainbowPaintProjectile)
{
	className = paintProjectile;
	Explosion = rainbowPaintExplosion;
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

datablock ParticleData (rainbowPaintParticle)
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

	colors[0] = "1 0 0 0.750";
	colors[1] = "1 1 0 0.750";
	colors[2] = "0 1 0 0.750";
	colors[3] = "0 0 1 0.20";

	sizes[0] = 0.1;
	sizes[1] = 0.5;
	sizes[2] = 0.5;
	sizes[3] = 0.5;

	times[0] = 0;
	times[1] = 0.33;
	times[2] = 0.5;
	times[3] = 1;
};

datablock ParticleEmitterData (rainbowPaintEmitter)
{
	ejectionPeriodMS = 5;
	periodVarianceMS = 0;
	ejectionVelocity = 20;
	velocityVariance = 0;
	ejectionOffset = 0;
	thetaMin = 0;
	thetaMax = 4;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = rainbowPaintParticle;
};

datablock ShapeBaseImageData (rainbowSprayCanImage)
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
	Projectile = rainbowPaintProjectile;
	projectileType = Projectile;
	doColorShift = 1;
	colorShiftColor = "1 0 2 2.0";
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
	stateEmitter[2] = rainbowPaintEmitter;
	stateEmitterTime[2] = 0.06;
	stateEmitterNode[2] = "muzzleNode";
	stateSound[2] = sprayFireSound;
	stateSequence[2] = "Fire";

	stateName[3] = "StopFire";
	stateTransitionOnTimeout[3] = 0;
	stateSequence[3] = "StopFire";
	stateTransitionOnTimeout[3] = "Ready";
};

function rainbowPaintProjectile::onCollision ( %this, %obj, %col, %fade, %pos, %normal )
{
	if ( %col.getClassName() $= "fxDTSBrick" )
	{
		if ( %col.colorFxID != 6 )
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

			%col.setColorFX (6);
		}
	}
}
