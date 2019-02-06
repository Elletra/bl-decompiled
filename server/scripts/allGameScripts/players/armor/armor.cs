exec ("./damage.cs");

exec ("./onAddRemove.cs");
exec ("./onCollision.cs");
exec ("./onDisabled.cs");
exec ("./onImpact.cs");
exec ("./onNewDataBlock.cs");


function Armor::onTrigger ( %this, %obj, %triggerNum, %val )
{
	if ( %triggerNum == 0  &&  %val)
	{
		if ( getSimTime() - %obj.spawnTime < 100 )
		{
			return;
		}

		if ( %obj.getMountedImage(0) <= 0 )
		{
			%obj.activateStuff();
		}
	}
}

function Armor::onEnterMissionArea ( %this, %obj )
{
	if ( isObject(%obj.client) )
	{
		%obj.client.onEnterMissionArea();
	}
}

function Armor::onLeaveMissionArea ( %this, %obj )
{
	if ( isObject(%obj.client) )
	{
		%obj.client.onLeaveMissionArea();
	}
}

function Armor::onEnterLiquid ( %this, %obj, %coverage, %type )
{
	if ( %obj.getMountedImage (3) == PlayerBurnImage.getId() )
	{
		%obj.unmountImage (3);
	}

	if ( %type == 8 )
	{
		if ( isEventPending(%obj.lavaSchedule) )
		{
			cancel (%obj.lavaSchedule);
			%obj.lavaSchedule = 0;
		}

		%obj.lavaDamage (20);
	}
}

function Armor::onLeaveLiquid ( %this, %obj, %type )
{
	if ( isEventPending(%obj.lavaSchedule) )
	{
		cancel (%obj.lavaSchedule);
		%obj.lavaSchedule = 0;
	}
}

function Armor::onStuck ( %this, %obj )
{
	// Your code here
}

function Armor::onReachDestination ( %this, %obj )
{
	// Your code here
}

function Armor::onTargetEnterLOS ( %this, %obj )
{
	// Your code here
}

function Armor::onTargetExitLOS ( %this, %obj )
{
	// Your code here
}
