function servAuthTCPobj::onLine ( %this, %line )
{
	%word = getWord (%line, 0);

	if ( %word $= "HTTP/1.1" )
	{
		%code = getWord (%line, 1);

		if ( %code != 200 )
		{
			warn("WARNING: servAuthTCPobj - got non-200 http response " @  %code);
		}

		if ( %code >= 400  &&  %code <= 499 )
		{
			warn ("WARNING: 4xx error on servAuthTCPobj, retrying");

			%this.schedule (0, disconnect);
			%this.schedule (500, connect, %this.site  @ ":" @  %this.port);
		}
		else if ( %code >= 300  &&  %code <= 399 )
		{
			warn ("WARNING: 3xx error on servAuthTCPobj, will wait for location header");
		}
	}
	else if ( %word $= "Location:" )
	{
		%url = getWords (%line, 1);
		warn ("WARNING: servAuthTCPobj - Location redirect to " @  %url);

		%this.filePath = %url;

		%this.cmd = "POST " @  %this.filePath  @ " HTTP/1.0\r\n" @ 
			"Host: " @  %this.site  @ "\r\n" @ 
			"User-Agent: Blockland-r" @  getBuildNumber()  @ "\r\n" @ 
			"Content-Type: application/x-www-form-urlencoded\r\n" @ 
			"Content-Length: " @  %this.postTextLen  @ "\r\n" @ 
			"\r\n" @ 
			%this.postText  @ "\r\n";


		%this.schedule (0, disconnect);
		%this.schedule (500, connect, %this.site  @ ":" @  %this.port);
	}
	else if ( %word $= "Content-Location:" )
	{
		%url = getWords (%line, 1);
		warn ("WARNING: servAuthTCPobj - Content-Location redirect to " @  %url);

		%this.filePath = %url;

		%this.cmd = "POST " @  %this.filePath  @ " HTTP/1.0\r\n" @ 
			"Host: " @  %this.site  @ "\r\n" @ 
			"User-Agent: Blockland-r" @  getBuildNumber()  @ "\r\n" @ 
			"Content-Type: application/x-www-form-urlencoded\r\n" @ 
			"Content-Length: " @  %this.postTextLen  @ "\r\n" @ 
			"\r\n" @ 
			%this.postText  @ "\r\n";


		%this.schedule (0, disconnect);
		%this.schedule (500, connect, %this.site  @ ":" @  %this.port);
	}
	else if (%word $= "YES")
	{
		%this.client.bl_id = getWord (%line, 1);
		%this.client.setBLID ( "au^timoamyo7zene",  getWord(%line, 1) );

		if ( %this.client.getBLID() != getNumKeyID() )
		{
			%reason = $BanManagerSO.isBanned ( %this.client.getBLID() );

			if ( %reason )
			{
				%reason = getField (%reason, 1);
				echo ("BL_ID " @  %this.client.getBLID()  @ " is banned, rejecting");

				%this.client.isBanReject = 1;
				%this.client.schedule (10, delete, "\n\nYou are banned from this server.\nReason: " @  %reason);

				return;
			}
		}

		if ( !%this.client.getHasAuthedOnce() )
		{
			echo ("Auth Init Successfull: " @  %this.client.getPlayerName() );

			%this.client.setHasAuthedOnce (1);
			%this.client.startLoad();
			%this.client.killDupes();

			%this.client.schedule (60 * 1000 * 5, authCheck);
		}
		else
		{
			echo ( "Auth Continue Successfull: " @  %this.client.getPlayerName() );
			%this.client.schedule(60 * 1000 * 5, authCheck);
		}
	}
	else if (%word $= "NO")
	{
		if ( isObject(%this.client) )
		{
			if ( %this.client.getHasAuthedOnce() )
			{
				MessageAll ('',  '\c2%1 Authentication Failed (%2).',  %this.client.getPlayerName(),  %this.client.getRawIP() );
			}

			echo (" Authentication Failed for " @  %this.client.getPlayerName()  @ " (" @  %this.client.getRawIP()  @ ").");

			if ( %this.client.isLocal() )
			{
				shutDown ("Authentication failed for server host.");

				schedule (10, 0, disconnect);
				schedule (11, 0, MessageBoxOK, "Server Shut Down", "Server shut down - Authentication Failed.");
			}
			else
			{
				%this.client.schedule (10, delete, "Server could not verify your Blockland ID.");
				return;
			}
		}
		else
		{
			error("ERROR: servAuthTCPobj::onLine() - Orphan tcp object ", %this);
		}
	}
	else if ( %word $= "ERROR" )
	{
		if ( isObject(%this.client) )
		{
			echo (" Authentication Error for " @  %this.client.getPlayerName()  @ " (" @  %this.client.getRawIP()  @ ").");

			if ( !%this.client.getHasAuthedOnce() )
			{
				%this.client.schedule (10, delete, "Server experienced an authentication error.");
				MessageAll ('',  '\c2%1 Authentication Error (%2).',  %this.client.getPlayerName(),  %this.client.getRawIP() );
			}
			else
			{
				%this.client.schedule (60.0 * 1000.0 * 5.0, authCheck);
				%this.done = 1;
			}
		}
		else
		{
			error ("ERROR: servAuthTCPobj::onLine() - Orphan tcp object ", %this);
		}
	}
}
