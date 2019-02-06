exec ("./onPlayerTouch.cs");


function fxDTSBrick::onActivate ( %obj, %player, %client, %pos, %vec )
{
	$InputTarget_["Self"] = %obj;
	$InputTarget_["Player"] = %player;
	$InputTarget_["Client"] = %client;

	if ( $Server::LAN )
	{
		$InputTarget_["MiniGame"] = getMiniGameFromObject (%client);
	}
	else if ( getMiniGameFromObject(%obj) == getMiniGameFromObject(%client) )
	{
		$InputTarget_["MiniGame"] = getMiniGameFromObject (%obj);
	}
	else
	{
		$InputTarget_["MiniGame"] = 0;
	}

	%obj.processInputEvent ("OnActivate", %client);
}

function fxDTSBrick::onBlownUp ( %obj, %client, %player )
{
	$InputTarget_["Self"] = %obj;
	$InputTarget_["Player"] = %player;
	$InputTarget_["Client"] = %client;

	if ( $Server::LAN )
	{
		$InputTarget_["MiniGame"] = getMiniGameFromObject (%client);
	}
	else if ( getMiniGameFromObject(%obj) == getMiniGameFromObject (%client) )
	{
		$InputTarget_["MiniGame"] = getMiniGameFromObject (%obj);
	}
	else
	{
		$InputTarget_["MiniGame"] = 0;
	}

	%obj.destroyingClient = %client;
	%obj.destroyingPlayer = %player;

	%obj.processInputEvent ("OnBlownUp", %client);
}

function fxDTSBrick::onRespawn ( %obj, %client, %player )
{
	if ( %obj.eventFloodCheck(1) )
	{
		return;
	}

	$InputTarget_["Self"] = %obj;
	%obj.processInputEvent ("OnRespawn", %client);
}

function fxDTSBrick::onRelay ( %obj, %client )
{
	$InputTarget_["Self"] = %obj;
	%obj.processInputEvent ("OnRelay", %client);
}

function fxDTSBrick::onProjectileHit ( %obj, %projectile, %client )
{
	if ( %obj.numEvents <= 0 )
	{
		return;
	}

	if ( %obj.eventFloodCheck(2) )
	{
		return;
	}

	if ( isObject(%client) )
	{
		if ( %client.eventFloodCheck(5) )
		{
			return;
		}
	}

	if ( isObject(%projectile.sourceObject) )
	{
		%player = %projectile.sourceObject;
	}


	$InputTarget_["Self"] = %obj;
	$InputTarget_["Player"] = %player;

	if ( isObject(%player) )
	{
		$InputTarget_["Client"] = %player.client;
	}
	else
	{
		$InputTarget_["Client"] = "";
	}


	$InputTarget_["Projectile"] = %projectile;

	if ( $Server::LAN )
	{
		$InputTarget_["MiniGame"] = getMiniGameFromObject (%client);
	}
	else if ( getMiniGameFromObject(%obj) == getMiniGameFromObject(%client) )
	{
		$InputTarget_["MiniGame"] = getMiniGameFromObject (%obj);
	}
	else
	{
		$InputTarget_["MiniGame"] = 0;
	}

	%obj.processInputEvent ("onProjectileHit", %client);
}

function fxDTSBrick::onPrintCountOverFlow ( %obj, %client )
{
	if ( %obj.eventFloodCheck(4) )
	{
		return;
	}

	$InputTarget_["Self"] = %obj;
	$InputTarget_["Client"] = %client;

	%obj.processInputEvent ("onPrintCountOverFlow", %client);
}

function fxDTSBrick::onPrintCountUnderFlow ( %obj, %client )
{
	if ( %obj.eventFloodCheck(4) )
	{
		return;
	}

	$InputTarget_["Self"] = %obj;
	$InputTarget_["Client"] = %client;

	%obj.processInputEvent ("onPrintCountUnderFlow", %client);
}

function fxDTSBrick::onToolBreak ( %obj, %client )
{
	$InputTarget_["Self"] = %obj;
	$InputTarget_["Player"] = %client.Player;
	$InputTarget_["Client"] = %client;

	if ( $Server::LAN )
	{
		$InputTarget_["MiniGame"] = getMiniGameFromObject (%client);
	}
	else if ( getMiniGameFromObject(%obj) == getMiniGameFromObject(%client) )
	{
		$InputTarget_["MiniGame"] = getMiniGameFromObject (%obj);
	}
	else
	{
		$InputTarget_["MiniGame"] = 0;
	}

	%obj.processInputEvent ("OnToolBreak", %client);
}

function fxDTSBrick::onMiniGameReset ( %obj, %client )
{
	$InputTarget_["Self"] = %obj;

	if ( isObject(%client) )
	{
		$InputTarget_["Player"] = %client.Player;
		$InputTarget_["Client"] = %client;
	}
	else
	{
		$InputTarget_["Player"] = 0;
		$InputTarget_["Client"] = 0;
	}

	$InputTarget_["MiniGame"] = getMiniGameFromObject (%obj);

	%obj.processInputEvent ("OnMiniGameReset", %client);
}


// =================
//  Register Events
// =================

registerInputEvent ("fxDTSBrick", "onActivate", "Self fxDTSBrick" TAB "Player Player" TAB 
	"Client GameConnection" TAB "MiniGame MiniGame");

registerInputEvent ("fxDTSBrick", "onBotTouch", "Self fxDTSBrick" TAB "Bot Bot" TAB 
	"Driver Player" TAB "Client GameConnection" TAB "MiniGame MiniGame");

registerInputEvent ("fxDTSBrick", "onProjectileHit", "Self fxDTSBrick" TAB "Player Player" TAB 
	"Client GameConnection" TAB "Projectile Projectile" TAB "MiniGame MiniGame");

registerInputEvent ("fxDTSBrick", "onBlownUp", "Self fxDTSBrick" TAB "Player Player" TAB 
	"Client GameConnection" TAB "MiniGame MiniGame");

registerInputEvent ("fxDTSBrick", "onRespawn", "Self fxDTSBrick");
registerInputEvent ("fxDTSBrick", "onRelay", "Self fxDTSBrick");
registerInputEvent ("fxDTSBrick", "onPrintCountOverFlow", "Self fxDTSBrick" TAB "Client GameConnection");
registerInputEvent ("fxDTSBrick", "onPrintCountUnderFlow", "Self fxDTSBrick" TAB "Client GameConnection");
registerInputEvent ("fxDTSBrick", "onMiniGameReset", "Self fxDTSBrick" TAB "MiniGame MiniGame", true);
