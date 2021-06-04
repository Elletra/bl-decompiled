// -----------------------------------------------------------------------------
//  Torque Game Engine
//  Copyright (C) GarageGames.com, Inc.
// -----------------------------------------------------------------------------

// -----------------------------------------------------------------------------
//  Functions to construct and initialize the default canvas window used by
//  the games.
// -----------------------------------------------------------------------------


function initCanvas ( %windowName )
{
	videoSetGammaCorrection ($pref::OpenGL::gammaCorrection);

	if ( !createCanvas (%windowName) )
	{
		quit ();
		return;
	}

	setOpenGLTextureCompressionHint ($pref::OpenGL::compressionHint);
	setOpenGLAnisotropy ($pref::OpenGL::anisotropy);
	setOpenGLMipReduction ($pref::OpenGL::mipReduction);
	setOpenGLSkyMipReduction ($pref::OpenGL::skyMipReduction);

	// Initialize the audio system.
	OpenALInit ();
}

function resetCanvas ()
{
	echo ("Resetting Canvas...");

	if ( isObject (PlayGui) )
	{
		PlayGui.setHasRendered (false);
	}
}

// Called when the game window comes back into focus.
function onWindowReactivate ()
{
	if ( $windowReactivating )
	{
		return;
	}

	echo ("Window reactivating...");

	$windowReactivating = true;

	if ( isFullScreen () && isObject (Canvas) )
	{
		if ( isObject (PlayGui) )
		{
			PlayGui.setHasRendered (false);
		}

		%oldShaderEnabled = $Shader::Enabled;
		$Shader::Enabled = false;

		Canvas.repaint ();

		flushTextureCache ();
		regenerateShadowMapFBOs ();

		Canvas.repaint ();

		$Shader::Enabled = %oldShaderEnabled;

		if ( $Shader::Enabled )
		{
			initializeShaderAssets ();
		}
	}

	$windowReactivating = false;
}

// Doesn't appear to be called by either the engine or other game scripts.  Unknown purpose.
function restartAudio ()
{
	OpenALInit ();

	if ( !isObject (ServerConnection) )
	{
		return;
	}

	%group = ServerConnection.getId ();
	%count = %group.getCount ();

	for ( %i = 0; %i < %count; %i++ )
	{
		%obj = %group.getObject (%i);

		if ( %obj.getClassName () $= "AudioEmitter" )
		{
			%profile = %obj.profile;

			%obj.profile = 0;
			%obj.profile = %profile;

			%obj.schedule (10, update);
		}
	}
}
