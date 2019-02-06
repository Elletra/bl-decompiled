function Armor::onAdd ( %this, %obj )
{
	%obj.setActionThread (root);
	applyDefaultCharacterPrefs (%obj);

	%obj.mountVehicle = 1;
	%obj.setRepairRate (0);
}

function Armor::onRemove ( %this, %obj )
{
	if ( isObject(%obj.client) )
	{
		if ( %obj.client.Player == %obj )
		{
			%obj.client.Player = 0;
		}
	}

	if ( isObject(%obj.tempBrick) )
	{
		%obj.tempBrick.delete();
	}

	if ( isObject(%obj.light) )
	{
		%obj.light.delete();
	}
}
