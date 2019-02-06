function fxDTSBrick::fireRelay ( %obj, %client )
{
	%currTime = getSimTime();

	if ( atoi(%currTime) - atoi(%obj.lastRelayTime) < 15 )
	{
		return;
	}

	%obj.lastRelayTime = %currTime;
	%obj.onRelay (%client);
}

function fxDTSBrick::fireRelayUp ( %obj, %client )
{
	%WB = %obj.getWorldBox();

	%sizeX = getWord (%WB, 3) - getWord (%WB, 0) - 0.1;
	%sizeY = getWord (%WB, 4) - getWord (%WB, 1) - 0.1;
	%sizeZ = getWord (%WB, 5) - getWord (%WB, 2) - 0.1;

	%size = %sizeX SPC %sizeY SPC 0.1;

	%pos = %obj.getPosition();

	%posX = getWord (%pos, 0);
	%posY = getWord (%pos, 1);
	%posZ = getWord (%pos, 2) + %sizeZ / 2 + 0.05;

	%pos = %posX SPC %posY SPC %posZ;

	%obj.fireRelayFromBox (%pos, %size, %client);
}

function fxDTSBrick::fireRelayDown ( %obj, %client )
{
	%WB = %obj.getWorldBox();

	%sizeX = getWord (%WB, 3) - getWord (%WB, 0) - 0.1;
	%sizeY = getWord (%WB, 4) - getWord (%WB, 1) - 0.1;
	%sizeZ = getWord (%WB, 5) - getWord (%WB, 2) - 0.1;

	%size = %sizeX SPC %sizeY SPC 0.1;

	%pos = %obj.getPosition();

	%posX = getWord (%pos, 0);
	%posY = getWord (%pos, 1);
	%posZ = getWord (%pos, 2) - %sizeZ / 2 - 0.05;

	%pos = %posX SPC %posY SPC %posZ;

	%obj.fireRelayFromBox (%pos, %size, %client);
}

function fxDTSBrick::fireRelayNorth ( %obj, %client )
{
	%WB = %obj.getWorldBox();

	%sizeX = getWord (%WB, 3) - getWord (%WB, 0) - 0.1;
	%sizeY = getWord (%WB, 4) - getWord (%WB, 1) - 0.1;
	%sizeZ = getWord (%WB, 5) - getWord (%WB, 2) - 0.1;

	%size = %sizeX SPC 0.1 SPC %sizeZ;

	%pos = %obj.getPosition();

	%posX = getWord (%pos, 0);
	%posY = getWord (%pos, 1) + %sizeY / 2 + 0.05;
	%posZ = getWord (%pos, 2);

	%pos = %posX SPC %posY SPC %posZ;

	%obj.fireRelayFromBox (%pos, %size, %client);
}

function fxDTSBrick::fireRelaySouth ( %obj, %client )
{
	%WB = %obj.getWorldBox();

	%sizeX = getWord (%WB, 3) - getWord (%WB, 0) - 0.1;
	%sizeY = getWord (%WB, 4) - getWord (%WB, 1) - 0.1;
	%sizeZ = getWord (%WB, 5) - getWord (%WB, 2) - 0.1;

	%size = %sizeX SPC 0.1 SPC %sizeZ;

	%pos = %obj.getPosition();

	%posX = getWord (%pos, 0);
	%posY = getWord (%pos, 1) - %sizeY / 2 - 0.05;
	%posZ = getWord (%pos, 2);

	%pos = %posX SPC %posY SPC %posZ;

	%obj.fireRelayFromBox (%pos, %size, %client);
}

function fxDTSBrick::fireRelayEast ( %obj, %client )
{
	%WB = %obj.getWorldBox();

	%sizeX = getWord (%WB, 3) - getWord (%WB, 0) - 0.1;
	%sizeY = getWord (%WB, 4) - getWord (%WB, 1) - 0.1;
	%sizeZ = getWord (%WB, 5) - getWord (%WB, 2) - 0.1;

	%size = 0.1 SPC %sizeY SPC %sizeZ;

	%pos = %obj.getPosition();

	%posX = getWord (%pos, 0) + %sizeX / 2 + 0.05;
	%posY = getWord (%pos, 1);
	%posZ = getWord (%pos, 2);

	%pos = %posX SPC %posY SPC %posZ;

	%obj.fireRelayFromBox (%pos, %size, %client);
}

function fxDTSBrick::fireRelayWest ( %obj, %client )
{
	%WB = %obj.getWorldBox();

	%sizeX = getWord (%WB, 3) - getWord (%WB, 0) - 0.1;
	%sizeY = getWord (%WB, 4) - getWord (%WB, 1) - 0.1;
	%sizeZ = getWord (%WB, 5) - getWord (%WB, 2) - 0.1;

	%size = 0.1 SPC %sizeY SPC %sizeZ;

	%pos = %obj.getPosition();

	%posX = getWord (%pos, 0) - %sizeX / 2 - 0.05;
	%posY = getWord (%pos, 1);
	%posZ = getWord (%pos, 2);

	%pos = %posX SPC %posY SPC %posZ;

	%obj.fireRelayFromBox (%pos, %size, %client);
}

function fxDTSBrick::fireRelayFromBox ( %obj, %pos, %size, %client )
{
	%mask = $TypeMasks::FxBrickAlwaysObjectType;
	%group = %obj.getGroup();

	initContainerBoxSearch (%pos, %size, %mask);

	while ( ( %searchObj = containerSearchNext() ) != 0 )
	{
		if ( %searchObj.getGroup() == %group  &&  %searchObj != %obj )
		{
			if ( %searchObj.numEvents > 0 )
			{
				%obj.addScheduledEvent ( %searchObj.schedule (33, fireRelay, %client) );
			}
		}
	}
}


// =================
//  Register Events
// =================

registerOutputEvent ("fxDTSBrick", "fireRelay", "");
registerOutputEvent ("fxDTSBrick", "fireRelayUp", "");
registerOutputEvent ("fxDTSBrick", "fireRelayDown", "");
registerOutputEvent ("fxDTSBrick", "fireRelayNorth", "");
registerOutputEvent ("fxDTSBrick", "fireRelayEast", "");
registerOutputEvent ("fxDTSBrick", "fireRelaySouth", "");
registerOutputEvent ("fxDTSBrick", "fireRelayWest", "");
