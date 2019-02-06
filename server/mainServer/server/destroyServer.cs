function destroyServer ()
{
	if ( $Server::LAN )
	{
		echo ("Destroying LAN Server");
	}
	else
	{
		echo ("Destroying NET Server");
	}

	$Server::ServerType = "";

	setAllowConnections (0);
	$missionRunning = 0;

	if ( isEventPending($LoadSaveFile_Tick_Schedule) )
	{
		cancel ($LoadSaveFile_Tick_Schedule);
	}


	%count = ClientGroup.getCount();

	for ( %i = 0;  %i < %count;  %i++ )
	{
		%client = ClientGroup.getObject(%i);
		%client.delete();
	}

	endMission();
	onServerDestroyed();

	if ( isEventPending($WebCom_PostSchedule) )
	{
		cancel ($WebCom_PostSchedule);
	}

	$Server::GuidList = "";

	deleteDataBlocks();

	if ( isEventPending($LoadingBricks_HandShakeSchedule) )
	{
		cancel ($LoadingBricks_HandShakeSchedule);
	}

	$LoadingBricks_HandShakeSchedule = 0;

	if ( isEventPending($UploadSaveFile_Tick_Schedule) )
	{
		cancel ($UploadSaveFile_Tick_Schedule);
	}

	$UploadSaveFile_Tick_Schedule = 0;

	if ( isEventPending($GameModeInitialResetCheckEvent) )
	{
		cancel ($GameModeInitialResetCheckEvent);
	}

	$GameModeInitialResetCheckEvent = 0;

	deleteVariables ("$InputEvent_*");
	deleteVariables ("$OutputEvent_*");
	deleteVariables ("$uiNameTable*");
	deleteVariables ("$BSD_InvData*");
	deleteVariables ("$DamageType::*");
	deleteVariables ("$MiniGame::*");
	deleteVariables ("$EnvGui::*");
	deleteVariables ("$EnvGuiServer::*");
	deleteVariables ("$GameModeGui::*");
	deleteVariables ("$GameModeGuiServer::*");
	deleteVariables ("$printNameTable*");
	deleteVariables ("$printARNumPrints*");
	deleteVariables ("$printARStart*");
	deleteVariables ("$printAREnd*");
	deleteVariables ("$PrintCountIdx*");

	$SaveFileArg = "";

	echo ("Exporting server prefs...");

	export ("$Pref::Server::*", "config/server/prefs.cs", 0);
	export ("$Pref::Net::PacketRateToClient", "config/server/prefs.cs", true);
	export ("$Pref::Net::PacketRateToServer", "config/server/prefs.cs", true);
	export ("$Pref::Net::PacketSize", "config/server/prefs.cs", true);
	export ("$Pref::Net::LagThreshold", "config/server/prefs.cs", true);

	purgeResources();
	DeactivateServerPackages();
}

function onServerDestroyed ()
{
	destroyGame();
}
