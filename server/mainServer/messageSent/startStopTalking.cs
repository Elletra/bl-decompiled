function serverCmdStartTalking ( %client )
{
	if ( %client.isTalking )
	{
		return;
	}


	%client.isTalking = 1;

	for ( %clientIndex = 0;  %clientIndex < ClientGroup.getCount();  %clientIndex++ )
	{
		%cl = ClientGroup.getObject (%clientIndex);
		messageClient (%cl, 'MsgStartTalking', '', %client);
	}
}

function serverCmdStopTalking ( %client )
{
	if ( !%client.isTalking )
	{
		return;
	}


	%client.isTalking = 0;
	
	for ( %clientIndex = 0;  %clientIndex < ClientGroup.getCount();  %clientIndex++ )
	{
		%cl = ClientGroup.getObject (%clientIndex);
		messageClient (%cl, 'MsgStopTalking', '', %client);
	}
}
