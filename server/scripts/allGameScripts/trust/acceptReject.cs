function serverCmdAcceptTrustInvite ( %client, %invitingBL_ID )
{
	%invitingBL_ID = mFloor (%invitingBL_ID);
	%invitingBrickGroup = "BrickGroup_" @  %invitingBL_ID;

	if ( !isObject(%invitingBrickGroup) )
	{
		%client.ClearInvitePending();
		return;
	}

	%invitingClient = %invitingBrickGroup.client;

	if ( !isObject(%invitingClient) )
	{
		%client.ClearInvitePending();
		return;
	}

	if ( %client.invitePendingBL_ID $= "" )
	{
		commandToClient (%client, 'MessageBoxOK', 'Trust Invitation Error', 'That invitation is too old.');
		return;
	}

	if ( %client.invitePendingBL_ID != %invitingBL_ID )
	{
		error ("ERROR: serverCmdAcceptTrustInvite() - invite response does not match saved invite");
		%client.ClearInvitePending();
		return;
	}

	SetMutualBrickGroupTrust (%invitingBL_ID, %client.getBLID(), %client.invitePendingLevel);

	commandToClient ( %client, 'MessageBoxOK', 'Trust Invitation Accepted', 
		'You have accepted %1\'s trust invitation.', %invitingClient.getPlayerName() );

	commandToClient ( %invitingClient, 'MessageBoxOK', 'Trust Invitation Accepted', 
		'%1 has accepted your trust invitation.', %client.getPlayerName() );

	commandToClient (%invitingClient, 'TrustInviteAccepted', %client, %client.getBLID(), %client.invitePendingLevel);

	secureCommandToClient ("zbR4HmJcSY8hdRhr", %invitingClient, 'ClientTrust', %client, %client.invitePendingLevel);
	secureCommandToClient ("zbR4HmJcSY8hdRhr", %client, 'ClientTrust', %invitingClient, %client.invitePendingLevel);

	%client.clearInvitePending();
}

function serverCmdRejectTrustInvite ( %client, %targetBL_ID )
{
	%targetBL_ID = mFloor (%targetBL_ID);

	if ( %client.invitePendingBL_ID != %targetBL_ID )
	{
		%client.clearInvitePending();
		return;
	}

	%client.clearInvitePending();
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

	%currTime = getSimTime();

	if ( %currTime - %invitingClient.lastTrustRejectionTime < 60 * 1000  ||  %invitingClient.lastTrustRejectionTime == 0 )
	{
		if ( !%invitingClient.isAdmin )
		{
			%invitingClient.lastTrustRejectionTime = %currTime;
			%invitingClient.trustRejectionCount++;

			if ( %invitingClient.trustRejectionCount >= 3 )
			{
				MessageAll ( 'MsgAdminForce', '\c3%1\c2 was kicked for spamming trust invites.', %invitingClient.getPlayerName() );
				%invitingClient.schedule (10, delete, "Do not spam trust invites.");
			}
		}
	}
	else
	{
		%invitingClient.lastTrustRejectionTime = 0;
		%invitingClient.trustRejectionCount = 0;
	}

	commandToClient ( %invitingClient, 'MessageBoxOK', 'Trust Invite Rejected', 
		'%1 has rejected your trust invitation.', %client.getPlayerName() );
}
