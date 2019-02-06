function SimGroup::getClient ( %this )
{
	if ( $Server::LAN )
	{
		error ("ERROR: SimGroup::getClient() - function should not be used in a LAN game.");
		return -1;
	}

	if ( isObject(%this.client) )
	{
		if ( %this.bl_id == %this.client.getBLID() )
		{
			return %this.client;
		}
	}

	%count = ClientGroup.getCount();

	for ( %i = 0;  %i < %count;  %i++ )
	{
		%cl = ClientGroup.getObject (%i);

		if ( %cl.getBLID() == %this.bl_id )
		{
			%this.client = %cl;
			return %cl;
		}
	}

	return 0;
}

function SimGroup::hasUser ( %this )
{
	if ( $Server::LAN )
	{
		return 1;
	}


	%count = ClientGroup.getCount();

	for ( %i = 0;  %i < %count;  %i++ )
	{
		%cl = ClientGroup.getObject (%i);

		if ( getTrustLevel(%this, %cl) )
		{
			return true;
		}
	}

	return false;
}
