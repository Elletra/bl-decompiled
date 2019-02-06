datablock AudioProfile (rewardSound)
{
	fileName = "base/data/sound/orchHitH.WAV";
	description = AudioDefault3d;
	preload = 1;
};

datablock ParticleData (WinStarParticle)
{
	dragCoefficient = 5;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	windCoefficient = 0;
	constantAcceleration = 0;
	lifetimeMS = 500;
	lifetimeVarianceMS = 0;
	useInvAlpha = 0;
	textureName = "~/data/particles/star1";

	colors[0] = "1 1 0 1";
	colors[1] = "1 1 0 1";
	colors[2] = "1 1 0 1";

	sizes[0] = 0.9;
	sizes[1] = 0.9;
	sizes[2] = 0.9;

	times[0] = 0;
	times[1] = 0.2;
	times[2] = 1;
};

datablock ParticleEmitterData (WinStarEmitter)
{
	ejectionPeriodMS = 35;
	periodVarianceMS = 0;
	ejectionVelocity = 0;
	ejectionOffset = 1.8;
	velocityVariance = 0;
	thetaMin = 0;
	thetaMax = 0;
	phiReferenceVel = 0;
	phiVariance = 0;
	overrideAdvance = 0;
	lifetimeMS = 100;
	particles = WinStarParticle;
	doFalloff = 0;
	emitterNode = GenericEmitterNode;
	pointEmitterNode = TenthEmitterNode;

	uiName = "Emote - Win";
};

datablock ExplosionData (WinStarExplosion)
{
	lifetimeMS = 2000;
	emitter[0] = WinStarEmitter;
};

datablock ProjectileData (winStarProjectile)
{
	Explosion = WinStarExplosion;
	armingDelay = 0;
	lifetime = 10;
	explodeOnDeath = true;
};
