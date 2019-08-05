function initServer ()
{
	%dashes = "";
	%version = atoi ($Version);
	%version = mClampF (%version, 0, 25);

	%i = 0;

	while ( %i < %version )
	{
		%dashes = %dashes @ "-";
		%i++;
	}

	echo ("\n--------- Initializing Base: Server " @ %dashes);

	$useSteam = 0;
	$useSteam = SteamAPI_Init ();

	if ( $useSteam )
	{
		echo ("Steam initialized...");
	}

	if ( $useSteam )
	{
		if ( SteamUnlock () )
		{
			echo ("Steam unlock successful, will use Steam authentication");
		}
		else
		{
			echo ("Steam unlock failed, will use Blockland authentication");
			$useSteam = 0;
			Unlock ();
		}
	}
	else
	{
		if ( !isUnlocked () )
		{
			Unlock ();
		}
	}

	$Server::Status = "Unknown";
	$Server::TestCheats = 1;

	initBaseServer ();

	exec ("./scripts/game/game.cs");
}

function initDedicated ()
{
	enableWinConsole (1);
	echo ("\n--------- Starting Dedicated Server ---------");

	$Server::Dedicated = 1;
	$Server::LAN = 0;

	createServer ("Internet");
}

function initDedicatedLAN ()
{
	enableWinConsole (1);
	echo ("\n--------- Starting Dedicated LAN Server ---------");

	$Server::Dedicated = 1;
	$Server::LAN = 1;

	createServer ("LAN");
}

function dedicatedKeyCheck ()
{
	if ( GameWindowExists ()  &&  !$Server::Dedicated )
	{
		return;
	}

	if ( isUnlocked () )
	{
		if ( $Server::LAN )
		{
			initDedicatedLAN ();
		}
		else if ( $useSteam  &&  SteamEnabled () )
		{
			$createServerAfterAuth = 1;
			auth_Init_Server ();
		}
		else
		{
			initDedicated ();
		}
	}
	else
	{
		dedicatedKeyPrompt ();
	}
}

function dedicatedKeyPrompt ()
{
	if ( GameWindowExists ()  &&  !$Server::Dedicated )
	{
		return;
	}

	echo ("");
	echo ("**********************************************************");
	echo ("*                                                        *");
	echo ("*  Authentication Key required for hosting internet game *");
	echo ("*    Input key by via setKey(\"XXXXX-XXXX-XXXX-XXXX\");    *");
	echo ("*                    (Dont screw up)                     *");
	echo ("*                                                        *");
	echo ("**********************************************************");
	echo ("");
	echo ("");
}

function setKey ( %val )
{
	if ( GameWindowExists ()  &&  !$Server::Dedicated )
	{
		return;
	}

	%val = trim (%val);

	if ( %val $= "" )
	{
		return;
	}

	%val = strreplace (%val, "-", "");
	%val = strreplace (%val, " ", "");
	%val = trim (%val);
	%val = strupr (%val);

	setKeyDat (%val, 238811);
	Unlock ();
	dedicatedKeyCheck ();
}

function serverPart2 ()
{
	if ( $Server::Dedicated )
	{
		$useSteam = 0;
		$useSteam = SteamAPI_Init ();

		if ( $useSteam )
		{
			echo ("Steam (server,1) initialized");

			if ( SteamUnlock () )
			{
				echo ("Steam (server,1) unlock successful, will use Steam authentication");
			}
			else
			{
				echo ("Steam (server,1) unlock failed, will use Blockland authentication");
				$useSteam = 0;
				Unlock ();
			}
		}
		else
		{
			Unlock ();
		}

		if ( isUnlocked () )
		{
			if ( $Server::LAN )
			{
				initDedicatedLAN ();
			}
			else
			{
				dedicatedKeyCheck ();
			}
		}
		else
		{
			dedicatedKeyPrompt ();
		}
	}
	else
	{
		initClient ();
	}

	if ( $connectArg !$= ""  &&  !$Server::Dedicated )
	{
		Connecting_Text.setText ("Connecting to " @ $connectArg);
		Canvas.pushDialog (connectingGui);
	}

	if ( $GameModeArg !$= ""  &&  !$Server::Dedicated )
	{
		setTimeScale (10);

		$Pref::Net::ServerType = "SinglePlayer";
		createServer ($Pref::Net::ServerType);
		ConnectToServer ("local", $Pref::Server::Password, 1, 0);
	}
}
