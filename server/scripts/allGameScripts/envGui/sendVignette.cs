function EnvGuiServer::sendVignetteAll ()
{
	%count = ClientGroup.getCount();

	for ( %i = 0;  %i < %count;  %i++ )
	{
		%client = ClientGroup.getObject (%i);
		EnvGuiServer::sendVignette (%client);
	}
}

function EnvGuiServer::sendVignette ( %client )
{
	if ( !isObject(%client) )
	{
		return;
	}

	if ( $EnvGuiServer::SimpleMode )
	{
		commandToClient (%client, 'setVignette', $Sky::VignetteMultiply, $Sky::VignetteColor);
	}
	else
	{
		commandToClient (%client, 'setVignette', $EnvGuiServer::VignetteMultiply, $EnvGuiServer::VignetteColor);
	}
}
