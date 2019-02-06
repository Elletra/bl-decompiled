function fxDTSBrick::onDeath ( %obj )
{
	if ( isObject(%obj.light) )
	{
		%obj.light.delete();
	}

	if ( isObject(%obj.emitter) )
	{
		%obj.emitter.delete();
	}

	if ( isObject(%obj.Item) )
	{
		%obj.Item.delete();
	}

	if ( isObject(%obj.AudioEmitter) )
	{
		%obj.AudioEmitter.delete();
	}

	if ( isObject(%obj.Vehicle) )
	{
		%obj.Vehicle.delete();
	}

	if ( isObject(%obj.vehicleSpawnMarker) )
	{
		%obj.vehicleSpawnMarker.delete();
	}

	if ( %obj.getName() )
	{
		%obj.setNTObjectName ("");
	}

	if ( %obj.numScheduledEvents )
	{
		%obj.cancelEvents();
	}


	%obj.getDataBlock().onDeath(%obj);


	if ( isObject($CurrBrickKiller) )
	{
		if ( isObject($CurrBrickKiller.miniGame) )
		{
			$CurrBrickKiller.incScore ($CurrBrickKiller.miniGame.Points_BreakBrick);
		}
	}
}

function fxDTSBrickData::onDeath ( %this, %obj )
{
	// Your code here
}
