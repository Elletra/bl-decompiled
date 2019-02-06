$Error::None = 1;
$Error::AddOn_Nested = -1;
$Error::AddOn_Disabled = -2;
$Error::AddOn_NotFound = -3;


function LoadRequiredAddOn ( %dirName )
{
	if ( %dirName $= "JVS_Content" )
	{
		if ( $GameModeArg $= "" )
		{
			if ( $AddOn__["Support_LegacyDoors"] == 1  ||  !isFile("add-ons/JVS_Content/server.cs")  ||  
				($AddOn__["Support_LegacyDoors"] != 1  &&  $AddOn__["JVS_Content"] != 1) )
			{
				%dirName = "Support_LegacyDoors";
			}
		}
		else
		{
			%foundJVSContent = 0;

			for ( %i = 0;  %i < $GameMode::AddOnCount;  %i++ )
			{
				if ( $GameMode::AddOn[%i] $= "JVS_Content" )
				{
					%foundJVSContent = 1;
				}
			}

			if ( !%foundJVSContent )
			{
				%dirName = "Support_LegacyDoors";
			}
		}
	}

	if ( strstr(%dirName, " ") != -1 )
	{
		%dirName = strreplace (%dirName, " ", "_");
	}

	if ( strstr(%dirName, "/") != -1 )
	{
		return $Error::AddOn_Nested;
	}

	%varName = getSafeVariableName (%dirName);

	if ( $GameModeArg !$= "" )
	{
		%foundIt = false;

		for ( %i = 0;  %i < $GameMode::AddOnCount;  %i++ )
		{
			if ( $GameMode::AddOn[%i] $= %dirName )
			{
				%foundIt = true;
				break;
			}
		}

		if ( !%foundIt )
		{
			error ("ERROR: LoadRequiredAddOn(\'" @  %dirName  @ 
				"\') - you can\'t force load an add-on that is not included in gamemode.txt");

			if ( GameWindowExists()  &&  !$Server::Dedicated )
			{
				schedule (11, 0, MessageBoxOK, "Game Mode Error", "Required add-on " @  %dirName  @ 
					" should be added to gamemode.txt");
			}

			if ( !isEventPending($disconnectEvent) )
			{
				$disconnectEvent = schedule (10, 0, disconnect);
			}

			return $Error::AddOn_NotFound;
		}
	}

	if ( $AddOnLoaded__[%varName] == 1 )
	{
		return $Error::None;
	}

	if ( $AddOn__[%varName] $= 1 )
	{
		%serverLaunchName = "Add-Ons/" @  %dirName  @ "/server.cs";

		if ( !isFile(%serverLaunchName) )
		{
			return $Error::AddOn_NotFound;
		}

		echo ("  Loading Add-On " @  %dirName);
		exec (%serverLaunchName);

		$AddOnLoaded__[%varName] = 1;

		return $Error::None;
	}
	else
	{
		return $Error::AddOn_Disabled;
	}
}
