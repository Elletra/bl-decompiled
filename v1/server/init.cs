function initServer()
{
	echo("\n--------- Initializing Base: Server ---------");
	Unlock();
	$Server::Status = "Unknown";
	$Server::TestCheats = 1;
	$Server::MissionFileSpec = "*/missions/*.mis";
	initBaseServer();
	exec("./scripts/centerPrint.cs");
	exec("./scripts/game.cs");
}

function initDedicated()
{
	enableWinConsole(1);
	echo("\n--------- Starting Dedicated Server ---------");
	$Server::Dedicated = 1;
	$Server::LAN = 0;
	if ($missionArg !$= "")
	{
		createServer("Internet", $missionArg);
	}
	else
	{
		echo("No mission specified (use -mission filename)");
	}
}

function initDedicatedLAN()
{
	enableWinConsole(1);
	echo("\n--------- Starting Dedicated LAN Server ---------");
	$Server::Dedicated = 1;
	$Server::LAN = 1;
	if ($missionArg !$= "")
	{
		createServer("LAN", $missionArg);
	}
	else
	{
		echo("No mission specified (use -mission filename)");
	}
}

function dedicatedKeyCheck()
{
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
	%val = trim(%val);
	if (%val $= "")
	{
		return;
	}
	%val = strreplace(%val, "-", "");
	%val = strreplace(%val, " ", "");
	%val = trim(%val);
	%val = strupr(%val);
	setKeyDat(%val);
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
	if ($missionArg !$= "" && !$Server::Dedicated)
	{
		createServer("SinglePlayer", $missionArg);
		%conn = new GameConnection(ServerConnection);
		RootGroup.add(ServerConnection);
		%conn.setConnectArgs($pref::Player::LANName, $pref::Player::NetName, $Pref::Player::ClanPrefix, $Pref::Player::ClanSuffix);
		%conn.setJoinPassword($Client::Password);
		%conn.connectLocal();
	}
}

