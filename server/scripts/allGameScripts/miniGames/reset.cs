function MiniGameSO::reset ( %obj, %client )
{
	if ( %client $= "" )
	{
		return;
	}

	if ( %client > 0 )
	{
		if ( %client.miniGame != %obj )
		{
			return;
		}
	}

	%currTime = getSimTime();

	if ( %obj.lastResetTime + 5000 > %currTime )
	{
		return;
	}

	%obj.lastResetTime = %currTime;

	cancel(%obj.timeLimitSchedule);
	%obj.timeLimitSchedule = 0;

	cancel(%obj.resetSchedule);
	%obj.resetSchedule = 0;


	%mask = $TypeMasks::PlayerObjectType | $TypeMasks::ProjectileObjectType | 
			$TypeMasks::VehicleObjectType | $TypeMasks::CorpseObjectType;

	if ( $Server::LAN )
	{
		%quotaObject = GlobalQuota;

		if ( isObject(%quotaObject) )
		{
			cancelQuotaSchedules (%quotaObject);
			%quotaObject.killObjects (%mask);
		}


		for ( %i = 0;  %i < mainBrickGroup.getCount();  %i++ )
		{
			%brickGroup = mainBrickGroup.getObject (%i);
			%brickCount = %brickGroup.getCount();

			for ( %j = 0;  %j < %brickCount;  %j++ )
			{
				%checkObj = %brickGroup.getObject (%j);

				if ( %checkObj.numEvents > 0 )
				{
					%checkObj.onMiniGameReset (%client);
				}

				if ( %checkObj.getDataBlock().getId() == brickVehicleSpawnData.getId() )
				{
					%checkObj.spawnVehicle(0);
				}

				if ( isObject(%checkObj.Item) )
				{
					%checkObj.Item.fadeIn(0);
				}
			}
		}
	}
	else
	{
		if ( %obj.UseAllPlayersBricks )
		{
			for ( %i = 0;  %i < %obj.numMembers;  %i++ )
			{
				%cl = %obj.member[%i];
				%brickGroup = %cl.brickGroup;
				%count = %brickGroup.getCount();
				%quotaObject = %brickGroup.QuotaObject;

				if ( isObject(%quotaObject) )
				{
					cancelQuotaSchedules (%quotaObject);
					%quotaObject.killObjects (%mask);
				}

				for ( %j = 0;  %j < %count;  %j++ )
				{
					%checkObj = %brickGroup.getObject (%j);

					if ( %checkObj.numEvents > 0 )
					{
						%checkObj.onMiniGameReset (%client);
					}

					if ( %checkObj.getDataBlock().getId() == brickVehicleSpawnData.getId() )
					{
						%checkObj.spawnVehicle(0);
					}

					if ( isObject(%checkObj.Item) )
					{
						%checkObj.Item.fadeIn(0);
					}
				}
			}
		}
		else
		{
			// Original was while ( %i < %obj.numMembers ) so I assumed it was fucked up and changed it to this

			if ( %obj.numMembers > 0 )
			{
				%brickGroup = %obj.owner.brickGroup;
				%count = %brickGroup.getCount();
				%quotaObject = %brickGroup.QuotaObject;

				if ( isObject(%quotaObject) )
				{
					cancelQuotaSchedules (%quotaObject);
					%quotaObject.killObjects (%mask);
				}

				for ( %i = 0;  %i < %count;  %i++ )
				{
					%checkObj = %brickGroup.getObject (%i);

					if ( %checkObj.numEvents > 0 )
					{
						%checkObj.onMiniGameReset (%client);
					}

					if ( %checkObj.getDataBlock().getId() == brickVehicleSpawnData.getId() )
					{
						%checkObj.spawnVehicle(0);
					}

					if ( isObject(%checkObj.Item) )
					{
						%checkObj.Item.fadeIn(0);
					}
				}
			}
		}

		if ( %obj == $DefaultMiniGame )
		{
			%brickGroup = BrickGroup_888888;
			%count = %brickGroup.getCount();
			%quotaObject = %brickGroup.QuotaObject;

			if ( isObject(%quotaObject) )
			{
				cancelQuotaSchedules (%quotaObject);
				%quotaObject.killObjects (%mask);
			}


			for ( %i = 0;  %i < %count;  %i++ )
			{
				%checkObj = %brickGroup.getObject (%i);

				if ( %checkObj.numEvents > 0 )
				{
					%checkObj.onMiniGameReset (%client);
				}

				if ( %checkObj.getDataBlock().getId() == brickVehicleSpawnData.getId() )
				{
					%checkObj.spawnVehicle(0);
				}

				if ( isObject(%checkObj.Item) )
				{
					%checkObj.Item.fadeIn(0);
				}
			}
		}
	}


	for ( %i = 0;  %i < %obj.numMembers;  %i++ )
	{
		%cl = %obj.member[%i];
		%cl.setScore(0);

		if ( $Pref::Server::ClearEventsOnMinigameChange )
		{
			%cl.ClearEventSchedules();
		}

		%cl.resetVehicles();

		%mask = $TypeMasks::PlayerObjectType | $TypeMasks::ProjectileObjectType | 
				$TypeMasks::VehicleObjectType | $TypeMasks::CorpseObjectType;
		
		%cl.ClearEventObjects (%mask);
		%cl.InstantRespawn();

		if ( isObject(%client) )
		{
			commandToClient (%cl, 'centerPrint', "\c3" @  %client.getPlayerName()  @ "\c5 reset the mini-game", 1);
		}
		else
		{
			commandToClient (%cl, 'centerPrint', "\c5Mini-game reset", 1);
		}
	}

	if ( %obj.TimeLimit > 0 )
	{
		%obj.timeLimitTick(1);
	}
}

function MiniGameSO::scheduleReset ( %obj, %time )
{
	if ( %time $= "" )
	{
		%time = 5000;
	}

	cancel (%obj.timeLimitSchedule);
	%obj.timeLimitSchedule = 0;

	cancel (%obj.resetSchedule);
	%obj.resetSchedule = %obj.schedule (%time, reset, 0);
}


function serverCmdResetMiniGame ( %client )
{
	if ( !isObject(%client) )
	{
		return;
	}

	%mg = %client.miniGame;

	if ( !isObject(%mg) )
	{
		return;
	}

	if ( %mg.owner != %client )
	{
		return;
	}

	%mg.reset (%client);
}
