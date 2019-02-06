function fxDTSBrick::fakeKillBrick ( %obj, %vector, %time, %client )
{
	if ( %obj.eventFloodCheck(1) )
	{
		return;
	}

	if ( isEventPending(%obj.reappearEvent) )
	{
		%obj.reappear();
	}

	%pos = %obj.getPosition();
	%explosionForce = VectorLen (%vector) * 2;
	%pos = VectorSub ( %pos, VectorNormalize(%vector) );

	%time = mClamp (%time, 0, 300);

	transmitBrickExplosion (%pos, %explosionForce, 1, %time * 1000, %obj);

	%obj.destroyingClient = %client;
	%obj.destroyingPlayer = %client.Player;
}

function fxDTSBrick::onFakeDeath ( %obj )
{
	%data = %obj.getDataBlock();
	%data.onFakeDeath (%obj);
}

function fxDTSBrickData::onFakeDeath ( %data, %obj )
{
	if ( isObject(%obj.emitter) )
	{
		if ( isObject(%obj.emitter.emitter) )
		{
			%obj.emitter.oldEmitterDB = %obj.emitter.emitter;
		}

		%obj.emitter.setEmitterDataBlock (0);
	}

	if ( isObject($CurrBrickKiller) )
	{
		if ( isObject($CurrBrickKiller.miniGame) )
		{
			$CurrBrickKiller.incScore ($CurrBrickKiller.miniGame.Points_BreakBrick);
		}
	}
}

function fxDTSBrick::onClearFakeDeath ( %obj )
{
	%data = %obj.getDataBlock();
	%data.onClearFakeDeath (%obj);
}

function fxDTSBrickData::onClearFakeDeath ( %data, %obj )
{
	if ( isObject(%obj.emitter) )
	{
		if ( isObject(%obj.emitter.oldEmitterDB) )
		{
			%obj.emitter.setEmitterDataBlock (%obj.emitter.oldEmitterDB);
		}
	}

	%obj.onRespawn (%obj.destroyingClient, %obj.destroyingPlayer);
}


// =================
//  Register Events
// =================

registerOutputEvent ("fxDTSBrick", "fakeKillBrick", "vector 200" TAB "int 0 300 5");
