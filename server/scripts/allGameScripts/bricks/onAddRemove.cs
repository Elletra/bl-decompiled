function fxDTSBrick::onAdd ( %obj )
{
	%obj.getDataBlock().onAdd (%obj);
}

function fxDTSBrickData::onAdd ( %this, %obj )
{
	// Your code here
}

function fxDTSBrick::onRemove ( %obj )
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


	%obj.getDataBlock().onRemove(%obj);


	if ( %obj.getName() !$= "" )
	{
		%obj.clearNTObjectName();
	}
}

function fxDTSBrickData::onRemove ( %this, %obj )
{
	// Your code here
}
