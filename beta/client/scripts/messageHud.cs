function MessageHud::open(%this)
{
	%offset = 0;
	if (%this.isVisible())
	{
		return;
	}
	if (%this.isTeamMsg)
	{
		%text = "TEAM:";
	}
	else
	{
		%text = "CHAT:";
	}
	MessageHud_Text.setValue(%text);
	%windowPos = "5 " @ getWord(OuterChatHud.position, 1) + getWord(OuterChatHud.extent, 1) + 18 + 1;
	%windowExt = getWord(OuterChatHud.extent, 0) @ " " @ getWord(MessageHud_Frame.extent, 1);
	%textExtent = getWord(MessageHud_TextSwatch.extent, 0);
	%ctrlExtent = getWord(MessageHud_Frame.extent, 0);
	Canvas.pushDialog(%this);
	MessageHud_Frame.position = %windowPos;
	MessageHud_Frame.extent = %windowExt;
	MessageHud_Edit.position = "0 0";
	MessageHud_Edit.extent = setWord(MessageHud_Edit.extent, 0, (%ctrlExtent - %textExtent) - 2 * %offset);
	%this.setVisible(1);
	deactivateKeyboard();
	MessageHud_Edit.makeFirstResponder(1);
	commandToServer('StartTalking');
}

function MessageHud::close(%this)
{
	if (!%this.isVisible())
	{
		return;
	}
	Canvas.popDialog(%this);
	%this.setVisible(0);
	if ($enableDirectInput)
	{
		activateKeyboard();
	}
	MessageHud_Edit.setValue("");
	commandToServer('StopTalking');
}

function MessageHud::toggleState(%this)
{
	%this.open();
}

function MessageHud_Edit::onEscape(%this)
{
	MessageHud.close();
}

function MessageHud_Edit::eval(%this)
{
	%text = trim(%this.getValue());
	if (%text !$= "")
	{
		%firstChar = getSubStr(%text, 0, 1);
		if (%firstChar $= "/")
		{
			%newText = getSubStr(%text, 1, 256);
			%command = getWord(%newText, 0);
			%par1 = getWord(%newText, 1);
			%par2 = getWord(%newText, 2);
			%par3 = getWord(%newText, 3);
			%par4 = getWord(%newText, 4);
			commandToServer(addTaggedString(%command), %par1, %par2, %par3, %par4);
		}
		else if (MessageHud.isTeamMsg)
		{
			commandToServer('teamMessageSent', %text);
		}
		else
		{
			commandToServer('messageSent', %text);
		}
	}
	MessageHud.close();
}

function toggleMessageHud(%make)
{
	if (%make)
	{
		MessageHud.isTeamMsg = 0;
		MessageHud.toggleState();
	}
}

function teamMessageHud(%make)
{
	if (%make)
	{
		MessageHud.isTeamMsg = 1;
		MessageHud.toggleState();
	}
}

