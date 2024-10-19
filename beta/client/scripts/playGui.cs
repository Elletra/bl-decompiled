function PlayGui::onWake(%this)
{
	$enableDirectInput = 1;
	activateDirectInput();
	Canvas.pushDialog(NewChatHud);
	moveMap.push();
	schedule(0, 0, "refreshCenterTextCtrl");
	schedule(0, 0, "refreshBottomTextCtrl");
}

function PlayGui::onSleep(%this)
{
	Canvas.popDialog(NewChatHud);
	moveMap.pop();
}

function refreshBottomTextCtrl()
{
	BottomPrintText.position = "0 0";
}

function refreshCenterTextCtrl()
{
	CenterPrintText.position = "0 0";
}

function PlayGui::killInvHud(%this)
{
	if (isEventPending(HUD_BrickBox.moveSchedule))
	{
		cancel(HUD_BrickBox.moveSchedule);
	}
	if (isObject(HUD_BrickBox))
	{
		HUD_BrickBox.delete();
	}
	if (isObject(HUD_BrickNameBG))
	{
		HUD_BrickNameBG.delete();
	}
	if (isObject(HUD_BrickName))
	{
		HUD_BrickName.delete();
	}
}

function PlayGui::createInvHUD(%this)
{
	%this.killInvHud();
	%numSlots = $BSD_NumInventorySlots;
	%res = getRes();
	%screenWidth = getWord(%res, 0);
	%screenHeight = getWord(%res, 1);
	%iconWidth = mFloor(%screenWidth / $BSD_NumInventorySlots);
	if (%iconWidth > 64)
	{
		%iconWidth = 64;
	}
	%iconBoxWidth = $BSD_NumInventorySlots * %iconWidth;
	%newBox = new GuiSwatchCtrl();
	%newBox.setName("Hud_BrickBox");
	%this.add(%newBox);
	%newBox.setProfile(HUDBitmapProfile);
	%newBox.setColor("0 0 0 0.25");
	%x = %screenWidth / 2 - %iconBoxWidth / 2;
	%y = (%screenHeight - %iconWidth * 1) - 5;
	%w = %iconBoxWidth;
	%h = %iconWidth;
	%newBox.resize(%x, %y, %w, %h);
	%newBox.horizSizing = "Center";
	%newActive = new GuiBitmapCtrl();
	%newBox.add(%newActive);
	%newActive.setProfile(HUDBitmapProfile);
	%newActive.setBitmap("base/client/ui/brickActive");
	%newActive.setBitmap("base/client/ui/brickIcons/brickIconActive");
	%newActive.setVisible(0);
	%x = 0;
	%y = 0;
	%w = %iconWidth;
	%h = %iconWidth;
	%newActive.resize(%x, %y, %w, %h);
	%newActive.setName("HUD_BrickActive");
	for (%i = 0; %i < %numSlots; %i++)
	{
		%newIcon = new GuiBitmapCtrl();
		%newBox.add(%newIcon);
		%newIcon.setProfile(HUDBitmapProfile);
		if (isObject($InvData[%i]))
		{
			if (!isFile($InvData[%i].iconName @ ".png"))
			{
				%newIcon.setBitmap("base/client/ui/brickIcons/unknown.png");
				continue;
			}
			error("setting inv icon playgui");
			%newIcon.setBitmap($InvData[%i].iconName);
		}
		%x = %i * %iconWidth;
		%y = 0;
		%w = %iconWidth;
		%h = %iconWidth;
		%newIcon.resize(%x, %y, %w, %h);
		$HUD_BrickIcon[%i] = %newIcon;
	}
	%newSwatch = new GuiSwatchCtrl();
	PlayGui.add(%newSwatch);
	%newSwatch.setProfile(HUDBrickNameProfile);
	%newSwatch.setColor("0.0 0.0 0.5 0.5");
	%w = %iconBoxWidth;
	%h = 18;
	%x = %screenWidth / 2 - %iconBoxWidth / 2;
	%y = ((%screenHeight - %iconWidth * 1) - %h) - 5;
	%newSwatch.resize(%x, %y, %w, %h);
	%newSwatch.setName("HUD_BrickNameBG");
	%newText = new GuiTextCtrl();
	%newSwatch.add(%newText);
	%newText.setProfile(HUDBrickNameProfile);
	%w = %iconBoxWidth;
	%h = 18;
	%x = 0;
	%y = 0;
	%newText.resize(%x, %y, %w, %h);
	%newText.setName("HUD_BrickName");
	%newText = new GuiTextCtrl();
	HUD_BrickNameBG.add(%newText);
	%newText.setProfile(HUDRightTextProfile);
	%key = strupr(getWord(moveMap.getBinding("openBSD"), 1));
	%newText.setText("Press" SPC %key SPC "for more bricks   ");
	%x = 0;
	%y = 0;
	%w = %iconBoxWidth;
	%h = 18;
	%newText.resize(%x, %y, %w, %h);
	%newText.setName("ToolTip_BSD");
	%newText = new GuiTextCtrl();
	HUD_BrickNameBG.add(%newText);
	%newText.setProfile(HUDLeftTextProfile);
	%key = strupr(getWord(moveMap.getBinding("useBricks"), 1)) SPC "or";
	%key = %key SPC strupr(getWord(moveMap.getBinding("useFirstSlot"), 1));
	%key = %key SPC strupr(getWord(moveMap.getBinding("useSecondSlot"), 1));
	%key = %key SPC strupr(getWord(moveMap.getBinding("useThirdSlot"), 1));
	%key = %key SPC strupr(getWord(moveMap.getBinding("useFourthSlot"), 1));
	%key = %key SPC strupr(getWord(moveMap.getBinding("useFifthSlot"), 1));
	%key = %key SPC strupr(getWord(moveMap.getBinding("useSixthSlot"), 1));
	%key = %key SPC strupr(getWord(moveMap.getBinding("useSeventhSlot"), 1));
	%key = %key SPC strupr(getWord(moveMap.getBinding("useEighthSlot"), 1));
	%key = %key SPC strupr(getWord(moveMap.getBinding("useNinthSlot"), 1));
	%key = %key SPC strupr(getWord(moveMap.getBinding("useTenthSlot"), 1));
	%newText.setText("  Press" SPC %key SPC "to use bricks");
	%x = 0;
	%y = 0;
	%w = %iconBoxWidth;
	%h = 18;
	%newText.resize(%x, %y, %w, %h);
	%newText.setName("ToolTip_Bricks");
	if (!$pref::HUD::showToolTips)
	{
		ToolTip_Bricks.setVisible(0);
		ToolTip_BSD.setVisible(0);
	}
	HUD_BrickName.setText("");
	if ($CurrScrollBrickSlot $= "")
	{
		$CurrScrollBrickSlot = 0;
	}
	if ($ScrollMode != $SCROLLMODE_BRICKS && $pref::HUD::HideBrickBox)
	{
		if ($pref::HUD::showToolTips)
		{
			PlayGui.hideBrickBox(64, 10, 0);
		}
		else
		{
			PlayGui.hideBrickBox(87, 10, 0);
		}
	}
	else if ($ScrollMode == $SCROLLMODE_BRICKS)
	{
		setActiveInv($CurrScrollBrickSlot);
		HUD_BrickActive.setVisible(1);
	}
}

function PlayGui::hideBrickBox(%this, %dist, %totalSteps, %currStep)
{
	if (%currStep >= %totalSteps)
	{
		return;
	}
	if (%currStep == %totalSteps - 1)
	{
		%sum = (%totalSteps - 1) * mFloor(%dist / %totalSteps);
		%stepDist = %dist - %sum;
	}
	else
	{
		%stepDist = mFloor(%dist / %totalSteps);
	}
	%pos = getWord(HUD_BrickBox.position, 1) + %stepDist;
	HUD_BrickBox.position = getWord(HUD_BrickBox.position, 0) SPC %pos;
	HUD_BrickNameBG.position = getWord(HUD_BrickNameBG.position, 0) SPC %pos - getWord(HUD_BrickNameBG.extent, 1);
	HUD_BrickBox.moveSchedule = PlayGui.schedule(10, hideBrickBox, %dist, %totalSteps, %currStep + 1);
}

function clientCmdPlayGui_LoadPaint()
{
	PlayGui.loadPaint();
}

function PlayGui::loadPaint(%this)
{
	%this.KillPaint();
	%swatchSize = 16;
	%res = getRes();
	%screenWidth = getWord(%res, 0);
	%screenHeight = getWord(%res, 1);
	%numDivs = 0;
	for (%i = 0; %i < 16; %i++)
	{
		if (getSprayCanDivisionSlot(%i) != 0)
		{
			%numDivs++;
		}
		else
		{
			break;
		}
	}
	%numDivs++;
	%lastDivStart = -1;
	%largestDivSize = 3;
	for (%i = 0; %i < %numDivs; %i++)
	{
		%currDivSize = getSprayCanDivisionSlot(%i) - %lastDivStart;
		$Paint_Row[%i] = new ScriptObject();
		$Paint_Row[%i].numSwatches = %currDivSize;
		RootGroup.add($Paint_Row[%i]);
		if (%currDivSize > %largestDivSize)
		{
			%largestDivSize = %currDivSize;
		}
		%lastDivStart = getSprayCanDivisionSlot(%i);
	}
	$Paint_NumPaintRows = %numDivs;
	$Paint_NumPaintRows = %numDivs;
	$Paint_Row[$Paint_NumPaintRows - 1].numSwatches = 3;
	%boxWidth = %numDivs * (%swatchSize + 1) + 1;
	%boxHeight = %largestDivSize * (%swatchSize + 1) + 1;
	%newBox = new GuiSwatchCtrl();
	%this.add(%newBox);
	%newBox.setProfile(HUDBitmapProfile);
	%newBox.setColor("0 0 0 0.25");
	%x = 0 + 5;
	%y = (%screenHeight - %boxHeight) - 5;
	%w = %boxWidth;
	%h = %boxHeight;
	%newBox.resize(%x, %y, %w, %h);
	%newBox.setName("HUD_PaintBox");
	%currDiv = 0;
	for (%i = 0; %i < 64; %i++)
	{
		%color = getColorIDTable(%i);
		%red = getWord(%color, 0);
		%green = getWord(%color, 1);
		%blue = getWord(%color, 2);
		%alpha = getWord(%color, 3);
		if (%red == 0 && %green == 0 && %blue == 0 && %alpha == 0)
		{
			break;
		}
		if (getSprayCanDivisionSlot(%currDiv) != 0 && %i > getSprayCanDivisionSlot(%currDiv))
		{
			%currDiv++;
		}
		%newSwatch = new GuiSwatchCtrl();
		%newBox.add(%newSwatch);
		%newSwatch.setProfile(BlockDefaultProfile);
		%newSwatch.setColor(%color);
		if (%currDiv > 0)
		{
			%swatchNum = (%i - getSprayCanDivisionSlot(%currDiv - 1)) - 1;
		}
		else
		{
			%swatchNum = %i;
		}
		%x = (%swatchSize + 1) * %currDiv + 1;
		%y = (%swatchSize + 1) * %swatchNum + 1;
		%w = %swatchSize;
		%h = %swatchSize;
		%newSwatch.resize(%x, %y, %w, %h);
		$Paint_Row[%currDiv].swatch[%swatchNum] = %newSwatch;
	}
	%newSwatch = new GuiSwatchCtrl();
	HUD_PaintBox.add(%newSwatch);
	%newSwatch.setProfile(BlockDefaultProfile);
	%newSwatch.setColor("0.2 0.2 0.2 1.0");
	%x = (%swatchSize + 1) * (%numDivs - 1) + 1;
	%y = 1;
	%w = %swatchSize;
	%h = %swatchSize;
	%newSwatch.resize(%x, %y, %w, %h);
	$Paint_Row[$Paint_NumPaintRows - 1].swatch[0] = %newSwatch;
	%newSwatch = new GuiSwatchCtrl();
	HUD_PaintBox.add(%newSwatch);
	%newSwatch.setProfile(BlockDefaultProfile);
	%newSwatch.setColor("1.0 1.0 0.9098 1.0");
	%x = (%swatchSize + 1) * (%numDivs - 1) + 1;
	%y = (%swatchSize + 1) * 1 + 1;
	%w = %swatchSize;
	%h = %swatchSize;
	%newSwatch.resize(%x, %y, %w, %h);
	$Paint_Row[$Paint_NumPaintRows - 1].swatch[1] = %newSwatch;
	%newSwatch = new GuiSwatchCtrl();
	HUD_PaintBox.add(%newSwatch);
	%newSwatch.setProfile(BlockDefaultProfile);
	%newSwatch.setColor("0.9098 0.9098 1.0 1.0");
	%x = (%swatchSize + 1) * (%numDivs - 1) + 1;
	%y = (%swatchSize + 1) * 2 + 1;
	%w = %swatchSize;
	%h = %swatchSize;
	%newSwatch.resize(%x, %y, %w, %h);
	$Paint_Row[$Paint_NumPaintRows - 1].swatch[2] = %newSwatch;
	%newActive = new GuiBitmapCtrl();
	HUD_PaintBox.add(%newActive);
	%newActive.setProfile(HUDBitmapProfile);
	%newActive.setBitmap("base/client/ui/paintActive");
	%x = 0;
	%y = 0;
	%w = 18;
	%h = 18;
	%newActive.resize(%x, %y, %w, %h);
	%newActive.setName("HUD_PaintActive");
	%newSwatch = new GuiSwatchCtrl();
	PlayGui.add(%newSwatch);
	%newSwatch.setProfile(HUDBrickNameProfile);
	%newSwatch.setColor("0.0 0.0 0.5 0.5");
	%w = %boxWidth;
	%h = 18;
	%x = 0 + 5;
	%y = ((%screenHeight - %boxHeight) - 5) - %h;
	%newSwatch.resize(%x, %y, %w, %h);
	%newSwatch.setName("HUD_PaintNameBG");
	%newText = new GuiTextCtrl();
	%newSwatch.add(%newText);
	%newText.setProfile(HUDCenterTextProfile);
	%w = %boxWidth;
	%h = 18;
	%x = 0;
	%y = 0;
	%newText.resize(%x, %y, %w, %h);
	%newText.setName("HUD_PaintName");
	%newText = new GuiTextCtrl();
	HUD_PaintNameBG.add(%newText);
	%newText.setProfile(HUDCenterTextProfile);
	%key = strupr(getWord(moveMap.getBinding("useSprayCan"), 1));
	%newText.setText("Press" SPC %key SPC "for paint");
	%x = 0;
	%y = 0;
	%w = %boxWidth;
	%h = 18;
	%newText.resize(%x, %y, %w, %h);
	%newText.setName("ToolTip_Paint");
	if (!$pref::HUD::showToolTips)
	{
		ToolTip_Paint.setVisible(0);
	}
	if ($CurrPaintSwatch $= "")
	{
		$CurrPaintSwatch = 0;
	}
	if ($CurrPaintRow $= "")
	{
		$CurrPaintRow = 0;
	}
	HUD_PaintName.setText("");
	%this.FadePaintRows();
	HUD_PaintActive.setVisible(0);
	if ($ScrollMode != $SCROLLMODE_PAINT && $pref::HUD::HidePaintBox)
	{
		PlayGui.hidePaintBox(getWord(HUD_PaintBox.extent, 0) + 5, 10, 0);
	}
	else if ($ScrollMode == $SCROLLMODE_PAINT)
	{
		%this.UnFadePaintRow($CurrPaintRow);
		%this.updatePaintActive();
		HUD_PaintActive.setVisible(1);
		ToolTip_Paint.setVisible(0);
	}
}

function PlayGui::hidePaintBox(%this, %dist, %totalSteps, %currStep)
{
	if (%currStep >= %totalSteps)
	{
		return;
	}
	if (%currStep == %totalSteps - 1)
	{
		%sum = (%totalSteps - 1) * mFloor(%dist / %totalSteps);
		%stepDist = %dist - %sum;
	}
	else
	{
		%stepDist = mFloor(%dist / %totalSteps);
	}
	%pos = getWord(HUD_PaintBox.position, 0) - %stepDist;
	HUD_PaintBox.position = %pos SPC getWord(HUD_PaintBox.position, 1);
	HUD_PaintNameBG.position = %pos SPC getWord(HUD_PaintNameBG.position, 1);
	HUD_PaintBox.moveSchedule = PlayGui.schedule(10, hidePaintBox, %dist, %totalSteps, %currStep + 1, %originalPos);
}

function PlayGui::updatePaintActive(%this)
{
	%x = (16 + 1) * $CurrPaintRow;
	%y = (16 + 1) * $CurrPaintSwatch;
	%w = 18;
	%h = 18;
	HUD_PaintActive.resize(%x, %y, %w, %h);
}

function PlayGui::FadePaintRows(%this)
{
	for (%i = 0; %i < $Paint_NumPaintRows; %i++)
	{
		PlayGui.FadePaintRow(%i);
	}
}

function PlayGui::FadePaintRow(%this, %row)
{
	for (%i = 0; %i < $Paint_Row[%row].numSwatches; %i++)
	{
		%color = $Paint_Row[%row].swatch[%i].getColor();
		%red = getWord(%color, 0);
		%green = getWord(%color, 1);
		%blue = getWord(%color, 2);
		%alpha = getWord(%color, 3);
		%newColor = %red @ " " @ %green @ " " @ %blue @ " " @ %alpha * 0.3;
		$Paint_Row[%row].swatch[%i].setColor(%newColor);
	}
}

function PlayGui::UnFadePaintRow(%this, %row)
{
	for (%i = 0; %i < $Paint_Row[%row].numSwatches; %i++)
	{
		%color = $Paint_Row[%row].swatch[%i].getColor();
		%red = getWord(%color, 0);
		%green = getWord(%color, 1);
		%blue = getWord(%color, 2);
		%alpha = getWord(%color, 3);
		%newColor = %red @ " " @ %green @ " " @ %blue @ " " @ %alpha / 0.3;
		$Paint_Row[%row].swatch[%i].setColor(%newColor);
	}
	HUD_PaintName.setText(getSprayCanDivisionName($CurrPaintRow));
}

function PlayGui::KillPaint(%this)
{
	if (isEventPending(HUD_PaintBox.moveSchedule))
	{
		cancel(HUD_PaintBox.moveSchedule);
	}
	if (isObject(HUD_PaintBox))
	{
		HUD_PaintBox.delete();
	}
	if (isObject(HUD_PaintNameBG))
	{
		HUD_PaintNameBG.delete();
	}
	if (isObject(HUD_PaintName))
	{
		HUD_PaintName.delete();
	}
	for (%i = 0; %i < $Paint_NumPaintRows; %i++)
	{
		if (isObject($Paint_Row[%i]))
		{
			$Paint_Row[%i].delete();
		}
	}
}

function clientCmdPlayGui_CreateToolHud(%numSlots)
{
	$HUD_NumToolSlots = %numSlots;
	PlayGui.createToolHUD();
}

function PlayGui::createToolHUD(%this)
{
	%this.killToolHud();
	%numSlots = $HUD_NumToolSlots;
	%res = getRes();
	%screenWidth = getWord(%res, 0);
	%screenHeight = getWord(%res, 1);
	%iconWidth = mFloor(%screenWidth / $BSD_NumInventorySlots);
	if (%iconWidth > 64)
	{
		%iconWidth = 64;
	}
	%iconBoxHeight = $HUD_NumToolSlots * %iconWidth;
	%newBox = new GuiSwatchCtrl();
	%newBox.setName("HUD_ToolBox");
	%this.add(%newBox);
	%newBox.setProfile(HUDBitmapProfile);
	%newBox.setColor("0 0 0 0.25");
	%x = (%screenWidth - %iconWidth) - 5;
	%y = 5;
	%w = %iconWidth;
	%h = %iconBoxHeight;
	%newBox.resize(%x, %y, %w, %h);
	%newActive = new GuiBitmapCtrl();
	%newBox.add(%newActive);
	%newActive.setProfile(HUDBitmapProfile);
	%newActive.setBitmap("base/client/ui/brickActive");
	%newActive.setBitmap("base/client/ui/brickIcons/brickIconActive");
	%newActive.setVisible(0);
	%x = 0;
	%y = 0;
	%w = %iconWidth;
	%h = %iconWidth;
	%newActive.resize(%x, %y, %w, %h);
	%newActive.setName("HUD_ToolActive");
	for (%i = 0; %i < %numSlots; %i++)
	{
		%newIcon = new GuiBitmapCtrl();
		%newBox.add(%newIcon);
		%newIcon.setProfile(HUDBitmapProfile);
		if ($ToolData[%i] > 0)
		{
			%newIcon.setBitmap($ToolData[%i].iconName);
		}
		%x = 0;
		%y = %i * %iconWidth;
		%w = %iconWidth;
		%h = %iconWidth;
		%newIcon.resize(%x, %y, %w, %h);
		$HUD_ToolIcon[%i] = %newIcon;
	}
	%newSwatch = new GuiSwatchCtrl();
	PlayGui.add(%newSwatch);
	%newSwatch.setProfile(HUDBrickNameProfile);
	%newSwatch.setColor("0.0 0.0 0.5 0.5");
	%w = %iconWidth;
	%h = 18;
	%x = (%screenWidth - %iconWidth) - 5;
	%y = %iconWidth * %numSlots + 5;
	%newSwatch.resize(%x, %y, %w, %h);
	%newSwatch.setName("HUD_ToolNameBG");
	%newText = new GuiTextCtrl();
	%newSwatch.add(%newText);
	%newText.setProfile(HUDCenterTextProfile);
	%w = %iconWidth;
	%h = 18;
	%x = 0;
	%y = 0;
	%newText.resize(%x, %y, %w, %h);
	%newText.setName("HUD_ToolName");
	%newText = new GuiTextCtrl();
	HUD_ToolNameBG.add(%newText);
	%newText.setProfile(HUDCenterTextProfile);
	%key = strupr(getWord(moveMap.getBinding("useTools"), 1));
	%newText.setText(%key SPC "= tools");
	%x = 0;
	%y = 0;
	%w = %iconWidth;
	%h = 18;
	%newText.resize(%x, %y, %w, %h);
	%newText.setName("ToolTip_Tools");
	if (!$pref::HUD::showToolTips)
	{
		ToolTip_Tools.setVisible(0);
	}
	HUD_ToolName.setText("");
	if ($CurrScrollToolSlot $= "")
	{
		$CurrScrollToolSlot = 0;
	}
	if ($ScrollMode != $SCROLLMODE_TOOLS && $pref::HUD::HideToolBox)
	{
		if ($pref::HUD::showToolTips)
		{
			PlayGui.hideToolBox($HUD_NumToolSlots * 64, 10, 0);
		}
		else
		{
			PlayGui.hideToolBox($HUD_NumToolSlots * 64 + 25, 10, 0);
		}
		HUD_ToolActive.setVisible(0);
	}
	else if ($ScrollMode == $SCROLLMODE_TOOLS)
	{
		setActiveTool($CurrScrollToolSlot);
		HUD_ToolActive.setVisible(1);
		ToolTip_Tools.setVisible(0);
	}
}

function PlayGui::hideToolBox(%this, %dist, %totalSteps, %currStep)
{
	if (%currStep >= %totalSteps)
	{
		return;
	}
	if (%currStep == %totalSteps - 1)
	{
		%sum = (%totalSteps - 1) * mFloor(%dist / %totalSteps);
		%stepDist = %dist - %sum;
	}
	else
	{
		%stepDist = mFloor(%dist / %totalSteps);
	}
	%pos = getWord(HUD_ToolBox.position, 1) - %stepDist;
	HUD_ToolBox.position = getWord(HUD_ToolBox.position, 0) SPC %pos;
	HUD_ToolNameBG.position = getWord(HUD_ToolNameBG.position, 0) SPC %pos + getWord(HUD_ToolBox.extent, 1);
	HUD_ToolBox.moveSchedule = PlayGui.schedule(10, hideToolBox, %dist, %totalSteps, %currStep + 1);
}

function PlayGui::killToolHud(%this)
{
	if (isEventPending(HUD_ToolBox.moveSchedule))
	{
		cancel(HUD_ToolBox.moveSchedule);
	}
	if (isObject(HUD_ToolBox))
	{
		HUD_ToolBox.delete();
	}
	if (isObject(HUD_ToolNameBG))
	{
		HUD_ToolNameBG.delete();
	}
	if (isObject(HUD_ToolName))
	{
		HUD_ToolName.delete();
	}
}

