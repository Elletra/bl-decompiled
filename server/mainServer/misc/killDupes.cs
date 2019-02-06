function GameConnection::killDupes ( %client )
{
	%ourIP   = %client.getRawIP();
	%ourBLID = %client.getBLID();

	%count   = ClientGroup.getCount();

	for ( %clientIndex = 0;  %clientIndex < %count;  %clientIndex++ )
	{
		%cl = ClientGroup.getObject (%clientIndex);

		// Check for same BL_ID

		if ( %cl != %client  &&  %cl.getBLID() $= %ourBLID )
		{
			// Check for different IPs or different names

			if ( %cl.getRawIP() !$= %ourIP  ||  %cl.getPlayerName() !$= %client.getPlayerName() )
			{
				// Make sure it's not a LAN game

				if ( !%cl.isLocal()  &&  !%cl.isLan() )
				{
					%cl.schedule (10, delete, "Someone using your Blockland ID joined the server from a different IP address.");
				}
			}
		}
	}
}
