function dumpLighting ()
{
	%file = new FileObject();
	%file.openForAppend ("base/dayCycleTemp.txt");

	%fraction = Sun.elevation;

	if ( %fraction < 0 )
	{
		%fraction += 360;
	}

	%fraction /= 360;

	%file.writeLine ("FRACTION " @  %fraction);
	%file.writeLine ("DIRECTCOLOR " @  $EnvGuiServer::DirectLightColor);
	%file.writeLine ("AMBIENTCOLOR " @  $EnvGuiServer::AmbientLightColor);
	%file.writeLine ("SKYCOLOR " @  $EnvGuiServer::SkyColor);
	%file.writeLine ("FOGCOLOR " @  $EnvGuiServer::FogColor);
	%file.writeLine ("SHADOWCOLOR " @  $EnvGuiServer::ShadowColor);
	%file.writeLine ("SUNFLARECOLOR " @  $EnvGuiServer::SunFlareColor);
	%file.writeLine ("");

	%file.close();
	%file.delete();
}

function dumpServerSkyBoxList ()
{
	echo ($EnvGuiServer::SkyCount  @ " skies");

	for ( %i = 0;  %i < $EnvGuiServer::SkyCount;  %i++ )
	{
		echo ( %i  @ " : " @  $EnvGuiServer::Sky[%i] );
	}
}
