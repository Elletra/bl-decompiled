$CorpseTimeoutValue = 5 * 1000;


function Armor::onDisabled ( %this, %obj, %state )
{
	%obj.playDeathCry();
	%obj.playDeathAnimation();

	%obj.setDamageFlash (0.75);

	if ( isEventPending(%obj.lavaSchedule) )
	{
		cancel (%obj.lavaSchedule);
		%obj.lavaSchedule = 0;
	}


	if ( %obj.getObjectMount() )
	{
		%vehicle = %obj.getObjectMount();
		%mask = $TypeMasks::PlayerObjectType | $TypeMasks::VehicleObjectType;

		if ( %vehicle.getType() & %mask )
		{
			%vehicle.onDriverLeave();
		}

		%obj.unmount();
	}


	%obj.setImageTrigger (0, 0);

	if ( isObject(%obj.tempBrick) )
	{
		%obj.tempBrick.delete();
		%obj.tempBrick = 0;
	}

	%client = %obj.client;
	%client.deathTime = getSimTime();

	%oldQuota = getCurrentQuotaObject();

	if ( isObject(%oldQuota) )
	{
		clearCurrentQuotaObject();
	}

	%obj.schedule ($CorpseTimeoutValue, RemoveBody);

	%count = %obj.getMountedObjectCount();

	for ( %i = 0;  %i < %count;  %i++ )
	{
		%rider = %obj.getMountedObject (0);
		%rider.getDataBlock().doDismount (%rider, 1);
	}

	if ( isObject(%oldQuota) )
	{
		setCurrentQuotaObject (%oldQuota);
	}
}
