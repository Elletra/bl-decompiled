// -----------------------------------------------------------------------------
//  Torque Game Engine
//  Copyright (C) GarageGames.com, Inc.
// -----------------------------------------------------------------------------

// -----------------------------------------------------------------------------
//  Mission start / end events sent from the server
// -----------------------------------------------------------------------------


// The client receives a mission start right before being dropped into the game.
function clientCmdMissionStart ( %seq )
{
	ClearPhysicsCache ();
}

// Received when the current mission is ended.
function clientCmdMissionEnd ( %seq )
{
	alxStopAll ();

	// Disable mission lighting if it's going, this is here in case the mission ends
	// while we are in the process of loading it.
	$lightingMission = false;
	$sceneLighting::terminateLighting = true;
}
