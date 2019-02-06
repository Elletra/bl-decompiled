function cleanUpBrickEmptyGroups ()
{
	%count = ClientGroup.getCount();
	
	for ( %i = 0;  %i < %count;  %i++ )
	{
		%cl = ClientGroup.getObject (%i);
		%cl.brickGroup.doNotDelete = true;
	}

	%currTime = getSimTime();

	for ( %i = 0;  %i < mainBrickGroup.getCount();  %i++ )
	{
		%brickGroup = mainBrickGroup.getObject (%i);

		if ( !%brickGroup.doNotDelete  &&  %brickGroup.getCount() <= 0 )
		{
			if ( %currTime - %brickGroup.quitTime >= 30 * 60 * 1000 )
			{
				%brickGroup.delete();
				%i--;
			}
		}
	}


	%count = ClientGroup.getCount();

	for ( %i = 0;  %i < %count;  %i++ )
	{
		%cl = ClientGroup.getObject (%i);
		%cl.brickGroup.doNotDelete = "";
	}
}

function removeEmptyBrickGroups ()
{
	%count = ClientGroup.getCount();

	for ( %i = 0;  %i < %count;  %i++ )
	{
		%cl = ClientGroup.getObject (%i);
		%cl.brickGroup.doNotDelete = true;
	}

	for ( %i = 0;  %i < mainBrickGroup.getCount();  %i++ )
	{
		%brickGroup = mainBrickGroup.getObject (%i);

		if ( !%brickGroup.doNotDelete  &&  %brickGroup.getCount() <= 0 )
		{
			%brickGroup.delete();
		}
	}

	%count = ClientGroup.getCount();

	for ( %i = 0;  %i < %count;  %i++ )
	{
		%cl = ClientGroup.getObject (%i);
		%cl.brickGroup.doNotDelete = "";
	}
}
