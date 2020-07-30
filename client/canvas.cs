function initCanvas (%windowName)
{
	videoSetGammaCorrection ($pref::OpenGL::gammaCorrection);
	if (!createCanvas (%windowName))
	{
		quit ();
		return;
	}
	setOpenGLTextureCompressionHint ($pref::OpenGL::compressionHint);
	setOpenGLAnisotropy ($pref::OpenGL::anisotropy);
	setOpenGLMipReduction ($pref::OpenGL::mipReduction);
	setOpenGLSkyMipReduction ($pref::OpenGL::skyMipReduction);
	OpenALInit ();
}

function resetCanvas ()
{
	echo ("Resetting Canvas...");
	if (isObject (PlayGui))
	{
		PlayGui.setHasRendered (0);
	}
}

function onWindowReactivate ()
{
	if ($windowReactivating)
	{
		return;
	}
	echo ("Window reactivating...");
	$windowReactivating = 1;
	if (isFullScreen () && isObject (Canvas))
	{
		if (isObject (PlayGui))
		{
			PlayGui.setHasRendered (0);
		}
		%oldShaderEnabled = $Shader::Enabled;
		$Shader::Enabled = 0;
		Canvas.repaint ();
		flushTextureCache ();
		regenerateShadowMapFBOs ();
		Canvas.repaint ();
		$Shader::Enabled = %oldShaderEnabled;
		if ($Shader::Enabled)
		{
			initializeShaderAssets ();
		}
	}
	$windowReactivating = 0;
}

function restartAudio ()
{
	OpenALInit ();
	if (!isObject (ServerConnection))
	{
		return;
	}
	%group = ServerConnection.getId ();
	%count = %group.getCount ();
	%i = 0;
	while (%i < %count)
	{
		%obj = %group.getObject (%i);
		if (%obj.getClassName () $= "AudioEmitter")
		{
			%profile = %obj.profile;
			%obj.profile = 0;
			%obj.profile = %profile;
			%obj.schedule (10, update);
		}
		%i += 1;
	}
}

