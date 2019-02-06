function fxDTSBrick::eventFloodCheck ( %obj, %maxFlood )
{
	%maxFlood = mClamp (%maxFlood, 1, 10);
	%currTime = getSimTime();

	if ( atoi(%currTime) - atoi(%obj.lastEventFloodTime) < 15 )
	{
		%obj.eventFloodCount++;

		if ( %obj.eventFloodCount > %maxFlood )
		{
			return 1;
		}
	}
	else
	{
		%obj.eventFloodCount = 0;
		%obj.lastEventFloodTime = %currTime;
	}

	return 0;
}

function GameConnection::eventFloodCheck ( %obj, %maxFlood )
{
	%maxFlood = mClamp (%maxFlood, 1, 10);
	%currTime = getSimTime();

	if ( atoi(%currTime) - atoi(%obj.lastEventFloodTime) < 15 )
	{
		%obj.eventFloodCount++;

		if ( %obj.eventFloodCount > %maxFlood )
		{
			return 1;
		}
	}
	else
	{
		%obj.eventFloodCount = 0;
		%obj.lastEventFloodTime = %currTime;
	}

	return 0;
}
