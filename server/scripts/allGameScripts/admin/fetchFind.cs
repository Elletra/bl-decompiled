function serverCmdFetch ( %client, %victimName )
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


	%victimClient = findclientbyname (%victimName);

	if ( %victimClient )
	{
		%victimPlayer = %victimClient.Player;

		if ( isObject(%victimPlayer) )
		{
			%client.lastF8Time = getSimTime();
			%player.teleportEffect();

			if ( !%victimPlayer.isMounted() )
			{
				%victimPlayer.setTransform ( %player.getTransform() );
				%victimPlayer.setVelocity ("0 0 0");
			}
			else
			{
				%mount = %victimPlayer;
				%i = 0;

				while ( %i < 100 )
				{
					if ( %mount.isMounted() )
					{
						%mount = %mount.getObjectMount();
					}

					%i++;
				}

				if ( %mount.getClassName() $= "Player"  ||  %mount.getClassName() $= "AIPlayer"  || 
					 %mount.getClassName() $= "WheeledVehicle"  ||  %mount.getClassName() $= "FlyingVehicle"  || 
					 %mount.getClassName() $= "HoverVehicle")
				{
					%mount.setTransform ( %player.getTransform() );
					%mount.setVelocity ("0 0 0");

					%mount.teleportEffect();
				}
			}
		}
	}
}

function serverCmdFind ( %client, %victimName )
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


	%victimClient = findclientbyname (%victimName);

	if ( %victimClient )
	{
		%victimPlayer = %victimClient.Player;

		if ( isObject(%victimPlayer) )
		{
			%client.lastF8Time = getSimTime();

			if ( !%player.isMounted() )
			{
				%player.setTransform ( %victimPlayer.getTransform() );
				%player.setVelocity ("0 0 0");

				%player.teleportEffect();
			}
			else
			{
				%mount = %player;
				%i = 0;

				while ( %i < 100 )
				{
					if ( %mount.isMounted() )
					{
						%mount = %mount.getObjectMount();
					}
					
					%i++;
				}

				if ( %mount.getClassName() $= "Player"  ||  %mount.getClassName() $= "AIPlayer"  || 
					 %mount.getClassName() $= "WheeledVehicle"  ||  %mount.getClassName() $= "FlyingVehicle"  || 
					 %mount.getClassName() $= "HoverVehicle")
				{
					%mount.setTransform ( %victimPlayer.getTransform() );
					%mount.setVelocity ("0 0 0");

					%mount.teleportEffect();
				}
			}
		}
	}
}
