function doIconScreenshot ()
{
	%oldContent = Canvas.getContent();
	Canvas.setContent (noHudGui);

	noHudGui.setHasRendered (1);
	PlayGui.setHasRendered (1);

	if ( $iconBrick.getClassName() $= "fxDTSBrick" )
	{
		%brickName = $iconBrick.getDataBlock().uiName;
	}
	else if ( $IconName !$= "" )
	{
		%brickName = $IconName;
	}
	else
	{
		%brickName = "ERROR";
	}

	screenShot ("iconShots/" @  %brickName  @ ".png", "PNG", 1);
	Canvas.setContent (%oldContent);
}


function serverCmdDoIcon ( %client, %brickName )
{
	if ( !%client.isAdmin )
	{
		return;
	}

	%brickData = "Brick" @  %brickName  @ "Data";

	if ( isObject($iconBrick) )
	{
		$iconBrick.delete();
	}

	$iconBrick = new fxDTSBrick()
	{
		dataBlock = %brickData;
		isPlanted = 1;
	};
	MissionCleanup.add ($iconBrick);

	$iconBrick.setTransform ("0 10 -1005 0 0 1 " @  %brickData.orientationFix * $piOver2);
	schedule (3000, 0, doIconScreenshot);
}

function serverCmdDoItemIcon ( %client, %data )
{
	if ( !%client.isAdmin )
	{
		return;
	}

	%camera = %client.Camera;

	%client.setControlObject (%camera);
	%camera.setMode ("Observer");
	%camera.setTransform ("-1.874 8.050 -1003.7 0.47718 -0.197864 0.856242 0.901965");

	if ( isObject($iconBrick) )
	{
		$iconBrick.delete();
	}

	$IconName = %data.uiName;

	$iconBrick = new Item ()
	{
		static = 1;
		rotate = 0;
		dataBlock = %data;
	};
	MissionCleanup.add ($iconBrick);

	$iconBrick.setTransform ("0 10 -1005 0 0 1 -1.57");
	$iconBrick.setNodeColor ("ALL", "1 1 1 1");
	$iconBrick.schedule (100, setNodeColor, "ALL", "1 1 1 1");

	schedule (1000, 0, doIconScreenshot);
}
