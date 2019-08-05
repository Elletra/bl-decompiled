function authTCPobj_Client::onDNSFailed ( %this )
{
	echo ("Client Auth DNS Failed");

	if ( isUnlocked () )
	{
		MM_AuthText.setText ("Offline Mode (DNS Failed)");
		MM_AuthBar.blinkSuccess ();
	}
	else if ( $useSteam  &&  SteamOfflineUnlock () )
	{
		$GameModeGui::GameModeCount = 0;

		MM_AuthText.setText ("Offline Mode (DNS Failed)");
		MM_AuthBar.blinkFail ();
	}
	else
	{
		MM_AuthText.setText ("Demo Mode");
		MM_AuthBar.blinkFail ();
	}
}

function authTCPobj_Client::onConnectFailed ( %this )
{
	%this.retryCount++;
	%maxRetries = 3;

	if ( %this.retryCount > %maxRetries )
	{
		if ( isUnlocked () )
		{
			echo ("Client auth connection failed, setting demo mode");

			MM_AuthText.setText ("Offline Mode");
			MM_AuthBar.blinkSuccess ();
		}
		else
		{
			echo ("Client auth failed, setting demo mode");

			if ( $useSteam  &&  SteamOfflineUnlock () )
			{
				$GameModeGui::GameModeCount = 0;

				MM_AuthText.setText ("Offline Mode (Connect Failed)");
				MM_AuthBar.blinkFail ();
			}
			else
			{
				MM_AuthText.setText ("Demo Mode");
				MM_AuthBar.blinkFail ();
			}
		}
	}
	else
	{
		echo ("Retrying client auth...");

		MM_AuthText.setText ("Retrying connection...");
		%this.schedule (5000, connect, %this.site @ ":" @ %this.port);
	}
}

function authTCPobj_Client::onConnected ( %this )
{
	%this.send (%this.cmd);
	MM_AuthText.setText ("Authentication: Validating key...");
}

function authTCPobj_Client::onDisconnect ( %this )
{
	if ( %this.success )
	{
		sendHatRequest ();
	}
}

function authTCPobj_Client::onLine ( %this, %line )
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
			warn ("WARNING: authTCPobj_Client - got non-200 http response " @ %code @ "");
		}

		if ( %code >= 400  &&  %code <= 499 )
		{
			warn ("WARNING: 4xx error on authTCPobj_Client, retrying");

			%this.schedule (0, disconnect);
			%this.schedule (500, connect, %this.site @ ":" @ %this.port);
		}

		if ( %code >= 300  &&  %code <= 399 )
		{
			warn ("WARNING: 3xx error on authTCPobj_Client, will wait for location header");
		}
	}
	else if ( %word $= "Location:" )
	{
		%url = getWords (%line, 1);
		warn ("WARNING: authTCPobj_Client - Location redirect to " @ %url);

		%this.filePath = %url;
		%this.cmd      = "GET " @ %this.filePath @ " HTTP/1.0\r\nHost: " @ %this.site @ "\r\n\r\n";
		%this.schedule (0, disconnect);
		%this.schedule (500, connect, %this.site @ ":" @ %this.port);
	}
	else if ( %word $= "Content-Location:" )
	{
		%url = getWords (%line, 1);
		warn ("WARNING: authTCPobj_Client - Content-Location redirect to " @ %url);

		%this.filePath = %url;
		%this.cmd      = "GET " @ %this.filePath @ " HTTP/1.0\r\nHost: " @ %this.site @ "\r\n\r\n";
		%this.schedule (0, disconnect);
		%this.schedule (500, connect, %this.site @ ":" @ %this.port);
	}
	else if ( %word $= "SEND_OGL_EXT" )
	{
		$sendOGLExt = 1;
	}
	else if ( %word $= "FAIL" )
	{
		%reason = getSubStr (%line, 5, strlen (%line) - 5);
		echo ("Authentication FAILED: " @ %reason);

		if ( getWord (%reason, 0) $= "MSG" )
		{
			%reason = getSubStr (%reason, 4, strlen (%reason) - 4);
			MessageBoxOK ("Authentication FAILED", %reason);
		}

		MM_AuthText.setText ("Authentication FAILED: " @ %reason);

		if ( %reason !$= "Version too old."  &&  stripos (%reason, "temporary") == -1  &&  
			 stripos (%reason, "temporarily") == -1  &&  $useSteam == 0 )
		{
			setKeyDat ("XXXXXAAAABBBBCCCC", 238811);
			Unlock ();
		}

		lock ();
		MM_UpdateDemoDisplay ();

		if ( MBOKFrame.isAwake ()  &&  MBOKFrame.getValue () $= "SUCCESS" )
		{
			MessageBoxOK ("Authentication FAILED", "Invalid key.", "canvas.pushDialog(keyGui);");
		}

		MM_AuthBar.blinkFail ();
		return;
	}
	else if ( %word $= "NAMEFAIL" )
	{
		%reason = getSubStr (%line, 9, strlen (%line) - 9.0);
		regName_registerWindow.setVisible (0);
		MessageBoxOK ("Name Change Failed", %reason);
		$NewNetName = "";

		return;
	}
	else if ( %word $= "NAMESUCCESS" )
	{
		%pos = strpos (%line, " ", 0);
		%pos = strpos (%line, " ", %pos + 1) + 1;

		$pref::Player::NetName = getSubStr (%line, %pos + 1, strlen (%line) - %pos - 1);

		Canvas.popDialog (regNameGui);

		MessageBoxOK ("Name Changed", "Your name has been changed to " @ $pref::Player::NetName @ "");

		$pref::Player::LANName = $pref::Player::NetName;
		$NewNetName = "";

		if ( strnicmp ($pref::Player::NetName, "Blockhead", strlen ("Blockhead")) )
		{
			steamGetAchievement ("ACH_NAME_CHANGE", "steamGetAchievement");
		}
	}
	else if ( %word $= "SUCCESS" )
	{
		%nr = getWord (%line, 1);

		if ( verifyNonsense (%nr) )
		{
			echo ("Authentication: SUCCESS");

			%pos = strpos (%line, " ", 0);
			%pos = strpos (%line, " ", %pos + 1) + 1;

			$pref::Player::NetName = getSubStr (%line, %pos + 1, strlen (%line) - %pos - 1);

			MM_AuthText.setText ("Welcome, " @ $pref::Player::NetName);

			$authed       = 1;
			%this.success = 1;

			MM_AuthBar.blinkSuccess ();
			JS_QueryInternetBlocker.setVisible (0);
		}
		else
		{
			MM_AuthText.setText ("Authentication FAILED: Version Error");
			lock ();

			MessageBoxOK ("Authentication FAILED", "Version Error.", "");
			MM_AuthBar.blinkFail ();
		}

		return;
	}
	else if ( %word $= "Set-Cookie:" )
	{
		%this.cookie = getSubStr (%line, 12, strlen (%line) - 12);
	}
	else if ( %word $= "PASSPHRASE" )
	{
		%passphrase = getWord (%line, 1);

		if ( getKeyID () !$= "" )
		{
			%crc = getPassPhraseResponse (%passphrase, %this.passPhraseCount);

			if ( %crc !$= "" )
			{
				%this.filePath = "/authConfirm2.php";

				if ( $NewNetName !$= "" )
				{
					%postText = "CRC=" @ %crc @ "&NAME=" @ urlEnc ($NewNetName);
				}
				else
				{
					%postText = "CRC=" @ %crc;
				}

				%postText = %postText @ MM_AuthBar::getExtendedPostString ();

				%this.postText    = %postText;
				%this.postTextLen = strlen (%postText);
				%this.cmd         = "POST " @ %this.filePath @ " HTTP/1.0\r\n" @ 
				                    "Cookie: " @ %this.cookie @ "\r\n" @ 
				                    "Host: " @ %this.site @ "\r\n" @ 
				                    "User-Agent: Blockland-r" @ getBuildNumber() @ "\r\n" @ 
				                    "Content-Type: application/x-www-form-urlencoded\r\n" @ 
				                    "Content-Length: " @ %this.postTextLen @ "\r\n" @ 
				                    "\r\n" @ %this.postText @ "\r\n";

				%this.schedule (0, disconnect);
				%this.schedule (10, connect, authTCPobj_Client.site @ ":" @ authTCPobj_Client.port);
			}

			%this.passPhraseCount++;
		}
		else
		{
			echo ("Authentication: FAIL No key");
			MM_AuthText.setText ("Authentication FAILED: No key found.");
			lock ();

			return;
		}
	}
	else if ( %word $= "DOUPDATE" )
	{
		$AU_AutoClose = 0;
	}
	else if ( %word $= "CRAPON_START" )
	{
		%file = new FileObject ();
		%file.openForWrite ("base/server/crapOns_Cache.cs");
		%file.writeLine ("");
		%file.close ();
		%file.delete ();
	}
	else if ( %word $= "CRAPON_CRC" )
	{
		%val = getWord (%line, 1);
		appendCrapOnCache ("$CrapOnCRC_[" @ %val @ "] = true;");
	}
	else if ( %word $= "CRAPON_NAME" )
	{
		%val = getWord (%line, 1);
		appendCrapOnCache ("$CrapOnName_[" @ %val @ "] = true;");
	}
	else if ( %word $= "CRAPON_DEDICATEDNAME" )
	{
		%val = getWord (%line, 1);
		appendCrapOnCache ("$CrapOnDedicatedName_[" @ %val @ "] = true;");
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

		if ( isMacintosh () )
		{
			if ( gotProtoURL () )
			{
				MainMenuGui.hideButtons ();
				parseProtocol (getProtoURL ());
			}
		}

		if ( $connectArg !$= ""  &&  !$Server::Dedicated )
		{
			ConnectToServer ($connectArg, $passwordArg, 1, 1);
			Connecting_Text.setText ("Connecting to " @ $connectArg);
			Canvas.pushDialog (connectingGui);
		}

		if ( $steamLobbyArg !$= "" )
		{
			SteamJoinLobby ($steamLobbyArg);
		}
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
	}
	else if ( %word $= "NOTE" )
	{
		%val = getWords (%line, 1, 99);
		echo ("NOTE: " @ %val);
	}
	else if ( %word $= "TIMESTAMP" )
	{
		%val = getWord (%line, 1);
		setUTC (%val);
	}
	else if ( %word $= "HATLIST" )
	{
		%wordCount = getWordCount (%line);

		for ( %i = 1;  %i < %wordCount;  %i++ )
		{
			%hat = getWord (%line, %i);
		}
	}
}
