function createServer ( %serverType )
{
	destroyServer();

	echo ("");

	$missionSequence = 0;

	$Server::PlayerCount = 0;
	$Server::ServerType = %serverType;


	if ( %serverType $= "SinglePlayer"  &&  $Server::Dedicated )
	{
		error ("ERROR: createServer() - SinglePlayer mode specified for dedicated server");
		%serverType = "LAN";
	}

	if ( $Server::Dedicated )
	{
		$Con::logBufferEnabled = 0;
		$Physics::enabled = 0;
		$Physics::maxBricks = 0;
	}

	if ( %serverType $= "SinglePlayer" )
	{
		echo ("Starting Single Player Server");

		$Server::LAN = 1;

		portInit (0);
		setAllowConnections (0);
	}
	else if ( %serverType $= "LAN" )
	{
		echo ("Starting LAN Server");

		$Server::LAN = 1;

		portInit (28050);
		setAllowConnections (1);
	}
	else if ( %serverType $= "Internet" )
	{
		echo ("Starting Internet Server");

		$Server::LAN = 0;

		$Pref::Server::Port = mFloor($Pref::Server::Port);

		if ( $Pref::Server::Port < 1024  ||  $Pref::Server::Port > 65535 )
		{
			$Pref::Server::Port = 28000;
		}

		if ( $portArg )
		{
			portInit ($portArg);
		}
		else
		{
			portInit ($Pref::Server::Port);
		}

		setAllowConnections (1);

		if ( !$Pref::Net::DisableUPnP  &&  getUpnpPort() != $Server::Port)
		{
			$pref::client::lastUpnpError = 0;
			upnpAdd ($Server::Port);
		}
	}

	$ServerGroup = new SimGroup (ServerGroup);

	onServerCreated();
	buildEnvironmentLists();

	if ( $UINameTableCreated == 0 )
	{
		createUINameTable();
	}

	createMission();

	$IamAdmin = 2;
	$EnvGuiServer::HasSetAdvancedOnce = 0;


	if ( $GameModeArg !$= "" )
	{
		EnvGuiServer::getIdxFromFilenames();
		EnvGuiServer::SetSimpleMode();

		if ( !$EnvGuiServer::SimpleMode )
		{
			EnvGuiServer::fillAdvancedVarsFromSimple();
			EnvGuiServer::SetAdvancedMode();
		}
	}
	else
	{
		$EnvGuiServer::SkyIdx     = 0;
		$EnvGuiServer::WaterIdx   = 0;
		$EnvGuiServer::GroundIdx  = 0;

		$EnvGuiServer::SkyFile    = "Add-Ons/Sky_Blue2/Blue2.dml";
		$EnvGuiServer::GroundFile = "Add-Ons/Ground_Plate/plate.ground";

		EnvGuiServer::getIdxFromFilenames();

		$EnvGuiServer::WaterIdx   = 0;
		$EnvGuiServer::SimpleMode = 1;
		EnvGuiServer::SetSimpleMode();

		DayCycle.setEnabled (0);

		EnvGuiServer::readAdvancedVarsFromSimple();
	}
}

function onServerCreated ()
{
	$Server::GameType = "Test App";
	$Server::MissionType = "Deathmatch";

	createGame();
}
