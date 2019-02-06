function GameConnection::centerPrint ( %client, %message, %time )
{
	if ( isObject(%client) )
	{
		%name = %client.getPlayerName();
		%message = strreplace (%message, "%1", %name);
	}

	commandToClient (%client, 'centerPrint', %message, %time);
}

function GameConnection::bottomPrint ( %client, %message, %time, %hideBar )
{
	if ( isObject(%client) )
	{
		%name = %client.getPlayerName();
		%message = strreplace (%message, "%1", %name);
	}

	commandToClient (%client, 'bottomPrint', %message, %time, %hideBar);
}

function GameConnection::chatMessage ( %client, %message )
{
	messageClient (%client, '', addTaggedString(%message), %client.getPlayerName(), %client.score);
}

function GameConnection::playSound ( %client, %soundData )
{
	if ( !isObject(%soundData) )
	{
		return;
	}

	if ( !isObject ( %soundData.getDescription() ) )
	{
		return;
	}

	if ( %soundData.getDescription().isLooping == 1 )
	{
		return;
	}

	if ( !%soundData.getDescription().is3D )
	{
		return;
	}

	%client.play2D (%soundData);
}


// =================
//  Register Events
// =================

registerOutputEvent ("GameConnection", "CenterPrint", "string 200 156" TAB "int 1 10 3");
registerOutputEvent ("GameConnection", "BottomPrint", "string 200 156" TAB "int 1 10 3" TAB "bool");
registerOutputEvent ("GameConnection", "ChatMessage", "string 200 176");
registerOutputEvent ("GameConnection", "IncScore", "int -99999 99999 1");
registerOutputEvent ("GameConnection", "playSound", "dataBlock Sound");
