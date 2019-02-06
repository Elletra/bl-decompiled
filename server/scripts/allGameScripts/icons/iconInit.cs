function serverCmdIconInit ( %client )
{
	if ( !%client.isAdmin )
	{
		return;
	}


	%camera = %client.Camera;

	%client.setControlObject (%camera);
	%camera.setMode ("Observer");
	%camera.setTransform ("-8.31 1.69 -998.36 0.4788 -0.2068 0.8532 0.9374");

	$EnvGuiServer::WaterIdx = 0;
	$EnvGuiServer::SkyFile = "Add-Ons/Sky_Blank/blank.dml";

	EnvGuiServer::getIdxFromFilenames();
	EnvGuiServer::SetSimpleMode();

	$EnvGuiServer::SunAzimuth = 310;
	$EnvGuiServer::SunElevation = 55;

	$EnvGuiServer::DirectLightColor = "1.0 1.0 1.0";
	$EnvGuiServer::AmbientLightColor = "0.4 0.4 0.4";
	$EnvGuiServer::ShadowColor = "0.4 0.4 0.4";

	$EnvGuiServer::SunFlareColor = "0.0 0.0 0.0 0.0";
	$EnvGuiServer::SunFlareSize = 0;

	$EnvGuiServer::DayCycleEnabled = 0;

	$EnvGuiServer::VignetteMultiply = 0;
	$EnvGuiServer::VignetteColor = "0 0 0 0";

	$EnvGuiServer::VisibleDistance = 600;
	$EnvGuiServer::FogDistance = 100;
	$EnvGuiServer::FogColor = "1.0 0.0 1.0";
	$EnvGuiServer::SkyColor = "1 0 1 1";

	$EnvGuiServer::SimpleMode = 0;

	EnvGuiServer::SetAdvancedMode();
}
