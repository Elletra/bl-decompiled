function fxDTSBrick::setEmitter ( %obj, %data, %client )
{
	if ( isObject(%obj.emitter) )
	{
		%obj.emitter.delete();
	}

	%obj.emitter = 0;

	if ( %obj.isDead() )
	{
		return;
	}


	if ( !isObject(%client) )
	{
		%client = %obj.client;
	}

	if ( !isObject(%client) )
	{
		%client = %obj.getGroup().client;
	}

	if ( !isObject(%client) )
	{
		%client = 0;
	}


	if ( !isObject(%data) )
	{
		return;
	}

	if ( %data.getClassName() !$= "ParticleEmitterData" )
	{
		return;
	}

	if ( %data.uiName $= "" )
	{
		return;
	}


	%brickData = %obj.getDataBlock();

	if ( %brickData.brickSizeX <= 1  &&  %brickData.brickSizeY <= 1  &&  %brickData.brickSizeZ <= 3 )
	{
		%nodeData = %data.pointEmitterNode;
	}
	else
	{
		%nodeData = %data.emitterNode;
	}

	if ( !isObject(%nodeData) )
	{
		%nodeData = GenericEmitterNode;
	}

	%emitter = new ParticleEmitterNode()
	{
		dataBlock = %nodeData;
		emitter = %data;
	};

	if ( !%emitter )
	{
		if ( $Server::LAN )
		{
			commandToClient (%client, 'centerPrint', "\c6You can\'t have more than " @  
				$Server::QuotaLAN::Environment  @ " lights/emitters!", 3);
		}
		else
		{
			commandToClient (%client, 'centerPrint', "\c6You can\'t have more than " @  
				$Server::Quota::Environment  @ " lights/emitters!", 3);
		}

		return;
	}

	%emitter.setEmitterDataBlock (%data);
	%emitter.setColor ( getColorIDTable(%obj.colorID) );

	MissionCleanup.add (%emitter);


	if ( %obj.isFakeDead() )
	{
		%obj.oldEmitterDB = %data;
		%emitter.setEmitterDataBlock (0);
	}

	%emitter.brick = %obj;
	%emitter.setTransform ( %obj.getTransform() );

	%obj.emitter = %emitter;
	%obj.setEmitterDirection (%obj.emitterDirection);
}

function fxDTSBrick::setEmitterDirection ( %obj, %dir )
{
	if ( %dir < 0  ||  %dir > 5 )
	{
		return;
	}

	%obj.emitterDirection = %dir;

	if ( %obj.getDataBlock().brickSizeX == 1  &&  %obj.getDataBlock().brickSizeY == 1  &&  %obj.getDataBlock().brickSizeZ == 1 )
	{
		%scaleZ = 0;
		%scaleY = 0;
		%scaleX = 0;
	}
	else
	{
		%wbox = %obj.getWorldBox();

		%scaleX = mAbs ( getWord(%wbox, 0) - getWord(%wbox, 3) );
		%scaleY = mAbs ( getWord(%wbox, 1) - getWord(%wbox, 4) );
		%scaleZ = mAbs ( getWord(%wbox, 2) - getWord(%wbox, 5) );
	}

	if ( %scaleX < 0.55 )
	{
		%scaleX = 0;
	}

	if ( %scaleY < 0.55 )
	{
		%scaleY = 0;
	}

	if ( %scaleZ < 0.66 )
	{
		%scaleZ = 0;
	}


	if ( isObject(%obj.emitter) )
	{
		%pos = getWords(%obj.getTransform(), 0, 2);

		if ( %dir == 0 )
		{
			%rot = "0 0 1 0";
		}
		else if ( %dir == 1 )
		{
			%rot = "0 1 0 " @ $pi;
		}
		else if ( %dir == 2 )
		{
			%rot = "1 0 0 " @ $piOver2;
			%temp = %scaleY;
			%scaleY = %scaleZ;
			%scaleZ = %temp;
		}
		else if ( %dir == 3 )
		{
			%rot = "0 -1 0 " @ $piOver2;
			%temp = %scaleX;
			%scaleX = %scaleZ;
			%scaleZ = %temp;
		}
		else if ( %dir == 4 )
		{
			%rot = "-1 0 0 " @ $piOver2;
			%temp = %scaleY;
			%scaleY = %scaleZ;
			%scaleZ = %temp;
		}
		else if ( %dir == 5 )
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

		%obj.emitter.setScale (%scaleX SPC %scaleY SPC %scaleZ);
		%obj.emitter.setTransform (%pos SPC %rot);
	}
}


// =================
//  Register Events
// =================

registerOutputEvent ("fxDTSBrick", "setEmitter", "dataBlock ParticleEmitterData");
registerOutputEvent ("fxDTSBrick", "setEmitterDirection", "list Up 0 Down 1 North 2 East 3 South 4 West 5");
