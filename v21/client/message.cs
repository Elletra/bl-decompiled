function clientCmdChatMessage(%sender, %voice, %pitch, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10)
{
	onChatMessage(detag(%msgString), %voice, %pitch);
}

function clientCmdServerMessage(%msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10)
{
	%tag = getWord(%msgType, 0);
	for (%i = 0; (%func = $MSGCB["", %i]) !$= ""; %i++)
	{
		call(%func, %msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10);
	}
	if (%tag !$= "")
	{
		for (%i = 0; (%func = $MSGCB[%tag, %i]) !$= ""; %i++)
		{
			call(%func, %msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10);
		}
	}
}

function addMessageCallback(%msgType, %func)
{
	for (%i = 0; (%afunc = $MSGCB[%msgType, %i]) !$= ""; %i++)
	{
		if (%afunc $= %func)
		{
			return;
		}
	}
	$MSGCB[%msgType, %i] = %func;
}

function defaultMessageCallback(%msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10)
{
	onServerMessage(detag(%msgString));
}

addMessageCallback("", defaultMessageCallback);
