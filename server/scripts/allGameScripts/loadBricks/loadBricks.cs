exec ("./startLoad.cs");
exec ("./endLoad.cs");
exec ("./ServerLoadSaveFile_Tick.cs");
exec ("./colors.cs");


function calcSaveOffset ()
{
	%offset["Bedroom"] = "50 -80 286.4";
	%offset["Kitchen"] = "-380 0 119.2";
	%offset["Slate"]   = "0 0 0";

	$LoadingBricks_PositionOffset = VectorSub ( "0 0 0",  %offset[$LoadingBricks_DirName] );
	$LoadingBricks_PositionOffset = VectorAdd ($LoadingBricks_PositionOffset, $LoadOffset);
}

function serverCmdSetSaveUploadDirName ( %client, %dirName, %ownership )
{
	if ( !%client.isAdmin )
	{
		return;
	}

	if ( $LoadingBricks_Client != %client )
	{
		return;
	}


	$LoadingBricks_DirName = %dirName;
	$LoadingBricks_Ownership = %ownership;

	if ( $LoadingBricks_Ownership $= "" )
	{
		$LoadingBricks_Ownership = 1;
	}

	calcSaveOffset();
}

function serverCmdUploadSaveFileLine ( %client, %line )
{
	if ( !%client.isAdmin )
	{
		return;
	}

	if ( %client != $LoadingBricks_Client )
	{
		return;
	}

	if ( isEventPending($LoadingBricks_TimeoutSchedule) )
	{
		cancel ($LoadingBricks_TimeoutSchedule);
		$LoadingBricks_TimeoutSchedule = schedule (30 * 1000, 0, serverLoadBricks_Timeout);
	}

	$LoadBrick_LineCount++;
	$LoadBrick_FileObj.writeLine (%line);
}

function serverLoadBricks_Timeout ()
{
	if ( isObject($LoadingBricks_Client) )
	{
		%name = $LoadingBricks_Client.getPlayerName();
	}
	else
	{
		%name = $LoadingBricks_Name;
	}

	MessageAll ('', %name  @ "\'s save file upload timed out.");
	
	$LoadingBricks_Client = "";
	$LoadingBricks_Name = "";
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
}


function fxDTSBrick::onLoadPlant ( %obj )
{
	%obj.getDataBlock().onLoadPlant(%obj);
}

function fxDTSBrickData::onLoadPlant ( %this, %obj )
{
	// Your code here
}