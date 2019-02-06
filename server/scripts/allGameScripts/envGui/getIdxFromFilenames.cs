function EnvGuiServer::getIdxFromFilenames ()
{
	for ( %i = 0;  %i < $EnvGuiServer::SkyCount;  %i++ )
	{
		if ( $EnvGuiServer::Sky[%i] $= $EnvGuiServer::SkyFile )
		{
			$EnvGuiServer::SkyIdx = %i;
			break;
		}
	}

	for ( %i = 0;  %i < $EnvGuiServer::WaterCount;  %i++ )
	{
		if ( $EnvGuiServer::Water[%i] $= $EnvGuiServer::WaterFile )
		{
			$EnvGuiServer::WaterIdx = %i;
			break;
		}
	}

	for ( %i = 0;  %i < $EnvGuiServer::GroundCount;  %i++ )
	{
		if ( $EnvGuiServer::Ground[%i] $= $EnvGuiServer::GroundFile )
		{
			$EnvGuiServer::GroundIdx = %i;
			break;
		}
	}

	if ( !$EnvGuiServer::SimpleMode )
	{
		for ( %i = 0;  %i < $EnvGuiServer::SunFlareCount;  %i++ )
		{
			if ( $EnvGuiServer::SunFlare[%i] $= $Sky::SunFlareTopTexture )
			{
				$EnvGuiServer::SunFlareTopIdx = %i;
				break;
			}
		}

		for ( %i = 0;  %i < $EnvGuiServer::SunFlareCount;  %i++ )
		{
			if ( $EnvGuiServer::SunFlare[%i] $= $Sky::SunFlareBottomTexture )
			{
				$EnvGuiServer::SunFlareBottomIdx = %i;
				break;
			}
		}

		for ( %i = 0;  %i < $EnvGuiServer::DayCycleCount;  %i++ )
		{
			if ( $EnvGuiServer::DayCycle[%i] $= $Sky::DayCycleFile )
			{
				$EnvGuiServer::DayCycleIdx = %i;
				break;
			}
		}
	}
}
