function Player::changeDataBlock ( %player, %data, %client )
{
	if ( !isObject(%data) )
	{
		return;
	}

	%oldData = %player.getDataBlock();

	if ( %data.getId() == %oldData.getId() )
	{
		return;
	}

	%player.playThread (3, root);

	if ( %oldData.canRide  &&  !%data.canRide )
	{
		if ( %player.getObjectMount() )
		{
			%player.getDataBlock().doDismount (%player, 1);
		}
	}

	%player.setDatablock (%data);
	%image = %player.getMountedImage (0);

	if ( %image )
	{
		if ( %image.getId() == %oldData.brickImage.getId()  &&  
			 %oldData.brickImage.getId() != %data.brickImage.getId() )
		{
			%player.mountImage (%data.brickImage, 0);
		}
	}

	if ( isObject(%player.client) )
	{
		commandToClient (%player.client, 'ShowEnergyBar', %data.showEnergyBar);
	}
}


// =================
//  Register Events
// =================

registerOutputEvent ("Player", "ChangeDataBlock", "datablock PlayerData");

registerOutputEvent ("Bot", "ChangeDataBlock", "datablock PlayerData");
