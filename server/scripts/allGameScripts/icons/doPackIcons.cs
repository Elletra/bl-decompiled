datablock StaticShapeData (dummyPlayer)
{
	category = "Misc";
	shapeFile = "~/data/shapes/player/m.dts";
};


function serverCmdDoPackIcons ( %client )
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

	$iconBrick.setTransform ("0 10 -1006 0 0 1 1.57");
	$iconBrick.setScale ("0.5 0.5 0.5");
	$iconBrick.setNodeColor ("ALL", "1 1 1 1");
	$iconBrick.hideNode ("ALL");


	%time = -1;

	for ( %i = 0;  %i < $numPack;  %i++ )
	{
		%name = $pack[%i];

		if ( %name !$= "NULL" )
		{
			%time++;
			schedulePlayerIcon (%name, %time + 1);
		}
	}
}

function serverCmdDoSecondPackIcons ( %client )
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

	$iconBrick.setTransform ("0 10 -1006 0 0 1 1.57");
	$iconBrick.setScale ("0.5 0.5 0.5");
	$iconBrick.setNodeColor ("ALL", "1 1 1 1");
	$iconBrick.hideNode ("ALL");


	%time = -1;

	for ( %i = 0;  %i < $numSecondPack;  %i++ )
	{
		%name = $SecondPack[%i];

		if ( %name !$= "NULL"  &&  %name !$= "none" )
		{
			%time++;
			schedulePlayerIcon (%name, %time + 1);
		}
	}
}
