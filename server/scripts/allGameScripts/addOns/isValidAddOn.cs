function isValidAddOn ( %dirName, %verbose )
{
	%dirName = strlwr (%dirName);

	if ( strstr(%dirName, "/") != -1 )
	{
		if ( %verbose )
		{
			warn ("    nested add-on - will not execute");
		}

		return false;
	}

	if ( strstr(%dirName, "_") == -1 )
	{
		if ( %verbose )
		{
			warn ("    Add-On folder does not follow the format <category>_<name> - will not execute");
		}

		return false;
	}

	%descriptionName = "Add-Ons/" @  %dirName  @ "/description.txt";

	if ( !isFile(%descriptionName) )
	{
		if ( %verbose )
		{
			warn ("    No description.txt for this add-on - will not execute");
		}

		return false;
	}

	if ( strstr(%dirName, "Copy of") != -1  ||  strstr(%dirName, "- Copy") != -1 )
	{
		if ( %verbose )
		{
			warn ("    Add-On folder is a copy - will not execute");
		}

		return false;
	}

	if ( strstr(%dirName, "(") != -1  ||  strstr(%dirName, ")") != -1 )
	{
		if ( %verbose )
		{
			warn ("    Add-On folder contains ()\'s, possibly a duplicate - will not execute");
		}

		return false;
	}

	%wordCount = getWordCount (%dirName);

	if ( %wordCount > 1 )
	{
		%lastWord = getWord (%dirName, %wordCount - 1);
		%floorLastWord = mFloor (%lastWord);

		if ( %floorLastWord $= %lastWord )
		{
			if ( %verbose )
			{
				warn ("    Add-On folder ends in \" " @  %lastWord  @ 
					", possibly a duplicate - will not execute");
			}

			return false;
		}
	}

	if ( strstr(%dirName, "+") != -1 )
	{
		if ( %verbose )
		{
			warn ("    Add-On folder contains +\'s - will not execute");
		}

		return false;
	}

	if ( strstr(%dirName, "[") != -1  ||  strstr(%dirName, "]") != -1 )
	{
		if ( %verbose )
		{
			warn ("    Add-On folder contains []\'s, possibly a duplicate - will not execute");
		}

		return false;
	}

	if ( strstr(%dirName, " ") != -1 )
	{
		if ( %verbose )
		{
			warn ("    Add-On folder contains a space - will not execute");
		}

		return false;
	}

	%spaceName = strreplace (%dirName, "_", " ");
	%firstWord = getWord (%spaceName, 0);

	if ( mFloor(%firstWord) $= %firstWord )
	{
		if ( %verbose )
		{
			warn ("    Add-On folder begins with " @  %firstWord  @ "_\" - will not execute");
		}

		return false;
	}

	%wordCount = getWordCount (%spaceName);
	%lastWord = getWord (%spaceName, %wordCount - 1);

	if ( mFloor(%lastWord) $= %lastWord )
	{
		if ( %verbose )
		{
			warn ("    Add-On folder ends with \"_" @  %lastWord  @ " - will not execute");
		}

		return false;
	}

	%nameCheckFilename = "Add-Ons/" @  %dirName  @ "/namecheck.txt";

	if ( isFile(%nameCheckFilename) )
	{
		%file = new FileObject();
		%file.openForRead (%nameCheckFilename);

		%nameCheck = %file.readLine();

		%file.close();
		%file.delete();

		if ( %nameCheck !$= %dirName )
		{
			if ( %verbose )
			{
				warn ("    Add-On has been renamed from " @  %nameCheck  @ "\" to " @  
					%dirName  @ " - will not execute");
			}

			return false;
		}
	}

	%zipFile = "Add-Ons/" @  %dirName  @ ".zip";

	if ( isFile(%zipFile) )
	{
		%zipCRC = getFileCRC (%zipFile);

		if ( $CrapOnCRC_[%zipCRC] == 1 )
		{
			if ( %verbose )
			{
				warn ("    Add-On is in the list of known bad add-on CRCs - will not execute");
			}

			return false;
		}

		if ( !isUnlocked() )
		{
			if ( !isValidDemoCRC(%zipCRC) )
			{
				if ( %verbose )
				{
					warn ("    Cannot use this add-on in demo mode");
				}

				return false;
			}
		}
	}
	else
	{
		if ( !isUnlocked() )
		{
			if ( %verbose )
			{
				warn ("    Cannot use non-zipped add-ons in demo mode");
			}

			return false;
		}
	}

	if ( $CrapOnName_[%dirName] == 1 )
	{
		if ( %verbose )
		{
			warn ("    Add-On is in the list of known bad add-on names - will not execute");
		}

		return false;
	}

	if ( strstr(%dirName, ".zip") != -1 )
	{
		if ( %verbose )
		{
			warn ("    Add-On folder name contains \".zip\" - will not execute (also please kill yourself)");
		}

		return false;
	}

	if ( $Server::Dedicated )
	{
		if ( $CrapOnDedicatedName_[%dirName] == 1 )
		{
			if ( %verbose )
			{
				warn ("    Add-On is in the list of known bad add-on names for dedicated servers - will not execute");
			}

			return false;
		}

		%zipFile = "Add-Ons/" @  %dirName  @ ".zip";

		if ( isFile(%zipFile) )
		{
			%zipCRC = getFileCRC (%zipFile);

			if ( $CrapOnDedicatedCRC_[%zipCRC] == 1 )
			{
				if ( %verbose )
				{
					warn ("    Add-On is in the list of known bad add-on CRCs for dedicated servers - will not execute");
				}

				return false;
			}
		}
	}

	return true;
}
