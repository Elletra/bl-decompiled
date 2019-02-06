datablock ParticleData (brickTrailParticle)
{
	dragCoefficient = 3;
	windCoefficient = 0;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 250;
	lifetimeVarianceMS = 0;
	spinSpeed = 10;
	spinRandomMin = -50.0;
	spinRandomMax = 50;
	useInvAlpha = 0;
	animateTexture = 0;
	textureName = "~/data/particles/dot";

	colors[0] = "0.2 0.2 1 0.5";
	colors[1] = "0 0 1 0.8";
	colors[2] = "0.2 0.2 1 0.0";

	sizes[0] = 0;
	sizes[1] = 0.3;
	sizes[2] = 0.01;

	times[0] = 0;
	times[1] = 0.3;
	times[2] = 1;
};

datablock ParticleEmitterData (brickTrailEmitter)
{
	ejectionPeriodMS = 1;
	periodVarianceMS = 0;
	ejectionVelocity = 60;
	velocityVariance = 0;
	ejectionOffset = 0;
	thetaMin = 0;
	thetaMax = 0;
	particles = brickTrailParticle;
};

datablock ParticleData (brickDeployExplosionParticle)
{
	dragCoefficient = 5;
	gravityCoefficient = 0.1;
	inheritedVelFactor = 0.2;
	constantAcceleration = 0;
	lifetimeMS = 500;
	lifetimeVarianceMS = 300;
	textureName = "~/data/particles/chunk";
	spinSpeed = 100;
	spinRandomMin = -150.0;
	spinRandomMax = 150;
	useInvAlpha = 0;

	colors[0] = "0.2 0.2 1.0 1.0";
	colors[1] = "0.0 0.0 1.0 0.0";

	sizes[0] = 0.4;
	sizes[1] = 0;
};

datablock ParticleEmitterData (brickDeployExplosionEmitter)
{
	lifetimeMS = 50;
	ejectionPeriodMS = 7;
	periodVarianceMS = 0;
	ejectionVelocity = 5;
	velocityVariance = 1;
	ejectionOffset = 0.2;
	thetaMin = 0;
	thetaMax = 90;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = "brickDeployExplosionParticle";
};

datablock ExplosionData (brickDeployExplosion)
{
	lifetimeMS = 300;
	particleEmitter = brickDeployExplosionEmitter;

	faceViewer = 1;
	explosionScale = "1 1 1";
	shakeCamera = 0;

	camShakeFreq = "20.0 22.0 20.0";
	camShakeAmp = "1.0 1.0 1.0";
	camShakeDuration = 0.5;
	camShakeRadius = 10;

	lightStartRadius = 0;
	lightEndRadius = 0;
	lightStartColor = "0 0 0";
	lightEndColor = "0 0 0";
};

datablock ProjectileData (brickDeployProjectile)
{
	directDamage = 0;
	radiusDamage = 0;
	damageRadius = 0;
	Explosion = brickDeployExplosion;
	muzzleVelocity = 60;
	velInheritFactor = 0;
	armingDelay = 0;
	lifetime = 250;
	fadeDelay = 70;
	bounceElasticity = 0;
	bounceFriction = 0;
	isBallistic = 0;
	gravityMod = 0;
	collideWithPlayers = 0;
	hasLight = 0;
	lightRadius = 3;
	lightColor = "0 0 0.5";
};

function brickDeployProjectile::onCollision ( %this, %obj, %col, %fade, %pos, %normal )
{
	if ( $Game::MissionCleaningUp )
	{
		return;
	}


	%client = %obj.client;
	%player = %client.Player;

	if ( !%player )
	{
		return;
	}

	if ( %client.currInv > 0 )
	{
		%data = %client.inventory[%client.currInv];
	}
	else if ( isObject(%client.instantUseData) )
	{
		%data = %client.instantUseData;
	}


	if ( !isObject(%data) )
	{
		return;
	}

	if ( %data.getClassName() !$= "fxDTSBrickData" )
	{
		return;
	}

	if ( isObject(%player.tempBrick) )
	{
		%player.tempBrick.setDatablock (%data);
		%aspectRatio = %data.printAspectRatio;

		if ( %aspectRatio !$= "" )
		{
			if ( %client.lastPrint[%aspectRatio] !$= "" )
			{
				%player.tempBrick.setPrint ( %client.lastPrint[%aspectRatio] );
			}
			else
			{
				%player.tempBrick.setPrint ( $printNameTable["letters/A"] );
			}
		}

		if ( getAngleIDFromPlayer(%player) + %data.orientationFix % 4 == 0 )
		{
			%rot = "0 0 1 0";
		}
		else if ( getAngleIDFromPlayer(%player) + %data.orientationFix % 4 == 1 )
		{
			%rot = "0 0 -1" SPC 90 * $pi / 180;
		}
		else if ( getAngleIDFromPlayer(%player) + %data.orientationFix % 4 == 2 )
		{
			%rot = "0 0 1" SPC 180 * $pi / 180;
		}
		else if ( getAngleIDFromPlayer(%player) + %data.orientationFix % 4 == 3 )
		{
			%rot = "0 0 1" SPC 90 * $pi / 180;
		}


		%posX = getWord (%pos, 0);
		%posY = getWord (%pos, 1);

		if ( %data.brickSizeZ % 2 == 0 )
		{
			%posZ = getWord (%pos, 2) + %data.brickSizeZ / 2 * 0.2 + 0.05;
		}
		else
		{
			%posZ = getWord (%pos, 2) + %data.brickSizeZ / 2 * 0.2 - 0.05;
		}

		if ( getWord(%normal, 2) < -0.9 )
		{
			%posZ -= %data.brickSizeZ * 0.2;
		}

		%pos = %posX SPC %posY SPC %posZ;
		%player.tempBrick.setTransform (%pos SPC %rot);
	}
	else
	{
		%b = new fxDTSBrick()
		{
			dataBlock = %data;
			angleID = getAngleIDFromPlayer (%player);
		};

		if ( isObject(%client.brickGroup) )
		{
			%client.brickGroup.add (%b);
		}
		else
		{
			error ("ERROR: brickDeployProjectile::onCollision() - client " @  %client.getPlayerName()  @ " has no brick group.");
		}


		%aspectRatio = %data.printAspectRatio;

		if ( %aspectRatio !$= "" )
		{
			if ( %client.lastPrint[%aspectRatio] !$= "" )
			{
				%b.setPrint ( %client.lastPrint[%aspectRatio] );
			}
			else
			{
				%b.setPrint ( $printNameTable["letters/A"] );
			}
		}

		if ( getAngleIDFromPlayer(%player) + %data.orientationFix % 4 == 0 )
		{
			%rot = "0 0 1 0";
		}
		else if ( getAngleIDFromPlayer(%player) + %data.orientationFix % 4 == 1 )
		{
			%rot = "0 0 -1 1.5708";
		}
		else if ( getAngleIDFromPlayer(%player) + %data.orientationFix % 4 == 2 )
		{
			%rot = "0 0 1 3.14159";
		}
		else if ( getAngleIDFromPlayer(%player) + %data.orientationFix % 4 == 3 )
		{
			%rot = "0 0 1 1.5708";
		}


		%posX = getWord (%pos, 0);
		%posY = getWord (%pos, 1);

		if ( %data.brickSizeZ % 2 == 0 )
		{
			%posZ = getWord (%pos, 2) + %data.brickSizeZ / 2 * 0.2 + 0.05;
		}
		else
		{
			%posZ = getWord(%pos, 2) + %data.brickSizeZ / 2 * 0.2 - 0.05;
		}

		if ( getWord(%normal, 2) < -0.9 )
		{
			%posZ -= %data.brickSizeZ * 0.2;
		}

		%pos = %posX SPC %posY SPC %posZ;

		%b.setTransform (%pos SPC %rot);
		%b.setColor (%client.currentColor);

		%player.tempBrick = %b;
	}
}

datablock ShapeBaseImageData (brickImage)
{
	shapeFile = "~/data/shapes/brickWeapon.dts";
	StaticShape = staticBrick2x2;
	ghost = ghostBrick2x2;
	emap = 0;
	mountPoint = 0;
	offset = "0 -0.05 0";
	rotation = eulerToMatrix("0 180 0");
	eyeOffset = "0.7 1.2 -0.8";
	eyeRotation = eulerToMatrix("0 180 0");
	correctMuzzleVector = 0;
	className = "WeaponImage";
	Item = "";
	ammo = "";
	Projectile = brickDeployProjectile;
	projectileType = Projectile;
	melee = 1;
	armReady = 1;
	showBricks = 1;
	doColorShift = 1;
	colorShiftColor = "0.647 0.647 0.647 1.000";

	stateName[0] = "Ready";
	stateTransitionOnTriggerDown[0] = "Fire";
	stateAllowImageChange[0] = 1;

	stateName[1] = "Fire";
	stateScript[1] = "onFire";
	stateFire[1] = 1;
	stateAllowImageChange[1] = 1;
	stateTimeoutValue[1] = 0.25;
	stateTransitionOnTimeout[1] = "StopFire";
	stateEmitter[1] = brickTrailEmitter;
	stateEmitterTime[1] = 0.1;
	stateSequence[1] = "Fire";

	stateName[2] = "StopFire";
	stateTransitionOnTriggerUp[2] = "Ready";
	stateAllowImageChange[2] = 1;
};

function brickImage::onDeploy ( %this, %obj, %slot )
{
	// Your code here
}


datablock ShapeBaseImageData (horseBrickImage : brickImage)
{
	mountPoint = 3;
	offset = "0 -0.1 0";
	rotation = eulerToMatrix ("0 -90 0");
};
