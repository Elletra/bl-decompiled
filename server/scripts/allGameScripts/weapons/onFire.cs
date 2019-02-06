function WeaponImage::onFire ( %this, %obj, %slot )
{
	%obj.hasShotOnce = true;

	if ( %this.minShotTime > 0 )
	{
		if ( getSimTime() - %obj.lastFireTime < %this.minShotTime )
		{
			return;
		}

		%obj.lastFireTime = getSimTime();
	}


	%client = %obj.client;

	if ( isObject(%client.miniGame) )
	{
		if ( getSimTime() - %client.lastF8Time < 3000 )
		{
			return;
		}
	}


	%projectile = %this.Projectile;
	%dataMuzzleVelocity = %projectile.muzzleVelocity;

	if ( %this.melee )
	{
		%initPos = %obj.getEyeTransform();
		%muzzlevector = %obj.getMuzzleVector (%slot);

		%start = %initPos;
		%vec = VectorScale (%muzzlevector, 20);
		%end = VectorAdd (%start, %vec);

		%mask = $TypeMasks::PlayerObjectType | $TypeMasks::VehicleObjectType | $TypeMasks::StaticObjectType;

		%raycast = containerRayCast (%start, %end, %mask, %obj);

		if ( %raycast )
		{
			%hitPos = posFromRaycast (%raycast);
			%eyeDiff = VectorLen ( VectorSub(%start, %hitPos) );

			%muzzlepoint = %obj.getMuzzlePoint (%slot);
			%muzzleDiff = VectorLen ( VectorSub(%muzzlepoint, %hitPos) );

			%ratio = %eyeDiff / %muzzleDiff;
			%dataMuzzleVelocity *= %ratio;
		}
	}
	else
	{
		%initPos = %obj.getMuzzlePoint (%slot);
		%muzzlevector = %obj.getMuzzleVector (%slot);

		if ( %obj.isFirstPerson() )
		{
			%start = %obj.getEyePoint();
			%eyeVec = VectorScale (%obj.getEyeVector(), 5);
			%end = VectorAdd (%start, %eyeVec);

			%mask = $TypeMasks::PlayerObjectType | $TypeMasks::VehicleObjectType | $TypeMasks::FxBrickObjectType | $TypeMasks::StaticShapeObjectType | $TypeMasks::StaticObjectType;
			
			if ( %obj.isMounted() )
			{
				%mount = %obj.getObjectMount();
			}
			else
			{
				%mount = 0;
			}


			%raycast = containerRayCast (%start, %end, %mask, %obj, %mount);

			if ( %raycast )
			{
				%eyeTarget = posFromRaycast (%raycast);
				%eyeTargetVec = VectorSub (%eyeTarget, %start);
				%eyeToMuzzle = VectorSub (%start, %initPos);

				if ( VectorLen(%eyeTargetVec) < 3.1 )
				{
					%muzzlevector = %obj.getEyeVector();
					%initPos = %start;
				}
			}
		}
	}

	%inheritFactor = %projectile.velInheritFactor;
	%objectVelocity = %obj.getVelocity();

	%eyeVector = %obj.getEyeVector();
	%rawMuzzleVector = %obj.getMuzzleVector (%slot);

	%dot = VectorDot (%eyeVector, %rawMuzzleVector);

	if ( %dot < 0.6 )
	{
		if ( VectorLen(%objectVelocity) < 14 )
		{
			%inheritFactor = 0;
		}
	}


	%gunVel = VectorScale ( %dataMuzzleVelocity,  getWord ( %obj.getScale(), 2 ) );

	%muzzleVelocity = VectorAdd ( VectorScale(%muzzlevector, %gunVel),  VectorScale(%objectVelocity, %inheritFactor) );

	if ( !isObject(%projectile) )
	{
		error("ERROR: WeaponImage::onFire() - " @  %this.getName()  @ " has invalid projectile \'" @  %projectile  @ "\'");
		return 0;
	}

	%p = new (%this.projectileType)()
	{
		dataBlock = %projectile;

		initialVelocity = %muzzleVelocity;
		initialPosition = %initPos;

		sourceObject = %obj;
		sourceSlot = %slot;

		client = %obj.client;
	};
	%p.setScale ( %obj.getScale() );
	MissionCleanup.add (%p);

	return %p;
}
