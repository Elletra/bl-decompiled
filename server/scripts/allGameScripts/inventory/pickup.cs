$LastError::Trust = 1;
$LastError::MiniGameDifferent = 2;
$LastError::MiniGameNotYours = 3;
$LastError::NotInMiniGame = 4;


function ShapeBase::pickup ( %this, %obj, %amount )
{
	%data = %obj.getDataBlock();
	return %data.onPickup (%obj, %this, %amount);
}

function ShapeBaseData::onPickup ( %this, %obj, %user, %amount )
{
	return 0;
}


function ItemData::onPickup ( %this, %obj, %user, %amount )
{
	if ( !%obj.canPickup )
	{
		return;
	}

	%player = %user;
	%client = %player.client;
	%data = %player.getDataBlock();

	if ( !isObject(%client) )
	{
		return;
	}


	%mg = %client.miniGame;

	if ( isObject(%mg) )
	{
		if ( %mg.WeaponDamage == 1  &&  getSimTime() - %client.lastF8Time < 5000 )
		{
			return;
		}
	}


	%canUse = true;

	if ( miniGameCanUse(%player, %obj) == 1 )
	{
		%canUse = true;
	}

	if ( miniGameCanUse(%player, %obj) == 0 )
	{
		%canUse = false;
	}


	if ( !%canUse )
	{
		if ( isObject(%obj.spawnBrick) )
		{
			%ownerName = %obj.spawnBrick.getGroup().name;
		}

		%msg = %ownerName  @ " does not trust you enough to use this item.";

		if ( $lastError == $LastError::Trust )
		{
			%msg = %ownerName  @ " does not trust you enough to use this item.";
		}
		else if ( $lastError == $LastError::MiniGameDifferent )
		{
			if ( isObject(%client.miniGame) )
			{
				%msg = "This item is not part of the mini-game.";
			}
			else
			{
				%msg = "This item is part of a mini-game.";
			}
		}
		else if ( $lastError == $LastError::MiniGameNotYours )
		{
			%msg = "You do not own this item.";
		}
		else if ( $lastError == $LastError::NotInMiniGame )
		{
			%msg = "This item is not part of the mini-game.";
		}

		commandToClient(%client, 'centerPrint', %msg, 1);
		return;
	}

	%freeslot = -1;

	for ( %i = 0;  %i < %data.maxTools;  %i++ )
	{
		if ( %player.tool[%i] == 0 )
		{
			%freeslot = %i;
			break;
		}
	}

	if ( %freeslot != -1 )
	{
		if ( %obj.isStatic() )
		{
			%obj.respawn();
		}
		else
		{
			%obj.delete();
		}


		%player.tool[%freeslot] = %this;

		if ( %user.client )
		{
			messageClient ( %user.client, 'MsgItemPickup', '', %freeslot, %this.getId() );
		}

		return 1;
	}
}

function Weapon::onPickup ( %this, %obj, %shape, %amount )
{
	ItemData::onPickup (%this, %obj, %shape, %amount);
	return;

	// TODO: remove all this shit below the above return

	if ( !%obj.canPickup )
	{
		return;
	}

	%player = %shape;
	%client = %player.client;

	%data = %player.getDataBlock();

	if ( !isObject(%client) )
	{
		return;
	}

	%mg = %client.miniGame;

	if ( isObject(%mg) )
	{
		if ( %mg.WeaponDamage == 1  &&  getSimTime() - %client.lastF8Time < 5000.0)
		{
			return;
		}
	}


	%canUse = 1;

	if ( miniGameCanUse(%player, %obj) == 1 )
	{
		%canUse = 1;
	}

	if ( miniGameCanUse(%player, %obj) == 0 )
	{
		%canUse = 0;
	}

	if ( !%canUse )
	{
		if ( isObject(%obj.spawnBrick) )
		{
			%ownerName = %obj.spawnBrick.getGroup().name;
		}

		%msg = %ownerName  @ " does not trust you enough to use this item.";

		if ( $lastError == $LastError::Trust )
		{
			%msg = %ownerName  @ " does not trust you enough to use this item.";
		}
		else if ( $lastError == $LastError::MiniGameDifferent )
		{
			if ( isObject(%client.miniGame) )
			{
				%msg = "This item is not part of the mini-game.";
			}
			else
			{
				%msg = "This item is part of a mini-game.";
			}
		}
		else if ( $lastError == $LastError::MiniGameNotYours )
		{
			%msg = "You do not own this item.";
		}
		else if ( $lastError == $LastError::NotInMiniGame )
		{
			%msg = "This item is not part of the mini-game.";
		}

		commandToClient (%client, 'centerPrint', %msg, 1);
		return;
	}

	if ( %player.weaponCount < %data.maxWeapons )
	{
		%freeslot = -1;

		for ( %i = 0;  %i < %data.maxTools;  %i++ )
		{
			if ( %player.tool[%i] == 0 )
			{
				%freeslot = %i;
			}
		}

		if (%freeslot != -1)
		{
			if ( %obj.isStatic() )
			{
				%obj.Respawn();
			}
			else
			{
				%obj.delete();
			}

			%player.weaponCount++;
			%player.tool[%freeslot] = %this;

			if ( %player.client )
			{
				messageClient ( %player.client, 'MsgItemPickup', '', %freeslot, %this.getId() );
			}

			return 1;
		}
	}
	else if ( %user.client )
	{
		messageClient (%user.client, 'MsgItemFailPickup', 'You already have a weapon!');
	}
}
