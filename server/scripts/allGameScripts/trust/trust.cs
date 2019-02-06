exec ("./acceptReject.cs");
exec ("./chainTrustCheck.cs");
exec ("./demote.cs");
exec ("./ignore.cs");
exec ("./mutualTrust.cs");
exec ("./trustCheckFailed.cs");
exec ("./trustInvite.cs");
exec ("./trustLevel.cs");
exec ("./trustListUpload.cs");
exec ("./undoTrustCheck.cs");



function updateTrustGui ( %client, %targetClient )
{
	if ( !isObject(%client) )
	{
		return;
	}

	if ( !isObject(%targetClient) )
	{
		return;
	}

	%trustLevelA = %client.brickGroup.trust[ %targetClient.getBLID() ];
	%trustLevelB = %targetClient.brickGroup.trust[ %client.getBLID() ];

	if ( %trustLevelA != %trustLevelB )
	{
		error ("ERROR: updateTrustGui() - trust level between " @  %client.getBLID()  @ 
			" and " @  %targetClient.getBLID()  @ " is asymmetrical");
		return;
	}

	secureCommandToClient ("zbR4HmJcSY8hdRhr", %targetClient, 'ClientTrust', %client, %trustLevel);
	secureCommandToClient ("zbR4HmJcSY8hdRhr", %client, 'ClientTrust', %targetClient, %trustLevel);
}
