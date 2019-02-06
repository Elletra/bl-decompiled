function doAllIcons ( %pos )
{
	if ( %pos > 0 )
	{
		doIconScreenshot();
	}

	%numDataBlocks = getDataBlockGroupSize();
	%brickData = 0;

	%pos++;

	while ( %pos < %numDataBlocks )
	{
		%checkDB = getDataBlock (%pos);

		if ( %checkDB.getClassName() $= "fxDTSBrickData" )
		{
			%brickData = %checkDB;
		}

		%pos++;
	}

	if ( isObject($iconBrick) )
	{
		$iconBrick.delete();
	}

	if ( %brickData != 0 )
	{
		$iconBrick = new fxDTSBrick()
		{
			dataBlock = %brickData;
			isPlanted = 1;
		};
		$iconBrick.setTransform ("0 10 -1005 0 0 1 -1.57");

		schedule (1000, 0, doAllIcons, %pos);
	}
}


function serverCmdDoAllIcons ( %client )
{
	if ( !%client.isAdmin )
	{
		return;
	}

	echo ("doing all icons!");
	doAllIcons (0);
}
