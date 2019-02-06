exec ("./kickBan/kickBan.cs");

exec ("./fetchFind.cs");
exec ("./dropCamera.cs");
exec ("./clear.cs");
exec ("./resetVehicles.cs");
exec ("./sad.cs");

exec ("./miscellaneous.cs");


function serverCmdSpy ( %client, %victimName )
{
	if ( !%client.isAdmin )
	{
		return;
	}


	if ( mFloor(%victimName) > 0 )
	{
		if ( isObject(%victimName.Player) )
		{
			%client.Camera.setMode ("Corpse", %victimName.Player);
			%client.setControlObject (%client.Camera);

			%client.isSpying = 1;
		}
		else
		{
			messageClient (%client, '', "Client does not have a player object");
		}
	}
	else
	{
		%victimClient = findclientbyname (%victimName);

		if ( %victimClient )
		{
			%victimPlayer = %victimClient.Player;

			if ( isObject(%victimPlayer) )
			{
				%client.Camera.setMode ("Corpse", %victimPlayer);
				%client.setControlObject (%client.Camera);

				%client.isSpying = 1;
			}
			else
			{
				messageClient (%client, '', "Client does not have a player object");
			}
		}
		else
		{
			messageClient (%client, '', "Client not found");
		}
	}
}

function serverCmdRet ( %client )
{
	if ( !%client.isAdmin )
	{
		return;
	}

	%client.isSpying = 0;
	%client.setControlObject (%client.Player);
}

function serverCmdWarp ( %client )
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


	%start = %player.getEyePoint();

	%eyeVec = %player.getEyeVector();
	%vector = VectorScale (%eyeVec, 1000);

	%end = VectorAdd (%start, %vector);

	%searchMasks = $TypeMasks::StaticObjectType | $TypeMasks::FxBrickObjectType;


	%scanTarg = containerRayCast (%start, %end, %searchMasks, %player);

	if ( %scanTarg )
	{
		%scanObj = getWord (%scanTarg, 0);
		%scanPos = getWords (%scanTarg, 1, 3);

		%player.setVelocity ("0 0 0");
		%player.setTransform (%scanPos);

		%player.teleportEffect();
	}
}
