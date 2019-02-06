function registerInputEvent ( %class, %name, %targetList, %adminOnly )
{
	%adminOnly = mFloor (%adminOnly);

	$InputEvent_Count[%class] = mFloor( $InputEvent_Count[%class] );

	for ( %i = 0;  %i < $InputEvent_Count[%class];  %i++ )
	{
		if ( $InputEvent_Name[%class, %i] $= %name )
		{
			$InputEvent_Name[%class, %i] = %name;
			$InputEvent_TargetList[%class, %i] = %targetList;
			$InputEvent_AdminOnly[%class, %i] = %adminOnly;

			return;
		}
	}

	%i = mFloor ( $InputEvent_Count[%class] );

	$InputEvent_Name[%class, %i] = %name;
	$InputEvent_TargetList[%class, %i] = %targetList;
	$InputEvent_AdminOnly[%class, %i] = %adminOnly;

	$InputEvent_Count[%class]++;


	if ( strstr($InputEvent_ClassList, %class) == -1 )
	{
		if ( $InputEvent_ClassList $= "" )
		{
			$InputEvent_ClassList = %class;
		}
		else
		{
			$InputEvent_ClassList = $InputEvent_ClassList SPC %class;
		}
	}
}

function unRegisterInputEvent ( %class, %name )
{
	%count = $InputEvent_Count[%class];

	for ( %i = 0;  %i < %count;  %i++ )
	{
		if ( $InputEvent_Name[%class, %i] $= %name )
		{
			for ( %j = %i + 1;  %j < %count;  %j++ )
			{
				$InputEvent_Name[%class, %j - 1] = $InputEvent_Name[%class, %j];
				$InputEvent_TargetList[%class, %j - 1] = $InputEvent_TargetList[%class, %j];
			}

			$InputEvent_Name[%class, %count - 1] = "";
			$InputEvent_TargetList[%class, %count - 1] = "";

			$InputEvent_Count[%class]--;

			echo ("Un-registering input event " @  %name  @ "\" from class " SPC  %class);
			return;
		}
	}

	error ("ERROR: unRegisterInputEvent() - event " @  %name  @ "\" is not registered on class " SPC  %class);
}
