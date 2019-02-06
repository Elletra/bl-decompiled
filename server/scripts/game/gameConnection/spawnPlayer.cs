function pickSpawnPoint ()
{
	%groupName = "MissionGroup/PlayerDropPoints";
	%group = nameToID (%groupName);

	if ( %group != -1 )
	{
		%count = %group.getCount();

		if ( %count != 0 )
		{
			%index = getRandom (%count - 1);
			%spawn = %group.getObject (%index);


			%rayHeight = %spawn.RayHeight;

			if ( %rayHeight $= "" )
			{
				%rayHeight = 100;
			}

			%trans = %spawn.getTransform();

			%transX = getWord (%trans, 0);
			%transY = getWord (%trans, 1);
			%transZ = getWord (%trans, 2);

			for ( %i = 0;  %i < 1000;  %i++ )
			{
				%r = getRandom (%spawn.radius * 10) / 10;
				%ang = getRandom ($pi * 2 * 100) / 100;

				%transX = getWord (%trans, 0);
				%transY = getWord (%trans, 1);
				%transZ = getWord (%trans, 2);

				%offsetX = getRandom() * %spawn.radius * 2 - %spawn.radius;
				%offsetY = getRandom() * %spawn.radius * 2 - %spawn.radius;


				if ( VectorLen(%offsetX SPC %offsetY SPC 0) <= %spawn.radius )
				{
					%transX = %transX + %offsetX;
					%transY = %transY + %offsetY;

					%start = %transX SPC %transY SPC %transZ + %rayHeight;
					%end = %transX SPC %transY SPC %transZ - 2;

					%mask = $TypeMasks::FxBrickObjectType | $TypeMasks::StaticObjectType | 
						$TypeMasks::StaticShapeObjectType | $TypeMasks::PlayerObjectType;
					
					%scanTarg = containerRayCast (%start, %end, %mask, 0);

					if ( %scanTarg )
					{
						%scanPos = posFromRaycast (%scanTarg);
						%transZ = getWord (%scanPos, 2);
						%boxCenter = VectorAdd (%scanPos, "0 0 1.6");

						%mask = $TypeMasks::FxBrickObjectType | $TypeMasks::StaticShapeObjectType | $TypeMasks::PlayerObjectType;

						if ( containerBoxClear(%mask, %boxCenter, 0.6, 0.6, 1.3) )
						{
							break;
						}
					}
				}
			}

			if ( %spawn.directional )
			{
				%spawnAngle = " " @  getWords (%spawn.getTransform(), 3, 6);
			}
			else
			{
				%spawnAngle = " 0 0 1 " @  getRandom ($pi * 2 * 100) / 100;
			}

			%returnTrans = %transX  @ " " @  %transY  @ " " @  %transZ @ %spawnAngle;
			return %returnTrans;
		}
		else
		{
			error ("No spawn points found in " @  %groupName);
		}
	}
	else
	{
		error ("Missing spawn points group " @  %groupName);
	}

	error ("default spawn!");
	return "0 0 300 1 0 0 0";
}

function GameConnection::spawnPlayer ( %client )
{
	if ( isObject(%client.Player) )
	{
		if ( %client.Player.getDamagePercent() < 1.0 )
		{
			%client.Player.delete();
		}
	}

	%spawnPoint = %client.getSpawnPoint();
	%client.createPlayer (%spawnPoint);


	if ( isObject(%client.Camera) )
	{
		%client.Camera.unmountImage (0);
	}


	messageClient (%client, 'MsgYourSpawn');

	if ( !%client.hasSpawnedOnce )
	{
		%client.hasSpawnedOnce = 1;

		messageAllExcept ( %client, -1, '', '\c1%1 spawned.', %client.getPlayerName() );
		echo (%client.getPlayerName()  @ " spawned.");

		steamGetAchievement ("ACH_HOST_SERVER", "steamGetAchievement");
	}
}

function GameConnection::getSpawnPoint ( %client )
{
	if ( isObject(%client.miniGame) )
	{
		%spawnPoint = %client.miniGame.pickSpawnPoint (%client);
	}
	else
	{
		%spawnPoint = %client.brickGroup.getBrickSpawnPoint();
	}

	return %spawnPoint;
}
