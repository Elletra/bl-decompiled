function StaticShapeData::create ( %data )
{
	%obj = new StaticShape ()
	{
		dataBlock = %data;
	};

	return %obj;
}

function StaticShapeData::onAdd ( %this, %obj )
{
	%obj.setSkinName (%this.skinName);
}

function StaticShapeData::damage ( %this, %obj, %sourceObject, %position, %damage, %damageType )
{
	%obj.setDamageLevel (%obj.getDamageLevel() + %damage);

	if ( %obj.getDamageLevel() >= %this.maxDamage )
	{
		%obj.team.count[%this.pack]--;
		%trans = %obj.getTransform();

		%exp = new Explosion ()
		{
			dataBlock = %this.Explosion;
		};
		MissionCleanup.add (%exp);

		echo ("PLAYING EXPLOSION SOUND ", %this.explosionSound);

		ServerPlay3D (%this.explosionSound, %trans);

		%exp.setTransform (%trans);
		%obj.setTransform ("0 0 -999");

		%obj.delete();
	}
}

function StaticShape::explode ( %obj )
{
	%obj.setDamageState (destroyed);
	%obj.schedule (100, setHidden, 1);

	ServerPlay3D ( %obj.getDataBlock().explosionSound, %obj.getTransform() );

	// FIXME: remove stuff below return

	return;

	%data = %obj.getDataBlock();
	%pos = %obj.getWorldBoxCenter();

	%exp = new Explosion ()
	{
		dataBlock = spearExplosion;
		initialPosition = %pos;
	};
	MissionCleanup.add (%exp);

	%obj.setTransform ("0 0 -999");
	%obj.schedule (100, delete);

	MissionCleanup.add (%exp);
}
