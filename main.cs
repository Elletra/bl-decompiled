// -----------------------------------------------------------------------------
//  Torque Game Engine
//  Copyright (C) GarageGames.com, Inc.
// -----------------------------------------------------------------------------

// -----------------------------------------------------------------------------
//  Load up defaults console values.
// -----------------------------------------------------------------------------


function initCommon ()
{
	// All mods need the random seed set.
	setRandomSeed ();

	// Very basic functions used by everyone.
	exec ("./client/canvas.cs");
	exec ("./client/audio.cs");
}

function initBaseClient ()
{
	// Base client functionality
	exec ("./client/message.cs");
	exec ("./client/mission.cs");
	exec ("./client/missionDownload.cs");
	exec ("./client/actionMap.cs");
}

function initBaseServer ()
{
	if ( true )
	{
		// Base server functionality.
		exec ("./server/mainServer.cs");
	}
	else
	{
		// This is presumably the development folder structure.
		exec ("./server/webCom.cs");
		exec ("./server/authQuery.cs");
		exec ("./server/audio.cs");
		exec ("./server/server.cs");
		exec ("./server/message.cs");
		exec ("./server/commands.cs");
		exec ("./server/missionInfo.cs");
		exec ("./server/missionLoad.cs");
		exec ("./server/missionDownload.cs");
		exec ("./server/clientConnection.cs");
		exec ("./server/game.cs");
	}
}

function onDatablockLimitExceeded ()
{
	$datablockExceededCount++;
}

function onDatablocksDeleted ()
{
	$datablockExceededCount = 0;
}
