function Observer::onTrigger ( %this, %obj, %trigger, %state )
{
	if ( %state == 0 )
	{
		return;
	}

	if ( $Game::MissionCleaningUp )
	{
		return;
	}


	%client = %obj.getControllingClient();
	%player = %client.Player;

	%mg = %client.miniGame;

	%elapsedTime = getSimTime() - %client.deathTime;

	if ( %obj.mode $= "Observer" )
	{
		%mg = %client.miniGame;
		%elapsedTime = getSimTime() - %client.deathTime;

		if ( isObject(%mg) )
		{
			if ( %mg.RespawnTime < 0  &&  %trigger == 2  &&  %obj.getControlObject() <= 0 )
			{
				%clientCount = ClientGroup.getCount();

				%bestPlayer = 0;
				%bestDistance = 99999;

				%pos = %obj.getPosition();

				for ( %i = 0;  %i < %clientCount;  %i++ )
				{
					%cl = ClientGroup.getObject (%i);
					%player = %cl.Player;

					if ( isObject(%player)  &&  %cl.miniGame == %mg )
					{
						%testDistance = VectorLen ( VectorSub(%player.getPosition(), %pos) );

						if ( %testDistance <= %bestDistance )
						{
							%bestDistance = %testDistance;
							%bestPlayer = %player;
						}
					}
				}

				if ( isObject(%bestPlayer) )
				{
					%obj.setOrbitMode (%bestPlayer, %obj.getTransform(), 0, 8, 8);
					%obj.mode = "Corpse";
				}
			}
		}

		%playerDead = 0;

		if ( isObject(%player) )
		{
			if ( %player.getDamagePercent() >= 1.0 )
			{
				%playerDead = 1;
			}
		}
		else
		{
			%playerDead = 1;
		}

		if ( %playerDead )
		{
			%respawnTime = $Game::MinRespawnTime;

			if ( isObject(%mg) )
			{
				%respawnTime = %mg.RespawnTime;
			}

			if ( %elapsedTime > %respawnTime  &&  %respawnTime > 0  &&  %trigger == 0 )
			{
				%client.spawnPlayer();
				%client.setCanRespawn (false);
				%this.setMode (%obj, "Observer");
			}
		}
	}
	else if ( %obj.mode $= "Corpse" )
	{
		if ( isObject(%client.Player) )
		{
			if ( %client.Player.canDismount )
			{
				if ( %client.Player.getDamagePercent() < 1.0 )
				{
					%client.setControlObject (%client.Player);
					%this.setMode (%obj, "Observer");

					return;
				}
			}
			else if ( %client.Player.getDamagePercent() < 1.0 )
			{
				return;
			}
		}

		if ( isObject(%mg) )
		{
			if ( %mg.RespawnTime < 0 )
			{
				if ( %trigger == 0  ||  %trigger == 4 )
				{
					%currTarget = %obj.getOrbitObject();

					if ( isObject(%currTarget) )
					{
						%currClient = %currTarget.client;
					}

					%clientCount = ClientGroup.getCount();
					%offset = 0;

					if ( %i < %clientCount )
					{
						for ( %i = 0;  %i < %clientCount;  %i++ )
						{
							%cl = ClientGroup.getObject (%i);

							if ( %cl == %currClient )
							{
								%offset = %i;
							}
						}
					}

					if ( %trigger == 0 )
					{
						for ( %i = 0;  %i < %clientCount;  %i++ )
						{
							%idx = %clientCount % %offset + %i + 1;
							%cl = ClientGroup.getObject (%idx);
							%player = %cl.Player;

							if ( isObject(%player) )
							{
								if (%cl.miniGame == %mg)
								{
									%obj.setOrbitMode (%player, %obj.getTransform(), 0, 8, 8);
									break;
								}
							}
						}
					}
					else if ( %trigger == 4 )
					{
						for ( %i = 0;  %i < %clientCount;  %i++ )
						{
							%idx = %offset - %i - 1;

							if ( %idx < 0 )
							{
								%idx += %clientCount;
							}

							%cl = ClientGroup.getObject (%idx);

							%player = %cl.Player;

							if ( isObject(%player) )
							{
								if ( %cl.miniGame == %mg )
								{
									%obj.setOrbitMode (%player, %obj.getTransform(), 0, 8, 8);
									break;
								}
							}
						}
					}
				}
				else if ( %trigger == 2 )
				{
					%currTarget = %obj.getOrbitObject();
					%this.setMode (%obj, "Observer");
				}
			}
			else if ( %elapsedTime > %mg.RespawnTime  &&  %mg.RespawnTime > 0 )
			{
				if ( %trigger == 0 )
				{
					%client.spawnPlayer();
					%client.setCanRespawn (false);

					%this.setMode (%obj, "Observer");
				}
			}
		}
		else if ( %elapsedTime > $Game::MinRespawnTime )
		{
			if ( %trigger == 0 )
			{
				%client.spawnPlayer();
				%client.setCanRespawn (false);

				%this.setMode (%obj, "Observer");
			}
		}
	}
}
