exec ("./dataBlocks/dataBlocks.cs");

exec ("./centerBottomPrint.cs");
exec ("./dumpCRCValues.cs");
exec ("./E.cs");
exec ("./eulerConversion.cs");
exec ("./getAngleIDFromPlayer.cs");
exec ("./getBrickCount.cs");
exec ("./getIDGetTransform.cs");
exec ("./getLine.cs");
exec ("./getPZ.cs");
exec ("./previewCenter.cs");
exec ("./quota.cs");
exec ("./serverSettings.cs");
exec ("./simGroupClients.cs");
exec ("./time.cs");

exec ("./UNUSED.cs");


function posFromTransform ( %transform )
{
	%position = getWord (%transform, 0)  @ " " @  getWord (%transform, 1)  @ " " @  getWord (%transform, 2);
	return %position;
}

function rotFromTransform ( %transform )
{
	%rotation = getWord (%transform, 3)  @ " " @  getWord (%transform, 4)  @ " " @  
		getWord (%transform, 5)  @ " " @  getWord (%transform, 6);

	return %rotation;
}

function posFromRaycast ( %transform )
{
	%position = getWord (%transform, 1)  @ " " @  getWord (%transform, 2)  @ " " @  getWord (%transform, 3);
	return %position;
}

function normalFromRaycast ( %transform )
{
	%norm = getWord (%transform, 4)  @ " " @  getWord (%transform, 5)  @ " " @  getWord (%transform, 6);
	return %norm;
}


// This is a terrible rounding function
// Just use mFloatLength dude

function round ( %val )
{
	%val = %val * 100;
	%val = mFloor (%val + 0.5);
	%val = %val / 100;

	return %val;
}


function profileJazz ()
{
	if ( isEventPending($profileJazzEvent) )
	{
		cancel ($profileJazzEvent);
	}

	profilerDump();
	profilerEnable (1);

	schedule (500, 0, profilerReset);

	$profileJazzEvent = schedule (1000 * 60 * 5, 0, profileJazz);
}


function writeFuncOffCheck ( %file, %fnNamespace, %fnName, %protectANY )
{
	if ( getBuildString() !$= "Debug"  &&  getBuildString() !$= "Release" )
	{
		return;
	}

	%offset = getFunctionOffset (%fnNamespace, %fnName);

	if ( %fnNamespace $= "" )
	{
		%file.writeLine ("if(  (fnNamespace == NULL) && !dStricmp(fnName, " @  %fnName  @ 
			") && (ip != " @  %offset  @ ")  )");

		%file.writeLine ("   fail = true;");
	}
	else if (%protectANY)
	{
		%file.writeLine ("if(  fnNamespace && !dStricmp(fnName, " @  %fnName  @ ") && (ip != " @  
			%offset  @ ")  )");

		%file.writeLine ("   fail = true;");
	}
	else
	{
		%file.writeLine ("if(  fnNamespace && !dStricmp(fnNamespace, " @  %fnNamespace  
			@ "\") && !dStricmp(fnName, " @  %fnName  @ ") && (ip != " @  %offset  @ ")  )");

		%file.writeLine ("   fail = true;");
	}
}


function getBL_IDFromObject ( %obj )
{
	if ( !isObject(%obj) )
	{
		error ("ERROR: getBL_IDfromObject() - " @  %obj  @ " is not an object");
		return -1;
	}

	%brickGroup = getBrickGroupFromObject (%obj);

	if ( isObject(%brickGroup) )
	{
		return %brickGroup.bl_id;
	}
	else
	{
		return -1;
	}
}
