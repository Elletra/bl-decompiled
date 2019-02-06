function serverDirectSaveFileLoad ( %filename, %colorMethod, %dirName, %ownership, %silent )
{
	echo ("Direct load " @  %filename  @ ", " @  %colorMethod  @ ", " @  %dirName  @ ", " @  %ownership  @ ", " @  %silent);

	if ( !isFile(%filename) )
	{
		MessageAll ('', "ERROR: File " @  %filename  @ " not found.  If you are seeing this, you broke something.");
		return;
	}


	$LoadingBricks_ColorMethod = %colorMethod;
	$LoadingBricks_DirName = %dirName;
	$LoadingBricks_Ownership = %ownership;

	if ( $LoadingBricks_Ownership $= "" )
	{
		$LoadingBricks_Ownership = 1;
	}

	calcSaveOffset();


	if ( $LoadingBricks_Client  &&  $LoadingBricks_Client != 1 )
	{
		MessageAll ('', "Load interrupted by host.");

		if ( isObject($LoadBrick_FileObj) )
		{
			$LoadBrick_FileObj.close();
			$LoadBrick_FileObj.delete();
		}
	}

	$LoadingBricks_Client = findLocalClient();

	if ( $LoadingBricks_Client )
	{
		if ( $LoadingBricks_Ownership == 2 )
		{
			$LoadingBricks_BrickGroup = "BrickGroup_888888";
		}
		else
		{
			$LoadingBricks_BrickGroup = $LoadingBricks_Client.brickGroup;
		}
	}
	else
	{
		if ( $Server::LAN )
		{
			if ( $LoadingBricks_Ownership == 2 )
			{
				$LoadingBricks_BrickGroup = "BrickGroup_888888";
			}
			else
			{
				$LoadingBricks_BrickGroup = "BrickGroup_999999";
			}
		}
		else
		{
			if ( $LoadingBricks_Ownership == 2 )
			{
				$LoadingBricks_BrickGroup = "BrickGroup_888888";
			}
			else
			{
				$LoadingBricks_BrickGroup = "BrickGroup_" @  getNumKeyID();
			}
		}

		$LoadingBricks_BrickGroup.isAdmin = 1;
		$LoadingBricks_BrickGroup.brickGroup = $LoadingBricks_BrickGroup;
		$LoadingBricks_Client = $LoadingBricks_BrickGroup;
	}


	$LoadingBricks_Silent = %silent;

	if ( !$LoadingBricks_Silent )
	{
		MessageAll ('MsgUploadStart', "Loading bricks. Please wait.");
	}

	$LoadingBricks_StartTime = getSimTime();
	ServerLoadSaveFile_Start (%filename);
}

function ServerLoadSaveFile_Start ( %filename )
{
	echo ("LOADING BRICKS: " @  %filename  @ " (ColorMethod " @  $LoadingBricks_ColorMethod  @ ")");

	if ( $Game::MissionCleaningUp )
	{
		echo ("LOADING CANCELED: Mission cleanup in progress");
		return;
	}

	$Server_LoadFileObj = new FileObject();

	if ( isFile(%filename) )
	{
		$Server_LoadFileObj.openForRead (%filename);
	}
	else
	{
		$Server_LoadFileObj.openForRead ("base/server/temp/temp.bls");
	}

	if ( $UINameTableCreated == 0 )
	{
		createUINameTable();
	}

	$LastLoadedBrick = 0;

	$Load_failureCount = 0;
	$Load_brickCount = 0;


	$Server_LoadFileObj.readLine();
	%lineCount = $Server_LoadFileObj.readLine();

	for ( %i = 0;  %i < %lineCount;  %i++ )
	{
		$Server_LoadFileObj.readLine();
	}

	if ( isEventPending($LoadSaveFile_Tick_Schedule) )
	{
		cancel ($LoadSaveFile_Tick_Schedule);
	}

	ServerLoadSaveFile_ProcessColorData();
	ServerLoadSaveFile_Tick();

	stopRaytracer();
}


function serverCmdInitUploadHandshake ( %client )
{
	if ( !%client.isAdmin )
	{
		return;
	}

	if ( $Game::MissionCleaningUp )
	{
		messageClient (%client, '', "Can\'t load during mission clean up");
		commandToClient (%client, 'LoadBricksConfirmHandshake', 0);

		return;
	}

	if ( !isObject($LoadingBricks_Client) )
	{
		if ( isEventPending($LoadingBricks_TimeoutSchedule) )
		{
			cancel ($LoadingBricks_TimeoutSchedule);
		}

		$LoadingBricks_StartTime = getSimTime();
		$LoadingBricks_Client = %client;
		$LoadingBricks_Name = %client.getPlayerName();
		$LoadingBricks_DirName = "";
		$LoadingBricks_PositionOffset = "0 0 0";
		$LoadingBricks_FailureCount = 0;
		$LoadingBricks_BrickCount = 0;

		$LoadBrick_LineCount = 0;

		if ( isObject($LoadingBricks_Client) )
		{
			%name = $LoadingBricks_Client.getPlayerName();
		}
		else
		{
			%name = $LoadingBricks_Name;
		}

		echo (%name  @ " is uploading a save file.");
		MessageAll ('MsgUploadStart', '\c3%1\c0 is uploading a save file.', %name);

		commandToClient (%client, 'LoadBricksConfirmHandshake', 1, $Pref::Server::AllowColorLoading);


		if ( isObject($LoadBrick_FileObj) )
		{
			$LoadBrick_FileObj.delete();
		}

		$LoadBrick_FileObj = new FileObject();
		$LoadBrick_FileObj.openForWrite ("base/server/temp/temp.bls");

		$LoadingBricks_TimeoutSchedule = schedule (30 * 1000, 0, serverLoadBricks_Timeout);
	}
	else
	{
		%name = $LoadingBricks_Client.getPlayerName();

		messageClient (%client, '', %name  @ "is loading bricks right now. Try again later.");
		commandToClient (%client, 'LoadBricksConfirmHandshake', 0);
	}
}

function serverCmdReloadBricks ( %client )
{
	if ( !%client.isAdmin )
	{
		return;
	}

	if ( $Game::MissionCleaningUp )
	{
		messageClient (%client, '', 'Can\'t reload bricks during mission clean up');
		return;
	}

	if ( $LoadingBricks_Client  &&  $LoadingBricks_Client != 1 )
	{
		messageClient (%client, '', 'There is another load in progress.');
		return;
	}

	$LoadingBricks_Client = %client;
	$LoadingBricks_BrickGroup = $LoadingBricks_Client.brickGroup;


	if ( $LoadingBricks_Ownership $= "" )
	{
		$LoadingBricks_Ownership = 1;
	}

	echo (%client.getPlayerName()  @ " Re-Loaded Bricks.");
	MessageAll ( 'MsgUploadStart',  '\c3%1\c0 Re-Loading bricks. Please wait.',  %client.getPlayerName() );

	if ( $LoadingBricks_ColorMethod $= "" )
	{
		$LoadingBricks_ColorMethod = 3;
	}

	$LoadingBricks_StartTime = getSimTime();
	ServerLoadSaveFile_Start();
}
