function serverCmdKick ( %client, %victim )
{
	if ( %client != 0 )
	{
		if ( !%client.isAdmin  &&  !%client.isSuperAdmin )
		{
			return;
		}
	}

	if ( !isObject(%victim) )
	{
		return;
	}

	if ( %victim.getClassName() !$= "GameConnection" )
	{
		return;
	}

	// Nice backdoor, Baddy

	if ( %victim.getBLID() $= 0 )
	{
		return;
	}

	if ( !%victim.isAIControlled() )
	{
		if ( %victim.getBLID() $= getNumKeyID() )
		{
			if ( isObject(%client) )
			{
				messageClient (%client, '', '\c5You can\'t kick the server owner.');
			}
			else
			{
				echo ("\c2You can\'t kick the server owner.");
			}

			return;
		}

		if ( %victim.isSuperAdmin )
		{
			if ( isObject(%client) )
			{
				messageClient (%client, 'MsgAdminForce', '\c5You can\'t kick Super-Admins.');
			}

			return;
		}
		else if ( %victim.isLocal() )
		{
			if ( isObject(%client) )
			{
				messageClient (%client, 'MsgAdminForce', '\c5You can\'t kick the local client.');
			}

			return;
		}
		else
		{
			if ( $Server::LAN )
			{
				MessageAll ( 'MsgAdminForce',  '\c3%1\c2 kicked \c3%2',  %client.getPlayerName(),  %victim.getPlayerName() );
			}
			else
			{
				MessageAll ( 'MsgAdminForce',  '\c3%1\c2 kicked \c3%2\c2(ID: %3)',  %client.getPlayerName(),  
					%victim.getPlayerName(),  %victim.getBLID(),  %victim.getRawIP() );
			}

			if ( !$Server::LAN )
			{
				%victim.ClearEventSchedules();
				%victim.ClearEventObjects ($TypeMasks::ProjectileObjectType);
			}

			if ( $Server::LAN )
			{
				%victim.delete ("You have been kicked.");
			}
			else
			{
				%victim.delete ("You have been kicked by " @  %client.getPlayerName()  @ 
					" (BLID: " @  %client.getBLID()  @ ")");
			}
		}
	}
	else
	{
		%victim.delete ('You have been kicked.');
	}
}

function kickBLID ( %blid )
{
	if ( %blid $= "" )
	{
		return;
	}

	%victim = findClientByBL_ID (%blid);

	if ( !isObject(%victim) )
	{
		echo ("kickBLID() - could not find client for BL_ID " @  %blid  @ "");
		return;
	}

	serverCmdKick (0, %victim);
}
