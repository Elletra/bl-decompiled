function serverCmdPrevSeat ( %client )
{
	%player = %client.Player;

	if ( !isObject(%player) )
	{
		return;
	}


	%vehicle = %player.getObjectMount();

	if ( !isObject(%vehicle) )
	{
		return;
	}


	%vehicleData = %vehicle.getDataBlock();

	if ( %vehicle.isMounted() )
	{
		if ( %vehicleData.nummountpoints <= 1 )
		{
			%currSeat = 0;

			%motherVehicle = %vehicle.getObjectMount();
			%motherVehicleData = %motherVehicle.getDataBlock();

			for ( %i = 0;  %i < %motherVehicleData.nummountpoints;  %i++ )
			{
				if ( %vehicle == %motherVehicle.getMountNodeObject(%i) )
				{
					%currSeat = %i;
				}
			}

			%vehicle = %motherVehicle;
			%vehicleData = %motherVehicleData;
		}
		else
		{
			warn ("WARNING: Multi-Seat turrets are not implemented yet");  // """"yet""""
		}
	}
	else
	{
		if ( %vehicleData.nummountpoints <= 1 )
		{
			return;
		}

		%currSeat = 0;

		for ( %i = 0;  %i < %vehicleData.nummountpoints;  %i++ )
		{
			if ( %player == %vehicle.getMountNodeObject(%i) )
			{
				%currSeat = %i;
			}
		}
	}


	for ( %i = 1;  %i < %vehicleData.nummountpoints;  %i++ )
	{
		%testSeat = %currSeat - %i;

		if ( %testSeat < 0 )
		{
			%testSeat += %vehicleData.nummountpoints;
		}

		%testSeat %= %vehicleData.nummountpoints;
		%blockingObj = %vehicle.getMountNodeObject (%testSeat);

		if ( isObject(%blockingObj) )
		{
			if ( %blockingObj.getDataBlock().rideAble )
			{
				if ( !%blockingObj.getMountedObject(0) )
				{
					%blockingObj.mountObject ( %player, mFloor ( %blockingObj.getDataBlock().mountNode[0] ) );
					%player.setControlObject (%blockingObj);

					return;
				}
			}
		}
		else
		{
			%vehicle.mountObject (%player, %testSeat);
			return;
		}
	}
}
