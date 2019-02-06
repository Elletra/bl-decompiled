function radiusDamage ( %sourceObject, %position, %radius, %damage, %damageType, %impulse )
{
	initContainerRadiusSearch (%position, %radius, $TypeMasks::ShapeBaseObjectType);

	%halfRadius = %radius / 2;

	while ( ( %targetObject = containerSearchNext() ) != 0 )
	{
		%coverage = calcExplosionCoverage (%position, %targetObject, $TypeMasks::VehicleObjectType);

		if ( %coverage != 0 )
		{
			%dist = containerSearchCurrRadiusDist();
			%distScale = (%dist < %halfRadius)  ?  1  :  1 - (%dist - %halfRadius) / %halfRadius;

			%targetObject.damage (%sourceObject, %position, %damage * %coverage * %distScale, %damageType);

			if ( %impulse )
			{
				%impulseVec = VectorSub (%targetObject.getWorldBoxCenter(), %position);
				%impulseVec = VectorNormalize (%impulseVec);
				%impulseVec = VectorScale (%impulseVec, %impulse * %distScale);

				%targetObject.applyImpulse (%position, %impulseVec);
			}
		}
	}
}


function ProjectileData::damage ( %this, %obj, %col, %fade, %pos, %normal )
{
	if ( %this.directDamage <= 0 )
	{
		return;
	}

	%damageType = $DamageType::Direct;

	if ( %this.directDamageType )
	{
		%damageType = %this.directDamageType;
	}

	%scale = getWord (%obj.getScale(), 2);
	%directDamage = mClampF (%this.directDamage, -100, 100) * %scale;

	if ( %col.getType() & $TypeMasks::PlayerObjectType )
	{
		%col.Damage (%obj, %pos, %directDamage, %damageType);
	}
	else
	{
		%col.Damage (%obj, %pos, %directDamage, %damageType);
	}
}

function ProjectileData::radiusDamage ( %this, %obj, %col, %distanceFactor, %pos, %damageAmt )
{
	// Validate distance factor

	if ( %distanceFactor <= 0 )
	{
		return;
	}
	else if ( %distanceFactor > 1 )
	{
		%distanceFactor = 1;
	}

	%damageAmt *= %distanceFactor;

	if ( %damageAmt )
	{
		// Use default damage type if no damage type is given

		%damageType = $DamageType::Radius;

		if ( %this.RadiusDamageType )
		{
			%damageType = %this.RadiusDamageType;
		}

		%col.Damage (%obj, %pos, %damageAmt, %damageType);

		// Burn the player?

		if ( %this.Explosion.playerBurnTime > 0 )
		{
			if ( %col.getType() & $TypeMasks::PlayerObjectType | $TypeMasks::CorpseObjectType )
			{
				// Check for vehicle protection from burning

				%doBurn = true;

				if ( %col.isMounted() )
				{
					%mountData = %col.getObjectMount().getDataBlock();

					if ( %mountData.protectPassengersBurn )
					{
						%doBurn = false;
					}
				}

				if ( %doBurn )
				{
					%col.burn (%this.Explosion.playerBurnTime * %distanceFactor);
				}
			}
		}
	}
}
