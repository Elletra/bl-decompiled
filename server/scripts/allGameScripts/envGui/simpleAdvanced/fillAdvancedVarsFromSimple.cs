function EnvGuiServer::fillAdvancedVarsFromSimple ()
{
	$EnvGuiServer::SkyFile = $EnvGuiServer::Sky[$EnvGuiServer::SkyIdx];
	$EnvGuiServer::WaterFile = $EnvGuiServer::Water[$EnvGuiServer::WaterIdx];
	$EnvGuiServer::GroundFile = $EnvGuiServer::Ground[$EnvGuiServer::GroundIdx];

	if ( $EnvGuiServer::SunAzimuth $= "" )
	{
		$EnvGuiServer::SunAzimuth = $Sky::SunAzimuth;
	}

	if ( $EnvGuiServer::SunElevation $= "" )
	{
		$EnvGuiServer::SunElevation = $Sky::SunElevation;
	}

	if ( $EnvGuiServer::DirectLightColor $= "" )
	{
		$EnvGuiServer::DirectLightColor = $Sky::DirectLightColor;
	}

	if ( $EnvGuiServer::AmbientLightColor $= "" )
	{
		$EnvGuiServer::AmbientLightColor = $Sky::AmbientLightColor;
	}

	if ( $EnvGuiServer::ShadowColor $= "" )
	{
		$EnvGuiServer::ShadowColor = $Sky::ShadowColor;
	}

	if ( $EnvGuiServer::SunFlareColor $= "" )
	{
		$EnvGuiServer::SunFlareColor = $Sky::SunFlareColor;
	}

	if ( $EnvGuiServer::SunFlareSize $= "" )
	{
		$EnvGuiServer::SunFlareSize = $Sky::SunFlareSize;
	}

	if ( $EnvGuiServer::DayCycleEnabled $= "" )
	{
		$EnvGuiServer::DayCycleEnabled = $Sky::DayCycleEnabled;
	}

	if ( $EnvGuiServer::DayCycleFile $= "" )
	{
		$EnvGuiServer::DayCycleFile = $Sky::DayCycleFile;
	}

	if ( $EnvGuiServer::DayLength $= "" )
	{
		$EnvGuiServer::DayLength = $Sky::DayLength;
	}

	if ( $EnvGuiServer::VignetteMultiply $= "" )
	{
		$EnvGuiServer::VignetteMultiply = $Sky::VignetteMultiply;
	}

	if ( $EnvGuiServer::VignetteColor $= "" )
	{
		$EnvGuiServer::VignetteColor = $Sky::VignetteColor;
	}

	EnvGuiServer::getIdxFromFilenames();

	if ( $EnvGuiServer::VisibleDistance $= "" )
	{
		$EnvGuiServer::VisibleDistance = $Sky::visibleDistance;
	}

	if ( $EnvGuiServer::FogDistance $= "" )
	{
		$EnvGuiServer::FogDistance = $Sky::FogDistance;
	}

	if ( $EnvGuiServer::FogHeight $= "" )
	{
		$EnvGuiServer::FogHeight = $Sky::FogHeight;
	}

	if ( $EnvGuiServer::FogColor $= "" )
	{
		$EnvGuiServer::FogColor = $Sky::FogColor;
	}

	if ( $EnvGuiServer::SkyColor $= "" )
	{
		$EnvGuiServer::SkyColor = $Sky::SkyColor;
	}

	if ( $EnvGuiServer::WaterColor $= "" )
	{
		$EnvGuiServer::WaterColor = $Water::Color;
	}

	if ( $EnvGuiServer::UnderWaterColor $= "" )
	{
		$EnvGuiServer::UnderWaterColor = $Water::OverlayColor;
	}

	if ( $EnvGuiServer::WaterScrollX $= "" )
	{
		$EnvGuiServer::WaterScrollX = getWord ($Water::ScrollSpeed, 0);
	}

	if ( $EnvGuiServer::WaterScrollY $= "" )
	{
		$EnvGuiServer::WaterScrollY = getWord ($Water::ScrollSpeed, 1);
	}

	if ( $EnvGuiServer::WaterHeight $= "" )
	{
		$EnvGuiServer::WaterHeight = $Water::Height;
	}

	if ( $EnvGuiServer::GroundColor $= "" )
	{
		$EnvGuiServer::GroundColor = $Ground::Color;
	}

	if ( $EnvGuiServer::GroundScrollX $= "" )
	{
		$EnvGuiServer::GroundScrollX = getWord ($Ground::ScrollSpeed, 0);
	}

	if ( $EnvGuiServer::GroundScrollY $= "" )
	{
		$EnvGuiServer::GroundScrollY = getWord ($Ground::ScrollSpeed, 1);
	}

	if ( $EnvGuiServer::WindVelocity $= "" )
	{
		$EnvGuiServer::WindVelocity = $Sky::windVelocity;
	}

	if ( $EnvGuiServer::WindEffectPrecipitation $= "" )
	{
		$EnvGuiServer::WindEffectPrecipitation = $Sky::windEffectPrecipitation;
	}
}
