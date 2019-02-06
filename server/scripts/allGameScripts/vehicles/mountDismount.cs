function Armor::onMount ( %this, %obj, %vehicle, %node )
{
	if ( %node == 0 )
	{
		if ( %vehicle.isHoleBot )
		{
			if ( %vehicle.controlOnMount )
			{
				%obj.setControlObject (%vehicle);
				%vehicle.lastDrivingClient = %obj.client;
			}
		}
		else if ( %vehicle.getControllingClient() == 0 )
		{
			%obj.setControlObject (%vehicle);
			%vehicle.lastDrivingClient = %obj.client;
		}
	}
	else
	{
		%obj.setControlObject (%obj);
	}

	%obj.setTransform ("0 0 0 0 0 1 0");
	%obj.playThread ( 0, %vehicle.getDataBlock().mountThread[%node] );

	ServerPlay3D ( playerMountSound,  %obj.getPosition() );


	if ( %vehicle.getDataBlock().lookUpLimit !$= "" )
	{
		%obj.setLookLimits ( %vehicle.getDataBlock().lookUpLimit,  %vehicle.getDataBlock().lookDownLimit );
	}
}

function Armor::onUnMount ( %this, %obj, %vehicle, %node )
{
	%obj.lastMountTime = getSimTime();

	if ( %node == 0 )
	{
		if ( isObject(%vehicle) )
		{
			%vehicle.onDriverLeave (%obj);
		}
	}

	%obj.setLookLimits (1, 0);
	%obj.playThread (0, root);
}

function Armor::doDismount ( %this, %obj, %forced )
{
	if ( !%obj.isMounted() )
	{
		return;
	}

	if ( !%obj.canDismount  &&  !%forced )
	{
		return;
	}

	%vehicle = %obj.getObjectMount();
	%vehicleVelocity = %vehicle.getVelocity();

	if ( %vehicle.getDataBlock().doSimpleDismount )
	{
		%obj.unmount();
		%this.onUnMount (%obj);

		%obj.setControlObject (%obj);
		%obj.setVelocity (%vehicleVelocity);

		return;
	}

	%pos = getWords (%obj.getTransform(), 0, 2);
	%oldPos = %pos;

	%scale = getWord (%vehicle.getScale(), 2);

	%vec[0] = VectorScale (" 0  0  2.2", %scale);
	%vec[1] = VectorScale (" 0  0  3", %scale);
	%vec[2] = VectorScale (" 0  0 -3", %scale);
	%vec[3] = VectorScale (" 3  0  0", %scale);
	%vec[4] = VectorScale ("-3  0  0", %scale);

	%impulseVec = "0 0 0";

	%vec[0] = MatrixMulVector ( %obj.getTransform(),  %vec[0] );

	%pos = "0 0 0";

	%numAttempts = 5;
	%success = -1;

	for ( %i = 0;  %i < %numAttempts;  %i++ )
	{
		%pos = VectorAdd ( %oldPos, VectorScale(%vec[%i], 1) );

		if ( %obj.checkDismountPoint(%oldPos, %pos) )
		{
			%success = %i;
			%impulseVec = %vec[%i];
		}
	}

	if ( %forced  &&  %success == -1 )
	{
		%pos = %oldPos;
	}

	%obj.mountVehicle = 0;
	%obj.unmount();
	%this.onUnMount (%obj);

	%obj.setControlObject (%obj);

	%obj.setVelocity (%vehicleVelocity);
	%obj.setTransform (%pos);
	%obj.applyImpulse ( %pos, VectorScale(%impulseVec, %obj.getDataBlock().mass) );
}

function Player::mountVehicles ( %this, %bool )
{
	%this.mountVehicle = %bool;
}

function fxDTSBrick::vehicleMinigameEject ( %obj )
{
	%vehicle = %obj.Vehicle;

	if ( !isObject(%vehicle) )
	{
		return;
	}

	%count = %vehicle.getMountedObjectCount();

	for ( %i = 0;  %i < %count;  %i++ )
	{
		%rider = %vehicle.getMountedObject (%i);

		if ( miniGameCanUse(%rider, %vehicle) != 1 )
		{
			%rider.getDataBlock().schedule (10, doDismount, %rider);
		}
	}
}
