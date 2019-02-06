function miniGameCanDamage ( %client, %victimObject )
{
	%miniGame1 = getMiniGameFromObject (%client);
	%miniGame2 = getMiniGameFromObject (%victimObject);

	if ( %client.isBot )
	{
		if ( isObject(%client.spawnBrick) )
		{
			%minigameHost1 = %miniGame1.owner;
			%isHost1 = %client.spawnBrick.getGroup().client == %minigameHost1;
			%isIncluded1 = %miniGame1.UseAllPlayersBricks;
			%botNum1 = 1;
			%botCount++;
		}
		else
		{
			%otherBotCount++;
		}
	}

	if ( %victimObject.isBot )
	{
		if ( isObject(%victimObject.spawnBrick) )
		{
			%minigameHost2 = %miniGame2.owner;
			%isHost2 = %victimObject.spawnBrick.getGroup().client == %minigameHost2;
			%isIncluded2 = %miniGame2.UseAllPlayersBricks;
			%botNum2 = 1;
			%botCount++;
		}
		else
		{
			%otherBotCount++;
		}
	}

	%type = %victimObject.getType();

	if ( %miniGame2 != %miniGame1  &&  getBL_IDFromObject(%client) == getBL_IDFromObject(%victimObject) )
	{
		%doHack = true;

		if ( %victimObject.getType() & $TypeMasks::PlayerObjectType )
		{
			if ( %victimObject.getControllingClient() > 0 )
			{
				%doHack = false;
			}
		}

		if ( %doHack )
		{
			%miniGame2 = %miniGame1;
		}
	}

	if ( $Server::LAN )
	{
		if ( !isObject(%miniGame1) )
		{
			return true;
		}

		if ( %type & $TypeMasks::PlayerObjectType )
		{
			if ( %victimObject.isBot  ||  %client.isBot )
			{
				if ( %miniGame1 != %miniGame2 )
				{
					return false;
				}

				if ( %botCount == 2 )
				{
					if ( %isHost1  &&  %isHost2  ||  %isIncluded1  ||  %isIncluded2 )
					{
						return true;
					}
					else
					{
						return false;
					}
				}

				if ( %miniGame1.BotDamage )
				{
					%cIsPlayerVehicle = %client.getClassName() $= "AIPlayer"  &&  !%client.isBot;

					if ( %cIsPlayerVehicle  &&  !%miniGame1.VehicleDamage )
					{
						return false;
					}

					if ( %otherBotCount )
					{
						return true;
					}

					if ( %botCount == 2 )
					{
						if ( %isHost1  &&  %isHost2  ||  %isIncluded1  ||  %isIncluded2 )
						{
							return true;
						}
						else
						{
							return false;
						}
					}

					if ( %botNum1 )
					{
						%a = 1;

						if ( %isHost[%a]  ||  %isIncluded[%a] )
						{
							return true;
						}
						else
						{
							return false;
						}
					}

					if ( %botNum2 )
					{
						%a = 2;

						if ( %isHost[%a]  ||  %isIncluded[%a] )
						{
							return true;
						}
						else
						{
							return false;
						}
					}

					return true;
				}
				else
				{
					return false;
				}
			}
			else
			{
				if ( isObject(%victimObject.client) )
				{
					if ( %miniGame1 != %miniGame2 )
					{
						return false;
					}

					if ( %miniGame1.WeaponDamage )
					{
						return true;
					}
				}
				else
				{
					if ( %miniGame1.VehicleDamage )
					{
						return true;
					}
				}
			}
		}
		else
		{
			if ( %type & $TypeMasks::VehicleObjectType )
			{
				if ( %miniGame1.VehicleDamage )
				{
					return true;
				}
			}
			else
			{
				if ( %type & $TypeMasks::FxBrickAlwaysObjectType )
				{
					if ( %miniGame1.BrickDamage )
					{
						return true;
					}
				}
				else
				{
					if ( %miniGame1.WeaponDamage )
					{
						return true;
					}
				}
			}
		}

		return false;
	}

	if ( !isObject(%miniGame1)  &&  !isObject(%miniGame2) )
	{
		return -1;
	}

	if ( %miniGame1 != %miniGame2 )
	{
		return false;
	}

	if ( !isObject(%miniGame1) )
	{
		return false;
	}

	%ruleDamage = 0;

	if ( %type & $TypeMasks::PlayerObjectType )
	{
		if ( %victimObject.isBot  ||  %client.isBot )
		{
			if ( %botCount == 2 )
			{
				if ( %isHost1  &&  %isHost2  ||  %isIncluded1  ||  %isIncluded2 )
				{
					return true;
				}
				else
				{
					return false;
				}
			}

			if ( %miniGame1.BotDamage )
			{
				%cIsPlayerVehicle = %client.getClassName() $= "AIPlayer"  &&  !%client.isBot;

				if ( %cIsPlayerVehicle  &&  !%miniGame1.vehicleDamage )
				{
					return false;
				}

				if ( %otherBotCount )
				{
					return true;
				}

				if ( %botCount == 2 )
				{
					if ( %isHost1  &&  %isHost2  ||  %isIncluded1  ||  %isIncluded2 )
					{
						return true;
					}
					else
					{
						return false;
					}
				}

				if ( %botNum1 )
				{
					%a = 1;

					if ( %isHost[%a]  ||  %isIncluded[%a] )
					{
						return true;
					}
					else
					{
						return false;
					}
				}

				if ( %botNum2 )
				{
					%a = 2;

					if ( %isHost[%a]  ||  %isIncluded[%a] )
					{
						return true;
					}
					else
					{
						return false;
					}
				}

				return true;
			}
			else
			{
				return false;
			}
		}
		else
		{
			if ( isObject(%victimObject.client) )
			{
				if ( %miniGame1.weaponDamage )
				{
					if ( %victimObject.client == %client )
					{
						if ( %miniGame1.selfDamage )
						{
							return true;
						}
						else
						{
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
					return false;
				}
			}
			else if ( %miniGame1.VehicleDamage )
			{
				%ruleDamage = 1;
			}
		}
	}
	else
	{
		if ( %type & $TypeMasks::VehicleObjectType )
		{
			if ( %miniGame1.VehicleDamage )
			{
				%ruleDamage = 1;
			}
		}
		else
		{
			if ( %type & $TypeMasks::FxBrickAlwaysObjectType )
			{
				if ( %miniGame1.BrickDamage )
				{
					%ruleDamage = 1;
				}
			}
			else if ( %miniGame1.WeaponDamage )
			{
				%ruleDamage = 1;
			}
		}
	}

	if ( %ruleDamage == 0 )
	{
		return false;
	}

	if ( %miniGame1.useAllPlayersBricks )
	{
		return true;
	}
	else
	{
		%victimBL_ID = getBL_IDFromObject (%victimObject);

		if ( %victimBL_ID == %miniGame1.owner.getBLID() )
		{
			return true;
		}
	}

	return false;
}
