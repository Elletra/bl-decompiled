function initServer()
{
	%dashes = "";
	%version = atoi($Version);
	%version = mClampF(%version, 0, 25);
	for (%i = 0; %i < %version; %i++)
	{
		%dashes = %dashes @ "-";
	}
	echo("\n--------- Initializing Base: Server " @ %dashes);
	$useSteam = 0;
	$useSteam = SteamAPI_Init();
	if ($useSteam)
	{
		echo("Steam initialized...");
	}
	$Server::Status = "Unknown";
	$Server::TestCheats = 1;
	initBaseServer();
	exec("./scripts/game.cs");
}

function initDedicated()
{
	enableWinConsole(1);
	echo("\n--------- Starting Dedicated Server ---------");
	$Server::Dedicated = 1;
	$Server::LAN = 0;
	createServer("Internet");
}

function initDedicatedLAN()
{
	enableWinConsole(1);
	echo("\n--------- Starting Dedicated LAN Server ---------");
	$Server::Dedicated = 1;
	$Server::LAN = 1;
	createServer("LAN");
}

function serverPart2()
{
	if ($Server::Dedicated)
	{
		if ($Server::LAN)
		{
			initDedicatedLAN();
		}
		else if ($Server::LAN)
		{
			initDedicatedLAN();
		}
		else
		{
			initDedicated();
		}
	}
	else
	{
		initClient();
	}
	if ($connectArg !$= "" && !$Server::Dedicated)
	{
		Connecting_Text.setText("Connecting to " @ $connectArg);
		Canvas.pushDialog(connectingGui);
	}
	if ($GameModeArg !$= "" && !$Server::Dedicated)
	{
		setTimeScale(10);
		$Pref::Net::ServerType = "SinglePlayer";
		createServer($Pref::Net::ServerType);
		ConnectToServer("local", $Pref::Server::Password, 1, 0);
	}
}

