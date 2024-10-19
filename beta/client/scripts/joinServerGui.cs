function JoinServerGui::onWake()
{
	JS_joinServer.setActive(JS_serverList.rowCount() > 0);
}

function JoinServerGui::queryWebMaster(%this)
{
	$TotalPlayerCount = 0;
	$TotalServerCount = 0;
	$JoinNetServer = 1;
	$MasterQueryCanceled = 0;
	if (isObject(queryMasterTCPObj))
	{
		queryMasterTCPObj.delete();
	}
	new TCPObject(queryMasterTCPObj);
	queryMasterTCPObj.site = "master.blockland.us";
	queryMasterTCPObj.port = 80;
	queryMasterTCPObj.filePath = "/blm/index.asp";
	queryMasterTCPObj.cmd = "GET " @ queryMasterTCPObj.filePath @ " HTTP/1.1\r\nHost: " @ queryMasterTCPObj.site @ "\r\n\r\n";
	queryMasterTCPObj.connect(queryMasterTCPObj.site @ ":" @ queryMasterTCPObj.port);
}

function queryMasterTCPObj::onConnected(%this)
{
	%this.send(%this.cmd);
}

function queryMasterTCPObj::onLine(%this, %line)
{
	if (%this.done)
	{
		return;
	}
	if (%line $= "END")
	{
		echo("---done---");
		ServerInfoSO_DisplayAll();
		ServerInfoSO_StartPingAll();
		%this.done = 1;
		%this.disconnect();
		return;
	}
	if (%line $= "START<BR>" || %line $= "START")
	{
		%this.gotHeader = 1;
		ServerInfoSO_ClearAll();
		echo("---reading entries----");
		return;
	}
	if (%this.gotHeader)
	{
		if (getWord(%line, 0) !$= "**OLD**")
		{
			echo(%line);
			%offset = 0;
			%pos = strpos(%line, "|", %offset);
			%time = getSubStr(%line, %offset, %pos - %offset);
			%offset = %pos + 1;
			%pos = strpos(%line, "|", %offset);
			%ip = getSubStr(%line, %offset, %pos - %offset);
			%offset = %pos + 1;
			%pos = strpos(%line, "|", %offset);
			%pass = getSubStr(%line, %offset, %pos - %offset);
			%offset = %pos + 1;
			%pos = strpos(%line, "|", %offset);
			%ded = getSubStr(%line, %offset, %pos - %offset);
			%offset = %pos + 1;
			%pos = strpos(%line, "|", %offset);
			%name = getSubStr(%line, %offset, %pos - %offset);
			%offset = %pos + 1;
			%pos = strpos(%line, "|", %offset);
			%players = getSubStr(%line, %offset, %pos - %offset);
			%offset = %pos + 1;
			%pos = strpos(%line, "|", %offset);
			%mods = getSubStr(%line, %offset, %pos - %offset);
			%offset = %pos + 1;
			%pos = strpos(%line, "|", %offset);
			%map = getSubStr(%line, %offset, %pos - %offset);
			%pos = strpos(%players, "/");
			%currPlayers = getSubStr(%players, 0, %pos);
			%maxPlayers = getSubStr(%players, %pos + 1, strlen(%players) - %pos);
			echo("curr players = ", %currPlayers);
			echo("max players = ", %maxPlayers);
			%ping = "???";
			if (%ded $= "Y")
			{
				%ded = "Yes";
			}
			else
			{
				%ded = "No";
			}
			if (%pass $= "Y")
			{
				%pass = "Yes";
			}
			else
			{
				%pass = "No";
			}
			ServerInfoSO_Add(%ip, %pass, %ded, %name, %currPlayers, %maxPlayers, %mods, %map);
		}
	}
}

function JoinServerGui::query(%this)
{
	%flags = $Pref::Filter::Dedicated | $Pref::Filter::NoPassword << 1 | $Pref::Filter::LinuxServer << 2 | $Pref::Filter::WindowsServer << 3 | $Pref::Filter::TeamDamageOn << 4 | $Pref::Filter::TeamDamageOff << 5 | $Pref::Filter::CurrentVersion << 7;
	queryMasterServer(0, $Client::GameTypeQuery, $Client::MissionTypeQuery, $pref::Filter::minPlayers, 100, 0, 2, $pref::Filter::maxPing, $pref::Filter::minCpu, %flags);
}

function JoinServerGui::queryLan(%this)
{
	$JoinNetServer = 0;
	%flags = $Pref::Filter::Dedicated | $Pref::Filter::NoPassword << 1 | $Pref::Filter::LinuxServer << 2 | $Pref::Filter::WindowsServer << 3 | $Pref::Filter::TeamDamageOn << 4 | $Pref::Filter::TeamDamageOff << 5 | $Pref::Filter::CurrentVersion << 7;
	echo("flags = ", %flags);
	queryLanServers(28000, 0, $Client::GameTypeQuery, $Client::MissionTypeQuery, $pref::Filter::minPlayers, 100, 0, 2, $pref::Filter::maxPing, $pref::Filter::minCpu, %flags);
}

function JoinServerGui::cancel(%this)
{
	$MasterQueryCanceled = 1;
	cancelServerQuery();
	JS_queryStatus.setVisible(0);
}

function JoinServerGui::join(%this)
{
	$MasterQueryCanceled = 1;
	cancelServerQuery();
	%id = JS_serverList.getSelectedId();
	if ($JoinNetServer)
	{
		%idx = JS_serverList.getSelectedId();
		if (%idx < 0)
		{
			return;
		}
		%so = $ServerSO[%idx];
		echo("obj = ", %so);
		if (isObject(%so))
		{
			$ServerInfo::Address = %so.ip;
			if (%so.Pass $= "Yes")
			{
				$ServerInfo::Password = 1;
			}
			else
			{
				$ServerInfo::Password = 0;
			}
		}
		else
		{
			return;
		}
	}
	else
	{
		%index = getField(JS_serverList.getRowTextById(%id), 8);
		if (!setServerInfo(%index))
		{
			return;
		}
	}
	if (isObject($conn))
	{
		$conn.delete();
	}
	if ($ServerInfo::Password)
	{
		Canvas.pushDialog("joinServerPassGui");
	}
	else
	{
		$conn = new GameConnection(ServerConnection);
		$conn.setConnectArgs($pref::Player::Name);
		$conn.setJoinPassword("");
		$conn.connect($ServerInfo::Address);
	}
}

function JoinServerGui::exit(%this)
{
	$MasterQueryCanceled = 1;
	cancelServerQuery();
	Canvas.setContent(MainMenuGui);
}

function JoinServerGui::update(%this)
{
	JS_queryStatus.setVisible(0);
	JS_serverList.clear();
	%sc = getServerCount();
	%playerCount = 0;
	for (%i = 0; %i < %sc; %i++)
	{
		setServerInfo(%i);
		JS_serverList.addRow(%i, ($ServerInfo::Password ? "Yes" : "No") TAB ($ServerInfo::Dedicated ? "D" : "L") TAB $ServerInfo::Name TAB $ServerInfo::Ping TAB $ServerInfo::PlayerCount TAB "/" TAB $ServerInfo::MaxPlayers TAB $ServerInfo::Version TAB $ServerInfo::MissionName TAB $ServerInfo::PlayerCount TAB %i);
		%playerCount = %playerCount + $ServerInfo::PlayerCount;
	}
	JS_serverList.sort(0);
	JS_serverList.setSelectedRow(0);
	JS_serverList.scrollVisible(0);
	%text = "";
	if (%playerCount == 1)
	{
		%text = %playerCount @ " Player / ";
	}
	else
	{
		%text = %playerCount @ " Players / ";
	}
	if (%sc == 1)
	{
		%text = %text @ %sc @ " Server";
	}
	else
	{
		%text = %text @ %sc @ " Servers";
	}
	JS_window.setText("Join Server - " @ %text);
	JS_joinServer.setActive(JS_serverList.rowCount() > 0);
}

function onServerQueryStatus(%status, %msg, %value)
{
	if (!JS_queryStatus.isVisible())
	{
		JS_queryStatus.setVisible(1);
	}
	if (%status $= "start")
	{
		JS_joinServer.setActive(0);
		JS_queryMaster.setActive(0);
		JS_statusText.setText(%msg);
		JS_statusBar.setValue(0);
		JS_serverList.clear();
	}
	else if (%status $= "ping")
	{
		JS_statusText.setText("Ping Servers");
		JS_statusBar.setValue(%value);
	}
	else if (%status $= "query")
	{
		JS_statusText.setText("Query Servers");
		JS_statusBar.setValue(%value);
	}
	else if (%status $= "done")
	{
		JS_queryMaster.setActive(1);
		JS_queryStatus.setVisible(0);
		JoinServerGui.update();
	}
}

function JS_sortList(%col)
{
	JS_serverList.sortedNumerical = 0;
	if (JS_serverList.sortedBy == %col)
	{
		JS_serverList.sortedAsc = !JS_serverList.sortedAsc;
		JS_serverList.sort(JS_serverList.sortedBy, JS_serverList.sortedAsc);
	}
	else
	{
		JS_serverList.sortedBy = %col;
		JS_serverList.sortedAsc = 0;
		JS_serverList.sort(JS_serverList.sortedBy, JS_serverList.sortedAsc);
	}
}

function JS_sortNumList(%col)
{
	JS_serverList.sortedNumerical = 1;
	if (JS_serverList.sortedBy == %col)
	{
		JS_serverList.sortedAsc = !JS_serverList.sortedAsc;
		JS_serverList.sortNumerical(JS_serverList.sortedBy, JS_serverList.sortedAsc);
	}
	else
	{
		JS_serverList.sortedBy = %col;
		JS_serverList.sortedAsc = 0;
		JS_serverList.sortNumerical(JS_serverList.sortedBy, JS_serverList.sortedAsc);
	}
}

function ServerInfoSO_ClearAll()
{
	for (%i = 0; %i < $ServerSO_Count; %i++)
	{
		if (isObject($ServerSO[%i]))
		{
			$ServerSO[%i].delete();
			$ServerSO[%i] = "";
		}
	}
	$ServerSO_Count = 0;
	JS_serverList.clear();
}

function ServerInfoSO_Add(%ip, %pass, %ded, %name, %currPlayers, %maxPlayers, %mods, %map)
{
	if ($ServerSO_Count <= 0)
	{
		$ServerSO_Count = 0;
	}
	$ServerSO[$ServerSO_Count] = new ScriptObject()
	{
		class = ServerSO;
		Ping = "???";
		ip = %ip;
		Pass = %pass;
		Ded = %ded;
		name = %name;
		currPlayers = %currPlayers;
		maxPlayers = %maxPlayers;
		mods = %mods;
		Map = %map;
		id = $ServerSO_Count;
	};
	%strIP = %ip;
	%strIP = strreplace(%strIP, ".", "_");
	%strIP = strreplace(%strIP, ":", "X");
	$ServerSOFromIP[%strIP] = $ServerSO_Count;
	$ServerSO_Count++;
}

function ServerInfoSO_DisplayAll()
{
	JS_serverList.clear();
	for (%i = 0; %i < $ServerSO_Count; %i++)
	{
		%obj = $ServerSO[%i];
		%rowText = %obj.serialize();
		echo("%rowtext = ", %rowText);
		JS_serverList.addRow(%i, %rowText);
	}
}

function ServerInfoSO_StartPingAll()
{
	$Pref::Net::MaxSimultaneousPings = 10;
	$ServerSO_PingCount = 0;
	if ($ServerSO_Count < $Pref::Net::MaxSimultaneousPings)
	{
		%count = $ServerSO_Count;
	}
	else
	{
		%count = $Pref::Net::MaxSimultaneousPings;
	}
	for (%i = 0; %i < %count; %i++)
	{
		pingSingleServer($ServerSO[%i].ip, %i);
		$ServerSO_PingCount = %i;
	}
}

function ServerInfoSO_PingNext(%slot)
{
	if (!$MasterQueryCanceled)
	{
		if ($ServerSO_PingCount < $ServerSO_Count - 1)
		{
			$ServerSO_PingCount++;
			pingSingleServer($ServerSO[$ServerSO_PingCount].ip, %slot);
		}
		else
		{
			return;
		}
	}
}

function onSimplePingReceived(%ip, %ping, %slot)
{
	echo("SCRIPT: Recieved ping packet from ", %ip, " ", %ping, "ms");
	ServerInfoSO_UpdatePing(%ip, %ping);
	ServerInfoSO_PingNext(%slot);
}

function onSimplePingTimeout(%ip, %slot)
{
	echo("SCRIPT: No response from ", %ip);
	ServerInfoSO_UpdatePing(%ip, "Dead");
	ServerInfoSO_PingNext(%slot);
}

function ServerInfoSO_UpdatePing(%ip, %ping)
{
	if (getSubStr(%ip, 0, 3) $= "IP:")
	{
		%ip = getSubStr(%ip, 3, strlen(%ip) - 3);
	}
	%strIP = %ip;
	%strIP = strreplace(%strIP, ".", "_");
	%strIP = strreplace(%strIP, ":", "X");
	%idx = $ServerSOFromIP[%strIP];
	%obj = $ServerSO[%idx];
	if (isObject(%obj))
	{
		%obj.Ping = %ping;
		echo("updated ping on obj ", %obj, " ", %obj.ip);
		$TotalServerCount++;
		$TotalPlayerCount += %obj.currPlayers;
		%text = "";
		if ($TotalPlayerCount == 1)
		{
			%text = $TotalPlayerCount @ " Player / ";
		}
		else
		{
			%text = $TotalPlayerCount @ " Players / ";
		}
		if ($TotalServerCount == 1)
		{
			%text = %text @ $TotalServerCount @ " Server";
		}
		else
		{
			%text = %text @ $TotalServerCount @ " Servers";
		}
		JS_window.setText("Join Server - " @ %text);
		%obj.Display();
	}
	else
	{
		error("No script object found for ip: ", %strIP);
	}
}

function ServerSO::serialize(%this)
{
	%ret = %this.Pass TAB %this.Ded TAB %this.name TAB %this.Ping TAB %this.currPlayers TAB "/" TAB %this.maxPlayers TAB %this.mods TAB %this.Map TAB %this.ip;
	return %ret;
}

function ServerSO::Display(%this)
{
	JS_serverList.removeRowById(%this.id);
	JS_serverList.addRow(%this.id, %this.serialize());
	if (JS_serverList.sortedNumerical)
	{
		JS_serverList.sortNumerical(JS_serverList.sortedBy, JS_serverList.sortedAsc);
	}
	else
	{
		JS_serverList.sort(JS_serverList.sortedBy, JS_serverList.sortedAsc);
	}
}

