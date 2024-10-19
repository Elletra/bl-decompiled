$minifigColor[0] = "0.867 0.000 0.000 1.000";
$minifigColor[1] = "0.973 0.800 0.000 1.000";
$minifigColor[2] = "0.000 0.471 0.196 1.000";
$minifigColor[3] = "0.000 0.317 0.745 1.000";
$minifigColor[4] = "0.996 0.996 0.910 1.000";
$minifigColor[5] = "0.647 0.647 0.647 1.000";
$minifigColor[6] = "0.471 0.471 0.471 1.000";
$minifigColor[7] = "0.200 0.200 0.200 1.000";
$minifigColor[8] = "0.400 0.196 0.000 1.000";
$minifigColor[9] = "0.667 0.000 0.000 0.700";
$minifigColor[10] = "1.000 0.500 0.000 0.800";
$minifigColor[11] = "0.850 1.000 0.000 0.600";
$minifigColor[12] = "0.990 0.960 0.000 0.800";
$minifigColor[13] = "0.000 0.471 0.196 0.750";
$minifigColor[14] = "0.000 0.200 0.640 0.750";
$minifigColor[15] = "0.550 0.700 1.000 0.700";
$minifigColor[16] = "0.850 0.850 0.850 0.700";
$minifigColor[17] = "0.847059 0.894118 0.654902 1.000000";
$minifigColor[18] = "0.631373 0.764706 0.541176 1.000000";
$minifigColor[19] = "0.639216 0.737255 0.274510 1.000000";
$minifigColor[20] = "0.756863 0.792157 0.866667 1.000000";
$minifigColor[21] = "0.329412 0.647059 0.686275 1.000000";
$minifigColor[22] = "0.258824 0.329412 0.576471 1.000000";
$minifigColor[23] = "0.976471 0.909804 0.600000 1.000000";
$minifigColor[24] = "0.776471 0.819608 0.231373 1.000000";
$minifigColor[25] = "0.756863 0.854902 0.721569 1.000000";
$minifigColor[26] = "0.713726 0.839216 0.831373 1.000000";
$minifigColor[27] = "0.701961 0.819608 0.886275 1.000000";
$minifigColor[28] = "0.764706 0.439216 0.627451 1.000000";
$hatColors["none"] = "0 1 2 3 4 5 6 7 8";
$hatColors["helmet"] = "0 1 2 3 4 5 6 7 8";
$hatColors["scoutHat"] = "2 8";
$hatColors["pointyHelmet"] = "6 7";
$packColors["none"] = "0 1 2 3 4 5 6 7 8";
$packColors["Armor"] = "6 7";
$packColors["Bucket"] = "7 8";
$packColors["Cape"] = "0 1 2 3 4 5 6 7 8";
$packColors["Pack"] = 8;
$packColors["Quiver"] = 8;
$packColors["Tank"] = "0 1 3 4 5 7";
$torsoColors["decalNone.png"] = "0 1 2 3 4 5 6 7 8";
$torsoColors["decalBtron.png"] = 4;
$torsoColors["decalDarth.png"] = 7;
$torsoColors["decalRedCoat.png"] = 4;
$torsoColors["decalBlueCoat.png"] = 4;
$torsoColors["decalMtron.png"] = 0;
$torsoColors["decalNewSpace.png"] = 4;
$torsoColors["decalOldSpace.png"] = "0 1 3 4 7";
$torsoColors["decalCastle1.png"] = "0 1 2 3 4 5 6 7 8";
$torsoColors["decalArmor.png"] = "0 1 2 3 4 5 6 7 8";
$torsoColors["decalPirate1.png"] = "0 1 2 3 4 5 6 7 8";
$torsoColors["decalBeads.png"] = "0 1 2 3 4 5 6 7 8";
$torsoColors["decalLion.png"] = "0 1 2 3 4 5 6 7 8";
$accentsAllowed["none"] = "none";
$accentsAllowed["helmet"] = "none visor";
$accentsAllowed["pointyHelmet"] = "none";
$accentsAllowed["scoutHat"] = "none plume";
$accentColors["visor"] = "7 9 10 11 12 13 14 15 16";
$accentColors["plume"] = "0 1 2 3 4 5 6 7";
$allColors = "0 1 2 3 4 5 6 7 8 17 18 19 20 21 22 23 24 25 26 27 28 9 10 11 12 13 14 15 16";
$normalColors = "0 1 2 3 4 5 6 7 8 17 18 19 20 21 22 23 24 25 26 27 28";
$basicColors = "0 1 2 3 4 5 6 7 8";
$accentColorsUnAuth["visor"] = $allColors;
$accentColorsUnAuth["plume"] = $normalColors;
function AvatarGui::onWake(%__unused)
{
	%x = getWord(Avatar_FacePreview.position, 0) + 64;
	%y = getWord(Avatar_FacePreview.position, 1);
	AvatarGui_CreatePartMenu("Avatar_FaceMenu", "Avatar_SetFace", "base/data/shapes/player/face.ifl", %x, %y);
	%x = getWord(Avatar_DecalPreview.position, 0) + 64;
	%y = getWord(Avatar_DecalPreview.position, 1) - 64;
	AvatarGui_CreatePartMenu("Avatar_DecalMenu", "Avatar_SetDecal", "base/data/shapes/player/decal.ifl", %x, %y);
	%x = getWord(Avatar_PackPreview.position, 0) + 64;
	%y = getWord(Avatar_PackPreview.position, 1) - 64;
	AvatarGui_CreatePartMenu("Avatar_PackMenu", "Avatar_SetPack", "base/data/shapes/player/Pack.txt", %x, %y);
	%x = getWord(Avatar_HatPreview.position, 0) + 64;
	%y = getWord(Avatar_HatPreview.position, 1);
	AvatarGui_CreatePartMenu("Avatar_HatMenu", "Avatar_SetHat", "base/data/shapes/player/Hat.txt", %x, %y);
	exec("base/data/shapes/player/player.cs");
	Avatar_Preview.setObject("", "base/data/shapes/player/m.dts", "", 100);
	Avatar_Preview.setSequence("", 0, headup, 0);
	Avatar_Preview.setThreadPos("", 0, 0);
	Avatar_UpdatePreview();
}

function AvatarGui::onSleep(%__unused)
{
}

function AvatarGui_CreatePartMenu(%name, %cmdString, %fileName, %xPos, %yPos)
{
	if (isObject(%name))
	{
		eval(%name @ ".delete();");
	}
	%newScroll = new GuiScrollCtrl();
	%newScroll.vScrollBar = "alwaysOn";
	%newScroll.hScrollBar = "alwaysOff";
	%newScroll.setProfile(ColorScrollProfile);
	Avatar_Window.add(%newScroll);
	%w = 64 + 12;
	%h = 64;
	%newScroll.resize(%xPos, %yPos, %w, %h);
	%newScroll.setName(%name);
	%newBox = new GuiSwatchCtrl();
	%newScroll.add(%newBox);
	%newBox.setColor("1 1 0 0.8");
	%newBox.resize(0, 0, 64, 64);
	%newBox.setName("Avatar_" @ fileBase(%fileName) @ "MenuBG");
	%file = new FileObject();
	%file.openForRead(%fileName);
	%itemCount = 0;
	%iconDir = "base/client/ui/avatarIcons/" @ fileBase(%fileName) @ "/";
	%varString = "$" @ fileBase(%fileName);
	for (%line = %file.readLine(); %line !$= ""; %line = %file.readLine())
	{
		%newImage = new GuiBitmapCtrl();
		%newBox.add(%newImage);
		%newImage.setBitmap(%iconDir @ %line);
		%x = (%itemCount % 4) * 64;
		%y = mFloor(%itemCount / 4) * 64;
		%newImage.resize(%x, %y, 64, 64);
		%newButton = new GuiBitmapButtonCtrl();
		%newBox.add(%newButton);
		%newButton.setBitmap("base/client/ui/btnDecal");
		%newButton.setText(" ");
		%newButton.resize(%x, %y, 64, 64);
		%newButton.command = %cmdString @ "(" @ %itemCount @ "," @ %newImage @ ");";
		%cmd = %varString @ "[" @ %itemCount @ "] = \"" @ %line @ "\";";
		eval(%cmd);
		%itemCount++;
	}
	$num[fileBase(%fileName)] = %itemCount - 1;
	%file.close();
	%file.delete();
	if (%itemCount >= 4)
	{
		%w = 4 * 64;
	}
	else
	{
		%w = %itemCount * 64;
	}
	%h = mFloor(%itemCount / 4 + 0.95) * 64;
	%newBox.resize(0, 0, %w, %h);
	if (%yPos + %h > 480)
	{
		%h = mFloor((480 - %yPos) / 64) * 64;
	}
	%newScroll.resize(%xPos, %yPos, %w + 12, %h);
	%newScroll.setVisible(0);
}

function AvatarGui_CreateSubPartMenu(%name, %cmdString, %subPartList, %xPos, %yPos)
{
	if (isObject(%name))
	{
		eval(%name @ ".delete();");
	}
	%baseName = strreplace(%name, "Menu", "");
	%baseName = strreplace(%baseName, "Avatar_", "");
	%newScroll = new GuiScrollCtrl();
	%newScroll.vScrollBar = "alwaysOn";
	%newScroll.hScrollBar = "alwaysOff";
	%newScroll.setProfile(ColorScrollProfile);
	Avatar_Window.add(%newScroll);
	%w = 64 + 12;
	%h = 64;
	%newScroll.resize(%xPos, %yPos, %w, %h);
	%newScroll.setName(%name);
	%newBox = new GuiSwatchCtrl();
	%newScroll.add(%newBox);
	%newBox.setColor("1 1 0 0.8");
	%newBox.resize(0, 0, 64, 64);
	%newBox.setName("Avatar_" @ %baseName @ "MenuBG");
	%iconDir = "base/client/ui/avatarIcons/" @ %baseName @ "/";
	%itemCount = 0;
	for (%line = getWord(%subPartList, %itemCount); %line !$= ""; %line = getWord(%subPartList, %itemCount))
	{
		echo("Accent line = ", %line);
		%newImage = new GuiBitmapCtrl();
		%newBox.add(%newImage);
		%newImage.setBitmap(%iconDir @ %line);
		%x = (%itemCount % 4) * 64;
		%y = mFloor(%itemCount / 4) * 64;
		%newImage.resize(%x, %y, 64, 64);
		%newButton = new GuiBitmapButtonCtrl();
		%newBox.add(%newButton);
		%newButton.setBitmap("base/client/ui/btnDecal");
		%newButton.setText(" ");
		%newButton.resize(%x, %y, 64, 64);
		%newButton.command = %cmdString @ "(" @ %itemCount @ "," @ %newImage @ ");";
		%itemCount++;
	}
	if (%itemCount >= 4)
	{
		%w = 4 * 64;
	}
	else
	{
		%w = %itemCount * 64;
	}
	%h = mFloor(%itemCount / 4 + 0.95) * 64;
	%newBox.resize(0, 0, %w, %h);
	if (%yPos + %h > 480)
	{
		%h = mFloor((480 - %yPos) / 64) * 64;
	}
	%newScroll.resize(%xPos, %yPos, %w + 12, %h);
	%newScroll.setVisible(0);
}

function AvatarGui_CreateColorMenu(%prefString, %colorList, %xPos, %yPos)
{
	%rowLimit = 6;
	if (isObject(Avatar_ColorMenu))
	{
		Avatar_ColorMenu.delete();
	}
	$CurrColorPrefString = %prefString;
	%newScroll = new GuiScrollCtrl();
	%newScroll.setProfile(ColorScrollProfile);
	%newScroll.vScrollBar = "alwaysOn";
	%newScroll.hScrollBar = "alwaysOff";
	Avatar_Window.add(%newScroll);
	%newScroll.resize(%xPos, %yPos, 32 + 12, 32);
	%newScroll.setName("Avatar_ColorMenu");
	%newBox = new GuiSwatchCtrl();
	%newScroll.add(%newBox);
	%newBox.setColor("0 0 0 1");
	%newBox.resize(0, 0, 32, 32);
	%itemCount = 0;
	for (%colorIndex = getWord(%colorList, %count); %colorIndex !$= ""; %colorIndex = getWord(%colorList, %itemCount))
	{
		%color = $minifigColor[%colorIndex];
		%newSwatch = new GuiSwatchCtrl();
		%newBox.add(%newSwatch);
		%newSwatch.setColor(%color);
		%x = (%itemCount % %rowLimit) * 32;
		%y = mFloor(%itemCount / %rowLimit) * 32;
		%newSwatch.resize(%x, %y, 32, 32);
		%newButton = new GuiBitmapButtonCtrl();
		%newBox.add(%newButton);
		%newButton.setBitmap("base/client/ui/btnColor");
		%newButton.setText(" ");
		%newButton.resize(%x, %y, 32, 32);
		%newButton.command = "Avatar_AssignColor(" @ %colorIndex @ ");";
		%itemCount++;
	}
	if (%itemCount >= %rowLimit)
	{
		%w = %rowLimit * 32;
	}
	else
	{
		%w = %itemCount * 32;
	}
	%h = (mFloor(%itemCount / %rowLimit) + 1) * 32;
	%newBox.resize(0, 0, %w, %h);
	if (%yPos + %h > 480)
	{
		%h = mFloor((480 - %yPos) / 32) * 32;
	}
	%newScroll.resize(%xPos, %yPos, %w + 12, %h);
}

function Avatar_AssignColor(%index)
{
	%cmd = $CurrColorPrefString @ " = " @ %index @ ";";
	eval(%cmd);
	$CurrColorPrefString = "";
	Avatar_ColorMenu.delete();
	Avatar_UpdatePreview();
}

function Avatar_ClickTorsoColor()
{
	%decalName = $decal[$pref::Player::DecalColor];
	%x = getWord(Avatar_TorsoColor.position, 0) + 32;
	%y = getWord(Avatar_TorsoColor.position, 1);
	if ($pref::Player::Authentic && $torsoColors[%decalName] !$= "")
	{
		AvatarGui_CreateColorMenu("$Pref::Player::TorsoColor", $torsoColors[%decalName], %x, %y);
	}
	else
	{
		AvatarGui_CreateColorMenu("$Pref::Player::TorsoColor", $normalColors, %x, %y);
	}
}

function Avatar_ClickPackColor()
{
	%packName = $pack[$pref::Player::Pack];
	%x = getWord(Avatar_PackColor.position, 0) + 32;
	%y = getWord(Avatar_PackColor.position, 1);
	if ($pref::Player::Authentic && $packColors[%packName] !$= "")
	{
		AvatarGui_CreateColorMenu("$Pref::Player::PackColor", $packColors[%packName], %x, %y);
	}
	else
	{
		AvatarGui_CreateColorMenu("$Pref::Player::PackColor", $normalColors, %x, %y);
	}
}

function Avatar_ClickHatColor()
{
	%hatName = $hat[$pref::Player::Hat];
	%x = getWord(Avatar_HatColor.position, 0) + 32;
	%y = getWord(Avatar_HatColor.position, 1);
	if ($pref::Player::Authentic && $hatColors[%hatName] !$= "")
	{
		AvatarGui_CreateColorMenu("$Pref::Player::HatColor", $hatColors[%hatName], %x, %y);
	}
	else
	{
		AvatarGui_CreateColorMenu("$Pref::Player::HatColor", $normalColors, %x, %y);
	}
}

function Avatar_ClickAccentColor()
{
	%hatName = $hat[$pref::Player::Hat];
	%AccentArray = $accentsAllowed[%hatName];
	%accentName = getWord(%AccentArray, $pref::Player::Accent);
	if (%accentName !$= "None")
	{
		%x = getWord(Avatar_AccentColor.position, 0) + 32;
		%y = getWord(Avatar_AccentColor.position, 1);
		if ($pref::Player::Authentic)
		{
			if ($accentColors[%accentName] $= "")
			{
				AvatarGui_CreateColorMenu("$Pref::Player::AccentColor", $allColors, %x, %y);
			}
			else
			{
				AvatarGui_CreateColorMenu("$Pref::Player::AccentColor", $accentColors[%accentName], %x, %y);
			}
		}
		else if ($accentColorsUnAuth[%accentName] $= "")
		{
			AvatarGui_CreateColorMenu("$Pref::Player::AccentColor", $allColors, %x, %y);
		}
		else
		{
			AvatarGui_CreateColorMenu("$Pref::Player::AccentColor", $accentColorsUnAuth[%accentName], %x, %y);
		}
	}
}

function Avatar_ClickHipColor()
{
	%x = getWord(Avatar_HipColor.position, 0) + 32;
	%y = getWord(Avatar_HipColor.position, 1);
	if ($pref::Player::Authentic)
	{
		AvatarGui_CreateColorMenu("$Pref::Player::HipColor", $basicColors, %x, %y);
	}
	else
	{
		AvatarGui_CreateColorMenu("$Pref::Player::HipColor", $normalColors, %x, %y);
	}
}

function Avatar_ClickRightLegColor()
{
	%x = getWord(Avatar_RightLegColor.position, 0) + 32;
	%y = getWord(Avatar_RightLegColor.position, 1);
	if ($pref::Player::Authentic)
	{
		AvatarGui_CreateColorMenu("$Pref::Player::RLegColor", $basicColors, %x, %y);
	}
	else
	{
		AvatarGui_CreateColorMenu("$Pref::Player::RLegColor", $normalColors, %x, %y);
	}
}

function Avatar_ClickRightArmColor()
{
	%x = getWord(Avatar_RightArmColor.position, 0) + 32;
	%y = getWord(Avatar_RightArmColor.position, 1);
	if ($pref::Player::Authentic)
	{
		AvatarGui_CreateColorMenu("$Pref::Player::RArmColor", $basicColors, %x, %y);
	}
	else
	{
		AvatarGui_CreateColorMenu("$Pref::Player::RArmColor", $normalColors, %x, %y);
	}
}

function Avatar_ClickRightHandColor()
{
	%x = getWord(Avatar_RightHandColor.position, 0) + 32;
	%y = getWord(Avatar_RightHandColor.position, 1);
	if ($pref::Player::Authentic)
	{
		AvatarGui_CreateColorMenu("$Pref::Player::RHandColor", $basicColors, %x, %y);
	}
	else
	{
		AvatarGui_CreateColorMenu("$Pref::Player::RHandColor", $normalColors, %x, %y);
	}
}

function Avatar_ClickLeftLegColor()
{
	%x = getWord(Avatar_LeftLegColor.position, 0) + 32;
	%y = getWord(Avatar_LeftLegColor.position, 1);
	if ($pref::Player::Authentic)
	{
		AvatarGui_CreateColorMenu("$Pref::Player::LLegColor", $basicColors, %x, %y);
	}
	else
	{
		AvatarGui_CreateColorMenu("$Pref::Player::LLegColor", $normalColors, %x, %y);
	}
}

function Avatar_ClickLeftArmColor()
{
	%x = getWord(Avatar_LeftArmColor.position, 0) + 32;
	%y = getWord(Avatar_LeftArmColor.position, 1);
	if ($pref::Player::Authentic)
	{
		AvatarGui_CreateColorMenu("$Pref::Player::LArmColor", $basicColors, %x, %y);
	}
	else
	{
		AvatarGui_CreateColorMenu("$Pref::Player::LArmColor", $normalColors, %x, %y);
	}
}

function Avatar_ClickLeftHandColor()
{
	%x = getWord(Avatar_LeftHandColor.position, 0) + 32;
	%y = getWord(Avatar_LeftHandColor.position, 1);
	if ($pref::Player::Authentic)
	{
		AvatarGui_CreateColorMenu("$Pref::Player::LHandColor", $basicColors, %x, %y);
	}
	else
	{
		AvatarGui_CreateColorMenu("$Pref::Player::LHandColor", $normalColors, %x, %y);
	}
}

function Avatar_ToggleFaceMenu()
{
	Avatar_FaceMenu.setVisible(!Avatar_FaceMenu.visible);
	Avatar_DecalMenu.setVisible(0);
	Avatar_PackMenu.setVisible(0);
	Avatar_HatMenu.setVisible(0);
	Avatar_AccentMenu.setVisible(0);
	if (isObject(Avatar_ColorMenu))
	{
		Avatar_ColorMenu.delete();
	}
}

function Avatar_ToggleDecalMenu()
{
	Avatar_FaceMenu.setVisible(0);
	Avatar_DecalMenu.setVisible(!Avatar_DecalMenu.visible);
	Avatar_PackMenu.setVisible(0);
	Avatar_HatMenu.setVisible(0);
	Avatar_AccentMenu.setVisible(0);
	if (isObject(Avatar_ColorMenu))
	{
		Avatar_ColorMenu.delete();
	}
}

function Avatar_TogglePackMenu()
{
	Avatar_FaceMenu.setVisible(0);
	Avatar_DecalMenu.setVisible(0);
	Avatar_PackMenu.setVisible(!Avatar_PackMenu.visible);
	Avatar_HatMenu.setVisible(0);
	Avatar_AccentMenu.setVisible(0);
	if (isObject(Avatar_ColorMenu))
	{
		Avatar_ColorMenu.delete();
	}
}

function Avatar_ToggleHatMenu()
{
	Avatar_FaceMenu.setVisible(0);
	Avatar_DecalMenu.setVisible(0);
	Avatar_PackMenu.setVisible(0);
	Avatar_HatMenu.setVisible(!Avatar_HatMenu.visible);
	Avatar_AccentMenu.setVisible(0);
	if (isObject(Avatar_ColorMenu))
	{
		Avatar_ColorMenu.delete();
	}
}

function Avatar_ToggleAccentMenu()
{
	Avatar_FaceMenu.setVisible(0);
	Avatar_DecalMenu.setVisible(0);
	Avatar_PackMenu.setVisible(0);
	Avatar_HatMenu.setVisible(0);
	Avatar_AccentMenu.setVisible(!Avatar_AccentMenu.visible);
	if (isObject(Avatar_ColorMenu))
	{
		Avatar_ColorMenu.delete();
	}
}

function Avatar_SetFace(%index, %__unused)
{
	$pref::Player::FaceColor = %index;
	Avatar_FaceMenu.setVisible(0);
	Avatar_UpdatePreview();
}

function Avatar_SetDecal(%index, %__unused)
{
	$pref::Player::DecalColor = %index;
	Avatar_DecalMenu.setVisible(0);
	Avatar_UpdatePreview();
}

function Avatar_SetPack(%index, %__unused)
{
	$pref::Player::Pack = %index;
	Avatar_PackMenu.setVisible(0);
	Avatar_UpdatePreview();
}

function Avatar_SetHat(%index, %__unused)
{
	$pref::Player::Hat = %index;
	Avatar_HatMenu.setVisible(0);
	%x = getWord(Avatar_AccentPreview.position, 0) + 64;
	%y = getWord(Avatar_AccentPreview.position, 1);
	AvatarGui_CreateSubPartMenu("Avatar_AccentMenu", "Avatar_SetAccent", $accentsAllowed[$hat[%index]], %x, %y);
	Avatar_UpdatePreview();
}

function Avatar_SetAccent(%index, %__unused)
{
	$pref::Player::Accent = %index;
	Avatar_AccentMenu.setVisible(0);
	Avatar_UpdatePreview();
}

function Avatar_UpdatePreview()
{
	Avatar_Preview.hideNode("", lski);
	Avatar_Preview.hideNode("", rski);
	Avatar_Preview.hideNode("", Armor);
	Avatar_Preview.hideNode("", Cape);
	Avatar_Preview.hideNode("", Pack);
	Avatar_Preview.hideNode("", Tank);
	Avatar_Preview.hideNode("", Bucket);
	Avatar_Preview.hideNode("", Quiver);
	Avatar_Preview.hideNode("", helmet);
	Avatar_Preview.hideNode("", scoutHat);
	Avatar_Preview.hideNode("", pointyHelmet);
	Avatar_Preview.hideNode("", bicorn);
	Avatar_Preview.hideNode("", plume);
	Avatar_Preview.hideNode("", visor);
	if ($pref::Player::Pack == 0)
	{
		Avatar_Preview.setThreadPos("", 0, 0);
	}
	else
	{
		Avatar_Preview.setThreadPos("", 0, 1);
	}
	Avatar_Preview.setNodeColor("", "LShoe", $minifigColor[$pref::Player::LLegColor]);
	Avatar_Preview.setNodeColor("", "RShoe", $minifigColor[$pref::Player::RLegColor]);
	Avatar_Preview.setNodeColor("", "Pants", $minifigColor[$pref::Player::HipColor]);
	Avatar_Preview.setNodeColor("", "Chest", $minifigColor[$pref::Player::TorsoColor]);
	Avatar_Preview.setNodeColor("", "Larm", $minifigColor[$pref::Player::LArmColor]);
	Avatar_Preview.setNodeColor("", "Rarm", $minifigColor[$pref::Player::RArmColor]);
	Avatar_Preview.setNodeColor("", "LHand", $minifigColor[$pref::Player::LHandColor]);
	Avatar_Preview.setNodeColor("", "RHand", $minifigColor[$pref::Player::RHandColor]);
	if ($hat[$pref::Player::Hat] !$= "" && $hat[$pref::Player::Hat] !$= "none")
	{
		Avatar_Preview.unHideNode("", $hat[$pref::Player::Hat]);
		Avatar_Preview.setNodeColor("", $hat[$pref::Player::Hat], $minifigColor[$pref::Player::HatColor]);
	}
	%partList = $accentsAllowed[$hat[$pref::Player::Hat]];
	%accentName = getWord(%partList, $pref::Player::Accent);
	if (%accentName !$= "" && %accentName !$= "none")
	{
		Avatar_Preview.unHideNode("", %accentName);
		Avatar_Preview.setNodeColor("", %accentName, $minifigColor[$pref::Player::AccentColor]);
	}
	if ($pack[$pref::Player::Pack] !$= "" && $pack[$pref::Player::Pack] !$= "none")
	{
		Avatar_Preview.unHideNode("", $pack[$pref::Player::Pack]);
		Avatar_Preview.setNodeColor("", $pack[$pref::Player::Pack], $minifigColor[$pref::Player::PackColor]);
	}
	Avatar_Preview.setIflFrame("", decal, $pref::Player::DecalColor);
	Avatar_Preview.setIflFrame("", face, $pref::Player::FaceColor);
	Avatar_TorsoColor.setColor($minifigColor[$pref::Player::TorsoColor]);
	Avatar_TorsoBG.setColor($minifigColor[$pref::Player::TorsoColor]);
	Avatar_DecalBG.setColor($minifigColor[$pref::Player::TorsoColor]);
	Avatar_DecalMenuBG.setColor($minifigColor[$pref::Player::TorsoColor]);
	Avatar_HipColor.setColor($minifigColor[$pref::Player::HipColor]);
	Avatar_HipBG.setColor($minifigColor[$pref::Player::HipColor]);
	Avatar_LeftArmColor.setColor($minifigColor[$pref::Player::LArmColor]);
	Avatar_LeftArmBG.setColor($minifigColor[$pref::Player::LArmColor]);
	Avatar_LeftHandColor.setColor($minifigColor[$pref::Player::LHandColor]);
	Avatar_LeftHandBG.setColor($minifigColor[$pref::Player::LHandColor]);
	Avatar_LeftLegColor.setColor($minifigColor[$pref::Player::LLegColor]);
	Avatar_LeftLegBG.setColor($minifigColor[$pref::Player::LLegColor]);
	Avatar_RightArmColor.setColor($minifigColor[$pref::Player::RArmColor]);
	Avatar_RightArmBG.setColor($minifigColor[$pref::Player::RArmColor]);
	Avatar_RightHandColor.setColor($minifigColor[$pref::Player::RHandColor]);
	Avatar_RightHandBG.setColor($minifigColor[$pref::Player::RHandColor]);
	Avatar_RightLegColor.setColor($minifigColor[$pref::Player::RLegColor]);
	Avatar_RightLegBG.setColor($minifigColor[$pref::Player::RLegColor]);
	Avatar_HatColor.setColor($minifigColor[$pref::Player::HatColor]);
	Avatar_HatBG.setColor($minifigColor[$pref::Player::HatColor]);
	Avatar_HatMenuBG.setColor($minifigColor[$pref::Player::HatColor]);
	Avatar_AccentColor.setColor($minifigColor[$pref::Player::AccentColor]);
	Avatar_AccentBG.setColor($minifigColor[$pref::Player::AccentColor]);
	Avatar_AccentMenuBG.setColor($minifigColor[$pref::Player::AccentColor]);
	Avatar_PackColor.setColor($minifigColor[$pref::Player::PackColor]);
	Avatar_PackBG.setColor($minifigColor[$pref::Player::PackColor]);
	Avatar_PackMenuBG.setColor($minifigColor[$pref::Player::PackColor]);
	Avatar_SecondPackColor.setColor($minifigColor[$pref::Player::SecondPackColor]);
	Avatar_SecondPackBG.setColor($minifigColor[$pref::Player::SecondPackColor]);
	Avatar_SecondPackMenuBG.setColor($minifigColor[$pref::Player::SecondPackColor]);
	%iconDir = "base/client/ui/avatarIcons/";
	Avatar_FacePreview.setBitmap(%iconDir @ "face/" @ $Face[$pref::Player::FaceColor]);
	Avatar_DecalPreview.setBitmap(%iconDir @ "decal/" @ $decal[$pref::Player::DecalColor]);
	Avatar_PackPreview.setBitmap(%iconDir @ "pack/" @ $pack[$pref::Player::Pack]);
	Avatar_HatPreview.setBitmap(%iconDir @ "hat/" @ $hat[$pref::Player::Hat]);
	%accentList = $accentsAllowed[$hat[$pref::Player::Hat]];
	%accent = getWord(%accentList, $pref::Player::Accent);
	Avatar_AccentPreview.setBitmap(%iconDir @ "accent/" @ %accent);
}

function Avatar_Randomize()
{
	$pref::Player::FaceColor = getRandom($numFace);
	$pref::Player::DecalColor = getRandom($numDecal);
	$pref::Player::Hat = getRandom($numHat);
	$pref::Player::Pack = getRandom($numPack);
	%partList = $accentsAllowed[$hat[$pref::Player::Hat]];
	%count = getWordCount(%partList) - 1;
	if (%count > 0)
	{
		%chance = getRandom(5);
		if (%chance != 0)
		{
			$pref::Player::Accent = getRandom(%count - 1) + 1;
		}
		else
		{
			$pref::Player::Accent = 0;
		}
	}
	else
	{
		$pref::Player::Accent = 0;
	}
	%x = getWord(Avatar_AccentPreview.position, 0) + 64;
	%y = getWord(Avatar_AccentPreview.position, 1);
	%hatName = $hat[$pref::Player::Hat];
	%AccentArray = $accentsAllowed[%hatName];
	%accentName = getWord(%AccentArray, $pref::Player::Accent);
	AvatarGui_CreateSubPartMenu("Avatar_AccentMenu", "Avatar_SetAccent", %AccentArray, %x, %y);
	if ($pref::Player::Authentic == 1)
	{
		%torsoColorList = $torsoColors[$decal[$pref::Player::DecalColor]];
		%packColorList = $packColors[$pack[$pref::Player::Pack]];
		%hatColorList = $hatColors[$hat[$pref::Player::Hat]];
		%accentColorList = $accentColors[%accentName];
		if (%torsoColorList $= "")
		{
			%torsoColorList = $basicColors;
		}
		if (%packColorList $= "")
		{
			%packColorList = $basicColors;
		}
		if (%hatColorList $= "")
		{
			%hatColorList = $basicColors;
		}
		if (%accentColorList $= "")
		{
			%accentColorList = $basicColors;
		}
		%hipColorList = $basicColors;
		%LLegColorList = $basicColors;
		%RLegColorList = $basicColors;
		%LArmColorList = $basicColors;
		%RArmColorList = $basicColors;
		%LHandColorList = $basicColors;
		%RHandColorList = $basicColors;
	}
	else
	{
		%torsoColorList = $normalColors;
		%packColorList = $normalColors;
		%hatColorList = $normalColors;
		%accentColorList = $accentColorsUnAuth[%accentName];
		%hipColorList = $normalColors;
		%LLegColorList = $normalColors;
		%RLegColorList = $normalColors;
		%LArmColorList = $normalColors;
		%RArmColorList = $normalColors;
		%LHandColorList = $normalColors;
		%RHandColorList = $normalColors;
	}
	%count = getWordCount(%torsoColorList) - 1;
	$pref::Player::TorsoColor = getWord(%torsoColorList, getRandom(%count));
	%count = getWordCount(%packColorList) - 1;
	$pref::Player::PackColor = getWord(%packColorList, getRandom(%count));
	%count = getWordCount(%hatColorList) - 1;
	$pref::Player::HatColor = getWord(%hatColorList, getRandom(%count));
	%count = getWordCount(%accentColorList) - 1;
	$pref::Player::AccentColor = getWord(%accentColorList, getRandom(%count));
	%count = getWordCount(%hipColorList) - 1;
	$pref::Player::HipColor = getWord(%hipColorList, getRandom(%count));
	%count = getWordCount(%LLegColorList) - 1;
	$pref::Player::LLegColor = getWord(%LLegColorList, getRandom(%count));
	%count = getWordCount(%LArmColorList) - 1;
	$pref::Player::LArmColor = getWord(%LArmColorList, getRandom(%count));
	%count = getWordCount(%LHandColorList) - 1;
	$pref::Player::LHandColor = getWord(%LHandColorList, getRandom(%count));
	if ($pref::Player::Symmetry == 1)
	{
		$pref::Player::RLegColor = $pref::Player::LLegColor;
		$pref::Player::RArmColor = $pref::Player::LArmColor;
		$pref::Player::RHandColor = $pref::Player::LHandColor;
	}
	else
	{
		%count = getWordCount(%RLegColorList) - 1;
		$pref::Player::RLegColor = getWord(%RLegColorList, getRandom(%count));
		%count = getWordCount(%RArmColorList) - 1;
		$pref::Player::RArmColor = getWord(%RArmColorList, getRandom(%count));
		%count = getWordCount(%RHandColorList) - 1;
		$pref::Player::RHandColor = getWord(%RHandColorList, getRandom(%count));
	}
	Avatar_UpdatePreview();
}

function Avatar_Done()
{
	Canvas.popDialog(AvatarGui);
	Canvas.popDialog(optionsDlg);
	clientCmdUpdatePrefs();
}

function Avatar_Clean()
{
	Avatar_FaceMenu.delete();
	Avatar_DecalMenu.delete();
	Avatar_HatMenu.delete();
	Avatar_PackMenu.delete();
	Avatar_SecondPackMenu.delete();
	Avatar_AccentMenu.delete();
	Avatar_ColorMenu.delete();
}

