$outerChatLenY[1] = 70;
$outerChatLenY[2] = 140;
$outerChatLenY[3] = 196;
$MaxMessageWavLength = 5000;
function playMessageSound(%message, %voice, %pitch)
{
	%wavStart = strstr(%message, "~w");
	if (%wavStart == -1)
	{
		return -1;
	}
	%wav = getSubStr(%message, %wavStart + 2, 1000);
	if (%voice !$= "")
	{
		%wavFile = "~/data/sound/voice/" @ %voice @ "/" @ %wav;
	}
	else
	{
		%wavFile = "~/data/sound/" @ %wav;
	}
	if (strstr(%wavFile, ".wav") != strlen(%wavFile) - 4)
	{
		%wavFile = %wavFile @ ".wav";
	}
	%wavFile = ExpandFilename(%wavFile);
	if (%pitch < 0.5 || %pitch > 2)
	{
		%pitch = 1;
	}
	%wavLengthMS = alxGetWaveLen(%wavFile) * %pitch;
	if (%wavLengthMS == 0)
	{
		error("** WAV file \"" @ %wavFile @ "\" is nonexistent or sound is zero-length **");
	}
	else if (%wavLengthMS <= $MaxMessageWavLength)
	{
		if ($ClientChatHandle[%sender] != 0)
		{
			alxStop($ClientChatHandle[%sender]);
		}
		$ClientChatHandle[%sender] = alxCreateSource(AudioMessage, %wavFile);
		if (%pitch != 1)
		{
			alxSourcef($ClientChatHandle[%sender], "AL_PITCH", %pitch);
		}
		alxPlay($ClientChatHandle[%sender]);
	}
	else
	{
		error("** WAV file \"" @ %wavFile @ "\" is too long **");
	}
	return %wavStart;
}

new MessageVector(HudMessageVector);
$LastHudTarget = 0;
function onChatMessage(%message, %voice, %pitch)
{
	if (%voice $= "")
	{
		%voice = "default";
	}
	if ((%wavStart = playMessageSound(%message, %voice, %pitch)) != -1)
	{
		%message = getSubStr(%message, 0, %wavStart);
	}
	if (getWordCount(%message))
	{
		ChatHud.addLine(%message);
		newChatHud_AddLine(%message);
	}
}

function onServerMessage(%message)
{
	if ((%wavStart = playMessageSound(%message)) != -1)
	{
		%message = getSubStr(%message, 0, %wavStart);
	}
	if (getWordCount(%message))
	{
		ChatHud.addLine(%message);
		newChatHud_AddLine(%message);
	}
}

function MainChatHud::onWake(%this)
{
	%this.setChatHudLength($pref::ChatHudLength);
}

function MainChatHud::setChatHudLength(%this, %length)
{
	%outerChatLenX = firstWord(OuterChatHud.extent);
	%chatScrollLenX = firstWord(ChatScrollHud.extent);
	OuterChatHud.extent = %outerChatLenX SPC $outerChatLenY[%length];
	ChatScrollHud.extent = %chatScrollLenX SPC $outerChatLenY[%length];
	%chatScrollHeight = getWord(ChatHud.getGroup().getGroup().extent, 1);
	if (%chatScrollHeight <= 0)
	{
		return;
	}
	%textHeight = ChatHud.profile.fontSize;
	if (%textHeight <= 0)
	{
		%textHeight = 12;
	}
	%pageLines = mFloor(%chatScrollHeight / %textHeight);
	if (%pageLines <= 0)
	{
		%pageLines = 1;
	}
	%pos = HudMessageVector.getNumLines() - %pageLines;
	if (%pos < 0)
	{
		%pos = 0;
	}
	ChatHud.position = "0 0";
	ChatHudShadow.position = getWord(ChatHud.position, 0) + 1 SPC getWord(ChatHud.position, 1) + 1;
	ChatHud.resize(0, 0, getWord(ChatHud.getExtent(), 0), getWord(ChatHud.getExtent(), 1));
	ChatHudShadow.resize(0, 0, getWord(ChatHudShadow.extent, 0), getWord(ChatHudShadow.extent, 1));
	echo("--chat hud position =" SPC ChatHud.getPosition());
	chatPageDown.position = firstWord(OuterChatHud.extent) - firstWord(chatPageDown.extent) SPC $outerChatLenY[%length] - getWord(chatPageDown.extent, 1);
	ChatWhosTalkingBox.resize(5, getWord(OuterChatHud.position, 1) + $outerChatLenY[%length], getWord(OuterChatHud.extent, 0), 18);
	chatPageDown.setVisible(0);
}

function MainChatHud::nextChatHudLen(%this)
{
	%len = $pref::ChatHudLength++;
	if ($pref::ChatHudLength == 4)
	{
		$pref::ChatHudLength = 1;
	}
	%this.setChatHudLength($pref::ChatHudLength);
}

function ChatHud::addLine(%this, %text)
{
	%textHeight = %this.profile.fontSize;
	if (%textHeight <= 0)
	{
		%textHeight = 12;
	}
	%chatScrollHeight = getWord(%this.getGroup().getGroup().extent, 1);
	%chatPosition = (getWord(%this.extent, 1) - %chatScrollHeight) + getWord(%this.position, 1);
	%linesToScroll = mFloor(%chatPosition / %textHeight + 0.5);
	if (%linesToScroll > 0)
	{
		%origPosition = %this.position;
	}
	while (!chatPageDown.isVisible() && HudMessageVector.getNumLines() && HudMessageVector.getNumLines() >= $pref::HudMessageLogSize)
	{
		%tag = HudMessageVector.getLineTag(0);
		if (%tag != 0)
		{
			%tag.delete();
		}
		HudMessageVector.popFrontLine();
	}
	HudMessageVector.pushBackLine(%text, $LastHudTarget);
	$LastHudTarget = 0;
	if (%linesToScroll > 0)
	{
		chatPageDown.setVisible(1);
		%this.position = %origPosition;
	}
	else
	{
		chatPageDown.setVisible(0);
	}
}

function ChatHud::pageUp(%this)
{
	%textHeight = %this.profile.fontSize;
	if (%textHeight <= 0)
	{
		%textHeight = 12;
	}
	%chatScrollHeight = getWord(%this.getGroup().getGroup().extent, 1);
	if (%chatScrollHeight <= 0)
	{
		return;
	}
	%pageLines = mFloor(%chatScrollHeight / %textHeight) - 1;
	if (%pageLines <= 0)
	{
		%pageLines = 1;
	}
	%chatPosition = -1 * getWord(%this.position, 1);
	%linesToScroll = mFloor(%chatPosition / %textHeight + 0.5);
	if (%linesToScroll <= 0)
	{
		return;
	}
	if (%linesToScroll > %pageLines)
	{
		%scrollLines = %pageLines;
	}
	else
	{
		%scrollLines = %linesToScroll;
	}
	%this.position = firstWord(%this.position) SPC getWord(%this.position, 1) + %scrollLines * %textHeight;
	chatPageDown.setVisible(1);
}

function ChatHudShadow::pageUp(%this)
{
	ChatHud::pageUp(%this);
}

function ChatHud::pageDown(%this)
{
	%textHeight = %this.profile.fontSize;
	if (%textHeight <= 0)
	{
		%textHeight = 12;
	}
	%chatScrollHeight = getWord(%this.getGroup().getGroup().extent, 1);
	if (%chatScrollHeight <= 0)
	{
		return;
	}
	%pageLines = mFloor(%chatScrollHeight / %textHeight) - 1;
	if (%pageLines <= 0)
	{
		%pageLines = 1;
	}
	%chatPosition = (getWord(%this.extent, 1) - %chatScrollHeight) + getWord(%this.position, 1);
	%linesToScroll = mFloor(%chatPosition / %textHeight + 0.5);
	if (%linesToScroll <= 0)
	{
		return;
	}
	if (%linesToScroll > %pageLines)
	{
		%scrollLines = %pageLines;
	}
	else
	{
		%scrollLines = %linesToScroll;
	}
	%this.position = firstWord(%this.position) SPC getWord(%this.position, 1) - %scrollLines * %textHeight;
	if (%scrollLines < %linesToScroll)
	{
		chatPageDown.setVisible(1);
	}
	else
	{
		chatPageDown.setVisible(0);
	}
}

function ChatHudShadow::pageDown(%this)
{
	ChatHud::pageDown(%this);
}

function pageUpMessageHud()
{
	ChatHud.pageUp();
	ChatHudShadow.pageUp();
	$NewChatSO.pageUp();
}

function pageDownMessageHud()
{
	ChatHud.pageDown();
	ChatHudShadow.pageDown();
	$NewChatSO.pageDown();
}

function cycleMessageHudSize()
{
	MainChatHud.nextChatHudLen();
}

