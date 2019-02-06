function serverCmdDropPlayerAtCamera ( %client )
{
	if ( !%client.isAdmin )
	{
		return;
	}


	%client.Camera.unmountImage (0);
	%player = %client.Player;

	if ( isObject(%player) )
	{
		if ( %player.getDamagePercent() < 1.0 )
		{
			if ( isObject(%client.miniGame) )
			{
				%client.incScore (-1);
			}

			%client.lastF8Time = getSimTime();

			if ( !%player.isMounted() )
			{
				%pos = getWords (%client.Camera.getTransform(), 0, 2);
				%rot = getWords (%client.Camera.getTransform(), 3, 7);

				%offset = VectorSub (%player.getEyePoint(), %player.getPosition());

				%start = %pos;
				%end = VectorSub (%pos, %offset);

				%mask = $TypeMasks::StaticObjectType | $TypeMasks::FxBrickObjectType;

				%raycast = containerRayCast (%start, %end, %mask);

				if ( %raycast )
				{
					%pos = posFromRaycast (%raycast);
				}
				else
				{
					%pos = VectorSub (%pos, %offset);
				}

				%player.setTransform (%pos SPC %rot);
				%player.setVelocity ("0 0 0");
				%client.setControlObject (%player);

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
					%mount.setTransform ( %client.Camera.getTransform() );

					if ( %mount.getType() & $TypeMasks::VehicleObjectType )
					{
						%mount.setAngularVelocity ("0 0 0");
					}

					%mount.setVelocity ("0 0 0");
					%mount.teleportEffect();
					%client.setControlObject (%player);
				}
			}
		}
		else
		{
			if ( isObject(%client.miniGame) )
			{
				if ( %client.miniGame.RespawnTime <= 0 )
				{
					return;
				}
			}

			%client.spawnPlayer();
		}
	}
	else
	{
		if ( isObject(%client.miniGame) )
		{
			if ( %client.miniGame.RespawnTime <= 0 )
			{
				return;
			}
		}

		%client.spawnPlayer();
	}
}

function serverCmdDropCameraAtPlayer ( %client )
{
	if ( !%client.isAdmin )
	{
		return;
	}

	if ( isObject(%client.Player) )
	{
		%client.Camera.setTransform ( %client.Player.getEyeTransform() );
		%client.Camera.setVelocity ("0 0 0");
	}

	%client.setControlObject (%client.Camera);
	%client.Camera.setControlObject (0);

	%client.Camera.mountImage (cameraImage, 0);

	%client.Camera.setFlyMode();
	%client.Camera.setMode ("Observer");
}
