exec ("./audioDescriptions.cs");
exec ("./genericEmitterNodes.cs");
exec ("./lights.cs");
exec ("./missionMarkers.cs");
exec ("./queueSO.cs");
exec ("./staticShapes.cs");
exec ("./triggers.cs");


datablock AudioProfile (glassExplosionSound)
{
	fileName = "~/data/sound/glassBreak.wav";
	description = AudioClosest3d;
	preload = 1;
};


function Explosion::onAdd ( %this, %obj )
{
	echo ("explosion on adD");  // adD
}

function ExplosionData::onAdd ( %this, %obj )
{
	echo ("explosion on adD");
}


function serverCmdListAllDataBlocks ( %client )
{
	if ( !%client.isAdmin )
	{
		return;
	}

	if ( !%client.isLocal() )
	{
		return;
	}


	%numDataBlocks = getDataBlockGroupSize();

	echo ("SERVER SAYS, ", %numDataBlocks, " DataBlocks");
	echo ("--------------------------");

	for ( %i = 0;  %i < %numDataBlocks;  %i++ )
	{
		%db = getDataBlock (%i);
		%dbClass = %db.getClassName();

		echo (%db, " : ", %dbClass);
	}

	echo ("--------------------------");
}

function transmitDataBlocks ( %text )
{
	MessageAll ('', "Transmitting Datablocks...");

	%count = ClientGroup.getCount();

	for ( %clientIndex = 0;  %clientIndex < %count;  %clientIndex++ )
	{
		%cl = ClientGroup.getObject (%clientIndex);
		%cl.transmitDataBlocks (1);
	}
}
