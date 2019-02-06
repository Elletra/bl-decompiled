exec ("./startStopTalking.cs");
exec ("./floodProtection.cs");
exec ("./chatFilter.cs");
exec ("./teamMessageSent.cs");


function serverCmdMessageSent ( %client, %text )
{
	%trimText = trim (%text);

	if ( %client.lastChatText $= %trimText )
	{
		%chatDelta = ( getSimTime() - %client.lastChatTime ) / getTimeScale();

		if ( %chatDelta < 15000 )
		{
			%client.spamMessageCount = $SPAM_MESSAGE_THRESHOLD;
			messageClient (%client, '', '\c5Do not repeat yourself.');
		}
	}

	%client.lastChatTime = getSimTime();
	%client.lastChatText = %trimText;

	%player = %client.Player;

	if ( isObject(%player) )
	{
		%player.playThread (3, talk);
		%player.schedule ( strlen(%text) * 50,  playThread,  3,  root );
	}

	%text = chatWhiteListFilter (%text);
	%text = StripMLControlChars (%text);
	%text = trim (%text);

	if ( strlen(%text) <= 0 )
	{
		return;
	}

	if ( $Pref::Server::MaxChatLen > 0  &&  strlen(%text) >= $Pref::Server::MaxChatLen)
	{
		%text = getSubStr (%text, 0, $Pref::Server::MaxChatLen);
	}

	%protocol = "http://";
	%protocolLen = strlen (%protocol);

	%urlStart = strpos (%text, %protocol);

	if ( %urlStart == -1 )
	{
		%protocol = "https://";
		%protocolLen = strlen (%protocol);
		%urlStart = strpos (%text, %protocol);
	}

	if ( %urlStart == -1 )
	{
		%protocol = "ftp://";
		%protocolLen = strlen (%protocol);
		%urlStart = strpos (%text, %protocol);
	}

	if ( %urlStart != -1 )
	{
		%urlEnd = strpos (%text, " ", %urlStart + 1);

		%skipProtocol = 0;

		if ( %protocol $= "http://" )
		{
			%skipProtocol = 1;
		}

		if ( %urlEnd == -1 )
		{
			%fullUrl = getSubStr ( %text,  %urlStart,  strlen(%text) - %urlStart );
			%url = getSubStr ( %text,  %urlStart + %protocolLen,  strlen(%text) - %urlStart - %protocolLen );
		}
		else
		{
			%fullUrl = getSubStr (%text,  %urlStart,  %urlEnd - %urlStart);
			%url = getSubStr (%text,  %urlStart + %protocolLen,  %urlEnd - %urlStart - %protocolLen);
		}

		if ( strlen(%url) > 0 )
		{
			%url = strreplace (%url, "<", "");
			%url = strreplace (%url, ">", "");

			if ( %skipProtocol )
			{
				%newText = strreplace (%text, %fullUrl, "<a:" @  %url  @ ">" @  %url  @ "</a>\c6");
			}
			else
			{
				%newText = strreplace (%text, %fullUrl, "<a:" @  %protocol @ %url  @ ">" @  %url  @ "</a>\c6");
			}

			echo (%newText);
			%text = %newText;
		}
	}

	if ( $Pref::Server::ETardFilter )
	{
		if ( !chatFilter(%client,  %text,  $Pref::Server::ETardList,  '\c5This is a civilized game.  Please use full words.') )
		{
			return 0;
		}
	}

	chatMessageAll (%client, '\c7%1\c3%2\c7%3\c6: %4', %client.clanPrefix, %client.getPlayerName(), 
		%client.clanSuffix, %text);

	echo (%client.getSimpleName(),  ": ",  %text);
}
