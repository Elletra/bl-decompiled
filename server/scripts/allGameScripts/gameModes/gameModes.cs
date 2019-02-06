exec ("./gameModeList.cs");
exec ("./parseGameModeFile.cs");
exec ("./changeGameMode.cs");

exec ("./getMissingAddOns.cs");
exec ("./addOnList.cs");
exec ("./musicList.cs");
exec ("./customGameModeList.cs");

exec ("./setPref.cs");


function GameModeInitialResetCheck ()
{
	if ( isEventPending($GameModeInitialResetCheckEvent) )
	{
		cancel ($GameModeInitialResetCheckEvent);
		$GameModeInitialResetCheckEvent = 0;
	}

	if ( isFunction("loadAllClientPersistence") )
	{
		loadAllClientPersistence();
	}
}
