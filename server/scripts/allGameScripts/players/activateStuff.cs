$Game::BrickActivateRange = 5;


function Player::activateStuff ( %player )
{
	%client = %player.client;

	if ( isObject(%client.miniGame) )
	{
		if ( %client.miniGame.WeaponDamage == 1 )
		{
			if ( getSimTime() - %client.lastF8Time < 5000 )
			{
				return 0;
			}
		}
	}

	if ( %player.getDamagePercent() >= 1.0 )
	{
		return 0;
	}


	%start = %player.getEyePoint();

	%vec = %player.getEyeVector();
	%scale = getWord (%player.getScale(), 2);

	%end = VectorAdd ( %start, VectorScale(%vec, 10  * %scale) );

	%mask = $TypeMasks::FxBrickObjectType | $TypeMasks::VehicleObjectType;

	if ( %player.isMounted() )
	{
		%exempt = %player.getObjectMount();
	}
	else
	{
		%exempt = %player;
	}


	%search = containerRayCast (%start, %end, %mask, %exempt);
	%victim = getWord (%search, 0);

	%currTime = getSimTime();

	if ( %currTime - %player.lastActivateTime <= 320 )
	{
		%player.activateLevel++;
	}
	else
	{
		%player.activateLevel = 0;
	}


	%player.lastActivateTime = getSimTime();

	if ( %player.activateLevel >= 5 )
	{
		%player.playThread (3, activate2);
	}
	else
	{
		%player.playThread (3, activate);
	}


	if ( %victim )
	{
		%pos = getWords (%search, 1, 3);

		if ( %victim.getType() & $TypeMasks::FxBrickObjectType )
		{
			%diff = VectorSub (%start, %pos);
			%len = VectorLen (%diff);

			if ( %len <= $Game::BrickActivateRange * %scale )
			{
				%victim.onActivate (%player, %client, %pos, %vec);
			}
		}
		else
		{
			%victim.onActivate (%player, %client, %pos, %vec);
		}

		return %victim;
	}
	else
	{
		return 0;
	}
}


function serverCmdActivateStuff ( %client )
{
	%player = %client.Player;

	if ( !isObject(%player) )
	{
		return;
	}

	%elapsedTime = getSimTime() - %client.lastActivateStuffTime;

	if ( %elapsedTime < 30 )
	{
		return;
	}

	%client.lastActivateStuffTime = getSimTime();
	%player.activateStuff();
}
