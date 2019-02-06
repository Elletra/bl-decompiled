datablock AudioDescription (AudioMusicLooping3d)
{
	volume = 1;
	isLooping = true;
	is3D = true;
	referenceDistance = 10;
	maxDistance = 30;
	type = $SimAudioType;
};


function updateMusicList ()
{
	deleteVariables ("$Music*");

	if ( isFile("config/server/musicList.cs") )
	{
		exec ("config/server/musicList.cs");
	}
	else
	{
		exec ("base/server/defaultMusicList.cs");
	}

	%dir = "base/data/sound/music/*.ogg";

	%fileCount = getFileCount (%dir);
	%filename = findFirstFile (%dir);

	for ( %i = 0;  %i < %fileCount;  %i++ )
	{
		%base = fileBase (%filename);
		%varName = getSafeVariableName (%base);

		if ( !isValidMusicFilename(%filename) )
		{
			%filename = findNextFile (%dir);
		}
		else
		{
			if ( mFloor ( $Music__[%varName] ) <= 0 )
			{
				$Music__[%varName] = -1;
			}
			else
			{
				$Music__[%varName] = 1;
			}

			%filename = findNextFile (%dir);
		}
	}

	export ("$Music__*", "config/server/musicList.cs");
}

function isValidMusicFilename ( %filename )
{
	%base = fileBase (%filename);
	%uiName = strreplace (%base, "_", " ");
	%firstWord = getWord (%uiName, 0);

	if ( %firstWord $= mFloor(%firstWord) )
	{
		return false;
	}

	%pos = strpos (%filename, "/", strlen("Add-Ons/Music/") + 1);

	if ( %pos != -1 )
	{
		return false;
	}

	if ( strstr(%filename, "Copy of") != -1  ||  strstr(%filename, "Copy_of") != -1  ||  
		 strstr(%filename, "- Copy") != -1   ||  strstr(%filename, "-_Copy") != -1   ||  
		 strstr(%filename, "(") != -1        ||  strstr(%filename, ")") != -1        ||  
		 strstr(%filename, "[") != -1        ||  strstr(%filename, "]") != -1        ||  
		 strstr(%filename, "+") != -1        ||  strstr(%filename, " ") != -1 )
	{
		return false;
	}

	return true;
}


exec ("./createMusicDataBlocks.cs");
