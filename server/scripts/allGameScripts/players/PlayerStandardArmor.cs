$PlayerDeathAnim::TorsoFrontFallForward = 1;
$PlayerDeathAnim::TorsoFrontFallBack = 2;
$PlayerDeathAnim::TorsoBackFallForward = 3;
$PlayerDeathAnim::TorsoLeftSpinDeath = 4;
$PlayerDeathAnim::TorsoRightSpinDeath = 5;
$PlayerDeathAnim::LegsLeftGimp = 6;
$PlayerDeathAnim::LegsRightGimp = 7;
$PlayerDeathAnim::TorsoBackFallForward = 8;
$PlayerDeathAnim::HeadFrontDirect = 9;
$PlayerDeathAnim::HeadBackFallForward = 10;
$PlayerDeathAnim::ExplosionBlowBack = 11;


datablock PlayerData (PlayerStandardArmor)
{
	renderFirstPerson = 0;
	emap = 1;
	className = Armor;

	shapeFile = "base/data/shapes/player/m.dts";

	cameraMaxDist = 8;
	cameraTilt = 0.261;
	cameraVerticalOffset = 0.75;
	cameraDefaultFov = 90;
	cameraMinFov = 5;
	cameraMaxFov = 120;

	aiAvoidThis = 1;

	minLookAngle = -1.5708;
	maxLookAngle = 1.5708;
	maxFreelookAngle = 3;

	mass = 90;
	drag = 0.1;
	density = 0.7;

	maxDamage = 100;
	maxEnergy = 100;

	repairRate = 0.33;
	rechargeRate = 0.8;

	runForce = 48 * 90;
	runEnergyDrain = 0;
	minRunEnergy = 0;

	maxForwardSpeed = 7;
	maxBackwardSpeed = 4;
	maxSideSpeed = 6;

	airControl = 0.1;

	maxForwardCrouchSpeed = 3;
	maxBackwardCrouchSpeed = 2;
	maxSideCrouchSpeed = 2;

	maxUnderwaterForwardSpeed = 8.4;
	maxUnderwaterBackwardSpeed = 7.8;
	maxUnderwaterSideSpeed = 7.8;

	jumpForce = 12 * 90;
	jumpEnergyDrain = 0;
	minJumpEnergy = 0;
	jumpDelay = 3;

	minJetEnergy = 0;
	jetEnergyDrain = 0;
	canJet = 1;

	minImpactSpeed = 30;

	speedDamageScale = 3.8;

	boundingBox = VectorScale ("1.25 1.25 2.65", 4);
	crouchBoundingBox = VectorScale ("1.25 1.25 1.00", 4);

	pickupRadius = 0.625;

	decalOffset = 0.25;

	jetEmitter = playerJetEmitter;
	jetGroundEmitter = playerJetGroundEmitter;
	jetGroundDistance = 4;

	footPuffNumParts = 10;
	footPuffRadius = 0.25;

	Splash = PlayerSplash;
	splashVelocity = 4;
	splashAngle = 67;
	splashFreqMod = 300;
	splashVelEpsilon = 0.6;

	bubbleEmitTime = 0.1;
	splashEmitter[0] = PlayerFoamDropletsEmitter;
	splashEmitter[1] = PlayerFoamEmitter;
	splashEmitter[2] = PlayerBubbleEmitter;

	mediumSplashSoundVelocity = 10;
	hardSplashSoundVelocity = 20;
	exitSplashSoundVelocity = 5;

	runSurfaceAngle = 70;
	jumpSurfaceAngle = 80;

	minJumpSpeed = 20;
	maxJumpSpeed = 30;

	horizMaxSpeed = 68;
	horizResistSpeed = 33;
	horizResistFactor = 0.35;

	upMaxSpeed = 80;
	upResistSpeed = 25;
	upResistFactor = 0.3;

	footstepSplashHeight = 0.35;

	JumpSound = JumpSound;

	impactWaterEasy = Splash1Sound;
	impactWaterMedium = Splash1Sound;
	impactWaterHard = Splash1Sound;

	groundImpactMinSpeed = 10;
	groundImpactShakeFreq = "4.0 4.0 4.0";
	groundImpactShakeAmp = "1.0 1.0 1.0";
	groundImpactShakeDuration = 0.8;
	groundImpactShakeFalloff = 10;

	exitingWater = exitWaterSound;

	maxItems = 10;
	maxWeapons = 5;
	maxTools = 5;

	uiName = "Standard Player";

	canRide = 1;
	showEnergyBar = 0;
	brickImage = brickImage;
};
