exec ("./playerEmote.cs");

exec ("./bsd.cs");
exec ("./painLow.cs");
exec ("./painMid.cs");
exec ("./painHigh.cs");
exec ("./win.cs");


function serverCmdSit ( %client )
{
	%player = %client.player;

	if ( !isObject(%player) )
	{
		return;
	}

	if ( %player.isMounted() )
	{
		return;
	}

	if ( %player.getDamagePercent() < 1.0 )
	{
		%player.setActionThread (sit, 1);
	}
}

function serverCmdHug ( %client )
{
	if ( isObject(%client.player) )
	{
		%client.player.playThread (1, armReadyBoth);
	}
}

function serverCmdZombie ( %client )
{
	if ( isObject(%client.player) )
	{
		%client.player.playThread (1, armReadyBoth);
	}
}
