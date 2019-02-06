function serverCmdDoPlayerIcons ( %client )
{
	if ( !%client.isAdmin )
	{
		return;
	}

	if ( isObject($iconBrick) )
	{
		$iconBrick.delete();
	}


	%camera = %client.Camera;

	%client.setControlObject (%camera);
	%camera.setMode ("Observer");
	%camera.setTransform ("-1.874 8.050 -1003.7 0.47718 -0.197864 0.856242 0.901965");

	$iconBrick = new StaticShape ()
	{
		dataBlock = dummyPlayer;
	};
	MissionCleanup.add ($iconBrick);

	$iconBrick.setTransform ("0 10 -1006 0 0 1 -1.57");
	$iconBrick.setScale ("0.5 0.5 0.5");
	$iconBrick.setNodeColor ("ALL", "1 1 1 1");
	$iconBrick.hideNode ("ALL");


	%time = -1.0;
	
	for ( %i = 0;  %i < $numHat;  %i++ )
	{
		%name = $hat[%i];

		if ( %name !$= "NULL" )
		{
			%time++;
			schedulePlayerIcon (%name, %time + 1);
		}
	}


	for ( %i = 0;  %i < $numAccent;  %i++ )
	{
		%name = $Accent[%i];

		if ( %name !$= "NULL" )
		{
			%time++;
			schedulePlayerIcon (%name, %time + 1);
		}
	}


	for ( %i = 0;  %i < $num["LHand"];  %i++ )
	{
		%name = $LHand[%i];

		if ( %name !$= "NULL" )
		{
			%time++;
			schedulePlayerIcon (%name, %time + 1);
		}
	}


	for ( %i = 0;  %i < $num["RHand"];  %i++ )
	{
		%name = $RHand[%i];

		if ( %name !$= "NULL" )
		{
			%time++;
			schedulePlayerIcon (%name, %time + 1);
		}
	}


	for ( %i = 0;  %i < $num["LArm"];  %i++ )
	{
		%name = $LArm[%i];

		if ( %name !$= "NULL" )
		{
			%time++;
			schedulePlayerIcon (%name, %time + 1);
		}
	}


	for ( %i = 0;  %i < $num["RArm"];  %i++ )
	{
		%name = $RArm[%i];

		if ( %name !$= "NULL" )
		{
			%time++;
			schedulePlayerIcon (%name, %time + 1);
		}
	}


	for ( %i = 0;  %i < $num["LLeg"];  %i++ )
	{
		%name = $LLeg[%i];
		
		if ( %name !$= "NULL" )
		{
			%time++;
			schedulePlayerIcon (%name, %time + 1);
		}
	}


	for ( %i = 0;  %i < $num["RLeg"];  %i++ )
	{
		%name = $RLeg[%i];
		
		if ( %name !$= "NULL" )
		{
			%time++;
			schedulePlayerIcon (%name, %time + 1);
		}
	}


	for ( %i = 0;  %i < $num["Chest"];  %i++ )
	{
		%name = $Chest[%i];
		
		if ( %name !$= "NULL" )
		{
			%time++;
			schedulePlayerIcon(%name, %time + 1);
		}
	}


	for ( %i = 0;  %i < $num["Hip"];  %i++ )
	{
		%name = $Hip[%i];
		
		if ( %name !$= "NULL" )
		{
			%time++;
			schedulePlayerIcon(%name, %time + 1);
		}
	}
}

function schedulePlayerIcon ( %meshName, %time )
{
	if ( !isObject($iconBrick) )
	{
		Con::errorf ("ERROR: schedulePlayerIcon() - no $iconBrick");  // gg baddy
		return;
	}

	%time *= 1000;

	$iconBrick.schedule (%time + 500, hideNode, "ALL");
	$iconBrick.schedule (%time + 600, unHideNode, %meshName);

	schedule (%time + 900, 0, eval, "$IconName = " @  %meshName  @ ";");
	schedule (%time + 1000, 0, doIconScreenshot);
}
