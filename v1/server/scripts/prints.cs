datablock AudioProfile(printFireSound)
{
	fileName = "~/data/sound/printFire.wav";
	description = AudioClosest3d;
	preload = 1;
};
function loadPrintedBrickTextures()
{
	$globalPrintCount = 0;
	loadPrintedBrickTexture("2x2");
	loadPrintedBrickTexture("2x1");
	loadPrintedBrickTexture("1x1");
	loadPrintedBrickTexture("1x1r");
	loadPrintedBrickTexture("2x3r");
	loadPrintedBrickTexture("10x6");
	loadPrintedBrickTexture("15x6");
	loadPrintedBrickTexture("20x6");
	loadPrintedBrickTexture("30x6");
	loadPrintedBrickTexture("Letters");
}

function loadPrintedBrickTexture(%aspectRatio)
{
	$printARStart[%aspectRatio] = $globalPrintCount;
	%dir = "base/data/prints/" @ %aspectRatio @ "/*.png";
	%fileCount = getFileCount(%dir);
	%fileName = findFirstFile(%dir);
	for (%i = 0; %i < %fileCount; %i++)
	{
		setPrintTexture($globalPrintCount, %fileName);
		$globalPrintCount++;
		%fileName = findNextFile(%dir);
	}
	$printARNumPrints[%aspectRatio] = %fileCount;
	$printAREnd[%aspectRatio] = $globalPrintCount;
}

function sendLetterPrintInfo(%client)
{
	commandToClient(%client, 'SetLetterPrintInfo', $printARStart["Letters"], $printARNumPrints["Letters"]);
}

datablock ParticleData(printGunExplosionParticle)
{
	dragCoefficient = 2;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 600;
	lifetimeVarianceMS = 200;
	textureName = "~/data/particles/thinRing";
	useInvAlpha = 0;
	spinSpeed = 100;
	spinRandomMin = -50;
	spinRandomMax = 50;
	colors[0] = "0.000 0.317 0.745 0.500";
	colors[1] = "0.000 0.317 0.745 0.000";
	sizes[0] = 0.8;
	sizes[1] = 1.2;
};
datablock ParticleEmitterData(printGunExplosionEmitter)
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
	particles = "printGunExplosionParticle";
};
datablock ExplosionData(printGunExplosion)
{
	lifetimeMS = 150;
	emitter[0] = printGunExplosionEmitter;
	faceViewer = 0;
	shakeCamera = 0;
};
datablock ProjectileData(printGunProjectile)
{
	Explosion = printGunExplosion;
	muzzleVelocity = 20;
	velInheritFactor = 0;
	armingDelay = 0;
	lifetime = 350;
	fadeDelay = 0;
	bounceElasticity = 0;
	bounceFriction = 0;
	isBallistic = 0;
	gravityMod = 0;
	hasLight = 0;
	lightRadius = 3;
	lightColor = "0 0 0.5";
};
datablock ParticleData(printGunParticle)
{
	dragCoefficient = 3;
	windCoefficient = 0;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 280;
	lifetimeVarianceMS = 20;
	spinSpeed = 10;
	spinRandomMin = -50;
	spinRandomMax = 50;
	useInvAlpha = 0;
	animateTexture = 0;
	textureName = "~/data/particles/thinRing";
	colors[0] = "0.000 0.317 0.745 0.500";
	colors[1] = "0.000 0.317 0.745 0.000";
	sizes[0] = 0.2;
	sizes[1] = 1.5;
	times[0] = 0;
	times[1] = 1;
};
datablock ParticleEmitterData(printGunEmitter)
{
	ejectionPeriodMS = 3;
	periodVarianceMS = 0;
	ejectionVelocity = 20;
	velocityVariance = 0;
	ejectionOffset = 0;
	thetaMin = 0;
	thetaMax = 0;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = printGunParticle;
};
datablock ItemData(printGun)
{
	category = "Tools";
	className = "Weapon";
	shapeFile = "~/data/shapes/printGun.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = 1;
	uiName = "Printer";
	iconName = "base/client/ui/itemIcons/Printer";
	doColorShift = 1;
	colorShiftColor = "0.996 0.996 0.910 1.000";
	image = printGunImage;
	canDrop = 1;
};
datablock ShapeBaseImageData(printGunImage)
{
	shapeFile = "~/data/shapes/printGun.dts";
	emap = 0;
	mountPoint = 0;
	offset = "0 0 0";
	eyeOffset = "0.7 1.2 -0.55";
	correctMuzzleVector = 0;
	className = "WeaponImage";
	Item = sprayCan;
	ammo = " ";
	Projectile = printGunProjectile;
	projectileType = Projectile;
	melee = 1;
	doRetraction = 0;
	armReady = 1;
	doColorShift = 1;
	colorShiftColor = "0.996 0.996 0.910 1.000";
	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0;
	stateTransitionOnTimeout[0] = "Ready";
	stateSound[0] = sprayActivateSound;
	stateName[1] = "Ready";
	stateTransitionOnTriggerDown[1] = "Fire";
	stateAllowImageChange[1] = 1;
	stateName[2] = "Fire";
	stateScript[2] = "onFire";
	stateFire[2] = 1;
	stateAllowImageChange[2] = 1;
	stateTimeoutValue[2] = 0.25;
	stateTransitionOnTimeout[2] = "Reload";
	stateTransitionOnTriggerUp[2] = "Ready";
	stateEmitter[2] = printGunEmitter;
	stateEmitterTime[2] = 0.15;
	stateEmitterNode[2] = "muzzleNode";
	stateSound[2] = printFireSound;
	stateSequence[2] = "fire";
	stateName[3] = "Reload";
	stateTransitionOnTriggerUp[3] = "Ready";
};
function printGunImage::onFire(%__unused, %obj, %__unused)
{
	%client = %obj.client;
	%start = %obj.getEyePoint();
	%vec = %obj.getEyeVector();
	%end = VectorAdd(%start, VectorScale(%vec, 10));
	%mask = $TypeMasks::FxBrickObjectType;
	%scanTarg = containerRayCast(%start, %end, %mask, 0);
	if (%scanTarg)
	{
		if (%scanTarg.getClassName() $= "fxDTSBrick")
		{
			%colData = %scanTarg.getDataBlock();
			if (%colData.printAspectRatio !$= "")
			{
				if (getTrustLevel(%obj, %scanTarg) < $TrustLevel::Print)
				{
					commandToClient(%obj.client, 'CenterPrint', %scanTarg.getGroup().name @ " does not trust you enough to do that.", 1);
					return;
				}
				%client = %obj.client;
				%ar = %colData.printAspectRatio;
				commandToClient(%client, 'openPrintSelectorDlg', %ar, $printARStart[%ar], $printARNumPrints[%ar]);
				%client.printBrick = %scanTarg;
			}
		}
	}
}

function printGunProjectile::OnCollision(%__unused, %obj, %col, %__unused, %__unused, %__unused)
{
	return;
	if (%col.getClassName() $= "fxDTSBrick")
	{
		%colData = %col.getDataBlock();
		if (%colData.printAspectRatio !$= "")
		{
			%client = %obj.client;
			%ar = %colData.printAspectRatio;
			commandToClient(%client, 'openPrintSelectorDlg', %ar, $printARStart[%ar], $printARNumPrints[%ar]);
			%client.printBrick = %col;
		}
	}
}

