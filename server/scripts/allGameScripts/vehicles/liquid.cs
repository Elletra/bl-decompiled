function VehicleData::onEnterLiquid ( %data, %obj, %coverage, %type )
{
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

function VehicleData::onLeaveLiquid ( %data, %obj, %type )
{
	if ( isEventPending(%obj.lavaSchedule) )
	{
		cancel (%obj.lavaSchedule);
		%obj.lavaSchedule = 0;
	}
}

function Vehicle::lavaDamage ( %obj, %amt )
{
	%obj.damage (0, %obj.getPosition(), %amt, $DamageType::Lava);

	if ( isEventPending(%obj.lavaSchedule) )
	{
		cancel (%obj.lavaSchedule);
		%obj.lavaSchedule = 0;
	}

	%obj.lavaSchedule = %obj.schedule (300, lavaDamage, %amt);
}
