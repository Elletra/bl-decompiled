function newMessageHud::onWake(%this)
{
}

function newMessageHud::onSleep(%this)
{
	commandToServer('stopTalking');
}

function newMessageHud::open(%this, %channel)
{
	%this.channel = %channel;
	if (%channel $= "SAY")
	{
		NMH_Channel.setText("\c0" @ %channel @ ":");
	}
	else if (%channel $= "TEAM")
	{
		NMH_Channel.setText("\c1" @ %channel @ ":");
	}
	else
	{
		error("ERROR(): newMessageHud::open() - unknown channel \"" @ %channel @ "\"");
		return;
	}
	if (!%this.isAwake())
	{
		NMH_Type.setValue("");
		Canvas.pushDialog(%this);
		%this.updatePosition();
		%this.schedule(10, updateTypePosition);
	}
}

function newMessageHud::updateTypePosition(%this)
{
	%x = getWord(NMH_Channel.getPosition(), 0);
	%x += NMH_Channel.getPixelWidth() + 2;
	%y = 0;
	%w = (getWord(NMH_Box.getExtent(), 0) - %x) - 2;
	%h = 18;
	NMH_Type.resize(%x, %y, %w, %h);
}

function newMessageHud::updatePosition(%this)
{
	if (!%this.isAwake())
	{
		return;
	}
	%x = getWord(NMH_Box.getPosition(), 0);
	%y = getWord(newChatText.getPosition(), 1);
	%y += getWord(newChatText.getExtent(), 1);
	%w = getWord(NMH_Box.getExtent(), 0);
	%h = getWord(NMH_Box.getExtent(), 1);
	NMH_Box.resize(%x, %y, %w, %h);
}

function NMH_Type::type(%this)
{
	%text = %this.getValue();
	if (strlen(%text) == 1)
	{
		if (%text !$= "/")
		{
			commandToServer('StartTalking');
		}
	}
}

function NMH_Type::send(%this)
{
	%text = %this.getValue();
	%firstChar = getSubStr(%text, 0, 1);
	if (%firstChar $= "/")
	{
		%newText = getSubStr(%text, 1, 256);
		%command = getWord(%newText, 0);
		%par1 = getWord(%newText, 1);
		%par2 = getWord(%newText, 2);
		%par3 = getWord(%newText, 3);
		%par4 = getWord(%newText, 4);
		%par5 = getWord(%newText, 4);
		%par6 = getWord(%newText, 4);
		commandToServer(addTaggedString(%command), %par1, %par2, %par3, %par4, %par5, %par6);
	}
	else if (newMessageHud.channel $= "SAY")
	{
		commandToServer('messageSent', %text);
	}
	else if (newMessageHud.channel $= "TEAM")
	{
		commandToServer('teamMessageSent', %text);
	}
	else
	{
		error("ERROR: NMH_Type::Send() - unknown channel \"" @ newMessageHuf.channel @ "\"");
	}
	Canvas.popDialog(newMessageHud);
}

