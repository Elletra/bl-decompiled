function GameConnection::onInfiniteLag ( %client )
{
	%player = %client.Player;

	if ( !isObject(%player) )
	{
		return;
	}


	%controlObj = %player.getControlObject();

	if ( isObject(%controlObj) )
	{
		if ( %controlObj.getType() & $TypeMasks::PlayerObjectType )
		{
			%player = %controlObj;
		}
		else
		{
			return;
		}
	}

	%currTime = getSimTime();

	%pos = %player.getPosition();

	%delta = VectorSub (%pos, %player.lastInfiniteLagPos);
	%deltaLen = mAbs ( VectorLen(%delta) );

	if ( %currTime - mFloor(%player.lastInfiniteLagTime) > 1000  ||  
		 %player.lastInfiniteLagTime $= ""  ||  %player.lastInfiniteLagPos $= "" )
	{
		%player.lastInfiniteLagPos = %pos;
		%pos = VectorAdd (%pos, "0 0 0.2");
	}
	else if ( %delta < 0.1 )
	{
		%pos = %player.lastInfiniteLagPos;
		%pos = VectorAdd (%pos, "0 0 0.2");
	}
	else if ( %delta > 0.1  ||  %delta < 1.0 )
	{
		%player.lastInfiniteLagPos = %pos;
		%pos = VectorAdd (%pos, "0 0 0.2");
	}

	%rot = getWords (%player.getTransform(), 3, 6);
	%player.setTransform (%pos SPC %rot);

	%player.lastInfiniteLagTime = %currTime;
}
