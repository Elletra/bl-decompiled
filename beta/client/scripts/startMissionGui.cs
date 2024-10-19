function startMissionGui::onWake(%this)
{
	SM_missionList.clear();
	%i = 0;
	for (%file = findFirstFile($Server::MissionFileSpec); %file !$= ""; %file = findNextFile($Server::MissionFileSpec))
	{
		if (strstr(%file, "CVS/") == -1)
		{
			SM_missionList.addRow(%i++, getMissionDisplayName(%file) @ "\t" @ getMissionPreviewImage(%file) @ "\t" @ %file);
		}
	}
	SM_missionList.sort(0);
	SM_missionList.setSelectedRow(0);
	SM_missionList.scrollVisible(0);
	TxtServerName.setValue($Pref::Server::Name);
	TxtServerInfo.setValue($Pref::Server::Info);
	TxtServerPassword.setValue($Pref::Server::Password);
	TxtServerAdminPassword.setValue($Pref::Server::AdminPassword);
	SliderNumPlayers.setValue($Pref::Server::MaxPlayers);
}

function startMissionGui::onSleep(%this)
{
}

function getMissionPreviewImage(%missionFile)
{
	%file = new FileObject();
	%MissionInfoCreate = "";
	if (%file.openForRead(%missionFile))
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
				%MissionInfoCreate = %MissionInfoCreate @ %line;
				break;
			}
			if (%inInfoBlock)
			{
				%MissionInfoCreate = %MissionInfoCreate @ %line @ " ";
			}
		}
		%file.close();
	}
	else
	{
		error("ERROR: getMissionPreviewImage() - File not found.");
	}
	%MissionInfoCreate = "%MissionInfoObject = " @ %MissionInfoCreate;
	echo(%MissionInfoCreate);
	eval(%MissionInfoCreate);
	%file.delete();
	%previewImage = %MissionInfoObject.previewImage;
	%MissionInfoObject.delete();
	if (%previewImage !$= "")
	{
		return %previewImage;
	}
	else
	{
		return "base/data/missions/default";
	}
}

function SM_StartMission()
{
	$Pref::Server::Name = TxtServerName.getValue();
	$Pref::Server::Info = TxtServerInfo.getValue();
	$Pref::Server::Password = TxtServerPassword.getValue();
	$Pref::Server::AdminPassword = TxtServerAdminPassword.getValue();
	$Pref::Server::MaxPlayers = SliderNumPlayers.getValue();
	$Client::Password = TxtServerPassword.getValue();
	%id = SM_missionList.getSelectedId();
	%mission = getField(SM_missionList.getRowTextById(%id), 2);
	if ($pref::HostMultiPlayer)
	{
		%serverType = "MultiPlayer";
	}
	else
	{
		%serverType = "SinglePlayer";
	}
	createServer(%serverType, %mission);
	%conn = new GameConnection(ServerConnection);
	RootGroup.add(ServerConnection);
	%conn.setConnectArgs($pref::Player::Name);
	%conn.setJoinPassword($Client::Password);
	error("-----------CONN = ", %conn);
	%conn.connectLocal();
}

function getMissionDisplayName(%missionFile)
{
	%file = new FileObject();
	%MissionInfoObject = "";
	if (%file.openForRead(%missionFile))
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
				%MissionInfoObject = %MissionInfoObject @ %line;
				break;
			}
			if (%inInfoBlock)
			{
				%MissionInfoObject = %MissionInfoObject @ %line @ " ";
			}
		}
		%file.close();
	}
	%MissionInfoObject = "%MissionInfoObject = " @ %MissionInfoObject;
	eval(%MissionInfoObject);
	%file.delete();
	%missionName = %MissionInfoObject.name;
	%MissionInfoObject.delete();
	if (%missionName !$= "")
	{
		return %missionName;
	}
	else
	{
		return fileBase(%missionFile);
	}
}

function getMissionDescription(%missionFile)
{
	%file = new FileObject();
	%MissionInfoObject = "";
	if (%file.openForRead(%missionFile))
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
				%MissionInfoObject = %MissionInfoObject @ %line;
				break;
			}
			if (%inInfoBlock)
			{
				%MissionInfoObject = %MissionInfoObject @ %line @ " ";
			}
		}
		%file.close();
	}
	%MissionInfoObject = "%MissionInfoObject = " @ %MissionInfoObject;
	eval(%MissionInfoObject);
	%file.delete();
	for (%i = 0; %MissionInfoObject.desc[%i] !$= ""; %i++)
	{
		if (%i == 0)
		{
			%text = %MissionInfoObject.desc[%i];
		}
		else
		{
			%text = %text @ "\n" @ %MissionInfoObject.desc[%i];
		}
	}
	%MissionInfoObject.delete();
	if (%text !$= "")
	{
		return %text;
	}
	else
	{
		return "...";
	}
}

function SM_missionList::select(%this)
{
	%selectedID = %this.getSelectedId();
	%row = %this.getRowTextById(%selectedID);
	%name = getField(%row, 0);
	%picture = getField(%row, 1);
	%fileName = getField(%row, 2);
	%description = %this.description[%selectedID];
	echo("filename = ", %fileName);
	SM_MapPreview.setBitmap(%picture);
	SM_MapName.setValue(%name);
	%description = getMissionDescription(%fileName);
	SM_MapDescription.setText(%description);
	SM_MapDescription.forceReflow();
	%picSize = getWord(SM_MapDescription.getGroup().getExtent(), 1);
	%w = getWord(SM_MapDescription.getExtent(), 0);
	%h = getWord(SM_MapDescription.getExtent(), 1);
	%x = getWord(SM_MapDescription.getPosition(), 0);
	%y = (%picSize - %h) - 4;
	SM_MapDescription.resize(%x, %y, %w, %h);
	return;
	if (strlen(%description) > 1)
	{
		SM_MapDescription.setValue(%description);
	}
	else
	{
		%description = getMissionDescription(%fileName);
		if (strlen(%description) > 1)
		{
			SM_MapDescription.setText(%description);
			%this.description[%selectedID] = %description;
		}
	}
}

