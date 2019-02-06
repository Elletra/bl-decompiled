function serverCmdCustomGameGui_SetPref ( %client, %varName, %value )
{
	if ( !%client.isAdmin )
	{
		return;
	}

	if ( isListenServer() )
	{
		if ( !%client.isLocal() )
		{
			commandToClient (%client, 'MessageBoxOK', 'Settings Not Changed', 
				"Remote admins cannot change the custom game settings on listen servers");

			return;
		}
	}

	%intVal = mFloor (%value);

	%strVal = %value;
	%strVal = strreplace (%value, "\"", "");  // This is just a guess -- it used to be `strreplace (%value, "", "");`
	%strVal = strreplace (%value, ";", "");

	if ( %varName $= "$Pref::Server::Port" )
	{
		$Pref::Server::Port = %intVal;
	}
	else if ( %varName $= "$Pref::Server::GhostLimit" )
	{
		$Pref::Server::GhostLimit = %intVal;
	}
	else if ( %varName $= "$Pref::Server::MaxBricksPerSecond" )
	{
		$Pref::Server::MaxBricksPerSecond = %intVal;
	}
	else if ( %varName $= "$Pref::Server::TooFarDistance" )
	{
		$Pref::Server::TooFarDistance = %intVal;
	}
	else if ( %varName $= "$Pref::Server::RandomBrickColor" )
	{
		$Pref::Server::RandomBrickColor = %intVal;
	}
	else if ( %varName $= "$Pref::Server::MaxChatLen" )
	{
		$Pref::Server::MaxChatLen = %intVal;
	}
	else if ( %varName $= "$Pref::Server::ETardFilter" )
	{
		$Pref::Server::ETardFilter = %intVal;
	}
	else if ( %varName $= "$Pref::Server::FallingDamage" )
	{
		$pref::Server::FallingDamage = %intVal;
	}
	else if ( %varName $= "$Pref::Server::Quota::Schedules" )
	{
		$Pref::Server::Quota::Schedules = %intVal;
	}
	else if ( %varName $= "$Pref::Server::Quota::Misc" )
	{
		$Pref::Server::Quota::Misc = %intVal;
	}
	else if ( %varName $= "$Pref::Server::Quota::Projectile" )
	{
		$Pref::Server::Quota::Projectile = %intVal;
	}
	else if ( %varName $= "$Pref::Server::Quota::Item" )
	{
		$Pref::Server::Quota::Item = %intVal;
	}
	else if ( %varName $= "$Pref::Server::Quota::Environment" )
	{
		$Pref::Server::Quota::Environment = %intVal;
	}
	else if ( %varName $= "$Pref::Server::Quota::Player" )
	{
		$Pref::Server::Quota::Player = %intVal;
	}
	else if ( %varName $= "$Pref::Server::Quota::Vehicle" )
	{
		$Pref::Server::Quota::Vehicle = %intVal;
	}
	else if ( %varName $= "$Pref::Server::MaxPhysVehicles_Total" )
	{
		$Pref::Server::MaxPhysVehicles_Total = %intVal;
	}
	else if ( %varName $= "$Pref::Server::MaxPlayerVehicles_Total" )
	{
		$Pref::Server::MaxPlayerVehicles_Total = %intVal;
	}
	else if ( %varName $= "$Pref::Server::QuotaLAN::Schedules" )
	{
		$Pref::Server::QuotaLAN::Schedules = %intVal;
	}
	else if ( %varName $= "$Pref::Server::QuotaLAN::Misc" )
	{
		$Pref::Server::QuotaLAN::Misc = %intVal;
	}
	else if ( %varName $= "$Pref::Server::QuotaLAN::Projectile" )
	{
		$Pref::Server::QuotaLAN::Projectile = %intVal;
	}
	else if ( %varName $= "$Pref::Server::QuotaLAN::Item" )
	{
		$Pref::Server::QuotaLAN::Item = %intVal;
	}
	else if ( %varName $= "$Pref::Server::QuotaLAN::Environment" )
	{
		$Pref::Server::QuotaLAN::Environment = %intVal;
	}
	else if ( %varName $= "$Pref::Server::QuotaLAN::Player" )
	{
		$Pref::Server::QuotaLAN::Player = %intVal;
	}
	else if ( %varName $= "$Pref::Server::QuotaLAN::Vehicle" )
	{
		$Pref::Server::QuotaLAN::Vehicle = %intVal;
	}
	else if ( %varName $= "$Pref::Server::BrickPublicDomainTimeout" )
	{
		$Pref::Server::BrickPublicDomainTimeout = %intVal;
	}
	else if ( %varName $= "" )
	{
		return;
	}
	else
	{
		error ("ERROR: serverCmdCustomGameGui_SetPref () - " @  %client.getSimpleName()  @ 
			" tried to set invalid variable name " @  %varName  @ " to \'" @  %value  @ "\'");
	}
}
