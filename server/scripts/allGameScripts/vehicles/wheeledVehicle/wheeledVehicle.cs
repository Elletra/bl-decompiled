exec ("./damage.cs");
exec ("./onCollision.cs");


datablock WheeledVehicleTire (emptyTire)
{
	shapeFile = "base/data/shapes/empty.dts";
	mass = 10;
	radius = 1;
	staticFriction = 5;
	kineticFriction = 5;
	restitution = 0.5;
	lateralForce = 18000;
	lateralDamping = 4000;
	lateralRelaxation = 0.01;
	longitudinalForce = 14000;
	longitudinalDamping = 2000;
	longitudinalRelaxation = 0.01;
};

datablock WheeledVehicleSpring (emptySpring)
{
	length = 0.3;
	force = 0;
	damping = 0;
	antiSwayForce = 0;
};


function WheeledVehicleData::create ( %block )
{
	%obj = new WheeledVehicle()
	{
		dataBlock = %block;
	};

	return %obj;
}

function WheeledVehicleData::onAdd ( %this, %obj )
{
	for ( %i = 0;  %i < %this.numWheels;  %i++ )
	{
		%obj.setWheelTire (%i, %this.defaultTire);
		%obj.setWheelSpring (%i, %this.defaultSpring);
	}

	if (%this.numWheels != 0)
	{
		if ( %this.numWheels == 1 )
		{
			%obj.setWheelSteering (0, 1);
			%obj.setWheelPowered (0, 1);
		}
		else if ( %this.numWheels == 2 )
		{
			%obj.setWheelSteering (0, 1);
			%obj.setWheelSteering (1, 1);
			%obj.setWheelPowered (0, 1);
			%obj.setWheelPowered (1, 1);
		}
		else if ( %this.numWheels == 3 )
		{
			%obj.setWheelSteering (0, 1);
			%obj.setWheelPowered (1, 1);
			%obj.setWheelPowered (2, 1);
		}
		else if ( %this.numWheels == 4 )
		{
			%obj.setWheelSteering (0, 1);
			%obj.setWheelSteering (1, 1);
			%obj.setWheelPowered (2, 1);
			%obj.setWheelPowered (3, 1);
		}
		else if ( %this.numWheels == 5 )
		{
			%obj.setWheelSteering (0, 1);
			%obj.setWheelPowered (1, 1);
			%obj.setWheelPowered (2, 1);
			%obj.setWheelPowered (3, 1);
			%obj.setWheelPowered (4, 1);
		}
		else if ( %this.numWheels == 6 )
		{
			%obj.setWheelSteering (0, 1);
			%obj.setWheelSteering (1, 1);
			%obj.setWheelPowered (2, 1);
			%obj.setWheelPowered (3, 1);
			%obj.setWheelPowered (4, 1);
			%obj.setWheelPowered (5, 1);
		}
		else
		{
			%obj.setWheelSteering (0, 1);
			%obj.setWheelSteering (1, 1);
			%obj.setWheelPowered (2, 1);
			%obj.setWheelPowered (3, 1);
			%obj.setWheelPowered (4, 1);
			%obj.setWheelPowered (5, 1);
		}
	}

	%obj.creationTime = getSimTime();
}

function WheeledVehicleData::onDriverLeave ( %this, %obj )
{
	// Your code here
}
