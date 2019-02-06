function serverCmdEnvGui_ClickDefaults ( %client )
{
	if ( !%client.isAdmin )
	{
		return;
	}

	EnvGuiServer::SetSimpleMode();
	EnvGuiServer::readAdvancedVarsFromSimple();
	EnvGuiServer::SetAdvancedMode();

	serverCmdEnvGui_RequestCurrentVars (%client);
}

function setGround ( %filename )
{
	$Ground::LoopsPerUnit = 1;
	$Ground::ScrollSpeed = "0 0";
	$Ground::Color = "255 255 255 255";
	$Ground::RayCastColor = "128 128 128 255";
	$Ground::ColorMultiply = 1;

	parseEnvironmentFile (%filename);

	groundPlane.topTexture = $Ground::TopTexture;
	groundPlane.bottomTexture = $Ground::TopTexture;
	groundPlane.loopsPerUnit = $Ground::LoopsPerUnit;
	groundPlane.scrollSpeed = $Ground::ScrollSpeed;

	groundPlane.color = $Ground::Color;
	groundPlane.blend = getWord (groundPlane.color, 3) < 255;
	groundPlane.colorMultiply = $Ground::ColorMultiply;
	groundPlane.rayCastColor = $Ground::RayCastColor;

	Sky.renderBottomTexture = getWord (groundPlane.color, 3) <= 0;
	Sky.noRenderBans = Sky.renderBottomTexture;

	Sky.sendUpdate();
	groundPlane.sendUpdate();
}


exec ("./simpleAdvanced/simpleAdvanced.cs");
exec ("./currentVars.cs");
exec ("./dayCycle.cs");
exec ("./dump.cs");
exec ("./envLists.cs");
exec ("./getIdxFromFilenames.cs");
exec ("./parseEnvironmentFile.cs");
exec ("./resourceList.cs");
exec ("./saveEnvironment.cs");
exec ("./sendVignette.cs");
exec ("./setSkyBox.cs");
exec ("./setVar.cs");
exec ("./setWater.cs");
exec ("./validateEnvironmentObjects.cs");
