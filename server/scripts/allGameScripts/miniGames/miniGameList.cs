function MiniGameSO::getLine ( %mg )
{
	if ( isObject(%mg.owner) )
	{
		%line = %mg.owner.getPlayerName() TAB %mg.owner.getBLID() TAB %mg.title TAB %mg.InviteOnly;
	}
	else
	{
		%line = " " TAB -1 TAB %mg.title TAB %mg.InviteOnly;
	}

	return %line;
}


function serverCmdRequestMiniGameList ( %client )
{
	if ( isObject($DefaultMiniGame) )
	{
		commandToClient (%client, 'AddMiniGameLine', $DefaultMiniGame.getLine(), $DefaultMiniGame, $DefaultMiniGame.colorIdx);
	}

	%count = ClientGroup.getCount();

	for ( %i = 0;  %i < %count;  %i++ )
	{
		%cl = ClientGroup.getObject (%i);
		%mg = %cl.miniGame;

		if ( isObject(%mg) )
		{
			if ( %cl.miniGame.owner == %cl )
			{
				commandToClient (%client, 'AddMiniGameLine', %mg.getLine(), %mg, %mg.colorIdx);
			}
		}
	}
}
