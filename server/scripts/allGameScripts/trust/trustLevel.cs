$TrustLevel::None = 0;
$TrustLevel::Build = 1;
$TrustLevel::Full = 2;
$TrustLevel::You = 3;

$TrustLevel::Paint = 2;
$TrustLevel::FXPaint = 2;
$TrustLevel::BuildOn = 1;
$TrustLevel::Print = 2;
$TrustLevel::UndoPaint = 2;
$TrustLevel::UndoFXPaint = 2;
$TrustLevel::UndoPrint = 2;
$TrustLevel::Wrench = 1;
$TrustLevel::Hammer = 2;
$TrustLevel::Wand = 3;
$TrustLevel::UndoBrick = 2;
$TrustLevel::DriveVehicle = 0;
$TrustLevel::RideVehicle = 0;
$TrustLevel::VehicleTurnover = 1;
$TrustLevel::ItemPickup = 1;
$TrustLevel::WrenchRendering = 2;
$TrustLevel::WrenchCollision = 2;
$TrustLevel::WrenchRaycasting = 2;
$TrustLevel::WrenchEvents = 2;


function GameConnection::getBL_IDTrustLevel ( %client, %targetBL_ID )
{
	if ( $Server::LAN )
	{
		return 2;
	}

	%brickGroup = %client.brickGroup;

	if ( !isObject(%brickGroup) )
	{
		error ("ERROR: GameConnection::getBL_IDTrustLevel() - " @  %client.getPlayerName()  @ 
			" (" @  %client  @ ") does not have a brick group.");

		MessageAll ('', "ERROR: GameConnection::getBL_IDTrustLevel() - " @  %client.getPlayerName()  @ 
			" (" @ %client @ ") does not have a brick group.");

		return 0;
	}

	%ourLevel = %brickGroup.trust[%targetBL_ID];

	if ( %ourLevel == 0 )
	{
		return 0;
	}

	%targetBrickGroup = "brickGroup_" @  %targetBL_ID;

	if ( !isObject(%targetBrickGroup) )
	{
		return 0;
	}

	%targetLevel = %targetBrickGroup.trust[ %client.getBLID() ];

	if ( %ourLevel == %targetLevel )
	{
		return %ourLevel;
	}
	else
	{
		return 0;
	}
}

function getTrustLevel ( %obj1, %obj2 )
{
	if ( !isObject(%obj1) )
	{
		error ("ERROR: getBL_IDfromObject() - " @  %obj1  @ " (%obj1) is not an object");
		return 0;
	}

	if ( !isObject(%obj2) )
	{
		error ("ERROR: getBL_IDfromObject() - " @  %obj2  @ " (%obj2) is not an object");
		return 0;
	}

	%brickGroup1 = getBrickGroupFromObject (%obj1);
	%bl_id1 = %brickGroup1.bl_id;

	%brickGroup2 = getBrickGroupFromObject (%obj2);
	%bl_id2 = %brickGroup2.bl_id;

	if ( %bl_id1 == %bl_id2 )
	{
		return $TrustLevel::You;
	}

	if ( %brickGroup1.isPublicDomain  ||  %brickGroup2.isPublicDomain )
	{
		return $TrustLevel::Full;
	}

	if ( %brickGroup1.trust[%bl_id2] != %brickGroup2.trust[%bl_id1] )
	{
		$lastError = $LastError::Trust;
		error ("ERROR: getTrustLevel() - trust levels between " @  %bl_id1  @ " and " @  %bl_id2  @ " are assymetrical.");
		return $TrustLevel::None;
	}
	else
	{
		$lastError = $LastError::Trust;
		return %brickGroup1.trust[%bl_id2];
	}
}
