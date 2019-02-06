$BRICK_TYPE::SOUND = 1;
$BRICK_TYPE::VEHICLESPAWN = 2;


datablock fxDTSBrickData (brick3x1x7WallData)
{
	brickFile = "~/data/bricks/special/3x1x7wall.blb";
	orientationFix = 1;
	category = "Special";
	subCategory = "Walls";
	uiName = "Castle Wall";
	iconName = "base/client/ui/brickIcons/Castle Wall";
};

datablock fxDTSBrickData (brick4x1x5windowData)
{
	brickFile = "~/data/bricks/special/4x1x5window.blb";
	orientationFix = 1;
	category = "Special";
	subCategory = "Walls";
	uiName = "1x4x5 Window";
	iconName = "base/client/ui/brickIcons/1x4x5 Window";
};

datablock fxDTSBrickData (brick2x2x5girderData)
{
	brickFile = "~/data/bricks/special/2x2x5girder.blb";
	category = "Special";
	subCategory = "Walls";
	uiName = "2x2x5 Lattice";
	iconName = "base/client/ui/brickIcons/2x2x5 Lattice";
};

datablock fxDTSBrickData (brickPineTreeData)
{
	brickFile = "~/data/bricks/special/pineTree.blb";
	collisionShapeName = "~/data/shapes/bricks/pineTree.dts";
	category = "Special";
	subCategory = "Misc";
	uiName = "Pine Tree";
	iconName = "base/client/ui/brickIcons/Pine Tree";
};

datablock fxDTSBrickData (brick4x1x2FenceData)
{
	brickFile = "~/data/bricks/special/4x1x2Fence.blb";
	orientationFix = 1;
	category = "Special";
	subCategory = "Walls";
	uiName = "1x4x2 Fence";
	iconName = "base/client/ui/brickIcons/1x4x2 Fence";
};

datablock fxDTSBrickData (brickMusicData)
{
	brickFile = "~/data/bricks/special/1x1music.blb";
	specialBrickType = "Sound";
	orientationFix = 1;
	category = "Special";
	subCategory = "Interactive";
	uiName = "Music Brick";
	iconName = "base/client/ui/brickIcons/Music Brick";
};

datablock fxDTSBrickData (brickSpawnPointData)
{
	brickFile = "~/data/bricks/special/spawnPoint.blb";
	iconName = "base/client/ui/brickIcons/Spawn Point";
	specialBrickType = "SpawnPoint";

	orientationFix = 1;
	indestructable = true;

	canCover = false;
	
	category = "Special";
	subCategory = "Interactive";
	uiName = "Spawn Point";
};

datablock fxDTSBrickData (brickVehicleSpawnData)
{
	brickFile = "~/data/bricks/special/vehicleSpawn.blb";
	specialBrickType = "VehicleSpawn";
	brickType = $BRICK_TYPE::VEHICLESPAWN;
	orientationFix = 1;
	canCover = 0;
	category = "Special";
	subCategory = "Interactive";
	uiName = "Vehicle Spawn";
	iconName = "base/client/ui/brickIcons/Vehicle Spawn";
	indestructable = 1;
};


exec ("./spawnPoint.cs");
