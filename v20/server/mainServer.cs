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
	if (!isNonsenseVerfied())
	{
		echo("Can't post to master yet, must auth...");
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
	postServerTCPObj.site = "master2.blockland.us";
	postServerTCPObj.port = 80;
	postServerTCPObj.filePath = "/postServer.php";
	%urlEncName = urlEnc($Pref::Server::Name);
	%urlEncMissionName = urlEnc($Server::MissionName);
	%urlEncModPaths = "";
	if ($Pref::Server::Password !$= "")
	{
		%passworded = 1;
	}
	else
	{
		%passworded = 0;
	}
	if ($Server::Dedicated)
	{
		%dedicated = 1;
	}
	else
	{
		%dedicated = 0;
	}
	%postText = "ServerName=" @ %urlEncName;
	%postText = %postText @ "&Port=" @ mFloor($Server::Port);
	%postText = %postText @ "&Players=" @ mFloor($server::playercount);
	%postText = %postText @ "&MaxPlayers=" @ mFloor($Pref::Server::MaxPlayers);
	%postText = %postText @ "&Map=" @ %urlEncMissionName;
	%postText = %postText @ "&Mod=" @ %urlEncModPaths;
	%postText = %postText @ "&Passworded=" @ %passworded;
	%postText = %postText @ "&Dedicated=" @ %dedicated;
	%postText = %postText @ "&BrickCount=" @ mFloor(getBrickCount());
	%postText = %postText @ "&DemoPlayers=" @ mFloor($Pref::Server::AllowDemoPlayers);
	%postText = %postText @ "&blid=" @ urlEnc(getKeyID());
	%postText = %postText @ "&csg=" @ urlEnc(getVersionNumber());
	%postText = %postText @ "&ver=" @ urlEnc($Version);
	%postTexLen = strlen(%postText);
	postServerTCPObj.cmd = "POST " @ postServerTCPObj.filePath @ " HTTP/1.0\r\nHost: " @ postServerTCPObj.site @ "\r\nContent-Type: application/x-www-form-urlencoded\r\nContent-Length: " @ %postTexLen @ "\r\n\r\n" @ %postText @ "\r\n";
	postServerTCPObj.connect(postServerTCPObj.site @ ":" @ postServerTCPObj.port);
	%schduleTime = 5 * 60 * 1000 * getTimeScale();
	$WebCom_PostSchedule = schedule(%schduleTime, 0, WebCom_PostServer);
	if ($Pref::Server::BrickPublicDomainTimeout > 0)
	{
		%elapsedMS = getSimTime() - $Server::lastPostTime;
		%elapsedMinutes = mFloor(%elapsedMS / (1000 * 60));
		if (%elapsedMinutes > 0)
		{
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
					%brickGroup.abandonedTime += %elapsedMinutes;
					if (%brickGroup.abandonedTime >= $Pref::Server::BrickPublicDomainTimeout)
					{
						%brickGroup.isPublicDomain = 1;
					}
				}
			}
			$Server::lastPostTime += %elapsedMinutes * 60 * 1000;
		}
	}
	$Server::lastPostTime = getSimTime();
}

function WebCom_PostServerUpdateLoop()
{
	if (!isEventPending($WebCom_PostSchedule))
	{
		WebCom_PostServer();
		return;
	}
	%timeLeft = getTimeRemaining($WebCom_PostSchedule);
	%schduleTime = 5 * 60 * 1000 * getTimeScale() * 0.2;
	if (%timeLeft > %schduleTime)
	{
		cancel($WebCom_PostSchedule);
		$WebCom_PostSchedule = schedule(%schduleTime, 0, WebCom_PostServer);
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

function postServerTCPObj::onDNSFailed(%this)
{
	echo("Post to master server FAILED: DNS error. Retrying in 5 seconds...");
	%this.disconnect();
	schedule(5000, 0, "WebCom_PostServer");
}

function postServerTCPObj::onConnectFailed(%this)
{
	echo("Post to master server FAILED: Connection failure.  Retrying in 5 seconds...");
	%this.disconnect();
	schedule(5000, 0, "WebCom_PostServer");
}

function postServerTCPObj::onDisconnect(%this)
{
}

function postServerTCPObj::onLine(%this, %line)
{
	%word = getWord(%line, 0);
	if (%word $= "FAIL")
	{
		%reason = getSubStr(%line, 5, 1000);
		if (%reason $= "no host")
		{
			echo("No host entry in master server, re-sending authentication request");
			auth_Init_Server();
		}
		else if (%reason $= "no user, no host")
		{
			echo("No user/host entry in master server, re-sending authentication request");
			auth_Init_Server();
		}
		else
		{
			echo("Posting to master failed.  Reason: " @ %reason);
		}
	}
	else if (%word $= "MMTOK")
	{
		%val = getWord(%line, 1);
		setMatchMakerToken(%val);
	}
}

function GameConnection::authCheck(%client)
{
	if ($Server::LAN)
	{
		%client.setPlayerName("au^timoamyo7zene", %client.LANname);
		%client.name = %client.LANname;
		if (%client.isLan())
		{
			echo("AUTHCHECK: " @ %client.getPlayerName() @ " = LAN client -> LAN server, loading");
			%client.bl_id = getLAN_BLID();
			%client.setBLID("au^timoamyo7zene", getLAN_BLID());
			%client.startLoad(%client);
			return;
		}
		else
		{
			echo("AUTHCHECK: " @ %client.getPlayerName() @ " = internet client -> LAN game, rejecting");
			%client.schedule(10, delete);
			return;
		}
	}
	else
	{
		%client.setPlayerName("au^timoamyo7zene", %client.netName);
		%client.name = %client.netName;
		if (%client.isLan())
		{
			echo("AUTHCHECK: " @ %client.getPlayerName() @ " = LAN client -> internet server, auth with server ip");
			%useServerIP = 1;
		}
		else
		{
			echo("AUTHCHECK: " @ %client.getPlayerName() @ " = internet client -> internet server, regular auth");
			%useServerIP = 0;
		}
	}
	if (isObject(%client.tcpObj))
	{
		%client.tcpObj.delete();
	}
	%tcp = new TCPObject(servAuthTCPobj);
	%tcp.site = "auth.blockland.us";
	%tcp.port = 80;
	%tcp.filePath = "/authQuery.php";
	%tcp.retryCount = 0;
	%postText = "NAME=" @ urlEnc(%client.getPlayerName());
	if (!%useServerIP)
	{
		%postText = %postText @ "&IP=" @ %client.getRawIP();
	}
	%postTexLen = strlen(%postText);
	%tcp.cmd = "POST " @ %tcp.filePath @ " HTTP/1.0\r\nHost: " @ %tcp.site @ "\r\nContent-Type: application/x-www-form-urlencoded\r\nContent-Length: " @ %postTexLen @ "\r\n\r\n" @ %postText @ "\r\n";
	%tcp.connect(%tcp.site @ ":" @ %tcp.port);
	%client.tcpObj = %tcp;
	%tcp.client = %client;
}

function GameConnection::startLoad(%client)
{
	sendLoadInfoToClient(%client);
	commandToClient(%client, 'updatePrefs');
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
	%client.score = 0;
	$instantGroup = ServerGroup;
	$instantGroup = MissionCleanup;
	echo("CADD: " @ %client @ " " @ %client.getAddress());
	echo(" +- bl_id = ", %client.getBLID());
	%autoAdmin = %client.autoAdminCheck();
	%count = ClientGroup.getCount();
	for (%cl = 0; %cl < %count; %cl++)
	{
		%other = ClientGroup.getObject(%cl);
		if (%other != %client)
		{
			secureCommandToClient("mod2maiegut^afoo", %client, 'ClientJoin', %other.getPlayerName(), %other, %other.getBLID(), %other.score, %other.isAIControlled(), %other.isAdmin, %other.isSuperAdmin);
		}
	}
	serverCmdRequestMiniGameList(%client);
	$Pref::Server::WelcomeMessage = strreplace($Pref::Server::WelcomeMessage, ";", "");
	eval("%taggedMessage = '" @ $Pref::Server::WelcomeMessage @ "';");
	messageClient(%client, '', %taggedMessage, %client.getPlayerName());
	messageAllExcept(%client, -1, 'MsgClientJoin', '\c1%1 connected.', %client.getPlayerName());
	secureCommandToAll("mod2maiegut^afoo", 'ClientJoin', %client.getPlayerName(), %client, %client.getBLID(), %client.score, %client.isAIControlled(), %client.isAdmin, %client.isSuperAdmin);
	if (%autoAdmin == 0)
	{
		echo(" +- no auto admin");
	}
	else if (%autoAdmin == 1)
	{
		MessageAll('MsgAdminForce', '\c2%1 has become Admin (Auto)', %client.getPlayerName());
		echo(" +- AUTO ADMIN");
	}
	else if (%autoAdmin == 2)
	{
		MessageAll('MsgAdminForce', '\c2%1 has become Super Admin (Auto)', %client.getPlayerName());
		echo(" +- AUTO SUPER ADMIN (List)");
	}
	else if (%autoAdmin == 3)
	{
		MessageAll('MsgAdminForce', '\c2%1 has become Super Admin (Host)', %client.getPlayerName());
		echo(" +- AUTO SUPER ADMIN (ID same as host)");
	}
	if (%client.getBLID() <= -1)
	{
		error("ERROR: GameConnection::startLoad() - Client has no bl_id");
		%client.schedule(10, delete);
		return;
	}
	else if (isObject("BrickGroup_" @ %client.getBLID()))
	{
		%obj = "BrickGroup_" @ %client.getBLID();
		%client.brickGroup = %obj.getId();
		%client.brickGroup.isPublicDomain = 0;
		%client.brickGroup.abandonedTime = 0;
		%client.brickGroup.name = %client.getPlayerName();
		%client.brickGroup.client = %client;
		%quotaObject = %client.brickGroup.QuotaObject;
		if (isObject(%quotaObject))
		{
			if (isEventPending(%quotaObject.cancelEventsEvent))
			{
				cancel(%quotaObject.cancelEventsEvent);
			}
			if (isEventPending(%quotaObject.cancelProjectilesEvent))
			{
				cancel(%quotaObject.cancelProjectilesEvent);
			}
		}
	}
	else
	{
		%client.brickGroup = new SimGroup("BrickGroup_" @ %client.getBLID());
		mainBrickGroup.add(%client.brickGroup);
		%client.brickGroup.client = %client;
		%client.brickGroup.name = %client.getPlayerName();
		%client.brickGroup.bl_id = %client.getBLID();
	}
	%client.InitializeTrustListUpload();
	if ($missionRunning)
	{
		%client.loadMission();
	}
	if ($server::playercount >= $Pref::Server::MaxPlayers || getSimTime() - $Server::lastPostTime > 30 * 1000 || $Server::lastPostTime < 30 * 1000)
	{
		WebCom_PostServer();
	}
}

function GameConnection::autoAdminCheck(%client)
{
	%ourBL_ID = %client.getBLID();
	if ($Pref::Server::AutoAdminServerOwner)
	{
		if (%ourBL_ID == getNumKeyID())
		{
			%client.isSuperAdmin = 1;
			%client.isAdmin = 1;
			return 3;
		}
	}
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

function GameConnection::killDupes(%client)
{
	%ourIP = %client.getRawIP();
	%ourBLID = %client.getBLID();
	%count = ClientGroup.getCount();
	for (%clientIndex = 0; %clientIndex < %count; %clientIndex++)
	{
		%cl = ClientGroup.getObject(%clientIndex);
		if (%cl == %client)
		{
		}
		else if (%cl.getBLID() !$= %ourBLID)
		{
		}
		else if (%cl.getRawIP() $= %ourIP && %cl.getPlayerName() $= %client.getPlayerName())
		{
		}
		else if (%cl.isLocal() || %cl.isLan())
		{
		}
		else
		{
			%cl.schedule(10, delete, "Someone using your Blockland ID joined the server from a different IP address.");
		}
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
			MessageBoxOK("Authentication Failure", "Authentication DNS Failed.");
			error("ERROR: - Authentication DNS Failed For Host.   GIVING UP");
		}
		else
		{
			%this.client.delete("Authentication DNS Failed");
			error("ERROR: - Authentication DNS Failed.   GIVING UP");
		}
	}
	else if (%this.client.isLocal() && !%this.client.getHasAuthedOnce())
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
	if (%this.client.isLocal() && !%this.client.getHasAuthedOnce())
	{
		%maxRetries = 1;
	}
	if (%this.retryCount > %maxRetries)
	{
		if (%this.client.isLocal())
		{
			shutDown("Authentication failed for server host.");
			MessageBoxOK("Authentication Failure", "Authentication Connection Failed.");
		}
		else
		{
			%this.client.delete("Authentication Connection Failed");
			error("ERROR: - Authentication Connection Failed.  GIVING UP");
		}
	}
	else if (%this.client.isLocal() && !%this.client.getHasAuthedOnce())
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
	%word = getWord(%line, 0);
	if (%word $= "YES")
	{
		%this.client.bl_id = getWord(%line, 1);
		%this.client.setBLID("au^timoamyo7zene", getWord(%line, 1));
		if (%this.client.getBLID() != getNumKeyID())
		{
			%reason = $BanManagerSO.isBanned(%this.client.getBLID());
			if (%reason)
			{
				%reason = getField(%reason, 1);
				echo("BL_ID " @ %this.client.getBLID() @ " is banned, rejecting");
				%this.client.isBanReject = 1;
				%this.client.schedule(10, delete, "\n\nYou are banned from this server.\nReason: " @ %reason);
				return;
			}
		}
		if (!%this.client.getHasAuthedOnce())
		{
			echo("Auth Init Successfull: " @ %this.client.getPlayerName());
			%this.client.setHasAuthedOnce(1);
			%this.client.startLoad();
			%this.client.killDupes();
			%this.client.schedule(60 * 1000 * 5, authCheck);
		}
		else
		{
			echo("Auth Continue Successfull: " @ %this.client.getPlayerName());
			%this.client.schedule(60 * 1000 * 5, authCheck);
		}
	}
	else if (%word $= "NO")
	{
		if (isObject(%this.client))
		{
			if (%this.client.getHasAuthedOnce())
			{
				MessageAll('', '\c2%1 Authentication Failed (%2).', %this.client.getPlayerName(), %this.client.getRawIP());
			}
			echo(" Authentication Failed for " @ %this.client.getPlayerName() @ " (" @ %this.client.getRawIP() @ ").");
			if (%this.client.isLocal())
			{
				shutDown("Authentication failed for server host.");
				disconnect();
				MessageBoxOK("Server Shut Down", "Server shut down - Authentication Failed.");
			}
			else
			{
				%this.client.schedule(10, delete, "Server could not verify your Blockland ID.");
				return;
			}
		}
		else
		{
			error("ERROR: servAuthTCPobj::onLine() - Orphan tcp object ", %this);
		}
	}
	else if (%word $= "ERROR")
	{
		if (isObject(%this.client))
		{
			if (%this.client.getHasAuthedOnce())
			{
				MessageAll('', '\c2%1 Authentication Error (%2).', %this.client.getPlayerName(), %this.client.getRawIP());
			}
			echo(" Authentication Error for " @ %this.client.getPlayerName() @ " (" @ %this.client.getRawIP() @ ").");
			if (%this.client.isLocal())
			{
				shutDown("Authentication error for server host.");
				disconnect();
				MessageBoxOK("Server Shut Down", "Server shut down - Authentication error.");
			}
			else
			{
				%this.client.schedule(10, delete, "Server experienced an authentication error.");
				return;
			}
		}
		else
		{
			error("ERROR: servAuthTCPobj::onLine() - Orphan tcp object ", %this);
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
	if (%port == 280000)
	{
		%port = 28000;
	}
	if (%port == 280001)
	{
		%port = 28001;
	}
	%port = mClampF(%port, 0, 65535);
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
	echo("");
	$missionSequence = 0;
	$server::playercount = 0;
	$Server::ServerType = %serverType;
	if (%serverType $= "SinglePlayer" && $Server::Dedicated)
	{
		error("ERROR: createServer() - SinglePlayer mode specified for dedicated server");
		%serverType = "LAN";
	}
	if ($Server::Dedicated)
	{
		$Con::logBufferEnabled = 0;
		$Physics::enabled = 0;
		$Physics::maxBricks = 0;
	}
	if (%serverType $= "SinglePlayer")
	{
		echo("Starting Single Player Server");
		$Server::LAN = 1;
		portInit(0);
		setAllowConnections(0);
	}
	else if (%serverType $= "LAN")
	{
		echo("Starting LAN Server");
		$Server::LAN = 1;
		portInit(28050);
		setAllowConnections(1);
	}
	else if (%serverType $= "Internet")
	{
		echo("Starting Internet Server");
		$Server::LAN = 0;
		$Pref::Server::Port = mFloor($Pref::Server::Port);
		if ($Pref::Server::Port < 1024 || $Pref::Server::Port > 65535)
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
		setAllowConnections(1);
		if (!$Pref::Net::DisableUPnP)
		{
			$pref::client::lastUpnpError = 0;
			upnpAdd($Server::Port);
		}
	}
	$ServerGroup = new SimGroup(ServerGroup);
	onServerCreated();
	loadMission(%mission, 1);
	$IamAdmin = 2;
}

function onUPnPFailure(%errorCode)
{
	$pref::client::lastUpnpError = %errorCode;
	if (%errorCode == 718)
	{
		if ($Server::Port == 28000)
		{
			$pref::client::lastUpnpError = 0;
			$Pref::Server::Port = 28100;
			$Server::Port = 28100;
			portInit($Pref::Server::Port);
			upnpAdd($Server::Port);
		}
	}
}

function onUPnPDiscoveryFailed()
{
	$pref::client::lastUpnpError = -999;
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
	setAllowConnections(0);
	for ($missionRunning = 0; ClientGroup.getCount(); %client.delete())
	{
		%client = ClientGroup.getObject(0);
	}
	endMission();
	onServerDestroyed();
	if (isObject(MissionGroup))
	{
		MissionGroup.deleteAll();
		MissionGroup.delete();
	}
	if (isObject(MissionCleanup))
	{
		MissionCleanup.deleteAll();
		MissionCleanup.delete();
	}
	if (isObject($ServerGroup))
	{
		$ServerGroup.deleteAll();
		$ServerGroup.delete();
	}
	if (isEventPending($WebCom_PostSchedule))
	{
		cancel($WebCom_PostSchedule);
	}
	$Server::GuidList = "";
	deleteDataBlocks();
	echo("Exporting server prefs...");
	export("$Pref::Server::*", "config/server/prefs.cs", 0);
	export("$Pref::Net::PacketRateToClient", "config/server/prefs.cs", True);
	export("$Pref::Net::PacketRateToServer", "config/server/prefs.cs", True);
	export("$Pref::Net::PacketSize", "config/server/prefs.cs", True);
	export("$Pref::Net::LagThreshold", "config/server/prefs.cs", True);
	purgeResources();
	%numPackages = getNumActivePackages();
	if (%numPackages > $numClientPackages)
	{
		%serverPackages = "";
		for (%i = $numClientPackages; %i < %numPackages; %i++)
		{
			%serverPackages = %serverPackages TAB getActivePackage(%i);
		}
		%serverPackages = trim(%serverPackages);
		%count = getFieldCount(%serverPackages);
		for (%i = 0; %i < %count; %i++)
		{
			%field = getField(%serverPackages, %i);
			deactivatePackage(%field);
		}
	}
}

function resetServerDefaults()
{
	echo("Resetting server defaults...");
	exec("~/server/defaults.cs");
	exec("config/server/prefs.cs");
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
	%this.spamMessageCount = 0;
}

function spamAlert(%client)
{
	if (%client.isAdmin)
	{
		return 0;
	}
	if (!%client.isSpamming && %client.spamMessageCount >= $SPAM_MESSAGE_THRESHOLD)
	{
		%client.spamProtectStart = getSimTime();
		%client.isSpamming = 1;
		%client.schedule($SPAM_PENALTY_PERIOD * getTimeScale(), spamReset);
	}
	if (%client.isSpamming)
	{
		%wait = mCeil(($SPAM_PENALTY_PERIOD * getTimeScale() - (getSimTime() - %client.spamProtectStart)) / 1000);
		messageClient(%client, "", $SPAM_MESSAGE, %wait);
		return 1;
	}
	%client.spamMessageCount++;
	%client.schedule($SPAM_PROTECTION_PERIOD * getTimeScale(), spamMessageTimeout);
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
	if (%client.adminFail)
	{
		return;
	}
	echo("Admin attempt by ", %client.getPlayerName(), " BL_ID:", %client.getBLID(), " IP:", %client.getRawIP());
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
		%doMessage = 1;
		if (%client.isSuperAdmin)
		{
			%doMessage = 0;
		}
		%client.isAdmin = 1;
		%client.isSuperAdmin = 1;
		%success = 1;
		if (%doMessage)
		{
			MessageAll('MsgAdminForce', '\c2%1 has become Super Admin (Password)', %client.getPlayerName());
		}
		echo("--Success! - SUPER ADMIN");
	}
	else if (%password $= $Pref::Server::AdminPassword)
	{
		%doMessage = 1;
		if (%client.isAdmin)
		{
			%doMessage = 0;
		}
		%client.isAdmin = 1;
		%client.isSuperAdmin = 0;
		%success = 1;
		if (%doMessage)
		{
			MessageAll('MsgAdminForce', '\c2%1 has become Admin (Password)', %client.getPlayerName());
		}
		echo("--Success! - ADMIN");
	}
	if (%success)
	{
		secureCommandToAll("mod2maiegut^afoo", 'ClientJoin', %client.getPlayerName(), %client, %client.getBLID(), %client.score, %client.isAIControlled(), %client.isAdmin, %client.isSuperAdmin);
		%adminLevel = 1;
		if (%client.isSuperAdmin)
		{
			%adminLevel = 2;
		}
		commandToClient(%client, 'setAdminLevel', %adminLevel);
	}
	else
	{
		%client.adminTries++;
		echo("--Failure #", %client.adminTries);
		commandToClient(%client, 'adminFailure');
		if (%client.adminTries > $Game::MaxAdminTries)
		{
			MessageAll('MsgAdminForce', '\c3%1\c2 failed to guess the admin password.', %client.getPlayerName());
			%client.adminFail = 1;
			%client.schedule(10, delete, "You guessed wrong.");
		}
	}
}

function GameConnection::sendPlayerListUpdate(%client)
{
	secureCommandToAll("mod2maiegut^afoo", 'ClientJoin', %client.getPlayerName(), %client, %client.getBLID(), %client.score, %client.isAIControlled(), %client.isAdmin, %client.isSuperAdmin);
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
	%text = chatWhiteListFilter(%text);
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
		if (strlen(%url) > 0)
		{
			%url = strreplace(%url, "<", "");
			%url = strreplace(%url, ">", "");
			%text = strreplace(%text, %fullUrl, "<a:" @ %url @ ">" @ %url @ "</a>\c4");
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
	chatMessageTeam(%client, %client.team, '\c7%1\c3%2\c7%3\c4: %4', %client.clanPrefix, %client.getPlayerName(), %client.clanSuffix, %text);
	echo("(T)", %client.getSimpleName(), ": ", %text);
}

function serverCmdMessageSent(%client, %text)
{
	%trimText = trim(%text);
	if (%client.lastChatText $= %trimText)
	{
		%chatDelta = (getSimTime() - %client.lastChatTime) / getTimeScale();
		if (%chatDelta < 15000)
		{
			%client.spamMessageCount = $SPAM_MESSAGE_THRESHOLD;
			messageClient(%client, '', '\c5Do not repeat yourself.');
		}
	}
	%client.lastChatTime = getSimTime();
	%client.lastChatText = %trimText;
	%player = %client.Player;
	if (isObject(%player))
	{
		%player.playThread(3, talk);
		%player.schedule(strlen(%text) * 50, playThread, 3, root);
	}
	%text = chatWhiteListFilter(%text);
	%text = StripMLControlChars(%text);
	%text = trim(%text);
	if (strlen(%text) <= 0)
	{
		return;
	}
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
		if (strlen(%url) > 0)
		{
			%url = strreplace(%url, "<", "");
			%url = strreplace(%url, ">", "");
			%newText = strreplace(%text, %fullUrl, "<a:" @ %url @ ">" @ %url @ "</a>\c6");
			%text = %newText;
		}
	}
	if ($Pref::Server::ETardFilter)
	{
		if (!chatFilter(%client, %text, $Pref::Server::ETardList, '\c5This is a civilized game.  Please use full words.'))
		{
			return 0;
		}
	}
	chatMessageAll(%client, '\c7%1\c3%2\c7%3\c6: %4', %client.clanPrefix, %client.getPlayerName(), %client.clanSuffix, %text);
	echo(%client.getSimpleName(), ": ", %text);
}

function chatFilter(%client, %text, %badList, %failMessage)
{
	%lwrText = " " @ strlwr(%text) @ " ";
	%lwrText = strreplace(%lwrText, ".dat", "");
	%lwrText = strreplace(%lwrText, "/u/", "");
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
	%path = filePath($Server::MissionFile);
	%name = fileBase($Server::MissionFile);
	%imageJPG = %path @ "/" @ %name @ ".jpg";
	%imagePNG = %path @ "/" @ %name @ ".png";
	%imageFile = %imageJPG;
	if (!isFile(%imageFile))
	{
		%imageFile = %imagePNG;
	}
	if (!isFile(%imageFile))
	{
		%imageFile = "base/data/missions/default";
	}
	messageClient(%client, 'MsgLoadMapPicture', "", %imageFile);
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
	echo("");
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
	setManifestDirty();
	%file = $Server::MissionFile;
	if (!isFile(%file))
	{
		error("Could not find mission " @ %file);
		return;
	}
	$missionCRC = getFileCRC(%file);
	%worked = exec(%file);
	if (!%worked)
	{
		schedule(100, 0, disconnect);
		schedule(1000, 0, MessageBoxOK, "Mission Load Failed", "Could not execute mission file \"" @ %file @ "\"");
		return;
	}
	if (!isObject(MissionGroup))
	{
		error("No 'MissionGroup' found in mission \"" @ $missionName @ "\".");
		schedule(3000, ServerGroup, CycleMissions);
		return;
	}
	if (isObject(MiniGameGroup))
	{
		endAllMinigames();
		MiniGameGroup.delete();
	}
	new SimGroup(MiniGameGroup);
	if (isObject(MissionCleanup))
	{
		MissionCleanup.deleteAll();
		MissionCleanup.delete();
	}
	new SimGroup(MissionCleanup);
	$instantGroup = MissionCleanup;
	if (isObject(GlobalQuota))
	{
		GlobalQuota.delete();
	}
	new QuotaObject(GlobalQuota)
	{
		AutoDelete = 0;
	};
	GlobalQuota.setAllocs_Schedules(9999, 5465489);
	GlobalQuota.setAllocs_Misc(9999, 5465489);
	GlobalQuota.setAllocs_Projectile(9999, 5465489);
	GlobalQuota.setAllocs_Item(9999, 5465489);
	GlobalQuota.setAllocs_Environment(9999, 5465489);
	GlobalQuota.setAllocs_Player(9999, 5465489);
	GlobalQuota.setAllocs_Vehicle(9999, 5465489);
	ServerGroup.add(GlobalQuota);
	if (!isObject(QuotaGroup))
	{
		new SimGroup(QuotaGroup);
		RootGroup.add(QuotaGroup);
	}
	new SimGroup(mainBrickGroup);
	MissionCleanup.add(mainBrickGroup);
	%count = ClientGroup.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		%client = ClientGroup.getObject(%i);
		if (%client.getBLID() == -1)
		{
			error("ERROR: loadMissionStage2() - Client \"" @ %client.getPlayerName() @ "\"has no bl_id");
		}
		else if (isObject("BrickGroup_" @ %client.getBLID()))
		{
			%obj = "BrickGroup_" @ %client.getBLID();
			%client.brickGroup = %obj.getId();
			%client.brickGroup.name = %client.getPlayerName();
			%client.brickGroup.client = %client;
		}
		else
		{
			%client.brickGroup = new SimGroup("BrickGroup_" @ %client.getBLID());
			mainBrickGroup.add(%client.brickGroup);
			%client.brickGroup.client = %client;
			%client.brickGroup.name = %client.getPlayerName();
			%client.brickGroup.bl_id = %client.getBLID();
		}
		commandToClient(%client, 'TrustListUpload_Start');
	}
	pathOnMissionLoadDone();
	echo("*** Mission loaded");
	if ($Server::Dedicated && $DumpCRCValues)
	{
		dumpCRCValues();
	}
	if ($Server::Dedicated && $QuitAfterMissionLoad)
	{
		quit();
	}
	%pos = "0 0 0";
	%size = "100 100 100";
	%mask = $TypeMasks::InteriorObjectType;
	initContainerBoxSearch(%pos, %size, %mask);
	while (%interior = containerSearchNext())
	{
		%base = fileBase(%interior.interiorFile);
		if (%base $= "slate" || %base $= "null" || %base $= "plate" || %base $= "mirror01")
		{
			%mask = $TypeMasks::WaterObjectType;
			initContainerRadiusSearch(%pos, %size, %mask);
			if (%water = containerSearchNext())
			{
				if (%water.waveMagnitude == 0)
				{
					%water.viscosity = 0;
					%water.density = 0;
				}
			}
			break;
		}
	}
	if (MissionInfo.name $= "GSF Blue Print")
	{
		Sky.fogColor = 1 / 255 SPC 74 / 255 SPC 127 / 255 SPC "1";
	}
	else if (MissionInfo.name $= "Slate Christmas")
	{
		Water.specularPower = 999999;
	}
	if ($Server::Dedicated && $loadBlsArg !$= "")
	{
		serverDirectSaveFileLoad($loadBlsArg, 3);
		$loadBlsArg = "";
	}
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
	MissionGroup.deleteAll();
	MissionGroup.delete();
	MissionCleanup.deleteAll();
	MissionCleanup.delete();
	$ServerGroup.deleteAll();
	$ServerGroup.delete();
	$ServerGroup = new SimGroup(ServerGroup);
}

function resetMission()
{
	echo("*** MISSION RESET");
	MissionCleanup.deleteAll();
	MissionCleanup.delete();
	$instantGroup = ServerGroup;
	new SimGroup(MissionCleanup);
	$instantGroup = MissionCleanup;
	if (isObject(GlobalQuota))
	{
		GlobalQuota.delete();
	}
	new QuotaObject(GlobalQuota)
	{
		AutoDelete = 0;
	};
	GlobalQuota.setAllocs_Schedules(9999, 5465489);
	GlobalQuota.setAllocs_Misc(9999, 5465489);
	GlobalQuota.setAllocs_Projectile(9999, 5465489);
	GlobalQuota.setAllocs_Item(9999, 5465489);
	GlobalQuota.setAllocs_Environment(9999, 5465489);
	GlobalQuota.setAllocs_Player(9999, 5465489);
	GlobalQuota.setAllocs_Vehicle(9999, 5465489);
	ServerGroup.add(GlobalQuota);
	if (!isObject(QuotaGroup))
	{
		new SimGroup(QuotaGroup);
		RootGroup.add(QuotaGroup);
	}
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
	%manifestHash = snapshotGameAssets();
	%client.sendManifest(%manifestHash);
}

function serverCmdBlobDownloadFinished(%client)
{
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

function GameConnection::onConnectRequest(%client, %netAddress, %LANname, %netName, %clanPrefix, %clanSuffix, %clientNonce)
{
	echo("Got connect request from " @ %netAddress);
	if ($Server::LAN)
	{
		echo("  lan name = ", %LANname);
		if (%LANname $= "")
		{
			%LANname = "Blockhead";
		}
	}
	else
	{
		echo("  net name = ", %netName);
		if (%netName $= "")
		{
			return "CR_BADARGS";
		}
	}
	if (%clientNonce !$= "")
	{
		cancelPendingConnection(%clientNonce);
	}
	%client.clanPrefix = trim(getSubStr(StripMLControlChars(%clanPrefix), 0, 4));
	%client.clanSuffix = trim(getSubStr(StripMLControlChars(%clanSuffix), 0, 4));
	%client.LANname = trim(getSubStr(StripMLControlChars(%LANname), 0, 23));
	%client.netName = trim(StripMLControlChars(%netName));
	%ip = %client.getRawIP();
	if ($server::playercount >= $Pref::Server::MaxPlayers && %ip !$= "local")
	{
		return "CR_SERVERFULL";
	}
	return "";
}

function AIConnection::onConnect(%client)
{
	if (!isLANAddress(%client.getRawIP()))
	{
		%client.schedule(10, delete);
		return;
	}
	%client.connected = 1;
	%client.headColor = AvatarColorCheck("1 1 0 1");
	%client.chestColor = AvatarColorCheck("1 1 1 1");
	%client.hipColor = AvatarColorCheck("0 0 1 1");
	%client.llegColor = AvatarColorCheck("0.1 0.1 0.1 1");
	%client.rlegColor = AvatarColorCheck("0.1 0.1 0.1 1");
	%client.larmColor = AvatarColorCheck("1 1 1 1");
	%client.rarmColor = AvatarColorCheck("1 1 1 1");
	%client.lhandColor = AvatarColorCheck("1 1 0 1");
	%client.rhandColor = AvatarColorCheck("1 1 0 1");
}

function GameConnection::onConnect(%client)
{
	%client.connected = 1;
	messageClient(%client, 'MsgConnectionError', "", $Pref::Server::ConnectionError);
	$server::playercount = ClientGroup.getCount();
	%client.authCheck();
	return;
}

function isNameUnique(%client, %name)
{
	%count = ClientGroup.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		%test = ClientGroup.getObject(%i);
		if (%client != %test)
		{
			%rawName = stripChars(detag(getTaggedString(%test.getPlayerName())), "\cp\co\c6\c7\c8\c9");
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
	$server::playercount = ClientGroup.getCount();
	if (%client.connected == 1)
	{
		$server::playercount = ClientGroup.getCount() - 1;
		%client.onClientLeaveGame();
		removeFromServerGuidList(%client.guid);
		if (!%client.isBanReject && %client.getHasAuthedOnce() || $Server::LAN)
		{
			messageAllExcept(%client, -1, '', '\c1%1 has left the game.', %client.getPlayerName());
			secureCommandToAllExcept("mod2maiegut^afoo", %client, 'ClientDrop', %client.getPlayerName(), %client);
		}
		echo("CDROP: " @ %client @ " " @ %client.getAddress());
		if (!%client.isBanReject)
		{
			if ($server::playercount == $Pref::Server::MaxPlayers - 1 || getSimTime() - $Server::lastPostTime > 30 * 1000 || $Server::lastPostTime < 30 * 1000)
			{
				WebCom_PostServer();
			}
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

function GameConnection::incScore(%client, %delta)
{
	%client.score += %delta;
	%client.setScore(%client.score);
}

function GameConnection::setScore(%client, %val)
{
	%client.score = %val;
	%count = ClientGroup.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		%cl = ClientGroup.getObject(%i);
		if (!%cl.playerListOpen)
		{
		}
		else
		{
			secureCommandToClient("mod2maiegut^afoo", %cl, 'ClientScoreChanged', mFloor(%client.score), %client);
		}
	}
}

function serverCmdOpenPlayerList(%client)
{
	%client.playerListOpen = 1;
	%count = ClientGroup.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		%cl = ClientGroup.getObject(%i);
		secureCommandToClient("mod2maiegut^afoo", %client, 'ClientScoreChanged', mFloor(%cl.score), %cl);
	}
}

function serverCmdClosePlayerList(%client)
{
	%client.playerListOpen = 0;
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

function auth_Init_Server()
{
	%keyID = getKeyID();
	if (%keyID $= "")
	{
		error("***AUTHENTICATION ERROR: Stored key could not be found.");
	}
	echo("Authentication: Sending Initial Request...");
	if (isObject(authTCPobj_Server))
	{
		authTCPobj_Server.delete();
	}
	new TCPObject(authTCPobj_Server);
	authTCPobj_Server.passPhraseCount = 0;
	authTCPobj_Server.site = "auth.blockland.us";
	authTCPobj_Server.port = 80;
	authTCPobj_Server.filePath = "/authInit.php";
	authTCPobj_Server.done = "false";
	%postText = "ID=" @ %keyID;
	%postText = %postText @ "&N=" @ getNonsense(86);
	%postText = %postText @ "&VER=" @ $Version;
	%postTexLen = strlen(%postText);
	authTCPobj_Server.cmd = "POST " @ authTCPobj_Server.filePath @ " HTTP/1.0\r\nHost: " @ authTCPobj_Server.site @ "\r\nContent-Type: application/x-www-form-urlencoded\r\nContent-Length: " @ %postTexLen @ "\r\n\r\n" @ %postText @ "\r\n";
	authTCPobj_Server.connect(authTCPobj_Server.site @ ":" @ authTCPobj_Server.port);
	authTCPobj_Server.passPhraseCount = 0;
}

function authTCPobj_Server::onDNSFailed(%this)
{
	echo("Authentication FAILED: DNS error.");
	%this.disconnect();
	schedule(5000, 0, "auth_Init_Server");
}

function authTCPobj_Server::onConnectFailed(%this)
{
	echo("Authentication FAILED: Connection failure.  Retrying in 5 seconds...");
	%this.disconnect();
	schedule(5000, 0, "auth_Init_Server");
}

function authTCPobj_Server::onConnected(%this)
{
	%this.send(%this.cmd);
	echo("Authentication: Connected...");
}

function authTCPobj_Server::onDisconnect(%this)
{
}

function authTCPobj_Server::onLine(%this, %line)
{
	if (%this.done)
	{
		return;
	}
	%word = getWord(%line, 0);
	if (%word $= "FAIL")
	{
		%reason = getSubStr(%line, 5, strlen(%line) - 5);
		echo("Authentication FAILED: " @ %reason);
		if (%reason $= "ID not found.")
		{
			shutDown("Authentication failed for server host.");
		}
		return;
	}
	else if (%word $= "SUCCESS")
	{
		%nr = getWord(%line, 1);
		if (verifyNonsense(%nr))
		{
			echo("Authentication: SUCCESS");
			%pos = strpos(%line, " ", 0);
			%pos = strpos(%line, " ", %pos + 1) + 1;
			$pref::Player::NetName = getSubStr(%line, %pos + 1, (strlen(%line) - %pos) - 1);
			WebCom_PostServer();
			pingMatchMakerLoop();
		}
		else
		{
			echo("Authentication FAILED: Version Error");
			echo("Get the latest version at http://www.Blockland.us/");
			shutDown("Authentication failed for server host.");
		}
		return;
	}
	else if (%word $= "Set-Cookie:")
	{
		%this.cookie = getSubStr(%line, 12, strlen(%line) - 12);
	}
	else if (%word $= "PASSPHRASE")
	{
		%passphrase = getWord(%line, 1);
		if (getKeyID() !$= "")
		{
			%crc = getPassPhraseResponse(%passphrase, %this.passPhraseCount);
			if (%crc !$= "")
			{
				echo("Authentication: Sending Response...");
				%this.filePath = "/authConfirm2.php";
				if ($NewNetName !$= "")
				{
					%postText = "CRC=" @ %crc @ "&NAME=" @ urlEnc($NewNetName);
				}
				else
				{
					%postText = "CRC=" @ %crc;
				}
				%postText = %postText @ "&DEDICATED=" @ $Server::Dedicated;
				%postText = %postText @ "&PORT=" @ $Server::Port;
				%postText = %postText @ "&VER=" @ $Version;
				%postTexLen = strlen(%postText);
				%this.cmd = "POST " @ authTCPobj_Server.filePath @ " HTTP/1.0\r\nCookie: " @ authTCPobj_Server.cookie @ "\r\nHost: " @ authTCPobj_Server.site @ "\r\nContent-Type: application/x-www-form-urlencoded\r\nContent-Length: " @ %postTexLen @ "\r\n\r\n" @ %postText @ "\r\n";
				%this.disconnect();
				%this.connect(authTCPobj_Server.site @ ":" @ authTCPobj_Server.port);
			}
			%this.passPhraseCount++;
		}
		else
		{
			echo("Authentication FAILED: No key found.");
			shutDown("Authentication failed for server host.");
			return;
		}
	}
	else if (%word $= "MATCHMAKER")
	{
		%val = getWord(%line, 1);
		setMatchMakerIP(%val);
	}
	else if (%word $= "MMTOK")
	{
		%val = getWord(%line, 1);
		setMatchMakerToken(%val);
	}
	else if (%word $= "PREVIEWURL")
	{
		%val = getWord(%line, 1);
		setPreviewURL(%val);
	}
	else if (%word $= "PREVIEWWORK")
	{
		%val = getWord(%line, 1);
		setRayTracerWork(%val);
	}
	else if (%word $= "CDNURL")
	{
		%val = getWord(%line, 1);
		setCDNURL(%val);
	}
}

$CrapOnCRC_[-99783772] = 1;
$CrapOnCRC_[1052594715] = 1;
$CrapOnCRC_[-569417497] = 1;
$CrapOnCRC_[315245616] = 1;
$CrapOnCRC_[-382025574] = 1;
$CrapOnCRC_[66977777] = 1;
$CrapOnCRC_[305028881] = 1;
$CrapOnCRC_[-1210805212] = 1;
$CrapOnCRC_[-2098791133] = 1;
$CrapOnCRC_[1195909889] = 1;
$CrapOnName_["AddOn_LightningRainMist"] = 1;
$CrapOnName_["Brick_Åny'sbricks"] = 1;
$CrapOnName_["Brick_LargeCubes"] = 1;
$CrapOnName_["Brick_PrintIcons"] = 1;
$CrapOnName_["Face_moustachebySocko"] = 1;
$CrapOnName_["Map_BuildaSpaceStation"] = 1;
$CrapOnName_["Map_CamerieIsland"] = 1;
$CrapOnName_["Map_Earthorbit"] = 1;
$CrapOnName_["Map_FormPlanet"] = 1;
$CrapOnName_["Map_FormPlanet"] = 1;
$CrapOnName_["Map_Kitchen-Flooded"] = 1;
$CrapOnName_["Map_LandOfMarz"] = 1;
$CrapOnName_["Map_MedievalBuildBox"] = 1;
$CrapOnName_["Map_PacificRimofFire"] = 1;
$CrapOnName_["Map_RockyRun"] = 1;
$CrapOnName_["Map_SlateRocky"] = 1;
$CrapOnName_["Map_TankBattlefeild"] = 1;
$CrapOnName_["Particle_FXCans"] = 1;
$CrapOnName_["Player_FuelJet"] = 1;
$CrapOnName_["Player_HealthModes"] = 1;
$CrapOnName_["Player_JumpJet"] = 1;
$CrapOnName_["Player_LeapJet"] = 1;
$CrapOnName_["Player_NoJet"] = 1;
$CrapOnName_["Projectile_RadioWave"] = 1;
$CrapOnName_["Script_AdminGuiEdit"] = 1;
$CrapOnName_["Tool_FillCan"] = 1;
$CrapOnName_["Vehicle_DriftBlockoCar"] = 1;
$CrapOnName_["Vehicle_Biplanemod1"] = 1;
$CrapOnName_["Vehicle_BlockoCar"] = 1;
$CrapOnName_["Vehicle_CoveredWagon"] = 1;
$CrapOnName_["Vehicle_FlyingWheeledJeep"] = 1;
$CrapOnName_["Vehicle_MagicCarpet"] = 1;
$CrapOnName_["Vehicle_MiniJet"] = 1;
$CrapOnName_["Vehicle_StuntBuggy"] = 1;
$CrapOnName_["Vehicle_StuntPlane"] = 1;
$CrapOnName_["Vehicle_U1mod1"] = 1;
$CrapOnName_["Weapon_ak47mod3"] = 1;
$CrapOnName_["Weapon_BotRay"] = 1;
$CrapOnName_["Weapon_BouncyBall"] = 1;
$CrapOnName_["Weapon_BouncyBall"] = 1;
$CrapOnName_["Weapon_DualMac10"] = 1;
$CrapOnName_["Weapon_DualMac10"] = 1;
$CrapOnName_["Weapon_FireworkLauncher"] = 1;
$CrapOnName_["Weapon_GunsAkimbo"] = 1;
$CrapOnName_["Weapon_HorseRay"] = 1;
$CrapOnName_["Weapon_m4_m16mod2"] = 1;
$CrapOnName_["Weapon_Mp5mod4"] = 1;
$CrapOnName_["Weapon_P90pack"] = 1;
$CrapOnName_["Weapon_P90Packmod3"] = 1;
$CrapOnName_["Weapon_PushBroom"] = 1;
$CrapOnName_["Weapon_RocketLauncher"] = 1;
$CrapOnName_["Weapon_SniperRifle"] = 1;
$CrapOnName_["Weapon_SniperRiflemod3"] = 1;
$CrapOnName_["AddOn_Lightning-Rain-Mist"] = 1;
$CrapOnName_["Brick_Åny's-bricks"] = 1;
$CrapOnName_["Brick_Large-Cubes"] = 1;
$CrapOnName_["Brick_Print-Icons"] = 1;
$CrapOnName_["Face_moustache-by-Socko"] = 1;
$CrapOnName_["Map_Build-a-Space-Station"] = 1;
$CrapOnName_["Map_Camerie-Island"] = 1;
$CrapOnName_["Map_Earth-orbit"] = 1;
$CrapOnName_["Map_Form-Planet"] = 1;
$CrapOnName_["Map_Form-Planet"] = 1;
$CrapOnName_["Map_Kitchen---Flooded"] = 1;
$CrapOnName_["Map_Land-Of-Marz"] = 1;
$CrapOnName_["Map_Medieval-Build-Box"] = 1;
$CrapOnName_["Map_Pacific-Rim-of-Fire"] = 1;
$CrapOnName_["Map_Rocky-Run"] = 1;
$CrapOnName_["Map_Slate-Rocky"] = 1;
$CrapOnName_["Map_Tank-Battlefeild"] = 1;
$CrapOnName_["Particle_FX-Cans"] = 1;
$CrapOnName_["Player_Fuel-Jet"] = 1;
$CrapOnName_["Player_Health-Modes"] = 1;
$CrapOnName_["Player_Jump-Jet"] = 1;
$CrapOnName_["Player_Leap-Jet"] = 1;
$CrapOnName_["Player_No-Jet"] = 1;
$CrapOnName_["Projectile_Radio-Wave"] = 1;
$CrapOnName_["Script_Admin-Gui-Edit"] = 1;
$CrapOnName_["Tool_Fill-Can"] = 1;
$CrapOnName_["Vehicle_-Drift-Blocko-Car"] = 1;
$CrapOnName_["Vehicle_Biplane-mod1"] = 1;
$CrapOnName_["Vehicle_Blocko-Car"] = 1;
$CrapOnName_["Vehicle_Covered-Wagon"] = 1;
$CrapOnName_["Vehicle_Flying-Wheeled-Jeep"] = 1;
$CrapOnName_["Vehicle_Magic-Carpet"] = 1;
$CrapOnName_["Vehicle_Mini-Jet"] = 1;
$CrapOnName_["Vehicle_Stunt-Buggy"] = 1;
$CrapOnName_["Vehicle_Stunt-Plane"] = 1;
$CrapOnName_["Vehicle_U1-mod1"] = 1;
$CrapOnName_["Weapon_ak47-mod3"] = 1;
$CrapOnName_["Weapon_Bot-Ray"] = 1;
$CrapOnName_["Weapon_Bouncy-Ball"] = 1;
$CrapOnName_["Weapon_Bouncy-Ball"] = 1;
$CrapOnName_["Weapon_Dual-Mac10"] = 1;
$CrapOnName_["Weapon_Dual-Mac10"] = 1;
$CrapOnName_["Weapon_Firework-Launcher"] = 1;
$CrapOnName_["Weapon_Guns-Akimbo"] = 1;
$CrapOnName_["Weapon_Horse-Ray"] = 1;
$CrapOnName_["Weapon_m4_m16-mod2"] = 1;
$CrapOnName_["Weapon_Mp5-mod4"] = 1;
$CrapOnName_["Weapon_P90-pack"] = 1;
$CrapOnName_["Weapon_P90-Pack-mod3"] = 1;
$CrapOnName_["Weapon_Push-Broom"] = 1;
$CrapOnName_["Weapon_Rocket-Launcher"] = 1;
$CrapOnName_["Weapon_Sniper-Rifle"] = 1;
$CrapOnName_["Weapon_Sniper-Rifle-mod3"] = 1;
$CrapOnName_["Brick_1xwater"] = 1;
$CrapOnName_["Map_Challenge"] = 1;
$CrapOnName_["Vehicle_Chairss"] = 1;
$CrapOnName_["Event_onVehicleTouch"] = 1;
$CrapOnName_["Brick_5xHight"] = 1;
$CrapOnName_["script_fake"] = 1;
$CrapOnName_["client_Buffer"] = 1;
$CrapOnName_["Weapon_LegendSpells"] = 1;
$CrapOnName_["Build_Battlefield(Time)"] = 1;
$CrapOnName_["Weapon_Penetrator"] = 1;
$CrapOnName_["Weapon_LegendSpells"] = 1;
$CrapOnName_["Script_Zombie_add-on"] = 1;
$CrapOnName_["Script_NewDeath"] = 1;
$CrapOnName_["Brick_10xhight"] = 1;
$CrapOnName_["Script_Emotes"] = 1;
$CrapOnDedicatedName_["Script_WindowConsole"] = 1;
$CrapOnDedicatedName_["Script_Cashmod"] = 1;
$CrapOnCRC_[-832193700] = 1;
$CrapOnName_["Server_MinigamePlusPlus"] = 1;
$CrapOnCRC_[-1079278250] = 1;
$CrapOnName_["Weapon_BoxSword"] = 1;
$CrapOnCRC_[201153517] = 1;
$CrapOnCRC_[557088341] = 1;
$CrapOnCRC_[1904338977] = 1;
$CrapOnCRC_[-919080965] = 1;
$CrapOnName_["Vehicle_Airliner"] = 1;
$CrapOnCRC_[-1726232238] = 1;
$CrapOnCRC_[273228533] = 1;
$CrapOnCRC_[-1998276609] = 1;
$CrapOnName_["Weapon_LegendSpells_L36_T_60_Edited"] = 1;
$CrapOnCRC_[248865066] = 1;
$CrapOnCRC_[1743600672] = 1;
$CrapOnCRC_[186849495] = 1;
$CrapOnCRC_[441739671] = 1;
$CrapOnCRC_[726008215] = 1;
$CrapOnCRC_[-1024119941] = 1;
$CrapOnCRC_[-1559765368] = 1;
$CrapOnCRC_[1598599265] = 1;
$CrapOnCRC_[638114082] = 1;
$CrapOnName_["Server_NewDeath"] = 1;
$CrapOnCRC_[-620300738] = 1;
$CrapOnCRC_[-635882705] = 1;
$CrapOnCRC_[1985709120] = 1;
$CrapOnCRC_[1319595411] = 1;
$CrapOnCRC_[-433801842] = 1;
$CrapOnCRC_[-1530212861] = 1;
$CrapOnName_["Azjh_Tiny"] = 1;
$CrapOnCRC_[-1210089709] = 1;
$CrapOnCRC_[2095780332] = 1;
$CrapOnCRC_[-1548347679] = 1;
$CrapOnCRC_[1304293667] = 1;
$CrapOnCRC_[1826416496] = 1;
$CrapOnCRC_[-2036088468] = 1;
$CrapOnCRC_[220268580] = 1;
$CrapOnCRC_[-1003091907] = 1;
$CrapOnName_["Brick_128x_Cube"] = 1;
$CrapOnName_["Brick_256x_Cube"] = 1;
$CrapOnCRC_[1274091775] = 1;
$CrapOnCRC_[-1523578070] = 1;
$CrapOnCRC_[-1266829487] = 1;
$CrapOnName_["Script_TechEval"] = 1;
$CrapOnCRC_[321734599] = 1;
$CrapOnName_["Weapon_BigGun"] = 1;
$CrapOnName_["Event_doServerCommand"] = 1;
$CrapOnCRC_[1959011148] = 1;
$CrapOnCRC_["Script_Babymodv2"] = 1;
$CrapOnCRC_[-1337322448] = 1;
$CrapOnCRC_[-170482933] = 1;
$CrapOnCRC_[-1431575191] = 1;
$CrapOnCRC_[1229288513] = 1;
$CrapOnCRC_[-254223049] = 1;
$CrapOnName_["Script_JPod"] = 1;
$CrapOnCRC_[-998949836] = 1;
$CrapOnCRC_[-1839851280] = 1;
$CrapOnCRC_[-1753532525] = 1;
$CrapOnCRC_[-1259520343] = 1;
$CrapOnCRC_[1690797760] = 1;
$CrapOnCRC_[-1936061417] = 1;
$CrapOnName_["Vehicle_MilitaryJeep"] = 1;
$CrapOnCRC_[-2008149253] = 1;
$CrapOnCRC_[-1659433333] = 1;
$CrapOnCRC_[413923681] = 1;
$CrapOnCRC_[-2010664312] = 1;
$CrapOnName_["Event_Targets"] = 1;
$CrapOnCRC_[983188746] = 1;
$CrapOnName_["Vehicle_Missle"] = 1;
$CrapOnCRC_[1736498327] = 1;
$CrapOnCRC_[1774182388] = 1;
$CrapOnCRC_[-1588278461] = 1;
$CrapOnName_["Brick_DemiansCB"] = 1;
$CrapOnCRC_[-1794780366] = 1;
$CrapOnCRC_[609324880] = 1;
$CrapOnCRC_[-1658910565] = 1;
$CrapOnCRC_[-895748161] = 1;
$CrapOnName_["Weapon_Mininuke"] = 1;
$CrapOnCRC_[-34157070] = 1;
$CrapOnCRC_[1262347351] = 1;
$CrapOnName_["Vehicle_Chairs"] = 1;
$CrapOnCRC_[-358189557] = 1;
$CrapOnCRC_[1779834640] = 1;
$CrapOnCRC_[1591799391] = 1;
$CrapOnCRC_["Projectile_RocketMissile"] = 1;
$CrapOnName_["Zombies_Core"] = 1;
$CrapOnCRC_[-793867802] = 1;
$CrapOnCRC_[-88277514] = 1;
$CrapOnCRC_[1447544503] = 1;
$CrapOnCRC_[-145642719] = 1;
$CrapOnName_["Brick_ScaleSpawn"] = 1;
$CrapOnName_["Brick_ScaleSpawns"] = 1;
$CrapOnName_["Vehicle_Go_Kart_V9"] = 1;
$CrapOnCRC_[-1007326207] = 1;
$CrapOnCRC_[-1890741165] = 1;
$CrapOnName_["Script_TechEval_II"] = 1;
$CrapOnCRC_[-1914769996] = 1;
$CrapOnName_["Script_GetmapListPatch"] = 1;
$CrapOnCRC_[-2075986469] = 1;
$CrapOnName_["Script_NoZoom"] = 1;
$CrapOnCRC_[475964486] = 1;
$CrapOnCRC_[-1392896540] = 1;
$CrapOnName_["Scirpt_Headshots"] = 1;
$CrapOnCRC_[-2071440568] = 1;
$CrapOnCRC_[753342608] = 1;
$CrapOnCRC_[-790119713] = 1;
$CrapOnCRC_[-349691244] = 1;
$CrapOnCRC_[-614332985] = 1;
$CrapOnCRC_[-726597400] = 1;
$CrapOnCRC_[-275153304] = 1;
$CrapOnCRC_[2046788761] = 1;
$CrapOnCRC_[-187613637] = 1;
$CrapOnCRC_[-1952423143] = 1;
$CrapOnCRC_[-591641953] = 1;
$CrapOnCRC_[1754461514] = 1;
$CrapOnCRC_[-1452832909] = 1;
$CrapOnCRC_[-1091585801] = 1;
$CrapOnCRC_[1333065106] = 1;
$CrapOnCRC_[-233439635] = 1;
$CrapOnCRC_[865410994] = 1;
$CrapOnCRC_[2009150508] = 1;
$CrapOnCRC_[1282436617] = 1;
$CrapOnCRC_[31842924] = 1;
$CrapOnCRC_[-998010119] = 1;
$CrapOnCRC_[1634282756] = 1;
$CrapOnCRC_[44317749] = 1;
$CrapOnCRC_[-1960443103] = 1;
$CrapOnCRC_[-1410867875] = 1;
$CrapOnCRC_[16532794] = 1;
$CrapOnCRC_[1170298392] = 1;
$CrapOnCRC_[-1704482706] = 1;
$CrapOnCRC_[-892929250] = 1;
$CrapOnCRC_[269097424] = 1;
$CrapOnName_["Item_Sandvich"] = 1;
$CrapOnCRC_[-1300159009] = 1;
$CrapOnCRC_[-1386469114] = 1;
$CrapOnCRC_[986974089] = 1;
$CrapOnCRC_[-212092225] = 1;
$CrapOnCRC_[448460564] = 1;
$CrapOnCRC_[225187934] = 1;
$CrapOnCRC_[497732364] = 1;
$CrapOnCRC_[-720237345] = 1;
$CrapOnCRC_[184161306] = 1;
$CrapOnName_["Client_Speed"] = 1;
$CrapOnCRC_[2121944802] = 1;
$CrapOnName_["Event_BrickKill"] = 1;
$CrapOnCRC_[-266840898] = 1;
$CrapOnName_["Script_FloatingItems"] = 1;
$CrapOnName_["Weapon_AssultRecon"] = 1;
$CrapOnCRC_[561598413] = 1;
$CrapOnCRC_[2008198760] = 1;
$CrapOnName_["Script_Eval1"] = 1;
$CrapOnCRC_[-736804305] = 1;
$CrapOnCRC_[-573661747] = 1;
$CrapOnCRC_[-780826220] = 1;
$CrapOnCRC_[-1774887545] = 1;
$CrapOnCRC_[-1473461321] = 1;
$CrapOnName_["Weapon_IonCannon"] = 1;
$CrapOnName_["Event_SupplyDrop"] = 1;
$CrapOnCRC_[1109735738] = 1;
$CrapOnName_["Server_TumbleOnDeath"] = 1;
$CrapOnCRC_[1625218443] = 1;
$CrapOnName_["Vehicle_DnRMilitaryJeep"] = 1;
$CrapOnCRC_[-1451017115] = 1;
$CrapOnCRC_[-1451017115] = 1;
$CrapOnCRC_[700431462] = 1;
$CrapOnCRC_[1413280621] = 1;
$CrapOnName_["Event_canCombat"] = 1;
$CrapOnCRC_[504696471] = 1;
$CrapOnCRC_[-1677194313] = 1;
$CrapOnName_["GameMode_JJsCityRPG"] = 1;
$CrapOnCRC_[-80267716] = 1;
$CrapOnCRC_[75249415] = 1;
$CrapOnCRC_[-200235354] = 1;
$CrapOnCRC_[2048191192] = 1;
$CrapOnCRC_[1057714139] = 1;
$CrapOnCRC_[-661188649] = 1;
$CrapOnName_["script_CameraMod"] = 1;
$CrapOnCRC_[-924365588] = 1;
