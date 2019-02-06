function GameConnection::ClearEventSchedules ( %client )
{
	if ( !isObject(%client.brickGroup) )
	{
		return;
	}

	%quotaObject = %client.brickGroup.QuotaObject;

	if ( !isObject(%quotaObject) )
	{
		return;
	}

	cancelQuotaSchedules (%quotaObject);
}

function GameConnection::ClearEventObjects ( %client, %mask )
{
	if ( !isObject(%client.brickGroup) )
	{
		return;
	}

	%quotaObject = %client.brickGroup.QuotaObject;

	if ( !isObject(%quotaObject) )
	{
		return;
	}


	if ( %mask $= "" )
	{
		%quotaObject.killObjects();
	}
	else
	{
		%quotaObject.killObjects (%mask);
	}
}


function serverCmdCancelAllEvents ( %client )
{
	if ( !%client.isAdmin )
	{
		return;
	}

	MessageAll ('', "\c3" @  %client.getPlayerName()  @ "\c0 canceled all events.");

	%count = mainBrickGroup.getCount();

	for ( %i = 0;  %i < %count;  %i++)
	{
		%brickGroup = mainBrickGroup.getObject (%i);
		%quotaObject = %brickGroup.QuotaObject;

		if ( isObject(%quotaObject) )
		{
			cancelQuotaSchedules (%quotaObject);

			if ( isObject(%brickGroup.client) )
			{
				%brickGroup.client.resetVehicles();
			}

			%mask = $TypeMasks::PlayerObjectType | $TypeMasks::ProjectileObjectType | 
					$TypeMasks::VehicleObjectType | $TypeMasks::CorpseObjectType;

			%quotaObject.killObjects (%mask);
		}
	}
}

function serverCmdCancelEvents ( %client )
{
	if ( isObject(%client.miniGame) )
	{
		if ( %client.miniGame.owner != %client )
		{
			messageClient (%client, '', "CancelEvents is not allowed while in a minigame.");
			return;
		}
	}

	if ( $Server::LAN )
	{
		if ( !%client.isAdmin )
		{
			return;
		}
	}


	%elapsedTime = getSimTime() - %client.lastCancelEventTime;

	if ( %elapsedTime < 5000 )
	{
		messageClient (%client, '', "You must wait " @  mCeil ( (5000  - %elapsedTime)  /  1000 )  
			@ " seconds.");

		return;
	}

	%client.lastCancelEventTime = getSimTime();

	messageClient (%client, '', "Deleting all events and event-spawned objects...");

	%client.ClearEventSchedules();
	%client.resetVehicles();

	%mask = $TypeMasks::PlayerObjectType | $TypeMasks::ProjectileObjectType | 
			$TypeMasks::VehicleObjectType | $TypeMasks::CorpseObjectType;

	%client.ClearEventObjects (%mask);
}
