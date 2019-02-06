function serverCmdClearFloatingBricks ( %client )
{
	if ( !%client.isAdmin )
	{
		return;
	}

	if ( getBrickCount() > 0 )
	{
		MessageAll ('MsgClearBricks', "\c3" @  %client.getPlayerName()  @ "\c0 cleared floating bricks.");
	}

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
				if ( %brick.getDistanceFromGround() == 2147483647  &&  %brick.isPlanted  &&  !%brick.isDead )
				{
					%brick.killBrick();
				}
			}
		}
	}
}
