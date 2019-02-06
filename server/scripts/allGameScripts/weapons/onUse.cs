function fixArmReady ( %obj )
{
	%leftimage = %obj.getMountedImage ($LeftHandSlot);
	%rightImage = %obj.getMountedImage ($RightHandSlot);

	%leftReady = 0;
	%rightReady = 0;

	if ( %leftimage )
	{
		%leftReady = %leftimage.armReady;
	}

	if ( %rightImage )
	{
		%rightReady = %rightImage.armReady;
	}


	if ( %rightReady )
	{
		if ( %leftReady )
		{
			%obj.playThread (1, armReadyBoth);
		}
		else
		{
			%obj.playThread (1, armReadyRight);
		}
	}
	else if ( %leftReady )
	{
		%obj.playThread (1, armReadyLeft);
	}
	else
	{
		%obj.playThread (1, root);
	}
}


function Player::updateArm ( %player, %newImage )
{
	if ( %newImage.armReady )
	{
		if ( %player.getMountedImage($LeftHandSlot) )
		{
			if ( %player.getMountedImage($LeftHandSlot).armReady )
			{
				%player.playThread (1, armReadyBoth);
			}
			else
			{
				%player.playThread (1, armReadyRight);
			}
		}
		else
		{
			%oldImage = %player.getMountedImage ($RightHandSlot);

			if ( %oldImage )
			{
				if ( !%oldImage.armReady )
				{
					%player.playThread (1, armReadyRight);
				}
			}
			else
			{
				%player.playThread (1, armReadyRight);
			}
		}
	}
}


function Weapon::onUse ( %this, %player, %invPosition )
{
	%mountPoint = %this.image.mountPoint;
	%mountedImage = %player.getMountedImage (%mountPoint);

	%image = %this.image;

	%player.updateArm (%image);
	%player.mountImage (%image, %mountPoint);

	return;

	if ( %mountedImage )
	{
		if ( %mountedImage == %this.image.getId() )
		{
			%player.unmountImage (%mountPoint);
			messageClient (%player.client, 'MsgHilightInv', '', -1);
			%player.currWeaponSlot = -1;
		}
		else
		{
			%player.mountImage (%this.image, %mountPoint);
			messageClient (%player.client, 'MsgHilightInv', '', %invPosition);
			%player.currWeaponSlot = %invPosition;
		}
	}
	else
	{
		%player.mountImage (%this.image, %mountPoint);
		messageClient (%player.client, 'MsgHilightInv', '', %invPosition);
		%player.currWeaponSlot = %invPosition;
	}
}
