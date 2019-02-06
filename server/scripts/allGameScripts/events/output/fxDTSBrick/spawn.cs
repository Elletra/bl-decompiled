function fxDTSBrick::spawnItem ( %obj, %vector, %itemData )
{
	if ( %obj.getFakeDeadTime() > 120  ||  ( !%obj.isRendering()  &&  !%obj.isRayCasting() ) )
	{
		return;
	}

	%item = new Item ()
	{
		dataBlock = %itemData;
		static = 0;
	};

	if ( !%item )
	{
		return;
	}

	MissionCleanup.add (%item);

	%item.setVelocity (%vector);
	%item.schedulePop();
	%item.spawnBrick = %obj;


	%dir = mFloor (%obj.itemDirection);

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

	%pos = %obj.getPosition();

	%item.setTransform (%pos SPC %rot);

	%itemBox = %item.getWorldBox();
	%itemBoxX = ( mAbs ( getWord(%itemBox, 0) - getWord(%itemBox, 3) ) ) / 2;
	%itemBoxY = ( mAbs ( getWord(%itemBox, 1) - getWord(%itemBox, 4) ) ) / 2;
	%itemBoxZ = ( mAbs ( getWord(%itemBox, 2) - getWord(%itemBox, 5) ) ) / 2;

	%itemBoxCenter = %item.getWorldBoxCenter();
	%itemCenter = %item.getPosition();
	%itemOffset = VectorSub (%itemCenter, %itemBoxCenter);

	%brickBox = %obj.getWorldBox();
	%brickBoxX = ( mAbs ( getWord(%brickBox, 0) - getWord(%brickBox, 3) ) ) / 2;
	%brickBoxY = ( mAbs ( getWord(%brickBox, 1) - getWord(%brickBox, 4) ) ) / 2;
	%brickBoxZ = ( mAbs ( getWord(%brickBox, 2) - getWord(%brickBox, 5) ) ) / 2;

	%vecZ = getWord (%vector, 2);

	if ( %vecZ > 0 )
	{
		if ( %itemBoxZ - %brickBoxZ + 0.1 > 0 )
		{
			%itemOffset = VectorAdd (%itemOffset, "0 0" SPC %itemBoxZ - %brickBoxZ + 0.1);
		}
	}
	else if ( %vecZ < 0 )
	{
		if ( %itemBoxZ - %brickBoxZ + 0.1 > 0 )
		{
			%itemOffset = VectorSub(%itemOffset, "0 0" SPC %itemBoxZ - %brickBoxZ + 0.1);
		}
	}

	%pos = VectorAdd (%pos, %itemOffset);
	%posX = getWord (%pos, 0);
	%posY = getWord (%pos, 1);
	%posZ = getWord (%pos, 2);

	%rot = getWords (%item.getTransform(), 3, 6);

	%item.setTransform (%posX SPC %posY SPC %posZ SPC %rot);
	%item.setCollisionTimeout (%obj);
}

function fxDTSBrick::spawnProjectile ( %obj, %velocity, %projectileData, %variance, %scale, %client )
{
	if ( %obj.getFakeDeadTime() > 120  ||  ( !%obj.isRendering()  &&  !%obj.isRayCasting() ) )
	{
		return;
	}

	if ( !isObject(%projectileData) )
	{
		return;
	}

	%WB = %obj.getWorldBox();

	%wbX = getWord (%WB, 0);
	%wbY = getWord (%WB, 1);
	%wbZ = getWord (%WB, 2);

	%wbXSize = getWord (%WB, 3) - %wbX;
	%wbYSize = getWord (%WB, 4) - %wbY;
	%wbZSize = getWord (%WB, 5) - %wbZ;

	if ( %wbXSize < 1.05 )
	{
		%wbX += %wbXSize / 2;
		%wbXSize = 0;
	}

	if ( %wbYSize < 1)
	{
		%wbY += %wbYSize / 2;
		%wbYSize = 0;
	}

	if ( %wbZSize < 0.65 )
	{
		%wbZ += %wbZSize / 2;
		%wbZSize = 0;
	}


	%pos = %wbX + getRandom() * %wbXSize SPC %wbY + getRandom() * %wbYSize SPC %wbZ + getRandom() * %wbZSize;

	%velx = getWord (%velocity, 0);
	%vely = getWord (%velocity, 1);
	%velz = getWord (%velocity, 2);

	%varx = getWord (%variance, 0);
	%vary = getWord (%variance, 1);
	%varz = getWord (%variance, 2);

	%x = %velx + %varx * getRandom() - %varx / 2;
	%y = %vely + %vary * getRandom() - %vary / 2;
	%z = %velz + %varz * getRandom() - %varz / 2;

	%muzzleVelocity = %x SPC %y SPC %z;

	%p = new Projectile()
	{
		dataBlock = %projectileData;

		initialVelocity = %muzzleVelocity;
		initialPosition = %pos;

		sourceClient = %client;
		sourceObject = %obj;

		client = %client;
	};

	if ( %p )
	{
		MissionCleanup.add (%p);

		%p.setScale (%scale SPC %scale SPC %scale);
		%p.spawnBrick = %obj;
	}
}

function fxDTSBrick::spawnExplosion ( %obj, %projectileData, %scale, %client )
{
	if ( %obj.getFakeDeadTime() > 120  ||  ( !%obj.isRendering()  &&  !%obj.isRayCasting() ) )
	{
		return;
	}

	if ( !isObject(%projectileData) )
	{
		return;
	}

	%WB = %obj.getWorldBox();

	%wbX = getWord (%WB, 0);
	%wbY = getWord (%WB, 1);
	%wbZ = getWord (%WB, 2);

	%wbXSize = getWord(%WB, 3) - %wbX;
	%wbYSize = getWord(%WB, 4) - %wbY;
	%wbZSize = getWord(%WB, 5) - %wbZ;

	if ( %wbXSize < 1.05 )
	{
		%wbX += %wbXSize / 2;
		%wbXSize = 0;
	}

	if ( %wbYSize < 1.05 )
	{
		%wbY += %wbYSize / 2;
		%wbYSize = 0;
	}

	if ( %wbZSize < 0.65 )
	{
		%wbZ += %wbZSize / 2;
		%wbZSize = 0;
	}


	%pos = %wbX + getRandom() * %wbXSize SPC %wbY + getRandom() * %wbYSize SPC %wbZ + getRandom() * %wbZSize;

	%p = new Projectile()
	{
		dataBlock = %projectileData;

		initialVelocity = "0 0 1";
		initialPosition = %pos;

		sourceClient = %client;
		sourceObject = %obj;

		client = %client;
	};

	if ( !isObject(%p) )
	{
		return;
	}

	MissionCleanup.add (%p);

	%p.setScale (%scale SPC %scale SPC %scale);
	%p.spawnBrick = %obj;
	%p.explode();
}


// =================
//  Register Events
// =================

registerOutputEvent ("fxDTSBrick", "spawnItem", "vector 200" TAB "dataBlock ItemData");

registerOutputEvent ("fxDTSBrick", "spawnProjectile", "vector 200" TAB "dataBlock ProjectileData" TAB 
	"vector 200" TAB "float 0.2 2 0.1 1");

registerOutputEvent ("fxDTSBrick", "spawnExplosion", "dataBlock ProjectileData" TAB 
	"float 0.2 2 0.1 1");
