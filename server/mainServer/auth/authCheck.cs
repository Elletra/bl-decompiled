function GameConnection::authCheck ( %client )
{
	if ( $Server::LAN )
	{
		%client.setPlayerName ("au^timoamyo7zene", %client.LANname);
		%client.name = %client.LANname;

		if ( %client.isLan() )
		{
			echo ("AUTHCHECK: " @  %client.getPlayerName()  @ " = LAN client -> LAN server, loading");

			%client.bl_id = getLAN_BLID();
			%client.setBLID ( "au^timoamyo7zene",  getLAN_BLID() );

			%client.startLoad (%client);

			return;
		}
		else
		{
			echo("AUTHCHECK: " @  %client.getPlayerName()  @ " = internet client -> LAN game, rejecting");
			%client.schedule(10, delete);
			return;
		}
	}
	else
	{
		%client.setPlayerName ("au^timoamyo7zene", %client.netName);
		%client.name = %client.netName;

		if ( %client.isLan() )
		{
			echo ("AUTHCHECK: " @  %client.getPlayerName()  @ " = LAN client -> internet server, auth with server ip");
			%useServerIP = 1;
		}
		else
		{
			echo ("AUTHCHECK: " @  %client.getPlayerName()  @ " = internet client -> internet server, regular auth");
			%useServerIP = 0;
		}
	}

	if ( isObject(%client.tcpObj) )
	{
		%client.tcpObj.delete();
	}

	%tcp = new TCPObject (servAuthTCPobj)
	{
		retryCount = 0;
		site = "auth.blockland.us";
		port = 80;
	};

	%postText = "NAME=" @  urlEnc ( %client.getPlayerName() );

	if ( !%useServerIP )
	{
		%postText = %postText  @ "&IP=" @  %client.getRawIP();
	}

	if ( !$useSteam )
	{
		%tcp.filePath = "/authQuery.php";
	}
	else
	{
		%postText = %postText  @ "&T=" @  %client.steamTicket;
		%tcp.filePath = "/authQuerySteam.php";
	}

	%tcp.postText = %postText;
	%tcp.postTextLen = strlen (%postText);

	%tcp.cmd = "POST " @  %tcp.filePath  @ " HTTP/1.0\r\n" @ 
		"Host: " @  %tcp.site  @ "\r\n" @ 
		"User-Agent: Blockland-r" @  getBuildNumber()  @ "\r\n" @ 
		"Content-Type: application/x-www-form-urlencoded\r\n" @ 
		"Content-Length: " @  %tcp.postTextLen  @ "\r\n" @ 
		"\r\n" @ 
		%tcp.postText  @ "\r\n";
	

	%tcp.connect(%tcp.site  @ ":" @  %tcp.port);

	%client.tcpObj = %tcp;
	%tcp.client = %client;
}
