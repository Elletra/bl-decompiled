function fxDTSBrick::respawnVehicle ( %obj )
{
	%obj.spawnVehicle();
}

function fxDTSBrick::recoverVehicle ( %obj )
{
	%vehicle = %obj.Vehicle;

	if ( !isObject(%vehicle) )
	{
		%obj.spawnVehicle();
		return;
	}

	%mountCount = %vehicle.getMountedObjectCount();

	if ( %mountCount <= 0 )
	{
		%obj.spawnVehicle();
		return;
	}


	for ( %i = 0;  %i < %mountCount;  %i++ )
	{
		%passenger = %vehicle.getMountedObject (%i);

		if ( isObject(%passenger.client) )
		{
			return;
		}

		if ( %passenger.getMountedObjectCount() > 0 )
		{
			return;
		}
	}

	%obj.spawnVehicle();
}


// =================
//  Register Events
// =================

registerOutputEvent ("fxDTSBrick", "respawnVehicle", "");
registerOutputEvent ("fxDTSBrick", "recoverVehicle", "");
