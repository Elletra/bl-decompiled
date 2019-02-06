function Armor::onImpact ( %this, %obj, %collidedObject, %vec, %vecLen )
{
	if ( %collidedObject.getClassName() $= "StaticShape" )
	{
		if ( %collidedObject.getDataBlock().className $= "Glass"  &&  !%collidedObject.indestructable )
		{
			%collidedObject.explode();
			return;
		}
	}

	%doDamage = false;
	%mg = getMiniGameFromObject (%obj);

	if ( isObject(%mg) )
	{
		if ( %mg.FallingDamage )
		{
			%doDamage = true;
		}
	}
	else
	{
		if ( $pref::Server::FallingDamage == 1 )
		{
			%doDamage = true;
		}
	}


	%image = %obj.getMountedImage (0);

	if ( %image )
	{
		if ( %image.getId() == AdminWandImage.getId() )
		{
			%doDamage = false;
		}
	}


	%scale = getWord (%obj.getScale(), 2);

	if ( %vecLen < %this.minImpactSpeed * %scale )
	{
		%doDamage = false;
	}

	if ( %doDamage )
	{
		%angle = VectorDot (VectorNormalize(%vec), "0 0 1");

		if ( %angle > 0.5 )
		{
			%damageType = $DamageType::Fall;
		}
		else
		{
			%damageType = $DamageType::Impact;
		}

		%obj.Damage (0, VectorAdd(%obj.getPosition(), %vec), %vecLen * %this.speedDamageScale, %damageType);
	}
}
