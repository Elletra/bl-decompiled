function serverCmdClearBricks ( %client, %confirm )
{
	// Bug that's actually in the game -- try it yourself

	if ( %server::Lan )
	{
		return;
	}

	if ( $Game::MissionCleaningUp )
	{
		messageClient (%client, '', 'Can\'t clear bricks during mission clean up');
		return;
	}

	if ( getSimTime() - %client.lastClearBricksTime < 5000 )
	{
		return;
	}


	%brickGroup = %client.brickGroup;

	if ( !isObject(%brickGroup) )
	{
		return;
	}

	if ( %brickGroup.getCount() <= 0 )
	{
		return;
	}

	%client.lastClearBricksTime = getSimTime();

	MessageAll ('MsgClearBricks', '\c3%1\c2 cleared \c3%2\c2\'s bricks', %client.getPlayerName(), %brickGroup.name);
	%brickGroup.ChainDeleteAll();
}

function serverCmdClearVehicles ( %client )
{
	if ( !%client.isAdmin )
	{
		return;
	}

	%vehicleCount = 0;
	%count = MissionCleanup.getCount();

	for ( %i = 0;  %i < %count;  %i++ )
	{
		%obj = MissionCleanup.getObject (%i);

		if ( %obj.getType() & $TypeMasks::VehicleObjectType )
		{
			if ( isObject(%obj.spawnBrick) )
			{
				%obj.spawnBrick.Vehicle = "";
				%obj.schedule (10, delete);
				%vehicleCount++;
			}
		}
	}

	$Server::numPhysVehicles = 0;
	MessageAll ('', "\c3" @  %client.getPlayerName()  @ "\c0 cleared all vehicles (" @  %vehicleCount  @ ").");
}

function serverCmdClearBots ( %client )
{
	if (!%client.isAdmin)
	{
		return;
	}


	%botCount = 0;
	%count = MissionCleanup.getCount();

	for ( %i = 0;  %i < %count;  %i++ )
	{
		%obj = MissionCleanup.getObject (%i);

		if ( (%obj.getType() & $TypeMasks::PlayerObjectType)  &&  !%obj.isMounted() )
		{
			%cl = %obj.getControllingClient();

			if ( isObject(%cl) )
			{
				if ( %cl.getControlObject() == %obj )
				{
					// jack
				}
				else
				{
					// shit
				}


				%cl = %obj.client;

				if ( isObject(%cl)) 
				{
					if ( %cl.Player == %obj )
					{
						// nothing
					}
					else
					{
						// here
					}

					if ( isObject(%obj.spawnBrick) )
					{
						%obj.spawnBrick.Vehicle = "";
					}

					%obj.schedule (10, delete);
					%botCount++;
				}
			}
		}
	}

	MessageAll ('', "\c3" @  %client.getPlayerName()  @ "\c0 cleared all bots (" @  %botCount  @ ").");
}
