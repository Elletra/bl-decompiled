function serverCmdInviteToMiniGame ( %client, %victim )
{
	if ( !isObject(%client.miniGame) )
	{
		return;
	}

	if ( %client.miniGame.owner != %client )
	{
		return;
	}

	if ( !isObject(%victim) )
	{
		return;
	}

	if ( %victim.getClassName() !$= "GameConnection" )
	{
		return;
	}

	if ( %victim.miniGame == %client.miniGame )
	{
		commandToClient (%client, 'MessageBoxOK', 'Mini-Game Invite Error', 
			'This person is already in the mini-game.');
		return;
	}

	if ( isObject(%victim.miniGame) )
	{
		commandToClient(%client, 'MessageBoxOK', 'Mini-Game Invite Error', 
			'This person is already in a different mini-game.');
		return;
	}

	if ( %victim.currentPhase < 3 )
	{
		commandToClient (%client, 'MessageBoxOK', 'Mini-Game Invite Error', 
			'This person hasn\'t connected yet.');
		return;
	}

	%ourBL_ID = %client.getBLID();

	if ( %victim.ignore[%ourBL_ID] == 1 )
	{
		commandToClient (%client, 'MessageBoxOK', 'Mini-Game Invite Error', 
			'This person is ignoring you.');
		return;
	}

	if ( %victim.miniGameInvitePending == %client.miniGame )
	{
		commandToClient (%client, 'MessageBoxOK', 'Mini-Game Invite Error', 
			'This person hasn\'t responded to your first invite yet.');
		return;
	}
	else if ( %victim.miniGameInvitePending > 0 )
	{
		commandToClient (%client, 'MessageBoxOK', 'Mini-Game Invite Error', 
			'This person is considering another invite right now.');
		return;
	}

	%victim.miniGameInvitePending = %client.miniGame;

	commandToClient (%victim, 'MiniGameInvite', %client.miniGame.title, %client.getPlayerName(), 
		%client.getBLID(), %client.miniGame);
}

function serverCmdAcceptMiniGameInvite ( %client, %miniGameID )
{
	// FIXME: can spam console with this easily
	if ( %miniGameID != %client.miniGameInvitePending )
	{
		echo ("response does not equal pending invite");
		return;
	}

	if ( !isObject(%miniGameID) )
	{
		return;
	}

	if ( %miniGameID.class !$= "MiniGameSO" )
	{
		return;
	}

	%miniGameID.addMember (%client);
	%client.miniGameInvitePending = 0;
}

function serverCmdRejectMiniGameInvite ( %client, %miniGameID )
{
	if ( %miniGameID != %client.miniGameInvitePending )
	{
		return;
	}

	%mg = %client.miniGameInvitePending;
	messageClient ( %mg.owner, '', '%1 rejected your mini-game invitation', %client.getPlayerName() );
	%client.miniGameInvitePending = 0;
}

function serverCmdIgnoreMiniGameInvite ( %client, %miniGameID )
{
	if ( %miniGameID != %client.miniGameInvitePending )
	{
		return;
	}

	%mg = %client.miniGameInvitePending;

	if ( !isObject(%mg) )
	{
		return;
	}

	%client.miniGameInvitePending = 0;

	if ( $Server::LAN )
	{
		return;
	}

	messageClient ( %mg.owner, '', '%1 rejected your mini-game invitation (ignoring)', 
		%client.getPlayerName() );

	%targetBL_ID = %mg.owner.getBLID();
	%client.ignore[%targetBL_ID] = 1;
}
