function initClient()
{
	%dashes = "";
	%version = atoi($Version);
	%version = mClampF(%version, 0, 25);
	for (%i = 0; %i < %version; %i++)
	{
		%dashes = %dashes @ "-";
	}
	echo("\n--------- Initializing Base: Client " @ %dashes);
	$Server::Dedicated = 0;
	$Client::GameTypeQuery = "Blockland";
	$Client::MissionTypeQuery = "Any";
	initBaseClient();
	initCanvas("Blockland");
	exec("./scripts/allClientScripts.cs");
	exec("base/client/ui/allClientGuis.gui");
	if (isFile("config/client/config.cs"))
	{
		exec("config/client/config.cs");
	}
	JoinServerGui.lastQueryTime = 0;
	echo("\n--------- Loading Client Add-Ons ---------");
	loadClientAddOns();
	$numClientPackages = getNumActivePackages();
	setNetPort(getRandom(64511) + 1024);
	setShadowDetailLevel($pref::shadows);
	setDefaultFov($pref::Player::defaultFov);
	setZoomSpeed($pref::Player::zoomSpeed);
	loadMainMenu();
	BringWindowToForeground();
	schedule(1000, 0, BringWindowToForeground);
	loadTrustList();
	updateTempBrickSettings();
}

function onUDPFailure()
{
	schedule(100, 0, setNetPort, getRandom(64511) + 1024);
}

function loadMainMenu()
{
	Canvas.setContent(MainMenuGui);
	Canvas.setCursor("DefaultCursor");
}

function convertFile(%inFileName, %outFileName)
{
	if (getBuildString() !$= "Debug" && getBuildString() !$= "Release")
	{
		return;
	}
	if (!isFile(%inFileName))
	{
		return;
	}
	%outFile = new FileObject();
	%outFile.openForWrite(%outFileName);
	%file = new FileObject();
	%file.openForRead(%inFileName);
	%buff = "";
	for (%line = %file.readLine(); !%file.isEOF(); %buff = %buff @ %line @ " ")
	{
		%line = %file.readLine();
		%line = trim(%line);
		%commentPos = strpos(%line, "//");
		if (%commentPos == 0)
		{
			continue;
		}
		else if (%commentPos > -1)
		{
			%line = getSubStr(%line, 0, %commentPos);
		}
		%line = strreplace(%line, "\t", " ");
		for (%line = trim(%line); 1; %line = strreplace(%line, "  ", " "))
		{
			if (strpos(%line, "  ") == -1)
			{
				break;
			}
		}
	}
	%outFile.writeLine(%buff);
	%file.close();
	%file.delete();
	%outFile.close();
	%outFile.delete();
}

$ArrangedActive = 0;
$ArrangedAddyCount = 0;
function notifyArrangedStart(%addy)
{
	if (!isObject(ServerGroup))
	{
		%timeDelta = getSimTime() - $arrangedConnectionRequestTime;
		if (%timeDelta > 5000 || %timeDelta < 0)
		{
			warn("Warning: notifyArrangedStart() - got notify without making a request");
			return;
		}
	}
	%addy = strreplace(%addy, "IP:", "");
	$ArrangedActive = 1;
	$ArrangedAddyCount = 0;
}

function notifyArrangedAddress(%addy)
{
	if (!$ArrangedActive)
	{
		echo("Got notifyArrangedAddress when no arranged connection active.");
		return;
	}
	$ArrangedAddyCount = mFloor($ArrangedAddyCount);
	$ArrangedAddys[$ArrangedAddyCount] = %addy;
	$ArrangedAddyCount++;
}

function notifyArrangedFinish(%nonceA, %nonceB, %spamConnect)
{
	if (!$ArrangedActive)
	{
		echo("Got notifyArrangedFinish when no arranged connection active.");
		return;
	}
	$ArrangedActive = 0;
	$ArrangedConnection = new GameConnection();
	if (isObject(ServerGroup))
	{
		%isClient = 0;
	}
	else
	{
		%isClient = 1;
		%spamConnect = 0;
		Connecting_Text.setText(Connecting_Text.getText() @ "\nStarting arranged connection...");
	}
	if ($ArrangedAddyCount == 1)
	{
		$ArrangedConnection.connectArranged(%spamConnect, %isClient, %nonceA, %nonceB, $ArrangedAddys[0]);
		return;
	}
	else if ($ArrangedAddyCount == 2)
	{
		$ArrangedConnection.connectArranged(%spamConnect, %isClient, %nonceA, %nonceB, $ArrangedAddys[0], $ArrangedAddys[1]);
		return;
	}
	else if ($ArrangedAddyCount == 3)
	{
		$ArrangedConnection.connectArranged(%spamConnect, %isClient, %nonceA, %nonceB, $ArrangedAddys[0], $ArrangedAddys[1], $ArrangedAddys[2]);
		return;
	}
	else if ($ArrangedAddyCount == 4)
	{
		$ArrangedConnection.connectArranged(%spamConnect, %isClient, %nonceA, %nonceB, $ArrangedAddys[0], $ArrangedAddys[1], $ArrangedAddys[2], $ArrangedAddys[3]);
		return;
	}
	else if ($ArrangedAddyCount == 5)
	{
		$ArrangedConnection.connectArranged(%spamConnect, %isClient, %nonceA, %nonceB, $ArrangedAddys[0], $ArrangedAddys[1], $ArrangedAddys[2], $ArrangedAddys[3], $ArrangedAddys[4]);
		return;
	}
	else if ($ArrangedAddyCount == 6)
	{
		$ArrangedConnection.connectArranged(%spamConnect, %isClient, %nonceA, %nonceB, $ArrangedAddys[0], $ArrangedAddys[1], $ArrangedAddys[2], $ArrangedAddys[3], $ArrangedAddys[4], $ArrangedAddys[5]);
		return;
	}
	else if ($ArrangedAddyCount == 7)
	{
		$ArrangedConnection.connectArranged(%spamConnect, %isClient, %nonceA, %nonceB, $ArrangedAddys[0], $ArrangedAddys[1], $ArrangedAddys[2], $ArrangedAddys[3], $ArrangedAddys[4], $ArrangedAddys[5], $ArrangedAddys[6]);
		return;
	}
	else if ($ArrangedAddyCount == 8)
	{
		$ArrangedConnection.connectArranged(%spamConnect, %isClient, %nonceA, %nonceB, $ArrangedAddys[0], $ArrangedAddys[1], $ArrangedAddys[2], $ArrangedAddys[3], $ArrangedAddys[4], $ArrangedAddys[5], $ArrangedAddys[6], $ArrangedAddys[7]);
		return;
	}
	else if ($ArrangedAddyCount == 9)
	{
		$ArrangedConnection.connectArranged(%spamConnect, %isClient, %nonceA, %nonceB, $ArrangedAddys[0], $ArrangedAddys[1], $ArrangedAddys[2], $ArrangedAddys[3], $ArrangedAddys[4], $ArrangedAddys[5], $ArrangedAddys[6], $ArrangedAddys[7], $ArrangedAddys[8]);
		return;
	}
	else if ($ArrangedAddyCount == 10)
	{
		$ArrangedConnection.connectArranged(%spamConnect, %isClient, %nonceA, %nonceB, $ArrangedAddys[0], $ArrangedAddys[1], $ArrangedAddys[2], $ArrangedAddys[3], $ArrangedAddys[4], $ArrangedAddys[5], $ArrangedAddys[6], $ArrangedAddys[7], $ArrangedAddys[8], $ArrangedAddys[9]);
		return;
	}
	error("notifyArrangedFinish - Failed to call with addyCount = " @ $ArrangedAddyCount);
}

function onSendPunchPacket(%ip)
{
	if (isObject(Connecting_Text))
	{
		Connecting_Text.setText(Connecting_Text.getText() @ "\nSending punch packet...");
	}
	else
	{
		echo("Sending punch packet to " @ %ip);
	}
}

