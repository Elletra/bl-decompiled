function GameConnection::startInvitationTimeout ( %client )
{
	if ( isEventPending(%client.clearInviteSchedule) )
	{
		error ("ERROR: GameConnection::StartInvitationTimeout() - clearInviteSchedule event pending.");
		cancel (%client.clearInviteSchedule);
	}

	%client.clearInviteSchedule = %client.schedule (1 * 60 * 1000, ClearInvitePending);
}

function GameConnection::clearInvitePending ( %client )
{
	if ( isEventPending(%client.clearInviteSchedule) )
	{
		cancel (%client.clearInviteSchedule);
	}

	%client.invitePendingBL_ID = "";
	%client.invitePendingLevel = "";
	%client.invitePendingClient = "";
}


function serverCmdTrust_Invite ( %client, %targetClient, %targetBL_ID, %level )
{
	echo ("serverCmdTrust_Invite from ", %client.getPlayerName(), " to ", %targetBL_ID, " level = ", %level);

	if ( isObject($DefaultMiniGame) )
	{
		if ( %client.miniGame == $DefaultMiniGame )
		{
			if ( !$DefaultMiniGame.EnableBuilding )
			{
				commandToClient (%client, 'MessageBoxOK', 'Trust Invite Error', 
					'Building is disabled in this game mode.  You cannot send trust invites.');
				return;
			}
		}
	}

	if ( !%targetClient.hasSpawnedOnce )
	{
		commandToClient (%client, 'MessageBoxOK', 'Trust Invite Error', 'That person has not spawned yet.');
		return;
	}

	if ( !%client.hasSpawnedOnce )
	{
		commandToClient (%client, 'MessageBoxOK', 'Trust Invite Error', 'You have not spawned yet.');
		return;
	}

	if ( %targetBL_ID == getLAN_BLID() )
	{
		commandToClient (%client, 'MessageBoxOK', 'Trust Invite Error', 'Trust lists do not apply on a LAN.');
		return;
	}

	%ourBL_ID = %client.getBLID();
	%targetBL_ID = mFloor (%targetBL_ID);

	%level = mFloor (%level);
	%currTrustLevel = %client.getBL_IDTrustLevel (%targetBL_ID);

	if ( %targetBL_ID == %ourBL_ID )
	{
		commandToClient (%client, 'MessageBoxOK', 'Trust Invite Error', 'You already trust yourself.  I hope.');
		return;
	}

	if ( %currTrustLevel >= %level )
	{
		commandToClient (%client, 'MessageBoxOK', 'Trust Invite', 'You already trust this person at that level.');
		return;
	}


	%targetBrickGroup = "BrickGroup_" @  %targetBL_ID;

	if ( !isObject(%targetBrickGroup) )
	{
		commandToClient (%client, 'MessageBoxOK', 'Trust Invite Error', 'Target brick group does not exist.');
		return;
	}

	if ( !isObject(%targetBrickGroup.client) )
	{
		commandToClient (%client, 'MessageBoxOK', 'Trust Invite Error', 'Target client does not exist.');
		return;
	}

	if ( !isObject(%targetClient) )
	{
		commandToClient (%client, 'MessageBoxOK', 'Trust Invite Error', 'Target client does not exist.');
		return;
	}

	if ( %targetClient.getClassName() !$= "GameConnection" )
	{
		commandToClient (%client, 'MessageBoxOK', 'Trust Invite Error', 'Target client is not a GameConnection.');
		return;
	}

	if ( %targetClient.getBLID() != %targetBL_ID )
	{
		commandToClient (%client, 'MessageBoxOK', 'Trust Invite Error', 'Target client does not match target bl_id.');
		return;
	}

	if ( %targetClient.Ignore[%ourBL_ID] == 1 )
	{
		commandToClient (%client, 'MessageBoxOK', 'Trust Invite Error', 'This person is ignoring your invites.');
		return;
	}

	if ( %targetClient.invitePendingBL_ID $= %ourBL_ID )
	{
		commandToClient (%client, 'MessageBoxOK', 'Trust Invite Error', 'This person hasn\t responded to your first invite yet.');
		return;
	}
	else if ( %targetClient.invitePendingBL_ID !$= "" )
	{
		commandToClient (%client, 'MessageBoxOK', 'Trust Invite Error', 'This person is responding to another invite right now.');
		return;
	}


	if ( %level == 1 )
	{
		%targetClient.invitePendingBL_ID = %ourBL_ID;
		%targetClient.invitePendingLevel = 1;
		%targetClient.invitePendingClient = %client;

		%targetClient.startInvitationTimeout();

		commandToClient (%targetClient, 'TrustInvite', %client.getPlayerName(), %client.getBLID(), 1);
	}
	else if ( %level == 2 )
	{
		%targetClient.invitePendingBL_ID = %ourBL_ID;
		%targetClient.invitePendingLevel = 2;
		%targetClient.invitePendingClient = %client;

		%targetClient.startInvitationTimeout();

		commandToClient (%targetClient, 'TrustInvite', %client.getPlayerName(), %client.getBLID(), 2);
	}
	else
	{
		commandToClient (%client, 'MessageBoxOK', 'Trust Invite Error', 'Invalid trust level specified.');
		return;
	}
}
