$Gui::fontCacheDirectory = ExpandFilename("./cache");
$Gui::clipboardFile = ExpandFilename("./cache/clipboard.gui");
if (!isObject(GuiDefaultProfile))
{
	new GuiControlProfile(GuiDefaultProfile)
	{
		tab = 0;
		canKeyFocus = 0;
		hasBitmapArray = 0;
		mouseOverSelected = 0;
		opaque = 0;
		fillColor = $platform $= "macos" ? "211 211 211" : "201 182 153";
		fillColorHL = $platform $= "macos" ? "244 244 244" : "221 202 173";
		fillColorNA = $platform $= "macos" ? "244 244 244" : "221 202 173";
		border = 0;
		borderColor = "0 0 0";
		borderColorHL = "128 128 128";
		borderColorNA = "64 64 64";
		fontType = "Arial";
		fontSize = 14;
		fontColor = "0 0 0";
		fontColorHL = "32 100 100";
		fontColorNA = "0 0 0";
		fontColorSEL = "200 200 200";
		bitmap = $platform $= "macos" ? "./osxWindow" : "./demoWindow";
		bitmapBase = "";
		textOffset = "0 0";
		modal = 1;
		justify = "left";
		autoSizeWidth = 0;
		autoSizeHeight = 0;
		returnTab = 0;
		numbersOnly = 0;
		cursorColor = "0 0 0 255";
		soundButtonDown = "";
		soundButtonOver = "";
	};
}
if (!isObject(GuiInputCtrlProfile))
{
	new GuiControlProfile(GuiInputCtrlProfile)
	{
		tab = 1;
		canKeyFocus = 1;
	};
}
if (!isObject(GuiDialogProfile))
{
	new GuiControlProfile(GuiDialogProfile);
}
if (!isObject(GuiSolidDefaultProfile))
{
	new GuiControlProfile(GuiSolidDefaultProfile)
	{
		opaque = 1;
		border = 1;
	};
}
if (!isObject(GuiWindowProfile))
{
	new GuiControlProfile(GuiWindowProfile)
	{
		opaque = 1;
		border = 2;
		fillColor = $platform $= "macos" ? "211 211 211" : "201 182 153";
		fillColorHL = $platform $= "macos" ? "190 255 255" : "64 150 150";
		fillColorNA = $platform $= "macos" ? "255 255 255" : "150 150 150";
		fontColor = $platform $= "macos" ? "0 0 0" : "255 255 255";
		fontColorHL = $platform $= "macos" ? "200 200 200" : "0 0 0";
		text = "GuiWindowCtrl test";
		bitmap = $platform $= "macos" ? "./osxWindow" : "./demoWindow";
		textOffset = $platform $= "macos" ? "5 5" : "6 6";
		hasBitmapArray = 1;
		justify = $platform $= "macos" ? "center" : "left";
	};
}
if (!isObject(GuiToolWindowProfile))
{
	new GuiControlProfile(GuiToolWindowProfile)
	{
		opaque = 1;
		border = 2;
		fillColor = "201 182 153";
		fillColorHL = "64 150 150";
		fillColorNA = "150 150 150";
		fontColor = "255 255 255";
		fontColorHL = "0 0 0";
		bitmap = "./torqueToolWindow";
		textOffset = "6 6";
	};
}
if (!isObject(EditorToolButtonProfile))
{
	new GuiControlProfile(EditorToolButtonProfile)
	{
		opaque = 1;
		border = 2;
	};
}
if (!isObject(GuiContentProfile))
{
	new GuiControlProfile(GuiContentProfile)
	{
		opaque = 1;
		fillColor = "255 255 255";
	};
}
if (!isObject(GuiModelessDialogProfile))
{
	new GuiControlProfile("GuiModelessDialogProfile")
	{
		modal = 0;
	};
}
if (!isObject(GuiButtonProfile))
{
	new GuiControlProfile(GuiButtonProfile)
	{
		opaque = 1;
		border = 1;
		fontColor = "0 0 0";
		fontColorHL = "32 100 100";
		fixedExtent = 1;
		justify = "center";
		canKeyFocus = 0;
	};
}
if (!isObject(GuiBorderButtonProfile))
{
	new GuiControlProfile(GuiBorderButtonProfile)
	{
		fontColorHL = "0 0 0";
	};
}
if (!isObject(GuiMenuBarProfile))
{
	new GuiControlProfile(GuiMenuBarProfile)
	{
		opaque = 1;
		fillColorHL = "0 0 96";
		border = 4;
		fontColor = "0 0 0";
		fontColorHL = "255 255 255";
		fontColorNA = "128 128 128";
		fixedExtent = 1;
		justify = "center";
		canKeyFocus = 0;
		mouseOverSelected = 1;
		bitmap = $platform $= "macos" ? "./osxMenu" : "./torqueMenu";
		hasBitmapArray = 1;
	};
}
if (!isObject(GuiButtonSmProfile))
{
	new GuiControlProfile(GuiButtonSmProfile : GuiButtonProfile)
	{
		fontSize = 14;
	};
}
if (!isObject(GuiRadioProfile))
{
	new GuiControlProfile(GuiRadioProfile)
	{
		fontSize = 14;
		fillColor = "232 232 232";
		fontColorHL = "32 100 100";
		fixedExtent = 1;
		bitmap = $platform $= "macos" ? "./osxRadio" : "./torqueRadio";
		hasBitmapArray = 1;
	};
}
if (!isObject(GuiScrollProfile))
{
	new GuiControlProfile(GuiScrollProfile)
	{
		opaque = 1;
		fillColor = "255 255 255";
		border = 3;
		borderThickness = 1;
		borderColor = "0 0 0";
		bitmap = $platform $= "macos" ? "./osxScroll" : "./demoScroll";
		hasBitmapArray = 1;
	};
}
if (!isObject(GuiSliderProfile))
{
	new GuiControlProfile(GuiSliderProfile);
}
if (!isObject(GuiTextProfile))
{
	new GuiControlProfile(GuiTextProfile)
	{
		fontColor = "0 0 0";
		fontColorLink = "255 96 96";
		fontColorLinkHL = "0 0 255";
		autoSizeWidth = 1;
		autoSizeHeight = 1;
	};
}
if (!isObject(EditorTextProfile))
{
	new GuiControlProfile(EditorTextProfile)
	{
		fontType = "Arial Bold";
		fontColor = "0 0 0";
		autoSizeWidth = 1;
		autoSizeHeight = 1;
	};
}
if (!isObject(EditorTextProfileWhite))
{
	new GuiControlProfile(EditorTextProfileWhite)
	{
		fontType = "Arial Bold";
		fontColor = "255 255 255";
		autoSizeWidth = 1;
		autoSizeHeight = 1;
	};
}
if (!isObject(GuiMediumTextProfile))
{
	new GuiControlProfile(GuiMediumTextProfile : GuiTextProfile)
	{
		fontSize = 24;
	};
}
if (!isObject(GuiBigTextProfile))
{
	new GuiControlProfile(GuiBigTextProfile : GuiTextProfile)
	{
		fontSize = 36;
	};
}
if (!isObject(GuiCenterTextProfile))
{
	new GuiControlProfile(GuiCenterTextProfile : GuiTextProfile)
	{
		justify = "center";
	};
}
if (!isObject(MissionEditorProfile))
{
	new GuiControlProfile(MissionEditorProfile)
	{
		canKeyFocus = 1;
	};
}
if (!isObject(EditorScrollProfile))
{
	new GuiControlProfile(EditorScrollProfile)
	{
		opaque = 1;
		fillColor = "192 192 192 192";
		border = 3;
		borderThickness = 2;
		borderColor = "0 0 0";
		bitmap = "./demoScroll";
		hasBitmapArray = 1;
	};
}
if (!isObject(GuiTextEditProfile))
{
	new GuiControlProfile(GuiTextEditProfile)
	{
		opaque = 1;
		fillColor = "255 255 255";
		fillColorHL = "128 128 128";
		border = 3;
		borderThickness = 2;
		borderColor = "0 0 0";
		fontColor = "0 0 0";
		fontColorHL = "255 255 255";
		fontColorNA = "128 128 128";
		textOffset = "0 2";
		autoSizeWidth = 0;
		autoSizeHeight = 1;
		tab = 1;
		canKeyFocus = 1;
	};
}
if (!isObject(GuiControlListPopupProfile))
{
	new GuiControlProfile(GuiControlListPopupProfile)
	{
		opaque = 1;
		fillColor = "255 255 255";
		fillColorHL = "128 128 128";
		border = 1;
		borderColor = "0 0 0";
		fontColor = "0 0 0";
		fontColorHL = "255 255 255";
		fontColorNA = "128 128 128";
		textOffset = "0 2";
		autoSizeWidth = 0;
		autoSizeHeight = 1;
		tab = 1;
		canKeyFocus = 1;
		bitmap = $platform $= "macos" ? "./osxScroll" : "./demoScroll";
		hasBitmapArray = 1;
	};
}
if (!isObject(GuiTextArrayProfile))
{
	new GuiControlProfile(GuiTextArrayProfile : GuiTextProfile)
	{
		fontColorHL = "32 100 100";
		fillColorHL = "200 200 200";
	};
}
if (!isObject(GuiTextListProfile))
{
	new GuiControlProfile(GuiTextListProfile : GuiTextProfile);
}
if (!isObject(GuiTreeViewProfile))
{
	new GuiControlProfile(GuiTreeViewProfile)
	{
		fontSize = 13;
		fontColor = "0 0 0";
		fontColorHL = "64 150 150";
	};
}
if (!isObject(GuiCheckBoxProfile))
{
	new GuiControlProfile(GuiCheckBoxProfile)
	{
		opaque = 0;
		fillColor = "232 232 232";
		border = 0;
		borderColor = "0 0 0";
		fontSize = 14;
		fontColor = "0 0 0";
		fontColorHL = "32 100 100";
		fixedExtent = 1;
		justify = "left";
		bitmap = $platform $= "macos" ? "./osxCheck" : "./demoCheck";
		hasBitmapArray = 1;
	};
}
if (!isObject(GuiPopUpMenuProfile))
{
	new GuiControlProfile(GuiPopUpMenuProfile)
	{
		opaque = 1;
		mouseOverSelected = 1;
		border = 4;
		borderThickness = 2;
		borderColor = "0 0 0";
		fontSize = 14;
		fontColor = "0 0 0";
		fontColorHL = "32 100 100";
		fontColorSEL = "32 100 100";
		fixedExtent = 1;
		justify = "center";
		bitmap = $platform $= "macos" ? "./osxScroll" : "./demoScroll";
		hasBitmapArray = 1;
	};
}
if (!isObject(GuiEditorClassProfile))
{
	new GuiControlProfile(GuiEditorClassProfile)
	{
		opaque = 1;
		fillColor = "232 232 232";
		border = 1;
		borderColor = "0 0 0";
		borderColorHL = "127 127 127";
		fontColor = "0 0 0";
		fontColorHL = "32 100 100";
		fixedExtent = 1;
		justify = "center";
		bitmap = $platform $= "macos" ? "./osxScroll" : "./demoScroll";
		hasBitmapArray = 1;
	};
}
if (!isObject(LoadTextProfile))
{
	new GuiControlProfile("LoadTextProfile")
	{
		fontColor = "66 219 234";
		autoSizeWidth = 1;
		autoSizeHeight = 1;
	};
}
if (!isObject(GuiMLTextProfile))
{
	new GuiControlProfile("GuiMLTextProfile")
	{
		fontColorLink = "255 96 96";
		fontColorLinkHL = "0 0 255";
	};
}
if (!isObject(GuiMLTextEditProfile))
{
	new GuiControlProfile(GuiMLTextEditProfile)
	{
		fontColorLink = "255 96 96";
		fontColorLinkHL = "0 0 255";
		fillColor = "255 255 255";
		fillColorHL = "128 128 128";
		fontColor = "0 0 0";
		fontColorHL = "255 255 255";
		fontColorNA = "128 128 128";
		autoSizeWidth = 1;
		autoSizeHeight = 1;
		tab = 1;
		canKeyFocus = 1;
	};
}
if (!isObject(GuiConsoleProfile))
{
	new GuiControlProfile("GuiConsoleProfile")
	{
		fontType = $platform $= "macos" ? "Courier New" : "Lucida Console";
		fontSize = 12;
		fontColor = "0 0 0";
		fontColorHL = "130 130 130";
		fontColorNA = "255 0 0";
		fontColors[6] = "50 50 50";
		fontColors[7] = "50 50 0";
		fontColors[8] = "0 0 50";
		fontColors[9] = "0 50 0";
	};
}
if (!isObject(GuiProgressProfile))
{
	new GuiControlProfile("GuiProgressProfile")
	{
		opaque = 0;
		fillColor = "44 152 162 100";
		border = 1;
		borderColor = "78 88 120";
	};
}
if (!isObject(GuiProgressTextProfile))
{
	new GuiControlProfile("GuiProgressTextProfile")
	{
		fontColor = "0 0 0";
		justify = "center";
	};
}
if (!isObject(GuiInspectorTextEditProfile))
{
	new GuiControlProfile("GuiInspectorTextEditProfile")
	{
		opaque = 1;
		fillColor = "255 255 255";
		fillColorHL = "128 128 128";
		border = 1;
		borderColor = "0 0 0";
		fontColor = "0 0 0";
		fontColorHL = "255 255 255";
		autoSizeWidth = 0;
		autoSizeHeight = 1;
		tab = 0;
		canKeyFocus = 1;
	};
}
if (!isObject(GuiBitmapBorderProfile))
{
	new GuiControlProfile(GuiBitmapBorderProfile)
	{
		bitmap = "./darkBorder";
		hasBitmapArray = 1;
	};
}
new GuiCursor(DefaultCursor)
{
	hotSpot = "1 1";
	bitmapName = "./CUR_3darrow";
};
