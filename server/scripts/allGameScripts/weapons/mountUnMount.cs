function WeaponImage::onMount ( %this, %obj, %slot )
{
	if ( !isObject(%obj) )
	{
		error ("ERROR: WeaponImage::onMount() called with no \"%obj\" parameter!");
		return;
	}

	if ( %this.showBricks )
	{
		%client = %obj.client;

		if ( isObject(%client) )
		{
			commandToClient (%client, 'ShowBricks', 1);
		}
	}

	if ( %this.ammo )
	{
		if ( %obj.getInventory(%this.ammo) )
		{
			%obj.setImageAmmo (%slot, 1);
		}
	}
	else
	{
		%obj.setImageAmmo (%slot, 1);
	}
}

function WeaponImage::onUnMount ( %this, %obj, %slot )
{
	%obj.playThread (2, root);

	%leftimage = %obj.getMountedImage ($LeftHandSlot);

	%client = %obj.client;

	if ( %this.showBricks )
	{
		if ( isObject(%client) )
		{
			commandToClient (%client, 'ShowBricks', 0);
		}
	}
}
