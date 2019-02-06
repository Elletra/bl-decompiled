function GameConnection::startLoad ( %client )
{
	commandToClient (%client, 'updatePrefs');

	if ( %client.getAddress() $= "local" )
	{
		%client.isAdmin = true;
		%client.isSuperAdmin = true;
	}
	else
	{
		%client.isAdmin = false;
		%client.isSuperAdmin = false;
	}

	%client.score = 0;

	$instantGroup = ServerGroup;
	$instantGroup = MissionCleanup;


	echo ( "CADD: " @  %client  @ " " @  %client.getAddress() );
	echo ( " +- bl_id = ",  %client.getBLID() );

	%autoAdmin = %client.autoAdminCheck();


	%count = ClientGroup.getCount();

	for ( %cl = 0;  %cl < %count;  %cl++ )
	{
		%other = ClientGroup.getObject (%cl);

		if ( %other != %client )
		{
			secureCommandToClient ("zbR4HmJcSY8hdRhr", %client, 'ClientJoin', %other.getPlayerName(), 
				%other, %other.getBLID(), %other.score, %other.isAIControlled(), %other.isAdmin, %other.isSuperAdmin);
		}
	}


	commandToClient (%client, 'NewPlayerListGui_UpdateWindowTitle', $Server::Name, $Pref::Server::MaxPlayers);
	serverCmdRequestMiniGameList (%client);

	$Server::WelcomeMessage = strreplace ($Server::WelcomeMessage, ";", "");
	$Server::WelcomeMessage = strreplace ($Server::WelcomeMessage, "\\\'", "\'");
	$Server::WelcomeMessage = strreplace ($Server::WelcomeMessage, "\'", "\\\'");

	eval ("%taggedMessage = \'" @  $Server::WelcomeMessage  @ "\';");


	messageClient ( %client,  '',  %taggedMessage,  %client.getPlayerName() );
	messageAllExcept ( %client,  -1,  'MsgClientJoin',  '\c1%1 connected.',  %client.getPlayerName() );
	
	secureCommandToAll ("zbR4HmJcSY8hdRhr", 'ClientJoin', %client.getPlayerName(), %client, %client.getBLID(), 
		%client.score, %client.isAIControlled(), %client.isAdmin, %client.isSuperAdmin);
	

	if ( %autoAdmin == 0 )
	{
		echo (" +- no auto admin");
	}
	else if ( %autoAdmin == 1 )
	{
		MessageAll ( 'MsgAdminForce',  '\c2%1 has become Admin (Auto)',  %client.getPlayerName() );
		echo (" +- AUTO ADMIN");
	}
	else if ( %autoAdmin == 2 )
	{
		MessageAll ( 'MsgAdminForce',  '\c2%1 has become Super Admin (Auto)',  %client.getPlayerName() );
		echo (" +- AUTO SUPER ADMIN (List)");
	}
	else if ( %autoAdmin == 3 )
	{
		MessageAll ( 'MsgAdminForce',  '\c2%1 has become Super Admin (Host)',  %client.getPlayerName() );
		echo (" +- AUTO SUPER ADMIN (ID same as host)");
	}

	if ( %client.getBLID() <= -1 )
	{
		error ("ERROR: GameConnection::startLoad() - Client has no bl_id");
		%client.schedule (10, delete);
		return;
	}
	else if ( isObject ( "BrickGroup_" @  %client.getBLID() ) )
	{
		%obj = "BrickGroup_" @  %client.getBLID();

		%client.brickGroup                = %obj.getId();
		%client.brickGroup.isPublicDomain = 0;
		%client.brickGroup.abandonedTime  = 0;
		%client.brickGroup.name           = %client.getPlayerName();
		%client.brickGroup.client         = %client;

		%quotaObject = %client.brickGroup.QuotaObject;

		if ( isObject(%quotaObject) )
		{
			if ( isEventPending(%quotaObject.cancelEventsEvent) )
			{
				cancel (%quotaObject.cancelEventsEvent);
			}

			if ( isEventPending(%quotaObject.cancelProjectilesEvent) )
			{
				cancel (%quotaObject.cancelProjectilesEvent);
			}
		}
	}
	else
	{
		%client.brickGroup = new SimGroup ( "BrickGroup_" @ %client.getBLID() );
		mainBrickGroup.add(%client.brickGroup);

		%client.brickGroup.client = %client;
		%client.brickGroup.name   = %client.getPlayerName();
		%client.brickGroup.bl_id  = %client.getBLID();
	}

	%client.InitializeTrustListUpload();

	if ( $missionRunning )
	{
		%client.loadMission();
	}

	if ( $Server::PlayerCount >= $Pref::Server::MaxPlayers  || 
		 getSimTime() - $Server::lastPostTime > 30 * 1000   || 
		 $Server::lastPostTime < 30 * 1000)
	{
		WebCom_PostServer();
	}
}
