function SimObject::processInputEvent ( %obj, %EventName, %client )
{
	if ( %obj.numEvents <= 0 )
	{
		return;
	}


	%foundOne = 0;

	for ( %i = 0;  %i < %obj.numEvents;  %i++ )
	{
		if ( %obj.eventInput[%i] $= %EventName  &&  %obj.eventEnabled[%i] )
		{
			%foundOne = 1;
			break;
		}
	}

	if ( !%foundOne )
	{
		return;
	}


	if ( isObject(%client) )
	{
		%quotaObject = getQuotaObjectFromClient (%client);
	}
	else if ( %obj.getType() & $TypeMasks::FxBrickAlwaysObjectType )
	{
		%quotaObject = getQuotaObjectFromBrick (%obj);
	}
	else
	{
		if ( getBuildString() !$= "Ship" )
		{
			error ("ERROR: SimObject::ProcessInputEvent() - could not get quota object for event " @  
				%EventName  @ " on object " @  %obj);
		}

		return;
	}

	if ( !isObject(%quotaObject) )
	{
		error ("ERROR: SimObject::ProcessInputEvent() - new quota object creation failed!");
	}

	setCurrentQuotaObject (%quotaObject);


	if ( %EventName $= "OnRelay"  &&  %obj.implicitCancelEvents )
	{
		%obj.cancelEvents();
	}

	for ( %i = 0;  %i < %obj.numEvents;  %i++ )
	{
		if ( %obj.eventEnabled[%i]  &&  %obj.eventInput[%i] $= %EventName  &&  %obj.eventOutput[%i] $= "CancelEvents" )
		{
			if ( %obj.eventDelay[%i] <= 0 )
			{
				if ( %obj.eventTarget[%i] == -1 )
				{
					%name = %obj.eventNT[%i];
					%group = %obj.getGroup();

					for ( %j = 0;  %j < %group.NTObjectCount[%name];  %j++ )
					{
						%target = %group.NTObject[%name, %j];

						if ( isObject(%target) )
						{
							%target.cancelEvents();
						}
					}
				}
				else
				{
					%target = $InputTarget_[ %obj.eventTarget[%i] ];

					if ( isObject(%target) )
					{
						%target.cancelEvents();
					}
				}
			}
		}
	}

	%eventCount = 0;

	for ( %i = 0;  %i < %obj.numEvents;  %i++ )
	{
		if ( %obj.eventInput[%i] $= %EventName  &&  %obj.eventEnabled[%i] )
		{
			if ( %obj.eventOutput[%i] !$= "CancelEvents"  ||  %obj.eventDelay[%i] > 0 )
			{
				if ( %obj.eventTarget[%i] == -1 )
				{
					%name = %obj.eventNT[%i];
					%group = %obj.getGroup();

					for ( %j = 0;  %j < %group.NTObjectCount[%name];  %j++ )
					{
						%target = %group.NTObject[%name, %j];

						if ( isObject(%target) )
						{
							%eventCount++;
						}
					}
				}
				else
				{
					%eventCount++;
				}
			}
		}
	}

	if ( %eventCount == 0 )
	{
		return;
	}

	%currTime = getSimTime();

	if ( %eventCount > %quotaObject.getAllocs_Schedules() )
	{
		commandToClient (%client, 'centerPrint', "<color:FFFFFF>Too many events at once!\n(" @  %EventName  @ ")", 1);

		if ( %client.SQH_StartTime <= 0 )
		{
			%client.SQH_StartTime = %currTime;
		}
		else
		{
			if ( %currTime - %client.SQH_LastTime < 2000 )
			{
				%client.SQH_HitCount++;
			}

			if ( %client.SQH_HitCount > 5 )
			{
				%client.ClearEventSchedules();
				%client.resetVehicles();

				%mask = $TypeMasks::PlayerObjectType | $TypeMasks::ProjectileObjectType | 
					$TypeMasks::VehicleObjectType | $TypeMasks::CorpseObjectType;

				%client.ClearEventObjects(%mask);
			}
		}

		%client.SQH_LastTime = %currTime;

		return;
	}

	if ( %currTime - %client.SQH_LastTime > 1000 )
	{
		%client.SQH_StartTime = 0;
		%client.SQH_HitCount = 0;
	}


	for ( %i = 0;  %i < %obj.numEvents;  %i++ )
	{
		if ( %obj.eventInput[%i] $= %EventName  &&  %obj.eventEnabled[%i] )
		{
			if ( %obj.eventOutput[%i] !$= "CancelEvents"  ||  %obj.eventDelay[%i] > 0 )
			{
				%delay = %obj.eventDelay[%i];

				%outputEvent = %obj.eventOutput[%i];

				%par1 = %obj.eventOutputParameter[%i, 1];
				%par2 = %obj.eventOutputParameter[%i, 2];
				%par3 = %obj.eventOutputParameter[%i, 3];
				%par4 = %obj.eventOutputParameter[%i, 4];

				%outputEventIdx = %obj.eventOutputIdx[%i];

				if ( %obj.eventTarget[%i] == -1 )
				{
					%name = %obj.eventNT[%i];
					%group = %obj.getGroup();

					for ( %j = 0;  %j < %group.NTObjectCount[%name];  %j++ )
					{
						%target = %group.NTObject[%name, %j];

						if ( isObject(%target) )
						{
							%targetClass = "fxDTSBrick";
							%numParameters = outputEvent_GetNumParametersFromIdx (%targetClass, %outputEventIdx);

							if ( %obj.eventOutputAppendClient[%i] )
							{
								if ( %numParameters == 0 )
								{
									%scheduleID = %target.schedule (%delay, %outputEvent, %client);
								}
								else if ( %numParameters == 1 )
								{
									%scheduleID = %target.schedule (%delay, %outputEvent, %par1, %client);
								}
								else if ( %numParameters == 2 )
								{
									%scheduleID = %target.schedule (%delay, %outputEvent, %par1, %par2, %client);
								}
								else if ( %numParameters == 3 )
								{
									%scheduleID = %target.schedule (%delay, %outputEvent, %par1, %par2, %par3, %client);
								}
								else if ( %numParameters == 4 )
								{
									%scheduleID = %target.schedule (%delay, %outputEvent, %par1, %par2, %par3, %par4, %client);
								}
								else
								{
									error("ERROR: SimObject::ProcessInputEvent() - bad number of parameters on event \'" @  
										%outputEvent  @ "\' (" @  %numParameters  @ ")");
								}
							}
							else if ( %numParameters == 0 )
							{
								%scheduleID = %target.schedule (%delay, %outputEvent);
							}
							else if ( %numParameters == 1 )
							{
								%scheduleID = %target.schedule (%delay, %outputEvent, %par1);
							}
							else if ( %numParameters == 2 )
							{
								%scheduleID = %target.schedule (%delay, %outputEvent, %par1, %par2);
							}
							else if ( %numParameters == 3 )
							{
								%scheduleID = %target.schedule (%delay, %outputEvent, %par1, %par2, %par3);
							}
							else if ( %numParameters == 4 )
							{
								%scheduleID = %target.schedule (%delay, %outputEvent, %par1, %par2, %par3, %par4);
							}
							else
							{
								error ("ERROR: SimObject::ProcessInputEvent() - bad number of parameters on event \'" @ 
									%outputEvent  @ "\' (" @  %numParameters  @ ")");
							}

							if ( %delay > 0 )
							{
								%obj.addScheduledEvent (%scheduleID);
							}
						}
					}
				}
				else
				{
					%target = $InputTarget_[ %obj.eventTarget[%i] ];

					if ( isObject(%target) )
					{
						%targetClass = inputEvent_GetTargetClass ( "fxDTSBrick",  %obj.eventInputIdx[%i],  %obj.eventTargetIdx[%i] );
						%numParameters = outputEvent_GetNumParametersFromIdx (%targetClass, %outputEventIdx);

						if ( %obj.eventOutputAppendClient[%i] )
						{
							if ( %numParameters == 0 )
							{
								%scheduleID = %target.schedule (%delay, %outputEvent, %client);
							}
							else if ( %numParameters == 1 )
							{
								%scheduleID = %target.schedule (%delay, %outputEvent, %par1, %client);
							}
							else if ( %numParameters == 2 )
							{
								%scheduleID = %target.schedule (%delay, %outputEvent, %par1, %par2, %client);
							}
							else if ( %numParameters == 3 )
							{
								%scheduleID = %target.schedule (%delay, %outputEvent, %par1, %par2, %par3, %client);
							}
							else if ( %numParameters == 4 )
							{
								%scheduleID = %target.schedule (%delay, %outputEvent, %par1, %par2, %par3, %par4, %client);
							}
							else
							{
								error ("ERROR: SimObject::ProcessInputEvent() - bad number of parameters on event \'" @ 
									%outputEvent  @ "\' (" @  %numParameters  @ ")");
							}
						}
						else if ( %numParameters == 0 )
						{
							%scheduleID = %target.schedule (%delay, %outputEvent);
						}
						else if ( %numParameters == 1 )
						{
							%scheduleID = %target.schedule (%delay, %outputEvent, %par1);
						}
						else if ( %numParameters == 2 )
						{
							%scheduleID = %target.schedule (%delay, %outputEvent, %par1, %par2);
						}
						else if ( %numParameters == 3 )
						{
							%scheduleID = %target.schedule (%delay, %outputEvent, %par1, %par2, %par3);
						}
						else if ( %numParameters == 4 )
						{
							%scheduleID = %target.schedule (%delay, %outputEvent, %par1, %par2, %par3, %par4);
						}
						else
						{
							error ("ERROR: SimObject::ProcessInputEvent() - bad number of parameters on event \'" @ 
								%outputEvent  @ "\' (" @  %numParameters  @ ")");
						}

						if ( %delay > 0  &&  %EventName !$= "onToolBreak" )
						{
							%obj.addScheduledEvent (%scheduleID);
						}
					}
				}
			}
		}
	}
}
