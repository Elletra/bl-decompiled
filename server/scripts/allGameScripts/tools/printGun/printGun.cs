exec ("./generatePrintCountTable.cs");
exec ("./loadPrintedBrickTextures.cs");
exec ("./letterPrints.cs");
exec ("./setPrint.cs");


datablock AudioProfile (printFireSound)
{
	fileName = "base/data/sound/printFire.wav";
	description = AudioClosest3d;
	preload = 1;
};

datablock ParticleData (printGunParticle)
{
	dragCoefficient = 3;
	windCoefficient = 0;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 280;
	lifetimeVarianceMS = 20;
	spinSpeed = 10;
	spinRandomMin = -50.0;
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

datablock ParticleEmitterData (printGunEmitter)
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

datablock ItemData (PrintGun)
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
	image = PrintGunImage;
	canDrop = 1;
};

datablock ShapeBaseImageData (PrintGunImage)
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
	Projectile = "";
	projectileType = Projectile;
	melee = 1;
	doRetraction = 0;
	armReady = 1;
	showBricks = 1;
	doColorShift = 1;
	colorShiftColor = "0.996 0.996 0.910 1.000";

	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0;
	stateTransitionOnTimeout[0] = "Ready";

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


function PrintGunImage::onFire ( %this, %player, %slot )
{
	%start = %player.getEyePoint();
	%vec = %player.getEyeVector();
	%end = VectorAdd ( %start, VectorScale(%vec, 10) );
	%mask = $TypeMasks::FxBrickAlwaysObjectType;

	%raycast = containerRayCast (%start, %end, %mask, 0);

	if ( !%raycast )
	{
		return;
	}

	%hitObj = getWord (%raycast, 0);
	%hitPos = getWords (%raycast, 1, 3);
	%hitNormal = getWords (%raycast, 4, 6);

	%this.onHitObject (%player, %slot, %hitObj, %hitPos, %hitNormal);
}

function PrintGunImage::onHitObject ( %this, %player, %slot, %hitObj, %hitPos, %hitNormal )
{
	%client = %player.client;

	if ( %hitObj.getType() & $TypeMasks::FxBrickAlwaysObjectType )
	{
		%colData = %hitObj.getDataBlock();

		if ( %colData.printAspectRatio !$= "" )
		{
			if ( getTrustLevel(%player, %hitObj) < $TrustLevel::Print )
			{
				%client.sendTrustFailureMessage ( %hitObj.getGroup() );
				return;
			}

			%ar = %colData.printAspectRatio;
			commandToClient ( %client, 'openPrintSelectorDlg', %ar, $printARStart[%ar], $printARNumPrints[%ar] );

			%client.printBrick = %hitObj;
		}
	}
}
