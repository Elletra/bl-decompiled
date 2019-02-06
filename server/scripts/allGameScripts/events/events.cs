exec ("./input/inputEvents.cs");
exec ("./output/outputEvents.cs");

exec ("./addEvent.cs");
exec ("./clearEvents.cs");
exec ("./requestEvents.cs");
exec ("./cancelEvents.cs");
exec ("./floodCheck.cs");


function SimObject::dumpEvents ( %obj )
{
	echo ("Object " @  %obj  @ " has " @  %obj.numEvents  @ " events");

	for ( %i = 0;  %i < %obj.numEvents;  %i++ )
	{
		echo ( "  " @  %i  @ ": " @  %obj.eventInput[%i]  @ " wait:" @  %obj.eventDelay[%i] );
		echo ( "    enabled: "    @  %obj.eventEnabled[%i] );
		echo ( "    target: "     @  %obj.eventTarget[%i]  @ " " @  %obj.eventNT[%i] );

		echo ( "    output:"      @  %obj.eventOutput[%i] SPC %obj.eventOutputParameter[%i, 1] SPC 
			%obj.eventOutputParameter[%i, 2] SPC %obj.eventOutputParameter[%i, 3] SPC %obj.eventOutputParameter[%i, 4] );
	}
}
