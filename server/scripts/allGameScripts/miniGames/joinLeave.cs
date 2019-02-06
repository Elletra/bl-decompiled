function serverCmdJoinMiniGame ( %client, %miniGameID )
{
	if ( %client.currentPhase < 3 )
	{
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

	if ( %miniGameID.InviteOnly )
	{
		messageClient (%client, '', 'That mini-game is invite-only.');
		return;
	}

	if ( %miniGameID.isMember(%client) )
	{
		messageClient (%client, '', 'You\'re already in that mini-game.');
		return;
	}

	if ( getSimTime() - %client.miniGameJoinTime < $Game::MiniGameJoinTime )
	{
		messageClient (%client, '', 'You must wait %1 seconds before joining another minigame.', 
			mCeil($Game::MiniGameJoinTime / 1000 - (getSimTime() - %client.miniGameJoinTime) / 1000) + 1);

		return;
	}

	if ( isObject(%client.miniGame)  &&  %client.miniGame == $DefaultMiniGame )
	{
		if ( !%client.isAdmin )
		{
			commandToClient (%client, 'messageBoxOK', "Minigame", "You can\'t leave the default minigame");
			return;
		}
	}

	%client.miniGameJoinTime = getSimTime();

	if ( isObject(%client.miniGame) )
	{
		%client.miniGame.removeMember (%client);
	}

	%miniGameID.addMember (%client);
}

function serverCmdLeaveMiniGame ( %client )
{
	if ( !isObject(%client.miniGame) )
	{
		error ("ERROR: serverCmdLeaveMiniGame() - " @  %client.getPlayerName()  @ " is not in a minigame!");
		return;
	}

	if ( isObject(%client.miniGame)  &&  %client.miniGame == $DefaultMiniGame )
	{
		if ( !%client.isAdmin )
		{
			commandToClient (%client, 'messageBoxOK', "Minigame", "You can\'t leave the default minigame");
			return;
		}
	}

	%client.miniGame.removeMember (%client);
	%client.miniGame = -1;
}

function serverCmdRemoveFromMiniGame ( %client, %victim )
{
	if ( !isObject(%client.miniGame) )
	{
		return;
	}

	if ( %client.miniGame.owner != %client )
	{
		return;
	}

	if ( %client.miniGame != %victim.miniGame )
	{
		return;
	}

	%client.miniGame.removeMember (%victim);

	messageClient ( %victim, '', '%1 kicked you from the minigame', %client.getPlayerName() );
	%client.miniGame.MessageAll ( '', '%1 kicked %2 from the minigame', %client.getPlayerName(), 
		%victim.getPlayerName() );
}
