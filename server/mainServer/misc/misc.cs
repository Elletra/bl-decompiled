exec ("./killDupes.cs");
exec ("./UPnP.cs");
exec ("./message.cs");
exec ("./miscGameConnection.cs");


function getMainMod ()
{
	%modPaths = getModPaths();
	%modPaths = strreplace (%modPaths, ";", "\t");
	%count = getFieldCount (%modPaths);

	%bestMod = "base";

	for ( %i = 0;  %i < %count;  %i++ )
	{
		%field = getField (%modPaths, %i);

		if ( %field !$= "base"  &&  %field !$= "screenshots"  &&  %field !$= "editor"  &&  %field !$= "Add-ons")
		{
			%bestMod = %field;
		}
	}

	return urlEnc (%bestMod);
}

function isNameUnique ( %client, %name )
{
	%count = ClientGroup.getCount();

	for ( %i = 0;  %i < %count;  %i++ )
	{
		%test = ClientGroup.getObject(%i);

		if ( %client != %test )
		{
			%rawName = stripChars ( detag ( getTaggedString ( %test.getPlayerName() ) ),  "\x10\x11\c6\c7\c8\c9" );

			if ( strcmp(%name, %rawName) == 0 )
			{
				return 0;
			}
		}
	}

	return 1;
}


function createGame ()
{
	// Your code here
}

function destroyGame ()
{
	// Your code here
}

function startGame ()
{
	// Your code here
}

function endGame ()
{
	// Your code here
}
