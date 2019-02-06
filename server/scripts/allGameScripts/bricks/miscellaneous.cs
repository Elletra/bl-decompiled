function fxDTSBrick::explode ( %obj )
{
	%obj.delete();
}


function fxDTSBrick::onColorChange ( %obj )
{
	%data = %obj.getDataBlock();
	%data.onColorChange (%obj);
}

function fxDTSBrickData::onColorChange ( %data, %obj )
{
	if ( isObject(%obj.emitter) )
	{
		%obj.emitter.setColor ( getColorIDTable(%obj.colorID) );
	}
}

function serverCmdTripOut ( %client )
{
	if ( !%client.isAdmin )
	{
		return;
	}


	%count = MissionCleanup.getCount();

	for ( %i = 0;  %i < %count;  %i++ )
	{
		%obj = MissionCleanup.getObject (%i);

		if ( %obj.getClassName() $= "fxDTSBrick" )
		{
			%obj.setColorFX (6);
			%obj.setShapeFX (1);
		}
	}
}
