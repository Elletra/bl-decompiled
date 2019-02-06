datablock AudioProfile (sprayFireSound)
{
	fileName = "~/data/sound/sprayLoop.wav";
	description = AudioClosestLooping3d;
	preload = 1;
};

datablock AudioProfile (sprayActivateSound)
{
	fileName = "~/data/sound/sprayActivate.wav";
	description = AudioClosest3d;
	preload = 1;
};

datablock ParticleData (bluePaintExplosionParticle)
{
	dragCoefficient = 2;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 600;
	lifetimeVarianceMS = 200;
	textureName = "~/data/particles/cloud";
	useInvAlpha = 1;
	spinSpeed = 100;
	spinRandomMin = -50.0;
	spinRandomMax = 50;

	colors[0] = "0.000 0.317 0.745 0.500";
	colors[1] = "0.000 0.317 0.745 0.000";

	sizes[0] = 0.8;
	sizes[1] = 1.2;
};

datablock ParticleData (bluePaintDropletParticle)
{
	dragCoefficient = 0;
	gravityCoefficient = 1;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 800;
	lifetimeVarianceMS = 200;
	textureName = "~/data/particles/chunk";
	useInvAlpha = 1;
	spinSpeed = 100;
	spinRandomMin = -50.0;
	spinRandomMax = 50;

	colors[0] = "0.000 0.317 0.745 0.500";
	colors[1] = "0.000 0.317 0.745 0.000";

	sizes[0] = 0.1;
	sizes[1] = 0;
};

datablock ParticleEmitterData (bluePaintExplosionEmitter)
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
	particles = "bluePaintExplosionParticle";
};

datablock ParticleEmitterData (bluePaintDropletEmitter)
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
	particles = "bluePaintDropletParticle";
};

datablock ExplosionData (bluePaintExplosion)
{
	lifetimeMS = 150;
	emitter[0] = bluePaintExplosionEmitter;
	emitter[1] = bluePaintDropletEmitter;
	faceViewer = 0;
	shakeCamera = 0;
};

datablock ProjectileData (bluePaintProjectile)
{
	className = paintProjectile;
	Explosion = bluePaintExplosion;
	muzzleVelocity = 20;
	velInheritFactor = 0;
	armingDelay = 0;
	lifetime = 400;
	fadeDelay = 0;
	bounceElasticity = 0;
	bounceFriction = 0;
	isBallistic = 0;
	gravityMod = 0;
	hasLight = 0;
};

datablock ParticleData (bluePaintParticle)
{
	dragCoefficient = 3;
	windCoefficient = 0;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 500;
	lifetimeVarianceMS = 75;
	spinSpeed = 10;
	spinRandomMin = -50.0;
	spinRandomMax = 50;
	useInvAlpha = 1;
	animateTexture = 0;
	textureName = "~/data/particles/cloud";

	colors[0] = "0.000 0.317 0.745 0.500";
	colors[1] = "0.000 0.317 0.745 0.000";

	sizes[0] = 0.1;
	sizes[1] = 0.9;

	times[0] = 0;
	times[1] = 1;
};

datablock ParticleEmitterData (bluePaintEmitter)
{
	ejectionPeriodMS = 3;
	periodVarianceMS = 0;
	ejectionVelocity = 20;
	velocityVariance = 0;
	ejectionOffset = 0;
	thetaMin = 0;
	thetaMax = 5;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = bluePaintParticle;
	useEmitterColors = 1;
};

datablock ShapeBaseImageData (blueSprayCanImage)
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
	Projectile = bluePaintProjectile;
	projectileType = Projectile;
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
	stateEmitter[2] = bluePaintEmitter;
	stateEmitterTime[2] = 0.07;
	stateEmitterNode[2] = "muzzleNode";
	stateSound[2] = sprayFireSound;
	stateSequence[2] = "Fire";

	stateName[3] = "StopFire";
	stateTransitionOnTimeout[3] = 0;
	stateSequence[3] = "StopFire";
	stateTransitionOnTimeout[3] = "Ready";
};

function paintProjectile::onCollision ( %this, %obj, %col, %fade, %pos, %normal )
{
	%className = %col.getClassName();
	%client = %obj.client;

	if ( %className $= "fxDTSBrick" )
	{
		if ( getTrustLevel(%obj, %col) < $TrustLevel::Paint )
		{
			if ( isObject(%client) )
			{
				%client.sendTrustFailureMessage ( %col.getGroup() );
			}

			return;
		}

		if ( %col.colorID != %this.colorID )
		{
			if ( isObject(%client) )
			{
				%client.undoStack.push (%col  TAB "COLOR" TAB  %col.colorID);
			}

			%col.setColor (%this.colorID);

			if ( isObject(%col.Vehicle) )
			{
				if ( %col.reColorVehicle )
				{
					%col.colorVehicle();
				}
			}
		}
	}
	else if ( %className $= "Player"  ||  %className $= "AIPlayer" )
	{
		if ( %col.getDataBlock().paintable )
		{
			if ( !isObject(%col.spawnBrick) )
			{
				return;
			}

			if ( getTrustLevel(%obj, %col) < $TrustLevel::Paint )
			{
				if ( isObject(%client) )
				{
					%client.sendTrustFailureMessage ( %col.spawnBrick.getGroup() );
				}

				return;
			}

			if ( isObject(%col.spawnBrick) )
			{
				if ( %col.spawnBrick.reColorVehicle )
				{
					return;
				}
			}

			if ( isObject(%col.client) )
			{
				return;
			}


			%color = getColorIDTable (%this.colorID);
			%rgba = getWords (%color, 0, 2)  SPC 1;

			if ( %col.color $= "" )
			{
				%col.color = "1 1 1 1";
			}

			if ( isObject(%client) )
			{
				if ( %rgba !$= %col.color )
				{
					%client.undoStack.push (%col TAB "COLORGENERIC" TAB %col.color);
				}
			}

			%col.setNodeColor ("ALL", %rgba);
			%col.color = %rgba;
		}
		else
		{
			%color = getColorIDTable (%this.colorID);
			%rgba = getWords (%color, 0, 2)  SPC 1;

			if (%col.color $= "")
			{
				%col.color = "1 1 1 1";
			}

			%col.setTempColor (%rgba, 2000, %obj.getLastImpactPosition(), %this);
		}
	}
	else if ( %className $= "WheeledVehicle"  ||  %className $= "HoverVehicle"  ||  %className $= "FlyingVehicle" )
	{
		if ( !isObject(%col.spawnBrick) )
		{
			return;
		}

		if ( %col.spawnBrick.reColorVehicle )
		{
			return;
		}

		if ( !%col.getDataBlock().paintable )
		{
			return;
		}

		if ( %col.getDamagePercent() >= 1.0 )
		{
			return;
		}

		if ( getTrustLevel(%obj, %col) < $TrustLevel::Paint )
		{
			if ( isObject(%client) )
			{
				%client.sendTrustFailureMessage ( %col.spawnBrick.getGroup() );
			}

			return;
		}


		%color = getColorIDTable (%this.colorID);
		%rgba = getWords (%color, 0, 2)  SPC 1;

		if ( %col.color $= "" )
		{
			%col.color = "1 1 1 1";
		}

		if ( isObject(%client) )
		{
			if ( %rgba !$= %col.color )
			{
				%client.undoStack.push (%col  TAB "COLORGENERIC" TAB  %col.color);
			}
		}

		%col.setNodeColor ("ALL", %rgba);
		%col.color = %rgba;
	}
}


exec ("./sprayFX/sprayFX.cs");

exec ("./setSprayCanColors.cs");
exec ("./useSprayCan.cs");
