addMessageCallback('MsgConnectionError', handleConnectionErrorMessage);
function handleConnectionErrorMessage(%__unused, %__unused, %msgError)
{
	$ServerConnectionErrorMessage = %msgError;
}

function GameConnection::initialControlSet(%this)
{
	echo("*** Initial Control Object");
	if (!Editor::checkActiveLoadDone())
	{
		if (Canvas.getContent() != PlayGui.getId())
		{
			Canvas.setContent(PlayGui);
		}
	}
}

function GameConnection::setLagIcon(%this, %state)
{
	if (%this.getAddress() $= "local")
	{
		return;
	}
	LagIcon.setVisible(%state $= "true");
}

function GameConnection::onConnectionAccepted(%this)
{
	LagIcon.setVisible(0);
	$IamAdmin = 0;
	InitClientTeamManager();
}

function GameConnection::onConnectionTimedOut(%this)
{
	disconnectedCleanup();
	MessageBoxOK("TIMED OUT", "The server connection has timed out.");
}

function GameConnection::onConnectionDropped(%this, %msg)
{
	disconnectedCleanup();
	MessageBoxOK("DISCONNECT", "The server has dropped the connection: " @ %msg);
}

function GameConnection::onConnectionError(%this, %msg)
{
	disconnectedCleanup();
	MessageBoxOK("DISCONNECT", $ServerConnectionErrorMessage @ " (" @ %msg @ ")");
}

function GameConnection::onConnectRequestRejected(%this, %msg)
{
	if (%msg $= "CR_INVALID_PROTOCOL_VERSION")
	{
		%error = "Incompatible protocol version: Your game version is not compatible with this server.";
	}
	else if (%msg $= "CR_INVALID_CONNECT_PACKET")
	{
		%error = "Internal Error: badly formed network packet";
	}
	else if (%msg $= "CR_YOUAREBANNED")
	{
		%error = "You are not allowed to play on this server.";
	}
	else if (%msg $= "CR_SERVERFULL")
	{
		%error = "This server is full.";
	}
	else if (%msg $= "CHR_PASSWORD")
	{
		MessageBoxOK("REJECTED", "Wrong password.");
		return;
	}
	else if (%msg $= "CHR_PROTOCOL")
	{
		%error = "Incompatible protocol version: Your game version is not compatible with this server.";
	}
	else if (%msg $= "CHR_CLASSCRC")
	{
		%error = "Incompatible game classes: Your game version is not compatible with this server.";
	}
	else if (%msg $= "CHR_INVALID_CHALLENGE_PACKET")
	{
		%error = "Internal Error: Invalid server response packet";
	}
	else
	{
		%error = "Connection error.  Please try another server.  Error code: (" @ %msg @ ")";
	}
	disconnectedCleanup();
	MessageBoxOK("REJECTED", %error);
}

function GameConnection::onConnectRequestTimedOut(%this)
{
	disconnectedCleanup();
	MessageBoxOK("TIMED OUT", "Your connection to the server timed out.");
}

function disconnect()
{
	if (isObject(ServerConnection))
	{
		ServerConnection.delete();
	}
	disconnectedCleanup();
	destroyServer();
}

function disconnectedCleanup()
{
	HudMessageVector.clear();
	alxStopAll();
	if (isObject(MusicPlayer))
	{
		MusicPlayer.stop();
	}
	LagIcon.setVisible(0);
	PlayerListGui.clear();
	lstAdminPlayerList.clear();
	clientCmdclearBottomPrint();
	clientCmdClearCenterPrint();
	Canvas.setContent(MainMenuGui);
	clearTextureHolds();
	purgeResources();
	PSD_KillPrints();
}

