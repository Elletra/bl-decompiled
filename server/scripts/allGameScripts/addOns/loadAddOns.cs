function loadAddOns ()
{
	echo ("");

	updateAddOnList();

	echo ("---------  Loading Add-Ons ---------");
	deleteVariables ("$AddOnLoaded__*");

	%dir = "Add-Ons/*/server.cs";

	%fileCount = getFileCount (%dir);
	%filename = findFirstFile (%dir);

	%dirCount = 0;

	// RTB prioritizing but no official support for any of its features???

	if ( isFile("Add-Ons/System_ReturnToBlockland/server.cs") )
	{
		%dirNameList[%dirCount] = "System_ReturnToBlockland";
		%dirCount++;
	}

	while ( %filename !$= "" )
	{
		%path = filePath (%filename);
		%dirName = getSubStr ( %path, strlen("Add-Ons/"), strlen(%path) - strlen("Add-Ons/") );

		if ( %dirName $= "System_ReturnToBlockland" )
		{
			%filename = findNextFile (%dir);
		}
		else
		{
			%dirNameList[%dirCount] = %dirName;
			%dirCount++;
			%filename = findNextFile (%dir);
		}
	}


	for ( %addOnItr = 0;  %addOnItr < %dirCount;  %addOnItr++ )
	{
		%dirName = %dirNameList[%addOnItr];
		%varName = getSafeVariableName (%dirName);

		if ( !$Server::Dedicated )
		{
			if ( getRealTime() - $lastProgressBarTime > 200 )
			{
				LoadingProgress.setValue (%addOnItr / %dirCount);
				$lastProgressBarTime = getRealTime();
				Canvas.repaint();
			}
		}

		if ( $AddOn__[%varName] !$= ""  &&  isValidAddOn(%dirName)  &&  $AddOn__[%varName] $= 1 )
		{
			if ( %dirName $= "JVS_Content" )
			{
				if ( $AddOn__["Support_LegacyDoors"] $= 1 )
				{
					echo ("  Skipping JVS_Content in favor of Support_LegacyDoors");
				}
				else
				{
					// Nada
				}
			}

			if ( $AddOnLoaded__[%varName] != 1 )
			{
				$AddOnLoaded__[%varName] = 1;

				%zipFile = "Add-Ons/" @  %dirName  @ ".zip";

				if ( isFile(%zipFile) )
				{
					%zipCRC = getFileCRC (%zipFile);
					echo ("\c4Loading Add-On: " @  %dirName  @ " \c1(CRC:" @  %zipCRC  @ ")");
				}
				else
				{
					echo("\c4Loading Add-On: " @ %dirName);
				}

				if ( verifyAddOnScripts(%dirName) == 0 )
				{
					echo ("\c2ADD-ON " @  %dirName  @ " CONTAINS SYNTAX ERRORS\n");
				}
				else
				{
					%oldDBCount = DataBlockGroup.getCount();

					exec ("Add-Ons/" @  %dirName  @ "/server.cs");

					%dbDiff = DataBlockGroup.getCount() - %oldDBCount;

					echo ("\c1" @  %dbDiff  @ " datablocks added.");
					echo ("");
				}
			}
		}
	}

	echo ("");
}

function loadGameModeAddOns ()
{
	echo ("");
	echo ("---------  Loading Add-Ons (Game Mode) ---------");

	deleteVariables ("$AddOnLoaded__*");

	for ( %i = 0;  %i < $GameMode::AddOnCount;  %i++ )
	{
		%dirName = $GameMode::AddOn[%i];
		%varName = getSafeVariableName (%dirName);

		if ( !$Server::Dedicated )
		{
			if ( getRealTime() - $lastProgressBarTime > 200 )
			{
				LoadingProgress.setValue (%i / $GameMode::AddOnCount);
				$lastProgressBarTime = getRealTime();
				Canvas.repaint();
			}
		}

		if ( !isValidAddOn(%dirName) )
		{
			error ("ERROR: Invalid add-on \'" @  %dirName  @ "\' specified for game mode \'" @  $GameModeArg  @ "\'");
		}
		else
		{
			$AddOnLoaded__[%varName] = 1;
			%zipFile = "Add-Ons/" @  %dirName  @ ".zip";

			if ( isFile(%zipFile) )
			{
				%zipCRC = getFileCRC (%zipFile);
				echo ("\c4Loading Add-On: " @  %dirName  @ " \c1(CRC:" @  %zipCRC  @ ")");
			}
			else
			{
				echo ("\c4Loading Add-On: " @  %dirName);
			}

			if ( VerifyAddOnScripts(%dirName) == 0 )
			{
				echo ("\c2ADD-ON " @  %dirName  @ " CONTAINS SYNTAX ERRORS\n");
			}
			else
			{
				%oldDBCount = DataBlockGroup.getCount();
				exec ("Add-Ons/" @  %dirName  @ "/server.cs");

				%dbDiff = DataBlockGroup.getCount() - %oldDBCount;

				echo ("\c1" @  %dbDiff  @ " datablocks added.");
				echo ("");
			}
		}
	}

	echo ("");
}
