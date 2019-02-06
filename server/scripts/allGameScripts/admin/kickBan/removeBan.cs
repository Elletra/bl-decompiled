function BanManagerSO::RemoveBanBL_ID ( %this, %testBL_ID )
{
	for ( %i = 0;  %i < %this.numBans;  %i++ )
	{
		if ( %this.victimBL_ID[%i] == %testBL_ID )
		{
			%this.removeBan (%i);
			return;
		}
	}
}

function BanManagerSO::removeBan ( %this, %idx )
{
	if ( %this.numBans <= %idx )
	{
		error ("ERROR: BanManagerSO::RemoveBan() - ban index " @  %idx  @ 
			" does not exist. (there are only " @  %this.numBans  @ " bans)");

		return;
	}

	
	for ( %i = %idx + 1;  %i < %this.numBans;  %i++ )
	{
		%this.adminName[%i - 1] = %this.adminName[%i];
		%this.adminBL_ID[%i - 1] = %this.adminBL_ID[%i];

		%this.victimName[%i - 1] = %this.victimName[%i];
		%this.victimBL_ID[%i - 1] = %this.victimBL_ID[%i];
		%this.victimIP[%i - 1] = %this.victimIP[%i];

		%this.Reason[%i - 1] = %this.Reason[%i];

		%this.expirationYear[%i - 1] = %this.expirationYear[%i];
		%this.expirationMinute[%i - 1] = %this.expirationMinute[%i];
	}


	%i = %this.numBans;

	%this.adminName[%i] = "";
	%this.adminBL_ID[%i] = "";

	%this.victimName[%i] = "";
	%this.victimBL_ID[%i] = "";
	%this.victimIP[%i] = "";

	%this.Reason[%i] = "";

	%this.expirationYear[%i] = "";
	%this.expirationMinute[%i] = "";

	%this.numBans--;
}

function serverCmdUnBan ( %client, %idx )
{
	if ( !%client.isAdmin )
	{
		return;
	}

	$BanManagerSO.removeBan (%idx);
	$BanManagerSO.saveBans();
	$BanManagerSO.sendBanList (%client);
}
