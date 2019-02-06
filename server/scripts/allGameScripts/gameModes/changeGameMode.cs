function changeGameMode ( %newGameMode )
{
	if ( %newGameMode $= "" )
	{
		return;
	}

	$GameModeArg = %newGameMode;

	if ( $GameModeArg $= "Add-Ons/GameMode_Custom/gamemode.txt"  ||  $GameModeArg $= "Custom" )
	{
		$GameModeArg = "";
		$SaveFileArg = "";
	}

	if ( $Server::LAN )
	{
		%serverType = "LAN";
	}
	else
	{
		%serverType = "Internet";
	}

	%count = ClientGroup.getCount();

	for ( %i = 0;  %i < %count;  %i++ )
	{
		%cl = ClientGroup.getObject (%i);
		commandToClient (%cl, 'GameModeChange');
	}

	%delay = 1500;

	if ( isListenServer() )
	{
		if ( ClientGroup.getCount() <= 1 )
		{
			%delay = 1;
		}

		// %delay + 0  gr8 math eric
		schedule (%delay + 0, 0, disconnect);

		Canvas.schedule (%delay + 1, setContent, "LoadingGui");

		schedule (%delay + 50, 0, createServer, %serverType);
		schedule (%delay + 60, 0, ConnectToServer, "local", $Pref::Server::Password, 1, 0);
	}
	else
	{
		schedule (%delay + 0, 0, destroyServer);
		schedule (%delay + 50, 0, createServer, %serverType);
	}
}


function serverCmdGameModeGuiServer_ChangeGameMode ( %client, %idx )
{
	if ( !%client.isAdmin )
	{
		return;
	}

	if ( isListenServer() )
	{
		if ( !%client.isLocal() )
		{
			commandToClient (%client, 'MessageBoxOK', 'Game mode not changed', 
				"Remote admins cannot change the game mode on listen servers");  // Why not, Baddy???

			return;
		}
	}

	if ( %idx < 0  ||  %idx >= $GameModeGuiServer::GameModeCount )
	{
		return;
	}

	%filename = $GameModeGuiServer::GameMode[%idx];
	%path = filePath (%filename);

	%displayName = %path;
	%displayName = strreplace (%displayName, "Add-Ons/", "");
	%displayName = getSubStr (%displayName, strlen ("gamemode_"), 999);
	%displayName = strreplace (%displayName, "_", " ");

	MessageAll ('', "\c3" @  %client.getPlayerName()  @ "\c0 changed the game mode to \c6" @  %displayName);

	changeGameMode ( $GameModeGuiServer::GameMode[%idx] );
}
