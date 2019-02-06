function getLine ( %phrase, %lineNum )
{
	%offset = 0;

	for ( %lineCount = 0;  %lineCount <= %lineNum;  %lineCount++ )
	{
		%pos = strpos (%phrase, "\n", %offset);

		if ( %pos >= 0 )
		{
			%len = %pos - %offset;
		}
		else
		{
			%len = 99999;
		}


		%line = getSubStr (%phrase, %offset, %len);

		if ( %lineCount == %lineNum )
		{
			return %line;
		}

		%offset = %pos + 1;

		if ( %pos == -1 )
		{
			return "";
		}
	}

	return "";
}

function getLineCount ( %phrase )
{
	%offset = 0;
	%lineCount = 0;

	while ( %offset >= 0 )
	{
		%offset = strpos (%phrase, "\n", %offset + 1);
		%lineCount++;
	}

	return %lineCount;
}
