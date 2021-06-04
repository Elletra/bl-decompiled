// -----------------------------------------------------------------------------
//  Torque Game Engine
//  Copyright (C) GarageGames.com, Inc.
// -----------------------------------------------------------------------------

// -----------------------------------------------------------------------------
//  Mission Loading
//  Server download handshaking.  This produces a number of manifest, blob, and
//  onPhaseX calls so the game scripts can update the game's GUI.
//
//  Loading Phases:
//  Phase 0: Download File Manifest
//  Phase 1: Download Datablocks
//  Phase 2: Download Ghost Objects
//  Phase 3: Scene Lighting
// -----------------------------------------------------------------------------


// -----------------------------------------------------------------------------
//  Phase 0
// -----------------------------------------------------------------------------

function onManifestHashReceived ()
{
	$manifestPending = true;

	LoadingProgressTxt.setValue ("DOWNLOADING FILE MANIFEST");
	LoadingProgress.setValue (0);
	LoadingSecondaryProgress.setValue (0);
}

function onManifestRecieved ()
{
	$manifestPending = false;
	$totalPendingBlobs = 0;
}

function setTotalPendingBlobs ( %tpb )
{
	$totalPendingBlobs = %tpb;
}

function onBlobCacheCheckFinished ()
{
	if ( $totalPendingBlobs == 1 )
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

function updateBlobsRemaining ( %blobsRemaining )
{
	if ( $manifestPending && %blobsRemaining == 1 )
	{
		LoadingProgressTxt.setValue ("DOWNLOADING FILE MANIFEST");
		LoadingProgress.setValue (0);
		LoadingSecondaryProgress.setValue (0);

		return;
	}

	if ( %blobsRemaining > $totalPendingBlobs )
	{
		$totalPendingBlobs = %blobsRemaining;
	}

	if ( $totalPendingBlobs > 0 )
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

function setDownloadSize ( %size )
{
	$currDownloadSize = %size;
}

function updateDownloadProgress ( %val )
{
	%percent = %val / $currDownloadSize;
	LoadingSecondaryProgress.setValue (%percent);
}

// -----------------------------------------------------------------------------
//  Phase 1
// -----------------------------------------------------------------------------

function clientCmdMissionStartPhase1 ( %seq )
{
	echo ("*** New Mission");
	echo ("*** Phase 1: Download Datablocks & Targets");

	onMissionDownloadPhase1 ();

	commandToServer ('MissionStartPhase1Ack', %seq);
}

function onDataBlockObjectReceived ( %index, %total )
{
	onPhase1Progress (%index / %total);
}

// -----------------------------------------------------------------------------
//  Phase 2
// -----------------------------------------------------------------------------

function clientCmdMissionStartPhase2 ( %seq )
{
	onPhase1Complete ();

	echo ("*** Phase 2: Download Ghost Objects");

	purgeResources ();

	onMissionDownloadPhase2 ();
	commandToServer ('MissionStartPhase2Ack', %seq);
}

function onGhostAlwaysStarted ( %ghostCount )
{
	$ghostCount = %ghostCount;
	$ghostsRecvd = 0;
}

function onGhostAlwaysObjectReceived ()
{
	$ghostsRecvd++;
	onPhase2Progress ($ghostsRecvd / $ghostCount);
}

// -----------------------------------------------------------------------------
//  Phase 3
// -----------------------------------------------------------------------------

function clientCmdMissionStartPhase3 ( %seq )
{
	onPhase2Complete ();

	echo ("*** Phase 3: Mission Lighting");

	$MSeq = %seq;
	sceneLightingComplete ();
}

function updateLightingProgress ()
{
	onPhase3Progress ($SceneLighting::lightingProgress);

	if ( $lightingMission )
	{
		$lightingProgressThread = schedule (1, 0, "updateLightingProgress");
	}
}

function sceneLightingComplete ()
{
	echo ("Mission lighting done");

	onPhase3Complete ();

	// This is also the end of the mission load cycle.
	onMissionDownloadComplete ();

	commandToServer ('MissionStartPhase3Ack', $MSeq);
	commandToServer ('RequestNamedTargets');
}
