$DamageLava = 0.01;
$DamageHotLava = 0.01;
$DamageCrustyLava = 0.01;


// ======
//  Pain
// ======

function Player::lavaDamage ( %obj, %amt )
{
	%obj.damage (0, %obj.getPosition(), %amt, $DamageType::Lava);

	if ( isEventPending(%obj.lavaSchedule) )
	{
		cancel (%obj.lavaSchedule);
		%obj.lavaSchedule = 0;
	}

	%obj.lavaSchedule = %obj.schedule (300, lavaDamage, %amt);
}

function Player::playPain ( %obj )
{
	%client = %obj.client;
	%data = %obj.getDataBlock();

	if ( %data.useCustomPainEffects )
	{
		if ( isObject(%data.painSound) )
		{
			%obj.playAudio (0, %data.painSound);
		}
	}
	else
	{
		%obj.playAudio (0, PainCrySound);
	}
}


// =======
//  Death
// =======


function Player::kill ( %player, %client )
{
	if ( %damageType $= "" )
	{
		%damageType = $DamageType::Suicide;
	}

	%player.hasShotOnce = true;
	%player.invulnerable = false;

	%player.damage (%player, %player.getPosition(), 10000, %damageType);
}

function Player::playDeathAnimation ( %this )
{
	%this.setArmThread ("root");
	%this.playThread (3, "Death1");

	// TODO: remove stuff below return

	return;


	%this.deathIdx++;

	if ( %this.deathIdx + 1 > 11 )
	{
		%this.deathIdx = 1;
	}

	%this.setActionThread ("Death" @  %this.deathIdx);
}

function Player::playCelAnimation ( %this, %anim )
{
	if ( %this.getState() !$= "Dead" )
	{
		%this.setActionThread ("cel" @  %anim);
	}
}

function Player::playDeathCry ( %obj )
{
	%client = %obj.client;
	%data = %obj.getDataBlock();

	if ( %data.useCustomPainEffects )
	{
		if ( isObject(%data.deathSound) )
		{
			%obj.playAudio (0, %data.deathSound);
		}
	}
	else
	{
		%obj.playAudio (0, DeathCrySound);
	}
}

function Player::removeBody ( %obj )
{
	%p = new Projectile ()
	{
		dataBlock = deathProjectile;

		initialVelocity = "0 0 0";
		initialPosition = %obj.getTransform();

		sourceObject = %obj;
		sourceSlot = 0;

		client = %obj.client;
	};
	%p.setScale ( %obj.getScale() );
	MissionCleanup.add (%p);


	%client = %obj.client;

	if ( isObject(%client) )
	{
		if ( isObject(%client.light) )
		{
			if ( %client.light.player == %obj )
			{
				%client.light.delete();
			}
		}
	}

	%obj.schedule (10, delete);
}


function serverCmdSuicide ( %client )
{
	%player = %client.Player;

	if ( isObject(%player) )
	{
		%player.damage (%player, %player.getPosition(), 10000, $DamageType::Suicide);
	}
}
