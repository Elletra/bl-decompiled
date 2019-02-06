$Min::MaxPhysVehicles_Total = 0;
$Max::MaxPhysVehicles_Total = 20;
$Default::MaxPhysVehicles_Total = 10;

$Min::MaxPlayerVehicles_Total = 0;
$Max::MaxPlayerVehicles_Total = 200;
$Default::MaxPlayerVehicles_Total = 50;

$Min::Net::PacketRateToClient = 10;
$Max::Net::PacketRateToClient = 32;
$Default::Net::PacketRateToClient = 32;

$Min::Net::PacketRateToServer = 8;
$Max::Net::PacketRateToServer = 32;
$Default::Net::PacketRateToServer = 32;

$Min::Net::PacketSize = 200;
$Max::Net::PacketSize = 1023;
$Default::Net::PacketSize = 1023;

$Min::Net::LagThreshold = 200;
$Max::Net::LagThreshold = 600;
$Default::Net::LagThreshold = 400;

$Min::GhostLimit = 32768;
$Max::GhostLimit = 1048576;
$Default::GhostLimit = 262144;


$Default::Port = 28000;
$Default::BrickLimit = 256000;
$Default::MaxBricksPerSecond = 10;
$Default::TooFarDistance = 9999;
$Default::RandomBrickColor = 0;
$Default::MaxChatLen = 120;
$Default::ETardFilter = 1;
$Default::FallingDamage = 0;
$Default::BrickPublicDomainTimeout = -1;
$Default::BrickRespawnTime = 30000;
$Default::ClearEventsOnClientExit = 1;
$Default::WrenchEventsAdminOnly = 0;


exec ("./miscellaneous.cs");
exec ("./server/server.cs");
exec ("./mission.cs");
exec ("./findClient.cs");
exec ("./gameConnection/gameConnection.cs");
exec ("./emptyBrickGroups.cs");
