function validatePrefs ()
{
	// Quotas (LAN)

	$Pref::Server::QuotaLAN::Environment = verifyQuotaNumber ($Pref::Server::QuotaLAN::Environment, 
		$Min::QuotaLAN::Environment, $Max::QuotaLAN::Environment, $Default::QuotaLAN::Environment);

	$Pref::Server::QuotaLAN::Item = verifyQuotaNumber ($Pref::Server::QuotaLAN::Item, 
		$Min::QuotaLAN::Item, $Max::QuotaLAN::Item, $Default::QuotaLAN::Item);

	$Pref::Server::QuotaLAN::Misc = verifyQuotaNumber ($Pref::Server::QuotaLAN::Misc, 
		$Min::QuotaLAN::Misc, $Max::QuotaLAN::Misc, $Default::QuotaLAN::Misc);

	$Pref::Server::QuotaLAN::Player = verifyQuotaNumber ($Pref::Server::QuotaLAN::Player, 
		$Min::QuotaLAN::Player, $Max::QuotaLAN::Player, $Default::QuotaLAN::Player);

	$Pref::Server::QuotaLAN::Projectile = verifyQuotaNumber ($Pref::Server::QuotaLAN::Projectile, 
		$Min::QuotaLAN::Projectile, $Max::QuotaLAN::Projectile, $Default::QuotaLAN::Projectile);

	$Pref::Server::QuotaLAN::Schedules = verifyQuotaNumber ($Pref::Server::QuotaLAN::Schedules, 
		$Min::QuotaLAN::Schedules, $Max::QuotaLAN::Schedules, $Default::QuotaLAN::Schedules);

	$Pref::Server::QuotaLAN::Vehicle = verifyQuotaNumber ($Pref::Server::QuotaLAN::Vehicle, 
		$Min::QuotaLAN::Vehicle, $Max::QuotaLAN::Vehicle, $Default::QuotaLAN::Vehicle);


	// Quotas (Internet)

	$Pref::Server::Quota::Environment = verifyQuotaNumber ($Pref::Server::Quota::Environment, 
		$Min::Quota::Environment, $Max::Quota::Environment, $Default::Quota::Environment);

	$Pref::Server::Quota::Item = verifyQuotaNumber ($Pref::Server::Quota::Item, 
		$Min::Quota::Item, $Max::Quota::Item, $Default::Quota::Item);

	$Pref::Server::Quota::Misc = verifyQuotaNumber ($Pref::Server::Quota::Misc, 
		$Min::Quota::Misc, $Max::Quota::Misc, $Default::Quota::Misc);

	$Pref::Server::Quota::Player = verifyQuotaNumber ($Pref::Server::Quota::Player, 
		$Min::Quota::Player, $Max::Quota::Player, $Default::Quota::Player);

	$Pref::Server::Quota::Projectile = verifyQuotaNumber ($Pref::Server::Quota::Projectile, 
		$Min::Quota::Projectile, $Max::Quota::Projectile, $Default::Quota::Projectile);

	$Pref::Server::Quota::Schedules = verifyQuotaNumber ($Pref::Server::Quota::Schedules, 
		$Min::Quota::Schedules, $Max::Quota::Schedules, $Default::Quota::Schedules);

	$Pref::Server::Quota::Vehicle = verifyQuotaNumber ($Pref::Server::Quota::Vehicle, 
		$Min::Quota::Vehicle, $Max::Quota::Vehicle, $Default::Quota::Vehicle);


	// Max Vehicles

	$Pref::Server::MaxPlayerVehicles_Total = verifyQuotaNumber ($Pref::Server::MaxPlayerVehicles_Total, 
		$Min::MaxPlayerVehicles_Total, $Max::MaxPlayerVehicles_Total, $Default::MaxPlayerVehicles_Total);

	$Pref::Server::MaxPhysVehicles_Total = verifyQuotaNumber ($Pref::Server::MaxPhysVehicles_Total, 
		$Min::MaxPhysVehicles_Total, $Max::MaxPhysVehicles_Total, $Default::MaxPhysVehicles_Total);


	// Packet Settings

	$pref::Net::PacketRateToClient = verifyQuotaNumber ($pref::Net::PacketRateToClient, 
		$Min::Net::PacketRateToClient, $Max::Net::PacketRateToClient, $Default::Net::PacketRateToClient);

	$pref::Net::PacketRateToServer = verifyQuotaNumber ($pref::Net::PacketRateToServer, 
		$Min::Net::PacketRateToServer, $Max::Net::PacketRateToServer, $Default::Net::PacketRateToServer);

	$pref::Net::PacketSize = verifyQuotaNumber ($pref::Net::PacketSize, 
		$Min::Net::PacketSize, $Max::Net::PacketSize, $Default::Net::PacketSize);


	// Miscellaneous

	$Pref::Net::LagThreshold = verifyQuotaNumber ($Pref::Net::LagThreshold, 
		$Min::Net::LagThreshold, $Max::Net::LagThreshold, $Default::Net::LagThreshold);

	$Pref::Server::GhostLimit = verifyQuotaNumber ($Pref::Server::GhostLimit, 
		$Min::GhostLimit, $Max::GhostLimit, $Default::GhostLimit);
}
