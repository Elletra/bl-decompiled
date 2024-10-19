function clientCmdMissionStart(%__unused)
{
}

function clientCmdMissionEnd(%__unused)
{
	alxStopAll();
	$lightingMission = 0;
	$sceneLighting::terminateLighting = 1;
}

