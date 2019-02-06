function serverCmdServerSettingsGui_RequestVariables ( %client )
{
	if ( !%client.isAdmin )
	{
		return;
	}

	commandToClient (%client, 'ServerSettingsGui_SetVariable', "ServerType", $Server::ServerType);
	commandToClient (%client, 'ServerSettingsGui_SetVariable', "ServerName", $Server::Name);
	commandToClient (%client, 'ServerSettingsGui_SetVariable', "MaxPlayers", $Pref::Server::MaxPlayers);

	if ( %client.isSuperAdmin )
	{
		commandToClient (%client, 'ServerSettingsGui_SetVariable', "JoinPassword", $Pref::Server::Password);
		commandToClient (%client, 'ServerSettingsGui_SetVariable', "AdminPassword", $Pref::Server::AdminPassword);
	}

	commandToClient ( %client, 'ServerSettingsGui_SetVariable', "RTBExists", 
		isFile("Add-Ons/System_ReturnToBlockland/server.cs") );

	commandToClient (%client, 'ServerSettingsGui_SetVariable', "UseRTB", $Pref::Server::UseRTB);
	commandToClient (%client, 'ServerSettingsGui_ApplyVariables');
}

function serverCmdServerSettingsGui_SetVariable ( %client, %varName, %value )
{
	if ( !%client.isAdmin )
	{
		return;
	}

	if ( %varName $= "ServerName" )
	{
		$Server::Name = %value;
		$Pref::Server::Name = %value;
	}
	else if ( %varName $= "MaxPlayers" )
	{
		$Pref::Server::MaxPlayers = mClamp (%value, 1, 99);
	}
	else if ( %varName $= "JoinPassword" )
	{
		if ( %client.isSuperAdmin )
		{
			$Pref::Server::Password = %value;
		}
	}
	else if ( %varName $= "AdminPassword" )
	{
		if ( %client.isSuperAdmin )
		{
			$Pref::Server::AdminPassword = %value;
		}
	}
	else if ( %varName $= "UseRTB" )  // See what I mean??? Random semi-integrations but no official support
	{
		$Pref::Server::UseRTB = mClamp (%value, 0, 1);
	}
}

function serverCmdServerSettingsGui_ApplyVariables ( %client )
{
	if ( !%client.isAdmin )
	{
		return;
	}

	WebCom_PostServer();
	commandToAll ('NewPlayerListGui_UpdateWindowTitle', $Server::Name, $Pref::Server::MaxPlayers);
}
