function fxDTSBrick::sendWrenchData ( %obj, %client )
{
	%data = "";
	%name = %obj.getName();

	if ( %name !$= "" )
	{
		%data = %data  TAB "N" SPC %name;
	}
	else
	{
		%data = %data  TAB "N" SPC " ";
	}

	if ( isObject(%obj.light) )
	{
		%lightDB = %obj.light.getDataBlock();
	}
	else
	{
		%lightDB = 0;
	}


	%data = %data  TAB "LDB" SPC %lightDB;

	if ( isObject(%obj.emitter) )
	{
		if ( isObject(%obj.emitter.emitter) )
		{
			%emitterDB = %obj.emitter.emitter.getId();
		}
		else
		{
			%emitterDB = 0;
		}
	}
	else
	{
		%emitterDB = 0;
	}


	%data = %data  TAB "EDB" SPC %emitterDB;
	%data = %data  TAB "EDIR" SPC mFloor (%obj.emitterDirection);

	if ( isObject(%obj.Item) )
	{
		%itemDB = %obj.Item.getDataBlock();
	}
	else
	{
		%itemDB = 0;
	}


	%data = %data TAB "IDB" SPC %itemDB;
	%data = %data TAB "IPOS" SPC mFloor (%obj.itemPosition);

	if ( %obj.itemDirection $= "" )
	{
		%obj.itemDirection = 2;
	}

	%data = %data  TAB "IDIR" SPC mFloor (%obj.itemDirection);

	if ( %obj.itemRespawnTime $= "" )
	{
		%obj.itemRespawnTime = 4000;
	}

	%data = %data TAB "IRT" SPC mFloor (%obj.itemRespawnTime / 1000.0);
	%data = %data TAB "RC"  SPC %obj.isRayCasting();
	%data = %data TAB "C"   SPC %obj.isColliding();
	%data = %data TAB "R"   SPC %obj.isRendering();
	%data = trim (%data);

	commandToClient (%client, 'SetWrenchData', %data);
	commandToClient (%client, 'WrenchLoadingDone');
}
