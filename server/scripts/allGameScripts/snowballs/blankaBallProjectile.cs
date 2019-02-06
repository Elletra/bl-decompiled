AddDamageType ("blankaBallDirect", '<bitmap:add-ons/Vehicle_Pirate_Cannon/ball> %1', 
	'%2 <bitmap:add-ons/Vehicle_Pirate_Cannon/ball> %1', 1, 1);

datablock ProjectileData (blankaBallProjectile)
{
	projectileShapeName = "base/data/shapes/snowBall.dts";
	directDamage = 0;
	DirectDamageType = $DamageType::blankaBallDirect;
	impactImpulse = 0;
	verticalImpulse = 0;
	Explosion = blankaBallExplosion;
	particleEmitter = blankaBallTrailEmitter;

	brickExplosionRadius = 0;
	brickExplosionImpact = 1;
	brickExplosionForce = 40;
	brickExplosionMaxVolume = 24;
	brickExplosionMaxVolumeFloating = 24;

	muzzleVelocity = 30;
	velInheritFactor = 1;
	armingDelay = 0;
	lifetime = 20000;
	fadeDelay = 19500;
	bounceElasticity = 0;
	bounceFriction = 0;
	isBallistic = 1;
	gravityMod = 0.75;
	hasLight = 0;
	lightRadius = 3;
	lightColor = "0 0 0.5";
};

function blankaBallProjectile::onCollision ( %this, %obj, %col, %fade, %pos, %normal )
{
	if ( %col.getType() & $TypeMasks::PlayerObjectType )
	{
		%canDamage = miniGameCanDamage (%obj, %col);

		if ( %canDamage )
		{
			%col.setWhiteOut (0.5);
		}

		if ( %canDamage  &&  isEventPending(%col.tempColorSchedule) )
		{
			%damage = %col.getDataBlock().maxDamage * 2;
			%client = %col.client;

			%col.damage (%obj, %pos, %damage, $DamageType::blankaBallDirect);
			%col.setDamageFlash (0);
			%col.setWhiteOut (1);
		}

		if ( %canDamage )
		{
			%col.setTempColor ("1 1 1 1", 5000, %pos, %this);
		}

		ServerPlay3D ( blankaBallExplosionSound, %col.getPosition() );
	}
	else
	{
		ServerPlay3D (blankaBallExplosionSound, %pos);
	}

	if ( isObject ( getMiniGameFromObject(%obj) ) )
	{
		Parent::onCollision (%this, %obj, %col, %fade, %pos, %normal);
	}
}
