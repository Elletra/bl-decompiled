function GameModeGuiServer::getMissingAddOns ( %filename )
{
	if ( !isFile(%filename) )
	{
		error ("ERROR: GameModeGuiServer::GetMissingAddOns(" @  %filename  @ ") - file does not exist");
		return 0;
	}

	%path = filePath (%filename);
	%missingAddons = "";

	if ( !isUnlocked() )
	{
		%zipFile = %path  @ ".zip";

		if ( isFile(%zipFile) )
		{
			%crc = getFileCRC (%zipFile);

			if ( !isValidDemoCRC(%crc) )
			{
				%missingAddons = %missingAddons TAB "Full version of game required.";
			}
		}
		else
		{
			%missingAddons = %missingAddons TAB "Full version of game required.";
		}
	}

	%descriptionFile = %path  @ "/description.txt";
	%previewFile = %path  @ "/preview.jpg";
	%thumbFile = %path  @ "/thumb.jpg";
	%saveFile = %path  @ "/save.bls";
	%colorSetFile = %path  @ "/colorSet.txt";

	if ( !isFile(%descriptionFile) )
	{
		%missingAddons = %missingAddons TAB %descriptionFile;
	}

	if ( !isFile(%previewFile) )
	{
		%missingAddons = %missingAddons TAB %previewFile;
	}

	if ( !isFile(%thumbFile) )
	{
		%missingAddons = %missingAddons TAB %thumbFile;
	}

	if ( !isFile(%saveFile) )
	{
		%missingAddons = %missingAddons TAB %saveFile;
	}

	if ( !isFile(%colorSetFile) )
	{
		%missingAddons = %missingAddons TAB %colorSetFile;
	}

	%file = new FileObject();
	%file.openForRead (%filename);

	while ( !%file.isEOF() )
	{
		%line = %file.readLine();
		%label = getWord (%line, 0);
		%value = trim ( getWords(%line, 1, 999) );

		if ( %label !$= ""  &&  getSubStr(%label, 0, 2) !$= "//" )
		{
			if ( %label $= "ADDON" )
			{
				if ( !isFile("Add-Ons/" @  %value  @ "/description.txt")  ||  !isFile("Add-Ons/" @  %value  @ "/server.cs") )
				{
					if ( strlen(%missingAddons) > 0 )
					{
						%missingAddons = %missingAddons TAB %value;
					}
					else
					{
						%missingAddons = %value;
					}
				}
			}
			else if ( %label $= "MUSIC" )
			{
				if ( !isFile("Add-Ons/Music/" @  %value  @ ".ogg") )
				{
					if ( strlen(%missingAddons) > 0 )
					{
						%missingAddons = %missingAddons TAB %value  @ ".ogg";
					}
					else
					{
						%missingAddons = %value;
					}
				}
			}
		}
	}

	%file.close();
	%file.delete();

	return %missingAddons;
}
