function buildDayCycleList ()
{
	$EnvGuiServer::DayCycleCount = 0;
	%pattern = "Add-Ons/DayCycle_*/*.daycycle";
	%filename = findFirstFile (%pattern);

	while ( %filename !$= "" )
	{
		$EnvGuiServer::DayCycle[$EnvGuiServer::DayCycleCount] = %filename;
		$EnvGuiServer::DayCycleCount++;

		%filename = findNextFile (%pattern);
	}
}

function loadDayCycle ( %filename )
{
	if ( !validateEnvironmentObjects() )
	{
		return;
	}

	if ( !isFile(%filename) )
	{
		error ("ERROR: loadDayCycle(" @  %filename  @ ") - file not found");
		return;
	}


	for ( %i = 0;  %i < 20;  %i++ )
	{
		DayCycle.targetFraction[%i] = 0;
		DayCycle.targetDirectColor[%i] = "0 0 0 0";
		DayCycle.targetAmbientColor[%i] = "0 0 0 0";
		DayCycle.targetSkyColor[%i] = "0 0 0 0";
		DayCycle.targetFogColor[%i] = "0 0 0 0";
		DayCycle.targetShadowColor[%i] = "0 0 0 0";
		DayCycle.targetSunFlareColor[%i] = "0 0 0 0";
		DayCycle.targetUseDefaultVector[%i] = 0;
	}

	%idx = -1;
	%file = new FileObject();
	%file.openForRead (%filename);

	%line = %file.readLine();

	while ( !%file.isEOF() )
	{
		%label = getWord (%line, 0);
		%value = getWords (%line, 1, 999);
		%label = trim (%label);

		if ( %label $= "" )
		{
			%line = %file.readLine();
			continue;
		}
		else if ( getSubStr(%label, 0, 2) $= "//" )
		{
			%line = %file.readLine();
			continue;
		}
		else if (%label $= "FRACTION")
		{
			%idx = %idx + 1.0;
			DayCycle.targetFraction[%idx] = mClampF (%value, 0, 1);
		}
		else if (%label $= "DIRECTCOLOR")
		{
			DayCycle.targetDirectColor[%idx] = getColorI (%value);
		}
		else if (%label $= "AMBIENTCOLOR")
		{
			DayCycle.targetAmbientColor[%idx] = getColorI (%value);
		}
		else if (%label $= "SKYCOLOR")
		{
			DayCycle.targetSkyColor[%idx] = getColorI (%value);
		}
		else if (%label $= "FOGCOLOR")
		{
			DayCycle.targetFogColor[%idx] = getColorI (%value);
		}
		else if (%label $= "SHADOWCOLOR")
		{
			DayCycle.targetShadowColor[%idx] = getColorI (%value);
		}
		else if (%label $= "SUNFLARECOLOR")
		{
			DayCycle.targetSunFlareColor[%idx] = getColorI (%value);
		}
		else if (%label $= "USEDEFAULTVECTOR")
		{
			DayCycle.targetUseDefaultVector[%idx] = mClamp (%value, 0, 1);
		}
		else
		{
			error ("WARNING: loadDayCycle(" @  %filename  @ ") - unknown label " @  %label  @ "");
		}

		%line = %file.readLine();
	}

	DayCycle.sendUpdate();

	%file.close();
	%file.delete();
}


function serverCmdEnvGui_RequestCurrent ( %client )
{
	if ( !%client.isAdmin )
	{
		return;
	}

	if ( !isObject(DayCycle) )
	{
		return;
	}

	if ( DayCycle.getClassName() !$= "fxDayCycle" )
	{
		return;
	}

	commandToClient (%client, 'EnvGui_UpdateDayLength', DayCycle.dayLength);
	commandToClient (%client, 'EnvGui_UpdateDayOffset', DayCycle.dayOffset);
}
