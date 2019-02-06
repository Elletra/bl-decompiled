function ServerCmdRequestWrenchEvents ( %client )
{
	%brick = %client.wrenchBrick;

	if ( !isObject(%brick)  &&  %client.isAdmin )
	{
		%brick = %client.adminWrenchBrick;
	}

	if ( !isObject(%brick) )
	{
		messageClient (%client, '', 'Wrench Error - RequestWrenchEvents: Brick no longer exists!');
		return;
	}

	for ( %i = 0;  %i < %brick.numEvents;  %i++ )
	{
		%line = %brick.serializeEvent (%i);
		commandToClient (%client, 'addEvent', %line);
	}

	commandToClient (%client, 'addEventsDone');
}

function SimObject::serializeEvent ( %obj, %idx )
{
	if ( %obj.eventTargetIdx[%idx] == -1 )
	{
		%group = %obj.getGroup();

		for ( %i = 0;  %i < %group.NTNameCount;  %i++ )
		{
			if ( %obj.eventNT[%idx] $= %group.NTName[%i] )
			{
				%NTNameIdx = %i;
				break;
			}
		}
	}
	else
	{
		%NTNameIdx = "";
	}

	%line = %obj.eventEnabled[%idx] TAB %obj.eventInputIdx[%idx] TAB %obj.eventDelay[%idx] TAB 
		%obj.eventTargetIdx[%idx] TAB %NTNameIdx TAB %obj.eventOutputIdx[%idx] TAB 
		%obj.eventOutputParameter[%idx, 1] TAB %obj.eventOutputParameter[%idx, 2] TAB 
		%obj.eventOutputParameter[%idx, 3] TAB %obj.eventOutputParameter[%idx, 4];

	return %line;
}

function SimObject::serializeEventToString ( %obj, %idx, %client )
{
	%line = %idx TAB %obj.eventEnabled[%idx] TAB %obj.eventInputIdx[%idx] TAB %obj.eventDelay[%idx] TAB 
		%obj.eventTargetIdx[%idx] TAB %obj.eventNT[%idx] TAB %obj.eventOutputIdx[%idx];


	if ( %obj.eventTargetIdx[%idx] == -1 )
	{
		%targetClass = "fxDTSBrick";
	}
	else
	{
		%targetClass = getWord ( getField ( $InputEvent_TargetList[ "fxDTSBrick", %obj.eventInputIdx[%idx] ], 
			%obj.eventTargetIdx[%idx] ),  1 );
	}


	for ( %i = 0;  %i < 4;  %i++ )
	{
		%field = getField ( $OutputEvent_parameterList[ %targetClass, %obj.eventOutputIdx[%idx] ], %i );
		%dataType = getWord (%field, 0);

		if ( %dataType $= "dataBlock" )
		{
			if ( isObject ( %obj.eventOutputParameter[%idx, %i + 1] ) )
			{
				%line = %line TAB ( %obj.eventOutputParameter[%idx, %i + 1] ).getName();
			}
			else
			{
				%line = %line TAB -1;
			}
		}
		else
		{
			%line = %line TAB %obj.eventOutputParameter[%idx, %i + 1];
		}
	}

	if ( isObject(%client) )
	{
		if ( %obj.eventTargetIdx[%idx] == -1 )
		{
			%outputName = outputEvent_GetOutputName ( "fxDTSBrick", %obj.eventOutputIdx[%idx] );

			if ( %outputName $= "setEmitter" )
			{
				%name = %obj.eventNT[%idx];
				%group = %obj.getGroup();

				for ( %j = 0;  %j < %group.NTObjectCount[%name];  %j++ )
				{
					%target = %group.NTObject[%name, %j];
					%ghostID = %client.getGhostID (%target);

					commandToClient(%client, 'TransmitEmitterDirection', %ghostID, %target.emitterDirection);
				}
			}
			else if ( %outputName $= "setItem" )
			{
				%name = %obj.eventNT[%idx];
				%group = %obj.getGroup();

				for ( %j = 0;  %j < %group.NTObjectCount[%name];  %j++ )
				{
					%target = %group.NTObject[%name,%j];
					%ghostID = %client.getGhostID (%target);

					commandToClient (%client, 'TransmitItemDirection', %ghostID, %target.itemPosition TAB 
						%target.itemDirection TAB %target.itemRespawnTime);
				}
			}
		}
		else
		{
			%targetClass = inputEvent_GetTargetClass ( "fxDTSBrick",  %obj.eventInputIdx[%idx],  %obj.eventTargetIdx[%idx] );

			if ( %targetClass $= "fxDTSBrick" )
			{
				%outputName = outputEvent_GetOutputName ( "fxDTSBrick",  %obj.eventOutputIdx[%idx] );

				if ( %outputName $= "setEmitter" )
				{
					%ghostID = %client.getGhostID (%obj);
					commandToClient (%client, 'TransmitEmitterDirection', %ghostID, %obj.emitterDirection);
				}
				else if ( %outputName $= "setItem" )
				{
					%ghostID = %client.getGhostID (%obj);

					commandToClient (%client, 'TransmitItemDirection', %ghostID, %obj.itemPosition TAB 
						%obj.itemDirection TAB %obj.itemRespawnTime);
				}
			}
		}
	}

	return %line;
}

function serverCmdRequestEventTables ( %client )
{
	%count = getWordCount ($InputEvent_ClassList);

	for ( %i = 0;  %i < %count;  %i++ )
	{
		%class = getWord ($InputEvent_ClassList, %i);

		for ( %j = 0;  %j < $InputEvent_Count[%class];  %j++ )
		{
			%name = $InputEvent_Name[%class, %j];
			%targetList = $InputEvent_TargetList[%class, %j];

			commandToClient (%client, 'RegisterInputEvent', %class, %name, %targetList);
		}
	}


	%count = getWordCount ($OutputEvent_ClassList);

	for ( %i = 0;  %i < %count;  %i++ )
	{
		%class = getWord ($OutputEvent_ClassList, %i);
		
		for ( %j = 0;  %j < $OutputEvent_Count[%class];  %j++ )
		{
			%name = $OutputEvent_Name[%class, %j];

			%parameterList = $OutputEvent_parameterList[%class, %j];
			%parameterListA = getSubStr (%parameterList, 0, 254);
			%parameterListB = getSubStr (%parameterList, 254, 254);
			%parameterListC = getSubStr (%parameterList, 508, 254);
			%parameterListD = getSubStr (%parameterList, 762, 254);

			commandToClient (%client, 'RegisterOutputEvent', %class, %name, %parameterListA, 
				%parameterListB, %parameterListC, %parameterListD);
		}
	}

	commandToClient (%client, 'RegisterEventsDone');
}
