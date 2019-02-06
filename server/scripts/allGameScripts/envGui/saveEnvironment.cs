function saveEnvironment ( %filename )
{
	%file = new FileObject();
	%file.openForWrite (%filename);

	%file.writeLine ( "$EnvGuiServer::SimpleMode" @  $EnvGuiServer::SimpleMode );
	%file.writeLine ( "$EnvGuiServer::SkyFile" @  $EnvGuiServer::Sky[$EnvGuiServer::SkyIdx] );
	%file.writeLine ( "$EnvGuiServer::WaterFile" @  $EnvGuiServer::Water[$EnvGuiServer::WaterIdx] );
	%file.writeLine ( "$EnvGuiServer::GroundFile" @  $EnvGuiServer::Ground[$EnvGuiServer::GroundIdx] );

	if ( !$EnvGuiServer::SimpleMode )
	{
		%file.writeLine ("$EnvGuiServer::SunFlareTopTexture" @  $EnvGuiServer::SunFlare[$EnvGuiServer::SunFlareTopIdx] );
		%file.writeLine ("$EnvGuiServer::SunFlareBottomTexture" @  $EnvGuiServer::SunFlare[$EnvGuiServer::SunFlareBottomIdx] );
		%file.writeLine ("$EnvGuiServer::DayOffset" @  $EnvGuiServer::DayOffset);
		%file.writeLine ("$EnvGuiServer::DayLength" @  $EnvGuiServer::DayLength);
		%file.writeLine ("$EnvGuiServer::DayCycleEnabled" @  $EnvGuiServer::DayCycleEnabled);
		%file.writeLine ( "$EnvGuiServer::DayCycle" @  $EnvGuiServer::DayCycle[$EnvGuiServer::DayCycleIdx] );
		%file.writeLine ("$EnvGuiServer::SunAzimuth" @  $EnvGuiServer::SunAzimuth);
		%file.writeLine ("$EnvGuiServer::SunElevation" @  $EnvGuiServer::SunElevation);
		%file.writeLine ("$EnvGuiServer::DirectLightColor" @  $EnvGuiServer::DirectLightColor);
		%file.writeLine ("$EnvGuiServer::AmbientLightColor" @  $EnvGuiServer::AmbientLightColor);
		%file.writeLine ("$EnvGuiServer::ShadowColor" @  $EnvGuiServer::ShadowColor);
		%file.writeLine ("$EnvGuiServer::SunFlareColor" @  $EnvGuiServer::SunFlareColor);
		%file.writeLine ("$EnvGuiServer::SunFlareSize" @  $EnvGuiServer::SunFlareSize);
		%file.writeLine ("$EnvGuiServer::VisibleDistance" @  $EnvGuiServer::VisibleDistance);
		%file.writeLine ("$EnvGuiServer::FogDistance" @  $EnvGuiServer::FogDistance);
		%file.writeLine ("$EnvGuiServer::FogHeight" @  $EnvGuiServer::FogHeight);
		%file.writeLine ("$EnvGuiServer::FogColor" @  $EnvGuiServer::FogColor);
		%file.writeLine ("$EnvGuiServer::WaterColor" @  $EnvGuiServer::WaterColor);
		%file.writeLine ("$EnvGuiServer::WaterHeight" @  $EnvGuiServer::WaterHeight);
		%file.writeLine ("$EnvGuiServer::UnderWaterColor" @  $EnvGuiServer::UnderWaterColor);
		%file.writeLine ("$EnvGuiServer::SkyColor" @  $EnvGuiServer::SkyColor);
		%file.writeLine ("$EnvGuiServer::WaterScrollX" @  $EnvGuiServer::WaterScrollX);
		%file.writeLine ("$EnvGuiServer::WaterScrollY" @  $EnvGuiServer::WaterScrollY);
		%file.writeLine ("$EnvGuiServer::GroundColor" @  $EnvGuiServer::GroundColor);
		%file.writeLine ("$EnvGuiServer::GroundScrollX" @  $EnvGuiServer::GroundScrollX);
		%file.writeLine ("$EnvGuiServer::GroundScrollY" @  $EnvGuiServer::GroundScrollY);
		%file.writeLine ("$EnvGuiServer::VignetteMultiply" @  $EnvGuiServer::VignetteMultiply);
		%file.writeLine ("$EnvGuiServer::VignetteColor" @  $EnvGuiServer::VignetteColor);
	}

	%file.close();
	%file.delete();
}
