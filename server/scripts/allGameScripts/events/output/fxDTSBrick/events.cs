exec ("./fireRelay.cs");
exec ("./spawn.cs");
exec ("./radiusImpulse.cs");
exec ("./prints.cs");

exec ("./fakeKillBrick.cs");
exec ("./disappearReappear.cs");

exec ("./setLight.cs");
exec ("./setEmitter.cs");
exec ("./setItem.cs");
exec ("./setMusic.cs");
exec ("./setVehicle.cs");
exec ("./respawnVehicle.cs");


function fxDTSBrick::respawn ( %obj )
{
	if ( isEventPending(%obj.reappearEvent) )
	{
		%obj.reappear();
	}

	transmitBrickExplosion (%obj.getPosition(), 0, 1, 1, %obj);
}

function fxDTSBrick::playSound ( %obj, %soundData )
{
	if ( %obj.getFakeDeadTime() > 120 )
	{
		return;
	}

	if ( !isObject(%soundData) )
	{
		return;
	}

	if ( !isObject ( %soundData.getDescription() ) )
	{
		return;
	}

	if ( %soundData.getDescription().isLooping == 1 )
	{
		return;
	}

	if ( !%soundData.getDescription().is3D )
	{
		return;
	}

	ServerPlay3D ( %soundData, %obj.getPosition() );
}


// =================
//  Register Events
// =================

registerOutputEvent ("fxDTSBrick", "respawn", "");
registerOutputEvent ("fxDTSBrick", "playSound", "dataBlock Sound");


// Engine-defined methods

registerOutputEvent ("fxDTSBrick", "setColor", "paintColor 0", 0);
registerOutputEvent ("fxDTSBrick", "setColorFX", "list None 0 Pearl 1 Chrome 2 Glow 3 Blink 4 Swirl 5 Rainbow 6", 0);
registerOutputEvent ("fxDTSBrick", "setShapeFX", "list None 0 Undulo 1 Water 2", 0);

registerOutputEvent ("fxDTSBrick", "setColliding", "bool", 0);
registerOutputEvent ("fxDTSBrick", "setRendering", "bool", 0);
registerOutputEvent ("fxDTSBrick", "setRayCasting", "bool", 0);
