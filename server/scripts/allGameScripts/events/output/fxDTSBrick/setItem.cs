function fxDTSBrick::setItem ( %obj, %data, %client )
{
	if ( isObject(%obj.Item) )
	{
		%obj.Item.delete();
	}

	%obj.Item = 0;


	if ( %obj.isDead() )
	{
		return;
	}

	if ( !isObject(%data) )
	{
		return;
	}

	if ( %data.getClassName() !$= "ItemData" )
	{
		return;
	}

	if ( %data.uiName $= "" )
	{
		return;
	}


	%item = new Item()
	{
		dataBlock = %data;
		static = 1;
	};

	if ( !isObject(%item) )
	{
		if ( $Server::LAN )
		{
			commandToClient (%client, 'centerPrint', "\c6You can\'t have more than " @  
				$Server::QuotaLAN::Item  @ " items!", 3);
		}
		else
		{
			commandToClient(%client, 'centerPrint', "\c6You can\'t have more than " @  
				$Server::Quota::Item  @ " items!", 3);
		}

		return;
	}

	MissionCleanup.add (%item);

	%obj.Item = %item;
	%item.spawnBrick = %obj;

	if ( %obj.itemRespawnTime < $Game::Item::MinRespawnTime )
	{
		%obj.itemRespawnTime = $Game::Item::MinRespawnTime;
	}
	else if ( %obj.itemRespawnTime > $Game::Item::MaxRespawnTime )
	{
		%obj.itemRespawnTime = $Game::Item::MaxRespawnTime;
	}

	%obj.Item.setRespawnTime (%obj.itemRespawnTime);
	%obj.itemDirection = mFloor (%obj.itemDirection);
	%obj.setItemDirection (%obj.itemDirection);
}

function fxDTSBrick::setItemDirection ( %obj, %dir )
{
	%dir = mClamp (%dir, 2, 5);
	%obj.itemDirection = %dir;

	if ( !isObject(%obj.Item) )
	{
		return;
	}


	%pos = getWords (%obj.Item.getTransform(), 0, 2);

	if ( %dir == 2 )
	{
		%rot = "0 0 1 0";
	}
	else if ( %dir == 3 )
	{
		%rot = "0 0 1 " @ $piOver2;
	}
	else if ( %dir == 4 )
	{
		%rot = "0 0 -1 " @ $pi;
	}
	else if ( %dir == 5 )
	{
		%rot = "0 0 -1 " @ $piOver2;
	}
	else
	{
		%rot = "0 0 1 0";
	}

	%obj.Item.setTransform (%pos SPC %rot);
	%obj.setItemPosition (%obj.itemPosition);
}

function fxDTSBrick::setItemPosition ( %obj, %dir )
{
	if ( %dir < 0  ||  %dir > 5 )
	{
		return;
	}

	%obj.itemPosition = %dir;

	if ( !isObject(%obj.Item) )
	{
		return;
	}


	%itemBox = %obj.Item.getWorldBox();

	%itemBoxX = ( mAbs ( getWord(%itemBox, 0) - getWord(%itemBox, 3) ) ) / 2;
	%itemBoxY = ( mAbs ( getWord(%itemBox, 1) - getWord(%itemBox, 4) ) ) / 2;
	%itemBoxZ = ( mAbs ( getWord(%itemBox, 2) - getWord(%itemBox, 5) ) ) / 2;

	%itemBoxCenter = %obj.Item.getWorldBoxCenter();
	%itemCenter = %obj.Item.getPosition();
	%itemOffset = VectorSub(%itemCenter, %itemBoxCenter);

	%brickBox = %obj.getWorldBox();

	%brickBoxX = ( mAbs ( getWord(%brickBox, 0) - getWord(%brickBox, 3) ) ) / 2;
	%brickBoxY = ( mAbs ( getWord(%brickBox, 1) - getWord(%brickBox, 4) ) ) / 2;
	%brickBoxZ = ( mAbs ( getWord(%brickBox, 2) - getWord(%brickBox, 5) ) ) / 2;

	%pos = %obj.getPosition();
	%pos = VectorAdd (%pos, %itemOffset);

	%posX = getWord (%pos, 0);
	%posY = getWord (%pos, 1);
	%posZ = getWord (%pos, 2);

	%rot = getWords (%obj.Item.getTransform(), 3, 6);

	if ( %dir == 0 )
	{
		%posZ += %itemBoxZ + %brickBoxZ;
	}
	else if ( %dir == 1 )
	{
		%posZ -= %itemBoxZ + %brickBoxZ;
	}
	else if ( %dir == 2 )
	{
		%posY += %itemBoxY + %brickBoxY;
	}
	else if ( %dir == 3 )
	{
		%posX += %itemBoxX + %brickBoxX;
	}
	else if ( %dir == 4 )
	{
		%posY -= %itemBoxY + %brickBoxY;
	}
	else if ( %dir == 5 )
	{
		%posX -= %itemBoxX + %brickBoxX;
	}

	%obj.Item.setTransform (%posX SPC %posY SPC %posZ SPC %rot);
}

function fxDTSBrick::setItemRespawntime ( %obj, %time )
{
	if ( %time < $Game::Item::MinRespawnTime )
	{
		%time = $Game::Item::MinRespawnTime;
	}
	else if ( %time > $Game::Item::MaxRespawnTime )
	{
		%time = $Game::Item::MaxRespawnTime;
	}


	%obj.itemRespawnTime = %time;

	if ( isObject(%obj.Item) )
	{
		%obj.Item.setRespawnTime (%time);
	}
}


// =================
//  Register Events
// =================

registerOutputEvent ("fxDTSBrick", "setItem", "dataBlock ItemData");
registerOutputEvent ("fxDTSBrick", "setItemDirection", "list North 2 East 3 South 4 West 5");
registerOutputEvent ("fxDTSBrick", "setItemPosition", "list Up 0 Down 1 North 2 East 3 South 4 West 5");
