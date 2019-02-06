function GameModeGuiServer::populateGameModeList ()
{
	deleteVariables ("$GameModeGuiServer::*");

	$GameModeGuiServer::GameModeCount = 0;

	%pattern = "Add-Ons/GameMode_*/gamemode.txt";
	%filename = findFirstFile (%pattern);

	while ( %filename !$= "" )
	{
		%path = filePath (%filename);
		%previewFile = %path  @ "/preview.jpg";
		%thumbFile = %path  @ "/thumb.jpg";

		%missingAddons = GameModeGuiServer::GetMissingAddOns (%filename);

		$GameModeGuiServer::GameMode[$GameModeGuiServer::GameModeCount] = %filename;
		$GameModeGuiServer::MissingAddOns[$GameModeGuiServer::GameModeCount] = %missingAddons;

		if ( isFile(%previewFile) )
		{
			$GameModeGuiServer::Preview[$GameModeGuiServer::GameModeCount] = %previewFile;
		}
		else
		{
			$GameModeGuiServer::Preview[$GameModeGuiServer::GameModeCount] = "";
		}

		if ( isFile(%thumbFile) )
		{
			$GameModeGuiServer::Thumb[$GameModeGuiServer::GameModeCount] = %thumbFile;
		}
		else
		{
			$GameModeGuiServer::Thumb[$GameModeGuiServer::GameModeCount] = "";
		}

		$GameModeGuiServer::GameModeCount++;

		%filename = findNextFile (%pattern);
	}
}

function dumpGameModeList ()
{
	echo ($GameModeGuiServer::GameModeCount  @ " game modes");

	for ( %i = 0;  %i < $GameModeGuiServer::GameModeCount;  %i++ )
	{
		echo ( %i  @ ": " @  $GameModeGuiServer::GameMode[%i] );
	}
}


function serverCmdGameModeGuiServer_RequestList ( %client )
{
	if ( !%client.isAdmin )
	{
		return;
	}

	if ( $GameModeGuiServer::GameModeCount <= 0 )
	{
		GameModeGuiServer::populateGameModeList();
	}

	for ( %i = 0;  %i < $GameModeGuiServer::GameModeCount;  %i++ )
	{
		%name = filePath ( $GameModeGuiServer::GameMode[%i] );
		%name = strreplace (%name, "Add-Ons/", "");

		%selected = false;

		if ( $GameModeArg $= "" )
		{
			if ( %name $= "Custom"  ||  %name $= "GameMode_Custom" )
			{
				%selected = true;
			}
		}
		else
		{
			if ( $GameModeArg $= $GameModeGuiServer::GameMode[%i] )
			{
				%selected = true;
			}
		}

		commandToClient (%client, 'GameModeGui_AddGameMode', %name, $GameModeGuiServer::MissingAddOns[%i], %selected);
	}

	commandToClient (%client, 'GameModeGui_Done');
}
