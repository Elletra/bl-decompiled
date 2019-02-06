function serverCmdSetWrenchData ( %client, %data )
{
	%brick = %client.wrenchBrick;

	if ( !isObject(%brick) )
	{
		messageClient (%client, '', 'Wrench Error - SetWrenchData: Brick no longer exists!');
		return;
	}

	if ( %brick.isDead() )
	{
		return;
	}


	%quotaObject = getQuotaObjectFromClient (%client);

	if ( !isObject(%quotaObject) )
	{
		error ("Error: serverCmdSetWrenchData() - new quota object creation failed!");
	}

	setCurrentQuotaObject (%quotaObject);


	%fieldCount = getFieldCount (%data);
	
	for ( %i = 0;  %i < %fieldCount;  %i++ )
	{
		%field = getField (%data, %i);
		%type = getWord (%field, 0);

		if ( %type $= "N" )
		{
			%name = trim ( getSubStr(%field, 2, strlen(%field) - 2) );
			%brick.setNTObjectName (%name);
		}
		else if ( %type $= "LDB" )
		{
			%db = getWord (%field, 1);
			%brick.setLight (%db, %client);
		}
		else if ( %type $= "EDB" )
		{
			%db = getWord (%field, 1);
			%brick.setEmitter (%db, %client);
		}
		else if ( %type $= "EDIR" )
		{
			%dir = getWord (%field, 1);
			%brick.setEmitterDirection (%dir);
		}
		else if ( %type $= "IDB" )
		{
			%db = getWord (%field, 1);
			%brick.setItem (%db, %client);
		}
		else if ( %type $= "IPOS" )
		{
			%pos = getWord (%field, 1);
			%brick.setItemPosition (%pos);
		}
		else if ( %type $= "IDIR" )
		{
			%dir = getWord (%field, 1);
			%brick.setItemDirection (%dir);
		}
		else if ( %type $= "IRT" )
		{
			%time = mFloor ( getWord(%field, 1) ) * 1000.0;
			%brick.setItemRespawntime (%time);
		}
		else if ( %type $= "SDB" )
		{
			%db = getWord (%field, 1);
			%brick.setSound (%db, %client);
		}
		else if ( %type $= "VDB" )
		{
			%db = getWord (%field, 1);
			%brick.setVehicle (%db, %client);
		}
		else if ( %type $= "RCV" )
		{
			%val = getWord (%field, 1);
			%brick.setReColorVehicle (%val);
		}
		else if ( %type $= "RC" )
		{
			if ( getTrustLevel(%client, %brick) >= $TrustLevel::WrenchRaycasting )
			{
				%val = getWord (%field, 1);
				%brick.setRayCasting (%val);
			}
		}
		else if ( %type $= "C" )
		{
			if ( getTrustLevel(%client, %brick) >= $TrustLevel::WrenchCollision )
			{
				%val = getWord (%field, 1);
				%brick.setColliding (%val);
			}
		}
		else if ( %type $= "R" )
		{
			if ( getTrustLevel(%client, %brick) >= $TrustLevel::WrenchRendering )
			{
				%val = getWord (%field, 1);

				if ( isObject(%brick.emitter) )
				{
					if ( isObject(%brick.emitter.emitter) )
					{
						%edb = %brick.emitter.emitter;
					}
				}

				%brick.setRendering (%val);

				if ( isObject(%edb) )
				{
					%brick.setEmitter (%edb, %client);
				}
			}
			else
			{
				error ("ERROR: clientCmdSetWrenchData() - unknown field type " @  %field  @ "");
			}
		}
	}
}
