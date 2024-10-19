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

