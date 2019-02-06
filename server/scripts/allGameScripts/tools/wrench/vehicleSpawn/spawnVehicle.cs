function fxDTSBrick::spawnVehicle ( %obj, %delay )
{
	if ( %delay > 0 )
	{
		if ( isEventPending(%obj.spawnVehicleSchedule) )
		{
			cancel (%obj.spawnVehicleSchedule);
		}

		%obj.spawnVehicleSchedule = %obj.schedule (%delay, spawnVehicle, 0);
		return;
	}
	else if ( isEventPending(%obj.spawnVehicleSchedule) )
	{
		cancel (%obj.spawnVehicleSchedule);
	}

	if ( %obj.getDataBlock().specialBrickType !$= "VehicleSpawn" )
	{
		return;
	}

	if ( !isObject(%obj.vehicleDataBlock) )
	{
		return;
	}

	if ( isObject(%obj.Vehicle) )
	{
		if ( %obj.Vehicle.getDamagePercent() < 1.0 )
		{
			%obj.Vehicle.delete();
		}
	}


	%trans = %obj.getTransform();

	%x = getWord (%trans, 0);
	%y = getWord (%trans, 1);
	%z = getWord (%trans, 2);
	%z += (getWord(%obj.getWorldBox(), 5) - getWord(%obj.getWorldBox(), 2)) / 2;

	%rot = getWords (%trans, 3, 6);

	%quotaObject = getQuotaObjectFromBrick (%obj);
	setCurrentQuotaObject (%quotaObject);

	if ( %obj.vehicleDataBlock.getClassName() $= "PlayerData" )
	{
		%v = new AIPlayer()
		{
			dataBlock = %obj.vehicleDataBlock;
		};
	}
	else if ( %obj.vehicleDataBlock.getClassName() $= "WheeledVehicleData" )
	{
		%v = new WheeledVehicle()
		{
			dataBlock = %obj.vehicleDataBlock;
		};
	}
	else if ( %obj.vehicleDataBlock.getClassName() $= "FlyingVehicleData" )
	{
		%z += %obj.vehicleDataBlock.createHoverHeight;

		%v = new FlyingVehicle()
		{
			dataBlock = %obj.vehicleDataBlock;
		};
	}
	else if ( %obj.vehicleDataBlock.getClassName() $= "HoverVehicleData" )
	{
		%z += %obj.vehicleDataBlock.createHoverHeight;

		%v = new HoverVehicle()
		{
			dataBlock = %obj.vehicleDataBlock;
		};
	}

	if ( !%v )
	{
		return;
	}

	MissionCleanup.add (%v);


	%v.spawnBrick = %obj;
	%v.brickGroup = %obj.getGroup();

	%obj.Vehicle = %v;
	%obj.colorVehicle();

	if ( !%v.getType() & $TypeMasks::PlayerObjectType )
	{
		%worldBoxZ = mAbs ( getWord ( %v.getWorldBox(), 2 )  - getWord ( %v.getWorldBox(), 5 ) );
		%worldBoxZ = mAbs ( getWord ( %v.getWorldBox(), 2 )  - getWord ( %v.getPosition(), 2 ) );
		%z += %worldBoxZ + 0.1;
	}


	%trans = %x SPC %y SPC %z SPC %rot;
	%v.setTransform (%trans);

	if ( %v.getType() & $TypeMasks::PlayerObjectType )
	{
		%pos = %v.getHackPosition();
	}
	else
	{
		%pos = %v.getWorldBoxCenter();
	}

	%p = new Projectile()
	{
		dataBlock = spawnProjectile;

		initialVelocity = "0 0 0";
		initialPosition = %pos;

		sourceObject = %v;
		sourceSlot = 0;

		client = %this;
	};

	if ( !%p )
	{
		return;
	}

	MissionCleanup.add (%p);
}
