function GameConnection::sendPlayerListUpdate ( %client )
{
	secureCommandToAll ("zbR4HmJcSY8hdRhr", 'ClientJoin', %client.getPlayerName(), %client, %client.getBLID(), 
		%client.score, %client.isAIControlled(), %client.isAdmin, %client.isSuperAdmin);
}

function serverCmdOpenPlayerList ( %client )
{
	%client.playerListOpen = true;
	%count = ClientGroup.getCount();

	for ( %i = 0;  %i < %count;  %i++ )
	{
		%cl = ClientGroup.getObject(%i);
		secureCommandToClient ("zbR4HmJcSY8hdRhr", %client, 'ClientScoreChanged', mFloor(%cl.score), %cl);
	}
}

function serverCmdClosePlayerList ( %client )
{
	%client.playerListOpen = false;
}

function listClients ()
{
	%count = ClientGroup.getCount();

	echo ("  ", %count, " Clients");
	echo ("Object  Name                    IP                BLID    Ping");
	echo ("--------------------------------------------------------------");

	for ( %clientIndex = 0;  %clientIndex < %count;  %clientIndex++ )
	{
		%cl = ClientGroup.getObject (%clientIndex);

		%idFillLen = 8 - strlen (%cl);
		%nameFillLen = 24 - strlen ( %cl.getSimpleName() );
		%ipFillLen = 18 - strlen ( %cl.getRawIP() );
		%blidFillLen = 8 - strlen ( %cl.getBLID() );

		echo ( %cl @ makePadString (" ", %idFillLen)  @  %cl.getSimpleName()  @  
			  makePadString(" ", %nameFillLen)        @  %cl.getRawIP()       @  
			  makePadString(" ", %ipFillLen)          @  %cl.getBLID()        @ 
			  makePadString(" ", %blidFillLen)        @  %cl.getPing() );
	}

	echo ("------------------------------------------------------------");
}
