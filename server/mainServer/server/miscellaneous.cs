function ServerPlay2D ( %profile )
{
	%count = ClientGroup.getCount();

	for ( %idx = 0;  %idx < %count;  %idx++ )
	{
		%client = ClientGroup.getObject(%idx);

		%client.play2D(%profile);
	}
}

function ServerPlay3D ( %profile, %transform )
{
	%count = ClientGroup.getCount();

	for ( %idx = 0;  %idx < %count;  %idx++ )
	{
		%client = ClientGroup.getObject(%idx);

		%client.play3D(%profile, %transform);
	}
}

function deactivateServerPackages ()
{
	while ( ( %numActivePackages = getNumActivePackages() )  >  $numClientPackages )
	{
		%serverPackages = "";

		for ( %i = $numClientPackages;  %i < %numActivePackages;  %i++ )
		{
			%serverPackages = %serverPackages TAB getActivePackage (%i);
		}

		%serverPackages = trim (%serverPackages);
		%count = getFieldCount (%serverPackages);

		for ( %i = 0;  %i < %count;  %i++ )
		{
			%field = getField (%serverPackages, %i);
			deactivatePackage (%field);
		}
	}

	resetAllOpCallFunc();
}

function resetServerDefaults ()
{
	echo ("Resetting server defaults...");
	exec ("~/server/defaults.cs");
	exec ("config/server/prefs.cs");

	loadMission ($Server::MissionFile);
}

function addToServerGuidList ( %guid )
{
	%count = getFieldCount ($Server::GuidList);

	for ( %i = 0;  %i < %count;  %i++ )
	{
		if ( getField($Server::GuidList, %i) == %guid )
		{
			return;
		}
	}

	if ( $Server::GuidList $= "" )
	{
		$Server::GuidList = %guid;
	}
	else
	{
		$Server::GuidList = $Server::GuidList TAB %guid;
	}
}

function removeFromServerGuidList ( %guid )
{
	%count = getFieldCount ($Server::GuidList);

	for ( %i = 0;  %i < %count;  %i++ )
	{
		if ( getField($Server::GuidList, %i) == %guid )
		{
			$Server::GuidList = removeField ($Server::GuidList, %i);
			return;
		}
	}
}

function onServerInfoQuery ()
{
	return "Doing Ok";  // lol
}
