function serverCmdBuyBrick ( %client, %position, %data )
{
	%player = %client.Player;

	if ( !isObject(%player) )
	{
		return;
	}

	%mg = %client.miniGame;

	if ( isObject(%mg) )
	{
		if ( !%mg.EnableBuilding )
		{
			return;
		}
	}


	%playerData = %player.getDataBlock();
	%maxItems = %playerData.maxItems;

	if ( %position < 0  ||  %position >= %maxItems )
	{
		return;
	}

	if ( isObject(%data) )
	{
		if ( %data.getClassName() !$= "fxDTSBrickData" )
		{
			return;
		}

		if ( %data.category $= "" )
		{
			return;
		}

		if ( %data.subCategory $= "" )
		{
			return;
		}

		if ( %data.uiName $= "" )
		{
			return;
		}

		%client.inventory[%position] = %data;
		messageClient (%client, 'MsgSetInvData', "", %position, %data);
	}
	else
	{
		%client.inventory[%position] = "";
	}
}
