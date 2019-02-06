function SimObject::clearEvents ( %obj )
{
	for ( %i = 0;  %i < %obj.numEvents;  %i++ )
	{
		%obj.eventDelay[%i] = "";
		%obj.eventCmd[%i] = "";
	}

	%obj.numEvents = 0;
	%obj.implicitCancelEvents = 0;
}


function serverCmdClearEvents ( %client )
{
	%brick = %client.wrenchBrick;

	if ( %brick == 0 )
	{
		return;
	}

	if ( !isObject(%brick) )
	{
		messageClient (%client, '', 'Wrench Error - ClearEvents: Brick no longer exists!');
		return;
	}

	if ( getTrustLevel(%client, %brick) < $TrustLevel::WrenchEvents )
	{
		%client.sendTrustFailureMessage ( %brick.getGroup() );
		return;
	}

	%brick.clearEvents();
}
