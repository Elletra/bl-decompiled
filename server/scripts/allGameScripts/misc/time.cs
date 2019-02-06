// Time, Dr. Freeman...?


function getTimeString ( %timeS )
{
	if ( %timeS >= 3600 )
	{
		%hours = mFloor (%timeS / 3600);
		%timeS = %timeS - %hours * 3600;

		%minutes = mFloor (%timeS / 60);
		%timeS = %timeS - %minutes * 60;

		%seconds = %timeS;

		if ( %minutes < 10 )
		{
			%minutes = 0 @ %minutes;
		}

		if ( %seconds < 10 )
		{
			%seconds = 0 @ %seconds;
		}

		return %hours  @ ":" @  %minutes  @ ":" @  %seconds;
	}
	else if ( %timeS >= 60 )
	{
		%minutes = mFloor (%timeS / 60);
		%timeS = %timeS - %minutes * 60;
		%seconds = %timeS;

		if ( %seconds < 10 )
		{
			%seconds = 0 @ %seconds;
		}

		return %minutes  @ ":" @  %seconds;
	}
	else
	{
		%seconds = %timeS;

		if ( %seconds < 10 )
		{
			%seconds = 0 @ %seconds;
		}

		return "0:" @  %seconds;
	}
}

// ((( CURRENT YEAR )))

function getCurrentYear ()
{
	return mFloor ( getSubStr ( getDateTime(),  6,  2) );
}

function getCurrentMinuteOfYear ()
{
	%time = getDateTime();

	%month = mFloor ( getSubStr(%time, 0, 2) );
	%day = mFloor ( getSubStr(%time, 3, 2) );
	%year = mFloor ( getSubStr(%time, 6, 2) );
	%hour = mFloor ( getSubStr(%time, 9, 2) );
	%minute = mFloor ( getSubStr(%time, 12, 2) );

	%dayOfYear = getDayOfYear (%month, %day);

	%currTime = %minute;
	%currTime += %hour * 60;
	%currTime += %dayOfYear * 60 * 24;

	return %currTime;
}
