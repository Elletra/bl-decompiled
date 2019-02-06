function serverCmdMissionStartPhase3Ack ( %client, %seq )
{
	if ( %seq != $missionSequence  ||  !$missionRunning )
	{
		return;
	}

	if ( %client.currentPhase != 2 )
	{
		return;
	}

	%client.currentPhase = 3;

	%client.startMission();
	%client.onClientEnterGame();
}
