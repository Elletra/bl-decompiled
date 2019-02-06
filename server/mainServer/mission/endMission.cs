function endMission ()
{
	if ( !isObject(MissionGroup) )
	{
		return;
	}

	echo ("*** ENDING MISSION");

	onMissionEnded();

	%count = ClientGroup.getCount();

	for ( %clientIndex = 0;  %clientIndex < %count;  %clientIndex++ )
	{
		%cl = ClientGroup.getObject(%clientIndex);

		%cl.endMission();
		%cl.resetGhosting();
		%cl.clearPaths();

		%cl.hasSpawnedOnce = 0;
	}

	if ( $Server::Dedicated )
	{
		setParticleDisconnectMode (1);
	}

	MissionGroup.deleteAll();
	MissionGroup.delete();

	MissionCleanup.deleteAll();
	MissionCleanup.delete();

	$ServerGroup.deleteAll();
	$ServerGroup.delete();

	$ServerGroup = new SimGroup (ServerGroup);

	if ( $Server::Dedicated )
	{
		setParticleDisconnectMode (0);
	}
}
