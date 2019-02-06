exec ("./registerInputEvent.cs");
exec ("./getTarget.cs");
exec ("./processInputEvent.cs");
exec ("./fxDTSBrick/events.cs");


function dumpInputEvents ( %class )
{
	%count = $InputEvent_Count[%class];
	echo ("Class " SPC  %class  SPC " has " @  %count  @ " registered input events");

	for ( %i = 0;  %i < %count;  %i++ )
	{
		echo ( "  " @  %i  @ ": " @  $InputEvent_Name[%class, %i] );
		%targetCount = getFieldCount ( $InputEvent_TargetList[%class, %i] );

		for ( %j = 0;  %j < %targetCount;  %j++ )
		{
			echo ( "    " @  getField ( $InputEvent_TargetList[%class, %i],  %j ) );
		}
	}
}
