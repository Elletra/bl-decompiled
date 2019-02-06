function MiniGameSO::addMember ( %obj, %client )
{
	if ( !isObject(%client) )
	{
		error ("ERROR: MiniGameSO::AddMember - new member " @  %client  @ " does not exist");
		return;
	}

	if ( %client.getClassName() !$= "GameConnection" )
	{
		error ("ERROR: MiniGameSO::AddMember - new member " @  %client  @ 
			" is not a client.  This function is only for adding clients to the minigame.");
		return;
	}

	if ( %obj.isMember(%client) )
	{
		return;
	}

	if ( isObject(%client.miniGame) )
	{
		%client.miniGame.removeMember (%client);
	}

	%obj.member[%obj.numMembers] = %client;
	%obj.numMembers++;

	%client.miniGame = %obj;

	for ( %i = 0;  %i < %obj.numMembers;  %i++ )
	{
		%cl = %obj.member[%i];

		messageClient (%cl, 'MsgClientInYourMiniGame', "\c1" @  %client.getPlayerName()  @ " joined the mini-game.", %client, 1);
		messageClient (%client, 'MsgClientInYourMiniGame', '', %cl, 1);
	}

	commandToClient (%client, 'SetPlayingMiniGame', 1);
	commandToClient (%client, 'SetBuildingDisabled', !%obj.EnableBuilding);
	commandToClient (%client, 'SetPaintingDisabled', !%obj.EnablePainting);

	%client.setScore (0);

	if ( !$Server::LAN )
	{
		if ( $Pref::Server::ClearEventsOnMinigameChange )
		{
			%client.ClearEventSchedules();
		}

		%client.resetVehicles();

		%mask = $TypeMasks::PlayerObjectType | $TypeMasks::ProjectileObjectType | $TypeMasks::VehicleObjectType | $TypeMasks::CorpseObjectType;
		%client.ClearEventObjects (%mask);
	}

	if ( %obj.respawnTime > 0 )
	{
		%client.instantRespawn();
	}
	else
	{
		if ( isObject(%client.player) )
		{
			%client.player.delete();
		}

		%client.setControlObject (%client.Camera);
		%obj.checkLastManStanding();
	}

	%player = %client.player;

	if ( isObject(%player) )
	{
		%player.setShapeNameColor ( $MiniGameColorF[%obj.colorIdx] );
	}

	if ( %obj.owner == %client )
	{
		%brickGroup = %client.brickGroup;
		%count = %brickGroup.getCount();

		for ( %i = 0;  %i < %count;  %i++ )
		{
			%checkObj = %brickGroup.getObject (%i);

			if ( %checkObj.getDataBlock().getId() == brickVehicleSpawnData.getId() )
			{
				%checkObj.vehicleMinigameEject();
			}
		}
	}
	else if ( %i < %obj.numMembers )
	{
		%brickGroup = %client.brickGroup;
		%count = %brickGroup.getCount();
		
		for ( %i = 0;  %i < %count;  %i++ )
		{
			%checkObj = %brickGroup.getObject (%i);

			if ( %checkObj.getDataBlock().getId() == brickVehicleSpawnData.getId() )
			{
				%checkObj.vehicleMinigameEject();
			}
		}
	}
}

function MiniGameSO::isMember ( %obj, %client )
{
	for ( %i = 0;  %i < %obj.numMembers;  %i++ )
	{
		if ( %obj.member[%i] == %client )
		{
			return true;
		}
	}

	return false;
}

function MiniGameSO::removeMember ( %obj, %client )
{
	if ( %obj.owner == %client  &&  $DefaultMiniGame != %obj )
	{
		%obj.endGame();
		return;
	}

	for ( %i = 0;  %i < %obj.numMembers;  %i++ )
	{
		if ( %obj.member[%i] == %client )
		{
			for ( %j = %i + 1;  %j < %obj.numMembers;  %j++ )
			{
				%obj.member[%j - 1] = %obj.member[%j];
			}

			%obj.member[%obj.numMembers - 1] = "";
			%obj.numMembers--;
		}
	}

	commandToClient (%client, 'SetPlayingMiniGame', 0);
	commandToClient (%client, 'SetRunningMiniGame', 0);

	for ( %i = 0;  %i < %obj.numMembers;  %i++ )
	{
		%cl = %obj.member[%i];
		messageClient (%cl, 'MsgClientInYourMiniGame', "\c1" @  %client.getPlayerName()  @ 
			" left the mini-game.", %client, 0);
	}

	%client.setScore (0);

	if ( !$Server::LAN )
	{
		if ( $Pref::Server::ClearEventsOnMinigameChange )
		{
			%client.ClearEventSchedules();
		}

		%client.resetVehicles();

		%mask = $TypeMasks::PlayerObjectType | $TypeMasks::ProjectileObjectType | 
			$TypeMasks::VehicleObjectType | $TypeMasks::CorpseObjectType;

		%client.ClearEventObjects(%mask);
	}

	%client.miniGame = -1;

	if ( isObject(%client.player) )
	{
		%client.InstantRespawn();
	}

	if ( %obj.numMembers <= 0  &&  $DefaultMiniGame != %obj )
	{
		%obj.endGame();
		%obj.schedule (10, delete);
	}

	%brickGroup = %client.brickGroup;
	%count = %brickGroup.getCount();

	for ( %i = 0;  %i < %count;  %i++ )
	{
		%checkObj = %brickGroup.getObject (%i);

		if ( %checkObj.getDataBlock().getId() == brickVehicleSpawnData.getId() )
		{
			%checkObj.vehicleMinigameEject();
		}
	}

	%obj.checkLastManStanding();
}
