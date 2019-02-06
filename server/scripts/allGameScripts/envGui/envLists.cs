function buildEnvironmentLists ()
{
	buildSkyBoxList();
	buildWaterList();
	buildGroundList();
	buildSunFlareList();
	buildDayCycleList();
}

function buildSkyBoxList ()
{
	$EnvGuiServer::SkyCount = 0;
	%pattern = "Add-Ons/Sky_*/*.dml";
	%filename = findFirstFile (%pattern);

	while ( %filename !$= "" )
	{
		%thumbFile = filePath (%filename)  @ "/" @  fileBase (%filename)  @ "-thumb.jpg";

		if ( isFile(%thumbFile) )
		{
			$EnvGuiServer::Sky[$EnvGuiServer::SkyCount] = %filename;
			$EnvGuiServer::SkyCount++;
		}

		%filename = findNextFile (%pattern);
	}
}

function buildWaterList ()
{
	$EnvGuiServer::WaterCount = 0;
	$EnvGuiServer::Water[$EnvGuiServer::WaterCount] = "NONE";
	$EnvGuiServer::WaterCount++;

	%pattern = "Add-Ons/Water_*/*.water";
	%filename = findFirstFile (%pattern);

	while ( %filename !$= "" )
	{
		%thumbFile = filePath (%filename)  @ "/" @  fileBase (%filename)  @ "-thumb.jpg";

		if ( isFile(%thumbFile) )
		{
			$EnvGuiServer::Water[$EnvGuiServer::WaterCount] = %filename;
			$EnvGuiServer::WaterCount++;
		}

		%filename = findNextFile (%pattern);
	}
}

function buildGroundList ()
{
	$EnvGuiServer::GroundCount = 0;
	%pattern = "Add-Ons/Ground_*/*.ground";
	%filename = findFirstFile (%pattern);

	while ( %filename !$= "" )
	{
		%thumbFile = filePath (%filename)  @ "/" @  fileBase (%filename)  @ "-thumb.jpg";

		if ( isFile(%thumbFile) )
		{
			$EnvGuiServer::Ground[$EnvGuiServer::GroundCount] = %filename;
			$EnvGuiServer::GroundCount++;
		}

		%filename = findNextFile (%pattern);
	}
}

function buildSunFlareList ()
{
	$EnvGuiServer::SunFlareCount = 0;

	$EnvGuiServer::SunFlare[$EnvGuiServer::SunFlareCount] = "base/lighting/corona.png";
	$EnvGuiServer::SunFlareCount++;
	$EnvGuiServer::SunFlare[$EnvGuiServer::SunFlareCount] = "base/lighting/corona2.png";
	$EnvGuiServer::SunFlareCount++;
	$EnvGuiServer::SunFlare[$EnvGuiServer::SunFlareCount] = "base/lighting/flare.jpg";
	$EnvGuiServer::SunFlareCount++;
	$EnvGuiServer::SunFlare[$EnvGuiServer::SunFlareCount] = "base/lighting/flare2.png";
	$EnvGuiServer::SunFlareCount++;
	$EnvGuiServer::SunFlare[$EnvGuiServer::SunFlareCount] = "base/lighting/lightFalloffMono.png";
	$EnvGuiServer::SunFlareCount++;

	%pattern = "Add-Ons/SunFlare_*/*.png";
	%filename = findFirstFile (%pattern);

	while ( %filename !$= "" )
	{
		%thumbFile = filePath (%filename)  @ "/" @  fileBase (%filename)  @ "-thumb.jpg";

		if ( isFile(%thumbFile) )
		{
			$EnvGuiServer::SunFlare[$EnvGuiServer::SunFlareCount] = %filename;
			$EnvGuiServer::SunFlareCount++;
		}

		%filename = findNextFile (%pattern);
	}
}


function serverCmdEnvGui_RequestLists ( %client )
{
	if ( !%client.isAdmin )
	{
		return;
	}

	if ( mFloor($EnvGuiServer::SkyCount) <= 0 )
	{
		buildEnvironmentLists();
	}

	commandToClient (%client, 'EnvGui_ClearLists');

	for ( %i = 0;  %i < $EnvGuiServer::SkyCount;  %i++ )
	{
		commandToClient ( %client, 'EnvGui_AddSky', $EnvGuiServer::Sky[%i] );
	}

	for ( %i = 0;  %i < $EnvGuiServer::WaterCount;  %i++ )
	{
		commandToClient ( %client, 'EnvGui_AddWater', $EnvGuiServer::Water[%i] );
	}

	for ( %i = 0;  %i < $EnvGuiServer::GroundCount;  %i++ )
	{
		commandToClient ( %client, 'EnvGui_AddGround', $EnvGuiServer::Ground[%i] );
	}

	for ( %i = 0;  %i < $EnvGuiServer::SunFlareCount;  %i++ )
	{
		commandToClient ( %client, 'EnvGui_AddSunFlare', $EnvGuiServer::SunFlare[%i] );
	}

	for ( %i = 0;  %i < $EnvGuiServer::DayCycleCount;  %i++ )
	{
		commandToClient ( %client, 'EnvGui_AddDayCycle', $EnvGuiServer::DayCycle[%i] );
	}

	serverCmdEnvGui_RequestCurrentVars (%client);
}
