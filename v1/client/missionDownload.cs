function clientCmdMissionStartPhase1(%seq, %missionName, %musicTrack)
{
	echo("*** New Mission: " @ %missionName);
	echo("*** Phase 1: Download Datablocks & Targets");
	onMissionDownloadPhase1(%missionName, %musicTrack);
	commandToServer('MissionStartPhase1Ack', %seq);
}

function onDataBlockObjectReceived(%index, %total)
{
	onPhase1Progress(%index / %total);
}

function clientCmdMissionStartPhase2(%seq, %missionName)
{
	onPhase1Complete();
	echo("*** Phase 2: Download Ghost Objects");
	purgeResources();
	onMissionDownloadPhase2(%missionName);
	commandToServer('MissionStartPhase2Ack', %seq);
}

function onGhostAlwaysStarted(%ghostCount)
{
	$ghostCount = %ghostCount;
	$ghostsRecvd = 0;
}

function onGhostAlwaysObjectReceived()
{
	$ghostsRecvd++;
	onPhase2Progress($ghostsRecvd / $ghostCount);
}

function clientCmdMissionStartPhase3(%seq, %missionName)
{
	onPhase2Complete();
	StartClientReplication();
	StartFoliageReplication();
	StartGrassReplication();
	echo("*** Phase 3: Mission Lighting");
	$MSeq = %seq;
	$Client::MissionFile = %missionName;
	if (lightScene("sceneLightingComplete", ""))
	{
		error("Lighting mission....");
		schedule(1, 0, "updateLightingProgress");
		onMissionDownloadPhase3(%missionName);
		$lightingMission = 1;
	}
}

function updateLightingProgress()
{
	onPhase3Progress($SceneLighting::lightingProgress);
	if ($lightingMission)
	{
		$lightingProgressThread = schedule(1, 0, "updateLightingProgress");
	}
}

function sceneLightingComplete()
{
	echo("Mission lighting done");
	onPhase3Complete();
	onMissionDownloadComplete();
	commandToServer('MissionStartPhase3Ack', $MSeq);
}

