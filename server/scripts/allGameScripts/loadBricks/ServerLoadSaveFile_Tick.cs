function ServerLoadSaveFile_Tick ()
{
	if ( isObject(ServerConnection) )
	{
		if ( !ServerConnection.isLocal() )
		{
			return;
		}
	}

	%line = $Server_LoadFileObj.readLine();

	if ( trim(%line) $= "" )
	{
		return;
	}

	%firstWord = getWord (%line, 0);

	if ( %firstWord $= "+-EVENT" )
	{
		if ( isObject($LastLoadedBrick) )
		{
			%idx = getField (%line, 1);
			%enabled = getField (%line, 2);
			%inputName = getField (%line, 3);
			%delay = getField (%line, 4);

			%targetName = getField (%line, 5);
			%NT = getField (%line, 6);

			%outputName = getField (%line, 7);

			%par1 = getField (%line, 8);
			%par2 = getField (%line, 9);
			%par3 = getField (%line, 10);
			%par4 = getField (%line, 11);

			%inputEventIdx = inputEvent_GetInputEventIdx (%inputName);

			%targetIdx = inputEvent_GetTargetIndex ("fxDTSBrick", %inputEventIdx, %targetName);

			if ( %targetName == -1 )
			{
				%targetClass = "fxDTSBrick";
			}
			else
			{
				%field = getField ( $InputEvent_TargetList["fxDTSBrick", %inputEventIdx],  %targetIdx );
				%targetClass = getWord (%field, 1);
			}

			%outputEventIdx = outputEvent_GetOutputEventIdx(%targetClass, %outputName);
			%NTNameIdx = -1;

			if ( $LoadingBricks_Client == $LoadingBricks_BrickGroup )
			{
				for ( %j = 0;  %j < 4;  %j++)
				{
					%field = getField ( $OutputEvent_parameterList[%targetClass, %outputEventIdx],  %j );
					%dataType = getWord (%field, 0);

					if ( %dataType $= "datablock" )
					{
						if ( %par[%j + 1] != -1  &&  !isObject ( %par[%j + 1] ) )
						{
							warn ( "WARNING: could not find datablock for event " @  %outputName  @ " -> " @  %par[%j + 1] );
						}
					}
				}
			}

			$LoadingBricks_Client.wrenchBrick = $LastLoadedBrick;

			serverCmdAddEvent ($LoadingBricks_Client, %enabled, %inputEventIdx, %delay, %targetIdx, %NTNameIdx, 
				%outputEventIdx, %par1, %par2, %par3, %par4);

			$LastLoadedBrick.eventNT[$LastLoadedBrick.numEvents - 1] = %NT;
		}
	}
	else if ( %firstWord $= "+-NTOBJECTNAME" )
	{
		if ( isObject($LastLoadedBrick) )
		{
			%name = getWord (%line, 1);
			$LastLoadedBrick.setNTObjectName (%name);
		}
	}
	else if ( %firstWord $= "+-LIGHT" )
	{
		if ( isObject($LastLoadedBrick) )
		{
			%line = getSubStr (%line, 8, strlen(%line) - 8);
			%pos = strpos (%line, "\"");

			%dbName = getSubStr (%line, 0, %pos);
			%db = $uiNameTable_Lights[%dbName];

			if ( $LoadingBricks_Client == $LoadingBricks_BrickGroup )
			{
				if ( !isObject(%db) )
				{
					warn ("WARNING: could not find light datablock for uiname " @  %dbName);
				}
			}

			if ( !isObject(%db) )
			{
				%db = $uiNameTable_Lights["Player\'s Light"];
			}

			if ( strlen(%line) - %pos - 2 >= 0 )
			{
				%line = getSubStr (%line, %pos + 2, strlen(%line) - %pos - 2);
				%enabled = %line;

				if ( %enabled $= "" )
				{
					%enabled = 1;
				}
			}
			else
			{
				%enabled = 1;
			}

			%quotaObject = getQuotaObjectFromBrick ($LastLoadedBrick);
			setCurrentQuotaObject (%quotaObject);


			$LastLoadedBrick.setLight (%db);

			if ( isObject($LastLoadedBrick.light) )
			{
				$LastLoadedBrick.light.setEnable (%enabled);
			}

			clearCurrentQuotaObject();
		}
	}
	else if ( %firstWord $= "+-EMITTER" )
	{
		if ( isObject($LastLoadedBrick) )
		{
			%line = getSubStr (%line, 10, strlen(%line) - 10);
			%pos = strpos (%line, "\"");
			%dbName = getSubStr (%line, 0, %pos);

			if ( %dbName $= "NONE" )
			{
				%db = 0;
			}
			else
			{
				%db = $uiNameTable_Emitters[%dbName];
			}

			if ( $LoadingBricks_Client == $LoadingBricks_BrickGroup )
			{
				if ( %db $= "")
				{
					warn ("WARNING: could not find emitter datablock for uiname " @  %dbName  @ "");
				}
			}

			%line = getSubStr (%line, %pos + 2, strlen(%line) - %pos - 2);
			%dir = getWord (%line, 0);

			%quotaObject = getQuotaObjectFromBrick ($LastLoadedBrick);
			setCurrentQuotaObject (%quotaObject);


			if ( isObject(%db) )
			{
				$LastLoadedBrick.setEmitter (%db);
			}

			$LastLoadedBrick.setEmitterDirection (%dir);

			clearCurrentQuotaObject();
		}
	}
	else if ( %firstWord $= "+-ITEM" )
	{
		if ( isObject($LastLoadedBrick) )
		{
			%line = getSubStr (%line, 7, strlen(%line) - 7);
			%pos = strpos (%line, "\"");
			%dbName = getSubStr (%line, 0, %pos);

			if ( %dbName $= "NONE" )
			{
				%db = 0;
			}
			else
			{
				%db = $uiNameTable_Items[%dbName];
			}

			if ( $LoadingBricks_Client == $LoadingBricks_BrickGroup )
			{
				if ( %dbName !$= "NONE"  &&  !isObject(%db) )
				{
					warn ("WARNING: could not find item datablock for uiname " @  %dbName  @ "");
				}
			}

			%line = getSubStr (%line, %pos + 2, strlen(%line) - %pos - 2);
			%pos = getWord (%line, 0);
			%dir = getWord (%line, 1);
			%respawnTime = getWord (%line, 2);

			%quotaObject = getQuotaObjectFromBrick ($LastLoadedBrick);
			setCurrentQuotaObject (%quotaObject);

			if ( isObject(%db) )
			{
				$LastLoadedBrick.setItem (%db);
			}

			$LastLoadedBrick.setItemDirection (%dir);
			$LastLoadedBrick.setItemPosition (%pos);
			$LastLoadedBrick.setItemRespawntime (%respawnTime);

			clearCurrentQuotaObject();
		}
	}
	else if ( %firstWord $= "+-AUDIOEMITTER" )
	{
		if (isObject($LastLoadedBrick))
		{
			%line = getSubStr(%line, 15, strlen(%line) - 15.0);
			%pos = strpos (%line, "\"");
			%dbName = getSubStr(%line, 0, %pos);
			%db = $uiNameTable_Music[%dbName];
			if ($LoadingBricks_Client == $LoadingBricks_BrickGroup)
			{
				if (!isObject(%db))
				{
					warn("WARNING: could not find music datablock for uiname " @ %dbName @ "");
				}
			}
			%quotaObject = getQuotaObjectFromBrick($LastLoadedBrick);
			setCurrentQuotaObject(%quotaObject);
			$LastLoadedBrick.setSound(%db);
			clearCurrentQuotaObject();
		}
	}
	else if ( %firstWord $= "+-VEHICLE" )
	{
		if ( isObject($LastLoadedBrick) )
		{
			%line = getSubStr (%line, 10, strlen(%line) - 10);
			%pos = strpos (%line, "\"");

			%dbName = getSubStr (%line, 0, %pos);
			%db = $uiNameTable_Vehicle[%dbName];

			if ( $LoadingBricks_Client == $LoadingBricks_BrickGroup )
			{
				if ( !isObject(%db) )
				{
					warn ("WARNING: could not find vehicle datablock for uiname " @  %dbName  @ "");
				}
			}

			%line = getSubStr (%line, %pos + 2, strlen(%line) - %pos - 2);
			%recolorVehicle = getWord (%line, 0);

			%quotaObject = getQuotaObjectFromBrick ($LastLoadedBrick);
			setCurrentQuotaObject (%quotaObject);

			$LastLoadedBrick.setVehicle (%db);
			$LastLoadedBrick.setReColorVehicle (%recolorVehicle);

			clearCurrentQuotaObject();
		}
	}
	else if ( %firstWord $= "Linecount" )
	{
		if ( isObject(ProgressGui) )
		{
			Progress_Bar.total = getWord (%line, 1);
			Progress_Bar.setValue (0);
			Progress_Bar.count = 0;

			Canvas.popDialog (ProgressGui);

			Progress_Window.setText ("Loading Progress");
			Progress_Text.setText ("Loading...");
		}
	}
	else if ( %firstWord $= "+-OWNER" )
	{
		if ( isObject($LastLoadedBrick)  &&  $LoadingBricks_Ownership == 1 )
		{
			%ownerBLID = mAbs ( mFloor ( getWord(%line, 1) ) );
			%oldGroup = $LastLoadedBrick.getGroup();

			if ( $Server::LAN )
			{
				$LastLoadedBrick.bl_id = %ownerBLID;
			}
			else if ( %ownerBLID != 999999 )
			{
				%ownerBrickGroup = "BrickGroup_" @  %ownerBLID;

				if ( isObject(%ownerBrickGroup) )
				{
					%ownerBrickGroup = %ownerBrickGroup.getId();
				}
				else
				{
					%ownerBrickGroup = new SimGroup ("BrickGroup_" @  %ownerBLID)
					{
						client = 0;
						name = "\c1BL_ID: " @  %ownerBLID  @ "\c0";
						bl_id = %ownerBLID;
					};

					mainBrickGroup.add (%ownerBrickGroup);
				}

				if ( isObject(%ownerBrickGroup) )
				{
					%ownerBrickGroup.add ($LastLoadedBrick);

					if ( isObject(brickSpawnPointData) )
					{
						if ( $LastLoadedBrick.getDataBlock().getId() == brickSpawnPointData.getId() )
						{
							if ( %ownerBrickGroup != %oldGroup )
							{
								%oldGroup.removeSpawnBrick ($LastLoadedBrick);
								%ownerBrickGroup.addSpawnBrick ($LastLoadedBrick);
							}
						}
					}
				}
			}
		}
	}
	else
	{
		if ( getBrickCount() >= getBrickLimit() )
		{
			MessageAll ( '',  'Brick limit reached (%1)',  getBrickLimit() );
			ServerLoadSaveFile_End();

			return;
		}

		%quotePos = strstr (%line, "\"");

		if ( %quotePos <= 0 )
		{
			error ("ERROR: ServerLoadSaveFile_Tick() - Bad line " @  %line  @ " - expected brick line but found no uiname");
			return;
		}


		%uiName = getSubStr (%line, 0, %quotePos);
		%db = $uiNameTable[%uiName];

		%line = getSubStr (%line, %quotePos + 2, 9999);

		%pos = getWords (%line, 0, 2);
		%angId = getWord (%line, 3);

		%isBaseplate = getWord (%line, 4);

		%colorId = $colorTranslation[ mFloor( getWord(%line, 5) ) ];
		%printName = getWord (%line, 6);

		if ( strpos(%printName, "/") != -1 )
		{
			%printName = fileBase (%printName);

			%aspectRatio = %db.printAspectRatio;

			%printIDName = %aspectRatio  @ "/" @  %printName;
			%printId = $printNameTable[%printIDName];

			if ( %printId $= "" )
			{
				%printIDName = "Letters/" @  %printName;
				%printId = $printNameTable[%printIDName];
			}

			if ( %printId $= "" )
			{
				%printId = $printNameTable["Letters/-space"];
			}
		}
		else
		{
			%printId = $printNameTable[%printName];
		}

		%colorFX = getWord (%line, 7);
		%shapeFX = getWord (%line, 8);

		%rayCasting = getWord (%line, 9);
		%collision = getWord (%line, 10);
		%rendering = getWord (%line, 11);

		%pos = VectorAdd (%pos, $LoadingBricks_PositionOffset);

		if ( %db )
		{
			%trans = %pos;

			if ( %angId == 0 )
			{
				%trans = %trans  @ " 1 0 0 0";
			}
			else if ( %angId == 1 )
			{
				%trans = %trans  @ " 0 0 1 " @  $piOver2;
			}
			else if ( %angId == 2 )
			{
				%trans = %trans  @ " 0 0 1 " @  $pi;
			}
			else if ( %angId == 3 )
			{
				%trans = %trans  @ " 0 0 -1 " @  $piOver2;
			}

			%b = new fxDTSBrick ()
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

			if ( isObject($LoadingBricks_BrickGroup) )
			{
				$LoadingBricks_BrickGroup.add (%b);
			}
			else
			{
				error ("ERROR: ServerLoadSaveFile_Tick() - $LoadingBricks_BrickGroup does not exist!");
				MessageAll ('', "ERROR: ServerLoadSaveFile_Tick() - $LoadingBricks_BrickGroup does not exist!");

				%b.delete();
				ServerLoadSaveFile_End();

				return;
			}

			%b.setTransform (%trans);
			%b.trustCheckFinished();

			$LastLoadedBrick = %b;

			%err = %b.plant();

			if ( %err == 1  ||  %err == 3  ||  %err == 5 )
			{
				$Load_failureCount++;
				%b.delete();
				$LastLoadedBrick = 0;
			}
			else
			{
				if ( %rayCasting !$= "" )
				{
					%b.setRayCasting (%rayCasting);
				}

				if ( %collision !$= "" )
				{
					%b.setColliding (%collision);
				}

				if ( %rendering !$= "" )
				{
					%b.setRendering (%rendering);
				}

				if ( $LoadingBricks_Ownership  &&  !$Server::LAN )
				{
					%oldGroup = %b.getGroup();
					%ownerGroup = "";

					if ( %b.getNumDownBricks() )
					{
						%ownerGroup = %b.getDownBrick(0).getGroup();
						%ownerGroup.add (%b);
					}
					else
					{
						if ( %b.getNumUpBricks() )
						{
							%ownerGroup = %b.getUpBrick(0).getGroup();
							%ownerGroup.add (%b);
						}
					}


					if ( isObject(brickSpawnPointData) )
					{
						if ( %b.getDataBlock().getId() == brickSpawnPointData.getId() )
						{
							if ( %ownerGroup > 0  &&  %ownerGroup != %oldGroup )
							{
								%oldGroup.removeSpawnBrick (%b);
								%ownerGroup.addSpawnBrick (%b);
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
			if ( !$Load_MissingBrickWarned[%uiName] )
			{
				warn ("WARNING: loadBricks() - DataBlock not found for brick named ", %uiName, "");
				$Load_MissingBrickWarned[%uiName] = 1;
			}

			$LastLoadedBrick = 0;
			$Load_failureCount++;
		}

		$Load_brickCount++;


		if ( isObject(ProgressGui) )
		{
			Progress_Bar.count++;
			Progress_Bar.setValue (Progress_Bar.count / Progress_Bar.total);

			if ( Progress_Bar.count + 1  == Progress_Bar.total )
			{
				Canvas.popDialog (ProgressGui);
			}
		}
	}


	if ( !$Server_LoadFileObj.isEOF() )
	{
		if ( $Server::ServerType $= "SinglePlayer" )
		{
			$LoadSaveFile_Tick_Schedule = schedule (0, 0, ServerLoadSaveFile_Tick);
		}
		else
		{
			$LoadSaveFile_Tick_Schedule = schedule (0, 0, ServerLoadSaveFile_Tick);
		}
	}
	else
	{
		ServerLoadSaveFile_End();
	}
}
