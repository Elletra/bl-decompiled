function GameConnection::ResetVehicles ( %client )
{
	if ( !isObject (MissionCleanUp) )
	{
		if ( getBuildString () !$= "Ship" )
		{
			Error ("ERROR: GameConnection::ResetVehicles() - MissionCleanUp group not found!");
		}

		return;
	}

	// Cycle through mission cleanup and look for vehicles that belong to this client's brick group
	%ourBrickGroup = %client.brickGroup;
	%count         = MissionCleanUp.getCount ();

	for ( %i = 0;  %i < %count;  %i++ )
	{
		%obj = MissionCleanup.getObject (%i);

		if ( !(%obj.getType () & ($TypeMasks::VehicleObjectType | $TypeMasks::PlayerObjectType)) )
		{
			continue;
		}

		if ( !isObject (%obj.spawnBrick) )
		{
			continue;
		}

		if ( %obj.spawnBrick.getGroup () != %ourBrickGroup )
		{
			continue;
		}

		%obj.spawnBrick.schedule (10, spawnVehicle);
	}
}

function serverCmdResetVehicles ( %client )
{
	if ( !%client.isAdmin )
	{
		return;
	}

	MessageAll ('', "\c3" @ %client.getPlayerName () @ "\c0 reset all vehicles.");

	%count = MissionCleanup.getCount ();

	for ( %i = 0;  %i < %count;  %i++ )
	{
		%obj = MissionCleanup.getObject (%i);

		if ( %obj.getType () & $TypeMasks::VehicleObjectType )
		{
			if ( isObject (%obj.spawnBrick) )
			{
				%obj.spawnBrick.schedule (10, spawnVehicle);
			}
			else
			{
				%obj.schedule (10, delete);
			}
		}
		else if ( (%obj.getType () & $TypeMasks::PlayerObjectType)  &&  isObject (%obj.spawnBrick) )
		{
			%obj.spawnBrick.schedule (10, spawnVehicle);
		}
	}
}
