function onManifestHashReceived ()
{
	$manifestPending = 1;
	LoadingProgressTxt.setValue ("DOWNLOADING FILE MANIFEST");
	LoadingProgress.setValue (0);
	LoadingSecondaryProgress.setValue (0);
}

function onManifestRecieved ()
{
	$manifestPending = 0;
	$totalPendingBlobs = 0;
}

function setTotalPendingBlobs (%tpb)
{
	$totalPendingBlobs = %tpb;
}

function onBlobCacheCheckFinished ()
{
	if ($totalPendingBlobs == 1)
	{
		LoadingProgressTxt.setValue ("CHECKING CDN FOR 1 FILE");
	}
	else 
	{
		LoadingProgressTxt.setValue ("CHECKING CDN FOR " @ $totalPendingBlobs @ " FILES");
	}
	LoadingProgress.setValue (0);
	LoadingSecondaryProgress.setValue (0);
}

function updateBlobsRemaining (%blobsRemaining)
{
	if ($manifestPending && %blobsRemaining == 1)
	{
		LoadingProgressTxt.setValue ("DOWNLOADING FILE MANIFEST");
		LoadingProgress.setValue (0);
		LoadingSecondaryProgress.setValue (0);
		return;
	}
	if (%blobsRemaining > $totalPendingBlobs)
	{
		$totalPendingBlobs = %blobsRemaining;
	}
	if ($totalPendingBlobs > 0)
	{
		LoadingProgressTxt.setValue ("DOWNLOADING FILE " @ ($totalPendingBlobs - %blobsRemaining) + 1 @ " OF " @ $totalPendingBlobs);
		LoadingProgress.setValue (($totalPendingBlobs - %blobsRemaining) / $totalPendingBlobs);
		LoadingSecondaryProgress.setValue (0);
		Canvas.repaint ();
	}
}

function onBlobDownloadFinished ()
{
	LoadingProgressTxt.setValue ("LOADING DATABLOCKS");
	LoadingSecondaryProgress.setValue (0);
	LoadingProgress.setValue (0);
	commandToServer ('BlobDownloadFinished');
}

function setDownloadSize (%size)
{
	$currDownloadSize = %size;
}

function updateDownloadProgress (%val)
{
	%percent = %val / $currDownloadSize;
	LoadingSecondaryProgress.setValue (%percent);
}

function clientCmdMissionStartPhase1 (%seq)
{
	echo ("*** New Mission");
	echo ("*** Phase 1: Download Datablocks & Targets");
	onMissionDownloadPhase1 ();
	commandToServer ('MissionStartPhase1Ack', %seq);
}

function onDataBlockObjectReceived (%index, %total)
{
	onPhase1Progress (%index / %total);
}

function clientCmdMissionStartPhase2 (%seq)
{
	onPhase1Complete ();
	echo ("*** Phase 2: Download Ghost Objects");
	purgeResources ();
	onMissionDownloadPhase2 ();
	commandToServer ('MissionStartPhase2Ack', %seq);
}

function onGhostAlwaysStarted (%ghostCount)
{
	$ghostCount = %ghostCount;
	$ghostsRecvd = 0;
}

function onGhostAlwaysObjectReceived ()
{
	$ghostsRecvd += 1;
	onPhase2Progress ($ghostsRecvd / $ghostCount);
}

function clientCmdMissionStartPhase3 (%seq)
{
	onPhase2Complete ();
	echo ("*** Phase 3: Mission Lighting");
	$MSeq = %seq;
	sceneLightingComplete ();
}

function updateLightingProgress ()
{
	onPhase3Progress ($SceneLighting::lightingProgress);
	if ($lightingMission)
	{
		$lightingProgressThread = schedule (1, 0, "updateLightingProgress");
	}
}

function sceneLightingComplete ()
{
	echo ("Mission lighting done");
	onPhase3Complete ();
	onMissionDownloadComplete ();
	commandToServer ('MissionStartPhase3Ack', $MSeq);
	commandToServer ('RequestNamedTargets');
}

