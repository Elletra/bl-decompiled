function servAuthTCPobj::onDNSFailed ( %this )
{
	%this.retryCount++;
	%maxRetries = 3;

	if ( %this.retryCount > %maxRetries )
	{
		if ( %this.client.isLocal() )
		{
			error ("ERROR: - Authentication DNS Failed For Host.");
			%this.client.schedule (60 * 1000 * 5, authCheck);
		}
		else
		{
			error ("ERROR: - Authentication DNS Failed.");
			%this.client.schedule (60 * 1000 * 5, authCheck);
		}
	}
	else if ( %this.client.isLocal()  &&  !%this.client.getHasAuthedOnce() )
	{
		error ("ERROR: - Authentication DNS Failed when attempting to host.");
		MessageBoxOK ("Cannot Host Internet Game", "Authentication DNS Failed.", "disconnect();");
	}
	else
	{
		%this.schedule (0, disconnect);
		%this.schedule (10, connect, %this.site  @ ":" @  %this.port);

		error ("ERROR: - Authentication DNS Failed.  Retry ", %this.retryCount);
	}
}

function servAuthTCPobj::onConnectFailed ( %this )
{
	%this.retryCount++;
	%maxRetries = 5;

	if ( %this.client.isLocal()  &&  !%this.client.getHasAuthedOnce() )
	{
		%maxRetries = 3;
	}

	if ( %this.retryCount > %maxRetries )
	{
		if ( %this.client.isLocal() )
		{
			error ("ERROR: - Authentication Connnection Failed For Host.");
			%this.client.schedule (60 * 1000 * 5, authCheck);
		}
		else
		{
			error ("ERROR: - Authentication Connection Failed.");
			%this.client.schedule (60 * 1000 * 5, authCheck);
		}
	}
	else if ( %this.client.isLocal()  &&  !%this.client.getHasAuthedOnce() )
	{
		if ( %this.retryCount > 1 )
		{
			error ("ERROR: - Authentication Connection Failed when attempting to host.");
			MessageBoxOK ("Cannot Host Internet Game", "Authentication Connection Failed.", "disconnect();");
		}
		else
		{
			%this.schedule (0, disconnect);
			%this.schedule (10, connect, %this.site  @ ":" @  %this.port);

			error ("ERROR: - Authentication Connection Failed.  Retry ", %this.retryCount);
		}
	}
	else
	{
		%this.schedule (0, disconnect);
		%this.schedule (10, connect, %this.site  @ ":" @  %this.port);

		error ("ERROR: - Authentication Connection Failed.  Retry ", %this.retryCount);
	}
}

function servAuthTCPobj::onConnected ( %this )
{
	%this.send (%this.cmd);
}


exec ("./onLine.cs");
