function serverCmdClearFarAwayBricks ( %client )
{
	if ( !%client.isAdmin )
	{
		return;
	}

	%player = %client.Player;

	if ( !isObject(%player) )
	{
		return;
	}

	if ( getBrickCount() <= 0 )
	{
		return;
	}

	MessageAll ('MsgClearBricks', "\c3" @  %client.getPlayerName()  @ "\c0 cleared far away bricks.");

	%playerPos = %player.getPosition();
	%groupCount = mainBrickGroup.getCount();

	for ( %i = 0;  %i < %groupCount;  %i++ )
	{
		%group = mainBrickGroup.getObject (%i);
		%count = %group.getCount();

		for ( %j = 0;  %j < %count;  %j++ )
		{
			%brick = %group.getObject (%j);

			if ( %brick.getType() & $TypeMasks::FxBrickAlwaysObjectType )
			{
				if ( %brick.getDistanceFromGround() == 0  &&  %brick.isPlanted  &&  !%brick.isDead )
				{
					%brickPos = %brick.getPosition();
					%delta = VectorSub (%brickPos, %playerPos);
					%distance = VectorLen (%delta);

					if ( %distance >= 1000 )
					{
						%brick.killBrick();
					}
				}
			}
		}
	}
}
