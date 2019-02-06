exec ("./createdDestroyed.cs");
exec ("./validatePrefs.cs");
exec ("./serverVars.cs");
exec ("./startEndGame.cs");


function isListenServer ()
{
	if ( !isObject(ServerConnection) )
	{
		return false;
	}

	if ( ServerConnection.isLocal() )
	{
		return true;
	}

	return false;
}
