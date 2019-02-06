function findLocalClient ()
{
	%count = ClientGroup.getCount();

	for ( %i = 0;  %i < %count;  %i++ )
	{
		%client = ClientGroup.getObject (%i);

		if ( %client.isLocal() )
		{
			return %client;
		}
	}

	return 0;
}

function findClientByBL_ID ( %bl_id )
{
	%count = ClientGroup.getCount();

	for ( %i = 0;  %i < %count;  %i++ )
	{
		%client = ClientGroup.getObject (%i);

		if ( %client.getBLID() == %bl_id )
		{
			return %client;
		}
	}

	return 0;
}

function findClientByName ( %partialName )
{
	%pnLen = strlen (%partialName);

	%bestCL = -1;
	%bestPos = 9999;

	for ( %clientIndex = 0;  %clientIndex < ClientGroup.getCount();  %clientIndex++ )
	{
		%cl = ClientGroup.getObject (%clientIndex);

		%pos = -1;
		%name = strlwr ( %cl.getPlayerName() );
		%pos = strstr ( %name,  strlwr(%partialName) );
		
		if ( %pos != -1 )
		{
			%bestCL = %cl;

			if ( %pos == 0 )
			{
				return %cl;
			}

			if ( %pos < %bestPos )
			{
				%bestPos = %pos;
				%bestCL = %cl;
			}
		}
	}


	if ( %bestCL != -1 )
	{
		return %bestCL;
	}
	else
	{
		return 0;
	}
}
