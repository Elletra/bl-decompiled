function GameConnection::initializeTrustListUpload ( %client )
{
	if ( $Server::LAN )
	{
		%count = ClientGroup.getCount();

		for ( %i = 0;  %i < %count;  %i++ )
		{
			%otherClient = ClientGroup.getObject (%i);

			if ( %otherClient == %client )
			{
				secureCommandToClient ("zbR4HmJcSY8hdRhr", %client, 'ClientTrust', %otherClient, -1);
			}
			else
			{
				%level = 10;
				secureCommandToClient ("zbR4HmJcSY8hdRhr", %client, 'ClientTrust', %otherClient, %level);
				secureCommandToClient ("zbR4HmJcSY8hdRhr", %otherClient, 'ClientTrust', %client, %level);
			}
		}

		return;
	}

	%ourBrickGroup = %client.brickGroup;
	%ourBL_ID = %client.getBLID();

	%count = %ourBrickGroup.potentialTrustCount;

	for ( %i = 0;  %i < %count;  %i++ )
	{
		%targetBL_ID = %ourBrickGroup.potentialTrustEntry[%i];
		%targetBrickGroup = "BrickGroup_" @  %targetBL_ID;

		if ( isObject(%targetBrickGroup) )
		{
			%targetBrickGroup.trust[%ourBL_ID] = 0;
		}

		%ourBrickGroup.trust[%targetBL_ID] = 0;
		%ourBrickGroup.potentialTrust[%targetBL_ID] = 0;
		%ourBrickGroup.potentialTrustEntry[%i] = 0;
	}

	%ourBrickGroup.potentialTrustCount = 0;
	commandToClient (%client, 'TrustListUpload_Start');
}


function serverCmdTrustListUpload_Line ( %client, %line )
{
	%bl_id = mFloor ( getWord(%line, 0) );
	%level = mFloor ( getWord(%line, 1) );

	if ( %level <= 0 )
	{
		return;
	}

	%ourBrickGroup = "BrickGroup_" @  %client.getBLID();

	if ( !isObject(%ourBrickGroup) )
	{
		error ("ERROR: serverCmdTrustListUpload_Line() - " @  %client.getPlayerName()  @ " does not have a brick group.");
		return;
	}

	if ( %ourBrickGroup.potentialTrustCount > 1024 )
	{
		messageClient (%client, '', 'Trust list upload limit reached.  Maximum 1024 entries.');
		return;
	}

	%ourBrickGroup.addPotentialTrust (%bl_id, %level);
}

function serverCmdTrustListUpload_Done ( %client )
{
	%ourBL_ID = %client.getBLID();
	%ourBrickGroup = %client.brickGroup;

	if ( !isObject(%ourBrickGroup) )
	{
		error ("ERROR: serverCmdTrustListUpload_Done() - " @  %client.getPlayerName()  @ " has no brick group");
		return;
	}

	%count = mFloor (%ourBrickGroup.potentialTrustCount);

	for ( %i = 0;  %i < %count;  %i++ )
	{
		%currBL_ID = %ourBrickGroup.potentialTrustEntry[%i];
		%currLevel = %ourBrickGroup.potentialTrust[%currBL_ID];

		if ( %currLevel > 0 )
		{
			%targetBrickGroup = "BrickGroup_" @  %currBL_ID;

			if ( isObject(%targetBrickGroup) )
			{
				if ( %targetBrickGroup.potentialTrust[%ourBL_ID] >= %currLevel )
				{
					SetMutualBrickGroupTrust (%currBL_ID, %ourBL_ID, %currLevel);
					%targetClient = %targetBrickGroup.client;

					if ( isObject(%targetClient) )
					{
						secureCommandToClient ("zbR4HmJcSY8hdRhr", %targetClient, 'ClientTrust', %client, %currLevel);
						secureCommandToClient ("zbR4HmJcSY8hdRhr", %client, 'ClientTrust', %targetClient, %currLevel);
					}
				}
			}
		}
	}

	%count = ClientGroup.getCount();
	
	for ( %i = 0;  %i < %count;  %i++ )
	{
		%otherClient = ClientGroup.getObject (%i);

		if ( %otherClient == %client  ||  %client.brickGroup == %otherClient.brickGroup )
		{
			secureCommandToClient ("zbR4HmJcSY8hdRhr", %client, 'ClientTrust', %otherClient, -1);
		}
		else
		{
			%level = mFloor ( %ourBrickGroup.Trust[ %otherClient.getBLID() ] );

			secureCommandToClient ("zbR4HmJcSY8hdRhr", %client, 'ClientTrust', %otherClient, %level);
			secureCommandToClient ("zbR4HmJcSY8hdRhr", %otherClient, 'ClientTrust', %client, %level);
		}
	}
}

function TrustListCheck ( %obj1, %obj2, %interactionType )
{
	if ( %obj2.getType() & $TypeMasks::PlayerObjectType )
	{
		if ( !%obj1.getType() & $TypeMasks::PlayerObjectType )
		{
			return;
		}
	}
}
