function EnvGuiServer::readAdvancedVarsFromSimple ()
{
	$EnvGuiServer::SkyFile = $EnvGuiServer::Sky[$EnvGuiServer::SkyIdx];
	$EnvGuiServer::WaterFile = $EnvGuiServer::Water[$EnvGuiServer::WaterIdx];
	$EnvGuiServer::GroundFile = $EnvGuiServer::Ground[$EnvGuiServer::GroundIdx];

	$EnvGuiServer::SunAzimuth = $Sky::SunAzimuth;
	$EnvGuiServer::SunElevation = $Sky::SunElevation;

	$EnvGuiServer::DirectLightColor = $Sky::DirectLightColor;
	$EnvGuiServer::AmbientLightColor = $Sky::AmbientLightColor;
	$EnvGuiServer::ShadowColor = $Sky::ShadowColor;

	$EnvGuiServer::SunFlareColor = $Sky::SunFlareColor;
	$EnvGuiServer::SunFlareSize = $Sky::SunFlareSize;

	$EnvGuiServer::DayCycleEnabled = $Sky::DayCycleEnabled;
	$EnvGuiServer::DayCycleFile = $Sky::DayCycleFile;
	$EnvGuiServer::DayLength = $Sky::DayLength;

	$EnvGuiServer::VignetteMultiply = $Sky::VignetteMultiply;
	$EnvGuiServer::VignetteColor = $Sky::VignetteColor;

	EnvGuiServer::getIdxFromFilenames();

	$EnvGuiServer::VisibleDistance = $Sky::visibleDistance;
	$EnvGuiServer::FogDistance = $Sky::FogDistance;
	$EnvGuiServer::FogHeight = $Sky::FogHeight;
	$EnvGuiServer::FogColor = $Sky::FogColor;

	$EnvGuiServer::SkyColor = $Sky::SkyColor;

	$EnvGuiServer::WaterColor = $Water::Color;
	$EnvGuiServer::UnderWaterColor = $Water::OverlayColor;
	$EnvGuiServer::WaterScrollX = getWord ($Water::ScrollSpeed, 0);
	$EnvGuiServer::WaterScrollY = getWord ($Water::ScrollSpeed, 1);
	$EnvGuiServer::WaterHeight = $Water::Height;

	$EnvGuiServer::GroundColor = $Ground::Color;
	$EnvGuiServer::GroundScrollX = getWord ($Ground::ScrollSpeed, 0);
	$EnvGuiServer::GroundScrollY = getWord ($Ground::ScrollSpeed, 1);

	$EnvGuiServer::WindVelocity = $Sky::windVelocity;
	$EnvGuiServer::WindEffectPrecipitation = $Sky::windEffectPrecipitation;
}
