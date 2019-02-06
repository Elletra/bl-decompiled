datablock AudioProfile (BrickBreakSound)
{
	fileName = "~/data/sound/breakBrick.wav";
	description = AudioClose3d;
	preload = 1;
};

datablock AudioProfile (BrickMoveSound)
{
	fileName = "~/data/sound/clickMove.wav";
	description = AudioClose3d;
	preload = 1;
};

datablock AudioProfile (BrickPlantSound)
{
	fileName = "~/data/sound/clickPlant.wav";
	description = AudioClose3d;
	preload = 1;
};

datablock AudioProfile (BrickRotateSound)
{
	fileName = "~/data/sound/clickRotate.wav";
	description = AudioClose3d;
	preload = 1;
};

datablock AudioProfile (BrickChangeSound)
{
	fileName = "~/data/sound/clickChange.wav";
	description = AudioClose3d;
	preload = 1;
};


function serverCmdCancelBrick ( %client )
{
	%player = %client.Player;

	if ( %player )
	{
		if ( isObject(%player.tempBrick) )
		{
			%player.tempBrick.delete();
			%player.tempBrick = "";
		}
	}
}


exec ("./shiftBrick.cs");
exec ("./rotateBrick.cs");
exec ("./plantBrick.cs");
exec ("./undoBrick.cs");

exec ("./brickInventory/brickInventory.cs");
exec ("./brickGroups/brickGroups.cs");
