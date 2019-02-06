function getBrickGroupFromObject ( %obj )
{
	if ( !isObject(%obj) )
	{
		error ("ERROR: getBrickGroupfromObject() - " @  %obj  @ " is not an object");
		return -1;
	}

	%brickGroup = -1;

	if ( %obj.getClassName() $= "GameConnection" )
	{
		%brickGroup = %obj.brickGroup;
	}
	else if ( %obj.getClassName() $= "SimGroup" )
	{
		%brickGroup = %obj;
	}
	else if ( %obj.getType() & $TypeMasks::PlayerObjectType )
	{
		if ( isObject(%obj.client) )
		{
			%brickGroup = %obj.client.brickGroup;
		}
		else
		{
			if ( isObject(%obj.spawnBrick) )
			{
				%brickGroup = %obj.spawnBrick.getGroup();
			}
		}
	}
	else if ( %obj.getType() & $TypeMasks::ItemObjectType )
	{
		if ( isObject(%obj.spawnBrick) )
		{
			%brickGroup = %obj.spawnBrick.getGroup();
		}
		else
		{
			%brickGroup = "BrickGroup_" @  %obj.bl_id;
		}
	}
	else if ( %obj.getType() & $TypeMasks::FxBrickAlwaysObjectType )
	{
		%brickGroup = %obj.getGroup();
	}
	else if ( %obj.getType() & $TypeMasks::VehicleObjectType )
	{
		if ( isObject(%obj.spawnBrick) )
		{
			%brickGroup = %obj.spawnBrick.getGroup();
		}
	}
	else if ( %obj.getType() & $TypeMasks::ProjectileObjectType )
	{
		if ( isObject(%obj.client) )
		{
			%brickGroup = %obj.client.brickGroup;
		}
	}
	else
	{
		if ( isObject(%obj.spawnBrick) )
		{
			%brickGroup = %obj.spawnBrick.getGroup();
		}

		if ( isObject(%obj.client) )
		{
			%brickGroup = %obj.client.brickGroup;
		}

		if ( %obj.getGroup().bl_id !$= "" )
		{
			%brickGroup = %obj.getGroup();
		}
	}

	return %brickGroup;
}
