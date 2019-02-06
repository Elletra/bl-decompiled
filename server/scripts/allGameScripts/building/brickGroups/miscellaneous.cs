function serverCmdRequestBrickManList ( %client )
{
	if ( !%client.isAdmin )
	{
		return;
	}

	%count = mainBrickGroup.getCount();
	
	for ( %i = 0;  %i < %count;  %i++ )
	{
		%subGroup = mainBrickGroup.getObject (%i);
		%brickCount = %subGroup.getCount();

		if ( $Server::LAN )
		{
			%line = "LAN" TAB "Everyone" TAB  %brickCount;
			commandToClient (%client, 'AddBrickManLine', -1, %line);
		}
		else
		{
			%line = %subGroup.bl_id TAB %subGroup.name TAB %brickCount;
			commandToClient (%client, 'AddBrickManLine', %subGroup.bl_id, %line);
		}
	}
}

function serverCmdDFG ( %client )
{
	if ( getBuildString() $= "Ship" )
	{
		return;
	}

	if ( !%client.isSuperAdmin )
	{
		return;
	}


	%group = %client.brickGroup;
	%count = %group.getCount();

	for ( %i = 0;  %i < %count;  %i++ )
	{
		%obj = %group.getObject (%i);

		if ( %obj.isPlanted() )
		{
			%dist = %obj.getDistanceFromGround();

			if ( %dist > 9999 )
			{
				%obj.setColor (34);
			}
			else if ( %dist == -1 )
			{
				%obj.setColor (0);
			}
			else
			{
				%obj.setColor (%dist);
			}
		}
	}
}
