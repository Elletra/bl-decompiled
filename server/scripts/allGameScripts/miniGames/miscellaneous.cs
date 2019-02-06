function MiniGameSO::timeLimitTick ( %obj, %echo )
{
	%elapsedTime = getSimTime() - %obj.lastResetTime;
	%timeRemaining = %obj.timeLimit * 1000 - %elapsedTime;

	cancel (%obj.timeLimitSchedule);

	%obj.timeLimitSchedule = 0;

	if ( %timeRemaining <= 10 )
	{
		%obj.chatMessageAll (0, '\c6Time\'s up.  No one wins.');
		%obj.scheduleReset();

		return;
	}
	else if ( %timeRemaining <= 10000 )
	{
		%obj.timeLimitSchedule = %obj.schedule (%timeRemaining, timeLimitTick, 1);
	}
	else if ( %timeRemaining <= 30000 )
	{
		%obj.timeLimitSchedule = %obj.schedule (%timeRemaining - 10000, timeLimitTick, 1);
	}
	else if ( %timeRemaining <= 60000 )
	{
		%obj.timeLimitSchedule = %obj.schedule (%timeRemaining - 30000, timeLimitTick, 1);
	}
	else
	{
		%obj.timeLimitSchedule = %obj.schedule (%timeRemaining - 60000, timeLimitTick, 1);
	}

	if ( %echo )
	{
		%secondsRemaining = mFloor (%timeRemaining / 1000 + 0.5);
		%obj.chatMessageAll (0, "\c6" @  getTimeString(%secondsRemaining)  @ " remaining");
	}
}

function MiniGameSO::respawnAll ( %obj, %client )
{
	if ( %client !$= "" )
	{
		if ( %client.miniGame != %obj )
		{
			return;
		}

		if ( isObject ( $InputTarget_["Self"] ) )
		{
			if ( $InputTarget_["Self"].getType() & $TypeMasks::FxBrickAlwaysObjectType )
			{
				if ( $InputTarget_["Self"].getGroup() != %obj.owner.brickGroup )
				{
					return;
				}
			}
			else if ( %client != %obj.owner )
			{
				return;
			}
		}
		else if ( %client != %obj.owner )
		{
			return;
		}
	}

	for ( %i = 0;  %i < %obj.numMembers;  %i++ )
	{
		%cl = %obj.member[%i];
		%cl.instantRespawn();
	}
}

function MiniGameSO::forceEquip ( %obj, %slot )
{
	for ( %i = 0;  %i < %obj.numMembers;  %i++ )
	{
		%cl = %obj.member[%i];

		if ( isObject(%player = %cl.player)  &&  %player.tool[%slot] != %obj.startEquip[%slot] )
		{
			%player.tool[%slot] = %obj.startEquip[%slot];
			messageClient (%cl, 'MsgItemPickup', "", %slot, %player.tool[%slot], 1);

			if ( %player.currTool == %slot )
			{
				if ( %player.getMountedImage(0) > 0  &&  %player.getMountedImage(0) != brickImage.getId() )
				{
					if ( isObject ( %obj.startEquip[%slot] ) )
					{
						%obj.startEquip[%slot].onUse (%player, %slot);
					}
					else
					{
						%player.unmountImage(0);
					}
				}
			}
		}
	}
}
