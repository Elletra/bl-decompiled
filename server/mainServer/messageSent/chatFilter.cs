function chatFilter ( %client, %text, %badList, %failMessage )
{
	%lwrText = " " @  strlwr (%text)  @ " ";

	%lwrText = strreplace (%lwrText, ".dat", "");
	%lwrText = strreplace (%lwrText, "/u/",  "");
	%lwrText = strreplace (%lwrText, "?",   " ");
	%lwrText = strreplace (%lwrText, "!",   " ");
	%lwrText = strreplace (%lwrText, ".",   " ");
	%lwrText = strreplace (%lwrText, "/",   " ");

	%lastChar = getSubStr (%badList, strlen(%badList) - 1, 1);

	if ( %lastChar !$= "," )
	{
		%badList = %badList  @ ",";
	}


	%offset = 0;
	%max = strlen (%badList) - 1;
	
	%i = 0;

	while ( %offset < %max )
	{
		%i++;

		if ( %i >= 1000 )
		{
			error ("ERROR: chatFilter() - loop safety hit");
			return 1;
		}

		%nextDelim = strpos (%badList, ",", %offset);

		if ( %nextDelim == -1 )
		{
			%offset = %max;
		}

		%wordLen = %nextDelim - %offset;
		%word = getSubStr (%badList, %offset, %wordLen);

		if ( strstr(%lwrText, %word) != -1 )
		{
			messageClient (%client, '', %failMessage, %word);
			return 0;
		}

		%offset += %wordLen + 1;
	}

	return 1;
}
