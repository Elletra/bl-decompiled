$WebCom_PostSchedule = 0;


function WebCom_PostServer ()
{
	if ( $Server::LAN )
	{
		echo ("Can\'t post to master server in LAN game");
		return;
	}

	if ( !$Server::Port )
	{
		error ("ERROR: WebCom_PostServer() - $Server::Port is not set, game hasn\'t started yet?");
		return;
	}

	if ( !$missionRunning )
	{
		error ("ERROR: WebCom_PostServer() - mission is not running");
		return;
	}

	if ( !isNonsenseVerfied() )
	{
		echo ("Can\'t post to master yet, must auth...");
		return;
	}

	echo ("Posting to master server");

	if ( isEventPending($WebCom_PostSchedule) )
	{
		cancel ($WebCom_PostSchedule);
	}


	if ( isObject(postServerTCPObj) )
	{
		postServerTCPObj.delete();
	}

	new TCPObject (postServerTCPObj)
	{
		site = "master2.blockland.us";
		port = 80;
		filePath = "/postServer.php";
	};

	%urlEncName = urlEnc ($Server::Name);
	%urlEncGameMode = urlEnc ($GameModeDisplayName);
	%urlEncModPaths = "";

	if ( $Pref::Server::Password !$= "" )
	{
		%passworded = 1;
	}
	else
	{
		%passworded = 0;
	}

	if ( $Server::Dedicated )
	{
		%dedicated = 1;
	}
	else
	{
		%dedicated = 0;
	}

	$Server::PlayerCount = ClientGroup.getCount();

	%postText = "ServerName=" @ %urlEncName;
	%postText = %postText @ "&Port="        @  mFloor ($Server::Port);
	%postText = %postText @ "&Players="     @  mFloor ($Server::PlayerCount);
	%postText = %postText @ "&MaxPlayers="  @  mFloor ($Pref::Server::MaxPlayers);

	%postText = %postText @ "&Map="         @  %urlEncGameMode;
	%postText = %postText @ "&Mod="         @  %urlEncModPaths;
	%postText = %postText @ "&Passworded="  @  %passworded;
	%postText = %postText @ "&Dedicated="   @  %dedicated;

	%postText = %postText @ "&BrickCount="  @  mFloor ( getBrickCount() );
	%postText = %postText @ "&DemoPlayers=" @  mFloor ($Pref::Server::AllowDemoPlayers);

	%postText = %postText @ "&blid="        @  urlEnc ( getNumKeyID() );
	%postText = %postText @ "&csg="         @  urlEnc ( getVersionNumber() );
	%postText = %postText @ "&ver="         @  urlEnc ($Version);
	%postText = %postText @ "&build="       @  urlEnc ( getBuildNumber() );

	postServerTCPObj.postText = %postText;
	postServerTCPObj.postTextLen = strlen (%postText);


	postServerTCPObj.cmd = "POST " @  postServerTCPObj.filePath  @ " HTTP/1.0\r\n" @ 
		"Host: " @  postServerTCPObj.site  @ "\r\n" @ 
		"User-Agent: Blockland-r" @  getBuildNumber()  @ "\r\n" @ 
		"Content-Type: application/x-www-form-urlencoded\r\n" @ 
		"Content-Length: " @  postServerTCPObj.postTextLen  @ "\r\n" @ 
		"\r\n" @ 
		postServerTCPObj.postText  @ "\r\n";
	

	postServerTCPObj.connect (postServerTCPObj.site  @ ":" @  postServerTCPObj.port);

	%scheduleTime = 5 * 60 * 1000 * getTimeScale();
	$WebCom_PostSchedule = schedule (%scheduleTime, 0, WebCom_PostServer);


	if ( $Pref::Server::BrickPublicDomainTimeout > 0 )
	{
		%elapsedMS = getSimTime() - $Server::lastPostTime;
		%elapsedMinutes = mFloor (%elapsedMS / 1000 * 60);


		if ( %elapsedMinutes > 0 )
		{
			%count = mainBrickGroup.getCount();


			for ( %i = 0;  %i < %count;  %i++ )
			{
				%brickGroup = mainBrickGroup.getObject (%i);


				if ( !%brickGroup.isPublicDomain  &&  %brickGroup.hasUser() )
				{
					%brickGroup.abandonedTime += %elapsedMinutes;


					if (%brickGroup.abandonedTime >= $Pref::Server::BrickPublicDomainTimeout)
					{
						%brickGroup.isPublicDomain = 1;
					}
				}
			}

			$Server::lastPostTime += %elapsedMinutes * 60 * 1000;
		}
	}

	$Server::lastPostTime = getSimTime();
}

function WebCom_PostServerUpdateLoop ()
{
	if ( !isEventPending($WebCom_PostSchedule) )
	{
		WebCom_PostServer();
		return;
	}

	%timeLeft = getTimeRemaining ($WebCom_PostSchedule);
	%scheduleTime = 5 * 60 * 1000 * getTimeScale() * 0.2;

	if ( %timeLeft > %scheduleTime )
	{
		cancel ($WebCom_PostSchedule);
		$WebCom_PostSchedule = schedule (%scheduleTime, 0, WebCom_PostServer);
	}
}


function pingMatchMakerLoop ()
{
	if ( isEventPending($pingMatchMakerEvent) )
	{
		cancel ($pingMatchMakerEvent);
	}

	if ( !doesAllowConnections() )
	{
		return;
	}

	if ( getMatchMakerIP() !$= "" )
	{
		pingMatchmaker();
	}

	%oldQuotaObject = getCurrentQuotaObject();

	if ( isObject(%oldQuotaObject) )
	{
		clearCurrentQuotaObject();
	}

	%scheduleTime = 30 * 1000 * getTimeScale();
	$pingMatchMakerEvent = schedule (%scheduleTime, 0, pingMatchMakerLoop);

	if ( isObject(%oldQuotaObject) )
	{
		setCurrentQuotaObject (%oldQuotaObject);
	}
}
