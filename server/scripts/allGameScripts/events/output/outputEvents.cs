exec ("./registerOutputEvent.cs");
exec ("./verifyOutputParameterList.cs");

exec ("./fxDTSBrick/events.cs");
exec ("./eventEnabled.cs");
exec ("./player/player.cs");
exec ("./gameConnection.cs");
exec ("./miniGame.cs");
exec ("./projectile.cs");


function SimObject::cancelEvents ( %obj )
{
	for ( %i = 0;  %i < %obj.numScheduledEvents;  %i++ )
	{
		if ( isEventPending ( %obj.scheduledEvent[%i] ) )
		{
			cancel ( %obj.scheduledEvent[%i] );
		}
	}

	%obj.numScheduledEvents = 0;
}

function outputEvent_GetOutputEventIdx ( %targetClass, %outputName )
{
	for ( %i = 0;  %i < $OutputEvent_Count[%targetClass];  %i++ )
	{
		if ( $OutputEvent_Name[%targetClass, %i] $= %outputName )
		{
			return %i;
		}
	}

	return -1;
}

function outputEvent_GetOutputName ( %class, %idx )
{
	return $OutputEvent_Name[%class, %idx];
}

function outputEvent_GetNumParametersFromIdx ( %class, %idx )
{
	return getFieldCount ( $OutputEvent_parameterList[%class, %idx] );
}

function dumpOutputEvents ( %class )
{
	%count = $OutputEvent_Count[%class];
	echo ("Class " @  %class  @ " has " @  %count  @ " registered output events");


	for ( %i = 0;  %i < %count;  %i++ )
	{
		echo ( "  " @  %i  @ ": " @  $OutputEvent_Name[%class, %i] );
		%parameterCount = getFieldCount ( $OutputEvent_parameterList[%class, %i] );


		for ( %j = 0;  %j < %parameterCount;  %j++ )
		{
			echo ("    " @  getField ( $OutputEvent_parameterList[%class, %i],  %j ) );
		}

		echo ("    Append client: " @  $OutputEvent_AppendClient[%class, %i] );
	}
}


// =================
//  Register Events
// =================

registerOutputEvent ("fxDTSBrick", "cancelEvents", "");
registerOutputEvent ("fxDTSBrick", "setEventEnabled", "intList 157" TAB "bool");
registerOutputEvent ("fxDTSBrick", "toggleEventEnabled", "intList 176");
