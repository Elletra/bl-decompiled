function registerInputEvent(%class, %name, %targetList)
{
	$InputEvent_Count[%class] = mFloor($InputEvent_Count[%class]);
	for (%i = 0; %i < $InputEvent_Count[%class]; %i++)
	{
		if ($InputEvent_Name[%class, %i] $= %name)
		{
			$InputEvent_Name[%class, %i] = %name;
			$InputEvent_TargetList[%class, %i] = %targetList;
			return;
		}
	}
	%i = mFloor($InputEvent_Count[%class]);
	$InputEvent_Name[%class, %i] = %name;
	$InputEvent_TargetList[%class, %i] = %targetList;
	$InputEvent_Count[%class]++;
	if (strstr($InputEvent_ClassList, %class) == -1)
	{
		if ($InputEvent_ClassList $= "")
		{
			$InputEvent_ClassList = %class;
		}
		else
		{
			$InputEvent_ClassList = $InputEvent_ClassList SPC %class;
		}
	}
}

function unRegisterInputEvent(%class, %name)
{
	%count = $InputEvent_Count[%class];
	for (%i = 0; %i < %count; %i++)
	{
		if ($InputEvent_Name[%class, %i] $= %name)
		{
			for (%j = %i + 1; %j < %count; %j++)
			{
				$InputEvent_Name[%class, %j - 1] = $InputEvent_Name[%class, %j];
				$InputEvent_TargetList[%class, %j - 1] = $InputEvent_TargetList[%class, %j];
			}
			$InputEvent_Name[%class, %count - 1] = "";
			$InputEvent_TargetList[%class, %count - 1] = "";
			$InputEvent_Count[%class]--;
			echo("Un-registering input event \"" @ %name @ "\" from class \"" @ %class @ "\"");
			return;
		}
	}
	error("ERROR: unRegisterInputEvent() - event \"" @ %name @ "\" is not registered on class \"" @ %class @ "\"");
}

function inputEvent_GetInputEventIdx(%inputEventName)
{
	%class = "fxDTSBrick";
	for (%x = 0; %x < $InputEvent_Count[%class]; %x++)
	{
		if ($InputEvent_Name[%class, %x] $= %inputEventName)
		{
			return %x;
		}
	}
	return -1;
}

function inputEvent_GetTargetIndex(%class, %i, %targetName)
{
	%fieldCount = getFieldCount($InputEvent_TargetList[%class, %i]);
	for (%x = 0; %x < %fieldCount; %x++)
	{
		%field = getField($InputEvent_TargetList[%class, %i], %x);
		%name = getWord(%field, 0);
		if (%name $= %targetName)
		{
			return %x;
		}
	}
	return -1;
}

function inputEvent_GetTargetClass(%class, %idx, %targetIdx)
{
	%targetList = $InputEvent_TargetList[%class, %idx];
	%field = getField(%targetList, %targetIdx);
	%class = getWord(%field, 1);
	return %class;
}

function inputEvent_GetTargetName(%class, %idx, %targetIdx)
{
	%targetList = $InputEvent_TargetList[%class, %idx];
	%field = getField(%targetList, %targetIdx);
	%name = getWord(%field, 0);
	return %name;
}

function dumpInputEvents(%class)
{
	%count = $InputEvent_Count[%class];
	echo("Class \"" @ %class @ "\" has " @ %count @ " registered input events");
	for (%i = 0; %i < %count; %i++)
	{
		echo("  " @ %i @ ": " @ $InputEvent_Name[%class, %i]);
		%targetCount = getFieldCount($InputEvent_TargetList[%class, %i]);
		for (%j = 0; %j < %targetCount; %j++)
		{
			echo("    " @ getField($InputEvent_TargetList[%class, %i], %j));
		}
	}
}

function SimObject::ProcessInputEvent(%obj, %EventName, %client)
{
	if (%obj.numEvents <= 0)
	{
		return;
	}
	if (!isObject(%client))
	{
		if (getBuildString() !$= "Ship")
		{
			error("ERROR: SimObject::ProcessInputEvent() - no client for event \"" @ %EventName @ "\"");
		}
		return;
	}
	%quotaObject = getQuotaObjectFromClient(%client);
	if (!isObject(%quotaObject))
	{
		error("ERROR: SimObject::ProcessInputEvent() - new quota object creation failed!");
	}
	setCurrentQuotaObject(%quotaObject);
	if (%EventName $= "OnRelay")
	{
		if (%obj.implicitCancelEvents)
		{
			%obj.cancelEvents();
		}
	}
	for (%i = 0; %i < %obj.numEvents; %i++)
	{
		if (!%obj.eventEnabled[%i])
		{
		}
		else if (%obj.eventInput[%i] !$= %EventName)
		{
		}
		else if (%obj.eventOutput[%i] !$= "CancelEvents")
		{
		}
		else if (%obj.eventDelay[%i] > 0)
		{
		}
		else if (%obj.eventTarget[%i] == -1)
		{
			%name = %obj.eventNT[%i];
			%group = %obj.getGroup();
			for (%j = 0; %j < %group.NTObjectCount[%name]; %j++)
			{
				%target = %group.NTObject[%name, %j];
				if (!isObject(%target))
				{
				}
				else
				{
					%target.cancelEvents();
				}
			}
		}
		else
		{
			%target = $InputTarget_[%obj.eventTarget[%i]];
			if (!isObject(%target))
			{
			}
			else
			{
				%target.cancelEvents();
			}
		}
	}
	%eventCount = 0;
	for (%i = 0; %i < %obj.numEvents; %i++)
	{
		if (%obj.eventInput[%i] !$= %EventName)
		{
		}
		else if (!%obj.eventEnabled[%i])
		{
		}
		else if (%obj.eventOutput[%i] $= "CancelEvents" && %obj.eventDelay[%i] == 0)
		{
		}
		else if (%obj.eventTarget[%i] == -1)
		{
			%name = %obj.eventNT[%i];
			%group = %obj.getGroup();
			for (%j = 0; %j < %group.NTObjectCount[%name]; %j++)
			{
				%target = %group.NTObject[%name, %j];
				if (!isObject(%target))
				{
				}
				else
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
	if (%eventCount == 0)
	{
		return;
	}
	%currTime = getSimTime();
	if (%eventCount > %quotaObject.getAllocs_Schedules())
	{
		commandToClient(%client, 'CenterPrint', "<color:FFFFFF>Too many events at once!\n(" @ %EventName @ ")", 1);
		if (%client.SQH_StartTime <= 0)
		{
			%client.SQH_StartTime = %currTime;
		}
		else
		{
			if (%currTime - %client.SQH_LastTime < 2000)
			{
				%client.SQH_HitCount++;
			}
			if (%client.SQH_HitCount > 5)
			{
				%client.ClearEventSchedules();
				%client.resetVehicles();
				%mask = $TypeMasks::PlayerObjectType | $TypeMasks::ProjectileObjectType | $TypeMasks::VehicleObjectType | $TypeMasks::CorpseObjectType;
				%client.ClearEventObjects(%mask);
			}
		}
		%client.SQH_LastTime = %currTime;
		return;
	}
	if (%currTime - %client.SQH_LastTime > 1000)
	{
		%client.SQH_StartTime = 0;
		%client.SQH_HitCount = 0;
	}
	for (%i = 0; %i < %obj.numEvents; %i++)
	{
		if (%obj.eventInput[%i] !$= %EventName)
		{
		}
		else if (!%obj.eventEnabled[%i])
		{
		}
		else if (%obj.eventOutput[%i] $= "CancelEvents" && %obj.eventDelay[%i] == 0)
		{
		}
		else
		{
			%delay = %obj.eventDelay[%i];
			%outputEvent = %obj.eventOutput[%i];
			%par1 = %obj.eventOutputParameter[%i, "1"];
			%par2 = %obj.eventOutputParameter[%i, "2"];
			%par3 = %obj.eventOutputParameter[%i, "3"];
			%par4 = %obj.eventOutputParameter[%i, "4"];
			%outputEventIdx = %obj.eventOutputIdx[%i];
			if (%obj.eventTarget[%i] == -1)
			{
				%name = %obj.eventNT[%i];
				%group = %obj.getGroup();
				for (%j = 0; %j < %group.NTObjectCount[%name]; %j++)
				{
					%target = %group.NTObject[%name, %j];
					if (!isObject(%target))
					{
					}
					else
					{
						%targetClass = "fxDTSBrick";
						%numParameters = outputEvent_GetNumParametersFromIdx(%targetClass, %outputEventIdx);
						if (%obj.eventOutputAppendClient[%i])
						{
							if (%numParameters == 0)
							{
								%scheduleID = %target.schedule(%delay, %outputEvent, %client);
							}
							else if (%numParameters == 1)
							{
								%scheduleID = %target.schedule(%delay, %outputEvent, %par1, %client);
							}
							else if (%numParameters == 2)
							{
								%scheduleID = %target.schedule(%delay, %outputEvent, %par1, %par2, %client);
							}
							else if (%numParameters == 3)
							{
								%scheduleID = %target.schedule(%delay, %outputEvent, %par1, %par2, %par3, %client);
							}
							else if (%numParameters == 4)
							{
								%scheduleID = %target.schedule(%delay, %outputEvent, %par1, %par2, %par3, %par4, %client);
							}
							else
							{
								error("ERROR: SimObject::ProcessInputEvent() - bad number of parameters on event '" @ %outputEvent @ "' (" @ numParameters @ ")");
							}
						}
						else if (%numParameters == 0)
						{
							%scheduleID = %target.schedule(%delay, %outputEvent);
						}
						else if (%numParameters == 1)
						{
							%scheduleID = %target.schedule(%delay, %outputEvent, %par1);
						}
						else if (%numParameters == 2)
						{
							%scheduleID = %target.schedule(%delay, %outputEvent, %par1, %par2);
						}
						else if (%numParameters == 3)
						{
							%scheduleID = %target.schedule(%delay, %outputEvent, %par1, %par2, %par3);
						}
						else if (%numParameters == 4)
						{
							%scheduleID = %target.schedule(%delay, %outputEvent, %par1, %par2, %par3, %par4);
						}
						else
						{
							error("ERROR: SimObject::ProcessInputEvent() - bad number of parameters on event '" @ %outputEvent @ "' (" @ numParameters @ ")");
						}
						if (%delay > 0)
						{
							%obj.addScheduledEvent(%scheduleID);
						}
					}
				}
			}
			else
			{
				%target = $InputTarget_[%obj.eventTarget[%i]];
				if (!isObject(%target))
				{
				}
				else
				{
					%targetClass = inputEvent_GetTargetClass("fxDTSBrick", %obj.eventInputIdx[%i], %obj.eventTargetIdx[%i]);
					%numParameters = outputEvent_GetNumParametersFromIdx(%targetClass, %outputEventIdx);
					if (%obj.eventOutputAppendClient[%i])
					{
						if (%numParameters == 0)
						{
							%scheduleID = %target.schedule(%delay, %outputEvent, %client);
						}
						else if (%numParameters == 1)
						{
							%scheduleID = %target.schedule(%delay, %outputEvent, %par1, %client);
						}
						else if (%numParameters == 2)
						{
							%scheduleID = %target.schedule(%delay, %outputEvent, %par1, %par2, %client);
						}
						else if (%numParameters == 3)
						{
							%scheduleID = %target.schedule(%delay, %outputEvent, %par1, %par2, %par3, %client);
						}
						else if (%numParameters == 4)
						{
							%scheduleID = %target.schedule(%delay, %outputEvent, %par1, %par2, %par3, %par4, %client);
						}
						else
						{
							error("ERROR: SimObject::ProcessInputEvent() - bad number of parameters on event '" @ %outputEvent @ "' (" @ numParameters @ ")");
						}
					}
					else if (%numParameters == 0)
					{
						%scheduleID = %target.schedule(%delay, %outputEvent);
					}
					else if (%numParameters == 1)
					{
						%scheduleID = %target.schedule(%delay, %outputEvent, %par1);
					}
					else if (%numParameters == 2)
					{
						%scheduleID = %target.schedule(%delay, %outputEvent, %par1, %par2);
					}
					else if (%numParameters == 3)
					{
						%scheduleID = %target.schedule(%delay, %outputEvent, %par1, %par2, %par3);
					}
					else if (%numParameters == 4)
					{
						%scheduleID = %target.schedule(%delay, %outputEvent, %par1, %par2, %par3, %par4);
					}
					else
					{
						error("ERROR: SimObject::ProcessInputEvent() - bad number of parameters on event '" @ %outputEvent @ "' (" @ numParameters @ ")");
					}
					if (%delay > 0 && %EventName !$= "onToolBreak")
					{
						%obj.addScheduledEvent(%scheduleID);
					}
				}
			}
		}
	}
}

function SimObject::SetEventEnabled(%obj, %idxList, %val)
{
	%val = mClamp(mFloor(%val), 0, 1);
	if (%idxList $= "ALL")
	{
		for (%i = 0; %i < %obj.numEvents; %i++)
		{
			%obj.eventEnabled[%i] = %val;
		}
		return;
	}
	%wordCount = getWordCount(%idxList);
	for (%i = 0; %i < %wordCount; %i++)
	{
		%idx = atoi(getWord(%idxList, %i));
		if (%idx > %obj.numEvents)
		{
		}
		else if (%idx < 0)
		{
		}
		else
		{
			%obj.eventEnabled[%idx] = %val;
		}
	}
}

function SimObject::ToggleEventEnabled(%obj, %idxList)
{
	if (%idxList $= "ALL")
	{
		for (%i = 0; %i < %obj.numEvents; %i++)
		{
			%obj.eventEnabled[%i] = !%obj.eventEnabled[%i];
		}
		return;
	}
	%wordCount = getWordCount(%idxList);
	for (%i = 0; %i < %wordCount; %i++)
	{
		%idx = atoi(getWord(%idxList, %i));
		if (%idx > %obj.numEvents)
		{
		}
		else if (%idx < 0)
		{
		}
		else
		{
			%obj.eventEnabled[%idx] = !%obj.eventEnabled[%idx];
		}
	}
}

function SimObject::addScheduledEvent(%obj, %scheduleID)
{
	%obj.numScheduledEvents = mFloor(%obj.numScheduledEvents);
	for (%i = 0; %i < %obj.numScheduledEvents; %i++)
	{
		if (isEventPending(%obj.scheduledEvent[%i]))
		{
		}
		else
		{
			%obj.scheduledEvent[%i] = %scheduleID;
			return;
		}
	}
	%obj.scheduledEvent[%obj.numScheduledEvents] = %scheduleID;
	%obj.numScheduledEvents++;
}

function SimObject::cancelEvents(%obj)
{
	for (%i = 0; %i < %obj.numScheduledEvents; %i++)
	{
		if (isEventPending(%obj.scheduledEvent[%i]))
		{
			cancel(%obj.scheduledEvent[%i]);
		}
	}
	%obj.numScheduledEvents = 0;
}

function registerOutputEvent(%class, %name, %parameterList, %appendClient)
{
	if (%appendClient $= "")
	{
		%appendClient = 1;
	}
	$OutputEvent_Count[%class] = mFloor($OutputEvent_Count[%class]);
	for (%i = 0; %i < $OutputEvent_Count[%class]; %i++)
	{
		if ($OutputEvent_Name[%class, %i] $= %name)
		{
			echo("registerOutputEvent() - Output event \"" @ %name @ "\" already registered on class " @ %class @ " - overwriting.");
			$OutputEvent_Name[%class, %i] = %name;
			$OutputEvent_parameterList[%class, %i] = %parameterList;
			verifyOutputParameterList(%class, %i);
			$OutputEvent_AppendClient[%class, %i] = mFloor(%appendClient);
			return;
		}
	}
	%i = mFloor($OutputEvent_Count[%class]);
	$OutputEvent_Name[%class, %i] = %name;
	$OutputEvent_parameterList[%class, %i] = %parameterList;
	verifyOutputParameterList(%class, %i);
	$OutputEvent_AppendClient[%class, %i] = mFloor(%appendClient);
	$OutputEvent_Count[%class]++;
	if (strstr($OutputEvent_ClassList, %class) == -1)
	{
		if ($OutputEvent_ClassList $= "")
		{
			$OutputEvent_ClassList = %class;
		}
		else
		{
			$OutputEvent_ClassList = $OutputEvent_ClassList SPC %class;
		}
	}
}

function unRegisterOutputEvent(%class, %name)
{
	%count = $OutputEvent_Count[%class];
	for (%i = 0; %i < %count; %i++)
	{
		if ($OutputEvent_Name[%class, %i] $= %name)
		{
			for (%j = %i + 1; %j < %count; %j++)
			{
				$OutputEvent_Name[%class, %j - 1] = $OutputEvent_Name[%class, %j];
				$OutputEvent_parameterList[%class, %j - 1] = $OutputEvent_parameterList[%class, %j];
				$OutputEvent_AppendClient[%class, %j - 1] = $OutputEvent_AppendClient[%class, %j];
			}
			$OutputEvent_Name[%class, %count - 1] = "";
			$OutputEvent_parameterList[%class, %count - 1] = "";
			$OutputEvent_AppendClient[%class, %count - 1] = "";
			$OutputEvent_Count[%class]--;
			echo("Un-registering output event \"" @ %name @ "\" from class \"" @ %class @ "\"");
			return;
		}
	}
	error("ERROR: unRegisterOutputEvent() - event \"" @ %name @ "\" is not registered on class \"" @ %class @ "\"");
}

function outputEvent_GetOutputEventIdx(%targetClass, %outputName)
{
	for (%i = 0; %i < $OutputEvent_Count[%targetClass]; %i++)
	{
		if ($OutputEvent_Name[%targetClass, %i] $= %outputName)
		{
			return %i;
		}
	}
	return -1;
}

function outputEvent_GetOutputName(%class, %idx)
{
	return $OutputEvent_Name[%class, %idx];
}

function outputEvent_GetNumParametersFromIdx(%class, %idx)
{
	return getFieldCount($OutputEvent_parameterList[%class, %idx]);
}

function verifyOutputParameterList(%class, %idx)
{
	%count = getFieldCount($OutputEvent_parameterList[%class, %idx]);
	%verifiedList = "";
	for (%i = 0; %i < %count; %i++)
	{
		%field = getField($OutputEvent_parameterList[%class, %idx], %i);
		%type = getWord(%field, 0);
		if (%type $= "int")
		{
			%min = mFloor(getWord(%field, 1));
			%max = mFloor(getWord(%field, 2));
			%default = mFloor(getWord(%field, 3));
			if (%min > %max)
			{
				%min = %max;
				error("WARNING: integer min > max on class:" @ %class @ ", event:" @ $OutputEvent_Name[%class, %idx]);
			}
			if (%default < %min)
			{
				%default = %min;
				error("WARNING: integer default < min on class:" @ %class @ ", event:" @ $OutputEvent_Name[%class, %idx]);
			}
			if (%default > %max)
			{
				%default = %max;
				error("WARNING: integer default > max on class:" @ %class @ ", event:" @ $OutputEvent_Name[%class, %idx]);
			}
			%verifiedField = "int" SPC %min SPC %max SPC %default;
		}
		else if (%type $= "intList")
		{
			%width = mFloor(getWord(%field, 1));
			if (%width <= 8)
			{
				%width = 100;
				error("WARNING: integer list width <= 8 on class:" @ %class @ ", event:" @ $OutputEvent_Name[%class, %idx]);
			}
			%verifiedField = "intList" SPC %width;
		}
		else if (%type $= "float")
		{
			%min = atof(getWord(%field, 1));
			%max = atof(getWord(%field, 2));
			%step = mAbs(getWord(%field, 3));
			%default = atof(getWord(%field, 4));
			if (%min > %max)
			{
				%min = %max - 1;
				error("WARNING: float min > max on class:" @ %class @ ", event:" @ $OutputEvent_Name[%class, %idx]);
			}
			if (%step > %max - %min)
			{
				error("WARNING: float step(" @ %step @ ") > range(" @ %max - %min @ ") on class:" @ %class @ ", event:" @ $OutputEvent_Name[%class, %idx]);
				%step = %max - %min;
			}
			if (%default < %min)
			{
				%default = %min;
				error("WARNING: float default < min on class:" @ %class @ ", event:" @ $OutputEvent_Name[%class, %idx]);
			}
			if (%default > %max)
			{
				%default = %max;
				error("WARNING: float default > max on class:" @ %class @ ", event:" @ $OutputEvent_Name[%class, %idx]);
			}
			%verifiedField = "float" SPC %min SPC %max SPC %step SPC %default;
		}
		else if (%type $= "bool")
		{
			%verifiedField = "bool";
		}
		else if (%type $= "string")
		{
			%maxLength = mFloor(getWord(%field, 1));
			%width = mFloor(getWord(%field, 2));
			if (%maxLength <= 0)
			{
				%maxLength = 1;
				error("WARNING: string maxLength < 1 on class:" @ %class @ ", event:" @ $OutputEvent_Name[%class, %idx]);
			}
			if (%maxLength > 200)
			{
				%maxLength = 200;
				error("WARNING: string maxLength > 200 on class:" @ %class @ ", event:" @ $OutputEvent_Name[%class, %idx]);
			}
			if (%width <= 18)
			{
				%width = 18;
				error("WARNING: string width < 18 on class:" @ %class @ ", event:" @ $OutputEvent_Name[%class, %idx]);
			}
			%verifiedField = "string" SPC %maxLength SPC %width;
		}
		else if (%type $= "datablock")
		{
			%dbClassName = getWord(%field, 1);
			%verifiedField = "datablock" SPC %dbClassName;
		}
		else if (%type $= "vector")
		{
			%verifiedField = "vector";
		}
		else if (%type $= "list")
		{
			%wordCount = getWordCount(%field);
			if ((%wordCount - 1) % 2 != 0)
			{
				error("WARNING: list has odd number of arguments on class:" @ %class @ ", event:" @ $OutputEvent_Name[%class, %idx]);
			}
			if (%wordCount == 1)
			{
				error("WARNING: list has no arguments on class:" @ %class @ ", event:" @ $OutputEvent_Name[%class, %idx]);
			}
			%j = 0;
			for (%verifiedField = "list"; %j < %wordCount; %verifiedField = %verifiedField SPC %text SPC %id)
			{
				%text = getWord(%field, %j++);
				%id = getWord(%field, %j++);
				if (%id != mFloor(%id))
				{
					%id = mFloor(%id);
					error("WARNING: list has non-integer ID \"" @ %id @ "\" on class:" @ %class @ ", event:" @ $OutputEvent_Name[%class, %idx]);
				}
			}
		}
		else if (%type $= "paintColor")
		{
			%verifiedField = "paintColor";
		}
		else
		{
			error("WARNING: Unknown output parameter type \"" @ %type @ "\" on class:" @ %class @ ", event:" @ $OutputEvent_Name[%class, %idx]);
			%verifiedField = %field;
		}
		if (%i == 0)
		{
			%verifiedList = %verifiedField;
		}
		else
		{
			%verifiedList = %verifiedList TAB %verifiedField;
		}
	}
}

function dumpOutputEvents(%class)
{
	%count = $OutputEvent_Count[%class];
	echo("Class \"" @ %class @ "\" has " @ %count @ " registered output events");
	for (%i = 0; %i < %count; %i++)
	{
		echo("  " @ %i @ ": " @ $OutputEvent_Name[%class, %i]);
		%parameterCount = getFieldCount($OutputEvent_parameterList[%class, %i]);
		for (%j = 0; %j < %parameterCount; %j++)
		{
			echo("    " @ getField($OutputEvent_parameterList[%class, %i], %j));
		}
		echo("    Append client: " @ $OutputEvent_AppendClient[%class, %i]);
	}
}

function serverCmdAddEvent(%client, %enabled, %inputEventIdx, %delay, %targetIdx, %NTNameIdx, %outputEventIdx, %par1, %par2, %par3, %par4)
{
	if ($Pref::Server::WrenchEventsAdminOnly == 1)
	{
		if (!%client.isAdmin)
		{
			return;
		}
	}
	%brick = %client.wrenchBrick;
	if (!isObject(%brick))
	{
		messageClient(%client, 'Wrench Error - AddEvent: Brick no longer exists!');
		return;
	}
	if (getTrustLevel(%client, %brick) < $TrustLevel::WrenchEvents && %brick != $LastLoadedBrick)
	{
		commandToClient(%client, 'CenterPrint', %brick.getGroup().name @ " does not trust you enough to do that.", 1);
		return;
	}
	if (%brick.numEvents >= $Game::MaxEventsPerBrick)
	{
		return;
	}
	%brickClass = %brick.getClassName();
	%enabled = mClamp(mFloor(%enabled), 0, 1);
	%delay = mClamp(%delay, 0, 30000);
	if (%inputEventIdx == -1)
	{
		%i = mFloor(%brick.numEvents);
		%brick.eventEnabled[%i] = %enabled;
		%brick.eventDelay[%i] = %delay;
		%brick.eventInputIdx[%i] = %inputEventIdx;
		%brick.numEvents++;
		return;
	}
	%inputEventIdx = mClamp(%inputEventIdx, 0, $InputEvent_Count[%brickClass]);
	%targetIdx = mClamp(%targetIdx, -1, getFieldCount($InputEvent_TargetList[%brickClass, %inputEventIdx]));
	%NTNameIdx = mClamp(%NTNameIdx, 0, %brick.getGroup().NTNameCount - 1);
	if (%targetIdx == -1)
	{
		%targetClass = "fxDTSBrick";
	}
	else
	{
		%targetClass = getWord(getField($InputEvent_TargetList[%brickClass, %inputEventIdx], %targetIdx), 1);
	}
	if (%targetClass $= "")
	{
		error("ERROR: serverCmdAddEvent() - invalid target class.  %inputEventIdx:" @ %inputEventIdx @ " %targetIdx:" @ %targetIdx);
		return;
	}
	%outputEventIdx = mClamp(%outputEventIdx, 0, $OutputEvent_Count[%targetClass]);
	%verifiedPar[1] = "";
	%verifiedPar[2] = "";
	%verifiedPar[3] = "";
	%verifiedPar[4] = "";
	%parameterCount = getFieldCount($OutputEvent_parameterList[%targetClass, %outputEventIdx]);
	for (%i = 1; %i < %parameterCount + 1; %i++)
	{
		%field = getField($OutputEvent_parameterList[%targetClass, %outputEventIdx], %i - 1);
		%type = getWord(%field, 0);
		if (%type $= "int")
		{
			%min = mFloor(getWord(%field, 1));
			%max = mFloor(getWord(%field, 2));
			%default = mFloor(getWord(%field, 3));
			%val = %par[%i];
			if (%val $= "")
			{
				%val = %default;
			}
			%verifiedPar[%i] = mClamp(%val, %min, %max);
		}
		else if (%type $= "intList")
		{
			%wordCount = getWordCount(%par[%i]);
			if (%par[%i] $= "ALL")
			{
				%verifiedPar[%i] = "ALL";
			}
			else
			{
				%verifiedPar[%i] = "";
				for (%w = 0; %w < %wordCount; %w++)
				{
					%word = atoi(getWord(%par[%i], %w));
					if (%w == 0)
					{
						%verifiedPar[%i] = %word;
					}
					else
					{
						%verifiedPar[%i] = %verifiedPar[%i] SPC %word;
					}
				}
			}
		}
		else if (%type $= "float")
		{
			%min = atof(getWord(%field, 1));
			%max = atof(getWord(%field, 2));
			%step = mAbs(getWord(%field, 3));
			%default = atof(getWord(%field, 4));
			%val = %par[%i];
			if (%val $= "")
			{
				%val = %default;
			}
			%val = mClampF(%val, %min, %max);
			%numSteps = mFloor((%val - %min) / %step);
			%val = %min + %numSteps * %step;
			%verifiedPar[%i] = %val;
		}
		else if (%type $= "bool")
		{
			if (%par[%i])
			{
				%verifiedPar[%i] = 1;
			}
			else
			{
				%verifiedPar[%i] = 0;
			}
		}
		else if (%type $= "string")
		{
			%maxLength = mFloor(getWord(%field, 1));
			%width = mFloor(getWord(%field, 2));
			%par[%i] = strreplace(%par[%i], "<font:", "&A01");
			%par[%i] = strreplace(%par[%i], "<color:", "&A02");
			%par[%i] = strreplace(%par[%i], "<bitmap:", "&A03");
			%par[%i] = strreplace(%par[%i], "<shadow:", "&A04");
			%par[%i] = strreplace(%par[%i], "<shadowcolor:", "&A05");
			%par[%i] = strreplace(%par[%i], "<linkcolor:", "&A06");
			%par[%i] = strreplace(%par[%i], "<linkcolorHL:", "&A07");
			%par[%i] = strreplace(%par[%i], "<a:", "&A08");
			%par[%i] = strreplace(%par[%i], "</a>", "&A09");
			%par[%i] = strreplace(%par[%i], "<br>", "&A10");
			%par[%i] = StripMLControlChars(%par[%i]);
			%par[%i] = strreplace(%par[%i], "&A01", "<font:");
			%par[%i] = strreplace(%par[%i], "&A02", "<color:");
			%par[%i] = strreplace(%par[%i], "&A03", "<bitmap:");
			%par[%i] = strreplace(%par[%i], "&A04", "<shadow:");
			%par[%i] = strreplace(%par[%i], "&A05", "<shadowcolor:");
			%par[%i] = strreplace(%par[%i], "&A06", "<linkcolor:");
			%par[%i] = strreplace(%par[%i], "&A07", "<linkcolorHL:");
			%par[%i] = strreplace(%par[%i], "&A08", "<a:");
			%par[%i] = strreplace(%par[%i], "&A09", "</a>");
			%par[%i] = strreplace(%par[%i], "&A10", "<br>");
			%verifiedPar[%i] = getSubStr(%par[%i], 0, %maxLength);
			%verifiedPar[%i] = chatWhiteListFilter(%verifiedPar[%i]);
			%verifiedPar[%i] = strreplace(%verifiedPar[%i], ";", "");
		}
		else if (%type $= "datablock")
		{
			%dbClassName = getWord(%field, 1);
			if (isObject(%par[%i]))
			{
				%newDB = %par[%i].getId();
			}
			else if (%par[%i] $= "NONE" || %par[%i] $= "-1")
			{
				%newDB = -1;
			}
			else
			{
				if (%dbClassName $= "FxLightData")
				{
					%newDB = "PlayerLight";
				}
				else if (%dbClassName $= "ItemData")
				{
					%newDB = "hammerItem";
				}
				else if (%dbClassName $= "ProjectileData")
				{
					if (isObject(gunProjectile))
					{
						%newDB = "gunProjectile";
					}
					else
					{
						%newDB = "deathProjectile";
					}
				}
				else if (%dbClassName $= "ParticleEmitterData")
				{
					%newDB = "PlayerFoamEmitter";
				}
				else if (%dbClassName $= "Music")
				{
					%newDB = "musicData_After_School_Special";
				}
				else if (%dbClassName $= "Sound")
				{
					%newDB = "lightOnSound";
				}
				else if (%dbClassName $= "Vehicle")
				{
					%newDB = "JeepVehicle";
				}
				else if (%dbClassName $= "PlayerData")
				{
					%newDB = "PlayerStandardArmor";
				}
				else
				{
					%newDB = -1;
				}
				if (isObject(%newDB))
				{
					%newDB = %newDB.getId();
				}
				else
				{
					%newDB = -1;
				}
			}
			if (!isObject(%newDB))
			{
				%newDB = -1;
			}
			if (%newDB != -1)
			{
				if (%dbClassName $= "Music")
				{
					if (%newDB.getClassName() !$= "AudioProfile")
					{
						return;
					}
					if (%newDB.uiName $= "")
					{
						return;
					}
					continue;
				}
				if (%dbClassName $= "Sound")
				{
					if (%newDB.getClassName() !$= "AudioProfile")
					{
						return;
					}
					if (%newDB.uiName !$= "")
					{
						return;
					}
					if (%newDB.getDescription().isLooping == 1)
					{
						return;
					}
					if (!%newDB.getDescription().is3D)
					{
						return;
					}
					continue;
				}
				if (%dbClassName $= "Vehicle")
				{
					%dbClass = %newDB.getClassName();
					if (%newDB.uiName $= "")
					{
						return;
					}
					if (%dbClass !$= "WheeledVehicleData" && %dbClass !$= "HoverVehicleData" && %dbClass !$= "FlyingVehicleData" && !(%dbClass $= "PlayerData" && %newDB.rideable))
					{
						return;
					}
					continue;
				}
				if (%newDB.getClassName() !$= %dbClassName)
				{
					return;
				}
				if (%newDB.uiName $= "")
				{
					return;
				}
			}
			%verifiedPar[%i] = %newDB;
		}
		else if (%type $= "vector")
		{
			%x = atof(getWord(%par[%i], 0));
			%y = atof(getWord(%par[%i], 1));
			%z = atof(getWord(%par[%i], 2));
			%mag = atoi(getWord(%field, 1));
			if (%mag == 0)
			{
				%mag = 200;
			}
			%vec = %x SPC %y SPC %z;
			if (VectorLen(%vec) > %mag)
			{
				%vec = VectorNormalize(%vec);
				%vec = VectorScale(%vec, %mag);
				%x = atoi(getWord(%vec, 0));
				%y = atoi(getWord(%vec, 1));
				%z = atoi(getWord(%vec, 2));
			}
			%verifiedPar[%i] = %x SPC %y SPC %z;
		}
		else if (%type $= "list")
		{
			%val = mFloor(%par[%i]);
			%itemCount = (getWordCount(%field) - 1) / 2;
			%foundMatch = 0;
			for (%j = 0; %j < %itemCount; %j++)
			{
				%idx = %j * 2 + 1;
				%name = getWord(%field, %idx);
				%id = getWord(%field, %idx + 1);
				if (%val == %id)
				{
					%foundMatch = 1;
					break;
				}
			}
			if (!%foundMatch)
			{
				return;
			}
			%verifiedPar[%i] = %val;
		}
		else if (%type $= "paintColor")
		{
			%color = %par[%i];
			if (%client == $LoadingBricks_Client && $LoadingBricks_ColorMethod == 3)
			{
				%color = $colorTranslation[%color];
			}
			%verifiedPar[%i] = mClamp(%color, 0, $maxSprayColors);
		}
		else
		{
			error("ERROR: serverCmdAddEvent() - default type validation for type \"" @ %type @ "\"");
			%verifiedPar[%i] = strreplace(%par[%i], ";", "");
		}
	}
	%i = mFloor(%brick.numEvents);
	%brick.eventInputIdx[%i] = %inputEventIdx;
	%brick.eventTargetIdx[%i] = %targetIdx;
	%brick.eventOutputIdx[%i] = %outputEventIdx;
	%brick.eventEnabled[%i] = %enabled;
	%brick.eventInput[%i] = $InputEvent_Name[%brickClass, %inputEventIdx];
	%brick.eventDelay[%i] = %delay;
	if (%targetIdx == -1)
	{
		%targetClass = "FxDTSBrick";
		%brick.eventTarget[%i] = -1;
		%brick.eventNT[%i] = %brick.getGroup().NTName[%NTNameIdx];
	}
	else
	{
		%brick.eventTarget[%i] = getWord(getField($InputEvent_TargetList[%brickClass, %inputEventIdx], %targetIdx), 0);
		%brick.eventNT[%i] = "";
	}
	%brick.eventOutput[%i] = $OutputEvent_Name[%targetClass, %outputEventIdx];
	%brick.eventOutputParameter[%i, "1"] = %verifiedPar[1];
	%brick.eventOutputParameter[%i, "2"] = %verifiedPar[2];
	%brick.eventOutputParameter[%i, "3"] = %verifiedPar[3];
	%brick.eventOutputParameter[%i, "4"] = %verifiedPar[4];
	if (%brick.eventOutput[%i] $= "FireRelay")
	{
		if (%brick.eventDelay[%i] < 33)
		{
			%brick.eventDelay[%i] = 33;
		}
	}
	%brick.eventOutputAppendClient[%i] = $OutputEvent_AppendClient[%targetClass, %outputEventIdx];
	%brick.numEvents++;
	if (!%brick.implicitCancelEvents)
	{
		%obj = %brick;
		for (%i = 0; %i < %obj.numEvents; %i++)
		{
			if (%obj.eventInput[%i] !$= "OnRelay")
			{
			}
			else if (%obj.eventTarget[%i] == -1)
			{
			}
			else if (%obj.eventTarget[%i] !$= "Self")
			{
			}
			else
			{
				%outputEvent = %obj.eventOutput[%i];
				if (%outputEvent !$= "fireRelay")
				{
				}
				else
				{
					%obj.implicitCancelEvents = 1;
					break;
				}
			}
		}
	}
}

function fxDTSBrick::addEvent(%obj, %enabled, %delay, %inputEvent, %target, %outputEvent, %par1, %par2, %par3, %par4)
{
	%inputEventIdx = inputEvent_GetInputEventIdx(%inputEvent);
	%targetIdx = inputEvent_GetTargetIndex("fxDTSBrick", %inputEventIdx, %target);
	%targetClass = inputEvent_GetTargetClass("fxDTSBrick", %inputEventIdx, %targetIdx);
	%outputEventIdx = outputEvent_GetOutputEventIdx(%targetClass, %outputEvent);
	%client = %obj.getGroup().client;
	%client.wrenchBrick = %obj;
	serverCmdAddEvent(%client, %enabled, %inputEventIdx, %delay, %targetIdx, %NTNameIdx, %outputEventIdx, %par1, %par2, %par3, %par4);
}

function SimObject::dumpEvents(%obj)
{
	echo("Object " @ %obj @ " has " @ %obj.numEvents @ " events");
	for (%i = 0; %i < %obj.numEvents; %i++)
	{
		echo("  " @ %i @ ": " @ %obj.eventInput[%i] @ " wait:" @ %obj.eventDelay[%i]);
		echo("    enabled: " @ %obj.eventEnabled[%i]);
		echo("    target: " @ %obj.eventTarget[%i] @ " " @ %obj.eventNT[%i]);
		echo("    output:" SPC %obj.eventOutput[%i] SPC %obj.eventOutputParameter[%i, "1"] SPC %obj.eventOutputParameter[%i, "2"] SPC %obj.eventOutputParameter[%i, "3"] SPC %obj.eventOutputParameter[%i, "4"]);
	}
}

function serverCmdClearEvents(%client)
{
	%brick = %client.wrenchBrick;
	if (%brick == 0)
	{
		return;
	}
	if (!isObject(%brick))
	{
		messageClient(%client, 'Wrench Error - ClearEvents: Brick no longer exists!');
		return;
	}
	if (getTrustLevel(%client, %brick) < $TrustLevel::WrenchEvents)
	{
		commandToClient(%client, 'CenterPrint', %brick.getGroup().name @ " does not trust you enough to do that.", 1);
		return;
	}
	%brick.clearEvents();
}

function SimObject::clearEvents(%obj)
{
	for (%i = 0; %i < %obj.numEvents; %i++)
	{
		%obj.eventDelay[%i] = "";
		%obj.eventCmd[%i] = "";
	}
	%obj.numEvents = 0;
	%obj.implicitCancelEvents = 0;
}

function ServerCmdRequestWrenchEvents(%client)
{
	%brick = %client.wrenchBrick;
	if (!isObject(%brick) && %client.isAdmin)
	{
		%brick = %client.adminWrenchBrick;
	}
	if (!isObject(%brick))
	{
		messageClient(%client, 'Wrench Error - RequestWrenchEvents: Brick no longer exists!');
		return;
	}
	for (%i = 0; %i < %brick.numEvents; %i++)
	{
		%line = %brick.serializeEvent(%i);
		commandToClient(%client, 'addEvent', %line);
	}
	commandToClient(%client, 'addEventsDone');
}

function SimObject::serializeEvent(%obj, %idx)
{
	if (%obj.eventTargetIdx[%idx] == -1)
	{
		%group = %obj.getGroup();
		for (%i = 0; %i < %group.NTNameCount; %i++)
		{
			if (%obj.eventNT[%idx] !$= %group.NTName[%i])
			{
			}
			else
			{
				%NTNameIdx = %i;
				break;
			}
		}
	}
	else
	{
		%NTNameIdx = "";
	}
	%line = %obj.eventEnabled[%idx] TAB %obj.eventInputIdx[%idx] TAB %obj.eventDelay[%idx] TAB %obj.eventTargetIdx[%idx] TAB %NTNameIdx TAB %obj.eventOutputIdx[%idx] TAB %obj.eventOutputParameter[%idx, "1"] TAB %obj.eventOutputParameter[%idx, "2"] TAB %obj.eventOutputParameter[%idx, "3"] TAB %obj.eventOutputParameter[%idx, "4"];
	return %line;
}

function SimObject::serializeEventToString(%obj, %idx, %client)
{
	%line = %idx TAB %obj.eventEnabled[%idx] TAB %obj.eventInputIdx[%idx] TAB %obj.eventDelay[%idx] TAB %obj.eventTargetIdx[%idx] TAB %obj.eventNT[%idx] TAB %obj.eventOutputIdx[%idx];
	if (%obj.eventTargetIdx[%idx] == -1)
	{
		%targetClass = "fxDTSBrick";
	}
	else
	{
		%targetClass = getWord(getField($InputEvent_TargetList["fxDTSBrick", %obj.eventInputIdx[%idx]], %obj.eventTargetIdx[%idx]), 1);
	}
	for (%i = 0; %i < 4; %i++)
	{
		%field = getField($OutputEvent_parameterList[%targetClass, %obj.eventOutputIdx[%idx]], %i);
		%dataType = getWord(%field, 0);
		if (%dataType $= "dataBlock")
		{
			if (isObject(%obj.eventOutputParameter[%idx, %i + 1]))
			{
				%line = %line TAB %obj.eventOutputParameter[%idx, %i + 1].getName();
			}
			else
			{
				%line = %line TAB "-1";
			}
		}
		else
		{
			%line = %line TAB %obj.eventOutputParameter[%idx, %i + 1];
		}
	}
	if (isObject(%client))
	{
		if (%obj.eventTargetIdx[%idx] == -1)
		{
			%outputName = outputEvent_GetOutputName("fxDTSBrick", %obj.eventOutputIdx[%idx]);
			if (%outputName $= "setEmitter")
			{
				%name = %obj.eventNT[%idx];
				%group = %obj.getGroup();
				for (%j = 0; %j < %group.NTObjectCount[%name]; %j++)
				{
					%target = %group.NTObject[%name, %j];
					%ghostID = %client.getGhostID(%target);
					commandToClient(%client, 'TransmitEmitterDirection', %ghostID, %target.emitterDirection);
				}
			}
			else if (%outputName $= "setItem")
			{
				%name = %obj.eventNT[%idx];
				%group = %obj.getGroup();
				for (%j = 0; %j < %group.NTObjectCount[%name]; %j++)
				{
					%target = %group.NTObject[%name, %j];
					%ghostID = %client.getGhostID(%target);
					commandToClient(%client, 'TransmitItemDirection', %ghostID, %target.itemPosition TAB %target.itemDirection TAB %target.itemRespawnTime);
				}
			}
		}
		else
		{
			%targetClass = inputEvent_GetTargetClass("fxDTSBrick", %obj.eventInputIdx[%idx], %obj.eventTargetIdx[%idx]);
			if (%targetClass $= "fxDTSBrick")
			{
				%outputName = outputEvent_GetOutputName("fxDTSBrick", %obj.eventOutputIdx[%idx]);
				if (%outputName $= "setEmitter")
				{
					%ghostID = %client.getGhostID(%obj);
					commandToClient(%client, 'TransmitEmitterDirection', %ghostID, %obj.emitterDirection);
				}
				else if (%outputName $= "setItem")
				{
					%ghostID = %client.getGhostID(%obj);
					commandToClient(%client, 'TransmitItemDirection', %ghostID, %obj.itemPosition TAB %obj.itemDirection TAB %obj.itemRespawnTime);
				}
			}
		}
	}
	return %line;
}

function serverCmdRequestEventTables(%client)
{
	%count = getWordCount($InputEvent_ClassList);
	for (%i = 0; %i < %count; %i++)
	{
		%class = getWord($InputEvent_ClassList, %i);
		for (%j = 0; %j < $InputEvent_Count[%class]; %j++)
		{
			%name = $InputEvent_Name[%class, %j];
			%targetList = $InputEvent_TargetList[%class, %j];
			commandToClient(%client, 'RegisterInputEvent', %class, %name, %targetList);
		}
	}
	%count = getWordCount($OutputEvent_ClassList);
	for (%i = 0; %i < %count; %i++)
	{
		%class = getWord($OutputEvent_ClassList, %i);
		for (%j = 0; %j < $OutputEvent_Count[%class]; %j++)
		{
			%name = $OutputEvent_Name[%class, %j];
			%parameterList = $OutputEvent_parameterList[%class, %j];
			commandToClient(%client, 'RegisterOutputEvent', %class, %name, %parameterList);
		}
	}
	commandToClient(%client, 'RegisterEventsDone');
}

function SimObject::setNTObjectName(%obj, %name)
{
	%name = trim(%name);
	%name = getSafeVariableName(%name);
	if (strlen(%name) < 1)
	{
		%obj.clearNTObjectName();
		%obj.setName("");
		return;
	}
	if (getSubStr(%name, 0, 1) !$= "_")
	{
		%name = "_" @ %name;
	}
	if (isObject(%name))
	{
		if (!(%name.getType() & $TypeMasks::FxBrickAlwaysObjectType))
		{
			error("ERROR: SimObject::setNTObjectName() - Non-Brick object named \"" @ %name @ "\" already exists!");
			return;
		}
	}
	if (%obj.getName() $= %name)
	{
		return;
	}
	%obj.clearNTObjectName();
	%group = %obj.getGroup();
	%group.NTObjectCount[%name] = mFloor(%group.NTObjectCount[%name]);
	if (%group.NTObjectCount[%name] <= 0)
	{
		%group.addNTName(%name);
	}
	%group.NTObject[%name, %group.NTObjectCount[%name]] = %obj;
	%group.NTObjectCount[%name]++;
	%obj.setName(%name);
}

function SimObject::clearNTObjectName(%obj)
{
	%group = %obj.getGroup();
	%oldName = %obj.getName();
	if (%oldName $= "")
	{
		return;
	}
	for (%i = 0; %i < %group.NTObjectCount[%oldName]; %i++)
	{
		if (%group.NTObject[%oldName, %i] != %obj)
		{
		}
		else
		{
			%group.NTObject[%oldName, %i] = %group.NTObject[%oldName, %group.NTObjectCount[%oldName] - 1];
			%group.NTObject[%oldName, %group.NTObjectCount[%oldName] - 1] = "";
			%group.NTObjectCount[%oldName]--;
			%i--;
		}
	}
	if (%group.NTObjectCount[%oldName] <= 0)
	{
		%group.removeNTName(%oldName);
	}
}

function SimGroup::addNTName(%obj, %name)
{
	%obj.NTNameCount = mFloor(%obj.NTNameCount);
	for (%i = 0; %i < %obj.NTNameCount; %i++)
	{
		if (%obj.NTName[%i] $= %name)
		{
			return;
		}
	}
	%obj.NTName[%obj.NTNameCount] = %name;
	%obj.NTNameCount++;
	%count = ClientGroup.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		%client = ClientGroup.getObject(%i);
		if (!isObject(%client.brickGroup))
		{
		}
		else if (%client.brickGroup.getId() != %obj.getId())
		{
		}
		else
		{
			commandToClient(%client, 'AddNTName', %name);
		}
	}
}

function SimGroup::removeNTName(%obj, %name)
{
	%obj.NTNameCount = mFloor(%obj.NTNameCount);
	for (%i = 0; %i < %obj.NTNameCount; %i++)
	{
		if (%obj.NTName[%i] !$= %name)
		{
		}
		else
		{
			%obj.NTName[%i] = %obj.NTName[%obj.NTNameCount - 1];
			%obj.NTName[%obj.NTNameCount - 1] = "";
			%obj.NTNameCount--;
			break;
		}
	}
	%count = ClientGroup.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		%client = ClientGroup.getObject(%i);
		if (!isObject(%client.brickGroup))
		{
		}
		else if (%client.brickGroup.getId() != %obj.getId())
		{
		}
		else
		{
			commandToClient(%client, 'RemoveNTName', %name);
		}
	}
}

function SimGroup::DumpNTNames(%obj)
{
	echo("Group " @ %obj.getName() @ " has " @ %obj.NTNameCount @ " NTNames:");
	for (%i = 0; %i < %obj.NTNameCount; %i++)
	{
		%name = %obj.NTName[%i];
		%count = %obj.NTObjectCount[%name];
		echo("  " @ %name @ ": " @ %count @ " objects");
		for (%j = 0; %j < %count; %j++)
		{
			echo("    " @ %obj.NTObject[%name, %j] @ " : " @ isObject(%obj.NTObject[%name, %j]));
		}
	}
}

function SimGroup::ClearAllNTNames(%obj)
{
	for (%i = 0; %i < %obj.NTNameCount; %i++)
	{
		%name = %obj.NTName[%i];
		%count = %obj.NTObjectCount[%name];
		for (%j = 0; %j < %count; %j++)
		{
			%obj.NTObject[%name, %j] = "";
		}
		%obj.NTName[%i] = "";
		%obj.NTObjectCount[%name] = "";
	}
	%obj.NTNameCount = 0;
}

function ServerCmdRequestNamedTargets(%client)
{
	%group = %client.brickGroup;
	%count = %group.NTNameCount;
	for (%i = 0; %i < %count; %i++)
	{
		%name = %group.NTName[%i];
		commandToClient(%client, 'AddNTName', %name);
	}
}

function ServerCmdRequestExtendedBrickInfo(%client)
{
	%client.TransmitExtendedBrickInfo(0, 0);
}

function ServerCmdCancelExtendedBrickInfoRequest(%client)
{
	if (isEventPending(%client.TransmitExtendedBrickInfoEvent))
	{
		cancel(%client.TransmitExtendedBrickInfoEvent);
	}
}

function GameConnection::TransmitExtendedBrickInfo(%client, %groupIdx, %brickIdx)
{
	if (isEventPending(%client.TransmitExtendedBrickInfoEvent))
	{
		cancel(%client.TransmitExtendedBrickInfoEvent);
	}
	%groupCount = mainBrickGroup.getCount();
	%brickGroup = mainBrickGroup.getObject(%groupIdx);
	%brickCount = %brickGroup.getCount();
	for (%i = 0; %i < 150; %i++)
	{
		if (%brickIdx >= %brickCount)
		{
			%safety = 0;
			for (%groupIdx++; 1; %groupIdx++)
			{
				%safety++;
				if (%safety > 50000)
				{
					error("ERROR: GameConnection::TransmitAllBrickNames() - More than 50k brick groups?");
					return;
				}
				if (%groupIdx >= %groupCount)
				{
					commandToClient(%client, 'TransmitAllBrickNamesDone');
					return;
				}
				%brickGroup = mainBrickGroup.getObject(%groupIdx);
				%brickCount = %brickGroup.getCount();
				if (%brickCount > 0)
				{
					break;
				}
			}
			%brickIdx = 0;
		}
		%brick = %brickGroup.getObject(%brickIdx);
		%ghostID = %client.getGhostID(%brick);
		if (%ghostID > 0)
		{
			if (%brick.getName() !$= "")
			{
				commandToClient(%client, 'TransmitBrickName', %ghostID, %brick.getName());
			}
			for (%j = 0; %j < %brick.numEvents; %j++)
			{
				%line = %brick.serializeEventToString(%j, %client);
				commandToClient(%client, 'TransmitEvent', %ghostID, %line);
			}
			if (%brick.isBasePlate)
			{
				if ($Server::LAN)
				{
					commandToClient(%client, 'TransmitBrickOwner', %ghostID, %brick.bl_id);
					continue;
				}
				commandToClient(%client, 'TransmitBrickOwner', %ghostID, %brick.getGroup().bl_id);
			}
		}
		%brickIdx++;
	}
	%client.TransmitExtendedBrickInfoEvent = %client.schedule(32, TransmitExtendedBrickInfo, %groupIdx, %brickIdx);
}

function SimGroup::getClient(%this)
{
	if ($Server::LAN)
	{
		error("ERROR: SimGroup::getClient() - function should not be used in a LAN game.");
		return -1;
	}
	if (isObject(%this.client))
	{
		if (%this.bl_id == %this.client.getBLID())
		{
			return %this.client;
		}
	}
	%count = ClientGroup.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		%cl = ClientGroup.getObject(%i);
		if (%cl.getBLID() == %this.bl_id)
		{
			%this.client = %cl;
			return %cl;
		}
	}
	return 0;
}

function SimGroup::hasUser(%this)
{
	if ($Server::LAN)
	{
		return 1;
	}
	%count = ClientGroup.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		%cl = ClientGroup.getObject(%i);
		if (getTrustLevel(%this, %cl))
		{
			return 1;
		}
	}
	return 0;
}

function New_QueueSO(%size)
{
	if (%size <= 1 || %size > 10000)
	{
		error("ERROR: New_QueueSO() - invalid size '" @ %size @ "'");
		return;
	}
	%ret = new ScriptObject()
	{
		class = QueueSO;
		size = %size;
		head = 0;
		tail = 0;
	};
	MissionCleanup.add(%ret);
	for (%i = 0; %i < %size; %i++)
	{
		%ret.val[%i] = 0;
	}
	return %ret;
}

function QueueSO::push(%obj, %val)
{
	%obj.val[%obj.head] = %val;
	%obj.head = (%obj.head + 1) % %obj.size;
	%obj.val[%obj.head] = 0;
	if (%obj.head == %obj.tail)
	{
		%obj.tail = (%obj.tail + 1) % %obj.size;
	}
}

function QueueSO::pop(%obj)
{
	if (%obj.head != %obj.tail)
	{
		%obj.head--;
		if (%obj.head < 0)
		{
			%obj.head = %obj.size - 1;
		}
		%ret = %obj.val[%obj.head];
		%obj.val[%obj.head] = 0;
		return %ret;
	}
	else
	{
		return 0;
	}
}

function QueueSO::dumpVals(%obj)
{
	for (%i = 0; %i < %obj.size; %i++)
	{
		%line = %i @ ": " @ %obj.val[%i];
		if (%obj.head == %i)
		{
			%line = %line @ " <Head";
		}
		if (%obj.tail == %i)
		{
			%line = %line @ " <Tail";
		}
		echo(%line);
	}
}

function serverCmdClearColors(%client)
{
	if (!%client.isAdmin)
	{
		return;
	}
	if ($LoadingBricks_Client $= %client)
	{
	}
	else
	{
		%name = $LoadingBricks_Client.getPlayerName();
		messageClient(%client, '', %name SPC "is loading bricks right now. Try again later.");
	}
}

function serverCmdSetColorMethod(%client, %val)
{
	if (!%client.isAdmin)
	{
		return;
	}
	if ($LoadingBricks_Client != %client)
	{
		return;
	}
	if ($Pref::Server::AllowColorLoading)
	{
		$LoadingBricks_ColorMethod = %val;
	}
	else
	{
		$LoadingBricks_ColorMethod = 3;
	}
}

function serverCmdSetSaveUploadDirName(%client, %dirName, %doOwnership)
{
	if (!%client.isAdmin)
	{
		return;
	}
	if ($LoadingBricks_Client != %client)
	{
		return;
	}
	$LoadingBricks_DirName = %dirName;
	$LoadingBricks_DoOwnership = %doOwnership;
	calcSaveOffset();
}

function serverCmdInitUploadHandshake(%client)
{
	if (!%client.isAdmin)
	{
		return;
	}
	if ($Game::MissionCleaningUp)
	{
		messageClient(%client, '', "Can't load during mission clean up");
		commandToClient(%client, 'LoadBricksConfirmHandshake', 0);
		return;
	}
	if (!isObject($LoadingBricks_Client))
	{
		if (isEventPending($LoadingBricks_TimeoutSchedule))
		{
			cancel($LoadingBricks_TimeoutSchedule);
		}
		$LoadingBricks_StartTime = getSimTime();
		$LoadingBricks_Client = %client;
		$LoadingBricks_Name = %client.getPlayerName();
		$LoadingBricks_DirName = "";
		$LoadingBricks_PositionOffset = "0 0 0";
		$LoadingBricks_FailureCount = 0;
		$LoadingBricks_BrickCount = 0;
		$LoadBrick_LineCount = 0;
		if (isObject($LoadingBricks_Client))
		{
			%name = $LoadingBricks_Client.getPlayerName();
		}
		else
		{
			%name = $LoadingBricks_Name;
		}
		echo(%name @ " is uploading a save file.");
		MessageAll('MsgUploadStart', '\c3%1\c0 is uploading a save file.', %name);
		commandToClient(%client, 'LoadBricksConfirmHandshake', 1, $Pref::Server::AllowColorLoading);
		if (isObject($LoadBrick_FileObj))
		{
			$LoadBrick_FileObj.delete();
		}
		$LoadBrick_FileObj = new FileObject();
		$LoadBrick_FileObj.openForWrite("base/server/temp/temp.bls");
		$LoadingBricks_TimeoutSchedule = schedule(30 * 1000, 0, serverLoadBricks_Timeout);
	}
	else
	{
		%name = $LoadingBricks_Client.getPlayerName();
		messageClient(%client, '', %name SPC "is loading bricks right now. Try again later.");
		commandToClient(%client, 'LoadBricksConfirmHandshake', 0);
	}
}

function serverCmdStartSaveFileUpload(%client, %colorMethod)
{
	return;
	if (!%client.isAdmin)
	{
		return;
	}
	if (!isObject($LoadingBricks_Client))
	{
		if ($Pref::Server::AllowColorLoading)
		{
			$LoadingBricks_ColorMethod = %colorMethod;
		}
		else
		{
			$LoadingBricks_ColorMethod = 3;
		}
		$LoadingBricks_StartTime = getSimTime();
		$LoadingBricks_Client = %client;
		$LoadingBricks_Name = %client.getPlayerName();
		$LoadingBricks_FailureCount = 0;
		$LoadingBricks_BrickCount = 0;
		$LoadBrick_LineCount = 0;
		%name = $LoadingBricks_Client.getPlayerName();
		MessageAll('', %name SPC "is uploading a save file.");
		commandToClient(%client, 'LoadBricksHandshake', 1, $Pref::Server::AllowColorLoading);
		if (isObject($LoadBrick_FileObj))
		{
			$LoadBrick_FileObj.delete();
		}
		$LoadBrick_FileObj = new FileObject();
		$LoadBrick_FileObj.openForWrite("base/server/temp/temp.bls");
		$LoadingBricks_TimeoutSchedule = schedule(30 * 1000, 0, serverLoadBricks_Timeout);
		$LoadingBricks_colorCount = -1;
		for (%i = 0; %i < 64; %i++)
		{
			if (getWord(getColorIDTable(%i), 3) > 0.001)
			{
				$LoadingBricks_colorCount++;
			}
		}
		if (%colorMethod == 0)
		{
		}
		else if (%colorMethod == 1)
		{
			$LoadingBricks_divCount = 0;
			for (%i = 0; %i < 16; %i++)
			{
				if (getSprayCanDivisionSlot(%i) != 0)
				{
					$LoadingBricks_divCount++;
				}
				else
				{
					break;
				}
			}
		}
		else if (%colorMethod == 2)
		{
			$LoadingBricks_colorCount = -1;
			$LoadingBricks_divCount = -1;
		}
		else if (%colorMethod == 3)
		{
		}
	}
	else
	{
		%name = $LoadingBricks_Client.getPlayerName();
		messageClient(%client, '', %name SPC "is loading bricks right now. Try again later.");
		commandToClient(%client, 'LoadBricksHandshake', 0);
	}
}

function serverCmdUploadSaveFileLine(%client, %line)
{
	if (!%client.isAdmin)
	{
		return;
	}
	if (%client != $LoadingBricks_Client)
	{
		return;
	}
	if (isEventPending($LoadingBricks_TimeoutSchedule))
	{
		cancel($LoadingBricks_TimeoutSchedule);
		$LoadingBricks_TimeoutSchedule = schedule(30 * 1000, 0, serverLoadBricks_Timeout);
	}
	$LoadBrick_LineCount++;
	$LoadBrick_FileObj.writeLine(%line);
}

function serverLoadBricks_Timeout()
{
	if (isObject($LoadingBricks_Client))
	{
		%name = $LoadingBricks_Client.getPlayerName();
	}
	else
	{
		%name = $LoadingBricks_Name;
	}
	MessageAll('', %name @ "'s save file upload timed out.");
	$LoadingBricks_Client = "";
	$LoadingBricks_Name = "";
	$LoadingBricks_ColorMethod = "";
	if ($Server::LAN == 0 && doesAllowConnections())
	{
		startRaytracer();
	}
	if (isObject($LoadBrick_FileObj))
	{
		$LoadBrick_FileObj.close();
		$LoadBrick_FileObj.delete();
	}
}

function ServerLoadSaveFile_Start(%filename)
{
	echo("LOADING BRICKS: " @ %filename @ " (ColorMethod " @ $LoadingBricks_ColorMethod @ ")");
	if ($Game::MissionCleaningUp)
	{
		echo("LOADING CANCELED: Mission cleanup in progress");
		return;
	}
	$Server_LoadFileObj = new FileObject();
	if (isFile(%filename))
	{
		$Server_LoadFileObj.openForRead(%filename);
	}
	else
	{
		$Server_LoadFileObj.openForRead("base/server/temp/temp.bls");
	}
	if ($UINameTableCreated == 0)
	{
		createUiNameTable();
	}
	$LastLoadedBrick = 0;
	$Load_failureCount = 0;
	$Load_brickCount = 0;
	$Server_LoadFileObj.readLine();
	%lineCount = $Server_LoadFileObj.readLine();
	for (%i = 0; %i < %lineCount; %i++)
	{
		$Server_LoadFileObj.readLine();
	}
	if (isEventPending($LoadSaveFile_Tick_Schedule))
	{
		cancel($LoadSaveFile_Tick_Schedule);
	}
	ServerLoadSaveFile_ProcessColorData();
	ServerLoadSaveFile_Tick();
	stopRaytracer();
}

function ServerLoadSaveFile_ProcessColorData()
{
	%colorCount = -1;
	for (%i = 0; %i < 64; %i++)
	{
		if (getWord(getColorIDTable(%i), 3) > 0.001)
		{
			%colorCount++;
		}
	}
	if ($LoadingBricks_ColorMethod == 0)
	{
	}
	else if ($LoadingBricks_ColorMethod == 1)
	{
		%divCount = 0;
		for (%i = 0; %i < 16; %i++)
		{
			if (getSprayCanDivisionSlot(%i) != 0)
			{
				%divCount++;
			}
			else
			{
				break;
			}
		}
	}
	else if ($LoadingBricks_ColorMethod == 2)
	{
		%colorCount = -1;
		%divCount = -1;
	}
	else if ($LoadingBricks_ColorMethod == 3)
	{
	}
	for (%i = 0; %i < 64; %i++)
	{
		%color = $Server_LoadFileObj.readLine();
		%red = getWord(%color, 0);
		%green = getWord(%color, 1);
		%blue = getWord(%color, 2);
		%alpha = getWord(%color, 3);
		if ($LoadingBricks_ColorMethod == 0)
		{
			if (%alpha >= 0.0001)
			{
				%match = 0;
				for (%j = 0; %j < 64; %j++)
				{
					if (colorMatch(getColorIDTable(%j), %color))
					{
						$colorTranslation[%i] = %j;
						%match = 1;
						break;
					}
				}
				if (%match == 0)
				{
					error("ERROR: ServerLoadSaveFile_ProcessColorData() - color method 0 specified but match not found for color " @ %color);
				}
			}
		}
		else if ($LoadingBricks_ColorMethod == 1)
		{
			if (%alpha >= 0.0001)
			{
				%match = 0;
				for (%j = 0; %j < 64; %j++)
				{
					if (colorMatch(getColorIDTable(%j), %color))
					{
						$colorTranslation[%i] = %j;
						%match = 1;
						break;
					}
				}
				if (%match == 0)
				{
					setSprayCanColor(%colorCount++, %color);
					$colorTranslation[%i] = %colorCount;
				}
			}
		}
		else if ($LoadingBricks_ColorMethod == 2)
		{
			setSprayCanColor(%colorCount++, %color);
			$colorTranslation[%i] = %i;
		}
		else if ($LoadingBricks_ColorMethod == 3)
		{
			if (%alpha < 0.0001)
			{
				continue;
			}
			%minDiff = 99999;
			%matchIdx = -1;
			for (%j = 0; %j < 64; %j++)
			{
				%checkColor = getColorIDTable(%j);
				%checkRed = getWord(%checkColor, 0);
				%checkGreen = getWord(%checkColor, 1);
				%checkBlue = getWord(%checkColor, 2);
				%checkAlpha = getWord(%checkColor, 3);
				%diff = 0;
				%diff += mAbs(mAbs(%checkRed) - mAbs(%red));
				%diff += mAbs(mAbs(%checkGreen) - mAbs(%green));
				%diff += mAbs(mAbs(%checkBlue) - mAbs(%blue));
				if (%checkAlpha > 0.99 && %alpha < 0.99 || %checkAlpha < 0.99 && %alpha > 0.99)
				{
					%diff += 1000;
				}
				else
				{
					%diff += mAbs(mAbs(%checkAlpha) - mAbs(%alpha)) * 0.5;
				}
				if (%diff < %minDiff)
				{
					%minDiff = %diff;
					%matchIdx = %j;
				}
			}
			if (%matchIdx == -1)
			{
				error("ERROR - LoadBricks() - Nearest match failed - wtf.");
				continue;
			}
			$colorTranslation[%i] = %matchIdx;
		}
	}
	if ($LoadingBricks_ColorMethod == 1)
	{
		echo("  setting spraycan division at ", %divCount, " ", %colorCount);
		setSprayCanDivision(%divCount, %colorCount, "File");
	}
	if ($LoadingBricks_ColorMethod != 0 && $LoadingBricks_ColorMethod != 3)
	{
		$maxSprayColors = %colorCount;
		for (%clientIndex = 0; %clientIndex < ClientGroup.getCount(); %clientIndex++)
		{
			%cl = ClientGroup.getObject(%clientIndex);
			%cl.transmitStaticBrickData();
			%cl.transmitDataBlocks(1);
			commandToClient(%cl, 'PlayGui_LoadPaint');
		}
	}
}

function ServerLoadSaveFile_Tick()
{
	if (isObject(ServerConnection))
	{
		if (!ServerConnection.isLocal())
		{
			return;
		}
	}
	%line = $Server_LoadFileObj.readLine();
	%firstWord = getWord(%line, 0);
	if (%firstWord $= "+-EVENT")
	{
		if (isObject($LastLoadedBrick))
		{
			%idx = getField(%line, 1);
			%enabled = getField(%line, 2);
			%inputName = getField(%line, 3);
			%delay = getField(%line, 4);
			%targetName = getField(%line, 5);
			%NT = getField(%line, 6);
			%outputName = getField(%line, 7);
			%par1 = getField(%line, 8);
			%par2 = getField(%line, 9);
			%par3 = getField(%line, 10);
			%par4 = getField(%line, 11);
			%inputEventIdx = inputEvent_GetInputEventIdx(%inputName);
			%targetIdx = inputEvent_GetTargetIndex("fxDTSBrick", %inputEventIdx, %targetName);
			if (%targetName == -1)
			{
				%targetClass = "fxDTSBrick";
			}
			else
			{
				%field = getField($InputEvent_TargetList["fxDTSBrick", %inputEventIdx], %targetIdx);
				%targetClass = getWord(%field, 1);
			}
			%outputEventIdx = outputEvent_GetOutputEventIdx(%targetClass, %outputName);
			%NTNameIdx = -1;
			$LoadingBricks_Client.wrenchBrick = $LastLoadedBrick;
			serverCmdAddEvent($LoadingBricks_Client, %enabled, %inputEventIdx, %delay, %targetIdx, %NTNameIdx, %outputEventIdx, %par1, %par2, %par3, %par4);
			$LastLoadedBrick.eventNT[$LastLoadedBrick.numEvents - 1] = %NT;
		}
	}
	else if (%firstWord $= "+-NTOBJECTNAME")
	{
		if (isObject($LastLoadedBrick))
		{
			%name = getWord(%line, 1);
			$LastLoadedBrick.setNTObjectName(%name);
		}
	}
	else if (%firstWord $= "+-LIGHT")
	{
		if (isObject($LastLoadedBrick))
		{
			%line = getSubStr(%line, 8, strlen(%line) - 8);
			%pos = strpos(%line, "\"");
			%dbName = getSubStr(%line, 0, %pos);
			%db = $uiNameTable_Lights[%dbName];
			if (!isObject(%db))
			{
				%db = $uiNameTable_Lights["Player's Light"];
			}
			if ((strlen(%line) - %pos) - 2 >= 0)
			{
				%line = getSubStr(%line, %pos + 2, (strlen(%line) - %pos) - 2);
				%enabled = %line;
				if (%enabled $= "")
				{
					%enabled = 1;
				}
			}
			else
			{
				%enabled = 1;
			}
			%quotaObject = getQuotaObjectFromBrick($LastLoadedBrick);
			setCurrentQuotaObject(%quotaObject);
			$LastLoadedBrick.setLight(%db);
			if (isObject($LastLoadedBrick.light))
			{
				$LastLoadedBrick.light.setEnable(%enabled);
			}
			clearCurrentQuotaObject();
		}
	}
	else if (%firstWord $= "+-EMITTER")
	{
		if (isObject($LastLoadedBrick))
		{
			%line = getSubStr(%line, 10, strlen(%line) - 10);
			%pos = strpos(%line, "\"");
			%dbName = getSubStr(%line, 0, %pos);
			if (%dbName $= "NONE")
			{
				%db = 0;
			}
			else
			{
				%db = $uiNameTable_Emitters[%dbName];
			}
			%line = getSubStr(%line, %pos + 2, (strlen(%line) - %pos) - 2);
			%dir = getWord(%line, 0);
			%quotaObject = getQuotaObjectFromBrick($LastLoadedBrick);
			setCurrentQuotaObject(%quotaObject);
			if (isObject(%db))
			{
				$LastLoadedBrick.setEmitter(%db);
			}
			$LastLoadedBrick.setEmitterDirection(%dir);
			clearCurrentQuotaObject();
		}
	}
	else if (%firstWord $= "+-ITEM")
	{
		if (isObject($LastLoadedBrick))
		{
			%line = getSubStr(%line, 7, strlen(%line) - 7);
			%pos = strpos(%line, "\"");
			%dbName = getSubStr(%line, 0, %pos);
			if (%dbName $= "NONE")
			{
				%db = 0;
			}
			else
			{
				%db = $uiNameTable_Items[%dbName];
			}
			%line = getSubStr(%line, %pos + 2, (strlen(%line) - %pos) - 2);
			%pos = getWord(%line, 0);
			%dir = getWord(%line, 1);
			%respawnTime = getWord(%line, 2);
			%quotaObject = getQuotaObjectFromBrick($LastLoadedBrick);
			setCurrentQuotaObject(%quotaObject);
			if (isObject(%db))
			{
				$LastLoadedBrick.setItem(%db);
			}
			$LastLoadedBrick.setItemDirection(%dir);
			$LastLoadedBrick.setItemPosition(%pos);
			$LastLoadedBrick.setItemRespawntime(%respawnTime);
			clearCurrentQuotaObject();
		}
	}
	else if (%firstWord $= "+-AUDIOEMITTER")
	{
		if (isObject($LastLoadedBrick))
		{
			%line = getSubStr(%line, 15, strlen(%line) - 15);
			%pos = strpos(%line, "\"");
			%dbName = getSubStr(%line, 0, %pos);
			%db = $uiNameTable_Music[%dbName];
			%quotaObject = getQuotaObjectFromBrick($LastLoadedBrick);
			setCurrentQuotaObject(%quotaObject);
			$LastLoadedBrick.setSound(%db);
			clearCurrentQuotaObject();
		}
	}
	else if (%firstWord $= "+-VEHICLE")
	{
		if (isObject($LastLoadedBrick))
		{
			%line = getSubStr(%line, 10, strlen(%line) - 10);
			%pos = strpos(%line, "\"");
			%dbName = getSubStr(%line, 0, %pos);
			%db = $uiNameTable_Vehicle[%dbName];
			%line = getSubStr(%line, %pos + 2, (strlen(%line) - %pos) - 2);
			%recolorVehicle = getWord(%line, 0);
			%quotaObject = getQuotaObjectFromBrick($LastLoadedBrick);
			setCurrentQuotaObject(%quotaObject);
			$LastLoadedBrick.setVehicle(%db);
			$LastLoadedBrick.setReColorVehicle(%recolorVehicle);
			clearCurrentQuotaObject();
		}
	}
	else if (%firstWord $= "")
	{
	}
	else if (%firstWord $= "Linecount")
	{
		if (isObject(ProgressGui))
		{
			Progress_Bar.total = getWord(%line, 1);
			Progress_Bar.setValue(0);
			Progress_Bar.count = 0;
			Canvas.popDialog(ProgressGui);
			Progress_Window.setText("Loading Progress");
			Progress_Text.setText("Loading...");
		}
	}
	else if (%firstWord $= "+-OWNER")
	{
		if (isObject($LastLoadedBrick))
		{
			if ($LoadingBricks_DoOwnership)
			{
				%ownerBLID = mAbs(mFloor(getWord(%line, 1)));
				%oldGroup = $LastLoadedBrick.getGroup();
				if ($Server::LAN)
				{
					$LastLoadedBrick.bl_id = %ownerBLID;
				}
				else
				{
					%ownerBrickGroup = "BrickGroup_" @ %ownerBLID;
					if (isObject(%ownerBrickGroup))
					{
						%ownerBrickGroup = %ownerBrickGroup.getId();
					}
					else
					{
						%ownerBrickGroup = new SimGroup("BrickGroup_" @ %ownerBLID);
						%ownerBrickGroup.client = 0;
						%ownerBrickGroup.name = "\c1BL_ID: " @ %ownerBLID @ "\c0";
						%ownerBrickGroup.bl_id = %ownerBLID;
						mainBrickGroup.add(%ownerBrickGroup);
					}
					if (isObject(%ownerBrickGroup))
					{
						%ownerBrickGroup.add($LastLoadedBrick);
						if (isObject(brickSpawnPointData))
						{
							if ($LastLoadedBrick.getDataBlock().getId() == brickSpawnPointData.getId())
							{
								if (%ownerBrickGroup != %oldGroup)
								{
									%oldGroup.removeSpawnBrick($LastLoadedBrick);
									%ownerBrickGroup.addSpawnBrick($LastLoadedBrick);
								}
							}
						}
					}
				}
			}
		}
	}
	else
	{
		if ($Server::BrickCount >= $Pref::Server::BrickLimit)
		{
			MessageAll('', 'Brick limit reached (%1)', $Pref::Server::BrickLimit);
			ServerLoadSaveFile_End();
			return;
		}
		if (!isUnlocked())
		{
			if ($Server::BrickCount >= 150)
			{
				MessageAll('', 'Demo brick limit reached (%1)', 150);
				ServerLoadSaveFile_End();
				return;
			}
		}
		%quotePos = strstr(%line, "\"");
		%uiName = getSubStr(%line, 0, %quotePos);
		%db = $uiNameTable[%uiName];
		%line = getSubStr(%line, %quotePos + 2, 9999);
		%pos = getWords(%line, 0, 2);
		%angId = getWord(%line, 3);
		%isBaseplate = getWord(%line, 4);
		%colorId = $colorTranslation[mFloor(getWord(%line, 5))];
		%printName = getWord(%line, 6);
		if (strpos(%printName, "/") != -1)
		{
			%printName = fileBase(%printName);
			%aspectRatio = %db.printAspectRatio;
			%printIDName = %aspectRatio @ "/" @ %printName;
			%printId = $printNameTable[%printIDName];
			if (%printId $= "")
			{
				%printIDName = "Letters/" @ %printName;
				%printId = $printNameTable[%printIDName];
			}
			if (%printId $= "")
			{
				%printId = $printNameTable["Letters/-space"];
			}
		}
		else
		{
			%printId = $printNameTable[%printName];
		}
		%colorFX = getWord(%line, 7);
		%shapeFX = getWord(%line, 8);
		%rayCasting = getWord(%line, 9);
		%collision = getWord(%line, 10);
		%rendering = getWord(%line, 11);
		%pos = VectorAdd(%pos, $LoadingBricks_PositionOffset);
		if (%db)
		{
			%trans = %pos;
			if (%angId == 0)
			{
				%trans = %trans SPC " 1 0 0 0";
			}
			else if (%angId == 1)
			{
				%trans = %trans SPC " 0 0 1" SPC $piOver2;
			}
			else if (%angId == 2)
			{
				%trans = %trans SPC " 0 0 1" SPC $pi;
			}
			else if (%angId == 3)
			{
				%trans = %trans SPC " 0 0 -1" SPC $piOver2;
			}
			%b = new fxDTSBrick()
			{
				dataBlock = %db;
				angleID = %angId;
				isBasePlate = %isBaseplate;
				colorID = %colorId;
				printID = %printId;
				colorFxID = %colorFX;
				shapeFxID = %shapeFX;
				isPlanted = 1;
			};
			if (isObject($LoadingBricks_BrickGroup))
			{
				$LoadingBricks_BrickGroup.add(%b);
			}
			else
			{
				error("ERROR: ServerLoadSaveFile_Tick() - $LoadingBricks_BrickGroup does not exist!");
				MessageAll('', "ERROR: ServerLoadSaveFile_Tick() - $LoadingBricks_BrickGroup does not exist!");
				ServerLoadSaveFile_End();
				return;
			}
			%b.setTransform(%trans);
			%b.TrustCheckFinished();
			$LastLoadedBrick = %b;
			%err = %b.plant();
			if (%err == 1 || %err == 3 || %err == 5)
			{
				$Load_failureCount++;
				%b.delete();
				$LastLoadedBrick = 0;
			}
			else
			{
				if (%rayCasting !$= "")
				{
					%b.setRayCasting(%rayCasting);
				}
				if (%collision !$= "")
				{
					%b.setColliding(%collision);
				}
				if (%rendering !$= "")
				{
					%b.setRendering(%rendering);
				}
				if ($LoadingBricks_DoOwnership && !$Server::LAN)
				{
					%oldGroup = %b.getGroup();
					%ownerGroup = "";
					if (%b.getNumDownBricks())
					{
						%ownerGroup = %b.getDownBrick(0).getGroup();
						%ownerGroup.add(%b);
					}
					else if (%b.getNumUpBricks())
					{
						%ownerGroup = %b.getUpBrick(0).getGroup();
						%ownerGroup.add(%b);
					}
					if (isObject(brickSpawnPointData))
					{
						if (%b.getDataBlock().getId() == brickSpawnPointData.getId())
						{
							if (%ownerGroup > 0 && %ownerGroup != %oldGroup)
							{
								%oldGroup.removeSpawnBrick(%b);
								%ownerGroup.addSpawnBrick(%b);
							}
						}
					}
				}
				else
				{
					$LastLoadedBrick.client = $LoadingBricks_Client;
				}
			}
		}
		else
		{
			if (!$Load_MissingBrickWarned[%uiName])
			{
				warn("WARNING: loadBricks() - DataBlock not found for brick named \"", %uiName, "\"");
				$Load_MissingBrickWarned[%uiName] = 1;
			}
			$LastLoadedBrick = 0;
			$Load_failureCount++;
		}
		$Load_brickCount++;
		if (isObject(ProgressGui))
		{
			Progress_Bar.count++;
			Progress_Bar.setValue(Progress_Bar.count / Progress_Bar.total);
			if (Progress_Bar.count + 1 == Progress_Bar.total)
			{
				Canvas.popDialog(ProgressGui);
			}
		}
	}
	if (!isUnlocked())
	{
		if ($LoadingBricks_Client.brickGroup.getCount() > 300)
		{
			ServerLoadSaveFile_End();
			return;
		}
	}
	if (!$Server_LoadFileObj.isEOF())
	{
		if ($Server::ServerType $= "SinglePlayer")
		{
			$LoadSaveFile_Tick_Schedule = schedule(0, 0, ServerLoadSaveFile_Tick);
		}
		else
		{
			$LoadSaveFile_Tick_Schedule = schedule(0, 0, ServerLoadSaveFile_Tick);
		}
	}
	else
	{
		ServerLoadSaveFile_End();
	}
}

function ServerLoadSaveFile_End()
{
	$LoadingBricks_Client = "";
	$LoadingBricks_ColorMethod = "";
	if ($Server::LAN == 0 && doesAllowConnections())
	{
		startRaytracer();
	}
	%time = getSimTime() - $LoadingBricks_StartTime;
	if (!$LoadingBricks_Silent)
	{
		MessageAll('MsgProcessComplete', $Load_brickCount - $Load_failureCount @ " / " @ $Load_brickCount @ " bricks created in " @ getTimeString(mFloor((%time / 1000) * 100) / 100));
	}
	deleteVariables("$Load_*");
	$LoadingBricks_Silent = 0;
	if (isEventPending($LoadSaveFile_Tick_Schedule))
	{
		cancel($LoadSaveFile_Tick_Schedule);
	}
	if (isEventPending($LoadingBricks_TimeoutSchedule))
	{
		cancel($LoadingBricks_TimeoutSchedule);
	}
	$Server_LoadFileObj.delete();
	$Server_LoadFileObj = 0;
}

function colorMatch(%colorA, %colorB)
{
	for (%i = 0; %i < 4; %i++)
	{
		if (mAbs(getWord(%colorA, %i) - getWord(%colorB, %i)) > 0.005)
		{
			return 0;
		}
	}
	return 1;
}

function createUiNameTable()
{
	$UINameTableCreated = 1;
	%dbCount = getDataBlockGroupSize();
	for (%i = 0; %i < %dbCount; %i++)
	{
		%db = getDataBlock(%i);
		if (%db.uiName $= "")
		{
		}
		else if (%db.getClassName() $= "FxDTSBrickData")
		{
			$uiNameTable[%db.uiName] = %db;
		}
		else if (%db.getClassName() $= "FxLightData")
		{
			if (%db.uiName !$= "")
			{
				$uiNameTable_Lights[%db.uiName] = %db;
			}
		}
		else if (%db.getClassName() $= "ParticleEmitterData")
		{
			if (%db.uiName !$= "")
			{
				$uiNameTable_Emitters[%db.uiName] = %db;
			}
		}
		else if (%db.getClassName() $= "ItemData")
		{
			if (%db.uiName !$= "")
			{
				$uiNameTable_Items[%db.uiName] = %db;
			}
		}
		else if (%db.getClassName() $= "AudioProfile")
		{
			if (%db.uiName !$= "")
			{
				if (%db.getDescription().isLooping)
				{
					$uiNameTable_Music[%db.uiName] = %db;
					continue;
				}
				$uiNameTable_Sounds[%db.uiName] = %db;
			}
		}
		else if (%db.getClassName() $= "PlayerData")
		{
			if (%db.uiName !$= "")
			{
				if (%db.rideable)
				{
					$uiNameTable_Vehicle[%db.uiName] = %db;
				}
			}
		}
		else if (%db.getClassName() $= "WheeledVehicleData")
		{
			if (%db.uiName !$= "")
			{
				if (%db.rideable)
				{
					$uiNameTable_Vehicle[%db.uiName] = %db;
				}
			}
		}
		else if (%db.getClassName() $= "FlyingVehicleData")
		{
			if (%db.uiName !$= "")
			{
				if (%db.rideable)
				{
					$uiNameTable_Vehicle[%db.uiName] = %db;
				}
			}
		}
		else if (%db.getClassName() $= "HoverVehicleData")
		{
			if (%db.uiName !$= "")
			{
				if (%db.rideable)
				{
					$uiNameTable_Vehicle[%db.uiName] = %db;
				}
			}
		}
	}
}

function calcSaveOffset()
{
	%offset["Bedroom"] = "0 -200 286.4";
	%offset["Kitchen"] = "-380 0 119.2";
	%offset["Slate"] = "0 0 0";
	if (%offset[$LoadingBricks_DirName] !$= "" && %offset[MissionInfo.saveName] !$= "")
	{
		$LoadingBricks_PositionOffset = VectorSub(%offset[MissionInfo.saveName], %offset[$LoadingBricks_DirName]);
	}
	else
	{
		$LoadingBricks_PositionOffset = "0 0 0";
	}
}

function serverDirectSaveFileLoad(%filename, %colorMethod, %dirName, %doOwnership, %silent)
{
	echo("direct load ", %filename);
	if (!isFile(%filename))
	{
		MessageAll('', "ERROR: File \"" @ %filename @ "\" not found.  If you are seeing this, you broke something.");
		return;
	}
	$LoadingBricks_ColorMethod = %colorMethod;
	$LoadingBricks_DirName = %dirName;
	$LoadingBricks_DoOwnership = %doOwnership;
	calcSaveOffset();
	if ($LoadingBricks_Client && $LoadingBricks_Client != 1)
	{
		MessageAll('', "Load interrupted by host.");
		if (isObject($LoadBrick_FileObj))
		{
			$LoadBrick_FileObj.close();
			$LoadBrick_FileObj.delete();
		}
	}
	$LoadingBricks_Client = findLocalClient();
	$LoadingBricks_BrickGroup = $LoadingBricks_Client.brickGroup;
	$LoadingBricks_Silent = %silent;
	if (!$LoadingBricks_Silent)
	{
		MessageAll('MsgUploadStart', "Loading bricks. Please wait.");
	}
	$LoadingBricks_StartTime = getSimTime();
	ServerLoadSaveFile_Start(%filename);
}

function serverCmdCancelSaveFileUpload(%client)
{
	if (!%client.isAdmin)
	{
		return;
	}
	if ($LoadingBricks_Client == %client)
	{
		MessageAll('', "Save file upload canceled.");
		$LoadingBricks_Client = "";
		$LoadingBricks_ColorMethod = "";
		if ($Server::LAN == 0 && doesAllowConnections())
		{
			startRaytracer();
		}
		if (isObject($LoadBrick_FileObj))
		{
			$LoadBrick_FileObj.close();
			$LoadBrick_FileObj.delete();
		}
		if (isEventPending($LoadSaveFile_Tick_Schedule))
		{
			cancel($LoadSaveFile_Tick_Schedule);
		}
		if (isEventPending($LoadingBricks_TimeoutSchedule))
		{
			cancel($LoadingBricks_TimeoutSchedule);
		}
	}
}

function serverCmdEndSaveFileUpload(%client)
{
	if (!%client.isAdmin)
	{
		return;
	}
	if ($LoadingBricks_Client == %client)
	{
		$LoadingBricks_BrickGroup = $LoadingBricks_Client.brickGroup;
		%time = getSimTime() - $LoadingBricks_StartTime;
		echo("Save file uploaded in " @ getTimeString(mFloor((%time / 1000) * 100) / 100) @ " (" @ $LoadBrick_LineCount @ " lines).  Processing...");
		MessageAll('MsgUploadEnd', "Save file uploaded in " @ getTimeString(mFloor((%time / 1000) * 100) / 100) @ " (" @ $LoadBrick_LineCount @ " lines).  Processing...");
		$LoadingBricks_StartTime = getSimTime();
		$LoadBrick_FileObj.close();
		$LoadBrick_FileObj.delete();
		if (isEventPending($LoadingBricks_TimeoutSchedule))
		{
			cancel($LoadingBricks_TimeoutSchedule);
		}
		ServerLoadSaveFile_Start();
	}
	else
	{
		messageClient(%client, '', "Error: serverCmdEndSaveFileUpload() - you are not currently loading bricks.");
	}
}

function serverCmdReloadBricks(%client)
{
	if (!%client.isAdmin)
	{
		return;
	}
	if ($Game::MissionCleaningUp)
	{
		messageClient(%client, '', 'Can\'t reload bricks during mission clean up');
		return;
	}
	if ($LoadingBricks_Client && $LoadingBricks_Client != 1)
	{
		messageClient(%client, '', 'There is another load in progress.');
		return;
	}
	$LoadingBricks_Client = %client;
	$LoadingBricks_BrickGroup = $LoadingBricks_Client.brickGroup;
	if ($LoadingBricks_DoOwnership $= "")
	{
		$LoadingBricks_DoOwnership = 1;
	}
	echo(%client.getPlayerName() @ " Re-Loaded Bricks.");
	MessageAll('MsgUploadStart', '\c3%1\c0 Re-Loading bricks. Please wait.', %client.getPlayerName());
	if ($LoadingBricks_ColorMethod $= "")
	{
		$LoadingBricks_ColorMethod = 3;
	}
	$LoadingBricks_StartTime = getSimTime();
	ServerLoadSaveFile_Start();
}

$currCheckVal = 10;
$RightHandSlot = 0;
$LeftHandSlot = 1;
$BackSlot = 2;
$RightFootSlot = 3;
$LeftFootSlot = 4;
$HeadSlot = 5;
$VisorSlot = 6;
$HipSlot = 7;
$pi = 3.1415927;
$piOver2 = 1.5707963;
$m2pi = 6.2831853;
$ItemTime = 10000;
$ItemDropTime = 1000;
$tallestBrick = 6 * 0.6;
$Game::MinRespawnTime = 1000;
$Game::MaxRespawnTime = 30000;
$Game::MinVehicleRespawnTime = 0;
$Game::MaxVehicleRespawnTime = 300000;
$Game::MinBrickRespawnTime = 2000;
$Game::MaxBrickRespawnTime = 300000;
$Game::Item::PopTime = 10 * 1000;
$Game::Item::RespawnTime = 4000;
$Game::Item::MinRespawnTime = 1000;
$Game::Item::MaxRespawnTime = 300000;
$Game::MiniGameJoinTime = 5000;
$Game::MaxAdminTries = 3;
$Game::DefaultMinImpactSpeed = 30;
$Game::DefaultSpeedDamageScale = 3.8;
$Game::DefaultMinRunOverSpeed = 2;
$Game::DefaultRunOverDamageScale = 5;
$Game::DefaultRunOverPushScale = 1.2;
$Game::MinMountTime = 1000;
$Game::OnTouchImmuneTime = 2000;
$Game::PlayerInvulnerabilityTime = 2500;
$Game::VehicleInvulnerabilityTime = 100;
$Game::MaxEventsPerBrick = 100;
$Game::BrickActivateRange = 5;
$Game::MaxQuota::Schedules = 1000;
$Game::MaxQuota::Misc = 9999;
$Game::MaxQuota::Projectile = 1000;
$Game::MaxQuota::Item = 1000;
$Game::MaxQuota::Environment = 5000;
$Game::MaxQuota::Player = 500;
$Game::MaxQuota::Vehicle = 200;
$Game::MinQuota::Schedules = 10;
$Game::MinQuota::Misc = 10;
$Game::MinQuota::Projectile = 5;
$Game::MinQuota::Item = 5;
$Game::MinQuota::Environment = 20;
$Game::MinQuota::Player = 0;
$Game::MinQuota::Vehicle = 0;
$Game::DefaultQuota::Schedules = 50;
$Game::DefaultQuota::Misc = 100;
$Game::DefaultQuota::Projectile = 25;
$Game::DefaultQuota::Item = 25;
$Game::DefaultQuota::Environment = 100;
$Game::DefaultQuota::Player = 10;
$Game::DefaultQuota::Vehicle = 5;
$Game::Max::MaxPhysicsVehicles_Total = 20;
$LastError::Trust = 1;
$LastError::MiniGameDifferent = 2;
$LastError::MiniGameNotYours = 3;
$LastError::NotInMiniGame = 4;
function eulerToMatrix(%euler)
{
	%euler = VectorScale(%euler, $pi / 180);
	%matrix = MatrixCreateFromEuler(%euler);
	%xvec = getWord(%matrix, 3);
	%yvec = getWord(%matrix, 4);
	%zvec = getWord(%matrix, 5);
	%ang = getWord(%matrix, 6);
	%ang = (%ang * 180) / $pi;
	%rotationMatrix = %xvec @ " " @ %yvec @ " " @ %zvec @ " " @ %ang;
	return %rotationMatrix;
}

function eulerToQuat(%euler)
{
	%euler = VectorScale(%euler, $pi / 180);
	%matrix = MatrixCreateFromEuler(%euler);
	%xvec = getWord(%matrix, 3);
	%yvec = getWord(%matrix, 4);
	%zvec = getWord(%matrix, 5);
	%ang = getWord(%matrix, 6);
	%quat = %xvec SPC %yvec SPC %zvec SPC %ang;
	return %quat;
}

function eulerToQuat_degrees(%euler)
{
	%euler = VectorScale(%euler, $pi / 180);
	%matrix = MatrixCreateFromEuler(%euler);
	%xvec = getWord(%matrix, 3);
	%yvec = getWord(%matrix, 4);
	%zvec = getWord(%matrix, 5);
	%ang = getWord(%matrix, 6);
	%ang = (%ang * 180) / $pi;
	%quat = %xvec SPC %yvec SPC %zvec SPC %ang;
	return %quat;
}

function getLine(%phrase, %lineNum)
{
	%offset = 0;
	%lineCount = 0;
	while (%lineCount <= %lineNum)
	{
		%pos = strpos(%phrase, "\n", %offset);
		if (%pos >= 0)
		{
			%len = %pos - %offset;
		}
		else
		{
			%len = 99999;
		}
		%line = getSubStr(%phrase, %offset, %len);
		if (%lineCount == %lineNum)
		{
			return %line;
		}
		%lineCount++;
		%offset = %pos + 1;
		if (%pos == -1)
		{
			return "";
		}
	}
	return "";
}

function getLineCount(%phrase)
{
	%offset = 0;
	for (%lineCount = 0; %offset >= 0; %lineCount++)
	{
		%offset = strpos(%phrase, "\n", %offset + 1);
	}
	return %lineCount;
}

function posFromTransform(%transform)
{
	%position = getWord(%transform, 0) @ " " @ getWord(%transform, 1) @ " " @ getWord(%transform, 2);
	return %position;
}

function rotFromTransform(%transform)
{
	%rotation = getWord(%transform, 3) @ " " @ getWord(%transform, 4) @ " " @ getWord(%transform, 5) @ " " @ getWord(%transform, 6);
	return %rotation;
}

function posFromRaycast(%transform)
{
	%position = getWord(%transform, 1) @ " " @ getWord(%transform, 2) @ " " @ getWord(%transform, 3);
	return %position;
}

function normalFromRaycast(%transform)
{
	%norm = getWord(%transform, 4) @ " " @ getWord(%transform, 5) @ " " @ getWord(%transform, 6);
	return %norm;
}

function round(%val)
{
	%val *= 100;
	%val = mFloor(%val + 0.5);
	%val /= 100;
	return %val;
}

function getTimeString(%timeS)
{
	if (%timeS >= 3600)
	{
		%hours = mFloor(%timeS / 3600);
		%timeS -= %hours * 3600;
		%minutes = mFloor(%timeS / 60);
		%timeS -= %minutes * 60;
		%seconds = %timeS;
		if (%minutes < 10)
		{
			%minutes = "0" @ %minutes;
		}
		if (%seconds < 10)
		{
			%seconds = "0" @ %seconds;
		}
		return %hours @ ":" @ %minutes @ ":" @ %seconds;
	}
	else if (%timeS >= 60)
	{
		%minutes = mFloor(%timeS / 60);
		%timeS -= %minutes * 60;
		%seconds = %timeS;
		if (%seconds < 10)
		{
			%seconds = "0" @ %seconds;
		}
		return %minutes @ ":" @ %seconds;
	}
	else
	{
		%seconds = %timeS;
		if (%seconds < 10)
		{
			%seconds = "0" @ %seconds;
		}
		return "0:" @ %seconds;
	}
}

function isListenServer()
{
	if (!isObject(ServerConnection))
	{
		return 0;
	}
	if (ServerConnection.isLocal())
	{
		return 1;
	}
	return 0;
}

function serverCmdKick(%client, %victim)
{
	if (%client != 0)
	{
		if (!%client.isAdmin && !%client.isSuperAdmin)
		{
			return;
		}
	}
	if (!isObject(%victim))
	{
		return;
	}
	if (%victim.getClassName() !$= "GameConnection")
	{
		return;
	}
	if (%victim.getBLID() $= "0")
	{
		return;
	}
	if (!%victim.isAIControlled())
	{
		if (%victim.getBLID() $= getNumKeyID())
		{
			if (isObject(%client))
			{
				messageClient(%client, '', '\c5You can\'t kick the server owner.');
			}
			else
			{
				echo("\c2You can't kick the server owner.");
			}
			return;
		}
		if (%victim.isSuperAdmin)
		{
			if (isObject(%client))
			{
				messageClient(%client, 'MsgAdminForce', '\c5You can\'t kick Super-Admins.');
			}
			return;
		}
		else if (%victim.isLocal())
		{
			if (isObject(%client))
			{
				messageClient(%client, 'MsgAdminForce', '\c5You can\'t kick the local client.');
			}
			return;
		}
		else
		{
			if ($Server::LAN)
			{
				MessageAll('MsgAdminForce', '\c3%1\c2 kicked \c3%2', %client.getPlayerName(), %victim.getPlayerName());
			}
			else
			{
				MessageAll('MsgAdminForce', '\c3%1\c2 kicked \c3%2\c2(ID: %3)', %client.getPlayerName(), %victim.getPlayerName(), %victim.getBLID(), %victim.getRawIP());
			}
			if (!$Server::LAN)
			{
				%victim.ClearEventSchedules();
				%victim.ClearEventObjects($TypeMasks::ProjectileObjectType);
			}
			if ($Server::LAN)
			{
				%victim.delete("You have been kicked.");
			}
			else
			{
				%victim.delete("You have been kicked by " @ %client.getPlayerName() @ " (BLID: " @ %client.getBLID() @ ")");
			}
		}
	}
	else
	{
		%victim.delete('You have been kicked.');
	}
}

function serverCmdBan(%client, %victimID, %victimBL_ID, %banTime, %reason)
{
	if (%client != 0)
	{
		if (!%client.isAdmin)
		{
			return;
		}
	}
	if (%victimBL_ID $= "")
	{
		if (isObject(%client))
		{
			messageClient(%client, '', '\c5No victimBL_ID specified.');
		}
		return;
	}
	%victimID = mFloor(%victimID);
	%victimBL_ID = mFloor(%victimBL_ID);
	%banTime = mFloor(%banTime);
	%reason = StripMLControlChars(%reason);
	if (%victimBL_ID $= "0")
	{
		return;
	}
	%count = ClientGroup.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		%cl = ClientGroup.getObject(%i);
		if (%cl.getBLID() == %victimBL_ID)
		{
			if (%cl.isSuperAdmin)
			{
				if (isObject(%client))
				{
					messageClient(%client, '', '\c5You can\'t ban Super-Admins.');
				}
				else
				{
					echo("\c2You can't ban Super-Admins.");
				}
				return;
			}
			if (%cl.isLocal())
			{
				if (isObject(%client))
				{
					messageClient(%client, '', '\c5You can\'t ban the local client.');
				}
				else
				{
					echo("\c2You can't ban the local client.");
				}
				return;
			}
		}
	}
	if (%victimBL_ID $= getNumKeyID())
	{
		if (isObject(%client))
		{
			messageClient(%client, '', '\c5You can\'t ban the server owner.');
		}
		else
		{
			echo("\c2You can't ban the server owner.");
		}
		return;
	}
	if (isObject(%victimID))
	{
		%victimName = %victimID.getPlayerName();
	}
	else
	{
		%brickGroup = "BrickGroup_" @ %victimBL_ID;
		if (isObject(%brickGroup))
		{
			%victimName = %brickGroup.name;
		}
	}
	if (isObject(%client))
	{
		echo("BAN issued by ", %client.getPlayerName(), " BL_ID:", %client.getBLID(), " IP:", %client.getRawIP());
	}
	else
	{
		echo("BAN issued by CONSOLE");
	}
	echo("  +- victim name = ", %victimName);
	echo("  +- victim bl_id = ", %victimBL_ID);
	echo("  +- ban time = ", %banTime);
	echo("  +- ban reason = ", %reason);
	if (!isObject($BanManagerSO))
	{
		CreateBanManager();
	}
	if (isObject(%client))
	{
		%adminName = %client.getPlayerName();
	}
	else
	{
		%adminName = "CONSOLE";
	}
	$BanManagerSO.addBan(%client, %victimID, %victimBL_ID, %reason, %banTime);
	$BanManagerSO.saveBans();
	if (%banTime == -1)
	{
		MessageAll('MsgAdminForce', '\c3%1\c2 permanently banned \c3%2\c2 (ID: %3) - \c2"%4"', %adminName, %victimName, %victimBL_ID, %reason);
	}
	else
	{
		MessageAll('MsgAdminForce', '\c3%1\c2 banned \c3%2\c2 (ID: %3) for %4 minutes - \c2"%5"', %adminName, %victimName, %victimBL_ID, %banTime, %reason);
	}
	%count = ClientGroup.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		%cl = ClientGroup.getObject(%i);
		if (%cl.getBLID() == %victimBL_ID)
		{
			if (!$Server::LAN)
			{
				%cl.ClearEventSchedules();
				%cl.ClearEventObjects($TypeMasks::ProjectileObjectType);
			}
			if (isObject(%client))
			{
				if (%banTime == 1)
				{
					%cl.delete("\nYou have been banned for " @ %banTime @ " minute by " @ %client.getPlayerName() @ " (BLID: " @ %client.getBLID() @ ")\n\nReason: " @ %reason);
				}
				else
				{
					%cl.delete("\nYou have been banned for " @ %banTime @ " minutes by " @ %client.getPlayerName() @ " (BLID: " @ %client.getBLID() @ ")\n\nReason: " @ %reason);
				}
				continue;
			}
			if (%banTime == 1)
			{
				%cl.delete("\nYou have been banned for " @ %banTime @ " minute.\n\nReason: " @ %reason);
				continue;
			}
			%cl.delete("\nYou have been banned for " @ %banTime @ " minutes.\n\nReason: " @ %reason);
		}
	}
}

function CreateBanManager()
{
	if (isObject($BanManagerSO))
	{
		$BanManagerSO.delete();
	}
	$BanManagerSO = new ScriptObject(BanManagerSO)
	{
		numBans = 0;
	};
	$BanManagerSO.loadBans();
}

function BanManagerSO::addBan(%this, %adminID, %victimID, %victimBL_ID, %reason, %banTime)
{
	if (%victimBL_ID == 0)
	{
		return;
	}
	%this.RemoveBanBL_ID(%victimBL_ID);
	if (isObject(%adminID))
	{
		%adminName = %adminID.getPlayerName();
		%adminBL_ID = %adminID.getBLID();
	}
	else
	{
		%adminName = "CONSOLE";
		%adminBL_ID = -1;
	}
	if (isObject(%victimID))
	{
		%victimName = %victimID.getPlayerName();
	}
	else
	{
		%brickGroup = "BrickGroup_" @ %victimBL_ID;
		if (isObject(%brickGroup))
		{
			%victimName = %brickGroup.name;
		}
	}
	if (isObject(%victimID))
	{
		%victimIP = %victimID.getRawIP();
	}
	else
	{
		%victimIP = "";
	}
	%banOverYear = 0;
	%banOverTime = 0;
	if (%banTime == -1)
	{
		%banOverYear = -1;
		%banOverTime = -1;
	}
	else
	{
		%currTime = getCurrentMinuteOfYear();
		%banOverYear = getCurrentYear();
		%banOverTime = %currTime + %banTime;
		if (%banOverTime > 525600)
		{
			%banOverYear++;
			%banOverTime -= 525600;
		}
	}
	%i = %this.numBans;
	%this.adminName[%i] = %adminName;
	%this.adminBL_ID[%i] = %adminBL_ID;
	%this.victimName[%i] = %victimName;
	%this.victimBL_ID[%i] = %victimBL_ID;
	%this.victimIP[%i] = %victimIP;
	%this.Reason[%i] = %reason;
	%this.expirationYear[%i] = %banOverYear;
	%this.expirationMinute[%i] = %banOverTime;
	%this.numBans++;
}

function BanManagerSO::RemoveBanBL_ID(%this, %testBL_ID)
{
	for (%i = 0; %i < %this.numBans; %i++)
	{
		if (%this.victimBL_ID[%i] == %testBL_ID)
		{
			%this.removeBan(%i);
			return;
		}
	}
}

function BanManagerSO::removeBan(%this, %idx)
{
	if (%this.numBans <= %idx)
	{
		error("ERROR: BanManagerSO::RemoveBan() - ban index \"" @ %idx @ "\" does not exist. (there are only " @ %this.numBans @ " bans)");
		return;
	}
	for (%i = %idx + 1; %i < %this.numBans; %i++)
	{
		%this.adminName[%i - 1] = %this.adminName[%i];
		%this.adminBL_ID[%i - 1] = %this.adminBL_ID[%i];
		%this.victimName[%i - 1] = %this.victimName[%i];
		%this.victimBL_ID[%i - 1] = %this.victimBL_ID[%i];
		%this.victimIP[%i - 1] = %this.victimIP[%i];
		%this.Reason[%i - 1] = %this.Reason[%i];
		%this.expirationYear[%i - 1] = %this.expirationYear[%i];
		%this.expirationMinute[%i - 1] = %this.expirationMinute[%i];
	}
	%i = %this.numBans;
	%this.adminName[%i] = "";
	%this.adminBL_ID[%i] = "";
	%this.victimName[%i] = "";
	%this.victimBL_ID[%i] = "";
	%this.victimIP[%i] = "";
	%this.Reason[%i] = "";
	%this.expirationYear[%i] = "";
	%this.expirationMinute[%i] = "";
	%this.numBans--;
}

function BanManagerSO::saveBans(%this)
{
	%file = new FileObject();
	%file.openForWrite("config/server/BANLIST.txt");
	for (%i = 0; %i < %this.numBans; %i++)
	{
		%doLine = 1;
		if (%this.expirationMinute > 0)
		{
			%timeLeft = %this.expirationMinute[%i] - getCurrentMinuteOfYear();
			%timeLeft += (%this.expirationYear[%i] - getCurrentYear()) * 525600;
			if (%timeLeft <= 0)
			{
				%doLine = 0;
			}
		}
		if (%doLine)
		{
			%line = %this.adminName[%i];
			%line = %line TAB %this.adminBL_ID[%i];
			%line = %line TAB %this.victimName[%i];
			%line = %line TAB %this.victimBL_ID[%i];
			%line = %line TAB %this.victimIP[%i];
			%line = %line TAB %this.Reason[%i];
			%line = %line TAB %this.expirationYear[%i];
			%line = %line TAB %this.expirationMinute[%i];
			%file.writeLine(%line);
		}
	}
	%file.close();
	%file.delete();
}

function BanManagerSO::loadBans(%this)
{
	%this.numBans = 0;
	%file = new FileObject();
	for (%file.openForRead("config/server/BANLIST.txt"); !%file.isEOF(); %this.numBans++)
	{
		%line = %file.readLine();
		%i = %this.numBans;
		%this.adminName[%i] = getField(%line, 0);
		%this.adminBL_ID[%i] = getField(%line, 1);
		%this.victimName[%i] = getField(%line, 2);
		%this.victimBL_ID[%i] = getField(%line, 3);
		%this.victimIP[%i] = getField(%line, 4);
		%this.Reason[%i] = getField(%line, 5);
		%this.expirationYear[%i] = getField(%line, 6);
		%this.expirationMinute[%i] = getField(%line, 7);
	}
	%file.close();
	%file.delete();
}

function BanManagerSO::dumpBans(%this)
{
	echo(%this.numBans @ " Bans:");
	echo("-----------------------------------------------------------------");
	for (%i = 0; %i < %this.numBans; %i++)
	{
		%line = %this.adminName[%i];
		%line = %line TAB %this.adminBL_ID[%i];
		%line = %line TAB %this.victimName[%i];
		%line = %line TAB %this.victimBL_ID[%i];
		%line = %line TAB %this.victimIP[%i];
		%line = %line TAB %this.Reason[%i];
		%line = %line TAB %this.expirationYear[%i];
		%line = %line TAB %this.expirationMinute[%i];
		echo(%line);
	}
	echo("-----------------------------------------------------------------");
}

function BanManagerSO::sendBanList(%this, %client)
{
	for (%i = 0; %i < %this.numBans; %i++)
	{
		%line = %this.adminName[%i];
		%line = %line TAB %this.victimName[%i];
		%line = %line TAB %this.victimBL_ID[%i];
		%line = %line TAB %this.victimIP[%i];
		%line = %line TAB %this.Reason[%i];
		if (%this.expirationMinute[%i] == -1)
		{
			%line = %line TAB -1;
			commandToClient(%client, 'AddUnBanLine', %line, %i);
		}
		else
		{
			%timeLeft = %this.expirationMinute[%i] - getCurrentMinuteOfYear();
			%timeLeft += (%this.expirationYear[%i] - getCurrentYear()) * 525600;
			%line = %line TAB %timeLeft;
			if (%timeLeft > 0)
			{
				commandToClient(%client, 'AddUnBanLine', %line, %i);
			}
		}
	}
}

function BanManagerSO::isBanned(%this, %testBL_ID)
{
	if (%testBL_ID == 0)
	{
		return;
	}
	for (%i = 0; %i < %this.numBans; %i++)
	{
		if (%this.victimBL_ID[%i] == %testBL_ID)
		{
			if (%this.expirationYear[%i] == -1)
			{
				return "1" TAB %this.Reason[%i];
				continue;
			}
			if (%this.expirationYear[%i] < getCurrentYear())
			{
				echo("BanManagerSO::isBanned() - ban expired last year, removing");
				%this.removeBan(%i);
				%i--;
				continue;
			}
			if (%this.expirationYear[%i] == getCurrentYear())
			{
				if (%this.expirationMinute[%i] < getCurrentMinuteOfYear())
				{
					echo("BanManagerSO::isBanned() - ban expired, removing");
					%this.removeBan(%i);
					%i--;
				}
				else
				{
					return "1" TAB %this.Reason[%i];
				}
				continue;
			}
			return "1" TAB %this.Reason[%i];
		}
	}
	return 0;
}

function getCurrentYear()
{
	return mFloor(getSubStr(getDateTime(), 6, 2));
}

function getCurrentMinuteOfYear()
{
	%time = getDateTime();
	%month = mFloor(getSubStr(%time, 0, 2));
	%day = mFloor(getSubStr(%time, 3, 2));
	%year = mFloor(getSubStr(%time, 6, 2));
	%hour = mFloor(getSubStr(%time, 9, 2));
	%minute = mFloor(getSubStr(%time, 12, 2));
	%dayOfYear = getDayOfYear(%month, %day);
	%currTime = %minute;
	%currTime += %hour * 60;
	%currTime += %dayOfYear * 60 * 24;
	return %currTime;
}

function getLongNumberString(%val)
{
	%val = 1000003;
	echo("  val = ", %val % 10);
	%ret = "";
	for (%i = 0; %i < 100; %i++)
	{
		%ret = %val % 10 @ %ret;
		%val = mFloor(%val / 10);
		if (%val <= 0)
		{
			break;
		}
	}
	return %ret;
}

function serverCmdRequestBanList(%client)
{
	if (!%client.isAdmin)
	{
		return;
	}
	$BanManagerSO.sendBanList(%client);
}

function serverCmdUnBan(%client, %idx)
{
	if (!%client.isAdmin)
	{
		return;
	}
	$BanManagerSO.removeBan(%idx);
	$BanManagerSO.saveBans();
	$BanManagerSO.sendBanList(%client);
}

function serverCmdMagicWand(%client)
{
	if (!%client.isAdmin)
	{
		return;
	}
	%player = %client.Player;
	%player.updateArm(AdminWandImage);
	%player.mountImage(AdminWandImage, 0);
}

function serverCmdWand(%client)
{
	%mg = %client.miniGame;
	if (isObject(%mg))
	{
		if (!%mg.enableWand)
		{
			return;
		}
	}
	%player = %client.Player;
	if (!isObject(%player))
	{
		return;
	}
	%player.updateArm(WandImage);
	%player.mountImage(WandImage, 0);
}

function serverLoadAvatarNames()
{
	serverLoadAvatarName("decal", "base/data/shapes/player/decal.ifl");
	serverLoadAvatarName("hat", "base/data/shapes/player/hat.txt");
	serverLoadAccentInfo("accent", "base/data/shapes/player/accent.txt");
	serverLoadAvatarName("pack", "base/data/shapes/player/pack.txt");
	serverLoadAvatarName("secondPack", "base/data/shapes/player/secondPack.txt");
	serverLoadAvatarName("LLeg", "base/data/shapes/player/LLeg.txt");
	serverLoadAvatarName("RLeg", "base/data/shapes/player/RLeg.txt");
	serverLoadAvatarName("LHand", "base/data/shapes/player/LHand.txt");
	serverLoadAvatarName("RHand", "base/data/shapes/player/RHand.txt");
	serverLoadAvatarName("LArm", "base/data/shapes/player/LArm.txt");
	serverLoadAvatarName("RArm", "base/data/shapes/player/RArm.txt");
	serverLoadAvatarName("Chest", "base/data/shapes/player/Chest.txt");
	serverLoadAvatarName("Hip", "base/data/shapes/player/Hip.txt");
	$avatarNamesLoaded = 1;
}

function serverLoadAvatarName(%arrayName, %filename)
{
	%file = new FileObject();
	%file.openForRead(%filename);
	if (%file.isEOF())
	{
		%file.delete();
		return;
	}
	for (%lineCount = 0; !%file.isEOF(); %lineCount++)
	{
		%line = %file.readLine();
		%command = "$" @ %arrayName @ "[" @ %lineCount @ "] = \"" @ %line @ "\";";
		eval(%command);
	}
	$num[%arrayName] = %lineCount;
	%file.close();
	%file.delete();
}

function serverLoadAccentInfo(%arrayName, %filename)
{
	%file = new FileObject();
	%file.openForRead(%filename);
	if (%file.isEOF())
	{
		%file.delete();
		return;
	}
	%file.readLine();
	%line = %file.readLine();
	%wc = getWordCount(%line);
	for (%i = 0; %i < %wc; %i++)
	{
		%word = getWord(%line, %i);
		%command = "$" @ %arrayName @ "[" @ %i @ "] = " @ %word @ ";";
		eval(%command);
	}
	$num[%arrayName] = %wc;
	for (%lineCount = 0; !%file.isEOF(); %lineCount++)
	{
		%line = %file.readLine();
		%wc = getWordCount(%line);
		%hat = getWord(%line, 0);
		%allowed = getWords(%line, 1, %wc - 1);
		%command = "$" @ %arrayName @ "sAllowed[" @ %hat @ "] = \"" @ %allowed @ "\";";
		eval(%command);
	}
	%file.close();
	%file.delete();
}

function ServerCmdUpdateBodyParts(%client, %hat, %accent, %pack, %secondPack, %chest, %hip, %LLeg, %RLeg, %LArm, %RArm, %LHand, %RHand)
{
	%currTime = getSimTime();
	if (%client.lastUpdateBodyPartsTime + 1000 > %currTime)
	{
		return;
	}
	%client.lastUpdateBodyPartsTime = %currTime;
	%client.hat = mFloor(%hat);
	%client.accent = mFloor(%accent);
	%client.pack = mFloor(%pack);
	%client.secondPack = mFloor(%secondPack);
	%client.chest = mFloor(%chest);
	%client.hip = mFloor(%hip);
	%client.lleg = mFloor(%LLeg);
	%client.rleg = mFloor(%RLeg);
	%client.larm = mFloor(%LArm);
	%client.rarm = mFloor(%RArm);
	%client.lhand = mFloor(%LHand);
	%client.rhand = mFloor(%RHand);
	if ($Chest[%client.chest] $= "")
	{
		%client.chest = 0;
	}
	if ($Hip[%client.hip] $= "")
	{
		%client.hip = 0;
	}
	if ($LLeg[%client.lleg] $= "")
	{
		%client.lleg = 0;
	}
	if ($RLeg[%client.rleg] $= "")
	{
		%client.rleg = 0;
	}
	if ($LArm[%client.larm] $= "")
	{
		%client.larm = 0;
	}
	if ($RArm[%client.rarm] $= "")
	{
		%client.rarm = 0;
	}
	if ($LHand[%client.lhand] $= "")
	{
		%client.lhand = 0;
	}
	if ($RHand[%client.rhand] $= "")
	{
		%client.rhand = 0;
	}
	%client.applyBodyParts();
	%client.ApplyBodyColors();
}

function ServerCmdUpdateBodyColors(%client, %headColor, %hatColor, %accentColor, %packColor, %secondPackColor, %chestColor, %hipColor, %LLegColor, %RLegColor, %LArmColor, %RArmColor, %LHandColor, %RHandColor, %decalName, %faceName)
{
	%currTime = getSimTime();
	if (%client.lastUpdateBodyColorsTime + 1000 > %currTime)
	{
		return;
	}
	%client.lastUpdateBodyColorsTime = %currTime;
	%client.hatColor = AvatarColorCheck(%hatColor);
	%client.accentColor = AvatarColorCheckT(%accentColor);
	%client.packColor = AvatarColorCheck(%packColor);
	%client.secondPackColor = AvatarColorCheck(%secondPackColor);
	%client.headColor = AvatarColorCheck(%headColor);
	%client.chestColor = AvatarColorCheck(%chestColor);
	%client.hipColor = AvatarColorCheck(%hipColor);
	%client.llegColor = AvatarColorCheck(%LLegColor);
	%client.rlegColor = AvatarColorCheck(%RLegColor);
	%client.larmColor = AvatarColorCheck(%LArmColor);
	%client.rarmColor = AvatarColorCheck(%RArmColor);
	%client.lhandColor = AvatarColorCheck(%LHandColor);
	%client.rhandColor = AvatarColorCheck(%RHandColor);
	if (getCharCount(%decalName, "/") > 0)
	{
		%decalName = "AAA-None";
	}
	if (getCharCount(%faceName, "/") > 0)
	{
		%decalName = "smiley";
	}
	%client.decalName = %decalName;
	%client.faceName = %faceName;
	%client.applyBodyParts();
	%client.ApplyBodyColors();
}

function GameConnection::applyBodyParts(%client)
{
	%player = %client.Player;
	if (!isObject(%player))
	{
		return;
	}
	if (fileName(%player.getDataBlock().shapeFile) !$= "m.dts")
	{
		return;
	}
	%client.validateAvatarPrefs();
	hideAllNodes(%player);
	%accentList = $accentsAllowed[$hat[%client.hat]];
	%AccentName = getWord(%accentList, %client.accent);
	if (%AccentName !$= "none")
	{
		%player.unHideNode(%AccentName);
	}
	%player.unHideNode($hat[%client.hat]);
	%player.unHideNode($Chest[%client.chest]);
	if ($pack[%client.pack] !$= "none")
	{
		%player.unHideNode($pack[%client.pack]);
	}
	if ($SecondPack[%client.secondPack] !$= "none")
	{
		%player.unHideNode($SecondPack[%client.secondPack]);
	}
	if ($Hip[%client.hip] $= "SkirtHip")
	{
		%player.unHideNode(skirtHip);
		%player.unHideNode(skirtTrimLeft);
		%player.unHideNode(skirtTrimRight);
	}
	else
	{
		%player.unHideNode($Hip[%client.hip]);
		%player.unHideNode($LLeg[%client.lleg]);
		%player.unHideNode($RLeg[%client.rleg]);
	}
	%player.unHideNode($LArm[%client.larm]);
	%player.unHideNode($RArm[%client.rarm]);
	%player.unHideNode($LHand[%client.lhand]);
	%player.unHideNode($RHand[%client.rhand]);
	if (%client.pack == 0 && %client.secondPack == 0)
	{
		%player.setHeadUp(0);
	}
	else
	{
		%player.setHeadUp(1);
	}
	%vehicle = %player.getObjectMount();
	if (%vehicle)
	{
		if (isObject(skiVehicle))
		{
			if (%vehicle.getDataBlock().getId() == skiVehicle.getId())
			{
				%player.unHideNode(lski);
				%player.unHideNode(rski);
				%color = getColorIDTable(%client.currentColor);
				%player.setNodeColor("LSki", %color);
				%player.setNodeColor("RSki", %color);
			}
		}
	}
}

function GameConnection::ApplyBodyColors(%client)
{
	%player = %client.Player;
	if (!isObject(%player))
	{
		return;
	}
	if (fileName(%player.getDataBlock().shapeFile) $= "m.dts")
	{
	}
	else if (fileName(%player.getDataBlock().shapeFile) $= "horse.dts")
	{
		%player.setNodeColor("body", %client.chestColor);
		%player.setNodeColor("head", "0 0 0 1");
		return;
	}
	else
	{
		%player.setNodeColor("ALL", %client.chestColor);
		return;
	}
	%accentList = $accentsAllowed[$hat[%client.hat]];
	%AccentName = getWord(%accentList, %client.accent);
	if (%AccentName !$= "none" && %AccentName !$= "")
	{
		%player.setNodeColor(%AccentName, %client.accentColor);
	}
	%hatName = $hat[%client.hat];
	if (%hatName !$= "none")
	{
		%player.setNodeColor(%hatName, %client.hatColor);
	}
	%player.setNodeColor("headSkin", %client.headColor);
	%packName = $pack[%client.pack];
	if (%packName !$= "none")
	{
		%player.setNodeColor(%packName, %client.packColor);
	}
	%secondPackName = $SecondPack[%client.secondPack];
	if (%secondPackName !$= "none")
	{
		%player.setNodeColor(%secondPackName, %client.secondPackColor);
	}
	%chestName = $Chest[%client.chest];
	if (%chestName !$= "none")
	{
		%player.setNodeColor(%chestName, %client.chestColor);
	}
	%hipName = $Hip[%client.hip];
	if (%hipName !$= "none")
	{
		%player.setNodeColor(%hipName, %client.hipColor);
	}
	%LLegName = $LLeg[%client.lleg];
	%RLegName = $RLeg[%client.rleg];
	if (%hipName $= "SkirtHip")
	{
		%player.setNodeColor("SkirtTrimLeft", %client.llegColor);
		%player.setNodeColor("SkirtTrimRight", %client.rlegColor);
	}
	else
	{
		if (%LLegName !$= "none")
		{
			%player.setNodeColor(%LLegName, %client.llegColor);
		}
		if (%RLegName !$= "none")
		{
			%player.setNodeColor(%RLegName, %client.rlegColor);
		}
	}
	%LArmName = $LArm[%client.larm];
	if (%LArmName !$= "none")
	{
		%player.setNodeColor(%LArmName, %client.larmColor);
	}
	%RArmName = $RArm[%client.rarm];
	if (%RArmName !$= "none")
	{
		%player.setNodeColor(%RArmName, %client.rarmColor);
	}
	%LHandName = $LHand[%client.lhand];
	if (%LHandName !$= "none")
	{
		%player.setNodeColor(%LHandName, %client.lhandColor);
	}
	%RHandName = $RHand[%client.rhand];
	if (%RHandName !$= "none")
	{
		%player.setNodeColor(%RHandName, %client.rhandColor);
	}
	%color = getColorIDTable(%client.currentColor);
	%player.setNodeColor(lski, %color);
	%player.setNodeColor(rski, %color);
	%player.setFaceName(%client.faceName);
	%player.setDecalName(%client.decalName);
}

function AvatarColorCheck(%color)
{
	%r = mFloor(getWord(%color, 0) * 1000) / 1000;
	%g = mFloor(getWord(%color, 1) * 1000) / 1000;
	%b = mFloor(getWord(%color, 2) * 1000) / 1000;
	%a = 1;
	if (%r <= 0)
	{
		%r = 0;
	}
	if (%g <= 0)
	{
		%g = 0;
	}
	if (%b <= 0)
	{
		%b = 0;
	}
	if (%a <= 0.2)
	{
		%a = 0.2;
	}
	if (%r > 1)
	{
		%r = 1;
	}
	if (%g > 1)
	{
		%g = 1;
	}
	if (%b > 1)
	{
		%b = 1;
	}
	if (%a > 1)
	{
		%a = 1;
	}
	return %r SPC %g SPC %b SPC %a;
}

function AvatarColorCheckT(%color)
{
	%r = getWord(%color, 0);
	%g = getWord(%color, 1);
	%b = getWord(%color, 2);
	%a = getWord(%color, 3);
	if (%r <= 0)
	{
		%r = 0;
	}
	if (%g <= 0)
	{
		%g = 0;
	}
	if (%b <= 0)
	{
		%b = 0;
	}
	if (%a <= 0.2)
	{
		%a = 0.2;
	}
	if (%r > 1)
	{
		%r = 1;
	}
	if (%g > 1)
	{
		%g = 1;
	}
	if (%b > 1)
	{
		%b = 1;
	}
	if (%a > 1)
	{
		%a = 1;
	}
	return %r SPC %g SPC %b SPC %a;
}

function applyCharacterPrefs(%client)
{
	if (!isObject(%client.Player))
	{
		return;
	}
	%client.applyBodyParts();
	%client.ApplyBodyColors();
}

function applyDefaultCharacterPrefs(%player)
{
	if (!isObject(%player))
	{
		return;
	}
	if (fileName(%player.getDataBlock().shapeFile) !$= "m.dts")
	{
		return;
	}
	hideAllNodes(%player);
	%player.unHideNode($Chest[0]);
	%player.unHideNode($Hip[0]);
	%player.unHideNode($LLeg[0]);
	%player.unHideNode($RLeg[0]);
	%player.unHideNode($LArm[0]);
	%player.unHideNode($RArm[0]);
	%player.unHideNode($LHand[0]);
	%player.unHideNode($RHand[0]);
	%player.setHeadUp(0);
	%mainColor = "";
	if (isObject(%player.spawnBrick))
	{
		if (isObject(%player.spawnBrick.VehicleSpawnMarker))
		{
			if (%player.spawnBrick.VehicleSpawnMarker.reColorVehicle)
			{
				%mainColor = getColorIDTable(%player.spawnBrick.colorID);
			}
		}
	}
	if (%mainColor $= "")
	{
		%mainColor = %player.color;
	}
	if (%mainColor $= "")
	{
		%mainColor = "1 1 1 1";
	}
	%mainColor = getWords(%mainColor, 0, 2) SPC "1";
	%player.setNodeColor("headSkin", "1 1 0 1");
	%player.setNodeColor($Chest[0], %mainColor);
	%player.setNodeColor($Hip[0], "0 0 1 1");
	%player.setNodeColor($LLeg[0], "0.1 0.1 0.1 1");
	%player.setNodeColor($RLeg[0], "0.1 0.1 0.1 1");
	%player.setNodeColor($LArm[0], %mainColor);
	%player.setNodeColor($RArm[0], %mainColor);
	%player.setNodeColor($LHand[0], "1 1 0 1");
	%player.setNodeColor($RHand[0], "1 1 0 1");
	%player.setNodeColor("headSkin", "1 1 0 1");
}

function hideAllNodes(%player)
{
	%player.hideNode(lski);
	%player.hideNode(rski);
	%player.hideNode(skirtTrimLeft);
	%player.hideNode(skirtTrimRight);
	for (%i = 0; %i < $num["Hat"]; %i++)
	{
		if ($hat[%i] !$= "None")
		{
			%player.hideNode($hat[%i]);
		}
	}
	for (%i = 0; %i < $num["Accent"]; %i++)
	{
		if ($Accent[%i] !$= "None")
		{
			%player.hideNode($Accent[%i]);
		}
	}
	for (%i = 0; %i < $num["Pack"]; %i++)
	{
		if ($pack[%i] !$= "None")
		{
			%player.hideNode($pack[%i]);
		}
	}
	for (%i = 0; %i < $num["SecondPack"]; %i++)
	{
		if ($SecondPack[%i] !$= "None")
		{
			%player.hideNode($SecondPack[%i]);
		}
	}
	for (%i = 0; %i < $num["Chest"]; %i++)
	{
		if ($Chest[%i] !$= "None")
		{
			%player.hideNode($Chest[%i]);
		}
	}
	for (%i = 0; %i < $num["Hip"]; %i++)
	{
		if ($Hip[%i] !$= "None")
		{
			%player.hideNode($Hip[%i]);
		}
	}
	for (%i = 0; %i < $num["RArm"]; %i++)
	{
		if ($RArm[%i] !$= "None")
		{
			%player.hideNode($RArm[%i]);
		}
	}
	for (%i = 0; %i < $num["LArm"]; %i++)
	{
		if ($LArm[%i] !$= "None")
		{
			%player.hideNode($LArm[%i]);
		}
	}
	for (%i = 0; %i < $num["RHand"]; %i++)
	{
		if ($RHand[%i] !$= "None")
		{
			%player.hideNode($RHand[%i]);
		}
	}
	for (%i = 0; %i < $num["LHand"]; %i++)
	{
		if ($LHand[%i] !$= "None")
		{
			%player.hideNode($LHand[%i]);
		}
	}
	for (%i = 0; %i < $num["RLeg"]; %i++)
	{
		if ($RLeg[%i] !$= "None")
		{
			%player.hideNode($RLeg[%i]);
		}
	}
	for (%i = 0; %i < $num["LLeg"]; %i++)
	{
		if ($LLeg[%i] !$= "None")
		{
			%player.hideNode($LLeg[%i]);
		}
	}
}

function GameConnection::validateAvatarPrefs(%client)
{
	%client.hat = mFloor(%client.hat);
	%client.accent = mFloor(%client.accent);
	%client.pack = mFloor(%client.pack);
	%client.secondPack = mFloor(%client.secondPack);
	%client.chest = mFloor(%client.chest);
	%client.hip = mFloor(%client.hip);
	%client.lleg = mFloor(%client.lleg);
	%client.rleg = mFloor(%client.rleg);
	%client.larm = mFloor(%client.larm);
	%client.rarm = mFloor(%client.rarm);
	%client.lhand = mFloor(%client.lhand);
	%client.rhand = mFloor(%client.rhand);
	if ($Chest[%client.chest] $= "")
	{
		%client.chest = 0;
	}
	if ($Hip[%client.hip] $= "")
	{
		%client.hip = 0;
	}
	if ($LLeg[%client.lleg] $= "")
	{
		%client.lleg = 0;
	}
	if ($RLeg[%client.rleg] $= "")
	{
		%client.rleg = 0;
	}
	if ($LArm[%client.larm] $= "")
	{
		%client.larm = 0;
	}
	if ($RArm[%client.rarm] $= "")
	{
		%client.rarm = 0;
	}
	if ($LHand[%client.lhand] $= "")
	{
		%client.lhand = 0;
	}
	if ($RHand[%client.rhand] $= "")
	{
		%client.rhand = 0;
	}
	%client.hatColor = AvatarColorCheck(%client.hatColor);
	%client.accentColor = AvatarColorCheckT(%client.accentColor);
	%client.packColor = AvatarColorCheck(%client.packColor);
	%client.secondPackColor = AvatarColorCheck(%client.secondPackColor);
	%client.headColor = AvatarColorCheck(%client.headColor);
	%client.chestColor = AvatarColorCheck(%client.chestColor);
	%client.hipColor = AvatarColorCheck(%client.hipColor);
	%client.llegColor = AvatarColorCheck(%client.llegColor);
	%client.rlegColor = AvatarColorCheck(%client.rlegColor);
	%client.larmColor = AvatarColorCheck(%client.larmColor);
	%client.rarmColor = AvatarColorCheck(%client.rarmColor);
	%client.lhandColor = AvatarColorCheck(%client.lhandColor);
	%client.rhandColor = AvatarColorCheck(%client.rhandColor);
}

function serverCmdChangeMap(%client, %mapName)
{
	if (!$missionRunning)
	{
		return;
	}
	if ($LoadingBricks_Client && $LoadingBricks_Client != 1)
	{
		ServerLoadSaveFile_End();
	}
	if (!%client.isAdmin && %client != 0)
	{
		return;
	}
	%filename = %mapName;
	if (!isFile(%filename))
	{
		return;
	}
	if (fileExt(%filename) !$= ".mis")
	{
		return;
	}
	%path = filePath(%mapName);
	%dirName = getSubStr(%path, strlen("Add-Ons/Map_"), strlen(%path) - strlen("Add-Ons/Map_"));
	MessageAll('MsgProcessComplete', '\c3%1 \c0changed the map to %2', %client.getPlayerName(), %dirName);
	echo(%client.getPlayerName() @ " changed the map to " @ %dirName);
	MessageAll('', 'Mission cleanup in progress...');
	%count = mainBrickGroup.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		%brickGroup = mainBrickGroup.getObject(%i);
		%quotaObject = %brickGroup.QuotaObject;
		if (!isObject(%quotaObject))
		{
		}
		else
		{
			cancelQuotaSchedules(%quotaObject);
		}
	}
	MissionCleanup.chainDeleteCallBack = "schedule(10, 0, changeMap, \"" @ %mapName @ "\");";
	MissionCleanup.chainDeleteAll();
	$Game::MissionCleaningUp = 1;
	setRaytracerAutoCenter();
	stopRaytracer();
}

function generateMapList()
{
	deleteVariables("$mapList*");
	$mapListCount = 0;
	%i = 0;
	for (%file = findFirstFile($Server::MissionFileSpec); %file !$= ""; %file = findNextFile($Server::MissionFileSpec))
	{
		if (!isValidMap(%file))
		{
		}
		else
		{
			%mapEntry = serverGetMissionDisplayName(%file) @ "\t" @ serverGetMissionPreviewImage(%file) @ "\t" @ %file;
			$mapList[$mapListCount] = %mapEntry;
			$mapListCount++;
		}
	}
}

function serverCmdGetMapList(%client)
{
	if (!%client.isAdmin)
	{
		return;
	}
	if (getSimTime() - %client.lastGetMapListTime < 5000)
	{
		return;
	}
	%client.lastGetMapListTime = getSimTime();
	echo("Sending map list to " @ %client.getPlayerName());
	if ($mapListCount <= 0)
	{
		generateMapList();
	}
	for (%i = 0; %i < $mapListCount; %i++)
	{
		commandToClient(%client, 'updateMapList', $mapList[%i]);
	}
}

function serverGetMissionDisplayName(%missionFile)
{
	%file = new FileObject();
	%MissionInfoObject = "";
	if (%file.openForRead(%missionFile))
	{
		%inInfoBlock = 0;
		while (!%file.isEOF())
		{
			%line = %file.readLine();
			%line = trim(%line);
			if (%line $= "new ScriptObject(MissionInfo) {")
			{
				%inInfoBlock = 1;
			}
			else if (%inInfoBlock && %line $= "};")
			{
				%inInfoBlock = 0;
				%MissionInfoObject = %MissionInfoObject @ %line;
				break;
			}
			if (%inInfoBlock)
			{
				%MissionInfoObject = %MissionInfoObject @ %line @ " ";
			}
		}
		%file.close();
	}
	%MissionInfoObject = "%MissionInfoObject = " @ %MissionInfoObject;
	eval(%MissionInfoObject);
	%file.delete();
	%missionName = %MissionInfoObject.name;
	%MissionInfoObject.delete();
	if (%missionName !$= "")
	{
		return %missionName;
	}
	else
	{
		return fileBase(%missionFile);
	}
}

function serverGetMissionPreviewImage(%missionFile)
{
	%path = filePath(%missionFile);
	%name = fileBase(%missionFile);
	%previewJPG = %path @ "/" @ %name @ ".jpg";
	%previewPNG = %path @ "/" @ %name @ ".png";
	%previewImage = %previewPNG;
	if (!isFile(%previewImage))
	{
		%previewImage = %previewJPG;
	}
	if (!isFile(%previewImage))
	{
		%previewImage = "";
	}
	return %previewImage;
}

function ServerCmdListAllDataBlocks(%client)
{
	if (!%client.isAdmin)
	{
		return;
	}
	%numDataBlocks = getDataBlockGroupSize();
	echo("SERVER SAYS, ", %numDataBlocks, " DataBlocks");
	echo("--------------------------");
	for (%i = 0; %i < %numDataBlocks; %i++)
	{
		%db = getDataBlock(%i);
		%dbClass = %db.getClassName();
		echo(%db, " : ", %dbClass);
	}
	echo("--------------------------");
}

function serverCmdGetID(%client)
{
	if (!%client.isAdmin)
	{
		return;
	}
	%player = %client.getControlObject();
	%start = %player.getEyePoint();
	%eyeVec = %player.getEyeVector();
	%vector = VectorScale(%eyeVec, 100);
	%end = VectorAdd(%start, %vector);
	%mask = $TypeMasks::InteriorObjectType | $TypeMasks::PlayerObjectType | $TypeMasks::StaticShapeObjectType | $TypeMasks::StaticObjectType | $TypeMasks::FxBrickAlwaysObjectType | $TypeMasks::VehicleObjectType;
	%scanTarg = containerRayCast(%start, %end, %mask, %player);
	if (%scanTarg)
	{
		%pos = posFromRaycast(%scanTarg);
		%vec = VectorSub(%pos, %start);
		%dist = VectorLen(%vec);
		%scanObj = getWord(%scanTarg, 0);
		messageClient(%client, '', "objectid = " @ %scanObj @ "  classname = " @ %scanObj.getClassName() @ " distance = " @ %dist);
	}
}

function serverCmdGetTransform(%client)
{
	if (!%client.isAdmin)
	{
		return;
	}
	%player = %client.getControlObject();
	%start = %player.getEyePoint();
	%eyeVec = %player.getEyeVector();
	%vector = VectorScale(%eyeVec, 100);
	%end = VectorAdd(%start, %vector);
	%mask = $TypeMasks::InteriorObjectType | $TypeMasks::PlayerObjectType | $TypeMasks::StaticShapeObjectType | $TypeMasks::StaticObjectType | $TypeMasks::FxBrickAlwaysObjectType | $TypeMasks::VehicleObjectType;
	%scanTarg = containerRayCast(%start, %end, %mask, %player);
	if (%scanTarg)
	{
		%pos = posFromRaycast(%scanTarg);
		%vec = VectorSub(%pos, %start);
		%dist = VectorLen(%vec);
		%scanObj = getWord(%scanTarg, 0);
		messageClient(%client, '', %scanObj.getTransform());
	}
}

function findClientByName(%partialName)
{
	%pnLen = strlen(%partialName);
	%clientIndex = 0;
	%bestCL = -1;
	%bestPos = 9999;
	for (%clientIndex = 0; %clientIndex < ClientGroup.getCount(); %clientIndex++)
	{
		%cl = ClientGroup.getObject(%clientIndex);
		%pos = -1;
		%name = strlwr(%cl.getPlayerName());
		%pos = strstr(%name, strlwr(%partialName));
		if (%pos != -1)
		{
			%bestCL = %cl;
			if (%pos == 0)
			{
				return %cl;
			}
			if (%pos < %bestPos)
			{
				%bestPos = %pos;
				%bestCL = %cl;
			}
		}
	}
	if (%bestCL != -1)
	{
		return %bestCL;
	}
	else
	{
		return 0;
	}
}

function serverCmdFetch(%client, %victimName)
{
	if (!%client.isAdmin)
	{
		return;
	}
	%player = %client.Player;
	if (!isObject(%player))
	{
		return;
	}
	%victimClient = findClientByName(%victimName);
	if (%victimClient)
	{
		%victimPlayer = %victimClient.Player;
		if (isObject(%victimPlayer))
		{
			%client.lastF8Time = getSimTime();
			%player.teleportEffect();
			if (!%victimPlayer.isMounted())
			{
				%victimPlayer.setTransform(%player.getTransform());
				%victimPlayer.setVelocity("0 0 0");
			}
			else
			{
				%mount = %victimPlayer;
				for (%i = 0; %i < 100; %i++)
				{
					if (!%mount.isMounted())
					{
						break;
					}
					%mount = %mount.getObjectMount();
				}
				if (%mount.getClassName() $= "Player" || %mount.getClassName() $= "AIPlayer" || %mount.getClassName() $= "WheeledVehicle" || %mount.getClassName() $= "FlyingVehicle" || %mount.getClassName() $= "HoverVehicle")
				{
					%mount.setTransform(%player.getTransform());
					%mount.setVelocity("0 0 0");
					%mount.teleportEffect();
				}
			}
		}
	}
}

function serverCmdFind(%client, %victimName)
{
	if (!%client.isAdmin)
	{
		return;
	}
	%player = %client.Player;
	if (!isObject(%player))
	{
		return;
	}
	%victimClient = findClientByName(%victimName);
	if (%victimClient)
	{
		%victimPlayer = %victimClient.Player;
		if (isObject(%victimPlayer))
		{
			%client.lastF8Time = getSimTime();
			if (!%player.isMounted())
			{
				%player.setTransform(%victimPlayer.getTransform());
				%player.setVelocity("0 0 0");
				%player.teleportEffect();
			}
			else
			{
				%mount = %player;
				for (%i = 0; %i < 100; %i++)
				{
					if (!%mount.isMounted())
					{
						break;
					}
					%mount = %mount.getObjectMount();
				}
				if (%mount.getClassName() $= "Player" || %mount.getClassName() $= "AIPlayer" || %mount.getClassName() $= "WheeledVehicle" || %mount.getClassName() $= "FlyingVehicle" || %mount.getClassName() $= "HoverVehicle")
				{
					%mount.setTransform(%victimPlayer.getTransform());
					%mount.setVelocity("0 0 0");
					%mount.teleportEffect();
				}
			}
		}
	}
}

function serverCmdRet(%client)
{
	if (!%client.isAdmin)
	{
		return;
	}
	%client.isSpying = 0;
	%client.setControlObject(%client.Player);
}

function serverCmdSpy(%client, %victimName)
{
	if (!%client.isAdmin)
	{
		return;
	}
	if (mFloor(%victimName) > 0)
	{
		if (isObject(%victimName.Player))
		{
			%client.Camera.setMode("Corpse", %victimName.Player);
			%client.setControlObject(%client.Camera);
			%client.isSpying = 1;
		}
		else
		{
			messageClient(%client, '', "Client does not have a player object");
		}
	}
	else
	{
		%victimClient = findClientByName(%victimName);
		if (%victimClient)
		{
			%victimPlayer = %victimClient.Player;
			if (isObject(%victimPlayer))
			{
				%client.Camera.setMode("Corpse", %victimPlayer);
				%client.setControlObject(%client.Camera);
				%client.isSpying = 1;
			}
			else
			{
				messageClient(%client, '', "Client does not have a player object");
			}
		}
		else
		{
			messageClient(%client, '', "Client not found");
		}
	}
}

function serverCmdWarp(%client)
{
	if (!%client.isAdmin)
	{
		return;
	}
	%player = %client.Player;
	if (!isObject(%player))
	{
		return;
	}
	%start = %player.getEyePoint();
	%eyeVec = %player.getEyeVector();
	%vector = VectorScale(%eyeVec, 1000);
	%end = VectorAdd(%start, %vector);
	%searchMasks = $TypeMasks::TerrainObjectType | $TypeMasks::InteriorObjectType | $TypeMasks::FxBrickObjectType;
	%scanTarg = containerRayCast(%start, %end, %searchMasks, %player);
	if (%scanTarg)
	{
		%scanObj = getWord(%scanTarg, 0);
		%scanPos = getWords(%scanTarg, 1, 3);
		%player.setVelocity("0 0 0");
		%player.setTransform(%scanPos);
		%player.teleportEffect();
	}
}

function serverCmdTimeScale(%client, %val)
{
	if (!%client.isAdmin)
	{
		return;
	}
	%val = mClampF(%val, 0.2, 2);
	setTimeScale(19038, %val);
	for (%clientIndex = 0; %clientIndex < ClientGroup.getCount(); %clientIndex++)
	{
		%cl = ClientGroup.getObject(%clientIndex);
		commandToClient(%cl, 'TimeScale', %val);
	}
	MessageAll('', %client.getPlayerName() @ " changed the timescale to " @ %val);
	WebCom_PostServerUpdateLoop();
	pingMatchMakerLoop();
}

function serverCmdRealBrickCount(%client)
{
	if (!%client.isAdmin)
	{
		return;
	}
	%count = MissionCleanup.getCount();
	%brickCount = 0;
	for (%i = 0; %i < %count; %i++)
	{
		%obj = MissionCleanup.getObject(%i);
		if (%obj.getClassName() $= "fxDTSBrick")
		{
			if (%obj.isPlanted)
			{
				%brickCount++;
			}
		}
	}
	if (%brickCount == 1)
	{
		messageClient(%client, '', %brickCount @ " brick");
	}
	else
	{
		messageClient(%client, '', %brickCount @ " bricks");
	}
}

function serverCmdBrickCount(%client)
{
	if ($Server::BrickCount == 1)
	{
		messageClient(%client, '', $Server::BrickCount @ " brick");
	}
	else
	{
		messageClient(%client, '', $Server::BrickCount @ " bricks");
	}
}

function serverCmdTripOut(%client)
{
	if (!%client.isAdmin)
	{
		return;
	}
	%count = MissionCleanup.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		%obj = MissionCleanup.getObject(%i);
		if (%obj.getClassName() $= "fxDTSBrick")
		{
			%obj.setColorFX(6);
			%obj.setShapeFX(1);
		}
	}
}

function serverCmdLight(%client)
{
	%player = %client.Player;
	if (!isObject(%player))
	{
		return;
	}
	if (%player.getDamagePercent() >= 1)
	{
		return;
	}
	if (isObject(%player.light))
	{
		%player.light.delete();
		ServerPlay3D(lightOffSound, %player.getPosition());
		%player.playAudio(0, lightOff);
	}
	else
	{
		%player.light = new fxLight()
		{
			dataBlock = PlayerLight;
		};
		MissionCleanup.add(%player.light);
		%player.light.setTransform(%player.getTransform());
		%player.light.attachToObject(%player);
		%player.light.Player = %player;
		ServerPlay3D(lightOnSound, %player.getPosition());
		%player.playAudio(0, LightOn);
	}
}

function serverCmdColorTest(%client)
{
	if (!%client.isAdmin)
	{
		return;
	}
	MessageAll('', '\c00\c11\c22\c33\c44\c55\c66\c77\c88');
}

function serverCmdDropPlayerAtCamera(%client)
{
	if (!%client.isAdmin)
	{
		return;
	}
	%client.Camera.unmountImage(0);
	%player = %client.Player;
	if (isObject(%player))
	{
		if (%player.getDamagePercent() < 1)
		{
			if (isObject(%client.miniGame))
			{
				%client.incScore(-1);
			}
			%client.lastF8Time = getSimTime();
			if (!%player.isMounted())
			{
				%pos = getWords(%client.Camera.getTransform(), 0, 2);
				%rot = getWords(%client.Camera.getTransform(), 3, 7);
				%offset = VectorSub(%player.getEyePoint(), %player.getPosition());
				%start = %pos;
				%end = VectorSub(%pos, %offset);
				%mask = $TypeMasks::InteriorObjectType | $TypeMasks::TerrainObjectType | $TypeMasks::FxBrickObjectType;
				%raycast = containerRayCast(%start, %end, %mask);
				if (%raycast)
				{
					%pos = posFromRaycast(%raycast);
				}
				else
				{
					%pos = VectorSub(%pos, %offset);
				}
				%player.setTransform(%pos SPC %rot);
				%player.setVelocity("0 0 0");
				%client.setControlObject(%player);
				%player.teleportEffect();
			}
			else
			{
				%mount = %player;
				for (%i = 0; %i < 100; %i++)
				{
					if (!%mount.isMounted())
					{
						break;
					}
					%mount = %mount.getObjectMount();
				}
				if (%mount.getClassName() $= "Player" || %mount.getClassName() $= "AIPlayer" || %mount.getClassName() $= "WheeledVehicle" || %mount.getClassName() $= "FlyingVehicle" || %mount.getClassName() $= "HoverVehicle")
				{
					%mount.setTransform(%client.Camera.getTransform());
					if (%mount.getType() & $TypeMasks::VehicleObjectType)
					{
						%mount.setAngularVelocity("0 0 0");
					}
					%mount.setVelocity("0 0 0");
					%mount.teleportEffect();
					%client.setControlObject(%player);
				}
			}
		}
		else
		{
			%client.spawnPlayer();
		}
	}
	else
	{
		%client.spawnPlayer();
	}
}

function serverCmdDropCameraAtPlayer(%client)
{
	if (!%client.isAdmin)
	{
		return;
	}
	if (isObject(%client.Player))
	{
		%client.Camera.setTransform(%client.Player.getEyeTransform());
		%client.Camera.setVelocity("0 0 0");
	}
	%client.setControlObject(%client.Camera);
	%client.Camera.mountImage(cameraImage, 0);
	%client.Camera.setMode("Observer");
}

function serverCmdSuicide(%client)
{
	if (isObject(%client.Player))
	{
		%client.Player.kill($DamageType::Suicide);
	}
}

function ServerCmdSetFocalPoint(%client)
{
	%start = %client.getControlObject().getEyePoint();
	%vec = %client.getControlObject().getEyeVector();
	%end = VectorAdd(%start, VectorScale(%vec, 100));
	%mask = $TypeMasks::InteriorObjectType | $TypeMasks::StaticObjectType | $TypeMasks::PlayerObjectType | $TypeMasks::FxBrickObjectType | $TypeMasks::TerrainObjectType;
	%search = containerRayCast(%start, %end, %mask, %client.getControlObject());
	%point = getWords(%search, 1, 3);
	commandToClient(%client, 'SetFocalPoint', %point);
}

function ServerCmdClearBricks(%client, %confirm)
{
	if (%server::Lan)
	{
		return;
	}
	if ($Game::MissionCleaningUp)
	{
		messageClient(%client, '', 'Can\'t clear bricks during mission clean up');
		return;
	}
	if (getSimTime() - %client.lastClearBricksTime < 5000)
	{
		return;
	}
	%brickGroup = %client.brickGroup;
	if (!isObject(%brickGroup))
	{
		return;
	}
	if (%brickGroup.getCount() <= 0)
	{
		return;
	}
	%client.lastClearBricksTime = getSimTime();
	MessageAll('MsgClearBricks', '\c3%1\c2 cleared \c3%2\c2\'s bricks', %client.getPlayerName(), %brickGroup.name);
	%brickGroup.chainDeleteAll();
}

function serverCmdCancelAllEvents(%client)
{
	if (!%client.isAdmin)
	{
		return;
	}
	MessageAll('', "\c3" @ %client.getPlayerName() @ "\c0 canceled all events.");
	%count = mainBrickGroup.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		%brickGroup = mainBrickGroup.getObject(%i);
		%quotaObject = %brickGroup.QuotaObject;
		if (!isObject(%quotaObject))
		{
		}
		else
		{
			cancelQuotaSchedules(%quotaObject);
			if (isObject(%brickGroup.client))
			{
				%brickGroup.client.resetVehicles();
			}
			%mask = $TypeMasks::PlayerObjectType | $TypeMasks::ProjectileObjectType | $TypeMasks::VehicleObjectType | $TypeMasks::CorpseObjectType;
			%quotaObject.killObjects(%mask);
		}
	}
}

function serverCmdCancelEvents(%client)
{
	if (isObject(%client.miniGame))
	{
		if (%client.miniGame.owner != %client)
		{
			messageClient(%client, '', "CancelEvents is not allowed while in a minigame.");
			return;
		}
	}
	if ($Server::LAN)
	{
		if (!%client.isAdmin)
		{
			return;
		}
	}
	%elapsedTime = getSimTime() - %client.lastCancelEventTime;
	if (%elapsedTime < 5000)
	{
		messageClient(%client, '', "You must wait " @ mCeil((5000 - %elapsedTime) / 1000) @ " seconds.");
		return;
	}
	%client.lastCancelEventTime = getSimTime();
	messageClient(%client, '', "Deleting all events and event-spawned objects...");
	%client.ClearEventSchedules();
	%client.resetVehicles();
	%mask = $TypeMasks::PlayerObjectType | $TypeMasks::ProjectileObjectType | $TypeMasks::VehicleObjectType | $TypeMasks::CorpseObjectType;
	%client.ClearEventObjects(%mask);
}

function GameConnection::ClearEventSchedules(%client)
{
	if (!isObject(%client.brickGroup))
	{
		return;
	}
	%quotaObject = %client.brickGroup.QuotaObject;
	if (!isObject(%quotaObject))
	{
		return;
	}
	cancelQuotaSchedules(%quotaObject);
}

function GameConnection::ClearEventObjects(%client, %mask)
{
	if (!isObject(%client.brickGroup))
	{
		return;
	}
	%quotaObject = %client.brickGroup.QuotaObject;
	if (!isObject(%quotaObject))
	{
		return;
	}
	if (%mask $= "")
	{
		%quotaObject.killObjects();
	}
	else
	{
		%quotaObject.killObjects(%mask);
	}
}

function ServerCmdResetVehicles(%client)
{
	if (!%client.isAdmin)
	{
		return;
	}
	MessageAll('', "\c3" @ %client.getPlayerName() @ "\c0 reset all vehicles.");
	%count = MissionCleanup.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		%obj = MissionCleanup.getObject(%i);
		if (%obj.getType() & $TypeMasks::VehicleObjectType)
		{
			if (isObject(%obj.spawnBrick))
			{
				%obj.spawnBrick.schedule(10, spawnVehicle);
			}
			else
			{
				%obj.schedule(10, delete);
			}
		}
		else if (%obj.getType() & $TypeMasks::PlayerObjectType)
		{
			if (isObject(%obj.spawnBrick))
			{
				%obj.spawnBrick.schedule(10, spawnVehicle);
			}
		}
	}
}

function ServerCmdClearVehicles(%client)
{
	if (!%client.isAdmin)
	{
		return;
	}
	%vehicleCount = 0;
	%count = MissionCleanup.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		%obj = MissionCleanup.getObject(%i);
		if (%obj.getType() & $TypeMasks::VehicleObjectType)
		{
			if (isObject(%obj.spawnBrick))
			{
				%obj.spawnBrick.Vehicle = "";
				%obj.schedule(10, delete);
				%vehicleCount++;
			}
		}
	}
	MessageAll('', "\c3" @ %client.getPlayerName() @ "\c0 cleared all vehicles (" @ %vehicleCount @ ").");
}

function ServerCmdClearBots(%client)
{
	if (!%client.isAdmin)
	{
		return;
	}
	%botCount = 0;
	%count = MissionCleanup.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		%obj = MissionCleanup.getObject(%i);
		if (!(%obj.getType() & $TypeMasks::PlayerObjectType))
		{
		}
		else if (%obj.isMounted())
		{
		}
		else
		{
			%cl = %obj.getControllingClient();
			if (isObject(%cl))
			{
				if (%cl.getControlObject() == %obj)
				{
					continue;
				}
			}
			%cl = %obj.client;
			if (isObject(%cl))
			{
				if (%cl.Player == %obj)
				{
					continue;
				}
			}
			if (isObject(%obj.spawnBrick))
			{
				%obj.spawnBrick.Vehicle = "";
			}
			%obj.schedule(10, delete);
			%botCount++;
		}
	}
	MessageAll('', "\c3" @ %client.getPlayerName() @ "\c0 cleared all bots (" @ %botCount @ ").");
}

function serverCmdDFG(%client)
{
	if (!%client.isSuperAdmin)
	{
		return;
	}
	%group = %client.brickGroup;
	%count = %group.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		%obj = %group.getObject(%i);
		if (%obj.isPlanted())
		{
			%dist = %obj.getDistanceFromGround();
			if (%dist > 9999)
			{
				%obj.setColor(34);
				continue;
			}
			if (%dist == -1)
			{
				%obj.setColor(0);
				continue;
			}
			%obj.setColor(%dist);
		}
	}
}

function serverCmdGetPZ(%client)
{
	if (!%client.isSuperAdmin)
	{
		return;
	}
	%player = %client.Player;
	%pos = %player.getPosition();
	%mask = $TypeMasks::PhysicalZoneObjectType;
	%radius = 10;
	for (initContainerRadiusSearch(%pos, %radius, %mask); (%searchObj = containerSearchNext()) != 0; messageClient(%client, '', "Got PZ: " @ %searchObj))
	{
		%searchObj = getWord(%searchObj, 0);
	}
}

function serverCmdRayPZ(%client)
{
	if (!%client.isSuperAdmin)
	{
		return;
	}
	%player = %client.Player;
	%start = %player.getEyePoint();
	%vec = VectorScale(%player.getEyeVector(), 10);
	%end = VectorAdd(%start, %vec);
	%mask = $TypeMasks::PhysicalZoneObjectType;
	%searchObj = containerRayCast(%start, %end, %mask, 0);
	%searchObj = getWord(%searchObj, 0);
	if (%searchObj !$= "")
	{
		messageClient(%client, '', "Ray PZ: " @ %searchObj);
	}
}

function serverCmdSetPreviewCenter(%client)
{
	if (!%client.isAdmin)
	{
		return;
	}
	if ($Server::LAN == 1)
	{
		commandToClient(%client, 'messageboxOK', "Fail", "Server preview images are only generated in internet games");
		return;
	}
	%obj = %client.getControlObject();
	if (!isObject(%obj))
	{
		return;
	}
	%pos = getWords(%obj.getTransform(), 0, 2);
	setRaytracerCenter(%pos);
	startRaytracer();
	messageClient(%client, '', "Preview image is now centered on " @ %pos);
}

function ServerCmdUseInventory(%client, %slot)
{
	%mg = %client.miniGame;
	if (isObject(%mg))
	{
		if (!%mg.EnableBuilding)
		{
			return;
		}
	}
	if (%client.isTalking)
	{
		serverCmdStopTalking(%client);
	}
	%player = %client.Player;
	if (%client.inventory[%slot])
	{
		%item = %client.inventory[%slot].getId();
	}
	else
	{
		%item = 0;
	}
	if (%item)
	{
		%item.onUse(%player, %slot);
		%client.currInvSlot = %slot;
	}
}

function serverCmdInstantUseBrick(%client, %data)
{
	%mg = %client.miniGame;
	if (isObject(%mg))
	{
		if (!%mg.EnableBuilding)
		{
			return;
		}
	}
	if (isObject(%data))
	{
		if (%data.getClassName() $= "fxDTSBrickData")
		{
			%data.onUse(%client.Player, -1);
			%client.currInv = -1;
			%client.currInvSlot = -1;
		}
		else
		{
			messageClient(%client, '', 'Nice try.  Brick DataBlocks only please.');
			return;
		}
	}
}

function ServerCmdUseTool(%client, %slot)
{
	if (%client.isTalking)
	{
		serverCmdStopTalking(%client);
	}
	%player = %client.Player;
	if (!isObject(%player))
	{
		return;
	}
	if (%player.tool[%slot] > 0)
	{
		%player.currTool = %slot;
		%client.currInv = -1;
		%client.currInvSlot = -1;
		%item = %player.tool[%slot].getId();
		%item.onUse(%player, %slot);
	}
}

function ServerCmdUnUseTool(%client)
{
	%player = %client.Player;
	if (%client.isTalking)
	{
		serverCmdStopTalking(%client);
	}
	if (!isObject(%player))
	{
		return;
	}
	if (isObject(%player))
	{
		%player.currTool = -1;
		%client.currInv = -1;
		%client.currInvSlot = -1;
		%player.unmountImage(0);
		%player.playThread(1, root);
	}
}

function ServerCmdDropTool(%client, %position)
{
	%player = %client.Player;
	if (!isObject(%player))
	{
		return;
	}
	%item = %player.tool[%position];
	if (isObject(%item))
	{
		if (%item.canDrop == 1)
		{
			%zScale = getWord(%player.getScale(), 2);
			%muzzlepoint = VectorAdd(%player.getPosition(), "0 0" SPC 1.5 * %zScale);
			%muzzlevector = %player.getEyeVector();
			%muzzlepoint = VectorAdd(%muzzlepoint, %muzzlevector);
			%playerRot = rotFromTransform(%player.getTransform());
			%thrownItem = new Item()
			{
				dataBlock = %item;
			};
			%thrownItem.setScale(%player.getScale());
			MissionCleanup.add(%thrownItem);
			%thrownItem.setTransform(%muzzlepoint @ " " @ %playerRot);
			%thrownItem.setVelocity(VectorScale(%muzzlevector, 20 * %zScale));
			%thrownItem.schedulePop();
			%thrownItem.miniGame = %client.miniGame;
			%thrownItem.bl_id = %client.getBLID();
			%thrownItem.setCollisionTimeout(%player);
			if (%item.className $= "Weapon")
			{
				%player.weaponCount--;
			}
			%player.tool[%position] = 0;
			messageClient(%client, 'MsgItemPickup', '', %position, 0);
			if (%player.getMountedImage(%item.image.mountPoint) > 0)
			{
				if (%player.getMountedImage(%item.image.mountPoint).getId() == %item.image.getId())
				{
					%player.unmountImage(%item.image.mountPoint);
				}
			}
		}
	}
}

function ServerCmdShiftBrick(%client, %x, %y, %z)
{
	%x = mFloor(%x);
	%y = mFloor(%y);
	%z = mFloor(%z);
	%player = %client.Player;
	%tempBrick = %player.tempBrick;
	if (!isObject(%player) || !isObject(%tempBrick))
	{
		return;
	}
	if (%z > 0)
	{
		%player.playThread(3, shiftUp);
	}
	else if (%z < 0)
	{
		%player.playThread(3, shiftDown);
	}
	else if (%y > 0)
	{
		%player.playThread(3, shiftLeft);
	}
	else if (%y < 0)
	{
		%player.playThread(3, shiftRight);
	}
	else if (%x > 0)
	{
		%player.playThread(3, shiftAway);
	}
	else if (%x < 0)
	{
		%player.playThread(3, shiftTO);
	}
	if (%tempBrick)
	{
		%forwardVec = %player.getForwardVector();
		%forwardX = getWord(%forwardVec, 0);
		%forwardY = getWord(%forwardVec, 1);
		if (%forwardX > 0)
		{
			if (%forwardX > mAbs(%forwardY))
			{
			}
			else if (%forwardY > 0)
			{
				%newY = %x;
				%newX = -1 * %y;
				%x = %newX;
				%y = %newY;
			}
			else
			{
				%newY = -1 * %x;
				%newX = 1 * %y;
				%x = %newX;
				%y = %newY;
			}
		}
		else if (mAbs(%forwardX) > mAbs(%forwardY))
		{
			%x *= -1;
			%y *= -1;
		}
		else if (%forwardY > 0)
		{
			%newY = %x;
			%newX = -1 * %y;
			%x = %newX;
			%y = %newY;
		}
		else
		{
			%newY = -1 * %x;
			%newX = 1 * %y;
			%x = %newX;
			%y = %newY;
		}
		%x *= 0.5;
		%y *= 0.5;
		%z *= 0.2;
		shift(%tempBrick, %x, %y, %z);
	}
}

function ServerCmdSuperShiftBrick(%client, %x, %y, %z)
{
	%x = mFloor(%x);
	%y = mFloor(%y);
	%z = mFloor(%z);
	%player = %client.Player;
	%tempBrick = %player.tempBrick;
	if (!isObject(%player) || !isObject(%tempBrick))
	{
		return;
	}
	if (%z > 0)
	{
		%player.playThread(3, shiftUp);
	}
	else if (%z < 0)
	{
		%player.playThread(3, shiftDown);
	}
	else if (%y > 0)
	{
		%player.playThread(3, shiftLeft);
	}
	else if (%y < 0)
	{
		%player.playThread(3, shiftRight);
	}
	else if (%x > 0)
	{
		%player.playThread(3, shiftAway);
	}
	else if (%x < 0)
	{
		%player.playThread(3, shiftTO);
	}
	if (%tempBrick)
	{
		%forwardVec = %player.getForwardVector();
		%forwardX = getWord(%forwardVec, 0);
		%forwardY = getWord(%forwardVec, 1);
		if (%forwardX > 0)
		{
			if (%forwardX > mAbs(%forwardY))
			{
			}
			else if (%forwardY > 0)
			{
				%newY = %x;
				%newX = -1 * %y;
				%x = %newX;
				%y = %newY;
			}
			else
			{
				%newY = -1 * %x;
				%newX = 1 * %y;
				%x = %newX;
				%y = %newY;
			}
		}
		else if (mAbs(%forwardX) > mAbs(%forwardY))
		{
			%x *= -1;
			%y *= -1;
		}
		else if (%forwardY > 0)
		{
			%newY = %x;
			%newX = -1 * %y;
			%x = %newX;
			%y = %newY;
		}
		else
		{
			%newY = -1 * %x;
			%newX = 1 * %y;
			%x = %newX;
			%y = %newY;
		}
		%data = %tempBrick.getDataBlock();
		if (%tempBrick.angleID == 0 || %tempBrick.angleID == 2)
		{
			%x *= %data.brickSizeX;
			%y *= %data.brickSizeY;
			%z *= %data.brickSizeZ;
		}
		else if (%tempBrick.angleID == 1 || %tempBrick.angleID == 3)
		{
			%x *= %data.brickSizeY;
			%y *= %data.brickSizeX;
			%z *= %data.brickSizeZ;
		}
		%x *= 0.5;
		%y *= 0.5;
		%z *= 0.2;
		shift(%tempBrick, %x, %y, %z);
	}
}

function shift(%obj, %x, %y, %z)
{
	%trans = %obj.getTransform();
	%transX = getWord(%trans, 0);
	%transY = getWord(%trans, 1);
	%transZ = getWord(%trans, 2);
	%transQuat = getWords(%trans, 3, 6);
	%obj.setTransform(%transX + %x @ " " @ %transY + %y @ " " @ %transZ + %z @ " " @ %transQuat);
}

function ServerCmdRotateBrick(%client, %dir)
{
	%dir = mFloor(%dir);
	if (%dir == 0)
	{
		return;
	}
	%player = %client.Player;
	%tempBrick = %player.tempBrick;
	if (!isObject(%player) || !isObject(%tempBrick))
	{
		return;
	}
	if (%dir > 0)
	{
		%player.playThread(3, rotCW);
	}
	else
	{
		%player.playThread(3, rotCCW);
	}
	%brickTrans = %tempBrick.getTransform();
	%x = getWord(%brickTrans, 0);
	%y = getWord(%brickTrans, 1);
	%z = getWord(%brickTrans, 2);
	%brickAngle = getWord(%brickTrans, 6);
	%vectorDir = getWord(%brickTrans, 5);
	%forwardVec = %player.getForwardVector();
	%forwardX = getWord(%forwardVec, 0);
	%forwardY = getWord(%forwardVec, 1);
	if (%tempBrick.angleID % 2 == 0)
	{
		%shiftX = 0.25;
		%shiftY = 0.25;
	}
	else
	{
		%shiftX = -0.25;
		%shiftY = -0.25;
	}
	if (%tempBrick.getDataBlock().brickSizeX % 2 == %tempBrick.getDataBlock().brickSizeY % 2)
	{
		%shiftX = 0;
		%shiftY = 0;
	}
	if (%forwardX > 0)
	{
		if (%forwardX > mAbs(%forwardY))
		{
		}
		else if (%forwardY > 0)
		{
			%x += %shiftX;
		}
		else
		{
			%y -= %shiftY;
			%x -= %shiftX;
		}
	}
	else if (mAbs(%forwardX) > mAbs(%forwardY))
	{
		%x += %shiftX;
		%y -= %shiftY;
	}
	else if (%forwardY > 0)
	{
		%x += %shiftX;
	}
	else
	{
		%y -= %shiftY;
		%x -= %shiftX;
	}
	if (%vectorDir == -1)
	{
		%brickAngle += $pi;
	}
	%brickAngle /= $piOver2;
	%brickAngle = mFloor(%brickAngle + 0.1);
	%brickAngle += %dir;
	if (%brickAngle > 4)
	{
		%brickAngle -= 4;
	}
	if (%brickAngle <= 0)
	{
		%brickAngle += 4;
	}
	%tempBrick.setTransform(%x SPC %y SPC %z @ " 0 0 1 " @ %brickAngle * $piOver2);
	return;
	if (%dir == 1)
	{
		if (%brickAngle == 1)
		{
			shift(%tempBrick, 0, 0.5, 0);
		}
		else if (%brickAngle == 2)
		{
			shift(%tempBrick, 0.5, 0, 0);
		}
		else if (%brickAngle == 3)
		{
			shift(%tempBrick, 0, -0.5, 0);
		}
		else if (%brickAngle == 4)
		{
			shift(%tempBrick, -0.5, 0, 0);
		}
	}
	else if (%brickAngle == 1)
	{
		shift(%tempBrick, -0.5, 0, 0);
	}
	else if (%brickAngle == 2)
	{
		shift(%tempBrick, 0, 0.5, 0);
	}
	else if (%brickAngle == 3)
	{
		shift(%tempBrick, 0.5, 0, 0);
	}
	else if (%brickAngle == 4)
	{
		shift(%tempBrick, 0, -0.5, 0);
	}
}

function ServerCmdUndoBrick(%client)
{
	%line = %client.undoStack.pop();
	%obj = getField(%line, 0);
	%player = %client.Player;
	if (isObject(%obj))
	{
		%action = getField(%line, 1);
		if (isObject(%player))
		{
			%player.playThread(3, undo);
		}
		if (%action !$= "COLORGENERIC")
		{
			if (!(%obj.getType() & $TypeMasks::FxBrickAlwaysObjectType))
			{
				error("ERROR: ServerCmdUndoBrick(" @ %client @ ") - \"" @ %obj @ "\" is not a brick");
				return;
			}
		}
		if (%action $= "PLANT")
		{
			if (%obj.getGroup() != %client.brickGroup)
			{
				return;
			}
			if (%obj.willCauseChainKill())
			{
				%obj.undoTrustCheck();
			}
			else
			{
				%obj.killBrick();
			}
		}
		else if (%action $= "COLOR")
		{
			if (getTrustLevel(%obj, %client) < $TrustLevel::UndoPaint)
			{
				commandToClient(%client, 'CenterPrint', %brickGroup.name @ " does not trust you enough to do that.", 1);
				return;
			}
			%oldColor = getField(%line, 2);
			%obj.setColor(%oldColor);
			if (isObject(%obj.Vehicle))
			{
				if (%obj.reColorVehicle)
				{
					%obj.colorVehicle();
				}
			}
		}
		else if (%action $= "COLORFX")
		{
			if (getTrustLevel(%obj, %client) < $TrustLevel::UndoFXPaint)
			{
				commandToClient(%client, 'CenterPrint', %brickGroup.name @ " does not trust you enough to do that.", 1);
				return;
			}
			%oldColorFX = getField(%line, 2);
			%obj.setColorFX(%oldColorFX);
		}
		else if (%action $= "SHAPEFX")
		{
			if (getTrustLevel(%obj, %client) < $TrustLevel::UndoFXPaint)
			{
				commandToClient(%client, 'CenterPrint', %brickGroup.name @ " does not trust you enough to do that.", 1);
				return;
			}
			%oldShapeFX = getField(%line, 2);
			%obj.setShapeFX(%oldShapeFX);
		}
		else if (%action $= "PRINT")
		{
			if (getTrustLevel(%obj, %client) < $TrustLevel::UndoPrint)
			{
				commandToClient(%client, 'CenterPrint', %brickGroup.name @ " does not trust you enough to do that.", 1);
				return;
			}
			%oldPrintID = getField(%line, 2);
			%obj.setPrint(%oldPrintID);
		}
		else if (%action $= "COLORGENERIC")
		{
			%oldColor = getField(%line, 2);
			%obj.setNodeColor("ALL", %oldColor);
			%obj.color = %oldColor;
		}
		else
		{
			error("ERROR: ServerCmdUndoBrick() - unknown undo state \"" @ %line @ "\"");
		}
	}
}

function ServerCmdPlantBrick(%client)
{
	if ($Game::MissionCleaningUp)
	{
		return 0;
	}
	if (!isUnlocked())
	{
		if (getDemoBrickCount() <= 0)
		{
			Canvas.pushDialog(demoBrickLimitGui);
			return 0;
		}
	}
	%mg = %client.miniGame;
	if (isObject(%mg))
	{
		if (!%mg.EnableBuilding)
		{
			return 0;
		}
	}
	if ($Server::BrickCount >= $Pref::Server::BrickLimit)
	{
		messageClient(%client, 'MsgPlantError_Limit');
		return 0;
	}
	if (!%client.isAdmin && !%client.isSuperAdmin)
	{
		if ($Pref::Server::MaxBricksPerSecond > 0)
		{
			%currTime = getSimTime();
			if (%client.bpsTime + 1000 < %currTime)
			{
				%client.bpsCount = 0;
				%client.bpsTime = %currTime;
			}
			if (%client.bpsCount >= $Pref::Server::MaxBricksPerSecond)
			{
				return 0;
			}
		}
	}
	%player = %client.Player;
	%tempBrick = %player.tempBrick;
	if (!isObject(%player) || !isObject(%tempBrick))
	{
		return 0;
	}
	%tempBrickTrans = %tempBrick.getTransform();
	%tempBrickPos = getWords(%tempBrickTrans, 0, 2);
	%brickData = %tempBrick.getDataBlock();
	if (%brickData.brickSizeX > %brickData.brickSizeY)
	{
		%brickRadius = %brickData.brickSizeX;
	}
	else
	{
		%brickRadius = %brickData.brickSizeY;
	}
	%brickRadius = (%brickRadius * 0.5) / 2;
	if ($Pref::Server::TooFarDistance == 0 || $Pref::Server::TooFarDistance $= "")
	{
		$Pref::Server::TooFarDistance = 50;
	}
	$Pref::Server::TooFarDistance = mClampF($Pref::Server::TooFarDistance, 20, 99999);
	if (VectorDist(%tempBrickPos, %client.Player.getPosition()) > $Pref::Server::TooFarDistance + %brickRadius)
	{
		messageClient(%client, 'MsgPlantError_TooFar');
		return 0;
	}
	%plantBrick = new fxDTSBrick()
	{
		dataBlock = %tempBrick.getDataBlock();
		position = %tempBrickTrans;
	};
	%client.brickGroup.add(%plantBrick);
	%plantBrick.setTransform(%tempBrickTrans);
	%plantBrick.setColor(%tempBrick.getColorID());
	%plantBrick.setPrint(%tempBrick.getPrintID());
	%plantBrick.client = %client;
	%plantErrorCode = %plantBrick.plant();
	if (!%plantBrick.isColliding())
	{
		%plantBrick.dontCollideAfterTrust = 1;
	}
	%plantBrick.setColliding(0);
	if (%plantErrorCode == 0)
	{
		if (!$Server::LAN)
		{
			if (%plantBrick.getNumDownBricks())
			{
				%plantBrick.stackBL_ID = %plantBrick.getDownBrick(0).stackBL_ID;
			}
			else if (%plantBrick.getNumUpBricks())
			{
				%plantBrick.stackBL_ID = %plantBrick.getUpBrick(0).stackBL_ID;
			}
			else
			{
				%plantBrick.stackBL_ID = %client.getBLID();
			}
			if (%plantBrick.stackBL_ID <= 0)
			{
				%plant.stackBL_ID = %client.getBLID();
			}
		}
		%client.undoStack.push(%plantBrick TAB "PLANT");
		if ($Server::LAN)
		{
			%plantBrick.TrustCheckFinished();
		}
		else
		{
			%plantBrick.PlantedTrustCheck();
		}
		%player.playThread(3, plant);
		if ($Pref::Server::RandomBrickColor == 1)
		{
			%randColor = getRandom(5);
			if (%randColor == 0)
			{
				%player.tempBrick.setColor(0);
			}
			else if (%randColor == 1)
			{
				%player.tempBrick.setColor(1);
			}
			else if (%randColor == 2)
			{
				%player.tempBrick.setColor(3);
			}
			else if (%randColor == 3)
			{
				%player.tempBrick.setColor(4);
			}
			else if (%randColor == 4)
			{
				%player.tempBrick.setColor(5);
			}
			else if (%randColor == 5)
			{
				%player.tempBrick.setColor(7);
			}
		}
		else
		{
			%player.tempBrick.setColor(%client.currentColor);
		}
		%client.bpsCount++;
	}
	else if (%plantErrorCode == 1)
	{
		%plantBrick.delete();
		messageClient(%client, 'MsgPlantError_Overlap');
	}
	else if (%plantErrorCode == 2)
	{
		%plantBrick.delete();
		messageClient(%client, 'MsgPlantError_Float');
	}
	else if (%plantErrorCode == 3)
	{
		%plantBrick.delete();
		messageClient(%client, 'MsgPlantError_Stuck');
	}
	else if (%plantErrorCode == 4)
	{
		%plantBrick.delete();
		messageClient(%client, 'MsgPlantError_Unstable');
	}
	else if (%plantErrorCode == 5)
	{
		%plantBrick.delete();
		messageClient(%client, 'MsgPlantError_Buried');
	}
	else
	{
		%plantBrick.delete();
		messageClient(%client, 'MsgPlantError_Forbidden');
	}
	if (getBrickCount() <= 100 && getRayTracerProgress() <= -1 && getRayTracerProgress() < 0 && $Server::LAN == 0 && doesAllowConnections())
	{
		startRaytracer();
	}
	return %plantBrick;
}

function ServerCmdCancelBrick(%client)
{
	%player = %client.Player;
	if (%player)
	{
		if (%player.tempBrick)
		{
			%player.tempBrick.delete();
			%player.tempBrick = "";
		}
	}
}

function serverCmdUsePrintGun(%client)
{
	%player = %client.Player;
	if (!isObject(%player))
	{
		return;
	}
	if (%client.isTalking)
	{
		serverCmdStopTalking(%client);
	}
	%mountPoint = printGunImage.mountPoint;
	%mountedImage = %player.getMountedImage(%mountPoint);
	if (%mountedImage)
	{
		if (%mountedImage == printGunImage.getId())
		{
			%player.unmountImage(%mountPoint);
			messageClient(%player.client, 'MsgHilightInv', '', -1);
			%player.currWeaponSlot = -1;
		}
		else
		{
			%player.mountImage(printGunImage, %mountPoint);
			messageClient(%player.client, 'MsgHilightInv', '', -1);
			%player.currWeaponSlot = -1;
		}
	}
	else
	{
		%player.mountImage(printGunImage, %mountPoint);
		messageClient(%player.client, 'MsgHilightInv', '', -1);
		%player.currWeaponSlot = -1;
	}
}

function serverCmdUseFXCan(%client, %index)
{
	if (isObject(%client.miniGame))
	{
		if (!%client.miniGame.enablePainting)
		{
			return;
		}
	}
	%player = %client.Player;
	%index = mFloor(%index);
	if (!isObject(%player))
	{
		return;
	}
	if (%client.isTalking)
	{
		serverCmdStopTalking(%client);
	}
	if (%index == 0)
	{
		%image = flatSprayCanImage;
	}
	else if (%index == 1)
	{
		%image = pearlSprayCanImage;
	}
	else if (%index == 2)
	{
		%image = chromeSprayCanImage;
	}
	else if (%index == 3)
	{
		%image = glowSprayCanImage;
	}
	else if (%index == 4)
	{
		%image = blinkSprayCanImage;
	}
	else if (%index == 5)
	{
		%image = swirlSprayCanImage;
	}
	else if (%index == 6)
	{
		%image = rainbowSprayCanImage;
	}
	else if (%index == 7)
	{
		%image = stableSprayCanImage;
	}
	else if (%index == 8)
	{
		%image = jelloSprayCanImage;
	}
	else
	{
		return;
	}
	%player.currSprayCan = -1;
	%player.currFXCan = %index;
	%player.currTool = -1;
	%player.updateArm(%image);
	%player.mountImage(%image, $RightHandSlot, 1);
}

function serverCmdUseSprayCan(%client, %index)
{
	if (isObject(%client.miniGame))
	{
		if (!%client.miniGame.enablePainting)
		{
			return;
		}
	}
	%index = mFloor(%index);
	%player = %client.Player;
	%color = %index;
	if (!isObject(%player))
	{
		return;
	}
	if (%client.isTalking)
	{
		serverCmdStopTalking(%client);
	}
	%image = nameToID("color" @ %color @ "SprayCanImage");
	if (isObject(%image))
	{
		%client.currentColor = %index;
		%player.updateArm(%image);
		%player.mountImage(%image, $RightHandSlot, 1, %image.skinName);
	}
	else
	{
		return;
	}
	if (isObject(%player.tempBrick))
	{
		%player.tempBrick.setColor(%index);
	}
	%player.currSprayCan = %index;
	%player.currFXCan = -1;
	%player.currTool = -1;
	return;
	%mountedImage = %player.getMountedImage($RightHandSlot);
	if (%mountedImage.Item $= "sprayCan" && %player.currWeaponSlot == %invPosition)
	{
		%color++;
		if (%color > $maxSprayColors)
		{
			%color = 0;
		}
		%image = nameToID("color" @ %color @ "SprayCanImage");
		%player.mountImage(%image, $RightHandSlot, 1, %image.skinName);
		%client.color = %color;
	}
	else
	{
		if (%color !$= "")
		{
			%image = nameToID("color" @ %color @ "SprayCanImage");
			%player.mountImage(%image, $RightHandSlot, 1, %image.skinName);
		}
		else
		{
			%image = nameToID("color0SprayCanImage");
			%player.mountImage(%image, $RightHandSlot, 1, %image.skinName);
			%client.color = 0;
		}
		%player.currWeaponSlot = %invPosition;
	}
	return;
	%player = %client.Player;
	%color = %client.color;
	%mountedImage = %player.getMountedImage($RightHandSlot);
	if (%mountedImage.Item $= "sprayCan")
	{
		if (%mountedImage == nameToID("redSprayCanImage"))
		{
			%image = yellowSprayCanImage;
			%client.color = "yellow";
		}
		else if (%mountedImage == nameToID("yellowSprayCanImage"))
		{
			%image = greenSprayCanImage;
			%client.color = "green";
		}
		else if (%mountedImage == nameToID("greenSprayCanImage"))
		{
			%image = blueSprayCanImage;
			%client.color = "blue";
		}
		else if (%mountedImage == nameToID("blueSprayCanImage"))
		{
			%image = whiteSprayCanImage;
			%client.color = "white";
		}
		else if (%mountedImage == nameToID("whiteSprayCanImage"))
		{
			%image = graySprayCanImage;
			%client.color = "gray";
		}
		else if (%mountedImage == nameToID("graySprayCanImage"))
		{
			%image = grayDarkSprayCanImage;
			%client.color = "grayDark";
		}
		else if (%mountedImage == nameToID("grayDarkSprayCanImage"))
		{
			%image = blackSprayCanImage;
			%client.color = "black";
		}
		else if (%mountedImage == nameToID("blackSprayCanImage"))
		{
			%image = redSprayCanImage;
			%client.color = "red";
		}
		else if (%mountedImage == nameToID("brownSprayCanImage"))
		{
			%image = redSprayCanImage;
			%client.color = "red";
		}
		%player.mountImage(%image, $RightHandSlot, 1, %image.skinName);
	}
	else
	{
		if (%color !$= "")
		{
			%image = nameToID(%color @ "SprayCanImage");
			%player.mountImage(%image, $RightHandSlot, 1, %image.skinName);
		}
		else
		{
			%image = redSprayCanImage;
			%player.mountImage(%image, $RightHandSlot, 1, %image.skinName);
		}
		messageClient(%client, 'MsgHilightInv', '', -1);
		%player.currWeaponSlot = -1;
	}
}

function serverCmdUseHammer(%client)
{
	if (%client.isAdmin)
	{
	}
	else if (ClientGroup.getCount() <= 1)
	{
	}
	else if (%client.canHammer == 0)
	{
		messageClient(%client, '', '\c3You cannot use the Hammer yet');
		return;
	}
	%player = %client.Player;
	%mountPoint = %this.image.mountPoint;
	%mountedImage = %player.getMountedImage(%mountPoint);
	if (%mountedImage)
	{
		if (%mountedImage == hammerImage.getId())
		{
			%player.unmountImage(%mountPoint);
			messageClient(%player.client, 'MsgHilightInv', '', -1);
			%player.currWeaponSlot = -1;
		}
		else
		{
			%player.mountImage(hammerImage, %mountPoint);
			messageClient(%player.client, 'MsgHilightInv', '', -1);
			%player.currWeaponSlot = -1;
		}
	}
	else
	{
		%player.mountImage(hammerImage, %mountPoint);
		messageClient(%player.client, 'MsgHilightInv', '', -1);
		%player.currWeaponSlot = -1;
	}
}

function serverCmdSetPrint(%client, %index)
{
	%printBrick = getWord(%client.printBrick, 0);
	if (isObject(%printBrick))
	{
		%ar = %printBrick.getDataBlock().printAspectRatio;
		if (%index >= $printARStart[%ar] && %index < $printARStart[%ar] + $printARNumPrints[%ar])
		{
			if (%printBrick.getPrintID() != %index)
			{
				%client.undoStack.push(%printBrick TAB "PRINT" TAB %printBrick.getPrintID());
				%printBrick.setPrint(%index);
			}
		}
		if (%index >= $printARStart["Letters"] && %index < $printARStart["Letters"] + $printARNumPrints["Letters"])
		{
			if (%printBrick.getPrintID() != %index)
			{
				%client.undoStack.push(%printBrick TAB "PRINT" TAB %printBrick.getPrintID());
				%printBrick.setPrint(%index);
			}
		}
		%client.lastPrint[%ar] = %index;
		%player = %client.Player;
		if (isObject(%player))
		{
			if (isObject(%player.tempBrick))
			{
				%data = %player.tempBrick.getDataBlock();
				%aspectRatio = %data.printAspectRatio;
				if (%aspectRatio $= %ar)
				{
					%player.tempBrick.setPrint(%client.lastPrint[%aspectRatio]);
				}
			}
		}
	}
	%client.printBrick = 0;
}

function serverCmdClearInventory(%client)
{
	%player = %client.Player;
	if (!isObject(%player))
	{
		return;
	}
	%maxItems = %player.getDataBlock().maxItems;
	for (%i = 0; %i < %maxItems; %i++)
	{
		%client.inventory[%i] = "";
	}
}

function serverCmdBuyBrick(%client, %position, %data)
{
	%player = %client.Player;
	if (!isObject(%player))
	{
		return;
	}
	%mg = %client.miniGame;
	if (isObject(%mg))
	{
		if (!%mg.EnableBuilding)
		{
			return;
		}
	}
	%playerData = %player.getDataBlock();
	%maxItems = %playerData.maxItems;
	if (%position < 0 || %position >= %maxItems)
	{
		return;
	}
	if (isObject(%data))
	{
		if (%data.getClassName() !$= "fxDTSBrickData")
		{
			return;
		}
		if (%data.category $= "")
		{
			return;
		}
		if (%data.subCategory $= "")
		{
			return;
		}
		if (%data.uiName $= "")
		{
			return;
		}
		%client.inventory[%position] = %data;
		messageClient(%client, 'MsgSetInvData', "", %position, %data);
	}
	else
	{
		%client.inventory[%position] = "";
	}
}

function serverCmdStartTalking(%client)
{
	if (%client.isTalking)
	{
		return;
	}
	%client.isTalking = 1;
	for (%clientIndex = 0; %clientIndex < ClientGroup.getCount(); %clientIndex++)
	{
		%cl = ClientGroup.getObject(%clientIndex);
		messageClient(%cl, 'MsgStartTalking', '', %client);
	}
}

function serverCmdStopTalking(%client)
{
	if (!%client.isTalking)
	{
		return;
	}
	%client.isTalking = 0;
	for (%clientIndex = 0; %clientIndex < ClientGroup.getCount(); %clientIndex++)
	{
		%cl = ClientGroup.getObject(%clientIndex);
		messageClient(%cl, 'MsgStopTalking', '', %client);
	}
}

function serverCmdNextSeat(%client)
{
	%player = %client.Player;
	if (!isObject(%player))
	{
		return;
	}
	%vehicle = %player.getObjectMount();
	if (!isObject(%vehicle))
	{
		return;
	}
	%vehicleData = %vehicle.getDataBlock();
	if (%vehicle.isMounted())
	{
		if (%vehicleData.nummountpoints <= 1)
		{
			%currSeat = 0;
			%motherVehicle = %vehicle.getObjectMount();
			%motherVehicleData = %motherVehicle.getDataBlock();
			for (%i = 0; %i < %motherVehicleData.nummountpoints; %i++)
			{
				if (%vehicle == %motherVehicle.getMountNodeObject(%i))
				{
					%currSeat = %i;
					break;
				}
			}
			%vehicle = %motherVehicle;
			%vehicleData = %motherVehicleData;
		}
		else
		{
			warn("WARNING: Multi-Seat turrets are not implemented yet");
		}
	}
	else
	{
		if (%vehicleData.nummountpoints <= 1)
		{
			return;
		}
		%currSeat = 0;
		for (%i = 0; %i < %vehicleData.nummountpoints; %i++)
		{
			if (%player == %vehicle.getMountNodeObject(%i))
			{
				%currSeat = %i;
				break;
			}
		}
	}
	for (%i = 1; %i < %vehicleData.nummountpoints; %i++)
	{
		%testSeat = %currSeat + %i;
		if (%testSeat < 0)
		{
			%testSeat += %vehicleData.nummountpoints;
		}
		%testSeat = %testSeat % %vehicleData.nummountpoints;
		%blockingObj = %vehicle.getMountNodeObject(%testSeat);
		if (isObject(%blockingObj))
		{
			if (!%blockingObj.getDataBlock().rideable)
			{
				continue;
			}
			if (%blockingObj.getMountedObject(0))
			{
				continue;
			}
			%blockingObj.mountObject(%player, mFloor(%blockingObj.getDataBlock().mountNode[0]));
			%player.setControlObject(%blockingObj);
			return;
		}
		else
		{
			%vehicle.mountObject(%player, %testSeat);
			return;
		}
	}
}

function serverCmdPrevSeat(%client)
{
	%player = %client.Player;
	if (!isObject(%player))
	{
		return;
	}
	%vehicle = %player.getObjectMount();
	if (!isObject(%vehicle))
	{
		return;
	}
	%vehicleData = %vehicle.getDataBlock();
	if (%vehicle.isMounted())
	{
		if (%vehicleData.nummountpoints <= 1)
		{
			%currSeat = 0;
			%motherVehicle = %vehicle.getObjectMount();
			%motherVehicleData = %motherVehicle.getDataBlock();
			for (%i = 0; %i < %motherVehicleData.nummountpoints; %i++)
			{
				if (%vehicle == %motherVehicle.getMountNodeObject(%i))
				{
					%currSeat = %i;
					break;
				}
			}
			%vehicle = %motherVehicle;
			%vehicleData = %motherVehicleData;
		}
		else
		{
			warn("WARNING: Multi-Seat turrets are not implemented yet");
		}
	}
	else
	{
		if (%vehicleData.nummountpoints <= 1)
		{
			return;
		}
		%currSeat = 0;
		for (%i = 0; %i < %vehicleData.nummountpoints; %i++)
		{
			if (%player == %vehicle.getMountNodeObject(%i))
			{
				%currSeat = %i;
				break;
			}
		}
	}
	for (%i = 1; %i < %vehicleData.nummountpoints; %i++)
	{
		%testSeat = %currSeat - %i;
		if (%testSeat < 0)
		{
			%testSeat += %vehicleData.nummountpoints;
		}
		%testSeat = %testSeat % %vehicleData.nummountpoints;
		%blockingObj = %vehicle.getMountNodeObject(%testSeat);
		if (isObject(%blockingObj))
		{
			if (!%blockingObj.getDataBlock().rideable)
			{
				continue;
			}
			if (%blockingObj.getMountedObject(0))
			{
				continue;
			}
			%blockingObj.mountObject(%player, mFloor(%blockingObj.getDataBlock().mountNode[0]));
			%player.setControlObject(%blockingObj);
			return;
		}
		else
		{
			%vehicle.mountObject(%player, %testSeat);
			return;
		}
	}
}

function serverCmdIconInit(%client)
{
	if (!%client.isAdmin)
	{
		return;
	}
	%camera = %client.Camera;
	%client.setControlObject(%camera);
	%camera.setMode("Observer");
	%camera.setTransform("-8.31 1.69 -998.36 0.4788 -0.2068 0.8532 0.9374");
}

function serverCmdDoAllIcons(%client)
{
	if (!%client.isAdmin)
	{
		return;
	}
	echo("doing all icons!");
	doAllIcons(0);
}

function serverCmdDoIcon(%client, %brickName)
{
	if (!%client.isAdmin)
	{
		return;
	}
	%brickData = "Brick" @ %brickName @ "Data";
	if (isObject($iconBrick))
	{
		$iconBrick.delete();
	}
	$iconBrick = new fxDTSBrick()
	{
		dataBlock = %brickData;
		isPlanted = 1;
	};
	MissionCleanup.add($iconBrick);
	$iconBrick.setTransform("0 10 -1005 0 0 1 " @ %brickData.orientationFix * $piOver2);
	schedule(3000, 0, doIconScreenshot);
}

function doIconScreenshot()
{
	%oldContent = Canvas.getContent();
	Canvas.setContent(noHudGui);
	if ($iconBrick.getClassName() $= "fxDTSBrick")
	{
		%brickName = $iconBrick.getDataBlock().uiName;
	}
	else if ($IconName !$= "")
	{
		%brickName = $IconName;
	}
	else
	{
		%brickName = "ERROR";
	}
	screenShot("iconShots/" @ %brickName @ ".png", "PNG");
	Canvas.setContent(%oldContent);
}

function serverCmdDoItemIcon(%client, %data)
{
	if (!%client.isAdmin)
	{
		return;
	}
	%camera = %client.Camera;
	%client.setControlObject(%camera);
	%camera.setMode("Observer");
	%camera.setTransform("-1.874 8.050 -1003.7 0.47718 -0.197864 0.856242 0.901965");
	if (isObject($iconBrick))
	{
		$iconBrick.delete();
	}
	$IconName = %data.uiName;
	$iconBrick = new Item()
	{
		static = 1;
		rotate = 0;
		dataBlock = %data;
	};
	MissionCleanup.add($iconBrick);
	$iconBrick.setTransform("0 10 -1005 0 0 1 -1.57");
	$iconBrick.setNodeColor("ALL", "1 1 1 1");
	$iconBrick.schedule(100, setNodeColor, "ALL", "1 1 1 1");
	schedule(1000, 0, doIconScreenshot);
}

datablock StaticShapeData(dummyPlayer)
{
	category = "Misc";
	shapeFile = "~/data/shapes/player/m.dts";
};
function serverCmdDoPackIcons(%client)
{
	if (!%client.isAdmin)
	{
		return;
	}
	if (isObject($iconBrick))
	{
		$iconBrick.delete();
	}
	%camera = %client.Camera;
	%client.setControlObject(%camera);
	%camera.setMode("Observer");
	%camera.setTransform("-1.874 8.050 -1003.7 0.47718 -0.197864 0.856242 0.901965");
	$iconBrick = new StaticShape()
	{
		dataBlock = dummyPlayer;
	};
	MissionCleanup.add($iconBrick);
	$iconBrick.setTransform("0 10 -1006 0 0 1 1.57");
	$iconBrick.setScale("0.5 0.5 0.5");
	$iconBrick.setNodeColor("ALL", "1 1 1 1");
	$iconBrick.hideNode("ALL");
	%time = -1;
	for (%i = 0; %i < $numPack; %i++)
	{
		%name = $pack[%i];
		if (%name !$= "NULL")
		{
			schedulePlayerIcon(%name, %time++);
		}
	}
}

function serverCmdDoSecondPackIcons(%client)
{
	if (!%client.isAdmin)
	{
		return;
	}
	if (isObject($iconBrick))
	{
		$iconBrick.delete();
	}
	%camera = %client.Camera;
	%client.setControlObject(%camera);
	%camera.setMode("Observer");
	%camera.setTransform("-1.874 8.050 -1003.7 0.47718 -0.197864 0.856242 0.901965");
	$iconBrick = new StaticShape()
	{
		dataBlock = dummyPlayer;
	};
	MissionCleanup.add($iconBrick);
	$iconBrick.setTransform("0 10 -1006 0 0 1 1.57");
	$iconBrick.setScale("0.5 0.5 0.5");
	$iconBrick.setNodeColor("ALL", "1 1 1 1");
	$iconBrick.hideNode("ALL");
	%time = -1;
	for (%i = 0; %i < $numSecondPack; %i++)
	{
		%name = $SecondPack[%i];
		if (%name !$= "NULL" && %name !$= "none")
		{
			schedulePlayerIcon(%name, %time++);
		}
	}
}

function serverCmdDoPlayerIcons(%client)
{
	if (!%client.isAdmin)
	{
		return;
	}
	if (isObject($iconBrick))
	{
		$iconBrick.delete();
	}
	%camera = %client.Camera;
	%client.setControlObject(%camera);
	%camera.setMode("Observer");
	%camera.setTransform("-1.874 8.050 -1003.7 0.47718 -0.197864 0.856242 0.901965");
	$iconBrick = new StaticShape()
	{
		dataBlock = dummyPlayer;
	};
	MissionCleanup.add($iconBrick);
	$iconBrick.setTransform("0 10 -1006 0 0 1 -1.57");
	$iconBrick.setScale("0.5 0.5 0.5");
	$iconBrick.setNodeColor("ALL", "1 1 1 1");
	$iconBrick.hideNode("ALL");
	%time = -1;
	for (%i = 0; %i < $numHat; %i++)
	{
		%name = $hat[%i];
		if (%name !$= "NULL")
		{
			schedulePlayerIcon(%name, %time++);
		}
	}
	for (%i = 0; %i < $numAccent; %i++)
	{
		%name = $Accent[%i];
		if (%name !$= "NULL")
		{
			schedulePlayerIcon(%name, %time++);
		}
	}
	for (%i = 0; %i < $num["LHand"]; %i++)
	{
		%name = $LHand[%i];
		if (%name !$= "NULL")
		{
			schedulePlayerIcon(%name, %time++);
		}
	}
	for (%i = 0; %i < $num["RHand"]; %i++)
	{
		%name = $RHand[%i];
		if (%name !$= "NULL")
		{
			schedulePlayerIcon(%name, %time++);
		}
	}
	for (%i = 0; %i < $num["LArm"]; %i++)
	{
		%name = $LArm[%i];
		if (%name !$= "NULL")
		{
			schedulePlayerIcon(%name, %time++);
		}
	}
	for (%i = 0; %i < $num["RArm"]; %i++)
	{
		%name = $RArm[%i];
		if (%name !$= "NULL")
		{
			schedulePlayerIcon(%name, %time++);
		}
	}
	for (%i = 0; %i < $num["LLeg"]; %i++)
	{
		%name = $LLeg[%i];
		if (%name !$= "NULL")
		{
			schedulePlayerIcon(%name, %time++);
		}
	}
	for (%i = 0; %i < $num["RLeg"]; %i++)
	{
		%name = $RLeg[%i];
		if (%name !$= "NULL")
		{
			schedulePlayerIcon(%name, %time++);
		}
	}
	for (%i = 0; %i < $num["Chest"]; %i++)
	{
		%name = $Chest[%i];
		if (%name !$= "NULL")
		{
			schedulePlayerIcon(%name, %time++);
		}
	}
	for (%i = 0; %i < $num["Hip"]; %i++)
	{
		%name = $Hip[%i];
		if (%name !$= "NULL")
		{
			schedulePlayerIcon(%name, %time++);
		}
	}
}

function schedulePlayerIcon(%meshName, %time)
{
	if (!isObject($iconBrick))
	{
		Con::errorf("ERROR: schedulePlayerIcon() - no $iconBrick");
		return;
	}
	%time *= 1000;
	$iconBrick.schedule(%time + 500, hideNode, "ALL");
	$iconBrick.schedule(%time + 600, unHideNode, %meshName);
	schedule(%time + 900, 0, eval, "$IconName = " @ %meshName @ ";");
	schedule(%time + 1000, 0, doIconScreenshot);
}

function doAllIcons(%pos)
{
	if (%pos > 0)
	{
		doIconScreenshot();
	}
	%numDataBlocks = getDataBlockGroupSize();
	%brickData = 0;
	for (%pos++; %pos < %numDataBlocks; %pos++)
	{
		%checkDB = getDataBlock(%pos);
		if (%checkDB.getClassName() $= "fxDTSBrickData")
		{
			%brickData = %checkDB;
			break;
		}
	}
	if (isObject($iconBrick))
	{
		$iconBrick.delete();
	}
	if (%brickData != 0)
	{
		$iconBrick = new fxDTSBrick()
		{
			dataBlock = %brickData;
			isPlanted = 1;
		};
		$iconBrick.setTransform("0 10 -1005 0 0 1 -1.57");
		schedule(1000, 0, doAllIcons, %pos);
	}
}

datablock AudioDescription(AudioDefault3d)
{
	volume = 1;
	isLooping = 0;
	is3D = 1;
	ReferenceDistance = 20;
	maxDistance = 100;
	type = $SimAudioType;
};
datablock AudioDescription(AudioClose3d)
{
	volume = 1;
	isLooping = 0;
	is3D = 1;
	ReferenceDistance = 10;
	maxDistance = 60;
	type = $SimAudioType;
};
datablock AudioDescription(AudioClosest3d)
{
	volume = 1;
	isLooping = 0;
	is3D = 1;
	ReferenceDistance = 5;
	maxDistance = 30;
	type = $SimAudioType;
};
datablock AudioDescription(AudioDefaultLooping3d)
{
	volume = 1;
	isLooping = 1;
	is3D = 1;
	ReferenceDistance = 20;
	maxDistance = 100;
	type = $SimAudioType;
};
datablock AudioDescription(AudioCloseLooping3d)
{
	volume = 1;
	isLooping = 1;
	is3D = 1;
	ReferenceDistance = 10;
	maxDistance = 50;
	type = $SimAudioType;
};
datablock AudioDescription(AudioClosestLooping3d)
{
	volume = 1;
	isLooping = 1;
	is3D = 1;
	ReferenceDistance = 5;
	maxDistance = 30;
	type = $SimAudioType;
};
datablock AudioDescription(Audio2D)
{
	volume = 1;
	isLooping = 0;
	is3D = 0;
	type = $SimAudioType;
};
datablock AudioDescription(AudioLooping2D)
{
	volume = 1;
	isLooping = 1;
	is3D = 0;
	type = $SimAudioType;
};
$Camera::movementSpeed = 40;
datablock CameraData(Observer)
{
	mode = "Observer";
};
function Observer::onTrigger(%this, %obj, %trigger, %state)
{
	if (%state == 0)
	{
		return;
	}
	if ($Game::MissionCleaningUp)
	{
		return;
	}
	%client = %obj.getControllingClient();
	if (%obj.mode $= "Observer")
	{
	}
	else if (%obj.mode $= "Corpse")
	{
		if (isObject(%client.Player))
		{
			if (%client.Player.canDismount)
			{
				if (%client.Player.getDamagePercent() < 1)
				{
					%client.setControlObject(%client.Player);
					%this.setMode(%obj, "Observer");
					return;
				}
			}
			else
			{
				return;
			}
		}
		if (%trigger != 0)
		{
			return;
		}
		%mg = %client.miniGame;
		%elapsedTime = getSimTime() - %client.deathTime;
		if (isObject(%mg))
		{
			if (%elapsedTime > %mg.respawnTime)
			{
				%client.spawnPlayer();
				%client.setCanRespawn(False);
				%this.setMode(%obj, "Observer");
			}
		}
		else if (%elapsedTime > $Game::MinRespawnTime || %client.isAdmin)
		{
			%client.spawnPlayer();
			%client.setCanRespawn(False);
			%this.setMode(%obj, "Observer");
		}
	}
}

function Observer::setMode(%this, %obj, %mode, %arg1, %arg2, %arg3)
{
	if (%mode $= "Observer")
	{
		%obj.setFlyMode();
	}
	else if (%mode $= "Corpse")
	{
		%obj.unmountImage(0);
		%transform = %arg1.getTransform();
		%obj.setOrbitMode(%arg1, %transform, 0, 8, 8);
	}
	%obj.mode = %mode;
}

function Camera::onAdd(%this, %obj)
{
	%this.setMode(%this.mode);
}

function Camera::setMode(%this, %mode, %arg1, %arg2, %arg3)
{
	%this.getDataBlock().setMode(%this, %mode, %arg1, %arg2, %arg3);
}

datablock MissionMarkerData(WayPointMarker)
{
	category = "Misc";
	shapeFile = "~/data/shapes/markers/octahedron.dts";
};
datablock MissionMarkerData(SpawnSphereMarker)
{
	category = "Misc";
	shapeFile = "~/data/shapes/markers/octahedron.dts";
};
datablock MissionMarkerData(VehicleSpawnMarkerData)
{
	category = "Misc";
	shapeFile = "~/data/shapes/markers/octahedron.dts";
};
function MissionMarkerData::create(%block)
{
	if (%block $= "WayPointMarker")
	{
		%obj = new WayPoint()
		{
			dataBlock = %block;
		};
		return %obj;
	}
	else if (%block $= "SpawnSphereMarker")
	{
		%obj = new SpawnSphere()
		{
			dataBlock = %block;
		};
		return %obj;
	}
	return -1;
}

function TriggerData::onEnterTrigger(%this, %trigger, %obj)
{
}

function TriggerData::onLeaveTrigger(%this, %trigger, %obj)
{
}

function TriggerData::onTickTrigger(%this, %trigger, %obj)
{
}

function ShapeBase::use(%this, %data)
{
	if (%this.getInventory(%data) > 0)
	{
		return %data.onUse(%this);
	}
	return 0;
}

function ShapeBase::throw(%this, %data, %amount)
{
	if (%this.getInventory(%data) > 0)
	{
		%obj = %data.onThrow(%this, %amount);
		if (%obj)
		{
			%this.throwObject(%obj);
			return 1;
		}
	}
	return 0;
}

function ShapeBase::pickup(%this, %obj, %amount)
{
	%data = %obj.getDataBlock();
	return %data.onPickup(%obj, %this, %amount);
}

function ShapeBase::maxInventory(%this, %data)
{
	return %this.getDataBlock().maxInv[%data.getName()];
}

function ShapeBase::incInventory(%this, %data, %amount)
{
	%max = %this.maxInventory(%data);
	%total = %this.inv[%data.getName()];
	if (%total < %max)
	{
		if (%total + %amount > %max)
		{
			%amount = %max - %total;
		}
		%this.setInventory(%data, %total + %amount);
		return %amount;
	}
	return 0;
}

function ShapeBase::decInventory(%this, %data, %amount)
{
	%total = %this.inv[%data.getName()];
	if (%total > 0)
	{
		if (%total < %amount)
		{
			%amount = %total;
		}
		%this.setInventory(%data, %total - %amount);
		return %amount;
	}
	return 0;
}

function ShapeBase::getInventory(%this, %data)
{
	if (!isObject(%data))
	{
		error("ERROR: ShapeBase::getInventory() - datablock " @ %data @ " does not exist.");
		return 0;
	}
	return %this.inv[%data.getName()];
}

function ShapeBase::setInventory(%this, %data, %value)
{
	if (%value < 0)
	{
		%value = 0;
	}
	else
	{
		%max = %this.maxInventory(%data);
		if (%value > %max)
		{
			%value = %max;
		}
	}
	%name = %data.getName();
	if (%this.inv[%name] != %value)
	{
		%this.inv[%name] = %value;
		%data.onInventory(%this, %value);
		%this.getDataBlock().onInventory(%data, %value);
	}
	return %value;
}

function ShapeBase::clearInventory(%this)
{
}

function ShapeBase::throwObject(%this, %obj)
{
	%throwForce = %this.throwForce;
	if (!%throwForce)
	{
		%throwForce = 20;
	}
	%eye = %this.getEyeVector();
	%vec = VectorScale(%eye, %throwForce);
	%verticalForce = %throwForce / 2;
	%dot = VectorDot("0 0 1", %eye);
	if (%dot < 0)
	{
		%dot = -%dot;
	}
	%vec = VectorAdd(%vec, VectorScale("0 0 " @ %verticalForce, 1 - %dot));
	%vec = VectorAdd(%vec, %this.getVelocity());
	%pos = getBoxCenter(%this.getWorldBox());
	%obj.setTransform(%pos);
	%obj.applyImpulse(%pos, %vec);
	%obj.setCollisionTimeout(%this);
}

function ShapeBase::onInventory(%this, %data, %value)
{
}

function ShapeBaseData::onUse(%this, %user)
{
	return 0;
}

function ShapeBaseData::onThrow(%this, %user, %amount)
{
	return 0;
}

function ShapeBaseData::onPickup(%this, %obj, %user, %amount)
{
	return 0;
}

function ShapeBaseData::onInventory(%this, %user, %value)
{
}

function ShapeBase::Damage(%this, %sourceObject, %position, %damage, %damageType)
{
	%this.getDataBlock().Damage(%this, %sourceObject, %position, %damage, %damageType);
}

function ShapeBaseData::Damage(%this, %obj, %position, %source, %amount, %damageType)
{
}

$Item::RespawnTime = 4 * 1000;
$Item::PopTime = 10 * 1000;
function Item::fadeOut(%obj)
{
	%obj.startFade(0, 0, 1);
	if (%obj.getDataBlock().doColorShift)
	{
		%color = getWords(%obj.getDataBlock().colorShiftColor, 0, 2);
		%obj.setNodeColor("ALL", %color SPC "0.25");
	}
	else
	{
		%obj.setNodeColor("ALL", "1 1 1 0.25");
	}
	%obj.canPickup = 0;
}

function Item::fadeIn(%obj, %delay)
{
	if (isEventPending(%obj.fadeInSchedule))
	{
		cancel(%obj.fadeInSchedule);
	}
	if (%delay > 0)
	{
		%obj.fadeInSchedule = %obj.schedule(%delay, "fadeIn");
		return;
	}
	%obj.startFade(0, 0, 0);
	%obj.setNodeColor("ALL", "1 1 1 0.25");
	if (%obj.getDataBlock().image.doColorShift)
	{
		%obj.setNodeColor("ALL", %obj.getDataBlock().image.colorShiftColor);
	}
	else
	{
		%obj.setNodeColor("ALL", "1 1 1 1");
	}
	%obj.canPickup = 1;
}

function Item::Respawn(%obj)
{
	%obj.fadeOut();
	if (isObject(%obj.spawnBrick))
	{
		%obj.fadeIn(%obj.spawnBrick.itemRespawnTime);
	}
	else
	{
		%obj.fadeIn($Game::Item::RespawnTime);
	}
}

function Item::schedulePop(%obj)
{
	%obj.schedule($Game::Item::PopTime - 1000, "startFade", 1000, 0, 1);
	if (%obj.getDataBlock().doColorShift)
	{
		%color = getWords(%obj.getDataBlock().colorShiftColor, 0, 2);
		%obj.schedule($Game::Item::PopTime - 1000, "setNodeColor", "ALL", %color SPC "0.5");
		%obj.schedule($Game::Item::PopTime - 800, "setNodeColor", "ALL", %color SPC "0.4");
		%obj.schedule($Game::Item::PopTime - 600, "setNodeColor", "ALL", %color SPC "0.3");
		%obj.schedule($Game::Item::PopTime - 400, "setNodeColor", "ALL", %color SPC "0.2");
		%obj.schedule($Game::Item::PopTime - 200, "setNodeColor", "ALL", %color SPC "0.1");
	}
	else
	{
		%obj.schedule($Game::Item::PopTime - 1000, "setNodeColor", "ALL", "1 1 1 0.5");
		%obj.schedule($Game::Item::PopTime - 800, "setNodeColor", "ALL", "1 1 1 0.4");
		%obj.schedule($Game::Item::PopTime - 600, "setNodeColor", "ALL", "1 1 1 0.3");
		%obj.schedule($Game::Item::PopTime - 400, "setNodeColor", "ALL", "1 1 1 0.2");
		%obj.schedule($Game::Item::PopTime - 200, "setNodeColor", "ALL", "1 1 1 0.1");
	}
	%obj.schedule($Game::Item::PopTime, "delete");
}

function ItemData::onThrow(%this, %user, %amount)
{
	if (%amount $= "")
	{
		%amount = 1;
	}
	if (%this.maxInventory !$= "")
	{
		if (%amount > %this.maxInventory)
		{
			%amount = %this.maxInventory;
		}
	}
	if (!%amount)
	{
		return 0;
	}
	%user.decInventory(%this, %amount);
	%obj = new Item()
	{
		dataBlock = %this;
		rotation = "0 0 1 " @ getRandom() * 360;
		count = %amount;
	};
	MissionGroup.add(%obj);
	%obj.schedulePop();
	return %obj;
}

function ItemData::onPickup(%this, %obj, %user, %amount)
{
	if (%obj.canPickup == 0)
	{
		return;
	}
	%player = %user;
	%client = %player.client;
	%data = %player.getDataBlock();
	if (!isObject(%client))
	{
		return;
	}
	%mg = %client.miniGame;
	if (isObject(%mg))
	{
		if (%mg.weaponDamage == 1)
		{
			if (getSimTime() - %client.lastF8Time < 5000)
			{
				return;
			}
		}
	}
	%canUse = 1;
	if (miniGameCanUse(%player, %obj) == 1)
	{
		%canUse = 1;
	}
	if (miniGameCanUse(%player, %obj) == 0)
	{
		%canUse = 0;
	}
	if (!%canUse)
	{
		if (isObject(%obj.spawnBrick))
		{
			%ownerName = %obj.spawnBrick.getGroup().name;
		}
		%msg = %ownerName @ " does not trust you enough to use this item.";
		if ($lastError == $LastError::Trust)
		{
			%msg = %ownerName @ " does not trust you enough to use this item.";
		}
		else if ($lastError == $LastError::MiniGameDifferent)
		{
			if (isObject(%client.miniGame))
			{
				%msg = "This item is not part of the mini-game.";
			}
			else
			{
				%msg = "This item is part of a mini-game.";
			}
		}
		else if ($lastError == $LastError::MiniGameNotYours)
		{
			%msg = "You do not own this item.";
		}
		else if ($lastError == $LastError::NotInMiniGame)
		{
			%msg = "This item is not part of the mini-game.";
		}
		commandToClient(%client, 'CenterPrint', %msg, 1);
		return;
	}
	%freeslot = -1;
	for (%i = 0; %i < %data.maxTools; %i++)
	{
		if (%player.tool[%i] == 0)
		{
			%freeslot = %i;
			break;
		}
	}
	if (%freeslot != -1)
	{
		if (%obj.isStatic())
		{
			%obj.Respawn();
		}
		else
		{
			%obj.delete();
		}
		%player.tool[%freeslot] = %this;
		if (%user.client)
		{
			messageClient(%user.client, 'MsgItemPickup', '', %freeslot, %this.getId());
		}
		return 1;
	}
}

function ItemData::create(%data)
{
	%obj = new Item()
	{
		dataBlock = %data;
		static = 1;
		rotate = %data.rotate;
	};
	%obj.setSkinName(%data.skinName);
	return %obj;
}

function ItemData::onAdd(%this, %obj)
{
	%obj.setSkinName(%this.skinName);
	if (%this.image.doColorShift)
	{
		%obj.setNodeColor("ALL", %this.image.colorShiftColor);
	}
	%obj.canPickup = 1;
}

function ItemImage::onMount(%this, %obj, %slot)
{
}

function ItemImage::onUnMount(%this, %obj, %slot)
{
}

function ItemData::onUse(%this, %player, %invPosition)
{
	%client = %player.client;
	%playerData = %player.getDataBlock();
	%mountPoint = %this.image.mountPoint;
	%mountedImage = %player.getMountedImage(%mountPoint);
	%image = %this.image;
	%player.updateArm(%image);
	%player.mountImage(%image, %mountPoint);
}

function Item::setThrower(%this, %newThrower)
{
	%this.thrower = %newThrower;
}

function ItemData::OnCollision(%obj)
{
}

function StaticShapeData::create(%data)
{
	%obj = new StaticShape()
	{
		dataBlock = %data;
	};
	return %obj;
}

function StaticShapeData::onAdd(%this, %obj)
{
	%obj.setSkinName(%this.skinName);
}

function StaticShapeData::Damage(%this, %obj, %sourceObject, %position, %damage, %damageType)
{
	%obj.setDamageLevel(%obj.getDamageLevel() + %damage);
	if (%obj.getDamageLevel() >= %this.maxDamage)
	{
		%obj.team.count[%this.pack]--;
		%trans = %obj.getTransform();
		%exp = new Explosion()
		{
			dataBlock = %this.Explosion;
		};
		MissionCleanup.add(%exp);
		echo("PLAYING EXPLOSION SOUND ", %this.explosionSound);
		ServerPlay3D(%this.explosionSound, %trans);
		%exp.setTransform(%trans);
		%obj.setTransform("0 0 -999");
		%obj.delete();
	}
}

function StaticShape::explode(%obj)
{
	%obj.setDamageState(destroyed);
	%obj.schedule(100, setHidden, 1);
	ServerPlay3D(%obj.getDataBlock().explosionSound, %obj.getTransform());
	return;
	%data = %obj.getDataBlock();
	%pos = %obj.getWorldBoxCenter();
	%exp = new Explosion()
	{
		dataBlock = spearExplosion;
		initialPosition = %pos;
	};
	MissionCleanup.add(%exp);
	%obj.setTransform("0 0 -999");
	%obj.schedule(100, delete);
	MissionCleanup.add(%exp);
}

datablock ParticleData(glassExplosionParticle)
{
	dragCoefficient = 0;
	gravityCoefficient = 1;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 2500;
	lifetimeVarianceMS = 800;
	textureName = "~/data/particles/chunk";
	spinSpeed = 100;
	spinRandomMin = -150;
	spinRandomMax = 150;
	useInvAlpha = 0;
	colors[0] = "0.7 0.7 0.9 1.0";
	colors[1] = "0.7 0.7 0.9 0.0";
	sizes[0] = 0.4;
	sizes[1] = 0;
};
datablock ParticleEmitterData(glassExplosionEmitter)
{
	ejectionPeriodMS = 1;
	periodVarianceMS = 0;
	ejectionVelocity = 25;
	velocityVariance = 20;
	ejectionOffset = 10;
	thetaMin = 0;
	thetaMax = 180;
	phiReferenceVel = 3600;
	phiVariance = 0;
	overrideAdvance = 0;
	particles = "glassExplosionParticle";
};
datablock ExplosionData(glassExplosion)
{
	lifetimeMS = 250;
	emitter[0] = glassExplosionEmitter;
	emitter[1] = glassExplosionEmitter;
	emitter[2] = glassExplosionEmitter;
	particleEmitter = "";
	particleDensity = 2000;
	particleRadius = 10;
	faceViewer = 1;
	explosionScale = "1 1 1";
	shakeCamera = 0;
};
datablock StaticShapeData(glassA)
{
	category = "Glass";
	Item = brick2x2;
	ghost = ghostBrick2x2;
	className = "Glass";
	shapeFile = "~/data/shapes/environment/glassA.dts";
	emap = 0;
	maxDamage = 800;
	destroyedLevel = 800;
	disabledLevel = 600;
	Explosion = glassExplosion;
	expDmgRadius = 8;
	expDamage = 0.35;
	expImpulse = 500;
	maxEnergy = 50;
	rechargeRate = 0.2;
	renderWhenDestroyed = 0;
	doesRepair = 1;
	deployedObject = 1;
	explosionSound = glassExplosionSound;
};
function glassA::OnCollision(%this, %obj, %col, %vec, %speed)
{
	return;
	%tempVec = %obj.getForwardVector();
	%forwardVec = %tempVec;
	%dot = VectorDot(%forwardVec, %vec);
	echo("glass collsion ");
	echo("-speed = ", %speed);
	echo("-vec = ", %vec);
	echo("-forvec = ", %forwardVec);
	echo("-dot = ", %dot);
}

function glassA::onImpact(%this, %obj, %collidedObject, %vec, %vecLen)
{
	echo("glass impact");
}

datablock StaticShapeData(fluorescentLight)
{
	category = "Glass";
	Item = brick2x2;
	ghost = ghostBrick2x2;
	className = "Glass";
	shapeFile = "~/data/shapes/environment/fluorescentLight.dts";
	maxDamage = 800;
	destroyedLevel = 800;
	disabledLevel = 600;
	Explosion = glassExplosion;
	expDmgRadius = 8;
	expDamage = 0.35;
	expImpulse = 500;
	maxEnergy = 50;
	rechargeRate = 0.2;
	renderWhenDestroyed = 0;
	doesRepair = 1;
	deployedObject = 1;
};
datablock StaticShapeData(lightBulbA)
{
	category = "Glass";
	Item = brick2x2;
	ghost = ghostBrick2x2;
	className = "Glass";
	shapeFile = "~/data/shapes/environment/bulb1.dts";
	maxDamage = 800;
	destroyedLevel = 800;
	disabledLevel = 600;
	Explosion = glassExplosion;
	expDmgRadius = 8;
	expDamage = 0.35;
	expImpulse = 500;
	maxEnergy = 50;
	rechargeRate = 0.2;
	renderWhenDestroyed = 0;
	doesRepair = 1;
	deployedObject = 1;
};
function Explosion::onAdd(%this, %obj)
{
	echo("explosion on adD");
}

function ExplosionData::onAdd(%this, %obj)
{
	echo("explosion on adD");
}

$WeaponSlot = 0;
datablock AudioProfile(weaponSwitchSound)
{
	fileName = "~/data/sound/weaponSwitch.wav";
	description = AudioClosest3d;
	preload = 1;
};
function Weapon::onUse(%this, %player, %invPosition)
{
	%mountPoint = %this.image.mountPoint;
	%mountedImage = %player.getMountedImage(%mountPoint);
	%image = %this.image;
	%player.updateArm(%image);
	%player.mountImage(%image, %mountPoint);
	return;
	if (%mountedImage)
	{
		if (%mountedImage == %this.image.getId())
		{
			%player.unmountImage(%mountPoint);
			messageClient(%player.client, 'MsgHilightInv', '', -1);
			%player.currWeaponSlot = -1;
		}
		else
		{
			%player.mountImage(%this.image, %mountPoint);
			messageClient(%player.client, 'MsgHilightInv', '', %invPosition);
			%player.currWeaponSlot = %invPosition;
		}
	}
	else
	{
		%player.mountImage(%this.image, %mountPoint);
		messageClient(%player.client, 'MsgHilightInv', '', %invPosition);
		%player.currWeaponSlot = %invPosition;
	}
}

function Weapon::onPickup(%this, %obj, %shape, %amount)
{
	ItemData::onPickup(%this, %obj, %shape, %amount);
	return;
	if (%obj.canPickup == 0)
	{
		return;
	}
	%player = %shape;
	%client = %player.client;
	%data = %player.getDataBlock();
	if (!isObject(%client))
	{
		return;
	}
	%mg = %client.miniGame;
	if (isObject(%mg))
	{
		if (%mg.weaponDamage == 1)
		{
			if (getSimTime() - %client.lastF8Time < 5000)
			{
				return;
			}
		}
	}
	%canUse = 1;
	if (miniGameCanUse(%player, %obj) == 1)
	{
		%canUse = 1;
	}
	if (miniGameCanUse(%player, %obj) == 0)
	{
		%canUse = 0;
	}
	if (!%canUse)
	{
		if (isObject(%obj.spawnBrick))
		{
			%ownerName = %obj.spawnBrick.getGroup().name;
		}
		%msg = %ownerName @ " does not trust you enough to use this item.";
		if ($lastError == $LastError::Trust)
		{
			%msg = %ownerName @ " does not trust you enough to use this item.";
		}
		else if ($lastError == $LastError::MiniGameDifferent)
		{
			if (isObject(%client.miniGame))
			{
				%msg = "This item is not part of the mini-game.";
			}
			else
			{
				%msg = "This item is part of a mini-game.";
			}
		}
		else if ($lastError == $LastError::MiniGameNotYours)
		{
			%msg = "You do not own this item.";
		}
		else if ($lastError == $LastError::NotInMiniGame)
		{
			%msg = "This item is not part of the mini-game.";
		}
		commandToClient(%client, 'CenterPrint', %msg, 1);
		return;
	}
	if (%player.weaponCount < %data.maxWeapons)
	{
		%freeslot = -1;
		for (%i = 0; %i < %data.maxTools; %i++)
		{
			if (%player.tool[%i] == 0)
			{
				%freeslot = %i;
				break;
			}
		}
		if (%freeslot == -1)
		{
		}
		else
		{
			if (%obj.isStatic())
			{
				%obj.Respawn();
			}
			else
			{
				%obj.delete();
			}
			%player.weaponCount++;
			%player.tool[%freeslot] = %this;
			if (%player.client)
			{
				messageClient(%player.client, 'MsgItemPickup', '', %freeslot, %this.getId());
			}
			return 1;
		}
	}
	else if (%user.client)
	{
		messageClient(%user.client, 'MsgItemFailPickup', 'You already have a weapon!');
	}
}

function Weapon::onInventory(%this, %obj, %amount)
{
	if (!%amount && (%slot = %obj.getMountSlot(%this.image)) != -1)
	{
		%obj.unmountImage(%slot);
	}
}

function WeaponImage::onFire(%this, %obj, %slot)
{
	%obj.hasShotOnce = 1;
	if (%this.minShotTime > 0)
	{
		if (getSimTime() - %obj.lastFireTime < %this.minShotTime)
		{
			return;
		}
		%obj.lastFireTime = getSimTime();
	}
	%client = %obj.client;
	if (isObject(%client.miniGame))
	{
		if (getSimTime() - %client.lastF8Time < 3000)
		{
			return;
		}
	}
	%projectile = %this.Projectile;
	%dataMuzzleVelocity = %projectile.muzzleVelocity;
	if (%this.melee)
	{
		%initPos = %obj.getEyeTransform();
		%muzzlevector = %obj.getMuzzleVector(%slot);
		%start = %initPos;
		%vec = VectorScale(%muzzlevector, 20);
		%end = VectorAdd(%start, %vec);
		%mask = $TypeMasks::InteriorObjectType | $TypeMasks::PlayerObjectType | $TypeMasks::VehicleObjectType | $TypeMasks::TerrainObjectType;
		%raycast = containerRayCast(%start, %end, %mask, %obj);
		if (%raycast)
		{
			%hitPos = posFromRaycast(%raycast);
			%eyeDiff = VectorLen(VectorSub(%start, %hitPos));
			%muzzlepoint = %obj.getMuzzlePoint(%slot);
			%muzzleDiff = VectorLen(VectorSub(%muzzlepoint, %hitPos));
			%ratio = %eyeDiff / %muzzleDiff;
			%dataMuzzleVelocity *= %ratio;
		}
	}
	else
	{
		%initPos = %obj.getMuzzlePoint(%slot);
		%muzzlevector = %obj.getMuzzleVector(%slot);
		if (%obj.isFirstPerson())
		{
			%start = %obj.getEyePoint();
			%eyeVec = VectorScale(%obj.getEyeVector(), 5);
			%end = VectorAdd(%start, %eyeVec);
			%mask = $TypeMasks::TerrainObjectType | $TypeMasks::InteriorObjectType | $TypeMasks::PlayerObjectType | $TypeMasks::VehicleObjectType | $TypeMasks::TerrainObjectType | $TypeMasks::FxBrickObjectType | $TypeMasks::StaticShapeObjectType;
			if (%obj.isMounted())
			{
				%mount = %obj.getObjectMount();
			}
			else
			{
				%mount = 0;
			}
			%raycast = containerRayCast(%start, %end, %mask, %obj, %mount);
			if (%raycast)
			{
				%eyeTarget = posFromRaycast(%raycast);
				%eyeTargetVec = VectorSub(%eyeTarget, %start);
				%eyeToMuzzle = VectorSub(%start, %initPos);
				if (VectorLen(%eyeTargetVec) < 3.1)
				{
					%muzzlevector = %obj.getEyeVector();
					%initPos = %start;
				}
			}
		}
	}
	%inheritFactor = %projectile.velInheritFactor;
	%objectVelocity = %obj.getVelocity();
	%eyeVector = %obj.getEyeVector();
	%rawMuzzleVector = %obj.getMuzzleVector(%slot);
	%dot = VectorDot(%eyeVector, %rawMuzzleVector);
	if (%dot < 0.6)
	{
		if (VectorLen(%objectVelocity) < 14)
		{
			%inheritFactor = 0;
		}
	}
	%gunVel = VectorScale(%dataMuzzleVelocity, getWord(%obj.getScale(), 2));
	%muzzleVelocity = VectorAdd(VectorScale(%muzzlevector, %gunVel), VectorScale(%objectVelocity, %inheritFactor));
	%p = new (%this.projectileType)()
	{
		dataBlock = %projectile;
		initialVelocity = %muzzleVelocity;
		initialPosition = %initPos;
		sourceObject = %obj;
		sourceSlot = %slot;
		client = %obj.client;
	};
	%p.setScale(%obj.getScale());
	MissionCleanup.add(%p);
	return %p;
}

function WeaponImage::onMount(%this, %obj, %slot)
{
	if (!isObject(%obj))
	{
		error("ERROR: WeaponImage::onMount() called with no \"%obj\" parameter!");
		return;
	}
	if (%this.showBricks)
	{
		%client = %obj.client;
		if (isObject(%client))
		{
			commandToClient(%client, 'ShowBricks', 1);
		}
	}
	if (%this.ammo)
	{
		if (%obj.getInventory(%this.ammo))
		{
			%obj.setImageAmmo(%slot, 1);
		}
	}
	else
	{
		%obj.setImageAmmo(%slot, 1);
	}
}

function WeaponImage::onUnMount(%this, %obj, %slot)
{
	%obj.playThread(2, root);
	%leftimage = %obj.getMountedImage($LeftHandSlot);
	%client = %obj.client;
	if (%this.showBricks)
	{
		if (isObject(%client))
		{
			commandToClient(%client, 'ShowBricks', 0);
		}
	}
	if (isObject(%obj.tempBrick) && $Pref::Server::RandomBrickColor == 1)
	{
		%obj.tempBrick.delete();
		%obj.tempBrick = "";
	}
}

function ammo::onInventory(%this, %obj, %amount)
{
	for (%i = 0; %i < 8; %i++)
	{
		if ((%image = %obj.getMountedImage(%i)) > 0)
		{
			if (isObject(%image.ammo) && %image.ammo.getId() == %this.getId())
			{
				%obj.setImageAmmo(%i, %amount != 0);
			}
		}
	}
}

function radiusDamage(%sourceObject, %position, %radius, %damage, %damageType, %impulse)
{
	initContainerRadiusSearch(%position, %radius, $TypeMasks::ShapeBaseObjectType);
	%halfRadius = %radius / 2;
	while ((%targetObject = containerSearchNext()) != 0)
	{
		%coverage = calcExplosionCoverage(%position, %targetObject, $TypeMasks::InteriorObjectType | $TypeMasks::TerrainObjectType | $TypeMasks::ForceFieldObjectType | $TypeMasks::VehicleObjectType);
		if (%coverage == 0)
		{
		}
		else
		{
			%dist = containerSearchCurrRadiusDist();
			%distScale = %dist < %halfRadius ? 1 : 1 - (%dist - %halfRadius) / %halfRadius;
			%targetObject.Damage(%sourceObject, %position, %damage * %coverage * %distScale, %damageType);
			if (%impulse)
			{
				%impulseVec = VectorSub(%targetObject.getWorldBoxCenter(), %position);
				%impulseVec = VectorNormalize(%impulseVec);
				%impulseVec = VectorScale(%impulseVec, %impulse * %distScale);
				%targetObject.applyImpulse(%position, %impulseVec);
			}
		}
	}
}

function ProjectileData::OnCollision(%this, %obj, %col, %fade, %pos, %normal, %velocity)
{
	%client = %obj.client;
	%victimClient = %col.client;
	%clientBLID = getBL_IDFromObject(%obj);
	if (isObject(%client.miniGame))
	{
		if (getSimTime() - %client.lastF8Time < 3000)
		{
			return;
		}
	}
	%scale = getWord(%obj.getScale(), 2);
	%mask = $TypeMasks::PlayerObjectType | $TypeMasks::VehicleObjectType;
	if (%col.getType() & %mask)
	{
		if (!isObject(%client))
		{
			return;
		}
		%yourStuffOverride = 0;
		if (!isObject(%client.miniGame))
		{
			if (isObject(%col.spawnBrick))
			{
				if (%col.spawnBrick.getGroup().bl_id == %clientBLID)
				{
					%yourStuffOverride = 1;
				}
			}
		}
		if (miniGameCanDamage(%client, %col) == 1 || %yourStuffOverride)
		{
			%this.Damage(%obj, %col, %fade, %pos, %normal);
			%this.impactImpulse(%obj, %col, %velocity);
		}
	}
	else if (%col.getType() & $TypeMasks::FxBrickObjectType)
	{
		%mg = %client.miniGame;
		if (isObject(%mg))
		{
			%respawnTime = %mg.brickRespawnTime;
		}
		else
		{
			%respawnTime = mClampF($Pref::Server::BrickRespawnTime, 1000, 60000);
		}
		%blowUp = 0;
		%brickExplosionMaxVolume = %this.brickExplosionMaxVolume * %scale;
		%brickExplosionMaxVolumeFloating = %this.brickExplosionMaxVolumeFloating * %scale;
		if (%this.brickExplosionImpact == 1)
		{
			%blowUp = 1;
			if (!%col.canExplode(%brickExplosionMaxVolume, %brickExplosionMaxVolumeFloating))
			{
				%blowUp = 0;
			}
			if ($Server::LAN)
			{
				if (isObject(%mg))
				{
					if (!%mg.brickDamage)
					{
						%blowUp = 0;
					}
				}
			}
			else if (isObject(%mg))
			{
				if (miniGameCanDamage(%client, %col) != 1)
				{
					%blowUp = 0;
				}
			}
			else if (isObject(%obj.sourceObject))
			{
				if (%col.getGroup().bl_id != %clientBLID && %col.getGroup() != %obj.sourceObject.getGroup())
				{
					%blowUp = 0;
				}
			}
			else if (%col.getGroup().bl_id != %clientBLID)
			{
				%blowUp = 0;
			}
			if (%blowUp)
			{
				%col.onBlownUp(%client, %obj.sourceObject);
				transmitBrickExplosion(%pos, %this.brickExplosionForce, 0.02, %respawnTime, %col);
			}
		}
		if (!%blowUp)
		{
			%col.onProjectileHit(%obj, %client);
		}
	}
}

function ProjectileData::onExplode(%this, %obj, %pos)
{
	%client = %obj.client;
	if (!isObject(%client))
	{
		return;
	}
	%clientBLID = getBL_IDFromObject(%obj);
	if (isObject(%client.miniGame))
	{
		if (getSimTime() - %client.lastF8Time < 3000)
		{
			return;
		}
	}
	%mg = %client.miniGame;
	if (isObject(%mg))
	{
		%respawnTime = %mg.brickRespawnTime;
	}
	else
	{
		%respawnTime = mClampF($Pref::Server::BrickRespawnTime, 1000, 60000);
	}
	%explosion = %this.Explosion;
	%scale = getWord(%obj.getScale(), 2);
	if (%this.brickExplosionRadius > 0)
	{
		%count = 0;
		%mask = $TypeMasks::FxBrickAlwaysObjectType;
		%radius = %this.brickExplosionRadius * %scale;
		%maxVolume = %this.brickExplosionMaxVolume * %scale;
		%maxFloatingVolume = %this.brickExplosionMaxVolumeFloating * %scale;
		%explosionForce = %this.brickExplosionForce;
		%explosionRadius = %this.brickExplosionRadius * %scale;
		$CurrBrickKiller = %client;
		initContainerRadiusSearch(%pos, %radius, %mask);
		while ((%searchObj = containerSearchNext()) != 0)
		{
			%searchObj = getWord(%searchObj, 0);
			if (!%searchObj.canExplode(%maxVolume, %maxFloatingVolume))
			{
			}
			else if ($Server::LAN)
			{
				if (isObject(%mg))
				{
					if (!%mg.brickDamage)
					{
						continue;
					}
				}
			}
			else if (isObject(%mg))
			{
				if (miniGameCanDamage(%client, %searchObj) != 1)
				{
					continue;
				}
			}
			else if (isObject(%obj.sourceObject))
			{
				if (%searchObj.getGroup().bl_id != %clientBLID && %searchObj.getGroup() != %obj.sourceObject.getGroup())
				{
					continue;
				}
			}
			else if (%searchObj.getGroup().bl_id != %clientBLID)
			{
			}
			else
			{
				if (%searchObj.numEvents > 0)
				{
					%oldQuota = getCurrentQuotaObject();
					setCurrentQuotaObject(GlobalQuota);
					%searchObj.schedule(10, onBlownUp, %client, %obj.sourceObject);
					if (isObject(%oldQuota))
					{
						setCurrentQuotaObject(%oldQuota);
					}
				}
				if (%count == 0)
				{
					startNewBrickExplosion(%pos, %explosionForce, %explosionRadius, %respawnTime);
				}
				addBrickToExplosion(%searchObj);
				%count++;
				if (%count > 100)
				{
					sendBrickExplosion();
					%count = 0;
				}
			}
		}
		if (%count > 0)
		{
			sendBrickExplosion();
		}
	}
	if ((%explosion.damageRadius <= 0 || %explosion.radiusDamage <= 0) && ((%explosion.impulse && %explosion.impulseVertical) <= 0 || %explosion.impulseRadius <= 0))
	{
		return;
	}
	%damageRadius = %explosion.damageRadius * %scale;
	%impulseRadius = %explosion.impulseRadius * %scale;
	%radiusDamage = %explosion.radiusDamage * %scale;
	%impulseForce = %explosion.impulseForce * %scale;
	%impulseVertical = %explosion.impulseVertical * %scale;
	if (%damageRadius > %impulseRadius)
	{
		%radius = %explosion.damageRadius;
	}
	else
	{
		%radius = %explosion.impulseRadius;
	}
	%mask = $TypeMasks::PlayerObjectType | $TypeMasks::VehicleObjectType | $TypeMasks::CorpseObjectType;
	initContainerRadiusSearch(%pos, %radius, %mask);
	while ((%searchObj = containerSearchNext()) != 0)
	{
		if (isObject(%mg))
		{
			if (!%mg.SelfDamage)
			{
				if (%searchObj.client == %client)
				{
					continue;
				}
			}
		}
		%searchObj = getWord(%searchObj, 0);
		if (%searchObj.getType() & ($TypeMasks::PlayerObjectType | $TypeMasks::CorpseObjectType))
		{
			%searchPos = %searchObj.getHackPosition();
		}
		else
		{
			%searchPos = %searchObj.getWorldBoxCenter();
		}
		%dist = VectorDist(%searchPos, %pos);
		%damageDistFactor = 1 - (%dist / %damageRadius) * (%dist / %damageRadius);
		%impulseDistFactor = 1 - (%dist / %impulseRadius) * (%dist / %impulseRadius);
		%yourStuffOverride = 0;
		if (!isObject(%client.miniGame))
		{
			if (isObject(%searchObj.spawnBrick))
			{
				if (%searchObj.spawnBrick.getGroup().bl_id == %clientBLID)
				{
					%yourStuffOverride = 1;
				}
			}
		}
		if (miniGameCanDamage(%client, %searchObj) == 1 || %yourStuffOverride)
		{
			%this.radiusDamage(%obj, %searchObj, %damageDistFactor, %pos, %radiusDamage);
			%this.radiusImpulse(%obj, %searchObj, %impulseDistFactor, %pos, %impulseForce, %impulseVertical);
		}
	}
}

function ProjectileData::Damage(%this, %obj, %col, %fade, %pos, %normal)
{
	if (%this.directDamage <= 0)
	{
		return;
	}
	%damageType = $DamageType::Direct;
	if (%this.DirectDamageType)
	{
		%damageType = %this.DirectDamageType;
	}
	%scale = getWord(%obj.getScale(), 2);
	%directDamage = mClampF(%this.directDamage, -100, 100) * %scale;
	if (%col.getType() & $TypeMasks::PlayerObjectType)
	{
		%col.Damage(%obj, %pos, %directDamage, %damageType);
	}
	else
	{
		%col.Damage(%obj, %pos, %directDamage, %damageType);
	}
}

function ProjectileData::radiusDamage(%this, %obj, %col, %distanceFactor, %pos, %damageAmt)
{
	if (%distanceFactor <= 0)
	{
		return;
	}
	else if (%distanceFactor > 1)
	{
		%distanceFactor = 1;
	}
	%damageAmt *= %distanceFactor;
	if (%damageAmt)
	{
		%damageType = $DamageType::Radius;
		if (%this.RadiusDamageType)
		{
			%damageType = %this.RadiusDamageType;
		}
		%col.Damage(%obj, %pos, %damageAmt, %damageType);
		if (%this.Explosion.playerBurnTime > 0)
		{
			if (%col.getType() & ($TypeMasks::PlayerObjectType | $TypeMasks::CorpseObjectType))
			{
				%doBurn = 1;
				if (%col.isMounted())
				{
					%mountData = %col.getObjectMount().getDataBlock();
					if (%mountData.protectPassengersBurn)
					{
						%doBurn = 0;
					}
				}
				if (%doBurn)
				{
					%col.burn(%this.Explosion.playerBurnTime * %distanceFactor);
				}
			}
		}
	}
}

function ProjectileData::radiusImpulse(%this, %obj, %col, %distanceFactor, %pos, %impulseAmt, %verticalAmt)
{
	if (%distanceFactor <= 0)
	{
		return;
	}
	else if (%distanceFactor > 1)
	{
		%distanceFactor = 1;
	}
	if (%impulseAmt || %verticalAmt)
	{
		if (%col.getType() & ($TypeMasks::PlayerObjectType | $TypeMasks::CorpseObjectType))
		{
			%colPos = %col.getHackPosition();
		}
		else
		{
			%colPos = %col.getWorldBoxCenter();
		}
		%impulseVec = VectorSub(%colPos, %pos);
		%impulseVecX = getWord(%impulseVec, 0);
		%impulseVecY = getWord(%impulseVec, 1);
		%impulseVecZ = getWord(%impulseVec, 2);
		if (%impulseVecZ < 0)
		{
			%mask = $TypeMasks::TerrainObjectType | $TypeMasks::InteriorObjectType | $TypeMasks::FxBrickObjectType;
			%start = %colPos;
			%end = VectorAdd(%colPos, "0 0 -3");
			if (containerRayCast(%start, %end, %mask))
			{
				%impulseVecZ = 0;
				%impulseVec = %impulseVecX SPC %impulseVecY SPC %impulseVecZ;
			}
		}
		%impulseVec = VectorNormalize(%impulseVec);
		%impulseVec = VectorScale(%impulseVec, %impulseAmt * %distanceFactor);
		%col.applyImpulse(%pos, %impulseVec);
		if (%obj.upVector !$= "")
		{
			%impulseVec = VectorScale(%obj.upVector, %verticalAmt * %distanceFactor);
		}
		else
		{
			%impulseVec = VectorScale("0 0 1", %verticalAmt * %distanceFactor);
		}
		%col.applyImpulse(%pos, %impulseVec);
		if (isObject(%obj.client))
		{
			%col.lastPusher = %obj.client;
			%col.lastPushTime = getSimTime();
		}
	}
}

function ProjectileData::impactImpulse(%this, %obj, %col, %vector)
{
	%vector = VectorNormalize(%vector);
	%colPos = %col.getPosition();
	%col.preHitVelocity = %col.getVelocity();
	%scale = getWord(%obj.getScale(), 2);
	%impulse = VectorScale(%vector, %this.impactImpulse);
	%impulse = VectorScale(%impulse, %scale);
	%verticalImpulse = VectorScale(%this.verticalImpulse, %scale);
	%col.applyImpulse(%colPos, %impulse);
	%col.applyImpulse(%colPos, "0 0" SPC %verticalImpulse);
	if (isObject(%obj.client))
	{
		%col.lastPusher = %obj.client;
		%col.lastPushTime = getSimTime();
	}
}

function Projectile::onAdd(%this)
{
	%this.originPoint = %this.initialPosition;
}

datablock TSShapeConstructor(mDts)
{
	baseShape = "base/data/shapes/player/m.dts";
	sequence0 = "base/data/shapes/player/m_root.dsq root";
	sequence1 = "base/data/shapes/player/m_run.dsq run";
	sequence2 = "base/data/shapes/player/m_run.dsq walk";
	sequence3 = "base/data/shapes/player/m_back.dsq back";
	sequence4 = "base/data/shapes/player/m_side.dsq side";
	sequence5 = "base/data/shapes/player/m_crouch.dsq crouch";
	sequence6 = "base/data/shapes/player/m_crouchRun.dsq crouchRun";
	sequence7 = "base/data/shapes/player/m_crouchBack.dsq crouchBack";
	sequence8 = "base/data/shapes/player/m_crouchSide.dsq crouchSide";
	sequence9 = "base/data/shapes/player/m_look.dsq look";
	sequence10 = "base/data/shapes/player/m_headSide.dsq headside";
	sequence11 = "base/data/shapes/player/m_headup.dsq headUp";
	sequence12 = "base/data/shapes/player/m_standjump.dsq jump";
	sequence13 = "base/data/shapes/player/m_standjump.dsq standjump";
	sequence14 = "base/data/shapes/player/m_fall.dsq fall";
	sequence15 = "base/data/shapes/player/m_root.dsq land";
	sequence16 = "base/data/shapes/player/m_armAttack.dsq armAttack";
	sequence17 = "base/data/shapes/player/m_armReadyLeft.dsq armReadyLeft";
	sequence18 = "base/data/shapes/player/m_armReadyRight.dsq armReadyRight";
	sequence19 = "base/data/shapes/player/m_armReadyBoth.dsq armReadyBoth";
	sequence20 = "base/data/shapes/player/m_spearReady.dsq spearready";
	sequence21 = "base/data/shapes/player/m_spearThrow.dsq spearThrow";
	sequence22 = "base/data/shapes/player/m_talk.dsq talk";
	sequence23 = "base/data/shapes/player/m_death1.dsq death1";
	sequence24 = "base/data/shapes/player/m_shiftUp.dsq shiftUp";
	sequence25 = "base/data/shapes/player/m_shiftDown.dsq shiftDown";
	sequence26 = "base/data/shapes/player/m_shiftAway.dsq shiftAway";
	sequence27 = "base/data/shapes/player/m_shiftTo.dsq shiftTo";
	sequence28 = "base/data/shapes/player/m_shiftLeft.dsq shiftLeft";
	sequence29 = "base/data/shapes/player/m_shiftRight.dsq shiftRight";
	sequence30 = "base/data/shapes/player/m_rotCW.dsq rotCW";
	sequence31 = "base/data/shapes/player/m_rotCCW.dsq rotCCW";
	sequence32 = "base/data/shapes/player/m_undo.dsq undo";
	sequence33 = "base/data/shapes/player/m_plant.dsq plant";
	sequence34 = "base/data/shapes/player/m_sit.dsq sit";
	sequence35 = "base/data/shapes/player/m_wrench.dsq wrench";
	sequence36 = "base/data/shapes/player/m_activate.dsq activate";
	sequence37 = "base/data/shapes/player/m_activate2.dsq activate2";
	sequence38 = "base/data/shapes/player/m_leftrecoil.dsq leftrecoil";
};
$CorpseTimeoutValue = 5 * 1000;
$DamageLava = 0.01;
$DamageHotLava = 0.01;
$DamageCrustyLava = 0.01;
$PlayerDeathAnim::TorsoFrontFallForward = 1;
$PlayerDeathAnim::TorsoFrontFallBack = 2;
$PlayerDeathAnim::TorsoBackFallForward = 3;
$PlayerDeathAnim::TorsoLeftSpinDeath = 4;
$PlayerDeathAnim::TorsoRightSpinDeath = 5;
$PlayerDeathAnim::LegsLeftGimp = 6;
$PlayerDeathAnim::LegsRightGimp = 7;
$PlayerDeathAnim::TorsoBackFallForward = 8;
$PlayerDeathAnim::HeadFrontDirect = 9;
$PlayerDeathAnim::HeadBackFallForward = 10;
$PlayerDeathAnim::ExplosionBlowBack = 11;
datablock AudioProfile(errorSound)
{
	fileName = "base/data/sound/error.wav";
	description = AudioClose3d;
	preload = 1;
};
datablock AudioProfile(JumpSound)
{
	fileName = "base/data/sound/jump.wav";
	description = AudioClosest3d;
	preload = 1;
};
datablock AudioProfile(DeathCrySound)
{
	fileName = "base/data/sound/death.wav";
	description = AudioClose3d;
	preload = 1;
};
datablock AudioProfile(PainCrySound)
{
	fileName = "base/data/sound/pain.wav";
	description = AudioClose3d;
	preload = 1;
};
datablock AudioProfile(ArmorMoveBubblesSound)
{
	fileName = "base/data/sound/underWater1.wav";
	description = AudioCloseLooping3d;
	preload = 1;
};
datablock AudioProfile(WaterBreathMaleSound)
{
	fileName = "base/data/sound/underWater1.wav";
	description = AudioClosestLooping3d;
	preload = 1;
};
datablock AudioProfile(Splash1Sound)
{
	fileName = "base/data/sound/splash1.wav";
	description = AudioClose3d;
	preload = 1;
};
datablock AudioProfile(exitWaterSound)
{
	fileName = "base/data/sound/exitWater.wav";
	description = AudioClose3d;
	preload = 1;
};
datablock AudioProfile(playerMountSound)
{
	fileName = "base/data/sound/playerMount.wav";
	description = AudioClosest3d;
	preload = 1;
};
datablock ParticleData(PlayerBubbleParticle)
{
	dragCoefficient = 0;
	gravityCoefficient = 1;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 1200;
	lifetimeVarianceMS = 400;
	useInvAlpha = 0;
	textureName = "base/data/particles/bubble";
	colors[0] = "0.7 0.8 1.0 0.8";
	colors[1] = "0.7 0.8 1.0 0.8";
	colors[2] = "0.7 0.8 1.0 0.0";
	sizes[0] = 0.2;
	sizes[1] = 0.2;
	sizes[2] = 0.2;
	times[0] = 0;
	times[1] = 0.5;
	times[2] = 1;
};
datablock ParticleEmitterData(PlayerBubbleEmitter)
{
	ejectionPeriodMS = 1;
	periodVarianceMS = 0;
	ejectionVelocity = 7;
	ejectionOffset = 0.4;
	velocityVariance = 3;
	thetaMin = 0;
	thetaMax = 30;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = "PlayerBubbleParticle";
	useEmitterColors = 1;
	useInvAlpha = 1;
	uiName = "Player Bubbles";
};
datablock ParticleData(PlayerFoamParticle)
{
	dragCoefficient = 0;
	gravityCoefficient = 1;
	inheritedVelFactor = 0.5;
	constantAcceleration = 0;
	lifetimeMS = 400;
	lifetimeVarianceMS = 100;
	useInvAlpha = 0;
	spinRandomMin = -90;
	spinRandomMax = 500;
	textureName = "base/data/particles/bubble";
	colors[0] = "0.7 0.8 1.0 1.0";
	colors[1] = "0.7 0.8 1.0 0.85";
	colors[2] = "0.7 0.8 1.0 0.00";
	sizes[0] = 0.1;
	sizes[1] = 0.1;
	sizes[2] = 0.1;
	times[0] = 0;
	times[1] = 0.5;
	times[2] = 1;
};
datablock ParticleEmitterData(PlayerFoamEmitter)
{
	ejectionPeriodMS = 10;
	periodVarianceMS = 0;
	ejectionVelocity = 3;
	velocityVariance = 1;
	ejectionOffset = 0.4;
	thetaMin = 0;
	thetaMax = 90;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = "PlayerFoamParticle";
	useEmitterColors = 1;
	uiName = "Player Foam";
};
datablock ParticleData(PlayerFoamDropletsParticle)
{
	dragCoefficient = 0;
	gravityCoefficient = 1;
	inheritedVelFactor = 0.5;
	constantAcceleration = -0;
	lifetimeMS = 500;
	lifetimeVarianceMS = 0;
	textureName = "base/data/particles/bubble";
	colors[0] = "0.7 0.8 1.0 1.0";
	colors[1] = "0.7 0.8 1.0 0.85";
	colors[2] = "0.7 0.8 1.0 0.0";
	sizes[0] = 0.15;
	sizes[1] = 0.15;
	sizes[2] = 0.15;
	times[0] = 0;
	times[1] = 0.8;
	times[2] = 1;
};
datablock ParticleEmitterData(PlayerFoamDropletsEmitter)
{
	ejectionPeriodMS = 7;
	periodVarianceMS = 0;
	ejectionVelocity = 2;
	velocityVariance = 1;
	ejectionOffset = 0.4;
	thetaMin = 40;
	thetaMax = 70;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	orientParticles = 0;
	particles = "PlayerFoamDropletsParticle";
	useEmitterColors = 1;
};
datablock SplashData(PlayerSplash)
{
	numSegments = 15;
	ejectionFreq = 15;
	ejectionAngle = 60;
	ringLifetime = 0.5;
	lifetimeMS = 300;
	velocity = 4;
	startRadius = 0.2;
	acceleration = -3;
	texWrap = 5;
	texture = "base/data/particles/bubble";
	colors[0] = "0.7 0.8 1.0 0.0";
	colors[1] = "0.7 0.8 1.0 0.3";
	colors[2] = "0.7 0.8 1.0 0.7";
	colors[3] = "0.7 0.8 1.0 0.0";
	times[0] = 0;
	times[1] = 0.4;
	times[2] = 0.8;
	times[3] = 1;
};
datablock ParticleData(playerJetParticle)
{
	dragCoefficient = 3;
	windCoefficient = 0;
	gravityCoefficient = 0;
	inheritedVelFactor = 1;
	constantAcceleration = 0;
	lifetimeMS = 130;
	lifetimeVarianceMS = 10;
	spinSpeed = 500;
	spinRandomMin = -150;
	spinRandomMax = 150;
	useInvAlpha = 0;
	animateTexture = 0;
	textureName = "base/data/particles/cloud";
	colors[0] = "0 0 1 1.0";
	colors[1] = "1 0.5 0 1.0";
	colors[2] = "1 0 0 0.000";
	sizes[0] = 0.6;
	sizes[1] = 0.4;
	sizes[2] = 0.7;
	times[0] = 0;
	times[1] = 0.1;
	times[2] = 1;
};
datablock ParticleEmitterData(playerJetEmitter)
{
	ejectionPeriodMS = 6;
	periodVarianceMS = 0;
	ejectionVelocity = 5;
	velocityVariance = 0;
	ejectionOffset = 0;
	thetaMin = 0;
	thetaMax = 0;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = playerJetParticle;
};
datablock ParticleData(playerJetGroundParticle)
{
	dragCoefficient = 3;
	windCoefficient = 0;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 500;
	lifetimeVarianceMS = 100;
	spinSpeed = 500;
	spinRandomMin = -150;
	spinRandomMax = 150;
	useInvAlpha = 1;
	animateTexture = 0;
	textureName = "base/data/particles/cloud";
	colors[0] = "1 1 1 0.00";
	colors[1] = "0.5 0.5 0.5 0.250";
	colors[2] = "0.8 0.8 1.0 0.000";
	sizes[0] = 0.4;
	sizes[1] = 0.4;
	sizes[2] = 1.5;
	times[0] = 0;
	times[1] = 0.3;
	times[2] = 1;
};
datablock ParticleEmitterData(playerJetGroundEmitter)
{
	ejectionPeriodMS = 14;
	periodVarianceMS = 0;
	ejectionVelocity = 4;
	velocityVariance = 0;
	ejectionOffset = 0;
	thetaMin = 85;
	thetaMax = 90;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = playerJetGroundParticle;
};
datablock PlayerData(PlayerStandardArmor)
{
	renderFirstPerson = 0;
	emap = 1;
	className = Armor;
	shapeFile = "base/data/shapes/player/m.dts";
	cameraMaxDist = 8;
	cameraTilt = 0.261;
	cameraVerticalOffset = 0.75;
	cameraDefaultFov = 90;
	cameraMinFov = 5;
	cameraMaxFov = 120;
	aiAvoidThis = 1;
	minLookAngle = -1.5708;
	maxLookAngle = 1.5708;
	maxFreelookAngle = 3;
	mass = 90;
	drag = 0.1;
	density = 0.7;
	maxDamage = 100;
	maxEnergy = 100;
	repairRate = 0.33;
	rechargeRate = 0.8;
	runForce = 48 * 90;
	runEnergyDrain = 0;
	minRunEnergy = 0;
	maxForwardSpeed = 7;
	maxBackwardSpeed = 4;
	maxSideSpeed = 6;
	airControl = 0.1;
	maxForwardCrouchSpeed = 3;
	maxBackwardCrouchSpeed = 2;
	maxSideCrouchSpeed = 2;
	maxUnderwaterForwardSpeed = 8.4;
	maxUnderwaterBackwardSpeed = 7.8;
	maxUnderwaterSideSpeed = 7.8;
	jumpForce = 12 * 90;
	jumpEnergyDrain = 0;
	minJumpEnergy = 0;
	jumpDelay = 3;
	minJetEnergy = 0;
	jetEnergyDrain = 0;
	canJet = 1;
	minImpactSpeed = 30;
	speedDamageScale = 3.8;
	boundingBox = VectorScale("1.25 1.25 2.65", 4);
	crouchBoundingBox = VectorScale("1.25 1.25 1.00", 4);
	pickupRadius = 0.625;
	decalOffset = 0.25;
	jetEmitter = playerJetEmitter;
	jetGroundEmitter = playerJetGroundEmitter;
	jetGroundDistance = 4;
	footPuffNumParts = 10;
	footPuffRadius = 0.25;
	Splash = PlayerSplash;
	splashVelocity = 4;
	splashAngle = 67;
	splashFreqMod = 300;
	splashVelEpsilon = 0.6;
	bubbleEmitTime = 0.1;
	splashEmitter[0] = PlayerFoamDropletsEmitter;
	splashEmitter[1] = PlayerFoamEmitter;
	splashEmitter[2] = PlayerBubbleEmitter;
	mediumSplashSoundVelocity = 10;
	hardSplashSoundVelocity = 20;
	exitSplashSoundVelocity = 5;
	runSurfaceAngle = 70;
	jumpSurfaceAngle = 80;
	minJumpSpeed = 20;
	maxJumpSpeed = 30;
	horizMaxSpeed = 68;
	horizResistSpeed = 33;
	horizResistFactor = 0.35;
	upMaxSpeed = 80;
	upResistSpeed = 25;
	upResistFactor = 0.3;
	footstepSplashHeight = 0.35;
	JumpSound = JumpSound;
	impactWaterEasy = Splash1Sound;
	impactWaterMedium = Splash1Sound;
	impactWaterHard = Splash1Sound;
	groundImpactMinSpeed = 10;
	groundImpactShakeFreq = "4.0 4.0 4.0";
	groundImpactShakeAmp = "1.0 1.0 1.0";
	groundImpactShakeDuration = 0.8;
	groundImpactShakeFalloff = 10;
	exitingWater = exitWaterSound;
	maxItems = 10;
	maxWeapons = 5;
	maxTools = 5;
	uiName = "Standard Player";
	canRide = 1;
	showEnergyBar = 0;
	brickImage = brickImage;
};
function Armor::onAdd(%this, %obj)
{
	applyDefaultCharacterPrefs(%obj);
	%obj.mountVehicle = 1;
	%obj.setRepairRate(0);
}

function Armor::onRemove(%this, %obj)
{
	if (isObject(%obj.client))
	{
		if (%obj.client.Player == %obj)
		{
			%obj.client.Player = 0;
		}
	}
	if (isObject(%obj.light))
	{
		%obj.light.delete();
	}
}

function Armor::onNewDataBlock(%this, %player)
{
	if (!isObject(%player.client))
	{
		applyDefaultCharacterPrefs(%player);
	}
	else
	{
		applyCharacterPrefs(%player.client);
	}
	%data = %this;
	if (%data.rideable)
	{
		%count = %player.getMountedObjectCount();
		%list = "";
		for (%i = 0; %i < %count; %i++)
		{
			%rider = %player.getMountedObject(0);
			if (%i == 0)
			{
				%list = %rider;
			}
			else
			{
				%list = %list SPC %rider;
			}
		}
		for (%i = 0; %i < %count; %i++)
		{
			%rider = getWord(%list, %i);
			%rider.getDataBlock().doDismount(%rider, 1);
		}
		if (%data.nummountpoints < %count)
		{
			%count = %data.nummountpoints;
		}
		for (%i = 0; %i < %count; %i++)
		{
			%rider = getWord(%list, %i);
			%mountNode = %data.mountNode[%i];
			if (%mountNode $= "")
			{
				%mountNode = %i;
			}
			%player.mountObject(%rider, %mountNode);
			if (%i == 0)
			{
				%rider.setControlObject(%player);
			}
		}
	}
	else
	{
		%count = %player.getMountedObjectCount();
		for (%i = 0; %i < %count; %i++)
		{
			%rider = %player.getMountedObject(0);
			%rider.getDataBlock().doDismount(%rider, 1);
		}
	}
}

function Armor::onMount(%this, %obj, %vehicle, %node)
{
	if (%node == 0)
	{
		if (%vehicle.getControllingClient() == 0)
		{
			%obj.setControlObject(%vehicle);
			%vehicle.lastDrivingClient = %obj.client;
		}
	}
	else
	{
		%obj.setControlObject(%obj);
	}
	%obj.setTransform("0 0 0 0 0 1 0");
	%obj.playThread(0, %vehicle.getDataBlock().mountThread[%node]);
	ServerPlay3D(playerMountSound, %obj.getPosition());
	if (%vehicle.getDataBlock().lookUpLimit !$= "")
	{
		%obj.setLookLimits(%vehicle.getDataBlock().lookUpLimit, %vehicle.getDataBlock().lookDownLimit);
	}
}

function Armor::onUnMount(%this, %obj, %vehicle, %node)
{
	%obj.lastMountTime = getSimTime();
	if (%node == 0)
	{
		if (isObject(%vehicle))
		{
			%vehicle.onDriverLeave(%obj);
		}
	}
	%obj.setLookLimits(1, 0);
	%obj.playThread(0, root);
}

function Armor::doDismount(%this, %obj, %forced)
{
	if (!%obj.isMounted())
	{
		return;
	}
	if (!%obj.canDismount && !%forced)
	{
		return;
	}
	%vehicle = %obj.getObjectMount();
	%vehicleVelocity = %vehicle.getVelocity();
	if (%vehicle.getDataBlock().doSimpleDismount)
	{
		%obj.unmount();
		%this.onUnMount(%obj);
		%obj.setControlObject(%obj);
		%obj.setVelocity(%vehicleVelocity);
		return;
	}
	%pos = getWords(%obj.getTransform(), 0, 2);
	%oldPos = %pos;
	%scale = getWord(%vehicle.getScale(), 2);
	%vec[0] = VectorScale(" 0  0  2.2", %scale);
	%vec[1] = VectorScale(" 0  0  3", %scale);
	%vec[2] = VectorScale(" 0  0 -3", %scale);
	%vec[3] = VectorScale(" 3  0  0", %scale);
	%vec[4] = VectorScale("-3  0  0", %scale);
	%impulseVec = "0 0 0";
	%vec[0] = MatrixMulVector(%obj.getTransform(), %vec[0]);
	%pos = "0 0 0";
	%numAttempts = 5;
	%success = -1;
	for (%i = 0; %i < %numAttempts; %i++)
	{
		%pos = VectorAdd(%oldPos, VectorScale(%vec[%i], 1));
		if (%obj.checkDismountPoint(%oldPos, %pos))
		{
			%success = %i;
			%impulseVec = %vec[%i];
			break;
		}
	}
	if (%forced && %success == -1)
	{
		%pos = %oldPos;
	}
	%obj.mountVehicle = 0;
	%obj.unmount();
	%this.onUnMount(%obj);
	%obj.setControlObject(%obj);
	%obj.setVelocity(%vehicleVelocity);
	%obj.setTransform(%pos);
	%obj.applyImpulse(%pos, VectorScale(%impulseVec, %obj.getDataBlock().mass));
}

function Armor::OnCollision(%this, %obj, %col, %vec, %speed)
{
	if (%obj.getState() $= "Dead")
	{
		return;
	}
	if (%col.getDamagePercent() >= 1)
	{
		return;
	}
	%colClassName = %col.getClassName();
	if (%colClassName $= "Item")
	{
		%client = %obj.client;
		%colData = %col.getDataBlock();
		for (%i = 0; %i < %this.maxTools; %i++)
		{
			if (%obj.tool[%i] == %colData)
			{
				return;
			}
		}
		%obj.pickup(%col);
	}
	else if (%colClassName $= "Player" || %colClassName $= "AIPlayer")
	{
		if (%col.getDataBlock().canRide && %this.rideable && %this.nummountpoints > 0)
		{
			if (getSimTime() - %col.lastMountTime <= $Game::MinMountTime)
			{
				return;
			}
			%colZpos = getWord(%col.getPosition(), 2);
			%objZpos = getWord(%obj.getPosition(), 2);
			if (%colZpos <= %objZpos + 0.2)
			{
				return;
			}
			%canUse = 0;
			if (isObject(%obj.spawnBrick))
			{
				%vehicleOwner = findClientByBL_ID(%obj.spawnBrick.getGroup().bl_id);
			}
			if (isObject(%vehicleOwner))
			{
				if (getTrustLevel(%col, %obj) >= $TrustLevel::RideVehicle)
				{
					%canUse = 1;
				}
			}
			else
			{
				%canUse = 1;
			}
			if (miniGameCanUse(%col, %obj) == 1)
			{
				%canUse = 1;
			}
			if (miniGameCanUse(%col, %obj) == 0)
			{
				%canUse = 0;
			}
			if (!%canUse)
			{
				if (!isObject(%obj.spawnBrick))
				{
					return;
				}
				%ownerName = %obj.spawnBrick.getGroup().name;
				%msg = %ownerName @ " does not trust you enough to do that";
				if ($lastError == $LastError::Trust)
				{
					%msg = %ownerName @ " does not trust you enough to ride.";
				}
				else if ($lastError == $LastError::MiniGameDifferent)
				{
					if (isObject(%col.client.miniGame))
					{
						%msg = "This vehicle is not part of the mini-game.";
					}
					else
					{
						%msg = "This vehicle is part of a mini-game.";
					}
				}
				else if ($lastError == $LastError::MiniGameNotYours)
				{
					%msg = "You do not own this vehicle.";
				}
				else if ($lastError == $LastError::NotInMiniGame)
				{
					%msg = "This vehicle is not part of the mini-game.";
				}
				commandToClient(%col.client, 'CenterPrint', %msg, 1);
				return;
			}
			for (%i = 0; %i < %this.nummountpoints; %i++)
			{
				if (%this.mountNode[%i] $= "")
				{
					%mountNode = %i;
				}
				else
				{
					%mountNode = %this.mountNode[%i];
				}
				%blockingObj = %obj.getMountNodeObject(%mountNode);
				if (isObject(%blockingObj))
				{
					if (!%blockingObj.getDataBlock().rideable)
					{
						continue;
					}
					if (%blockingObj.getMountedObject(0))
					{
						continue;
					}
					%blockingObj.mountObject(%col, 0);
					if (%blockingObj.getControllingClient() == 0)
					{
						%col.setControlObject(%blockingObj);
					}
					%col.setTransform("0 0 0 0 0 1 0");
					%col.setActionThread(root, 0);
				}
				else
				{
					%obj.mountObject(%col, %mountNode);
					%col.setActionThread(root, 0);
					if (%i == 0)
					{
						if (%obj.getControllingClient() == 0)
						{
							%col.setControlObject(%obj);
						}
						if (isObject(%obj.spawnBrick))
						{
							%obj.lastControllingClient = %col;
						}
					}
					break;
				}
			}
		}
	}
}

function Armor::onImpact(%this, %obj, %collidedObject, %vec, %vecLen)
{
	if (%collidedObject.getClassName() $= "StaticShape")
	{
		if (%collidedObject.getDataBlock().className $= "Glass")
		{
			if (!%collidedObject.indestructable)
			{
				%collidedObject.explode();
				return;
			}
		}
	}
	%doDamage = 0;
	%mg = getMiniGameFromObject(%obj);
	if (isObject(%mg))
	{
		if (%mg.fallingDamage)
		{
			%doDamage = 1;
		}
	}
	else if ($pref::Server::FallingDamage == 1)
	{
		%doDamage = 1;
	}
	%image = %obj.getMountedImage(0);
	if (%image)
	{
		if (%image.getId() == AdminWandImage.getId())
		{
			%doDamage = 0;
		}
	}
	%scale = getWord(%obj.getScale(), 2);
	if (%vecLen < %this.minImpactSpeed * %scale)
	{
		%doDamage = 0;
	}
	if (%doDamage)
	{
		%angle = VectorDot(VectorNormalize(%vec), "0 0 1");
		if (%angle > 0.5)
		{
			%damageType = $DamageType::Fall;
		}
		else
		{
			%damageType = $DamageType::Impact;
		}
		%obj.Damage(0, VectorAdd(%obj.getPosition(), %vec), %vecLen * %this.speedDamageScale, %damageType);
	}
}

function Armor::Damage(%data, %obj, %sourceObject, %position, %damage, %damageType)
{
	if (%obj.getState() $= "Dead")
	{
		return;
	}
	if (getSimTime() - %obj.spawnTime < $Game::PlayerInvulnerabilityTime && !%obj.hasShotOnce)
	{
		return;
	}
	if (%obj.isMounted() && %damageType != $DamageType::Suicide && %data.rideable == 0)
	{
		%mountData = %obj.getObjectMount().getDataBlock();
		if ($Damage::Direct[%damageType])
		{
			if (%mountData.protectPassengersDirect)
			{
				return;
			}
		}
		else if (%mountData.protectPassengersRadius)
		{
			return;
		}
	}
	if (%obj.isCrouched())
	{
		if ($Damage::Direct[%damageType])
		{
			%damage = %damage * 2.1;
		}
		else
		{
			%damage = %damage * 0.75;
		}
	}
	%scale = getWord(%obj.getScale(), 2);
	%damage = %damage / %scale;
	%obj.applyDamage(%damage);
	%location = "Body";
	%client = %obj.client;
	if (isObject(%sourceObject))
	{
		if (%sourceObject.getClassName() $= "GameConnection")
		{
			%sourceClient = %sourceObject;
		}
		else
		{
			%sourceClient = %sourceObject.client;
		}
	}
	else
	{
		%sourceClient = 0;
	}
	if (isObject(%sourceObject))
	{
		if (%sourceObject.getType() & $TypeMasks::VehicleObjectType)
		{
			if (%sourceObject.getControllingClient())
			{
				%sourceClient = %sourceObject.getControllingClient();
			}
		}
	}
	if (%obj.getState() $= "Dead")
	{
		if (isObject(%client))
		{
			%client.onDeath(%sourceObject, %sourceClient, %damageType, %location);
		}
		else if (isObject(%obj.spawnBrick))
		{
			%mg = getMiniGameFromObject(%sourceObject);
			if (isObject(%mg))
			{
				%obj.spawnBrick.spawnVehicle(%mg.vehicleReSpawnTime);
			}
			else
			{
				%obj.spawnBrick.spawnVehicle(0);
			}
		}
	}
	if ($Damage::Direct[%damageType] == 1)
	{
		%obj.lastDirectDamageType = %damageType;
		%obj.lastDirectDamageTime = getSimTime();
	}
	if (getSimTime() - %obj.lastPainTime > 300)
	{
		%obj.painLevel = %damage;
	}
	else
	{
		%obj.painLevel += %damage;
	}
	%obj.lastPainTime = getSimTime();
	if (%data.useCustomPainEffects == 1)
	{
		if (%obj.painLevel >= 40)
		{
			if (isObject(%data.PainHighImage))
			{
				%obj.emote(%data.PainHighImage, 1);
			}
		}
		else if (%obj.painLevel >= 25)
		{
			if (isObject(%data.PainMidImage))
			{
				%obj.emote(%data.PainMidImage, 1);
			}
		}
		else if (isObject(%data.PainLowImage))
		{
			%obj.emote(%data.PainLowImage, 1);
		}
	}
	else if (%obj.painLevel >= 40)
	{
		%obj.emote(PainHighImage, 1);
	}
	else if (%obj.painLevel >= 25)
	{
		%obj.emote(PainMidImage, 1);
	}
	else
	{
		%obj.emote(PainLowImage, 1);
	}
}

function Armor::onDamage(%this, %obj, %delta)
{
	if (%delta > 0 && %obj.getState() !$= "Dead")
	{
		%flash = %obj.getDamageFlash() + (%delta / %this.maxDamage) * 2;
		if (%flash > 0.75)
		{
			%flash = 0.75;
		}
		%obj.setDamageFlash(%flash);
		if (%delta > 10)
		{
			%obj.playPain();
		}
	}
}

function Armor::onDisabled(%this, %obj, %state)
{
	%obj.playDeathCry();
	%obj.playDeathAnimation();
	%obj.setDamageFlash(0.75);
	if (%obj.getObjectMount())
	{
		%vehicle = %obj.getObjectMount();
		%mask = $TypeMasks::PlayerObjectType | $TypeMasks::VehicleObjectType;
		if (%vehicle.getType() & %mask)
		{
			%vehicle.onDriverLeave();
		}
		%obj.unmount();
	}
	%obj.setImageTrigger(0, 0);
	if (isObject(%obj.tempBrick))
	{
		%obj.tempBrick.delete();
		%obj.tempBrick = 0;
	}
	%client = %obj.client;
	%client.deathTime = getSimTime();
	%oldQuota = getCurrentQuotaObject();
	if (isObject(%oldQuota))
	{
		setCurrentQuotaObject(GlobalQuota);
	}
	%obj.schedule($CorpseTimeoutValue, RemoveBody);
	%count = %obj.getMountedObjectCount();
	for (%i = 0; %i < %count; %i++)
	{
		%rider = %obj.getMountedObject(0);
		%rider.getDataBlock().doDismount(%rider, 1);
	}
	if (isObject(%oldQuota))
	{
		setCurrentQuotaObject(%oldQuota);
	}
}

function Player::RemoveBody(%obj)
{
	%p = new Projectile()
	{
		dataBlock = deathProjectile;
		initialVelocity = "0 0 0";
		initialPosition = %obj.getTransform();
		sourceObject = %obj;
		sourceSlot = 0;
		client = %obj.client;
	};
	%p.setScale(%obj.getScale());
	MissionCleanup.add(%p);
	%client = %obj.client;
	if (isObject(%client))
	{
		if (isObject(%client.light))
		{
			if (%client.light.Player == %obj)
			{
				%client.light.delete();
			}
		}
	}
	%obj.schedule(10, delete);
}

function Armor::onLeaveMissionArea(%this, %obj)
{
	if (isObject(%obj.client))
	{
		%obj.client.onLeaveMissionArea();
	}
}

function Armor::onEnterMissionArea(%this, %obj)
{
	if (isObject(%obj.client))
	{
		%obj.client.onEnterMissionArea();
	}
}

function Armor::onEnterLiquid(%this, %obj, %coverage, %type)
{
}

function Armor::onLeaveLiquid(%this, %obj, %type)
{
}

function Armor::onTrigger(%this, %obj, %triggerNum, %val)
{
	if (%triggerNum == 0)
	{
		if (%val)
		{
			if (getSimTime() - %obj.spawnTime < 100)
			{
				return;
			}
			if (%obj.getMountedImage(0) <= 0)
			{
				%obj.ActivateStuff();
			}
		}
	}
}

function Player::ActivateStuff(%player)
{
	%client = %player.client;
	if (isObject(%client.miniGame))
	{
		if (%client.miniGame.weaponDamage == 1)
		{
			if (getSimTime() - %client.lastF8Time < 5000)
			{
				return;
			}
		}
	}
	%start = %player.getEyePoint();
	%vec = %player.getEyeVector();
	%scale = getWord(%player.getScale(), 2);
	%end = VectorAdd(%start, VectorScale(%vec, 10 * %scale));
	%mask = $TypeMasks::FxBrickObjectType | $TypeMasks::VehicleObjectType;
	if (%player.isMounted())
	{
		%exempt = %player.getObjectMount();
	}
	else
	{
		%exempt = %player;
	}
	%search = containerRayCast(%start, %end, %mask, %exempt);
	%victim = getWord(%search, 0);
	%currTime = getSimTime();
	if (%currTime - %player.lastActivateTime <= 320)
	{
		%player.activateLevel++;
	}
	else
	{
		%player.activateLevel = 0;
	}
	%player.lastActivateTime = getSimTime();
	if (%player.activateLevel >= 5)
	{
		%player.playThread(3, activate2);
	}
	else
	{
		%player.playThread(3, activate);
	}
	if (%victim)
	{
		%pos = getWords(%search, 1, 3);
		if (%victim.getType() & $TypeMasks::FxBrickObjectType)
		{
			%diff = VectorSub(%start, %pos);
			%len = VectorLen(%diff);
			if (%len <= $Game::BrickActivateRange * %scale)
			{
				%victim.onActivate(%player, %client, %pos, %vec);
			}
		}
		else
		{
			%victim.onActivate(%player, %client, %pos, %vec);
		}
	}
}

function Player::kill(%player, %client)
{
	if (%damageType $= "")
	{
		%damageType = $DamageType::Suicide;
	}
	%player.Damage(%player, %player.getPosition(), 10000, %damageType);
}

function Player::mountVehicles(%this, %bool)
{
	%this.mountVehicle = %bool;
}

function Player::isPilot(%this)
{
	%vehicle = %this.getObjectMount();
	if (%vehicle)
	{
		if (%vehicle.getMountNodeObject(0) == %this)
		{
			return 1;
		}
	}
	return 0;
}

function Player::playDeathAnimation(%this)
{
	%this.setArmThread("root");
	%this.playThread(3, "Death1");
	return;
	if (%this.deathIdx++ > 11)
	{
		%this.deathIdx = 1;
	}
	%this.setActionThread("Death" @ %this.deathIdx);
}

function Player::playCelAnimation(%this, %anim)
{
	if (%this.getState() !$= "Dead")
	{
		%this.setActionThread("cel" @ %anim);
	}
}

function Player::playDeathCry(%obj)
{
	%client = %obj.client;
	%data = %obj.getDataBlock();
	if (%data.useCustomPainEffects)
	{
		if (isObject(%data.painSound))
		{
			%obj.playAudio(0, %data.deathSound);
		}
	}
	else
	{
		%obj.playAudio(0, DeathCrySound);
	}
}

function Player::playPain(%obj)
{
	%client = %obj.client;
	%data = %obj.getDataBlock();
	if (%data.useCustomPainEffects)
	{
		if (isObject(%data.painSound))
		{
			%obj.playAudio(0, %data.painSound);
		}
	}
	else
	{
		%obj.playAudio(0, PainCrySound);
	}
}

function fixArmReady(%obj)
{
	%leftimage = %obj.getMountedImage($LeftHandSlot);
	%rightImage = %obj.getMountedImage($RightHandSlot);
	%leftReady = 0;
	%rightReady = 0;
	if (%leftimage)
	{
		%leftReady = %leftimage.armReady;
	}
	if (%rightImage)
	{
		%rightReady = %rightImage.armReady;
	}
	if (%rightReady)
	{
		if (%leftReady)
		{
			%obj.playThread(1, armReadyBoth);
		}
		else
		{
			%obj.playThread(1, armReadyRight);
		}
	}
	else if (%leftReady)
	{
		%obj.playThread(1, armReadyLeft);
	}
	else
	{
		%obj.playThread(1, root);
	}
}

function Player::updateArm(%player, %newImage)
{
	if (%newImage.armReady)
	{
		if (%player.getMountedImage($LeftHandSlot))
		{
			if (%player.getMountedImage($LeftHandSlot).armReady)
			{
				%player.playThread(1, armReadyBoth);
			}
			else
			{
				%player.playThread(1, armReadyRight);
			}
		}
		else
		{
			%oldImage = %player.getMountedImage($RightHandSlot);
			if (%oldImage)
			{
				if (!%oldImage.armReady)
				{
					%player.playThread(1, armReadyRight);
				}
			}
			else
			{
				%player.playThread(1, armReadyRight);
			}
		}
	}
}

function Player::onDriverLeave(%obj, %player)
{
	%obj.getDataBlock().onDriverLeave(%obj, %player);
}

function PlayerData::onDriverLeave(%obj, %player)
{
}

function Player::GiveDefaultEquipment(%player)
{
	%player.tool[0] = hammerItem.getId();
	%player.tool[1] = wrenchItem.getId();
	%player.tool[2] = printGun.getId();
	%player.tool[3] = 0;
	%player.tool[4] = 0;
	%client = %player.client;
	if (isObject(%client))
	{
		messageClient(%client, 'MsgItemPickup', "", 0, hammerItem.getId(), 1);
		messageClient(%client, 'MsgItemPickup', "", 1, wrenchItem.getId(), 1);
		messageClient(%client, 'MsgItemPickup', "", 2, printGun.getId(), 1);
		messageClient(%client, 'MsgItemPickup', "", 3, 0, 1);
		messageClient(%client, 'MsgItemPickup', "", 4, 0, 1);
	}
}

function Player::SetTempColor(%player, %color, %time, %position, %projectileData)
{
	if (isEventPending(%player.tempColorSchedule))
	{
		cancel(%player.tempColorSchedule);
	}
	%playerZ = getWord(%player.getPosition(), 2);
	%projectileZ = getWord(%position, 2);
	%zDiff = %projectileZ - %playerZ;
	%client = %player.client;
	if (isObject(%client) && %position !$= "" && fileBase(%player.getDataBlock().shapeFile) $= "m")
	{
		if (%zDiff < 0.63)
		{
			%player.setNodeColor($LLeg[%client.lleg], %color);
			%player.setNodeColor($RLeg[%client.rleg], %color);
		}
		else if (%zDiff < 1.04)
		{
			%player.setNodeColor($Hip[%client.hip], %color);
			%player.setNodeColor($LHand[%client.lhand], %color);
			%player.setNodeColor($RHand[%client.rhand], %color);
		}
		else if (%zDiff < 1.72)
		{
			%player.setNodeColor($Chest[%client.chest], %color);
			%player.setNodeColor($LArm[%client.larm], %color);
			%player.setNodeColor($RArm[%client.rarm], %color);
			%player.setDecalName("AAA-None");
		}
		else if (%zDiff < 1.98)
		{
			if (%client.pack > 0)
			{
				%player.setNodeColor($pack[%client.pack], %color);
			}
			if (%client.secondPack > 0)
			{
				%player.setNodeColor($SecondPack[%client.secondPack], %color);
			}
		}
		else if (%zDiff < 2.35)
		{
			%player.setNodeColor("headskin", %color);
		}
		else
		{
			if (%client.hat > 0)
			{
				%player.setNodeColor($hat[%client.hat], %color);
			}
			if (%client.accent > 0)
			{
				%player.setNodeColor($Accent[%client.accent], %color);
			}
		}
	}
	else if (%position !$= "" && fileBase(%player.getDataBlock().shapeFile) $= "m")
	{
		if (%zDiff < 0.63)
		{
			for (%i = 0; %i < $num["LLeg"]; %i++)
			{
				%player.setNodeColor($LLeg[%i], %color);
			}
			for (%i = 0; %i < $num["RLeg"]; %i++)
			{
				%player.setNodeColor($RLeg[%i], %color);
			}
		}
		else if (%zDiff < 1.04)
		{
			for (%i = 0; %i < $num["Hip"]; %i++)
			{
				%player.setNodeColor($Hip[%i], %color);
			}
			for (%i = 0; %i < $num["LHand"]; %i++)
			{
				%player.setNodeColor($LHand[%i], %color);
			}
			for (%i = 0; %i < $num["RHand"]; %i++)
			{
				%player.setNodeColor($RHand[%i], %color);
			}
		}
		else if (%zDiff < 1.72)
		{
			for (%i = 0; %i < $num["Chest"]; %i++)
			{
				%player.setNodeColor($Chest[%i], %color);
			}
			for (%i = 0; %i < $num["Larm"]; %i++)
			{
				%player.setNodeColor($LArm[%i], %color);
			}
			for (%i = 0; %i < $num["Rarm"]; %i++)
			{
				%player.setNodeColor($RArm[%i], %color);
			}
			%player.setDecalName("AAA-None");
		}
		else if (%zDiff < 1.98)
		{
			for (%i = 1; %i < $num["Pack"]; %i++)
			{
				%player.setNodeColor($pack[%i], %color);
			}
			for (%i = 1; %i < $num["SecondPack"]; %i++)
			{
				%player.setNodeColor($SecondPack[%i], %color);
			}
		}
		else if (%zDiff < 2.35)
		{
			%player.setNodeColor("headskin", %color);
		}
		else
		{
			for (%i = 1; %i < $num["Hat"]; %i++)
			{
				%player.setNodeColor($hat[%i], %color);
			}
			for (%i = 1; %i < $num["Accent"]; %i++)
			{
				%player.setNodeColor($Accent[%i], %color);
			}
		}
	}
	else
	{
		%player.setNodeColor("ALL", %color);
		%player.setDecalName("AAA-None");
	}
	if (%time > 0)
	{
		if (isObject(%player.client))
		{
			%player.tempColorSchedule = %player.schedule(%time, ClearTempColor, %projectileData);
		}
		else if (isObject(%player.spawnBrick))
		{
			%player.tempColorSchedule = %player.spawnBrick.schedule(%time, colorVehicle);
		}
	}
}

function Player::ClearTempColor(%player, %projectileData)
{
	if (isEventPending(%player.tempColorSchedule))
	{
		cancel(%player.tempColorSchedule);
	}
	if (isObject(%projectileData))
	{
		%p = new Projectile()
		{
			dataBlock = %projectileData;
			initialPosition = %player.getHackPosition();
		};
		if (isObject(%p))
		{
			MissionCleanup.add(%p);
			%scale = VectorScale(%player.getScale(), 2);
			%p.setScale(%scale);
			%p.explode();
		}
	}
	if (isObject(%player.client))
	{
		%player.client.ApplyBodyColors();
	}
}

function Player::burn(%player, %time)
{
	if (isEventPending(%player.burnSchedule))
	{
		cancel(%player.burnSchedule);
	}
	if (%player.isEnabled())
	{
		%player.SetTempColor("0 0 0 1", %time);
	}
	else
	{
		%player.SetTempColor("0 0 0 1", -1);
	}
	%player.setDecalName("AAA-None");
	%player.mountImage(PlayerBurnImage, 3);
	if (%player.isEnabled())
	{
		%player.burnSchedule = %player.schedule(%time, clearBurn);
	}
}

function Player::BurnPlayer(%player, %time)
{
	%player.burn(%time * 1000);
}

function Player::clearBurn(%player)
{
	if (%player.getDamagePercent() >= 1)
	{
		return;
	}
	if (isEventPending(%player.burnSchedule))
	{
		%player.ClearTempColor();
		cancel(%player.burnSchedule);
	}
	%player.unmountImage(3);
	%pos = %player.getHackPosition();
	%pos = VectorAdd(%pos, VectorScale("0 0 -1", getWord(%player.getScale(), 2)));
	%p = new Projectile()
	{
		dataBlock = PlayerSootProjectile;
		initialPosition = %pos;
	};
	%p.setScale(%player.getScale());
	MissionCleanup.add(%p);
}

function Player::teleportEffect(%player)
{
	%p = new Projectile()
	{
		dataBlock = playerTeleportProjectile;
		initialPosition = %player.getHackPosition();
	};
	if (%p)
	{
		%p.setScale(%player.getScale());
		MissionCleanup.add(%p);
	}
	%player.emote(PlayerTeleportImage, 1);
}

datablock ParticleData(PlayerSootParticle)
{
	dragCoefficient = 5;
	windCoefficient = 0;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 1000;
	lifetimeVarianceMS = 500;
	spinSpeed = 10;
	spinRandomMin = -50;
	spinRandomMax = 50;
	useInvAlpha = 1;
	animateTexture = 0;
	textureName = "base/data/particles/cloud";
	colors[0] = "0.0 0.0 0.0 1.0";
	colors[1] = "0.0 0.0 0.0 0.0";
	sizes[0] = 1;
	sizes[1] = 1;
	times[0] = 0;
	times[1] = 1;
};
datablock ParticleEmitterData(playerSootEmitter)
{
	ejectionPeriodMS = 1;
	periodVarianceMS = 0;
	lifetimeMS = 7;
	ejectionVelocity = 5;
	velocityVariance = 5;
	ejectionOffset = 0;
	thetaMin = 0;
	thetaMax = 180;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = "playerSootParticle";
};
datablock ExplosionData(PlayerSootExplosion)
{
	lifetimeMS = 150;
	soundProfile = "";
	particleEmitter = playerSootEmitter;
	particleDensity = 230;
	particleRadius = 2;
	faceViewer = 1;
	explosionScale = "1 1 1";
	shakeCamera = 0;
	camShakeFreq = "7.0 8.0 7.0";
	camShakeAmp = "1.0 1.0 1.0";
	camShakeDuration = 0.5;
	camShakeRadius = 15;
	lightStartRadius = 0;
	lightEndRadius = 0;
	lightStartColor = "0.45 0.3 0.1";
	lightEndColor = "0 0 0";
	impulseRadius = 0;
	impulseForce = 0;
	radiusDamage = 0;
	damageRadius = 0;
};
datablock ProjectileData(PlayerSootProjectile)
{
	directDamage = 0;
	radiusDamage = 0;
	damageRadius = 0;
	Explosion = PlayerSootExplosion;
	explodeOnDeath = 1;
	armingDelay = 0;
	lifetime = 10;
	uiName = "Player Soot";
};
function Armor::onStuck(%this, %obj)
{
}

function Armor::onReachDestination(%this, %obj)
{
}

function Armor::onTargetEnterLOS(%this, %obj)
{
}

function Armor::onTargetExitLOS(%this, %obj)
{
}

datablock AudioProfile(deathExplosionSound)
{
	fileName = "~/data/sound/bodyRemove.wav";
	description = AudioClose3d;
	preload = 0;
};
datablock ParticleData(deathExplosionParticle1)
{
	dragCoefficient = 3;
	windCoefficient = 0;
	gravityCoefficient = -1.5;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 900;
	lifetimeVarianceMS = 800;
	spinSpeed = 10;
	spinRandomMin = -50;
	spinRandomMax = 50;
	useInvAlpha = 1;
	animateTexture = 0;
	textureName = "~/data/particles/cloud";
	colors[0] = "1 1 1 0.5";
	colors[1] = "1 1 1 0.5";
	colors[2] = "1 1 1 0.0";
	sizes[0] = 2;
	sizes[1] = 6;
	sizes[2] = 4;
	times[0] = 0;
	times[1] = 0.1;
	times[2] = 1;
};
datablock ParticleEmitterData(deathExplosionEmitter1)
{
	ejectionPeriodMS = 3;
	periodVarianceMS = 0;
	lifetimeMS = 21;
	ejectionVelocity = 8;
	velocityVariance = 1;
	ejectionOffset = 0;
	thetaMin = 0;
	thetaMax = 90;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = "deathExplosionParticle1";
};
datablock ParticleData(deathExplosionParticle2)
{
	dragCoefficient = 0.1;
	windCoefficient = 0;
	gravityCoefficient = 2;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 1000;
	lifetimeVarianceMS = 500;
	spinSpeed = 10;
	spinRandomMin = -50;
	spinRandomMax = 50;
	useInvAlpha = 0;
	animateTexture = 0;
	textureName = "~/data/particles/chunk";
	colors[0] = "1.0 1.0 0.0 1.0";
	colors[1] = "0.0 0.0 0.0 0.0";
	sizes[0] = 0.25;
	sizes[1] = 0.25;
	times[0] = 0;
	times[1] = 1;
};
datablock ParticleEmitterData(deathExplosionEmitter2)
{
	ejectionPeriodMS = 1;
	periodVarianceMS = 0;
	lifetimeMS = 7;
	ejectionVelocity = 15;
	velocityVariance = 5;
	ejectionOffset = 0;
	thetaMin = 0;
	thetaMax = 90;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = "deathExplosionParticle2";
};
datablock ExplosionData(deathExplosion)
{
	lifetimeMS = 150;
	soundProfile = deathExplosionSound;
	emitter[0] = deathExplosionEmitter1;
	emitter[1] = deathExplosionEmitter2;
	faceViewer = 1;
	explosionScale = "1 1 1";
	shakeCamera = 0;
	camShakeFreq = "7.0 8.0 7.0";
	camShakeAmp = "1.0 1.0 1.0";
	camShakeDuration = 0.5;
	camShakeRadius = 15;
	lightStartRadius = 4;
	lightEndRadius = 3;
	lightStartColor = "0.45 0.3 0.1";
	lightEndColor = "0 0 0";
};
datablock ProjectileData(deathProjectile)
{
	projectileShapeName = "~/data/shapes/empty.dts";
	directDamage = 0;
	radiusDamage = 0;
	damageRadius = 0;
	Explosion = deathExplosion;
	muzzleVelocity = 50;
	velInheritFactor = 1;
	explodeOnDeath = 1;
	armingDelay = 0;
	lifetime = 10;
	fadeDelay = 10;
	bounceElasticity = 0;
	bounceFriction = 0;
	isBallistic = 1;
	gravityMod = 0.5;
	hasLight = 0;
	lightRadius = 3;
	lightColor = "0 0 0.5";
	uiName = "Player Death";
};
datablock AudioProfile(spawnExplosionSound)
{
	fileName = "~/data/sound/spawn.wav";
	description = AudioClose3d;
	preload = 0;
};
datablock ParticleData(spawnExplosionParticle1)
{
	dragCoefficient = 0;
	windCoefficient = 0;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 400;
	lifetimeVarianceMS = 399;
	spinSpeed = 10;
	spinRandomMin = -50;
	spinRandomMax = 50;
	useInvAlpha = 1;
	animateTexture = 0;
	textureName = "~/data/particles/cloud";
	colors[0] = "1 0 1 0.5";
	colors[1] = "0 1 0 1 0.5";
	colors[2] = "1 0 1 0.0";
	sizes[0] = 0;
	sizes[1] = 4;
	sizes[2] = 4;
	times[0] = 0;
	times[1] = 0.1;
	times[2] = 1;
};
datablock ParticleEmitterData(spawnExplosionEmitter1)
{
	ejectionPeriodMS = 4;
	periodVarianceMS = 0;
	lifetimeMS = 21;
	ejectionVelocity = 5;
	velocityVariance = 1;
	ejectionOffset = 0;
	thetaMin = 0;
	thetaMax = 120;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = "spawnExplosionParticle1";
};
datablock ParticleData(spawnExplosionParticle2)
{
	dragCoefficient = 0.1;
	windCoefficient = 0;
	gravityCoefficient = 2;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 1000;
	lifetimeVarianceMS = 400;
	spinSpeed = 10;
	spinRandomMin = -50;
	spinRandomMax = 50;
	useInvAlpha = 0;
	animateTexture = 0;
	textureName = "~/data/particles/star1";
	colors[0] = "1.0 0.0 0.0 1.0";
	colors[1] = "0.0 0.0 0.0 0.0";
	sizes[0] = 0.25;
	sizes[1] = 0.25;
	times[0] = 0;
	times[1] = 1;
};
datablock ParticleEmitterData(spawnExplosionEmitter2)
{
	ejectionPeriodMS = 1;
	periodVarianceMS = 0;
	lifetimeMS = 7;
	ejectionVelocity = 0;
	velocityVariance = 0;
	ejectionOffset = 2;
	thetaMin = 0;
	thetaMax = 180;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = "spawnExplosionParticle2";
};
datablock ExplosionData(spawnExplosion)
{
	lifetimeMS = 150;
	soundProfile = spawnExplosionSound;
	emitter[0] = spawnExplosionEmitter1;
	emitter[1] = spawnExplosionEmitter2;
	faceViewer = 1;
	explosionScale = "1 1 1";
	shakeCamera = 0;
	camShakeFreq = "7.0 8.0 7.0";
	camShakeAmp = "1.0 1.0 1.0";
	camShakeDuration = 0.5;
	camShakeRadius = 15;
	hasLight = 0;
	lightStartRadius = 4;
	lightEndRadius = 3;
	lightStartColor = "0.45 0.3 0.1";
	lightEndColor = "0 0 0";
};
datablock ProjectileData(spawnProjectile)
{
	directDamage = 0;
	radiusDamage = 0;
	damageRadius = 0;
	Explosion = spawnExplosion;
	muzzleVelocity = 50;
	velInheritFactor = 1;
	explodeOnDeath = 1;
	armingDelay = 1000;
	lifetime = 10;
	fadeDelay = 10;
	bounceElasticity = 0;
	bounceFriction = 0;
	isBallistic = 1;
	gravityMod = 0.5;
	hasLight = 0;
	lightRadius = 3;
	lightColor = "0 0 0.5";
	uiName = "Player Spawn";
};
datablock AudioProfile(hammerHitSound)
{
	fileName = "~/data/sound/hammerHit.wav";
	description = AudioClosest3d;
	preload = 1;
};
datablock AudioProfile(glassExplosionSound)
{
	fileName = "~/data/sound/glassBreak.wav";
	description = AudioClosest3d;
	preload = 1;
};
datablock ParticleData(hammerSparkParticle)
{
	dragCoefficient = 4;
	gravityCoefficient = 1;
	inheritedVelFactor = 0.2;
	constantAcceleration = 0;
	lifetimeMS = 400;
	lifetimeVarianceMS = 300;
	textureName = "~/data/particles/star1";
	useInvAlpha = 0;
	spinSpeed = 150;
	spinRandomMin = -150;
	spinRandomMax = 150;
	colors[0] = "1.0 1.0 0.0 0.0";
	colors[1] = "1.0 1.0 0.0 0.5";
	colors[2] = "1.0 1.0 0.0 0.0";
	sizes[0] = 0.15;
	sizes[1] = 0.05;
	sizes[2] = 0;
	times[0] = 0.1;
	times[1] = 0.5;
	times[2] = 1;
	useInvAlpha = 0;
};
datablock ParticleEmitterData(hammerSparkEmitter)
{
	lifetimeMS = 50;
	ejectionPeriodMS = 3;
	periodVarianceMS = 0;
	ejectionVelocity = 5;
	velocityVariance = 3;
	ejectionOffset = 0.5;
	thetaMin = 0;
	thetaMax = 90;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = hammerSparkParticle;
};
datablock ParticleData(hammerExplosionParticle)
{
	dragCoefficient = 10;
	gravityCoefficient = -0.15;
	inheritedVelFactor = 0.2;
	constantAcceleration = 0;
	lifetimeMS = 500;
	lifetimeVarianceMS = 300;
	textureName = "~/data/particles/cloud";
	spinSpeed = 50;
	spinRandomMin = -50;
	spinRandomMax = 50;
	colors[0] = "1.0 1.0 1.0 0.25";
	colors[1] = "0.0 0.0 0.0 0.0";
	sizes[0] = 0.5;
	sizes[1] = 1;
	useInvAlpha = 0;
};
datablock ParticleEmitterData(hammerExplosionEmitter)
{
	lifetimeMS = 50;
	ejectionPeriodMS = 3;
	periodVarianceMS = 0;
	ejectionVelocity = 10;
	velocityVariance = 1;
	ejectionOffset = 0;
	thetaMin = 80;
	thetaMax = 85;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = hammerExplosionParticle;
};
datablock ExplosionData(hammerExplosion)
{
	lifetimeMS = 400;
	emitter[0] = hammerExplosionEmitter;
	emitter[1] = hammerSparkEmitter;
	faceViewer = 1;
	explosionScale = "1 1 1";
	shakeCamera = 1;
	camShakeFreq = "20.0 22.0 20.0";
	camShakeAmp = "0.50 0.50 0.50";
	camShakeDuration = 0.5;
	camShakeRadius = 10;
	lightStartRadius = 2;
	lightEndRadius = 1;
	lightStartColor = "0.6 0.6 0.0";
	lightEndColor = "0 0 0";
};
AddDamageType("HammerDirect", '<bitmap:base/client/ui/ci/hammer> %1', '%2 <bitmap:base/client/ui/ci/hammer> %1', 0, 1);
datablock ProjectileData(hammerProjectile)
{
	directDamage = 10;
	radiusDamage = 0;
	damageRadius = 0;
	Explosion = hammerExplosion;
	DirectDamageType = $DamageType::HammerDirect;
	RadiusDamageType = $DamageType::HammerDirect;
	muzzleVelocity = 50;
	velInheritFactor = 1;
	armingDelay = 0;
	lifetime = 0;
	fadeDelay = 70;
	bounceElasticity = 0;
	bounceFriction = 0;
	isBallistic = 0;
	gravityMod = 0;
	explodeOnDeath = 1;
	hasLight = 0;
	lightRadius = 3;
	lightColor = "0 0 0.5";
};
datablock ItemData(hammerItem)
{
	category = "Tools";
	className = "Weapon";
	shapeFile = "~/data/shapes/Hammer.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = 1;
	uiName = "Hammer ";
	iconName = "base/client/ui/itemIcons/Hammer";
	doColorShift = 1;
	colorShiftColor = "0.200 0.200 0.200 1.000";
	image = hammerImage;
	canDrop = 1;
};
datablock ShapeBaseImageData(hammerImage)
{
	shapeFile = "~/data/shapes/Hammer.dts";
	emap = 1;
	mountPoint = 0;
	offset = "0 0 0";
	eyeOffset = "0.7 1.2 -0.15";
	correctMuzzleVector = 0;
	className = "WeaponImage";
	Item = hammerItem;
	ammo = " ";
	Projectile = hammerProjectile;
	projectileType = Projectile;
	melee = 1;
	doRetraction = 0;
	armReady = 1;
	showBricks = 1;
	doColorShift = 1;
	colorShiftColor = "0.200 0.200 0.200 1.000";
	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0;
	stateTransitionOnTimeout[0] = "Ready";
	stateName[1] = "Ready";
	stateTransitionOnTriggerDown[1] = "PreFire";
	stateAllowImageChange[1] = 1;
	stateName[2] = "PreFire";
	stateScript[2] = "onPreFire";
	stateAllowImageChange[2] = 1;
	stateTimeoutValue[2] = 0.01;
	stateTransitionOnTimeout[2] = "Fire";
	stateName[3] = "Fire";
	stateTransitionOnTimeout[3] = "CheckFire";
	stateTimeoutValue[3] = 0.2;
	stateFire[3] = 1;
	stateAllowImageChange[3] = 1;
	stateSequence[3] = "Fire";
	stateScript[3] = "onFire";
	stateWaitForTimeout[3] = 1;
	stateSequence[3] = "Fire";
	stateName[4] = "CheckFire";
	stateTransitionOnTriggerUp[4] = "StopFire";
	stateTransitionOnTriggerDown[4] = "Fire";
	stateName[5] = "StopFire";
	stateTransitionOnTimeout[5] = "Ready";
	stateTimeoutValue[5] = 0.2;
	stateAllowImageChange[5] = 1;
	stateWaitForTimeout[5] = 1;
	stateSequence[5] = "StopFire";
	stateScript[5] = "onStopFire";
};
function hammerImage::onPreFire(%this, %obj, %slot)
{
	%obj.playThread(2, armattack);
}

function hammerImage::onStopFire(%this, %obj, %slot)
{
	%obj.playThread(2, root);
}

function hammerImage::onFire(%this, %player, %slot)
{
	%start = %player.getEyePoint();
	%muzzleVec = %player.getMuzzleVector(%slot);
	%muzzleVecZ = getWord(%muzzleVec, 2);
	if (%muzzleVecZ < -0.9)
	{
		%range = 5.5;
	}
	else
	{
		%range = 5;
	}
	%vec = VectorScale(%muzzleVec, %range * getWord(%player.getScale(), 2));
	%end = VectorAdd(%start, %vec);
	%mask = $TypeMasks::InteriorObjectType | $TypeMasks::FxBrickAlwaysObjectType | $TypeMasks::PlayerObjectType | $TypeMasks::VehicleObjectType | $TypeMasks::StaticShapeObjectType;
	if (%player.isMounted())
	{
		%raycast = containerRayCast(%start, %end, %mask, %player, %player.getObjectMount());
	}
	else
	{
		%raycast = containerRayCast(%start, %end, %mask, %player);
	}
	if (!%raycast)
	{
		%mask = $TypeMasks::TerrainObjectType;
		%raycast = containerRayCast(%start, %end, %mask);
	}
	if (!%raycast)
	{
		return;
	}
	%hitObj = getWord(%raycast, 0);
	%hitPos = getWords(%raycast, 1, 3);
	%hitNormal = getWords(%raycast, 4, 6);
	%projectilePos = VectorSub(%hitPos, VectorScale(%player.getEyeVector(), 0.25));
	%p = new Projectile()
	{
		dataBlock = hammerProjectile;
		initialVelocity = %hitNormal;
		initialPosition = %projectilePos;
		sourceObject = %player;
		sourceSlot = %slot;
		client = %player.client;
	};
	%p.setScale(%player.getScale());
	MissionCleanup.add(%p);
	%this.onHitObject(%player, %slot, %hitObj, %hitPos, %hitNormal);
}

function hammerImage::onHitObject(%this, %player, %slot, %hitObj, %hitPos, %hitNormal)
{
	%client = %player.client;
	ServerPlay3D(hammerHitSound, %hitPos);
	if (!isObject(%client))
	{
		return;
	}
	if (%hitObj.getType() & $TypeMasks::FxBrickAlwaysObjectType)
	{
		if (!isObject(%client))
		{
			return;
		}
		if (!%hitObj.willCauseChainKill())
		{
			if (getTrustLevel(%player, %hitObj) < $TrustLevel::Hammer)
			{
				if (%hitObj.stackBL_ID $= "" || %hitObj.stackBL_ID != %client.getBLID())
				{
					commandToClient(%client, 'CenterPrint', %hitObj.getGroup().name @ " does not trust you enough to do that.", 1);
					return;
				}
			}
			%hitObj.onToolBreak(%client);
			$CurrBrickKiller = %client;
			%hitObj.killBrick();
		}
	}
	else if (%hitObj.getClassName() $= "Player")
	{
		if (miniGameCanDamage(%client, %hitObj) == 1)
		{
			%hitObj.Damage(%player, %hitPos, hammerProjectile.directDamage, $DamageType::HammerDirect);
		}
	}
	else if (%hitObj.getClassName() $= "WheeledVehicle" || %hitObj.getClassName() $= "HoverVehicle" || %hitObj.getClassName() $= "FlyingVehicle")
	{
		%mount = %player;
		for (%i = 0; %i < 100; %i++)
		{
			if (%mount == %hitObj)
			{
				return;
			}
			if (!%mount.isMounted())
			{
				break;
			}
			%mount = %mount.getObjectMount();
		}
		%doFlip = 0;
		if (isObject(%hitObj.spawnBrick))
		{
			%vehicleOwner = findClientByBL_ID(%hitObj.spawnBrick.getGroup().bl_id);
		}
		else
		{
			%vehicleOwner = 0;
		}
		if (isObject(%vehicleOwner))
		{
			if (getTrustLevel(%player, %hitObj) >= $TrustLevel::VehicleTurnover)
			{
				%doFlip = 1;
			}
		}
		else
		{
			%doFlip = 1;
		}
		if (miniGameCanDamage(%player, %hitObj) == 1)
		{
			%doFlip = 1;
		}
		if (miniGameCanDamage(%player, %hitObj) == 0)
		{
			%doFlip = 0;
		}
		if (%doFlip)
		{
			%impulse = VectorNormalize(%vec);
			%impulse = VectorAdd(%impulse, "0 0 1");
			%impulse = VectorNormalize(%impulse);
			%force = %hitObj.getDataBlock().mass * 5;
			%impulse = VectorScale(%impulse, %force);
			%hitObj.applyImpulse(%hitPos, %impulse);
		}
	}
}

datablock AudioProfile(wrenchHitSound)
{
	fileName = "~/data/sound/wrenchHit.wav";
	description = AudioClose3d;
	preload = 1;
};
datablock AudioProfile(wrenchMissSound)
{
	fileName = "~/data/sound/wrenchMiss.wav";
	description = AudioClose3d;
	preload = 1;
};
datablock ParticleData(wrenchSparkParticle)
{
	dragCoefficient = 4;
	gravityCoefficient = 1;
	inheritedVelFactor = 0.2;
	constantAcceleration = 0;
	lifetimeMS = 400;
	lifetimeVarianceMS = 300;
	textureName = "~/data/particles/chunk";
	useInvAlpha = 0;
	spinSpeed = 150;
	spinRandomMin = -150;
	spinRandomMax = 150;
	colors[0] = "0.2 0.1 0.0 1.0";
	colors[1] = "0.0 0.0 0.0 0.5";
	colors[2] = "0.0 0.0 0.0 0.0";
	sizes[0] = 0.15;
	sizes[1] = 0.05;
	sizes[2] = 0;
	times[0] = 0.1;
	times[1] = 0.5;
	times[2] = 1;
	useInvAlpha = 1;
};
datablock ParticleEmitterData(wrenchSparkEmitter)
{
	lifetimeMS = 50;
	ejectionPeriodMS = 3;
	periodVarianceMS = 0;
	ejectionVelocity = 5;
	velocityVariance = 3;
	ejectionOffset = 0.5;
	thetaMin = 0;
	thetaMax = 90;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = wrenchSparkParticle;
};
datablock ParticleData(wrenchExplosionParticle)
{
	dragCoefficient = 2;
	gravityCoefficient = 0.5;
	inheritedVelFactor = 0.2;
	constantAcceleration = 0;
	lifetimeMS = 500;
	lifetimeVarianceMS = 300;
	textureName = "~/data/particles/nut";
	spinSpeed = 50;
	spinRandomMin = 300;
	spinRandomMax = 650;
	colors[0] = "1.0 1.0 0.0 1.0";
	colors[1] = "1.0 0.0 0.0 0.0";
	sizes[0] = 0.3;
	sizes[1] = 0.3;
	useInvAlpha = 0;
};
datablock ParticleEmitterData(wrenchExplosionEmitter)
{
	lifetimeMS = 50;
	ejectionPeriodMS = 3;
	periodVarianceMS = 0;
	ejectionVelocity = 2;
	velocityVariance = 1;
	ejectionOffset = 0.2;
	thetaMin = 0;
	thetaMax = 180;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = wrenchExplosionParticle;
};
datablock ExplosionData(wrenchExplosion)
{
	lifetimeMS = 400;
	emitter[0] = wrenchExplosionEmitter;
	emitter[1] = wrenchSparkEmitter;
	faceViewer = 1;
	explosionScale = "1 1 1";
	shakeCamera = 0;
	camShakeFreq = "20.0 22.0 20.0";
	camShakeAmp = "1.0 1.0 1.0";
	camShakeDuration = 0.5;
	camShakeRadius = 10;
	lightStartRadius = 2;
	lightEndRadius = 1;
	lightStartColor = "1.0 0.5 0.0";
	lightEndColor = "0 0 0";
};
datablock ProjectileData(wrenchProjectile)
{
	directDamage = 10;
	radiusDamage = 0;
	damageRadius = 0;
	Explosion = wrenchExplosion;
	muzzleVelocity = 50;
	velInheritFactor = 1;
	armingDelay = 0;
	lifetime = 0;
	fadeDelay = 70;
	bounceElasticity = 0;
	bounceFriction = 0;
	isBallistic = 0;
	gravityMod = 0;
	explodeOnDeath = 1;
	hasLight = 0;
	lightRadius = 3;
	lightColor = "0 0 0.5";
};
datablock ItemData(wrenchItem)
{
	category = "Tools";
	className = "Weapon";
	shapeFile = "~/data/shapes/wrench.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = 1;
	uiName = "wrench";
	iconName = "base/client/ui/itemIcons/wrench";
	doColorShift = 1;
	colorShiftColor = "0.471 0.471 0.471 1.000";
	image = wrenchImage;
	canDrop = 1;
};
datablock ShapeBaseImageData(wrenchImage)
{
	shapeFile = "~/data/shapes/wrench.dts";
	emap = 1;
	mountPoint = 0;
	offset = "0 0 0";
	eyeOffset = "0.7 1.2 -0.15";
	correctMuzzleVector = 0;
	className = "WeaponImage";
	Item = wrenchItem;
	ammo = " ";
	Projectile = wrenchProjectile;
	projectileType = Projectile;
	melee = 1;
	doRetraction = 0;
	armReady = 1;
	showBricks = 1;
	doColorShift = 1;
	colorShiftColor = "0.471 0.471 0.471 1.000";
	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0;
	stateTransitionOnTimeout[0] = "Ready";
	stateName[1] = "Ready";
	stateTransitionOnTriggerDown[1] = "PreFire";
	stateAllowImageChange[1] = 1;
	stateName[2] = "PreFire";
	stateScript[2] = "onPreFire";
	stateAllowImageChange[2] = 1;
	stateTimeoutValue[2] = 0.01;
	stateTransitionOnTimeout[2] = "Fire";
	stateName[3] = "Fire";
	stateTransitionOnTimeout[3] = "CheckFire";
	stateTimeoutValue[3] = 0.5;
	stateFire[3] = 1;
	stateAllowImageChange[3] = 1;
	stateSequence[3] = "Fire";
	stateScript[3] = "onFire";
	stateWaitForTimeout[3] = 1;
	stateSequence[3] = "Fire";
	stateName[4] = "CheckFire";
	stateTransitionOnTriggerUp[4] = "StopFire";
	stateName[5] = "StopFire";
	stateTransitionOnTimeout[5] = "Ready";
	stateTimeoutValue[5] = 0.01;
	stateAllowImageChange[5] = 1;
	stateWaitForTimeout[5] = 1;
	stateSequence[5] = "StopFire";
	stateScript[5] = "onStopFire";
};
function wrenchImage::onPreFire(%this, %obj, %slot)
{
	%obj.playThread(2, wrench);
}

function wrenchImage::onStopFire(%this, %obj, %slot)
{
	%obj.playThread(2, root);
}

function wrenchImage::onFire(%this, %player, %slot)
{
	%start = %player.getEyePoint();
	%vec = VectorScale(%player.getMuzzleVector(%slot), 10 * getWord(%player.getScale(), 2));
	%end = VectorAdd(%start, %vec);
	%mask = $TypeMasks::InteriorObjectType | $TypeMasks::TerrainObjectType | $TypeMasks::FxBrickAlwaysObjectType;
	%raycast = containerRayCast(%start, %end, %mask);
	if (!%raycast)
	{
		return;
	}
	%hitObj = getWord(%raycast, 0);
	%hitPos = getWords(%raycast, 1, 3);
	%hitNormal = getWords(%raycast, 4, 6);
	%projectilePos = VectorSub(%hitPos, VectorScale(%player.getEyeVector(), 0.25));
	%p = new Projectile()
	{
		dataBlock = wrenchProjectile;
		initialVelocity = "0 0 0";
		initialPosition = %projectilePos;
		sourceObject = %player;
		sourceSlot = %slot;
		client = %player.client;
	};
	%p.setScale(%player.getScale());
	MissionCleanup.add(%p);
	%this.onHitObject(%player, %slot, %hitObj, %hitPos, %hitNormal);
}

function wrenchImage::onHitObject(%this, %player, %slot, %hitObj, %hitPos, %hitNormal)
{
	%client = %player.client;
	if (%hitObj.getType() & $TypeMasks::FxBrickAlwaysObjectType)
	{
		%adminOverride = 0;
		if (getTrustLevel(%player, %hitObj) < $TrustLevel::Wrench)
		{
			if (%client.isAdmin)
			{
				%adminOverride = 1;
			}
			else
			{
				commandToClient(%client, 'CenterPrint', %hitObj.getGroup().name @ " does not trust you enough to do that.", 1);
				ServerPlay3D(wrenchMissSound, %hitPos);
				return;
			}
		}
		if (!$Server::LAN)
		{
			if (isObject(%client.miniGame))
			{
				if (isObject(%brickGroup.client))
				{
					if (isObject(%brickGroup.client.miniGame))
					{
						if (%brickGroup.client.miniGame != %client.miniGame)
						{
							commandToClient(%client, 'CenterPrint', %brickGroup.name @ " is not in the minigame.", 1);
							ServerPlay3D(wrenchMissSound, %hitPos);
							return;
						}
					}
				}
			}
			else if (isObject(%brickGroup.client))
			{
				if (isObject(%brickGroup.client.miniGame))
				{
					if (%brickGroup.client.miniGame.useAllPlayersBricks || %brickGroup.client.miniGame.owner == %brickGroup.client)
					{
						commandToClient(%client, 'CenterPrint', %brickGroup.name @ "'s bricks are in a minigame right now.", 1);
						ServerPlay3D(wrenchMissSound, %hitPos);
						return;
					}
				}
			}
		}
		if (%adminOverride)
		{
			%client.wrenchBrick = "";
			%client.adminWrenchBrick = %hitObj;
		}
		else
		{
			%client.wrenchBrick = %hitObj;
			%client.adminWrenchBrick = "";
		}
		if (%client.brickGroup == %hitObj.getGroup())
		{
			%allowNamedTargets = 1;
		}
		else
		{
			%allowNamedTargets = 0;
		}
		if (isObject(%hitObj.client))
		{
			%lanHeading = %hitObj.client.getPlayerName();
		}
		%netHeading = %hitObj.getGroup().name @ " - (BL_ID: " @ %hitObj.getGroup().bl_id @ ")";
		if (%hitObj.getDataBlock().specialBrickType $= "Sound")
		{
			if ($Server::LAN)
			{
				commandToClient(%client, 'openWrenchSoundDlg', %lanHeading, %allowNamedTargets, %adminOverride, $Pref::Server::WrenchEventsAdminOnly);
			}
			else
			{
				commandToClient(%client, 'openWrenchSoundDlg', %netHeading, %allowNamedTargets, %adminOverride, $Pref::Server::WrenchEventsAdminOnly);
			}
			%hitObj.sendWrenchSoundData(%client);
		}
		else if (%hitObj.getDataBlock().specialBrickType $= "VehicleSpawn")
		{
			if ($Server::LAN)
			{
				commandToClient(%client, 'openWrenchVehicleSpawnDlg', %lanHeading, %allowNamedTargets, %adminOverride, $Pref::Server::WrenchEventsAdminOnly);
			}
			else
			{
				commandToClient(%client, 'openWrenchVehicleSpawnDlg', %netHeading, %allowNamedTargets, %adminOverride, $Pref::Server::WrenchEventsAdminOnly);
			}
			%hitObj.sendWrenchVehicleSpawnData(%client);
		}
		else
		{
			if ($Server::LAN)
			{
				commandToClient(%client, 'openWrenchDlg', %lanHeading, %allowNamedTargets, %adminOverride, $Pref::Server::WrenchEventsAdminOnly);
			}
			else
			{
				commandToClient(%client, 'openWrenchDlg', %netHeading, %allowNamedTargets, %adminOverride, $Pref::Server::WrenchEventsAdminOnly);
			}
			%hitObj.sendWrenchData(%client);
		}
		ServerPlay3D(wrenchHitSound, %hitPos);
		return;
	}
	else
	{
		ServerPlay3D(wrenchMissSound, %hitPos);
	}
}

function serverCmdVehicleSpawn_Respawn(%client, %data)
{
	%brick = %client.wrenchBrick;
	if (!isObject(%brick))
	{
		messageClient(%client, 'Wrench Error - VehicleSpawn_Respawn: Brick no longer exists!');
		return;
	}
	if (isObject(%data) && %brick.vehicleDataBlock != %data)
	{
		%brick.setVehicle(%data, %client);
		return;
	}
	%quotaObject = getQuotaObjectFromClient(%client);
	if (!isObject(%quotaObject))
	{
		error("Error: serverCmdVehicleSpawn_Respawn() - new quota object creation failed!");
	}
	setCurrentQuotaObject(%quotaObject);
	%brick.spawnVehicle();
}

function serverCmdSetWrenchData(%client, %data)
{
	%brick = %client.wrenchBrick;
	if (!isObject(%brick))
	{
		messageClient(%client, 'Wrench Error - SetWrenchData: Brick no longer exists!');
		return;
	}
	if (%brick.isDead())
	{
		return;
	}
	%quotaObject = getQuotaObjectFromClient(%client);
	if (!isObject(%quotaObject))
	{
		error("Error: serverCmdSetWrenchData() - new quota object creation failed!");
	}
	setCurrentQuotaObject(%quotaObject);
	%fieldCount = getFieldCount(%data);
	for (%i = 0; %i < %fieldCount; %i++)
	{
		%field = getField(%data, %i);
		%type = getWord(%field, 0);
		if (%type $= "N")
		{
			%name = trim(getSubStr(%field, 2, strlen(%field) - 2));
			%brick.setNTObjectName(%name);
		}
		else if (%type $= "LDB")
		{
			%db = getWord(%field, 1);
			%brick.setLight(%db, %client);
		}
		else if (%type $= "EDB")
		{
			%db = getWord(%field, 1);
			%brick.setEmitter(%db, %client);
		}
		else if (%type $= "EDIR")
		{
			%dir = getWord(%field, 1);
			%brick.setEmitterDirection(%dir);
		}
		else if (%type $= "IDB")
		{
			%db = getWord(%field, 1);
			%brick.setItem(%db, %client);
		}
		else if (%type $= "IPOS")
		{
			%pos = getWord(%field, 1);
			%brick.setItemPosition(%pos);
		}
		else if (%type $= "IDIR")
		{
			%dir = getWord(%field, 1);
			%brick.setItemDirection(%dir);
		}
		else if (%type $= "IRT")
		{
			%time = mFloor(getWord(%field, 1)) * 1000;
			%brick.setItemRespawntime(%time);
		}
		else if (%type $= "SDB")
		{
			%db = getWord(%field, 1);
			%brick.setSound(%db, %client);
		}
		else if (%type $= "VDB")
		{
			%db = getWord(%field, 1);
			%brick.setVehicle(%db, %client);
		}
		else if (%type $= "RCV")
		{
			%val = getWord(%field, 1);
			%brick.setReColorVehicle(%val);
		}
		else if (%type $= "RC")
		{
			if (getTrustLevel(%client, %brick) < $TrustLevel::WrenchRaycasting)
			{
				continue;
			}
			%val = getWord(%field, 1);
			%brick.setRayCasting(%val);
		}
		else if (%type $= "C")
		{
			if (getTrustLevel(%client, %brick) < $TrustLevel::WrenchCollision)
			{
				continue;
			}
			%val = getWord(%field, 1);
			%brick.setColliding(%val);
		}
		else if (%type $= "R")
		{
			if (getTrustLevel(%client, %brick) < $TrustLevel::WrenchRendering)
			{
				continue;
			}
			%val = getWord(%field, 1);
			if (isObject(%brick.emitter))
			{
				if (isObject(%brick.emitter.emitter))
				{
					%edb = %brick.emitter.emitter;
				}
			}
			%brick.setRendering(%val);
			if (isObject(%edb))
			{
				%brick.setEmitter(%edb, %client);
			}
		}
		else
		{
			error("ERROR: clientCmdSetWrenchData() - unknown field type \"" @ %field @ "\"");
		}
	}
}

function fxDTSBrick::setLight(%obj, %data, %client)
{
	if (isObject(%data))
	{
		if (isObject(%obj.light))
		{
			if (%obj.light.getDataBlock().getId() == %data.getId())
			{
				return;
			}
			else
			{
				%obj.light.delete();
				%obj.light = 0;
			}
		}
	}
	else
	{
		if (isObject(%obj.light))
		{
			%obj.light.delete();
			%obj.light = 0;
		}
		return;
	}
	if (%obj.isDead())
	{
		return;
	}
	if (%data.getClassName() !$= "fxLightData")
	{
		return;
	}
	if (%data.uiName $= "")
	{
		return;
	}
	%light = new fxLight()
	{
		dataBlock = %data;
	};
	if (!%light)
	{
		if ($Server::LAN)
		{
			commandToClient(%client, 'CenterPrint', "\c6You can't have more than " @ $Pref::Server::QuotaLAN::Environment @ " lights/emitters!", 3);
		}
		else
		{
			commandToClient(%client, 'CenterPrint', "\c6You can't have more than " @ $Pref::Server::Quota::Environment @ " lights/emitters!", 3);
		}
		return;
	}
	MissionCleanup.add(%light);
	%light.brick = %obj;
	%obj.light = %light;
	%light.setTransform(%obj.getTransform());
	%light.attachToBrick(%obj);
	%light.setEnable(1);
}

function fxDTSBrick::setEmitter(%obj, %data, %client)
{
	if (isObject(%obj.emitter))
	{
		%obj.emitter.delete();
	}
	%obj.emitter = 0;
	if (%obj.isDead())
	{
		return;
	}
	if (!isObject(%client))
	{
		%client = %obj.client;
	}
	if (!isObject(%client))
	{
		%client = %obj.getGroup().client;
	}
	if (!isObject(%client))
	{
		%client = 0;
	}
	if (!isObject(%data))
	{
		return;
	}
	if (%data.getClassName() !$= "ParticleEmitterData")
	{
		return;
	}
	if (%data.uiName $= "")
	{
		return;
	}
	%brickData = %obj.getDataBlock();
	if (%brickData.brickSizeX <= 1 && %brickData.brickSizeY <= 1 && %brickData.brickSizeZ <= 3)
	{
		%nodeData = %data.pointEmitterNode;
	}
	else
	{
		%nodeData = %data.emitterNode;
	}
	if (!isObject(%nodeData))
	{
		%nodeData = GenericEmitterNode;
	}
	%emitter = new ParticleEmitterNode()
	{
		dataBlock = %nodeData;
		emitter = %data;
	};
	if (!%emitter)
	{
		if ($Server::LAN)
		{
			commandToClient(%client, 'CenterPrint', "\c6You can't have more than " @ $Pref::Server::QuotaLAN::Environment @ " lights/emitters!", 3);
		}
		else
		{
			commandToClient(%client, 'CenterPrint', "\c6You can't have more than " @ $Pref::Server::Quota::Environment @ " lights/emitters!", 3);
		}
		return;
	}
	%emitter.setEmitterDataBlock(%data);
	%emitter.setColor(getColorIDTable(%obj.colorID));
	MissionCleanup.add(%emitter);
	if (%obj.isFakeDead())
	{
		%obj.oldEmitterDB = %data;
		%emitter.setEmitterDataBlock(0);
	}
	%emitter.brick = %obj;
	%emitter.setTransform(%obj.getTransform());
	%obj.emitter = %emitter;
	%obj.setEmitterDirection(%obj.emitterDirection);
}

function fxDTSBrick::setEmitterDirection(%obj, %dir)
{
	if (%dir < 0 || %dir > 5)
	{
		return;
	}
	%obj.emitterDirection = %dir;
	if (%obj.getDataBlock().brickSizeX == 1 && %obj.getDataBlock().brickSizeY == 1 && %obj.getDataBlock().brickSizeZ == 1)
	{
		%scaleX = %scaleY = %scaleZ = 0;
	}
	else
	{
		%wbox = %obj.getWorldBox();
		%scaleX = mAbs(getWord(%wbox, 0) - getWord(%wbox, 3));
		%scaleY = mAbs(getWord(%wbox, 1) - getWord(%wbox, 4));
		%scaleZ = mAbs(getWord(%wbox, 2) - getWord(%wbox, 5));
	}
	if (%scaleX < 0.55)
	{
		%scaleX = 0;
	}
	if (%scaleY < 0.55)
	{
		%scaleY = 0;
	}
	if (%scaleZ < 0.66)
	{
		%scaleZ = 0;
	}
	if (isObject(%obj.emitter))
	{
		%pos = getWords(%obj.getTransform(), 0, 2);
		if (%dir == 0)
		{
			%rot = "0 0 1 0";
		}
		else if (%dir == 1)
		{
			%rot = "0 1 0 " @ $pi;
		}
		else if (%dir == 2)
		{
			%rot = "1 0 0 " @ $piOver2;
			%temp = %scaleY;
			%scaleY = %scaleZ;
			%scaleZ = %temp;
		}
		else if (%dir == 3)
		{
			%rot = "0 -1 0 " @ $piOver2;
			%temp = %scaleX;
			%scaleX = %scaleZ;
			%scaleZ = %temp;
		}
		else if (%dir == 4)
		{
			%rot = "-1 0 0 " @ $piOver2;
			%temp = %scaleY;
			%scaleY = %scaleZ;
			%scaleZ = %temp;
		}
		else if (%dir == 5)
		{
			%rot = "0 1 0 " @ $piOver2;
			%temp = %scaleX;
			%scaleX = %scaleZ;
			%scaleZ = %temp;
		}
		else
		{
			%rot = "0 0 1 0";
		}
		%obj.emitter.setScale(%scaleX SPC %scaleY SPC %scaleZ);
		%obj.emitter.setTransform(%pos SPC %rot);
	}
}

function fxDTSBrick::setItem(%obj, %data, %client)
{
	if (isObject(%obj.Item))
	{
		%obj.Item.delete();
	}
	%obj.Item = 0;
	if (%obj.isDead())
	{
		return;
	}
	if (!isObject(%data))
	{
		return;
	}
	if (%data.getClassName() !$= "ItemData")
	{
		return;
	}
	if (%data.uiName $= "")
	{
		return;
	}
	%item = new Item()
	{
		dataBlock = %data;
		static = 1;
	};
	if (!isObject(%item))
	{
		if ($Server::LAN)
		{
			commandToClient(%client, 'CenterPrint', "\c6You can't have more than " @ $Pref::Server::QuotaLAN::Item @ " items!", 3);
		}
		else
		{
			commandToClient(%client, 'CenterPrint', "\c6You can't have more than " @ $Pref::Server::Quota::Item @ " items!", 3);
		}
		return;
	}
	MissionCleanup.add(%item);
	%obj.Item = %item;
	%item.spawnBrick = %obj;
	if (%obj.itemRespawnTime < $Game::Item::MinRespawnTime)
	{
		%obj.itemRespawnTime = $Game::Item::MinRespawnTime;
	}
	else if (%obj.itemRespawnTime > $Game::Item::MaxRespawnTime)
	{
		%obj.itemRespawnTime = $Game::Item::MaxRespawnTime;
	}
	%obj.Item.setRespawnTime(%obj.itemRespawnTime);
	%obj.itemDirection = mFloor(%obj.itemDirection);
	%obj.setItemDirection(%obj.itemDirection);
}

function fxDTSBrick::setItemDirection(%obj, %dir)
{
	%dir = mClamp(%dir, 2, 5);
	%obj.itemDirection = %dir;
	if (!isObject(%obj.Item))
	{
		return;
	}
	%pos = getWords(%obj.Item.getTransform(), 0, 2);
	if (%dir == 2)
	{
		%rot = "0 0 1 0";
	}
	else if (%dir == 3)
	{
		%rot = "0 0 1 " @ $piOver2;
	}
	else if (%dir == 4)
	{
		%rot = "0 0 -1 " @ $pi;
	}
	else if (%dir == 5)
	{
		%rot = "0 0 -1 " @ $piOver2;
	}
	else
	{
		%rot = "0 0 1 0";
	}
	%obj.Item.setTransform(%pos SPC %rot);
	%obj.setItemPosition(%obj.itemPosition);
}

function fxDTSBrick::setItemPosition(%obj, %dir)
{
	if (%dir < 0 || %dir > 5)
	{
		return;
	}
	%obj.itemPosition = %dir;
	if (!isObject(%obj.Item))
	{
		return;
	}
	%itemBox = %obj.Item.getWorldBox();
	%itemBoxX = mAbs(getWord(%itemBox, 0) - getWord(%itemBox, 3)) / 2;
	%itemBoxY = mAbs(getWord(%itemBox, 1) - getWord(%itemBox, 4)) / 2;
	%itemBoxZ = mAbs(getWord(%itemBox, 2) - getWord(%itemBox, 5)) / 2;
	%itemBoxCenter = %obj.Item.getWorldBoxCenter();
	%itemCenter = %obj.Item.getPosition();
	%itemOffset = VectorSub(%itemCenter, %itemBoxCenter);
	%brickBox = %obj.getWorldBox();
	%brickBoxX = mAbs(getWord(%brickBox, 0) - getWord(%brickBox, 3)) / 2;
	%brickBoxY = mAbs(getWord(%brickBox, 1) - getWord(%brickBox, 4)) / 2;
	%brickBoxZ = mAbs(getWord(%brickBox, 2) - getWord(%brickBox, 5)) / 2;
	%pos = %obj.getPosition();
	%pos = VectorAdd(%pos, %itemOffset);
	%posX = getWord(%pos, 0);
	%posY = getWord(%pos, 1);
	%posZ = getWord(%pos, 2);
	%rot = getWords(%obj.Item.getTransform(), 3, 6);
	if (%dir == 0)
	{
		%posZ += %itemBoxZ + %brickBoxZ;
	}
	else if (%dir == 1)
	{
		%posZ -= %itemBoxZ + %brickBoxZ;
	}
	else if (%dir == 2)
	{
		%posY += %itemBoxY + %brickBoxY;
	}
	else if (%dir == 3)
	{
		%posX += %itemBoxX + %brickBoxX;
	}
	else if (%dir == 4)
	{
		%posY -= %itemBoxY + %brickBoxY;
	}
	else if (%dir == 5)
	{
		%posX -= %itemBoxX + %brickBoxX;
	}
	%obj.Item.setTransform(%posX SPC %posY SPC %posZ SPC %rot);
}

function fxDTSBrick::setItemRespawntime(%obj, %time)
{
	if (%time < $Game::Item::MinRespawnTime)
	{
		%time = $Game::Item::MinRespawnTime;
	}
	else if (%time > $Game::Item::MaxRespawnTime)
	{
		%time = $Game::Item::MaxRespawnTime;
	}
	%obj.itemRespawnTime = %time;
	if (isObject(%obj.Item))
	{
		%obj.Item.setRespawnTime(%time);
	}
}

function fxDTSBrick::setMusic(%obj, %data, %client)
{
	%obj.setSound(%data, %client);
}

function fxDTSBrick::setSound(%obj, %data, %client)
{
	if (isObject(%obj.AudioEmitter))
	{
		%obj.AudioEmitter.delete();
	}
	%obj.AudioEmitter = 0;
	if (%obj.getDataBlock().specialBrickType !$= "Sound")
	{
		return;
	}
	if (!isObject(%data))
	{
		return;
	}
	if (%data.getClassName() !$= "AudioProfile")
	{
		return;
	}
	if (%data.uiName $= "")
	{
		return;
	}
	if (!%data.getDescription().isLooping)
	{
		return;
	}
	%audioEmitter = new AudioEmitter()
	{
		profile = %data;
		useProfileDescription = 1;
	};
	MissionCleanup.add(%audioEmitter);
	%obj.AudioEmitter = %audioEmitter;
	%audioEmitter.setTransform(%obj.getTransform());
}

function fxDTSBrick::setVehicle(%obj, %data, %client)
{
	if (%obj.vehicleDataBlock == %data)
	{
		return;
	}
	if (isObject(%obj.Vehicle))
	{
		%obj.Vehicle.delete();
	}
	%obj.Vehicle = 0;
	if (isObject(%obj.VehicleSpawnMarker))
	{
		%obj.VehicleSpawnMarker.delete();
	}
	%obj.VehicleSpawnMarker = 0;
	if (%obj.getDataBlock().specialBrickType !$= "VehicleSpawn")
	{
		return;
	}
	if (!isObject(%data))
	{
		%obj.vehicleDataBlock = 0;
		return;
	}
	if (%data.getClassName() !$= "PlayerData" && %data.getClassName() !$= "WheeledVehicleData" && %data.getClassName() !$= "FlyingVehicleData" && %data.getClassName() !$= "HoverVehicleData")
	{
		return;
	}
	if (!%data.rideable)
	{
		return;
	}
	if (%data.uiName $= "")
	{
		return;
	}
	if (%data.getClassName() $= "PlayerData")
	{
		%brickGroup = %obj.getGroup();
		if (!$Server::LAN)
		{
			if (%brickGroup.numPlayerVehicles >= $Pref::Server::Quota::Player)
			{
				if (%client.brickGroup == %brickGroup)
				{
					if ($Pref::Server::Quota::Player == 1)
					{
						commandToClient(%client, 'CenterPrint', "\c0You already have a player-vehicle", 2);
					}
					else
					{
						commandToClient(%client, 'CenterPrint', "\c0You already have " @ $Pref::Server::Quota::Player @ " player-vehicles", 2);
					}
				}
				else if ($Pref::Server::Quota::Player == 1)
				{
					commandToClient(%client, 'CenterPrint', "\c0" @ %brickGroup.name @ " already has a player-vehicle", 2);
				}
				else
				{
					commandToClient(%client, 'CenterPrint', "\c0" @ %brickGroup.name @ " already has " @ $Pref::Server::Quota::Player @ " player-vehicles", 2);
				}
				return;
			}
		}
		if ($Server::numPlayerVehicles >= $Pref::Server::MaxPlayerVehicles_Total)
		{
			if ($Pref::Server::MaxPlayerVehicles_Total == 1)
			{
				commandToClient(%client, 'CenterPrint', "\c0Server is limited to 1 player-vehicle", 2);
			}
			else
			{
				commandToClient(%client, 'CenterPrint', "\c0Server is limited to " @ $Pref::Server::MaxPlayerVehicles_Total @ " player-vehicles", 2);
			}
			return;
		}
	}
	else
	{
		%brickGroup = %obj.getGroup();
		if (!$Server::LAN)
		{
			if (%brickGroup.numPhysVehicles >= $Pref::Server::Quota::Vehicle)
			{
				if (%client.brickGroup == %brickGroup)
				{
					if ($Pref::Server::Quota::Vehicle == 1)
					{
						commandToClient(%client, 'CenterPrint', "\c0You already have a physics-vehicle", 2);
					}
					else
					{
						commandToClient(%client, 'CenterPrint', "\c0You already have " @ $Pref::Server::Quota::Vehicle @ " physics-vehicles", 2);
					}
				}
				else if ($Pref::Server::Quota::Vehicle == 1)
				{
					commandToClient(%client, 'CenterPrint', "\c0" @ %brickGroup.name @ " already has a physics-vehicle", 2);
				}
				else
				{
					commandToClient(%client, 'CenterPrint', "\c0" @ %brickGroup.name @ " already has " @ $Pref::Server::Quota::Vehicle @ " physics-vehicles", 2);
				}
				return;
			}
		}
		$Pref::Server::MaxPhysVehicles_Total = mClamp($Pref::Server::MaxPhysVehicles_Total, 0, $Game::Max::MaxPhysicsVehicles_Total);
		if ($Server::numPhysVehicles >= $Pref::Server::MaxPhysVehicles_Total)
		{
			if ($Pref::Server::MaxPhysVehicles_Total == 1)
			{
				commandToClient(%client, 'CenterPrint', "\c0Server is limited to 1 physics-vehicle", 2);
			}
			else
			{
				commandToClient(%client, 'CenterPrint', "\c0Server is limited to " @ $Pref::Server::MaxPhysVehicles_Total @ " physics-vehicles", 2);
			}
			return;
		}
	}
	%obj.vehicleDataBlock = %data;
	%obj.spawnVehicle();
	%obj.VehicleSpawnMarker = new VehicleSpawnMarker()
	{
		dataBlock = VehicleSpawnMarkerData;
		uiName = %data.uiName;
		reColorVehicle = %obj.reColorVehicle;
		vehicleDataBlock = %data;
		brick = %obj;
	};
	MissionCleanup.add(%obj.VehicleSpawnMarker);
	%obj.VehicleSpawnMarker.setTransform(%obj.getTransform());
}

function fxDTSBrick::setReColorVehicle(%obj, %val)
{
	%obj.reColorVehicle = %val;
	if (%val)
	{
		%obj.colorVehicle();
	}
	else
	{
		%obj.unColorVehicle();
	}
	if (isObject(%obj.VehicleSpawnMarker))
	{
		%obj.VehicleSpawnMarker.setData(%obj.VehicleSpawnMarker.getUiName(), %obj.reColorVehicle);
	}
}

function fxDTSBrick::sendWrenchVehicleSpawnData(%obj, %client)
{
	%data = "";
	%name = %obj.getName();
	if (%name !$= "")
	{
		%data = %data TAB "N" SPC %name;
	}
	else
	{
		%data = %data TAB "N" SPC " ";
	}
	if (isObject(%obj.vehicleDataBlock))
	{
		%db = %obj.vehicleDataBlock.getId();
	}
	else
	{
		%db = 0;
	}
	%data = %data TAB "VDB" SPC %db;
	if (%obj.reColorVehicle)
	{
		%val = 1;
	}
	else
	{
		%val = 0;
	}
	%data = %data TAB "RCV" SPC %val;
	%data = %data TAB "RC" SPC %obj.isRayCasting();
	%data = %data TAB "C" SPC %obj.isColliding();
	%data = %data TAB "R" SPC %obj.isRendering();
	%data = trim(%data);
	commandToClient(%client, 'SetWrenchData', %data);
	commandToClient(%client, 'WrenchLoadingDone');
}

function fxDTSBrick::sendWrenchSoundData(%obj, %client)
{
	%data = "";
	%name = %obj.getName();
	if (%name !$= "")
	{
		%data = %data TAB "N" SPC %name;
	}
	else
	{
		%data = %data TAB "N" SPC " ";
	}
	if (isObject(%obj.AudioEmitter))
	{
		%emitterDB = %obj.AudioEmitter.profile.getId();
	}
	else
	{
		%emitterDB = 0;
	}
	%data = %data TAB "SDB" SPC %emitterDB;
	%data = trim(%data);
	commandToClient(%client, 'SetWrenchData', %data);
	commandToClient(%client, 'WrenchLoadingDone');
}

function fxDTSBrick::sendWrenchData(%obj, %client)
{
	%data = "";
	%name = %obj.getName();
	if (%name !$= "")
	{
		%data = %data TAB "N" SPC %name;
	}
	else
	{
		%data = %data TAB "N" SPC " ";
	}
	if (isObject(%obj.light))
	{
		%lightDB = %obj.light.getDataBlock();
	}
	else
	{
		%lightDB = 0;
	}
	%data = %data TAB "LDB" SPC %lightDB;
	if (isObject(%obj.emitter))
	{
		if (isObject(%obj.emitter.emitter))
		{
			%emitterDB = %obj.emitter.emitter.getId();
		}
		else
		{
			%emitterDB = 0;
		}
	}
	else
	{
		%emitterDB = 0;
	}
	%data = %data TAB "EDB" SPC %emitterDB;
	%data = %data TAB "EDIR" SPC mFloor(%obj.emitterDirection);
	if (isObject(%obj.Item))
	{
		%itemDB = %obj.Item.getDataBlock();
	}
	else
	{
		%itemDB = 0;
	}
	%data = %data TAB "IDB" SPC %itemDB;
	%data = %data TAB "IPOS" SPC mFloor(%obj.itemPosition);
	if (%obj.itemDirection $= "")
	{
		%obj.itemDirection = 2;
	}
	%data = %data TAB "IDIR" SPC mFloor(%obj.itemDirection);
	if (%obj.itemRespawnTime $= "")
	{
		%obj.itemRespawnTime = 4000;
	}
	%data = %data TAB "IRT" SPC mFloor(%obj.itemRespawnTime / 1000);
	%data = %data TAB "RC" SPC %obj.isRayCasting();
	%data = %data TAB "C" SPC %obj.isColliding();
	%data = %data TAB "R" SPC %obj.isRendering();
	%data = trim(%data);
	commandToClient(%client, 'SetWrenchData', %data);
	commandToClient(%client, 'WrenchLoadingDone');
}

datablock AudioProfile(wandHitSound)
{
	fileName = "~/data/sound/wandHit.wav";
	description = AudioClosest3d;
	preload = 1;
};
datablock ParticleData(wandExplosionParticle)
{
	dragCoefficient = 10;
	gravityCoefficient = 0;
	inheritedVelFactor = 0.2;
	constantAcceleration = 0;
	lifetimeMS = 500;
	lifetimeVarianceMS = 300;
	spinSpeed = 0;
	spinRandomMin = -150;
	spinRandomMax = 150;
	textureName = "~/data/particles/star1";
	colors[0] = "0.0 0.7 0.9 0.9";
	colors[1] = "0.0 0.7 0.9 0.9";
	colors[1] = "0.0 0.0 0.9 0.0";
	sizes[0] = 0;
	sizes[1] = 0.7;
	sizes[2] = 0.2;
	times[0] = 0;
	times[1] = 0.2;
	times[2] = 1;
};
datablock ParticleEmitterData(wandExplosionEmitter)
{
	lifetimeMS = 50;
	ejectionPeriodMS = 1;
	periodVarianceMS = 0;
	ejectionVelocity = 15;
	velocityVariance = 4;
	ejectionOffset = 0.25;
	thetaMin = 0;
	thetaMax = 120;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = "wandExplosionParticle";
};
datablock ExplosionData(wandExplosion)
{
	lifetimeMS = 500;
	emitter[0] = wandExplosionEmitter;
	soundProfile = wandHitSound;
	faceViewer = 1;
	explosionScale = "1 1 1";
	shakeCamera = 1;
	camShakeFreq = "2.0 2.0 2.0";
	camShakeAmp = "1.0 1.0 1.0";
	camShakeDuration = 0.5;
	camShakeRadius = 10;
	lightStartRadius = 3;
	lightEndRadius = 1;
	lightStartColor = "00.0 0.6 0.9";
	lightEndColor = "0 0 0";
};
datablock ProjectileData(wandProjectile)
{
	directDamage = 0;
	radiusDamage = 0;
	damageRadius = 0.5;
	Explosion = wandExplosion;
	muzzleVelocity = 50;
	velInheritFactor = 1;
	armingDelay = 0;
	lifetime = 0;
	fadeDelay = 70;
	bounceElasticity = 0;
	bounceFriction = 0;
	isBallistic = 0;
	gravityMod = 0;
	explodeOnDeath = 1;
	hasLight = 0;
	lightRadius = 3;
	lightColor = "0 0 0.5";
};
datablock ItemData(WandItem)
{
	category = "Weapon";
	className = "Tool";
	shapeFile = "~/data/shapes/wand.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = 1;
	uiName = "Wand";
	iconName = "base/client/ui/itemIcons/wand";
	image = WandImage;
	canDrop = 1;
};
function WandItem::onUse(%this, %player, %invPosition)
{
	%client = %player.client;
	if (isObject(%client))
	{
		%mg = %client.miniGame;
		if (isObject(%mg))
		{
			if (!%mg.enableWand)
			{
				return;
			}
		}
	}
	Weapon::onUse(%this, %player, %invPosition);
}

datablock ParticleData(WandParticleA)
{
	textureName = "~/data/particles/star1";
	dragCoefficient = 0;
	gravityCoefficient = -0.5;
	inheritedVelFactor = 0;
	windCoefficient = 0;
	constantAcceleration = 0;
	lifetimeMS = 600;
	lifetimeVarianceMS = 100;
	spinSpeed = 0;
	spinRandomMin = -90;
	spinRandomMax = 90;
	useInvAlpha = 0;
	colors[0] = "0.0   0   1.0 1.0";
	colors[1] = "0.0 0.0 0.6 0.0";
	sizes[0] = 0.3;
	sizes[1] = 0.1;
	times[0] = 0;
	times[1] = 1;
};
datablock ParticleEmitterData(WandEmitterA)
{
	ejectionPeriodMS = 4;
	periodVarianceMS = 2;
	ejectionVelocity = 0;
	ejectionOffset = 0.1;
	velocityVariance = 0;
	thetaMin = 0;
	thetaMax = 180;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = WandParticleA;
};
datablock ParticleData(WandParticleB)
{
	textureName = "~/data/particles/star1";
	dragCoefficient = 0;
	gravityCoefficient = -0.5;
	inheritedVelFactor = 0;
	windCoefficient = 0;
	constantAcceleration = 0;
	lifetimeMS = 600;
	lifetimeVarianceMS = 100;
	spinSpeed = 0;
	spinRandomMin = -90;
	spinRandomMax = 90;
	useInvAlpha = 0;
	colors[0] = "0.3   1   1.0 1.0";
	colors[1] = "0.0 0.0 0.6 0.0";
	sizes[0] = 0.3;
	sizes[1] = 0.1;
	times[0] = 0;
	times[1] = 1;
};
datablock ParticleEmitterData(WandEmitterB)
{
	ejectionPeriodMS = 4;
	periodVarianceMS = 2;
	ejectionVelocity = 0;
	ejectionOffset = 0.1;
	velocityVariance = 0;
	thetaMin = 0;
	thetaMax = 180;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = WandParticleB;
};
datablock ShapeBaseImageData(WandImage)
{
	shapeFile = "~/data/shapes/wand.dts";
	emap = 0;
	mountPoint = 0;
	eyeOffset = "0.7 1.2 -0.25";
	correctMuzzleVector = 0;
	className = "WeaponImage";
	Item = WandItem;
	ammo = " ";
	Projectile = wandProjectile;
	projectileType = Projectile;
	melee = 1;
	doRetraction = 0;
	armReady = 1;
	showBricks = 1;
	doColorShift = 1;
	colorShiftColor = "1 1 1 1";
	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0;
	stateTransitionOnTimeout[0] = "Ready";
	stateName[1] = "Ready";
	stateTransitionOnTriggerDown[1] = "PreFire";
	stateAllowImageChange[1] = 1;
	stateEmitter[1] = WandEmitterA;
	stateEmitterTime[1] = 0.15;
	stateEmitterNode[1] = "muzzlePoint";
	stateTransitionOnTimeout[1] = "Ready";
	stateTimoutValue[1] = 0.1;
	stateName[2] = "PreFire";
	stateScript[2] = "onPreFire";
	stateAllowImageChange[2] = 0;
	stateTimeoutValue[2] = 0.1;
	stateTransitionOnTimeout[2] = "Fire";
	stateName[3] = "Fire";
	stateTransitionOnTimeout[3] = "CheckFire";
	stateTimeoutValue[3] = 0.2;
	stateFire[3] = 1;
	stateAllowImageChange[3] = 1;
	stateSequence[3] = "Fire";
	stateScript[3] = "onFire";
	stateWaitForTimeout[3] = 1;
	stateEmitter[3] = WandEmitterB;
	stateEmitterTime[3] = 0.4;
	stateEmitterNode[3] = "blah";
	stateName[4] = "CheckFire";
	stateTransitionOnTriggerUp[4] = "StopFire";
	stateTransitionOnTriggerDown[4] = "Fire";
	stateName[5] = "StopFire";
	stateTransitionOnTimeout[5] = "Ready";
	stateTimeoutValue[5] = 0.2;
	stateAllowImageChange[5] = 1;
	stateWaitForTimeout[5] = 1;
	stateSequence[5] = "StopFire";
	stateScript[5] = "onStopFire";
};
function WandImage::onPreFire(%this, %obj, %slot)
{
	%obj.playThread(2, armattack);
}

function WandImage::onStopFire(%this, %obj, %slot)
{
	%obj.playThread(2, root);
}

function WandImage::onFire(%this, %player, %slot)
{
	%start = %player.getEyePoint();
	%muzzleVec = %player.getMuzzleVector(%slot);
	%muzzleVecZ = getWord(%muzzleVec, 2);
	if (%muzzleVecZ < -0.9)
	{
		%range = 5.5;
	}
	else
	{
		%range = 5;
	}
	%vec = VectorScale(%muzzleVec, %range * getWord(%player.getScale(), 2));
	%end = VectorAdd(%start, %vec);
	%mask = $TypeMasks::InteriorObjectType | $TypeMasks::FxBrickAlwaysObjectType | $TypeMasks::PlayerObjectType | $TypeMasks::VehicleObjectType | $TypeMasks::StaticShapeObjectType;
	if (%player.isMounted())
	{
		%exempt = %player.getObjectMount();
	}
	else
	{
		%exempt = %player;
	}
	%raycast = containerRayCast(%start, %end, %mask, %exempt);
	if (!%raycast)
	{
		%mask = $TypeMasks::TerrainObjectType;
		%raycast = containerRayCast(%start, %end, %mask, %exempt);
	}
	if (!%raycast)
	{
		return;
	}
	%hitObj = getWord(%raycast, 0);
	%hitPos = getWords(%raycast, 1, 3);
	%hitNormal = getWords(%raycast, 4, 6);
	%projectilePos = VectorSub(%hitPos, VectorScale(%player.getEyeVector(), 0.25));
	%p = new Projectile()
	{
		dataBlock = wandProjectile;
		initialVelocity = %hitNormal;
		initialPosition = %projectilePos;
		sourceObject = %player;
		sourceSlot = %slot;
		client = %player.client;
	};
	%p.setScale(%player.getScale());
	MissionCleanup.add(%p);
	%this.onHitObject(%player, %slot, %hitObj, %hitPos, %hitNormal);
}

function WandImage::onHitObject(%this, %player, %slot, %hitObj, %hitPos, %hitNormal)
{
	%client = %player.client;
	if (!isObject(%client))
	{
		return;
	}
	if (%hitObj.getType() & $TypeMasks::FxBrickAlwaysObjectType)
	{
		if (getTrustLevel(%player, %hitObj) < $TrustLevel::Wand)
		{
			if (%hitObj.stackBL_ID $= "" || %hitObj.stackBL_ID != %client.getBLID())
			{
				commandToClient(%client, 'CenterPrint', %hitObj.getGroup().name @ " does not trust you enough to do that.", 1);
				return;
			}
		}
		%hitObj.onToolBreak(%client);
		$CurrBrickKiller = %client;
		%hitObj.killBrick();
	}
	else if (%hitObj.getType() & $TypeMasks::PlayerObjectType)
	{
		if (miniGameCanDamage(%client, %hitObj) == 1)
		{
		}
		else if (miniGameCanDamage(%client, %hitObj) == 0)
		{
			commandToClient(%client, 'CenterPrint', %hitObj.client.getPlayerName() @ " is in a different minigame.", 1);
			return;
		}
		else if (getTrustLevel(%player, %hitObj) < $TrustLevel::Wand)
		{
			commandToClient(%client, 'CenterPrint', %hitObj.client.getPlayerName() @ " does not trust you enough to do that.", 1);
			return;
		}
		%hitObj.setVelocity("0 0 15");
	}
}

datablock AudioProfile(sprayFireSound)
{
	fileName = "~/data/sound/sprayLoop.wav";
	description = AudioClosestLooping3d;
	preload = 1;
};
datablock AudioProfile(sprayActivateSound)
{
	fileName = "~/data/sound/sprayActivate.wav";
	description = AudioClosest3d;
	preload = 1;
};
datablock ParticleData(bluePaintExplosionParticle)
{
	dragCoefficient = 2;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 600;
	lifetimeVarianceMS = 200;
	textureName = "~/data/particles/cloud";
	useInvAlpha = 1;
	spinSpeed = 100;
	spinRandomMin = -50;
	spinRandomMax = 50;
	colors[0] = "0.000 0.317 0.745 0.500";
	colors[1] = "0.000 0.317 0.745 0.000";
	sizes[0] = 0.8;
	sizes[1] = 1.2;
};
datablock ParticleData(bluePaintDropletParticle)
{
	dragCoefficient = 0;
	gravityCoefficient = 1;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 800;
	lifetimeVarianceMS = 200;
	textureName = "~/data/particles/chunk";
	useInvAlpha = 1;
	spinSpeed = 100;
	spinRandomMin = -50;
	spinRandomMax = 50;
	colors[0] = "0.000 0.317 0.745 0.500";
	colors[1] = "0.000 0.317 0.745 0.000";
	sizes[0] = 0.1;
	sizes[1] = 0;
};
datablock ParticleEmitterData(bluePaintExplosionEmitter)
{
	lifetimeMS = 100;
	ejectionPeriodMS = 40;
	periodVarianceMS = 0;
	ejectionVelocity = 3.5;
	velocityVariance = 0;
	ejectionOffset = 0;
	thetaMin = 35;
	thetaMax = 85;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = "bluePaintExplosionParticle";
};
datablock ParticleEmitterData(bluePaintDropletEmitter)
{
	lifetimeMS = 100;
	ejectionPeriodMS = 40;
	periodVarianceMS = 0;
	ejectionVelocity = 5;
	velocityVariance = 0;
	ejectionOffset = 0;
	thetaMin = 0;
	thetaMax = 90;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = "bluePaintDropletParticle";
};
datablock ExplosionData(bluePaintExplosion)
{
	lifetimeMS = 150;
	emitter[0] = bluePaintExplosionEmitter;
	emitter[1] = bluePaintDropletEmitter;
	faceViewer = 0;
	shakeCamera = 0;
};
datablock ProjectileData(bluePaintProjectile)
{
	className = paintProjectile;
	Explosion = bluePaintExplosion;
	muzzleVelocity = 20;
	velInheritFactor = 0;
	armingDelay = 0;
	lifetime = 400;
	fadeDelay = 0;
	bounceElasticity = 0;
	bounceFriction = 0;
	isBallistic = 0;
	gravityMod = 0;
	hasLight = 0;
};
datablock ParticleData(bluePaintParticle)
{
	dragCoefficient = 3;
	windCoefficient = 0;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 500;
	lifetimeVarianceMS = 75;
	spinSpeed = 10;
	spinRandomMin = -50;
	spinRandomMax = 50;
	useInvAlpha = 1;
	animateTexture = 0;
	textureName = "~/data/particles/cloud";
	colors[0] = "0.000 0.317 0.745 0.500";
	colors[1] = "0.000 0.317 0.745 0.000";
	sizes[0] = 0.1;
	sizes[1] = 0.9;
	times[0] = 0;
	times[1] = 1;
};
datablock ParticleEmitterData(bluePaintEmitter)
{
	ejectionPeriodMS = 3;
	periodVarianceMS = 0;
	ejectionVelocity = 20;
	velocityVariance = 0;
	ejectionOffset = 0;
	thetaMin = 0;
	thetaMax = 5;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = bluePaintParticle;
	useEmitterColors = 1;
};
datablock ShapeBaseImageData(blueSprayCanImage)
{
	shapeFile = "~/data/shapes/spraycan.dts";
	emap = 0;
	mountPoint = 0;
	offset = "0 0 0";
	eyeOffset = "0.7 1.0 -0.6";
	correctMuzzleVector = 0;
	className = "WeaponImage";
	Item = sprayCan;
	ammo = " ";
	Projectile = bluePaintProjectile;
	projectileType = Projectile;
	melee = 1;
	doRetraction = 0;
	armReady = 1;
	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0.5;
	stateTransitionOnTimeout[0] = "CapOff";
	stateTransitionOnTriggerDown[0] = "Fire";
	stateWaitForTimeout[0] = 0;
	stateSound[0] = sprayActivateSound;
	stateSequence[0] = "Shake";
	stateName[4] = "CapOff";
	stateSequence[4] = "capOff";
	stateTimeoutValue[4] = 0.2;
	stateTransitionOnTriggerDown[4] = "fire";
	stateWaitForTimeout[4] = 0;
	stateTransitionOnTimeout[4] = "Ready";
	stateName[1] = "Ready";
	stateTransitionOnTriggerDown[1] = "fire";
	stateAllowImageChange[1] = 1;
	stateName[2] = "Fire";
	stateScript[2] = "onFire";
	stateFire[2] = 1;
	stateAllowImageChange[2] = 1;
	stateTimeoutValue[2] = 0.04;
	stateTransitionOnTimeout[2] = "Fire";
	stateTransitionOnTriggerUp[2] = "StopFire";
	stateEmitter[2] = bluePaintEmitter;
	stateEmitterTime[2] = 0.07;
	stateEmitterNode[2] = "muzzleNode";
	stateSound[2] = sprayFireSound;
	stateSequence[2] = "Fire";
	stateName[3] = "StopFire";
	stateTransitionOnTimeout[3] = 0;
	stateSequence[3] = "StopFire";
	stateTransitionOnTimeout[3] = "Ready";
};
function paintProjectile::OnCollision(%this, %obj, %col, %fade, %pos, %normal)
{
	%className = %col.getClassName();
	if (%className $= "fxDTSBrick")
	{
		if (getTrustLevel(%obj, %col) < $TrustLevel::Paint)
		{
			commandToClient(%obj.client, 'CenterPrint', %col.getGroup().name @ " does not trust you enough to do that.", 1);
			return;
		}
		if (%col.colorID != %this.colorID)
		{
			if (isObject(%obj.client))
			{
				%obj.client.undoStack.push(%col TAB "COLOR" TAB %col.colorID);
			}
			%col.setColor(%this.colorID);
			if (isObject(%col.Vehicle))
			{
				if (%col.reColorVehicle)
				{
					%col.colorVehicle();
				}
			}
		}
	}
	else if (%className $= "Player" || %className $= "AIPlayer")
	{
		if (%col.getDataBlock().paintable)
		{
			if (!isObject(%col.spawnBrick))
			{
				return;
			}
			if (getTrustLevel(%obj, %col) < $TrustLevel::Paint)
			{
				commandToClient(%obj.client, 'CenterPrint', %col.spawnBrick.getGroup().name @ " does not trust you enough to do that.", 1);
				return;
			}
			if (isObject(%col.spawnBrick))
			{
				if (%col.spawnBrick.reColorVehicle)
				{
					return;
				}
			}
			if (isObject(%col.client))
			{
				return;
			}
			%color = getColorIDTable(%this.colorID);
			%rgba = getWords(%color, 0, 2) SPC "1";
			if (%col.color $= "")
			{
				%col.color = "1 1 1 1";
			}
			if (isObject(%obj.client))
			{
				if (%rgba !$= %col.color)
				{
					%obj.client.undoStack.push(%col TAB "COLORGENERIC" TAB %col.color);
				}
			}
			%col.setNodeColor("ALL", %rgba);
			%col.color = %rgba;
		}
		else
		{
			%color = getColorIDTable(%this.colorID);
			%rgba = getWords(%color, 0, 2) SPC "1";
			if (%col.color $= "")
			{
				%col.color = "1 1 1 1";
			}
			%col.SetTempColor(%rgba, 2000, %obj.getLastImpactPosition(), %this);
		}
	}
	else if (%className $= "WheeledVehicle" || %className $= "HoverVehicle" || %className $= "FlyingVehicle")
	{
		if (!isObject(%col.spawnBrick))
		{
			return;
		}
		if (%col.spawnBrick.reColorVehicle)
		{
			return;
		}
		if (!%col.getDataBlock().paintable)
		{
			return;
		}
		if (%col.getDamagePercent() >= 1)
		{
			return;
		}
		if (getTrustLevel(%obj, %col) < $TrustLevel::Paint)
		{
			commandToClient(%obj.client, 'CenterPrint', %col.spawnBrick.getGroup().name @ " does not trust you enough to do that.", 1);
			return;
		}
		%color = getColorIDTable(%this.colorID);
		%rgba = getWords(%color, 0, 2) SPC "1";
		if (%col.color $= "")
		{
			%col.color = "1 1 1 1";
		}
		if (isObject(%obj.client))
		{
			if (%rgba !$= %col.color)
			{
				%obj.client.undoStack.push(%col TAB "COLORGENERIC" TAB %col.color);
			}
		}
		%col.setNodeColor("ALL", %rgba);
		%col.color = %rgba;
	}
}

function setSprayCanColors()
{
	%filename = "config/server/colorSet.txt";
	if (!isFile(%filename))
	{
		echo("Colorset not found, creating default colorset file");
		%file = new FileObject();
		%file.openForWrite(%filename);
		%file.writeLine("0.900 0.000 0.000 1.000");
		%file.writeLine("0.900 0.900 0.000 1.000");
		%file.writeLine("0.000 0.500 0.250 1.000");
		%file.writeLine("0.200 0.000 0.800 1.000");
		%file.writeLine("0.900 0.900 0.900 1.000");
		%file.writeLine("0.750 0.750 0.750 1.000");
		%file.writeLine("0.500 0.500 0.500 1.000");
		%file.writeLine("0.200 0.200 0.200 1.000");
		%file.writeLine("100 50 0 255");
		%file.writeLine("DIV:Standard");
		%file.writeLine("");
		%file.writeLine("230 87 20 255");
		%file.writeLine("191 46 123 255");
		%file.writeLine("99 0 30 255");
		%file.writeLine("34 69 69 255");
		%file.writeLine("0 36 85 255");
		%file.writeLine("27 117 196 255");
		%file.writeLine("255 255 255 255");
		%file.writeLine("20 20 20 255");
		%file.writeLine("255 255 255 64");
		%file.writeLine("DIV:Bold");
		%file.writeLine("");
		%file.writeLine("236 131 173 255");
		%file.writeLine("255 154 108 255");
		%file.writeLine("255 224 156 255");
		%file.writeLine("244 224 200 255");
		%file.writeLine("200 235 125 255");
		%file.writeLine("138 178 141 255");
		%file.writeLine("143 237 245 255");
		%file.writeLine("178 169 231 255");
		%file.writeLine("224 143 244 255");
		%file.writeLine("DIV:Soft");
		%file.writeLine("");
		%file.writeLine("0.667 0.000 0.000 0.700");
		%file.writeLine("1.000 0.500 0.000 0.700");
		%file.writeLine("0.990 0.960 0.000 0.700");
		%file.writeLine("0.000 0.471 0.196 0.700");
		%file.writeLine("0.000 0.200 0.640 0.700");
		%file.writeLine("152 41 100 178");
		%file.writeLine("0.550 0.700 1.000 0.700");
		%file.writeLine("0.850 0.850 0.850 0.700");
		%file.writeLine("0.100 0.100 0.100 0.700");
		%file.writeLine("DIV:Transparent");
		%file.close();
		%file.delete();
	}
	if (!isFile(%filename))
	{
		error("ERROR: setSprayCanColors() - File \"" @ %filename @ "\" not found and could not be created!");
		return 0;
	}
	%file = new FileObject();
	%file.openForRead(%filename);
	%i = -1;
	%divCount = -1;
	while (!%file.isEOF())
	{
		%line = %file.readLine();
		if (getSubStr(%line, 0, 4) $= "DIV:")
		{
			%divName = getSubStr(%line, 4, strlen(%line) - 4);
			setSprayCanDivision(%divCount++, %i, %divName);
		}
		else if (%line !$= "")
		{
			if (%i >= 63)
			{
				break;
			}
			%r = mAbs(getWord(%line, 0));
			%g = mAbs(getWord(%line, 1));
			%b = mAbs(getWord(%line, 2));
			%a = mAbs(getWord(%line, 3));
			if (mFloor(%r) != %r || mFloor(%g) != %g || mFloor(%b) != %b || mFloor(%a) != %a || %r <= 1 && %g <= 1 && %b <= 1 && %a <= 1)
			{
				setSprayCanColor(%i++, %r SPC %g SPC %b SPC %a);
				continue;
			}
			setSprayCanColorI(%i++, %r SPC %g SPC %b SPC %a);
		}
	}
	%file.close();
	%file.delete();
	$maxSprayColors = %i;
	for (%j = %divCount + 1; %j < 16; %j++)
	{
		setSprayCanDivision(%j, 0, "");
	}
	for (%j = %i + 1; %j < 64; %j++)
	{
		setColorTable(%j, "1.0 0.0 1.0 0.0");
	}
	return 1;
}

function setSprayCanColorI(%id, %color)
{
	%red = getWord(%color, 0);
	%green = getWord(%color, 1);
	%blue = getWord(%color, 2);
	%alpha = getWord(%color, 3);
	%red = mClamp(%red, 0, 255);
	%green = mClamp(%green, 0, 255);
	%blue = mClamp(%blue, 0, 255);
	%alpha = mClamp(%alpha, 1, 255);
	%red /= 255;
	%green /= 255;
	%blue /= 255;
	%alpha /= 255;
	%floatColor = %red SPC %green SPC %blue SPC %alpha;
	setSprayCanColor(%id, %floatColor);
}

function setSprayCanColor(%id, %color)
{
	%red = getWord(%color, 0);
	%green = getWord(%color, 1);
	%blue = getWord(%color, 2);
	%alpha = getWord(%color, 3);
	%red = mClampF(%red, 0, 1);
	%green = mClampF(%green, 0, 1);
	%blue = mClampF(%blue, 0, 1);
	%alpha = mClampF(%alpha, 1 / 255, 1);
	%imageAlpha = mClampF(%alpha, 10 / 255, 1);
	%color = %red SPC %green SPC %blue SPC %alpha;
	%imagecolor = %red SPC %green SPC %blue SPC %imageAlpha;
	setColorTable(%id, %color);
	%rgbColor = getWords(%color, 0, 2);
	%alphaVal = getWord(%color, 3);
	if (%alphaVal > 0.99)
	{
		%invalpha = 1;
	}
	else
	{
		%invalpha = 0;
		if (%red < 8 / 255 && %green < 8 / 255 && %blue < 8 / 255)
		{
			%rgbColor = 8 / 255 SPC 8 / 255 SPC 8 / 255;
		}
	}
	%dbName = "color" @ %id @ "PaintExplosionParticle";
	%commandString = "datablock ParticleData(" @ %dbName @ " : bluePaintExplosionParticle)" @ "{ colors[0] = \"" @ %rgbColor @ " 0.500\";" @ "  colors[1] = \"" @ %rgbColor @ " 0.000\";" @ " useInvAlpha = " @ %invalpha @ ";" @ "};";
	eval(%commandString);
	%dbName = "color" @ %id @ "PaintDropletParticle";
	%commandString = "datablock ParticleData(" @ %dbName @ " : bluePaintDropletParticle)" @ "{ colors[0] = \"" @ %rgbColor @ " 0.500\";" @ "  colors[1] = \"" @ %rgbColor @ " 0.000\";" @ " useInvAlpha = " @ %invalpha @ ";" @ "};";
	eval(%commandString);
	%dbName = "color" @ %id @ "PaintExplosionEmitter";
	%particleDBName = "color" @ %id @ "PaintExplosionParticle";
	%commandString = "datablock ParticleEmitterData(" @ %dbName @ " : bluePaintExplosionEmitter)" @ "{ particles = " @ %particleDBName @ ";" @ "};";
	eval(%commandString);
	%dbName = "color" @ %id @ "PaintDropletEmitter";
	%particleDBName = "color" @ %id @ "PaintDropletParticle";
	%commandString = "datablock ParticleEmitterData(" @ %dbName @ " : bluePaintDropletEmitter)" @ "{ particles = " @ %particleDBName @ ";" @ "};";
	eval(%commandString);
	%dbName = "color" @ %id @ "PaintExplosion";
	%emitter0Name = "color" @ %id @ "PaintExplosionEmitter";
	%emitter1Name = "color" @ %id @ "PaintDropletEmitter";
	%commandString = "datablock ExplosionData(" @ %dbName @ " : bluePaintExplosion)" @ "{ emitter[0] = " @ %emitter0Name @ ";" @ "  emitter[1] = " @ %emitter1Name @ ";" @ "};";
	eval(%commandString);
	%dbName = "color" @ %id @ "PaintProjectile";
	%explosionDBName = "color" @ %id @ "PaintExplosion";
	%commandString = "datablock ProjectileData(" @ %dbName @ " : bluePaintProjectile)" @ "{ explosion = " @ %explosionDBName @ ";" @ " colorID = " @ %id @ ";" @ "};";
	eval(%commandString);
	%dbName = "color" @ %id @ "SprayCanImage";
	%projectileDBName = "color" @ %id @ "PaintProjectile";
	%stateEmitterDBName = "color" @ %id @ "PaintEmitter";
	if (%alphaVal > 0.99)
	{
		%shapeFile = "base/data/shapes/spraycan.dts";
	}
	else
	{
		%shapeFile = "base/data/shapes/transspraycan.dts";
	}
	%commandString = "datablock ShapeBaseImageData(" @ %dbName @ " : blueSprayCanImage)" @ "{ projectile = " @ %projectileDBName @ ";" @ "  doColorShift = true;" @ "  colorShiftColor = \"" @ %imagecolor @ "\";" @ "  shapeFile = \"" @ %shapeFile @ "\";" @ "};";
	eval(%commandString);
}

datablock ParticleData(AdminWandExplosionParticle)
{
	dragCoefficient = 14;
	gravityCoefficient = 0;
	inheritedVelFactor = 0.2;
	constantAcceleration = 0;
	lifetimeMS = 500;
	lifetimeVarianceMS = 300;
	spinSpeed = 0;
	spinRandomMin = -150;
	spinRandomMax = 150;
	textureName = "~/data/particles/cloud";
	colors[0] = "0.9 0.9 0.0 0.9";
	colors[1] = "0.9 0.4 0.0 0.9";
	colors[2] = "0.9 0.0 0.0 0.0";
	sizes[0] = 0;
	sizes[1] = 0.7;
	sizes[2] = 0.2;
	times[0] = 0;
	times[1] = 0.2;
	times[2] = 1;
};
datablock ParticleEmitterData(AdminWandExplosionEmitter)
{
	lifetimeMS = 50;
	ejectionPeriodMS = 1;
	periodVarianceMS = 0;
	ejectionVelocity = 15;
	velocityVariance = 4;
	ejectionOffset = 0.05;
	thetaMin = 0;
	thetaMax = 120;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = "AdminWandExplosionParticle";
};
datablock ExplosionData(AdminWandExplosion)
{
	lifetimeMS = 500;
	emitter[0] = AdminWandExplosionEmitter;
	soundProfile = wandHitSound;
	faceViewer = 1;
	explosionScale = "1 1 1";
	shakeCamera = 1;
	camShakeFreq = "2.0 2.0 2.0";
	camShakeAmp = "1.0 1.0 1.0";
	camShakeDuration = 0.5;
	camShakeRadius = 10;
	lightStartRadius = 3;
	lightEndRadius = 1;
	lightStartColor = "00.9 0.6 0.0";
	lightEndColor = "0 0 0";
	radiusDamage = 0;
	damageRadius = 0;
	impulseRadius = 0;
	impulseForce = 0;
};
datablock ProjectileData(AdminWandProjectile)
{
	directDamage = 0;
	impactImpulse = 0;
	verticalImpulse = 0;
	Explosion = AdminWandExplosion;
	muzzleVelocity = 50;
	velInheritFactor = 1;
	armingDelay = 0;
	lifetime = 0;
	fadeDelay = 70;
	bounceElasticity = 0;
	bounceFriction = 0;
	isBallistic = 0;
	gravityMod = 0;
	explodeOnDeath = 1;
	hasLight = 0;
	lightRadius = 3;
	lightColor = "0 0 0.5";
};
datablock ParticleData(AdminWandParticleA)
{
	textureName = "~/data/particles/cloud";
	dragCoefficient = 0;
	gravityCoefficient = -0.5;
	inheritedVelFactor = 0;
	windCoefficient = 0;
	constantAcceleration = 0;
	lifetimeMS = 600;
	lifetimeVarianceMS = 100;
	spinSpeed = 0;
	spinRandomMin = -90;
	spinRandomMax = 90;
	useInvAlpha = 0;
	colors[0] = "0.9 0.4 0.0 1.0";
	colors[1] = "0.9 0.0 0.0 0.0";
	sizes[0] = 0.3;
	sizes[1] = 0.1;
	times[0] = 0;
	times[1] = 1;
};
datablock ParticleEmitterData(AdminWandEmitterA)
{
	ejectionPeriodMS = 4;
	periodVarianceMS = 2;
	ejectionVelocity = 0;
	ejectionOffset = 0.1;
	velocityVariance = 0;
	thetaMin = 0;
	thetaMax = 180;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = AdminWandParticleA;
};
datablock ParticleData(AdminWandParticleB)
{
	textureName = "~/data/particles/cloud";
	dragCoefficient = 0;
	gravityCoefficient = -0.5;
	inheritedVelFactor = 0;
	windCoefficient = 0;
	constantAcceleration = 0;
	lifetimeMS = 600;
	lifetimeVarianceMS = 100;
	spinSpeed = 0;
	spinRandomMin = -90;
	spinRandomMax = 90;
	useInvAlpha = 0;
	colors[0] = "1.0 1.0 0.0 1.0";
	colors[1] = "0.9 0.4 0.0 0.0";
	sizes[0] = 0.3;
	sizes[1] = 0.1;
	times[0] = 0;
	times[1] = 1;
};
datablock ParticleEmitterData(AdminWandEmitterB)
{
	ejectionPeriodMS = 4;
	periodVarianceMS = 2;
	ejectionVelocity = 0;
	ejectionOffset = 0.1;
	velocityVariance = 0;
	thetaMin = 0;
	thetaMax = 180;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = AdminWandParticleB;
};
datablock ShapeBaseImageData(AdminWandImage)
{
	shapeFile = "~/data/shapes/wand.dts";
	emap = 0;
	mountPoint = 0;
	eyeOffset = "0.7 1.2 -0.25";
	correctMuzzleVector = 0;
	className = "WeaponImage";
	Item = "";
	ammo = " ";
	Projectile = AdminWandProjectile;
	projectileType = Projectile;
	melee = 1;
	doRetraction = 0;
	armReady = 1;
	showBricks = 1;
	doColorShift = 1;
	colorShiftColor = "1 0 0 1";
	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0;
	stateTransitionOnTimeout[0] = "Ready";
	stateName[1] = "Ready";
	stateTransitionOnTriggerDown[1] = "PreFire";
	stateAllowImageChange[1] = 1;
	stateEmitter[1] = AdminWandEmitterA;
	stateEmitterTime[1] = 0.15;
	stateEmitterNode[1] = "muzzlePoint";
	stateTransitionOnTimeout[1] = "Ready";
	stateTimoutValue[1] = 0.1;
	stateName[2] = "PreFire";
	stateScript[2] = "onPreFire";
	stateAllowImageChange[2] = 0;
	stateTimeoutValue[2] = 0.1;
	stateTransitionOnTimeout[2] = "Fire";
	stateName[3] = "Fire";
	stateTransitionOnTimeout[3] = "CheckFire";
	stateTimeoutValue[3] = 0.2;
	stateFire[3] = 1;
	stateAllowImageChange[3] = 1;
	stateSequence[3] = "Fire";
	stateScript[3] = "onFire";
	stateWaitForTimeout[3] = 1;
	stateEmitter[3] = AdminWandEmitterB;
	stateEmitterTime[3] = 0.4;
	stateEmitterNode[3] = "blah";
	stateName[4] = "CheckFire";
	stateTransitionOnTriggerUp[4] = "StopFire";
	stateTransitionOnTriggerDown[4] = "Fire";
	stateName[5] = "StopFire";
	stateTransitionOnTimeout[5] = "Ready";
	stateTimeoutValue[5] = 0.2;
	stateAllowImageChange[5] = 1;
	stateWaitForTimeout[5] = 1;
	stateSequence[5] = "StopFire";
	stateScript[5] = "onStopFire";
};
function AdminWandImage::onPreFire(%this, %obj, %slot)
{
	%obj.playThread(2, armattack);
}

function AdminWandImage::onStopFire(%this, %obj, %slot)
{
	%obj.playThread(2, root);
}

function AdminWandImage::onFire(%this, %player, %slot)
{
	%start = %player.getEyePoint();
	%muzzleVec = %player.getMuzzleVector(%slot);
	%muzzleVecZ = getWord(%muzzleVec, 2);
	if (%muzzleVecZ < -0.9)
	{
		%range = 5.5;
	}
	else
	{
		%range = 5;
	}
	%range = 500;
	%vec = VectorScale(%muzzleVec, %range * getWord(%player.getScale(), 2));
	%end = VectorAdd(%start, %vec);
	%mask = $TypeMasks::InteriorObjectType | $TypeMasks::FxBrickAlwaysObjectType | $TypeMasks::PlayerObjectType | $TypeMasks::VehicleObjectType | $TypeMasks::StaticShapeObjectType;
	if (%player.isMounted())
	{
		%exempt = %player.getObjectMount();
	}
	else
	{
		%exempt = %player;
	}
	%raycast = containerRayCast(%start, %end, %mask, %exempt);
	if (!%raycast)
	{
		%mask = $TypeMasks::TerrainObjectType;
		%raycast = containerRayCast(%start, %end, %mask, %exempt);
	}
	if (!%raycast)
	{
		return;
	}
	%hitObj = getWord(%raycast, 0);
	%hitPos = getWords(%raycast, 1, 3);
	%hitNormal = getWords(%raycast, 4, 6);
	%projectilePos = VectorSub(%hitPos, VectorScale(%player.getEyeVector(), 0.25));
	%p = new Projectile()
	{
		dataBlock = AdminWandProjectile;
		initialVelocity = %hitNormal;
		initialPosition = %projectilePos;
		sourceObject = %player;
		sourceSlot = %slot;
		client = %player.client;
	};
	%p.setScale(%player.getScale());
	MissionCleanup.add(%p);
	%this.onHitObject(%player, %slot, %hitObj, %hitPos, %hitNormal);
}

function AdminWandImage::onHitObject(%this, %player, %slot, %hitObj, %hitPos, %hitNormal)
{
	%client = %player.client;
	if (!isObject(%client))
	{
		return;
	}
	if (!%client.isAdmin)
	{
		return;
	}
	if (%hitObj.getType() & $TypeMasks::FxBrickAlwaysObjectType)
	{
		$CurrBrickKiller = %client;
		%hitObj.killBrick();
	}
	else if (%hitObj.getType() & $TypeMasks::PlayerObjectType)
	{
		%vel = VectorNormalize(%player.getEyeVector());
		%vel = VectorAdd(%vel, "0 0 1");
		%vel = VectorNormalize(%vel);
		%vel = VectorScale(%vel, 20);
		%hitObj.setVelocity(%vel);
	}
}

datablock ParticleData(flatPaintExplosionParticle)
{
	dragCoefficient = 2;
	gravityCoefficient = 0.5;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 600;
	lifetimeVarianceMS = 400;
	textureName = "~/data/particles/cloud";
	useInvAlpha = 1;
	spinSpeed = 100;
	spinRandomMin = -50;
	spinRandomMax = 50;
	colors[0] = "0.2 0.2 0.2 0.50";
	colors[1] = "0.2 0.2 0.2 1.00";
	colors[2] = "0.0 0.0 0.0 0.000";
	sizes[0] = 0.9;
	sizes[1] = 0.3;
	sizes[2] = 0;
	times[0] = 0;
	times[1] = 0.7;
	times[2] = 1;
};
datablock ParticleEmitterData(flatPaintExplosionEmitter)
{
	lifetimeMS = 100;
	ejectionPeriodMS = 40;
	periodVarianceMS = 0;
	ejectionVelocity = 1.5;
	velocityVariance = 0;
	ejectionOffset = 0;
	thetaMin = 35;
	thetaMax = 85;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = "flatPaintExplosionParticle";
};
datablock ExplosionData(flatPaintExplosion)
{
	lifetimeMS = 150;
	emitter[0] = flatPaintExplosionEmitter;
	faceViewer = 0;
	shakeCamera = 0;
};
datablock ProjectileData(flatPaintProjectile)
{
	className = paintProjectile;
	Explosion = flatPaintExplosion;
	muzzleVelocity = 20;
	velInheritFactor = 0;
	armingDelay = 0;
	lifetime = 525;
	fadeDelay = 0;
	bounceElasticity = 0;
	bounceFriction = 0;
	isBallistic = 0;
	gravityMod = 0;
	hasLight = 0;
	lightRadius = 3;
	lightColor = "0 0 0.5";
};
datablock ParticleData(flatPaintParticle)
{
	dragCoefficient = 0;
	windCoefficient = 0;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 5;
	lifetimeMS = 420;
	lifetimeVarianceMS = 75;
	spinSpeed = 0;
	spinRandomMin = -150;
	spinRandomMax = 150;
	useInvAlpha = 1;
	animateTexture = 0;
	textureName = "~/data/particles/thinring";
	colors[0] = "0.2 0.2 0.2 1.00";
	colors[1] = "0.2 0.2 0.2 1.00";
	sizes[0] = 0.1;
	sizes[1] = 1;
	times[0] = 0;
	times[1] = 1;
};
datablock ParticleEmitterData(flatPaintEmitter)
{
	ejectionPeriodMS = 10;
	periodVarianceMS = 0;
	ejectionVelocity = 7;
	velocityVariance = 0;
	ejectionOffset = 0;
	thetaMin = 0;
	thetaMax = 0;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = flatPaintParticle;
};
datablock ShapeBaseImageData(flatSprayCanImage)
{
	shapeFile = "~/data/shapes/spraycan.dts";
	emap = 0;
	mountPoint = 0;
	offset = "0 0 0";
	eyeOffset = "0.7 1.0 -0.6";
	correctMuzzleVector = 0;
	className = "WeaponImage";
	Item = sprayCan;
	ammo = " ";
	Projectile = flatPaintProjectile;
	projectileType = Projectile;
	doColorShift = 1;
	colorShiftColor = "0.2 0.2 0.2 1.0";
	melee = 1;
	doRetraction = 0;
	armReady = 1;
	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0.5;
	stateTransitionOnTimeout[0] = "CapOff";
	stateTransitionOnTriggerDown[0] = "Fire";
	stateWaitForTimeout[0] = 0;
	stateSound[0] = sprayActivateSound;
	stateSequence[0] = "Shake";
	stateName[4] = "CapOff";
	stateSequence[4] = "capOff";
	stateTimeoutValue[4] = 0.2;
	stateTransitionOnTriggerDown[4] = "fire";
	stateWaitForTimeout[4] = 0;
	stateTransitionOnTimeout[4] = "Ready";
	stateName[1] = "Ready";
	stateTransitionOnTriggerDown[1] = "fire";
	stateAllowImageChange[1] = 1;
	stateName[2] = "Fire";
	stateScript[2] = "onFire";
	stateFire[2] = 1;
	stateAllowImageChange[2] = 1;
	stateTimeoutValue[2] = 0.04;
	stateTransitionOnTimeout[2] = "Fire";
	stateTransitionOnTriggerUp[2] = "StopFire";
	stateEmitter[2] = flatPaintEmitter;
	stateEmitterTime[2] = 0.06;
	stateEmitterNode[2] = "muzzleNode";
	stateSound[2] = sprayFireSound;
	stateSequence[2] = "Fire";
	stateName[3] = "StopFire";
	stateTransitionOnTimeout[3] = 0;
	stateSequence[3] = "StopFire";
	stateTransitionOnTimeout[3] = "Ready";
};
function flatPaintProjectile::OnCollision(%this, %obj, %col, %fade, %pos, %normal)
{
	if (%col.getClassName() $= "fxDTSBrick")
	{
		if (%col.colorFxID != 0)
		{
			%brickGroup = %col.getGroup();
			%client = %obj.client;
			if (!isObject(%client))
			{
				return;
			}
			if (%client.brickGroup != %brickGroup)
			{
				%trustLevel = %brickGroup.Trust[%client.getBLID()];
				if (%trustLevel < $TrustLevel::FXPaint)
				{
					commandToClient(%client, 'CenterPrint', %brickGroup.name @ " does not trust you enough to do that.", 1);
					return;
				}
			}
			if (isObject(%obj.client))
			{
				%obj.client.undoStack.push(%col TAB "COLORFX" TAB %col.colorFxID);
			}
			%col.setColorFX(0);
		}
	}
}

datablock ParticleData(pearlPaintExplosionParticle)
{
	dragCoefficient = 2;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 600;
	lifetimeVarianceMS = 400;
	textureName = "~/data/particles/bubble";
	useInvAlpha = 0;
	spinSpeed = 100;
	spinRandomMin = -50;
	spinRandomMax = 50;
	colors[0] = "0.9 0.9 1 1.00";
	colors[1] = "1 1 1 1.00";
	colors[2] = "0.5 0.5 0.5 0.000";
	sizes[0] = 0.2;
	sizes[1] = 0.2;
	sizes[2] = 0.9;
	times[0] = 0;
	times[1] = 0.9;
	times[2] = 1;
};
datablock ParticleData(pearlPaintDropletParticle)
{
	dragCoefficient = 0;
	gravityCoefficient = 1;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 800;
	lifetimeVarianceMS = 200;
	textureName = "~/data/particles/chunk";
	useInvAlpha = 0;
	spinSpeed = 100;
	spinRandomMin = -50;
	spinRandomMax = 50;
	colors[0] = "1 1 1 0.500";
	colors[1] = "0.000 0.317 0.745 0.000";
	sizes[0] = 0.1;
	sizes[1] = 0;
};
datablock ParticleEmitterData(pearlPaintExplosionEmitter)
{
	lifetimeMS = 100;
	ejectionPeriodMS = 40;
	periodVarianceMS = 0;
	ejectionVelocity = 3.5;
	velocityVariance = 0;
	ejectionOffset = 0;
	thetaMin = 35;
	thetaMax = 85;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = "pearlPaintExplosionParticle";
};
datablock ParticleEmitterData(pearlPaintDropletEmitter)
{
	lifetimeMS = 100;
	ejectionPeriodMS = 40;
	periodVarianceMS = 0;
	ejectionVelocity = 5;
	velocityVariance = 0;
	ejectionOffset = 0;
	thetaMin = 0;
	thetaMax = 90;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = "pearlPaintDropletParticle";
};
datablock ExplosionData(pearlPaintExplosion)
{
	lifetimeMS = 150;
	emitter[0] = pearlPaintExplosionEmitter;
	emitter[1] = pearlPaintDropletEmitter;
	faceViewer = 0;
	shakeCamera = 0;
};
datablock ProjectileData(pearlPaintProjectile)
{
	className = paintProjectile;
	Explosion = pearlPaintExplosion;
	muzzleVelocity = 20;
	velInheritFactor = 0;
	armingDelay = 0;
	lifetime = 525;
	fadeDelay = 0;
	bounceElasticity = 0;
	bounceFriction = 0;
	isBallistic = 0;
	gravityMod = 0;
	hasLight = 0;
	lightRadius = 3;
	lightColor = "0 0 0.5";
};
datablock ParticleData(pearlPaintParticle)
{
	dragCoefficient = 0;
	windCoefficient = 0;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 420;
	lifetimeVarianceMS = 175;
	spinSpeed = 0;
	spinRandomMin = -50;
	spinRandomMax = 50;
	useInvAlpha = 0;
	animateTexture = 0;
	textureName = "~/data/particles/bubble";
	colors[0] = "1 1 1 1.00";
	colors[1] = "1 1 1 1.00";
	colors[2] = "0.5 0.5 0.5 0.000";
	sizes[0] = 0.1;
	sizes[1] = 0.5;
	sizes[2] = 1.2;
	times[0] = 0;
	times[1] = 0.9;
	times[2] = 1;
};
datablock ParticleEmitterData(pearlPaintEmitter)
{
	ejectionPeriodMS = 8;
	periodVarianceMS = 0;
	ejectionVelocity = 20;
	velocityVariance = 0;
	ejectionOffset = 0;
	thetaMin = 0;
	thetaMax = 5;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = pearlPaintParticle;
};
datablock ShapeBaseImageData(pearlSprayCanImage)
{
	shapeFile = "~/data/shapes/spraycan.dts";
	emap = 0;
	mountPoint = 0;
	offset = "0 0 0";
	eyeOffset = "0.7 1.0 -0.6";
	correctMuzzleVector = 0;
	className = "WeaponImage";
	Item = sprayCan;
	ammo = " ";
	Projectile = pearlPaintProjectile;
	projectileType = Projectile;
	doColorShift = 1;
	colorShiftColor = "1.0 1.0 0.9098 1.0";
	melee = 1;
	doRetraction = 0;
	armReady = 1;
	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0.5;
	stateTransitionOnTimeout[0] = "CapOff";
	stateTransitionOnTriggerDown[0] = "Fire";
	stateWaitForTimeout[0] = 0;
	stateSound[0] = sprayActivateSound;
	stateSequence[0] = "Shake";
	stateName[4] = "CapOff";
	stateSequence[4] = "capOff";
	stateTimeoutValue[4] = 0.2;
	stateTransitionOnTriggerDown[4] = "fire";
	stateWaitForTimeout[4] = 0;
	stateTransitionOnTimeout[4] = "Ready";
	stateName[1] = "Ready";
	stateTransitionOnTriggerDown[1] = "fire";
	stateAllowImageChange[1] = 1;
	stateName[2] = "Fire";
	stateScript[2] = "onFire";
	stateFire[2] = 1;
	stateAllowImageChange[2] = 1;
	stateTimeoutValue[2] = 0.04;
	stateTransitionOnTimeout[2] = "Fire";
	stateTransitionOnTriggerUp[2] = "StopFire";
	stateEmitter[2] = pearlPaintEmitter;
	stateEmitterTime[2] = 0.06;
	stateEmitterNode[2] = "muzzleNode";
	stateSound[2] = sprayFireSound;
	stateSequence[2] = "Fire";
	stateName[3] = "StopFire";
	stateTransitionOnTimeout[3] = 0;
	stateSequence[3] = "StopFire";
	stateTransitionOnTimeout[3] = "Ready";
};
function pearlPaintProjectile::OnCollision(%this, %obj, %col, %fade, %pos, %normal)
{
	if (%col.getClassName() $= "fxDTSBrick")
	{
		if (%col.colorFxID != 1)
		{
			%brickGroup = %col.getGroup();
			%client = %obj.client;
			if (!isObject(%client))
			{
				return;
			}
			if (%client.brickGroup != %brickGroup)
			{
				%trustLevel = %brickGroup.Trust[%client.getBLID()];
				if (%trustLevel < $TrustLevel::FXPaint)
				{
					commandToClient(%client, 'CenterPrint', %brickGroup.name @ " does not trust you enough to do that.", 1);
					return;
				}
			}
			if (isObject(%obj.client))
			{
				%obj.client.undoStack.push(%col TAB "COLORFX" TAB %col.colorFxID);
			}
			%col.setColorFX(1);
		}
	}
}

datablock ParticleData(chromePaintExplosionParticle)
{
	dragCoefficient = 2;
	gravityCoefficient = 0.5;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 600;
	lifetimeVarianceMS = 400;
	textureName = "~/data/particles/bubble";
	useInvAlpha = 1;
	spinSpeed = 100;
	spinRandomMin = -50;
	spinRandomMax = 50;
	colors[0] = "0.9 0.9 1 1.00";
	colors[1] = "1 1 1 1.00";
	colors[2] = "0.5 0.5 0.5 0.000";
	sizes[0] = 0.2;
	sizes[1] = 0.3;
	sizes[2] = 0;
	times[0] = 0;
	times[1] = 0.7;
	times[2] = 1;
};
datablock ParticleData(chromePaintDropletParticle)
{
	dragCoefficient = 0;
	gravityCoefficient = 1;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 800;
	lifetimeVarianceMS = 200;
	textureName = "~/data/particles/bubble";
	useInvAlpha = 0;
	spinSpeed = 100;
	spinRandomMin = -50;
	spinRandomMax = 50;
	colors[0] = "1 1 1 0.500";
	colors[1] = "0.000 0.317 0.745 0.000";
	sizes[0] = 0.1;
	sizes[1] = 0;
};
datablock ParticleEmitterData(chromePaintExplosionEmitter)
{
	lifetimeMS = 100;
	ejectionPeriodMS = 40;
	periodVarianceMS = 0;
	ejectionVelocity = 3.5;
	velocityVariance = 0;
	ejectionOffset = 0;
	thetaMin = 35;
	thetaMax = 85;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = "chromePaintExplosionParticle";
};
datablock ParticleEmitterData(chromePaintDropletEmitter)
{
	lifetimeMS = 100;
	ejectionPeriodMS = 40;
	periodVarianceMS = 0;
	ejectionVelocity = 5;
	velocityVariance = 0;
	ejectionOffset = 0;
	thetaMin = 0;
	thetaMax = 90;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = "chromePaintDropletParticle";
};
datablock ExplosionData(chromePaintExplosion)
{
	lifetimeMS = 150;
	emitter[0] = chromePaintExplosionEmitter;
	emitter[1] = chromePaintDropletEmitter;
	faceViewer = 0;
	shakeCamera = 0;
};
datablock ProjectileData(chromePaintProjectile)
{
	className = paintProjectile;
	Explosion = chromePaintExplosion;
	muzzleVelocity = 20;
	velInheritFactor = 0;
	armingDelay = 0;
	lifetime = 525;
	fadeDelay = 0;
	bounceElasticity = 0;
	bounceFriction = 0;
	isBallistic = 0;
	gravityMod = 0;
	hasLight = 0;
	lightRadius = 3;
	lightColor = "0 0 0.5";
};
datablock ParticleData(chromePaintParticle)
{
	dragCoefficient = 0;
	windCoefficient = 0;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 420;
	lifetimeVarianceMS = 75;
	spinSpeed = 0;
	spinRandomMin = -50;
	spinRandomMax = 50;
	useInvAlpha = 1;
	animateTexture = 0;
	textureName = "~/data/particles/bubble";
	colors[0] = "1 1 1 1.00";
	colors[1] = "1 1 1 1.00";
	colors[2] = "0.5 0.5 0.5 0.000";
	sizes[0] = 0.1;
	sizes[1] = 0.5;
	sizes[2] = 1.2;
	times[0] = 0;
	times[1] = 0.9;
	times[2] = 1;
};
datablock ParticleEmitterData(chromePaintEmitter)
{
	ejectionPeriodMS = 10;
	periodVarianceMS = 0;
	ejectionVelocity = 20;
	velocityVariance = 0;
	ejectionOffset = 0;
	thetaMin = 0;
	thetaMax = 5;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = chromePaintParticle;
};
datablock ShapeBaseImageData(chromeSprayCanImage)
{
	shapeFile = "~/data/shapes/spraycan.dts";
	emap = 0;
	mountPoint = 0;
	offset = "0 0 0";
	eyeOffset = "0.7 1.0 -0.6";
	correctMuzzleVector = 0;
	className = "WeaponImage";
	Item = sprayCan;
	ammo = " ";
	Projectile = chromePaintProjectile;
	projectileType = Projectile;
	doColorShift = 1;
	colorShiftColor = "0.5 0.5 0.5 1.0";
	melee = 1;
	doRetraction = 0;
	armReady = 1;
	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0.5;
	stateTransitionOnTimeout[0] = "CapOff";
	stateTransitionOnTriggerDown[0] = "Fire";
	stateWaitForTimeout[0] = 0;
	stateSound[0] = sprayActivateSound;
	stateSequence[0] = "Shake";
	stateName[4] = "CapOff";
	stateSequence[4] = "capOff";
	stateTimeoutValue[4] = 0.2;
	stateTransitionOnTriggerDown[4] = "fire";
	stateWaitForTimeout[4] = 0;
	stateTransitionOnTimeout[4] = "Ready";
	stateName[1] = "Ready";
	stateTransitionOnTriggerDown[1] = "fire";
	stateAllowImageChange[1] = 1;
	stateName[2] = "Fire";
	stateScript[2] = "onFire";
	stateFire[2] = 1;
	stateAllowImageChange[2] = 1;
	stateTimeoutValue[2] = 0.04;
	stateTransitionOnTimeout[2] = "Fire";
	stateTransitionOnTriggerUp[2] = "StopFire";
	stateEmitter[2] = chromePaintEmitter;
	stateEmitterTime[2] = 0.06;
	stateEmitterNode[2] = "muzzleNode";
	stateSound[2] = sprayFireSound;
	stateSequence[2] = "Fire";
	stateName[3] = "StopFire";
	stateTransitionOnTimeout[3] = 0;
	stateSequence[3] = "StopFire";
	stateTransitionOnTimeout[3] = "Ready";
};
function chromePaintProjectile::OnCollision(%this, %obj, %col, %fade, %pos, %normal)
{
	if (%col.getClassName() $= "fxDTSBrick")
	{
		if (%col.colorFxID != 2)
		{
			%brickGroup = %col.getGroup();
			%client = %obj.client;
			if (!isObject(%client))
			{
				return;
			}
			if (%client.brickGroup != %brickGroup)
			{
				%trustLevel = %brickGroup.Trust[%client.getBLID()];
				if (%trustLevel < $TrustLevel::FXPaint)
				{
					commandToClient(%client, 'CenterPrint', %brickGroup.name @ " does not trust you enough to do that.", 1);
					return;
				}
			}
			if (isObject(%obj.client))
			{
				%obj.client.undoStack.push(%col TAB "COLORFX" TAB %col.colorFxID);
			}
			%col.setColorFX(2);
		}
	}
}

datablock ParticleData(glowPaintExplosionParticle)
{
	dragCoefficient = 2;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 1600;
	lifetimeVarianceMS = 400;
	textureName = "~/data/particles/star1";
	useInvAlpha = 0;
	spinSpeed = 100;
	spinRandomMin = -50;
	spinRandomMax = 50;
	colors[0] = "0.9 0.9 1 1.00";
	colors[1] = "1 1 1 1.00";
	colors[2] = "0.5 0.5 0.5 0.000";
	sizes[0] = 0.3;
	sizes[1] = 0.1;
	sizes[2] = 0.9;
	times[0] = 0;
	times[1] = 0.95;
	times[2] = 1;
};
datablock ParticleData(glowPaintDropletParticle)
{
	dragCoefficient = 0;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 500;
	lifetimeVarianceMS = 100;
	textureName = "~/data/particles/chunk";
	useInvAlpha = 0;
	spinSpeed = 100;
	spinRandomMin = -50;
	spinRandomMax = 50;
	colors[0] = "1 1 1 0.500";
	colors[1] = "0.000 0.317 0.745 0.000";
	sizes[0] = 0.1;
	sizes[1] = 0;
};
datablock ParticleEmitterData(glowPaintExplosionEmitter)
{
	lifetimeMS = 100;
	ejectionPeriodMS = 40;
	periodVarianceMS = 0;
	ejectionVelocity = 3.5;
	velocityVariance = 0;
	ejectionOffset = 0.2;
	thetaMin = 0;
	thetaMax = 180;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = "glowPaintExplosionParticle";
};
datablock ParticleEmitterData(glowPaintDropletEmitter)
{
	lifetimeMS = 100;
	ejectionPeriodMS = 40;
	periodVarianceMS = 0;
	ejectionVelocity = 5;
	velocityVariance = 0;
	ejectionOffset = 0;
	thetaMin = 0;
	thetaMax = 90;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = "glowPaintDropletParticle";
};
datablock ExplosionData(glowPaintExplosion)
{
	lifetimeMS = 150;
	emitter[0] = glowPaintExplosionEmitter;
	emitter[1] = glowPaintDropletEmitter;
	faceViewer = 0;
	shakeCamera = 0;
};
datablock ProjectileData(glowPaintProjectile)
{
	className = paintProjectile;
	Explosion = glowPaintExplosion;
	muzzleVelocity = 20;
	velInheritFactor = 0;
	armingDelay = 0;
	lifetime = 525;
	fadeDelay = 0;
	bounceElasticity = 0;
	bounceFriction = 0;
	isBallistic = 0;
	gravityMod = 0;
	hasLight = 0;
	lightRadius = 3;
	lightColor = "0 0 0.5";
};
datablock ParticleData(glowPaintParticle)
{
	dragCoefficient = 0;
	windCoefficient = 0;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 420;
	lifetimeVarianceMS = 175;
	spinSpeed = 0;
	spinRandomMin = -50;
	spinRandomMax = 50;
	useInvAlpha = 0;
	animateTexture = 0;
	textureName = "~/data/particles/bubble";
	colors[0] = "1 1 1 2.00";
	colors[1] = "1 1 1 2.00";
	colors[2] = "1 1 1 0.000";
	sizes[0] = 0.1;
	sizes[1] = 0.5;
	sizes[2] = 0;
	times[0] = 0;
	times[1] = 0.6;
	times[2] = 1;
};
datablock ParticleEmitterData(glowPaintEmitter)
{
	ejectionPeriodMS = 3;
	periodVarianceMS = 0;
	ejectionVelocity = 20;
	velocityVariance = 0;
	ejectionOffset = 0;
	thetaMin = 0;
	thetaMax = 3;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = glowPaintParticle;
};
datablock ShapeBaseImageData(glowSprayCanImage)
{
	shapeFile = "~/data/shapes/spraycan.dts";
	emap = 0;
	mountPoint = 0;
	offset = "0 0 0";
	eyeOffset = "0.7 1.0 -0.6";
	correctMuzzleVector = 0;
	className = "WeaponImage";
	Item = sprayCan;
	ammo = " ";
	Projectile = glowPaintProjectile;
	projectileType = Projectile;
	doColorShift = 1;
	colorShiftColor = "3 3 3 2.0";
	melee = 1;
	doRetraction = 0;
	armReady = 1;
	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0.5;
	stateTransitionOnTimeout[0] = "CapOff";
	stateTransitionOnTriggerDown[0] = "Fire";
	stateWaitForTimeout[0] = 0;
	stateSound[0] = sprayActivateSound;
	stateSequence[0] = "Shake";
	stateName[4] = "CapOff";
	stateSequence[4] = "capOff";
	stateTimeoutValue[4] = 0.2;
	stateTransitionOnTriggerDown[4] = "fire";
	stateWaitForTimeout[4] = 0;
	stateTransitionOnTimeout[4] = "Ready";
	stateName[1] = "Ready";
	stateTransitionOnTriggerDown[1] = "fire";
	stateAllowImageChange[1] = 1;
	stateName[2] = "Fire";
	stateScript[2] = "onFire";
	stateFire[2] = 1;
	stateAllowImageChange[2] = 1;
	stateTimeoutValue[2] = 0.04;
	stateTransitionOnTimeout[2] = "Fire";
	stateTransitionOnTriggerUp[2] = "StopFire";
	stateEmitter[2] = glowPaintEmitter;
	stateEmitterTime[2] = 0.06;
	stateEmitterNode[2] = "muzzleNode";
	stateSound[2] = sprayFireSound;
	stateSequence[2] = "Fire";
	stateName[3] = "StopFire";
	stateTransitionOnTimeout[3] = 0;
	stateSequence[3] = "StopFire";
	stateTransitionOnTimeout[3] = "Ready";
};
function glowPaintProjectile::OnCollision(%this, %obj, %col, %fade, %pos, %normal)
{
	if (%col.getClassName() $= "fxDTSBrick")
	{
		if (%col.colorFxID != 3)
		{
			%brickGroup = %col.getGroup();
			%client = %obj.client;
			if (!isObject(%client))
			{
				return;
			}
			if (%client.brickGroup != %brickGroup)
			{
				%trustLevel = %brickGroup.Trust[%client.getBLID()];
				if (%trustLevel < $TrustLevel::FXPaint)
				{
					commandToClient(%client, 'CenterPrint', %brickGroup.name @ " does not trust you enough to do that.", 1);
					return;
				}
			}
			if (isObject(%obj.client))
			{
				%obj.client.undoStack.push(%col TAB "COLORFX" TAB %col.colorFxID);
			}
			%col.setColorFX(3);
		}
	}
}

datablock ParticleData(blinkPaintExplosionParticle)
{
	dragCoefficient = 2;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 300;
	lifetimeVarianceMS = 290;
	textureName = "~/data/particles/bubble";
	useInvAlpha = 1;
	spinSpeed = 100;
	spinRandomMin = -50;
	spinRandomMax = 50;
	colors[0] = "1 1 0 1.00";
	colors[1] = "1 1 0 1.00";
	colors[2] = "0 0 0 1.000";
	sizes[0] = 0.2;
	sizes[1] = 0.7;
	sizes[2] = 0;
	times[0] = 0;
	times[1] = 0.8;
	times[2] = 1;
};
datablock ParticleData(blinkPaintDropletParticle)
{
	dragCoefficient = 0;
	gravityCoefficient = 1;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 800;
	lifetimeVarianceMS = 200;
	textureName = "~/data/particles/chunk";
	useInvAlpha = 1;
	spinSpeed = 100;
	spinRandomMin = -50;
	spinRandomMax = 50;
	colors[0] = "0 0 0 1";
	colors[1] = "0 0 0 1";
	sizes[0] = 0.1;
	sizes[1] = 0;
};
datablock ParticleEmitterData(blinkPaintExplosionEmitter)
{
	lifetimeMS = 100;
	ejectionPeriodMS = 40;
	periodVarianceMS = 0;
	ejectionVelocity = 1.5;
	velocityVariance = 0;
	ejectionOffset = 0.2;
	thetaMin = 0;
	thetaMax = 120;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = "blinkPaintExplosionParticle";
};
datablock ParticleEmitterData(blinkPaintDropletEmitter)
{
	lifetimeMS = 100;
	ejectionPeriodMS = 40;
	periodVarianceMS = 0;
	ejectionVelocity = 5;
	velocityVariance = 0;
	ejectionOffset = 0;
	thetaMin = 0;
	thetaMax = 90;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = "blinkPaintDropletParticle";
};
datablock ExplosionData(blinkPaintExplosion)
{
	lifetimeMS = 150;
	emitter[0] = blinkPaintExplosionEmitter;
	emitter[1] = blinkPaintDropletEmitter;
	faceViewer = 0;
	shakeCamera = 0;
};
datablock ProjectileData(blinkPaintProjectile)
{
	className = paintProjectile;
	Explosion = blinkPaintExplosion;
	muzzleVelocity = 20;
	velInheritFactor = 0;
	armingDelay = 0;
	lifetime = 525;
	fadeDelay = 0;
	bounceElasticity = 0;
	bounceFriction = 0;
	isBallistic = 0;
	gravityMod = 0;
	hasLight = 0;
	lightRadius = 3;
	lightColor = "0 0 0.5";
};
datablock ParticleData(blinkPaintParticle)
{
	dragCoefficient = 0;
	windCoefficient = 0;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 420;
	lifetimeVarianceMS = 75;
	spinSpeed = 0;
	spinRandomMin = -50;
	spinRandomMax = 50;
	useInvAlpha = 1;
	animateTexture = 0;
	textureName = "~/data/particles/bubble";
	colors[0] = "0 0 0 1.0";
	colors[1] = "1 1 0 1.0";
	colors[2] = "0.5 0.5 0 0.5";
	colors[3] = "1 1 0 0.0";
	sizes[0] = 0.1;
	sizes[1] = 0.5;
	sizes[2] = 0.2;
	sizes[3] = 1.5;
	times[0] = 0;
	times[1] = 0.5;
	times[2] = 0.9;
	times[3] = 1;
};
datablock ParticleEmitterData(blinkPaintEmitter)
{
	ejectionPeriodMS = 10;
	periodVarianceMS = 0;
	ejectionVelocity = 20;
	velocityVariance = 0;
	ejectionOffset = 0;
	thetaMin = 0;
	thetaMax = 5;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = blinkPaintParticle;
};
datablock ShapeBaseImageData(blinkSprayCanImage)
{
	shapeFile = "~/data/shapes/spraycan.dts";
	emap = 0;
	mountPoint = 0;
	offset = "0 0 0";
	eyeOffset = "0.7 1.0 -0.6";
	correctMuzzleVector = 0;
	className = "WeaponImage";
	Item = sprayCan;
	ammo = " ";
	Projectile = blinkPaintProjectile;
	projectileType = Projectile;
	doColorShift = 1;
	colorShiftColor = "1.0 1.0 0.0 1.0";
	melee = 1;
	doRetraction = 0;
	armReady = 1;
	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0.5;
	stateTransitionOnTimeout[0] = "CapOff";
	stateTransitionOnTriggerDown[0] = "Fire";
	stateWaitForTimeout[0] = 0;
	stateSound[0] = sprayActivateSound;
	stateSequence[0] = "Shake";
	stateName[4] = "CapOff";
	stateSequence[4] = "capOff";
	stateTimeoutValue[4] = 0.2;
	stateTransitionOnTriggerDown[4] = "fire";
	stateWaitForTimeout[4] = 0;
	stateTransitionOnTimeout[4] = "Ready";
	stateName[1] = "Ready";
	stateTransitionOnTriggerDown[1] = "fire";
	stateAllowImageChange[1] = 1;
	stateName[2] = "Fire";
	stateScript[2] = "onFire";
	stateFire[2] = 1;
	stateAllowImageChange[2] = 1;
	stateTimeoutValue[2] = 0.04;
	stateTransitionOnTimeout[2] = "Fire";
	stateTransitionOnTriggerUp[2] = "StopFire";
	stateEmitter[2] = blinkPaintEmitter;
	stateEmitterTime[2] = 0.06;
	stateEmitterNode[2] = "muzzleNode";
	stateSound[2] = sprayFireSound;
	stateSequence[2] = "Fire";
	stateName[3] = "StopFire";
	stateTransitionOnTimeout[3] = 0;
	stateSequence[3] = "StopFire";
	stateTransitionOnTimeout[3] = "Ready";
};
function blinkPaintProjectile::OnCollision(%this, %obj, %col, %fade, %pos, %normal)
{
	if (%col.getClassName() $= "fxDTSBrick")
	{
		if (%col.colorFxID != 4)
		{
			%brickGroup = %col.getGroup();
			%client = %obj.client;
			if (!isObject(%client))
			{
				return;
			}
			if (%client.brickGroup != %brickGroup)
			{
				%trustLevel = %brickGroup.Trust[%client.getBLID()];
				if (%trustLevel < $TrustLevel::FXPaint)
				{
					commandToClient(%client, 'CenterPrint', %brickGroup.name @ " does not trust you enough to do that.", 1);
					return;
				}
			}
			if (isObject(%obj.client))
			{
				%obj.client.undoStack.push(%col TAB "COLORFX" TAB %col.colorFxID);
			}
			%col.setColorFX(4);
		}
	}
}

datablock ParticleData(swirlPaintExplosionParticle)
{
	dragCoefficient = 2;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 600;
	lifetimeVarianceMS = 400;
	textureName = "~/data/particles/bubble";
	useInvAlpha = 0;
	spinSpeed = 100;
	spinRandomMin = -5000;
	spinRandomMax = -1500;
	colors[0] = "0.6 0.6 1 1.00";
	colors[1] = "0 0 1 1.00";
	colors[2] = "0 0 0.5 0.000";
	sizes[0] = 0.4;
	sizes[1] = 0.4;
	sizes[2] = 0.9;
	times[0] = 0;
	times[1] = 0.9;
	times[2] = 1;
};
datablock ParticleData(swirlPaintDropletParticle)
{
	dragCoefficient = 0;
	gravityCoefficient = 1;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 800;
	lifetimeVarianceMS = 200;
	textureName = "~/data/particles/chunk";
	useInvAlpha = 0;
	spinSpeed = 100;
	spinRandomMin = -50;
	spinRandomMax = 50;
	colors[0] = "1 1 1 0.500";
	colors[1] = "0.000 0.317 0.745 0.000";
	sizes[0] = 0.1;
	sizes[1] = 0;
};
datablock ParticleEmitterData(swirlPaintExplosionEmitter)
{
	lifetimeMS = 100;
	ejectionPeriodMS = 40;
	periodVarianceMS = 0;
	ejectionVelocity = 3.5;
	velocityVariance = 0;
	ejectionOffset = 0;
	thetaMin = 35;
	thetaMax = 85;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = "swirlPaintExplosionParticle";
};
datablock ParticleEmitterData(swirlPaintDropletEmitter)
{
	lifetimeMS = 100;
	ejectionPeriodMS = 40;
	periodVarianceMS = 0;
	ejectionVelocity = 5;
	velocityVariance = 0;
	ejectionOffset = 0;
	thetaMin = 0;
	thetaMax = 90;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = "swirlPaintDropletParticle";
};
datablock ExplosionData(swirlPaintExplosion)
{
	lifetimeMS = 150;
	emitter[0] = swirlPaintExplosionEmitter;
	emitter[1] = swirlPaintDropletEmitter;
	faceViewer = 0;
	shakeCamera = 0;
};
datablock ProjectileData(swirlPaintProjectile)
{
	className = paintProjectile;
	Explosion = swirlPaintExplosion;
	muzzleVelocity = 20;
	velInheritFactor = 0;
	armingDelay = 0;
	lifetime = 525;
	fadeDelay = 0;
	bounceElasticity = 0;
	bounceFriction = 0;
	isBallistic = 0;
	gravityMod = 0;
	hasLight = 0;
	lightRadius = 3;
	lightColor = "0 0 0.5";
};
datablock ParticleData(swirlPaintParticle)
{
	dragCoefficient = 0;
	windCoefficient = 0;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 420;
	lifetimeVarianceMS = 175;
	spinSpeed = 0;
	spinRandomMin = -5000;
	spinRandomMax = -1500;
	useInvAlpha = 0;
	animateTexture = 0;
	textureName = "~/data/particles/bubble";
	colors[0] = "0.6 0.6 1 1.00";
	colors[1] = "0 0 1 1.00";
	colors[2] = "0 0 0.5 0.000";
	sizes[0] = 0.1;
	sizes[1] = 0.7;
	sizes[2] = 2.2;
	times[0] = 0;
	times[1] = 0.9;
	times[2] = 1;
};
datablock ParticleEmitterData(swirlPaintEmitter)
{
	ejectionPeriodMS = 5;
	periodVarianceMS = 0;
	ejectionVelocity = 20;
	velocityVariance = 0;
	ejectionOffset = 0;
	thetaMin = 0;
	thetaMax = 5;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = swirlPaintParticle;
};
datablock ShapeBaseImageData(swirlSprayCanImage)
{
	shapeFile = "~/data/shapes/spraycan.dts";
	emap = 0;
	mountPoint = 0;
	offset = "0 0 0";
	eyeOffset = "0.7 1.0 -0.6";
	correctMuzzleVector = 0;
	className = "WeaponImage";
	Item = sprayCan;
	ammo = " ";
	Projectile = swirlPaintProjectile;
	projectileType = Projectile;
	doColorShift = 1;
	colorShiftColor = "0.0 0.0 0.9098 1.0";
	melee = 1;
	doRetraction = 0;
	armReady = 1;
	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0.5;
	stateTransitionOnTimeout[0] = "CapOff";
	stateTransitionOnTriggerDown[0] = "Fire";
	stateWaitForTimeout[0] = 0;
	stateSound[0] = sprayActivateSound;
	stateSequence[0] = "Shake";
	stateName[4] = "CapOff";
	stateSequence[4] = "capOff";
	stateTimeoutValue[4] = 0.2;
	stateTransitionOnTriggerDown[4] = "fire";
	stateWaitForTimeout[4] = 0;
	stateTransitionOnTimeout[4] = "Ready";
	stateName[1] = "Ready";
	stateTransitionOnTriggerDown[1] = "fire";
	stateAllowImageChange[1] = 1;
	stateName[2] = "Fire";
	stateScript[2] = "onFire";
	stateFire[2] = 1;
	stateAllowImageChange[2] = 1;
	stateTimeoutValue[2] = 0.04;
	stateTransitionOnTimeout[2] = "Fire";
	stateTransitionOnTriggerUp[2] = "StopFire";
	stateEmitter[2] = swirlPaintEmitter;
	stateEmitterTime[2] = 0.06;
	stateEmitterNode[2] = "muzzleNode";
	stateSound[2] = sprayFireSound;
	stateSequence[2] = "Fire";
	stateName[3] = "StopFire";
	stateTransitionOnTimeout[3] = 0;
	stateSequence[3] = "StopFire";
	stateTransitionOnTimeout[3] = "Ready";
};
function swirlPaintProjectile::OnCollision(%this, %obj, %col, %fade, %pos, %normal)
{
	if (%col.getClassName() $= "fxDTSBrick")
	{
		if (%col.colorFxID != 5)
		{
			%brickGroup = %col.getGroup();
			%client = %obj.client;
			if (!isObject(%client))
			{
				return;
			}
			if (%client.brickGroup != %brickGroup)
			{
				%trustLevel = %brickGroup.Trust[%client.getBLID()];
				if (%trustLevel < $TrustLevel::FXPaint)
				{
					commandToClient(%client, 'CenterPrint', %brickGroup.name @ " does not trust you enough to do that.", 1);
					return;
				}
			}
			if (isObject(%obj.client))
			{
				%obj.client.undoStack.push(%col TAB "COLORFX" TAB %col.colorFxID);
			}
			%col.setColorFX(5);
		}
	}
}

datablock ParticleData(rainbowPaintExplosionParticle)
{
	dragCoefficient = 3;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 600;
	lifetimeVarianceMS = 400;
	textureName = "~/data/particles/chunk";
	useInvAlpha = 0;
	spinSpeed = 100;
	spinRandomMin = -500;
	spinRandomMax = 500;
	colors[0] = "1 0 0 0.750";
	colors[1] = "0 1 0 0.750";
	colors[2] = "0 0 1 0.750";
	colors[3] = "0 0 1 0.00";
	sizes[0] = 0.3;
	sizes[1] = 0.3;
	sizes[2] = 0.3;
	sizes[3] = 0.3;
	times[0] = 0;
	times[1] = 0.45;
	times[2] = 0.9;
	times[3] = 1;
};
datablock ParticleData(rainbowPaintDropletParticle)
{
	dragCoefficient = 0;
	gravityCoefficient = 1;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 800;
	lifetimeVarianceMS = 200;
	textureName = "~/data/particles/chunk";
	useInvAlpha = 0;
	spinSpeed = 100;
	spinRandomMin = -50;
	spinRandomMax = 50;
	colors[0] = "1 0 0 0.750";
	colors[1] = "0 1 0 0.750";
	colors[2] = "0 0 1 0.750";
	colors[3] = "0 0 1 0.00";
	sizes[0] = 0.1;
	sizes[1] = 0.1;
	sizes[2] = 0.1;
	sizes[3] = 0;
	times[0] = 0;
	times[1] = 0.45;
	times[2] = 0.9;
	times[3] = 1;
};
datablock ParticleEmitterData(rainbowPaintExplosionEmitter)
{
	lifetimeMS = 100;
	ejectionPeriodMS = 40;
	periodVarianceMS = 0;
	ejectionVelocity = 1.5;
	velocityVariance = 0;
	ejectionOffset = 0.4;
	thetaMin = 35;
	thetaMax = 85;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = "rainbowPaintExplosionParticle";
};
datablock ParticleEmitterData(rainbowPaintDropletEmitter)
{
	lifetimeMS = 100;
	ejectionPeriodMS = 40;
	periodVarianceMS = 0;
	ejectionVelocity = 5;
	velocityVariance = 0;
	ejectionOffset = 0;
	thetaMin = 0;
	thetaMax = 90;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = "rainbowPaintDropletParticle";
};
datablock ExplosionData(rainbowPaintExplosion)
{
	lifetimeMS = 100;
	emitter[0] = rainbowPaintExplosionEmitter;
	emitter[1] = rainbowPaintDropletEmitter;
	faceViewer = 0;
	shakeCamera = 0;
};
datablock ProjectileData(rainbowPaintProjectile)
{
	className = paintProjectile;
	Explosion = rainbowPaintExplosion;
	muzzleVelocity = 20;
	velInheritFactor = 0;
	armingDelay = 0;
	lifetime = 525;
	fadeDelay = 0;
	bounceElasticity = 0;
	bounceFriction = 0;
	isBallistic = 0;
	gravityMod = 0;
	hasLight = 0;
	lightRadius = 3;
	lightColor = "0 0 0.5";
};
datablock ParticleData(rainbowPaintParticle)
{
	dragCoefficient = 0;
	windCoefficient = 0;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 420;
	lifetimeVarianceMS = 75;
	spinSpeed = 0;
	spinRandomMin = -550;
	spinRandomMax = 550;
	useInvAlpha = 0;
	animateTexture = 0;
	textureName = "~/data/particles/chunk";
	colors[0] = "1 0 0 0.750";
	colors[1] = "1 1 0 0.750";
	colors[2] = "0 1 0 0.750";
	colors[3] = "0 0 1 0.20";
	sizes[0] = 0.1;
	sizes[1] = 0.5;
	sizes[2] = 0.5;
	sizes[3] = 0.5;
	times[0] = 0;
	times[1] = 0.33;
	times[2] = 0.5;
	times[3] = 1;
};
datablock ParticleEmitterData(rainbowPaintEmitter)
{
	ejectionPeriodMS = 5;
	periodVarianceMS = 0;
	ejectionVelocity = 20;
	velocityVariance = 0;
	ejectionOffset = 0;
	thetaMin = 0;
	thetaMax = 4;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = rainbowPaintParticle;
};
datablock ShapeBaseImageData(rainbowSprayCanImage)
{
	shapeFile = "~/data/shapes/spraycan.dts";
	emap = 0;
	mountPoint = 0;
	offset = "0 0 0";
	eyeOffset = "0.7 1.0 -0.6";
	correctMuzzleVector = 0;
	className = "WeaponImage";
	Item = sprayCan;
	ammo = " ";
	Projectile = rainbowPaintProjectile;
	projectileType = Projectile;
	doColorShift = 1;
	colorShiftColor = "1 0 2 2.0";
	melee = 1;
	doRetraction = 0;
	armReady = 1;
	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0.5;
	stateTransitionOnTimeout[0] = "CapOff";
	stateTransitionOnTriggerDown[0] = "Fire";
	stateWaitForTimeout[0] = 0;
	stateSound[0] = sprayActivateSound;
	stateSequence[0] = "Shake";
	stateName[4] = "CapOff";
	stateSequence[4] = "capOff";
	stateTimeoutValue[4] = 0.2;
	stateTransitionOnTriggerDown[4] = "fire";
	stateWaitForTimeout[4] = 0;
	stateTransitionOnTimeout[4] = "Ready";
	stateName[1] = "Ready";
	stateTransitionOnTriggerDown[1] = "fire";
	stateAllowImageChange[1] = 1;
	stateName[2] = "Fire";
	stateScript[2] = "onFire";
	stateFire[2] = 1;
	stateAllowImageChange[2] = 1;
	stateTimeoutValue[2] = 0.04;
	stateTransitionOnTimeout[2] = "Fire";
	stateTransitionOnTriggerUp[2] = "StopFire";
	stateEmitter[2] = rainbowPaintEmitter;
	stateEmitterTime[2] = 0.06;
	stateEmitterNode[2] = "muzzleNode";
	stateSound[2] = sprayFireSound;
	stateSequence[2] = "Fire";
	stateName[3] = "StopFire";
	stateTransitionOnTimeout[3] = 0;
	stateSequence[3] = "StopFire";
	stateTransitionOnTimeout[3] = "Ready";
};
function rainbowPaintProjectile::OnCollision(%this, %obj, %col, %fade, %pos, %normal)
{
	if (%col.getClassName() $= "fxDTSBrick")
	{
		if (%col.colorFxID != 6)
		{
			%brickGroup = %col.getGroup();
			%client = %obj.client;
			if (!isObject(%client))
			{
				return;
			}
			if (%client.brickGroup != %brickGroup)
			{
				%trustLevel = %brickGroup.Trust[%client.getBLID()];
				if (%trustLevel < $TrustLevel::FXPaint)
				{
					commandToClient(%client, 'CenterPrint', %brickGroup.name @ " does not trust you enough to do that.", 1);
					return;
				}
			}
			if (isObject(%obj.client))
			{
				%obj.client.undoStack.push(%col TAB "COLORFX" TAB %col.colorFxID);
			}
			%col.setColorFX(6);
		}
	}
}

datablock ParticleData(stablePaintExplosionParticle)
{
	dragCoefficient = 15;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 600;
	lifetimeVarianceMS = 400;
	textureName = "~/data/particles/dot";
	useInvAlpha = 1;
	spinSpeed = 100;
	spinRandomMin = -50;
	spinRandomMax = 50;
	colors[0] = "0.3 0.3 0.3 1.00";
	colors[1] = "0.3 0.3 0.3 1.00";
	colors[2] = "0.3 0.3 0.3 0.000";
	sizes[0] = 0.2;
	sizes[1] = 0.2;
	sizes[2] = 0;
	times[0] = 0;
	times[1] = 0.8;
	times[2] = 1;
};
datablock ParticleData(stablePaintDropletParticle)
{
	dragCoefficient = 5;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 800;
	lifetimeVarianceMS = 200;
	textureName = "~/data/particles/dot";
	useInvAlpha = 1;
	spinSpeed = 100;
	spinRandomMin = -50;
	spinRandomMax = 50;
	colors[0] = "0.3 0.3 0.3 0.500";
	colors[1] = "0.000 0.0 0.0 0.000";
	sizes[0] = 0.1;
	sizes[1] = 0;
};
datablock ParticleEmitterData(stablePaintExplosionEmitter)
{
	lifetimeMS = 100;
	ejectionPeriodMS = 40;
	periodVarianceMS = 0;
	ejectionVelocity = 3.5;
	velocityVariance = 0;
	ejectionOffset = 0;
	thetaMin = 35;
	thetaMax = 85;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = "stablePaintExplosionParticle";
};
datablock ParticleEmitterData(stablePaintDropletEmitter)
{
	lifetimeMS = 100;
	ejectionPeriodMS = 40;
	periodVarianceMS = 0;
	ejectionVelocity = 5;
	velocityVariance = 0;
	ejectionOffset = 0;
	thetaMin = 0;
	thetaMax = 90;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = "stablePaintDropletParticle";
};
datablock ExplosionData(stablePaintExplosion)
{
	lifetimeMS = 150;
	emitter[0] = stablePaintExplosionEmitter;
	emitter[1] = stablePaintDropletEmitter;
	faceViewer = 0;
	shakeCamera = 0;
};
datablock ProjectileData(stablePaintProjectile)
{
	className = paintProjectile;
	Explosion = stablePaintExplosion;
	muzzleVelocity = 20;
	velInheritFactor = 0;
	armingDelay = 0;
	lifetime = 525;
	fadeDelay = 0;
	bounceElasticity = 0;
	bounceFriction = 0;
	isBallistic = 0;
	gravityMod = 0;
	hasLight = 0;
	lightRadius = 3;
	lightColor = "0 0 0.5";
};
datablock ParticleData(stablePaintParticle)
{
	dragCoefficient = 0;
	windCoefficient = 0;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 420;
	lifetimeVarianceMS = 75;
	spinSpeed = 0;
	spinRandomMin = -50;
	spinRandomMax = 50;
	useInvAlpha = 1;
	animateTexture = 0;
	textureName = "~/data/particles/dot";
	colors[0] = "0.3 0.3 0.3 0.50";
	colors[1] = "0.3 0.3 0.3 0.50";
	colors[2] = "0.3 0.3 0.3 0.000";
	sizes[0] = 0.05;
	sizes[1] = 0.5;
	sizes[2] = 0.5;
	times[0] = 0;
	times[1] = 0.9;
	times[2] = 1;
};
datablock ParticleEmitterData(stablePaintEmitter)
{
	ejectionPeriodMS = 1;
	periodVarianceMS = 0;
	ejectionVelocity = 20;
	velocityVariance = 0;
	ejectionOffset = 0;
	thetaMin = 0;
	thetaMax = 0;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = stablePaintParticle;
};
datablock ShapeBaseImageData(stableSprayCanImage)
{
	shapeFile = "~/data/shapes/spraycan.dts";
	emap = 0;
	mountPoint = 0;
	offset = "0 0 0";
	eyeOffset = "0.7 1.0 -0.6";
	correctMuzzleVector = 0;
	className = "WeaponImage";
	Item = sprayCan;
	ammo = " ";
	Projectile = stablePaintProjectile;
	projectileType = Projectile;
	doColorShift = 1;
	colorShiftColor = "0 0 0 1.0";
	melee = 1;
	doRetraction = 0;
	armReady = 1;
	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0.5;
	stateTransitionOnTimeout[0] = "CapOff";
	stateTransitionOnTriggerDown[0] = "Fire";
	stateWaitForTimeout[0] = 0;
	stateSound[0] = sprayActivateSound;
	stateSequence[0] = "Shake";
	stateName[4] = "CapOff";
	stateSequence[4] = "capOff";
	stateTimeoutValue[4] = 0.2;
	stateTransitionOnTriggerDown[4] = "fire";
	stateWaitForTimeout[4] = 0;
	stateTransitionOnTimeout[4] = "Ready";
	stateName[1] = "Ready";
	stateTransitionOnTriggerDown[1] = "fire";
	stateAllowImageChange[1] = 1;
	stateName[2] = "Fire";
	stateScript[2] = "onFire";
	stateFire[2] = 1;
	stateAllowImageChange[2] = 1;
	stateTimeoutValue[2] = 0.04;
	stateTransitionOnTimeout[2] = "Fire";
	stateTransitionOnTriggerUp[2] = "StopFire";
	stateEmitter[2] = stablePaintEmitter;
	stateEmitterTime[2] = 0.07;
	stateEmitterNode[2] = "muzzleNode";
	stateSound[2] = sprayFireSound;
	stateSequence[2] = "Fire";
	stateName[3] = "StopFire";
	stateTransitionOnTimeout[3] = 0;
	stateSequence[3] = "StopFire";
	stateTransitionOnTimeout[3] = "Ready";
};
function stablePaintProjectile::OnCollision(%this, %obj, %col, %fade, %pos, %normal)
{
	if (%col.getClassName() $= "fxDTSBrick")
	{
		if (%col.shapeFxID != 0)
		{
			%brickGroup = %col.getGroup();
			%client = %obj.client;
			if (!isObject(%client))
			{
				return;
			}
			if (%client.brickGroup != %brickGroup)
			{
				%trustLevel = %brickGroup.Trust[%client.getBLID()];
				if (%trustLevel < $TrustLevel::FXPaint)
				{
					commandToClient(%client, 'CenterPrint', %brickGroup.name @ " does not trust you enough to do that.", 1);
					return;
				}
			}
			if (isObject(%obj.client))
			{
				%obj.client.undoStack.push(%col TAB "SHAPEFX" TAB %col.shapeFxID);
			}
			%col.setShapeFX(0);
		}
	}
}

datablock ParticleData(jelloPaintExplosionParticle)
{
	dragCoefficient = 0;
	gravityCoefficient = 0.8;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 600;
	lifetimeVarianceMS = 400;
	textureName = "~/data/particles/chunk";
	useInvAlpha = 0;
	spinSpeed = 100;
	spinRandomMin = -500;
	spinRandomMax = 500;
	colors[0] = "1 0 0 0.50";
	colors[1] = "1 0 0 0.50";
	colors[2] = "1 0 0 0.00";
	sizes[0] = 0.3;
	sizes[1] = 0.3;
	sizes[2] = 0.3;
	times[0] = 0;
	times[1] = 0.5;
	times[2] = 1;
};
datablock ParticleData(jelloPaintDropletParticle)
{
	dragCoefficient = 0;
	gravityCoefficient = 1;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 800;
	lifetimeVarianceMS = 200;
	textureName = "~/data/particles/chunk";
	useInvAlpha = 0;
	spinSpeed = 100;
	spinRandomMin = -50;
	spinRandomMax = 50;
	colors[0] = "1 0 0 1.0";
	colors[1] = "1 0 0 1.0";
	sizes[0] = 0.1;
	sizes[1] = 0;
};
datablock ParticleEmitterData(jelloPaintExplosionEmitter)
{
	lifetimeMS = 100;
	ejectionPeriodMS = 40;
	periodVarianceMS = 0;
	ejectionVelocity = 0.5;
	velocityVariance = 0;
	ejectionOffset = 0.4;
	thetaMin = 35;
	thetaMax = 85;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = "jelloPaintExplosionParticle";
};
datablock ParticleEmitterData(jelloPaintDropletEmitter)
{
	lifetimeMS = 100;
	ejectionPeriodMS = 40;
	periodVarianceMS = 0;
	ejectionVelocity = 5;
	velocityVariance = 0;
	ejectionOffset = 0;
	thetaMin = 0;
	thetaMax = 90;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = "jelloPaintDropletParticle";
};
datablock ExplosionData(jelloPaintExplosion)
{
	lifetimeMS = 100;
	emitter[0] = jelloPaintExplosionEmitter;
	emitter[1] = jelloPaintDropletEmitter;
	faceViewer = 0;
	shakeCamera = 0;
};
datablock ProjectileData(jelloPaintProjectile)
{
	className = paintProjectile;
	Explosion = jelloPaintExplosion;
	muzzleVelocity = 20;
	velInheritFactor = 0;
	armingDelay = 0;
	lifetime = 525;
	fadeDelay = 0;
	bounceElasticity = 0;
	bounceFriction = 0;
	isBallistic = 0;
	gravityMod = 0;
	hasLight = 0;
	lightRadius = 3;
	lightColor = "0 0 0.5";
};
datablock ParticleData(jelloPaintParticle)
{
	dragCoefficient = 0;
	windCoefficient = 0;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 420;
	lifetimeVarianceMS = 75;
	spinSpeed = 0;
	spinRandomMin = -550;
	spinRandomMax = 550;
	useInvAlpha = 0;
	animateTexture = 0;
	textureName = "~/data/particles/chunk";
	colors[0] = "0.5 0 0 0.750";
	colors[1] = "0.5 0 0 0.750";
	colors[2] = "1 0 0 0.000";
	sizes[0] = 0.1;
	sizes[1] = 0.5;
	sizes[2] = 0.5;
	times[0] = 0;
	times[1] = 0.3;
	times[2] = 1;
};
datablock ParticleEmitterData(jelloPaintEmitter)
{
	ejectionPeriodMS = 10;
	periodVarianceMS = 0;
	ejectionVelocity = 20;
	velocityVariance = 0;
	ejectionOffset = 0;
	thetaMin = 0;
	thetaMax = 5;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = jelloPaintParticle;
};
datablock ShapeBaseImageData(jelloSprayCanImage)
{
	shapeFile = "~/data/shapes/transspraycan.dts";
	emap = 0;
	mountPoint = 0;
	offset = "0 0 0";
	eyeOffset = "0.7 1.0 -0.6";
	correctMuzzleVector = 0;
	className = "WeaponImage";
	Item = sprayCan;
	ammo = " ";
	Projectile = jelloPaintProjectile;
	projectileType = Projectile;
	doColorShift = 1;
	colorShiftColor = "0.5 0 0 0.7";
	melee = 1;
	doRetraction = 0;
	armReady = 1;
	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0.5;
	stateTransitionOnTimeout[0] = "CapOff";
	stateTransitionOnTriggerDown[0] = "Fire";
	stateWaitForTimeout[0] = 0;
	stateSound[0] = sprayActivateSound;
	stateSequence[0] = "Shake";
	stateName[4] = "CapOff";
	stateSequence[4] = "capOff";
	stateTimeoutValue[4] = 0.2;
	stateTransitionOnTriggerDown[4] = "fire";
	stateWaitForTimeout[4] = 0;
	stateTransitionOnTimeout[4] = "Ready";
	stateName[1] = "Ready";
	stateTransitionOnTriggerDown[1] = "fire";
	stateAllowImageChange[1] = 1;
	stateName[2] = "Fire";
	stateScript[2] = "onFire";
	stateFire[2] = 1;
	stateAllowImageChange[2] = 1;
	stateTimeoutValue[2] = 0.04;
	stateTransitionOnTimeout[2] = "Fire";
	stateTransitionOnTriggerUp[2] = "StopFire";
	stateEmitter[2] = jelloPaintEmitter;
	stateEmitterTime[2] = 0.06;
	stateEmitterNode[2] = "muzzleNode";
	stateSound[2] = sprayFireSound;
	stateSequence[2] = "Fire";
	stateName[3] = "StopFire";
	stateTransitionOnTimeout[3] = 0;
	stateSequence[3] = "StopFire";
	stateTransitionOnTimeout[3] = "Ready";
};
function jelloPaintProjectile::OnCollision(%this, %obj, %col, %fade, %pos, %normal)
{
	if (%col.getClassName() $= "fxDTSBrick")
	{
		if (%col.shapeFxID != 1)
		{
			%brickGroup = %col.getGroup();
			%client = %obj.client;
			if (!isObject(%client))
			{
				return;
			}
			if (%client.brickGroup != %brickGroup)
			{
				%trustLevel = %brickGroup.Trust[%client.getBLID()];
				if (%trustLevel < $TrustLevel::FXPaint)
				{
					commandToClient(%client, 'CenterPrint', %brickGroup.name @ " does not trust you enough to do that.", 1);
					return;
				}
			}
			if (isObject(%obj.client))
			{
				%obj.client.undoStack.push(%col TAB "SHAPEFX" TAB %col.shapeFxID);
			}
			%col.setShapeFX(1);
		}
	}
}

datablock ParticleData(brickTrailParticle)
{
	dragCoefficient = 3;
	windCoefficient = 0;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 250;
	lifetimeVarianceMS = 0;
	spinSpeed = 10;
	spinRandomMin = -50;
	spinRandomMax = 50;
	useInvAlpha = 0;
	animateTexture = 0;
	textureName = "~/data/particles/dot";
	colors[0] = "0.2 0.2 1 0.5";
	colors[1] = "0 0 1 0.8";
	colors[2] = "0.2 0.2 1 0.0";
	sizes[0] = 0;
	sizes[1] = 0.3;
	sizes[2] = 0.01;
	times[0] = 0;
	times[1] = 0.3;
	times[2] = 1;
};
datablock ParticleEmitterData(brickTrailEmitter)
{
	ejectionPeriodMS = 1;
	periodVarianceMS = 0;
	ejectionVelocity = 60;
	velocityVariance = 0;
	ejectionOffset = 0;
	thetaMin = 0;
	thetaMax = 0;
	particles = brickTrailParticle;
};
datablock ParticleData(brickDeployExplosionParticle)
{
	dragCoefficient = 5;
	gravityCoefficient = 0.1;
	inheritedVelFactor = 0.2;
	constantAcceleration = 0;
	lifetimeMS = 500;
	lifetimeVarianceMS = 300;
	textureName = "~/data/particles/chunk";
	spinSpeed = 100;
	spinRandomMin = -150;
	spinRandomMax = 150;
	useInvAlpha = 0;
	colors[0] = "0.2 0.2 1.0 1.0";
	colors[1] = "0.0 0.0 1.0 0.0";
	sizes[0] = 0.4;
	sizes[1] = 0;
};
datablock ParticleEmitterData(brickDeployExplosionEmitter)
{
	lifetimeMS = 50;
	ejectionPeriodMS = 7;
	periodVarianceMS = 0;
	ejectionVelocity = 5;
	velocityVariance = 1;
	ejectionOffset = 0.2;
	thetaMin = 0;
	thetaMax = 90;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = "brickDeployExplosionParticle";
};
datablock ExplosionData(brickDeployExplosion)
{
	lifetimeMS = 300;
	particleEmitter = brickDeployExplosionEmitter;
	faceViewer = 1;
	explosionScale = "1 1 1";
	shakeCamera = 0;
	camShakeFreq = "20.0 22.0 20.0";
	camShakeAmp = "1.0 1.0 1.0";
	camShakeDuration = 0.5;
	camShakeRadius = 10;
	lightStartRadius = 0;
	lightEndRadius = 3;
	lightStartColor = "1 1 1";
	lightEndColor = "0 0 0";
};
datablock ProjectileData(brickDeployProjectile)
{
	directDamage = 0;
	radiusDamage = 0;
	damageRadius = 0;
	Explosion = brickDeployExplosion;
	muzzleVelocity = 60;
	velInheritFactor = 0;
	armingDelay = 0;
	lifetime = 250;
	fadeDelay = 70;
	bounceElasticity = 0;
	bounceFriction = 0;
	isBallistic = 0;
	gravityMod = 0;
	collideWithPlayers = 0;
	hasLight = 0;
	lightRadius = 3;
	lightColor = "0 0 0.5";
};
function getAngleIDFromPlayer(%player)
{
	%forwardVec = %player.getForwardVector();
	%forwardX = getWord(%forwardVec, 0);
	%forwardY = getWord(%forwardVec, 1);
	if (%forwardX > 0)
	{
		if (%forwardX > mAbs(%forwardY))
		{
			return 0;
		}
		else if (%forwardY > 0)
		{
			return 1;
		}
		else
		{
			return 3;
		}
	}
	else if (mAbs(%forwardX) > mAbs(%forwardY))
	{
		return 2;
	}
	else if (%forwardY > 0)
	{
		return 1;
	}
	else
	{
		return 3;
	}
}

function brickDeployProjectile::OnCollision(%this, %obj, %col, %fade, %pos, %normal)
{
	if ($Game::MissionCleaningUp)
	{
		return;
	}
	%client = %obj.client;
	%player = %client.Player;
	if (!%player)
	{
		return;
	}
	if (%client.currInv > 0)
	{
		%data = %client.inventory[%client.currInv];
	}
	else if (isObject(%client.instantUseData))
	{
		%data = %client.instantUseData;
	}
	if (!isObject(%data))
	{
		return;
	}
	if (%data.getClassName() !$= "fxDTSBrickData")
	{
		return;
	}
	if (isObject(%player.tempBrick))
	{
		%player.tempBrick.setDataBlock(%data);
		%aspectRatio = %data.printAspectRatio;
		if (%aspectRatio !$= "")
		{
			if (%client.lastPrint[%aspectRatio] !$= "")
			{
				%player.tempBrick.setPrint(%client.lastPrint[%aspectRatio]);
			}
			else
			{
				%player.tempBrick.setPrint($printNameTable["letters/A"]);
			}
		}
		if ((getAngleIDFromPlayer(%player) + %data.orientationFix) % 4 == 0)
		{
			%rot = "0 0 1 0";
		}
		else if ((getAngleIDFromPlayer(%player) + %data.orientationFix) % 4 == 1)
		{
			%rot = "0 0 -1" SPC (90 * $pi) / 180;
		}
		else if ((getAngleIDFromPlayer(%player) + %data.orientationFix) % 4 == 2)
		{
			%rot = "0 0 1" SPC (180 * $pi) / 180;
		}
		else if ((getAngleIDFromPlayer(%player) + %data.orientationFix) % 4 == 3)
		{
			%rot = "0 0 1" SPC (90 * $pi) / 180;
		}
		%posX = getWord(%pos, 0);
		%posY = getWord(%pos, 1);
		if (%data.brickSizeZ % 2 == 0)
		{
			%posZ = getWord(%pos, 2) + (%data.brickSizeZ / 2) * 0.2 + 0.05;
		}
		else
		{
			%posZ = (getWord(%pos, 2) + (%data.brickSizeZ / 2) * 0.2) - 0.05;
		}
		if (getWord(%normal, 2) < -0.9)
		{
			%posZ -= %data.brickSizeZ * 0.2;
		}
		if (%col.getType() & $TypeMasks::TerrainObjectType)
		{
			%posZ -= 0.1;
		}
		%pos = %posX SPC %posY SPC %posZ;
		%player.tempBrick.setTransform(%pos SPC %rot);
	}
	else
	{
		%b = new fxDTSBrick()
		{
			dataBlock = %data;
			angleID = getAngleIDFromPlayer(%player);
		};
		if (isObject(%client.brickGroup))
		{
			%client.brickGroup.add(%b);
		}
		else
		{
			error("ERROR: brickDeployProjectile::onCollision() - client \"" @ %client.getPlayerName() @ "\" has no brick group.");
		}
		%aspectRatio = %data.printAspectRatio;
		if (%aspectRatio !$= "")
		{
			if (%client.lastPrint[%aspectRatio] !$= "")
			{
				%b.setPrint(%client.lastPrint[%aspectRatio]);
			}
			else
			{
				%b.setPrint($printNameTable["letters/A"]);
			}
		}
		if ((getAngleIDFromPlayer(%player) + %data.orientationFix) % 4 == 0)
		{
			%rot = "0 0 1 0";
		}
		else if ((getAngleIDFromPlayer(%player) + %data.orientationFix) % 4 == 1)
		{
			%rot = "0 0 -1 1.5708";
		}
		else if ((getAngleIDFromPlayer(%player) + %data.orientationFix) % 4 == 2)
		{
			%rot = "0 0 1 3.14159";
		}
		else if ((getAngleIDFromPlayer(%player) + %data.orientationFix) % 4 == 3)
		{
			%rot = "0 0 1 1.5708";
		}
		%posX = getWord(%pos, 0);
		%posY = getWord(%pos, 1);
		if (%data.brickSizeZ % 2 == 0)
		{
			%posZ = getWord(%pos, 2) + (%data.brickSizeZ / 2) * 0.2 + 0.05;
		}
		else
		{
			%posZ = (getWord(%pos, 2) + (%data.brickSizeZ / 2) * 0.2) - 0.05;
		}
		if (getWord(%normal, 2) < -0.9)
		{
			%posZ -= %data.brickSizeZ * 0.2;
		}
		if (%col.getType() & $TypeMasks::TerrainObjectType)
		{
			%posZ -= 0.1;
		}
		%pos = %posX SPC %posY SPC %posZ;
		%b.setTransform(%pos SPC %rot);
		%b.setColor(%client.currentColor);
		%player.tempBrick = %b;
	}
}

datablock ShapeBaseImageData(brickImage)
{
	shapeFile = "~/data/shapes/brickWeapon.dts";
	StaticShape = staticBrick2x2;
	ghost = ghostBrick2x2;
	emap = 0;
	mountPoint = 0;
	offset = "0 -0.05 0";
	rotation = eulerToMatrix("0 180 0");
	eyeOffset = "0.7 1.2 -0.8";
	eyeRotation = eulerToMatrix("0 180 0");
	correctMuzzleVector = 0;
	className = "WeaponImage";
	Item = "";
	ammo = "";
	Projectile = brickDeployProjectile;
	projectileType = Projectile;
	melee = 1;
	armReady = 1;
	showBricks = 1;
	doColorShift = 1;
	colorShiftColor = "0.647 0.647 0.647 1.000";
	stateName[0] = "Ready";
	stateTransitionOnTriggerDown[0] = "Fire";
	stateAllowImageChange[0] = 1;
	stateName[1] = "Fire";
	stateScript[1] = "onFire";
	stateFire[1] = 1;
	stateAllowImageChange[1] = 1;
	stateTimeoutValue[1] = 0.25;
	stateTransitionOnTimeout[1] = "StopFire";
	stateEmitter[1] = brickTrailEmitter;
	stateEmitterTime[1] = 0.1;
	stateSequence[1] = "Fire";
	stateName[2] = "StopFire";
	stateTransitionOnTriggerUp[2] = "Ready";
	stateAllowImageChange[2] = 1;
};
function brickImage::onDeploy(%this, %obj, %slot)
{
}

datablock ShapeBaseImageData(horseBrickImage : brickImage)
{
	mountPoint = 3;
	offset = "0 -0.1 0";
	rotation = eulerToMatrix("0 -90 0");
};
$BRICK_TYPE::SOUND = 1;
$BRICK_TYPE::VEHICLESPAWN = 2;
datablock fxDTSBrickData(brick1x1Data)
{
	brickFile = "~/data/bricks/bricks/1x1.blb";
	category = "Bricks";
	subCategory = "1x";
	uiName = "1x1";
	iconName = "base/client/ui/brickIcons/1x1";
};
datablock fxDTSBrickData(brick1x2Data)
{
	brickFile = "~/data/bricks/bricks/1x2.blb";
	category = "Bricks";
	subCategory = "1x";
	uiName = "1x2";
	iconName = "base/client/ui/brickIcons/1x2";
};
datablock fxDTSBrickData(brick1x3Data)
{
	brickFile = "~/data/bricks/bricks/1x3.blb";
	category = "Bricks";
	subCategory = "1x";
	uiName = "1x3";
	iconName = "base/client/ui/brickIcons/1x3";
};
datablock fxDTSBrickData(brick1x4Data)
{
	brickFile = "~/data/bricks/bricks/1x4.blb";
	category = "Bricks";
	subCategory = "1x";
	uiName = "1x4";
	iconName = "base/client/ui/brickIcons/1x4";
};
datablock fxDTSBrickData(brick1x6Data)
{
	brickFile = "~/data/bricks/bricks/1x6.blb";
	category = "Bricks";
	subCategory = "1x";
	uiName = "1x6";
	iconName = "base/client/ui/brickIcons/1x6";
};
datablock fxDTSBrickData(brick1x8Data)
{
	brickFile = "~/data/bricks/bricks/1x8.blb";
	category = "Bricks";
	subCategory = "1x";
	uiName = "1x8";
	iconName = "base/client/ui/brickIcons/1x8";
};
datablock fxDTSBrickData(brick1x10Data)
{
	brickFile = "~/data/bricks/bricks/1x10.blb";
	category = "Bricks";
	subCategory = "1x";
	uiName = "1x10";
	iconName = "base/client/ui/brickIcons/1x10";
};
datablock fxDTSBrickData(brick1x12Data)
{
	brickFile = "~/data/bricks/bricks/1x12.blb";
	category = "Bricks";
	subCategory = "1x";
	uiName = "1x12";
	iconName = "base/client/ui/brickIcons/1x12";
};
datablock fxDTSBrickData(brick1x16Data)
{
	brickFile = "~/data/bricks/bricks/1x16.blb";
	category = "Bricks";
	subCategory = "1x";
	uiName = "1x16";
	iconName = "base/client/ui/brickIcons/1x16";
};
datablock fxDTSBrickData(brick2x2Data)
{
	brickFile = "~/data/bricks/bricks/2x2.blb";
	category = "Bricks";
	subCategory = "2x";
	uiName = "2x2";
	iconName = "base/client/ui/brickIcons/2x2";
};
datablock fxDTSBrickData(brick2x3Data)
{
	brickFile = "~/data/bricks/bricks/2x3.blb";
	category = "Bricks";
	subCategory = "2x";
	uiName = "2x3";
	iconName = "base/client/ui/brickIcons/2x3";
};
datablock fxDTSBrickData(brick2x4Data)
{
	brickFile = "~/data/bricks/bricks/2x4.blb";
	category = "Bricks";
	subCategory = "2x";
	uiName = "2x4";
	iconName = "base/client/ui/brickIcons/2x4";
};
datablock fxDTSBrickData(brick2x6Data)
{
	brickFile = "~/data/bricks/bricks/2x6.blb";
	category = "Bricks";
	subCategory = "2x";
	uiName = "2x6";
	iconName = "base/client/ui/brickIcons/2x6";
};
datablock fxDTSBrickData(brick2x8Data)
{
	brickFile = "~/data/bricks/bricks/2x8.blb";
	category = "Bricks";
	subCategory = "2x";
	uiName = "2x8";
	iconName = "base/client/ui/brickIcons/2x8";
};
datablock fxDTSBrickData(brick2x10Data)
{
	brickFile = "~/data/bricks/bricks/2x10.blb";
	category = "Bricks";
	subCategory = "2x";
	uiName = "2x10";
	iconName = "base/client/ui/brickIcons/2x10";
};
datablock fxDTSBrickData(brick4x4Data)
{
	brickFile = "~/data/bricks/bricks/4x4.blb";
	category = "Bricks";
	subCategory = "4x";
	uiName = "4x4";
	iconName = "base/client/ui/brickIcons/4x4";
};
datablock fxDTSBrickData(brick4x6Data)
{
	brickFile = "~/data/bricks/bricks/4x6.blb";
	category = "Bricks";
	subCategory = "4x";
	uiName = "4x6";
	iconName = "base/client/ui/brickIcons/4x6";
};
datablock fxDTSBrickData(brick4x8Data)
{
	brickFile = "~/data/bricks/bricks/4x8.blb";
	category = "Bricks";
	subCategory = "4x";
	uiName = "4x8";
	iconName = "base/client/ui/brickIcons/4x8";
};
datablock fxDTSBrickData(brick4x10Data)
{
	brickFile = "~/data/bricks/bricks/4x10.blb";
	category = "Bricks";
	subCategory = "4x";
	uiName = "4x10";
	iconName = "base/client/ui/brickIcons/4x10";
};
datablock fxDTSBrickData(brick4x12Data)
{
	brickFile = "~/data/bricks/bricks/4x12.blb";
	category = "Bricks";
	subCategory = "4x";
	uiName = "4x12";
	iconName = "base/client/ui/brickIcons/4x12";
};
datablock fxDTSBrickData(brick4x16Data)
{
	brickFile = "~/data/bricks/bricks/4x16.blb";
	category = "Bricks";
	subCategory = "4x";
	uiName = "4x16";
	iconName = "base/client/ui/brickIcons/4x16";
};
datablock fxDTSBrickData(brick8x8Data)
{
	brickFile = "~/data/bricks/bricks/8x8.blb";
	category = "Bricks";
	subCategory = "8x";
	uiName = "8x8";
	iconName = "base/client/ui/brickIcons/8x8";
};
datablock fxDTSBrickData(brick8x16Data)
{
	brickFile = "~/data/bricks/bricks/8x16.blb";
	category = "Bricks";
	subCategory = "8x";
	uiName = "8x16";
	iconName = "base/client/ui/brickIcons/8x16";
};
datablock fxDTSBrickData(brick10x10Data)
{
	brickFile = "~/data/bricks/bricks/10x10.blb";
	category = "Bricks";
	subCategory = "10x";
	uiName = "10x10";
	iconName = "base/client/ui/brickIcons/10x10";
};
datablock fxDTSBrickData(brick10x20Data)
{
	brickFile = "~/data/bricks/bricks/10x20.blb";
	category = "Bricks";
	subCategory = "10x";
	uiName = "10x20";
	iconName = "base/client/ui/brickIcons/10x20";
};
datablock fxDTSBrickData(brick12x24Data)
{
	brickFile = "~/data/bricks/bricks/12x24.blb";
	category = "Bricks";
	subCategory = "12x";
	uiName = "12x24";
	iconName = "base/client/ui/brickIcons/12x24";
};
datablock fxDTSBrickData(brick2x2x3Data)
{
	brickFile = "~/data/bricks/bricks/2x2x3.blb";
	category = "Bricks";
	subCategory = "3x Height";
	uiName = "2x2x3";
	iconName = "base/client/ui/brickIcons/2x2x3";
};
datablock fxDTSBrickData(brick2x4x3Data)
{
	brickFile = "~/data/bricks/bricks/2x4x3.blb";
	category = "Bricks";
	subCategory = "3x Height";
	uiName = "2x4x3";
	iconName = "base/client/ui/brickIcons/2x4x3";
};
datablock fxDTSBrickData(brick2x6x3Data)
{
	brickFile = "~/data/bricks/bricks/2x6x3.blb";
	category = "Bricks";
	subCategory = "3x Height";
	uiName = "2x6x3";
	iconName = "base/client/ui/brickIcons/2x6x3";
};
datablock fxDTSBrickData(brick1x1x5Data)
{
	brickFile = "~/data/bricks/bricks/1x1x5.blb";
	category = "Bricks";
	subCategory = "5x Height";
	uiName = "1x1x5";
	iconName = "base/client/ui/brickIcons/1x1x5";
};
datablock fxDTSBrickData(brick1x2x5Data)
{
	brickFile = "~/data/bricks/bricks/1x2x5.blb";
	category = "Bricks";
	subCategory = "5x Height";
	uiName = "1x2x5";
	iconName = "base/client/ui/brickIcons/1x2x5";
};
datablock fxDTSBrickData(brick1x3x5Data)
{
	brickFile = "~/data/bricks/bricks/1x3x5.blb";
	category = "Bricks";
	subCategory = "5x Height";
	uiName = "1x3x5";
	iconName = "base/client/ui/brickIcons/1x3x5";
};
datablock fxDTSBrickData(brick1x4x5Data)
{
	brickFile = "~/data/bricks/bricks/1x4x5.blb";
	category = "Bricks";
	subCategory = "5x Height";
	uiName = "1x4x5";
	iconName = "base/client/ui/brickIcons/1x4x5";
};
datablock fxDTSBrickData(brick1x6x5Data)
{
	brickFile = "~/data/bricks/bricks/1x6x5.blb";
	category = "Bricks";
	subCategory = "5x Height";
	uiName = "1x6x5";
	iconName = "base/client/ui/brickIcons/1x6x5";
};
datablock fxDTSBrickData(brick1x12x5Data)
{
	brickFile = "~/data/bricks/bricks/1x12x5.blb";
	category = "Bricks";
	subCategory = "5x Height";
	uiName = "1x12x5";
	iconName = "base/client/ui/brickIcons/1x12x5";
};
datablock fxDTSBrickData(brick2x2x5Data)
{
	brickFile = "~/data/bricks/bricks/2x2x5.blb";
	category = "Bricks";
	subCategory = "5x Height";
	uiName = "2x2x5";
	iconName = "base/client/ui/brickIcons/2x2x5";
};
datablock fxDTSBrickData(brick2x3x5Data)
{
	brickFile = "~/data/bricks/bricks/2x3x5.blb";
	category = "Bricks";
	subCategory = "5x Height";
	uiName = "2x3x5";
	iconName = "base/client/ui/brickIcons/2x3x5";
};
datablock fxDTSBrickData(brick2x4x5Data)
{
	brickFile = "~/data/bricks/bricks/2x4x5.blb";
	category = "Bricks";
	subCategory = "5x Height";
	uiName = "2x4x5";
	iconName = "base/client/ui/brickIcons/2x4x5";
};
datablock fxDTSBrickData(brick2x6x5Data)
{
	brickFile = "~/data/bricks/bricks/2x6x5.blb";
	category = "Bricks";
	subCategory = "5x Height";
	uiName = "2x6x5";
	iconName = "base/client/ui/brickIcons/2x6x5";
};
datablock fxDTSBrickData(brick2x12x5Data)
{
	brickFile = "~/data/bricks/bricks/2x12x5.blb";
	category = "Bricks";
	subCategory = "5x Height";
	uiName = "2x12x5";
	iconName = "base/client/ui/brickIcons/2x12x5";
};
datablock fxDTSBrickData(brick1x1fData)
{
	brickFile = "~/data/bricks/flats/1x1f.blb";
	category = "Plates";
	subCategory = "1x";
	uiName = "1x1F";
	iconName = "base/client/ui/brickIcons/1x1F";
};
datablock fxDTSBrickData(brick1x2fData)
{
	brickFile = "~/data/bricks/flats/1x2f.blb";
	category = "Plates";
	subCategory = "1x";
	uiName = "1x2F";
	iconName = "base/client/ui/brickIcons/1x2F";
};
datablock fxDTSBrickData(brick1x3fData)
{
	brickFile = "~/data/bricks/flats/1x3f.blb";
	category = "Plates";
	subCategory = "1x";
	uiName = "1x3F";
	iconName = "base/client/ui/brickIcons/1x3F";
};
datablock fxDTSBrickData(brick1x4fData)
{
	brickFile = "~/data/bricks/flats/1x4f.blb";
	category = "Plates";
	subCategory = "1x";
	uiName = "1x4F";
	iconName = "base/client/ui/brickIcons/1x4F";
};
datablock fxDTSBrickData(brick1x6fData)
{
	brickFile = "~/data/bricks/flats/1x6f.blb";
	category = "Plates";
	subCategory = "1x";
	uiName = "1x6F";
	iconName = "base/client/ui/brickIcons/1x6F";
};
datablock fxDTSBrickData(brick1x8fData)
{
	brickFile = "~/data/bricks/flats/1x8f.blb";
	category = "Plates";
	subCategory = "1x";
	uiName = "1x8F";
	iconName = "base/client/ui/brickIcons/1x8F";
};
datablock fxDTSBrickData(brick1x10fData)
{
	brickFile = "~/data/bricks/flats/1x10f.blb";
	category = "Plates";
	subCategory = "1x";
	uiName = "1x10F";
	iconName = "base/client/ui/brickIcons/1x10F";
};
datablock fxDTSBrickData(brick1x12fData)
{
	brickFile = "~/data/bricks/flats/1x12f.blb";
	category = "Plates";
	subCategory = "1x";
	uiName = "1x12F";
	iconName = "base/client/ui/brickIcons/1x12F";
};
datablock fxDTSBrickData(brick1x16fData)
{
	brickFile = "~/data/bricks/flats/1x16f.blb";
	category = "Plates";
	subCategory = "1x";
	uiName = "1x16F";
	iconName = "base/client/ui/brickIcons/1x16F";
};
datablock fxDTSBrickData(brick2x2fData)
{
	brickFile = "~/data/bricks/flats/2x2f.blb";
	category = "Plates";
	subCategory = "2x";
	uiName = "2x2F";
	iconName = "base/client/ui/brickIcons/2x2F";
};
datablock fxDTSBrickData(brick2x3fData)
{
	brickFile = "~/data/bricks/flats/2x3f.blb";
	category = "Plates";
	subCategory = "2x";
	uiName = "2x3F";
	iconName = "base/client/ui/brickIcons/2x3F";
};
datablock fxDTSBrickData(brick2x4fData)
{
	brickFile = "~/data/bricks/flats/2x4f.blb";
	category = "Plates";
	subCategory = "2x";
	uiName = "2x4F";
	iconName = "base/client/ui/brickIcons/2x4F";
};
datablock fxDTSBrickData(brick2x6fData)
{
	brickFile = "~/data/bricks/flats/2x6f.blb";
	category = "Plates";
	subCategory = "2x";
	uiName = "2x6F";
	iconName = "base/client/ui/brickIcons/2x6F";
};
datablock fxDTSBrickData(brick2x8fData)
{
	brickFile = "~/data/bricks/flats/2x8f.blb";
	category = "Plates";
	subCategory = "2x";
	uiName = "2x8F";
	iconName = "base/client/ui/brickIcons/2x8F";
};
datablock fxDTSBrickData(brick2x10fData)
{
	brickFile = "~/data/bricks/flats/2x10f.blb";
	category = "Plates";
	subCategory = "2x";
	uiName = "2x10F";
	iconName = "base/client/ui/brickIcons/2x10F";
};
datablock fxDTSBrickData(brick4x4fData)
{
	brickFile = "~/data/bricks/flats/4x4f.blb";
	category = "Plates";
	subCategory = "4x";
	uiName = "4x4F";
	iconName = "base/client/ui/brickIcons/4x4F";
};
datablock fxDTSBrickData(brick4x6fData)
{
	brickFile = "~/data/bricks/flats/4x6f.blb";
	category = "Plates";
	subCategory = "4x";
	uiName = "4x6F";
	iconName = "base/client/ui/brickIcons/4x6F";
};
datablock fxDTSBrickData(brick4x8fData)
{
	brickFile = "~/data/bricks/flats/4x8f.blb";
	category = "Plates";
	subCategory = "4x";
	uiName = "4x8F";
	iconName = "base/client/ui/brickIcons/4x8F";
};
datablock fxDTSBrickData(brick4x10fData)
{
	brickFile = "~/data/bricks/flats/4x10f.blb";
	category = "Plates";
	subCategory = "4x";
	uiName = "4x10F";
	iconName = "base/client/ui/brickIcons/4x10F";
};
datablock fxDTSBrickData(brick4x12fData)
{
	brickFile = "~/data/bricks/flats/4x12f.blb";
	category = "Plates";
	subCategory = "4x";
	uiName = "4x12F";
	iconName = "base/client/ui/brickIcons/4x12F";
};
datablock fxDTSBrickData(brick4x16fData)
{
	brickFile = "~/data/bricks/flats/4x16f.blb";
	category = "Plates";
	subCategory = "4x";
	uiName = "4x16F";
	iconName = "base/client/ui/brickIcons/4x16F";
};
datablock fxDTSBrickData(brick6x6fData)
{
	brickFile = "~/data/bricks/flats/6x6f.blb";
	category = "Plates";
	subCategory = "6x";
	uiName = "6x6F";
	iconName = "base/client/ui/brickIcons/6x6F";
};
datablock fxDTSBrickData(brick6x8fData)
{
	brickFile = "~/data/bricks/flats/6x8f.blb";
	category = "Plates";
	subCategory = "6x";
	uiName = "6x8F";
	iconName = "base/client/ui/brickIcons/6x8F";
};
datablock fxDTSBrickData(brick6x10fData)
{
	brickFile = "~/data/bricks/flats/6x10f.blb";
	category = "Plates";
	subCategory = "6x";
	uiName = "6x10F";
	iconName = "base/client/ui/brickIcons/6x10F";
};
datablock fxDTSBrickData(brick6x12fData)
{
	brickFile = "~/data/bricks/flats/6x12f.blb";
	category = "Plates";
	subCategory = "6x";
	uiName = "6x12F";
	iconName = "base/client/ui/brickIcons/6x12F";
};
datablock fxDTSBrickData(brick6x16fData)
{
	brickFile = "~/data/bricks/flats/6x16f.blb";
	category = "Plates";
	subCategory = "6x";
	uiName = "6x16F";
	iconName = "base/client/ui/brickIcons/6x16F";
};
datablock fxDTSBrickData(brick6x24fData)
{
	brickFile = "~/data/bricks/flats/6x24f.blb";
	category = "Plates";
	subCategory = "6x";
	uiName = "6x24F";
	iconName = "base/client/ui/brickIcons/6x24F";
};
datablock fxDTSBrickData(brick8x8fData)
{
	brickFile = "~/data/bricks/flats/8x8f.blb";
	category = "Plates";
	subCategory = "8x";
	uiName = "8x8F";
	iconName = "base/client/ui/brickIcons/8x8F";
};
datablock fxDTSBrickData(brick8x16fData)
{
	brickFile = "~/data/bricks/flats/8x16f.blb";
	category = "Plates";
	subCategory = "8x";
	uiName = "8x16F";
	iconName = "base/client/ui/brickIcons/8x16F";
};
datablock fxDTSBrickData(brick1x1RoundData)
{
	brickFile = "~/data/bricks/rounds/1x1round.blb";
	collisionShapeName = "~/data/shapes/bricks/1x1round.dts";
	canCover = 0;
	category = "Rounds";
	subCategory = "Bricks";
	uiName = "1x1 Round";
	iconName = "base/client/ui/brickIcons/1x1 Round";
};
datablock fxDTSBrickData(brick1x1ConeData)
{
	brickFile = "~/data/bricks/rounds/1x1Cone.blb";
	collisionShapeName = "~/data/shapes/bricks/1x1Cone.dts";
	canCover = 0;
	category = "Rounds";
	subCategory = "Bricks";
	uiName = "1x1 Cone";
	iconName = "base/client/ui/brickIcons/1x1 Cone";
};
datablock fxDTSBrickData(brick2x2RoundData)
{
	brickFile = "~/data/bricks/rounds/2x2round.blb";
	collisionShapeName = "~/data/shapes/bricks/2x2round.dts";
	canCover = 0;
	category = "Rounds";
	subCategory = "Bricks";
	uiName = "2x2 Round";
	iconName = "base/client/ui/brickIcons/2x2 Round";
};
datablock fxDTSBrickData(brick2x2x2ConeData)
{
	brickFile = "~/data/bricks/rounds/2x2x2Cone.blb";
	collisionShapeName = "~/data/shapes/bricks/2x2x2Cone.dts";
	canCover = 0;
	category = "Rounds";
	subCategory = "Bricks";
	uiName = "2x2x2 Cone";
	iconName = "base/client/ui/brickIcons/2x2x2 Cone";
};
datablock fxDTSBrickData(brick1x1fRoundData)
{
	brickFile = "~/data/bricks/rounds/1x1fround.blb";
	collisionShapeName = "~/data/shapes/bricks/1x1fround.dts";
	canCover = 0;
	category = "Rounds";
	subCategory = "Plates";
	uiName = "1x1F Round";
	iconName = "base/client/ui/brickIcons/1x1F Round";
};
datablock fxDTSBrickData(brick2x2fRoundData)
{
	brickFile = "~/data/bricks/rounds/2x2fround.blb";
	collisionShapeName = "~/data/shapes/bricks/2x2fround.dts";
	canCover = 0;
	category = "Rounds";
	subCategory = "Plates";
	uiName = "2x2F Round";
	iconName = "base/client/ui/brickIcons/2x2F Round";
};
datablock fxDTSBrickData(brick2x2DiscData)
{
	brickFile = "~/data/bricks/rounds/2x2Disc.blb";
	collisionShapeName = "~/data/shapes/bricks/2x2fround.dts";
	canCover = 0;
	category = "Rounds";
	subCategory = "Bricks";
	uiName = "2x2 Disc";
	iconName = "base/client/ui/brickIcons/2x2 Disc";
};
datablock fxDTSBrickData(brick3x1x7WallData)
{
	brickFile = "~/data/bricks/special/3x1x7wall.blb";
	orientationFix = 1;
	category = "Special";
	subCategory = "Walls";
	uiName = "Castle Wall";
	iconName = "base/client/ui/brickIcons/Castle Wall";
};
datablock fxDTSBrickData(brick4x1x5windowData)
{
	brickFile = "~/data/bricks/special/4x1x5window.blb";
	orientationFix = 1;
	category = "Special";
	subCategory = "Walls";
	uiName = "1x4x5 Window";
	iconName = "base/client/ui/brickIcons/1x4x5 Window";
};
datablock fxDTSBrickData(brick2x2x5girderData)
{
	brickFile = "~/data/bricks/special/2x2x5girder.blb";
	category = "Special";
	subCategory = "Walls";
	uiName = "2x2x5 Lattice";
	iconName = "base/client/ui/brickIcons/2x2x5 Lattice";
};
datablock fxDTSBrickData(brickPineTreeData)
{
	brickFile = "~/data/bricks/special/pineTree.blb";
	collisionShapeName = "~/data/shapes/bricks/pineTree.dts";
	category = "Special";
	subCategory = "Misc";
	uiName = "Pine Tree";
	iconName = "base/client/ui/brickIcons/Pine Tree";
};
datablock fxDTSBrickData(brick4x1x2FenceData)
{
	brickFile = "~/data/bricks/special/4x1x2Fence.blb";
	orientationFix = 1;
	category = "Special";
	subCategory = "Walls";
	uiName = "1x4x2 Fence";
	iconName = "base/client/ui/brickIcons/1x4x2 Fence";
};
datablock fxDTSBrickData(brick1x3RampData)
{
	brickFile = "~/data/bricks/ramps/1x3ramp.blb";
	collisionShapeName = "~/data/shapes/bricks/1x3ramp.dts";
	orientationFix = 1;
	category = "Ramps";
	subCategory = "25 Degree";
	uiName = "25° Ramp 1x";
	iconName = "base/client/ui/brickIcons/25 Ramp 1x";
};
datablock fxDTSBrickData(brick2x3RampData)
{
	brickFile = "~/data/bricks/ramps/2x3ramp.blb";
	collisionShapeName = "~/data/shapes/bricks/2x3ramp.dts";
	orientationFix = 1;
	category = "Ramps";
	subCategory = "25 Degree";
	uiName = "25° Ramp 2x";
	iconName = "base/client/ui/brickIcons/25 Ramp 2x";
};
datablock fxDTSBrickData(brick4x3RampData)
{
	brickFile = "~/data/bricks/ramps/4x3ramp.blb";
	collisionShapeName = "~/data/shapes/bricks/4x3ramp.dts";
	orientationFix = 1;
	category = "Ramps";
	subCategory = "25 Degree";
	uiName = "25° Ramp 4x";
	iconName = "base/client/ui/brickIcons/25 Ramp 4x";
};
datablock fxDTSBrickData(brick1x3RampUpData)
{
	brickFile = "~/data/bricks/ramps/1x3rampUp.blb";
	collisionShapeName = "~/data/shapes/bricks/1x3rampUp.dts";
	orientationFix = 1;
	category = "Ramps";
	subCategory = "25 Degree";
	uiName = "-25° Ramp 1x";
	iconName = "base/client/ui/brickIcons/-25 Ramp 1x";
};
datablock fxDTSBrickData(brick2x3RampUpData)
{
	brickFile = "~/data/bricks/ramps/2x3rampUp.blb";
	collisionShapeName = "~/data/shapes/bricks/2x3rampUp.dts";
	orientationFix = 1;
	category = "Ramps";
	subCategory = "25 Degree";
	uiName = "-25° Ramp 2x";
	iconName = "base/client/ui/brickIcons/-25 Ramp 2x";
};
datablock fxDTSBrickData(brick3x3RampCornerData)
{
	brickFile = "~/data/bricks/ramps/3x3rampCorner.blb";
	collisionShapeName = "~/data/shapes/bricks/3x3rampCorner.dts";
	orientationFix = 1;
	category = "Ramps";
	subCategory = "25 Degree";
	uiName = "25° Ramp Corner";
	iconName = "base/client/ui/brickIcons/25 Ramp Corner";
};
datablock fxDTSBrickData(brick3x3RampUpCornerData)
{
	brickFile = "~/data/bricks/ramps/3x3rampUpCorner.blb";
	collisionShapeName = "~/data/shapes/bricks/3x3rampUpCorner.dts";
	orientationFix = 1;
	category = "Ramps";
	subCategory = "25 Degree";
	uiName = "-25° Ramp Corner";
	iconName = "base/client/ui/brickIcons/-25 Ramp Corner";
};
datablock fxDTSBrickData(brick4x2crestLowData)
{
	brickFile = "~/data/bricks/ramps/4x2crestLow.blb";
	collisionShapeName = "~/data/shapes/bricks/4x2crestLow.dts";
	orientationFix = 1;
	category = "Ramps";
	subCategory = "25 Degree Crest";
	uiName = "25° Crest 4x";
	iconName = "base/client/ui/brickIcons/25 Crest 4x";
};
datablock fxDTSBrickData(brick2x2crestLowData)
{
	brickFile = "~/data/bricks/ramps/2x2crestLow.blb";
	collisionShapeName = "~/data/shapes/bricks/2x2crestLow.dts";
	orientationFix = 1;
	category = "Ramps";
	subCategory = "25 Degree Crest";
	uiName = "25° Crest 2x";
	iconName = "base/client/ui/brickIcons/25 Crest 2x";
};
datablock fxDTSBrickData(brick1x2crestLowData)
{
	brickFile = "~/data/bricks/ramps/1x2crestLow.blb";
	collisionShapeName = "~/data/shapes/bricks/1x2crestLow.dts";
	orientationFix = 1;
	category = "Ramps";
	subCategory = "25 Degree Crest";
	uiName = "25° Crest 1x";
	iconName = "base/client/ui/brickIcons/25 Crest 1x";
};
datablock fxDTSBrickData(brick1x2crestLowEndData)
{
	brickFile = "~/data/bricks/ramps/1x2crestLowEnd.blb";
	collisionShapeName = "~/data/shapes/bricks/1x2crestLowEnd.dts";
	orientationFix = 1;
	category = "Ramps";
	subCategory = "25 Degree Crest";
	uiName = "25° Crest End";
	iconName = "base/client/ui/brickIcons/25 Crest End";
};
datablock fxDTSBrickData(brick2x2crestLowCornerData)
{
	brickFile = "~/data/bricks/ramps/2x2crestLowCorner.blb";
	collisionShapeName = "~/data/shapes/bricks/2x2crestLowCorner.dts";
	orientationFix = 1;
	category = "Ramps";
	subCategory = "25 Degree Crest";
	uiName = "25° Crest Corner";
	iconName = "base/client/ui/brickIcons/25 Crest Corner";
};
datablock fxDTSBrickData(brick1x2RampData)
{
	brickFile = "~/data/bricks/ramps/1x2ramp.blb";
	collisionShapeName = "~/data/shapes/bricks/1x2ramp.dts";
	orientationFix = 1;
	category = "Ramps";
	subCategory = "45 Degree";
	uiName = "45° Ramp 1x";
	iconName = "base/client/ui/brickIcons/45 Ramp 1x";
};
datablock fxDTSBrickData(brick2x2RampData)
{
	brickFile = "~/data/bricks/ramps/2x2ramp.blb";
	collisionShapeName = "~/data/shapes/bricks/2x2ramp.dts";
	orientationFix = 1;
	category = "Ramps";
	subCategory = "45 Degree";
	uiName = "45° Ramp 2x";
	iconName = "base/client/ui/brickIcons/45 Ramp 2x";
};
datablock fxDTSBrickData(brick4x2RampData)
{
	brickFile = "~/data/bricks/ramps/4x2ramp.blb";
	collisionShapeName = "~/data/shapes/bricks/4x2ramp.dts";
	orientationFix = 1;
	category = "Ramps";
	subCategory = "45 Degree";
	uiName = "45° Ramp 4x";
	iconName = "base/client/ui/brickIcons/45 Ramp 4x";
};
datablock fxDTSBrickData(brick1x2RampUpData)
{
	brickFile = "~/data/bricks/ramps/1x2rampUp.blb";
	collisionShapeName = "~/data/shapes/bricks/1x2rampUp.dts";
	orientationFix = 1;
	category = "Ramps";
	subCategory = "45 Degree";
	uiName = "-45° Ramp 1x";
	iconName = "base/client/ui/brickIcons/-45 Ramp 1x";
};
datablock fxDTSBrickData(brick2x2RampUpData)
{
	brickFile = "~/data/bricks/ramps/2x2rampUp.blb";
	collisionShapeName = "~/data/shapes/bricks/2x2rampUp.dts";
	orientationFix = 1;
	category = "Ramps";
	subCategory = "45 Degree";
	uiName = "-45° Ramp 2x";
	iconName = "base/client/ui/brickIcons/-45 Ramp 2x";
};
datablock fxDTSBrickData(brick2x2RampCornerData)
{
	brickFile = "~/data/bricks/ramps/2x2rampCorner.blb";
	collisionShapeName = "~/data/shapes/bricks/2x2rampCorner.dts";
	orientationFix = 1;
	category = "Ramps";
	subCategory = "45 Degree";
	uiName = "45° Ramp Corner";
	iconName = "base/client/ui/brickIcons/45 Ramp Corner";
};
datablock fxDTSBrickData(brick2x2RampUpCornerData)
{
	brickFile = "~/data/bricks/ramps/2x2rampUpCorner.blb";
	collisionShapeName = "~/data/shapes/bricks/2x2rampUpCorner.dts";
	orientationFix = 1;
	category = "Ramps";
	subCategory = "45 Degree";
	uiName = "-45° Ramp Corner";
	iconName = "base/client/ui/brickIcons/-45 Ramp Corner";
};
datablock fxDTSBrickData(brick4x2crestHighData)
{
	brickFile = "~/data/bricks/ramps/4x2crestHigh.blb";
	collisionShapeName = "~/data/shapes/bricks/4x2crestHigh.dts";
	orientationFix = 1;
	category = "Ramps";
	subCategory = "45 Degree Crest";
	uiName = "45° Crest 4x";
	iconName = "base/client/ui/brickIcons/45 Crest 4x";
};
datablock fxDTSBrickData(brick2x2crestHighData)
{
	brickFile = "~/data/bricks/ramps/2x2crestHigh.blb";
	collisionShapeName = "~/data/shapes/bricks/2x2crestHigh.dts";
	orientationFix = 1;
	category = "Ramps";
	subCategory = "45 Degree Crest";
	uiName = "45° Crest 2x";
	iconName = "base/client/ui/brickIcons/45 Crest 2x";
};
datablock fxDTSBrickData(brick1x2crestHighData)
{
	brickFile = "~/data/bricks/ramps/1x2crestHigh.blb";
	collisionShapeName = "~/data/shapes/bricks/1x2crestHigh.dts";
	orientationFix = 1;
	category = "Ramps";
	subCategory = "45 Degree Crest";
	uiName = "45° Crest 1x";
	iconName = "base/client/ui/brickIcons/45 Crest 1x";
};
datablock fxDTSBrickData(brick1x2crestHighEndData)
{
	brickFile = "~/data/bricks/ramps/1x2crestHighEnd.blb";
	collisionShapeName = "~/data/shapes/bricks/1x2crestHighEnd.dts";
	orientationFix = 1;
	category = "Ramps";
	subCategory = "45 Degree Crest";
	uiName = "45° Crest End";
	iconName = "base/client/ui/brickIcons/45 Crest End";
};
datablock fxDTSBrickData(brick2x2crestHighCornerData)
{
	brickFile = "~/data/bricks/ramps/2x2crestHighCorner.blb";
	collisionShapeName = "~/data/shapes/bricks/2x2crestHighCorner.dts";
	orientationFix = 1;
	category = "Ramps";
	subCategory = "45 Degree Crest";
	uiName = "45° Crest Corner";
	iconName = "base/client/ui/brickIcons/45 Crest Corner";
};
datablock fxDTSBrickData(brick1x2x3RampData)
{
	brickFile = "~/data/bricks/ramps/1x2x3ramp.blb";
	collisionShapeName = "~/data/shapes/bricks/1x2x3ramp.dts";
	orientationFix = 1;
	category = "Ramps";
	subCategory = "72 Degree";
	uiName = "72° Ramp 1x";
	iconName = "base/client/ui/brickIcons/72 Ramp 1x";
};
datablock fxDTSBrickData(brick2x2x3RampData)
{
	brickFile = "~/data/bricks/ramps/2x2x3ramp.blb";
	collisionShapeName = "~/data/shapes/bricks/2x2x3ramp.dts";
	orientationFix = 1;
	category = "Ramps";
	subCategory = "72 Degree";
	uiName = "72° Ramp 2x";
	iconName = "base/client/ui/brickIcons/72 Ramp 2x";
};
datablock fxDTSBrickData(brick2x2x3RampCornerData)
{
	brickFile = "~/data/bricks/ramps/2x2x3rampCorner.blb";
	collisionShapeName = "~/data/shapes/bricks/2x2x3rampCorner.dts";
	orientationFix = 1;
	category = "Ramps";
	subCategory = "72 Degree";
	uiName = "72° Ramp Corner";
	iconName = "base/client/ui/brickIcons/72 Ramp Corner";
};
datablock fxDTSBrickData(brick1x2x3RampUpData)
{
	brickFile = "~/data/bricks/ramps/1x2x3rampUp.blb";
	collisionShapeName = "~/data/shapes/bricks/1x2x3rampUp.dts";
	orientationFix = 1;
	category = "Ramps";
	subCategory = "72 Degree";
	uiName = "-72° Ramp 1x";
	iconName = "base/client/ui/brickIcons/-72 Ramp 1x";
};
datablock fxDTSBrickData(brick2x2x3RampUpData)
{
	brickFile = "~/data/bricks/ramps/2x2x3rampUp.blb";
	collisionShapeName = "~/data/shapes/bricks/2x2x3rampUp.dts";
	orientationFix = 1;
	category = "Ramps";
	subCategory = "72 Degree";
	uiName = "-72° Ramp 2x";
	iconName = "base/client/ui/brickIcons/-72 Ramp 2x";
};
datablock fxDTSBrickData(brick2x2x3RampUpCornerData)
{
	brickFile = "~/data/bricks/ramps/2x2x3rampUpCorner.blb";
	collisionShapeName = "~/data/shapes/bricks/2x2x3rampUpCorner.dts";
	orientationFix = 1;
	category = "Ramps";
	subCategory = "72 Degree";
	uiName = "-72° Ramp Corner";
	iconName = "base/client/ui/brickIcons/-72 Ramp Corner";
};
datablock fxDTSBrickData(brick1x2x5RampData)
{
	brickFile = "~/data/bricks/ramps/1x2x5ramp.blb";
	collisionShapeName = "~/data/shapes/bricks/1x2x5ramp.dts";
	orientationFix = 1;
	category = "Ramps";
	subCategory = "80 Degree";
	uiName = "80° Ramp 1x";
	iconName = "base/client/ui/brickIcons/80 Ramp 1x";
};
datablock fxDTSBrickData(brick2x2x5RampData)
{
	brickFile = "~/data/bricks/ramps/2x2x5ramp.blb";
	collisionShapeName = "~/data/shapes/bricks/2x2x5ramp.dts";
	orientationFix = 1;
	category = "Ramps";
	subCategory = "80 Degree";
	uiName = "80° Ramp 2x";
	iconName = "base/client/ui/brickIcons/80 Ramp 2x";
};
datablock fxDTSBrickData(brick2x2x5RampCornerData)
{
	brickFile = "~/data/bricks/ramps/2x2x5rampCorner.blb";
	collisionShapeName = "~/data/shapes/bricks/2x2x5rampCorner.dts";
	orientationFix = 1;
	category = "Ramps";
	subCategory = "80 Degree";
	uiName = "80° Ramp Corner";
	iconName = "base/client/ui/brickIcons/80 Ramp Corner";
};
datablock fxDTSBrickData(brick1x2x5RampUpData)
{
	brickFile = "~/data/bricks/ramps/1x2x5rampUp.blb";
	collisionShapeName = "~/data/shapes/bricks/1x2x5rampUp.dts";
	orientationFix = 1;
	category = "Ramps";
	subCategory = "80 Degree";
	uiName = "-80° Ramp 1x";
	iconName = "base/client/ui/brickIcons/-80 Ramp 1x";
};
datablock fxDTSBrickData(brick2x2x5RampUpData)
{
	brickFile = "~/data/bricks/ramps/2x2x5rampUp.blb";
	collisionShapeName = "~/data/shapes/bricks/2x2x5rampUp.dts";
	orientationFix = 1;
	category = "Ramps";
	subCategory = "80 Degree";
	uiName = "-80° Ramp 2x";
	iconName = "base/client/ui/brickIcons/-80 Ramp 2x";
};
datablock fxDTSBrickData(brick2x2x5RampUpCornerData)
{
	brickFile = "~/data/bricks/ramps/2x2x5rampUpCorner.blb";
	collisionShapeName = "~/data/shapes/bricks/2x2x5rampUpCorner.dts";
	orientationFix = 1;
	category = "Ramps";
	subCategory = "80 Degree";
	uiName = "-80° Ramp Corner";
	iconName = "base/client/ui/brickIcons/-80 Ramp Corner";
};
datablock fxDTSBrickData(brick16x16fData)
{
	brickFile = "~/data/bricks/baseplates/16x16f.blb";
	category = "Baseplates";
	subCategory = "Plain";
	uiName = "16x16 Base";
	iconName = "base/client/ui/brickIcons/16x16 Base";
};
datablock fxDTSBrickData(brick16x32fData)
{
	brickFile = "~/data/bricks/baseplates/16x32f.blb";
	category = "Baseplates";
	subCategory = "Plain";
	uiName = "16x32 Base";
	iconName = "base/client/ui/brickIcons/16x32 Base";
};
datablock fxDTSBrickData(brick32x32fData)
{
	brickFile = "~/data/bricks/baseplates/32x32f.blb";
	category = "Baseplates";
	subCategory = "Plain";
	uiName = "32x32 Base";
	iconName = "base/client/ui/brickIcons/32x32 Base";
};
datablock fxDTSBrickData(brick48x48fData)
{
	brickFile = "~/data/bricks/baseplates/48x48f.blb";
	category = "Baseplates";
	subCategory = "Plain";
	uiName = "48x48 Base";
	iconName = "base/client/ui/brickIcons/48x48 Base";
};
datablock fxDTSBrickData(brick64x64fData)
{
	brickFile = "~/data/bricks/baseplates/64x64f.blb";
	category = "Baseplates";
	subCategory = "Plain";
	uiName = "64x64 Base";
	iconName = "base/client/ui/brickIcons/64x64 Base";
};
datablock fxDTSBrickData(brick32x32froadsData)
{
	brickFile = "~/data/bricks/baseplates/32x32froads.blb";
	category = "Baseplates";
	subCategory = "Road";
	uiName = "32x32 Road";
	iconName = "base/client/ui/brickIcons/32x32 Road";
};
datablock fxDTSBrickData(brick32x32froadtData)
{
	brickFile = "~/data/bricks/baseplates/32x32froadt.blb";
	category = "Baseplates";
	subCategory = "Road";
	uiName = "32x32 Road T";
	iconName = "base/client/ui/brickIcons/32x32 Road T";
};
datablock fxDTSBrickData(brick32x32froadcData)
{
	brickFile = "~/data/bricks/baseplates/32x32froadc.blb";
	category = "Baseplates";
	subCategory = "Road";
	uiName = "32x32 Road C";
	iconName = "base/client/ui/brickIcons/32x32 Road C";
};
datablock fxDTSBrickData(brick32x32froadxData)
{
	brickFile = "~/data/bricks/baseplates/32x32froadx.blb";
	category = "Baseplates";
	subCategory = "Road";
	uiName = "32x32 Road X";
	iconName = "base/client/ui/brickIcons/32x32 Road X";
};
datablock fxDTSBrickData(brick2x2RampPrintData)
{
	brickFile = "~/data/bricks/ramps/2x2rampPrint.blb";
	collisionShapeName = "~/data/shapes/bricks/2x2ramp.dts";
	orientationFix = 1;
	printAspectRatio = "2x2r";
	category = "Ramps";
	subCategory = "Prints";
	uiName = "45° Ramp 2x Print";
	iconName = "base/client/ui/brickIcons/45 Ramp 2x Print";
};
datablock fxDTSBrickData(brick2x2RampUpPrintData)
{
	brickFile = "~/data/bricks/ramps/2x2rampUpPrint.blb";
	collisionShapeName = "~/data/shapes/bricks/2x2rampUp.dts";
	orientationFix = 1;
	printAspectRatio = "2x2r";
	category = "Ramps";
	subCategory = "Prints";
	uiName = "-45° Ramp 2x Print";
	iconName = "base/client/ui/brickIcons/-45 Ramp 2x Print";
};
datablock fxDTSBrickData(brick2x2fPrintData)
{
	brickFile = "~/data/bricks/flats/2x2fPrint.blb";
	orientationFix = 3;
	printAspectRatio = "2x2f";
	category = "Plates";
	subCategory = "Prints";
	uiName = "2x2F Print";
	iconName = "base/client/ui/brickIcons/2x2F Print";
};
datablock fxDTSBrickData(brick1x2fPrintData)
{
	brickFile = "~/data/bricks/flats/1x2fPrint.blb";
	orientationFix = 3;
	printAspectRatio = "1x2f";
	category = "Plates";
	subCategory = "Prints";
	uiName = "1x2F Print";
	iconName = "base/client/ui/brickIcons/1x2F Print";
};
datablock fxDTSBrickData(brick1x1fPrintData)
{
	brickFile = "~/data/bricks/flats/1x1fPrint.blb";
	orientationFix = 3;
	printAspectRatio = "1x1f";
	category = "Plates";
	subCategory = "Prints";
	uiName = "1x1F Print";
	iconName = "base/client/ui/brickIcons/1x1F Print";
};
datablock fxDTSBrickData(brick1x1PrintData)
{
	brickFile = "~/data/bricks/bricks/1x1Print.blb";
	orientationFix = 3;
	printAspectRatio = "1x1";
	category = "Bricks";
	subCategory = "Prints";
	uiName = "1x1 Print";
	iconName = "base/client/ui/brickIcons/1x1 Print";
};
datablock fxDTSBrickData(brick1x4x4PrintData)
{
	brickFile = "~/data/bricks/bricks/1x4x4Print.blb";
	orientationFix = 3;
	printAspectRatio = "1x1";
	category = "Bricks";
	subCategory = "Prints";
	uiName = "1x4x4 Print";
	iconName = "base/client/ui/brickIcons/1x4x4 Print";
};
datablock fxDTSBrickData(brickMusicData)
{
	brickFile = "~/data/bricks/special/1x1music.blb";
	specialBrickType = "Sound";
	orientationFix = 1;
	category = "Special";
	subCategory = "Interactive";
	uiName = "Music Brick";
	iconName = "base/client/ui/brickIcons/Music Brick";
};
datablock fxDTSBrickData(brickSpawnPointData)
{
	brickFile = "~/data/bricks/special/spawnPoint.blb";
	specialBrickType = "SpawnPoint";
	orientationFix = 1;
	canCover = 0;
	category = "Special";
	subCategory = "Interactive";
	uiName = "Spawn Point";
	iconName = "base/client/ui/brickIcons/Spawn Point";
	indestructable = 1;
};
datablock fxDTSBrickData(brickVehicleSpawnData)
{
	brickFile = "~/data/bricks/special/vehicleSpawn.blb";
	specialBrickType = "VehicleSpawn";
	brickType = $BRICK_TYPE::VEHICLESPAWN;
	orientationFix = 1;
	canCover = 0;
	category = "Special";
	subCategory = "Interactive";
	uiName = "Vehicle Spawn";
	iconName = "base/client/ui/brickIcons/Vehicle Spawn";
	indestructable = 1;
};
function fxDTSBrickData::onUse(%this, %player, %InvSlot)
{
	if (!isObject(%player))
	{
		return;
	}
	%player.updateArm(%player.getDataBlock().brickImage);
	%player.mountImage(%player.getDataBlock().brickImage, 0);
	%client = %player.client;
	%client.currInv = %InvSlot;
	if (%InvSlot = -1)
	{
		%client.instantUseData = %this;
	}
	else
	{
		%client.instantUseData = 0;
	}
	if (isObject(%player.tempBrick))
	{
		%oldDB = %player.tempBrick.getDataBlock();
		if (%player.tempBrick.angleID == 0)
		{
			%oldXSize = %oldDB.brickSizeX;
			%oldYSize = %oldDB.brickSizeY;
			%newXSize = %this.brickSizeX;
			%newYSize = %this.brickSizeY;
		}
		else if (%player.tempBrick.angleID == 2)
		{
			%oldXSize = %oldDB.brickSizeX;
			%oldYSize = %oldDB.brickSizeY;
			%newXSize = %this.brickSizeX;
			%newYSize = %this.brickSizeY;
		}
		else if (%player.tempBrick.angleID == 1)
		{
			%oldXSize = %oldDB.brickSizeY;
			%oldYSize = %oldDB.brickSizeX;
			%newXSize = %this.brickSizeY;
			%newYSize = %this.brickSizeX;
		}
		else if (%player.tempBrick.angleID == 3)
		{
			%oldXSize = %oldDB.brickSizeY;
			%oldYSize = %oldDB.brickSizeX;
			%newXSize = %this.brickSizeY;
			%newYSize = %this.brickSizeX;
		}
		if (%oldXSize % 2 > %newXSize % 2)
		{
			%shiftX = 0.2;
		}
		else if (%oldXSize % 2 < %newXSize % 2)
		{
			%shiftX = -0.2;
		}
		else
		{
			%shiftX = 0;
		}
		if (%oldYSize % 2 > %newYSize % 2)
		{
			%shiftY = 0.2;
		}
		else if (%oldYSize % 2 < %newYSize % 2)
		{
			%shiftY = -0.2;
		}
		else
		{
			%shiftY = 0;
		}
		%player.tempBrick.setDataBlock(%this);
		%trans = %player.tempBrick.getTransform();
		%x = getWord(%trans, 0);
		%y = getWord(%trans, 1);
		%z = getWord(%trans, 2);
		%rot = getWords(%trans, 3, 6);
		%forwardVec = %player.getForwardVector();
		%forwardX = getWord(%forwardVec, 0);
		%forwardY = getWord(%forwardVec, 1);
		if (%forwardX > 0)
		{
			if (%forwardX > mAbs(%forwardY))
			{
			}
			else if (%forwardY > 0)
			{
				%x += %shiftX;
			}
			else
			{
				%y += %shiftY;
			}
		}
		else if (mAbs(%forwardX) > mAbs(%forwardY))
		{
			%x += %shiftX;
			%y += %shiftY;
		}
		else if (%forwardY > 0)
		{
			%x += %shiftX;
		}
		else
		{
			%y += %shiftY;
		}
		%player.tempBrick.setTransform(%x SPC %y SPC %z SPC %rot);
		%aspectRatio = %this.printAspectRatio;
		if (%aspectRatio !$= "")
		{
			if (%client.lastPrint[%aspectRatio] $= "")
			{
				%player.tempBrick.setPrint($printNameTable["letters/A"]);
			}
			else
			{
				%player.tempBrick.setPrint(%client.lastPrint[%aspectRatio]);
			}
		}
	}
}

function brickVehicleSpawnData::onColorChange(%data, %obj)
{
	Parent::onColorChange(%data, %obj);
	%obj.colorVehicle();
}

function fxDTSBrick::explode(%obj)
{
	%obj.delete();
}

function fxDTSBrick::onAdd(%obj)
{
	%obj.getDataBlock().onAdd(%obj);
}

function fxDTSBrickData::onAdd(%this, %obj)
{
}

function fxDTSBrick::onDeath(%obj)
{
	if (isObject(%obj.light))
	{
		%obj.light.delete();
	}
	if (isObject(%obj.emitter))
	{
		%obj.emitter.delete();
	}
	if (isObject(%obj.Item))
	{
		%obj.Item.delete();
	}
	if (isObject(%obj.AudioEmitter))
	{
		%obj.AudioEmitter.delete();
	}
	if (isObject(%obj.Vehicle))
	{
		%obj.Vehicle.delete();
	}
	if (isObject(%obj.VehicleSpawnMarker))
	{
		%obj.VehicleSpawnMarker.delete();
	}
	if (%obj.getName())
	{
		%obj.setNTObjectName("");
	}
	if (%obj.numScheduledEvents)
	{
		%obj.cancelEvents();
	}
	%obj.getDataBlock().onDeath(%obj);
	if (isObject($CurrBrickKiller))
	{
		if (isObject($CurrBrickKiller.miniGame))
		{
			$CurrBrickKiller.incScore($CurrBrickKiller.miniGame.Points_BreakBrick);
		}
	}
	$Server::BrickCount--;
}

function fxDTSBrick::onRemove(%obj)
{
	if (isObject(%obj.light))
	{
		%obj.light.delete();
	}
	if (isObject(%obj.emitter))
	{
		%obj.emitter.delete();
	}
	if (isObject(%obj.Item))
	{
		%obj.Item.delete();
	}
	if (isObject(%obj.AudioEmitter))
	{
		%obj.AudioEmitter.delete();
	}
	if (isObject(%obj.Vehicle))
	{
		%obj.Vehicle.delete();
	}
	if (isObject(%obj.VehicleSpawnMarker))
	{
		%obj.VehicleSpawnMarker.delete();
	}
	if (%obj.isPlanted && !%obj.isDead())
	{
		$Server::BrickCount--;
	}
	%obj.getDataBlock().onRemove(%obj);
	if (%obj.getName() !$= "")
	{
		%obj.clearNTObjectName();
	}
}

function fxDTSBrickData::onRemove(%this, %obj)
{
}

function fxDTSBrickData::onDeath(%this, %obj)
{
}

function fxDTSBrick::onPlant(%obj)
{
	$Server::BrickCount++;
	%obj.getDataBlock().onPlant(%obj);
}

function fxDTSBrick::onLoadPlant(%obj)
{
	$Server::BrickCount++;
	%obj.getDataBlock().onLoadPlant(%obj);
}

function fxDTSBrickData::onPlant(%this, %obj)
{
}

function fxDTSBrickData::onLoadPlant(%this, %obj)
{
}

$FXBrick::ChainBatchSize = 100;
$FXBrick::ChainDelay = 10;
function fxDTSBrick::PlantedTrustCheck(%obj)
{
	%obj.chainTrustCheckDown(0);
}

function fxDTSBrick::TrustCheckFinished(%obj)
{
	%obj.setTrusted(1);
	%client = %obj.client;
	if (isObject(%client))
	{
		if (isObject(%client.miniGame))
		{
			%client.incScore(%client.miniGame.Points_PlantBrick);
		}
	}
	if (!%obj.dontCollideAfterTrust)
	{
		%obj.setColliding(1);
	}
	%obj.getDataBlock().onTrustCheckFinished(%obj);
}

function fxDTSBrick::TrustCheckFailed(%obj)
{
	%client = %obj.client;
	if (isObject(%client))
	{
		%client.undoStack.pop();
	}
	%obj.getDataBlock().onTrustCheckFailed(%obj);
	%obj.schedule(10, delete);
}

function fxDTSBrickData::onTrustCheckFinished(%data, %brick)
{
}

function fxDTSBrickData::onTrustCheckFailed(%data, %brick)
{
}

function fxDTSBrick::chainTrustCheckDown(%obj, %idx)
{
	%ourBrickGroup = %obj.getGroup();
	%count = %obj.getNumDownBricks();
	for (%i = 0; %i < $FXBrick::ChainBatchSize; %i++)
	{
		if (%idx >= %count)
		{
			%obj.ChainTrustCheckUp();
			return;
		}
		%checkBrick = %obj.getDownBrick(%i);
		if (getTrustLevel(%checkBrick, %obj) < $TrustLevel::BuildOn)
		{
			%client = %obj.client;
			%checkBrickGroup = %checkBrick.getGroup();
			commandToClient(%client, 'CenterPrint', %checkBrickGroup.name @ " does not trust you enough to do that.", 1);
			%obj.TrustCheckFailed();
			return;
		}
		%idx++;
	}
	%obj.schedule($FXBrick::ChainDelay, chainTrustCheckDown, %idx);
}

function fxDTSBrick::ChainTrustCheckUp(%obj, %idx)
{
	%ourBrickGroup = %obj.getGroup();
	%count = %obj.getNumUpBricks();
	for (%i = 0; %i < $FXBrick::ChainBatchSize; %i++)
	{
		if (%idx >= %count)
		{
			if (%obj.getDataBlock().isWaterBrick)
			{
				%obj.ChainTrustCheckVolume();
			}
			else
			{
				%obj.TrustCheckFinished();
			}
			return;
		}
		%checkBrick = %obj.getUpBrick(%i);
		if (getTrustLevel(%checkBrick, %obj) < $TrustLevel::BuildOn)
		{
			%client = %obj.client;
			%checkBrickGroup = %checkBrick.getGroup();
			commandToClient(%client, 'CenterPrint', %checkBrickGroup.name @ " does not trust you enough to do that.", 1);
			%obj.TrustCheckFailed();
			return;
		}
		%idx++;
	}
	%obj.schedule($FXBrick::ChainDelay, ChainTrustCheckUp, %idx);
}

function fxDTSBrick::ChainTrustCheckVolume(%obj, %idx)
{
	%ourBrickGroup = %obj.getGroup();
	%client = %obj.client;
	%pos = %obj.getPosition();
	%data = %obj.getDataBlock();
	%size = %data.brickSizeX / 2 - 0.05 SPC %data.brickSizeY / 2 - 0.05 SPC %data.brickSizeZ / 2 - 0.05;
	%mask = $TypeMasks::FxBrickAlwaysObjectType;
	initContainerBoxSearch(%pos, %size, %mask);
	while (%checkBrick = containerSearchNext())
	{
		if (getTrustLevel(%checkBrick, %obj) < $TrustLevel::BuildOn)
		{
			%checkBrickGroup = %checkBrick.getGroup();
			commandToClient(%client, 'CenterPrint', %checkBrickGroup.name @ " does not trust you enough to do that.", 1);
			%obj.TrustCheckFailed();
			return;
		}
	}
	%obj.TrustCheckFinished();
}

function fxDTSBrick::undoTrustCheck(%obj)
{
	%obj.chainUndoTrustCheckDown(0);
}

function fxDTSBrick::chainUndoTrustCheckDown(%obj, %idx)
{
	%ourBrickGroup = %obj.getGroup();
	%count = %obj.getNumDownBricks();
	for (%i = 0; %i < $FXBrick::ChainBatchSize; %i++)
	{
		if (%idx >= %count)
		{
			%obj.ChainUndoTrustCheckUp();
			return;
		}
		%checkBrick = %obj.getDownBrick(%i);
		if (getTrustLevel(%checkBrick, %obj) < $TrustLevel::UndoBrick)
		{
			%client = %obj.client;
			%checkBrickGroup = %checkBrick.getGroup();
			commandToClient(%client, 'CenterPrint', %checkBrickGroup.name @ " does not trust you enough to do that.", 1);
			return;
		}
		%idx++;
	}
	%obj.schedule($FXBrick::ChainDelay, chainUndoTrustCheckDown, %idx);
}

function fxDTSBrick::ChainUndoTrustCheckUp(%obj, %idx)
{
	%ourBrickGroup = %obj.getGroup();
	%count = %obj.getNumUpBricks();
	for (%i = 0; %i < $FXBrick::ChainBatchSize; %i++)
	{
		if (%idx >= %count)
		{
			%obj.killBrick();
			return;
		}
		%checkBrick = %obj.getUpBrick(%i);
		if (getTrustLevel(%checkBrick, %obj) < $TrustLevel::UndoBrick)
		{
			%client = %obj.client;
			%checkBrickGroup = %checkBrick.getGroup();
			commandToClient(%client, 'CenterPrint', %checkBrickGroup.name @ " does not trust you enough to do that.", 1);
			return;
		}
		%idx++;
	}
	%obj.schedule($FXBrick::ChainDelay, ChainUndoTrustCheckUp, %idx);
}

function fxDTSBrick::spawnVehicle(%obj, %delay)
{
	if (%delay > 0)
	{
		if (isEventPending(%obj.spawnVehicleSchedule))
		{
			cancel(%obj.spawnVehicleSchedule);
		}
		%obj.spawnVehicleSchedule = %obj.schedule(%delay, spawnVehicle, 0);
		return;
	}
	else if (isEventPending(%obj.spawnVehicleSchedule))
	{
		cancel(%obj.spawnVehicleSchedule);
	}
	if (%obj.getDataBlock().specialBrickType !$= "VehicleSpawn")
	{
		return;
	}
	if (!isObject(%obj.vehicleDataBlock))
	{
		return;
	}
	if (isObject(%obj.Vehicle))
	{
		if (%obj.Vehicle.getDamagePercent() < 1)
		{
			%obj.Vehicle.delete();
		}
	}
	%trans = %obj.getTransform();
	%x = getWord(%trans, 0);
	%y = getWord(%trans, 1);
	%z = getWord(%trans, 2);
	%z += (getWord(%obj.getWorldBox(), 5) - getWord(%obj.getWorldBox(), 2)) / 2;
	%rot = getWords(%trans, 3, 6);
	%quotaObject = getQuotaObjectFromBrick(%obj);
	setCurrentQuotaObject(%quotaObject);
	if (%obj.vehicleDataBlock.getClassName() $= "PlayerData")
	{
		%v = new AIPlayer()
		{
			dataBlock = %obj.vehicleDataBlock;
		};
	}
	else if (%obj.vehicleDataBlock.getClassName() $= "WheeledVehicleData")
	{
		%v = new WheeledVehicle()
		{
			dataBlock = %obj.vehicleDataBlock;
		};
	}
	else if (%obj.vehicleDataBlock.getClassName() $= "FlyingVehicleData")
	{
		%z += %obj.vehicleDataBlock.createHoverHeight;
		%v = new FlyingVehicle()
		{
			dataBlock = %obj.vehicleDataBlock;
		};
	}
	else if (%obj.vehicleDataBlock.getClassName() $= "HoverVehicleData")
	{
		%z += %obj.vehicleDataBlock.createHoverHeight;
		%v = new HoverVehicle()
		{
			dataBlock = %obj.vehicleDataBlock;
		};
	}
	if (!%v)
	{
		return;
	}
	MissionCleanup.add(%v);
	%v.spawnBrick = %obj;
	%v.brickGroup = %obj.getGroup();
	%obj.Vehicle = %v;
	%obj.colorVehicle();
	if (!(%v.getType() & $TypeMasks::PlayerObjectType))
	{
		%worldBoxZ = mAbs(getWord(%v.getWorldBox(), 2) - getWord(%v.getWorldBox(), 5));
		%worldBoxZ = mAbs(getWord(%v.getWorldBox(), 2) - getWord(%v.getPosition(), 2));
		%z += %worldBoxZ + 0.1;
	}
	%trans = %x SPC %y SPC %z SPC %rot;
	%v.setTransform(%trans);
	%wb = %v.getWorldBox();
	%wbMinX = getWord(%wb, 0);
	%wbMinY = getWord(%wb, 1);
	%wbMinZ = getWord(%wb, 2) + 0.5;
	%wbSizeX = getWord(%wb, 3) - %wbMinX;
	%wbSizeY = getWord(%wb, 4) - %wbMinY;
	if (getTerrainHeight(%wbMinX SPC %wbMinY) > %wbMinZ || getTerrainHeight(%wbMinX + %wbSizeX SPC %wbMinY) > %wbMinZ || getTerrainHeight(%wbMinX SPC %wbMinY + %wbSizeY) > %wbMinZ || getTerrainHeight(%wbMinX + %wbSizeX SPC %wbMinY + %wbSizeY) > %wbMinZ)
	{
		%v.delete();
		%obj.Vehicle = "";
		ServerPlay3D(errorSound, %obj.getPosition());
		return;
	}
	if (%v.getType() & $TypeMasks::PlayerObjectType)
	{
		%pos = %v.getHackPosition();
	}
	else
	{
		%pos = %v.getWorldBoxCenter();
	}
	%p = new Projectile()
	{
		dataBlock = spawnProjectile;
		initialVelocity = "0 0 0";
		initialPosition = %pos;
		sourceObject = %v;
		sourceSlot = 0;
		client = %this;
	};
	if (!%p)
	{
		return;
	}
	MissionCleanup.add(%p);
}

function fxDTSBrick::colorVehicle(%obj)
{
	if (!isObject(%obj.Vehicle))
	{
		return;
	}
	if (!%obj.vehicleDataBlock.paintable)
	{
		return;
	}
	if (fileName(%obj.Vehicle.getDataBlock().shapeFile) $= "m.dts")
	{
		applyDefaultCharacterPrefs(%obj.Vehicle);
		return;
	}
	if (%obj.reColorVehicle)
	{
		%RGB = getWords(getColorIDTable(%obj.colorID), 0, 2);
		%obj.Vehicle.setNodeColor("ALL", %RGB SPC "1");
	}
	else
	{
		if (%obj.Vehicle.color $= "")
		{
			%obj.Vehicle.color = "1 1 1 1";
		}
		%obj.Vehicle.setNodeColor("ALL", %obj.Vehicle.color);
	}
}

function fxDTSBrick::unColorVehicle(%obj)
{
	if (!isObject(%obj.Vehicle))
	{
		return;
	}
	%obj.Vehicle.setNodeColor("ALL", "1 1 1 1");
}

function fxDTSBrick::isBlocked(%obj)
{
	%data = %obj.getDataBlock();
	%pos = %obj.getPosition();
	%size = %data.brickSizeX / 2 SPC %data.brickSizeY / 2 SPC %data.brickSizeZ / 5;
	%mask = $TypeMasks::PlayerObjectType;
	initContainerBoxSearch(%pos, %size, %mask);
	while (%searchObj = containerSearchNext())
	{
		%searchObjPos = %searchObj.getPosition();
		%searchObjData = %searchObj.getDataBlock();
		%searchObjRadius = (getWord(%searchObjData.boundingBox, 0) / 8) * 0.75;
		%vec = VectorSub(%searchObjPos, %pos);
		%vec = getWords(%vec, 0, 1) SPC "0";
		%dist = VectorLen(%vec);
		if (%dist < %searchObjRadius)
		{
			return 1;
		}
	}
	return 0;
}

function fxDTSBrick::vehicleMinigameEject(%obj)
{
	%vehicle = %obj.Vehicle;
	if (!isObject(%vehicle))
	{
		return;
	}
	%count = %vehicle.getMountedObjectCount();
	for (%i = 0; %i < %count; %i++)
	{
		%rider = %vehicle.getMountedObject(%i);
		if (miniGameCanUse(%rider, %vehicle) != 1)
		{
			%rider.getDataBlock().schedule(10, doDismount, %rider);
		}
	}
}

function fxDTSBrick::onDisappear(%obj)
{
	if (isObject(%obj.emitter))
	{
		if (isObject(%obj.emitter.emitter))
		{
			%obj.emitter.oldEmitterDB = %obj.emitter.emitter;
		}
		%obj.emitter.setEmitterDataBlock(0);
	}
}

function fxDTSBrick::onReappear(%obj)
{
	if (isObject(%obj.emitter))
	{
		if (isObject(%obj.emitter.oldEmitterDB))
		{
			%obj.emitter.setEmitterDataBlock(%obj.emitter.oldEmitterDB);
		}
	}
}

function fxDTSBrick::onColorChange(%obj)
{
	%data = %obj.getDataBlock();
	%data.onColorChange(%obj);
}

function fxDTSBrickData::onColorChange(%data, %obj)
{
	if (isObject(%obj.emitter))
	{
		%obj.emitter.setColor(getColorIDTable(%obj.colorID));
	}
}

function fxDTSBrick::onFakeDeath(%obj)
{
	%data = %obj.getDataBlock();
	%data.onFakeDeath(%obj);
}

function fxDTSBrickData::onFakeDeath(%data, %obj)
{
	if (isObject(%obj.emitter))
	{
		if (isObject(%obj.emitter.emitter))
		{
			%obj.emitter.oldEmitterDB = %obj.emitter.emitter;
		}
		%obj.emitter.setEmitterDataBlock(0);
	}
	if (isObject($CurrBrickKiller))
	{
		if (isObject($CurrBrickKiller.miniGame))
		{
			$CurrBrickKiller.incScore($CurrBrickKiller.miniGame.Points_BreakBrick);
		}
	}
}

function fxDTSBrick::onClearFakeDeath(%obj)
{
	%data = %obj.getDataBlock();
	%data.onClearFakeDeath(%obj);
}

function fxDTSBrickData::onClearFakeDeath(%data, %obj)
{
	if (isObject(%obj.emitter))
	{
		if (isObject(%obj.emitter.oldEmitterDB))
		{
			%obj.emitter.setEmitterDataBlock(%obj.emitter.oldEmitterDB);
		}
	}
	%obj.onRespawn(%obj.destroyingClient, %obj.destroyingPlayer);
}

function updateDemoBrickCount(%val)
{
	if (isObject(Demo_BrickCount))
	{
		Demo_BrickCount.setText(%val);
	}
}

registerInputEvent("fxDTSBrick", "onActivate", "Self fxDTSBrick" TAB "Player Player" TAB "Client GameConnection" TAB "MiniGame MiniGame");
registerInputEvent("fxDTSBrick", "onPlayerTouch", "Self fxDTSBrick" TAB "Player Player" TAB "Client GameConnection" TAB "MiniGame MiniGame");
registerInputEvent("fxDTSBrick", "onBotTouch", "Self fxDTSBrick" TAB "Bot Player" TAB "Driver Player" TAB "Client GameConnection" TAB "MiniGame MiniGame");
registerInputEvent("fxDTSBrick", "onProjectileHit", "Self fxDTSBrick" TAB "Player Player" TAB "Client GameConnection" TAB "Projectile Projectile" TAB "MiniGame MiniGame");
registerInputEvent("fxDTSBrick", "onBlownUp", "Self fxDTSBrick" TAB "Player Player" TAB "Client GameConnection" TAB "MiniGame MiniGame");
registerInputEvent("fxDTSBrick", "onRespawn", "Self fxDTSBrick");
registerInputEvent("fxDTSBrick", "onRelay", "Self fxDTSBrick");
registerInputEvent("fxDTSBrick", "onPrintCountOverFlow", "Self fxDTSBrick" TAB "Client GameConnection");
registerInputEvent("fxDTSBrick", "onPrintCountUnderFlow", "Self fxDTSBrick" TAB "Client GameConnection");
function fxDTSBrick::onActivate(%obj, %player, %client, %pos, %vec)
{
	$InputTarget_["Self"] = %obj;
	$InputTarget_["Player"] = %player;
	$InputTarget_["Client"] = %client;
	if ($Server::LAN)
	{
		$InputTarget_["MiniGame"] = getMiniGameFromObject(%client);
	}
	else if (getMiniGameFromObject(%obj) == getMiniGameFromObject(%client))
	{
		$InputTarget_["MiniGame"] = getMiniGameFromObject(%obj);
	}
	else
	{
		$InputTarget_["MiniGame"] = 0;
	}
	%obj.ProcessInputEvent("OnActivate", %client);
}

function fxDTSBrick::onPlayerTouch(%obj, %player)
{
	%data = %obj.getDataBlock();
	%data.onPlayerTouch(%obj, %player);
}

function fxDTSBrickData::onPlayerTouch(%data, %obj, %player)
{
	if (getSimTime() - %player.spawnTime < $Game::OnTouchImmuneTime)
	{
		return;
	}
	if (%obj.numEvents <= 0)
	{
		return;
	}
	%image = %player.getMountedImage(0);
	if (%image)
	{
		if (%image.getId() == AdminWandImage.getId())
		{
			return;
		}
	}
	%client = %player.client;
	$InputTarget_["Self"] = %obj;
	$InputTarget_["Player"] = %player;
	$InputTarget_["Client"] = %client;
	if ($Server::LAN)
	{
		$InputTarget_["MiniGame"] = getMiniGameFromObject(%client);
	}
	else if (getMiniGameFromObject(%obj) == getMiniGameFromObject(%client))
	{
		$InputTarget_["MiniGame"] = getMiniGameFromObject(%obj);
	}
	else
	{
		$InputTarget_["MiniGame"] = 0;
	}
	%isVehicle = 0;
	$InputTarget_["Driver"] = 0;
	if (!isObject(%client))
	{
		%isVehicle = 1;
		$InputTarget_["Bot"] = %player;
		if (isObject(%player.getMountedObject(0)))
		{
			$InputTarget_["Driver"] = %player.getMountedObject(0);
		}
		if ($Server::LAN)
		{
			if (isObject(%player.spawnBrick))
			{
				%client = %player.spawnBrick.client;
			}
			if (!isObject(%client))
			{
				%client = %player.getControllingClient();
			}
			if (!isObject(%client))
			{
				%client = ClientGroup.getObject(0);
			}
		}
		else
		{
			if (isObject(%player.spawnBrick))
			{
				%client = findClientByBL_ID(%player.spawnBrick.getGroup().bl_id);
			}
			if (!isObject(%client))
			{
				%client = %player.lastControllingClient;
			}
		}
	}
	if (isObject(%client))
	{
		if (%isVehicle)
		{
			%obj.ProcessInputEvent("OnBotTouch", %client);
		}
		else
		{
			%obj.ProcessInputEvent("OnPlayerTouch", %client);
		}
	}
}

function fxDTSBrick::onBlownUp(%obj, %client, %player)
{
	$InputTarget_["Self"] = %obj;
	$InputTarget_["Player"] = %player;
	$InputTarget_["Client"] = %client;
	if ($Server::LAN)
	{
		$InputTarget_["MiniGame"] = getMiniGameFromObject(%client);
	}
	else if (getMiniGameFromObject(%obj) == getMiniGameFromObject(%client))
	{
		$InputTarget_["MiniGame"] = getMiniGameFromObject(%obj);
	}
	else
	{
		$InputTarget_["MiniGame"] = 0;
	}
	%obj.destroyingClient = %client;
	%obj.destroyingPlayer = %player;
	%obj.ProcessInputEvent("OnBlownUp", %client);
}

function fxDTSBrick::onRespawn(%obj, %client, %player)
{
	if (%obj.eventFloodCheck(1))
	{
		return;
	}
	$InputTarget_["Self"] = %obj;
	%obj.ProcessInputEvent("OnRespawn", %client);
}

function fxDTSBrick::onRelay(%obj, %client)
{
	$InputTarget_["Self"] = %obj;
	%obj.ProcessInputEvent("OnRelay", %client);
}

function fxDTSBrick::onProjectileHit(%obj, %projectile, %client)
{
	if (%obj.numEvents <= 0)
	{
		return;
	}
	if (%obj.eventFloodCheck(2))
	{
		return;
	}
	if (isObject(%client))
	{
		if (%client.eventFloodCheck(5))
		{
			return;
		}
	}
	if (isObject(%projectile.sourceObject))
	{
		%player = %projectile.sourceObject;
	}
	$InputTarget_["Self"] = %obj;
	$InputTarget_["Player"] = %player;
	if (isObject(%player))
	{
		$InputTarget_["Client"] = %player.client;
	}
	else
	{
		$InputTarget_["Client"] = "";
	}
	$InputTarget_["Projectile"] = %projectile;
	if ($Server::LAN)
	{
		$InputTarget_["MiniGame"] = getMiniGameFromObject(%client);
	}
	else if (getMiniGameFromObject(%obj) == getMiniGameFromObject(%client))
	{
		$InputTarget_["MiniGame"] = getMiniGameFromObject(%obj);
	}
	else
	{
		$InputTarget_["MiniGame"] = 0;
	}
	%obj.ProcessInputEvent("onProjectileHit", %client);
}

function fxDTSBrick::onPrintCountOverFlow(%obj, %client)
{
	if (%obj.eventFloodCheck(4))
	{
		return;
	}
	$InputTarget_["Self"] = %obj;
	$InputTarget_["Client"] = %client;
	%obj.ProcessInputEvent("onPrintCountOverFlow", %client);
}

function fxDTSBrick::onPrintCountUnderFlow(%obj, %client)
{
	if (%obj.eventFloodCheck(4))
	{
		return;
	}
	$InputTarget_["Self"] = %obj;
	$InputTarget_["Client"] = %client;
	%obj.ProcessInputEvent("onPrintCountUnderFlow", %client);
}

function fxDTSBrick::onToolBreak(%obj, %client)
{
	$InputTarget_["Self"] = %obj;
	$InputTarget_["Player"] = %client.Player;
	$InputTarget_["Client"] = %client;
	if ($Server::LAN)
	{
		$InputTarget_["MiniGame"] = getMiniGameFromObject(%client);
	}
	else if (getMiniGameFromObject(%obj) == getMiniGameFromObject(%client))
	{
		$InputTarget_["MiniGame"] = getMiniGameFromObject(%obj);
	}
	else
	{
		$InputTarget_["MiniGame"] = 0;
	}
	%obj.ProcessInputEvent("OnToolBreak", %client);
}

registerOutputEvent("fxDTSBrick", "setColor", "paintColor 0", 0);
registerOutputEvent("fxDTSBrick", "setColorFX", "list None 0 Pearl 1 Chrome 2 Glow 3 Blink 4 Swirl 5 Rainbow 6", 0);
registerOutputEvent("fxDTSBrick", "setShapeFX", "list None 0 Undulo 1 Water 2", 0);
registerOutputEvent("fxDTSBrick", "setColliding", "bool", 0);
registerOutputEvent("fxDTSBrick", "setRendering", "bool", 0);
registerOutputEvent("fxDTSBrick", "setRayCasting", "bool", 0);
registerOutputEvent("fxDTSBrick", "disappear", "int -1 300 5");
registerOutputEvent("fxDTSBrick", "fakeKillBrick", "vector 200" TAB "int 0 300 5");
registerOutputEvent("fxDTSBrick", "respawn", "");
registerOutputEvent("fxDTSBrick", "setEmitter", "dataBlock ParticleEmitterData");
registerOutputEvent("fxDTSBrick", "setEmitterDirection", "list Up 0 Down 1 North 2 East 3 South 4 West 5");
registerOutputEvent("fxDTSBrick", "setLight", "dataBlock FxLightData");
registerOutputEvent("fxDTSBrick", "setItem", "dataBlock ItemData");
registerOutputEvent("fxDTSBrick", "setItemDirection", "list North 2 East 3 South 4 West 5");
registerOutputEvent("fxDTSBrick", "setItemPosition", "list Up 0 Down 1 North 2 East 3 South 4 West 5");
registerOutputEvent("fxDTSBrick", "setMusic", "dataBlock Music");
registerOutputEvent("fxDTSBrick", "playSound", "dataBlock Sound");
registerOutputEvent("fxDTSBrick", "spawnItem", "vector 200" TAB "dataBlock ItemData");
registerOutputEvent("fxDTSBrick", "spawnProjectile", "vector 200" TAB "dataBlock ProjectileData" TAB "vector 200" TAB "float 0.2 2 0.1 1");
registerOutputEvent("fxDTSBrick", "spawnExplosion", "dataBlock ProjectileData" TAB "float 0.2 2 0.1 1");
registerOutputEvent("fxDTSBrick", "fireRelay", "");
registerOutputEvent("fxDTSBrick", "fireRelayUp", "");
registerOutputEvent("fxDTSBrick", "fireRelayDown", "");
registerOutputEvent("fxDTSBrick", "fireRelayNorth", "");
registerOutputEvent("fxDTSBrick", "fireRelayEast", "");
registerOutputEvent("fxDTSBrick", "fireRelaySouth", "");
registerOutputEvent("fxDTSBrick", "fireRelayWest", "");
registerOutputEvent("fxDTSBrick", "cancelEvents", "");
registerOutputEvent("fxDTSBrick", "setEventEnabled", "intList 157" TAB "bool");
registerOutputEvent("fxDTSBrick", "toggleEventEnabled", "intList 176");
registerOutputEvent("fxDTSBrick", "setVehicle", "dataBlock Vehicle");
registerOutputEvent("fxDTSBrick", "respawnVehicle", "");
registerOutputEvent("fxDTSBrick", "recoverVehicle", "");
registerOutputEvent("fxDTSBrick", "radiusImpulse", "int 1 100 5" TAB "int -50000 50000 50" TAB "int -50000 50000 10");
registerOutputEvent("fxDTSBrick", "incrementPrintCount", "int 1 9 1");
registerOutputEvent("fxDTSBrick", "decrementPrintCount", "int 1 9 1");
registerOutputEvent("fxDTSBrick", "setPrintCount", "int 0 9 0");
function fxDTSBrick::disappear(%obj, %time)
{
	%data = %obj.getDataBlock();
	%data.disappear(%obj, %time);
}

function fxDTSBrickData::disappear(%data, %obj, %time)
{
	if (isEventPending(%obj.reappearEvent))
	{
		cancel(%obj.reappearEvent);
	}
	if (%time > 0)
	{
		%obj.setRendering(0);
		%obj.setRayCasting(0);
		%obj.setColliding(0);
		%obj.reappearEvent = %obj.schedule(%time * 1000, reappear);
	}
	else if (%time == 0)
	{
		%obj.setRendering(1);
		%obj.setRayCasting(1);
		%obj.setColliding(1);
		if (%obj.isFakeDead())
		{
			transmitBrickExplosion(%obj.getPosition(), 0, 1, 1, %obj);
		}
	}
	else if (%time < 0)
	{
		%obj.setRendering(0);
		%obj.setRayCasting(0);
		%obj.setColliding(0);
	}
}

function fxDTSBrick::reappear(%obj)
{
	%data = %obj.getDataBlock();
	%data.reappear(%obj);
}

function fxDTSBrickData::reappear(%data, %obj)
{
	if (isEventPending(%obj.reappearEvent))
	{
		cancel(%obj.reappearEvent);
	}
	%obj.setRendering(1);
	%obj.setRayCasting(1);
	%obj.setColliding(1);
}

function fxDTSBrick::fakeKillBrick(%obj, %vector, %time, %client)
{
	if (%obj.eventFloodCheck(1))
	{
		return;
	}
	if (isEventPending(%obj.reappearEvent))
	{
		%obj.reappear();
	}
	%pos = %obj.getPosition();
	%explosionForce = VectorLen(%vector) * 2;
	%pos = VectorSub(%pos, VectorNormalize(%vector));
	%time = mClamp(%time, 0, 300);
	transmitBrickExplosion(%pos, %explosionForce, 1, %time * 1000, %obj);
	%obj.destroyingClient = %client;
	%obj.destroyingPlayer = %client.Player;
}

function fxDTSBrick::Respawn(%obj)
{
	if (isEventPending(%obj.reappearEvent))
	{
		%obj.reappear();
	}
	transmitBrickExplosion(%obj.getPosition(), 0, 1, 1, %obj);
}

function fxDTSBrick::playSound(%obj, %soundData)
{
	if (%obj.getFakeDeadTime() > 120)
	{
		return;
	}
	if (!isObject(%soundData))
	{
		return;
	}
	if (!isObject(%soundData.getDescription()))
	{
		return;
	}
	if (%soundData.getDescription().isLooping == 1)
	{
		return;
	}
	if (!%soundData.getDescription().is3D)
	{
		return;
	}
	ServerPlay3D(%soundData, %obj.getPosition());
}

function fxDTSBrick::spawnItem(%obj, %vector, %itemData)
{
	if (%obj.getFakeDeadTime() > 120 || !%obj.isRendering() && !%obj.isRayCasting())
	{
		return;
	}
	%item = new Item()
	{
		dataBlock = %itemData;
		static = 0;
	};
	if (!%item)
	{
		return;
	}
	MissionCleanup.add(%item);
	%item.setVelocity(%vector);
	%oldQuotaObject = getCurrentQuotaObject();
	if (isObject(%oldQuotaObject))
	{
		setCurrentQuotaObject(GlobalQuota);
	}
	%item.schedulePop();
	if (isObject(%oldQuotaObject))
	{
		setCurrentQuotaObject(%oldQuotaObject);
	}
	%item.spawnBrick = %obj;
	%dir = mFloor(%obj.itemDirection);
	if (%dir == 2)
	{
		%rot = "0 0 1 0";
	}
	else if (%dir == 3)
	{
		%rot = "0 0 1 " @ $piOver2;
	}
	else if (%dir == 4)
	{
		%rot = "0 0 -1 " @ $pi;
	}
	else if (%dir == 5)
	{
		%rot = "0 0 -1 " @ $piOver2;
	}
	else
	{
		%rot = "0 0 1 0";
	}
	%pos = %obj.getPosition();
	%item.setTransform(%pos SPC %rot);
	%itemBox = %item.getWorldBox();
	%itemBoxX = mAbs(getWord(%itemBox, 0) - getWord(%itemBox, 3)) / 2;
	%itemBoxY = mAbs(getWord(%itemBox, 1) - getWord(%itemBox, 4)) / 2;
	%itemBoxZ = mAbs(getWord(%itemBox, 2) - getWord(%itemBox, 5)) / 2;
	%itemBoxCenter = %item.getWorldBoxCenter();
	%itemCenter = %item.getPosition();
	%itemOffset = VectorSub(%itemCenter, %itemBoxCenter);
	%brickBox = %obj.getWorldBox();
	%brickBoxX = mAbs(getWord(%brickBox, 0) - getWord(%brickBox, 3)) / 2;
	%brickBoxY = mAbs(getWord(%brickBox, 1) - getWord(%brickBox, 4)) / 2;
	%brickBoxZ = mAbs(getWord(%brickBox, 2) - getWord(%brickBox, 5)) / 2;
	%vecZ = getWord(%vector, 2);
	if (%vecZ > 0)
	{
		if ((%itemBoxZ - %brickBoxZ) + 0.1 > 0)
		{
			%itemOffset = VectorAdd(%itemOffset, "0 0" SPC (%itemBoxZ - %brickBoxZ) + 0.1);
		}
	}
	else if (%vecZ < 0)
	{
		if ((%itemBoxZ - %brickBoxZ) + 0.1 > 0)
		{
			%itemOffset = VectorSub(%itemOffset, "0 0" SPC (%itemBoxZ - %brickBoxZ) + 0.1);
		}
	}
	%pos = VectorAdd(%pos, %itemOffset);
	%posX = getWord(%pos, 0);
	%posY = getWord(%pos, 1);
	%posZ = getWord(%pos, 2);
	%rot = getWords(%item.getTransform(), 3, 6);
	%item.setTransform(%posX SPC %posY SPC %posZ SPC %rot);
	%item.setCollisionTimeout(%obj);
}

function fxDTSBrick::spawnProjectile(%obj, %velocity, %projectileData, %variance, %scale, %client)
{
	if (%obj.getFakeDeadTime() > 120 || !%obj.isRendering() && !%obj.isRayCasting())
	{
		return;
	}
	if (!isObject(%projectileData))
	{
		return;
	}
	%wb = %obj.getWorldBox();
	%wbX = getWord(%wb, 0);
	%wbY = getWord(%wb, 1);
	%wbZ = getWord(%wb, 2);
	%wbXSize = getWord(%wb, 3) - %wbX;
	%wbYSize = getWord(%wb, 4) - %wbY;
	%wbZSize = getWord(%wb, 5) - %wbZ;
	if (%wbXSize < 1.05)
	{
		%wbX += %wbXSize / 2;
		%wbXSize = 0;
	}
	if (%wbYSize < 1.05)
	{
		%wbY += %wbYSize / 2;
		%wbYSize = 0;
	}
	if (%wbZSize < 0.65)
	{
		%wbZ += %wbZSize / 2;
		%wbZSize = 0;
	}
	%pos = %wbX + getRandom() * %wbXSize SPC %wbY + getRandom() * %wbYSize SPC %wbZ + getRandom() * %wbZSize;
	%velx = getWord(%velocity, 0);
	%vely = getWord(%velocity, 1);
	%velz = getWord(%velocity, 2);
	%varx = getWord(%variance, 0);
	%vary = getWord(%variance, 1);
	%varz = getWord(%variance, 2);
	%x = (%velx + %varx * getRandom()) - %varx / 2;
	%y = (%vely + %vary * getRandom()) - %vary / 2;
	%z = (%velz + %varz * getRandom()) - %varz / 2;
	%muzzleVelocity = %x SPC %y SPC %z;
	%p = new Projectile()
	{
		dataBlock = %projectileData;
		initialVelocity = %muzzleVelocity;
		initialPosition = %pos;
		sourceClient = %client;
		sourceObject = %obj;
		client = %client;
	};
	if (%p)
	{
		MissionCleanup.add(%p);
		%p.setScale(%scale SPC %scale SPC %scale);
		%p.spawnBrick = %obj;
	}
}

function fxDTSBrick::spawnExplosion(%obj, %projectileData, %scale, %client)
{
	if (%obj.getFakeDeadTime() > 120 || !%obj.isRendering() && !%obj.isRayCasting())
	{
		return;
	}
	if (!isObject(%projectileData))
	{
		return;
	}
	%wb = %obj.getWorldBox();
	%wbX = getWord(%wb, 0);
	%wbY = getWord(%wb, 1);
	%wbZ = getWord(%wb, 2);
	%wbXSize = getWord(%wb, 3) - %wbX;
	%wbYSize = getWord(%wb, 4) - %wbY;
	%wbZSize = getWord(%wb, 5) - %wbZ;
	if (%wbXSize < 1.05)
	{
		%wbX += %wbXSize / 2;
		%wbXSize = 0;
	}
	if (%wbYSize < 1.05)
	{
		%wbY += %wbYSize / 2;
		%wbYSize = 0;
	}
	if (%wbZSize < 0.65)
	{
		%wbZ += %wbZSize / 2;
		%wbZSize = 0;
	}
	%pos = %wbX + getRandom() * %wbXSize SPC %wbY + getRandom() * %wbYSize SPC %wbZ + getRandom() * %wbZSize;
	%p = new Projectile()
	{
		dataBlock = %projectileData;
		initialVelocity = "0 0 1";
		initialPosition = %pos;
		sourceClient = %client;
		sourceObject = %obj;
		client = %client;
	};
	if (!isObject(%p))
	{
		return;
	}
	MissionCleanup.add(%p);
	%p.setScale(%scale SPC %scale SPC %scale);
	%p.spawnBrick = %obj;
	%p.explode();
}

function fxDTSBrick::fireRelay(%obj, %client)
{
	%currTime = getSimTime();
	if (atoi(%currTime) - atoi(%obj.lastRelayTime) < 15)
	{
		return;
	}
	%obj.lastRelayTime = %currTime;
	%obj.onRelay(%client);
}

function fxDTSBrick::fireRelayUp(%obj, %client)
{
	%wb = %obj.getWorldBox();
	%sizeX = (getWord(%wb, 3) - getWord(%wb, 0)) - 0.1;
	%sizeY = (getWord(%wb, 4) - getWord(%wb, 1)) - 0.1;
	%sizeZ = (getWord(%wb, 5) - getWord(%wb, 2)) - 0.1;
	%size = %sizeX SPC %sizeY SPC "0.1";
	%pos = %obj.getPosition();
	%posX = getWord(%pos, 0);
	%posY = getWord(%pos, 1);
	%posZ = getWord(%pos, 2) + %sizeZ / 2 + 0.05;
	%pos = %posX SPC %posY SPC %posZ;
	%obj.fireRelayFromBox(%pos, %size, %client);
}

function fxDTSBrick::fireRelayDown(%obj, %client)
{
	%wb = %obj.getWorldBox();
	%sizeX = (getWord(%wb, 3) - getWord(%wb, 0)) - 0.1;
	%sizeY = (getWord(%wb, 4) - getWord(%wb, 1)) - 0.1;
	%sizeZ = (getWord(%wb, 5) - getWord(%wb, 2)) - 0.1;
	%size = %sizeX SPC %sizeY SPC "0.1";
	%pos = %obj.getPosition();
	%posX = getWord(%pos, 0);
	%posY = getWord(%pos, 1);
	%posZ = (getWord(%pos, 2) - %sizeZ / 2) - 0.05;
	%pos = %posX SPC %posY SPC %posZ;
	%obj.fireRelayFromBox(%pos, %size, %client);
}

function fxDTSBrick::fireRelayNorth(%obj, %client)
{
	%wb = %obj.getWorldBox();
	%sizeX = (getWord(%wb, 3) - getWord(%wb, 0)) - 0.1;
	%sizeY = (getWord(%wb, 4) - getWord(%wb, 1)) - 0.1;
	%sizeZ = (getWord(%wb, 5) - getWord(%wb, 2)) - 0.1;
	%size = %sizeX SPC "0.1" SPC %sizeZ;
	%pos = %obj.getPosition();
	%posX = getWord(%pos, 0);
	%posY = getWord(%pos, 1) + %sizeY / 2 + 0.05;
	%posZ = getWord(%pos, 2);
	%pos = %posX SPC %posY SPC %posZ;
	%obj.fireRelayFromBox(%pos, %size, %client);
}

function fxDTSBrick::fireRelaySouth(%obj, %client)
{
	%wb = %obj.getWorldBox();
	%sizeX = (getWord(%wb, 3) - getWord(%wb, 0)) - 0.1;
	%sizeY = (getWord(%wb, 4) - getWord(%wb, 1)) - 0.1;
	%sizeZ = (getWord(%wb, 5) - getWord(%wb, 2)) - 0.1;
	%size = %sizeX SPC "0.1" SPC %sizeZ;
	%pos = %obj.getPosition();
	%posX = getWord(%pos, 0);
	%posY = (getWord(%pos, 1) - %sizeY / 2) - 0.05;
	%posZ = getWord(%pos, 2);
	%pos = %posX SPC %posY SPC %posZ;
	%obj.fireRelayFromBox(%pos, %size, %client);
}

function fxDTSBrick::fireRelayEast(%obj, %client)
{
	%wb = %obj.getWorldBox();
	%sizeX = (getWord(%wb, 3) - getWord(%wb, 0)) - 0.1;
	%sizeY = (getWord(%wb, 4) - getWord(%wb, 1)) - 0.1;
	%sizeZ = (getWord(%wb, 5) - getWord(%wb, 2)) - 0.1;
	%size = "0.1" SPC %sizeY SPC %sizeZ;
	%pos = %obj.getPosition();
	%posX = getWord(%pos, 0) + %sizeX / 2 + 0.05;
	%posY = getWord(%pos, 1);
	%posZ = getWord(%pos, 2);
	%pos = %posX SPC %posY SPC %posZ;
	%obj.fireRelayFromBox(%pos, %size, %client);
}

function fxDTSBrick::fireRelayWest(%obj, %client)
{
	%wb = %obj.getWorldBox();
	%sizeX = (getWord(%wb, 3) - getWord(%wb, 0)) - 0.1;
	%sizeY = (getWord(%wb, 4) - getWord(%wb, 1)) - 0.1;
	%sizeZ = (getWord(%wb, 5) - getWord(%wb, 2)) - 0.1;
	%size = "0.1" SPC %sizeY SPC %sizeZ;
	%pos = %obj.getPosition();
	%posX = (getWord(%pos, 0) - %sizeX / 2) - 0.05;
	%posY = getWord(%pos, 1);
	%posZ = getWord(%pos, 2);
	%pos = %posX SPC %posY SPC %posZ;
	%obj.fireRelayFromBox(%pos, %size, %client);
}

function fxDTSBrick::fireRelayFromBox(%obj, %pos, %size, %client)
{
	%mask = $TypeMasks::FxBrickAlwaysObjectType;
	%group = %obj.getGroup();
	initContainerBoxSearch(%pos, %size, %mask);
	while ((%searchObj = containerSearchNext()) != 0)
	{
		if (!%searchObj.getGroup() == %group)
		{
		}
		else if (%searchObj == %obj)
		{
		}
		else if (%searchObj.numEvents <= 0)
		{
		}
		else
		{
			%obj.addScheduledEvent(%searchObj.schedule(33, fireRelay, %client));
		}
	}
}

function fxDTSBrick::respawnVehicle(%obj)
{
	%obj.spawnVehicle();
}

function fxDTSBrick::recoverVehicle(%obj)
{
	%vehicle = %obj.Vehicle;
	if (!isObject(%vehicle))
	{
		%obj.spawnVehicle();
		return;
	}
	%mountCount = %vehicle.getMountedObjectCount();
	if (%mountCount <= 0)
	{
		%obj.spawnVehicle();
		return;
	}
	for (%i = 0; %i < %mountCount; %i++)
	{
		%passenger = %vehicle.getMountedObject(%i);
		if (isObject(%passenger.client))
		{
			return;
		}
		if (%passenger.getMountedObjectCount() > 0)
		{
			return;
		}
	}
	%obj.spawnVehicle();
}

function fxDTSBrick::radiusImpulse(%obj, %radius, %force, %verticalForce, %client)
{
	%pos = %obj.getPosition();
	%mask = $TypeMasks::PlayerObjectType | $TypeMasks::VehicleObjectType | $TypeMasks::CorpseObjectType | $TypeMasks::ItemObjectType;
	initContainerRadiusSearch(%pos, %radius, %mask);
	%mg = getMiniGameFromObject(%client);
	while ((%searchObj = containerSearchNext()) != 0)
	{
		%searchObj = getWord(%searchObj, 0);
		if (isObject(%mg))
		{
			if (miniGameCanDamage(%client, %searchObj) == 0)
			{
				continue;
			}
		}
		else if (!$Server::LAN)
		{
			if (%searchObj.client != %client)
			{
				continue;
			}
			if (isObject(%searchObj.spawnBrick))
			{
				if (%searchObj.spawnBrick.getGroup.bl_id != %client.getBLID())
				{
					continue;
				}
			}
		}
		if (%searchObj.getType() & $TypeMasks::PlayerObjectType)
		{
			%searchPos = %searchObj.getHackPosition();
		}
		else
		{
			%searchPos = %searchObj.getWorldBoxCenter();
		}
		%dist = VectorDist(%searchPos, %pos);
		%distanceFactor = 1 - (%dist / %radius) * (%dist / %radius);
		if (%distanceFactor <= 0)
		{
			continue;
		}
		else if (%distanceFactor > 1)
		{
			%distanceFactor = 1;
		}
		%impulseVec = VectorSub(%searchPos, %pos);
		%impulseVec = VectorNormalize(%impulseVec);
		%impulseVec = VectorScale(%impulseVec, %force * %distanceFactor);
		%searchObj.applyImpulse(%searchPos, %impulseVec);
		%impulseVec = VectorScale("0 0 1", %verticalForce * %distanceFactor);
		%searchObj.applyImpulse(%pos, %impulseVec);
		if (isObject(%client))
		{
			%searchObj.lastPusher = %client;
			%searchObj.lastPushTime = getSimTime();
		}
	}
}

function generatePrintCountTable()
{
	for (%i = 0; %i < 10; %i++)
	{
		$PrintCountIdx[%i] = -1;
	}
	%i = 0;
	for (%texture = strlwr(getPrintTexture(%i)); %texture !$= ""; %texture = strlwr(getPrintTexture(%i++)))
	{
		%path = filePath(%texture);
		%name = fileName(%texture);
		if (strstr(%path, "print_letters_default") != -1 && %name $= "0.png")
		{
			$PrintCountIdx[0] = %i;
		}
		if (strstr(%path, "print_letters_default") != -1 && %name $= "1.png")
		{
			$PrintCountIdx[1] = %i;
		}
		if (strstr(%path, "print_letters_default") != -1 && %name $= "2.png")
		{
			$PrintCountIdx[2] = %i;
		}
		if (strstr(%path, "print_letters_default") != -1 && %name $= "3.png")
		{
			$PrintCountIdx[3] = %i;
		}
		if (strstr(%path, "print_letters_default") != -1 && %name $= "4.png")
		{
			$PrintCountIdx[4] = %i;
		}
		if (strstr(%path, "print_letters_default") != -1 && %name $= "5.png")
		{
			$PrintCountIdx[5] = %i;
		}
		if (strstr(%path, "print_letters_default") != -1 && %name $= "6.png")
		{
			$PrintCountIdx[6] = %i;
		}
		if (strstr(%path, "print_letters_default") != -1 && %name $= "7.png")
		{
			$PrintCountIdx[7] = %i;
		}
		if (strstr(%path, "print_letters_default") != -1 && %name $= "8.png")
		{
			$PrintCountIdx[8] = %i;
		}
		if (strstr(%path, "print_letters_default") != -1 && %name $= "9.png")
		{
			$PrintCountIdx[9] = %i;
		}
	}
	if ($PrintCountIdx[%i] == -1)
	{
		%i = 0;
		for (%texture = strlwr(getPrintTexture(%i)); %texture !$= ""; %texture = strlwr(getPrintTexture(%i++)))
		{
			%path = filePath(%texture);
			%name = fileName(%texture);
			if (strstr(%path, "print_letters_") != -1 && %name $= "0.png")
			{
				$PrintCountIdx[0] = %i;
			}
			if (strstr(%path, "print_letters_") != -1 && %name $= "1.png")
			{
				$PrintCountIdx[1] = %i;
			}
			if (strstr(%path, "print_letters_") != -1 && %name $= "2.png")
			{
				$PrintCountIdx[2] = %i;
			}
			if (strstr(%path, "print_letters_") != -1 && %name $= "3.png")
			{
				$PrintCountIdx[3] = %i;
			}
			if (strstr(%path, "print_letters_") != -1 && %name $= "4.png")
			{
				$PrintCountIdx[4] = %i;
			}
			if (strstr(%path, "print_letters_") != -1 && %name $= "5.png")
			{
				$PrintCountIdx[5] = %i;
			}
			if (strstr(%path, "print_letters_") != -1 && %name $= "6.png")
			{
				$PrintCountIdx[6] = %i;
			}
			if (strstr(%path, "print_letters_") != -1 && %name $= "7.png")
			{
				$PrintCountIdx[7] = %i;
			}
			if (strstr(%path, "print_letters_") != -1 && %name $= "8.png")
			{
				$PrintCountIdx[8] = %i;
			}
			if (strstr(%path, "print_letters_") != -1 && %name $= "9.png")
			{
				$PrintCountIdx[9] = %i;
			}
		}
	}
	if ($PrintCountIdx[%i] == -1)
	{
		for (%i = 0; %i < 10; %i++)
		{
			$PrintCountIdx[%i] = 0;
		}
	}
}

function fxDTSBrick::getPrintCount(%obj)
{
	if ($PrintCountIdx[0] $= "")
	{
		generatePrintCountTable();
	}
	if (%obj.printCount $= "")
	{
		%texture = strlwr(getPrintTexture(%obj.getPrintID()));
		for (%i = 0; %i < 10; %i++)
		{
			if (getPrintTexture($PrintCountIdx[%i]) $= %texture)
			{
				%obj.printCount = %i;
				break;
			}
		}
	}
	%obj.printCount = mFloor(%obj.printCount);
	return %obj.printCount;
}

function fxDTSBrick::incrementPrintCount(%obj, %amt, %client)
{
	%obj.getPrintCount();
	%amt = mClamp(%amt, 1, 9);
	%obj.printCount += %amt;
	if (%obj.printCount > 9)
	{
		%obj.printCount -= 10;
		%obj.onPrintCountOverFlow(%client);
	}
	%obj.setPrint($PrintCountIdx[%obj.printCount]);
}

function fxDTSBrick::decrementPrintCount(%obj, %amt, %client)
{
	%obj.getPrintCount();
	%amt = mClamp(%amt, 1, 9);
	%obj.printCount -= %amt;
	if (%obj.printCount < 0)
	{
		%obj.printCount += 10;
		%obj.onPrintCountUnderFlow(%client);
	}
	%obj.setPrint($PrintCountIdx[%obj.printCount]);
}

function fxDTSBrick::setPrintCount(%obj, %count, %client)
{
	%obj.getPrintCount();
	%obj.printCount = %count;
	%obj.setPrint($PrintCountIdx[%obj.printCount]);
}

function fxDTSBrick::eventFloodCheck(%obj, %maxFlood)
{
	%maxFlood = mClamp(%maxFlood, 1, 10);
	%currTime = getSimTime();
	if (atoi(%currTime) - atoi(%obj.lastEventFloodTime) < 15)
	{
		%obj.eventFloodCount++;
		if (%obj.eventFloodCount > %maxFlood)
		{
			return 1;
		}
	}
	else
	{
		%obj.eventFloodCount = 0;
		%obj.lastEventFloodTime = %currTime;
	}
	return 0;
}

function GameConnection::eventFloodCheck(%obj, %maxFlood)
{
	%maxFlood = mClamp(%maxFlood, 1, 10);
	%currTime = getSimTime();
	if (atoi(%currTime) - atoi(%obj.lastEventFloodTime) < 15)
	{
		%obj.eventFloodCount++;
		if (%obj.eventFloodCount > %maxFlood)
		{
			return 1;
		}
	}
	else
	{
		%obj.eventFloodCount = 0;
		%obj.lastEventFloodTime = %currTime;
	}
	return 0;
}

registerOutputEvent("GameConnection", "CenterPrint", "string 200 156" TAB "int 1 10 3");
registerOutputEvent("GameConnection", "BottomPrint", "string 200 156" TAB "int 1 10 3");
registerOutputEvent("GameConnection", "ChatMessage", "string 200 176");
registerOutputEvent("GameConnection", "IncScore", "int -99999 99999 1");
registerOutputEvent("GameConnection", "playSound", "dataBlock Sound");
function GameConnection::CenterPrint(%client, %message, %time)
{
	if (isObject(%client))
	{
		%name = %client.getPlayerName();
		%message = strreplace(%message, "%1", %name);
	}
	commandToClient(%client, 'CenterPrint', %message, %time);
}

function GameConnection::BottomPrint(%client, %message, %time)
{
	if (isObject(%client))
	{
		%name = %client.getPlayerName();
		%message = strreplace(%message, "%1", %name);
	}
	commandToClient(%client, 'BottomPrint', %message, %time);
}

function GameConnection::ChatMessage(%client, %message)
{
	messageClient(%client, '', addTaggedString(%message), %client.getPlayerName(), %client.score);
}

function GameConnection::playSound(%client, %soundData)
{
	if (!isObject(%soundData))
	{
		return;
	}
	if (!isObject(%soundData.getDescription()))
	{
		return;
	}
	if (%soundData.getDescription().isLooping == 1)
	{
		return;
	}
	if (!%soundData.getDescription().is3D)
	{
		return;
	}
	%client.play2D(%soundData);
}

registerOutputEvent("Player", "Kill", "");
registerOutputEvent("Player", "BurnPlayer", "int 0 30 3");
registerOutputEvent("Player", "ClearBurn", "");
registerOutputEvent("Player", "SetVelocity", "vector 200", 0);
registerOutputEvent("Player", "AddVelocity", "vector 200");
registerOutputEvent("Player", "SetPlayerScale", "float 0.2 2 0.1 1");
registerOutputEvent("Player", "AddHealth", "int -1000 1000 25");
registerOutputEvent("Player", "SetHealth", "int 0 1000 100");
registerOutputEvent("Player", "ChangeDataBlock", "datablock PlayerData");
registerOutputEvent("Player", "Dismount", "");
registerOutputEvent("Player", "SpawnProjectile", "int -100 100 10" TAB "dataBlock ProjectileData" TAB "vector 200" TAB "float 0.2 2 0.1 1");
registerOutputEvent("Player", "SpawnExplosion", "dataBlock ProjectileData" TAB "float 0.2 2 0.1 1");
registerOutputEvent("Player", "ClearTools", "");
registerOutputEvent("Player", "InstantRespawn", "");
function Player::InstantRespawn(%player)
{
	%client = %player.client;
	if (!isObject(%client))
	{
		return;
	}
	%client.InstantRespawn();
}

function Player::ClearTools(%player)
{
	%client = %player.client;
	%maxTools = %player.getDataBlock().maxTools;
	for (%i = 0; %i < %maxTools; %i++)
	{
		%player.tool[%i] = 0;
		if (isObject(%client))
		{
			messageClient(%client, 'MsgItemPickup', "", %i, 0, 1);
		}
	}
	%player.unmountImage(0);
}

function Player::spawnExplosion(%player, %projectileData, %scale, %client)
{
	if (!isObject(%projectileData))
	{
		return;
	}
	%pos = %player.getHackPosition();
	%p = new Projectile()
	{
		dataBlock = %projectileData;
		initialVelocity = "0 0 1";
		initialPosition = %pos;
		sourceClient = %client;
		sourceObject = %player;
		client = %client;
	};
	if (!isObject(%p))
	{
		return;
	}
	MissionCleanup.add(%p);
	%p.setScale(%scale SPC %scale SPC %scale);
	%p.explode();
}

function Player::spawnProjectile(%player, %speed, %projectileData, %variance, %scale, %client)
{
	if (!isObject(%projectileData))
	{
		return;
	}
	%velocity = VectorScale(%player.getEyeVector(), %speed);
	%pos = %player.getEyePoint();
	%velx = getWord(%velocity, 0);
	%vely = getWord(%velocity, 1);
	%velz = getWord(%velocity, 2);
	%varx = getWord(%variance, 0);
	%vary = getWord(%variance, 1);
	%varz = getWord(%variance, 2);
	%x = (%velx + %varx * getRandom()) - %varx / 2;
	%y = (%vely + %vary * getRandom()) - %vary / 2;
	%z = (%velz + %varz * getRandom()) - %varz / 2;
	%muzzleVelocity = %x SPC %y SPC %z;
	%p = new Projectile()
	{
		dataBlock = %projectileData;
		initialVelocity = %muzzleVelocity;
		initialPosition = %pos;
		sourceClient = %client;
		sourceObject = %player;
		client = %client;
	};
	if (%p)
	{
		MissionCleanup.add(%p);
		%p.setScale(%scale SPC %scale SPC %scale);
	}
}

function Player::Dismount(%player, %client)
{
	%player.getDataBlock().doDismount(%player, 1);
}

function Player::SetPlayerScale(%player, %val)
{
	%player.setScale(%val SPC %val SPC %val);
}

function Player::AddVelocity(%player, %vector)
{
	%vel = %player.getVelocity();
	%vel = VectorAdd(%vel, %vector);
	%player.setVelocity(%vel);
}

function Player::ChangeDataBlock(%player, %data, %client)
{
	if (!isObject(%data))
	{
		return;
	}
	%oldData = %player.getDataBlock();
	if (%data.getId() == %oldData.getId())
	{
		return;
	}
	if (%oldData.canRide && !%data.canRide)
	{
		if (%player.getObjectMount())
		{
			%player.getDataBlock().doDismount(%player, 1);
		}
	}
	%player.setDataBlock(%data);
	%image = %player.getMountedImage(0);
	if (%image)
	{
		if (%image.getId() == %oldData.brickImage.getId() && %oldData.brickImage.getId() != %data.brickImage.getId())
		{
			%player.mountImage(%data.brickImage, 0);
		}
	}
	if (isObject(%player.client))
	{
		commandToClient(%player.client, 'ShowEnergyBar', %data.showEnergyBar);
	}
}

function Player::AddHealth(%player, %amt)
{
	if (%player.getDamagePercent() >= 1)
	{
		return;
	}
	if (%amt > 0)
	{
		%player.setDamageLevel(%player.getDamageLevel() - %amt);
	}
	else
	{
		%player.Damage(%player, %player.getPosition(), %amt * -1, $DamageType::Default);
	}
}

function Player::SetHealth(%player, %health)
{
	if (%player.getDamagePercent() >= 1)
	{
		return;
	}
	if (%health <= 0)
	{
		%player.Damage(%player, %player.getPosition, %player.getDataBlock().maxDamage, $DamageType::Default);
	}
	else
	{
		%damageLevel = %player.getDataBlock().maxDamage - %health;
		if (%damageLevel < 0)
		{
			%damageLevel = 0;
		}
		%player.setDamageLevel(%damageLevel);
	}
}

registerOutputEvent("MiniGame", "ChatMsgAll", "string 200 176");
registerOutputEvent("MiniGame", "CenterPrintAll", "string 200 156" TAB "int 1 10 3");
registerOutputEvent("MiniGame", "BottomPrintAll", "string 200 156" TAB "int 1 10 3");
registerOutputEvent("MiniGame", "Reset", "");
registerOutputEvent("MiniGame", "RespawnAll", "");
registerOutputEvent("Projectile", "Explode", "", 0);
registerOutputEvent("Projectile", "Delete", "", 0);
registerOutputEvent("Projectile", "Bounce", "float 0 2 0.1 0.5");
registerOutputEvent("Projectile", "Redirect", "vector 200" TAB "bool");
function Projectile::Bounce(%obj, %factor, %client)
{
	%vel = %obj.getLastImpactVelocity();
	%norm = %obj.getLastImpactNormal();
	%bounceVel = VectorSub(%vel, VectorScale(%norm, VectorDot(%vel, %norm) * 2));
	%bounceVel = VectorScale(%bounceVel, %factor);
	if (VectorLen(%bounceVel) > 200)
	{
		%bounceVel = VectorScale(VectorNormalize(%bounceVel), 200);
	}
	%p = new Projectile()
	{
		dataBlock = %obj.getDataBlock();
		initialPosition = %obj.getLastImpactPosition();
		initialVelocity = %bounceVel;
		sourceObject = 0;
		sourceSlot = %obj.sourceSlot;
		client = %obj.client;
	};
	if (%p)
	{
		MissionCleanup.add(%p);
		%p.setScale(%obj.getScale());
		%p.spawnBrick = %obj.spawnBrick;
	}
	%obj.delete();
}

function Projectile::Redirect(%obj, %vector, %normalized, %client)
{
	%vel = %obj.getLastImpactVelocity();
	if (%normalized)
	{
		%vec = VectorNormalize(%vector);
		%len = VectorLen(%vel);
		%bounceVel = VectorScale(%vec, %len);
	}
	else
	{
		%bounceVel = %vector;
	}
	if (VectorLen(%bounceVel) > 200)
	{
		%bounceVel = VectorScale(VectorNormalize(%bounceVel), 200);
	}
	%p = new Projectile()
	{
		dataBlock = %obj.getDataBlock();
		initialPosition = %obj.getLastImpactPosition();
		initialVelocity = %bounceVel;
		sourceObject = 0;
		sourceSlot = %obj.sourceSlot;
		client = %obj.client;
	};
	if (!%p)
	{
		MissionCleanup.add(%p);
		%p.setScale(%obj.getScale());
	}
	%obj.delete();
}

function Vehicle::onDriverLeave(%obj, %player)
{
	%obj.getDataBlock().onDriverLeave(%obj, %player);
}

function Vehicle::onRemove(%obj)
{
}

function Vehicle::onActivate(%vehicle, %activatingObj, %activatingClient, %pos, %vec)
{
	if (VectorLen(%vehicle.getVelocity()) > 2)
	{
		return;
	}
	%client = %activatingClient;
	if (!isObject(%client))
	{
		return;
	}
	%player = %activatingObj;
	if (!isObject(%player))
	{
		return;
	}
	%doFlip = 0;
	if (isObject(%vehicle.spawnBrick))
	{
		%vehicleOwner = findClientByBL_ID(%vehicle.spawnBrick.getGroup().bl_id);
	}
	else
	{
		%vehicleOwner = 0;
	}
	if (isObject(%vehicleOwner))
	{
		if (getTrustLevel(%player, %vehicle) >= $TrustLevel::VehicleTurnover)
		{
			%doFlip = 1;
		}
	}
	else
	{
		%doFlip = 1;
	}
	if (miniGameCanUse(%player, %vehicle) == 1)
	{
		%doFlip = 1;
	}
	if (miniGameCanUse(%player, %vehicle) == 0)
	{
		%doFlip = 0;
	}
	if (%doFlip)
	{
		%impulse = VectorNormalize(%vec);
		%impulse = VectorAdd(%impulse, "0 0 1");
		%impulse = VectorNormalize(%impulse);
		%force = %vehicle.getDataBlock().mass * 5;
		%scaleRatio = getWord(%player.getScale(), 2) / getWord(%vehicle.getScale(), 2);
		%force *= %scaleRatio;
		%impulse = VectorScale(%impulse, %force);
		%vehicle.applyImpulse(%pos, %impulse);
	}
}

function Vehicle::OnCollision()
{
	echo("vehicle on collision");
}

function VehicleData::OnCollision()
{
	echo("vehicledata on collision");
}

function WheeledVehicleData::create(%block)
{
	%obj = new WheeledVehicle()
	{
		dataBlock = %block;
	};
	return %obj;
}

datablock WheeledVehicleTire(emptyTire)
{
	shapeFile = "base/data/shapes/empty.dts";
	mass = 10;
	radius = 1;
	staticFriction = 5;
	kineticFriction = 5;
	restitution = 0.5;
	lateralForce = 18000;
	lateralDamping = 4000;
	lateralRelaxation = 0.01;
	longitudinalForce = 14000;
	longitudinalDamping = 2000;
	longitudinalRelaxation = 0.01;
};
datablock WheeledVehicleSpring(emptySpring)
{
	length = 0.3;
	force = 0;
	damping = 0;
	antiSwayForce = 0;
};
function WheeledVehicleData::onAdd(%this, %obj)
{
	for (%i = 0; %i < %this.numWheels; %i++)
	{
		%obj.setWheelTire(%i, %this.defaultTire);
		%obj.setWheelSpring(%i, %this.defaultSpring);
	}
	if (%this.numWheels == 0)
	{
	}
	else if (%this.numWheels == 1)
	{
		%obj.setWheelSteering(0, 1);
		%obj.setWheelPowered(0, 1);
	}
	else if (%this.numWheels == 2)
	{
		%obj.setWheelSteering(0, 1);
		%obj.setWheelSteering(1, 1);
		%obj.setWheelPowered(0, 1);
		%obj.setWheelPowered(1, 1);
	}
	else if (%this.numWheels == 3)
	{
		%obj.setWheelSteering(0, 1);
		%obj.setWheelPowered(1, 1);
		%obj.setWheelPowered(2, 1);
	}
	else if (%this.numWheels == 4)
	{
		%obj.setWheelSteering(0, 1);
		%obj.setWheelSteering(1, 1);
		%obj.setWheelPowered(2, 1);
		%obj.setWheelPowered(3, 1);
	}
	else if (%this.numWheels == 5)
	{
		%obj.setWheelSteering(0, 1);
		%obj.setWheelPowered(1, 1);
		%obj.setWheelPowered(2, 1);
		%obj.setWheelPowered(3, 1);
		%obj.setWheelPowered(4, 1);
	}
	else if (%this.numWheels == 6)
	{
		%obj.setWheelSteering(0, 1);
		%obj.setWheelSteering(1, 1);
		%obj.setWheelPowered(2, 1);
		%obj.setWheelPowered(3, 1);
		%obj.setWheelPowered(4, 1);
		%obj.setWheelPowered(5, 1);
	}
	else
	{
		%obj.setWheelSteering(0, 1);
		%obj.setWheelSteering(1, 1);
		%obj.setWheelPowered(2, 1);
		%obj.setWheelPowered(3, 1);
		%obj.setWheelPowered(4, 1);
		%obj.setWheelPowered(5, 1);
	}
	%obj.creationTime = getSimTime();
}

function FlyingVehicleData::OnCollision(%this, %obj, %col, %vec, %speed)
{
	WheeledVehicleData::OnCollision(%this, %obj, %col, %vec, %speed);
}

function WheeledVehicleData::OnCollision(%this, %obj, %col, %vec, %speed)
{
	if (%obj.getDamageState() $= "Dead")
	{
		return;
	}
	if (%col.getDamagePercent() >= 1)
	{
		return;
	}
	%runOver = 0;
	if (isObject(%obj.client))
	{
		if (%col.client == %obj.client)
		{
			return;
		}
	}
	%canUse = 0;
	if (isObject(%obj.spawnBrick))
	{
		%vehicleOwner = findClientByBL_ID(%obj.spawnBrick.getGroup().bl_id);
	}
	else
	{
		%vehicleOwner = 0;
	}
	if (isObject(%vehicleOwner))
	{
		if (getTrustLevel(%col, %obj) >= $TrustLevel::RideVehicle)
		{
			%canUse = 1;
		}
	}
	else
	{
		%canUse = 1;
	}
	if (miniGameCanUse(%col, %obj) == 1)
	{
		%canUse = 1;
	}
	if (miniGameCanUse(%col, %obj) == 0)
	{
		%canUse = 0;
	}
	if (miniGameCanDamage(%col, %obj) == 1)
	{
		%canDamage = 1;
	}
	else
	{
		%canDamage = 0;
	}
	%minSpeed = mClampF(%this.minRunOverSpeed, $Game::DefaultMinRunOverSpeed, 999);
	if (!isObject(%obj.getControllingObject()))
	{
		%minSpeed += 2;
	}
	%relativeSpeed = VectorLen(VectorSub(%obj.getVelocity(), %col.getVelocity()));
	if (%col.getDataBlock().canRide && %this.rideable && %this.nummountpoints > 0)
	{
		if (getSimTime() - %col.lastMountTime > $Game::MinMountTime)
		{
			%colZpos = getWord(%col.getPosition(), 2);
			%objZpos = getWord(%obj.getPosition(), 2);
			if (%colZpos > %objZpos + 0.2)
			{
				if (%canUse)
				{
					for (%i = 0; %i < %this.nummountpoints; %i++)
					{
						%blockingObj = %obj.getMountNodeObject(%i);
						if (isObject(%blockingObj))
						{
							if (!%blockingObj.getDataBlock().rideable)
							{
								continue;
							}
							if (%blockingObj.getMountedObject(0))
							{
								continue;
							}
							%blockingObj.mountObject(%col, 0);
							if (%blockingObj.getControllingClient() == 0)
							{
								%col.setControlObject(%blockingObj);
							}
						}
						else
						{
							%obj.mountObject(%col, %i);
							if (%i == 0)
							{
								if (%obj.getControllingClient() == 0)
								{
									%col.setControlObject(%obj);
								}
							}
							break;
						}
					}
				}
				else
				{
					%ownerName = %obj.spawnBrick.getGroup().name;
					%msg = %ownerName @ " does not trust you enough to do that";
					if ($lastError == $LastError::Trust)
					{
						%msg = %ownerName @ " does not trust you enough to ride.";
					}
					else if ($lastError == $LastError::MiniGameDifferent)
					{
						if (isObject(%col.client.miniGame))
						{
							%msg = "This vehicle is not part of the mini-game.";
						}
						else
						{
							%msg = "This vehicle is part of a mini-game.";
						}
					}
					else if ($lastError == $LastError::MiniGameNotYours)
					{
						%msg = "You do not own this vehicle.";
					}
					else if ($lastError == $LastError::NotInMiniGame)
					{
						%msg = "This vehicle is not part of the mini-game.";
					}
					commandToClient(%col.client, 'CenterPrint', %msg, 1);
					%runOver = 1;
				}
			}
			else
			{
				%runOver = 1;
			}
		}
	}
	else
	{
		%runOver = 1;
	}
	if (%canDamage)
	{
		if (%runOver)
		{
			if (%col.getType() & $TypeMasks::PlayerObjectType)
			{
				%vehicleSpeed = VectorLen(%obj.getVelocity());
				if (%vehicleSpeed > %minSpeed)
				{
					%damageScale = %this.runOverDamageScale;
					if (%damageScale $= "")
					{
						%damageScale = $Game::DefaultRunOverDamageScale;
					}
					%damageType = %this.damageType;
					if (%damageType $= "")
					{
						%damageType = $DamageType::Vehicle;
					}
					%damageAmt = %vehicleSpeed * %damageScale;
					%col.Damage(%obj, %pos, %damageAmt, %damageType);
				}
			}
		}
		%pushScale = %this.runOverPushScale;
		if (%pushScale $= "")
		{
			%pushScale = $Game::DefaultRunOverPushScale;
		}
		%pushVec = %obj.getVelocity();
		%pushVec = VectorScale(%pushVec, %pushScale);
		%col.setVelocity(%pushVec);
	}
}

function VehicleData::onDriverLeave(%this, %obj)
{
}

function WheeledVehicleData::onDriverLeave(%this, %obj)
{
}

function WheeledVehicleData::onDamage(%this, %obj)
{
}

function WheeledVehicleData::Damage(%this, %obj, %sourceObject, %position, %damage, %damageType)
{
	if (%obj.getDamageState() !$= "Enabled")
	{
		return;
	}
	if ($Damage::VehicleDamageScale[%damageType] !$= "")
	{
		%damage *= $Damage::VehicleDamageScale[%damageType];
	}
	%scale = getWord(%obj.getScale(), 2);
	%damage = %damage / %scale;
	if (%damage == 0)
	{
		return;
	}
	if (getSimTime() - %obj.creationTime < $Game::VehicleInvulnerabilityTime)
	{
		return;
	}
	%obj.applyDamage(%damage);
	if (isObject(%sourceObject.client))
	{
		%obj.lastDamageClient = %sourceObject.client;
	}
	if (%obj.getDamagePercent() >= 1)
	{
		%obj.setDamageState("Disabled");
		%obj.setNodeColor("ALL", "0 0 0 1");
		%pos = VectorAdd(%obj.getPosition(), "0 0" SPC %this.initialExplosionOffset);
		if (isObject(%obj.lastDamageClient))
		{
			%client = %obj.lastDamageClient;
		}
		else
		{
			%client = %obj.spawnBrick.getGroup().client;
		}
		if (%this.initialExplosionScale $= "")
		{
			%this.initialExplosionScale = 1;
		}
		%this.initialExplosionScale = mClampF(%this.initialExplosionScale, 0.2, 5);
		%finalScale = getWord(%obj.getScale(), 2) * %this.initialExplosionScale;
		if (isObject(%this.initialExplosionProjectile))
		{
			%projectileData = %this.initialExplosionProjectile;
		}
		else
		{
			%projectileData = vehicleExplosionProjectile;
		}
		%p = new Projectile()
		{
			dataBlock = %this.initialExplosionProjectile;
			initialPosition = %pos;
			sourceClient = %client;
		};
		%p.setScale(%finalScale SPC %finalScale SPC %finalScale);
		%p.client = %client;
		MissionCleanup.add(%p);
		for (%i = 0; %i < %this.numWheels; %i++)
		{
			%obj.setWheelTire(%i, emptyTire);
			%obj.setWheelSpring(%i, emptySpring);
		}
		%obj.schedule(%this.burnTime, finalExplosion);
		%mg = getMiniGameFromObject(%sourceObject);
		if (isObject(%mg))
		{
			%respawnTime = %mg.vehicleReSpawnTime;
		}
		else
		{
			%respawnTime = 0;
		}
		if (%respawnTime < %this.burnTime)
		{
			%respawnTime = %this.burnTime;
		}
		%respawnTime += 100;
		%obj.spawnBrick.spawnVehicle(%respawnTime);
		if (isObject(%this.burnDataBlock))
		{
			%obj.setDataBlock(%this.burnDataBlock);
		}
	}
}

function FlyingVehicleData::Damage(%this, %obj, %sourceObject, %position, %damage, %damageType)
{
	if (%obj.getDamageState() !$= "Enabled")
	{
		return;
	}
	if ($Damage::VehicleDamageScale[%damageType] !$= "")
	{
		%damage *= $Damage::VehicleDamageScale[%damageType];
	}
	%scale = getWord(%obj.getScale(), 2);
	%damage = %damage / %scale;
	if (%damage == 0)
	{
		return;
	}
	if (getSimTime() - %obj.creationTime < 1000)
	{
		return;
	}
	%obj.applyDamage(%damage);
	if (isObject(%sourceObject.client))
	{
		%obj.lastDamageClient = %sourceObject.client;
	}
	if (%obj.getDamagePercent() >= 1)
	{
		%obj.setDamageState("Disabled");
		%obj.setNodeColor("ALL", "0 0 0 1");
		%pos = VectorAdd(%obj.getPosition(), "0 0" SPC %this.initialExplosionOffset);
		if (isObject(%obj.lastDamageClient))
		{
			%client = %obj.lastDamageClient;
		}
		else
		{
			%client = %obj.spawnBrick.getGroup().client;
		}
		if (isObject(%this.initialExplosionProjectile))
		{
			%projectileData = %this.initialExplosionProjectile;
		}
		else
		{
			%projectileData = vehicleExplosionProjectile;
		}
		if (%this.initialExplosionScale $= "")
		{
			%this.initialExplosionScale = 1;
		}
		%this.initialExplosionScale = mClampF(%this.initialExplosionScale, 0.2, 5);
		%finalScale = getWord(%obj.getScale(), 2) * %this.initialExplosionScale;
		%p = new Projectile()
		{
			dataBlock = %projectileData;
			initialPosition = %pos;
			sourceClient = %client;
		};
		%p.setScale(%finalScale SPC %finalScale SPC %finalScale);
		%p.client = %client;
		MissionCleanup.add(%p);
		%obj.schedule(%this.burnTime, finalExplosion);
		%mg = getMiniGameFromObject(%obj);
		if (isObject(%mg))
		{
			%obj.spawnBrick.spawnVehicle(%mg.vehicleReSpawnTime);
		}
		else
		{
			%obj.spawnBrick.spawnVehicle(0);
		}
		if (isObject(%this.burnDataBlock))
		{
			%obj.setDataBlock(%this.burnDataBlock);
		}
	}
}

function Vehicle::finalExplosion(%obj)
{
	for (%data = %obj.getDataBlock(); %obj.getMountedObjectCount() > 0; %obj.getMountedObject(0).unmount())
	{
	}
	%pos = VectorAdd(%obj.getPosition(), "0 0" SPC %data.finalExplosionOffset);
	if (isObject(%obj.lastDamageClient))
	{
		%client = %obj.lastDamageClient;
	}
	else
	{
		%client = %obj.spawnBrick.getGroup().client;
	}
	if (%data.finalExplosionScale $= "")
	{
		%data.finalExplosionScale = 1;
	}
	%data.finalExplosionScale = mClampF(%data.finalExplosionScale, 0.2, 5);
	%finalScale = getWord(%obj.getScale(), 2) * %data.finalExplosionScale;
	if (isObject(%data.finalExplosionProjectile))
	{
		%projectileData = %data.finalExplosionProjectile;
	}
	else
	{
		%projectileData = vehicleFinalExplosionProjectile;
	}
	%p = new Projectile()
	{
		dataBlock = %projectileData;
		initialPosition = %pos;
		sourceClient = %client;
	};
	%p.setScale(%finalScale SPC %finalScale SPC %finalScale);
	%p.upVector = %obj.getUpVector();
	%p.client = %client;
	MissionCleanup.add(%p);
	%obj.delete();
}

function Vehicle::teleportEffect(%vehicle)
{
	%p = new Projectile()
	{
		dataBlock = playerTeleportProjectile;
		initialPosition = %vehicle.getTransform();
	};
	if (%p)
	{
		%scale = %vehicle.getScale();
		%minZ = getWord(%vehicle.getWorldBox(), 2);
		%maxZ = getWord(%vehicle.getWorldBox(), 5);
		%zSize = %maxZ - %minZ;
		%ratio = %zSize / 2.65;
		%scale = VectorScale(%vehicle.getScale(), %ratio);
		%p.setScale(%scale);
		MissionCleanup.add(%p);
	}
}

function VehicleSpawnMarker::onAdd(%obj)
{
	%vehicleClass = %obj.vehicleDataBlock.getClassName();
	if (%vehicleClass $= "WheeledVehicleData" || %vehicleClass $= "HoverVehicleData" || %vehicleClass $= "FlyingVehicleData")
	{
		if (!$Server::LAN)
		{
			%brickGroup = %obj.brick.getGroup();
			%brickGroup.numPhysVehicles++;
		}
		$Server::numPhysVehicles++;
	}
	else if (%vehicleClass $= "PlayerData" || %vehicleClass $= "AIPlayerData")
	{
		if (!$Server::LAN)
		{
			%brickGroup = %obj.brick.getGroup();
			%brickGroup.numPlayerVehicles++;
		}
		$Server::numPlayerVehicles++;
	}
}

function VehicleSpawnMarker::onRemove(%obj)
{
	%vehicleClass = %obj.vehicleDataBlock.getClassName();
	if (%vehicleClass $= "WheeledVehicleData" || %vehicleClass $= "HoverVehicleData" || %vehicleClass $= "FlyingVehicleData")
	{
		if (!$Server::LAN)
		{
			%brickGroup = %obj.brick.getGroup();
			%brickGroup.numPhysVehicles--;
		}
		$Server::numPhysVehicles--;
	}
	else if (%vehicleClass $= "PlayerData" || %vehicleClass $= "AIPlayerData")
	{
		if (!$Server::LAN)
		{
			%brickGroup = %obj.brick.getGroup();
			%brickGroup.numPlayerVehicles--;
		}
		$Server::numPlayerVehicles--;
	}
}

datablock PrecipitationData(Snow)
{
	type = 1;
	materialList = "~/data/specialfx/snowflakes.dml";
	sizeX = 0.1;
	sizeY = 0.1;
	movingBoxPer = 0.35;
	divHeightVal = 1.5;
	sizeBigBox = 1;
	topBoxSpeed = 80;
	frontBoxSpeed = 80;
	topBoxDrawPer = 0.5;
	bottomDrawHeight = 40;
	skipIfPer = -0.3;
	bottomSpeedPer = 1;
	frontSpeedPer = 1.5;
	frontRadiusPer = 0.5;
};
datablock PrecipitationData(SnowA)
{
	dropTexture = "~/data/specialfx/snow";
	dropSize = 0.25;
	splashSize = 0.2;
	useTrueBillboards = 1;
	splashMS = 250;
};
datablock ParticleEmitterNodeData(GenericEmitterNode)
{
	timeMultiple = 1;
};
datablock ParticleEmitterNodeData(HalfEmitterNode)
{
	timeMultiple = 1 / 2;
};
datablock ParticleEmitterNodeData(FifthEmitterNode)
{
	timeMultiple = 1 / 5;
};
datablock ParticleEmitterNodeData(TenthEmitterNode)
{
	timeMultiple = 1 / 10;
};
datablock ParticleEmitterNodeData(TwentiethEmitterNode)
{
	timeMultiple = 1 / 20;
};
datablock ParticleEmitterNodeData(FourtiethEmitterNode)
{
	timeMultiple = 1 / 40;
};
datablock StaticShapeData(LCD)
{
	category = "Misc";
	class = "LCD";
	shapeFile = "~/data/shapes/environment/LCD.dts";
};
function LCD::onAdd(%this, %obj)
{
	%obj.setSkinName(%obj.skinName);
	%obj.playThread(0, time0);
	%obj.time = 0;
	if (%obj.getName() $= "minutes")
	{
		%obj.schedule(%obj.Delay, advanceTime);
	}
}

function StaticShape::advanceTime(%obj)
{
	%obj.time++;
	%minutes = %obj.time % 10;
	%tenMinutes = mFloor(%obj.time / 10) % 6;
	%hours = mFloor(%obj.time / 60) % 10;
	%tenHours = mFloor(%obj.time / 600) % 10;
	eval("%obj.playthread(0, time" @ %minutes @ ");");
	eval("tenMinutes.playThread(0, time" @ %tenMinutes @ ");");
	eval("hours.playThread(0, time" @ %hours @ ");");
	eval("tenHours.playThread(0, time" @ %tenHours @ ");");
	%obj.schedule(%obj.Delay, advanceTime);
	if (%obj.time == 6000)
	{
		explodeClock();
	}
}

function explodeClock()
{
	minutes.schedule(1000, clockExplode);
	tenMinutes.schedule(1200, clockExplode);
	hours.schedule(1400, clockExplode);
	tenHours.schedule(1600, clockExplode);
}

function StaticShape::clockExplode(%obj)
{
	%p = new Projectile()
	{
		dataBlock = clockProjectile;
		initialVelocity = "0 0 0";
		initialPosition = %obj.getPosition();
		sourceObject = %obj;
	};
	MissionCleanup.add(%p);
}

datablock StaticShapeData(LCDColon)
{
	category = "Misc";
	shapeFile = "~/data/shapes/environment/LCDColon.dts";
};
function LCDColon::onAdd(%this, %obj)
{
	%obj.setSkinName(%obj.skinName);
	%obj.playThread(0, blink);
}

datablock ParticleData(ClockExplosionSmoke)
{
	textureName = "~/data/particles/cloud";
	dragCoeffiecient = 5;
	gravityCoefficient = -0.5;
	inheritedVelFactor = 0.25;
	constantAcceleration = -0.3;
	lifetimeMS = 1800;
	lifetimeVarianceMS = 300;
	useInvAlpha = 1;
	spinRandomMin = -80;
	spinRandomMax = 80;
	colors[0] = "1.0 1.0 0.26 1.0";
	colors[1] = "0.5 0.2 0.2 1.0";
	colors[2] = "0.0 0.0 0.0 0.0";
	sizes[0] = 3.5;
	sizes[1] = 6;
	sizes[2] = 2;
	times[0] = 0;
	times[1] = 0.05;
	times[2] = 1;
};
datablock ParticleEmitterData(ClockExplosionSmokeEmitter)
{
	ejectionPeriodMS = 10;
	periodVarianceMS = 0;
	ejectionVelocity = 3;
	velocityVariance = 0.5;
	thetaMin = 0;
	thetaMax = 180;
	lifetimeMS = 250;
	particles = "ClockExplosionSmoke";
};
datablock ParticleData(ClockExplosionSparks)
{
	textureName = "~/data/particles/chunk";
	dragCoefficient = 0;
	gravityCoefficient = 5;
	inheritedVelFactor = 0.2;
	constantAcceleration = 0;
	lifetimeMS = 500;
	lifetimeVarianceMS = 350;
	colors[0] = "0.60 0.40 0.30 1.0";
	colors[1] = "0.60 0.40 0.30 1.0";
	colors[2] = "1.0 0.40 0.30 0.0";
	sizes[0] = 0.5;
	sizes[1] = 0.25;
	sizes[2] = 0.25;
	times[0] = 0;
	times[1] = 0.5;
	times[2] = 1;
};
datablock ParticleEmitterData(ClockExplosionSparkEmitter)
{
	ejectionPeriodMS = 3;
	periodVarianceMS = 0;
	ejectionVelocity = 35;
	velocityVariance = 1;
	ejectionOffset = 0;
	thetaMin = 0;
	thetaMax = 180;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvances = 0;
	orientParticles = 1;
	lifetimeMS = 100;
	particles = "ClockExplosionSparks";
};
datablock ExplosionData(ClockExplosion)
{
	lifetimeMS = 1200;
	particleEmitter = ClockExplosionSmokeEmitter;
	particleDensity = 750;
	particleRadius = 2;
	emitter[0] = ClockExplosionSparkEmitter;
	shakeCamera = 1;
	camShakeFreq = "10.0 11.0 10.0";
	camShakeAmp = "1.0 1.0 1.0";
	camShakeDuration = 0.5;
	camShakeRadius = 10;
	impulseRadius = 10;
	impulseForce = 40;
	lightStartRadius = 6;
	lightEndRadius = 3;
	lightStartColor = "0.5 0.5 0";
	lightEndColor = "0 0 0";
};
datablock ProjectileData(clockProjectile)
{
	projectileShapeName = "";
	directDamage = 22;
	radiusDamage = 22;
	damageRadius = 3.5;
	Explosion = ClockExplosion;
	muzzleVelocity = 20;
	velInheritFactor = 1;
	armingDelay = 0;
	lifetime = 1000;
	fadeDelay = 4000;
	explodeOnDeath = 1;
	bounceElasticity = 0.2;
	bounceFriction = 0;
	bounceAngle = 0.6;
	isBallistic = 1;
	gravityMod = 0.8;
	hasLight = 0;
	lightRadius = 4;
	lightColor = "0.5 0.5 0.25";
	hasWaterLight = 1;
	waterLightColor = "0 0.5 0.5";
};
function changeMap(%mapName)
{
	if (stripos(%mapName, "Add-Ons/Map_") == -1)
	{
		%filename = "Add-Ons/Map_" @ %mapName @ "/" @ %mapName @ ".mis";
	}
	else
	{
		%filename = %mapName;
	}
	echo(" changing map to " @ %filename);
	if (!isValidMap(%filename))
	{
		error("ERROR: changeMap() - map \"" @ %filename @ "\" is an invalid map file");
		%filename = $Server::MissionFile;
	}
	$Game::MissionCleaningUp = 0;
	endGame();
	loadMission(%filename);
}

function announce(%text)
{
	MessageAll('', "\c3" @ %text);
}

function kickBLID(%blid)
{
	if (%blid $= "")
	{
		return;
	}
	%victim = findClientByBL_ID(%blid);
	if (!isObject(%victim))
	{
		echo("kickBLID() - could not find client for BL_ID \"" @ %blid @ "\"");
		return;
	}
	serverCmdKick(0, %victim);
}

function banBLID(%victimBL_ID, %banTime, %reason)
{
	if (%victimBL_ID $= "" || %banTime $= "")
	{
		echo("Usage: banBLID(victimBL_ID, banTime in minutes (-1 for permanent ban), reason);");
		return;
	}
	serverCmdBan(0, 0, %victimBL_ID, %banTime, %reason);
}

function talk(%text)
{
	if (%text !$= "")
	{
		MessageAll('', "\c5CONSOLE: \c6" @ %text);
	}
}

function shutDown(%text)
{
	%count = ClientGroup.getCount();
	for (%clientIndex = 0; %clientIndex < %count; %clientIndex++)
	{
		%cl = ClientGroup.getObject(%clientIndex);
		if (!%cl.isLocal())
		{
			if (%text !$= "")
			{
				%cl.schedule(10, delete, %text);
				continue;
			}
			%cl.schedule(10, delete, "The server was shut down.");
		}
	}
}

function listClients()
{
	%count = ClientGroup.getCount();
	echo("  ", %count, " Clients");
	echo("------------------------------------------------------------");
	%clientIndex = 0;
	for (%clientIndex = 0; %clientIndex < %count; %clientIndex++)
	{
		%cl = ClientGroup.getObject(%clientIndex);
		%idFillLen = 8 - strlen(%cl);
		%nameFillLen = 24 - strlen(%cl.getSimpleName());
		%zoneName = %cl.keyID;
		echo(%cl @ makePadString(" ", %idFillLen) @ %cl.getSimpleName() @ makePadString(" ", %nameFillLen) @ %zoneName @ " " @ %cl.getAddress() @ " " @ %cl.getBLID() @ "     " @ %cl.getPing());
	}
	echo("------------------------------------------------------------");
}

function profileJazz()
{
	if (isEventPending($profileJazzEvent))
	{
		cancel($profileJazzEvent);
	}
	profilerDump();
	profilerEnable(1);
	schedule(500, 0, profilerReset);
	$profileJazzEvent = schedule(1000 * 60 * 5, 0, profileJazz);
}

function transmitDataBlocks(%text)
{
	MessageAll('', "Transmitting Datablocks...");
	%count = ClientGroup.getCount();
	for (%clientIndex = 0; %clientIndex < %count; %clientIndex++)
	{
		%cl = ClientGroup.getObject(%clientIndex);
		%cl.transmitDataBlocks(1);
	}
}

function E(%val)
{
	%filePattern = "base/server/*" @ %val @ "*.cs";
	for (%file = findFirstFile(%filePattern); %file !$= ""; %file = findNextFile(%filePattern))
	{
		exec(%file);
	}
	%filePattern = "base/server/*/" @ %val @ "*.cs";
	for (%file = findFirstFile(%filePattern); %file !$= ""; %file = findNextFile(%filePattern))
	{
		exec(%file);
	}
	%filePattern = "add-ons/*/" @ %val @ "*.cs";
	for (%file = findFirstFile(%filePattern); %file !$= ""; %file = findNextFile(%filePattern))
	{
		exec(%file);
	}
	%filePattern = "add-ons/" @ %val @ "*.cs";
	for (%file = findFirstFile(%filePattern); %file !$= ""; %file = findNextFile(%filePattern))
	{
		exec(%file);
	}
}

function dumpCRCValues()
{
	if (getBuildString() !$= "Debug" && getBuildString() !$= "Release")
	{
		return;
	}
	%file = new FileObject();
	%file.openForWrite("base/Protection.txt");
	%file.writeLine("Main.cs");
	%file.writeLine("  use this around line 377 of main.cc");
	%file.writeLine("------------------------");
	%file.writeLine("  CRC: " @ getFileCRC("main.cs"));
	%file.writeLine("  LEN: " @ getFileLength("main.cs"));
	%file.writeLine("");
	%file.writeLine("");
	%file.writeLine("DSO CRCs");
	%file.writeLine("------------------------");
	%pattern = "*.dso";
	for (%filename = findFirstFile(%pattern); %filename !$= ""; %filename = findNextFile(%pattern))
	{
		%file.writeLine(%filename @ " " @ getFileCRC(%filename));
	}
	%file.writeLine("");
	%file.writeLine("//Put this code into compiledEval.cc around line 518 in the #ifdef TORQUE_FUNC_PROTECT block");
	%file.writeLine("//Wrench Event Saving");
	%file.writeLine("//------------------------");
	writeFuncOffCheck(%file, "", "serverCmdRequestEventTables");
	writeFuncOffCheck(%file, "", "ServerCmdRequestExtendedBrickInfo");
	writeFuncOffCheck(%file, "SimObject", "serializeEventToString", 1);
	writeFuncOffCheck(%file, "GameConnection", "TransmitExtendedBrickInfo");
	%file.writeLine("");
	%file.writeLine("//Authentication");
	%file.writeLine("//------------------------");
	writeFuncOffCheck(%file, "", "auth_Init_Client");
	writeFuncOffCheck(%file, "authTCPobj_Client", "onConnected");
	writeFuncOffCheck(%file, "authTCPobj_Client", "onLine");
	writeFuncOffCheck(%file, "", "auth_Init_Server");
	writeFuncOffCheck(%file, "authTCPobj_Server", "onConnected");
	writeFuncOffCheck(%file, "authTCPobj_Server", "onLine");
	%file.writeLine("");
	%file.writeLine("//Things that call the auth functions");
	%file.writeLine("//------------------------");
	writeFuncOffCheck(%file, "mainMenuGui", "onWake");
	writeFuncOffCheck(%file, "", "WebCom_PostServer");
	writeFuncOffCheck(%file, "postServerTCPObj", "onConnected");
	writeFuncOffCheck(%file, "postServerTCPObj", "onLine");
	%file.writeLine("");
	%file.writeLine("//Client connection functions - things you might modify to let pirates join your server");
	%file.writeLine("//------------------------");
	writeFuncOffCheck(%file, "GameConnection", "onConnect");
	writeFuncOffCheck(%file, "GameConnection", "authCheck");
	%file.writeLine("");
	%file.writeLine("//Crap-On list update helper function");
	%file.writeLine("//------------------------");
	writeFuncOffCheck(%file, "", "appendCrapOnCache");
	%file.writeLine("//Helper functions for entering your key in a dedicated server");
	%file.writeLine("//------------------------");
	writeFuncOffCheck(%file, "", "dedicatedKeyCheck");
	writeFuncOffCheck(%file, "", "dedicatedKeyPrompt");
	writeFuncOffCheck(%file, "", "setKey");
	%file.writeLine("");
	%file.close();
	%file.delete();
}

function writeFuncOffCheck(%file, %fnNamespace, %fnName, %protectANY)
{
	if (getBuildString() !$= "Debug" && getBuildString() !$= "Release")
	{
		return;
	}
	%offset = getFunctionOffset(%fnNamespace, %fnName);
	if (%fnNamespace $= "")
	{
		%file.writeLine("if(  (fnNamespace == NULL) && !dStricmp(fnName, \"" @ %fnName @ "\") && (ip != " @ %offset @ ")  )");
		%file.writeLine("   fail = true;");
	}
	else if (%protectANY)
	{
		%file.writeLine("if(  fnNamespace && !dStricmp(fnName, \"" @ %fnName @ "\") && (ip != " @ %offset @ ")  )");
		%file.writeLine("   fail = true;");
	}
	else
	{
		%file.writeLine("if(  fnNamespace && !dStricmp(fnNamespace, \"" @ %fnNamespace @ "\") && !dStricmp(fnName, \"" @ %fnName @ "\") && (ip != " @ %offset @ ")  )");
		%file.writeLine("   fail = true;");
	}
}

function removeEmptyBrickGroups()
{
	%count = ClientGroup.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		%cl = ClientGroup.getObject(%i);
		%cl.brickGroup.DoNotDelete = 1;
	}
	for (%i = 0; %i < mainBrickGroup.getCount(); %i++)
	{
		%brickGroup = mainBrickGroup.getObject(%i);
		if (%brickGroup.DoNotDelete == 1)
		{
			%i++;
		}
		else if (%brickGroup.getCount() > 0)
		{
			%i++;
		}
		else
		{
			%brickGroup.delete();
		}
	}
	%count = ClientGroup.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		%cl = ClientGroup.getObject(%i);
		%cl.brickGroup.DoNotDelete = "";
	}
}

datablock AudioProfile(lightOnSound)
{
	fileName = "~/data/sound/lightOn.wav";
	description = AudioClosest3d;
	preload = 1;
};
datablock AudioProfile(lightOffSound)
{
	fileName = "~/data/sound/lightOff.wav";
	description = AudioClosest3d;
	preload = 1;
};
datablock fxLightData(PlayerLight)
{
	uiName = "Player's Light";
	LightOn = 1;
	radius = 10;
	Brightness = 5;
	color = "1 1 1 1";
	FlareOn = 1;
	FlareTP = 1;
	FlareBitmap = "base/lighting/corona";
	FlareColor = "1 1 1";
	ConstantSizeOn = 1;
	ConstantSize = 1;
	NearSize = 3;
	FarSize = 0.5;
	NearDistance = 10;
	FarDistance = 30;
	FadeTime = 0.1;
	BlendMode = 0;
};
function fxLight::onRemove(%obj)
{
}

datablock ParticleData(PlayerBurnParticle)
{
	textureName = "base/data/particles/cloud";
	dragCoefficient = 0;
	gravityCoefficient = -0.3;
	inheritedVelFactor = 0;
	windCoefficient = 0;
	constantAcceleration = 0;
	lifetimeMS = 1100;
	lifetimeVarianceMS = 900;
	spinSpeed = 0;
	spinRandomMin = -90;
	spinRandomMax = 90;
	useInvAlpha = 1;
	colors[0] = "0 0 0 0.0";
	colors[1] = "0.0 0.0 0.0 1.0";
	colors[2] = "0 0 0 0.0";
	sizes[0] = 0.5;
	sizes[1] = 1.5;
	sizes[2] = 1.86;
	times[0] = 0;
	times[1] = 0.4;
	times[2] = 1;
};
datablock ParticleEmitterData(PlayerBurnEmitter)
{
	ejectionPeriodMS = 20;
	periodVarianceMS = 4;
	ejectionVelocity = 0;
	ejectionOffset = 0.2;
	velocityVariance = 0;
	thetaMin = 0;
	thetaMax = 180;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = PlayerBurnParticle;
	uiName = "Player Fire";
};
datablock ShapeBaseImageData(PlayerBurnImage)
{
	shapeFile = "~/data/shapes/empty.dts";
	emap = 0;
	mountPoint = 5;
	offset = "0 0 -0.65";
	stateName[0] = "Ready";
	stateTransitionOnTimeout[0] = "FireA";
	stateTimeoutValue[0] = 0.01;
	stateName[1] = "FireA";
	stateTransitionOnTimeout[1] = "FireB";
	stateWaitForTimeout[1] = True;
	stateTimeoutValue[1] = 0.05;
	stateEmitter[1] = PlayerBurnEmitter;
	stateEmitterTime[1] = 5;
	stateEmitterNode[1] = "emitterPoint";
	stateName[2] = "FireB";
	stateTransitionOnTimeout[2] = "FireA";
	stateWaitForTimeout[2] = True;
	stateTimeoutValue[2] = 0.05;
	stateEmitter[2] = PlayerBurnEmitter;
	stateEmitterTime[2] = 0.05;
};
datablock ParticleData(CameraParticleA)
{
	textureName = "~/data/particles/dot";
	dragCoefficient = 0;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	windCoefficient = 0;
	constantAcceleration = 0;
	lifetimeMS = 200;
	lifetimeVarianceMS = 0;
	spinSpeed = 0;
	spinRandomMin = -90;
	spinRandomMax = 90;
	useInvAlpha = 0;
	colors[0] = "0.6 0.0 0.0 0.0";
	colors[1] = "1   1   0.3 1.0";
	colors[2] = "0.6 0.0 0.0 0.0";
	sizes[0] = 1.5;
	sizes[1] = 1.5;
	sizes[2] = 0;
	times[0] = 0;
	times[1] = 0.5;
	times[2] = 1;
};
datablock ParticleEmitterData(CameraEmitterA)
{
	ejectionPeriodMS = 1;
	periodVarianceMS = 0;
	ejectionVelocity = 0;
	ejectionOffset = 0.1;
	velocityVariance = 0;
	thetaMin = 0;
	thetaMax = 180;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = CameraParticleA;
	doFalloff = 0;
	doDetail = 0;
	pointEmitterNode = FifthEmitterNode;
};
datablock ShapeBaseImageData(cameraImage)
{
	shapeFile = "~/data/shapes/empty.dts";
	emap = 0;
	mountPoint = 0;
	offset = "0 0 0";
	doRetraction = 0;
	firstPersonParticles = 0;
	stateName[0] = "Ready";
	stateTransitionOnTimeout[0] = "FireA";
	stateTimeoutValue[0] = 0.01;
	stateName[1] = "FireA";
	stateTransitionOnTimeout[1] = "FireB";
	stateWaitForTimeout[1] = True;
	stateTimeoutValue[1] = 0.05;
	stateEmitter[1] = CameraEmitterA;
	stateEmitterTime[1] = 0.05;
	stateName[2] = "FireB";
	stateTransitionOnTimeout[2] = "FireA";
	stateWaitForTimeout[2] = True;
	stateTimeoutValue[2] = 0.05;
	stateEmitter[2] = CameraEmitterA;
	stateEmitterTime[2] = 0.05;
};
datablock ParticleData(PlayerTeleportParticleA)
{
	textureName = "~/data/particles/dot";
	dragCoefficient = 0;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	windCoefficient = 0;
	constantAcceleration = 0;
	lifetimeMS = 100;
	lifetimeVarianceMS = 99;
	spinSpeed = 0;
	spinRandomMin = -90;
	spinRandomMax = 90;
	useInvAlpha = 0;
	colors[0] = "0.6 0.0 0.0 0.0";
	colors[1] = "1   1   0.3 1.0";
	colors[2] = "0.6 0.0 0.0 0.0";
	sizes[0] = 1.5;
	sizes[1] = 1.5;
	sizes[2] = 0;
	times[0] = 0;
	times[1] = 0.5;
	times[2] = 1;
};
datablock ParticleEmitterData(PlayerTeleportEmitterA)
{
	ejectionPeriodMS = 1;
	periodVarianceMS = 0;
	ejectionVelocity = 10;
	ejectionOffset = 0.1;
	velocityVariance = 0;
	thetaMin = 0;
	thetaMax = 180;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = PlayerTeleportParticleA;
	doFalloff = 0;
	doDetail = 0;
	uiName = "Player Teleport A";
};
datablock ExplosionData(PlayerTeleportExplosion)
{
	lifetimeMS = 150;
	soundProfile = "";
	particleEmitter = "";
	particleDensity = 230;
	particleRadius = 2;
	emitter[0] = PlayerTeleportEmitterA;
	faceViewer = 1;
	explosionScale = "1 1 1";
	shakeCamera = 0;
	camShakeFreq = "7.0 8.0 7.0";
	camShakeAmp = "1.0 1.0 1.0";
	camShakeDuration = 0.5;
	camShakeRadius = 15;
	lightStartRadius = 0;
	lightEndRadius = 0;
	lightStartColor = "0.45 0.3 0.1";
	lightEndColor = "0 0 0";
	impulseRadius = 0;
	impulseForce = 0;
	radiusDamage = 0;
	damageRadius = 0;
};
datablock ProjectileData(playerTeleportProjectile)
{
	directDamage = 0;
	radiusDamage = 0;
	damageRadius = 0;
	Explosion = PlayerTeleportExplosion;
	explodeOnDeath = 1;
	armingDelay = 0;
	lifetime = 10;
	uiName = "Player Teleport";
};
datablock ParticleData(playerTeleportParticleB)
{
	dragCoefficient = 0.1;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 1000;
	lifetimeVarianceMS = 500;
	useInvAlpha = 0;
	textureName = "base/data/particles/dot";
	colors[0] = "0.6 0.0 0.0 0.0";
	colors[1] = "1   1   0.3 1.0";
	colors[2] = "0.6 0.0 0.0 0.0";
	sizes[0] = 0.4;
	sizes[1] = 0.2;
	sizes[2] = 0;
	times[0] = 0;
	times[1] = 0.2;
	times[2] = 1;
};
datablock ParticleEmitterData(playerTeleportEmitterB)
{
	ejectionPeriodMS = 35;
	periodVarianceMS = 0;
	ejectionVelocity = 1.5;
	ejectionOffset = 0.75;
	velocityVariance = 0.49;
	thetaMin = 0;
	thetaMax = 180;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = playerTeleportParticleB;
	uiName = "Player Teleport B";
};
datablock ShapeBaseImageData(PlayerTeleportImage)
{
	shapeFile = "base/data/shapes/empty.dts";
	emap = 0;
	mountPoint = $BackSlot;
	stateName[0] = "Ready";
	stateTransitionOnTimeout[0] = "FireA";
	stateTimeoutValue[0] = 0.01;
	stateName[1] = "FireA";
	stateTransitionOnTimeout[1] = "Done";
	stateWaitForTimeout[1] = True;
	stateTimeoutValue[1] = 3;
	stateEmitter[1] = playerTeleportEmitterB;
	stateEmitterTime[1] = 3;
	stateName[2] = "Done";
	stateScript[2] = "onDone";
};
function PlayerTeleportImage::onDone(%this, %obj, %slot)
{
	%obj.unmountImage(%slot);
}

function ParticleEmitterNode::onRemove(%obj)
{
}

datablock AudioProfile(rewardSound)
{
	fileName = "base/data/sound/orchHitH.WAV";
	description = AudioDefault3d;
	preload = 1;
};
function serverCmdAlarm(%client)
{
	if (isObject(%client.Player))
	{
		%client.Player.emote(AlarmProjectile);
	}
}

function serverCmdBSD(%client)
{
	if (isObject(%client.Player))
	{
		%client.Player.emote(BSDProjectile);
	}
}

function serverCmdZombie(%client)
{
	if (isObject(%client.Player))
	{
		%client.Player.playThread(1, armReadyBoth);
	}
}

function serverCmdHug(%client)
{
	if (isObject(%client.Player))
	{
		%client.Player.playThread(1, armReadyBoth);
	}
}

function serverCmdSit(%client)
{
	%player = %client.Player;
	if (!isObject(%player))
	{
		return;
	}
	if (%player.isMounted())
	{
		return;
	}
	if (%player.getDamagePercent() < 1)
	{
		%player.setActionThread(sit, 1);
	}
}

function Player::emote(%player, %data, %skipSpam)
{
	if (!%skipSpam)
	{
		if (getSimTime() - %player.lastVoiceTime < 1000)
		{
			%player.voiceCount++;
		}
		else if (getSimTime() - %player.lastVoiceTime > 10000)
		{
			%player.voiceCount = 0;
		}
		if (%player.voiceCount > 5)
		{
			return;
		}
		%player.lastVoiceTime = getSimTime();
	}
	if (%data.getClassName() $= "ShapeBaseImageData")
	{
		%player.mountImage(%data, 3);
	}
	else if (%data.getClassName() $= "ProjectileData")
	{
		%pos = %player.getEyePoint();
		%trans = %player.getTransform();
		%posX = getWord(%pos, 0);
		%posY = getWord(%pos, 1);
		%posZ = getWord(%pos, 2);
		%finalPos = %posX @ " " @ %posY @ " " @ %posZ;
		%p = new Projectile()
		{
			dataBlock = %data;
			initialVelocity = "0 0 1";
			initialPosition = %finalPos;
			sourceObject = %player;
			sourceSlot = 0;
			client = %client;
		};
		%p.setScale(%player.getScale());
	}
}

datablock ParticleData(BSDParticle)
{
	dragCoefficient = 5;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	windCoefficient = 0;
	constantAcceleration = 0;
	lifetimeMS = 500;
	lifetimeVarianceMS = 0;
	useInvAlpha = 1;
	textureName = "~/data/particles/bricks";
	colors[0] = "1 0.5 0 0";
	colors[1] = "1 1 1 0.8";
	colors[2] = "1 0.5 0 0";
	sizes[0] = 1.8;
	sizes[1] = 1.5;
	sizes[2] = 1.5;
	times[0] = 0;
	times[1] = 0.2;
	times[2] = 1;
};
datablock ParticleEmitterData(BSDEmitter)
{
	ejectionPeriodMS = 35;
	periodVarianceMS = 0;
	ejectionVelocity = 0;
	ejectionOffset = 1.2;
	velocityVariance = 0;
	thetaMin = 0;
	thetaMax = 0;
	phiReferenceVel = 0;
	phiVariance = 0;
	overrideAdvance = 0;
	lifetimeMS = 100;
	particles = "BSDParticle";
	uiName = "Emote - Bricks";
};
datablock ExplosionData(BSDExplosion)
{
	lifetimeMS = 2000;
	emitter[0] = BSDEmitter;
};
datablock ProjectileData(BSDProjectile)
{
	Explosion = BSDExplosion;
	armingDelay = 0;
	lifetime = 10;
	explodeOnDeath = 1;
};
datablock ParticleData(PainLowParticle)
{
	dragCoefficient = 2;
	gravityCoefficient = 1;
	inheritedVelFactor = 0;
	windCoefficient = 0;
	constantAcceleration = 0;
	lifetimeMS = 1500;
	lifetimeVarianceMS = 100;
	useInvAlpha = 1;
	spinSpeed = 10;
	spinRandomMin = -150;
	spinRandomMax = 150;
	textureName = "~/data/particles/pain";
	colors[0] = "0.0 1.0 0 0";
	colors[1] = "0.0 1 0 0.8";
	colors[2] = "0.0 0.0 0 0";
	sizes[0] = 1;
	sizes[1] = 1;
	sizes[2] = 1;
	times[0] = 0;
	times[1] = 0.2;
	times[2] = 1;
};
datablock ParticleEmitterData(PainLowEmitter)
{
	ejectionPeriodMS = 65;
	periodVarianceMS = 0;
	ejectionVelocity = 3;
	ejectionOffset = 0.5;
	velocityVariance = 0;
	thetaMin = 0;
	thetaMax = 180;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = "PainLowParticle";
	uiName = "Pain - Low";
};
datablock ShapeBaseImageData(PainLowImage)
{
	shapeFile = "base/data/shapes/empty.dts";
	emap = 0;
	mountPoint = $HeadSlot;
	stateName[0] = "Ready";
	stateTransitionOnTimeout[0] = "FireA";
	stateTimeoutValue[0] = 0.01;
	stateName[1] = "FireA";
	stateTransitionOnTimeout[1] = "Done";
	stateWaitForTimeout[1] = True;
	stateEmitter[1] = PainLowEmitter;
	stateEmitterTime[1] = 0.35;
	stateTimeoutValue[1] = 0.35;
	stateName[2] = "Done";
	stateScript[2] = "onDone";
};
function PainLowImage::onDone(%this, %obj, %slot)
{
	%obj.unmountImage(%slot);
}

datablock ParticleData(PainMidParticle)
{
	dragCoefficient = 2;
	gravityCoefficient = 1;
	inheritedVelFactor = 0;
	windCoefficient = 0;
	constantAcceleration = 0;
	lifetimeMS = 1500;
	lifetimeVarianceMS = 100;
	useInvAlpha = 1;
	spinSpeed = 10;
	spinRandomMin = -150;
	spinRandomMax = 150;
	textureName = "~/data/particles/pain";
	colors[0] = "1.0 1.0 0 0.0";
	colors[1] = "1.0 1.0 0 0.8";
	colors[2] = "0.5 0.0 0 0.0";
	sizes[0] = 1.8;
	sizes[1] = 1.5;
	sizes[2] = 1.5;
	times[0] = 0;
	times[1] = 0.2;
	times[2] = 1;
};
datablock ParticleEmitterData(PainMidEmitter)
{
	ejectionPeriodMS = 65;
	periodVarianceMS = 0;
	ejectionVelocity = 3;
	ejectionOffset = 0.5;
	velocityVariance = 0;
	thetaMin = 0;
	thetaMax = 180;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = "PainMidParticle";
	uiName = "Pain - Mid";
};
datablock ShapeBaseImageData(PainMidImage : PainLowImage)
{
	stateEmitter[1] = PainMidEmitter;
	stateEmitterTime[1] = 0.35;
	stateTimeoutValue[1] = 0.35;
};
function PainMidImage::onDone(%this, %obj, %slot)
{
	%obj.unmountImage(%slot);
}

datablock ParticleData(PainHighParticle)
{
	dragCoefficient = 2;
	gravityCoefficient = 1;
	inheritedVelFactor = 0;
	windCoefficient = 0;
	constantAcceleration = 0;
	lifetimeMS = 1500;
	lifetimeVarianceMS = 100;
	useInvAlpha = 1;
	spinSpeed = 10;
	spinRandomMin = -150;
	spinRandomMax = 150;
	textureName = "~/data/particles/pain";
	colors[0] = "1 0.0 0 0";
	colors[1] = "1 1 1 0.8";
	colors[2] = "0.5 0.0 0 0";
	sizes[0] = 1.8;
	sizes[1] = 3.5;
	sizes[2] = 1.5;
	times[0] = 0;
	times[1] = 0.1;
	times[2] = 1;
};
datablock ParticleEmitterData(PainHighEmitter)
{
	ejectionPeriodMS = 65;
	periodVarianceMS = 0;
	ejectionVelocity = 3;
	ejectionOffset = 0.5;
	velocityVariance = 0;
	thetaMin = 0;
	thetaMax = 180;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = "PainHighParticle";
	uiName = "Pain - High";
};
datablock ShapeBaseImageData(PainHighImage : PainLowImage)
{
	stateEmitter[1] = PainHighEmitter;
	stateEmitterTime[1] = 0.35;
	stateTimeoutValue[1] = 0.35;
};
function PainHighImage::onDone(%this, %obj, %slot)
{
	%obj.unmountImage(%slot);
}

datablock ParticleData(WinStarParticle)
{
	dragCoefficient = 5;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	windCoefficient = 0;
	constantAcceleration = 0;
	lifetimeMS = 500;
	lifetimeVarianceMS = 0;
	useInvAlpha = 0;
	textureName = "~/data/particles/star1";
	colors[0] = "1 1 0 1";
	colors[1] = "1 1 0 1";
	colors[2] = "1 1 0 1";
	sizes[0] = 0.9;
	sizes[1] = 0.9;
	sizes[2] = 0.9;
	times[0] = 0;
	times[1] = 0.2;
	times[2] = 1;
};
datablock ParticleEmitterData(WinStarEmitter)
{
	ejectionPeriodMS = 35;
	periodVarianceMS = 0;
	ejectionVelocity = 0;
	ejectionOffset = 1.8;
	velocityVariance = 0;
	thetaMin = 0;
	thetaMax = 0;
	phiReferenceVel = 0;
	phiVariance = 0;
	overrideAdvance = 0;
	lifetimeMS = 100;
	particles = "WinStarParticle";
	doFalloff = 0;
	emitterNode = GenericEmitterNode;
	pointEmitterNode = TenthEmitterNode;
	uiName = "Emote - Win";
};
datablock ExplosionData(WinStarExplosion)
{
	lifetimeMS = 2000;
	emitter[0] = WinStarEmitter;
};
datablock ProjectileData(winStarProjectile)
{
	Explosion = WinStarExplosion;
	armingDelay = 0;
	lifetime = 10;
	explodeOnDeath = 1;
};
datablock AudioDescription(AudioMusicLooping3d)
{
	volume = 1;
	isLooping = 1;
	is3D = 1;
	ReferenceDistance = 10;
	maxDistance = 30;
	type = $SimAudioType;
};
function createMusicDatablocks()
{
	updateMusicList();
	%dir = "Add-ons/Music/*.ogg";
	%fileCount = getFileCount(%dir);
	%filename = findFirstFile(%dir);
	for (%i = 0; %i < %fileCount; %i++)
	{
		%base = fileBase(%filename);
		%uiName = strreplace(%base, "_", " ");
		%varName = getSafeVariableName(%base);
		if (!$Server::Dedicated)
		{
			if (getRealTime() - $lastProgressBarTime > 200)
			{
				LoadingProgress.setValue(%i / %fileCount);
				$lastProgressBarTime = getRealTime();
				Canvas.repaint();
			}
		}
		if (!isValidMusicFilename(%filename))
		{
			%filename = findNextFile(%dir);
		}
		else if (getFileLength(%filename) > 1048576)
		{
			error("ERROR: Music file \"" @ %filename @ "\" > 1mb - ignoring");
			%filename = findNextFile(%dir);
		}
		else
		{
			if ($Music__[%varName] $= "1")
			{
				%dbName = "musicData_" @ %varName;
				%command = "datablock AudioProfile(" @ %dbName @ ") {" @ "filename = \"" @ %filename @ "\";" @ "description = AudioMusicLooping3d;" @ "preload = true;" @ "uiName = \"" @ %uiName @ "\";" @ "};";
				eval(%command);
				if (%dbName.isStereo())
				{
					error("ERROR: Stereo sound detected on \"" @ %dbName.getName() @ "\" - Removing datablock.");
					schedule(1000, 0, MessageAll, '', "Stereo sound detected on  \"" @ fileName(%dbName.fileName) @ "\" - Removing datablock.");
					%dbName.uiName = "";
					%dbName.delete();
					if (getBuildString() $= "Ship")
					{
						fileDelete(%filename);
						continue;
					}
					warning("WARNING: '" @ %filename @ "' is a stereo music block and would be deleted if this was the public build!");
				}
			}
			%filename = findNextFile(%dir);
		}
	}
}

function updateMusicList()
{
	deleteVariables("$Music*");
	if (isFile("config/server/musicList.cs"))
	{
		exec("config/server/musicList.cs");
	}
	else
	{
		exec("base/server/defaultMusicList.cs");
	}
	%dir = "base/data/sound/music/*.ogg";
	%fileCount = getFileCount(%dir);
	%filename = findFirstFile(%dir);
	for (%i = 0; %i < %fileCount; %i++)
	{
		%base = fileBase(%filename);
		%varName = getSafeVariableName(%base);
		if (!isValidMusicFilename(%filename))
		{
			%filename = findNextFile(%dir);
		}
		else
		{
			if (mFloor($Music__[%varName]) <= 0)
			{
				$Music__[%varName] = -1;
			}
			else
			{
				$Music__[%varName] = 1;
			}
			%filename = findNextFile(%dir);
		}
	}
	export("$Music__*", "config/server/musicList.cs");
}

function isValidMusicFilename(%filename)
{
	%base = fileBase(%filename);
	%uiName = strreplace(%base, "_", " ");
	%firstWord = getWord(%uiName, 0);
	if (%firstWord $= mFloor(%firstWord))
	{
		return 0;
	}
	%pos = strpos(%filename, "/", strlen("Add-Ons/Music/") + 1);
	if (%pos != -1)
	{
		return 0;
	}
	if (strstr(%filename, "Copy of") != -1 || strstr(%filename, "Copy_of") != -1 || strstr(%filename, "- Copy") != -1 || strstr(%filename, "-_Copy") != -1 || strstr(%filename, "(") != -1 || strstr(%filename, ")") != -1 || strstr(%filename, "[") != -1 || strstr(%filename, "]") != -1 || strstr(%filename, "+") != -1 || strstr(%filename, " ") != -1)
	{
		return 0;
	}
	return 1;
}

function brickSpawnPointData::onLoadPlant(%this, %obj)
{
	brickSpawnPointData::onPlant(%this, %obj);
}

function brickSpawnPointData::onPlant(%this, %obj)
{
	%group = %obj.getGroup();
	if (!isObject(%group))
	{
		error("ERROR: brickSpawnPointData::onPlant() - " @ %obj @ " planted outside of a group");
	}
	%group.addSpawnBrick(%obj);
}

function brickSpawnPointData::onRemove(%this, %obj)
{
	%group = %obj.getGroup();
	if (isObject(%group))
	{
		%group.removeSpawnBrick(%obj);
	}
}

function SimGroup::addSpawnBrick(%group, %brick)
{
	%i = mFloor(%group.spawnBrickCount);
	%group.spawnBrick[%i] = %brick;
	%group.spawnBrickCount++;
}

function SimGroup::removeSpawnBrick(%group, %brick)
{
	for (%i = 0; %i < %group.spawnBrickCount; %i++)
	{
		if (%group.spawnBrick[%i] == %brick)
		{
			%group.spawnBrick[%i] = %group.spawnBrick[%group.spawnBrickCount - 1];
			%group.spawnBrick[%group.spawnBrickCount - 1] = -1;
			%group.spawnBrickCount--;
			%i--;
		}
	}
}

function SimGroup::getBrickSpawnPoint(%group)
{
	if (%group.spawnBrickCount <= 0)
	{
		return pickSpawnPoint();
	}
	%startIdx = getRandom(%group.spawnBrickCount - 1);
	%brick = %group.spawnBrick[%idx];
	for (%i = 0; %i < %group.spawnBrickCount; %i++)
	{
		%idx = (%startIdx + %i) % %group.spawnBrickCount;
		%brick = %group.spawnBrick[%idx];
		if (!%brick.isBlocked())
		{
			break;
		}
	}
	return %brick.getSpawnPoint();
}

function fxDTSBrick::getSpawnPoint(%brick)
{
	%trans = %brick.getTransform();
	%x = getWord(%trans, 0);
	%y = getWord(%trans, 1);
	%z = getWord(%trans, 2) - 1.3;
	%rot = getWords(%trans, 3, 6);
	%start = %x SPC %y SPC %z + 2.8;
	%end = %x SPC %y SPC %z;
	%mask = $TypeMasks::TerrainObjectType | $TypeMasks::InteriorObjectType | $TypeMasks::FxBrickAlwaysObjectType;
	%raycast = containerRayCast(%start, %end, %mask, %brick);
	if (%raycast)
	{
		%pos = posFromRaycast(%raycast);
		%pos = VectorAdd(%pos, "0 0 0.1");
		%trans = %pos SPC %rot;
	}
	else
	{
		%trans = %x SPC %y SPC %z SPC %rot;
	}
	return %trans;
}

function SimGroup::dumpSpawnPoints(%group)
{
	echo("");
	echo(%group.spawnBrickCount, " SpawnBricks:");
	echo("--------------------------");
	for (%i = 0; %i < %group.spawnBrickCount; %i++)
	{
		if (isObject(%group.spawnBrick[%i]))
		{
			echo("  ", %group.spawnBrick[%i]);
		}
		else
		{
			echo("  ", %group.spawnBrick[%i], " <---- NOT AN OBJECT");
		}
	}
	echo("--------------------------");
}

function ServerCmdRequestBrickManList(%client)
{
	if (!%client.isAdmin)
	{
		return;
	}
	%count = mainBrickGroup.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		%subGroup = mainBrickGroup.getObject(%i);
		%brickCount = %subGroup.getCount();
		if ($Server::LAN)
		{
			%line = "LAN" TAB "Everyone" TAB %brickCount;
			commandToClient(%client, 'AddBrickManLine', -1, %line);
		}
		else
		{
			%line = %subGroup.bl_id TAB %subGroup.name TAB %brickCount;
			commandToClient(%client, 'AddBrickManLine', %subGroup.bl_id, %line);
		}
	}
}

function ServerCmdHilightBrickGroup(%client, %bl_id)
{
	if (!%client.isAdmin)
	{
		return;
	}
	%bl_id = mFloor(%bl_id);
	%group = "BrickGroup_" @ %bl_id;
	if (!isObject(%group))
	{
		return;
	}
	if (%group.getClassName() !$= "SimGroup")
	{
		error("ERROR: ServerCmdClearBrickGroup() - \"" @ %group @ "\" is not a SimGroup!");
		MessageAll('', "ERROR: ServerCmdClearBrickGroup() - \"" @ %group @ "\" is not a SimGroup!");
		return;
	}
	%bestColor = 0;
	%bestScore = 0;
	%worstColor = 0;
	%worstScore = 0;
	for (%i = 0; %i < $maxSprayColors; %i++)
	{
		%rgba = getColorIDTable(%i);
		%r = getWord(%rgba, 0);
		%g = getWord(%rgba, 1);
		%b = getWord(%rgba, 2);
		%a = getWord(%rgba, 3);
		%score = %r + %g + %b + 10 * %a;
		%lowScore = (1 - %r) + (1 - %g) + (1 - %b) + %a * 10;
		if (%lowScore > %worstScore)
		{
			%worstColor = %i;
			%worstScore = %lowScore;
		}
		if (%score > %bestScore)
		{
			%bestColor = %i;
			%bestScore = %score;
		}
	}
	$HilightColor = %bestColor;
	$LowlightColor = %worstColor;
	if (%group.getCount() > 10000)
	{
		%time = 3000;
	}
	else if (%group.getCount() > 4000)
	{
		%time = 2000;
	}
	else if (%group.getCount() > 2000)
	{
		%time = 1500;
	}
	else
	{
		%time = 1000;
	}
	%group.chainBlink(0, 1, 1, %time);
}

function SimGroup::chainBlink(%group, %idx, %count, %firstPass, %timeBetween)
{
	%idx = mFloor(%idx);
	if (%idx == 0 && %firstPass == 1)
	{
		if (%group.isChainBlinking)
		{
			return;
		}
		else
		{
			%group.isChainBlinking = 1;
		}
	}
	for (%i = 0; %i < 3; %i++)
	{
		if (%idx < %group.getCount())
		{
			%obj = %group.getObject(%idx);
		}
		else
		{
			if (%count != 0)
			{
				%group.schedule(%timeBetween, chainBlink, 0, %count--, 0, %timeBetween);
			}
			else
			{
				%group.isChainBlinking = 0;
			}
			return;
		}
		if (%obj.getClassName() $= "FxDtsBrick")
		{
			if (%obj.isPlanted())
			{
				if (%firstPass || %obj.oldColor $= "")
				{
					%obj.oldColor = %obj.getColorID();
					%obj.oldColorFX = %obj.getColorFxID();
					%obj.setColorFX(3);
				}
				if (%count == 0)
				{
					%obj.setColor(%obj.oldColor);
					%obj.setColorFX(%obj.oldColorFX);
					%obj.oldColor = "";
					%obj.oldColorFX = "";
					continue;
				}
				%x = %count % 2;
				if (%x == 0)
				{
					echo("hilight 0");
					%obj.setColor($HilightColor);
					continue;
				}
				if (%x == 1)
				{
					%obj.setColor($HilightColor);
					continue;
				}
				echo("wtf should not happen");
			}
		}
		%idx++;
	}
	%group.schedule(1, chainBlink, %idx, %count, %firstPass, %timeBetween);
}

function ServerCmdClearBrickGroup(%client, %bl_id)
{
	if (!%client.isAdmin)
	{
		return;
	}
	%bl_id = mFloor(%bl_id);
	%group = "BrickGroup_" @ %bl_id;
	if (!isObject(%group))
	{
		error("ERROR: ServerCmdClearBrickGroup() - \"" @ %group @ "\" does not exist!");
		MessageAll('', "ERROR: ServerCmdClearBrickGroup() - \"" @ %group @ "\" does not exist!");
		return;
	}
	if (%group.getClassName() !$= "SimGroup")
	{
		error("ERROR: ServerCmdClearBrickGroup() - \"" @ %group @ "\" is not a SimGroup!");
		MessageAll('', "ERROR: ServerCmdClearBrickGroup() - \"" @ %group @ "\" is not a SimGroup!");
		return;
	}
	if (%group.bl_id == getLAN_BLID())
	{
		MessageAll('MsgClearBricks', '\c3%1\c2 cleared the bricks', %client.getPlayerName());
	}
	else
	{
		MessageAll('MsgClearBricks', '\c3%1\c2 cleared \c3%2\c2\'s bricks', %client.getPlayerName(), %group.name);
	}
	%group.deleteAll();
	ServerCmdRequestBrickManList(%client);
}

function ServerCmdClearAllBricks(%client)
{
	if (!%client.isAdmin)
	{
		return;
	}
	if ($Game::MissionCleaningUp)
	{
		messageClient(%client, '', 'Can\'t clear bricks during mission clean up');
		return;
	}
	if ($Server::BrickCount > 0)
	{
		MessageAll('MsgClearBricks', "\c3" @ %client.getPlayerName() @ "\c0 cleared all bricks.");
	}
	setRaytracerAutoCenter();
	stopRaytracer();
	%x = 0;
	while (1)
	{
		if (%x >= mainBrickGroup.getCount())
		{
			break;
		}
		%subGroup = mainBrickGroup.getObject(%x);
		if (%subGroup.getCount() <= 0)
		{
			%x++;
		}
		else
		{
			%subGroup.chainDeleteAll();
			%x++;
		}
	}
	ServerCmdRequestBrickManList(%client);
}

function SimGroup::chainDeleteAll(%group)
{
	if (isEventPending(%group.chainDeleteSchedule))
	{
		cancel(%group.chainDeleteSchedule);
	}
	if (%group.NTNameCount > 0)
	{
		%group.ClearAllNTNames();
	}
	%count = %group.getCount();
	if (%count > 0)
	{
		%obj = %group.getObject(0);
		if (%obj.getClassName() $= "SimGroup")
		{
			if (%obj.getCount() > 0)
			{
				%obj.chainDeleteCallBack = %group @ ".schedule(0, chainDeleteAll);";
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
		if (isObject(%oldQuotaObject))
		{
			setCurrentQuotaObject(GlobalQuota);
		}
		eval(%group.chainDeleteCallBack);
		if (isObject(%oldQuotaObject))
		{
			setCurrentQuotaObject(%oldQuotaObject);
		}
		%group.chainDeleteCallBack = "";
		return;
	}
	%oldQuotaObject = getCurrentQuotaObject();
	if (isObject(%oldQuotaObject))
	{
		setCurrentQuotaObject(GlobalQuota);
	}
	%group.chainDeleteSchedule = %group.schedule(0, chainDeleteAll);
	if (isObject(%oldQuotaObject))
	{
		setCurrentQuotaObject(%oldQuotaObject);
	}
}

function serverCmdTrust_Invite(%client, %targetClient, %targetBL_ID, %level)
{
	echo("serverCmdTrust_Invite from ", %client.getPlayerName(), " to ", %targetBL_ID, " level = ", %level);
	if (%targetBL_ID == getLAN_BLID())
	{
		commandToClient(%client, 'MessageBoxOK', 'Trust Invite Error', 'Trust lists do not apply on a LAN.');
		return;
	}
	%ourBL_ID = %client.getBLID();
	%targetBL_ID = mFloor(%targetBL_ID);
	%level = mFloor(%level);
	%currTrustLevel = %client.getBL_IDTrustLevel(%targetBL_ID);
	if (%targetBL_ID == %ourBL_ID)
	{
		commandToClient(%client, 'MessageBoxOK', 'Trust Invite Error', 'You already trust yourself.  I hope.');
		return;
	}
	if (%currTrustLevel >= %level)
	{
		commandToClient(%client, 'MessageBoxOK', 'Trust Invite', 'You already trust this person at that level.');
		return;
	}
	%targetBrickGroup = "BrickGroup_" @ %targetBL_ID;
	if (!isObject(%targetBrickGroup))
	{
		commandToClient(%client, 'MessageBoxOK', 'Trust Invite Error', 'Target brick group does not exist.');
		return;
	}
	if (!isObject(%targetBrickGroup.client))
	{
		commandToClient(%client, 'MessageBoxOK', 'Trust Invite Error', 'Target client does not exist.');
		return;
	}
	if (!isObject(%targetClient))
	{
		commandToClient(%client, 'MessageBoxOK', 'Trust Invite Error', 'Target client does not exist.');
		return;
	}
	if (%targetClient.getClassName() !$= "GameConnection")
	{
		commandToClient(%client, 'MessageBoxOK', 'Trust Invite Error', 'Target client is not a GameConnection.');
		return;
	}
	if (%targetClient.getBLID() != %targetBL_ID)
	{
		commandToClient(%client, 'MessageBoxOK', 'Trust Invite Error', 'Target client does not match target bl_id.');
		return;
	}
	if (%targetClient.Ignore[%ourBL_ID] == 1)
	{
		commandToClient(%client, 'MessageBoxOK', 'Trust Invite Error', 'This person is ignoring your invites.');
		return;
	}
	if (%targetClient.invitePendingBL_ID $= %ourBL_ID)
	{
		commandToClient(%client, 'MessageBoxOK', 'Trust Invite Error', 'This person hasn\'t responded to your first invite yet.');
		return;
	}
	else if (%targetClient.invitePendingBL_ID !$= "")
	{
		commandToClient(%client, 'MessageBoxOK', 'Trust Invite Error', 'This person is responding to another invite right now.');
		return;
	}
	if (%level == 1)
	{
		%targetClient.invitePendingBL_ID = %ourBL_ID;
		%targetClient.invitePendingLevel = 1;
		%targetClient.invitePendingClient = %client;
		%targetClient.StartInvitationTimeout();
		commandToClient(%targetClient, 'TrustInvite', %client.getPlayerName(), %client.getBLID(), 1);
	}
	else if (%level == 2)
	{
		%targetClient.invitePendingBL_ID = %ourBL_ID;
		%targetClient.invitePendingLevel = 2;
		%targetClient.invitePendingClient = %client;
		%targetClient.StartInvitationTimeout();
		commandToClient(%targetClient, 'TrustInvite', %client.getPlayerName(), %client.getBLID(), 2);
	}
	else
	{
		commandToClient(%client, 'MessageBoxOK', 'Trust Invite Error', 'Invalid trust level specified.');
		return;
	}
}

function serverCmdAcceptTrustInvite(%client, %invitingBL_ID)
{
	%invitingBL_ID = mFloor(%invitingBL_ID);
	%invitingBrickGroup = "BrickGroup_" @ %invitingBL_ID;
	if (!isObject(%invitingBrickGroup))
	{
		%client.ClearInvitePending();
		return;
	}
	%invitingClient = %invitingBrickGroup.client;
	if (!isObject(%invitingClient))
	{
		%client.ClearInvitePending();
		return;
	}
	if (%client.invitePendingBL_ID $= "")
	{
		commandToClient(%client, 'MessageBoxOK', 'Trust Invitation Error', 'That invitation is too old.');
		return;
	}
	if (%client.invitePendingBL_ID != %invitingBL_ID)
	{
		error("ERROR: serverCmdAcceptTrustInvite() - invite response does not match saved invite");
		%client.ClearInvitePending();
		return;
	}
	SetMutualBrickGroupTrust(%invitingBL_ID, %client.getBLID(), %client.invitePendingLevel);
	commandToClient(%client, 'MessageBoxOK', 'Trust Invitation Accepted', 'You have accepted %1\'s trust invitation.', %invitingClient.getPlayerName());
	commandToClient(%invitingClient, 'MessageBoxOK', 'Trust Invitation Accepted', '%1 has accepted your trust invitation.', %client.getPlayerName());
	commandToClient(%invitingClient, 'TrustInviteAccepted', %client, %client.getBLID(), %client.invitePendingLevel);
	secureCommandToClient("mod2maiegut^afoo", %invitingClient, 'ClientTrust', %client, %client.invitePendingLevel);
	secureCommandToClient("mod2maiegut^afoo", %client, 'ClientTrust', %invitingClient, %client.invitePendingLevel);
	%client.ClearInvitePending();
}

function serverCmdRejectTrustInvite(%client, %targetBL_ID)
{
	%targetBL_ID = mFloor(%targetBL_ID);
	if (%client.invitePendingBL_ID != %targetBL_ID)
	{
		%client.ClearInvitePending();
		return;
	}
	%client.ClearInvitePending();
	%invitingBrickGroup = "BrickGroup_" @ %targetBL_ID;
	if (!isObject(%invitingBrickGroup))
	{
		return;
	}
	%invitingClient = %invitingBrickGroup.client;
	if (!isObject(%invitingClient))
	{
		return;
	}
	%currTime = getSimTime();
	if (%currTime - %invitingClient.lastTrustRejectionTime < 60 * 1000 || %invitingClient.lastTrustRejectionTime == 0)
	{
		if (!%invitingClient.isAdmin)
		{
			%invitingClient.lastTrustRejectionTime = %currTime;
			%invitingClient.trustRejectionCount++;
			if (%invitingClient.trustRejectionCount >= 3)
			{
				MessageAll('MsgAdminForce', '\c3%1\c2 was kicked for spamming trust invites.', %invitingClient.getPlayerName());
				%invitingClient.schedule(10, delete, "Do not spam trust invites.");
			}
		}
	}
	else
	{
		%invitingClient.lastTrustRejectionTime = 0;
		%invitingClient.trustRejectionCount = 0;
	}
	commandToClient(%invitingClient, 'MessageBoxOK', 'Trust Invite Rejected', '%1 has rejected your trust invitation.', %client.getPlayerName());
}

function serverCmdIgnoreTrustInvite(%client, %targetBL_ID)
{
	%targetBL_ID = mFloor(%targetBL_ID);
	if (%client.invitePendingBL_ID != %targetBL_ID)
	{
		%client.ClearInvitePending();
		return;
	}
	%client.ClearInvitePending();
	%client.Ignore[%targetBL_ID] = 1;
	%invitingBrickGroup = "BrickGroup_" @ %targetBL_ID;
	if (!isObject(%invitingBrickGroup))
	{
		return;
	}
	%invitingClient = %invitingBrickGroup.client;
	if (!isObject(%invitingClient))
	{
		return;
	}
	commandToClient(%invitingClient, 'MessageBoxOK', 'Trust Invite Rejected + Ignored', '%1 has rejected your trust invitation and will ignore any future invites from you.', %client.getPlayerName());
}

function serverCmdUnIgnore(%client, %targetClient)
{
	%targetBL_ID = %targetClient.getBLID();
	if (%client.Ignore[%targetBL_ID] == 1)
	{
		%client.Ignore[%targetBL_ID] = 0;
		commandToClient(%client, 'MessageBoxOK', 'Ignore Removed', 'You are no longer ignoring %1', %targetClient.getPlayerName());
	}
}

function GameConnection::StartInvitationTimeout(%client)
{
	if (isEventPending(%client.clearInviteSchedule))
	{
		error("ERROR: GameConnection::StartInvitationTimeout() - clearInviteSchedule event pending.");
		cancel(%client.clearInviteSchedule);
	}
	%client.clearInviteSchedule = %client.schedule(1 * 60 * 1000, ClearInvitePending);
}

function GameConnection::ClearInvitePending(%client)
{
	if (isEventPending(%client.clearInviteSchedule))
	{
		cancel(%client.clearInviteSchedule);
	}
	%client.invitePendingBL_ID = "";
	%client.invitePendingLevel = "";
	%client.invitePendingClient = "";
}

function serverCmdTrust_Demote(%client, %targetBL_ID, %level)
{
	%targetBL_ID = mFloor(%targetBL_ID);
	%level = mFloor(%level);
	%ourBL_ID = %client.getBLID();
	%ourBrickGroup = "BrickGroup_" @ %ourBL_ID;
	%targetBrickGroup = "BrickGroup_" @ %targetBL_ID;
	if (!isObject(%ourBrickGroup) || !isObject(%targetBrickGroup))
	{
		return;
	}
	%targetClient = %targetBrickGroup.client;
	if (%ourBrickGroup.Trust[%targetBL_ID] <= %level)
	{
		messageClient(%client, 'MessageBoxOK', '%1 is already at or below that trust level.', %targetBrickGroup.name);
		return;
	}
	if (%level == 0)
	{
		SetMutualBrickGroupTrust(%ourBL_ID, %targetBL_ID, 0);
		if (isObject(%targetClient))
		{
			messageClient(%targetClient, 'MessageBoxOK', '%1 has removed you from their trust list.', %client.getPlayerName());
		}
		secureCommandToClient("mod2maiegut^afoo", %targetClient, 'ClientTrust', %client, 0);
		secureCommandToClient("mod2maiegut^afoo", %client, 'ClientTrust', %targetClient, 0);
		secureCommandToClient("mod2maiegut^afoo", %targetClient, 'TrustDemoted', %client, %client.getBLID(), %level);
	}
	else if (%level == 1)
	{
		SetMutualBrickGroupTrust(%ourBL_ID, %targetBL_ID, 1);
		if (isObject(%targetClient))
		{
			messageClient(%targetClient, 'MessageBoxOK', '%1 has demoted you to build trust.', %client.getPlayerName());
		}
		secureCommandToClient("mod2maiegut^afoo", %targetClient, 'ClientTrust', %client, 1);
		secureCommandToClient("mod2maiegut^afoo", %client, 'ClientTrust', %targetClient, 1);
		secureCommandToClient("mod2maiegut^afoo", %targetClient, 'TrustDemoted', %client, %client.getBLID(), %level);
	}
	else
	{
		commandToClient(%client, 'MessageBoxOK', 'Trust Demote Error', 'Invalid trust level specified.');
	}
}

function GameConnection::getBL_IDTrustLevel(%client, %targetBL_ID)
{
	if ($Server::LAN)
	{
		return 2;
	}
	%brickGroup = %client.brickGroup;
	if (!isObject(%brickGroup))
	{
		error("ERROR: GameConnection::getBL_IDTrustLevel() - \"" @ %client.getPlayerName() @ "\" (" @ %client @ ") does not have a brick group.");
		MessageAll('', "ERROR: GameConnection::getBL_IDTrustLevel() - \"" @ %client.getPlayerName() @ "\" (" @ %client @ ") does not have a brick group.");
		return 0;
	}
	%ourLevel = %brickGroup.Trust[%targetBL_ID];
	if (%ourLevel == 0)
	{
		return 0;
	}
	%targetBrickGroup = "brickGroup_" @ %targetBL_ID;
	if (!isObject(%targetBrickGroup))
	{
		return 0;
	}
	%targetLevel = %targetBrickGroup.Trust[%client.getBLID()];
	if (%ourLevel == %targetLevel)
	{
		return %ourLevel;
	}
	else
	{
		return 0;
	}
}

function SetMutualBrickGroupTrust(%bl_idA, %bl_idB, %level)
{
	%bl_idA = mFloor(%bl_idA);
	%bl_idB = mFloor(%bl_idB);
	%level = mFloor(%level);
	if (%level < 0 || %level > 2)
	{
		error("ERROR: SetMutualBrickGroupTrust() - invalid trust level ", %level);
		return;
	}
	%brickGroupA = "BrickGroup_" @ %bl_idA;
	%brickGroupB = "BrickGroup_" @ %bl_idB;
	%brickGroupA.abandonedTime = 0;
	%brickGroupB.abandonedTime = 0;
	%brickGroupA.isPublicDomain = 0;
	%brickGroupB.isPublicDomain = 0;
	if (!isObject(%brickGroupA) || !isObject(%brickGroupB))
	{
		return;
	}
	%brickGroupA.Trust[%bl_idB] = %level;
	%brickGroupB.Trust[%bl_idA] = %level;
	%brickGroupA.addPotentialTrust(%bl_idB, %level);
	%brickGroupB.addPotentialTrust(%bl_idA, %level);
}

function SimGroup::addPotentialTrust(%this, %bl_id, %level)
{
	%bl_id = mFloor(%bl_id);
	%level = mFloor(%level);
	%this.potentialTrust[%bl_id] = %level;
	%count = mFloor(%this.potentialTrustCount);
	for (%i = 0; %i < %count; %i++)
	{
		if (%this.potentialTrustEntry[%i] == %bl_id)
		{
			return;
		}
	}
	%this.potentialTrustEntry[%count] = %bl_id;
	%this.potentialTrustCount++;
}

function GameConnection::InitializeTrustListUpload(%client)
{
	if ($Server::LAN)
	{
		%count = ClientGroup.getCount();
		for (%i = 0; %i < %count; %i++)
		{
			%otherClient = ClientGroup.getObject(%i);
			if (%otherClient == %client)
			{
				secureCommandToClient("mod2maiegut^afoo", %client, 'ClientTrust', %otherClient, -1);
			}
			else
			{
				%level = 10;
				secureCommandToClient("mod2maiegut^afoo", %client, 'ClientTrust', %otherClient, %level);
				secureCommandToClient("mod2maiegut^afoo", %otherClient, 'ClientTrust', %client, %level);
			}
		}
		return;
	}
	%ourBrickGroup = %client.brickGroup;
	%ourBL_ID = %client.getBLID();
	%count = %ourBrickGroup.potentialTrustCount;
	for (%i = 0; %i < %count; %i++)
	{
		%targetBL_ID = %ourBrickGroup.potentialTrustEntry[%i];
		%targetBrickGroup = "BrickGroup_" @ %targetBL_ID;
		if (isObject(%targetBrickGroup))
		{
			%targetBrickGroup.Trust[%ourBL_ID] = 0;
		}
		%ourBrickGroup.Trust[%targetBL_ID] = 0;
		%ourBrickGroup.potentialTrust[%targetBL_ID] = 0;
		%ourBrickGroup.potentialTrustEntry[%i] = 0;
	}
	%ourBrickGroup.potentialTrustCount = 0;
	commandToClient(%client, 'TrustListUpload_Start');
}

function serverCmdTrustListUpload_Line(%client, %line)
{
	%bl_id = mFloor(getWord(%line, 0));
	%level = mFloor(getWord(%line, 1));
	if (%level <= 0)
	{
		return;
	}
	%ourBrickGroup = "BrickGroup_" @ %client.getBLID();
	if (!isObject(%ourBrickGroup))
	{
		error("ERROR: serverCmdTrustListUpload_Line() - \"" @ %client.getPlayerName() @ "\" does not have a brick group.");
		return;
	}
	if (%ourBrickGroup.potentialTrustCount > 1024)
	{
		messageClient(%client, '', 'Trust list upload limit reached.  Maximum 1024 entries.');
		return;
	}
	%ourBrickGroup.addPotentialTrust(%bl_id, %level);
}

function serverCmdTrustListUpload_Done(%client)
{
	%ourBL_ID = %client.getBLID();
	%ourBrickGroup = %client.brickGroup;
	if (!isObject(%ourBrickGroup))
	{
		error("ERROR: serverCmdTrustListUpload_Done() - \"" @ %client.getPlayerName() @ "\" has no brick group");
		return;
	}
	%count = mFloor(%ourBrickGroup.potentialTrustCount);
	for (%i = 0; %i < %count; %i++)
	{
		%currBL_ID = %ourBrickGroup.potentialTrustEntry[%i];
		%currLevel = %ourBrickGroup.potentialTrust[%currBL_ID];
		if (%currLevel > 0)
		{
			%targetBrickGroup = "BrickGroup_" @ %currBL_ID;
			if (isObject(%targetBrickGroup))
			{
				if (%targetBrickGroup.potentialTrust[%ourBL_ID] >= %currLevel)
				{
					SetMutualBrickGroupTrust(%currBL_ID, %ourBL_ID, %currLevel);
					%targetClient = %targetBrickGroup.client;
					if (isObject(%targetClient))
					{
						secureCommandToClient("mod2maiegut^afoo", %targetClient, 'ClientTrust', %client, %currLevel);
						secureCommandToClient("mod2maiegut^afoo", %client, 'ClientTrust', %targetClient, %currLevel);
					}
				}
			}
		}
	}
	%count = ClientGroup.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		%otherClient = ClientGroup.getObject(%i);
		if (%otherClient == %client || %client.brickGroup == %otherClient.brickGroup)
		{
			secureCommandToClient("mod2maiegut^afoo", %client, 'ClientTrust', %otherClient, -1);
		}
		else
		{
			%level = mFloor(%ourBrickGroup.Trust[%otherClient.getBLID()]);
			secureCommandToClient("mod2maiegut^afoo", %client, 'ClientTrust', %otherClient, %level);
			secureCommandToClient("mod2maiegut^afoo", %otherClient, 'ClientTrust', %client, %level);
		}
	}
}

function updateTrustGui(%client, %targetClient)
{
	if (!isObject(%client))
	{
		return;
	}
	if (!isObject(%targetClient))
	{
		return;
	}
	%trustLevelA = %client.brickGroup.Trust[%targetClient.getBLID()];
	%trustLevelB = %targetClient.brickGroup.Trust[%client.getBLID()];
	if (%trustLevelA != %trustLevelB)
	{
		error("ERROR: updateTrustGui() - trust level between " @ %client.getBLID() @ " and " @ %targetClient.getBLID() @ " is asymmetrical");
		return;
	}
	secureCommandToClient("mod2maiegut^afoo", %targetClient, 'ClientTrust', %client, %trustLevel);
	secureCommandToClient("mod2maiegut^afoo", %client, 'ClientTrust', %targetClient, %trustLevel);
}

$TrustLevel::None = 0;
$TrustLevel::Build = 1;
$TrustLevel::Full = 2;
$TrustLevel::You = 3;
$TrustLevel::Paint = 2;
$TrustLevel::FXPaint = 2;
$TrustLevel::BuildOn = 1;
$TrustLevel::Print = 2;
$TrustLevel::UndoPaint = 2;
$TrustLevel::UndoFXPaint = 2;
$TrustLevel::UndoPrint = 2;
$TrustLevel::Wrench = 1;
$TrustLevel::Hammer = 2;
$TrustLevel::Wand = 3;
$TrustLevel::UndoBrick = 2;
$TrustLevel::DriveVehicle = 2;
$TrustLevel::RideVehicle = 1;
$TrustLevel::VehicleTurnover = 1;
$TrustLevel::ItemPickup = 1;
$TrustLevel::WrenchRendering = 2;
$TrustLevel::WrenchCollision = 2;
$TrustLevel::WrenchRaycasting = 2;
$TrustLevel::WrenchEvents = 2;
function TrustListCheck(%obj1, %obj2, %interactionType)
{
	if (%obj2.getType() & $TypeMasks::PlayerObjectType)
	{
		if (!(%obj1.getType() & $TypeMasks::PlayerObjectType))
		{
			return;
		}
	}
}

function getTrustLevel(%obj1, %obj2)
{
	if ($Server::LAN)
	{
		return $TrustLevel::You;
	}
	if (!isObject(%obj1))
	{
		error("ERROR: getBL_IDfromObject() - \"" @ %obj1 @ "\" (%obj1) is not an object");
		return 0;
	}
	if (!isObject(%obj2))
	{
		error("ERROR: getBL_IDfromObject() - \"" @ %obj2 @ "\" (%obj2) is not an object");
		return 0;
	}
	%brickGroup1 = getBrickGroupFromObject(%obj1);
	%bl_id1 = %brickGroup1.bl_id;
	%brickGroup2 = getBrickGroupFromObject(%obj2);
	%bl_id2 = %brickGroup2.bl_id;
	if (%bl_id1 == %bl_id2)
	{
		return $TrustLevel::You;
	}
	if (%brickGroup1.isPublicDomain || %brickGroup2.isPublicDomain)
	{
		return $TrustLevel::Full;
	}
	if (%brickGroup1.Trust[%bl_id2] != %brickGroup2.Trust[%bl_id1])
	{
		$lastError = $LastError::Trust;
		error("ERROR: getTrustLevel() - trust levels between " @ %bl_id1 @ " and " @ %bl_id2 @ " are assymetrical.");
		return $TrustLevel::None;
	}
	else
	{
		$lastError = $LastError::Trust;
		return %brickGroup1.Trust[%bl_id2];
	}
}

function getBL_IDFromObject(%obj)
{
	if (!isObject(%obj))
	{
		error("ERROR: getBL_IDfromObject() - \"" @ %obj @ "\" is not an object");
		return -1;
	}
	%brickGroup = getBrickGroupFromObject(%obj);
	if (isObject(%brickGroup))
	{
		return %brickGroup.bl_id;
	}
	else
	{
		return -1;
	}
}

function getBrickGroupFromObject(%obj)
{
	if (!isObject(%obj))
	{
		error("ERROR: getBrickGroupfromObject() - \"" @ %obj @ "\" is not an object");
		return -1;
	}
	%brickGroup = -1;
	if (%obj.getClassName() $= "GameConnection")
	{
		%brickGroup = %obj.brickGroup;
	}
	else if (%obj.getClassName() $= "SimGroup")
	{
		%brickGroup = %obj;
	}
	else if (%obj.getType() & $TypeMasks::PlayerObjectType)
	{
		if (isObject(%obj.client))
		{
			%brickGroup = %obj.client.brickGroup;
		}
		else if (isObject(%obj.spawnBrick))
		{
			%brickGroup = %obj.spawnBrick.getGroup();
		}
	}
	else if (%obj.getType() & $TypeMasks::ItemObjectType)
	{
		if (isObject(%obj.spawnBrick))
		{
			%brickGroup = %obj.spawnBrick.getGroup();
		}
		else
		{
			%brickGroup = "BrickGroup_" @ %obj.bl_id;
		}
	}
	else if (%obj.getType() & $TypeMasks::FxBrickAlwaysObjectType)
	{
		%brickGroup = %obj.getGroup();
	}
	else if (%obj.getType() & $TypeMasks::VehicleObjectType)
	{
		if (isObject(%obj.spawnBrick))
		{
			%brickGroup = %obj.spawnBrick.getGroup();
		}
	}
	else if (%obj.getType() & $TypeMasks::ProjectileObjectType)
	{
		if (isObject(%obj.client))
		{
			%brickGroup = %obj.client.brickGroup;
		}
	}
	else
	{
		if (isObject(%obj.spawnBrick))
		{
			%brickGroup = %obj.spawnBrick.getGroup();
		}
		if (isObject(%obj.client))
		{
			%brickGroup = %obj.client.brickGroup;
		}
		if (%obj.getGroup().bl_id !$= "")
		{
			%brickGroup = %obj.getGroup();
		}
	}
	return %brickGroup;
}

function serverCmdRequestMiniGameList(%client)
{
	%count = ClientGroup.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		%cl = ClientGroup.getObject(%i);
		%mg = %cl.miniGame;
		if (isObject(%mg))
		{
			if (%cl.miniGame.owner == %cl)
			{
				commandToClient(%client, 'AddMiniGameLine', %mg.getLine(), %mg, %mg.colorIdx);
			}
		}
	}
}

function serverCmdJoinMiniGame(%client, %miniGameID)
{
	if (%client.currentPhase < 3)
	{
		return;
	}
	if (!isObject(%miniGameID))
	{
		return;
	}
	if (%miniGameID.class !$= "MiniGameSO")
	{
		return;
	}
	if (%miniGameID.inviteOnly)
	{
		messageClient(%client, '', 'That mini-game is invite-only.');
		return;
	}
	if (%miniGameID.isMember(%client))
	{
		messageClient(%client, '', 'You\'re already in that mini-game.');
		return;
	}
	if (getSimTime() - %client.miniGameJoinTime < $Game::MiniGameJoinTime)
	{
		messageClient(%client, '', 'You must wait %1 seconds before joining another minigame.', mCeil($Game::MiniGameJoinTime / 1000 - (getSimTime() - %client.miniGameJoinTime) / 1000) + 1);
		return;
	}
	%client.miniGameJoinTime = getSimTime();
	if (isObject(%client.miniGame))
	{
		%client.miniGame.removeMember(%client);
	}
	%miniGameID.addMember(%client);
}

function serverCmdLeaveMiniGame(%client)
{
	if (!isObject(%client.miniGame))
	{
		error("ERROR: serverCmdLeaveMiniGame() - \"" @ %client.getPlayerName() @ "\" is not in a minigame!");
		return;
	}
	%client.miniGame.removeMember(%client);
	%client.miniGame = -1;
}

function serverCmdRemoveFromMiniGame(%client, %victim)
{
	if (!isObject(%client.miniGame))
	{
		return;
	}
	if (%client.miniGame.owner != %client)
	{
		return;
	}
	if (%client.miniGame != %victim.miniGame)
	{
		return;
	}
	%client.miniGame.removeMember(%victim);
	messageClient(%victim, '', '%1 kicked you from the minigame', %client.getPlayerName());
	%client.miniGame.MessageAll('', '%1 kicked %2 from the minigame', %client.getPlayerName(), %victim.getPlayerName());
}

function serverCmdInviteToMiniGame(%client, %victim)
{
	if (!isObject(%client.miniGame))
	{
		return;
	}
	if (%client.miniGame.owner != %client)
	{
		return;
	}
	if (!isObject(%victim))
	{
		return;
	}
	if (%victim.getClassName() !$= "GameConnection")
	{
		return;
	}
	if (%victim.miniGame == %client.miniGame)
	{
		commandToClient(%client, 'MessageBoxOK', 'Mini-Game Invite Error', 'This person is already in the mini-game.');
		return;
	}
	if (isObject(%victim.miniGame))
	{
		commandToClient(%client, 'MessageBoxOK', 'Mini-Game Invite Error', 'This person is already in a different mini-game.');
		return;
	}
	if (%victim.currentPhase < 3)
	{
		commandToClient(%client, 'MessageBoxOK', 'Mini-Game Invite Error', 'This person hasn\'t connected yet.');
		return;
	}
	%ourBL_ID = %client.getBLID();
	if (%victim.Ignore[%ourBL_ID] == 1)
	{
		commandToClient(%client, 'MessageBoxOK', 'Mini-Game Invite Error', 'This person is ignoring you.');
		return;
	}
	if (%victim.miniGameInvitePending == %client.miniGame)
	{
		commandToClient(%client, 'MessageBoxOK', 'Mini-Game Invite Error', 'This person hasn\'t responded to your first invite yet.');
		return;
	}
	else if (%victim.miniGameInvitePending > 0)
	{
		commandToClient(%client, 'MessageBoxOK', 'Mini-Game Invite Error', 'This person is considering another invite right now.');
		return;
	}
	%victim.miniGameInvitePending = %client.miniGame;
	commandToClient(%victim, 'MiniGameInvite', %client.miniGame.title, %client.getPlayerName(), %client.getBLID(), %client.miniGame);
}

function serverCmdAcceptMiniGameInvite(%client, %miniGameID)
{
	if (%miniGameID != %client.miniGameInvitePending)
	{
		echo("response does not equal pending invite");
		return;
	}
	if (!isObject(%miniGameID))
	{
		return;
	}
	if (%miniGameID.class !$= "MiniGameSO")
	{
		return;
	}
	%miniGameID.addMember(%client);
	%client.miniGameInvitePending = 0;
}

function serverCmdRejectMiniGameInvite(%client, %miniGameID)
{
	if (%miniGameID != %client.miniGameInvitePending)
	{
		return;
	}
	%mg = %client.miniGameInvitePending;
	messageClient(%mg.owner, '', '%1 rejected your mini-game invitation', %client.getPlayerName());
	%client.miniGameInvitePending = 0;
}

function serverCmdIgnoreMiniGameInvite(%client, %miniGameID)
{
	if (%miniGameID != %client.miniGameInvitePending)
	{
		return;
	}
	%mg = %client.miniGameInvitePending;
	if (!isObject(%mg))
	{
		return;
	}
	%client.miniGameInvitePending = 0;
	if ($Server::LAN)
	{
		return;
	}
	messageClient(%mg.owner, '', '%1 rejected your mini-game invitation (ignoring)', %client.getPlayerName());
	%targetBL_ID = %mg.owner.getBLID();
	%client.Ignore[%targetBL_ID] = 1;
}

function InitMinigameColors()
{
	%i = -1;
	%i++;
	$MiniGameColorName[%i] = "Red";
	$MiniGameColorI[%i] = "255 0 0";
	$MiniGameColorH[%i] = "#FF0000";
	%i++;
	$MiniGameColorName[%i] = "Orange";
	$MiniGameColorI[%i] = "255 128 0";
	$MiniGameColorH[%i] = "#FF8800";
	%i++;
	$MiniGameColorName[%i] = "Yellow";
	$MiniGameColorI[%i] = "255 255 0";
	$MiniGameColorH[%i] = "#FFFF00";
	%i++;
	$MiniGameColorName[%i] = "Green";
	$MiniGameColorI[%i] = "0 255 0";
	$MiniGameColorH[%i] = "#00FF00";
	%i++;
	$MiniGameColorName[%i] = "Dark Green";
	$MiniGameColorI[%i] = "0 128 0";
	$MiniGameColorH[%i] = "#008800";
	%i++;
	$MiniGameColorName[%i] = "Cyan";
	$MiniGameColorI[%i] = "0 255 255";
	$MiniGameColorH[%i] = "#00FFFF";
	%i++;
	$MiniGameColorName[%i] = "Dark Cyan";
	$MiniGameColorI[%i] = "0 128 128";
	$MiniGameColorH[%i] = "#008888";
	%i++;
	$MiniGameColorName[%i] = "Blue";
	$MiniGameColorI[%i] = "0 128 255";
	$MiniGameColorH[%i] = "#0088FF";
	%i++;
	$MiniGameColorName[%i] = "Pink";
	$MiniGameColorI[%i] = "255 128 255";
	$MiniGameColorH[%i] = "#FF88FF";
	%i++;
	$MiniGameColorName[%i] = "Black";
	$MiniGameColorI[%i] = "0 0 0";
	$MiniGameColorH[%i] = "#000000";
	$MiniGameColorCount = %i + 1;
	for (%i = 0; %i < $MiniGameColorCount; %i++)
	{
		$MiniGameColorF[%i] = colorItoColorF($MiniGameColorI[%i]);
		$MiniGameColorTaken[%i] = 0;
	}
}

function colorItoColorF(%intColor)
{
	%r = getWord(%intColor, 0);
	%g = getWord(%intColor, 1);
	%b = getWord(%intColor, 2);
	%r = %r / 255;
	%g = %g / 255;
	%b = %b / 255;
	return %r SPC %g SPC %b;
}

function serverCmdRequestMiniGameColorList(%client)
{
	for (%i = 0; %i < $MiniGameColorCount; %i++)
	{
		if ($MiniGameColorTaken[%i] == 0)
		{
			commandToClient(%client, 'AddMiniGameColor', %i, $MiniGameColorName[%i], $MiniGameColorI[%i]);
		}
	}
}

function serverCmdCreateMiniGame(%client, %gameTitle, %gameColorIdx, %useSpawnBricks)
{
	%gameTitle = trim(%gameTitle);
	%gameColorIdx = mFloor(%gameColorIdx);
	if (isObject(%client.miniGame))
	{
		if (%client.miniGame.owner == %client)
		{
			commandToClient(%client, 'CreateMiniGameFail', "You're already running a minigame.");
			return;
		}
	}
	if (%gameTitle $= "")
	{
		commandToClient(%client, 'CreateMiniGameFail', "Invalid game title.");
		return;
	}
	if (%gameColorIdx < 0 || %gameColorIdx >= $MiniGameColorCount)
	{
		commandToClient(%client, 'CreateMiniGameFail', "Bad color index.");
		return;
	}
	if ($MiniGameColorTaken[%gameColorIdx] == 1)
	{
		commandToClient(%client, 'CreateMiniGameFail', "Game color is taken.");
		return;
	}
	if (isObject(%client.miniGame))
	{
		%client.miniGame.removeMember(%client);
	}
	messageClient(%client, '', '\c5Mini-game created.');
	CreateMiniGameSO(%client, %gameTitle, %gameColorIdx, %useSpawnBricks);
	commandToClient(%client, 'CreateMiniGameSuccess');
	commandToClient(%client, 'SetPlayingMiniGame', 1);
	commandToClient(%client, 'SetRunningMiniGame', 1);
	commandToAll('AddMiniGameLine', %client.miniGame.getLine(), %client.miniGame, %client.miniGame.colorIdx);
}

function serverCmdEndMiniGame(%client)
{
	if (isObject(%client.miniGame))
	{
		if (%client.miniGame.owner == %client)
		{
			%mg = %client.miniGame;
			%mg.endGame();
			%mg.delete();
		}
		else
		{
			error("ERROR: serverCmdEndMiniGame() - \"" @ %client.getPlayerName() @ "\" tried to end a minigame that he's not in charge of.");
		}
	}
	else
	{
		error("ERROR: serverCmdEndMiniGame() - \"" @ %client.getPlayerName() @ "\" tried to end a minigame when he's not even in one.");
	}
}

function ServerCmdSetMiniGameData(%client, %line)
{
	if (!isObject(%client.miniGame))
	{
		return;
	}
	if (%client.miniGame.owner != %client)
	{
		return;
	}
	%mg = %client.miniGame;
	%fieldCount = getFieldCount(%line);
	%sendUpdate = 0;
	for (%i = 0; %i < %fieldCount; %i++)
	{
		%field = getField(%line, %i);
		%type = getWord(%field, 0);
		if (%type $= "T")
		{
			%title = getSubStr(%field, 2, strlen(%field) - 2);
			%title = getSubStr(%title, 0, 35);
			%mg.title = %title;
			%sendUpdate = 1;
		}
		else if (%type $= "IO")
		{
			%mg.inviteOnly = mFloor(getWord(%field, 1));
			%sendUpdate = 1;
		}
		else if (%type $= "UAPB")
		{
			%mg.useAllPlayersBricks = mFloor(getWord(%field, 1));
		}
		else if (%type $= "PUOB")
		{
			%mg.PlayersUseOwnBricks = mFloor(getWord(%field, 1));
		}
		else if (%type $= "USB")
		{
			%usb = mFloor(getWord(%field, 1));
			if (%mg.useSpawnBricks != %usb)
			{
				%mg.useSpawnBricks = %usb;
				%mg.RespawnAll();
			}
		}
		else if (%type $= "PBB")
		{
			%mg.Points_BreakBrick = mFloor(getWord(%field, 1));
		}
		else if (%type $= "PPB")
		{
			%mg.Points_PlantBrick = mFloor(getWord(%field, 1));
		}
		else if (%type $= "PKP")
		{
			%mg.Points_KillPlayer = mFloor(getWord(%field, 1));
		}
		else if (%type $= "PKS")
		{
			%mg.Points_KillSelf = mFloor(getWord(%field, 1));
		}
		else if (%type $= "PD")
		{
			%mg.Points_Die = mFloor(getWord(%field, 1));
		}
		else if (%type $= "RT")
		{
			%time = getWord(%field, 1) * 1000;
			if (%time < $Game::MinRespawnTime)
			{
				%time = $Game::MinRespawnTime;
			}
			if (%time > $Game::MaxRespawnTime)
			{
				%time = $Game::MaxRespawnTime;
			}
			%mg.respawnTime = %time;
		}
		else if (%type $= "VRT")
		{
			%time = getWord(%field, 1) * 1000;
			if (%time < $Game::MinVehicleRespawnTime)
			{
				%time = $Game::MinVehicleRespawnTime;
			}
			if (%time > $Game::MaxVehicleRespawnTime)
			{
				%time = $Game::MaxVehicleRespawnTime;
			}
			%mg.vehicleReSpawnTime = %time;
		}
		else if (%type $= "BRT")
		{
			%time = getWord(%field, 1) * 1000;
			if (%time < $Game::MinBrickRespawnTime)
			{
				%time = $Game::MinBrickRespawnTime;
			}
			if (%time > $Game::MaxBrickRespawnTime)
			{
				%time = $Game::MaxBrickRespawnTime;
			}
			%mg.brickRespawnTime = %time;
		}
		else if (%type $= "DB")
		{
			%db = mFloor(getWord(%field, 1));
			if (%db.getClassName() $= "PlayerData")
			{
				if (%db.uiName $= "")
				{
					%mg.playerDataBlock = PlayerStandardArmor.getId();
				}
				else
				{
					%mg.playerDataBlock = %db;
				}
			}
			else
			{
				%mg.playerDataBlock = PlayerStandardArmor.getId();
			}
			%mg.updatePlayerDataBlock();
		}
		else if (%type $= "FD")
		{
			%mg.fallingDamage = mFloor(getWord(%field, 1));
		}
		else if (%type $= "WD")
		{
			%mg.weaponDamage = mFloor(getWord(%field, 1));
		}
		else if (%type $= "SD")
		{
			%mg.SelfDamage = mFloor(getWord(%field, 1));
		}
		else if (%type $= "VD")
		{
			%mg.VehicleDamage = mFloor(getWord(%field, 1));
		}
		else if (%type $= "BD")
		{
			%mg.brickDamage = mFloor(getWord(%field, 1));
		}
		else if (%type $= "EW")
		{
			%mg.enableWand = mFloor(getWord(%field, 1));
		}
		else if (%type $= "EB")
		{
			%val = mFloor(getWord(%field, 1));
			if (%mg.EnableBuilding != %val)
			{
				%mg.EnableBuilding = %val;
				%mg.updateEnableBuilding();
			}
		}
		else if (%type $= "EP")
		{
			%val = mFloor(getWord(%field, 1));
			if (%mg.enablePainting != %val)
			{
				%mg.enablePainting = %val;
				%mg.updateEnablePainting();
			}
		}
		else if (%type $= "SE")
		{
			%idx = mFloor(getWord(%field, 1));
			%db = mFloor(getWord(%field, 2));
			if (%mg.startEquip[%idx] != %db)
			{
				if (!isObject(%db))
				{
					%mg.startEquip[%idx] = 0;
				}
				else if (%db.getClassName() $= "ItemData")
				{
					if (%db.uiName !$= "")
					{
						%mg.startEquip[%idx] = %db;
						continue;
					}
					%mg.startEquip[%idx] = 0;
				}
				%mg.forceEquip(%idx);
			}
		}
		else
		{
			error("ERROR: ServerCmdSetMiniGameData() - Unknown type \"" @ %type @ "\"");
		}
	}
	if (%sendUpdate)
	{
		commandToAll('AddMiniGameLine', %mg.getLine(), %mg, %mg.colorIdx);
	}
}

function serverCmdResetMiniGame(%client)
{
	if (!isObject(%client))
	{
		return;
	}
	%mg = %client.miniGame;
	if (!isObject(%mg))
	{
		return;
	}
	if (%mg.owner != %client)
	{
		return;
	}
	%mg.Reset(%client);
}

function CreateMiniGameSO(%client, %title, %colorIdx, %useSpawnBricks)
{
	if ($MiniGameColorTaken[%colorIdx])
	{
		error("ERROR: CreateMiniGameSO() - Color index " @ %colorIdx @ ", \"" @ $MiniGameColorName[%colorIdx] @ "\", is taken!");
		return 0;
	}
	$MiniGameColorTaken[%colorIdx] = 1;
	%mg = new ScriptObject()
	{
		class = MiniGameSO;
		owner = %client;
		title = %title;
		colorIdx = %colorIdx;
		numMembers = 0;
		inviteOnly = 1;
		useAllPlayersBricks = 0;
		PlayersUseOwnBricks = 0;
		useSpawnBricks = %useSpawnBricks;
		Points_BreakBrick = 0;
		Points_PlantBrick = 0;
		Points_KillPlayer = 0;
		Points_KillSelf = 0;
		Points_Die = 0;
		respawnTime = 1;
		vehicleReSpawnTime = 1;
		brickRespawnTime = 1;
		fallingDamage = 1;
		weaponDamage = 0;
		SelfDamage = 0;
		VehicleDamage = 0;
		brickDamage = 0;
		enableWand = 0;
		EnableBuilding = 0;
		playerDataBlock = PlayerStandardArmor.getId();
		StartEquip0 = 0;
		StartEquip1 = 0;
		StartEquip2 = 0;
		StartEquip3 = 0;
		StartEquip4 = 0;
	};
	MiniGameGroup.add(%mg);
	%mg.addMember(%client);
	%client.miniGame = %mg;
	return %mg;
}

function MiniGameSO::addMember(%obj, %client)
{
	if (!isObject(%client))
	{
		error("ERROR: MiniGameSO::AddMember - new member \"" @ %client @ "\" does not exist");
		return;
	}
	if (%client.getClassName() !$= "GameConnection")
	{
		error("ERROR: MiniGameSO::AddMember - new member \"" @ %client @ "\" is not a client.  This function is only for adding clients to the minigame.");
		return;
	}
	if (%obj.isMember(%client))
	{
		return;
	}
	if (isObject(%client.miniGame))
	{
		%client.miniGame.removeMember(%client);
	}
	%obj.member[%obj.numMembers] = %client;
	%obj.numMembers++;
	%client.miniGame = %obj;
	for (%i = 0; %i < %obj.numMembers; %i++)
	{
		%cl = %obj.member[%i];
		messageClient(%cl, 'MsgClientInYourMiniGame', "\c1" @ %client.getPlayerName() @ " joined the mini-game.", %client, 1);
		messageClient(%client, 'MsgClientInYourMiniGame', '', %cl, 1);
	}
	commandToClient(%client, 'SetPlayingMiniGame', 1);
	commandToClient(%client, 'SetBuildingDisabled', !%obj.EnableBuilding);
	commandToClient(%client, 'SetPaintingDisabled', !%obj.enablePainting);
	%client.setScore(0);
	if (!$Server::LAN)
	{
		if ($Pref::Server::ClearEventsOnMinigameChange)
		{
			%client.ClearEventSchedules();
		}
		%client.resetVehicles();
		%mask = $TypeMasks::PlayerObjectType | $TypeMasks::ProjectileObjectType | $TypeMasks::VehicleObjectType | $TypeMasks::CorpseObjectType;
		%client.ClearEventObjects(%mask);
	}
	%client.InstantRespawn();
	%player = %client.Player;
	if (isObject(%player))
	{
		%player.setShapeNameColor($MiniGameColorF[%obj.colorIdx]);
	}
	if (%obj.owner == %client)
	{
		%brickGroup = %client.brickGroup;
		%count = %brickGroup.getCount();
		for (%i = 0; %i < %count; %i++)
		{
			%checkObj = %brickGroup.getObject(%i);
			if (%checkObj.getDataBlock().getId() == brickVehicleSpawnData.getId())
			{
				%checkObj.vehicleMinigameEject();
			}
		}
	}
	else if (%obj.useAllPlayersBricks)
	{
		%brickGroup = %client.brickGroup;
		%count = %brickGroup.getCount();
		for (%i = 0; %i < %count; %i++)
		{
			%checkObj = %brickGroup.getObject(%i);
			if (%checkObj.getDataBlock().getId() == brickVehicleSpawnData.getId())
			{
				%checkObj.vehicleMinigameEject();
			}
		}
	}
}

function MiniGameSO::isMember(%obj, %client)
{
	for (%i = 0; %i < %obj.numMembers; %i++)
	{
		if (%obj.member[%i] == %client)
		{
			return 1;
		}
	}
	return 0;
}

function MiniGameSO::removeMember(%obj, %client)
{
	if (%obj.owner == %client)
	{
		%obj.endGame();
		return;
	}
	for (%i = 0; %i < %obj.numMembers; %i++)
	{
		if (%obj.member[%i] == %client)
		{
			for (%j = %i + 1; %j < %obj.numMembers; %j++)
			{
				%obj.member[%j - 1] = %obj.member[%j];
			}
			%obj.member[%obj.numMembers - 1] = "";
			%obj.numMembers--;
		}
	}
	commandToClient(%client, 'SetPlayingMiniGame', 0);
	commandToClient(%client, 'SetRunningMiniGame', 0);
	for (%i = 0; %i < %obj.numMembers; %i++)
	{
		%cl = %obj.member[%i];
		messageClient(%cl, 'MsgClientInYourMiniGame', "\c1" @ %client.getPlayerName() @ " left the mini-game.", %client, 0);
	}
	%client.setScore(0);
	if (!$Server::LAN)
	{
		if ($Pref::Server::ClearEventsOnMinigameChange)
		{
			%client.ClearEventSchedules();
		}
		%client.resetVehicles();
		%mask = $TypeMasks::PlayerObjectType | $TypeMasks::ProjectileObjectType | $TypeMasks::VehicleObjectType | $TypeMasks::CorpseObjectType;
		%client.ClearEventObjects(%mask);
	}
	%client.miniGame = -1;
	if (isObject(%client.Player))
	{
		%client.InstantRespawn();
	}
	if (%obj.numMembers <= 0)
	{
		%obj.endGame();
		%obj.schedule(10, delete);
	}
	%brickGroup = %client.brickGroup;
	%count = %brickGroup.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		%checkObj = %brickGroup.getObject(%i);
		if (%checkObj.getDataBlock().getId() == brickVehicleSpawnData.getId())
		{
			%checkObj.vehicleMinigameEject();
		}
	}
}

function MiniGameSO::Report(%obj)
{
	echo("");
	echo("MiniGame: ", %obj.title);
	echo("  Color: ", $MiniGameColorName[%obj.colorIdx]);
	echo("");
	echo("  InviteOnly:", %obj.inviteOnly);
	echo("  UseAllPlayersBricks:", %obj.useAllPlayersBricks);
	echo("  PlayersUseOwnBricks:", %obj.PlayersUseOwnBricks);
	echo("  UseSpawnBricks:", %obj.useSpawnBricks);
	echo("");
	echo("  Points_BreakBrick:", %obj.Points_BreakBrick);
	echo("  Points_PlantBrick:", %obj.Points_PlantBrick);
	echo("  Points_KillPlayer:", %obj.Points_KillPlayer);
	echo("  Points_KillSelf:", %obj.Points_KillSelf);
	echo("  Points_Die:", %obj.Points_Die);
	echo("");
	echo("  RespawnTime:", %obj.respawnTime);
	echo("  VehicleRespawnTime:", %obj.vehicleReSpawnTime);
	echo("");
	echo("  JetLevel:", %obj.JetLevel);
	echo("  FallingDamage:", %obj.fallingDamage);
	echo("  WeaponDamage:", %obj.weaponDamage);
	echo("  VehicleDamage:", %obj.VehicleDamage);
	echo("  BrickDamage:", %obj.brickDamage);
	echo("");
	echo("  SelfDamage:", %obj.SelfDamage);
	echo("  EnableWand:", %obj.enableWand);
	echo("  EnableBuilding:", %obj.EnableBuilding);
	echo("");
	for (%i = 0; %i < 5; %i++)
	{
		echo("  StartEquip" @ %i @ ": " @ %obj.startEquip[%i] @ " | " @ %obj.startEquip[%i].uiName);
	}
	echo("  ", %obj.numMembers, " Members:");
	echo("  -------------------------------");
	for (%i = 0; %i < %obj.numMembers; %i++)
	{
		if (%obj.member[%i] == %obj.owner)
		{
			echo("    " @ %obj.member[%i] @ ": " @ %obj.member[%i].getPlayerName(), " <--- Owner");
		}
		else
		{
			echo("    " @ %obj.member[%i] @ ": " @ %obj.member[%i].getPlayerName());
		}
	}
	echo("  -------------------------------");
}

function MiniGameSO::endGame(%obj)
{
	%obj.ending = 1;
	for (%i = 0; %i < %obj.numMembers; %i++)
	{
		%cl = %obj.member[%i];
		%cl.miniGame = 0;
		%player = %cl.Player;
		if (isObject(%player))
		{
			%player.setShapeNameColor("1 1 1");
		}
		commandToClient(%cl, 'SetPlayingMiniGame', 0);
		if (%obj.owner == %cl)
		{
			commandToClient(%cl, 'SetRunningMiniGame', 0);
		}
		messageClient(%cl, '', '\c5The mini-game ended.');
		%cl.setScore(0);
		if (%cl == %obj.owner)
		{
			if (isObject(%cl.Player))
			{
				%cl.Player.GiveDefaultEquipment();
				if (%player.currTool > -1)
				{
					%item = %player.tool[%player.currTool];
					if (isObject(%item))
					{
						%item.onUse(%player, %player.currTool);
						continue;
					}
					%player.unmountImage(0);
				}
				if (%cl.Player.getDamagePercent() >= 1)
				{
					%cl.miniGame = %obj;
					%cl.InstantRespawn();
					%cl.miniGame = 0;
				}
				else
				{
					%cl.Player.setDamageLevel(0);
					if (isObject(%cl.Player.getMountedImage(0)))
					{
						if (%cl.Player.getMountedImage(0).getId() == %cl.Player.getDataBlock().brickImage.getId())
						{
							%cl.Player.mountImage(PlayerStandardArmor.brickImage, 0);
						}
					}
					%cl.Player.setDataBlock(PlayerStandardArmor);
					commandToClient(%cl, 'ShowEnergyBar', PlayerStandardArmor.showEnergyBar);
					fixArmReady(%player);
					applyCharacterPrefs(%cl);
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
	$MiniGameColorTaken[%obj.colorIdx] = 0;
	commandToAll('RemoveMiniGameLine', %obj);
}

function MiniGameSO::Reset(%obj, %client)
{
	if (%client !$= "")
	{
		if (%client.miniGame != %obj)
		{
			return;
		}
		if (%client != %obj.owner)
		{
			if (isObject($InputTarget_["Self"]))
			{
				if ($InputTarget_["Self"].getType() & $TypeMasks::FxBrickAlwaysObjectType)
				{
					if ($InputTarget_["Self"].getGroup() != %obj.owner.brickGroup)
					{
						return;
					}
				}
				else if (%client != %obj.owner)
				{
					return;
				}
			}
			else
			{
				return;
			}
		}
	}
	else
	{
		return;
	}
	%currTime = getSimTime();
	if (%obj.lastResetTime + 5000 > %currTime)
	{
		return;
	}
	%obj.lastResetTime = %currTime;
	if (%obj.useAllPlayersBricks)
	{
		for (%i = 0; %i < %obj.numMembers; %i++)
		{
			%cl = %obj.member[%i];
			%brickGroup = %cl.brickGroup;
			%count = %brickGroup.getCount();
			for (%j = 0; %j < %count; %j++)
			{
				%checkObj = %brickGroup.getObject(%j);
				if (%checkObj.getDataBlock().getId() == brickVehicleSpawnData.getId())
				{
					%checkObj.spawnVehicle(0);
				}
				if (isObject(%checkObj.Item))
				{
					%checkObj.Item.fadeIn(0);
				}
			}
		}
	}
	else
	{
		%brickGroup = %obj.owner.brickGroup;
		%count = %brickGroup.getCount();
		for (%i = 0; %i < %count; %i++)
		{
			%checkObj = %brickGroup.getObject(%i);
			if (%checkObj.getDataBlock().getId() == brickVehicleSpawnData.getId())
			{
				%checkObj.spawnVehicle(0);
			}
			if (isObject(%checkObj.Item))
			{
				%checkObj.Item.fadeIn(0);
			}
		}
	}
	for (%i = 0; %i < %obj.numMembers; %i++)
	{
		%cl = %obj.member[%i];
		%cl.setScore(0);
		if ($Pref::Server::ClearEventsOnMinigameChange)
		{
			%cl.ClearEventSchedules();
		}
		%cl.resetVehicles();
		%mask = $TypeMasks::PlayerObjectType | $TypeMasks::ProjectileObjectType | $TypeMasks::VehicleObjectType | $TypeMasks::CorpseObjectType;
		%cl.ClearEventObjects(%mask);
		%cl.InstantRespawn();
		messageClient(%cl, '', '\c3%1\c5 reset the mini-game', %client.getPlayerName());
	}
}

function MiniGameSO::RespawnAll(%obj, %client)
{
	if (%client !$= "")
	{
		if (%client.miniGame != %obj)
		{
			return;
		}
		if (isObject($InputTarget_["Self"]))
		{
			if ($InputTarget_["Self"].getType() & $TypeMasks::FxBrickAlwaysObjectType)
			{
				if ($InputTarget_["Self"].getGroup() != %obj.owner.brickGroup)
				{
					return;
				}
			}
			else if (%client != %obj.owner)
			{
				return;
			}
		}
		else if (%client != %obj.owner)
		{
			return;
		}
	}
	for (%i = 0; %i < %obj.numMembers; %i++)
	{
		%cl = %obj.member[%i];
		%cl.InstantRespawn();
	}
}

function MiniGameSO::forceEquip(%obj, %slot)
{
	for (%i = 0; %i < %obj.numMembers; %i++)
	{
		%cl = %obj.member[%i];
		%player = %cl.Player;
		if (isObject(%player))
		{
			if (%player.tool[%slot] != %obj.startEquip[%slot])
			{
				%player.tool[%slot] = %obj.startEquip[%slot];
				messageClient(%cl, 'MsgItemPickup', "", %slot, %player.tool[%slot], 1);
				if (%player.currTool == %slot)
				{
					if (%player.getMountedImage(0) > 0 && %player.getMountedImage(0) != brickImage.getId())
					{
						if (isObject(%obj.startEquip[%slot]))
						{
							%obj.startEquip[%slot].onUse(%player, %slot);
							continue;
						}
						%player.unmountImage(0);
					}
				}
			}
		}
	}
}

function MiniGameSO::updatePlayerDataBlock(%obj)
{
	for (%i = 0; %i < %obj.numMembers; %i++)
	{
		%cl = %obj.member[%i];
		%player = %cl.Player;
		commandToClient(%cl, 'ShowEnergyBar', %obj.playerDataBlock.showEnergyBar);
		if (isObject(%player))
		{
			if (%player.getDataBlock() != %obj.playerDataBlock)
			{
				if (%player.getMountedImage(0) && isObject(%player.getDataBlock().brickImage))
				{
					if (%player.getMountedImage(0).getId() == %player.getDataBlock().brickImage.getId())
					{
						%player.mountImage(%obj.playerDataBlock.brickImage, 0);
					}
				}
				if (!%obj.playerDataBlock.canRide)
				{
					if (%player.getObjectMount())
					{
						%player.getDataBlock().doDismount(%player);
					}
				}
				%player.setDataBlock(%obj.playerDataBlock);
				fixArmReady(%player);
				applyCharacterPrefs(%cl);
			}
		}
	}
}

function MiniGameSO::updateEnableBuilding(%obj)
{
	for (%i = 0; %i < %obj.numMembers; %i++)
	{
		%cl = %obj.member[%i];
		%player = %cl.Player;
		commandToClient(%cl, 'SetBuildingDisabled', !%obj.EnableBuilding);
		if (!%obj.EnableBuilding)
		{
			if (isObject(%player))
			{
				for (%j = 0; %j < %player.getDataBlock().maxItems; %j++)
				{
					%client.inventory[%j] = 0;
					messageClient(%cl, 'MsgSetInvData', "", %j, 0);
				}
				if (isObject(%player.tempBrick))
				{
					%player.tempBrick.delete();
				}
				if (%player.getMountedImage(0) == %player.getDataBlock().brickImage.getId())
				{
					%player.unmountImage(0);
				}
			}
		}
	}
}

function MiniGameSO::updateEnablePainting(%obj)
{
	for (%i = 0; %i < %obj.numMembers; %i++)
	{
		%cl = %obj.member[%i];
		%player = %cl.Player;
		commandToClient(%cl, 'SetPaintingDisabled', !%obj.enablePainting);
		if (!%obj.enablePainting)
		{
			if (isObject(%player))
			{
				%imgName = %player.getMountedImage(0).getName();
				if (strpos(%imgName, "SprayCan") != -1)
				{
					%player.unmountImage(0);
				}
			}
		}
	}
}

function MiniGameSO::BottomPrintAll(%obj, %text, %time, %client)
{
	if (%client !$= "")
	{
		if (%client.miniGame != %obj)
		{
			return;
		}
		if (isObject($InputTarget_["Self"]))
		{
			if ($InputTarget_["Self"].getType() & $TypeMasks::FxBrickAlwaysObjectType)
			{
				if ($InputTarget_["Self"].getGroup() != %obj.owner.brickGroup)
				{
					return;
				}
			}
			else if (%client != %obj.owner)
			{
				return;
			}
		}
		else if (%client != %obj.owner)
		{
			return;
		}
	}
	if (isObject(%client))
	{
		%name = %client.getPlayerName();
		%text = strreplace(%text, "%1", %name);
	}
	for (%i = 0; %i < %obj.numMembers; %i++)
	{
		%cl = %obj.member[%i];
		commandToClient(%cl, 'BottomPrint', %text, %time);
	}
}

function MiniGameSO::CenterPrintAll(%obj, %text, %time, %client)
{
	if (%client !$= "")
	{
		if (%client.miniGame != %obj)
		{
			return;
		}
		if (isObject($InputTarget_["Self"]))
		{
			if ($InputTarget_["Self"].getType() & $TypeMasks::FxBrickAlwaysObjectType)
			{
				if ($InputTarget_["Self"].getGroup() != %obj.owner.brickGroup)
				{
					return;
				}
			}
			else if (%client != %obj.owner)
			{
				return;
			}
		}
		else if (%client != %obj.owner)
		{
			return;
		}
	}
	if (isObject(%client))
	{
		%name = %client.getPlayerName();
		%text = strreplace(%text, "%1", %name);
	}
	for (%i = 0; %i < %obj.numMembers; %i++)
	{
		%cl = %obj.member[%i];
		commandToClient(%cl, 'CenterPrint', %text, %time);
	}
}

function MiniGameSO::ChatMsgAll(%obj, %text, %client)
{
	if (%client !$= "")
	{
		if (%client.miniGame != %obj)
		{
			return;
		}
		if (isObject($InputTarget_["Self"]))
		{
			if ($InputTarget_["Self"].getType() & $TypeMasks::FxBrickAlwaysObjectType)
			{
				if ($InputTarget_["Self"].getGroup() != %obj.owner.brickGroup)
				{
					return;
				}
			}
			else if (%client != %obj.owner)
			{
				return;
			}
		}
		else if (%client != %obj.owner)
		{
			return;
		}
	}
	%obj.chatMessageAll(%client, addTaggedString(%text), %client.getPlayerName(), %client.score);
}

function MiniGameSO::chatMessageAll(%obj, %sender, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10)
{
	for (%i = 0; %i < %obj.numMembers; %i++)
	{
		%cl = %obj.member[%i];
		commandToClient(%cl, 'ChatMessage', %sender, %voiceTag, %voicePitch, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10);
	}
}

function MiniGameSO::MessageAll(%obj, %msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10)
{
	for (%i = 0; %i < %obj.numMembers; %i++)
	{
		%cl = %obj.member[%i];
		messageClient(%cl, %msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10);
	}
}

function MiniGameSO::messageAllExcept(%obj, %exception, %msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10)
{
	for (%i = 0; %i < %obj.numMembers; %i++)
	{
		%cl = %obj.member[%i];
		if (%cl != %exception)
		{
			messageClient(%cl, %msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10);
		}
	}
}

function MiniGameSO::pickSpawnPoint(%obj, %client)
{
	if (%obj.useSpawnBricks)
	{
		if (%obj.useAllPlayersBricks)
		{
			if (%obj.PlayersUseOwnBricks)
			{
				%brickGroup = %client.brickGroup;
				return %brickGroup.getBrickSpawnPoint();
			}
			else
			{
				%totalSpawnPoints = 0;
				$currCheckVal++;
				for (%i = 0; %i < %obj.numMembers; %i++)
				{
					%brickGroup = %obj.member[%i].brickGroup;
					if (%brickGroup.checkVal != $currCheckVal)
					{
						%brickGroup.checkVal = $currCheckVal;
						%totalSpawnPoints += %obj.member[%i].brickGroup.spawnBrickCount;
					}
				}
				if (%totalSpawnPoints <= 0)
				{
					return pickSpawnPoint();
				}
				%rnd = getRandom();
				%finalBrickGroup = 0;
				%currPercent = 0;
				$currCheckVal++;
				for (%i = 0; %i < %obj.numMembers; %i++)
				{
					%brickGroup = %obj.member[%i].brickGroup;
					if (%brickGroup.checkVal != $currCheckVal)
					{
						%currPercent += %obj.member[%i].brickGroup.spawnBrickCount / %totalSpawnPoints;
						if (%currPercent >= %rnd)
						{
							%finalBrickGroup = %brickGroup;
							break;
						}
					}
				}
				if (!isObject(%finalBrickGroup))
				{
					%finalBrickGroup = %obj.member[%obj.numMembers - 1].brickGroup;
				}
				if (isObject(%finalBrickGroup))
				{
					return %finalBrickGroup.getBrickSpawnPoint();
				}
				else
				{
					error("MiniGameSO::PickSpawnPoint() - no brick group found");
					return pickSpawnPoint();
				}
			}
		}
		else
		{
			%brickGroup = %obj.owner.brickGroup;
			return %brickGroup.getBrickSpawnPoint();
		}
	}
	else
	{
		return pickSpawnPoint();
	}
}

function MiniGameSO::getLine(%mg)
{
	%line = %mg.owner.getPlayerName() TAB %mg.owner.getBLID() TAB %mg.title TAB %mg.inviteOnly;
	return %line;
}

function endAllMinigames()
{
	%count = ClientGroup.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		%cl = ClientGroup.getObject(%i);
		if (isObject(%cl.miniGame))
		{
			if (%cl.miniGame.owner == %cl)
			{
				%cl.miniGame.endGame();
			}
		}
	}
}

function miniGameCanUse(%player, %thing)
{
	if ($Server::LAN)
	{
		return 1;
	}
	%miniGame1 = getMiniGameFromObject(%player);
	%miniGame2 = getMiniGameFromObject(%thing);
	if (%miniGame2 != %miniGame1 && getBL_IDFromObject(%player) == getBL_IDFromObject(%thing))
	{
		%doHack = 1;
		if (%thing.getType() & $TypeMasks::PlayerObjectType)
		{
			if (%thing.getControllingClient() > 0)
			{
				%doHack = 0;
			}
		}
		if (%doHack)
		{
			%miniGame2 = %miniGame1;
		}
	}
	if (!isObject(%miniGame1) && !isObject(%miniGame2))
	{
		return -1;
	}
	if (%miniGame1 != %miniGame2)
	{
		$lastError = $LastError::MiniGameDifferent;
		return 0;
	}
	if (%thing.getType() & $TypeMasks::ItemObjectType)
	{
		if (!isObject(%thing.spawnBrick))
		{
			return 1;
		}
	}
	%playerBL_ID = getBL_IDFromObject(%player);
	%thingBL_ID = getBL_IDFromObject(%thing);
	if (%miniGame1.useAllPlayersBricks)
	{
		if (%miniGame1.PlayersUseOwnBricks)
		{
			if (%playerBL_ID == %thingBL_ID)
			{
				return 1;
			}
			else
			{
				$lastError = $LastError::MiniGameNotYours;
				return 0;
			}
		}
		else
		{
			return 1;
		}
	}
	else
	{
		if (%thing.client)
		{
			if (%thing.client.Player == %thing)
			{
				return 1;
			}
		}
		%ownerBL_ID = %miniGame1.owner.getBLID();
		if (%thingBL_ID == %ownerBL_ID)
		{
			return 1;
		}
		else
		{
			$lastError = $LastError::NotInMiniGame;
			return 0;
		}
	}
}

function miniGameCanDamage(%client, %victimObject)
{
	%miniGame1 = getMiniGameFromObject(%client);
	%miniGame2 = getMiniGameFromObject(%victimObject);
	%type = %victimObject.getType();
	if (%miniGame2 != %miniGame1 && getBL_IDFromObject(%client) == getBL_IDFromObject(%victimObject))
	{
		%doHack = 1;
		if (%victimObject.getType() & $TypeMasks::PlayerObjectType)
		{
			if (%victimObject.getControllingClient() > 0)
			{
				%doHack = 0;
			}
		}
		if (%doHack)
		{
			%miniGame2 = %miniGame1;
		}
	}
	if ($Server::LAN)
	{
		if (!isObject(%miniGame1))
		{
			return -1;
		}
		if (%type & $TypeMasks::PlayerObjectType)
		{
			if (isObject(%victimObject.client))
			{
				if (%miniGame1 != %miniGame2)
				{
					return 0;
				}
				if (%miniGame1.weaponDamage)
				{
					return 1;
				}
			}
			else if (%miniGame1.VehicleDamage)
			{
				return 1;
			}
		}
		else if (%type & $TypeMasks::VehicleObjectType)
		{
			if (%miniGame1.VehicleDamage)
			{
				return 1;
			}
		}
		else if (%type & $TypeMasks::FxBrickAlwaysObjectType)
		{
			if (%miniGame1.brickDamage)
			{
				return 1;
			}
		}
		else if (%miniGame1.weaponDamage)
		{
			return 1;
		}
		return 0;
	}
	if (!isObject(%miniGame1) && !isObject(%miniGame2))
	{
		return -1;
	}
	if (%miniGame1 != %miniGame2)
	{
		return 0;
	}
	if (!isObject(%miniGame1))
	{
		return 0;
	}
	%ruleDamage = 0;
	if (%type & $TypeMasks::PlayerObjectType)
	{
		if (isObject(%victimObject.client))
		{
			if (%miniGame1.weaponDamage)
			{
				if (%victimObject.client == %client)
				{
					if (%miniGame1.SelfDamage)
					{
						return 1;
					}
					else
					{
						return 0;
					}
				}
				else
				{
					return 1;
				}
			}
			else
			{
				return 0;
			}
		}
		else if (%miniGame1.VehicleDamage)
		{
			%ruleDamage = 1;
		}
	}
	else if (%type & $TypeMasks::VehicleObjectType)
	{
		if (%miniGame1.VehicleDamage)
		{
			%ruleDamage = 1;
		}
	}
	else if (%type & $TypeMasks::FxBrickAlwaysObjectType)
	{
		if (%miniGame1.brickDamage)
		{
			%ruleDamage = 1;
		}
	}
	else if (%miniGame1.weaponDamage)
	{
		%ruleDamage = 1;
	}
	if (%ruleDamage == 0)
	{
		return 0;
	}
	if (%miniGame1.useAllPlayersBricks)
	{
		return 1;
	}
	else
	{
		%victimBL_ID = getBL_IDFromObject(%victimObject);
		if (%victimBL_ID == %miniGame1.owner.getBLID())
		{
			return 1;
		}
	}
	return 0;
}

function getMiniGameFromObject(%obj)
{
	if (!isObject(%obj))
	{
		return -1;
	}
	%miniGame = -1;
	if (isObject(%obj.miniGame))
	{
		return %obj.miniGame;
	}
	if (%obj.getClassName() $= "GameConnection")
	{
		%miniGame = %obj.miniGame;
	}
	else if (%obj.getType() & $TypeMasks::PlayerObjectType || %obj.getType() & $TypeMasks::CorpseObjectType)
	{
		if (isObject(%obj.client))
		{
			%miniGame = %obj.client.miniGame;
		}
		else if ($Server::LAN)
		{
			if (isObject(%obj.spawnBrick))
			{
				if (isObject(%obj.spawnBrick.client))
				{
					%miniGame = %obj.spawnBrick.client.miniGame;
				}
			}
		}
		else if (isObject(%obj.spawnBrick))
		{
			if (isObject(%obj.spawnBrick.getGroup().getClient()))
			{
				%miniGame = %obj.spawnBrick.getGroup().getClient().miniGame;
			}
		}
	}
	else if (%obj.getType() & $TypeMasks::ItemObjectType)
	{
		if ($Server::LAN)
		{
			return -1;
			if (isObject(%obj.spawnBrick))
			{
				if (isObject(%obj.spawnBrick.client))
				{
					%miniGame = %obj.spawnBrick.client.miniGame;
				}
			}
		}
		else if (isObject(%obj.spawnBrick))
		{
			if (isObject(%obj.spawnBrick.getGroup().getClient()))
			{
				%miniGame = %obj.spawnBrick.getGroup().getClient().miniGame;
			}
		}
		else
		{
			%miniGame = %obj.miniGame;
		}
	}
	else if (%obj.getType() & $TypeMasks::FxBrickAlwaysObjectType)
	{
		if ($Server::LAN)
		{
			return -1;
			if (isObject(%obj.client))
			{
				%miniGame = %obj.client.miniGame;
			}
		}
		else if (isObject(%obj.getGroup().getClient()))
		{
			%miniGame = %obj.getGroup().getClient().miniGame;
		}
	}
	else if (%obj.getType() & $TypeMasks::VehicleObjectType)
	{
		if ($Server::LAN)
		{
			return -1;
			if (isObject(%obj.spawnBrick))
			{
				if (isObject(%obj.spawnBrick.client))
				{
					%miniGame = %obj.spawnBrick.client.miniGame;
				}
			}
		}
		else if (isObject(%obj.spawnBrick))
		{
			if (isObject(%obj.spawnBrick.getGroup().getClient()))
			{
				%miniGame = %obj.spawnBrick.getGroup().getClient().miniGame;
			}
		}
	}
	else if (%obj.getType() & $TypeMasks::ProjectileObjectType)
	{
		if (isObject(%obj.client))
		{
			%miniGame = %obj.client.miniGame;
		}
	}
	else
	{
		if (!isObject(%miniGame))
		{
			if (isObject(%obj.miniGame))
			{
				%miniGame = %obj.miniGame;
			}
		}
		if (!$Server::LAN)
		{
			if (!isObject(%miniGame))
			{
				if (isObject(%obj.spawnBrick))
				{
					if (isObject(%obj.spawnBrick.getGroup().getClient()))
					{
						%miniGame = %obj.spawnBrick.getGroup().getClient().miniGame;
					}
				}
			}
		}
		if (!isObject(%miniGame))
		{
			if (isObject(%obj.client))
			{
				%miniGame = %obj.client.miniGame;
			}
		}
	}
	if (!isObject(%miniGame))
	{
		%miniGame = -1;
	}
	return %miniGame;
}

$MiniGameLevel::None = 0;
$MiniGameLevel::Damage = 1;
$MiniGameLevel::Full = 2;
function getMiniGameLevel(%obj1, %obj2)
{
	%miniGame1 = getMiniGameFromObject(%obj1);
	%miniGame2 = getMiniGameFromObject(%obj2);
	if (%miniGame1 != %miniGame2)
	{
		return $MiniGameLevel::None;
	}
	%bl_id1 = getBL_IDFromObject(%obj1);
	%bl_id2 = getBL_IDFromObject(%obj2);
	%bl_idOwner = %miniGame1.owner.getBLID();
	if (%miniGame1.useAllPlayersBricks)
	{
		if (%miniGame1.PlayersUseOwnBricks)
		{
			if (%bl_id1 == %bl_id2)
			{
				return $MiniGameLevel::Full;
			}
			else
			{
				return $MiniGameLevel::Damage;
			}
		}
		else
		{
			return $MiniGameLevel::Full;
		}
	}
	else
	{
		if (%obj1.getType() & $TypeMasks::PlayerObjectType)
		{
			if (isObject(%obj1.client))
			{
				%obj1RealPlayer = 1;
			}
		}
		if (%obj2.getType() & $TypeMasks::PlayerObjectType)
		{
			if (isObject(%obj2.client))
			{
				%obj2RealPlayer = 1;
			}
		}
		if (%obj1RealPlayer && %obj2RealPlayer)
		{
			return $MiniGameLevel::Full;
		}
		else if (%obj1RealPlayer)
		{
			if (%bl_id2 == %bl_idOwner)
			{
				return $MiniGameLevel::Full;
			}
			else
			{
				return $MiniGameLevel::Damage;
			}
		}
		else if (%obj2RealPlayer)
		{
			if (%bl_id1 == %bl_idOwner)
			{
				return $MiniGameLevel::Full;
			}
			else
			{
				return $MiniGameLevel::Damage;
			}
		}
		else if (%bl_id1 == %bl_id2)
		{
			return $MiniGameLevel::Full;
		}
		else
		{
			return $MiniGameLevel::None;
		}
	}
}

function updateAddOnList()
{
	echo("\n--------- Updating Add-On List ---------");
	deleteVariables("$AddOn__*");
	if (isFile("config/server/ADD_ON_LIST.cs"))
	{
		exec("config/server/ADD_ON_LIST.cs");
	}
	else
	{
		exec("base/server/defaultAddOnList.cs");
	}
	if (isFile("base/server/crapOns_Cache.cs"))
	{
		exec("base/server/crapOns_Cache.cs");
	}
	%dir = "Add-Ons/*/server.cs";
	%fileCount = getFileCount(%dir);
	%filename = findFirstFile(%dir);
	for (%i = 0; %i < %fileCount; %i++)
	{
		%path = filePath(%filename);
		%dirName = getSubStr(%path, strlen("Add-Ons/"), strlen(%path) - strlen("Add-Ons/"));
		%varName = getSafeVariableName(%dirName);
		echo("Checking Add-On " @ %dirName);
		if (!isValidAddOn(%dirName, 1))
		{
			deleteVariables("$AddOn__" @ %varName);
			%filename = findNextFile(%dir);
		}
		else
		{
			if ($Server::Dedicated)
			{
				if (mFloor($AddOn__[%varName]) == 0)
				{
					$AddOn__[%varName] = 1;
				}
				else if (mFloor($AddOn__[%varName]) <= 0)
				{
					$AddOn__[%varName] = -1;
				}
				else
				{
					$AddOn__[%varName] = 1;
				}
			}
			else if (mFloor($AddOn__[%varName]) <= 0)
			{
				$AddOn__[%varName] = -1;
			}
			else
			{
				$AddOn__[%varName] = 1;
			}
			%filename = findNextFile(%dir);
		}
	}
	echo("");
	export("$AddOn__*", "config/server/ADD_ON_LIST.cs");
}

function loadAddOns()
{
	echo("");
	updateAddOnList();
	echo("---------  Loading Add-Ons ---------");
	deleteVariables("$AddOnLoaded__*");
	%dir = "Add-Ons/*/server.cs";
	%fileCount = getFileCount(%dir);
	%filename = findFirstFile(%dir);
	%dirCount = 0;
	if (isFile("Add-Ons/System_ReturnToBlockland/server.cs"))
	{
		%dirNameList[%dirCount] = "System_ReturnToBlockland";
		%dirCount++;
	}
	while (%filename !$= "")
	{
		%path = filePath(%filename);
		%dirName = getSubStr(%path, strlen("Add-Ons/"), strlen(%path) - strlen("Add-Ons/"));
		if (%dirName $= "System_ReturnToBlockland")
		{
			%filename = findNextFile(%dir);
		}
		else
		{
			%dirNameList[%dirCount] = %dirName;
			%dirCount++;
			%filename = findNextFile(%dir);
		}
	}
	for (%addOnItr = 0; %addOnItr < %dirCount; %addOnItr++)
	{
		%dirName = %dirNameList[%addOnItr];
		%varName = getSafeVariableName(%dirName);
		if (!$Server::Dedicated)
		{
			if (getRealTime() - $lastProgressBarTime > 200)
			{
				LoadingProgress.setValue(%addOnItr / %dirCount);
				$lastProgressBarTime = getRealTime();
				Canvas.repaint();
			}
		}
		if ($AddOn__[%varName] $= "" || !isValidAddOn(%dirName))
		{
		}
		else if ($AddOn__[%varName] !$= "1")
		{
		}
		else if ($AddOnLoaded__[%varName] == 1)
		{
		}
		else
		{
			$AddOnLoaded__[%varName] = 1;
			%zipFile = "Add-Ons/" @ %dirName @ ".zip";
			if (isFile(%zipFile))
			{
				%zipCRC = getFileCRC(%zipFile);
				echo("\c4Loading Add-On: " @ %dirName @ " \c1(CRC:" @ %zipCRC @ ")");
			}
			else
			{
				echo("\c4Loading Add-On: " @ %dirName);
			}
			if (VerifyAddOnScripts(%dirName) == 0)
			{
				echo("\c2ADD-ON \"" @ %dirName @ "\" CONTAINS SYNTAX ERRORS\n");
			}
			else
			{
				%oldDBCount = DataBlockGroup.getCount();
				exec("Add-Ons/" @ %dirName @ "/server.cs");
				%dbDiff = DataBlockGroup.getCount() - %oldDBCount;
				echo("\c1" @ %dbDiff @ " datablocks added.");
				echo("");
			}
		}
	}
	echo("");
}

function VerifyAddOnScripts(%dirName)
{
	%pattern = "Add-Ons/" @ %dirName @ "/*.cs";
	for (%file = findFirstFile(%pattern); %file !$= ""; %file = findNextFile(%pattern))
	{
		if (getFileLength(%file) > 0)
		{
			if (compile(%file) == 0)
			{
				return 0;
			}
		}
	}
	return 1;
}

$Error::None = 1;
$Error::AddOn_Nested = -1;
$Error::AddOn_Disabled = -2;
$Error::AddOn_NotFound = -3;
function LoadRequiredAddOn(%dirName)
{
	if (strstr(%dirName, " ") != -1)
	{
		%dirName = strreplace(%dirName, " ", "_");
	}
	if (strstr(%dirName, "/") != -1)
	{
		return $Error::AddOn_Nested;
	}
	%varName = getSafeVariableName(%dirName);
	if ($AddOnLoaded__[%varName] == 1)
	{
		return $Error::None;
	}
	if ($AddOn__[%varName] $= "1")
	{
		%serverLaunchName = "Add-Ons/" @ %dirName @ "/server.cs";
		if (!isFile(%serverLaunchName))
		{
			return $Error::AddOn_NotFound;
		}
		echo("  Loading Add-On \"" @ %dirName @ "\"");
		exec(%serverLaunchName);
		$AddOnLoaded__[%varName] = 1;
		return $Error::None;
	}
	else
	{
		return $Error::AddOn_Disabled;
	}
}

function ForceRequiredAddOn(%dirName)
{
	if (strstr(%dirName, " ") != -1)
	{
		%dirName = strreplace(%dirName, " ", "_");
	}
	if (strstr(%dirName, "/") != -1)
	{
		return $Error::AddOn_Nested;
	}
	%varName = getSafeVariableName(%dirName);
	if ($AddOnLoaded__[%varName] == 1)
	{
		return $Error::None;
	}
	if ($AddOn__[%varName] $= "" || !isValidAddOn(%dirName))
	{
		error("ERROR: ForceRequiredAddOn() - \"" @ %dirName @ "\" is not a valid add-on");
		return $Error::AddOn_NotFound;
	}
	%serverLaunchName = "Add-Ons/" @ %dirName @ "/server.cs";
	if (!isFile(%serverLaunchName))
	{
		error("ERROR: ForceRequiredAddOn() - \"" @ %dirName @ "\" server.cs not found (this should never happen)");
		return $Error::AddOn_NotFound;
	}
	echo("  Loading Add-On \"" @ %dirName @ "\"");
	exec(%serverLaunchName);
	$AddOnLoaded__[%varName] = 1;
	if ($AddOn__[%varName] $= "1")
	{
		return $Error::None;
	}
	else
	{
		return $Error::AddOn_Disabled;
	}
}

function isValidAddOn(%dirName, %verbose)
{
	%dirName = strlwr(%dirName);
	if (strstr(%dirName, "/") != -1)
	{
		if (%verbose)
		{
			warn("    nested add-on - will not execute");
		}
		return 0;
	}
	if (strstr(%dirName, "_") == -1)
	{
		if (%verbose)
		{
			warn("    Add-On folder does not follow the format <category>_<name> - will not execute");
		}
		return 0;
	}
	%descriptionName = "Add-Ons/" @ %dirName @ "/description.txt";
	if (!isFile(%descriptionName))
	{
		if (%verbose)
		{
			warn("    No description.txt for this add-on - will not execute");
		}
		return 0;
	}
	if (strstr(%dirName, "Copy of") != -1 || strstr(%dirName, "- Copy") != -1)
	{
		if (%verbose)
		{
			warn("    Add-On folder is a copy - will not execute");
		}
		return 0;
	}
	if (strstr(%dirName, "(") != -1 || strstr(%dirName, ")") != -1)
	{
		if (%verbose)
		{
			warn("    Add-On folder contains ()'s, possibly a duplicate - will not execute");
		}
		return 0;
	}
	%wordCount = getWordCount(%dirName);
	if (%wordCount > 1)
	{
		%lastWord = getWord(%dirName, %wordCount - 1);
		%floorLastWord = mFloor(%lastWord);
		if (%floorLastWord $= %lastWord)
		{
			if (%verbose)
			{
				warn("    Add-On folder ends in \" " @ %lastWord @ "\", possibly a duplicate - will not execute");
			}
			return 0;
		}
	}
	if (strstr(%dirName, "+") != -1)
	{
		if (%verbose)
		{
			warn("    Add-On folder contains +'s - will not execute");
		}
		return 0;
	}
	if (strstr(%dirName, "[") != -1 || strstr(%dirName, "]") != -1)
	{
		if (%verbose)
		{
			warn("    Add-On folder contains []'s, possibly a duplicate - will not execute");
		}
		return 0;
	}
	if (strstr(%dirName, " ") != -1)
	{
		if (%verbose)
		{
			warn("    Add-On folder contains a space - will not execute");
		}
		return 0;
	}
	%spaceName = strreplace(%dirName, "_", " ");
	%firstWord = getWord(%spaceName, 0);
	if (mFloor(%firstWord) $= %firstWord)
	{
		if (%verbose)
		{
			warn("    Add-On folder begins with \"" @ %firstWord @ "_\" - will not execute");
		}
		return 0;
	}
	%wordCount = getWordCount(%spaceName);
	%lastWord = getWord(%spaceName, %wordCount - 1);
	if (mFloor(%lastWord) $= %lastWord)
	{
		if (%verbose)
		{
			warn("    Add-On folder ends with \"_" @ %lastWord @ "\" - will not execute");
		}
		return 0;
	}
	%nameCheckFilename = "Add-Ons/" @ %dirName @ "/namecheck.txt";
	if (isFile(%nameCheckFilename))
	{
		%file = new FileObject();
		%file.openForRead(%nameCheckFilename);
		%nameCheck = %file.readLine();
		%file.close();
		%file.delete();
		if (%nameCheck !$= %dirName)
		{
			if (%verbose)
			{
				warn("    Add-On has been renamed from \"" @ %nameCheck @ "\" to \"" @ %dirName @ "\" - will not execute");
			}
			return 0;
		}
	}
	%zipFile = "Add-Ons/" @ %dirName @ ".zip";
	if (isFile(%zipFile))
	{
		%zipCRC = getFileCRC(%zipFile);
		if ($CrapOnCRC_[%zipCRC] == 1)
		{
			if (%verbose)
			{
				warn("    Add-On is in the list of known bad add-on CRCs - will not execute");
			}
			return 0;
		}
	}
	if ($CrapOnName_[%dirName] == 1)
	{
		if (%verbose)
		{
			warn("    Add-On is in the list of known bad add-on names - will not execute");
		}
		return 0;
	}
	if (strstr(%dirName, ".zip") != -1)
	{
		if (%verbose)
		{
			warn("    Add-On folder name contains \".zip\" - will not execute (also please kill yourself)");
		}
		return 0;
	}
	if ($Server::Dedicated)
	{
		if ($CrapOnDedicatedName_[%dirName] == 1)
		{
			if (%verbose)
			{
				warn("    Add-On is in the list of known bad add-on names for dedicated servers - will not execute");
			}
			return 0;
		}
		%zipFile = "Add-Ons/" @ %dirName @ ".zip";
		if (isFile(%zipFile))
		{
			%zipCRC = getFileCRC(%zipFile);
			if ($CrapOnDedicatedCRC_[%zipCRC] == 1)
			{
				if (%verbose)
				{
					warn("    Add-On is in the list of known bad add-on CRCs for dedicated servers - will not execute");
				}
				return 0;
			}
		}
	}
	return 1;
}

function isValidMap(%file)
{
	%path = filePath(%file);
	%dirName = getSubStr(%path, strlen("Add-Ons/"), strlen(%path) - strlen("Add-Ons/"));
	%mapName = fileBase(%file);
	if (!isFile(%file))
	{
		return 0;
	}
	if (%dirName $= "Map_Tutorial")
	{
		return 0;
	}
	if (strstr(%dirName, "/") != -1)
	{
		return 0;
	}
	if (strstr(%dirName, "_") == -1)
	{
		return 0;
	}
	if (strstr(%dirName, "Copy of") != -1 || strstr(%dirName, "- Copy") != -1)
	{
		return 0;
	}
	if (strstr(%dirName, "(") != -1 || strstr(%dirName, ")") != -1)
	{
		return 0;
	}
	%wordCount = getWordCount(%dirName);
	if (%wordCount > 1)
	{
		%lastWord = getWord(%dirName, %wordCount - 1);
		%floorLastWord = mFloor(%lastWord);
		if (%floorLastWord $= %lastWord)
		{
			return 0;
		}
	}
	if (strstr(%dirName, "+") != -1)
	{
		return 0;
	}
	if (strstr(%dirName, "[") != -1 || strstr(%dirName, "]") != -1)
	{
		return 0;
	}
	if (strstr(%dirName, " ") != -1)
	{
		return 0;
	}
	%spaceName = strreplace(%dirName, "_", " ");
	%firstWord = getWord(%spaceName, 0);
	if (mFloor(%firstWord) $= %firstWord)
	{
		return 0;
	}
	%wordCount = getWordCount(%spaceName);
	%lastWord = getWord(%spaceName, %wordCount - 1);
	if (mFloor(%lastWord) $= %lastWord)
	{
		return 0;
	}
	%nameCheckFilename = "Add-Ons/" @ %dirName @ "/namecheck.txt";
	if (isFile(%nameCheckFilename))
	{
		%file = new FileObject();
		%file.openForRead(%nameCheckFilename);
		%nameCheck = %file.readLine();
		%file.close();
		%file.delete();
		if (%nameCheck !$= %dirName)
		{
			return 0;
		}
	}
	%zipFile = "Add-Ons/" @ %dirName @ ".zip";
	if (isFile(%zipFile))
	{
		%zipCRC = getFileCRC(%zipFile);
		if ($CrapOnCRC_[%zipCRC] == 1)
		{
			return 0;
		}
	}
	if ($CrapOnName_[%dirName] == 1)
	{
		return 0;
	}
	if (strstr(%dirName, ".zip") != -1)
	{
		return 0;
	}
	return 1;
}

datablock AudioProfile(printFireSound)
{
	fileName = "base/data/sound/printFire.wav";
	description = AudioClosest3d;
	preload = 1;
};
function loadPrintedBrickTextures()
{
	%arList = "";
	%count = DataBlockGroup.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		%db = DataBlockGroup.getObject(%i);
		if (%db.getClassName() !$= "fxDTSBrickData")
		{
		}
		else
		{
			%newAR = %db.printAspectRatio;
			if (%newAR $= "")
			{
			}
			else if (strpos(%newAR, " ") != -1)
			{
				warn("WARNING: loadPrintedBrickTextures() - Bad aspect ratio name \"" @ %newAR @ "\" on " @ %db.getName() @ " - Cannot have spaces");
			}
			else if (strpos(%newAR, "/") != -1)
			{
				warn("WARNING: loadPrintedBrickTextures() - Bad aspect ratio name \"" @ %newAR @ "\" on " @ %db.getName() @ " - Cannot have /'s");
			}
			else if (strpos(%newAR, "\"") != -1)
			{
				warn("WARNING: loadPrintedBrickTextures() - Bad aspect ratio name \"" @ %newAR @ "\" on " @ %db.getName() @ " - Cannot have \"'s");
			}
			else
			{
				%arCount = getFieldCount(%arList);
				%addtoList = 1;
				for (%j = 0; %j < %arCount; %j++)
				{
					%field = getField(%arList, %j);
					if (%field $= %newAR)
					{
						%addtoList = 0;
						break;
					}
				}
				if (!%addtoList)
				{
				}
				else if (%arList $= "")
				{
					%arList = %newAR;
				}
				else
				{
					%arList = %arList TAB %newAR;
				}
			}
		}
	}
	deleteVariables("$printNameTable*");
	$globalPrintCount = 0;
	%fieldCount = getFieldCount(%arList);
	for (%i = 0; %i < %fieldCount; %i++)
	{
		%field = getField(%arList, %i);
		loadPrintedBrickTexture(%field);
	}
	loadPrintedBrickTexture("Letters");
}

function loadDefaultLetterPrints()
{
	%dir = "Add-Ons/Print_Letters_Default/prints/*.png";
	%fileCount = getFileCount(%dir);
	%filename = findFirstFile(%dir);
	%localPrintCount = 0;
	for (%i = 0; %i < %fileCount; %i++)
	{
		%path = filePath(%filename);
		%dirName = getSubStr(%path, strlen("Add-Ons/"), (strlen(%path) - strlen("Add-Ons/")) - strlen("/prints"));
		%varName = getSafeVariableName(%dirName);
		if ($AddOn__[%varName] $= "")
		{
			%filename = findNextFile(%dir);
		}
		else if ($AddOn__[%varName] !$= "1")
		{
			%filename = findNextFile(%dir);
		}
		else
		{
			%fileBase = fileBase(%filename);
			if (strpos(%fileBase, " ") != -1)
			{
				%filename = findNextFile(%dir);
			}
			else
			{
				%iconFileName = "Add-Ons/" @ %dirName @ "/icons/" @ %fileBase @ ".png";
				if (!isFile(%iconFileName))
				{
					%filename = findNextFile(%dir);
				}
				else
				{
					%idString = "Letters/" @ %fileBase;
					if ($printNameTable[%idString] !$= "")
					{
						%filename = findNextFile(%dir);
					}
					else
					{
						$printNameTable[%idString] = $globalPrintCount;
						setPrintTexture($globalPrintCount, %filename);
						$globalPrintCount++;
						%localPrintCount++;
						%filename = findNextFile(%dir);
					}
				}
			}
		}
	}
	return %localPrintCount;
}

function loadPrintedBrickTexture(%aspectRatio)
{
	$printARStart[%aspectRatio] = $globalPrintCount;
	if (%aspectRatio $= "Letters" && $AddOn__["Print_Letters_Default"] == 1)
	{
		%localPrintCount = loadDefaultLetterPrints();
	}
	else
	{
		%localPrintCount = 0;
	}
	%dir = "Add-Ons/Print_" @ %aspectRatio @ "_*/prints/*.png";
	%fileCount = getFileCount(%dir);
	%filename = findFirstFile(%dir);
	for (%i = 0; %i < %fileCount; %i++)
	{
		%path = filePath(%filename);
		%dirName = getSubStr(%path, strlen("Add-Ons/"), (strlen(%path) - strlen("Add-Ons/")) - strlen("/prints"));
		%varName = getSafeVariableName(%dirName);
		if ($AddOn__[%varName] $= "")
		{
			%filename = findNextFile(%dir);
		}
		else if ($AddOn__[%varName] !$= "1")
		{
			%filename = findNextFile(%dir);
		}
		else if (%dirName $= "Print_Letters_Default")
		{
		}
		else
		{
			%fileBase = fileBase(%filename);
			if (strpos(%fileBase, " ") != -1)
			{
				warn("WARNING: loadPrintedBrickTexture() - Bad print file name \"" @ %filename @ "\" - Cannot have spaces");
				%filename = findNextFile(%dir);
			}
			else
			{
				%iconFileName = "Add-Ons/" @ %dirName @ "/icons/" @ %fileBase @ ".png";
				if (!isFile(%iconFileName))
				{
					warn("WARNING: loadPrintedBrickTexture() - Print \"" @ %filename @ "\" has no icon - skipping");
					%filename = findNextFile(%dir);
				}
				else
				{
					%idString = %aspectRatio @ "/" @ %fileBase;
					if ($printNameTable[%idString] !$= "")
					{
						warn("WARNING: loadPrintedBrickTexture() - Print \"" @ %filename @ "\" - " @ %idString @ " already exists - skipping");
						%filename = findNextFile(%dir);
					}
					else
					{
						$printNameTable[%idString] = $globalPrintCount;
						setPrintTexture($globalPrintCount, %filename);
						$globalPrintCount++;
						%localPrintCount++;
						%filename = findNextFile(%dir);
					}
				}
			}
		}
	}
	$printARNumPrints[%aspectRatio] = %localPrintCount;
	$printAREnd[%aspectRatio] = $globalPrintCount;
}

function sendLetterPrintInfo(%client)
{
	commandToClient(%client, 'SetLetterPrintInfo', $printARStart["Letters"], $printARNumPrints["Letters"]);
}

datablock ParticleData(printGunParticle)
{
	dragCoefficient = 3;
	windCoefficient = 0;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 280;
	lifetimeVarianceMS = 20;
	spinSpeed = 10;
	spinRandomMin = -50;
	spinRandomMax = 50;
	useInvAlpha = 0;
	animateTexture = 0;
	textureName = "~/data/particles/thinRing";
	colors[0] = "0.000 0.317 0.745 0.500";
	colors[1] = "0.000 0.317 0.745 0.000";
	sizes[0] = 0.2;
	sizes[1] = 1.5;
	times[0] = 0;
	times[1] = 1;
};
datablock ParticleEmitterData(printGunEmitter)
{
	ejectionPeriodMS = 3;
	periodVarianceMS = 0;
	ejectionVelocity = 20;
	velocityVariance = 0;
	ejectionOffset = 0;
	thetaMin = 0;
	thetaMax = 0;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = printGunParticle;
};
datablock ItemData(printGun)
{
	category = "Tools";
	className = "Weapon";
	shapeFile = "~/data/shapes/printGun.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = 1;
	uiName = "Printer";
	iconName = "base/client/ui/itemIcons/Printer";
	doColorShift = 1;
	colorShiftColor = "0.996 0.996 0.910 1.000";
	image = printGunImage;
	canDrop = 1;
};
datablock ShapeBaseImageData(printGunImage)
{
	shapeFile = "~/data/shapes/printGun.dts";
	emap = 0;
	mountPoint = 0;
	offset = "0 0 0";
	eyeOffset = "0.7 1.2 -0.55";
	correctMuzzleVector = 0;
	className = "WeaponImage";
	Item = sprayCan;
	ammo = " ";
	Projectile = "";
	projectileType = Projectile;
	melee = 1;
	doRetraction = 0;
	armReady = 1;
	showBricks = 1;
	doColorShift = 1;
	colorShiftColor = "0.996 0.996 0.910 1.000";
	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0;
	stateTransitionOnTimeout[0] = "Ready";
	stateName[1] = "Ready";
	stateTransitionOnTriggerDown[1] = "Fire";
	stateAllowImageChange[1] = 1;
	stateName[2] = "Fire";
	stateScript[2] = "onFire";
	stateFire[2] = 1;
	stateAllowImageChange[2] = 1;
	stateTimeoutValue[2] = 0.25;
	stateTransitionOnTimeout[2] = "Reload";
	stateTransitionOnTriggerUp[2] = "Ready";
	stateEmitter[2] = printGunEmitter;
	stateEmitterTime[2] = 0.15;
	stateEmitterNode[2] = "muzzleNode";
	stateSound[2] = printFireSound;
	stateSequence[2] = "fire";
	stateName[3] = "Reload";
	stateTransitionOnTriggerUp[3] = "Ready";
};
function printGunImage::onFire(%this, %player, %slot)
{
	%start = %player.getEyePoint();
	%vec = %player.getEyeVector();
	%end = VectorAdd(%start, VectorScale(%vec, 10));
	%mask = $TypeMasks::FxBrickAlwaysObjectType;
	%raycast = containerRayCast(%start, %end, %mask, 0);
	if (!%raycast)
	{
		return;
	}
	%hitObj = getWord(%raycast, 0);
	%hitPos = getWords(%raycast, 1, 3);
	%hitNormal = getWords(%raycast, 4, 6);
	%this.onHitObject(%player, %slot, %hitObj, %hitPos, %hitNormal);
}

function printGunImage::onHitObject(%this, %player, %slot, %hitObj, %hitPos, %hitNormal)
{
	%client = %player.client;
	if (%hitObj.getType() & $TypeMasks::FxBrickAlwaysObjectType)
	{
		%colData = %hitObj.getDataBlock();
		if (%colData.printAspectRatio !$= "")
		{
			if (getTrustLevel(%player, %hitObj) < $TrustLevel::Print)
			{
				commandToClient(%client, 'CenterPrint', %hitObj.getGroup().name @ " does not trust you enough to do that.", 1);
				return;
			}
			%ar = %colData.printAspectRatio;
			commandToClient(%client, 'openPrintSelectorDlg', %ar, $printARStart[%ar], $printARNumPrints[%ar]);
			%client.printBrick = %hitObj;
		}
	}
}

function CenterPrintAll(%message, %time, %lines)
{
	if (%lines $= "" || %lines > 3 || %lines < 1)
	{
		%lines = 1;
	}
	%count = ClientGroup.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		%cl = ClientGroup.getObject(%i);
		if (!%cl.isAIControlled())
		{
			commandToClient(%cl, 'centerPrint', %message, %time, %lines);
		}
	}
}

function BottomPrintAll(%message, %time, %lines)
{
	if (%lines $= "" || %lines > 3 || %lines < 1)
	{
		%lines = 1;
	}
	%count = ClientGroup.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		%cl = ClientGroup.getObject(%i);
		if (!%cl.isAIControlled())
		{
			commandToClient(%cl, 'bottomPrint', %message, %time, %lines);
		}
	}
}

function CenterPrint(%client, %message, %time, %lines)
{
	if (%lines $= "" || %lines > 3 || %lines < 1)
	{
		%lines = 1;
	}
	commandToClient(%client, 'CenterPrint', %message, %time, %lines);
}

function BottomPrint(%client, %message, %time, %lines)
{
	if (%lines $= "" || %lines > 3 || %lines < 1)
	{
		%lines = 1;
	}
	commandToClient(%client, 'BottomPrint', %message, %time, %lines);
}

function clearCenterPrint(%client)
{
	commandToClient(%client, 'ClearCenterPrint');
}

function clearBottomPrint(%client)
{
	commandToClient(%client, 'ClearBottomPrint');
}

function clearCenterPrintAll()
{
	%count = ClientGroup.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		%cl = ClientGroup.getObject(%i);
		if (!%cl.isAIControlled())
		{
			commandToClient(%cl, 'ClearCenterPrint');
		}
	}
}

function clearBottomPrintAll()
{
	%count = ClientGroup.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		%cl = ClientGroup.getObject(%i);
		if (!%cl.isAIControlled())
		{
			commandToClient(%cl, 'ClearBottomPrint');
		}
	}
}

function getQuotaObjectFromBrick(%brick)
{
	%brickGroup = %brick.getGroup();
	return getQuotaObjectFromBrickGroup(%brickGroup);
}

function getQuotaObjectFromClient(%client)
{
	%brickGroup = %client.brickGroup;
	return getQuotaObjectFromBrickGroup(%brickGroup);
}

function verifyQuotaNumber(%val, %min, %max, %default)
{
	if (strlen(%val) > 6)
	{
		return %max;
	}
	if (mFloor(%val) !$= %val)
	{
		return %default;
	}
	return mClamp(%val, %min, %max);
}

function getQuotaObjectFromBrickGroup(%brickGroup)
{
	%quotaObject = %brickGroup.QuotaObject;
	if (isObject(%quotaObject))
	{
		return %quotaObject;
	}
	else
	{
		%oldQuotaObject = getCurrentQuotaObject();
		if (%oldQuotaObject != 0)
		{
			setCurrentQuotaObject(GlobalQuota);
		}
		if ($Server::LAN)
		{
			%quotaObject = new QuotaObject();
			%val = verifyQuotaNumber($Pref::Server::QuotaLAN::Schedules, $Game::MinQuota::Schedules, $Game::MaxQuota::Schedules, $Game::DefaultQuota::Schedules);
			$Pref::Server::QuotaLAN::Schedules = %val;
			%quotaObject.setAllocs_Schedules(%val, 5465489);
			%val = verifyQuotaNumber($Pref::Server::QuotaLAN::Misc, $Game::MinQuota::Misc, $Game::MaxQuota::Misc, $Game::DefaultQuota::Misc);
			$Pref::Server::QuotaLAN::Misc = %val;
			%quotaObject.setAllocs_Misc(%val, 5465489);
			%val = verifyQuotaNumber($Pref::Server::QuotaLAN::Projectile, $Game::MinQuota::Projectile, $Game::MaxQuota::Projectile, $Game::DefaultQuota::Projectile);
			$Pref::Server::QuotaLAN::Projectile = %val;
			%quotaObject.setAllocs_Projectile(%val, 5465489);
			%val = verifyQuotaNumber($Pref::Server::QuotaLAN::Item, $Game::MinQuota::Item, $Game::MaxQuota::Item, $Game::DefaultQuota::Item);
			$Pref::Server::QuotaLAN::Item = %val;
			%quotaObject.setAllocs_Item(%val, 5465489);
			%val = verifyQuotaNumber($Pref::Server::QuotaLAN::Environment, $Game::MinQuota::Environment, $Game::MaxQuota::Environment, $Game::DefaultQuota::Environment);
			$Pref::Server::QuotaLAN::Environment = %val;
			%quotaObject.setAllocs_Environment(%val, 5465489);
			%val = verifyQuotaNumber($Pref::Server::QuotaLAN::Player, $Game::MinQuota::Player, $Game::MaxQuota::Player, $Game::DefaultQuota::Player);
			$Pref::Server::QuotaLAN::Player = %val;
			%quotaObject.setAllocs_Player(%val, 5465489);
			%val = verifyQuotaNumber($Pref::Server::QuotaLAN::Vehicle, $Game::MinQuota::Vehicle, $Game::MaxQuota::Vehicle, $Game::DefaultQuota::Vehicle);
			$Pref::Server::QuotaLAN::Vehicle = %val;
			%quotaObject.setAllocs_Vehicle(%val, 5465489);
		}
		else
		{
			%quotaObject = new QuotaObject();
			%val = verifyQuotaNumber($Pref::Server::Quota::Schedules, $Game::MinQuota::Schedules, $Game::MaxQuota::Schedules, $Game::DefaultQuota::Schedules);
			$Pref::Server::Quota::Schedules = %val;
			%quotaObject.setAllocs_Schedules(%val, 5465489);
			%val = verifyQuotaNumber($Pref::Server::Quota::Misc, $Game::MinQuota::Misc, $Game::MaxQuota::Misc, $Game::DefaultQuota::Misc);
			$Pref::Server::Quota::Misc = %val;
			%quotaObject.setAllocs_Misc(%val, 5465489);
			%val = verifyQuotaNumber($Pref::Server::Quota::Projectile, $Game::MinQuota::Projectile, $Game::MaxQuota::Projectile, $Game::DefaultQuota::Projectile);
			$Pref::Server::Quota::Projectile = %val;
			%quotaObject.setAllocs_Projectile(%val, 5465489);
			%val = verifyQuotaNumber($Pref::Server::Quota::Item, $Game::MinQuota::Item, $Game::MaxQuota::Item, $Game::DefaultQuota::Item);
			$Pref::Server::Quota::Item = %val;
			%quotaObject.setAllocs_Item(%val, 5465489);
			%val = verifyQuotaNumber($Pref::Server::Quota::Environment, $Game::MinQuota::Environment, $Game::MaxQuota::Environment, $Game::DefaultQuota::Environment);
			$Pref::Server::Quota::Environment = %val;
			%quotaObject.setAllocs_Environment(%val, 5465489);
			%val = verifyQuotaNumber($Pref::Server::Quota::Player, $Game::MinQuota::Player, $Game::MaxQuota::Player, $Game::DefaultQuota::Player);
			$Pref::Server::Quota::Player = %val;
			%quotaObject.setAllocs_Player(%val, 5465489);
			%val = verifyQuotaNumber($Pref::Server::Quota::Vehicle, $Game::MinQuota::Vehicle, $Game::MaxQuota::Vehicle, $Game::DefaultQuota::Vehicle);
			$Pref::Server::Quota::Vehicle = %val;
			%quotaObject.setAllocs_Vehicle(%val, 5465489);
		}
		QuotaGroup.add(%quotaObject);
		%brickGroup.QuotaObject = %quotaObject;
		if (%oldQuotaObject != 0)
		{
			setCurrentQuotaObject(%oldQuotaObject);
		}
		return %quotaObject;
	}
}

datablock AudioProfile(vehicleExplosionSound)
{
	fileName = "base/data/sound/vehicleExplosion.wav";
	description = AudioDefault3d;
	preload = 1;
};
datablock ParticleData(vehicleExplosionParticle)
{
	dragCoefficient = 3;
	windCoefficient = 0;
	gravityCoefficient = -0.5;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 1900;
	lifetimeVarianceMS = 300;
	spinSpeed = 10;
	spinRandomMin = -50;
	spinRandomMax = 50;
	useInvAlpha = 1;
	animateTexture = 0;
	textureName = "base/data/particles/cloud";
	colors[0] = "0.9 0.3 0.2 0.9";
	colors[1] = "0.0 0.0 0.0 0.0";
	sizes[0] = 4;
	sizes[1] = 10;
	times[0] = 0;
	times[1] = 1;
};
datablock ParticleEmitterData(vehicleExplosionEmitter)
{
	ejectionPeriodMS = 7;
	periodVarianceMS = 0;
	lifetimeMS = 21;
	ejectionVelocity = 8;
	velocityVariance = 1;
	ejectionOffset = 0;
	thetaMin = 0;
	thetaMax = 90;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = "vehicleExplosionParticle";
	uiName = "Vehicle Explosion";
	emitterNode = TenthEmitterNode;
};
datablock ParticleData(vehicleExplosionParticle2)
{
	dragCoefficient = 0.1;
	windCoefficient = 0;
	gravityCoefficient = 2;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 1000;
	lifetimeVarianceMS = 500;
	spinSpeed = 10;
	spinRandomMin = -50;
	spinRandomMax = 50;
	useInvAlpha = 1;
	animateTexture = 0;
	textureName = "base/data/particles/chunk";
	colors[0] = "1.0 1.0 0.0 1.0";
	colors[1] = "1.0 0.0 0.0 0.0";
	sizes[0] = 0.5;
	sizes[1] = 0.5;
	times[0] = 0;
	times[1] = 1;
};
datablock ParticleEmitterData(vehicleExplosionEmitter2)
{
	ejectionPeriodMS = 1;
	periodVarianceMS = 0;
	lifetimeMS = 7;
	ejectionVelocity = 15;
	velocityVariance = 5;
	ejectionOffset = 0;
	thetaMin = 0;
	thetaMax = 90;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = "vehicleExplosionParticle2";
	uiName = "Vehicle Explosion 2";
	emitterNode = TenthEmitterNode;
};
datablock ExplosionData(vehicleExplosion)
{
	lifetimeMS = 150;
	soundProfile = vehicleExplosionSound;
	emitter[0] = vehicleExplosionEmitter;
	emitter[1] = vehicleExplosionEmitter2;
	faceViewer = 1;
	explosionScale = "1 1 1";
	shakeCamera = 1;
	camShakeFreq = "7.0 8.0 7.0";
	camShakeAmp = "1.0 1.0 1.0";
	camShakeDuration = 0.5;
	camShakeRadius = 15;
	lightStartRadius = 14;
	lightEndRadius = 3;
	lightStartColor = "0.9 0.3 0.1";
	lightEndColor = "0 0 0";
	impulseRadius = 10;
	impulseForce = 500;
	radiusDamage = 30;
	damageRadius = 3.5;
};
datablock ProjectileData(vehicleExplosionProjectile)
{
	directDamage = 0;
	radiusDamage = 0;
	damageRadius = 0;
	Explosion = vehicleExplosion;
	DirectDamageType = $DamageType::VehicleExplosion;
	RadiusDamageType = $DamageType::VehicleExplosion;
	explodeOnDeath = 1;
	armingDelay = 0;
	lifetime = 0;
	uiName = "Vehicle Explosion";
};
datablock ParticleData(VehicleBurnParticle)
{
	textureName = "base/data/particles/cloud";
	dragCoefficient = 0;
	gravityCoefficient = -1;
	inheritedVelFactor = 0;
	windCoefficient = 0;
	constantAcceleration = 3;
	lifetimeMS = 1200;
	lifetimeVarianceMS = 100;
	spinSpeed = 0;
	spinRandomMin = -90;
	spinRandomMax = 90;
	useInvAlpha = 0;
	colors[0] = "1   1   0.3 0.0";
	colors[1] = "1   1   0.3 1.0";
	colors[2] = "0.6 0.0 0.0 0.0";
	sizes[0] = 0;
	sizes[1] = 2;
	sizes[2] = 1;
	times[0] = 0;
	times[1] = 0.2;
	times[2] = 1;
};
datablock ParticleEmitterData(VehicleBurnEmitter)
{
	ejectionPeriodMS = 14;
	periodVarianceMS = 4;
	ejectionVelocity = 0;
	ejectionOffset = 1;
	velocityVariance = 0;
	thetaMin = 30;
	thetaMax = 90;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = VehicleBurnParticle;
	uiName = "Vehicle Fire";
};
datablock ParticleData(vehicleFinalExplosionParticle)
{
	dragCoefficient = 1;
	windCoefficient = 0;
	gravityCoefficient = -0.5;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 1900;
	lifetimeVarianceMS = 1000;
	spinSpeed = 10;
	spinRandomMin = -50;
	spinRandomMax = 50;
	useInvAlpha = 1;
	animateTexture = 0;
	textureName = "base/data/particles/cloud";
	colors[0] = "0.0 0.0 0.0 0.5";
	colors[1] = "0.0 0.0 0.0 1.0";
	colors[2] = "0.0 0.0 0.0 0.0";
	sizes[0] = 5;
	sizes[1] = 10;
	sizes[2] = 5;
	times[0] = 0;
	times[1] = 0.1;
	times[2] = 1;
};
datablock ParticleEmitterData(vehicleFinalExplosionEmitter)
{
	ejectionPeriodMS = 7;
	periodVarianceMS = 0;
	lifetimeMS = 21;
	ejectionVelocity = 8;
	velocityVariance = 1;
	ejectionOffset = 0;
	thetaMin = 10;
	thetaMax = 40;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = "vehicleFinalExplosionParticle";
	uiName = "Vehicle Final Explosion 1";
	emitterNode = TwentiethEmitterNode;
};
datablock ParticleData(vehicleFinalExplosionParticle2)
{
	dragCoefficient = 3;
	windCoefficient = 0;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 1000;
	lifetimeVarianceMS = 500;
	spinSpeed = 10;
	spinRandomMin = -500;
	spinRandomMax = 500;
	useInvAlpha = 0;
	animateTexture = 0;
	textureName = "base/data/particles/cloud";
	colors[0] = "1.0 0.5 0.0 1.0";
	colors[1] = "1.0 0.0 0.0 0.0";
	sizes[0] = 1.5;
	sizes[1] = 2.5;
	times[0] = 0;
	times[1] = 1;
};
datablock ParticleEmitterData(vehicleFinalExplosionEmitter2)
{
	ejectionPeriodMS = 1;
	periodVarianceMS = 0;
	lifetimeMS = 15;
	ejectionVelocity = 30;
	velocityVariance = 5;
	ejectionOffset = 0;
	thetaMin = 85;
	thetaMax = 90;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = "vehicleFinalExplosionParticle2";
	uiName = "Vehicle Final Explosion 2";
	emitterNode = TenthEmitterNode;
};
datablock ParticleData(vehicleFinalExplosionParticle3)
{
	dragCoefficient = 13;
	windCoefficient = 0;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 100;
	lifetimeVarianceMS = 50;
	spinSpeed = 10;
	spinRandomMin = -500;
	spinRandomMax = 500;
	useInvAlpha = 0;
	animateTexture = 0;
	textureName = "base/data/particles/star1";
	colors[0] = "1.0 0.5 0.0 1.0";
	colors[1] = "1.0 0.0 0.0 0.0";
	sizes[0] = 15;
	sizes[1] = 0.5;
	times[0] = 0;
	times[1] = 1;
};
datablock ParticleEmitterData(vehicleFinalExplosionEmitter3)
{
	ejectionPeriodMS = 1;
	periodVarianceMS = 0;
	lifetimeMS = 15;
	ejectionVelocity = 30;
	velocityVariance = 5;
	ejectionOffset = 0;
	thetaMin = 85;
	thetaMax = 90;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = "vehicleFinalExplosionParticle3";
	uiName = "Vehicle Final Explosion 3";
	emitterNode = TenthEmitterNode;
};
datablock ExplosionData(vehicleFinalExplosion)
{
	lifetimeMS = 150;
	soundProfile = vehicleExplosionSound;
	emitter[0] = vehicleFinalExplosionEmitter3;
	emitter[1] = vehicleFinalExplosionEmitter2;
	particleEmitter = vehicleFinalExplosionEmitter;
	particleDensity = 20;
	particleRadius = 1;
	faceViewer = 1;
	explosionScale = "1 1 1";
	shakeCamera = 1;
	camShakeFreq = "7.0 8.0 7.0";
	camShakeAmp = "10.0 10.0 10.0";
	camShakeDuration = 0.75;
	camShakeRadius = 15;
	lightStartRadius = 0;
	lightEndRadius = 20;
	lightStartColor = "0.45 0.3 0.1";
	lightEndColor = "0 0 0";
	impulseRadius = 15;
	impulseForce = 1000;
	impulseVertical = 2000;
	radiusDamage = 30;
	damageRadius = 8;
	playerBurnTime = 5000;
};
datablock ProjectileData(vehicleFinalExplosionProjectile)
{
	directDamage = 0;
	radiusDamage = 0;
	damageRadius = 0;
	Explosion = vehicleFinalExplosion;
	DirectDamageType = $DamageType::VehicleExplosion;
	RadiusDamageType = $DamageType::VehicleExplosion;
	explodeOnDeath = 1;
	armingDelay = 0;
	lifetime = 0;
	uiName = "Vehicle Final Explosion";
};
datablock ParticleData(VehicleTireParticle)
{
	textureName = "base/data/particles/chunk";
	dragCoefficient = 0;
	gravityCoefficient = 2;
	windCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 800;
	lifetimeVarianceMS = 300;
	colors[0] = "0 0 0 1";
	colors[1] = "0 0 0 0";
	sizes[0] = 0.25;
	sizes[1] = 0;
	useInvAlpha = 1;
};
datablock ParticleEmitterData(VehicleTireEmitter)
{
	ejectionPeriodMS = 3;
	periodVarianceMS = 0;
	ejectionVelocity = 5;
	velocityVariance = 3;
	ejectionOffset = 0.1;
	thetaMin = 10;
	thetaMax = 30;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvances = 0;
	particles = "VehicleTireParticle";
	uiName = "Vehicle Tire";
};
datablock ParticleData(vehicleSplashMist)
{
	dragCoefficient = 2;
	gravityCoefficient = -0.05;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 400;
	lifetimeVarianceMS = 100;
	useInvAlpha = 0;
	spinRandomMin = -90;
	spinRandomMax = 500;
	textureName = "base/data/particles/cloud";
	colors[0] = "0.7 0.8 1.0 1.0";
	colors[1] = "0.7 0.8 1.0 0.5";
	colors[2] = "0.7 0.8 1.0 0.0";
	sizes[0] = 2.5;
	sizes[1] = 2.5;
	sizes[2] = 5;
	times[0] = 0;
	times[1] = 0.5;
	times[2] = 1;
};
datablock ParticleEmitterData(vehicleSplashMistEmitter)
{
	ejectionPeriodMS = 5;
	periodVarianceMS = 0;
	ejectionVelocity = 3;
	velocityVariance = 2;
	ejectionOffset = 1;
	thetaMin = 85;
	thetaMax = 85;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvances = 0;
	lifetimeMS = 250;
	particles = "vehicleSplashMist";
	uiName = "Vehicle Splash Mist";
	emitterNode = FifthEmitterNode;
};
datablock ParticleData(vehicleBubbleParticle)
{
	dragCoefficient = 0;
	gravityCoefficient = -0.5;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 400;
	lifetimeVarianceMS = 100;
	useInvAlpha = 0;
	textureName = "base/data/particles/cloud";
	colors[0] = "0.7 0.8 1.0 0.4";
	colors[1] = "0.7 0.8 1.0 0.4";
	colors[2] = "0.7 0.8 1.0 0.0";
	sizes[0] = 0.1;
	sizes[1] = 0.3;
	sizes[2] = 0.3;
	times[0] = 0;
	times[1] = 0.5;
	times[2] = 1;
};
datablock ParticleEmitterData(vehicleBubbleEmitter)
{
	ejectionPeriodMS = 1;
	periodVarianceMS = 0;
	ejectionVelocity = 2;
	ejectionOffset = 1.5;
	velocityVariance = 0.5;
	thetaMin = 0;
	thetaMax = 80;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvances = 0;
	particles = "vehicleBubbleParticle";
	uiName = "Vehicle Bubbles";
	emitterNode = FifthEmitterNode;
};
datablock ParticleData(vehicleFoamParticle)
{
	dragCoefficient = 2;
	gravityCoefficient = -0.05;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 400;
	lifetimeVarianceMS = 100;
	useInvAlpha = 0;
	spinRandomMin = -90;
	spinRandomMax = 500;
	textureName = "base/data/particles/cloud";
	colors[0] = "0.7 0.8 1.0 0.20";
	colors[1] = "0.7 0.8 1.0 0.20";
	colors[2] = "0.7 0.8 1.0 0.00";
	sizes[0] = 1.2;
	sizes[1] = 1.4;
	sizes[2] = 2.6;
	times[0] = 0;
	times[1] = 0.5;
	times[2] = 1;
};
datablock ParticleEmitterData(vehicleFoamEmitter)
{
	ejectionPeriodMS = 20;
	periodVarianceMS = 0;
	ejectionVelocity = 3;
	velocityVariance = 1;
	ejectionOffset = 0.75;
	thetaMin = 85;
	thetaMax = 85;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvances = 0;
	particles = "vehicleFoamParticle";
	uiName = "Vehicle Foam";
	emitterNode = GenericEmitterNode;
};
datablock ParticleData(vehicleFoamDropletsParticle)
{
	dragCoefficient = 1;
	gravityCoefficient = 0.2;
	inheritedVelFactor = 0.2;
	constantAcceleration = -0;
	lifetimeMS = 600;
	lifetimeVarianceMS = 0;
	textureName = "base/data/particles/cloud";
	colors[0] = "0.7 0.8 1.0 1.0";
	colors[1] = "0.7 0.8 1.0 0.5";
	colors[2] = "0.7 0.8 1.0 0.0";
	sizes[0] = 0.8;
	sizes[1] = 0.3;
	sizes[2] = 0;
	times[0] = 0;
	times[1] = 0.5;
	times[2] = 1;
};
datablock ParticleEmitterData(vehicleFoamDropletsEmitter)
{
	ejectionPeriodMS = 7;
	periodVarianceMS = 0;
	ejectionVelocity = 2;
	velocityVariance = 1;
	ejectionOffset = 1;
	thetaMin = 60;
	thetaMax = 80;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvances = 0;
	orientParticles = 1;
	particles = "vehicleFoamDropletsParticle";
	uiName = "Vehicle Foam Droplets";
	emitterNode = GenericEmitterNode;
};
datablock ParticleData(vehicleSplashParticle)
{
	dragCoefficient = 1;
	gravityCoefficient = 0.2;
	inheritedVelFactor = 0.2;
	constantAcceleration = -0;
	lifetimeMS = 600;
	lifetimeVarianceMS = 0;
	textureName = "base/data/particles/cloud";
	colors[0] = "0.7 0.8 1.0 1.0";
	colors[1] = "0.7 0.8 1.0 0.5";
	colors[2] = "0.7 0.8 1.0 0.0";
	sizes[0] = 0.5;
	sizes[1] = 0.5;
	sizes[2] = 0.5;
	times[0] = 0;
	times[1] = 0.5;
	times[2] = 1;
};
datablock ParticleEmitterData(vehicleSplashEmitter)
{
	ejectionPeriodMS = 1;
	periodVarianceMS = 0;
	ejectionVelocity = 3;
	velocityVariance = 1;
	ejectionOffset = 0;
	thetaMin = 60;
	thetaMax = 80;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvances = 0;
	orientParticles = 1;
	lifetimeMS = 100;
	particles = "vehicleSplashParticle";
	uiName = "Vehicle Splash";
	emitterNode = TenthEmitterNode;
};
function pingMatchMakerLoop()
{
	if (isEventPending($pingMatchMakerEvent))
	{
		cancel($pingMatchMakerEvent);
	}
	if (!doesAllowConnections())
	{
		return;
	}
	if (getMatchMakerIP() $= "")
	{
	}
	else
	{
		pingMatchmaker();
	}
	%oldQuotaObject = getCurrentQuotaObject();
	if (isObject(%oldQuotaObject))
	{
		setCurrentQuotaObject(GlobalQuota);
	}
	%schduleTime = 30 * 1000 * getTimeScale();
	$pingMatchMakerEvent = schedule(%schduleTime, 0, pingMatchMakerLoop);
	if (isObject(%oldQuotaObject))
	{
		setCurrentQuotaObject(%oldQuotaObject);
	}
}

