function FlyingVehicleData::damage ( %this, %obj, %sourceObject, %position, %damage, %damageType )
{
	if ( %obj.getDamageState() !$= "Enabled" )
	{
		return;
	}

	if ( $Damage::VehicleDamageScale[%damageType] !$= "" )
	{
		%damage *= $Damage::VehicleDamageScale[%damageType];
	}

	%scale = getWord (%obj.getScale(), 2);

	%damage /= %scale;

	if ( %damage == 0 )
	{
		return;
	}

	if ( getSimTime() - %obj.creationTime < 1000 )
	{
		return;
	}

	%obj.applyDamage (%damage);


	if ( isObject(%sourceObject.client) )
	{
		%obj.lastDamageClient = %sourceObject.client;
	}

	if ( %obj.getDamagePercent() >= 1.0 )
	{
		%obj.setDamageState ("Disabled");
		%obj.setNodeColor ("ALL", "0 0 0 1");

		%pos = VectorAdd (%obj.getPosition(), "0 0" SPC %this.initialExplosionOffset);

		if ( isObject(%obj.lastDamageClient) )
		{
			%client = %obj.lastDamageClient;
		}
		else
		{
			%client = %obj.spawnBrick.getGroup().client;
		}

		if ( isObject(%this.initialExplosionProjectile) )
		{
			%projectileData = %this.initialExplosionProjectile;
		}
		else
		{
			%projectileData = vehicleExplosionProjectile;
		}

		if ( %this.initialExplosionScale $= "" )
		{
			%this.initialExplosionScale = 1;
		}

		%this.initialExplosionScale = mClampF (%this.initialExplosionScale, 0.2, 5);

		%finalScale = getWord (%obj.getScale(), 2) * %this.initialExplosionScale;

		%p = new Projectile()
		{
			dataBlock = %projectileData;
			initialPosition = %pos;
			sourceClient = %client;
		};
		%p.setScale (%finalScale SPC %finalScale SPC %finalScale);
		%p.client = %client;

		MissionCleanup.add(%p);

		%obj.schedule (%this.burnTime, finalExplosion);

		%mg = getMiniGameFromObject (%obj);

		if ( isObject(%mg) )
		{
			if ( %mg.VehicleRespawnTime > 0 )
			{
				%obj.spawnBrick.spawnVehicle (%mg.VehicleRespawnTime);
			}
		}
		else
		{
			%obj.spawnBrick.spawnVehicle (5000);
		}

		if ( isObject(%this.burnDataBlock) )
		{
			%obj.setDatablock (%this.burnDataBlock);
		}
	}
}

function FlyingVehicleData::onCollision ( %this, %obj, %col, %vec, %speed )
{
	WheeledVehicleData::onCollision (%this, %obj, %col, %vec, %speed);
}
