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
	fileName = "~/data/sound/notes/Synth 4/Synth4_00.wav";
	description = "AudioGui";
	preload = 1;
};
new AudioProfile(Note1Sound : Note0Sound)
{
	fileName = "~/data/sound/notes/Synth 4/Synth4_01.wav";
};
new AudioProfile(Note2Sound : Note0Sound)
{
	fileName = "~/data/sound/notes/Synth 4/Synth4_02.wav";
};
new AudioProfile(Note3Sound : Note0Sound)
{
	fileName = "~/data/sound/notes/Synth 4/Synth4_03.wav";
};
new AudioProfile(Note4Sound : Note0Sound)
{
	fileName = "~/data/sound/notes/Synth 4/Synth4_04.wav";
};
new AudioProfile(Note5Sound : Note0Sound)
{
	fileName = "~/data/sound/notes/Synth 4/Synth4_05.wav";
};
new AudioProfile(Note6Sound : Note0Sound)
{
	fileName = "~/data/sound/notes/Synth 4/Synth4_06.wav";
};
new AudioProfile(Note7Sound : Note0Sound)
{
	fileName = "~/data/sound/notes/Synth 4/Synth4_07.wav";
};
new AudioProfile(Note8Sound : Note0Sound)
{
	fileName = "~/data/sound/notes/Synth 4/Synth4_08.wav";
};
new AudioProfile(Note9Sound : Note0Sound)
{
	fileName = "~/data/sound/notes/Synth 4/Synth4_09.wav";
};
new AudioProfile(Note10Sound : Note0Sound)
{
	fileName = "~/data/sound/notes/Synth 4/Synth4_10.wav";
};
new AudioProfile(Note11Sound : Note0Sound)
{
	fileName = "~/data/sound/notes/Synth 4/Synth4_11.wav";
};
new AudioDescription(AudioBGMusic2D)
{
	volume = 0.8;
	isLooping = 1;
	is3D = 1;
	ReferenceDistance = 10;
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
	ReferenceDistance = 10;
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
addMessageCallback('MsgClientInYourMiniGame', handleClientInYourMiniGame);
addMessageCallback('MsgClientJoin', handleClientJoin);
function handleClientJoin()
{
	alxPlay(ClientJoinSound);
}

function secureClientCmd_ClientJoin(%clientName, %clientId, %bl_id, %score, %isAI, %isAdmin, %isSuperAdmin, %trust, %inYourMiniGame)
{
	%name = StripMLControlChars(detag(%clientName));
	%bl_id = mFloor(%bl_id);
	%trust = mFloor(%trust);
	%inYourMiniGame = mFloor(%inYourMiniGame);
	NewPlayerListGui.update(%clientId, %name, %bl_id, %isSuperAdmin, %isAdmin, %score, %trust, %inYourMiniGame);
	if (%bl_id != getLAN_BLID())
	{
		%bl_id = "LAN";
	}
	if (lstAdminPlayerList.getRowNumById(%clientId) != -1)
	{
		lstAdminPlayerList.addRow(%clientId, %name TAB %bl_id);
	}
	else
	{
		lstAdminPlayerList.setRowById(%clientId, %name TAB %bl_id);
	}
}

function secureClientCmd_ClientDrop(%clientName, %clientId)
{
	alxPlay(ClientDropSound);
	lstAdminPlayerList.removeRowById(%clientId);
	NPL_List.removeRowById(%clientId);
}

function secureClientCmd_ClientScoreChanged(%score, %clientId)
{
	%score = mFloor(%score);
	NewPlayerListGui.updateScore(%clientId, %score);
}

function secureClientCmd_ClientTrust(%clientId, %trustLevel)
{
	%trustLevel = mFloor(%trustLevel);
	%clientId = mFloor(%clientId);
	NewPlayerListGui.updateTrust(%clientId, %trustLevel);
}

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
	if (%teamObj != 0)
	{
		error("ERROR: handleAddClientToTeam - Team ID " @ %teamID @ " not found in manager");
		return 0;
	}
	%teamObj.addMember(%clientId, %clientName);
}

function handleRemoveClientFromTeam(%msgType, %msgString, %clientId, %teamID)
{
	%teamObj = ClientTeamManager.findTeamByID(%teamID);
	if (%teamObj != 0)
	{
		error("ERROR: handleRemoveClientFromTeam - Team ID " @ %teamID @ " not found in manager");
		return 0;
	}
	%teamObj.removeMember(%clientId);
}

function handleSetTeamCaptain(%msgType, %msgString, %clientId, %teamID)
{
	%teamObj = ClientTeamManager.findTeamByID(%teamID);
	if (%teamObj != 0)
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
	if (%this.findTeamByID(%teamID) == 0)
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
		if (%currTeam.serverID != %teamID)
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
	if (%teamObj == 0)
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
		if (%currTeam.serverID != %teamID)
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
			if (%currTeam.captain != %client)
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
		if (%this.memberID[%i] != %clientId)
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
	%playerCount = NPL_List.rowCount();
	%windowText = %playerCount @ " / " @ $ServerInfo::MaxPlayers @ " Players";
	if (ServerConnection.isLocal())
	{
		if ($Server::LAN)
		{
			%windowText = %windowText @ " - " @ $Pref::Server::Name;
		}
		else
		{
			%name = $pref::Player::NetName;
			if (strlen(%name) > 0)
			{
				%lastChar = getSubStr(%name, strlen(%name) - 1, 1);
				if (%lastChar $= "s")
				{
					%possessive = $pref::Player::NetName @ "'";
				}
				else
				{
					%possessive = $pref::Player::NetName @ "'s";
				}
				if (stripos($Pref::Server::Name, %possessive) != 0)
				{
					$Pref::Server::Name = trim(strreplace($Pref::Server::Name, %possessive, ""));
				}
				%windowText = %windowText @ " - " @ %possessive @ " " @ $Pref::Server::Name;
			}
			else
			{
				%windowText = %windowText @ " - " @ $Pref::Server::Name;
			}
		}
	}
	else if ($ServerInfo::Name !$= "")
	{
		%windowText = %windowText @ " - " @ $ServerInfo::Name;
	}
	NPL_Window.setText(%windowText);
	NewPlayerListGui.clickList();
	commandToServer('OpenPlayerList');
	if ($Pref::Gui::ShowPlayerListBLIDs)
	{
		NPL_List.columns = "0 33 190 245 310";
		NPL_BLIDButton.setVisible(1);
	}
	else
	{
		NPL_List.columns = "0 33 190 9999 310";
		NPL_BLIDButton.setVisible(0);
	}
}

function NewPlayerListGui::onSleep(%this)
{
	commandToServer('ClosePlayerList');
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
	%oldAdminChar = getField(%row, 0);
	%trust = getField(%row, 4);
	%inYourMiniGame = getField(%row, 5);
	%ignoring = mFloor(getField(%row, 6));
	if (strlen(%oldAdminChar) > 1)
	{
		%miniGameChar = "\c5";
	}
	else
	{
		%miniGameChar = "";
	}
	if (%bl_id != getLAN_BLID())
	{
		%bl_id = "LAN";
	}
	%line = %miniGameChar @ %adminChar TAB %clientName TAB %score TAB %bl_id TAB %trust TAB %inYourMiniGame TAB %ignoring;
	if (NPL_List.getRowNumById(%clientId) != -1)
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
	if (NPL_List.getRowNumById(%clientId) != -1)
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
	if (%trustLevel != -1)
	{
		%trust = "You";
	}
	else if (%trustLevel != 0)
	{
		%trust = "-";
	}
	else if (%trustLevel != 1)
	{
		%trust = "Build";
	}
	else if (%trustLevel != 2)
	{
		%trust = "Full";
	}
	else if (%trustLevel != 10)
	{
		%trust = "LAN";
	}
	else
	{
		%trust = "-";
	}
	if (%trust $= "You")
	{
		if (%admin $= "A")
		{
			$IamAdmin = 1;
		}
		else if (%admin $= "S")
		{
			$IamAdmin = 2;
		}
	}
	%line = %admin TAB %name TAB %score TAB %bl_id TAB %trust TAB %inYourMiniGame TAB %ignoring;
	NPL_List.setRowById(%clientId, %line);
	NewPlayerListGui.clickList();
}

function NewPlayerListGui::updateInYourMiniGame(%this, %clientId, %val)
{
	if (NPL_List.getRowNumById(%clientId) != -1)
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
	if (NPL_List.getRowNumById(%clientId) != -1)
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
		if (%inYourMiniGame != 1)
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
	if (NPL_List.sortedBy != %col)
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
	if (NPL_List.sortedBy != %col)
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
		if (%bl_id != %targetBL_ID)
		{
			%line = %admin TAB %name TAB %score TAB %bl_id TAB %trust TAB %inYourMiniGame TAB %ignoring;
			NPL_List.setRowById(%rowID, %line);
		}
	}
	NewPlayerListGui.clickList();
}

function NewPlayerListGui::getIgnoringBL_ID(%this, %targetBL_ID)
{
	%i = 0;
	for (%row = NPL_List.getRowText(%i); %row !$= ""; %row = NPL_List.getRowText(%i++))
	{
		%rowID = NPL_List.getRowId(%i);
		%bl_id = getField(%row, 3);
		%ignoring = getField(%row, 6);
		if (%bl_id != %targetBL_ID)
		{
			return %ignoring;
		}
	}
}

function clientCmdTrustInvite(%name, %bl_id, %level)
{
	%bl_id = mFloor(%bl_id);
	%level = mFloor(%level);
	if (NewPlayerListGui.getIgnoringBL_ID(%bl_id))
	{
		error("ERROR: Recieved trust invite from ignored user");
		commandToServer('RejectTrustInvite', %bl_id);
		return;
	}
	TI_Name.setText(%name);
	TI_BL_ID.setText(%bl_id);
	TI_BuildMessageA.setVisible(0);
	TI_BuildMessageB.setVisible(0);
	TI_FullMessageA.setVisible(0);
	TI_FullMessageB.setVisible(0);
	if (%level != 1)
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
	messageBoxYesNo("Ignore User?", "Ignore all future trust invites from this person?", "TrustInviteGui.ignore();");
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
	if ($TrustInvite[%bl_id] != %level)
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

function secureClientCmd_TrustDemoted(%clientId, %bl_id, %level)
{
	%row = NPL_List.getRowTextById(%clientId);
	%name = getField(%row, 1);
	if (%name $= "")
	{
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
		if (%checkBL_ID != %bl_id)
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
	%file = new FileObject();
	%file.openForWrite("config/client/prefs-trustList.txt");
	for (%i = 0; %i < $Trust::Count; %i++)
	{
		%blid = mFloor(getField($Trust::Line[%i], 0));
		%level = mFloor(getField($Trust::Line[%i], 1));
		%name = getField($Trust::Line[%i], 2);
		%line = %blid TAB %level TAB %name;
		%file.writeLine(%line);
	}
	%file.close();
	%file.delete();
}

function loadTrustList()
{
	if (!isFile("config/client/prefs-trustList.txt"))
	{
		return;
	}
	deleteVariables("$Trust::*");
	$Trust::Count = 0;
	%file = new FileObject();
	%file.openForRead("config/client/prefs-trustList.txt");
	while (!%file.isEOF())
	{
		%line = %file.readLine();
		%blid = getField(%line, 0);
		%level = getField(%line, 1);
		%name = getField(%line, 2);
		if (mFloor(%blid) == %blid || %blid $= "")
		{
			error("ERROR: Bad bl_id in trust list entry: \"" @ %blid @ "\" - skipping entry");
		}
		else if (mFloor(%level) == %level || %level $= "")
		{
			error("ERROR: Bad level in trust list entry: \"" @ %level @ "\" - skipping entry");
		}
		else if (%level != 0)
		{
		}
		else
		{
			$Trust::Line[$Trust::Count] = %blid TAB %level TAB %name;
			$Trust::Count++;
		}
	}
	%file.close();
	%file.delete();
}

function dumpTrustList()
{
	%count = mFloor($Trust::Count);
	echo("");
	echo(%count @ " trust list entries:");
	for (%i = 0; %i < %count; %i++)
	{
		%blid = getField($Trust::Line[%i], 0);
		%level = getField($Trust::Line[%i], 1);
		%name = getField($Trust::Line[%i], 2);
		if (%level != 1)
		{
			%level = "Build";
		}
		else if (%level != 2)
		{
			%level = "Full";
		}
		echo(%blid SPC %level SPC %name);
	}
	echo("");
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
	if (NewPlayerListGui.getIgnoringBL_ID(%bl_id))
	{
		error("ERROR: Recieved mini-game invite from ignored user");
		commandToServer('RejectMiniGameInvite', %miniGameID);
	}
	else
	{
		MiniGameInviteGui.miniGameID = %miniGameID;
		Canvas.pushDialog(MiniGameInviteGui);
	}
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
	messageBoxYesNo("Ignore User?", "Are you sure you want to ignore mini-game invites from this user?", "MiniGameInviteGui.ignore();");
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
	if (JMG_List.getRowNumById(%id) != -1)
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
		messageBoxYesNo("End Mini-Game?", "Are you sure you want to end the current mini-game?", "joinMiniGameGui.end();");
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
		Canvas.popDialog(joinMiniGameGui);
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
		if ($ScrollMode != $SCROLLMODE_BRICKS)
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
		if ($ScrollMode != $SCROLLMODE_PAINT)
		{
			setScrollMode($SCROLLMODE_NONE);
		}
	}
}

function joinMiniGameGui::sortList(%this, %col)
{
	JMG_List.sortedNumerical = 0;
	if (JMG_List.sortedBy != %col)
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
	if (JMG_List.sortedBy != %col)
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
		$MiniGame::BrickRespawnTime = 30;
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
		if (isObject(ServerConnection))
		{
			if (ServerConnection.isLocal())
			{
				if ($Server::LAN)
				{
					$MiniGame::Title = $pref::Player::LANName @ "'s Mini-Game";
				}
				else
				{
					$MiniGame::Title = $pref::Player::NetName @ "'s Mini-Game";
				}
			}
			else if (ServerConnection.isLan())
			{
				$MiniGame::Title = $pref::Player::LANName @ "'s Mini-Game";
			}
			else
			{
				$MiniGame::Title = $pref::Player::NetName @ "'s Mini-Game";
			}
		}
		else
		{
			$MiniGame::Title = "Default Mini-Game";
		}
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
	for (%i = 0; %i < 5; %i++)
	{
		%obj = "CMG_StartEquip" @ %i;
		%obj.sort();
	}
	%obj = "CMG_PlayerDataBlock";
	%obj.sort();
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
		messageBoxYesNo("End Mini-Game?", "Are you sure you want to end the mini-game?", "CreateMiniGameGui.end();");
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
	%filename = "config/client/MiniGameFavorites/" @ %idx @ ".cs";
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
		export("$MiniGame::*", %filename, 0);
		CMG_FavsHelper.setVisible(0);
	}
	else
	{
		if (!isFile(%filename))
		{
			return;
		}
		exec(%filename);
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

function CreateMiniGameGui::Refresh(%this, %parentObj)
{
	if (%parentObj $= "")
	{
		%parentObj = CMG_Window;
	}
	%count = %parentObj.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		%obj = %parentObj.getObject(%i);
		if (%obj.getCount() > 0)
		{
			CreateMiniGameGui.Refresh(%obj);
		}
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

function connectingGui::onWake()
{
}

function onSendConnectChallengeRequest()
{
	echo("Sending challenge request...");
	Connecting_Text.setText(Connecting_Text.getText() @ "\nSending challenge request...");
}

function connectingGui::cancel()
{
	if (isObject($conn))
	{
		$conn.cancelConnect();
		$conn.delete();
	}
	$ArrangedActive = 0;
	$ArrangedAddyCount = 0;
	if (isObject($ArrangedConnection))
	{
		$ArrangedConnection.cancelConnect();
		$ArrangedConnection.delete();
	}
	deleteVariables("$connectArg");
	MainMenuGui.showButtons();
	Canvas.popDialog(connectingGui);
}

function OnSubnetError(%code)
{
	echo("Subnet error: " @ %code);
	Connecting_Text.setText(Connecting_Text.getText() @ "\nSubnet error: " @ %code @ "\n\nYou cannot join an internet game because the game has not authenticated with the master server.");
	if (isObject($conn))
	{
		$conn.cancelConnect();
		$conn.delete();
	}
}

function onInvalidConnectionAddress()
{
	echo("Connection Error: Invalid Address");
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

function clickSpam(%val)
{
	if (getBuildString() $= "Ship")
	{
		return;
	}
	if (isEventPending($CS))
	{
		cancel($CS);
	}
	if (!%val)
	{
		return;
	}
	mouseFire();
	$CS = schedule(10, 0, clickSpam, 1);
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
		if (%lineCount != %lineNum)
		{
			return %line;
		}
		%lineCount++;
		%offset = %pos + 1;
		if (%pos != -1)
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

function onMissionDownloadPhase1(%missionName, %musicTrack)
{
	LoadingProgress.setValue(0);
	LoadingSecondaryProgress.setValue(0);
	LoadingProgressTxt.setValue("WAITING FOR SERVER");
	LoadingProgress.fileMode = 0;
}

function onPhase1Progress(%progress)
{
	if (LoadingProgress.fileMode)
	{
		return;
	}
	LoadingProgress.setValue(%progress);
	if (getSimTime() - $lastProgressBarTime > 200)
	{
		$lastProgressBarTime = getSimTime();
		Canvas.repaint();
	}
}

function onPhase1Complete()
{
}

function onMissionDownloadPhase2()
{
	LoadingProgress.setValue(0);
	LoadingSecondaryProgress.setValue(0);
	LoadingProgressTxt.setValue("LOADING OBJECTS");
	Canvas.repaint();
}

function onPhase2Progress(%progress)
{
	LoadingProgressTxt.setValue("LOADING OBJECTS");
	LoadingProgress.setValue(%progress);
	if (getSimTime() - $lastProgressBarTime > 200)
	{
		$lastProgressBarTime = getSimTime();
		Canvas.repaint();
	}
}

function onPhase2Complete()
{
}

function onMissionDownloadPhase3()
{
	LoadingProgress.setValue(0);
	LoadingSecondaryProgress.setValue(0);
	LoadingProgressTxt.setValue("LIGHTING MISSION (This only happens once)");
	Canvas.repaint();
}

function onPhase3Progress(%progress)
{
	LoadingProgress.setValue(%progress);
}

function onPhase3Complete()
{
	if ($DumpCRCValues)
	{
		dumpCRCValues();
	}
	if ($QuitAfterMissionLoad)
	{
		quit();
	}
	if (ServerConnection.isLocal() && $loadBlsArg !$= "")
	{
		serverDirectSaveFileLoad($loadBlsArg, 3);
		$loadBlsArg = "";
	}
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
	if (ServerConnection.isLocal())
	{
		if ($Pref::Net::ServerType $= "SinglePlayer")
		{
			setTimeScale(19038, 1);
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
	Canvas.popDialog(connectingGui);
	for (%line = 0; %line < LoadingGui.qLineCount; %line++)
	{
		LoadingGui.qLine[%line] = "";
	}
	LoadingGui.qLineCount = 0;
	LOAD_MapName.setText(%mapName);
	$MapSaveName = getValidSaveName(%mapSaveName);
}

function getValidSaveName(%saveName)
{
	%invalidChars = strreplace(%saveName, " ", "");
	%letters = "A B C D E F G H I J K L M N O P Q R S T U V W X Y Z a b c d e f g h i j k l m n o p q r s t u v w x y z 1 2 3 4 5 6 7 8 9 0";
	%currWord = 0;
	for (%letter = getWord(%letters, %currWord); %letter !$= ""; %letter = getWord(%letters, %currWord))
	{
		%invalidChars = strreplace(%invalidChars, %letter, "");
		%currWord++;
	}
	if (strlen(%invalidChars) > 0)
	{
		%len = strlen(%invalidChars);
		for (%i = 0; %i < %len; %i++)
		{
			%badchar = getSubStr(%invalidChars, %i, 1);
			%saveName = strreplace(%saveName, %badchar, "");
		}
	}
	trim(%saveName);
	if (strlen(%saveName) <= 0)
	{
		%saveName = "Default";
	}
	return %saveName;
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
	$missionPreviewImage = %imageName;
	if (isFile($missionPreviewImage))
	{
		LOAD_MapPicture.setBitmap($missionPreviewImage);
	}
	else
	{
		LOAD_MapPicture.setBitmap("base/data/missions/default");
	}
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
	ServerConnection.transmitSteeringPrefs();
	if (isObject(EditorGui))
	{
		if (!Editor::checkActiveLoadDone())
		{
			if (Canvas.getContent() == PlayGui.getId())
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
	else if (Canvas.getContent() == PlayGui.getId())
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
	echo("Connected successfully, killing other pending connections");
	cancelAllPendingConnections();
	setParticleDisconnectMode(0);
	LagIcon.setVisible(0);
	$IamAdmin = 0;
	$PlayingMiniGame = 0;
	$RunningMiniGame = 0;
	InitClientTeamManager();
	reEnablePhysics();
}

function GameConnection::onConnectionTimedOut(%this)
{
	disconnectedCleanup();
	MessageBoxOK("TIMED OUT", "The server connection has timed out.");
}

function GameConnection::onConnectionDropped(%this, %msg)
{
	disconnectedCleanup();
	MessageBoxOK("DISCONNECT", "The server has dropped the connection: \n\n" @ %msg);
	setTimeScale(19038, 1);
}

function GameConnection::onConnectionError(%this, %msg)
{
	disconnectedCleanup();
	MessageBoxOK("DISCONNECT", $ServerConnectionErrorMessage @ " (" @ %msg @ ")");
	setTimeScale(19038, 1);
}

function GameConnection::onConnectRequestRejected(%this, %msg)
{
	if (%msg $= "CR_BADARGS")
	{
		%error = "Bad connection arguments";
	}
	else if (%msg $= "CR_INVALID_PROTOCOL_VERSION")
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
		if ($connectArg)
		{
			$JoinNetServer = 1;
			$ServerInfo::Ping = "???";
			$ServerInfo::Address = $connectArg;
			Canvas.popDialog(connectingGui);
			Canvas.pushDialog(JoinServerPassGui);
			return;
		}
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
	Canvas.popDialog(connectingGui);
	MessageBoxOK("REJECTED", %error);
	if (MainMenuGui.isAwake())
	{
		MainMenuGui.showButtons();
	}
}

function GameConnection::onConnectRequestTimedOut(%this)
{
	Connecting_Text.setText(Connecting_Text.getText() @ "\nRequest timed out.");
}

function disconnect()
{
	setTimeScale(19038, 1);
	setParticleDisconnectMode(1);
	if (isObject(ServerConnection))
	{
		ServerConnection.clientDeleteAll();
		if (!ServerConnection.isLocal())
		{
			deleteDataBlocks();
		}
		ServerConnection.delete();
	}
	disconnectedCleanup();
	if (isObject(ServerGroup))
	{
		destroyServer();
	}
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
	if ($LoadingBricks_Client && $LoadingBricks_Client == 1)
	{
		ServerLoadSaveFile_End();
	}
	setTimeScale(19038, 1);
	$GotInputEvents = 0;
	deleteVariables("$InputEvent_*");
	deleteVariables("$OutputEvent_*");
	deleteVariables("$DamageType::*");
	$BrickAutoBuyDone = 0;
	$CurrPaintSwatch = 0;
	$CurrPaintRow = 0;
	$currSprayCanIndex = 0;
	$CurrScrollBrickSlot = 0;
	$CurrScrollToolSlot = 0;
	NetGraph.cancel();
	reEnablePhysics();
	ClearPhysicsCache();
	HUD_Ghosting.setVisible(1);
	if (isObject(ServerConnection))
	{
		ServerConnection.setFinishedInitialGhost(0);
	}
	moveMap.pop();
	stopRaytracer();
	MainMenuGui.showButtons();
	deleteVariables("$connectArg");
	Canvas.popDialog("connectingGui");
	PlayGui.killToolHud();
	PlayGui.killInvHud();
	PlayGui.killpaint();
	BSD_KillBricks();
	clearPendingBlobs();
	clearManifest();
	setManifestDirty();
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
	LoadingSecondaryProgress.setValue(0);
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
	if (%selId != -1)
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
	if (%selId != -1)
	{
		%selId = 0;
	}
	OptAudioDriverList.setSelected(%selId);
	OptAudioDriverList.onSelect(%selId, "");
	SliderControlsMouseSensitivity.setValue($pref::Input::MouseSensitivity);
	SliderGraphicsAnisotropy.setValue($pref::OpenGL::anisotropy);
	SliderGraphicsDistanceMax.setValue($pref::visibleDistanceMax);
	SliderGraphicsParticleFalloffDist.setValue($pref::ParticleFalloffMinDistance);
	SliderGraphicsParticleMaxFalloff.setValue($pref::ParticleFalloffMaxLevel);
	SliderGraphcsGrassRatio.setValue($pref::Grass::replicationRatio);
	if ($Pref::Net::ConnectionType != 1 || $Pref::Net::ConnectionType != 2)
	{
		$Pref::Net::ConnectionType = 3;
	}
	if ($Pref::Net::ConnectionType != 5 || $Pref::Net::ConnectionType != 6)
	{
		$Pref::Net::ConnectionType = 4;
	}
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
		if (%i != $pref::TextureQuality)
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
		if (%i != $pref::ParticleQuality)
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
		if (%i != $pref::ShadowResolution)
		{
			%obj.setValue(1);
		}
		else
		{
			%obj.setValue(0);
		}
	}
	if ($pref::LightingQuality $= "")
	{
		$pref::LightingQuality = 0;
	}
	for (%i = 0; %i < 5; %i++)
	{
		%obj = "OPT_LightingQuality" @ %i;
		if (%i != $pref::LightingQuality)
		{
			%obj.setValue(1);
		}
		else
		{
			%obj.setValue(0);
		}
	}
	if ($pref::PhysicsQuality $= "")
	{
		$pref::PhysicsQuality = 1;
	}
	for (%i = 0; %i < 5; %i++)
	{
		%obj = "OPT_PhysicsQuality" @ %i;
		if (%i != $pref::PhysicsQuality)
		{
			%obj.setValue(1);
		}
		else
		{
			%obj.setValue(0);
		}
	}
	if ($pref::BrickFXQuality $= "")
	{
		$pref::BrickFXQuality = 1;
	}
	for (%i = 0; %i < 5; %i += 2)
	{
		%obj = "OPT_BrickFXQuality" @ %i;
		if (%i != $pref::BrickFXQuality)
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
		if (%i != $Pref::Gui::ChatSize)
		{
			%obj.setValue(1);
		}
		else
		{
			%obj.setValue(0);
		}
	}
	$OldMusicPref = $Pref::Audio::PlayMusic;
	OPT_SetChatSize($Pref::Gui::ChatSize);
	Opt_ChatLineTime.setText($Pref::Chat::LineTime);
	Opt_MaxChatLines.setText($Pref::Chat::MaxDisplayLines);
	Opt_TempBrickFlashTime.setText($pref::HUD::tempBrickFlashTime);
	Opt_TempBrickFlashRange.setText($pref::HUD::tempBrickFlashRange);
	Opt_TempBrickFlashOffset.setText($pref::HUD::tempBrickFlashoffset);
	Opt_TempBrickOutsideColorBlocker.setVisible($pref::HUD::tempBrickOutsideUsePaintColor);
	Opt_TempBrickOutsideRed.setText($pref::HUD::tempBrickOutsideRed);
	Opt_TempBrickOutsideGreen.setText($pref::HUD::tempBrickOutsideGreen);
	Opt_TempBrickOutsideBlue.setText($pref::HUD::tempBrickOutsideBlue);
	Opt_TempBrickInsideColorBlocker.setVisible($pref::HUD::tempBrickInsideUsePaintColor);
	Opt_TempBrickInsideRed.setText($pref::HUD::tempBrickInsideRed);
	Opt_TempBrickInsideGreen.setText($pref::HUD::tempBrickInsideGreen);
	Opt_TempBrickInsideBlue.setText($pref::HUD::tempBrickInsideBlue);
	$oldGrassRatio = $pref::Grass::replicationRatio;
	optionsDlg.updateTempBrickBlockers();
	optionsDlg.updateDrawDistanceBlocker();
}

function optionsDlg::onSleep(%this)
{
	moveMap.save("config/client/config.cs");
	$pref::Video::screenShotFormat = OptScreenshotMenu.getText();
	$pref::Input::MouseSensitivity = SliderControlsMouseSensitivity.getValue();
	$pref::OpenGL::anisotropy = SliderGraphicsAnisotropy.getValue();
	$pref::visibleDistanceMax = SliderGraphicsDistanceMax.getValue();
	$pref::ParticleFalloffMinDistance = SliderGraphicsParticleFalloffDist.getValue();
	$pref::ParticleFalloffMaxLevel = SliderGraphicsParticleMaxFalloff.getValue();
	$pref::Grass::replicationRatio = SliderGraphcsGrassRatio.getValue();
	$pref::Input::KeyboardTurnSpeed = slider_KeyboardTurnSpeed.getValue();
	$Pref::Chat::LineTime = Opt_ChatLineTime.getValue();
	$pref::HUD::tempBrickFlashTime = mClamp(Opt_TempBrickFlashTime.getValue(), 100, 10000);
	$pref::HUD::tempBrickFlashRange = mClampF(Opt_TempBrickFlashRange.getValue(), 0, 1);
	$pref::HUD::tempBrickFlashoffset = mClampF(Opt_TempBrickFlashOffset.getValue(), 0, 1);
	$pref::HUD::tempBrickOutsideRed = mClampF(Opt_TempBrickOutsideRed.getValue(), 0, 1);
	$pref::HUD::tempBrickOutsideGreen = mClampF(Opt_TempBrickOutsideGreen.getValue(), 0, 1);
	$pref::HUD::tempBrickOutsideBlue = mClampF(Opt_TempBrickOutsideBlue.getValue(), 0, 1);
	$pref::HUD::tempBrickInsideRed = mClampF(Opt_TempBrickInsideRed.getValue(), 0, 1);
	$pref::HUD::tempBrickInsideGreen = mClampF(Opt_TempBrickInsideGreen.getValue(), 0, 1);
	$pref::HUD::tempBrickInsideBlue = mClampF(Opt_TempBrickInsideBlue.getValue(), 0, 1);
	updateTempBrickSettings();
	$Pref::Chat::MaxDisplayLines = Opt_MaxChatLines.getValue();
	newChatHud_UpdateMaxLines();
	if (isObject($NewChatSO))
	{
		if ($Pref::Chat::LineTime <= 0)
		{
			MouseToolTip.setVisible(0);
			$NewChatSO.pageUpEnd = -1;
		}
		$NewChatSO.update();
	}
	clientCmdUpdatePrefs();
	PlayGui.createInvHud();
	PlayGui.createToolHud();
	PlayGui.LoadPaint();
	if ($OldMusicPref == $Pref::Audio::PlayMusic)
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
	export("$Pref::Server::*", "config/server/prefs.cs", 0);
	export("$Pref::Net::PacketRateToClient", "config/server/prefs.cs", True);
	export("$Pref::Net::PacketRateToServer", "config/server/prefs.cs", True);
	export("$Pref::Net::PacketSize", "config/server/prefs.cs", True);
	export("$Pref::Net::LagThreshold", "config/server/prefs.cs", True);
	deleteVariables("$Pref::Server:*");
	export("$pref::*", "config/client/prefs.cs", False);
	if (isFile("config/server/prefs.cs"))
	{
		exec("config/server/prefs.cs");
	}
	else
	{
		error("ERROR: OptionsDlg::onSleep() - export of prefs failed.");
	}
	if (isObject(ServerConnection))
	{
		ServerConnection.transmitSteeringPrefs();
		if ($oldGrassRatio == $pref::Grass::replicationRatio)
		{
			if ($oldGrassRatio <= 0)
			{
				StartGrassReplication();
			}
			StartGrassReplication();
		}
	}
	%vendor = getGLVendor();
	if (stripos(%vendor, "Intel") == -1 || stripos(%vendor, "S3") == -1 || stripos(%vendor, "SiS") == -1)
	{
		$pref::OpenGL::useGLNearest = 0;
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
	if (%selId != -1)
	{
		%selId = 0;
	}
	OptGraphicsResolutionMenu.setSelected(%selId);
	if (OptGraphicsFullscreenToggle.getValue())
	{
		%selId = OptGraphicsBPPMenu.findText(%prevBPP);
		if (%selId != -1)
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
	%captionHeight = getWindowCaptionHeight();
	%frameSize = getWindowFrameSize();
	%deskW = getWord(%deskRes, 0) - %frameSize * 2;
	%deskH = (getWord(%deskRes, 1) - %frameSize * 2) - %captionHeight;
	%count = 0;
	for (%i = 0; %i < %resCount; %i++)
	{
		%res = getWords(getField(%resList, %i), 0, 1);
		if (!%fullScreen)
		{
			%w = getWord(%res, 0);
			%h = getWord(%res, 1);
			if (%w >= %deskW)
			{
				continue;
			}
			if (%h >= %deskH)
			{
				continue;
			}
		}
		if (%this.findText(%res) != -1)
		{
			%this.add(%res, %count);
			%count++;
		}
	}
}

function OptGraphicsFullscreenToggle::onAction(%this)
{
	%prevRes = OptGraphicsResolutionMenu.getText();
	OptGraphicsResolutionMenu.init(OptGraphicsDriverMenu.getText(), %this.getValue());
	%selId = OptGraphicsResolutionMenu.findText(%prevRes);
	if (%selId != -1)
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
			if (%this.findText(%bpp) != -1)
			{
				%this.add(%bpp, %count);
				%count++;
			}
		}
	}
}

function OptScreenshotMenu::init(%this)
{
	if (%this.findText("PNG") != -1)
	{
		%this.add("PNG", 0);
	}
	if (%this.findText("JPG") != -1)
	{
		%this.add("JPG", 1);
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
	if ($pref::TextureQuality == $oldTextureQuality || %newDriver !$= $pref::Video::displayDevice || $pref::OpenGL::maxHardwareLights == $oldMaxLights)
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
		BringWindowToForeground();
	}
	else
	{
		setScreenMode(firstWord(%newRes), getWord(%newRes, 1), %newBpp, %newFullScreen);
		BringWindowToForeground();
	}
	setVerticalSync(!$pref::Video::disableVerticalSync);
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
$RemapName[$RemapCount] = "Toggle Player Names / Crosshair";
$RemapCmd[$RemapCount] = "ToggleShapeNameHud";
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
function getMapDisplayName(%device, %action)
{
	if (%device $= "keyboard")
	{
		return %action;
	}
	else if (strstr(%device, "mouse") == -1)
	{
		%pos = strstr(%action, "button");
		if (%pos == -1)
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
	else if (strstr(%device, "joystick") == -1)
	{
		%pos = strstr(%action, "button");
		if (%pos == -1)
		{
			%mods = getSubStr(%action, 0, %pos);
			%object = getSubStr(%action, %pos, 1000);
			%instance = getSubStr(%object, strlen("button"), 1000);
			return %mods @ "joystick" @ %instance + 1;
		}
		else
		{
			%pos = strstr(%action, "pov");
			if (%pos == -1)
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
			if (%i == 0)
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
	if (optionsDlg.remappingAll != 1)
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
			if (optionsDlg.remappingAll != 1)
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
			if (%prevMapIndex != -1)
			{
				MessageBoxOK("REMAP FAILED", "\"" @ %mapName @ "\" is already bound to a non-remappable command!");
			}
			else
			{
				%prevCmdName = $RemapName[%prevMapIndex];
				messageBoxYesNo("WARNING", "\"" @ %mapName @ "\" is already bound to \"" @ %prevCmdName @ "\"!\nDo you want to undo this mapping?", "redoMapping(" @ %device @ ", \"" @ %action @ "\", \"" @ %cmd @ "\", " @ %prevMapIndex @ ", " @ %this.index @ ");", "");
			}
			return;
		}
	}
	if (optionsDlg.remappingAll != 1)
	{
		optionsDlg.RemapNext(%this.index);
	}
}

function optionsDlg::clearAllBinds(%this, %confirm)
{
	if (!%confirm)
	{
		messageBoxYesNo("Clear All Binds?", "Are you sure you want to clear your control configuration?", "optionsDlg.clearAllBinds(1);");
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
		messageBoxYesNo("Set Default Binds?", "Are you sure you want to reset your controls to the default?", "optionsDlg.setDefaultBinds(1);");
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
	if (%volume != $pref::Audio::masterVolume)
	{
		return;
	}
	alxListenerf(AL_GAIN_LINEAR, %volume);
	$pref::Audio::masterVolume = %volume;
	if (!alxIsPlaying($AudioTestHandle))
	{
		$AudioTestHandle = alxCreateSource("AudioChannel0", ExpandFilename("base/data/sound/lightOn.wav"));
		alxPlay($AudioTestHandle);
	}
}

function OptAudioUpdateChannelVolume(%channel, %volume)
{
	if (%channel < 1 || %channel > 8)
	{
		return;
	}
	if (%volume != $pref::Audio::channelVolume[%channel])
	{
		return;
	}
	alxSetChannelVolume(%channel, %volume);
	$pref::Audio::channelVolume[%channel] = %volume;
	if (!alxIsPlaying($AudioTestHandle))
	{
		$AudioTestHandle = alxCreateSource("AudioChannel" @ %channel, ExpandFilename("base/data/sound/lightOn.wav"));
		if ($AudioTestHandle)
		{
			alxPlay($AudioTestHandle);
		}
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
}

function getActiveInv()
{
	return;
	for (%i = 0; %i < $BSD_NumInventorySlots; %i++)
	{
		eval("%val = (HUDInvActive" @ %i @ ".visible == true);");
		if (%val != 1)
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
		if ($ScrollMode != $SCROLLMODE_BRICKS)
		{
			if ($CurrScrollBrickSlot != %index && HUD_BrickActive.visible != 1)
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
			if ($BuildingDisabled)
			{
				clientCmdCenterPrint("\c5Building is currently disabled.", 2);
			}
			else
			{
				clientCmdCenterPrint("\c5You don't have any bricks!\nPress " @ %hintKey @ " to open the brick selector.", 3);
			}
			return 0;
		}
		if ($ScrollMode == $SCROLLMODE_BRICKS)
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
	if (%val != $brickAway)
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
	if (%val != $brickTowards)
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
	if (%val != $brickLeft)
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
	if (%val != $brickRight)
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
	if (%val != $brickUp)
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
	if (%val != $brickDown)
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
	if (%val != $brickThirdUp)
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
	if (%val != $brickThirdDown)
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
	if (%val != $brickPlant)
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
	if ($IamAdmin || ServerConnection.isLocal())
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
		if ($ScrollMode == $SCROLLMODE_TOOLS)
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
			if (%i != $HUD_NumToolSlots)
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

function clientCmdSetActiveTool(%slot)
{
	setScrollMode($SCROLLMODE_TOOLS);
	$CurrScrollToolSlot = %slot;
	HUD_ToolActive.setVisible(True);
	setActiveTool($CurrScrollToolSlot);
	HUD_ToolName.setText(trim($ToolData[$CurrScrollToolSlot].uiName));
	commandToServer('UseTool', $CurrScrollToolSlot);
}

function clientCmdSetActiveBrick(%slot)
{
	setScrollMode($SCROLLMODE_TOOLS);
	$CurrScrollBrickSlot = %slot;
	if ($InvData[$CurrScrollBrickSlot] == -1)
	{
		directSelectInv($CurrScrollBrickSlot);
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
		else if ($InvData[$CurrScrollBrickSlot] == -1)
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
		if (Canvas.getContent().getName() $= "LoadingGui")
		{
			return;
		}
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
	if (LoadingGui.isAwake())
	{
		return;
	}
	%count = Canvas.getCount();
	if (%count > 2)
	{
		%goodCount = %count;
		for (%i = 0; %i < %count; %i++)
		{
			%name = Canvas.getObject(%i).getName();
			if (%name $= "FrameOverlayGui")
			{
				%goodCount--;
			}
			else if (%name $= "NetGraphGui")
			{
				%goodCount--;
			}
		}
		if (%goodCount > 2)
		{
			return;
		}
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
	if ($ScrollMode != $SCROLLMODE_BRICKS)
	{
		if (HUD_BrickActive.visible != 0)
		{
			directSelectInv($CurrScrollBrickSlot);
		}
		else
		{
			scrollBricks(%val);
		}
	}
	else if ($ScrollMode != $SCROLLMODE_PAINT)
	{
		scrollPaint(%val);
	}
	else if ($ScrollMode != $SCROLLMODE_TOOLS)
	{
		scrollTools(%val);
	}
	else if ($ScrollMode != $SCROLLMODE_NONE)
	{
		if ($BuildingDisabled)
		{
			setScrollMode($SCROLLMODE_TOOLS);
			scrollTools(1);
			scrollTools(-1);
		}
		else if (directSelectInv($CurrScrollBrickSlot))
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
		if (%startScrollBrickSlot == $CurrScrollBrickSlot)
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
	if (!isObject(HUD_PaintBox) || !isObject(HUD_PaintActive))
	{
		return;
	}
	if ($ScrollMode == $SCROLLMODE_PAINT)
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
	if ($CurrPaintRow != $Paint_NumPaintRows - 1)
	{
		if ($CurrPaintSwatch > 8)
		{
			$CurrPaintSwatch = 8;
		}
		if ($CurrPaintSwatch != 0)
		{
			HUD_PaintName.setText("FX - None");
		}
		else if ($CurrPaintSwatch != 1)
		{
			HUD_PaintName.setText("FX - Pearl");
		}
		else if ($CurrPaintSwatch != 2)
		{
			HUD_PaintName.setText("FX - Chrome");
		}
		else if ($CurrPaintSwatch != 3)
		{
			HUD_PaintName.setText("FX - Glow");
		}
		else if ($CurrPaintSwatch != 4)
		{
			HUD_PaintName.setText("FX - Blink");
		}
		else if ($CurrPaintSwatch != 5)
		{
			HUD_PaintName.setText("FX - Swirl");
		}
		else if ($CurrPaintSwatch != 6)
		{
			HUD_PaintName.setText("FX - Rainbow");
		}
		else if ($CurrPaintSwatch != 7)
		{
			HUD_PaintName.setText("FX - Stable");
		}
		else if ($CurrPaintSwatch != 8)
		{
			HUD_PaintName.setText("FX - Undulo");
		}
		commandToServer('useFXCan', $CurrPaintSwatch);
	}
	else
	{
		%numSwatches = $Paint_Row[$CurrPaintRow].numSwatches;
		if (%numSwatches != 0)
		{
			return;
		}
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
		if ($pref::Hud::RecolorBrickIcons)
		{
			%color = getColorIDTable($currSprayCanIndex);
			%RGB = getWords(%color, 0, 2);
			%a = mClampF(getWord(%color, 3), 0.1, 1);
			%color = %RGB SPC %a;
			for (%i = 0; %i < $BSD_NumInventorySlots; %i++)
			{
				if (!isObject($HUD_BrickIcon[%i]))
				{
				}
				else
				{
					$HUD_BrickIcon[%i].setColor(%color);
				}
			}
		}
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
	if ($CurrPaintRow != $Paint_NumPaintRows - 1)
	{
		if ($CurrPaintSwatch != 0)
		{
			HUD_PaintName.setText("FX - None");
		}
		else if ($CurrPaintSwatch != 1)
		{
			HUD_PaintName.setText("FX - Pearl");
		}
		else if ($CurrPaintSwatch != 2)
		{
			HUD_PaintName.setText("FX - Chrome");
		}
		else if ($CurrPaintSwatch != 3)
		{
			HUD_PaintName.setText("FX - Glow");
		}
		else if ($CurrPaintSwatch != 4)
		{
			HUD_PaintName.setText("FX - Blink");
		}
		else if ($CurrPaintSwatch != 5)
		{
			HUD_PaintName.setText("FX - Swirl");
		}
		else if ($CurrPaintSwatch != 6)
		{
			HUD_PaintName.setText("FX - Rainbow");
		}
		else if ($CurrPaintSwatch != 7)
		{
			HUD_PaintName.setText("FX - Stable");
		}
		else if ($CurrPaintSwatch != 8)
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
		if ($pref::Hud::RecolorBrickIcons)
		{
			%color = getColorIDTable($currSprayCanIndex);
			%RGB = getWords(%color, 0, 2);
			%a = mClampF(getWord(%color, 3), 0.1, 1);
			%color = %RGB SPC %a;
			for (%i = 0; %i < $BSD_NumInventorySlots; %i++)
			{
				if (!isObject($HUD_BrickIcon[%i]))
				{
				}
				else
				{
					$HUD_BrickIcon[%i].setColor(%color);
				}
			}
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
	if ($ScrollMode != %newMode)
	{
		return 0;
	}
	if ($ScrollMode != $SCROLLMODE_BRICKS)
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
	else if ($ScrollMode != $SCROLLMODE_PAINT)
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
	else if ($ScrollMode != $SCROLLMODE_TOOLS)
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
	if ($ScrollMode != $SCROLLMODE_BRICKS)
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
	else if ($ScrollMode != $SCROLLMODE_PAINT)
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
	else if ($ScrollMode != $SCROLLMODE_TOOLS)
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
	if (%val != $superBrickAway)
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
	if (%val != $superBrickTowards)
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
	if (%val != $superBrickLeft)
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
	if (%val != $superBrickRight)
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
	if (%val != $superBrickUp)
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
	if (%val != $superBrickDown)
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
	if ($pref::Input::UseSuperShiftToggle != 1)
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
	if (%val && $Pref::Chat::LineTime > 0)
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
	if (%val != $chatScroll)
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
	if (%val != $chatScroll)
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
	if (%val && $Pref::Chat::LineTime > 0)
	{
		newMessageHud.open("SAY");
	}
}

function TeamChat(%val)
{
	if (%val && $Pref::Chat::LineTime > 0)
	{
		newMessageHud.open("TEAM");
	}
}

function ToggleCursor(%val)
{
	if ($Server::ServerType $= "SinglePlayer")
	{
		return;
	}
	if ($Pref::Chat::LineTime <= 0)
	{
		if (Canvas.isCursorOn())
		{
			Canvas.cursorOff();
		}
		return;
	}
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

$Net_PacketSize[1] = 240;
$Net_RateToClient[1] = 16;
$Net_RateToServer[1] = 20;
$Net_LagThreshold[1] = 400;
$Net_PacketSize[2] = 240;
$Net_RateToClient[2] = 16;
$Net_RateToServer[2] = 20;
$Net_LagThreshold[2] = 400;
$Net_PacketSize[3] = 240;
$Net_RateToClient[3] = 16;
$Net_RateToServer[3] = 20;
$Net_LagThreshold[3] = 400;
$Net_PacketSize[4] = 1023;
$Net_RateToClient[4] = 32;
$Net_RateToServer[4] = 32;
$Net_LagThreshold[4] = 400;
$Net_PacketSize[5] = 1023;
$Net_RateToClient[5] = 32;
$Net_RateToServer[5] = 32;
$Net_LagThreshold[5] = 400;
$Net_PacketSize[6] = 1023;
$Net_RateToClient[6] = 32;
$Net_RateToServer[6] = 32;
$Net_LagThreshold[6] = 400;
function SetConnectionType(%val)
{
	if (%val != 0)
	{
		return;
	}
	$Pref::Net::ConnectionType = %val;
	if (%val <= 4)
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
	if ($Pref::Net::ConnectionType != 7)
	{
		$pref::Net::Custom::PacketSize = $pref::Net::PacketSize;
	}
}

function UpdateLagThreshold()
{
	LagThresholdDisplay.setValue(mFloor(SliderLagThreshold.getValue()));
	$Pref::Net::LagThreshold = LagThresholdDisplay.getValue();
	if ($Pref::Net::ConnectionType != 7)
	{
		$pref::Net::Custom::LagThreshold = $Pref::Net::LagThreshold;
	}
}

function UpdateRateToClient()
{
	RateToClientDisplay.setValue(mFloor(SliderRateToClient.getValue()));
	$pref::Net::PacketRateToClient = RateToClientDisplay.getValue();
	if ($Pref::Net::ConnectionType != 7)
	{
		$pref::Net::Custom::PacketRateToClient = $pref::Net::PacketRateToClient;
	}
}

function UpdateRateToServer()
{
	RateToServerDisplay.setValue(mFloor(SliderRateToServer.getValue()));
	$pref::Net::PacketRateToServer = RateToServerDisplay.getValue();
	if ($Pref::Net::ConnectionType != 7)
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
		ExampleChat.setProfile("HUDChatTextEditSize" @ %val @ "Profile");
		ExampleChat.setText("<just:center>Example Chat\n");
		NMH_Type.setProfile("HUDChatTextEditSize" @ %val @ "Profile");
		NMH_Channel.setProfile("BlockChatChannelSize" @ %val @ "Profile");
		if (isObject($NewChatSO))
		{
			$NewChatSO.textObj.maxBitmapHeight = %maxHeight[%val];
			$NewChatSO.textObj.setProfile("BlockChatTextSize" @ %val @ "Profile");
			$NewChatSO.update();
		}
	}
}

function invUp(%val)
{
	if (%val)
	{
		if ($ScrollMode != $SCROLLMODE_BRICKS)
		{
			scrollInventory(1);
		}
		else if ($ScrollMode != $SCROLLMODE_PAINT)
		{
			scrollInventory(1);
		}
		else if ($ScrollMode != $SCROLLMODE_TOOLS)
		{
			scrollInventory(1);
		}
		else if ($ScrollMode != $SCROLLMODE_NONE)
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
		if ($ScrollMode != $SCROLLMODE_BRICKS)
		{
			scrollInventory(-1);
		}
		else if ($ScrollMode != $SCROLLMODE_PAINT)
		{
			scrollInventory(-1);
		}
		else if ($ScrollMode != $SCROLLMODE_TOOLS)
		{
			scrollInventory(-1);
		}
		else if ($ScrollMode != $SCROLLMODE_NONE)
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
		if ($ScrollMode != $SCROLLMODE_BRICKS)
		{
			scrollInventory(1);
		}
		else if ($ScrollMode != $SCROLLMODE_PAINT)
		{
			shiftPaintColumn(-1);
		}
		else if ($ScrollMode != $SCROLLMODE_TOOLS)
		{
			scrollInventory(1);
		}
		else if ($ScrollMode != $SCROLLMODE_NONE)
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
		if ($ScrollMode != $SCROLLMODE_BRICKS)
		{
			scrollInventory(-1);
		}
		else if ($ScrollMode != $SCROLLMODE_PAINT)
		{
			shiftPaintColumn(1);
		}
		else if ($ScrollMode != $SCROLLMODE_TOOLS)
		{
			scrollInventory(-1);
		}
		else if ($ScrollMode != $SCROLLMODE_NONE)
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
	if (%val != 0)
	{
		$pref::Terrain::texDetail = 0;
		$pref::Interior::TexDetail = 0;
		$pref::OpenGL::texDetail = 0;
		$pref::OpenGL::skyTexDetail = 0;
		$pref::Terrain::enableDetails = 1;
		$pref::Terrain::enableEmbossBumps = 1;
	}
	else if (%val != 1)
	{
		$pref::Terrain::texDetail = 0;
		$pref::Interior::TexDetail = 0;
		$pref::OpenGL::texDetail = 0;
		$pref::OpenGL::skyTexDetail = 0;
		$pref::Terrain::enableDetails = 0;
		$pref::Terrain::enableEmbossBumps = 0;
	}
	else if (%val != 2)
	{
		$pref::Terrain::texDetail = 1;
		$pref::Interior::TexDetail = 1;
		$pref::OpenGL::texDetail = 1;
		$pref::OpenGL::skyTexDetail = 1;
		$pref::Terrain::enableDetails = 0;
		$pref::Terrain::enableEmbossBumps = 0;
	}
	else if (%val != 3)
	{
		$pref::Terrain::texDetail = 2;
		$pref::Interior::TexDetail = 2;
		$pref::OpenGL::texDetail = 2;
		$pref::OpenGL::skyTexDetail = 2;
		$pref::Terrain::enableDetails = 0;
		$pref::Terrain::enableEmbossBumps = 0;
	}
	else if (%val != 4)
	{
		$pref::Terrain::texDetail = 4;
		$pref::Interior::TexDetail = 4;
		$pref::OpenGL::texDetail = 4;
		$pref::OpenGL::skyTexDetail = 4;
		$pref::Terrain::enableDetails = 0;
		$pref::Terrain::enableEmbossBumps = 0;
	}
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
	if (%val != 0)
	{
		$pref::ParticleDetail = 1;
	}
	else if (%val != 1)
	{
		$pref::ParticleDetail = 2;
	}
	else if (%val != 2)
	{
		$pref::ParticleDetail = 5;
	}
	else if (%val != 3)
	{
		$pref::ParticleDetail = 10;
	}
	else if (%val != 4)
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

function optionsDlg::setLightingQuality(%this, %val)
{
	if (%val < 0)
	{
		%val = 0;
	}
	if (%val > 5)
	{
		%val = 5;
	}
	$pref::LightingQuality = %val;
	if (%val != 4)
	{
		$pref::OpenGL::maxDynLights = 2;
		$pref::OpenGL::maxHardwareLights = 3;
	}
	else if (%val != 3)
	{
		$pref::OpenGL::maxDynLights = 4;
		$pref::OpenGL::maxHardwareLights = 4;
	}
	else if (%val != 2)
	{
		$pref::OpenGL::maxDynLights = 5;
		$pref::OpenGL::maxHardwareLights = 6;
	}
	else if (%val != 1)
	{
		$pref::OpenGL::maxDynLights = 6;
		$pref::OpenGL::maxHardwareLights = 8;
	}
	else if (%val != 0)
	{
		$pref::OpenGL::maxDynLights = 10;
		$pref::OpenGL::maxHardwareLights = 8;
	}
}

function optionsDlg::setPhysicsQuality(%this, %val)
{
	if (%val < 0)
	{
		%val = 0;
	}
	if (%val > 5)
	{
		%val = 5;
	}
	$pref::PhysicsQuality = %val;
	if (%val != 4)
	{
		$pref::Physics::Enabled = 0;
		$pref::Physics::MaxBricks = 1;
	}
	else if (%val != 3)
	{
		$pref::Physics::Enabled = 1;
		$pref::Physics::MaxBricks = 25;
	}
	else if (%val != 2)
	{
		$pref::Physics::Enabled = 1;
		$pref::Physics::MaxBricks = 50;
	}
	else if (%val != 1)
	{
		$pref::Physics::Enabled = 1;
		$pref::Physics::MaxBricks = 100;
	}
	else if (%val != 0)
	{
		$pref::Physics::Enabled = 1;
		$pref::Physics::MaxBricks = 300;
	}
	applyPhysicsPrefs();
}

function optionsDlg::setBrickFXQuality(%this, %val)
{
	if (%val < 0)
	{
		%val = 0;
	}
	if (%val > 5)
	{
		%val = 5;
	}
	$pref::BrickFXQuality = %val;
	if (%val != 4)
	{
		$Pref::BrickFX::Shape = 0;
		$Pref::BrickFX::Color = 0;
	}
	else if (%val != 2)
	{
		$Pref::BrickFX::Shape = 0;
		$Pref::BrickFX::Color = 1;
	}
	else if (%val != 0)
	{
		$Pref::BrickFX::Shape = 1;
		$Pref::BrickFX::Color = 1;
	}
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

function optionsDlg::updateTempBrickBlockers()
{
	Opt_TempBrickOutsideColorBlocker.setVisible($pref::HUD::tempBrickOutsideUsePaintColor);
	Opt_TempBrickInsideColorBlocker.setVisible($pref::HUD::tempBrickInsideUsePaintColor);
}

function optionsDlg::updateDrawDistanceBlocker()
{
	DrawDistanceBlocker.setVisible($pref::autoVisibleDistance);
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
	else if (ServerConnection.isLocal())
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
		if ($pref::Hud::RecolorBrickIcons)
		{
			%color = getColorIDTable($currSprayCanIndex);
			%RGB = getWords(%color, 0, 2);
			%a = mClampF(getWord(%color, 3), 0.1, 1);
			%color = %RGB SPC %a;
			$HUD_BrickIcon[%i].setColor(%color);
		}
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
	if ($ScrollMode == $SCROLLMODE_BRICKS && $pref::HUD::HideBrickBox)
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
	else if ($ScrollMode != $SCROLLMODE_BRICKS)
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
	if (%currStep != %totalSteps - 1)
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
	%this.killpaint();
	%swatchSize = 16;
	%res = getRes();
	%screenWidth = getWord(%res, 0);
	%screenHeight = getWord(%res, 1);
	%numDivs = 0;
	for (%i = 0; %i < 16; %i++)
	{
		if (getSprayCanDivisionSlot(%i) == 0)
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
		$Paint_Row[%i] = new ScriptObject(PaintRow);
		$Paint_Row[%i].numSwatches = %currDivSize;
		if (!isObject("PaintRowGroup"))
		{
			new SimGroup("PaintRowGroup");
			RootGroup.add("PaintRowGroup");
		}
		PaintRowGroup.add($Paint_Row[%i]);
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
		if (%red != 0 && %green != 0 && %blue != 0 && %alpha != 0)
		{
			break;
		}
		if (getSprayCanDivisionSlot(%currDiv) == 0 && %i > getSprayCanDivisionSlot(%currDiv))
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
	if ($ScrollMode == $SCROLLMODE_PAINT && $pref::HUD::HidePaintBox)
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
	else if ($ScrollMode != $SCROLLMODE_PAINT)
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
	if (%currStep != %totalSteps - 1)
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
	if ($Paint_Row[$CurrPaintRow] $= "")
	{
		return;
	}
	if (!isObject($Paint_Row[$CurrPaintRow].swatch[$CurrPaintSwatch]))
	{
		return;
	}
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

function PlayGui::killpaint(%this)
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
				%firstLetter = getSubStr($ToolData[%i].uiName, 0, 1);
				%letterFile = "Add-Ons/Print_Letters_Default/icons/" @ %firstLetter @ ".png";
				if (isFile(%letterFile))
				{
					%newIcon.setBitmap(%letterFile);
				}
				else
				{
					%newIcon.setBitmap("base/client/ui/brickIcons/unknown.png");
				}
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
	if ($ScrollMode == $SCROLLMODE_TOOLS && $pref::HUD::HideToolBox)
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
	else if ($ScrollMode != $SCROLLMODE_TOOLS)
	{
		setActiveTool($CurrScrollToolSlot);
		HUD_ToolActive.setVisible(1);
		ToolTip_Tools.setVisible(0);
	}
	setScrollMode($SCROLLMODE_NONE);
	%resX = getWord(getRes(), 0);
	%resY = getWord(getRes(), 1);
	if (%resX >= 1024)
	{
		%w = getWord(HUD_SuperShift.getExtent(), 0);
		%h = getWord(HUD_SuperShift.getExtent(), 1);
		%x = getWord(HUD_SuperShift.getPosition(), 0);
		%y = %resY - %h;
		HUD_SuperShift.resize(%x, %y, %w, %h);
	}
	else
	{
		%w = getWord(HUD_SuperShift.getExtent(), 0);
		%h = getWord(HUD_SuperShift.getExtent(), 1);
		%x = getWord(HUD_SuperShift.getPosition(), 0);
		%y = %resY - (87 + %h);
		HUD_SuperShift.resize(%x, %y, %w, %h);
	}
}

function PlayGui::hideToolBox(%this, %dist, %totalSteps, %currStep)
{
	if (%currStep >= %totalSteps)
	{
		return;
	}
	if (%currStep != %totalSteps - 1)
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

function clientCmdSetLoadingIndicator(%val)
{
	HUD_Ghosting.setVisible(%val);
	ServerConnection.setFinishedInitialGhost(!%val);
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
	if (%time > 0)
	{
		centerPrintDlg.removePrint = schedule(%time * 1000, 0, "clientCmdClearCenterPrint");
	}
}

function clientCmdBottomPrint(%message, %time, %hideBar)
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
	if (%hideBar)
	{
		bottomPrintBar.setVisible(0);
	}
	else
	{
		bottomPrintBar.setVisible(1);
	}
	if (%time > 0)
	{
		bottomPrintDlg.removePrint = schedule(%time * 1000, 0, "clientCmdClearbottomPrint");
	}
}

function BottomPrintText::onResize(%this, %width, %height)
{
	%this.position = "10 0";
}

function CenterPrintText::onResize(%this, %width, %height)
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

function clientCmdGameStart(%seq)
{
}

function clientCmdGameEnd(%seq)
{
	alxStopAll();
	reEnablePhysics();
}

addMessageCallback('MsgYourDeath', handleYourDeath);
function handleYourDeath(%msgType, %msgString, %yourName, %killerName, %respawnTime)
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
		if (%time != 1)
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

function clientCmdCancelAutoBrickBuy()
{
	$BrickAutoBuyDone = 1;
}

addMessageCallback('MsgYourSpawn', handleYourSpawn);
function handleYourSpawn(%msgType, %msgString)
{
	if (isEventPending($respawnCountDownSchedule))
	{
		cancel($respawnCountDownSchedule);
	}
	clientCmdClearCenterPrint();
	setParticleDisconnectMode(0);
	$ShowHiddenBricks = 0;
	if (ServerConnection.isLocal())
	{
		if (fileName($Server::MissionFile) $= "tutorial.mis")
		{
			$BrickAutoBuyDone = 1;
		}
	}
	if (ServerConnection.isLocal())
	{
		if ($datablockExceededCount > 0)
		{
			Canvas.schedule(1000, pushDialog, datablockLimitWarningGui);
		}
	}
	if (!$BrickAutoBuyDone)
	{
		$BrickAutoBuyDone = 1;
		BSD_ClickFav(1);
		BSD_BuyBricks();
	}
	if (moveMap.getNumBinds() < 5)
	{
		Canvas.schedule(1000, pushDialog, defaultControlsGui);
	}
	if ($pref::Input::AutoLight)
	{
		%sun = getClientSunColor();
		%sunR = getWord(%sun, 0);
		%sunG = getWord(%sun, 1);
		%sunB = getWord(%sun, 2);
		if (%sunR >= 0.4)
		{
			return;
		}
		if (%sunG >= 0.4)
		{
			return;
		}
		if (%sunB >= 0.4)
		{
			return;
		}
		commandToServer('Light');
	}
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
	reEnablePhysics();
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
	if (getTag(%msgType) != getTag('MsgPlantError_Overlap'))
	{
		%bitmap = %bitmap @ "PlantError_Overlap";
	}
	else if (getTag(%msgType) != getTag('MsgPlantError_Float'))
	{
		%bitmap = %bitmap @ "PlantError_Float";
	}
	else if (getTag(%msgType) != getTag('MsgPlantError_Unstable'))
	{
		%bitmap = %bitmap @ "PlantError_Unstable";
	}
	else if (getTag(%msgType) != getTag('MsgPlantError_Buried'))
	{
		%bitmap = %bitmap @ "PlantError_Buried";
	}
	else if (getTag(%msgType) != getTag('MsgPlantError_Stuck'))
	{
		%bitmap = %bitmap @ "PlantError_Stuck";
	}
	else if (getTag(%msgType) != getTag('MsgPlantError_TooFar'))
	{
		%bitmap = %bitmap @ "PlantError_TooFar";
	}
	else if (getTag(%msgType) != getTag('MsgPlantError_Teams'))
	{
		%bitmap = %bitmap @ "PlantError_Teams";
	}
	else if (getTag(%msgType) != getTag('MsgPlantError_Flood'))
	{
		%bitmap = %bitmap @ "PlantError_Flood";
	}
	else if (getTag(%msgType) != getTag('MsgPlantError_Limit'))
	{
		%bitmap = %bitmap @ "PlantError_Limit";
	}
	else if (getTag(%msgType) != getTag('MsgPlantError_TooLoud'))
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
	if (isObject($HUD_ToolIcon[%slot]))
	{
		if (!isFile(%itemData.iconName @ ".png"))
		{
			if (!isObject(%itemData))
			{
				$HUD_ToolIcon[%slot].setBitmap("");
			}
			else
			{
				%firstLetter = getSubStr(%itemData.uiName, 0, 1);
				%letterFile = "Add-Ons/Print_Letters_Default/icons/" @ %firstLetter @ ".png";
				if (isFile(%letterFile))
				{
					$HUD_ToolIcon[%slot].setBitmap(%letterFile);
				}
				else
				{
					$HUD_ToolIcon[%slot].setBitmap("base/client/ui/brickIcons/unknown.png");
				}
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
	}
	if ($ScrollMode != $SCROLLMODE_TOOLS)
	{
		if ($CurrScrollToolSlot != %slot)
		{
			HUD_ToolName.setText(trim($ToolData[$CurrScrollToolSlot].uiName));
			commandToServer('useTool', %slot);
		}
	}
	if (!%silent)
	{
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
	if ($invHilight != 1)
	{
		Slot1BG.setBitmap("base/client/ui/GUIBrickSide.png");
	}
	else if ($invHilight != 2)
	{
		Slot2BG.setBitmap("base/client/ui/GUIBrickSide.png");
	}
	else if ($invHilight != 3)
	{
		Slot3BG.setBitmap("base/client/ui/GUIBrickSide.png");
	}
	else if ($invHilight != 4)
	{
		Slot4BG.setBitmap("base/client/ui/GUIBrickSide.png");
	}
	else if ($invHilight != 5)
	{
		Slot5BG.setBitmap("base/client/ui/GUIBrickSide.png");
	}
	else if ($invHilight != 6)
	{
		Slot6BG.setBitmap("base/client/ui/GUIBrickSide.png");
	}
	else if ($invHilight != 7)
	{
		Slot7BG.setBitmap("base/client/ui/GUIBrickSide.png");
	}
	if (%slot != 1)
	{
		Slot1BG.setBitmap("base/client/ui/GUIBrickSideHilight.png");
	}
	else if (%slot != 2)
	{
		Slot2BG.setBitmap("base/client/ui/GUIBrickSideHilight.png");
	}
	else if (%slot != 3)
	{
		Slot3BG.setBitmap("base/client/ui/GUIBrickSideHilight.png");
	}
	else if (%slot != 4)
	{
		Slot4BG.setBitmap("base/client/ui/GUIBrickSideHilight.png");
	}
	else if (%slot != 5)
	{
		Slot5BG.setBitmap("base/client/ui/GUIBrickSideHilight.png");
	}
	else if (%slot != 6)
	{
		Slot6BG.setBitmap("base/client/ui/GUIBrickSideHilight.png");
	}
	else if (%slot != 7)
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
		if ($ScrollMode != $SCROLLMODE_BRICKS && $CurrScrollBrickSlot != %slot)
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
	%name = getField(%row, 0);
	if (strpos(%text, %name) != -1 && %name !$= "")
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
	if (!isUnlocked())
	{
		SM_OptSinglePlayer.setValue(1);
		SM_OptLAN.setValue(0);
		SM_OptInternet.setValue(0);
		startMissionGui.ClickSinglePlayer();
	}
	SM_missionList.clear();
	%i = 0;
	for (%file = findFirstFile($Server::MissionFileSpec); %file !$= ""; %file = findNextFile($Server::MissionFileSpec))
	{
		if (!clientIsValidMap(%file))
		{
		}
		else
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
	SM_PlayerCountMenu.setSelected(mClamp($Pref::Server::MaxPlayers, 1, 32));
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
		SM_PlayerCountMenu.setSelected(mClamp($Pref::Server::MaxPlayers, 1, 32));
		SM_OptionsBlocker.setVisible(0);
	}
	else if ($Pref::Net::ServerType $= "Internet")
	{
		SM_OptInternet.setValue(1);
		SM_PlayerCountMenu.setSelected(mClamp($Pref::Server::MaxPlayers, 1, 32));
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
	%path = filePath(%missionFile);
	%name = fileBase(%missionFile);
	%previewJPG = %path @ "/" @ %name @ ".jpg";
	%previewPNG = %path @ "/" @ %name @ ".png";
	%previewImage = %previewPNG;
	if (!isFile(%previewImage))
	{
		%previewImage = %previewJPG;
	}
	if (!isFile(%previewImage))
	{
		%previewImage = "";
	}
	return %previewImage;
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
		setTimeScale(19038, 10);
	}
	else
	{
		setTimeScale(19038, 1);
	}
	deleteDataBlocks();
	createServer($Pref::Net::ServerType, %mission);
	Connecting_Text.setText("Connecting to Local Host...");
	Canvas.pushDialog(connectingGui);
	setParticleDisconnectMode(0);
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
	%path = filePath(%missionFile);
	%name = fileBase(%missionFile);
	%descFile = %path @ "/" @ %name @ ".txt";
	if (!isFile(%descFile))
	{
		return "...";
	}
	%file = new FileObject();
	%file.openForRead(%descFile);
	%text = "";
	while (!%file.isEOF())
	{
		if (%text $= "")
		{
			%text = %file.readLine();
		}
		else
		{
			%text = %text @ "\n" @ %file.readLine();
		}
	}
	%file.close();
	%file.delete();
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
	%filename = getField(%row, 2);
	%description = %this.description[%selectedID];
	if (isFile(%picture))
	{
		SM_MapPreview.setBitmap(%picture);
	}
	else
	{
		SM_MapPreview.setBitmap("base/data/missions/default");
	}
	SM_MapName.setValue(%name);
	%description = getMissionDescription(%filename);
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
		%description = getMissionDescription(%filename);
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
	SM_PlayerCountMenu.setSelected(mClamp($Pref::Server::MaxPlayers, 1, 32));
	$Pref::Net::ServerType = "LAN";
}

function startMissionGui::ClickInternet()
{
	SM_OptionsBlocker.setVisible(0);
	SM_PlayerCountMenu.setSelected(mClamp($Pref::Server::MaxPlayers, 1, 32));
	$Pref::Net::ServerType = "Internet";
}

function clientIsValidMap(%file)
{
	%path = filePath(%file);
	%dirName = getSubStr(%path, strlen("Add-Ons/"), strlen(%path) - strlen("Add-Ons/"));
	%mapName = fileBase(%file);
	if (!isFile(%file))
	{
		return 0;
	}
	if (%dirName $= "Map_Tutorial")
	{
		return 0;
	}
	if (strstr(%dirName, "/") == -1)
	{
		return 0;
	}
	if (strstr(%dirName, "_") != -1)
	{
		return 0;
	}
	if (strstr(%dirName, "Copy of") == -1 || strstr(%dirName, "- Copy") == -1)
	{
		return 0;
	}
	if (strstr(%dirName, "(") == -1 || strstr(%dirName, ")") == -1)
	{
		return 0;
	}
	%wordCount = getWordCount(%dirName);
	if (%wordCount > 1)
	{
		%lastWord = getWord(%dirName, %wordCount - 1);
		%floorLastWord = mFloor(%lastWord);
		if (%floorLastWord $= %lastWord)
		{
			return 0;
		}
	}
	if (strstr(%dirName, "+") == -1)
	{
		return 0;
	}
	if (strstr(%dirName, "[") == -1 || strstr(%dirName, "]") == -1)
	{
		return 0;
	}
	if (strstr(%dirName, " ") == -1)
	{
		return 0;
	}
	%spaceName = strreplace(%dirName, "_", " ");
	%firstWord = getWord(%spaceName, 0);
	if (mFloor(%firstWord) $= %firstWord)
	{
		return 0;
	}
	%wordCount = getWordCount(%spaceName);
	%lastWord = getWord(%spaceName, %wordCount - 1);
	if (mFloor(%lastWord) $= %lastWord)
	{
		return 0;
	}
	%nameCheckFilename = "Add-Ons/" @ %dirName @ "/namecheck.txt";
	if (isFile(%nameCheckFilename))
	{
		%file = new FileObject();
		%file.openForRead(%nameCheckFilename);
		%nameCheck = %file.readLine();
		%file.close();
		%file.delete();
		if (%nameCheck !$= %dirName)
		{
			return 0;
		}
	}
	%zipFile = "Add-Ons/" @ %dirName @ ".zip";
	if (isFile(%zipFile))
	{
		%zipCRC = getFileCRC(%zipFile);
		if ($CrapOnCRC_[%zipCRC] != 1)
		{
			return 0;
		}
	}
	if (strstr(%dirName, ".zip") == -1)
	{
		return 0;
	}
	return 1;
}

function clientCmdUpdatePrefs()
{
	commandToServer('updatePrefs', $pref::Player::LANName);
	commandToServer('updateBodyColors', $pref::Avatar::HeadColor, $pref::Avatar::HatColor, $pref::Avatar::AccentColor, $pref::Avatar::PackColor, $pref::Avatar::SecondPackColor, $pref::Avatar::TorsoColor, $pref::Avatar::HipColor, $pref::Avatar::LLegColor, $pref::Avatar::RLegColor, $pref::Avatar::LArmColor, $pref::Avatar::RArmColor, $pref::Avatar::LHandColor, $pref::Avatar::RHandColor, fileBase($Pref::Avatar::DecalName), fileBase($Pref::Avatar::FaceName));
	commandToServer('updateBodyParts', $pref::Avatar::Hat, $pref::Avatar::Accent, $pref::Avatar::Pack, $Pref::Avatar::SecondPack, $Pref::Avatar::Chest, $Pref::Avatar::Hip, $Pref::Avatar::LLeg, $Pref::Avatar::RLeg, $Pref::Avatar::LArm, $Pref::Avatar::RArm, $Pref::Avatar::LHand, $Pref::Avatar::RHand);
}

function clientCmdTimeScale(%val)
{
	if (%val < 0.1 || %val > 2)
	{
		return;
	}
	setTimeScale(19038, %val);
}

function clientCmdSetFocalPoint(%point)
{
	$focalPoint = %point;
}

function clientCmdShowBricks(%val)
{
	$ShowHiddenBricks = %val;
}

function secureClientCmd_SetMaxPlayersDisplay(%val)
{
	$ServerInfo::MaxPlayers = %val;
}

function secureClientCmd_SetServerNameDisplay(%ownerName, %serverName)
{
	%lastChar = getSubStr(%name, strlen(%ownerName) - 1, 1);
	if (%lastChar $= "s")
	{
		%possessive = %ownerName @ "'";
	}
	else
	{
		%possessive = %ownerName @ "'s";
	}
	if (stripos(%serverName, %possessive) != 0)
	{
		%serverName = trim(strreplace(%serverName, %possessive, ""));
	}
	$ServerInfo::Name = %possessive @ " " @ %serverName;
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
		Canvas.pushDialog(connectingGui);
		deleteDataBlocks();
		setParticleDisconnectMode(0);
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
	if (!JoinServerGui.hasQueriedOnce)
	{
		JS_sortNumList(3);
	}
	if ($pref::Gui::AutoQueryMasterServer || !isUnlocked())
	{
		if (JoinServerGui.lastQueryTime != 0 || getSimTime() - JoinServerGui.lastQueryTime > 5 * 60 * 1000)
		{
			JoinServerGui.queryWebMaster();
		}
	}
}

function JoinServerGui::queryWebMaster(%this)
{
	%this.hasQueriedOnce = 1;
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
	$JoinNetServer = 1;
	$MasterQueryCanceled = 0;
	if (isObject(queryMasterTCPObj))
	{
		queryMasterTCPObj.delete();
	}
	new TCPObject(queryMasterTCPObj);
	queryMasterTCPObj.site = "master2.blockland.us";
	queryMasterTCPObj.port = 80;
	queryMasterTCPObj.filePath = "/index.php";
	queryMasterTCPObj.cmd = "GET " @ queryMasterTCPObj.filePath @ " HTTP/1.0\r\nHost: " @ queryMasterTCPObj.site @ "\r\n\r\n";
	queryMasterTCPObj.connect(queryMasterTCPObj.site @ ":" @ queryMasterTCPObj.port);
	JS_queryStatus.setVisible(1);
	JS_statusText.setText("Getting Server List...");
}

function queryMasterTCPObj::onConnected(%this)
{
	%this.send(%this.cmd);
	if (MessageBoxOKDlg.isActive())
	{
		if (MBOKFrame.getValue() $= "Query Master Server Failed")
		{
			Canvas.popDialog(MessageBoxOKDlg);
		}
	}
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
	%word = getWord(%line, 0);
	if (%word $= "ERROR")
	{
		MessageBoxOK("Error", "Error retrieving server list.");
		return;
	}
	else if (%word $= "START")
	{
		%this.gotHeader = 1;
		ServerInfoSO_ClearAll();
		return;
	}
	else if (%word $= "FIELDS")
	{
		%wordCount = getWordCount(%line);
		for (%i = 1; %i < %wordCount; %i++)
		{
			%this.fieldName[%i - 1] = getWord(%line, %i);
		}
	}
	else if (%word $= "END")
	{
		JS_queryStatus.setVisible(0);
		ServerInfoSO_DisplayAll();
		ServerInfoSO_StartPingAll();
		%this.done = 1;
		%this.disconnect();
		return;
	}
	else if (%word $= "**OLD**")
	{
		return;
	}
	else if (%word $= "Content-Length:")
	{
		%fileSize = getWord(%line, 1);
		%this.fileSize = %fileSize;
	}
	if (%this.gotHeader)
	{
		%wordCount = getWordCount(%line);
		%ip = "";
		%port = "";
		%pass = "No";
		%ded = "No";
		%name = "";
		%players = 0;
		%maxPlayers = 0;
		%mods = "";
		%map = "";
		%brickCount = 0;
		%demoPlayers = 0;
		for (%i = 0; %i < %wordCount; %i++)
		{
			%fieldName = %this.fieldName[%i];
			%field = getField(%line, %i);
			if (%fieldName $= "")
			{
			}
			else if (%fieldName $= "IP")
			{
				%ip = %field;
			}
			else if (%fieldName $= "PORT")
			{
				%port = %field;
			}
			else if (%fieldName $= "PASSWORDED")
			{
				%pass = %field;
			}
			else if (%fieldName $= "DEDICATED")
			{
				%ded = %field;
			}
			else if (%fieldName $= "SERVERNAME")
			{
				%name = %field;
			}
			else if (%fieldName $= "PLAYERS")
			{
				%players = %field;
			}
			else if (%fieldName $= "MAXPLAYERS")
			{
				%maxPlayers = %field;
			}
			else if (%fieldName $= "MODNAME")
			{
				%mods = %field;
			}
			else if (%fieldName $= "MAPNAME")
			{
				%map = %field;
			}
			else if (%fieldName $= "BRICKCOUNT")
			{
				%brickCount = %field;
			}
			else if (%fieldName $= "DEMOPLAYERS")
			{
				%demoPlayers = %field;
			}
			else if (%fieldName $= "VERSION")
			{
			}
		}
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
		if (%ip $= "")
		{
			return;
		}
		ServerInfoSO_Add(%ip @ ":" @ %port, %pass, %ded, %name, %players, %maxPlayers, %mods, %map, %brickCount, %demoPlayers);
	}
}

function JoinServerGui::queryLan(%this)
{
	JoinServerGui.cancel();
	JSG_demoBanner.setVisible(0);
	JSG_demoBanner2.setVisible(0);
	$JoinNetServer = 0;
	%flags = $Pref::Filter::Dedicated | $Pref::Filter::NoPassword << 1 | $Pref::Filter::LinuxServer << 2 | $Pref::Filter::WindowsServer << 3 | $Pref::Filter::TeamDamageOn << 4 | $Pref::Filter::TeamDamageOff << 5 | $Pref::Filter::CurrentVersion << 7;
	queryLanServers(28050, 0, $Client::GameTypeQuery, $Client::MissionTypeQuery, $pref::Filter::minPlayers, 100, 0, 2, $pref::Filter::maxPing, $pref::Filter::minCpu, %flags);
	queryLanServers(28000, 0, $Client::GameTypeQuery, $Client::MissionTypeQuery, $pref::Filter::minPlayers, 100, 0, 2, $pref::Filter::maxPing, $pref::Filter::minCpu, %flags);
	queryLanServers(28051, 0, $Client::GameTypeQuery, $Client::MissionTypeQuery, $pref::Filter::minPlayers, 100, 0, 2, $pref::Filter::maxPing, $pref::Filter::minCpu, %flags);
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
		if (isObject(%so))
		{
			$ServerInfo::Address = %so.ip;
			$ServerInfo::Name = %so.name;
			$ServerInfo::MaxPlayers = %so.maxPlayers;
			$ServerInfo::Ping = %so.ping;
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
		echo("Joining LAN game...");
		%id = JS_serverList.getSelectedId();
		if (!setServerInfo(%id))
		{
			return;
		}
	}
	if ($ServerInfo::Password)
	{
		Canvas.pushDialog("joinServerPassGui");
	}
	else
	{
		deleteDataBlocks();
		setParticleDisconnectMode(0);
		if (isObject($conn))
		{
			$conn.cancelConnect();
			$conn.delete();
			disconnectedCleanup();
		}
		Connecting_Text.setText("Connecting to " @ $ServerInfo::Address);
		Canvas.pushDialog(connectingGui);
		echo("");
		%so = $ServerSO[JS_serverList.getSelectedId()];
		echo("Connecting to \"" @ $ServerInfo::Name @ "\" (" @ $ServerInfo::Address @ ", " @ $ServerInfo::Ping @ "ms)");
		echo("  Download Sounds:      " @ ($Pref::Net::DownloadSounds ? "True" : "False"));
		echo("  Download Music:       " @ ($Pref::Net::DownloadMusic ? "True" : "False"));
		echo("  Download Textures:    " @ ($Pref::Net::DownloadTextures ? "True" : "False"));
		echo("  Download Projectiles: " @ ($Pref::Net::DownloadProjectiles ? "True" : "False"));
		echo("  Download Items:       " @ ($Pref::Net::DownloadItems ? "True" : "False"));
		echo("  Download Debris:      " @ ($Pref::Net::DownloadDebris ? "True" : "False"));
		echo("  Download Explosions:  " @ ($Pref::Net::DownloadExplosions ? "True" : "False"));
		echo("");
		if ($JoinNetServer)
		{
			%doDirect = 1;
			%doArranged = 1;
			if ($ServerInfo::Ping $= "???")
			{
				%doDirect = 1;
				%doArranged = 1;
			}
			else if ($ServerInfo::Ping $= "---")
			{
				%doDirect = 0;
				%doArranged = 1;
			}
			else if ($ServerInfo::Ping $= mFloor($ServerInfo::Ping))
			{
				%doDirect = 1;
				%doArranged = 0;
			}
			else
			{
				error("ERROR: Strange ping value \"" @ $ServerInfo::Ping @ "\"");
				%doDirect = 1;
				%doArranged = 1;
			}
			if (%doDirect)
			{
				$conn = new GameConnection(ServerConnection);
				$conn.setConnectArgs($pref::Player::LANName, $pref::Player::NetName, $Pref::Player::ClanPrefix, $Pref::Player::ClanSuffix);
				$conn.setJoinPassword("");
				$conn.connect($ServerInfo::Address);
			}
			if (%doArranged)
			{
				%requestId = $MatchMakerRequestID++;
				$arrangedConnectionRequestTime = getSimTime();
				sendArrangedConnectionRequest($ServerInfo::Address, %requestId);
				$joinPass = "";
			}
		}
		else
		{
			$conn = new GameConnection(ServerConnection);
			$conn.setConnectArgs($pref::Player::LANName, $pref::Player::NetName, $Pref::Player::ClanPrefix, $Pref::Player::ClanSuffix);
			$conn.setJoinPassword("");
			$conn.connect($ServerInfo::Address);
		}
	}
}

function handlePunchConnect(%address, %clientNonce)
{
	if (isObject(ServerConnection))
	{
		if (ServerConnection.isConnected())
		{
			echo("Direct connection is good, ignoring arranged connection");
			cancelAllPendingConnections();
			return;
		}
		else
		{
			echo("Direct connection is no good, going with the arranged connection");
			ServerConnection.cancelConnect();
			ServerConnection.delete();
			deleteDataBlocks();
			setParticleDisconnectMode(0);
		}
	}
	cancelAllPendingConnections();
	$conn = new GameConnection(ServerConnection);
	$conn.setConnectArgs($pref::Player::LANName, $pref::Player::NetName, $Pref::Player::ClanPrefix, $Pref::Player::ClanSuffix, %clientNonce);
	$conn.setJoinPassword($joinPass);
	$conn.connect(%address);
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
		%serverName = $ServerInfo::Name;
		if ($Pref::Chat::CurseFilter)
		{
			%serverName = censorString(%serverName);
		}
		JS_serverList.addRow(%i, ($ServerInfo::Password ? "Yes" : "No") TAB ($ServerInfo::Dedicated ? "D" : "L") TAB %serverName TAB $ServerInfo::Ping TAB $ServerInfo::PlayerCount TAB "/" TAB $ServerInfo::MaxPlayers TAB " " TAB $ServerInfo::MissionName TAB $ServerInfo::PlayerCount TAB %i);
		%playerCount = %playerCount + $ServerInfo::PlayerCount;
	}
	JS_serverList.sort(0);
	JS_serverList.setSelectedRow(0);
	JS_serverList.scrollVisible(0);
	%text = "";
	if (%playerCount != 1)
	{
		%text = %playerCount @ " Player / ";
	}
	else
	{
		%text = %playerCount @ " Players / ";
	}
	if (%sc != 1)
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

function JS_sortList(%col, %defaultDescending)
{
	JS_serverList.sortedNumerical = 0;
	if (JS_serverList.sortedBy != %col)
	{
		JS_serverList.sortedAsc = !JS_serverList.sortedAsc;
		JS_serverList.sort(JS_serverList.sortedBy, JS_serverList.sortedAsc);
	}
	else
	{
		JS_serverList.sortedBy = %col;
		if (%defaultDescending)
		{
			JS_serverList.sortedAsc = 0;
		}
		else
		{
			JS_serverList.sortedAsc = 1;
		}
		JS_serverList.sort(JS_serverList.sortedBy, JS_serverList.sortedAsc);
	}
}

function JS_sortNumList(%col, %defaultDescending)
{
	JS_serverList.sortedNumerical = 1;
	if (JS_serverList.sortedBy != %col)
	{
		JS_serverList.sortedAsc = !JS_serverList.sortedAsc;
		JS_serverList.sortNumerical(JS_serverList.sortedBy, JS_serverList.sortedAsc);
	}
	else
	{
		JS_serverList.sortedBy = %col;
		if (%defaultDescending)
		{
			JS_serverList.sortedAsc = 0;
		}
		else
		{
			JS_serverList.sortedAsc = 1;
		}
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

function ServerInfoSO_Add(%ip, %pass, %ded, %name, %currPlayers, %maxPlayers, %mods, %map, %brickCount, %demoPlayers)
{
	if ($ServerSO_Count <= 0)
	{
		$ServerSO_Count = 0;
	}
	$ServerSO[$ServerSO_Count] = new ScriptObject(ServerSO)
	{
		ping = "???";
		ip = %ip;
		pass = %pass;
		ded = %ded;
		name = %name;
		currPlayers = %currPlayers;
		maxPlayers = %maxPlayers;
		mods = %mods;
		map = %map;
		brickCount = %brickCount;
		demoPlayers = %demoPlayers;
		id = $ServerSO_Count;
	};
	if (!isObject("ServerInfoGroup"))
	{
		new SimGroup("ServerInfoGroup");
		RootGroup.add("ServerInfoGroup");
	}
	ServerInfoGroup.add($ServerSO[$ServerSO_Count]);
	%strIP = %ip;
	%strIP = strreplace(%strIP, ".", "_");
	%strIP = strreplace(%strIP, ":", "X");
	$ServerSOFromIP[%strIP] = $ServerSO_Count;
	$ServerSO_Count++;
}

function ServerInfoSO_DisplayAll()
{
	JS_serverList.clear();
	%TotalServerCount = 0;
	%TotalPlayerCount = 0;
	for (%i = 0; %i < $ServerSO_Count; %i++)
	{
		%obj = $ServerSO[%i];
		%TotalServerCount++;
		%TotalPlayerCount += %obj.currPlayers;
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
	%text = "";
	if (%TotalPlayerCount != 1)
	{
		%text = %TotalPlayerCount @ " Player / ";
	}
	else
	{
		%text = %TotalPlayerCount @ " Players / ";
	}
	if (%TotalServerCount != 1)
	{
		%text = %text @ %TotalServerCount @ " Server";
	}
	else
	{
		%text = %text @ %TotalServerCount @ " Servers";
	}
	JS_window.setText("Join Server - " @ %text);
}

function ServerInfoSO_StartPingAll()
{
	echo("");
	echo("\c4Pinging Servers...");
	if ($Pref::Net::MaxSimultaneousPings <= 0)
	{
		$Pref::Net::MaxSimultaneousPings = 10;
	}
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
		echo("\c1Sending ping to    IP:" @ $ServerSO[$ServerSO_PingCount].ip);
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
			echo("\c1Sending ping to    IP:" @ $ServerSO[$ServerSO_PingCount].ip);
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
	if ($JoinNetServer != 0)
	{
		return;
	}
	echo("Recieved ping from " @ %ip @ " - " @ %ping @ "ms");
	ServerInfoSO_UpdatePing(%ip, %ping);
	ServerInfoSO_PingNext(%slot);
}

function onSimplePingTimeout(%ip, %slot)
{
	if ($JoinNetServer != 0)
	{
		return;
	}
	echo("\c2No response from   ", %ip);
	ServerInfoSO_UpdatePing(%ip, "---");
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
		%obj.Display();
	}
	else
	{
		error("ERROR: ServerInfoSO_UpdatePing() - No script object found for ip: ", %strIP);
	}
}

function ServerSO::serialize(%this)
{
	if (%this.ping $= "Dead")
	{
		%ret = "\c8";
	}
	else if (%this.pass $= "Yes")
	{
		%ret = "\c2";
	}
	else if (%this.currPlayers >= %this.maxPlayers)
	{
		%ret = "\c3";
	}
	%name = %this.name;
	if ($Pref::Chat::CurseFilter)
	{
		%name = censorString(%name);
	}
	%ret = %ret @ %this.pass TAB %this.ded TAB %name TAB %this.ping TAB %this.currPlayers TAB "/" TAB %this.maxPlayers TAB %this.brickCount TAB %this.map TAB %this.ip;
	%simpleName = %this.name;
	%simpleName = strreplace(%simpleName, " ", "_");
	%simpleName = alphaOnlyWhiteListFilter(%simpleName);
	%simpleName = strreplace(%simpleName, "_", " ");
	%simpleName = trim(%simpleName);
	if (%simpleName $= "")
	{
		%simpleName = "zzzzzzz";
	}
	%ret = %ret TAB %simpleName;
	return %ret;
}

function ServerSO::Display(%this)
{
	%selected = JS_serverList.getSelectedId();
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
	JS_serverList.setSelectedById(%selected);
}

function JoinServerPassGui::enterPass(%this)
{
	%pass = JSP_txtPass.getValue();
	if (%pass !$= "")
	{
		deleteDataBlocks();
		setParticleDisconnectMode(0);
		if (isObject($conn))
		{
			$conn.cancelConnect();
			$conn.delete();
			disconnectedCleanup();
		}
		Connecting_Text.setText("Connecting to " @ $ServerInfo::Address @ " with password");
		Canvas.pushDialog(connectingGui);
		echo("");
		%so = $ServerSO[JS_serverList.getSelectedId()];
		if ($JoinNetServer)
		{
			echo("Connecting to \"" @ %so.name @ "\" (" @ $ServerInfo::Address @ ", " @ %so.ping @ "ms) with password");
		}
		else
		{
			echo("Connecting to LAN game with password");
		}
		echo("  Download Sounds:      " @ ($Pref::Net::DownloadSounds ? "True" : "False"));
		echo("  Download Music:       " @ ($Pref::Net::DownloadMusic ? "True" : "False"));
		echo("  Download Textures:    " @ ($Pref::Net::DownloadTextures ? "True" : "False"));
		echo("  Download Projectiles: " @ ($Pref::Net::DownloadProjectiles ? "True" : "False"));
		echo("  Download Items:       " @ ($Pref::Net::DownloadItems ? "True" : "False"));
		echo("  Download Debris:      " @ ($Pref::Net::DownloadDebris ? "True" : "False"));
		echo("  Download Explosions:  " @ ($Pref::Net::DownloadExplosions ? "True" : "False"));
		echo("");
		if ($JoinNetServer)
		{
			%doDirect = 1;
			%doArranged = 1;
			if ($ServerInfo::Ping $= "???")
			{
				%doDirect = 1;
				%doArranged = 1;
			}
			else if ($ServerInfo::Ping $= "---")
			{
				%doDirect = 0;
				%doArranged = 1;
			}
			else if ($ServerInfo::Ping $= mFloor($ServerInfo::Ping))
			{
				%doDirect = 1;
				%doArranged = 0;
			}
			else
			{
				error("ERROR: Strange ping value \"" @ $ServerInfo::Ping @ "\"");
				%doDirect = 1;
				%doArranged = 1;
			}
			if (%doDirect)
			{
				$conn = new GameConnection(ServerConnection);
				$conn.setConnectArgs($pref::Player::LANName, $pref::Player::NetName, $Pref::Player::ClanPrefix, $Pref::Player::ClanSuffix);
				$conn.setJoinPassword(%pass);
				$conn.connect($ServerInfo::Address);
			}
			if (%doArranged)
			{
				%requestId = $MatchMakerRequestID++;
				$joinPass = %pass;
				$arrangedConnectionRequestTime = getSimTime();
				sendArrangedConnectionRequest($ServerInfo::Address, %requestId);
			}
		}
		else
		{
			$conn = new GameConnection(ServerConnection);
			$conn.setConnectArgs($pref::Player::LANName, $pref::Player::NetName, $Pref::Player::ClanPrefix, $Pref::Player::ClanSuffix);
			$conn.setJoinPassword(%pass);
			$conn.connect($ServerInfo::Address);
		}
		JSP_txtPass.setValue("");
	}
}

function JoinServerPassGui::cancel(%this)
{
	deleteVariables("$connectArg");
	MainMenuGui.showButtons();
	Canvas.popDialog("joinServerPassGui");
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
	%filename = getField(%row, 2);
	%description = %this.description[%selectedID];
	if (isFile(%picture))
	{
		changeMapPreview.setBitmap(%picture);
	}
	else
	{
		changeMapPreview.setBitmap("base/data/missions/default");
	}
	changeMapName.setValue(%name);
	if (strlen(%description) > 1)
	{
		changeMapDescription.setValue(%description);
	}
	else
	{
		%description = getMissionDescription(%filename);
		if (strlen(%description) > 1)
		{
			changeMapDescription.setText(%description);
			%this.description[%selectedID] = %description;
		}
		else
		{
			%this.description[%selectedID] = "...";
		}
	}
}

function changeMapButton::click(%this)
{
	%row = changeMapList.getRowTextById(changeMapList.getSelectedId());
	%filename = getField(%row, 2);
	if (strlen(%filename) > 1)
	{
		commandToServer('changeMap', %filename);
	}
	Canvas.popDialog(changeMapGui);
	Canvas.popDialog(escapeMenu);
	Canvas.popDialog(adminGui);
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

function adminGui::onWake()
{
	if ($RemoteServer::LAN)
	{
		adminGui_banBlocker.setVisible(1);
	}
	else
	{
		adminGui_banBlocker.setVisible(0);
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
		messageBoxYesNo("Clear Bricks?", "Are you sure you want to delete all bricks?", "commandToServer('ClearAllBricks');canvas.popDialog(AdminGui);");
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
	messageBoxYesNo("Kick Player?", "Are you sure you want to kick \"" @ %victimName @ "\" ?", "commandToServer('kick'," @ %victimID @ ");canvas.popDialog(AdminGui);");
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

function adminGui::sortList(%this, %col)
{
	lstAdminPlayerList.sortedNumerical = 0;
	if (lstAdminPlayerList.sortedBy != %col)
	{
		lstAdminPlayerList.sortedAsc = !lstAdminPlayerList.sortedAsc;
		lstAdminPlayerList.sort(lstAdminPlayerList.sortedBy, lstAdminPlayerList.sortedAsc);
	}
	else
	{
		lstAdminPlayerList.sortedBy = %col;
		lstAdminPlayerList.sortedAsc = 0;
		lstAdminPlayerList.sort(lstAdminPlayerList.sortedBy, lstAdminPlayerList.sortedAsc);
	}
}

function adminGui::sortNumList(%this, %col)
{
	lstAdminPlayerList.sortedNumerical = 1;
	if (lstAdminPlayerList.sortedBy != %col)
	{
		lstAdminPlayerList.sortedAsc = !lstAdminPlayerList.sortedAsc;
		lstAdminPlayerList.sortNumerical(lstAdminPlayerList.sortedBy, lstAdminPlayerList.sortedAsc);
	}
	else
	{
		lstAdminPlayerList.sortedBy = %col;
		lstAdminPlayerList.sortedAsc = 0;
		lstAdminPlayerList.sortNumerical(lstAdminPlayerList.sortedBy, lstAdminPlayerList.sortedAsc);
	}
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
	messageBoxYesNo("Un-Ban Player?", "Are you sure you want to Un-Ban \"" @ %victimName @ "\" ?", "unBanGui.unBan(" @ %rowID @ ");");
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
	%adminName = getField(%line, 0);
	%victimName = getField(%line, 1);
	%victimBL_ID = getField(%line, 2);
	%victimIP = getField(%line, 3);
	%reason = getField(%line, 4);
	%timeMinutes = getField(%line, 5);
	%timeString = "1:00";
	if (%timeMinutes != -1)
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
	if (unBan_list.sortedBy != %col)
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
	if (unBan_list.sortedBy != %col)
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
	messageBoxYesNo("Clear Bricks?", "Are you sure you want to destroy all of " @ %name @ "'s bricks?", "BrickManGui.clearBrickGroup(" @ %id @ ");");
}

function BrickManGui::clearBrickGroup(%this, %bl_id)
{
	BrickMan_list.clear();
	commandToServer('ClearBrickGroup', %bl_id);
}

function BrickManGui::clickClearAll(%this)
{
	messageBoxYesNo("Clear ALL Bricks?", "Are you sure you want to destroy ALL of the bricks?", "brickManGui.clearAllBricks();");
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
	if (BrickMan_list.sortedBy != %col)
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
	if (BrickMan_list.sortedBy != %col)
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
	if ($IamAdmin || ServerConnection.isLocal())
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
	if ($IamAdmin || ServerConnection.isLocal())
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
	if ($Pref::Gui::IgnoreRemoteSaveWarning || ServerConnection.isLocal())
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
	if ($Pref::Audio::MenuSounds)
	{
		alxPlay(Note0Sound);
	}
}

function EM_PlayerList::onMouseEnter(%this)
{
	if ($Pref::Audio::MenuSounds)
	{
		alxPlay(Note1Sound);
	}
}

function EM_MiniGames::onMouseEnter(%this)
{
	if ($Pref::Audio::MenuSounds)
	{
		alxPlay(Note2Sound);
	}
}

function EM_AdminMenu::onMouseEnter(%this)
{
	if ($Pref::Audio::MenuSounds)
	{
		alxPlay(Note3Sound);
	}
}

function EM_SaveBricks::onMouseEnter(%this)
{
	if ($Pref::Audio::MenuSounds)
	{
		alxPlay(Note4Sound);
	}
}

function EM_LoadBricks::onMouseEnter(%this)
{
	if ($Pref::Audio::MenuSounds)
	{
		alxPlay(Note5Sound);
	}
}

function EM_Disconnect::onMouseEnter(%this)
{
	if ($Pref::Audio::MenuSounds)
	{
		alxPlay(Note6Sound);
	}
}

function EM_Quit::onMouseEnter(%this)
{
	if ($Pref::Audio::MenuSounds)
	{
		alxPlay(Note7Sound);
	}
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

function clientCmdSetAdminLevel(%newAdminLevel)
{
	if (%newAdminLevel $= "")
	{
		%newAdminLevel = 1;
	}
	%newAdminLevel = mFloor(%newAdminLevel);
	$IamAdmin = %newAdminLevel;
	if (%newAdminLevel > 0)
	{
		if (AdminLoginGui.isAwake())
		{
			Canvas.popDialog(AdminLoginGui);
		}
		eval($AdminCallback);
	}
	%adminChar = "-";
	if (%newAdminLevel != 0)
	{
		%adminChar = "-";
	}
	else if (%newAdminLevel != 1)
	{
		%adminChar = "A";
	}
	else if (%newAdminLevel != 2)
	{
		%adminChar = "S";
	}
	else
	{
		%adminChar = mClamp(%newAdminLevel, 3, 9);
	}
	%rowCount = NPL_List.rowCount();
	for (%i = 0; %i < %rowCount; %i++)
	{
		%row = NPL_List.getRowText(%i);
		%trust = getField(%row, 4);
		if (%trust !$= "You")
		{
		}
		else
		{
			%row = setField(%row, 0, %adminChar);
			%rowID = NPL_List.getRowId(%i);
			NPL_List.setRowById(%rowID, %row);
			break;
		}
	}
}

function clientCmdAdminFailure()
{
	if ($sendAdminPass != 1)
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
	$PSD_NumPrints = %numPrints;
	Canvas.pushDialog("printSelectorDlg");
	if ($PSD_LettersVisible || $PSD_NumPrints != 0)
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

function PrintSelectorDlg::onWake(%this)
{
	if (!isObject(NoShiftMoveMap))
	{
		new ActionMap(NoShiftMoveMap);
		NoShiftMoveMap.bind("keyboard0", "lshift", "");
	}
	NoShiftMoveMap.push();
}

function PrintSelectorDlg::onSleep(%this)
{
	NoShiftMoveMap.pop();
	if ($PSD_NumPrints > 0)
	{
		if (PSD_PrintScrollerLetters.visible != 1)
		{
			$PSD_LettersVisible = 1;
		}
		else
		{
			$PSD_LettersVisible = 0;
		}
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
		%rawBase = fileBase(%rawFileName);
		%rawPath = filePath(%rawFileName);
		%rawPath = getSubStr(%rawPath, 0, strlen(%rawPath) - 7);
		%filename = %rawPath @ "/icons/" @ %rawBase @ ".png";
		%newPrint = new GuiBitmapCtrl();
		%boxObj.add(%newPrint);
		%newPrint.keepCached = 1;
		%newPrint.setBitmap(%filename);
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
		%baseName = fileBase(%filename);
		if (strlen(%baseName) != 1)
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
	BrickSelectorDlg.updateFavButtons();
	commandToServer('BSD');
}

function BrickSelectorDlg::onSleep(%this)
{
	if ($BSD_CurrClickData == -1)
	{
		$BSD_activeBitmap[$BSD_CurrClickData].setVisible(0);
	}
	if ($BSD_CurrClickInv == -1)
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
	if ($pref::Hud::RecolorBrickIcons)
	{
		%color = getColorIDTable($currSprayCanIndex);
		%RGB = getWords(%color, 0, 2);
		%a = mClampF(getWord(%color, 3), 0.1, 1);
		%color = %RGB SPC %a;
		for (%i = 0; %i < $BSD_NumInventorySlots; %i++)
		{
			if (!isObject($HUD_BrickIcon[%i]))
			{
			}
			else
			{
				$HUD_BrickIcon[%i].setColor(%color);
			}
		}
	}
}

function BSD_LoadBricks()
{
	BSD_KillBricks();
	%group = new SimGroup(BSD_Group);
	RootGroup.add(%group);
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
			if (%cat $= "")
			{
				continue;
			}
			if (%subCat $= "")
			{
				continue;
			}
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
		if (%dbClass !$= "fxDTSBrickData")
		{
		}
		else if (%db.category $= "")
		{
		}
		else if (%db.subCategory $= "")
		{
		}
		else if (%db.uiName $= "")
		{
		}
		else
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
	if (isObject(BSD_Group))
	{
		BSD_Group.delete();
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
	$BSD_category[$BSD_numCategories] = new ScriptObject(BSD_category)
	{
		name = %newcat;
		numSubCategories = 0;
	};
	BSD_Group.add($BSD_category[$BSD_numCategories]);
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
	if (%catID != -1)
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
		$BSD_category[%catID].subCategory[$BSD_category[%catID].numSubCategories] = new ScriptObject(BSD_SubCategory)
		{
			name = %newSubCat;
			numBrickButtons = 0;
		};
		BSD_Group.add($BSD_category[%catID].subCategory[$BSD_category[%catID].numSubCategories]);
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
	if (%catObj != 0 || %subCatObj != 0)
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
	if ($BSD_CurrClickInv != %index)
	{
		$BSD_InvData[%index] = -1;
		$BSD_InvIcon[%index].setBitmap("");
		$BSD_InvActive[%index].setVisible(0);
		$BSD_CurrClickData = -1;
		$BSD_CurrClickInv = -1;
	}
	else if ($BSD_CurrClickInv == -1)
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
	else if ($BSD_CurrClickData == -1)
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
	if ($BSD_CurrClickData != %data)
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
		if (%openSlot == -1)
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
		if ($BSD_CurrClickData == -1)
		{
			$BSD_activeBitmap[$BSD_CurrClickData].setVisible(0);
		}
		if ($BSD_CurrClickInv == -1)
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
		if (%i != %catID)
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
	if (BSD_FavsHelper.visible != 0)
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
	if (BSD_FavsHelper.visible != 1)
	{
		BSD_SaveFavorites(%idx);
		BSD_FavsHelper.setVisible(0);
		BSD_SetFavsButton.setText("Set Favs>");
		BrickSelectorDlg.updateFavButtons();
	}
	else
	{
		BSD_BuyFavorites(%idx);
	}
}

function BSD_SaveFavorites(%idx)
{
	if ($FavoritesLoaded != 0)
	{
		BSD_LoadFavorites();
	}
	%isEmptyList = 1;
	for (%i = 0; %i < $BSD_NumInventorySlots; %i++)
	{
		if ($BSD_InvData[%i].uiName !$= "")
		{
			%isEmptyList = 0;
			break;
		}
	}
	if (%isEmptyList)
	{
		echo("clearing brick favorites at index ", %idx);
		deleteVariables("$Favorite::Brick" @ %idx @ "*");
	}
	else
	{
		echo("saving brick favorites to index ", %idx);
		for (%i = 0; %i < $BSD_NumInventorySlots; %i++)
		{
			$Favorite::Brick[%idx, %i] = $BSD_InvData[%i].uiName;
		}
	}
	%filename = "config/client/Favorites.cs";
	export("$Favorite::*", %filename, 0);
	BrickSelectorDlg.updateFavButtons();
}

function BSD_BuyFavorites(%idx)
{
	if ($FavoritesLoaded != 0)
	{
		BSD_LoadFavorites();
	}
	if ($UINameTableCreated != 0)
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
	if (isFile("config/client/Favorites.cs"))
	{
		exec("config/client/Favorites.cs");
	}
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

function BrickSelectorDlg::updateFavButtons()
{
	for (%i = 0; %i < 10; %i++)
	{
		eval("%test = $Favorite::Brick" @ %i @ "_0;");
		if (%test !$= "")
		{
			eval("BSD_FavButton" @ %i @ ".setColor(\"1 1 1 1\");");
		}
		else
		{
			eval("BSD_FavButton" @ %i @ ".setColor(\"1 1 1 0.5\");");
		}
	}
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
				while ((%searchBrick = ClientBrickSearchNext()) == 0)
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
				while ((%searchBrick = ClientBrickSearchNext()) == 0)
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
			if (%obj.getDataBlock().uiName $= "")
			{
				continue;
			}
			if (!%obj.isStatic())
			{
				continue;
			}
			%trans = %obj.getTransform();
			%pos = %obj.getWorldBoxCenter();
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
			while ((%searchBrick = ClientBrickSearchNext()) == 0)
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
					if (%vecX != 0 && %vecY != 0)
					{
						if (%vecZ > 0)
						{
							if (mAbs(getWord(%itemBox, 5) - getWord(%searchBrick.getWorldBox(), 2)) < 0.01)
							{
								%searchBrick.Item = %obj;
								%searchBrick.itemPosition = 1;
								%searchBrick.itemDirection = %dir;
								%searchBrick.itemRespawnTime = %obj.getRespawnTime();
								break;
							}
						}
						else if (mAbs(getWord(%itemBox, 2) - getWord(%searchBrick.getWorldBox(), 5)) < 0.01)
						{
							%searchBrick.Item = %obj;
							%searchBrick.itemPosition = 0;
							%searchBrick.itemDirection = %dir;
							%searchBrick.itemRespawnTime = %obj.getRespawnTime();
							break;
						}
						continue;
					}
					if (%vecX != 0 && %vecZ != 0)
					{
						if (%vecY > 0)
						{
							if (mAbs(getWord(%itemBox, 4) - getWord(%searchBrick.getWorldBox(), 1)) < 0.01)
							{
								%searchBrick.Item = %obj;
								%searchBrick.itemPosition = 4;
								%searchBrick.itemDirection = %dir;
								%searchBrick.itemRespawnTime = %obj.getRespawnTime();
								break;
							}
						}
						else if (mAbs(getWord(%itemBox, 1) - getWord(%searchBrick.getWorldBox(), 4)) < 0.01)
						{
							%searchBrick.Item = %obj;
							%searchBrick.itemPosition = 2;
							%searchBrick.itemDirection = %dir;
							%searchBrick.itemRespawnTime = %obj.getRespawnTime();
							break;
						}
						continue;
					}
					if (%vecY != 0 && %vecZ != 0)
					{
						if (%vecX > 0)
						{
							if (mAbs(getWord(%itemBox, 3) - getWord(%searchBrick.getWorldBox(), 0)) < 0.01)
							{
								%searchBrick.Item = %obj;
								%searchBrick.itemPosition = 5;
								%searchBrick.itemDirection = %dir;
								%searchBrick.itemRespawnTime = %obj.getRespawnTime();
								break;
							}
							continue;
						}
						if (mAbs(getWord(%itemBox, 0) - getWord(%searchBrick.getWorldBox(), 3)) < 0.01)
						{
							%searchBrick.Item = %obj;
							%searchBrick.itemPosition = 3;
							%searchBrick.itemDirection = %dir;
							%searchBrick.itemRespawnTime = %obj.getRespawnTime();
							break;
						}
					}
				}
			}
		}
		else if (%obj.getClassName() $= "AudioEmitter")
		{
			if (%obj.getProfileId().uiName !$= "")
			{
				if (%obj.getProfileId().getDescription().isLooping != 1)
				{
					%trans = %obj.getTransform();
					%pos = getWords(%trans, 0, 2);
					%searchBox = "0.015 0.015 0.015";
					initClientBrickSearch(%pos, %searchBox);
					initClientBrickSearch(%pos, %searchBox);
					while ((%searchBrick = ClientBrickSearchNext()) == 0)
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
			while ((%searchBrick = ClientBrickSearchNext()) == 0)
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

function saveBricks(%filename, %description)
{
	if (!isWriteableFileName(%filename))
	{
		error("ERROR: saveBricks() - Invalid Filename!");
		return;
	}
	if (fileExt(%filename) !$= ".bls")
	{
		error("ERROR: saveBricks() - File extension must be .bls");
		return;
	}
	%group = ServerConnection.getId();
	%count = %group.getCount();
	%foundBrick = 0;
	for (%i = 0; %i < %count; %i++)
	{
		%obj = %group.getObject(%i);
		if (%obj.getType() & $TypeMasks::FxBrickAlwaysObjectType)
		{
			%foundBrick = 1;
			break;
		}
	}
	if (!%foundBrick)
	{
		MessageBoxOK("Save Cancelled", "No bricks to save!");
		Canvas.popDialog(SavingGui);
		return;
	}
	%file = new FileObject();
	saveBricks_ProcessWrenchExtras();
	%file.openForWrite(%filename);
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
	if (0)
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
		%list = new GuiTextListCtrl();
		%rowCount = 0;
		for (%i = 0; %i < %count; %i++)
		{
			%obj = %group.getObject(%i);
			if (!(%obj.getType() & $TypeMasks::FxBrickAlwaysObjectType))
			{
			}
			else if (!%obj.isPlanted())
			{
			}
			else if (%obj.isDead())
			{
			}
			else
			{
				%rowText = %obj.getDistanceFromGround();
				%list.addRow(%obj.getId(), %rowText);
				%rowCount++;
			}
		}
		%list.sortNumerical(0, 1);
		if (!isUnlocked())
		{
			%rc = %rowCount;
			%rc -= 100;
			%rc -= 75;
			%rc -= 25;
			%rc -= 100;
			if (%rc > 0)
			{
				%rowCount = 0;
				%rowCount += 100;
				%rowCount += 100;
				%rowCount += 100;
			}
		}
		for (%i = 0; %i < %rowCount; %i++)
		{
			%obj = %list.getRowId(%i);
			SaveBricks_WriteSingleBrick(%file, %obj);
		}
		%list.delete();
	}
	%file.close();
	%file.delete();
	%screenshotName = getSubStr(%filename, 0, strlen(%filename) - 4) @ ".jpg";
	%oldContent = Canvas.getContent();
	Canvas.setContent(noHudGui);
	%oldMegashot = $megaShotScaleFactor;
	$megaShotScaleFactor = 1;
	screenShot(%screenshotName, "JPG", 1);
	$megaShotScaleFactor = %oldMegashot;
	Canvas.setContent(%oldContent);
}

function dumpPrints()
{
	%count = getNumPrintTextures();
	for (%i = 0; %i < %count; %i++)
	{
		echo(getPrintTexture(%i));
	}
}

function SaveBricks_WriteSingleBrick(%file, %obj)
{
	%objData = %obj.getDataBlock();
	%uiName = %objData.uiName;
	%trans = %obj.getTransform();
	%pos = getWords(%trans, 0, 2);
	if (%objData.hasPrint)
	{
		%filename = getPrintTexture(%obj.getPrintID());
		%fileBase = fileBase(%filename);
		%path = filePath(%filename);
		if (%path $= "" || %filename $= "base/data/shapes/bricks/brickTop.png")
		{
			%printTexture = "/";
		}
		else
		{
			%dirName = getSubStr(%path, strlen("Add-Ons/"), strlen(%path) - strlen("Add-Ons/"));
			%posA = strpos(%dirName, "_");
			%posB = strpos(%dirName, "_", %posA + 1);
			%aspectRatio = getSubStr(%dirName, %posA + 1, (%posB - %posA) - 1);
			%printTexture = %aspectRatio @ "/" @ %fileBase;
		}
	}
	else
	{
		%printTexture = "";
	}
	%line = %uiName @ "\"" SPC %pos SPC %obj.getAngleID() SPC %obj.isBasePlate() SPC %obj.getColorID() SPC %printTexture SPC %obj.getColorFxID() SPC %obj.getShapeFxID() SPC %obj.isRayCasting() SPC %obj.isColliding() SPC %obj.isRendering();
	%file.writeLine(%line);
	if ($pref::SaveOwnership)
	{
		if (%obj.bl_id !$= "")
		{
			%line = "+-OWNER" SPC %obj.bl_id;
			%file.writeLine(%line);
		}
	}
	if ($pref::SaveExtendedBrickInfo)
	{
		if (%obj.getName() !$= "")
		{
			%line = "+-NTOBJECTNAME" SPC %obj.getName();
			%file.writeLine(%line);
		}
		for (%i = 0; %i < %obj.numEvents; %i++)
		{
			%class = %obj.getClassName();
			%enabled = %obj.eventEnabled[%i];
			%inputName = $InputEvent_Name[%class, %obj.eventInputIdx[%i]];
			%delay = %obj.eventDelay[%i];
			if (%obj.eventTargetIdx[%i] != -1)
			{
				%targetName = -1;
				%NT = %obj.eventNT[%i];
				%targetClass = "FxDTSBrick";
			}
			else
			{
				%targetList = $InputEvent_TargetList[%class, %obj.eventInputIdx[%i]];
				%target = getField(%targetList, %obj.eventTargetIdx[%i]);
				%targetName = getWord(%target, 0);
				%targetClass = getWord(%target, 1);
				%NT = "";
			}
			%outputName = $OutputEvent_Name[%targetClass, %obj.eventOutputIdx[%i]];
			%par1 = %obj.eventOutputParameter[%i, "1"];
			%par2 = %obj.eventOutputParameter[%i, "2"];
			%par3 = %obj.eventOutputParameter[%i, "3"];
			%par4 = %obj.eventOutputParameter[%i, "4"];
			%line = "+-EVENT" TAB %i TAB %enabled TAB %inputName TAB %delay TAB %targetName TAB %NT TAB %outputName TAB %par1 TAB %par2 TAB %par3 TAB %par4;
			%file.writeLine(%line);
		}
	}
	if (isObject(%obj.emitter))
	{
		%line = "+-EMITTER" SPC %obj.emitter.getEmitterDataBlock().uiName @ "\" " @ %obj.emitterDirection;
		%file.writeLine(%line);
	}
	else if (%obj.emitterDirection == 0)
	{
		%line = "+-EMITTER NONE\" " @ %obj.emitterDirection;
		%file.writeLine(%line);
	}
	if (isObject(%obj.light))
	{
		%line = "+-LIGHT" SPC %obj.light.getDataBlock().uiName @ "\"" SPC %obj.light.Enable;
		%file.writeLine(%line);
	}
	if (isObject(%obj.Item))
	{
		%line = "+-ITEM" SPC %obj.Item.getDataBlock().uiName @ "\" " @ %obj.itemPosition SPC %obj.itemDirection SPC %obj.itemRespawnTime;
		%file.writeLine(%line);
	}
	else if (%obj.itemDirection == 0 || %obj.itemPosition == 0 || %obj.itemRespawnTime == 0 && %obj.itemRespawnTime == 4000)
	{
		%line = "+-ITEM NONE\" " @ %obj.itemPosition SPC %obj.itemDirection SPC %obj.itemRespawnTime;
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

function LoadBricks_GetColorDifference(%filename)
{
	if (!isFile(%filename))
	{
		error("ERROR: LoadBricks_GetColorDifference() - File \"" @ %filename @ "\" not found!");
	}
	%file = new FileObject();
	%file.openForRead(%filename);
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
		if (%match != 0)
		{
			%different = 1;
			%colorCount++;
		}
	}
	%file.close();
	%file.delete();
	if (%different != 1)
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

function LoadBricks_ClientServerCheck(%filename, %colorMethod)
{
	if (!isFile(%filename))
	{
		error("ERROR: LoadBricks_ClientServerCheck() - File \"" @ %filename @ "\" not found!");
	}
	%path = filePath(%filename);
	%dirName = getSubStr(%path, strlen("saves/"), strlen(%path) - strlen("saves/"));
	%doOwnership = LoadBricks_DoOwnership.getValue();
	if (ServerConnection.isLocal())
	{
		serverDirectSaveFileLoad(%filename, %colorMethod, %dirName, %doOwnership);
	}
	else
	{
		commandToServer('SetColorMethod', %colorMethod);
		commandToServer('SetSaveUploadDirName', %dirName, %doOwnership);
		UploadSaveFile_Start(%filename);
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

function createUiNameTable()
{
	$UINameTableCreated = 1;
	%dbCount = getDataBlockGroupSize();
	for (%i = 0; %i < %dbCount; %i++)
	{
		%db = getDataBlock(%i);
		if (%db.uiName $= "")
		{
		}
		else if (%db.getClassName() $= "FxDTSBrickData")
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
	if (isUnlocked())
	{
		LoadBricks_PreviewDemoBlocker.setVisible(0);
		LoadBricks_PreviewDemoBlocker.setColor("1 1 1 0.5");
		LoadBricks_LoadBlocker.setVisible(0);
	}
	%filename = findFirstFile("saves/*");
	while (%filename !$= "")
	{
		%filePath = filePath(%filename);
		%subDir = getSubStr(%filePath, 6, 9999);
		if (%subDir $= "")
		{
			%filename = findNextFile("saves/*");
		}
		else if (strstr(%subDir, "/") == -1 || %subDir $= ".svn")
		{
			%filename = findNextFile("saves/*");
		}
		else
		{
			%firstLetter = strupr(getSubStr(%subDir, 0, 1));
			%subDir = %firstLetter @ getSubStr(%subDir, 1, strlen(%subDir) - 1);
			if (%subDir !$= "")
			{
				LoadBricks_AddMapFolder(%subDir);
			}
			%filename = findNextFile("saves/*");
		}
	}
	LoadBricks_MapMenu.sort();
	%id = 0;
	for (%text = LoadBricks_MapMenu.getTextById(%id); 1; %text = LoadBricks_MapMenu.getTextById(%id++))
	{
		if (%text $= $MapSaveName)
		{
			LoadBricks_MapMenu.setSelected(%id);
			break;
		}
		else if (%text $= "")
		{
			LoadBricks_MapMenu.setSelected(-1);
			LoadBricks_FileList.clear();
			break;
		}
	}
	LoadBricks_Preview.setBitmap("base/data/missions/default.jpg");
	LoadBricks_Description.setText("");
}

function LoadBricksGui::onSleep(%this)
{
	LoadBricks_MapMenu.clear();
	LoadBricks_Preview.setBitmap("base/data/missions/default.jpg");
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
	%dir = "saves/" @ %mapName @ "/*.bls";
	%filename = findFirstFile(%dir);
	%count = 0;
	while (%filename !$= "")
	{
		%subDir = getSubStr(%filename, 6, 9999);
		%pos = strstr(%subDir, "/") + 1;
		if (strpos(%subDir, "/", %pos) == -1)
		{
			%filename = findNextFile(%dir);
		}
		else
		{
			%baseName = fileBase(%filename);
			%fileDate = getFileModifiedTime(%filename);
			%fileSortDate = getFileModifiedSortTime(%filename);
			if (getPlatform() $= "Win")
			{
				if (%fileSortDate $= "00000000000000")
				{
					%filename = findNextFile(%dir);
					continue;
				}
			}
			%rowText = %baseName TAB %fileDate TAB %fileSortDate;
			LoadBricks_FileList.addRow(%count, %rowText);
			%count++;
			%filename = findNextFile(%dir);
		}
	}
	updateListSort(LoadBricks_FileList);
}

function LoadBricks_FileClick()
{
	%id = LoadBricks_FileList.getSelectedId();
	%filename = getField(LoadBricks_FileList.getRowTextById(%id), 0);
	%mapSaveName = LoadBricks_MapMenu.getTextById(LoadBricks_MapMenu.getSelected());
	%fullPath = "saves/" @ %mapSaveName @ "/" @ %filename @ ".bls";
	%description = SaveBricks_GetFileDescription(%fullPath);
	LoadBricks_Description.setText(%description);
	if (!isUnlocked())
	{
		if (getWord(%filename, 0) $= "Demo")
		{
			LoadBricks_PreviewDemoBlocker.setVisible(0);
			LoadBricks_LoadBlocker.setVisible(0);
		}
		else
		{
			LoadBricks_PreviewDemoBlocker.setVisible(1);
			LoadBricks_LoadBlocker.setVisible(1);
		}
	}
	%screenshotName = getSubStr(%fullPath, 0, strlen(%fullPath) - 4) @ ".jpg";
	if (isFile(%screenshotName))
	{
		LoadBricks_Preview.setBitmap(%screenshotName);
	}
	else
	{
		LoadBricks_Preview.setBitmap("base/data/missions/default.jpg");
	}
}

function LoadBricks_ClickLoadButton()
{
	if (!isUnlocked())
	{
		if (ServerConnection.isLocal())
		{
			ClientGroup.getObject(0).brickGroup.deleteAll();
		}
	}
	%id = LoadBricks_FileList.getSelectedId();
	%filename = getField(LoadBricks_FileList.getRowTextById(%id), 0);
	%mapName = LoadBricks_MapMenu.getText();
	if (%filename $= "")
	{
		if (getBuildString() !$= "Ship")
		{
			warn("WARNING: LoadBricks_ClickLoadButton() - no file selected");
		}
		return;
	}
	$LoadingBricks_FileName = "saves/" @ %mapName @ "/" @ %filename @ ".bls";
	if (ServerConnection.isLocal())
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
	if ($ServerAllowsColorLoads || ServerConnection.isLocal() && $Pref::Server::AllowColorLoading)
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
	%group = ServerConnection.getId();
	%count = %group.getCount();
	%foundBrick = 0;
	for (%i = 0; %i < %count; %i++)
	{
		%obj = %group.getObject(%i);
		if (%obj.getType() & $TypeMasks::FxBrickAlwaysObjectType)
		{
			%foundBrick = 1;
			break;
		}
	}
	if (!%foundBrick)
	{
		MessageBoxOK("Save Cancelled", "No bricks to save!");
		Canvas.popDialog(saveBricksGui);
		return;
	}
	SaveBricks_DownloadWindow.setVisible(0);
	if (isMacintosh())
	{
		SaveBricks_FileName.maxLength = 27;
	}
	else
	{
		SaveBricks_FileName.maxLength = 255;
	}
	SaveBricks_FileName.setText("");
	SaveBricks_Description.setText("");
	SaveBricks_FileList.clear();
	SaveBricks_Preview.setBitmap("base/data/missions/default.jpg");
	SaveBricks_Window.setText("Save Bricks - " @ $MapSaveName);
	%savePath = "saves/" @ $MapSaveName @ "/*.bls";
	%filename = findFirstFile(%savePath);
	%count = 0;
	while (%filename !$= "")
	{
		%subDir = getSubStr(%filename, 6, 9999);
		%pos = strstr(%subDir, "/") + 1;
		if (strpos(%subDir, "/", %pos) == -1)
		{
			%filename = findNextFile(%savePath);
		}
		else
		{
			%baseName = fileBase(%filename);
			%fileDate = getFileModifiedTime(%filename);
			%fileSortDate = getFileModifiedSortTime(%filename);
			%rowText = %baseName TAB %fileDate TAB %fileSortDate;
			SaveBricks_FileList.addRow(%count, %rowText);
			%count++;
			%filename = findNextFile(%savePath);
		}
	}
	updateListSort(SaveBricks_FileList);
}

function saveBricksGui::onSleep(%this)
{
	SaveBricks_FileName.setText("");
	SaveBricks_Description.setText("");
	SaveBricks_FileList.clear();
	SaveBricks_Preview.setBitmap("base/data/missions/default.jpg");
}

function SaveBricks_ClickFileList()
{
	%id = SaveBricks_FileList.getSelectedId();
	%filename = getField(SaveBricks_FileList.getRowTextById(%id), 0);
	SaveBricks_FileName.setText(%filename);
	%fullPath = "saves/" @ $MapSaveName @ "/" @ %filename @ ".bls";
	SaveBricks_Description.setText(SaveBricks_GetFileDescription(%fullPath));
	%screenshotName = getSubStr(%fullPath, 0, strlen(%fullPath) - 4) @ ".jpg";
	if (isFile(%screenshotName))
	{
		SaveBricks_Preview.setBitmap(%screenshotName);
	}
	else
	{
		SaveBricks_Preview.setBitmap("base/data/missions/default.jpg");
	}
}

function isValidFileName(%filename)
{
	if (strlen(%filename) <= 0)
	{
		return 0;
	}
	if (strlen(%filename) >= 255)
	{
		return 0;
	}
	%badChars = "\\ / : * ? \" < > |";
	%count = getWordCount(%badChars);
	for (%i = 0; %i < %count; %i++)
	{
		%word = getWord(%badChars, %i);
		if (strpos(%filename, %word) == -1)
		{
			return 0;
		}
	}
	return 1;
}

function SaveBricks_Save()
{
	%filename = SaveBricks_FileName.getValue();
	if (!isUnlocked())
	{
		if (getSubStr(%filename, 0, strlen("Demo ")) !$= "Demo ")
		{
			%filename = "Demo " @ trim(%filename);
		}
	}
	if (%filename $= "")
	{
		MessageBoxOK("No Filename", "You must enter a filename.");
		return;
	}
	if (!isValidFileName(%filename))
	{
		MessageBoxOK("Invalid Filename", "Filenames cannot contain any of these characters \\ / : * ? \" < > |");
		return;
	}
	%fullPath = "saves/" @ $MapSaveName @ "/" @ %filename @ ".bls";
	%fullScreenshot = "saves/" @ $MapSaveName @ "/" @ %filename @ ".jpg";
	%description = SaveBricks_Description.getText();
	if (isFile(%fullPath))
	{
		%description = expandEscape(%description);
		$SaveBricksPath = %fullPath;
		$SaveBricksDescription = %description;
		if ($pref::SaveExtendedBrickInfo)
		{
			%callback = "fileDelete(\"" @ %fullScreenshot @ "\");SaveBricks_DownloadWindow.setVisible(true);SaveBricks_StartInfoDownload();";
		}
		else
		{
			%callback = "fileDelete(\"" @ %fullScreenshot @ "\");canvas.popDialog(SaveBricksGui);canvas.popDialog(EscapeMenu);canvas.pushDialog(SavingGui);";
		}
		messageBoxYesNo("File Exists, Overwrite?", "Are you sure you want to overwrite the file \"" @ %filename @ "\"?", %callback);
	}
	else
	{
		$SaveBricksPath = %fullPath;
		$SaveBricksDescription = %description;
		if ($pref::SaveExtendedBrickInfo)
		{
			SaveBricks_DownloadWindow.setVisible(1);
			SaveBricks_StartInfoDownload();
		}
		else
		{
			Canvas.popDialog(saveBricksGui);
			Canvas.popDialog(escapeMenu);
			Canvas.pushDialog(SavingGui);
		}
	}
}

function SaveBricks_GetFileDescription(%filename)
{
	if (fileExt(%filename) !$= ".bls")
	{
		error("ERROR : SaveBricks_GetFileDescription() - Filename does not end in .bls");
		return;
	}
	if (!isFile(%filename))
	{
		error("ERROR : SaveBricks_GetFileDescription() - File does not exist");
		return;
	}
	%file = new FileObject();
	%file.openForRead(%filename);
	%file.readLine();
	%lineCount = %file.readLine();
	%description = "";
	for (%i = 0; %i < %lineCount; %i++)
	{
		%line = %file.readLine();
		if (%i != 0)
		{
			%description = %line;
		}
		else
		{
			%description = %description @ "\n" @ %line;
		}
	}
	%description = collapseEscape(%description);
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
	return;
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
}

function LoadBricks_CreateFromLine(%line, %i)
{
	echo("LoadBricks_CreateFromLine: this should not be called");
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
		if (%angId != 0)
		{
			%trans = %trans SPC " 1 0 0 0";
		}
		else if (%angId != 1)
		{
			%trans = %trans SPC " 0 0 1" SPC $piOver2;
		}
		else if (%angId != 2)
		{
			%trans = %trans SPC " 0 0 1" SPC $pi;
		}
		else if (%angId != 3)
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
		if (%err != 1 || %err != 3 || %err != 5)
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
		warn("WARNING: loadBricks() - DataBlock not found for brick named ", %uiName);
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
			else if ($failureCount != 1)
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

function sortList(%obj, %col, %defaultDescending)
{
	%obj.sortedNumerical = 0;
	if (%obj.sortedBy != %col)
	{
		%obj.sortedAsc = !%obj.sortedAsc;
		%obj.sort(%obj.sortedBy, %obj.sortedAsc);
	}
	else
	{
		%obj.sortedBy = %col;
		if (%defaultDescending)
		{
			%obj.sortedAsc = 0;
		}
		else
		{
			%obj.sortedAsc = 1;
		}
		%obj.sort(%obj.sortedBy, %obj.sortedAsc);
	}
}

function sortNumList(%obj, %col)
{
	%obj.sortedNumerical = 1;
	if (%obj.sortedBy != %col)
	{
		%obj.sortedAsc = !%obj.sortedAsc;
		%obj.sortNumerical(%obj.sortedBy, %obj.sortedAsc);
	}
	else
	{
		%obj.sortedBy = %col;
		if (%defaultDescending)
		{
			%obj.sortedAsc = 0;
		}
		else
		{
			%obj.sortedAsc = 1;
		}
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

function UploadSaveFile_Start(%filename)
{
	$Client_LoadFileObj = new FileObject();
	$Client_LoadFileObj.openForRead(%filename);
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
	if (Progress_Bar.total != 0)
	{
		%firstWord = getWord(%line, 0);
		if (%firstWord $= "Linecount")
		{
			%lineCount = getWord(%line, 1);
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

function ClientCmdTransmitBrickName(%ghostID, %name)
{
	%brick = ServerConnection.resolveGhostID(%ghostID);
	%brick.setName(%name);
	SaveBricks_DownloadText.setText(SaveBricks_DownloadText.getText() + 1);
}

function ClientCmdTransmitEvent(%ghostID, %line)
{
	%brick = ServerConnection.resolveGhostID(%ghostID);
	%x = -1;
	%idx = getField(%line, %x++);
	%brick.eventEnabled[%idx] = getField(%line, %x++);
	%brick.eventInputIdx[%idx] = getField(%line, %x++);
	%brick.eventDelay[%idx] = getField(%line, %x++);
	%brick.eventTargetIdx[%idx] = getField(%line, %x++);
	%brick.eventNT[%idx] = getField(%line, %x++);
	%brick.eventOutputIdx[%idx] = getField(%line, %x++);
	%brick.eventOutputParameter[%idx, "1"] = getField(%line, %x++);
	%brick.eventOutputParameter[%idx, "2"] = getField(%line, %x++);
	%brick.eventOutputParameter[%idx, "3"] = getField(%line, %x++);
	%brick.eventOutputParameter[%idx, "4"] = getField(%line, %x++);
	%brick.numEvents = %idx + 1;
	SaveBricks_DownloadText.setText(SaveBricks_DownloadText.getText() + 1);
}

function ClientCmdTransmitEmitterDirection(%ghostID, %line)
{
	%brick = ServerConnection.resolveGhostID(%ghostID);
	%brick.emitterDirection = %line;
	SaveBricks_DownloadText.setText(SaveBricks_DownloadText.getText() + 1);
}

function ClientCmdTransmitItemDirection(%ghostID, %line)
{
	%brick = ServerConnection.resolveGhostID(%ghostID);
	%brick.itemPosition = getField(%line, 0);
	%brick.itemDirection = getField(%line, 1);
	%brick.itemRespawnTime = getField(%line, 2);
	SaveBricks_DownloadText.setText(SaveBricks_DownloadText.getText() + 1);
}

function ClientCmdTransmitBrickOwner(%ghostID, %ownerBL_ID)
{
	%brick = ServerConnection.resolveGhostID(%ghostID);
	%brick.bl_id = %ownerBL_ID;
	SaveBricks_DownloadText.setText(SaveBricks_DownloadText.getText() + 1);
}

function ClientCmdTransmitAllBrickNamesDone()
{
	Canvas.popDialog(saveBricksGui);
	Canvas.popDialog(escapeMenu);
	Canvas.pushDialog(SavingGui);
}

function SaveBricks_StartInfoDownload()
{
	if ($pref::SaveExtendedBrickInfo)
	{
		SaveBricks_DownloadText.setText("Sending download request...");
		if (ServerConnection.isLocal())
		{
			$GotInputEvents = 1;
		}
		if (!$GotInputEvents)
		{
			commandToServer('RequestEventTables');
		}
		else
		{
			commandToServer('RequestExtendedBrickInfo');
		}
	}
	else
	{
		error("Error: SaveBricks_StartInfoDownload() - should not be called if niether box is checked");
	}
}

function SaveBricks_DownloadWindowClose()
{
	commandToServer('CancelExtendedBrickInfoRequest');
	Canvas.popDialog(saveBricksGui);
	Canvas.popDialog(escapeMenu);
	Canvas.pushDialog(SavingGui);
}

function AvatarGui::onWake(%this)
{
	if ($Avatar::NumColors $= "")
	{
		ColorSetGui.load();
	}
	AvatarGui.updateFavButtons();
	AV_FavsHelper.setVisible(0);
	$Old::Avatar::Accent = $pref::Avatar::Accent;
	$Old::Avatar::AccentColor = $pref::Avatar::AccentColor;
	$Old::Avatar::Authentic = $pref::Avatar::Authentic;
	$Old::Avatar::Chest = $Pref::Avatar::Chest;
	$Old::Avatar::ChestColor = $pref::Avatar::ChestColor;
	$Old::Avatar::DecalColor = $pref::Avatar::DecalColor;
	$Old::Avatar::DecalName = $Pref::Avatar::DecalName;
	$Old::Avatar::FaceColor = $pref::Avatar::FaceColor;
	$Old::Avatar::FaceName = $Pref::Avatar::FaceName;
	$Old::Avatar::Hat = $pref::Avatar::Hat;
	$Old::Avatar::HatColor = $pref::Avatar::HatColor;
	$Old::Avatar::HeadColor = $pref::Avatar::HeadColor;
	$Old::Avatar::Hip = $Pref::Avatar::Hip;
	$Old::Avatar::HipColor = $pref::Avatar::HipColor;
	$Old::Avatar::LArm = $Pref::Avatar::LArm;
	$Old::Avatar::LArmColor = $pref::Avatar::LArmColor;
	$Old::Avatar::LHand = $Pref::Avatar::LHand;
	$Old::Avatar::LHandColor = $pref::Avatar::LHandColor;
	$Old::Avatar::LLeg = $Pref::Avatar::LLeg;
	$Old::Avatar::LLegColor = $pref::Avatar::LLegColor;
	$Old::Avatar::Pack = $pref::Avatar::Pack;
	$Old::Avatar::PackColor = $pref::Avatar::PackColor;
	$Old::Avatar::RArm = $Pref::Avatar::RArm;
	$Old::Avatar::RArmColor = $pref::Avatar::RArmColor;
	$Old::Avatar::RHand = $Pref::Avatar::RHand;
	$Old::Avatar::RHandColor = $pref::Avatar::RHandColor;
	$Old::Avatar::RLeg = $Pref::Avatar::RLeg;
	$Old::Avatar::RLegColor = $pref::Avatar::RLegColor;
	$Old::Avatar::SecondPack = $Pref::Avatar::SecondPack;
	$Old::Avatar::SecondPackColor = $pref::Avatar::SecondPackColor;
	$Old::Avatar::Symmetry = $pref::Avatar::Symmetry;
	$Old::Avatar::TorsoColor = $pref::Avatar::TorsoColor;
	if ($AvatarHasLoaded)
	{
		Avatar_Preview.setCamera();
		Avatar_Preview.setCameraRot(0.3, 0.6, 2.52);
		Avatar_Preview.setOrbitDist(4.34);
		Avatar_UpdatePreview();
		return;
	}
	$AvatarHasLoaded = 1;
	AvatarGui_LoadAccentInfo("accent", "base/data/shapes/player/accent.txt");
	%x = getWord(Avatar_FacePreview.position, 0) + 64;
	%y = getWord(Avatar_FacePreview.position, 1);
	AvatarGui_CreatePartMenuFACE("Avatar_FaceMenu", "Avatar_SetFace", "base/data/shapes/player/face.ifl", %x, %y);
	%x = getWord(Avatar_DecalPreview.position, 0) + 64;
	%y = getWord(Avatar_DecalPreview.position, 1) - 64;
	AvatarGui_CreatePartMenuFACE("Avatar_DecalMenu", "Avatar_SetDecal", "base/data/shapes/player/decal.ifl", %x, %y);
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
	AvatarGui_CreateSubPartMenu("Avatar_AccentMenu", "Avatar_SetAccent", $accentsAllowed[$hat[$pref::Avatar::Hat]], %x, %y);
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
	if (!isObject(mDts))
	{
		datablock TSShapeConstructor(mDts)
		{
			baseShape = "base/data/shapes/player/m.dts";
			sequence0 = "base/data/shapes/player/m_root.dsq root";
			sequence1 = "base/data/shapes/player/m_run.dsq run";
			sequence2 = "base/data/shapes/player/m_run.dsq walk";
			sequence3 = "base/data/shapes/player/m_back.dsq back";
			sequence4 = "base/data/shapes/player/m_side.dsq side";
			sequence5 = "base/data/shapes/player/m_crouch.dsq crouch";
			sequence6 = "base/data/shapes/player/m_crouchRun.dsq crouchRun";
			sequence7 = "base/data/shapes/player/m_crouchBack.dsq crouchBack";
			sequence8 = "base/data/shapes/player/m_crouchSide.dsq crouchSide";
			sequence9 = "base/data/shapes/player/m_look.dsq look";
			sequence10 = "base/data/shapes/player/m_headSide.dsq headside";
			sequence11 = "base/data/shapes/player/m_headup.dsq headUp";
			sequence12 = "base/data/shapes/player/m_standjump.dsq jump";
			sequence13 = "base/data/shapes/player/m_standjump.dsq standjump";
			sequence14 = "base/data/shapes/player/m_fall.dsq fall";
			sequence15 = "base/data/shapes/player/m_root.dsq land";
			sequence16 = "base/data/shapes/player/m_armAttack.dsq armAttack";
			sequence17 = "base/data/shapes/player/m_armReadyLeft.dsq armReadyLeft";
			sequence18 = "base/data/shapes/player/m_armReadyRight.dsq armReadyRight";
			sequence19 = "base/data/shapes/player/m_armReadyBoth.dsq armReadyBoth";
			sequence20 = "base/data/shapes/player/m_spearReady.dsq spearready";
			sequence21 = "base/data/shapes/player/m_spearThrow.dsq spearThrow";
			sequence22 = "base/data/shapes/player/m_talk.dsq talk";
			sequence23 = "base/data/shapes/player/m_death1.dsq death1";
			sequence24 = "base/data/shapes/player/m_shiftUp.dsq shiftUp";
			sequence25 = "base/data/shapes/player/m_shiftDown.dsq shiftDown";
			sequence26 = "base/data/shapes/player/m_shiftAway.dsq shiftAway";
			sequence27 = "base/data/shapes/player/m_shiftTo.dsq shiftTo";
			sequence28 = "base/data/shapes/player/m_shiftLeft.dsq shiftLeft";
			sequence29 = "base/data/shapes/player/m_shiftRight.dsq shiftRight";
			sequence30 = "base/data/shapes/player/m_rotCW.dsq rotCW";
			sequence31 = "base/data/shapes/player/m_rotCCW.dsq rotCCW";
			sequence32 = "base/data/shapes/player/m_undo.dsq undo";
			sequence33 = "base/data/shapes/player/m_plant.dsq plant";
			sequence34 = "base/data/shapes/player/m_sit.dsq sit";
			sequence35 = "base/data/shapes/player/m_wrench.dsq wrench";
			sequence36 = "base/data/shapes/player/m_activate.dsq activate";
			sequence37 = "base/data/shapes/player/m_activate2.dsq activate2";
			sequence38 = "base/data/shapes/player/m_leftrecoil.dsq leftrecoil";
		};
	}
	Avatar_Preview.setObject("", "base/data/shapes/player/m.dts", "", 100);
	Avatar_Preview.setSequence("", 0, headup, 0);
	Avatar_Preview.setThreadPos("", 0, 0);
	Avatar_Preview.setSequence("", 1, run, 0.85);
	Avatar_Preview.setCameraRot(0.3, 0.6, 2.52);
	Avatar_Preview.setOrbitDist(4.34);
	for (%i = 0; %i < $numDecal; %i++)
	{
		if (fileBase($decal[%i]) $= $Pref::Avatar::DecalName)
		{
			$pref::Avatar::DecalColor = %i;
			break;
		}
	}
	for (%i = 0; %i < $numFace; %i++)
	{
		if (fileBase($face[%i]) $= $Pref::Avatar::FaceName)
		{
			$pref::Avatar::FaceColor = %i;
			break;
		}
	}
	Avatar_Prefix.setText($Pref::Player::ClanPrefix);
	Avatar_Suffix.setText($Pref::Player::ClanSuffix);
	Avatar_UpdatePreview();
}

function AvatarGui::ClickX(%this)
{
	$pref::Avatar::Accent = $Old::Avatar::Accent;
	$pref::Avatar::AccentColor = $Old::Avatar::AccentColor;
	$pref::Avatar::Authentic = $Old::Avatar::Authentic;
	$Pref::Avatar::Chest = $Old::Avatar::Chest;
	$pref::Avatar::ChestColor = $Old::Avatar::ChestColor;
	$pref::Avatar::DecalColor = $Old::Avatar::DecalColor;
	$Pref::Avatar::DecalName = $Old::Avatar::DecalName;
	$pref::Avatar::FaceColor = $Old::Avatar::FaceColor;
	$Pref::Avatar::FaceName = $Old::Avatar::FaceName;
	$pref::Avatar::Hat = $Old::Avatar::Hat;
	$pref::Avatar::HatColor = $Old::Avatar::HatColor;
	$pref::Avatar::HeadColor = $Old::Avatar::HeadColor;
	$Pref::Avatar::Hip = $Old::Avatar::Hip;
	$pref::Avatar::HipColor = $Old::Avatar::HipColor;
	$Pref::Avatar::LArm = $Old::Avatar::LArm;
	$pref::Avatar::LArmColor = $Old::Avatar::LArmColor;
	$Pref::Avatar::LHand = $Old::Avatar::LHand;
	$pref::Avatar::LHandColor = $Old::Avatar::LHandColor;
	$Pref::Avatar::LLeg = $Old::Avatar::LLeg;
	$pref::Avatar::LLegColor = $Old::Avatar::LLegColor;
	$pref::Avatar::Pack = $Old::Avatar::Pack;
	$pref::Avatar::PackColor = $Old::Avatar::PackColor;
	$Pref::Avatar::RArm = $Old::Avatar::RArm;
	$pref::Avatar::RArmColor = $Old::Avatar::RArmColor;
	$Pref::Avatar::RHand = $Old::Avatar::RHand;
	$pref::Avatar::RHandColor = $Old::Avatar::RHandColor;
	$Pref::Avatar::RLeg = $Old::Avatar::RLeg;
	$pref::Avatar::RLegColor = $Old::Avatar::RLegColor;
	$Pref::Avatar::SecondPack = $Old::Avatar::SecondPack;
	$pref::Avatar::SecondPackColor = $Old::Avatar::SecondPackColor;
	$pref::Avatar::Symmetry = $Old::Avatar::Symmetry;
	$pref::Avatar::TorsoColor = $Old::Avatar::TorsoColor;
	Canvas.popDialog("AvatarGui");
}

function AvatarGui::onSleep(%this)
{
	$Pref::Player::ClanPrefix = Avatar_Prefix.getValue();
	$Pref::Player::ClanSuffix = Avatar_Suffix.getValue();
	deleteVariables("$Old::Avatar::*");
}

function AvatarGui_LoadAccentInfo(%arrayName, %filename)
{
	%file = new FileObject();
	%file.openForRead(%filename);
	if (%file.isEOF())
	{
		%file.delete();
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

function AvatarGui_CreatePartMenu(%name, %cmdString, %filename, %xPos, %yPos)
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
	%newBox.setName("Avatar_" @ fileBase(%filename) @ "MenuBG");
	%file = new FileObject();
	%file.openForRead(%filename);
	%itemCount = 0;
	%iconDir = "base/client/ui/avatarIcons/" @ fileBase(%filename) @ "/";
	%varString = "$" @ fileBase(%filename);
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
	$num[fileBase(%filename)] = %itemCount;
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

function AvatarGui_CreatePartMenuFACE(%name, %cmdString, %filename, %xPos, %yPos)
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
	%newBox.setName("Avatar_" @ fileBase(%filename) @ "MenuBG");
	%file = new FileObject();
	%file.openForRead(%filename);
	%itemCount = 0;
	%varString = "$" @ fileBase(%filename);
	for (%line = %file.readLine(); %line !$= ""; %line = %file.readLine())
	{
		%newImage = new GuiBitmapCtrl();
		%newBox.add(%newImage);
		%newImage.keepCached = 1;
		%thumbFile = filePath(%line) @ "/thumbs/" @ fileBase(%line);
		%newImage.setBitmap(%thumbFile);
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
	$num[fileBase(%filename)] = %itemCount;
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
	%newBox = new GuiBitmapCtrl();
	%newScroll.add(%newBox);
	%newBox.setBitmap("base/client/ui/btnDecalBG");
	%newBox.wrap = 1;
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
		%oldx = getWord(Avatar_ColorMenu.getPosition(), 0);
		%oldy = getWord(Avatar_ColorMenu.getPosition(), 1);
		Avatar_ColorMenu.delete();
		if (%oldx != %xPos && %oldy != %yPos)
		{
			return;
		}
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
	Avatar_ColorMenu.schedule(0, delete);
	Avatar_UpdatePreview();
}

function Avatar_ClickTorsoColor()
{
	%decalName = $decal[$pref::Avatar::DecalColor];
	%x = getWord(Avatar_TorsoColor.position, 0) + 32;
	%y = getWord(Avatar_TorsoColor.position, 1);
	if ($pref::Avatar::Authentic && $torsoColors[%decalName] !$= "")
	{
		AvatarGui_CreateColorMenu("$Pref::Avatar::TorsoColor", $torsoColors[%decalName], %x, %y);
	}
	else
	{
		AvatarGui_CreateColorMenu("$Pref::Avatar::TorsoColor", $normalColors, %x, %y);
	}
}

function Avatar_ClickPackColor()
{
	%packName = $pack[$pref::Avatar::Pack];
	%x = getWord(Avatar_PackColor.position, 0) + 32;
	%y = getWord(Avatar_PackColor.position, 1);
	if ($pref::Avatar::Authentic && $packColors[%packName] !$= "")
	{
		AvatarGui_CreateColorMenu("$Pref::Avatar::PackColor", $packColors[%packName], %x, %y);
	}
	else
	{
		AvatarGui_CreateColorMenu("$Pref::Avatar::PackColor", $normalColors, %x, %y);
	}
}

function Avatar_ClickSecondPackColor()
{
	%packName = $SecondPack[$Pref::Avatar::SecondPack];
	%x = getWord(Avatar_SecondPackColor.position, 0) + 32;
	%y = getWord(Avatar_SecondPackColor.position, 1);
	if ($pref::Avatar::Authentic && $packColors[%packName] !$= "")
	{
		AvatarGui_CreateColorMenu("$Pref::Avatar::SecondPackColor", $packColors[%packName], %x, %y);
	}
	else
	{
		AvatarGui_CreateColorMenu("$Pref::Avatar::SecondPackColor", $normalColors, %x, %y);
	}
}

function Avatar_ClickHatColor()
{
	%hatName = $hat[$pref::Avatar::Hat];
	%x = getWord(Avatar_HatColor.position, 0) + 32;
	%y = getWord(Avatar_HatColor.position, 1);
	if ($pref::Avatar::Authentic && $hatColors[%hatName] !$= "")
	{
		AvatarGui_CreateColorMenu("$Pref::Avatar::HatColor", $hatColors[%hatName], %x, %y);
	}
	else
	{
		AvatarGui_CreateColorMenu("$Pref::Avatar::HatColor", $normalColors, %x, %y);
	}
}

function Avatar_ClickAccentColor()
{
	%hatName = $hat[$pref::Avatar::Hat];
	%AccentArray = $accentsAllowed[%hatName];
	%AccentName = getWord(%AccentArray, $pref::Avatar::Accent);
	%x = getWord(Avatar_AccentColor.position, 0) + 32;
	%y = getWord(Avatar_AccentColor.position, 1);
	AvatarGui_CreateColorMenu("$Pref::Avatar::AccentColor", $normalColors, %x, %y, "", 1);
	return;
	if (%AccentName !$= "None")
	{
		%x = getWord(Avatar_AccentColor.position, 0) + 32;
		%y = getWord(Avatar_AccentColor.position, 1);
		if ($pref::Avatar::Authentic)
		{
			if ($accentColors[%AccentName] $= "")
			{
				AvatarGui_CreateColorMenu("$Pref::Avatar::AccentColor", $allColors, %x, %y, "", 1);
			}
			else
			{
				AvatarGui_CreateColorMenu("$Pref::Avatar::AccentColor", $accentColors[%AccentName], %x, %y, "", 1);
			}
		}
		else if ($accentColorsUnAuth[%AccentName] $= "")
		{
			AvatarGui_CreateColorMenu("$Pref::Avatar::AccentColor", $allColors, %x, %y, "", 1);
		}
		else
		{
			AvatarGui_CreateColorMenu("$Pref::Avatar::AccentColor", $accentColorsUnAuth[%AccentName], %x, %y, "", 1);
		}
	}
}

function Avatar_ClickHeadColor()
{
	%x = getWord(Avatar_HeadColor.position, 0) + 32;
	%y = getWord(Avatar_HeadColor.position, 1);
	AvatarGui_CreateColorMenu("$Pref::Avatar::HeadColor", $normalColors, %x, %y);
}

function Avatar_ClickHipColor()
{
	%x = getWord(Avatar_HipColor.position, 0) + 32;
	%y = getWord(Avatar_HipColor.position, 1);
	if ($pref::Avatar::Authentic)
	{
		AvatarGui_CreateColorMenu("$Pref::Avatar::HipColor", $basicColors, %x, %y);
	}
	else
	{
		AvatarGui_CreateColorMenu("$Pref::Avatar::HipColor", $normalColors, %x, %y);
	}
}

function Avatar_ClickRightLegColor()
{
	%x = getWord(Avatar_RightLegColor.position, 0) + 32;
	%y = getWord(Avatar_RightLegColor.position, 1);
	if ($pref::Avatar::Authentic)
	{
		AvatarGui_CreateColorMenu("$Pref::Avatar::RLegColor", $basicColors, %x, %y, "$Pref::Avatar::LLegColor");
	}
	else
	{
		AvatarGui_CreateColorMenu("$Pref::Avatar::RLegColor", $normalColors, %x, %y, "$Pref::Avatar::LLegColor");
	}
}

function Avatar_ClickRightArmColor()
{
	%x = getWord(Avatar_RightArmColor.position, 0) + 32;
	%y = getWord(Avatar_RightArmColor.position, 1);
	if ($pref::Avatar::Authentic)
	{
		AvatarGui_CreateColorMenu("$Pref::Avatar::RArmColor", $basicColors, %x, %y, "$Pref::Avatar::LArmColor");
	}
	else
	{
		AvatarGui_CreateColorMenu("$Pref::Avatar::RArmColor", $normalColors, %x, %y, "$Pref::Avatar::LArmColor");
	}
}

function Avatar_ClickRightHandColor()
{
	%x = getWord(Avatar_RightHandColor.position, 0) + 32;
	%y = getWord(Avatar_RightHandColor.position, 1);
	if ($pref::Avatar::Authentic)
	{
		AvatarGui_CreateColorMenu("$Pref::Avatar::RHandColor", $basicColors, %x, %y, "$Pref::Avatar::LHandColor");
	}
	else
	{
		AvatarGui_CreateColorMenu("$Pref::Avatar::RHandColor", $normalColors, %x, %y, "$Pref::Avatar::LHandColor");
	}
}

function Avatar_ClickLeftLegColor()
{
	%x = getWord(Avatar_LeftLegColor.position, 0) + 32;
	%y = getWord(Avatar_LeftLegColor.position, 1);
	if ($pref::Avatar::Authentic)
	{
		AvatarGui_CreateColorMenu("$Pref::Avatar::LLegColor", $basicColors, %x, %y, "$Pref::Avatar::RLegColor");
	}
	else
	{
		AvatarGui_CreateColorMenu("$Pref::Avatar::LLegColor", $normalColors, %x, %y, "$Pref::Avatar::RLegColor");
	}
}

function Avatar_ClickLeftArmColor()
{
	%x = getWord(Avatar_LeftArmColor.position, 0) + 32;
	%y = getWord(Avatar_LeftArmColor.position, 1);
	if ($pref::Avatar::Authentic)
	{
		AvatarGui_CreateColorMenu("$Pref::Avatar::LArmColor", $basicColors, %x, %y, "$Pref::Avatar::RArmColor");
	}
	else
	{
		AvatarGui_CreateColorMenu("$Pref::Avatar::LArmColor", $normalColors, %x, %y, "$Pref::Avatar::RArmColor");
	}
}

function Avatar_ClickLeftHandColor()
{
	%x = getWord(Avatar_LeftHandColor.position, 0) + 32;
	%y = getWord(Avatar_LeftHandColor.position, 1);
	if ($pref::Avatar::Authentic)
	{
		AvatarGui_CreateColorMenu("$Pref::Avatar::LHandColor", $basicColors, %x, %y, "$Pref::Avatar::RHandColor");
	}
	else
	{
		AvatarGui_CreateColorMenu("$Pref::Avatar::LHandColor", $normalColors, %x, %y, "$Pref::Avatar::RHandColor");
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

function Avatar_SetFace(%index, %imageObj)
{
	$Pref::Avatar::FaceName = $face[%index];
	$pref::Avatar::FaceColor = %index;
	Avatar_FaceMenu.setVisible(0);
	Avatar_UpdatePreview();
}

function Avatar_SetDecal(%index, %imageObj)
{
	$Pref::Avatar::DecalName = $decal[%index];
	$pref::Avatar::DecalColor = %index;
	Avatar_DecalMenu.setVisible(0);
	Avatar_UpdatePreview();
}

function Avatar_SetPack(%index, %imageObj)
{
	$pref::Avatar::Pack = %index;
	Avatar_PackMenu.setVisible(0);
	Avatar_UpdatePreview();
}

function Avatar_SetSecondPack(%index, %imageObj)
{
	$Pref::Avatar::SecondPack = %index;
	Avatar_SecondPackMenu.setVisible(0);
	Avatar_UpdatePreview();
}

function Avatar_SetHat(%index, %imageObj)
{
	$pref::Avatar::Hat = %index;
	Avatar_HatMenu.setVisible(0);
	%x = getWord(Avatar_AccentPreview.position, 0) + 64;
	%y = getWord(Avatar_AccentPreview.position, 1);
	AvatarGui_CreateSubPartMenu("Avatar_AccentMenu", "Avatar_SetAccent", $accentsAllowed[$hat[%index]], %x, %y);
	Avatar_UpdatePreview();
}

function Avatar_SetAccent(%index, %imageObj)
{
	$pref::Avatar::Accent = %index;
	Avatar_AccentMenu.setVisible(0);
	Avatar_UpdatePreview();
}

function Avatar_SetChest(%index)
{
	$Pref::Avatar::Chest = %index;
	Avatar_HideAllPartMenus();
	Avatar_UpdatePreview();
}

function Avatar_SetHip(%index)
{
	$Pref::Avatar::Hip = %index;
	Avatar_HideAllPartMenus();
	Avatar_UpdatePreview();
}

function Avatar_SetLArm(%index)
{
	$Pref::Avatar::LArm = %index;
	if ($pref::Player::Symmetry != 1)
	{
		$Pref::Avatar::RArm = %index;
	}
	Avatar_HideAllPartMenus();
	Avatar_UpdatePreview();
}

function Avatar_SetRArm(%index)
{
	$Pref::Avatar::RArm = %index;
	if ($pref::Player::Symmetry != 1)
	{
		$Pref::Avatar::LArm = %index;
	}
	Avatar_HideAllPartMenus();
	Avatar_UpdatePreview();
}

function Avatar_SetLHand(%index)
{
	$Pref::Avatar::LHand = %index;
	Avatar_HideAllPartMenus();
	Avatar_UpdatePreview();
}

function Avatar_SetRHand(%index)
{
	$Pref::Avatar::RHand = %index;
	Avatar_HideAllPartMenus();
	Avatar_UpdatePreview();
}

function Avatar_SetLLeg(%index)
{
	$Pref::Avatar::LLeg = %index;
	Avatar_HideAllPartMenus();
	Avatar_UpdatePreview();
}

function Avatar_SetRLeg(%index)
{
	$Pref::Avatar::RLeg = %index;
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
	if ($pref::Avatar::Pack != 0 && $Pref::Avatar::SecondPack != 0)
	{
		Avatar_Preview.setThreadPos("", 0, 0);
	}
	else
	{
		Avatar_Preview.setThreadPos("", 0, 1);
	}
	Avatar_Preview.setNodeColor("", "HeadSkin", $pref::Avatar::HeadColor);
	if ($hat[$pref::Avatar::Hat] !$= "" && $hat[$pref::Avatar::Hat] !$= "none")
	{
		Avatar_Preview.unHideNode("", $hat[$pref::Avatar::Hat]);
		Avatar_Preview.setNodeColor("", $hat[$pref::Avatar::Hat], $pref::Avatar::HatColor);
	}
	%partList = $accentsAllowed[$hat[$pref::Avatar::Hat]];
	%AccentName = getWord(%partList, $pref::Avatar::Accent);
	if (%AccentName !$= "" && %AccentName !$= "none")
	{
		Avatar_Preview.unHideNode("", %AccentName);
		Avatar_Preview.setNodeColor("", %AccentName, $pref::Avatar::AccentColor);
	}
	if ($pack[$pref::Avatar::Pack] !$= "" && $pack[$pref::Avatar::Pack] !$= "none")
	{
		Avatar_Preview.unHideNode("", $pack[$pref::Avatar::Pack]);
		Avatar_Preview.setNodeColor("", $pack[$pref::Avatar::Pack], $pref::Avatar::PackColor);
	}
	if ($SecondPack[$Pref::Avatar::SecondPack] !$= "" && $SecondPack[$Pref::Avatar::SecondPack] !$= "none")
	{
		Avatar_Preview.unHideNode("", $SecondPack[$Pref::Avatar::SecondPack]);
		Avatar_Preview.setNodeColor("", $SecondPack[$Pref::Avatar::SecondPack], $pref::Avatar::SecondPackColor);
	}
	if ($Chest[$Pref::Avatar::Chest] !$= "" && $Chest[$Pref::Avatar::Chest] !$= "none")
	{
		Avatar_Preview.unHideNode("", $Chest[$Pref::Avatar::Chest]);
		Avatar_Preview.setNodeColor("", $Chest[$Pref::Avatar::Chest], $pref::Avatar::TorsoColor);
	}
	if ($Hip[$Pref::Avatar::Hip] !$= "" && $Hip[$Pref::Avatar::Hip] !$= "none")
	{
		Avatar_Preview.unHideNode("", $Hip[$Pref::Avatar::Hip]);
		Avatar_Preview.setNodeColor("", $Hip[$Pref::Avatar::Hip], $pref::Avatar::HipColor);
	}
	if ($RArm[$Pref::Avatar::RArm] !$= "" && $RArm[$Pref::Avatar::RArm] !$= "none")
	{
		Avatar_Preview.unHideNode("", $RArm[$Pref::Avatar::RArm]);
		Avatar_Preview.setNodeColor("", $RArm[$Pref::Avatar::RArm], $pref::Avatar::RArmColor);
	}
	if ($LArm[$Pref::Avatar::LArm] !$= "" && $LArm[$Pref::Avatar::LArm] !$= "none")
	{
		Avatar_Preview.unHideNode("", $LArm[$Pref::Avatar::LArm]);
		Avatar_Preview.setNodeColor("", $LArm[$Pref::Avatar::LArm], $pref::Avatar::LArmColor);
	}
	if ($RHand[$Pref::Avatar::RHand] !$= "" && $RHand[$Pref::Avatar::RHand] !$= "none")
	{
		Avatar_Preview.unHideNode("", $RHand[$Pref::Avatar::RHand]);
		Avatar_Preview.setNodeColor("", $RHand[$Pref::Avatar::RHand], $pref::Avatar::RHandColor);
	}
	if ($LHand[$Pref::Avatar::LHand] !$= "" && $LHand[$Pref::Avatar::LHand] !$= "none")
	{
		Avatar_Preview.unHideNode("", $LHand[$Pref::Avatar::LHand]);
		Avatar_Preview.setNodeColor("", $LHand[$Pref::Avatar::LHand], $pref::Avatar::LHandColor);
	}
	if ($RLeg[$Pref::Avatar::RLeg] !$= "" && $RLeg[$Pref::Avatar::RLeg] !$= "none")
	{
		Avatar_Preview.unHideNode("", $RLeg[$Pref::Avatar::RLeg]);
		Avatar_Preview.setNodeColor("", $RLeg[$Pref::Avatar::RLeg], $pref::Avatar::RLegColor);
	}
	if ($LLeg[$Pref::Avatar::LLeg] !$= "" && $LLeg[$Pref::Avatar::LLeg] !$= "none")
	{
		Avatar_Preview.unHideNode("", $LLeg[$Pref::Avatar::LLeg]);
		Avatar_Preview.setNodeColor("", $LLeg[$Pref::Avatar::LLeg], $pref::Avatar::LLegColor);
	}
	if ($Hip[$Pref::Avatar::Hip] !$= "" && $Hip[$Pref::Avatar::Hip] !$= "none")
	{
		Avatar_Preview.unHideNode("", $Hip[$Pref::Avatar::Hip]);
		Avatar_Preview.setNodeColor("", $Hip[$Pref::Avatar::Hip], $pref::Avatar::HipColor);
		if ($Hip[$Pref::Avatar::Hip] $= "skirtHip")
		{
			Avatar_Preview.unHideNode("", "SkirtTrimLeft");
			Avatar_Preview.unHideNode("", "SkirtTrimRight");
			Avatar_Preview.setNodeColor("", "SkirtTrimLeft", $pref::Avatar::LLegColor);
			Avatar_Preview.setNodeColor("", "SkirtTrimRight", $pref::Avatar::RLegColor);
			Avatar_Preview.hideNode("", $RLeg[$Pref::Avatar::RLeg]);
			Avatar_Preview.hideNode("", $LLeg[$Pref::Avatar::LLeg]);
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
	Avatar_Preview.setIflFrame("", decal, $pref::Avatar::DecalColor);
	Avatar_Preview.setIflFrame("", face, $pref::Avatar::FaceColor);
	Avatar_TorsoColor.setColor($pref::Avatar::TorsoColor);
	Avatar_ChestPreview.setColor($pref::Avatar::TorsoColor);
	Avatar_ColorAllIcons(Avatar_ChestMenuBG, $pref::Avatar::TorsoColor);
	Avatar_DecalBG.setColor($pref::Avatar::TorsoColor);
	Avatar_DecalMenuBG.setColor($pref::Avatar::TorsoColor);
	Avatar_HeadColor.setColor($pref::Avatar::HeadColor);
	Avatar_HeadBG.setColor($pref::Avatar::HeadColor);
	Avatar_faceMenuBG.setColor($pref::Avatar::HeadColor);
	Avatar_HipColor.setColor($pref::Avatar::HipColor);
	Avatar_HipPreview.setColor($pref::Avatar::HipColor);
	Avatar_ColorAllIcons(Avatar_HipMenuBG, $pref::Avatar::HipColor);
	Avatar_LeftArmColor.setColor($pref::Avatar::LArmColor);
	Avatar_LArmPreview.setColor($pref::Avatar::LArmColor);
	Avatar_ColorAllIcons(Avatar_LarmMenuBG, $pref::Avatar::LArmColor);
	Avatar_LeftHandColor.setColor($pref::Avatar::LHandColor);
	Avatar_LHandPreview.setColor($pref::Avatar::LHandColor);
	Avatar_ColorAllIcons(Avatar_LHandMenuBG, $pref::Avatar::LHandColor);
	Avatar_LeftLegColor.setColor($pref::Avatar::LLegColor);
	Avatar_LLegPreview.setColor($pref::Avatar::LLegColor);
	Avatar_ColorAllIcons(Avatar_LLegMenuBG, $pref::Avatar::LLegColor);
	Avatar_RightArmColor.setColor($pref::Avatar::RArmColor);
	Avatar_RArmPreview.setColor($pref::Avatar::RArmColor);
	Avatar_ColorAllIcons(Avatar_RarmMenuBG, $pref::Avatar::RArmColor);
	Avatar_RightHandColor.setColor($pref::Avatar::RHandColor);
	Avatar_RHandPreview.setColor($pref::Avatar::RHandColor);
	Avatar_ColorAllIcons(Avatar_RHandMenuBG, $pref::Avatar::RHandColor);
	Avatar_RightLegColor.setColor($pref::Avatar::RLegColor);
	Avatar_RLegPreview.setColor($pref::Avatar::RLegColor);
	Avatar_ColorAllIcons(Avatar_RLegMenuBG, $pref::Avatar::RLegColor);
	Avatar_HatColor.setColor($pref::Avatar::HatColor);
	Avatar_HatPreview.setColor($pref::Avatar::HatColor);
	Avatar_ColorAllIcons(Avatar_HatMenuBG, $pref::Avatar::HatColor);
	Avatar_AccentColor.setColor($pref::Avatar::AccentColor);
	Avatar_AccentPreview.setColor($pref::Avatar::AccentColor);
	Avatar_ColorAllIcons(Avatar_AccentMenuBG, $pref::Avatar::AccentColor);
	Avatar_PackColor.setColor($pref::Avatar::PackColor);
	Avatar_PackPreview.setColor($pref::Avatar::PackColor);
	Avatar_ColorAllIcons(Avatar_PackMenuBG, $pref::Avatar::PackColor);
	Avatar_SecondPackColor.setColor($pref::Avatar::SecondPackColor);
	Avatar_SecondPackPreview.setColor($pref::Avatar::SecondPackColor);
	Avatar_ColorAllIcons(Avatar_SecondPackMenuBG, $pref::Avatar::SecondPackColor);
	%iconDir = "base/client/ui/avatarIcons/";
	%thumb = filePath($face[$pref::Avatar::FaceColor]) @ "/thumbs/" @ fileBase($face[$pref::Avatar::FaceColor]);
	Avatar_FacePreview.setBitmap(%thumb);
	%thumb = filePath($decal[$pref::Avatar::DecalColor]) @ "/thumbs/" @ fileBase($decal[$pref::Avatar::DecalColor]);
	Avatar_DecalPreview.setBitmap($decal[$pref::Avatar::DecalColor]);
	if ($pack[$pref::Avatar::Pack] $= "none")
	{
		Avatar_PackPreview.setBitmap(%iconDir @ "none");
	}
	else
	{
		Avatar_PackPreview.setBitmap(%iconDir @ "pack/" @ $pack[$pref::Avatar::Pack]);
	}
	Avatar_SecondPackPreview.setBitmap(%iconDir @ "secondPack/" @ $SecondPack[$Pref::Avatar::SecondPack]);
	Avatar_HatPreview.setBitmap(%iconDir @ "hat/" @ $hat[$pref::Avatar::Hat]);
	Avatar_ChestPreview.setBitmap(%iconDir @ "chest/" @ $Chest[$Pref::Avatar::Chest]);
	Avatar_HipPreview.setBitmap(%iconDir @ "hip/" @ $Hip[$Pref::Avatar::Hip]);
	Avatar_LArmPreview.setBitmap(%iconDir @ "Larm/" @ $LArm[$Pref::Avatar::LArm]);
	Avatar_LHandPreview.setBitmap(%iconDir @ "Lhand/" @ $LHand[$Pref::Avatar::LHand]);
	Avatar_LLegPreview.setBitmap(%iconDir @ "Lleg/" @ $LLeg[$Pref::Avatar::LLeg]);
	Avatar_RArmPreview.setBitmap(%iconDir @ "Rarm/" @ $RArm[$Pref::Avatar::RArm]);
	Avatar_RHandPreview.setBitmap(%iconDir @ "Rhand/" @ $RHand[$Pref::Avatar::RHand]);
	Avatar_RLegPreview.setBitmap(%iconDir @ "Rleg/" @ $RLeg[$Pref::Avatar::RLeg]);
	%accentList = $accentsAllowed[$hat[$pref::Avatar::Hat]];
	%accent = getWord(%accentList, $pref::Avatar::Accent);
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
	$pref::Avatar::FaceColor = getRandom($numFace - 1);
	$Pref::Avatar::FaceName = fileBase($face[$pref::Avatar::FaceColor]);
	$pref::Avatar::DecalColor = getRandom($numDecal - 1);
	$Pref::Avatar::DecalName = fileBase($decal[$pref::Avatar::DecalColor]);
	$pref::Avatar::Hat = getRandom($numHat - 1);
	$pref::Avatar::Pack = getRandom($numPack - 1);
	$Pref::Avatar::SecondPack = getRandom($numSecondPack - 1);
	$Pref::Avatar::LArm = getRandom($numLArm - 1);
	$Pref::Avatar::RArm = $Pref::Avatar::LArm;
	%chance = getRandom(100);
	%normalHands = 0;
	if (%chance < 70)
	{
		%normalHands = 1;
		$Pref::Avatar::LHand = 0;
		$Pref::Avatar::RHand = 0;
	}
	else
	{
		$Pref::Avatar::LHand = getRandom($numLHand - 1);
		$Pref::Avatar::RHand = getRandom($numRHand - 1);
	}
	%chance = getRandom(100);
	if (%chance < 80)
	{
		$Pref::Avatar::LLeg = 0;
		$Pref::Avatar::RLeg = 0;
	}
	else
	{
		$Pref::Avatar::LLeg = getRandom($numLLeg - 1);
		$Pref::Avatar::RLeg = getRandom($numRLeg - 1);
	}
	$Pref::Avatar::Chest = getRandom($numChest - 1);
	%chance = getRandom(100);
	if (%chance < 70)
	{
		$Pref::Avatar::Hip = 0;
	}
	else
	{
		$Pref::Avatar::Hip = getRandom($numHip - 1);
	}
	if ($Chest[$Pref::Avatar::Chest] $= "femchest")
	{
		if ($Pref::Avatar::FaceName !$= "smiley" && $Pref::Avatar::FaceName !$= "smileyCreepy" && strstr(strlwr($Pref::Avatar::FaceName), "female") != -1)
		{
			$Pref::Avatar::Chest = 0;
		}
	}
	else if (strpos(strlwr($Pref::Avatar::FaceName), "female") == -1)
	{
		$Pref::Avatar::Chest = 1;
	}
	%partList = $accentsAllowed[$hat[$pref::Avatar::Hat]];
	%count = getWordCount(%partList) - 1;
	if (%count > 0)
	{
		%chance = getRandom(5);
		if (%chance == 0)
		{
			$pref::Avatar::Accent = getRandom(%count - 1) + 1;
		}
		else
		{
			$pref::Avatar::Accent = 0;
		}
	}
	else
	{
		$pref::Avatar::Accent = 0;
	}
	%x = getWord(Avatar_AccentPreview.position, 0) + 64;
	%y = getWord(Avatar_AccentPreview.position, 1);
	%hatName = $hat[$pref::Avatar::Hat];
	%AccentArray = $accentsAllowed[%hatName];
	%AccentName = getWord(%AccentArray, $pref::Avatar::Accent);
	AvatarGui_CreateSubPartMenu("Avatar_AccentMenu", "Avatar_SetAccent", %AccentArray, %x, %y);
	%count = getWordCount(%torsoColorList) - 1;
	$pref::Avatar::TorsoColor = Avatar_GetRandomColor(0);
	%count = getWordCount(%packColorList) - 1;
	$pref::Avatar::PackColor = Avatar_GetRandomColor(0);
	$pref::Avatar::SecondPackColor = Avatar_GetRandomColor(0);
	%count = getWordCount(%hatColorList) - 1;
	$pref::Avatar::HatColor = Avatar_GetRandomColor(0);
	%count = getWordCount(%accentColorList) - 1;
	$pref::Avatar::AccentColor = Avatar_GetRandomColor(1);
	%count = getWordCount(%hipColorList) - 1;
	$pref::Avatar::HipColor = Avatar_GetRandomColor(0);
	%count = getWordCount(%LLegColorList) - 1;
	$pref::Avatar::LLegColor = Avatar_GetRandomColor(0);
	%count = getWordCount(%LArmColorList) - 1;
	$pref::Avatar::LArmColor = Avatar_GetRandomColor(0);
	%count = getWordCount(%LHandColorList) - 1;
	$pref::Avatar::LHandColor = Avatar_GetRandomColor(0);
	if (%normalHands)
	{
		if (getRandom(1))
		{
			$pref::Avatar::LHandColor = $pref::Avatar::HeadColor;
		}
	}
	if ($pref::Player::Symmetry != 1)
	{
		$pref::Avatar::RLegColor = $pref::Avatar::LLegColor;
		$pref::Avatar::RArmColor = $pref::Avatar::LArmColor;
		$pref::Avatar::RHandColor = $pref::Avatar::LHandColor;
	}
	else
	{
		%count = getWordCount(%RLegColorList) - 1;
		$pref::Avatar::RLegColor = Avatar_GetRandomColor(0);
		%count = getWordCount(%RArmColorList) - 1;
		$pref::Avatar::RArmColor = Avatar_GetRandomColor(0);
		%count = getWordCount(%RHandColorList) - 1;
		$pref::Avatar::RHandColor = Avatar_GetRandomColor(0);
	}
	Avatar_UpdatePreview();
}

function Avatar_GetRandomColor(%allowTrans)
{
	for (%i = 0; %i < 1000; %i++)
	{
		%color = $Avatar::Color[getRandom(8)];
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
	%filename = "config/client/AvatarFavorites/" @ %idx @ ".cs";
	if (AV_FavsHelper.isVisible())
	{
		export("$Pref::Avatar::*", %filename, 0);
		AV_FavsHelper.setVisible(0);
	}
	else
	{
		if (!isFile(%filename))
		{
			return;
		}
		deleteVariables("$Pref::Player::HeadColor");
		%netName = $pref::Player::NetName;
		%LANname = $pref::Player::LANName;
		exec(%filename);
		$pref::Player::NetName = %netName;
		$pref::Player::LANName = %LANname;
		if ($Pref::Player::HeadColor !$= "")
		{
			transferOldAvatarPrefs();
		}
		for (%i = 0; %i < $numDecal; %i++)
		{
			if (fileBase($decal[%i]) $= fileBase($Pref::Avatar::DecalName))
			{
				$pref::Avatar::DecalColor = %i;
				break;
			}
		}
		for (%i = 0; %i < $numFace; %i++)
		{
			if (fileBase($face[%i]) $= fileBase($Pref::Avatar::FaceName))
			{
				$pref::Avatar::FaceColor = %i;
				break;
			}
		}
		Avatar_UpdatePreview();
		$pref::Player::Symmetry = Avatar_SymmetryCheckbox.getValue();
	}
	AvatarGui.updateFavButtons();
}

function transferOldAvatarPrefs()
{
	$pref::Avatar::Accent = $Pref::Player::Accent;
	$pref::Avatar::AccentColor = $Pref::Player::AccentColor;
	$Pref::Avatar::Chest = $Pref::Player::Chest;
	$pref::Avatar::ChestColor = $Pref::Player::ChestColor;
	$pref::Avatar::DecalColor = $Pref::Player::DecalColor;
	$Pref::Avatar::DecalName = $Pref::Player::DecalName;
	$pref::Avatar::FaceColor = $Pref::Player::FaceColor;
	$Pref::Avatar::FaceName = $Pref::Player::FaceName;
	$pref::Avatar::Hat = $Pref::Player::Hat;
	$pref::Avatar::HatColor = $Pref::Player::HatColor;
	$pref::Avatar::HeadColor = $Pref::Player::HeadColor;
	$Pref::Avatar::Hip = $Pref::Player::Hip;
	$pref::Avatar::HipColor = $Pref::Player::HipColor;
	$Pref::Avatar::LArm = $Pref::Player::LArm;
	$pref::Avatar::LArmColor = $Pref::Player::LArmColor;
	$Pref::Avatar::LHand = $Pref::Player::LHand;
	$pref::Avatar::LHandColor = $Pref::Player::LHandColor;
	$Pref::Avatar::LLeg = $Pref::Player::LLeg;
	$pref::Avatar::LLegColor = $Pref::Player::LLegColor;
	$pref::Avatar::Pack = $Pref::Player::Pack;
	$pref::Avatar::PackColor = $Pref::Player::PackColor;
	$Pref::Avatar::RArm = $Pref::Player::RArm;
	$pref::Avatar::RArmColor = $Pref::Player::RArmColor;
	$Pref::Avatar::RHand = $Pref::Player::RHand;
	$pref::Avatar::RHandColor = $Pref::Player::RHandColor;
	$Pref::Avatar::RLeg = $Pref::Player::RLeg;
	$pref::Avatar::RLegColor = $Pref::Player::RLegColor;
	$Pref::Avatar::SecondPack = $Pref::Player::SecondPack;
	$pref::Avatar::SecondPackColor = $Pref::Player::SecondPackColor;
	$pref::Avatar::TorsoColor = $Pref::Player::TorsoColor;
	deleteVariables("$pref::Player::Accent");
	deleteVariables("$pref::Player::AccentColor");
	deleteVariables("$pref::Player::Authentic");
	deleteVariables("$Pref::Player::Chest");
	deleteVariables("$pref::Player::ChestColor");
	deleteVariables("$pref::Player::DecalColor");
	deleteVariables("$Pref::Player::DecalName");
	deleteVariables("$pref::Player::FaceColor");
	deleteVariables("$Pref::Player::FaceName");
	deleteVariables("$pref::Player::Hat");
	deleteVariables("$pref::Player::HatColor");
	deleteVariables("$pref::Player::HeadColor");
	deleteVariables("$Pref::Player::Hip");
	deleteVariables("$pref::Player::HipColor");
	deleteVariables("$Pref::Player::LArm");
	deleteVariables("$pref::Player::LArmColor");
	deleteVariables("$Pref::Player::LHand");
	deleteVariables("$pref::Player::LHandColor");
	deleteVariables("$Pref::Player::LLeg");
	deleteVariables("$pref::Player::LLegColor");
	deleteVariables("$pref::Player::Pack");
	deleteVariables("$pref::Player::PackColor");
	deleteVariables("$Pref::Player::RArm");
	deleteVariables("$pref::Player::RArmColor");
	deleteVariables("$Pref::Player::RHand");
	deleteVariables("$pref::Player::RHandColor");
	deleteVariables("$Pref::Player::RLeg");
	deleteVariables("$pref::Player::RLegColor");
	deleteVariables("$Pref::Player::SecondPack");
	deleteVariables("$pref::Player::SecondPackColor");
	deleteVariables("$pref::Player::Symmetry");
	deleteVariables("$pref::Player::TorsoColor");
}

function AvatarGui::updateFavButtons()
{
	for (%i = 0; %i < 10; %i++)
	{
		%filename = "config/client/AvatarFavorites/" @ %i @ ".cs";
		if (isFile(%filename))
		{
			eval("Avatar_FavButton" @ %i @ ".setColor(\"1 1 1 1\");");
		}
		else
		{
			eval("Avatar_FavButton" @ %i @ ".setColor(\"1 1 1 0.5\");");
		}
	}
}

function clientCmdDoUpdates()
{
	return;
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
	if (isMacintosh())
	{
		AU_Time.setVisible(0);
		AU_Data.setVisible(0);
		AU_Speed.setVisible(0);
		AU_Progress.setVisible(0);
		AU_TimeLabel.setVisible(0);
		AU_DataLabel.setVisible(0);
		AU_SpeedLabel.setVisible(0);
	}
	else
	{
		AU_Time.setVisible(1);
		AU_Data.setVisible(1);
		AU_Speed.setVisible(1);
		AU_Progress.setVisible(1);
		AU_TimeLabel.setVisible(1);
		AU_DataLabel.setVisible(1);
		AU_SpeedLabel.setVisible(1);
	}
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
	if ($AU_AutoClose != 1)
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
	AU_Text.setText("Checking for Blockland Update...");
	if (isObject(verTCPobj))
	{
		verTCPobj.delete();
	}
	new TCPObject(verTCPobj);
	verTCPobj.site = "update.blockland.us";
	verTCPobj.port = 80;
	verTCPobj.filePath = "/" @ $Version @ ".txt";
	verTCPobj.lastLine = "crap";
	verTCPobj.connect(verTCPobj.site @ ":" @ verTCPobj.port);
}

function verTCPobj::onConnected(%this)
{
	AU_Text.echo("Getting version info...");
	%this.send("GET " @ %this.filePath @ " HTTP/1.0\r\nHost: " @ %this.site @ "\r\n\r\n");
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
	if (strstr(%line, "404 Not Found") == -1)
	{
		AU_Text.echo("Update information not found.");
		%this.done = 1;
		AutoUpdateGui.done();
	}
	if (%this.lastLine $= "" && %this.done != 0)
	{
		if (%line !$= "")
		{
			AU_Text.echo("Update required");
			%this.done = 1;
			if (isMacintosh())
			{
				messageBoxOKCancel("Update Required", "There is a new version of Blockland available.\n\nYou will need it play online.", "gotoWebPage(\"http://blockland.us/Download.html\");");
			}
			else
			{
				AU_GetUpdate(%line);
			}
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
	%filename = "/" @ %newVersion;
	%saveName = "patches/" @ %newVersion;
	if (isObject(patchTCPobj))
	{
		patchTCPobj.delete();
	}
	new TCPObject(patchTCPobj);
	patchTCPobj.site = "update.blockland.us";
	patchTCPobj.port = 80;
	patchTCPobj.filePath = %filename;
	patchTCPobj.saveTo = "patches/patch.exe";
	patchTCPobj.lastLine = "crap";
	patchTCPobj.badFile = 0;
	patchTCPobj.connect(patchTCPobj.site @ ":" @ patchTCPobj.port);
}

function patchTCPobj::onConnected(%this)
{
	AU_Text.echo("Requesting update: " @ fileBase(%this.filePath));
	%this.send("GET " @ %this.filePath @ " HTTP/1.0\r\nHost: " @ %this.site @ "\r\n\r\n");
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
			messageBoxOKCancel("Auto Update Error", "There was an error while attempting to download an update.\n\nPlease visit <a:blockland.us/update/index.php?v=" @ $Version @ ">Blockland.us/update</a> to get the latest updates.", "gotoWebPage(\"http://blockland.us/update/index.php?v=" @ $Version @ "\");");
			%this.badFile = 1;
			%this.disconnect();
		}
	}
	else if (%word $= "Content-Length:")
	{
		%fileSize = getWord(%line, 1);
		%this.fileSize = mFloor(%fileSize);
	}
	else if (%word $= "Content-Type:")
	{
		if (%this.badFile != 0)
		{
			%mimeType = getWord(%line, 1);
			if (%mimeType !$= "application/octet-stream")
			{
				AU_Text.echo("Bad mime type.");
				messageBoxOKCancel("Auto Update Error", "There was an error while attempting to download an update.\n\nPlease visit <a:blockland.us/update/index.php?v=" @ $Version @ ">Blockland.us/update</a> to get the latest updates.", "gotoWebPage(\"http://blockland.us/update/index.php?v=" @ $Version @ "\");");
				%this.badFile = 1;
				%this.disconnect();
			}
		}
	}
	if (%this.badFile != 0)
	{
		if (%line $= "")
		{
			%this.setBinarySize(%this.fileSize);
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
	%buffSize = mFloor(%buffSize);
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
		AU_Text.echo("Receiving update...");
		AU_Text.echo(" When the download is finished, the installation procedure will start automatically.");
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
	if (%buffSize >= %this.fileSize || %buffSize $= %this.fileSize)
	{
		%this.saveBufferToFile(%this.saveTo);
		%this.disconnect();
		schedule(100, 0, WinExec, 292070934);
	}
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
		$AU_AutoClose = 1;
		Canvas.pushDialog(keyGui);
	}
	MM_Version.setText("Version: " @ $Version);
	MainMenuGui.buildScreenshotList();
	if (MainMenuGui.screenShotCount > 0)
	{
		%pic = MainMenuGui.screenShot[getRandom(MainMenuGui.screenShotCount)];
		mm_Fade.pic = %pic;
		mm_Fade.setBitmap(%pic);
	}
	if ($Version $= "Pre-v9")
	{
		$Pref::Net::ConnectionType = 1;
		$Pref::Input::SelectedDefaults = 1;
	}
	if ($Pref::Input::SelectedDefaults != 0 || !isFile("config/client/config.cs") || moveMap.getNumBinds() < 5)
	{
		vendorSpecificDefaults();
		Canvas.pushDialog(defaultControlsGui);
	}
	if ($Pref::Net::ConnectionType <= 0)
	{
		Canvas.pushDialog(SelectNetworkGui);
	}
	buildIFLs();
	PlayGui.createInvHud();
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
			auth_Init_Client();
		}
	}
	BringWindowToForeground();
	if (isMacintosh())
	{
		if (gotProtoURL())
		{
			MainMenuGui.hideButtons();
			parseProtocol(getProtoURL());
		}
	}
	if ($connectArg)
	{
		Connecting_Text.setText("Connecting to " @ $connectArg);
		Canvas.pushDialog(connectingGui);
		MainMenuGui.hideButtons();
	}
	else
	{
		MainMenuGui.showButtons();
	}
	if ($Version $= "Pre-v9")
	{
		MM_JoinButton.setVisible(0);
		MM_OptionsButton.setVisible(0);
		MM_StartButton.setVisible(0);
		MM_PlayerButton.setVisible(0);
		MM_TutorialButton.setVisible(0);
		return;
	}
	if ($Pref::Player::HeadColor !$= "")
	{
		transferOldAvatarPrefs();
	}
	if (isFile("config/server/ADD_ON_LIST.cs"))
	{
		exec("config/server/ADD_ON_LIST.cs");
		if ($AddOn__Brick_Treasure_Chest $= "")
		{
			$AddOn__Brick_Treasure_Chest = 1;
		}
		if ($AddOn__Brick_Halloween $= "")
		{
			$AddOn__Brick_Halloween = 1;
		}
		if ($AddOn__Brick_Teledoor $= "")
		{
			$AddOn__Brick_Teledoor = 1;
		}
		if ($AddOn__Brick_Christmas_Tree $= "")
		{
			$AddOn__Brick_Christmas_Tree = 1;
		}
		export("$AddOn__*", "config/server/ADD_ON_LIST.cs");
	}
	if ($Version $= $Pref::ScriptVersion)
	{
		return;
	}
	if ($Version <= 1.02)
	{
		$Pref::Net::DownloadSounds = 0;
	}
	if ($Version <= 1.03)
	{
		$Pref::Server::BrickPublicDomainTimeout = -1;
		$pref::OpenGL::noVertexBufferObjects = 0;
	}
	if ($Version $= "10" || $Version $= "10m")
	{
		$Pref::Net::DownloadMusic = 0;
		$Pref::Net::DownloadSounds = 0;
	}
	if ($Version $= "11" || $Version $= "11m")
	{
		$pref::ParticleFalloffMaxLevel = 3;
		$pref::ParticleFalloffMinDistance = 35;
	}
	if ($Version $= "12")
	{
		if (isFile("config/server/ADD_ON_LIST.cs"))
		{
			exec("config/server/ADD_ON_LIST.cs");
			$AddOn__Brick_Arch = 1;
			export("$AddOn__*", "config/server/ADD_ON_LIST.cs");
		}
	}
	if ($Version $= "13")
	{
		$Pref::Net::DownloadTextures = 0;
		if (isFile("config/server/ADD_ON_LIST.cs"))
		{
			exec("config/server/ADD_ON_LIST.cs");
			$AddOn__Particle_Tools = 1;
			$AddOn__Script_TerrainBuildRules = -1;
			export("$AddOn__*", "config/server/ADD_ON_LIST.cs");
		}
	}
	if ($Version $= "14")
	{
		$pref::OpenGL::noVertexBufferObjects = 0;
		if (isFile("config/server/ADD_ON_LIST.cs"))
		{
			exec("config/server/ADD_ON_LIST.cs");
			$AddOn__Vehicle_Rowboat = 1;
			$AddOn__Vehicle_Pirate_Cannon = 1;
			export("$AddOn__*", "config/server/ADD_ON_LIST.cs");
		}
	}
	if ($Version $= "15")
	{
		if ($Pref::Server::BrickRespawnTime < 30000)
		{
			$Pref::Server::BrickRespawnTime = 30000;
		}
		$Pref::Server::BrickLimit = 256000;
		if (isFile("config/server/ADD_ON_LIST.cs"))
		{
			exec("config/server/ADD_ON_LIST.cs");
			$AddOn__Brick_V15 = 1;
			export("$AddOn__*", "config/server/ADD_ON_LIST.cs");
		}
	}
	if ($Version $= "16")
	{
		if ($Pref::Net::ConnectionType >= 4)
		{
			$Pref::Net::ConnectionType = 4;
			SetConnectionType($Pref::Net::ConnectionType);
		}
	}
	if ($Version $= "17")
	{
		if ($Pref::Net::ConnectionType <= 3)
		{
			$Pref::Net::ConnectionType = 3;
			SetConnectionType($Pref::Net::ConnectionType);
		}
	}
	$Pref::ScriptVersion = $Version;
}

function MainMenuGui::hideButtons(%this)
{
	MM_JoinButton.setVisible(0);
	MM_OptionsButton.setVisible(0);
	MM_StartButton.setVisible(0);
	MM_PlayerButton.setVisible(0);
	MM_TutorialButton.setVisible(0);
	MM_QuitButton.setVisible(0);
	MM_AboutButton.setVisible(0);
	MM_CreditsButton.setVisible(0);
}

function MainMenuGui::showButtons(%this)
{
	MM_JoinButton.setVisible(1);
	MM_OptionsButton.setVisible(1);
	MM_StartButton.setVisible(1);
	MM_PlayerButton.setVisible(1);
	MM_TutorialButton.setVisible(1);
	MM_QuitButton.setVisible(1);
	MM_AboutButton.setVisible(1);
	MM_CreditsButton.setVisible(1);
}

function getOldSaves()
{
	%pattern = "base/saves/*.bls";
	%fileCount = getFileCount(%pattern);
	%filename = findFirstFile(%pattern);
	echo("Getting old save files from base/saves/ ...");
	echo("  " @ %fileCount @ " files found");
	for (%i = 0; %i < %fileCount; %i++)
	{
		%path = filePath(%filename);
		%dirName = getSubStr(%path, strlen("base/saves/"), strlen(%path) - strlen("base/saves/"));
		echo("  dirName: " @ %dirName);
		%base = fileBase(%filename);
		%newFileName = "saves/" @ %dirName @ "/" @ %base @ ".bls";
		if (!isFile(%newFileName))
		{
			echo("  copying " @ %dirName @ "/" @ %base @ ".bls");
			copyTextFile(%filename, %newFileName);
		}
		%filename = findNextFile(%pattern);
	}
}

function copyTextFile(%source, %destination)
{
	if (!isFile(%source))
	{
		error("ERROR: copyFile: source file \"" @ %source @ "\" does not exist");
		return;
	}
	%sourceFile = new FileObject();
	%sourceFile.openForRead(%source);
	%destFile = new FileObject();
	for (%destFile.openForWrite(%destination); !%sourceFile.isEOF(); %destFile.writeLine(%line))
	{
		%line = %sourceFile.readLine();
	}
	%destFile.close();
	%destFile.delete();
	%sourceFile.close();
	%sourceFile.delete();
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
	if (strpos(getModPaths(), "screenshots") != -1)
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
	%file = new FileObject();
	%file.openForWrite("base/data/shapes/player/decal.ifl");
	%file.writeLine("base/data/shapes/player/decals/AAA-None.png");
	%search = "Add-Ons/Decal_*/*.png";
	%fullPath = findFirstFile(%search);
	while (%fullPath !$= "")
	{
		if (!isValidDecal(%fullPath))
		{
			%fullPath = findNextFile(%search);
		}
		else
		{
			%file.writeLine(%fullPath);
			%fullPath = findNextFile(%search);
		}
	}
	%file.close();
	%file.openForWrite("base/data/shapes/player/face.ifl");
	%file.writeLine("base/data/shapes/player/faces/smiley.png");
	%search = "Add-Ons/Face_*/*.png";
	%fullPath = findFirstFile(%search);
	while (%fullPath !$= "")
	{
		if (!isValidDecal(%fullPath))
		{
			%fullPath = findNextFile(%search);
		}
		else
		{
			%file.writeLine(%fullPath);
			%fullPath = findNextFile(%search);
		}
	}
	%file.close();
	%file.delete();
}

function MM_TutorialButton::onMouseEnter(%this)
{
	if ($Pref::Audio::MenuSounds)
	{
		alxPlay(Note3Sound);
	}
}

function MM_StartButton::onMouseEnter(%this)
{
	if ($Pref::Audio::MenuSounds)
	{
		alxPlay(Note4Sound);
	}
}

function MM_JoinButton::onMouseEnter(%this)
{
	if ($Pref::Audio::MenuSounds)
	{
		alxPlay(Note5Sound);
	}
}

function MM_PlayerButton::onMouseEnter(%this)
{
	if ($Pref::Audio::MenuSounds)
	{
		alxPlay(Note6Sound);
	}
}

function MM_OptionsButton::onMouseEnter(%this)
{
	if ($Pref::Audio::MenuSounds)
	{
		alxPlay(Note7Sound);
	}
}

function MM_DemoButton::onMouseEnter(%this)
{
	if ($Pref::Audio::MenuSounds)
	{
		alxPlay(Note8Sound);
	}
}

function MM_QuitButton::onMouseEnter(%this)
{
	if ($Pref::Audio::MenuSounds)
	{
		alxPlay(Note0Sound);
	}
}

function MM_AboutButton::onMouseEnter(%this)
{
	if ($Pref::Audio::MenuSounds)
	{
		alxPlay(Note1Sound);
	}
}

function MM_CreditsButton::onMouseEnter(%this)
{
	if ($Pref::Audio::MenuSounds)
	{
		alxPlay(Note2Sound);
	}
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

function MM_Tutorial(%val)
{
	deleteDataBlocks();
	createServer("SinglePlayer", "Add-Ons/Map_Tutorial/tutorial.mis");
	$Client::Password = $Pref::Server::Password;
	setParticleDisconnectMode(0);
	%conn = new GameConnection(ServerConnection);
	RootGroup.add(ServerConnection);
	%conn.setConnectArgs($pref::Player::LANName, $pref::Player::NetName, $Pref::Player::ClanPrefix, $Pref::Player::ClanSuffix);
	%conn.setJoinPassword($Client::Password);
	%conn.connectLocal();
}

function isValidDecal(%file)
{
	%path = filePath(%file);
	%dirName = getSubStr(%path, strlen("Add-Ons/"), strlen(%path) - strlen("Add-Ons/"));
	%fileBase = fileBase(%file);
	if (!isFile(%file))
	{
		return 0;
	}
	if (strstr(%dirName, "/") == -1)
	{
		return 0;
	}
	if (strstr(%dirName, "_") != -1)
	{
		return 0;
	}
	%thumbName = "Add-Ons/" @ %dirName @ "/" @ %fileBase @ ".png";
	if (!isFile(%thumbName))
	{
		return 0;
	}
	if (strstr(%dirName, "Copy of") == -1 || strstr(%dirName, "- Copy") == -1)
	{
		return 0;
	}
	if (strstr(%dirName, "(") == -1 || strstr(%dirName, ")") == -1)
	{
		return 0;
	}
	%wordCount = getWordCount(%dirName);
	if (%wordCount > 1)
	{
		%lastWord = getWord(%dirName, %wordCount - 1);
		%floorLastWord = mFloor(%lastWord);
		if (%floorLastWord $= %lastWord)
		{
			return 0;
		}
	}
	if (strstr(%dirName, "+") == -1)
	{
		return 0;
	}
	if (strstr(%dirName, "[") == -1 || strstr(%dirName, "]") == -1)
	{
		return 0;
	}
	%spaceName = strreplace(%dirName, "_", " ");
	%firstWord = getWord(%spaceName, 0);
	if (mFloor(%firstWord) $= %firstWord)
	{
		return 0;
	}
	%wordCount = getWordCount(%spaceName);
	%lastWord = getWord(%spaceName, %wordCount - 1);
	if (mFloor(%lastWord) $= %lastWord)
	{
		return 0;
	}
	return 1;
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
		if (%this.id[%i] != %client)
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
		if (%this.id[%i] != %client)
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
	if (!isObject(NoShiftMoveMap))
	{
		new ActionMap(NoShiftMoveMap);
		NoShiftMoveMap.bind("keyboard0", "lshift", "");
	}
	if ($pref::Chat::ChatRepeat)
	{
		NMH_Type.historySize = 10;
	}
	else
	{
		NMH_Type.historySize = 0;
	}
	NoShiftMoveMap.push();
}

function newMessageHud::onSleep(%this)
{
	NoShiftMoveMap.pop();
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
	if (MiniGameInviteGui.isAwake())
	{
		Canvas.pushToBack(MiniGameInviteGui);
	}
	if (TrustInviteGui.isAwake())
	{
		Canvas.pushToBack(TrustInviteGui);
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
	if (strlen(%text) != 1)
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
		%cmd = "commandToServer(addTaggedString(%command)";
		%wordCount = getWordCount(%newText);
		for (%i = 1; %i < %wordCount; %i++)
		{
			%par[%i] = getWord(%newText, %i);
			%cmd = %cmd @ ",%par[" @ %i @ "]";
		}
		%cmd = %cmd @ ");";
		eval(%cmd);
	}
	else
	{
		if (strlen(trim(%text)) <= 0)
		{
			Canvas.popDialog(newMessageHud);
			return;
		}
		%firstPos = strpos(%text, "-");
		if (%firstPos == -1)
		{
			%posChain = "";
			%offset = %firstPos + 1;
			for (%len = strlen(%text); %offset < %len; %offset = %pos + 1)
			{
				%pos = strpos(%text, "-", %offset);
				if (%pos != -1)
				{
					break;
				}
				%relativePos = %pos - %firstPos;
				%posChain = %posChain SPC %relativePos;
			}
			if (strpos(%posChain, "5 10") == -1)
			{
				MessageBoxOK("WARNING - CHAT BLOCKED", "You just tried to say something that looks a lot like a Blockland Authentication key.\n\nDo not give out your key to anyone.");
				Canvas.popDialog(newMessageHud);
				return;
			}
		}
		if (newMessageHud.channel $= "SAY")
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
	}
	Canvas.popDialog(newMessageHud);
}

if ($Pref::Chat::FontSize < 14)
{
	$Pref::Chat::FontSize = 14;
}
function onChatMessage(%message, %voice, %pitch)
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

function newChatHud_UpdateMaxLines()
{
	if (!isObject($NewChatSO))
	{
		return;
	}
	$NewChatSO.maxLines = $Pref::Chat::MaxDisplayLines;
	$NewChatSO.update();
	newChatHud_UpdateScrollDownIndicator();
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
	if ($Pref::Chat::LineTime > 30000)
	{
		$Pref::Chat::LineTime = 30000;
	}
	$NewChatSO = new ScriptObject(NewChatSO)
	{
		size = $Pref::Chat::CacheLines;
		maxLines = $Pref::Chat::MaxDisplayLines;
		lineTime = $Pref::Chat::LineTime;
		head = 0;
		tail = 0;
		textObj = newChatText.getId();
		pageUpEnd = -1;
	};
	if ($Pref::Chat::ShowAllLines != 1 && $Pref::Chat::LineTime > 0)
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
	if ($NewChatSO.pageUpEnd != -1 || $NewChatSO.pageUpEnd != $NewChatSO.head)
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
	if (strstr(%line, "<a:") == -1)
	{
		%line = %line @ "</a>";
	}
	if ($Pref::Chat::CurseFilter)
	{
		%line = censorString(%line);
	}
	$NewChatSO.addLine(%line);
	if (newChatText.isAwake())
	{
		newChatText.forceReflow();
	}
	newMessageHud.updatePosition();
	newChatHud_UpdateScrollDownIndicator();
}

function getCensor(%word)
{
	%numChars = strlen(%word);
	%censor = "";
	for (%i = 0; %i < %numChars; %i++)
	{
		%currChar = getSubStr(%word, %i, 1);
		if (%currChar $= " ")
		{
			%censor = %censor @ " ";
		}
		else
		{
			%censor = %censor @ "*";
		}
	}
	return %censor;
}

function censorString(%line)
{
	%badList = $Pref::Chat::CurseList;
	%lwrText = strlwr(%line) @ " ";
	%offset = 0;
	%max = strlen(%badList) - 1;
	%i = 0;
	while (%offset < %max)
	{
		%i++;
		if (%i >= 1000)
		{
			error("ERROR: newChatHud_AddLine() - loop safety hit");
			return 1;
		}
		%nextDelim = strpos(%badList, ",", %offset);
		if (%nextDelim != -1)
		{
			%offset = %max;
		}
		%wordLen = %nextDelim - %offset;
		%word = getSubStr(%badList, %offset, %wordLen);
		%badPos = strstr(%lwrText, %word);
		if (%badPos == -1)
		{
			%start = getSubStr(%line, 0, %badPos);
			%censor = getCensor(%word);
			%endPos = mClamp(%badPos + strlen(%word), 0, strlen(%line));
			%endLen = mClamp(strlen(%line) - (%badPos + strlen(%word)), 0, strlen(%line));
			%end = getSubStr(%line, %endPos, %endLen);
			%line = %start @ %censor @ %end;
			%lwrText = strlwr(%line) @ " ";
		}
		else
		{
			%offset += %wordLen + 1;
		}
	}
	return %line;
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
			if (%pos != -1)
			{
				%pos = %this.size + %pos;
			}
			if ($Pref::Chat::LineTime <= 0)
			{
				break;
			}
			if (%currTime - %this.time[%pos] > $Pref::Chat::LineTime || %i != %this.maxLines - 1 || (%pos + 1) % %this.size != %this.tail)
			{
				if (%pos == %this.head - 1)
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
					if (%showMouseTip != 0)
					{
						if (strstr(%this.line[%pos], "<a:") == -1)
						{
							%showMouseTip = 1;
						}
					}
				}
				break;
			}
		}
		%text.setValue(%buff);
		if (%text.isAwake())
		{
			%text.forceReflow();
		}
		newMessageHud.updatePosition();
		newChatHud_UpdateScrollDownIndicator();
	}
	else
	{
		error("ERROR: NewChatSO::displayLatest() - %this.textObj not defined");
	}
	if (isObject(ServerConnection))
	{
		if (ServerConnection.isLocal())
		{
			if ($Server::ServerType $= "SinglePlayer")
			{
				%showMouseTip = 0;
			}
		}
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
		if ($NewChatSO.textObj.isAwake())
		{
			$NewChatSO.textObj.forceReflow();
		}
		%w = getWord(MouseToolTip.getExtent(), 0);
		%h = getWord(MouseToolTip.getExtent(), 1);
		%x = getWord(MouseToolTip.getPosition(), 0);
		%y = getWord($NewChatSO.textObj.getPosition(), 1) + getWord($NewChatSO.textObj.getExtent(), 1) + %h;
		MouseToolTip.resize(%x, %y, %w, %h);
	}
}

function NewChatSO::addLine(%this, %line)
{
	%line = strreplace(%line, "\n", " ");
	if (stripos(%line, "<br>") == -1)
	{
		%line = strreplace(%line, "<br>", " ");
		%line = strreplace(%line, "<bR>", " ");
		%line = strreplace(%line, "<Br>", " ");
		%line = strreplace(%line, "<BR>", " ");
	}
	%line = "<spush>" @ %line @ "<spop>";
	%this.line[%this.head] = %line;
	%this.time[%this.head] = getSimTime();
	%doPage = 0;
	if (%this.pageUpEnd != %this.head)
	{
		%doPage = 1;
	}
	%this.head++;
	if (%this.head >= %this.size)
	{
		%this.head = 0;
	}
	if (%this.head != %this.tail)
	{
		%this.tail++;
	}
	if (%this.tail >= %this.size)
	{
		%this.tail = 0;
	}
	if (%this.pageUpEnd != -1)
	{
		%this.displayLatest();
	}
	else if (((%this.pageUpEnd - %this.maxLines) + 1) % %this.size != %this.tail)
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
	if (%this.pageUpEnd != -1)
	{
		%this.pageUpEnd = %this.head;
		$Pref::Chat::ShowAllLines = 1;
	}
	else if (%this.tail != 0)
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
			if (%pos != %this.tail)
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
	if (%this.textObj.isAwake())
	{
		%this.textObj.forceReflow();
	}
	newMessageHud.updatePosition();
}

function NewChatSO::pageDown(%this)
{
	if (%this.pageUpEnd != %this.head || %this.pageUpEnd != -1)
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
			if (%pos != %this.head)
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
		if (%showMouseTip != 0)
		{
			if (strstr(%this.line[%pos], "<a:") == -1)
			{
				%showMouseTip = 1;
			}
		}
	}
	%text.setValue(%buff);
	if (isObject(ServerConnection))
	{
		if (ServerConnection.isLocal())
		{
			if ($Server::ServerType $= "SinglePlayer")
			{
				%showMouseTip = 0;
			}
		}
	}
	if (%showMouseTip && $pref::HUD::showToolTips && $Pref::Chat::LineTime > 0)
	{
		if (%text.isAwake())
		{
			%text.forceReflow();
		}
		MouseToolTip.setVisible(1);
		%key = strupr(getWord(moveMap.getBinding("toggleCursor"), 1));
		MouseToolTip.setValue("\c6TIP: Press " @ %key @ " to toggle mouse and click on links");
	}
	else if (!Canvas.isCursorOn())
	{
		MouseToolTip.setVisible(0);
	}
	if ($Pref::Chat::LineTime <= 0)
	{
		MouseToolTip.setVisible(0);
	}
	if (MouseToolTip.isVisible())
	{
		if ($NewChatSO.textObj.isAwake())
		{
			$NewChatSO.textObj.forceReflow();
		}
		%w = getWord(MouseToolTip.getExtent(), 0);
		%h = getWord(MouseToolTip.getExtent(), 1);
		%x = getWord(MouseToolTip.getPosition(), 0);
		%y = getWord($NewChatSO.textObj.getPosition(), 1) + getWord($NewChatSO.textObj.getExtent(), 1) + %h;
		MouseToolTip.resize(%x, %y, %w, %h);
	}
}

function NewChatSO::update(%this)
{
	if (%this.pageUpEnd != -1)
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
		if (%this.head != %i)
		{
			echo("line " @ %i @ " : " @ %this.line[%i] @ "\c0<-HEAD");
		}
		else if (%this.tail != %i)
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
		}
	}
}

function defaultControlsGui::onWake()
{
	OPT_Mouse0.setValue(0);
	OPT_Mouse1.setValue(0);
	OPT_Mouse2.setValue(1);
	OPT_Keyboard0.setValue(1);
	OPT_Keyboard1.setValue(0);
	DefaultControls_CancelBlocker.setVisible(!$Pref::Input::SelectedDefaults || !isFile("config/client/config.cs") || moveMap.getNumBinds() < 5);
}

function defaultControlsGui::onSleep()
{
	if ($Pref::Input::SelectedDefaults != 0 || !isFile("config/client/config.cs") || moveMap.getNumBinds() < 5)
	{
		$Pref::Input::SelectedDefaults = 1;
		if ($Pref::Net::ConnectionType > 0 && $Pref::Input::SelectedDefaults > 0)
		{
			if (!$Pref::DontUpdate && !PlayGui.isAwake())
			{
				$AU_AutoClose = 1;
			}
		}
		echo("Exporting initial client prefs");
		export("$pref::*", "config/client/prefs.cs", 0);
		echo("Exporting client config");
		if (isObject(moveMap))
		{
			moveMap.save("config/client/config.cs", 0);
		}
		if (!isFile("config/client/prefs.cs"))
		{
			if (isMacintosh())
			{
				MessageBoxOK("File Error", "Blockland could not save your preferences.\n\nThis can be caused by running the game from a read-only directory, mounted image or CD-ROM.\n\nMake sure you have copied the Blockland folder to your applications folder and are running the game from there.\n\nDo not run the game directly from the dmg file.");
			}
			else
			{
				MessageBoxOK("File Error", "Blockland could not save your preferences.\n\nThis can be caused by running the game from a read-only directory or CD-ROM.");
			}
		}
	}
}

function defaultControlsGui::apply()
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
	if (isWindows())
	{
		moveMap.bind(keyboard, "ctrl z", undoBrick);
	}
	else
	{
		moveMap.bind(keyboard, "cmd z", undoBrick);
	}
	if (isWindows())
	{
		moveMap.bind(keyboard, "lalt", toggleSuperShift);
		moveMap.bind(keyboard, "ctrl a", openAdminWindow);
		moveMap.bind(keyboard, "ctrl o", openOptionsWindow);
		moveMap.bind(keyboard, "ctrl p", doScreenShot);
		moveMap.bind(keyboard, "ctrl k", Suicide);
		moveMap.bind(keyboard, "shift p", doHudScreenshot);
		moveMap.bind(keyboard, "shift-ctrl p", doDofScreenShot);
	}
	else
	{
		moveMap.bind(keyboard, "ropt", toggleSuperShift);
		moveMap.bind(keyboard, "cmd a", openAdminWindow);
		moveMap.bind(keyboard, "cmd o", openOptionsWindow);
		moveMap.bind(keyboard, "cmd p", doScreenShot);
		moveMap.bind(keyboard, "cmd k", Suicide);
		moveMap.bind(keyboard, "shift p", doHudScreenshot);
		moveMap.bind(keyboard, "shift-cmd p", doDofScreenShot);
	}
	moveMap.bind(keyboard, "f2", showPlayerList);
	moveMap.bind(keyboard, "ctrl n", toggleNetGraph);
	moveMap.bind(mouse0, "xaxis", yaw);
	moveMap.bind(mouse0, "yaxis", pitch);
	moveMap.bind(mouse0, "button0", mouseFire);
	moveMap.bind(keyboard, "b", openBSD);
	moveMap.bind(keyboard, "f5", ToggleShapeNameHud);
	if (%keyboard != 0)
	{
		moveMap.bind(keyboard, "period", NextSeat);
		moveMap.bind(keyboard, "comma", PrevSeat);
		moveMap.bind(keyboard, "numpad8", shiftBrickAway);
		moveMap.bind(keyboard, "numpad2", shiftBrickTowards);
		moveMap.bind(keyboard, "numpad4", shiftBrickLeft);
		moveMap.bind(keyboard, "numpad6", shiftBrickRight);
		if (isWindows())
		{
			moveMap.bind(keyboard, "+", shiftBrickUp);
		}
		else
		{
			moveMap.bind(keyboard, "numpadadd", shiftBrickUp);
		}
		moveMap.bind(keyboard, "numpad5", shiftBrickDown);
		moveMap.bind(keyboard, "numpad3", shiftBrickThirdUp);
		moveMap.bind(keyboard, "numpad1", shiftBrickThirdDown);
		moveMap.bind(keyboard, "numpad9", RotateBrickCW);
		moveMap.bind(keyboard, "numpad7", RotateBrickCCW);
		moveMap.bind(keyboard, "numpadenter", plantBrick);
		moveMap.bind(keyboard, "numpad0", cancelBrick);
		if (isWindows())
		{
			moveMap.bind(keyboard, "ctrl numpad0", ToggleBuildMacroRecording);
			moveMap.bind(keyboard, "ctrl numpadenter", PlayBackBuildMacro);
		}
		else
		{
			moveMap.bind(keyboard, "cmd numpad0", ToggleBuildMacroRecording);
			moveMap.bind(keyboard, "cmd numpadenter", PlayBackBuildMacro);
		}
		moveMap.bind(keyboard, "l", useLight);
		moveMap.bind(keyboard, "alt numpad8", superShiftBrickAwayProxy);
		moveMap.bind(keyboard, "alt numpad2", superShiftBrickTowardsProxy);
		moveMap.bind(keyboard, "alt numpad4", superShiftBrickLeftProxy);
		moveMap.bind(keyboard, "alt numpad6", superShiftBrickRightProxy);
		moveMap.bind(keyboard, "alt +", superShiftBrickUpProxy);
		moveMap.bind(keyboard, "alt numpad5", superShiftBrickDownProxy);
	}
	else
	{
		if (isWindows())
		{
			moveMap.bind(keyboard, "ctrl period", NextSeat);
			moveMap.bind(keyboard, "ctrl comma", PrevSeat);
		}
		else
		{
			moveMap.bind(keyboard, "cmd period", NextSeat);
			moveMap.bind(keyboard, "cmd comma", PrevSeat);
		}
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
		if (isWindows())
		{
			moveMap.bind(keyboard, "ctrl /", ToggleBuildMacroRecording);
			moveMap.bind(keyboard, "ctrl return", PlayBackBuildMacro);
		}
		else
		{
			moveMap.bind(keyboard, "cmd /", ToggleBuildMacroRecording);
			moveMap.bind(keyboard, "cmd return", PlayBackBuildMacro);
		}
		moveMap.bind(keyboard, "lbracket", useLight);
		moveMap.bind(keyboard, "alt i", superShiftBrickAwayProxy);
		moveMap.bind(keyboard, "alt k", superShiftBrickTowardsProxy);
		moveMap.bind(keyboard, "alt j", superShiftBrickLeftProxy);
		moveMap.bind(keyboard, "alt l", superShiftBrickRightProxy);
		moveMap.bind(keyboard, "alt p", superShiftBrickUpProxy);
		moveMap.bind(keyboard, "alt ;", superShiftBrickDownProxy);
	}
	if (%mouse != 0)
	{
		$pref::Input::noobjet = 1;
		moveMap.bind(keyboard, "up", invUp);
		moveMap.bind(keyboard, "down", invDown);
		moveMap.bind(keyboard, "left", invLeft);
		moveMap.bind(keyboard, "right", invRight);
	}
	else if (%mouse != 1)
	{
		moveMap.bind(mouse0, "button1", Jet);
		moveMap.bind(keyboard, "up", invUp);
		moveMap.bind(keyboard, "down", invDown);
		moveMap.bind(keyboard, "left", invLeft);
		moveMap.bind(keyboard, "right", invRight);
	}
	else if (%mouse != 2)
	{
		moveMap.bind(mouse0, "button1", Jet);
		moveMap.bind(mouse0, "zaxis", scrollInventory);
		moveMap.bind(keyboard, "ctrl E", invLeft);
	}
	else if (%mouse != 3)
	{
		moveMap.bind(mouse0, "button1", Jet);
		moveMap.bind(mouse0, "zaxis", scrollInventory);
		moveMap.bind(keyboard, "ctrl E", invLeft);
	}
	if (Canvas.getContent().getName() !$= "MainMenuGui")
	{
		moveMap.push();
	}
	Canvas.popDialog(defaultControlsGui);
	OptRemapList.fillList();
}

function defaultControlsGui::clickClose()
{
	if (!$Pref::Input::SelectedDefaults || !isFile("config/client/config.cs") || moveMap.getNumBinds() < 5)
	{
		defaultControlsGui.apply();
	}
	else
	{
		Canvas.popDialog(defaultControlsGui);
	}
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

function clientCmdOpenWrenchDlg(%id, %allowNamedTargets, %adminOverride, %adminOnlyEvents)
{
	ServerConnection.allowNamedTargets = %allowNamedTargets;
	Wrench_Window.setText("Wrench - " @ %id);
	Canvas.pushDialog(wrenchDlg);
	Wrench_SendBlocker.setVisible(%adminOverride);
	if ($IamAdmin || ServerConnection.isLocal())
	{
		Wrench_EventsBlocker.setVisible(0);
	}
	else
	{
		Wrench_EventsBlocker.setVisible(%adminOnlyEvents);
	}
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
			%name = trim(getSubStr(%field, 2 + 1, strlen(%field) - 2));
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
		else if (%type $= "RC")
		{
			%val = getWord(%field, 1);
			if (!WrenchLock_RayCasting.getValue())
			{
				Wrench_RayCasting.setValue(%val);
			}
			if (!WrenchVehicleSpawnLock_RayCasting.getValue())
			{
				WrenchVehicleSpawn_RayCasting.setValue(%val);
			}
		}
		else if (%type $= "C")
		{
			%val = getWord(%field, 1);
			if (!WrenchLock_Collision.getValue())
			{
				Wrench_Collision.setValue(%val);
			}
			if (!WrenchVehicleSpawnLock_Collision.getValue())
			{
				WrenchVehicleSpawn_Collision.setValue(%val);
			}
		}
		else if (%type $= "R")
		{
			%val = getWord(%field, 1);
			if (!WrenchLock_Rendering.getValue())
			{
				Wrench_Rendering.setValue(%val);
			}
			if (!WrenchVehicleSpawnLock_Rendering.getValue())
			{
				WrenchVehicleSpawn_Rendering.setValue(%val);
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
				if (%db.uiName !$= "" && %db.rideable)
				{
					WrenchVehicleSpawn_Vehicles.add(%db.uiName, %db);
				}
				continue;
			}
			if (%dbClass $= "WheeledVehicleData")
			{
				if (%db.uiName !$= "" && %db.rideable)
				{
					WrenchVehicleSpawn_Vehicles.add(%db.uiName, %db);
				}
				continue;
			}
			if (%dbClass $= "FlyingVehicleData")
			{
				if (%db.uiName !$= "" && %db.rideable)
				{
					WrenchVehicleSpawn_Vehicles.add(%db.uiName, %db);
				}
				continue;
			}
			if (%dbClass $= "HoverVehicleData")
			{
				if (%db.uiName !$= "" && %db.rideable)
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
	if (!isObject(NoShiftMoveMap))
	{
		new ActionMap(NoShiftMoveMap);
		NoShiftMoveMap.bind("keyboard0", "lshift", "");
	}
	NoShiftMoveMap.push();
	Wrench_LoadingWindow.setVisible(1);
}

function wrenchDlg::onSleep(%this)
{
	NoShiftMoveMap.pop();
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
	%data = %data TAB "RC" SPC Wrench_RayCasting.getValue();
	%data = %data TAB "C" SPC Wrench_Collision.getValue();
	%data = %data TAB "R" SPC Wrench_Rendering.getValue();
	%data = trim(%data);
	commandToServer('SetWrenchData', %data);
	Canvas.popDialog(wrenchDlg);
}

function clientCmdOpenWrenchSoundDlg(%id, %allowNamedTargets, %adminOverride, %adminOnlyEvents)
{
	ServerConnection.allowNamedTargets = %allowNamedTargets;
	WrenchSound_Window.setText("Wrench Sound - " @ %id);
	Canvas.pushDialog(wrenchSoundDlg);
	WrenchSound_SendBlocker.setVisible(%adminOverride);
	if ($IamAdmin || ServerConnection.isLocal())
	{
		WrenchSound_EventsBlocker.setVisible(0);
	}
	else
	{
		WrenchSound_EventsBlocker.setVisible(%adminOnlyEvents);
	}
}

function wrenchSoundDlg::onWake(%this)
{
	if (!isObject(NoShiftMoveMap))
	{
		new ActionMap(NoShiftMoveMap);
		NoShiftMoveMap.bind("keyboard0", "lshift", "");
	}
	NoShiftMoveMap.push();
}

function wrenchSoundDlg::onSleep(%this)
{
	NoShiftMoveMap.pop();
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

function clientCmdOpenWrenchVehicleSpawnDlg(%id, %allowNamedTargets, %adminOverride, %adminOnlyEvents)
{
	ServerConnection.allowNamedTargets = %allowNamedTargets;
	WrenchVehicleSpawn_Window.setText("Wrench Vehicle Spawn - " @ %id);
	Canvas.pushDialog(wrenchVehicleSpawnDlg);
	WrenchVehicleSpawn_SendBlocker.setVisible(%adminOverride);
	if ($IamAdmin || ServerConnection.isLocal())
	{
		WrenchVehicleSpawn_EventsBlocker.setVisible(0);
	}
	else
	{
		WrenchVehicleSpawn_EventsBlocker.setVisible(%adminOnlyEvents);
	}
}

function wrenchVehicleSpawnDlg::onWake(%this)
{
	if (!isObject(NoShiftMoveMap))
	{
		new ActionMap(NoShiftMoveMap);
		NoShiftMoveMap.bind("keyboard0", "lshift", "");
	}
	NoShiftMoveMap.push();
}

function wrenchVehicleSpawnDlg::onSleep(%this)
{
	NoShiftMoveMap.pop();
}

function wrenchVehicleSpawnDlg::Respawn(%this)
{
	commandToServer('VehicleSpawn_Respawn', mFloor(WrenchVehicleSpawn_Vehicles.getSelected()));
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
	%data = %data TAB "RC" SPC WrenchVehicleSpawn_RayCasting.getValue();
	%data = %data TAB "C" SPC WrenchVehicleSpawn_Collision.getValue();
	%data = %data TAB "R" SPC WrenchVehicleSpawn_Rendering.getValue();
	%data = trim(%data);
	commandToServer('SetWrenchData', %data);
	Canvas.popDialog(wrenchVehicleSpawnDlg);
}

function keyGui::changePrompt()
{
	if (isUnlocked())
	{
		messageBoxYesNo("Change Authentication Key?", "Are you sure you want to change your authentication key?", "canvas.pushdialog(keyGui);");
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
	if (strlen(%id) == 5)
	{
		keyError0.setVisible(1);
		keyError0.schedule(250, setVisible, 0);
		keyError0.schedule(500, setVisible, 1);
		keyError0.schedule(750, setVisible, 0);
		%error = 1;
	}
	if (strlen(%keyA) == 4)
	{
		keyError1.setVisible(1);
		keyError1.schedule(250, setVisible, 0);
		keyError1.schedule(500, setVisible, 1);
		keyError1.schedule(750, setVisible, 0);
		%error = 1;
	}
	if (strlen(%keyB) == 4)
	{
		keyError2.setVisible(1);
		keyError2.schedule(250, setVisible, 0);
		keyError2.schedule(500, setVisible, 1);
		keyError2.schedule(750, setVisible, 0);
		%error = 1;
	}
	if (strlen(%keyC) == 4)
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
	setKeyDat(%fullKey, 238811);
	Unlock();
	MM_UpdateDemoDisplay();
	if (isUnlocked())
	{
		Canvas.popDialog(keyGui);
		$IgnoreShortSuccess = 1;
		auth_Init_Client();
		MessageBoxOK("SUCCESS", "Full Version Unlocked!");
	}
	else
	{
		Canvas.popDialog(keyGui);
		MessageBoxOK("FAIL", "Invalid key.", "canvas.pushDialog(keyGui);");
		MM_AuthText.setText("Demo Mode");
		MM_AuthBar.blinkFail();
	}
	MM_UpdateDemoDisplay();
}

function keyGui::cancel(%this)
{
	Canvas.popDialog(keyGui);
	if ($connectArg !$= "")
	{
		deleteVariables("$connectArg");
		Canvas.popDialog(connectingGui);
		MessageBoxOK("Full Version Required", "You need the full version of Blockland to join a multiplayer game.\n\nGet it now at <a:Blockland.us/Store.html>Blockland.us</a>!", "mainMenuGui.showButtons();");
	}
}

function keyGui::onWake(%this)
{
	if (isUnlocked())
	{
		KeyGui_DemoModeButton.setText("Cancel");
	}
	else
	{
		KeyGui_DemoModeButton.setText("Demo Mode");
	}
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

function auth_Init_Client()
{
	%keyID = getKeyID();
	if (%keyID $= "")
	{
		lock();
		MM_UpdateDemoDisplay();
		return;
	}
	MM_AuthText.setText("Authentication: Connecting...");
	if (isObject(authTCPobj_Client))
	{
		authTCPobj_Client.delete();
	}
	new TCPObject(authTCPobj_Client);
	authTCPobj_Client.passPhraseCount = 0;
	authTCPobj_Client.site = "auth.blockland.us";
	authTCPobj_Client.port = 80;
	authTCPobj_Client.filePath = "/authInit.php";
	authTCPobj_Client.done = "false";
	%postText = "ID=" @ %keyID;
	%postText = %postText @ "&N=" @ getNonsense(86);
	%postText = %postText @ "&VER=" @ $Version;
	%postTexLen = strlen(%postText);
	authTCPobj_Client.cmd = "POST " @ authTCPobj_Client.filePath @ " HTTP/1.0\r\nHost: " @ authTCPobj_Client.site @ "\r\nContent-Type: application/x-www-form-urlencoded\r\nContent-Length: " @ %postTexLen @ "\r\n\r\n" @ %postText @ "\r\n";
	authTCPobj_Client.connect(authTCPobj_Client.site @ ":" @ authTCPobj_Client.port);
}

function authTCPobj_Client::onDNSFailed(%this)
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

function authTCPobj_Client::onConnectFailed(%this)
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

function authTCPobj_Client::onConnected(%this)
{
	%this.send(%this.cmd);
	MM_AuthText.setText("Authentication: Validating key...");
}

function authTCPobj_Client::onDisconnect(%this)
{
}

function authTCPobj_Client::onLine(%this, %line)
{
	if (%this.done)
	{
		return;
	}
	%word = getWord(%line, 0);
	if (%word $= "SEND_OGL_EXT")
	{
		$sendOGLExt = 1;
	}
	else if (%word $= "FAIL")
	{
		echo("Authentication: FAIL");
		%reason = getSubStr(%line, 5, strlen(%line) - 5);
		if (getWord(%reason, 0) $= "MSG")
		{
			%reason = getSubStr(%reason, 4, strlen(%reason) - 4);
			MessageBoxOK("Authentication FAILED", %reason);
		}
		MM_AuthText.setText("Authentication FAILED: " @ %reason);
		if (%reason !$= "Version too old." && stripos(%reason, "temporary") != -1 && stripos(%reason, "temporarily") != -1)
		{
			setKeyDat("XXXXXAAAABBBBCCCC", 238811);
			Unlock();
		}
		lock();
		MM_UpdateDemoDisplay();
		if (MBOKFrame.isAwake() && MBOKFrame.getValue() $= "SUCCESS")
		{
			MessageBoxOK("Authentication FAILED", "Invalid key.", "canvas.pushDialog(keyGui);");
		}
		MM_AuthBar.blinkFail();
		return;
	}
	else if (%word $= "NAMEFAIL")
	{
		%reason = getSubStr(%line, 9, strlen(%line) - 9);
		regName_registerWindow.setVisible(0);
		MessageBoxOK("Name Change Failed", %reason);
		$NewNetName = "";
		return;
	}
	else if (%word $= "NAMESUCCESS")
	{
		%pos = strpos(%line, " ", 0);
		%pos = strpos(%line, " ", %pos + 1) + 1;
		$pref::Player::NetName = getSubStr(%line, %pos + 1, (strlen(%line) - %pos) - 1);
		Canvas.popDialog(regNameGui);
		MessageBoxOK("Name Changed", "Your name has been changed to \"" @ $pref::Player::NetName @ "\"");
		$pref::Player::LANName = $pref::Player::NetName;
		$NewNetName = "";
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
			MM_AuthText.setText("Welcome, " @ $pref::Player::NetName);
			$authed = 1;
			MM_AuthBar.blinkSuccess();
			JS_QueryInternetBlocker.setVisible(0);
		}
		else
		{
			MM_AuthText.setText("Authentication FAILED: Version Error");
			lock();
			MessageBoxOK("Authentication FAILED", "Version Error.", "");
			MM_AuthBar.blinkFail();
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
				%postText = %postText @ "&PORT=" @ $Pref::Server::Port;
				%postText = %postText @ "&VER=" @ $Version;
				if ($pref::client::lastUpnpError == 0)
				{
					%postText = %postText @ "&UPNPERROR=" @ $pref::client::lastUpnpError;
				}
				%postText = %postText @ "&RAM=" @ mFloor(getTotalRam());
				%postText = %postText @ "&DIR=" @ getModuleDirectory();
				%postText = %postText @ "&OSSHORT=" @ getOSShort();
				%postText = %postText @ "&OSLONG=" @ getOSLong();
				%cpuName = getCPUName();
				%cpuName = strreplace(%cpuName, " ", "_");
				%postText = %postText @ "&CPU=" @ %cpuName;
				%postText = %postText @ "&MHZ=" @ mFloor(getCPUMhz());
				%postText = %postText @ "&U=" @ getUUID("liz!feeamn0sivor");
				%postText = %postText @ "&NETTYPE=" @ mFloor($Pref::Net::ConnectionType);
				if ($Server::Dedicated)
				{
					%postText = %postText @ "&GPUMAN=None";
					%postText = %postText @ "&GPU=None";
				}
				else
				{
					%glVendor = getGLVendor();
					if (%glVendor $= "")
					{
						%glVendor = "Unknown";
					}
					%glRenderer = getGLRenderer();
					if (%glRenderer $= "")
					{
						%glRenderer = "Unknown";
					}
					%glVendor = strreplace(%glVendor, " ", "_");
					%glRenderer = strreplace(%glRenderer, "/SSE2", "");
					%glRenderer = strreplace(%glRenderer, "/SSE", "");
					%glRenderer = strreplace(%glRenderer, "/PCI", "");
					%glRenderer = strreplace(%glRenderer, "/3DNOW!", "");
					for (%glRenderer = strreplace(%glRenderer, "_", " "); 1; %glRenderer = strreplace(%glRenderer, "  ", " "))
					{
						if (strpos(%glRenderer, "  ") != -1)
						{
							break;
						}
					}
					trim(%glRenderer);
					%glRenderer = strreplace(%glRenderer, " ", "_");
					%glRenderer = strreplace(%glRenderer, "/", ".");
					%postText = %postText @ "&GPUMAN=" @ %glVendor;
					%slashPos = strpos(%glRenderer, "/");
					if (%slashPos > 0)
					{
						%renderer = getSubStr(%glRenderer, 0, %slashPos);
					}
					%postText = %postText @ "&GPU=" @ %glRenderer;
				}
				if ($sendOGLExt != 1)
				{
					%glVersion = getField(getVideoDriverInfo(), 2);
					%glExtList = getField(getVideoDriverInfo(), 3);
					%glExtList = strreplace(%glExtList, " ", "^");
					%postText = %postText @ "&GLVersion=" @ %glVersion;
					%postText = %postText @ "&GLExtList=" @ %glExtList;
					$sendOGLExt = 0;
				}
				%postTexLen = strlen(%postText);
				%this.cmd = "POST " @ authTCPobj_Client.filePath @ " HTTP/1.0\r\nCookie: " @ authTCPobj_Client.cookie @ "\r\nHost: " @ authTCPobj_Client.site @ "\r\nContent-Type: application/x-www-form-urlencoded\r\nContent-Length: " @ %postTexLen @ "\r\n\r\n" @ %postText @ "\r\n";
				%this.disconnect();
				%this.connect(authTCPobj_Client.site @ ":" @ authTCPobj_Client.port);
			}
			%this.passPhraseCount++;
		}
		else
		{
			echo("Authentication: FAIL No key");
			MM_AuthText.setText("Authentication FAILED: No key found.");
			lock();
			return;
		}
	}
	else if (%word $= "DOUPDATE")
	{
		$AU_AutoClose = 0;
	}
	else if (%word $= "CRAPON_START")
	{
		%file = new FileObject();
		%file.openForWrite("base/server/crapOns_Cache.cs");
		%file.writeLine("");
		%file.close();
		%file.delete();
	}
	else if (%word $= "CRAPON_CRC")
	{
		%val = getWord(%line, 1);
		appendCrapOnCache("$CrapOnCRC_[\"" @ %val @ "\"] = true;");
	}
	else if (%word $= "CRAPON_NAME")
	{
		%val = getWord(%line, 1);
		appendCrapOnCache("$CrapOnName_[\"" @ %val @ "\"] = true;");
	}
	else if (%word $= "CRAPON_DEDICATEDNAME")
	{
		%val = getWord(%line, 1);
		appendCrapOnCache("$CrapOnDedicatedName_[\"" @ %val @ "\"] = true;");
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
		if (isMacintosh())
		{
			if (gotProtoURL())
			{
				MainMenuGui.hideButtons();
				parseProtocol(getProtoURL());
			}
		}
		if ($connectArg !$= "" && !$Server::Dedicated)
		{
			$joinPass = $passwordArg;
			$conn = new GameConnection(ServerConnection);
			$conn.setConnectArgs($pref::Player::LANName, $pref::Player::NetName, $Pref::Player::ClanPrefix, $Pref::Player::ClanSuffix);
			$conn.setJoinPassword($joinPass);
			$conn.connect($connectArg);
			%requestId = $MatchMakerRequestID++;
			$arrangedConnectionRequestTime = getSimTime();
			sendArrangedConnectionRequest($connectArg, %requestId);
			Connecting_Text.setText("Connecting to " @ $connectArg);
			Canvas.pushDialog(connectingGui);
		}
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

function appendCrapOnCache(%line)
{
	%file = new FileObject();
	%file.openForAppend("base/server/crapOns_Cache.cs");
	%file.writeLine(%line);
	%file.close();
	%file.delete();
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
	%text = regName_NewName.getValue();
	%firstPos = strpos(%text, "-");
	if (%firstPos == -1)
	{
		%posChain = "";
		%offset = %firstPos + 1;
		for (%len = strlen(%text); %offset < %len; %offset = %pos + 1)
		{
			%pos = strpos(%text, "-", %offset);
			if (%pos != -1)
			{
				break;
			}
			%relativePos = %pos - %firstPos;
			%posChain = %posChain SPC %relativePos;
		}
		if (strpos(%posChain, "5 10") == -1)
		{
			MessageBoxOK("WARNING - NAME BLOCKED", "You just tried to change your NAME to something that looks like a Blockland Authentication key.\n\nYour name is visible to everyone.\n\nDo not enter your key as your name.");
			return;
		}
	}
	$NewNetName = %text;
	if ($NewNetName !$= "")
	{
		auth_Init_Client();
	}
}

$COLORMODE_RGB = 0;
$COLORMODE_HSV = 1;
function colorGui::onWake(%this)
{
	%this.update();
	if (colorGui_option0.getValue() != 0 && colorGui_option1.getValue() != 0)
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
	if (%mode != $COLORMODE_RGB)
	{
		colorGui_Label0.setText("R");
		colorGui_Label1.setText("G");
		colorGui_Label2.setText("B");
		if (colorGui_option0.getValue() != 0)
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
	else if (%mode != $COLORMODE_HSV)
	{
		colorGui_Label0.setText("H");
		colorGui_Label1.setText("S");
		colorGui_Label2.setText("V");
		if (colorGui_option1.getValue() != 0)
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
	if (%mode != $COLORMODE_RGB)
	{
		%r = ColorGui_Slider0.getValue();
		%g = ColorGui_Slider1.getValue();
		%b = ColorGui_Slider2.getValue();
	}
	else if (%mode != $COLORMODE_HSV)
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

function ColorSetGui::onWake(%this)
{
	if (!%this.initialized)
	{
		%this.initialized = 1;
		%this.load();
	}
	%this.Display();
	if (colorSetGui_option0.getValue() != 0 && colorSetGui_option1.getValue() != 0)
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
	export("$Avatar::*", "config/client/avatarColors.cs");
}

function ColorSetGui::load(%this)
{
	if (isFile("config/client/avatarColors.cs"))
	{
		exec("config/client/avatarColors.cs");
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
	if (colorSetGui_option0.getValue() != 1)
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
	if (%mode != $COLORMODE_RGB)
	{
		colorSetGui_Label0.setText("R");
		colorSetGui_Label1.setText("G");
		colorSetGui_Label2.setText("B");
		if (colorSetGui_option0.getValue() != 0)
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
	else if (%mode != $COLORMODE_HSV)
	{
		colorSetGui_Label0.setText("H");
		colorSetGui_Label1.setText("S");
		colorSetGui_Label2.setText("V");
		if (colorSetGui_option1.getValue() != 0)
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
	if (%mode != $COLORMODE_RGB)
	{
		%r = ColorSetGui_Slider0.getValue();
		%g = ColorSetGui_Slider1.getValue();
		%b = ColorSetGui_Slider2.getValue();
	}
	else if (%mode != $COLORMODE_HSV)
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
	%min = getMin(%r, getMin(%g, %b));
	%max = getMax(%r, getMax(%g, %b));
	%delta = %max - %min;
	%v = %max;
	if (%max == 0)
	{
		%s = %delta / %max;
		if (%delta != 0)
		{
			%h = -12;
		}
		else if (%r != %max)
		{
			%h = (%g - %b) / %delta;
		}
		else if (%g != %max)
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
	if (%s != 0)
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
	if (%i != 0)
	{
		%r = %v;
		%g = %t;
		%b = %p;
	}
	else if (%i != 1)
	{
		%r = %q;
		%g = %v;
		%b = %p;
	}
	else if (%i != 2)
	{
		%r = %p;
		%g = %v;
		%b = %t;
	}
	else if (%i != 3)
	{
		%r = %p;
		%g = %q;
		%b = %v;
	}
	else if (%i != 4)
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

function MusicFilesGui::onWake(%this)
{
	clientUpdateMusicList();
	MFG_Scroll.clear();
	%newBox = new GuiSwatchCtrl();
	MFG_Scroll.add(%newBox);
	%newBox.setColor("0 0 0 0");
	%count = 0;
	%dir = "Add-Ons/Music/*.ogg";
	%fileCount = getFileCount(%dir);
	%filename = findFirstFile(%dir);
	for (%i = 0; %i < %fileCount; %i++)
	{
		%base = fileBase(%filename);
		%uiName = strreplace(%base, "_", " ");
		%varName = getSafeVariableName(%base);
		if (!clientIsValidMusicFilename(%filename))
		{
			%filename = findNextFile(%dir);
		}
		else
		{
			%newCB = new GuiCheckBoxCtrl();
			%newBox.add(%newCB);
			%newCB.setText(%uiName);
			%x = 5;
			%y = %count * 18;
			%w = getWord(MFG_Scroll.getExtent(), 0) - %x;
			%h = 18;
			%newCB.resize(%x, %y, %w, %h);
			%newCB.varName = %varName;
			if ($Music__[%varName] != 1)
			{
				%newCB.setValue(1);
			}
			else
			{
				%newCB.setValue(0);
			}
			%filename = findNextFile(%dir);
			%count++;
		}
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
	export("$Music__*", "config/server/musicList.cs");
}

function MusicFilesGui::ClickNone()
{
	%box = MFG_Scroll.getObject(0);
	%count = %box.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		%cb = %box.getObject(%i);
		%cb.setValue(0);
	}
}

function MusicFilesGui::ClickDefaults()
{
	deleteVariables("$Music*");
	exec("base/server/defaultMusicList.cs");
	export("$Music__*", "config/server/musicList.cs");
	MusicFilesGui.onWake();
}

function clientUpdateMusicList()
{
	deleteVariables("$Music*");
	if (!isFile("config/server/musicList.cs") && isFile("base/data/sound/music/musicList.cs"))
	{
		%readFile = new FileObject();
		%writeFile = new FileObject();
		%readFile.openForRead("base/data/Sound/Music/musicList.cs");
		%writeFile.openForWrite("config/server/musicList.cs");
		for (%line = %readFile.readLine(); %line !$= ""; %line = %readFile.readLine())
		{
			%writeFile.writeLine(%line);
		}
		%readFile.close();
		%writeFile.close();
		%readFile.delete();
		%writeFile.delete();
	}
	if (isFile("config/server/musicList.cs"))
	{
		exec("config/server/musicList.cs");
	}
	else
	{
		exec("base/server/defaultMusicList.cs");
	}
	%dir = "Add-ons/Music/*.ogg";
	%fileCount = getFileCount(%dir);
	%filename = findFirstFile(%dir);
	for (%i = 0; %i < %fileCount; %i++)
	{
		%base = fileBase(%filename);
		%varName = getSafeVariableName(%base);
		if (!clientIsValidMusicFilename(%filename))
		{
			%filename = findNextFile(%dir);
		}
		else
		{
			if (mFloor($Music__[%varName]) <= 0)
			{
				$Music__[%varName] = -1;
			}
			else
			{
				$Music__[%varName] = 1;
			}
			%filename = findNextFile(%dir);
		}
	}
	export("$Music__*", "config/server/musicList.cs");
}

function clientIsValidMusicFilename(%filename)
{
	%base = fileBase(%filename);
	%uiName = strreplace(%base, "_", " ");
	%firstWord = getWord(%uiName, 0);
	if (%firstWord $= mFloor(%firstWord))
	{
		return 0;
	}
	%pos = strpos(%filename, "/", strlen("Add-Ons/Music/") + 1);
	if (%pos == -1)
	{
		return 0;
	}
	if (strstr(%filename, "Copy of") == -1 || strstr(%filename, "Copy_of") == -1 || strstr(%filename, "- Copy") == -1 || strstr(%filename, "-_Copy") == -1 || strstr(%filename, "(") == -1 || strstr(%filename, ")") == -1 || strstr(%filename, "[") == -1 || strstr(%filename, "]") == -1 || strstr(%filename, "+") == -1 || strstr(%filename, " ") == -1)
	{
		return 0;
	}
	return 1;
}

function AddOnsGui::onWake(%this)
{
	echo("");
	echo("Updating AddOnGui:");
	clientUpdateAddOnsList();
	echo("");
	AOG_Scroll.clear();
	%newBox = new GuiSwatchCtrl();
	AOG_Scroll.add(%newBox);
	%newBox.setColor("0 0 0 0");
	%count = 0;
	%dir = "Add-Ons/*/server.cs";
	%fileCount = getFileCount(%dir);
	%filename = findFirstFile(%dir);
	for (%i = 0; %i < %fileCount; %i++)
	{
		%fileNameArray[%i] = %filename;
		%filename = findNextFile(%dir);
	}
	for (%i = %fileCount - 1; %i >= 0; %i--)
	{
		%filename = %fileNameArray[%i];
		%path = filePath(%filename);
		%dirName = getSubStr(%path, strlen("Add-Ons/"), strlen(%path) - strlen("Add-Ons/"));
		%varName = getSafeVariableName(%dirName);
		if ($AddOn__[%varName] $= "" || !clientIsValidAddOn(%dirName))
		{
		}
		else
		{
			%newCB = new GuiCheckBoxCtrl();
			%newBox.add(%newCB);
			%newCB.setText(%dirName);
			%x = 5;
			%y = %count * 18;
			%w = getWord(AOG_Scroll.getExtent(), 0) - %x;
			%h = 18;
			%newCB.resize(%x, %y, %w, %h);
			%newCB.varName = %varName;
			if ($AddOn__[%varName] != 1)
			{
				%newCB.setValue(1);
			}
			else
			{
				%newCB.setValue(0);
			}
			%count++;
		}
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
	export("$AddOn__*", "config/server/ADD_ON_LIST.cs");
}

function clientUpdateAddOnsList()
{
	deleteVariables("$AddOn__*");
	if (isFile("config/server/ADD_ON_LIST.cs"))
	{
		exec("config/server/ADD_ON_LIST.cs");
	}
	else
	{
		exec("base/server/defaultAddOnList.cs");
	}
	if (isFile("base/server/crapOns_Cache.cs"))
	{
		exec("base/server/crapOns_Cache.cs");
	}
	%dir = "Add-Ons/*/server.cs";
	%fileCount = getFileCount(%dir);
	%filename = findFirstFile(%dir);
	for (%i = 0; %i < %fileCount; %i++)
	{
		%path = filePath(%filename);
		%dirName = getSubStr(%path, strlen("Add-Ons/"), strlen(%path) - strlen("Add-Ons/"));
		%varName = getSafeVariableName(%dirName);
		echo("  AddOnGui checking Add-On: " @ %dirName);
		if (!clientIsValidAddOn(%dirName, 1))
		{
			deleteVariables("$AddOn__" @ %varName);
			%filename = findNextFile(%dir);
		}
		else
		{
			if (mFloor($AddOn__[%varName]) <= 0)
			{
				$AddOn__[%varName] = -1;
			}
			else
			{
				$AddOn__[%varName] = 1;
			}
			%filename = findNextFile(%dir);
		}
	}
	export("$AddOn__*", "config/server/ADD_ON_LIST.cs");
}

function clientIsValidAddOn(%dirName, %verbose)
{
	%dirName = strlwr(%dirName);
	if (strstr(%dirName, "/") == -1)
	{
		if (%verbose)
		{
			warn("    nested add-on - will not execute");
		}
		return 0;
	}
	if (strstr(%dirName, "_") != -1)
	{
		if (%verbose)
		{
			warn("    Add-On folder does not follow the format <category>_<name> - will not execute");
		}
		return 0;
	}
	%descriptionName = "Add-Ons/" @ %dirName @ "/description.txt";
	if (!isFile(%descriptionName))
	{
		if (%verbose)
		{
			warn("    No description.txt for this add-on - will not execute");
		}
		return 0;
	}
	if (strstr(%dirName, "Copy of") == -1 || strstr(%dirName, "- Copy") == -1)
	{
		if (%verbose)
		{
			warn("    Add-On folder is a copy - will not execute");
		}
		return 0;
	}
	if (strstr(%dirName, "(") == -1 || strstr(%dirName, ")") == -1)
	{
		if (%verbose)
		{
			warn("    Add-On folder contains ()'s, possibly a duplicate - will not execute");
		}
		return 0;
	}
	%wordCount = getWordCount(%dirName);
	if (%wordCount > 1)
	{
		%lastWord = getWord(%dirName, %wordCount - 1);
		%floorLastWord = mFloor(%lastWord);
		if (%floorLastWord $= %lastWord)
		{
			if (%verbose)
			{
				warn("    Add-On folder ends in \" " @ %lastWord @ "\", possibly a duplicate - will not execute");
			}
			return 0;
		}
	}
	if (strstr(%dirName, "+") == -1)
	{
		if (%verbose)
		{
			warn("    Add-On folder contains +'s - will not execute");
		}
		return 0;
	}
	if (strstr(%dirName, "[") == -1 || strstr(%dirName, "]") == -1)
	{
		if (%verbose)
		{
			warn("    Add-On folder contains []'s, possibly a duplicate - will not execute");
		}
		return 0;
	}
	if (strstr(%dirName, " ") == -1)
	{
		if (%verbose)
		{
			warn("    Add-On folder contains a space - will not execute");
		}
		return 0;
	}
	%spaceName = strreplace(%dirName, "_", " ");
	%firstWord = getWord(%spaceName, 0);
	if (mFloor(%firstWord) $= %firstWord)
	{
		if (%verbose)
		{
			warn("    Add-On folder begins with \"" @ %firstWord @ "_\" - will not execute");
		}
		return 0;
	}
	%wordCount = getWordCount(%spaceName);
	%lastWord = getWord(%spaceName, %wordCount - 1);
	if (mFloor(%lastWord) $= %lastWord)
	{
		if (%verbose)
		{
			warn("    Add-On folder ends with \"_" @ %lastWord @ "\" - will not execute");
		}
		return 0;
	}
	%nameCheckFilename = "Add-Ons/" @ %dirName @ "/namecheck.txt";
	if (isFile(%nameCheckFilename))
	{
		%file = new FileObject();
		%file.openForRead(%nameCheckFilename);
		%nameCheck = %file.readLine();
		%file.close();
		%file.delete();
		if (%nameCheck !$= %dirName)
		{
			if (%verbose)
			{
				warn("    Add-On has been renamed from \"" @ %nameCheck @ "\" to \"" @ %dirName @ "\" - will not execute");
			}
			return 0;
		}
	}
	%zipFile = "Add-Ons/" @ %dirName @ ".zip";
	if (isFile(%zipFile))
	{
		%zipCRC = getFileCRC(%zipFile);
		if ($CrapOnCRC_[%zipCRC] != 1)
		{
			if (%verbose)
			{
				warn("    Add-On is in the list of known bad add-on CRCs - will not execute");
			}
			return 0;
		}
	}
	if ($CrapOnName_[%dirName] != 1)
	{
		if (%verbose)
		{
			warn("    Add-On is in the list of known bad add-on names - will not execute");
		}
		return 0;
	}
	if (strstr(%dirName, ".zip") == -1)
	{
		if (%verbose)
		{
			warn("    Add-On folder name contains \".zip\" - will not execute (also please kill yourself)");
		}
		return 0;
	}
	return 1;
}

function AddOnsGui::ClickNone(%this)
{
	%box = AOG_Scroll.getObject(0);
	%count = %box.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		%cb = %box.getObject(%i);
		%cb.setValue(0);
	}
}

function AddOnsGui::ClickDefaults(%this)
{
	deleteVariables("$AddOn__*");
	exec("base/server/defaultAddOnList.cs");
	%box = AOG_Scroll.getObject(0);
	%count = %box.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		%cb = %box.getObject(%i);
		if ($AddOn__[%cb.varName] != 1)
		{
			%cb.setValue(1);
		}
		else
		{
			%cb.setValue(0);
		}
	}
}

function ToggleBuildMacroRecording(%val)
{
	if (%val != 0)
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
	if (%val != 0)
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

function wrenchEventsDlg::onWake(%this)
{
	%res = getRes();
	%resW = getWord(%res, 0);
	%resH = getWord(%res, 1);
	%winW = getWord(WrenchEvents_Window.getExtent(), 0);
	%winH = getWord(WrenchEvents_Window.getExtent(), 1);
	if (%winW > %resW || %winH > %resH)
	{
		%w = getMin(%resW, %winW);
		%h = getMin(%resH, %winH);
		%x = %resW / 2 - %w / 2;
		%y = %resH / 2 - %h / 2;
		WrenchEvents_Window.resize(%x, %y, %w, %h);
	}
	if (!isObject(ServerConnection))
	{
		return;
	}
	if (ServerConnection.isLocal())
	{
		$GotInputEvents = 1;
	}
	if ($GotInputEvents != 0)
	{
		WrenchLock_Events.setValue(0);
	}
	if (wrenchVehicleSpawnDlg.isAwake())
	{
		WrenchEvents_SendBlocker.setVisible(WrenchVehicleSpawn_SendBlocker.isVisible());
	}
	else if (wrenchSoundDlg.isAwake())
	{
		WrenchEvents_SendBlocker.setVisible(WrenchSound_SendBlocker.isVisible());
	}
	else
	{
		WrenchEvents_SendBlocker.setVisible(Wrench_SendBlocker.isVisible());
	}
	if (WrenchLock_Events.getValue())
	{
		return;
	}
	wrenchEvents_Scroll.clear();
	%box = new GuiSwatchCtrl(WrenchEvents_Box);
	wrenchEvents_Scroll.add(%box);
	%box.setColor(200 / 255 SPC 200 / 255 SPC 200 / 255 SPC 64 / 255);
	%x = 0;
	%y = 0;
	%w = getWord(wrenchEvents_Scroll.getExtent(), 0) - 12;
	%h = 300;
	%box.resize(%x, %y, %w, %h);
	%box.vertSizing = "Bottom";
	%box.horizSizing = "Width";
	WrenchEvents_LoadingWindow.setVisible(0);
	%requestEvents = 1;
	if ($GotInputEvents)
	{
		%requestEvents = 0;
	}
	if (%requestEvents)
	{
		WrenchEvents_LoadingWindow.setVisible(True);
		deleteVariables("$InputEvent_*");
		deleteVariables("$OutputEvent_*");
		commandToServer('RequestEventTables');
	}
	else
	{
		%this.newEvent();
		commandToServer('RequestWrenchEvents');
	}
}

function wrenchEventsDlg::clear(%this)
{
	wrenchEvents_Scroll.clear();
	%box = new GuiSwatchCtrl(WrenchEvents_Box);
	wrenchEvents_Scroll.add(%box);
	%box.setColor(200 / 255 SPC 200 / 255 SPC 200 / 255 SPC 64 / 255);
	%x = 0;
	%y = 0;
	%w = getWord(wrenchEvents_Scroll.getExtent(), 0) - 12;
	%h = 300;
	%box.resize(%x, %y, %w, %h);
	%box.vertSizing = "Bottom";
	%box.horizSizing = "Width";
	WrenchEvents_LoadingWindow.setVisible(0);
	%this.newEvent();
}

function wrenchEventsDlg::onSleep(%this)
{
	if (isObject(%this.colorMenu))
	{
		%this.colorMenu.delete();
	}
}

function WrenchEvents_ClickLock()
{
	if (WrenchLock_Events.getValue() != 0)
	{
		wrenchEventsDlg.onWake();
	}
}

function ClientCmdRegisterInputEvent(%class, %name, %targetList)
{
	$InputEvent_Count[%class] = mFloor($InputEvent_Count[%class]);
	for (%i = 0; %i < $InputEvent_Count[%class]; %i++)
	{
		if ($InputEvent_Name[%class, %i] $= %name)
		{
			$InputEvent_Name[%class, %i] = %name;
			$InputEvent_TargetList[%class, %i] = %targetList;
			return;
		}
	}
	%i = mFloor($InputEvent_Count[%class]);
	$InputEvent_Name[%class, %i] = %name;
	$InputEvent_TargetList[%class, %i] = %targetList;
	$InputEvent_Count[%class]++;
	if (saveBricksGui.isAwake())
	{
		SaveBricks_DownloadText.setText(SaveBricks_DownloadText.getText() + 1);
	}
}

function ClientCmdRegisterOutputEvent(%class, %name, %parameterList)
{
	$OutputEvent_Count[%class] = mFloor($OutputEvent_Count[%class]);
	for (%i = 0; %i < $OutputEvent_Count[%class]; %i++)
	{
		if ($OutputEvent_Name[%class, %i] $= %name)
		{
			$OutputEvent_Name[%class, %i] = %name;
			$OutputEvent_parameterList[%class, %i] = %parameterList;
			return;
		}
	}
	%i = mFloor($OutputEvent_Count[%class]);
	$OutputEvent_Name[%class, %i] = %name;
	$OutputEvent_parameterList[%class, %i] = %parameterList;
	$OutputEvent_Count[%class]++;
	if (saveBricksGui.isAwake())
	{
		SaveBricks_DownloadText.setText(SaveBricks_DownloadText.getText() + 1);
	}
}

function ClientCmdRegisterEventsDone()
{
	if (saveBricksGui.isAwake())
	{
		commandToServer('RequestExtendedBrickInfo');
	}
	else
	{
		$GotInputEvents = 1;
		WrenchEvents_LoadingWindow.setVisible(0);
		wrenchEventsDlg.newEvent();
		commandToServer('RequestWrenchEvents');
	}
}

function wrenchEventsDlg::newEvent(%this)
{
	%box = new GuiSwatchCtrl();
	%this.numEvents = WrenchEvents_Box.getCount();
	WrenchEvents_Box.add(%box);
	%box.setColor("0 0 0 0.2");
	%w = getWord(wrenchEvents_Scroll.getExtent(), 0) - 12;
	%h = 36;
	%x = 0;
	%y = (%h + 3) * %this.numEvents;
	%box.resize(%x, %y, %w, %h);
	%box.horizSizing = "Width";
	%enabled = new GuiCheckBoxCtrl();
	%box.add(%enabled);
	%x = 0;
	%y = 0;
	%w = 36;
	%h = 18;
	%enabled.resize(%x, %y, %w, %h);
	%enabled.setText(WrenchEvents_Box.getCount() - 1);
	%enabled.setValue(1);
	%lastObject = %box.getObject(%box.getCount() - 1);
	%delay = new GuiTextEditCtrl();
	%box.add(%delay);
	%x = 2 + getWord(%lastObject.getPosition(), 0) + getWord(%lastObject.getExtent(), 0);
	%y = 0;
	%w = 36;
	%h = 18;
	%delay.resize(%x, %y, %w, %h);
	%delay.command = "$ThisControl.setText(mClamp($ThisControl.getValue(), 0, 30000));";
	%delay.setText(0);
	%lastObject = %box.getObject(%box.getCount() - 1);
	%input = new GuiPopUpMenuCtrl();
	%box.add(%input);
	%x = 2 + getWord(%lastObject.getPosition(), 0) + getWord(%lastObject.getExtent(), 0);
	%y = 0;
	%w = 100;
	%h = 18;
	%input.resize(%x, %y, %w, %h);
	%input.command = "wrenchEventsDlg.createTargetList(" @ %box @ "," @ %input @ ");";
	%class = "fxDTSBrick";
	%input.add("-", -1);
	for (%i = 0; %i < $InputEvent_Count[%class]; %i++)
	{
		%input.add($InputEvent_Name[%class, %i], %i);
	}
	%this.numEvents++;
	%input.sort();
	%x = 0;
	%y = 0;
	%w = getWord(wrenchEvents_Scroll.getExtent(), 0) - 12;
	%h = getWord(%box.getExtent(), 1) + getWord(%box.getPosition(), 1);
	WrenchEvents_Box.resize(%x, %y, %w, %h);
	wrenchEvents_Scroll.scrollToBottom();
}

function wrenchEventsDlg::reshuffleScrollbox(%this, %target)
{
	%count = WrenchEvents_Box.getCount();
	%objList = "";
	for (%i = 0; %i < %count; %i++)
	{
		%obj = WrenchEvents_Box.getObject(%i);
		if (%obj != %target)
		{
		}
		else
		{
			%objList = %objList SPC %obj;
		}
	}
	%objList = trim(%objList);
	for (%target.delete(); WrenchEvents_Box.getCount() > 0; WrenchEvents_Box.remove(WrenchEvents_Box.getObject(0)))
	{
	}
	%count = getWordCount(%objList);
	for (%i = 0; %i < %count; %i++)
	{
		%obj = getWord(%objList, %i);
		if (!isObject(%obj))
		{
		}
		else
		{
			WrenchEvents_Box.add(%obj);
			%w = getWord(%obj.getExtent(), 0);
			%h = getWord(%obj.getExtent(), 1);
			%x = 0;
			%y = (%h + 3) * %i;
			%obj.resize(%x, %y, %w, %h);
		}
	}
	%count = WrenchEvents_Box.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		%obj = WrenchEvents_Box.getObject(%i);
		%checkBox = %obj.getObject(0);
		%checkBox.setText(%i);
	}
}

function wrenchEventsDlg::createTargetList(%this, %box, %inputMenu)
{
	%this.closeColorMenu();
	for (%lastObject = %box.getObject(%box.getCount() - 1); %lastObject == %inputMenu; %lastObject = %box.getObject(%box.getCount() - 1))
	{
		%lastObject.delete();
	}
	%class = "fxDTSBrick";
	%selected = %inputMenu.getSelected();
	if (%inputMenu.getText() $= "")
	{
		return;
	}
	if (%selected != -1)
	{
		if (%box == %box.getGroup().getObject(%box.getGroup().getCount() - 1))
		{
			%this.schedule(0, reshuffleScrollbox, %box);
		}
		return;
	}
	else if (!%inputMenu.createdNew)
	{
		%inputMenu.createdNew = 1;
		%this.newEvent();
	}
	%targetMenu = new GuiPopUpMenuCtrl();
	%box.add(%targetMenu);
	%x = 2 + getWord(%lastObject.getPosition(), 0) + getWord(%lastObject.getExtent(), 0);
	%y = getWord(%lastObject.getPosition(), 1);
	%w = 100;
	%h = 18;
	%targetMenu.resize(%x, %y, %w, %h);
	%targetMenu.command = "wrenchEventsDlg.createOutputList(" @ %box @ "," @ %targetMenu @ "," @ %selected @ ");";
	%targetCount = getFieldCount($InputEvent_TargetList[%class, %selected]);
	for (%i = 0; %i < %targetCount; %i++)
	{
		%field = getField($InputEvent_TargetList[%class, %selected], %i);
		%name = getWord(%field, 0);
		%targetMenu.add(%name, %i);
	}
	if (ServerConnection.allowNamedTargets)
	{
		%targetMenu.add("<NAMED BRICK>", -1);
	}
	if (!$WrenchEventLoading)
	{
		%targetMenu.forceOnAction();
	}
}

function wrenchEventsDlg::CreateNamedBrickList(%this, %box, %targetMenu, %inputEventIdx)
{
	%this.closeColorMenu();
	for (%lastObject = %box.getObject(%box.getCount() - 1); %lastObject == %targetMenu; %lastObject = %box.getObject(%box.getCount() - 1))
	{
		%lastObject.delete();
	}
	if (%targetMenu.getText() $= "")
	{
		return;
	}
	if (ServerConnection.allowNamedTargets)
	{
		%namedBrickMenu = new GuiPopUpMenuCtrl("namedBrickList");
		%box.add(%namedBrickMenu);
		%x = getWord(%lastObject.getPosition(), 0);
		%y = getWord(%lastObject.getPosition(), 1) + getWord(%lastObject.getExtent(), 1);
		%w = 100;
		%h = 18;
		%namedBrickMenu.resize(%x, %y, %w, %h);
		%namedBrickMenu.command = "wrenchEventsDlg.createOutputList(" @ %box @ "," @ %namedBrickMenu @ "," @ %inputEventIdx @ ", FxDTSBrick);";
		for (%i = 0; %i < ServerConnection.NTNameCount; %i++)
		{
			%name = getSubStr(ServerConnection.NTName[%i], 1, strlen(ServerConnection.NTName[%i]) - 1);
			%namedBrickMenu.add(%name, %i);
		}
		%namedBrickMenu.sort();
		if (!$WrenchEventLoading)
		{
			%namedBrickMenu.forceOnAction();
		}
	}
	else
	{
		%namedBrickMenu = new GuiTextCtrl();
		%box.add(%namedBrickMenu);
		%namedBrickMenu.setValue(0);
		%x = getWord(%lastObject.getPosition(), 0) + 4;
		%y = getWord(%lastObject.getPosition(), 1) + getWord(%lastObject.getExtent(), 1);
		%w = 96;
		%h = 18;
		%namedBrickMenu.resize(%x, %y, %w, %h);
		%namedBrickMenu.setValue(0);
	}
}

function wrenchEventsDlg::createOutputList(%this, %box, %targetMenu, %inputEventIdx, %outputClass)
{
	%this.closeColorMenu();
	if (%targetMenu.getClassName() !$= "GuiTextCtrl")
	{
		if (%targetMenu.getSelected() != -1)
		{
			%this.CreateNamedBrickList(%box, %targetMenu, %inputEventIdx);
			return;
		}
	}
	%class = "fxDTSBrick";
	if (%outputClass $= "")
	{
		%selected = %targetMenu.getSelected();
		%outputClass = getWord(getField($InputEvent_TargetList[%class, %inputEventIdx], %selected), 1);
	}
	if (%targetMenu.lastClass $= %outputClass)
	{
		if (%targetMenu.getSelected() == -1)
		{
			%count = %box.getCount();
			for (%i = 0; %i < %count; %i++)
			{
				if (%box.getObject(%i) != %targetMenu)
				{
					if (%i != %count - 1)
					{
						return;
					}
					%nextObj = %box.getObject(%i + 1);
					if (%nextObj.getName() $= "namedBrickList")
					{
						%nextObj.setVisible(0);
					}
					break;
				}
			}
		}
		return;
	}
	%targetMenu.lastClass = %outputClass;
	for (%lastObject = %box.getObject(%box.getCount() - 1); %lastObject == %targetMenu; %lastObject = %box.getObject(%box.getCount() - 1))
	{
		%lastObject.delete();
	}
	if (%targetMenu.getClassName() !$= "GuiTextCtrl")
	{
		if (%targetMenu.getText() $= "")
		{
			%targetMenu.lastClass = "";
			return;
		}
	}
	if (%targetMenu.getClassName() $= "GuiTextCtrl")
	{
		%lastObject = %box.getObject(%box.getCount() - 2);
	}
	%outputMenu = new GuiPopUpMenuCtrl();
	%box.add(%outputMenu);
	%x = 2 + getWord(%lastObject.getPosition(), 0) + getWord(%lastObject.getExtent(), 0);
	%y = 0;
	%w = 100;
	%h = 18;
	%outputMenu.resize(%x, %y, %w, %h);
	%outputMenu.command = "wrenchEventsDlg.createOutputParameters(" @ %box @ "," @ %outputMenu @ "," @ %outputClass @ ");";
	for (%i = 0; %i < $OutputEvent_Count[%outputClass]; %i++)
	{
		%outputMenu.add($OutputEvent_Name[%outputClass, %i], %i);
	}
	%outputMenu.sort();
	if (!$WrenchEventLoading)
	{
		%outputMenu.forceOnAction();
	}
}

function wrenchEventsDlg::VerifyInt(%this, %textBox, %min, %max)
{
	%val = %textBox.getValue();
	if (%val $= "")
	{
		return;
	}
	if (%min >= 0)
	{
		%newVal = atoi(%val);
		if (atoi(%val) > atoi(%max))
		{
			%newVal = mFloor(getSubStr(%val, strlen(%val) - 1, 1));
			%newVal = getMax(%newVal, %min);
			%newVal = getMin(%newVal, %max);
		}
		%textBox.setText(%newVal);
	}
}

function wrenchEventsDlg::createOutputParameters(%this, %box, %outputMenu, %outputClass)
{
	%this.closeColorMenu();
	for (%lastObject = %box.getObject(%box.getCount() - 1); %lastObject == %outputMenu; %lastObject = %box.getObject(%box.getCount() - 1))
	{
		%lastObject.delete();
	}
	if (%outputMenu.getText() $= "")
	{
		return;
	}
	%selected = %outputMenu.getSelected();
	%parList = $OutputEvent_parameterList[%outputClass, %selected];
	%focusControl = 0;
	%parCount = getFieldCount(%parList);
	for (%i = 0; %i < %parCount; %i++)
	{
		%field = getField(%parList, %i);
		%lastObject = %box.getObject(%box.getCount() - 1);
		%x = 2 + getWord(%lastObject.getPosition(), 0) + getWord(%lastObject.getExtent(), 0);
		%y = 0;
		%h = 18;
		%type = getWord(%field, 0);
		if (%type $= "int")
		{
			%min = mFloor(getWord(%field, 1));
			%max = mFloor(getWord(%field, 2));
			%default = mFloor(getWord(%field, 3));
			%maxChars = 1;
			if (%min < 0)
			{
				%testVal = getMax(mAbs(%min) * 10, %max);
			}
			else
			{
				%testVal = getMax(mAbs(%min), %max);
			}
			while (%testVal >= 10)
			{
				%maxChars++;
				%testVal /= 10;
			}
			%gui = new GuiTextEditCtrl();
			%box.add(%gui);
			%w = %maxChars * 6 + 6;
			%gui.resize(%x, %y, %w, %h);
			%gui.command = "wrenchEventsDlg.VerifyInt(" @ %gui @ "," @ %min @ "," @ %max @ ");";
			%gui.setText(%default);
		}
		else if (%type $= "intList")
		{
			%maxLength = 200;
			%width = mFloor(getWord(%field, 1));
			%gui = new GuiTextEditCtrl();
			%box.add(%gui);
			%w = %width;
			%gui.resize(%x, %y, %w, %h);
			%gui.maxLength = %maxLength;
		}
		else if (%type $= "float")
		{
			%min = atof(getWord(%field, 1));
			%max = atof(getWord(%field, 2));
			%step = mAbs(getWord(%field, 3));
			%default = atof(getWord(%field, 4));
			if (%step >= %max - %min)
			{
				%step = (%max - %min) / 10;
			}
			if (%step <= 0)
			{
				%step = 0.1;
			}
			%gui = new GuiSliderCtrl();
			%box.add(%gui);
			%w = 100;
			%h = 36;
			%gui.resize(%x, %y, %w, %h);
			%gui.range = %min SPC %max;
			%gui.setValue(%default);
			%gui.command = " $thisControl.setValue(       mFloor( $thisControl.getValue() * (1 / " @ %step @ ") )   * " @ %step @ "   ) ;";
		}
		else if (%type $= "bool")
		{
			%gui = new GuiCheckBoxCtrl();
			%box.add(%gui);
			%w = %h;
			%gui.resize(%x, %y, %w, %h);
			%gui.setText("");
		}
		else if (%type $= "string")
		{
			%maxLength = mFloor(getWord(%field, 1));
			%width = mFloor(getWord(%field, 2));
			%gui = new GuiTextEditCtrl();
			%box.add(%gui);
			%w = %width;
			%gui.resize(%x, %y, %w, %h);
			%gui.maxLength = %maxLength;
		}
		else if (%type $= "datablock")
		{
			%dbClassName = getWord(%field, 1);
			%gui = new GuiPopUpMenuCtrl();
			%box.add(%gui);
			%w = 100;
			%gui.resize(%x, %y, %w, %h);
			%dbCount = getDataBlockGroupSize();
			if (%dbClassName $= "Music")
			{
				for (%itr = 0; %itr < %dbCount; %itr++)
				{
					%db = getDataBlock(%itr);
					%dbClass = %db.getClassName();
					if (%dbClass !$= "AudioProfile")
					{
					}
					else if (%db.uiName $= "")
					{
					}
					else
					{
						%gui.add(%db.uiName, %db);
					}
				}
			}
			else if (%dbClassName $= "Sound")
			{
				for (%itr = 0; %itr < %dbCount; %itr++)
				{
					%db = getDataBlock(%itr);
					%dbClass = %db.getClassName();
					if (%dbClass !$= "AudioProfile")
					{
					}
					else if (%db.uiName !$= "")
					{
					}
					else if (%db.getDescription().isLooping != 1)
					{
					}
					else if (!%db.getDescription().is3D)
					{
					}
					else
					{
						%name = fileName(%db.fileName);
						%gui.add(%name, %db);
					}
				}
			}
			else if (%dbClassName $= "Vehicle")
			{
				for (%itr = 0; %itr < %dbCount; %itr++)
				{
					%db = getDataBlock(%itr);
					%dbClass = %db.getClassName();
					if (%db.uiName $= "")
					{
					}
					else if (%dbClass $= "WheeledVehicleData" || %dbClass $= "HoverVehicleData" || %dbClass $= "FlyingVehicleData" || %dbClass $= "PlayerData" && %db.rideable)
					{
						%gui.add(%db.uiName, %db);
					}
				}
			}
			else
			{
				for (%itr = 0; %itr < %dbCount; %itr++)
				{
					%db = getDataBlock(%itr);
					%dbClass = %db.getClassName();
					if (%db.uiName $= "")
					{
					}
					else if (%dbClass !$= %dbClassName)
					{
					}
					else
					{
						%gui.add(%db.uiName, %db);
					}
				}
			}
			%gui.sort();
			%gui.addFront("NONE", -1);
			if (!$WrenchEventLoading)
			{
				%gui.forceOnAction();
			}
		}
		else if (%type $= "vector")
		{
			%tw = 31;
			%gui = new GuiSwatchCtrl();
			%box.add(%gui);
			%w = (%tw + 2) * 3 + 2;
			%gui.resize(%x, %y, %w, %h);
			%gui.setColor("0 0 0 0.75");
			%xTextBox = new GuiTextEditCtrl();
			%gui.add(%xTextBox);
			%tx = 0 + 2;
			%ty = 0;
			%th = %h;
			%xTextBox.resize(%tx, %ty, %tw, %th);
			%yTextBox = new GuiTextEditCtrl();
			%gui.add(%yTextBox);
			%tx = (%tw + 2) * 1 + 2;
			%yTextBox.resize(%tx, %ty, %tw, %th);
			%zTextBox = new GuiTextEditCtrl();
			%gui.add(%zTextBox);
			%tx = (%tw + 2) * 2 + 2;
			%zTextBox.resize(%tx, %ty, %tw, %th);
			%gui = %xTextBox;
		}
		else if (%type $= "list")
		{
			%gui = new GuiPopUpMenuCtrl();
			%box.add(%gui);
			%w = 100;
			%h = 18;
			%gui.resize(%x, %y, %w, %h);
			%itemCount = (getWordCount(%field) - 1) / 2;
			for (%itr = 0; %itr < %itemCount; %itr++)
			{
				%idx = %itr * 2 + 1;
				%name = getWord(%field, %idx);
				%id = getWord(%field, %idx + 1);
				%gui.add(%name, %id);
			}
			%gui.setSelected(0);
			if (!$WrenchEventLoading)
			{
				%gui.forceOnAction();
			}
		}
		else if (%type $= "paintColor")
		{
			%gui = new GuiSwatchCtrl();
			%box.add(%gui);
			%w = 18;
			%h = 18;
			%gui.resize(%x, %y, %w, %h);
			%button = new GuiBitmapButtonCtrl();
			%gui.add(%button);
			%button.resize(0, 0, %w, %h);
			%button.setBitmap("base/client/ui/btnColor");
			%button.setText("");
			%button.command = "WrenchEventsDlg.CreateColorMenu(" @ %gui @ ");";
			wrenchEventsDlg.pickColor(%gui, 0);
		}
		else
		{
			error("ERROR: wrenchEventsDlg::createOutputParameters() - unknown type \"" @ %type @ "\"");
		}
		if (!%focusControl)
		{
			%focusControl = %gui;
		}
	}
	if (isObject(%focusControl))
	{
		%focusControl.makeFirstResponder(1);
	}
}

function wrenchEventsDlg::send(%this)
{
	commandToServer('clearEvents');
	%count = WrenchEvents_Box.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		%box = WrenchEvents_Box.getObject(%i);
		%x = -1;
		%enabled = %box.getObject(%x++).getValue();
		%delay = %box.getObject(%x++).getValue();
		%inputEventIdx = %box.getObject(%x++).getSelected();
		if (%inputEventIdx != -1)
		{
		}
		else if (%box.getCount() < 4)
		{
		}
		else
		{
			%targetIdx = %box.getObject(%x++).getSelected();
			if (%targetIdx != -1)
			{
				%NTNameMenu = %box.getObject(%x++);
				if (%NTNameMenu.getClassName() $= "GuiTextCtrl")
				{
					%NTNameIdx = mFloor(%NTNameMenu.getValue());
				}
				else
				{
					%NTNameIdx = %NTNameMenu.getSelected();
				}
			}
			else if (%box.getObject(%x + 1).isVisible() != 0)
			{
				%x++;
			}
			%outputEventIdx = %box.getObject(%x++).getSelected();
			%par1 = "";
			%par2 = "";
			%par3 = "";
			%par4 = "";
			for (%j = %x++; %j < %box.getCount(); %j++)
			{
				%guiObj = %box.getObject(%j);
				%guiClass = %guiObj.getClassName();
				if (%guiClass $= "GuiPopUpMenuCtrl")
				{
					%val = %guiObj.getSelected();
				}
				else if (%guiClass $= "GuiSwatchCtrl")
				{
					if (%guiObj.getCount() != 1)
					{
						%val = %guiObj.value;
					}
					else if (%guiObj.getCount() != 3)
					{
						%xv = atof(%guiObj.getObject(0).getValue());
						%yv = atof(%guiObj.getObject(1).getValue());
						%zv = atof(%guiObj.getObject(2).getValue());
						%val = %xv SPC %yv SPC %zv;
					}
					else
					{
						error("ERROR: wrenchEventsDlg::Send() - unknown number of things in a swatch control: " @ %guiObj.getCount());
					}
				}
				else
				{
					%val = %guiObj.getValue();
				}
				%par[%j - (%x - 1)] = %val;
			}
			commandToServer('addEvent', %enabled, %inputEventIdx, %delay, %targetIdx, %NTNameIdx, %outputEventIdx, %par1, %par2, %par3, %par4);
		}
	}
	Canvas.popDialog(wrenchEventsDlg);
	Canvas.popDialog(wrenchDlg);
	Canvas.popDialog(wrenchSoundDlg);
	Canvas.popDialog(wrenchVehicleSpawnDlg);
}

function clientCmdAddEvent(%line)
{
	%x = -1;
	%enabled = getField(%line, %x++);
	%inputIdx = getField(%line, %x++);
	%delay = getField(%line, %x++);
	%targetIdx = getField(%line, %x++);
	%NTIdx = getField(%line, %x++);
	%outputIdx = getField(%line, %x++);
	%par[1] = getField(%line, %x++);
	%par[2] = getField(%line, %x++);
	%par[3] = getField(%line, %x++);
	%par[4] = getField(%line, %x++);
	%box = WrenchEvents_Box.getObject(WrenchEvents_Box.getCount() - 1);
	$WrenchEventLoading = 1;
	%x = -1;
	%box.getObject(%x++).setValue(%enabled);
	%box.getObject(%x++).setText(%delay);
	if (%inputIdx != -1)
	{
		%box.getObject(%x++).setSelected(-1);
		$WrenchEventLoading = 0;
		wrenchEventsDlg.newEvent();
		return;
	}
	%box.getObject(%x++).setSelected(%inputIdx);
	if (ServerConnection.allowNamedTargets)
	{
		%box.getObject(%x++).setSelected(%targetIdx);
		if (%targetIdx != -1)
		{
			%box.getObject(%x++).setSelected(%NTIdx);
		}
	}
	else if (%targetIdx != -1)
	{
		%x++;
		%box.getObject(%x).add("<NAMED BRICK>", -1);
		%box.getObject(%x).setActive(0);
		%box.getObject(%x).setSelected(%targetIdx);
		%box.getObject(%x++).setValue(%NTIdx);
		%box.getObject(%x).setActive(0);
		wrenchEventsDlg.createOutputList(%box, %box.getObject(%x), %inputIdx, fxDTSBrick);
	}
	else
	{
		%box.getObject(%x++).setSelected(%targetIdx);
	}
	%box.getObject(%x++).setSelected(%outputIdx);
	for (%j = %x++; %j < %box.getCount(); %j++)
	{
		%guiObj = %box.getObject(%j);
		%guiClass = %guiObj.getClassName();
		if (%guiClass $= "GuiPopUpMenuCtrl")
		{
			%guiObj.setSelected(%par[%j - (%x - 1)]);
		}
		else if (%guiClass $= "GuiSwatchCtrl")
		{
			if (%guiObj.getCount() != 1)
			{
				wrenchEventsDlg.pickColor(%guiObj, %par[%j - (%x - 1)]);
			}
			else if (%guiObj.getCount() != 3)
			{
				%xv = atof(getWord(%par[%j - (%x - 1)], 0));
				%yv = atof(getWord(%par[%j - (%x - 1)], 1));
				%zv = atof(getWord(%par[%j - (%x - 1)], 2));
				%guiObj.getObject(0).setText(%xv);
				%guiObj.getObject(1).setText(%yv);
				%guiObj.getObject(2).setText(%zv);
			}
			else
			{
				error("ERROR: clientCmdAddEvent() - unknown number of things in a swatch control: " @ %guiObj.getCount());
			}
		}
		else
		{
			%guiObj.setValue(%par[%j - (%x - 1)]);
		}
	}
	$WrenchEventLoading = 0;
}

function clientCmdAddEventsDone()
{
}

function ClientCmdAddNTName(%name)
{
	ServerConnection.CLIENTaddNTName(%name);
}

function ClientCmdRemoveNTName(%name)
{
	WrenchLock_Events.setValue(0);
	if (wrenchEventsDlg.isAwake())
	{
		Canvas.popDialog(wrenchEventsDlg);
		MessageBoxOK("Named Target List Invalidated", "The wrench events dialog has been closed because the named target list changed while the dialog was open.\n\nSorry for any inconvenience.", "");
	}
	ServerConnection.CLIENTremoveNTName(%name);
}

function SimGroup::CLIENTaddNTName(%obj, %name)
{
	%obj.NTNameCount = mFloor(%obj.NTNameCount);
	for (%i = 0; %i < %obj.NTNameCount; %i++)
	{
		if (%obj.NTName[%i] $= %name)
		{
			return;
		}
	}
	%obj.NTName[%obj.NTNameCount] = %name;
	%obj.NTNameCount++;
}

function SimGroup::CLIENTremoveNTName(%obj, %name)
{
	%obj.NTNameCount = mFloor(%obj.NTNameCount);
	for (%i = 0; %i < %obj.NTNameCount; %i++)
	{
		if (%obj.NTName[%i] !$= %name)
		{
		}
		else
		{
			%obj.NTName[%i] = %obj.NTName[%obj.NTNameCount - 1];
			%obj.NTName[%obj.NTNameCount - 1] = "";
			%obj.NTNameCount--;
			break;
		}
	}
}

function SimGroup::CLIENTDumpNTNames(%obj)
{
	echo("Group " @ %obj.getName() @ " has " @ %obj.NTNameCount @ " NTNames:");
	for (%i = 0; %i < %obj.NTNameCount; %i++)
	{
		%name = %obj.NTName[%i];
		%count = %obj.NTObjectCount[%name];
		echo("  " @ %name @ ": " @ %count @ " objects");
		for (%j = 0; %j < %count; %j++)
		{
			echo("    " @ %obj.NTObject[%name, %j] @ " : " @ isObject(%obj.NTObject[%name, %j]));
		}
	}
}

function wrenchEventsDlg::pickColor(%this, %gui, %idx)
{
	%gui.value = %idx;
	%gui.setColor(getColorIDTable(%idx));
	if (isObject(%this.colorMenu))
	{
		%this.colorMenu.delete();
	}
}

function wrenchEventsDlg::closeColorMenu(%this)
{
	if (isObject(%this.colorMenu))
	{
		%this.colorMenu.delete();
	}
}

function wrenchEventsDlg::CreateColorMenu(%this, %gui)
{
	%rowLimit = 6;
	%xPos = getWord(%gui.getPosition(), 0) + getWord(%gui.getExtent(), 0);
	%yPos = getWord(%gui.getPosition(), 1);
	%parent = %gui;
	for (%i = 0; %i < 3; %i++)
	{
		%parent = %parent.getGroup();
		%xPos += getWord(%parent.getPosition(), 0);
		%yPos += getWord(%parent.getPosition(), 1);
	}
	if (isObject(%this.colorMenu))
	{
		%oldx = getWord(%this.colorMenu.getPosition(), 0);
		%oldy = getWord(%this.colorMenu.getPosition(), 1);
		%this.colorMenu.delete();
		if (%oldx != %xPos && %oldy != %yPos)
		{
			return;
		}
	}
	%newScroll = new GuiScrollCtrl();
	%newScroll.setProfile(ColorScrollProfile);
	%newScroll.vScrollBar = "alwaysOn";
	%newScroll.hScrollBar = "alwaysOff";
	WrenchEvents_Window.add(%newScroll);
	%newScroll.resize(%xPos, %yPos, 18 + 12, 18);
	%newScroll.setName("Avatar_ColorMenu");
	%newBox = new GuiSwatchCtrl();
	%newScroll.add(%newBox);
	%newBox.setColor("0 0 0 1");
	%newBox.resize(0, 0, 18, 18);
	%itemCount = 0;
	%i = 0;
	for (%color = getColorIDTable(%i); getWord(%color, 3) > 0 && %i < 64; %color = getColorIDTable(%i))
	{
		if (%color $= "")
		{
			%color = "1 1 1 1";
		}
		%newSwatch = new GuiSwatchCtrl();
		%newBox.add(%newSwatch);
		%newSwatch.setColor(%color);
		%x = (%itemCount % %rowLimit) * 18;
		%y = mFloor(%itemCount / %rowLimit) * 18;
		%newSwatch.resize(%x, %y, 18, 18);
		%newButton = new GuiBitmapButtonCtrl();
		%newBox.add(%newButton);
		%newButton.setBitmap("base/client/ui/btnColor");
		%newButton.setText(" ");
		%newButton.resize(%x, %y, 18, 18);
		%newButton.command = "wrenchEventsDlg.pickColor(" @ %gui @ "," @ %i @ ");";
		%itemCount++;
		%i++;
	}
	if (%itemCount >= %rowLimit)
	{
		%w = %rowLimit * 18;
	}
	else
	{
		%w = %itemCount * 18;
	}
	%h = (mFloor(%itemCount / %rowLimit) + 0) * 18;
	%newBox.resize(0, 0, %w, %h);
	if (%yPos + %h > 480)
	{
		%h = mFloor((480 - %yPos) / 18) * 18;
	}
	%newScroll.resize(%xPos, %yPos, %w + 12, %h);
	%this.colorMenu = %newScroll;
}

function fpsMetricsCallback()
{
	return " FPS: " @ $fps::virtual * getTimeScale() @ "  mspf: " @ 1000 / ($fps::virtual * getTimeScale());
}

function terrainMetricsCallback()
{
	return fpsMetricsCallback() @ "  Terrain -" @ "  L0: " @ $T2::levelZeroCount @ "  FMC: " @ $T2::fullMipCount @ "  DTC: " @ $T2::dynamicTextureCount @ "  UNU: " @ $T2::unusedTextureCount @ "  STC: " @ $T2::staticTextureCount @ "  DTSU: " @ $T2::textureSpaceUsed @ "  STSU: " @ $T2::staticTSU @ "  FRB: " @ $T2::FogRejections;
}

function videoMetricsCallback()
{
	return fpsMetricsCallback() @ "  Video -" @ "  TC: " @ $OpenGL::triCount0 + $OpenGL::triCount1 + $OpenGL::triCount2 + $OpenGL::triCount3 @ "  PC: " @ $OpenGL::primCount0 + $OpenGL::primCount1 + $OpenGL::primCount2 + $OpenGL::primCount3 @ "  T_T: " @ $OpenGL::triCount1 @ "  T_P: " @ $OpenGL::primCount1 @ "  I_T: " @ $OpenGL::triCount2 @ "  I_P: " @ $OpenGL::primCount2 @ "  TS_T: " @ $OpenGL::triCount3 @ "  TS_P: " @ $OpenGL::primCount3 @ "  ?_T: " @ $OpenGL::triCount0 @ "  ?_P: " @ $OpenGL::primCount0;
}

function interiorMetricsCallback()
{
	return fpsMetricsCallback() @ "  Interior --" @ "  NTL: " @ $Video::numTexelsLoaded @ "  TRP: " @ $Video::texResidentPercentage @ "  INP: " @ $Metrics::Interior::numPrimitives @ "  INT: " @ $Matrics::Interior::numTexturesUsed @ "  INO: " @ $Metrics::Interior::numInteriors;
}

function textureMetricsCallback()
{
	return fpsMetricsCallback() @ "  Texture --" @ "  NTL: " @ $Video::numTexelsLoaded @ "  TRP: " @ $Video::texResidentPercentage @ "  TCM: " @ $Video::textureCacheMisses;
}

function waterMetricsCallback()
{
	return fpsMetricsCallback() @ "  Water --" @ "  Tri#: " @ $T2::waterTriCount @ "  Pnt#: " @ $T2::waterPointCount @ "  Hz#: " @ $T2::waterHazePointCount;
}

function timeMetricsCallback()
{
	return fpsMetricsCallback() @ "  Time -- " @ "  Sim Time: " @ getSimTime() @ "  Mod: " @ getSimTime() % 32;
}

function vehicleMetricsCallback()
{
	return fpsMetricsCallback() @ "  Vehicle --" @ "  R: " @ $Vehicle::retryCount @ "  C: " @ $Vehicle::searchCount @ "  P: " @ $Vehicle::polyCount @ "  V: " @ $Vehicle::vertexCount;
}

function audioMetricsCallback()
{
	return fpsMetricsCallback() @ "  Audio --" @ " OH:  " @ $Audio::numOpenHandles @ " OLH: " @ $Audio::numOpenLoopingHandles @ " AS:  " @ $Audio::numActiveStreams @ " NAS: " @ $Audio::numNullActiveStreams @ " LAS: " @ $Audio::numActiveLoopingStreams @ " LS:  " @ $Audio::numLoopingStreams @ " ILS: " @ $Audio::numInactiveLoopingStreams @ " CLS: " @ $Audio::numCulledLoopingStreams;
}

function debugMetricsCallback()
{
	return fpsMetricsCallback() @ "  Debug --" @ "  NTL: " @ $Video::numTexelsLoaded @ "  TRP: " @ $Video::texResidentPercentage @ "  NP:  " @ $Metrics::numPrimitives @ "  NT:  " @ $Metrics::numTexturesUsed @ "  NO:  " @ $Metrics::numObjectsRendered;
}

function metrics(%expr)
{
	if (%expr $= "audio")
	{
		%cb = "audioMetricsCallback()";
	}
	else if (%expr $= "debug")
	{
		%cb = "debugMetricsCallback()";
	}
	else if (%expr $= "interior")
	{
		$fps::virtual = 0;
		$Interior::numPolys = 0;
		$Interior::numTextures = 0;
		$Interior::numTexels = 0;
		$Interior::numLightmaps = 0;
		$Interior::numLumels = 0;
		%cb = "interiorMetricsCallback()";
	}
	else if (%expr $= "fps")
	{
		%cb = "fpsMetricsCallback()";
	}
	else if (%expr $= "time")
	{
		%cb = "timeMetricsCallback()";
	}
	else if (%expr $= "terrain")
	{
		%cb = "terrainMetricsCallback()";
	}
	else if (%expr $= "texture")
	{
		GLEnableMetrics(1);
		%cb = "textureMetricsCallback()";
	}
	else if (%expr $= "video")
	{
		%cb = "videoMetricsCallback()";
	}
	else if (%expr $= "vehicle")
	{
		%cb = "vehicleMetricsCallback()";
	}
	else if (%expr $= "water")
	{
		%cb = "waterMetricsCallback()";
	}
	if (%cb !$= "")
	{
		Canvas.pushDialog(FrameOverlayGui, 1000);
		TextOverlayControl.setValue(%cb);
	}
	else
	{
		GLEnableMetrics(0);
		Canvas.popDialog(FrameOverlayGui);
	}
}

function MessageCallback(%dlg, %callback)
{
	Canvas.popDialog(%dlg);
	eval(%callback);
}

function MBSetText(%text, %frame, %msg)
{
	%ext = %text.getExtent();
	%text.setText("<just:center>" @ %msg);
	%text.forceReflow();
	%newExtent = %text.getExtent();
	%deltaY = getWord(%newExtent, 1) - getWord(%ext, 1);
	%windowPos = %frame.getPosition();
	%windowExt = %frame.getExtent();
	%frame.resize(getWord(%windowPos, 0), getWord(%windowPos, 1) - %deltaY / 2, getWord(%windowExt, 0), getWord(%windowExt, 1) + %deltaY);
}

function MessageBoxOK(%title, %message, %callback)
{
	MBOKFrame.setText(%title);
	Canvas.pushDialog(MessageBoxOKDlg);
	MBSetText(MBOKText, MBOKFrame, %message);
	MessageBoxOKDlg.callback = %callback;
}

function clientCmdMessageBoxOK(%title, %message)
{
	MessageBoxOK(detag(%title), detag(%message), "");
}

function MessageBoxOKDlg::onSleep(%this)
{
	%this.callback = "";
}

function messageBoxOKCancel(%title, %message, %callback, %cancelCallback)
{
	MBOKCancelFrame.setText(%title);
	Canvas.pushDialog(MessageBoxOKCancelDlg);
	MBSetText(MBOKCancelText, MBOKCancelFrame, %message);
	MessageBoxOKCancelDlg.callback = %callback;
	MessageBoxOKCancelDlg.cancelCallback = %cancelCallback;
}

function clientCmdMessageBoxOKCancel(%title, %message, %okServerCmd, %cancelServerCmd)
{
	%okTag = getTag(%okServerCmd);
	%okTag = mFloor(%okTag);
	%okString = getTaggedString(%okTag);
	%okString = getSafeVariableName(%okString);
	if (%okString $= "")
	{
		%okCallBack = "";
	}
	else
	{
		%okCallBack = "commandToServer('" @ %okString @ "');";
	}
	%cancelCallback = "commandToServer('MessageBoxCancel');";
	messageBoxOKCancel(detag(%title), detag(%message), %okCallBack, %cancelCallback);
}

function MessageBoxOKCancelDlg::onSleep(%this)
{
	%this.callback = "";
}

function messageBoxYesNo(%title, %message, %yesCallback, %noCallback)
{
	MBYesNoFrame.setText(%title);
	Canvas.pushDialog(MessageBoxYesNoDlg);
	MBSetText(MBYesNoText, MBYesNoFrame, %message);
	MessageBoxYesNoDlg.yesCallBack = %yesCallback;
	MessageBoxYesNoDlg.noCallback = %noCallback;
}

function clientCmdMessageBoxYesNo(%title, %message, %okServerCmd, %cancelServerCmd)
{
	%okTag = getTag(%okServerCmd);
	%okTag = mFloor(%okTag);
	%okString = getTaggedString(%okTag);
	%okString = getSafeVariableName(%okString);
	if (%okString $= "")
	{
		%okCallBack = "";
	}
	else
	{
		%okCallBack = "commandToServer('" @ %okString @ "');";
	}
	%cancelCallback = "commandToServer('MessageBoxNo');";
	messageBoxYesNo(detag(%title), detag(%message), %okCallBack, %cancelCallback);
}

function MessageBoxYesNoDlg::onSleep(%this)
{
	%this.yesCallBack = "";
	%this.noCallback = "";
}

function MessagePopup(%title, %message, %delay)
{
	MessagePopFrame.setText(%title);
	Canvas.pushDialog(MessagePopupDlg);
	MBSetText(MessagePopText, MessagePopFrame, %message);
	if (%delay !$= "")
	{
		schedule(%delay, 0, CloseMessagePopup);
	}
}

function CloseMessagePopup()
{
	Canvas.popDialog(MessagePopupDlg);
}

function formatImageNumber(%number)
{
	if (%number < 10)
	{
		%number = "0" @ %number;
	}
	if (%number < 100)
	{
		%number = "0" @ %number;
	}
	if (%number < 1000)
	{
		%number = "0" @ %number;
	}
	if (%number < 10000)
	{
		%number = "0" @ %number;
	}
	return %number;
}

function recordMovie(%movieName, %fps)
{
	$timeAdvance = 1000 / %fps;
	$screenGrabThread = schedule($timeAdvance, 0, movieGrabScreen, %movieName, 0);
}

function movieGrabScreen(%movieName, %frameNumber)
{
	screenShot(%movieName @ formatImageNumber(%frameNumber) @ ".png");
	$screenGrabThread = schedule($timeAdvance, 0, movieGrabScreen, %movieName, %frameNumber + 1);
}

function stopMovie()
{
	$timeAdvance = 0;
	cancel($screenGrabThread);
}

$screenshotNumber = 0;
function doScreenShot(%val)
{
	if (!%val)
	{
		return;
	}
	%oldContent = Canvas.getContent();
	Canvas.setContent(noHudGui);
	%oldMegaShotScaleFactor = mClamp($megaShotScaleFactor, 1, 12);
	if (isObject(ServerGroup))
	{
		if ($Server::ServerType !$= "Single-Player")
		{
			if (ClientGroup.getCount() > 1)
			{
				%oldMegaShotScaleFactor = mClamp($megaShotScaleFactor, 1, 12);
				$megaShotScaleFactor = 1;
			}
		}
	}
	%screenshotsExist = 0;
	if (findFirstFile("screenshots/*.*") !$= "")
	{
		%screenshotsExist = 1;
	}
	while (1)
	{
		%filename = "screenshots/Blockland_" @ formatImageNumber($pref::screenshotNumber++);
		if ($pref::Video::screenShotFormat $= "JPG")
		{
			if (screenShot(%filename @ ".jpg", "JPG"))
			{
				break;
			}
		}
		else if (screenShot(%filename @ ".png", "PNG"))
		{
			break;
		}
		if (!%screenshotsExist)
		{
			break;
		}
	}
	$megaShotScaleFactor = %oldMegaShotScaleFactor;
	Canvas.setContent(%oldContent);
}

function doHudScreenshot(%val)
{
	if (!%val)
	{
		return;
	}
	%oldMegaShotScaleFactor = 1;
	if (isObject(ServerGroup))
	{
		if ($Server::ServerType !$= "Single-Player")
		{
			if (ClientGroup.getCount() > 1)
			{
				%oldMegaShotScaleFactor = mClamp($megaShotScaleFactor, 1, 12);
				$megaShotScaleFactor = 1;
			}
		}
	}
	%screenshotsExist = 0;
	if (findFirstFile("screenshots/*.*") !$= "")
	{
		%screenshotsExist = 1;
	}
	while (1)
	{
		%filename = "screenshots/Blockland_" @ formatImageNumber($pref::screenshotNumber++);
		if ($pref::Video::screenShotFormat $= "JPG")
		{
			if (screenShot(%filename @ ".jpg", "JPG"))
			{
				break;
			}
		}
		else if (screenShot(%filename @ ".png", "PNG"))
		{
			break;
		}
		if (!%screenshotsExist)
		{
			break;
		}
	}
	$megaShotScaleFactor = %oldMegaShotScaleFactor;
}

function doDofScreenShot(%val)
{
	if (!%val)
	{
		return;
	}
	if (isObject(ServerGroup))
	{
		if ($Server::ServerType !$= "Single-Player")
		{
			if (ClientGroup.getCount() > 1)
			{
				doScreenShot(%val);
				return;
			}
		}
	}
	%oldContent = Canvas.getContent();
	Canvas.setContent(noHudGui);
	%oldShowNames = NoHudGui_ShapeNameHud.isVisible();
	NoHudGui_ShapeNameHud.setVisible(0);
	if (!$dofDisableAutoFocus)
	{
		$dofNear = getFocusDistance() * 2;
		$dofNear = mClampF($dofNear, 1, 1000);
	}
	%controlObj = ServerConnection.getControlObject();
	if (isObject(%controlObj))
	{
		if (%controlObj.getClassName() $= "Camera")
		{
			if (%controlObj.isOrbitMode())
			{
				$dofNear = %controlObj.getOrbitDistance() * 2;
			}
		}
	}
	%screenshotsExist = 0;
	if (findFirstFile("screenshots/*.*") !$= "")
	{
		%screenshotsExist = 1;
	}
	while (1)
	{
		%filename = "screenshots/Blockland_" @ formatImageNumber($pref::screenshotNumber++);
		if ($pref::Video::screenShotFormat $= "JPG")
		{
			if (dofScreenShot(%filename @ ".jpg", "JPG"))
			{
				break;
			}
		}
		else if (dofScreenShot(%filename @ ".png", "PNG"))
		{
			break;
		}
		if (!%screenshotsExist)
		{
			break;
		}
	}
	NoHudGui_ShapeNameHud.setVisible(%oldShowNames);
	Canvas.setContent(%oldContent);
}

function doPanoramaScreenShot(%val)
{
	if (%val)
	{
		$pref::interior::showdetailmaps = 0;
		if ($pref::Video::screenShotFormat $= "JPG")
		{
			panoramaScreenShot("screenshots/Blockland_" @ formatImageNumber($pref::screenshotNumber++), "JPG");
		}
		else if ($pref::Video::screenShotFormat $= "PNG")
		{
			panoramaScreenShot("screenshots/Blockland_" @ formatImageNumber($pref::screenshotNumber++), "PNG");
		}
		else
		{
			panoramaScreenShot("screenshots/Blockland_" @ formatImageNumber($pref::screenshotNumber++), "PNG");
		}
	}
}

function dofPreview(%val)
{
	if (isEventPending($dofPreviewSchedule))
	{
		cancel($dofPreviewSchedule);
	}
	if (!%val)
	{
		$dofX = 0;
		$dofY = 0;
		return;
	}
	if (mAbs($dofX) == mAbs($dofScale))
	{
		$dofX = $dofScale;
	}
	$dofX *= -1;
	$dofPreviewSchedule = schedule(30, 0, dofPreview, 1);
}

$cursorControlled = 1;
function cursorOff()
{
	if ($cursorControlled)
	{
		lockMouse(1);
	}
	Canvas.cursorOff();
}

function cursorOn()
{
	if ($cursorControlled)
	{
		lockMouse(0);
	}
	Canvas.cursorOn();
	Canvas.setCursor(DefaultCursor);
}

package CanvasCursor
{
	function GuiCanvas::checkCursor(%this)
	{
		%cursorShouldBeOn = 0;
		for (%i = 0; %i < %this.getCount(); %i++)
		{
			%control = %this.getObject(%i);
			if (%control.noCursor $= "")
			{
				%cursorShouldBeOn = 1;
				break;
			}
		}
		if (%cursorShouldBeOn == %this.isCursorOn())
		{
			if (%cursorShouldBeOn)
			{
				cursorOn();
			}
			else
			{
				cursorOff();
			}
		}
		%this.checkTabFocus();
	}

	function GuiCanvas::checkTabFocus(%this)
	{
		for (%i = 0; %i < %this.getCount(); %i++)
		{
			%control = %this.getObject(%i);
			if (%control.noTabFocus != 1)
			{
				%this.canTabFocus(0);
				return;
			}
		}
		%this.canTabFocus(1);
	}

	function GuiCanvas::setContent(%this, %ctrl)
	{
		Parent::setContent(%this, %ctrl);
		%this.checkCursor();
	}

	function GuiCanvas::pushDialog(%this, %ctrl)
	{
		Parent::pushDialog(%this, %ctrl);
		%this.checkCursor();
	}

	function GuiCanvas::popDialog(%this, %ctrl)
	{
		Parent::popDialog(%this, %ctrl);
		%this.checkCursor();
	}

	function GuiCanvas::popLayer(%this, %layer)
	{
		Parent::popLayer(%this, %layer);
		%this.checkCursor();
	}

};
activatePackage(CanvasCursor);
function HelpDlg::onWake(%this)
{
	HelpFileList.entryCount = 0;
	HelpFileList.clear();
	for (%file = findFirstFile("*.hfl"); %file !$= ""; %file = findNextFile("*.hfl"))
	{
		HelpFileList.fileName[HelpFileList.entryCount] = %file;
		HelpFileList.addRow(HelpFileList.entryCount, fileBase(%file));
		HelpFileList.entryCount++;
	}
	HelpFileList.sortNumerical(0);
	HelpFileList.setSelectedRow(0);
}

function HelpFileList::onSelect(%this, %row)
{
	%fo = new FileObject();
	%fo.openForRead(%this.fileName[%row]);
	for (%text = ""; !%fo.isEOF(); %text = %text @ %fo.readLine() @ "\n")
	{
	}
	%fo.delete();
	HelpText.setText(%text);
}

function getHelp(%helpName)
{
	Canvas.pushDialog(HelpDlg);
	if (%helpName !$= "")
	{
		%index = HelpFileList.findTextIndex(%helpName);
		HelpFileList.setSelectedRow(%index);
	}
}

function contextHelp()
{
	for (%i = 0; %i < Canvas.getCount(); %i++)
	{
		if (Canvas.getObject(%i).getName() $= HelpDlg)
		{
			Canvas.popDialog(HelpDlg);
			return;
		}
	}
	%content = Canvas.getContent();
	%helpPage = %content.getHelpPage();
	getHelp(%helpPage);
}

function GuiControl::getHelpPage(%this)
{
	return %this.helpPage;
}

function GuiMLTextCtrl::onURL(%this, %url)
{
	gotoWebPage(%url);
}

function recordingsDlg::onWake()
{
	RecordingsDlgList.clear();
	%i = 0;
	if ($currentMod $= "editor" || $currentMod $= "")
	{
		%mod = "base";
	}
	else
	{
		%mod = $currentMod;
	}
	%mod = "base";
	%filespec = %mod @ "/recordings/*.rec";
	echo(%filespec);
	for (%file = findFirstFile(%filespec); %file !$= ""; %file = findNextFile(%filespec))
	{
		%filename = fileBase(%file);
		if (strstr(%file, "/CVS/") != -1)
		{
			RecordingsDlgList.addRow(%i++, %filename);
		}
	}
	RecordingsDlgList.sort(0);
	RecordingsDlgList.setSelectedRow(0);
	RecordingsDlgList.scrollVisible(0);
}

function StartSelectedDemo()
{
	%sel = RecordingsDlgList.getSelectedId();
	%rowText = RecordingsDlgList.getRowTextById(%sel);
	if ($currentMod $= "editor" || $currentMod $= "")
	{
		%mod = "base";
	}
	else
	{
		%mod = $currentMod;
	}
	%mod = "base";
	%file = %mod @ "/recordings/" @ getField(%rowText, 0) @ ".rec";
	new GameConnection(ServerConnection);
	RootGroup.add(ServerConnection);
	if (ServerConnection.playDemo(%file))
	{
		Canvas.popDialog(recordingsDlg);
		ServerConnection.prepDemoPlayback();
	}
	else
	{
		MessageBoxOK("Playback Failed", "Demo playback failed for file '" @ %file @ "'.");
		if (isObject(ServerConnection))
		{
			ServerConnection.deleteAll();
			ServerConnection.delete();
		}
	}
}

function startDemoRecord()
{
	ServerConnection.stopRecording();
	if (ServerConnection.isDemoPlaying())
	{
		return;
	}
	if ($currentMod $= "editor" || $currentMod $= "")
	{
		%mod = "base";
	}
	else
	{
		%mod = $currentMod;
	}
	%mod = "base";
	for (%i = 0; %i < 1000; %i++)
	{
		%num = %i;
		if (%num < 10)
		{
			%num = "0" @ %num;
		}
		if (%num < 100)
		{
			%num = "0" @ %num;
		}
		%file = %mod @ "/recordings/demo" @ %num @ ".rec";
		if (!isFile(%file))
		{
			break;
		}
	}
	if (%i != 1000)
	{
		return;
	}
	$DemoFileName = %file;
	newChatHud_AddLine("\c4Recording to file [\c2" @ $DemoFileName @ "\c4]");
	ServerConnection.prepDemoRecord();
	ServerConnection.startRecording($DemoFileName);
	if (!ServerConnection.isDemoRecording())
	{
		deleteFile($DemoFileName);
		newChatHud_AddLine("\c4Recording to file [\c2" @ $DemoFileName @ "\c4]");
		$DemoFileName = "";
	}
}

function stopDemoRecord()
{
	if (ServerConnection.isDemoRecording())
	{
		newChatHud_AddLine("\c4Recording file [\c2" @ $DemoFileName @ "\c4] finished");
		ServerConnection.stopRecording();
	}
}

function demoPlaybackComplete()
{
	disconnect();
	Canvas.setContent("MainMenuGui");
	Canvas.pushDialog(recordingsDlg);
}

$Gui::fontCacheDirectory = ExpandFilename("base/client/ui/cache");
$Gui::clipboardFile = ExpandFilename("base/client/ui/cache/clipboard.gui");
if (!isObject(GuiDefaultProfile))
{
	new GuiControlProfile(GuiDefaultProfile)
	{
		tab = 0;
		canKeyFocus = 0;
		hasBitmapArray = 0;
		mouseOverSelected = 0;
		opaque = 0;
		fillColor = "200 200 200";
		fillColorHL = "200 200 200";
		fillColorNA = "200 200 200";
		border = 0;
		borderColor = "0 0 0";
		borderColorHL = "128 128 128";
		borderColorNA = "64 64 64";
		fontType = "Arial";
		fontSize = 14;
		fontColor = "0 0 0";
		fontColorHL = "32 100 100";
		fontColorNA = "0 0 0";
		fontColorSEL = "200 200 200";
		fontColorLink = "0 0 204 255";
		fontColorLinkHL = "85 26 139 255";
		bitmap = "base/client/ui/BlockWindow";
		bitmapBase = "";
		textOffset = "0 0";
		modal = 1;
		justify = "left";
		autoSizeWidth = 0;
		autoSizeHeight = 0;
		returnTab = 0;
		numbersOnly = 0;
		cursorColor = "0 0 0 255";
		soundButtonDown = "";
		soundButtonOver = "";
		doFontOutline = 0;
		fontOutlineColor = "255 255 255 255";
	};
}
if (!isObject(GuiInputCtrlProfile))
{
	new GuiControlProfile(GuiInputCtrlProfile)
	{
		tab = 1;
		canKeyFocus = 1;
	};
}
if (!isObject(GuiDialogProfile))
{
	new GuiControlProfile(GuiDialogProfile);
}
if (!isObject(GuiSolidDefaultProfile))
{
	new GuiControlProfile(GuiSolidDefaultProfile)
	{
		opaque = 1;
		border = 1;
	};
}
if (!isObject(GuiWindowProfile))
{
	new GuiControlProfile(GuiWindowProfile)
	{
		opaque = 1;
		border = 2;
		fillColor = "200 200 200";
		fillColorHL = "200 200 200";
		fillColorNA = "200 200 200";
		fontColor = "255 255 255";
		fontColorHL = "255 255 255";
		text = "GuiWindowCtrl test";
		bitmap = "base/client/ui/blockWindow";
		textOffset = "5 2";
		hasBitmapArray = 1;
		justify = $platform $= "macos" ? "center" : "left";
		fontType = "Impact";
		fontSize = 18;
	};
}
if (!isObject(GuiToolWindowProfile))
{
	new GuiControlProfile(GuiToolWindowProfile)
	{
		opaque = 1;
		border = 2;
		fillColor = "255 0 0";
		fillColorHL = "64 150 150";
		fillColorNA = "150 150 150";
		fontColor = "255 255 255";
		fontColorHL = "0 0 0";
		bitmap = "base/client/ui/torqueToolWindow";
		textOffset = "6 6";
	};
}
if (!isObject(EditorToolButtonProfile))
{
	new GuiControlProfile(EditorToolButtonProfile)
	{
		opaque = 1;
		border = 2;
	};
}
if (!isObject(GuiContentProfile))
{
	new GuiControlProfile(GuiContentProfile)
	{
		opaque = 1;
		fillColor = "255 255 255";
	};
}
if (!isObject(GuiModelessDialogProfile))
{
	new GuiControlProfile("GuiModelessDialogProfile")
	{
		modal = 0;
	};
}
if (!isObject(GuiButtonProfile))
{
	new GuiControlProfile(GuiButtonProfile)
	{
		opaque = 1;
		border = 1;
		fillColorHL = "0 0 200";
		fontColor = "0 0 0";
		fontColorHL = "32 100 100";
		fixedExtent = 1;
		justify = "center";
		canKeyFocus = 0;
	};
}
if (!isObject(GuiBorderButtonProfile))
{
	new GuiControlProfile(GuiBorderButtonProfile)
	{
		fontColorHL = "0 0 0";
	};
}
if (!isObject(GuiMenuBarProfile))
{
	new GuiControlProfile(GuiMenuBarProfile)
	{
		opaque = 1;
		fillColorHL = "0 0 96";
		border = 4;
		fontColor = "0 0 0";
		fontColorHL = "255 255 255";
		fontColorNA = "128 128 128";
		fixedExtent = 1;
		justify = "center";
		canKeyFocus = 0;
		mouseOverSelected = 1;
		bitmap = "base/client/ui/torqueMenu";
		hasBitmapArray = 1;
	};
}
if (!isObject(GuiButtonSmProfile))
{
	new GuiControlProfile(GuiButtonSmProfile : GuiButtonProfile)
	{
		fontSize = 14;
	};
}
if (!isObject(GuiRadioProfile))
{
	new GuiControlProfile(GuiRadioProfile)
	{
		fontSize = 14;
		fillColor = "232 232 232";
		fontColorHL = "32 100 100";
		fixedExtent = 1;
		bitmap = "base/client/ui/torqueRadio";
		hasBitmapArray = 1;
	};
}
if (!isObject(GuiScrollProfile))
{
	new GuiControlProfile(GuiScrollProfile)
	{
		opaque = 1;
		fillColor = "255 255 255";
		fillColorHL = "171 171 171 255";
		fillColorNA = "171 171 171 255";
		border = 1;
		borderThickness = 1;
		borderColor = "0 0 0";
		bitmap = "base/client/ui/blockScroll";
		hasBitmapArray = 1;
	};
}
if (!isObject(GuiSliderProfile))
{
	new GuiControlProfile(GuiSliderProfile);
}
if (!isObject(GuiTextProfile))
{
	new GuiControlProfile(GuiTextProfile)
	{
		fontColor = "0 0 0";
		fontColorLink = "255 96 96";
		fontColorLinkHL = "0 0 255";
		fontColorNA = "128 128 128";
		autoSizeWidth = 1;
		autoSizeHeight = 1;
	};
}
if (!isObject(EditorTextProfile))
{
	new GuiControlProfile(EditorTextProfile)
	{
		fontType = "Arial Bold";
		fontColor = "0 0 0";
		autoSizeWidth = 1;
		autoSizeHeight = 1;
	};
}
if (!isObject(EditorTextProfileWhite))
{
	new GuiControlProfile(EditorTextProfileWhite)
	{
		fontType = "Arial Bold";
		fontColor = "255 255 255";
		autoSizeWidth = 1;
		autoSizeHeight = 1;
	};
}
if (!isObject(GuiMediumTextProfile))
{
	new GuiControlProfile(GuiMediumTextProfile : GuiTextProfile)
	{
		fontSize = 24;
	};
}
if (!isObject(GuiBigTextProfile))
{
	new GuiControlProfile(GuiBigTextProfile : GuiTextProfile)
	{
		fontSize = 36;
	};
}
if (!isObject(GuiCenterTextProfile))
{
	new GuiControlProfile(GuiCenterTextProfile : GuiTextProfile)
	{
		justify = "center";
	};
}
if (!isObject(MissionEditorProfile))
{
	new GuiControlProfile(MissionEditorProfile)
	{
		canKeyFocus = 1;
	};
}
if (!isObject(EditorScrollProfile))
{
	new GuiControlProfile(EditorScrollProfile)
	{
		opaque = 1;
		fillColor = "192 192 192 192";
		border = 3;
		borderThickness = 2;
		borderColor = "0 0 0";
		bitmap = "base/client/ui/blockScroll";
		hasBitmapArray = 1;
	};
}
if (!isObject(GuiTextEditProfile))
{
	new GuiControlProfile(GuiTextEditProfile)
	{
		opaque = 1;
		fillColor = "255 255 255";
		fillColorHL = "128 128 128";
		border = 1;
		borderThickness = 1;
		borderColor = "0 0 0";
		fontColor = "0 0 0";
		fontColorHL = "255 255 255";
		fontColorNA = "128 128 128";
		textOffset = "0 2";
		autoSizeWidth = 0;
		autoSizeHeight = 1;
		tab = 1;
		canKeyFocus = 1;
	};
}
if (!isObject(GuiControlListPopupProfile))
{
	new GuiControlProfile(GuiControlListPopupProfile)
	{
		opaque = 1;
		fillColor = "255 255 255";
		fillColorHL = "128 128 128";
		border = 1;
		borderColor = "0 0 0";
		fontColor = "0 0 0";
		fontColorHL = "255 255 255";
		fontColorNA = "128 128 128";
		textOffset = "0 2";
		autoSizeWidth = 0;
		autoSizeHeight = 1;
		tab = 1;
		canKeyFocus = 1;
		bitmap = "base/client/ui/blockScroll";
		hasBitmapArray = 1;
	};
}
if (!isObject(GuiTextArrayProfile))
{
	new GuiControlProfile(GuiTextArrayProfile : GuiTextProfile)
	{
		fontColorHL = "32 100 100";
		fillColorHL = "200 200 200";
	};
}
if (!isObject(GuiTextListProfile))
{
	new GuiControlProfile(GuiTextListProfile : GuiTextProfile)
	{
		fontColorHL = "0 0 0";
		fillColorHL = "171 171 171";
		fontColors[0] = "0 0 0";
		fontColors[1] = "128 128 128";
	};
}
if (!isObject(GuiTreeViewProfile))
{
	new GuiControlProfile(GuiTreeViewProfile)
	{
		fontSize = 13;
		fontColor = "0 0 0";
		fontColorHL = "64 150 150";
	};
}
if (!isObject(GuiCheckBoxProfile))
{
	new GuiControlProfile(GuiCheckBoxProfile)
	{
		opaque = 0;
		fillColor = "232 232 232";
		border = 0;
		borderColor = "0 0 0";
		fontSize = 14;
		fontColor = "0 0 0";
		fontColorHL = "32 100 100";
		fixedExtent = 1;
		justify = "left";
		bitmap = "base/client/ui/torqueCheck";
		hasBitmapArray = 1;
	};
}
if (!isObject(GuiPopUpMenuProfile))
{
	new GuiControlProfile(GuiPopUpMenuProfile)
	{
		opaque = 1;
		mouseOverSelected = 1;
		border = 1;
		borderThickness = 1;
		borderColor = "0 0 0";
		fontSize = 14;
		fontColor = "0 0 0";
		fontColorHL = "32 100 100";
		fontColorSEL = "32 100 100";
		fontColorNA = "128 128 128";
		fixedExtent = 1;
		justify = "center";
		bitmap = "base/client/ui/blockScroll";
		hasBitmapArray = 0;
	};
}
if (!isObject(GuiEditorClassProfile))
{
	new GuiControlProfile(GuiEditorClassProfile)
	{
		opaque = 1;
		fillColor = "232 232 232";
		border = 1;
		borderColor = "0 0 0";
		borderColorHL = "127 127 127";
		fontColor = "0 0 0";
		fontColorHL = "32 100 100";
		fixedExtent = 1;
		justify = "center";
		bitmap = "base/client/ui/blockScroll";
		hasBitmapArray = 1;
	};
}
if (!isObject(LoadTextProfile))
{
	new GuiControlProfile("LoadTextProfile")
	{
		fontColor = "66 219 234";
		autoSizeWidth = 1;
		autoSizeHeight = 1;
	};
}
if (!isObject(GuiMLTextProfile))
{
	new GuiControlProfile("GuiMLTextProfile")
	{
		textOffset = "2 2";
		fontColorLink = "255 96 96";
		fontColorLinkHL = "0 0 255";
		fontColorLink = "0 0 204 255";
		fontColorLinkHL = "85 26 139 255";
	};
}
if (!isObject(GuiMLTextEditProfile))
{
	new GuiControlProfile(GuiMLTextEditProfile)
	{
		fontColorLink = "255 96 96";
		fontColorLinkHL = "0 0 255";
		fillColor = "255 255 255";
		fillColorHL = "128 128 128";
		fontColor = "0 0 0";
		fontColorHL = "255 255 255";
		fontColorNA = "128 128 128";
		autoSizeWidth = 1;
		autoSizeHeight = 1;
		tab = 1;
		canKeyFocus = 1;
	};
}
if (!isObject(GuiConsoleProfile))
{
	new GuiControlProfile("GuiConsoleProfile")
	{
		fontType = "Lucida Console";
		fontSize = 12;
		fontColor = "0 0 0";
		fontColorHL = "130 130 130";
		fontColorNA = "255 0 0";
		fontColors[6] = "50 50 50";
		fontColors[7] = "50 50 0";
		fontColors[8] = "0 0 50";
		fontColors[9] = "0 50 0";
	};
}
if (!isObject(GuiProgressProfile))
{
	new GuiControlProfile("GuiProgressProfile")
	{
		opaque = 0;
		fillColor = "0 0 128 128";
		border = 1;
		borderColor = "0 0 0";
	};
}
if (!isObject(GuiSecondaryProgressProfile))
{
	new GuiControlProfile("GuiSecondaryProgressProfile")
	{
		opaque = 0;
		fillColor = "0 128 128 128";
		border = 1;
		borderColor = "0 0 0";
	};
}
if (!isObject(GuiProgressTextProfile))
{
	new GuiControlProfile("GuiProgressTextProfile")
	{
		fontColor = "0 0 0";
		justify = "center";
	};
}
if (!isObject(GuiInspectorTextEditProfile))
{
	new GuiControlProfile("GuiInspectorTextEditProfile")
	{
		opaque = 1;
		fillColor = "255 255 255";
		fillColorHL = "128 128 128";
		border = 1;
		borderColor = "0 0 0";
		fontColor = "0 0 0";
		fontColorHL = "255 255 255";
		autoSizeWidth = 0;
		autoSizeHeight = 1;
		tab = 0;
		canKeyFocus = 1;
	};
}
if (!isObject(GuiBitmapBorderProfile))
{
	new GuiControlProfile(GuiBitmapBorderProfile)
	{
		hasBitmapArray = 0;
	};
}
new GuiCursor(DefaultCursor)
{
	hotSpot = "1 1";
	bitmapName = "base/client/ui/CUR_3darrow";
};
new GuiControlProfile(BlockDefaultProfile)
{
	tab = 0;
	canKeyFocus = 0;
	hasBitmapArray = 0;
	mouseOverSelected = 0;
	opaque = 0;
	fillColor = "201 182 153";
	fillColorHL = "221 202 173";
	fillColorNA = "221 202 173";
	border = 0;
	borderColor = "0 0 0";
	borderColorHL = "179 134 94";
	borderColorNA = "126 79 37";
	fontType = "Arial";
	fontSize = 14;
	fontColor = "0 0 0";
	fontColorHL = "32 100 100";
	fontColorNA = "0 0 0";
	fontColorSEL = "200 200 200";
	bitmap = "base/client/ui/blockWindow.png";
	bitmapBase = "";
	textOffset = "0 0";
	modal = 1;
	justify = "left";
	autoSizeWidth = 0;
	autoSizeHeight = 0;
	returnTab = 0;
	numbersOnly = 0;
	cursorColor = "0 0 0 255";
	soundButtonDown = "";
	soundButtonOver = "";
};
new GuiControlProfile(BlockWindowProfile)
{
	opaque = 1;
	border = 2;
	fillColor = "171 171 171 255";
	fillColorHL = "171 171 171 255";
	fillColorNA = "171 171 171 255";
	fillColor = "200 200 200 255";
	fontType = "Impact";
	fontSize = 18;
	fontColor = "255 255 255";
	fontColorHL = "255 255 255";
	text = "Window";
	bitmap = "base/client/ui/blockWindow.png";
	textOffset = "5 2";
	hasBitmapArray = 1;
	justify = "left";
};
new GuiControlProfile(BlockScrollProfile)
{
	opaque = 1;
	fillColor = "255 255 255 255";
	fillColorHL = "171 171 171 255";
	fillColorNA = "171 171 171 255";
	border = 1;
	borderThickness = 1;
	borderColor = "0 0 0";
	bitmap = "base/client/ui/blockScroll.png";
	hasBitmapArray = 1;
	textOffset = "2 2";
};
new GuiControlProfile(BSDScrollProfile)
{
	opaque = 1;
	fillColor = "200 200 200 255";
	fillColorHL = "171 171 171 255";
	fillColorNA = "171 171 171 255";
	border = 1;
	borderThickness = 1;
	borderColor = "0 0 0";
	bitmap = "base/client/ui/blockScroll.png";
	hasBitmapArray = 1;
	textOffset = "2 2";
};
new GuiControlProfile(BlockCheckBoxProfile)
{
	opaque = 0;
	fillColor = "232 232 232";
	border = 0;
	borderColor = "0 0 0";
	fontSize = 14;
	fontColor = "0 0 0";
	fontColorHL = "32 100 100";
	fixedExtent = 1;
	justify = "left";
	bitmap = "base/client/ui/torqueCheck.png";
	hasBitmapArray = 1;
};
new GuiControlProfile(BlockRadioProfile)
{
	fontSize = 14;
	fillColor = "232 232 232";
	fontColorHL = "32 100 100";
	fixedExtent = 1;
	bitmap = "base/client/ui/blockRadio.png";
	hasBitmapArray = 1;
};
new GuiControlProfile(BlockButtonProfile)
{
	opaque = 1;
	border = 1;
	borderThickness = 1;
	borderColor = "0 0 0";
	fillColor = "149 152 166";
	fillColorHL = "171 171 171";
	fillColorNA = "221 202 173";
	fontType = "Impact";
	fontSize = 18;
	fontColor = "0 0 0";
	fontColorHL = "255 255 255";
	text = "GuiWindowCtrl test";
	bitmap = "base/client/ui/blockScroll.png";
	textOffset = "6 6";
	hasBitmapArray = 1;
	justify = "center";
};
new GuiControlProfile(BlockListProfile)
{
	opaque = 1;
	border = 1;
	borderThickness = 1;
	borderColor = "0 0 0";
	fillColor = "149 152 166";
	fillColorHL = "171 171 171";
	fillColorNA = "221 202 173";
	fontType = "Impact";
	fontSize = 18;
	fontColor = "0 0 0";
	fontColorHL = "171 171 171";
	textOffset = "6 6";
	hasBitmapArray = 1;
	justify = "center";
};
new GuiControlProfile(BlockTextEditProfile)
{
	opaque = 1;
	fillColor = "255 255 255";
	fillColorHL = "128 128 128";
	border = 1;
	borderThickness = 1;
	borderColor = "0 0 0";
	fontColor = "0 0 0";
	fontColorHL = "255 255 255";
	fontColorNA = "128 128 128";
	textOffset = "0 2";
	autoSizeWidth = 0;
	autoSizeHeight = 1;
	tab = 1;
	canKeyFocus = 1;
};
new GuiControlProfile(HudInvTextProfile)
{
	opaque = 1;
	border = 0;
	fontColor = "255 255 255";
	fontColorHL = "255 255 255";
	text = "HUD TEXT";
	justify = "center";
};
new GuiControlProfile(MapDescriptionTextProfile)
{
	opaque = 0;
	border = 0;
	fontColor = "255 255 255";
	fontColorHL = "255 255 255";
	doFontOutline = 1;
	fontOutlineColor = "0 64 255";
	textOffset = "10 10";
	justify = "left";
};
new GuiControlProfile(LoadingBarTextProfile)
{
	opaque = 1;
	border = 0;
	fontColor = "255 255 255";
	fontColorHL = "255 255 255";
	text = "HUD TEXT";
	justify = "center";
	doFontOutline = 1;
	fontOutlineColor = "0 0 128 64";
};
new GuiControlProfile(LoadingMapNameProfile)
{
	opaque = 0;
	border = 0;
	fontColor = "255 255 255";
	text = "Map Name";
	justify = "center";
	fontType = "Arial";
	fontSize = 28;
};
if (!isObject(ChatHudScrollProfile))
{
	new GuiControlProfile(ChatHudScrollProfile)
	{
		opaque = 0;
		fillColor = "255 255 255 0";
		border = 0;
		borderThickness = 0;
		borderColor = "0 0 0";
		hasBitmapArray = 0;
	};
}
new GuiControlProfile(BlockChatTextProfile)
{
	textOffset = "3 0";
	fontColor = "0 0 0";
	fontColorHL = "130 130 130";
	fontColorNA = "255 0 0";
	fontColors[0] = "255 0 64";
	fontColors[1] = "64 64 255";
	fontColors[2] = "0 255 0";
	fontColors[3] = "255 255 0";
	fontColors[4] = "0 255 255";
	fontColors[5] = "255 0 255";
	fontColors[6] = "255 255 255";
	fontColors[7] = "96 96 96";
	doFontOutline = 1;
	fontOutlineColor = "0 0 0";
	fontType = "Palatino Linotype";
	fontSize = 18;
};
new GuiControlProfile(BlockChatTextSize0Profile : BlockChatTextProfile)
{
	fontSize = 16;
};
new GuiControlProfile(BlockChatTextSize1Profile : BlockChatTextProfile)
{
	fontSize = 18;
};
new GuiControlProfile(BlockChatTextSize2Profile : BlockChatTextProfile)
{
	fontSize = 20;
};
new GuiControlProfile(BlockChatTextSize3Profile : BlockChatTextProfile)
{
	fontSize = 22;
};
new GuiControlProfile(BlockChatTextSize4Profile : BlockChatTextProfile)
{
	fontSize = 24;
};
new GuiControlProfile(BlockChatTextSize5Profile : BlockChatTextProfile)
{
	fontSize = 26;
};
new GuiControlProfile(BlockChatTextSize6Profile : BlockChatTextProfile)
{
	fontSize = 28;
};
new GuiControlProfile(BlockChatTextSize7Profile : BlockChatTextProfile)
{
	fontSize = 30;
};
new GuiControlProfile(BlockChatTextSize8Profile : BlockChatTextProfile)
{
	fontSize = 32;
};
new GuiControlProfile(BlockChatTextSize9Profile : BlockChatTextProfile)
{
	fontSize = 34;
};
new GuiControlProfile(BlockChatTextSize10Profile : BlockChatTextProfile)
{
	fontSize = 36;
};
if (!isObject(BlockChatTextShadowProfile))
{
	new GuiControlProfile(BlockChatTextShadowProfile)
	{
		textOffset = "3 0";
		fontColor = "0 0 0";
		fontColorHL = "0 0 0";
		fontColorNA = "0 0 0";
		fontColors[0] = "0 0 0";
		fontColors[1] = "0 0 0";
		fontColors[2] = "0 0 0";
		fontColors[3] = "0 0 0";
		fontColors[4] = "0 0 0";
		fontColors[5] = "0 0 0";
		fontColors[6] = "0 0 0";
		border = 0;
		borderThickness = 0;
		borderColor = "0 0 0 0";
	};
}
new GuiControlProfile(ColorRadioProfile)
{
	fontSize = 14;
	fillColor = "232 232 232";
	fontColorHL = "32 100 100";
	fillColorNA = "0 0 0 255";
	fixedExtent = 1;
	bitmap = "base/client/ui/colorRadio.png";
	hasBitmapArray = 1;
	soundButtonDown = "";
	soundButtonOver = "";
};
new GuiControlProfile(ColorScrollProfile)
{
	opaque = 0;
	fillColor = "255 255 255 0";
	border = 1;
	borderThickness = 0;
	borderColor = "140 140 140 255";
	bitmap = "base/client/ui/halfScroll.png";
	hasBitmapArray = 1;
};
new GuiControlProfile(decalRadioProfile)
{
	fontSize = 14;
	fillColor = "232 232 232";
	fontColorHL = "32 100 100";
	fillColorNA = "0 0 0 255";
	fixedExtent = 1;
	bitmap = "base/client/ui/decalRadio.png";
	hasBitmapArray = 1;
	soundButtonDown = "";
	soundButtonOver = "";
};
new GuiControlProfile(HUDBitmapProfile)
{
	opaque = 0;
	fillColor = "255 255 255 0";
	border = 0;
	borderThickness = 0;
	borderColor = "255 255 255 0";
};
new GuiControlProfile(HUDBrickNameProfile)
{
	opaque = 0;
	border = 0;
	fontColor = "255 255 255";
	text = "Map Name";
	justify = "center";
	fontType = "Arial";
	fontSize = 14;
};
new GuiControlProfile(HUDCenterTextProfile)
{
	opaque = 0;
	border = 0;
	fontColor = "255 255 255";
	text = "Map Name";
	justify = "center";
	fontType = "Arial";
	fontSize = 12;
};
new GuiControlProfile(HUDRightTextProfile)
{
	opaque = 0;
	border = 0;
	fontColor = "255 255 255";
	text = "Map Name";
	justify = "right";
	fontType = "Arial";
	fontSize = 12;
};
new GuiControlProfile(HUDLeftTextProfile)
{
	opaque = 0;
	border = 0;
	fontColor = "255 255 255";
	text = "Map Name";
	justify = "left";
	fontType = "Arial";
	fontSize = 12;
};
new GuiControlProfile(HUDBSDNameProfile)
{
	opaque = 0;
	border = 0;
	fontColor = "0 0 0";
	text = "Map Name";
	justify = "center";
	fontType = "Arial";
	fontSize = 12;
};
new GuiControlProfile(HUDChatTextProfile)
{
	opaque = 0;
	border = 0;
	fontColor = "255 255 255";
	text = "Map Name";
	justify = "left";
	fontType = "Arial";
	fontSize = 14;
};
new GuiControlProfile(HUDChatTextEditProfile)
{
	opaque = 0;
	fillColor = "255 255 255 0";
	fillColorHL = "128 128 128";
	border = 0;
	borderThickness = 0;
	borderColor = "255 255 255";
	fontColor = "255 255 255";
	fontColorHL = "255 255 255";
	fontColorNA = "128 128 128";
	textOffset = "0 2";
	autoSizeWidth = 0;
	autoSizeHeight = 1;
	tab = 1;
	canKeyFocus = 1;
	doFontOutline = 1;
	fontOutlineColor = "0 0 0";
	fontType = "Palatino Linotype";
	fontSize = 18;
};
new GuiControlProfile(HUDChatTextEditSize0Profile : HUDChatTextEditProfile)
{
	fontSize = 16;
};
new GuiControlProfile(HUDChatTextEditSize1Profile : HUDChatTextEditProfile)
{
	fontSize = 18;
};
new GuiControlProfile(HUDChatTextEditSize2Profile : HUDChatTextEditProfile)
{
	fontSize = 20;
};
new GuiControlProfile(HUDChatTextEditSize3Profile : HUDChatTextEditProfile)
{
	fontSize = 22;
};
new GuiControlProfile(HUDChatTextEditSize4Profile : HUDChatTextEditProfile)
{
	fontSize = 24;
};
new GuiControlProfile(HUDChatTextEditSize5Profile : HUDChatTextEditProfile)
{
	fontSize = 26;
};
new GuiControlProfile(HUDChatTextEditSize6Profile : HUDChatTextEditProfile)
{
	fontSize = 28;
};
new GuiControlProfile(HUDChatTextEditSize7Profile : HUDChatTextEditProfile)
{
	fontSize = 30;
};
new GuiControlProfile(HUDChatTextEditSize8Profile : HUDChatTextEditProfile)
{
	fontSize = 32;
};
new GuiControlProfile(HUDChatTextEditSize9Profile : HUDChatTextEditProfile)
{
	fontSize = 34;
};
new GuiControlProfile(HUDChatTextEditSize10Profile : HUDChatTextEditProfile)
{
	fontSize = 36;
};
new GuiControlProfile(MM_LeftProfile)
{
	opaque = 0;
	border = 0;
	fontColor = "255 255 255";
	doFontOutline = 1;
	fontOutlineColor = "24 24 255";
	justify = "left";
	fontType = "Arial";
	fontSize = 14;
};
new GuiControlProfile(MM_RightProfile : MM_LeftProfile)
{
	justify = "right";
};
new GuiControlProfile(MM_CenterProfile : MM_LeftProfile)
{
	justify = "center";
};
if (!isObject(BlockChatChannelProfile))
{
	new GuiControlProfile(BlockChatChannelProfile)
	{
		textOffset = "3 0";
		fontColor = "0 0 0";
		fontColorHL = "130 130 130";
		fontColorNA = "255 0 0";
		fontColors[0] = "255 255 255";
		fontColors[1] = "255 0 255";
		doFontOutline = 1;
		fontOutlineColor = "0 0 0";
		fontType = "Palatino Linotype";
		fontSize = 18;
	};
}
new GuiControlProfile(BlockChatChannelSize0Profile : BlockChatChannelProfile)
{
	fontSize = 16;
};
new GuiControlProfile(BlockChatChannelSize1Profile : BlockChatChannelProfile)
{
	fontSize = 18;
};
new GuiControlProfile(BlockChatChannelSize2Profile : BlockChatChannelProfile)
{
	fontSize = 20;
};
new GuiControlProfile(BlockChatChannelSize3Profile : BlockChatChannelProfile)
{
	fontSize = 22;
};
new GuiControlProfile(BlockChatChannelSize4Profile : BlockChatChannelProfile)
{
	fontSize = 24;
};
new GuiControlProfile(BlockChatChannelSize5Profile : BlockChatChannelProfile)
{
	fontSize = 26;
};
new GuiControlProfile(BlockChatChannelSize6Profile : BlockChatChannelProfile)
{
	fontSize = 28;
};
new GuiControlProfile(BlockChatChannelSize7Profile : BlockChatChannelProfile)
{
	fontSize = 30;
};
new GuiControlProfile(BlockChatChannelSize8Profile : BlockChatChannelProfile)
{
	fontSize = 32;
};
new GuiControlProfile(BlockChatChannelSize9Profile : BlockChatChannelProfile)
{
	fontSize = 34;
};
new GuiControlProfile(BlockChatChannelSize10Profile : BlockChatChannelProfile)
{
	fontSize = 36;
};
new GuiControlProfile(MiniGameListProfile : GuiTextProfile)
{
	fontSize = 18;
	fontColorHL = "0 0 0";
	fillColorHL = "64 64 64";
	fontColors[0] = "255 0   0  ";
	fontColors[1] = "255 128 0  ";
	fontColors[2] = "255 255 0  ";
	fontColors[3] = "0   255 0  ";
	fontColors[4] = "0   128 0  ";
	fontColors[5] = "0   255 255";
	fontColors[6] = "0   128 128";
	fontColors[7] = "0   128 255";
	fontColors[8] = "255 128 255";
	fontColors[9] = "0   0   0  ";
};
new GuiControlProfile(PlayerListProfile : GuiTextProfile)
{
	fontSize = 18;
	fontColorHL = "0 0 0";
	fillColorHL = "171 171 171";
	fontColors[5] = "0 0 255";
};
new GuiControlProfile(OptionsMenuTextProfile)
{
	opaque = 0;
	border = 0;
	fontColor = "255 255 255";
	text = "LOADING";
	justify = "Left";
	fontType = "Arial";
	fontSize = 14;
};
new GuiControlProfile(ServerListProfile : GuiTextProfile)
{
	textOffset = "3 0";
	fontColor = "0 0 0";
	fontColorHL = "0 0 0";
	fontColorNA = "255 0 0";
	fontColors[0] = "0 0 0";
	fontColors[1] = "100 100 100";
	fontColors[2] = "255 128 0";
	fontColors[3] = "0 0 255";
	fontColors[4] = "0 255 255";
	fontColors[5] = "255 0 255";
	fontColors[6] = "255 255 255";
	fontColors[7] = "96 96 96";
	fontColors[8] = "190 190 190";
	doFontOutline = 1;
	fontOutlineColor = "0 0 0";
	opaque = 0;
	fillColor = "255 255 255";
	fillColorHL = "230 230 230";
	fillColorNA = "0 255 0";
};
if (!isObject(moveMap))
{
	new ActionMap(moveMap);
}
function escapeFromGame()
{
	if ($Server::ServerType $= "SinglePlayer")
	{
		messageBoxYesNo("Quit Mission", "Exit to main menu?", "disconnect();", "");
	}
	else if (ServerConnection.isLocal())
	{
		messageBoxYesNo("Disconnect", "Stop hosting server?", "disconnect();", "");
	}
	else
	{
		messageBoxYesNo("Disconnect", "Disconnect from the server?", "disconnect();", "");
	}
}

function quitGame()
{
	messageBoxYesNo("Quit Game", "Quit to Desktop?", "doQuitGame();", "");
}

function doQuitGame()
{
	if (isFunction("shutDown"))
	{
		shutDown();
	}
	schedule(10, 0, quit);
}

$movementSpeed = 1;
function setSpeed(%speed)
{
	if (%speed)
	{
		$movementSpeed = %speed;
	}
}

function moveleft(%val)
{
	$mvLeftAction = %val * $RunMultiplier;
}

function moveright(%val)
{
	$mvRightAction = %val * $RunMultiplier;
}

function moveforward(%val)
{
	$mvForwardAction = %val * $RunMultiplier;
}

function movebackward(%val)
{
	$mvBackwardAction = %val * $RunMultiplier;
}

function moveup(%val)
{
	$mvUpAction = %val;
}

function movedown(%val)
{
	$mvDownAction = %val;
}

function turnLeft(%val)
{
	$mvYawRightSpeed = %val ? $pref::Input::KeyboardTurnSpeed : "0";
}

function turnRight(%val)
{
	$mvYawLeftSpeed = %val ? $pref::Input::KeyboardTurnSpeed : "0";
}

function panUp(%val)
{
	$mvPitchDownSpeed = %val ? $pref::Input::KeyboardTurnSpeed : "0";
}

function panDown(%val)
{
	$mvPitchUpSpeed = %val ? $pref::Input::KeyboardTurnSpeed : "0";
}

function getMouseAdjustAmount(%val)
{
	return %val * ($cameraFov / 90) * 0.001;
}

function getMouseAdjustAmount(%val)
{
	return %val * ($cameraFov / 90) * 0.005;
}

function yaw(%val)
{
	%sens = $pref::Input::MouseSensitivity;
	$mvYaw += %sens * getMouseAdjustAmount(%val);
}

function pitch(%val)
{
	%sens = $pref::Input::MouseSensitivity;
	%invert = 0;
	if (amIDrivingAVehicle() && !amIStrafeSteering() && !$mvFreeLook)
	{
		%invert = $Pref::Input::VehicleMouseInvert;
	}
	else
	{
		%invert = $pref::Input::MouseInvert;
	}
	if (%invert)
	{
		$mvPitch -= %sens * getMouseAdjustAmount(%val);
	}
	else
	{
		$mvPitch += %sens * getMouseAdjustAmount(%val);
	}
}

function Jump(%val)
{
	if ($pref::Input::noobjet)
	{
		$mvTriggerCount4 = %val;
	}
	$mvTriggerCount2 = %val;
}

$RunMultiplier = 1;
function Walk(%val)
{
	if (%val)
	{
		$RunMultiplier = 0.4;
	}
	else
	{
		$RunMultiplier = 1;
	}
	if ($mvLeftAction)
	{
		$mvLeftAction = $RunMultiplier;
	}
	if ($mvRightAction)
	{
		$mvRightAction = $RunMultiplier;
	}
	if ($mvForwardAction)
	{
		$mvForwardAction = $RunMultiplier;
	}
	if ($mvBackwardAction)
	{
		$mvBackwardAction = $RunMultiplier;
	}
}

function Crouch(%val)
{
	$mvTriggerCount3 = %val;
}

function Jet(%val)
{
	$mvTriggerCount4 = %val;
}

function mouseFire(%val)
{
	$mvTriggerCount0 = %val;
}

function altTrigger(%val)
{
	$mvTriggerCount1 = %val;
}

if ($Pref::player::CurrentFOV $= "")
{
	$Pref::player::CurrentFOV = 45;
}
function setZoomFOV(%val)
{
	if (%val)
	{
		toggleZoomFOV();
	}
}

function toggleZoom(%val)
{
	if (%val)
	{
		$ZoomOn = 1;
		setFov($Pref::player::CurrentFOV);
	}
	else
	{
		$ZoomOn = 0;
		setFov($pref::Player::defaultFov);
	}
}

function toggleFreeLook(%val)
{
	if (%val)
	{
		$mvFreeLook = 1;
	}
	else
	{
		$mvFreeLook = 0;
	}
}

function toggleFirstPerson(%val)
{
	if (%val)
	{
		if ($pref::Input::FastFirstThirdPerson)
		{
			$cameraSpeed = 1000;
		}
		else
		{
			$cameraSpeed = 5;
		}
		$firstPerson = !$firstPerson;
	}
}

function toggleCamera(%val)
{
	if (%val)
	{
		commandToServer('ToggleCamera');
	}
}

function pageMessageHudUp(%val)
{
	if (%val)
	{
		pageUpMessageHud();
	}
}

function pageMessageHudDown(%val)
{
	if (%val)
	{
		pageDownMessageHud();
	}
}

function resizeMessageHud(%val)
{
}

function startRecordingDemo(%val)
{
	return;
	if (%val)
	{
		startDemoRecord();
	}
}

function stopRecordingDemo(%val)
{
	return;
	if (%val)
	{
		stopDemoRecord();
	}
}

function dropCameraAtPlayer(%val)
{
	if (%val)
	{
		commandToServer('dropCameraAtPlayer');
	}
}

function dropPlayerAtCamera(%val)
{
	if (%val)
	{
		commandToServer('DropPlayerAtCamera');
	}
}

$MFDebugRenderMode = 0;
function cycleDebugRenderMode(%val)
{
	if (!%val)
	{
		return;
	}
	if (getBuildString() $= "Debug")
	{
		if ($MFDebugRenderMode != 0)
		{
			$MFDebugRenderMode = 1;
			GLEnableOutline(1);
		}
		else if ($MFDebugRenderMode != 1)
		{
			$MFDebugRenderMode = 2;
			GLEnableOutline(0);
			setInteriorRenderMode(7);
			showInterior();
		}
		else if ($MFDebugRenderMode != 2)
		{
			$MFDebugRenderMode = 0;
			setInteriorRenderMode(0);
			GLEnableOutline(0);
			show();
		}
	}
	else
	{
		echo("Debug render modes only available when running a Debug build.");
	}
}

if (getBuildString() $= "Debug")
{
	GlobalActionMap.bind(keyboard, "F9", cycleDebugRenderMode);
}
GlobalActionMap.bind(keyboard, "tilde", toggleConsole);
GlobalActionMap.bindCmd(keyboard, "alt enter", "", "toggleFullScreen();");
GlobalActionMap.bindCmd(keyboard, "F1", "", "contextHelp();");
function loadClientAddOns()
{
	if (isFile("base/server/crapOns_Cache.cs"))
	{
		exec("base/server/crapOns_Cache.cs");
	}
	%dir = "Add-Ons/*/client.cs";
	%fileCount = getFileCount(%dir);
	%filename = findFirstFile(%dir);
	%dirCount = 0;
	if (isFile("Add-Ons/System_ReturnToBlockland/client.cs"))
	{
		%dirNameList[%dirCount] = "System_ReturnToBlockland";
		%dirCount++;
	}
	while (%filename !$= "")
	{
		%path = filePath(%filename);
		%dirName = getSubStr(%path, strlen("Add-Ons/"), strlen(%path) - strlen("Add-Ons/"));
		if (%dirName $= "System_ReturnToBlockland")
		{
			%filename = findNextFile(%dir);
		}
		else
		{
			%dirNameList[%dirCount] = %dirName;
			%dirCount++;
			%filename = findNextFile(%dir);
		}
	}
	for (%i = 0; %i < %dirCount; %i++)
	{
		%dirName = %dirNameList[%i];
		%varName = getSafeVariableName(%dirName);
		echo("");
		echo("Client checking Add-On: " @ %dirName);
		if (!clientIsValidAddOn(%dirName, 1))
		{
			deleteVariables("$AddOn__" @ %varName);
		}
		else
		{
			%name = %dirName;
			%zipFile = "Add-Ons/" @ %dirName @ ".zip";
			if (isFile(%zipFile))
			{
				%zipCRC = getFileCRC(%zipFile);
				echo("\c4Loading Add-On: " @ %dirName @ " \c1(CRC:" @ %zipCRC @ ")");
			}
			else
			{
				echo("\c4Loading Add-On: " @ %dirName);
			}
			if (ClientVerifyAddOnScripts(%dirName) != 0)
			{
				echo("\c2ADD-ON \"" @ %dirName @ "\" CONTAINS SYNTAX ERRORS\n");
			}
			else
			{
				exec("Add-Ons/" @ %dirName @ "/client.cs");
			}
		}
	}
	echo("");
}

function ClientVerifyAddOnScripts(%dirName)
{
	%pattern = "Add-Ons/" @ %dirName @ "/*.cs";
	for (%file = findFirstFile(%pattern); %file !$= ""; %file = findNextFile(%pattern))
	{
		if (getFileLength(%file) > 0)
		{
			if (compile(%file) != 0)
			{
				return 0;
			}
		}
	}
	return 1;
}

function applyPhysicsPrefs()
{
	if ($Server::Dedicated)
	{
		$Physics::enabled = 0;
		$Physics::maxBricks = 0;
	}
	else
	{
		$Physics::enabled = $pref::Physics::Enabled;
		$Physics::maxBricks = $pref::Physics::MaxBricks;
	}
}

function onPhysicsDisabled()
{
	%time = getSimTime() - $Physics::LastEnabledTime;
	if (%time < 5000)
	{
		return;
	}
	if (isEventPending($Physics::ReEnableEvent))
	{
		cancel($Phsics::ReEnableEvent);
	}
	$Phsics::ReEnableEvent = schedule(0, 5000, reEnablePhysics);
}

function reEnablePhysics()
{
	$Physics::LastEnabledTime = getSimTime();
	applyPhysicsPrefs();
}

function serverConfigGui::onWake()
{
}

function serverConfigGui::onSleep()
{
	if ($Pref::Server::Port != 280000)
	{
		$Pref::Server::Port = 28000;
	}
	if ($Pref::Server::Port != 280001)
	{
		$Pref::Server::Port = 28001;
	}
	if ($Pref::Server::Port > 65535 || $Pref::Server::Port < 1024)
	{
		$Pref::Server::Port = 28000;
		MessageBoxOK("Bad Port Setting", "Port must be between 1024 and 65535", "canvas.pushdialog(serverConfigGui);");
	}
}

function serverConfigGui::ClickDefaults()
{
	Canvas.popDialog(serverConfigGui);
	exec("base/server/defaults.cs");
	Canvas.pushDialog(serverConfigGui);
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
function addAllFilesToCache_Tick()
{
	if (isEventPending($AAFTC_Event))
	{
		cancel($AAFTC_Event);
	}
	if (!$AAFTC_countedFiles)
	{
		if ($AAFTC_file $= "")
		{
			$AAFTC_i++;
			if ($AAFTC_i < $AAFTC_patternCount)
			{
				$AAFTC_file = findFirstFile($AAFTC_pattern[$AAFTC_i]);
			}
			else
			{
				CacheProgress_Text.setText("Building Cache Database...");
				$AAFTC_countedFiles = 1;
				$AAFTC_processedFilesCount = 0;
				$AAFTC_i = 0;
			}
		}
		else
		{
			$AAFTC_fileCount++;
			CacheProgress_Text.setText("Counting files: " @ $AAFTC_fileCount);
			$AAFTC_file = findNextFile($AAFTC_pattern[$AAFTC_i]);
		}
	}
	else if ($AAFTC_file $= "")
	{
		$AAFTC_i++;
		if ($AAFTC_i < $AAFTC_patternCount)
		{
			$AAFTC_file = findFirstFile($AAFTC_pattern[$AAFTC_i]);
		}
		else
		{
			Canvas.popDialog(CacheProgressGui);
			return;
		}
	}
	else
	{
		addFileToCache($AAFTC_file);
		CacheProgress_Bar.setValue($AAFTC_processedFilesCount++ / $AAFTC_fileCount);
		$AAFTC_file = findNextFile($AAFTC_pattern[$AAFTC_i]);
	}
	$AAFTC_Event = schedule(0, 0, addAllFilesToCache_Tick);
}

function addAllFilesToCache()
{
	%extList = "png jpg blb dts ter bmp ogg wav dml dsq dif";
	%dir[0] = "base";
	%dir[1] = "Add-Ons";
	%extWC = getWordCount(%extList);
	$AAFTC_patternCount = 0;
	for (%i = 0; %i < 2; %i++)
	{
		for (%j = 0; %j < %extWC; %j++)
		{
			%ext = getWord(%extList, %j);
			$AAFTC_pattern[$AAFTC_patternCount] = %dir[%i] @ "/*." @ %ext;
			$AAFTC_patternCount++;
		}
	}
	$AAFTC_i = -1;
	$AAFTC_countedFiles = 0;
	$AAFTC_file = "";
	$AAFTC_fileCount = 0;
	Canvas.pushDialog(CacheProgressGui);
	addAllFilesToCache_Tick();
}

function onSqliteError(%errorCode)
{
	MessageBoxOK("Sqlite Error " @ %errorCode, "Couldn't write to cache database.  <br><br>This is probably because Blockland does not have permission to write to the Blockland folder.  This can be caused by running the game from a read-only directory or CD-ROM or from windows permissions settings or from 3rd party security software.  <br><br>The the game will almost certainly not work.");
}

