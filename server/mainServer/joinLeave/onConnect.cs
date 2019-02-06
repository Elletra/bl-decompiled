function GameConnection::onConnectRequest ( %client, %netAddress, %LANname, %netName, %clanPrefix, %clanSuffix, %clientNonce )
{
	echo ("Got connect request from " @  %netAddress);

	if ( $Server::LAN )
	{
		echo ("  lan name = ", %LANname);

		if ( %LANname $= "" )
		{
			%LANname = "Blockhead";
		}
	}
	else
	{
		echo ("  net name = ", %netName);

		if ( %netName $= "" )
		{
			return "CR_BADARGS";
		}
	}

	if ( %clientNonce !$= "" )
	{
		cancelPendingConnection (%clientNonce);
	}

	%client.clanPrefix = trim ( getSubStr ( StripMLControlChars(%clanPrefix),  0,  4 ) );
	%client.clanSuffix = trim ( getSubStr ( StripMLControlChars(%clanSuffix),  0,  4 ) );

	%client.LANname = trim ( getSubStr ( StripMLControlChars(%LANname),  0,  23 ) );
	%client.netName = trim ( StripMLControlChars(%netName) );

	%ip = %client.getRawIP();

	if ( $Server::PlayerCount >= $Pref::Server::MaxPlayers  &&  %ip !$= "local" )
	{
		return "CR_SERVERFULL";
	}

	return "";
}

// Now defunct

function AIConnection::onConnect ( %client )
{
	if ( !isLANAddress ( %client.getRawIP() ) )
	{
		%client.schedule (10, delete);
		return;
	}

	%client.connected = 1;

	%client.headColor  = AvatarColorCheck ("1 1 0 1");
	%client.chestColor = AvatarColorCheck ("1 1 1 1");
	%client.hipColor   = AvatarColorCheck ("0 0 1 1");
	%client.llegColor  = AvatarColorCheck ("0.1 0.1 0.1 1");
	%client.rlegColor  = AvatarColorCheck ("0.1 0.1 0.1 1");
	%client.larmColor  = AvatarColorCheck ("1 1 1 1");
	%client.rarmColor  = AvatarColorCheck ("1 1 1 1");
	%client.lhandColor = AvatarColorCheck ("1 1 0 1");
	%client.rhandColor = AvatarColorCheck ("1 1 0 1");
}

function GameConnection::onConnect ( %client )
{
	%client.connected = 1;

	messageClient (%client, 'MsgConnectionError', "", $Pref::Server::ConnectionError);

	$Server::PlayerCount = ClientGroup.getCount();
	%client.authCheck();

	return;
}
