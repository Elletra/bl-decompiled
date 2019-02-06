function GameConnection::onDrop ( %client, %reason )
{
	$Server::PlayerCount = ClientGroup.getCount();

	if ( %client.connected )
	{
		$Server::PlayerCount = ClientGroup.getCount() - 1;

		%client.onClientLeaveGame();
		removeFromServerGuidList (%client.guid);


		if ( !%client.isBanReject  &&  %client.getHasAuthedOnce()  ||  $Server::LAN )
		{
			messageAllExcept ( %client,  -1,  '',  '\c1%1 has left the game.',  %client.getPlayerName() );
			secureCommandToAllExcept ("zbR4HmJcSY8hdRhr", %client, 'ClientDrop', %client.getPlayerName(), %client);
		}

		echo ( "CDROP: " @  %client  @ " - " @  %client.getPlayerName()  @ " - " @  %client.getAddress() );

		if ( !%client.isBanReject )
		{
			if ( $Server::PlayerCount == $Pref::Server::MaxPlayers - 1  ||  
				 getSimTime() - $Server::lastPostTime > 30 * 1000  ||  $Server::lastPostTime < 30 * 1000 )
			{
				WebCom_PostServer();
			}
		}
	}
}
