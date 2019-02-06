function updateWaterFog ()
{
	%height = getWord (WaterPlane.getTransform(), 2);
	%waterVis = 220 - getWord ($EnvGuiServer::WaterColor, 3) * 200;

	Sky.fogVolume1 = %waterVis SPC -10 SPC %height;
	Sky.sendUpdate();
}


function setWater ( %filename )
{
	if ( isObject(WaterZone) )
	{
		WaterZone.delete();
	}

	$Water::LoopsPerUnit = 1;
	$Water::ScrollSpeed = "0 0";
	$Water::Color = "255 255 255 128";
	$Water::OverlayColor = "0 0 128 128";
	$Water::Height = 5;
	$Water::Kill = 0;
	$Water::ColorMultiply = 1;

	if ( %filename $= "NONE" )
	{
		Sky.fogVolume1 = "0 0 0";
		Sky.sendUpdate();

		if ( isObject(WaterPlane) )
		{
			WaterPlane.delete();
		}

		return;
	}

	parseEnvironmentFile (%filename);

	if ( !isObject(WaterPlane) )
	{
		%pos = getWords (groundPlane.getTransform(), 0, 2);
		%pos = VectorAdd (%pos, "0 0 " @  $Water::Height);

		new fxPlane (WaterPlane)
		{
			position = %pos;
			blend = 0;
			useShader = true;
			isSolid = false;
		};
		MissionGroup.add (WaterPlane);
	}

	if ( !isObject(WaterZone) )
	{
		%pos = getWords (WaterPlane.getTransform(), 0, 2);
		%pos = VectorSub (%pos, "0 0 100");
		%pos = VectorAdd (%pos, "0 0 0.5");
		%pos = VectorSub (%pos, "500000 -500000 0");

		%waterType = 0;

		if ( $Water::Kill )
		{
			%waterType = 8;
		}

		new PhysicalZone (WaterZone)
		{
			isWater = true;

			waterViscosity = 40;
			waterDensity = 1;
			waterColor = getColorF ($Water::OverlayColor);

			polyhedron = "0 0 0 1 0 0 0 -1 0 0 0 1";
			position = %pos;

			waterType = %waterType;
		};
		MissionGroup.add (WaterZone);
		WaterZone.setScale ("1000000 1000000 100");
	}

	WaterPlane.topTexture = $Water::TopTexture;
	WaterPlane.bottomTexture = $Water::BottomTexture;
	WaterPlane.loopsPerUnit = $Water::LoopsPerUnit;
	WaterPlane.scrollSpeed = $Water::ScrollSpeed;

	%pos = getWords (groundPlane.getTransform(), 0, 2);
	%pos = VectorAdd (%pos, "0 0 " @  $Water::Height);

	WaterPlane.setTransform (%pos  @ " 0 0 1 0");

	updateWaterFog();

	%pos = getWords (WaterPlane.getTransform(), 0, 2);
	%pos = VectorSub (%pos, "0 0 100");
	%pos = VectorAdd (%pos, "0 0 0.5");
	%pos = VectorSub (%pos, "500000 -500000 0");

	WaterZone.appliedForce = getWord ($Water::ScrollSpeed, 0) * 450 SPC 
		getWord ($Water::ScrollSpeed, 1) * 450 SPC 0;

	WaterZone.setTransform (%pos  @ " 0 0 1 0");

	WaterPlane.color = $Water::Color;
	WaterPlane.colorMultiply = $Water::ColorMultiply;
	WaterPlane.blend = getWord (WaterPlane.color, 3) < 255;

	WaterPlane.sendUpdate();
}
