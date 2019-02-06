function New_QueueSO ( %size )
{
	if ( %size <= 1  ||  %size > 10000 )
	{
		error ("ERROR: New_QueueSO() - invalid size \'" @  %size  @ "\'");
		return;
	}


	%ret = new ScriptObject ()
	{
		class = QueueSO;
		size = %size;
		head = 0;
		tail = 0;
	};
	MissionCleanup.add (%ret);

	for ( %i = 0;  %i < %size;  %i++ )
	{
		%ret.val[%i] = 0;
	}

	return %ret;
}


function QueueSO::push ( %obj, %val )
{
	%obj.val[%obj.head] = %val;
	%obj.head = %obj.head + 1  %  %obj.size;
	%obj.val[%obj.head] = 0;

	if ( %obj.head == %obj.tail )
	{
		%obj.tail = %obj.tail + 1  %  %obj.size;
	}
}

function QueueSO::pop ( %obj )
{
	if ( %obj.head != %obj.tail )
	{
		%obj.head--;

		if ( %obj.head < 0 )
		{
			%obj.head = %obj.size - 1;
		}

		%ret = %obj.val[%obj.head];
		%obj.val[%obj.head] = 0;

		return %ret;
	}
	else
	{
		return 0;
	}
}

function QueueSO::dumpVals ( %obj )
{
	for ( %i = 0;  %i < %obj.size;  %i++ )
	{
		%line = %i  @ ": " @  %obj.val[%i];

		if ( %obj.head == %i )
		{
			%line = %line  @ " <Head";
		}

		if ( %obj.tail == %i )
		{
			%line = %line  @ " <Tail";
		}

		echo (%line);
	}
}
