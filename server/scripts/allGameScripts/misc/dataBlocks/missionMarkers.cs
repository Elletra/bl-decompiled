datablock MissionMarkerData (WayPointMarker)
{
	category = "Misc";
	shapeFile = "~/data/shapes/markers/octahedron.dts";
};

datablock MissionMarkerData (SpawnSphereMarker)
{
	category = "Misc";
	shapeFile = "~/data/shapes/markers/octahedron.dts";
};


function MissionMarkerData::create ( %block )
{
	if ( %block $= "WayPointMarker" )
	{
		%obj = new WayPoint ()
		{
			dataBlock = %block;
		};

		return %obj;
	}
	else if ( %block $= "SpawnSphereMarker" )
	{
		%obj = new SpawnSphere ()
		{
			dataBlock = %block;
		};

		return %obj;
	}

	return -1;
}
