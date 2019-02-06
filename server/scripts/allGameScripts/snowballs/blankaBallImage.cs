datablock AudioProfile (blankaBallFireSound)
{
	fileName = "base/data/sound/northToss.wav";
	description = AudioClose3d;
	preload = 1;
};

datablock ShapeBaseImageData (blankaBallImage)
{
	shapeFile = "base/data/shapes/snowBall.dts";
	emap = 1;
	mountPoint = 0;
	offset = "-0.03 0.05 0.03";
	correctMuzzleVector = 1;
	className = "WeaponImage";
	Item = blankaBallItem;
	ammo = " ";
	Projectile = blankaBallProjectile;
	projectileType = Projectile;
	melee = 0;
	armReady = 1;
	doColorShift = 1;
	colorShiftColor = "0.400 0.196 0 1.000";

	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0.1;
	stateTransitionOnTimeout[0] = "Ready";
	stateSequence[0] = "ready";
	stateSound[0] = weaponSwitchSound;

	stateName[1] = "Ready";
	stateTransitionOnTriggerDown[1] = "Charge";
	stateAllowImageChange[1] = 1;

	stateName[2] = "Charge";
	stateTransitionOnTimeout[2] = "Armed";
	stateTimeoutValue[2] = 0.2;
	stateWaitForTimeout[2] = 0;
	stateTransitionOnTriggerUp[2] = "AbortCharge";
	stateScript[2] = "onCharge";
	stateAllowImageChange[2] = 0;

	stateName[3] = "AbortCharge";
	stateTransitionOnTimeout[3] = "Ready";
	stateTimeoutValue[3] = 0.3;
	stateWaitForTimeout[3] = 1;
	stateScript[3] = "onAbortCharge";
	stateAllowImageChange[3] = 0;

	stateName[4] = "Armed";
	stateTransitionOnTriggerUp[4] = "Fire";
	stateAllowImageChange[4] = 0;

	stateName[5] = "Fire";
	stateTransitionOnTimeout[5] = "Ready";
	stateTimeoutValue[5] = 0.5;
	stateFire[5] = 1;
	stateSequence[5] = "fire";
	stateScript[5] = "onFire";
	stateWaitForTimeout[5] = 1;
	stateAllowImageChange[5] = 0;
};


function blankaBallImage::onMount ( %this, %obj, %slot )
{
	%obj.playThread (1, armReadyRight);
	%obj.playThread (3, activate2);
}

function blankaBallImage::onUnMount ( %this, %obj, %slot )
{
	if ( %obj.snowThrown != 1 )
	{
		%p = new Projectile()
		{
			dataBlock = blankaBallProjectile;

			initialPosition = %obj.getMuzzlePoint(0);
			initialVelocity = VectorAdd ( VectorScale(%obj.getMuzzleVector(0), 4), %obj.getVelocity() );

			sourceObject = %obj;
			client = %obj.client;

			sourceSlot = 0;
			originPoint = %obj.getMuzzlePoint(0);

			scale = %obj.getScale();

			miniGame = %obj.client.miniGame;
		};
		MissionCleanup.add (%p);

		%obj.playThread (3, activate);
	}
}

function blankaBallImage::onCharge ( %this, %obj, %slot )
{
	%obj.playThread (2, spearReady);
}

function blankaBallImage::onAbortCharge ( %this, %obj, %slot )
{
	%obj.playThread (2, root);
}

function blankaBallImage::onFire ( %this, %obj, %slot )
{
	%obj.playThread (2, spearThrow);
	ServerPlay3D ( blankaBallFireSound, %obj.getPosition() );

	Parent::onFire (%this, %obj, %slot);

	%obj.snowThrown = 1;
	%obj.unmountImage (0);
	%obj.snowThrown = 0;

	%obj.playThread (1, root);
}
