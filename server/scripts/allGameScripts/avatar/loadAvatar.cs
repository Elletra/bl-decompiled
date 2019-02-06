function serverLoadAvatarNames ()
{
	serverLoadAvatarName ("decal", "base/data/shapes/player/decal.ifl");
	serverLoadAvatarName ("hat", "base/data/shapes/player/hat.txt");

	serverLoadAccentInfo ("accent", "base/data/shapes/player/accent.txt");

	serverLoadAvatarName ("pack", "base/data/shapes/player/pack.txt");
	serverLoadAvatarName ("secondPack", "base/data/shapes/player/secondPack.txt");
	serverLoadAvatarName ("LLeg", "base/data/shapes/player/LLeg.txt");
	serverLoadAvatarName ("RLeg", "base/data/shapes/player/RLeg.txt");
	serverLoadAvatarName ("LHand", "base/data/shapes/player/LHand.txt");
	serverLoadAvatarName ("RHand", "base/data/shapes/player/RHand.txt");
	serverLoadAvatarName ("LArm", "base/data/shapes/player/LArm.txt");
	serverLoadAvatarName ("RArm", "base/data/shapes/player/RArm.txt");
	serverLoadAvatarName ("Chest", "base/data/shapes/player/Chest.txt");
	serverLoadAvatarName ("Hip", "base/data/shapes/player/Hip.txt");

	$avatarNamesLoaded = 1;
}

function serverLoadAvatarName ( %arrayName, %filename )
{
	%file = new FileObject();
	%file.openForRead (%filename);

	if ( %file.isEOF() )
	{
		%file.delete();
		return;
	}


	%lineCount = 0;

	while ( !%file.isEOF() )
	{
		%line = %file.readLine();

		%command = "$" @  %arrayName  @ "[" @  %lineCount  @ "] = " @  %line  @ ";";
		eval (%command);

		%lineCount++;
	}

	$num[%arrayName] = %lineCount;

	%file.close();
	%file.delete();
}

function serverLoadAccentInfo ( %arrayName, %filename )
{
	%file = new FileObject();
	%file.openForRead (%filename);

	if ( %file.isEOF() )
	{
		%file.delete();
		return;
	}


	%file.readLine();
	%line = %file.readLine();

	%wc = getWordCount (%line);

	for ( %i = 0;  %i < %wc;  %i++ )
	{
		%word = getWord (%line, %i);
		%command = "$" @  %arrayName @  "[" @  %i  @ "] = \"" @  %word  @ "\";";

		eval (%command);
	}


	$num[%arrayName] = %wc;
	%lineCount = 0;

	while ( !%file.isEOF() )
	{
		%line = %file.readLine();

		%wc = getWordCount (%line);
		%hat = getWord (%line, 0);
		%allowed = getWords (%line, 1, %wc - 1);

		%command = "$" @  %arrayName  @ "sAllowed[" @  %hat  @ "] = \"" @  %allowed  @ "\";";
		eval (%command);

		%lineCount++;
	}

	%file.close();
	%file.delete();
}
