function auth_Init_Client ()
{
	if ( $useSteam )
	{
		echo ("Starting client steam authentication...");
		MM_AuthText.setText ("Authentication: Connecting...");
	}
	else
	{
		%keyID = getKeyID ();

		if ( %keyID $= "" )
		{
			lock ();
			MM_UpdateDemoDisplay ();

			return;
		}

		echo ("Starting client authentication...");
		MM_AuthText.setText ("Authentication: Connecting...");
	}

	if ( isObject(authTCPobj_Client) )
	{
		authTCPobj_Client.delete ();
	}

	new TCPObject (authTCPobj_Client)
	{
		passPhraseCount = 0;
		site            = "auth.blockland.us";
		port            = 80;
		done            = false;
		success         = false;
	};

	if ( $useSteam )
	{
		authTCPobj_Client.filePath = "/authSteam.php";

		%postText = "T="      @ SteamGetAuthSessionTicket ();
		%postText = %postText @ "&SID=" @ getSteamId ();
		%postText = %postText @ "&N="   @ getNonsense (86);
		%postText = %postText @ "&VER=" @ $Version;

		if ( $NewNetName !$= "" )
		{
			%postText = %postText @ "&NAME=" @ urlEnc ($NewNetName);
		}

		%postText = %postText @ MM_AuthBar::getExtendedPostString ();
	}
	else
	{
		authTCPobj_Client.filePath = "/authInit.php";

		%postText = "ID="     @ %keyID;
		%postText = %postText @ "&N="   @ getNonsense (86);
		%postText = %postText @ "&VER=" @ $Version;
	}

	authTCPobj_Client.postText    = %postText;
	authTCPobj_Client.postTextLen = strlen (%postText);
	authTCPobj_Client.cmd         = "POST " @ authTCPobj_Client.filePath @ " HTTP/1.0\r\n" @ 
	                                "Host: " @ authTCPobj_Client.site @ "\r\n" @ 
	                                "User-Agent: Blockland-r" @ getBuildNumber () @ "\r\n" @ 
	                                "Content-Type: application/x-www-form-urlencoded\r\n" @ 
	                                "Content-Length: " @ authTCPobj_Client.postTextLen @ "\r\n" @ 
	                                "\r\n" @ authTCPobj_Client.postText @ "\r\n";

	authTCPobj_Client.connect (authTCPobj_Client.site @ ":" @ authTCPobj_Client.port);
}
