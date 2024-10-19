function SimGroup::getClient(%this)
{
	if ($Server::LAN)
	{
		error("ERROR: SimGroup::getClient() - function should not be used in a LAN game.");
		return -1;
	}
	if (isObject(%this.client))
	{
		if (%this.bl_id == %this.client.bl_id)
		{
			return %this.client;
		}
	}
	%count = ClientGroup.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		%cl = ClientGroup.getObject(%i);
		if (%cl.bl_id == %this.bl_id)
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
		%name = $LoadingBricks_Client.name;
		messageClient(%client, '', %name SPC "is loading bricks right now. Try again later.");
	}
}

function serverCmdReplaceColor(%client, %idx, %color)
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
		%name = $LoadingBricks_Client.name;
		messageClient(%client, '', %name SPC "is loading bricks right now. Try again later.");
	}
}

function serverCmdAppendColor(%client, %color)
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
		%name = $LoadingBricks_Client.name;
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

function serverCmdInitUploadHandshake(%client)
{
	if (!%client.isAdmin)
	{
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
		$LoadingBricks_Name = %client.name;
		$LoadingBricks_FailureCount = 0;
		$LoadingBricks_BrickCount = 0;
		$LoadBrick_LineCount = 0;
		if (isObject($LoadingBricks_Client))
		{
			%name = $LoadingBricks_Client.name;
		}
		else
		{
			%name = $LoadingBricks_Name;
		}
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
		%name = $LoadingBricks_Client.name;
		messageClient(%client, '', %name SPC "is loading bricks right now. Try again later.");
		commandToClient(%client, 'LoadBricksConfirmHandshake', 0);
	}
}

function serverCmdStartSaveFileUpload(%client, %colorMethod)
{
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
		$LoadingBricks_Name = %client.name;
		$LoadingBricks_FailureCount = 0;
		$LoadingBricks_BrickCount = 0;
		$LoadBrick_LineCount = 0;
		%name = $LoadingBricks_Client.name;
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
		%name = $LoadingBricks_Client.name;
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
		%name = $LoadingBricks_Client.name;
	}
	else
	{
		%name = $LoadingBricks_Name;
	}
	MessageAll('', %name @ "'s save file upload timed out.");
	$LoadingBricks_Client = "";
	$LoadingBricks_Name = "";
	$LoadingBricks_ColorMethod = "";
	if (isObject($LoadBrick_FileObj))
	{
		$LoadBrick_FileObj.close();
		$LoadBrick_FileObj.delete();
	}
}

function ServerLoadSaveFile_Start(%fileName)
{
	echo("SERVER LOADBRICKS colormethod = ", $LoadingBricks_ColorMethod);
	$Server_LoadFileObj = new FileObject();
	if (isFile(%fileName))
	{
		$Server_LoadFileObj.openForRead(%fileName);
	}
	else
	{
		$Server_LoadFileObj.openForRead("base/server/temp/temp.bls");
	}
	if ($UINameTableCreated == 0)
	{
		createUiNameTable();
	}
	if ($PrintNameTableCreated == 0)
	{
		createPrintNameTable();
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
}

function ServerLoadSaveFile_ProcessColorData()
{
	echo("ServerLoadSaveFile_ProcessColorData() - ", $LoadingBricks_ColorMethod);
	%colorCount = -1;
	for (%i = 0; %i < 64; %i++)
	{
		if (getWord(getColorIDTable(%i), 3) > 0.001)
		{
			%colorCount++;
		}
	}
	echo("  color count = ", %colorCount);
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
	echo("  color count = ", %colorCount);
	echo("  div count = ", %divCount);
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
			if (%alpha >= 0.0001)
			{
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
					%diff += mAbs(mAbs(%checkAlpha) - mAbs(%alpha));
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
	if (!isObject(ServerGroup))
	{
		return;
	}
	%line = $Server_LoadFileObj.readLine();
	%firstWord = getWord(%line, 0);
	if (%firstWord $= "+-LIGHT")
	{
		if ($LastLoadedBrick)
		{
			%line = getSubStr(%line, 8, strlen(%line) - 8);
			%pos = strpos(%line, "\"");
			%dbName = getSubStr(%line, 0, %pos);
			%db = $uiNameTable_Lights[%dbName];
			$LastLoadedBrick.setLight(%db);
		}
	}
	else if (%firstWord $= "+-EMITTER")
	{
		if ($LastLoadedBrick)
		{
			%line = getSubStr(%line, 10, strlen(%line) - 10);
			%pos = strpos(%line, "\"");
			%dbName = getSubStr(%line, 0, %pos);
			%db = $uiNameTable_Emitters[%dbName];
			%line = getSubStr(%line, %pos + 2, (strlen(%line) - %pos) - 2);
			%dir = getWord(%line, 0);
			$LastLoadedBrick.setEmitter(%db);
			$LastLoadedBrick.setEmitterDirection(%dir);
		}
	}
	else if (%firstWord $= "+-ITEM")
	{
		if ($LastLoadedBrick)
		{
			%line = getSubStr(%line, 7, strlen(%line) - 7);
			%pos = strpos(%line, "\"");
			%dbName = getSubStr(%line, 0, %pos);
			%db = $uiNameTable_Items[%dbName];
			%line = getSubStr(%line, %pos + 2, (strlen(%line) - %pos) - 2);
			%pos = getWord(%line, 0);
			%dir = getWord(%line, 1);
			%respawnTime = getWord(%line, 2);
			$LastLoadedBrick.setItem(%db);
			$LastLoadedBrick.setItemDirection(%dir);
			$LastLoadedBrick.setItemPosition(%pos);
			$LastLoadedBrick.setItemRespawntime(%respawnTime);
		}
	}
	else if (%firstWord $= "+-AUDIOEMITTER")
	{
		if ($LastLoadedBrick)
		{
			%line = getSubStr(%line, 15, strlen(%line) - 15);
			%pos = strpos(%line, "\"");
			%dbName = getSubStr(%line, 0, %pos);
			%db = $uiNameTable_Music[%dbName];
			$LastLoadedBrick.setSound(%db);
		}
	}
	else if (%firstWord $= "+-VEHICLE")
	{
		if ($LastLoadedBrick)
		{
			%line = getSubStr(%line, 10, strlen(%line) - 10);
			%pos = strpos(%line, "\"");
			%dbName = getSubStr(%line, 0, %pos);
			%db = $uiNameTable_Vehicle[%dbName];
			%line = getSubStr(%line, %pos + 2, (strlen(%line) - %pos) - 2);
			%recolorVehicle = getWord(%line, 0);
			$LastLoadedBrick.setVehicle(%db);
			$LastLoadedBrick.setReColorVehicle(%recolorVehicle);
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
	else
	{
		if ($Server::BrickCount >= $Pref::Server::BrickLimit)
		{
			MessageAll('', 'Brick limit reached (%1)', $Pref::Server::BrickLimit);
			ServerLoadSaveFile_End();
			return;
		}
		%quotePos = strstr(%line, "\"");
		%uiName = getSubStr(%line, 0, %quotePos);
		%line = getSubStr(%line, %quotePos + 2, 9999);
		%pos = getWords(%line, 0, 2);
		%angId = getWord(%line, 3);
		%isBaseplate = getWord(%line, 4);
		%colorId = $colorTranslation[mFloor(getWord(%line, 5))];
		%printId = $printNameTable[getWord(%line, 6)];
		%colorFX = getWord(%line, 7);
		%shapeFX = getWord(%line, 8);
		%db = $uiNameTable[%uiName];
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
			if ($Server::LAN)
			{
				BrickGroup_LAN.add(%b);
			}
			else if (isObject($LoadingBricks_BrickGroup))
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
			%b.setTrusted(1);
			%b.setTransform(%trans);
			%err = %b.plant();
			if (%err == 1 || %err == 3 || %err == 5)
			{
				$Load_failureCount++;
				%b.delete();
				$Server::BrickCount++;
				$LastLoadedBrick = 0;
			}
			else
			{
				$LastLoadedBrick = %b;
			}
		}
		else
		{
			error("ERROR: loadBricks() - DataBlock not found for brick named \"", %uiName, "\"");
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
	if (!$Server_LoadFileObj.isEOF())
	{
		if ($Server::ServerType $= "SinglePlayer")
		{
			$LoadSaveFile_Tick_Schedule = schedule(1, 0, ServerLoadSaveFile_Tick);
		}
		else
		{
			$LoadSaveFile_Tick_Schedule = schedule(1, 0, ServerLoadSaveFile_Tick);
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
	%time = getSimTime() - $LoadingBricks_StartTime;
	MessageAll('MsgProcessComplete', $Load_brickCount - $Load_failureCount @ " / " @ $Load_brickCount @ " bricks created in " @ getTimeString(mFloor((%time / 1000) * 100) / 100));
	if (isEventPending($LoadSaveFile_Tick_Schedule))
	{
		cancel($LoadSaveFile_Tick_Schedule);
	}
	if (isEventPending($LoadingBricks_TimeoutSchedule))
	{
		cancel($LoadingBricks_TimeoutSchedule);
	}
	$Server_LoadFileObj.delete();
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

function createPrintNameTable()
{
	$PrintNameTableCreated = 1;
	%count = getNumPrintTextures();
	for (%i = 0; %i < %count; %i++)
	{
		$printNameTable[getPrintTexture(%i)] = %i;
	}
}

function createUiNameTable()
{
	$UINameTableCreated = 1;
	%dbCount = getDataBlockGroupSize();
	for (%i = 0; %i < %dbCount; %i++)
	{
		%db = getDataBlock(%i);
		if (%db.getClassName() $= "FxDTSBrickData")
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
				if (%db.rideAble)
				{
					$uiNameTable_Vehicle[%db.uiName] = %db;
				}
			}
		}
		else if (%db.getClassName() $= "WheeledVehicleData")
		{
			if (%db.uiName !$= "")
			{
				if (%db.rideAble)
				{
					$uiNameTable_Vehicle[%db.uiName] = %db;
				}
			}
		}
		else if (%db.getClassName() $= "FlyingVehicleData")
		{
			if (%db.uiName !$= "")
			{
				if (%db.rideAble)
				{
					$uiNameTable_Vehicle[%db.uiName] = %db;
				}
			}
		}
		else if (%db.getClassName() $= "HoverVehicleData")
		{
			if (%db.uiName !$= "")
			{
				if (%db.rideAble)
				{
					$uiNameTable_Vehicle[%db.uiName] = %db;
				}
			}
		}
	}
}

function serverDirectSaveFileLoad(%fileName, %colorMethod)
{
	echo("direct load ", %fileName);
	if (!isFile(%fileName))
	{
		MessageAll('', "ERROR: File \"" @ %fileName @ "\" not found.  If you are seeing this, you broke something.");
		return;
	}
	$LoadingBricks_ColorMethod = %colorMethod;
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
	MessageAll('MsgUploadStart', "Loading bricks. Please wait.");
	$LoadingBricks_StartTime = getSimTime();
	ServerLoadSaveFile_Start(%fileName);
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
		messageClient(%client, '', "Error: serverCmdEndBrickLoad() - you are not currently loading bricks.");
	}
}

function serverCmdReloadBricks(%client)
{
	if (!%client.isAdmin)
	{
		return;
	}
	if ($LoadingBricks_Client && $LoadingBricks_Client != 1)
	{
		messageClient(%client, '', 'There is another load in progress.');
		return;
	}
	$LoadingBricks_Client = %client;
	$LoadingBricks_BrickGroup = $LoadingBricks_Client.brickGroup;
	MessageAll('MsgUploadStart', "Re-Loading bricks. Please wait.");
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
$Game::MinMountTime = 500;
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

function getWords(%phrase, %start, %end)
{
	if (%start > %end)
	{
		return;
	}
	%returnPhrase = getWord(%phrase, %start);
	if (%start == %end)
	{
		return %returnPhrase;
	}
	for (%i = %start + 1; %i <= %end; %i++)
	{
		%returnPhrase = %returnPhrase @ " " @ getWord(%phrase, %i);
	}
	return %returnPhrase;
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
	%val = mFloor(%val);
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
	if (isObject(ServerGroup) && isObject(ServerConnection))
	{
		return 1;
	}
	else
	{
		return 0;
	}
}

function serverCmdKick(%client, %victim)
{
	if (%client.isAdmin || %client.isSuperAdmin || %client == 0)
	{
		if (!%victim.isAIControlled())
		{
			if (%victim.isSuperAdmin)
			{
				messageClient(%client, 'MsgAdminForce', '\c5You can\'t kick Super-Admins.');
				return;
			}
			else if (%victim.isLocal())
			{
				messageClient(%client, 'MsgAdminForce', '\c5You can\'t kick the local client.');
				return;
			}
			else
			{
				if ($Server::LAN)
				{
					MessageAll('MsgAdminForce', '\c3%1\c2 kicked \c3%2', %client.name, %victim.name);
				}
				else
				{
					MessageAll('MsgAdminForce', '\c3%1\c2 kicked \c3%2\c2(ID: %3)', %client.name, %victim.name, %victim.bl_id, getRawIP(%victim));
				}
				%victim.delete("You have been kicked.");
			}
		}
		else
		{
			%victim.delete('You have been kicked.');
		}
	}
}

function serverCmdBan(%client, %victimID, %victimBL_ID, %banTime, %reason)
{
	if (!%client.isAdmin)
	{
		return;
	}
	%victimID = mFloor(%victimID);
	%victimBL_ID = mFloor(%victimBL_ID);
	%banTime = mFloor(%banTime);
	%reason = StripMLControlChars(%reason);
	%count = ClientGroup.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		%cl = ClientGroup.getObject(%i);
		if (%cl.bl_id == %victimBL_ID)
		{
			if (%cl.isSuperAdmin)
			{
				messageClient(%client, '', '\c5You can\'t ban Super-Admins.');
				return;
			}
			if (%cl.isLocal())
			{
				messageClient(%client, '', '\c5You can\'t ban the local client.');
				return;
			}
		}
	}
	echo("BAN issued by ", %client.name, " BL_ID:", %client.bl_id, " IP:", getRawIP(%client));
	echo("  +- victim = ", %victimID.name);
	echo("  +- victim bl_id = ", %victimBL_ID);
	echo("  +- ban time = ", %banTime);
	echo("  +- ban reason = ", %reason);
	if (!isObject($BanManagerSO))
	{
		CreateBanManager();
	}
	$BanManagerSO.addBan(%client, %victimID, %victimBL_ID, %reason, %banTime);
	$BanManagerSO.saveBans();
	if (%banTime == -1)
	{
		MessageAll('MsgAdminForce', '\c3%1\c2 permanently banned \c3%2\c2 (ID: %3)', %client.name, %victimID.name, %victimBL_ID);
	}
	else
	{
		MessageAll('MsgAdminForce', '\c3%1\c2 banned \c3%2\c2 (ID: %3) for %4 minutes', %client.name, %victimID.name, %victimBL_ID, %banTime);
	}
	%count = ClientGroup.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		%cl = ClientGroup.getObject(%i);
		if (%cl.bl_id == %victimBL_ID)
		{
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
	$BanManagerSO = new ScriptObject()
	{
		class = BanManagerSO;
		numBans = 0;
	};
	$BanManagerSO.loadBans();
}

function BanManagerSO::addBan(%this, %adminID, %victimID, %victimBL_ID, %reason, %banTime)
{
	if (%this.isBanned(%victimBL_ID))
	{
		echo("BanManagerSO::addBan() - Already banned.");
		return;
	}
	%adminName = %adminID.name;
	%adminBL_ID = %adminID.bl_id;
	%victimName = %victimID.name;
	%victimIP = getRawIP(%victimID);
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
	%file.openForWrite("base/config/server/BANLIST.txt");
	for (%i = 0; %i < %this.numBans; %i++)
	{
		%doLine = 1;
		%timeleft = %this.expirationMinute[%i] - getCurrentMinuteOfYear();
		%timeleft += (%this.expirationYear[%i] - getCurrentYear()) * 525600;
		if (%timeleft <= 0)
		{
			%doLine = 0;
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
	for (%file.openForRead("base/config/server/BANLIST.txt"); !%file.isEOF(); %this.numBans++)
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
			%timeleft = %this.expirationMinute[%i] - getCurrentMinuteOfYear();
			%timeleft += (%this.expirationYear[%i] - getCurrentYear()) * 525600;
			%line = %line TAB %timeleft;
			if (%timeleft > 0)
			{
				commandToClient(%client, 'AddUnBanLine', %line, %i);
			}
		}
	}
}

function BanManagerSO::isBanned(%this, %testBL_ID)
{
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

function getDayOfYear(%month, %day)
{
	%dayOfYear = 0;
	for (%i = 1; %i < %month; %i++)
	{
		if (%i == 1)
		{
			%dayOfYear += 31;
		}
		else if (%i == 2)
		{
			%dayOfYear += 28;
		}
		else if (%i == 3)
		{
			%dayOfYear += 31;
		}
		else if (%i == 4)
		{
			%dayOfYear += 30;
		}
		else if (%i == 5)
		{
			%dayOfYear += 31;
		}
		else if (%i == 6)
		{
			%dayOfYear += 30;
		}
		else if (%i == 7)
		{
			%dayOfYear += 31;
		}
		else if (%i == 8)
		{
			%dayOfYear += 31;
		}
		else if (%i == 9)
		{
			%dayOfYear += 30;
		}
		else if (%i == 10)
		{
			%dayOfYear += 31;
		}
		else if (%i == 11)
		{
			%dayOfYear += 30;
		}
		else if (%i == 12)
		{
			%dayOfYear += 31;
		}
	}
	%dayOfYear += %day;
	return %dayOfYear;
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

function serverLoadAvatarName(%arrayName, %fileName)
{
	%file = new FileObject();
	%file.openForRead(%fileName);
	if (%file.isEOF())
	{
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

function serverLoadAccentInfo(%arrayName, %fileName)
{
	%file = new FileObject();
	%file.openForRead(%fileName);
	if (%file.isEOF())
	{
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

function serverLoadMinifigColors()
{
	$minifigColor[0] = "0.867 0.000 0.000 1.000";
	$minifigColor[1] = "0.973 0.800 0.000 1.000";
	$minifigColor[2] = "0.000 0.471 0.196 1.000";
	$minifigColor[3] = "0.000 0.317 0.745 1.000";
	$minifigColor[4] = "0.996 0.996 0.910 1.000";
	$minifigColor[5] = "0.647 0.647 0.647 1.000";
	$minifigColor[6] = "0.471 0.471 0.471 1.000";
	$minifigColor[7] = "0.200 0.200 0.200 1.000";
	$minifigColor[8] = "0.400 0.196 0.000 1.000";
	$minifigColor[9] = "0.667 0.000 0.000 0.700";
	$minifigColor[10] = "1.000 0.500 0.000 0.800";
	$minifigColor[11] = "0.850 1.000 0.000 0.600";
	$minifigColor[12] = "0.990 0.960 0.000 0.800";
	$minifigColor[13] = "0.000 0.471 0.196 0.750";
	$minifigColor[14] = "0.000 0.200 0.640 0.750";
	$minifigColor[15] = "0.550 0.700 1.000 0.700";
	$minifigColor[16] = "0.850 0.850 0.850 0.700";
	$minifigColor[17] = "0.847059 0.894118 0.654902 1.000000";
	$minifigColor[18] = "0.631373 0.764706 0.541176 1.000000";
	$minifigColor[19] = "0.639216 0.737255 0.274510 1.000000";
	$minifigColor[20] = "0.756863 0.792157 0.866667 1.000000";
	$minifigColor[21] = "0.329412 0.647059 0.686275 1.000000";
	$minifigColor[22] = "0.258824 0.329412 0.576471 1.000000";
	$minifigColor[23] = "0.976471 0.909804 0.600000 1.000000";
	$minifigColor[24] = "0.776471 0.819608 0.231373 1.000000";
	$minifigColor[25] = "0.756863 0.854902 0.721569 1.000000";
	$minifigColor[26] = "0.713726 0.839216 0.831373 1.000000";
	$minifigColor[27] = "0.701961 0.819608 0.886275 1.000000";
	$minifigColor[28] = "0.764706 0.439216 0.627451 1.000000";
	$hatColors["none"] = "0 1 2 3 4 5 6 7 8";
	$hatColors["helmet"] = "0 1 2 3 4 5 6 7 8";
	$hatColors["scoutHat"] = "2 8";
	$hatColors["pointyHelmet"] = "6 7";
	$packColors["none"] = "0 1 2 3 4 5 6 7 8";
	$packColors["Armor"] = "6 7";
	$packColors["Bucket"] = "7 8";
	$packColors["Cape"] = "0 1 2 3 4 5 6 7 8";
	$packColors["Pack"] = 8;
	$packColors["Quiver"] = 8;
	$packColors["Tank"] = "0 1 3 4 5 7";
	$torsoColors["decalNone.png"] = "0 1 2 3 4 5 6 7 8";
	$torsoColors["decalBtron.png"] = 4;
	$torsoColors["decalDarth.png"] = 7;
	$torsoColors["decalRedCoat.png"] = 4;
	$torsoColors["decalBlueCoat.png"] = 4;
	$torsoColors["decalMtron.png"] = 0;
	$torsoColors["decalNewSpace.png"] = 4;
	$torsoColors["decalOldSpace.png"] = "0 1 3 4 7";
	$torsoColors["decalCastle1.png"] = "0 1 2 3 4 5 6 7 8";
	$torsoColors["decalArmor.png"] = "0 1 2 3 4 5 6 7 8";
	$torsoColors["decalPirate1.png"] = "0 1 2 3 4 5 6 7 8";
	$torsoColors["decalBeads.png"] = "0 1 2 3 4 5 6 7 8";
	$torsoColors["decalLion.png"] = "0 1 2 3 4 5 6 7 8";
	$normalColors = "0 1 2 3 4 5 6 7 8 17 18 19 20 21 22 23 24 25 26 27 28";
}

function ServerCmdUpdatePrefs(%client, %LANname)
{
	%client.LANname = StripMLControlChars(%LANname);
	if ($Server::LAN)
	{
		if (%client.name !$= %client.LANname)
		{
			if (isObject(%client.Player))
			{
				%client.Player.setShapeName(%client.LANname);
			}
			MessageAll('', "\c3" @ %client.name @ "\c2 is now named \c3" @ %client.LANname);
			%client.name = %client.LANname;
			MessageAll('MsgClientJoin', '', %client.name, %client, %client.bl_id, %client.score, %client.isAIControlled(), %client.isAdmin, %client.isSuperAdmin);
		}
	}
}

function ServerCmdUpdateBodyParts(%client, %hat, %accent, %pack, %secondPack, %chest, %hip, %LLeg, %RLeg, %LArm, %RArm, %LHand, %RHand)
{
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
	hideAllNodes(%player);
	%accentList = $accentsAllowed[$hat[%client.hat]];
	%accentName = getWord(%accentList, %client.accent);
	if (%accentName !$= "none")
	{
		%player.unHideNode(%accentName);
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
	%accentList = $accentsAllowed[$hat[%client.hat]];
	%accentName = getWord(%accentList, %client.accent);
	if (%accentName !$= "none" && %accentName !$= "")
	{
		%player.setNodeColor(%accentName, %client.accentColor);
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
	%player.setFaceName(%client.faceName);
	%player.setDecalName(%client.decalName);
}

function AvatarColorCheck(%color)
{
	%r = getWord(%color, 0);
	%g = getWord(%color, 1);
	%b = getWord(%color, 2);
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

function serverLoadAvatarShapeMenu(%fileName)
{
	%file = new FileObject();
	%file.openForRead("base/data/shapes/player/" @ %fileName @ ".txt");
	if (%file.isEOF())
	{
		return;
	}
	for (%lineCount = 0; !%file.isEOF(); %lineCount++)
	{
		%line = %file.readLine();
		%command = "$" @ %fileName @ "[" @ %lineCount @ "] = " @ %line @ ";";
		eval(%command);
		%objName.itemCount++;
	}
	%file.delete();
}

function applyCharacterPrefs(%client)
{
	if (!isObject(%client.Player))
	{
		return;
	}
	%client.applyBodyParts();
	%client.ApplyBodyColors();
	return;
	%player = %client.Player;
	if ($hat[%client.hat] !$= "none")
	{
		%player.setNodeColor($hat[%client.hat], %client.hatColor);
	}
	if ($pack[%client.pack] !$= "none")
	{
		%player.setNodeColor($pack[%client.pack], %client.packColor);
	}
	%player.setNodeColor(chest, %client.chestColor);
	%player.setNodeColor(Pants, %client.hipColor);
	%player.setNodeColor(LShoe, %client.llegColor);
	%player.setNodeColor(RShoe, %client.rlegColor);
	%player.setNodeColor(larm, %client.larmColor);
	%player.setNodeColor(rarm, %client.rarmColor);
	%player.setNodeColor(lhand, %client.lhandColor);
	%player.setNodeColor(rhand, %client.rhandColor);
	%player.setNodeColor(HeadSkin, %client.headColor);
	%player.setFaceName(%client.faceName);
	%player.setDecalName(%client.decalName);
	hideAllNodes(%player);
	%player.unHideNode($hat[%client.hat]);
	%player.unHideNode($pack[%client.pack]);
	%accentList = $accentsAllowed[$hat[%client.hat]];
	%accentName = getWord(%accentList, %client.accent);
	%player.unHideNode(%accentName);
	if (%accentName !$= "none")
	{
		%player.setNodeColor(%accentName, %client.accentColor);
	}
	if (%client.pack == 0 && %client.secondPack == 0)
	{
		%player.setHeadUp(0);
	}
	else
	{
		%player.setHeadUp(1);
	}
	if (%client.decalColor == 0 || $packs[%client.pack] $= "Armor")
	{
		%player.hideNode(decal);
	}
	else
	{
		%player.unHideNode(decal);
	}
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
	if (%client.isAdmin || %client == 0)
	{
		%fileName = %mapName;
		if (!isFile(%fileName))
		{
			return;
		}
		if (fileExt(%fileName) !$= ".mis")
		{
			return;
		}
		MessageAll('MsgProcessComplete', '\c3%1 \c0changed the map', %client.name);
		endGame();
		loadMission(%fileName);
	}
}

function serverCmdGetMapList(%client)
{
	%nameList = "";
	%i = 0;
	for (%file = findFirstFile($Server::MissionFileSpec); %file !$= ""; %file = findNextFile($Server::MissionFileSpec))
	{
		if (strstr(%file, "CVS/") == -1 && strstr(%file, "common/") == -1)
		{
			%mapEntry = serverGetMissionDisplayName(%file) @ "\t" @ serverGetMissionPreviewImage(%file) @ "\t" @ %file;
			commandToClient(%client, 'updateMapList', %mapEntry);
		}
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
	%previewImage = %MissionInfoObject.previewImage;
	%MissionInfoObject.delete();
	if (%previewImage !$= "")
	{
		return %previewImage;
	}
	else
	{
		return "base/data/missions/default";
	}
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
	%vector = VectorScale(%eyeVec, 10);
	%end = VectorAdd(%start, %vector);
	%searchMasks = $TypeMasks::InteriorObjectType | $TypeMasks::PlayerObjectType | $TypeMasks::StaticShapeObjectType | $TypeMasks::StaticObjectType | $TypeMasks::FxBrickObjectType;
	%scanTarg = containerRayCast(%start, %end, %searchMasks, %player);
	if (%scanTarg)
	{
		%scanObj = getWord(%scanTarg, 0);
		messageClient(%client, '', "objectid = " @ %scanObj);
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
		%name = strlwr(%cl.name);
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
			if (!%victimPlayer.isMounted())
			{
				%victimPlayer.setTransform(%player.getTransform());
				%victimPlayer.setVelocity("0 0 0");
			}
			else
			{
				%mount = %victimPlayer.getObjectMount();
				if (%mount.getClassName() $= "Player" || %mount.getClassName() $= "AIPlayer" || %mount.getClassName() $= "WheeledVehicle" || %mount.getClassName() $= "FlyingVehicle" || %mount.getClassName() $= "HoverVehicle")
				{
					%mount.setTransform(%player.getTransform());
					%mount.setVelocity("0 0 0");
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
			if (!%player.isMounted())
			{
				%player.setTransform(%victimPlayer.getTransform());
				%player.setVelocity("0 0 0");
			}
			else
			{
				%mount = %player.getObjectMount();
				if (%mount.getClassName() $= "Player" || %mount.getClassName() $= "AIPlayer" || %mount.getClassName() $= "WheeledVehicle" || %mount.getClassName() $= "FlyingVehicle" || %mount.getClassName() $= "HoverVehicle")
				{
					%mount.setTransform(%victimPlayer.getTransform());
					%mount.setVelocity("0 0 0");
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
	%searchMasks = $TypeMasks::TerrainObjectType | $TypeMasks::InteriorObjectType;
	%scanTarg = containerRayCast(%start, %end, %searchMasks, %player);
	if (%scanTarg)
	{
		%scanObj = getWord(%scanTarg, 0);
		%scanPos = getWords(%scanTarg, 1, 3);
		%player.setVelocity("0 0 0");
		%player.setTransform(%scanPos);
	}
}

function serverCmdTimeScale(%client, %val)
{
	if (!%client.isAdmin)
	{
		return;
	}
	if (%val < 0.5 || %val > 2)
	{
		return;
	}
	$timeScale = %val;
	for (%clientIndex = 0; %clientIndex < ClientGroup.getCount(); %clientIndex++)
	{
		%cl = ClientGroup.getObject(%clientIndex);
		commandToClient(%cl, 'TimeScale', %val);
	}
}

function serverCmdDoUpdates(%client)
{
	if (!%client.isAdmin)
	{
		return;
	}
	doUpdates();
}

function doUpdates(%text)
{
	%clientIndex = 0;
	for (%clientIndex = 0; %clientIndex < ClientGroup.getCount(); %clientIndex++)
	{
		%cl = ClientGroup.getObject(%clientIndex);
		schedule(10, 0, commandToClient, %cl, 'DoUpdates');
	}
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
	if (isObject(%client.Player))
	{
		if (%client.Player.getDamagePercent() < 1)
		{
			if (isObject(%client.light))
			{
				%client.light.delete();
				ServerPlay3D(lightOffSound, %client.Player.getPosition());
				%client.Player.playAudio(0, lightOff);
			}
			else
			{
				%client.light = new fxLight()
				{
					dataBlock = PlayerLight;
				};
				MissionCleanup.add(%client.light);
				%client.light.setTransform(%client.Player.getTransform());
				%client.light.attachToObject(%client.Player);
				%client.light.Player = %client.Player;
				ServerPlay3D(lightOnSound, %client.Player.getPosition());
				%client.Player.playAudio(0, LightOn);
			}
		}
	}
}

function serverCmdChatTest(%client, %num)
{
	if (!%client.isAdmin)
	{
		return;
	}
	for (%i = 0; %i < %num; %i++)
	{
		MessageAll('', "\c2" @ %i);
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

function serverCmdToggleCamera(%client)
{
	return;
	if (%client.isAdmin)
	{
		%control = %client.getControlObject();
		if (%control == %client.Player)
		{
			%control = %client.Camera;
			%control.mode = toggleCameraFly;
		}
		else
		{
			%control = %client.Player;
			%control.mode = observerFly;
		}
		%client.setControlObject(%control);
	}
}

function serverCmdDropPlayerAtCamera(%client)
{
	if (%client.isAdmin)
	{
		%client.Camera.unmountImage(0);
		if (isObject(%client.Player))
		{
			if (%client.Player.getDamagePercent() < 1)
			{
				if (!%client.Player.isMounted())
				{
					%client.Player.setTransform(%client.Camera.getTransform());
					%client.Player.setVelocity("0 0 0");
					%client.setControlObject(%client.Player);
				}
				else
				{
					%mount = %client.Player.getObjectMount();
					if (%mount.getClassName() $= "Player" || %mount.getClassName() $= "AIPlayer" || %mount.getClassName() $= "WheeledVehicle" || %mount.getClassName() $= "FlyingVehicle" || %mount.getClassName() $= "HoverVehicle")
					{
						%mount.setTransform(%client.Camera.getTransform());
						%mount.setVelocity("0 0 0");
						%client.setControlObject(%client.Player);
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
}

function serverCmdDropCameraAtPlayer(%client)
{
	if (%client.isAdmin)
	{
		if (isObject(%client.Player))
		{
			%client.Camera.setTransform(%client.Player.getEyeTransform());
			%client.Camera.setVelocity("0 0 0");
		}
		%client.setControlObject(%client.Camera);
		%client.Camera.mountImage(cameraImage, 0);
		%client.Camera.setMode("Observer");
	}
}

function serverCmdSuicide(%client)
{
	if (isObject(%client.Player))
	{
		%client.Player.kill($DamageType::Suicide);
	}
}

function serverCmdSelectObject(%client, %mouseVec, %cameraPoint)
{
	return;
	%selectRange = 200;
	%mouseScaled = VectorScale(%mouseVec, %selectRange);
	%rangeEnd = VectorAdd(%cameraPoint, %mouseScaled);
	%searchMasks = $TypeMasks::PlayerObjectType | $TypeMasks::CorpseObjectType | $TypeMasks::ItemObjectType | $TypeMasks::TriggerObjectType;
	%player = %client.Player;
	if ($firstPerson)
	{
		%scanTarg = containerRayCast(%cameraPoint, %rangeEnd, %searchMasks, %player);
	}
	else
	{
		%scanTarg = containerRayCast(%cameraPoint, %rangeEnd, %searchMasks);
	}
	if (%scanTarg)
	{
		%targetObject = firstWord(%scanTarg);
		%client.setSelectedObj(%targetObject);
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

function ServerCmdClearBricks(%client, %__unused)
{
	if (%server::Lan)
	{
		return;
	}
	if (getSimTime() - %client.lastClearBricksTime < 5000)
	{
		return;
	}
	%client.lastClearBricksTime = getSimTime();
	%brickGroup = %client.brickGroup;
	if (!isObject(%brickGroup))
	{
		return;
	}
	MessageAll('MsgClearBricks', '\c3%1\c2 cleared \c3%2\c2\'s bricks', %client.name, %brickGroup.name);
	%brickGroup.deleteAll();
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
	if (%player.inventory[%slot])
	{
		%item = %player.inventory[%slot].getId();
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
			%muzzlepoint = VectorAdd(%player.getPosition(), "0 0 1.5");
			%muzzlevector = %player.getEyeVector();
			%muzzlepoint = VectorAdd(%muzzlepoint, %muzzlevector);
			%playerRot = rotFromTransform(%player.getTransform());
			%thrownItem = new Item()
			{
				dataBlock = %item;
				initialVelocity = VectorScale(%muzzlevector, 15);
				initialPosition = "80.0366 -215.868 183.615";
			};
			MissionCleanup.add(%thrownItem);
			%thrownItem.setTransform(%muzzlepoint @ " " @ %playerRot);
			%thrownItem.setVelocity(VectorScale(%muzzlevector, 20));
			%thrownItem.schedulePop();
			%thrownItem.miniGame = %client.miniGame;
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

function ServerCmddropInventory(%client, %position)
{
	return;
	%player = %client.Player;
	%item = %player.inventory[%position];
	if (%item && %player)
	{
		%muzzlepoint = VectorAdd(%player.getPosition(), "0 0 1.5");
		%muzzlevector = %player.getEyeVector();
		%muzzlepoint = VectorAdd(%muzzlepoint, %muzzlevector);
		%playerRot = rotFromTransform(%player.getTransform());
		%thrownItem = new Item()
		{
			dataBlock = %item;
			initialVelocity = VectorScale(%muzzlevector, 15);
			initialPosition = "80.0366 -215.868 183.615";
			count = %player.getinventory(%item);
		};
		MissionCleanup.add(%thrownItem);
		%thrownItem.setTransform(%muzzlepoint @ " " @ %playerRot);
		%thrownItem.setVelocity(VectorScale(%muzzlevector, 20));
		if (%item.persistant == 0)
		{
			%thrownItem.schedule($ItemTime, delete);
		}
		%thrownItem.setCollisionTimeout(%player);
		if (%player.isEquiped[%position] == 1 || %player.currWeaponSlot == %position)
		{
			%image = %item.image;
			%mountedImage = %player.getMountedImage(%image.mountPoint);
			%player.unmountImage(%image.mountPoint);
			if (%player.currWeaponSlot == %position)
			{
				%player.currWeaponSlot = -1;
				%player.unmountImage($RightHandSlot);
			}
			else
			{
				%player.isEquiped[%position] = 0;
			}
		}
		%player.inventory[%position] = 0;
		if (%item.className $= "Weapon")
		{
			%player.weaponCount--;
		}
		messageClient(%client, 'MsgDropItem', "", %position);
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
	if (isObject(%obj))
	{
		%action = getField(%line, 1);
		%client.Player.playThread(3, undo);
		if (%action $= "PLANT")
		{
			%obj.undoTrustCheck();
			return;
			%upCount = %obj.getNumUpBricks();
			for (%i = 0; %i < %upCount; %i++)
			{
				%checkBrick = %obj.getUpBrick(%i);
				%checkBrickGroup = %checkBrick.getGroup();
				if (%checkBrickGroup != %client.brickGroup)
				{
					if (%checkBrickGroup.Trust[%client.bl_id] < $TrustLevel::UndoBrick)
					{
						%plantBrick.delete();
						commandToClient(%client, 'CenterPrint', %checkBrickGroup.name @ " does not trust you enough to do that.", 1);
						return;
					}
				}
			}
			%downCount = %obj.getNumDownBricks();
			for (%i = 0; %i < %downCount; %i++)
			{
				%checkBrick = %obj.getDownBrick(%i);
				%checkBrickGroup = %checkBrick.getGroup();
				if (%checkBrickGroup != %client.brickGroup)
				{
					if (%checkBrickGroup.Trust[%client.bl_id] < $TrustLevel::UndoBrick)
					{
						%plantBrick.delete();
						commandToClient(%client, 'CenterPrint', %checkBrickGroup.name @ " does not trust you enough to do that.", 1);
						return;
					}
				}
			}
			%obj.killBrick();
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
	if (!isUnlocked())
	{
		if (getDemoBrickCount() <= 0)
		{
			Canvas.pushDialog(demoBrickLimitGui);
			return;
		}
	}
	%mg = %client.miniGame;
	if (isObject(%mg))
	{
		if (!%mg.EnableBuilding)
		{
			return;
		}
	}
	if ($Server::BrickCount >= $Pref::Server::BrickLimit)
	{
		messageClient(%client, 'MsgPlantError_Limit');
		return;
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
				return;
			}
		}
	}
	%player = %client.Player;
	%tempBrick = %player.tempBrick;
	if (!isObject(%player) || !isObject(%tempBrick))
	{
		return;
	}
	if (%tempBrick)
	{
		%tempBrickTrans = %tempBrick.getTransform();
		%tempBrickPos = getWords(%tempBrickTrans, 0, 2);
		if (VectorDist(%tempBrickPos, %client.Player.getPosition()) > 28)
		{
			messageClient(%client, 'MsgPlantError_TooFar');
			return;
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
		%plantErrorCode = %plantBrick.plant();
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
					%plantBrick.stackBL_ID = %client.bl_id;
				}
				if (%plantBrick.stackBL_ID <= 0)
				{
					%plant.stackBL_ID = %client.bl_id;
				}
			}
			%plantBrick.client = %client;
			if (!$Server::LAN)
			{
				%plantBrick.PlantedTrustCheck();
			}
			else
			{
				%plantBrick.setTrusted(1);
			}
			%player.playThread(3, plant);
			%client.undoStack.push(%plantBrick TAB "PLANT");
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
	}
	return;
	if (%tempBrick)
	{
		%tempBrickTrans = %tempBrick.getTransform();
		%tempBrickPos = getWords(%tempBrickTrans, 0, 2);
		if (VectorDist(%tempBrickPos, %client.Player.getPosition()) > 28)
		{
			messageClient(%client, 'MsgError', '\c3You are too far away.');
			return;
		}
		%quatZ = getWord(%tempBrickTrans, 5);
		%quatAng = getWord(%tempBrickTrans, 6);
		if (%quatZ == -1)
		{
			%quatAng += $pi;
		}
		%angleTest = mFloor(%quatAng / $piOver2);
		%solid = %tempBrick.getDataBlock().solid;
		%newBrickSizeX = %solid.x;
		%newBrickSizeY = %solid.y;
		%newBrickSizeZ = %solid.z;
		if (%angleTest == 1 || %angleTest == 3)
		{
			%tempX = %newBrickSizeX;
			%newBrickSizeX = %newBrickSizeY;
			%newBrickSizeY = %tempX;
		}
		%newBrick = new fxBrick()
		{
			xSize = %newBrickSizeX;
			ySize = %newBrickSizeY;
			zSize = %newBrickSizeZ;
		};
		MissionCleanup.add(%newBrick);
		%newBrick.upSize = 0;
		%newBrick.downSize = 0;
		%newBrick.up[0] = -1;
		%newBrick.down[0] = -1;
		%newBrick.dead = 0;
		%tempBrickX = getWord(%tempBrickTrans, 0);
		%tempBrickY = getWord(%tempBrickTrans, 1);
		%tempBrickZ = getWord(%tempBrickTrans, 2);
		%startX = %tempBrickX;
		%startY = %tempBrickY;
		%startZ = %tempBrickZ;
		%startZ += %solid.z * 0.2;
		%loopEndX = 1;
		%loopEndY = 1;
		%xStep = 1;
		%yStep = 1;
		if (%angleTest == 0)
		{
			%startX += 0.25;
			%startY += 0.25;
			%loopEndX = %solid.x;
			%loopEndY = %solid.y;
			%xStep = 1;
			%yStep = 1;
		}
		else if (%angleTest == 1)
		{
			%startX += 0.25;
			%startY -= 0.25;
			%loopEndX = %solid.y;
			%loopEndY = %solid.x * -1;
			%xStep = 1;
			%yStep = -1;
		}
		else if (%angleTest == 2)
		{
			%startX -= 0.25;
			%startY -= 0.25;
			%loopEndX = %solid.x * -1;
			%loopEndY = %solid.y * -1;
			%xStep = -1;
			%yStep = -1;
		}
		else if (%angleTest == 3)
		{
			%startX -= 0.25;
			%startY += 0.25;
			%loopEndX = %solid.y * -1;
			%loopEndY = %solid.x;
			%xStep = -1;
			%yStep = 1;
		}
		else
		{
			error("Error in ServerCmdPlantBrick(): %angleTest > 3 or < 0");
			return;
		}
		%tempBrickTrans = %tempBrick.getTransform();
		%tempBrickXpos = getWord(%tempBrickTrans, 0);
		%tempBrickYpos = getWord(%tempBrickTrans, 1);
		%tempBrickZpos = getWord(%tempBrickTrans, 2);
		%tempBrickRot = getWords(%tempBrickTrans, 3, 6);
		%tempBrickAngle = getWord(%tempBrickTrans, 6);
		%vectorDir = getWord(%tempBrickTrans, 5);
		if (%vectorDir == -1)
		{
			%tempBrickAngle += $pi;
		}
		%tempBrickAngle /= $piOver2;
		%tempBrickAngle = mFloor(%tempBrickAngle + 0.1);
		if (%tempBrickAngle > 4)
		{
			%tempBrickAngle -= 4;
		}
		if (%tempBrickAngle <= 0)
		{
			%tempBrickAngle += 4;
		}
		%tempBrickStartX = %tempBrickXpos;
		%tempBrickStartY = %tempBrickYpos;
		%tempBrickEndX = %tempBrickXpos;
		%tempBrickEndY = %tempBrickYpos;
		%tempBrickData = %tempBrick.getDataBlock().solid;
		if (%tempBrickAngle == 1)
		{
			%tempBrickEndX += %tempBrickData.y * 0.5;
			%tempBrickStartY -= %tempBrickData.x * 0.5;
		}
		else if (%tempBrickAngle == 2)
		{
			%tempBrickStartX -= %tempBrickData.x * 0.5;
			%tempBrickStartY -= %tempBrickData.y * 0.5;
		}
		else if (%tempBrickAngle == 3)
		{
			%tempBrickStartX -= %tempBrickData.y * 0.5;
			%tempBrickEndY += %tempBrickData.x * 0.5;
		}
		else if (%tempBrickAngle == 4)
		{
			%tempBrickEndX += %tempBrickData.x * 0.5;
			%tempBrickEndY += %tempBrickData.y * 0.5;
		}
		%tempBrickStartZ = %tempBrickZpos;
		%tempBrickEndZ = %tempBrickZpos + %solid.z * 0.2;
		%xOffset = (%loopEndX * 0.5) / 2;
		%yOffset = (%loopEndY * 0.5) / 2;
		%zoffSet = (%solid.z * 0.2) / 2;
		%radius = %solid.x * 0.5;
		if (%solid.y * 0.5 > %radius)
		{
			%radius = %solid.y * 0.5;
		}
		if (%solid.z * 0.2 > %radius)
		{
			%radius = %solid.z * 0.2;
		}
		%radius /= 2;
		%radius += 0.5;
		%brickCenter = %tempBrickX + %xOffset @ " " @ %tempBrickY + %yOffset @ " " @ %tempBrickZ + %zoffSet;
		%mask = $TypeMasks::StaticShapeObjectType;
		if (!containerBoxEmpty(%mask, %brickCenter, %newBrickSizeX * 0.25 - 0.1, %newBrickSizeY * 0.25 - 0.1, %newBrickSizeZ * 0.1 - 0.1))
		{
			messageClient(%client, 'MsgError', '\c3There isn\'t enough room there.');
			%newBrick.delete();
			return;
		}
		%mask = $TypeMasks::StaticShapeObjectType;
		%attachment = 0;
		%searchBoxSize = %newBrickSizeX * 0.5 + 0.1;
		%searchBoxSize = %searchBoxSize @ " " @ %newBrickSizeY * 0.5 + 0.1;
		%searchBoxSize = %searchBoxSize @ " " @ %newBrickSizeZ * 0.2 + 0.1;
		initContainerBoxSearch(%brickCenter, %searchBoxSize, %mask);
		while ((%checkObj = containerSearchNext()) != 0)
		{
			%xOverLap = 0;
			%yOverLap = 0;
			%zOverLap = 0;
			%zTouch = 0;
			if (%checkObj.getClassName() $= "fxBrick")
			{
				%checkPos = %checkObj.getTransform();
				%checkXpos = getWord(%checkPos, 0);
				%checkYpos = getWord(%checkPos, 1);
				%checkZpos = getWord(%checkPos, 2);
				%checkXsize = (%checkObj.xSize * 0.5) / 2;
				%checkYsize = (%checkObj.ySize * 0.5) / 2;
				%checkZsize = (%checkObj.zSize * 0.2) / 2;
				%checkStartX = %checkXpos - %checkXsize;
				%checkStartY = %checkYpos - %checkYsize;
				%checkStartZ = %checkZpos - %checkZsize;
				%checkEndX = %checkXpos + %checkXsize;
				%checkEndY = %checkYpos + %checkYsize;
				%checkEndZ = %checkZpos + %checkZsize;
				%checkStartX = round(%checkStartX);
				%checkStartY = round(%checkStartY);
				%checkStartZ = round(%checkStartZ);
				%checkEndX = round(%checkEndX);
				%checkEndY = round(%checkEndY);
				%checkEndZ = round(%checkEndZ);
				%tempBrickStartX = round(%tempBrickStartX);
				%tempBrickStartY = round(%tempBrickStartY);
				%tempBrickStartZ = round(%tempBrickStartZ);
				%tempBrickEndX = round(%tempBrickEndX);
				%tempBrickEndY = round(%tempBrickEndY);
				%tempBrickEndZ = round(%tempBrickEndZ);
				if (%tempBrickStartX >= %checkStartX)
				{
					if (%checkEndX > %tempBrickStartX)
					{
						%xOverLap = 1;
					}
				}
				else if (%checkStartX >= %tempBrickStartX)
				{
					if (%tempBrickEndX > %checkStartX)
					{
						%xOverLap = 1;
					}
				}
				if (%tempBrickStartY >= %checkStartY)
				{
					if (%checkEndY > %tempBrickStartY)
					{
						%yOverLap = 1;
					}
				}
				else if (%checkStartY >= %tempBrickStartY)
				{
					if (%tempBrickEndY > %checkStartY)
					{
						%yOverLap = 1;
					}
				}
				if (%xOverLap && %yOverLap)
				{
					if (%tempBrickStartZ >= %checkStartZ)
					{
						if (%tempBrickStartZ < %checkEndZ)
						{
							%zOverLap = 1;
						}
					}
					else if (%checkStartZ >= %tempBrickStartZ)
					{
						if (%checkStartZ < %tempBrickEndZ)
						{
							%zOverLap = 1;
						}
					}
					if (%zOverLap)
					{
						messageClient(%client, 'MsgError', '\c3You can\'t put a brick inside another brick.');
						%newBrick.delete();
						return;
						continue;
					}
					if (%tempBrickStartZ $= %checkEndZ)
					{
						%attachedOnTop = 0;
						%zTouch = 1;
					}
					if (%tempBrickEndZ $= %checkStartZ)
					{
						%attachedOnTop = 1;
						%zTouch = 1;
					}
				}
			}
			else if (%checkObj.getDataBlock().className $= "Brick" || %checkObj.getDataBlock().className $= "Baseplate")
			{
				%checkData = %checkObj.getDataBlock();
				%checkTrans = %checkObj.getTransform();
				%checkXpos = getWord(%checkTrans, 0);
				%checkYpos = getWord(%checkTrans, 1);
				%checkZpos = getWord(%checkTrans, 2) - %checkData.z * 0.2 * 0.5;
				%comp1 = round(%tempBrickZ + %solid.z * 0.2);
				%comp2 = round(%checkZpos);
				if (%comp1 == %comp2)
				{
					%attachedOnTop = 1;
					%zTouch = 1;
				}
				%comp1 = round(%tempBrickZ);
				%comp2 = round(%checkZpos + %checkData.z * 0.2);
				if (%comp1 == %comp2)
				{
					%attachedOnTop = 0;
					%zTouch = 1;
				}
				%tempBottom = round(%tempBrickZ);
				%tempTop = round(%tempBrickZ + %solid.z * 0.2);
				%checkBottom = round(%checkZpos);
				%checkTop = round(%checkZpos + %checkData.z * 0.2);
				if (%tempBottom >= %checkBottom)
				{
					if (%checkTop > %tempBottom)
					{
						%zOverLap = 1;
					}
				}
				if (%checkBottom >= %tempBottom)
				{
					if (%tempTop > %checkBottom)
					{
						%zOverLap = 1;
					}
				}
				%checkAngle = getWord(%checkTrans, 6);
				%vectorDir = getWord(%checkTrans, 5);
				if (%vectorDir == -1)
				{
					%checkAngle += $pi;
				}
				%checkAngle /= $piOver2;
				%checkAngle = mFloor(%checkAngle + 0.1);
				if (%checkAngle > 4)
				{
					%checkAngle -= 4;
				}
				if (%checkAngle <= 0)
				{
					%checkAngle += 4;
				}
				%checkStartX = %checkXpos;
				%checkStartY = %checkYpos;
				%checkEndX = %checkXpos;
				%checkEndY = %checkYpos;
				%checkXsize = 0;
				%checkYsize = 0;
				if (%checkAngle == 1)
				{
					%checkXsize = %checkData.y * 0.5;
					%checkYsize = %checkData.x * 0.5;
				}
				else if (%checkAngle == 2)
				{
					%checkXsize = %checkData.x * 0.5;
					%checkYsize = %checkData.y * 0.5;
				}
				else if (%checkAngle == 3)
				{
					%checkXsize = %checkData.y * 0.5;
					%checkYsize = %checkData.x * 0.5;
				}
				else if (%checkAngle == 4)
				{
					%checkXsize = %checkData.x * 0.5;
					%checkYsize = %checkData.y * 0.5;
				}
				%checkStartX = %checkXpos - %checkXsize / 2;
				%checkStartY = %checkYpos - %checkYsize / 2;
				%checkEndX = %checkXpos + %checkXsize / 2;
				%checkEndY = %checkYpos + %checkYsize / 2;
				%checkStartX = round(%checkStartX);
				%checkStartY = round(%checkStartY);
				%checkEndX = round(%checkEndX);
				%checkEndY = round(%checkEndY);
				%tempBrickStartX = round(%tempBrickStartX);
				%tempBrickStartY = round(%tempBrickStartY);
				%tempBrickEndX = round(%tempBrickEndX);
				%tempBrickEndY = round(%tempBrickEndY);
				if (%tempBrickStartX >= %checkStartX)
				{
					if (%checkEndX > %tempBrickStartX)
					{
						%xOverLap = 1;
					}
				}
				if (%checkStartX >= %tempBrickStartX)
				{
					if (%tempBrickEndX > %checkStartX)
					{
						%xOverLap = 1;
					}
				}
				if (%tempBrickStartY >= %checkStartY)
				{
					if (%checkEndY > %tempBrickStartY)
					{
						%yOverLap = 1;
					}
				}
				if (%checkStartY >= %tempBrickStartY)
				{
					if (%tempBrickEndY > %checkStartY)
					{
						%yOverLap = 1;
					}
				}
				if (%xOverLap && %yOverLap && %zOverLap)
				{
					messageClient(%client, 'MsgError', '\c3You can\'t put a brick inside another brick.');
					%newBrick.delete();
					return;
				}
			}
			if (%checkObj.dead != 1)
			{
				if (%zTouch && %xOverLap && %yOverLap)
				{
					%attachment = 1;
					if (%attachedOnTop == 1)
					{
						%newBrick.up[%newBrick.upSize] = %checkObj;
						%newBrick.upSize++;
						continue;
					}
					%newBrick.down[%newBrick.downSize] = %checkObj;
					%newBrick.downSize++;
				}
			}
		}
		if (%attachment == 0)
		{
			messageClient(%client, 'MsgError', '\c3You can\'t put a brick in mid air.');
			%newBrick.delete();
			return;
		}
		%newBrick.setTransform(%brickCenter @ " 0 0 1 0");
		if ($Pref::Server::MaxBricksPerSecond > 0)
		{
			%client.bpsCount++;
		}
		if ($Pref::Server::RandomBrickColor == 1)
		{
			%randColor = getRandom(5);
			if (%randColor == 0)
			{
				%newBrick.setColor(0);
			}
			else if (%randColor == 1)
			{
				%newBrick.setColor(1);
			}
			else if (%randColor == 2)
			{
				%newBrick.setColor(3);
			}
			else if (%randColor == 3)
			{
				%newBrick.setColor(4);
			}
			else if (%randColor == 4)
			{
				%newBrick.setColor(5);
			}
			else if (%randColor == 5)
			{
				%newBrick.setColor(7);
			}
		}
		else
		{
			%newBrick.setColor(%client.currentColor);
		}
		%client.incScore(1);
		%newBrick.client = %client;
		for (%i = 0; %i < %newBrick.upSize; %i++)
		{
			%attachedBrick = %newBrick.up[%i];
			if (!%attachedBrick.isBasePlate)
			{
				%attachedBrick.down[%attachedBrick.downSize] = %newBrick;
				%attachedBrick.downSize++;
			}
		}
		for (%i = 0; %i < %newBrick.downSize; %i++)
		{
			%attachedBrick = %newBrick.down[%i];
			if (!%attachedBrick.isBasePlate)
			{
				%attachedBrick.up[%attachedBrick.upSize] = %newBrick;
				%attachedBrick.upSize++;
			}
		}
	}
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
		%player.inventory[%i] = "";
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
		if (%data.getClassName() $= "fxDTSBrickData")
		{
			%player.inventory[%position] = %data;
			messageClient(%client, 'MsgSetInvData', "", %position, %data);
		}
		else
		{
			messageClient(%client, '', 'Nice try.  Brick DataBlocks only please.');
			return;
		}
	}
	else
	{
		%player.inventory[%position] = "";
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
	%vehicle = %player.getObjectMount();
	if (!isObject(%vehicle))
	{
		return;
	}
	%vehicleData = %vehicle.getDataBlock();
	if (%vehicleData.numMountPoints <= 1)
	{
		return;
	}
	%currSeat = 0;
	for (%i = 0; %i < %vehicleData.numMountPoints; %i++)
	{
		if (%player == %vehicle.getMountNodeObject(%i))
		{
			%currSeat = %i;
			break;
		}
	}
	for (%i = 1; %i < %vehicleData.numMountPoints; %i++)
	{
		%testSeat = %currSeat + %i;
		%testSeat = %testSeat % %vehicleData.numMountPoints;
		if (%vehicle.getMountNodeObject(%testSeat) <= 0)
		{
			%vehicle.mountObject(%player, %testSeat);
			return;
		}
	}
}

function serverCmdPrevSeat(%client)
{
	%player = %client.Player;
	%vehicle = %player.getObjectMount();
	if (!isObject(%vehicle))
	{
		return;
	}
	if (!isObject(%vehicle))
	{
		return;
	}
	%vehicleData = %vehicle.getDataBlock();
	if (%vehicleData.numMountPoints <= 1)
	{
		return;
	}
	%currSeat = 0;
	for (%i = 0; %i < %vehicleData.numMountPoints; %i++)
	{
		if (%player == %vehicle.getMountNodeObject(%i))
		{
			%currSeat = %i;
			break;
		}
	}
	for (%i = 1; %i < %vehicleData.numMountPoints; %i++)
	{
		%testSeat = %currSeat - %i;
		if (%testSeat < 0)
		{
			%testSeat += %vehicleData.numMountPoints;
		}
		%testSeat = %testSeat % %vehicleData.numMountPoints;
		if (%vehicle.getMountNodeObject(%testSeat) <= 0)
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
	$iconBrick.setTransform("0 10 -1005 0 0 1 -1.57");
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
	referenceDistance = 20;
	maxDistance = 100;
	type = $SimAudioType;
};
datablock AudioDescription(AudioClose3d)
{
	volume = 1;
	isLooping = 0;
	is3D = 1;
	referenceDistance = 10;
	maxDistance = 60;
	type = $SimAudioType;
};
datablock AudioDescription(AudioClosest3d)
{
	volume = 1;
	isLooping = 0;
	is3D = 1;
	referenceDistance = 5;
	maxDistance = 30;
	type = $SimAudioType;
};
datablock AudioDescription(AudioDefaultLooping3d)
{
	volume = 1;
	isLooping = 1;
	is3D = 1;
	referenceDistance = 20;
	maxDistance = 100;
	type = $SimAudioType;
};
datablock AudioDescription(AudioCloseLooping3d)
{
	volume = 1;
	isLooping = 1;
	is3D = 1;
	referenceDistance = 10;
	maxDistance = 50;
	type = $SimAudioType;
};
datablock AudioDescription(AudioClosestLooping3d)
{
	volume = 1;
	isLooping = 1;
	is3D = 1;
	referenceDistance = 5;
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

datablock TriggerData(brickBinTrigger)
{
	tickPeriodMS = 100;
};
function brickBinTrigger::onEnterTrigger(%this, %trigger, %obj)
{
	echo("entering bin trigger");
	Parent::onEnterTrigger(%this, %trigger, %obj);
	if (%obj.getClassName() $= "Player")
	{
		%client = %obj.client;
		if (isObject(%client))
		{
			commandToClient(%client, 'BSD_Open');
		}
		%obj.canBuy = 1;
	}
}

function brickBinTrigger::onLeaveTrigger(%this, %trigger, %obj)
{
	Parent::onLeaveTrigger(%this, %trigger, %obj);
	%obj.canBuy = 0;
}

function brickBinTrigger::onTickTrigger(%this, %trigger)
{
	Parent::onTickTrigger(%this, %trigger);
}

function serverCmdUse(%client, %data)
{
	%client.getControlObject().use(%data);
}

function ShapeBase::use(%this, %data)
{
	if (%this.getinventory(%data) > 0)
	{
		return %data.onUse(%this);
	}
	return 0;
}

function ShapeBase::throw(%this, %data, %amount)
{
	if (%this.getinventory(%data) > 0)
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

function ShapeBase::getinventory(%this, %data)
{
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

function ShapeBase::setDamageDt(%this, %damageAmount, %damageType)
{
	if (%obj.getState() !$= "Dead")
	{
		%this.Damage(0, "0 0 0", %damageAmount, %damageType);
		%obj.damageSchedule = %obj.schedule(50, "setDamageDt", %damageAmount, %damageType);
	}
	else
	{
		%obj.damageSchedule = "";
	}
}

function ShapeBase::clearDamageDt(%this)
{
	if (%obj.damageSchedule !$= "")
	{
		cancel(%obj.damageSchedule);
		%obj.damageSchedule = "";
	}
}

function ShapeBaseData::Damage(%this, %obj, %position, %__unused, %amount, %damageType)
{
}

$Item::RespawnTime = 4 * 1000;
$Item::PopTime = 10 * 1000;
function Item::fadeOut(%obj)
{
	%obj.startFade(0, 0, 1);
	%obj.setNodeColor("ALL", "1 1 1 0.25");
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
	%obj.schedule($Game::Item::PopTime - 1000, "setNodeColor", "ALL", "1 1 1 0.5");
	%obj.schedule($Game::Item::PopTime - 800, "setNodeColor", "ALL", "1 1 1 0.4");
	%obj.schedule($Game::Item::PopTime - 600, "setNodeColor", "ALL", "1 1 1 0.3");
	%obj.schedule($Game::Item::PopTime - 400, "setNodeColor", "ALL", "1 1 1 0.2");
	%obj.schedule($Game::Item::PopTime - 200, "setNodeColor", "ALL", "1 1 1 0.1");
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
	if (!$Server::LAN)
	{
		if (%obj.isStatic())
		{
			%ownerBrickGroup = %obj.spawnBrick.getGroup();
			%ownerClient = %ownerBrickGroup.client;
			if (isObject(%mg))
			{
				if (!isObject(%ownerClient))
				{
					commandToClient(%client, 'CenterPrint', "This item is not part of the mini-game.", 1);
					return;
				}
				if (%ownerClient.miniGame != %mg)
				{
					commandToClient(%client, 'CenterPrint', "This item is not part of the mini-game.", 1);
					return;
				}
				if (%mg.useAllPlayersBricks)
				{
					if (%mg.playersUseOwnBricks)
					{
						if (%client.brickGroup != %ownerBrickGroup)
						{
							commandToClient(%client, 'CenterPrint', "You do not own this item.", 1);
							return;
						}
					}
				}
				else if (%ownerBrickGroup.client != %mg.owner)
				{
					commandToClient(%client, 'CenterPrint', "This item is not part of the mini-game.", 1);
					return;
				}
			}
			else
			{
				if (isObject(%ownerClient))
				{
					if (isObject(%ownerClient.miniGame))
					{
						commandToClient(%client, 'CenterPrint', "This item is part of a mini-game.", 1);
						return;
					}
				}
				if (%ownerBrickGroup != %client.brickGroup)
				{
					%trustLevel = %ownerBrickGroup.Trust[%client.bl_id];
					if (%trustLevel < $TrustLevel::ItemPickup)
					{
						commandToClient(%client, 'CenterPrint', %ownerBrickGroup.name @ " does not trust you enough to do that.", 1);
						return;
					}
				}
			}
		}
		else if (isObject(%obj.miniGame))
		{
			if (%obj.miniGame != %mg)
			{
				if (isObject(%mg))
				{
					commandToClient(%client, 'CenterPrint', "This item is not part of the mini-game.", 1);
				}
				else
				{
					commandToClient(%client, 'CenterPrint', "This item is part of a mini-game.", 1);
				}
				return;
			}
		}
		else if (isObject(%mg))
		{
			commandToClient(%client, 'CenterPrint', "This item is not part of the mini-game.", 1);
			return;
		}
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
	return;
	%mountPoint = %this.image.mountPoint;
	%mountSlot = %mountPoint;
	%mountedImage = %player.getMountedImage(%mountPoint);
	if (%this.equipment == 1)
	{
		echo("EQUIPMENT NO LONGER USED");
		return;
		if (%player.isEquiped[%invPosition] == 1)
		{
			messageClient(%client, 'MsgDeEquipInv', '', %invPosition);
			%player.isEquiped[%invPosition] = 0;
			%player.unmountImage(%mountSlot);
		}
		else
		{
			for (%i = 0; %i < %playerData.maxItems; %i++)
			{
				if (%player.isEquiped[%i] == 1)
				{
					%checkMountPoint = %player.inventory[%i].image.mountPoint;
					if (%mountPoint == %checkMountPoint)
					{
						messageClient(%client, 'MsgDeEquipInv', '', %i);
						%player.isEquiped[%i] = 0;
						break;
					}
				}
			}
			messageClient(%client, 'MsgEquipInv', '', %invPosition);
			%player.isEquiped[%invPosition] = 1;
			%player.mountImage(%this.image, %mountPoint, 1, %color);
		}
	}
	else if (%player.currWeaponSlot == %invPosition)
	{
		%player.unmountImage(%mountSlot);
		messageClient(%client, 'MsgHilightInv', '', -1);
		%player.currWeaponSlot = -1;
	}
	else
	{
		messageClient(%client, 'MsgHilightInv', '', %invPosition);
		%player.mountImage(%this.image, %mountPoint, 1, %color);
		%player.currWeaponSlot = %invPosition;
	}
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

function StaticShape::Explode(%obj)
{
	%obj.setDamageState(destroyed);
	%obj.schedule(100, delete);
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
	dynamicType = $TypeMasks::StationObjectType;
	isShielded = 1;
	energyPerDamagePoint = 110;
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
	dynamicType = $TypeMasks::StationObjectType;
	isShielded = 1;
	energyPerDamagePoint = 110;
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
	dynamicType = $TypeMasks::StationObjectType;
	isShielded = 1;
	energyPerDamagePoint = 110;
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
	if (!$Server::LAN)
	{
		if (%obj.isStatic())
		{
			%ownerBrickGroup = %obj.spawnBrick.getGroup();
			%ownerClient = %ownerBrickGroup.client;
			if (isObject(%mg))
			{
				if (!isObject(%ownerBrickGroup.client))
				{
					commandToClient(%client, 'CenterPrint', "This item is not part of the mini-game.", 1);
					return;
				}
				if (%ownerBrickGroup.client.miniGame != %mg)
				{
					commandToClient(%client, 'CenterPrint', "This item is not part of the mini-game.", 1);
					return;
				}
				if (%mg.useAllPlayersBricks)
				{
					if (%mg.playersUseOwnBricks)
					{
						if (%client.brickGroup != %ownerBrickGroup)
						{
							commandToClient(%client, 'CenterPrint', "You do not own this item.", 1);
							return;
						}
					}
				}
				else if (%ownerBrickGroup.client != %mg.owner)
				{
					commandToClient(%client, 'CenterPrint', "This item is not part of the mini-game.", 1);
					return;
				}
			}
			else
			{
				if (isObject(%ownerClient))
				{
					if (isObject(%ownerClient.miniGame))
					{
						commandToClient(%client, 'CenterPrint', "This item is part of a mini-game.", 1);
						return;
					}
				}
				if (%ownerBrickGroup != %client.brickGroup)
				{
					%trustLevel = %ownerBrickGroup.Trust[%client.bl_id];
					if (%trustLevel < $TrustLevel::ItemPickup)
					{
						commandToClient(%client, 'CenterPrint', %ownerBrickGroup.name @ " does not trust you enough to do that.", 1);
						return;
					}
				}
			}
		}
		else if (isObject(%obj.miniGame))
		{
			if (%obj.miniGame != %mg)
			{
				if (isObject(%mg))
				{
					commandToClient(%client, 'CenterPrint', "This item is not part of the mini-game.", 1);
				}
				else
				{
					commandToClient(%client, 'CenterPrint', "This item is part of a mini-game.", 1);
				}
				return;
			}
		}
		else if (isObject(%mg))
		{
			commandToClient(%client, 'CenterPrint', "This item is not part of the mini-game.", 1);
			return;
		}
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
	if (%this.minShotTime > 0)
	{
		if (getSimTime() - %obj.lastFireTime < %this.minShotTime)
		{
			return;
		}
		%obj.lastFireTime = getSimTime();
	}
	%projectile = %this.Projectile;
	if (%this.melee)
	{
		%initPos = %obj.getEyeTransform();
		%muzzlevector = %obj.getMuzzleVector(%slot);
	}
	else
	{
		%initPos = %obj.getMuzzlePoint(%slot);
		%muzzlevector = %obj.getMuzzleVector(%slot);
	}
	%objectVelocity = %obj.getVelocity();
	%muzzleVelocity = VectorAdd(VectorScale(%muzzlevector, %projectile.muzzleVelocity), VectorScale(%objectVelocity, %projectile.velInheritFactor));
	%p = new (%this.projectileType)()
	{
		dataBlock = %projectile;
		initialVelocity = %muzzleVelocity;
		initialPosition = %initPos;
		sourceObject = %obj;
		sourceSlot = %slot;
		client = %obj.client;
	};
	MissionCleanup.add(%p);
	return %p;
}

function WeaponImage::onMount(%this, %obj, %slot)
{
	if (%this.ammo)
	{
		if (%obj.getinventory(%this.ammo))
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
	%mask = $TypeMasks::PlayerObjectType | $TypeMasks::VehicleObjectType;
	if (%col.getType() & %mask)
	{
		if (!isObject(%client))
		{
			return;
		}
		if (miniGameCanDamage(%client, %col) == 1)
		{
			%this.Damage(%obj, %col, %fade, %pos, %normal);
			%this.impactImpulse(%obj, %col, %velocity);
		}
	}
	else if (%col.getType() & $TypeMasks::FxBrickObjectType)
	{
		if (%this.brickExplosionImpact == 1)
		{
			if (%col.getDataBlock().indestructable == 1)
			{
				return;
			}
			if (!%col.isPlanted())
			{
				return;
			}
			if (miniGameCanDamage(%client, %col) != 1)
			{
				return;
			}
			%volume = %col.getDataBlock().getVolume();
			if (%volume <= %this.brickExplosionMaxVolume)
			{
				transmitBrickExplosion(%pos, %this.brickExplosionForce, 0.02, %client.miniGame.brickRespawnTime, %col);
			}
			else if (%volume <= %this.brickExplosionMaxVolumeFloating)
			{
				if (%col.hasFakePathToGround() == 0)
				{
					transmitBrickExplosion(%pos, %this.brickExplosionForce, 0.02, %client.miniGame.brickRespawnTime, %col);
				}
			}
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
	%mg = %client.miniGame;
	if (!isObject(%mg))
	{
		return;
	}
	%explosion = %this.Explosion;
	if (%this.brickExplosionRadius > 0)
	{
		%count = 0;
		%brickList = "";
		%mask = $TypeMasks::FxBrickObjectType;
		%radius = %this.brickExplosionRadius;
		$CurrBrickKiller = %client;
		initContainerRadiusSearch(%pos, %radius, %mask);
		while ((%searchObj = containerSearchNext()) != 0)
		{
			if (%searchObj.isDead())
			{
			}
			else if (%searchObj.isFakeDead())
			{
			}
			else if (!%searchObj.isPlanted())
			{
			}
			else if (%searchObj.getDataBlock().indestructable == 1)
			{
			}
			else
			{
				%brickVolume = %searchObj.getDataBlock().getVolume();
				if (getWord(getColorIDTable(%searchObj.getColorID()), 3) < 0.95)
				{
					%brickVolume *= 0.25;
				}
				if (%brickVolume > %this.brickExplosionMaxVolumeFloating)
				{
				}
				else
				{
					if (%brickVolume > %this.brickExplosionMaxVolume)
					{
						if (%searchObj.hasFakePathToGround())
						{
							continue;
						}
					}
					if (miniGameCanDamage(%client, %searchObj) != 1)
					{
					}
					else
					{
						%brickList = %brickList @ getWord(%searchObj, 0) @ " ";
						%count++;
						if (%count > 100)
						{
							transmitBrickExplosion(%pos, %this.brickExplosionForce, %this.brickExplosionRadius, %client.miniGame.brickRespawnTime, %brickList);
							%brickList = "";
							%count = 0;
						}
					}
				}
			}
		}
		if (%count > 0)
		{
			transmitBrickExplosion(%pos, %this.brickExplosionForce, %this.brickExplosionRadius, %client.miniGame.brickRespawnTime, %brickList);
		}
	}
	if ((%explosion.damageRadius <= 0 || %explosion.radiusDamage <= 0) && ((%explosion.impulse && %explosion.impulseVertical) <= 0 || %explosion.impulseRadius <= 0))
	{
		return;
	}
	if (%explosion.damageRadius > %explosion.impulseRadius)
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
		%searchObj = getWord(%searchObj, 0);
		if (%searchObj.getType() & $TypeMasks::PlayerObjectType)
		{
			%searchPos = %searchObj.getHackPosition();
		}
		else
		{
			%searchPos = %searchObj.getWorldBoxCenter();
		}
		%dist = VectorDist(%searchPos, %pos);
		%damageDistFactor = 1 - (%dist / %explosion.damageRadius) * (%dist / %explosion.damageRadius);
		%impulseDistFactor = 1 - (%dist / %explosion.impulseRadius) * (%dist / %explosion.impulseRadius);
		if (miniGameCanDamage(%client, %searchObj) == 1)
		{
			%this.radiusDamage(%obj, %searchObj, %damageDistFactor, %pos, %explosion.radiusDamage);
			%this.radiusImpulse(%obj, %searchObj, %impulseDistFactor, %pos, %explosion.impulseForce, %explosion.impulseVertical);
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
	if (%col.getType() & $TypeMasks::PlayerObjectType)
	{
		%col.Damage(%obj, %pos, %this.directDamage, %damageType);
	}
	else
	{
		%col.Damage(%obj, %pos, %this.directDamage, %damageType);
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
			if (%col.getType() & $TypeMasks::PlayerObjectType)
			{
				%col.burn(%this.Explosion.playerBurnTime * %distanceFactor);
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
		%colPos = %col.getPosition();
		%colPos = VectorAdd(%colPos, "0 0 1");
		%impulseVec = VectorSub(%colPos, %pos);
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
	%col.applyImpulse(%colPos, VectorScale(%vector, %this.impactImpulse));
	%col.applyImpulse(%colPos, "0 0" SPC %this.verticalImpulse);
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

exec("base/data/shapes/player/player.cs");
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
datablock AudioProfile(FootFallSound)
{
	fileName = "base/data/sound/pain.wav";
	description = AudioClose3d;
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
	uiName = "Player Foam";
};
datablock ParticleData(PlayerFoamDropletsParticle)
{
	dragCoefficient = 0;
	gravityCoefficient = 1;
	inheritedVelFactor = 0.5;
	constantAcceleration = -0;
	lifetimeMS = 1600;
	lifetimeVarianceMS = 0;
	textureName = "base/data/particles/bubble";
	colors[0] = "0.7 0.8 1.0 1.0";
	colors[1] = "0.7 0.8 1.0 0.85";
	colors[2] = "0.7 0.8 1.0 0.0";
	sizes[0] = 0.15;
	sizes[1] = 0.15;
	sizes[2] = 0.15;
	times[0] = 0;
	times[1] = 0.5;
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
};
datablock SplashData(PlayerSplash)
{
	numSegments = 15;
	ejectionFreq = 15;
	ejectionAngle = 40;
	ringLifetime = 0.5;
	lifetimeMS = 300;
	velocity = 4;
	startRadius = 0;
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
	className = armor;
	shapeFile = "base/data/shapes/player/m.dts";
	cameraMaxDist = 8;
	cameraTilt = 0.261;
	cameraVerticalOffset = 0.75;
	computeCRC = 0;
	canObserve = 1;
	cmdCategory = "Clients";
	cameraDefaultFov = 90;
	cameraMinFov = 5;
	cameraMaxFov = 120;
	aiAvoidThis = 1;
	minLookAngle = -1.5708;
	maxLookAngle = 1.5708;
	maxFreelookAngle = 3;
	mass = 90;
	drag = 0.1;
	maxDrag = 0.2;
	density = 0.7;
	maxDamage = 100;
	maxEnergy = 100;
	repairRate = 0.33;
	energyPerDamagePoint = 75;
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
	maxForwardProneSpeed = 4;
	maxBackwardProneSpeed = 3;
	maxSideProneSpeed = 3;
	maxForwardWalkSpeed = 7;
	maxBackwardWalkSpeed = 6;
	maxSideWalkSpeed = 6;
	maxUnderwaterForwardSpeed = 8.4;
	maxUnderwaterBackwardSpeed = 7.8;
	maxUnderwaterSideSpeed = 7.8;
	jumpForce = 12 * 90;
	jumpEnergyDrain = 0;
	minJumpEnergy = 0;
	jumpDelay = 0;
	minJetEnergy = 0;
	jetEnergyDrain = 0;
	canJet = 1;
	recoverDelay = 0;
	recoverRunForceScale = 1.2;
	minImpactSpeed = 30;
	speedDamageScale = 3.8;
	boundingBox = VectorScale("1.25 1.25 2.65", 4);
	crouchBoundingBox = VectorScale("1.25 1.25 1.00", 4);
	proneBoundingBox = VectorScale("1 2.3 1", 4);
	pickupRadius = 0.625;
	boxNormalHeadPercentage = 0.83;
	boxNormalTorsoPercentage = 0.49;
	boxHeadLeftPercentage = 0;
	boxHeadRightPercentage = 1;
	boxHeadBackPercentage = 0;
	boxHeadFrontPercentage = 1;
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
	FootSoftSound = FootFallSound;
	FootHardSound = FootFallSound;
	FootMetalSound = FootFallSound;
	FootSnowSound = FootFallSound;
	FootShallowSound = FootFallSound;
	FootWadingSound = FootFallSound;
	FootUnderwaterSound = FootFallSound;
	impactWaterEasy = Splash1Sound;
	impactWaterMedium = Splash1Sound;
	impactWaterHard = Splash1Sound;
	groundImpactMinSpeed = 10;
	groundImpactShakeFreq = "4.0 4.0 4.0";
	groundImpactShakeAmp = "1.0 1.0 1.0";
	groundImpactShakeDuration = 0.8;
	groundImpactShakeFalloff = 10;
	exitingWater = exitWaterSound;
	observeParameters = "0.5 4.5 4.5";
	maxItems = 10;
	maxWeapons = 5;
	maxTools = 5;
	uiName = "Standard Player";
	canRide = 1;
	showEnergyBar = 0;
	brickImage = brickImage;
};
function armor::onAdd(%this, %obj)
{
	%obj.mountVehicle = 1;
	%obj.setRechargeRate(%this.rechargeRate);
	%obj.setRepairRate(0);
}

function armor::onRemove(%this, %obj)
{
	if (%obj.client.Player == %obj)
	{
		%obj.client.Player = 0;
	}
}

function armor::onNewDataBlock(%this, %obj)
{
}

function armor::onMount(%this, %obj, %vehicle, %node)
{
	if (%node == 0)
	{
		%obj.setControlObject(%vehicle);
		%vehicle.lastDrivingClient = %obj.client;
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

function armor::onUnMount(%this, %obj, %vehicle, %node)
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

function armor::doDismount(%this, %obj, %forced)
{
	if (!%obj.isMounted())
	{
		return;
	}
	if (!%obj.canDismount)
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
	%vec[0] = " 0  0  2.2";
	%vec[1] = " 0  0  3";
	%vec[2] = " 0  0 -3";
	%vec[3] = " 3  0  0";
	%vec[4] = "-3  0  0";
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

function armor::OnCollision(%this, %obj, %col, %vec, %speed)
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
		if (%col.getDataBlock().canRide && %this.rideAble)
		{
			if (!isObject(%col.client))
			{
				return;
			}
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
			if (%obj.getMountedObject(0))
			{
				return;
			}
			%canUse = 0;
			if (getTrustLevel(%col, %obj) >= $TrustLevel::RideVehicle)
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
			%obj.mountObject(%col, 2);
			%col.setTransform("0 0 0 0 0 1 0");
			if (isObject(%obj.spawnBrick))
			{
				%col.setControlObject(%obj);
			}
			%col.setActionThread(root, 0);
		}
	}
}

function armor::onImpact(%this, %obj, %collidedObject, %vec, %vecLen)
{
	if (%collidedObject.getClassName() $= "StaticShape")
	{
		if (%collidedObject.getDataBlock().className $= "Glass")
		{
			if (!%collidedObject.indestructable)
			{
				%collidedObject.Explode();
				return;
			}
		}
	}
	%doDamage = 0;
	if (isObject(%obj.client))
	{
		%mg = %obj.client.miniGame;
	}
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
		%time = ((%vecLen - 10) / 40) * 6 + 2;
		%time = %time * 1000;
		if (%time > 8000)
		{
			%time = 8000;
		}
		if (%time < 2000)
		{
			%time = 2000;
		}
	}
}

function armor::Damage(%this, %obj, %sourceObject, %position, %damage, %damageType)
{
	if (%obj.getState() $= "Dead")
	{
		return;
	}
	%obj.applyDamage(%damage);
	%location = "Body";
	if (%obj.isCrouched())
	{
		if ($Damage::Direct[%damageType])
		{
			%damage = %damage * 2;
		}
		else
		{
			%damage = %damage * 0.75;
		}
	}
	%client = %obj.client;
	%sourceClient = %sourceObject ? %sourceObject.client : "0";
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
			%mg = getMiniGameFromObject(%obj);
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
	if (%obj.painLevel >= 40)
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

function armor::onDamage(%this, %obj, %delta)
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

function armor::onDisabled(%this, %obj, %state)
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
	if (%obj.tempBrick > 0)
	{
		%obj.tempBrick.delete();
		%obj.tempBrick = 0;
	}
	%client = %obj.client;
	%client.deathTime = getSimTime();
	%obj.schedule($CorpseTimeoutValue, RemoveBody);
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
	MissionCleanup.add(%p);
	%p.setTransform(%obj.getTransform());
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

function armor::onLeaveMissionArea(%this, %obj)
{
	%obj.client.onLeaveMissionArea();
}

function armor::onEnterMissionArea(%this, %obj)
{
	%obj.client.onEnterMissionArea();
}

function armor::onEnterLiquid(%this, %obj, %coverage, %type)
{
	if (%type == 0)
	{
	}
	else if (%type == 1)
	{
	}
	else if (%type == 2)
	{
	}
	else if (%type == 3)
	{
	}
	else if (%type == 4)
	{
		%obj.setDamageDt(%this, $DamageLava, "Lava");
	}
	else if (%type == 5)
	{
		%obj.setDamageDt(%this, $DamageHotLava, "Lava");
	}
	else if (%type == 6)
	{
		%obj.setDamageDt(%this, $DamageCrustyLava, "Lava");
	}
	else if (%type == 7)
	{
	}
}

function armor::onLeaveLiquid(%this, %obj, %type)
{
	%obj.clearDamageDt();
}

function armor::onTrigger(%this, %obj, %triggerNum, %val)
{
	if (%triggerNum == 0)
	{
		if (%val)
		{
			if (%obj.getMountedImage(0) <= 0)
			{
				%obj.ActivateStuff();
			}
		}
	}
}

function Player::ActivateStuff(%obj)
{
	%client = %obj.client;
	%start = %obj.getEyePoint();
	%vec = %obj.getEyeVector();
	%end = VectorAdd(%start, VectorScale(%vec, 10));
	%mask = $TypeMasks::FxBrickObjectType | $TypeMasks::VehicleObjectType;
	%searchObj = containerRayCast(%start, %end, %mask, %obj);
	if (%searchObj)
	{
		%pos = getWords(%searchObj, 1, 3);
		%searchObj.activate(%obj, %client, %pos, %vec);
	}
}

function Player::kill(%this, %damageType)
{
	%this.Damage(0, %this.getPosition(), 10000, %damageType);
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

function Player::playDeathCry(%this)
{
	%client = %this.client;
	%this.playAudio(0, DeathCrySound);
}

function Player::playPain(%this)
{
	%client = %this.client;
	%this.playAudio(0, PainCrySound);
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

function Player::SetTempColor(%player, %color, %time)
{
	if (isEventPending(%player.tempColorSchedule))
	{
		cancel(%player.tempColorSchedule);
	}
	%player.setNodeColor("ALL", %color);
	%player.setDecalName("AAA-None");
	if (isObject(%player.client))
	{
		%player.tempColorSchedule = %player.client.schedule(%time, ApplyBodyColors);
	}
	else if (isObject(%player.spawnBrick))
	{
		%player.tempColorSchedule = %player.spawnBrick.schedule(%time, colorVehicle);
	}
}

function Player::burn(%player, %time)
{
	if (isEventPending(%player.burnSchedule))
	{
		cancel(%player.burnSchedule);
	}
	%player.SetTempColor("0 0 0 1", %time);
	%player.setDecalName("AAA-None");
	%player.mountImage(PlayerBurnImage, 3);
	%player.burnSchedule = %player.schedule(%time, clearBurn);
}

function Player::clearBurn(%player)
{
	if (%player.getDamagePercent() >= 1)
	{
		return;
	}
	%player.unmountImage(3);
	%pos = %player.getHackPosition();
	%pos = VectorAdd(%pos, "0 0 -1");
	%p = new Projectile()
	{
		dataBlock = PlayerSootProjectile;
		initialPosition = %pos;
	};
	MissionCleanup.add(%p);
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
};
function armor::onStuck(%this, %obj)
{
}

function armor::onReachDestination(%this, %obj)
{
}

function armor::onTargetEnterLOS(%this, %obj)
{
}

function armor::onTargetExitLOS(%this, %obj)
{
}

function AIPlayer::spawnPlayer()
{
	%player = new AIPlayer()
	{
		dataBlock = PlayerStandardArmor;
		AIPlayer = 1;
	};
	MissionCleanup.add(%player);
	%player.setMoveSpeed(1);
	%player.setTransform(pickSpawnPoint());
	%player.setEnergyLevel(60);
	%player.setShapeName(%this.name);
	return %player;
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
	camShakeAmp = "1.0 1.0 1.0";
	camShakeDuration = 0.5;
	camShakeRadius = 10;
	lightStartRadius = 2;
	lightEndRadius = 1;
	lightStartColor = "0.6 0.6 0.0";
	lightEndColor = "0 0 0";
};
AddDamageType("HammerDirect", '<bitmap:add-ons/ci/hammer> %1', '%2 <bitmap:add-ons/ci/hammer> %1', 0, 1);
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
	lifetime = 100;
	fadeDelay = 70;
	bounceElasticity = 0;
	bounceFriction = 0;
	isBallistic = 0;
	gravityMod = 0;
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

function hammerProjectile::OnCollision(%this, %obj, %col, %fade, %pos, %normal, %velocity)
{
	Parent::OnCollision(%this, %obj, %col, %fade, %pos, %normal);
	%player = %obj.client.Player;
	if (!isObject(%player))
	{
		return;
	}
	if (%col.getClassName() $= "fxDTSBrick")
	{
		if (%col.isExposed())
		{
			if (getTrustLevel(%obj, %col) < $TrustLevel::Hammer)
			{
				commandToClient(%obj.client, 'CenterPrint', %col.getGroup().name @ " does not trust you enough to do that.", 1);
				return;
			}
			$CurrBrickKiller = %obj.client;
			%col.killBrick();
			return;
		}
	}
	else if (%col.getClassName() $= "WheeledVehicle" || %col.getClassName() $= "HoverVehicle" || %col.getClassName() $= "FlyingVehicle")
	{
		%doFlip = 0;
		if (getTrustLevel(%obj, %col) >= $TrustLevel::VehicleTurnover)
		{
			%doFlip = 1;
		}
		if (miniGameCanDamage(%player, %col) == 1)
		{
			%doFlip = 1;
		}
		if (miniGameCanDamage(%player, %col) == 0)
		{
			%doFlip = 0;
		}
		if (%doFlip)
		{
			%impulse = VectorNormalize(%velocity);
			%impulse = VectorAdd(%impulse, "0 0 1");
			%impulse = VectorNormalize(%impulse);
			%force = %col.getDataBlock().mass * 5;
			%impulse = VectorScale(%impulse, %force);
			%col.applyImpulse(%pos, %impulse);
		}
	}
	ServerPlay3D(hammerHitSound, %pos);
	return;
	if (%col.getClassName() $= "fxBrick")
	{
		if (!%col.isBasePlate)
		{
			if (%col.downSize <= 0)
			{
				killBrick(%col);
			}
			else if (%col.upSize <= 0)
			{
				killBrick(%col);
			}
		}
		return;
	}
	if (%col.getClassName() !$= "StaticShape")
	{
		return;
	}
	%colData = %col.getDataBlock();
	%colDataClass = %colData.className;
	if (%colDataClass $= "brick")
	{
		if (%col.downSize == 0)
		{
			killBrick(%col);
		}
		else if (%col.upSize == 0)
		{
			killBrick(%col);
		}
	}
}

function removeFromUpList(%toRemove, %removeFromBrick)
{
	for (%i = 0; %i < %removeFromBrick.upSize; %i++)
	{
		if (%removeFromBrick.up[%i] == %toRemove)
		{
			%removeFromBrick.up[%i] = %removeFromBrick.up[%removeFromBrick.upSize - 1];
			%removeFromBrick.up[%removeFromBrick.upSize - 1] = "";
			%removeFromBrick.upSize--;
			return;
		}
	}
}

function removeFromDownList(%toRemove, %removeFromBrick)
{
	for (%i = 0; %i < %removeFromBrick.downSize; %i++)
	{
		if (%removeFromBrick.down[%i] == %toRemove)
		{
			%removeFromBrick.down[%i] = %removeFromBrick.down[%removeFromBrick.downSize - 1];
			%removeFromBrick.down[%removeFromBrick.downSize - 1] = "";
			%removeFromBrick.downSize--;
			return;
		}
	}
}

function hasPathToGround(%brick, %checkVal)
{
	if (!%checkVal)
	{
		error("Error: hasPathToGround Called without check value!!");
		return 0;
	}
	if (!isObject(%brick))
	{
		error("Error: object ", %brick, " missing from brick tree!");
		return 0;
	}
	if (%brick.dead == 1)
	{
		return 0;
	}
	if (%brick.isBasePlate)
	{
		return 1;
	}
	if (%brick.groundCheckVal == %checkVal)
	{
	}
	if (%brick.checkVal == %checkVal)
	{
		return 0;
	}
	%brick.checkVal = %checkVal;
	for (%i = 0; %i < %brick.downSize; %i++)
	{
		if (hasPathToGround(%brick.down[%i], %checkVal))
		{
			return 1;
		}
	}
	for (%i = 0; %i < %brick.upSize; %i++)
	{
		if (hasPathToGround(%brick.up[%i], %checkVal))
		{
			return 1;
		}
	}
	return 0;
}

function killBrick(%brick)
{
	for (%i = 0; %i < %brick.upSize; %i++)
	{
		%child = %brick.up[%i];
		removeFromDownList(%brick, %child);
	}
	for (%i = 0; %i < %brick.downSize; %i++)
	{
		%child = %brick.down[%i];
		removeFromUpList(%brick, %child);
	}
	%upCheckval = $currCheckVal++;
	%downCheckval = $currCheckVal++;
	%killCheckval = $currCheckVal++;
	for (%i = 0; %i < %brick.upSize; %i++)
	{
		%child = %brick.up[%i];
		if (!hasPathToGround(%child, $currCheckVal++))
		{
			chainKillBrick(%child, %killCheckval, 0);
		}
	}
	for (%i = 0; %i < %brick.downSize; %i++)
	{
		%child = %brick.down[%i];
		if (!hasPathToGround(%child, $currCheckVal++))
		{
			chainKillBrick(%child, %killCheckval, 0);
		}
	}
	%brick.dead = 1;
	%brick.schedule(10, Explode);
	if ($currCheckVal > 50000)
	{
		$currCheckVal = 1;
	}
}

function chainKillBrick(%brick, %checkVal, %iteration)
{
	if (!isObject(%brick))
	{
		error("Error: Chain kill brick ", %brick, " not found!");
		return 0;
	}
	if (%brick.checkVal == %checkVal)
	{
		return 0;
	}
	if (%brick.dead == 1)
	{
		return 0;
	}
	%brick.checkVal = %checkVal;
	%brick.dead = 1;
	if (%iteration <= 1)
	{
		%brick.schedule(%iteration * 10 + 0, Explode);
	}
	else
	{
		%brick.schedule(%iteration * 35 + 50, Explode);
	}
	%iteration++;
	for (%i = 0; %i < %brick.upSize; %i++)
	{
		if (chainKillBrick(%brick.up[%i], %checkVal, %iteration))
		{
			%iteration++;
		}
	}
	for (%i = 0; %i < %brick.downSize; %i++)
	{
		if (chainKillBrick(%brick.down[%i], %checkVal, %iteration))
		{
			%iteration++;
		}
	}
	return 1;
}

function recursionTest(%iteration)
{
	if (%iteration >= 10)
	{
		return;
	}
	echo("this is a stack size test step ", %iteration);
	recursionTest(%iteration++);
}

function killHangBrick(%checkBrick, %sourceBrick, %iteration)
{
	if (%sourceBrick != -1)
	{
		for (%i = 0; %i < %checkBrick.downSize; %i++)
		{
			if (%checkBrick.down[%i] == %sourceBrick)
			{
				%checkBrick.down[%i] = %checkBrick.down[%checkBrick.downSize - 1];
				%checkBrick.downSize--;
				break;
			}
		}
	}
	if (%checkBrick.downSize == 0)
	{
		for (%i = 0; %i < %checkBrick.upSize; %i++)
		{
			killHangBrick(%checkBrick.up[%i], %checkBrick, %iteration++);
		}
		if (%iteration <= 1)
		{
			%brick.schedule(%iteration * 25 + 0, Explode);
		}
		else
		{
			%brick.schedule(%iteration * 25 + 50, Explode);
		}
	}
}

function killTopBrick(%topBrick)
{
	for (%i = 0; %i < %topBrick.downSize; %i++)
	{
		removeFromUpList(%topBrick, %topBrick.down[%i]);
		if (%topBrick.down[%i].wasHung == 1)
		{
			hangCheck(%topBrick.down[%i]);
		}
	}
	%topBrick.schedule(10, Explode);
}

function hangCheck(%brick)
{
	if (%brick.downSize == 0)
	{
		killHangBrick(%brick, -1, 0);
		return;
	}
	for (%i = 0; %i < %brick.downSize; %i++)
	{
		if (%brick.down[%i].wasHung != 1)
		{
			return;
		}
	}
	for (%i = 0; %i < %brick.downSize; %i++)
	{
		hangCheck(%brick.down[%i]);
	}
}

datablock AudioProfile(wrenchHitSound)
{
	fileName = "~/data/sound/wrenchHit.wav";
	description = AudioClosest3d;
	preload = 1;
};
datablock AudioProfile(wrenchMissSound)
{
	fileName = "~/data/sound/wrenchMiss.wav";
	description = AudioClosest3d;
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
	lifetime = 200;
	fadeDelay = 70;
	bounceElasticity = 0;
	bounceFriction = 0;
	isBallistic = 0;
	gravityMod = 0;
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

function wrenchProjectile::OnCollision(%this, %obj, %col, %fade, %pos, %normal)
{
	%client = %obj.client;
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
			commandToClient(%client, 'CenterPrint', "\c5 Building is currently disabled.", 1);
			ServerPlay3D(wrenchMissSound, %pos);
			return;
		}
	}
	if (%col.getClassName() $= "fxDTSBrick")
	{
		if (getTrustLevel(%obj, %col) < $TrustLevel::Wrench)
		{
			commandToClient(%obj.client, 'CenterPrint', %col.getGroup().name @ " does not trust you enough to do that.", 1);
			return;
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
							ServerPlay3D(wrenchMissSound, %pos);
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
						ServerPlay3D(wrenchMissSound, %pos);
						return;
					}
				}
			}
		}
		%client.wrenchBrick = %col;
		if (%col.getDataBlock().specialBrickType $= "Sound")
		{
			commandToClient(%client, 'openWrenchSoundDlg', %col);
			%col.sendWrenchSoundData(%client);
		}
		else if (%col.getDataBlock().specialBrickType $= "VehicleSpawn")
		{
			commandToClient(%client, 'openWrenchVehicleSpawnDlg', %col);
			%col.sendWrenchVehicleSpawnData(%client);
		}
		else
		{
			if ($Server::LAN)
			{
				commandToClient(%client, 'openWrenchDlg', %col);
			}
			else
			{
				commandToClient(%client, 'openWrenchDlg', %col @ " - " @ %col.getGroup().name);
			}
			%col.sendWrenchData(%client);
		}
		ServerPlay3D(wrenchHitSound, %pos);
		return;
	}
	ServerPlay3D(wrenchMissSound, %pos);
	return;
}

function serverCmdVehicleSpawn_Respawn(%client)
{
	%brick = %client.wrenchBrick;
	if (!isObject(%brick))
	{
		messageClient(%client, 'Wrench Error: Brick no longer exists!');
		return;
	}
	%brick.spawnVehicle();
}

function serverCmdSetWrenchData(%client, %data)
{
	%brick = %client.wrenchBrick;
	if (!isObject(%brick))
	{
		messageClient(%client, 'Wrench Error: Brick no longer exists!');
		return;
	}
	%fieldCount = getFieldCount(%data);
	for (%i = 0; %i < %fieldCount; %i++)
	{
		%field = getField(%data, %i);
		%type = getWord(%field, 0);
		if (%type $= "N")
		{
			%name = trim(getSubStr(%field, 2, strlen(%field) - 2));
			%brick.setName(%name);
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
			%brick.setItem(%db);
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
		else
		{
			error("ERROR: clientCmdSetWrenchData() - unknown field type \"" @ %field @ "\"");
		}
	}
}

function fxDTSBrick::setLight(%obj, %data, %client)
{
	if (isObject(%obj.light))
	{
		%obj.light.delete();
	}
	%obj.light = 0;
	if (!isObject(%data))
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
	if (!$Server::LAN)
	{
		%brickGroup = %obj.getGroup();
		if (%brickGroup.numLights >= $Pref::Server::MaxLights_PerPlayer)
		{
			commandToClient(%client, 'CenterPrint', "\c0Players are limited to " @ $Pref::Server::MaxLights_PerPlayer @ " lights each", 2);
			return;
		}
	}
	if ($Server::NumLights >= $Pref::Server::MaxLights_Total)
	{
		commandToClient(%client, 'CenterPrint', "\c0Server is limited to " @ $Pref::Server::MaxLights_Total @ " lights", 2);
		return;
	}
	%light = new fxLight()
	{
		dataBlock = %data;
	};
	MissionCleanup.add(%light);
	if (!$Server::LAN)
	{
		%brickGroup = %obj.getGroup();
		%brickGroup.numLights++;
	}
	$Server::NumLights++;
	%light.brick = %obj;
	%obj.light = %light;
	%light.setTransform(%obj.getTransform());
	%light.attachToBrick(%obj);
}

function fxDTSBrick::setEmitter(%obj, %data, %client)
{
	if (isObject(%obj.emitter))
	{
		%obj.emitter.delete();
	}
	%obj.emitter = 0;
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
	if (!$Server::LAN)
	{
		%brickGroup = %obj.getGroup();
		if (%brickGroup.numEmitters >= $Pref::Server::MaxEmitters_PerPlayer)
		{
			commandToClient(%client, 'CenterPrint', "\c0Players are limited to " @ $Pref::Server::MaxEmitters_PerPlayer @ " particle emitters each", 2);
			return;
		}
	}
	if ($Server::NumEmitters >= $Pref::Server::MaxEmitters_Total)
	{
		commandToClient(%client, 'CenterPrint', "\c0Server is limited to " @ $Pref::Server::MaxEmitters_Total @ " particle emitters", 2);
		return;
	}
	if (%data.emitterNode !$= "")
	{
		%nodeData = %data.emitterNode;
	}
	else
	{
		%nodeData = GenericEmitterNode;
	}
	%emitter = new ParticleEmitterNode()
	{
		dataBlock = %nodeData;
		emitter = %data;
	};
	MissionCleanup.add(%emitter);
	if (!$Server::LAN)
	{
		%brickGroup = %obj.getGroup();
		%brickGroup.numEmitters++;
	}
	$Server::NumEmitters++;
	%emitter.brick = %obj;
	%obj.emitter = %emitter;
	%emitter.setTransform(%obj.getTransform());
}

function fxDTSBrick::setEmitterDirection(%obj, %dir)
{
	if (%dir < 0 || %dir > 5)
	{
		return;
	}
	%obj.emitterDirection = %dir;
	if (isObject(%obj.emitter))
	{
		%pos = getWords(%obj.emitter.getTransform(), 0, 2);
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
		}
		else if (%dir == 3)
		{
			%rot = "0 -1 0 " @ $piOver2;
		}
		else if (%dir == 4)
		{
			%rot = "-1 0 0 " @ $piOver2;
		}
		else if (%dir == 5)
		{
			%rot = "0 1 0 " @ $piOver2;
		}
		else
		{
			%rot = "0 0 1 0";
		}
		%obj.emitter.setTransform(%pos SPC %rot);
	}
}

function fxDTSBrick::setItem(%obj, %data)
{
	if (isObject(%obj.Item))
	{
		%obj.Item.delete();
	}
	%obj.Item = 0;
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
	%item.setTransform(%obj.getTransform());
}

function fxDTSBrick::setItemDirection(%obj, %dir)
{
	if (%dir < 2 || %dir > 5)
	{
		return;
	}
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
	%brickBox = %obj.getWorldBox();
	%brickBoxX = mAbs(getWord(%brickBox, 0) - getWord(%brickBox, 3)) / 2;
	%brickBoxY = mAbs(getWord(%brickBox, 1) - getWord(%brickBox, 4)) / 2;
	%brickBoxZ = mAbs(getWord(%brickBox, 2) - getWord(%brickBox, 5)) / 2;
	%trans = %obj.getTransform();
	%posX = getWord(%trans, 0);
	%posY = getWord(%trans, 1);
	%posZ = getWord(%trans, 2);
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
	if (!%data.description.isLooping)
	{
		return;
	}
	%pos = %obj.getPosition();
	%radius = AudioMusicLooping3d.maxDistance * 2;
	%mask = $TypeMasks::MarkerObjectType;
	initContainerRadiusSearch(%pos, %radius, %mask);
	while ((%searchObj = containerSearchNext()) != 0)
	{
		if (%searchObj.getClassName() $= "AudioEmitter")
		{
			messageClient(%client, 'MsgPlantError_TooLoud');
			return;
		}
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
		error("ERROR:fxDTSBrick::setVehicle() - Brick is not a vehicle spawn");
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
	if (!%data.rideAble)
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
			if (%brickGroup.numPlayerVehicles >= $Pref::Server::MaxPlayerVehicles_PerPlayer)
			{
				if (%client.brickGroup == %brickGroup)
				{
					if ($Pref::Server::MaxPlayerVehicles_PerPlayer == 1)
					{
						commandToClient(%client, 'CenterPrint', "\c0You already have a player-vehicle", 2);
					}
					else
					{
						commandToClient(%client, 'CenterPrint', "\c0You already have " @ $Pref::Server::MaxPlayerVehicles_PerPlayer @ " player-vehicles", 2);
					}
				}
				else if ($Pref::Server::MaxPlayerVehicles_PerPlayer == 1)
				{
					commandToClient(%client, 'CenterPrint', "\c0" @ %brickGroup.name @ " already has a player-vehicle", 2);
				}
				else
				{
					commandToClient(%client, 'CenterPrint', "\c0" @ %brickGroup.name @ " already has " @ $Pref::Server::MaxPlayerVehicles_PerPlayer @ " player-vehicles", 2);
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
			if (%brickGroup.numPhysVehicles >= $Pref::Server::MaxPhysVehicles_PerPlayer)
			{
				if (%client.brickGroup == %brickGroup)
				{
					if ($Pref::Server::MaxPhysVehicles_PerPlayer == 1)
					{
						commandToClient(%client, 'CenterPrint', "\c0You already have a physics-vehicle", 2);
					}
					else
					{
						commandToClient(%client, 'CenterPrint', "\c0You already have " @ $Pref::Server::MaxPhysVehicles_PerPlayer @ " physics-vehicles", 2);
					}
				}
				else if ($Pref::Server::MaxPhysVehicles_PerPlayer == 1)
				{
					commandToClient(%client, 'CenterPrint', "\c0" @ %brickGroup.name @ " already has a physics-vehicle", 2);
				}
				else
				{
					commandToClient(%client, 'CenterPrint', "\c0" @ %brickGroup.name @ " already has " @ $Pref::Server::MaxPhysVehicles_PerPlayer @ " physics-vehicles", 2);
				}
				return;
			}
		}
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
		%emitterDB = %obj.emitter.emitter.getId();
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
	lifetime = 100;
	fadeDelay = 70;
	bounceElasticity = 0;
	bounceFriction = 0;
	isBallistic = 0;
	gravityMod = 0;
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

function wandProjectile::OnCollision(%this, %obj, %col, %fade, %pos, %normal)
{
	%player = %obj.client.Player;
	if (!%player)
	{
		return;
	}
	if (%col.getClassName() $= "Player")
	{
		%client = %obj.client;
		if (miniGameCanDamage(%client, %col) == 1)
		{
		}
		else if (miniGameCanDamage(%client, %col) == 0)
		{
			commandToClient(%obj.client, 'CenterPrint', %col.client.name @ " is in a different minigame.", 1);
			return;
		}
		else if (getTrustLevel(%obj, %col) < $TrustLevel::Wand)
		{
			commandToClient(%obj.client, 'CenterPrint', %col.client.name @ " does not trust you enough to do that.", 1);
			return;
		}
		%col.setVelocity("0 0 15");
	}
	if (%col.getClassName() $= "fxDTSBrick")
	{
		if (getTrustLevel(%obj, %col) < $TrustLevel::Wand)
		{
			commandToClient(%obj.client, 'CenterPrint', %col.getGroup().name @ " does not trust you enough to do that.", 1);
			return;
		}
		$CurrBrickKiller = %obj.client;
		%col.killBrick();
		return;
	}
	if (%col.getClassName() !$= "StaticShape")
	{
		return;
	}
	%colData = %col.getDataBlock();
	%colDataClass = %colData.className;
	if (%colDataClass $= "brick")
	{
		killBrick(%col);
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
	muzzleVelocity = 25;
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
function sprayCan::onUse(%this, %player, %invPosition)
{
	echo("sprayCan::onUse");
	%client = %player.client;
	if (%client)
	{
		%color = %client.color;
	}
	%mountedImage = %player.getMountedImage($RightHandSlot);
	echo("using spraycan");
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
		messageClient(%client, 'MsgHilightInv', '', %invPosition);
		%player.currWeaponSlot = %invPosition;
	}
	return;
	if (%mountedImage.Item $= "sprayCan" && %player.currWeaponSlot == %invPosition)
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
		messageClient(%client, 'MsgHilightInv', '', %invPosition);
		%player.currWeaponSlot = %invPosition;
	}
}

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
			%col.SetTempColor(%rgba, 2000);
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
	%fileName = "base/config/server/colorSet.txt";
	if (!isFile(%fileName))
	{
		error("ERROR: setSprayCanColors() - File \"" @ %fileName @ "\" not found!");
		return;
	}
	%file = new FileObject();
	%file.openForRead(%fileName);
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
			%r = getWord(%line, 0);
			%g = getWord(%line, 1);
			%b = getWord(%line, 2);
			%a = getWord(%line, 3);
			if (mFloor(%r) != mAbs(%r) || mFloor(%g) != mAbs(%g) || mFloor(%b) != mAbs(%b) || mFloor(%a) != mAbs(%a))
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
}

function setSprayCanColorI(%id, %color)
{
	%red = getWord(%color, 0);
	%green = getWord(%color, 1);
	%blue = getWord(%color, 2);
	%alpha = getWord(%color, 3);
	%red /= 255;
	%green /= 255;
	%blue /= 255;
	%alpha /= 255;
	%floatColor = %red @ " " @ %green @ " " @ %blue @ " " @ %alpha;
	setSprayCanColor(%id, %floatColor);
}

function setSprayCanColor(%id, %color)
{
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
	%dbName = "color" @ %id @ "PaintParticle";
	%commandString = "datablock ParticleData(" @ %dbName @ " : bluePaintParticle)" @ "{ colors[0] = \"" @ %rgbColor @ " 1.000\";" @ "  colors[1] = \"" @ %rgbColor @ " 0.000\";" @ " useInvAlpha = " @ %invalpha @ ";" @ "};";
	eval(%commandString);
	%dbName = "color" @ %id @ "PaintEmitter";
	%particleDBName = "color" @ %id @ "PaintParticle";
	%commandString = "datablock ParticleEmitterData(" @ %dbName @ " : bluePaintEmitter)" @ "{ particles = " @ %particleDBName @ ";" @ "};";
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
	%commandString = "datablock ShapeBaseImageData(" @ %dbName @ " : blueSprayCanImage)" @ "{ projectile = " @ %projectileDBName @ ";" @ "  stateEmitter[2] = " @ %stateEmitterDBName @ ";" @ "  doColorShift = true ;" @ "  colorShiftColor = \"" @ %color @ "\";" @ "  shapeFile = \"" @ %shapeFile @ "\";" @ "};";
	eval(%commandString);
}

datablock AudioProfile(AdminWandHitSound)
{
	fileName = "~/data/sound/wandHit.wav";
	description = AudioClosest3d;
	preload = 1;
};
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
	soundProfile = AdminWandHitSound;
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
	lifetime = 100;
	fadeDelay = 70;
	bounceElasticity = 0;
	bounceFriction = 0;
	isBallistic = 0;
	gravityMod = 0;
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

function AdminWandProjectile::OnCollision(%this, %obj, %col, %fade, %pos, %normal, %velocity)
{
	%player = %obj.client.Player;
	if (!%player)
	{
		return;
	}
	if (%col.getClassName() $= "Player")
	{
		%vel = VectorNormalize(%velocity);
		%vel = VectorAdd(%vel, "0 0 1");
		%vel = VectorNormalize(%vel);
		%vel = VectorScale(%vel, 20);
		%col.setVelocity(%vel);
	}
	if (%col.getClassName() $= "fxDTSBrick")
	{
		$CurrBrickKiller = 0;
		%col.killBrick();
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
				%trustLevel = %brickGroup.Trust[%client.bl_id];
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
				%trustLevel = %brickGroup.Trust[%client.bl_id];
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
				%trustLevel = %brickGroup.Trust[%client.bl_id];
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
				%trustLevel = %brickGroup.Trust[%client.bl_id];
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
				%trustLevel = %brickGroup.Trust[%client.bl_id];
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
				%trustLevel = %brickGroup.Trust[%client.bl_id];
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
				%trustLevel = %brickGroup.Trust[%client.bl_id];
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
				%trustLevel = %brickGroup.Trust[%client.bl_id];
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
				%trustLevel = %brickGroup.Trust[%client.bl_id];
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
	%client = %obj.client;
	%player = %client.Player;
	if (!%player)
	{
		return;
	}
	if (%player.currInv > 0)
	{
		%data = %player.inventory[%player.currInv];
	}
	else if (isObject(%player.instantUseData))
	{
		%data = %player.instantUseData;
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
		%player.tempBrick.setDatablock(%data);
		%aspectRatio = %data.printAspectRatio;
		if (%aspectRatio !$= "")
		{
			%player.tempBrick.setPrint($printARStart["Letters"] + 25);
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
			error("ERROR: brickDeployProjectile::onCollision() - client \"" @ %client.name @ "\" has no brick group.");
		}
		%aspectRatio = %data.printAspectRatio;
		if (%aspectRatio !$= "")
		{
			%b.setPrint($printARStart["Letters"] + 25);
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
	doColorShift = 1;
	colorShiftColor = "0.647 0.647 0.647 1.000";
	stateName[0] = "Ready";
	stateTransitionOnTriggerDown[0] = "Fire";
	stateAllowImageChange[0] = 1;
	stateName[1] = "Fire";
	stateScript[1] = "onFire";
	stateFire[1] = 1;
	stateAllowImageChange[1] = 0;
	stateTimeoutValue[1] = 0.25;
	stateTransitionOnTimeout[1] = "StopFire";
	stateEmitter[1] = brickTrailEmitter;
	stateEmitterTime[1] = 0.1;
	stateSequence[1] = "Fire";
	stateName[2] = "StopFire";
	stateTransitionOnTriggerUp[2] = "Ready";
};
function brickImage::onDeploy(%this, %obj, %slot)
{
}

function brickImage::onMount(%this, %obj, %slot)
{
	if (%this.armReady)
	{
		if (%obj.getMountedImage($LeftHandSlot))
		{
			if (%obj.getMountedImage($LeftHandSlot).armReady)
			{
				%obj.playThread(1, armReadyBoth);
			}
			else
			{
				%obj.playThread(1, armReadyRight);
			}
		}
		else
		{
			%obj.playThread(1, armReadyRight);
		}
	}
	if (%this.ammo)
	{
		if (%obj.getinventory(%this.ammo))
		{
			%obj.setImageAmmo(%slot, 1);
		}
	}
	else
	{
		%obj.setImageAmmo(%slot, 1);
	}
}

function brickImage::onUnMount(%this, %obj, %slot)
{
	%obj.playThread(2, root);
	%leftimage = %obj.getMountedImage($LeftHandSlot);
}

datablock ShapeBaseImageData(horseBrickImage : brickImage)
{
	mountPoint = 3;
	offset = "0 -0.1 0";
	rotation = eulerToMatrix("0 -90 0");
};
function horseBrickImage::onMount(%this, %obj, %slot)
{
	brickImage::onMount(%this, %obj, %slot);
}

function horseBrickImage::onUnMount(%this, %obj, %slot)
{
	brickImage::onUnMount(%this, %obj, %slot);
}

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
	printAspectRatio = "1x1r";
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
	printAspectRatio = "1x1r";
	category = "Ramps";
	subCategory = "Prints";
	uiName = "-45° Ramp 2x Print";
	iconName = "base/client/ui/brickIcons/-45 Ramp 2x Print";
};
datablock fxDTSBrickData(brick2x2fPrintData)
{
	brickFile = "~/data/bricks/flats/2x2fPrint.blb";
	orientationFix = 3;
	printAspectRatio = "2x2";
	category = "Plates";
	subCategory = "Prints";
	uiName = "2x2F Print";
	iconName = "base/client/ui/brickIcons/2x2F Print";
};
datablock fxDTSBrickData(brick2x1fPrintData)
{
	brickFile = "~/data/bricks/flats/2x1fPrint.blb";
	orientationFix = 3;
	printAspectRatio = "2x1";
	category = "Plates";
	subCategory = "Prints";
	uiName = "1x2F Print";
	iconName = "base/client/ui/brickIcons/1x2F Print";
};
datablock fxDTSBrickData(brick1x1fPrintData)
{
	brickFile = "~/data/bricks/flats/1x1fPrint.blb";
	orientationFix = 3;
	printAspectRatio = "1x1";
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
function serverCmdTest(%client)
{
	if (!%client.isAdmin)
	{
		return;
	}
	%test = new fxDTSBrick()
	{
		dataBlock = brick2x2x5girderData;
	};
	%test.setTransform(%client.Player.getPosition());
	%client.Player.tempBrick = %test;
}

function serverCmdTest2(%client)
{
	if (!%client.isAdmin)
	{
		return;
	}
	%test = new fxDTSBrick()
	{
		dataBlock = brick2x2Data;
	};
	%test.setTransform(%client.Player.getPosition());
	%client.Player.tempBrick = %test;
}

function serverCmdBrick(%client, %brickName)
{
	if (!%client.isAdmin)
	{
		return;
	}
	%brickData = "brick" @ %brickName @ "Data";
	if (!%brickData.getId())
	{
		return;
	}
	%test = new fxDTSBrick()
	{
		dataBlock = %brickData;
	};
	%test.setTransform(%client.Player.getPosition());
	%client.Player.tempBrick = %test;
}

function fxDTSBrickData::onUse(%this, %player, %InvSlot)
{
	if (!isObject(%player))
	{
		return;
	}
	%player.mountImage(%player.getDataBlock().brickImage, 0);
	%player.currInv = %InvSlot;
	if (%InvSlot = -1)
	{
		%player.instantUseData = %this;
	}
	else
	{
		%player.instantUseData = 0;
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
		%player.tempBrick.setDatablock(%this);
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
			%player.tempBrick.setPrint($printARStart["Letters"] + 25);
		}
		return;
		%b = new fxDTSBrick()
		{
			dataBlock = %this;
		};
		MissionCleanup.add(%b);
		%b.setTransform(%player.tempBrick.getTransform());
		%b.angleID = %player.tempBrick.angleID;
		%b.setColor(%player.tempBrick.colorID);
		%player.tempBrick.delete();
		%player.tempBrick = %b;
	}
	return;
	if (%player.currInv == %InvSlot && %player.getMountedImage(0) == brickImage.getId())
	{
		%player.unmountImage(0);
		%player.currInv = -1;
	}
	else
	{
		%player.mountImage(brickImage, 0);
		%player.currInv = %InvSlot;
	}
}

function fxDTSBrick::Explode(%obj)
{
	%obj.delete();
	return;
	%data = %obj.getDataBlock();
	%pos = %obj.getWorldBoxCenter();
	%exp = new Explosion()
	{
		dataBlock = spearExplosion;
		initialPosition = %pos;
	};
	%obj.setTransform("0 0 -999");
	%obj.schedule(100, delete);
	MissionCleanup.add(%exp);
	echo("obj = ", %obj);
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
}

function fxDTSBrickData::onRemove(%this, %obj)
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
			%obj.schedule(10, delete);
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
			%client = %obj.client;
			if (isObject(%client))
			{
				if (isObject(%client.miniGame))
				{
					%client.incScore(%client.miniGame.Points_PlantBrick);
				}
			}
			%obj.setTrusted(1);
			return;
		}
		%checkBrick = %obj.getUpBrick(%i);
		if (getTrustLevel(%checkBrick, %obj) < $TrustLevel::BuildOn)
		{
			%client = %obj.client;
			%checkBrickGroup = %checkBrick.getGroup();
			commandToClient(%client, 'CenterPrint', %checkBrickGroup.name @ " does not trust you enough to do that.", 1);
			%obj.schedule(10, delete);
			return;
		}
		%idx++;
	}
	%obj.schedule($FXBrick::ChainDelay, ChainTrustCheckUp, %idx);
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
	%rot = getWords(%trans, 3, 6);
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
	MissionCleanup.add(%v);
	%v.spawnBrick = %obj;
	%v.brickGroup = %obj.getGroup();
	%obj.Vehicle = %v;
	%obj.colorVehicle();
	%worldBoxZ = mAbs(getWord(%v.getWorldBox(), 2) - getWord(%v.getWorldBox(), 5));
	%worldBoxZ = mAbs(getWord(%v.getWorldBox(), 2) - getWord(%v.getPosition(), 2));
	%z += %worldBoxZ + 0.1;
	%trans = %x SPC %y SPC %z SPC %rot;
	%v.setTransform(%trans);
	%p = new Projectile()
	{
		dataBlock = spawnProjectile;
		initialVelocity = "0 0 0";
		initialPosition = %trans;
		sourceObject = %v;
		sourceSlot = 0;
		client = %this;
	};
	MissionCleanup.add(%p);
	%p.setTransform(%trans);
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

function fxDTSBrick::activate(%obj)
{
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

function fxDTSBrick::onFakeDeath(%obj)
{
	if (isObject(%obj.light))
	{
		%obj.light.setEnable(0);
	}
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
	if (isObject(%obj.light))
	{
		%obj.light.setEnable(1);
	}
	if (isObject(%obj.emitter))
	{
		%obj.emitter.setEmitterDataBlock(%obj.emitter.oldEmitterDB);
	}
}

function updateDemoBrickCount(%val)
{
	if (isObject(Demo_BrickCount))
	{
		Demo_BrickCount.setText(%val);
	}
}

function Vehicle::onDriverLeave(%obj, %player)
{
	%obj.getDataBlock().onDriverLeave(%obj, %player);
}

function Vehicle::onRemove(%obj)
{
}

function Vehicle::activate(%vehicle, %__unused, %activatingClient, %pos, %vec)
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
	if (!$Server::LAN)
	{
		%ownerBrickGroup = %vehicle.spawnBrick.getGroup();
		%ownerClient = %ownerBrickGroup.client;
		%mg = %client.miniGame;
		if (isObject(%mg))
		{
			if (!isObject(%ownerClient))
			{
				commandToClient(%client, 'CenterPrint', "This vehicle is not part of the mini-game.", 1);
				return;
			}
			if (%ownerClient.miniGame != %mg)
			{
				commandToClient(%client, 'CenterPrint', "This vehicle is not part of the mini-game.", 1);
				return;
			}
			if (%mg.useAllPlayersBricks)
			{
				if (%mg.playersUseOwnBricks)
				{
					if (%client.brickGroup != %ownerBrickGroup)
					{
						commandToClient(%client, 'CenterPrint', "You do not own this vehicle.", 1);
						return;
					}
				}
			}
			else if (%ownerBrickGroup.client != %mg.owner)
			{
				commandToClient(%client, 'CenterPrint', "This vehicle is not part of the mini-game.", 1);
				return;
			}
		}
		else
		{
			if (isObject(%ownerClient))
			{
				if (isObject(%ownerClient.miniGame))
				{
					commandToClient(%client, 'CenterPrint', "This vehicle is part of a mini-game.", 1);
					return;
				}
			}
			if (%ownerBrickGroup != %client.brickGroup)
			{
				%trustLevel = %ownerBrickGroup.Trust[%client.bl_id];
				if (%trustLevel < $TrustLevel::VehicleTurnover)
				{
					commandToClient(%client, 'CenterPrint', %ownerBrickGroup.name @ " does not trust you enough to do that.", 1);
					return;
				}
			}
		}
	}
	%impulse = VectorNormalize(%vec);
	%impulse = VectorAdd(%impulse, "0 0 1");
	%impulse = VectorNormalize(%impulse);
	%force = %vehicle.getDataBlock().mass * 5;
	%impulse = VectorScale(%impulse, %force);
	%vehicle.applyImpulse(%pos, %impulse);
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
	%obj.setWheelSteering(0, 1);
	%obj.setWheelSteering(1, 1);
	%obj.setWheelPowered(2, 1);
	%obj.setWheelPowered(3, 1);
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
	%canUse = 0;
	if (getTrustLevel(%col, %obj) >= $TrustLevel::RideVehicle)
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
	if (%col.getDataBlock().canRide && %this.rideAble)
	{
		if (getSimTime() - %col.lastMountTime > $Game::MinMountTime)
		{
			%colZpos = getWord(%col.getPosition(), 2);
			%objZpos = getWord(%obj.getPosition(), 2);
			if (%colZpos > %objZpos + 0.2)
			{
				if (%canUse)
				{
					for (%i = 0; %i < %this.numMountPoints; %i++)
					{
						if (!%obj.getMountNodeObject(%i))
						{
							%obj.mountObject(%col, %i);
							if (%i == 0)
							{
								%col.setControlObject(%obj);
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
					return;
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
	if (%runOver && %canDamage)
	{
		if (%col.getType() & $TypeMasks::PlayerObjectType)
		{
			%vehicleSpeed = VectorLen(%obj.getVelocity());
			%minSpeed = %this.minRunOverSpeed;
			if (%minSpeed $= "")
			{
				%minSpeed = $Game::DefaultMinRunOverSpeed;
			}
			if (%vehicleSpeed > %minSpeed)
			{
				%damageScale = %this.runOverDamageScale;
				if (%damageScale $= "")
				{
					%damageScale = 1;
				}
				%damageType = %this.damageType;
				if (%damageType $= "")
				{
					%damageType = $DamageType::Vehicle;
				}
				%damageAmt = %vehicleSpeed * %damageScale;
				%col.Damage(%obj, %pos, %damageAmt, %damageType);
			}
			%pushScale = %this.runOverPushScale;
			if (%pushScale $= "")
			{
				%pushScale = 1;
			}
			%pushVec = %obj.getVelocity();
			%pushVec = VectorScale(%pushVec, %pushScale);
			%col.setVelocity(%pushVec);
		}
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
		%p = new Projectile()
		{
			dataBlock = %this.initialExplosionProjectile;
			initialPosition = %pos;
			sourceClient = %client;
		};
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
			%obj.spawnBrick.spawnVehicle(%mg.vehicleReSpawnTime);
		}
		else
		{
			%obj.spawnBrick.spawnVehicle(0);
		}
		if (isObject(%this.burnDataBlock))
		{
			%obj.setDatablock(%this.burnDataBlock);
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
		%p = new Projectile()
		{
			dataBlock = %this.initialExplosionProjectile;
			initialPosition = %pos;
			sourceClient = %client;
		};
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
			%obj.setDatablock(%this.burnDataBlock);
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
	%p = new Projectile()
	{
		dataBlock = %data.finalExplosionProjectile;
		initialPosition = %pos;
		sourceClient = %client;
	};
	%p.upVector = %obj.getUpVector();
	%p.client = %client;
	MissionCleanup.add(%p);
	%obj.delete();
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
	dropSize = 0.2;
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
		%obj.schedule(%obj.delay, advanceTime);
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
	%obj.schedule(%obj.delay, advanceTime);
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
	%fileName = "base/data/missions/" @ %mapName @ ".mis";
	endGame();
	loadMission(%fileName);
}

function announce(%text)
{
	MessageAll('', "\c3" @ %text);
}

function kickID(%clientId)
{
	serverCmdKick(0, %clientId);
}

function banID(%clientId)
{
	serverCmdBan(0, %clientId);
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
		%nameFillLen = 24 - strlen(%cl.name);
		%zoneName = %cl.keyID;
		echo(%cl, makePadString(" ", %idFillLen), %cl.name, makePadString(" ", %nameFillLen), %zoneName, " ", %cl.getAddress() TAB %cl.bl_id);
	}
	echo("------------------------------------------------------------");
	return;
	%clientIndex = 0;
	for (%clientIndex = 0; %clientIndex < ClientGroup.getCount(); %clientIndex++)
	{
		%cl = ClientGroup.getObject(%clientIndex);
		echo(%cl, " ", %cl.name, " ", %cl.getAddress());
	}
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
	%count = ClientGroup.getCount();
	for (%clientIndex = 0; %clientIndex < %count; %clientIndex++)
	{
		%cl = ClientGroup.getObject(%clientIndex);
		%cl.transmitDataBlocks(1);
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
	if (isObject(%obj.brick))
	{
		if (!$Server::LAN)
		{
			%brickGroup = %obj.brick.getGroup();
			%brickGroup.numLights--;
		}
		$Server::NumLights--;
	}
	else if (%obj.brick !$= "" && %obj.brick != 0)
	{
		error("ERROR: Light (ID:" @ %obj @ ") has brick pointer to " @ %obj.brick @ " but brick does not exist.");
	}
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
	colors[1] = "0.2 0.2 0.2 1.0";
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
function ParticleEmitterNode::onRemove(%obj)
{
	if (isObject(%obj.brick))
	{
		if (!$Server::LAN)
		{
			%brickGroup = %obj.brick.getGroup();
			%brickGroup.numEmitters--;
		}
		$Server::NumEmitters--;
	}
	else
	{
		error("ERROR: EmitterNode (ID:" @ %obj @ ") has brick pointer to " @ %obj.brick @ " but brick does not exist.");
	}
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
		%p.setTransform(%finalPos);
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
	referenceDistance = 10;
	maxDistance = 30;
	type = $SimAudioType;
};
function createMusicDatablocks()
{
	updateMusicList();
	%dir = "base/data/sound/music/*.ogg";
	%fileCount = getFileCount(%dir);
	%fileName = findFirstFile(%dir);
	for (%i = 0; %i < %fileCount; %i++)
	{
		%name = fileBase(%fileName);
		%varName = %name;
		%varName = strreplace(%name, " ", "_");
		%varName = strreplace(%name, "-", "_");
		if ($Music__[%varName] $= "1")
		{
			%dbName = "musicData_" @ strreplace(%name, " ", "_");
			%command = "datablock AudioProfile(" @ %dbName @ ") {" @ "filename = \"" @ %fileName @ "\";" @ "description = AudioMusicLooping3d;" @ "preload = true;" @ "uiName = \"" @ %name @ "\";" @ "};";
			eval(%command);
		}
		%fileName = findNextFile(%dir);
	}
}

function updateMusicList()
{
	deleteVariables("$Music*");
	exec("Base/data/sound/music/musicList.cs");
	%dir = "base/data/sound/music/*.ogg";
	%fileCount = getFileCount(%dir);
	%fileName = findFirstFile(%dir);
	for (%i = 0; %i < %fileCount; %i++)
	{
		%name = fileBase(%fileName);
		%name = strreplace(%name, " ", "_");
		%name = strreplace(%name, " ", "_");
		if (mFloor($Music__[%name]) <= 0)
		{
			$Music__[%name] = -1;
		}
		else
		{
			$Music__[%name] = 1;
		}
		%fileName = findNextFile(%dir);
	}
	export("$Music__*", "Base/data/sound/music/musicList.cs");
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
	%trans = %brick.getTransform();
	%x = getWord(%trans, 0);
	%y = getWord(%trans, 1);
	%z = getWord(%trans, 2) - 1.3;
	%rot = getWords(%trans, 3, 6);
	%trans = %x SPC %y SPC %z SPC %rot;
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
	if (%bl_id == -1)
	{
		%group = "BrickGroup_LAN";
	}
	else
	{
		%group = "BrickGroup_" @ %bl_id;
	}
	if (!isObject(%group))
	{
		return;
	}
	if (%group.getClassName() !$= "SimGroup")
	{
		echo("ERROR: ServerCmdClearBrickGroup() - \"" @ %group @ "\" is not a SimGroup!");
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
	if (%bl_id == -1)
	{
		%group = "BrickGroup_LAN";
	}
	else
	{
		%group = "BrickGroup_" @ %bl_id;
	}
	if (!isObject(%group))
	{
		echo("ERROR: ServerCmdClearBrickGroup() - \"" @ %group @ "\" is not exist!");
		MessageAll('', "ERROR: ServerCmdClearBrickGroup() - \"" @ %group @ "\" is not exist!");
		return;
	}
	if (%group.getClassName() !$= "SimGroup")
	{
		echo("ERROR: ServerCmdClearBrickGroup() - \"" @ %group @ "\" is not a SimGroup!");
		MessageAll('', "ERROR: ServerCmdClearBrickGroup() - \"" @ %group @ "\" is not a SimGroup!");
		return;
	}
	if (%group.bl_id $= "LAN")
	{
		MessageAll('MsgClearBricks', '\c3%1\c2 cleared the bricks', %client.name);
	}
	else
	{
		MessageAll('MsgClearBricks', '\c3%1\c2 cleared \c3%2\c2\'s bricks', %client.name, %group.name);
	}
	if (isObject(%group.client))
	{
		%group.deleteAll();
	}
	else
	{
		%group.delete();
	}
	ServerCmdRequestBrickManList(%client);
}

function ServerCmdClearAllBricks(%client)
{
	if (!%client.isAdmin)
	{
		return;
	}
	if ($Server::BrickCount > 0)
	{
		MessageAll('MsgClearBricks', "\c3" @ %client.name @ "\c0 cleared all bricks.");
	}
	%x = 0;
	while (1)
	{
		if (%x >= mainBrickGroup.getCount())
		{
			break;
		}
		%subGroup = mainBrickGroup.getObject(%x);
		if (isObject(%subGroup.client) || $Server::LAN)
		{
			%subGroup.deleteAll();
			%x++;
		}
		else
		{
			%subGroup.delete();
		}
	}
	ServerCmdRequestBrickManList(%client);
}

function SimGroup::deleteAll(%group)
{
	while (1)
	{
		if (%group.getCount() <= 0)
		{
			return;
		}
		%obj = %group.getObject(0);
		if (isObject(%obj))
		{
			%obj.delete();
		}
		else
		{
			return;
		}
	}
}

function serverCmdTrust_Invite(%client, %targetClient, %targetBL_ID, %level)
{
	echo("serverCmdTrust_Invite from ", %client.name, " to ", %targetBL_ID, " level = ", %level);
	if (%targetBL_ID $= "LAN")
	{
		commandToClient(%client, 'MessageBoxOK', 'Trust Invite Error', 'Trust lists do not apply on a LAN.');
		return;
	}
	%ourBL_ID = %client.bl_id;
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
	if (%targetClient.bl_id != %targetBL_ID)
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
		commandToClient(%targetClient, 'TrustInvite', %client.name, %client.bl_id, 1);
	}
	else if (%level == 2)
	{
		%targetClient.invitePendingBL_ID = %ourBL_ID;
		%targetClient.invitePendingLevel = 2;
		%targetClient.invitePendingClient = %client;
		%targetClient.StartInvitationTimeout();
		commandToClient(%targetClient, 'TrustInvite', %client.name, %client.bl_id, 2);
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
	SetMutualBrickGroupTrust(%invitingBL_ID, %client.bl_id, %client.invitePendingLevel);
	commandToClient(%client, 'MessageBoxOK', 'Trust Invitation Accepted', 'You have accepted %1\'s trust invitation.', %invitingClient.name);
	commandToClient(%invitingClient, 'MessageBoxOK', 'Trust Invitation Accepted', '%1 has accepted your trust invitation.', %client.name);
	commandToClient(%invitingClient, 'TrustInviteAccepted', %client, %client.bl_id, %client.invitePendingLevel);
	messageClient(%invitingClient, 'MsgClientTrust', '', %client, %client.invitePendingLevel);
	messageClient(%client, 'MsgClientTrust', '', %invitingClient, %client.invitePendingLevel);
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
	commandToClient(%invitingClient, 'MessageBoxOK', 'Trust Invite Rejected', '%1 has rejected your trust invitation.', %client.name);
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
	commandToClient(%invitingClient, 'MessageBoxOK', 'Trust Invite Rejected + Ignored', '%1 has rejected your trust invitation and will ignore any future invites from you.', %client.name);
}

function serverCmdUnIgnore(%client, %targetClient)
{
	%targetBL_ID = %targetClient.bl_id;
	if (%client.Ignore[%targetBL_ID] == 1)
	{
		%client.Ignore[%targetBL_ID] = 0;
		commandToClient(%client, 'MessageBoxOK', 'Ignore Removed', 'You are no longer ignoring %1', %targetClient.name);
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
	%ourBL_ID = %client.bl_id;
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
			messageClient(%targetClient, 'MessageBoxOK', '%1 has removed you from their trust list.', %client.name);
		}
		messageClient(%targetClient, 'MsgClientTrust', '', %client, 0);
		messageClient(%client, 'MsgClientTrust', '', %targetClient, 0);
		commandToClient(%targetClient, 'TrustDemoted', %client, %client.bl_id, %level);
	}
	else if (%level == 1)
	{
		SetMutualBrickGroupTrust(%ourBL_ID, %targetBL_ID, 1);
		if (isObject(%targetClient))
		{
			messageClient(%targetClient, 'MessageBoxOK', '%1 has demoted you to build trust.', %client.name);
		}
		messageClient(%targetClient, 'MsgClientTrust', '', %client, 1);
		messageClient(%client, 'MsgClientTrust', '', %targetClient, 1);
		commandToClient(%targetClient, 'TrustDemoted', %client, %client.bl_id, %level);
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
		error("ERROR: GameConnection::getBL_IDTrustLevel() - \"" @ %client.name @ "\" (" @ %client @ ") does not have a brick group.");
		MessageAll('', "ERROR: GameConnection::getBL_IDTrustLevel() - \"" @ %client.name @ "\" (" @ %client @ ") does not have a brick group.");
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
	%targetLevel = %targetBrickGroup.Trust[%client.bl_id];
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
				messageClient(%client, 'MsgClientTrust', '', %otherClient, -1);
			}
			else
			{
				%level = 10;
				messageClient(%client, 'MsgClientTrust', '', %otherClient, %level);
				messageClient(%otherClient, 'MsgClientTrust', '', %client, %level);
			}
		}
		return;
	}
	%ourBrickGroup = %client.brickGroup;
	%ourBL_ID = %client.bl_id;
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
	%ourBrickGroup = "BrickGroup_" @ %client.bl_id;
	if (!isObject(%ourBrickGroup))
	{
		error("ERROR: serverCmdTrustListUpload_Line() - \"" @ %client.name @ "\" does not have a brick group.");
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
	%ourBL_ID = %client.bl_id;
	%ourBrickGroup = %client.brickGroup;
	if (!isObject(%ourBrickGroup))
	{
		error("ERROR: serverCmdTrustListUpload_Done() - \"" @ %client.name @ "\" has no brick group");
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
						messageClient(%targetClient, 'MsgClientTrust', '', %client, %currLevel);
						messageClient(%client, 'MsgClientTrust', '', %targetClient, %currLevel);
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
			messageClient(%client, 'MsgClientTrust', '', %otherClient, -1);
		}
		else
		{
			%level = mFloor(%ourBrickGroup.Trust[%otherClient.bl_id]);
			messageClient(%client, 'MsgClientTrust', '', %otherClient, %level);
			messageClient(%otherClient, 'MsgClientTrust', '', %client, %level);
		}
	}
}

$TrustLevel::None = 0;
$TrustLevel::Build = 1;
$TrustLevel::Full = 2;
$TrustLevel::Paint = 1;
$TrustLevel::FXPaint = 1;
$TrustLevel::BuildOn = 1;
$TrustLevel::Print = 1;
$TrustLevel::UndoPaint = 1;
$TrustLevel::UndoFXPaint = 1;
$TrustLevel::UndoPrint = 1;
$TrustLevel::Wrench = 1;
$TrustLevel::Hammer = 2;
$TrustLevel::Wand = 2;
$TrustLevel::UndoBrick = 2;
$TrustLevel::RideVehicle = 1;
$TrustLevel::VehicleTurnover = 1;
$TrustLevel::ItemPickup = 1;
function TrustListCheck(%obj1, %obj2, %__unused)
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
		return $TrustLevel::Full;
	}
	%brickGroup1 = getBrickGroupFromObject(%obj1);
	%bl_id1 = %brickGroup1.bl_id;
	%brickGroup2 = getBrickGroupFromObject(%obj2);
	%bl_id2 = %brickGroup2.bl_id;
	if (%bl_id1 == %bl_id2)
	{
		return $TrustLevel::Full;
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

function getBL_IDfromObject(%obj)
{
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
	}
	else if (%obj.getType() & $TypeMasks::FxBrickObjectType)
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
		messageClient(%client, '', 'You must wait %1 seconds before joining another minigame.', mFloor($Game::MiniGameJoinTime / 1000 - (getSimTime() - %client.miniGameJoinTime) / 1000) + 1);
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
		error("ERROR: serverCmdLeaveMiniGame() - \"" @ %client.name @ "\" is not in a minigame!");
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
	messageClient(%victim, '', '%1 kicked you from the minigame', %client.name);
	%client.miniGame.MessageAll('', '%1 kicked %2 from the minigame', %client.name, %victim.name);
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
	if (%victim.currentPhase < 3)
	{
		commandToClient(%client, 'MessageBoxOK', 'Mini-Game Invite Error', 'This person hasn\'t connected yet.');
		return;
	}
	%ourBL_ID = %client.bl_id;
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
	commandToClient(%victim, 'MiniGameInvite', %client.miniGame.title, %client.name, %client.bl_id, %client.miniGame);
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
	messageClient(%mg.owner, '', '%1 rejected your mini-game invitation', %client.name);
	%client.miniGameInvitePending = 0;
}

function serverCmdIgnoreMiniGameInvite(%client, %miniGameID)
{
	if (%miniGameID != %client.miniGameInvitePending)
	{
		return;
	}
	%client.miniGameInvitePending = 0;
	%mg = %client.miniGameInvitePending;
	if (!isObject(%mg))
	{
		return;
	}
	if ($Server::LAN)
	{
		return;
	}
	messageClient(%mg.owner, '', '%1 rejected your mini-game invitation', %client.name);
	%targetBL_ID = %mg.owner.bl_id;
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
		if ($MiniGameColorTaken == 0)
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
			messageClient(%client, 'CreateMiniGameFail', "You're already running a minigame.");
			return;
		}
	}
	if (%gameTitle $= "")
	{
		messageClient(%client, 'CreateMiniGameFail', "Invalid game title.");
		return;
	}
	if (%gameColorIdx < 0 || %gameColorIdx >= $MiniGameColorCount)
	{
		messageClient(%client, 'CreateMiniGameFail', "Bad color index.");
		return;
	}
	if ($MiniGameColorTaken[%gameColorIdx] == 1)
	{
		messageClient(%client, 'CreateMiniGameFail', "Game color is taken.");
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
			error("ERROR: serverCmdEndMiniGame() - \"" @ %client.name @ "\" tried to end a minigame that he's not in charge of.");
		}
	}
	else
	{
		error("ERROR: serverCmdEndMiniGame() - \"" @ %client.name @ "\" tried to end a minigame when he's not even in one.");
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
			%mg.playersUseOwnBricks = mFloor(getWord(%field, 1));
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
			%mg.WeaponDamage = mFloor(getWord(%field, 1));
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
			%mg.BrickDamage = mFloor(getWord(%field, 1));
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
	%mg = %client.miniGame;
	if (!isObject(%mg))
	{
		return;
	}
	if (%mg.owner != %client)
	{
		return;
	}
	%mg.reset();
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
		playersUseOwnBricks = 0;
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
		WeaponDamage = 0;
		SelfDamage = 0;
		VehicleDamage = 0;
		BrickDamage = 0;
		enableWand = 0;
		EnableBuilding = 0;
		playerDataBlock = PlayerStandardArmor.getId();
		StartEquip0 = 0;
		StartEquip1 = 0;
		StartEquip2 = 0;
		StartEquip3 = 0;
		StartEquip4 = 0;
	};
	MissionCleanup.add(%mg);
	%mg.addMember(%client);
	%client.miniGame = %mg;
	return %mg;
}

function MiniGameSO::addMember(%obj, %client)
{
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
		messageClient(%cl, 'MsgClientInYourMiniGame', "\c1" @ %client.name @ " joined the mini-game.", %client, 1);
		messageClient(%client, 'MsgClientInYourMiniGame', '', %cl, 1);
	}
	commandToClient(%client, 'SetPlayingMiniGame', 1);
	commandToClient(%client, 'SetBuildingDisabled', !%obj.EnableBuilding);
	commandToClient(%client, 'SetPaintingDisabled', !%obj.enablePainting);
	%client.setScore(0);
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
		messageClient(%cl, 'MsgClientInYourMiniGame', "\c1" @ %client.name @ " left the mini-game.", %client, 0);
	}
	%client.setScore(0);
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
	echo("  PlayersUseOwnBricks:", %obj.playersUseOwnBricks);
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
	echo("  WeaponDamage:", %obj.WeaponDamage);
	echo("  VehicleDamage:", %obj.VehicleDamage);
	echo("  BrickDamage:", %obj.BrickDamage);
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
			echo("    " @ %obj.member[%i] @ ": " @ %obj.member[%i].name, " <--- Owner");
		}
		else
		{
			echo("    " @ %obj.member[%i] @ ": " @ %obj.member[%i].name);
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
					%cl.Player.setDatablock(PlayerStandardArmor);
					%cl.Player.setRechargeRate(PlayerStandardArmor.rechargeRate);
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

function MiniGameSO::reset(%obj)
{
	if (%obj.useAllPlayersBricks)
	{
		for (%i = 0; %i < %obj.numMembers; %i++)
		{
			%cl = %obj.member[%i];
			%brickGroup = %cl.brickGroup;
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
		%cl.InstantRespawn();
		messageClient(%cl, '', '\c3%1\c5 reset the mini-game', %obj.owner.name);
	}
}

function MiniGameSO::RespawnAll(%obj)
{
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
				%player.setDatablock(%obj.playerDataBlock);
				%player.setRechargeRate(%obj.playerDataBlock.rechargeRate);
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
					%player.inventory[%j] = 0;
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
			if (%obj.playersUseOwnBricks)
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

function MiniGameSO::getLine(%obj)
{
	%line = %obj.owner.name TAB %obj.owner.bl_id TAB %obj.title TAB %obj.inviteOnly;
	return %line;
}

function EndAllMiniGames()
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
	if (!isObject(%miniGame1) && !isObject(%miniGame2))
	{
		return -1;
	}
	if (%miniGame1 != %miniGame2)
	{
		$lastError = $LastError::MiniGameDifferent;
		return 0;
	}
	%playerBL_ID = getBL_IDfromObject(%player);
	%thingBL_ID = getBL_IDfromObject(%thing);
	if (%miniGame1.useAllPlayersBricks)
	{
		if (%miniGame1.playersUseOwnBricks)
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
		%ownerBL_ID = %miniGame1.owner.bl_id;
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
				if (%miniGame1.WeaponDamage)
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
		else if (%type & $TypeMasks::FxBrickObjectType)
		{
			if (%miniGame1.BrickDamage)
			{
				return 1;
			}
		}
		else if (%miniGame1.WeaponDamage)
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
			if (%miniGame1.WeaponDamage)
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
	else if (%type & $TypeMasks::FxBrickObjectType)
	{
		if (%miniGame1.BrickDamage)
		{
			%ruleDamage = 1;
		}
	}
	else if (%miniGame1.WeaponDamage)
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
		%victimBL_ID = getBL_IDfromObject(%victimObject);
		if (%victimBL_ID == %miniGame1.owner.bl_id)
		{
			return 1;
		}
	}
	return 0;
}

function getMiniGameFromObject(%obj)
{
	%miniGame = -1;
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
			if (isObject(%obj.spawnBrick.getGroup().client))
			{
				%miniGame = %obj.spawnBrick.getGroup().client.miniGame;
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
	}
	else if (%obj.getType() & $TypeMasks::FxBrickObjectType)
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
		if (isObject(%obj.spawnBrick))
		{
			if (isObject(%obj.spawnBrick.getGroup().getClient()))
			{
				%miniGame = %obj.spawnBrick.getGroup().getClient().miniGame;
			}
		}
		if (!isObject(%miniGame))
		{
			if (isObject(%obj.client))
			{
				%miniGame = %obj.client.miniGame;
			}
		}
		if (!isObject(%miniGame))
		{
			if (%obj.getGroup().bl_id !$= "")
			{
				%miniGame = %obj.getGroup();
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
	%bl_id1 = getBL_IDfromObject(%obj1);
	%bl_id2 = getBL_IDfromObject(%obj2);
	%bl_idOwner = %miniGame1.owner.bl_id;
	if (%miniGame1.useAllPlayersBricks)
	{
		if (%miniGame1.playersUseOwnBricks)
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

%i = -1;
$TrustLevel::None = 0;
$TrustLevel::Build = 1;
$TrustLevel::Full = 2;
%strType = "PaintBrick";
$Interaction::Type[%strType] = %i++;
$Interaction::TrustLevel[%strType] = 1;
$Interaction::NumObjects[%strType] = 2;
$Interaction::ObjTypeMask["0", %strType] = $TypeMasks::PlayerObjectType;
$Interaction::ObjTypeMask["1", %strType] = $TypeMasks::FxBrickObjectType;
%strType = "HammerBrick";
$Interaction::Type[%strType] = %i++;
$Interaction::TrustLevel[%strType] = 2;
$Interaction::NumObjects[%strType] = 2;
$Interaction::ObjTypeMask["0", %strType] = $TypeMasks::PlayerObjectType;
$Interaction::ObjTypeMask["1", %strType] = $TypeMasks::FxBrickObjectType;
%strType = "WandBrick";
$Interaction::Type[%strType] = %i++;
$Interaction::TrustLevel[%strType] = 2;
$Interaction::NumObjects[%strType] = 2;
$Interaction::ObjTypeMask["0", %strType] = $TypeMasks::PlayerObjectType;
$Interaction::ObjTypeMask["1", %strType] = $TypeMasks::FxBrickObjectType;
function wtf(%obj0)
{
	%i = 0;
	echo("%obj0 = ", %obj[%i]);
}

function wtf2(%obj0)
{
	echo("%obj0 = ", %obj[0]);
}

function wtf3(%obj0)
{
	echo("%obj0 = ", %obj0);
	echo("%obj0 = ", %obj[0]);
}

function InteractionCheck(%strType, %obj0, %obj1, %obj2, %obj3)
{
	%intType = $Interaction::Type[%strType];
	%foo = %obj0;
	%foo = %obj1;
	%foo = %obj2;
	%foo = %obj3;
	if (%intType !$= "")
	{
		for (%i = 0; %i < $Interaction::NumObjects[%strType]; %i++)
		{
			eval("%testObj = %obj" @ %i @ ";");
			if (isObject(%testObj))
			{
				if (!(%testObj.getType() & $Interaction::ObjTypeMask[%i, %strType]))
				{
					error("ERROR: InteractionCheck() - Object " @ %i @ " (" @ %obj[%i] @ ") is not of the expected type for \"" @ %strType @ "\" interaction.");
					return 0;
				}
			}
			else
			{
				error("ERROR: InteractionCheck() - Object " @ %i @ " not provided for \"" @ %strType @ "\" interaction.");
			}
		}
	}
	else
	{
		echo("No type information for interaction \"" @ %strType @ "\"");
	}
	echo("int type = ", %intType);
	if (%intType == $Interaction::Type["ItemPickup"])
	{
	}
	if (%intType == $Interaction::Type["PaintBrick"] || %intType == $Interaction::Type["HammerBrick"] || %intType == $Interaction::Type["WandBrick"])
	{
		%brickGroup = %obj1.getGroup();
		%client = %obj0.client;
		if (!isObject(%client))
		{
			return 0;
		}
		if (%client.brickGroup == %brickGroup)
		{
			return 1;
		}
		%trustLevel = %brickGroup.Trust[%client.bl_id];
		if (%trustLevel < $Interaction::TrustLevel[%strType])
		{
			commandToClient(%client, 'CenterPrint', %brickGroup.name @ " does not trust you enough to do that.", 1);
			return 0;
		}
		else
		{
			return 1;
		}
	}
	else
	{
		echo("Unknown interaction type \"" @ %strType @ "\"");
		if (%client.isAdmin)
		{
			return 1;
		}
		else
		{
			return 0;
		}
	}
	return 0;
}

function updateAddOnList()
{
	deleteVariables("$AddOn__*");
	exec("Add-Ons/ADD_ON_LIST.cs");
	%dir = "Add-Ons/*.cs";
	%fileCount = getFileCount(%dir);
	%fileName = findFirstFile(%dir);
	for (%i = 0; %i < %fileCount; %i++)
	{
		%name = fileBase(%fileName);
		if (%name !$= "ADD_ON_LIST")
		{
			%name = strreplace(%name, " ", "_");
			%name = strreplace(%name, "-", "_");
			if ($Server::Dedicated)
			{
				if (mFloor($AddOn__[%name]) == 0)
				{
					$AddOn__[%name] = 1;
				}
				else if (mFloor($AddOn__[%name]) <= 0)
				{
					$AddOn__[%name] = -1;
				}
				else
				{
					$AddOn__[%name] = 1;
				}
				continue;
			}
			if (mFloor($AddOn__[%name]) <= 0)
			{
				$AddOn__[%name] = -1;
				continue;
			}
			$AddOn__[%name] = 1;
		}
		%fileName = findNextFile(%dir);
	}
	export("$AddOn__*", "Add-Ons/ADD_ON_LIST.cs");
}

function loadAddOns()
{
	updateAddOnList();
	%dir = "Add-Ons/*.cs";
	%fileCount = getFileCount(%dir);
	%fileName = findFirstFile(%dir);
	for (%i = 0; %i < %fileCount; %i++)
	{
		%name = fileBase(%fileName);
		if (%name !$= "ADD_ON_LIST")
		{
			%varName = %name;
			%varName = strreplace(%varName, " ", "_");
			%varName = strreplace(%varName, "-", "_");
			if ($AddOn__[%varName] $= "1")
			{
				exec(%fileName);
			}
		}
		%fileName = findNextFile(%dir);
	}
}

