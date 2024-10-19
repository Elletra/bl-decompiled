function clientCmdDoUpdates()
{
	schedule(10, 0, disconnect);
	$AU_AutoClose = 0;
	Canvas.schedule(1000, pushDialog, AutoUpdateGui);
}

function AutoUpdateGui::onWake(%this)
{
	AU_Text.setText("");
	AU_Time.setText("00:00:00");
	AU_Data.setText("0 KB / 0 KB");
	AU_Speed.setText("0 B/s");
	AU_Progress.setValue(0);
	AU_Done.setVisible(0);
	if (Canvas.getContent().getName() !$= "GuiEditorGui")
	{
		AU_start();
	}
}

function AutoUpdateGui::onSleep(%this)
{
	if (isObject(verTCPobj))
	{
		verTCPobj.disconnect();
	}
	if (isObject(patchTCPobj))
	{
		patchTCPobj.disconnect();
	}
	$AU_AutoClose = 1;
}

function AutoUpdateGui::done(%this)
{
	if ($AU_AutoClose == 1)
	{
		Canvas.popDialog(AutoUpdateGui);
	}
	else
	{
		AU_Done.setVisible(1);
	}
}

function AU_Text::echo(%this, %text)
{
	%this.setText(%this.getValue() @ "\n" @ %text);
}

function AU_start()
{
	AU_Text.setText("Starting...");
	if (isObject(verTCPobj))
	{
		verTCPobj.delete();
	}
	new TCPObject(verTCPobj);
	verTCPobj.site = "master.blockland.us";
	verTCPobj.port = 80;
	verTCPobj.filePath = "/updates/" @ $Version @ ".txt";
	verTCPobj.lastLine = "crap";
	verTCPobj.connect(verTCPobj.site @ ":" @ verTCPobj.port);
}

function verTCPobj::onConnected(%this)
{
	AU_Text.echo("Getting version info...");
	%this.send("GET " @ %this.filePath @ " HTTP/1.1\r\nHost: " @ %this.site @ "\r\n\r\n");
}

function verTCPobj::onDNSFailed(%this)
{
	AU_Text.echo("DNS Failed");
	AutoUpdateGui.done();
}

function verTCPobj::onConnectFailed(%this)
{
	AU_Text.echo("Connection Failed");
	AutoUpdateGui.done();
}

function verTCPobj::onLine(%this, %line)
{
	if (strstr(%line, "404 Not Found") != -1)
	{
		AU_Text.echo("Update information not found.");
		%this.done = 1;
		AutoUpdateGui.done();
	}
	if (%this.lastLine $= "" && %this.done == 0)
	{
		if (%line !$= "")
		{
			AU_Text.echo("Update required");
			%this.done = 1;
			AU_GetUpdate(%line);
		}
		else
		{
			AU_Text.echo("You have the current version.");
			%this.done = 1;
			AutoUpdateGui.done();
		}
	}
	%this.lastLine = %line;
}

function AU_GetUpdate(%newVersion)
{
	%fileName = "/updates/" @ %newVersion;
	%saveName = "patches/" @ %newVersion;
	if (isObject(patchTCPobj))
	{
		patchTCPobj.delete();
	}
	new TCPObject(patchTCPobj);
	patchTCPobj.site = "master.blockland.us";
	patchTCPobj.port = 80;
	patchTCPobj.filePath = %fileName;
	patchTCPobj.saveTo = %saveName;
	patchTCPobj.lastLine = "crap";
	patchTCPobj.connect(patchTCPobj.site @ ":" @ patchTCPobj.port);
	%file = new FileObject();
	%file.openForWrite("patches/patch.bat");
	%file.writeLine(%newVersion);
	%file.writeLine("cd ..");
	%file.writeLine("Blockland.exe");
	%file.close();
	%file.delete();
}

function patchTCPobj::onConnected(%this)
{
	AU_Text.echo("Requesting update: " @ fileBase(%this.filePath));
	%this.send("GET " @ %this.filePath @ " HTTP/1.1\r\nHost: " @ %this.site @ "\r\n\r\n");
}

function patchTCPobj::onDNSFailed(%this)
{
	AU_Text.echo("DNS Failed");
	AutoUpdateGui.done();
}

function patchTCPobj::onConnectFailed(%this)
{
	AU_Text.echo("Connection Failed");
	AutoUpdateGui.done();
}

function patchTCPobj::onLine(%this, %line)
{
	%word = getWord(%line, 0);
	if (%word $= "Content-Length:")
	{
		%fileSize = getWord(%line, 1);
		%this.fileSize = %fileSize;
	}
	if (%line $= "")
	{
		%this.setBinary(1);
	}
}

function roundMegs(%val)
{
	%val *= 100;
	%val = mFloor(%val);
	%val /= 100;
	%frac = %val - mFloor(%val);
	if (strlen(%frac) < 2)
	{
		%val = %val @ "0";
	}
	return %val;
}

function getTimeString(%timeS)
{
	if (%timeS >= 3600)
	{
		%hours = mFloor(%timeS / 3600);
		%timeS -= %hours * 3600;
		%minutes = mFloor(%timeS / 60);
		%timeS -= %minutes * 60;
		%seconds = %timeS;
		if (%minutes < 10)
		{
			%minutes = "0" @ %minutes;
		}
		if (%seconds < 10)
		{
			%seconds = "0" @ %seconds;
		}
		return %hours @ ":" @ %minutes @ ":" @ %seconds;
	}
	else if (%timeS >= 60)
	{
		%minutes = mFloor(%timeS / 60);
		%timeS -= %minutes * 60;
		%seconds = %timeS;
		if (%seconds < 10)
		{
			%seconds = "0" @ %seconds;
		}
		return %minutes @ ":" @ %seconds;
	}
	else
	{
		%seconds = %timeS;
		if (%seconds < 10)
		{
			%seconds = "0" @ %seconds;
		}
		return "0:" @ %seconds;
	}
}

function patchTCPobj::onBinChunk(%this, %buffSize)
{
	AU_Progress.setValue(%buffSize / %this.fileSize);
	if (%this.fileSize >= 1048576)
	{
		%buffMegs = roundMegs(%buffSize / 1048576);
		%fileMegs = roundMegs(%this.fileSize / 1048576);
		AU_Data.setText(%buffMegs @ "MB / " @ %fileMegs @ "MB");
	}
	else
	{
		%buffK = roundMegs(%buffSize / 1024);
		%fileK = roundMegs(%this.fileSize / 1024);
		AU_Data.setText(%buffK @ "KB / " @ %fileK @ "KB");
	}
	%rawSpeed = 1;
	if (%this.startTime $= "")
	{
		AU_Text.echo("Recieving update...");
		AU_Text.echo("When the download is finished, the update will automatically be installed.");
		%this.startTime = getSimTime();
	}
	else
	{
		%elapsedTime = getSimTime() - %this.startTime;
		%elapsedTime = %elapsedTime / 1000;
		%rawSpeed = %buffSize / %elapsedTime;
		if (%rawSpeed >= 1048576)
		{
			%speed = roundMegs(%rawSpeed / 1048576);
			AU_Speed.setText(%speed @ " MB/s");
		}
		else if (%rawSpeed >= 1024)
		{
			%speed = roundMegs(%rawSpeed / 1024);
			AU_Speed.setText(%speed @ " KB/s");
		}
		else
		{
			AU_Speed.setText(%rawSpeed @ " B/s");
		}
	}
	%fileLeft = %this.fileSize - %buffSize;
	%timeLeftS = %fileLeft / %rawSpeed;
	AU_Time.setText(getTimeString(mFloor(%timeLeftS)));
	if (%buffSize >= %this.fileSize)
	{
		%this.disconnect();
		%this.saveBufferToFile(%this.saveTo);
		schedule(100, 0, AU_RunPatch);
	}
}

function AU_RunPatch()
{
	WinExec("doPatch.bat");
}

