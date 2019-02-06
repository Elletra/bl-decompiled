function serverCmdIgnoreTrustInvite ( %client, %targetBL_ID )
{
	%targetBL_ID = mFloor (%targetBL_ID);

	if ( %client.invitePendingBL_ID != %targetBL_ID )
	{
		%client.clearInvitePending();
		return;
	}

	%client.clearInvitePending();
	%client.ignore[%targetBL_ID] = 1;

	%invitingBrickGroup = "BrickGroup_" @  %targetBL_ID;

	if ( !isObject(%invitingBrickGroup) )
	{
		return;
	}

	%invitingClient = %invitingBrickGroup.client;

	if ( !isObject(%invitingClient) )
	{
		return;
	}

	commandToClient ( %invitingClient, 'MessageBoxOK', 'Trust Invite Rejected + Ignored', 
		'%1 has rejected your trust invitation and will ignore any future invites from you.', 
		%client.getPlayerName() );
}

function serverCmdUnIgnore ( %client, %targetClient )
{
	%targetBL_ID = %targetClient.getBLID();

	if ( %client.ignore[%targetBL_ID] == 1 )
	{
		%client.ignore[%targetBL_ID] = 0;

		commandToClient ( %client, 'MessageBoxOK', 'Ignore Removed', 'You are no longer ignoring %1', 
			%targetClient.getPlayerName() );
	}
}
