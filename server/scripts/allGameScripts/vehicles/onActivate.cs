function Vehicle::onActivate ( %vehicle, %activatingObj, %activatingClient, %pos, %vec )
{
	if ( VectorLen ( %vehicle.getVelocity() ) > 2 )
	{
		return;
	}

	%client = %activatingClient;

	if ( !isObject(%client) )
	{
		return;
	}

	%player = %activatingObj;

	if ( !isObject(%player) )
	{
		return;
	}


	%doFlip = false;

	if ( isObject(%vehicle.spawnBrick) )
	{
		%vehicleOwner = findClientByBL_ID (%vehicle.spawnBrick.getGroup().bl_id);
	}
	else
	{
		%vehicleOwner = 0;
	}

	if ( isObject(%vehicleOwner) )
	{
		if ( getTrustLevel(%player, %vehicle) >= $TrustLevel::VehicleTurnover )
		{
			%doFlip = true;
		}
	}
	else
	{
		%doFlip = true;
	}

	if ( miniGameCanUse(%player, %vehicle) == 1 )
	{
		%doFlip = true;
	}

	if ( miniGameCanUse(%player, %vehicle) == 0 )
	{
		%doFlip = false;
	}

	if ( %doFlip )
	{
		%impulse = VectorNormalize (%vec);
		%impulse = VectorAdd (%impulse, "0 0 1");
		%impulse = VectorNormalize (%impulse);

		%force = %vehicle.getDataBlock().mass * 5;
		%scaleRatio = getWord (%player.getScale(), 2) / getWord (%vehicle.getScale(), 2);
		%force *= %scaleRatio;

		%impulse = VectorScale (%impulse, %force);
		%vehicle.applyImpulse (%pos, %impulse);
	}
}
