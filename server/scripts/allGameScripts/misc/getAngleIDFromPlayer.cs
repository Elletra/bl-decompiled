function getAngleIDFromPlayer ( %player )
{
	%forwardVec = %player.getForwardVector();

	%forwardX = getWord (%forwardVec, 0);
	%forwardY = getWord (%forwardVec, 1);

	if ( %forwardX > 0 )
	{
		if ( %forwardX > mAbs(%forwardY) )
		{
			return 0;
		}
		else if ( %forwardY > 0 )
		{
			return 1;
		}
		else
		{
			return 3;
		}
	}
	else
	{
		if ( mAbs(%forwardX) > mAbs(%forwardY) )
		{
			return 2;
		}
		else if ( %forwardY > 0 )
		{
			return 1;
		}
		else
		{
			return 3;
		}
	}
}
