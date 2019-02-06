function inputEvent_GetInputEventIdx ( %inputEventName )
{
	%class = "fxDTSBrick";

	for ( %x = 0;  %x < $InputEvent_Count[%class];  %x++ )
	{
		if ( $InputEvent_Name[%class, %x] $= %inputEventName )
		{
			return %x;
		}
	}

	return -1;
}

function inputEvent_GetTargetIndex ( %class, %i, %targetName )
{
	%fieldCount = getFieldCount ( $InputEvent_TargetList[%class, %i] );

	for ( %x = 0;  %x < %fieldCount;  %x++ )
	{
		%field = getField ( $InputEvent_TargetList[%class, %i],  %x );
		%name = getWord (%field, 0);

		if ( %name $= %targetName )
		{
			return %x;
		}
	}

	return -1;
}

function inputEvent_GetTargetClass ( %class, %idx, %targetIdx )
{
	%targetList = $InputEvent_TargetList[%class, %idx];

	%field = getField (%targetList, %targetIdx);
	%class = getWord (%field, 1);

	return %class;
}

function inputEvent_GetTargetName ( %class, %idx, %targetIdx )
{
	%targetList = $InputEvent_TargetList[%class, %idx];

	%field = getField (%targetList, %targetIdx);
	%name = getWord (%field, 0);

	return %name;
}
