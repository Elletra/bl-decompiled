$Game::MaxEventsPerBrick = 100;


function serverCmdAddEvent ( %client, %enabled, %inputEventIdx, %delay, %targetIdx, %NTNameIdx, %outputEventIdx, %par1, %par2, %par3, %par4 )
{
	%clientIsAdmin = 1;

	if ( isObject(%client) )
	{
		%clientIsAdmin = %client.isAdmin;
	}
	else
	{
		%clientIsAdmin = 1;
	}

	if ( $Server::WrenchEventsAdminOnly == 1  &&  !%clientIsAdmin )
	{
		return;
	}

	%brick = %client.wrenchBrick;

	if ( !isObject(%brick) )
	{
		messageClient (%client, '', 'Wrench Error - AddEvent: Brick no longer exists!');
		return;
	}

	if ( getTrustLevel(%client, %brick) < $TrustLevel::WrenchEvents  &&  %brick != $LastLoadedBrick )
	{
		%client.sendTrustFailureMessage ( %brick.getGroup() );
		return;
	}

	if ( %brick.numEvents >= $Game::MaxEventsPerBrick )
	{
		return;
	}

	%brickClass = %brick.getClassName();
	%inputEventName = $InputEvent_Name[%brickClass, %inputEventIdx];

	if ( %inputEventName $= "onPlayerEnterZone"  ||  %inputEventName $= "onPlayerleaveZone"  ||  %inputEventName $= "onInZone" )
	{
		if ( !%clientIsAdmin )
		{
			messageClient (%client, '', "The event \'" @  %inputEventName @  "\' is admin only!");
			return;
		}
	}

	if ( $InputEvent_AdminOnly[%brickClass, %inputEventIdx] )
	{
		if ( !%clientIsAdmin )
		{
			messageClient (%client, '', "The event \'" @  %inputEventName  @ "\' is admin only!");
			return;
		}
	}

	%enabled = mClamp (mFloor(%enabled), 0, 1);
	%delay = mClamp (%delay, 0, 30000);

	if ( %inputEventIdx == -1 )
	{
		%i = mFloor (%brick.numEvents);

		%brick.eventEnabled[%i] = %enabled;
		%brick.eventDelay[%i] = %delay;
		%brick.eventInputIdx[%i] = %inputEventIdx;

		%brick.numEvents++;

		return;
	}

	%inputEventIdx = mClamp ( %inputEventIdx,  0,  $InputEvent_Count[%brickClass] );

	%targetIdx = mClamp ( %targetIdx,  -1,  getFieldCount ( $InputEvent_TargetList[%brickClass, %inputEventIdx] ) );
	%NTNameIdx = mClamp ( %NTNameIdx, 0, %brick.getGroup().NTNameCount - 1 );

	if ( %targetIdx == -1 )
	{
		%targetClass = "fxDTSBrick";
	}
	else
	{
		%targetClass = getWord ( getField ( $InputEvent_TargetList[%brickClass, %inputEventIdx],  %targetIdx ),  1 );
	}

	if ( %targetClass $= "" )
	{
		error ("ERROR: serverCmdAddEvent() - invalid target class.  %inputEventIdx:" @  %inputEventIdx  @ 
			" %targetIdx:" @  %targetIdx);

		return;
	}

	%outputEventIdx = mClamp ( %outputEventIdx,  0,  $OutputEvent_Count[%targetClass] );

	%verifiedPar[1] = "";
	%verifiedPar[2] = "";
	%verifiedPar[3] = "";
	%verifiedPar[4] = "";


	%parameterCount = getFieldCount ( $OutputEvent_parameterList[%targetClass, %outputEventIdx] );

	for ( %i = 1;  %i < %parameterCount + 1;  %i++ )
	{
		%field = getField ( $OutputEvent_parameterList[%targetClass, %outputEventIdx],  %i - 1 );
		%type = getWord (%field, 0);

		if ( %type $= "int" )
		{
			%min = mFloor ( getWord(%field, 1) );
			%max = mFloor ( getWord(%field, 2) );

			%default = mFloor (getWord(%field, 3) );

			%val = %par[%i];

			if ( %val $= "" )
			{
				%val = %default;
			}

			%verifiedPar[%i] = mClamp (%val, %min, %max);
		}
		else if ( %type $= "intList" )
		{
			%wordCount = getWordCount ( %par[%i] );

			if ( %par[%i] $= "ALL" )
			{
				%verifiedPar[%i] = "ALL";
			}
			else
			{
				%verifiedPar[%i] = "";

				for ( %w = 0;  %w < %wordCount;  %w++ )
				{
					%word = atoi ( getWord( %par[%i],  %w ) );

					if ( %w == 0 )
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
		else if ( %type $= "float" )
		{
			%min = atof ( getWord(%field, 1) );
			%max = atof ( getWord(%field, 2) );

			%step = mAbs ( getWord(%field, 3) );
			%default = atof ( getWord(%field, 4) );

			%val = %par[%i];

			if ( %val $= "" )
			{
				%val = %default;
			}

			%val = mClampF (%val, %min, %max);

			%numSteps = mFloor ( ( %val - %min )  /  %step );
			%val = %min + %numSteps * %step;
			%verifiedPar[%i] = %val;
		}
		else if ( %type $= "bool" )
		{
			if ( %par[%i] )
			{
				%verifiedPar[%i] = 1;
			}
			else
			{
				%verifiedPar[%i] = 0;
			}
		}
		else if ( %type $= "string" )
		{
			%maxLength = mFloor ( getWord(%field, 1) );
			%width = mFloor ( getWord(%field, 2) );

			%par[%i] = strreplace (%par[%i], "<font:", "&A01");
			%par[%i] = strreplace (%par[%i], "<color:", "&A02");
			%par[%i] = strreplace (%par[%i], "<bitmap:", "&A03");
			%par[%i] = strreplace (%par[%i], "<shadow:", "&A04");
			%par[%i] = strreplace (%par[%i], "<shadowcolor:", "&A05");
			%par[%i] = strreplace (%par[%i], "<linkcolor:", "&A06");
			%par[%i] = strreplace (%par[%i], "<linkcolorHL:", "&A07");
			%par[%i] = strreplace (%par[%i], "<a:", "&A08");
			%par[%i] = strreplace (%par[%i], "</a>", "&A09");
			%par[%i] = strreplace (%par[%i], "<br>", "&A10");

			%par[%i] = StripMLControlChars ( %par[%i] );

			%par[%i] = strreplace (%par[%i], "&A01", "<font:");
			%par[%i] = strreplace (%par[%i], "&A02", "<color:");
			%par[%i] = strreplace (%par[%i], "&A03", "<bitmap:");
			%par[%i] = strreplace (%par[%i], "&A04", "<shadow:");
			%par[%i] = strreplace (%par[%i], "&A05", "<shadowcolor:");
			%par[%i] = strreplace (%par[%i], "&A06", "<linkcolor:");
			%par[%i] = strreplace (%par[%i], "&A07", "<linkcolorHL:");
			%par[%i] = strreplace (%par[%i], "&A08", "<a:");
			%par[%i] = strreplace (%par[%i], "&A09", "</a>");
			%par[%i] = strreplace (%par[%i], "&A10", "<br>");

			%verifiedPar[%i] = getSubStr (%par[%i], 0, %maxLength);
			%verifiedPar[%i] = chatWhiteListFilter ( %verifiedPar[%i] );
			%verifiedPar[%i] = strreplace (%verifiedPar[%i], ";", "");
		}
		else if (%type $= "datablock")
		{
			%dbClassName = getWord (%field, 1);

			if ( isObject ( %par[%i] ) )
			{
				%newDB = %par[%i].getId();
			}
			else if ( %par[%i] $= "NONE"  ||  %par[%i] $= -1 )
			{
				%newDB = -1;
			}
			else 
			{
				if ( %dbClassName $= "FxLightData" )
				{
					%newDB = "PlayerLight";
				}
				else if ( %dbClassName $= "ItemData" )
				{
					%newDB = "hammerItem";
				}
				else if ( %dbClassName $= "ProjectileData" )
				{
					if ( isObject(gunProjectile) )
					{
						%newDB = "gunProjectile";
					}
					else
					{
						%newDB = "deathProjectile";
					}
				}
				else if ( %dbClassName $= "ParticleEmitterData" )
				{
					%newDB = "PlayerFoamEmitter";
				}
				else if ( %dbClassName $= "Music" )
				{
					%newDB = "musicData_After_School_Special";
				}
				else if ( %dbClassName $= "Sound" )
				{
					%newDB = "lightOnSound";
				}
				else if ( %dbClassName $= "Vehicle" )
				{
					%newDB = "JeepVehicle";
				}
				else if ( %dbClassName $= "PlayerData" )
				{
					%newDB = "PlayerStandardArmor";
				}
				else
				{
					%newDB = -1;
				}

				if ( isObject(%newDB) )
				{
					%newDB = %newDB.getId();
				}
				else
				{
					%newDB = -1;
				}
			}

			if ( !isObject(%newDB) )
			{
				%newDB = -1;
			}

			if ( %newDB != -1 )
			{
				if ( %dbClassName $= "Music" )
				{
					if ( %newDB.getClassName() !$= "AudioProfile" )
					{
						return;
					}

					if ( %newDB.uiName $= "" )
					{
						return;
					}
				}
				else if ( %dbClassName $= "Sound" )
				{
					if ( %newDB.getClassName() !$= "AudioProfile" )
					{
						return;
					}

					if ( %newDB.uiName !$= "" )
					{
						return;
					}

					if ( %newDB.getDescription().isLooping == 1 )
					{
						return;
					}

					if ( !%newDB.getDescription().is3D )
					{
						return;
					}
				}
				else if ( %dbClassName $= "Vehicle" )
				{
					%dbClass = %newDB.getClassName();

					if ( %newDB.uiName $= "" )
					{
						return;
					}

					if ( %dbClass !$= "WheeledVehicleData"  &&  %dbClass !$= "HoverVehicleData"  &&  
						 %dbClass !$= "FlyingVehicleData"  && (%dbClass !$= "PlayerData"  &&  %newDB.rideAble) )
					{
						return;
					}
				}
				else 
				{
					if ( %newDB.getClassName() !$= %dbClassName )
					{
						return;
					}

					if ( %newDB.uiName $= "" )
					{
						return;
					}
				}
			}

			%verifiedPar[%i] = %newDB;
		}
		else if ( %type $= "vector" )
		{
			%x = atof ( getWord ( %par[%i],  0) );
			%y = atof ( getWord ( %par[%i],  1) );
			%z = atof ( getWord ( %par[%i],  2) );

			%mag = atoi ( getWord (%field, 1) );

			if ( %mag == 0 )
			{
				%mag = 200;
			}

			%vec = %x SPC %y SPC %z;

			if ( VectorLen(%vec) > %mag )
			{
				%vec = VectorNormalize (%vec);
				%vec = VectorScale (%vec, %mag);

				%x = atoi ( getWord(%vec, 0) );
				%y = atoi (getWord(%vec, 1) );
				%z = atoi ( getWord(%vec, 2) );
			}

			%verifiedPar[%i] = %x SPC %y SPC %z;
		}
		else if ( %type $= "list" )
		{
			%val = mFloor ( %par[%i] );
			%itemCount = ( getWordCount(%field) - 1 ) / 2;
			%foundMatch = false;

			for ( %j = 0;  %j < %itemCount;  %j++ )
			{
				%idx = %j * 2 + 1;
				%name = getWord (%field, %idx);
				%id = getWord (%field, %idx + 1);

				if ( %val == %id )
				{
					%foundMatch = true;
				}
			}

			if ( !%foundMatch )
			{
				return;
			}

			%verifiedPar[%i] = %val;
		}
		else if ( %type $= "paintColor" )
		{
			%color = %par[%i];

			if ( %client == $LoadingBricks_Client  &&  $LoadingBricks_ColorMethod == 3 )
			{
				%color = $colorTranslation[%color];
			}

			%verifiedPar[%i] = mClamp (%color, 0, $maxSprayColors);
		}
		else
		{
			error ("ERROR: serverCmdAddEvent() - default type validation for type " @  %type  @ "");
			%verifiedPar[%i] = strreplace (%par[%i], ";", "");
		}
	}

	%i = mFloor (%brick.numEvents);

	%brick.eventInputIdx[%i]  = %inputEventIdx;
	%brick.eventTargetIdx[%i] = %targetIdx;
	%brick.eventOutputIdx[%i] = %outputEventIdx;
	%brick.eventEnabled[%i]   = %enabled;
	%brick.eventInput[%i]     = $InputEvent_Name[%brickClass, %inputEventIdx];
	%brick.eventDelay[%i]     = %delay;

	if ( %targetIdx == -1 )
	{
		%targetClass = "FxDTSBrick";

		%brick.eventTarget[%i] = -1;
		%brick.eventNT[%i] = %brick.getGroup().NTName[%NTNameIdx];
	}
	else
	{
		%brick.eventTarget[%i] = getWord ( getField ( $InputEvent_TargetList[%brickClass, %inputEventIdx],  %targetIdx ),  0 );
		%brick.eventNT[%i] = "";
	}

	%brick.eventOutput[%i] = $OutputEvent_Name[%targetClass, %outputEventIdx];

	%brick.eventOutputParameter[%i, 1] = %verifiedPar[1];
	%brick.eventOutputParameter[%i, 2] = %verifiedPar[2];
	%brick.eventOutputParameter[%i, 3] = %verifiedPar[3];
	%brick.eventOutputParameter[%i, 4] = %verifiedPar[4];

	if ( %brick.eventOutput[%i] $= "FireRelay" )
	{
		if ( %brick.eventDelay[%i] < 33 )
		{
			%brick.eventDelay[%i] = 33;
		}
	}


	%brick.eventOutputAppendClient[%i] = $OutputEvent_AppendClient[%targetClass, %outputEventIdx];
	%brick.numEvents++;

	if ( !%brick.implicitCancelEvents )
	{
		%obj = %brick;

		for ( %i = 0;  %i < %obj.numEvents;  %i++ )
		{
			if ( %obj.eventInput[%i] $= "OnRelay"  &&  %obj.eventTarget[%i] != -1  &&  %obj.eventTarget[%i] $= "Self" )
			{
				%outputEvent = %obj.eventOutput[%i];

				if ( %outputEvent $= "fireRelay" )
				{
					%obj.implicitCancelEvents = 1;
					break;
				}
			}
		}
	}
}

function fxDTSBrick::addEvent ( %obj, %enabled, %delay, %inputEvent, %target, %outputEvent, %par1, %par2, %par3, %par4 )
{
	%inputEventIdx = inputEvent_GetInputEventIdx (%inputEvent);

	%targetIdx = inputEvent_GetTargetIndex ("fxDTSBrick", %inputEventIdx, %target);
	%targetClass = inputEvent_GetTargetClass ("fxDTSBrick", %inputEventIdx, %targetIdx);

	%outputEventIdx = outputEvent_GetOutputEventIdx (%targetClass, %outputEvent);

	%client = %obj.getGroup().client;
	%client.wrenchBrick = %obj;

	serverCmdAddEvent (%client, %enabled, %inputEventIdx, %delay, %targetIdx, %NTNameIdx, %outputEventIdx, %par1, %par2, %par3, %par4);
}

function SimObject::addScheduledEvent ( %obj, %scheduleID )
{
	%obj.numScheduledEvents = mFloor (%obj.numScheduledEvents);

	for ( %i = 0;  %i < %obj.numScheduledEvents;  %i++ )
	{
		if ( !isEventPending ( %obj.scheduledEvent[%i] ) )
		{
			%obj.scheduledEvent[%i] = %scheduleID;
			return;
		}
	}

	%obj.scheduledEvent[%obj.numScheduledEvents] = %scheduleID;
	%obj.numScheduledEvents++;
}
