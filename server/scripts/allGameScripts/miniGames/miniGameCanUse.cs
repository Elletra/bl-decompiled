function miniGameCanUse ( %player, %thing )
{
	if ( $Server::LAN )
	{
		return true;
	}

	%miniGame1 = getMiniGameFromObject (%player);
	%miniGame2 = getMiniGameFromObject (%thing);

	if ( %miniGame2 != %miniGame1  &&  getBL_IDFromObject(%player) == getBL_IDFromObject(%thing) )
	{
		%doHack = true;

		if ( %thing.getType() & $TypeMasks::PlayerObjectType )
		{
			if ( %thing.getControllingClient() > 0 )
			{
				%doHack = false;
			}
		}

		if ( %doHack )
		{
			%miniGame2 = %miniGame1;
		}
	}

	if ( !isObject(%miniGame1)  &&  !isObject(%miniGame2) )
	{
		return -1;
	}

	if ( %miniGame1 != %miniGame2 )
	{
		$lastError = $LastError::MiniGameDifferent;
		return false;
	}

	if ( %thing.getType() & $TypeMasks::ItemObjectType )
	{
		if ( !isObject(%thing.spawnBrick) )
		{
			return true;
		}
	}

	%playerBL_ID = getBL_IDFromObject (%player);
	%thingBL_ID = getBL_IDFromObject (%thing);

	if ( %miniGame1.useAllPlayersBricks )
	{
		if ( %miniGame1.playersUseOwnBricks )
		{
			if ( %playerBL_ID == %thingBL_ID )
			{
				return true;
			}
			else
			{
				$lastError = $LastError::MiniGameNotYours;
				return false;
			}
		}
		else
		{
			return true;
		}
	}
	else
	{
		if ( %thing.client )
		{
			if ( %thing.client.player == %thing )
			{
				return true;
			}
		}

		%ownerBL_ID = %miniGame1.owner.getBLID();

		if ( %thingBL_ID == %ownerBL_ID )
		{
			return true;
		}
		else
		{
			$lastError = $LastError::NotInMiniGame;
			return false;
		}
	}
}
