addMessageCallback('MsgClientJoin', handleClientJoin);
addMessageCallback('MsgClientDrop', handleClientDrop);
addMessageCallback('MsgClientScoreChanged', handleClientScoreChanged);
function handleClientJoin(%__unused, %__unused, %clientName, %clientId, %__unused, %score, %isAI, %isAdmin, %isSuperAdmin)
{
	PlayerListGui.update(%clientId, detag(%clientName), %isSuperAdmin, %isAdmin, %isAI, %score);
	%name = StripMLControlChars(detag(%clientName));
	if (lstAdminPlayerList.getRowNumById(%clientId) == -1)
	{
		lstAdminPlayerList.addRow(%clientId, %name);
	}
	else
	{
		lstAdminPlayerList.setRowById(%clientId, %name);
	}
}

function handleClientDrop(%__unused, %__unused, %clientName, %clientId)
{
	PlayerListGui.remove(%clientId);
	lstAdminPlayerList.removeRowById(%clientId);
}

function handleClientScoreChanged(%__unused, %__unused, %score, %clientId)
{
	PlayerListGui.updateScore(%clientId, %score);
}

addMessageCallback('InitTeams', handleInitTeams);
addMessageCallback('AddTeam', handleAddTeam);
addMessageCallback('RemoveTeam', handleRemoveTeam);
addMessageCallback('SetTeamName', handleSetTeamName);
function handleInitTeams(%__unused, %__unused)
{
	InitClientTeamManager();
}

function handleAddTeam(%__unused, %__unused, %teamID, %teamName)
{
	ClientTeamManager.addTeam(%teamID, %teamName);
}

function handleRemoveTeam(%__unused, %__unused, %teamID)
{
	ClientTeamManager.removeTeam(%teamID);
}

function handleSetTeamName(%__unused, %__unused, %teamID, %teamName)
{
	ClientTeamManager.setTeamName(%teamID, %teamName);
}

addMessageCallback('AddClientToTeam', handleAddClientToTeam);
addMessageCallback('RemoveClientFromTeam', handleRemoveClientFromTeam);
addMessageCallback('SetTeamCaptain', handleSetTeamCaptain);
function handleAddClientToTeam(%__unused, %__unused, %clientId, %clientName, %teamID)
{
	%teamObj = ClientTeamManager.findTeamByID(%teamID);
	if (%teamObj == 0)
	{
		error("ERROR: handleAddClientToTeam - Team ID " @ %teamID @ " not found in manager");
		return 0;
	}
	%teamObj.addMember(%clientId, %clientName);
}

function handleRemoveClientFromTeam(%__unused, %__unused, %clientId, %teamID)
{
	%teamObj = ClientTeamManager.findTeamByID(%teamID);
	if (%teamObj == 0)
	{
		error("ERROR: handleRemoveClientFromTeam - Team ID " @ %teamID @ " not found in manager");
		return 0;
	}
	%teamObj.removeMember(%clientId);
}

function handleSetTeamCaptain(%__unused, %__unused, %clientId, %teamID)
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

