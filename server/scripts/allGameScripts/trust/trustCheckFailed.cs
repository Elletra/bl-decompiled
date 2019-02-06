function fxDTSBrick::trustCheckFinished ( %obj )
{
	%obj.setTrusted (1);

	%client = %obj.client;

	if ( isObject(%client) )
	{
		if ( isObject(%client.miniGame) )
		{
			%client.incScore (%client.miniGame.Points_PlantBrick);
		}
	}

	if ( !%obj.dontCollideAfterTrust )
	{
		%obj.setColliding (1);
	}

	%obj.getDataBlock().onTrustCheckFinished (%obj);
}

function fxDTSBrick::trustCheckFailed ( %obj )
{
	%client = %obj.client;

	if ( isObject(%client) )
	{
		%client.undoStack.pop();
	}

	%obj.getDataBlock().onTrustCheckFailed (%obj);
	%obj.schedule (10, delete);
}


function fxDTSBrickData::onTrustCheckFinished ( %data, %brick )
{
	// Your code here
}

function fxDTSBrickData::onTrustCheckFailed ( %data, %brick )
{
	// Your code here
}


function GameConnection::sendTrustFailureMessage ( %client, %targetBrickGroup )
{
	commandToClient (%client, 'CenterPrint', %targetBrickGroup.getTrustFailureMessage(), 1);
}

function SimGroup::getTrustFailureMessage ( %group )
{
	%parent = %group.getGroup();

	if ( !isObject(%parent) )
	{
		%msg = "ERROR: SimGroup::getTrustFailureMessage(" @  %group.getName()  @ " [" @  %group.getId()  @ 
			"]) - brickgroup is not in a parent group";

		error (%msg);
		return %msg;
	}

	if ( %parent != mainBrickGroup.getId() )
	{
		%msg = "ERROR: SimGroup::getTrustFailureMessage(" @  %group.getName()  @ " [" @  %group.getId()  @ 
			"]) - brickgroup is not in the main brick group";
		
		error (%msg);
		return %msg;
	}

	if ( %group.bl_id $= "" )
	{
		%msg = "ERROR: SimGroup::getTrustFailureMessage(" @  %group.getName()  @ " [" @  %group.getId()  @ 
			"]) - brickgroup has no bl_id";
		
		error (%msg);
		return %msg;
	}

	if ( %group.bl_id == 888888 )
	{
		return "You cannot modify public bricks";
	}
	else
	{
		if ( %group.name $= "" )
		{
			%group.name = "\c1BL_ID: " @  %group.bl_id  @ "\c0";
		}

		%msg = %group.name  @ " does not trust you enough to do that.";
		return %msg;
	}
}
