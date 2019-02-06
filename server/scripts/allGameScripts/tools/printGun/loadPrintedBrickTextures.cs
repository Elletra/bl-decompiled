function loadGameModePrintedBrickTextures ()
{
	deleteVariables ("$printNameTable*");
	deleteVariables ("$printARNumPrints*");
	deleteVariables ("$printARStart*");
	deleteVariables ("$printAREnd*");

	$globalPrintCount = 0;
	%localPrintCount = 0;

	for ( %i = 0;  %i < $GameMode::AddOnCount;  %i++ )
	{
		if ( !strnicmp ( $GameMode::AddOn[%i], "Print_", strlen("Print_") ) )
		{
			%start = strlen ("Print_");
			%end = stripos ($GameMode::AddOn[%i], "_", %start + 1);

			%loadingAR = getSubStr ($GameMode::AddOn[%i], %start, %end - %start);

			if ( %arLoaded[%loadingAR] != 1 )
			{
				$printARStart[%loadingAR] = $globalPrintCount;
				%localPrintCount = 0;

				for ( %j = %i;  %j < $GameMode::AddOnCount;  %j++ )
				{
					if ( !strnicmp ( $GameMode::AddOn[%j], "Print_", strlen("Print_") ) )
					{
						%start = strlen ("Print_");
						%end = stripos ($GameMode::AddOn[%j], "_", %start + 1);

						%testAR = getSubStr ($GameMode::AddOn[%j], %start, %end - %start);

						if ( %testAR $= %loadingAR )
						{
							echo ( "Loading " @  $GameMode::AddOn[%j] );

							%pattern = "Add-Ons/" @  $GameMode::AddOn[%j]  @ "/prints/*.png";
							%filename = findFirstFile (%pattern);

							while ( %filename !$= "" )
							{
								%fileBase = fileBase (%filename);
								%iconFileName = "Add-Ons/" @  $GameMode::AddOn[%j]  @ "/icons/" @  
												%fileBase  @ ".png";

								if ( strpos(%fileBase, " ") != -1 )
								{
									warn ("WARNING: loadGameModePrintedBrickTextures() - Bad print file name " @  
										%filename  @ " - Cannot have spaces");

									%filename = findNextFile (%pattern);
								}
								else if ( !isFile(%iconFileName) )
								{
									warn ("WARNING: loadGameModePrintedBrickTextures() - Print " @  
										%filename  @ " has no icon - skipping");

									%filename = findNextFile (%pattern);
								}
								else
								{
									%idString = %loadingAR  @ "/" @  %fileBase;

									if ( $printNameTable[%idString] !$= "" )
									{
										warn ("WARNING: loadGameModePrintedBrickTextures() - Print " @  
											%filename  @ " - " @  %idString  @ " already exists - skipping");

										%filename = findNextFile (%pattern);
									}
									else
									{
										$printNameTable[%idString] = $globalPrintCount;
										setPrintTexture ($globalPrintCount, %filename);

										$globalPrintCount++;
										%localPrintCount++;

										%filename = findNextFile (%pattern);
									}
								}
							}
						}
					}
				}

				$printARNumPrints[%loadingAR] = %localPrintCount;
				%arLoaded[%loadingAR] = 1;
				$printAREnd[%loadingAR] = $globalPrintCount;
			}
		}
	}
}

function loadPrintedBrickTextures ()
{
	%arList = "";
	%count = DataBlockGroup.getCount();

	for ( %i = 0;  %i < %count;  %i++ )
	{
		%db = DataBlockGroup.getObject (%i);

		if ( %db.getClassName() $= "fxDTSBrickData" )
		{
			%newAR = %db.printAspectRatio;

			if ( %newAR !$= "" )
			{
				if ( strpos(%newAR, " ") != -1 )
				{
					warn ("WARNING: loadPrintedBrickTextures() - Bad aspect ratio name " @  
						%newAR  @ " on " @  %db.getName()  @ " - Cannot have spaces");
				}
				else if ( strpos(%newAR, "/") != -1 )
				{
					warn ("WARNING: loadPrintedBrickTextures() - Bad aspect ratio name " @  
						%newAR @ " on " @  %db.getName()  @ " - Cannot have /\'s");
				}
				else if ( strpos(%newAR, "") != -1 )
				{
					warn ("WARNING: loadPrintedBrickTextures() - Bad aspect ratio name " @  
						%newAR  @ " on " @  %db.getName()  @ " - Cannot have \"\'s");
				}
				else
				{
					%arCount = getFieldCount (%arList);
					%addtoList = 1;

					for ( %j = 0;  %j < %arCount;  %j++ )
					{
						%field = getField (%arList, %j);

						if ( %field $= %newAR )
						{
							%addtoList = false;
						}
					}

					if ( %addtoList )
					{
						if ( %arList $= "" )
						{
							%arList = %newAR;
						}
						else
						{
							%arList = %arList TAB %newAR;
						}
					}
				}
			}
		}
	}

	deleteVariables ("$printNameTable*");
	$globalPrintCount = 0;

	%fieldCount = getFieldCount (%arList);

	for ( %i = 0;  %i < %fieldCount;  %i++ )
	{
		%field = getField (%arList, %i);
		loadPrintedBrickTexture (%field);
	}

	loadPrintedBrickTexture ("Letters");
}

function loadPrintedBrickTexture ( %aspectRatio )
{
	$printARStart[%aspectRatio] = $globalPrintCount;

	if ( %aspectRatio $= "Letters"  &&  $AddOn__["Print_Letters_Default"] == 1 )
	{
		%localPrintCount = loadDefaultLetterPrints();
	}
	else
	{
		%localPrintCount = 0;
	}

	%dir = "Add-Ons/Print_" @  %aspectRatio  @ "_*/prints/*.png";

	%fileCount = getFileCount (%dir);
	%filename = findFirstFile (%dir);

	for ( %i = 0;  %i < %fileCount;  %i++ )
	{
		%path = filePath (%filename);
		%dirName = getSubStr ( %path, strlen("Add-Ons/"), strlen(%path) - strlen("Add-Ons/") - strlen("/prints") );
		%varName = getSafeVariableName (%dirName);

		if ( $AddOn__[%varName] $= "" )
		{
			%filename = findNextFile (%dir);
		}
		else if ( $AddOn__[%varName] !$= 1 )
		{
			%filename = findNextFile (%dir);
		}
		else if ( %dirName !$= "Print_Letters_Default" )
		{
			%fileBase = fileBase (%filename);

			if ( strpos(%fileBase, " ") != -1 )
			{
				warn ("WARNING: loadPrintedBrickTexture() - Bad print file name " @  
					%filename  @ " - Cannot have spaces");

				%filename = findNextFile (%dir);
			}
			else
			{
				%iconFileName = "Add-Ons/" @  %dirName  @ "/icons/" @  %fileBase  @ ".png";

				if ( !isFile(%iconFileName) )
				{
					warn("WARNING: loadPrintedBrickTexture() - Print " @  
						%filename  @ " has no icon - skipping");

					%filename = findNextFile (%dir);
				}
				else
				{
					%idString = %aspectRatio  @ "/" @  %fileBase;

					if ( $printNameTable[%idString] !$= "" )
					{
						warn ("WARNING: loadPrintedBrickTexture() - Print " @  
							%filename  @ " - " @  %idString  @ " already exists - skipping");

						%filename = findNextFile (%dir);
					}
					else
					{
						$printNameTable[%idString] = $globalPrintCount;
						setPrintTexture ($globalPrintCount, %filename);

						$globalPrintCount++;
						%localPrintCount++;

						%filename = findNextFile (%dir);
					}
				}
			}
		}
	}

	$printARNumPrints[%aspectRatio] = %localPrintCount;
	$printAREnd[%aspectRatio] = $globalPrintCount;
}
