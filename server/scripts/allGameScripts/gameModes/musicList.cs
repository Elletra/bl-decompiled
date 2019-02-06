function CustomGameGuiServer::populateMusicList ()
{
	deleteVariables ("$CustomGameGuiServer::Music*");

	$CustomGameGuiServer::MusicCount = 0;

	%pattern = "Add-Ons/Music/*.ogg";

	%fileCount = getFileCount (%pattern);
	%filename = findFirstFile (%pattern);

	for ( %i = 0;  %i < %fileCount;  %i++ )
	{
		%base = fileBase (%filename);
		%uiName = strreplace (%base, "_", " ");
		%varName = getSafeVariableName (%base);

		if ( !isValidMusicFilename(%filename) )
		{
			%filename = findNextFile (%pattern);
		}
		else
		{
			$CustomGameGuiServer::Music[$CustomGameGuiServer::MusicCount] = %base;
			$CustomGameGuiServer::MusicCount++;

			%filename = findNextFile (%pattern);
		}
	}
}


function serverCmdCustomGameGui_SetMusicEnabled ( %client, %varName, %enabled )
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

	$Music__[%varName] = %enabled;
}
