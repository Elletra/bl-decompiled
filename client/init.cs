function initClient ()
{
	%dashes  = "";
	%version = atoi ($Version);
	%version = mClampF (%version, 0, 25);

	for ( %i = 0;   %i < %version;  %i++ )
	{
		%dashes = %dashes @ "-";
	}

	echo ("\n--------- Initializing Base: Client " @ %dashes);

	if ( $pref::Video::resolution $= "" )
	{
		if ( $pref::Video::fullScreen )
		{
			$pref::Video::resolution = getDesktopResolution ();

			if ( $pref::Video::resolution $= "" )
			{
				$pref::Video::resolution = "800 600 32";
			}
		}
		else
		{
			%desktopW   = getWord (getDesktopResolution (), 0 );
			%desktopH   = getWord (getDesktopResolution (), 1 );
			%desktopBpp = getWord (getDesktopResolution (), 2 );

			%fudge = 30;

			if ( %desktopW > 1680 + %fudge  &&  %desktopH > 1050 + %fudge )
			{
				$pref::Video::resolution = "1680 1050 " @ %desktopBpp;
			}
			else
			{
				if ( %desktopW > 1280 + %fudge  &&  %desktopH > 720 + %fudge )
				{
					$pref::Video::resolution = "1280 720 " @ %desktopBpp;
				}
				else
				{
					$pref::Video::resolution = "800 600 " @ %desktopBpp;
				}
			}
		}
	}

	$Server::Dedicated = 0;
	$Client::GameTypeQuery = "Blockland";
	$Client::MissionTypeQuery = "Any";

	initBaseClient ();
	initCanvas ("Blockland");

	exec ("./scripts/allClientScripts.cs");
	exec ("base/client/ui/allClientGuis.gui");

	if ( isFile ("config/client/config.cs") )
	{
		exec ("config/client/config.cs");
	}

	JoinServerGui.lastQueryTime = 0;

	echo ("\n--------- Loading Client Add-Ons ---------");

	loadClientAddOns ();
	$numClientPackages = getNumActivePackages ();

	setNetPort (getRandom (64511) + 1024);

	optionsDlg.setShaderQuality ($Pref::ShaderQuality);

	setDefaultFov ($pref::Player::defaultFov);
	setZoomSpeed ($pref::Player::zoomSpeed);

	loadMainMenu ();

	BringWindowToForeground ();  // FIXME: Remove this bullshit
	schedule (1000, 0, BringWindowToForeground);  // FIXME: Remove this too

	loadTrustList ();
	updateTempBrickSettings ();
}

function onUDPFailure ()
{
	schedule (100, 0, setNetPort, getRandom (64511) + 1024);
}

function loadMainMenu ()
{
	Canvas.setContent (MainMenuGui);
	Canvas.setCursor ("DefaultCursor");
}

function convertFile ( %inFileName, %outFileName )
{
	if ( getBuildString () !$= "Debug"  &&  getBuildString () !$= "Release" )
	{
		return;
	}

	if ( !isFile (%inFileName) )
	{
		return;
	}


	%outFile = new FileObject ();
	%outFile.openForWrite (%outFileName);

	%file = new FileObject ();
	%file.openForRead (%inFileName);

	%buff = "";
	%line = %file.readLine ();

	while ( !%file.isEOF () )
	{
		%line = %file.readLine ();
		%line = trim (%line);

		%commentPos = strpos (%line, "//");

		if ( %commentPos == 0 )
		{
			break;
		}
		else if ( %commentPos > -1 )
		{
			%line = getSubStr (%line, 0, %commentPos);
		}

		%line = strreplace (%line, "\t", " ");
		%line = trim (%line);

		while ( true )
		{
			if ( strpos (%line, "  ") != -1 )
			{
				%line = strreplace (%line, "  ", " ");
			}
		}

		%buff = %buff @ %line @ " ";
	}

	%outFile.writeLine (%buff);

	%file.close ();
	%file.delete ();

	%outFile.close ();
	%outFile.delete ();
}


// Arrange me, addy~

$ArrangedActive = 0;
$ArrangedAddyCount = 0;

function notifyArrangedStart ( %addy )
{
	if ( !isObject (ServerGroup) )
	{
		%timeDelta = getSimTime () - $arrangedConnectionRequestTime;

		if ( %timeDelta > 5000  ||  %timeDelta < 0 )
		{
			warn ("Warning: notifyArrangedStart() - got notify without making a request");
			return;
		}
	}

	%addy = strreplace (%addy, "IP:", "");

	$ArrangedActive = 1;
	$ArrangedAddyCount = 0;
}

function notifyArrangedAddress ( %addy )
{
	if ( !$ArrangedActive )
	{
		echo ("Got notifyArrangedAddress when no arranged connection active.");
		return;
	}

	$ArrangedAddyCount = mFloor ($ArrangedAddyCount);
	$ArrangedAddys[$ArrangedAddyCount] = %addy;
	$ArrangedAddyCount++;
}

function notifyArrangedFinish ( %nonceA, %nonceB, %spamConnect )
{
	if ( !$ArrangedActive )
	{
		echo ("Got notifyArrangedFinish when no arranged connection active.");
		return;
	}

	$ArrangedActive = 0;
	$ArrangedConnection = new GameConnection();

	if ( isObject (ServerGroup) )
	{
		%isClient = 0;
	}
	else
	{
		%isClient = 1;
		%spamConnect = 0;
		Connecting_Text.setText (Connecting_Text.getText () @ "\nStarting arranged connection...");
	}

	switch ( $ArrangedAddyCount )
	{
		case 1:
			$ArrangedConnection.connectArranged (%spamConnect, %isClient, %nonceA, %nonceB, $ArrangedAddys[0]);

		case 2:
			$ArrangedConnection.connectArranged (%spamConnect, %isClient, %nonceA, %nonceB, $ArrangedAddys[0], 
				$ArrangedAddys[1]);

		case 3:
			$ArrangedConnection.connectArranged (%spamConnect, %isClient, %nonceA, %nonceB, $ArrangedAddys[0], 
				$ArrangedAddys[1], $ArrangedAddys[2]);

		case 4:
			$ArrangedConnection.connectArranged (%spamConnect, %isClient, %nonceA, %nonceB, $ArrangedAddys[0], 
				$ArrangedAddys[1], $ArrangedAddys[2], $ArrangedAddys[3]);

		case 5:
			$ArrangedConnection.connectArranged(%spamConnect, %isClient, %nonceA, %nonceB, $ArrangedAddys[0], 
				$ArrangedAddys[1], $ArrangedAddys[2], $ArrangedAddys[3], $ArrangedAddys[4]);

		case 6:
			$ArrangedConnection.connectArranged(%spamConnect, %isClient, %nonceA, %nonceB, $ArrangedAddys[0], 
				$ArrangedAddys[1], $ArrangedAddys[2], $ArrangedAddys[3], $ArrangedAddys[4], $ArrangedAddys[5]);

		case 7:
			$ArrangedConnection.connectArranged(%spamConnect, %isClient, %nonceA, %nonceB, $ArrangedAddys[0], 
				$ArrangedAddys[1], $ArrangedAddys[2], $ArrangedAddys[3], $ArrangedAddys[4], $ArrangedAddys[5], 
				$ArrangedAddys[6]);

		case 8:
			$ArrangedConnection.connectArranged(%spamConnect, %isClient, %nonceA, %nonceB, $ArrangedAddys[0], 
				$ArrangedAddys[1], $ArrangedAddys[2], $ArrangedAddys[3], $ArrangedAddys[4], $ArrangedAddys[5], 
				$ArrangedAddys[6], $ArrangedAddys[7]);

		case 9:
			$ArrangedConnection.connectArranged(%spamConnect, %isClient, %nonceA, %nonceB, $ArrangedAddys[0], 
				$ArrangedAddys[1], $ArrangedAddys[2], $ArrangedAddys[3], $ArrangedAddys[4], $ArrangedAddys[5], 
				$ArrangedAddys[6], $ArrangedAddys[7], $ArrangedAddys[8]);

		case 10:
			$ArrangedConnection.connectArranged(%spamConnect, %isClient, %nonceA, %nonceB, $ArrangedAddys[0], 
				$ArrangedAddys[1], $ArrangedAddys[2], $ArrangedAddys[3], $ArrangedAddys[4], $ArrangedAddys[5], 
				$ArrangedAddys[6], $ArrangedAddys[7], $ArrangedAddys[8], $ArrangedAddys[9]);
			
		default:
			error ("notifyArrangedFinish - Failed to call with addyCount = " @ $ArrangedAddyCount);
	}
}

function onSendPunchPacket ( %ip )
{
	if ( isObject (Connecting_Text) )
	{
		Connecting_Text.setText (Connecting_Text.getText () @ "\nSending punch packet...");
	}
	else
	{
		echo ("Sending punch packet to " @ %ip);
	}
}
