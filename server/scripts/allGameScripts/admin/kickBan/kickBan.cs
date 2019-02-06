exec ("./addBan.cs");
exec ("./removeBan.cs");
exec ("./kick.cs");


function CreateBanManager ()
{
	if ( isObject($BanManagerSO) )
	{
		$BanManagerSO.delete();
	}

	$BanManagerSO = new ScriptObject (BanManagerSO)
	{
		numBans = 0;
	};

	$BanManagerSO.loadBans();
}


function BanManagerSO::saveBans ( %this )
{
	%file = new FileObject();
	%file.openForWrite ("config/server/BANLIST.txt");

	for ( %i = 0;  %i < %this.numBans;  %i++ )
	{
		%doLine = true;

		if ( %this.expirationMinute > 0 )
		{
			%timeLeft = %this.expirationMinute[%i] - getCurrentMinuteOfYear();
			%timeLeft += ( %this.expirationYear[%i] - getCurrentYear() ) * 525600;

			if ( %timeLeft <= 0 )
			{
				%doLine = false;
			}
		}

		if ( %doLine )
		{
			%line = %this.adminName[%i];
			%line = %line TAB %this.adminBL_ID[%i];

			%line = %line TAB %this.victimName[%i];
			%line = %line TAB %this.victimBL_ID[%i];
			%line = %line TAB %this.victimIP[%i];

			%line = %line TAB %this.Reason[%i];

			%line = %line TAB %this.expirationYear[%i];
			%line = %line TAB %this.expirationMinute[%i];

			%file.writeLine (%line);
		}
	}

	%file.close();
	%file.delete();
}

function BanManagerSO::loadBans ( %this )
{
	%this.numBans = 0;

	%file = new FileObject();
	%file.openForRead ("config/server/BANLIST.txt");

	while ( !%file.isEOF() )
	{
		%line = %file.readLine();

		%i = %this.numBans;

		%this.adminName[%i] = getField (%line, 0);
		%this.adminBL_ID[%i] = getField (%line, 1);

		%this.victimName[%i] = getField (%line, 2);
		%this.victimBL_ID[%i] = getField (%line, 3);
		%this.victimIP[%i] = getField (%line, 4);

		%this.Reason[%i] = getField (%line, 5);

		%this.expirationYear[%i] = getField (%line, 6);
		%this.expirationMinute[%i] = getField (%line, 7);

		%this.numBans++;
	}

	%file.close();
	%file.delete();
}

function BanManagerSO::dumpBans ( %this )
{
	echo (%this.numBans  @ " Bans:");
	echo ("-----------------------------------------------------------------");

	for ( %i = 0;  %i < %this.numBans;  %i++ )
	{
		%line = %this.adminName[%i];
		%line = %line TAB %this.adminBL_ID[%i];

		%line = %line TAB %this.victimName[%i];
		%line = %line TAB %this.victimBL_ID[%i];
		%line = %line TAB %this.victimIP[%i];

		%line = %line TAB %this.Reason[%i];

		%line = %line TAB %this.expirationYear[%i];
		%line = %line TAB %this.expirationMinute[%i];

		echo (%line);
	}

	echo ("-----------------------------------------------------------------");
}

function BanManagerSO::sendBanList ( %this, %client )
{
	for ( %i = 0;  %i < %this.numBans;  %i++ )
	{
		%line = %this.adminName[%i];

		%line = %line TAB %this.victimName[%i];
		%line = %line TAB %this.victimBL_ID[%i];
		%line = %line TAB %this.victimIP[%i];

		%line = %line TAB %this.Reason[%i];


		if ( %this.expirationMinute[%i] == -1 )
		{
			%line = %line TAB -1;
			commandToClient (%client, 'AddUnBanLine', %line, %i);
		}
		else
		{
			%timeLeft = %this.expirationMinute[%i] - getCurrentMinuteOfYear();
			%timeLeft = %timeLeft + ( %this.expirationYear[%i] - getCurrentYear() ) * 525600;

			%line = %line TAB %timeLeft;

			if ( %timeLeft > 0 )
			{
				commandToClient (%client, 'AddUnBanLine', %line, %i);
			}
		}
	}
}

function BanManagerSO::isBanned ( %this, %testBL_ID )
{
	// lol

	if ( %testBL_ID == 0 )
	{
		return;
	}


	for ( %i = 0;  %i < %this.numBans;  %i++ )
	{
		if ( %this.victimBL_ID[%i] == %testBL_ID )
		{
			if ( %this.expirationYear[%i] == -1 )
			{
				return 1 TAB %this.Reason[%i];
			}
			else if ( %this.expirationYear[%i] < getCurrentYear() )
			{
				echo ("BanManagerSO::isBanned() - ban expired last year, removing");

				%this.removeBan (%i);
				%i--;
			}
			else
			{
				if ( %this.expirationYear[%i] == getCurrentYear() )
				{
					if ( %this.expirationMinute[%i] < getCurrentMinuteOfYear() )
					{
						echo ("BanManagerSO::isBanned() - ban expired, removing");

						%this.removeBan (%i);
						%i--;
					}
					else
					{
						return 1 TAB %this.Reason[%i];
					}
				}
				else
				{
					return 1 TAB %this.Reason[%i];
				}
			}
		}
	}

	return 0;
}


function serverCmdRequestBanList ( %client )
{
	if ( !%client.isAdmin )
	{
		return;
	}

	$BanManagerSO.sendBanList (%client);
}
