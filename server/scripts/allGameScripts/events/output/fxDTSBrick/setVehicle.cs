datablock MissionMarkerData (VehicleSpawnMarkerData)
{
	category = "Misc";
	shapeFile = "~/data/shapes/markers/octahedron.dts";
};


function VehicleSpawnMarker::onAdd ( %obj )
{
	%vehicleClass = %obj.vehicleDataBlock.getClassName();

	if ( %vehicleClass $= "WheeledVehicleData"  ||  %vehicleClass $= "HoverVehicleData"  ||  
		 %vehicleClass $= "FlyingVehicleData" )
	{
		if ( !$Server::LAN )
		{
			%brickGroup = %obj.brick.getGroup();
			%brickGroup.numPhysVehicles++;
		}

		$Server::numPhysVehicles++;
	}
	else if ( %vehicleClass $= "PlayerData"  ||  %vehicleClass $= "AIPlayerData" )
	{
		if ( !$Server::LAN )
		{
			%brickGroup = %obj.brick.getGroup();
			%brickGroup.numPlayerVehicles++;
		}

		$Server::numPlayerVehicles++;
	}
}

function VehicleSpawnMarker::onRemove ( %obj )
{
	%vehicleClass = %obj.vehicleDataBlock.getClassName();

	if ( %vehicleClass $= "WheeledVehicleData"  ||  %vehicleClass $= "HoverVehicleData"  ||  
		 %vehicleClass $= "FlyingVehicleData")
	{
		if ( !$Server::LAN )
		{
			%brickGroup = %obj.brick.getGroup();
			%brickGroup.numPhysVehicles--;
		}

		$Server::numPhysVehicles--;
	}
	else if (%vehicleClass $= "PlayerData" || %vehicleClass $= "AIPlayerData")
	{
		if ( !$Server::LAN )
		{
			%brickGroup = %obj.brick.getGroup();
			%brickGroup.numPlayerVehicles--;
		}

		$Server::numPlayerVehicles--;
	}
}


function fxDTSBrick::setVehicle ( %obj, %data, %client )
{
	if ( %obj.vehicleDataBlock == %data )
	{
		return;
	}

	if ( isObject(%obj.Vehicle) )
	{
		%obj.Vehicle.delete();
	}

	%obj.Vehicle = 0;

	if ( isObject(%obj.vehicleSpawnMarker) )
	{
		%obj.vehicleSpawnMarker.delete();
	}

	%obj.vehicleSpawnMarker = 0;

	if ( %obj.getDataBlock().specialBrickType !$= "VehicleSpawn" )
	{
		return;
	}


	if ( !isObject(%data) )
	{
		%obj.vehicleDataBlock = 0;
		return;
	}

	if ( %data.getClassName() !$= "PlayerData"  &&  %data.getClassName() !$= "WheeledVehicleData"  &&  
		 %data.getClassName() !$= "FlyingVehicleData"  &&  %data.getClassName() !$= "HoverVehicleData" )
	{
		return;
	}

	if ( !%data.rideAble )
	{
		return;
	}

	if ( %data.uiName $= "" )
	{
		return;
	}

	if ( %data.getClassName() $= "PlayerData" )
	{
		%brickGroup = %obj.getGroup();

		if ( !$Server::LAN  &&  %brickGroup.numPlayerVehicles >= $Server::Quota::Player )
		{
			if ( %client.brickGroup == %brickGroup )
			{
				if ( $Server::Quota::Player == 1 )
				{
					commandToClient (%client, 'centerPrint', "\c0You already have a player-vehicle", 2);
				}
				else
				{
					commandToClient (%client, 'centerPrint', "\c0You already have " @  $Server::Quota::Player  
						@ " player-vehicles", 2);
				}
			}
			else
			{
				if ( $Server::Quota::Player == 1 )
				{
					commandToClient (%client, 'centerPrint', "\c0" @  %brickGroup.name  @ " already has a player-vehicle", 2);
				}
				else
				{
					commandToClient (%client, 'centerPrint', "\c0" @  %brickGroup.name  @ " already has " @  
						$Server::Quota::Player @ " player-vehicles", 2);
				}
			}

			return;
		}

		if ( $Server::numPlayerVehicles >= $Server::MaxPlayerVehicles_Total )
		{
			if ( $Server::MaxPlayerVehicles_Total == 1 )
			{
				commandToClient (%client, 'centerPrint', "\c0Server is limited to 1 player-vehicle", 2);
			}
			else
			{
				commandToClient (%client, 'centerPrint', "\c0Server is limited to " @  
					$Server::MaxPlayerVehicles_Total  @ " player-vehicles", 2);
			}
			return;
		}
	}
	else
	{
		%brickGroup = %obj.getGroup();

		if ( !$Server::LAN  &&  %brickGroup.numPhysVehicles >= $Server::Quota::Vehicle )
		{
			if ( %client.brickGroup == %brickGroup )
			{
				if ( $Server::Quota::Vehicle == 1 )
				{
					commandToClient (%client, 'centerPrint', "\c0You already have a physics-vehicle", 2);
				}
				else
				{
					commandToClient (%client, 'centerPrint', "\c0You already have " @  
						$Server::Quota::Vehicle  @ " physics-vehicles", 2);
				}
			}
			else
			{
				if ( $Server::Quota::Vehicle == 1 )
				{
					commandToClient (%client, 'centerPrint', "\c0" @  %brickGroup.name  @ 
						" already has a physics-vehicle", 2);
				}
				else
				{
					commandToClient (%client, 'centerPrint', "\c0" @  %brickGroup.name  @ " already has " @  
						$Server::Quota::Vehicle  @ " physics-vehicles", 2);
				}
			}

			return;
		}


		$Server::MaxPhysVehicles_Total = mClamp ($Server::MaxPhysVehicles_Total, 0, $Max::MaxPhysVehicles_Total);

		if ( $Server::numPhysVehicles >= $Server::MaxPhysVehicles_Total )
		{
			if ( $Server::MaxPhysVehicles_Total == 1 )
			{
				commandToClient (%client, 'centerPrint', "\c0Server is limited to 1 physics-vehicle", 2);
			}
			else
			{
				commandToClient (%client, 'centerPrint', "\c0Server is limited to " @  $Server::MaxPhysVehicles_Total  
					@ " physics-vehicles", 2);
			}

			return;
		}
	}


	%obj.vehicleDataBlock = %data;
	%obj.spawnVehicle();

	%oldQuotaObject = getCurrentQuotaObject();

	if ( isObject(%oldQuotaObject) )
	{
		clearCurrentQuotaObject();
	}

	if ( %obj.reColorVehicle $= "" )
	{
		%obj.reColorVehicle = 1;
	}

	%obj.vehicleSpawnMarker = new VehicleSpawnMarker ()
	{
		dataBlock = VehicleSpawnMarkerData;
		uiName = %data.uiName;
		reColorVehicle = %obj.reColorVehicle;
		vehicleDataBlock = %data;
		brick = %obj;
	};
	MissionCleanup.add (%obj.vehicleSpawnMarker);

	%obj.vehicleSpawnMarker.setTransform ( %obj.getTransform() );


	if ( isObject(%oldQuotaObject) )
	{
		setCurrentQuotaObject (%oldQuotaObject);
	}
}

function fxDTSBrick::setVehiclePowered ( %obj, %on, %client )
{
	%vehicle = %obj.Vehicle;

	if ( !isObject(%vehicle) )
	{
		return;
	}


	%data = %vehicle.getDataBlock();

	for ( %i = 0;  %i < %data.numWheels;  %i++ )
	{
		%vehicle.setWheelPowered (%i, %on);
	}

	%vehicle.poweredTime = getSimTime();
}


// =================
//  Register Events
// =================

registerOutputEvent ("fxDTSBrick", "setVehicle", "dataBlock Vehicle");
registerOutputEvent ("fxDTSBrick", "setVehiclePowered", "bool");
