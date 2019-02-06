function GameConnection::onClientEnterGame ( %client )
{
	if ( %client.getBLID() $= "" )
	{
		%client.schedule (10, delete);
		return;
	}

	if ( !$Server::LAN )
	{
		%doReset = 1;
		%count = ClientGroup.getCount();
		
		for ( %i = 0;  %i < %count;  %i++ )
		{
			%cl = ClientGroup.getObject (%i);

			if ( %cl != %client  &&  %cl.getBLID() == %client.getBLID() )
			{
				%doReset = 0;
			}
		}

		if ( %client.isLocal() )
		{
			%doReset = 0;
		}

		if ( %doReset )
		{
			%client.resetVehicles();
		}
	}

	%client.undoStack = New_QueueSO (512);

	commandToClient ( %client, 'SetBuildingDisabled', 0 );
	commandToClient ( %client, 'SetPaintingDisabled', 0 );
	commandToClient ( %client, 'SetPlayingMiniGame', 0 );
	commandToClient ( %client, 'SetRunningMiniGame', 0 );
	commandToClient ( %client, 'SetRemoteServerData',  $Server::LAN,  isListenServer() );

	sendTimeScaleToClient (%client);

	%client.transmitMaxPlayers();

	if ( !$Server::LAN )
	{
		%client.transmitServerName();
	}

	%client.Camera = new Camera ()
	{
		dataBlock = Observer;
	};

	%client.Camera.mode = "Observer";
	MissionCleanup.add (%client.Camera);

	%client.dummyCamera = new Camera ()
	{
		dataBlock = Observer;
	};

	MissionCleanup.add (%client.dummyCamera);
	%client.Camera.scopeToClient (%client);

	%client.score = 0;

	commandToClient (%client, 'clearMapList');

	%client.bpsCount = 0;
	%client.bpsTime = %currTime;


	sendLetterPrintInfo (%client);

	commandToClient (%client, 'PSD_KillPrints');
	commandToClient (%client, 'PlayGui_LoadPaint');


	EnvGuiServer::SendVignette (%client);

	if ( isObject($DefaultMiniGame) )
	{
		$DefaultMiniGame.addMember (%client);
	}
	else
	{
		%client.spawnPlayer();
	}
}
