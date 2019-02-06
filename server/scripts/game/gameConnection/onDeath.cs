function GameConnection::onDeath ( %client, %sourceObject, %sourceClient, %damageType, %damLoc )
{
	if ( %sourceObject.sourceObject.isBot )
	{
		%sourceClientIsBot = 1;
		%sourceClient = %sourceObject.sourceObject;
	}

	%player = %client.Player;

	if ( isObject(%player) )
	{
		%player.setShapeName ("", 8564862);

		if ( isObject(%player.tempBrick) )
		{
			%player.tempBrick.delete();
			%player.tempBrick = 0;
		}

		%player.client = 0;
	}
	else
	{
		warn ("WARNING: No player object in GameConnection::onDeath() for client \'" @  %client  @ "\'");
	}

	if ( isObject(%client.Camera)  &&  isObject(%client.Player) )
	{
		if ( %client.getControlObject() == %client.Camera  &&  %client.Camera.getControlObject() > 0 )
		{
			%client.Camera.setControlObject (%client.dummyCamera);
		}
		else
		{
			%client.Camera.setMode ("Corpse", %client.player);
			%client.setControlObject (%client.Camera);
			%client.Camera.setControlObject (0);
		}
	}


	%client.player = 0;

	if ( $Damage::Direct[%damageType] != 1 )
	{
		if ( getSimTime() - %player.lastDirectDamageTime < 100  &&  %player.lastDirectDamageType !$= "" )
		{
			%damageType = %player.lastDirectDamageType;
		}
	}

	if ( %damageType == $DamageType::Impact )
	{
		if ( isObject(%player.lastPusher)  &&  getSimTime() - %player.lastPushTime <= 1000 )
		{
			%sourceClient = %player.lastPusher;
		}
	}


	%message = '%2 killed %1';

	if ( %sourceClient == %client  ||  %sourceClient == 0 )
	{
		%message = $DeathMessage_Suicide[%damageType];
	}
	else
	{
		%message = $DeathMessage_Murder[%damageType];
	}


	if ( $Damage::Direct[%damageType] == 1  &&  %player.getWaterCoverage() < 0.05 )
	{
		if ( %sourceClient  &&  isObject(%sourceClient.Player) )
		{
			%playerVelocity = VectorLen ( VectorSub ( %player.preHitVelocity,  
				%sourceClient.player.getVelocity() ) ) / 2.64 * 6 * 3600 / 5280;
		}
		else
		{
			%playerVelocity = VectorLen (%player.preHitVelocity) / 2.64 * 6 * 3600 / 5280;
		}

		%playerPos = %player.getPosition();

		%mask = $TypeMasks::StaticShapeObjectType | $TypeMasks::FxBrickObjectType | $TypeMasks::TerrainObjectType;

		%res0 = containerRayCast ( VectorAdd (%playerPos, "0 0 2"),  VectorAdd (%playerPos, "0 0  -6.8"),  %mask );
		%res1 = containerRayCast ( VectorAdd (%playerPos, "0 0 2"),  VectorAdd (%playerPos, "0 -1 -6.8"),  %mask );
		%res2 = containerRayCast ( VectorAdd (%playerPos, "0 0 2"),  VectorAdd (%playerPos, "1 1  -6.8"),  %mask );
		%res3 = containerRayCast ( VectorAdd (%playerPos, "0 0 2"),  VectorAdd (%playerPos, "-1 1 -6.8"),  %mask );

		if ( !isObject ( getWord(%res0, 0) )  &&  !isObject ( getWord(%res1, 0) )  && 
			 !isObject ( getWord(%res2, 0) )  &&  !isObject ( getWord(%res3, 0) ) )
		{
			%range = round ( VectorLen ( VectorSub(%playerPos, %sourceObject.originPoint) ) / 2.65 * 6 );

			if ( isObject(%sourceClient.player) )
			{
				%sourceClient.player.emote (winStarProjectile, 1);
			}

			if ( !%sourceClientIsBot )
			{
				%sourceClient.play2D (rewardSound);

				commandToClient (%sourceClient, 'BottomPrint', "<bitmap:base/client/ui/ci/star>\c3 MID AIR KILL - " @  
					%client.getPlayerName()  @ " " @  round(%playerVelocity)  @ "MPH, " @  %range  @ "ft!", 3);
			}

			commandToClient (%client, 'BottomPrint', "\c5 MID AIR\'d by " @  %sourceClient.getPlayerName()  @ " - " @  
				round(%playerVelocity)  @ "MPH, " @  %range  @ "ft!", 3);
		}
	}

	if ( isObject(%client.miniGame) )
	{
		if ( %sourceClient == %client )
		{
			%client.incScore (%client.miniGame.Points_KillSelf);
		}
		else if ( %sourceClient == 0 )
		{
			%client.incScore (%client.miniGame.Points_Die);
		}
		else
		{
			if ( !%sourceClientIsBot )
			{
				%sourceClient.incScore (%client.miniGame.Points_KillPlayer);
			}

			%client.incScore (%client.miniGame.Points_Die);
		}
	}


	%clientName = %client.getPlayerName();

	if ( isObject(%sourceClient) )
	{
		%sourceClientName = %sourceClient.getPlayerName();
	}
	else if ( isObject(%sourceObject.sourceObject)  &&  %sourceObject.sourceObject.getClassName() $= "AIPlayer" )
	{
		%sourceClientName = %sourceObject.sourceObject.name;
	}
	else
	{
		%sourceClientName = "";
	}


	%mg = %client.miniGame;

	if ( isObject(%mg) )
	{
		%mg.messageAllExcept (%client, 'MsgClientKilled', %message, %client.getPlayerName(), %sourceClientName);
		messageClient (%client, 'MsgYourDeath', %message, %client.getPlayerName(), %sourceClientName, %mg.RespawnTime);

		if ( %mg.respawnTime < 0 )
		{
			commandToClient (%client, 'centerPrint', "", 1);
		}

		%mg.checkLastManStanding();
	}
	else
	{
		messageAllExcept (%client, -1, 'MsgClientKilled', %message, %client.getPlayerName(), %sourceClientName);
		messageClient (%client, 'MsgYourDeath', %message, %client.getPlayerName(), %sourceClientName, $Game::MinRespawnTime);
	}
}
