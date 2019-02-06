function MiniGameSO::endGame ( %obj )
{
	%obj.ending = true;

	for ( %i = 0;  %i < %obj.numMembers;  %i++ )
	{
		%cl = %obj.member[%i];
		%cl.miniGame = 0;

		%player = %cl.Player;

		if ( isObject(%player) )
		{
			%player.setShapeNameColor ("1 1 1");
		}

		commandToClient (%cl, 'SetPlayingMiniGame', 0);

		if ( %obj.owner == %cl )
		{
			commandToClient (%cl, 'SetRunningMiniGame', 0);
		}

		messageClient (%cl, '', '\c5The mini-game ended.');

		%cl.setScore (0);

		if ( %cl == %obj.owner )
		{
			if ( isObject(%cl.Player) )
			{
				%cl.Player.GiveDefaultEquipment();

				if ( %player.currTool > -1 )
				{
					%item = %player.tool[%player.currTool];

					if ( isObject(%item) )
					{
						%item.onUse (%player, %player.currTool);
					}
					else
					{
						%player.unmountImage (0);
					}
				}

				if ( %cl.Player.getDamagePercent() >= 1 )
				{
					%cl.miniGame = %obj;
					%cl.InstantRespawn();
					%cl.miniGame = 0;
				}
				else
				{
					%cl.Player.setDamageLevel (0);

					if ( isObject ( %cl.Player.getMountedImage(0) ) )
					{
						if ( %cl.Player.getMountedImage(0).getId() == %cl.Player.getDataBlock().brickImage.getId() )
						{
							%cl.Player.mountImage (PlayerStandardArmor.brickImage, 0);
						}
					}

					%cl.Player.setDatablock (PlayerStandardArmor);
					commandToClient (%cl, 'ShowEnergyBar', PlayerStandardArmor.showEnergyBar);
					fixArmReady (%player);
					applyCharacterPrefs (%cl);
				}
			}
			else
			{
				%cl.miniGame = %obj;
				%cl.InstantRespawn();
				%cl.miniGame = 0;
			}
		}
		else
		{
			%cl.InstantRespawn();
		}
	}

	$MiniGameColorTaken[%obj.colorIdx] = false;
	commandToAll ('RemoveMiniGameLine', %obj);
}


function serverCmdEndMiniGame ( %client )
{
	if ( isObject(%client.miniGame) )
	{
		if ( %client.miniGame.owner == %client )
		{
			%mg = %client.miniGame;
			%mg.endGame();
			%mg.delete();
		}
		else
		{
			error ("ERROR: serverCmdEndMiniGame() - " @  %client.getPlayerName()  @ 
				" tried to end a minigame that he\'s not in charge of.");             // "HE"?!?!?!?!?!?!
		}
	}
	else
	{
		error ("ERROR: serverCmdEndMiniGame() - " @  %client.getPlayerName()  @ 
			" tried to end a minigame when he\'s not even in one.");
	}
}

function endAllMinigames()
{
	%count = ClientGroup.getCount();

	for ( %i = 0;  %i < %count;  %i++ )
	{
		%cl = ClientGroup.getObject (%i);

		if ( isObject(%cl.miniGame) )
		{
			if ( %cl.miniGame.owner == %cl )
			{
				%cl.miniGame.endGame();
			}
		}
	}
}
