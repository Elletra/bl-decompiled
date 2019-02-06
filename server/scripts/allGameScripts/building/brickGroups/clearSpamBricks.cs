function serverCmdClearSpamBricks ( %client )
{
	if ( !%client.isAdmin )
	{
		return;
	}

	if ( getBrickCount() > 0 )
	{
		MessageAll ('MsgClearBricks', "\c3" @  %client.getPlayerName()  @ "\c0 cleared spam bricks.");
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
				if ( %brick.getDistanceFromGround() == 0  &&  %brick.isPlanted  &&  !%brick.isDead )
				{
					%brickData = %brick.getDataBlock().getId();

					if ( %brickData.category $= "Baseplates" )
					{
						if ( %brick.getNumUpBricks() == 0  &&  ( %brick.getColorID() == 0  ||  isObject(%brick.Item) ) )
						{
							%brick.killBrick();
						}
						else
						{
							if ( %brick.getNumUpBricks() == 1  &&  %brick.getColorID() == 0  &&  
								(%brickData.subCategory $= "Plain"  ||  %brickData.subCategory $= "Road") )
							{
								%upBrick = %brick.getUpBrick (0);

								if ( %upBrick.getDataBlock().getId() == %brickData  &&  %upBrick.getColorID() == 0 )
								{
									%brick.killBrick();
								}
							}
						}
					}
					else
					{
						%brick.killBrick();
					}
				}
			}
		}
	}
}
