$MissionLoadPause = 2000;


function GameConnection::loadMission ( %this )
{
	%this.currentPhase = 0;

	if ( %this.isAIControlled() )
	{
		%this.onClientEnterGame();
	}
	else
	{
		commandToClient (%this, 'MissionStartPhase1', $missionSequence);
	}
}

function GameConnection::startMission ( %this )
{
	commandToClient (%this, 'MissionStart', $missionSequence);
}

function GameConnection::endMission ( %this )
{
	commandToClient (%this, 'MissionEnd', $missionSequence);
}


exec ("./autoAdminCheck.cs");
exec ("./startLoad.cs");

exec ("./phase1.cs");
exec ("./phase2.cs");
exec ("./phase3.cs");
