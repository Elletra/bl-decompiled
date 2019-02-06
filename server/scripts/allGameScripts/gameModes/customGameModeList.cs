function serverCmdCustomGameGui_RequestList ( %client )
{
	if ( !%client.isAdmin )
	{
		return;
	}


	if ( $CustomGameGuiServer::AddOnCount <= 0 )
	{
		CustomGameGuiServer::populateAddOnList();
	}

	if ( $CustomGameGuiServer::MusicCount <= 0 )
	{
		CustomGameGuiServer::populateMusicList();
	}

	for ( %i = 0;  %i < $CustomGameGuiServer::AddOnCount;  %i++ )
	{
		%varName = $CustomGameGuiServer::AddOn[%i];
		commandToClient ( %client, 'CustomGameGui_AddAddOn', %varName, $AddOn__[%varName] );
	}

	for ( %i = 0;  %i < $CustomGameGuiServer::MusicCount;  %i++ )
	{
		%base = $CustomGameGuiServer::Music[%i];
		%varName = getSafeVariableName (%base);

		commandToClient ( %client, 'CustomGameGui_AddMusic', %base, $Music__[%varName] );
	}

	commandToClient (%client, 'CustomGameGui_AddAdvancedConfig', "$Pref::Server::Port", 
		$Pref::Server::Port);

	commandToClient (%client, 'CustomGameGui_AddAdvancedConfig', "$Pref::Server::GhostLimit", 
		$Pref::Server::GhostLimit);

	commandToClient (%client, 'CustomGameGui_AddAdvancedConfig', "$Pref::Server::MaxBricksPerSecond", 
		$Pref::Server::MaxBricksPerSecond);

	commandToClient (%client, 'CustomGameGui_AddAdvancedConfig', "$Pref::Server::TooFarDistance", 
		$Pref::Server::TooFarDistance);

	commandToClient (%client, 'CustomGameGui_AddAdvancedConfig', "$Pref::Server::RandomBrickColor", 
		$Pref::Server::RandomBrickColor);

	commandToClient (%client, 'CustomGameGui_AddAdvancedConfig', "$Pref::Server::MaxChatLen", 
		$Pref::Server::MaxChatLen);

	commandToClient (%client, 'CustomGameGui_AddAdvancedConfig', "$Pref::Server::ETardFilter", 
		$Pref::Server::ETardFilter);

	commandToClient (%client, 'CustomGameGui_AddAdvancedConfig', "$Pref::Server::FallingDamage", 
		$pref::Server::FallingDamage);

	commandToClient (%client, 'CustomGameGui_AddAdvancedConfig', "$Pref::Server::Quota::Schedules", 
		$Pref::Server::Quota::Schedules);

	commandToClient (%client, 'CustomGameGui_AddAdvancedConfig', "$Pref::Server::Quota::Misc", 
		$Pref::Server::Quota::Misc);

	commandToClient (%client, 'CustomGameGui_AddAdvancedConfig', "$Pref::Server::Quota::Projectile", 
		$Pref::Server::Quota::Projectile);

	commandToClient (%client, 'CustomGameGui_AddAdvancedConfig', "$Pref::Server::Quota::Item", 
		$Pref::Server::Quota::Item);

	commandToClient (%client, 'CustomGameGui_AddAdvancedConfig', "$Pref::Server::Quota::Environment", 
		$Pref::Server::Quota::Environment);

	commandToClient (%client, 'CustomGameGui_AddAdvancedConfig', "$Pref::Server::Quota::Player", 
		$Pref::Server::Quota::Player);

	commandToClient (%client, 'CustomGameGui_AddAdvancedConfig', "$Pref::Server::Quota::Vehicle", 
		$Pref::Server::Quota::Vehicle);

	commandToClient (%client, 'CustomGameGui_AddAdvancedConfig', "$Pref::Server::MaxPhysVehicles_Total", 
		$Pref::Server::MaxPhysVehicles_Total);

	commandToClient (%client, 'CustomGameGui_AddAdvancedConfig', "$Pref::Server::MaxPlayerVehicles_Total", 
		$Pref::Server::MaxPlayerVehicles_Total);

	commandToClient (%client, 'CustomGameGui_AddAdvancedConfig', "$Pref::Server::QuotaLAN::Schedules", 
		$Pref::Server::QuotaLAN::Schedules);

	commandToClient (%client, 'CustomGameGui_AddAdvancedConfig', "$Pref::Server::QuotaLAN::Misc", 
		$Pref::Server::QuotaLAN::Misc);

	commandToClient (%client, 'CustomGameGui_AddAdvancedConfig', "$Pref::Server::QuotaLAN::Projectile", 
		$Pref::Server::QuotaLAN::Projectile);

	commandToClient (%client, 'CustomGameGui_AddAdvancedConfig', "$Pref::Server::QuotaLAN::Item", 
		$Pref::Server::QuotaLAN::Item);

	commandToClient (%client, 'CustomGameGui_AddAdvancedConfig', "$Pref::Server::QuotaLAN::Environment", 
		$Pref::Server::QuotaLAN::Environment);

	commandToClient (%client, 'CustomGameGui_AddAdvancedConfig', "$Pref::Server::QuotaLAN::Player", 
		$Pref::Server::QuotaLAN::Player);

	commandToClient (%client, 'CustomGameGui_AddAdvancedConfig', "$Pref::Server::QuotaLAN::Vehicle", 
		$Pref::Server::QuotaLAN::Vehicle);

	commandToClient (%client, 'CustomGameGui_AddAdvancedConfig', "$Pref::Server::BrickPublicDomainTimeout", 
		$Pref::Server::BrickPublicDomainTimeout);


	commandToClient (%client, 'CustomGameGui_ListDone');
}


function serverCmdCustomGameGui_ListUploadDone ( %client )
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
				"Remote admins cannot change the custom game settings on listen servers");  // WHY?

			return;
		}
	}

	export ("$Music__*", "config/server/musicList.cs");
	export ("$AddOn__*", "config/server/ADD_ON_LIST.cs");

	export ("$Pref::Server::*", "config/server/prefs.cs", false);

	export ("$Pref::Net::PacketRateToClient", "config/server/prefs.cs", true);
	export ("$Pref::Net::PacketRateToServer", "config/server/prefs.cs", true);
	export ("$Pref::Net::PacketSize", "config/server/prefs.cs", true);
	export ("$Pref::Net::LagThreshold", "config/server/prefs.cs", true);
}
