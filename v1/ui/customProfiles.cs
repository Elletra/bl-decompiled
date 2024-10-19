new GuiControlProfile(BlockDefaultProfile)
{
	tab = 0;
	canKeyFocus = 0;
	hasBitmapArray = 0;
	mouseOverSelected = 0;
	opaque = 0;
	fillColor = "201 182 153";
	fillColorHL = "221 202 173";
	fillColorNA = "221 202 173";
	border = 0;
	borderColor = "0 0 0";
	borderColorHL = "179 134 94";
	borderColorNA = "126 79 37";
	fontType = "Arial";
	fontSize = 14;
	fontColor = "0 0 0";
	fontColorHL = "32 100 100";
	fontColorNA = "0 0 0";
	fontColorSEL = "200 200 200";
	bitmap = "./blockWindow.png";
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
new GuiControlProfile(BlockWindowProfile)
{
	opaque = 1;
	border = 2;
	fillColor = "171 171 171 255";
	fillColorHL = "171 171 171 255";
	fillColorNA = "171 171 171 255";
	fillColor = "200 200 200 255";
	fontType = "Impact";
	fontSize = 18;
	fontColor = "255 255 255";
	fontColorHL = "255 255 255";
	text = "Window";
	bitmap = "./blockWindow.png";
	textOffset = "5 2";
	hasBitmapArray = 1;
	justify = "left";
};
new GuiControlProfile(BlockScrollProfile)
{
	opaque = 1;
	fillColor = "255 255 255 255";
	fillColorHL = "171 171 171 255";
	fillColorNA = "171 171 171 255";
	border = 1;
	borderThickness = 1;
	borderColor = "0 0 0";
	bitmap = "./blockScroll.png";
	hasBitmapArray = 1;
	textOffset = "2 2";
};
new GuiControlProfile(BSDScrollProfile)
{
	opaque = 1;
	fillColor = "200 200 200 255";
	fillColorHL = "171 171 171 255";
	fillColorNA = "171 171 171 255";
	border = 1;
	borderThickness = 1;
	borderColor = "0 0 0";
	bitmap = "./blockScroll.png";
	hasBitmapArray = 1;
	textOffset = "2 2";
};
new GuiControlProfile(BlockCheckBoxProfile)
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
	bitmap = "./torqueCheck.png";
	hasBitmapArray = 1;
};
new GuiControlProfile(BlockRadioProfile)
{
	fontSize = 14;
	fillColor = "232 232 232";
	fontColorHL = "32 100 100";
	fixedExtent = 1;
	bitmap = "./blockRadio.png";
	hasBitmapArray = 1;
};
new GuiControlProfile(BlockButtonProfile)
{
	opaque = 1;
	border = 1;
	borderThickness = 1;
	borderColor = "0 0 0";
	fillColor = "149 152 166";
	fillColorHL = "171 171 171";
	fillColorNA = "221 202 173";
	fontType = "Impact";
	fontSize = 18;
	fontColor = "0 0 0";
	fontColorHL = "255 255 255";
	text = "GuiWindowCtrl test";
	bitmap = "./blockScroll.png";
	textOffset = "6 6";
	hasBitmapArray = 1;
	justify = "center";
};
new GuiControlProfile(BlockListProfile)
{
	opaque = 1;
	border = 1;
	borderThickness = 1;
	borderColor = "0 0 0";
	fillColor = "149 152 166";
	fillColorHL = "171 171 171";
	fillColorNA = "221 202 173";
	fontType = "Impact";
	fontSize = 18;
	fontColor = "0 0 0";
	fontColorHL = "171 171 171";
	textOffset = "6 6";
	hasBitmapArray = 1;
	justify = "center";
};
new GuiControlProfile(BlockTextEditProfile)
{
	opaque = 1;
	fillColor = "255 255 255";
	fillColorHL = "128 128 128";
	border = 1;
	borderThickness = 1;
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
new GuiControlProfile(HudInvTextProfile)
{
	opaque = 1;
	border = 0;
	fontColor = "255 255 255";
	fontColorHL = "255 255 255";
	text = "HUD TEXT";
	justify = "center";
};
new GuiControlProfile(MapDescriptionTextProfile)
{
	opaque = 0;
	border = 0;
	fontColor = "255 255 255";
	fontColorHL = "255 255 255";
	doFontOutline = 1;
	fontOutlineColor = "0 64 255";
	textOffset = "10 10";
	justify = "left";
};
new GuiControlProfile(LoadingBarTextProfile)
{
	opaque = 0;
	border = 0;
	fontColor = "255 255 255";
	text = "LOADING";
	justify = "center";
	fontType = "Arial";
	fontSize = 14;
};
new GuiControlProfile(LoadingMapNameProfile)
{
	opaque = 0;
	border = 0;
	fontColor = "255 255 255";
	text = "Map Name";
	justify = "center";
	fontType = "Arial";
	fontSize = 28;
};
if (!isObject(ChatHudScrollProfile))
{
	new GuiControlProfile(ChatHudScrollProfile)
	{
		opaque = 0;
		fillColor = "255 255 255 0";
		border = 0;
		borderThickness = 0;
		borderColor = "0 0 0";
		hasBitmapArray = 0;
	};
}
new GuiControlProfile(BlockChatTextProfile)
{
	textOffset = "3 0";
	fontColor = "0 0 0";
	fontColorHL = "130 130 130";
	fontColorNA = "255 0 0";
	fontColors[0] = "255 0 64";
	fontColors[1] = "64 64 255";
	fontColors[2] = "0 255 0";
	fontColors[3] = "255 255 0";
	fontColors[4] = "0 255 255";
	fontColors[5] = "255 0 255";
	fontColors[6] = "255 255 255";
	fontColors[7] = "96 96 96";
	doFontOutline = 1;
	fontOutlineColor = "0 0 0";
	fontType = "Palatino Linotype";
	fontSize = 18;
};
new GuiControlProfile(BlockChatTextSize0Profile : BlockChatTextProfile)
{
	fontSize = 16;
};
new GuiControlProfile(BlockChatTextSize1Profile : BlockChatTextProfile)
{
	fontSize = 18;
};
new GuiControlProfile(BlockChatTextSize2Profile : BlockChatTextProfile)
{
	fontSize = 20;
};
new GuiControlProfile(BlockChatTextSize3Profile : BlockChatTextProfile)
{
	fontSize = 22;
};
new GuiControlProfile(BlockChatTextSize4Profile : BlockChatTextProfile)
{
	fontSize = 24;
};
new GuiControlProfile(BlockChatTextSize5Profile : BlockChatTextProfile)
{
	fontSize = 26;
};
new GuiControlProfile(BlockChatTextSize6Profile : BlockChatTextProfile)
{
	fontSize = 28;
};
new GuiControlProfile(BlockChatTextSize7Profile : BlockChatTextProfile)
{
	fontSize = 30;
};
new GuiControlProfile(BlockChatTextSize8Profile : BlockChatTextProfile)
{
	fontSize = 32;
};
new GuiControlProfile(BlockChatTextSize9Profile : BlockChatTextProfile)
{
	fontSize = 34;
};
new GuiControlProfile(BlockChatTextSize10Profile : BlockChatTextProfile)
{
	fontSize = 36;
};
if (!isObject(BlockChatTextShadowProfile))
{
	new GuiControlProfile(BlockChatTextShadowProfile)
	{
		textOffset = "3 0";
		fontColor = "0 0 0";
		fontColorHL = "0 0 0";
		fontColorNA = "0 0 0";
		fontColors[0] = "0 0 0";
		fontColors[1] = "0 0 0";
		fontColors[2] = "0 0 0";
		fontColors[3] = "0 0 0";
		fontColors[4] = "0 0 0";
		fontColors[5] = "0 0 0";
		fontColors[6] = "0 0 0";
		border = 0;
		borderThickness = 0;
		borderColor = "0 0 0 0";
	};
}
new GuiControlProfile(ColorRadioProfile)
{
	fontSize = 14;
	fillColor = "232 232 232";
	fontColorHL = "32 100 100";
	fillColorNA = "0 0 0 255";
	fixedExtent = 1;
	bitmap = "./colorRadio.png";
	hasBitmapArray = 1;
	soundButtonDown = "";
	soundButtonOver = "";
};
new GuiControlProfile(ColorScrollProfile)
{
	opaque = 0;
	fillColor = "255 255 255 0";
	border = 1;
	borderThickness = 0;
	borderColor = "140 140 140 255";
	bitmap = "./halfScroll.png";
	hasBitmapArray = 1;
};
new GuiControlProfile(decalRadioProfile)
{
	fontSize = 14;
	fillColor = "232 232 232";
	fontColorHL = "32 100 100";
	fillColorNA = "0 0 0 255";
	fixedExtent = 1;
	bitmap = "./decalRadio.png";
	hasBitmapArray = 1;
	soundButtonDown = "";
	soundButtonOver = "";
};
new GuiControlProfile(HUDBitmapProfile)
{
	opaque = 0;
	fillColor = "255 255 255 0";
	border = 0;
	borderThickness = 0;
	borderColor = "255 255 255 0";
};
new GuiControlProfile(HUDBrickNameProfile)
{
	opaque = 0;
	border = 0;
	fontColor = "255 255 255";
	text = "Map Name";
	justify = "center";
	fontType = "Arial";
	fontSize = 14;
};
new GuiControlProfile(HUDCenterTextProfile)
{
	opaque = 0;
	border = 0;
	fontColor = "255 255 255";
	text = "Map Name";
	justify = "center";
	fontType = "Arial";
	fontSize = 12;
};
new GuiControlProfile(HUDRightTextProfile)
{
	opaque = 0;
	border = 0;
	fontColor = "255 255 255";
	text = "Map Name";
	justify = "right";
	fontType = "Arial";
	fontSize = 12;
};
new GuiControlProfile(HUDLeftTextProfile)
{
	opaque = 0;
	border = 0;
	fontColor = "255 255 255";
	text = "Map Name";
	justify = "left";
	fontType = "Arial";
	fontSize = 12;
};
new GuiControlProfile(HUDBSDNameProfile)
{
	opaque = 0;
	border = 0;
	fontColor = "0 0 0";
	text = "Map Name";
	justify = "center";
	fontType = "Arial";
	fontSize = 12;
};
new GuiControlProfile(HUDChatTextProfile)
{
	opaque = 0;
	border = 0;
	fontColor = "255 255 255";
	text = "Map Name";
	justify = "left";
	fontType = "Arial";
	fontSize = 14;
};
new GuiControlProfile(HUDChatTextEditProfile)
{
	opaque = 0;
	fillColor = "255 255 255 0";
	fillColorHL = "128 128 128";
	border = 0;
	borderThickness = 0;
	borderColor = "255 255 255";
	fontColor = "255 255 255";
	fontColorHL = "255 255 255";
	fontColorNA = "128 128 128";
	textOffset = "0 2";
	autoSizeWidth = 0;
	autoSizeHeight = 1;
	tab = 1;
	canKeyFocus = 1;
	doFontOutline = 1;
	fontOutlineColor = "0 0 0";
	fontType = "Palatino Linotype";
	fontSize = 18;
};
new GuiControlProfile(HUDChatTextEditSize0Profile : HUDChatTextEditProfile)
{
	fontSize = 16;
};
new GuiControlProfile(HUDChatTextEditSize1Profile : HUDChatTextEditProfile)
{
	fontSize = 18;
};
new GuiControlProfile(HUDChatTextEditSize2Profile : HUDChatTextEditProfile)
{
	fontSize = 20;
};
new GuiControlProfile(HUDChatTextEditSize3Profile : HUDChatTextEditProfile)
{
	fontSize = 22;
};
new GuiControlProfile(HUDChatTextEditSize4Profile : HUDChatTextEditProfile)
{
	fontSize = 24;
};
new GuiControlProfile(HUDChatTextEditSize5Profile : HUDChatTextEditProfile)
{
	fontSize = 26;
};
new GuiControlProfile(HUDChatTextEditSize6Profile : HUDChatTextEditProfile)
{
	fontSize = 28;
};
new GuiControlProfile(HUDChatTextEditSize7Profile : HUDChatTextEditProfile)
{
	fontSize = 30;
};
new GuiControlProfile(HUDChatTextEditSize8Profile : HUDChatTextEditProfile)
{
	fontSize = 32;
};
new GuiControlProfile(HUDChatTextEditSize9Profile : HUDChatTextEditProfile)
{
	fontSize = 34;
};
new GuiControlProfile(HUDChatTextEditSize10Profile : HUDChatTextEditProfile)
{
	fontSize = 36;
};
new GuiControlProfile(MM_LeftProfile)
{
	opaque = 0;
	border = 0;
	fontColor = "255 255 255";
	doFontOutline = 1;
	fontOutlineColor = "24 24 255";
	justify = "left";
	fontType = "Arial";
	fontSize = 14;
};
new GuiControlProfile(MM_RightProfile : MM_LeftProfile)
{
	justify = "right";
};
new GuiControlProfile(MM_CenterProfile : MM_LeftProfile)
{
	justify = "center";
};
if (!isObject(BlockChatChannelProfile))
{
	new GuiControlProfile(BlockChatChannelProfile)
	{
		textOffset = "3 0";
		fontColor = "0 0 0";
		fontColorHL = "130 130 130";
		fontColorNA = "255 0 0";
		fontColors[0] = "255 255 255";
		fontColors[1] = "255 0 255";
		doFontOutline = 1;
		fontOutlineColor = "0 0 0";
		fontType = "Palatino Linotype";
		fontSize = 18;
	};
}
new GuiControlProfile(BlockChatChannelSize0Profile : BlockChatChannelProfile)
{
	fontSize = 16;
};
new GuiControlProfile(BlockChatChannelSize1Profile : BlockChatChannelProfile)
{
	fontSize = 18;
};
new GuiControlProfile(BlockChatChannelSize2Profile : BlockChatChannelProfile)
{
	fontSize = 20;
};
new GuiControlProfile(BlockChatChannelSize3Profile : BlockChatChannelProfile)
{
	fontSize = 22;
};
new GuiControlProfile(BlockChatChannelSize4Profile : BlockChatChannelProfile)
{
	fontSize = 24;
};
new GuiControlProfile(BlockChatChannelSize5Profile : BlockChatChannelProfile)
{
	fontSize = 26;
};
new GuiControlProfile(BlockChatChannelSize6Profile : BlockChatChannelProfile)
{
	fontSize = 28;
};
new GuiControlProfile(BlockChatChannelSize7Profile : BlockChatChannelProfile)
{
	fontSize = 30;
};
new GuiControlProfile(BlockChatChannelSize8Profile : BlockChatChannelProfile)
{
	fontSize = 32;
};
new GuiControlProfile(BlockChatChannelSize9Profile : BlockChatChannelProfile)
{
	fontSize = 34;
};
new GuiControlProfile(BlockChatChannelSize10Profile : BlockChatChannelProfile)
{
	fontSize = 36;
};
new GuiControlProfile(MiniGameListProfile : GuiTextProfile)
{
	fontSize = 18;
	fontColorHL = "0 0 0";
	fillColorHL = "64 64 64";
	fontColors[0] = "255 0   0  ";
	fontColors[1] = "255 128 0  ";
	fontColors[2] = "255 255 0  ";
	fontColors[3] = "0   255 0  ";
	fontColors[4] = "0   128 0  ";
	fontColors[5] = "0   255 255";
	fontColors[6] = "0   128 128";
	fontColors[7] = "0   128 255";
	fontColors[8] = "255 128 255";
	fontColors[9] = "0   0   0  ";
};
new GuiControlProfile(PlayerListProfile : GuiTextProfile)
{
	fontSize = 18;
	fontColorHL = "0 0 0";
	fillColorHL = "171 171 171";
	fontColors[5] = "0 0 255";
};
new GuiControlProfile(OptionsMenuTextProfile)
{
	opaque = 0;
	border = 0;
	fontColor = "255 255 255";
	text = "LOADING";
	justify = "Left";
	fontType = "Arial";
	fontSize = 14;
};
