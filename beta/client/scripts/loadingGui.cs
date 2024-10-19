function LoadingGui::onAdd(%this)
{
	%this.qLineCount = 0;
}

function LoadingGui::onWake(%this)
{
	CloseMessagePopup();
	moveMap.push();
	Canvas.pushDialog(NewChatHud);
}

function LoadingGui::onSleep(%this)
{
	if (%this.qLineCount !$= "")
	{
		for (%line = 0; %line < %this.qLineCount; %line++)
		{
			%this.qLine[%line] = "";
		}
	}
	%this.qLineCount = 0;
	LOAD_MapName.setText("");
	LOAD_MapDescription.setText("");
	LoadingProgress.setValue(0);
	LoadingProgressTxt.setValue("WAITING FOR SERVER");
	Canvas.popDialog(NewChatHud);
}

