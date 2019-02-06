function serverCmdSetPreviewCenter ( %client )
{
	if ( !%client.isAdmin )
	{
		return;
	}

	if ( $Server::LAN == 1 )
	{
		commandToClient (%client, 'messageboxOK', "Fail", "Server preview images are only generated in internet games");
		return;
	}

	%obj = %client.getControlObject();

	if ( !isObject(%obj) )
	{
		return;
	}

	%pos = getWords (%obj.getTransform(), 0, 2);

	setRaytracerCenter (%pos);
	startRaytracer();

	messageClient (%client, '', "Preview image is now centered on " @  %pos);
}

function serverCmdClearPreviewCenter ( %client )
{
	if ( !%client.isAdmin )
	{
		return;
	}

	if ( $Server::LAN == 1 )
	{
		commandToClient (%client, 'messageboxOK', "Fail", "Server preview images are only generated in internet games");
		return;
	}

	setRaytracerAutoCenter();
	startRaytracer();

	messageClient (%client, '', "Preview image is auto-centered");
}
