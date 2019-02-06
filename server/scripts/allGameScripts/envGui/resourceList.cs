function EnvGuiServer::populateEnvResourceList()
{
	$EnvGuiServer::ResourceCount = 0;

	EnvGuiServer::AddToResourceList ("Add-Ons/Sky_*/*.png");
	EnvGuiServer::AddToResourceList ("Add-Ons/Sky_*/*.jpg");
	EnvGuiServer::AddToResourceList ("Add-Ons/Sky_*/*.dml");
	EnvGuiServer::AddToResourceList ("Add-Ons/Ground_*/*.png");
	EnvGuiServer::AddToResourceList ("Add-Ons/Ground_*/*.jpg");
	EnvGuiServer::AddToResourceList ("Add-Ons/Water_*/*.png");
	EnvGuiServer::AddToResourceList ("Add-Ons/Water_*/*.jpg");

	echo ($EnvGuiServer::ResourceCount  @ " environmental resource files found");
}

function EnvGuiServer::addToResourceList ( %pattern )
{
	%filename = findFirstFile (%pattern);

	while ( %filename !$= "" )
	{
		$EnvGuiServer::Resource[$EnvGuiServer::ResourceCount] = %filename;
		$EnvGuiServer::ResourceCount++;

		%filename = findNextFile (%pattern);
	}
}
