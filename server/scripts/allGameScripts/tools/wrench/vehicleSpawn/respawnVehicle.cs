function serverCmdVehicleSpawn_Respawn ( %client, %data )
{
	%brick = %client.wrenchBrick;

	if ( !isObject(%brick) )
	{
		messageClient (%client, '', 'Wrench Error - VehicleSpawn_Respawn: Brick no longer exists!');
		return;
	}

	if ( isObject(%data)  &&  %brick.vehicleDataBlock != %data )
	{
		%brick.setVehicle (%data, %client);
		return;
	}

	%quotaObject = getQuotaObjectFromClient (%client);

	if ( !isObject(%quotaObject) )
	{
		error ("Error: serverCmdVehicleSpawn_Respawn() - new quota object creation failed!");
	}

	setCurrentQuotaObject (%quotaObject);
	%brick.spawnVehicle();
}
