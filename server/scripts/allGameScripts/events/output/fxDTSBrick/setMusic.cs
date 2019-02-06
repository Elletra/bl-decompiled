function fxDTSBrick::setMusic ( %obj, %data, %client )
{
	%obj.setSound (%data, %client);
}

function fxDTSBrick::setSound ( %obj, %data, %client )
{
	if ( isObject(%obj.AudioEmitter) )
	{
		%obj.AudioEmitter.delete();
	}

	%obj.AudioEmitter = 0;


	if ( %obj.getDataBlock().specialBrickType !$= "Sound" )
	{
		return;
	}

	if ( !isObject(%data) )
	{
		return;
	}

	if ( %data.getClassName() !$= "AudioProfile" )
	{
		return;
	}

	if ( %data.uiName $= "" )
	{
		return;
	}

	if ( !%data.getDescription().isLooping )
	{
		return;
	}

	if ( %data.fileName $= "" )
	{
		return;
	}


	%audioEmitter = new AudioEmitter()
	{
		profile = %data;
		useProfileDescription = 1;
	};
	MissionCleanup.add (%audioEmitter);

	%obj.AudioEmitter = %audioEmitter;
	%audioEmitter.setTransform ( %obj.getTransform() );
}


// =================
//  Register Events
// =================

registerOutputEvent ("fxDTSBrick", "setMusic", "dataBlock Music");
