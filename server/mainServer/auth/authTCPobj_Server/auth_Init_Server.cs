function auth_Init_Server ()
{
	if ( $useSteam )
	{
		echo ("Starting server steam authentication...");
	}
	else
	{
		%keyID = getKeyID();

		if ( %keyID $= "" )
		{
			lock();
			MM_UpdateDemoDisplay();

			return;
		}

		echo ("Starting server authentication...");
	}

	if ( isObject(authTCPobj_Server) )
	{
		authTCPobj_Server.delete();
	}

	new TCPObject (authTCPobj_Server)
	{
		passPhraseCount = 0;
		site = "auth.blockland.us";
		port = 80;
		done = false;
	};

	if ( $useSteam )
	{
		authTCPobj_Server.filePath = "/authSteam.php";

		%postText = "T=" @  SteamGetAuthSessionTicket();

		%postText = %postText  @ "&SID="       @  getSteamId();
		%postText = %postText  @ "&N="         @  getNonsense (86);
		%postText = %postText  @ "&DEDICATED=" @  $Server::Dedicated;
		%postText = %postText  @ "&PORT="      @  $Server::Port;
		%postText = %postText  @ "&VER="       @  $Version;
		%postText = %postText  @ "&BUILD="     @  urlEnc ( getBuildNumber() );
	}
	else
	{
		authTCPobj_Server.filePath = "/authInit.php";

		%postText = "ID=" @  %keyID;

		%postText = %postText  @ "&N="     @  getNonsense (86);
		%postText = %postText  @ "&VER="   @  $Version;
		%postText = %postText  @ "&BUILD=" @  urlEnc ( getBuildNumber() );
	}


	authTCPobj_Server.postText = %postText;
	authTCPobj_Server.postTextLen = strlen (%postText);

	authTCPobj_Server.cmd = "POST " @  authTCPobj_Server.filePath  @ " HTTP/1.0\r\n" @ 
		"Host: " @  authTCPobj_Server.site  @ "\r\n" @ 
		"User-Agent: Blockland-r" @  getBuildNumber()  @ "\r\n" @ 
		"Content-Type: application/x-www-form-urlencoded\r\n" @ 
		"Content-Length: " @  authTCPobj_Server.postTextLen  @ "\r\n" @ 
		"\r\n" @ 
		authTCPobj_Server.postText  @ "\r\n";


	authTCPobj_Server.connect (authTCPobj_Server.site  @ ":" @  authTCPobj_Server.port);
	authTCPobj_Server.passPhraseCount = 0;
}
