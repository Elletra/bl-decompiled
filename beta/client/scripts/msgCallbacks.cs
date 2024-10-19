addMessageCallback('MsgYourDeath', handleYourDeath);
function handleYourDeath(%msgType, %__unused)
{
	setScrollMode($SCROLLMODE_BRICKS);
}

addMessageCallback('MsgError', handleError);
function handleError(%msgType, %__unused)
{
	alxPlay(AudioError);
}

addMessageCallback('MsgPlantError_Overlap', handlePlantError);
addMessageCallback('MsgPlantError_Float', handlePlantError);
addMessageCallback('MsgPlantError_Unstable', handlePlantError);
addMessageCallback('MsgPlantError_Buried', handlePlantError);
addMessageCallback('MsgPlantError_Stuck', handlePlantError);
addMessageCallback('MsgPlantError_TooFar', handlePlantError);
addMessageCallback('MsgPlantError_Teams', handlePlantError);
addMessageCallback('MsgPlantError_Flood', handlePlantError);
addMessageCallback('MsgPlantError_Limit', handlePlantError);
function handlePlantError(%msgType, %__unused)
{
	if ($pref::Video::useSmallPlantErrors)
	{
		%hudObj = Hud_PlantErrorSmall;
		%bitmap = "base/client/ui/PlantErrors_Small/";
	}
	else
	{
		%hudObj = HUD_PlantError;
		%bitmap = "base/client/ui/PlantErrors/";
	}
	if (getTag(%msgType) == getTag('MsgPlantError_Overlap'))
	{
		%bitmap = %bitmap @ "PlantError_Overlap";
	}
	else if (getTag(%msgType) == getTag('MsgPlantError_Float'))
	{
		%bitmap = %bitmap @ "PlantError_Float";
	}
	else if (getTag(%msgType) == getTag('MsgPlantError_Unstable'))
	{
		%bitmap = %bitmap @ "PlantError_Unstable";
	}
	else if (getTag(%msgType) == getTag('MsgPlantError_Buried'))
	{
		%bitmap = %bitmap @ "PlantError_Buried";
	}
	else if (getTag(%msgType) == getTag('MsgPlantError_Stuck'))
	{
		%bitmap = %bitmap @ "PlantError_Stuck";
	}
	else if (getTag(%msgType) == getTag('MsgPlantError_TooFar'))
	{
		%bitmap = %bitmap @ "PlantError_TooFar";
	}
	else if (getTag(%msgType) == getTag('MsgPlantError_Teams'))
	{
		%bitmap = %bitmap @ "PlantError_Teams";
	}
	else if (getTag(%msgType) == getTag('MsgPlantError_Flood'))
	{
		%bitmap = %bitmap @ "PlantError_Flood";
	}
	else if (getTag(%msgType) == getTag('MsgPlantError_Limit'))
	{
		%bitmap = %bitmap @ "PlantError_Limit";
	}
	else
	{
		%bitmap = %bitmap @ "PlantError_Forbidden";
	}
	%hudObj.setBitmap(%bitmap);
	%hudObj.setVisible(1);
	alxPlay(AudioError);
	if (isEventPending(%hudObj.hideSchedule))
	{
		cancel(%hudObj.hideSchedule);
	}
	%hudObj.hideSchedule = %hudObj.schedule(800, setVisible, 0);
}

addMessageCallback('MsgItemPickup', handleItemPickup);
function handleItemPickup(%msgType, %__unused, %slot, %itemData)
{
	$ToolData[%slot] = %itemData;
	$HUD_ToolIcon[%slot].setBitmap(%itemData.iconName);
	alxPlay(ItemPickup);
}

addMessageCallback('MsgDropItem', handleDropItem);
function handleDropItem(%msgType, %__unused, %slot)
{
}

addMessageCallback('MsgClearInv', handleClearInv);
function handleClearInv(%msgType)
{
	$CurrScrollBrickSlot = 0;
	HUD_BrickActive.setVisible(0);
	for (%i = 0; %i < $BSD_NumInventorySlots; %i++)
	{
		$InvData[%i] = 0;
		$HUD_BrickIcon[%i].setBitmap("");
	}
	HUD_BrickName.setText("");
	for (%i = 0; %i < $HUD_NumToolSlots; %i++)
	{
		$ToolData[%i] = 0;
		$HUD_ToolIcon[%i].setBitmap("");
	}
	if (isObject(HUD_ToolName))
	{
		HUD_ToolName.setText("");
	}
	$CurrScrollToolSlot = 0;
	return;
}

addMessageCallback('MsgHilightInv', handleHilightInv);
function handleHilightInv(%msgType, %__unused, %slot)
{
	return;
	for (%i = 0; %i < $BSD_NumInventorySlots; %i++)
	{
		eval("HUDInvActive" @ %i @ ".setVisible(false);");
	}
	if (%slot >= 0)
	{
		eval("HUDInvActive" @ %slot @ ".setVisible(true);");
	}
	return;
	%slot++;
	if ($invHilight == 1)
	{
		Slot1BG.setBitmap("base/client/ui/GUIBrickSide.png");
	}
	else if ($invHilight == 2)
	{
		Slot2BG.setBitmap("base/client/ui/GUIBrickSide.png");
	}
	else if ($invHilight == 3)
	{
		Slot3BG.setBitmap("base/client/ui/GUIBrickSide.png");
	}
	else if ($invHilight == 4)
	{
		Slot4BG.setBitmap("base/client/ui/GUIBrickSide.png");
	}
	else if ($invHilight == 5)
	{
		Slot5BG.setBitmap("base/client/ui/GUIBrickSide.png");
	}
	else if ($invHilight == 6)
	{
		Slot6BG.setBitmap("base/client/ui/GUIBrickSide.png");
	}
	else if ($invHilight == 7)
	{
		Slot7BG.setBitmap("base/client/ui/GUIBrickSide.png");
	}
	if (%slot == 1)
	{
		Slot1BG.setBitmap("base/client/ui/GUIBrickSideHilight.png");
	}
	else if (%slot == 2)
	{
		Slot2BG.setBitmap("base/client/ui/GUIBrickSideHilight.png");
	}
	else if (%slot == 3)
	{
		Slot3BG.setBitmap("base/client/ui/GUIBrickSideHilight.png");
	}
	else if (%slot == 4)
	{
		Slot4BG.setBitmap("base/client/ui/GUIBrickSideHilight.png");
	}
	else if (%slot == 5)
	{
		Slot5BG.setBitmap("base/client/ui/GUIBrickSideHilight.png");
	}
	else if (%slot == 6)
	{
		Slot6BG.setBitmap("base/client/ui/GUIBrickSideHilight.png");
	}
	else if (%slot == 7)
	{
		Slot7BG.setBitmap("base/client/ui/GUIBrickSideHilight.png");
	}
	$invHilight = %slot;
}

addMessageCallback('MsgEquipInv', handleEquipInv);
function handleEquipInv(%msgType, %__unused, %slot)
{
}

addMessageCallback('MsgDeEquipInv', handleDeEquipInv);
function handleDeEquipInv(%msgType, %__unused, %slot)
{
}

addMessageCallback('MsgSetInvData', handleSetInvData);
function handleSetInvData(%msgType, %__unused, %slot, %data)
{
	$InvData[%slot] = %data;
	if (isObject(%data))
	{
		if (!isFile(%data.iconName @ ".png"))
		{
			$HUD_BrickIcon[%slot].setBitmap("base/client/ui/brickIcons/unknown.png");
		}
		else
		{
			$HUD_BrickIcon[%slot].setBitmap(%data.iconName);
		}
	}
}

addMessageCallback('MsgStartTalking', handleStartTalking);
function handleStartTalking(%msgType, %__unused, %clientId)
{
	WhoTalk_addID(%clientId);
	return;
	%text = chatWhosTalkingText.getValue();
	%name = lstAdminPlayerList.getRowTextById(%clientId);
	if (strpos(%text, %name) == -1 && %name !$= "")
	{
		%text = %text SPC %name;
		ChatWhosTalkingBox.setVisible(1);
		chatWhosTalkingText.setText(%text);
	}
}

addMessageCallback('MsgStopTalking', handleStopTalking);
function handleStopTalking(%msgType, %__unused, %clientId)
{
	WhoTalk_removeID(%clientId);
	return;
	%text = chatWhosTalkingText.getValue();
	%name = lstAdminPlayerList.getRowTextById(%clientId);
	%text = trim(strreplace(%text, " " @ %name, ""));
	chatWhosTalkingText.setText(%text);
	if (strlen(%text) <= 0)
	{
		ChatWhosTalkingBox.setVisible(0);
	}
}

