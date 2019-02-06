function Player::emote ( %player, %data, %skipSpam )
{
	if ( !isObject(%data) )
	{
		return;
	}

	if ( !%skipSpam )
	{
		if ( getSimTime() - %player.lastVoiceTime < 1000 )
		{
			%player.voiceCount++;
		}
		else if ( getSimTime() - %player.lastVoiceTime > 10000 )
		{
			%player.voiceCount = 0;
		}

		if ( %player.voiceCount > 5 )
		{
			return;
		}

		%player.lastVoiceTime = getSimTime();
	}

	if ( %data.getClassName() $= "ShapeBaseImageData" )
	{
		%player.mountImage (%data, 3);
	}
	else if ( %data.getClassName() $= "ProjectileData" )
	{
		%pos = %player.getEyePoint();
		%trans = %player.getTransform();

		%posX = getWord (%pos, 0);
		%posY = getWord (%pos, 1);
		%posZ = getWord (%pos, 2);

		%finalPos = %posX @ " " @ %posY @ " " @ %posZ;

		%p = new Projectile()
		{
			dataBlock = %data;

			initialVelocity = "0 0 1";
			initialPosition = %finalPos;

			sourceObject = %player;
			sourceSlot = 0;

			client = %client;
		};

		%p.setScale ( %player.getScale() );
	}
}
