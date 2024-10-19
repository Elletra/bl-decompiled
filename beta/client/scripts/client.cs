function SAD(%password)
{
	if (%password !$= "")
	{
		commandToServer('SAD', %password);
	}
}

function SADSetPassword(%password)
{
	commandToServer('SADSetPassword', %password);
}

function buildwall()
{
	for (%j = 0; %j < 16; %j++)
	{
		for (%i = 0; %i < 10; %i++)
		{
			commandToServer('plantBrick');
			commandToServer('shiftBrick', 0, 0, 3);
			commandToServer('plantBrick');
			commandToServer('shiftBrick', 0, 0, 3);
			commandToServer('plantBrick');
			commandToServer('shiftBrick', 0, 0, 3);
			commandToServer('plantBrick');
			commandToServer('shiftBrick', 0, 0, 3);
			commandToServer('plantBrick');
			commandToServer('shiftBrick', 0, 0, 3);
			commandToServer('plantBrick');
			commandToServer('shiftBrick', 0, 0, 3);
			commandToServer('plantBrick');
			commandToServer('shiftBrick', 0, 0, 3);
			commandToServer('plantBrick');
			commandToServer('shiftBrick', 0, 0, 3);
			commandToServer('plantBrick');
			commandToServer('shiftBrick', 0, 0, 3);
			commandToServer('plantBrick');
			commandToServer('shiftBrick', 0, 0, 3);
			commandToServer('shiftBrick', 2, 0, -30);
		}
		commandToServer('shiftBrick', -20, 2, 0);
	}
}

function stress()
{
	for (%j = 0; %j < 16; %j++)
	{
		for (%i = 0; %i < 10; %i++)
		{
			for (%k = 0; %k < 30; %k++)
			{
				commandToServer('plantBrick');
				commandToServer('shiftBrick', 0, 0, 3);
			}
			commandToServer('shiftBrick', 2, 0, %k * -3);
		}
		commandToServer('shiftBrick', -20, 2, 0);
	}
}

function buildstairs()
{
	for (%i = 0; %i < 30; %i++)
	{
		commandToServer('plantBrick');
		commandToServer('shiftBrick', 1, 0, 3);
	}
}

function clientCmdSyncClock(%time)
{
	HudClock.setTime(%time);
}

function GameConnection::prepDemoRecord(%this)
{
	%this.demoChatLines = HudMessageVector.getNumLines();
	for (%i = 0; %i < %this.demoChatLines; %i++)
	{
		%this.demoChatText[%i] = HudMessageVector.getLineText(%i);
		%this.demoChatTag[%i] = HudMessageVector.getLineTag(%i);
		echo("Chat line " @ %i @ ": " @ %this.demoChatText[%i]);
	}
}

function GameConnection::prepDemoPlayback(%this)
{
	for (%i = 0; %i < %this.demoChatLines; %i++)
	{
		HudMessageVector.pushBackLine(%this.demoChatText[%i], %this.demoChatTag[%i]);
	}
	Canvas.setContent(PlayGui);
}

function getWords(%phrase, %start, %end)
{
	if (%start > %end)
	{
		return;
	}
	%returnPhrase = getWord(%phrase, %start);
	if (%start == %end)
	{
		return %returnPhrase;
	}
	for (%i = %start + 1; %i <= %end; %i++)
	{
		%returnPhrase = %returnPhrase @ " " @ getWord(%phrase, %i);
	}
	return %returnPhrase;
}

function getLine(%phrase, %lineNum)
{
	%offset = 0;
	%lineCount = 0;
	while (%lineCount <= %lineNum)
	{
		%pos = strpos(%phrase, "\n", %offset);
		if (%pos >= 0)
		{
			%len = %pos - %offset;
		}
		else
		{
			%len = 99999;
		}
		%line = getSubStr(%phrase, %offset, %len);
		if (%lineCount == %lineNum)
		{
			return %line;
		}
		%lineCount++;
		%offset = %pos + 1;
		if (%pos == -1)
		{
			return "";
		}
	}
	return "";
}

function getLineCount(%phrase)
{
	%offset = 0;
	for (%lineCount = 0; %offset >= 0; %lineCount++)
	{
		%offset = strpos(%phrase, "\n", %offset + 1);
	}
	return %lineCount;
}

