function onNeedRelight ()
{
	// Your code here
}

function onMissionLoaded ()
{
	startGame();

	$IamAdmin = 2;

	if ( $GameModeGuiServer::GameModeCount <= 0 )
	{
		GameModeGuiServer::PopulateGameModeList();
	}

	if ( $EnvGuiServer::ResourceCount <= 0 )
	{
		EnvGuiServer::PopulateEnvResourceList();
	}

	snapshotGameAssets();

	WebCom_PostServer();
	pingMatchMakerLoop();

	if ( $Server::Dedicated )
	{
		schedule (2000, 0, echo, "Dedicated server is now running.");
	}
}

function onMissionEnded ()
{
	$Game::Running = 0;
	$Game::Cycling = 0;
}


function GameConnection::onEnterMissionArea ( %client )
{
	// Your code here
}

function GameConnection::onLeaveMissionArea ( %client )
{
	// Your code here
}
