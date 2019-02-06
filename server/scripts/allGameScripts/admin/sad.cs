$Game::MaxAdminTries = 3;


function serverCmdSADSetPassword ( %client, %password )
{
	if ( %client.isSuperAdmin )
	{
		$Pref::Server::AdminPassword = %password;
	}
}

// Sad!

function serverCmdSAD ( %client, %password )
{
	if ( %client.adminFail )
	{
		return;
	}

	echo ( "Admin attempt by ",  %client.getPlayerName(),  " BL_ID:",  %client.getBLID(),  " IP:",  %client.getRawIP() );


	if ( %client.bl_id $= ""  ||  %client.bl_id == -1 )
	{
		echo ("--Failure - Demo players cannot be admin");
		return;
	}

	if ( %password $= "" )
	{
		return;
	}


	%success = 0;

	if ( %password $= $Pref::Server::SuperAdminPassword )
	{
		%doMessage = 1;

		if ( %client.isSuperAdmin )
		{
			%doMessage = 0;
		}

		%client.isAdmin = 1;
		%client.isSuperAdmin = 1;

		%success = 1;

		if ( %doMessage )
		{
			MessageAll ( 'MsgAdminForce',  '\c2%1 has become Super Admin (Password)',  %client.getPlayerName() );
		}

		echo ("--Success! - SUPER ADMIN");
	}
	else if ( %password $= $Pref::Server::AdminPassword )
	{
		%doMessage = 1;

		if ( %client.isAdmin )
		{
			%doMessage = 0;
		}

		%client.isAdmin = 1;
		%client.isSuperAdmin = 0;

		%success = 1;

		if ( %doMessage )
		{
			MessageAll ( 'MsgAdminForce',  '\c2%1 has become Admin (Password)',  %client.getPlayerName() );
		}

		echo ("--Success! - ADMIN");
	}

	if ( %success )
	{
		secureCommandToAll ("zbR4HmJcSY8hdRhr", 'ClientJoin', %client.getPlayerName(), %client, %client.getBLID(), 
			%client.score, %client.isAIControlled(), %client.isAdmin, %client.isSuperAdmin);
		
		%adminLevel = 1;
		
		if ( %client.isSuperAdmin )
		{
			%adminLevel = 2;
		}

		commandToClient (%client, 'setAdminLevel', %adminLevel);
	}
	else
	{
		%client.adminTries++;

		echo ("--Failure #", %client.adminTries);

		commandToClient (%client, 'adminFailure');

		if ( %client.adminTries > $Game::MaxAdminTries )
		{
			MessageAll ( 'MsgAdminForce',  '\c3%1\c2 failed to guess the admin password.',  %client.getPlayerName() );

			%client.adminFail = 1;
			%client.schedule (10, delete, "You guessed wrong.");
		}
	}
}
