function auth_Init()
{
	%keyID = getKeyID();
	if (%keyID $= "")
	{
		echo("***AUTHENTICATION ERROR: Stored key could not be found.");
	}
	echo("Authentication: Sending Initial Request...");
	if (isObject(authTCPobj))
	{
		authTCPobj.delete();
	}
	new TCPObject(authTCPobj);
	authTCPobj.site = "master.blockland.us";
	authTCPobj.port = 80;
	authTCPobj.filePath = "/auth/authInit.asp";
	authTCPobj.done = "false";
	%postText = "ID=" @ %keyID;
	%postTexLen = strlen(%postText);
	authTCPobj.cmd = "POST " @ authTCPobj.filePath @ " HTTP/1.1\r\nHost: " @ authTCPobj.site @ "\r\nContent-Type: application/x-www-form-urlencoded\r\nContent-Length: " @ %postTexLen @ "\r\n\r\n" @ %postText @ "\r\n";
	authTCPobj.connect(authTCPobj.site @ ":" @ authTCPobj.port);
	authTCPobj.passPhraseCount = 0;
}

function authTCPobj::onDNSFailed(%this)
{
	echo("Authentication FAILED: DNS error.");
}

function authTCPobj::onConnectFailed(%this)
{
	echo("Authentication FAILED: Connection failure.");
}

function authTCPobj::onConnected(%this)
{
	%this.send(%this.cmd);
	echo("Authentication: Connected...");
}

function authTCPobj::onDisconnect(%this)
{
}

function authTCPobj::onLine(%this, %line)
{
	if (%this.done)
	{
		return;
	}
	if (getWord(%line, 0) $= "FAIL")
	{
		%reason = getSubStr(%line, 5, strlen(%line) - 5);
		echo("Authentication FAILED: " @ %reason);
		return;
	}
	if (getWord(%line, 0) $= "SUCCESS")
	{
		echo("Authentication Success!");
		WebCom_PostServer();
		return;
	}
	if (getWord(%line, 0) $= "Set-Cookie:")
	{
		%this.cookie = getSubStr(%line, 12, strlen(%line) - 12);
	}
	if (getWord(%line, 0) $= "PASSPHRASE")
	{
		%passphrase = getWord(%line, 1);
		if (getKeyID() !$= "")
		{
			%crc = getPassPhraseResponse(%passphrase, %this.passPhraseCount);
			if (%crc !$= "")
			{
				echo("Authentication: Sending Response...");
				%this.filePath = "/auth/authConfirm.asp";
				if ($NewNetName !$= "")
				{
					%postText = "CRC=" @ %crc @ "&NAME=" @ urlEnc($NewNetName);
				}
				else
				{
					%postText = "CRC=" @ %crc;
				}
				%postText = %postText @ "&DEDICATED=" @ $Server::Dedicated;
				%postText = %postText @ "&PORT=" @ $Server::Port;
				%postTexLen = strlen(%postText);
				%this.cmd = "POST " @ authTCPobj.filePath @ " HTTP/1.1\r\nCookie: " @ authTCPobj.cookie @ "\r\nHost: " @ authTCPobj.site @ "\r\nContent-Type: application/x-www-form-urlencoded\r\nContent-Length: " @ %postTexLen @ "\r\n\r\n" @ %postText @ "\r\n";
				%this.disconnect();
				%this.connect(authTCPobj.site @ ":" @ authTCPobj.port);
			}
			%this.passPhraseCount++;
		}
		else
		{
			echo("Authentication FAILED: No key found.");
			return;
		}
	}
}

function urlEnc(%string)
{
	%string = strreplace(%string, " ", "%20");
	%string = strreplace(%string, "$", "%24");
	%string = strreplace(%string, "&", "%26");
	%string = strreplace(%string, "+", "%2B");
	%string = strreplace(%string, ",", "%2C");
	%string = strreplace(%string, "/", "%2F");
	%string = strreplace(%string, ":", "%3A");
	%string = strreplace(%string, ";", "%3B");
	%string = strreplace(%string, "=", "%3D");
	%string = strreplace(%string, "?", "%3F");
	%string = strreplace(%string, "@", "%40");
	return %string;
}

