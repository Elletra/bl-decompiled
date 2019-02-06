function MiniGameSO::bottomPrintAll ( %obj, %text, %time, %hideBar, %client )
{
	if ( %client !$= "" )
	{
		if ( %client.miniGame != %obj )
		{
			return;
		}
	}

	if ( isObject(%client) )
	{
		%name = %client.getPlayerName();
		%text = strreplace (%text, "%1", %name);
	}

	for ( %i = 0;  %i < %obj.numMembers;  %i++ )
	{
		%cl = %obj.member[%i];
		commandToClient (%cl, 'bottomPrint', %text, %time, %hideBar);
	}
}

function MiniGameSO::centerPrintAll ( %obj, %text, %time, %client )
{
	if ( %client !$= "" )
	{
		if ( %client.miniGame != %obj )
		{
			return;
		}
	}

	if ( isObject(%client) )
	{
		%name = %client.getPlayerName();
		%text = strreplace (%text, "%1", %name);
	}

	for ( %i = 0;  %i < %obj.numMembers;  %i++ )
	{
		%cl = %obj.member[%i];
		commandToClient (%cl, 'centerPrint', %text, %time);
	}
}

function MiniGameSO::chatMsgAll ( %obj, %text, %client )
{
	if ( %client !$= "" )
	{
		if ( %client.miniGame != %obj )
		{
			return;
		}
	}

	%obj.chatMessageAll (%client, addTaggedString(%text), %client.getPlayerName(), %client.score);
}


// =================
//  Register Events
// =================

registerOutputEvent ("MiniGame", "ChatMsgAll", "string 200 176");
registerOutputEvent ("MiniGame", "CenterPrintAll", "string 200 156" TAB "int 1 10 3");
registerOutputEvent ("MiniGame", "BottomPrintAll", "string 200 156" TAB "int 1 10 3" TAB "bool");
registerOutputEvent ("MiniGame", "Reset", "");
registerOutputEvent ("MiniGame", "RespawnAll", "");
