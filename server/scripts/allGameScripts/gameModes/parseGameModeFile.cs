function GameModeGuiServer::parseGameModeFile ( %filename, %append )
{
	if ( !isFile(%filename) )
	{
		error ("ERROR GameModeGuiServer::ParseGameModeFile(" @  %filename  @ ", " @  %append  @ ") - file does not exist");
		return false;
	}

	if ( !isUnlocked() )
	{
		%path = filePath (%filename);
		%zipFile = %path  @ ".zip";

		if ( isFile(%zipFile) )
		{
			%crc = getFileCRC (%zipFile);

			if ( !isValidDemoCRC(%crc) )
			{
				error ("ERROR: GameModeGuiServer::ParseGameModeFile(" @  %filename  @ ") - Cannot use this add-on in demo mode");
				return false;
			}
		}
		else
		{
			error ("ERROR: GameModeGuiServer::ParseGameModeFile(" @  %filename  @ ") - Cannot load non-zipped game modes in demo mode");
			return false;
		}
	}

	if ( !%append )
	{
		deleteVariables ("$GameMode::*");

		$GameMode::AddOnCount = 0;
		$GameMode::MusicCount = 0;
	}

	%file = new FileObject();
	%file.openForRead (%filename);

	while ( !%file.isEOF() )
	{
		%line = %file.readLine();
		%label = getWord (%line, 0);
		%value = getWords (%line, 1, 999);
		%value = trim (%value);

		if ( %label !$= "" )
		{
			if ( getSubStr(%label, 0, 2) !$= "//" )
			{
				if ( %label $= "ADDON" )
				{
					$GameMode::AddOn[$GameMode::AddOnCount] = %value;
					$GameMode::AddOnCount++;
				}
				else if ( %label $= "MUSIC" )
				{
					$GameMode::Music[$GameMode::MusicCount] = %value;
					$GameMode::MusicCount++;
				}
				else if ( %label $= "$EnvGuiServer::SimpleMode" )
				{
					$EnvGuiServer::SimpleMode = mClamp (%value, 0, 1);
				}
				else if ( %label $= "$EnvGuiServer::SkyFile" )
				{
					$EnvGuiServer::SkyFile = %value;
				}
				else if ( %label $= "$EnvGuiServer::WaterFile" )
				{
					$EnvGuiServer::WaterFile = %value;
				}
				else if ( %label $= "$EnvGuiServer::GroundFile" )
				{
					$EnvGuiServer::GroundFile = %value;
				}
				else if ( %label $= "$EnvGuiServer::SunFlareTopTexture" )
				{
					$Sky::SunFlareTopTexture = %value;
				}
				else if ( %label $= "$EnvGuiServer::SunFlareBottomTexture" )
				{
					$Sky::SunFlareBottomTexture = %value;
				}
				else if ( %label $= "$EnvGuiServer::DayOffset" )
				{
					$EnvGuiServer::DayOffset = mClampF (%value, 0, 1);
				}
				else if ( %label $= "$EnvGuiServer::DayLength" )
				{
					$EnvGuiServer::DayLength = mClamp (%value, 0, 86400);
				}
				else if ( %label $= "$EnvGuiServer::DayCycleEnabled" )
				{
					$EnvGuiServer::DayCycleEnabled = mClamp (%value, 0, 1);
				}
				else if ( %label $= "$EnvGuiServer::DayCycle" )
				{
					$Sky::DayCycleFile = %value;
				}
				else if ( %label $= "$EnvGuiServer::SunAzimuth" )
				{
					$EnvGuiServer::SunAzimuth = mClampF (%value, -360, 360);
				}
				else if ( %label $= "$EnvGuiServer::SunElevation" )
				{
					$EnvGuiServer::SunElevation = mClampF (%value, -360, 360);
				}
				else if ( %label $= "$EnvGuiServer::DirectLightColor" )
				{
					$EnvGuiServer::DirectLightColor = getColorF (%value);
				}
				else if ( %label $= "$EnvGuiServer::AmbientLightColor" )
				{
					$EnvGuiServer::AmbientLightColor = getColorF (%value);
				}
				else if ( %label $= "$EnvGuiServer::ShadowColor" )
				{
					$EnvGuiServer::ShadowColor = getColorF (%value);
				}
				else if ( %label $= "$EnvGuiServer::SunFlareColor" )
				{
					$EnvGuiServer::SunFlareColor = getColorF (%value);
				}
				else if ( %label $= "$EnvGuiServer::SunFlareSize" )
				{
					$EnvGuiServer::SunFlareSize = mClampF (%value, 0, 5);
				}
				else if ( %label $= "$EnvGuiServer::VisibleDistance" )
				{
					$EnvGuiServer::VisibleDistance = mClampF (%value, 20, 1000);
				}
				else if ( %label $= "$EnvGuiServer::FogDistance" )
				{
					$EnvGuiServer::FogDistance = mClampF (%value, 0, 1000);
				}
				else if ( %label $= "$EnvGuiServer::FogColor" )
				{
					$EnvGuiServer::FogColor = getColorF (%value);
				}
				else if ( %label $= "$EnvGuiServer::WaterColor" )
				{
					$EnvGuiServer::WaterColor = getColorF (%value);
				}
				else if ( %label $= "$EnvGuiServer::WaterHeight" )
				{
					$EnvGuiServer::WaterHeight = mClampF (%value, 1, 1000);
				}
				else if ( %label $= "$EnvGuiServer::UnderWaterColor" )
				{
					$EnvGuiServer::UnderWaterColor = getColorF (%value);
				}
				else if ( %label $= "$EnvGuiServer::SkyColor" )
				{
					$EnvGuiServer::SkyColor = getColorF (%value);
				}
				else if ( %label $= "$EnvGuiServer::WaterScrollX" )
				{
					$EnvGuiServer::WaterScrollX = mClampF (%value, -10, 10);
				}
				else if ( %label $= "$EnvGuiServer::WaterScrollY" )
				{
					$EnvGuiServer::WaterScrollY = mClampF (%value, -10, 10);
				}
				else if ( %label $= "$EnvGuiServer::GroundColor" )
				{
					$EnvGuiServer::GroundColor = getColorF (%value);
				}
				else if ( %label $= "$EnvGuiServer::GroundScrollX" )
				{
					$EnvGuiServer::GroundScrollX = mClampF (%value, -10, 10);
				}
				else if ( %label $= "$EnvGuiServer::GroundScrollY" )
				{
					$EnvGuiServer::GroundScrollY = mClampF (%value, -10, 10);
				}
				else if ( %label $= "$EnvGuiServer::WindVelocity" )
				{
					$EnvGuiServer::WindVelocity = %value;
				}
				else if ( %label $= "$EnvGuiServer::WindEffectPrecipitation" )
				{
					$EnvGuiServer::WindEffectPrecipitation = mClamp (%value, 0, 1);
				}
				else if ( %label $= "$EnvGuiServer::VignetteMultiply" )
				{
					$EnvGuiServer::VignetteMultiply = mClamp (%value, 0, 1);
				}
				else if ( %label $= "$EnvGuiServer::VignetteColor" )
				{
					$EnvGuiServer::VignetteColor = getColorF (%value);
				}

				if ( %label $= "$MiniGame::Enabled" )
				{
					$MiniGame::Enabled = mClamp (%value, 0, 1);
				}
				else if ( %label $= "$MiniGame::GameColor" )
				{
					$MiniGame::GameColor = %value;
				}
				else if ( %label $= "$MiniGame::InviteOnly" )
				{
					$MiniGame::InviteOnly = mClamp (%value, 0, 1);
				}
				else if ( %label $= "$MiniGame::IncludeAllPlayersBricks" )
				{
					$MiniGame::IncludeAllPlayersBricks = mClamp (%value, 0, 1);
				}
				else if ( %label $= "$MiniGame::PlayersUseOwnBricks" )
				{
					$MiniGame::PlayersUseOwnBricks = mClamp (%value, 0, 1);
				}
				else if ( %label $= "$MiniGame::Points_BreakBrick" )
				{
					$MiniGame::Points_BreakBrick = mClamp (%value, -9999.0, 9999);
				}
				else if ( %label $= "$MiniGame::Points_PlantBrick" )
				{
					$MiniGame::Points_PlantBrick = mClamp (%value, -9999.0, 9999);
				}
				else if ( %label $= "$MiniGame::Points_KillPlayer" )
				{
					$MiniGame::Points_KillPlayer = mClamp (%value, -9999.0, 9999);
				}
				else if ( %label $= "$MiniGame::Points_KillSelf" )
				{
					$MiniGame::Points_KillSelf = mClamp (%value, -9999.0, 9999);
				}
				else if ( %label $= "$MiniGame::Points_KillBot" )
				{
					$MiniGame::Points_KillBot = mClamp (%value, -9999.0, 9999);
				}
				else if ( %label $= "$MiniGame::Points_Die" )
				{
					$MiniGame::Points_Die = mClamp (%value, -9999.0, 9999);
				}
				else if ( %label $= "$MiniGame::RespawnTime" )
				{
					$MiniGame::RespawnTime = mClamp (%value, -1.0, 300) * 1000;
				}
				else if ( %label $= "$MiniGame::VehicleRespawnTime" )
				{
					$MiniGame::VehicleRespawnTime = mClamp (%value, -1.0, 300) * 1000;
				}
				else if ( %label $= "$MiniGame::BrickRespawnTime" )
				{
					$MiniGame::BrickRespawnTime = mClamp (%value, 0, 300) * 1000;
				}
				else if ( %label $= "$MiniGame::BotRespawnTime" )
				{
					$MiniGame::BotRespawnTime = mClamp (%value, -1.0, 300) * 1000;
				}
				else if ( %label $= "$MiniGame::UseSpawnBricks" )
				{
					$MiniGame::UseSpawnBricks = mClamp (%value, 0, 1);
				}
				else if ( %label $= "$MiniGame::FallingDamage" )
				{
					$MiniGame::FallingDamage = mClamp (%value, 0, 1);
				}
				else if ( %label $= "$MiniGame::WeaponDamage" )
				{
					$MiniGame::WeaponDamage = mClamp (%value, 0, 1);
				}
				else if ( %label $= "$MiniGame::SelfDamage" )
				{
					$MiniGame::SelfDamage = mClamp (%value, 0, 1);
				}
				else if ( %label $= "$MiniGame::VehicleDamage" )
				{
					$MiniGame::VehicleDamage = mClamp (%value, 0, 1);
				}
				else if ( %label $= "$MiniGame::BrickDamage" )
				{
					$MiniGame::BrickDamage = mClamp (%value, 0, 1);
				}
				else if ( %label $= "$MiniGame::BotDamage" )
				{
					$MiniGame::BotDamage = mClamp (%value, 0, 1);
				}
				else if ( %label $= "$MiniGame::EnableWand" )
				{
					$MiniGame::EnableWand = mClamp (%value, 0, 1);
				}
				else if ( %label $= "$MiniGame::EnableBuilding" )
				{
					$MiniGame::EnableBuilding = mClamp (%value, 0, 1);
				}
				else if ( %label $= "$MiniGame::EnablePainting" )
				{
					$MiniGame::EnablePainting = mClamp (%value, 0, 1);
				}
				else if ( %label $= "$MiniGame::PlayerDataBlockName" )
				{
					$MiniGame::PlayerDataBlockName = %value;
				}
				else if ( %label $= "$MiniGame::StartEquipName0" )
				{
					$MiniGame::StartEquipName0 = %value;
				}
				else if ( %label $= "$MiniGame::StartEquipName1" )
				{
					$MiniGame::StartEquipName1 = %value;
				}
				else if ( %label $= "$MiniGame::StartEquipName2" )
				{
					$MiniGame::StartEquipName2 = %value;
				}
				else if ( %label $= "$MiniGame::StartEquipName3" )
				{
					$MiniGame::StartEquipName3 = %value;
				}
				else if ( %label $= "$MiniGame::StartEquipName4" )
				{
					$MiniGame::StartEquipName4 = %value;
				}
				else if ( %label $= "$MiniGame::StartEquipName5" )
				{
					$MiniGame::StartEquipName5 = %value;
				}
				else if ( %label $= "$MiniGame::StartEquipName6" )
				{
					$MiniGame::StartEquipName6 = %value;
				}
				else if ( %label $= "$MiniGame::StartEquipName7" )
				{
					$MiniGame::StartEquipName7 = %value;
				}
				else if ( %label $= "$MiniGame::StartEquipName8" )
				{
					$MiniGame::StartEquipName8 = %value;
				}
				else if ( %label $= "$MiniGame::StartEquipName9" )
				{
					$MiniGame::StartEquipName9 = %value;
				}
				else if ( %label $= "$MiniGame::StartEquipName10" )
				{
					$MiniGame::StartEquipName10 = %value;
				}
				else if ( %label $= "$MiniGame::StartEquipName11" )
				{
					$MiniGame::StartEquipName11 = %value;
				}
				else if ( %label $= "$MiniGame::StartEquipName12" )
				{
					$MiniGame::StartEquipName12 = %value;
				}
				else if ( %label $= "$MiniGame::StartEquipName13" )
				{
					$MiniGame::StartEquipName13 = %value;
				}
				else if ( %label $= "$MiniGame::StartEquipName14" )
				{
					$MiniGame::StartEquipName14 = %value;
				}
				else if ( %label $= "$MiniGame::TimeLimit" )
				{
					$MiniGame::TimeLimit = mClamp (%value, 0, 3600);
				}

				if ( %label $= "$Server::BrickRespawnTime" )
				{
					$Server::BrickRespawnTime = mClamp (%value, 1000, 360000);
				}
				else if ( %label $= "$Server::ClearEventsOnClientExit" )
				{
					$Server::ClearEventsOnClientExit = mClamp (%value, 0, 1);
				}
				else if ( %label $= "$Server::MaxBricksPerSecond" )
				{
					$Server::MaxBricksPerSecond = mClamp (%value, 1, 1000);
				}
				else if ( %label $= "$Server::MaxPhysVehicles_Total" )
				{
					$Server::MaxPhysVehicles_Total = mClamp (%value, $Min::MaxPhysVehicles_Total, $Max::MaxPhysVehicles_Total);
				}
				else if ( %label $= "$Server::MaxPlayerVehicles_Total" )
				{
					$Server::MaxPlayerVehicles_Total = mClamp (%value, $Min::MaxPlayerVehicles_Total, $Max::MaxPlayerVehicles_Total);
				}
				else if ( %label $= "$Server::Quota::Environment" )
				{
					$Server::Quota::Environment = mClamp (%value, $Min::Quota::Environment, $Max::Quota::Environment);
				}
				else if ( %label $= "$Server::Quota::Item" )
				{
					$Server::Quota::Item = mClamp (%value, $Min::Quota::Item, $Max::Quota::Item);
				}
				else if ( %label $= "$Server::Quota::Misc" )
				{
					$Server::Quota::Misc = mClamp (%value, $Min::Quota::Misc, $Max::Quota::Misc);
				}
				else if ( %label $= "$Server::Quota::Player" )
				{
					$Server::Quota::Player = mClamp (%value, $Min::Quota::Player, $Max::Quota::Player);
				}
				else if ( %label $= "$Server::Quota::Projectile" )
				{
					$Server::Quota::Projectile = mClamp (%value, $Min::Quota::Projectile, $Max::Quota::Projectile);
				}
				else if ( %label $= "$Server::Quota::Schedules" )
				{
					$Server::Quota::Schedules = mClamp (%value, $Min::Quota::Schedules, $Max::Quota::Schedules);
				}
				else if ( %label $= "$Server::Quota::Vehicle" )
				{
					$Server::Quota::Vehicle = mClamp (%value, $Min::Quota::Vehicle, $Max::Quota::Vehicle);
				}
				else if ( %label $= "$Server::QuotaLAN::Environment" )
				{
					$Server::QuotaLAN::Environment = mClamp (%value, $Min::QuotaLAN::Environment, $Max::QuotaLAN::Environment);
				}
				else if ( %label $= "$Server::QuotaLAN::Item" )
				{
					$Server::QuotaLAN::Item = mClamp (%value, $Min::QuotaLAN::Item, $Max::QuotaLAN::Item);
				}
				else if ( %label $= "$Server::QuotaLAN::Misc" )
				{
					$Server::QuotaLAN::Misc = mClamp (%value, $Min::QuotaLAN::Misc, $Max::QuotaLAN::Misc);
				}
				else if ( %label $= "$Server::QuotaLAN::Player" )
				{
					$Server::QuotaLAN::Player = mClamp (%value, $Min::QuotaLAN::Player, $Max::QuotaLAN::Player);
				}
				else if ( %label $= "$Server::QuotaLAN::Projectile" )
				{
					$Server::QuotaLAN::Projectile = mClamp (%value, $Min::QuotaLAN::Projectile, $Max::QuotaLAN::Projectile);
				}
				else if ( %label $= "$Server::QuotaLAN::Schedules" )
				{
					$Server::QuotaLAN::Schedules = mClamp (%value, $Min::QuotaLAN::Schedules, $Max::QuotaLAN::Schedules);
				}
				else if ( %label $= "$Server::QuotaLAN::Vehicle" )
				{
					$Server::QuotaLAN::Vehicle = mClamp (%value, $Min::QuotaLAN::Vehicle, $Max::QuotaLAN::Vehicle);
				}
				else if ( %label $= "$Server::WelcomeMessage" )
				{
					$Server::WelcomeMessage = %value;
				}
				else if ( %label $= "$Server::WrenchEventsAdminOnly" )
				{
					$Server::WrenchEventsAdminOnly = mClamp (%value, 0, 1);
				}
				else if ( %label $= "$Server::GhostLimit" )
				{
					$Server::GhostLimit = mClamp (%value, $Min::GhostLimit, $Max::GhostLimit);
				}
				else if ( %label $= "$GameMode::BrickOwnership" )
				{
					$GameMode::BrickOwnership = %value;
				}
			}
		}
	}

	%file.close();
	%file.delete();

	return true;
}
