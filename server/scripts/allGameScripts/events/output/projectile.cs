function Projectile::Bounce ( %obj, %factor, %client )
{
	%vel = %obj.getLastImpactVelocity();
	%norm = %obj.getLastImpactNormal();

	%bounceVel = VectorSub ( %vel, VectorScale ( %norm, VectorDot(%vel, %norm) * 2 ) );
	%bounceVel = VectorScale (%bounceVel, %factor);

	if ( VectorLen(%bounceVel) > 200 )
	{
		%bounceVel = VectorScale (VectorNormalize(%bounceVel), 200);
	}

	%p = new Projectile()
	{
		dataBlock = %obj.getDataBlock();

		initialPosition = %obj.getLastImpactPosition();
		initialVelocity = %bounceVel;

		sourceObject = 0;
		sourceSlot = %obj.sourceSlot;

		client = %obj.client;
	};

	if ( %p )
	{
		MissionCleanup.add (%p);

		%p.setScale ( %obj.getScale() );
		%p.spawnBrick = %obj.spawnBrick;
	}

	%obj.delete();
}

function Projectile::Redirect ( %obj, %vector, %normalized, %client )
{
	%vel = %obj.getLastImpactVelocity();

	if ( %normalized )
	{
		%vec = VectorNormalize (%vector);
		%len = VectorLen (%vel);
		%bounceVel = VectorScale (%vec, %len);
	}
	else
	{
		%bounceVel = %vector;
	}

	if ( VectorLen(%bounceVel) > 200 )
	{
		%bounceVel = VectorScale (VectorNormalize(%bounceVel), 200);
	}

	%p = new Projectile()
	{
		dataBlock = %obj.getDataBlock();

		initialPosition = %obj.getLastImpactPosition();
		initialVelocity = %bounceVel;

		sourceObject = 0;
		sourceSlot = %obj.sourceSlot;

		client = %obj.client;
	};

	if ( !%p )
	{
		MissionCleanup.add (%p);
		%p.setScale ( %obj.getScale() );
	}

	%obj.delete();
}


// =================
//  Register Events
// =================

registerOutputEvent ("Projectile", "Explode", "", 0);
registerOutputEvent ("Projectile", "Delete", "", 0);
registerOutputEvent ("Projectile", "Bounce", "float 0 2 0.1 0.5");
registerOutputEvent ("Projectile", "Redirect", "vector 200" TAB "bool");
