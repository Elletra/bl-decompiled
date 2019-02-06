function Player::spawnExplosion ( %player, %projectileData, %scale, %client )
{
	if ( !isObject(%projectileData) )
	{
		return;
	}


	%pos = %player.getHackPosition();

	%p = new Projectile ()
	{
		dataBlock = %projectileData;

		initialVelocity = "0 0 1";
		initialPosition = %pos;

		sourceClient = %client;
		sourceObject = %player;

		client = %client;
	};

	if ( !isObject(%p) )
	{
		return;
	}

	MissionCleanup.add (%p);

	%p.setScale (%scale SPC %scale SPC %scale);
	%p.explode();
}

function Player::spawnProjectile ( %player, %speed, %projectileData, %variance, %scale, %client )
{
	if ( !isObject(%projectileData) )
	{
		return;
	}


	%velocity = VectorScale (%player.getEyeVector(), %speed);
	%pos = %player.getEyePoint();

	%velx = getWord (%velocity, 0);
	%vely = getWord (%velocity, 1);
	%velz = getWord (%velocity, 2);

	%varx = getWord (%variance, 0);
	%vary = getWord (%variance, 1);
	%varz = getWord (%variance, 2);

	%x = %velx + %varx * getRandom() - %varx / 2;
	%y = %vely + %vary * getRandom() - %vary / 2;
	%z = %velz + %varz * getRandom() - %varz / 2;

	%muzzleVelocity = %x SPC %y SPC %z;

	%p = new Projectile ()
	{
		dataBlock = %projectileData;

		initialVelocity = %muzzleVelocity;
		initialPosition = %pos;

		sourceClient = %client;
		sourceObject = %player;

		client = %client;
	};

	if ( %p )
	{
		MissionCleanup.add (%p);
		%p.setScale (%scale SPC %scale SPC %scale);
	}
}


// =================
//  Register Events
// =================

registerOutputEvent ("Player", "SpawnProjectile", "int -100 100 10" TAB "dataBlock ProjectileData" TAB 
	"vector 200" TAB "float 0.2 2 0.1 1");

registerOutputEvent ("Player", "SpawnExplosion", "dataBlock ProjectileData" TAB "float 0.2 2 0.1 1");


registerOutputEvent ("Bot", "SpawnProjectile", "int -100 100 10" TAB "dataBlock ProjectileData" TAB 
	"vector 200" TAB "float 0.2 2 0.1 1");

registerOutputEvent ("Bot", "SpawnExplosion", "dataBlock ProjectileData" TAB "float 0.2 2 0.1 1");
