function fxDTSBrick::colorVehicle ( %obj )
{
	if ( !isObject(%obj.Vehicle) )
	{
		return;
	}

	if ( !%obj.vehicleDataBlock.paintable )
	{
		return;
	}

	if ( fileName(%obj.Vehicle.getDataBlock().shapeFile) $= "m.dts" )
	{
		applyDefaultCharacterPrefs (%obj.Vehicle);
		return;
	}

	if ( %obj.reColorVehicle )
	{
		%RGB = getWords ( getColorIDTable(%obj.colorID), 0, 2 );
		%obj.Vehicle.setNodeColor ("ALL", %RGB  SPC 1);
	}
	else
	{
		if ( %obj.Vehicle.color $= "" )
		{
			%obj.Vehicle.color = "1 1 1 1";
		}

		%obj.Vehicle.setNodeColor ("ALL", %obj.Vehicle.color);
	}
}

function fxDTSBrick::unColorVehicle ( %obj )
{
	if ( !isObject(%obj.Vehicle) )
	{
		return;
	}

	%obj.Vehicle.setNodeColor ("ALL", "1 1 1 1");
}

function fxDTSBrick::setReColorVehicle ( %obj, %val )
{
	%obj.reColorVehicle = %val;

	if ( %val )
	{
		%obj.colorVehicle();
	}
	else
	{
		%obj.unColorVehicle();
	}

	if ( isObject(%obj.vehicleSpawnMarker) )
	{
		%obj.vehicleSpawnMarker.setData ( %obj.vehicleSpawnMarker.getUiName(),  %obj.reColorVehicle );
	}
}

function brickVehicleSpawnData::onColorChange ( %data, %obj )
{
	Parent::onColorChange (%data, %obj);
	%obj.colorVehicle();
}
