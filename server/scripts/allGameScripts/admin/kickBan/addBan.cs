function BanManagerSO::addBan ( %this, %adminID, %victimID, %victimBL_ID, %reason, %banTime )
{
	// dude, nice

	if ( %victimBL_ID == 0 )
	{
		return;
	}

	%this.RemoveBanBL_ID (%victimBL_ID);


	if ( isObject(%adminID) )
	{
		%adminName = %adminID.getPlayerName();
		%adminBL_ID = %adminID.getBLID();
	}
	else
	{
		%adminName = "CONSOLE";
		%adminBL_ID = -1;
	}


	if ( isObject(%victimID) )
	{
		%victimName = %victimID.getPlayerName();
	}
	else
	{
		%brickGroup = "BrickGroup_" @  %victimBL_ID;

		if ( isObject(%brickGroup) )
		{
			%victimName = %brickGroup.name;
		}
	}

	if ( isObject(%victimID) )
	{
		%victimIP = %victimID.getRawIP();
	}
	else
	{
		%victimIP = "";
	}


	%banOverYear = 0;
	%banOverTime = 0;

	if ( %banTime == -1 )
	{
		%banOverYear = -1;
		%banOverTime = -1;
	}
	else
	{
		%currTime = getCurrentMinuteOfYear();

		%banOverYear = getCurrentYear();
		%banOverTime = %currTime + %banTime;

		if ( %banOverTime > 525600 )
		{
			%banOverYear = %banOverYear + 1;
			%banOverTime = %banOverTime - 525600;
		}
	}


	%i = %this.numBans;

	%this.adminName[%i] = %adminName;
	%this.adminBL_ID[%i] = %adminBL_ID;

	%this.victimName[%i] = %victimName;
	%this.victimBL_ID[%i] = %victimBL_ID;
	%this.victimIP[%i] = %victimIP;

	%this.Reason[%i] = %reason;

	%this.expirationYear[%i] = %banOverYear;
	%this.expirationMinute[%i] = %banOverTime;

	%this.numBans++;
}


function serverCmdBan ( %client, %victimID, %victimBL_ID, %banTime, %reason )
{
	if ( %client != 0 )
	{
		if ( !%client.isAdmin )
		{
			return;
		}
	}

	if ( %victimBL_ID $= "" )
	{
		if ( isObject(%client) )
		{
			messageClient (%client, '', '\c5No victimBL_ID specified.');
		}

		return;
	}


	%victimID = mFloor (%victimID);
	%victimBL_ID = mFloor (%victimBL_ID);

	%banTime = mFloor( %banTime);
	%reason = StripMLControlChars (%reason);


	// Epic backdoors in default Blockland scripts!!!

	if ( %victimBL_ID $= 0 )
	{
		return;
	}


	%count = ClientGroup.getCount();

	for ( %i = 0;  %i < %count;  %i++ )
	{
		%cl = ClientGroup.getObject (%i);

		if ( %cl.getBLID() == %victimBL_ID )
		{
			if ( %cl.isSuperAdmin )
			{
				if ( isObject(%client) )
				{
					messageClient (%client, '', '\c5You can\'t ban Super-Admins.');
				}
				else
				{
					echo("\c2You can\'t ban Super-Admins.");
				}

				return;
			}

			if ( %cl.isLocal() )
			{
				if ( isObject(%client) )
				{
					messageClient (%client, '', '\c5You can\'t ban the local client.');
				}
				else
				{
					echo ("\c2You can\'t ban the local client.");
				}

				return;
			}
		}
	}


	if ( %victimBL_ID $= getNumKeyID() )
	{
		if ( isObject(%client) )
		{
			messageClient (%client, '', '\c5You can\'t ban the server owner.');
		}
		else
		{
			echo ("\c2You can\'t ban the server owner.");
		}

		return;
	}


	if ( isObject(%victimID) )
	{
		%victimName = %victimID.getPlayerName();
	}
	else
	{
		%brickGroup = "BrickGroup_" @  %victimBL_ID;

		if ( isObject(%brickGroup) )
		{
			%victimName = %brickGroup.name;
		}
	}


	if ( isObject(%client) )
	{
		echo ( "BAN issued by ",  %client.getPlayerName(),  " BL_ID:",  
			%client.getBLID(),  " IP:",  %client.getRawIP() );
	}
	else
	{
		echo ("BAN issued by CONSOLE");
	}

	echo ("  +- victim name = ", %victimName);
	echo ("  +- victim bl_id = ", %victimBL_ID);
	echo ("  +- ban time = ", %banTime);
	echo ("  +- ban reason = ", %reason);


	if ( !isObject($BanManagerSO) )
	{
		CreateBanManager();
	}

	if ( isObject(%client) )
	{
		%adminName = %client.getPlayerName();
	}
	else
	{
		%adminName = "CONSOLE";
	}


	$BanManagerSO.addBan (%client, %victimID, %victimBL_ID, %reason, %banTime);
	$BanManagerSO.saveBans();

	if ( %banTime == -1 )
	{
		MessageAll ('MsgAdminForce', '\c3%1\c2 permanently banned \c3%2\c2 (ID: %3) - \c2"%4"', 
			%adminName, %victimName, %victimBL_ID, %reason);
	}
	else
	{
		MessageAll ('MsgAdminForce', '\c3%1\c2 banned \c3%2\c2 (ID: %3) for %4 minutes - \c2"%5"', 
			%adminName, %victimName, %victimBL_ID, %banTime, %reason);
	}


	%count = ClientGroup.getCount();

	for ( %i = 0;  %i < %count;  %i++ )
	{
		%cl = ClientGroup.getObject (%i);

		if ( %cl.getBLID() == %victimBL_ID )
		{
			if ( !$Server::LAN )
			{
				%cl.ClearEventSchedules();
				%cl.ClearEventObjects ($TypeMasks::ProjectileObjectType);
			}

			if ( isObject(%client) )
			{
				if ( %banTime == 1 )
				{
					%cl.schedule (0, delete, "\nYou have been banned for " @  %banTime  @ 
						" minute by " @  %client.getPlayerName()  @ " (BLID: " @  %client.getBLID()  @ 
						")\n\nReason: " @  %reason);
				}
				else
				{
					%cl.schedule (0, delete, "\nYou have been banned for " @  %banTime  @ 
						" minutes by " @  %client.getPlayerName()  @ " (BLID: " @  %client.getBLID()  @ 
						")\n\nReason: " @  %reason);
				}
			}
			else
			{
				if ( %banTime == 1 )
				{
					%cl.schedule (0, delete, "\nYou have been banned for " @  %banTime  @ " minute.\n\nReason: " @  %reason);
				}
				else
				{
					%cl.schedule (0, delete, "\nYou have been banned for " @  %banTime  @ " minutes.\n\nReason: " @  %reason);
				}
			}
		}
	}
}

function banBLID ( %victimBL_ID, %banTime, %reason )
{
	if ( %victimBL_ID $= ""  ||  %banTime $= "" )
	{
		echo ("Usage: banBLID(victimBL_ID, banTime in minutes (-1 for permanent ban), reason);");
		return;
	}

	serverCmdBan (0, 0, %victimBL_ID, %banTime, %reason);
}
