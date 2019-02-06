exec ("./auth_Init_Server.cs");
exec ("./onLine.cs");


function authTCPobj_Server::onDNSFailed ( %this )
{
	echo ("Authentication FAILED: DNS error.");

	%this.schedule (0, disconnect);
	schedule (5000, 0, "auth_Init_Server");
}

function authTCPobj_Server::onConnectFailed ( %this )
{
	echo("Authentication FAILED: Connection failure.  Retrying in 5 seconds...");

	%this.schedule (0, disconnect);
	schedule (5000, 0, "auth_Init_Server");
}

function authTCPobj_Server::onConnected ( %this )
{
	%this.send (%this.cmd);
	echo ("Authentication: Connected...");
}

function authTCPobj_Server::onDisconnect ( %this )
{
	// Your code here
}
