function initCanvas(%windowName)
{
	videoSetGammaCorrection($pref::OpenGL::gammaCorrection);
	if (!createCanvas(%windowName))
	{
		quit();
		return;
	}
	setOpenGLTextureCompressionHint($pref::OpenGL::compressionHint);
	setOpenGLAnisotropy($pref::OpenGL::anisotropy);
	setOpenGLMipReduction($pref::OpenGL::mipReduction);
	setOpenGLInteriorMipReduction($pref::OpenGL::interiorMipReduction);
	setOpenGLSkyMipReduction($pref::OpenGL::skyMipReduction);
	exec("~/ui/defaultProfiles.cs");
	exec("~/ui/customProfiles.cs");
	exec("~/ui/ConsoleDlg.gui");
	exec("~/ui/InspectDlg.gui");
	exec("~/ui/LoadFileDlg.gui");
	exec("~/ui/SaveFileDlg.gui");
	exec("~/ui/MessageBoxOkDlg.gui");
	exec("~/ui/MessageBoxYesNoDlg.gui");
	exec("~/ui/MessageBoxOKCancelDlg.gui");
	exec("~/ui/MessagePopupDlg.gui");
	exec("~/ui/HelpDlg.gui");
	exec("~/ui/RecordingsDlg.gui");
	exec("~/ui/NetGraphGui.gui");
	exec("./metrics.cs");
	exec("./messageBox.cs");
	exec("./screenshot.cs");
	exec("./cursor.cs");
	exec("./help.cs");
	exec("./recordings.cs");
	OpenALInit();
}

function resetCanvas()
{
	if (isObject(Canvas))
	{
		Canvas.repaint();
	}
	if (isObject(PlayGui))
	{
		PlayGui.createInvHud();
		PlayGui.createToolHud();
		PlayGui.LoadPaint();
	}
}

function onWindowReactivate()
{
	if ($Pref::Audio::TurnOffAltTab)
	{
		if (isEventPending($OpenALInitSchedule))
		{
			cancel($OpenALInitSchedule);
		}
		$OpenALInitSchedule = schedule(100, 0, restartAudio);
	}
}

function restartAudio()
{
	OpenALInit();
	if (!isObject(ServerConnection))
	{
		return;
	}
	%group = ServerConnection.getId();
	%count = %group.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		%obj = %group.getObject(%i);
		if (%obj.getClassName() $= "AudioEmitter")
		{
			%profile = %obj.profile;
			%obj.profile = 0;
			%obj.profile = %profile;
			%obj.schedule(10, update);
		}
	}
}

