function serverCmdEnvGui_RequestCurrentVars ( %client )
{
	if ( !%client.isAdmin )
	{
		return;
	}

	commandToClient (%client, 'EnvGui_SetVar', "SimpleMode", $EnvGuiServer::SimpleMode);
	commandToClient (%client, 'EnvGui_SetVar', "SkyIdx", $EnvGuiServer::SkyIdx);
	commandToClient (%client, 'EnvGui_SetVar', "WaterIdx", $EnvGuiServer::WaterIdx);
	commandToClient (%client, 'EnvGui_SetVar', "GroundIdx", $EnvGuiServer::GroundIdx);

	if ( !$EnvGuiServer::HasSetAdvancedOnce )
	{
		EnvGuiServer::readAdvancedVarsFromSimple();
	}

	commandToClient (%client, 'EnvGui_SetVar', "SunFlareTopIdx", $EnvGuiServer::SunFlareTopIdx);
	commandToClient (%client, 'EnvGui_SetVar', "SunFlareBottomIdx", $EnvGuiServer::SunFlareBottomIdx);
	commandToClient (%client, 'EnvGui_SetVar', "DayOffset", $EnvGuiServer::DayOffset);
	commandToClient (%client, 'EnvGui_SetVar', "DayLength", $EnvGuiServer::DayLength);
	commandToClient (%client, 'EnvGui_SetVar', "DayCycleEnabled", $EnvGuiServer::DayCycleEnabled);
	commandToClient (%client, 'EnvGui_SetVar', "DayCycleIdx", $EnvGuiServer::DayCycleIdx);
	commandToClient (%client, 'EnvGui_SetVar', "SunAzimuth", $EnvGuiServer::SunAzimuth);
	commandToClient (%client, 'EnvGui_SetVar', "SunElevation", $EnvGuiServer::SunElevation);
	commandToClient (%client, 'EnvGui_SetVar', "DirectLightColor", $EnvGuiServer::DirectLightColor);
	commandToClient (%client, 'EnvGui_SetVar', "AmbientLightColor", $EnvGuiServer::AmbientLightColor);
	commandToClient (%client, 'EnvGui_SetVar', "ShadowColor", $EnvGuiServer::ShadowColor);
	commandToClient (%client, 'EnvGui_SetVar', "SunFlareColor", $EnvGuiServer::SunFlareColor);
	commandToClient (%client, 'EnvGui_SetVar', "SunFlareSize", $EnvGuiServer::SunFlareSize);
	commandToClient (%client, 'EnvGui_SetVar', "SunFlareTopIdx", $EnvGuiServer::SunFlareTopIdx);
	commandToClient (%client, 'EnvGui_SetVar', "SunFlareBottomIdx", $EnvGuiServer::SunFlareBottomIdx);
	commandToClient (%client, 'EnvGui_SetVar', "VisibleDistance", $EnvGuiServer::VisibleDistance);
	commandToClient (%client, 'EnvGui_SetVar', "FogDistance", $EnvGuiServer::FogDistance);
	commandToClient (%client, 'EnvGui_SetVar', "FogHeight", $EnvGuiServer::FogHeight);
	commandToClient (%client, 'EnvGui_SetVar', "FogColor", $EnvGuiServer::FogColor);
	commandToClient (%client, 'EnvGui_SetVar', "WaterColor", $EnvGuiServer::WaterColor);
	commandToClient (%client, 'EnvGui_SetVar', "WaterHeight", $EnvGuiServer::WaterHeight);
	commandToClient (%client, 'EnvGui_SetVar', "UnderWaterColor", $EnvGuiServer::UnderWaterColor);
	commandToClient (%client, 'EnvGui_SetVar', "SkyColor", $EnvGuiServer::SkyColor);
	commandToClient (%client, 'EnvGui_SetVar', "WaterScrollX", $EnvGuiServer::WaterScrollX);
	commandToClient (%client, 'EnvGui_SetVar', "WaterScrollY", $EnvGuiServer::WaterScrollY);
	commandToClient (%client, 'EnvGui_SetVar', "GroundColor", $EnvGuiServer::GroundColor);
	commandToClient (%client, 'EnvGui_SetVar', "GroundScrollX", $EnvGuiServer::GroundScrollX);
	commandToClient (%client, 'EnvGui_SetVar', "GroundScrollY", $EnvGuiServer::GroundScrollY);
	commandToClient (%client, 'EnvGui_SetVar', "VignetteMultiply", $EnvGuiServer::VignetteMultiply);
	commandToClient (%client, 'EnvGui_SetVar', "VignetteColor", $EnvGuiServer::VignetteColor);

	commandToClient (%client, 'EnvGui_ListsDone');
}
