function SimGroup::chainDeleteAll ( %group )
{
	if ( isEventPending(%group.chainDeleteSchedule) )
	{
		cancel (%group.chainDeleteSchedule);
	}

	if ( %group.NTNameCount > 0 )
	{
		%group.clearAllNTNames();
	}

	%count = %group.getCount();

	if ( %count > 0 )
	{
		%obj = %group.getObject (0);

		if ( %obj.getClassName() $= "SimGroup" )
		{
			if ( %obj.getCount() > 0 )
			{
				%obj.chainDeleteCallBack = %group  @ ".schedule(0, chainDeleteAll);";
				%obj.chainDeleteAll();

				return;
			}
		}

		%obj.delete();
	}
	else
	{
		%group.chainDeleteSchedule = 0;
		%oldQuotaObject = getCurrentQuotaObject();

		if ( isObject(%oldQuotaObject) )
		{
			clearCurrentQuotaObject();
		}

		eval (%group.chainDeleteCallBack);

		if ( isObject(%oldQuotaObject) )
		{
			setCurrentQuotaObject (%oldQuotaObject);
		}

		%group.chainDeleteCallBack = "";

		return;
	}

	%oldQuotaObject = getCurrentQuotaObject();

	if ( isObject(%oldQuotaObject) )
	{
		clearCurrentQuotaObject();
	}

	%group.chainDeleteSchedule = %group.schedule (0, chainDeleteAll);

	if ( isObject(%oldQuotaObject) )
	{
		setCurrentQuotaObject (%oldQuotaObject);
	}
}
