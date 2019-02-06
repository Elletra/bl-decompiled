datablock ParticleData (BSDParticle)
{
	dragCoefficient = 5;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	windCoefficient = 0;
	constantAcceleration = 0;
	lifetimeMS = 500;
	lifetimeVarianceMS = 0;
	useInvAlpha = 1;
	textureName = "~/data/particles/bricks";

	colors[0] = "1 0.5 0 0";
	colors[1] = "1 1 1 0.8";
	colors[2] = "1 0.5 0 0";

	sizes[0] = 1.8;
	sizes[1] = 1.5;
	sizes[2] = 1.5;

	times[0] = 0;
	times[1] = 0.2;
	times[2] = 1;
};

datablock ParticleEmitterData (BSDEmitter)
{
	ejectionPeriodMS = 35;
	periodVarianceMS = 0;
	ejectionVelocity = 0;
	ejectionOffset = 1.2;
	velocityVariance = 0;
	thetaMin = 0;
	thetaMax = 0;
	phiReferenceVel = 0;
	phiVariance = 0;
	overrideAdvance = 0;
	lifetimeMS = 100;

	particles = BSDParticle;

	uiName = "Emote - Bricks";
};

datablock ExplosionData (BSDExplosion)
{
	lifetimeMS = 2000;
	emitter[0] = BSDEmitter;
};

datablock ProjectileData (BSDProjectile)
{
	Explosion = BSDExplosion;
	armingDelay = 0;
	lifetime = 10;
	explodeOnDeath = 1;
};


function serverCmdBSD ( %client )
{
	if ( isObject(%client.player) )
	{
		%client.player.emote (BSDProjectile);
	}
}
