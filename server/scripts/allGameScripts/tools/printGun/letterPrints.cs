function loadDefaultLetterPrints ()
{
	%dir = "Add-Ons/Print_Letters_Default/prints/*.png";

	%fileCount = getFileCount (%dir);
	%filename = findFirstFile (%dir);

	%localPrintCount = 0;

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
		else
		{
			%fileBase = fileBase (%filename);

			if ( strpos(%fileBase, " ") != -1 )
			{
				%filename = findNextFile (%dir);
			}
			else
			{
				%iconFileName = "Add-Ons/" @  %dirName  @ "/icons/" @  %fileBase  @ ".png";

				if ( !isFile(%iconFileName) )
				{
					%filename = findNextFile (%dir);
				}
				else
				{
					%idString = "Letters/" @  %fileBase;

					if ( $printNameTable[%idString] !$= "" )
					{
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

	return %localPrintCount;
}

function sendLetterPrintInfo ( %client )
{
	commandToClient ( %client, 'SetLetterPrintInfo', $printARStart["Letters"], $printARNumPrints["Letters"] );
}
