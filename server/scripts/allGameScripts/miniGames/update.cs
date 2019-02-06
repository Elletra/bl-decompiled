function MiniGameSO::updatePlayerDataBlock ( %obj )
{
	for ( %i = 0;  %i < %obj.numMembers;  %i++ )
	{
		%cl = %obj.member[%i];
		%player = %cl.player;

		commandToClient (%cl, 'ShowEnergyBar', %obj.PlayerDataBlock.showEnergyBar);

		if ( isObject(%player) )
		{
			if ( %player.getDataBlock() != %obj.PlayerDataBlock )
			{
				if ( %player.getMountedImage(0)  &&  isObject(%player.getDataBlock().brickImage) )
				{
					if ( %player.getMountedImage(0).getId() == %player.getDataBlock().brickImage.getId() )
					{
						%player.mountImage (%obj.PlayerDataBlock.brickImage, 0);
					}
				}

				if ( !%obj.PlayerDataBlock.canRide )
				{
					if ( %player.getObjectMount() )
					{
						%player.getDataBlock().doDismount (%player);
					}
				}

				%player.setDatablock (%obj.PlayerDataBlock);
				fixArmReady (%player);
				applyCharacterPrefs (%cl);
			}
		}
	}
}

function MiniGameSO::setEnableBuilding ( %obj, %val )
{
	%obj.EnableBuilding = %val;
	%obj.updateEnableBuilding();
}

function MiniGameSO::updateEnableBuilding ( %obj )
{
	for ( %i = 0;  %i < %obj.numMembers;  %i++ )
	{
		%cl = %obj.member[%i];
		%player = %cl.Player;

		commandToClient (%cl, 'SetBuildingDisabled', !%obj.EnableBuilding);

		if ( !%obj.EnableBuilding )
		{
			if ( isObject(%player) )
			{
				for ( %j = 0;  %j < %player.getDataBlock().maxItems;  %j++ )
				{
					%client.inventory[%j] = 0;
					messageClient (%cl, 'MsgSetInvData', "", %j, 0);
				}

				if ( isObject(%player.tempBrick) )
				{
					%player.tempBrick.delete();
					%player.tempBrick = 0;
				}

				if ( %player.getMountedImage(0) == %player.getDataBlock().brickImage.getId() )
				{
					%player.unmountImage(0);
				}
			}
		}
	}
}

function MiniGameSO::setEnablePainting ( %obj, %val )
{
	%obj.EnablePainting = %val;
	%obj.updateEnablePainting();
}

function MiniGameSO::updateEnablePainting ( %obj )
{
	for ( %i = 0;  %i < %obj.numMembers;  %i++ )
	{
		%cl = %obj.member[%i];
		%player = %cl.player;

		commandToClient (%cl, 'SetPaintingDisabled', !%obj.EnablePainting);

		if ( !%obj.EnablePainting  &&  isObject(%player) )
		{
			if ( isObject ( %player.getMountedImage(0) ) )
			{
				%imgName = %player.getMountedImage(0).getName();

				if ( strpos(%imgName, "SprayCan") != -1 )
				{
					%player.unmountImage(0);
				}
			}
		}
	}
}
