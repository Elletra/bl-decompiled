function GameConnection::onClientLeaveGame ( %client )
{
	%client = %client;
	serverCmdStopTalking (%client);

	%mg = %client.miniGame;

	if ( isObject(%mg) )
	{
		%mg.removeMember (%client);
	}

	if ( isObject(%client.light) )
	{
		%client.light.delete();
	}

	if ( isObject(%client.undoStack) )
	{
		%client.undoStack.delete();
	}

	%player = %client.Player;

	if ( isObject(%player)  &&  isObject(%player.tempBrick) )
	{
		%player.tempBrick.delete();
		%player.tempBrick = 0;
	}

	if ( isObject(%client.tcpObj) )
	{
		%client.tcpObj.delete();
	}

	if ( isObject(%client.Camera) )
	{
		%client.Camera.delete();
	}

	if ( isObject(%client.dummyCamera) )
	{
		%client.dummyCamera.delete();
	}

	if ( isObject(%client.Player) )
	{
		%client.Player.delete();
	}

	if ( !$Server::LAN )
	{
		%doReset = 1;
		%count = ClientGroup.getCount();

		for ( %i = 0;  %i < %count;  %i++ )
		{
			%cl = ClientGroup.getObject (%i);

			if ( %cl != %client  &&  %cl.getBLID() == %client.getBLID() )
			{
				%doReset = 0;
			}
		}

		if ( !isObject($ServerGroup) )
		{
			%doReset = 0;
		}


		if ( %doReset )
		{
			%client.resetVehicles();

			if ( $Server::ClearEventsOnClientExit  &&  isObject(%client.brickGroup) )
			{
				%quotaObject = %client.brickGroup.QuotaObject;

				if ( isObject(%quotaObject) )
				{
					%quotaObject.cancelEventsEvent = schedule (31000, %quotaObject, "cancelQuotaSchedules", %quotaObject);
					%quotaObject.cancelProjectilesEvent = %quotaObject.schedule (31000, $TypeMasks::ProjectileObjectType);
				}
			}
		}
	}

	if ( isObject(%client.brickGroup) )
	{
		if ( $Server::LAN )
		{
			if ( %client.getBLID() != getLAN_BLID() )
			{
				error ("ERROR: GameConnection::onClientLeaveGame() - Client " @  %client.getPlayerName()  @ 
					" has invalid LAN bl_id (" @  %client.getBLID()  @ ").");

				%client.brickGroup.delete();
			}
		}
		else
		{
			if ( %client.bl_id $= ""  ||  %client.getBLID() == -1 )
			{
				%client.brickGroup.delete();
			}

			%client.brickGroup.quitTime = getSimTime();
			cleanUpBrickEmptyGroups();
		}
	}
	else if ( $missionRunning  &&  %client.hasAuthedOnce )
	{
		error("ERROR: GameConnection::onClientLeaveGame() - Client " @  %client.getPlayerName()  @ " has no brick group.");
	}
}
