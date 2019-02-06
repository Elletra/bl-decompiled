$currCheckVal = 10;


function MiniGameSO::pickSpawnPoint ( %obj, %client )
{
	if ( %obj.useSpawnBricks )
	{
		if ( %obj.useAllPlayersBricks )
		{
			if ( %obj.playersUseOwnBricks )
			{
				%brickGroup = %client.brickGroup;

				if ( %brickGroup.spawnBrickCount > 0 )
				{
					return %brickGroup.getBrickSpawnPoint();
				}
				else
				{
					return BrickGroup_888888.getBrickSpawnPoint();
				}
			}
			else
			{
				%totalSpawnPoints = 0;
				$currCheckVal++;

				for ( %i = 0;  %i < %obj.numMembers;  %i++ )
				{
					%brickGroup = %obj.member[%i].brickGroup;

					if ( %brickGroup.checkVal != $currCheckVal )
					{
						%brickGroup.checkVal = $currCheckVal;
						%totalSpawnPoints += %obj.member[%i].brickGroup.spawnBrickCount;
					}
				}

				BrickGroup_888888.checkVal = $currCheckVal;
				%totalSpawnPoints += BrickGroup_888888.spawnBrickCount;

				if ( %totalSpawnPoints <= 0 )
				{
					return pickSpawnPoint();
				}

				%rnd = getRandom();
				%finalBrickGroup = 0;
				%currPercent = 0;
				$currCheckVal++;

				for ( %i = 0;  %i <= %obj.numMembers;  %i++ )
				{
					if ( %i == %obj.numMembers )
					{
						%brickGroup = BrickGroup_888888;
					}
					else
					{
						%brickGroup = %obj.member[%i].brickGroup;
					}

					if ( %brickGroup.checkVal != $currCheckVal )
					{
						%currPercent += %brickGroup.spawnBrickCount / %totalSpawnPoints;

						if ( %currPercent >= %rnd )
						{
							%finalBrickGroup = %brickGroup;
						}
						else
						{
							// Nothing here?
						}
					}
				}

				if ( !isObject(%finalBrickGroup) )
				{
					%finalBrickGroup = %obj.member[%obj.numMembers - 1].brickGroup;
				}

				if ( isObject(%finalBrickGroup) )
				{
					return %finalBrickGroup.getBrickSpawnPoint();
				}
				else
				{
					error ("MiniGameSO::PickSpawnPoint() - no brick group found");
					return pickSpawnPoint();
				}
			}
		}
		else
		{
			%brickGroup = %obj.owner.brickGroup;

			if ( %brickGroup.spawnBrickCount > 0 )
			{
				return %brickGroup.getBrickSpawnPoint();
			}
			else
			{
				return BrickGroup_888888.getBrickSpawnPoint();
			}
		}
	}
	else
	{
		return pickSpawnPoint();
	}
}
