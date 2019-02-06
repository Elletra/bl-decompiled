function chatMessageClient ( %client, %sender, %voiceTag, %voicePitch, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10 )
{
	if ( !%client.muted[%sender] )
	{
		commandToClient (%client, 'ChatMessage', %sender, %voiceTag, %voicePitch, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, 
			%a8, %a9, %a10);
	}
}

function chatMessageTeam ( %sender, %team, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10 )
{
	if ( %msgString $= ""  ||  spamAlert(%sender) )
	{
		return;
	}

	%mg = %sender.miniGame;

	if ( isObject(%mg) )
	{
		%mg.chatMessageAll (%sender, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10);
	}
	else
	{
		messageClient (%sender, '', '\c5Team chat disabled - You are not in a mini-game.');
	}
}

function chatMessageAll ( %sender, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10 )
{
	if ( %msgString $= ""  ||  spamAlert(%sender) )
	{
		return;
	}

	%count = ClientGroup.getCount();

	for ( %i = 0;  %i < %count;  %i++ )
	{
		%obj = ClientGroup.getObject (%i);

		if ( %sender.team != 0 )
		{
			chatMessageClient (%obj, %sender, %sender.voiceTag, %sender.voicePitch, %msgString, %a1, %a2, 
				%a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10);
		}
		else if ( %obj.team == %sender.team )
		{
			chatMessageClient (%obj, %sender, %sender.voiceTag, %sender.voicePitch, %msgString, %a1, %a2, 
				%a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10);
		}
	}
}

function messageClient ( %client, %msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10, %a11, %a12, %a13 )
{
	commandToClient ( %client, 'ServerMessage', %msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10, 
		%a11, %a12, %a13 );
}

function messageTeam ( %team, %msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10, %a11, %a12, %a13 )
{
	%count = ClientGroup.getCount();

	for ( %cl = 0;  %cl < %count;  %cl++ )
	{
		%recipient = ClientGroup.getObject (%cl);

		if ( %recipient.team == %team )
		{
			messageClient (%recipient, %msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10, %a11, 
				%a12, %a13);
		}
	}
}

function messageTeamExcept ( %client, %msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10, %a11, %a12, %a13 )
{
	%team = %client.team;
	%count = ClientGroup.getCount();

	for ( %cl = 0;  %cl < %count;  %cl++ )
	{
		%recipient = ClientGroup.getObject (%cl);

		if ( %recipient.team == %team  &&  %recipient != %client )
		{
			messageClient (%recipient, %msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10, %a11, 
				%a12, %a13);
		}
	}
}

function MessageAll ( %msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10, %a11, %a12, %a13 )
{
	%count = ClientGroup.getCount();

	for ( %cl = 0;  %cl < %count;  %cl++ )
	{
		%client = ClientGroup.getObject (%cl);

		messageClient ( %client, %msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10, %a11, 
			%a12, %a13 );
	}
}

function messageAllExcept ( %client, %team, %msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10, %a11, %a12, %a13 )
{
	%count = ClientGroup.getCount();

	for ( %cl = 0;  %cl < %count;  %cl++ )
	{
		%recipient = ClientGroup.getObject (%cl);

		if ( %recipient != %client  &&  %recipient.team != %team )
		{
			messageClient (%recipient, %msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10, %a11, 
				%a12, %a13);
		}
	}
}

function announce ( %text )
{
	MessageAll ('', "\c3" @  %text);
}

function talk ( %text )
{
	if ( %text !$= "" )
	{
		MessageAll ('', "\c5CONSOLE: \c6" @  %text);
	}
}
