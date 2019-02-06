function fxDTSBrick::sendWrenchSoundData ( %obj, %client )
{
	%data = "";
	%name = %obj.getName();

	if ( %name !$= "" )
	{
		%data = %data  TAB "N" SPC %name;
	}
	else
	{
		%data = %data  TAB "N" SPC " ";
	}

	if ( isObject(%obj.AudioEmitter) )
	{
		if ( isObject(%obj.AudioEmitter.profile) )
		{
			%emitterDB = %obj.AudioEmitter.profile.getId();
		}
		else
		{
			%emitterDB = 0;
		}
	}
	else
	{
		%emitterDB = 0;
	}

	%data = %data  TAB "SDB" SPC %emitterDB;
	%data = trim (%data);

	commandToClient (%client, 'SetWrenchData', %data);
	commandToClient (%client, 'WrenchLoadingDone');
}
