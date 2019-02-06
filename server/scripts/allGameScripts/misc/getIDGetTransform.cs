function serverCmdGetID ( %client )
{
	if ( !%client.isAdmin )
	{
		return;
	}


	%player = %client.getControlObject();

	%start = %player.getEyePoint();

	%eyeVec = %player.getEyeVector();
	%vector = VectorScale (%eyeVec, 100);

	%end = VectorAdd (%start, %vector);

	%mask = $TypeMasks::PlayerObjectType | $TypeMasks::StaticShapeObjectType | 
		$TypeMasks::StaticObjectType | $TypeMasks::FxBrickAlwaysObjectType | 
		$TypeMasks::VehicleObjectType;


	%scanTarg = containerRayCast (%start, %end, %mask, %player);
	
	if (%scanTarg)
	{
		%pos = posFromRaycast (%scanTarg);
		%vec = VectorSub (%pos, %start);
		%dist = VectorLen (%vec);
		%scanObj = getWord (%scanTarg, 0);

		messageClient (%client, '', "objectid = " @  %scanObj  @ "  classname = " @  
			%scanObj.getClassName()  @ " distance = " @  %dist);
	}
}

function serverCmdGetTransform ( %client )
{
	if ( !%client.isAdmin )
	{
		return;
	}


	%player = %client.getControlObject();

	%start = %player.getEyePoint();

	%eyeVec = %player.getEyeVector();
	%vector = VectorScale (%eyeVec, 100);

	%end = VectorAdd (%start, %vector);

	%mask = $TypeMasks::PlayerObjectType | $TypeMasks::StaticShapeObjectType | 
		$TypeMasks::StaticObjectType | $TypeMasks::FxBrickAlwaysObjectType | 
		$TypeMasks::VehicleObjectType;


	%scanTarg = containerRayCast (%start, %end, %mask, %player);

	if ( %scanTarg )
	{
		%pos = posFromRaycast (%scanTarg);
		%vec = VectorSub (%pos, %start);
		%dist = VectorLen (%vec);
		%scanObj = getWord (%scanTarg, 0);

		messageClient ( %client,  '',  %scanObj.getTransform() );
	}
}
