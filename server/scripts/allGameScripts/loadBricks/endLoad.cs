function ServerLoadSaveFile_End ()
{
	if ( $GameModeArg !$= ""  &&  $LoadingBricks_Client == $LoadingBricks_BrickGroup )
	{
		%count = ClientGroup.getCount();

		for ( %i = 0;  %i < %count;  %i++ )
		{
			%client = ClientGroup.getObject (%i);
			%client.hereAtInitialLoad = 1;
		}

		GameModeInitialResetCheck();
	}

	$LoadingBricks_Client = "";
	$LoadingBricks_ColorMethod = "";


	if ( $Server::LAN == 0  &&  doesAllowConnections() )
	{
		startRaytracer();
	}

	%time = getSimTime() - $LoadingBricks_StartTime;

	if ( !$LoadingBricks_Silent )
	{
		MessageAll ( 'MsgProcessComplete', $Load_brickCount - $Load_failureCount  @ " / " @  $Load_brickCount  @ 
			" bricks created in " @  getTimeString ( mFloor(%time / 1000 * 100 ) / 100 ) );
	}

	deleteVariables ("$Load_*");

	$LoadingBricks_Silent = 0;


	if ( isEventPending($LoadSaveFile_Tick_Schedule) )
	{
		cancel ($LoadSaveFile_Tick_Schedule);
	}

	if ( isEventPending($LoadingBricks_TimeoutSchedule) )
	{
		cancel ($LoadingBricks_TimeoutSchedule);
	}

	$Server_LoadFileObj.delete();
	$Server_LoadFileObj = 0;
}


function serverCmdCancelSaveFileUpload ( %client )
{
	if ( !%client.isAdmin )
	{
		return;
	}


	if ( $LoadingBricks_Client == %client )
	{
		MessageAll ('', "Save file upload canceled.");

		$LoadingBricks_Client = "";
		$LoadingBricks_ColorMethod = "";


		if ( $Server::LAN == 0  &&  doesAllowConnections() )
		{
			startRaytracer();
		}

		if ( isObject($LoadBrick_FileObj) )
		{
			$LoadBrick_FileObj.close();
			$LoadBrick_FileObj.delete();
		}

		if ( isEventPending($LoadSaveFile_Tick_Schedule) )
		{
			cancel ($LoadSaveFile_Tick_Schedule);
		}

		$LoadSaveFile_Tick_Schedule = 0;

		if ( isEventPending($LoadingBricks_TimeoutSchedule) )
		{
			cancel ($LoadingBricks_TimeoutSchedule);
		}

		$LoadingBricks_TimeoutSchedule = 0;
	}
}

function serverCmdEndSaveFileUpload ( %client )
{
	if ( !%client.isAdmin )
	{
		return;
	}

	if ( $LoadingBricks_Client == %client )
	{
		if ( $LoadingBricks_Ownership == 2 )
		{
			$LoadingBricks_BrickGroup = "BrickGroup_888888";
		}
		else
		{
			$LoadingBricks_BrickGroup = $LoadingBricks_Client.brickGroup;
		}


		%time = getSimTime() - $LoadingBricks_StartTime;

		echo ("Save file uploaded in " @  getTimeString ( mFloor(%time / 1000 * 100) / 100 )  @ 
			" (" @  $LoadBrick_LineCount  @ " lines).  Processing...");

		MessageAll ('MsgUploadEnd', "Save file uploaded in " @  getTimeString ( mFloor(%time / 1000 * 100) / 100 ) @ 
			" (" @  $LoadBrick_LineCount  @ " lines).  Processing...");


		$LoadingBricks_StartTime = getSimTime();

		$LoadBrick_FileObj.close();
		$LoadBrick_FileObj.delete();


		if ( isEventPending($LoadingBricks_TimeoutSchedule) )
		{
			cancel ($LoadingBricks_TimeoutSchedule);
		}

		ServerLoadSaveFile_Start();
	}
	else
	{
		messageClient (%client, '', "Error: serverCmdEndSaveFileUpload() - you are not currently loading bricks.");
	}
}
