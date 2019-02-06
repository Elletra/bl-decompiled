function CustomGameGuiServer::populateAddOnList ()
{
	deleteVariables ("$CustomGameGuiServer::AddOn*");

	$CustomGameGuiServer::AddOnCount = 0;

	%pattern = "Add-Ons/*/server.cs";

	%fileCount = getFileCount (%pattern);
	%filename = findFirstFile (%pattern);

	for ( %i = 0;  %i < %fileCount;  %i++ )
	{
		%path = filePath (%filename);
		%dirName = getSubStr ( %path, strlen("Add-Ons/"), strlen(%path) - strlen("Add-Ons/") );
		%varName = getSafeVariableName (%dirName);

		if ( !isValidAddOn(%dirName, 1) )
		{
			%filename = findNextFile (%pattern);
		}
		else
		{
			$CustomGameGuiServer::AddOn[$CustomGameGuiServer::AddOnCount] = %dirName;
			$CustomGameGuiServer::AddOnCount++;

			%filename = findNextFile (%pattern);
		}
	}
}

function serverCmdCustomGameGui_SetAddOnEnabled ( %client, %varName, %enabled )
{
	if ( !%client.isAdmin )
	{
		return;
	}

	if ( isListenServer() )
	{
		if ( !%client.isLocal() )
		{
			return;
		}
	}

	%varName = getSafeVariableName (%varName);

	if ( %enabled > 0 )
	{
		%enabled = 1;
	}
	else
	{
		%enabled = -1;
	}

	$AddOn__[%varName] = %enabled;
}
