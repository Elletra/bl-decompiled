function fxDTSBrick::disappear ( %obj, %time )
{
	%data = %obj.getDataBlock();
	%data.disappear (%obj, %time);
}

function fxDTSBrickData::disappear ( %data, %obj, %time )
{
	if ( isEventPending(%obj.reappearEvent) )
	{
		cancel (%obj.reappearEvent);
	}

	if ( %time > 0 )
	{
		%obj.setRendering (0);
		%obj.setRayCasting (0);
		%obj.setColliding (0);

		%obj.reappearEvent = %obj.schedule (%time * 1000, reappear);
	}
	else if ( %time == 0 )
	{
		%obj.setRendering (1);
		%obj.setRayCasting (1);
		%obj.setColliding (1);

		if ( %obj.isFakeDead() )
		{
			transmitBrickExplosion (%obj.getPosition(), 0, 1, 1, %obj);
		}
	}
	else if ( %time < 0 )
	{
		%obj.setRendering (0);
		%obj.setRayCasting (0);
		%obj.setColliding (0);
	}
}

function fxDTSBrick::reappear ( %obj )
{
	%data = %obj.getDataBlock();
	%data.reappear (%obj);
}

function fxDTSBrickData::reappear ( %data, %obj )
{
	if ( isEventPending(%obj.reappearEvent) )
	{
		cancel (%obj.reappearEvent);
	}

	%obj.setRendering (1);
	%obj.setRayCasting (1);
	%obj.setColliding (1);
}

function fxDTSBrick::onDisappear ( %obj )
{
	if ( isObject(%obj.emitter) )
	{
		if ( isObject(%obj.emitter.emitter) )
		{
			%obj.emitter.oldEmitterDB = %obj.emitter.emitter;
		}

		%obj.emitter.setEmitterDataBlock (0);
	}
}

function fxDTSBrick::onReappear ( %obj )
{
	if ( isObject(%obj.emitter) )
	{
		if ( isObject(%obj.emitter.oldEmitterDB) )
		{
			%obj.emitter.setEmitterDataBlock (%obj.emitter.oldEmitterDB);
		}
	}
}


// =================
//  Register Events
// =================

registerOutputEvent ("fxDTSBrick", "disappear", "int -1 300 5");
