$Game::VehicleInvulnerabilityTime = 100;


function WheeledVehicleData::damage ( %this, %obj, %sourceObject, %position, %damage, %damageType )
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

	if ( getSimTime() - %obj.creationTime < $Game::VehicleInvulnerabilityTime )
	{
		return;
	}

	%obj.applyDamage (%damage);

	if ( isObject(%sourceObject.client ))
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

		if ( %this.initialExplosionScale $= "" )
		{
			%this.initialExplosionScale = 1;
		}


		%this.initialExplosionScale = mClampF (%this.initialExplosionScale, 0.2, 5);
		%finalScale = getWord (%obj.getScale(), 2) * %this.initialExplosionScale;

		if ( isObject(%this.initialExplosionProjectile) )
		{
			if ( %this.initialExplosionProjectile.getClassName() $= "ProjectileData" )
			{
				%projectileData = %this.initialExplosionProjectile;
			}
			else
			{
				%projectileData = vehicleExplosionProjectile;
			}
		}
		else
		{
			%projectileData = vehicleExplosionProjectile;
		}

		%p = new Projectile ()
		{
			dataBlock = %projectileData;
			initialPosition = %pos;
			sourceClient = %client;
		};

		%p.setScale (%finalScale SPC %finalScale SPC %finalScale);
		%p.client = %client;

		MissionCleanup.add (%p);


		for ( %i = 0;  %i < %this.numWheels;  %i++ )
		{
			%obj.setWheelTire (%i, emptyTire);
			%obj.setWheelSpring (%i, emptySpring);
		}

		%obj.schedule (%this.burnTime, finalExplosion);

		%mg = getMiniGameFromObject (%sourceObject);

		if ( isObject(%mg) )
		{
			%respawnTime = %mg.VehicleRespawnTime;
		}
		else
		{
			%respawnTime = 0;
		}

		if ( %respawnTime >= 0 )
		{
			if ( %respawnTime < %this.burnTime )
			{
				%respawnTime = %this.burnTime;
			}

			%respawnTime += 100;

			if ( isObject(%obj.spawnBrick) )
			{
				%obj.spawnBrick.spawnVehicle (%respawnTime);
			}
		}

		if ( isObject(%this.burnDataBlock) )
		{
			%obj.setDatablock (%this.burnDataBlock);
		}
	}
}

function WheeledVehicleData::onDamage ( %this, %obj )
{
	// Your code here
}
