function GameConnection::resetVehicles ( %client )
{
	if ( !isObject(MissionCleanup) )
	{
		if ( getBuildString() !$= "Ship" )
		{
			error ("ERROR: GameConnection::ResetVehicles() - MissionCleanUp group not found!");
		}

		return;
	}

	%ourBrickGroup = %client.brickGroup;
	%count = MissionCleanup.getCount();

	for ( %i = 0;  %i < %count;  %i++ )
	{
		%obj = MissionCleanup.getObject(%i);

		if ( %obj.getType() & $TypeMasks::VehicleObjectType | $TypeMasks::PlayerObjectType  &&  isObject(%obj.spawnBrick) )
		{
			if ( %obj.spawnBrick.getGroup() == %ourBrickGroup )
			{
				%obj.spawnBrick.schedule (10, spawnVehicle);
			}
		}
	}
}


function serverCmdResetVehicles ( %client )
{
	if ( !%client.isAdmin )
	{
		return;
	}


	MessageAll ('', "\c3" @  %client.getPlayerName()  @ "\c0 reset all vehicles.");

	%count = MissionCleanup.getCount();

	for ( %i = 0;  %i < %count;  %i++ )
	{
		%obj = MissionCleanup.getObject (%i);

		if ( %obj.getType() & $TypeMasks::VehicleObjectType )
		{
			if ( isObject(%obj.spawnBrick) )
			{
				%obj.spawnBrick.schedule (10, spawnVehicle);
			}
			else
			{
				%obj.schedule (10, delete);
			}
		}
		else if ( %obj.getType() & $TypeMasks::PlayerObjectType )
		{
			if ( isObject(%obj.spawnBrick) )
			{
				%obj.spawnBrick.schedule (10, spawnVehicle);
			}
		}
	}
}
