$WeaponSlot = 0;


datablock AudioProfile (weaponSwitchSound)
{
	fileName = "~/data/sound/weaponSwitch.wav";
	description = AudioClosest3d;
	preload = 1;
};


function ItemData::onUse ( %this, %player, %invPosition )
{
	%client = %player.client;

	%playerData = %player.getDataBlock();

	%mountPoint = %this.image.mountPoint;
	%mountedImage = %player.getMountedImage (%mountPoint);

	%image = %this.image;

	%player.updateArm (%image);
	%player.mountImage (%image, %mountPoint);
}

function ShapeBase::use ( %this, %data )
{
	if ( %this.getInventory(%data) > 0 )
	{
		return %data.onUse (%this);
	}

	return 0;
}

function ShapeBaseData::onUse ( %this, %user )
{
	return 0;
}


function serverCmdUseTool ( %client, %slot )
{
	if ( %client.isTalking )
	{
		serverCmdStopTalking (%client);
	}


	%player = %client.Player;

	if ( !isObject(%player) )
	{
		return;
	}


	if ( %player.tool[%slot] > 0 )
	{
		%player.currTool = %slot;
		%client.currInv = -1;
		%client.currInvSlot = -1;

		%item = %player.tool[%slot].getId();
		%item.onUse (%player, %slot);
	}
}

function serverCmdUnUseTool ( %client )
{
	%player = %client.Player;

	if ( %client.isTalking )
	{
		serverCmdStopTalking (%client);
	}

	if ( !isObject(%player) )
	{
		return;
	}


	if ( isObject(%player) )
	{
		%player.currTool = -1;
		%client.currInv = -1;
		%client.currInvSlot = -1;

		%player.unmountImage (0);
		%player.playThread (1, root);
	}
}
