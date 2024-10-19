$GuiAudioType = 1;
$SimAudioType = 2;
$MessageAudioType = 3;
new AudioDescription(AudioGui)
{
	volume = 1;
	isLooping = 0;
	is3D = 0;
	type = $GuiAudioType;
};
new AudioDescription(AudioMessage)
{
	volume = 1;
	isLooping = 0;
	is3D = 0;
	type = $MessageAudioType;
};
new AudioProfile(AudioButtonOver)
{
	fileName = "~/data/sound/buttonOver.wav";
	description = "AudioGui";
	preload = 1;
};
new AudioProfile(AudioError)
{
	fileName = "~/data/sound/error.wav";
	description = "AudioGui";
	preload = 1;
};
new AudioProfile(ItemPickup)
{
	fileName = "~/data/sound/error.wav";
	description = "AudioGui";
	preload = 1;
};
new AudioProfile(AdminSound)
{
	fileName = "~/data/sound/admin.wav";
	description = "AudioGui";
	preload = 1;
};
new AudioProfile(BrickClearSound)
{
	fileName = "~/data/sound/brickClear.wav";
	description = "AudioGui";
	preload = 1;
};
new AudioProfile(ClientJoinSound)
{
	fileName = "~/data/sound/playerConnect.wav";
	description = "AudioGui";
	preload = 1;
};
new AudioProfile(ClientDropSound)
{
	fileName = "~/data/sound/playerLeave.wav";
	description = "AudioGui";
	preload = 1;
};
new AudioProfile(UploadStartSound)
{
	fileName = "~/data/sound/uploadStart.wav";
	description = "AudioGui";
	preload = 1;
};
new AudioProfile(UploadEndSound)
{
	fileName = "~/data/sound/uploadEnd.wav";
	description = "AudioGui";
	preload = 1;
};
new AudioProfile(ProcessCompleteSound)
{
	fileName = "~/data/sound/processComplete.wav";
	description = "AudioGui";
	preload = 1;
};
new AudioProfile(Note0Sound)
{
	fileName = "~/data/sound/notes/Synth 4/00.wav";
	description = "AudioGui";
	preload = 1;
};
new AudioProfile(Note1Sound : Note0Sound)
{
	fileName = "~/data/sound/notes/Synth 4/01.wav";
};
new AudioProfile(Note2Sound : Note0Sound)
{
	fileName = "~/data/sound/notes/Synth 4/02.wav";
};
new AudioProfile(Note3Sound : Note0Sound)
{
	fileName = "~/data/sound/notes/Synth 4/03.wav";
};
new AudioProfile(Note4Sound : Note0Sound)
{
	fileName = "~/data/sound/notes/Synth 4/04.wav";
};
new AudioProfile(Note5Sound : Note0Sound)
{
	fileName = "~/data/sound/notes/Synth 4/05.wav";
};
new AudioProfile(Note6Sound : Note0Sound)
{
	fileName = "~/data/sound/notes/Synth 4/06.wav";
};
new AudioProfile(Note7Sound : Note0Sound)
{
	fileName = "~/data/sound/notes/Synth 4/07.wav";
};
new AudioProfile(Note8Sound : Note0Sound)
{
	fileName = "~/data/sound/notes/Synth 4/08.wav";
};
new AudioProfile(Note9Sound : Note0Sound)
{
	fileName = "~/data/sound/notes/Synth 4/09.wav";
};
new AudioProfile(Note10Sound : Note0Sound)
{
	fileName = "~/data/sound/notes/Synth 4/10.wav";
};
new AudioProfile(Note11Sound : Note0Sound)
{
	fileName = "~/data/sound/notes/Synth 4/11.wav";
};
new AudioDescription(AudioBGMusic2D)
{
	volume = 0.8;
	isLooping = 1;
	is3D = 1;
	referenceDistance = 10;
	maxDistance = 60;
	type = $GuiAudioType;
};
new AudioProfile(TitleMusic)
{
	fileName = "~/data/sound/music/Ambient Deep.ogg";
	description = "AudioBGMusic2D";
	preload = 1;
};
new AudioDescription(AudioClientClose3d)
{
	volume = 1;
	isLooping = 0;
	is3D = 1;
	referenceDistance = 10;
	maxDistance = 60;
	type = $SimAudioType;
};
new AudioProfile(BrickBreak)
{
	fileName = "~/data/sound/breakBrick.wav";
	description = "AudioClientClose3d";
	preload = 1;
};
new AudioProfile(BrickMove)
{
	fileName = "~/data/sound/clickMove.wav";
	description = "AudioClientClose3d";
	preload = 1;
};
new AudioProfile(BrickPlant)
{
	fileName = "~/data/sound/clickPlant.wav";
	description = "AudioClientClose3d";
	preload = 1;
};
new AudioProfile(BrickRotate)
{
	fileName = "~/data/sound/clickRotate.wav";
	description = "AudioClientClose3d";
	preload = 1;
};
new AudioProfile(BrickChange)
{
	fileName = "~/data/sound/clickChange.wav";
	description = "AudioClientClose3d";
	preload = 1;
};
loadBrickSounds();
addMessageCallback('MsgClientJoin', handleClientJoin);
addMessageCallback('MsgClientDrop', handleClientDrop);
addMessageCallback('MsgClientScoreChanged', handleClientScoreChanged);
function handleClientJoin(%msgType, %msgString, %clientName, %clientId, %bl_id, %score, %__unused, %isAdmin, %isSuperAdmin, %trust, %inYourMiniGame)
{
	%name = StripMLControlChars(detag(%clientName));
	%bl_id = mFloor(%bl_id);
	%trust = mFloor(%trust);
	%inYourMiniGame = mFloor(%inYourMiniGame);
	if (NPL_List.getRowTextById(%clientId) $= "" && %msgString !$= "")
	{
		alxPlay(ClientJoinSound);
	}
	NewPlayerListGui.update(%clientId, %name, %bl_id, %isSuperAdmin, %isAdmin, %score, %trust, %inYourMiniGame);
	if (lstAdminPlayerList.getRowNumById(%clientId) == -1)
	{
		lstAdminPlayerList.addRow(%clientId, %name TAB %bl_id);
	}
	else
	{
		lstAdminPlayerList.setRowById(%clientId, %name TAB %bl_id);
	}
}

function handleClientDrop(%msgType, %msgString, %clientName, %clientId)
{
	alxPlay(ClientDropSound);
	lstAdminPlayerList.removeRowById(%clientId);
	NPL_List.removeRowById(%clientId);
}

function handleClientScoreChanged(%msgType, %msgString, %score, %clientId)
{
	NewPlayerListGui.updateScore(%clientId, %score);
}

addMessageCallback('MsgClientTrust', handleClientTrust);
function handleClientTrust(%msgType, %msgString, %clientId, %trustLevel)
{
	%trustLevel = mFloor(%trustLevel);
	%clientId = mFloor(%clientId);
	NewPlayerListGui.updateTrust(%clientId, %trustLevel);
}

addMessageCallback('MsgClientInYourMiniGame', handleClientInYourMiniGame);
function handleClientInYourMiniGame(%msgType, %msgString, %clientId, %val)
{
	%clientId = mFloor(%clientId);
	%val = mFloor(%val);
	NewPlayerListGui.updateInYourMiniGame(%clientId, %val);
}

addMessageCallback('InitTeams', handleInitTeams);
addMessageCallback('AddTeam', handleAddTeam);
addMessageCallback('RemoveTeam', handleRemoveTeam);
addMessageCallback('SetTeamName', handleSetTeamName);
function handleInitTeams(%msgType, %msgString)
{
	InitClientTeamManager();
}

function handleAddTeam(%msgType, %msgString, %teamID, %teamName)
{
	ClientTeamManager.addTeam(%teamID, %teamName);
}

function handleRemoveTeam(%msgType, %msgString, %teamID)
{
	ClientTeamManager.removeTeam(%teamID);
}

function handleSetTeamName(%msgType, %msgString, %teamID, %teamName)
{
	ClientTeamManager.setTeamName(%teamID, %teamName);
}

addMessageCallback('AddClientToTeam', handleAddClientToTeam);
addMessageCallback('RemoveClientFromTeam', handleRemoveClientFromTeam);
addMessageCallback('SetTeamCaptain', handleSetTeamCaptain);
function handleAddClientToTeam(%msgType, %msgString, %clientId, %clientName, %teamID)
{
	%teamObj = ClientTeamManager.findTeamByID(%teamID);
	if (%teamObj == 0)
	{
		error("ERROR: handleAddClientToTeam - Team ID " @ %teamID @ " not found in manager");
		return 0;
	}
	%teamObj.addMember(%clientId, %clientName);
}

function handleRemoveClientFromTeam(%msgType, %msgString, %clientId, %teamID)
{
	%teamObj = ClientTeamManager.findTeamByID(%teamID);
	if (%teamObj == 0)
	{
		error("ERROR: handleRemoveClientFromTeam - Team ID " @ %teamID @ " not found in manager");
		return 0;
	}
	%teamObj.removeMember(%clientId);
}

function handleSetTeamCaptain(%msgType, %msgString, %clientId, %teamID)
{
	%teamObj = ClientTeamManager.findTeamByID(%teamID);
	if (%teamObj == 0)
	{
		error("ERROR: handleSetTeamCaptain - Team ID " @ %teamID @ " not found in manager");
		return 0;
	}
	%teamObj.captain = %clientId;
}

function InitClientTeamManager()
{
	if (isObject(ClientTeamManager))
	{
		for (%i = 0; %i < ClientTeamManager.teamCount; %i++)
		{
			if (isObject(ClientTeamManager.team[%i]))
			{
				ClientTeamManager.team[%i].delete();
			}
		}
		ClientTeamManager.delete();
	}
	new ScriptObject(ClientTeamManager)
	{
		class = SO_ClientTeamManager;
		teamCount = 0;
	};
}

function SO_ClientTeamManager::addTeam(%this, %teamID, %teamName)
{
	if (%this.findTeamByID(%teamID) != 0)
	{
		error("ERROR: SO_ClientTeamManager::addTeam - Team ID " @ %teamID @ " is already in use");
		return 0;
	}
	%newTeam = new ScriptObject()
	{
		class = SO_ClientTeam;
		memberCount = 0;
		serverID = %teamID;
		name = %teamName;
	};
	%this.team[%this.teamCount] = %newTeam;
	%this.teamCount++;
}

function SO_ClientTeamManager::removeTeam(%this, %teamID)
{
	for (%i = 0; %i < %this.teamCount; %i++)
	{
		%currTeam = %this.team[%i];
		if (%currTeam.serverID == %teamID)
		{
			%currTeam.delete();
			for (%j = %i; %j < %this.teamCount - 1; %j++)
			{
				%this.team[%j] = %this.team[%j + 1];
			}
			%this.team[%this.teamCount] = "";
			%this.teamCount--;
			return 1;
		}
	}
	error("ERROR: SO_ClientTeamManager::removeTeam - Team ID " @ %teamID @ " not found in manager");
	return 0;
}

function SO_ClientTeamManager::setTeamName(%this, %teamID, %teamName)
{
	%teamObj = %this.findTeamByID(%teamID);
	if (%teamObj != 0)
	{
		%teamObj.name = %teamName;
	}
	else
	{
		error("ERROR: SO_ClientTeamManager::setTeamName - Team ID " @ %teamID @ " not found in manager");
	}
}

function SO_ClientTeamManager::findTeamByID(%this, %teamID)
{
	for (%i = 0; %i < %this.teamCount; %i++)
	{
		%currTeam = %this.team[%i];
		if (%currTeam.serverID == %teamID)
		{
			return %currTeam;
		}
	}
	return 0;
}

function SO_ClientTeamManager::dumpTeams(%this)
{
	echo("===============");
	echo("CLIENT Team Manager ID = ", %this);
	echo("Number of teams = ", %this.teamCount);
	for (%i = 0; %i < %this.teamCount; %i++)
	{
		%currTeam = %this.team[%i];
		echo("   Team " @ %i @ " = " @ %currTeam @ "(server:" @ %currTeam.serverID @ ") : " @ %currTeam.name @ " : " @ %currTeam.memberCount @ " members");
		for (%j = 0; %j < %currTeam.memberCount; %j++)
		{
			%client = %currTeam.memberID[%j];
			%clientName = StripMLControlChars(getTaggedString(%currTeam.memberName[%j]));
			if (%currTeam.captain == %client)
			{
				echo("      " @ %client @ " : " @ %clientName @ " <CAPT>");
			}
			else
			{
				echo("      " @ %client @ " : " @ %clientName);
			}
		}
	}
	echo("===============");
}

function SO_ClientTeam::addMember(%this, %clientId, %name)
{
	%this.memberID[%this.memberCount] = %clientId;
	%this.memberName[%this.memberCount] = %name;
	%this.memberCount++;
}

function SO_ClientTeam::removeMember(%this, %clientId)
{
	for (%i = 0; %i < %this.memberCount; %i++)
	{
		if (%this.memberID[%i] == %clientId)
		{
			for (%j = %i; %j < %this.memberCount - 1; %j++)
			{
				%this.memberID[%j] = %this.memberID[%j + 1];
				%this.memberName[%j] = %this.memberName[%j + 1];
			}
			%this.memberID[%this.memberCount] = "";
			%this.memberName[%this.memberCount] = "";
			%this.memberCount--;
			return 1;
		}
	}
	error("ERROR: SO_ClientTeam::removeMember - Client ID " @ %clientId @ " not found in team " @ %this @ "(server:" @ %this.serverID @ ")");
}

function SO_ClientTeam::setCaptain(%this, %clientId)
{
	%this.captain = %client;
}

function NewPlayerListGui::onWake(%this)
{
	if (NPL_List.getSelectedId() <= 0)
	{
		NPL_List.setSelectedRow(0);
	}
	NewPlayerListGui.clickList();
}

function NewPlayerListGui::onSleep(%this)
{
}

function NewPlayerListGui::toggle(%this)
{
	if (%this.isAwake())
	{
		Canvas.popDialog(%this);
	}
	else
	{
		Canvas.pushDialog(%this);
	}
}

function NewPlayerListGui::update(%this, %clientId, %clientName, %bl_id, %isSuperAdmin, %isAdmin, %score)
{
	%adminChar = "-";
	if (%isSuperAdmin)
	{
		%adminChar = "S";
	}
	else if (%isAdmin)
	{
		%adminChar = "A";
	}
	%row = NPL_List.getRowTextById(%clientId);
	%trust = getField(%row, 4);
	%inYourMiniGame = getField(%row, 5);
	%ignoring = mFloor(getField(%row, 6));
	%line = %adminChar TAB %clientName TAB %score TAB %bl_id TAB %trust TAB %inYourMiniGame TAB %ignoring;
	if (NPL_List.getRowNumById(%clientId) == -1)
	{
		NPL_List.addRow(%clientId, %line);
	}
	else
	{
		NPL_List.setRowById(%clientId, %line);
	}
}

function NewPlayerListGui::updateTrust(%this, %clientId, %trustLevel)
{
	if (NPL_List.getRowNumById(%clientId) == -1)
	{
		error("ERROR: NewPlayerListGui::UpdateTrust() - trust update recieved for non-existant client (" @ %clientId @ ")");
		return;
	}
	%row = NPL_List.getRowTextById(%clientId);
	%admin = getField(%row, 0);
	%name = getField(%row, 1);
	%score = getField(%row, 2);
	%bl_id = getField(%row, 3);
	%inYourMiniGame = getField(%row, 5);
	%ignoring = mFloor(getField(%row, 6));
	%trust = "-";
	if (%trustLevel == -1)
	{
		%trust = "You";
	}
	else if (%trustLevel == 0)
	{
		%trust = "-";
	}
	else if (%trustLevel == 1)
	{
		%trust = "Build";
	}
	else if (%trustLevel == 2)
	{
		%trust = "Full";
	}
	else if (%trustLevel == 10)
	{
		%trust = "LAN";
	}
	else
	{
		%trust = "-";
	}
	if (%trust $= "You" && (%admin $= "A" || %admin $= "S"))
	{
		$IamAdmin = 1;
	}
	%line = %admin TAB %name TAB %score TAB %bl_id TAB %trust TAB %inYourMiniGame TAB %ignoring;
	NPL_List.setRowById(%clientId, %line);
	NewPlayerListGui.clickList();
}

function NewPlayerListGui::updateInYourMiniGame(%this, %clientId, %val)
{
	if (NPL_List.getRowNumById(%clientId) == -1)
	{
		error("ERROR: NewPlayerListGui::UpdateInYourMiniGame() - InYourMiniGame update recieved for non-existant client (" @ %clientId @ ")");
		return;
	}
	%row = NPL_List.getRowTextById(%clientId);
	%admin = StripMLControlChars(getField(%row, 0));
	%name = getField(%row, 1);
	%score = getField(%row, 2);
	%bl_id = getField(%row, 3);
	%trust = getField(%row, 4);
	%ignoring = mFloor(getField(%row, 6));
	if (%val)
	{
		%row = "\c5" @ %admin TAB %name TAB %score TAB %bl_id TAB %trust TAB %val TAB %ignoring;
	}
	else
	{
		%row = %admin TAB %name TAB %score TAB %bl_id TAB %trust TAB %val TAB %ignoring;
	}
	NPL_List.setRowById(%clientId, %row);
	NewPlayerListGui.clickList();
}

function NewPlayerListGui::ClearInYourMiniGame(%this)
{
	for (%i = 0; %i < 1000; %i++)
	{
		%row = NPL_List.getRowText(%i);
		if (%row $= "")
		{
			NewPlayerListGui.clickList();
			return;
		}
		%id = NPL_List.getRowId(%i);
		%admin = StripMLControlChars(getField(%row, 0));
		%name = getField(%row, 1);
		%score = getField(%row, 2);
		%bl_id = getField(%row, 3);
		%trust = getField(%row, 4);
		%ignoring = mFloor(getField(%row, 6));
		%row = %admin TAB %name TAB %score TAB %bl_id TAB %trust TAB "0" TAB %ignoring;
		NPL_List.setRowById(%id, %row);
	}
}

function NewPlayerListGui::updateScore(%this, %clientId, %score)
{
	if (NPL_List.getRowNumById(%clientId) == -1)
	{
		error("ERROR: NewPlayerListGui::UpdateTrust() - score update recieved for non-existant client (" @ %clientId @ ")");
		return;
	}
	%row = NPL_List.getRowTextById(%clientId);
	%admin = getField(%row, 0);
	%name = getField(%row, 1);
	%bl_id = getField(%row, 3);
	%trust = getField(%row, 4);
	%inYourMiniGame = getField(%row, 5);
	%ignoring = mFloor(getField(%row, 6));
	%line = %admin TAB %name TAB %score TAB %bl_id TAB %trust TAB %inYourMiniGame TAB %ignoring;
	NPL_List.setRowById(%clientId, %line);
}

function NewPlayerListGui::clickList(%this)
{
	%id = NPL_List.getSelectedId();
	%row = NPL_List.getRowTextById(%id);
	NPL_Window.setText("Player List - " @ getField(%row, 1));
	if ($RemoteServer::LAN)
	{
		NPL_TrustInviteBuildBlocker.setVisible(1);
		NPL_TrustInviteFullBlocker.setVisible(1);
		NPL_TrustRemoveBuildBlocker.setVisible(1);
		NPL_TrustRemoveFullBlocker.setVisible(1);
	}
	else
	{
		%trust = getField(%row, 4);
		if (%trust $= "")
		{
			NPL_TrustInviteBuildBlocker.setVisible(0);
			NPL_TrustInviteFullBlocker.setVisible(0);
			NPL_TrustRemoveBuildBlocker.setVisible(1);
			NPL_TrustRemoveFullBlocker.setVisible(1);
		}
		else if (%trust $= "-")
		{
			NPL_TrustInviteBuildBlocker.setVisible(0);
			NPL_TrustInviteFullBlocker.setVisible(0);
			NPL_TrustRemoveBuildBlocker.setVisible(1);
			NPL_TrustRemoveFullBlocker.setVisible(1);
		}
		else if (%trust $= "Build")
		{
			NPL_TrustInviteBuildBlocker.setVisible(1);
			NPL_TrustInviteFullBlocker.setVisible(0);
			NPL_TrustRemoveBuildBlocker.setVisible(0);
			NPL_TrustRemoveFullBlocker.setVisible(1);
		}
		else if (%trust $= "Full")
		{
			NPL_TrustInviteBuildBlocker.setVisible(1);
			NPL_TrustInviteFullBlocker.setVisible(1);
			NPL_TrustRemoveBuildBlocker.setVisible(0);
			NPL_TrustRemoveFullBlocker.setVisible(0);
		}
		else if (%trust $= "You")
		{
			NPL_TrustInviteBuildBlocker.setVisible(1);
			NPL_TrustInviteFullBlocker.setVisible(1);
			NPL_TrustRemoveBuildBlocker.setVisible(1);
			NPL_TrustRemoveFullBlocker.setVisible(1);
		}
	}
	if ($RunningMiniGame)
	{
		%inYourMiniGame = getField(%row, 5);
		if (%inYourMiniGame == 1)
		{
			%trust = getField(%row, 4);
			if (%trust $= "You")
			{
				NPL_MiniGameInviteBlocker.setVisible(1);
				NPL_MiniGameRemoveBlocker.setVisible(1);
			}
			else
			{
				NPL_MiniGameInviteBlocker.setVisible(1);
				NPL_MiniGameRemoveBlocker.setVisible(0);
			}
		}
		else
		{
			NPL_MiniGameInviteBlocker.setVisible(0);
			NPL_MiniGameRemoveBlocker.setVisible(1);
		}
	}
	else
	{
		NPL_MiniGameInviteBlocker.setVisible(1);
		NPL_MiniGameRemoveBlocker.setVisible(1);
	}
	%ignoring = mFloor(getField(%row, 6));
	if (%ignoring)
	{
		NPL_UnIgnoreBlocker.setVisible(0);
	}
	else
	{
		NPL_UnIgnoreBlocker.setVisible(1);
	}
}

function NewPlayerListGui::sortList(%this, %col)
{
	NPL_List.sortedNumerical = 0;
	if (NPL_List.sortedBy == %col)
	{
		NPL_List.sortedAsc = !NPL_List.sortedAsc;
		NPL_List.sort(NPL_List.sortedBy, NPL_List.sortedAsc);
	}
	else
	{
		NPL_List.sortedBy = %col;
		NPL_List.sortedAsc = 0;
		NPL_List.sort(NPL_List.sortedBy, NPL_List.sortedAsc);
	}
}

function NewPlayerListGui::sortNumList(%this, %col)
{
	NPL_List.sortedNumerical = 1;
	if (NPL_List.sortedBy == %col)
	{
		NPL_List.sortedAsc = !NPL_List.sortedAsc;
		NPL_List.sortNumerical(NPL_List.sortedBy, NPL_List.sortedAsc);
	}
	else
	{
		NPL_List.sortedBy = %col;
		NPL_List.sortedAsc = 0;
		NPL_List.sortNumerical(NPL_List.sortedBy, NPL_List.sortedAsc);
	}
}

function NewPlayerListGui::ClickTrustInviteBuild(%this)
{
	%row = NPL_List.getRowTextById(NPL_List.getSelectedId());
	if (%row $= "")
	{
		return;
	}
	echo("row = ", %row);
	%bl_id = getField(%row, 3);
	%targetClient = NPL_List.getSelectedId();
	commandToServer('Trust_Invite', %targetClient, %bl_id, 1);
	rememberSentTrustInvite(%bl_id, 1);
	NewPlayerListGui.showTrustMessage();
}

function NewPlayerListGui::ClickTrustInviteFull(%this)
{
	%row = NPL_List.getRowTextById(NPL_List.getSelectedId());
	if (%row $= "")
	{
		return;
	}
	%bl_id = getField(%row, 3);
	%targetClient = NPL_List.getSelectedId();
	commandToServer('Trust_Invite', %targetClient, %bl_id, 2);
	rememberSentTrustInvite(%bl_id, 2);
	NewPlayerListGui.showTrustMessage();
}

function NewPlayerListGui::showTrustMessage(%this)
{
	if (isEventPending(%this.showTrustSchedule))
	{
		cancel(%this.showTrustSchedule);
	}
	NPL_TrustWindow.setVisible(1);
	%this.showTrustSchedule = NPL_TrustWindow.schedule(800, setVisible, 0);
}

function NewPlayerListGui::ClickTrustDemoteNONE(%this)
{
	%row = NPL_List.getRowTextById(NPL_List.getSelectedId());
	if (%row $= "")
	{
		return;
	}
	%bl_id = getField(%row, 3);
	commandToServer('Trust_Demote', %bl_id, 0);
	%name = getField(%row, 1);
	updateClientTrustList(%bl_id, 0, %name);
}

function NewPlayerListGui::ClickTrustDemoteBUILD(%this)
{
	%row = NPL_List.getRowTextById(NPL_List.getSelectedId());
	if (%row $= "")
	{
		return;
	}
	%bl_id = getField(%row, 3);
	commandToServer('Trust_Demote', %bl_id, 1);
	%name = getField(%row, 1);
	updateClientTrustList(%bl_id, 1, %name);
}

function NewPlayerListGui::ClickMiniGameInvite(%this)
{
	%victimID = NPL_List.getSelectedId();
	if (!%victimID)
	{
		return;
	}
	commandToServer('InviteToMiniGame', %victimID);
}

function NewPlayerListGui::ClickMiniGameRemove(%this)
{
	%victimID = NPL_List.getSelectedId();
	if (!%victimID)
	{
		return;
	}
	commandToServer('RemoveFromMiniGame', %victimID);
}

function NewPlayerListGui::ClickUnIgnore(%this)
{
	%victimID = NPL_List.getSelectedId();
	if (!%victimID)
	{
		return;
	}
	commandToServer('UnIgnore', %victimID);
	NewPlayerListGui.setIgnoring(%victimID, 0);
}

function NewPlayerListGui::setIgnoring(%this, %clientId, %val)
{
	%row = NPL_List.getRowTextById(%clientId);
	if (%row $= "")
	{
		return;
	}
	%bl_id = getField(%row, 3);
	NewPlayerListGui.setIgnoringBL_ID(%bl_id, %val);
}

function NewPlayerListGui::setIgnoringBL_ID(%this, %targetBL_ID, %val)
{
	%i = 0;
	for (%row = NPL_List.getRowText(%i); %row !$= ""; %row = NPL_List.getRowText(%i++))
	{
		%rowID = NPL_List.getRowId(%i);
		%admin = getField(%row, 0);
		%name = getField(%row, 1);
		%score = getField(%row, 2);
		%bl_id = getField(%row, 3);
		%trust = getField(%row, 4);
		%inYourMiniGame = getField(%row, 5);
		%ignoring = %val;
		if (%bl_id == %targetBL_ID)
		{
			%line = %admin TAB %name TAB %score TAB %bl_id TAB %trust TAB %inYourMiniGame TAB %ignoring;
			NPL_List.setRowById(%rowID, %line);
		}
	}
	NewPlayerListGui.clickList();
}

function clientCmdTrustInvite(%name, %bl_id, %level)
{
	%bl_id = mFloor(%bl_id);
	%level = mFloor(%level);
	TI_Name.setText(%name);
	TI_BL_ID.setText(%bl_id);
	TI_BuildMessageA.setVisible(0);
	TI_BuildMessageB.setVisible(0);
	TI_FullMessageA.setVisible(0);
	TI_FullMessageB.setVisible(0);
	if (%level == 1)
	{
		TI_BuildMessageA.setVisible(1);
		TI_BuildMessageB.setVisible(1);
	}
	else
	{
		TI_FullMessageA.setVisible(1);
		TI_FullMessageB.setVisible(1);
	}
	Canvas.pushDialog(TrustInviteGui);
}

function TrustInviteGui::ClickAccept(%this)
{
	%targetBL_ID = mFloor(TI_BL_ID.getText());
	if (TI_BuildMessageA.isVisible())
	{
		%level = 1;
	}
	else
	{
		%level = 2;
	}
	%name = TI_Name.getText();
	updateClientTrustList(%targetBL_ID, %level, %name);
	commandToServer('AcceptTrustInvite', %targetBL_ID);
	Canvas.popDialog(TrustInviteGui);
}

function TrustInviteGui::ClickReject(%this)
{
	%targetBL_ID = mFloor(TI_BL_ID.getText());
	commandToServer('RejectTrustInvite', %targetBL_ID);
	Canvas.popDialog(TrustInviteGui);
}

function TrustInviteGui::ClickIgnore(%this)
{
	MessageBoxYesNo("Ignore User?", "Ignore all future trust invites from this person?", "TrustInviteGui.ignore();");
}

function TrustInviteGui::Ignore(%this)
{
	%targetBL_ID = mFloor(TI_BL_ID.getText());
	commandToServer('IgnoreTrustInvite', %targetBL_ID);
	Canvas.popDialog(TrustInviteGui);
	NewPlayerListGui.setIgnoringBL_ID(%targetBL_ID, 1);
}

function rememberSentTrustInvite(%bl_id, %level)
{
	$TrustInvite[%bl_id] = %level;
}

function clientCmdTrustInviteAccepted(%clientId, %bl_id, %level)
{
	if ($TrustInvite[%bl_id] == %level)
	{
		%row = NPL_List.getRowTextById(%clientId);
		%name = getField(%row, 1);
		if (%name $= "")
		{
			error("ERROR: clientCmdTrustInviteAccepted() - No name for BL_ID:" @ %bl_id);
		}
		updateClientTrustList(%bl_id, %level, %name);
	}
	else
	{
		error("ERROR: clientCmdTrustInviteAccepted() - Server says we invited BL_ID:" @ %bl_id @ " for level:" @ %level @ " but we didn't!");
	}
}

function clientCmdTrustDemoted(%clientId, %bl_id, %level)
{
	echo(" clientCmdTrustDemoted ", %clientId, " | ", %bl_id, " | ", %level);
	%row = NPL_List.getRowTextById(%clientId);
	%name = getField(%row, 1);
	if (%name $= "")
	{
		echo(" clientCmdTrustDemoted - no name, bailing");
		return;
	}
	updateClientTrustList(%bl_id, %level, %name);
}

function updateClientTrustList(%bl_id, %level, %name)
{
	%bl_id = mFloor(%bl_id);
	%level = mFloor(%level);
	for (%i = 0; %i < $Trust::Count; %i++)
	{
		%checkBL_ID = getWord($Trust::Line[%i], 0);
		if (%checkBL_ID == %bl_id)
		{
			$Trust::Line[%i] = %bl_id TAB %level TAB %name;
			saveTrustList();
			return;
		}
	}
	%i = mFloor($Trust::Count);
	$Trust::Line[%i] = %bl_id TAB %level TAB %name;
	$Trust::Count++;
	saveTrustList();
}

function saveTrustList()
{
	export("$Trust::*", "base/config/client/prefs-trustList.cs");
}

function loadTrustList()
{
	if (isFile("base/config/client/prefs-trustList.cs"))
	{
		exec("base/config/client/prefs-trustList.cs");
	}
}

function clientCmdTrustListUpload_Start()
{
	for (%i = 0; %i < $Trust::Count; %i++)
	{
		%bl_id = getField($Trust::Line[%i], 0);
		%level = getField($Trust::Line[%i], 1);
		if (%level > 0)
		{
			%line = %bl_id SPC %level;
			commandToServer('TrustListUpload_Line', %line);
		}
	}
	commandToServer('TrustListUpload_Done');
}

function MiniGameInviteGui::onWake(%this)
{
}

function MiniGameInviteGui::onSleep(%this)
{
}

function clientCmdMiniGameInvite(%title, %name, %bl_id, %miniGameID)
{
	MGI_Title.setText(%title);
	MGI_Name.setText(%name);
	MGI_BL_ID.setText(%bl_id);
	MiniGameInviteGui.miniGameID = %miniGameID;
	Canvas.pushDialog(MiniGameInviteGui);
}

function MiniGameInviteGui::ClickAccept(%this)
{
	commandToServer('AcceptMiniGameInvite', MiniGameInviteGui.miniGameID);
	Canvas.popDialog(MiniGameInviteGui);
}

function MiniGameInviteGui::ClickReject(%this)
{
	commandToServer('RejectMiniGameInvite', MiniGameInviteGui.miniGameID);
	Canvas.popDialog(MiniGameInviteGui);
}

function MiniGameInviteGui::ClickIgnore(%this)
{
	MessageBoxYesNo("Ignore User?", "Are you sure you want to ignore mini-game invites from this user?", "MiniGameInviteGui.ignore();");
}

function MiniGameInviteGui::Ignore(%this)
{
	commandToServer('IgnoreMiniGameInvite', MiniGameInviteGui.miniGameID);
	Canvas.popDialog(MiniGameInviteGui);
	%targetBL_ID = mFloor(MGI_BL_ID.getText());
	NewPlayerListGui.setIgnoringBL_ID(%targetBL_ID, 1);
}

function joinMiniGameGui::onWake(%this)
{
	if ($PlayingMiniGame)
	{
		JMG_JoinBlocker.setVisible(1);
		JMG_LeaveBlocker.setVisible(0);
		if ($RunningMiniGame)
		{
			CMG_EndBlocker.setVisible(0);
		}
	}
	else
	{
		JMG_JoinBlocker.setVisible(0);
		JMG_LeaveBlocker.setVisible(1);
		CMG_EndBlocker.setVisible(1);
	}
	joinMiniGameGui.clickList();
}

function joinMiniGameGui::onSleep(%this)
{
}

function clientCmdAddMiniGameLine(%line, %id, %colorIdx)
{
	%colorIdx = mFloor(%colorIdx);
	%id = mFloor(%id);
	%CC0 = "\c0";
	%CC1 = "\c1";
	%CC2 = "\c2";
	%CC3 = "\c3";
	%CC4 = "\c4";
	%CC5 = "\c5";
	%CC6 = "\c6";
	%CC7 = "\c7";
	%CC8 = "\c8";
	%CC9 = "\c9";
	%line = %CC[%colorIdx] @ %line;
	if (JMG_List.getRowNumById(%id) == -1)
	{
		JMG_List.addRow(%id, %line);
	}
	else
	{
		JMG_List.setRowById(%id, %line);
	}
}

function clientCmdRemoveMiniGameLine(%id)
{
	%id = mFloor(%id);
	JMG_List.removeRowById(%id);
}

function clientCmdResetMiniGameList()
{
	JMG_List.clear();
}

function joinMiniGameGui::clickList(%this)
{
	%row = JMG_List.getRowTextById(JMG_List.getSelectedId());
	if (%row $= "")
	{
		JMG_JoinBlocker.setVisible(1);
		return;
	}
	%inviteOnly = getField(%row, 3);
	if (%inviteOnly)
	{
		JMG_JoinBlocker.setVisible(1);
	}
	else
	{
		JMG_JoinBlocker.setVisible(0);
	}
}

function joinMiniGameGui::ClickJoin(%this)
{
	%mgID = JMG_List.getSelectedId();
	if (%mgID <= 0)
	{
		return;
	}
	commandToServer('JoinMiniGame', %mgID);
}

function joinMiniGameGui::ClickLeave(%this)
{
	if (!$PlayingMiniGame)
	{
		error("ERROR: JoinMiniGameGui::ClickLeave() - You shouldn't have been able to click this unless you're playing a minigame");
		return;
	}
	if ($RunningMiniGame)
	{
		MessageBoxYesNo("End Mini-Game?", "Are you sure you want to end the current mini-game?", "joinMiniGameGui.end();");
	}
	else
	{
		commandToServer('LeaveMiniGame');
	}
}

function joinMiniGameGui::end(%this)
{
	commandToServer('EndMiniGame');
}

function joinMiniGameGui::ClickCreate(%this)
{
	Canvas.pushDialog(CreateMiniGameGui);
	Canvas.popDialog(%this);
}

function clientCmdSetPlayingMiniGame(%val)
{
	$PlayingMiniGame = %val;
	if ($PlayingMiniGame)
	{
		JMG_JoinBlocker.setVisible(1);
		JMG_LeaveBlocker.setVisible(0);
	}
	else
	{
		NewPlayerListGui.ClearInYourMiniGame();
		JMG_LeaveBlocker.setVisible(1);
		$BuildingDisabled = 0;
		$PaintingDisabled = 0;
	}
	joinMiniGameGui.clickList();
}

function clientCmdSetRunningMiniGame(%val)
{
	$RunningMiniGame = %val;
	if ($RunningMiniGame)
	{
		CMG_EndBlocker.setVisible(0);
	}
	else
	{
		CMG_EndBlocker.setVisible(1);
	}
}

function clientCmdSetBuildingDisabled(%val)
{
	$BuildingDisabled = %val;
	if ($BuildingDisabled)
	{
		if ($ScrollMode == $SCROLLMODE_BRICKS)
		{
			setScrollMode($SCROLLMODE_NONE);
		}
	}
}

function clientCmdSetPaintingDisabled(%val)
{
	$PaintingDisabled = %val;
	if ($PaintingDisabled)
	{
		if ($ScrollMode == $SCROLLMODE_PAINT)
		{
			setScrollMode($SCROLLMODE_NONE);
		}
	}
}

function joinMiniGameGui::sortList(%this, %col)
{
	JMG_List.sortedNumerical = 0;
	if (JMG_List.sortedBy == %col)
	{
		JMG_List.sortedAsc = !JMG_List.sortedAsc;
		JMG_List.sort(JMG_List.sortedBy, JMG_List.sortedAsc);
	}
	else
	{
		JMG_List.sortedBy = %col;
		JMG_List.sortedAsc = 0;
		JMG_List.sort(JMG_List.sortedBy, JMG_List.sortedAsc);
	}
}

function joinMiniGameGui::sortNumList(%this, %col)
{
	JMG_List.sortedNumerical = 1;
	if (JMG_List.sortedBy == %col)
	{
		JMG_List.sortedAsc = !JMG_List.sortedAsc;
		JMG_List.sortNumerical(JMG_List.sortedBy, JMG_List.sortedAsc);
	}
	else
	{
		JMG_List.sortedBy = %col;
		JMG_List.sortedAsc = 0;
		JMG_List.sortNumerical(JMG_List.sortedBy, JMG_List.sortedAsc);
	}
}

function CreateMiniGameGui::onWake(%this)
{
	CMG_FavsHelper.setVisible(0);
	CMG_ColorList.clear();
	commandToServer('RequestMiniGameColorList');
	if ($RunningMiniGame)
	{
		CMG_Window.setText("Edit Mini-Game");
		CMG_CreateButton.setText("Update >>");
		CMG_ColorBlocker.setVisible(1);
	}
	else
	{
		CMG_Window.setText("Create Mini-Game");
		CMG_CreateButton.setText("Create >>");
		CMG_ColorBlocker.setVisible(0);
	}
	if (!$cmg_hasLoaded)
	{
		$cmg_hasLoaded = 1;
		$MiniGame::BrickDamage = 1;
		$MiniGame::BrickRespawnTime = 10;
		$MiniGame::ColorName = "Red";
		$MiniGame::EnableBuilding = 1;
		$MiniGame::EnablePainting = 1;
		$MiniGame::EnableWand = 0;
		$MiniGame::FallingDamage = 1;
		$MiniGame::InviteOnly = 0;
		$MiniGame::PlayerDataBlock = "Standard Player";
		$MiniGame::PlayersUseOwnBricks = 0;
		$MiniGame::Points::BreakBrick = 0;
		$MiniGame::Points::Die = 0;
		$MiniGame::Points::KillPlayer = 1;
		$MiniGame::Points::KillSelf = -1;
		$MiniGame::Points::PlantBrick = 0;
		$MiniGame::RespawnTime = 1;
		$MiniGame::SelfDamage = 1;
		$MiniGame::StartEquip0 = "Hammer ";
		$MiniGame::StartEquip1 = "Wrench";
		$MiniGame::StartEquip2 = "Printer";
		$MiniGame::StartEquip3 = "Gun";
		$MiniGame::StartEquip4 = "Rocket L.";
		$MiniGame::Title = "Default Mini-Game";
		$MiniGame::UseAllPlayersBricks = 0;
		$MiniGame::UseSpawnBricks = 1;
		$MiniGame::VehicleDamage = 1;
		$MiniGame::VehicleRespawnTime = 5;
		$MiniGame::WeaponDamage = 1;
		CreateMiniGameGui.Refresh();
		for (%i = 0; %i < 5; %i++)
		{
			%obj = "CMG_StartEquip" @ %i;
			%obj.setSelected(%obj.findText($MiniGame::StartEquip[%i]));
		}
	}
}

function CreateMiniGameGui::LoadDataBlocks()
{
	CMG_PlayerDataBlock.clear();
	for (%i = 0; %i < 5; %i++)
	{
		%obj = "CMG_StartEquip" @ %i;
		%obj.clear();
		%obj.add(" NONE", 0);
		%obj.setSelected(0);
	}
	%dbCount = getDataBlockGroupSize();
	for (%j = 0; %j < %dbCount; %j++)
	{
		%db = getDataBlock(%j);
		%dbClass = %db.getClassName();
		if (%dbClass $= "ItemData")
		{
			if (%db.uiName !$= "")
			{
				for (%i = 0; %i < 5; %i++)
				{
					%obj = "CMG_StartEquip" @ %i;
					%obj.add(%db.uiName, %db);
				}
			}
		}
		else if (%dbClass $= "PlayerData")
		{
			if (%db.uiName !$= "")
			{
				%obj = "CMG_PlayerDataBlock";
				%obj.add(%db.uiName, %db);
			}
		}
	}
	$MiniGame::InviteOnly = mFloor($MiniGame::InviteOnly);
	$MiniGame::UseAllPlayersBricks = mFloor($MiniGame::UseAllPlayersBricks);
	$MiniGame::PlayersUseOwnBricks = mFloor($MiniGame::PlayersUseOwnBricks);
	$MiniGame::UseSpawnBricks = mFloor($MiniGame::UseSpawnBricks);
	$MiniGame::Points::BreakBrick = mFloor($MiniGame::Points::BreakBrick);
	$MiniGame::Points::PlantBrick = mFloor($MiniGame::Points::PlantBrick);
	$MiniGame::Points::KillPlayer = mFloor($MiniGame::Points::KillPlayer);
	$MiniGame::Points::KillSelf = mFloor($MiniGame::Points::KillSelf);
	$MiniGame::Points::Die = mFloor($MiniGame::Points::Die);
	$MiniGame::RespawnTime = mFloor($MiniGame::RespawnTime);
	$MiniGame::VehicleRespawnTime = mFloor($MiniGame::VehicleRespawnTime);
	$MiniGame::BrickRespawnTime = mFloor($MiniGame::BrickRespawnTime);
	$MiniGame::JetLevel = mFloor($MiniGame::JetLevel);
	$MiniGame::FallingDamage = mFloor($MiniGame::FallingDamage);
	$MiniGame::WeaponDamage = mFloor($MiniGame::WeaponDamage);
	$MiniGame::VehicleDamage = mFloor($MiniGame::VehicleDamage);
	$MiniGame::BrickDamage = mFloor($MiniGame::BrickDamage);
	$MiniGame::SelfDamage = mFloor($MiniGame::SelfDamage);
	$MiniGame::EnableWand = mFloor($MiniGame::EnableWand);
	$MiniGame::EnableBuilding = mFloor($MiniGame::EnableBuilding);
	if ($MiniGame::PlayerDataBlock $= "")
	{
		$MiniGame::PlayerDataBlock = "Standard Player";
	}
	%obj = "CMG_PlayerDataBlock";
	%obj.setSelected(%obj.findText($MiniGame::PlayerDataBlock));
	for (%i = 0; %i < 5; %i++)
	{
		if ($MiniGame::StartEquip[%i] $= "")
		{
			$MiniGame::StartEquip[%i] = " NONE";
		}
		%obj = "CMG_StartEquip" @ %i;
		%obj.setSelected(%obj.findText($MiniGame::StartEquip[%i]));
	}
	if ($MiniGame::RespawnTime < 1)
	{
		$MiniGame::RespawnTime = 1;
	}
	if ($MiniGame::Title $= "")
	{
		$MiniGame::Title = "Default Mini-Game";
	}
}

function CreateMiniGameGui::onSleep(%this)
{
}

function ClientCmdAddMiniGameColor(%idx, %name, %RGB)
{
	CMG_ColorList.add(%name, %idx);
	CMG_ColorList.colorF[%idx] = colorItoColorF(%RGB);
	if (CMG_ColorList.getText() $= "")
	{
		CMG_ColorList.setSelected(%idx);
	}
	CreateMiniGameGui.clickColorList();
}

function colorItoColorF(%intColor)
{
	%r = getWord(%intColor, 0);
	%g = getWord(%intColor, 1);
	%b = getWord(%intColor, 2);
	%r = %r / 255;
	%g = %g / 255;
	%b = %b / 255;
	return %r SPC %g SPC %b;
}

function CreateMiniGameGui::clickColorList()
{
	%idx = CMG_ColorList.getSelected();
	CMG_Swatch.setColor(CMG_ColorList.colorF[%idx] SPC "255");
}

function CreateMiniGameGui::ClickCreate(%this)
{
	if ($RunningMiniGame)
	{
		CreateMiniGameGui.send();
		Canvas.popDialog(CreateMiniGameGui);
	}
	else
	{
		%colorIdx = CMG_ColorList.getSelected();
		commandToServer('createMiniGame', $MiniGame::Title, %colorIdx, $MiniGame::UseSpawnBricks);
	}
}

function CreateMiniGameGui::clickReset(%this)
{
	if ($RunningMiniGame)
	{
		CreateMiniGameGui.send();
		commandToServer('ResetMiniGame');
		Canvas.popDialog(CreateMiniGameGui);
	}
}

function CreateMiniGameGui::clickEnd(%this)
{
	if ($RunningMiniGame)
	{
		MessageBoxYesNo("End Mini-Game?", "Are you sure you want to end the mini-game?", "CreateMiniGameGui.end();");
	}
}

function CreateMiniGameGui::end(%this)
{
	commandToServer('EndMiniGame');
	Canvas.popDialog(CreateMiniGameGui);
}

function clientCmdCreateMiniGameSuccess()
{
	CreateMiniGameGui.send();
	Canvas.popDialog(CreateMiniGameGui);
}

function clientCmdCreateMiniGameFail(%reason)
{
	MessageBoxOK("Mini-Game Creation Failure", "Mini-Game Creation Failed.  Reason:\n\n" @ %reason);
}

function CreateMiniGameGui::clickSetFavs(%this)
{
	CMG_FavsHelper.setVisible(!CMG_FavsHelper.isVisible());
}

function CreateMiniGameGui::ClickFav(%this, %idx)
{
	%idx = mFloor(%idx);
	%fileName = "base/config/client/MiniGameFavorites/" @ %idx @ ".cs";
	if (CMG_FavsHelper.isVisible())
	{
		$MiniGame::ColorName = CMG_ColorList.getText();
		%obj = "CMG_PlayerDataBlock";
		$MiniGame::PlayerDataBlock = %obj.getText();
		for (%i = 0; %i < 5; %i++)
		{
			%obj = "CMG_StartEquip" @ %i;
			$MiniGame::StartEquip[%i] = %obj.getText();
		}
		export("$MiniGame::*", %fileName, 0);
		CMG_FavsHelper.setVisible(0);
	}
	else
	{
		if (!isFile(%fileName))
		{
			return;
		}
		exec(%fileName);
		%idx = CMG_ColorList.findText($MiniGame::ColorName);
		CMG_ColorList.setSelected(%idx);
		CMG_Swatch.setColor(CMG_ColorList.colorF[%idx] SPC "255");
		for (%i = 0; %i < 5; %i++)
		{
			%obj = "CMG_StartEquip" @ %i;
			%obj.setSelected(%obj.findText($MiniGame::StartEquip[%i]));
		}
		if ($MiniGame::PlayerDataBlock $= "")
		{
			$MiniGame::PlayerDataBlock = "Standard Player";
		}
		%obj = "CMG_PlayerDataBlock";
		%obj.setSelected(%obj.findText($MiniGame::PlayerDataBlock));
		CreateMiniGameGui.Refresh();
	}
}

function CreateMiniGameGui::Refresh(%this)
{
	%count = CMG_Window.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		%obj = CMG_Window.getObject(%i);
		%var = %obj.variable;
		if (%var !$= "")
		{
			%var = strreplace(%var, ";", "");
			eval(%obj @ ".setValue(" @ %var @ ");");
		}
	}
}

function CreateMiniGameGui::send(%this)
{
	%line = "";
	%line = %line TAB "T" SPC $MiniGame::Title;
	%line = %line TAB "IO" SPC $MiniGame::InviteOnly;
	%line = %line TAB "UAPB" SPC $MiniGame::UseAllPlayersBricks;
	%line = %line TAB "PUOB" SPC $MiniGame::PlayersUseOwnBricks;
	%line = %line TAB "USB" SPC $MiniGame::UseSpawnBricks;
	%line = %line TAB "PBB" SPC $MiniGame::Points::BreakBrick;
	%line = %line TAB "PPB" SPC $MiniGame::Points::PlantBrick;
	%line = %line TAB "PKP" SPC $MiniGame::Points::KillPlayer;
	%line = %line TAB "PKS" SPC $MiniGame::Points::KillSelf;
	%line = %line TAB "PD" SPC $MiniGame::Points::Die;
	%line = %line TAB "RT" SPC $MiniGame::RespawnTime;
	%line = %line TAB "VRT" SPC $MiniGame::VehicleRespawnTime;
	%line = %line TAB "BRT" SPC $MiniGame::BrickRespawnTime;
	%line = %line TAB "FD" SPC $MiniGame::FallingDamage;
	%line = %line TAB "WD" SPC $MiniGame::WeaponDamage;
	%line = %line TAB "SD" SPC $MiniGame::SelfDamage;
	%line = %line TAB "VD" SPC $MiniGame::VehicleDamage;
	%line = %line TAB "BD" SPC $MiniGame::BrickDamage;
	%line = %line TAB "EW" SPC $MiniGame::EnableWand;
	%line = %line TAB "EB" SPC $MiniGame::EnableBuilding;
	%line = %line TAB "EP" SPC $MiniGame::EnablePainting;
	%line = %line TAB "DB" SPC CMG_PlayerDataBlock.getSelected();
	for (%i = 0; %i < 5; %i++)
	{
		%listObj = "CMG_StartEquip" @ %i;
		%listObj = %listObj.getId();
		%line = %line TAB "SE" SPC %i SPC %listObj.getSelected();
	}
	%line = trim(%line);
	if (strlen(%line) > 200)
	{
		%tempLine = "";
		%fieldCount = getFieldCount(%line);
		for (%i = 0; %i < %fieldCount; %i++)
		{
			%field = getField(%line, %i);
			if (strlen(%tempLine TAB %field) > 200)
			{
				%tempLine = trim(%tempLine);
				commandToServer('SetMiniGameData', %tempLine);
				%tempLine = "";
			}
			%tempLine = %tempLine TAB %field;
		}
		%tempLine = trim(%tempLine);
		commandToServer('SetMiniGameData', %tempLine);
	}
	else
	{
		commandToServer('SetMiniGameData', %line);
	}
}

function ConnectingGui::onWake()
{
}

function onSendConnectChallengeRequest()
{
	Connecting_Text.setText(Connecting_Text.getText() @ "\nSending Challenge Request...");
}

function ConnectingGui::cancel()
{
	if (isObject($conn))
	{
		$conn.cancelConnect();
		$conn.delete();
	}
	Canvas.popDialog(ConnectingGui);
}

function OnSubnetError()
{
	Connecting_Text.setText(Connecting_Text.getText() @ "\nSubnet error: This does not appear to be a LAN game.");
	if (isObject($conn))
	{
		$conn.cancelConnect();
		$conn.delete();
	}
}

function onInvalidConnectionAddress()
{
	Connecting_Text.setText(Connecting_Text.getText() @ "\nInvalid Address");
	if (isObject($conn))
	{
		$conn.cancelConnect();
		$conn.delete();
	}
}

function SAD(%password)
{
	if (%password !$= "")
	{
		commandToServer('SAD', %password);
	}
}

function SADSetPassword(%password)
{
	commandToServer('SADSetPassword', %password);
}

function buildwall()
{
	%count = 0;
	for (%j = 0; %j < 16; %j++)
	{
		for (%i = 0; %i < 10; %i++)
		{
			for (%k = 0; %k < 10; %k++)
			{
				commandToServer('plantBrick');
				commandToServer('shiftBrick', 0, 0, 3);
			}
			commandToServer('shiftBrick', 2, 0, -30);
		}
		commandToServer('shiftBrick', -20, 2, 0);
	}
}

function buildConfetti()
{
	%count = 0;
	%paint = 0;
	for (%j = 0; %j < 16; %j++)
	{
		for (%i = 0; %i < 10; %i++)
		{
			for (%k = 0; %k < 30; %k++)
			{
				commandToServer('useSprayCan', %paint);
				%paint++;
				if (%paint > 4)
				{
					%paint = 0;
				}
				commandToServer('plantBrick');
				commandToServer('shiftBrick', 0, 0, 1);
			}
			commandToServer('shiftBrick', 1, 0, -30);
		}
		commandToServer('shiftBrick', -10, 1, 0);
	}
}

function buildFloor()
{
	%count = 0;
	for (%j = 0; %j < 16; %j++)
	{
		for (%i = 0; %i < 10; %i++)
		{
			commandToServer('plantBrick');
			commandToServer('shiftBrick', 2, 0, 0);
		}
		commandToServer('shiftBrick', -20, 2, 0);
	}
}

function buildFloor2()
{
	%count = 0;
	for (%j = 0; %j < 32; %j++)
	{
		for (%i = 0; %i < 32; %i++)
		{
			commandToServer('plantBrick');
			commandToServer('shiftBrick', 1, 0, 0);
		}
		commandToServer('shiftBrick', -32, 1, 0);
	}
}

function two()
{
	for (%i = 0; %i < 30; %i++)
	{
		commandToServer('plantBrick');
		commandToServer('shiftBrick', 0, 0, 3);
	}
}

function maze()
{
	for (%j = 0; %j < 10; %j++)
	{
		for (%i = 0; %i < 10; %i++)
		{
			commandToServer('plantBrick');
			commandToServer('shiftbrick', 7, 0, 0);
		}
		commandToServer('shiftbrick', -70, 7, 0);
	}
}

function maze2()
{
	for (%j = 0; %j < 10; %j++)
	{
		for (%i = 0; %i < 10; %i++)
		{
			commandToServer('plantBrick');
			commandToServer('shiftbrick', 8, 0, 0);
		}
		commandToServer('shiftbrick', -80, 8, 0);
	}
}

function stress()
{
	for (%j = 0; %j < 16; %j++)
	{
		for (%i = 0; %i < 10; %i++)
		{
			for (%k = 0; %k < 30; %k++)
			{
				commandToServer('plantBrick');
				commandToServer('shiftBrick', 0, 0, 3);
			}
			commandToServer('shiftBrick', 2, 0, %k * -3);
		}
		commandToServer('shiftBrick', -20, 2, 0);
	}
}

function buildstairs()
{
	for (%i = 0; %i < 30; %i++)
	{
		commandToServer('plantBrick');
		commandToServer('shiftBrick', 1, 0, 3);
	}
}

function clientCmdSetRemoteServerData(%serverLAN)
{
	$RemoteServer::LAN = %serverLAN;
}

function clientCmdSyncClock(%time)
{
}

function GameConnection::prepDemoRecord(%this)
{
}

function GameConnection::prepDemoPlayback(%this)
{
	Canvas.setContent(PlayGui);
}

function getWords(%phrase, %start, %end)
{
	if (%start > %end)
	{
		return;
	}
	%returnPhrase = getWord(%phrase, %start);
	if (%start == %end)
	{
		return %returnPhrase;
	}
	for (%i = %start + 1; %i <= %end; %i++)
	{
		%returnPhrase = %returnPhrase @ " " @ getWord(%phrase, %i);
	}
	return %returnPhrase;
}

function getLine(%phrase, %lineNum)
{
	%offset = 0;
	%lineCount = 0;
	while (%lineCount <= %lineNum)
	{
		%pos = strpos(%phrase, "\n", %offset);
		if (%pos >= 0)
		{
			%len = %pos - %offset;
		}
		else
		{
			%len = 99999;
		}
		%line = getSubStr(%phrase, %offset, %len);
		if (%lineCount == %lineNum)
		{
			return %line;
		}
		%lineCount++;
		%offset = %pos + 1;
		if (%pos == -1)
		{
			return "";
		}
	}
	return "";
}

function getLineCount(%phrase)
{
	%offset = 0;
	for (%lineCount = 0; %offset >= 0; %lineCount++)
	{
		%offset = strpos(%phrase, "\n", %offset + 1);
	}
	return %lineCount;
}

function onMissionDownloadPhase1(%missionName, %__unused)
{
	LoadingProgress.setValue(0);
	LoadingProgressTxt.setValue("LOADING DATABLOCKS");
}

function onPhase1Progress(%progress)
{
	LoadingProgress.setValue(%progress);
	Canvas.repaint();
}

function onPhase1Complete()
{
}

function onMissionDownloadPhase2()
{
	LoadingProgress.setValue(0);
	LoadingProgressTxt.setValue("LOADING OBJECTS");
	Canvas.repaint();
}

function onPhase2Progress(%progress)
{
	LoadingProgress.setValue(%progress);
	Canvas.repaint();
}

function onPhase2Complete()
{
}

function onFileChunkReceived(%fileName, %ofs, %size)
{
	LoadingProgress.setValue(%ofs / %size);
	LoadingProgressTxt.setValue("Downloading " @ %fileName @ "...");
}

function onMissionDownloadPhase3()
{
	LoadingProgress.setValue(0);
	LoadingProgressTxt.setValue("LIGHTING MISSION (This only happens once)");
	Canvas.repaint();
}

function onPhase3Progress(%progress)
{
	LoadingProgress.setValue(%progress);
}

function onPhase3Complete()
{
	LoadingProgress.setValue(1);
	$lightingMission = 0;
}

function onMissionDownloadComplete()
{
	clientCmdBSD_LoadBricks();
	clientCmdWrench_LoadMenus();
	CreateMiniGameGui.LoadDataBlocks();
	$RunningMiniGame = 0;
	$PlayingMiniGame = 0;
	$BuildingDisabled = 0;
	$PaintingDisabled = 0;
	NewPlayerListGui.ClearInYourMiniGame();
	$Camera::movementSpeed = 40;
	if (isObject(ServerGroup))
	{
		if ($Pref::Net::ServerType $= "SinglePlayer")
		{
			$timeScale = 1;
		}
	}
}

addMessageCallback('MsgLoadInfo', handleLoadInfoMessage);
addMessageCallback('MsgLoadDescripition', handleLoadDescriptionMessage);
addMessageCallback('MsgLoadMapPicture', handleLoadMapPictureMessage);
addMessageCallback('MsgLoadInfoDone', handleLoadInfoDoneMessage);
function handleLoadInfoMessage(%msgType, %msgString, %mapName, %mapSaveName)
{
	Canvas.setContent("LoadingGui");
	for (%line = 0; %line < LoadingGui.qLineCount; %line++)
	{
		LoadingGui.qLine[%line] = "";
	}
	LoadingGui.qLineCount = 0;
	LOAD_MapName.setText(%mapName);
	$MapSaveName = %mapSaveName;
}

function handleLoadDescriptionMessage(%msgType, %msgString, %line)
{
	%text = LOAD_MapDescription.getText();
	if (%text !$= "")
	{
		LOAD_MapDescription.setText(%text @ "\n" @ %line);
	}
	else
	{
		LOAD_MapDescription.setText(%line);
	}
	return;
	LoadingGui.qLine[LoadingGui.qLineCount] = %line;
	LoadingGui.qLineCount++;
	%text = "<spush><font:Arial:16>";
	for (%line = 0; %line < LoadingGui.qLineCount - 1; %line++)
	{
		%text = %text @ LoadingGui.qLine[%line] @ " ";
	}
	%text = %text @ LoadingGui.qLine[%line] @ "<spop>";
	LOAD_MapDescription.setText(%text);
}

function handleLoadMapPictureMessage(%msgType, %msgString, %imageName)
{
	LOAD_MapPicture.setBitmap(%imageName);
}

function handleLoadInfoDoneMessage(%msgType, %msgString)
{
}

addMessageCallback('MsgConnectionError', handleConnectionErrorMessage);
function handleConnectionErrorMessage(%msgType, %msgString, %msgError)
{
	$ServerConnectionErrorMessage = %msgError;
}

function GameConnection::initialControlSet(%this)
{
	echo("*** Initial Control Object");
	if (isObject(EditorGui))
	{
		if (!Editor::checkActiveLoadDone())
		{
			if (Canvas.getContent() != PlayGui.getId())
			{
				%trustInvite = TrustInviteGui.isAwake();
				Canvas.setContent(PlayGui);
				if (%trustInvite)
				{
					Canvas.pushDialog(TrustInviteGui);
				}
			}
		}
	}
	else if (Canvas.getContent() != PlayGui.getId())
	{
		%trustInvite = TrustInviteGui.isAwake();
		Canvas.setContent(PlayGui);
		if (%trustInvite)
		{
			Canvas.pushDialog(TrustInviteGui);
		}
	}
}

function GameConnection::setLagIcon(%this, %state)
{
	if (%this.getAddress() $= "local")
	{
		return;
	}
	LagIcon.setVisible(%state $= "true");
}

function GameConnection::onConnectionAccepted(%this)
{
	LagIcon.setVisible(0);
	$IamAdmin = 0;
	$PlayingMiniGame = 0;
	$RunningMiniGame = 0;
	InitClientTeamManager();
}

function GameConnection::onConnectionTimedOut(%this)
{
	disconnectedCleanup();
	MessageBoxOK("TIMED OUT", "The server connection has timed out.");
}

function GameConnection::onConnectionDropped(%this, %msg)
{
	disconnectedCleanup();
	MessageBoxOK("DISCONNECT", "The server has dropped the connection: " @ %msg);
	$timeScale = 1;
}

function GameConnection::onConnectionError(%this, %msg)
{
	disconnectedCleanup();
	MessageBoxOK("DISCONNECT", $ServerConnectionErrorMessage @ " (" @ %msg @ ")");
	$timeScale = 1;
}

function GameConnection::onConnectRequestRejected(%this, %msg)
{
	if (%msg $= "CR_INVALID_PROTOCOL_VERSION")
	{
		%error = "Incompatible protocol version: Your game version is not compatible with this server.";
	}
	else if (%msg $= "CR_INVALID_CONNECT_PACKET")
	{
		%error = "Internal Error: badly formed network packet";
	}
	else if (%msg $= "CR_YOUAREBANNED")
	{
		%error = "You are not allowed to play on this server.";
	}
	else if (%msg $= "CR_SERVERFULL")
	{
		%error = "This server is full.";
	}
	else if (%msg $= "CHR_PASSWORD")
	{
		%error = "Wrong password.";
	}
	else if (%msg $= "CHR_PROTOCOL")
	{
		%error = "Incompatible protocol version: Your game version is not compatible with this server.";
	}
	else if (%msg $= "CHR_CLASSCRC")
	{
		%error = "Incompatible game classes: Your game version is not compatible with this server.";
	}
	else if (%msg $= "CHR_INVALID_CHALLENGE_PACKET")
	{
		%error = "Internal Error: Invalid server response packet";
	}
	else
	{
		%error = "Connection error.  Please try another server.  Error code: (" @ %msg @ ")";
	}
	Canvas.popDialog(ConnectingGui);
	MessageBoxOK("REJECTED", %error);
}

function GameConnection::onConnectRequestTimedOut(%this)
{
	Connecting_Text.setText(Connecting_Text.getText() @ "\nRequest timed out.");
}

function disconnect()
{
	if (isObject(ServerConnection))
	{
		ServerConnection.delete();
	}
	disconnectedCleanup();
	destroyServer();
}

function disconnectedCleanup()
{
	alxStopAll();
	if (isObject(MusicPlayer))
	{
		MusicPlayer.stop();
	}
	LagIcon.setVisible(0);
	NPL_List.clear();
	lstAdminPlayerList.clear();
	NPL_List.clear();
	JMG_List.clear();
	clientCmdclearBottomPrint();
	clientCmdClearCenterPrint();
	Canvas.setContent(MainMenuGui);
	clearTextureHolds();
	purgeResources();
	PSD_KillPrints();
	WhoTalk_Kill();
	if (isEventPending($LoadingBricks_HandShakeSchedule))
	{
		cancel($LoadingBricks_HandShakeSchedule);
	}
	if (isEventPending($UploadSaveFile_Tick_Schedule))
	{
		cancel($UploadSaveFile_Tick_Schedule);
	}
	$timeScale = 1;
}

function LoadingGui::onAdd(%this)
{
	%this.qLineCount = 0;
}

function LoadingGui::onWake(%this)
{
	CloseMessagePopup();
	moveMap.push();
	Canvas.pushDialog(NewChatHud);
}

function LoadingGui::onSleep(%this)
{
	if (%this.qLineCount !$= "")
	{
		for (%line = 0; %line < %this.qLineCount; %line++)
		{
			%this.qLine[%line] = "";
		}
	}
	%this.qLineCount = 0;
	LOAD_MapName.setText("");
	LOAD_MapDescription.setText("");
	LoadingProgress.setValue(0);
	LoadingProgressTxt.setValue("WAITING FOR SERVER");
	Canvas.popDialog(NewChatHud);
}

function optionsDlg::setPane(%this, %pane)
{
	OptAudioPane.setVisible(0);
	OptGraphicsPane.setVisible(0);
	OptNetworkPane.setVisible(0);
	OptControlsPane.setVisible(0);
	OptAdvGraphicsPane.setVisible(0);
	("Opt" @ %pane @ "Pane").setVisible(1);
	OptRemapList.fillList();
}

function loadAvatarPrefs()
{
	loadAvatarPref(decalBox, $pref::Player::DecalColor);
	loadAvatarPref(faceBox, $pref::Player::FaceColor);
	loadAvatarPref(hatBox, $pref::Player::Hat);
	loadAvatarPref(accentBox, $pref::Player::Accent);
	loadAvatarPref(packBox, $pref::Player::Pack);
	loadAvatarPref(hatColorBox, $pref::Player::HatColor);
	loadAvatarPref(accentColorBox, $pref::Player::AccentColor);
	loadAvatarPref(packColorBox, $pref::Player::PackColor);
	loadAvatarPref(LArmColorBox, $pref::Player::LArmColor);
	loadAvatarPref(RArmColorBox, $pref::Player::RArmColor);
	loadAvatarPref(LHandColorBox, $pref::Player::LHandColor);
	loadAvatarPref(RHandColorBox, $pref::Player::RHandColor);
	loadAvatarPref(LLegColorBox, $pref::Player::LLegColor);
	loadAvatarPref(RLegColorBox, $pref::Player::RLegColor);
	loadAvatarPref(TorsoColorBox, $pref::Player::TorsoColor);
	loadAvatarPref(HipColorBox, $pref::Player::HipColor);
}

function loadAvatarPref(%obj, %var)
{
	for (%i = 0; %i < %obj.itemCount; %i++)
	{
		%obj.radio[%i].setValue(0);
	}
	%obj.radio[%var].setValue(1);
}

function loadAvatarColorMenus()
{
	loadAvatarColorMenu("base/data/shapes/player/LArm.ifl", LArmColorBox, "$pref::Player::LArmColor", 16, ColorRadioProfile);
	loadAvatarColorMenu("base/data/shapes/player/RArm.ifl", RArmColorBox, "$pref::Player::RArmColor", 16, ColorRadioProfile);
	loadAvatarColorMenu("base/data/shapes/player/LHand.ifl", LHandColorBox, "$pref::Player::LHandColor", 16, ColorRadioProfile);
	loadAvatarColorMenu("base/data/shapes/player/RHand.ifl", RHandColorBox, "$pref::Player::RHandColor", 16, ColorRadioProfile);
	loadAvatarColorMenu("base/data/shapes/player/LLeg.ifl", LLegColorBox, "$pref::Player::LLegColor", 16, ColorRadioProfile);
	loadAvatarColorMenu("base/data/shapes/player/RLeg.ifl", RLegColorBox, "$pref::Player::RLegColor", 16, ColorRadioProfile);
	loadAvatarColorMenu("base/data/shapes/player/Chest.ifl", TorsoColorBox, "$pref::Player::TorsoColor", 16, ColorRadioProfile);
	loadAvatarColorMenu("base/data/shapes/player/pelvis.ifl", HipColorBox, "$pref::Player::HipColor", 16, ColorRadioProfile);
	loadAvatarColorMenu("base/data/shapes/player/hat.ifl", hatColorBox, "$pref::Player::HatColor", 16, ColorRadioProfile);
	loadAvatarColorMenu("base/data/shapes/player/accent.ifl", accentColorBox, "$pref::Player::AccentColor", 16, ColorRadioProfile);
	loadAvatarColorMenu("base/data/shapes/player/pack.ifl", packColorBox, "$pref::Player::PackColor", 16, ColorRadioProfile);
	loadAvatarColorMenu("base/data/shapes/player/decal.ifl", decalBox, "$pref::Player::DecalColor", 64, decalRadioProfile);
	loadAvatarColorMenu("base/data/shapes/player/face.ifl", faceBox, "$pref::Player::FaceColor", 64, decalRadioProfile);
	loadAvatarShapeMenu("packs", packBox, "$pref::Player::pack", 64, decalRadioProfile);
	loadAvatarShapeMenu("hats", hatBox, "$pref::Player::hat", 64, decalRadioProfile);
	loadAvatarShapeMenu("accents", accentBox, "$pref::Player::accent", 64, decalRadioProfile);
	loadDecalNames();
	%decalBoxWidth = getWord(decalBox.extent, 0);
	%faceBoxWidth = getWord(faceBox.extent, 0);
	if (%decalBoxWidth > %faceBoxWidth)
	{
		%x = 0;
		%y = 0;
		%w = %decalBoxWidth;
		%h = 64;
		faceBox.resize(%x, %y, %w, %h);
	}
	loadAvatarAccentColors();
}

function loadAvatarAccentColors()
{
	loadAvatarAccentColor("plume");
}

function loadAvatarAccentColor(%name)
{
	%file = new FileObject();
	%file.openForRead("base/data/shapes/player/decal.ifl");
	if (%file.isEOF())
	{
		return;
	}
	for (%lineCount = 0; !%file.isEOF(); %lineCount++)
	{
		%line = %file.readLine();
	}
	%file.delete();
}

function loadDecalNames()
{
	%file = new FileObject();
	%file.openForRead("base/data/shapes/player/decal.ifl");
	if (%file.isEOF())
	{
		return;
	}
	for (%lineCount = 0; !%file.isEOF(); %lineCount++)
	{
		%line = %file.readLine();
		$decals[%lineCount] = %line;
	}
	%file.delete();
}

function loadAvatarShapeMenu(%fileName, %objName, %varName, %size, %radioProfile)
{
	%file = new FileObject();
	%file.openForRead("base/data/shapes/player/" @ %fileName @ ".txt");
	if (%file.isEOF())
	{
		return;
	}
	for (%lineCount = 0; !%file.isEOF(); %lineCount++)
	{
		%line = %file.readLine();
		%newColor = new GuiBitmapCtrl();
		%objName.image[%lineCount] = %newColor;
		%objName.add(%newColor);
		%newColor.setBitmap("base/client/ui/" @ %fileName @ "/" @ %line);
		%x = (%size + 1) * %lineCount;
		%y = 0;
		%w = %size;
		%h = %size;
		%newColor.resize(%x, %y, %w, %h);
		%x = getWord(%objName.getPosition(), 0);
		%y = getWord(%objName.getPosition(), 1);
		%w = (%size + 1) * (%lineCount + 1);
		%h = %size;
		%objName.resize(%x, %y, %w, %h);
		%newRadio = new GuiRadioCtrl();
		%objName.radio[%lineCount] = %newRadio;
		%objName.add(%newRadio);
		%newRadio.setProfile(%radioProfile);
		%x = (%size + 1) * %lineCount;
		%y = 0;
		%w = %size;
		%h = %size;
		%newRadio.resize(%x, %y, %w, %h);
		%newRadio.text = "";
		%commandString = %varName @ " = \"" @ %lineCount @ "\"; PPUpdate();";
		%newRadio.command = %commandString;
		%command = "$" @ %fileName @ "[" @ %lineCount @ "] = " @ %line @ ";";
		eval(%command);
		%objName.itemCount++;
	}
	%file.delete();
}

function loadAvatarColorMenu(%fileName, %objName, %varName, %size, %radioProfile)
{
	%file = new FileObject();
	%file.openForRead(%fileName);
	if (%file.isEOF())
	{
		return;
	}
	for (%lineCount = 0; !%file.isEOF(); %lineCount++)
	{
		%line = %file.readLine();
		%newColor = new GuiBitmapCtrl();
		%objName.image[%lineCount] = %newColor;
		%objName.add(%newColor);
		if (fileBase(%fileName) $= "decal")
		{
			%newColor.setBitmap("base/data/shapes/player/decalIcons/" @ %line);
		}
		else if (fileBase(%fileName) $= "face")
		{
			%newColor.setBitmap("base/data/shapes/player/smileyIcons/" @ %line);
		}
		else
		{
			%newColor.setBitmap("base/data/shapes/player/" @ %line);
		}
		%x = (%size + 1) * %lineCount;
		%y = 0;
		%w = %size;
		%h = %size;
		%newColor.resize(%x, %y, %w, %h);
		%x = getWord(%objName.getPosition(), 0);
		%y = getWord(%objName.getPosition(), 1);
		%w = (%size + 1) * (%lineCount + 1);
		%h = %size;
		%objName.resize(%x, %y, %w, %h);
		%newRadio = new GuiRadioCtrl();
		%objName.radio[%lineCount] = %newRadio;
		%objName.add(%newRadio);
		%newRadio.setProfile(%radioProfile);
		%x = (%size + 1) * %lineCount;
		%y = 0;
		%w = %size;
		%h = %size;
		%newRadio.resize(%x, %y, %w, %h);
		%newRadio.text = "";
		%commandString = %varName @ " = \"" @ %lineCount @ "\"; PPUpdate();";
		%newRadio.command = %commandString;
		%objName.itemCount++;
	}
	%file.delete();
}

function killSelectionImages()
{
	killSelectionBox(decalBox);
	killSelectionBox(faceBox);
	killSelectionBox(hatBox);
	killSelectionBox(accentBox);
	killSelectionBox(packBox);
	killSelectionBox(hatColorBox);
	killSelectionBox(accentColorBox);
	killSelectionBox(packColorBox);
	killSelectionBox(LHandColorBox);
	killSelectionBox(RHandColorBox);
	killSelectionBox(LArmColorBox);
	killSelectionBox(RArmColorBox);
	killSelectionBox(LLegColorBox);
	killSelectionBox(RLegColorBox);
	killSelectionBox(TorsoColorBox);
	killSelectionBox(HipColorBox);
	killSelectionBox(hatColorBox);
	killSelectionBox(accentColorBox);
	killSelectionBox(packColorBox);
}

function killSelectionBox(%obj)
{
	if (!isObject(%obj))
	{
		return;
	}
	for (%i = 0; %i < %obj.itemCount; %i++)
	{
		if (isObject(%obj.image[%i]))
		{
			%obj.image[%i].delete();
		}
		if (isObject(%obj.radio[%i]))
		{
			%obj.radio[%i].delete();
		}
		%obj.image[%i] = "";
		%obj.radio[%i] = "";
	}
	for (%obj.itemCount = ""; %obj.getCount() > 0; %obj.getObject(0).delete())
	{
	}
}

function PPHideAllNodes()
{
	playerPreview.hideNode("", lski);
	playerPreview.hideNode("", rski);
	playerPreview.hideNode("", armor);
	playerPreview.hideNode("", cape);
	playerPreview.hideNode("", pack);
	playerPreview.hideNode("", tank);
	playerPreview.hideNode("", bucket);
	playerPreview.hideNode("", quiver);
	playerPreview.hideNode("", helmet);
	playerPreview.hideNode("", scouthat);
	playerPreview.hideNode("", pointyhelmet);
	playerPreview.hideNode("", plume);
	playerPreview.hideNode("", visor);
}

function PPUpdate()
{
	PPHideAllNodes();
	$hatColorBlocked["scoutHat", "0"] = 1;
	$hatColorBlocked["scoutHat", "1"] = 1;
	$hatColorBlocked["scoutHat", "2"] = 0;
	$hatColorBlocked["scoutHat", "3"] = 1;
	$hatColorBlocked["scoutHat", "4"] = 1;
	$hatColorBlocked["scoutHat", "5"] = 1;
	$hatColorBlocked["scoutHat", "6"] = 1;
	$hatColorBlocked["scoutHat", "7"] = 1;
	$hatColorBlocked["scoutHat", "8"] = 0;
	$hatColorBlocked["pointyHelmet", "0"] = 1;
	$hatColorBlocked["pointyHelmet", "1"] = 1;
	$hatColorBlocked["pointyHelmet", "2"] = 1;
	$hatColorBlocked["pointyHelmet", "3"] = 1;
	$hatColorBlocked["pointyHelmet", "4"] = 1;
	$hatColorBlocked["pointyHelmet", "5"] = 1;
	$hatColorBlocked["pointyHelmet", "6"] = 0;
	$hatColorBlocked["pointyHelmet", "7"] = 0;
	$hatColorBlocked["pointyHelmet", "8"] = 1;
	$packColorBlocked["Armor", "0"] = 1;
	$packColorBlocked["Armor", "1"] = 1;
	$packColorBlocked["Armor", "2"] = 1;
	$packColorBlocked["Armor", "3"] = 1;
	$packColorBlocked["Armor", "4"] = 1;
	$packColorBlocked["Armor", "5"] = 1;
	$packColorBlocked["Armor", "6"] = 0;
	$packColorBlocked["Armor", "7"] = 0;
	$packColorBlocked["Armor", "8"] = 1;
	$packColorBlocked["Bucket", "0"] = 1;
	$packColorBlocked["Bucket", "1"] = 1;
	$packColorBlocked["Bucket", "2"] = 1;
	$packColorBlocked["Bucket", "3"] = 1;
	$packColorBlocked["Bucket", "4"] = 1;
	$packColorBlocked["Bucket", "5"] = 1;
	$packColorBlocked["Bucket", "6"] = 1;
	$packColorBlocked["Bucket", "7"] = 0;
	$packColorBlocked["Bucket", "8"] = 0;
	$packColorBlocked["Pack", "0"] = 1;
	$packColorBlocked["Pack", "1"] = 1;
	$packColorBlocked["Pack", "2"] = 1;
	$packColorBlocked["Pack", "3"] = 1;
	$packColorBlocked["Pack", "4"] = 1;
	$packColorBlocked["Pack", "5"] = 1;
	$packColorBlocked["Pack", "6"] = 1;
	$packColorBlocked["Pack", "7"] = 1;
	$packColorBlocked["Pack", "8"] = 0;
	$packColorBlocked["Quiver", "0"] = 1;
	$packColorBlocked["Quiver", "1"] = 1;
	$packColorBlocked["Quiver", "2"] = 1;
	$packColorBlocked["Quiver", "3"] = 1;
	$packColorBlocked["Quiver", "4"] = 1;
	$packColorBlocked["Quiver", "5"] = 1;
	$packColorBlocked["Quiver", "6"] = 1;
	$packColorBlocked["Quiver", "7"] = 1;
	$packColorBlocked["Quiver", "8"] = 0;
	$packColorBlocked["Tank", "0"] = 0;
	$packColorBlocked["Tank", "1"] = 0;
	$packColorBlocked["Tank", "2"] = 1;
	$packColorBlocked["Tank", "3"] = 0;
	$packColorBlocked["Tank", "4"] = 0;
	$packColorBlocked["Tank", "5"] = 0;
	$packColorBlocked["Tank", "6"] = 1;
	$packColorBlocked["Tank", "7"] = 0;
	$packColorBlocked["Tank", "8"] = 1;
	$torsoColorBlocked["decalBTron.png", "0"] = 1;
	$torsoColorBlocked["decalBTron.png", "1"] = 1;
	$torsoColorBlocked["decalBTron.png", "2"] = 1;
	$torsoColorBlocked["decalBTron.png", "3"] = 1;
	$torsoColorBlocked["decalBTron.png", "4"] = 0;
	$torsoColorBlocked["decalBTron.png", "5"] = 1;
	$torsoColorBlocked["decalBTron.png", "6"] = 1;
	$torsoColorBlocked["decalBTron.png", "7"] = 1;
	$torsoColorBlocked["decalBTron.png", "8"] = 1;
	$torsoColorBlocked["decalDarth.png", "0"] = 1;
	$torsoColorBlocked["decalDarth.png", "1"] = 1;
	$torsoColorBlocked["decalDarth.png", "2"] = 1;
	$torsoColorBlocked["decalDarth.png", "3"] = 1;
	$torsoColorBlocked["decalDarth.png", "4"] = 1;
	$torsoColorBlocked["decalDarth.png", "5"] = 1;
	$torsoColorBlocked["decalDarth.png", "6"] = 1;
	$torsoColorBlocked["decalDarth.png", "7"] = 0;
	$torsoColorBlocked["decalDarth.png", "8"] = 1;
	$torsoColorBlocked["decalRedCoat.png", "0"] = 1;
	$torsoColorBlocked["decalRedCoat.png", "1"] = 1;
	$torsoColorBlocked["decalRedCoat.png", "2"] = 1;
	$torsoColorBlocked["decalRedCoat.png", "3"] = 1;
	$torsoColorBlocked["decalRedCoat.png", "4"] = 0;
	$torsoColorBlocked["decalRedCoat.png", "5"] = 1;
	$torsoColorBlocked["decalRedCoat.png", "6"] = 1;
	$torsoColorBlocked["decalRedCoat.png", "7"] = 1;
	$torsoColorBlocked["decalRedCoat.png", "8"] = 1;
	$torsoColorBlocked["decalBlueCoat.png", "0"] = 1;
	$torsoColorBlocked["decalBlueCoat.png", "1"] = 1;
	$torsoColorBlocked["decalBlueCoat.png", "2"] = 1;
	$torsoColorBlocked["decalBlueCoat.png", "3"] = 1;
	$torsoColorBlocked["decalBlueCoat.png", "4"] = 0;
	$torsoColorBlocked["decalBlueCoat.png", "5"] = 1;
	$torsoColorBlocked["decalBlueCoat.png", "6"] = 1;
	$torsoColorBlocked["decalBlueCoat.png", "7"] = 1;
	$torsoColorBlocked["decalBlueCoat.png", "8"] = 1;
	$torsoColorBlocked["decalMtron.png", "0"] = 0;
	$torsoColorBlocked["decalMtron.png", "1"] = 1;
	$torsoColorBlocked["decalMtron.png", "2"] = 1;
	$torsoColorBlocked["decalMtron.png", "3"] = 1;
	$torsoColorBlocked["decalMtron.png", "4"] = 1;
	$torsoColorBlocked["decalMtron.png", "5"] = 1;
	$torsoColorBlocked["decalMtron.png", "6"] = 1;
	$torsoColorBlocked["decalMtron.png", "7"] = 1;
	$torsoColorBlocked["decalMtron.png", "8"] = 1;
	$torsoColorBlocked["decalNewSpace.png", "0"] = 1;
	$torsoColorBlocked["decalNewSpace.png", "1"] = 1;
	$torsoColorBlocked["decalNewSpace.png", "2"] = 1;
	$torsoColorBlocked["decalNewSpace.png", "3"] = 1;
	$torsoColorBlocked["decalNewSpace.png", "4"] = 0;
	$torsoColorBlocked["decalNewSpace.png", "5"] = 1;
	$torsoColorBlocked["decalNewSpace.png", "6"] = 1;
	$torsoColorBlocked["decalNewSpace.png", "7"] = 1;
	$torsoColorBlocked["decalNewSpace.png", "8"] = 1;
	$torsoColorBlocked["decalOldSpace.png", "0"] = 0;
	$torsoColorBlocked["decalOldSpace.png", "1"] = 0;
	$torsoColorBlocked["decalOldSpace.png", "2"] = 1;
	$torsoColorBlocked["decalOldSpace.png", "3"] = 0;
	$torsoColorBlocked["decalOldSpace.png", "4"] = 0;
	$torsoColorBlocked["decalOldSpace.png", "5"] = 1;
	$torsoColorBlocked["decalOldSpace.png", "6"] = 1;
	$torsoColorBlocked["decalOldSpace.png", "7"] = 0;
	$torsoColorBlocked["decalOldSpace.png", "8"] = 1;
	$accentBlocked["none", "none"] = 0;
	$accentBlocked["none", "plume"] = 1;
	$accentBlocked["none", "visor"] = 1;
	$accentBlocked["helmet", "none"] = 0;
	$accentBlocked["helmet", "plume"] = 1;
	$accentBlocked["helmet", "visor"] = 0;
	$accentBlocked["pointyHelmet", "none"] = 0;
	$accentBlocked["pointyHelmet", "plume"] = 1;
	$accentBlocked["pointyHelmet", "visor"] = 1;
	$accentBlocked["scoutHat", "none"] = 0;
	$accentBlocked["scoutHat", "plume"] = 0;
	$accentBlocked["scoutHat", "visor"] = 1;
	%hatName = $hats[$pref::Player::Hat];
	for (%i = 0; %i < hatColorBox.itemCount; %i++)
	{
		if (hatColorBox.radio[%i].getValue() == 1)
		{
			$pref::Player::HatColor = %i;
			break;
		}
	}
	for (%i = 0; %i < 9; %i++)
	{
		if ($hatColorBlocked[%hatName, %i] == 1)
		{
			hatColorBox.radio[%i].setActive(0);
		}
		else
		{
			hatColorBox.radio[%i].setActive(1);
		}
	}
	if ($hatColorBlocked[%hatName, $pref::Player::HatColor] == 1)
	{
		for (%i = 0; %i < 9; %i++)
		{
			if ($hatColorBlocked[%hatName, %i] == 0)
			{
				$pref::Player::HatColor = %i;
				break;
			}
		}
	}
	%packName = $packs[$pref::Player::Pack];
	for (%i = 0; %i < packColorBox.itemCount; %i++)
	{
		if (packColorBox.radio[%i].getValue() == 1)
		{
			$pref::Player::PackColor = %i;
			break;
		}
	}
	for (%i = 0; %i < 9; %i++)
	{
		if ($packColorBlocked[%packName, %i] == 1)
		{
			packColorBox.radio[%i].setActive(0);
		}
		else
		{
			packColorBox.radio[%i].setActive(1);
		}
	}
	if ($packColorBlocked[%packName, $pref::Player::PackColor] == 1)
	{
		for (%i = 0; %i < 9; %i++)
		{
			if ($packColorBlocked[%packName, %i] == 0)
			{
				$pref::Player::PackColor = %i;
				break;
			}
		}
	}
	%decalName = $decals[$pref::Player::DecalColor];
	for (%i = 0; %i < TorsoColorBox.itemCount; %i++)
	{
		if (TorsoColorBox.radio[%i].getValue() == 1)
		{
			$pref::Player::TorsoColor = %i;
			break;
		}
	}
	for (%i = 0; %i < TorsoColorBox.itemCount; %i++)
	{
		if ($torsoColorBlocked[%decalName, %i] == 1)
		{
			TorsoColorBox.radio[%i].setActive(0);
		}
		else
		{
			TorsoColorBox.radio[%i].setActive(1);
		}
	}
	if ($torsoColorBlocked[%decalName, $pref::Player::TorsoColor] == 1)
	{
		for (%i = 0; %i < TorsoColorBox.itemCount; %i++)
		{
			if ($torsoColorBlocked[%decalName, %i] == 0)
			{
				$pref::Player::TorsoColor = %i;
				break;
			}
		}
	}
	for (%i = 0; %i < accentBox.itemCount; %i++)
	{
		if (accentBox.radio[%i].getValue() == 1)
		{
			$pref::Player::Accent = %i;
			break;
		}
	}
	%hatName = $hats[$pref::Player::Hat];
	%accentName = $accents[$pref::Player::Accent];
	for (%i = 0; %i < accentBox.itemCount; %i++)
	{
		if ($accentBlocked[%hatName, $accents[%i]] == 1)
		{
			accentBox.radio[%i].setActive(0);
		}
		else
		{
			accentBox.radio[%i].setActive(1);
		}
	}
	if ($accentBlocked[%hatName, %accentName] == 1)
	{
		for (%i = 0; %i < accentBox.itemCount; %i++)
		{
			if ($accentBlocked[%hatName, $accents[%i]] == 0)
			{
				$pref::Player::Accent = %i;
				break;
			}
		}
	}
	for (%i = 0; %i < accentColorBox.itemCount; %i++)
	{
		if (accentColorBox.radio[%i].getValue() == 1)
		{
			$pref::Player::AccentColor = %i;
			break;
		}
	}
	$hatColors["none"] = "";
	$hatColors["helmet"] = "0 1 2 3 4 5 6 7 8";
	$hatColors["scoutHat"] = "2 7";
	$hatColors["pointyHelmet"] = "6 7";
	$packColors["none"] = "";
	$packColors["Armor"] = "6 7";
	$packColors["Bucket"] = "7 8";
	$packColors["Cape"] = "0 1 2 3 4 5 6 7 8";
	$packColors["Pack"] = 8;
	$packColors["Quiver"] = 8;
	$packColors["Tank"] = "0 1 3 4 5 7";
	$torsoColors["decalNone.png"] = "0 1 2 3 4 5 6 7 8 9";
	$torsoColors["decalBtron.png"] = 4;
	$torsoColors["decalDarth.png"] = 7;
	$torsoColors["decalRedCoat.png"] = 4;
	$torsoColors["decalBlueCoat.png"] = 4;
	$torsoColors["decalMtron.png"] = 0;
	$torsoColors["decalNewSpace.png"] = 4;
	$torsoColors["decalOldSpace.png"] = "0 1 3 4 7";
	$torsoColors["decalCastle1.png"] = "0 1 2 3 4 5 6 7 8 9";
	$torsoColors["decalArmor.png"] = "0 1 2 3 4 5 6 7 8 9";
	$torsoColors["decalPirate1.png"] = "0 1 2 3 4 5 6 7 8 9";
	$torsoColors["decalBeads.png"] = "0 1 2 3 4 5 6 7 8 9";
	$torsoColors["decalLion.png"] = "0 1 2 3 4 5 6 7 8 9";
	$accentsAllowed["none"] = "none";
	$accentsAllowed["helmet"] = "none visor";
	$accentsAllowed["pointyHelmet"] = "none";
	$accentsAllowed["scoutHat"] = "none plume";
	$minifigColor[0] = "0.867 0.000 0.000 1.000";
	$minifigColor[1] = "0.973 0.800 0.000 1.000";
	$minifigColor[2] = "0.000 0.471 0.196 1.000";
	$minifigColor[3] = "0.000 0.317 0.745 1.000";
	$minifigColor[4] = "0.996 0.996 0.910 1.000";
	$minifigColor[5] = "0.647 0.647 0.647 1.000";
	$minifigColor[6] = "0.471 0.471 0.471 1.000";
	$minifigColor[7] = "0.200 0.200 0.200 1.000";
	$minifigColor[8] = "0.400 0.196 0.000 1.000";
	$minifigColor[9] = "0.667 0.000 0.000 0.700";
	$minifigColor[10] = "1.000 0.500 0.000 0.800";
	$minifigColor[11] = "0.850 1.000 0.000 0.600";
	$minifigColor[12] = "0.990 0.960 0.000 0.800";
	$minifigColor[13] = "0.000 0.471 0.196 0.750";
	$minifigColor[14] = "0.000 0.200 0.640 0.750";
	$minifigColor[15] = "0.550 0.700 1.000 0.700";
	$minifigColor[16] = "0.850 0.850 0.850 0.700";
	if ($hats[$pref::Player::Hat] !$= "none")
	{
		playerPreview.unHideNode("", $hats[$pref::Player::Hat]);
	}
	if ($accents[$pref::Player::Accent] !$= "none")
	{
		playerPreview.unHideNode("", $accents[$pref::Player::Accent]);
	}
	if ($packs[$pref::Player::Pack] !$= "none")
	{
		playerPreview.unHideNode("", $packs[$pref::Player::Pack]);
	}
	if ($pref::Player::Pack == 0)
	{
		playerPreview.setThreadPos("", 0, 0);
	}
	else
	{
		playerPreview.setThreadPos("", 0, 1);
	}
	playerPreview.setNodeColor("", "LeftLeg", $minifigColor[$pref::Player::LLegColor]);
	playerPreview.setNodeColor("", "RightLeg", $minifigColor[$pref::Player::RLegColor]);
	playerPreview.setNodeColor("", "Hip", $minifigColor[$pref::Player::HipColor]);
	playerPreview.setNodeColor("", "Torso", $minifigColor[$pref::Player::TorsoColor]);
	playerPreview.setNodeColor("", "LeftArm", $minifigColor[$pref::Player::LArmColor]);
	playerPreview.setNodeColor("", "RightArm", $minifigColor[$pref::Player::RArmColor]);
	playerPreview.setNodeColor("", "LeftHand", $minifigColor[$pref::Player::LHandColor]);
	playerPreview.setNodeColor("", "RightHand", $minifigColor[$pref::Player::RHandColor]);
	playerPreview.setNodeColor("", $hats[$pref::Player::Hat], $minifigColor[$pref::Player::HatColor]);
	playerPreview.setNodeColor("", $accents[$pref::Player::Accent], $minifigColor[$pref::Player::AccentColor]);
	playerPreview.setNodeColor("", $packs[$pref::Player::Pack], $minifigColor[$pref::Player::PackColor]);
	playerPreview.setIflFrame("", decal, $pref::Player::DecalColor);
	playerPreview.setIflFrame("", face, $pref::Player::FaceColor);
}

function menuChange()
{
	%hatSelection = menuHat.getSelected();
	%accentSelection = menuAccent.getSelected();
	%accentColorSelection = menuAccentColor.getSelected();
	menuAccent.clear();
	menuAccent.add("None", 0);
	if (%hatSelection == 1)
	{
		menuAccent.add("Tri-plume", 1);
	}
	if (%hatSelection == 2)
	{
		menuAccent.add("Visor", 1);
	}
	if (%accentSelection < menuAccent.size())
	{
		menuAccent.setSelected(%accentSelection);
	}
	else
	{
		menuAccent.setSelected(0);
	}
	%accentSelection = menuAccent.getSelected();
	%accentColorSelection = menuAccentColor.getSelected();
	menuAccentColor.clear();
	if (%accentSelection == 0)
	{
		fillColorList(menuAccentColor);
	}
	if (%hatSelection == 1 && %accentSelection == 1)
	{
		fillColorList(menuAccentColor);
	}
	if (%hatSelection == 2 && %accentSelection == 1)
	{
		menuAccentColor.add("Red", 0);
		menuAccentColor.add("Yellow", 1);
		menuAccentColor.add("Green", 2);
		menuAccentColor.add("Blue", 3);
		menuAccentColor.add("Light Blue", 4);
		menuAccentColor.add("Clear", 5);
		menuAccentColor.add("Black", 6);
	}
	if (%accentColorSelection < menuAccentColor.size())
	{
		menuAccentColor.setSelected(%accentColorSelection);
	}
	else
	{
		menuAccentColor.setSelected(0);
	}
	PPUpdate();
}

function optionsDlg::onWake(%this)
{
	%this.setPane(graphics);
	slider_KeyboardTurnSpeed.setValue($pref::Input::KeyboardTurnSpeed);
	Opt_SSSmartToggle.setVisible($pref::Input::UseSuperShiftToggle);
	%buffer = getDisplayDeviceList();
	%count = getFieldCount(%buffer);
	OptGraphicsDriverMenu.clear();
	OptScreenshotMenu.init();
	OptScreenshotMenu.setValue($pref::Video::screenShotFormat);
	for (%i = 0; %i < %count; %i++)
	{
		OptGraphicsDriverMenu.add(getField(%buffer, %i), %i);
	}
	%selId = OptGraphicsDriverMenu.findText($pref::Video::displayDevice);
	if (%selId == -1)
	{
		%selId = 0;
	}
	OptGraphicsDriverMenu.setSelected(%selId);
	OptGraphicsDriverMenu.onSelect(%selId, "");
	OptAudioUpdate();
	OptAudioVolumeMaster.setValue($pref::Audio::masterVolume);
	OptAudioVolumeShell.setValue($pref::Audio::channelVolume[$GuiAudioType]);
	OptAudioVolumeSim.setValue($pref::Audio::channelVolume[$SimAudioType]);
	OptAudioDriverList.clear();
	OptAudioDriverList.add("OpenAL", 1);
	OptAudioDriverList.add("None", 2);
	%selId = OptAudioDriverList.findText($pref::Audio::driver);
	if (%selId == -1)
	{
		%selId = 0;
	}
	OptAudioDriverList.setSelected(%selId);
	OptAudioDriverList.onSelect(%selId, "");
	killSelectionImages();
	SliderControlsMouseSensitivity.setValue($pref::Input::MouseSensitivity);
	SliderGraphicsAnisotropy.setValue($pref::OpenGL::anisotropy);
	SliderGraphicsDistanceMod.setValue($pref::visibleDistanceMod);
	if ($Pref::Net::ConnectionType > 0 && $Pref::Net::ConnectionType <= 7)
	{
		%obj = "OPT_ConnectionType" @ $Pref::Net::ConnectionType;
		%obj.setValue(1);
		SetConnectionType($Pref::Net::ConnectionType);
	}
	if ($pref::TextureQuality $= "")
	{
		$pref::TextureQuality = 0;
	}
	$oldTextureQuality = $pref::TextureQuality;
	for (%i = 0; %i < 5; %i++)
	{
		%obj = "OPT_TextureQuality" @ %i;
		if (%i == $pref::TextureQuality)
		{
			%obj.setValue(1);
		}
		else
		{
			%obj.setValue(0);
		}
	}
	$oldMaxLights = $pref::OpenGL::maxHardwareLights;
	if ($pref::ParticleQuality $= "")
	{
		$pref::ParticleQuality = 0;
	}
	for (%i = 0; %i < 5; %i++)
	{
		%obj = "OPT_ParticleQuality" @ %i;
		if (%i == $pref::ParticleQuality)
		{
			%obj.setValue(1);
		}
		else
		{
			%obj.setValue(0);
		}
	}
	if ($pref::ShadowResolution $= "")
	{
		$pref::ShadowResolution = 0;
	}
	for (%i = 0; %i < 5; %i++)
	{
		%obj = "OPT_ShadowQuality" @ %i;
		if (%i == $pref::ShadowResolution)
		{
			%obj.setValue(1);
		}
		else
		{
			%obj.setValue(0);
		}
	}
	if ($Pref::Gui::ChatSize $= "")
	{
		$Pref::Gui::ChatSize = 1;
	}
	for (%i = 0; %i < 11; %i++)
	{
		%obj = "OPT_ChatSize" @ %i;
		if (%i == $Pref::Gui::ChatSize)
		{
			%obj.setValue(1);
		}
		else
		{
			%obj.setValue(0);
		}
	}
	$OldMusicPref = $Pref::Audio::PlayMusic;
}

function optionsDlg::onSleep(%this)
{
	moveMap.save("base/config/client/config.cs");
	$pref::Input::MouseSensitivity = SliderControlsMouseSensitivity.getValue();
	$pref::OpenGL::anisotropy = SliderGraphicsAnisotropy.getValue();
	$pref::visibleDistanceMod = SliderGraphicsDistanceMod.getValue();
	$pref::Input::KeyboardTurnSpeed = slider_KeyboardTurnSpeed.getValue();
	clientCmdUpdatePrefs();
	killSelectionImages();
	PlayGui.createInvHud();
	PlayGui.createToolHud();
	PlayGui.LoadPaint();
	if ($OldMusicPref != $Pref::Audio::PlayMusic)
	{
		if ($Pref::Audio::PlayMusic)
		{
			if (!isObject(ServerConnection))
			{
				return;
			}
			%group = ServerConnection.getId();
			%count = %group.getCount();
			for (%i = 0; %i < %count; %i++)
			{
				%obj = %group.getObject(%i);
				if (%obj.getClassName() $= "AudioEmitter")
				{
					%profile = %obj.profile;
					%obj.profile = 0;
					%obj.profile = %profile;
					%obj.schedule(10, update);
				}
			}
		}
		else
		{
			alxStopAll();
		}
	}
	export("$pref::*", "base/config/client/prefs.cs", False);
}

function OptPlayerHeadMenu::onSelect(%this, %id, %text)
{
	%headcode = OptPlayerHeadMenu.getSelected();
	OptPlayerVisorMenu.clear();
	OptPlayerVisorMenu.add("None", -1);
	OptPlayerVisorMenu.setSelected(-1);
	if (%headcode == 0)
	{
		OptPlayerVisorMenu.add("Visor", 1);
	}
	else if (%headcode == 1)
	{
		OptPlayerVisorMenu.add("Tri-Plume", 0);
	}
	%text = OptPlayerVisorMenu.getTextById($pref::Accessory::visorCode);
	if (%text !$= "")
	{
		OptPlayerVisorMenu.setSelected($pref::Accessory::visorCode);
	}
}

function UpdatePacketSize()
{
	PacketSizeDisplay.setValue(mFloor(SliderPacketSize.getValue()));
	$pref::Net::PacketSize = PacketSizeDisplay.getValue();
}

function UpdateLagThreshold()
{
	LagThresholdDisplay.setValue(mFloor(SliderLagThreshold.getValue()));
	$Pref::Net::LagThreshold = LagThresholdDisplay.getValue();
}

function UpdateRateToClient()
{
	RateToClientDisplay.setValue(mFloor(SliderRateToClient.getValue()));
	$pref::Net::PacketRateToClient = RateToClientDisplay.getValue();
}

function UpdateRateToServer()
{
	RateToServerDisplay.setValue(mFloor(SliderRateToServer.getValue()));
	$pref::Net::PacketRateToServer = RateToServerDisplay.getValue();
}

function OptGraphicsDriverMenu::onSelect(%this, %id, %text)
{
	if (OptGraphicsResolutionMenu.size() > 0)
	{
		%prevRes = OptGraphicsResolutionMenu.getText();
	}
	else
	{
		%prevRes = getWords($pref::Video::resolution, 0, 1);
	}
	if (isDeviceFullScreenOnly(%this.getText()))
	{
		OptGraphicsFullscreenToggle.setValue(1);
		OptGraphicsFullscreenToggle.setActive(0);
		OptGraphicsFullscreenToggle.onAction();
	}
	else
	{
		OptGraphicsFullscreenToggle.setActive(1);
	}
	if (OptGraphicsFullscreenToggle.getValue())
	{
		if (OptGraphicsBPPMenu.size() > 0)
		{
			%prevBPP = OptGraphicsBPPMenu.getText();
		}
		else
		{
			%prevBPP = getWord($pref::Video::resolution, 2);
		}
	}
	OptGraphicsResolutionMenu.init(%this.getText(), OptGraphicsFullscreenToggle.getValue());
	OptGraphicsBPPMenu.init(%this.getText());
	%selId = OptGraphicsResolutionMenu.findText(%prevRes);
	if (%selId == -1)
	{
		%selId = 0;
	}
	OptGraphicsResolutionMenu.setSelected(%selId);
	if (OptGraphicsFullscreenToggle.getValue())
	{
		%selId = OptGraphicsBPPMenu.findText(%prevBPP);
		if (%selId == -1)
		{
			%selId = 0;
		}
		OptGraphicsBPPMenu.setSelected(%selId);
		OptGraphicsBPPMenu.setText(OptGraphicsBPPMenu.getTextById(%selId));
	}
	else
	{
		OptGraphicsBPPMenu.setText("Default");
	}
	OptGraphicsResolutionMenu.onSelect();
}

function OptGraphicsResolutionMenu::onSelect(%this, %id, %text)
{
	OptGraphicsHzMenu.clear();
	%device = OptGraphicsDriverMenu.getText();
	%resList = getResolutionList(%device);
	%resCount = getFieldCount(%resList);
	%currRes = OptGraphicsResolutionMenu.getText();
	%currBpp = OptGraphicsBPPMenu.getText();
	if (%currBpp $= "Default")
	{
		%currBpp = getWord(getRes(), 2);
	}
	if (!OptGraphicsFullscreenToggle.getValue())
	{
		OptGraphicsHzMenu.add("Default", 0);
		OptGraphicsHzMenu.setSelected(0);
		return;
	}
	%count = -1;
	for (%i = 0; %i < %resCount; %i++)
	{
		%checkField = getField(%resList, %i);
		%checkRes = getWords(%checkField, 0, 1);
		%checkBpp = getWord(%checkField, 2);
		if (%checkRes $= %currRes && %checkBpp $= %currBpp)
		{
			%refreshRate = getWord(%checkField, 3);
			echo("--hz = ", %refreshRate);
			OptGraphicsHzMenu.add(%refreshRate, %count++);
		}
	}
	OptGraphicsHzMenu.setSelected(%count);
}

function OptGraphicsResolutionMenu::init(%this, %device, %fullScreen)
{
	%this.clear();
	%resList = getResolutionList(%device);
	%resCount = getFieldCount(%resList);
	%deskRes = getDesktopResolution();
	%count = 0;
	for (%i = 0; %i < %resCount; %i++)
	{
		%res = getWords(getField(%resList, %i), 0, 1);
		if (!%fullScreen)
		{
			if (firstWord(%res) >= firstWord(%deskRes))
			{
				continue;
			}
			if (getWord(%res, 1) >= getWord(%deskRes, 1))
			{
				continue;
			}
		}
		if (%this.findText(%res) == -1)
		{
			%this.add(%res, %count);
			%count++;
		}
	}
}

function OptGraphicsFullscreenToggle::onAction(%this)
{
	Parent::onAction();
	%prevRes = OptGraphicsResolutionMenu.getText();
	OptGraphicsResolutionMenu.init(OptGraphicsDriverMenu.getText(), %this.getValue());
	%selId = OptGraphicsResolutionMenu.findText(%prevRes);
	if (%selId == -1)
	{
		%selId = 0;
	}
	OptGraphicsResolutionMenu.setSelected(%selId);
}

function OptGraphicsBPPMenu::init(%this, %device)
{
	%this.clear();
	if (%device $= "Voodoo2")
	{
		%this.add(16, 0);
	}
	else
	{
		%resList = getResolutionList(%device);
		%resCount = getFieldCount(%resList);
		%count = 0;
		for (%i = 0; %i < %resCount; %i++)
		{
			%bpp = getWord(getField(%resList, %i), 2);
			if (%this.findText(%bpp) == -1)
			{
				%this.add(%bpp, %count);
				%count++;
			}
		}
	}
}

function OptScreenshotMenu::init(%this)
{
	if (%this.findText("PNG") == -1)
	{
		%this.add("PNG", 0);
	}
	if (%this.findText("JPEG") == -1)
	{
		%this.add("JPEG", 1);
	}
}

function optionsDlg::applyGraphics(%this)
{
	%newDriver = OptGraphicsDriverMenu.getText();
	%newRes = OptGraphicsResolutionMenu.getText();
	%newBpp = OptGraphicsBPPMenu.getText();
	%newFullScreen = OptGraphicsFullscreenToggle.getValue();
	%newHz = OptGraphicsHzMenu.getText();
	$pref::Video::screenShotFormat = OptScreenshotMenu.getText();
	if ($pref::TextureQuality != $oldTextureQuality || %newDriver !$= $pref::Video::displayDevice || $pref::OpenGL::maxHardwareLights != $oldMaxLights)
	{
		$oldTextureQuality = $pref::TextureQuality;
		$oldMaxLights = $pref::OpenGL::maxHardwareLights;
		if (%newFullScreen)
		{
			setDisplayDevice(%newDriver, firstWord(%newRes), getWord(%newRes, 1), %newBpp, %newFullScreen, %newHz);
		}
		else
		{
			setDisplayDevice(%newDriver, firstWord(%newRes), getWord(%newRes, 1), %newBpp, %newFullScreen);
		}
	}
	else if (%newFullScreen)
	{
		setScreenMode(firstWord(%newRes), getWord(%newRes, 1), %newBpp, %newFullScreen, %newHz);
	}
	else
	{
		setScreenMode(firstWord(%newRes), getWord(%newRes, 1), %newBpp, %newFullScreen);
	}
}

function optionsDlg::applyPlayer(%this)
{
}

$RemapCount = 0;
$RemapDivision[$RemapCount] = "Movement";
$RemapName[$RemapCount] = "Forward";
$RemapCmd[$RemapCount] = "moveforward";
$RemapCount++;
$RemapName[$RemapCount] = "Backward";
$RemapCmd[$RemapCount] = "movebackward";
$RemapCount++;
$RemapName[$RemapCount] = "Strafe Left";
$RemapCmd[$RemapCount] = "moveleft";
$RemapCount++;
$RemapName[$RemapCount] = "Strafe Right";
$RemapCmd[$RemapCount] = "moveright";
$RemapCount++;
$RemapName[$RemapCount] = "Jump";
$RemapCmd[$RemapCount] = "jump";
$RemapCount++;
$RemapName[$RemapCount] = "Crouch";
$RemapCmd[$RemapCount] = "crouch";
$RemapCount++;
$RemapName[$RemapCount] = "Walk";
$RemapCmd[$RemapCount] = "walk";
$RemapCount++;
$RemapName[$RemapCount] = "Jet";
$RemapCmd[$RemapCount] = "jet";
$RemapCount++;
$RemapDivision[$RemapCount] = "View";
$RemapName[$RemapCount] = "Turn Left";
$RemapCmd[$RemapCount] = "turnLeft";
$RemapCount++;
$RemapName[$RemapCount] = "Turn Right";
$RemapCmd[$RemapCount] = "turnRight";
$RemapCount++;
$RemapName[$RemapCount] = "Look Up";
$RemapCmd[$RemapCount] = "panUp";
$RemapCount++;
$RemapName[$RemapCount] = "Look Down";
$RemapCmd[$RemapCount] = "panDown";
$RemapCount++;
$RemapName[$RemapCount] = "Toggle Zoom";
$RemapCmd[$RemapCount] = "toggleZoom";
$RemapCount++;
$RemapName[$RemapCount] = "Free Look";
$RemapCmd[$RemapCount] = "toggleFreeLook";
$RemapCount++;
$RemapName[$RemapCount] = "Switch 1st/3rd";
$RemapCmd[$RemapCount] = "toggleFirstPerson";
$RemapCount++;
$RemapName[$RemapCount] = "Drop Camera at Player";
$RemapCmd[$RemapCount] = "dropCameraAtPlayer";
$RemapCount++;
$RemapName[$RemapCount] = "Drop Player at Camera";
$RemapCmd[$RemapCount] = "dropPlayerAtCamera";
$RemapCount++;
$RemapDivision[$RemapCount] = "Action";
$RemapName[$RemapCount] = "Fire Weapon/Tool";
$RemapCmd[$RemapCount] = "mouseFire";
$RemapCount++;
$RemapName[$RemapCount] = "Suicide";
$RemapCmd[$RemapCount] = "Suicide";
$RemapCount++;
$RemapName[$RemapCount] = "Next Vehicle Seat";
$RemapCmd[$RemapCount] = "NextSeat";
$RemapCount++;
$RemapName[$RemapCount] = "Prev Vehicle Seat";
$RemapCmd[$RemapCount] = "PrevSeat";
$RemapCount++;
$RemapDivision[$RemapCount] = "Communication";
$RemapName[$RemapCount] = "Global Chat";
$RemapCmd[$RemapCount] = "GlobalChat";
$RemapCount++;
$RemapName[$RemapCount] = "Team Chat";
$RemapCmd[$RemapCount] = "TeamChat";
$RemapCount++;
$RemapName[$RemapCount] = "Chat Hud PageUp";
$RemapCmd[$RemapCount] = "PageUpNewChatHud";
$RemapCount++;
$RemapName[$RemapCount] = "Chat Hud PageDown";
$RemapCmd[$RemapCount] = "PageDownNewChatHud";
$RemapCount++;
$RemapDivision[$RemapCount] = "Gui";
$RemapName[$RemapCount] = "Toggle Cursor";
$RemapCmd[$RemapCount] = "ToggleCursor";
$RemapCount++;
$RemapName[$RemapCount] = "Toggle Player Names";
$RemapCmd[$RemapCount] = "ToggleShapeNameHud";
$RemapCount++;
$RemapName[$RemapCount] = "Open Admin Window";
$RemapCmd[$RemapCount] = "openAdminWindow";
$RemapCount++;
$RemapName[$RemapCount] = "Open Options Window";
$RemapCmd[$RemapCount] = "openOptionsWindow";
$RemapCount++;
$RemapName[$RemapCount] = "Show Player List";
$RemapCmd[$RemapCount] = "showPlayerList";
$RemapCount++;
$RemapName[$RemapCount] = "Toggle NetGraph";
$RemapCmd[$RemapCount] = "toggleNetGraph";
$RemapCount++;
$RemapDivision[$RemapCount] = "Tools / Inventory";
$RemapName[$RemapCount] = "Use Bricks";
$RemapCmd[$RemapCount] = "useBricks";
$RemapCount++;
$RemapName[$RemapCount] = "Use Tools";
$RemapCmd[$RemapCount] = "useTools";
$RemapCount++;
$RemapName[$RemapCount] = "Use Spray Can";
$RemapCmd[$RemapCount] = "useSprayCan";
$RemapCount++;
$RemapName[$RemapCount] = "Use Light";
$RemapCmd[$RemapCount] = "useLight";
$RemapCount++;
$RemapName[$RemapCount] = "Drop Tool";
$RemapCmd[$RemapCount] = "dropTool";
$RemapCount++;
$RemapName[$RemapCount] = "Use 1st Slot";
$RemapCmd[$RemapCount] = "useFirstSlot";
$RemapCount++;
$RemapName[$RemapCount] = "Use 2nd Slot";
$RemapCmd[$RemapCount] = "useSecondSlot";
$RemapCount++;
$RemapName[$RemapCount] = "Use 3rd Slot";
$RemapCmd[$RemapCount] = "useThirdSlot";
$RemapCount++;
$RemapName[$RemapCount] = "Use 4th Slot";
$RemapCmd[$RemapCount] = "useFourthSlot";
$RemapCount++;
$RemapName[$RemapCount] = "Use 5th Slot";
$RemapCmd[$RemapCount] = "useFifthSlot";
$RemapCount++;
$RemapName[$RemapCount] = "Use 6th Slot";
$RemapCmd[$RemapCount] = "useSixthSlot";
$RemapCount++;
$RemapName[$RemapCount] = "Use 7th Slot";
$RemapCmd[$RemapCount] = "useSeventhSlot";
$RemapCount++;
$RemapName[$RemapCount] = "Use 8th Slot";
$RemapCmd[$RemapCount] = "useEighthSlot";
$RemapCount++;
$RemapName[$RemapCount] = "Use 9th Slot";
$RemapCmd[$RemapCount] = "useNinthSlot";
$RemapCount++;
$RemapName[$RemapCount] = "Use 10th Slot";
$RemapCmd[$RemapCount] = "useTenthSlot";
$RemapCount++;
$RemapName[$RemapCount] = "Inventory Up";
$RemapCmd[$RemapCount] = "invUp";
$RemapCount++;
$RemapName[$RemapCount] = "Inventory Down";
$RemapCmd[$RemapCount] = "invDown";
$RemapCount++;
$RemapName[$RemapCount] = "Inventory Left";
$RemapCmd[$RemapCount] = "invLeft";
$RemapCount++;
$RemapName[$RemapCount] = "Inventory Right";
$RemapCmd[$RemapCount] = "invRight";
$RemapCount++;
$RemapDivision[$RemapCount] = "Building";
$RemapName[$RemapCount] = "Open Brick Selector";
$RemapCmd[$RemapCount] = "openBSD";
$RemapCount++;
$RemapName[$RemapCount] = "Plant Brick";
$RemapCmd[$RemapCount] = "plantBrick";
$RemapCount++;
$RemapName[$RemapCount] = "Undo Brick";
$RemapCmd[$RemapCount] = "undoBrick";
$RemapCount++;
$RemapName[$RemapCount] = "Cancel Brick";
$RemapCmd[$RemapCount] = "cancelBrick";
$RemapCount++;
$RemapName[$RemapCount] = "Shift Brick Away";
$RemapCmd[$RemapCount] = "shiftBrickAway";
$RemapCount++;
$RemapName[$RemapCount] = "Shift Brick Towards";
$RemapCmd[$RemapCount] = "shiftBrickTowards";
$RemapCount++;
$RemapName[$RemapCount] = "Shift Brick Left";
$RemapCmd[$RemapCount] = "shiftBrickLeft";
$RemapCount++;
$RemapName[$RemapCount] = "Shift Brick Right";
$RemapCmd[$RemapCount] = "shiftBrickRight";
$RemapCount++;
$RemapName[$RemapCount] = "Shift Brick Up";
$RemapCmd[$RemapCount] = "shiftBrickUp";
$RemapCount++;
$RemapName[$RemapCount] = "Shift Brick Down";
$RemapCmd[$RemapCount] = "shiftBrickDown";
$RemapCount++;
$RemapName[$RemapCount] = "Shift Brick Up 1/3";
$RemapCmd[$RemapCount] = "shiftBrickThirdUp";
$RemapCount++;
$RemapName[$RemapCount] = "Shift Brick Down 1/3";
$RemapCmd[$RemapCount] = "shiftBrickThirdDown";
$RemapCount++;
$RemapName[$RemapCount] = "Rotate Brick CW";
$RemapCmd[$RemapCount] = "RotateBrickCW";
$RemapCount++;
$RemapName[$RemapCount] = "Rotate Brick CCW";
$RemapCmd[$RemapCount] = "RotateBrickCCW";
$RemapCount++;
$RemapName[$RemapCount] = "Toggle Super Shift";
$RemapCmd[$RemapCount] = "toggleSuperShift";
$RemapCount++;
$RemapName[$RemapCount] = "Super Shift Brick Away";
$RemapCmd[$RemapCount] = "superShiftBrickAwayProxy";
$RemapCount++;
$RemapName[$RemapCount] = "Super Shift Brick Towards";
$RemapCmd[$RemapCount] = "superShiftBrickTowardsProxy";
$RemapCount++;
$RemapName[$RemapCount] = "Super Shift Brick Left";
$RemapCmd[$RemapCount] = "superShiftBrickLeftProxy";
$RemapCount++;
$RemapName[$RemapCount] = "Super Shift Brick Right";
$RemapCmd[$RemapCount] = "superShiftBrickRightProxy";
$RemapCount++;
$RemapName[$RemapCount] = "Super Shift Brick Up";
$RemapCmd[$RemapCount] = "superShiftBrickUpProxy";
$RemapCount++;
$RemapName[$RemapCount] = "Super Shift Brick Down";
$RemapCmd[$RemapCount] = "superShiftBrickDownProxy";
$RemapCount++;
$RemapName[$RemapCount] = "Toggle Build Macro Recording";
$RemapCmd[$RemapCount] = "ToggleBuildMacroRecording";
$RemapCount++;
$RemapName[$RemapCount] = "Playback Build Macro";
$RemapCmd[$RemapCount] = "PlayBackBuildMacro";
$RemapCount++;
$RemapDivision[$RemapCount] = "Recording";
$RemapName[$RemapCount] = "Take Hud Screenshot";
$RemapCmd[$RemapCount] = "doHudScreenshot";
$RemapCount++;
$RemapName[$RemapCount] = "Take Screenshot";
$RemapCmd[$RemapCount] = "doScreenShot";
$RemapCount++;
$RemapName[$RemapCount] = "Take DOF Screenshot";
$RemapCmd[$RemapCount] = "doDofScreenShot";
$RemapCount++;
$RemapName[$RemapCount] = "Start Recording Demo";
$RemapCmd[$RemapCount] = "startRecordingDemo";
$RemapCount++;
$RemapName[$RemapCount] = "Stop Recording Demo";
$RemapCmd[$RemapCount] = "stopRecordingDemo";
$RemapCount++;
$RemapDivision[$RemapCount] = "Emotes";
$RemapName[$RemapCount] = "Sit";
$RemapCmd[$RemapCount] = "emoteSit";
$RemapCount++;
$RemapName[$RemapCount] = "Love";
$RemapCmd[$RemapCount] = "emoteLove";
$RemapCount++;
$RemapName[$RemapCount] = "Hate";
$RemapCmd[$RemapCount] = "emoteHate";
$RemapCount++;
$RemapName[$RemapCount] = "Confusion";
$RemapCmd[$RemapCount] = "emoteConfusion";
$RemapCount++;
$RemapName[$RemapCount] = "Alarm";
$RemapCmd[$RemapCount] = "emoteAlarm";
$RemapCount++;
function restoreDefaultMappings()
{
	moveMap.delete();
	exec("base/client/scripts/default.bind.cs");
	OptRemapList.fillList();
}

function getMapDisplayName(%device, %action)
{
	if (%device $= "keyboard")
	{
		return %action;
	}
	else if (strstr(%device, "mouse") != -1)
	{
		%pos = strstr(%action, "button");
		if (%pos != -1)
		{
			%mods = getSubStr(%action, 0, %pos);
			%object = getSubStr(%action, %pos, 1000);
			%instance = getSubStr(%object, strlen("button"), 1000);
			return %mods @ "mouse" @ %instance + 1;
		}
		else
		{
			error("Mouse input object other than button passed to getDisplayMapName!");
		}
	}
	else if (strstr(%device, "joystick") != -1)
	{
		%pos = strstr(%action, "button");
		if (%pos != -1)
		{
			%mods = getSubStr(%action, 0, %pos);
			%object = getSubStr(%action, %pos, 1000);
			%instance = getSubStr(%object, strlen("button"), 1000);
			return %mods @ "joystick" @ %instance + 1;
		}
		else
		{
			%pos = strstr(%action, "pov");
			if (%pos != -1)
			{
				%wordCount = getWordCount(%action);
				%mods = %wordCount > 1 ? getWords(%action, 0, %wordCount - 2) @ " " : "";
				%object = getWord(%action, %wordCount - 1);
				if (%object $= "upov")
				{
					%object = "POV1 up";
				}
				else if (%object $= "dpov")
				{
					%object = "POV1 down";
				}
				else if (%object $= "lpov")
				{
					%object = "POV1 left";
				}
				else if (%object $= "rpov")
				{
					%object = "POV1 right";
				}
				else if (%object $= "upov2")
				{
					%object = "POV2 up";
				}
				else if (%object $= "dpov2")
				{
					%object = "POV2 down";
				}
				else if (%object $= "lpov2")
				{
					%object = "POV2 left";
				}
				else if (%object $= "rpov2")
				{
					%object = "POV2 right";
				}
				else
				{
					%object = "??";
				}
				return %mods @ %object;
			}
			else
			{
				error("Unsupported Joystick input object passed to getDisplayMapName!");
			}
		}
	}
	return "??";
}

function buildFullMapString(%index)
{
	%name = $RemapName[%index];
	%cmd = $RemapCmd[%index];
	%temp = moveMap.getBinding(%cmd);
	%device = getField(%temp, 0);
	%object = getField(%temp, 1);
	if (%device !$= "" && %object !$= "")
	{
		%mapString = getMapDisplayName(%device, %object);
	}
	else
	{
		%mapString = "";
	}
	%mapString = strupr(%mapString);
	%name = %name @ "\c5";
	return %name TAB %mapString;
}

function OptRemapList::fillList(%this)
{
	%this.clear();
	for (%i = 0; %i < $RemapCount; %i++)
	{
		if ($RemapDivision[%i] !$= "")
		{
			if (%i != 0)
			{
				%this.addRow(-1, "");
			}
			%this.addRow(-1, "   \c4" @ $RemapDivision[%i]);
			%this.addRow(-1, "\c4------------------------------------------------------------------");
		}
		%this.addRow(%i, buildFullMapString(%i));
	}
}

function OptRemapList::doRemap(%this)
{
	%selId = %this.getSelectedId();
	%name = $RemapName[%selId];
	if (%name $= "")
	{
		return;
	}
	OptRemapText.setValue("REMAP \"" @ %name @ "\"");
	OptRemapInputCtrl.index = %selId;
	Canvas.pushDialog(RemapDlg);
}

function optionsDlg::RemapAll(%this)
{
	optionsDlg.remappingAll = 1;
	optionsDlg.RemapNext(-1);
}

function optionsDlg::RemapNext(%this, %idx)
{
	%idx++;
	%name = $RemapName[%idx];
	if (%name $= "")
	{
		optionsDlg.remappingAll = 0;
		return;
	}
	else
	{
		OptRemapText.setValue("REMAP \"" @ %name @ "\"");
		OptRemapInputCtrl.index = %idx;
		Canvas.pushDialog(RemapDlg);
		return;
	}
}

function redoMapping(%device, %action, %cmd, %oldIndex, %newIndex)
{
	moveMap.bind(%device, %action, %cmd);
	OptRemapList.setRowById(%oldIndex, buildFullMapString(%oldIndex));
	OptRemapList.setRowById(%newIndex, buildFullMapString(%newIndex));
	if (optionsDlg.remappingAll == 1)
	{
		optionsDlg.RemapNext(%newIndex);
	}
}

function findRemapCmdIndex(%command)
{
	for (%i = 0; %i < $RemapCount; %i++)
	{
		if (%command $= $RemapCmd[%i])
		{
			return %i;
		}
	}
	return -1;
}

function OptRemapInputCtrl::onInputEvent(%this, %device, %action)
{
	Canvas.popDialog(RemapDlg);
	if (%device $= "keyboard")
	{
		if (%action $= "escape")
		{
			optionsDlg.remappingAll = 0;
			return;
		}
		else if (%action $= "backspace")
		{
			%bind = moveMap.getBinding($RemapCmd[%this.index]);
			%device = getWord(%bind, 0);
			%action = getWords(%bind, 1, getWordCount(%bind) - 1);
			moveMap.unbind(%device, %action);
			OptRemapList.setRowById(%this.index, buildFullMapString(%this.index));
			if (optionsDlg.remappingAll == 1)
			{
				optionsDlg.RemapNext(%this.index);
			}
			return;
		}
	}
	%cmd = $RemapCmd[%this.index];
	%name = $RemapName[%this.index];
	%prevMap = moveMap.getCommand(%device, %action);
	if (%prevMap !$= %cmd)
	{
		if (%prevMap $= "")
		{
			moveMap.bind(%device, %action, %cmd);
			OptRemapList.setRowById(%this.index, buildFullMapString(%this.index));
		}
		else
		{
			%mapName = getMapDisplayName(%device, %action);
			%prevMapIndex = findRemapCmdIndex(%prevMap);
			if (%prevMapIndex == -1)
			{
				MessageBoxOK("REMAP FAILED", "\"" @ %mapName @ "\" is already bound to a non-remappable command!");
			}
			else
			{
				%prevCmdName = $RemapName[%prevMapIndex];
				MessageBoxYesNo("WARNING", "\"" @ %mapName @ "\" is already bound to \"" @ %prevCmdName @ "\"!\nDo you want to undo this mapping?", "redoMapping(" @ %device @ ", \"" @ %action @ "\", \"" @ %cmd @ "\", " @ %prevMapIndex @ ", " @ %this.index @ ");", "");
			}
			return;
		}
	}
	if (optionsDlg.remappingAll == 1)
	{
		optionsDlg.RemapNext(%this.index);
	}
}

function optionsDlg::clearAllBinds(%this, %confirm)
{
	if (!%confirm)
	{
		MessageBoxYesNo("Clear All Binds?", "Are you sure you want to clear your control configuration?", "optionsDlg.clearAllBinds(1);");
	}
	else
	{
		for (%index = 0; %index < $RemapCount; %index++)
		{
			%bind = moveMap.getBinding($RemapCmd[%index]);
			%device = getWord(%bind, 0);
			%action = getWords(%bind, 1, getWordCount(%bind) - 1);
			moveMap.unbind(%device, %action);
			OptRemapList.setRowById(%index, buildFullMapString(%index));
		}
	}
}

function optionsDlg::setDefaultBinds(%this, %confirm)
{
	if (!%confirm)
	{
		MessageBoxYesNo("Set Default Binds?", "Are you sure you want to reset your controls to the default?", "optionsDlg.setDefaultBinds(1);");
	}
}

function OptAudioUpdate()
{
	%text = " Vendor: " @ alGetString("AL_VENDOR") @ "\n Version: " @ alGetString("AL_VERSION") @ "\n Renderer: " @ alGetString("AL_RENDERER") @ "\n Extensions: " @ alGetString("AL_EXTENSIONS");
	OptAudioInfo.setText(%text);
}

new AudioDescription(AudioChannel0)
{
	volume = 1;
	isLooping = 0;
	is3D = 0;
	type = 0;
};
new AudioDescription(AudioChannel1)
{
	volume = 1;
	isLooping = 0;
	is3D = 0;
	type = 1;
};
new AudioDescription(AudioChannel2)
{
	volume = 1;
	isLooping = 0;
	is3D = 0;
	type = 2;
};
new AudioDescription(AudioChannel3)
{
	volume = 1;
	isLooping = 0;
	is3D = 0;
	type = 3;
};
new AudioDescription(AudioChannel4)
{
	volume = 1;
	isLooping = 0;
	is3D = 0;
	type = 4;
};
new AudioDescription(AudioChannel5)
{
	volume = 1;
	isLooping = 0;
	is3D = 0;
	type = 5;
};
new AudioDescription(AudioChannel6)
{
	volume = 1;
	isLooping = 0;
	is3D = 0;
	type = 6;
};
new AudioDescription(AudioChannel7)
{
	volume = 1;
	isLooping = 0;
	is3D = 0;
	type = 7;
};
new AudioDescription(AudioChannel8)
{
	volume = 1;
	isLooping = 0;
	is3D = 0;
	type = 8;
};
$AudioTestHandle = 0;
function OptAudioUpdateMasterVolume(%volume)
{
	if (%volume == $pref::Audio::masterVolume)
	{
		return;
	}
	alxListenerf(AL_GAIN_LINEAR, %volume);
	$pref::Audio::masterVolume = %volume;
	if (!alxIsPlaying($AudioTestHandle))
	{
		$AudioTestHandle = alxCreateSource("AudioChannel0", ExpandFilename("base/data/sound/testing.wav"));
		alxPlay($AudioTestHandle);
	}
}

function OptAudioUpdateChannelVolume(%channel, %volume)
{
	if (%channel < 1 || %channel > 8)
	{
		return;
	}
	if (%volume == $pref::Audio::channelVolume[%channel])
	{
		return;
	}
	alxSetChannelVolume(%channel, %volume);
	$pref::Audio::channelVolume[%channel] = %volume;
	if (!alxIsPlaying($AudioTestHandle))
	{
		$AudioTestHandle = alxCreateSource("AudioChannel" @ %channel, ExpandFilename("base/data/sound/testing.wav"));
		alxPlay($AudioTestHandle);
	}
}

function OptAudioDriverList::onSelect(%this, %id, %text)
{
	if (%text $= "")
	{
		return;
	}
	if ($pref::Audio::driver $= %text)
	{
		return;
	}
	$pref::Audio::driver = %text;
	OpenALInit();
}

function setActiveInv(%index)
{
	if (%index < 0)
	{
		HUD_BrickActive.setVisible(0);
		HUD_BrickName.setText("");
		return;
	}
	HUD_BrickActive.setVisible(1);
	%x = 64 * %index;
	%y = 0;
	%w = 64;
	%h = 64;
	HUD_BrickActive.resize(%x, %y, %w, %h);
	HUD_BrickName.setText($InvData[%index].uiName);
	return;
	eval("HUDInvActive" @ $CurrScrollBrickSlot @ ".setVisible(false);");
	if (%index < 0)
	{
		return;
	}
	eval("HUDInvActive" @ %index @ ".setVisible(true);");
}

function getActiveInv()
{
	return;
	for (%i = 0; %i < $BSD_NumInventorySlots; %i++)
	{
		eval("%val = (HUDInvActive" @ %i @ ".visible == true);");
		if (%val == 1)
		{
			return %i;
		}
	}
	return -1;
}

function directSelectInv(%index)
{
	if (%index < 0)
	{
		scrollBricks(1);
		return;
	}
	if ($InvData[%index] > 0)
	{
		if ($ScrollMode == $SCROLLMODE_BRICKS)
		{
			if ($CurrScrollBrickSlot == %index && HUD_BrickActive.visible == 1)
			{
				setActiveInv(-1);
				HUD_BrickName.setText("");
				commandToServer('unUseTool');
				setScrollMode($SCROLLMODE_NONE);
			}
			else
			{
				setActiveInv(%index);
				$CurrScrollBrickSlot = %index;
				commandToServer('useInventory', %index);
				if ($RecordingBuildMacro)
				{
					$BuildMacroSO.pushEvent("Server", 'useInventory', %index);
				}
			}
		}
		else
		{
			setScrollMode($SCROLLMODE_BRICKS);
			setActiveInv(%index);
			$CurrScrollBrickSlot = %index;
			commandToServer('useInventory', %index);
			if ($RecordingBuildMacro)
			{
				$BuildMacroSO.pushEvent("Server", 'useInventory', %index);
			}
			HUD_BrickName.setText($InvData[$CurrScrollBrickSlot].uiName);
		}
	}
	else
	{
		%direction = 1;
		$CurrScrollBrickSlot--;
		for (%i = 0; %i < $BSD_NumInventorySlots - 1; %i++)
		{
			$CurrScrollBrickSlot += %direction;
			if ($CurrScrollBrickSlot < 0)
			{
				$CurrScrollBrickSlot = $BSD_NumInventorySlots - 1;
			}
			if ($CurrScrollBrickSlot >= $BSD_NumInventorySlots)
			{
				$CurrScrollBrickSlot = 0;
			}
			if ($InvData[$CurrScrollBrickSlot] > 0)
			{
				break;
			}
		}
		if ($InvData[$CurrScrollBrickSlot] > 0)
		{
			setActiveInv($CurrScrollBrickSlot);
			commandToServer('useInventory', $CurrScrollBrickSlot);
			if ($RecordingBuildMacro)
			{
				$BuildMacroSO.pushEvent("Server", 'useInventory', $CurrScrollBrickSlot);
			}
		}
		else if (isObject($LastInstantUseData))
		{
			commandToServer('InstantUseBrick', $LastInstantUseData);
			$InstantUse = 1;
			setActiveInv(-1);
			setScrollMode($SCROLLMODE_BRICKS);
			HUD_BrickName.setText($LastInstantUseData.uiName);
			return 1;
		}
		else
		{
			%device = getWord(moveMap.getBinding(openBSD), 0);
			if (%device $= "Keyboard")
			{
				%hintKey = strupr(getWord(moveMap.getBinding(openBSD), 1));
			}
			else if (%device $= "Mouse0")
			{
				%hintKey = "MOUSE " @ strupr(getWord(moveMap.getBinding(openBSD), 1));
			}
			else if (%device $= "Joystick0")
			{
				%hintKey = "JOYSTICK " @ strupr(getWord(moveMap.getBinding(openBSD), 1));
			}
			else
			{
				%hintKey = moveMap.getBinding(openBSD);
			}
			clientCmdCenterPrint("\c5You don't have any bricks!\nPress " @ %hintKey @ " to open the brick selector.", 3);
			return 0;
		}
		if ($ScrollMode != $SCROLLMODE_BRICKS)
		{
			setScrollMode($SCROLLMODE_BRICKS);
		}
	}
	return 1;
}

function useFirstSlot(%val)
{
	if (%val)
	{
		directSelectInv(0);
	}
}

function useSecondSlot(%val)
{
	if (%val)
	{
		directSelectInv(1);
	}
}

function useThirdSlot(%val)
{
	if (%val)
	{
		directSelectInv(2);
	}
}

function useFourthSlot(%val)
{
	if (%val)
	{
		directSelectInv(3);
	}
}

function useFifthSlot(%val)
{
	if (%val)
	{
		directSelectInv(4);
	}
}

function useSixthSlot(%val)
{
	if (%val)
	{
		directSelectInv(5);
	}
}

function useSeventhSlot(%val)
{
	if (%val)
	{
		directSelectInv(6);
	}
}

function useEighthSlot(%val)
{
	if (%val)
	{
		directSelectInv(7);
	}
}

function useNinthSlot(%val)
{
	if (%val)
	{
		directSelectInv(8);
	}
}

function useTenthSlot(%val)
{
	if (%val)
	{
		directSelectInv(9);
	}
}

function dropFirstSlot(%val)
{
	if (%val)
	{
		commandToServer('dropInventory', 0);
	}
}

function dropSecondSlot(%val)
{
	if (%val)
	{
		commandToServer('dropInventory', 1);
	}
}

function dropThirdSlot(%val)
{
	if (%val)
	{
		commandToServer('dropInventory', 2);
	}
}

function dropFourthSlot(%val)
{
	if (%val)
	{
		commandToServer('dropInventory', 3);
	}
}

function dropFifthSlot(%val)
{
	if (%val)
	{
		commandToServer('dropInventory', 4);
	}
}

function dropSixthSlot(%val)
{
	if (%val)
	{
		commandToServer('dropInventory', 5);
	}
}

function dropSeventhSlot(%val)
{
	if (%val)
	{
		commandToServer('dropInventory', 6);
	}
}

function dropEighthSlot(%val)
{
	if (%val)
	{
		commandToServer('dropInventory', 7);
	}
}

function dropNinthSlot(%val)
{
	if (%val)
	{
		commandToServer('dropInventory', 8);
	}
}

function dropTenthSlot(%val)
{
	if (%val)
	{
		commandToServer('dropInventory', 9);
	}
}

$BrickFirstRepeatTime = 200;
$BrickRepeatTime = 50;
function repeatBrickAway(%val)
{
	if (%val == $brickAway)
	{
		commandToServer('shiftBrick', 1, 0, 0);
		if ($RecordingBuildMacro)
		{
			$BuildMacroSO.pushEvent("Server", 'shiftBrick', 1, 0, 0);
		}
		schedule($BrickRepeatTime, 0, repeatBrickAway, %val);
	}
}

function repeatBrickTowards(%val)
{
	if (%val == $brickTowards)
	{
		commandToServer('shiftBrick', -1, 0, 0);
		if ($RecordingBuildMacro)
		{
			$BuildMacroSO.pushEvent("Server", 'shiftBrick', -1, 0, 0);
		}
		schedule($BrickRepeatTime, 0, repeatBrickTowards, %val);
	}
}

function repeatBrickLeft(%val)
{
	if (%val == $brickLeft)
	{
		commandToServer('shiftBrick', 0, 1, 0);
		if ($RecordingBuildMacro)
		{
			$BuildMacroSO.pushEvent("Server", 'shiftBrick', 0, 1, 0);
		}
		schedule($BrickRepeatTime, 0, repeatBrickLeft, %val);
	}
}

function repeatBrickRight(%val)
{
	if (%val == $brickRight)
	{
		commandToServer('shiftBrick', 0, -1, 0);
		if ($RecordingBuildMacro)
		{
			$BuildMacroSO.pushEvent("Server", 'shiftBrick', 0, -1, 0);
		}
		schedule($BrickRepeatTime, 0, repeatBrickRight, %val);
	}
}

function repeatBrickUp(%val)
{
	if (%val == $brickUp)
	{
		commandToServer('shiftBrick', 0, 0, 3);
		if ($RecordingBuildMacro)
		{
			$BuildMacroSO.pushEvent("Server", 'shiftBrick', 0, 0, 3);
		}
		schedule($BrickRepeatTime, 0, repeatBrickUp, %val);
	}
}

function repeatBrickDown(%val)
{
	if (%val == $brickDown)
	{
		commandToServer('shiftBrick', 0, 0, -3);
		if ($RecordingBuildMacro)
		{
			$BuildMacroSO.pushEvent("Server", 'shiftBrick', 0, 0, -3);
		}
		schedule($BrickRepeatTime, 0, repeatBrickDown, %val);
	}
}

function repeatBrickThirdUp(%val)
{
	if (%val == $brickThirdUp)
	{
		commandToServer('shiftBrick', 0, 0, 1);
		if ($RecordingBuildMacro)
		{
			$BuildMacroSO.pushEvent("Server", 'shiftBrick', 0, 0, 1);
		}
		schedule($BrickRepeatTime, 0, repeatBrickThirdUp, %val);
	}
}

function repeatBrickThirdDown(%val)
{
	if (%val == $brickThirdDown)
	{
		commandToServer('shiftBrick', 0, 0, -1);
		if ($RecordingBuildMacro)
		{
			$BuildMacroSO.pushEvent("Server", 'shiftBrick', 0, 0, -1);
		}
		schedule($BrickRepeatTime, 0, repeatBrickThirdDown, %val);
	}
}

function repeatBrickPlant(%val)
{
	if (%val == $brickPlant)
	{
		commandToServer('plantBrick');
		if ($RecordingBuildMacro)
		{
			$BuildMacroSO.pushEvent("Server", 'plantBrick');
		}
		schedule($BrickRepeatTime, 0, repeatBrickPlant, %val);
	}
}

function shiftBrickAway(%val)
{
	if ($SuperShift)
	{
		superShiftBrickAway(%val);
		return;
	}
	$brickAway++;
	$brickAway = $brickAway % 1000;
	if (%val)
	{
		commandToServer('shiftBrick', 1, 0, 0);
		if ($RecordingBuildMacro)
		{
			$BuildMacroSO.pushEvent("Server", 'shiftBrick', 1, 0, 0);
		}
		schedule($BrickFirstRepeatTime, 0, repeatBrickAway, $brickAway);
	}
}

function shiftBrickTowards(%val)
{
	if ($SuperShift)
	{
		superShiftBrickTowards(%val);
		return;
	}
	$brickTowards++;
	$brickTowards = $brickTowards % 1000;
	if (%val)
	{
		commandToServer('shiftBrick', -1, 0, 0);
		if ($RecordingBuildMacro)
		{
			$BuildMacroSO.pushEvent("Server", 'shiftBrick', -1, 0, 0);
		}
		schedule($BrickFirstRepeatTime, 0, repeatBrickTowards, $brickTowards);
	}
}

function shiftBrickLeft(%val)
{
	if ($SuperShift)
	{
		superShiftBrickLeft(%val);
		return;
	}
	$brickLeft++;
	$brickLeft = $brickLeft % 1000;
	if (%val)
	{
		commandToServer('shiftBrick', 0, 1, 0);
		if ($RecordingBuildMacro)
		{
			$BuildMacroSO.pushEvent("Server", 'shiftBrick', 0, 1, 0);
		}
		schedule($BrickFirstRepeatTime, 0, repeatBrickLeft, $brickLeft);
	}
}

function shiftBrickRight(%val)
{
	if ($SuperShift)
	{
		superShiftBrickRight(%val);
		return;
	}
	$brickRight++;
	$brickRight = $brickRight % 1000;
	if (%val)
	{
		commandToServer('shiftBrick', 0, -1, 0);
		if ($RecordingBuildMacro)
		{
			$BuildMacroSO.pushEvent("Server", 'shiftBrick', 0, -1, 0);
		}
		schedule($BrickFirstRepeatTime, 0, repeatBrickRight, $brickRight);
	}
}

function shiftBrickUp(%val)
{
	if ($SuperShift)
	{
		superShiftBrickUp(%val);
		return;
	}
	$brickUp++;
	$brickUp = $brickUp % 1000;
	if (%val)
	{
		commandToServer('shiftBrick', 0, 0, 3);
		if ($RecordingBuildMacro)
		{
			$BuildMacroSO.pushEvent("Server", 'shiftBrick', 0, 0, 3);
		}
		schedule($BrickFirstRepeatTime, 0, repeatBrickUp, $brickUp);
	}
}

function shiftBrickDown(%val)
{
	if ($SuperShift)
	{
		superShiftBrickDown(%val);
		return;
	}
	$brickDown++;
	$brickDown = $brickDown % 1000;
	if (%val)
	{
		commandToServer('shiftBrick', 0, 0, -3);
		if ($RecordingBuildMacro)
		{
			$BuildMacroSO.pushEvent("Server", 'shiftBrick', 0, 0, -3);
		}
		schedule($BrickFirstRepeatTime, 0, repeatBrickDown, $brickDown);
	}
}

function shiftBrickThirdUp(%val)
{
	$brickThirdUp++;
	$brickThirdUp = $brickThirdUp % 1000;
	if (%val)
	{
		commandToServer('shiftBrick', 0, 0, 1);
		if ($RecordingBuildMacro)
		{
			$BuildMacroSO.pushEvent("Server", 'shiftBrick', 0, 0, 1);
		}
		schedule($BrickFirstRepeatTime, 0, repeatBrickThirdUp, $brickThirdUp);
	}
}

function shiftBrickThirdDown(%val)
{
	$brickThirdDown++;
	$brickThirdDown = $brickThirdDown % 1000;
	if (%val)
	{
		commandToServer('shiftBrick', 0, 0, -1);
		if ($RecordingBuildMacro)
		{
			$BuildMacroSO.pushEvent("Server", 'shiftBrick', 0, 0, -1);
		}
		schedule($BrickFirstRepeatTime, 0, repeatBrickThirdDown, $brickThirdDown);
	}
}

function RotateBrickCW(%val)
{
	if (%val)
	{
		commandToServer('rotateBrick', 1);
		if ($RecordingBuildMacro)
		{
			$BuildMacroSO.pushEvent("Server", 'rotateBrick', 1);
		}
	}
}

function RotateBrickCCW(%val)
{
	if (%val)
	{
		commandToServer('rotateBrick', -1);
		if ($RecordingBuildMacro)
		{
			$BuildMacroSO.pushEvent("Server", 'rotateBrick', -1);
		}
	}
}

function plantBrick(%val)
{
	$brickPlant++;
	$brickPlant = $brickPlant % 1000;
	if (%val)
	{
		commandToServer('plantBrick');
		if ($RecordingBuildMacro)
		{
			$BuildMacroSO.pushEvent("Server", 'plantBrick');
		}
		schedule($BrickFirstRepeatTime, 0, repeatBrickPlant, $brickPlant);
	}
}

function cancelBrick(%val)
{
	if (%val)
	{
		commandToServer('cancelBrick');
		if ($RecordingBuildMacro)
		{
			$BuildMacroSO.pushEvent("Server", 'cancelBrick');
		}
	}
}

function openAdminWindow(%val)
{
	if ($IamAdmin || isObject(ServerGroup))
	{
		if (%val)
		{
			if (adminGui.isAwake())
			{
				Canvas.popDialog(adminGui);
			}
			else
			{
				Canvas.popDialog(escapeMenu);
				Canvas.pushDialog(adminGui);
			}
		}
	}
	else
	{
		$AdminCallback = "canvas.pushDialog(admingui);";
		Canvas.pushDialog("adminLoginGui");
	}
}

function openOptionsWindow(%val)
{
	if (%val)
	{
		Canvas.pushDialog(optionsDlg);
	}
}

function useTools(%val)
{
	if (%val)
	{
		if ($ScrollMode != $SCROLLMODE_TOOLS)
		{
			if ($CurrScrollToolSlot <= 0)
			{
				$CurrScrollToolSlot = 0;
			}
			for (%i = 0; %i < $HUD_NumToolSlots; %i++)
			{
				%idx = (%i + $CurrScrollToolSlot) % $HUD_NumToolSlots;
				if ($ToolData[%idx] > 0)
				{
					$CurrScrollToolSlot = %idx;
					break;
				}
			}
			if (%i == $HUD_NumToolSlots)
			{
				return;
			}
			setScrollMode($SCROLLMODE_TOOLS);
			HUD_ToolActive.setVisible(True);
			setActiveTool($CurrScrollToolSlot);
			HUD_ToolName.setText(trim($ToolData[$CurrScrollToolSlot].uiName));
			commandToServer('UseTool', $CurrScrollToolSlot);
		}
		else
		{
			setScrollMode($SCROLLMODE_NONE);
		}
	}
}

function useBricks(%val)
{
	if (%val)
	{
		if ($BuildingDisabled)
		{
			clientCmdCenterPrint("\c5Building is currently disabled.", 2);
		}
		else if ($InvData[$CurrScrollBrickSlot] != -1)
		{
			directSelectInv($CurrScrollBrickSlot);
		}
		else
		{
			directSelectInv(0);
		}
	}
}

function useSprayCan(%val)
{
	if (%val)
	{
		if ($PaintingDisabled)
		{
			clientCmdCenterPrint("\c5Painting is currently disabled.", 2);
		}
		else
		{
			shiftPaintColumn(1);
		}
	}
}

function useLight(%val)
{
	if (%val)
	{
		commandToServer('Light');
	}
}

function showPlayerList(%val)
{
	if (%val)
	{
		NewPlayerListGui.toggle();
	}
}

function openBSD(%val)
{
	if (%val)
	{
		if ($BuildingDisabled)
		{
			clientCmdCenterPrint("\c5Building is currently disabled.", 2);
		}
		else if (BrickSelectorDlg.isAwake())
		{
			BSD_BuyBricks();
		}
		else
		{
			Canvas.pushDialog(BrickSelectorDlg);
		}
	}
}

$SCROLLMODE_BRICKS = 0;
$SCROLLMODE_PAINT = 1;
$SCROLLMODE_TOOLS = 2;
$SCROLLMODE_NONE = 3;
function scrollInventory(%val)
{
	if (Canvas.getCount() > 2)
	{
		return;
	}
	if (%val < 0)
	{
		%val = 1;
	}
	else
	{
		%val = -1;
	}
	if ($ZoomOn)
	{
		if (%val > 0)
		{
			if ($Pref::player::CurrentFOV > 5)
			{
				$Pref::player::CurrentFOV -= 5;
			}
		}
		else if ($Pref::player::CurrentFOV < 85)
		{
			$Pref::player::CurrentFOV += 5;
		}
		setFov($Pref::player::CurrentFOV);
		return;
	}
	if ($ScrollMode == $SCROLLMODE_BRICKS)
	{
		if (HUD_BrickActive.visible == 0)
		{
			directSelectInv($CurrScrollBrickSlot);
		}
		else
		{
			scrollBricks(%val);
		}
	}
	else if ($ScrollMode == $SCROLLMODE_PAINT)
	{
		scrollPaint(%val);
	}
	else if ($ScrollMode == $SCROLLMODE_TOOLS)
	{
		scrollTools(%val);
	}
	else if ($ScrollMode == $SCROLLMODE_NONE)
	{
		if (directSelectInv($CurrScrollBrickSlot))
		{
			setScrollMode($SCROLLMODE_BRICKS);
		}
	}
}

function scrollBricks(%direction)
{
	if ($pref::Input::ReverseBrickScroll)
	{
		%direction = %direction * -1;
	}
	%startScrollBrickSlot = $CurrScrollBrickSlot;
	if ($CurrScrollBrickSlot $= "")
	{
		if (%direction > 0)
		{
			$CurrScrollBrickSlot = -1;
		}
		else
		{
			$CurrScrollBrickSlot = 1;
		}
	}
	for (%i = 0; %i < $BSD_NumInventorySlots - 1; %i++)
	{
		$CurrScrollBrickSlot += %direction;
		if ($CurrScrollBrickSlot < 0)
		{
			$CurrScrollBrickSlot = $BSD_NumInventorySlots - 1;
		}
		if ($CurrScrollBrickSlot >= $BSD_NumInventorySlots)
		{
			$CurrScrollBrickSlot = 0;
		}
		if ($InvData[$CurrScrollBrickSlot] > 0)
		{
			break;
		}
	}
	if ($InvData[$CurrScrollBrickSlot] > 0)
	{
		if (%startScrollBrickSlot != $CurrScrollBrickSlot)
		{
			setActiveInv($CurrScrollBrickSlot);
			commandToServer('UseInventory', $CurrScrollBrickSlot);
			if ($RecordingBuildMacro)
			{
				$BuildMacroSO.pushEvent("Server", 'useInventory', $CurrScrollBrickSlot);
			}
		}
	}
}

function shiftPaintColumn(%direction)
{
	%canIndex = 0;
	if ($ScrollMode != $SCROLLMODE_PAINT)
	{
		setScrollMode($SCROLLMODE_PAINT);
		HUD_PaintBox.setVisible(1);
		HUD_PaintActive.setVisible(1);
		PlayGui.UnFadePaintRow($CurrPaintRow);
		%canIndex = 0;
	}
	else
	{
		PlayGui.FadePaintRow($CurrPaintRow);
		$CurrPaintRow += %direction;
		if ($CurrPaintRow >= $Paint_NumPaintRows)
		{
			$CurrPaintRow = 0;
		}
		if ($CurrPaintRow < 0)
		{
			$CurrPaintRow = $Paint_NumPaintRows - 1;
		}
		PlayGui.UnFadePaintRow($CurrPaintRow);
	}
	if ($CurrPaintRow == $Paint_NumPaintRows - 1)
	{
		if ($CurrPaintSwatch > 8)
		{
			$CurrPaintSwatch = 8;
		}
		if ($CurrPaintSwatch == 0)
		{
			HUD_PaintName.setText("FX - None");
		}
		else if ($CurrPaintSwatch == 1)
		{
			HUD_PaintName.setText("FX - Pearl");
		}
		else if ($CurrPaintSwatch == 2)
		{
			HUD_PaintName.setText("FX - Chrome");
		}
		else if ($CurrPaintSwatch == 3)
		{
			HUD_PaintName.setText("FX - Glow");
		}
		else if ($CurrPaintSwatch == 4)
		{
			HUD_PaintName.setText("FX - Blink");
		}
		else if ($CurrPaintSwatch == 5)
		{
			HUD_PaintName.setText("FX - Swirl");
		}
		else if ($CurrPaintSwatch == 6)
		{
			HUD_PaintName.setText("FX - Rainbow");
		}
		else if ($CurrPaintSwatch == 7)
		{
			HUD_PaintName.setText("FX - Stable");
		}
		else if ($CurrPaintSwatch == 8)
		{
			HUD_PaintName.setText("FX - Undulo");
		}
		commandToServer('useFXCan', $CurrPaintSwatch);
	}
	else
	{
		%numSwatches = $Paint_Row[$CurrPaintRow].numSwatches;
		if ($CurrPaintSwatch >= %numSwatches)
		{
			$CurrPaintSwatch = %numSwatches - 1;
		}
		HUD_PaintName.setText(getSprayCanDivisionName($CurrPaintRow) SPC "-" SPC $CurrPaintSwatch + 1);
		for (%i = 0; %i < $CurrPaintRow; %i++)
		{
			%canIndex += $Paint_Row[%i].numSwatches;
		}
		%canIndex += $CurrPaintSwatch;
		$currSprayCanIndex = %canIndex;
		commandToServer('useSprayCan', %canIndex);
		if ($RecordingBuildMacro)
		{
			$BuildMacroSO.pushEvent("Server", 'useSprayCan', %canIndex);
		}
	}
	PlayGui.updatePaintActive();
}

function scrollPaint(%direction)
{
	%numSwatches = $Paint_Row[$CurrPaintRow].numSwatches;
	$CurrPaintSwatch += %direction;
	if ($CurrPaintSwatch < 0)
	{
		$CurrPaintSwatch = %numSwatches - 1;
	}
	if ($CurrPaintSwatch >= %numSwatches)
	{
		$CurrPaintSwatch = 0;
	}
	PlayGui.updatePaintActive();
	if ($CurrPaintRow == $Paint_NumPaintRows - 1)
	{
		if ($CurrPaintSwatch == 0)
		{
			HUD_PaintName.setText("FX - None");
		}
		else if ($CurrPaintSwatch == 1)
		{
			HUD_PaintName.setText("FX - Pearl");
		}
		else if ($CurrPaintSwatch == 2)
		{
			HUD_PaintName.setText("FX - Chrome");
		}
		else if ($CurrPaintSwatch == 3)
		{
			HUD_PaintName.setText("FX - Glow");
		}
		else if ($CurrPaintSwatch == 4)
		{
			HUD_PaintName.setText("FX - Blink");
		}
		else if ($CurrPaintSwatch == 5)
		{
			HUD_PaintName.setText("FX - Swirl");
		}
		else if ($CurrPaintSwatch == 6)
		{
			HUD_PaintName.setText("FX - Rainbow");
		}
		else if ($CurrPaintSwatch == 7)
		{
			HUD_PaintName.setText("FX - Stable");
		}
		else if ($CurrPaintSwatch == 8)
		{
			HUD_PaintName.setText("FX - Undulo");
		}
		commandToServer('useFXCan', $CurrPaintSwatch);
	}
	else
	{
		HUD_PaintName.setText(getSprayCanDivisionName($CurrPaintRow) SPC "-" SPC $CurrPaintSwatch + 1);
		%canIndex = 0;
		for (%i = 0; %i < $CurrPaintRow; %i++)
		{
			%canIndex += $Paint_Row[%i].numSwatches;
		}
		%canIndex += $CurrPaintSwatch;
		$currSprayCanIndex = %canIndex;
		commandToServer('useSprayCan', %canIndex);
		if ($RecordingBuildMacro)
		{
			$BuildMacroSO.pushEvent("Server", 'useSprayCan', %canIndex);
		}
	}
}

function setActiveTool(%index)
{
	if (%index < 0)
	{
		HUD_ToolActive.setVisible(0);
		HUD_ToolName.setText("");
		return;
	}
	HUD_ToolActive.setVisible(1);
	%x = 0;
	%y = 64 * %index;
	%w = 64;
	%h = 64;
	HUD_ToolActive.resize(%x, %y, %w, %h);
	HUD_ToolName.setText(trim($ToolData[%index].uiName));
}

function scrollTools(%direction)
{
	if ($CurrScrollToolSlot $= "")
	{
		if (%direction > 0)
		{
			$CurrScrollToolSlot = -1;
		}
		else
		{
			$CurrScrollToolSlot = 1;
		}
	}
	for (%i = 0; %i < $HUD_NumToolSlots; %i++)
	{
		$CurrScrollToolSlot += %direction;
		if ($CurrScrollToolSlot < 0)
		{
			$CurrScrollToolSlot = $HUD_NumToolSlots - 1;
		}
		else if ($CurrScrollToolSlot >= $HUD_NumToolSlots)
		{
			$CurrScrollToolSlot = 0;
		}
		if ($ToolData[$CurrScrollToolSlot] > 0)
		{
			break;
		}
	}
	if ($ToolData[$CurrScrollToolSlot] > 0)
	{
		setActiveTool($CurrScrollToolSlot);
		commandToServer('UseTool', $CurrScrollToolSlot);
	}
	else
	{
		setActiveTool(-1);
	}
}

function setScrollMode(%newMode)
{
	if ($ScrollMode == %newMode)
	{
		return 0;
	}
	if ($ScrollMode == $SCROLLMODE_BRICKS)
	{
		setActiveInv(-1);
		HUD_BrickName.setText("");
		if ($pref::HUD::HideBrickBox)
		{
			if ($pref::HUD::showToolTips)
			{
				PlayGui.hideBrickBox(64, 10, 0);
			}
			else
			{
				PlayGui.hideBrickBox(87, 10, 0);
			}
		}
	}
	else if ($ScrollMode == $SCROLLMODE_PAINT)
	{
		PlayGui.FadePaintRow($CurrPaintRow);
		HUD_PaintActive.setVisible(0);
		HUD_PaintName.setText("");
		if ($pref::HUD::HidePaintBox)
		{
			if ($pref::HUD::showToolTips)
			{
				PlayGui.hidePaintBox((getWord(HUD_PaintBox.extent, 0) - 100) + 5, 10, 0);
			}
			else
			{
				PlayGui.hidePaintBox(getWord(HUD_PaintBox.extent, 0) + 5, 10, 0);
			}
		}
		if ($pref::HUD::showToolTips)
		{
			ToolTip_Paint.setVisible(1);
		}
		if ($InstantUse)
		{
			$InstantUse = 0;
		}
		else
		{
			commandToServer('UnUseTool');
		}
	}
	else if ($ScrollMode == $SCROLLMODE_TOOLS)
	{
		HUD_ToolActive.setVisible(0);
		HUD_ToolName.setText("");
		if ($pref::HUD::HideToolBox)
		{
			if ($pref::HUD::showToolTips)
			{
				PlayGui.hideToolBox($HUD_NumToolSlots * 64, 20, 0);
			}
			else
			{
				PlayGui.hideToolBox($HUD_NumToolSlots * 64 + 25, 20, 0);
			}
		}
		if ($pref::HUD::showToolTips)
		{
			ToolTip_Tools.setVisible(1);
		}
		if ($InstantUse)
		{
			$InstantUse = 0;
		}
		else
		{
			commandToServer('UnUseTool');
		}
	}
	$ScrollMode = %newMode;
	if ($ScrollMode == $SCROLLMODE_BRICKS)
	{
		if ($pref::HUD::HideBrickBox)
		{
			if ($pref::HUD::showToolTips)
			{
				PlayGui.hideBrickBox(-64, 10, 0);
			}
			else
			{
				PlayGui.hideBrickBox(-87, 10, 0);
			}
		}
	}
	else if ($ScrollMode == $SCROLLMODE_PAINT)
	{
		if ($pref::HUD::HidePaintBox)
		{
			if ($pref::HUD::showToolTips)
			{
				PlayGui.hidePaintBox((-1 * getWord(HUD_PaintBox.extent, 0) + 100) - 5, 1, 0);
			}
			else
			{
				PlayGui.hidePaintBox(-1 * getWord(HUD_PaintBox.extent, 0) - 5, 1, 0);
			}
		}
	}
	else if ($ScrollMode == $SCROLLMODE_TOOLS)
	{
		if ($pref::HUD::HideToolBox)
		{
			if ($pref::HUD::showToolTips)
			{
				PlayGui.hideToolBox(-$HUD_NumToolSlots * 64, 10, 0);
			}
			else
			{
				PlayGui.hideToolBox(-$HUD_NumToolSlots * 64 - 25, 10, 0);
			}
		}
		ToolTip_Tools.setVisible(0);
	}
	$InstantUse = 0;
	return 1;
}

function superRepeatBrickAway(%val)
{
	if (%val == $superBrickAway)
	{
		commandToServer('superShiftBrick', 1, 0, 0);
		if ($RecordingBuildMacro)
		{
			$BuildMacroSO.pushEvent("Server", 'superShiftBrick', 1, 0, 0);
		}
		schedule($BrickRepeatTime, 0, superRepeatBrickAway, %val);
	}
}

function superRepeatBrickTowards(%val)
{
	if (%val == $superBrickTowards)
	{
		commandToServer('superShiftBrick', -1, 0, 0);
		if ($RecordingBuildMacro)
		{
			$BuildMacroSO.pushEvent("Server", 'superShiftBrick', -1, 0, 0);
		}
		schedule($BrickRepeatTime, 0, superRepeatBrickTowards, %val);
	}
}

function superRepeatBrickLeft(%val)
{
	if (%val == $superBrickLeft)
	{
		commandToServer('superShiftBrick', 0, 1, 0);
		if ($RecordingBuildMacro)
		{
			$BuildMacroSO.pushEvent("Server", 'superShiftBrick', 0, 1, 0);
		}
		schedule($BrickRepeatTime, 0, superRepeatBrickLeft, %val);
	}
}

function superRepeatBrickRight(%val)
{
	if (%val == $superBrickRight)
	{
		commandToServer('superShiftBrick', 0, -1, 0);
		if ($RecordingBuildMacro)
		{
			$BuildMacroSO.pushEvent("Server", 'superShiftBrick', 0, -1, 0);
		}
		schedule($BrickRepeatTime, 0, superRepeatBrickRight, %val);
	}
}

function superRepeatBrickUp(%val)
{
	if (%val == $superBrickUp)
	{
		commandToServer('superShiftBrick', 0, 0, 1);
		if ($RecordingBuildMacro)
		{
			$BuildMacroSO.pushEvent("Server", 'superShiftBrick', 0, 0, 1);
		}
		schedule($BrickRepeatTime, 0, superRepeatBrickUp, %val);
	}
}

function superRepeatBrickDown(%val)
{
	if (%val == $superBrickDown)
	{
		commandToServer('superShiftBrick', 0, 0, -1);
		if ($RecordingBuildMacro)
		{
			$BuildMacroSO.pushEvent("Server", 'superShiftBrick', 0, 0, -1);
		}
		schedule($BrickRepeatTime, 0, superRepeatBrickDown, %val);
	}
}

function superShiftBrickAwayProxy(%val)
{
	if ($pref::Input::UseSuperShiftToggle && $pref::Input::UseSuperShiftSmartToggle)
	{
		shiftBrickAway(%val);
	}
	else
	{
		superShiftBrickAway(%val);
	}
}

function superShiftBrickTowardsProxy(%val)
{
	if ($pref::Input::UseSuperShiftToggle && $pref::Input::UseSuperShiftSmartToggle)
	{
		shiftBrickTowards(%val);
	}
	else
	{
		superShiftBrickTowards(%val);
	}
}

function superShiftBrickLeftProxy(%val)
{
	if ($pref::Input::UseSuperShiftToggle && $pref::Input::UseSuperShiftSmartToggle)
	{
		shiftBrickLeft(%val);
	}
	else
	{
		superShiftBrickLeft(%val);
	}
}

function superShiftBrickRightProxy(%val)
{
	if ($pref::Input::UseSuperShiftToggle && $pref::Input::UseSuperShiftSmartToggle)
	{
		shiftBrickRight(%val);
	}
	else
	{
		superShiftBrickRight(%val);
	}
}

function superShiftBrickUpProxy(%val)
{
	if ($pref::Input::UseSuperShiftToggle && $pref::Input::UseSuperShiftSmartToggle)
	{
		shiftBrickUp(%val);
	}
	else
	{
		superShiftBrickUp(%val);
	}
}

function superShiftBrickDownProxy(%val)
{
	if ($pref::Input::UseSuperShiftToggle && $pref::Input::UseSuperShiftSmartToggle)
	{
		shiftBrickDown(%val);
	}
	else
	{
		superShiftBrickDown(%val);
	}
}

function superShiftBrickAway(%val)
{
	$superBrickAway++;
	$superBrickAway = $superBrickAway % 1000;
	if (%val)
	{
		commandToServer('superShiftBrick', 1, 0, 0);
		if ($RecordingBuildMacro)
		{
			$BuildMacroSO.pushEvent("Server", 'superShiftBrick', 1, 0, 0);
		}
		schedule($BrickFirstRepeatTime, 0, superRepeatBrickAway, $superBrickAway);
	}
}

function superShiftBrickTowards(%val)
{
	$superBrickTowards++;
	$superBrickTowards = $superBrickTowards % 1000;
	if (%val)
	{
		commandToServer('superShiftBrick', -1, 0, 0);
		if ($RecordingBuildMacro)
		{
			$BuildMacroSO.pushEvent("Server", 'superShiftBrick', -1, 0, 0);
		}
		schedule($BrickFirstRepeatTime, 0, superRepeatBrickTowards, $superBrickTowards);
	}
}

function superShiftBrickLeft(%val)
{
	$superBrickLeft++;
	$superBrickLeft = $superBrickLeft % 1000;
	if (%val)
	{
		commandToServer('superShiftBrick', 0, 1, 0);
		if ($RecordingBuildMacro)
		{
			$BuildMacroSO.pushEvent("Server", 'superShiftBrick', 0, 1, 0);
		}
		schedule($BrickFirstRepeatTime, 0, superRepeatBrickLeft, $superBrickLeft);
	}
}

function superShiftBrickRight(%val)
{
	$superBrickRight++;
	$superBrickRight = $superBrickRight % 1000;
	if (%val)
	{
		commandToServer('superShiftBrick', 0, -1, 0);
		if ($RecordingBuildMacro)
		{
			$BuildMacroSO.pushEvent("Server", 'superShiftBrick', 0, -1, 0);
		}
		schedule($BrickFirstRepeatTime, 0, superRepeatBrickRight, $superBrickRight);
	}
}

function superShiftBrickUp(%val)
{
	$superBrickUp++;
	$superBrickUp = $superBrickUp % 1000;
	if (%val)
	{
		commandToServer('superShiftBrick', 0, 0, 1);
		if ($RecordingBuildMacro)
		{
			$BuildMacroSO.pushEvent("Server", 'superShiftBrick', 0, 0, 1);
		}
		schedule($BrickFirstRepeatTime, 0, superRepeatBrickUp, $superBrickUp);
	}
}

function superShiftBrickDown(%val)
{
	$superBrickDown++;
	$superBrickDown = $superBrickDown % 1000;
	if (%val)
	{
		commandToServer('superShiftBrick', 0, 0, -1);
		if ($RecordingBuildMacro)
		{
			$BuildMacroSO.pushEvent("Server", 'superShiftBrick', 0, 0, -1);
		}
		schedule($BrickFirstRepeatTime, 0, superRepeatBrickDown, $superBrickDown);
	}
}

function toggleSuperShift(%val)
{
	%minTime = 200;
	if ($pref::Input::UseSuperShiftToggle == 1)
	{
		if (%val)
		{
			$lastSuperShiftToggleTime = getSimTime();
			$SuperShift = !$SuperShift;
			HUD_SuperShift.setVisible($SuperShift);
			$brickAway++;
			$brickTowards++;
			$brickLeft++;
			$brickRight++;
			$brickUp++;
			$brickDown++;
			$brickThirdUp++;
			$brickThirdDown++;
			$brickPlant++;
			$superBrickAway++;
			$superBrickTowards++;
			$superBrickLeft++;
			$superBrickRight++;
			$superBrickUp++;
			$superBrickDown++;
		}
		else if ($pref::Input::UseSuperShiftSmartToggle)
		{
			%time = getSimTime() - $lastSuperShiftToggleTime;
			if (%time > %minTime)
			{
				$SuperShift = !$SuperShift;
				HUD_SuperShift.setVisible($SuperShift);
				$brickAway++;
				$brickTowards++;
				$brickLeft++;
				$brickRight++;
				$brickUp++;
				$brickDown++;
				$brickThirdUp++;
				$brickThirdDown++;
				$brickPlant++;
				$superBrickAway++;
				$superBrickTowards++;
				$superBrickLeft++;
				$superBrickRight++;
				$superBrickUp++;
				$superBrickDown++;
			}
		}
	}
}

function dropTool(%val)
{
	if (%val)
	{
		commandToServer('DropTool', $CurrScrollToolSlot);
	}
}

function undoBrick(%val)
{
	if (%val)
	{
		commandToServer('UndoBrick');
		if ($RecordingBuildMacro)
		{
			$BuildMacroSO.pushEvent("Server", 'UndoBrick');
		}
	}
}

function PageUpNewChatHud(%val)
{
	$chatScroll++;
	$chatScroll = $chatScroll % 1000;
	if (%val)
	{
		if (isObject($NewChatSO))
		{
			$NewChatSO.pageUp();
		}
		schedule(200, 0, repeatPageUpNewChatHud, $chatScroll);
	}
}

function repeatPageUpNewChatHud(%val)
{
	if (%val == $chatScroll)
	{
		if (isObject($NewChatSO))
		{
			$NewChatSO.pageUp();
		}
		schedule(50, 0, repeatPageUpNewChatHud, %val);
	}
}

function PageDownNewChatHud(%val)
{
	$chatScroll++;
	$chatScroll = $chatScroll % 1000;
	if (%val)
	{
		if (isObject($NewChatSO))
		{
			$NewChatSO.pageDown();
		}
		schedule(200, 0, repeatPageDownNewChatHud, $chatScroll);
	}
}

function repeatPageDownNewChatHud(%val)
{
	if (%val == $chatScroll)
	{
		if (isObject($NewChatSO))
		{
			$NewChatSO.pageDown();
		}
		schedule(50, 0, repeatPageDownNewChatHud, %val);
	}
}

function GlobalChat(%val)
{
	if (%val)
	{
		newMessageHud.open("SAY");
	}
}

function TeamChat(%val)
{
	if (%val)
	{
		newMessageHud.open("TEAM");
	}
}

function ToggleCursor(%val)
{
	if (%val)
	{
		if (Canvas.isCursorOn())
		{
			Canvas.cursorOff();
			$NewChatSO.update();
		}
		else
		{
			if ($pref::HUD::showToolTips)
			{
				MouseToolTip.setVisible(1);
				%key = strupr(getWord(moveMap.getBinding("toggleCursor"), 1));
				MouseToolTip.setValue("\c6TIP: Press " @ %key @ " to toggle mouse and click on links");
				%w = getWord(MouseToolTip.getExtent(), 0);
				%h = getWord(MouseToolTip.getExtent(), 1);
				%x = getWord(MouseToolTip.getPosition(), 0);
				%y = getWord($NewChatSO.textObj.getPosition(), 1) + getWord($NewChatSO.textObj.getExtent(), 1) + %h;
				MouseToolTip.resize(%x, %y, %w, %h);
			}
			Canvas.cursorOn();
		}
	}
}

$Net_PacketSize[1] = 100;
$Net_RateToClient[1] = 10;
$Net_RateToServer[1] = 8;
$Net_LagThreshold[1] = 400;
$Net_PacketSize[2] = 200;
$Net_RateToClient[2] = 12;
$Net_RateToServer[2] = 16;
$Net_LagThreshold[2] = 400;
$Net_PacketSize[3] = 240;
$Net_RateToClient[3] = 16;
$Net_RateToServer[3] = 20;
$Net_LagThreshold[3] = 400;
$Net_PacketSize[4] = 350;
$Net_RateToClient[4] = 20;
$Net_RateToServer[4] = 24;
$Net_LagThreshold[4] = 400;
$Net_PacketSize[5] = 400;
$Net_RateToClient[5] = 24;
$Net_RateToServer[5] = 24;
$Net_LagThreshold[5] = 400;
$Net_PacketSize[6] = 450;
$Net_RateToClient[6] = 32;
$Net_RateToServer[6] = 32;
$Net_LagThreshold[6] = 400;
function SetConnectionType(%val)
{
	if (%val == 0)
	{
		return;
	}
	$Pref::Net::ConnectionType = %val;
	if (%val <= 6)
	{
		CustomNetworkBlocker.setVisible(1);
		$pref::Net::PacketSize = $Net_PacketSize[%val];
		$pref::Net::PacketRateToClient = $Net_RateToClient[%val];
		$pref::Net::PacketRateToServer = $Net_RateToServer[%val];
		$Pref::Net::LagThreshold = $Net_LagThreshold[%val];
		PacketSizeDisplay.setValue($pref::Net::PacketSize);
		SliderPacketSize.setValue($pref::Net::PacketSize);
		LagThresholdDisplay.setValue($Pref::Net::LagThreshold);
		SliderLagThreshold.setValue($Pref::Net::LagThreshold);
		RateToClientDisplay.setValue($pref::Net::PacketRateToClient);
		SliderRateToClient.setValue($pref::Net::PacketRateToClient);
		RateToServerDisplay.setValue($pref::Net::PacketRateToServer);
		SliderRateToServer.setValue($pref::Net::PacketRateToServer);
	}
	else
	{
		CustomNetworkBlocker.setVisible(0);
		PacketSizeDisplay.setValue($pref::Net::Custom::PacketSize);
		SliderPacketSize.setValue($pref::Net::Custom::PacketSize);
		LagThresholdDisplay.setValue($pref::Net::Custom::LagThreshold);
		SliderLagThreshold.setValue($pref::Net::Custom::LagThreshold);
		RateToClientDisplay.setValue($pref::Net::Custom::PacketRateToClient);
		SliderRateToClient.setValue($pref::Net::Custom::PacketRateToClient);
		RateToServerDisplay.setValue($pref::Net::Custom::PacketRateToServer);
		SliderRateToServer.setValue($pref::Net::Custom::PacketRateToServer);
		UpdatePacketSize();
		UpdateLagThreshold();
		UpdateRateToClient();
		UpdateRateToServer();
	}
}

function UpdatePacketSize()
{
	PacketSizeDisplay.setValue(mFloor(SliderPacketSize.getValue()));
	$pref::Net::PacketSize = PacketSizeDisplay.getValue();
	if ($Pref::Net::ConnectionType == 7)
	{
		$pref::Net::Custom::PacketSize = $pref::Net::PacketSize;
	}
}

function UpdateLagThreshold()
{
	LagThresholdDisplay.setValue(mFloor(SliderLagThreshold.getValue()));
	$Pref::Net::LagThreshold = LagThresholdDisplay.getValue();
	if ($Pref::Net::ConnectionType == 7)
	{
		$pref::Net::Custom::LagThreshold = $Pref::Net::LagThreshold;
	}
}

function UpdateRateToClient()
{
	RateToClientDisplay.setValue(mFloor(SliderRateToClient.getValue()));
	$pref::Net::PacketRateToClient = RateToClientDisplay.getValue();
	if ($Pref::Net::ConnectionType == 7)
	{
		$pref::Net::Custom::PacketRateToClient = $pref::Net::PacketRateToClient;
	}
}

function UpdateRateToServer()
{
	RateToServerDisplay.setValue(mFloor(SliderRateToServer.getValue()));
	$pref::Net::PacketRateToServer = RateToServerDisplay.getValue();
	if ($Pref::Net::ConnectionType == 7)
	{
		$pref::Net::Custom::PacketRateToServer = $pref::Net::PacketRateToServer;
	}
}

function OPT_MaxLightsDrop()
{
	SliderMaxLights.setValue(mFloor(SliderMaxLights.getValue()));
}

function OPT_DynLightsDrop()
{
	SliderDynLights.setValue(mFloor(SliderDynLights.getValue()));
}

function OPT_ChatSizeDrop()
{
	SliderChatFontSize.setValue(mFloor(SliderChatFontSize.getValue()));
	if (isObject($NewChatSO))
	{
		$NewChatSO.update();
	}
}

function OPT_ChatSizeSlide()
{
	if (isObject($NewChatSO))
	{
		$NewChatSO.update();
	}
}

function OPT_SetChatSize(%val)
{
	%maxHeight[0] = 16;
	%maxHeight[1] = 18;
	%maxHeight[2] = 20;
	%maxHeight[3] = 22;
	%maxHeight[4] = 24;
	%maxHeight[5] = 26;
	%maxHeight[6] = 28;
	%maxHeight[7] = 30;
	%maxHeight[8] = 32;
	%maxHeight[9] = 34;
	%maxHeight[10] = 36;
	if (%val >= 0 && %val <= 10)
	{
		$Pref::Gui::ChatSize = %val;
		$NewChatSO.textObj.maxBitmapHeight = %maxHeight[%val];
		$NewChatSO.textObj.setProfile("BlockChatTextSize" @ %val @ "Profile");
		NMH_Type.setProfile("HUDChatTextEditSize" @ %val @ "Profile");
		NMH_Channel.setProfile("BlockChatChannelSize" @ %val @ "Profile");
		$NewChatSO.update();
	}
}

function invUp(%val)
{
	if (%val)
	{
		if ($ScrollMode == $SCROLLMODE_BRICKS)
		{
			scrollInventory(1);
		}
		else if ($ScrollMode == $SCROLLMODE_PAINT)
		{
			scrollInventory(1);
		}
		else if ($ScrollMode == $SCROLLMODE_TOOLS)
		{
			scrollInventory(1);
		}
		else if ($ScrollMode == $SCROLLMODE_NONE)
		{
			setScrollMode($SCROLLMODE_BRICKS);
			scrollInventory(1);
		}
	}
}

function invDown(%val)
{
	if (%val)
	{
		if ($ScrollMode == $SCROLLMODE_BRICKS)
		{
			scrollInventory(-1);
		}
		else if ($ScrollMode == $SCROLLMODE_PAINT)
		{
			scrollInventory(-1);
		}
		else if ($ScrollMode == $SCROLLMODE_TOOLS)
		{
			scrollInventory(-1);
		}
		else if ($ScrollMode == $SCROLLMODE_NONE)
		{
			setScrollMode($SCROLLMODE_BRICKS);
			scrollInventory(-1);
		}
	}
}

function invLeft(%val)
{
	if (%val)
	{
		if ($ScrollMode == $SCROLLMODE_BRICKS)
		{
			scrollInventory(1);
		}
		else if ($ScrollMode == $SCROLLMODE_PAINT)
		{
			shiftPaintColumn(-1);
		}
		else if ($ScrollMode == $SCROLLMODE_TOOLS)
		{
			scrollInventory(1);
		}
		else if ($ScrollMode == $SCROLLMODE_NONE)
		{
			setScrollMode($SCROLLMODE_BRICKS);
			scrollInventory(1);
		}
	}
}

function invRight(%val)
{
	if (%val)
	{
		if ($ScrollMode == $SCROLLMODE_BRICKS)
		{
			scrollInventory(-1);
		}
		else if ($ScrollMode == $SCROLLMODE_PAINT)
		{
			shiftPaintColumn(1);
		}
		else if ($ScrollMode == $SCROLLMODE_TOOLS)
		{
			scrollInventory(-1);
		}
		else if ($ScrollMode == $SCROLLMODE_NONE)
		{
			setScrollMode($SCROLLMODE_BRICKS);
			scrollInventory(-1);
		}
	}
}

function optionsDlg::setTextureQuality(%this, %val)
{
	if (%val < 0)
	{
		%val = 0;
	}
	if (%val > 5)
	{
		%val = 5;
	}
	$pref::TextureQuality = %val;
	$pref::Terrain::texDetail = %val;
	$pref::Interior::TexDetail = %val;
	$pref::OpenGL::texDetail = %val;
	$pref::OpenGL::skyTexDetail = %val;
}

function optionsDlg::setParticleQuality(%this, %val)
{
	if (%val < 0)
	{
		%val = 0;
	}
	if (%val > 5)
	{
		%val = 5;
	}
	$pref::ParticleQuality = %val;
	if (%val == 0)
	{
		$pref::ParticleDetail = 1;
	}
	else if (%val == 1)
	{
		$pref::ParticleDetail = 2;
	}
	else if (%val == 2)
	{
		$pref::ParticleDetail = 5;
	}
	else if (%val == 3)
	{
		$pref::ParticleDetail = 10;
	}
	else if (%val == 4)
	{
		$pref::ParticleDetail = 15;
	}
}

function optionsDlg::setShadowQuality(%this, %val)
{
	if (%val < 0)
	{
		%val = 0;
	}
	if (%val > 4)
	{
		%val = 4;
	}
	$pref::ShadowQuality = %val;
	setShadowResolution(%val);
}

function toggleNetGraph(%val)
{
	if (%val)
	{
		NetGraph::toggleNetGraph();
	}
}

function Suicide(%val)
{
	if (%val)
	{
		commandToServer('Suicide');
	}
}

function ToggleShapeNameHud(%val)
{
	if (%val)
	{
		if (PlayGui_ShapeNameHud.isVisible())
		{
			PlayGui_ShapeNameHud.setVisible(0);
			NoHudGui_ShapeNameHud.setVisible(0);
			Crosshair.setVisible(0);
		}
		else
		{
			PlayGui_ShapeNameHud.setVisible(1);
			NoHudGui_ShapeNameHud.setVisible(1);
			Crosshair.setVisible(1);
		}
	}
}

function emoteSit(%val)
{
	if (%val)
	{
		commandToServer('Sit');
	}
}

function emoteLove(%val)
{
	if (%val)
	{
		commandToServer('Love');
	}
}

function emoteHate(%val)
{
	if (%val)
	{
		commandToServer('Hate');
	}
}

function emoteConfusion(%val)
{
	if (%val)
	{
		commandToServer('Confusion');
	}
}

function emoteAlarm(%val)
{
	if (%val)
	{
		commandToServer('Alarm');
	}
}

function NextSeat(%val)
{
	if (%val)
	{
		commandToServer('NextSeat');
	}
}

function PrevSeat(%val)
{
	if (%val)
	{
		commandToServer('PrevSeat');
	}
}

function PlayGui::onWake(%this)
{
	$enableDirectInput = 1;
	activateDirectInput();
	Canvas.pushDialog(NewChatHud);
	moveMap.push();
	schedule(0, 0, "refreshCenterTextCtrl");
	schedule(0, 0, "refreshBottomTextCtrl");
	HUD_SuperShift.setVisible($SuperShift);
	if (isUnlocked())
	{
		Demo_BrickCountBox.setVisible(0);
	}
	else if (isObject(ServerGroup))
	{
		Demo_BrickCountBox.setVisible(1);
	}
	else
	{
		Demo_BrickCountBox.setVisible(0);
	}
}

function PlayGui::onSleep(%this)
{
	Canvas.popDialog(NewChatHud);
	moveMap.pop();
}

function refreshBottomTextCtrl()
{
	BottomPrintText.position = "10 0";
}

function refreshCenterTextCtrl()
{
	CenterPrintText.position = "0 0";
}

function PlayGui::killInvHud(%this)
{
	if (isEventPending(HUD_BrickBox.moveSchedule))
	{
		cancel(HUD_BrickBox.moveSchedule);
	}
	if (isObject(HUD_BrickBox))
	{
		HUD_BrickBox.delete();
	}
	if (isObject(HUD_BrickNameBG))
	{
		HUD_BrickNameBG.delete();
	}
	if (isObject(HUD_BrickName))
	{
		HUD_BrickName.delete();
	}
}

function PlayGui::createInvHud(%this)
{
	%this.killInvHud();
	%numSlots = $BSD_NumInventorySlots;
	%res = getRes();
	%screenWidth = getWord(%res, 0);
	%screenHeight = getWord(%res, 1);
	%iconWidth = mFloor(%screenWidth / $BSD_NumInventorySlots);
	if (%iconWidth > 64)
	{
		%iconWidth = 64;
	}
	%bottomSpace = 0;
	%iconBoxWidth = $BSD_NumInventorySlots * %iconWidth;
	%newBox = new GuiSwatchCtrl();
	%newBox.setName("Hud_BrickBox");
	PlayGui.add(%newBox);
	%newBox.setProfile(HUDBitmapProfile);
	%newBox.setColor("0 0 0 0.25");
	%x = %screenWidth / 2 - %iconBoxWidth / 2;
	%y = (%screenHeight - %iconWidth * 1) - %bottomSpace;
	%w = %iconBoxWidth;
	%h = %iconWidth;
	%newBox.resize(%x, %y, %w, %h);
	%newBox.horizSizing = "Center";
	%newActive = new GuiBitmapCtrl();
	HUD_BrickBox.add(%newActive);
	%newActive.setProfile(HUDBitmapProfile);
	%newActive.setBitmap("base/client/ui/brickActive");
	%newActive.setBitmap("base/client/ui/brickIcons/brickIconActive");
	%newActive.setVisible(0);
	%x = 0;
	%y = 0;
	%w = %iconWidth;
	%h = %iconWidth;
	%newActive.resize(%x, %y, %w, %h);
	%newActive.setName("HUD_BrickActive");
	for (%i = 0; %i < %numSlots; %i++)
	{
		%newSwatch = new GuiSwatchCtrl();
		HUD_BrickBox.add(%newSwatch);
		%newSwatch.setProfile(HUDBitmapProfile);
		%newSwatch.setColor("0 0 0 0.25");
		%x = %i * %iconWidth + 2;
		%y = 0 + 4;
		%w = %iconWidth - 4;
		%h = %iconWidth - 8;
		%newSwatch.resize(%x, %y, %w, %h);
		%newIcon = new GuiBitmapCtrl();
		HUD_BrickBox.add(%newIcon);
		%newIcon.setProfile(HUDBitmapProfile);
		if (isObject($InvData[%i]))
		{
			if (!isFile($InvData[%i].iconName @ ".png"))
			{
				%newIcon.setBitmap("base/client/ui/brickIcons/unknown.png");
				continue;
			}
			%newIcon.setBitmap($InvData[%i].iconName);
		}
		%x = %i * %iconWidth;
		%y = 0;
		%w = %iconWidth;
		%h = %iconWidth;
		%newIcon.resize(%x, %y, %w, %h);
		$HUD_BrickIcon[%i] = %newIcon;
		if ($pref::Gui::ShowBrickSlotNumbers)
		{
			%newText = new GuiTextCtrl();
			HUD_BrickBox.add(%newText);
			%newText.setProfile(HUDBrickNameProfile);
			%newText.setText((%i + 1) % 10);
			%x = %i * %iconWidth;
			%y = 2;
			%w = 16;
			%h = 18;
			%newText.resize(%x, %y, %w, %h);
		}
	}
	%newSwatch = new GuiSwatchCtrl();
	PlayGui.add(%newSwatch);
	%newSwatch.setProfile(HUDBrickNameProfile);
	%newSwatch.setColor("0.0 0.0 0.5 0.0");
	%w = %iconBoxWidth;
	%h = 18;
	%x = %screenWidth / 2 - %iconBoxWidth / 2;
	%y = ((%screenHeight - %iconWidth * 1) - %h) - %bottomSpace;
	%newSwatch.resize(%x, %y, %w, %h);
	%newSwatch.setName("HUD_BrickNameBG");
	%newCorner = new GuiBitmapCtrl();
	HUD_BrickNameBG.add(%newCorner);
	%newCorner.setProfile(HUDBitmapProfile);
	%newCorner.setBitmap("base/client/ui/BlueHudLeftCorner");
	%newCorner.resize(0, 0, 10, 18);
	%newCorner = new GuiBitmapCtrl();
	HUD_BrickNameBG.add(%newCorner);
	%newCorner.setProfile(HUDBitmapProfile);
	%newCorner.setBitmap("base/client/ui/BlueHudRightCorner");
	%newCorner.resize(%iconBoxWidth - 10, 0, 10, 18);
	%newSwatch = new GuiSwatchCtrl();
	HUD_BrickNameBG.add(%newSwatch);
	%newSwatch.setProfile(HUDBrickNameProfile);
	%newSwatch.setColor("0 0 0.5 0.5");
	%x = 10;
	%y = 0;
	%w = %iconBoxWidth - 20;
	%h = 18;
	%newSwatch.resize(%x, %y, %w, %h);
	%newText = new GuiTextCtrl();
	HUD_BrickNameBG.add(%newText);
	%newText.setProfile(HUDBrickNameProfile);
	%w = %iconBoxWidth;
	%h = 18;
	%x = 0;
	%y = 0;
	%newText.resize(%x, %y, %w, %h);
	%newText.setName("HUD_BrickName");
	%newText = new GuiTextCtrl();
	HUD_BrickNameBG.add(%newText);
	%newText.setProfile(HUDRightTextProfile);
	%key = strupr(getWord(moveMap.getBinding("openBSD"), 1));
	%newText.setText("Press" SPC %key SPC "for more bricks   ");
	%x = 0;
	%y = 0;
	%w = %iconBoxWidth;
	%h = 18;
	%newText.resize(%x, %y, %w, %h);
	%newText.setName("ToolTip_BSD");
	%newText = new GuiTextCtrl();
	HUD_BrickNameBG.add(%newText);
	%newText.setProfile(HUDLeftTextProfile);
	%key = strupr(getWord(moveMap.getBinding("useBricks"), 1)) SPC "or";
	%key = %key SPC strupr(getWord(moveMap.getBinding("useFirstSlot"), 1));
	%key = %key SPC strupr(getWord(moveMap.getBinding("useSecondSlot"), 1));
	%key = %key SPC strupr(getWord(moveMap.getBinding("useThirdSlot"), 1));
	%key = %key SPC strupr(getWord(moveMap.getBinding("useFourthSlot"), 1));
	%key = %key SPC strupr(getWord(moveMap.getBinding("useFifthSlot"), 1));
	%key = %key SPC strupr(getWord(moveMap.getBinding("useSixthSlot"), 1));
	%key = %key SPC strupr(getWord(moveMap.getBinding("useSeventhSlot"), 1));
	%key = %key SPC strupr(getWord(moveMap.getBinding("useEighthSlot"), 1));
	%key = %key SPC strupr(getWord(moveMap.getBinding("useNinthSlot"), 1));
	%key = %key SPC strupr(getWord(moveMap.getBinding("useTenthSlot"), 1));
	%newText.setText("  Press" SPC %key SPC "to use bricks");
	%x = 0;
	%y = 0;
	%w = %iconBoxWidth;
	%h = 18;
	%newText.resize(%x, %y, %w, %h);
	%newText.setName("ToolTip_Bricks");
	if (!$pref::HUD::showToolTips)
	{
		ToolTip_Bricks.setVisible(0);
		ToolTip_BSD.setVisible(0);
	}
	HUD_BrickName.setText("");
	if ($CurrScrollBrickSlot $= "")
	{
		$CurrScrollBrickSlot = 0;
	}
	if ($ScrollMode != $SCROLLMODE_BRICKS && $pref::HUD::HideBrickBox)
	{
		if ($pref::HUD::showToolTips)
		{
			PlayGui.hideBrickBox(64, 10, 0);
		}
		else
		{
			PlayGui.hideBrickBox(87, 10, 0);
		}
	}
	else if ($ScrollMode == $SCROLLMODE_BRICKS)
	{
		setActiveInv($CurrScrollBrickSlot);
		HUD_BrickActive.setVisible(1);
	}
}

function PlayGui::hideBrickBox(%this, %dist, %totalSteps, %currStep)
{
	if (%currStep >= %totalSteps)
	{
		return;
	}
	if (%currStep == %totalSteps - 1)
	{
		%sum = (%totalSteps - 1) * mFloor(%dist / %totalSteps);
		%stepDist = %dist - %sum;
	}
	else
	{
		%stepDist = mFloor(%dist / %totalSteps);
	}
	%pos = getWord(HUD_BrickBox.position, 1) + %stepDist;
	HUD_BrickBox.position = getWord(HUD_BrickBox.position, 0) SPC %pos;
	HUD_BrickNameBG.position = getWord(HUD_BrickNameBG.position, 0) SPC %pos - getWord(HUD_BrickNameBG.extent, 1);
	HUD_BrickBox.moveSchedule = PlayGui.schedule(10, hideBrickBox, %dist, %totalSteps, %currStep + 1);
}

function clientCmdPlayGui_LoadPaint()
{
	PlayGui.LoadPaint();
}

function PlayGui::LoadPaint(%this)
{
	%this.KillPaint();
	%swatchSize = 16;
	%res = getRes();
	%screenWidth = getWord(%res, 0);
	%screenHeight = getWord(%res, 1);
	%numDivs = 0;
	for (%i = 0; %i < 16; %i++)
	{
		if (getSprayCanDivisionSlot(%i) != 0)
		{
			%numDivs++;
		}
		else
		{
			break;
		}
	}
	%numDivs++;
	%lastDivStart = -1;
	%largestDivSize = 3;
	for (%i = 0; %i < %numDivs; %i++)
	{
		%currDivSize = getSprayCanDivisionSlot(%i) - %lastDivStart;
		$Paint_Row[%i] = new ScriptObject();
		$Paint_Row[%i].numSwatches = %currDivSize;
		RootGroup.add($Paint_Row[%i]);
		if (%currDivSize > %largestDivSize)
		{
			%largestDivSize = %currDivSize;
		}
		%lastDivStart = getSprayCanDivisionSlot(%i);
	}
	$Paint_NumPaintRows = %numDivs;
	$Paint_NumPaintRows = %numDivs;
	$Paint_Row[$Paint_NumPaintRows - 1].numSwatches = 8 + 1;
	%sideSpace = 0;
	%bottomSpace = 0;
	%boxWidth = %numDivs * (%swatchSize + 1) + 1;
	%boxHeight = %largestDivSize * (%swatchSize + 1) + 1;
	%newBox = new GuiSwatchCtrl();
	%this.add(%newBox);
	%newBox.setProfile(HUDBitmapProfile);
	%newBox.setColor("0 0 0 0.0");
	%x = 0 + %sideSpace;
	%y = ((%screenHeight - %boxHeight) - 18) - %bottomSpace;
	%w = %boxWidth + 100;
	%h = %boxHeight + 18;
	%newBox.resize(%x, %y, %w, %h);
	%newBox.setName("HUD_PaintBox");
	%newBmp = new GuiBitmapCtrl();
	%newBox.add(%newBmp);
	%newBmp.setProfile(HUDBitmapProfile);
	%newBmp.setBitmap("base/client/ui/paintLabelBG");
	%x = %boxWidth - 14;
	%y = 0;
	%w = 100;
	%h = 100;
	%newBmp.resize(%x, %y, %w, %h);
	%newBmp = new GuiBitmapCtrl();
	%newBox.add(%newBmp);
	%newBmp.setProfile(HUDBitmapProfile);
	%newBmp.setBitmap("base/client/ui/paintLabelBGLoop");
	%newBmp.wrap = 1;
	%x = %boxWidth - 14;
	%y = 100;
	%w = 100;
	%h = 100;
	%newBmp.resize(%x, %y, %w, %h);
	%newBmp = new GuiBitmapCtrl();
	%newBox.add(%newBmp);
	%newBmp.setProfile(HUDBitmapProfile);
	%newBmp.setBitmap("base/client/ui/paintLabel");
	%x = %boxWidth - 14;
	%y = 0;
	%w = 100;
	%h = 100;
	%newBmp.resize(%x, %y, %w, %h);
	%newBmp.setName("HUD_PaintIcon");
	%canIndex = 0;
	for (%i = 0; %i < $CurrPaintRow; %i++)
	{
		%canIndex += $Paint_Row[%i].numSwatches;
	}
	%canIndex += $CurrPaintSwatch;
	HUD_PaintIcon.setColor(getColorIDTable(%canIndex));
	%newBmp = new GuiBitmapCtrl();
	%newBox.add(%newBmp);
	%newBmp.setProfile(HUDBitmapProfile);
	%newBmp.setBitmap("base/client/ui/paintLabelTop");
	%x = %boxWidth - 14;
	%y = 0;
	%w = 100;
	%h = 100;
	%newBmp.resize(%x, %y, %w, %h);
	%newSwatch = new GuiSwatchCtrl();
	%newBox.add(%newSwatch);
	%newSwatch.setProfile(HUDBitmapProfile);
	%newSwatch.setColor("0 0 0 0.25");
	%x = 0;
	%y = 18;
	%w = %boxWidth;
	%h = %boxHeight;
	%newSwatch.resize(%x, %y, %w, %h);
	%currDiv = 0;
	for (%i = 0; %i < 64; %i++)
	{
		%color = getColorIDTable(%i);
		%red = getWord(%color, 0);
		%green = getWord(%color, 1);
		%blue = getWord(%color, 2);
		%alpha = getWord(%color, 3);
		if (%red == 0 && %green == 0 && %blue == 0 && %alpha == 0)
		{
			break;
		}
		if (getSprayCanDivisionSlot(%currDiv) != 0 && %i > getSprayCanDivisionSlot(%currDiv))
		{
			%currDiv++;
		}
		%newSwatch = new GuiSwatchCtrl();
		%newBox.add(%newSwatch);
		%newSwatch.setProfile(BlockDefaultProfile);
		%newSwatch.setColor(%color);
		if (%currDiv > 0)
		{
			%swatchNum = (%i - getSprayCanDivisionSlot(%currDiv - 1)) - 1;
		}
		else
		{
			%swatchNum = %i;
		}
		%x = (%swatchSize + 1) * %currDiv + 1;
		%y = (%swatchSize + 1) * %swatchNum + 1 + 18;
		%w = %swatchSize;
		%h = %swatchSize;
		%newSwatch.resize(%x, %y, %w, %h);
		$Paint_Row[%currDiv].swatch[%swatchNum] = %newSwatch;
	}
	%count = -1;
	%count++;
	%newSwatch = new GuiSwatchCtrl();
	HUD_PaintBox.add(%newSwatch);
	%newSwatch.setProfile(BlockDefaultProfile);
	%newSwatch.setColor("0.2 0.2 0.2 1.0");
	%x = (%swatchSize + 1) * (%numDivs - 1) + 1;
	%y = 1 + 18;
	%w = %swatchSize;
	%h = %swatchSize;
	%newSwatch.resize(%x, %y, %w, %h);
	$Paint_Row[$Paint_NumPaintRows - 1].swatch[%count] = %newSwatch;
	%count++;
	%newSwatch = new GuiBitmapCtrl();
	HUD_PaintBox.add(%newSwatch);
	%newSwatch.setProfile(BlockDefaultProfile);
	%newSwatch.setColor("1 1 1 1");
	%newSwatch.setBitmap("base/client/ui/FXpearl.png");
	%x = (%swatchSize + 1) * (%numDivs - 1) + 1;
	%y = (%swatchSize + 1) * %count + 1 + 18;
	%w = %swatchSize;
	%h = %swatchSize;
	%newSwatch.resize(%x, %y, %w, %h);
	$Paint_Row[$Paint_NumPaintRows - 1].swatch[%count] = %newSwatch;
	%count++;
	%newSwatch = new GuiBitmapCtrl();
	HUD_PaintBox.add(%newSwatch);
	%newSwatch.setProfile(BlockDefaultProfile);
	%newSwatch.setColor("1 1 1 1");
	%newSwatch.setBitmap("base/client/ui/FXchrome.png");
	%x = (%swatchSize + 1) * (%numDivs - 1) + 1;
	%y = (%swatchSize + 1) * %count + 1 + 18;
	%w = %swatchSize;
	%h = %swatchSize;
	%newSwatch.resize(%x, %y, %w, %h);
	$Paint_Row[$Paint_NumPaintRows - 1].swatch[%count] = %newSwatch;
	%count++;
	%newSwatch = new GuiBitmapCtrl();
	HUD_PaintBox.add(%newSwatch);
	%newSwatch.setProfile(BlockDefaultProfile);
	%newSwatch.setColor("1 1 1 1");
	%newSwatch.setBitmap("base/client/ui/FXglow.png");
	%x = (%swatchSize + 1) * (%numDivs - 1) + 1;
	%y = (%swatchSize + 1) * %count + 1 + 18;
	%w = %swatchSize;
	%h = %swatchSize;
	%newSwatch.resize(%x, %y, %w, %h);
	$Paint_Row[$Paint_NumPaintRows - 1].swatch[%count] = %newSwatch;
	%count++;
	%newSwatch = new GuiBitmapCtrl();
	HUD_PaintBox.add(%newSwatch);
	%newSwatch.setProfile(BlockDefaultProfile);
	%newSwatch.setColor("1 1 1 1");
	%newSwatch.setBitmap("base/client/ui/FXblink.png");
	%x = (%swatchSize + 1) * (%numDivs - 1) + 1;
	%y = (%swatchSize + 1) * %count + 1 + 18;
	%w = %swatchSize;
	%h = %swatchSize;
	%newSwatch.resize(%x, %y, %w, %h);
	$Paint_Row[$Paint_NumPaintRows - 1].swatch[%count] = %newSwatch;
	%count++;
	%newSwatch = new GuiBitmapCtrl();
	HUD_PaintBox.add(%newSwatch);
	%newSwatch.setProfile(BlockDefaultProfile);
	%newSwatch.setColor("1 1 1 1");
	%newSwatch.setBitmap("base/client/ui/FXswirl.png");
	%x = (%swatchSize + 1) * (%numDivs - 1) + 1;
	%y = (%swatchSize + 1) * %count + 1 + 18;
	%w = %swatchSize;
	%h = %swatchSize;
	%newSwatch.resize(%x, %y, %w, %h);
	$Paint_Row[$Paint_NumPaintRows - 1].swatch[%count] = %newSwatch;
	%count++;
	%newSwatch = new GuiBitmapCtrl();
	HUD_PaintBox.add(%newSwatch);
	%newSwatch.setProfile(BlockDefaultProfile);
	%newSwatch.setColor("1 1 1 1");
	%newSwatch.setBitmap("base/client/ui/FXrainbow.png");
	%x = (%swatchSize + 1) * (%numDivs - 1) + 1;
	%y = (%swatchSize + 1) * %count + 1 + 18;
	%w = %swatchSize;
	%h = %swatchSize;
	%newSwatch.resize(%x, %y, %w, %h);
	$Paint_Row[$Paint_NumPaintRows - 1].swatch[%count] = %newSwatch;
	%count++;
	%newSwatch = new GuiBitmapCtrl();
	HUD_PaintBox.add(%newSwatch);
	%newSwatch.setProfile(BlockDefaultProfile);
	%newSwatch.setColor("1 1 1 1");
	%newSwatch.setBitmap("base/client/ui/FXstable.png");
	%x = (%swatchSize + 1) * (%numDivs - 1) + 1;
	%y = (%swatchSize + 1) * %count + 1 + 18;
	%w = %swatchSize;
	%h = %swatchSize;
	%newSwatch.resize(%x, %y, %w, %h);
	$Paint_Row[$Paint_NumPaintRows - 1].swatch[%count] = %newSwatch;
	%count++;
	%newSwatch = new GuiBitmapCtrl();
	HUD_PaintBox.add(%newSwatch);
	%newSwatch.setProfile(BlockDefaultProfile);
	%newSwatch.setColor("1 1 1 1");
	%newSwatch.setBitmap("base/client/ui/FXjello.png");
	%x = (%swatchSize + 1) * (%numDivs - 1) + 1;
	%y = (%swatchSize + 1) * %count + 1 + 18;
	%w = %swatchSize;
	%h = %swatchSize;
	%newSwatch.resize(%x, %y, %w, %h);
	$Paint_Row[$Paint_NumPaintRows - 1].swatch[%count] = %newSwatch;
	%newActive = new GuiBitmapCtrl();
	HUD_PaintBox.add(%newActive);
	%newActive.setProfile(HUDBitmapProfile);
	%newActive.setBitmap("base/client/ui/paintActive");
	%x = 0;
	%y = 0;
	%w = 18;
	%h = 18;
	%newActive.resize(%x, %y, %w, %h);
	%newActive.setName("HUD_PaintActive");
	%newSwatch = new GuiSwatchCtrl();
	PlayGui.add(%newSwatch);
	%newSwatch.setProfile(HUDBrickNameProfile);
	%newSwatch.setColor("0.0 0.0 0.5 0.0");
	%w = %boxWidth;
	%h = 18;
	%x = 0 + %sideSpace;
	%y = ((%screenHeight - %boxHeight) - %bottomSpace) - %h;
	%newSwatch.resize(%x, %y, %w, %h);
	%newSwatch.setName("HUD_PaintNameBG");
	%newCorner = new GuiBitmapCtrl();
	HUD_PaintNameBG.add(%newCorner);
	%newCorner.setProfile(HUDBitmapProfile);
	%newCorner.setBitmap("base/client/ui/BlueHudLeftCorner");
	%newCorner.resize(0, 0, 10, 18);
	%newCorner = new GuiBitmapCtrl();
	HUD_PaintNameBG.add(%newCorner);
	%newCorner.setProfile(HUDBitmapProfile);
	%newCorner.setBitmap("base/client/ui/BlueHudRightCorner");
	%newCorner.resize(%boxWidth - 10, 0, 10, 18);
	%newSwatch = new GuiSwatchCtrl();
	HUD_PaintNameBG.add(%newSwatch);
	%newSwatch.setProfile(HUDBitmapProfile);
	%newSwatch.setColor("0 0 0.5 0.5");
	%x = 10;
	%y = 0;
	%w = %boxWidth - 20;
	%h = 18;
	%newSwatch.resize(%x, %y, %w, %h);
	%newText = new GuiTextCtrl();
	HUD_PaintNameBG.add(%newText);
	%newText.setProfile(HUDCenterTextProfile);
	%w = %boxWidth;
	%h = 18;
	%x = 0;
	%y = 0;
	%newText.resize(%x, %y, %w, %h);
	%newText.setName("HUD_PaintName");
	%newBmp = new GuiBitmapCtrl();
	HUD_PaintBox.add(%newBmp);
	%newBmp.setProfile(HUDBrickNameProfile);
	%newBmp.setBitmap("base/client/ui/ItemIcons/toolLabelBG");
	%x = %boxWidth + 4;
	%y = 61;
	%w = 64;
	%h = 18;
	%newBmp.resize(%x, %y, %w, %h);
	%newBmp.setName("ToolTip_Paint");
	%newText = new GuiTextCtrl();
	%newBmp.add(%newText);
	%newText.setProfile(HUDCenterTextProfile);
	%key = strupr(getWord(moveMap.getBinding("useSprayCan"), 1));
	%newText.setText(%key @ " = Paint");
	%x = 0;
	%y = 0;
	%w = 64;
	%h = 18;
	%newText.resize(%x, %y, %w, %h);
	if (!$pref::HUD::showToolTips)
	{
		ToolTip_Paint.setVisible(0);
	}
	if ($CurrPaintSwatch $= "")
	{
		$CurrPaintSwatch = 0;
	}
	if ($CurrPaintRow $= "")
	{
		$CurrPaintRow = 0;
	}
	HUD_PaintName.setText("");
	%this.FadePaintRows();
	HUD_PaintActive.setVisible(0);
	if ($ScrollMode != $SCROLLMODE_PAINT && $pref::HUD::HidePaintBox)
	{
		if ($pref::HUD::showToolTips)
		{
			PlayGui.hidePaintBox((getWord(HUD_PaintBox.extent, 0) - 100) + 5, 10, 0);
		}
		else
		{
			PlayGui.hidePaintBox(getWord(HUD_PaintBox.extent, 0) + 5, 10, 0);
		}
	}
	else if ($ScrollMode == $SCROLLMODE_PAINT)
	{
		%this.UnFadePaintRow($CurrPaintRow);
		%this.updatePaintActive();
		HUD_PaintActive.setVisible(1);
	}
}

function PlayGui::hidePaintBox(%this, %dist, %totalSteps, %currStep)
{
	if (%currStep >= %totalSteps)
	{
		return;
	}
	if (%currStep == %totalSteps - 1)
	{
		%sum = (%totalSteps - 1) * mFloor(%dist / %totalSteps);
		%stepDist = %dist - %sum;
	}
	else
	{
		%stepDist = mFloor(%dist / %totalSteps);
	}
	%pos = getWord(HUD_PaintBox.position, 0) - %stepDist;
	HUD_PaintBox.position = %pos SPC getWord(HUD_PaintBox.position, 1);
	HUD_PaintNameBG.position = %pos SPC getWord(HUD_PaintNameBG.position, 1);
	HUD_PaintBox.moveSchedule = PlayGui.schedule(10, hidePaintBox, %dist, %totalSteps, %currStep + 1, %originalPos);
}

function PlayGui::updatePaintActive(%this)
{
	%x = (16 + 1) * $CurrPaintRow;
	%y = getWord($Paint_Row[$CurrPaintRow].swatch[$CurrPaintSwatch].getPosition(), 1) - 1;
	%w = 18;
	%h = 18;
	HUD_PaintActive.resize(%x, %y, %w, %h);
	HUD_PaintIcon.setColor($Paint_Row[$CurrPaintRow].swatch[$CurrPaintSwatch].getColor());
}

function PlayGui::FadePaintRows(%this)
{
	for (%i = 0; %i < $Paint_NumPaintRows; %i++)
	{
		PlayGui.FadePaintRow(%i);
	}
}

function PlayGui::FadePaintRow(%this, %row)
{
	for (%i = 0; %i < $Paint_Row[%row].numSwatches; %i++)
	{
		%color = $Paint_Row[%row].swatch[%i].getColor();
		%red = getWord(%color, 0);
		%green = getWord(%color, 1);
		%blue = getWord(%color, 2);
		%alpha = getWord(%color, 3);
		%newColor = %red @ " " @ %green @ " " @ %blue @ " " @ %alpha * 0.3;
		$Paint_Row[%row].swatch[%i].setColor(%newColor);
	}
}

function PlayGui::UnFadePaintRow(%this, %row)
{
	for (%i = 0; %i < $Paint_Row[%row].numSwatches; %i++)
	{
		%color = $Paint_Row[%row].swatch[%i].getColor();
		%red = getWord(%color, 0);
		%green = getWord(%color, 1);
		%blue = getWord(%color, 2);
		%alpha = getWord(%color, 3);
		%newColor = %red @ " " @ %green @ " " @ %blue @ " " @ %alpha / 0.3;
		$Paint_Row[%row].swatch[%i].setColor(%newColor);
	}
	HUD_PaintName.setText(getSprayCanDivisionName($CurrPaintRow));
}

function PlayGui::KillPaint(%this)
{
	if (isEventPending(HUD_PaintBox.moveSchedule))
	{
		cancel(HUD_PaintBox.moveSchedule);
	}
	if (isObject(HUD_PaintBox))
	{
		HUD_PaintBox.delete();
	}
	if (isObject(HUD_PaintNameBG))
	{
		HUD_PaintNameBG.delete();
	}
	if (isObject(HUD_PaintName))
	{
		HUD_PaintName.delete();
	}
	for (%i = 0; %i < $Paint_NumPaintRows; %i++)
	{
		if (isObject($Paint_Row[%i]))
		{
			$Paint_Row[%i].delete();
		}
	}
}

function clientCmdPlayGui_CreateToolHud(%numSlots)
{
	$HUD_NumToolSlots = %numSlots;
	PlayGui.createToolHud();
}

function PlayGui::createToolHud(%this)
{
	%this.killToolHud();
	%numSlots = $HUD_NumToolSlots;
	%res = getRes();
	%screenWidth = getWord(%res, 0);
	%screenHeight = getWord(%res, 1);
	%iconWidth = mFloor(%screenWidth / $BSD_NumInventorySlots);
	if (%iconWidth > 64)
	{
		%iconWidth = 64;
	}
	%iconBoxHeight = $HUD_NumToolSlots * %iconWidth;
	%topSpace = 0;
	%sideSpace = 0;
	%newBox = new GuiBitmapCtrl();
	%newBox.setName("HUD_ToolBox");
	%this.add(%newBox);
	%newBox.setProfile(HUDBitmapProfile);
	%newBox.setBitmap("base/client/ui/itemIcons/ToolBG");
	%newBox.wrap = 1;
	%x = (%screenWidth - %iconWidth) - %sideSpace;
	%y = 0;
	%w = %iconWidth;
	%h = %iconBoxHeight;
	%newBox.resize(%x, %y, %w, %h);
	%newActive = new GuiBitmapCtrl();
	%newBox.add(%newActive);
	%newActive.setProfile(HUDBitmapProfile);
	%newActive.setBitmap("base/client/ui/itemIcons/ItemActive");
	%newActive.setVisible(0);
	%x = 0;
	%y = 0;
	%w = %iconWidth;
	%h = %iconWidth;
	%newActive.resize(%x, %y, %w, %h);
	%newActive.setName("HUD_ToolActive");
	for (%i = 0; %i < %numSlots; %i++)
	{
		%newIcon = new GuiBitmapCtrl();
		%newBox.add(%newIcon);
		%newIcon.setProfile(HUDBitmapProfile);
		if ($ToolData[%i] > 0)
		{
			if (!isFile($ToolData[%i].iconName @ ".png"))
			{
				%newIcon.setBitmap("base/client/ui/brickIcons/unknown.png");
				continue;
			}
			%newIcon.setBitmap($ToolData[%i].iconName);
		}
		if ($ToolData[%i].doColorShift)
		{
			%newIcon.setColor($ToolData[%i].colorShiftColor);
		}
		else
		{
			%newIcon.setColor("1 1 1 1");
		}
		%x = 0;
		%y = %i * %iconWidth;
		%w = %iconWidth;
		%h = %iconWidth;
		%newIcon.resize(%x, %y, %w, %h);
		$HUD_ToolIcon[%i] = %newIcon;
	}
	%newSwatch = new GuiBitmapCtrl();
	PlayGui.add(%newSwatch);
	%newSwatch.setProfile(HUDBrickNameProfile);
	%newSwatch.setBitmap("base/client/ui/ItemIcons/toolLabelBG");
	%w = %iconWidth;
	%h = 18;
	%x = (%screenWidth - %iconWidth) - %sideSpace;
	%y = %iconWidth * %numSlots + 0;
	%newSwatch.resize(%x, %y, %w, %h);
	%newSwatch.setName("HUD_ToolNameBG");
	%newText = new GuiTextCtrl();
	%newSwatch.add(%newText);
	%newText.setProfile(HUDCenterTextProfile);
	%w = %iconWidth;
	%h = 18;
	%x = 0;
	%y = 0;
	%newText.resize(%x, %y, %w, %h);
	%newText.setName("HUD_ToolName");
	%newText = new GuiTextCtrl();
	HUD_ToolNameBG.add(%newText);
	%newText.setProfile(HUDCenterTextProfile);
	%key = strupr(getWord(moveMap.getBinding("useTools"), 1));
	%newText.setText(%key SPC "= tools");
	%x = 0;
	%y = 0;
	%w = %iconWidth;
	%h = 18;
	%newText.resize(%x, %y, %w, %h);
	%newText.setName("ToolTip_Tools");
	if (!$pref::HUD::showToolTips)
	{
		ToolTip_Tools.setVisible(0);
	}
	HUD_ToolName.setText("");
	if ($CurrScrollToolSlot $= "")
	{
		$CurrScrollToolSlot = 0;
	}
	if ($ScrollMode != $SCROLLMODE_TOOLS && $pref::HUD::HideToolBox)
	{
		if ($pref::HUD::showToolTips)
		{
			PlayGui.hideToolBox($HUD_NumToolSlots * 64, 10, 0);
		}
		else
		{
			PlayGui.hideToolBox($HUD_NumToolSlots * 64 + 25, 10, 0);
		}
		HUD_ToolActive.setVisible(0);
	}
	else if ($ScrollMode == $SCROLLMODE_TOOLS)
	{
		setActiveTool($CurrScrollToolSlot);
		HUD_ToolActive.setVisible(1);
		ToolTip_Tools.setVisible(0);
	}
	setScrollMode($SCROLLMODE_NONE);
}

function PlayGui::hideToolBox(%this, %dist, %totalSteps, %currStep)
{
	if (%currStep >= %totalSteps)
	{
		return;
	}
	if (%currStep == %totalSteps - 1)
	{
		%sum = (%totalSteps - 1) * mFloor(%dist / %totalSteps);
		%stepDist = %dist - %sum;
	}
	else
	{
		%stepDist = mFloor(%dist / %totalSteps);
	}
	%pos = getWord(HUD_ToolBox.position, 1) - %stepDist;
	HUD_ToolBox.position = getWord(HUD_ToolBox.position, 0) SPC %pos;
	HUD_ToolNameBG.position = getWord(HUD_ToolNameBG.position, 0) SPC %pos + getWord(HUD_ToolBox.extent, 1);
	HUD_ToolBox.moveSchedule = PlayGui.schedule(10, hideToolBox, %dist, %totalSteps, %currStep + 1);
}

function PlayGui::killToolHud(%this)
{
	if (isEventPending(HUD_ToolBox.moveSchedule))
	{
		cancel(HUD_ToolBox.moveSchedule);
	}
	if (isObject(HUD_ToolBox))
	{
		HUD_ToolBox.delete();
	}
	if (isObject(HUD_ToolNameBG))
	{
		HUD_ToolNameBG.delete();
	}
	if (isObject(HUD_ToolName))
	{
		HUD_ToolName.delete();
	}
}

function clientCmdShowEnergyBar(%val)
{
	HUD_EnergyBar.setVisible(%val);
}

function clientCmdSetScrollMode(%mode)
{
	setScrollMode(%mode);
}

$centerPrintActive = 0;
$bottomPrintActive = 0;
$CenterPrintSizes[1] = 20;
$CenterPrintSizes[2] = 36;
$CenterPrintSizes[3] = 56;
function clientCmdCenterPrint(%message, %time, %size)
{
	if ($centerPrintActive)
	{
		if (centerPrintDlg.removePrint !$= "")
		{
			cancel(centerPrintDlg.removePrint);
		}
	}
	else
	{
		centerPrintDlg.visible = 1;
		$centerPrintActive = 1;
	}
	CenterPrintText.setText("<just:center>" @ %message @ "\n");
	centerPrintDlg.extent = firstWord(centerPrintDlg.extent) @ " " @ $CenterPrintSizes[%size];
	if (%time > 0)
	{
		centerPrintDlg.removePrint = schedule(%time * 1000, 0, "clientCmdClearCenterPrint");
	}
}

function clientCmdBottomPrint(%message, %time, %size)
{
	if ($bottomPrintActive)
	{
		if (bottomPrintDlg.removePrint !$= "")
		{
			cancel(bottomPrintDlg.removePrint);
		}
	}
	else
	{
		bottomPrintDlg.setVisible(1);
		$bottomPrintActive = 1;
	}
	BottomPrintText.setText(%message);
	bottomPrintDlg.extent = firstWord(bottomPrintDlg.extent) @ " " @ $CenterPrintSizes[%size];
	if (%time > 0)
	{
		bottomPrintDlg.removePrint = schedule(%time * 1000, 0, "clientCmdClearbottomPrint");
	}
}

function BottomPrintText::onResize(%this, %__unused, %__unused)
{
	%this.position = "10 0";
}

function CenterPrintText::onResize(%this, %__unused, %__unused)
{
	%this.position = "0 0";
}

function clientCmdClearCenterPrint()
{
	$centerPrintActive = 0;
	centerPrintDlg.visible = 0;
	if (isEventPending(centerPrintDlg.removePrint))
	{
		cancel(centerPrintDlg.removePrint);
	}
	centerPrintDlg.removePrint = "";
}

function clientCmdclearBottomPrint()
{
	$bottomPrintActive = 0;
	bottomPrintDlg.visible = 0;
	if (isEventPending(bottomPrintDlg.removePrint))
	{
		cancel(bottomPrintDlg.removePrint);
	}
	bottomPrintDlg.removePrint = "";
}

function clientCmdGameStart(%__unused)
{
}

function clientCmdGameEnd(%__unused)
{
	alxStopAll();
	Canvas.setContent(EndGameGui);
}

addMessageCallback('MsgYourDeath', handleYourDeath);
function handleYourDeath(%msgType, %msgString, %__unused, %__unused, %respawnTime)
{
	setScrollMode($SCROLLMODE_NONE);
	respawnCountDownTick(mFloor(%respawnTime / 1000));
}

function respawnCountDownTick(%time)
{
	if (isEventPending($respawnCountDownSchedule))
	{
		cancel($respawnCountDownSchedule);
	}
	if (%time <= 0)
	{
		clientCmdCenterPrint("\c5Click to respawn.", 300);
	}
	else
	{
		if (%time == 1)
		{
			clientCmdCenterPrint("\c5Respawning in 1 second...", 2);
		}
		else
		{
			clientCmdCenterPrint("\c5Respawning in " @ %time @ " seconds...", 2);
		}
		$respawnCountDownSchedule = schedule(1000, 0, respawnCountDownTick, %time - 1);
	}
}

addMessageCallback('MsgYourSpawn', handleYourSpawn);
function handleYourSpawn(%msgType, %msgString)
{
	if (isEventPending($respawnCountDownSchedule))
	{
		cancel($respawnCountDownSchedule);
	}
	clientCmdClearCenterPrint();
}

addMessageCallback('MsgError', handleError);
function handleError(%msgType, %msgString)
{
	alxPlay(AudioError);
}

addMessageCallback('MsgAdminForce', handleAdminForce);
function handleAdminForce(%msgType, %msgString)
{
	alxPlay(AdminSound);
}

addMessageCallback('MsgClearBricks', handleClearBricks);
function handleClearBricks(%msgType, %msgString)
{
	alxPlay(BrickClearSound);
}

addMessageCallback('MsgPlantError_Overlap', handlePlantError);
addMessageCallback('MsgPlantError_Float', handlePlantError);
addMessageCallback('MsgPlantError_Unstable', handlePlantError);
addMessageCallback('MsgPlantError_Buried', handlePlantError);
addMessageCallback('MsgPlantError_Stuck', handlePlantError);
addMessageCallback('MsgPlantError_TooFar', handlePlantError);
addMessageCallback('MsgPlantError_Teams', handlePlantError);
addMessageCallback('MsgPlantError_Flood', handlePlantError);
addMessageCallback('MsgPlantError_Limit', handlePlantError);
addMessageCallback('MsgPlantError_TooLoud', handlePlantError);
addMessageCallback('MsgPlantError', handlePlantError);
function handlePlantError(%msgType, %msgString)
{
	if ($pref::Video::useSmallPlantErrors)
	{
		%hudObj = HUD_PlantErrorSmall;
		%bitmap = "base/client/ui/PlantErrors_Small/";
	}
	else
	{
		%hudObj = HUD_PlantError;
		%bitmap = "base/client/ui/PlantErrors/";
	}
	if (getTag(%msgType) == getTag('MsgPlantError_Overlap'))
	{
		%bitmap = %bitmap @ "PlantError_Overlap";
	}
	else if (getTag(%msgType) == getTag('MsgPlantError_Float'))
	{
		%bitmap = %bitmap @ "PlantError_Float";
	}
	else if (getTag(%msgType) == getTag('MsgPlantError_Unstable'))
	{
		%bitmap = %bitmap @ "PlantError_Unstable";
	}
	else if (getTag(%msgType) == getTag('MsgPlantError_Buried'))
	{
		%bitmap = %bitmap @ "PlantError_Buried";
	}
	else if (getTag(%msgType) == getTag('MsgPlantError_Stuck'))
	{
		%bitmap = %bitmap @ "PlantError_Stuck";
	}
	else if (getTag(%msgType) == getTag('MsgPlantError_TooFar'))
	{
		%bitmap = %bitmap @ "PlantError_TooFar";
	}
	else if (getTag(%msgType) == getTag('MsgPlantError_Teams'))
	{
		%bitmap = %bitmap @ "PlantError_Teams";
	}
	else if (getTag(%msgType) == getTag('MsgPlantError_Flood'))
	{
		%bitmap = %bitmap @ "PlantError_Flood";
	}
	else if (getTag(%msgType) == getTag('MsgPlantError_Limit'))
	{
		%bitmap = %bitmap @ "PlantError_Limit";
	}
	else if (getTag(%msgType) == getTag('MsgPlantError_TooLoud'))
	{
		%bitmap = %bitmap @ "PlantError_TooLoud";
	}
	else
	{
		%bitmap = %bitmap @ "PlantError_Forbidden";
	}
	%hudObj.setBitmap(%bitmap);
	%hudObj.setVisible(1);
	if ($Pref::Audio::PlantErrorSound)
	{
		alxPlay(AudioError);
	}
	if (isEventPending(%hudObj.hideSchedule))
	{
		cancel(%hudObj.hideSchedule);
	}
	%hudObj.hideSchedule = %hudObj.schedule(800, setVisible, 0);
}

addMessageCallback('MsgItemPickup', handleItemPickup);
function handleItemPickup(%msgType, %msgString, %slot, %itemData, %silent)
{
	$ToolData[%slot] = %itemData;
	if (!isFile(%itemData.iconName @ ".png"))
	{
		if (!isObject(%itemData))
		{
			$HUD_ToolIcon[%slot].setBitmap("");
		}
		else
		{
			$HUD_ToolIcon[%slot].setBitmap("base/client/ui/brickIcons/unknown.png");
		}
	}
	else
	{
		$HUD_ToolIcon[%slot].setBitmap(%itemData.iconName);
	}
	if ($ToolData[%slot].doColorShift)
	{
		$HUD_ToolIcon[%slot].setColor($ToolData[%slot].colorShiftColor);
	}
	else
	{
		$HUD_ToolIcon[%slot].setColor("1 1 1 1");
	}
	if (!%silent)
	{
		if ($ScrollMode == $SCROLLMODE_TOOLS)
		{
			if ($CurrScrollToolSlot == %slot)
			{
				commandToServer('useTool', %slot);
			}
		}
		alxPlay(ItemPickup);
	}
}

addMessageCallback('MsgDropItem', handleDropItem);
function handleDropItem(%msgType, %string, %slot)
{
}

addMessageCallback('MsgClearInv', handleClearInv);
function handleClearInv(%msgType)
{
	$CurrScrollBrickSlot = 0;
	HUD_BrickActive.setVisible(0);
	for (%i = 0; %i < $BSD_NumInventorySlots; %i++)
	{
		$InvData[%i] = 0;
		$HUD_BrickIcon[%i].setBitmap("");
	}
	HUD_BrickName.setText("");
	for (%i = 0; %i < $HUD_NumToolSlots; %i++)
	{
		$ToolData[%i] = 0;
		$HUD_ToolIcon[%i].setBitmap("");
	}
	if (isObject(HUD_ToolName))
	{
		HUD_ToolName.setText("");
	}
	$CurrScrollToolSlot = 0;
	return;
}

addMessageCallback('MsgHilightInv', handleHilightInv);
function handleHilightInv(%msgType, %msgString, %slot)
{
	return;
	for (%i = 0; %i < $BSD_NumInventorySlots; %i++)
	{
		eval("HUDInvActive" @ %i @ ".setVisible(false);");
	}
	if (%slot >= 0)
	{
		eval("HUDInvActive" @ %slot @ ".setVisible(true);");
	}
	return;
	%slot++;
	if ($invHilight == 1)
	{
		Slot1BG.setBitmap("base/client/ui/GUIBrickSide.png");
	}
	else if ($invHilight == 2)
	{
		Slot2BG.setBitmap("base/client/ui/GUIBrickSide.png");
	}
	else if ($invHilight == 3)
	{
		Slot3BG.setBitmap("base/client/ui/GUIBrickSide.png");
	}
	else if ($invHilight == 4)
	{
		Slot4BG.setBitmap("base/client/ui/GUIBrickSide.png");
	}
	else if ($invHilight == 5)
	{
		Slot5BG.setBitmap("base/client/ui/GUIBrickSide.png");
	}
	else if ($invHilight == 6)
	{
		Slot6BG.setBitmap("base/client/ui/GUIBrickSide.png");
	}
	else if ($invHilight == 7)
	{
		Slot7BG.setBitmap("base/client/ui/GUIBrickSide.png");
	}
	if (%slot == 1)
	{
		Slot1BG.setBitmap("base/client/ui/GUIBrickSideHilight.png");
	}
	else if (%slot == 2)
	{
		Slot2BG.setBitmap("base/client/ui/GUIBrickSideHilight.png");
	}
	else if (%slot == 3)
	{
		Slot3BG.setBitmap("base/client/ui/GUIBrickSideHilight.png");
	}
	else if (%slot == 4)
	{
		Slot4BG.setBitmap("base/client/ui/GUIBrickSideHilight.png");
	}
	else if (%slot == 5)
	{
		Slot5BG.setBitmap("base/client/ui/GUIBrickSideHilight.png");
	}
	else if (%slot == 6)
	{
		Slot6BG.setBitmap("base/client/ui/GUIBrickSideHilight.png");
	}
	else if (%slot == 7)
	{
		Slot7BG.setBitmap("base/client/ui/GUIBrickSideHilight.png");
	}
	$invHilight = %slot;
}

addMessageCallback('MsgEquipInv', handleEquipInv);
function handleEquipInv(%msgType, %msgString, %slot)
{
}

addMessageCallback('MsgDeEquipInv', handleDeEquipInv);
function handleDeEquipInv(%msgType, %msgString, %slot)
{
}

addMessageCallback('MsgSetInvData', handleSetInvData);
function handleSetInvData(%msgType, %msgString, %slot, %data)
{
	$InvData[%slot] = %data;
	if (isObject(%data))
	{
		if (!isFile(%data.iconName @ ".png"))
		{
			$HUD_BrickIcon[%slot].setBitmap("base/client/ui/brickIcons/unknown.png");
		}
		else
		{
			$HUD_BrickIcon[%slot].setBitmap(%data.iconName);
		}
		if ($ScrollMode == $SCROLLMODE_BRICKS && $CurrScrollBrickSlot == %slot)
		{
			commandToServer('useInventory', %slot);
			setActiveInv(%slot);
		}
	}
	else
	{
		$HUD_BrickIcon[%slot].setBitmap("");
	}
}

addMessageCallback('MsgStartTalking', handleStartTalking);
function handleStartTalking(%msgType, %msgString, %clientId)
{
	WhoTalk_addID(%clientId);
	return;
	%text = chatWhosTalkingText.getValue();
	%row = lstAdminPlayerList.getRowTextById(%clientId);
	echo("row = ", %row);
	%name = getField(%row, 0);
	if (strpos(%text, %name) == -1 && %name !$= "")
	{
		%text = %text SPC %name;
		chatWhosTalkingBox.setVisible(1);
		chatWhosTalkingText.setText(%text);
	}
}

addMessageCallback('MsgStopTalking', handleStopTalking);
function handleStopTalking(%msgType, %msgString, %clientId)
{
	WhoTalk_removeID(%clientId);
	return;
	%text = chatWhosTalkingText.getValue();
	%name = lstAdminPlayerList.getRowTextById(%clientId);
	%text = trim(strreplace(%text, " " @ %name, ""));
	chatWhosTalkingText.setText(%text);
	if (strlen(%text) <= 0)
	{
		chatWhosTalkingBox.setVisible(0);
	}
}

addMessageCallback('MsgUploadStart', handleUploadStart);
addMessageCallback('MsgUploadEnd', handleUploadEnd);
addMessageCallback('MsgProcessComplete', handleProcessComplete);
function handleUploadStart(%msgType, %msgString, %clientId)
{
	alxPlay(UploadStartSound);
}

function handleUploadEnd(%msgType, %msgString, %clientId)
{
	alxPlay(UploadEndSound);
}

function handleProcessComplete(%msgType, %msgString, %clientId)
{
	alxPlay(ProcessCompleteSound);
}

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
	SM_PlayerCountMenu.clear();
	for (%i = 1; %i <= 32; %i++)
	{
		SM_PlayerCountMenu.add(%i, %i);
	}
	SM_PlayerCountMenu.setSelected($Pref::Server::MaxPlayers);
	SM_OptSinglePlayer.setValue(0);
	SM_OptLAN.setValue(0);
	SM_OptInternet.setValue(0);
	if ($Pref::Net::ServerType $= "SinglePlayer")
	{
		SM_OptSinglePlayer.setValue(1);
		SM_PlayerCountMenu.setSelected(1);
		SM_OptionsBlocker.setVisible(1);
	}
	else if ($Pref::Net::ServerType $= "LAN")
	{
		SM_OptLAN.setValue(1);
		SM_PlayerCountMenu.setSelected($Pref::Server::MaxPlayers);
		SM_OptionsBlocker.setVisible(0);
	}
	else if ($Pref::Net::ServerType $= "Internet")
	{
		SM_OptInternet.setValue(1);
		SM_PlayerCountMenu.setSelected($Pref::Server::MaxPlayers);
		SM_OptionsBlocker.setVisible(0);
	}
	else
	{
		$Pref::Net::ServerType = "SinglePlayer";
		SM_OptSinglePlayer.setValue(1);
		SM_OptionsBlocker.setVisible(1);
	}
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
	if ($Pref::Net::ServerType !$= "SinglePlayer")
	{
		$Pref::Server::MaxPlayers = SM_PlayerCountMenu.getSelected();
	}
	$Client::Password = TxtServerPassword.getValue();
	%id = SM_missionList.getSelectedId();
	%mission = getField(SM_missionList.getRowTextById(%id), 2);
	if ($Pref::Net::ServerType $= "SinglePlayer")
	{
		$timeScale = 10;
	}
	else
	{
		$timeScale = 1;
	}
	createServer($Pref::Net::ServerType, %mission);
	%conn = new GameConnection(ServerConnection);
	RootGroup.add(ServerConnection);
	%conn.setConnectArgs($pref::Player::LANName, $pref::Player::NetName, $Pref::Player::ClanPrefix, $Pref::Player::ClanSuffix);
	%conn.setJoinPassword($Client::Password);
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

function startMissionGui::ClickSinglePlayer()
{
	SM_OptionsBlocker.setVisible(1);
	SM_PlayerCountMenu.setSelected(1);
	$Pref::Net::ServerType = "SinglePlayer";
}

function startMissionGui::ClickLAN()
{
	SM_OptionsBlocker.setVisible(0);
	SM_PlayerCountMenu.setSelected($Pref::Server::MaxPlayers);
	$Pref::Net::ServerType = "LAN";
}

function startMissionGui::ClickInternet()
{
	SM_OptionsBlocker.setVisible(0);
	SM_PlayerCountMenu.setSelected($Pref::Server::MaxPlayers);
	$Pref::Net::ServerType = "Internet";
	if (!$Pref::Gui::IgnorePortForwardWarning)
	{
		Canvas.pushDialog(portForwardInfoGui);
	}
}

function clientCmdUpdatePrefs()
{
	commandToServer('updatePrefs', $pref::Player::LANName);
	commandToServer('updateBodyColors', $pref::Player::HeadColor, $pref::Player::HatColor, $pref::Player::AccentColor, $pref::Player::PackColor, $pref::Player::SecondPackColor, $pref::Player::TorsoColor, $pref::Player::HipColor, $pref::Player::LLegColor, $pref::Player::RLegColor, $pref::Player::LArmColor, $pref::Player::RArmColor, $pref::Player::LHandColor, $pref::Player::RHandColor, $Pref::Player::DecalName, $Pref::Player::FaceName);
	commandToServer('updateBodyParts', $pref::Player::Hat, $pref::Player::Accent, $pref::Player::Pack, $Pref::Player::SecondPack, $Pref::Player::Chest, $Pref::Player::Hip, $Pref::Player::LLeg, $Pref::Player::RLeg, $Pref::Player::LArm, $Pref::Player::RArm, $Pref::Player::LHand, $Pref::Player::RHand);
}

function clientCmdTimeScale(%val)
{
	if (%val < 0.1 || %val > 2)
	{
		return;
	}
	$timeScale = %val;
}

function clientCmdSetFocalPoint(%point)
{
	$focalPoint = %point;
}

function MJ_connect()
{
	cancelServerQuery();
	%ip = MJ_txtIP.getValue();
	%joinPass = MJ_txtJoinPass.getValue();
	echo("Attempting to connect to ", %ip);
	if (%ip)
	{
		if (isObject($conn))
		{
			$conn.cancelConnect();
			$conn.delete();
		}
		Connecting_Text.setText("Connecting to " @ %ip);
		Canvas.pushDialog(ConnectingGui);
		$conn = new GameConnection(ServerConnection);
		$conn.setConnectArgs($pref::Player::LANName, $pref::Player::NetName, $Pref::Player::ClanPrefix, $Pref::Player::ClanSuffix);
		$conn.setJoinPassword(%joinPass);
		$conn.connect(%ip);
	}
}

function ManualJoin::onWake()
{
	MJ_txtJoinPass.setText("");
}

function JoinServerGui::onWake()
{
	if ($pref::Gui::AutoQueryMasterServer)
	{
		if (JoinServerGui.lastQueryTime == 0 || getSimTime() - JoinServerGui.lastQueryTime > 5 * 60 * 1000)
		{
			JoinServerGui.hasQueriedOnce = 1;
			JoinServerGui.queryWebMaster();
		}
	}
}

function JoinServerGui::queryWebMaster(%this)
{
	if (!isUnlocked())
	{
		JSG_demoBanner.setVisible(1);
		JSG_demoBanner.setColor("1 1 1 0.65");
		JSG_demoBanner2.setVisible(1);
		JSG_demoBanner2.setColor("1 1 1 0.65");
	}
	else
	{
		JSG_demoBanner.setVisible(0);
		JSG_demoBanner2.setVisible(0);
	}
	JoinServerGui.lastQueryTime = getSimTime();
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
	queryMasterTCPObj.filePath = "/master/index.asp";
	queryMasterTCPObj.cmd = "GET " @ queryMasterTCPObj.filePath @ " HTTP/1.1\r\nHost: " @ queryMasterTCPObj.site @ "\r\n\r\n";
	queryMasterTCPObj.connect(queryMasterTCPObj.site @ ":" @ queryMasterTCPObj.port);
	JS_queryStatus.setVisible(1);
	JS_statusText.setText("Getting Server List...");
}

function queryMasterTCPObj::onConnected(%this)
{
	%this.send(%this.cmd);
}

function queryMasterTCPObj::onDNSFailed(%this)
{
	MessageBoxOK("Query Master Server Failed", "<just:left>DNS Failed during master server query.\n\n" @ "1.  Verify your internet connection\n\n" @ "2.  Make sure any security software you have is set to allow Blockland.exe to connect to the internet.");
	JS_queryStatus.setVisible(0);
}

function queryMasterTCPObj::onConnectFailed(%this)
{
	MessageBoxOK("Query Master Server Failed", "<just:left>Connection failed during master server query.\n\n" @ "1.  Verify your internet connection\n\n" @ "2.  Make sure any security software you have is set to allow Blockland.exe to connect to the internet.");
	JS_queryStatus.setVisible(0);
}

function queryMasterTCPObj::onLine(%this, %line)
{
	if (%this.done)
	{
		return;
	}
	if (%this.fileSize)
	{
		if (%this.gotHttpHeader)
		{
			%this.buffSize += strlen(%line) + 2;
			JS_statusBar.setValue(%this.buffSize / %this.fileSize);
		}
		else if (%line $= "")
		{
			%this.gotHttpHeader = 1;
		}
	}
	if (%line $= "END")
	{
		JS_queryStatus.setVisible(0);
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
		return;
	}
	if (%this.gotHeader)
	{
		if (getWord(%line, 0) !$= "**OLD**")
		{
			%ip = getField(%line, 0);
			%port = getField(%line, 1);
			%pass = getField(%line, 2);
			%ded = getField(%line, 3);
			%name = getField(%line, 4);
			%players = getField(%line, 5);
			%maxPlayers = getField(%line, 6);
			%mods = getField(%line, 7);
			%map = getField(%line, 8);
			%demoPlayers = getField(%line, 9);
			%ping = "???";
			if (%ded)
			{
				%ded = "Yes";
			}
			else
			{
				%ded = "No";
			}
			if (%pass)
			{
				%pass = "Yes";
			}
			else
			{
				%pass = "No";
			}
			ServerInfoSO_Add(%ip @ ":" @ %port, %pass, %ded, %name, %players, %maxPlayers, %mods, %map);
		}
	}
	else
	{
		%word = getWord(%line, 0);
		if (%word $= "Content-Length:")
		{
			%fileSize = getWord(%line, 1);
			%this.fileSize = %fileSize;
		}
	}
}

function JoinServerGui::queryLan(%this)
{
	JSG_demoBanner.setVisible(0);
	JSG_demoBanner2.setVisible(0);
	$JoinNetServer = 0;
	%flags = $Pref::Filter::Dedicated | $Pref::Filter::NoPassword << 1 | $Pref::Filter::LinuxServer << 2 | $Pref::Filter::WindowsServer << 3 | $Pref::Filter::TeamDamageOn << 4 | $Pref::Filter::TeamDamageOff << 5 | $Pref::Filter::CurrentVersion << 7;
	queryLanServers(28050, 0, $Client::GameTypeQuery, $Client::MissionTypeQuery, $pref::Filter::minPlayers, 100, 0, 2, $pref::Filter::maxPing, $pref::Filter::minCpu, %flags);
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
			if (%so.pass $= "Yes")
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
		echo("joining LAN game...");
		%id = JS_serverList.getSelectedId();
		if (!setServerInfo(%id))
		{
			return;
		}
	}
	if (isObject($conn))
	{
		$conn.cancelConnect();
		$conn.delete();
	}
	if ($ServerInfo::Password)
	{
		Canvas.pushDialog("joinServerPassGui");
	}
	else
	{
		deleteDataBlocks();
		Connecting_Text.setText("Connecting to " @ $ServerInfo::Address);
		Canvas.pushDialog(ConnectingGui);
		$conn = new GameConnection(ServerConnection);
		$conn.setConnectArgs($pref::Player::LANName, $pref::Player::NetName, $Pref::Player::ClanPrefix, $Pref::Player::ClanSuffix);
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
		JS_serverList.addRow(%i, ($ServerInfo::Password ? "Yes" : "No") TAB ($ServerInfo::Dedicated ? "D" : "L") TAB $ServerInfo::Name TAB $ServerInfo::Ping TAB $ServerInfo::PlayerCount TAB "/" TAB $ServerInfo::MaxPlayers TAB " " TAB $ServerInfo::MissionName TAB $ServerInfo::PlayerCount TAB %i);
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
}

function onServerQueryStatus(%status, %msg, %value)
{
	if (!JS_queryStatus.isVisible())
	{
		JS_queryStatus.setVisible(1);
	}
	if (%status $= "start")
	{
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
		ping = "???";
		ip = %ip;
		pass = %pass;
		ded = %ded;
		name = %name;
		currPlayers = %currPlayers;
		maxPlayers = %maxPlayers;
		mods = %mods;
		map = %map;
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
		%doRow = 1;
		if ($Pref::Filter::Dedicated && %obj.ded !$= "Yes")
		{
			%doRow = 0;
		}
		if ($Pref::Filter::NoPassword && %obj.pass $= "Yes")
		{
			%doRow = 0;
		}
		if ($Pref::Filter::NotEmpty && %obj.currPlayers <= 0)
		{
			%doRow = 0;
		}
		if ($Pref::Filter::NotFull && %obj.currPlayers >= %obj.maxPlayers)
		{
			%doRow = 0;
		}
		if (%obj.ping $= "Dead")
		{
			%doRow = 0;
		}
		if (%obj.ping !$= "???")
		{
			if (%obj.ping > $pref::Filter::maxPing)
			{
				%doRow = 0;
			}
		}
		if (%doRow)
		{
			%rowText = %obj.serialize();
			JS_serverList.addRow(%i, %rowText);
		}
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
		%obj.ping = %ping;
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
	%ret = %this.pass TAB %this.ded TAB %this.name TAB %this.ping TAB %this.currPlayers TAB "/" TAB %this.maxPlayers TAB %this.mods TAB %this.map TAB %this.ip;
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

function JoinServerPassGui::enterPass(%this)
{
	%pass = JSP_txtPass.getValue();
	if (%pass !$= "")
	{
		deleteDataBlocks();
		Connecting_Text.setText("Connecting to " @ $ServerInfo::Address);
		Canvas.pushDialog(ConnectingGui);
		$conn = new GameConnection(ServerConnection);
		$conn.setConnectArgs($pref::Player::LANName, $pref::Player::NetName, $Pref::Player::ClanPrefix, $Pref::Player::ClanSuffix);
		$conn.setJoinPassword(%pass);
		$conn.connect($ServerInfo::Address);
		JSP_txtPass.setValue("");
	}
}

function filtersGui::onWake()
{
	Filter_PingMenu.clear();
	Filter_PingMenu.add(50, 50);
	Filter_PingMenu.add(100, 100);
	Filter_PingMenu.add(150, 150);
	Filter_PingMenu.add(250, 250);
	Filter_PingMenu.add(450, 450);
	Filter_PingMenu.add(999, 999);
	Filter_PingMenu.setSelected(mFloor($pref::Filter::maxPing));
	if (Filter_PingMenu.getSelected() <= 0)
	{
		Filter_PingMenu.setSelected(999);
	}
}

function filtersGui::onSleep()
{
	$pref::Filter::maxPing = Filter_PingMenu.getSelected();
	if ($JoinNetServer)
	{
		ServerInfoSO_DisplayAll();
	}
}

function noHudGui::onWake(%this)
{
	$enableDirectInput = 1;
	activateDirectInput();
	moveMap.push();
	schedule(0, 0, "refreshCenterTextCtrl");
	schedule(0, 0, "refreshBottomTextCtrl");
}

function noHudGui::onSleep(%this)
{
	moveMap.pop();
}

function changeMapGui::onWake(%this)
{
	if (changeMapList.rowCount() <= 0)
	{
		changeMapList.clear();
		commandToServer('getMapList');
	}
}

function changeMapGui::onSleep(%this)
{
}

function changeMapList::select(%this)
{
	%selectedID = %this.getSelectedId();
	%row = %this.getRowTextById(%selectedID);
	%name = getField(%row, 0);
	%picture = getField(%row, 1);
	%fileName = getField(%row, 2);
	%description = %this.description[%selectedID];
	changeMapPreview.setBitmap(%picture);
	changeMapName.setValue(%name);
	if (strlen(%description) > 1)
	{
		changeMapDescription.setValue(%description);
	}
	else
	{
		%description = getMissionDescription(%fileName);
		if (strlen(%description) > 1)
		{
			changeMapDescription.setText(%description);
			%this.description[%selectedID] = %description;
		}
	}
}

function changeMapButton::click(%this)
{
	%row = changeMapList.getRowTextById(changeMapList.getSelectedId());
	%fileName = getField(%row, 2);
	if (strlen(%fileName) > 1)
	{
		commandToServer('changeMap', %fileName);
	}
}

function clientCmdUpdateMapList(%entry)
{
	changeMapList.addRow(changeMapList.rowCount(), %entry);
	changeMapList.sort(0);
	changeMapList.setSelectedRow(0);
}

function clientCmdClearMapList()
{
	changeMapList.clear();
	for (%i = 0; changeMapList.description[%i] !$= ""; %i++)
	{
		changeMapList.description[%i] = "";
	}
}

function adminGui::kick()
{
	%victimID = lstAdminPlayerList.getSelectedId();
	commandToServer('kick', %victimID);
}

function adminGui::ban()
{
	%victimID = lstAdminPlayerList.getSelectedId();
	commandToServer('ban', %victimID);
}

function adminGui::spy()
{
	%victimID = lstAdminPlayerList.getSelectedId();
	%victimName = lstAdminPlayerList.getRowTextById(%victimID);
	if (%victimID <= 0)
	{
		return;
	}
	commandToServer('spy', %victimID);
}

function adminGui::changeMap()
{
}

function adminGui::ClickClearBricks()
{
	if ($RemoteServer::LAN)
	{
		MessageBoxYesNo("Clear Bricks?", "Are you sure you want to delete all bricks?", "commandToServer('ClearAllBricks');canvas.popDialog(AdminGui);");
	}
	else
	{
		Canvas.pushDialog(BrickManGui);
		Canvas.popDialog(adminGui);
		Canvas.popDialog(escapeMenu);
	}
}

function AdminGui_Wand()
{
	commandToServer('MagicWand');
	Canvas.popDialog(adminGui);
	Canvas.popDialog(escapeMenu);
}

function AdminGui_ClearBricks()
{
}

function AdminGui_KickPlayer()
{
	%victimID = lstAdminPlayerList.getSelectedId();
	if (%victimID <= 0)
	{
		return;
	}
	%victimName = getField(lstAdminPlayerList.getRowTextById(%victimID), 0);
	MessageBoxYesNo("Kick Player?", "Are you sure you want to kick \"" @ %victimName @ "\" ?", "commandToServer('kick'," @ %victimID @ ");canvas.popDialog(AdminGui);");
}

function AdminGui_BanPlayer()
{
	%victimID = lstAdminPlayerList.getSelectedId();
	%victimName = getField(lstAdminPlayerList.getRowTextById(%victimID), 0);
	%victimBL_ID = getField(lstAdminPlayerList.getRowTextById(%victimID), 1);
	if (%victimID <= 0)
	{
		return;
	}
	addBanGui.setVictim(%victimName, %victimID, %victimBL_ID);
	Canvas.pushDialog(addBanGui);
}

function addBanGui::onWake(%this)
{
	if (!$addBanGui::loaded)
	{
		$addBanGui::loaded = 1;
		AddBan_Days.clear();
		AddBan_Days.add("0 Days", 0);
		AddBan_Days.add("1 Day", 1);
		for (%i = 2; %i < 16; %i++)
		{
			AddBan_Days.add(%i @ " Days", %i);
		}
		AddBan_Hours.clear();
		AddBan_Hours.add("0 Hours", 0);
		AddBan_Hours.add("1 Hour", 1);
		for (%i = 2; %i < 24; %i++)
		{
			AddBan_Hours.add(%i @ " Hours", %i);
		}
		AddBan_Minutes.clear();
		AddBan_Minutes.add("0 Minutes", 0);
		AddBan_Minutes.add("1 Minute", 1);
		for (%i = 2; %i < 60; %i++)
		{
			AddBan_Minutes.add(%i @ " Minutes", %i);
		}
		AddBan_Days.setSelected(0);
		AddBan_Hours.setSelected(0);
		AddBan_Minutes.setSelected(0);
		AddBan_Forever.setValue(0);
		AddBan_TimeBlocker.setVisible(0);
	}
}

function addBanGui::onSleep(%this)
{
}

function addBanGui::setVictim(%this, %name, %id, %bl_id)
{
	addBan_Window.setText("BAN " @ %name);
	addBanGui.victimName = %name;
	addBanGui.victimId = %id;
	addBanGui.victimBL_ID = %bl_id;
}

function addBanGui::clickForever()
{
	echo(AddBan_Forever.getValue());
	if (AddBan_Forever.getValue())
	{
		AddBan_TimeBlocker.setVisible(1);
	}
	else
	{
		AddBan_TimeBlocker.setVisible(0);
	}
}

function addBanGui::ban()
{
	%banTime = 0;
	if (AddBan_Forever.getValue())
	{
		%banTime = -1;
	}
	else
	{
		%banTime = AddBan_Minutes.getSelected();
		%banTime += AddBan_Hours.getSelected() * 60;
		%banTime += AddBan_Days.getSelected() * 60 * 24;
		if (%banTime <= 0)
		{
			return;
		}
	}
	%reason = addBan_reason.getValue();
	commandToServer('Ban', addBanGui.victimId, addBanGui.victimBL_ID, %banTime, %reason);
	Canvas.popDialog(addBanGui);
}

function unBanGui::onWake(%this)
{
	unBan_list.clear();
	commandToServer('RequestBanList');
}

function unBanGui::onSleep(%this)
{
}

function unBanGui::clickUnBan(%this)
{
	%rowID = unBan_list.getSelectedId();
	if (%rowID < 0)
	{
		return;
	}
	%row = unBan_list.getRowTextById(%rowID);
	%victimName = getField(%row, 1);
	MessageBoxYesNo("Un-Ban Player?", "Are you sure you want to Un-Ban \"" @ %victimName @ "\" ?", "unBanGui.unBan(" @ %rowID @ ");");
}

function unBanGui::unBan(%this, %idx)
{
	unBan_list.clear();
	commandToServer('unBan', %idx);
}

function clientCmdClearUnBans()
{
	unBan_list.clear();
}

function clientCmdAddUnBanLine(%line, %idx)
{
	echo("clientCmdAddUnBanLine() - ", %line);
	%adminName = getField(%line, 0);
	%victimName = getField(%line, 1);
	%victimBL_ID = getField(%line, 2);
	%victimIP = getField(%line, 3);
	%reason = getField(%line, 4);
	%timeMinutes = getField(%line, 5);
	%timeString = "1:00";
	if (%timeMinutes == -1)
	{
		%timeString = "Forever";
	}
	else if (%timeMinutes > 24 * 60)
	{
		%numDays = mFloor(%timeMinutes / (24 * 60));
		%timeLeftover = %timeMinutes % (24 * 60);
		%timeString = %numDays @ "d " @ getTimeString(%timeLeftover);
	}
	else
	{
		%timeString = getTimeString(%timeMinutes);
	}
	%row = %adminName;
	%row = %row TAB %victimName;
	%row = %row TAB %victimBL_ID;
	%row = %row TAB %victimIP;
	%row = %row TAB %reason;
	%row = %row TAB %timeString;
	%row = %row TAB %timeMinutes;
	unBan_list.addRow(%idx, %row);
}

function unBanGui::sortList(%this, %col)
{
	unBan_list.sortedNumerical = 0;
	if (unBan_list.sortedBy == %col)
	{
		unBan_list.sortedAsc = !unBan_list.sortedAsc;
		unBan_list.sort(unBan_list.sortedBy, unBan_list.sortedAsc);
	}
	else
	{
		unBan_list.sortedBy = %col;
		unBan_list.sortedAsc = 0;
		unBan_list.sort(unBan_list.sortedBy, unBan_list.sortedAsc);
	}
}

function unBanGui::sortNumList(%this, %col)
{
	unBan_list.sortedNumerical = 1;
	if (unBan_list.sortedBy == %col)
	{
		unBan_list.sortedAsc = !unBan_list.sortedAsc;
		unBan_list.sortNumerical(unBan_list.sortedBy, unBan_list.sortedAsc);
	}
	else
	{
		unBan_list.sortedBy = %col;
		unBan_list.sortedAsc = 0;
		unBan_list.sortNumerical(unBan_list.sortedBy, unBan_list.sortedAsc);
	}
}

function BrickManGui::onWake(%this)
{
	BrickMan_list.clear();
	commandToServer('RequestBrickManList');
}

function BrickManGui::onSleep(%this)
{
}

function clientCmdClearBrickMan()
{
	BrickMan_list.clear();
}

function clientCmdAddBrickManLine(%bl_id, %line)
{
	if (BrickMan_list.getRowTextById(%bl_id) $= "")
	{
		BrickMan_list.addRow(%bl_id, %line);
	}
	else
	{
		BrickMan_list.setRowById(%bl_id, %line);
	}
}

function BrickManGui::clickClear(%this)
{
	%id = BrickMan_list.getSelectedId();
	%row = BrickMan_list.getRowTextById(%id);
	%name = getField(%row, 1);
	%brickCount = getField(%row, 2);
	if (%row $= "")
	{
		return;
	}
	MessageBoxYesNo("Clear Bricks?", "Are you sure you want to destroy all of " @ %name @ "'s bricks?", "BrickManGui.clearBrickGroup(" @ %id @ ");");
}

function BrickManGui::clearBrickGroup(%this, %bl_id)
{
	BrickMan_list.clear();
	commandToServer('ClearBrickGroup', %bl_id);
}

function BrickManGui::clickClearAll(%this)
{
	MessageBoxYesNo("Clear ALL Bricks?", "Are you sure you want to destroy ALL of the bricks?", "brickManGui.clearAllBricks();");
}

function BrickManGui::clearAllBricks(%this)
{
	BrickMan_list.clear();
	commandToServer('ClearAllBricks');
}

function BrickManGui::clickHilight(%this)
{
	%id = BrickMan_list.getSelectedId();
	commandToServer('HilightBrickGroup', %id);
}

function BrickManGui::clickBan(%this)
{
	%row = BrickMan_list.getRowTextById(BrickMan_list.getSelectedId());
	echo("row = ", %row);
	if (%row $= "")
	{
		return;
	}
	%victimID = 0;
	%victimName = getField(%row, 1);
	%victimBL_ID = getField(%row, 0);
	addBanGui.setVictim(%victimName, %victimID, %victimBL_ID);
	Canvas.pushDialog(addBanGui);
}

function BrickManGui::sortList(%this, %col)
{
	BrickMan_list.sortedNumerical = 0;
	if (BrickMan_list.sortedBy == %col)
	{
		BrickMan_list.sortedAsc = !BrickMan_list.sortedAsc;
		BrickMan_list.sort(BrickMan_list.sortedBy, BrickMan_list.sortedAsc);
	}
	else
	{
		BrickMan_list.sortedBy = %col;
		BrickMan_list.sortedAsc = 0;
		BrickMan_list.sort(BrickMan_list.sortedBy, BrickMan_list.sortedAsc);
	}
}

function BrickManGui::sortNumList(%this, %col)
{
	BrickMan_list.sortedNumerical = 1;
	if (BrickMan_list.sortedBy == %col)
	{
		BrickMan_list.sortedAsc = !BrickMan_list.sortedAsc;
		BrickMan_list.sortNumerical(BrickMan_list.sortedBy, BrickMan_list.sortedAsc);
	}
	else
	{
		BrickMan_list.sortedBy = %col;
		BrickMan_list.sortedAsc = 0;
		BrickMan_list.sortNumerical(BrickMan_list.sortedBy, BrickMan_list.sortedAsc);
	}
}

function escapeMenu::toggle(%this)
{
	if (%this.isAwake())
	{
		Canvas.popDialog(%this);
	}
	else if (Canvas.getContent().getName() $= "LoadingGui")
	{
		disconnect();
	}
	else
	{
		Canvas.pushDialog(%this);
	}
}

function escapeMenu::onWake(%this)
{
	if ($Pref::Gui::ColorEscapeMenu)
	{
		EM_Options.mColor = "255 255 255 255";
		EM_PlayerList.mColor = " 50 255  50 255";
		EM_MiniGames.mColor = "170 170 170 255";
		EM_AdminMenu.mColor = "255 255  50 255";
		EM_SaveBricks.mColor = " 50 130 255 255";
		EM_LoadBricks.mColor = " 50 255 255 255";
		EM_Disconnect.mColor = "255 150   0 255";
		EM_Quit.mColor = "255  75   0 255";
	}
	else
	{
		EM_Options.mColor = "255 255 255 255";
		EM_PlayerList.mColor = "255 255 255 255";
		EM_MiniGames.mColor = "255 255 255 255";
		EM_AdminMenu.mColor = "255 255 255 255";
		EM_SaveBricks.mColor = "255 255 255 255";
		EM_LoadBricks.mColor = "255 255 255 255";
		EM_Disconnect.mColor = "255 255 255 255";
		EM_Quit.mColor = "255 255 255 255";
	}
}

function escapeMenu::clickAdmin()
{
	if ($IamAdmin == 1 || isObject(ServerGroup))
	{
		Canvas.pushDialog("adminGui");
	}
	else
	{
		$AdminCallback = "canvas.pushDialog(admingui);";
		Canvas.pushDialog("adminLoginGui");
	}
}

function escapeMenu::clickLoadBricks()
{
	if ($IamAdmin == 1 || isObject(ServerGroup))
	{
		Canvas.pushDialog("loadBricksGui");
	}
	else
	{
		$AdminCallback = "canvas.pushDialog(loadBricksGui);";
		Canvas.pushDialog("adminLoginGui");
	}
}

function escapeMenu::clickSaveBricks()
{
	if ($Pref::Gui::IgnoreRemoteSaveWarning || isObject(ServerGroup))
	{
		Canvas.pushDialog(saveBricksGui);
	}
	else
	{
		Canvas.pushDialog(saveBricksWarningGui);
	}
}

function escapeMenu::clickMinigames()
{
	if ($RunningMiniGame)
	{
		Canvas.pushDialog(CreateMiniGameGui);
	}
	else
	{
		Canvas.pushDialog(joinMiniGameGui);
	}
	Canvas.popDialog(escapeMenu);
}

function EM_Options::onMouseEnter(%this)
{
	alxPlay(Note0Sound);
}

function EM_PlayerList::onMouseEnter(%this)
{
	alxPlay(Note1Sound);
}

function EM_MiniGames::onMouseEnter(%this)
{
	alxPlay(Note2Sound);
}

function EM_AdminMenu::onMouseEnter(%this)
{
	alxPlay(Note3Sound);
}

function EM_SaveBricks::onMouseEnter(%this)
{
	alxPlay(Note4Sound);
}

function EM_LoadBricks::onMouseEnter(%this)
{
	alxPlay(Note5Sound);
}

function EM_Disconnect::onMouseEnter(%this)
{
	alxPlay(Note6Sound);
}

function EM_Quit::onMouseEnter(%this)
{
	alxPlay(Note7Sound);
}

function AdminLoginGui::onWake()
{
	txtAdminPass.setText("");
}

function AdminLoginGui::onSleep()
{
	txtAdminPass.setText("");
}

function sendAdminLogin()
{
	SAD(txtAdminPass.getValue());
}

function clientCmdAdminSuccess()
{
	$IamAdmin = 1;
	Canvas.popDialog(AdminLoginGui);
	eval($AdminCallback);
}

function clientCmdAdminFailure()
{
	if ($sendAdminPass == 1)
	{
		MessageBoxOK("Login Failure", "Wrong Password");
	}
}

function clientCmdSetLetterPrintInfo(%start, %numLetters)
{
	$PSD_letterStart = %start;
	$PSD_numLetters = %numLetters;
}

function clientCmdOpenPrintSelectorDlg(%aspectRatio, %startPrint, %numPrints)
{
	if (PSD_Window.scrollcount $= "")
	{
		PSD_Window.scrollcount = 0;
	}
	if (!isObject("PSD_PrintScroller" @ %aspectRatio))
	{
		PSD_LoadPrints(%aspectRatio, %startPrint, %numPrints);
	}
	if (!isObject("PSD_PrintScrollerLetters"))
	{
		PSD_LoadPrints("Letters", $PSD_letterStart, $PSD_numLetters);
	}
	Canvas.pushDialog("printSelectorDlg");
	if ($PSD_LettersVisible)
	{
		PSD_PrintScrollerLetters.setVisible(1);
	}
	else
	{
		%cmdString = "PSD_PrintScroller" @ %aspectRatio @ ".setVisible(true);";
		eval(%cmdString);
	}
	$PSD_CurrentAR = %aspectRatio;
}

function PrintSelectorDlg::onSleep(%this)
{
	if (PSD_PrintScrollerLetters.visible == 1)
	{
		$PSD_LettersVisible = 1;
	}
	else
	{
		$PSD_LettersVisible = 0;
	}
	for (%i = 0; %i < PSD_Window.scrollcount; %i++)
	{
		PSD_Window.Scroll[%i].setVisible(0);
	}
}

function ClientCmdPSD_KillPrints()
{
	PSD_KillPrints();
}

function PSD_KillPrints()
{
	for (%i = 0; %i < PSD_Window.scrollcount; %i++)
	{
		if (isObject(PSD_Window.Scroll[%i]))
		{
			PSD_Window.Scroll[%i].delete();
		}
	}
	PSD_Window.scrollcount = 0;
}

function PSD_click(%number)
{
	commandToServer('setPrint', %number);
	Canvas.popDialog(PrintSelectorDlg);
}

function PSD_LettersTab()
{
	PSD_PrintScrollerLetters.setVisible(1);
	%cmdString = "PSD_PrintScroller" @ $PSD_CurrentAR @ ".setVisible(false);";
	eval(%cmdString);
}

function PSD_PrintsTab()
{
	PSD_PrintScrollerLetters.setVisible(0);
	%cmdString = "PSD_PrintScroller" @ $PSD_CurrentAR @ ".setVisible(true);";
	eval(%cmdString);
}

function PSD_LoadPrints(%aspectRatio, %startPrint, %numPrints)
{
	%scrollName = "PSD_PrintScroller" @ %aspectRatio;
	%scrollObj = new GuiScrollCtrl();
	PSD_Window.add(%scrollObj);
	PSD_Window.Scroll[PSD_Window.scrollcount] = %scrollObj;
	PSD_Window.scrollcount++;
	%scrollObj.rowHeight = 65;
	%scrollObj.setName(%scrollName);
	%scrollObj.hScrollBar = "alwaysOff";
	%scrollObj.vScrollBar = "alwaysOn";
	%scrollObj.setProfile(ColorScrollProfile);
	%scrollObj.resize(6, 42, 205, 392);
	%boxName = "PSD_PrintBox" @ %aspectRatio;
	%boxObj = new GuiControl();
	%scrollObj.add(%boxObj);
	%boxObj.setName(%boxName);
	%boxObj.setProfile(ColorScrollProfile);
	%boxObj.resize(0, 0, 64, 64);
	%Xsize = 64;
	%Ysize = 64;
	%numColumns = 3;
	for (%i = %startPrint; %i < %startPrint + %numPrints; %i++)
	{
		%rawFileName = getPrintTexture(%i);
		%fileName = strreplace(%rawFileName, "prints", "printIcons");
		%newPrint = new GuiBitmapCtrl();
		%boxObj.add(%newPrint);
		%newPrint.keepCached = 1;
		%newPrint.setBitmap(%fileName);
		%x = (%Xsize + 1) * %columnCount;
		%y = (%Ysize + 1) * %rowCount;
		%w = %Xsize;
		%h = %Ysize;
		%newPrint.resize(%x, %y, %w, %h);
		%newButton = new GuiBitmapButtonCtrl();
		%boxObj.add(%newButton);
		%newButton.keepCached = 1;
		%newButton.setProfile(BlockButtonProfile);
		%newButton.setBitmap("base/client/ui/btnPrint");
		%newButton.setText(" ");
		%newButton.command = "PSD_click(" @ %i @ ");";
		%x = (%Xsize + 1) * %columnCount;
		%y = (%Ysize + 1) * %rowCount;
		%w = %Xsize;
		%h = %Ysize;
		%newButton.resize(%x, %y, %w, %h);
		%baseName = fileBase(%fileName);
		if (strlen(%baseName) == 1)
		{
			%newButton.accelerator = %baseName;
		}
		if (%baseName $= "-bang")
		{
			%newButton.accelerator = "shift 1";
		}
		if (%baseName $= "-at")
		{
			%newButton.accelerator = "shift 2";
		}
		if (%baseName $= "-pound")
		{
			%newButton.accelerator = "shift 3";
		}
		if (%baseName $= "-dollar")
		{
			%newButton.accelerator = "shift 4";
		}
		if (%baseName $= "-percent")
		{
			%newButton.accelerator = "shift 5";
		}
		if (%baseName $= "-caret")
		{
			%newButton.accelerator = "shift 6";
		}
		if (%baseName $= "-and")
		{
			%newButton.accelerator = "shift 7";
		}
		if (%baseName $= "-asterisk")
		{
			%newButton.accelerator = "shift 8";
		}
		if (%baseName $= "-minus")
		{
			%newButton.accelerator = "-";
		}
		if (%baseName $= "-equals")
		{
			%newButton.accelerator = "=";
		}
		if (%baseName $= "-plus")
		{
			%newButton.accelerator = "shift =";
		}
		if (%baseName $= "-period")
		{
			%newButton.accelerator = ".";
		}
		if (%baseName $= "-less than")
		{
			%newButton.accelerator = "shift ,";
		}
		if (%baseName $= "-greater than")
		{
			%newButton.accelerator = "shift .";
		}
		if (%baseName $= "-qmark")
		{
			%newButton.accelerator = "shift /";
		}
		if (%baseName $= "-apostrophe")
		{
			%newButton.accelerator = "'";
		}
		if (%baseName $= "-space")
		{
			%newButton.accelerator = "space";
		}
		%x = 0;
		%y = 0;
		%w = (%Xsize + 1) * %numColumns;
		%h = (%Ysize + 1) * (%rowCount + 1);
		%boxObj.resize(%x, %y, %w, %h);
		%columnCount++;
		if (%columnCount >= %numColumns)
		{
			%rowCount++;
			%columnCount = 0;
		}
	}
	%scrollObj.setVisible(0);
}

function BrickSelectorDlg::onWake(%this)
{
	$BSD_CurrClickData = -1;
	$BSD_CurrClickInv = -1;
	BSD_FavsHelper.setVisible(0);
	BSD_SetFavsButton.setText("Set Favs>");
	HUD_BrickBox.setVisible(0);
	HUD_BrickNameBG.setVisible(0);
	HUD_PaintBox.setVisible(0);
	HUD_PaintNameBG.setVisible(0);
	HUD_ToolBox.setVisible(0);
	HUD_ToolNameBG.setVisible(0);
	commandToServer('BSD');
}

function BrickSelectorDlg::onSleep(%this)
{
	if ($BSD_CurrClickData != -1)
	{
		$BSD_activeBitmap[$BSD_CurrClickData].setVisible(0);
	}
	if ($BSD_CurrClickInv != -1)
	{
		$BSD_InvActive[$BSD_CurrClickInv].setVisible(0);
	}
	$BSD_CurrClickData = -1;
	$BSD_CurrClickInv = -1;
	HUD_BrickBox.setVisible(1);
	HUD_BrickNameBG.setVisible(1);
	HUD_PaintBox.setVisible(1);
	HUD_PaintNameBG.setVisible(1);
	HUD_ToolBox.setVisible(1);
	HUD_ToolNameBG.setVisible(1);
}

function clientCmdBSD_Open()
{
	Canvas.pushDialog(BrickSelectorDlg);
}

function clientCmdBSD_LoadBricks()
{
	BSD_LoadBricks();
	BSD_LoadFavorites();
	$UINameTableCreated = 0;
	$PrintNameTableCreated = 0;
	BSD_Window.pushToBack(BSD_FavsHelper);
	PlayGui.createInvHud();
}

function BSD_LoadBricks()
{
	BSD_KillBricks();
	$BSD_numCategories = 0;
	%dbCount = getDataBlockGroupSize();
	for (%i = 0; %i < %dbCount; %i++)
	{
		%db = getDataBlock(%i);
		%dbClass = %db.getClassName();
		if (%dbClass $= "fxDTSBrickData")
		{
			%cat = %db.category;
			%subCat = %db.subCategory;
			BSD_addCategory(%cat);
			%subCatObj = BSD_addSubCategory(%cat, %subCat);
			%subCatObj.numBricks++;
		}
	}
	%newScrollBox = new GuiControl();
	BSD_Window.add(%newScrollBox);
	%x = 3;
	%y = 57;
	%w = 634;
	%h = 363;
	%newScrollBox.resize(%x, %y, %w, %h);
	%newScrollBox.setName("BSD_ScrollBox");
	%newTabBox = new GuiControl();
	BSD_Window.add(%newTabBox);
	%x = 3;
	%y = 30;
	%w = 634;
	%h = 25;
	%newTabBox.resize(%x, %y, %w, %h);
	%newTabBox.setName("BSD_TabBox");
	for (%i = 0; %i < $BSD_numCategories; %i++)
	{
		%newTab = new GuiBitmapButtonCtrl();
		BSD_TabBox.add(%newTab);
		%newTab.setProfile(BlockButtonProfile);
		%x = %i * 80;
		%y = 0;
		%w = 80;
		%h = 25;
		%newTab.resize(%x, %y, %w, %h);
		%newTab.setText($BSD_category[%i].name);
		%newTab.setBitmap("base/client/ui/tab1");
		%newTab.command = "BSD_ShowTab(" @ %i @ ");";
		%newScroll = new GuiScrollCtrl();
		BSD_ScrollBox.add(%newScroll);
		%newScroll.rowHeight = 64;
		%newScroll.hScrollBar = "alwaysOff";
		%newScroll.vScrollBar = "alwaysOn";
		%newScroll.setProfile(BSDScrollProfile);
		%newScroll.defaultLineHeight = 32;
		%x = 0;
		%y = 0;
		%w = 634;
		%h = 363;
		%newScroll.resize(%x, %y, %w, %h);
		%newBox = new GuiControl();
		%newScroll.add(%newBox);
		%newBox.setProfile(ColorScrollProfile);
		%x = 0;
		%y = 0;
		%w = 0;
		%h = 0;
		%newBox.resize(%x, %y, %w, %h);
		$BSD_category[%i].tab = %newTab;
		$BSD_category[%i].Scroll = %newScroll;
		$BSD_category[%i].box = %newBox;
		BSD_createSubHeadings($BSD_category[%i]);
	}
	for (%i = 0; %i < %dbCount; %i++)
	{
		%db = getDataBlock(%i);
		%dbClass = %db.getClassName();
		if (%dbClass $= "fxDTSBrickData")
		{
			BSD_CreateBrickButton(%db);
		}
	}
	BSD_CreateInventoryButtons();
	BSD_Window.pushToBack(BSD_ClearBtn);
	BSD_ShowTab(0);
}

function BSD_DumpCategories()
{
	for (%i = 0; %i < $BSD_numCategories; %i++)
	{
		echo($BSD_category[%i].name);
		for (%j = 0; %j < $BSD_category[%i].numSubCategories; %j++)
		{
			echo("  ", $BSD_category[%i].subCategory[%j].name);
		}
	}
}

function BSD_KillBricks()
{
	%numCats = $BSD_numCategories;
	for (%i = 0; %i < %numCats; %i++)
	{
		$BSD_category[%i].delete();
	}
	$BSD_numCategories = 0;
	if (isObject(BSD_InvBox))
	{
		BSD_InvBox.delete();
	}
	if (isObject(BSD_TabBox))
	{
		BSD_TabBox.delete();
	}
	if (isObject(BSD_ScrollBox))
	{
		BSD_ScrollBox.delete();
	}
	for (%i = 0; %i < $BSD_NumInventorySlots; %i++)
	{
		$BSD_InvData[%i] = -1;
		$BSD_InvIcon[%i] = -1;
	}
}

function BSD_category::onRemove(%this)
{
	%this.tab.delete();
	%this.Scroll.delete();
	for (%j = 0; %j < %this.numSubCategories; %j++)
	{
		if (isObject(%this.subCategory[%j]))
		{
			%this.subCategory[%j].delete();
		}
	}
	$BSD_numCategories--;
}

function BSD_addCategory(%newcat)
{
	for (%i = 0; %i < $BSD_numCategories; %i++)
	{
		if ($BSD_category[%i].name $= %newcat)
		{
			return;
		}
	}
	$BSD_category[$BSD_numCategories] = new ScriptObject()
	{
		class = BSD_category;
		name = %newcat;
		numSubCategories = 0;
	};
	RootGroup.add($BSD_category[$BSD_numCategories]);
	$BSD_numCategories++;
}

function BSD_addSubCategory(%cat, %newSubCat)
{
	%catID = -1;
	for (%i = 0; %i < $BSD_numCategories; %i++)
	{
		if ($BSD_category[%i].name $= %cat)
		{
			%catID = %i;
			break;
		}
	}
	if (%catID == -1)
	{
		error("Error: BSD_addSubCategory - category \"", %cat, "\" not found.");
		return;
	}
	else
	{
		for (%i = 0; %i < $BSD_category[%catID].numSubCategories; %i++)
		{
			if ($BSD_category[%catID].subCategory[%i].name $= %newSubCat)
			{
				return $BSD_category[%catID].subCategory[%i];
			}
		}
		$BSD_category[%catID].subCategory[$BSD_category[%catID].numSubCategories] = new ScriptObject()
		{
			name = %newSubCat;
			numBrickButtons = 0;
		};
		RootGroup.add($BSD_category[%catID].subCategory[$BSD_category[%catID].numSubCategories]);
		$BSD_category[%catID].numSubCategories++;
		return $BSD_category[%catID].subCategory[$BSD_category[%catID].numSubCategories - 1];
	}
}

function BSD_findCategory(%catName)
{
	for (%i = 0; %i < $BSD_numCategories; %i++)
	{
		if ($BSD_category[%i].name $= %catName)
		{
			return $BSD_category[%i];
		}
	}
}

function BSD_findSubCategory(%catObj, %subCatName)
{
	for (%i = 0; %i < %catObj.numSubCategories; %i++)
	{
		if (%catObj.subCategory[%i].name $= %subCatName)
		{
			return %catObj.subCategory[%i];
		}
	}
}

function BSD_createSubHeadings(%cat)
{
	%box = %cat.box;
	for (%i = 0; %i < %cat.numSubCategories; %i++)
	{
		%subCatObj = %cat.subCategory[%i];
		%boxExtent = %box.getExtent();
		%boxMinExtent = %box.getMinExtent();
		%boxHeight = getWord(%boxExtent, 1) - getWord(%boxMinExtent, 1);
		%subCatObj.startHeight = %boxHeight;
		%newBar = new GuiBitmapCtrl();
		%box.add(%newBar);
		%newBar.keepCached = 1;
		%x = 0 + 18;
		%y = %subCatObj.startHeight + 15;
		%w = 581;
		%h = 1;
		%newBar.resize(%x, %y, %w, %h);
		%newHeading = new GuiTextCtrl();
		%box.add(%newHeading);
		%newHeading.setProfile(BlockButtonProfile);
		%newHeading.setText(%subCatObj.name);
		%x = 0 + 18;
		%y = %subCatObj.startHeight - 2;
		%w = 100;
		%h = 18;
		%newHeading.resize(%x, %y, %w, %h);
		%numRows = mCeil(%subCatObj.numBricks / 6);
		%x = 0;
		%y = 0;
		%w = 634;
		%h = %boxHeight + 18 + %numRows * 96 + 5;
		%box.resize(%x, %y, %w, %h);
	}
}

function BSD_CreateBrickButton(%data)
{
	%catName = %data.category;
	%subCatName = %data.subCategory;
	%catObj = BSD_findCategory(%catName);
	%subCatObj = BSD_findSubCategory(%catObj, %subCatName);
	if (%catObj == 0 || %subCatObj == 0)
	{
		error("ERROR: BSD_CreateBrickButton - Couldnt find category objects");
		return;
	}
	%brickName = %data.uiName;
	%brickIcon = %data.iconName;
	if (%brickName $= "")
	{
		%brickName = "No Name";
	}
	if (!isFile(%brickIcon @ ".png"))
	{
		%brickIcon = "base/client/ui/brickIcons/unknown";
	}
	%box = %catObj.box;
	%x = (%subCatObj.numBrickButtons % 6) * 97 + 18;
	%y = mFloor(%subCatObj.numBrickButtons / 6) * 97 + %subCatObj.startHeight + 18;
	%subCatObj.numBrickButtons++;
	%newIconBG = new GuiBitmapCtrl();
	%box.add(%newIconBG);
	%newIconBG.resize(%x, %y, 96, 96);
	%newIconBG.keepCached = 1;
	%newIconBG.setBitmap("base/client/ui/brickicons/brickiconbg");
	%newIconBG.setProfile(BlockDefaultProfile);
	%newIcon = new GuiBitmapCtrl();
	%box.add(%newIcon);
	%newIcon.resize(%x, %y, 96, 96);
	%newIcon.keepCached = 1;
	%newIcon.setBitmap(%brickIcon);
	%newIcon.setProfile(BlockDefaultProfile);
	%newActive = new GuiBitmapCtrl();
	%box.add(%newActive);
	%newActive.resize(%x, %y, 96, 96);
	%newActive.keepCached = 1;
	%newActive.setBitmap("base/client/ui/brickicons/brickIconActive");
	%newActive.setProfile(BlockDefaultProfile);
	%newActive.setVisible(0);
	$BSD_activeBitmap[%data] = %newActive;
	%newIconButton = new GuiBitmapButtonCtrl();
	%box.add(%newIconButton);
	%newIconButton.resize(%x, %y, 96, 96);
	%newIconButton.setBitmap("base/client/ui/brickicons/brickIconBtn");
	%newIconButton.setProfile(BlockButtonProfile);
	%newIconButton.setText(" ");
	%newIconButton.command = "BSD_ClickIcon(" @ %data @ ");";
	%newIconButton.altCommand = "BSD_RightClickIcon(" @ %data @ ");";
	%newLabel = new GuiTextCtrl();
	%box.add(%newLabel);
	%w = 96;
	%h = 18;
	%x = %x;
	%y = (%y + 96) - %h;
	%newLabel.resize(%x, %y, %w, %h);
	%newLabel.setProfile(HUDBSDNameProfile);
	%newLabel.setText(%brickName);
}

function BSD_KillInventoryButtons()
{
	if (isObject(BSD_InvBox))
	{
		BSD_InvBox.delete();
	}
}

function BSD_CreateInventoryButtons()
{
	$BSD_NumInventorySlots = 10;
	%invWidth = 617;
	%buttonWidth = mFloor(%invWidth / 11) - 1;
	%newBox = new GuiSwatchCtrl();
	BSD_Window.add(%newBox);
	%newBox.setColor("0.2 0.5 1 1");
	%x = 3;
	%y = (480 - %buttonWidth) - 4;
	%w = %invWidth - 58;
	%h = %buttonWidth;
	%newBox.resize(%x, %y, %w, %h);
	$BSD_InvBox = %newBox;
	%newBox.setName("BSD_InvBox");
	for (%i = 0; %i < $BSD_NumInventorySlots; %i++)
	{
		%x = %i * (%buttonWidth + 1);
		%y = 0;
		%w = %buttonWidth;
		%h = %buttonWidth;
		%newInvBG = new GuiBitmapCtrl();
		%newBox.add(%newInvBG);
		%newInvBG.keepCached = 1;
		%newInvBG.setBitmap("base/client/ui/brickicons/brickiconbg");
		%newInvBG.resize(%x, %y, %w, %h);
		%newIcon = new GuiBitmapCtrl();
		%newBox.add(%newIcon);
		%newIcon.keepCached = 1;
		%newIcon.setProfile(HUDBitmapProfile);
		%newIcon.resize(%x, %y, %w, %h);
		$BSD_InvIcon[%i] = %newIcon;
		%newActive = new GuiBitmapCtrl();
		%newBox.add(%newActive);
		%newActive.keepCached = 1;
		%newActive.setBitmap("base/client/ui/brickicons/brickiconActive");
		%newActive.setVisible(0);
		%newActive.resize(%x, %y, %w, %h);
		$BSD_InvActive[%i] = %newActive;
		%newInvButton = new GuiBitmapButtonCtrl();
		%newBox.add(%newInvButton);
		%newInvButton.setBitmap("base/client/ui/brickicons/brickIconBtn");
		%newInvButton.setProfile(BlockButtonProfile);
		%newInvButton.setText(" ");
		%newInvButton.resize(%x, %y, %w, %h);
		%newInvButton.command = "BSD_ClickInv(" @ %i @ ");";
	}
}

function BSD_ClickClear()
{
	for (%i = 0; %i < $BSD_NumInventorySlots; %i++)
	{
		$BSD_InvIcon[%i].setBitmap("");
		$BSD_InvData[%i] = -1;
	}
}

function BSD_ClickInv(%index)
{
	if ($BSD_CurrClickInv == %index)
	{
		$BSD_InvData[%index] = -1;
		$BSD_InvIcon[%index].setBitmap("");
		$BSD_InvActive[%index].setVisible(0);
		$BSD_CurrClickData = -1;
		$BSD_CurrClickInv = -1;
	}
	else if ($BSD_CurrClickInv != -1)
	{
		%tempData = $BSD_InvData[%index];
		$BSD_InvData[%index] = $BSD_InvData[$BSD_CurrClickInv];
		$BSD_InvData[$BSD_CurrClickInv] = %tempData;
		if ($BSD_InvData[%index] > 0)
		{
			if (!isFile($BSD_InvData[%index].iconName @ ".png"))
			{
				$BSD_InvIcon[%index].setBitmap("base/client/ui/brickIcons/unknown");
			}
			else
			{
				$BSD_InvIcon[%index].setBitmap($BSD_InvData[%index].iconName);
			}
		}
		else
		{
			$BSD_InvIcon[%index].setBitmap("");
		}
		if ($BSD_InvData[$BSD_CurrClickInv] > 0)
		{
			if (!isFile($BSD_InvData[$BSD_CurrClickInv].iconName @ ".png"))
			{
				$BSD_InvIcon[$BSD_CurrClickInv].setBitmap("base/client/ui/brickIcons/unknown");
			}
			else
			{
				$BSD_InvIcon[$BSD_CurrClickInv].setBitmap($BSD_InvData[$BSD_CurrClickInv].iconName);
			}
		}
		else
		{
			$BSD_InvIcon[$BSD_CurrClickInv].setBitmap("");
		}
		$BSD_InvActive[%index].setVisible(0);
		$BSD_InvActive[$BSD_CurrClickInv].setVisible(0);
		$BSD_CurrClickData = -1;
		$BSD_CurrClickInv = -1;
	}
	else if ($BSD_CurrClickData != -1)
	{
		$BSD_InvData[%index] = $BSD_CurrClickData;
		if (!isFile($BSD_InvData[%index].iconName @ ".png"))
		{
			$BSD_InvIcon[%index].setBitmap("base/client/ui/brickIcons/unknown");
		}
		else
		{
			$BSD_InvIcon[%index].setBitmap($BSD_InvData[%index].iconName);
		}
		$BSD_activeBitmap[$BSD_CurrClickData].setVisible(0);
		$BSD_CurrClickData = -1;
		$BSD_CurrClickInv = -1;
	}
	else
	{
		$BSD_InvActive[%index].setVisible(1);
		$BSD_CurrClickData = -1;
		$BSD_CurrClickInv = %index;
	}
}

function BSD_ClickIcon(%data)
{
	if ($BSD_CurrClickData == %data)
	{
		$BSD_activeBitmap[%data].setVisible(0);
		%openSlot = -1;
		for (%i = 0; %i < $BSD_NumInventorySlots; %i++)
		{
			if ($BSD_InvData[%i] <= 0)
			{
				%openSlot = %i;
				break;
			}
		}
		if (%openSlot != -1)
		{
			$BSD_InvData[%openSlot] = $BSD_CurrClickData;
			if (!isFile($BSD_InvData[%openSlot].iconName @ ".png"))
			{
				$BSD_InvIcon[%openSlot].setBitmap("base/client/ui/brickIcons/unknown");
			}
			else
			{
				$BSD_InvIcon[%openSlot].setBitmap($BSD_InvData[%openSlot].iconName);
			}
		}
		else if ($pref::Input::QueueBrickBuying)
		{
			for (%i = 0; %i < $BSD_NumInventorySlots - 1; %i++)
			{
				$BSD_InvData[%i] = $BSD_InvData[%i + 1];
				if (!isFile($BSD_InvData[%i].iconName @ ".png"))
				{
					$BSD_InvIcon[%i].setBitmap("base/client/ui/brickIcons/unknown");
				}
				else
				{
					$BSD_InvIcon[%i].setBitmap($BSD_InvData[%i].iconName);
				}
			}
			$BSD_InvData[$BSD_NumInventorySlots - 1] = $BSD_CurrClickData;
			if (!isFile($BSD_InvData[$BSD_NumInventorySlots - 1].iconName @ ".png"))
			{
				$BSD_InvIcon[$BSD_NumInventorySlots - 1].setBitmap("base/client/ui/brickIcons/unknown");
			}
			else
			{
				$BSD_InvIcon[$BSD_NumInventorySlots - 1].setBitmap($BSD_InvData[$BSD_NumInventorySlots - 1].iconName);
			}
		}
		$BSD_activeBitmap[$BSD_CurrClickData].setVisible(0);
		$BSD_CurrClickData = -1;
		$BSD_CurrClickInv = -1;
	}
	else
	{
		if ($BSD_CurrClickData != -1)
		{
			$BSD_activeBitmap[$BSD_CurrClickData].setVisible(0);
		}
		if ($BSD_CurrClickInv != -1)
		{
			$BSD_InvActive[$BSD_CurrClickInv].setVisible(0);
		}
		$BSD_activeBitmap[%data].setVisible(1);
		$BSD_CurrClickData = %data;
		$BSD_CurrClickInv = -1;
	}
}

function BSD_RightClickIcon(%data)
{
	Canvas.popDialog(BrickSelectorDlg);
	commandToServer('InstantUseBrick', %data);
	$LastInstantUseData = %data;
	$InstantUse = 1;
	setActiveInv(-1);
	setScrollMode($SCROLLMODE_BRICKS);
	HUD_BrickName.setText(%data.uiName);
}

function BSD_ShowTab(%catID)
{
	$BSD_CurrTab = %catID;
	for (%i = 0; %i < $BSD_numCategories; %i++)
	{
		if (%i == %catID)
		{
			$BSD_category[%i].Scroll.setVisible(1);
			$BSD_category[%i].tab.setBitmap("base/client/ui/tab1use");
		}
		else
		{
			$BSD_category[%i].Scroll.setVisible(0);
			$BSD_category[%i].tab.setBitmap("base/client/ui/tab1");
		}
	}
}

function BSD_NextTab()
{
	$BSD_CurrTab++;
	if ($BSD_CurrTab >= $BSD_numCategories)
	{
		$BSD_CurrTab = 0;
	}
	BSD_ShowTab($BSD_CurrTab);
}

function BSD_BuyBricks()
{
	for (%i = 0; %i < $BSD_NumInventorySlots; %i++)
	{
		$HUD_BrickIcon[%i].setBitmap("");
		$InvData[%i] = -1;
		commandToServer('BuyBrick', %i, $BSD_InvData[%i]);
	}
	Canvas.popDialog(BrickSelectorDlg);
}

function BSD_SetFavs()
{
	BSD_FavsHelper.setVisible(!BSD_FavsHelper.visible);
	if (BSD_FavsHelper.visible == 0)
	{
		BSD_SetFavsButton.setText("Set Favs>");
	}
	else
	{
		BSD_SetFavsButton.setText(" Cancel ");
	}
}

function BSD_ClickFav(%idx)
{
	if (BSD_FavsHelper.visible == 1)
	{
		BSD_SaveFavorites(%idx);
		BSD_FavsHelper.setVisible(0);
		BSD_SetFavsButton.setText("Set Favs>");
	}
	else
	{
		BSD_BuyFavorites(%idx);
	}
}

function BSD_SaveFavorites(%idx)
{
	if ($FavoritesLoaded == 0)
	{
		BSD_LoadFavorites();
	}
	echo("saving brick faves to index ", %idx);
	for (%i = 0; %i < $BSD_NumInventorySlots; %i++)
	{
		$Favorite::Brick[%idx, %i] = $BSD_InvData[%i].uiName;
	}
	%fileName = "~/config/client/Favorites.cs";
	export("$Favorite::*", %fileName, 0);
}

function BSD_BuyFavorites(%idx)
{
	if ($FavoritesLoaded == 0)
	{
		BSD_LoadFavorites();
	}
	if ($UINameTableCreated == 0)
	{
		createUiNameTable();
	}
	for (%i = 0; %i < $BSD_NumInventorySlots; %i++)
	{
		$BSD_InvData[%i] = $uiNameTable[$Favorite::Brick[%idx, %i]];
		if (isObject($BSD_InvData[%i]))
		{
			if (!isFile($BSD_InvData[%i].iconName @ ".png"))
			{
				$BSD_InvIcon[%i].setBitmap("base/client/ui/brickIcons/unknown");
			}
			else
			{
				$BSD_InvIcon[%i].setBitmap($BSD_InvData[%i].iconName);
			}
		}
		else
		{
			$BSD_InvIcon[%i].setBitmap("");
		}
	}
}

function BSD_LoadFavorites()
{
	exec("~/config/client/Favorites.cs");
	$FavoritesLoaded = 1;
}

function listAllDataBlocks()
{
	%numDataBlocks = getDataBlockGroupSize();
	echo(%numDataBlocks, " DataBlocks");
	echo("--------------------------");
	for (%i = 0; %i < %numDataBlocks; %i++)
	{
		%db = getDataBlock(%i);
		%dbClass = %db.getClassName();
		echo(%db, " : ", %dbClass);
	}
	echo("--------------------------");
}

function clientCmdUseBrickControls()
{
	return;
	if (!$UsingBrickControls)
	{
		$BC_mouseX = moveMap.getCommand("mouse0", "xaxis");
		$BC_mouseY = moveMap.getCommand("mouse0", "yaxis");
		$BC_mouseZ = moveMap.getCommand("mouse0", "zaxis");
		$BC_mouseButton0 = moveMap.getCommand("mouse0", "button0");
		$BC_mouseButton1 = moveMap.getCommand("mouse0", "button1");
		$BC_mouseButton2 = moveMap.getCommand("mouse0", "button2");
		moveMap.bind(mouse0, "xaxis", mouseMoveBrickX);
		moveMap.bind(mouse0, "yaxis", mouseMoveBrickY);
		moveMap.bind(mouse0, "zaxis", mouseMoveBrickZ);
		moveMap.bind(mouse0, "button0", plantBrick);
		moveMap.bind(mouse0, "button1", RotateBrickCW);
		moveMap.bind(mouse0, "button2", RotateBrickCCW);
		$UsingBrickControls = 1;
	}
}

function clientCmdStopBrickControls()
{
	return;
	moveMap.bind(mouse0, "xaxis", $BC_mouseX);
	moveMap.bind(mouse0, "yaxis", $BC_mouseY);
	moveMap.bind(mouse0, "zaxis", $BC_mouseZ);
	moveMap.bind(mouse0, "button0", $BC_mouseButton0);
	moveMap.bind(mouse0, "button1", $BC_mouseButton1);
	moveMap.bind(mouse0, "button2", $BC_mouseButton2);
	$UsingBrickControls = 0;
}

function mouseMoveBrickX(%val)
{
	$mouseBrickShiftX += %val * 0.1;
	if ($mouseBrickShiftX >= 1)
	{
		commandToServer('ShiftBrick', 0, -1, 0);
		$mouseBrickShiftX = 0;
	}
	else if ($mouseBrickShiftX <= -1)
	{
		commandToServer('ShiftBrick', 0, 1, 0);
		$mouseBrickShiftX = 0;
	}
}

function mouseMoveBrickY(%val)
{
	$mouseBrickShiftY += %val * 0.1;
	if ($mouseBrickShiftY >= 1)
	{
		commandToServer('ShiftBrick', -1, 0, 0);
		$mouseBrickShiftY = 0;
	}
	else if ($mouseBrickShiftY <= -1)
	{
		commandToServer('ShiftBrick', 1, 0, 0);
		$mouseBrickShiftY = 0;
	}
}

function mouseMoveBrickZ(%val)
{
	commandToServer('ShiftBrick', 0, 0, %val / 120);
}

function saveBricks_ProcessWrenchExtras()
{
	%group = ServerConnection.getId();
	%count = %group.getCount();
	%brickCount = 0;
	for (%i = 0; %i < %count; %i++)
	{
		%obj = %group.getObject(%i);
		if (%obj.getClassName() $= "ParticleEmitterNode")
		{
			if (%obj.getEmitterDataBlock().uiName !$= "")
			{
				%trans = %obj.getTransform();
				%pos = getWords(%trans, 0, 2);
				%rot = getWords(%trans, 3, 6);
				if (%rot $= "1 0 0 0")
				{
					%dir = 0;
				}
				else if (%rot $= "0 1 0 3.14159")
				{
					%dir = 1;
				}
				else if (%rot $= "1 0 0 1.5708")
				{
					%dir = 2;
				}
				else if (%rot $= "0 -1 0 1.5708")
				{
					%dir = 3;
				}
				else if (%rot $= "-1 0 0 1.5708")
				{
					%dir = 4;
				}
				else if (%rot $= "0 1 0 1.5708")
				{
					%dir = 5;
				}
				else
				{
					%dir = 0;
				}
				%searchBox = "0.015 0.015 0.015";
				initClientBrickSearch(%pos, %searchBox);
				while ((%searchBrick = ClientBrickSearchNext()) != 0)
				{
					if (%searchBrick.isPlanted())
					{
						%searchBrick.emitter = %obj;
						%searchBrick.emitterDirection = %dir;
						break;
					}
				}
			}
		}
		else if (%obj.getClassName() $= "FxLight")
		{
			if (%obj.getDataBlock().uiName !$= "")
			{
				%trans = %obj.getTransform();
				%pos = getWords(%trans, 0, 2);
				%searchBox = "0.015 0.015 0.015";
				initClientBrickSearch(%pos, %searchBox);
				initClientBrickSearch(%pos, %searchBox);
				while ((%searchBrick = ClientBrickSearchNext()) != 0)
				{
					if (%searchBrick.isPlanted())
					{
						%searchBrick.light = %obj;
						break;
					}
				}
			}
		}
		else if (%obj.getClassName() $= "Item")
		{
			if (%obj.getDataBlock().uiName !$= "")
			{
				%trans = %obj.getTransform();
				%pos = getWords(%trans, 0, 2);
				%posX = getWord(%pos, 0);
				%posY = getWord(%pos, 1);
				%posZ = getWord(%pos, 2);
				%rot = getWords(%trans, 3, 6);
				if (%rot $= "1 0 0 0")
				{
					%dir = 2;
				}
				else if (%rot $= "0 0 1 1.5708")
				{
					%dir = 3;
				}
				else if (%rot $= "0 0 1 3.1416")
				{
					%dir = 4;
				}
				else if (%rot $= "0 0 -1 1.5708")
				{
					%dir = 5;
				}
				else
				{
					%dir = 2;
				}
				%itemBox = %obj.getWorldBox();
				%itemBoxX = mAbs(getWord(%itemBox, 0) - getWord(%itemBox, 3)) + 0.05;
				%itemBoxY = mAbs(getWord(%itemBox, 1) - getWord(%itemBox, 4)) + 0.05;
				%itemBoxZ = mAbs(getWord(%itemBox, 2) - getWord(%itemBox, 5)) + 0.05;
				%searchBox = %itemBoxX SPC %itemBoxY SPC %itemBoxZ;
				initClientBrickSearch(%pos, %searchBox);
				while ((%searchBrick = ClientBrickSearchNext()) != 0)
				{
					if (%searchBrick.isPlanted())
					{
						%brickPos = %searchBrick.getPosition();
						%brickPosX = getWord(%brickPos, 0);
						%brickPosY = getWord(%brickPos, 1);
						%brickPosZ = getWord(%brickPos, 2);
						%vecX = %brickPosX - %posX;
						%vecY = %brickPosY - %posY;
						%vecZ = %brickPosZ - %posZ;
						if (mAbs(%vecX) < 0.01)
						{
							%vecX = 0;
						}
						if (mAbs(%vecY) < 0.01)
						{
							%vecY = 0;
						}
						if (mAbs(%vecZ) < 0.01)
						{
							%vecZ = 0;
						}
						if (%vecX == 0 && %vecY == 0)
						{
							if (%vecZ > 0)
							{
								if (mAbs(getWord(%itemBox, 5) - getWord(%searchBrick.getWorldBox(), 2)) < 0.01)
								{
									%searchBrick.Item = %obj;
									%searchBrick.itemDirection = 1;
									break;
								}
							}
							else if (mAbs(getWord(%itemBox, 2) - getWord(%searchBrick.getWorldBox(), 5)) < 0.01)
							{
								%searchBrick.Item = %obj;
								%searchBrick.itemDirection = 0;
								break;
							}
						}
						else if (%vecX == 0 && %vecZ == 0)
						{
							if (%vecY > 0)
							{
								if (mAbs(getWord(%itemBox, 4) - getWord(%searchBrick.getWorldBox(), 1)) < 0.01)
								{
									%searchBrick.Item = %obj;
									%searchBrick.itemDirection = 4;
									break;
								}
							}
							else if (mAbs(getWord(%itemBox, 1) - getWord(%searchBrick.getWorldBox(), 4)) < 0.01)
							{
								%searchBrick.Item = %obj;
								%searchBrick.itemDirection = 2;
								break;
							}
						}
						else if (%vecY == 0 && %vecZ == 0)
						{
							if (%vecX > 0)
							{
								if (mAbs(getWord(%itemBox, 3) - getWord(%searchBrick.getWorldBox(), 0)) < 0.01)
								{
									%searchBrick.Item = %obj;
									%searchBrick.itemDirection = 5;
									break;
								}
								continue;
							}
							if (mAbs(getWord(%itemBox, 0) - getWord(%searchBrick.getWorldBox(), 3)) < 0.01)
							{
								%searchBrick.Item = %obj;
								%searchBrick.itemDirection = 3;
								break;
							}
						}
						%searchBrick.itemRespawnTime = %obj.getRespawnTime();
					}
				}
			}
		}
		else if (%obj.getClassName() $= "AudioEmitter")
		{
			if (%obj.getProfileId().uiName !$= "")
			{
				if (%obj.getProfileId().getDescription().isLooping == 1)
				{
					%trans = %obj.getTransform();
					%pos = getWords(%trans, 0, 2);
					%searchBox = "0.015 0.015 0.015";
					initClientBrickSearch(%pos, %searchBox);
					initClientBrickSearch(%pos, %searchBox);
					while ((%searchBrick = ClientBrickSearchNext()) != 0)
					{
						if (%searchBrick.isPlanted())
						{
							%searchBrick.AudioEmitter = %obj;
							break;
						}
					}
				}
			}
		}
		else if (%obj.getClassName() $= "VehicleSpawnMarker")
		{
			%trans = %obj.getTransform();
			%pos = getWords(%trans, 0, 2);
			%searchBox = "0.015 0.015 0.015";
			initClientBrickSearch(%pos, %searchBox);
			initClientBrickSearch(%pos, %searchBox);
			while ((%searchBrick = ClientBrickSearchNext()) != 0)
			{
				if (%searchBrick.isPlanted())
				{
					%searchBrick.VehicleSpawnMarker = %obj;
					break;
				}
			}
		}
	}
}

function saveBricks(%fileName, %description)
{
	%file = new FileObject();
	if (!isWriteableFileName(%fileName))
	{
		error("ERROR: saveBricks() - Invalid Filename!");
		return;
	}
	if (fileExt(%fileName) !$= ".bls")
	{
		error("ERROR: saveBricks() - File extension must be .bls");
		return;
	}
	saveBricks_ProcessWrenchExtras();
	%file.openForWrite(%fileName);
	%file.writeLine("This is a Blockland save file.  You probably shouldn't modify it cause you'll screw it up.");
	%lineCount = getLineCount(%description);
	%file.writeLine(%lineCount);
	for (%i = 0; %i < %lineCount; %i++)
	{
		%line = getLine(%description, %i);
		%file.writeLine(%line);
	}
	for (%i = 0; %i < 64; %i++)
	{
		%color = getColorIDTable(%i);
		%file.writeLine(%color);
	}
	if (isObject(ServerGroup))
	{
		%groupCount = mainBrickGroup.getCount();
		for (%g = 0; %g < %groupCount; %g++)
		{
			%group = mainBrickGroup.getObject(%g);
			%count = %group.getCount();
			for (%i = 0; %i < %count; %i++)
			{
				%obj = %group.getObject(%i);
				if (%obj.getClassName() $= "fxDTSBrick")
				{
					if (%obj.isPlanted() && !%obj.isDead())
					{
						%brickCount++;
					}
				}
			}
		}
		%file.writeLine("Linecount" SPC %brickCount);
		for (%g = 0; %g < %groupCount; %g++)
		{
			%group = mainBrickGroup.getObject(%g);
			%count = %group.getCount();
			for (%i = 0; %i < %count; %i++)
			{
				%obj = %group.getObject(%i);
				if (%obj.getClassName() $= "fxDTSBrick")
				{
					if (%obj.isPlanted() && !%obj.isDead())
					{
						SaveBricks_WriteSingleBrick(%file, %obj);
					}
				}
			}
		}
	}
	else
	{
		%group = ServerConnection.getId();
		%count = %group.getCount();
		%brickCount = 0;
		for (%i = 0; %i < %count; %i++)
		{
			%obj = %group.getObject(%i);
			if (%obj.getClassName() $= "fxDTSBrick")
			{
				if (%obj.isPlanted() && !%obj.isDead())
				{
					%brickCount++;
				}
			}
		}
		%file.writeLine("Linecount" SPC %brickCount);
		for (%i = 0; %i < %count; %i++)
		{
			%obj = %group.getObject(%i);
			if (%obj.getClassName() $= "fxDTSBrick")
			{
				if (%obj.isPlanted() && !%obj.isDead())
				{
					SaveBricks_WriteSingleBrick(%file, %obj);
				}
			}
		}
	}
	%file.close();
	%file.delete();
	%screenshotName = getSubStr(%fileName, 0, strlen(%fileName) - 4) @ ".jpg";
	%oldContent = Canvas.getContent();
	Canvas.setContent(noHudGui);
	screenShot(%screenshotName, "JPEG", 1);
	Canvas.setContent(%oldContent);
}

function SaveBricks_WriteSingleBrick(%file, %obj)
{
	%objData = %obj.getDataBlock();
	%uiName = %objData.uiName;
	%trans = %obj.getTransform();
	%pos = getWords(%trans, 0, 2);
	if (%objData.hasPrint)
	{
		%printTexture = getPrintTexture(%obj.getPrintID());
	}
	else
	{
		%printTexture = "NOPRINT";
	}
	%line = %uiName @ "\"" SPC %pos SPC %obj.getAngleID() SPC %obj.isBasePlate() SPC %obj.getColorID() SPC %printTexture SPC %obj.getColorFxID() SPC %obj.getShapeFxID();
	%file.writeLine(%line);
	if (isObject(%obj.emitter))
	{
		%line = "+-EMITTER" SPC %obj.emitter.getEmitterDataBlock().uiName @ "\" " @ %obj.emitterDirection;
		%file.writeLine(%line);
	}
	if (isObject(%obj.light))
	{
		%line = "+-LIGHT" SPC %obj.light.getDataBlock().uiName @ "\"";
		%file.writeLine(%line);
	}
	if (isObject(%obj.Item))
	{
		%line = "+-ITEM" SPC %obj.Item.getDataBlock().uiName @ "\" " @ %obj.itemPosition SPC %obj.itemDirection SPC %obj.itemRespawnTime;
		%file.writeLine(%line);
	}
	if (isObject(%obj.AudioEmitter))
	{
		%line = "+-AUDIOEMITTER" SPC %obj.AudioEmitter.getProfileId().uiName @ "\" ";
		%file.writeLine(%line);
	}
	if (isObject(%obj.VehicleSpawnMarker))
	{
		%line = "+-VEHICLE" SPC %obj.VehicleSpawnMarker.getUiName() @ "\" " @ %obj.VehicleSpawnMarker.getReColorVehicle();
		%file.writeLine(%line);
	}
}

function LoadBricks_GetColorDifference(%fileName)
{
	%file = new FileObject();
	if (!isFile(%fileName))
	{
		error("ERROR: LoadBricks_GetColorDifference() - File \"" @ %fileName @ "\" not found!");
	}
	%file.openForRead(%fileName);
	%file.readLine();
	%lineCount = %file.readLine();
	for (%i = 0; %i < %lineCount; %i++)
	{
		%file.readLine();
	}
	%colorCount = 0;
	for (%i = 0; %i < 64; %i++)
	{
		if (getWord(getColorIDTable(%i), 3) > 0.001)
		{
			%colorCount++;
		}
	}
	%different = 0;
	for (%i = 0; %i < 64; %i++)
	{
		%color = %file.readLine();
		%match = 0;
		for (%j = 0; %j < 64; %j++)
		{
			if (colorMatch(getColorIDTable(%j), %color))
			{
				%match = 1;
				break;
			}
		}
		if (%match == 0)
		{
			%different = 1;
			%colorCount++;
		}
	}
	%file.close();
	%file.delete();
	if (%different == 1)
	{
		if (%colorCount > 64)
		{
			return "REPLACE";
		}
		else
		{
			return "APPEND";
		}
	}
	else
	{
		return "SAME";
	}
}

function LoadBricks_ClientServerCheck(%fileName, %colorMethod)
{
	if (!isFile(%fileName))
	{
		error("ERROR: LoadBricks_ClientServerCheck() - File \"" @ %fileName @ "\" not found!");
	}
	if (isObject(ServerGroup))
	{
		serverDirectSaveFileLoad(%fileName, %colorMethod);
	}
	else
	{
		commandToServer('SetColorMethod', %colorMethod);
		UploadSaveFile_Start(%fileName);
		return;
	}
}

function clientCmdLoadBricksConfirmHandshake(%val, %allowColorLoads)
{
	if ($LoadingBricks_FileName $= "")
	{
		return;
	}
	$ServerAllowsColorLoads = %allowColorLoads;
	if (%val)
	{
		LoadBricks_ColorCheck();
	}
	else
	{
		if (isEventPending($LoadingBricks_HandShakeSchedule))
		{
			cancel($LoadingBricks_HandShakeSchedule);
		}
		$LoadingBricks_FileName = "";
	}
}

function clientCmdLoadBricksHandshake(%val, %allowColorLoads)
{
	$ServerAllowsColorLoads = %allowColorLoads;
	if (%val)
	{
		if ($LoadingBricks_FileName !$= "")
		{
			Canvas.popDialog(LoadBricksGui);
			Canvas.popDialog(LoadBricksColorGui);
			Canvas.popDialog(escapeMenu);
			UploadSaveFile_Start($LoadingBricks_FileName);
		}
	}
	else
	{
		if (isEventPending($LoadingBricks_HandShakeSchedule))
		{
			cancel($LoadingBricks_HandShakeSchedule);
		}
		$LoadingBricks_FileName = "";
		$LoadingBricks_ColorMethod = "";
	}
}

function createPrintNameTable()
{
	$PrintNameTableCreated = 1;
	%count = getNumPrintTextures();
	for (%i = 0; %i < %count; %i++)
	{
		$printNameTable[getPrintTexture(%i)] = %i;
	}
}

function createUiNameTable()
{
	$UINameTableCreated = 1;
	%dbCount = getDataBlockGroupSize();
	for (%i = 0; %i < %dbCount; %i++)
	{
		%db = getDataBlock(%i);
		if (%db.getClassName() $= "FxDTSBrickData")
		{
			$uiNameTable[%db.uiName] = %db;
		}
		else if (%db.getClassName() $= "FxLightData")
		{
			if (%db.uiName !$= "")
			{
				$uiNameTable_Lights[%db.uiName] = %db;
			}
		}
		else if (%db.getClassName() $= "ParticleEmitterData")
		{
			if (%db.uiName !$= "")
			{
				$uiNameTable_Emitters[%db.uiName] = %db;
			}
		}
		else if (%db.getClassName() $= "ItemData")
		{
			if (%db.uiName !$= "")
			{
				$uiNameTable_Items[%db.uiName] = %db;
			}
		}
		else if (%db.getClassName() $= "AudioProfile")
		{
			if (%db.uiName !$= "")
			{
				if (%db.getDescription().isLooping)
				{
					$uiNameTable_Music[%db.uiName] = %db;
					continue;
				}
				$uiNameTable_Sounds[%db.uiName] = %db;
			}
		}
	}
}

function LoadBricksGui::onWake(%this)
{
	LoadBricks_MapMenu.clear();
	for (%fileName = findFirstFile("base/saves/*"); %fileName !$= ""; %fileName = findNextFile("base/saves/*"))
	{
		%filePath = filePath(%fileName);
		%subDir = getSubStr(%filePath, 11, 9999);
		if (%subDir !$= "")
		{
			LoadBricks_AddMapFolder(%subDir);
		}
	}
	LoadBricks_MapMenu.sort();
	%id = 0;
	for (%text = LoadBricks_MapMenu.getTextById(%id); %text !$= ""; %text = LoadBricks_MapMenu.getTextById(%id++))
	{
		if (%text $= $MapSaveName)
		{
			break;
		}
	}
	if (%text $= "")
	{
		$LoadBricks_LastMapMenuID = mFloor($LoadBricks_LastMapMenuID);
		LoadBricks_MapMenu.setSelected($LoadBricks_LastMapMenuID);
	}
	else
	{
		LoadBricks_MapMenu.setSelected(%id);
	}
	LoadBricks_MapClick();
	LoadBricks_Preview.setBitmap("");
	LoadBricks_Description.setText("");
}

function LoadBricks_AddMapFolder(%name)
{
	%rowCount = 0;
	for (%rowText = LoadBricks_MapMenu.getTextById(%rowCount); %rowText !$= ""; %rowText = LoadBricks_MapMenu.getTextById(%rowCount++))
	{
		if (%rowText $= %name)
		{
			return;
		}
	}
	LoadBricks_MapMenu.add(%name, %rowCount);
}

function LoadBricks_MapClick()
{
	%mapName = LoadBricks_MapMenu.getText();
	$LoadBricks_LastMapMenuID = LoadBricks_MapMenu.getSelected();
	if (%mapName $= "")
	{
		return;
	}
	LoadBricks_FileList.clear();
	%dir = "base/saves/" @ %mapName @ "/*.bls";
	%fileName = findFirstFile(%dir);
	for (%count = 0; %fileName !$= ""; %fileName = findNextFile(%dir))
	{
		%baseName = fileBase(%fileName);
		%fileDate = getFileModifiedTime(%fileName);
		%fileSortDate = getFileModifiedSortTime(%fileName);
		%rowText = %baseName TAB %fileDate TAB %fileSortDate;
		LoadBricks_FileList.addRow(%count, %rowText);
		%count++;
	}
	updateListSort(LoadBricks_FileList);
}

function LoadBricks_FileClick()
{
	%id = LoadBricks_FileList.getSelectedId();
	%fileName = getField(LoadBricks_FileList.getRowTextById(%id), 0);
	%mapSaveName = LoadBricks_MapMenu.getTextById(LoadBricks_MapMenu.getSelected());
	%fullPath = "base/saves/" @ %mapSaveName @ "/" @ %fileName @ ".bls";
	%description = SaveBricks_GetFileDescription(%fullPath);
	LoadBricks_Description.setText(%description);
	%screenshotName = getSubStr(%fullPath, 0, strlen(%fullPath) - 4) @ ".jpg";
	if (isFile(%screenshotName))
	{
		LoadBricks_Preview.setBitmap(%screenshotName);
	}
	else
	{
		LoadBricks_Preview.setBitmap("base/data/missions/default.png");
	}
}

function LoadBricks_ClickLoadButton()
{
	if (!isUnlocked())
	{
		if (isObject(ServerGroup))
		{
			ServerCmdClearAllBricks(ClientGroup.getObject(0));
		}
	}
	%id = LoadBricks_FileList.getSelectedId();
	%fileName = getField(LoadBricks_FileList.getRowTextById(%id), 0);
	%mapName = LoadBricks_MapMenu.getText();
	if (%fileName $= "")
	{
		echo("ERROR: LoadBricks_ClickLoadButton() - no file selected");
		return;
	}
	$LoadingBricks_FileName = "base/saves/" @ %mapName @ "/" @ %fileName @ ".bls";
	if (isObject(ServerGroup))
	{
		LoadBricks_ColorCheck();
	}
	else
	{
		commandToServer('InitUploadHandshake');
		$LoadingBricks_HandShakeSchedule = schedule(30 * 1000, 0, eval, "$LoadingBricks_FileName = \"\";");
	}
}

function LoadBricks_ColorCheck()
{
	if ($ServerAllowsColorLoads || isObject(ServerGroup) && $Pref::Server::AllowColorLoading)
	{
		%colorDiff = LoadBricks_GetColorDifference($LoadingBricks_FileName);
		if (%colorDiff $= "SAME")
		{
			LoadBricks_ClientServerCheck($LoadingBricks_FileName, 0);
			Canvas.popDialog(LoadBricksGui);
			Canvas.popDialog(escapeMenu);
		}
		else if (%colorDiff $= "APPEND")
		{
			BTN_LoadBricksColor_Append.setVisible(1);
			Canvas.pushDialog(LoadBricksColorGui);
		}
		else if (%colorDiff $= "REPLACE")
		{
			BTN_LoadBricksColor_Append.setVisible(0);
			Canvas.pushDialog(LoadBricksColorGui);
		}
	}
	else
	{
		LoadBricks_ClientServerCheck($LoadingBricks_FileName, 3);
		Canvas.popDialog(LoadBricksGui);
		Canvas.popDialog(LoadBricksColorGui);
		Canvas.popDialog(escapeMenu);
	}
}

function saveBricksGui::onWake(%this)
{
	SaveBricks_FileName.setText("");
	SaveBricks_Description.setText("");
	SaveBricks_FileList.clear();
	SaveBricks_Preview.setBitmap("base/data/missions/default.png");
	SaveBricks_Window.setText("Save Bricks - " @ $MapSaveName);
	%savePath = "base/saves/" @ $MapSaveName @ "/*.bls";
	%fileName = findFirstFile(%savePath);
	for (%count = 0; %fileName !$= ""; %fileName = findNextFile(%savePath))
	{
		%baseName = fileBase(%fileName);
		%fileDate = getFileModifiedTime(%fileName);
		%fileSortDate = getFileModifiedSortTime(%fileName);
		%rowText = %baseName TAB %fileDate TAB %fileSortDate;
		SaveBricks_FileList.addRow(%count, %rowText);
		%count++;
	}
	updateListSort(SaveBricks_FileList);
}

function SaveBricks_ClickFileList()
{
	%id = SaveBricks_FileList.getSelectedId();
	%fileName = getField(SaveBricks_FileList.getRowTextById(%id), 0);
	SaveBricks_FileName.setText(%fileName);
	%fullPath = "base/saves/" @ $MapSaveName @ "/" @ %fileName @ ".bls";
	SaveBricks_Description.setText(SaveBricks_GetFileDescription(%fullPath));
	%screenshotName = getSubStr(%fullPath, 0, strlen(%fullPath) - 4) @ ".jpg";
	if (isFile(%screenshotName))
	{
		SaveBricks_Preview.setBitmap(%screenshotName);
	}
	else
	{
		SaveBricks_Preview.setBitmap("base/data/missions/default.png");
	}
}

function isValidFileName(%fileName)
{
	if (strlen(%fileName) <= 0)
	{
		return 0;
	}
	if (strlen(%fileName) >= 255)
	{
		return 0;
	}
	%badChars = "\\ / : * ? \" < > |";
	%count = getWordCount(%badChars);
	for (%i = 0; %i < %count; %i++)
	{
		%word = getWord(%badChars, %i);
		if (strpos(%fileName, %word) != -1)
		{
			return 0;
		}
	}
	return 1;
}

function SaveBricks_Save()
{
	%fileName = SaveBricks_FileName.getValue();
	if (%fileName $= "")
	{
		MessageBoxOK("No Filename", "You must enter a filename.");
		return;
	}
	if (!isValidFileName(%fileName))
	{
		MessageBoxOK("Invalid Filename", "Filenames cannot contain any of these characters \\ / : * ? \" < > |");
		return;
	}
	%fullPath = "base/saves/" @ $MapSaveName @ "/" @ %fileName @ ".bls";
	%description = SaveBricks_Description.getText();
	if (isFile(%fullPath))
	{
		%description = expandEscape(%description);
		$SaveBricksPath = %fullPath;
		$SaveBricksDescription = %description;
		%callback = "canvas.popDialog(SaveBricksGui);canvas.popDialog(EscapeMenu);canvas.pushDialog(SavingGui);";
		MessageBoxYesNo("File Exists, Overwrite?", "Are you sure you want to overwrite the file \"" @ %fileName @ "\"?", %callback);
	}
	else
	{
		$SaveBricksPath = %fullPath;
		$SaveBricksDescription = %description;
		Canvas.popDialog(saveBricksGui);
		Canvas.popDialog(escapeMenu);
		Canvas.pushDialog(SavingGui);
	}
}

function SaveBricks_GetFileDescription(%fileName)
{
	%file = new FileObject();
	if (fileExt(%fileName) !$= ".bls")
	{
		error("ERROR : SaveBricks_GetFileDescription() - Filename does not end in .bls");
		return;
	}
	if (!isFile(%fileName))
	{
		error("ERROR : SaveBricks_GetFileDescription() - File does not exist");
		return;
	}
	%file.openForRead(%fileName);
	%file.readLine();
	%lineCount = %file.readLine();
	%description = "";
	for (%i = 0; %i < %lineCount; %i++)
	{
		%line = %file.readLine();
		if (%i == 0)
		{
			%description = %line;
		}
		else
		{
			%description = %description @ "\n" @ %line;
		}
	}
	%file.close();
	%file.delete();
	return %description;
}

function colorMatch(%colorA, %colorB)
{
	for (%i = 0; %i < 4; %i++)
	{
		if (mAbs(getWord(%colorA, %i) - getWord(%colorB, %i)) > 0.005)
		{
			return 0;
		}
	}
	return 1;
}

function LoadBricks_SendLineToServer(%line, %i)
{
	commandToServer('UploadSaveFileLine', %line);
	%firstWord = getWord(%line, 0);
	if (%firstWord $= "+-LIGHT")
	{
	}
	else if (%firstWord $= "+-EMITTER")
	{
	}
	else if (%firstWord $= "+-ITEM")
	{
	}
	else if (%firstWord $= "+-AUDIOEMITTER")
	{
	}
	else
	{
		Progress_Bar.setValue(%i / Progress_Bar.total);
		if (%i + 1 == Progress_Bar.total)
		{
			echo("popping progress gui");
			commandToServer('EndBrickLoad');
		}
	}
}

function LoadBricks_CreateFromLine(%line, %i)
{
	%firstWord = getWord(%line, 0);
	if (%firstWord $= "+-LIGHT")
	{
		if ($LastLoadedBrick)
		{
			%line = getSubStr(%line, 8, strlen(%line) - 8);
			%pos = strpos(%line, "\"");
			%dbName = getSubStr(%line, 0, %pos);
			%db = $uiNameTable_Lights[%dbName];
			$LastLoadedBrick.setLight(%db);
		}
		return;
	}
	else if (%firstWord $= "+-EMITTER")
	{
		if ($LastLoadedBrick)
		{
			%line = getSubStr(%line, 10, strlen(%line) - 10);
			%pos = strpos(%line, "\"");
			%dbName = getSubStr(%line, 0, %pos);
			%db = $uiNameTable_Emitters[%dbName];
			%line = getSubStr(%line, %pos + 2, (strlen(%line) - %pos) - 2);
			%dir = getWord(%line, 0);
			$LastLoadedBrick.setEmitter(%db);
			$LastLoadedBrick.setEmitterDirection(%dir);
		}
		return;
	}
	else if (%firstWord $= "+-ITEM")
	{
		if ($LastLoadedBrick)
		{
			%line = getSubStr(%line, 7, strlen(%line) - 7);
			%pos = strpos(%line, "\"");
			%dbName = getSubStr(%line, 0, %pos);
			%db = $uiNameTable_Items[%dbName];
			%line = getSubStr(%line, %pos + 2, (strlen(%line) - %pos) - 2);
			%pos = getWord(%line, 0);
			%dir = getWord(%line, 1);
			$LastLoadedBrick.setItem(%db);
			$LastLoadedBrick.setItemDirection(%dir);
			$LastLoadedBrick.setItemPosition(%pos);
		}
		return;
	}
	else if (%firstWord $= "+-AUDIOEMITTER")
	{
		if ($LastLoadedBrick)
		{
			%line = getSubStr(%line, 15, strlen(%line) - 15);
			%pos = strpos(%line, "\"");
			%dbName = getSubStr(%line, 0, %pos);
			%db = $uiNameTable_Music[%dbName];
			$LastLoadedBrick.setSound(%db);
		}
		return;
	}
	%quotePos = strstr(%line, "\"");
	%uiName = getSubStr(%line, 0, %quotePos);
	%line = getSubStr(%line, %quotePos + 2, 9999);
	%pos = getWords(%line, 0, 2);
	%angId = getWord(%line, 3);
	%isBaseplate = getWord(%line, 4);
	%colorId = $colorTranslation[mFloor(getWord(%line, 5))];
	%printId = $printNameTable[getWord(%line, 6)];
	%colorFX = getWord(%line, 7);
	%shapeFX = getWord(%line, 8);
	%db = $uiNameTable[%uiName];
	if (%db)
	{
		%trans = %pos;
		if (%angId == 0)
		{
			%trans = %trans SPC " 1 0 0 0";
		}
		else if (%angId == 1)
		{
			%trans = %trans SPC " 0 0 1" SPC $piOver2;
		}
		else if (%angId == 2)
		{
			%trans = %trans SPC " 0 0 1" SPC $pi;
		}
		else if (%angId == 3)
		{
			%trans = %trans SPC " 0 0 -1" SPC $piOver2;
		}
		%b = new fxDTSBrick()
		{
			dataBlock = %db;
			angleID = %angId;
			isBasePlate = %isBaseplate;
			colorID = %colorId;
			printID = %printId;
			colorFxID = %colorFX;
			shapeFxID = %shapeFX;
			isPlanted = 1;
		};
		MissionCleanup.add(%b);
		%b.setTransform(%trans);
		%err = %b.plant();
		if (%err == 1 || %err == 3 || %err == 5)
		{
			$failureCount++;
			%b.delete();
			$LastLoadedBrick = 0;
		}
		else
		{
			$LastLoadedBrick = %b;
			%brickCount++;
		}
	}
	else
	{
		error("ERROR: loadBricks() - DataBlock not found for brick named ", %uiName);
		$failureCount++;
	}
	Progress_Bar.setValue(%i / Progress_Bar.total);
	if (%i + 1 >= Progress_Bar.total)
	{
		if (ProgressGui.isAwake())
		{
			echo("popping progress gui 2");
			%time = getSimTime() - $LoadingBricks_StartTime;
			MessageAll('', %i + 1 @ " bricks loaded in " @ getTimeString(%time / 1000));
			if ($failureCount > 1)
			{
				MessageAll('', $failureCount @ " bricks could not be placed");
			}
			else if ($failureCount == 1)
			{
				MessageAll('', "1 brick could not be placed");
			}
		}
	}
}

function LoadBricks_ClickFastLoad()
{
	if ($pref::FastLoad && !$Pref::Gui::IgnoreFastLoadWarning)
	{
		Canvas.pushDialog(fastLoadWarningGui);
	}
}

function sortList(%obj, %col)
{
	%obj.sortedNumerical = 0;
	if (%obj.sortedBy == %col)
	{
		%obj.sortedAsc = !%obj.sortedAsc;
		%obj.sort(%obj.sortedBy, %obj.sortedAsc);
	}
	else
	{
		%obj.sortedBy = %col;
		%obj.sortedAsc = 0;
		%obj.sort(%obj.sortedBy, %obj.sortedAsc);
	}
}

function sortNumList(%obj, %col)
{
	%obj.sortedNumerical = 1;
	if (%obj.sortedBy == %col)
	{
		%obj.sortedAsc = !%obj.sortedAsc;
		%obj.sortNumerical(%obj.sortedBy, %obj.sortedAsc);
	}
	else
	{
		%obj.sortedBy = %col;
		%obj.sortedAsc = 0;
		%obj.sortNumerical(%obj.sortedBy, %obj.sortedAsc);
	}
}

function updateListSort(%obj)
{
	if (%obj.sortedNumerical $= "")
	{
		%obj.sortedNumerical = 0;
		%obj.sortedBy = 0;
		%obj.sortedAsc = 1;
	}
	if (%obj.sortedNumerical)
	{
		%obj.sortNumerical(%obj.sortedBy, %obj.sortedAsc);
	}
	else
	{
		%obj.sort(%obj.sortedBy, %obj.sortedAsc);
	}
}

function ColorWarning_ClickMatch()
{
	LoadBricks_ClientServerCheck($LoadingBricks_FileName, 3);
	Canvas.popDialog(LoadBricksGui);
	Canvas.popDialog(LoadBricksColorGui);
	Canvas.popDialog(escapeMenu);
}

function ColorWarning_ClickReplace()
{
	LoadBricks_ClientServerCheck($LoadingBricks_FileName, 2);
	Canvas.popDialog(LoadBricksGui);
	Canvas.popDialog(LoadBricksColorGui);
	Canvas.popDialog(escapeMenu);
}

function ColorWarning_ClickAppend()
{
	LoadBricks_ClientServerCheck($LoadingBricks_FileName, 1);
	Canvas.popDialog(LoadBricksGui);
	Canvas.popDialog(LoadBricksColorGui);
	Canvas.popDialog(escapeMenu);
}

function ColorWarning_ClickCancel()
{
	commandToServer('CancelSaveFileUpload');
	Canvas.popDialog(LoadBricksColorGui);
}

function UploadSaveFile_Start(%fileName)
{
	$Client_LoadFileObj = new FileObject();
	$Client_LoadFileObj.openForRead(%fileName);
	Progress_Bar.total = 0;
	Progress_Bar.count = 0;
	UploadSaveFile_Tick();
}

function UploadSaveFile_Tick()
{
	if (!isObject($Client_LoadFileObj))
	{
		return;
	}
	%line = $Client_LoadFileObj.readLine();
	LoadBricks_SendLineToServer(%line);
	if (isEventPending($UploadSaveFile_Tick_Schedule))
	{
		cancel($UploadSaveFile_Tick_Schedule);
	}
	if (Progress_Bar.total == 0)
	{
		%firstWord = getWord(%line, 0);
		if (%firstWord $= "Linecount")
		{
			%lineCount = getWord(%line, 1);
			echo("reading linecount ", %lineCount);
			Canvas.pushDialog(ProgressGui);
			Progress_Window.setText("Loading Progress");
			Progress_Bar.setValue(0);
			Progress_Bar.total = %lineCount;
			Progress_Bar.count = 0;
			Progress_Text.setText("Uploading...");
		}
	}
	else
	{
		%prefix = getSubStr(%line, 0, 2);
		if (%prefix !$= "+-")
		{
			Progress_Bar.count++;
			Progress_Bar.setValue(Progress_Bar.count / Progress_Bar.total);
		}
	}
	%time = 100 / $pref::Net::PacketRateToServer;
	if (%time < 1)
	{
		%time = 1;
	}
	if (!$Client_LoadFileObj.isEOF())
	{
		$UploadSaveFile_Tick_Schedule = schedule(%time, 0, UploadSaveFile_Tick);
	}
	else
	{
		UploadSaveFile_End();
	}
}

function UploadSaveFile_End()
{
	$Client_LoadFileObj.delete();
	Canvas.popDialog(ProgressGui);
	commandToServer('EndSaveFileUpload');
}

$pref::Player::Authentic = 0;
$minifigColor[0] = "0.867 0.000 0.000 1.000";
$minifigColor[1] = "0.973 0.800 0.000 1.000";
$minifigColor[2] = "0.000 0.471 0.196 1.000";
$minifigColor[3] = "0.000 0.317 0.745 1.000";
$minifigColor[4] = "0.996 0.996 0.910 1.000";
$minifigColor[5] = "0.647 0.647 0.647 1.000";
$minifigColor[6] = "0.471 0.471 0.471 1.000";
$minifigColor[7] = "0.200 0.200 0.200 1.000";
$minifigColor[8] = "0.400 0.196 0.000 1.000";
$minifigColor[9] = "0.667 0.000 0.000 0.700";
$minifigColor[10] = "1.000 0.500 0.000 0.800";
$minifigColor[11] = "0.850 1.000 0.000 0.600";
$minifigColor[12] = "0.990 0.960 0.000 0.800";
$minifigColor[13] = "0.000 0.471 0.196 0.750";
$minifigColor[14] = "0.000 0.200 0.640 0.750";
$minifigColor[15] = "0.550 0.700 1.000 0.700";
$minifigColor[16] = "0.850 0.850 0.850 0.700";
$minifigColor[17] = "0.847059 0.894118 0.654902 1.000000";
$minifigColor[18] = "0.631373 0.764706 0.541176 1.000000";
$minifigColor[19] = "0.639216 0.737255 0.274510 1.000000";
$minifigColor[20] = "0.756863 0.792157 0.866667 1.000000";
$minifigColor[21] = "0.329412 0.647059 0.686275 1.000000";
$minifigColor[22] = "0.258824 0.329412 0.576471 1.000000";
$minifigColor[23] = "0.976471 0.909804 0.600000 1.000000";
$minifigColor[24] = "0.776471 0.819608 0.231373 1.000000";
$minifigColor[25] = "0.756863 0.854902 0.721569 1.000000";
$minifigColor[26] = "0.713726 0.839216 0.831373 1.000000";
$minifigColor[27] = "0.701961 0.819608 0.886275 1.000000";
$minifigColor[28] = "0.764706 0.439216 0.627451 1.000000";
$hatColors["none"] = "0 1 2 3 4 5 6 7 8";
$hatColors["helmet"] = "0 1 2 3 4 5 6 7 8";
$hatColors["scoutHat"] = "2 8";
$hatColors["pointyHelmet"] = "6 7";
$packColors["none"] = "0 1 2 3 4 5 6 7 8";
$packColors["Armor"] = "6 7";
$packColors["Bucket"] = "7 8";
$packColors["Cape"] = "0 1 2 3 4 5 6 7 8";
$packColors["Pack"] = 8;
$packColors["Quiver"] = 8;
$packColors["Tank"] = "0 1 3 4 5 7";
$torsoColors["decalNone.png"] = "0 1 2 3 4 5 6 7 8";
$torsoColors["decalBtron.png"] = 4;
$torsoColors["decalDarth.png"] = 7;
$torsoColors["decalRedCoat.png"] = 4;
$torsoColors["decalBlueCoat.png"] = 4;
$torsoColors["decalMtron.png"] = 0;
$torsoColors["decalNewSpace.png"] = 4;
$torsoColors["decalOldSpace.png"] = "0 1 3 4 7";
$torsoColors["decalCastle1.png"] = "0 1 2 3 4 5 6 7 8";
$torsoColors["decalArmor.png"] = "0 1 2 3 4 5 6 7 8";
$torsoColors["decalPirate1.png"] = "0 1 2 3 4 5 6 7 8";
$torsoColors["decalBeads.png"] = "0 1 2 3 4 5 6 7 8";
$torsoColors["decalLion.png"] = "0 1 2 3 4 5 6 7 8";
$accentColors["visor"] = "7 9 10 11 12 13 14 15 16";
$accentColors["plume"] = "0 1 2 3 4 5 6 7";
$allColors = "0 1 2 3 4 5 6 7 8 17 18 19 20 21 22 23 24 25 26 27 28 9 10 11 12 13 14 15 16";
$normalColors = "0 1 2 3 4 5 6 7 8 17 18 19 20 21 22 23 24 25 26 27 28";
$basicColors = "0 1 2 3 4 5 6 7 8";
$accentColorsUnAuth["visor"] = $allColors;
$accentColorsUnAuth["plume"] = $normalColors;
function AvatarGui::onWake(%this)
{
	if ($Avatar::NumColors $= "")
	{
		ColorSetGui.Load();
	}
	AV_FavsHelper.setVisible(0);
	if ($AvatarHasLoaded)
	{
		Avatar_Preview.setObject("", "base/data/shapes/player/m.dts", "", 100);
		Avatar_Preview.setSequence("", 0, headup, 0);
		Avatar_Preview.setThreadPos("", 0, 0);
		Avatar_Preview.setSequence("", 1, run, 0.85);
		Avatar_Preview.setCameraRot(0.3, 0.6, 2.52);
		Avatar_Preview.setOrbitDist(4.34);
		Avatar_UpdatePreview();
		return;
	}
	$AvatarHasLoaded = 1;
	AvatarGui_LoadAccentInfo("accent", "base/data/shapes/player/accent.txt");
	%x = getWord(Avatar_FacePreview.position, 0) + 64;
	%y = getWord(Avatar_FacePreview.position, 1);
	AvatarGui_CreatePartMenu("Avatar_FaceMenu", "Avatar_SetFace", "base/data/shapes/player/face.ifl", %x, %y);
	%x = getWord(Avatar_DecalPreview.position, 0) + 64;
	%y = getWord(Avatar_DecalPreview.position, 1) - 64;
	AvatarGui_CreatePartMenu("Avatar_DecalMenu", "Avatar_SetDecal", "base/data/shapes/player/decal.ifl", %x, %y);
	%x = getWord(Avatar_PackPreview.position, 0) + 64;
	%y = getWord(Avatar_PackPreview.position, 1) - 64;
	AvatarGui_CreatePartMenu("Avatar_PackMenu", "Avatar_SetPack", "base/data/shapes/player/Pack.txt", %x, %y);
	%x = getWord(Avatar_SecondPackPreview.position, 0) + 64;
	%y = getWord(Avatar_SecondPackPreview.position, 1) - 64;
	AvatarGui_CreatePartMenu("Avatar_SecondPackMenu", "Avatar_SetSecondPack", "base/data/shapes/player/SecondPack.txt", %x, %y);
	%x = getWord(Avatar_HatPreview.position, 0) + 64;
	%y = getWord(Avatar_HatPreview.position, 1);
	AvatarGui_CreatePartMenu("Avatar_HatMenu", "Avatar_SetHat", "base/data/shapes/player/Hat.txt", %x, %y);
	%x = getWord(Avatar_AccentPreview.position, 0) + 64;
	%y = getWord(Avatar_AccentPreview.position, 1);
	AvatarGui_CreateSubPartMenu("Avatar_AccentMenu", "Avatar_SetAccent", $accentsAllowed[$hat[$pref::Player::Hat]], %x, %y);
	%x = getWord(Avatar_ChestPreview.position, 0) + 64;
	%y = getWord(Avatar_ChestPreview.position, 1);
	AvatarGui_CreatePartMenu("Avatar_ChestMenu", "Avatar_SetChest", "base/data/shapes/player/Chest.txt", %x, %y);
	%x = getWord(Avatar_HipPreview.position, 0) + 64;
	%y = getWord(Avatar_HipPreview.position, 1);
	AvatarGui_CreatePartMenu("Avatar_HipMenu", "Avatar_SetHip", "base/data/shapes/player/hip.txt", %x, %y);
	%x = getWord(Avatar_RLegPreview.position, 0) + 64;
	%y = getWord(Avatar_RLegPreview.position, 1);
	AvatarGui_CreatePartMenu("Avatar_RLegMenu", "Avatar_SetRLeg", "base/data/shapes/player/RLeg.txt", %x, %y);
	%x = getWord(Avatar_LLegPreview.position, 0) + 64;
	%y = getWord(Avatar_LLegPreview.position, 1);
	AvatarGui_CreatePartMenu("Avatar_LLegMenu", "Avatar_SetLLeg", "base/data/shapes/player/LLeg.txt", %x, %y);
	%x = getWord(Avatar_RArmPreview.position, 0) + 64;
	%y = getWord(Avatar_RArmPreview.position, 1);
	AvatarGui_CreatePartMenu("Avatar_RArmMenu", "Avatar_SetRArm", "base/data/shapes/player/RArm.txt", %x, %y);
	%x = getWord(Avatar_LArmPreview.position, 0) + 64;
	%y = getWord(Avatar_LArmPreview.position, 1);
	AvatarGui_CreatePartMenu("Avatar_LArmMenu", "Avatar_SetLArm", "base/data/shapes/player/LArm.txt", %x, %y);
	%x = getWord(Avatar_RHandPreview.position, 0) + 64;
	%y = getWord(Avatar_RHandPreview.position, 1);
	AvatarGui_CreatePartMenu("Avatar_RHandMenu", "Avatar_SetRHand", "base/data/shapes/player/RHand.txt", %x, %y);
	%x = getWord(Avatar_LHandPreview.position, 0) + 64;
	%y = getWord(Avatar_LHandPreview.position, 1);
	AvatarGui_CreatePartMenu("Avatar_LHandMenu", "Avatar_SetLHand", "base/data/shapes/player/LHand.txt", %x, %y);
	exec("base/data/shapes/player/player.cs");
	Avatar_Preview.setObject("", "base/data/shapes/player/m.dts", "", 100);
	Avatar_Preview.setSequence("", 0, headup, 0);
	Avatar_Preview.setThreadPos("", 0, 0);
	Avatar_Preview.setSequence("", 1, run, 0.85);
	Avatar_Preview.setCameraRot(0.3, 0.6, 2.52);
	Avatar_Preview.setOrbitDist(4.34);
	for (%i = 0; %i < $numDecal; %i++)
	{
		if (fileBase($decal[%i]) $= $Pref::Player::DecalName)
		{
			$pref::Player::DecalColor = %i;
			break;
		}
	}
	for (%i = 0; %i < $numFace; %i++)
	{
		if (fileBase($face[%i]) $= $Pref::Player::FaceName)
		{
			$pref::Player::FaceColor = %i;
			break;
		}
	}
	Avatar_Prefix.setText($Pref::Player::ClanPrefix);
	Avatar_Suffix.setText($Pref::Player::ClanSuffix);
	Avatar_UpdatePreview();
}

function AvatarGui::onSleep(%this)
{
	$Pref::Player::ClanPrefix = Avatar_Prefix.getValue();
	$Pref::Player::ClanSuffix = Avatar_Suffix.getValue();
}

function AvatarGui_LoadAccentInfo(%arrayName, %fileName)
{
	%file = new FileObject();
	%file.openForRead(%fileName);
	if (%file.isEOF())
	{
		return;
	}
	%file.readLine();
	%line = %file.readLine();
	%wc = getWordCount(%line);
	for (%i = 0; %i < %wc; %i++)
	{
		%word = getWord(%line, %i);
		%command = "$" @ %arrayName @ "[" @ %i @ "] = " @ %word @ ";";
		eval(%command);
	}
	$num[%arrayName] = %wc;
	for (%lineCount = 0; !%file.isEOF(); %lineCount++)
	{
		%line = %file.readLine();
		%wc = getWordCount(%line);
		%hat = getWord(%line, 0);
		%allowed = getWords(%line, 1, %wc - 1);
		%command = "$" @ %arrayName @ "sAllowed[" @ %hat @ "] = \"" @ %allowed @ "\";";
		eval(%command);
	}
	%file.close();
	%file.delete();
}

function AvatarGui_CreatePartMenu(%name, %cmdString, %fileName, %xPos, %yPos)
{
	if (isObject(%name))
	{
		eval(%name @ ".delete();");
	}
	%newScroll = new GuiScrollCtrl();
	%newScroll.vScrollBar = "alwaysOn";
	%newScroll.hScrollBar = "alwaysOff";
	%newScroll.setProfile(ColorScrollProfile);
	Avatar_Window.add(%newScroll);
	%w = 64 + 12;
	%h = 64;
	%newScroll.resize(%xPos, %yPos, %w, %h);
	%newScroll.setName(%name);
	Avatar_Window.schedule(10, pushToBack, %name);
	%newBox = new GuiBitmapCtrl();
	%newScroll.add(%newBox);
	%newBox.setBitmap("base/client/ui/btnDecalBG");
	%newBox.wrap = 1;
	%newBox.resize(0, 0, 64, 64);
	%newBox.setName("Avatar_" @ fileBase(%fileName) @ "MenuBG");
	%file = new FileObject();
	%file.openForRead(%fileName);
	%itemCount = 0;
	%iconDir = "base/client/ui/avatarIcons/" @ fileBase(%fileName) @ "/";
	%varString = "$" @ fileBase(%fileName);
	for (%line = %file.readLine(); %line !$= ""; %line = %file.readLine())
	{
		%newImage = new GuiBitmapCtrl();
		%newBox.add(%newImage);
		%newImage.keepCached = 1;
		%newImage.setBitmap(%iconDir @ %line);
		%x = (%itemCount % 4) * 64;
		%y = mFloor(%itemCount / 4) * 64;
		%newImage.resize(%x, %y, 64, 64);
		%newButton = new GuiBitmapButtonCtrl();
		%newBox.add(%newButton);
		%newButton.setBitmap("base/client/ui/btnDecal");
		%newButton.setText(" ");
		%newButton.resize(%x, %y, 64, 64);
		%newButton.command = %cmdString @ "(" @ %itemCount @ "," @ %newImage @ ");";
		%cmd = %varString @ "[" @ %itemCount @ "] = \"" @ %line @ "\";";
		eval(%cmd);
		%itemCount++;
	}
	$num[fileBase(%fileName)] = %itemCount;
	%file.close();
	%file.delete();
	if (%itemCount >= 4)
	{
		%w = 4 * 64;
	}
	else
	{
		%w = %itemCount * 64;
	}
	%h = mFloor(%itemCount / 4 + 0.95) * 64;
	%newBox.resize(0, 0, %w, %h);
	if (%yPos + %h > 480)
	{
		%h = mFloor((480 - %yPos) / 64) * 64;
	}
	%newScroll.resize(%xPos, %yPos, %w + 12, %h);
	%newScroll.setVisible(0);
}

function AvatarGui_CreateSubPartMenu(%name, %cmdString, %subPartList, %xPos, %yPos)
{
	if (isObject(%name))
	{
		eval(%name @ ".delete();");
	}
	%baseName = strreplace(%name, "Menu", "");
	%baseName = strreplace(%baseName, "Avatar_", "");
	%newScroll = new GuiScrollCtrl();
	%newScroll.vScrollBar = "alwaysOn";
	%newScroll.hScrollBar = "alwaysOff";
	%newScroll.setProfile(ColorScrollProfile);
	Avatar_Window.add(%newScroll);
	%w = 64 + 12;
	%h = 64;
	%newScroll.resize(%xPos, %yPos, %w, %h);
	%newScroll.setName(%name);
	%newBox = new GuiSwatchCtrl();
	%newScroll.add(%newBox);
	%newBox.setColor("1 1 0 0.8");
	%newBox.resize(0, 0, 64, 64);
	%newBox.setName("Avatar_" @ %baseName @ "MenuBG");
	%iconDir = "base/client/ui/avatarIcons/" @ %baseName @ "/";
	%itemCount = 0;
	for (%line = getWord(%subPartList, %itemCount); %line !$= ""; %line = getWord(%subPartList, %itemCount))
	{
		%newImage = new GuiBitmapCtrl();
		%newBox.add(%newImage);
		%newImage.keepCached = 1;
		%newImage.setBitmap(%iconDir @ %line);
		%x = (%itemCount % 4) * 64;
		%y = mFloor(%itemCount / 4) * 64;
		%newImage.resize(%x, %y, 64, 64);
		%newButton = new GuiBitmapButtonCtrl();
		%newBox.add(%newButton);
		%newButton.setBitmap("base/client/ui/btnDecal");
		%newButton.setText(" ");
		%newButton.resize(%x, %y, 64, 64);
		%newButton.command = %cmdString @ "(" @ %itemCount @ "," @ %newImage @ ");";
		%itemCount++;
	}
	if (%itemCount >= 4)
	{
		%w = 4 * 64;
	}
	else
	{
		%w = %itemCount * 64;
	}
	%h = mFloor(%itemCount / 4 + 0.95) * 64;
	%newBox.resize(0, 0, %w, %h);
	if (%yPos + %h > 480)
	{
		%h = mFloor((480 - %yPos) / 64) * 64;
	}
	%newScroll.resize(%xPos, %yPos, %w + 12, %h);
	%newScroll.setVisible(0);
}

function AvatarGui_CreateColorMenu(%prefString, %colorList, %xPos, %yPos, %symmetryPrefString, %allowTrans)
{
	%rowLimit = 6;
	if (isObject(Avatar_ColorMenu))
	{
		Avatar_ColorMenu.delete();
	}
	$CurrColorPrefString = %prefString;
	$CurrColorSymmetryPrefString = %symmetryPrefString;
	%newScroll = new GuiScrollCtrl();
	%newScroll.setProfile(ColorScrollProfile);
	%newScroll.vScrollBar = "alwaysOn";
	%newScroll.hScrollBar = "alwaysOff";
	Avatar_Window.add(%newScroll);
	%newScroll.resize(%xPos, %yPos, 32 + 12, 32);
	%newScroll.setName("Avatar_ColorMenu");
	%newBox = new GuiSwatchCtrl();
	%newScroll.add(%newBox);
	%newBox.setColor("0 0 0 1");
	%newBox.resize(0, 0, 32, 32);
	%itemCount = 0;
	%colorIndex = getWord(%colorList, %count);
	for (%i = 0; %i < $Avatar::NumColors; %i++)
	{
		%color = $Avatar::Color[%i];
		if (%color $= "")
		{
			%color = "1 1 1 1";
		}
		%alpha = getWord(%color, 3);
		if (%allowTrans || !%allowTrans & %alpha >= 1)
		{
			%newSwatch = new GuiSwatchCtrl();
			%newBox.add(%newSwatch);
			%newSwatch.setColor(%color);
			%x = (%itemCount % %rowLimit) * 32;
			%y = mFloor(%itemCount / %rowLimit) * 32;
			%newSwatch.resize(%x, %y, 32, 32);
			%newButton = new GuiBitmapButtonCtrl();
			%newBox.add(%newButton);
			%newButton.setBitmap("base/client/ui/btnColor");
			%newButton.setText(" ");
			%newButton.resize(%x, %y, 32, 32);
			%newButton.command = "Avatar_AssignColor(" @ %i @ ");";
			%itemCount++;
		}
	}
	if (%itemCount >= %rowLimit)
	{
		%w = %rowLimit * 32;
	}
	else
	{
		%w = %itemCount * 32;
	}
	%h = (mFloor(%itemCount / %rowLimit) + 1) * 32;
	%newBox.resize(0, 0, %w, %h);
	if (%yPos + %h > 480)
	{
		%h = mFloor((480 - %yPos) / 32) * 32;
	}
	%newScroll.resize(%xPos, %yPos, %w + 12, %h);
}

function Avatar_AssignColor(%index)
{
	%color = $Avatar::Color[%index];
	%cmd = $CurrColorPrefString @ " = \"" @ %color @ "\";";
	eval(%cmd);
	$CurrColorPrefString = "";
	if ($pref::Player::Symmetry && $CurrColorSymmetryPrefString !$= "")
	{
		%cmd = $CurrColorSymmetryPrefString @ " = \"" @ %color @ "\";";
		eval(%cmd);
		$CurrColorSymmetryPrefString = "";
	}
	Avatar_ColorMenu.delete();
	Avatar_UpdatePreview();
}

function Avatar_ClickTorsoColor()
{
	%decalName = $decal[$pref::Player::DecalColor];
	%x = getWord(Avatar_TorsoColor.position, 0) + 32;
	%y = getWord(Avatar_TorsoColor.position, 1);
	if ($pref::Player::Authentic && $torsoColors[%decalName] !$= "")
	{
		AvatarGui_CreateColorMenu("$Pref::Player::TorsoColor", $torsoColors[%decalName], %x, %y);
	}
	else
	{
		AvatarGui_CreateColorMenu("$Pref::Player::TorsoColor", $normalColors, %x, %y);
	}
}

function Avatar_ClickPackColor()
{
	%packName = $pack[$pref::Player::Pack];
	%x = getWord(Avatar_PackColor.position, 0) + 32;
	%y = getWord(Avatar_PackColor.position, 1);
	if ($pref::Player::Authentic && $packColors[%packName] !$= "")
	{
		AvatarGui_CreateColorMenu("$Pref::Player::PackColor", $packColors[%packName], %x, %y);
	}
	else
	{
		AvatarGui_CreateColorMenu("$Pref::Player::PackColor", $normalColors, %x, %y);
	}
}

function Avatar_ClickSecondPackColor()
{
	%packName = $SecondPack[$Pref::Player::SecondPack];
	%x = getWord(Avatar_SecondPackColor.position, 0) + 32;
	%y = getWord(Avatar_SecondPackColor.position, 1);
	if ($pref::Player::Authentic && $packColors[%packName] !$= "")
	{
		AvatarGui_CreateColorMenu("$Pref::Player::SecondPackColor", $packColors[%packName], %x, %y);
	}
	else
	{
		AvatarGui_CreateColorMenu("$Pref::Player::SecondPackColor", $normalColors, %x, %y);
	}
}

function Avatar_ClickHatColor()
{
	%hatName = $hat[$pref::Player::Hat];
	%x = getWord(Avatar_HatColor.position, 0) + 32;
	%y = getWord(Avatar_HatColor.position, 1);
	if ($pref::Player::Authentic && $hatColors[%hatName] !$= "")
	{
		AvatarGui_CreateColorMenu("$Pref::Player::HatColor", $hatColors[%hatName], %x, %y);
	}
	else
	{
		AvatarGui_CreateColorMenu("$Pref::Player::HatColor", $normalColors, %x, %y);
	}
}

function Avatar_ClickAccentColor()
{
	%hatName = $hat[$pref::Player::Hat];
	%AccentArray = $accentsAllowed[%hatName];
	%accentName = getWord(%AccentArray, $pref::Player::Accent);
	%x = getWord(Avatar_AccentColor.position, 0) + 32;
	%y = getWord(Avatar_AccentColor.position, 1);
	AvatarGui_CreateColorMenu("$Pref::Player::AccentColor", $normalColors, %x, %y, "", 1);
	return;
	if (%accentName !$= "None")
	{
		%x = getWord(Avatar_AccentColor.position, 0) + 32;
		%y = getWord(Avatar_AccentColor.position, 1);
		if ($pref::Player::Authentic)
		{
			if ($accentColors[%accentName] $= "")
			{
				AvatarGui_CreateColorMenu("$Pref::Player::AccentColor", $allColors, %x, %y, "", 1);
			}
			else
			{
				AvatarGui_CreateColorMenu("$Pref::Player::AccentColor", $accentColors[%accentName], %x, %y, "", 1);
			}
		}
		else if ($accentColorsUnAuth[%accentName] $= "")
		{
			AvatarGui_CreateColorMenu("$Pref::Player::AccentColor", $allColors, %x, %y, "", 1);
		}
		else
		{
			AvatarGui_CreateColorMenu("$Pref::Player::AccentColor", $accentColorsUnAuth[%accentName], %x, %y, "", 1);
		}
	}
}

function Avatar_ClickHeadColor()
{
	%x = getWord(Avatar_HeadColor.position, 0) + 32;
	%y = getWord(Avatar_HeadColor.position, 1);
	AvatarGui_CreateColorMenu("$Pref::Player::HeadColor", $normalColors, %x, %y);
}

function Avatar_ClickHipColor()
{
	%x = getWord(Avatar_HipColor.position, 0) + 32;
	%y = getWord(Avatar_HipColor.position, 1);
	if ($pref::Player::Authentic)
	{
		AvatarGui_CreateColorMenu("$Pref::Player::HipColor", $basicColors, %x, %y);
	}
	else
	{
		AvatarGui_CreateColorMenu("$Pref::Player::HipColor", $normalColors, %x, %y);
	}
}

function Avatar_ClickRightLegColor()
{
	%x = getWord(Avatar_RightLegColor.position, 0) + 32;
	%y = getWord(Avatar_RightLegColor.position, 1);
	if ($pref::Player::Authentic)
	{
		AvatarGui_CreateColorMenu("$Pref::Player::RLegColor", $basicColors, %x, %y, "$Pref::Player::LLegColor");
	}
	else
	{
		AvatarGui_CreateColorMenu("$Pref::Player::RLegColor", $normalColors, %x, %y, "$Pref::Player::LLegColor");
	}
}

function Avatar_ClickRightArmColor()
{
	%x = getWord(Avatar_RightArmColor.position, 0) + 32;
	%y = getWord(Avatar_RightArmColor.position, 1);
	if ($pref::Player::Authentic)
	{
		AvatarGui_CreateColorMenu("$Pref::Player::RArmColor", $basicColors, %x, %y, "$Pref::Player::LArmColor");
	}
	else
	{
		AvatarGui_CreateColorMenu("$Pref::Player::RArmColor", $normalColors, %x, %y, "$Pref::Player::LArmColor");
	}
}

function Avatar_ClickRightHandColor()
{
	%x = getWord(Avatar_RightHandColor.position, 0) + 32;
	%y = getWord(Avatar_RightHandColor.position, 1);
	if ($pref::Player::Authentic)
	{
		AvatarGui_CreateColorMenu("$Pref::Player::RHandColor", $basicColors, %x, %y, "$Pref::Player::LHandColor");
	}
	else
	{
		AvatarGui_CreateColorMenu("$Pref::Player::RHandColor", $normalColors, %x, %y, "$Pref::Player::LHandColor");
	}
}

function Avatar_ClickLeftLegColor()
{
	%x = getWord(Avatar_LeftLegColor.position, 0) + 32;
	%y = getWord(Avatar_LeftLegColor.position, 1);
	if ($pref::Player::Authentic)
	{
		AvatarGui_CreateColorMenu("$Pref::Player::LLegColor", $basicColors, %x, %y, "$Pref::Player::RLegColor");
	}
	else
	{
		AvatarGui_CreateColorMenu("$Pref::Player::LLegColor", $normalColors, %x, %y, "$Pref::Player::RLegColor");
	}
}

function Avatar_ClickLeftArmColor()
{
	%x = getWord(Avatar_LeftArmColor.position, 0) + 32;
	%y = getWord(Avatar_LeftArmColor.position, 1);
	if ($pref::Player::Authentic)
	{
		AvatarGui_CreateColorMenu("$Pref::Player::LArmColor", $basicColors, %x, %y, "$Pref::Player::RArmColor");
	}
	else
	{
		AvatarGui_CreateColorMenu("$Pref::Player::LArmColor", $normalColors, %x, %y, "$Pref::Player::RArmColor");
	}
}

function Avatar_ClickLeftHandColor()
{
	%x = getWord(Avatar_LeftHandColor.position, 0) + 32;
	%y = getWord(Avatar_LeftHandColor.position, 1);
	if ($pref::Player::Authentic)
	{
		AvatarGui_CreateColorMenu("$Pref::Player::LHandColor", $basicColors, %x, %y, "$Pref::Player::RHandColor");
	}
	else
	{
		AvatarGui_CreateColorMenu("$Pref::Player::LHandColor", $normalColors, %x, %y, "$Pref::Player::RHandColor");
	}
}

function Avatar_TogglePartMenu(%obj)
{
	%vis = !%obj.visible;
	Avatar_HideAllPartMenus();
	%obj.setVisible(%vis);
}

function Avatar_HideAllPartMenus()
{
	Avatar_FaceMenu.setVisible(0);
	Avatar_DecalMenu.setVisible(0);
	Avatar_PackMenu.setVisible(0);
	Avatar_HatMenu.setVisible(0);
	Avatar_AccentMenu.setVisible(0);
	Avatar_ChestMenu.setVisible(0);
	Avatar_SecondPackMenu.setVisible(0);
	Avatar_ChestMenu.setVisible(0);
	Avatar_HipMenu.setVisible(0);
	Avatar_LArmMenu.setVisible(0);
	Avatar_RArmMenu.setVisible(0);
	Avatar_LHandMenu.setVisible(0);
	Avatar_RHandMenu.setVisible(0);
	Avatar_LLegMenu.setVisible(0);
	Avatar_RLegMenu.setVisible(0);
	if (isObject(Avatar_ColorMenu))
	{
		Avatar_ColorMenu.delete();
	}
}

function Avatar_SetFace(%index, %__unused)
{
	$Pref::Player::FaceName = fileBase($face[%index]);
	$pref::Player::FaceColor = %index;
	Avatar_FaceMenu.setVisible(0);
	Avatar_UpdatePreview();
}

function Avatar_SetDecal(%index, %__unused)
{
	$Pref::Player::DecalName = fileBase($decal[%index]);
	$pref::Player::DecalColor = %index;
	Avatar_DecalMenu.setVisible(0);
	Avatar_UpdatePreview();
}

function Avatar_SetPack(%index, %__unused)
{
	$pref::Player::Pack = %index;
	Avatar_PackMenu.setVisible(0);
	Avatar_UpdatePreview();
}

function Avatar_SetSecondPack(%index, %__unused)
{
	$Pref::Player::SecondPack = %index;
	Avatar_SecondPackMenu.setVisible(0);
	Avatar_UpdatePreview();
}

function Avatar_SetHat(%index, %__unused)
{
	$pref::Player::Hat = %index;
	Avatar_HatMenu.setVisible(0);
	%x = getWord(Avatar_AccentPreview.position, 0) + 64;
	%y = getWord(Avatar_AccentPreview.position, 1);
	AvatarGui_CreateSubPartMenu("Avatar_AccentMenu", "Avatar_SetAccent", $accentsAllowed[$hat[%index]], %x, %y);
	Avatar_UpdatePreview();
}

function Avatar_SetAccent(%index, %__unused)
{
	$pref::Player::Accent = %index;
	Avatar_AccentMenu.setVisible(0);
	Avatar_UpdatePreview();
}

function Avatar_SetChest(%index)
{
	$Pref::Player::Chest = %index;
	Avatar_HideAllPartMenus();
	Avatar_UpdatePreview();
}

function Avatar_SetHip(%index)
{
	$Pref::Player::Hip = %index;
	Avatar_HideAllPartMenus();
	Avatar_UpdatePreview();
}

function Avatar_SetLArm(%index)
{
	$Pref::Player::LArm = %index;
	if ($pref::Player::Symmetry == 1)
	{
		$Pref::Player::RArm = %index;
	}
	Avatar_HideAllPartMenus();
	Avatar_UpdatePreview();
}

function Avatar_SetRArm(%index)
{
	$Pref::Player::RArm = %index;
	if ($pref::Player::Symmetry == 1)
	{
		$Pref::Player::LArm = %index;
	}
	Avatar_HideAllPartMenus();
	Avatar_UpdatePreview();
}

function Avatar_SetLHand(%index)
{
	$Pref::Player::LHand = %index;
	Avatar_HideAllPartMenus();
	Avatar_UpdatePreview();
}

function Avatar_SetRHand(%index)
{
	$Pref::Player::RHand = %index;
	Avatar_HideAllPartMenus();
	Avatar_UpdatePreview();
}

function Avatar_SetLLeg(%index)
{
	$Pref::Player::LLeg = %index;
	Avatar_HideAllPartMenus();
	Avatar_UpdatePreview();
}

function Avatar_SetRLeg(%index)
{
	$Pref::Player::RLeg = %index;
	Avatar_HideAllPartMenus();
	Avatar_UpdatePreview();
}

function Avatar_UpdatePreview()
{
	Avatar_Preview.hideNode("", lski);
	Avatar_Preview.hideNode("", rski);
	for (%i = 0; %i < $num["Pack"]; %i++)
	{
		if ($pack[%i] !$= "None")
		{
			Avatar_Preview.hideNode("", $pack[%i]);
		}
	}
	for (%i = 0; %i < $num["SecondPack"]; %i++)
	{
		if ($SecondPack[%i] !$= "None")
		{
			Avatar_Preview.hideNode("", $SecondPack[%i]);
		}
	}
	for (%i = 0; %i < $num["Hat"]; %i++)
	{
		if ($hat[%i] !$= "None")
		{
			Avatar_Preview.hideNode("", $hat[%i]);
		}
	}
	for (%i = 0; %i < $num["Accent"]; %i++)
	{
		if ($Accent[%i] !$= "None")
		{
			Avatar_Preview.hideNode("", $Accent[%i]);
		}
	}
	for (%i = 0; %i < $num["Chest"]; %i++)
	{
		if ($Chest[%i] !$= "None")
		{
			Avatar_Preview.hideNode("", $Chest[%i]);
		}
	}
	for (%i = 0; %i < $num["Hip"]; %i++)
	{
		if ($Hip[%i] !$= "None")
		{
			Avatar_Preview.hideNode("", $Hip[%i]);
		}
	}
	for (%i = 0; %i < $num["RArm"]; %i++)
	{
		if ($RArm[%i] !$= "None")
		{
			Avatar_Preview.hideNode("", $RArm[%i]);
		}
	}
	for (%i = 0; %i < $num["LArm"]; %i++)
	{
		if ($LArm[%i] !$= "None")
		{
			Avatar_Preview.hideNode("", $LArm[%i]);
		}
	}
	for (%i = 0; %i < $num["RHand"]; %i++)
	{
		if ($RHand[%i] !$= "None")
		{
			Avatar_Preview.hideNode("", $RHand[%i]);
		}
	}
	for (%i = 0; %i < $num["LHand"]; %i++)
	{
		if ($LHand[%i] !$= "None")
		{
			Avatar_Preview.hideNode("", $LHand[%i]);
		}
	}
	for (%i = 0; %i < $num["RLeg"]; %i++)
	{
		if ($RLeg[%i] !$= "None")
		{
			Avatar_Preview.hideNode("", $RLeg[%i]);
		}
	}
	for (%i = 0; %i < $num["LLeg"]; %i++)
	{
		if ($LLeg[%i] !$= "None")
		{
			Avatar_Preview.hideNode("", $LLeg[%i]);
		}
	}
	if ($pref::Player::Pack == 0 && $Pref::Player::SecondPack == 0)
	{
		Avatar_Preview.setThreadPos("", 0, 0);
	}
	else
	{
		Avatar_Preview.setThreadPos("", 0, 1);
	}
	Avatar_Preview.setNodeColor("", "HeadSkin", $pref::Player::HeadColor);
	if ($hat[$pref::Player::Hat] !$= "" && $hat[$pref::Player::Hat] !$= "none")
	{
		Avatar_Preview.unHideNode("", $hat[$pref::Player::Hat]);
		Avatar_Preview.setNodeColor("", $hat[$pref::Player::Hat], $pref::Player::HatColor);
	}
	%partList = $accentsAllowed[$hat[$pref::Player::Hat]];
	%accentName = getWord(%partList, $pref::Player::Accent);
	if (%accentName !$= "" && %accentName !$= "none")
	{
		Avatar_Preview.unHideNode("", %accentName);
		Avatar_Preview.setNodeColor("", %accentName, $pref::Player::AccentColor);
	}
	if ($pack[$pref::Player::Pack] !$= "" && $pack[$pref::Player::Pack] !$= "none")
	{
		Avatar_Preview.unHideNode("", $pack[$pref::Player::Pack]);
		Avatar_Preview.setNodeColor("", $pack[$pref::Player::Pack], $pref::Player::PackColor);
	}
	if ($SecondPack[$Pref::Player::SecondPack] !$= "" && $SecondPack[$Pref::Player::SecondPack] !$= "none")
	{
		Avatar_Preview.unHideNode("", $SecondPack[$Pref::Player::SecondPack]);
		Avatar_Preview.setNodeColor("", $SecondPack[$Pref::Player::SecondPack], $pref::Player::SecondPackColor);
	}
	if ($Chest[$Pref::Player::Chest] !$= "" && $Chest[$Pref::Player::Chest] !$= "none")
	{
		Avatar_Preview.unHideNode("", $Chest[$Pref::Player::Chest]);
		Avatar_Preview.setNodeColor("", $Chest[$Pref::Player::Chest], $pref::Player::TorsoColor);
	}
	if ($Hip[$Pref::Player::Hip] !$= "" && $Hip[$Pref::Player::Hip] !$= "none")
	{
		Avatar_Preview.unHideNode("", $Hip[$Pref::Player::Hip]);
		Avatar_Preview.setNodeColor("", $Hip[$Pref::Player::Hip], $pref::Player::HipColor);
	}
	if ($RArm[$Pref::Player::RArm] !$= "" && $RArm[$Pref::Player::RArm] !$= "none")
	{
		Avatar_Preview.unHideNode("", $RArm[$Pref::Player::RArm]);
		Avatar_Preview.setNodeColor("", $RArm[$Pref::Player::RArm], $pref::Player::RArmColor);
	}
	if ($LArm[$Pref::Player::LArm] !$= "" && $LArm[$Pref::Player::LArm] !$= "none")
	{
		Avatar_Preview.unHideNode("", $LArm[$Pref::Player::LArm]);
		Avatar_Preview.setNodeColor("", $LArm[$Pref::Player::LArm], $pref::Player::LArmColor);
	}
	if ($RHand[$Pref::Player::RHand] !$= "" && $RHand[$Pref::Player::RHand] !$= "none")
	{
		Avatar_Preview.unHideNode("", $RHand[$Pref::Player::RHand]);
		Avatar_Preview.setNodeColor("", $RHand[$Pref::Player::RHand], $pref::Player::RHandColor);
	}
	if ($LHand[$Pref::Player::LHand] !$= "" && $LHand[$Pref::Player::LHand] !$= "none")
	{
		Avatar_Preview.unHideNode("", $LHand[$Pref::Player::LHand]);
		Avatar_Preview.setNodeColor("", $LHand[$Pref::Player::LHand], $pref::Player::LHandColor);
	}
	if ($RLeg[$Pref::Player::RLeg] !$= "" && $RLeg[$Pref::Player::RLeg] !$= "none")
	{
		Avatar_Preview.unHideNode("", $RLeg[$Pref::Player::RLeg]);
		Avatar_Preview.setNodeColor("", $RLeg[$Pref::Player::RLeg], $pref::Player::RLegColor);
	}
	if ($LLeg[$Pref::Player::LLeg] !$= "" && $LLeg[$Pref::Player::LLeg] !$= "none")
	{
		Avatar_Preview.unHideNode("", $LLeg[$Pref::Player::LLeg]);
		Avatar_Preview.setNodeColor("", $LLeg[$Pref::Player::LLeg], $pref::Player::LLegColor);
	}
	if ($Hip[$Pref::Player::Hip] !$= "" && $Hip[$Pref::Player::Hip] !$= "none")
	{
		Avatar_Preview.unHideNode("", $Hip[$Pref::Player::Hip]);
		Avatar_Preview.setNodeColor("", $Hip[$Pref::Player::Hip], $pref::Player::HipColor);
		if ($Hip[$Pref::Player::Hip] $= "skirtHip")
		{
			Avatar_Preview.unHideNode("", "SkirtTrimLeft");
			Avatar_Preview.unHideNode("", "SkirtTrimRight");
			Avatar_Preview.setNodeColor("", "SkirtTrimLeft", $pref::Player::LLegColor);
			Avatar_Preview.setNodeColor("", "SkirtTrimRight", $pref::Player::RLegColor);
			Avatar_Preview.hideNode("", $RLeg[$Pref::Player::RLeg]);
			Avatar_Preview.hideNode("", $LLeg[$Pref::Player::LLeg]);
		}
		else
		{
			Avatar_Preview.hideNode("", "SkirtTrimLeft");
			Avatar_Preview.hideNode("", "SkirtTrimRight");
		}
	}
	else
	{
		Avatar_Preview.hideNode("", "SkirtTrimLeft");
		Avatar_Preview.hideNode("", "SkirtTrimRight");
	}
	Avatar_Preview.setIflFrame("", decal, $pref::Player::DecalColor);
	Avatar_Preview.setIflFrame("", face, $pref::Player::FaceColor);
	Avatar_TorsoColor.setColor($pref::Player::TorsoColor);
	Avatar_ChestPreview.setColor($pref::Player::TorsoColor);
	Avatar_ColorAllIcons(Avatar_ChestMenuBG, $pref::Player::TorsoColor);
	Avatar_DecalBG.setColor($pref::Player::TorsoColor);
	Avatar_DecalMenuBG.setColor($pref::Player::TorsoColor);
	Avatar_HeadColor.setColor($pref::Player::HeadColor);
	Avatar_HeadBG.setColor($pref::Player::HeadColor);
	Avatar_faceMenuBG.setColor($pref::Player::HeadColor);
	Avatar_HipColor.setColor($pref::Player::HipColor);
	Avatar_HipPreview.setColor($pref::Player::HipColor);
	Avatar_ColorAllIcons(Avatar_HipMenuBG, $pref::Player::HipColor);
	Avatar_LeftArmColor.setColor($pref::Player::LArmColor);
	Avatar_LArmPreview.setColor($pref::Player::LArmColor);
	Avatar_ColorAllIcons(Avatar_LarmMenuBG, $pref::Player::LArmColor);
	Avatar_LeftHandColor.setColor($pref::Player::LHandColor);
	Avatar_LHandPreview.setColor($pref::Player::LHandColor);
	Avatar_ColorAllIcons(Avatar_LHandMenuBG, $pref::Player::LHandColor);
	Avatar_LeftLegColor.setColor($pref::Player::LLegColor);
	Avatar_LLegPreview.setColor($pref::Player::LLegColor);
	Avatar_ColorAllIcons(Avatar_LLegMenuBG, $pref::Player::LLegColor);
	Avatar_RightArmColor.setColor($pref::Player::RArmColor);
	Avatar_RArmPreview.setColor($pref::Player::RArmColor);
	Avatar_ColorAllIcons(Avatar_RarmMenuBG, $pref::Player::RArmColor);
	Avatar_RightHandColor.setColor($pref::Player::RHandColor);
	Avatar_RHandPreview.setColor($pref::Player::RHandColor);
	Avatar_ColorAllIcons(Avatar_RHandMenuBG, $pref::Player::RHandColor);
	Avatar_RightLegColor.setColor($pref::Player::RLegColor);
	Avatar_RLegPreview.setColor($pref::Player::RLegColor);
	Avatar_ColorAllIcons(Avatar_RLegMenuBG, $pref::Player::RLegColor);
	Avatar_HatColor.setColor($pref::Player::HatColor);
	Avatar_HatPreview.setColor($pref::Player::HatColor);
	Avatar_ColorAllIcons(Avatar_HatMenuBG, $pref::Player::HatColor);
	Avatar_AccentColor.setColor($pref::Player::AccentColor);
	Avatar_AccentPreview.setColor($pref::Player::AccentColor);
	Avatar_ColorAllIcons(Avatar_AccentMenuBG, $pref::Player::AccentColor);
	Avatar_PackColor.setColor($pref::Player::PackColor);
	Avatar_PackPreview.setColor($pref::Player::PackColor);
	Avatar_ColorAllIcons(Avatar_PackMenuBG, $pref::Player::PackColor);
	Avatar_SecondPackColor.setColor($pref::Player::SecondPackColor);
	Avatar_SecondPackPreview.setColor($pref::Player::SecondPackColor);
	Avatar_ColorAllIcons(Avatar_SecondPackMenuBG, $pref::Player::SecondPackColor);
	%iconDir = "base/client/ui/avatarIcons/";
	Avatar_FacePreview.setBitmap(%iconDir @ "face/" @ $face[$pref::Player::FaceColor]);
	Avatar_DecalPreview.setBitmap(%iconDir @ "decal/" @ $decal[$pref::Player::DecalColor]);
	if ($pack[$pref::Player::Pack] $= "none")
	{
		Avatar_PackPreview.setBitmap(%iconDir @ "none");
	}
	else
	{
		Avatar_PackPreview.setBitmap(%iconDir @ "pack/" @ $pack[$pref::Player::Pack]);
	}
	Avatar_SecondPackPreview.setBitmap(%iconDir @ "secondPack/" @ $SecondPack[$Pref::Player::SecondPack]);
	Avatar_HatPreview.setBitmap(%iconDir @ "hat/" @ $hat[$pref::Player::Hat]);
	Avatar_ChestPreview.setBitmap(%iconDir @ "chest/" @ $Chest[$Pref::Player::Chest]);
	Avatar_HipPreview.setBitmap(%iconDir @ "hip/" @ $Hip[$Pref::Player::Hip]);
	Avatar_LArmPreview.setBitmap(%iconDir @ "Larm/" @ $LArm[$Pref::Player::LArm]);
	Avatar_LHandPreview.setBitmap(%iconDir @ "Lhand/" @ $LHand[$Pref::Player::LHand]);
	Avatar_LLegPreview.setBitmap(%iconDir @ "Lleg/" @ $LLeg[$Pref::Player::LLeg]);
	Avatar_RArmPreview.setBitmap(%iconDir @ "Rarm/" @ $RArm[$Pref::Player::RArm]);
	Avatar_RHandPreview.setBitmap(%iconDir @ "Rhand/" @ $RHand[$Pref::Player::RHand]);
	Avatar_RLegPreview.setBitmap(%iconDir @ "Rleg/" @ $RLeg[$Pref::Player::RLeg]);
	%accentList = $accentsAllowed[$hat[$pref::Player::Hat]];
	%accent = getWord(%accentList, $pref::Player::Accent);
	if (%accent $= "")
	{
		Avatar_AccentPreview.setBitmap(%iconDir @ "none");
	}
	else
	{
		Avatar_AccentPreview.setBitmap(%iconDir @ "accent/" @ %accent);
	}
}

function Avatar_ColorAllIcons(%box, %color)
{
	%count = %box.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		%obj = %box.getObject(%i);
		if (%obj.getClassName() $= "GuiBitmapCtrl")
		{
			%obj.setColor(%color);
		}
	}
}

function Avatar_Randomize()
{
	$pref::Player::FaceColor = getRandom($numFace - 1);
	$Pref::Player::FaceName = fileBase($face[$pref::Player::FaceColor]);
	$pref::Player::DecalColor = getRandom($numDecal - 1);
	$Pref::Player::DecalName = fileBase($decal[$pref::Player::DecalColor]);
	$pref::Player::Hat = getRandom($numHat - 1);
	$pref::Player::Pack = getRandom($numPack - 1);
	$Pref::Player::SecondPack = getRandom($numSecondPack - 1);
	$Pref::Player::LArm = getRandom($numLArm - 1);
	$Pref::Player::RArm = getRandom($numRArm - 1);
	$Pref::Player::LHand = getRandom($numLHand - 1);
	$Pref::Player::RHand = getRandom($numRHand - 1);
	$Pref::Player::LLeg = getRandom($numLLeg - 1);
	$Pref::Player::RLeg = getRandom($numRLeg - 1);
	$Pref::Player::Chest = getRandom($numChest - 1);
	$Pref::Player::Hip = getRandom($numHip - 1);
	if ($Chest[$Pref::Player::Chest] $= "femchest")
	{
		if ($Pref::Player::FaceName !$= "smiley" && $Pref::Player::FaceName !$= "smileyCreepy" && strstr(strlwr($Pref::Player::FaceName), "female") == -1)
		{
			$Pref::Player::Chest = 0;
		}
	}
	else if (strpos(strlwr($Pref::Player::FaceName), "female") != -1)
	{
		$Pref::Player::Chest = 1;
	}
	%partList = $accentsAllowed[$hat[$pref::Player::Hat]];
	%count = getWordCount(%partList) - 1;
	if (%count > 0)
	{
		%chance = getRandom(5);
		if (%chance != 0)
		{
			$pref::Player::Accent = getRandom(%count - 1) + 1;
		}
		else
		{
			$pref::Player::Accent = 0;
		}
	}
	else
	{
		$pref::Player::Accent = 0;
	}
	%x = getWord(Avatar_AccentPreview.position, 0) + 64;
	%y = getWord(Avatar_AccentPreview.position, 1);
	%hatName = $hat[$pref::Player::Hat];
	%AccentArray = $accentsAllowed[%hatName];
	%accentName = getWord(%AccentArray, $pref::Player::Accent);
	AvatarGui_CreateSubPartMenu("Avatar_AccentMenu", "Avatar_SetAccent", %AccentArray, %x, %y);
	if ($pref::Player::Authentic == 1)
	{
		%torsoColorList = $torsoColors[$decal[$pref::Player::DecalColor]];
		%packColorList = $packColors[$pack[$pref::Player::Pack]];
		%hatColorList = $hatColors[$hat[$pref::Player::Hat]];
		%accentColorList = $accentColors[%accentName];
		if (%torsoColorList $= "")
		{
			%torsoColorList = $basicColors;
		}
		if (%packColorList $= "")
		{
			%packColorList = $basicColors;
		}
		if (%hatColorList $= "")
		{
			%hatColorList = $basicColors;
		}
		if (%accentColorList $= "")
		{
			%accentColorList = $basicColors;
		}
		%hipColorList = $basicColors;
		%LLegColorList = $basicColors;
		%RLegColorList = $basicColors;
		%LArmColorList = $basicColors;
		%RArmColorList = $basicColors;
		%LHandColorList = $basicColors;
		%RHandColorList = $basicColors;
	}
	else
	{
		%torsoColorList = $normalColors;
		%packColorList = $normalColors;
		%hatColorList = $normalColors;
		%accentColorList = $accentColorsUnAuth[%accentName];
		%hipColorList = $normalColors;
		%LLegColorList = $normalColors;
		%RLegColorList = $normalColors;
		%LArmColorList = $normalColors;
		%RArmColorList = $normalColors;
		%LHandColorList = $normalColors;
		%RHandColorList = $normalColors;
	}
	%count = getWordCount(%torsoColorList) - 1;
	$pref::Player::TorsoColor = Avatar_GetRandomColor(0);
	%count = getWordCount(%packColorList) - 1;
	$pref::Player::PackColor = Avatar_GetRandomColor(0);
	$pref::Player::SecondPackColor = Avatar_GetRandomColor(0);
	%count = getWordCount(%hatColorList) - 1;
	$pref::Player::HatColor = Avatar_GetRandomColor(0);
	%count = getWordCount(%accentColorList) - 1;
	$pref::Player::AccentColor = Avatar_GetRandomColor(1);
	%count = getWordCount(%hipColorList) - 1;
	$pref::Player::HipColor = Avatar_GetRandomColor(0);
	%count = getWordCount(%LLegColorList) - 1;
	$pref::Player::LLegColor = Avatar_GetRandomColor(0);
	%count = getWordCount(%LArmColorList) - 1;
	$pref::Player::LArmColor = Avatar_GetRandomColor(0);
	%count = getWordCount(%LHandColorList) - 1;
	$pref::Player::LHandColor = Avatar_GetRandomColor(0);
	if ($pref::Player::Symmetry == 1)
	{
		$pref::Player::RLegColor = $pref::Player::LLegColor;
		$pref::Player::RArmColor = $pref::Player::LArmColor;
		$pref::Player::RHandColor = $pref::Player::LHandColor;
	}
	else
	{
		%count = getWordCount(%RLegColorList) - 1;
		$pref::Player::RLegColor = Avatar_GetRandomColor(0);
		%count = getWordCount(%RArmColorList) - 1;
		$pref::Player::RArmColor = Avatar_GetRandomColor(0);
		%count = getWordCount(%RHandColorList) - 1;
		$pref::Player::RHandColor = Avatar_GetRandomColor(0);
	}
	Avatar_UpdatePreview();
}

function Avatar_GetRandomColor(%allowTrans)
{
	for (%i = 0; %i < 1000; %i++)
	{
		%color = $Avatar::Color[getRandom($Avatar::NumColors)];
		%alpha = getWord(%color, 3);
		if (%allowTrans)
		{
			return %color;
		}
		else if (%alpha >= 1)
		{
			return %color;
		}
	}
	return %color;
}

function Avatar_Done()
{
	Canvas.popDialog(AvatarGui);
	Canvas.popDialog(optionsDlg);
	clientCmdUpdatePrefs();
}

function Avatar_Clean()
{
	Avatar_FaceMenu.delete();
	Avatar_DecalMenu.delete();
	Avatar_HatMenu.delete();
	Avatar_PackMenu.delete();
	Avatar_SecondPackMenu.delete();
	Avatar_AccentMenu.delete();
	Avatar_ColorMenu.delete();
}

function AvatarGui::clickSetFavs(%this)
{
	AV_FavsHelper.setVisible(!AV_FavsHelper.isVisible());
}

function AvatarGui::ClickFav(%this, %idx)
{
	%idx = mFloor(%idx);
	%fileName = "base/config/client/AvatarFavorites/" @ %idx @ ".cs";
	if (AV_FavsHelper.isVisible())
	{
		export("$Pref::Player::*", %fileName, 0);
		AV_FavsHelper.setVisible(0);
	}
	else
	{
		exec(%fileName);
		Avatar_UpdatePreview();
	}
}

function clientCmdDoUpdates()
{
	schedule(10, 0, disconnect);
	$AU_AutoClose = 0;
	Canvas.schedule(1000, pushDialog, AutoUpdateGui);
}

function AutoUpdateGui::onWake(%this)
{
	AU_Text.setText("");
	AU_Time.setText("00:00:00");
	AU_Data.setText("0 KB / 0 KB");
	AU_Speed.setText("0 B/s");
	AU_Progress.setValue(0);
	AU_Done.setVisible(0);
	if (Canvas.getContent().getName() !$= "GuiEditorGui")
	{
		AU_start();
	}
}

function AutoUpdateGui::onSleep(%this)
{
	if (isObject(verTCPobj))
	{
	}
	if (isObject(patchTCPobj))
	{
	}
	$AU_AutoClose = 1;
}

function AutoUpdateGui::done(%this)
{
	if ($AU_AutoClose == 1)
	{
		Canvas.popDialog(AutoUpdateGui);
	}
	else
	{
		AU_Done.setVisible(1);
	}
}

function AU_Text::echo(%this, %text)
{
	%this.setText(%this.getValue() @ "\n" @ %text);
}

function AU_start()
{
	AU_Text.setText("Starting Blockland Update...");
	if (isObject(verTCPobj))
	{
		verTCPobj.delete();
	}
	new TCPObject(verTCPobj);
	verTCPobj.site = "master.blockland.us";
	verTCPobj.port = 80;
	verTCPobj.filePath = "/updates/" @ $Version @ ".txt";
	verTCPobj.lastLine = "crap";
	verTCPobj.connect(verTCPobj.site @ ":" @ verTCPobj.port);
}

function verTCPobj::onConnected(%this)
{
	AU_Text.echo("Getting version info...");
	%this.send("GET " @ %this.filePath @ " HTTP/1.1\r\nHost: " @ %this.site @ "\r\n\r\n");
}

function verTCPobj::onDNSFailed(%this)
{
	AU_Text.echo("DNS Failed");
	AutoUpdateGui.done();
}

function verTCPobj::onConnectFailed(%this)
{
	AU_Text.echo("Connection Failed");
	AutoUpdateGui.done();
}

function verTCPobj::onLine(%this, %line)
{
	if (strstr(%line, "404 Not Found") != -1)
	{
		AU_Text.echo("Update information not found.");
		%this.done = 1;
		AutoUpdateGui.done();
	}
	if (%this.lastLine $= "" && %this.done == 0)
	{
		if (%line !$= "")
		{
			AU_Text.echo("Update required");
			%this.done = 1;
			AU_GetUpdate(%line);
		}
		else
		{
			AU_Text.echo("You have the current version.");
			%this.done = 1;
			AutoUpdateGui.done();
		}
	}
	%this.lastLine = %line;
}

function AU_GetUpdate(%newVersion)
{
	%fileName = "/updates/" @ %newVersion;
	%saveName = "patches/" @ %newVersion;
	if (isObject(patchTCPobj))
	{
		patchTCPobj.delete();
	}
	new TCPObject(patchTCPobj);
	patchTCPobj.site = "master.blockland.us";
	patchTCPobj.port = 80;
	patchTCPobj.filePath = %fileName;
	patchTCPobj.saveTo = "patches/patch.exe";
	patchTCPobj.lastLine = "crap";
	patchTCPobj.badFile = 0;
	patchTCPobj.connect(patchTCPobj.site @ ":" @ patchTCPobj.port);
}

function patchTCPobj::onConnected(%this)
{
	AU_Text.echo("Requesting update: " @ fileBase(%this.filePath));
	%this.send("GET " @ %this.filePath @ " HTTP/1.1\r\nHost: " @ %this.site @ "\r\n\r\n");
}

function patchTCPobj::onDNSFailed(%this)
{
	AU_Text.echo("DNS Failed");
	AutoUpdateGui.done();
}

function patchTCPobj::onConnectFailed(%this)
{
	AU_Text.echo("Connection Failed");
	AutoUpdateGui.done();
}

function patchTCPobj::onLine(%this, %line)
{
	%word = getWord(%line, 0);
	if (%word $= "HTTP/1.1")
	{
		%httpCode = getWord(%line, 1);
		if (%httpCode !$= "200")
		{
			AU_Text.echo("HTTP Error: " @ getSubStr(%line, 9, 9999));
			MessageBoxOKCancel("Auto Update Error", "There was an http error while attempting to download an update.\n\nPlease visit <a:www.Blockland.us/index.asp?p=updates&v=" @ $Version @ ">Blockland.us/update</a> to get the latest updates.", "gotoWebPage(\"www.Blockland.us/index.asp?p=updates&v=" @ $Version @ "\");");
			%this.badFile = 1;
			%this.disconnect();
		}
	}
	else if (%word $= "Content-Length:")
	{
		%fileSize = getWord(%line, 1);
		%this.fileSize = %fileSize;
	}
	else if (%word $= "Content-Type:")
	{
		if (%this.badFile == 0)
		{
			%mimeType = getWord(%line, 1);
			if (%mimeType !$= "application/octet-stream")
			{
				AU_Text.echo("Bad mime type.");
				MessageBoxOKCancel("Auto Update Error", "There was an error while attempting to download an update.\n\nPlease visit <a:www.Blockland.us/index.asp?p=updates&v=" @ $Version @ ">Blockland.us/update</a> to get the latest updates.", "gotoWebPage(\"www.Blockland.us/index.asp?p=updates&v=" @ $Version @ "\");");
				%this.badFile = 1;
				%this.disconnect();
			}
		}
	}
	if (%this.badFile == 0)
	{
		if (%line $= "")
		{
			%this.setBinary(1);
		}
	}
}

function roundMegs(%val)
{
	%val *= 100;
	%val = mFloor(%val);
	%val /= 100;
	%frac = %val - mFloor(%val);
	if (strlen(%frac) < 2)
	{
		%val = %val @ "0";
	}
	return %val;
}

function getTimeString(%timeS)
{
	if (%timeS >= 3600)
	{
		%hours = mFloor(%timeS / 3600);
		%timeS -= %hours * 3600;
		%minutes = mFloor(%timeS / 60);
		%timeS -= %minutes * 60;
		%seconds = %timeS;
		if (%minutes < 10)
		{
			%minutes = "0" @ %minutes;
		}
		if (%seconds < 10)
		{
			%seconds = "0" @ %seconds;
		}
		return %hours @ ":" @ %minutes @ ":" @ %seconds;
	}
	else if (%timeS >= 60)
	{
		%minutes = mFloor(%timeS / 60);
		%timeS -= %minutes * 60;
		%seconds = %timeS;
		if (%seconds < 10)
		{
			%seconds = "0" @ %seconds;
		}
		return %minutes @ ":" @ %seconds;
	}
	else
	{
		%seconds = %timeS;
		if (%seconds < 10)
		{
			%seconds = "0" @ %seconds;
		}
		return "0:" @ %seconds;
	}
}

function patchTCPobj::onBinChunk(%this, %buffSize)
{
	AU_Progress.setValue(%buffSize / %this.fileSize);
	if (%this.fileSize >= 1048576)
	{
		%buffMegs = roundMegs(%buffSize / 1048576);
		%fileMegs = roundMegs(%this.fileSize / 1048576);
		AU_Data.setText(%buffMegs @ "MB / " @ %fileMegs @ "MB");
	}
	else
	{
		%buffK = roundMegs(%buffSize / 1024);
		%fileK = roundMegs(%this.fileSize / 1024);
		AU_Data.setText(%buffK @ "KB / " @ %fileK @ "KB");
	}
	%rawSpeed = 1;
	if (%this.startTime $= "")
	{
		AU_Text.echo("Recieving update...");
		AU_Text.echo("When the download is finished, the installation procedure will start automatically.");
		%this.startTime = getSimTime();
	}
	else
	{
		%elapsedTime = getSimTime() - %this.startTime;
		%elapsedTime = %elapsedTime / 1000;
		%rawSpeed = %buffSize / %elapsedTime;
		if (%rawSpeed >= 1048576)
		{
			%speed = roundMegs(%rawSpeed / 1048576);
			AU_Speed.setText(%speed @ " MB/s");
		}
		else if (%rawSpeed >= 1024)
		{
			%speed = roundMegs(%rawSpeed / 1024);
			AU_Speed.setText(%speed @ " KB/s");
		}
		else
		{
			AU_Speed.setText(%rawSpeed @ " B/s");
		}
	}
	%fileLeft = %this.fileSize - %buffSize;
	%timeLeftS = %fileLeft / %rawSpeed;
	AU_Time.setText(getTimeString(mFloor(%timeLeftS)));
	if (%buffSize >= %this.fileSize)
	{
		%this.disconnect();
		%this.saveBufferToFile(%this.saveTo);
		schedule(100, 0, AU_RunPatch, fileName(%this.saveTo));
	}
}

function AU_RunPatch(%name)
{
	WinExec(%name);
}

function MainMenuGui::onWake(%this)
{
	if (!isUnlocked())
	{
		Unlock();
	}
	MM_UpdateDemoDisplay(1);
	if (!isUnlocked())
	{
		Canvas.pushDialog(keyGui);
	}
	MM_Version.setText("Version: " @ $Version);
	if ($Pref::Input::SelectedDefaults == 0)
	{
		Canvas.pushDialog(DefaultControlsGui);
	}
	if ($Pref::Net::ConnectionType <= 0)
	{
		Canvas.pushDialog(SelectNetworkGui);
	}
	buildIFLs();
	if ($pref::TextureQuality $= "")
	{
		$pref::TextureQuality = 0;
	}
	optionsDlg.setTextureQuality($pref::TextureQuality);
	if ($pref::ParticleQuality $= "")
	{
		$pref::ParticleQuality = 0;
	}
	optionsDlg.setParticleQuality($pref::ParticleQuality);
	if ($pref::ShadowQuality $= "")
	{
		$pref::ShadowQuality = 0;
	}
	optionsDlg.setShadowQuality($pref::ShadowQuality);
	MM_AuthNameButton.setVisible(0);
	MM_AuthRetryButton.setVisible(0);
	if ($authed)
	{
		MM_AuthNameButton.setVisible(1);
	}
	if (isUnlocked())
	{
		if (!$authed)
		{
			$NewNetName = "";
			auth_Init();
		}
	}
	MainMenuGui.buildScreenshotList();
	if (MainMenuGui.screenShotCount <= 0)
	{
		return;
	}
	%pic = MainMenuGui.screenShot[getRandom(MainMenuGui.screenShotCount)];
	mm_Fade.pic = %pic;
	mm_Fade.setBitmap(%pic);
}

function MainMenuGui::onSleep()
{
	alxStop(%this.music);
}

function MainMenuGui::PlayMusic(%this)
{
	if (!%this.isAwake())
	{
		return;
	}
	%this.music = alxPlay(TitleMusic);
}

function MainMenuGui::buildScreenshotList(%this)
{
	if (strpos(getModPaths(), "screenshots") == -1)
	{
		setModPaths(getModPaths() @ ";" @ "screenshots");
	}
	%i = -1;
	for (%file = findFirstFile("screenshots/*.png"); %file !$= ""; %file = findNextFile("screenshots/*.png"))
	{
		%this.screenShot[%i++] = %file;
	}
	for (%file = findFirstFile("screenshots/*.jpg"); %file !$= ""; %file = findNextFile("screenshots/*.jpg"))
	{
		%this.screenShot[%i++] = %file;
	}
	%this.screenShotCount = %i;
}

function mm_Fade::OnWait(%this)
{
	MM_BG.setBitmap(mm_Fade.pic);
}

function mm_Fade::onDone(%this)
{
	if (MainMenuGui.screenShotCount <= 0)
	{
		return;
	}
	%pic = MainMenuGui.screenShot[getRandom(MainMenuGui.screenShotCount)];
	mm_Fade.pic = %pic;
	mm_Fade.setBitmap(%pic);
	%this.reset();
}

function buildIFLs()
{
	if ($BuiltIFLs == 1)
	{
		return;
	}
	$BuiltIFLs = 1;
	%file = new FileObject();
	%file.openForWrite("base/data/shapes/player/decal.ifl");
	for (%fullPath = findFirstFile("base/data/shapes/player/decals/*.png"); %fullPath !$= ""; %fullPath = findNextFile("base/data/shapes/player/decals/*.png"))
	{
		%fileName = fileName(%fullPath);
		%file.writeLine("decals/" @ %fileName);
	}
	%file.close();
	%file.openForWrite("base/data/shapes/player/face.ifl");
	for (%fullPath = findFirstFile("base/data/shapes/player/faces/*.png"); %fullPath !$= ""; %fullPath = findNextFile("base/data/shapes/player/faces/*.png"))
	{
		%fileName = fileName(%fullPath);
		%file.writeLine("faces/" @ %fileName);
	}
	%file.close();
	%file.delete();
}

function MM_StartButton::onMouseEnter(%this)
{
	alxPlay(Note4Sound);
}

function MM_JoinButton::onMouseEnter(%this)
{
	alxPlay(Note5Sound);
}

function MM_PlayerButton::onMouseEnter(%this)
{
	alxPlay(Note6Sound);
}

function MM_OptionsButton::onMouseEnter(%this)
{
	alxPlay(Note7Sound);
}

function MM_DemoButton::onMouseEnter(%this)
{
	alxPlay(Note8Sound);
}

function MM_QuitButton::onMouseEnter(%this)
{
	alxPlay(Note0Sound);
}

function MM_AboutButton::onMouseEnter(%this)
{
	alxPlay(Note1Sound);
}

function MM_CreditsButton::onMouseEnter(%this)
{
	alxPlay(Note2Sound);
}

function MM_UpdateDemoDisplay()
{
	if (isUnlocked())
	{
		DemoBanner.setVisible(0);
		JSG_demoBanner.setVisible(0);
		JSG_demoBanner2.setVisible(0);
		MM_BuyNowBlink(0);
		buyNowButton_B.setVisible(0);
		buyNowButton_W.setVisible(0);
		SM_demoBanner1.setVisible(0);
		SM_demoBanner2.setVisible(0);
		Demo_BrickCountBox.setVisible(0);
		Canvas.popDialog(demoBrickLimitGui);
	}
	else
	{
		DemoBanner.setVisible(1);
		JSG_demoBanner.setVisible(1);
		JSG_demoBanner.setColor("1 1 1 0.45");
		JSG_demoBanner2.setVisible(1);
		JSG_demoBanner2.setColor("1 1 1 0.45");
		MM_BuyNowBlink(1);
		SM_demoBanner1.setVisible(1);
		SM_demoBanner2.setVisible(1);
		SM_demoBanner1.setColor("1 1 1 0.35");
		SM_demoBanner2.setColor("1 1 1 0.35");
		Demo_BrickCountBox.setVisible(1);
	}
}

function MM_BuyNowBlink(%val)
{
	if (isEventPending($MM_BuyNowBlinkEvent))
	{
		cancel($MM_BuyNowBlinkEvent);
	}
	if (!%val)
	{
		return;
	}
	if (!MainMenuGui.isAwake())
	{
		return;
	}
	if (buyNowButton_B.isVisible())
	{
		buyNowButton_B.setVisible(0);
		buyNowButton_W.setVisible(1);
	}
	else
	{
		buyNowButton_B.setVisible(1);
		buyNowButton_W.setVisible(0);
	}
	$MM_BuyNowBlinkEvent = schedule(200, 0, MM_BuyNowBlink, 1);
}

function WhoTalk_Init()
{
	if (isObject($WhoTalkSO))
	{
		$WhoTalkSO.delete();
	}
	$WhoTalkSO = new ScriptObject()
	{
		class = WhoTalkSO;
		textObj = chatWhosTalkingText.getId();
		count = 0;
	};
}

function WhoTalk_Kill()
{
	if (isObject($WhoTalkSO))
	{
		$WhoTalkSO.textObj.setText("");
		$WhoTalkSO.delete();
	}
}

function WhoTalk_addID(%client)
{
	if (!isObject($WhoTalkSO))
	{
		WhoTalk_Init();
	}
	$WhoTalkSO.addID(%client);
}

function WhoTalk_removeID(%client)
{
	if (!isObject($WhoTalkSO))
	{
		return;
	}
	$WhoTalkSO.removeID(%client);
}

function WhoTalkSO::addID(%this, %client)
{
	if (%this.HasID(%client))
	{
		return;
	}
	%this.id[%this.count] = %client;
	%this.count++;
	%this.Display();
}

function WhoTalkSO::removeID(%this, %client)
{
	for (%i = 0; %i < %this.count; %i++)
	{
		if (%this.id[%i] == %client)
		{
			for (%j = %i + 1; %j < %this.count; %j++)
			{
				%this.id[%j - 1] = %this.id[%j];
			}
			%this.count--;
			%this.Display();
			return;
		}
	}
}

function WhoTalkSO::HasID(%this, %client)
{
	for (%i = 0; %i < %this.count; %i++)
	{
		if (%this.id[%i] == %client)
		{
			return 1;
		}
	}
	return 0;
}

function WhoTalkSO::Display(%this)
{
	%text = %this.textObj;
	if (isObject(%text))
	{
		%buff = "";
		for (%i = 0; %i < %this.count; %i++)
		{
			%buff = %buff @ " " @ getField(lstAdminPlayerList.getRowTextById(%this.id[%i]), 0);
		}
		%text.setText(%buff);
	}
	else
	{
		error("ERROR: WhoTalkSO::Display() - text object not found.");
	}
}

function newMessageHud::onWake(%this)
{
}

function newMessageHud::onSleep(%this)
{
	commandToServer('stopTalking');
}

function newMessageHud::open(%this, %channel)
{
	%this.channel = %channel;
	if (%channel $= "SAY")
	{
		NMH_Channel.setText("\c0" @ %channel @ ":");
	}
	else if (%channel $= "TEAM")
	{
		NMH_Channel.setText("\c1" @ %channel @ ":");
	}
	else
	{
		error("ERROR(): newMessageHud::open() - unknown channel \"" @ %channel @ "\"");
		return;
	}
	if (!%this.isAwake())
	{
		NMH_Type.setValue("");
		Canvas.pushDialog(%this);
		%this.updatePosition();
		%this.schedule(10, updateTypePosition);
	}
}

function newMessageHud::updateTypePosition(%this)
{
	%pixelWidth = NMH_Channel.getPixelWidth();
	if (%pixelWidth < 5)
	{
		%this.schedule(100, updateTypePosition);
		return;
	}
	%x = getWord(NMH_Channel.getPosition(), 0);
	%x += %pixelWidth + 2;
	%y = 0;
	%w = (getWord(NMH_Box.getExtent(), 0) - %x) - 2;
	%h = getWord(NMH_Type.getExtent(), 1);
	NMH_Type.resize(%x, %y, %w, %h);
}

function newMessageHud::updatePosition(%this)
{
	if (!%this.isAwake())
	{
		return;
	}
	%x = getWord(NMH_Box.getPosition(), 0);
	%y = getWord(newChatText.getPosition(), 1);
	%y += getWord(newChatText.getExtent(), 1);
	%w = getWord(NMH_Box.getExtent(), 0);
	%h = getWord(NMH_Type.getExtent(), 1);
	NMH_Box.resize(%x, %y, %w, %h);
}

function NMH_Type::type(%this)
{
	%text = %this.getValue();
	if (strlen(%text) == 1)
	{
		if (%text !$= "/")
		{
			commandToServer('StartTalking');
		}
	}
}

function NMH_Type::send(%this)
{
	%text = %this.getValue();
	%firstChar = getSubStr(%text, 0, 1);
	if (%firstChar $= "/")
	{
		%newText = getSubStr(%text, 1, 256);
		%command = getWord(%newText, 0);
		%par1 = getWord(%newText, 1);
		%par2 = getWord(%newText, 2);
		%par3 = getWord(%newText, 3);
		%par4 = getWord(%newText, 4);
		%par5 = getWord(%newText, 4);
		%par6 = getWord(%newText, 4);
		commandToServer(addTaggedString(%command), %par1, %par2, %par3, %par4, %par5, %par6);
	}
	else if (newMessageHud.channel $= "SAY")
	{
		commandToServer('messageSent', %text);
	}
	else if (newMessageHud.channel $= "TEAM")
	{
		commandToServer('teamMessageSent', %text);
	}
	else
	{
		error("ERROR: NMH_Type::Send() - unknown channel \"" @ newMessageHuf.channel @ "\"");
	}
	Canvas.popDialog(newMessageHud);
}

if ($Pref::Chat::FontSize < 14)
{
	$Pref::Chat::FontSize = 14;
}
function onChatMessage(%message, %__unused, %__unused)
{
	if (strlen(%message) > 0)
	{
		newChatHud_AddLine(%message);
	}
}

function onServerMessage(%message)
{
	if (strlen(%message) > 0)
	{
		newChatHud_AddLine(%message);
	}
}

function newChatHud_Init()
{
	if ($Pref::Chat::CacheLines $= "")
	{
		$Pref::Chat::CacheLines = 1000;
		$Pref::Chat::MaxDisplayLines = 10;
		$Pref::Chat::LineTime = 4000;
	}
	if ($Pref::Chat::CacheLines < 100)
	{
		$Pref::Chat::CacheLines = 100;
	}
	if ($Pref::Chat::CacheLines > 50000)
	{
		$Pref::Chat::CacheLines = 50000;
	}
	if ($Pref::Chat::MaxDisplayLines < 4)
	{
		$Pref::Chat::MaxDisplayLines = 4;
	}
	if ($Pref::Chat::MaxDisplayLines > 100)
	{
		$Pref::Chat::MaxDisplayLines = 100;
	}
	if ($Pref::Chat::LineTime < 1000)
	{
		$Pref::Chat::LineTime = 1000;
	}
	if ($Pref::Chat::LineTime > 60000)
	{
		$Pref::Chat::LineTime = 60000;
	}
	$NewChatSO = new ScriptObject()
	{
		class = NewChatSO;
		size = $Pref::Chat::CacheLines;
		maxLines = $Pref::Chat::MaxDisplayLines;
		lineTime = $Pref::Chat::LineTime;
		head = 0;
		tail = 0;
		textObj = newChatText.getId();
		pageUpEnd = -1;
	};
	if ($Pref::Chat::ShowAllLines == 1)
	{
		$NewChatSO.pageUpEnd = 0;
	}
	if ($Pref::Gui::ChatSize $= "")
	{
		$Pref::Gui::ChatSize = 1;
	}
	OPT_SetChatSize($Pref::Gui::ChatSize);
}

function newChatHud_UpdateScrollDownIndicator()
{
	if ($NewChatSO.pageUpEnd == -1 || $NewChatSO.pageUpEnd == $NewChatSO.head)
	{
		chatScrollDownIndicator.setVisible(0);
	}
	else
	{
		chatScrollDownIndicator.setVisible(1);
	}
	newChatHud_UpdateIndicatorPosition();
}

function newChatHud_UpdateIndicatorPosition()
{
	%w = getWord(chatScrollDownIndicator.getExtent(), 0);
	%h = getWord(chatScrollDownIndicator.getExtent(), 1);
	%x = getWord(chatScrollDownIndicator.getPosition(), 0);
	%y = (getWord($NewChatSO.textObj.getPosition(), 1) + getWord($NewChatSO.textObj.getExtent(), 1)) - %h;
	chatScrollDownIndicator.resize(%x, %y, %w, %h);
}

function newChatHud_AddLine(%line)
{
	if (!isObject($NewChatSO))
	{
		newChatHud_Init();
	}
	if (strstr(%line, "<a:") != -1)
	{
		%line = %line @ "</a>";
	}
	$NewChatSO.addLine(%line);
	newChatText.forceReflow();
	newMessageHud.updatePosition();
	newChatHud_UpdateScrollDownIndicator();
}

function NewChatSO::displayLatest(%this)
{
	if (isEventPending(%this.displaySchedule))
	{
		cancel(%this.displaySchedule);
	}
	%text = %this.textObj;
	if (isObject(%text))
	{
		%buff = "";
		%currTime = getSimTime();
		for (%i = 0; %i < %this.maxLines; %i++)
		{
			%pos = (%this.head - 1) - %i;
			if (%pos == -1)
			{
				%pos = %this.size + %pos;
			}
			if (%currTime - %this.time[%pos] > %this.lineTime || %i == %this.maxLines - 1 || (%pos + 1) % %this.size == %this.tail)
			{
				if (%pos != %this.head - 1)
				{
					%this.displaySchedule = %this.schedule(500, displayLatest);
				}
				%showMouseTip = 0;
				for (%i--; %i >= 0; %i--)
				{
					%pos = (%this.head - 1) - %i;
					if (%pos < 0)
					{
						%pos = %this.size + %pos;
					}
					%buff = %buff @ %this.line[%pos] @ "\n";
					if (%showMouseTip == 0)
					{
						if (strstr(%this.line[%pos], "<a:") != -1)
						{
							%showMouseTip = 1;
						}
					}
				}
				break;
			}
		}
		%text.setValue(%buff);
		%text.forceReflow();
		newMessageHud.updatePosition();
		newChatHud_UpdateScrollDownIndicator();
	}
	else
	{
		error("ERROR: NewChatSO::displayLatest() - %this.textObj not defined");
	}
	if (%showMouseTip && $pref::HUD::showToolTips)
	{
		MouseToolTip.setVisible(1);
		%key = strupr(getWord(moveMap.getBinding("toggleCursor"), 1));
		MouseToolTip.setValue("\c6TIP: Press " @ %key @ " to toggle mouse and click on links");
	}
	else if (!Canvas.isCursorOn())
	{
		MouseToolTip.setVisible(0);
	}
	if (MouseToolTip.isVisible())
	{
		$NewChatSO.textObj.forceReflow();
		%w = getWord(MouseToolTip.getExtent(), 0);
		%h = getWord(MouseToolTip.getExtent(), 1);
		%x = getWord(MouseToolTip.getPosition(), 0);
		%y = getWord($NewChatSO.textObj.getPosition(), 1) + getWord($NewChatSO.textObj.getExtent(), 1) + %h;
		MouseToolTip.resize(%x, %y, %w, %h);
	}
}

function NewChatSO::addLine(%this, %line)
{
	%this.line[%this.head] = %line;
	%this.time[%this.head] = getSimTime();
	%doPage = 0;
	if (%this.pageUpEnd == %this.head)
	{
		%doPage = 1;
	}
	%this.head++;
	if (%this.head >= %this.size)
	{
		%this.head = 0;
	}
	if (%this.head == %this.tail)
	{
		%this.tail++;
	}
	if (%this.tail >= %this.size)
	{
		%this.tail = 0;
	}
	if (%this.pageUpEnd == -1)
	{
		%this.displayLatest();
	}
	else if (((%this.pageUpEnd - %this.maxLines) + 1) % %this.size == %this.tail)
	{
		%this.pageUpEnd = (%this.pageUpEnd + 1) % %this.size;
		%this.displayPage();
	}
	if (%doPage)
	{
		%this.pageUpEnd = %this.head;
		%this.displayPage();
	}
}

function NewChatSO::pageUp(%this)
{
	if (isEventPending(%this.displaySchedule))
	{
		cancel(%this.displaySchedule);
	}
	if (%this.pageUpEnd == -1)
	{
		%this.pageUpEnd = %this.head;
		$Pref::Chat::ShowAllLines = 1;
	}
	else if (%this.tail == 0)
	{
		if (%this.pageUpEnd <= %this.tail + %this.maxLines * 2)
		{
			if (%this.head <= %this.tail + %this.maxLines)
			{
			}
			else
			{
				%this.pageUpEnd = %this.tail + %this.maxLines;
			}
		}
		else
		{
			%this.pageUpEnd -= %this.maxLines;
		}
	}
	else
	{
		for (%i = 0; %i <= %this.maxLines * 2; %i++)
		{
			%pos = %this.pageUpEnd - %i;
			if (%pos < 0)
			{
				%pos += %this.size;
			}
			if (%pos == %this.tail)
			{
				break;
			}
		}
		%this.pageUpEnd = %pos + %this.maxLines;
		if (%this.pageUpEnd > %this.size)
		{
			%this.pageUpEnd -= %this.size;
		}
	}
	newChatHud_UpdateScrollDownIndicator();
	%this.displayPage();
	%this.textObj.forceReflow();
	newMessageHud.updatePosition();
}

function NewChatSO::pageDown(%this)
{
	if (%this.pageUpEnd == %this.head || %this.pageUpEnd == -1)
	{
		%this.pageUpEnd = -1;
		%this.displayLatest();
		$Pref::Chat::ShowAllLines = 0;
	}
	else
	{
		if (isEventPending(%this.displaySchedule))
		{
			cancel(%this.displaySchedule);
		}
		for (%i = 0; %i <= %this.maxLines; %i++)
		{
			%pos = %this.pageUpEnd + %i;
			if (%pos >= %this.size)
			{
				%pos -= %this.size;
			}
			if (%pos == %this.head)
			{
				break;
			}
		}
		%this.pageUpEnd = %pos;
		%this.displayPage();
	}
	newChatHud_UpdateScrollDownIndicator();
}

function NewChatSO::displayPage(%this)
{
	%text = %this.textObj;
	if (!isObject(%text))
	{
		error("ERROR: NewChatSO::DisplayPage() - textObj is not defined for object " @ %this.getName() @ " (" @ %this @ ")");
		return;
	}
	%start = %this.pageUpEnd - %this.maxLines;
	if (%start < 0)
	{
		%start += %this.size;
	}
	%showMouseTip = 0;
	%buff = "";
	for (%i = 0; %i < %this.maxLines; %i++)
	{
		%pos = (%start + %i) % %this.size;
		if (%this.line[%pos] !$= "")
		{
			%buff = %buff @ %this.line[%pos] @ "\n";
		}
		if (%showMouseTip == 0)
		{
			if (strstr(%this.line[%pos], "<a:") != -1)
			{
				%showMouseTip = 1;
			}
		}
	}
	%text.setValue(%buff);
	if (%showMouseTip && $pref::HUD::showToolTips)
	{
		%text.forceReflow();
		MouseToolTip.setVisible(1);
		%key = strupr(getWord(moveMap.getBinding("toggleCursor"), 1));
		MouseToolTip.setValue("\c6TIP: Press " @ %key @ " to toggle mouse and click on links");
	}
	else if (!Canvas.isCursorOn())
	{
		MouseToolTip.setVisible(0);
	}
	if (MouseToolTip.isVisible())
	{
		$NewChatSO.textObj.forceReflow();
		%w = getWord(MouseToolTip.getExtent(), 0);
		%h = getWord(MouseToolTip.getExtent(), 1);
		%x = getWord(MouseToolTip.getPosition(), 0);
		%y = getWord($NewChatSO.textObj.getPosition(), 1) + getWord($NewChatSO.textObj.getExtent(), 1) + %h;
		MouseToolTip.resize(%x, %y, %w, %h);
	}
}

function NewChatSO::update(%this)
{
	if (%this.pageUpEnd == -1)
	{
		%this.displayLatest();
	}
	else
	{
		%this.displayPage();
	}
}

function NewChatSO::dumpLines(%this)
{
	echo("head = ", %this.head);
	echo("tail = ", %this.tail);
	for (%i = 0; %i < %this.size; %i++)
	{
		if (%this.head == %i)
		{
			echo("line " @ %i @ " : " @ %this.line[%i] @ "\c0<-HEAD");
		}
		else if (%this.tail == %i)
		{
			echo("line " @ %i @ " : " @ %this.line[%i] @ "\c0<-TAIL");
		}
		else
		{
			echo("line " @ %i @ " : " @ %this.line[%i]);
		}
	}
}

function SelectNetworkGui::onWake(%this)
{
}

function SelectNetworkGui::onSleep(%this)
{
	if ($Pref::Net::ConnectionType > 0 && $Pref::Input::SelectedDefaults > 0)
	{
		if (!$Pref::DontUpdate)
		{
			$AU_AutoClose = 1;
			Canvas.pushDialog(AutoUpdateGui);
		}
	}
}

function DefaultControlsGui::onWake()
{
	OPT_Mouse0.setValue(0);
	OPT_Mouse1.setValue(0);
	OPT_Mouse2.setValue(1);
	OPT_Keyboard0.setValue(1);
	OPT_Keyboard1.setValue(0);
}

function DefaultControlsGui::onSleep()
{
	if ($Pref::Input::SelectedDefaults == 0)
	{
		$Pref::Input::SelectedDefaults = 1;
		if ($Pref::Net::ConnectionType > 0 && $Pref::Input::SelectedDefaults > 0)
		{
			if (!$Pref::DontUpdate)
			{
				$AU_AutoClose = 1;
				Canvas.pushDialog(AutoUpdateGui);
			}
		}
	}
}

function DefaultControlsGui::apply()
{
	%mouse = -1;
	if (OPT_Mouse0.getValue())
	{
		%mouse = 0;
	}
	else if (OPT_Mouse1.getValue())
	{
		%mouse = 1;
	}
	else if (OPT_Mouse2.getValue())
	{
		%mouse = 2;
	}
	else if (OPT_Mouse3.getValue())
	{
		%mouse = 3;
	}
	else
	{
		MessageBoxOK("Error", "Please select a mouse type.");
		return;
	}
	%keyboard = -1;
	if (OPT_Keyboard0.getValue())
	{
		%keyboard = 0;
	}
	else if (OPT_Keyboard1.getValue())
	{
		%keyboard = 1;
	}
	else
	{
		MessageBoxOK("Error", "Please select a keyboard type.");
		return;
	}
	moveMap.delete();
	new ActionMap(moveMap);
	$pref::Input::noobjet = 0;
	moveMap.bindCmd(keyboard, "escape", "", "escapeMenu.toggle();");
	moveMap.bind(keyboard, "w", moveforward);
	moveMap.bind(keyboard, "s", movebackward);
	moveMap.bind(keyboard, "a", moveleft);
	moveMap.bind(keyboard, "d", moveright);
	moveMap.bind(keyboard, "space", Jump);
	moveMap.bind(keyboard, "lshift", Crouch);
	moveMap.bind(keyboard, "c", Walk);
	moveMap.bind(keyboard, "f", toggleZoom);
	moveMap.bind(keyboard, "z", toggleFreeLook);
	moveMap.bind(keyboard, "tab", toggleFirstPerson);
	moveMap.bind(keyboard, "f8", dropCameraAtPlayer);
	moveMap.bind(keyboard, "f7", dropPlayerAtCamera);
	moveMap.bind(keyboard, "t", GlobalChat);
	moveMap.bind(keyboard, "y", TeamChat);
	moveMap.bind(keyboard, "pageup", PageUpNewChatHud);
	moveMap.bind(keyboard, "pagedown", PageDownNewChatHud);
	moveMap.bind(keyboard, "m", ToggleCursor);
	moveMap.bind(keyboard, 1, useBricks);
	moveMap.bind(keyboard, "q", useTools);
	moveMap.bind(keyboard, "e", useSprayCan);
	moveMap.bind(keyboard, "ctrl w", dropTool);
	moveMap.bind(keyboard, 2, useSecondSlot);
	moveMap.bind(keyboard, 3, useThirdSlot);
	moveMap.bind(keyboard, 4, useFourthSlot);
	moveMap.bind(keyboard, 5, useFifthSlot);
	moveMap.bind(keyboard, 6, useSixthSlot);
	moveMap.bind(keyboard, 7, useSeventhSlot);
	moveMap.bind(keyboard, 8, useEighthSlot);
	moveMap.bind(keyboard, 9, useNinthSlot);
	moveMap.bind(keyboard, 0, useTenthSlot);
	moveMap.bind(keyboard, "ctrl z", undoBrick);
	moveMap.bind(keyboard, "lalt", toggleSuperShift);
	moveMap.bind(keyboard, "ctrl a", openAdminWindow);
	moveMap.bind(keyboard, "ctrl o", openOptionsWindow);
	moveMap.bind(keyboard, "ctrl p", doScreenShot);
	moveMap.bind(keyboard, "ctrl k", Suicide);
	moveMap.bind(keyboard, "shift p", doHudScreenshot);
	moveMap.bind(keyboard, "shift-ctrl p", doDofScreenShot);
	moveMap.bind(keyboard, "f2", showPlayerList);
	moveMap.bind(keyboard, "ctrl n", toggleNetGraph);
	moveMap.bind(keyboard, "f3", startRecordingDemo);
	moveMap.bind(keyboard, "f4", stopRecordingDemo);
	moveMap.bind(mouse0, "xaxis", yaw);
	moveMap.bind(mouse0, "yaxis", pitch);
	moveMap.bind(mouse0, "button0", mouseFire);
	moveMap.bind(keyboard, "b", openBSD);
	moveMap.bind(keyboard, "f5", ToggleShapeNameHud);
	moveMap.bind(keyboard, "l", useLight);
	moveMap.bind(keyboard, "period", NextSeat);
	moveMap.bind(keyboard, "comma", PrevSeat);
	if (%keyboard == 0)
	{
		moveMap.bind(keyboard, "numpad8", shiftBrickAway);
		moveMap.bind(keyboard, "numpad2", shiftBrickTowards);
		moveMap.bind(keyboard, "numpad4", shiftBrickLeft);
		moveMap.bind(keyboard, "numpad6", shiftBrickRight);
		moveMap.bind(keyboard, "+", shiftBrickUp);
		moveMap.bind(keyboard, "numpad5", shiftBrickDown);
		moveMap.bind(keyboard, "numpad3", shiftBrickThirdUp);
		moveMap.bind(keyboard, "numpad1", shiftBrickThirdDown);
		moveMap.bind(keyboard, "numpad9", RotateBrickCW);
		moveMap.bind(keyboard, "numpad7", RotateBrickCCW);
		moveMap.bind(keyboard, "numpadenter", plantBrick);
		moveMap.bind(keyboard, "numpad0", cancelBrick);
		moveMap.bind(keyboard, "alt numpad8", superShiftBrickAwayProxy);
		moveMap.bind(keyboard, "alt numpad2", superShiftBrickTowardsProxy);
		moveMap.bind(keyboard, "alt numpad4", superShiftBrickLeftProxy);
		moveMap.bind(keyboard, "alt numpad6", superShiftBrickRightProxy);
		moveMap.bind(keyboard, "alt +", superShiftBrickUpProxy);
		moveMap.bind(keyboard, "alt numpad5", superShiftBrickDownProxy);
	}
	else
	{
		moveMap.bind(keyboard, "i", shiftBrickAway);
		moveMap.bind(keyboard, "k", shiftBrickTowards);
		moveMap.bind(keyboard, "j", shiftBrickLeft);
		moveMap.bind(keyboard, "l", shiftBrickRight);
		moveMap.bind(keyboard, "p", shiftBrickUp);
		moveMap.bind(keyboard, ";", shiftBrickDown);
		moveMap.bind(keyboard, "period", shiftBrickThirdUp);
		moveMap.bind(keyboard, "comma", shiftBrickThirdDown);
		moveMap.bind(keyboard, "o", RotateBrickCW);
		moveMap.bind(keyboard, "u", RotateBrickCCW);
		moveMap.bind(keyboard, "return", plantBrick);
		moveMap.bind(keyboard, "/", cancelBrick);
		moveMap.bind(keyboard, "alt i", superShiftBrickAwayProxy);
		moveMap.bind(keyboard, "alt k", superShiftBrickTowardsProxy);
		moveMap.bind(keyboard, "alt j", superShiftBrickLeftProxy);
		moveMap.bind(keyboard, "alt l", superShiftBrickRightProxy);
		moveMap.bind(keyboard, "alt p", superShiftBrickUpProxy);
		moveMap.bind(keyboard, "alt ;", superShiftBrickDownProxy);
	}
	if (%mouse == 0)
	{
		$pref::Input::noobjet = 1;
		moveMap.bind(keyboard, "up", invUp);
		moveMap.bind(keyboard, "down", invDown);
		moveMap.bind(keyboard, "left", invLeft);
		moveMap.bind(keyboard, "right", invRight);
	}
	else if (%mouse == 1)
	{
		moveMap.bind(mouse0, "button1", Jet);
		moveMap.bind(keyboard, "up", invUp);
		moveMap.bind(keyboard, "down", invDown);
		moveMap.bind(keyboard, "left", invLeft);
		moveMap.bind(keyboard, "right", invRight);
	}
	else if (%mouse == 2)
	{
		moveMap.bind(mouse0, "button1", Jet);
		moveMap.bind(mouse0, "zaxis", scrollInventory);
		moveMap.bind(keyboard, "ctrl E", invLeft);
	}
	else if (%mouse == 3)
	{
		moveMap.bind(mouse0, "button1", Jet);
		moveMap.bind(mouse0, "zaxis", scrollInventory);
		moveMap.bind(keyboard, "ctrl E", invLeft);
	}
	moveMap.push();
	Canvas.popDialog(DefaultControlsGui);
	OptRemapList.fillList();
}

function SavingGui::onWake(%this)
{
}

function SavingGui::onRender(%this)
{
	if ($SaveBricksPath !$= "")
	{
		%this.schedule(10, save);
	}
}

function SavingGui::save(%this)
{
	if ($SaveBricksPath !$= "")
	{
		saveBricks($SaveBricksPath, $SaveBricksDescription);
		$SaveBricksPath = "";
		$SaveBricksDescription = "";
	}
}

function clientCmdOpenWrenchDlg(%id)
{
	Wrench_Window.setText("Wrench - " @ %id);
	Canvas.pushDialog(wrenchDlg);
}

function clientCmdSetWrenchData(%data)
{
	%fieldCount = getFieldCount(%data);
	for (%j = 0; %j < %fieldCount; %j++)
	{
		%field = getField(%data, %j);
		%type = getWord(%field, 0);
		if (%type $= "N")
		{
			%name = trim(getSubStr(%field, 2, strlen(%field) - 2));
			if (!WrenchLock_Name.getValue())
			{
				Wrench_Name.setText(%name);
			}
			if (!WrenchSoundLock_Name.getValue())
			{
				WrenchSound_Name.setText(%name);
			}
			if (!WrenchVehicleSpawnLock_Name.getValue())
			{
				WrenchVehicleSpawn_Name.setText(%name);
			}
		}
		else if (%type $= "LDB")
		{
			if (!WrenchLock_Lights.getValue())
			{
				%db = getWord(%field, 1);
				Wrench_Lights.setSelected(%db);
			}
		}
		else if (%type $= "EDB")
		{
			if (!WrenchLock_Emitters.getValue())
			{
				%db = getWord(%field, 1);
				Wrench_Emitters.setSelected(%db);
			}
		}
		else if (%type $= "EDIR")
		{
			if (!WrenchLock_EmitterDir.getValue())
			{
				%idx = getWord(%field, 1);
				%obj = "Wrench_EmitterDir" @ %idx;
				if (isObject(%obj))
				{
					for (%i = 0; %i < 6; %i++)
					{
						%clearObj = "Wrench_EmitterDir" @ %i;
						%clearObj.setValue(0);
					}
					%obj.setValue(1);
					continue;
				}
				error("ERROR: clientCmdSetWrenchData() - Bad emitter direction \"" @ %idx @ "\"");
			}
		}
		else if (%type $= "IDB")
		{
			if (!WrenchLock_Items.getValue())
			{
				%db = getWord(%field, 1);
				Wrench_Items.setSelected(%db);
			}
		}
		else if (%type $= "IPOS")
		{
			if (!WrenchLock_ItemPos.getValue())
			{
				%idx = getWord(%field, 1);
				%obj = "Wrench_ItemPos" @ %idx;
				if (isObject(%obj))
				{
					for (%i = 0; %i < 6; %i++)
					{
						%clearObj = "Wrench_ItemPos" @ %i;
						%clearObj.setValue(0);
					}
					%obj.setValue(1);
					continue;
				}
				error("ERROR: clientCmdSetWrenchData() - Bad item position \"" @ %idx @ "\"");
			}
		}
		else if (%type $= "IDIR")
		{
			if (!WrenchLock_ItemDir.getValue())
			{
				%idx = getWord(%field, 1);
				%obj = "Wrench_ItemDir" @ %idx;
				if (isObject(%obj))
				{
					for (%i = 2; %i < 6; %i++)
					{
						%clearObj = "Wrench_ItemDir" @ %i;
						%clearObj.setValue(0);
					}
					%obj.setValue(1);
					continue;
				}
				error("ERROR: clientCmdSetWrenchData() - Bad item direction \"" @ %idx @ "\"");
			}
		}
		else if (%type $= "IRT")
		{
			if (!WrenchLock_ItemRespawnTime.getValue())
			{
				Wrench_ItemRespawnTime.setText(getWord(%field, 1));
			}
		}
		else if (%type $= "SDB")
		{
			if (!WrenchSoundLock_Sounds.getValue())
			{
				%db = getWord(%field, 1);
				WrenchSound_Sounds.setSelected(%db);
			}
		}
		else if (%type $= "VDB")
		{
			if (!WrenchVehicleSpawnLock_Vehicles.getValue())
			{
				%db = getWord(%field, 1);
				WrenchVehicleSpawn_Vehicles.setSelected(%db);
			}
		}
		else if (%type $= "RCV")
		{
			if (!WrenchVehicleSpawnLock_ReColorVehicle.getValue())
			{
				%val = getWord(%field, 1);
				WrenchVehicleSpawn_ReColorVehicle.setValue(%val);
			}
		}
		else
		{
			error("ERROR: clientCmdSetWrenchData() - unknown field type \"" @ %field @ "\"");
		}
	}
}

function clientCmdWrenchLoadingDone()
{
	Wrench_LoadingWindow.setVisible(0);
}

function clientCmdWrench_LoadMenus()
{
	wrenchDlg.LoadDataBlocks();
}

function wrenchDlg::LoadDataBlocks()
{
	%oldLight = Wrench_Lights.getText();
	%oldEmitter = Wrench_Emitters.getText();
	%oldItem = Wrench_Items.getText();
	%oldSound = WrenchSound_Sounds.getText();
	Wrench_Lights.clear();
	Wrench_Emitters.clear();
	Wrench_Items.clear();
	WrenchSound_Sounds.clear();
	WrenchVehicleSpawn_Vehicles.clear();
	Wrench_Lights.add(" NONE", 0);
	Wrench_Emitters.add(" NONE", 0);
	Wrench_Items.add(" NONE", 0);
	WrenchSound_Sounds.add(" NONE", 0);
	WrenchVehicleSpawn_Vehicles.add(" NONE", 0);
	%dbCount = getDataBlockGroupSize();
	for (%i = 0; %i < %dbCount; %i++)
	{
		%db = getDataBlock(%i);
		%dbClass = %db.getClassName();
		if (%db.uiName !$= "")
		{
			if (%dbClass $= "FxLightData")
			{
				Wrench_Lights.add(%db.uiName, %db);
				continue;
			}
			if (%dbClass $= "ParticleEmitterData")
			{
				Wrench_Emitters.add(%db.uiName, %db);
				continue;
			}
			if (%dbClass $= "ItemData")
			{
				Wrench_Items.add(%db.uiName, %db);
				continue;
			}
			if (%dbClass $= "AudioProfile")
			{
				if (%db.getDescription().isLooping)
				{
					WrenchSound_Sounds.add(%db.uiName, %db);
				}
				continue;
			}
			if (%dbClass $= "PlayerData")
			{
				if (%db.uiName !$= "" && %db.rideAble)
				{
					WrenchVehicleSpawn_Vehicles.add(%db.uiName, %db);
				}
				continue;
			}
			if (%dbClass $= "WheeledVehicleData")
			{
				if (%db.uiName !$= "" && %db.rideAble)
				{
					WrenchVehicleSpawn_Vehicles.add(%db.uiName, %db);
				}
				continue;
			}
			if (%dbClass $= "FlyingVehicleData")
			{
				if (%db.uiName !$= "" && %db.rideAble)
				{
					WrenchVehicleSpawn_Vehicles.add(%db.uiName, %db);
				}
				continue;
			}
			if (%dbClass $= "HoverVehicleData")
			{
				if (%db.uiName !$= "" && %db.rideAble)
				{
					WrenchVehicleSpawn_Vehicles.add(%db.uiName, %db);
				}
			}
		}
	}
	Wrench_Emitters.sort();
	Wrench_Items.sort();
	WrenchSound_Sounds.sort();
	WrenchVehicleSpawn_Vehicles.sort();
	Wrench_Lights.setSelected(Wrench_Lights.findText(%oldLight));
	Wrench_Emitters.setSelected(Wrench_Emitters.findText(%oldEmitter));
	Wrench_Items.setSelected(Wrench_Items.findText(%oldItem));
	WrenchSound_Sounds.setSelected(WrenchSound_Sounds.findText(%oldSound));
}

function wrenchDlg::onWake(%this)
{
	Wrench_LoadingWindow.setVisible(1);
	Wrench_Name.setActive(0);
}

function wrenchDlg::onSleep(%this)
{
}

function wrenchDlg::send(%this)
{
	%data = "";
	%name = trim(Wrench_Name.getValue());
	if (%name !$= "")
	{
		%data = %data TAB "N" SPC %name;
	}
	else
	{
		%data = %data TAB "N" SPC " ";
	}
	%lightDB = Wrench_Lights.getSelected();
	%data = %data TAB "LDB" SPC %lightDB;
	%emitterDB = Wrench_Emitters.getSelected();
	%data = %data TAB "EDB" SPC %emitterDB;
	%emitterDir = 0;
	for (%i = 0; %i < 6; %i++)
	{
		%obj = "Wrench_EmitterDir" @ %i;
		if (%obj.getValue())
		{
			%emitterDir = %i;
			break;
		}
	}
	%data = %data TAB "EDIR" SPC %emitterDir;
	%itemDB = Wrench_Items.getSelected();
	%data = %data TAB "IDB" SPC %itemDB;
	%itemPos = 0;
	for (%i = 0; %i < 6; %i++)
	{
		%obj = "Wrench_ItemPos" @ %i;
		if (%obj.getValue())
		{
			%itemPos = %i;
			break;
		}
	}
	%data = %data TAB "IPOS" SPC %itemPos;
	%itemDir = 0;
	for (%i = 2; %i < 6; %i++)
	{
		%obj = "Wrench_ItemDir" @ %i;
		if (%obj.getValue())
		{
			%itemDir = %i;
			break;
		}
	}
	%data = %data TAB "IDIR" SPC %itemDir;
	%val = mFloor(Wrench_ItemRespawnTime.getValue());
	%data = %data TAB "IRT" SPC %val;
	%data = trim(%data);
	commandToServer('SetWrenchData', %data);
	Canvas.popDialog(wrenchDlg);
}

function clientCmdOpenWrenchSoundDlg(%id)
{
	WrenchSound_Window.setText("Wrench Sound - " @ %id);
	Canvas.pushDialog(wrenchSoundDlg);
}

function wrenchSoundDlg::send(%this)
{
	%data = "";
	%name = trim(WrenchSound_Name.getValue());
	if (%name !$= "")
	{
		%data = %data TAB "N" SPC %name;
	}
	else
	{
		%data = %data TAB "N" SPC " ";
	}
	%soundDB = WrenchSound_Sounds.getSelected();
	%data = %data TAB "SDB" SPC %soundDB;
	%data = trim(%data);
	commandToServer('SetWrenchData', %data);
	Canvas.popDialog(wrenchSoundDlg);
}

function clientCmdOpenWrenchVehicleSpawnDlg(%id)
{
	WrenchSound_Window.setText("Wrench Vehicle Spawn - " @ %id);
	Canvas.pushDialog(wrenchVehicleSpawnDlg);
}

function wrenchVehicleSpawnDlg::Respawn(%this)
{
	commandToServer('VehicleSpawn_Respawn');
	Canvas.popDialog(wrenchVehicleSpawnDlg);
}

function wrenchVehicleSpawnDlg::send(%this)
{
	%data = "";
	%name = trim(WrenchVehicleSpawn_Name.getValue());
	if (%name !$= "")
	{
		%data = %data TAB "N" SPC %name;
	}
	else
	{
		%data = %data TAB "N" SPC " ";
	}
	%vehicleDB = mFloor(WrenchVehicleSpawn_Vehicles.getSelected());
	%data = %data TAB "VDB" SPC %vehicleDB;
	%val = WrenchVehicleSpawn_ReColorVehicle.getValue();
	%data = %data TAB "RCV" SPC %val;
	%data = trim(%data);
	commandToServer('SetWrenchData', %data);
	Canvas.popDialog(wrenchVehicleSpawnDlg);
}

function keyGui::changePrompt()
{
	if (isUnlocked())
	{
		MessageBoxYesNo("Change Authentication Key?", "Are you sure you want to change your authentication key?", "canvas.pushdialog(keyGui);");
	}
	else
	{
		Canvas.pushDialog(keyGui);
	}
}

function keyGui::done()
{
	%id = strupr(keyText0.getValue());
	%keyA = strupr(keyText1.getValue());
	%keyB = strupr(keyText2.getValue());
	%keyC = strupr(keyText3.getValue());
	%error = 0;
	if (strlen(%id) != 5)
	{
		keyError0.setVisible(1);
		keyError0.schedule(250, setVisible, 0);
		keyError0.schedule(500, setVisible, 1);
		keyError0.schedule(750, setVisible, 0);
		%error = 1;
	}
	if (strlen(%keyA) != 4)
	{
		keyError1.setVisible(1);
		keyError1.schedule(250, setVisible, 0);
		keyError1.schedule(500, setVisible, 1);
		keyError1.schedule(750, setVisible, 0);
		%error = 1;
	}
	if (strlen(%keyB) != 4)
	{
		keyError2.setVisible(1);
		keyError2.schedule(250, setVisible, 0);
		keyError2.schedule(500, setVisible, 1);
		keyError2.schedule(750, setVisible, 0);
		%error = 1;
	}
	if (strlen(%keyC) != 4)
	{
		keyError3.setVisible(1);
		keyError3.schedule(250, setVisible, 0);
		keyError3.schedule(500, setVisible, 1);
		keyError3.schedule(750, setVisible, 0);
		%error = 1;
	}
	if (%error)
	{
		return;
	}
	%fullKey = %id @ %keyA @ %keyB @ %keyC;
	%fullKey = strupr(%fullKey);
	setKeyDat(%fullKey);
	Unlock();
	MM_UpdateDemoDisplay();
	if (isUnlocked())
	{
		Canvas.popDialog(keyGui);
		$IgnoreShortSuccess = 1;
		auth_Init();
		MessageBoxOK("SUCCESS", "Full Version Unlocked!");
	}
	else
	{
		Canvas.popDialog(keyGui);
		MessageBoxOK("FAIL", "Invalid key.", "canvas.pushDialog(keyGui);");
	}
	MM_UpdateDemoDisplay();
}

function keyGui::onWake(%this)
{
	keyError0.setVisible(0);
	keyError1.setVisible(0);
	keyError2.setVisible(0);
	keyError3.setVisible(0);
	keyText0.setValue("");
	keyText1.setValue("");
	keyText2.setValue("");
	keyText3.setValue("");
}

function keyGui::onSleep(%this)
{
	keyError0.setVisible(0);
	keyError1.setVisible(0);
	keyError2.setVisible(0);
	keyError3.setVisible(0);
	keyText0.setValue("");
	keyText1.setValue("");
	keyText2.setValue("");
	keyText3.setValue("");
}

function keyGui::type(%index)
{
	%obj = "keyText" @ %index;
	if (strlen(%obj.getValue()) >= %obj.maxLength)
	{
		%newObj = "keyText" @ %index + 1;
		if (isObject(%newObj))
		{
			Canvas.tabNext();
		}
	}
}

function keyText0::onPaste(%this, %text)
{
	keyGui.pasteAuthKey(%text);
}

function keyText1::onPaste(%this, %text)
{
	keyGui.pasteAuthKey(%text);
}

function keyText2::onPaste(%this, %text)
{
	keyGui.pasteAuthKey(%text);
}

function keyText3::onPaste(%this, %text)
{
	keyGui.pasteAuthKey(%text);
}

function keyGui::pasteAuthKey(%this, %text)
{
	%text = strreplace(%text, " ", "");
	%text = strreplace(%text, "\t", "");
	%text = strreplace(%text, "-", "");
	%pt0 = getSubStr(%text, 0, 5);
	%pt1 = getSubStr(%text, 5, 4);
	%pt2 = getSubStr(%text, 9, 4);
	%pt3 = getSubStr(%text, 13, 4);
	keyText0.setText(%pt0);
	keyText1.setText(%pt1);
	keyText2.setText(%pt2);
	keyText3.setText(%pt3);
}

function auth_Init()
{
	%keyID = getKeyID();
	if (%keyID $= "")
	{
		return;
	}
	MM_AuthText.setText("Authentication: Connecting...");
	if (isObject(authTCPobj))
	{
		authTCPobj.delete();
	}
	new TCPObject(authTCPobj);
	authTCPobj.passPhraseCount = 0;
	authTCPobj.site = "master.blockland.us";
	authTCPobj.port = 80;
	authTCPobj.filePath = "/auth/authInit.asp";
	authTCPobj.done = "false";
	%postText = "ID=" @ %keyID;
	%postTexLen = strlen(%postText);
	authTCPobj.cmd = "POST " @ authTCPobj.filePath @ " HTTP/1.1\r\nHost: " @ authTCPobj.site @ "\r\nContent-Type: application/x-www-form-urlencoded\r\nContent-Length: " @ %postTexLen @ "\r\n\r\n" @ %postText @ "\r\n";
	authTCPobj.connect(authTCPobj.site @ ":" @ authTCPobj.port);
}

function authTCPobj::onDNSFailed(%this)
{
	if (isUnlocked())
	{
		MM_AuthText.setText("Offline Mode");
		MM_AuthBar.blinkSuccess();
	}
	else
	{
		MM_AuthText.setText("Demo Mode");
		MM_AuthBar.blinkFail();
	}
}

function authTCPobj::onConnectFailed(%this)
{
	if (isUnlocked())
	{
		MM_AuthText.setText("Offline Mode");
		MM_AuthBar.blinkSuccess();
	}
	else
	{
		MM_AuthText.setText("Demo Mode");
		MM_AuthBar.blinkFail();
	}
}

function authTCPobj::onConnected(%this)
{
	%this.send(%this.cmd);
	MM_AuthText.setText("Authentication: Validating key...");
}

function authTCPobj::onDisconnect(%this)
{
}

function authTCPobj::onLine(%this, %line)
{
	if (%this.done)
	{
		return;
	}
	if (getWord(%line, 0) $= "FAIL")
	{
		echo("Authentication: FAIL");
		%reason = getSubStr(%line, 5, strlen(%line) - 5);
		MM_AuthText.setText("Authentication FAILED: " @ %reason);
		setKeyDat("XXXXXAAAABBBBCCCC");
		Unlock();
		MM_UpdateDemoDisplay();
		if (MBOKFrame.isAwake() && MBOKFrame.getValue() $= "SUCCESS")
		{
			MessageBoxOK("FAIL", "Invalid key.", "canvas.pushDialog(keyGui);");
		}
		MM_AuthBar.blinkFail();
		return;
	}
	if (getWord(%line, 0) $= "NAMEFAIL")
	{
		%reason = getSubStr(%line, 9, strlen(%line) - 9);
		regName_registerWindow.setVisible(0);
		MessageBoxOK("Name Change Failed", %reason);
		return;
	}
	if (getWord(%line, 0) $= "NAMESUCCESS")
	{
		$pref::Player::NetName = getSubStr(%line, 12, strlen(%line) - 12);
		Canvas.popDialog(regNameGui);
		MessageBoxOK("Name Changed", "Your name has been changed to \"" @ $pref::Player::NetName @ "\"");
	}
	if (getWord(%line, 0) $= "SUCCESS")
	{
		echo("Authentication: SUCCESS");
		$pref::Player::NetName = getSubStr(%line, 8, strlen(%line) - 8);
		MM_AuthText.setText("Welcome, " @ $pref::Player::NetName);
		$authed = 1;
		MM_AuthBar.blinkSuccess();
		return;
	}
	if (getWord(%line, 0) $= "SHORTSUCCESS")
	{
		echo("Authentication: SHORTSUCCESS");
		if ($IgnoreShortSuccess == 1)
		{
			return;
		}
		if (!$Server::Dedicated)
		{
			if ($NewNetName $= "")
			{
				$pref::Player::NetName = getSubStr(%line, 13, strlen(%line) - 13);
				MM_AuthText.setText("Welcome, " @ $pref::Player::NetName);
				$authed = 1;
				MM_AuthBar.blinkSuccess();
				%this.done = 1;
				%this.disconnect();
				return;
			}
		}
	}
	if (getWord(%line, 0) $= "Set-Cookie:")
	{
		%this.cookie = getSubStr(%line, 12, strlen(%line) - 12);
	}
	if (getWord(%line, 0) $= "PASSPHRASE")
	{
		%passphrase = getWord(%line, 1);
		if (getKeyID() !$= "")
		{
			%crc = getPassPhraseResponse(%passphrase, %this.passPhraseCount);
			if (%crc !$= "")
			{
				%this.filePath = "/auth/authConfirm.asp";
				if ($NewNetName !$= "")
				{
					%postText = "CRC=" @ %crc @ "&NAME=" @ urlEnc($NewNetName);
				}
				else
				{
					%postText = "CRC=" @ %crc;
				}
				%postText = %postText @ "&DEDICATED=" @ $Server::Dedicated;
				%postText = %postText @ "&PORT=" @ $Pref::Server::Port;
				%postTexLen = strlen(%postText);
				%this.cmd = "POST " @ authTCPobj.filePath @ " HTTP/1.1\r\nCookie: " @ authTCPobj.cookie @ "\r\nHost: " @ authTCPobj.site @ "\r\nContent-Type: application/x-www-form-urlencoded\r\nContent-Length: " @ %postTexLen @ "\r\n\r\n" @ %postText @ "\r\n";
				%this.disconnect();
				%this.connect(authTCPobj.site @ ":" @ authTCPobj.port);
			}
			%this.passPhraseCount++;
		}
		else
		{
			echo("Authentication: FAIL No key");
			MM_AuthText.setText("Authentication FAILED: No key found.");
			return;
		}
	}
}

function MM_AuthBar::blinkFail(%obj)
{
	MM_AuthNameButton.setVisible(0);
	MM_AuthRetryButton.setVisible(1);
	MM_AuthBar.setBitmap("base/client/ui/authBarFail");
	MM_AuthBar.schedule(250, setBitmap, "base/client/ui/authBar");
	MM_AuthBar.schedule(500, setBitmap, "base/client/ui/authBarFail");
	MM_AuthBar.schedule(750, setBitmap, "base/client/ui/authBar");
}

function MM_AuthBar::blinkSuccess(%obj)
{
	MM_AuthNameButton.setVisible(1);
	MM_AuthRetryButton.setVisible(0);
	MM_AuthBar.setBitmap("base/client/ui/authBarWin");
	MM_AuthBar.schedule(250, setBitmap, "base/client/ui/authBar");
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

function regNameGui::onWake()
{
	regName_CurrName.setText($pref::Player::NetName);
	regName_registerWindow.setVisible(0);
}

function regNameGui::onSleep()
{
}

function regNameGui::register()
{
	$NewNetName = regName_NewName.getValue();
	if ($NewNetName !$= "")
	{
		auth_Init();
	}
}

$COLORMODE_RGB = 0;
$COLORMODE_HSV = 1;
function colorGui::onWake(%this)
{
	%this.update();
	if (colorGui_option0.getValue() == 0 && colorGui_option1.getValue() == 0)
	{
		colorGui_option0.setValue(1);
	}
}

function colorGui::onSleep(%this)
{
}

function colorGui::activate(%this, %val, %callback)
{
}

function colorGui::setMode(%this, %mode)
{
	if (%mode == $COLORMODE_RGB)
	{
		colorGui_Label0.setText("R");
		colorGui_Label1.setText("G");
		colorGui_Label2.setText("B");
		if (colorGui_option0.getValue() == 0)
		{
			%h = ColorGui_Slider0.getValue();
			%s = ColorGui_Slider1.getValue();
			%v = ColorGui_Slider2.getValue();
			%a = Colorgui_Slider3.getValue();
			%RGB = HSVtoRGB(%h, %s, %v);
			%r = getWord(%RGB, 0);
			%g = getWord(%RGB, 1);
			%b = getWord(%RGB, 2);
			ColorGui_Slider0.setValue(%r);
			ColorGui_Slider1.setValue(%g);
			ColorGui_Slider2.setValue(%b);
			ColorGui_Result.setColor(%r SPC %g SPC %b SPC %a);
		}
	}
	else if (%mode == $COLORMODE_HSV)
	{
		colorGui_Label0.setText("H");
		colorGui_Label1.setText("S");
		colorGui_Label2.setText("V");
		if (colorGui_option1.getValue() == 0)
		{
			%r = ColorGui_Slider0.getValue();
			%g = ColorGui_Slider1.getValue();
			%b = ColorGui_Slider2.getValue();
			%a = Colorgui_Slider3.getValue();
			%hsv = RGBtoHSV(%r, %g, %b);
			ColorGui_Slider0.setValue(getWord(%hsv, 0));
			ColorGui_Slider1.setValue(getWord(%hsv, 1));
			ColorGui_Slider2.setValue(getWord(%hsv, 2));
			ColorGui_Result.setColor(%r SPC %g SPC %b SPC %a);
		}
	}
}

function colorGui::update()
{
	if (colorGui_option0.getValue())
	{
		%mode = $COLORMODE_RGB;
	}
	else if (colorGui_option1.getValue())
	{
		%mode = $COLORMODE_HSV;
	}
	else
	{
		%mode = $COLORMODE_RGB;
	}
	if (%mode == $COLORMODE_RGB)
	{
		%r = ColorGui_Slider0.getValue();
		%g = ColorGui_Slider1.getValue();
		%b = ColorGui_Slider2.getValue();
	}
	else if (%mode == $COLORMODE_HSV)
	{
		%h = ColorGui_Slider0.getValue();
		%s = ColorGui_Slider1.getValue();
		%v = ColorGui_Slider2.getValue();
		%RGB = HSVtoRGB(%h, %s, %v);
		%r = getWord(%RGB, 0);
		%g = getWord(%RGB, 1);
		%b = getWord(%RGB, 2);
	}
	%a = Colorgui_Slider3.getValue();
	ColorGui_Result.setColor(%r SPC %g SPC %b SPC %a);
}

function RGBtoHSV(%r, %g, %b)
{
	%min = getMin(%r, %g, %b);
	%max = getMax(%r, %g, %b);
	%delta = %max - %min;
	%v = %max;
	if (%max != 0)
	{
		%s = %delta / %max;
		if (%r == %max)
		{
			%h = (%g - %b) / %delta;
		}
		else if (%g == %max)
		{
			%h = 2 + (%b - %r) / %delta;
		}
		else
		{
			%h = 4 + (%r - %g) / %delta;
		}
		%h *= 60;
		if (%h < 0)
		{
			%h += 360;
		}
	}
	else
	{
		%s = 0;
		%h = 0;
		%v = 0;
	}
	%h = %h / 360;
	return %h SPC %s SPC %v;
}

function HSVtoRGB(%h, %s, %v)
{
	if (%s == 0)
	{
		%r = %g = %b = %v;
		return %r SPC %g SPC %b;
	}
	%h = %h * 360;
	if (%h >= 360)
	{
		%h -= 360;
	}
	%h /= 60;
	%i = mFloor(%h);
	%f = %h - %i;
	%p = %v * (1 - %s);
	%q = %v * (1 - %s * %f);
	%t = %v * (1 - %s * (1 - %f));
	if (%i == 0)
	{
		%r = %v;
		%g = %t;
		%b = %p;
	}
	else if (%i == 1)
	{
		%r = %q;
		%g = %v;
		%b = %p;
	}
	else if (%i == 2)
	{
		%r = %p;
		%g = %v;
		%b = %t;
	}
	else if (%i == 3)
	{
		%r = %p;
		%g = %q;
		%b = %v;
	}
	else if (%i == 4)
	{
		%r = %t;
		%g = %p;
		%b = %v;
	}
	else
	{
		%r = %v;
		%g = %p;
		%b = %q;
	}
	return %r SPC %g SPC %b;
}

function getMax(%a, %b, %c)
{
	if (%a >= %b && %a >= %c)
	{
		return %a;
	}
	if (%b >= %a && %b >= %c)
	{
		return %b;
	}
	if (%c >= %a && %c >= %b)
	{
		return %c;
	}
	return %a;
}

function getMin(%a, %b, %c)
{
	if (%a <= %b && %a <= %c)
	{
		return %a;
	}
	if (%b <= %a && %b <= %c)
	{
		return %b;
	}
	if (%c <= %a && %c <= %b)
	{
		return %c;
	}
	return %a;
}

function ColorSetGui::onWake(%this)
{
	if (!%this.initialized)
	{
		%this.initialized = 1;
		%this.Load();
	}
	%this.Display();
	if (colorSetGui_option0.getValue() == 0 && colorSetGui_option1.getValue() == 0)
	{
		colorSetGui_option0.setValue(1);
	}
	ColorSetGui.selectColor(0);
}

function ColorSetGui::onSleep(%this)
{
}

function ColorSetGui::save(%this)
{
	export("$Avatar::*", "base/config/client/avatarColors.cs");
}

function ColorSetGui::Load(%this)
{
	%file = new FileObject();
	if (isFile("Base/config/client/avatarColors.cs"))
	{
		exec("Base/config/client/avatarColors.cs");
	}
	else
	{
		%this.defaults();
		%this.save();
	}
}

function ColorSetGui::defaults(%this)
{
	deleteVariables("$Avatar::*");
	%i = -1;
	$Avatar::Color[%i++] = "0.900 0.000 0.000 1.000";
	$Avatar::Color[%i++] = "0.900 0.900 0.000 1.000";
	$Avatar::Color[%i++] = "0.000 0.500 0.250 1.000";
	$Avatar::Color[%i++] = "0.200 0.000 0.800 1.000";
	$Avatar::Color[%i++] = "0.900 0.900 0.900 1.000";
	$Avatar::Color[%i++] = "0.750 0.750 0.750 1.000";
	$Avatar::Color[%i++] = "0.500 0.500 0.500 1.000";
	$Avatar::Color[%i++] = "0.200 0.200 0.200 1.000";
	$Avatar::Color[%i++] = IColorToFColor("100 50 0 255");
	$Avatar::Color[%i++] = IColorToFColor("230 87 20 255");
	$Avatar::Color[%i++] = IColorToFColor("191 46 123 255");
	$Avatar::Color[%i++] = IColorToFColor("99 0 30 255");
	$Avatar::Color[%i++] = IColorToFColor("34 69 69 255");
	$Avatar::Color[%i++] = IColorToFColor("0 36 85 255");
	$Avatar::Color[%i++] = IColorToFColor("27 117 196 255");
	$Avatar::Color[%i++] = IColorToFColor("255 255 255 255");
	$Avatar::Color[%i++] = IColorToFColor("20 20 20 255");
	$Avatar::Color[%i++] = IColorToFColor("255 255 255 64");
	$Avatar::Color[%i++] = IColorToFColor("236 131 173 255");
	$Avatar::Color[%i++] = IColorToFColor("255 154 108 255");
	$Avatar::Color[%i++] = IColorToFColor("255 224 156 255");
	$Avatar::Color[%i++] = IColorToFColor("244 224 200 255");
	$Avatar::Color[%i++] = IColorToFColor("200 235 125 255");
	$Avatar::Color[%i++] = IColorToFColor("138 178 141 255");
	$Avatar::Color[%i++] = IColorToFColor("143 237 245 255");
	$Avatar::Color[%i++] = IColorToFColor("178 169 231 255");
	$Avatar::Color[%i++] = IColorToFColor("224 143 244 255");
	$Avatar::Color[%i++] = "0.667 0.000 0.000 0.700";
	$Avatar::Color[%i++] = "1.000 0.500 0.000 0.700";
	$Avatar::Color[%i++] = "0.990 0.960 0.000 0.700";
	$Avatar::Color[%i++] = "0.000 0.471 0.196 0.700";
	$Avatar::Color[%i++] = "0.000 0.200 0.640 0.700";
	$Avatar::Color[%i++] = IColorToFColor("152 41 100 178");
	$Avatar::Color[%i++] = "0.550 0.700 1.000 0.700";
	$Avatar::Color[%i++] = "0.850 0.850 0.850 0.700";
	$Avatar::Color[%i++] = "0.100 0.100 0.100 0.700";
	$Avatar::NumColors = %i++;
	%this.Display();
}

function ColorSetGui::Display(%this)
{
	if (isObject(ColorSet_Box))
	{
		ColorSet_Box.clear();
		%newBox = ColorSet_Box;
	}
	else
	{
		ColorSet_Scroll.clear();
		%newBox = new GuiSwatchCtrl(ColorSet_Box);
		ColorSet_Scroll.add(%newBox);
		%newBox.setColor("0 0 0 0");
		%newBox.resize(0, 0, 32, 32);
	}
	%itemCount = 0;
	%rowLimit = 6;
	for (%i = 0; %i < $Avatar::NumColors; %i++)
	{
		%color = $Avatar::Color[%i];
		%newSwatch = new GuiSwatchCtrl("ColorSetSwatch" @ %i);
		%newBox.add(%newSwatch);
		%newSwatch.setColor(%color);
		%x = (%itemCount % %rowLimit) * 32;
		%y = mFloor(%itemCount / %rowLimit) * 32;
		%newSwatch.resize(%x, %y, 32, 32);
		%newButton = new GuiBitmapButtonCtrl();
		%newBox.add(%newButton);
		%newButton.setBitmap("base/client/ui/btnColor");
		%newButton.setText(" ");
		%newButton.resize(%x, %y, 32, 32);
		%newButton.command = "colorSetGui.selectColor(" @ %i @ ");";
		%itemCount++;
	}
	if (%itemCount >= %rowLimit)
	{
		%w = %rowLimit * 32;
	}
	else
	{
		%w = %itemCount * 32;
	}
	%h = (mFloor(%itemCount / %rowLimit) + 1) * 32;
	%newBox.resize(0, 0, %w, %h);
}

function ColorSetGui::AddColor(%this, %color)
{
	if ($Avatar::Color[ColorSetGui.currColor] $= "")
	{
		$Avatar::Color[$Avatar::NumColors] = "0.5 0.5 0.5 1";
	}
	else
	{
		$Avatar::Color[$Avatar::NumColors] = $Avatar::Color[ColorSetGui.currColor];
	}
	ColorSetGui.currColor = $Avatar::NumColors;
	$Avatar::NumColors++;
	ColorSetGui.Display();
}

function ColorSetGui::deleteColor(%this)
{
	%idx = %this.currColor;
	if (%idx < 0 || %idx >= $Avatar::NumColors)
	{
		return;
	}
	if ($Avatar::Color[%idx] $= "")
	{
		return;
	}
	if (%idx + 1 < $Avatar::NumColors)
	{
		for (%i = %idx + 1; %i < $Avatar::NumColors; %i++)
		{
			$Avatar::Color[%i - 1] = $Avatar::Color[%i];
		}
	}
	$Avatar::Color[$Avatar::NumColors] = "";
	$Avatar::NumColors--;
	%this.Display();
}

function ColorSetGui::selectColor(%this, %idx)
{
	%color = $Avatar::Color[%idx];
	colorSet_Result.setColor(%color);
	%this.currColor = %idx;
	%r = getWord(%color, 0);
	%g = getWord(%color, 1);
	%b = getWord(%color, 2);
	%a = getWord(%color, 3);
	if (colorSetGui_option0.getValue() == 1)
	{
		ColorSetGui_Slider0.setValue(%r);
		ColorSetGui_Slider1.setValue(%g);
		ColorSetGui_Slider2.setValue(%b);
		ColorSetGui_Slider3.setValue(%a);
	}
	else
	{
		%hsv = RGBtoHSV(%r, %g, %b);
		%h = getWord(%hsv, 0);
		%s = getWord(%hsv, 1);
		%v = getWord(%hsv, 2);
		ColorSetGui_Slider0.setValue(%h);
		ColorSetGui_Slider1.setValue(%s);
		ColorSetGui_Slider2.setValue(%v);
		ColorSetGui_Slider3.setValue(%a);
	}
}

function ColorSetGui::setMode(%this, %mode)
{
	if (%mode == $COLORMODE_RGB)
	{
		colorSetGui_Label0.setText("R");
		colorSetGui_Label1.setText("G");
		colorSetGui_Label2.setText("B");
		if (colorSetGui_option0.getValue() == 0)
		{
			%h = ColorSetGui_Slider0.getValue();
			%s = ColorSetGui_Slider1.getValue();
			%v = ColorSetGui_Slider2.getValue();
			%a = ColorSetGui_Slider3.getValue();
			%RGB = HSVtoRGB(%h, %s, %v);
			%r = getWord(%RGB, 0);
			%g = getWord(%RGB, 1);
			%b = getWord(%RGB, 2);
			ColorSetGui_Slider0.setValue(%r);
			ColorSetGui_Slider1.setValue(%g);
			ColorSetGui_Slider2.setValue(%b);
			$Avatar::Color[ColorSetGui.currColor] = %r SPC %g SPC %b SPC %a;
			colorSet_Result.setColor(%r SPC %g SPC %b SPC %a);
			%obj = ColorSetSwatch @ ColorSetGui.currColor;
			%obj.setColor(%r SPC %g SPC %b SPC %a);
		}
	}
	else if (%mode == $COLORMODE_HSV)
	{
		colorSetGui_Label0.setText("H");
		colorSetGui_Label1.setText("S");
		colorSetGui_Label2.setText("V");
		if (colorSetGui_option1.getValue() == 0)
		{
			%r = ColorSetGui_Slider0.getValue();
			%g = ColorSetGui_Slider1.getValue();
			%b = ColorSetGui_Slider2.getValue();
			%a = ColorSetGui_Slider3.getValue();
			%hsv = RGBtoHSV(%r, %g, %b);
			ColorSetGui_Slider0.setValue(getWord(%hsv, 0));
			ColorSetGui_Slider1.setValue(getWord(%hsv, 1));
			ColorSetGui_Slider2.setValue(getWord(%hsv, 2));
			$Avatar::Color[ColorSetGui.currColor] = %r SPC %g SPC %b SPC %a;
			colorSet_Result.setColor(%r SPC %g SPC %b SPC %a);
			%obj = ColorSetSwatch @ ColorSetGui.currColor;
			%obj.setColor(%r SPC %g SPC %b SPC %a);
		}
	}
}

function ColorSetGui::update()
{
	if (colorSetGui_option0.getValue())
	{
		%mode = $COLORMODE_RGB;
	}
	else if (colorSetGui_option1.getValue())
	{
		%mode = $COLORMODE_HSV;
	}
	else
	{
		%mode = $COLORMODE_RGB;
	}
	if (%mode == $COLORMODE_RGB)
	{
		%r = ColorSetGui_Slider0.getValue();
		%g = ColorSetGui_Slider1.getValue();
		%b = ColorSetGui_Slider2.getValue();
	}
	else if (%mode == $COLORMODE_HSV)
	{
		%h = ColorSetGui_Slider0.getValue();
		%s = ColorSetGui_Slider1.getValue();
		%v = ColorSetGui_Slider2.getValue();
		%RGB = HSVtoRGB(%h, %s, %v);
		%r = getWord(%RGB, 0);
		%g = getWord(%RGB, 1);
		%b = getWord(%RGB, 2);
	}
	%a = ColorSetGui_Slider3.getValue();
	$Avatar::Color[ColorSetGui.currColor] = %r SPC %g SPC %b SPC %a;
	colorSet_Result.setColor($Avatar::Color[ColorSetGui.currColor]);
	%obj = ColorSetSwatch @ ColorSetGui.currColor;
	%obj.setColor($Avatar::Color[ColorSetGui.currColor]);
}

function IColorToFColor(%IColor)
{
	%r = getWord(%IColor, 0);
	%g = getWord(%IColor, 1);
	%b = getWord(%IColor, 2);
	%a = getWord(%IColor, 3);
	%r /= 255;
	%g /= 255;
	%b /= 255;
	%a /= 255;
	return %r SPC %g SPC %b SPC %a;
}

function RGBtoHSV(%r, %g, %b)
{
	%min = getMin(%r, %g, %b);
	%max = getMax(%r, %g, %b);
	%delta = %max - %min;
	%v = %max;
	if (%max != 0)
	{
		%s = %delta / %max;
		if (%r == %max)
		{
			%h = (%g - %b) / %delta;
		}
		else if (%g == %max)
		{
			%h = 2 + (%b - %r) / %delta;
		}
		else
		{
			%h = 4 + (%r - %g) / %delta;
		}
		%h *= 60;
		if (%h < 0)
		{
			%h += 360;
		}
	}
	else
	{
		%s = 0;
		%h = 0;
		%v = 0;
	}
	%h = %h / 360;
	return %h SPC %s SPC %v;
}

function HSVtoRGB(%h, %s, %v)
{
	if (%s == 0)
	{
		%r = %g = %b = %v;
		return %r SPC %g SPC %b;
	}
	%h = %h * 360;
	if (%h >= 360)
	{
		%h -= 360;
	}
	%h /= 60;
	%i = mFloor(%h);
	%f = %h - %i;
	%p = %v * (1 - %s);
	%q = %v * (1 - %s * %f);
	%t = %v * (1 - %s * (1 - %f));
	if (%i == 0)
	{
		%r = %v;
		%g = %t;
		%b = %p;
	}
	else if (%i == 1)
	{
		%r = %q;
		%g = %v;
		%b = %p;
	}
	else if (%i == 2)
	{
		%r = %p;
		%g = %v;
		%b = %t;
	}
	else if (%i == 3)
	{
		%r = %p;
		%g = %q;
		%b = %v;
	}
	else if (%i == 4)
	{
		%r = %t;
		%g = %p;
		%b = %v;
	}
	else
	{
		%r = %v;
		%g = %p;
		%b = %q;
	}
	return %r SPC %g SPC %b;
}

function getMax(%a, %b, %c)
{
	if (%a >= %b && %a >= %c)
	{
		return %a;
	}
	if (%b >= %a && %b >= %c)
	{
		return %b;
	}
	if (%c >= %a && %c >= %b)
	{
		return %c;
	}
	return %a;
}

function getMin(%a, %b, %c)
{
	if (%a <= %b && %a <= %c)
	{
		return %a;
	}
	if (%b <= %a && %b <= %c)
	{
		return %b;
	}
	if (%c <= %a && %c <= %b)
	{
		return %c;
	}
	return %a;
}

function MusicFilesGui::onWake(%this)
{
	clientUpdateMusicList();
	MFG_Scroll.clear();
	%newBox = new GuiSwatchCtrl();
	MFG_Scroll.add(%newBox);
	%newBox.setColor("0 0 0 0");
	%count = 0;
	%dir = "base/data/sound/music/*.ogg";
	%fileCount = getFileCount(%dir);
	%fileName = findFirstFile(%dir);
	for (%i = 0; %i < %fileCount; %i++)
	{
		%name = fileBase(%fileName);
		%varName = strreplace(%name, " ", "_");
		%newCB = new GuiCheckBoxCtrl();
		%newBox.add(%newCB);
		%newCB.setText(%name);
		%x = 5;
		%y = %count * 18;
		%w = getWord(MFG_Scroll.getExtent(), 0) - %x;
		%h = 18;
		%newCB.resize(%x, %y, %w, %h);
		%newCB.varName = %varName;
		if ($Music__[%varName] == 1)
		{
			%newCB.setValue(1);
		}
		else
		{
			%newCB.setValue(0);
		}
		%fileName = findNextFile(%dir);
		%count++;
	}
	%x = 0;
	%y = 0;
	%w = getWord(MFG_Scroll.getExtent(), 0);
	%h = %count * 18;
	%newBox.resize(%x, %y, %w, %h);
}

function MusicFilesGui::onSleep(%this)
{
	deleteVariables("$Music*");
	%box = MFG_Scroll.getObject(0);
	%count = %box.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		%cb = %box.getObject(%i);
		if (%cb.getValue())
		{
			$Music__[%cb.varName] = 1;
		}
		else
		{
			$Music__[%cb.varName] = -1;
		}
	}
	export("$Music__*", "Base/data/sound/music/musicList.cs");
}

function clientUpdateMusicList()
{
	deleteVariables("$Music*");
	exec("Base/data/sound/music/musicList.cs");
	%dir = "base/data/sound/music/*.ogg";
	%fileCount = getFileCount(%dir);
	%fileName = findFirstFile(%dir);
	for (%i = 0; %i < %fileCount; %i++)
	{
		%name = fileBase(%fileName);
		%name = strreplace(%name, " ", "_");
		if (mFloor($Music__[%name]) <= 0)
		{
			$Music__[%name] = -1;
		}
		else
		{
			$Music__[%name] = 1;
		}
		%fileName = findNextFile(%dir);
	}
	export("$Music__*", "Base/data/sound/music/musicList.cs");
}

function AddOnsGui::onWake(%this)
{
	clientUpdateAddOnsList();
	AOG_Scroll.clear();
	%newBox = new GuiSwatchCtrl();
	AOG_Scroll.add(%newBox);
	%newBox.setColor("0 0 0 0");
	%count = 0;
	%dir = "Add-Ons/*.cs";
	%fileCount = getFileCount(%dir);
	%fileName = findFirstFile(%dir);
	for (%i = 0; %i < %fileCount; %i++)
	{
		echo(%fileName);
		%name = fileBase(%fileName);
		%varName = %name;
		%varName = strreplace(%varName, " ", "_");
		%varName = strreplace(%varName, "-", "_");
		if (%name !$= "ADD_ON_LIST")
		{
			echo(%name);
			%newCB = new GuiCheckBoxCtrl();
			%newBox.add(%newCB);
			%newCB.setText(%name);
			%x = 5;
			%y = %count * 18;
			%w = getWord(AOG_Scroll.getExtent(), 0) - %x;
			%h = 18;
			%newCB.resize(%x, %y, %w, %h);
			%newCB.varName = %varName;
			if ($AddOn__[%varName] == 1)
			{
				%newCB.setValue(1);
			}
			else
			{
				%newCB.setValue(0);
			}
			%count++;
		}
		%fileName = findNextFile(%dir);
	}
	%x = 0;
	%y = 0;
	%w = getWord(AOG_Scroll.getExtent(), 0);
	%h = %count * 18;
	%newBox.resize(%x, %y, %w, %h);
}

function AddOnsGui::onSleep(%this)
{
	deleteVariables("$AddOn__*");
	%box = AOG_Scroll.getObject(0);
	%count = %box.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		%cb = %box.getObject(%i);
		if (%cb.getValue())
		{
			$AddOn__[%cb.varName] = 1;
		}
		else
		{
			$AddOn__[%cb.varName] = -1;
		}
	}
	export("$AddOn__*", "Add-Ons/ADD_ON_LIST.cs");
}

function clientUpdateAddOnsList()
{
	deleteVariables("$AddOn__*");
	exec("Add-Ons/ADD_ON_LIST.cs");
	%dir = "Add-Ons/*.cs";
	%fileCount = getFileCount(%dir);
	%fileName = findFirstFile(%dir);
	for (%i = 0; %i < %fileCount; %i++)
	{
		%name = fileBase(%fileName);
		if (%name !$= "ADD_ON_LIST")
		{
			%name = strreplace(%name, " ", "_");
			%name = strreplace(%name, "-", "_");
			if (mFloor($AddOn__[%name]) <= 0)
			{
				$AddOn__[%name] = -1;
				continue;
			}
			$AddOn__[%name] = 1;
		}
		%fileName = findNextFile(%dir);
	}
	export("$AddOn__*", "Add-Ons/ADD_ON_LIST.cs");
}

function ToggleBuildMacroRecording(%val)
{
	if (%val == 0)
	{
		return;
	}
	if ($RecordingBuildMacro)
	{
		$RecordingBuildMacro = 0;
		$BuildMacroSO.compress();
		clientCmdCenterPrint("<color:FF0000>Build Macro Recording STOPPED", 2);
	}
	else
	{
		if (isObject($BuildMacroSO))
		{
			$BuildMacroSO.delete();
		}
		$BuildMacroSO = new ScriptObject()
		{
			class = BuildMacroSO;
			numEvents = 0;
		};
		$RecordingBuildMacro = 1;
		clientCmdCenterPrint("<color:00FF00>Build Macro Recording STARTED", 2);
		$BuildMacroSO.pushEvent("Server", 'useSprayCan', $currSprayCanIndex);
	}
}

function PlayBackBuildMacro(%val)
{
	if (%val == 0)
	{
		return;
	}
	if (isObject($BuildMacroSO))
	{
		$BuildMacroSO.PlayBack();
	}
}

function BuildMacroSO::pushEvent(%this, %eventType, %event, %arg1, %arg2, %arg3, %arg4, %arg5, %arg6)
{
	if (%this.numEvents > 10000)
	{
		error("ERROR: BuildMacroSO::PushEvent() - > 10000 events, aborting.");
		$RecordingBuildMacro = 0;
		return;
	}
	if (%eventType $= "Client")
	{
		%val = %eventType TAB %event TAB %arg1 TAB %arg2 TAB %arg3 TAB %arg4 TAB %arg5 TAB %arg6;
	}
	else if (%eventType $= "Server")
	{
		%val = %eventType TAB getTaggedString(%event) TAB %arg1 TAB %arg2 TAB %arg3 TAB %arg4 TAB %arg5 TAB %arg6;
	}
	else
	{
		error("ERROR: BuildMacroSO::PushEvent() - unknown event type \"" @ %eventType @ "\"");
	}
	%this.event[%this.numEvents] = %val;
	%this.numEvents++;
}

function BuildMacroSO::compress(%this)
{
	for (%i = 0; %i < %this.numEvents; %i++)
	{
		%string = %this.event[%i];
		%event = getField(%string, 1);
		if (%event $= "PlantBrick")
		{
			%j = %i + 1;
			while (%j < %this.numEvents)
			{
				%stringB = %this.event[%j];
				%eventB = getField(%stringB, 1);
				if (%eventB $= "PlantBrick")
				{
					%this.deleteEvent(%j);
				}
				else
				{
					break;
				}
			}
		}
		else if (%event $= "ShiftBrick")
		{
			%x = getField(%string, 2);
			%y = getField(%string, 3);
			%z = getField(%string, 4);
			%j = %i + 1;
			while (%j < %this.numEvents)
			{
				%stringB = %this.event[%j];
				%eventB = getField(%stringB, 1);
				if (%eventB $= "ShiftBrick")
				{
					%x += getField(%stringB, 2);
					%y += getField(%stringB, 3);
					%z += getField(%stringB, 4);
					%this.deleteEvent(%j);
				}
				else if (%eventB $= "UseInventory")
				{
					%j++;
				}
				else if (%eventB $= "UseSprayCan")
				{
					%j++;
				}
				else
				{
					break;
				}
			}
			%this.event[%i] = "Server" TAB "ShiftBrick" TAB %x TAB %y TAB %z;
		}
		else if (%event $= "UseSprayCan")
		{
			%canIndex = getField(%string, 2);
			%j = %i + 1;
			while (%j < %this.numEvents)
			{
				%stringB = %this.event[%j];
				%eventB = getField(%stringB, 1);
				if (%eventB $= "ShiftBrick")
				{
					%j++;
				}
				else if (%eventB $= "SuperShiftBrick")
				{
					%j++;
				}
				else if (%eventB $= "RotateBrick")
				{
					%j++;
				}
				else if (%eventB $= "UseInventory")
				{
					%j++;
				}
				else if (%eventB $= "UseSprayCan")
				{
					%canIndex = getField(%stringB, 2);
					%this.deleteEvent(%j);
				}
				else
				{
					break;
				}
			}
			%this.event[%i] = "Server" TAB "UseSprayCan" TAB %canIndex;
		}
		else if (%event $= "UseInventory")
		{
			%idx = getField(%string, 2);
			%j = %i + 1;
			while (%j < %this.numEvents)
			{
				%stringB = %this.event[%j];
				%eventB = getField(%stringB, 1);
				if (%eventB $= "UseInventory")
				{
					%idx = getField(%stringB, 2);
					%this.deleteEvent(%j);
				}
				else if (%eventB $= "UseSprayCan")
				{
					%j++;
				}
				else
				{
					break;
				}
			}
			%this.event[%i] = "Server" TAB "UseInventory" TAB %idx;
		}
	}
}

function BuildMacroSO::deleteEvent(%this, %idx)
{
	for (%i = %idx + 1; %i < %this.numEvents; %i++)
	{
		%this.event[%i - 1] = %this.event[%i];
	}
	%this.numEvents--;
}

function BuildMacroSO::PlayBack(%this, %line)
{
	if (isEventPending(%this.playBackEvent))
	{
		cancel(%this.playBackEvent);
	}
	%line = mFloor(%line);
	%this.PlayEvent(%line);
	if (getField(%this.event[%line], 1) $= "PlantBrick")
	{
		%time = $pref::Input::MacroRate;
	}
	else
	{
		%time = 10;
	}
	if ($IamAdmin)
	{
		%time = 1;
	}
	if (%line + 1 < %this.numEvents)
	{
		%this.playBackEvent = %this.schedule(%time, PlayBack, %line++);
	}
}

function BuildMacroSO::PlayEvent(%this, %eventNum)
{
	%string = %this.event[%eventNum];
	%eventType = getField(%string, 0);
	%event = getField(%string, 1);
	%arg1 = getField(%string, 2);
	%arg2 = getField(%string, 3);
	%arg3 = getField(%string, 4);
	%arg4 = getField(%string, 5);
	%arg5 = getField(%string, 6);
	%arg6 = getField(%string, 7);
	if (%eventType $= "Client")
	{
		eval(%event @ "(" @ %arg1 @ "," @ %arg2 @ "," @ %arg3 @ "," @ %arg4 @ "," @ %arg5 @ "," @ %arg6 @ ");");
	}
	else if (%eventType $= "Server")
	{
		%event = strreplace(%event, ";", "");
		eval("%tagEvent = '" @ %event @ "';");
		commandToServer(%tagEvent, %arg1, %arg2, %arg3, %arg4, %arg5, %arg6);
	}
}

function BuildMacroSO::dump(%this)
{
	echo(%this.numEvents @ " Events");
	echo("----------------------------------------------");
	for (%i = 0; %i < %this.numEvents; %i++)
	{
		%string = %this.event[%i];
		%eventType = getField(%string, 0);
		%event = getField(%string, 1);
		%arg1 = getField(%string, 2);
		%arg2 = getField(%string, 3);
		%arg3 = getField(%string, 4);
		%arg4 = getField(%string, 5);
		%arg5 = getField(%string, 6);
		%arg6 = getField(%string, 7);
		echo("  " @ %i @ " - " @ %eventType TAB %event TAB %arg1 TAB %arg2 TAB %arg3 TAB %arg4 TAB %arg5 TAB %arg6);
	}
	echo("----------------------------------------------");
}

function demoBrickLimitGui::onWake()
{
	DBLG_StartOver.setColor("1 1 1 0.5");
}

