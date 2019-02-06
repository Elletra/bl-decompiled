function centerPrintAll ( %message, %time, %lines )
{
	if ( %lines $= ""  ||  (%lines > 3  ||  %lines < 1 ) )
	{
		%lines = 1;
	}

	%count = ClientGroup.getCount();

	for ( %i = 0;  %i < %count;  %i++ )
	{
		%cl = ClientGroup.getObject (%i);

		if ( !%cl.isAIControlled() )
		{
			commandToClient (%cl, 'centerPrint', %message, %time, %lines);
		}
	}
}

function bottomPrintAll ( %message, %time, %lines )
{
	if ( %lines $= ""  ||  (%lines > 3  ||  %lines < 1 ) )
	{
		%lines = 1;
	}

	%count = ClientGroup.getCount();

	for ( %i = 0;  %i < %count;  %i++ )
	{
		%cl = ClientGroup.getObject (%i);

		if ( !%cl.isAIControlled() )
		{
			commandToClient (%cl, 'bottomPrint', %message, %time, %lines);
		}
	}
}

function centerPrint ( %client, %message, %time, %lines )
{
	if ( %lines $= ""  ||  (%lines > 3  ||  %lines < 1) )
	{
		%lines = 1;
	}

	commandToClient (%client, 'centerPrint', %message, %time, %lines);
}

function bottomPrint ( %client, %message, %time, %lines )
{
	if ( %lines $= ""  ||  (%lines > 3  ||  %lines < 1) )
	{
		%lines = 1;
	}

	commandToClient (%client, 'bottomPrint', %message, %time, %lines);
}

function clearCenterPrint ( %client )
{
	commandToClient (%client, 'clearCenterPrint');
}

function clearBottomPrint ( %client )
{
	commandToClient (%client, 'clearBottomPrint');
}

function clearCenterPrintAll ()
{
	%count = ClientGroup.getCount();

	for ( %i = 0;  %i < %count;  %i++ )
	{
		%cl = ClientGroup.getObject (%i);

		if ( !%cl.isAIControlled() )
		{
			commandToClient (%cl, 'clearCenterPrint');
		}
	}
}

function clearBottomPrintAll ()
{
	%count = ClientGroup.getCount();

	for ( %i = 0;  %i < %count;  %i++ )
	{
		%cl = ClientGroup.getObject (%i);

		if ( !%cl.isAIControlled() )
		{
			commandToClient (%cl, 'clearBottomPrint');
		}
	}
}
