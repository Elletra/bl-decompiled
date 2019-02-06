datablock ParticleData (PlayerSootParticle)
{
	dragCoefficient = 5;
	windCoefficient = 0;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 1000;
	lifetimeVarianceMS = 500;
	spinSpeed = 10;
	spinRandomMin = -50.0;
	spinRandomMax = 50;
	useInvAlpha = 1;
	animateTexture = 0;

	textureName = "base/data/particles/cloud";

	colors[0] = "0.0 0.0 0.0 1.0";
	colors[1] = "0.0 0.0 0.0 0.0";

	sizes[0] = 1;
	sizes[1] = 1;

	times[0] = 0;
	times[1] = 1;
};

datablock ParticleEmitterData (playerSootEmitter)
{
	ejectionPeriodMS = 1;
	periodVarianceMS = 0;
	lifetimeMS = 7;
	ejectionVelocity = 5;
	velocityVariance = 5;
	ejectionOffset = 0;
	thetaMin = 0;
	thetaMax = 180;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = "playerSootParticle";
};

datablock ExplosionData (PlayerSootExplosion)
{
	lifetimeMS = 150;
	soundProfile = "";
	particleEmitter = playerSootEmitter;
	particleDensity = 230;
	particleRadius = 2;
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

datablock ProjectileData (PlayerSootProjectile)
{
	directDamage = 0;
	radiusDamage = 0;
	damageRadius = 0;
	Explosion = PlayerSootExplosion;
	explodeOnDeath = 1;
	armingDelay = 0;
	lifetime = 10;
	uiName = "Player Soot";
};

function Player::teleportEffect ( %player )
{
	%p = new Projectile()
	{
		dataBlock = playerTeleportProjectile;
		initialPosition = %player.getHackPosition();
	};

	if ( %p )
	{
		%p.setScale ( %player.getScale() );
		MissionCleanup.add (%p);
	}

	%player.emote (PlayerTeleportImage, 1);
}
