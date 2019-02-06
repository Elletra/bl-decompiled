// e

function E ( %val )
{
	if ( !isUnlocked() )
	{
		return;
	}


	%filePattern = "base/server/*" @ %val @ "*.cs";
	%file = findFirstFile (%filePattern);

	while ( %file !$= "" )
	{
		exec (%file);
		%file = findNextFile (%filePattern);
	}

	%filePattern = "base/server/*/" @ %val @ "*.cs";
	%file = findFirstFile (%filePattern);

	while ( %file !$= "" )
	{
		exec (%file);
		%file = findNextFile (%filePattern);
	}

	%filePattern = "add-ons/*/" @ %val @ "*.cs";
	%file = findFirstFile (%filePattern);

	while ( %file !$= "" )
	{
		exec (%file);
		%file = findNextFile (%filePattern);
	}

	%filePattern = "add-ons/" @ %val @ "*.cs";
	%file = findFirstFile (%filePattern);

	while ( %file !$= "" )
	{
		exec (%file);
		%file = findNextFile (%filePattern);
	}
}
