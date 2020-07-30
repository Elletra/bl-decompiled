$WebCom_PostSchedule = 0;
function WebCom_PostServer ()
{
	if ($Server::LAN)
	{
		echo ("Can\'t post to master server in LAN game");
		return;
	}
	if (!$Server::Port)
	{
		error ("ERROR: WebCom_PostServer() - $Server::Port is not set, game hasn\'t started yet?");
		return;
	}
	if (!$missionRunning)
	{
		error ("ERROR: WebCom_PostServer() - mission is not running");
		return;
	}
	echo ("Posting to master server...");
	if (isEventPending ($WebCom_PostSchedule))
	{
		cancel ($WebCom_PostSchedule);
	}
	if (isObject (postServerTCPObj))
	{
		postServerTCPObj.delete ();
	}
	new TCPObject (postServerTCPObj);
	postServerTCPObj.site = "master3.blockland.us";
	postServerTCPObj.port = 80;
	postServerTCPObj.filePath = "/postServer.php";
	%urlEncName = urlEnc ($Server::Name);
	%urlEncGameMode = urlEnc ($GameModeDisplayName);
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
		%steamTicket = "";
		%dToken = getDedicatedToken ();
	}
	else 
	{
		%dedicated = 0;
		%steamTicket = SteamGetAuthSessionTicket ();
		%dToken = "";
	}
	$Server::PlayerCount = ClientGroup.getCount ();
	%postText = "steamTicket=" @ urlEnc (%steamTicket);
	%postText = %postText @ "&dToken=" @ urlEnc (%dToken);
	%postText = %postText @ "&blid=" @ getMyBLID ();
	%postText = %postText @ "&port=" @ mFloor ($Server::Port);
	%postText = %postText @ "&passworded=" @ %passworded;
	%postText = %postText @ "&serverName=" @ %urlEncName;
	%postText = %postText @ "&players=" @ mFloor ($Server::PlayerCount);
	%postText = %postText @ "&maxPlayers=" @ mFloor ($Pref::Server::MaxPlayers);
	%postText = %postText @ "&dedicated=" @ %dedicated;
	%postText = %postText @ "&brickCount=" @ mFloor (getBrickCount ());
	%postText = %postText @ "&gameMode=" @ %urlEncGameMode;
	%postText = %postText @ "&build=" @ urlEnc (getBuildNumber ());
	postServerTCPObj.postText = %postText;
	postServerTCPObj.postTextLen = strlen (%postText);
	postServerTCPObj.cmd = "POST " @ postServerTCPObj.filePath @ " HTTP/1.0\r\n" @ "Host: " @ postServerTCPObj.site @ "\r\n" @ "User-Agent: Blockland-r" @ getBuildNumber () @ "\r\n" @ "Content-Type: application/x-www-form-urlencoded\r\n" @ "Content-Length: " @ postServerTCPObj.postTextLen @ "\r\n" @ "\r\n" @ postServerTCPObj.postText @ "\r\n";
	postServerTCPObj.connect (postServerTCPObj.site @ ":" @ postServerTCPObj.port);
	%schduleTime = 5 * 60 * 1000 * getTimeScale ();
	$WebCom_PostSchedule = schedule (%schduleTime, 0, WebCom_PostServer);
	if ($Pref::Server::BrickPublicDomainTimeout > 0)
	{
		%elapsedMS = getSimTime () - $Server::lastPostTime;
		%elapsedMinutes = mFloor (%elapsedMS / (1000 * 60));
		if (%elapsedMinutes > 0)
		{
			%count = mainBrickGroup.getCount ();
			%i = 0;
			while (%i < %count)
			{
				%brickGroup = mainBrickGroup.getObject (%i);
				if (%brickGroup.isPublicDomain)
				{
					
				}
				else if (%brickGroup.hasUser ())
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
				%i += 1;
			}
			$Server::lastPostTime += %elapsedMinutes * 60 * 1000;
		}
	}
	$Server::lastPostTime = getSimTime ();
}

function WebCom_PostServerUpdateLoop ()
{
	if (!isEventPending ($WebCom_PostSchedule))
	{
		WebCom_PostServer ();
		return;
	}
	%timeLeft = getTimeRemaining ($WebCom_PostSchedule);
	%schduleTime = 5 * 60 * 1000 * getTimeScale () * 0.2;
	if (%timeLeft > %schduleTime)
	{
		cancel ($WebCom_PostSchedule);
		$WebCom_PostSchedule = schedule (%schduleTime, 0, WebCom_PostServer);
	}
}

function postServerTCPObj::onConnected (%this)
{
	%this.send (%this.cmd);
}

function postServerTCPObj::onDNSFailed (%this)
{
	echo ("Post to master server FAILED: DNS error. Retrying in 5 seconds...");
	%this.schedule (0, disconnect);
	schedule (5000, 0, "WebCom_PostServer");
}

function postServerTCPObj::onConnectFailed (%this)
{
	echo ("Post to master server FAILED: Connection failure.  Retrying in 5 seconds...");
	%this.schedule (0, disconnect);
	schedule (5000, 0, "WebCom_PostServer");
}

function postServerTCPObj::onDisconnect (%this)
{
	
}

function postServerTCPObj::onLine (%this, %line)
{
	%word = getWord (%line, 0);
	if (%word $= "HTTP/1.1")
	{
		%code = getWord (%line, 1);
		if (%code != 200)
		{
			warn ("WARNING: postServerTCPObj - got non-200 http response \"" @ %code @ "\"");
		}
		if (%code >= 400 && %code <= 499)
		{
			warn ("WARNING: 4xx error on postServerTCPObj, retrying");
			%this.schedule (0, disconnect);
			%this.schedule (5000, connect, %this.site @ ":" @ %this.port);
		}
		if (%code >= 300 && %code <= 399)
		{
			warn ("WARNING: 3xx error on postServerTCPObj, will wait for location header");
		}
	}
	else if (%word $= "Location:")
	{
		%url = getWords (%line, 1);
		warn ("WARNING: postServerTCPObj - Location redirect to " @ %url);
		%this.filePath = %url;
		%this.cmd = "POST " @ %this.filePath @ " HTTP/1.0\r\n" @ "Host: " @ %this.site @ "\r\n" @ "User-Agent: Blockland-r" @ getBuildNumber () @ "\r\n" @ "Content-Type: application/x-www-form-urlencoded\r\n" @ "Content-Length: " @ %this.postTextLen @ "\r\n" @ "\r\n" @ %this.postText @ "\r\n";
		%this.schedule (0, disconnect);
		%this.schedule (500, connect, %this.site @ ":" @ %this.port);
	}
	else if (%word $= "Content-Location:")
	{
		%url = getWords (%line, 1);
		warn ("WARNING: postServerTCPObj - Content-Location redirect to " @ %url);
		%this.filePath = %url;
		%this.cmd = "POST " @ %this.filePath @ " HTTP/1.0\r\n" @ "Host: " @ %this.site @ "\r\n" @ "User-Agent: Blockland-r" @ getBuildNumber () @ "\r\n" @ "Content-Type: application/x-www-form-urlencoded\r\n" @ "Content-Length: " @ %this.postTextLen @ "\r\n" @ "\r\n" @ %this.postText @ "\r\n";
		%this.schedule (0, disconnect);
		%this.schedule (500, connect, %this.site @ ":" @ %this.port);
	}
	else if (%word $= "FAIL")
	{
		%reason = getSubStr (%line, 5, 1000);
		if (%reason $= "No matching server entry found")
		{
			echo ("No matching server entry found, re-sending authentication request");
			schedule (1000, 0, auth_Init_Server);
		}
		else 
		{
			echo ("Posting to master failed: " @ %reason);
		}
	}
	else if (%word $= "SUCCESS")
	{
		echo ("Posting to master server: Success");
	}
	else if (%word $= "MMTOK")
	{
		%val = getWord (%line, 1);
		setMatchMakerToken (%val);
	}
	else if (%word $= "MATCHMAKER")
	{
		%val = getWord (%line, 1);
		setMatchMakerIP (%val);
	}
	else if (%word $= "NOTE")
	{
		%val = getWords (%line, 1, 99);
		echo ("NOTE: " @ %val);
	}
}

function GameConnection::authCheck (%client)
{
	if ($Server::LAN)
	{
		%client.setPlayerName ("au^timoamyo7zene", %client.LANname);
		%client.name = %client.LANname;
		if (%client.isLan ())
		{
			echo ("AUTHCHECK: " @ %client.getPlayerName () @ " = LAN client -> LAN server, loading");
			%client.bl_id = getLAN_BLID ();
			%client.setBLID ("au^timoamyo7zene", getLAN_BLID ());
			%client.startLoad (%client);
			return;
		}
		else 
		{
			echo ("AUTHCHECK: " @ %client.getPlayerName () @ " = internet client -> LAN game, rejecting");
			%client.schedule (10, delete);
			return;
		}
	}
	else 
	{
		%client.setPlayerName ("au^timoamyo7zene", %client.netName);
		%client.name = %client.netName;
	}
	if (isObject (%client.tcpObj))
	{
		%client.tcpObj.delete ();
	}
	%tcp = new TCPObject (servAuthTCPobj);
	%tcp.retryCount = 0;
	%tcp.site = "master3.blockland.us";
	%tcp.port = 80;
	%tcp.filePath = "/authQuery.php";
	%postText = "joinToken=" @ %client.getJoinToken ();
	%postText = %postText @ "&blid=" @ %client.getBLID ();
	%tcp.postText = %postText;
	%tcp.postTextLen = strlen (%postText);
	%tcp.cmd = "POST " @ %tcp.filePath @ " HTTP/1.0\r\n" @ "Host: " @ %tcp.site @ "\r\n" @ "User-Agent: Blockland-r" @ getBuildNumber () @ "\r\n" @ "Content-Type: application/x-www-form-urlencoded\r\n" @ "Content-Length: " @ %tcp.postTextLen @ "\r\n" @ "\r\n" @ %tcp.postText @ "\r\n";
	%tcp.connect (%tcp.site @ ":" @ %tcp.port);
	%client.tcpObj = %tcp;
	%tcp.client = %client;
}

function GameConnection::startLoad (%client)
{
	commandToClient (%client, 'updatePrefs');
	if (%client.getAddress () $= "local")
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
	echo ("CADD: " @ %client @ " " @ %client.getAddress ());
	echo (" +- bl_id = ", %client.getBLID ());
	%autoAdmin = %client.autoAdminCheck ();
	%count = ClientGroup.getCount ();
	%cl = 0;
	while (%cl < %count)
	{
		%other = ClientGroup.getObject (%cl);
		if (%other != %client)
		{
			secureCommandToClient ("zbR4HmJcSY8hdRhr", %client, 'ClientJoin', %other.getPlayerName (), %other, %other.getBLID (), %other.score, %other.isAIControlled (), %other.isAdmin, %other.isSuperAdmin);
		}
		%cl += 1;
	}
	commandToClient (%client, 'NewPlayerListGui_UpdateWindowTitle', $Server::Name, $Pref::Server::MaxPlayers);
	serverCmdRequestMiniGameList (%client);
	$Server::WelcomeMessage = strreplace ($Server::WelcomeMessage, ";", "");
	$Server::WelcomeMessage = strreplace ($Server::WelcomeMessage, "\\\'", "\'");
	$Server::WelcomeMessage = strreplace ($Server::WelcomeMessage, "\'", "\\\'");
	eval ("%taggedMessage = \'" @ $Server::WelcomeMessage @ "\';");
	messageClient (%client, '', %taggedMessage, %client.getPlayerName ());
	messageAllExcept (%client, -1, 'MsgClientJoin', '\c1%1 connected.', %client.getPlayerName ());
	secureCommandToAll ("zbR4HmJcSY8hdRhr", 'ClientJoin', %client.getPlayerName (), %client, %client.getBLID (), %client.score, %client.isAIControlled (), %client.isAdmin, %client.isSuperAdmin);
	if (%autoAdmin == 0)
	{
		echo (" +- no auto admin");
	}
	else if (%autoAdmin == 1)
	{
		MessageAll ('MsgAdminForce', '\c2%1 has become Admin (Auto)', %client.getPlayerName ());
		echo (" +- AUTO ADMIN");
	}
	else if (%autoAdmin == 2)
	{
		MessageAll ('MsgAdminForce', '\c2%1 has become Super Admin (Auto)', %client.getPlayerName ());
		echo (" +- AUTO SUPER ADMIN (List)");
	}
	else if (%autoAdmin == 3)
	{
		MessageAll ('MsgAdminForce', '\c2%1 has become Super Admin (Host)', %client.getPlayerName ());
		echo (" +- AUTO SUPER ADMIN (ID same as host)");
	}
	if (%client.getBLID () <= -1)
	{
		error ("ERROR: GameConnection::startLoad() - Client has no bl_id");
		%client.schedule (10, delete);
		return;
	}
	else if (isObject ("BrickGroup_" @ %client.getBLID ()))
	{
		%obj = "BrickGroup_" @ %client.getBLID ();
		%client.brickGroup = %obj.getId ();
		%client.brickGroup.isPublicDomain = 0;
		%client.brickGroup.abandonedTime = 0;
		%client.brickGroup.name = %client.getPlayerName ();
		%client.brickGroup.client = %client;
		%quotaObject = %client.brickGroup.QuotaObject;
		if (isObject (%quotaObject))
		{
			if (isEventPending (%quotaObject.cancelEventsEvent))
			{
				cancel (%quotaObject.cancelEventsEvent);
			}
			if (isEventPending (%quotaObject.cancelProjectilesEvent))
			{
				cancel (%quotaObject.cancelProjectilesEvent);
			}
		}
	}
	else 
	{
		%client.brickGroup = new SimGroup (("BrickGroup_" @ %client.getBLID ()));
		mainBrickGroup.add (%client.brickGroup);
		%client.brickGroup.client = %client;
		%client.brickGroup.name = %client.getPlayerName ();
		%client.brickGroup.bl_id = %client.getBLID ();
	}
	%client.InitializeTrustListUpload ();
	if ($missionRunning)
	{
		%client.loadMission ();
	}
	if ($Server::PlayerCount >= $Pref::Server::MaxPlayers || getSimTime () - $Server::lastPostTime > 30 * 1000 || $Server::lastPostTime < 30 * 1000)
	{
		WebCom_PostServer ();
	}
}

function GameConnection::autoAdminCheck (%client)
{
	if (getMyBLID () <= 0)
	{
		return 0;
	}
	%clientBLID = %client.getBLID ();
	if (%clientBLID == getMyBLID ())
	{
		%client.isSuperAdmin = 1;
		%client.isAdmin = 1;
		return 3;
	}
	%count = getWordCount ($Pref::Server::AutoSuperAdminList);
	%i = 0;
	while (%i < %count)
	{
		%checkBL_ID = getWord ($Pref::Server::AutoSuperAdminList, %i);
		if (%clientBLID $= %checkBL_ID)
		{
			%client.isSuperAdmin = 1;
			%client.isAdmin = 1;
			return 2;
		}
		%i += 1;
	}
	%count = getWordCount ($Pref::Server::AutoAdminList);
	%i = 0;
	while (%i < %count)
	{
		%checkBL_ID = getWord ($Pref::Server::AutoAdminList, %i);
		if (%clientBLID $= %checkBL_ID)
		{
			%client.isAdmin = 1;
			return 1;
		}
		%i += 1;
	}
	return 0;
}

function GameConnection::killDupes (%client)
{
	%ourIP = %client.getRawIP ();
	%ourBLID = %client.getBLID ();
	%count = ClientGroup.getCount ();
	%clientIndex = 0;
	while (%clientIndex < %count)
	{
		%cl = ClientGroup.getObject (%clientIndex);
		if (%cl == %client)
		{
			
		}
		else if (%cl.getBLID () !$= %ourBLID)
		{
			
		}
		else if (%cl.getRawIP () $= %ourIP && %cl.getPlayerName () $= %client.getPlayerName ())
		{
			
		}
		else if (%cl.isLocal () || %cl.isLan ())
		{
			
		}
		else 
		{
			%cl.schedule (10, delete, "Someone using your Blockland ID joined the server from a different IP address.");
		}
		%clientIndex += 1;
	}
}

function servAuthTCPobj::onDNSFailed (%this)
{
	%this.retryCount += 1;
	%maxRetries = 3;
	if (%this.retryCount > %maxRetries)
	{
		if (%this.client.isLocal ())
		{
			error ("ERROR: - Authentication DNS Failed For Host.");
			%this.client.schedule (60 * 1000 * 5, authCheck);
		}
		else 
		{
			error ("ERROR: - Authentication DNS Failed.");
			%this.client.schedule (60 * 1000 * 5, authCheck);
		}
	}
	else if (%this.client.isLocal () && !%this.client.getHasAuthedOnce ())
	{
		error ("ERROR: - Authentication DNS Failed when attempting to host.");
		MessageBoxOK ("Cannot Host Internet Game", "Authentication DNS Failed.", "disconnect();");
	}
	else 
	{
		%this.schedule (0, disconnect);
		%this.schedule (10, connect, %this.site @ ":" @ %this.port);
		error ("ERROR: - Authentication DNS Failed.  Retry ", %this.retryCount);
	}
}

function servAuthTCPobj::onConnectFailed (%this)
{
	%this.retryCount += 1;
	%maxRetries = 5;
	if (%this.client.isLocal () && !%this.client.getHasAuthedOnce ())
	{
		%maxRetries = 3;
	}
	if (%this.retryCount > %maxRetries)
	{
		if (%this.client.isLocal ())
		{
			error ("ERROR: - Authentication Connnection Failed For Host.");
			%this.client.schedule (60 * 1000 * 5, authCheck);
		}
		else 
		{
			error ("ERROR: - Authentication Connection Failed.");
			%this.client.schedule (60 * 1000 * 5, authCheck);
		}
	}
	else if (%this.client.isLocal () && !%this.client.getHasAuthedOnce ())
	{
		if (%this.retryCount > 1)
		{
			error ("ERROR: - Authentication Connection Failed when attempting to host.");
			MessageBoxOK ("Cannot Host Internet Game", "Authentication Connection Failed.", "disconnect();");
		}
		else 
		{
			%this.schedule (0, disconnect);
			%this.schedule (10, connect, %this.site @ ":" @ %this.port);
			error ("ERROR: - Authentication Connection Failed.  Retry ", %this.retryCount);
		}
	}
	else 
	{
		%this.schedule (0, disconnect);
		%this.schedule (10, connect, %this.site @ ":" @ %this.port);
		error ("ERROR: - Authentication Connection Failed.  Retry ", %this.retryCount);
	}
}

function servAuthTCPobj::onConnected (%this)
{
	%this.send (%this.cmd);
}

function servAuthTCPobj::onLine (%this, %line)
{
	%word = getWord (%line, 0);
	if (%word $= "HTTP/1.1")
	{
		%code = getWord (%line, 1);
		if (%code != 200)
		{
			warn ("WARNING: servAuthTCPobj - got non-200 http response \"" @ %code @ "\"");
		}
		if (%code >= 400 && %code <= 499)
		{
			warn ("WARNING: 4xx error on servAuthTCPobj, retrying");
			%this.schedule (0, disconnect);
			%this.schedule (500, connect, %this.site @ ":" @ %this.port);
		}
		if (%code >= 300 && %code <= 399)
		{
			warn ("WARNING: 3xx error on servAuthTCPobj, will wait for location header");
		}
	}
	else if (%word $= "Location:")
	{
		%url = getWords (%line, 1);
		warn ("WARNING: servAuthTCPobj - Location redirect to " @ %url);
		%this.filePath = %url;
		%this.cmd = "POST " @ %this.filePath @ " HTTP/1.0\r\n" @ "Host: " @ %this.site @ "\r\n" @ "User-Agent: Blockland-r" @ getBuildNumber () @ "\r\n" @ "Content-Type: application/x-www-form-urlencoded\r\n" @ "Content-Length: " @ %this.postTextLen @ "\r\n" @ "\r\n" @ %this.postText @ "\r\n";
		%this.schedule (0, disconnect);
		%this.schedule (500, connect, %this.site @ ":" @ %this.port);
	}
	else if (%word $= "Content-Location:")
	{
		%url = getWords (%line, 1);
		warn ("WARNING: servAuthTCPobj - Content-Location redirect to " @ %url);
		%this.filePath = %url;
		%this.cmd = "POST " @ %this.filePath @ " HTTP/1.0\r\n" @ "Host: " @ %this.site @ "\r\n" @ "User-Agent: Blockland-r" @ getBuildNumber () @ "\r\n" @ "Content-Type: application/x-www-form-urlencoded\r\n" @ "Content-Length: " @ %this.postTextLen @ "\r\n" @ "\r\n" @ %this.postText @ "\r\n";
		%this.schedule (0, disconnect);
		%this.schedule (500, connect, %this.site @ ":" @ %this.port);
	}
	else if (%word $= "NAME")
	{
		%this.client.netName = getWords (%line, 1, 99);
		%this.client.setPlayerName ("au^timoamyo7zene", %this.client.netName);
		%this.client.name = %this.client.netName;
	}
	else if (%word $= "STEAMID")
	{
		%this.client.steamID = getWord (%line, 1);
	}
	else if (%word $= "SUCCESS")
	{
		if (!%this.client.getHasAuthedOnce ())
		{
			echo ("Auth Init Successfull: " @ %this.client.getPlayerName ());
			%this.client.setHasAuthedOnce (1);
			%this.client.startLoad ();
			%this.client.killDupes ();
			%this.client.schedule (60 * 1000 * 5, authCheck);
			if (!$Pref::Server::AllowMultiClient)
			{
				%count = ClientGroup.getCount ();
				%i = 0;
				while (%i < %count)
				{
					%cl = ClientGroup.getObject (%i);
					if (%cl == %this.client)
					{
						
					}
					else if (%cl.getBLID () == %this.client.getBLID ())
					{
						%cl.schedule (10, delete, "Duplicate client removed (2)");
					}
					%i += 1;
				}
			}
		}
		else 
		{
			echo ("Auth Continue Successfull: " @ %this.client.getPlayerName ());
			%this.client.schedule (60 * 1000 * 5, authCheck);
		}
	}
	else if (%word $= "FAIL")
	{
		if (isObject (%this.client))
		{
			if (%this.client.getHasAuthedOnce ())
			{
				MessageAll ('', '\c2%1 Authentication Failed (%2).', %this.client.getPlayerName (), %this.client.getRawIP ());
			}
			echo (" Authentication Failed for " @ %this.client.getPlayerName () @ " (" @ %this.client.getRawIP () @ ").");
			if (%this.client.isLocal ())
			{
				shutDown ("Authentication failed for server host.");
				schedule (10, 0, disconnect);
				schedule (11, 0, MessageBoxOK, "Server Shut Down", "Server shut down - Authentication Failed.");
			}
			else 
			{
				%this.client.schedule (10, delete, "Server could not verify your Blockland ID.");
				return;
			}
		}
		else 
		{
			error ("ERROR: servAuthTCPobj::onLine() - Orphan tcp object ", %this);
		}
	}
	else if (%word $= "ERROR:")
	{
		if (isObject (%this.client))
		{
			echo (" Authentication Error for " @ %this.client.getPlayerName () @ " (" @ %this.client.getRawIP () @ ") - " @ %line);
			if (!%this.client.getHasAuthedOnce ())
			{
				%this.client.schedule (10, delete, "Server experienced an authentication error.");
				MessageAll ('', '\c2%1 Authentication Error (%2).', %this.client.getPlayerName (), %this.client.getRawIP ());
			}
			else 
			{
				%this.client.schedule (60 * 1000 * 5, authCheck);
				%this.done = 1;
			}
		}
		else 
		{
			error ("ERROR: servAuthTCPobj::onLine() - Orphan tcp object ", %this);
		}
	}
}

function ServerPlay2D (%profile)
{
	%idx = 0;
	while (%idx < ClientGroup.getCount ())
	{
		ClientGroup.getObject (%idx).play2D (%profile);
		%idx += 1;
	}
}

function ServerPlay3D (%profile, %transform)
{
	%idx = 0;
	while (%idx < ClientGroup.getCount ())
	{
		ClientGroup.getObject (%idx).play3D (%profile, %transform);
		%idx += 1;
	}
}

function portInit (%port)
{
	if (%port == 280000)
	{
		%port = 28000;
	}
	if (%port == 280001)
	{
		%port = 28001;
	}
	%port = mClampF (%port, 0, 65535);
	%failCount = 0;
	while (%failCount < 10 && !setNetPort (%port))
	{
		echo ("Port init failed on port " @ %port @ " trying next port.");
		%port += 1;
		%failCount += 1;
	}
	$Server::Port = %port;
}

function createServer (%serverType)
{
	destroyServer ();
	echo ("");
	$missionSequence = 0;
	$Server::PlayerCount = 0;
	$Server::ServerType = %serverType;
	if (%serverType $= "SinglePlayer" && $Server::Dedicated)
	{
		error ("ERROR: createServer() - SinglePlayer mode specified for dedicated server");
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
		echo ("Starting Single Player Server");
		$Server::LAN = 1;
		portInit (0);
		setAllowConnections (0);
	}
	else if (%serverType $= "LAN")
	{
		echo ("Starting LAN Server");
		$Server::LAN = 1;
		portInit (28050);
		setAllowConnections (1);
	}
	else if (%serverType $= "Internet")
	{
		echo ("Starting Internet Server");
		$Server::LAN = 0;
		$Pref::Server::Port = mFloor ($Pref::Server::Port);
		if ($Pref::Server::Port < 1024 || $Pref::Server::Port > 65535)
		{
			$Pref::Server::Port = 28000;
		}
		if ($portArg)
		{
			portInit ($portArg);
		}
		else 
		{
			portInit ($Pref::Server::Port);
		}
		setAllowConnections (1);
		if (!$Pref::Net::DisableUPnP && !$noUpnpArg)
		{
			if (getUpnpPort () != $Server::Port)
			{
				$pref::client::lastUpnpError = 0;
				upnpAdd ($Server::Port);
			}
		}
	}
	$ServerGroup = new SimGroup (ServerGroup);
	onServerCreated ();
	buildEnvironmentLists ();
	if ($UINameTableCreated == 0)
	{
		createUINameTable ();
	}
	createMission ();
	$IamAdmin = 2;
	$EnvGuiServer::HasSetAdvancedOnce = 0;
	if ($GameModeArg !$= "")
	{
		EnvGuiServer::getIdxFromFilenames ();
		EnvGuiServer::SetSimpleMode ();
		if (!$EnvGuiServer::SimpleMode)
		{
			EnvGuiServer::fillAdvancedVarsFromSimple ();
			EnvGuiServer::SetAdvancedMode ();
		}
	}
	else 
	{
		$EnvGuiServer::SkyIdx = 0;
		$EnvGuiServer::WaterIdx = 0;
		$EnvGuiServer::GroundIdx = 0;
		$EnvGuiServer::SkyFile = "Add-Ons/Sky_Blue2/Blue2.dml";
		$EnvGuiServer::GroundFile = "Add-Ons/Ground_Plate/plate.ground";
		EnvGuiServer::getIdxFromFilenames ();
		$EnvGuiServer::WaterIdx = 0;
		$EnvGuiServer::SimpleMode = 1;
		EnvGuiServer::SetSimpleMode ();
		DayCycle.setEnabled (0);
		EnvGuiServer::readAdvancedVarsFromSimple ();
	}
}

function onUPnPFailure (%errorCode)
{
	$pref::client::lastUpnpError = %errorCode;
	if (%errorCode == 718)
	{
		if ($Server::Port == 28000)
		{
			$pref::client::lastUpnpError = 0;
			$Pref::Server::Port = 28100;
			$Server::Port = 28100;
			portInit ($Pref::Server::Port);
			upnpAdd ($Server::Port);
		}
	}
}

function onUPnPDiscoveryFailed ()
{
	$pref::client::lastUpnpError = -999;
}

function destroyServer ()
{
	if ($Server::LAN)
	{
		echo ("Destroying LAN Server");
	}
	else 
	{
		echo ("Destroying NET Server");
	}
	$Server::ServerType = "";
	setAllowConnections (0);
	$missionRunning = 0;
	if (isEventPending ($LoadSaveFile_Tick_Schedule))
	{
		cancel ($LoadSaveFile_Tick_Schedule);
	}
	while (ClientGroup.getCount ())
	{
		%client = ClientGroup.getObject (0);
		%client.delete ();
	}
	endMission ();
	onServerDestroyed ();
	if (isEventPending ($WebCom_PostSchedule))
	{
		cancel ($WebCom_PostSchedule);
	}
	$Server::GuidList = "";
	deleteDataBlocks ();
	if (isEventPending ($LoadingBricks_HandShakeSchedule))
	{
		cancel ($LoadingBricks_HandShakeSchedule);
	}
	$LoadingBricks_HandShakeSchedule = 0;
	if (isEventPending ($UploadSaveFile_Tick_Schedule))
	{
		cancel ($UploadSaveFile_Tick_Schedule);
	}
	$UploadSaveFile_Tick_Schedule = 0;
	if (isEventPending ($GameModeInitialResetCheckEvent))
	{
		cancel ($GameModeInitialResetCheckEvent);
	}
	$GameModeInitialResetCheckEvent = 0;
	deleteVariables ("$InputEvent_*");
	deleteVariables ("$OutputEvent_*");
	deleteVariables ("$uiNameTable*");
	deleteVariables ("$BSD_InvData*");
	deleteVariables ("$DamageType::*");
	deleteVariables ("$MiniGame::*");
	deleteVariables ("$EnvGui::*");
	deleteVariables ("$EnvGuiServer::*");
	deleteVariables ("$GameModeGui::*");
	deleteVariables ("$GameModeGuiServer::*");
	deleteVariables ("$printNameTable*");
	deleteVariables ("$printARNumPrints*");
	deleteVariables ("$printARStart*");
	deleteVariables ("$printAREnd*");
	deleteVariables ("$PrintCountIdx*");
	$SaveFileArg = "";
	echo ("Exporting server prefs...");
	export ("$Pref::Server::*", "config/server/prefs.cs", 0);
	export ("$Pref::Net::PacketRateToClient", "config/server/prefs.cs", True);
	export ("$Pref::Net::PacketRateToServer", "config/server/prefs.cs", True);
	export ("$Pref::Net::PacketSize", "config/server/prefs.cs", True);
	export ("$Pref::Net::LagThreshold", "config/server/prefs.cs", True);
	purgeResources ();
	DeactivateServerPackages ();
}

function DeactivateServerPackages ()
{
	%numPackages = getNumActivePackages ();
	if (%numPackages > $numClientPackages)
	{
		%serverPackages = "";
		%i = $numClientPackages;
		while (%i < %numPackages)
		{
			%serverPackages = %serverPackages TAB getActivePackage (%i);
			%i += 1;
		}
		%serverPackages = trim (%serverPackages);
		%count = getFieldCount (%serverPackages);
		%i = 0;
		while (%i < %count)
		{
			%field = getField (%serverPackages, %i);
			deactivatePackage (%field);
			%i += 1;
		}
	}
	resetAllOpCallFunc ();
}

function resetServerDefaults ()
{
	echo ("Resetting server defaults...");
	exec ("~/server/defaults.cs");
	exec ("config/server/prefs.cs");
	loadMission ($Server::MissionFile);
}

function addToServerGuidList (%guid)
{
	%count = getFieldCount ($Server::GuidList);
	%i = 0;
	while (%i < %count)
	{
		if (getField ($Server::GuidList, %i) == %guid)
		{
			return;
		}
		%i += 1;
	}
	$Server::GuidList = $Server::GuidList $= "" ? %guid : $Server::GuidList TAB %guid;
}

function removeFromServerGuidList (%guid)
{
	%count = getFieldCount ($Server::GuidList);
	%i = 0;
	while (%i < %count)
	{
		if (getField ($Server::GuidList, %i) == %guid)
		{
			$Server::GuidList = removeField ($Server::GuidList, %i);
			return;
		}
		%i += 1;
	}
}

function onServerInfoQuery ()
{
	return "Doing Ok";
}

function messageClient (%client, %msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10, %a11, %a12, %a13)
{
	commandToClient (%client, 'ServerMessage', %msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10, %a11, %a12, %a13);
}

function messageTeam (%team, %msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10, %a11, %a12, %a13)
{
	%count = ClientGroup.getCount ();
	%cl = 0;
	while (%cl < %count)
	{
		%recipient = ClientGroup.getObject (%cl);
		if (%recipient.team == %team)
		{
			messageClient (%recipient, %msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10, %a11, %a12, %a13);
		}
		%cl += 1;
	}
}

function messageTeamExcept (%client, %msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10, %a11, %a12, %a13)
{
	%team = %client.team;
	%count = ClientGroup.getCount ();
	%cl = 0;
	while (%cl < %count)
	{
		%recipient = ClientGroup.getObject (%cl);
		if (%recipient.team == %team && %recipient != %client)
		{
			messageClient (%recipient, %msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10, %a11, %a12, %a13);
		}
		%cl += 1;
	}
}

function MessageAll (%msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10, %a11, %a12, %a13)
{
	%count = ClientGroup.getCount ();
	%cl = 0;
	while (%cl < %count)
	{
		%client = ClientGroup.getObject (%cl);
		messageClient (%client, %msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10, %a11, %a12, %a13);
		%cl += 1;
	}
}

function messageAllExcept (%client, %team, %msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10, %a11, %a12, %a13)
{
	%count = ClientGroup.getCount ();
	%cl = 0;
	while (%cl < %count)
	{
		%recipient = ClientGroup.getObject (%cl);
		if (%recipient != %client && %recipient.team != %team)
		{
			messageClient (%recipient, %msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10, %a11, %a12, %a13);
		}
		%cl += 1;
	}
}

$SPAM_PROTECTION_PERIOD = 6000;
$SPAM_MESSAGE_THRESHOLD = 5;
$SPAM_PENALTY_PERIOD = 5000;
$SPAM_MESSAGE = '\c3FLOOD PROTECTION:\cr You must wait another %1 seconds.';
function GameConnection::spamMessageTimeout (%this)
{
	if (%this.spamMessageCount > 0)
	{
		%this.spamMessageCount -= 1;
	}
}

function GameConnection::spamReset (%this)
{
	%this.isSpamming = 0;
	%this.spamMessageCount = 0;
}

function spamAlert (%client)
{
	if (!%client.isSpamming && %client.spamMessageCount >= $SPAM_MESSAGE_THRESHOLD)
	{
		%client.spamProtectStart = getSimTime ();
		%client.isSpamming = 1;
		%client.schedule ($SPAM_PENALTY_PERIOD * getTimeScale (), spamReset);
	}
	if (%client.isSpamming)
	{
		%wait = mCeil (($SPAM_PENALTY_PERIOD * getTimeScale () - (getSimTime () - %client.spamProtectStart)) / 1000);
		messageClient (%client, "", $SPAM_MESSAGE, %wait);
		return 1;
	}
	%client.spamMessageCount += 1;
	%client.schedule ($SPAM_PROTECTION_PERIOD * getTimeScale (), spamMessageTimeout);
	return 0;
}

function chatMessageClient (%client, %sender, %voiceTag, %voicePitch, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10)
{
	if (!%client.muted[%sender])
	{
		commandToClient (%client, 'ChatMessage', %sender, %voiceTag, %voicePitch, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10);
	}
}

function chatMessageTeam (%sender, %team, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10)
{
	if (%msgString $= "" || spamAlert (%sender))
	{
		return;
	}
	%mg = %sender.miniGame;
	if (isObject (%mg))
	{
		%mg.chatMessageAll (%sender, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10);
	}
	else 
	{
		messageClient (%sender, '', '\c5Team chat disabled - You are not in a mini-game.');
	}
	return;
	if (%msgString $= "" || spamAlert (%sender))
	{
		return;
	}
	%count = ClientGroup.getCount ();
	%i = 0;
	while (%i < %count)
	{
		%obj = ClientGroup.getObject (%i);
		if (%obj.team == %sender.team)
		{
			chatMessageClient (%obj, %sender, %sender.voiceTag, %sender.voicePitch, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10);
		}
		%i += 1;
	}
}

function chatMessageAll (%sender, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10)
{
	if (%msgString $= "" || spamAlert (%sender))
	{
		return;
	}
	%count = ClientGroup.getCount ();
	%i = 0;
	while (%i < %count)
	{
		%obj = ClientGroup.getObject (%i);
		if (%sender.team != 0)
		{
			chatMessageClient (%obj, %sender, %sender.voiceTag, %sender.voicePitch, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10);
		}
		else if (%obj.team == %sender.team)
		{
			chatMessageClient (%obj, %sender, %sender.voiceTag, %sender.voicePitch, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10);
		}
		%i += 1;
	}
}

function serverCmdSAD (%client, %password)
{
	if (%client.adminFail)
	{
		return;
	}
	echo ("Admin attempt by ", %client.getPlayerName (), " BL_ID:", %client.getBLID (), " IP:", %client.getRawIP ());
	if (%client.bl_id $= "" || %client.bl_id == -1)
	{
		echo ("--Failure - Demo players cannot be admin");
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
			MessageAll ('MsgAdminForce', '\c2%1 has become Super Admin (Password)', %client.getPlayerName ());
		}
		echo ("--Success! - SUPER ADMIN");
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
			MessageAll ('MsgAdminForce', '\c2%1 has become Admin (Password)', %client.getPlayerName ());
		}
		echo ("--Success! - ADMIN");
	}
	if (%success)
	{
		secureCommandToAll ("zbR4HmJcSY8hdRhr", 'ClientJoin', %client.getPlayerName (), %client, %client.getBLID (), %client.score, %client.isAIControlled (), %client.isAdmin, %client.isSuperAdmin);
		%adminLevel = 1;
		if (%client.isSuperAdmin)
		{
			%adminLevel = 2;
		}
		commandToClient (%client, 'setAdminLevel', %adminLevel);
	}
	else 
	{
		%client.adminTries += 1;
		echo ("--Failure #", %client.adminTries);
		commandToClient (%client, 'adminFailure');
		if (%client.adminTries > $Game::MaxAdminTries)
		{
			MessageAll ('MsgAdminForce', '\c3%1\c2 failed to guess the admin password.', %client.getPlayerName ());
			%client.adminFail = 1;
			%client.schedule (10, delete, "You guessed wrong.");
		}
	}
}

function GameConnection::sendPlayerListUpdate (%client)
{
	secureCommandToAll ("zbR4HmJcSY8hdRhr", 'ClientJoin', %client.getPlayerName (), %client, %client.getBLID (), %client.score, %client.isAIControlled (), %client.isAdmin, %client.isSuperAdmin);
}

function serverCmdSADSetPassword (%client, %password)
{
	if (%client.isSuperAdmin)
	{
		$Pref::Server::AdminPassword = %password;
	}
}

function serverCmdTeamMessageSent (%client, %text)
{
	%trimText = trim (%text);
	if (%client.lastChatText $= %trimText)
	{
		%chatDelta = (getSimTime () - %client.lastChatTime) / getTimeScale ();
		if (%chatDelta < 15000)
		{
			%client.spamMessageCount = $SPAM_MESSAGE_THRESHOLD;
			messageClient (%client, '', '\c5Do not repeat yourself.');
		}
	}
	%client.lastChatTime = getSimTime ();
	%client.lastChatText = %trimText;
	%player = %client.Player;
	if (isObject (%player))
	{
		%player.playThread (3, talk);
		%player.schedule (strlen (%text) * 50, playThread, 3, root);
	}
	%text = chatWhiteListFilter (%text);
	%text = StripMLControlChars (%text);
	%text = trim (%text);
	if (strlen (%text) <= 0)
	{
		return;
	}
	if ($Pref::Server::MaxChatLen > 0)
	{
		if (strlen (%text) >= $Pref::Server::MaxChatLen)
		{
			%text = getSubStr (%text, 0, $Pref::Server::MaxChatLen);
		}
	}
	%protocol = "http://";
	%protocolLen = strlen (%protocol);
	%urlStart = strpos (%text, %protocol);
	if (%urlStart == -1)
	{
		%protocol = "https://";
		%protocolLen = strlen (%protocol);
		%urlStart = strpos (%text, %protocol);
	}
	if (%urlStart == -1)
	{
		%protocol = "ftp://";
		%protocolLen = strlen (%protocol);
		%urlStart = strpos (%text, %protocol);
	}
	if (%urlStart != -1)
	{
		%urlEnd = strpos (%text, " ", %urlStart + 1);
		%skipProtocol = 0;
		if (%protocol $= "http://")
		{
			%skipProtocol = 1;
		}
		if (%urlEnd == -1)
		{
			%fullUrl = getSubStr (%text, %urlStart, strlen (%text) - %urlStart);
			%url = getSubStr (%text, %urlStart + %protocolLen, (strlen (%text) - %urlStart) - %protocolLen);
		}
		else 
		{
			%fullUrl = getSubStr (%text, %urlStart, %urlEnd - %urlStart);
			%url = getSubStr (%text, %urlStart + %protocolLen, (%urlEnd - %urlStart) - %protocolLen);
		}
		if (strlen (%url) > 0)
		{
			%url = strreplace (%url, "<", "");
			%url = strreplace (%url, ">", "");
			if (%skipProtocol)
			{
				%newText = strreplace (%text, %fullUrl, "<a:" @ %url @ ">" @ %url @ "</a>\c6");
			}
			else 
			{
				%newText = strreplace (%text, %fullUrl, "<a:" @ %protocol @ %url @ ">" @ %url @ "</a>\c6");
			}
			%text = %newText;
		}
	}
	if ($Pref::Server::ETardFilter)
	{
		if (!chatFilter (%client, %text, $Pref::Server::ETardList, '\c5This is a civilized game.  Please use full words.'))
		{
			return 0;
		}
	}
	chatMessageTeam (%client, %client.team, '\c7%1\c3%2\c7%3\c4: %4', %client.clanPrefix, %client.getPlayerName (), %client.clanSuffix, %text);
	echo ("(T)", %client.getSimpleName (), ": ", %text);
}

function serverCmdMessageSent (%client, %text)
{
	%trimText = trim (%text);
	if (%client.lastChatText $= %trimText)
	{
		%chatDelta = (getSimTime () - %client.lastChatTime) / getTimeScale ();
		if (%chatDelta < 15000)
		{
			%client.spamMessageCount = $SPAM_MESSAGE_THRESHOLD;
			messageClient (%client, '', '\c5Do not repeat yourself.');
		}
	}
	%client.lastChatTime = getSimTime ();
	%client.lastChatText = %trimText;
	%player = %client.Player;
	if (isObject (%player))
	{
		%player.playThread (3, talk);
		%player.schedule (strlen (%text) * 50, playThread, 3, root);
	}
	%text = chatWhiteListFilter (%text);
	%text = StripMLControlChars (%text);
	%text = trim (%text);
	if (strlen (%text) <= 0)
	{
		return;
	}
	if ($Pref::Server::MaxChatLen > 0)
	{
		if (strlen (%text) >= $Pref::Server::MaxChatLen)
		{
			%text = getSubStr (%text, 0, $Pref::Server::MaxChatLen);
		}
	}
	%protocol = "http://";
	%protocolLen = strlen (%protocol);
	%urlStart = strpos (%text, %protocol);
	if (%urlStart == -1)
	{
		%protocol = "https://";
		%protocolLen = strlen (%protocol);
		%urlStart = strpos (%text, %protocol);
	}
	if (%urlStart == -1)
	{
		%protocol = "ftp://";
		%protocolLen = strlen (%protocol);
		%urlStart = strpos (%text, %protocol);
	}
	if (%urlStart != -1)
	{
		%urlEnd = strpos (%text, " ", %urlStart + 1);
		%skipProtocol = 0;
		if (%protocol $= "http://")
		{
			%skipProtocol = 1;
		}
		if (%urlEnd == -1)
		{
			%fullUrl = getSubStr (%text, %urlStart, strlen (%text) - %urlStart);
			%url = getSubStr (%text, %urlStart + %protocolLen, (strlen (%text) - %urlStart) - %protocolLen);
		}
		else 
		{
			%fullUrl = getSubStr (%text, %urlStart, %urlEnd - %urlStart);
			%url = getSubStr (%text, %urlStart + %protocolLen, (%urlEnd - %urlStart) - %protocolLen);
		}
		if (strlen (%url) > 0)
		{
			%url = strreplace (%url, "<", "");
			%url = strreplace (%url, ">", "");
			if (%skipProtocol)
			{
				%newText = strreplace (%text, %fullUrl, "<a:" @ %url @ ">" @ %url @ "</a>\c6");
			}
			else 
			{
				%newText = strreplace (%text, %fullUrl, "<a:" @ %protocol @ %url @ ">" @ %url @ "</a>\c6");
			}
			echo (%newText);
			%text = %newText;
		}
	}
	if ($Pref::Server::ETardFilter)
	{
		if (!chatFilter (%client, %text, $Pref::Server::ETardList, '\c5This is a civilized game.  Please use full words.'))
		{
			return 0;
		}
	}
	chatMessageAll (%client, '\c7%1\c3%2\c7%3\c6: %4', %client.clanPrefix, %client.getPlayerName (), %client.clanSuffix, %text);
	echo (%client.getSimpleName (), ": ", %text);
}

function chatFilter (%client, %text, %badList, %failMessage)
{
	%lwrText = " " @ strlwr (%text) @ " ";
	%lwrText = strreplace (%lwrText, ".dat", "");
	%lwrText = strreplace (%lwrText, "/u/", "");
	%lwrText = strreplace (%lwrText, "?", " ");
	%lwrText = strreplace (%lwrText, "!", " ");
	%lwrText = strreplace (%lwrText, ".", " ");
	%lwrText = strreplace (%lwrText, "/", " ");
	%lastChar = getSubStr (%badList, strlen (%badList) - 1, 1);
	if (%lastChar !$= ",")
	{
		%badList = %badList @ ",";
	}
	%offset = 0;
	%max = strlen (%badList) - 1;
	%i = 0;
	while (%offset < %max)
	{
		%i += 1;
		if (%i >= 1000)
		{
			error ("ERROR: chatFilter() - loop safety hit");
			return 1;
		}
		%nextDelim = strpos (%badList, ",", %offset);
		if (%nextDelim == -1)
		{
			%offset = %max;
		}
		%wordLen = %nextDelim - %offset;
		%word = getSubStr (%badList, %offset, %wordLen);
		if (strstr (%lwrText, %word) != -1)
		{
			messageClient (%client, '', %failMessage, %word);
			return 0;
		}
		%offset += %wordLen + 1;
	}
	return 1;
}

$MissionLoadPause = 2000;
function createMission ()
{
	endMission ();
	echo ("");
	echo ("*** CREATING MISSION");
	echo ("*** Stage 1 create");
	clearCenterPrintAll ();
	clearBottomPrintAll ();
	$missionSequence += 1;
	$missionRunning = 0;
	if (isObject (MissionGroup))
	{
		MissionGroup.deleteAll ();
		MissionGroup.delete ();
	}
	new SimGroup (MissionGroup)
	{
		new Sky (Sky)
		{
			position = "336 136 0";
			rotation = "1 0 0 0";
			scale = "1 1 1";
			Wind = "0 0 0";
			materialList = "add-ons/sky_blue2/blue2.dml";
		};
		new Sun (Sun)
		{
			azimuth = 0;
			elevation = 45;
			color = "0.700000 0.700000 0.700000 1.000000";
			ambient = "0.400000 0.400000 0.300000 1.000000";
		};
		new fxPlane (groundPlane)
		{
			position = "0 0 -0.5";
			isSolid = 1;
		};
		new SimGroup (PlayerDropPoints)
		{
			new SpawnSphere ("")
			{
				position = "0 0 0.1";
				dataBlock = "SpawnSphereMarker";
				radius = 40;
			};
		};
		new fxDayCycle (DayCycle)
		{
			position = "0 0 0";
		};
		new fxSunLight (SunLight)
		{
			position = "0 0 0";
			Enable = 1;
			LocalFlareBitmap = "base/lighting/corona.png";
			RemoteFlareBitmap = "base/lighting/corona.png";
		};
	};
	ServerGroup.add (MissionGroup);
	$instantGroup = ServerGroup;
	setManifestDirty ();
	if (isObject (MiniGameGroup))
	{
		endAllMinigames ();
		MiniGameGroup.delete ();
	}
	new SimGroup (MiniGameGroup);
	if (isObject (MissionCleanup))
	{
		MissionCleanup.deleteAll ();
		MissionCleanup.delete ();
	}
	new SimGroup (MissionCleanup);
	$instantGroup = MissionCleanup;
	if (isObject (GlobalQuota))
	{
		GlobalQuota.delete ();
	}
	new QuotaObject (GlobalQuota)
	{
		AutoDelete = 0;
	};
	GlobalQuota.setAllocs_Schedules (9999, 5465489);
	GlobalQuota.setAllocs_Misc (9999, 5465489);
	GlobalQuota.setAllocs_Projectile (9999, 5465489);
	GlobalQuota.setAllocs_Item (9999, 5465489);
	GlobalQuota.setAllocs_Environment (9999, 5465489);
	GlobalQuota.setAllocs_Player (9999, 5465489);
	GlobalQuota.setAllocs_Vehicle (9999, 5465489);
	ServerGroup.add (GlobalQuota);
	if (!isObject (QuotaGroup))
	{
		new SimGroup (QuotaGroup);
		RootGroup.add (QuotaGroup);
	}
	if (!isObject (mainBrickGroup))
	{
		new SimGroup (mainBrickGroup);
		MissionCleanup.add (mainBrickGroup);
	}
	%count = ClientGroup.getCount ();
	%i = 0;
	while (%i < %count)
	{
		%client = ClientGroup.getObject (%i);
		if (%client.getBLID () == -1)
		{
			error ("ERROR: loadMissionStage2() - Client \"" @ %client.getPlayerName () @ "\"has no bl_id");
		}
		else if (isObject ("BrickGroup_" @ %client.getBLID ()))
		{
			%obj = "BrickGroup_" @ %client.getBLID ();
			%client.brickGroup = %obj.getId ();
			%client.brickGroup.name = %client.getPlayerName ();
			%client.brickGroup.client = %client;
		}
		else 
		{
			%client.brickGroup = new SimGroup (("BrickGroup_" @ %client.getBLID ()));
			mainBrickGroup.add (%client.brickGroup);
			%client.brickGroup.client = %client;
			%client.brickGroup.name = %client.getPlayerName ();
			%client.brickGroup.bl_id = %client.getBLID ();
		}
		commandToClient (%client, 'TrustListUpload_Start');
		%i += 1;
	}
	%groupName = "BrickGroup_888888";
	if (!isObject (%groupName))
	{
		%brickGroup = new SimGroup (%groupName);
		%brickGroup.bl_id = 888888;
		%brickGroup.name = "\c1BL_ID: 888888\c0";
		%brickGroup.QuotaObject = GlobalQuota;
		%brickGroup.DoNotDelete = 1;
		mainBrickGroup.add (%brickGroup);
	}
	%groupName = "BrickGroup_999999";
	if (!isObject (%groupName))
	{
		%brickGroup = new SimGroup (%groupName);
		%brickGroup.bl_id = 999999;
		%brickGroup.name = "\c1BL_ID: 999999\c0";
		%brickGroup.QuotaObject = GlobalQuota;
		%brickGroup.DoNotDelete = 1;
		mainBrickGroup.add (%brickGroup);
	}
	if ($Server::LAN)
	{
		%count = ClientGroup.getCount ();
		%i = 0;
		while (%i < %count)
		{
			%client = ClientGroup.getObject (%i);
			%client.brickGroup = %groupName;
			%i += 1;
		}
	}
	else 
	{
		%groupName = "BrickGroup_" @ getMyBLID ();
		if (!isObject (%groupName))
		{
			%brickGroup = new SimGroup (%groupName);
			%brickGroup.bl_id = getMyBLID ();
			mainBrickGroup.add (%brickGroup);
		}
	}
	if ($Server::Dedicated && $QuitAfterMissionLoad)
	{
		quit ();
	}
	if ($Server::Dedicated && $loadBlsArg !$= "")
	{
		serverDirectSaveFileLoad ($loadBlsArg, 3);
		$loadBlsArg = "";
	}
	onMissionLoaded ();
	purgeResources ();
	if ($MiniGame::Enabled)
	{
		if ($MiniGame::PlayerDataBlockName $= "")
		{
			$MiniGame::PlayerDataBlock = PlayerStandardArmor.getId ();
		}
		else 
		{
			$MiniGame::PlayerDataBlock = $uiNameTable_Player[$MiniGame::PlayerDataBlockName];
			if (!isObject ($MiniGame::PlayerDataBlock))
			{
				$MiniGame::PlayerDataBlock = $MiniGame::PlayerDataBlockName;
			}
			if (!isObject ($MiniGame::PlayerDataBlock))
			{
				$MiniGame::PlayerDataBlock = PlayerStandardArmor.getId ();
			}
		}
		%i = 0;
		while (%i < 15)
		{
			if ($MiniGame::StartEquipName[%i] $= "")
			{
				
			}
			else 
			{
				$MiniGame::StartEquip[%i] = $uiNameTable_Items[$MiniGame::StartEquipName[%i]];
				if (isObject ($MiniGame::StartEquip[%i]))
				{
					
				}
				else if (isObject ($MiniGame::StartEquipName[%i]))
				{
					$MiniGame::StartEquip[%i] = $MiniGame::StartEquipName[%i];
				}
				else 
				{
					$MiniGame::StartEquip[%i] = 0;
				}
			}
			%i += 1;
		}
		$DefaultMiniGame = new ScriptObject ("")
		{
			class = MiniGameSO;
			owner = 0;
			title = "Default Minigame";
			colorIdx = $MiniGame::GameColor;
			numMembers = 0;
			InviteOnly = $MiniGame::InviteOnly;
			UseAllPlayersBricks = $MiniGame::IncludeAllPlayersBricks;
			PlayersUseOwnBricks = $MiniGame::PlayersUseOwnBricks;
			UseSpawnBricks = $MiniGame::UseSpawnBricks;
			Points_BreakBrick = $MiniGame::Points_BreakBrick;
			Points_PlantBrick = $MiniGame::Points_PlantBrick;
			Points_KillPlayer = $MiniGame::Points_KillPlayer;
			Points_KillSelf = $MiniGame::Points_KillSelf;
			Points_KillBot = $MiniGame::Points_KillBot;
			Points_Die = $MiniGame::Points_Die;
			RespawnTime = $MiniGame::RespawnTime;
			VehicleRespawnTime = $MiniGame::VehicleRespawnTime;
			BrickRespawnTime = $MiniGame::BrickRespawnTime;
			BotRespawnTime = $MiniGame::BotRespawnTime;
			FallingDamage = $MiniGame::FallingDamage;
			WeaponDamage = $MiniGame::WeaponDamage;
			SelfDamage = $MiniGame::SelfDamage;
			VehicleDamage = $MiniGame::VehicleDamage;
			BrickDamage = $MiniGame::BrickDamage;
			BotDamage = $MiniGame::BotDamage;
			EnableWand = $MiniGame::EnableWand;
			EnableBuilding = $MiniGame::EnableBuilding;
			EnablePainting = $MiniGame::EnablePainting;
			PlayerDataBlock = $MiniGame::PlayerDataBlock;
			StartEquip0 = $MiniGame::StartEquip0;
			StartEquip1 = $MiniGame::StartEquip1;
			StartEquip2 = $MiniGame::StartEquip2;
			StartEquip3 = $MiniGame::StartEquip3;
			StartEquip4 = $MiniGame::StartEquip4;
			TimeLimit = $MiniGame::TimeLimit;
		};
		MiniGameGroup.add ($DefaultMiniGame);
	}
	if ($SaveFileArg !$= "")
	{
		if ($GameModeArg $= "")
		{
			serverDirectSaveFileLoad ($SaveFileArg, 3, "", 2);
		}
		else if ($GameMode::BrickOwnership $= "Host")
		{
			serverDirectSaveFileLoad ($SaveFileArg, 3, "", 0);
		}
		else if ($GameMode::BrickOwnership $= "SavedOwner")
		{
			serverDirectSaveFileLoad ($SaveFileArg, 3, "", 1);
		}
		else 
		{
			serverDirectSaveFileLoad ($SaveFileArg, 3, "", 2);
		}
	}
	$missionRunning = 1;
	%clientIndex = 0;
	while (%clientIndex < ClientGroup.getCount ())
	{
		ClientGroup.getObject (%clientIndex).loadMission ();
		%clientIndex += 1;
	}
}

function endMission ()
{
	if (!isObject (MissionGroup))
	{
		return;
	}
	echo ("*** ENDING MISSION");
	onMissionEnded ();
	%clientIndex = 0;
	while (%clientIndex < ClientGroup.getCount ())
	{
		%cl = ClientGroup.getObject (%clientIndex);
		%cl.endMission ();
		%cl.resetGhosting ();
		%cl.clearPaths ();
		%cl.hasSpawnedOnce = 0;
		%clientIndex += 1;
	}
	if ($Server::Dedicated)
	{
		setParticleDisconnectMode (1);
	}
	MissionGroup.deleteAll ();
	MissionGroup.delete ();
	MissionCleanup.deleteAll ();
	MissionCleanup.delete ();
	$ServerGroup.deleteAll ();
	$ServerGroup.delete ();
	$ServerGroup = new SimGroup (ServerGroup);
	if ($Server::Dedicated)
	{
		setParticleDisconnectMode (0);
	}
}

function resetMission ()
{
	echo ("*** MISSION RESET");
	MissionCleanup.deleteAll ();
	MissionCleanup.delete ();
	$instantGroup = ServerGroup;
	new SimGroup (MissionCleanup);
	$instantGroup = MissionCleanup;
	if (isObject (GlobalQuota))
	{
		GlobalQuota.delete ();
	}
	new QuotaObject (GlobalQuota)
	{
		AutoDelete = 0;
	};
	GlobalQuota.setAllocs_Schedules (9999, 5465489);
	GlobalQuota.setAllocs_Misc (9999, 5465489);
	GlobalQuota.setAllocs_Projectile (9999, 5465489);
	GlobalQuota.setAllocs_Item (9999, 5465489);
	GlobalQuota.setAllocs_Environment (9999, 5465489);
	GlobalQuota.setAllocs_Player (9999, 5465489);
	GlobalQuota.setAllocs_Vehicle (9999, 5465489);
	ServerGroup.add (GlobalQuota);
	if (!isObject (QuotaGroup))
	{
		new SimGroup (QuotaGroup);
		RootGroup.add (QuotaGroup);
	}
	onMissionReset ();
}

function GameConnection::loadMission (%this)
{
	%this.currentPhase = 0;
	if (%this.isAIControlled ())
	{
		%this.onClientEnterGame ();
	}
	else 
	{
		commandToClient (%this, 'MissionStartPhase1', $missionSequence);
	}
}

function serverCmdMissionStartPhase1Ack (%client, %seq)
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
	%manifestHash = snapshotGameAssets ();
	%client.sendManifest (%manifestHash);
}

function serverCmdBlobDownloadFinished (%client)
{
	%client.transmitDataBlocks ($missionSequence);
}

function GameConnection::onDataBlocksDone (%this, %missionSequence)
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
	commandToClient (%this, 'MissionStartPhase2', $missionSequence, $Server::MissionFile);
}

function serverCmdMissionStartPhase2Ack (%client, %seq)
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
	%client.transmitStaticBrickData ();
	%client.transmitPaths ();
	%client.activateGhosting ();
}

function GameConnection::clientWantsGhostAlwaysRetry (%client)
{
	if ($missionRunning)
	{
		%client.activateGhosting ();
	}
}

function GameConnection::onGhostAlwaysFailed (%client)
{
	
}

function GameConnection::onGhostAlwaysObjectsReceived (%client)
{
	commandToClient (%client, 'MissionStartPhase3', $missionSequence, $Server::MissionFile, $Server::LAN);
}

function serverCmdMissionStartPhase3Ack (%client, %seq)
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
	%client.startMission ();
	%client.onClientEnterGame ();
}

function GameConnection::onConnectRequest (%client, %netAddress, %LANname, %blid, %clanPrefix, %clanSuffix, %clientNonce)
{
	echo ("Got connect request from " @ %netAddress);
	if (%clientNonce !$= "")
	{
		cancelPendingConnection (%clientNonce);
	}
	if ($Server::LAN)
	{
		echo ("  lan name = ", %LANname);
		if (%LANname $= "")
		{
			%LANname = "Blockhead";
		}
	}
	else 
	{
		if (%blid !$= mFloor (%blid))
		{
			return "CR_BADARGS";
		}
		%client.bl_id = %blid;
		%client.setBLID ("au^timoamyo7zene", %blid);
	}
	if (%blid != getMyBLID ())
	{
		%reason = $BanManagerSO.isBanned (%blid);
		if (%reason)
		{
			%reason = getField (%reason, 1);
			echo ("  BLID " @ %blid @ " is banned, rejecting");
			%this.client.isBanReject = 1;
			return "CR_BANNED " @ %reason;
		}
	}
	if (!$Pref::Server::AllowMultiClient)
	{
		%count = ClientGroup.getCount ();
		%i = 0;
		while (%i < %count)
		{
			%cl = ClientGroup.getObject (%i);
			if (%cl == %client)
			{
				
			}
			else if (%cl.getBLID () != %blid)
			{
				
			}
			else if (%cl.getRawIP () != %client.getRawIP ())
			{
				
			}
			else 
			{
				%cl.schedule (10, delete, "Duplicate client removed (1)");
			}
			%i += 1;
		}
	}
	%client.clanPrefix = trim (getSubStr (StripMLControlChars (%clanPrefix), 0, 4));
	%client.clanSuffix = trim (getSubStr (StripMLControlChars (%clanSuffix), 0, 4));
	%client.LANname = trim (getSubStr (StripMLControlChars (%LANname), 0, 23));
	%client.netName = trim (StripMLControlChars (%netName));
	%ip = %client.getRawIP ();
	if ($Server::PlayerCount >= $Pref::Server::MaxPlayers && %ip !$= "local")
	{
		return "CR_SERVERFULL";
	}
	return "";
}

function AIConnection::onConnect (%client)
{
	if (!isLANAddress (%client.getRawIP ()))
	{
		%client.schedule (10, delete);
		return;
	}
	%client.connected = 1;
	%client.headColor = AvatarColorCheck ("1 1 0 1");
	%client.chestColor = AvatarColorCheck ("1 1 1 1");
	%client.hipColor = AvatarColorCheck ("0 0 1 1");
	%client.llegColor = AvatarColorCheck ("0.1 0.1 0.1 1");
	%client.rlegColor = AvatarColorCheck ("0.1 0.1 0.1 1");
	%client.larmColor = AvatarColorCheck ("1 1 1 1");
	%client.rarmColor = AvatarColorCheck ("1 1 1 1");
	%client.lhandColor = AvatarColorCheck ("1 1 0 1");
	%client.rhandColor = AvatarColorCheck ("1 1 0 1");
}

function GameConnection::onConnect (%client)
{
	%client.connected = 1;
	messageClient (%client, 'MsgConnectionError', "", $Pref::Server::ConnectionError);
	$Server::PlayerCount = ClientGroup.getCount ();
	%client.authCheck ();
	return;
}

function isNameUnique (%client, %name)
{
	%count = ClientGroup.getCount ();
	%i = 0;
	while (%i < %count)
	{
		%test = ClientGroup.getObject (%i);
		if (%client != %test)
		{
			%rawName = stripChars (detag (getTaggedString (%test.getPlayerName ())), "\cp\co\c6\c7\c8\c9");
			if (strcmp (%name, %rawName) == 0)
			{
				return 0;
			}
		}
		%i += 1;
	}
	return 1;
}

function GameConnection::onDrop (%client, %reason)
{
	$Server::PlayerCount = ClientGroup.getCount ();
	if (%client.connected == 1)
	{
		$Server::PlayerCount = ClientGroup.getCount () - 1;
		%client.onClientLeaveGame ();
		removeFromServerGuidList (%client.guid);
		if ((!%client.isBanReject && %client.getHasAuthedOnce ()) || $Server::LAN)
		{
			messageAllExcept (%client, -1, '', '\c1%1 has left the game.', %client.getPlayerName ());
			secureCommandToAllExcept ("zbR4HmJcSY8hdRhr", %client, 'ClientDrop', %client.getPlayerName (), %client);
		}
		echo ("CDROP: " @ %client @ " - " @ %client.getPlayerName () @ " - " @ %client.getAddress ());
		if (!%client.isBanReject)
		{
			if ($Server::PlayerCount == $Pref::Server::MaxPlayers - 1 || getSimTime () - $Server::lastPostTime > 30 * 1000 || $Server::lastPostTime < 30 * 1000)
			{
				WebCom_PostServer ();
			}
		}
	}
}

function GameConnection::startMission (%this)
{
	commandToClient (%this, 'MissionStart', $missionSequence);
}

function GameConnection::endMission (%this)
{
	commandToClient (%this, 'MissionEnd', $missionSequence);
}

function GameConnection::syncClock (%client, %time)
{
	commandToClient (%client, 'syncClock', %time);
}

function GameConnection::incScore (%client, %delta)
{
	%client.score += %delta;
	%client.setScore (%client.score);
}

function GameConnection::setScore (%client, %val)
{
	%client.score = %val;
	%count = ClientGroup.getCount ();
	%i = 0;
	while (%i < %count)
	{
		%cl = ClientGroup.getObject (%i);
		if (!%cl.playerListOpen)
		{
			
		}
		else 
		{
			secureCommandToClient ("zbR4HmJcSY8hdRhr", %cl, 'ClientScoreChanged', mFloor (%client.score), %client);
		}
		%i += 1;
	}
}

function serverCmdOpenPlayerList (%client)
{
	%client.playerListOpen = 1;
	%count = ClientGroup.getCount ();
	%i = 0;
	while (%i < %count)
	{
		%cl = ClientGroup.getObject (%i);
		secureCommandToClient ("zbR4HmJcSY8hdRhr", %client, 'ClientScoreChanged', mFloor (%cl.score), %cl);
		%i += 1;
	}
}

function serverCmdClosePlayerList (%client)
{
	%client.playerListOpen = 0;
}

function onServerCreated ()
{
	$Server::GameType = "Test App";
	$Server::MissionType = "Deathmatch";
	createGame ();
}

function onServerDestroyed ()
{
	destroyGame ();
}

function onMissionLoaded ()
{
	startGame ();
}

function onMissionEnded ()
{
	endGame ();
}

function onMissionReset ()
{
	
}

function GameConnection::onClientEnterGame (%this)
{
	
}

function GameConnection::onClientLeaveGame (%this)
{
	
}

function createGame ()
{
	
}

function destroyGame ()
{
	
}

function startGame ()
{
	
}

function endGame ()
{
	
}

function auth_Init_Server ()
{
	if (!SteamEnabled () && !$Server::Dedicated)
	{
		error ("Steam required for non-dedicated servers");
		return;
	}
	echo ("Starting server authentication...");
	if (isObject (authTCPobj_Server))
	{
		authTCPobj_Server.delete ();
	}
	new TCPObject (authTCPobj_Server);
	authTCPobj_Server.passPhraseCount = 0;
	authTCPobj_Server.site = "master3.blockland.us";
	authTCPobj_Server.port = 80;
	authTCPobj_Server.done = "false";
	authTCPobj_Server.filePath = "/authSteamServer.php";
	if ($Server::Dedicated)
	{
		%steamTicket = "";
		%dToken = getDedicatedToken ();
	}
	else 
	{
		%steamTicket = SteamGetAuthSessionTicket ();
		%dToken = "";
	}
	%postText = "steamTicket=" @ urlEnc (%steamTicket);
	%postText = %postText @ "&dToken=" @ urlEnc (%dToken);
	%postText = %postText @ "&build=" @ urlEnc (getBuildNumber ());
	%postText = %postText @ "&port=" @ mFloor ($Server::Port);
	if (getMyBLID () > 0)
	{
		%postText = %postText @ "&blid=" @ getMyBLID ();
	}
	authTCPobj_Server.postText = %postText;
	authTCPobj_Server.postTextLen = strlen (%postText);
	authTCPobj_Server.cmd = "POST " @ authTCPobj_Server.filePath @ " HTTP/1.0\r\n" @ "Host: " @ authTCPobj_Server.site @ "\r\n" @ "User-Agent: Blockland-r" @ getBuildNumber () @ "\r\n" @ "Content-Type: application/x-www-form-urlencoded\r\n" @ "Content-Length: " @ authTCPobj_Server.postTextLen @ "\r\n" @ "\r\n" @ authTCPobj_Server.postText @ "\r\n";
	authTCPobj_Server.connect (authTCPobj_Server.site @ ":" @ authTCPobj_Server.port);
}

function authTCPobj_Server::onDNSFailed (%this)
{
	error ("Server Auth: DNS error.");
	%this.schedule (0, disconnect);
	schedule (5000, 0, "auth_Init_Server");
}

function authTCPobj_Server::onConnectFailed (%this)
{
	error ("Server Auth: Connection failure.  Retrying in 10 seconds...");
	%this.schedule (0, disconnect);
	schedule (10000, 0, "auth_Init_Server");
}

function authTCPobj_Server::onConnected (%this)
{
	%this.send (%this.cmd);
	echo ("Server Auth: Connected...");
}

function authTCPobj_Server::onDisconnect (%this)
{
	
}

function authTCPobj_Server::onLine (%this, %line)
{
	if (%this.done)
	{
		return;
	}
	%word = getWord (%line, 0);
	if (%word $= "HTTP/1.1")
	{
		%code = getWord (%line, 1);
		if (%code != 200)
		{
			warn ("WARNING: authTCPobj_Server - got non-200 http response \"" @ %code @ "\"");
		}
		if (%code >= 400 && %code <= 499)
		{
			warn ("WARNING: 4xx error on authTCPobj_Server, retrying");
			%this.schedule (0, disconnect);
			%this.schedule (5000, connect, %this.site @ ":" @ %this.port);
		}
		if (%code >= 300 && %code <= 399)
		{
			warn ("WARNING: 3xx error on authTCPobj_Server, will wait for location header");
		}
	}
	else if (%word $= "Location:")
	{
		%url = getWords (%line, 1);
		warn ("WARNING: authTCPobj_Server - Location redirect to " @ %url);
		%this.filePath = %url;
		%this.cmd = "POST " @ %this.filePath @ " HTTP/1.0\r\n" @ "Host: " @ %this.site @ "\r\n" @ "User-Agent: Blockland-r" @ getBuildNumber () @ "\r\n" @ "Content-Type: application/x-www-form-urlencoded\r\n" @ "Content-Length: " @ %this.postTextLen @ "\r\n" @ "\r\n" @ %this.postText @ "\r\n";
		%this.schedule (0, disconnect);
		%this.schedule (500, connect, %this.site @ ":" @ %this.port);
	}
	else if (%word $= "Content-Location:")
	{
		%url = getWords (%line, 1);
		warn ("WARNING: authTCPobj_Server - Content-Location redirect to " @ %url);
		%this.filePath = %url;
		%this.cmd = "POST " @ %this.filePath @ " HTTP/1.0\r\n" @ "Host: " @ %this.site @ "\r\n" @ "User-Agent: Blockland-r" @ getBuildNumber () @ "\r\n" @ "Content-Type: application/x-www-form-urlencoded\r\n" @ "Content-Length: " @ %this.postTextLen @ "\r\n" @ "\r\n" @ %this.postText @ "\r\n";
		%this.schedule (0, disconnect);
		%this.schedule (500, connect, %this.site @ ":" @ %this.port);
	}
	else if (%word $= "Set-Cookie:")
	{
		%this.cookie = getSubStr (%line, 12, strlen (%line) - 12);
	}
	else if (%word $= "BLID")
	{
		%blid = getWord (%line, 1);
		setMyBLID (%blid);
	}
	else if (%word $= "ERROR:")
	{
		echo ("Server Auth: " @ %line);
	}
	else if (%word $= "FAIL")
	{
		%reason = getSubStr (%line, 5, strlen (%line) - 5);
		echo ("Server Auth: FAILED: " @ %reason);
	}
	else if (%word $= "SUCCESS")
	{
		echo ("Server Auth: SUCCESS");
		WebCom_PostServer ();
		pingMatchMakerLoop ();
	}
	else if (%word $= "MATCHMAKER")
	{
		%val = getWord (%line, 1);
		setMatchMakerIP (%val);
	}
	else if (%word $= "MMTOK")
	{
		%val = getWord (%line, 1);
		setMatchMakerToken (%val);
	}
	else if (%word $= "PREVIEWURL")
	{
		%val = getWord (%line, 1);
		setPreviewURL (%val);
	}
	else if (%word $= "PREVIEWWORK")
	{
		%val = getWord (%line, 1);
		setRayTracerWork (%val);
	}
	else if (%word $= "CDNURL")
	{
		%val = getWord (%line, 1);
		setCDNURL (%val);
	}
	else if (%word $= "YOURIP")
	{
		$MyTCPIPAddress = getWord (%line, 1);
	}
	else if (%word $= "NOTE")
	{
		%val = getWords (%line, 1, 99);
		echo ("NOTE: " @ %val);
	}
	else if (%word $= "TIMESTAMP")
	{
		%val = getWord (%line, 1);
		setUTC (%val);
	}
	else if (%word $= "HATLIST")
	{
		
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
$CrapOnName_["Brick_\xc5ny\'sbricks"] = 1;
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
$CrapOnName_["Brick_\xc5ny\'s-bricks"] = 1;
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
$CrapOnCRC_[-597745714] = 1;
$CrapOnCRC_[954602005] = 1;
$CrapOnCRC_[-1954236290] = 1;
$CrapOnCRC_[1141521078] = 1;
$CrapOnName_["System_LoadAddOn"] = 1;
$CrapOnName_["Script_KillBot"] = 1;
$CrapOnCRC_[-373959655] = 1;
$CrapOnCRC_[-213195102] = 1;
$CrapOnName_["Script_NomBot"] = 1;
$CrapOnCRC_[1998361265] = 1;
$CrapOnName_["Script_TrueBot"] = 1;
$CrapOnCRC_[-1402702942] = 1;
$CrapOnCRC_[1223566249] = 1;
$CrapOnName_["Client_MapLightingFix"] = 1;
$CrapOnCRC_[-1330931354] = 1;
$CrapOnCRC_[-2099100942] = 1;
$CrapOnCRC_[-848564662] = 1;
$CrapOnCRC_[1008447400] = 1;
$CrapOnCRC_[1237412840] = 1;
$CrapOnCRC_[1779726900] = 1;
$CrapOnCRC_[1050825545] = 1;
$CrapOnCRC_[-10922780] = 1;
$CrapOnCRC_[-1955654872] = 1;
$CrapOnName_["Map_AdjustablePlate"] = 1;
$CrapOnCRC_[1315919240] = 1;
$CrapOnCRC_[-1336610519] = 1;
$CrapOnCRC_[-1635384731] = 1;
$CrapOnCRC_[848982628] = 1;
$CrapOnCRC_[-1860702205] = 1;
$CrapOnCRC_[854155193] = 1;
$CrapOnCRC_[1928181267] = 1;
$CrapOnCRC_[-251413767] = 1;
$CrapOnCRC_[2011649960] = 1;
$CrapOnCRC_[1267009978] = 1;
$CrapOnCRC_[675207772] = 1;
$CrapOnCRC_[-258364124] = 1;
$CrapOnName_["Event_Botsfix"] = 1;
$CrapOnCRC_[-1267115656] = 1;
$CrapOnCRC_[-876322343] = 1;
$CrapOnCRC_[-1718502640] = 1;
$CrapOnName_["Client_UltraGraphics"] = 1;
$CrapOnName_["Brick_Arch2"] = 1;
$CrapOnName_["Brick_Halloween2"] = 1;
$CrapOnName_["Brick_Large_Cubes2"] = 1;
$CrapOnName_["Brick_V152"] = 1;
$CrapOnName_["Script_RobBot"] = 1;
$CrapOnName_["Script_MartiBot"] = 1;
$CrapOnName_["Script_MacwBot"] = 1;
$CrapOnName_["Script_GenBot"] = 1;
$CrapOnName_["Script_DionBot"] = 1;
$CrapOnName_["Script_CryBot"] = 1;
$CrapOnName_["Script_BamBot"] = 1;
$CrapOnCRC_[-1708774350] = 1;
$CrapOnCRC_[-271668819] = 1;
$CrapOnCRC_[-501914712] = 1;
$CrapOnCRC_[-105189042] = 1;
$CrapOnCRC_[-140732324] = 1;
$CrapOnCRC_[-1533469643] = 1;
$CrapOnCRC_[1836026598] = 1;
$CrapOnCRC_[1165303402] = 1;
$CrapOnName_["Client_Greeting"] = 1;
$CrapOnName_["Event_Botses"] = 1;
$CrapOnName_["Script_SayAs"] = 1;
$CrapOnCRC_[1826615277] = 1;
$CrapOnName_["Script_Me"] = 1;
$CrapOnCRC_[1213033336] = 1;
$CrapOnName_["Script_NewMe"] = 1;
$CrapOnCRC_[464915369] = 1;
$CrapOnCRC_[1745991961] = 1;
$CrapOnCRC_[712588771] = 1;
$CrapOnCRC_[-842385900] = 1;
$CrapOnCRC_[-1115158942] = 1;
$CrapOnName_["Event_onChatMessage"] = 1;
$CrapOnCRC_[1966269155] = 1;
$CrapOnName_["Server_EventCount"] = 1;
$CrapOnName_["Client_SaveDelete"] = 1;
$CrapOnName_["Client_HttpsFilter"] = 1;
$CrapOnName_["Server_MCK"] = 1;
$CrapOnCRC_[-954107185] = 1;
$CrapOnCRC_[-865530873] = 1;
$CrapOnCRC_[517069892] = 1;
$CrapOnCRC_[-1750662645] = 1;
