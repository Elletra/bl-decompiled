function getMiniGameFromObject ( %obj )
{
	if ( !isObject(%obj) )
	{
		return -1;
	}

	%miniGame = -1;

	if ( isObject(%obj.miniGame) )
	{
		return %obj.miniGame;
	}

	if ( %obj.getClassName() $= "ScriptObject" )
	{
		if ( %obj.class $= "MiniGameSO" )
		{
			%miniGame = %obj;
		}
		else
		{
			%miniGame = -1;
		}
	}
	else if ( %obj.getClassName() $= "GameConnection" )
	{
		%miniGame = %obj.miniGame;
	}
	else if ( %obj.getType() & $TypeMasks::PlayerObjectType  ||  %obj.getType() & $TypeMasks::CorpseObjectType )
	{
		if ( isObject(%obj.client) )
		{
			%miniGame = %obj.client.miniGame;
		}
		else
		{
			if ( $Server::LAN )
			{
				if ( isObject(%obj.spawnBrick) )
				{
					if ( isObject(%obj.spawnBrick.client) )
					{
						%miniGame = %obj.spawnBrick.client.miniGame;
					}
				}
			}
			else if ( isObject(%obj.spawnBrick) )
			{
				if ( isObject ( %obj.spawnBrick.getGroup().getClient() ) )
				{
					%miniGame = %obj.spawnBrick.getGroup().getClient().miniGame;
				}
			}
		}
	}
	else if ( %obj.getType() & $TypeMasks::ItemObjectType )
	{
		if ( $Server::LAN )
		{
			return -1;  // FIXME???

			if ( isObject(%obj.spawnBrick) )
			{
				if ( isObject(%obj.spawnBrick.client) )
				{
					%miniGame = %obj.spawnBrick.client.miniGame;
				}
			}
		}
		else
		{
			if ( isObject(%obj.spawnBrick) )
			{
				if ( isObject ( %obj.spawnBrick.getGroup().getClient() ) )
				{
					%miniGame = %obj.spawnBrick.getGroup().getClient().miniGame;
				}
			}
			else
			{
				%miniGame = %obj.miniGame;
			}
		}
	}
	else if ( %obj.getType() & $TypeMasks::FxBrickAlwaysObjectType )
	{
		if ( $Server::LAN )
		{
			%clientCount = ClientGroup.getCount();

			for ( %i = 0;  %i < %clientCount;  %i++ )
			{
				%cl = ClientGroup.getObject (%i);

				if ( isObject(%cl.miniGame) )
				{
					%miniGame = %cl.miniGame;
				}
			}
		}
		else if ( isObject ( %obj.getGroup().getClient() ) )
		{
			%miniGame = %obj.getGroup().getClient().miniGame;
		}
	}
	else if ( %obj.getType() & $TypeMasks::VehicleObjectType )
	{
		if ( $Server::LAN )
		{
			return -1;  // FIXME???

			if ( isObject(%obj.spawnBrick) )
			{
				if ( isObject(%obj.spawnBrick.client) )
				{
					%miniGame = %obj.spawnBrick.client.miniGame;
				}
			}
		}
		else if (isObject(%obj.spawnBrick))
		{
			if ( isObject ( %obj.spawnBrick.getGroup().getClient() ) )
			{
				%miniGame = %obj.spawnBrick.getGroup().getClient().miniGame;
			}
		}
	}
	else if ( %obj.getType() & $TypeMasks::ProjectileObjectType )
	{
		if ( isObject(%obj.client) )
		{
			%miniGame = %obj.client.miniGame;
		}
	}
	else
	{
		if ( !isObject(%miniGame)  &&  isObject(%obj.miniGame) )
		{
			%miniGame = %obj.miniGame;
		}

		if ( !$Server::LAN )
		{
			if ( !isObject(%miniGame)  &&  isObject(%obj.spawnBrick) )
			{
				if ( isObject ( %obj.spawnBrick.getGroup().getClient() ) )
				{
					%miniGame = %obj.spawnBrick.getGroup().getClient().miniGame;
				}
			}
		}

		if ( !isObject(%miniGame)  &&  isObject(%obj.client) )
		{
			%miniGame = %obj.client.miniGame;
		}
	}

	%blid = getBL_IDFromObject (%obj);

	if ( %blid == 888888 )
	{
		if ( isObject($DefaultMiniGame) )
		{
			%miniGame = $DefaultMiniGame;
		}
	}

	if ( !isObject(%miniGame) )
	{
		if ( isObject($DefaultMiniGame) )
		{
			%miniGame = $DefaultMiniGame;
		}
	}

	if ( !isObject(%miniGame) )
	{
		%miniGame = -1;
	}

	return %miniGame;
}
