function serverCmdMissionStartPhase1Ack ( %client, %seq )
{
	if ( %seq != $missionSequence  ||  !$missionRunning )
	{
		return;
	}

	if ( %client.currentPhase != 0 )
	{
		return;
	}

	%client.currentPhase = 1;

	%manifestHash = snapshotGameAssets();
	%client.sendManifest (%manifestHash);
}

function serverCmdBlobDownloadFinished ( %client )
{
	%client.transmitDataBlocks ($missionSequence);
}

function GameConnection::onDataBlocksDone ( %this, %missionSequence )
{
	if ( %missionSequence != $missionSequence )
	{
		return;
	}

	if ( %this.currentPhase != 1 )
	{
		return;
	}

	%this.currentPhase = 1.5;
	commandToClient (%this, 'MissionStartPhase2', $missionSequence, $Server::MissionFile);
}
