$MiniGameLevel::None = 0;
$MiniGameLevel::Damage = 1;
$MiniGameLevel::Full = 2;


function getMiniGameLevel ( %obj1, %obj2 )
{
	%miniGame1 = getMiniGameFromObject (%obj1);
	%miniGame2 = getMiniGameFromObject (%obj2);

	if ( %miniGame1 != %miniGame2 )
	{
		return $MiniGameLevel::None;
	}

	%bl_id1 = getBL_IDFromObject (%obj1);
	%bl_id2 = getBL_IDFromObject (%obj2);
	%bl_idOwner = %miniGame1.owner.getBLID();

	if ( %miniGame1.useAllPlayersBricks )
	{
		if ( %miniGame1.playersUseOwnBricks )
		{
			if ( %bl_id1 == %bl_id2 )
			{
				return $MiniGameLevel::Full;
			}
			else
			{
				return $MiniGameLevel::Damage;
			}
		}
		else
		{
			return $MiniGameLevel::Full;
		}
	}
	else
	{
		if ( %obj1.getType() & $TypeMasks::PlayerObjectType )
		{
			if ( isObject(%obj1.client) )
			{
				%obj1RealPlayer = 1;
			}
		}

		if ( %obj2.getType() & $TypeMasks::PlayerObjectType )
		{
			if ( isObject(%obj2.client) )
			{
				%obj2RealPlayer = 1;
			}
		}

		if ( %obj1RealPlayer  &&  %obj2RealPlayer )
		{
			return $MiniGameLevel::Full;
		}
		else if ( %obj1RealPlayer )
		{
			if ( %bl_id2 == %bl_idOwner )
			{
				return $MiniGameLevel::Full;
			}
			else
			{
				return $MiniGameLevel::Damage;
			}
		}
		else if ( %obj2RealPlayer )
		{
			if ( %bl_id1 == %bl_idOwner )
			{
				return $MiniGameLevel::Full;
			}
			else
			{
				return $MiniGameLevel::Damage;
			}
		}
		else
		{
			if ( %bl_id1 == %bl_id2 )
			{
				return $MiniGameLevel::Full;
			}
			else
			{
				return $MiniGameLevel::None;
			}
		}
	}
}
