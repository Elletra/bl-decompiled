datablock AudioProfile (errorSound)
{
	fileName = "base/data/sound/error.wav";
	description = AudioClose3d;
	preload = 1;
};

datablock AudioProfile (JumpSound)
{
	fileName = "base/data/sound/jump.wav";
	description = AudioClosest3d;
	preload = 1;
};

datablock AudioProfile (DeathCrySound)
{
	fileName = "base/data/sound/death.wav";
	description = AudioClose3d;
	preload = 1;
};

datablock AudioProfile (PainCrySound)
{
	fileName = "base/data/sound/pain.wav";
	description = AudioClose3d;
	preload = 1;
};

datablock AudioProfile (playerMountSound)
{
	fileName = "base/data/sound/playerMount.wav";
	description = AudioClosest3d;
	preload = 1;
};

datablock AudioProfile (WaterBreathMaleSound)
{
	fileName = "base/data/sound/underWater1.wav";
	description = AudioClosestLooping3d;
	preload = 1;
};

datablock AudioProfile (exitWaterSound)
{
	fileName = "base/data/sound/exitWater.wav";
	description = AudioClose3d;
	preload = 1;
};

datablock AudioProfile (Splash1Sound)
{
	fileName = "base/data/sound/splash1.wav";
	description = AudioClose3d;
	preload = 1;
};

datablock SplashData (PlayerSplash)
{
	numSegments = 15;
	ejectionFreq = 15;
	ejectionAngle = 60;
	ringLifetime = 0.5;
	lifetimeMS = 300;
	velocity = 4;
	startRadius = 0.2;
	acceleration = -3.0;
	texWrap = 5;

	texture = "base/data/particles/bubble";

	colors[0] = "0.7 0.8 1.0 0.0";
	colors[1] = "0.7 0.8 1.0 0.3";
	colors[2] = "0.7 0.8 1.0 0.7";
	colors[3] = "0.7 0.8 1.0 0.0";

	times[0] = 0;
	times[1] = 0.4;
	times[2] = 0.8;
	times[3] = 1;
};
