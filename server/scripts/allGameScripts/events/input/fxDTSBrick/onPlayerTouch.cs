$Game::OnTouchImmuneTime = 2000;


function fxDTSBrick::onPlayerTouch ( %obj, %player )
{
	%data = %obj.getDataBlock();
	%data.onPlayerTouch (%obj, %player);
}

function fxDTSBrickData::onPlayerTouch ( %data, %obj, %player )
{
	if ( getSimTime() - %player.spawnTime < $Game::OnTouchImmuneTime )
	{
		return;
	}

	if ( %obj.numEvents <= 0 )
	{
		return;
	}


	%image = %player.getMountedImage (0);

	if ( %image )
	{
		if ( %image.getId() == AdminWandImage.getId() )
		{
			return;
		}
	}


	%client = %player.client;

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


	%isVehicle = 0;
	$InputTarget_["Driver"] = 0;

	if ( !isObject(%client) )
	{
		%isVehicle = 1;
		$InputTarget_["Bot"] = %player;

		if ( isObject ( %player.getMountedObject(0) ) )
		{
			$InputTarget_["Driver"] = %player.getMountedObject (0);
		}

		if ( %obj.getGroup().blid != 888888 )
		{
			if ( $Server::LAN )
			{
				if ( isObject(%player.spawnBrick) )
				{
					%client = %player.spawnBrick.client;
				}

				if ( !isObject(%client) )
				{
					%client = %player.getControllingClient();
				}

				if ( !isObject(%client) )
				{
					%client = ClientGroup.getObject (0);
				}
			}
			else
			{
				if ( isObject(%player.spawnBrick) )
				{
					%client = findClientByBL_ID (%player.spawnBrick.getGroup().bl_id);
				}

				if ( !isObject(%client) )
				{
					%client = %player.lastControllingClient;
				}
			}
		}
	}


	if ( isObject(%client)  ||  %obj.getGroup().bl_id == 888888 )
	{
		if ( %isVehicle )
		{
			%obj.processInputEvent ("OnBotTouch", %client);
		}
		else
		{
			%obj.processInputEvent ("OnPlayerTouch", %client);
		}
	}
}


// =================
//  Register Events
// =================

registerInputEvent ("fxDTSBrick", "onPlayerTouch", "Self fxDTSBrick" TAB "Player Player" TAB 
	"Client GameConnection" TAB "MiniGame MiniGame");
