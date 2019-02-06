function SimObject::setEventEnabled ( %obj, %idxList, %val )
{
	%val = mClamp (mFloor(%val), 0, 1);

	if ( %idxList $= "ALL" )
	{
		for ( %i = 0;  %i < %obj.numEvents;  %i++ )
		{
			%obj.eventEnabled[%i] = %val;
		}

		return;
	}


	%wordCount = getWordCount (%idxList);

	for ( %i = 0;  %i < %wordCount;  %i++ )
	{
		%idx = atoi ( getWord(%idxList, %i) );

		if ( %idx <= %obj.numEvents  &&  %idx >= 0)
		{
			%obj.eventEnabled[%idx] = %val;
		}
	}
}

function SimObject::toggleEventEnabled ( %obj, %idxList )
{
	if ( %idxList $= "ALL" )
	{
		for ( %i = 0;  %i < %obj.numEvents;  %i++ )
		{
			%obj.eventEnabled[%i] = !%obj.eventEnabled[%i];
		}

		return;
	}


	%wordCount = getWordCount (%idxList);

	for ( %i = 0;  %i < %wordCount;  %i++ )
	{
		%idx = atoi ( getWord(%idxList, %i) );

		if ( %idx <= %obj.numEvents  &&  %idx >= 0 )
		{
			%obj.eventEnabled[%idx] = !%obj.eventEnabled[%idx];
		}
	}
}
