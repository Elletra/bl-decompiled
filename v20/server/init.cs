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
	Unlock();
	$Server::Status = "Unknown";
	$Server::TestCheats = 1;
	$Server::MissionFileSpec = "Add-Ons/Map_*/*.mis";
	initBaseServer();
	exec("./scripts/game.cs");
}

function initDedicated()
{
	enableWinConsole(1);
	echo("\n--------- Starting Dedicated Server ---------");
	$Server::Dedicated = 1;
	$Server::LAN = 0;
	if ($missionArg $= "")
	{
		%mis = "skylands";
	}
	else
	{
		%mis = $missionArg;
	}
	if (!isFile(%mis) || strpos(%mis, "tutorial") == -1)
	{
		%base = fileBase(%mis);
		%mis = "Add-Ons/Map_" @ %base @ "/" @ %base @ ".mis";
		if (!isFile(%mis))
		{
			%mis = findFirstFile("Add-Ons/Map_*/" @ %base @ ".mis");
		}
		if (!isFile(%mis))
		{
			%mis = findFirstFile("Add-Ons/Map_" @ %base @ "*/*.mis");
		}
		if (!isFile(%mis))
		{
			%mis = findFirstFile("Add-Ons/Map_*" @ %base @ "*/*.mis");
		}
		if (!isFile(%mis))
		{
			%mis = "Add-Ons/Map_Slate/slate.mis";
		}
		if (!isFile(%mis) || strpos(%mis, "tutorial") == -1)
		{
			%mis = findFirstFile("Add-Ons/Map_*/*.mis");
			if (strpos(%mis, "tutorial") == -1)
			{
				%mis = findNextFile("Add-Ons/Map_*/*.mis");
			}
		}
		if (!isFile(%mis))
		{
			error("ERROR: No mission files found! You broke the game!");
			return;
		}
	}
	createServer("Internet", %mis);
}

function initDedicatedLAN()
{
	enableWinConsole(1);
	echo("\n--------- Starting Dedicated LAN Server ---------");
	$Server::Dedicated = 1;
	$Server::LAN = 1;
	if ($missionArg $= "")
	{
		%mis = "slate";
	}
	else
	{
		%mis = $missionArg;
	}
	if (!isFile(%mis))
	{
		%base = fileBase($missionArg);
		%mis = "Add-Ons/Map_" @ %base @ "/" @ %base @ ".mis";
		if (!isFile(%mis))
		{
			%mis = findFirstFile("Add-Ons/Map_*/" @ %base @ ".mis");
		}
		if (!isFile(%mis))
		{
			%mis = findFirstFile("Add-Ons/Map_" @ %base @ "*/*.mis");
		}
		if (!isFile(%mis))
		{
			%mis = findFirstFile("Add-Ons/Map_*" @ %base @ "*/*.mis");
		}
		if (!isFile(%mis))
		{
			%mis = "Add-Ons/Map_Slate/slate.mis";
		}
		if (!isFile(%mis))
		{
			%mis = findFirstFile("Add-Ons/Map_*/*.mis");
		}
		if (!isFile(%mis))
		{
			warn("ERROR: No mission files found! You broke the game!");
			return;
		}
	}
	createServer("LAN", %mis);
}

function dedicatedKeyCheck()
{
	if (GameWindowExists() && !$Server::Dedicated)
	{
		return;
	}
	if (isUnlocked())
	{
		if ($Server::LAN)
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
		dedicatedKeyPrompt();
	}
}

function dedicatedKeyPrompt()
{
	if (GameWindowExists() && !$Server::Dedicated)
	{
		return;
	}
	echo("");
	echo("**********************************************************");
	echo("*                                                        *");
	echo("*  Authentication Key required for hosting internet game *");
	echo("*    Input key by via setKey(\"XXXXX-XXXX-XXXX-XXXX\");    *");
	echo("*                    (Dont screw up)                     *");
	echo("*                                                        *");
	echo("**********************************************************");
	echo("");
	echo("");
}

function setKey(%val)
{
	if (GameWindowExists() && !$Server::Dedicated)
	{
		return;
	}
	%val = trim(%val);
	if (%val $= "")
	{
		return;
	}
	%val = strreplace(%val, "-", "");
	%val = strreplace(%val, " ", "");
	%val = trim(%val);
	%val = strupr(%val);
	setKeyDat(%val, 238811);
	Unlock();
	dedicatedKeyCheck();
}

function serverPart2()
{
	if ($Server::Dedicated)
	{
		if (isUnlocked())
		{
			if ($Server::LAN)
			{
				initDedicatedLAN();
			}
			else
			{
				dedicatedKeyCheck();
			}
		}
		else
		{
			dedicatedKeyPrompt();
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
	if ($missionArg !$= "" && !$Server::Dedicated)
	{
		%mis = $missionArg;
		if (!isFile(%mis))
		{
			%base = fileBase($missionArg);
			%mis = "Add-Ons/Map_" @ %base @ "/" @ %base @ ".mis";
			if (!isFile(%mis))
			{
				%mis = findFirstFile("Add-Ons/Map_*/" @ %base @ ".mis");
			}
			if (!isFile(%mis))
			{
				%mis = findFirstFile("Add-Ons/Map_" @ %base @ "*/*.mis");
			}
			if (!isFile(%mis))
			{
				%mis = findFirstFile("Add-Ons/Map_*" @ %base @ "*/*.mis");
			}
			if (!isFile(%mis))
			{
				%mis = findFirstFile("Add-Ons/Map_*/*.mis");
			}
		}
		setTimeScale(19038, 10);
		$Pref::Net::ServerType = "SinglePlayer";
		createServer($Pref::Net::ServerType, %mis);
		%conn = new GameConnection(ServerConnection);
		RootGroup.add(ServerConnection);
		%conn.setConnectArgs($pref::Player::LANName, $pref::Player::NetName, $Pref::Player::ClanPrefix, $Pref::Player::ClanSuffix);
		%conn.setJoinPassword($Pref::Server::Password);
		%conn.connectLocal();
	}
}

