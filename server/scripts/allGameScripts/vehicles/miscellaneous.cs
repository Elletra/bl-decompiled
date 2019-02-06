function Player::isPilot ( %this )
{
	%vehicle = %this.getObjectMount();

	if ( %vehicle )
	{
		if ( %vehicle.getMountNodeObject(0) == %this )
		{
			return true;
		}
	}

	return false;
}

function Player::onDriverLeave ( %obj, %player )
{
	%obj.getDataBlock().onDriverLeave (%obj, %player);
}

function PlayerData::onDriverLeave ( %obj, %player )
{
	// Your code here
}


function Vehicle::teleportEffect ( %vehicle )
{
	%p = new Projectile()
	{
		dataBlock = playerTeleportProjectile;
		initialPosition = %vehicle.getTransform();
	};

	if ( %p )
	{
		%scale = %vehicle.getScale();

		%minZ = getWord (%vehicle.getWorldBox(), 2);
		%maxZ = getWord (%vehicle.getWorldBox(), 5);

		%zsize = %maxZ - %minZ;

		%ratio = %zsize / 2.65;
		%scale = VectorScale (%vehicle.getScale(), %ratio);

		%p.setScale (%scale);
		MissionCleanup.add (%p);
	}
}

function Vehicle::onDriverLeave ( %obj, %player )
{
	%obj.getDataBlock().onDriverLeave (%obj, %player);
}

function VehicleData::onDriverLeave ( %this, %obj )
{
	// Your code here
}

function Vehicle::onRemove ( %obj )
{
	// Your code here
}

function Vehicle::onCollision ()
{
	echo ("vehicle on collision");
}

function VehicleData::onCollision ()
{
	echo ("vehicledata on collision");
}
