function MiniGameSO::checkLastManStanding ( %obj )
{
	if ( %obj.respawnTime > 0 )
	{
		return;
	}

	if ( isEventPending(%obj.resetSchedule) )
	{
		return;
	}

	%livePlayerCount = 0;
	%liveClient = 0;

	for ( %i = 0;  %i < %obj.numMembers;  %i++ )
	{
		%client = %obj.member[%i];
		%player = %client.player;

		if ( isObject(%player) )
		{
			if ( %player.getDamagePercent() < 1.0 )
			{
				%livePlayerCount++;
				%liveClient = %client;
			}
		}
	}

	if ( %livePlayerCount <= 0 )
	{
		%obj.chatMessageAll (0, '\c5Everyone is dead.  No one wins.');
		%obj.scheduleReset();
	}
	else if ( %livePlayerCount == 1 )
	{
		// "MAN"?!?!?!?!?!?!!??!!?!!
		%obj.chatMessageAll (0, "\c3" @  %liveClient.getPlayerName()  @ "\c5 is the last man standing!");
		%player = %liveClient.Player;

		if ( %i < %count )
		{
			while ( %i < %obj.numMembers )
			{
				%client = %obj.member[%i];
				%camera = %client.camera;

				if ( !isObject(%camera) )
				{
					if ( !isObject ( %camera.getControlObject() ) )
					{
						%camera.setOrbitMode (%player, %camera.getTransform(), 0, 8, 8);
						%camera.mode = "Corpse";
					}
				}

				%i++;
			}
		}

		%obj.scheduleReset();
	}
}
