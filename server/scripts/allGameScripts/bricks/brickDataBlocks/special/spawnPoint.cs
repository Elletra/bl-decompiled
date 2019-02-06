function brickSpawnPointData::onPlant ( %this, %obj )
{
	%group = %obj.getGroup();

	if ( !isObject(%group) )
	{
		error ("ERROR: brickSpawnPointData::onPlant() - " @  %obj  @ " planted outside of a group");
	}

	%group.addSpawnBrick (%obj);
}

function brickSpawnPointData::onRemove ( %this, %obj )
{
	%group = %obj.getGroup();

	if ( isObject(%group) )
	{
		%group.removeSpawnBrick (%obj);
	}
}

function brickSpawnPointData::onLoadPlant ( %this, %obj )
{
	brickSpawnPointData::onPlant (%this, %obj);
}


function fxDTSBrick::isBlocked ( %obj )
{
	%data = %obj.getDataBlock();

	%pos = %obj.getPosition();
	%size = %data.brickSizeX / 2 SPC %data.brickSizeY / 2 SPC %data.brickSizeZ / 5;

	%mask = $TypeMasks::PlayerObjectType;

	initContainerBoxSearch (%pos, %size, %mask);

	while ( ( %searchObj = containerSearchNext() ) != 0 )
	{
		%searchObjPos = %searchObj.getPosition();
		%searchObjData = %searchObj.getDataBlock();
		%searchObjRadius = getWord (%searchObjData.boundingBox, 0) / 8 * 0.75;

		%vec = VectorSub (%searchObjPos, %pos);
		%vec = getWords (%vec, 0, 1) SPC 0;

		%dist = VectorLen (%vec);

		if ( %dist < %searchObjRadius )
		{
			return true;
		}
	}

	return false;
}

function fxDTSBrick::getSpawnPoint ( %brick )
{
	%trans = %brick.getTransform();

	%x = getWord (%trans, 0);
	%y = getWord (%trans, 1);
	%z = getWord (%trans, 2) - 1.3;

	%rot = getWords (%trans, 3, 6);

	%start = %x SPC %y SPC %z + 2.8;
	%end = %x SPC %y SPC %z;

	%mask = $TypeMasks::FxBrickAlwaysObjectType;

	%raycast = containerRayCast (%start, %end, %mask, %brick);

	if ( %raycast )
	{
		%pos = posFromRaycast (%raycast);
		%pos = VectorAdd (%pos, "0 0 0.1");
		%trans = %pos SPC %rot;
	}
	else
	{
		%trans = %x SPC %y SPC %z SPC %rot;
	}

	return %trans;
}


function SimGroup::addSpawnBrick ( %group, %brick )
{
	%i = mFloor (%group.spawnBrickCount);
	%group.spawnBrick[%i] = %brick;
	%group.spawnBrickCount++;
}

function SimGroup::removeSpawnBrick ( %group, %brick )
{
	for ( %i = 0;  %i < %group.spawnBrickCount;  %i++ )
	{
		if ( %group.spawnBrick[%i] == %brick )
		{
			%group.spawnBrick[%i] = %group.spawnBrick[%group.spawnBrickCount - 1];
			%group.spawnBrick[%group.spawnBrickCount - 1] = -1;
			%group.spawnBrickCount--;

			%i--;
		}
	}
}

function SimGroup::getBrickSpawnPoint ( %group )
{
	if ( %group.spawnBrickCount <= 0 )
	{
		return pickSpawnPoint();
	}

	%startIdx = getRandom (%group.spawnBrickCount - 1);
	%brick = %group.spawnBrick[%idx];

	for ( %i = 0;  %i < %group.spawnBrickCount;  %i++ )
	{
		%idx = (%startIdx + %i)  %  %group.spawnBrickCount;
		%brick = %group.spawnBrick[%idx];

		if ( !%brick.isBlocked() )
		{
			break;
		}
	}

	return %brick.getSpawnPoint();
}

function SimGroup::dumpSpawnPoints ( %group )
{
	echo ("");
	echo (%group.spawnBrickCount, " SpawnBricks:");
	echo ("--------------------------");

	for ( %i = 0;  %i < %group.spawnBrickCount;  %i++ )
	{
		if ( isObject ( %group.spawnBrick[%i] ) )
		{
			echo ( "  ", %group.spawnBrick[%i] );
		}
		else
		{
			echo ("  ", %group.spawnBrick[%i], " <---- NOT AN OBJECT");
		}
	}

	echo ("--------------------------");
}
