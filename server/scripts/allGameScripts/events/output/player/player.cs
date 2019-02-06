exec ("./burn.cs");
exec ("./changeDataBlock.cs");
exec ("./health.cs");
exec ("./spawn.cs");


function GameConnection::instantRespawn ( %client, %clientagain )
{
	%player = %client.player;

	if ( isObject(%player) )
	{
		if ( isObject(%player.tempBrick) )
		{
			%player.tempBrick.delete();
			%player.tempBrick = 0;
		}

		%player.delete();
	}

	if ( isObject(%client.light) )
	{
		%client.light.delete();
	}

	%client.spawnPlayer();
}

function Player::instantRespawn ( %player )
{
	%client = %player.client;

	if ( !isObject(%client) )
	{
		return;
	}

	%client.instantRespawn();
}

function Player::clearTools ( %player )
{
	%client = %player.client;
	%maxTools = %player.getDataBlock().maxTools;

	// Ah yes, `ClearTools` uses a for loop that's based on maxTools, while
	// `GiveDefaultEquipment` and the like are hardcoded at 5, which led to
	// years of weird bugs for playertypes with more than 5 maxTools

	// Great coding

	for ( %i = 0;  %i < %maxTools;  %i++ )
	{
		%player.tool[%i] = 0;

		if ( isObject(%client) )
		{
			messageClient (%client, 'MsgItemPickup', "", %i, 0, 1);
		}
	}

	%player.unmountImage (0);
}

function Player::dismount ( %player, %client )
{
	%player.getDataBlock().doDismount (%player, 1);
}

function Player::setPlayerScale ( %player, %val )
{
	%player.setScale (%val SPC %val SPC %val);
}

function Player::addVelocity ( %player, %vector )
{
	%vel = %player.getVelocity();
	%vel = VectorAdd (%vel, %vector);
	%player.setVelocity (%vel);
}


// =================
//  Register Events
// =================

registerOutputEvent ("Player", "Kill", "");

registerOutputEvent ("Player", "SetVelocity", "vector 200", 0);
registerOutputEvent ("Player", "AddVelocity", "vector 200");

registerOutputEvent ("Player", "SetPlayerScale", "float 0.2 2 0.1 1");

registerOutputEvent ("Player", "Dismount", "");

registerOutputEvent ("Player", "ClearTools", "");
registerOutputEvent ("Player", "InstantRespawn", "");


// Bot

registerOutputEvent ("Bot", "Kill", "");

registerOutputEvent ("Bot", "SetVelocity", "vector 200", 0);
registerOutputEvent ("Bot", "AddVelocity", "vector 200");

registerOutputEvent ("Bot", "SetPlayerScale", "float 0.2 2 0.1 1");

registerOutputEvent ("Bot", "Dismount", "");

registerOutputEvent ("Bot", "ClearTools", "");
registerOutputEvent ("Bot", "InstantRespawn", "");
