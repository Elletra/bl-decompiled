function ServerCmdRequestExtendedBrickInfo ( %client )
{
	%client.TransmitExtendedBrickInfo (0, 0);
}

function ServerCmdCancelExtendedBrickInfoRequest ( %client )
{
	if ( isEventPending(%client.TransmitExtendedBrickInfoEvent) )
	{
		cancel( %client.TransmitExtendedBrickInfoEvent);
	}
}

function GameConnection::TransmitExtendedBrickInfo ( %client, %groupIdx, %brickIdx )
{
	if ( isEventPending(%client.TransmitExtendedBrickInfoEvent) )
	{
		cancel (%client.TransmitExtendedBrickInfoEvent);
	}

	%groupCount = mainBrickGroup.getCount();
	%brickGroup = mainBrickGroup.getObject (%groupIdx);
	%brickCount = %brickGroup.getCount();


	for ( %i = 0;  %i < 150;  %i++ )
	{
		if ( %brickIdx >= %brickCount )
		{
			%safety = 0;
			%groupIdx++;

			while ( true )
			{
				%safety++;

				if ( %safety > 50000 )
				{
					error ("ERROR: GameConnection::TransmitAllBrickNames() - More than 50k brick groups?");
					return;
				}

				if ( %groupIdx >= %groupCount )
				{
					commandToClient (%client, 'TransmitAllBrickNamesDone');
					return;
				}

				%brickGroup = mainBrickGroup.getObject (%groupIdx);
				%brickCount = %brickGroup.getCount();

				if ( %brickCount <= 0 )
				{
					%groupIdx++;
				}
			}

			%brickIdx = 0;
		}

		%brick = %brickGroup.getObject (%brickIdx);
		%ghostID = %client.getGhostID (%brick);

		if ( %ghostID > 0 )
		{
			if ( %brick.getName() !$= "" )
			{
				commandToClient ( %client, 'TransmitBrickName', %ghostID, %brick.getName() );
			}

			for ( %j = 0;  %j < %brick.numEvents;  %j++ )
			{
				%line = %brick.serializeEventToString (%j, %client);
				commandToClient (%client, 'TransmitEvent', %ghostID, %line);
			}

			if ( %brick.isBasePlate )
			{
				if ( $Server::LAN )
				{
					commandToClient (%client, 'TransmitBrickOwner', %ghostID, %brick.bl_id);
				}
				else
				{
					commandToClient (%client, 'TransmitBrickOwner', %ghostID, %brick.getGroup().bl_id);
				}
			}
		}

		%brickIdx++;
	}

	%client.TransmitExtendedBrickInfoEvent = %client.schedule (32, TransmitExtendedBrickInfo, %groupIdx, %brickIdx);
}
