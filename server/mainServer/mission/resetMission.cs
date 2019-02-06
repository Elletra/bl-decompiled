function resetMission ()
{
	echo ("*** MISSION RESET");

	MissionCleanup.deleteAll();
	MissionCleanup.delete();

	$instantGroup = ServerGroup;

	new SimGroup (MissionCleanup);

	$instantGroup = MissionCleanup;

	if ( isObject(GlobalQuota) )
	{
		GlobalQuota.delete();
	}

	new QuotaObject (GlobalQuota)
	{
		AutoDelete = 0;
	};

	GlobalQuota.setAllocs_Schedules (9999, 5465489);
	GlobalQuota.setAllocs_Misc (9999, 5465489);
	GlobalQuota.setAllocs_Projectile (9999, 5465489);
	GlobalQuota.setAllocs_Item (9999, 5465489);
	GlobalQuota.setAllocs_Environment (9999, 5465489);
	GlobalQuota.setAllocs_Player (9999, 5465489);
	GlobalQuota.setAllocs_Vehicle (9999, 5465489);

	ServerGroup.add (GlobalQuota);

	if ( !isObject(QuotaGroup) )
	{
		new SimGroup (QuotaGroup);
		RootGroup.add (QuotaGroup);
	}

	onMissionReset();
}
