function clientCmdChatMessage (%sender, %voice, %pitch, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10)
{
	onChatMessage (detag (%msgString), %voice, %pitch);
}

function clientCmdServerMessage (%msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10)
{
	%tag = getWord (%msgType, 0);
	%i = 0;
	while ((%func = $MSGCB["", %i]) !$= "")
	{
		call (%func, %msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10);
		%i += 1;
	}
	if (%tag !$= "")
	{
		%i = 0;
		while ((%func = $MSGCB[%tag, %i]) !$= "")
		{
			call (%func, %msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10);
			%i += 1;
		}
	}
}

function addMessageCallback (%msgType, %func)
{
	%i = 0;
	while ((%afunc = $MSGCB[%msgType, %i]) !$= "")
	{
		if (%afunc $= %func)
		{
			return;
		}
		%i += 1;
	}
	$MSGCB[%msgType, %i] = %func;
}

function defaultMessageCallback (%msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10)
{
	onServerMessage (detag (%msgString));
}

addMessageCallback ("", defaultMessageCallback);
