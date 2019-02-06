function Player::giveDefaultEquipment ( %player )
{
	// FIXME: don't make this hard-coded at 5

	%player.tool[0] = hammerItem.getId();
	%player.tool[1] = WrenchItem.getId();
	%player.tool[2] = PrintGun.getId();
	%player.tool[3] = 0;
	%player.tool[4] = 0;

	%client = %player.client;

	if ( isObject(%client) )
	{
		// FIXME: don't make this hard-coded at 5

		messageClient (%client, 'MsgItemPickup', "", 0, hammerItem.getId(), 1);
		messageClient (%client, 'MsgItemPickup', "", 1, WrenchItem.getId(), 1);
		messageClient (%client, 'MsgItemPickup', "", 2, PrintGun.getId(), 1);
		messageClient (%client, 'MsgItemPickup', "", 3, 0, 1);
		messageClient (%client, 'MsgItemPickup', "", 4, 0, 1);
	}
}

function ShapeBase::incInventory ( %this, %data, %amount )
{
	%max = %this.maxInventory (%data);
	%total = %this.inv[ %data.getName() ];

	if ( %total < %max )
	{
		if ( %total + %amount > %max )
		{
			%amount = %max - %total;
		}

		%this.setInventory (%data, %total + %amount);
		return %amount;
	}

	return 0;
}

function ShapeBase::decInventory ( %this, %data, %amount )
{
	%total = %this.inv[ %data.getName() ];

	if ( %total > 0 )
	{
		if ( %total < %amount )
		{
			%amount = %total;
		}

		%this.setInventory (%data, %total - %amount);
		return %amount;
	}

	return 0;
}

function ShapeBase::setInventory ( %this, %data, %value )
{
	if ( %value < 0 )
	{
		%value = 0;
	}
	else
	{
		%max = %this.maxInventory (%data);

		if ( %value > %max )
		{
			%value = %max;
		}
	}


	%name = %data.getName();

	if ( %this.inv[%name] != %value )
	{
		%this.inv[%name] = %value;
		%data.onInventory (%this, %value);
		%this.getDataBlock().onInventory (%data, %value);
	}

	return %value;
}

function ShapeBase::clearInventory ( %this )
{
	// Your code here
}
