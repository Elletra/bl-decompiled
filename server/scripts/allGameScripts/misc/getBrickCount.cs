function serverCmdRealBrickCount ( %client )
{
	if ( !%client.isAdmin )
	{
		return;
	}

	%count = MissionCleanup.getCount();
	%brickCount = 0;

	for ( %i = 0;  %i < %count;  %i++ )
	{
		%obj = MissionCleanup.getObject (%i);

		if ( %obj.getClassName() $= "fxDTSBrick" )
		{
			if ( %obj.isPlanted )
			{
				%brickCount++;
			}
		}
	}

	if ( %brickCount == 1 )
	{
		messageClient (%client,  '',  %brickCount  @ " brick");
	}
	else
	{
		messageClient (%client,  '',  %brickCount  @ " bricks");
	}
}

function serverCmdBrickCount ( %client )
{
	if ( getBrickCount() == 1 )
	{
		messageClient (%client, '', getBrickCount()  @ " brick");
	}
	else
	{
		messageClient (%client, '', getBrickCount()  @ " bricks");
	}
}
