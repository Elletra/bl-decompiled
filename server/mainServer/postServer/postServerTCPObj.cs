function postServerTCPObj::onConnected ( %this )
{
	%this.send (%this.cmd);
}

function postServerTCPObj::onDNSFailed ( %this )
{
	echo ("Post to master server FAILED: DNS error. Retrying in 5 seconds...");

	%this.schedule (0, disconnect);
	schedule (5000, 0, "WebCom_PostServer");
}

function postServerTCPObj::onConnectFailed ( %this )
{
	echo("Post to master server FAILED: Connection failure.  Retrying in 5 seconds...");

	%this.schedule (0, disconnect);
	schedule (5000, 0, "WebCom_PostServer");
}

function postServerTCPObj::onDisconnect ( %this )
{
	// Your code here
}

function postServerTCPObj::onLine ( %this, %line )
{
	%word = getWord (%line, 0);

	if ( %word $= "HTTP/1.1" )
	{
		%code = getWord (%line, 1);

		if ( %code != 200 )
		{
			warn ("WARNING: postServerTCPObj - got non-200 http response " @  %code);
		}

		if ( %code >= 400  &&  %code <= 499 )
		{
			warn ("WARNING: 4xx error on postServerTCPObj, retrying");

			%this.schedule (0, disconnect);
			%this.schedule (500, connect, %this.site  @ ":" @  %this.port);
		}
		else if ( %code >= 300  &&  %code <= 399 )
		{
			warn ("WARNING: 3xx error on postServerTCPObj, will wait for location header");
		}
	}
	else if ( %word $= "Location:" )
	{
		%url = getWords (%line, 1);
		warn ("WARNING: postServerTCPObj - Location redirect to " @  %url);

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
		warn ("WARNING: postServerTCPObj - Content-Location redirect to " @  %url);

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
	else if ( %word $= "FAIL" )
	{
		%reason = getSubStr (%line, 5, 1000);

		if ( %reason $= "no host" )
		{
			echo ("No host entry in master server, re-sending authentication request");
			auth_Init_Server();
		}
		else if ( %reason $= "no user, no host" )
		{
			echo ("No user/host entry in master server, re-sending authentication request");
			auth_Init_Server();
		}
		else
		{
			echo ("Posting to master failed.  Reason: " @  %reason);
		}
	}
	else if ( %word $= "MMTOK" )
	{
		%val = getWord (%line, 1);
		setMatchMakerToken (%val);
	}
	else if ( %word $= "MATCHMAKER" )
	{
		%val = getWord (%line, 1);
		setMatchMakerIP (%val);
	}
	else if ( %word $= "NOTE" )
	{
		%val = getWords (%line, 1, 99);
		echo ("NOTE: " @  %val);
	}
}
