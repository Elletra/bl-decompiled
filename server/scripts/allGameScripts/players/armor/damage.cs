$Game::PlayerInvulnerabilityTime = 2500;


function ShapeBase::damage ( %this, %sourceObject, %position, %damage, %damageType )
{
	%this.getDataBlock().damage (%this, %sourceObject, %position, %damage, %damageType);
}

function ShapeBaseData::damage ( %this, %obj, %position, %source, %amount, %damageType )
{
	// Your code here
}


function Armor::damage ( %data, %obj, %sourceObject, %position, %damage, %damageType )
{
	if ( %obj.getState() $= "Dead" )
	{
		return;
	}

	if ( getSimTime() - %obj.spawnTime < $Game::PlayerInvulnerabilityTime  &&  !%obj.hasShotOnce )
	{
		return;
	}

	if ( %obj.invulnerable )
	{
		return;
	}

	if ( %obj.isMounted()  &&  %damageType != $DamageType::Suicide  &&  %data.rideAble == 0 )
	{
		%mountData = %obj.getObjectMount().getDataBlock();

		if ( $Damage::Direct[%damageType] )
		{
			if ( %mountData.protectPassengersDirect )
			{
				return;
			}
		}
		else if ( %mountData.protectPassengersRadius )
		{
			return;
		}
	}

	if ( $Damage::Direct[%damageType] == 1 )
	{
		%obj.lastDirectDamageType = %damageType;
		%obj.lastDirectDamageTime = getSimTime();
	}


	%obj.lastDamageType = %damageType;

	if ( getSimTime() - %obj.lastPainTime > 300 )
	{
		%obj.painLevel = %damage;
	}
	else
	{
		%obj.painLevel += %damage;
	}


	%obj.lastPainTime = getSimTime();

	if ( %obj.isCrouched() )
	{
		if ( $Damage::Direct[%damageType] )
		{
			%damage *= 2.1;
		}
		else
		{
			%damage *= 0.75;
		}
	}


	%scale = getWord (%obj.getScale(), 2);
	%damage /= %scale;
	%obj.applyDamage (%damage);

	%location = "Body";
	%client = %obj.client;

	if ( isObject(%sourceObject) )
	{
		if ( %sourceObject.getClassName() $= "GameConnection" )
		{
			%sourceClient = %sourceObject;
		}
		else
		{
			%sourceClient = %sourceObject.client;
		}
	}
	else
	{
		%sourceClient = 0;
	}


	if ( isObject(%sourceObject) )
	{
		if ( %sourceObject.getType() & $TypeMasks::VehicleObjectType )
		{
			if ( %sourceObject.getControllingClient() )
			{
				%sourceClient = %sourceObject.getControllingClient();
			}
		}
	}


	if ( %obj.getState() $= "Dead" )
	{
		if ( isObject(%client) )
		{
			%client.onDeath (%sourceObject, %sourceClient, %damageType, %location);
		}
		else if ( isObject(%obj.spawnBrick) )
		{
			%mg = getMiniGameFromObject (%sourceObject);

			if ( isObject(%mg) )
			{
				%obj.spawnBrick.spawnVehicle (%mg.VehicleRespawnTime);
			}
			else
			{
				%obj.spawnBrick.spawnVehicle (5000);
			}
		}
	}
	else if ( %data.useCustomPainEffects == 1 )
	{
		if ( %obj.painLevel >= 40 )
		{
			if ( isObject(%data.PainHighImage) )
			{
				%obj.emote (%data.PainHighImage, 1);
			}
		}
		else
		{
			if ( %obj.painLevel >= 25 )
			{
				if ( isObject(%data.PainMidImage) )
				{
					%obj.emote (%data.PainMidImage, 1);
				}
			}
			else
			{
				if ( isObject(%data.PainLowImage) )
				{
					%obj.emote (%data.PainLowImage, 1);
				}
			}
		}
	}
	else
	{
		if ( %obj.painLevel >= 40 )
		{
			%obj.emote (PainHighImage, 1);
		}
		else if ( %obj.painLevel >= 25 )
		{
			%obj.emote (PainMidImage, 1);
		}
		else
		{
			%obj.emote (PainLowImage, 1);
		}
	}
}

function Armor::onDamage ( %this, %obj, %delta )
{
	if ( %delta > 0  &&  %obj.getState() !$= "Dead" )
	{
		%flash = %obj.getDamageFlash() + %delta / %this.maxDamage * 2;

		if ( %flash > 0.75 )
		{
			%flash = 0.75;
		}

		%obj.setDamageFlash (%flash);

		%painThreshold = 7;

		if ( %this.painThreshold !$= "" )
		{
			%painThreshold = %this.painThreshold;
		}

		if ( %delta > %painThreshold )
		{
			%obj.playPain();
		}
	}
}
