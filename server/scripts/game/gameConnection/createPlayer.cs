function GameConnection::createPlayer ( %client, %spawnPoint )
{
	if ( %client.Player > 0 )
	{
		error ("Attempting to create an angus ghost!");
	}

	if ( isObject(%client.miniGame) )
	{
		if ( !%client.miniGame.ending )
		{
			%data = %client.miniGame.PlayerDataBlock;
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


	%oldQuotaObject = getCurrentQuotaObject();

	if ( isObject(%oldQuotaObject) )
	{
		clearCurrentQuotaObject();
	}

	%player = new Player ()
	{
		dataBlock = %data;
		client = %client;
	};

	if ( !isObject(%player) )
	{
		error ("ERROR: GameConnection::createPlayer(" @  %client  @ ", " @  %spawnPoint  @ 
			") - failed to create player with datablock " @  %data);

		return;
	}

	MissionCleanup.add(%player);

	%client.player = %player;
	%player.weaponCount = 0;
	%player.spawnTime = getSimTime();

	if ( isObject(%oldQuotaObject) )
	{
		setCurrentQuotaObject (%oldQuotaObject);
	}

	commandToClient (%client, 'ShowEnergyBar', %data.showEnergyBar);
	applyCharacterPrefs (%client);
	commandToClient (%client, 'PlayGui_CreateToolHud', %player.getDataBlock().maxTools);


	%client = %client;  // good coding
	%mg = %client.miniGame;

	if ( isObject(%mg) )
	{
		if ( !%mg.ending )
		{
			%player.setShapeNameColor ( $MiniGameColorF[%mg.colorIdx] );

			// FIXME: make this not hard-coded

			for ( %i = 0;  %i < 5;  %i++ )
			{
				if ( isObject ( %mg.startEquip[%i] ) )
				{
					%player.tool[%i] = %mg.startEquip[%i];
				}
				else
				{
					%player.tool[%i] = 0;
				}

				messageClient (%client, 'MsgItemPickup', "", %i, %player.tool[%i], 1);
			}
		}
		else
		{
			%player.setShapeNameColor ("1 1 1");
			%player.GiveDefaultEquipment (1);
		}
	}
	else
	{
		%player.setShapeNameColor ("1 1 1");
		%player.GiveDefaultEquipment (1);
	}

	%player.currWeaponSlot = -1;

	%player.setTransform (%spawnPoint);
	%player.setEnergyLevel (%player.getDataBlock().maxEnergy);
	%player.setShapeName (%client.getPlayerName(), 8564862);

	%player.canDismount = 1;

	if ( isObject(%client.Camera) )
	{
		%client.Camera.setTransform ( %player.getEyeTransform() );
	}

	%client.Player = %player;
	%client.setControlObject (%player);


	%p = new Projectile ()
	{
		dataBlock       = spawnProjectile;
		initialVelocity = "0 0 0";
		initialPosition = %player.getHackPosition();
		sourceObject    = %player;
		sourceSlot      = 0;
		client          = %client;
	};

	if ( isObject(%p) )
	{
		%p.setScale ( %player.getScale() );
		MissionCleanup.add (%p);
	}
}
