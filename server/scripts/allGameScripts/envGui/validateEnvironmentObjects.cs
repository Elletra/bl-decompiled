function validateEnvironmentObjects ()
{
	if ( !isObject(Sky) )
	{
		error ("ERROR: sky object not found");
		return false;
	}

	if ( Sky.getClassName() !$= "Sky" )
	{
		error ("ERROR: sky object of wrong class");
		return false;
	}

	if ( !isObject(Sun) )
	{
		error ("ERROR: sun object not found");
		return false;
	}

	if ( Sun.getClassName() !$= "Sun" )
	{
		error ("ERROR: sun object of wrong class");
		return false;
	}

	if ( !isObject(SunLight) )
	{
		error ("ERROR: sunlight object not found");
		return false;
	}

	if ( SunLight.getClassName() !$= "fxSunLight" )
	{
		error ("ERROR: sunlight object of wrong class");
		return false;
	}

	return true;
}
