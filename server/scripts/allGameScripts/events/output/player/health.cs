function Player::addHealth ( %player, %amt )
{
	if ( %player.getDamagePercent() >= 1.0 )
	{
		return;
	}

	if ( %amt > 0.0 )
	{
		%player.setDamageLevel (%player.getDamageLevel() - %amt);
	}
	else
	{
		%player.Damage (%player, %player.getPosition(), %amt * -1, $DamageType::Default);
	}
}

function Player::setHealth ( %player, %health )
{
	if ( %player.getDamagePercent() >= 1.0 )
	{
		return;
	}

	if ( %health <= 0.0 )
	{
		%player.Damage (%player, %player.getPosition, %player.getDataBlock().maxDamage, $DamageType::Default);
	}
	else
	{
		%damageLevel = %player.getDataBlock().maxDamage - %health;

		if ( %damageLevel < 0.0 )
		{
			%damageLevel = 0;
		}

		%player.setDamageLevel (%damageLevel);
	}
}


// =================
//  Register Events
// =================

registerOutputEvent ("Player", "AddHealth", "int -1000 1000 25");
registerOutputEvent ("Player", "SetHealth", "int 0 1000 100");

registerOutputEvent ("Bot", "AddHealth", "int -1000 1000 25");
registerOutputEvent ("Bot", "SetHealth", "int 0 1000 100");
