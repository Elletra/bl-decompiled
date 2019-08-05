function MM_AuthBar::getExtendedPostString ()
{
	%postText = "";
	%postText = %postText @ "&DEDICATED=" @ $Server::Dedicated;
	%postText = %postText @ "&PORT=" @ $Pref::Server::Port;
	%postText = %postText @ "&VER=" @ $Version;
	%postText = %postText @ "&BUILD=" @ getBuildNumber ();

	if ( $pref::client::lastUpnpError != 0 )
	{
		%postText = %postText @ "&UPNPERROR=" @ $pref::client::lastUpnpError;
	}

	%postText = %postText @ "&RAM=" @ mFloor (getTotalRam ());
	%postText = %postText @ "&DIR=" @ getModuleDirectory ();
	%postText = %postText @ "&OSSHORT=" @ getOSShort ();
	%postText = %postText @ "&OSLONG=" @ getOSLong ();

	%cpuName = getCPUName ();
	%cpuName = strreplace (%cpuName, " ", "_");

	%postText = %postText @ "&CPU=" @ %cpuName;
	%postText = %postText @ "&MHZ=" @ mFloor (getCPUMhz ());
	%postText = %postText @ "&U=" @ getUUID ("liz!feeamn0sivor");
	%postText = %postText @ "&NETTYPE=" @ mFloor ($Pref::Net::ConnectionType);

	if ( $Server::Dedicated )
	{
		%postText = %postText @ "&GPUMAN=None";
		%postText = %postText @ "&GPU=None";
	}
	else
	{
		%glVendor = getGLVendor ();

		if ( %glVendor $= "" )
		{
			%glVendor = "Unknown";
		}

		%glRenderer = getGLRenderer ();

		if ( %glRenderer $= "" )
		{
			%glRenderer = "Unknown";
		}

		%glVendor   = strreplace (%glVendor, " ", "_");
		%glRenderer = strreplace (%glRenderer, "/SSE2", "");
		%glRenderer = strreplace (%glRenderer, "/SSE", "");
		%glRenderer = strreplace (%glRenderer, "/PCI", "");
		%glRenderer = strreplace (%glRenderer, "/3DNOW!", "");
		%glRenderer = strreplace (%glRenderer, "_", " ");
		%glRenderer = strreplace (%glRenderer, "  ", " ");

		trim (%glRenderer);

		%glRenderer = strreplace (%glRenderer, " ", "_");
		%glRenderer = strreplace (%glRenderer, "/", ".");

		%postText = %postText @ "&GPUMAN=" @ %glVendor;
		%slashPos = strpos (%glRenderer, "/");

		if ( %slashPos > 0 )
		{
			%renderer = getSubStr (%glRenderer, 0, %slashPos);
		}

		%postText = %postText @ "&GPU=" @ %glRenderer;
	}

	if ( $sendOGLExt == 1 )
	{
		%glVersion = getField (getVideoDriverInfo(), 2);
		%glExtList = getField (getVideoDriverInfo(), 3);
		%glExtList = strreplace (%glExtList, " ", "^");

		%postText = %postText @ "&GLVersion=" @ %glVersion;
		%postText = %postText @ "&GLExtList=" @ %glExtList;

		$sendOGLExt = 0;
	}

	%postText = %postText @ "&GLVersion=" @ getGLVersion ();
	%postText = %postText @ "&GLEW_ARB_shader_objects=" @ GLEW_ARB_shader_objects ();
	%postText = %postText @ "&GLEW_ARB_shading_language_100=" @ GLEW_ARB_shading_language_100 ();
	%postText = %postText @ "&GLEW_EXT_texture_array=" @ GLEW_EXT_texture_array ();
	%postText = %postText @ "&GLEW_EXT_texture3D=" @ GLEW_EXT_texture3D ();
	%postText = %postText @ "&glTexImage3D=" @ glTexImage3D ();
	%postText = %postText @ "&GLEW_EXT_framebuffer_object=" @ GLEW_EXT_framebuffer_object ();
	%postText = %postText @ "&GLEW_ARB_shadow=" @ GLEW_ARB_shadow ();
	%postText = %postText @ "&GLEW_ARB_texture_rg=" @ GLEW_ARB_texture_rg ();
	%postText = %postText @ "&getShaderVersion=" @ getShaderVersion ();

	return %postText;
}
