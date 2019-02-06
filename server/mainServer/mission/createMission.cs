function createMission ()
{
	endMission();

	echo ("");
	echo ("*** CREATING MISSION");
	echo ("*** Stage 1 create");

	clearCenterPrintAll();
	clearBottomPrintAll();

	$missionSequence++;
	$missionRunning = 0;

	if ( isObject(MissionGroup) )
	{
		MissionGroup.deleteAll();
		MissionGroup.delete();
	}

	new SimGroup (MissionGroup)
	{
		new Sky (Sky)
		{
			position = "336 136 0";
			rotation = "1 0 0 0";
			scale = "1 1 1";
			Wind = "0 0 0";
			materialList = "add-ons/sky_blue2/blue2.dml";
		};

		new Sun (Sun)
		{
			azimuth = 0;
			elevation = 45;
			color = "0.700000 0.700000 0.700000 1.000000";
			ambient = "0.400000 0.400000 0.300000 1.000000";
		};

		new fxPlane (groundPlane)
		{
			position = "0 0 -0.5";
			isSolid = 1;
		};

		new SimGroup (PlayerDropPoints)
		{
			new SpawnSphere ()
			{
				position = "0 0 0.1";
				dataBlock = "SpawnSphereMarker";
				radius = 40;
			};
		};

		new fxDayCycle (DayCycle)
		{
			position = "0 0 0";
		};

		new fxSunLight (SunLight)
		{
			position = "0 0 0";
			Enable = 1;
			LocalFlareBitmap = "base/lighting/corona.png";
			RemoteFlareBitmap = "base/lighting/corona.png";
		};
	};

	ServerGroup.add (MissionGroup);
	$instantGroup = ServerGroup;

	setManifestDirty();

	if ( isObject(MiniGameGroup) )
	{
		endAllMinigames();
		MiniGameGroup.delete();
	}

	new SimGroup (MiniGameGroup);

	if ( isObject(MissionCleanup) )
	{
		MissionCleanup.deleteAll();
		MissionCleanup.delete();
	}

	new SimGroup (MissionCleanup);

	$instantGroup = MissionCleanup;

	if ( isObject(GlobalQuota) )
	{
		GlobalQuota.delete();
	}

	new QuotaObject (GlobalQuota)
	{
		AutoDelete = 0;
	};

	GlobalQuota.setAllocs_Schedules (9999, 5465489);
	GlobalQuota.setAllocs_Misc (9999, 5465489);
	GlobalQuota.setAllocs_Projectile (9999, 5465489);
	GlobalQuota.setAllocs_Item (9999, 5465489);
	GlobalQuota.setAllocs_Environment (9999, 5465489);
	GlobalQuota.setAllocs_Player (9999, 5465489);
	GlobalQuota.setAllocs_Vehicle (9999, 5465489);

	ServerGroup.add (GlobalQuota);

	if ( !isObject(QuotaGroup) )
	{
		new SimGroup (QuotaGroup);
		RootGroup.add (QuotaGroup);
	}

	if ( !isObject(mainBrickGroup) )
	{
		new SimGroup (mainBrickGroup);
		MissionCleanup.add (mainBrickGroup);
	}

	%count = ClientGroup.getCount();

	for ( %i = 0;  %i < %count;  %i++ )
	{
		%client = ClientGroup.getObject (%i);

		if ( %client.getBLID() == -1 )
		{
			error ("ERROR: loadMissionStage2() - Client " @  %client.getPlayerName()  @ " has no bl_id");
		}
		else if ( isObject ( "BrickGroup_" @  %client.getBLID() ) )
		{
			%obj = "BrickGroup_" @  %client.getBLID();

			%client.brickGroup        = %obj.getId();
			%client.brickGroup.name   = %client.getPlayerName();
			%client.brickGroup.client = %client;
		}
		else
		{
			%client.brickGroup = new SimGroup ( "BrickGroup_" @  %client.getBLID() );
			mainBrickGroup.add (%client.brickGroup);

			%client.brickGroup.client = %client;
			%client.brickGroup.name   = %client.getPlayerName();
			%client.brickGroup.bl_id  = %client.getBLID();
		}

		commandToClient (%client, 'TrustListUpload_Start');
	}


	%groupName = "BrickGroup_888888";

	if ( !isObject(%groupName) )
	{
		%brickGroup = new SimGroup (%groupName);

		%brickGroup.bl_id       = 888888;
		%brickGroup.name        = "\c1BL_ID: 888888\c0";
		%brickGroup.QuotaObject = GlobalQuota;
		%brickGroup.DoNotDelete = 1;

		mainBrickGroup.add (%brickGroup);
	}

	%groupName = "BrickGroup_999999";

	if ( !isObject(%groupName) )
	{
		%brickGroup = new SimGroup (%groupName);

		%brickGroup.bl_id       = 999999;
		%brickGroup.name        = "\c1BL_ID: 999999\c0";
		%brickGroup.QuotaObject = GlobalQuota;
		%brickGroup.DoNotDelete = 1;

		mainBrickGroup.add (%brickGroup);
	}


	if ( $Server::LAN )
	{
		%count = ClientGroup.getCount();

		for ( %i = 0;  %i < %count;  %i++ )
		{
			%client = ClientGroup.getObject(%i);
			%client.brickGroup = %groupName;
		}
	}
	else
	{
		%groupName = "BrickGroup_" @  getNumKeyID();

		if ( !isObject(%groupName) )
		{
			%brickGroup = new SimGroup (%groupName);
			%brickGroup.bl_id = getNumKeyID();

			mainBrickGroup.add (%brickGroup);
		}
	}

	if ( $Server::Dedicated  &&  $QuitAfterMissionLoad )
	{
		quit();
	}

	if ( $Server::Dedicated  &&  $loadBlsArg !$= "" )
	{
		serverDirectSaveFileLoad ($loadBlsArg, 3);
		$loadBlsArg = "";
	}

	onMissionLoaded();
	purgeResources();

	if ( $MiniGame::Enabled )
	{
		if ( $MiniGame::PlayerDataBlockName $= "" )
		{
			$MiniGame::PlayerDataBlock = PlayerStandardArmor.getId();
		}
		else
		{
			$MiniGame::PlayerDataBlock = $uiNameTable_Player[$MiniGame::PlayerDataBlockName];

			if ( !isObject($MiniGame::PlayerDataBlock) )
			{
				$MiniGame::PlayerDataBlock = $MiniGame::PlayerDataBlockName;
			}

			if ( !isObject($MiniGame::PlayerDataBlock) )
			{
				$MiniGame::PlayerDataBlock = PlayerStandardArmor.getId();
			}
		}


		for ( %i = 0;  %i < 15;  %i++ )
		{
			if ( $MiniGame::StartEquipName[%i] !$= "" )
			{
				$MiniGame::StartEquip[%i] = $uiNameTable_Items [ $MiniGame::StartEquipName[%i] ];

				if ( !isObject ( $MiniGame::StartEquip[%i] ) )
				{
					if ( isObject ( $MiniGame::StartEquipName[%i] ) )
					{
						$MiniGame::StartEquip[%i] = $MiniGame::StartEquipName[%i];
					}
					else
					{
						$MiniGame::StartEquip[%i] = 0;
					}
				}
			}
		}

		$DefaultMiniGame = new ScriptObject ()
		{
			class = MiniGameSO;
			owner = 0;
			title = "Default Minigame";
			colorIdx = $MiniGame::GameColor;
			numMembers = 0;
			InviteOnly = $MiniGame::InviteOnly;

			UseAllPlayersBricks = $MiniGame::IncludeAllPlayersBricks;
			PlayersUseOwnBricks = $MiniGame::PlayersUseOwnBricks;
			UseSpawnBricks      = $MiniGame::UseSpawnBricks;

			Points_BreakBrick   = $MiniGame::Points_BreakBrick;
			Points_PlantBrick   = $MiniGame::Points_PlantBrick;
			Points_KillPlayer   = $MiniGame::Points_KillPlayer;
			Points_KillSelf     = $MiniGame::Points_KillSelf;
			Points_KillBot      = $MiniGame::Points_KillBot;
			Points_Die          = $MiniGame::Points_Die;

			RespawnTime        = $MiniGame::RespawnTime;
			VehicleRespawnTime = $MiniGame::VehicleRespawnTime;
			BrickRespawnTime   = $MiniGame::BrickRespawnTime;
			BotRespawnTime     = $MiniGame::BotRespawnTime;

			FallingDamage = $MiniGame::FallingDamage;
			WeaponDamage  = $MiniGame::WeaponDamage;
			SelfDamage    = $MiniGame::SelfDamage;
			VehicleDamage = $MiniGame::VehicleDamage;
			BrickDamage   = $MiniGame::BrickDamage;
			BotDamage     = $MiniGame::BotDamage;

			EnableWand     = $MiniGame::EnableWand;
			EnableBuilding = $MiniGame::EnableBuilding;
			EnablePainting = $MiniGame::EnablePainting;

			PlayerDataBlock = $MiniGame::PlayerDataBlock;

			StartEquip0 = $MiniGame::StartEquip0;
			StartEquip1 = $MiniGame::StartEquip1;
			StartEquip2 = $MiniGame::StartEquip2;
			StartEquip3 = $MiniGame::StartEquip3;
			StartEquip4 = $MiniGame::StartEquip4;

			TimeLimit = $MiniGame::TimeLimit;
		};

		MiniGameGroup.add ($DefaultMiniGame);
	}

	if ( !isUnlocked() )
	{
		if ( !isValidDemoCRC ( getFileCRC($SaveFileArg) ) )
		{
			echo ("Save file argument \'" @  $SaveFileArg  @ "\' is not allowed in demo");
			$SaveFileArg = "";
		}
	}

	if ( $SaveFileArg !$= "" )
	{
		if ( $GameModeArg $= "" )
		{
			serverDirectSaveFileLoad ($SaveFileArg, 3, "", 2);
		}
		else if ( $GameMode::BrickOwnership $= "Host" )
		{
			serverDirectSaveFileLoad ($SaveFileArg, 3, "", 0);
		}
		else if ( $GameMode::BrickOwnership $= "SavedOwner" )
		{
			serverDirectSaveFileLoad ($SaveFileArg, 3, "", 1);
		}
		else
		{
			serverDirectSaveFileLoad ($SaveFileArg, 3, "", 2);
		}
	}

	$missionRunning = 1;

	%count = ClientGroup.getCount();

	for ( %clientIndex = 0;  %clientIndex < %count;  %clientIndex++ )
	{
		%client = ClientGroup.getObject(%clientIndex);
		%client.loadMission();
	}
}
