function updateAddOnList ()
{
	echo ("\n--------- Updating Add-On List ---------");
	deleteVariables ("$AddOn__*");

	if ( isFile("config/server/ADD_ON_LIST.cs") )
	{
		exec ("config/server/ADD_ON_LIST.cs");
	}
	else
	{
		exec ("base/server/defaultAddOnList.cs");
	}

	if ( isFile("base/server/crapOns_Cache.cs") )
	{
		exec ("base/server/crapOns_Cache.cs");
	}

	%dir = "Add-Ons/*/server.cs";

	%fileCount = getFileCount (%dir);
	%filename = findFirstFile (%dir);

	for ( %i = 0;  %i < %fileCount;  %i++ )
	{
		%path = filePath (%filename);
		%dirName = getSubStr ( %path, strlen("Add-Ons/"), strlen(%path) - strlen("Add-Ons/") );
		%varName = getSafeVariableName (%dirName);

		echo ("Checking Add-On " @  %dirName);

		if ( !isValidAddOn(%dirName, 1) )
		{
			deleteVariables ("$AddOn__" @  %varName);
			%filename = findNextFile (%dir);
		}
		else
		{
			if ( mFloor($AddOn__[%varName]) <= 0 )
			{
				$AddOn__[%varName] = -1;
			}
			else
			{
				$AddOn__[%varName] = 1;
			}

			%filename = findNextFile(%dir);
		}
	}

	echo ("");
	export ("$AddOn__*", "config/server/ADD_ON_LIST.cs");
}

function verifyAddOnScripts ( %dirName )
{
	%pattern = "Add-Ons/" @  %dirName  @ "/*.cs";
	%file = findFirstFile (%pattern);

	while ( %file !$= "" )
	{
		if ( getFileLength(%file) > 0 )
		{
			if ( compile(%file) == 0 )
			{
				return 0;
			}
		}

		%file = findNextFile (%pattern);
	}

	return 1;
}

function RTB_registerPref ( %displayName, %category, %varName, %varType, %varCategory, %defaultValue, %a, %b )
{
	%varName = strreplace (%varName, "$", "");

	if ( %varName $= "" )
	{
		return;
	}

	%cmd = "$" @  %varName  @ " = " @  %defaultValue  @ ";";
	eval (%cmd);
}
