function createGameModeMusicDataBlocks ()
{
	for ( %i = 0;  %i < $GameMode::MusicCount;  %i++ )
	{
		%filename = "Add-Ons/Music/" @  $GameMode::Music[%i]  @ ".ogg";
		%base = fileBase (%filename);
		%uiName = strreplace (%base, "_", " ");
		%varName = getSafeVariableName (%base);

		if ( !$Server::Dedicated )
		{
			if ( getRealTime() - $lastProgressBarTime > 200 )
			{
				LoadingProgress.setValue (%i / %fileCount);
				$lastProgressBarTime = getRealTime();
				Canvas.repaint();
			}
		}

		if ( !isFile(%filename) )
		{
			error ("ERROR: createGameModeMusicDataBlocks() - file \'" @  %filename  @ "\' does not exist");
		}
		else if ( isValidMusicFilename(%filename) )
		{
			if ( getFileLength(%filename) > 1048576 )
			{
				error ("ERROR: createGameModeMusicDataBlocks() - Music file " @  %filename  @ " > 1mb - ignoring");
			}
			else
			{
				%dbName = "musicData_" @  %varName;
				%command = "datablock AudioProfile(" @  %dbName  @ ") {" @ 
					"filename = \"" @  %filename  @ "\";" @ 
					"description = AudioMusicLooping3d;" @ 
					"preload = true;" @ 
					"uiName = \"" @ %uiName @ "\";" @ 
				"};";
				
				eval (%command);

				if ( %dbName.isStereo() )
				{
					error ("ERROR: createGameModeMusicDataBlocks() - Stereo sound detected on " @  
						%dbName.getName()  @ " - Removing datablock.");

					schedule (1000, 0, MessageAll, '', "Stereo sound detected on  " @  fileName(%dbName.fileName)  
						@ " - Removing datablock.");

					%dbName.uiName = "";
					%dbName.delete();

					if ( getBuildString() $= "Ship" )
					{
						fileDelete (%filename);
					}
					else
					{
						warning ("WARNING: \'" @  %filename  
							@ "\' is a stereo music block and would be deleted if this was the public build!");
					}
				}
			}
		}
	}
}

function createMusicDatablocks ()
{
	updateMusicList();

	%dir = "Add-ons/Music/*.ogg";

	%fileCount = getFileCount (%dir);
	%filename = findFirstFile (%dir);

	for ( %i = 0;  %i < %fileCount;  %i++ )
	{
		%base = fileBase (%filename);
		%uiName = strreplace (%base, "_", " ");
		%varName = getSafeVariableName (%base);

		if ( !$Server::Dedicated )
		{
			if ( getRealTime() - $lastProgressBarTime > 200 )
			{
				LoadingProgress.setValue (%i / %fileCount);
				$lastProgressBarTime = getRealTime();
				Canvas.repaint();
			}
		}

		if ( !isValidMusicFilename(%filename) )
		{
			%filename = findNextFile (%dir);
		}
		else if ( getFileLength(%filename) > 1048576 )
		{
			error ("ERROR: Music file " @  %filename  @ " > 1mb - ignoring");
			%filename = findNextFile (%dir);
		}
		else
		{
			if ( $Music__[%varName] $= 1 )
			{
				%dbName = "musicData_" @  %varName;
				%command = "datablock AudioProfile(" @  %dbName  @ ") {" @ 
					"filename = \"" @  %filename  @ "\";" @ 
					"description = AudioMusicLooping3d;" @ 
					"preload = true;" @ 
					"uiName = \"" @ %uiName @ "\";" @ 
				"};";

				eval (%command);

				if ( %dbName.isStereo() )
				{
					error ("ERROR: Stereo sound detected on " @  %dbName.getName()  @ " - Removing datablock.");
					schedule (1000, 0, MessageAll, '', "Stereo sound detected on  " @  fileName(%dbName.fileName)  @ 
						" - Removing datablock.");

					%dbName.uiName = "";
					%dbName.delete();

					if ( getBuildString() $= "Ship" )
					{
						fileDelete (%filename);
					}
					else
					{
						warning ("WARNING: \'" @  %filename  @ 
							"\' is a stereo music block and would be deleted if this was the public build!");
					}
				}
			}

			%filename = findNextFile (%dir);
		}
	}
}
