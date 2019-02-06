datablock ParticleData (PlayerBurnParticle)
{
	dragCoefficient = 0;
	gravityCoefficient = -0.3;
	inheritedVelFactor = 0;
	windCoefficient = 0;
	constantAcceleration = 0;
	lifetimeMS = 1100;
	lifetimeVarianceMS = 900;
	spinSpeed = 0;
	spinRandomMin = -90.0;
	spinRandomMax = 90;
	useInvAlpha = 1;
	
	textureName = "base/data/particles/cloud";

	colors[0] = "0 0 0 0.0";
	colors[1] = "0.0 0.0 0.0 1.0";
	colors[2] = "0 0 0 0.0";

	sizes[0] = 0.5;
	sizes[1] = 1.5;
	sizes[2] = 1.86;

	times[0] = 0;
	times[1] = 0.4;
	times[2] = 1;
};

datablock ParticleEmitterData (PlayerBurnEmitter)
{
	ejectionPeriodMS = 20;
	periodVarianceMS = 4;
	ejectionVelocity = 0;
	ejectionOffset = 0.2;
	velocityVariance = 0;
	thetaMin = 0;
	thetaMax = 180;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = PlayerBurnParticle;
	uiName = "Player Fire";
};

datablock ShapeBaseImageData (PlayerBurnImage)
{
	shapeFile = "~/data/shapes/empty.dts";
	emap = 0;
	mountPoint = 5;
	offset = "0 0 -0.65";

	stateName[0] = "Ready";
	stateTransitionOnTimeout[0] = "FireA";
	stateTimeoutValue[0] = 0.01;

	stateName[1] = "FireA";
	stateTransitionOnTimeout[1] = "FireB";
	stateWaitForTimeout[1] = True;
	stateTimeoutValue[1] = 0.05;
	stateEmitter[1] = PlayerBurnEmitter;
	stateEmitterTime[1] = 5;
	stateEmitterNode[1] = "emitterPoint";

	stateName[2] = "FireB";
	stateTransitionOnTimeout[2] = "FireA";
	stateWaitForTimeout[2] = True;
	stateTimeoutValue[2] = 0.05;
	stateEmitter[2] = PlayerBurnEmitter;
	stateEmitterTime[2] = 0.05;
};


function Player::burn ( %player, %time )
{
	if ( isEventPending(%player.burnSchedule) )
	{
		cancel (%player.burnSchedule);
	}

	if ( %player.isEnabled() )
	{
		%player.setTempColor ("0 0 0 1", %time);
	}
	else
	{
		%player.setTempColor ("0 0 0 1", -1.0);
	}

	%player.setDecalName ("AAA-None");

	if ( %player.getWaterCoverage() <= 0.001 )
	{
		%player.mountImage (PlayerBurnImage, 3);
	}

	if (%player.isEnabled() )
	{
		%player.burnSchedule = %player.schedule (%time, clearBurn);
	}
}

function Player::burnPlayer ( %player, %time )
{
	%player.burn (%time * 1000.0);
}

function Player::clearBurn ( %player )
{
	if ( %player.getDamagePercent() >= 1.0 )
	{
		return;
	}

	if ( isEventPending(%player.burnSchedule) )
	{
		%player.ClearTempColor();
		cancel (%player.burnSchedule);
	}

	if ( %player.getMountedImage(3) == PlayerBurnImage.getId() )
	{
		%player.unmountImage (3);
	}

	%pos = %player.getHackPosition();
	%pos = VectorAdd ( %pos, VectorScale( "0 0 -1", getWord(%player.getScale(), 2) ) );

	%p = new Projectile ()
	{
		dataBlock = PlayerSootProjectile;
		initialPosition = %pos;
	};
	%p.setScale ( %player.getScale() );
	MissionCleanup.add (%p);
}


// =================
//  Register Events
// =================

registerOutputEvent ("Player", "BurnPlayer", "int 0 30 3");
registerOutputEvent ("Player", "ClearBurn", "");

registerOutputEvent ("Bot", "BurnPlayer", "int 0 30 3");
registerOutputEvent ("Bot", "ClearBurn", "");
