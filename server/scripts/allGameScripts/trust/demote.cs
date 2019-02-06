function serverCmdTrust_Demote ( %client, %targetBL_ID, %level )
{
	%targetBL_ID = mFloor (%targetBL_ID);
	%level = mFloor (%level);
	%ourBL_ID = %client.getBLID();

	%ourBrickGroup = "BrickGroup_" @  %ourBL_ID;
	%targetBrickGroup = "BrickGroup_" @  %targetBL_ID;

	if ( !isObject(%ourBrickGroup)  ||  !isObject(%targetBrickGroup) )
	{
		return;
	}

	%targetClient = %targetBrickGroup.client;

	if ( %ourBrickGroup.Trust[%targetBL_ID] <= %level )
	{
		messageClient (%client, 'MessageBoxOK', '%1 is already at or below that trust level.', 
			%targetBrickGroup.name);
		return;
	}

	if ( %level == 0 )
	{
		SetMutualBrickGroupTrust (%ourBL_ID, %targetBL_ID, 0);

		if ( isObject(%targetClient) )
		{
			messageClient ( %targetClient, 'MessageBoxOK', '%1 has removed you from their trust list.', 
				%client.getPlayerName() );
		}

		secureCommandToClient ("zbR4HmJcSY8hdRhr", %targetClient, 'ClientTrust', %client, 0);
		secureCommandToClient ("zbR4HmJcSY8hdRhr", %client, 'ClientTrust', %targetClient, 0);
		secureCommandToClient ("zbR4HmJcSY8hdRhr", %targetClient, 'TrustDemoted', %client, %client.getBLID(), %level);
	}
	else if ( %level == 1 )
	{
		SetMutualBrickGroupTrust (%ourBL_ID, %targetBL_ID, 1);

		if ( isObject(%targetClient) )
		{
			messageClient ( %targetClient, 'MessageBoxOK', '%1 has demoted you to build trust.', 
				%client.getPlayerName() );
		}

		secureCommandToClient ("zbR4HmJcSY8hdRhr", %targetClient, 'ClientTrust', %client, 1);
		secureCommandToClient ("zbR4HmJcSY8hdRhr", %client, 'ClientTrust', %targetClient, 1);
		secureCommandToClient ("zbR4HmJcSY8hdRhr", %targetClient, 'TrustDemoted', %client, %client.getBLID(), %level);
	}
	else
	{
		commandToClient (%client, 'MessageBoxOK', 'Trust Demote Error', 'Invalid trust level specified.');
	}
}
