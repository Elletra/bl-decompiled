function fxDTSBrick::sendWrenchVehicleSpawnData ( %obj, %client )
{
	%data = "";
	%name = %obj.getName();

	if ( %name !$= "" )
	{
		%data = %data  TAB "N" SPC %name;
	}
	else
	{
		%data = %data  TAB "N" SPC " ";
	}

	if ( isObject(%obj.vehicleDataBlock) )
	{
		%db = %obj.vehicleDataBlock.getId();
	}
	else
	{
		%db = 0;
	}

	%data = %data  TAB "VDB" SPC %db;

	if ( %obj.reColorVehicle )
	{
		%val = 1;
	}
	else
	{
		%val = 0;
	}

	%data = %data  TAB "RCV" SPC %val;
	%data = %data  TAB "RC"  SPC %obj.isRayCasting();
	%data = %data  TAB "C"   SPC %obj.isColliding();
	%data = %data  TAB "R"   SPC %obj.isRendering();
	%data = trim (%data);

	commandToClient (%client, 'SetWrenchData', %data);
	commandToClient (%client, 'WrenchLoadingDone');
}
