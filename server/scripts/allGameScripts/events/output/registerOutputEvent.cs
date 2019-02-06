function registerOutputEvent ( %class, %name, %parameterList, %appendClient )
{
	if ( %appendClient $= "" )
	{
		%appendClient = 1;
	}

	$OutputEvent_Count[%class] = mFloor ( $OutputEvent_Count[%class] );

	for ( %i = 0;  %i < $OutputEvent_Count[%class];  %i++ )
	{
		if ( $OutputEvent_Name[%class, %i] $= %name )
		{
			echo ("registerOutputEvent() - Output event " @  %name  @ " already registered on class " SPC  %class  SPC " - overwriting.");

			$OutputEvent_Name[%class, %i] = %name;

			$OutputEvent_parameterList[%class, %i] = %parameterList;
			verifyOutputParameterList (%class, %i);

			$OutputEvent_AppendClient[%class, %i] = mFloor (%appendClient);

			return;
		}
	}

	%i = mFloor ( $OutputEvent_Count[%class] );

	$OutputEvent_Name[%class, %i] = %name;

	$OutputEvent_parameterList[%class, %i] = %parameterList;
	verifyOutputParameterList (%class, %i);

	$OutputEvent_AppendClient[%class, %i] = mFloor (%appendClient);

	$OutputEvent_Count[%class]++;


	if ( strstr($OutputEvent_ClassList, %class) == -1 )
	{
		if ( $OutputEvent_ClassList $= "" )
		{
			$OutputEvent_ClassList = %class;
		}
		else
		{
			$OutputEvent_ClassList = $OutputEvent_ClassList SPC %class;
		}
	}
}

function unRegisterOutputEvent ( %class, %name )
{
	%count = $OutputEvent_Count[%class];

	for ( %i = 0;  %i < %count;  %i++ )
	{
		if ($OutputEvent_Name[%class, %i] $= %name)
		{

			for ( %j = %i + 1;  %j < %count;  %j++ )
			{
				$OutputEvent_Name[%class, %j - 1] = $OutputEvent_Name[%class, %j];
				$OutputEvent_parameterList[%class, %j - 1] = $OutputEvent_parameterList[%class, %j];
				$OutputEvent_AppendClient[%class, %j - 1] = $OutputEvent_AppendClient[%class, %j];
			}

			$OutputEvent_Name[%class, %count - 1] = "";
			$OutputEvent_parameterList[%class, %count - 1] = "";
			$OutputEvent_AppendClient[%class, %count - 1] = "";
			$OutputEvent_Count[%class]--;

			echo ("Un-registering output event " @  %name  @ "\" from class " SPC  %class);
			return;
		}
	}

	error ("ERROR: unRegisterOutputEvent() - event " @  %name  @ "\" is not registered on class " SPC  %class);
}
