function startGame ()
{
	if ( $Game::Running )
	{
		error ("startGame: End the game first!");
		return;
	}

	%count = ClientGroup.getCount();

	for ( %clientIndex = 0;  %clientIndex < %count;  %clientIndex++ )
	{
		%cl = ClientGroup.getObject (%clientIndex);
		commandToClient (%cl, 'GameStart');
		%cl.score = 0;
	}

	$Game::Running = 1;
}

function endGame ()
{
	if ( !$Game::Running )
	{
		error ("endGame: No game running!");
		return;
	}

	endAllMinigames();
	setTimeScale (1);


	%count = ClientGroup.getCount();

	for ( %clientIndex = 0;  %clientIndex < %count;  %clientIndex++ )
	{
		%cl = ClientGroup.getObject (%clientIndex);
		commandToClient (%cl, 'GameEnd');
	}

	resetMission();

	$Game::Running = false;
	$Game::MissionCleaningUp = false;
}

function shutDown ( %text )
{
	%count = ClientGroup.getCount();
	
	for ( %clientIndex = 0;  %clientIndex < %count;  %clientIndex++ )
	{
		%cl = ClientGroup.getObject (%clientIndex);

		if ( !%cl.isLocal() )
		{
			if ( %text !$= "" )
			{
				%cl.schedule (10, delete, %text);
			}
			else
			{
				%cl.schedule (10, delete, "The server was shut down.");
			}
		}
	}
}
