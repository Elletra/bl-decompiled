function onMissionDownloadPhase1(%__unused, %__unused)
{
	MessageHud.close();
	LoadingProgress.setValue(0);
	LoadingProgressTxt.setValue("LOADING DATABLOCKS");
}

function onPhase1Progress(%progress)
{
	LoadingProgress.setValue(%progress);
	Canvas.repaint();
}

function onPhase1Complete()
{
}

function onMissionDownloadPhase2()
{
	LoadingProgress.setValue(0);
	LoadingProgressTxt.setValue("LOADING OBJECTS");
	Canvas.repaint();
}

function onPhase2Progress(%progress)
{
	LoadingProgress.setValue(%progress);
	Canvas.repaint();
}

function onPhase2Complete()
{
}

function onFileChunkReceived(%fileName, %ofs, %size)
{
	LoadingProgress.setValue(%ofs / %size);
	LoadingProgressTxt.setValue("Downloading " @ %fileName @ "...");
}

function onMissionDownloadPhase3()
{
	LoadingProgress.setValue(0);
	LoadingProgressTxt.setValue("LIGHTING MISSION (This only happens once)");
	Canvas.repaint();
}

function onPhase3Progress(%progress)
{
	LoadingProgress.setValue(%progress);
}

function onPhase3Complete()
{
	LoadingProgress.setValue(1);
	$lightingMission = 0;
}

function onMissionDownloadComplete()
{
}

addMessageCallback('MsgLoadInfo', handleLoadInfoMessage);
addMessageCallback('MsgLoadDescripition', handleLoadDescriptionMessage);
addMessageCallback('MsgLoadMapPicture', handleLoadMapPictureMessage);
addMessageCallback('MsgLoadInfoDone', handleLoadInfoDoneMessage);
function handleLoadInfoMessage(%__unused, %__unused, %mapName, %mapSaveName)
{
	Canvas.setContent("LoadingGui");
	for (%line = 0; %line < LoadingGui.qLineCount; %line++)
	{
		LoadingGui.qLine[%line] = "";
	}
	LoadingGui.qLineCount = 0;
	LOAD_MapName.setText(%mapName);
	$MapSaveName = %mapSaveName;
}

function handleLoadDescriptionMessage(%__unused, %__unused, %line)
{
	%text = LOAD_MapDescription.getText();
	if (%text !$= "")
	{
		LOAD_MapDescription.setText(%text @ "\n" @ %line);
	}
	else
	{
		LOAD_MapDescription.setText(%line);
	}
	return;
	LoadingGui.qLine[LoadingGui.qLineCount] = %line;
	LoadingGui.qLineCount++;
	%text = "<spush><font:Arial:16>";
	for (%line = 0; %line < LoadingGui.qLineCount - 1; %line++)
	{
		%text = %text @ LoadingGui.qLine[%line] @ " ";
	}
	%text = %text @ LoadingGui.qLine[%line] @ "<spop>";
	LOAD_MapDescription.setText(%text);
}

function handleLoadMapPictureMessage(%__unused, %__unused, %imageName)
{
	LOAD_MapPicture.setBitmap(%imageName);
}

function handleLoadInfoDoneMessage(%__unused, %__unused)
{
}

