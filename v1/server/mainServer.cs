$WebCom_PostSchedule = 0;
function WebCom_PostServer()
{
	if ($Server::LAN)
	{
		return;
	}
	if (!$Server::Port)
	{
		return;
	}
	if (!$missionRunning)
	{
		return;
	}
	echo("Posting to master server");
	if (isEventPending($WebCom_PostSchedule))
	{
		cancel($WebCom_PostSchedule);
	}
	if (isObject(postServerTCPObj))
	{
		postServerTCPObj.delete();
	}
	new TCPObject(postServerTCPObj);
	postServerTCPObj.site = "master.blockland.us";
	postServerTCPObj.port = 80;
	postServerTCPObj.filePath = "/master/postServer.asp";
	%urlEncName = urlEnc($Pref::Server::Name);
	%urlEncMissionName = urlEnc($Server::MissionName);
	%urlEncModPaths = getMainMod();
	if ($Pref::Server::Password !$= "")
	{
		%passworded = "True";
	}
	else
	{
		%passworded = "False";
	}
	if ($Server::Dedicated)
	{
		%dedicated = "True";
	}
	else
	{
		%dedicated = "False";
	}
	%postText = "ServerName=" @ %urlEncName;
	%postText = %postText @ "&Port=" @ mFloor($Server::Port);
	%postText = %postText @ "&Players=" @ mFloor($server::playercount);
	%postText = %postText @ "&MaxPlayers=" @ mFloor($Pref::Server::MaxPlayers);
	%postText = %postText @ "&Map=" @ %urlEncMissionName;
	%postText = %postText @ "&Mod=" @ %urlEncModPaths;
	%postText = %postText @ "&Passworded=" @ %passworded;
	%postText = %postText @ "&Dedicated=" @ %dedicated;
	%postText = %postText @ "&BrickCount=" @ mFloor($Server::BrickCount);
	%postText = %postText @ "&DemoPlayers=" @ mFloor($Pref::Server::AllowDemoPlayers);
	%postTexLen = strlen(%postText);
	postServerTCPObj.cmd = "POST " @ postServerTCPObj.filePath @ " HTTP/1.1\r\nHost: " @ postServerTCPObj.site @ "\r\nContent-Type: application/x-www-form-urlencoded\r\nContent-Length: " @ %postTexLen @ "\r\n\r\n" @ %postText @ "\r\n";
	postServerTCPObj.connect(postServerTCPObj.site @ ":" @ postServerTCPObj.port);
	$WebCom_PostSchedule = schedule(2 * 60 * 1000, 0, WebCom_PostServer);
	%count = mainBrickGroup.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		%brickGroup = mainBrickGroup.getObject(%i);
		if (%brickGroup.isPublicDomain)
		{
		}
		else if (%brickGroup.hasUser())
		{
		}
		else
		{
			%brickGroup.abandonedTime += 2;
			if (%brickGroup.abandonedTime >= $Pref::Server::BrickPublicDomainTimeout)
			{
				%brickGroup.isPublicDomain = 1;
			}
		}
	}
}

function getMainMod()
{
	%modPaths = getModPaths();
	%modPaths = strreplace(%modPaths, ";", "\t");
	%count = getFieldCount(%modPaths);
	%bestMod = "base";
	for (%i = 0; %i < %count; %i++)
	{
		%field = getField(%modPaths, %i);
		if (%field $= "base")
		{
		}
		else if (%field $= "screenshots")
		{
		}
		else if (%field $= "editor")
		{
		}
		else if (%field $= "Add-ons")
		{
		}
		else
		{
			%bestMod = %field;
		}
	}
	return urlEnc(%bestMod);
}

function postServerTCPObj::onConnected(%this)
{
	%this.send(%this.cmd);
}

function postServerTCPObj::onLine(%this, %line)
{
	%word = getWord(%line, 0);
	if (%word $= "FAIL")
	{
		if (strpos(%line, "no host") != -1)
		{
			echo("Error: No host entry in master server, re-sending authentication request");
			auth_Init();
		}
	}
}

function urlEnc(%string)
{
	%string = strreplace(%string, " ", "%20");
	%string = strreplace(%string, "$", "%24");
	%string = strreplace(%string, "&", "%26");
	%string = strreplace(%string, "+", "%2B");
	%string = strreplace(%string, ",", "%2C");
	%string = strreplace(%string, "/", "%2F");
	%string = strreplace(%string, ":", "%3A");
	%string = strreplace(%string, ";", "%3B");
	%string = strreplace(%string, "=", "%3D");
	%string = strreplace(%string, "?", "%3F");
	%string = strreplace(%string, "@", "%40");
	return %string;
}

function GameConnection::authCheck(%client)
{
	echo("AUTHCHECK: ", %client, " ", %client.name);
	if ($Server::LAN)
	{
		%client.name = %client.LANname;
		if (%client.isLan())
		{
			echo("    LAN server, LAN client, loading");
			%client.bl_id = "LAN";
			%client.startLoad(%client);
			return;
		}
		else
		{
			echo("   Internet Client connecting to LAN game, rejecting");
			%client.delete();
			return;
		}
	}
	else
	{
		%client.name = %client.netName;
		if (%client.isLan())
		{
			echo("    LAN client connecting to internet server, authenticating with server ip");
			%useServerIP = 1;
		}
		else
		{
			echo("    Internet client connecting to internet server, regular authentication");
			%useServerIP = 0;
		}
	}
	if (isObject(%client.tcpObj))
	{
		%client.tcpObj.delete();
	}
	%tcp = new TCPObject(servAuthTCPobj);
	%tcp.site = "master.blockland.us";
	%tcp.port = 80;
	%tcp.filePath = "/auth/authQuery.asp";
	%tcp.retryCount = 0;
	%postText = "NAME=" @ urlEnc(%client.name);
	if (!%useServerIP)
	{
		%postText = %postText @ "&IP=" @ getRawIP(%client);
	}
	%postTexLen = strlen(%postText);
	%tcp.cmd = "POST " @ %tcp.filePath @ " HTTP/1.1\r\nHost: " @ %tcp.site @ "\r\nContent-Type: application/x-www-form-urlencoded\r\nContent-Length: " @ %postTexLen @ "\r\n\r\n" @ %postText @ "\r\n";
	%tcp.connect(%tcp.site @ ":" @ %tcp.port);
	%client.tcpObj = %tcp;
	%tcp.client = %client;
}

function GameConnection::startLoad(%client)
{
	sendLoadInfoToClient(%client);
	if (%client.getAddress() $= "local")
	{
		%client.isAdmin = 1;
		%client.isSuperAdmin = 1;
	}
	else
	{
		%client.isAdmin = 0;
		%client.isSuperAdmin = 0;
	}
	%client.gender = "Male";
	%client.score = 0;
	$instantGroup = ServerGroup;
	$instantGroup = MissionCleanup;
	echo("CADD: " @ %client @ " " @ %client.getAddress());
	echo(" +- bl_id = ", %client.bl_id);
	%autoAdmin = %client.autoAdminCheck();
	%count = ClientGroup.getCount();
	for (%cl = 0; %cl < %count; %cl++)
	{
		%other = ClientGroup.getObject(%cl);
		if (%other != %client)
		{
			messageClient(%client, 'MsgClientJoin', "", %other.name, %other, %other.bl_id, %other.score, %other.isAIControlled(), %other.isAdmin, %other.isSuperAdmin);
		}
	}
	serverCmdRequestMiniGameList(%client);
	$Pref::Server::WelcomeMessage = strreplace($Pref::Server::WelcomeMessage, ";", "");
	eval("%taggedMessage = '" @ $Pref::Server::WelcomeMessage @ "';");
	messageClient(%client, 'MsgClientJoin', %taggedMessage, %client.name, %client, %client.bl_id, %client.score, %client.isAIControlled(), %client.isAdmin, %client.isSuperAdmin);
	messageAllExcept(%client, -1, 'MsgClientJoin', '\c1%1 connected.', %client.name, %client, %client.bl_id, %client.score, %client.isAIControlled(), %client.isAdmin, %client.isSuperAdmin);
	if (%autoAdmin == 0)
	{
		echo(" +- no auto admin");
	}
	else if (%autoAdmin == 1)
	{
		MessageAll('MsgAdminForce', '\c2%1 has become Admin (Auto)', %client.name);
		echo(" +- AUTO ADMIN");
	}
	else if (%autoAdmin == 2)
	{
		MessageAll('MsgAdminForce', '\c2%1 has become Super Admin (Auto)', %client.name);
		echo(" +- AUTO SUPER ADMIN");
	}
	if (%client.bl_id $= "")
	{
		error("ERROR: GameConnection::onClientEnterGame() - Client has no bl_id");
	}
	else if (isObject("BrickGroup_" @ %client.bl_id))
	{
		%obj = "BrickGroup_" @ %client.bl_id;
		%client.brickGroup = %obj.getId();
		%client.brickGroup.isPublicDomain = 0;
		%client.brickGroup.abandonedTime = 0;
		%client.brickGroup.name = %client.name;
		%client.brickGroup.client = %client;
	}
	else
	{
		%client.brickGroup = new SimGroup("BrickGroup_" @ %client.bl_id);
		mainBrickGroup.add(%client.brickGroup);
		%client.brickGroup.client = %client;
		%client.brickGroup.name = %client.name;
		%client.brickGroup.bl_id = %client.bl_id;
	}
	%client.InitializeTrustListUpload();
	if ($missionRunning)
	{
		%client.loadMission();
	}
	WebCom_PostServer();
}

function GameConnection::autoAdminCheck(%client)
{
	%ourBL_ID = %client.bl_id;
	%count = getWordCount($Pref::Server::AutoSuperAdminList);
	for (%i = 0; %i < %count; %i++)
	{
		%checkBL_ID = getWord($Pref::Server::AutoSuperAdminList, %i);
		if (%ourBL_ID $= %checkBL_ID)
		{
			%client.isSuperAdmin = 1;
			%client.isAdmin = 1;
			return 2;
		}
	}
	%count = getWordCount($Pref::Server::AutoAdminList);
	for (%i = 0; %i < %count; %i++)
	{
		%checkBL_ID = getWord($Pref::Server::AutoAdminList, %i);
		if (%ourBL_ID $= %checkBL_ID)
		{
			%client.isAdmin = 1;
			return 1;
		}
	}
	return 0;
}

function GameConnection::isLocal(%client)
{
	%ip = getRawIP(%client);
	if (%ip $= "local" || %ip $= "127.0.0.1")
	{
		return 1;
	}
	else
	{
		return 0;
	}
}

function GameConnection::isLan(%client)
{
	%ip = getRawIP(%client);
	if (%ip $= "local" || %ip $= "127.0.0.1")
	{
		return 1;
	}
	%posA = strpos(%ip, ".");
	%numA = mFloor(getSubStr(%ip, 0, %posA));
	%posA++;
	%posB = strpos(%ip, ".", %posA);
	%numB = mFloor(getSubStr(%ip, %posA, %posB - %posA));
	if (%numA == 5)
	{
		return 1;
	}
	if (%numA == 10)
	{
		return 1;
	}
	if (%numA == 192 && %numB == 168)
	{
		return 1;
	}
	return 0;
}

function GameConnection::killDupes(%client)
{
	%count = ClientGroup.getCount();
	for (%clientIndex = 0; %clientIndex < %count; %clientIndex++)
	{
		%cl = ClientGroup.getObject(%clientIndex);
	}
}

function servAuthTCPobj::onDNSFailed(%this)
{
	%this.retryCount++;
	%maxRetries = 3;
	if (%this.retryCount > %maxRetries)
	{
		if (%this.client.isLocal())
		{
			shutDown("Authentication failed for server host.");
			disconnect();
			MessageBoxOK("Server Shut Down", "Authentication DNS Failed.");
			error("ERROR: - Authentication DNS Failed For Host.   GIVING UP");
		}
		else
		{
			%this.client.delete("Authentication DNS Failed");
			error("ERROR: - Authentication DNS Failed.   GIVING UP");
		}
	}
	else if (%this.client.isLocal() && !%this.client.hasAuthedOnce)
	{
		error("ERROR: - Authentication DNS Failed when attempting to host.");
		MessageBoxOK("Cannot Host Internet Game", "Authentication DNS Failed.");
	}
	else
	{
		%this.connect(%this.site @ ":" @ %this.port);
		error("ERROR: - Authentication DNS Failed.  Retry ", %this.retryCount);
	}
}

function servAuthTCPobj::onConnectFailed(%this)
{
	%this.retryCount++;
	%maxRetries = 3;
	if (%this.client.isLocal() && !%this.client.hasAuthedOnce)
	{
		%maxRetries = 1;
	}
	if (%this.retryCount > %maxRetries)
	{
		if (%this.client.isLocal())
		{
			shutDown("Authentication failed for server host.");
			disconnect();
			MessageBoxOK("Server Shut Down", "Authentication Connection Failed.");
		}
		else
		{
			%this.client.delete("Authentication Connection Failed");
			error("ERROR: - Authentication Connection Failed.  GIVING UP");
		}
	}
	else if (%this.client.isLocal() && !%this.client.hasAuthedOnce)
	{
		error("ERROR: - Authentication Connection Failed when attempting to host.");
		MessageBoxOK("Cannot Host Internet Game", "Authentication Connection Failed.");
	}
	else
	{
		%this.connect(%this.site @ ":" @ %this.port);
		error("ERROR: - Authentication Connection Failed.  Retry ", %this.retryCount);
	}
}

function servAuthTCPobj::onConnected(%this)
{
	%this.send(%this.cmd);
}

function servAuthTCPobj::onLine(%this, %line)
{
	if (getWord(%line, 0) $= "NO")
	{
		if (isObject(%this.client))
		{
			if (%this.client.hasAuthedOnce)
			{
				MessageAll('', '\c2%1 Authentication Failed (%2).', %this.client.name, getRawIP(%this.client));
			}
			echo(" Authentication Failed for " @ %this.client.name @ " (" @ getRawIP(%this.client) @ ").");
			if (%this.client.isLocal())
			{
				shutDown("Authentication failed for server host.");
				disconnect();
				MessageBoxOK("Server Shut Down", "Authentication Failed.");
			}
			else
			{
				%this.client.delete("Authentication Failed.");
			}
		}
		else
		{
			error("ERROR: servAuthTCPobj::onLine() - Orphan tcp object ", %this);
		}
	}
	if (getWord(%line, 0) $= "YES")
	{
		%this.client.bl_id = getWord(%line, 1);
		%reason = $BanManagerSO.isBanned(%this.client.bl_id);
		if (%reason)
		{
			%reason = getField(%reason, 1);
			echo("BL_ID " @ %this.client.bl_id @ " is banned, rejecting");
			%this.client.isBanReject = 1;
			%this.client.delete("\n\nYou are banned from this server.\nReason: " @ %reason);
			return;
		}
		if (!%this.client.hasAuthedOnce)
		{
			echo("Auth Init Successfull: " @ %this.client.name);
			%this.client.hasAuthedOnce = 1;
			%this.client.startLoad();
			%this.client.killDupes();
			%this.client.schedule(60 * 1000 * 5, authCheck);
		}
		else
		{
			echo("Auth Continue Successfull: " @ %this.client.name);
			%this.client.schedule(60 * 1000 * 5, authCheck);
		}
	}
}

function ServerPlay2D(%profile)
{
	for (%idx = 0; %idx < ClientGroup.getCount(); %idx++)
	{
		ClientGroup.getObject(%idx).play2D(%profile);
	}
}

function ServerPlay3D(%profile, %transform)
{
	for (%idx = 0; %idx < ClientGroup.getCount(); %idx++)
	{
		ClientGroup.getObject(%idx).play3D(%profile, %transform);
	}
}

function portInit(%port)
{
	for (%failCount = 0; %failCount < 10 && !setNetPort(%port); %failCount++)
	{
		echo("Port init failed on port " @ %port @ " trying next port.");
		%port++;
	}
	$Server::Port = %port;
}

function createServer(%serverType, %mission)
{
	if (%mission $= "")
	{
		error("ERROR: createServer() - mission name unspecified");
		return;
	}
	destroyServer();
	$missionSequence = 0;
	$server::playercount = 0;
	$Server::ServerType = %serverType;
	if (%serverType $= "SinglePlayer" && $Server::Dedicated)
	{
		error("ERROR: createServer() - SinglePlayer mode specified for dedicated server");
		%serverType = "LAN";
	}
	if (%serverType $= "SinglePlayer")
	{
		echo("Starting Single Player Server");
		$Server::LAN = 1;
		portInit(0);
		allowConnections(0);
	}
	else if (%serverType $= "LAN")
	{
		echo("Starting LAN Server");
		$Server::LAN = 1;
		portInit(28050);
		allowConnections(1);
	}
	else if (%serverType $= "Internet")
	{
		echo("Starting Internet Server");
		$Server::LAN = 0;
		if (mFloor($Pref::Server::Port) <= 0)
		{
			$Pref::Server::Port = 28000;
		}
		if ($portArg)
		{
			portInit($portArg);
		}
		else
		{
			portInit($Pref::Server::Port);
		}
		allowConnections(1);
	}
	$ServerGroup = new SimGroup(ServerGroup);
	onServerCreated();
	loadMission(%mission, 1);
	$IamAdmin = 1;
}

function destroyServer()
{
	if ($Server::LAN)
	{
		echo("Destroying LAN Server");
	}
	else
	{
		echo("Destroying NET Server");
	}
	$Server::ServerType = "";
	allowConnections(0);
	stopHeartbeat();
	$missionRunning = 0;
	endMission();
	onServerDestroyed();
	if (isObject(MissionGroup))
	{
		MissionGroup.delete();
	}
	if (isObject(MissionCleanup))
	{
		MissionCleanup.delete();
	}
	if (isObject($ServerGroup))
	{
		$ServerGroup.delete();
	}
	while (ClientGroup.getCount())
	{
		%client = ClientGroup.getObject(0);
		%client.delete();
	}
	if (isEventPending($WebCom_PostSchedule))
	{
		cancel($WebCom_PostSchedule);
	}
	$Server::GuidList = "";
	deleteDataBlocks();
	echo("Exporting server prefs...");
	export("$Pref::Server::*", "~/config/server/prefs.cs", 0);
	export("$Pref::Net::PacketRateToClient", "~/config/server/prefs.cs", True);
	export("$Pref::Net::PacketRateToServer", "~/config/server/prefs.cs", True);
	export("$Pref::Net::PacketSize", "~/config/server/prefs.cs", True);
	export("$Pref::Net::LagThreshold", "~/config/server/prefs.cs", True);
	purgeResources();
}

function resetServerDefaults()
{
	echo("Resetting server defaults...");
	exec("~/server/defaults.cs");
	exec("~/config/server/prefs.cs");
	loadMission($Server::MissionFile);
}

function addToServerGuidList(%guid)
{
	%count = getFieldCount($Server::GuidList);
	for (%i = 0; %i < %count; %i++)
	{
		if (getField($Server::GuidList, %i) == %guid)
		{
			return;
		}
	}
	$Server::GuidList = $Server::GuidList $= "" ? %guid : $Server::GuidList TAB %guid;
}

function removeFromServerGuidList(%guid)
{
	%count = getFieldCount($Server::GuidList);
	for (%i = 0; %i < %count; %i++)
	{
		if (getField($Server::GuidList, %i) == %guid)
		{
			$Server::GuidList = removeField($Server::GuidList, %i);
			return;
		}
	}
}

function onServerInfoQuery()
{
	return "Doing Ok";
}

function messageClient(%client, %msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10, %a11, %a12, %a13)
{
	commandToClient(%client, 'ServerMessage', %msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10, %a11, %a12, %a13);
}

function messageTeam(%team, %msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10, %a11, %a12, %a13)
{
	%count = ClientGroup.getCount();
	for (%cl = 0; %cl < %count; %cl++)
	{
		%recipient = ClientGroup.getObject(%cl);
		if (%recipient.team == %team)
		{
			messageClient(%recipient, %msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10, %a11, %a12, %a13);
		}
	}
}

function messageTeamExcept(%client, %msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10, %a11, %a12, %a13)
{
	%team = %client.team;
	%count = ClientGroup.getCount();
	for (%cl = 0; %cl < %count; %cl++)
	{
		%recipient = ClientGroup.getObject(%cl);
		if (%recipient.team == %team && %recipient != %client)
		{
			messageClient(%recipient, %msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10, %a11, %a12, %a13);
		}
	}
}

function MessageAll(%msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10, %a11, %a12, %a13)
{
	%count = ClientGroup.getCount();
	for (%cl = 0; %cl < %count; %cl++)
	{
		%client = ClientGroup.getObject(%cl);
		messageClient(%client, %msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10, %a11, %a12, %a13);
	}
}

function messageAllExcept(%client, %team, %msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10, %a11, %a12, %a13)
{
	%count = ClientGroup.getCount();
	for (%cl = 0; %cl < %count; %cl++)
	{
		%recipient = ClientGroup.getObject(%cl);
		if (%recipient != %client && %recipient.team != %team)
		{
			messageClient(%recipient, %msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10, %a11, %a12, %a13);
		}
	}
}

function commandToAll(%cmd, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10, %a11, %a12, %a13)
{
	%count = ClientGroup.getCount();
	for (%cl = 0; %cl < %count; %cl++)
	{
		%recipient = ClientGroup.getObject(%cl);
		commandToClient(%recipient, %cmd, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10, %a11, %a12, %a13);
	}
}

function commandToAllExcept(%client, %cmd, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10, %a11, %a12, %a13)
{
	%count = ClientGroup.getCount();
	for (%cl = 0; %cl < %count; %cl++)
	{
		%recipient = ClientGroup.getObject(%cl);
		if (%recipient != %client)
		{
			commandToClient(%recipient, %cmd, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10, %a11, %a12, %a13);
		}
	}
}

$SPAM_PROTECTION_PERIOD = 10000;
$SPAM_MESSAGE_THRESHOLD = 4;
$SPAM_PENALTY_PERIOD = 10000;
$SPAM_MESSAGE = '\c3FLOOD PROTECTION:\cr You must wait another %1 seconds.';
function GameConnection::spamMessageTimeout(%this)
{
	if (%this.spamMessageCount > 0)
	{
		%this.spamMessageCount--;
	}
}

function GameConnection::spamReset(%this)
{
	%this.isSpamming = 0;
}

function spamAlert(%client)
{
	if (%client.isAdmin)
	{
		return 0;
	}
	if ($Pref::Server::FloodProtectionEnabled != 1)
	{
		return 0;
	}
	if (!%client.isSpamming && %client.spamMessageCount >= $SPAM_MESSAGE_THRESHOLD)
	{
		%client.spamProtectStart = getSimTime();
		%client.isSpamming = 1;
		%client.schedule($SPAM_PENALTY_PERIOD, spamReset);
	}
	if (%client.isSpamming)
	{
		%wait = mFloor(($SPAM_PENALTY_PERIOD - (getSimTime() - %client.spamProtectStart)) / 1000);
		messageClient(%client, "", $SPAM_MESSAGE, %wait);
		return 1;
	}
	%client.spamMessageCount++;
	%client.schedule($SPAM_PROTECTION_PERIOD, spamMessageTimeout);
	return 0;
}

function chatMessageClient(%client, %sender, %voiceTag, %voicePitch, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10)
{
	if (!%client.muted[%sender])
	{
		commandToClient(%client, 'ChatMessage', %sender, %voiceTag, %voicePitch, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10);
	}
}

function chatMessageTeam(%sender, %team, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10)
{
	%mg = %sender.miniGame;
	if (isObject(%mg))
	{
		%mg.chatMessageAll(%sender, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10);
	}
	else
	{
		messageClient(%sender, '', '\c5Team chat disabled - You are not in a mini-game.');
	}
	return;
	if (%msgString $= "" || spamAlert(%sender))
	{
		return;
	}
	%count = ClientGroup.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		%obj = ClientGroup.getObject(%i);
		if (%obj.team == %sender.team)
		{
			chatMessageClient(%obj, %sender, %sender.voiceTag, %sender.voicePitch, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10);
		}
	}
}

function chatMessageAll(%sender, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10)
{
	if (%msgString $= "" || spamAlert(%sender))
	{
		return;
	}
	%count = ClientGroup.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		%obj = ClientGroup.getObject(%i);
		if (%sender.team != 0)
		{
			chatMessageClient(%obj, %sender, %sender.voiceTag, %sender.voicePitch, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10);
		}
		else if (%obj.team == %sender.team)
		{
			chatMessageClient(%obj, %sender, %sender.voiceTag, %sender.voicePitch, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10);
		}
	}
}

function serverCmdSAD(%client, %password)
{
	echo("Admin attempt by ", %client.name, " BL_ID:", %client.bl_id, " IP:", getRawIP(%client));
	if (%client.bl_id $= "" || %client.bl_id == -1)
	{
		echo("--Failure - Demo players cannot be admin");
		return;
	}
	if (%password $= "")
	{
		return;
	}
	%success = 0;
	if (%password $= $Pref::Server::SuperAdminPassword)
	{
		if (%client.isSuperAdmin)
		{
			return;
		}
		%client.isAdmin = 1;
		%client.isSuperAdmin = 1;
		%success = 1;
		MessageAll('MsgAdminForce', '\c2%1 has become Super Admin (Password)', %client.name);
		echo("--Success! - SUPER ADMIN");
	}
	else if (%password $= $Pref::Server::AdminPassword)
	{
		if (%client.isAdmin)
		{
			return;
		}
		%client.isAdmin = 1;
		%client.isSuperAdmin = 0;
		%success = 1;
		MessageAll('MsgAdminForce', '\c2%1 has become Admin (Password)', %client.name);
		echo("--Success! - ADMIN");
	}
	if (%success)
	{
		MessageAll('MsgClientJoin', '', %client.name, %client, %client.bl_id, %client.score, %client.isAIControlled(), %client.isAdmin, %client.isSuperAdmin);
		commandToClient(%client, 'adminSuccess');
	}
	else
	{
		%client.adminTries++;
		echo("--Failure #", %client.adminTries);
		commandToClient(%client, 'adminFailure');
		if (%client.adminTries > $Game::MaxAdminTries)
		{
			MessageAll('MsgAdminForce', '\c3%1\c2 failed to guess the admin password.', %client.name);
			%client.delete("You guessed wrong.");
		}
	}
}

function serverCmdSADSetPassword(%client, %password)
{
	if (%client.isSuperAdmin)
	{
		$Pref::Server::AdminPassword = %password;
	}
}

function serverCmdTeamMessageSent(%client, %text)
{
	%obj = %client.Player;
	if (isObject(%obj))
	{
		%obj.playThread(3, talk);
		%obj.schedule(strlen(%text) * 50, playThread, 3, root);
	}
	%text = StripMLControlChars(%text);
	%text = trim(%text);
	if ($Pref::Server::MaxChatLen > 0)
	{
		if (strlen(%text) >= $Pref::Server::MaxChatLen)
		{
			%text = getSubStr(%text, 3, $Pref::Server::MaxChatLen);
		}
	}
	%protocolLen = 7;
	%urlStart = strpos(%text, "http://");
	if (%urlStart == -1)
	{
		%protocolLen = 8;
		%urlStart = strpos(%text, "https://");
	}
	if (%urlStart == -1)
	{
		%protocolLen = 6;
		%urlStart = strpos(%text, "ftp://");
	}
	if (%urlStart != -1)
	{
		%urlEnd = strpos(%text, " ", %urlStart + 1);
		if (%urlEnd == -1)
		{
			%fullUrl = getSubStr(%text, %urlStart, strlen(%text) - %urlStart);
			%url = getSubStr(%text, %urlStart + %protocolLen, (strlen(%text) - %urlStart) - %protocolLen);
		}
		else
		{
			%fullUrl = getSubStr(%text, %urlStart, %urlEnd - %urlStart);
			%url = getSubStr(%text, %urlStart + %protocolLen, (%urlEnd - %urlStart) - %protocolLen);
		}
		%text = strreplace(%text, %fullUrl, "<a:" @ %url @ ">" @ %url @ "</a>\c4");
	}
	if ($Pref::Server::CurseFilter)
	{
		if (!chatFilter(%client, %text, $Pref::Server::CurseList, '\c5Do not use foul language.'))
		{
			return 0;
		}
	}
	if ($Pref::Server::ETardFilter)
	{
		if (!chatFilter(%client, %text, $Pref::Server::ETardList, '\c5This is a civilized game.  Please use full words.'))
		{
			return 0;
		}
	}
	if (strlen(%text) <= 0)
	{
		return;
	}
	chatMessageTeam(%client, %client.team, '\c7%1\c3%2\c7%3\c4: %4', %client.clanPrefix, %client.name, %client.clanSuffix, %text);
	echo("(T)", %client.name, ": ", %text);
}

function serverCmdMessageSent(%client, %text)
{
	%obj = %client.Player;
	if (isObject(%obj))
	{
		%obj.playThread(3, talk);
		%obj.schedule(strlen(%text) * 50, playThread, 3, root);
	}
	%text = StripMLControlChars(%text);
	%text = strreplace(%text, "<a:", "");
	%text = trim(%text);
	if ($Pref::Server::MaxChatLen > 0)
	{
		if (strlen(%text) >= $Pref::Server::MaxChatLen)
		{
			%text = getSubStr(%text, 0, $Pref::Server::MaxChatLen);
		}
	}
	%protocolLen = 7;
	%urlStart = strpos(%text, "http://");
	if (%urlStart == -1)
	{
		%protocolLen = 8;
		%urlStart = strpos(%text, "https://");
	}
	if (%urlStart == -1)
	{
		%protocolLen = 6;
		%urlStart = strpos(%text, "ftp://");
	}
	if (%urlStart != -1)
	{
		echo("URL FOUND");
		%urlEnd = strpos(%text, " ", %urlStart + 1);
		if (%urlEnd == -1)
		{
			%fullUrl = getSubStr(%text, %urlStart, strlen(%text) - %urlStart);
			%url = getSubStr(%text, %urlStart + %protocolLen, (strlen(%text) - %urlStart) - %protocolLen);
		}
		else
		{
			%fullUrl = getSubStr(%text, %urlStart, %urlEnd - %urlStart);
			%url = getSubStr(%text, %urlStart + %protocolLen, (%urlEnd - %urlStart) - %protocolLen);
		}
		%newText = strreplace(%text, %fullUrl, "<a:" @ %url @ ">" @ %url @ "</a>\c6");
		%text = %newText;
	}
	if ($Pref::Server::CurseFilter)
	{
		if (!chatFilter(%client, %text, $Pref::Server::CurseList, '\c5Do not use foul language.'))
		{
			return 0;
		}
	}
	if ($Pref::Server::ETardFilter)
	{
		if (!chatFilter(%client, %text, $Pref::Server::ETardList, '\c5This is a civilized game.  Please use full words.'))
		{
			return 0;
		}
	}
	if (strlen(%text) <= 0)
	{
		error("ERROR: null chat");
		return;
	}
	chatMessageAll(%client, '\c7%1\c3%2\c7%3\c6: %4', %client.clanPrefix, %client.name, %client.clanSuffix, %text);
	echo(%client.name, ": ", %text);
}

function chatFilter(%client, %text, %badList, %failMessage)
{
	%lwrText = " " @ strlwr(%text) @ " ";
	%lwrText = strreplace(%lwrText, "?", " ");
	%lwrText = strreplace(%lwrText, "!", " ");
	%lwrText = strreplace(%lwrText, ".", " ");
	%lwrText = strreplace(%lwrText, "/", " ");
	%lastChar = getSubStr(%badList, strlen(%badList) - 1, 1);
	if (%lastChar !$= ",")
	{
		%badList = %badList @ ",";
	}
	%offset = 0;
	%max = strlen(%badList) - 1;
	for (%i = 0; %offset < %max; %offset += %wordLen + 1)
	{
		%i++;
		if (%i >= 1000)
		{
			error("ERROR: chatFilter() - loop safety hit");
			return 1;
		}
		%nextDelim = strpos(%badList, ",", %offset);
		if (%nextDelim == -1)
		{
			%offset = %max;
		}
		%wordLen = %nextDelim - %offset;
		%word = getSubStr(%badList, %offset, %wordLen);
		if (strstr(%lwrText, %word) != -1)
		{
			messageClient(%client, '', %failMessage, %word);
			return 0;
		}
	}
	return 1;
}

function clearLoadInfo()
{
	if (isObject(MissionInfo))
	{
		MissionInfo.delete();
	}
}

function buildLoadInfo(%mission)
{
	clearLoadInfo();
	%infoObject = "";
	%file = new FileObject();
	if (%file.openForRead(%mission))
	{
		%inInfoBlock = 0;
		while (!%file.isEOF())
		{
			%line = %file.readLine();
			%line = trim(%line);
			if (%line $= "new ScriptObject(MissionInfo) {")
			{
				%inInfoBlock = 1;
			}
			else if (%inInfoBlock && %line $= "};")
			{
				%inInfoBlock = 0;
				%infoObject = %infoObject @ %line;
				break;
			}
			if (%inInfoBlock)
			{
				%infoObject = %infoObject @ %line @ " ";
			}
		}
		%file.close();
	}
	eval(%infoObject);
	%file.delete();
}

function dumpLoadInfo()
{
	echo("Mission Name: " @ MissionInfo.name);
	echo("Mission SaveName: " @ MissionInfo.saveName);
	echo("Mission Description:");
	for (%i = 0; MissionInfo.desc[%i] !$= ""; %i++)
	{
		echo("   " @ MissionInfo.desc[%i]);
	}
}

function sendLoadInfoToClient(%client)
{
	messageClient(%client, 'MsgLoadMapPicture', "", MissionInfo.previewImage);
	if (MissionInfo.saveName !$= "")
	{
		messageClient(%client, 'MsgLoadInfo', "", MissionInfo.name, MissionInfo.saveName);
	}
	else
	{
		messageClient(%client, 'MsgLoadInfo', "", MissionInfo.name, "Default");
	}
	for (%i = 0; MissionInfo.desc[%i] !$= ""; %i++)
	{
		messageClient(%client, 'MsgLoadDescripition', "", MissionInfo.desc[%i]);
	}
	messageClient(%client, 'MsgLoadInfoDone');
}

$MissionLoadPause = 2000;
function loadMission(%missionName, %isFirstMission)
{
	endMission();
	echo("*** LOADING MISSION: " @ %missionName);
	echo("*** Stage 1 load");
	clearCenterPrintAll();
	clearBottomPrintAll();
	$missionSequence++;
	$missionRunning = 0;
	$Server::MissionFile = %missionName;
	clearLoadInfo();
	buildLoadInfo(%missionName);
	dumpLoadInfo();
	%count = ClientGroup.getCount();
	for (%cl = 0; %cl < %count; %cl++)
	{
		%client = ClientGroup.getObject(%cl);
		if (!%client.isAIControlled())
		{
			sendLoadInfoToClient(%client);
		}
	}
	if (%isFirstMission || $Server::ServerType $= "SinglePlayer")
	{
		loadMissionStage2();
	}
	else
	{
		schedule($MissionLoadPause, ServerGroup, loadMissionStage2);
	}
}

function loadMissionStage2()
{
	echo("*** Stage 2 load");
	$instantGroup = ServerGroup;
	%file = $Server::MissionFile;
	if (!isFile(%file))
	{
		error("Could not find mission " @ %file);
		return;
	}
	$missionCRC = getFileCRC(%file);
	exec(%file);
	if (!isObject(MissionGroup))
	{
		error("No 'MissionGroup' found in mission \"" @ $missionName @ "\".");
		schedule(3000, ServerGroup, CycleMissions);
		return;
	}
	new SimGroup(MissionCleanup);
	$instantGroup = MissionCleanup;
	new SimGroup(mainBrickGroup);
	MissionCleanup.add(mainBrickGroup);
	%count = ClientGroup.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		%client = ClientGroup.getObject(%i);
		if (%client.bl_id $= "")
		{
			error("ERROR: loadMissionStage2() - Client \"" @ %client.name @ "\"has no bl_id");
		}
		else if (isObject("BrickGroup_" @ %client.bl_id))
		{
			%obj = "BrickGroup_" @ %client.bl_id;
			%client.brickGroup = %obj.getId();
			%client.brickGroup.name = %client.name;
			%client.brickGroup.client = %client;
		}
		else
		{
			%client.brickGroup = new SimGroup("BrickGroup_" @ %client.bl_id);
			mainBrickGroup.add(%client.brickGroup);
			%client.brickGroup.client = %client;
			%client.brickGroup.name = %client.name;
			%client.brickGroup.bl_id = %client.bl_id;
		}
		commandToClient(%client, 'TrustListUpload_Start');
	}
	pathOnMissionLoadDone();
	echo("*** Mission loaded");
	$missionRunning = 1;
	for (%clientIndex = 0; %clientIndex < ClientGroup.getCount(); %clientIndex++)
	{
		ClientGroup.getObject(%clientIndex).loadMission();
	}
	onMissionLoaded();
	purgeResources();
}

function endMission()
{
	if (!isObject(MissionGroup))
	{
		return;
	}
	echo("*** ENDING MISSION");
	onMissionEnded();
	for (%clientIndex = 0; %clientIndex < ClientGroup.getCount(); %clientIndex++)
	{
		%cl = ClientGroup.getObject(%clientIndex);
		%cl.endMission();
		%cl.resetGhosting();
		%cl.clearPaths();
		%cl.hasSpawnedOnce = 0;
	}
	MissionGroup.delete();
	MissionCleanup.delete();
	$ServerGroup.delete();
	$ServerGroup = new SimGroup(ServerGroup);
}

function resetMission()
{
	echo("*** MISSION RESET");
	MissionCleanup.delete();
	$instantGroup = ServerGroup;
	new SimGroup(MissionCleanup);
	$instantGroup = MissionCleanup;
	onMissionReset();
}

function GameConnection::loadMission(%this)
{
	%this.currentPhase = 0;
	if (%this.isAIControlled())
	{
		%this.onClientEnterGame();
	}
	else
	{
		commandToClient(%this, 'MissionStartPhase1', $missionSequence, $Server::MissionFile, MissionGroup.musicTrack);
		echo("*** Sending mission load to client: " @ $Server::MissionFile);
	}
}

function serverCmdMissionStartPhase1Ack(%client, %seq)
{
	if (%seq != $missionSequence || !$missionRunning)
	{
		return;
	}
	if (%client.currentPhase != 0)
	{
		return;
	}
	%client.currentPhase = 1;
	%client.setMissionCRC($missionCRC);
	%client.transmitDataBlocks($missionSequence);
}

function GameConnection::onDataBlocksDone(%this, %missionSequence)
{
	if (%missionSequence != $missionSequence)
	{
		return;
	}
	if (%this.currentPhase != 1)
	{
		return;
	}
	%this.currentPhase = 1.5;
	commandToClient(%this, 'MissionStartPhase2', $missionSequence, $Server::MissionFile);
}

function serverCmdMissionStartPhase2Ack(%client, %seq)
{
	if (%seq != $missionSequence || !$missionRunning)
	{
		return;
	}
	if (%client.currentPhase != 1.5)
	{
		return;
	}
	%client.currentPhase = 2;
	%client.transmitStaticBrickData();
	%client.transmitPaths();
	%client.activateGhosting();
}

function GameConnection::clientWantsGhostAlwaysRetry(%client)
{
	if ($missionRunning)
	{
		%client.activateGhosting();
	}
}

function GameConnection::onGhostAlwaysFailed(%client)
{
}

function GameConnection::onGhostAlwaysObjectsReceived(%client)
{
	commandToClient(%client, 'MissionStartPhase3', $missionSequence, $Server::MissionFile, $Server::LAN);
}

function serverCmdMissionStartPhase3Ack(%client, %seq)
{
	if (%seq != $missionSequence || !$missionRunning)
	{
		return;
	}
	if (%client.currentPhase != 2)
	{
		return;
	}
	%client.currentPhase = 3;
	%client.startMission();
	%client.onClientEnterGame();
}

function getRawIP(%client)
{
	%rawip = %client.getAddress();
	if (%rawip $= "local")
	{
		return %rawip;
	}
	%finalip = getSubStr(%rawip, 3, 20);
	%portstart = strstr(%finalip, ":");
	%finalip = getSubStr(%finalip, 0, %portstart);
	return %finalip;
}

function GameConnection::onConnectRequest(%client, %netAddress, %LANname, %netName, %clanPrefix, %clanSuffix)
{
	echo("Connect request from: " @ %netAddress);
	echo("  lan name = ", %LANname);
	echo("  net name = ", %netName);
	%client.clanPrefix = trim(getSubStr(StripMLControlChars(%clanPrefix), 0, 4));
	%client.clanSuffix = trim(getSubStr(StripMLControlChars(%clanSuffix), 0, 4));
	%client.LANname = getSubStr(StripMLControlChars(%LANname), 0, 23);
	%client.netName = StripMLControlChars(%netName);
	if ($server::playercount >= $Pref::Server::MaxPlayers && %ip !$= "local")
	{
		return "CR_SERVERFULL";
	}
	return "";
}

function GameConnection::onConnect(%client)
{
	%client.connected = 1;
	commandToClient(%client, 'updatePrefs');
	messageClient(%client, 'MsgConnectionError', "", $Pref::Server::ConnectionError);
	$server::playercount++;
	%client.authCheck();
	return;
}

function GameConnection::setPlayerName(%client, %name)
{
	%name = StripMLControlChars(%name);
	%client.sendGuid = 0;
	%name = stripTrailingSpaces(strToPlayerName(%name));
	if (strlen(%name) < 3)
	{
		%name = "Poser";
	}
	if (!isNameUnique(%client, %name))
	{
		%isUnique = 0;
		for (%suffix = 1; !%isUnique; %suffix++)
		{
			%nameTry = %name @ "." @ %suffix;
			%isUnique = isNameUnique(%client, %nameTry);
		}
		%name = %nameTry;
	}
	%client.nameBase = %name;
}

function isNameUnique(%client, %name)
{
	%count = ClientGroup.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		%test = ClientGroup.getObject(%i);
		if (%client != %test)
		{
			%rawName = stripChars(detag(getTaggedString(%test.name)), "\cp\co\c6\c7\c8\c9");
			if (strcmp(%name, %rawName) == 0)
			{
				return 0;
			}
		}
	}
	return 1;
}

function GameConnection::onDrop(%client, %reason)
{
	if (%client.connected == 1)
	{
		%client.onClientLeaveGame();
		removeFromServerGuidList(%client.guid);
		if (!%client.isBanReject && %client.hasAuthedOnce || $Server::LAN)
		{
			messageAllExcept(%client, -1, 'MsgClientDrop', '\c1%1 has left the game.', %client.name, %client);
		}
		removeTaggedString(%client.name);
		echo("CDROP: " @ %client @ " " @ %client.getAddress());
		$server::playercount--;
		WebCom_PostServer();
		if ($server::playercount == 0 && $Server::Dedicated && $Pref::Server::ResetOnEmpty)
		{
			schedule(0, 0, "resetServerDefaults");
		}
	}
}

function GameConnection::startMission(%this)
{
	commandToClient(%this, 'MissionStart', $missionSequence);
}

function GameConnection::endMission(%this)
{
	commandToClient(%this, 'MissionEnd', $missionSequence);
}

function GameConnection::syncClock(%client, %time)
{
	commandToClient(%client, 'syncClock', %time);
}

function GameConnection::incScore(%this, %delta)
{
	%this.score += %delta;
	MessageAll('MsgClientScoreChanged', "", %this.score, %this);
}

function GameConnection::setScore(%this, %val)
{
	%this.score = %val;
	MessageAll('MsgClientScoreChanged', "", %this.score, %this);
}

function onServerCreated()
{
	$Server::GameType = "Test App";
	$Server::MissionType = "Deathmatch";
	createGame();
}

function onServerDestroyed()
{
	destroyGame();
}

function onMissionLoaded()
{
	if (MissionInfo.saveName $= "")
	{
		error("ERROR: MissionInfo.saveName not specified. \"Default\" will be used.");
		MessageAll('', "ERROR: MissionInfo.saveName not specified. \"Default\" will be used.");
	}
	startGame();
}

function onMissionEnded()
{
	endGame();
}

function onMissionReset()
{
}

function GameConnection::onClientEnterGame(%this)
{
}

function GameConnection::onClientLeaveGame(%this)
{
}

function createGame()
{
}

function destroyGame()
{
}

function startGame()
{
}

function endGame()
{
}

