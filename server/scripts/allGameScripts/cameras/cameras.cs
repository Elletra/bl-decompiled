$Camera::movementSpeed = 40;


datablock CameraData (Observer)
{
	mode = "Observer";
};

function Observer::setMode ( %this, %obj, %mode, %arg1, %arg2, %arg3 )
{
	if ( %mode $= "Observer" )
	{
		%currTarget = %obj.getOrbitObject();
		%obj.setFlyMode();
	}
	else if ( %mode $= "Corpse" )
	{
		%obj.unmountImage (0);
		%transform = %arg1.getTransform();
		%obj.setOrbitMode (%arg1, %transform, 0, 8, 8);
	}

	%obj.mode = %mode;
}

function Camera::onAdd ( %this, %obj )
{
	%this.setMode (%this.mode);
}

function Camera::setMode ( %this, %mode, %arg1, %arg2, %arg3 )
{
	%this.getDataBlock().setMode (%this, %mode, %arg1, %arg2, %arg3);
}


exec ("./cameraImage.cs");
exec ("./orbitMode.cs");
exec ("./onTrigger.cs");
