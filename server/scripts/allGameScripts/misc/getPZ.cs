function serverCmdGetPZ ( %client )
{
	if ( !%client.isSuperAdmin )
	{
		return;
	}

	%player = %client.Player;
	%pos = %player.getPosition();

	%mask = $TypeMasks::PhysicalZoneObjectType;

	%radius = 10;

	initContainerRadiusSearch (%pos, %radius, %mask);

	while ( ( %searchObj = containerSearchNext() ) != 0 )
	{
		%searchObj = getWord (%searchObj, 0);
		messageClient (%client, '', "Got PZ: " @  %searchObj);
	}
}

function serverCmdRayPZ ( %client )
{
	if ( !%client.isSuperAdmin )
	{
		return;
	}


	%player = %client.Player;

	%start = %player.getEyePoint();

	%vec = VectorScale (%player.getEyeVector(), 10);
	%end = VectorAdd (%start, %vec);

	%mask = $TypeMasks::PhysicalZoneObjectType;

	%searchObj = containerRayCast (%start, %end, %mask, 0);
	%searchObj = getWord (%searchObj, 0);

	if ( %searchObj !$= "" )
	{
		messageClient (%client, '', "Ray PZ: " @  %searchObj);
	}
}
