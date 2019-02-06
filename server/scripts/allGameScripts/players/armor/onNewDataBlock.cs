function Armor::onNewDataBlock ( %this, %player )
{
	if ( !isObject(%player.client) )
	{
		applyDefaultCharacterPrefs (%player);
	}
	else
	{
		applyCharacterPrefs (%player.client);
	}


	%data = %this;

	if ( %data.rideAble )
	{
		%count = %player.getMountedObjectCount();
		%list = "";

		for ( %i = 0;  %i < %count;  %i++ )
		{
			%rider = %player.getMountedObject (0);

			if ( %i == 0 )
			{
				%list = %rider;
			}
			else
			{
				%list = %list SPC %rider;
			}
		}

		for ( %i = 0;  %i < %count;  %i++ )
		{
			%rider = getWord (%list, %i);
			%rider.getDataBlock().doDismount (%rider, 1);
		}

		if ( %data.nummountpoints < %count )
		{
			%count = %data.nummountpoints;
		}


		for ( %i = 0;  %i < %count;  %i++ )
		{
			%rider = getWord (%list, %i);
			%mountNode = %data.mountNode[%i];

			if ( %mountNode $= "" )
			{
				%mountNode = %i;
			}

			%player.mountObject (%rider, %mountNode);

			if ( %i == 0 )
			{
				%rider.setControlObject (%player);
			}
		}
	}
	else
	{
		%count = %player.getMountedObjectCount();

		for ( %i = 0;  %i < %count;  %i++ )
		{
			%rider = %player.getMountedObject (0);
			%rider.getDataBlock().doDismount (%rider, 1);
		}
	}
}
