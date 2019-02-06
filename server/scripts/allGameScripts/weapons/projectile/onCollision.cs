function ProjectileData::onCollision ( %this, %obj, %col, %fade, %pos, %normal, %velocity )
{
	%client = %obj.client;

	if ( isObject(%obj.sourceObject)  &&  %obj.sourceObject.isBot )
	{
		%client = %obj.sourceObject;
	}

	%victimClient = %col.client;
	%clientBLID = getBL_IDFromObject (%obj);

	if ( isObject(%client.miniGame) )
	{
		if ( getSimTime() - %client.lastF8Time < 3000 )
		{
			return;
		}
	}


	%scale = getWord (%obj.getScale(), 2);
	%mask = $TypeMasks::PlayerObjectType | $TypeMasks::VehicleObjectType;

	if ( %col.getType() & %mask )
	{
		if ( !isObject(%client) )
		{
			return;
		}

		%yourStuffOverride = 0;

		if ( !isObject(%client.miniGame)  &&  isObject(%col.spawnBrick) )
		{
			if ( %col.spawnBrick.getGroup().bl_id == %clientBLID  ||  %col.spawnBrick.getGroup().bl_id == 888888 )
			{
				%yourStuffOverride = 1;
			}
		}

		if ( miniGameCanDamage(%client, %col) == 1  ||  %yourStuffOverride )
		{
			%this.Damage (%obj, %col, %fade, %pos, %normal);
			%this.impactImpulse (%obj, %col, %velocity);
		}
	}
	else if ( %col.getType() & $TypeMasks::FxBrickObjectType )
	{
		%mg = %client.miniGame;

		if ( isObject(%mg) )
		{
			%respawnTime = %mg.BrickRespawnTime;
		}
		else
		{
			%respawnTime = mClampF ($Server::BrickRespawnTime, 1000, 60000);
		}

		%blowUp = false;

		%brickExplosionMaxVolume = %this.brickExplosionMaxVolume * %scale;
		%brickExplosionMaxVolumeFloating = %this.brickExplosionMaxVolumeFloating * %scale;

		if ( %this.brickExplosionImpact == 1 )
		{
			%blowUp = true;

			if ( !%col.canExplode(%brickExplosionMaxVolume, %brickExplosionMaxVolumeFloating) )
			{
				%blowUp = false;
			}

			if ( $Server::LAN )
			{
				if ( isObject(%mg) )
				{
					if ( !%mg.BrickDamage )
					{
						%blowUp = false;
					}
				}
			}
			else if ( isObject(%mg) )
			{
				if ( miniGameCanDamage(%client, %col) != 1 )
				{
					%blowUp = false;
				}
			}
			else if ( isObject(%obj.sourceObject) )
			{
				if ( %col.getGroup().bl_id != %clientBLID  &&  %col.getGroup() != %obj.sourceObject.getGroup() )
				{
					%blowUp = false;
				}
			}
			else if ( %col.getGroup().bl_id != %clientBLID )
			{
				%blowUp = false;
			}


			if ( %blowUp )
			{
				%col.onBlownUp (%client, %obj.sourceObject);
				transmitBrickExplosion (%pos, %this.brickExplosionForce, 0.02, %respawnTime, %col);
			}
		}

		if ( !%blowUp )
		{
			%col.onProjectileHit (%obj, %client);
		}
	}
}
