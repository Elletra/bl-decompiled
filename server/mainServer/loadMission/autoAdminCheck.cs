
function GameConnection::autoAdminCheck ( %client )
{
	%ourBL_ID = %client.getBLID();

	if ( $Pref::Server::AutoAdminServerOwner )
	{
		if ( %ourBL_ID == getNumKeyID() )
		{
			%client.isSuperAdmin = true;
			%client.isAdmin = true;

			return 3;
		}
	}

	%count = getWordCount($Pref::Server::AutoSuperAdminList);

	for ( %i = 0;  %i < %count;  %i++ )
	{
		%checkBL_ID = getWord ($Pref::Server::AutoSuperAdminList, %i);

		if ( %ourBL_ID $= %checkBL_ID )
		{
			%client.isSuperAdmin = true;
			%client.isAdmin = true;

			return 2;
		}
	}


	%count = getWordCount ($Pref::Server::AutoAdminList);

	for ( %i = 0;  %i < %count;  %i++ )
	{
		%checkBL_ID = getWord ($Pref::Server::AutoAdminList, %i);

		if ( %ourBL_ID $= %checkBL_ID )
		{
			%client.isAdmin = true;
			return true;
		}
	}

	return false;
}
