function serverCmdMissionStartPhase2Ack ( %client, %seq )
{
	if ( %seq != $missionSequence  || !$missionRunning )
	{
		return;
	}

	if ( %client.currentPhase != 1.5 )
	{
		return;
	}

	%client.currentPhase = 2;

	%client.transmitStaticBrickData();
	%client.transmitPaths();
	%client.activateGhosting();
}

function GameConnection::clientWantsGhostAlwaysRetry ( %client )
{
	if ( $missionRunning )
	{
		%client.activateGhosting();
	}
}

function GameConnection::onGhostAlwaysFailed ( %client )
{
	// Your code here
}

function GameConnection::onGhostAlwaysObjectsReceived ( %client )
{
	commandToClient (%client, 'MissionStartPhase3', $missionSequence, $Server::MissionFile, $Server::LAN);
}
