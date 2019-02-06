function isValidMap ( %file )
{
	%path = filePath (%file);
	%dirName = getSubStr ( %path, strlen("Add-Ons/"), strlen(%path) - strlen("Add-Ons/") );
	%mapName = fileBase (%file);

	if ( !isFile(%file) )
	{
		return false;
	}

	if ( %dirName $= "Map_Tutorial" )
	{
		return false;
	}

	if ( strstr(%dirName, "/") != -1 )
	{
		return false;
	}

	if ( strstr(%dirName, "_") == -1 )
	{
		return false;
	}

	if ( strstr(%dirName, "Copy of") != -1  ||  strstr(%dirName, "- Copy") != -1 )
	{
		return false;
	}

	if ( strstr(%dirName, "(") != -1  ||  strstr(%dirName, ")") != -1 )
	{
		return false;
	}

	%wordCount = getWordCount (%dirName);

	if ( %wordCount > 1 )
	{
		%lastWord = getWord (%dirName, %wordCount - 1);
		%floorLastWord = mFloor (%lastWord);

		if ( %floorLastWord $= %lastWord )
		{
			return false;
		}
	}

	if ( strstr(%dirName, "+") != -1 )
	{
		return false;
	}

	if ( strstr(%dirName, "[") != -1  ||  strstr(%dirName, "]") != -1 )
	{
		return false;
	}

	if ( strstr(%dirName, " ") != -1 )
	{
		return false;
	}

	%spaceName = strreplace (%dirName, "_", " ");
	%firstWord = getWord (%spaceName, 0);

	if ( mFloor(%firstWord) $= %firstWord )
	{
		return false;
	}

	%wordCount = getWordCount (%spaceName);
	%lastWord = getWord (%spaceName, %wordCount - 1);

	if ( mFloor(%lastWord) $= %lastWord )
	{
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
			return false;
		}
	}

	%zipFile = "Add-Ons/" @  %dirName  @ ".zip";

	if ( isFile(%zipFile) )
	{
		%zipCRC = getFileCRC (%zipFile);

		if ( $CrapOnCRC_[%zipCRC] == 1 )
		{
			return false;
		}
	}

	if ( $CrapOnName_[%dirName] == 1 )
	{
		return false;
	}

	if ( strstr(%dirName, ".zip") != -1 )
	{
		return false;
	}

	return true;
}
