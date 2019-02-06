function authTCPobj_Server::onLine ( %this, %line )
{
	if ( %this.done )
	{
		return;
	}

	%word = getWord (%line, 0);

	if ( %word $= "HTTP/1.1" )
	{
		%code = getWord (%line, 1);

		if ( %code != 200 )
		{
			warn ("WARNING: authTCPobj_Server - got non-200 http response " @  %code);
		}

		if ( %code >= 400  &&  %code <= 499 )
		{
			warn("WARNING: 4xx error on authTCPobj_Server, retrying");

			%this.schedule (0, disconnect);
			%this.schedule (500, connect, %this.site  @ ":" @  %this.port);
		}
		else if ( %code >= 300  &&  %code <= 399 )
		{
			warn("WARNING: 3xx error on authTCPobj_Server, will wait for location header");
		}
	}
	else if ( %word $= "Location:" )
	{
		%url = getWords (%line, 1);
		warn ("WARNING: authTCPobj_Server - Location redirect to " @  %url);

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
		warn ("WARNING: authTCPobj_Server - Content-Location redirect to " @  %url);

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
		%reason = getSubStr ( %line, 5, strlen (%line) - 5 );

		echo ("Authentication FAILED: " @  %reason);

		if ( %reason $= "ID not found." )
		{
			shutDown ("Authentication failed for server host.");
		}

		return;
	}
	else if ( %word $= "SUCCESS" )
	{
		%nr = getWord (%line, 1);

		if ( verifyNonsense(%nr) )
		{
			echo ("Authentication: SUCCESS");

			%pos = strpos (%line, " ", 0);
			%pos = strpos (%line, " ", %pos + 1) + 1;

			$pref::Player::NetName = getSubStr (%line, %pos + 1, strlen(%line) - %pos - 1);

			WebCom_PostServer();
			pingMatchMakerLoop();
		}
		else
		{
			echo ("Authentication FAILED: Version Error");
			echo ("Get the latest version at http://www.Blockland.us/");
			shutDown ("Authentication failed for server host.");
		}

		return;
	}
	else if ( %word $= "Set-Cookie:" )
	{
		%this.cookie = getSubStr (%line, 12, strlen(%line) - 12);
	}
	else if ( %word $= "PASSPHRASE" )
	{
		%passphrase = getWord (%line, 1);

		if ( getKeyID() !$= "" )
		{
			%crc = getPassPhraseResponse (%passphrase, %this.passPhraseCount);


			if ( %crc !$= "" )
			{
				echo ("Authentication: Sending Response...");

				%this.filePath = "/authConfirm2.php";

				if ( $NewNetName !$= "" )
				{
					%postText = "CRC=" @  %crc  @ "&NAME=" @  urlEnc($NewNetName);
				}
				else
				{
					%postText = "CRC=" @  %crc;
				}

				%postText = %postText @ "&DEDICATED=" @  $Server::Dedicated;
				%postText = %postText @ "&PORT="      @  $Server::Port;
				%postText = %postText @ "&VER="       @  $Version;
				%postText = %postText @ "&BUILD="     @  urlEnc ( getBuildNumber() );

				%this.postText = %postText;
				%this.postTextLen = strlen (%postText);


				%this.cmd = "POST " @  %this.filePath  @ " HTTP/1.0\r\n" @ 
					"Cookie: " @  %this.cookie  @ "\r\n" @ 
					"Host: " @  %this.site  @ "\r\n" @ 
					"User-Agent: Blockland-r" @  getBuildNumber()  @ "\r\n" @ 
					"Content-Type: application/x-www-form-urlencoded\r\n" @ 
					"Content-Length: " @  %this.postTextLen  @ "\r\n" @ 
					"\r\n" @ 
					%this.postText  @ "\r\n";


				%this.schedule (0, disconnect);
				%this.schedule (10, connect, authTCPobj_Server.site  @ ":" @  authTCPobj_Server.port);
			}

			%this.passPhraseCount++;
		}
		else
		{
			echo ("Authentication FAILED: No key found.");
			shutDown ("Authentication failed for server host.");

			return;
		}
	}
	else if ( %word $= "MATCHMAKER" )
	{
		%val = getWord (%line, 1);
		setMatchMakerIP (%val);
	}
	else if ( %word $= "MMTOK" )
	{
		%val = getWord (%line, 1);
		setMatchMakerToken (%val);
	}
	else if ( %word $= "PREVIEWURL" )
	{
		%val = getWord (%line, 1);
		setPreviewURL (%val);
	}
	else if ( %word $= "PREVIEWWORK" )
	{
		%val = getWord (%line, 1);
		setRayTracerWork (%val);
	}
	else if ( %word $= "CDNURL" )
	{
		%val = getWord (%line, 1);
		setCDNURL (%val);
	}
	else if ( %word $= "YOURIP" )
	{
		$MyTCPIPAddress = getWord (%line, 1);
	}
	else if ( %word $= "YOURBLID" )
	{
		%val = getWord (%line, 1);
		setMyBLID (%val);

		if ( $useSteam  &&  SteamEnabled() )
		{
			if ( $createServerAfterAuth )
			{
				initDedicated();
			}

			$createServerAfterAuth = 0;
		}
	}
	else if ( %word $= "NOTE" )
	{
		%val = getWords (%line, 1, 99);
		echo ("NOTE: " @  %val);
	}
	else if ( %word $= "TIMESTAMP" )
	{
		%val = getWord (%line, 1);
		setUTC (%val);
	}
}
