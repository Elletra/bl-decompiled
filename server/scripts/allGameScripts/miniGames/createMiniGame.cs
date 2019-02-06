function CreateMiniGameSO ( %client, %title, %colorIdx, %useSpawnBricks )
{
	if ( $MiniGameColorTaken[%colorIdx] )
	{
		error ("ERROR: CreateMiniGameSO() - Color index " @  %colorIdx  @ ", " @  
			$MiniGameColorName[%colorIdx]  @ ", is taken!");

		return 0;
	}

	$MiniGameColorTaken[%colorIdx] = 1;

	%mg = new ScriptObject()
	{
		class = MiniGameSO;

		owner = %client;
		title = %title;
		colorIdx = %colorIdx;

		numMembers = 0;

		InviteOnly = true;

		UseAllPlayersBricks = false;
		PlayersUseOwnBricks = false;
		UseSpawnBricks = %useSpawnBricks;

		Points_BreakBrick = 0;
		Points_PlantBrick = 0;
		Points_KillPlayer = 0;
		Points_KillBot = 1;
		Points_KillSelf = 0;
		Points_Die = 0;

		RespawnTime = 1;
		VehicleRespawnTime = 1;
		BrickRespawnTime = 1;
		BotRespawnTime = 5000;

		FallingDamage = true;
		WeaponDamage = false;
		SelfDamage = false;
		VehicleDamage = false;
		BrickDamage = false;
		BotDamage = true;

		EnableWand = false;
		EnableBuilding = false;

		PlayerDataBlock = PlayerStandardArmor.getId();

		StartEquip0 = 0;
		StartEquip1 = 0;
		StartEquip2 = 0;
		StartEquip3 = 0;
		StartEquip4 = 0;

		TimeLimit = 0;
	};
	MiniGameGroup.add (%mg);

	%mg.addMember (%client);
	%client.miniGame = %mg;

	return %mg;
}

function MiniGameSO::onAdd ( %obj )
{
	// Your code here
}


function serverCmdCreateMiniGame ( %client, %gameTitle, %gameColorIdx, %useSpawnBricks )
{
	%gameTitle = trim (%gameTitle);
	%gameColorIdx = mFloor (%gameColorIdx);

	if ( isObject(%client.miniGame)  &&  %client.miniGame == $DefaultMiniGame )
	{
		if ( !%client.isAdmin )
		{
			commandToClient (%client, 'messageBoxOK', "Minigame", "You can\'t leave the default minigame");
			return;
		}
	}

	if ( isObject(%client.miniGame) )
	{
		if ( %client.miniGame.owner == %client )
		{
			commandToClient (%client, 'CreateMiniGameFail', "You\'re already running a minigame.");
			return;
		}
	}

	if ( %gameTitle $= "" )
	{
		commandToClient (%client, 'CreateMiniGameFail', "Invalid game title.");
		return;
	}

	if ( %gameColorIdx < 0  ||  %gameColorIdx >= $MiniGameColorCount )
	{
		commandToClient (%client, 'CreateMiniGameFail', "Bad color index.");
		return;
	}

	if ( $MiniGameColorTaken[%gameColorIdx] == 1 )
	{
		commandToClient (%client, 'CreateMiniGameFail', "Game color is taken.");
		return;
	}

	if ( isObject(%client.miniGame) )
	{
		%client.miniGame.removeMember (%client);
	}

	messageClient (%client, '', '\c5Mini-game created.');
	CreateMiniGameSO (%client, %gameTitle, %gameColorIdx, %useSpawnBricks);

	commandToClient (%client, 'CreateMiniGameSuccess');
	commandToClient (%client, 'SetPlayingMiniGame', 1);
	commandToClient (%client, 'SetRunningMiniGame', 1);

	commandToAll ('AddMiniGameLine', %client.miniGame.getLine(), %client.miniGame, %client.miniGame.colorIdx);

	%client.miniGame.schedule (100, Reset, %client);
}
