function serverCmdUseInventory ( %client, %slot )
{
	%mg = %client.miniGame;

	if ( isObject(%mg) )
	{
		if ( !%mg.EnableBuilding )
		{
			return;
		}
	}

	if ( %client.isTalking )
	{
		serverCmdStopTalking (%client);
	}


	%player = %client.Player;

	if ( %client.inventory[%slot] )
	{
		%item = %client.inventory[%slot].getId();
	}
	else
	{
		%item = 0;
	}

	if ( %item )
	{
		%item.onUse (%player, %slot);
		%client.currInvSlot = %slot;
	}
}

function serverCmdInstantUseBrick ( %client, %data )
{
	%mg = %client.miniGame;

	if ( isObject(%mg) )
	{
		if ( !%mg.EnableBuilding )
		{
			return;
		}
	}


	if ( isObject(%data) )
	{
		if ( %data.getClassName() $= "fxDTSBrickData" )
		{
			%data.onUse (%client.Player, -1);
			%client.currInv = -1;
			%client.currInvSlot = -1;
		}
		else
		{
			messageClient (%client, '', 'Nice try.  Brick DataBlocks only please.');
			return;
		}
	}
}
