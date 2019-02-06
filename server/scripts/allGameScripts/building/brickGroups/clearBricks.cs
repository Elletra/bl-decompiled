function serverCmdClearBrickGroup ( %client, %bl_id )
{
	if ( !%client.isAdmin )
	{
		return;
	}


	%bl_id = mFloor(%bl_id);
	%group = "BrickGroup_" @  %bl_id;

	if ( !isObject(%group) )
	{
		error ("ERROR: ServerCmdClearBrickGroup() - " @  %group  @ " does not exist!");
		MessageAll ('', "ERROR: ServerCmdClearBrickGroup() - " @  %group  @ " does not exist!");

		return;
	}

	if ( %group.getClassName() !$= "SimGroup" )
	{
		error ("ERROR: ServerCmdClearBrickGroup() - " @  %group  @ " is not a SimGroup!");
		MessageAll ('', "ERROR: ServerCmdClearBrickGroup() - " @  %group  @ " is not a SimGroup!");
		
		return;
	}

	if ( %group.bl_id == getLAN_BLID() )
	{
		MessageAll ( 'MsgClearBricks', '\c3%1\c2 cleared the bricks', %client.getPlayerName() );
	}
	else
	{
		MessageAll ('MsgClearBricks', '\c3%1\c2 cleared \c3%2\c2\s bricks', %client.getPlayerName(), %group.name);
	}

	%group.deleteAll();
	ServerCmdRequestBrickManList (%client);
}

function serverCmdClearAllBricks ( %client )
{
	if ( !%client.isAdmin )
	{
		return;
	}

	if ( $Game::MissionCleaningUp )
	{
		messageClient (%client, '', 'Can\'t clear bricks during mission clean up');
		return;
	}

	if ( getBrickCount() > 0 )
	{
		MessageAll ('MsgClearBricks', "\c3" @  %client.getPlayerName()  @ "\c0 cleared all bricks.");
	}

	setRaytracerAutoCenter();
	stopRaytracer();

	for ( %x = 0;  %x < mainBrickGroup.getCount();  %x++ )
	{
		%subGroup = mainBrickGroup.getObject (%x);

		if ( %subGroup.getCount() > 0 )
		{
			%subGroup.chainDeleteAll();
		}
	}

	serverCmdRequestBrickManList (%client);
}
