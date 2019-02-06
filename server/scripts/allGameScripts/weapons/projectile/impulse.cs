function ProjectileData::radiusImpulse ( %this, %obj, %col, %distanceFactor, %pos, %impulseAmt, %verticalAmt )
{
	if ( %col.invulnerable )
	{
		return;
	}

	if ( %distanceFactor <= 0 )
	{
		return;
	}
	else if ( %distanceFactor > 1 )
	{
		%distanceFactor = 1;
	}


	if ( %impulseAmt  ||  %verticalAmt )
	{
		if ( %col.getType() & $TypeMasks::PlayerObjectType | $TypeMasks::CorpseObjectType )
		{
			%colPos = %col.getHackPosition();
		}
		else
		{
			%colPos = %col.getWorldBoxCenter();
		}

		%impulseVec  = VectorSub (%colPos, %pos);
		%impulseVecX = getWord (%impulseVec, 0);
		%impulseVecY = getWord (%impulseVec, 1);
		%impulseVecZ = getWord (%impulseVec, 2);

		if ( %impulseVecZ < 0 )
		{
			%mask = $TypeMasks::StaticShapeObjectType | $TypeMasks::FxBrickObjectType;

			%start = %colPos;
			%end = VectorAdd (%colPos, "0 0 -3");

			if ( containerRayCast(%start, %end, %mask) )
			{
				%impulseVecZ = 0;
				%impulseVec = %impulseVecX SPC %impulseVecY SPC %impulseVecZ;
			}
		}


		%impulseVec = VectorNormalize (%impulseVec);
		%impulseVec = VectorScale (%impulseVec, %impulseAmt * %distanceFactor);

		%col.applyImpulse (%pos, %impulseVec);

		if ( %obj.upVector !$= "" )
		{
			%impulseVec = VectorScale (%obj.upVector, %verticalAmt * %distanceFactor);
		}
		else
		{
			%impulseVec = VectorScale ("0 0 1", %verticalAmt * %distanceFactor);
		}

		%col.applyImpulse (%pos, %impulseVec);

		if ( isObject(%obj.client) )
		{
			%col.lastPusher = %obj.client;
			%col.lastPushTime = getSimTime();
		}
	}
}

function ProjectileData::impactImpulse ( %this, %obj, %col, %vector )
{
	if ( %col.invulnerable )
	{
		return;
	}

	%vector = VectorNormalize (%vector);

	%colPos = %col.getPosition();
	%col.preHitVelocity = %col.getVelocity();

	%scale = getWord (%obj.getScale(), 2);

	%impulse = VectorScale (%vector, %this.impactImpulse);
	%impulse = VectorScale (%impulse, %scale);

	%verticalImpulse = VectorScale (%this.verticalImpulse, %scale);

	%col.applyImpulse (%colPos, %impulse);
	%col.applyImpulse (%colPos, "0 0" SPC  %verticalImpulse);

	if ( isObject(%obj.client) )
	{
		%col.lastPusher = %obj.client;
		%col.lastPushTime = getSimTime();
	}
}
