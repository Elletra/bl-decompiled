function setSkyBox ( %filename )
{
	if ( !isFile(%filename) )
	{
		error ("ERROR: setSkyBox(" @  %filename  @ ") - file does not exist");
		return;
	}

	if ( !validateEnvironmentObjects() )
	{
		return;
	}

	$Sky::visibleDistance = 800;
	$Sky::FogDistance = 400;
	$Sky::FogColor = "1.0 1.0 1.0 1.0";
	$Sky::RenderBans = 1;
	$Sky::SkyColor = "1.0 1.0 1.0 1.0";

	$Sky::CloudHeight0 = 0.0;
	$Sky::CloudHeight1 = 0.0;
	$Sky::CloudHeight2 = 0.0;

	$Sky::CloudSpeed0 = 0.0;
	$Sky::CloudSpeed1 = 0.0;
	$Sky::CloudSpeed2 = 0.0;

	$Sky::SunElevation = 35;
	$Sky::SunAzimuth = 250;
	$Sky::SunFlareColor = "1.0 1.0 1.0 1.0";
	$Sky::SunFlareSize = 1.0;
	$Sky::SunFlareTopTexture = "base/lighting/corona.png";
	$Sky::SunFlareBottomTexture = "base/lighting/corona.png";

	$Sky::DirectLightColor = "1.0 1.0 1.0 1.0";
	$Sky::AmbientLightColor = "0.5 0.5 0.5 1.0";
	$Sky::ShadowColor = "0.3 0.3 0.5 1.0";

	$Sky::DayCycleFile = "Add-Ons/DayCycle_Default/default.daycycle";
	$Sky::DayCycleEnabled = 0;
	$Sky::DayLength = 300;

	$Sky::VignetteMultiply = 0;
	$Sky::VignetteColor = "0 0 0 0.2";

	$Sky::windVelocity = "0 0 0";
	$Sky::windEffectPrecipitation = 0;

	$Rain::DropTexture = "";
	$Rain::SplashTexture = "";
	$Rain::DropSize = 0.5;
	$Rain::SplashSize = 1.0;
	$Rain::SplashMS = 100;
	$Rain::UseTrueBillboards = 0;

	%atmosphereFile = filePath (%filename)  @ "/" @  fileBase (%filename)  @ ".atmosphere";
	parseEnvironmentFile (%atmosphereFile);

	Sky.materialList = %filename;
	Sky.visibleDistance = $Sky::visibleDistance;
	Sky.fogDistance = $Sky::FogDistance;
	Sky.fogColor = $Sky::FogColor;
	Sky.noRenderBans = !$Sky::RenderBans;
	Sky.skyColor = $Sky::SkyColor;

	Sky.cloudHeight[0] = $Sky::CloudHeight0;
	Sky.cloudHeight[1] = $Sky::CloudHeight1;
	Sky.cloudHeight[2] = $Sky::CloudHeight2;

	Sky.cloudSpeed[0] = $Sky::CloudSpeed0;
	Sky.cloudSpeed[1] = $Sky::CloudSpeed1;
	Sky.cloudSpeed[2] = $Sky::CloudSpeed2;

	Sky.windVelocity = $Sky::windVelocity;
	Sky.windEffectPrecipitation = $Sky::windEffectPrecipitation;

	Sky.sendUpdate();

	EnvGuiServer::SendVignetteAll();

	Sun.azimuth = $Sky::SunAzimuth;
	Sun.elevation = $Sky::SunElevation;
	Sun.color = $Sky::DirectLightColor;
	Sun.ambient = $Sky::AmbientLightColor;
	Sun.shadowColor = $Sky::ShadowColor;

	Sun.sendUpdate();

	SunLight.setFlareBitmaps ($Sky::SunFlareTopTexture, $Sky::SunFlareBottomTexture);

	SunLight.FlareSize = $Sky::SunFlareSize;
	SunLight.color = $Sky::SunFlareColor;

	if ( isObject(Rain) )
	{
		Rain.delete();
	}

	if ( isFile($Rain::DropTexture) )
	{
		new Precipitation (Rain)
		{
			dataBlock = DataBlockGroup.getObject (0);

			dropTexture = $Rain::DropTexture;
			splashTexture = $Rain::SplashTexture;

			dropSize = $Rain::DropSize;
			splashSize = $Rain::SplashSize;
			splashMS = $Rain::SplashMS;

			useTrueBillboards = $Rain::UseTrueBillboards;

			minSpeed = $Rain::minSpeed;
			maxSpeed = $Rain::maxSpeed;

			minMass = $Rain::minMass;
			maxMass = $Rain::maxMass;

			maxTurbulence = $Rain::maxTurbulence;
			turbulenceSpeed = $Rain::turbulenceSpeed;
			rotateWithCamVel = $Rain::rotateWithCamVel;
			useTurbulence = $Rain::useTurbulence;

			numDrops = $Rain::numDrops;

			boxWidth = $Rain::boxWidth;
			boxHeight = $Rain::boxHeight;

			doCollision = $Rain::doCollision;
		};
		MissionGroup.add (Rain);
	}

	loadDayCycle ($Sky::DayCycleFile);

	DayCycle.setDayLength ($Sky::DayLength);
	DayCycle.setEnabled ($Sky::DayCycleEnabled);
}
