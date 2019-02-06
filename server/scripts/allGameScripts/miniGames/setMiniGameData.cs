$Game::MinRespawnTime = 1000;
$Game::MaxRespawnTime = 30000;

$Game::MinVehicleRespawnTime = 0;
$Game::MaxVehicleRespawnTime = 360000;

$Game::MinBrickRespawnTime = 2000;
$Game::MaxBrickRespawnTime = 360000;

$Game::MinBotRespawnTime = 0;
$Game::MaxBotRespawnTime = 360000;

$Game::Item::PopTime = 10.0 * 1000.0;
$Game::Item::RespawnTime = 4000;
$Game::Item::MinRespawnTime = 1000;
$Game::Item::MaxRespawnTime = 360000;

$Game::MiniGameJoinTime = 5000;


function serverCmdSetMiniGameData ( %client, %line )
{
	if ( !isObject(%client.miniGame) )
	{
		return;
	}

	if ( %client.miniGame.owner != %client )
	{
		return;
	}

	%mg = %client.miniGame;

	%fieldCount = getFieldCount (%line);
	%sendUpdate = false;

	for ( %i = 0;  %i < %fieldCount;  %i++ )
	{
		%field = getField (%line, %i);
		%type = getWord (%field, 0);

		if ( %type $= "T" )
		{
			%title = getSubStr (%field, 2, strlen(%field) - 2);
			%title = getSubStr (%title, 0, 35);

			%mg.title = %title;

			%sendUpdate = true;
		}
		else if ( %type $= "IO" )
		{
			%mg.InviteOnly = mFloor ( getWord(%field, 1) );
			%sendUpdate = 1;
		}
		else if ( %type $= "UAPB" )
		{
			%mg.UseAllPlayersBricks = mFloor ( getWord(%field, 1) );
		}
		else if ( %type $= "PUOB" )
		{
			%mg.PlayersUseOwnBricks = mFloor ( getWord(%field, 1) );
		}
		else if ( %type $= "USB" )
		{
			%usb = mFloor ( getWord(%field, 1) );

			if ( %mg.UseSpawnBricks != %usb )
			{
				%mg.UseSpawnBricks = %usb;
				%mg.RespawnAll();
			}
		}
		else if ( %type $= "PBB" )
		{
			%mg.Points_BreakBrick = mFloor ( getWord(%field, 1) );
		}
		else if ( %type $= "PPB" )
		{
			%mg.Points_PlantBrick = mFloor ( getWord(%field, 1) );
		}
		else if ( %type $= "PKP" )
		{
			%mg.Points_KillPlayer = mFloor ( getWord(%field, 1) );
		}
		else if ( %type $= "PKS" )
		{
			%mg.Points_KillSelf = mFloor ( getWord(%field, 1) );
		}
		else if ( %type $= "PKB" )
		{
			%mg.Points_KillBot = mFloor ( getWord(%field, 1) );
		}
		else if ( %type $= "PD" )
		{
			%mg.Points_Die = mFloor ( getWord(%field, 1) );
		}
		else if ( %type $= "RT" )
		{
			%time = getWord (%field, 1) * 1000;

			if ( %time > 0 )
			{
				if ( %time < $Game::MinRespawnTime )
				{
					%time = $Game::MinRespawnTime;
				}
				if (%time > $Game::MaxRespawnTime)
				{
					%time = $Game::MaxRespawnTime;
				}
			}
			else
			{
				%time = -1;
			}

			%mg.RespawnTime = %time;
		}
		else if ( %type $= "VRT" )
		{
			%time = getWord (%field, 1) * 1000;

			if ( %time > 0 )
			{
				if ( %time < $Game::MinVehicleRespawnTime )
				{
					%time = $Game::MinVehicleRespawnTime;
				}

				if ( %time > $Game::MaxVehicleRespawnTime )
				{
					%time = $Game::MaxVehicleRespawnTime;
				}
			}
			else
			{
				%time = -1;
			}

			%mg.VehicleRespawnTime = %time;
		}
		else if ( %type $= "BRT" )
		{
			%time = getWord (%field, 1) * 1000;

			if ( %time < $Game::MinBrickRespawnTime )
			{
				%time = $Game::MinBrickRespawnTime;
			}

			if ( %time > $Game::MaxBrickRespawnTime )
			{
				%time = $Game::MaxBrickRespawnTime;
			}

			%mg.BrickRespawnTime = %time;
		}
		else if ( %type $= "BtRT" )
		{
			%time = getWord (%field, 1) * 1000;

			if ( %time < $Game::MinBotRespawnTime )
			{
				%time = $Game::MinBotRespawnTime;
			}

			if ( %time > $Game::MaxBotRespawnTime )
			{
				%time = $Game::MaxBotRespawnTime;
			}

			%mg.BotRespawnTime = %time;
		}
		else if ( %type $= "DB" )
		{
			%db = mFloor ( getWord(%field, 1) );

			if ( isObject(%db) )
			{
				if ( %db.getClassName() $= "PlayerData" )
				{
					if ( %db.uiName $= "" )
					{
						%mg.PlayerDataBlock = PlayerStandardArmor.getId();
					}
					else
					{
						%mg.PlayerDataBlock = %db;
					}
				}
				else
				{
					%mg.PlayerDataBlock = PlayerStandardArmor.getId();
				}

				%mg.updatePlayerDataBlock();
			}
		}
		else if ( %type $= "FD" )
		{
			%mg.FallingDamage = mFloor ( getWord(%field, 1) );
		}
		else if ( %type $= "WD" )
		{
			%mg.WeaponDamage = mFloor ( getWord(%field, 1) );
		}
		else if ( %type $= "SD" )
		{
			%mg.SelfDamage = mFloor ( getWord(%field, 1) );
		}
		else if ( %type $= "VD" )
		{
			%mg.VehicleDamage = mFloor ( getWord(%field, 1) );
		}
		else if ( %type $= "BD" )
		{
			%mg.BrickDamage = mFloor ( getWord(%field, 1) );
		}
		else if ( %type $= "BtD" )
		{
			%mg.BotDamage = mFloor ( getWord(%field, 1) );
		}
		else if ( %type $= "EW" )
		{
			%mg.EnableWand = mFloor ( getWord(%field, 1) );
		}
		else if ( %type $= "EB" )
		{
			%val = mFloor ( getWord(%field, 1) );

			if ( %mg.EnableBuilding != %val )
			{
				%mg.EnableBuilding = %val;
				%mg.updateEnableBuilding();
			}
		}
		else if ( %type $= "EP" )
		{
			%val = mFloor ( getWord(%field, 1) );

			if ( %mg.EnablePainting != %val )
			{
				%mg.EnablePainting = %val;
				%mg.updateEnablePainting();
			}
		}
		else if ( %type $= "SE" )
		{
			%idx = mFloor ( getWord(%field, 1) );
			%db = mFloor ( getWord(%field, 2) );

			if ( %mg.startEquip[%idx] != %db )
			{
				if ( !isObject(%db) )
				{
					%mg.startEquip[%idx] = 0;
				}
				else if ( %db.getClassName() $= "ItemData" )
				{
					if ( %db.uiName !$= "" )
					{
						%mg.startEquip[%idx] = %db;
					}
					else
					{
						%mg.startEquip[%idx] = 0;
					}
				}

				%mg.forceEquip (%idx);
			}
		}
		else
		{
			error("ERROR: ServerCmdSetMiniGameData() - Unknown type " @ %type @ "");
		}
	}

	if ( %sendUpdate )
	{
		commandToAll ('AddMiniGameLine', %mg.getLine(), %mg, %mg.colorIdx);
	}
}
