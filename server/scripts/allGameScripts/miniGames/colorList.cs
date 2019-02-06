function initMinigameColors ()
{
	%i = -1;

	$MiniGameColorName[%i++] = "Red";
	$MiniGameColorI[%i] = "255 0 0";
	$MiniGameColorH[%i] = "#FF0000";

	$MiniGameColorName[%i++] = "Orange";
	$MiniGameColorI[%i] = "255 128 0";
	$MiniGameColorH[%i] = "#FF8800";

	$MiniGameColorName[%i++] = "Yellow";
	$MiniGameColorI[%i] = "255 255 0";
	$MiniGameColorH[%i] = "#FFFF00";

	$MiniGameColorName[%i++] = "Green";
	$MiniGameColorI[%i] = "0 255 0";
	$MiniGameColorH[%i] = "#00FF00";

	$MiniGameColorName[%i++] = "Dark Green";
	$MiniGameColorI[%i] = "0 128 0";
	$MiniGameColorH[%i] = "#008800";

	$MiniGameColorName[%i++] = "Cyan";
	$MiniGameColorI[%i] = "0 255 255";
	$MiniGameColorH[%i] = "#00FFFF";

	$MiniGameColorName[%i++] = "Dark Cyan";
	$MiniGameColorI[%i] = "0 128 128";
	$MiniGameColorH[%i] = "#008888";

	$MiniGameColorName[%i++] = "Blue";
	$MiniGameColorI[%i] = "0 128 255";
	$MiniGameColorH[%i] = "#0088FF";

	$MiniGameColorName[%i++] = "Pink";
	$MiniGameColorI[%i] = "255 128 255";
	$MiniGameColorH[%i] = "#FF88FF";

	$MiniGameColorName[%i++] = "Black";
	$MiniGameColorI[%i] = "0 0 0";
	$MiniGameColorH[%i] = "#000000";

	$MiniGameColorCount = %i + 1;

	for ( %i = 0;  %i < $MiniGameColorCount;  %i++ )
	{
		$MiniGameColorF[%i] = getColorF ( $MiniGameColorI[%i] );
		$MiniGameColorTaken[%i] = 0;
	}
}

function serverCmdRequestMiniGameColorList ( %client )
{
	for ( %i = 0;  %i < $MiniGameColorCount;  %i++ )
	{
		if ( $MiniGameColorTaken[%i] == 0 )
		{
			commandToClient ( %client, 'AddMiniGameColor', %i, $MiniGameColorName[%i], 
				$MiniGameColorI[%i] );
		}
	}
}
