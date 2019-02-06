function serverCmdEnvGui_SetVar ( %client, %varName, %value )
{
	if ( !%client.isAdmin )
	{
		return;
	}


	if ( %varName $= "SimpleMode" )
	{
		if ( $EnvGuiServer::SimpleMode !$= %value )
		{
			$EnvGuiServer::SimpleMode = mClamp (%value, 0, 1);

			if ( $EnvGuiServer::SimpleMode )
			{
				EnvGuiServer::SetSimpleMode();
			}
			else if ( !$EnvGuiServer::HasSetAdvancedOnce )
			{
				EnvGuiServer::readAdvancedVarsFromSimple();
				EnvGuiServer::SetAdvancedMode();

				serverCmdEnvGui_RequestCurrentVars (%client);
			}
			else
			{
				EnvGuiServer::SetAdvancedMode();
			}
		}
	}
	else if ( %varName $= "SkyIdx" )
	{
		if ( $EnvGuiServer::SkyIdx !$= %value )
		{
			$EnvGuiServer::SkyIdx = mClamp (%value, 0, $EnvGuiServer::SkyCount);
			%filename = $EnvGuiServer::Sky[$EnvGuiServer::SkyIdx];

			setSkyBox (%filename);
		}
	}
	else if ( %varName $= "WaterIdx" )
	{
		if ( $EnvGuiServer::WaterIdx !$= %value )
		{
			$EnvGuiServer::WaterIdx = mClamp (%value, 0, $EnvGuiServer::WaterCount);
			%filename = $EnvGuiServer::Water[$EnvGuiServer::WaterIdx];

			setWater (%filename);
		}
	}
	else if ( %varName $= "GroundIdx" )
	{
		if ( $EnvGuiServer::GroundIdx !$= %value )
		{
			$EnvGuiServer::GroundIdx = mClamp (%value, 0, $EnvGuiServer::GroundCount);
			%filename = $EnvGuiServer::Ground[$EnvGuiServer::GroundIdx];

			setGround (%filename);
		}
	}
	else if ( %varName $= "SunFlareTopIdx" )
	{
		if ( $EnvGuiServer::SunFlareTopIdx !$= %value )
		{
			$EnvGuiServer::SunFlareTopIdx = mClamp (%value, 0, $EnvGuiServer::SunFlareCount);

			%top = $EnvGuiServer::SunFlare[$EnvGuiServer::SunFlareTopIdx];
			%bottom = $EnvGuiServer::SunFlare[$EnvGuiServer::SunFlareBottomIdx];

			SunLight.setFlareBitmaps (%top, %bottom);
		}
	}
	else if ( %varName $= "SunFlareBottomIdx" )
	{
		if ( $EnvGuiServer::SunFlareBottomIdx !$= %value )
		{
			$EnvGuiServer::SunFlareBottomIdx = mClamp (%value, 0, $EnvGuiServer::SunFlareCount);

			%top = $EnvGuiServer::SunFlare[$EnvGuiServer::SunFlareTopIdx];
			%bottom = $EnvGuiServer::SunFlare[$EnvGuiServer::SunFlareBottomIdx];

			SunLight.setFlareBitmaps (%top, %bottom);
		}
	}
	else if ( %varName $= "DayOffset" )
	{
		if ( $EnvGuiServer::DayOffset !$= %value )
		{
			$EnvGuiServer::DayOffset = mClampF (%value, 0, 1);
			DayCycle.setDayOffset($EnvGuiServer::DayOffset);
		}
	}
	else if ( %varName $= "DayLength" )
	{
		if ( $EnvGuiServer::DayLength !$= %value )
		{
			$EnvGuiServer::DayLength = mClamp (%value, 0, 86400);
			DayCycle.setDayLength ($EnvGuiServer::DayLength);
		}
	}
	else if ( %varName $= "DayCycleEnabled" )
	{
		if ( $EnvGuiServer::DayCycleEnabled !$= %value )
		{
			$EnvGuiServer::DayCycleEnabled = mClamp (%value, 0, 1);
			DayCycle.setEnabled ($EnvGuiServer::DayCycleEnabled);
		}
	}
	else if ( %varName $= "DayCycleIdx" )
	{
		if ( $EnvGuiServer::DayCycleIdx !$= %value )
		{
			$EnvGuiServer::DayCycleIdx = mClamp (%value, 0, $EnvGuiServer::DayCycleCount);
			echo ( "server setting daycycle to " @  $EnvGuiServer::DayCycle[$EnvGuiServer::DayCycleIdx] );
			loadDayCycle ( $EnvGuiServer::DayCycle[$EnvGuiServer::DayCycleIdx] );
		}
	}
	else if ( %varName $= "SunAzimuth" )
	{
		if ( $EnvGuiServer::SunAzimuth !$= %value )
		{
			$EnvGuiServer::SunAzimuth = mClampF (%value, 0, 360);
			Sun.azimuth = $EnvGuiServer::SunAzimuth;
			Sun.sendUpdate();
		}
	}
	else if ( %varName $= "SunElevation" )
	{
		if ( $EnvGuiServer::SunElevation !$= %value )
		{
			$EnvGuiServer::SunElevation = mClampF (%value, -10.0, 190);
			Sun.elevation = $EnvGuiServer::SunElevation;
			Sun.sendUpdate();
		}
	}
	else if ( %varName $= "DirectLightColor" )
	{
		if ( $EnvGuiServer::DirectLightColor !$= %value )
		{
			$EnvGuiServer::DirectLightColor = getColorF (%value);
			Sun.color = $EnvGuiServer::DirectLightColor;
			Sun.sendUpdate();
		}
	}
	else if ( %varName $= "AmbientLightColor" )
	{
		if ( $EnvGuiServer::AmbientLightColor !$= %value )
		{
			$EnvGuiServer::AmbientLightColor = getColorF (%value);
			Sun.ambient = $EnvGuiServer::AmbientLightColor;
			Sun.sendUpdate();
		}
	}
	else if ( %varName $= "ShadowColor" )
	{
		if ( $EnvGuiServer::ShadowColor !$= %value )
		{
			$EnvGuiServer::ShadowColor = getColorF (%value);
			Sun.shadowColor = $EnvGuiServer::ShadowColor;
			Sun.sendUpdate();
		}
	}
	else if ( %varName $= "SunFlareColor" )
	{
		if ( $EnvGuiServer::SunFlareColor !$= %value )
		{
			$EnvGuiServer::SunFlareColor = getColorF (%value);
			SunLight.color = $EnvGuiServer::SunFlareColor;
			SunLight.sendUpdate();
		}
	}
	else if ( %varName $= "SunFlareSize" )
	{
		if ( $EnvGuiServer::SunFlareSize !$= %value )
		{
			$EnvGuiServer::SunFlareSize = mClampF (%value, 0, 10);
			SunLight.FlareSize = $EnvGuiServer::SunFlareSize;
			SunLight.sendUpdate();
		}
	}
	else if ( %varName $= "SunFlareIdx" )
	{
		if ( $EnvGuiServer::SunFlareIdx !$= %value )
		{
			$EnvGuiServer::SunFlareIdx = mClamp (%value, 0, $EnvGuiServer::SunFlareCount);
		}
	}
	else if ( %varName $= "VisibleDistance" )
	{
		if ( $EnvGuiServer::VisibleDistance !$= %value )
		{
			$EnvGuiServer::VisibleDistance = mClampF (%value, 0, 1000);
			Sky.visibleDistance = $EnvGuiServer::VisibleDistance;
			Sky.sendUpdate();
		}
	}
	else if ( %varName $= "FogDistance" )
	{
		if ( $EnvGuiServer::FogDistance !$= %value )
		{
			$EnvGuiServer::FogDistance = mClampF (%value, 0, 1000);
			Sky.fogDistance = $EnvGuiServer::FogDistance;
			Sky.sendUpdate();
		}
	}
	else if (%varName $= "FogHeight")
	{
		if ($EnvGuiServer::FogHeight !$= %value)
		{
			$EnvGuiServer::FogHeight = mClampF(%value, 0, 1000);
		}
	}
	else if ( %varName $= "FogColor" )
	{
		if ( $EnvGuiServer::FogColor !$= %value )
		{
			$EnvGuiServer::FogColor = getColorF (%value);
			Sky.fogColor = $EnvGuiServer::FogColor;
			Sky.sendUpdate();
		}
	}
	else if ( %varName $= "WaterColor" )
	{
		if ( $EnvGuiServer::WaterColor !$= %value )
		{
			$EnvGuiServer::WaterColor = getColorF (%value);

			if ( isObject(WaterPlane) )
			{
				WaterPlane.color = getColorI ($EnvGuiServer::WaterColor);
				WaterPlane.blend = getWord (WaterPlane.color, 3) < 255;

				WaterPlane.sendUpdate();
				updateWaterFog();
			}
		}
	}
	else if (%varName $= "WaterHeight")
	{
		if ( $EnvGuiServer::WaterHeight !$= %value )
		{
			$EnvGuiServer::WaterHeight = mClampF (%value, 0, 100);

			if ( isObject(WaterPlane) )
			{
				%pos = getWords (groundPlane.getTransform(), 0, 2);
				%pos = VectorAdd (%pos, "0 0 " @  $EnvGuiServer::WaterHeight);

				WaterPlane.setTransform (%pos  @ " 0 0 1 0");
				WaterPlane.sendUpdate();
				updateWaterFog();
			}

			if ( isObject(WaterZone) )
			{
				%pos = getWords (WaterPlane.getTransform(), 0, 2);
				%pos = VectorSub (%pos, "0 0 100");
				%pos = VectorAdd (%pos, "0 0 0.5");
				%pos = VectorSub (%pos, "500000 -500000 0");

				WaterZone.setTransform (%pos  @ " 0 0 1 0");
			}
		}
	}
	else if ( %varName $= "UnderWaterColor" )
	{
		if ( $EnvGuiServer::UnderWaterColor !$= %value )
		{
			$EnvGuiServer::UnderWaterColor = getColorF (%value);

			if ( isObject(WaterZone) )
			{
				WaterZone.setWaterColor ($EnvGuiServer::UnderWaterColor);
			}
		}
	}
	else if ( %varName $= "SkyColor" )
	{
		if ( $EnvGuiServer::SkyColor !$= %value )
		{
			$EnvGuiServer::SkyColor = getColorF (%value);
			Sky.skyColor = getColorF ($EnvGuiServer::SkyColor);
			Sky.sendUpdate();
		}
	}
	else if ( %varName $= "WaterScrollX" )
	{
		if ( $EnvGuiServer::WaterScrollX !$= %value )
		{
			$EnvGuiServer::WaterScrollX = %value;
			$EnvGuiServer::WaterScrollX = mClampF ($EnvGuiServer::WaterScrollX, -10, 10);
			$EnvGuiServer::WaterScrollY = mClampF ($EnvGuiServer::WaterScrollY, -10, 10);

			if ( isObject(WaterPlane) )
			{
				WaterPlane.scrollSpeed = $EnvGuiServer::WaterScrollX SPC $EnvGuiServer::WaterScrollY;
				WaterPlane.sendUpdate();
			}

			if ( isObject(WaterZone) )
			{
				WaterZone.appliedForce = $EnvGuiServer::WaterScrollX * 414 SPC $EnvGuiServer::WaterScrollY * -414 SPC 0;
				WaterZone.sendUpdate();
			}
		}
	}
	else if ( %varName $= "WaterScrollY" )
	{
		if ( $EnvGuiServer::WaterScrollX !$= %value )
		{
			$EnvGuiServer::WaterScrollY = %value;
			$EnvGuiServer::WaterScrollX = mClampF ($EnvGuiServer::WaterScrollX, -10, 10);
			$EnvGuiServer::WaterScrollY = mClampF ($EnvGuiServer::WaterScrollY, -10, 10);

			if ( isObject(WaterPlane) )
			{
				WaterPlane.scrollSpeed = $EnvGuiServer::WaterScrollX SPC $EnvGuiServer::WaterScrollY;
				WaterPlane.sendUpdate();
			}

			if ( isObject(WaterZone) )
			{
				WaterZone.appliedForce = $EnvGuiServer::WaterScrollX * 414 SPC $EnvGuiServer::WaterScrollY * -414 SPC 0;
				WaterZone.sendUpdate();
			}
		}
	}
	else if ( %varName $= "GroundColor" )
	{
		if ( $EnvGuiServer::GroundColor !$= %value )
		{
			$EnvGuiServer::GroundColor = getColorF (%value);

			if ( isObject(groundPlane) )
			{
				groundPlane.color = getColorI ($EnvGuiServer::GroundColor);
				groundPlane.blend = getWord (groundPlane.color, 3) < 255;
				groundPlane.sendUpdate();

				Sky.renderBottomTexture = getWord (groundPlane.color, 3) <= 0;
				Sky.noRenderBans = Sky.renderBottomTexture;
				Sky.sendUpdate();
			}
		}
	}
	else if ( %varName $= "GroundScrollX" )
	{
		if ( $EnvGuiServer::GroundScrollX !$= %value )
		{
			$EnvGuiServer::GroundScrollX = %value;
			$EnvGuiServer::GroundScrollX = mClampF ($EnvGuiServer::GroundScrollX, -10, 10);
			$EnvGuiServer::GroundScrollY = mClampF ($EnvGuiServer::GroundScrollY, -10, 10);

			groundPlane.scrollSpeed = $EnvGuiServer::GroundScrollX SPC $EnvGuiServer::GroundScrollY;
			groundPlane.sendUpdate();
		}
	}
	else if ( %varName $= "GroundScrollY" )
	{
		if ( $EnvGuiServer::GroundScrollY !$= %value )
		{
			$EnvGuiServer::GroundScrollY = %value;
			$EnvGuiServer::GroundScrollX = mClampF ($EnvGuiServer::GroundScrollX, -10, 10);
			$EnvGuiServer::GroundScrollY = mClampF ($EnvGuiServer::GroundScrollY, -10, 10);

			groundPlane.scrollSpeed = $EnvGuiServer::GroundScrollX SPC $EnvGuiServer::GroundScrollY;
			groundPlane.sendUpdate();
		}
	}
	else if ( %varName $= "VignetteMultiply" )
	{
		if ( $EnvGuiServer::VignetteMultiply !$= %value )
		{
			$EnvGuiServer::VignetteMultiply = mClamp (%value, 0, 1);
			EnvGuiServer::SendVignetteAll();
		}
	}
	else if ( %varName $= "VignetteColor" )
	{
		if ( $EnvGuiServer::VignetteColor !$= %value )
		{
			$EnvGuiServer::VignetteColor = getColorF (%value);
			EnvGuiServer::SendVignetteAll();
		}
	}
}
