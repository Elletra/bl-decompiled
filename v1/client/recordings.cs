function recordingsDlg::onWake()
{
	RecordingsDlgList.clear();
	%i = 0;
	if ($currentMod $= "editor" || $currentMod $= "")
	{
		%mod = "base";
	}
	else
	{
		%mod = $currentMod;
	}
	%filespec = %mod @ "/recordings/*.rec";
	echo(%filespec);
	for (%file = findFirstFile(%filespec); %file !$= ""; %file = findNextFile(%filespec))
	{
		%fileName = fileBase(%file);
		if (strstr(%file, "/CVS/") == -1)
		{
			RecordingsDlgList.addRow(%i++, %fileName);
		}
	}
	RecordingsDlgList.sort(0);
	RecordingsDlgList.setSelectedRow(0);
	RecordingsDlgList.scrollVisible(0);
}

function StartSelectedDemo()
{
	%sel = RecordingsDlgList.getSelectedId();
	%rowText = RecordingsDlgList.getRowTextById(%sel);
	if ($currentMod $= "editor" || $currentMod $= "")
	{
		%mod = "base";
	}
	else
	{
		%mod = $currentMod;
	}
	%file = %mod @ "/recordings/" @ getField(%rowText, 0) @ ".rec";
	new GameConnection(ServerConnection);
	RootGroup.add(ServerConnection);
	if (ServerConnection.playDemo(%file))
	{
		Canvas.popDialog(recordingsDlg);
		ServerConnection.prepDemoPlayback();
	}
	else
	{
		MessageBoxOK("Playback Failed", "Demo playback failed for file '" @ %file @ "'.");
		if (isObject(ServerConnection))
		{
			ServerConnection.delete();
		}
	}
}

function startDemoRecord()
{
	ServerConnection.stopRecording();
	if (ServerConnection.isDemoPlaying())
	{
		return;
	}
	if ($currentMod $= "editor" || $currentMod $= "")
	{
		%mod = "base";
	}
	else
	{
		%mod = $currentMod;
	}
	for (%i = 0; %i < 1000; %i++)
	{
		%num = %i;
		if (%num < 10)
		{
			%num = "0" @ %num;
		}
		if (%num < 100)
		{
			%num = "0" @ %num;
		}
		%file = %mod @ "/recordings/demo" @ %num @ ".rec";
		if (!isFile(%file))
		{
			break;
		}
	}
	if (%i == 1000)
	{
		return;
	}
	$DemoFileName = %file;
	newChatHud_AddLine("\c4Recording to file [\c2" @ $DemoFileName @ "\c4]");
	ServerConnection.prepDemoRecord();
	ServerConnection.startRecording($DemoFileName);
	if (!ServerConnection.isDemoRecording())
	{
		deleteFile($DemoFileName);
		newChatHud_AddLine("\c4Recording to file [\c2" @ $DemoFileName @ "\c4]");
		$DemoFileName = "";
	}
}

function stopDemoRecord()
{
	if (ServerConnection.isDemoRecording())
	{
		newChatHud_AddLine("\c4Recording file [\c2" @ $DemoFileName @ "\c4] finished");
		ServerConnection.stopRecording();
	}
}

function demoPlaybackComplete()
{
	disconnect();
	Canvas.setContent("MainMenuGui");
	Canvas.pushDialog(recordingsDlg);
}

