function parseEnvironmentFile ( %filename )
{
	if ( !isFile(%filename) )
	{
		echo (%filename  @ "atmosphere not found");
		return;
	}


	%file = new FileObject();
	%file.openForRead (%filename);

	while ( !%file.isEOF() )
	{
		%line = %file.readLine();
		%label = getWord (%line, 0);
		%value = getWords (%line, 1, 999);

		if ( %label !$= ""  &&  getSubStr(%label, 0, 2) !$= "//" )
		{
			// Skybox

			if ( %label $= "$Sky::visibleDistance" )
			{
				$Sky::visibleDistance = mClampF (%value, 0, 1000);
			}
			else if ( %label $= "$Sky::fogDistance" )
			{
				$Sky::FogDistance = mClampF (%value, 0, 1000);
			}
			else if ( %label $= "$Sky::fogColor" )
			{
				$Sky::FogColor = getColorF(%value);
			}
			else if ( %label $= "$Sky::renderBans" )
			{
				$Sky::RenderBans = mClamp (%value, 0, 1);
			}
			else if ( %label $= "$Sky::cloudHeight0" )
			{
				$Sky::CloudHeight0 = mClampF (%value, 0, 100);
			}
			else if ( %label $= "$Sky::cloudHeight1" )
			{
				$Sky::CloudHeight1 = mClampF (%value, 0, 100);
			}
			else if ( %label $= "$Sky::cloudHeight2" )
			{
				$Sky::CloudHeight2 = mClampF (%value, 0, 100);
			}
			else if ( %label $= "$Sky::cloudSpeed0" )
			{
				$Sky::CloudSpeed0 = mClampF (%value, 0, 10);
			}
			else if ( %label $= "$Sky::cloudSpeed1" )
			{
				$Sky::CloudSpeed1 = mClampF (%value, 0, 10);
			}
			else if ( %label $= "$Sky::cloudSpeed2" )
			{
				$Sky::CloudSpeed2 = mClampF (%value, 0, 10);
			}
			else if ( %label $= "$Sky::sunElevation" )
			{
				$Sky::SunElevation = mClampF (%value, -360, 360);
			}
			else if ( %label $= "$Sky::sunAzimuth" )
			{
				$Sky::SunAzimuth = mClampF (%value, -360, 360);
			}
			else if ( %label $= "$Sky::sunFlareColor" )
			{
				$Sky::SunFlareColor = getColorF(%value);
			}
			else if ( %label $= "$Sky::sunFlareSize" )
			{
				$Sky::SunFlareSize = mClampF (%value, 0, 10);
			}
			else if ( %label $= "$Sky::sunFlareTopTexture" )
			{
				if ( isFile(%value)  ||  isFile(%value  @ ".jpg" )  ||  isFile(%value  @ ".png") )
				{
					$Sky::SunFlareTopTexture = %value;
				}
			}
			else if ( %label $= "$Sky::sunFlareBottomTexture" )
			{
				if ( isFile(%value)  ||  isFile(%value  @ ".jpg")  ||  isFile(%value  @ ".png") )
				{
					$Sky::SunFlareBottomTexture = %value;
				}
			}
			else if ( %label $= "$Sky::directLightColor" )
			{
				$Sky::DirectLightColor = getColorF(%value);
			}
			else if ( %label $= "$Sky::ambientLightColor" )
			{
				$Sky::AmbientLightColor = getColorF(%value);
			}
			else if ( %label $= "$Sky::shadowColor" )
			{
				$Sky::ShadowColor = getColorF(%value);
			}
			else if ( %label $= "$Sky::skyColor" )
			{
				$Sky::SkyColor = getColorF(%value);
			}
			else if ( %label $= "$Sky::dayCycleFile" )
			{
				if ( isFile(%value) )
				{
					$Sky::DayCycleFile = %value;
				}
			}
			else if ( %label $= "$Sky::dayCycleEnabled" )
			{
				$Sky::DayCycleEnabled = mClamp (%value, 0, 1);
			}
			else if ( %label $= "$Sky::vignetteMultiply" )
			{
				$Sky::VignetteMultiply = mClamp (%value, 0, 1);
			}
			else if ( %label $= "$Sky::vignetteColor" )
			{
				$Sky::VignetteColor = getColorF(%value);
			}
			else if ( %label $= "$Sky::windVelocity" )
			{
				$Sky::windVelocity = %value;
			}
			else if ( %label $= "$Sky::windEffectPrecipitation" )
			{
				$Sky::windEffectPrecipitation = mClamp (%value, 0, 1);
			}


			// Precipitation

			else if ( %label $= "$Rain::DropTexture" )
			{
				if ( isFile(%value) )
				{
					$Rain::DropTexture = %value;
				}
			}
			else if ( %label $= "$Rain::SplashTexture" )
			{
				if ( isFile(%value) )
				{
					$Rain::SplashTexture = %value;
				}
			}
			else if ( %label $= "$Rain::DropSize" )
			{
				$Rain::DropSize = mClampF (%value, 0, 10);
			}
			else if ( %label $= "$Rain::SplashSize" )
			{
				$Rain::SplashSize = mClampF (%value, 0, 10);
			}
			else if ( %label $= "$Rain::SplashMS" )
			{
				$Rain::SplashMS = mClamp (%value, 0, 1000);
			}
			else if ( %label $= "$Rain::UseTrueBillBoards" )
			{
				$Rain::UseTrueBillboards = mClamp (%value, 0, 1);
			}
			else if ( %label $= "$Rain::minSpeed" )
			{
				$Rain::minSpeed = mClampF (%value, 0, 10);
			}
			else if ( %label $= "$Rain::maxSpeed" )
			{
				$Rain::maxSpeed = mClampF (%value, 0, 10);
			}
			else if ( %label $= "$Rain::minMass" )
			{
				$Rain::minMass = mClampF (%value, 0, 10);
			}
			else if ( %label $= "$Rain::maxMass" )
			{
				$Rain::maxMass = mClampF (%value, 0, 10);
			}
			else if ( %label $= "$Rain::maxTurbulence" )
			{
				$Rain::maxTurbulence = mClampF (%value, 0, 10);
			}
			else if ( %label $= "$Rain::turbulenceSpeed" )
			{
				$Rain::turbulenceSpeed = mClampF (%value, 0, 10);
			}
			else if ( %label $= "$Rain::rotateWithCamVel" )
			{
				$Rain::rotateWithCamVel = mClamp (%value, 0, 1);
			}
			else if ( %label $= "$Rain::useTurbulence" )
			{
				$Rain::useTurbulence = mClamp (%value, 0, 1);
			}
			else if ( %label $= "$Rain::numDrops" )
			{
				$Rain::numDrops = mClamp (%value, 1, 10000);
			}
			else if ( %label $= "$Rain::boxWidth" )
			{
				$Rain::boxWidth = mClamp (%value, 0, 1000);
			}
			else if ( %label $= "$Rain::boxHeight" )
			{
				$Rain::boxHeight = mClamp (%value, 0, 1000);
			}
			else if ( %label $= "$Rain::doCollision" )
			{
				$Rain::doCollision = mClamp (%value, 0, 1);
			}


			// Water

			else if ( %label $= "$Water::topTexture" )
			{
				if ( isFile(%value) )
				{
					$Water::TopTexture = %value;
				}
			}
			else if ( %label $= "$Water::bottomTexture" )
			{
				if ( isFile(%value) )
				{
					$Water::BottomTexture = %value;
				}
			}
			else if ( %label $= "$Water::color" )
			{
				$Water::Color = getColorI(%value);
			}
			else if ( %label $= "$Water::overlayColor" )
			{
				$Water::OverlayColor = getColorI(%value);
			}
			else if ( %label $= "$Water::scrollSpeed" )
			{
				$Water::ScrollSpeed = mClampF (getWord(%value, 0), -100, 100) SPC mClampF (getWord(%value, 1), -100, 100);
			}
			else if ( %label $= "$Water::loopsPerUnit" )
			{
				$Water::LoopsPerUnit = mClampF (%value, 0.001, 10);
			}
			else if ( %label $= "$Water::height" )
			{
				$Water::Height = mClampF (%value, 0, 100);
			}
			else if ( %label $= "$Water::kill" )
			{
				$Water::Kill = mClamp (%value, 0, 1);
			}
			else if ( %label $= "$Water::ColorMultiply" )
			{
				$Water::ColorMultiply = mClamp (%value, 0, 1);
			}
			

			// Ground

			if ( %label $= "$Ground::topTexture" )
			{
				$Ground::TopTexture = %value;
			}
			else if ( %label $= "$Ground::scrollSpeed" )
			{
				$Ground::ScrollSpeed = mClampF (getWord(%value, 0), -100, 100) SPC mClampF (getWord(%value, 1), -100, 100);
			}
			else if ( %label $= "$Ground::loopsPerUnit" )
			{
				$Ground::LoopsPerUnit = mClampF (%value, 0.001, 10);
			}
			else if ( %label $= "$Ground::Color" )
			{
				$Ground::Color = getColorI(%value);
			}
			else if ( %label $= "$Ground::ColorMultiply" )
			{
				$Ground::ColorMultiply = mClamp (%value, 0, 1);
			}
			else if ( %label $= "$Ground::RayCastColor" )
			{
				$Ground::RayCastColor = getColorI(%value);
			}
		}
	}

	%file.close();
	%file.delete();
}
