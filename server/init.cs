// -----------------------------------------------------------------------------
//  Torque Game Engine
//  Copyright (C) GarageGames.com, Inc.
// -----------------------------------------------------------------------------


function initServer ()
{
	%dashes = "";
	%version = atoi ($Version);
	%version = mClampF (%version, 0, 25);

	for ( %i = 0; %i < %version; %i++ )
	{
		%dashes = %dashes @ "-";
	}

	echo ("\n--------- Initializing Base: Server " @ %dashes);

	// Initialize the Steam API, if available.
	$useSteam = false;
	$useSteam = SteamAPI_Init ();

	if ( $useSteam )
	{
		echo ("Steam initialized...");
	}

	// Neither of these appear to have any function in Blockland.
	$Server::Status = "Unknown";
	$Server::TestCheats = true;

	// The common module provides the basic server functionality.
	initBaseServer ();

	// Load up game server support script.
	exec ("./scripts/game.cs");
}

function initDedicated ()
{
	enableWinConsole (true);

	echo ("\n--------- Starting Dedicated Server ---------");

	$Server::Dedicated = true;
	$Server::LAN = false;

	createServer ("Internet");
}

function initDedicatedLAN ()
{
	enableWinConsole (true);

	echo ("\n--------- Starting Dedicated LAN Server ---------");

	$Server::Dedicated = true;
	$Server::LAN = true;

	createServer ("LAN");
}

function serverPart2 ()
{
	if ( $Server::Dedicated )
	{
		if ( $Server::LAN )
		{
			initDedicatedLAN ();
		}
		else if ( $Server::LAN )
		{
			initDedicatedLAN ();
		}
		else
		{
			initDedicated ();
		}
	}
	else
	{
		initClient ();
	}

	if ( $connectArg !$= "" && !$Server::Dedicated )
	{
		Connecting_Text.setText ("Connecting to " @ $connectArg);
		Canvas.pushDialog (connectingGui);
	}

	if ( $GameModeArg !$= "" && !$Server::Dedicated )
	{
		setTimeScale (10);

		$Pref::Net::ServerType = "SinglePlayer";

		createServer ($Pref::Net::ServerType);
		ConnectToServer ("local", $Pref::Server::Password, true, false);
	}
}
