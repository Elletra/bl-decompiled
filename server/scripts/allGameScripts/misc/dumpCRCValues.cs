function dumpCRCValues ()
{
	if ( getBuildString() !$= "Debug"  &&  getBuildString() !$= "Release" )
	{
		return;
	}

	%file = new FileObject();
	%file.openForWrite ("base/Protection.txt");

	%file.writeLine ("#include \"console/console.h");
	%file.writeLine ("");
	%file.writeLine ("ConsoleFunction(isValidDemoCRC, bool, 2, 2, \"(int CRC)\")");
	%file.writeLine ("{");
	%file.writeLine ("   //returns true if CRC is in our white list");
	%file.writeLine ("   int crc = dAtoi(argv[1]);");
	%file.writeLine ("");
	%file.writeLine ("   #ifdef TORQUE_DEBUG");
	%file.writeLine ("   return true;");
	%file.writeLine ("   #endif");
	%file.writeLine ("");

	%pattern = "Add-Ons/*.zip";
	%addOn = findFirstFile (%pattern);

	while ( %addOn !$= "" )
	{
		if ( %addOn $= "Add-Ons/GameMode_Custom.zip" )
		{
			%addOn = findNextFile (%pattern);
		}
		else
		{
			%crc = getFileCRC (%addOn);
			%padlen = 12 - strlen (%crc);

			%file.writeLine ("   if (crc == " @  %crc  @ ")" @  makePadString(" ", %padlen)  @ 
				"return true; //" @  %addOn);

			%addOn = findNextFile (%pattern);
		}
	}


	%pattern = "saves/*.bls";
	%addOn = findFirstFile (%pattern);

	while ( %addOn !$= "" )
	{
		%crc = getFileCRC (%addOn);
		%padlen = 12 - strlen (%crc);

		%file.writeLine ("   if (crc == " @  %crc  @ ")" @  makePadString(" ", %padlen)  @ 
			"return true; //" @  %addOn);

		%addOn = findNextFile (%pattern);
	}


	%pattern = "Add-Ons/Gamemode_*/*.bls";
	%addOn = findFirstFile (%pattern);

	while ( %addOn !$= "" )
	{
		%crc = getFileCRC (%addOn);
		%padlen = 12 - strlen (%crc);

		%file.writeLine ("   if (crc == " @  %crc  @ ")" @  makePadString(" ", %padlen)  @ 
			"return true; //" @  %addOn);
		
		%addOn = findNextFile (%pattern);
	}

	%file.writeLine ("");
	%file.writeLine ("   return false;");
	%file.writeLine ("}");

	%file.close();
	%file.delete();
}
