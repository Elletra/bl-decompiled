function portInit ( %port )
{
	if ( %port == 280000 )
	{
		%port = 28000;
	}
    else if ( %port == 280001 )
	{
		%port = 28001;
	}

	%port = mClampF (%port, 0, 65535);

	%failCount = 0;

	while ( %failCount < 10  &&  !setNetPort(%port) )
	{
		echo ("Port init failed on port " @  %port  @ " trying next port.");

		%port++;
		%failCount++;
	}

	$Server::Port = %port;
}

function onUPnPFailure ( %errorCode )
{
	$pref::client::lastUpnpError = %errorCode;

	if ( %errorCode == 718  && $Server::Port == 28000 )
	{
		$pref::client::lastUpnpError = 0;

		$Pref::Server::Port = 28100;
		$Server::Port = 28100;

		portInit ($Pref::Server::Port);
		upnpAdd ($Server::Port);
	}
}

function onUPnPDiscoveryFailed ()
{
	$pref::client::lastUpnpError = -999;
}
