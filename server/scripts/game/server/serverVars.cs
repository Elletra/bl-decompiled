function copyPrefsToServerVars ()
{
	$Server::Name = $Pref::Server::Name;

	if ( $serverNameArg !$= "" )
	{
		$Server::Name = $serverNameArg;
	}

	echo ("Copying prefs to server variables");

	$Server::BrickRespawnTime        = $Pref::Server::BrickRespawnTime;
	$Server::ClearEventsOnClientExit = $Pref::Server::ClearEventsOnClientExit;

	$Server::MaxBricksPerSecond      = $Pref::Server::MaxBricksPerSecond;
	$Server::MaxPhysVehicles_Total   = $Pref::Server::MaxPhysVehicles_Total;
	$Server::MaxPlayerVehicles_Total = $Pref::Server::MaxPlayerVehicles_Total;

	$Server::Quota::Environment = $Pref::Server::Quota::Environment;
	$Server::Quota::Item        = $Pref::Server::Quota::Item;
	$Server::Quota::Misc        = $Pref::Server::Quota::Misc;
	$Server::Quota::Player      = $Pref::Server::Quota::Player;
	$Server::Quota::Projectile  = $Pref::Server::Quota::Projectile;
	$Server::Quota::Schedules   = $Pref::Server::Quota::Schedules;
	$Server::Quota::Vehicle     = $Pref::Server::Quota::Vehicle;

	$Server::QuotaLAN::Environment = $Pref::Server::QuotaLAN::Environment;
	$Server::QuotaLAN::Item        = $Pref::Server::QuotaLAN::Item;
	$Server::QuotaLAN::Misc        = $Pref::Server::QuotaLAN::Misc;
	$Server::QuotaLAN::Player      = $Pref::Server::QuotaLAN::Player;
	$Server::QuotaLAN::Projectile  = $Pref::Server::QuotaLAN::Projectile;
	$Server::QuotaLAN::Schedules   = $Pref::Server::QuotaLAN::Schedules;
	$Server::QuotaLAN::Vehicle     = $Pref::Server::QuotaLAN::Vehicle;

	$Server::WelcomeMessage        = $Pref::Server::WelcomeMessage;
	$Server::WrenchEventsAdminOnly = $Pref::Server::WrenchEventsAdminOnly;
	$Server::GhostLimit            = $Pref::Server::GhostLimit;
}

function setDefaultServerVars ()
{
	echo ("Setting default server variables");

	$Server::BrickRespawnTime        = $Default::BrickRespawnTime;
	$Server::ClearEventsOnClientExit = $Default::ClearEventsOnClientExit;

	$Server::MaxBricksPerSecond      = $Default::MaxBricksPerSecond;
	$Server::MaxPhysVehicles_Total   = $Default::MaxPhysVehicles_Total;
	$Server::MaxPlayerVehicles_Total = $Default::MaxPlayerVehicles_Total;

	$Server::Quota::Environment = $Default::Quota::Environment;
	$Server::Quota::Item        = $Default::Quota::Item;
	$Server::Quota::Misc        = $Default::Quota::Misc;
	$Server::Quota::Player      = $Default::Quota::Player;
	$Server::Quota::Projectile  = $Default::Quota::Projectile;
	$Server::Quota::Schedules   = $Default::Quota::Schedules;
	$Server::Quota::Vehicle     = $Default::Quota::Vehicle;

	$Server::QuotaLAN::Environment = $Default::QuotaLAN::Environment;
	$Server::QuotaLAN::Item        = $Default::QuotaLAN::Item;
	$Server::QuotaLAN::Misc        = $Default::QuotaLAN::Misc;
	$Server::QuotaLAN::Player      = $Default::QuotaLAN::Player;
	$Server::QuotaLAN::Projectile  = $Default::QuotaLAN::Projectile;
	$Server::QuotaLAN::Schedules   = $Default::QuotaLAN::Schedules;
	$Server::QuotaLAN::Vehicle     = $Default::QuotaLAN::Vehicle;

	$Server::WrenchEventsAdminOnly = $Default::WrenchEventsAdminOnly;
	$Server::GhostLimit            = $Default::GhostLimit;
}
