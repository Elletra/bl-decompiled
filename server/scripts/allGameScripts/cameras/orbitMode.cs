// FIXME: remove "observer" nonsense, or at least add a way to disable it through a field or something


function SimObject::onCameraEnterOrbit ( %obj, %camera )
{
	%client = %obj.client;

	if ( !isObject(%client) )
	{
		return;
	}


	%observerName = "<OBSERVER>";

	if ( %camera.getControllingClient() )
	{
		%observerName = %camera.getControllingClient().getPlayerName();
	}


	%playerName = "<PLAYERNAME>";

	if ( %obj.getControllingClient() )
	{
		%playerName = %obj.getControllingClient().getPlayerName();
	}
	else if ( isObject(%obj.client) )
	{
		%playerName = %obj.client.getPlayerName();
	}


	%obj.observerCount++;

	if ( %obj.observerCount == 1 )
	{
		commandToClient (%client, 'bottomPrint', "\c3" @  %obj.observerCount  @ " observer", 2, 1);
	}
	else
	{
		commandToClient (%client, 'bottomPrint', "\c3" @  %obj.observerCount  @ " observers", 2, 1);
	}
}

function SimObject::onCameraLeaveOrbit ( %obj, %camera )
{
	%client = %obj.client;

	if ( !isObject(%client) )
	{
		return;
	}


	%observerName = "<OBSERVER>";

	if ( %camera.getControllingClient() )
	{
		%observerName = %camera.getControllingClient().getPlayerName();
	}


	%playerName = "<PLAYERNAME>";

	if ( %obj.getControllingClient() )
	{
		%playerName = %obj.getControllingClient().getPlayerName();
	}
	else if ( isObject(%obj.client) )
	{
		%playerName = %obj.client.getPlayerName();
	}


	%obj.observerCount--;
	%mg = %client.miniGame;

	if ( isObject(%mg) )
	{
		if ( %mg.respawnTime <= 0 )
		{
			if ( %obj.observerCount == 1 )
			{
				commandToClient (%client, 'bottomPrint', "\c3" @  %obj.observerCount  @ " observer", 2, 1);
			}
			else
			{
				commandToClient (%client, 'bottomPrint', "\c3" @  %obj.observerCount  @ " observers", 2, 1);
			}
		}
	}
}
