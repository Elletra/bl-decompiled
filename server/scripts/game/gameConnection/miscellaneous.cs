function GameConnection::setCanRespawn ( %client, %val )
{
	%client.canRespawn = %val;
}

// ???

function GameConnection::cheat ( %client )
{
	%name = %client.getPlayerName();
	%client.cheat++;

	if ( %client.cheat > 10 )
	{
		%client.schedule (10, delete, "");
	}
}

function GameConnection::setLoadingIndicator ( %client, %val )
{
	commandToClient (%client, 'setLoadingIndicator', %val);
}

function GameConnection::transmitMaxPlayers ( %client )
{
	secureCommandToClient ("zbR4HmJcSY8hdRhr", %client, 'SetMaxPlayersDisplay', $Pref::Server::MaxPlayers);
}

function GameConnection::transmitServerName ( %client )
{
	secureCommandToClient ("zbR4HmJcSY8hdRhr", %client, 'SetServerNameDisplay', $pref::Player::NetName, $Server::Name);
}
