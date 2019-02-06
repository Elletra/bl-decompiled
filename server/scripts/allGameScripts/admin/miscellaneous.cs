function serverCmdTimeScale ( %client, %val )
{
	if ( !%client.isAdmin )
	{
		return;
	}

	%val = mClampF (%val, 0.2, 2);
	setTimeScale (%val);

	MessageAll ('',  %client.getPlayerName()  @ " changed the timescale to " @  %val);

	WebCom_PostServerUpdateLoop();
	pingMatchMakerLoop();
}

function serverCmdColorTest ( %client )
{
	if ( !%client.isAdmin )
	{
		return;
	}

	MessageAll ('', '\c00\c11\c22\c33\c44\c55\c66\c77\c88');
}

function serverCmdDPB ( %client )
{
	if ( !%client.isAdmin )
	{
		return;
	}


	%client.destroyPublicBricks = !%client.destroyPublicBricks;

	if ( %client.destroyPublicBricks )
	{
		%client.bottomPrint ("\c6Destroy public bricks is \c0ON", 4);
	}
	else
	{
		%client.bottomPrint ("\c6Destroy public bricks is \c2OFF", 4);
	}
}
