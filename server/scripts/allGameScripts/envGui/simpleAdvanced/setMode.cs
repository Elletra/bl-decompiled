function EnvGuiServer::setSimpleMode ()
{
	%filename = $EnvGuiServer::Sky[$EnvGuiServer::SkyIdx];
	setSkyBox (%filename);

	%filename = $EnvGuiServer::Water[$EnvGuiServer::WaterIdx];
	setWater (%filename);

	%filename = $EnvGuiServer::Ground[$EnvGuiServer::GroundIdx];
	setGround (%filename);

	DayCycle.setDayOffset (0);
	DayCycle.setDayLength (300);
}

function EnvGuiServer::setAdvancedMode ()
{
	$EnvGuiServer::HasSetAdvancedOnce = true;

	Sun.azimuth = $EnvGuiServer::SunAzimuth;
	Sun.elevation = $EnvGuiServer::SunElevation;
	Sun.color = $EnvGuiServer::DirectLightColor;
	Sun.ambient = $EnvGuiServer::AmbientLightColor;
	Sun.shadowColor = $EnvGuiServer::ShadowColor;

	Sun.sendUpdate();

	EnvGuiServer::SendVignetteAll();

	SunLight.FlareSize = $EnvGuiServer::SunFlareSize;
	SunLight.color = $EnvGuiServer::SunFlareColor;
	SunLight.setFlareBitmaps ( $EnvGuiServer::SunFlare[$EnvGuiServer::SunFlareTopIdx], 
		$EnvGuiServer::SunFlare[$EnvGuiServer::SunFlareBottomIdx] );

	SunLight.sendUpdate();

	Sky.visibleDistance = $EnvGuiServer::VisibleDistance;
	Sky.fogDistance = $EnvGuiServer::FogDistance;
	Sky.fogColor = getColorF ($EnvGuiServer::FogColor);
	Sky.skyColor = getColorF ($EnvGuiServer::SkyColor);

	Sky.windVelocity = $EnvGuiServer::WindVelocity;
	Sky.windEffectPrecipitation = $EnvGuiServer::WindEffectPrecipitation;

	Sky.sendUpdate();

	if (isObject(WaterPlane))
	{
		%pos = getWords (groundPlane.getTransform(), 0, 2);
		%pos = VectorAdd (%pos, "0 0 " @  $EnvGuiServer::WaterHeight);

		WaterPlane.setTransform (%pos  @ " 0 0 1 0");

		WaterPlane.scrollSpeed = $EnvGuiServer::WaterScrollX SPC $EnvGuiServer::WaterScrollY;
		WaterPlane.color = getColorI ($EnvGuiServer::WaterColor);
		WaterPlane.blend = getWord (WaterPlane.color, 3) < 255;

		WaterPlane.sendUpdate();
		updateWaterFog();

		if ( isObject(WaterZone) )
		{
			%pos = getWords (WaterPlane.getTransform(), 0, 2);
			%pos = VectorSub (%pos, "0 0 100");
			%pos = VectorAdd (%pos, "0 0 0.5");
			%pos = VectorSub (%pos, "500000 -500000 0");

			WaterZone.setTransform (%pos  @ " 0 0 1 0");

			WaterZone.appliedForce = $EnvGuiServer::WaterScrollX * 414 SPC $EnvGuiServer::WaterScrollY * -414 SPC 0;
			WaterZone.setWaterColor ( getColorF($EnvGuiServer::UnderWaterColor) );
		}
	}

	groundPlane.color = getColorI ($EnvGuiServer::GroundColor);
	groundPlane.blend = getWord (groundPlane.color, 3) < 255;
	groundPlane.scrollSpeed = $EnvGuiServer::GroundScrollX SPC $EnvGuiServer::GroundScrollY;

	groundPlane.sendUpdate();

	Sky.renderBottomTexture = getWord (groundPlane.color, 3) <= 0;
	Sky.noRenderBans = Sky.renderBottomTexture;

	Sky.sendUpdate();

	loadDayCycle ( $EnvGuiServer::DayCycle[$EnvGuiServer::DayCycleIdx] );
	DayCycle.setEnabled ($EnvGuiServer::DayCycleEnabled);
}
