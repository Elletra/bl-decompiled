$Game::EndGameScore = 0;
$Game::EndGamePause = 10;
function makePadString(%char, %num)
{
	for (%i = 0; %i < %num; %i++)
	{
		%ret = %ret @ %char;
	}
}

function onNeedRelight()
{
}

function onServerCreated()
{
	$Game::StartTime = 0;
	exec("./DamageTypes.cs");
	initDefaultDamageTypes();
	if (1)
	{
		exec("./allGameScripts.cs");
	}
	else
	{
		exec("./simGroup.cs");
		exec("./queue.cs");
		exec("./serverLoadBricks.cs");
		exec("./constants.cs");
		exec("./kickBan.cs");
		exec("./serverCmd.cs");
		exec("./serverCmdPlay.cs");
		exec("./icons.cs");
		exec("./audioProfiles.cs");
		exec("./camera.cs");
		exec("./markers.cs");
		exec("./triggers.cs");
		exec("./inventory.cs");
		exec("./shapeBase.cs");
		exec("./item.cs");
		exec("./staticShape.cs");
		exec("./explosion.cs");
		exec("./weapon.cs");
		exec("./radiusDamage.cs");
		exec("./projectile.cs");
		exec("./player.cs");
		exec("./aiPlayer.cs");
		exec("./playerDeath.cs");
		exec("./playerSpawn.cs");
		exec("./hammer.cs");
		exec("./wrench.cs");
		exec("./wand.cs");
		exec("./sprayCan.cs");
		exec("./AdminWand.cs");
		exec("./FXCans/flatCan.cs");
		exec("./FXCans/pearlCan.cs");
		exec("./FXCans/chromeCan.cs");
		exec("./FXCans/glowCan.cs");
		exec("./FXCans/blinkCan.cs");
		exec("./FXCans/swirlCan.cs");
		exec("./FXCans/rainbowCan.cs");
		exec("./FXCans/stableCan.cs");
		exec("./FXCans/jelloCan.cs");
		exec("./brickWeapon.cs");
		exec("./bricks.cs");
		exec("./fxDTSBrick.cs");
		exec("./vehicle.cs");
		exec("./vehicleSpawnMarker.cs");
		exec("./precipitation.cs");
		exec("./particles.cs");
		exec("./clock.cs");
		exec("./consoleCmd.cs");
		exec("./lights.cs");
		exec("./effects.cs");
		exec("./emote.cs");
		exec("./music.cs");
		exec("./brickSpawnPoint.cs");
		exec("./BrickMan.cs");
		exec("./TrustList.cs");
		exec("./TrustListCheck.cs");
		exec("./MiniGame.cs");
		exec("./miniGameCheck.cs");
		exec("./interactionCheck.cs");
		exec("./AddOns.cs");
	}
	loadAddOns();
	$Game::StartTime = $Sim::Time;
	serverLoadAvatarShapeMenu("packs");
	serverLoadAvatarShapeMenu("hats");
	serverLoadAvatarShapeMenu("accents");
	setSprayCanColors();
	createMusicDatablocks();
	exec("./prints.cs");
	loadPrintedBrickTextures();
	serverLoadAvatarNames();
	serverLoadMinifigColors();
	if ($Server::Dedicated && !$Server::LAN)
	{
		exec("base/server/authComServer.cs");
		auth_Init();
	}
	CreateBanManager();
	InitMinigameColors();
}

function onServerDestroyed()
{
}

function onMissionLoaded()
{
	startGame();
	$Server::MissionName = MissionInfo.name;
	$Server::BrickCount = 0;
	if ($Pref::Server::BrickLimit > 128000)
	{
		$Pref::Server::BrickLimit = 128000;
	}
	if ($Pref::Server::BrickLimit <= 0)
	{
		$Pref::Server::BrickLimit = 128000;
	}
	$IamAdmin = 1;
	WebCom_PostServer();
}

function onMissionEnded()
{
	cancel($Game::Schedule);
	$Game::Running = 0;
	$Game::Cycling = 0;
}

function startGame()
{
	if ($Game::Running)
	{
		error("startGame: End the game first!");
		return;
	}
	for (%clientIndex = 0; %clientIndex < ClientGroup.getCount(); %clientIndex++)
	{
		%cl = ClientGroup.getObject(%clientIndex);
		commandToClient(%cl, 'GameStart');
		%cl.score = 0;
	}
	if ($Pref::Server::TimeLimit)
	{
		$Game::Schedule = schedule($Pref::Server::TimeLimit * 1000, 0, "onGameDurationEnd");
	}
	$Game::Running = 1;
}

function endGame()
{
	if (!$Game::Running)
	{
		error("endGame: No game running!");
		return;
	}
	EndAllMiniGames();
	$timeScale = 1;
	for (%clientIndex = 0; %clientIndex < ClientGroup.getCount(); %clientIndex++)
	{
		%cl = ClientGroup.getObject(%clientIndex);
		commandToClient(%cl, 'TimeScale', $timeScale);
	}
	cancel($Game::Schedule);
	for (%clientIndex = 0; %clientIndex < ClientGroup.getCount(); %clientIndex++)
	{
		%cl = ClientGroup.getObject(%clientIndex);
		commandToClient(%cl, 'GameEnd');
	}
	resetMission();
	$Game::Running = 0;
}

function onGameDurationEnd()
{
	if ($Game::Duration && !isObject(EditorGui))
	{
		cycleGame();
	}
}

function cycleGame()
{
	if (!$Game::Cycling)
	{
		$Game::Cycling = 1;
		$Game::Schedule = schedule(0, 0, "onCycleExec");
	}
}

function onCycleExec()
{
	endGame();
	$Game::Schedule = schedule($Game::EndGamePause * 1000, 0, "onCyclePauseEnd");
}

function onCyclePauseEnd()
{
	$Game::Cycling = 0;
	%search = $Server::MissionFileSpec;
	for (%file = findFirstFile(%search); %file !$= ""; %file = findNextFile(%search))
	{
		if (%file $= $Server::MissionFile)
		{
			%file = findNextFile(%search);
			if (%file $= "")
			{
				%file = findFirstFile(%search);
			}
			break;
		}
	}
	loadMission(%file);
}

function GameConnection::onClientEnterGame(%this)
{
	%this.undoStack = New_QueueSO(20);
	commandToClient(%this, 'SetBuildingDisabled', 0);
	commandToClient(%this, 'SetPaintingDisabled', 0);
	commandToClient(%this, 'SetPlayingMiniGame', 0);
	commandToClient(%this, 'SetRunningMiniGame', 0);
	commandToClient(%this, 'SetRemoteServerData', $Server::LAN);
	commandToClient(%this, 'TimeScale', $timeScale);
	%this.Camera = new Camera()
	{
		dataBlock = Observer;
	};
	MissionCleanup.add(%this.Camera);
	%this.Camera.scopeToClient(%this);
	%this.score = 0;
	commandToClient(%this, 'clearMapList');
	%this.bpsCount = 0;
	%this.bpsTime = %currTime;
	sendLetterPrintInfo(%this);
	commandToClient(%this, 'PSD_KillPrints');
	commandToClient(%this, 'PlayGui_LoadPaint');
	%this.spawnPlayer();
}

function GameConnection::setCanRespawn(%this, %val)
{
	%this.canRespawn = %val;
}

function GameConnection::onClientLeaveGame(%this)
{
	%client = %this;
	serverCmdStopTalking(%this);
	%mg = %this.miniGame;
	if (isObject(%mg))
	{
		%mg.removeMember(%this);
	}
	if (isObject(%this.light))
	{
		%this.light.delete();
	}
	if (isObject(%this.undoStack))
	{
		%this.undoStack.delete();
	}
	%player = %this.Player;
	if (isObject(%player))
	{
		if (isObject(%player.tempBrick))
		{
			%player.tempBrick.delete();
		}
	}
	if (isObject(%this.tcpObj))
	{
		%this.tcpObj.delete();
	}
	if (isObject(%this.Camera))
	{
		%this.Camera.delete();
	}
	if (isObject(%this.Player))
	{
		%this.Player.delete();
	}
	if (isObject(%this.miniGame))
	{
		%this.miniGame.removeMember(%this);
	}
	if (isObject(%this.brickGroup))
	{
		if ($Server::LAN)
		{
			if (%this.bl_id !$= "LAN")
			{
				error("ERROR: GameConnection::onClientLeaveGame() - Client \"" @ %this.name @ "\" has invalid LAN bl_id (" @ %this.bl_id @ ").");
				%this.brickGroup.delete();
			}
		}
		else if (%this.bl_id $= "" || %this.bl_id == -1)
		{
			%this.brickGroup.delete();
		}
		else
		{
			%ourBrickGroup = %this.brickGroup;
			%count = ClientGroup.getCount();
			for (%i = 0; %i < %count; %i++)
			{
				%cl = ClientGroup.getObject(%i);
				if (%cl != %this)
				{
					if (%cl.brickGroup == %ourBrickGroup)
					{
						return;
					}
				}
			}
			if (%this.brickGroup.getCount() <= 0)
			{
				%this.brickGroup.delete();
			}
		}
	}
	else if ($missionRunning)
	{
		error("ERROR: GameConnection::onClientLeaveGame() - Client \"" @ %client.name @ "\" has no brick group.");
	}
}

function GameConnection::onLeaveMissionArea(%this)
{
}

function GameConnection::onEnterMissionArea(%this)
{
}

function GameConnection::onDeath(%this, %sourceObject, %sourceClient, %damageType, %__unused)
{
	%player = %this.Player;
	%player.setShapeName("");
	if (isObject(%player.tempBrick))
	{
		%player.tempBrick.delete();
		%player.tempBrick = "";
	}
	if (isObject(%this.Camera) && isObject(%this.Player))
	{
		%this.Camera.setMode("Corpse", %this.Player);
		%this.setControlObject(%this.Camera);
	}
	%this.Player = 0;
	if ($Damage::Direct[%damageType] != 1)
	{
		if (getSimTime() - %player.lastDirectDamageTime < 100)
		{
			if (%player.lastDirectDamageType !$= "")
			{
				%damageType = %player.lastDirectDamageType;
			}
		}
	}
	if (%damageType == $DamageType::Impact)
	{
		if (isObject(%player.lastPusher))
		{
			if (getSimTime() - %player.lastPushTime <= 1000)
			{
				%sourceClient = %player.lastPusher;
			}
		}
	}
	%message = "%2 killed %1";
	if (%sourceClient == %this || %sourceClient == 0)
	{
		%message = $DeathMessage_Suicide[%damageType];
	}
	else
	{
		%message = $DeathMessage_Murder[%damageType];
	}
	if ($Damage::Direct[%damageType] == 1)
	{
		if (%sourceClient && isObject(%sourceClient.Player))
		{
			%playerVelocity = ((VectorLen(VectorSub(%player.preHitVelocity, %sourceClient.Player.getVelocity())) / 2.64) * 6 * 3600) / 5280;
		}
		else
		{
			%playerVelocity = ((VectorLen(%player.preHitVelocity) / 2.64) * 6 * 3600) / 5280;
		}
		%playerPos = %player.getPosition();
		%res0 = containerRayCast(VectorAdd(%playerPos, "0 0 2"), VectorAdd(%playerPos, "0 0  -6.8"), $TypeMasks::InteriorObjectType | $TypeMasks::FxBrickObjectType);
		%res1 = containerRayCast(VectorAdd(%playerPos, "0 0 2"), VectorAdd(%playerPos, "0 -1 -6.8"), $TypeMasks::InteriorObjectType | $TypeMasks::FxBrickObjectType);
		%res2 = containerRayCast(VectorAdd(%playerPos, "0 0 2"), VectorAdd(%playerPos, "1 1  -6.8"), $TypeMasks::InteriorObjectType | $TypeMasks::FxBrickObjectType);
		%res3 = containerRayCast(VectorAdd(%playerPos, "0 0 2"), VectorAdd(%playerPos, "-1 1 -6.8"), $TypeMasks::InteriorObjectType | $TypeMasks::FxBrickObjectType);
		if (!isObject(getWord(%res0, 0)) && !isObject(getWord(%res1, 0)) && !isObject(getWord(%res2, 0)) && !isObject(getWord(%res3, 0)))
		{
			%range = round((VectorLen(VectorSub(%playerPos, %sourceObject.originPoint)) / 2.65) * 6);
			if (isObject(%sourceClient.Player))
			{
				%sourceClient.Player.emote(winStarProjectile, 1);
			}
			%sourceClient.play2D(rewardSound);
			commandToClient(%sourceClient, 'BottomPrint', "<bitmap:add-ons/ci/star>\c3 MID AIR KILL - " @ %this.name @ " " @ round(%playerVelocity) @ "MPH, " @ %range @ "ft!", 3);
			commandToClient(%this, 'BottomPrint', "\c5 MID AIR'd by " @ %sourceClient.name @ " - " @ round(%playerVelocity) @ "MPH, " @ %range @ "ft!", 3);
		}
	}
	if (isObject(%this.miniGame))
	{
		if (%sourceClient == %this)
		{
			%this.incScore(%this.miniGame.Points_KillSelf);
		}
		else if (%sourceClient == 0)
		{
			%this.incScore(%this.miniGame.Points_Die);
		}
		else
		{
			%sourceClient.incScore(%this.miniGame.Points_KillPlayer);
			%this.incScore(%this.miniGame.Points_Die);
		}
	}
	if (isObject(%this.miniGame))
	{
		%this.miniGame.messageAllExcept(%this, 'MsgClientKilled', %message, %this.name, %sourceClient.name);
		messageClient(%this, 'MsgYourDeath', %message, %this.name, %sourceClient.name, %this.miniGame.respawnTime);
	}
	else
	{
		messageAllExcept(%this, -1, 'MsgClientKilled', %message, %this.name, %sourceClient.name);
		messageClient(%this, 'MsgYourDeath', %message, %this.name, %sourceClient.name, $Game::MinRespawnTime);
	}
	return;
	if (%sourceClient == %this || %sourceClient == 0)
	{
		if (isObject(%this.miniGame))
		{
			%this.miniGame.messageAllExcept(%this, 'MsgClientKilled', %message, %this.name, %sourceClient.name);
			messageClient(%this, 'MsgYourDeath', %message, %this.name, %sourceClient.name, %this.miniGame.respawnTime);
		}
		else
		{
			messageAllExcept(%this, -1, 'MsgClientKilled', %message, %this.name, %sourceClient.name);
			messageClient(%this, 'MsgYourDeath', %message, %this.name, %sourceClient.name, $Game::MinRespawnTime);
		}
		if (isObject(%this.miniGame))
		{
			%this.incScore(%this.miniGame.Points_KillSelf);
		}
	}
	else if (isObject(%sourceClient))
	{
		if (isObject(%this.miniGame))
		{
			%this.miniGame.messageAllExcept(%this, 'MsgClientKilled', '%1 was killed by %2!', %this.name, %sourceClient.name);
			messageClient(%this, 'MsgYourDeath', '%1 was killed by %2!', %this.name, %sourceClient.name, %this.miniGame.respawnTime);
		}
		else
		{
			messageAllExcept(%this, -1, 'MsgClientKilled', '%1 was killed by %2!', %this.name, %sourceClient.name);
			messageClient(%this, 'MsgYourDeath', '%1 was killed by %2!', %this.name, %sourceClient.name, $Game::MinRespawnTime);
		}
		if (isObject(%this.miniGame))
		{
			%this.incScore(%this.miniGame.Points_Die);
		}
		if (isObject(%sourceClient.miniGame))
		{
			%sourceClient.incScore(%sourceClient.miniGame.Points_KillPlayer);
		}
	}
	else
	{
		if (isObject(%this.miniGame))
		{
			%this.incScore(%this.miniGame.Points_Die);
		}
		if (isObject(%this.miniGame))
		{
			%this.miniGame.messageAllExcept(%this, 'MsgClientKilled', '%1 dies.', %this.name);
			messageClient(%this, 'MsgYourDeath', '%1 dies.', %this.name, "", %this.miniGame.respawnTime);
		}
		else
		{
			messageAllExcept(%this, -1, 'MsgClientKilled', '%1 dies.', %this.name);
			messageClient(%this, 'MsgYourDeath', '%1 dies.', %this.name, "", $Game::MinRespawnTime);
		}
	}
}

function GameConnection::InstantRespawn(%client)
{
	%player = %client.Player;
	if (isObject(%player))
	{
		if (isObject(%player.tempBrick))
		{
			%player.tempBrick.delete();
		}
		%player.delete();
	}
	if (isObject(%client.light))
	{
		%client.light.delete();
	}
	%client.spawnPlayer();
}

function GameConnection::spawnPlayer(%client)
{
	if (isObject(%client.Player))
	{
		if (%client.Player.getDamagePercent() < 1)
		{
			%client.Player.delete();
		}
	}
	if (isObject(%client.miniGame))
	{
		%spawnPoint = %client.miniGame.pickSpawnPoint(%client);
	}
	else
	{
		%spawnPoint = pickSpawnPoint();
	}
	%client.createPlayer(%spawnPoint);
	if (isObject(%client.Camera))
	{
		%client.Camera.unmountImage(0);
	}
	messageClient(%client, 'MsgYourSpawn');
	if (!%client.hasSpawnedOnce)
	{
		%client.hasSpawnedOnce = 1;
		messageAllExcept(%client, -1, '', '\c1%1 spawned.', %client.name);
	}
}

function GameConnection::createPlayer(%this, %spawnPoint)
{
	if (%this.Player > 0)
	{
		error("Attempting to create an angus ghost!");
	}
	if (isObject(%this.miniGame))
	{
		if (!%this.miniGame.ending)
		{
			%data = %this.miniGame.playerDataBlock;
		}
		else
		{
			%data = PlayerStandardArmor;
		}
	}
	else
	{
		%data = PlayerStandardArmor;
	}
	%player = new Player()
	{
		dataBlock = %data;
		client = %this;
	};
	MissionCleanup.add(%player);
	%this.Player = %player;
	%player.weaponCount = 0;
	commandToClient(%this, 'ShowEnergyBar', %data.showEnergyBar);
	applyCharacterPrefs(%this);
	messageClient(%this, 'MsgClearInv');
	commandToClient(%this, 'PlayGui_CreateToolHud', %player.getDataBlock().maxTools);
	%client = %this;
	%mg = %client.miniGame;
	if (isObject(%mg))
	{
		if (!%mg.ending)
		{
			%player.setShapeNameColor($MiniGameColorF[%mg.colorIdx]);
			for (%i = 0; %i < 5; %i++)
			{
				if (isObject(%mg.startEquip[%i]))
				{
					%player.tool[%i] = %mg.startEquip[%i];
					messageClient(%this, 'MsgItemPickup', "", %i, %player.tool[%i], 1);
				}
			}
		}
		else
		{
			%player.setShapeNameColor("1 1 1");
			%player.GiveDefaultEquipment(1);
		}
	}
	else
	{
		%player.setShapeNameColor("1 1 1");
		%player.GiveDefaultEquipment(1);
	}
	%player.currWeaponSlot = -1;
	%player.setTransform(%spawnPoint);
	%player.setEnergyLevel(%player.getDataBlock().maxEnergy);
	%player.setShapeName(%this.name);
	%player.canDismount = 1;
	%this.Camera.setTransform(%player.getEyeTransform());
	%this.Player = %player;
	%this.setControlObject(%player);
	%p = new Projectile()
	{
		dataBlock = spawnProjectile;
		initialVelocity = "0 0 0";
		initialPosition = %player.getHackPosition();
		sourceObject = %player;
		sourceSlot = 0;
		client = %this;
	};
	MissionCleanup.add(%p);
	%p.setTransform(%player.getWorldBoxCenter());
}

function pickSpawnPoint()
{
	%groupName = "MissionGroup/PlayerDropPoints";
	%group = nameToID(%groupName);
	if (%group != -1)
	{
		%count = %group.getCount();
		if (%count != 0)
		{
			%index = getRandom(%count - 1);
			%spawn = %group.getObject(%index);
			%rayHeight = %spawn.RayHeight;
			if (%rayHeight $= "")
			{
				%rayHeight = 10;
			}
			%trans = %spawn.getTransform();
			%transX = getWord(%trans, 0);
			%transY = getWord(%trans, 1);
			%transZ = getWord(%trans, 2);
			for (%i = 0; %i < 1000; %i++)
			{
				%r = getRandom(%spawn.radius * 10) / 10;
				%ang = getRandom($pi * 2 * 100) / 100;
				%transX = getWord(%trans, 0);
				%transY = getWord(%trans, 1);
				%transZ = getWord(%trans, 2);
				%transX += %r * mCos(%ang);
				%transY += %r * mSin(%ang);
				%start = %transX SPC %transY SPC %transZ + %rayHeight;
				%end = %transX SPC %transY SPC %transZ - 2;
				%mask = $TypeMasks::FxBrickObjectType | $TypeMasks::InteriorObjectType | $TypeMasks::StaticShapeObjectType | $TypeMasks::PlayerObjectType;
				%scanTarg = containerRayCast(%start, %end, %mask, 0);
				if (%scanTarg)
				{
					%scanPos = posFromRaycast(%scanTarg);
					%transZ = getWord(%scanPos, 2);
					%boxCenter = VectorAdd(%scanPos, "0 0 1.6");
					%mask = $TypeMasks::FxBrickObjectType | $TypeMasks::StaticShapeObjectType | $TypeMasks::PlayerObjectType;
					if (containerBoxClear(%mask, %boxCenter, 0.6, 0.6, 1.3))
					{
						break;
					}
				}
			}
			%spawnAngle = getRandom($pi * 2 * 100) / 100;
			%returnTrans = %transX @ " " @ %transY @ " " @ %transZ @ " 0 0 1 " @ %spawnAngle;
			return %returnTrans;
		}
		else
		{
			error("No spawn points found in " @ %groupName);
		}
	}
	else
	{
		error("Missing spawn points group " @ %groupName);
	}
	error("default spawn!");
	return "0 0 300 1 0 0 0";
}

function GameConnection::Cheat(%this)
{
	%name = %this.name;
	%this.Cheat++;
	if (%this.Cheat > 10)
	{
		%this.delete("");
	}
}

function findLocalClient()
{
	%count = ClientGroup.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		%client = ClientGroup.getObject(%i);
		if (%client.isLocal())
		{
			return %client;
		}
	}
	return 0;
}

function findClientByBL_ID(%bl_id)
{
	%count = ClientGroup.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		%client = ClientGroup.getObject(%i);
		if (%client.bl_id == %bl_id)
		{
			return %client;
		}
	}
	return 0;
}

