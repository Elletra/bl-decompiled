function onServerCreated ()
{
	$Game::StartTime = 0;

	exec ("~/server/scripts/DamageTypes.cs");

	initDefaultDamageTypes();

	exec ("~/server/scripts/allGameScripts/allGameScripts.cs");


	if ( $Server::Dedicated )
	{
		// I tried to clean this up the best I could but it's still ugly

		// Should have made this a loop or something Baddy
		// smh

		if ( !isFunction ("serverCmdRequestEventTables")  || 
			 !isFunction ("ServerCmdRequestExtendedBrickInfo") || 
			 !isFunction ("SimObject", "serializeEventToString") || 
			 !isFunction ("GameConnection", "TransmitExtendedBrickInfo") || 
			 !isFunction ("auth_Init_Server") || 
			 !isFunction ("authTCPobj_Server", "onConnected") || 
			 !isFunction ("authTCPobj_Server", "onLine") || 
			 !isFunction ("WebCom_PostServer") || 
			 !isFunction ("postServerTCPObj", "onConnected") || 
			 !isFunction ("postServerTCPObj", "onLine") || 
			 !isFunction ("GameConnection", "onConnect") || 
			 !isFunction ("GameConnection", "authCheck") || 
			 !isFunction ("isValidDemoCRC") )
		{
			error ("ERROR: Required functions missing from allGameScripts (server)");
			return;
		}
	}
	else
	{
		if ( !isFunction ("serverCmdRequestEventTables") || 
			 !isFunction ("ServerCmdRequestExtendedBrickInfo") || 
			 !isFunction ("SimObject", "serializeEventToString") || 
			 !isFunction ("GameConnection", "TransmitExtendedBrickInfo") || 
			 !isFunction ("auth_Init_Client") || 
			 !isFunction ("authTCPobj_Client", "onConnected") || 
			 !isFunction ("authTCPobj_Client", "onLine") || 
			 !isFunction ("auth_Init_Server") || 
			 !isFunction ("authTCPobj_Server", "onConnected") || 
			 !isFunction ("authTCPobj_Server", "onLine") || 
			 !isFunction ("mainMenuGui", "onWake") || 
			 !isFunction ("WebCom_PostServer") || 
			 !isFunction ("postServerTCPObj", "onConnected") || 
			 !isFunction ("postServerTCPObj", "onLine") || 
			 !isFunction ("GameConnection", "onConnect") || 
			 !isFunction ("GameConnection", "authCheck") )
		{
			error ("ERROR: Required functions missing from allGameScripts");

			schedule (10, 0, disconnect);
			schedule (100, 0, MessageBoxOK, "File Error", "Required script files have been corrupted.\n\nPlease re-install the game.");
			
			return;
		}
	}

	validatePrefs();
	copyPrefsToServerVars();

	if ( $GameModeArg $= ""  ||  $GameModeArg $= "GameMode_Custom"  ||  $GameModeArg $= "Custom" )
	{
		$GameModeArg = "";
		$GameModeDisplayName = "Custom";
	}
	else
	{
		%filename = $GameModeArg;

		if ( !isFile(%filename) )
		{
			%filename = "Add-Ons/" @  $GameModeArg  @ "/gamemode.txt";
		}

		if ( !isFile(%filename) )
		{
			%filename = "Add-Ons/GameMode_" @  $GameModeArg  @ "/gamemode.txt";
		}

		if ( !isFile(%filename) )
		{
			error ("ERROR: Could not find matching game mode for \'" @  $GameModeArg  @ "\'");
			$GameModeArg = "";
		}
		else
		{
			$GameModeArg = %filename;

			%filename = findFirstFile ($GameModeArg);
			%path = filePath (%filename);

			$GameModeDisplayName = %path;
			$GameModeDisplayName = strreplace ($GameModeDisplayName, "Add-Ons/", "");
			$GameModeDisplayName = getSubStr ($GameModeDisplayName, strlen("gamemode_"), 999);
			$GameModeDisplayName = strreplace ($GameModeDisplayName, "_", " ");

			echo ("Using game mode file \'" @  $GameModeArg  @ "\'");
		}
	}

	if ( $GameModeArg !$= "" )
	{
		setDefaultServerVars();

		if ( !GameModeGuiServer::ParseGameModeFile($GameModeArg) )
		{
			error ("ERROR: problem parsing game mode file \'" @  $GameModeArg  @ "\'");
			$GameModeArg = "";
		}
	}

	if ( $GameModeArg !$= "" )
	{
		%filename = filePath ($GameModeArg)  @ "/save.bls";

		if ( isFile(%filename) )
		{
			$SaveFileArg = %filename;
		}
	}

	setGhostLimit ($Server::GhostLimit);
	%dataBlockCount_Paint = DataBlockGroup.getCount();

	setSprayCanColors();
	%dataBlockCount_Paint = DataBlockGroup.getCount() - %dataBlockCount_Paint;

	if ( !$Server::Dedicated )
	{
		Canvas.setContent ("LoadingGui");

		LoadingProgress.setValue (0);
		LoadingSecondaryProgress.setValue (0);
		LoadingProgressTxt.setValue ("LOADING ADD-ONS");

		Canvas.repaint();
	}


	// Press F to pay respects

	if ( isValidAddOn("System_ReturnToBlockland", 0) )
	{
		if ( $Pref::Server::UseRTB )
		{
			echo ("");
			echo ("Loading RTB first...");
			exec ("Add-Ons/System_ReturnToBlockland/server.cs");
			echo ("");
		}
	}

	// Also why was there official support for RTB without its features being implemented as part of
	// the official game?
	// That never made any sense to me


	%dataBlockCount_AddOns = DataBlockGroup.getCount();

	if ( $GameModeArg $= "" )
	{
		loadAddOns();
	}
	else
	{
		loadGameModeAddOns();
	}

	%dataBlockCount_AddOns = DataBlockGroup.getCount() - %dataBlockCount_AddOns;

	if ( !$Server::Dedicated )
	{
		LoadingProgress.setValue (0);
		LoadingSecondaryProgress.setValue (0);
		LoadingProgressTxt.setValue ("LOADING MUSIC");

		Canvas.repaint();
	}

	%dataBlockCount_Music = DataBlockGroup.getCount();

	if ( $GameModeArg $= "" )
	{
		createMusicDatablocks();
	}
	else
	{
		createGameModeMusicDataBlocks();
	}

	%dataBlockCount_Music = DataBlockGroup.getCount() - %dataBlockCount_Music;
	verifyBrickUINames();

	echo ("");
	echo ("");
	echo ("Datablock Report: ");
	echo ( "  Base:    " @  DataBlockGroup.getCount() - %dataBlockCount_Paint + %dataBlockCount_AddOns + %dataBlockCount_Music );
	echo ( "  Paint:   " @  %dataBlockCount_Paint );
	echo ( "  Add-Ons: " @  %dataBlockCount_AddOns );
	echo ( "  Music:   " @  %dataBlockCount_Music );
	echo ( "  Total:   " @  DataBlockGroup.getCount() );
	echo ("");


	$Game::StartTime = $Sim::Time;

	if ( $GameModeArg $= "" )
	{
		loadPrintedBrickTextures();
	}
	else
	{
		loadGameModePrintedBrickTextures();
	}

	serverLoadAvatarNames();

	if ( $Server::Dedicated  &&  !$Server::LAN )
	{
		if ( !$useSteam  &&  SteamEnabled() )
		{
			auth_Init_Server();
		}
	}

	CreateBanManager();
	InitMinigameColors();

	if ( $SaveFileArg !$= "" )
	{
		%filename = $SaveFileArg;

		if ( !isFile(%filename) )
		{
			%filename = $SaveFileArg  @ ".bls";
		}

		if ( !isFile(%filename) )
		{
			%filename = filePath($GameModeArg)  @ "/" @  $SaveFileArg;
		}

		if ( !isFile(%filename) )
		{
			%filename = filePath($GameModeArg)  @ "/" @  $SaveFileArg  @ ".bls";
		}

		if ( !isFile(%filename) )
		{
			%filename = "saves/" @  $SaveFileArg;
		}

		if ( !isFile(%filename) )
		{
			%filename = "saves/" @  $SaveFileArg  @ ".bls";
		}

		if ( !isFile(%filename) )
		{
			$SaveFileArg = "";
		}
		else
		{
			$SaveFileArg = %filename;
		}
	}
}

function onServerDestroyed ()
{
	// Your code here
}
