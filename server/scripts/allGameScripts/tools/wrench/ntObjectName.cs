function SimObject::setNTObjectName ( %obj, %name )
{
	%name = trim (%name);
	%name = getSafeVariableName (%name);

	if ( strlen(%name) < 1 )
	{
		%obj.clearNTObjectName();
		%obj.setName ("");

		return;
	}

	if ( getSubStr (%name, 0, 1) !$= "_" )
	{
		%name = "_" @ %name;
	}


	if ( isObject(%name) )
	{
		if ( !%name.getType() & $TypeMasks::FxBrickAlwaysObjectType )
		{
			error ("ERROR: SimObject::setNTObjectName() - Non-Brick object named " @  %name  @ " already exists!");
			return;
		}
	}

	if ( %obj.getName() $= %name )
	{
		return;
	}


	%obj.clearNTObjectName();

	%group = %obj.getGroup();
	%group.NTObjectCount[%name] = mFloor ( %group.NTObjectCount[%name] );

	if ( %group.NTObjectCount[%name] <= 0 )
	{
		%group.addNTName (%name);
	}

	%group.NTObject[ %name,  %group.NTObjectCount[%name] ] = %obj;
	%group.NTObjectCount[%name]++;

	%obj.setName (%name);
}

function SimObject::clearNTObjectName ( %obj )
{
	%group = %obj.getGroup();

	if ( !isObject(%group) )
	{
		return;
	}

	%oldName = %obj.getName();

	if ( %oldName $= "" )
	{
		return;
	}


	for ( %i = 0;  %i < %group.NTObjectCount[%oldName];  %i++ )
	{
		if ( %group.NTObject[%oldName, %i] == %obj )
		{
			%group.NTObject[%oldName, %i] = %group.NTObject[%oldName, %group.NTObjectCount[%oldName] - 1];
			%group.NTObject[%oldName, %group.NTObjectCount[%oldName] - 1] = "";

			%group.NTObjectCount[%oldName]--;
			%i--;
		}
	}

	if ( %group.NTObjectCount[%oldName] <= 0 )
	{
		%group.removeNTName (%oldName);
	}
}

function SimGroup::addNTName ( %obj, %name )
{
	%obj.NTNameCount = mFloor (%obj.NTNameCount);

	for ( %i = 0;  %i < %obj.NTNameCount;  %i++ )
	{
		if ( %obj.NTName[%i] $= %name )
		{
			return;
		}
	}

	%obj.NTName[%obj.NTNameCount] = %name;
	%obj.NTNameCount++;


	%count = ClientGroup.getCount();

	for ( %i = 0;  %i < %count;  %i++ )
	{
		%client = ClientGroup.getObject (%i);

		if ( isObject(%client.brickGroup) )
		{
			if ( %client.brickGroup.getId() == %obj.getId() )
			{
				commandToClient (%client, 'AddNTName', %name);
			}
		}
	}
}

function SimGroup::removeNTName ( %obj, %name )
{
	%obj.NTNameCount = mFloor (%obj.NTNameCount);

	for ( %i = 0;  %i < %obj.NTNameCount;  %i++ )
	{
		if ( %obj.NTName[%i] $= %name )
		{
			%obj.NTName[%i] = %obj.NTName[%obj.NTNameCount - 1];
			%obj.NTName[%obj.NTNameCount - 1] = "";

			%obj.NTNameCount--;

			break;
		}
	}


	%count = ClientGroup.getCount();

	for ( %i = 0;  %i < %count;  %i++ )
	{
		%client = ClientGroup.getObject (%i);

		if ( isObject(%client.brickGroup) )
		{
			if ( %client.brickGroup.getId() == %obj.getId() )
			{
				commandToClient (%client, 'RemoveNTName', %name);
			}
		}
	}
}

function SimGroup::DumpNTNames ( %obj )
{
	echo ("Group " @  %obj.getName()  @ " has " @  %obj.NTNameCount  @ " NTNames:");


	for ( %i = 0;  %i < %obj.NTNameCount;  %i++ )
	{
		%name = %obj.NTName[%i];
		%count = %obj.NTObjectCount[%name];

		echo ("  " @  %name  @ ": " @  %count  @ " objects");

		for ( %j = 0;  %j < %count;  %j++ )
		{
			echo( "    " @  %obj.NTObject[%name, %j]  @ " : " @  isObject ( %obj.NTObject[%name, %j] ) );
		}
	}
}

function SimGroup::ClearAllNTNames ( %obj )
{
	for ( %i = 0;  %i < %obj.NTNameCount;  %i++ )
	{
		%name = %obj.NTName[%i];
		%count = %obj.NTObjectCount[%name];

		for ( %j = 0;  %j < %count;  %j++ )
		{
			%obj.NTObject[%name, %j] = "";
		}

		%obj.NTName[%i] = "";
		%obj.NTObjectCount[%name] = "";
	}

	%obj.NTNameCount = 0;
}


function ServerCmdRequestNamedTargets ( %client )
{
	%group = %client.brickGroup;
	%count = %group.NTNameCount;

	for ( %i = 0;  %i < %count;  %i++ )
	{
		%name = %group.NTName[%i];
		commandToClient (%client, 'AddNTName', %name);
	}
}
