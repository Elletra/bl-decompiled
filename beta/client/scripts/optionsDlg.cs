function optionsDlg::setPane(%this, %pane)
{
	OptAudioPane.setVisible(0);
	OptGraphicsPane.setVisible(0);
	OptNetworkPane.setVisible(0);
	OptControlsPane.setVisible(0);
	OptPlayerPane.setVisible(0);
	("Opt" @ %pane @ "Pane").setVisible(1);
	OptRemapList.fillList();
}

function loadAvatarPrefs()
{
	loadAvatarPref(decalBox, $pref::Player::DecalColor);
	loadAvatarPref(faceBox, $pref::Player::FaceColor);
	loadAvatarPref(hatBox, $pref::Player::Hat);
	loadAvatarPref(accentBox, $pref::Player::Accent);
	loadAvatarPref(packBox, $pref::Player::Pack);
	loadAvatarPref(hatColorBox, $pref::Player::HatColor);
	loadAvatarPref(accentColorBox, $pref::Player::AccentColor);
	loadAvatarPref(packColorBox, $pref::Player::PackColor);
	loadAvatarPref(LArmColorBox, $pref::Player::LArmColor);
	loadAvatarPref(RArmColorBox, $pref::Player::RArmColor);
	loadAvatarPref(LHandColorBox, $pref::Player::LHandColor);
	loadAvatarPref(RHandColorBox, $pref::Player::RHandColor);
	loadAvatarPref(LLegColorBox, $pref::Player::LLegColor);
	loadAvatarPref(RLegColorBox, $pref::Player::RLegColor);
	loadAvatarPref(TorsoColorBox, $pref::Player::TorsoColor);
	loadAvatarPref(HipColorBox, $pref::Player::HipColor);
}

function loadAvatarPref(%obj, %var)
{
	for (%i = 0; %i < %obj.itemCount; %i++)
	{
		%obj.radio[%i].setValue(0);
	}
	%obj.radio[%var].setValue(1);
}

function loadAvatarColorMenus()
{
	loadAvatarColorMenu("base/data/shapes/player/LArm.ifl", LArmColorBox, "$pref::Player::LArmColor", 16, ColorRadioProfile);
	loadAvatarColorMenu("base/data/shapes/player/RArm.ifl", RArmColorBox, "$pref::Player::RArmColor", 16, ColorRadioProfile);
	loadAvatarColorMenu("base/data/shapes/player/LHand.ifl", LHandColorBox, "$pref::Player::LHandColor", 16, ColorRadioProfile);
	loadAvatarColorMenu("base/data/shapes/player/RHand.ifl", RHandColorBox, "$pref::Player::RHandColor", 16, ColorRadioProfile);
	loadAvatarColorMenu("base/data/shapes/player/LLeg.ifl", LLegColorBox, "$pref::Player::LLegColor", 16, ColorRadioProfile);
	loadAvatarColorMenu("base/data/shapes/player/RLeg.ifl", RLegColorBox, "$pref::Player::RLegColor", 16, ColorRadioProfile);
	loadAvatarColorMenu("base/data/shapes/player/Chest.ifl", TorsoColorBox, "$pref::Player::TorsoColor", 16, ColorRadioProfile);
	loadAvatarColorMenu("base/data/shapes/player/pelvis.ifl", HipColorBox, "$pref::Player::HipColor", 16, ColorRadioProfile);
	loadAvatarColorMenu("base/data/shapes/player/hat.ifl", hatColorBox, "$pref::Player::HatColor", 16, ColorRadioProfile);
	loadAvatarColorMenu("base/data/shapes/player/accent.ifl", accentColorBox, "$pref::Player::AccentColor", 16, ColorRadioProfile);
	loadAvatarColorMenu("base/data/shapes/player/pack.ifl", packColorBox, "$pref::Player::PackColor", 16, ColorRadioProfile);
	loadAvatarColorMenu("base/data/shapes/player/decal.ifl", decalBox, "$pref::Player::DecalColor", 64, decalRadioProfile);
	loadAvatarColorMenu("base/data/shapes/player/face.ifl", faceBox, "$pref::Player::FaceColor", 64, decalRadioProfile);
	loadAvatarShapeMenu("packs", packBox, "$pref::Player::pack", 64, decalRadioProfile);
	loadAvatarShapeMenu("hats", hatBox, "$pref::Player::hat", 64, decalRadioProfile);
	loadAvatarShapeMenu("accents", accentBox, "$pref::Player::accent", 64, decalRadioProfile);
	loadDecalNames();
	%decalBoxWidth = getWord(decalBox.extent, 0);
	%faceBoxWidth = getWord(faceBox.extent, 0);
	if (%decalBoxWidth > %faceBoxWidth)
	{
		%x = 0;
		%y = 0;
		%w = %decalBoxWidth;
		%h = 64;
		faceBox.resize(%x, %y, %w, %h);
	}
	loadAvatarAccentColors();
}

function loadAvatarAccentColors()
{
	loadAvatarAccentColor("plume");
}

function loadAvatarAccentColor(%name)
{
	%file = new FileObject();
	%file.openForRead("base/data/shapes/player/decal.ifl");
	if (%file.isEOF())
	{
		return;
	}
	for (%lineCount = 0; !%file.isEOF(); %lineCount++)
	{
		%line = %file.readLine();
	}
	%file.delete();
}

function loadDecalNames()
{
	%file = new FileObject();
	%file.openForRead("base/data/shapes/player/decal.ifl");
	if (%file.isEOF())
	{
		return;
	}
	for (%lineCount = 0; !%file.isEOF(); %lineCount++)
	{
		%line = %file.readLine();
		$decals[%lineCount] = %line;
	}
	%file.delete();
}

function loadAvatarShapeMenu(%fileName, %objName, %varName, %size, %radioProfile)
{
	%file = new FileObject();
	%file.openForRead("base/data/shapes/player/" @ %fileName @ ".txt");
	if (%file.isEOF())
	{
		return;
	}
	for (%lineCount = 0; !%file.isEOF(); %lineCount++)
	{
		%line = %file.readLine();
		%newColor = new GuiBitmapCtrl();
		%objName.image[%lineCount] = %newColor;
		%objName.add(%newColor);
		%newColor.setBitmap("base/client/ui/" @ %fileName @ "/" @ %line);
		%x = (%size + 1) * %lineCount;
		%y = 0;
		%w = %size;
		%h = %size;
		%newColor.resize(%x, %y, %w, %h);
		%x = getWord(%objName.getPosition(), 0);
		%y = getWord(%objName.getPosition(), 1);
		%w = (%size + 1) * (%lineCount + 1);
		%h = %size;
		%objName.resize(%x, %y, %w, %h);
		%newRadio = new GuiRadioCtrl();
		%objName.radio[%lineCount] = %newRadio;
		%objName.add(%newRadio);
		%newRadio.setProfile(%radioProfile);
		%x = (%size + 1) * %lineCount;
		%y = 0;
		%w = %size;
		%h = %size;
		%newRadio.resize(%x, %y, %w, %h);
		%newRadio.text = "";
		%commandString = %varName @ " = \"" @ %lineCount @ "\"; PPUpdate();";
		%newRadio.command = %commandString;
		%command = "$" @ %fileName @ "[" @ %lineCount @ "] = " @ %line @ ";";
		eval(%command);
		%objName.itemCount++;
	}
	%file.delete();
}

function loadAvatarColorMenu(%fileName, %objName, %varName, %size, %radioProfile)
{
	%file = new FileObject();
	%file.openForRead(%fileName);
	if (%file.isEOF())
	{
		return;
	}
	for (%lineCount = 0; !%file.isEOF(); %lineCount++)
	{
		%line = %file.readLine();
		%newColor = new GuiBitmapCtrl();
		%objName.image[%lineCount] = %newColor;
		%objName.add(%newColor);
		if (fileBase(%fileName) $= "decal")
		{
			%newColor.setBitmap("base/data/shapes/player/decalIcons/" @ %line);
		}
		else if (fileBase(%fileName) $= "face")
		{
			%newColor.setBitmap("base/data/shapes/player/smileyIcons/" @ %line);
		}
		else
		{
			%newColor.setBitmap("base/data/shapes/player/" @ %line);
		}
		%x = (%size + 1) * %lineCount;
		%y = 0;
		%w = %size;
		%h = %size;
		%newColor.resize(%x, %y, %w, %h);
		%x = getWord(%objName.getPosition(), 0);
		%y = getWord(%objName.getPosition(), 1);
		%w = (%size + 1) * (%lineCount + 1);
		%h = %size;
		%objName.resize(%x, %y, %w, %h);
		%newRadio = new GuiRadioCtrl();
		%objName.radio[%lineCount] = %newRadio;
		%objName.add(%newRadio);
		%newRadio.setProfile(%radioProfile);
		%x = (%size + 1) * %lineCount;
		%y = 0;
		%w = %size;
		%h = %size;
		%newRadio.resize(%x, %y, %w, %h);
		%newRadio.text = "";
		%commandString = %varName @ " = \"" @ %lineCount @ "\"; PPUpdate();";
		%newRadio.command = %commandString;
		%objName.itemCount++;
	}
	%file.delete();
}

function killSelectionImages()
{
	killSelectionBox(decalBox);
	killSelectionBox(faceBox);
	killSelectionBox(hatBox);
	killSelectionBox(accentBox);
	killSelectionBox(packBox);
	killSelectionBox(hatColorBox);
	killSelectionBox(accentColorBox);
	killSelectionBox(packColorBox);
	killSelectionBox(LHandColorBox);
	killSelectionBox(RHandColorBox);
	killSelectionBox(LArmColorBox);
	killSelectionBox(RArmColorBox);
	killSelectionBox(LLegColorBox);
	killSelectionBox(RLegColorBox);
	killSelectionBox(TorsoColorBox);
	killSelectionBox(HipColorBox);
	killSelectionBox(hatColorBox);
	killSelectionBox(accentColorBox);
	killSelectionBox(packColorBox);
}

function killSelectionBox(%obj)
{
	for (%i = 0; %i < %obj.itemCount; %i++)
	{
		if (isObject(%obj.image[%i]))
		{
			%obj.image[%i].delete();
		}
		if (isObject(%obj.radio[%i]))
		{
			%obj.radio[%i].delete();
		}
		%obj.image[%i] = "";
		%obj.radio[%i] = "";
	}
	for (%obj.itemCount = ""; %obj.getCount() > 0; %obj.getObject(0).delete())
	{
	}
}

function PPHideAllNodes()
{
	playerPreview.hideNode("", lski);
	playerPreview.hideNode("", rski);
	playerPreview.hideNode("", armor);
	playerPreview.hideNode("", cape);
	playerPreview.hideNode("", pack);
	playerPreview.hideNode("", tank);
	playerPreview.hideNode("", bucket);
	playerPreview.hideNode("", quiver);
	playerPreview.hideNode("", helmet);
	playerPreview.hideNode("", scouthat);
	playerPreview.hideNode("", pointyhelmet);
	playerPreview.hideNode("", plume);
	playerPreview.hideNode("", visor);
}

function PPUpdate()
{
	PPHideAllNodes();
	$hatColorBlocked["scoutHat", "0"] = 1;
	$hatColorBlocked["scoutHat", "1"] = 1;
	$hatColorBlocked["scoutHat", "2"] = 0;
	$hatColorBlocked["scoutHat", "3"] = 1;
	$hatColorBlocked["scoutHat", "4"] = 1;
	$hatColorBlocked["scoutHat", "5"] = 1;
	$hatColorBlocked["scoutHat", "6"] = 1;
	$hatColorBlocked["scoutHat", "7"] = 1;
	$hatColorBlocked["scoutHat", "8"] = 0;
	$hatColorBlocked["pointyHelmet", "0"] = 1;
	$hatColorBlocked["pointyHelmet", "1"] = 1;
	$hatColorBlocked["pointyHelmet", "2"] = 1;
	$hatColorBlocked["pointyHelmet", "3"] = 1;
	$hatColorBlocked["pointyHelmet", "4"] = 1;
	$hatColorBlocked["pointyHelmet", "5"] = 1;
	$hatColorBlocked["pointyHelmet", "6"] = 0;
	$hatColorBlocked["pointyHelmet", "7"] = 0;
	$hatColorBlocked["pointyHelmet", "8"] = 1;
	$packColorBlocked["Armor", "0"] = 1;
	$packColorBlocked["Armor", "1"] = 1;
	$packColorBlocked["Armor", "2"] = 1;
	$packColorBlocked["Armor", "3"] = 1;
	$packColorBlocked["Armor", "4"] = 1;
	$packColorBlocked["Armor", "5"] = 1;
	$packColorBlocked["Armor", "6"] = 0;
	$packColorBlocked["Armor", "7"] = 0;
	$packColorBlocked["Armor", "8"] = 1;
	$packColorBlocked["Bucket", "0"] = 1;
	$packColorBlocked["Bucket", "1"] = 1;
	$packColorBlocked["Bucket", "2"] = 1;
	$packColorBlocked["Bucket", "3"] = 1;
	$packColorBlocked["Bucket", "4"] = 1;
	$packColorBlocked["Bucket", "5"] = 1;
	$packColorBlocked["Bucket", "6"] = 1;
	$packColorBlocked["Bucket", "7"] = 0;
	$packColorBlocked["Bucket", "8"] = 0;
	$packColorBlocked["Pack", "0"] = 1;
	$packColorBlocked["Pack", "1"] = 1;
	$packColorBlocked["Pack", "2"] = 1;
	$packColorBlocked["Pack", "3"] = 1;
	$packColorBlocked["Pack", "4"] = 1;
	$packColorBlocked["Pack", "5"] = 1;
	$packColorBlocked["Pack", "6"] = 1;
	$packColorBlocked["Pack", "7"] = 1;
	$packColorBlocked["Pack", "8"] = 0;
	$packColorBlocked["Quiver", "0"] = 1;
	$packColorBlocked["Quiver", "1"] = 1;
	$packColorBlocked["Quiver", "2"] = 1;
	$packColorBlocked["Quiver", "3"] = 1;
	$packColorBlocked["Quiver", "4"] = 1;
	$packColorBlocked["Quiver", "5"] = 1;
	$packColorBlocked["Quiver", "6"] = 1;
	$packColorBlocked["Quiver", "7"] = 1;
	$packColorBlocked["Quiver", "8"] = 0;
	$packColorBlocked["Tank", "0"] = 0;
	$packColorBlocked["Tank", "1"] = 0;
	$packColorBlocked["Tank", "2"] = 1;
	$packColorBlocked["Tank", "3"] = 0;
	$packColorBlocked["Tank", "4"] = 0;
	$packColorBlocked["Tank", "5"] = 0;
	$packColorBlocked["Tank", "6"] = 1;
	$packColorBlocked["Tank", "7"] = 0;
	$packColorBlocked["Tank", "8"] = 1;
	$torsoColorBlocked["decalBTron.png", "0"] = 1;
	$torsoColorBlocked["decalBTron.png", "1"] = 1;
	$torsoColorBlocked["decalBTron.png", "2"] = 1;
	$torsoColorBlocked["decalBTron.png", "3"] = 1;
	$torsoColorBlocked["decalBTron.png", "4"] = 0;
	$torsoColorBlocked["decalBTron.png", "5"] = 1;
	$torsoColorBlocked["decalBTron.png", "6"] = 1;
	$torsoColorBlocked["decalBTron.png", "7"] = 1;
	$torsoColorBlocked["decalBTron.png", "8"] = 1;
	$torsoColorBlocked["decalDarth.png", "0"] = 1;
	$torsoColorBlocked["decalDarth.png", "1"] = 1;
	$torsoColorBlocked["decalDarth.png", "2"] = 1;
	$torsoColorBlocked["decalDarth.png", "3"] = 1;
	$torsoColorBlocked["decalDarth.png", "4"] = 1;
	$torsoColorBlocked["decalDarth.png", "5"] = 1;
	$torsoColorBlocked["decalDarth.png", "6"] = 1;
	$torsoColorBlocked["decalDarth.png", "7"] = 0;
	$torsoColorBlocked["decalDarth.png", "8"] = 1;
	$torsoColorBlocked["decalRedCoat.png", "0"] = 1;
	$torsoColorBlocked["decalRedCoat.png", "1"] = 1;
	$torsoColorBlocked["decalRedCoat.png", "2"] = 1;
	$torsoColorBlocked["decalRedCoat.png", "3"] = 1;
	$torsoColorBlocked["decalRedCoat.png", "4"] = 0;
	$torsoColorBlocked["decalRedCoat.png", "5"] = 1;
	$torsoColorBlocked["decalRedCoat.png", "6"] = 1;
	$torsoColorBlocked["decalRedCoat.png", "7"] = 1;
	$torsoColorBlocked["decalRedCoat.png", "8"] = 1;
	$torsoColorBlocked["decalBlueCoat.png", "0"] = 1;
	$torsoColorBlocked["decalBlueCoat.png", "1"] = 1;
	$torsoColorBlocked["decalBlueCoat.png", "2"] = 1;
	$torsoColorBlocked["decalBlueCoat.png", "3"] = 1;
	$torsoColorBlocked["decalBlueCoat.png", "4"] = 0;
	$torsoColorBlocked["decalBlueCoat.png", "5"] = 1;
	$torsoColorBlocked["decalBlueCoat.png", "6"] = 1;
	$torsoColorBlocked["decalBlueCoat.png", "7"] = 1;
	$torsoColorBlocked["decalBlueCoat.png", "8"] = 1;
	$torsoColorBlocked["decalMtron.png", "0"] = 0;
	$torsoColorBlocked["decalMtron.png", "1"] = 1;
	$torsoColorBlocked["decalMtron.png", "2"] = 1;
	$torsoColorBlocked["decalMtron.png", "3"] = 1;
	$torsoColorBlocked["decalMtron.png", "4"] = 1;
	$torsoColorBlocked["decalMtron.png", "5"] = 1;
	$torsoColorBlocked["decalMtron.png", "6"] = 1;
	$torsoColorBlocked["decalMtron.png", "7"] = 1;
	$torsoColorBlocked["decalMtron.png", "8"] = 1;
	$torsoColorBlocked["decalNewSpace.png", "0"] = 1;
	$torsoColorBlocked["decalNewSpace.png", "1"] = 1;
	$torsoColorBlocked["decalNewSpace.png", "2"] = 1;
	$torsoColorBlocked["decalNewSpace.png", "3"] = 1;
	$torsoColorBlocked["decalNewSpace.png", "4"] = 0;
	$torsoColorBlocked["decalNewSpace.png", "5"] = 1;
	$torsoColorBlocked["decalNewSpace.png", "6"] = 1;
	$torsoColorBlocked["decalNewSpace.png", "7"] = 1;
	$torsoColorBlocked["decalNewSpace.png", "8"] = 1;
	$torsoColorBlocked["decalOldSpace.png", "0"] = 0;
	$torsoColorBlocked["decalOldSpace.png", "1"] = 0;
	$torsoColorBlocked["decalOldSpace.png", "2"] = 1;
	$torsoColorBlocked["decalOldSpace.png", "3"] = 0;
	$torsoColorBlocked["decalOldSpace.png", "4"] = 0;
	$torsoColorBlocked["decalOldSpace.png", "5"] = 1;
	$torsoColorBlocked["decalOldSpace.png", "6"] = 1;
	$torsoColorBlocked["decalOldSpace.png", "7"] = 0;
	$torsoColorBlocked["decalOldSpace.png", "8"] = 1;
	$accentBlocked["none", "none"] = 0;
	$accentBlocked["none", "plume"] = 1;
	$accentBlocked["none", "visor"] = 1;
	$accentBlocked["helmet", "none"] = 0;
	$accentBlocked["helmet", "plume"] = 1;
	$accentBlocked["helmet", "visor"] = 0;
	$accentBlocked["pointyHelmet", "none"] = 0;
	$accentBlocked["pointyHelmet", "plume"] = 1;
	$accentBlocked["pointyHelmet", "visor"] = 1;
	$accentBlocked["scoutHat", "none"] = 0;
	$accentBlocked["scoutHat", "plume"] = 0;
	$accentBlocked["scoutHat", "visor"] = 1;
	%hatName = $hats[$pref::Player::Hat];
	for (%i = 0; %i < hatColorBox.itemCount; %i++)
	{
		if (hatColorBox.radio[%i].getValue() == 1)
		{
			$pref::Player::HatColor = %i;
			break;
		}
	}
	for (%i = 0; %i < 9; %i++)
	{
		if ($hatColorBlocked[%hatName, %i] == 1)
		{
			hatColorBox.radio[%i].setActive(0);
		}
		else
		{
			hatColorBox.radio[%i].setActive(1);
		}
	}
	if ($hatColorBlocked[%hatName, $pref::Player::HatColor] == 1)
	{
		for (%i = 0; %i < 9; %i++)
		{
			if ($hatColorBlocked[%hatName, %i] == 0)
			{
				$pref::Player::HatColor = %i;
				break;
			}
		}
	}
	%packName = $packs[$pref::Player::Pack];
	for (%i = 0; %i < packColorBox.itemCount; %i++)
	{
		if (packColorBox.radio[%i].getValue() == 1)
		{
			$pref::Player::PackColor = %i;
			break;
		}
	}
	for (%i = 0; %i < 9; %i++)
	{
		if ($packColorBlocked[%packName, %i] == 1)
		{
			packColorBox.radio[%i].setActive(0);
		}
		else
		{
			packColorBox.radio[%i].setActive(1);
		}
	}
	if ($packColorBlocked[%packName, $pref::Player::PackColor] == 1)
	{
		for (%i = 0; %i < 9; %i++)
		{
			if ($packColorBlocked[%packName, %i] == 0)
			{
				$pref::Player::PackColor = %i;
				break;
			}
		}
	}
	%decalName = $decals[$pref::Player::DecalColor];
	for (%i = 0; %i < TorsoColorBox.itemCount; %i++)
	{
		if (TorsoColorBox.radio[%i].getValue() == 1)
		{
			$pref::Player::TorsoColor = %i;
			break;
		}
	}
	for (%i = 0; %i < TorsoColorBox.itemCount; %i++)
	{
		if ($torsoColorBlocked[%decalName, %i] == 1)
		{
			TorsoColorBox.radio[%i].setActive(0);
		}
		else
		{
			TorsoColorBox.radio[%i].setActive(1);
		}
	}
	if ($torsoColorBlocked[%decalName, $pref::Player::TorsoColor] == 1)
	{
		for (%i = 0; %i < TorsoColorBox.itemCount; %i++)
		{
			if ($torsoColorBlocked[%decalName, %i] == 0)
			{
				$pref::Player::TorsoColor = %i;
				break;
			}
		}
	}
	for (%i = 0; %i < accentBox.itemCount; %i++)
	{
		if (accentBox.radio[%i].getValue() == 1)
		{
			$pref::Player::Accent = %i;
			break;
		}
	}
	%hatName = $hats[$pref::Player::Hat];
	%accentName = $accents[$pref::Player::Accent];
	for (%i = 0; %i < accentBox.itemCount; %i++)
	{
		if ($accentBlocked[%hatName, $accents[%i]] == 1)
		{
			accentBox.radio[%i].setActive(0);
		}
		else
		{
			accentBox.radio[%i].setActive(1);
		}
	}
	if ($accentBlocked[%hatName, %accentName] == 1)
	{
		for (%i = 0; %i < accentBox.itemCount; %i++)
		{
			if ($accentBlocked[%hatName, $accents[%i]] == 0)
			{
				$pref::Player::Accent = %i;
				break;
			}
		}
	}
	for (%i = 0; %i < accentColorBox.itemCount; %i++)
	{
		if (accentColorBox.radio[%i].getValue() == 1)
		{
			$pref::Player::AccentColor = %i;
			break;
		}
	}
	$hatColors["none"] = "";
	$hatColors["helmet"] = "0 1 2 3 4 5 6 7 8";
	$hatColors["scoutHat"] = "2 7";
	$hatColors["pointyHelmet"] = "6 7";
	$packColors["none"] = "";
	$packColors["Armor"] = "6 7";
	$packColors["Bucket"] = "7 8";
	$packColors["Cape"] = "0 1 2 3 4 5 6 7 8";
	$packColors["Pack"] = 8;
	$packColors["Quiver"] = 8;
	$packColors["Tank"] = "0 1 3 4 5 7";
	$torsoColors["decalNone.png"] = "0 1 2 3 4 5 6 7 8 9";
	$torsoColors["decalBtron.png"] = 4;
	$torsoColors["decalDarth.png"] = 7;
	$torsoColors["decalRedCoat.png"] = 4;
	$torsoColors["decalBlueCoat.png"] = 4;
	$torsoColors["decalMtron.png"] = 0;
	$torsoColors["decalNewSpace.png"] = 4;
	$torsoColors["decalOldSpace.png"] = "0 1 3 4 7";
	$torsoColors["decalCastle1.png"] = "0 1 2 3 4 5 6 7 8 9";
	$torsoColors["decalArmor.png"] = "0 1 2 3 4 5 6 7 8 9";
	$torsoColors["decalPirate1.png"] = "0 1 2 3 4 5 6 7 8 9";
	$torsoColors["decalBeads.png"] = "0 1 2 3 4 5 6 7 8 9";
	$torsoColors["decalLion.png"] = "0 1 2 3 4 5 6 7 8 9";
	$accentsAllowed["none"] = "none";
	$accentsAllowed["helmet"] = "none visor";
	$accentsAllowed["pointyHelmet"] = "none";
	$accentsAllowed["scoutHat"] = "none plume";
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
	if ($hats[$pref::Player::Hat] !$= "none")
	{
		playerPreview.unHideNode("", $hats[$pref::Player::Hat]);
	}
	if ($accents[$pref::Player::Accent] !$= "none")
	{
		playerPreview.unHideNode("", $accents[$pref::Player::Accent]);
	}
	if ($packs[$pref::Player::Pack] !$= "none")
	{
		playerPreview.unHideNode("", $packs[$pref::Player::Pack]);
	}
	if ($pref::Player::Pack == 0)
	{
		playerPreview.setThreadPos("", 0, 0);
	}
	else
	{
		playerPreview.setThreadPos("", 0, 1);
	}
	playerPreview.setNodeColor("", "LeftLeg", $minifigColor[$pref::Player::LLegColor]);
	playerPreview.setNodeColor("", "RightLeg", $minifigColor[$pref::Player::RLegColor]);
	playerPreview.setNodeColor("", "Hip", $minifigColor[$pref::Player::HipColor]);
	playerPreview.setNodeColor("", "Torso", $minifigColor[$pref::Player::TorsoColor]);
	playerPreview.setNodeColor("", "LeftArm", $minifigColor[$pref::Player::LArmColor]);
	playerPreview.setNodeColor("", "RightArm", $minifigColor[$pref::Player::RArmColor]);
	playerPreview.setNodeColor("", "LeftHand", $minifigColor[$pref::Player::LHandColor]);
	playerPreview.setNodeColor("", "RightHand", $minifigColor[$pref::Player::RHandColor]);
	playerPreview.setNodeColor("", $hats[$pref::Player::Hat], $minifigColor[$pref::Player::HatColor]);
	playerPreview.setNodeColor("", $accents[$pref::Player::Accent], $minifigColor[$pref::Player::AccentColor]);
	playerPreview.setNodeColor("", $packs[$pref::Player::Pack], $minifigColor[$pref::Player::PackColor]);
	playerPreview.setIflFrame("", decal, $pref::Player::DecalColor);
	playerPreview.setIflFrame("", face, $pref::Player::FaceColor);
}

function menuChange()
{
	%hatSelection = menuHat.getSelected();
	%accentSelection = menuAccent.getSelected();
	%accentColorSelection = menuAccentColor.getSelected();
	menuAccent.clear();
	menuAccent.add("None", 0);
	if (%hatSelection == 1)
	{
		menuAccent.add("Tri-plume", 1);
	}
	if (%hatSelection == 2)
	{
		menuAccent.add("Visor", 1);
	}
	if (%accentSelection < menuAccent.size())
	{
		menuAccent.setSelected(%accentSelection);
	}
	else
	{
		menuAccent.setSelected(0);
	}
	%accentSelection = menuAccent.getSelected();
	%accentColorSelection = menuAccentColor.getSelected();
	menuAccentColor.clear();
	if (%accentSelection == 0)
	{
		fillColorList(menuAccentColor);
	}
	if (%hatSelection == 1 && %accentSelection == 1)
	{
		fillColorList(menuAccentColor);
	}
	if (%hatSelection == 2 && %accentSelection == 1)
	{
		menuAccentColor.add("Red", 0);
		menuAccentColor.add("Yellow", 1);
		menuAccentColor.add("Green", 2);
		menuAccentColor.add("Blue", 3);
		menuAccentColor.add("Light Blue", 4);
		menuAccentColor.add("Clear", 5);
		menuAccentColor.add("Black", 6);
	}
	if (%accentColorSelection < menuAccentColor.size())
	{
		menuAccentColor.setSelected(%accentColorSelection);
	}
	else
	{
		menuAccentColor.setSelected(0);
	}
	PPUpdate();
}

function optionsDlg::onWake(%this)
{
	%this.setPane(Graphics);
	OPT_SSSmartToggle.setVisible($pref::Input::UseSuperShiftToggle);
	%buffer = getDisplayDeviceList();
	%count = getFieldCount(%buffer);
	OptGraphicsDriverMenu.clear();
	OptScreenshotMenu.init();
	OptScreenshotMenu.setValue($pref::Video::screenShotFormat);
	for (%i = 0; %i < %count; %i++)
	{
		OptGraphicsDriverMenu.add(getField(%buffer, %i), %i);
	}
	%selId = OptGraphicsDriverMenu.findText($pref::Video::displayDevice);
	if (%selId == -1)
	{
		%selId = 0;
	}
	OptGraphicsDriverMenu.setSelected(%selId);
	OptGraphicsDriverMenu.onSelect(%selId, "");
	OptAudioUpdate();
	OptAudioVolumeMaster.setValue($pref::Audio::masterVolume);
	OptAudioVolumeShell.setValue($pref::Audio::channelVolume[$GuiAudioType]);
	OptAudioVolumeSim.setValue($pref::Audio::channelVolume[$SimAudioType]);
	OptAudioDriverList.clear();
	OptAudioDriverList.add("OpenAL", 1);
	OptAudioDriverList.add("none", 2);
	%selId = OptAudioDriverList.findText($pref::Audio::driver);
	if (%selId == -1)
	{
		%selId = 0;
	}
	OptAudioDriverList.setSelected(%selId);
	OptAudioDriverList.onSelect(%selId, "");
	killSelectionImages();
	SliderControlsMouseSensitivity.setValue($pref::Input::MouseSensitivity);
	SliderGraphicsAnisotropy.setValue($pref::OpenGL::anisotropy);
	SliderGraphicsDistanceMod.setValue($pref::visibleDistanceMod);
	PacketSizeDisplay.setValue($pref::Net::PacketSize);
	SliderPacketSize.setValue($pref::Net::PacketSize);
	LagThresholdDisplay.setValue($Pref::Net::LagThreshold);
	SliderLagThreshold.setValue($Pref::Net::LagThreshold);
	RateToClientDisplay.setValue($pref::Net::PacketRateToClient);
	SliderRateToClient.setValue($pref::Net::PacketRateToClient);
	RateToServerDisplay.setValue($pref::Net::PacketRateToServer);
	SliderRateToServer.setValue($pref::Net::PacketRateToServer);
	TxtMasterServer.setValue($pref::Master0);
}

function optionsDlg::onSleep(%this)
{
	moveMap.save("base/client/config.cs");
	$pref::Input::MouseSensitivity = SliderControlsMouseSensitivity.getValue();
	$pref::OpenGL::anisotropy = SliderGraphicsAnisotropy.getValue();
	$pref::visibleDistanceMod = SliderGraphicsDistanceMod.getValue();
	$pref::Master0 = TxtMasterServer.getValue();
	clientCmdUpdatePrefs();
	killSelectionImages();
	PlayGui.createInvHUD();
	PlayGui.createToolHUD();
	PlayGui.loadPaint();
}

function OptPlayerHeadMenu::onSelect(%this, %__unused, %text)
{
	%headcode = OptPlayerHeadMenu.getSelected();
	OptPlayerVisorMenu.clear();
	OptPlayerVisorMenu.add("None", -1);
	OptPlayerVisorMenu.setSelected(-1);
	if (%headcode == 0)
	{
		OptPlayerVisorMenu.add("Visor", 1);
	}
	else if (%headcode == 1)
	{
		OptPlayerVisorMenu.add("Tri-Plume", 0);
	}
	%text = OptPlayerVisorMenu.getTextById($pref::Accessory::visorCode);
	if (%text !$= "")
	{
		OptPlayerVisorMenu.setSelected($pref::Accessory::visorCode);
	}
}

function UpdatePacketSize()
{
	PacketSizeDisplay.setValue(mFloor(SliderPacketSize.getValue()));
	$pref::Net::PacketSize = PacketSizeDisplay.getValue();
}

function UpdateLagThreshold()
{
	LagThresholdDisplay.setValue(mFloor(SliderLagThreshold.getValue()));
	$Pref::Net::LagThreshold = LagThresholdDisplay.getValue();
}

function UpdateRateToClient()
{
	RateToClientDisplay.setValue(mFloor(SliderRateToClient.getValue()));
	$pref::Net::PacketRateToClient = RateToClientDisplay.getValue();
}

function UpdateRateToServer()
{
	RateToServerDisplay.setValue(mFloor(SliderRateToServer.getValue()));
	$pref::Net::PacketRateToServer = RateToServerDisplay.getValue();
}

function OptGraphicsDriverMenu::onSelect(%this, %__unused, %text)
{
	if (OptGraphicsResolutionMenu.size() > 0)
	{
		%prevRes = OptGraphicsResolutionMenu.getText();
	}
	else
	{
		%prevRes = getWords($pref::Video::resolution, 0, 1);
	}
	if (isDeviceFullScreenOnly(%this.getText()))
	{
		OptGraphicsFullscreenToggle.setValue(1);
		OptGraphicsFullscreenToggle.setActive(0);
		OptGraphicsFullscreenToggle.onAction();
	}
	else
	{
		OptGraphicsFullscreenToggle.setActive(1);
	}
	if (OptGraphicsFullscreenToggle.getValue())
	{
		if (OptGraphicsBPPMenu.size() > 0)
		{
			%prevBPP = OptGraphicsBPPMenu.getText();
		}
		else
		{
			%prevBPP = getWord($pref::Video::resolution, 2);
		}
	}
	OptGraphicsResolutionMenu.init(%this.getText(), OptGraphicsFullscreenToggle.getValue());
	OptGraphicsBPPMenu.init(%this.getText());
	%selId = OptGraphicsResolutionMenu.findText(%prevRes);
	if (%selId == -1)
	{
		%selId = 0;
	}
	OptGraphicsResolutionMenu.setSelected(%selId);
	if (OptGraphicsFullscreenToggle.getValue())
	{
		%selId = OptGraphicsBPPMenu.findText(%prevBPP);
		if (%selId == -1)
		{
			%selId = 0;
		}
		OptGraphicsBPPMenu.setSelected(%selId);
		OptGraphicsBPPMenu.setText(OptGraphicsBPPMenu.getTextById(%selId));
	}
	else
	{
		OptGraphicsBPPMenu.setText("Default");
	}
}

function OptGraphicsResolutionMenu::init(%this, %device, %fullScreen)
{
	%this.clear();
	%resList = getResolutionList(%device);
	%resCount = getFieldCount(%resList);
	%deskRes = getDesktopResolution();
	%count = 0;
	for (%i = 0; %i < %resCount; %i++)
	{
		%res = getWords(getField(%resList, %i), 0, 1);
		if (!%fullScreen)
		{
			if (firstWord(%res) >= firstWord(%deskRes))
			{
				continue;
			}
			if (getWord(%res, 1) >= getWord(%deskRes, 1))
			{
				continue;
			}
		}
		if (%this.findText(%res) == -1)
		{
			%this.add(%res, %count);
			%count++;
		}
	}
}

function OptGraphicsFullscreenToggle::onAction(%this)
{
	Parent::onAction();
	%prevRes = OptGraphicsResolutionMenu.getText();
	OptGraphicsResolutionMenu.init(OptGraphicsDriverMenu.getText(), %this.getValue());
	%selId = OptGraphicsResolutionMenu.findText(%prevRes);
	if (%selId == -1)
	{
		%selId = 0;
	}
	OptGraphicsResolutionMenu.setSelected(%selId);
}

function OptGraphicsBPPMenu::init(%this, %device)
{
	%this.clear();
	if (%device $= "Voodoo2")
	{
		%this.add(16, 0);
	}
	else
	{
		%resList = getResolutionList(%device);
		%resCount = getFieldCount(%resList);
		%count = 0;
		for (%i = 0; %i < %resCount; %i++)
		{
			%bpp = getWord(getField(%resList, %i), 2);
			if (%this.findText(%bpp) == -1)
			{
				%this.add(%bpp, %count);
				%count++;
			}
		}
	}
}

function OptScreenshotMenu::init(%this)
{
	if (%this.findText("PNG") == -1)
	{
		%this.add("PNG", 0);
	}
	if (%this.findText("JPEG") == -1)
	{
		%this.add("JPEG", 1);
	}
}

function optionsDlg::applyGraphics(%this)
{
	%newDriver = OptGraphicsDriverMenu.getText();
	%newRes = OptGraphicsResolutionMenu.getText();
	%newBpp = OptGraphicsBPPMenu.getText();
	%newFullScreen = OptGraphicsFullscreenToggle.getValue();
	$pref::Video::screenShotFormat = OptScreenshotMenu.getText();
	if (%newDriver !$= $pref::Video::displayDevice)
	{
		setDisplayDevice(%newDriver, firstWord(%newRes), getWord(%newRes, 1), %newBpp, %newFullScreen);
	}
	else
	{
		setScreenMode(firstWord(%newRes), getWord(%newRes, 1), %newBpp, %newFullScreen);
	}
}

function optionsDlg::applyPlayer(%this)
{
}

$RemapCount = 0;
$RemapName[$RemapCount] = "Forward";
$RemapCmd[$RemapCount] = "moveforward";
$RemapCount++;
$RemapName[$RemapCount] = "Backward";
$RemapCmd[$RemapCount] = "movebackward";
$RemapCount++;
$RemapName[$RemapCount] = "Strafe Left";
$RemapCmd[$RemapCount] = "moveleft";
$RemapCount++;
$RemapName[$RemapCount] = "Strafe Right";
$RemapCmd[$RemapCount] = "moveright";
$RemapCount++;
$RemapName[$RemapCount] = "Turn Left";
$RemapCmd[$RemapCount] = "turnLeft";
$RemapCount++;
$RemapName[$RemapCount] = "Turn Right";
$RemapCmd[$RemapCount] = "turnRight";
$RemapCount++;
$RemapName[$RemapCount] = "Look Up";
$RemapCmd[$RemapCount] = "panUp";
$RemapCount++;
$RemapName[$RemapCount] = "Look Down";
$RemapCmd[$RemapCount] = "panDown";
$RemapCount++;
$RemapName[$RemapCount] = "Jump";
$RemapCmd[$RemapCount] = "jump";
$RemapCount++;
$RemapName[$RemapCount] = "Crouch";
$RemapCmd[$RemapCount] = "crouch";
$RemapCount++;
$RemapName[$RemapCount] = "Jet";
$RemapCmd[$RemapCount] = "jet";
$RemapCount++;
$RemapName[$RemapCount] = "Fire Weapon";
$RemapCmd[$RemapCount] = "mouseFire";
$RemapCount++;
$RemapName[$RemapCount] = "Adjust Zoom";
$RemapCmd[$RemapCount] = "setZoomFov";
$RemapCount++;
$RemapName[$RemapCount] = "Toggle Zoom";
$RemapCmd[$RemapCount] = "toggleZoom";
$RemapCount++;
$RemapName[$RemapCount] = "Free Look";
$RemapCmd[$RemapCount] = "toggleFreeLook";
$RemapCount++;
$RemapName[$RemapCount] = "Switch 1st/3rd";
$RemapCmd[$RemapCount] = "toggleFirstPerson";
$RemapCount++;
$RemapName[$RemapCount] = "Global Chat";
$RemapCmd[$RemapCount] = "GlobalChat";
$RemapCount++;
$RemapName[$RemapCount] = "Team Chat";
$RemapCmd[$RemapCount] = "TeamChat";
$RemapCount++;
$RemapName[$RemapCount] = "Message Hud PageUp";
$RemapCmd[$RemapCount] = "PageUpNewChatHud";
$RemapCount++;
$RemapName[$RemapCount] = "Message Hud PageDown";
$RemapCmd[$RemapCount] = "PageDownNewChatHud";
$RemapCount++;
$RemapName[$RemapCount] = "Drop Camera at Player";
$RemapCmd[$RemapCount] = "dropCameraAtPlayer";
$RemapCount++;
$RemapName[$RemapCount] = "Drop Player at Camera";
$RemapCmd[$RemapCount] = "dropPlayerAtCamera";
$RemapCount++;
$RemapName[$RemapCount] = "Use Tools";
$RemapCmd[$RemapCount] = "useTools";
$RemapCount++;
$RemapName[$RemapCount] = "Use Spray Can";
$RemapCmd[$RemapCount] = "useSprayCan";
$RemapCount++;
$RemapName[$RemapCount] = "Drop Tool";
$RemapCmd[$RemapCount] = "dropTool";
$RemapCount++;
$RemapName[$RemapCount] = "Use 1st Slot";
$RemapCmd[$RemapCount] = "useFirstSlot";
$RemapCount++;
$RemapName[$RemapCount] = "Use 2nd Slot";
$RemapCmd[$RemapCount] = "useSecondSlot";
$RemapCount++;
$RemapName[$RemapCount] = "Use 3rd Slot";
$RemapCmd[$RemapCount] = "useThirdSlot";
$RemapCount++;
$RemapName[$RemapCount] = "Use 4th Slot";
$RemapCmd[$RemapCount] = "useFourthSlot";
$RemapCount++;
$RemapName[$RemapCount] = "Use 5th Slot";
$RemapCmd[$RemapCount] = "useFifthSlot";
$RemapCount++;
$RemapName[$RemapCount] = "Use 6th Slot";
$RemapCmd[$RemapCount] = "useSixthSlot";
$RemapCount++;
$RemapName[$RemapCount] = "Use 7th Slot";
$RemapCmd[$RemapCount] = "useSeventhSlot";
$RemapCount++;
$RemapName[$RemapCount] = "Use 8th Slot";
$RemapCmd[$RemapCount] = "useEighthSlot";
$RemapCount++;
$RemapName[$RemapCount] = "Use 9th Slot";
$RemapCmd[$RemapCount] = "useNinthSlot";
$RemapCount++;
$RemapName[$RemapCount] = "Use 10th Slot";
$RemapCmd[$RemapCount] = "useTenthSlot";
$RemapCount++;
$RemapName[$RemapCount] = "Shift Brick Away";
$RemapCmd[$RemapCount] = "shiftBrickAway";
$RemapCount++;
$RemapName[$RemapCount] = "Shift Brick Towards";
$RemapCmd[$RemapCount] = "shiftBrickTowards";
$RemapCount++;
$RemapName[$RemapCount] = "Shift Brick Left";
$RemapCmd[$RemapCount] = "shiftBrickLeft";
$RemapCount++;
$RemapName[$RemapCount] = "Shift Brick Right";
$RemapCmd[$RemapCount] = "shiftBrickRight";
$RemapCount++;
$RemapName[$RemapCount] = "Shift Brick Up";
$RemapCmd[$RemapCount] = "shiftBrickUp";
$RemapCount++;
$RemapName[$RemapCount] = "Shift Brick Down";
$RemapCmd[$RemapCount] = "shiftBrickDown";
$RemapCount++;
$RemapName[$RemapCount] = "Shift Brick Up 1/3";
$RemapCmd[$RemapCount] = "shiftBrickThirdUp";
$RemapCount++;
$RemapName[$RemapCount] = "Shift Brick Down 1/3";
$RemapCmd[$RemapCount] = "shiftBrickThirdDown";
$RemapCount++;
$RemapName[$RemapCount] = "Rotate Brick CW";
$RemapCmd[$RemapCount] = "RotateBrickCW";
$RemapCount++;
$RemapName[$RemapCount] = "Rotate Brick CCW";
$RemapCmd[$RemapCount] = "RotateBrickCCW";
$RemapCount++;
$RemapName[$RemapCount] = "Plant Brick";
$RemapCmd[$RemapCount] = "plantBrick";
$RemapCount++;
$RemapName[$RemapCount] = "Undo Brick";
$RemapCmd[$RemapCount] = "undoBrick";
$RemapCount++;
$RemapName[$RemapCount] = "Cancel Brick";
$RemapCmd[$RemapCount] = "cancelBrick";
$RemapCount++;
$RemapName[$RemapCount] = "Super Shift Brick Away";
$RemapCmd[$RemapCount] = "superShiftBrickAwayProxy";
$RemapCount++;
$RemapName[$RemapCount] = "Super Shift Brick Towards";
$RemapCmd[$RemapCount] = "superShiftBrickTowardsProxy";
$RemapCount++;
$RemapName[$RemapCount] = "Super Shift Brick Left";
$RemapCmd[$RemapCount] = "superShiftBrickLeftProxy";
$RemapCount++;
$RemapName[$RemapCount] = "Super Shift Brick Right";
$RemapCmd[$RemapCount] = "superShiftBrickRightProxy";
$RemapCount++;
$RemapName[$RemapCount] = "Super Shift Brick Up";
$RemapCmd[$RemapCount] = "superShiftBrickUpProxy";
$RemapCount++;
$RemapName[$RemapCount] = "Super Shift Brick Down";
$RemapCmd[$RemapCount] = "superShiftBrickDownProxy";
$RemapCount++;
$RemapName[$RemapCount] = "Toggle Super Shift";
$RemapCmd[$RemapCount] = "toggleSuperShift";
$RemapCount++;
$RemapName[$RemapCount] = "Open Admin Window";
$RemapCmd[$RemapCount] = "openAdminWindow";
$RemapCount++;
$RemapName[$RemapCount] = "Open Options Window";
$RemapCmd[$RemapCount] = "openOptionsWindow";
$RemapCount++;
$RemapName[$RemapCount] = "Take Screenshot";
$RemapCmd[$RemapCount] = "doScreenShot";
$RemapCount++;
$RemapName[$RemapCount] = "Take Panoramic Screenshot";
$RemapCmd[$RemapCount] = "doPanoramaScreenShot";
$RemapCount++;
$RemapName[$RemapCount] = "Show Player List";
$RemapCmd[$RemapCount] = "showPlayerList";
$RemapCount++;
$RemapName[$RemapCount] = "Open Brick Selector";
$RemapCmd[$RemapCount] = "openBSD";
$RemapCount++;
function restoreDefaultMappings()
{
	moveMap.delete();
	exec("base/client/scripts/default.bind.cs");
	OptRemapList.fillList();
}

function getMapDisplayName(%device, %action)
{
	if (%device $= "keyboard")
	{
		return %action;
	}
	else if (strstr(%device, "mouse") != -1)
	{
		%pos = strstr(%action, "button");
		if (%pos != -1)
		{
			%mods = getSubStr(%action, 0, %pos);
			%object = getSubStr(%action, %pos, 1000);
			%instance = getSubStr(%object, strlen("button"), 1000);
			return %mods @ "mouse" @ %instance + 1;
		}
		else
		{
			error("Mouse input object other than button passed to getDisplayMapName!");
		}
	}
	else if (strstr(%device, "joystick") != -1)
	{
		%pos = strstr(%action, "button");
		if (%pos != -1)
		{
			%mods = getSubStr(%action, 0, %pos);
			%object = getSubStr(%action, %pos, 1000);
			%instance = getSubStr(%object, strlen("button"), 1000);
			return %mods @ "joystick" @ %instance + 1;
		}
		else
		{
			%pos = strstr(%action, "pov");
			if (%pos != -1)
			{
				%wordCount = getWordCount(%action);
				%mods = %wordCount > 1 ? getWords(%action, 0, %wordCount - 2) @ " " : "";
				%object = getWord(%action, %wordCount - 1);
				if (%object $= "upov")
				{
					%object = "POV1 up";
				}
				else if (%object $= "dpov")
				{
					%object = "POV1 down";
				}
				else if (%object $= "lpov")
				{
					%object = "POV1 left";
				}
				else if (%object $= "rpov")
				{
					%object = "POV1 right";
				}
				else if (%object $= "upov2")
				{
					%object = "POV2 up";
				}
				else if (%object $= "dpov2")
				{
					%object = "POV2 down";
				}
				else if (%object $= "lpov2")
				{
					%object = "POV2 left";
				}
				else if (%object $= "rpov2")
				{
					%object = "POV2 right";
				}
				else
				{
					%object = "??";
				}
				return %mods @ %object;
			}
			else
			{
				error("Unsupported Joystick input object passed to getDisplayMapName!");
			}
		}
	}
	return "??";
}

function buildFullMapString(%index)
{
	%name = $RemapName[%index];
	%cmd = $RemapCmd[%index];
	%temp = moveMap.getBinding(%cmd);
	%device = getField(%temp, 0);
	%object = getField(%temp, 1);
	if (%device !$= "" && %object !$= "")
	{
		%mapString = getMapDisplayName(%device, %object);
	}
	else
	{
		%mapString = "";
	}
	return %name TAB %mapString;
}

function OptRemapList::fillList(%this)
{
	%this.clear();
	for (%i = 0; %i < $RemapCount; %i++)
	{
		%this.addRow(%i, buildFullMapString(%i));
	}
}

function OptRemapList::doRemap(%this)
{
	%selId = %this.getSelectedId();
	%name = $RemapName[%selId];
	OptRemapText.setValue("REMAP \"" @ %name @ "\"");
	OptRemapInputCtrl.index = %selId;
	Canvas.pushDialog(RemapDlg);
}

function redoMapping(%device, %action, %cmd, %oldIndex, %newIndex)
{
	moveMap.bind(%device, %action, %cmd);
	OptRemapList.setRowById(%oldIndex, buildFullMapString(%oldIndex));
	OptRemapList.setRowById(%newIndex, buildFullMapString(%newIndex));
}

function findRemapCmdIndex(%command)
{
	for (%i = 0; %i < $RemapCount; %i++)
	{
		if (%command $= $RemapCmd[%i])
		{
			return %i;
		}
	}
	return -1;
}

function OptRemapInputCtrl::onInputEvent(%this, %device, %action)
{
	Canvas.popDialog(RemapDlg);
	if (%device $= "keyboard")
	{
		if (%action $= "escape")
		{
			return;
		}
		else if (%action $= "backspace")
		{
			%bind = moveMap.getBinding($RemapCmd[%this.index]);
			%device = getWord(%bind, 0);
			%action = getWord(%bind, 1);
			moveMap.unbind(%device, %action);
			OptRemapList.setRowById(%this.index, buildFullMapString(%this.index));
			return;
		}
	}
	%cmd = $RemapCmd[%this.index];
	%name = $RemapName[%this.index];
	%prevMap = moveMap.getCommand(%device, %action);
	if (%prevMap !$= %cmd)
	{
		if (%prevMap $= "")
		{
			moveMap.bind(%device, %action, %cmd);
			OptRemapList.setRowById(%this.index, buildFullMapString(%this.index));
		}
		else
		{
			%mapName = getMapDisplayName(%device, %action);
			%prevMapIndex = findRemapCmdIndex(%prevMap);
			if (%prevMapIndex == -1)
			{
				MessageBoxOK("REMAP FAILED", "\"" @ %mapName @ "\" is already bound to a non-remappable command!");
			}
			else
			{
				%prevCmdName = $RemapName[%prevMapIndex];
				MessageBoxYesNo("WARNING", "\"" @ %mapName @ "\" is already bound to \"" @ %prevCmdName @ "\"!\nDo you want to undo this mapping?", "redoMapping(" @ %device @ ", \"" @ %action @ "\", \"" @ %cmd @ "\", " @ %prevMapIndex @ ", " @ %this.index @ ");", "");
			}
			return;
		}
	}
}

function OptAudioUpdate()
{
	%text = "Vendor: " @ alGetString("AL_VENDOR") @ "\nVersion: " @ alGetString("AL_VERSION") @ "\nRenderer: " @ alGetString("AL_RENDERER") @ "\nExtensions: " @ alGetString("AL_EXTENSIONS");
	OptAudioInfo.setText(%text);
}

new AudioDescription(AudioChannel0)
{
	volume = 1;
	isLooping = 0;
	is3D = 0;
	type = 0;
};
new AudioDescription(AudioChannel1)
{
	volume = 1;
	isLooping = 0;
	is3D = 0;
	type = 1;
};
new AudioDescription(AudioChannel2)
{
	volume = 1;
	isLooping = 0;
	is3D = 0;
	type = 2;
};
new AudioDescription(AudioChannel3)
{
	volume = 1;
	isLooping = 0;
	is3D = 0;
	type = 3;
};
new AudioDescription(AudioChannel4)
{
	volume = 1;
	isLooping = 0;
	is3D = 0;
	type = 4;
};
new AudioDescription(AudioChannel5)
{
	volume = 1;
	isLooping = 0;
	is3D = 0;
	type = 5;
};
new AudioDescription(AudioChannel6)
{
	volume = 1;
	isLooping = 0;
	is3D = 0;
	type = 6;
};
new AudioDescription(AudioChannel7)
{
	volume = 1;
	isLooping = 0;
	is3D = 0;
	type = 7;
};
new AudioDescription(AudioChannel8)
{
	volume = 1;
	isLooping = 0;
	is3D = 0;
	type = 8;
};
$AudioTestHandle = 0;
function OptAudioUpdateMasterVolume(%volume)
{
	if (%volume == $pref::Audio::masterVolume)
	{
		return;
	}
	alxListenerf(AL_GAIN_LINEAR, %volume);
	$pref::Audio::masterVolume = %volume;
	if (!alxIsPlaying($AudioTestHandle))
	{
		$AudioTestHandle = alxCreateSource("AudioChannel0", ExpandFilename("base/data/sound/testing.wav"));
		alxPlay($AudioTestHandle);
	}
}

function OptAudioUpdateChannelVolume(%channel, %volume)
{
	if (%channel < 1 || %channel > 8)
	{
		return;
	}
	if (%volume == $pref::Audio::channelVolume[%channel])
	{
		return;
	}
	alxSetChannelVolume(%channel, %volume);
	$pref::Audio::channelVolume[%channel] = %volume;
	if (!alxIsPlaying($AudioTestHandle))
	{
		$AudioTestHandle = alxCreateSource("AudioChannel" @ %channel, ExpandFilename("base/data/sound/testing.wav"));
		alxPlay($AudioTestHandle);
	}
}

function OptAudioDriverList::onSelect(%this, %__unused, %text)
{
	if (%text $= "")
	{
		return;
	}
	if ($pref::Audio::driver $= %text)
	{
		return;
	}
	$pref::Audio::driver = %text;
	OpenALInit();
}

function setActiveInv(%index)
{
	if (%index < 0)
	{
		HUD_BrickActive.setVisible(0);
		HUD_BrickName.setText("");
		return;
	}
	HUD_BrickActive.setVisible(1);
	%x = 64 * %index;
	%y = 0;
	%w = 64;
	%h = 64;
	HUD_BrickActive.resize(%x, %y, %w, %h);
	HUD_BrickName.setText($InvData[%index].uiName);
	return;
	eval("HUDInvActive" @ $CurrScrollBrickSlot @ ".setVisible(false);");
	if (%index < 0)
	{
		return;
	}
	eval("HUDInvActive" @ %index @ ".setVisible(true);");
}

function getActiveInv()
{
	return;
	for (%i = 0; %i < $BSD_NumInventorySlots; %i++)
	{
		eval("%val = (HUDInvActive" @ %i @ ".visible == true);");
		if (%val == 1)
		{
			return %i;
		}
	}
	return -1;
}

function directSelectInv(%index)
{
	if (%index < 0)
	{
		scrollBricks(1);
		return;
	}
	if ($InvData[%index] > 0)
	{
		if ($ScrollMode == $SCROLLMODE_BRICKS)
		{
			if ($CurrScrollBrickSlot == %index && HUD_BrickActive.visible == 1)
			{
				setActiveInv(-1);
				HUD_BrickName.setText("");
				commandToServer('unUseTool');
			}
			else
			{
				setActiveInv(%index);
				$CurrScrollBrickSlot = %index;
				commandToServer('useInventory', %index);
			}
		}
		else
		{
			setScrollMode($SCROLLMODE_BRICKS);
			setActiveInv(%index);
			$CurrScrollBrickSlot = %index;
			commandToServer('useInventory', %index);
			HUD_BrickName.setText($InvData[$CurrScrollBrickSlot].uiName);
		}
	}
	else if ($ScrollMode != $SCROLLMODE_BRICKS)
	{
		setScrollMode($SCROLLMODE_BRICKS);
	}
}

function useFirstSlot(%val)
{
	if (%val)
	{
		directSelectInv(0);
	}
}

function useSecondSlot(%val)
{
	if (%val)
	{
		directSelectInv(1);
	}
}

function useThirdSlot(%val)
{
	if (%val)
	{
		directSelectInv(2);
	}
}

function useFourthSlot(%val)
{
	if (%val)
	{
		directSelectInv(3);
	}
}

function useFifthSlot(%val)
{
	if (%val)
	{
		directSelectInv(4);
	}
}

function useSixthSlot(%val)
{
	if (%val)
	{
		directSelectInv(5);
	}
}

function useSeventhSlot(%val)
{
	if (%val)
	{
		directSelectInv(6);
	}
}

function useEighthSlot(%val)
{
	if (%val)
	{
		directSelectInv(7);
	}
}

function useNinthSlot(%val)
{
	if (%val)
	{
		directSelectInv(8);
	}
}

function useTenthSlot(%val)
{
	if (%val)
	{
		directSelectInv(9);
	}
}

function dropFirstSlot(%val)
{
	if (%val)
	{
		commandToServer('dropInventory', 0);
	}
}

function dropSecondSlot(%val)
{
	if (%val)
	{
		commandToServer('dropInventory', 1);
	}
}

function dropThirdSlot(%val)
{
	if (%val)
	{
		commandToServer('dropInventory', 2);
	}
}

function dropFourthSlot(%val)
{
	if (%val)
	{
		commandToServer('dropInventory', 3);
	}
}

function dropFifthSlot(%val)
{
	if (%val)
	{
		commandToServer('dropInventory', 4);
	}
}

function dropSixthSlot(%val)
{
	if (%val)
	{
		commandToServer('dropInventory', 5);
	}
}

function dropSeventhSlot(%val)
{
	if (%val)
	{
		commandToServer('dropInventory', 6);
	}
}

function dropEighthSlot(%val)
{
	if (%val)
	{
		commandToServer('dropInventory', 7);
	}
}

function dropNinthSlot(%val)
{
	if (%val)
	{
		commandToServer('dropInventory', 8);
	}
}

function dropTenthSlot(%val)
{
	if (%val)
	{
		commandToServer('dropInventory', 9);
	}
}

$BrickFirstRepeatTime = 200;
$BrickRepeatTime = 50;
function repeatBrickAway(%val)
{
	if (%val == $brickAway)
	{
		commandToServer('shiftBrick', 1, 0, 0);
		schedule($BrickRepeatTime, 0, repeatBrickAway, %val);
	}
}

function repeatBrickTowards(%val)
{
	if (%val == $brickTowards)
	{
		commandToServer('shiftBrick', -1, 0, 0);
		schedule($BrickRepeatTime, 0, repeatBrickTowards, %val);
	}
}

function repeatBrickLeft(%val)
{
	if (%val == $brickLeft)
	{
		commandToServer('shiftBrick', 0, 1, 0);
		schedule($BrickRepeatTime, 0, repeatBrickLeft, %val);
	}
}

function repeatBrickRight(%val)
{
	if (%val == $brickRight)
	{
		commandToServer('shiftBrick', 0, -1, 0);
		schedule($BrickRepeatTime, 0, repeatBrickRight, %val);
	}
}

function repeatBrickUp(%val)
{
	if (%val == $brickUp)
	{
		commandToServer('shiftBrick', 0, 0, 3);
		schedule($BrickRepeatTime, 0, repeatBrickUp, %val);
	}
}

function repeatBrickDown(%val)
{
	if (%val == $brickDown)
	{
		commandToServer('shiftBrick', 0, 0, -3);
		schedule($BrickRepeatTime, 0, repeatBrickDown, %val);
	}
}

function repeatBrickThirdUp(%val)
{
	if (%val == $brickThirdUp)
	{
		commandToServer('shiftBrick', 0, 0, 1);
		schedule($BrickRepeatTime, 0, repeatBrickThirdUp, %val);
	}
}

function repeatBrickThirdDown(%val)
{
	if (%val == $brickThirdDown)
	{
		commandToServer('shiftBrick', 0, 0, -1);
		schedule($BrickRepeatTime, 0, repeatBrickThirdDown, %val);
	}
}

function repeatBrickPlant(%val)
{
	if (%val == $brickPlant)
	{
		commandToServer('plantBrick');
		schedule($BrickRepeatTime, 0, repeatBrickPlant, %val);
	}
}

function shiftBrickAway(%val)
{
	if ($SuperShift)
	{
		superShiftBrickAway(%val);
		return;
	}
	$brickAway++;
	$brickAway = $brickAway % 1000;
	if (%val)
	{
		commandToServer('shiftBrick', 1, 0, 0);
		schedule($BrickFirstRepeatTime, 0, repeatBrickAway, $brickAway);
	}
}

function shiftBrickTowards(%val)
{
	if ($SuperShift)
	{
		superShiftBrickTowards(%val);
		return;
	}
	$brickTowards++;
	$brickTowards = $brickTowards % 1000;
	if (%val)
	{
		commandToServer('shiftBrick', -1, 0, 0);
		schedule($BrickFirstRepeatTime, 0, repeatBrickTowards, $brickTowards);
	}
}

function shiftBrickLeft(%val)
{
	if ($SuperShift)
	{
		superShiftBrickLeft(%val);
		return;
	}
	$brickLeft++;
	$brickLeft = $brickLeft % 1000;
	if (%val)
	{
		commandToServer('shiftBrick', 0, 1, 0);
		schedule($BrickFirstRepeatTime, 0, repeatBrickLeft, $brickLeft);
	}
}

function shiftBrickRight(%val)
{
	if ($SuperShift)
	{
		superShiftBrickRight(%val);
		return;
	}
	$brickRight++;
	$brickRight = $brickRight % 1000;
	if (%val)
	{
		commandToServer('shiftBrick', 0, -1, 0);
		schedule($BrickFirstRepeatTime, 0, repeatBrickRight, $brickRight);
	}
}

function shiftBrickUp(%val)
{
	if ($SuperShift)
	{
		superShiftBrickUp(%val);
		return;
	}
	$brickUp++;
	$brickUp = $brickUp % 1000;
	if (%val)
	{
		commandToServer('shiftBrick', 0, 0, 3);
		schedule($BrickFirstRepeatTime, 0, repeatBrickUp, $brickUp);
	}
}

function shiftBrickDown(%val)
{
	if ($SuperShift)
	{
		superShiftBrickDown(%val);
		return;
	}
	$brickDown++;
	$brickDown = $brickDown % 1000;
	if (%val)
	{
		commandToServer('shiftBrick', 0, 0, -3);
		schedule($BrickFirstRepeatTime, 0, repeatBrickDown, $brickDown);
	}
}

function shiftBrickThirdUp(%val)
{
	$brickThirdUp++;
	$brickThirdUp = $brickThirdUp % 1000;
	if (%val)
	{
		commandToServer('shiftBrick', 0, 0, 1);
		schedule($BrickFirstRepeatTime, 0, repeatBrickThirdUp, $brickThirdUp);
	}
}

function shiftBrickThirdDown(%val)
{
	$brickThirdDown++;
	$brickThirdDown = $brickThirdDown % 1000;
	if (%val)
	{
		commandToServer('shiftBrick', 0, 0, -1);
		schedule($BrickFirstRepeatTime, 0, repeatBrickThirdDown, $brickThirdDown);
	}
}

function RotateBrickCW(%val)
{
	if (%val)
	{
		commandToServer('rotateBrick', 1);
	}
}

function RotateBrickCCW(%val)
{
	if (%val)
	{
		commandToServer('rotateBrick', -1);
	}
}

function plantBrick(%val)
{
	$brickPlant++;
	$brickPlant = $brickPlant % 1000;
	if (%val)
	{
		commandToServer('plantBrick');
		schedule($BrickFirstRepeatTime, 0, repeatBrickPlant, $brickPlant);
	}
}

function cancelBrick(%val)
{
	if (%val)
	{
		commandToServer('cancelBrick');
	}
}

function openAdminWindow(%val)
{
	if ($IamAdmin || isObject(ServerGroup))
	{
		if (%val)
		{
			Canvas.pushDialog(admingui);
		}
	}
	else
	{
		$AdminCallback = "canvas.pushDialog(admingui);";
		Canvas.pushDialog("adminLoginGui");
	}
}

function openOptionsWindow(%val)
{
	if (%val)
	{
		Canvas.pushDialog(optionsDlg);
	}
}

function useTools(%val)
{
	if (%val)
	{
		if ($ScrollMode != $SCROLLMODE_TOOLS)
		{
			if ($CurrScrollToolSlot <= 0)
			{
				$CurrScrollToolSlot = 0;
			}
			for (%i = 0; %i < $HUD_NumToolSlots; %i++)
			{
				%idx = (%i + $CurrScrollToolSlot) % $HUD_NumToolSlots;
				if ($ToolData[%idx] > 0)
				{
					$CurrScrollToolSlot = %idx;
					break;
				}
			}
			if (%i == $HUD_NumToolSlots)
			{
				return;
			}
			setScrollMode($SCROLLMODE_TOOLS);
			HUD_ToolActive.setVisible(True);
			setActiveTool($CurrScrollToolSlot);
			HUD_ToolName.setText($ToolData[$CurrScrollToolSlot].uiName);
			commandToServer('UseTool', $CurrScrollToolSlot);
		}
		else
		{
			setScrollMode($SCROLLMODE_BRICKS);
		}
	}
}

function useSprayCan(%val)
{
	if (%val)
	{
		shiftPaintColumn(1);
	}
}

function showPlayerList(%val)
{
	if (%val)
	{
		PlayerListGui.toggle();
	}
}

function openBSD(%val)
{
	if (%val)
	{
		if (BrickSelectorDlg.isAwake())
		{
			BSD_BuyBricks();
		}
		else
		{
			Canvas.pushDialog(BrickSelectorDlg);
		}
	}
}

$SCROLLMODE_BRICKS = 0;
$SCROLLMODE_PAINT = 1;
$SCROLLMODE_TOOLS = 2;
function scrollInventory(%val)
{
	if (Canvas.getCount() > 2)
	{
		return;
	}
	if (%val < 0)
	{
		%val = 1;
	}
	else
	{
		%val = -1;
	}
	if ($ScrollMode == $SCROLLMODE_BRICKS)
	{
		if (HUD_BrickActive.visible == 0)
		{
			directSelectInv($CurrScrollBrickSlot);
		}
		else
		{
			scrollBricks(%val);
		}
	}
	else if ($ScrollMode == $SCROLLMODE_PAINT)
	{
		scrollPaint(%val);
	}
	else if ($ScrollMode == $SCROLLMODE_TOOLS)
	{
		scrollTools(%val);
	}
}

function scrollBricks(%direction)
{
	if ($pref::Input::ReverseBrickScroll)
	{
		%direction = %direction * -1;
	}
	%startScrollBrickSlot = $CurrScrollBrickSlot;
	if ($CurrScrollBrickSlot $= "")
	{
		if (%direction > 0)
		{
			$CurrScrollBrickSlot = -1;
		}
		else
		{
			$CurrScrollBrickSlot = 1;
		}
	}
	for (%i = 0; %i < $BSD_NumInventorySlots; %i++)
	{
		$CurrScrollBrickSlot += %direction;
		if ($CurrScrollBrickSlot < 0)
		{
			$CurrScrollBrickSlot = $BSD_NumInventorySlots - 1;
		}
		if ($CurrScrollBrickSlot >= $BSD_NumInventorySlots)
		{
			$CurrScrollBrickSlot = 0;
		}
		if ($InvData[$CurrScrollBrickSlot] > 0)
		{
			break;
		}
	}
	if ($InvData[$CurrScrollBrickSlot] > 0)
	{
		if (%startScrollBrickSlot != $CurrScrollBrickSlot)
		{
			setActiveInv($CurrScrollBrickSlot);
			commandToServer('UseInventory', $CurrScrollBrickSlot);
		}
		else
		{
			setActiveInv(-1);
			commandToServer('unUseTool');
		}
	}
	else
	{
		setActiveInv(-1);
		commandToServer('unUseTool');
	}
}

function shiftPaintColumn(%direction)
{
	%canIndex = 0;
	if ($ScrollMode != $SCROLLMODE_PAINT)
	{
		setScrollMode($SCROLLMODE_PAINT);
		HUD_PaintBox.setVisible(1);
		HUD_PaintActive.setVisible(1);
		PlayGui.UnFadePaintRow($CurrPaintRow);
		%canIndex = 0;
	}
	else
	{
		PlayGui.FadePaintRow($CurrPaintRow);
		$CurrPaintRow += %direction;
		if ($CurrPaintRow >= $Paint_NumPaintRows)
		{
			$CurrPaintRow = 0;
		}
		if ($CurrPaintRow < 0)
		{
			$CurrPaintRow = $Paint_NumPaintRows - 1;
		}
		PlayGui.UnFadePaintRow($CurrPaintRow);
	}
	if ($CurrPaintRow == $Paint_NumPaintRows - 1)
	{
		if ($CurrPaintSwatch >= 2)
		{
			$CurrPaintSwatch = 2;
		}
		if ($CurrPaintSwatch == 0)
		{
			HUD_PaintName.setText("FX - Flat");
			commandToServer('useFlatCan');
		}
		else if ($CurrPaintSwatch == 1)
		{
			HUD_PaintName.setText("FX - Pearl");
			commandToServer('usePearlCan');
		}
		else if ($CurrPaintSwatch == 2)
		{
			HUD_PaintName.setText("FX - Chrome");
			commandToServer('useChromeCan');
		}
	}
	else
	{
		HUD_PaintName.setText(getSprayCanDivisionName($CurrPaintRow));
		%numSwatches = $Paint_Row[$CurrPaintRow].numSwatches;
		if ($CurrPaintSwatch >= %numSwatches)
		{
			$CurrPaintSwatch = %numSwatches - 1;
		}
		for (%i = 0; %i < $CurrPaintRow; %i++)
		{
			%canIndex += $Paint_Row[%i].numSwatches;
		}
		%canIndex += $CurrPaintSwatch;
		commandToServer('useSprayCan', %canIndex);
	}
	%x = (16 + 1) * $CurrPaintRow;
	%y = (16 + 1) * $CurrPaintSwatch;
	%w = 18;
	%h = 18;
	HUD_PaintActive.resize(%x, %y, %w, %h);
}

function scrollPaint(%direction)
{
	%numSwatches = $Paint_Row[$CurrPaintRow].numSwatches;
	$CurrPaintSwatch += %direction;
	if ($CurrPaintSwatch < 0)
	{
		$CurrPaintSwatch = %numSwatches - 1;
	}
	if ($CurrPaintSwatch >= %numSwatches)
	{
		$CurrPaintSwatch = 0;
	}
	PlayGui.updatePaintActive();
	if ($CurrPaintRow == $Paint_NumPaintRows - 1)
	{
		if ($CurrPaintSwatch == 0)
		{
			HUD_PaintName.setText("FX - Flat");
			commandToServer('useFlatCan');
		}
		else if ($CurrPaintSwatch == 1)
		{
			HUD_PaintName.setText("FX - Pearl");
			commandToServer('usePearlCan');
		}
		else if ($CurrPaintSwatch == 2)
		{
			HUD_PaintName.setText("FX - Chrome");
			commandToServer('useChromeCan');
		}
	}
	else
	{
		%canIndex = 0;
		for (%i = 0; %i < $CurrPaintRow; %i++)
		{
			%canIndex += $Paint_Row[%i].numSwatches;
		}
		%canIndex += $CurrPaintSwatch;
		commandToServer('useSprayCan', %canIndex);
	}
}

function setActiveTool(%index)
{
	if (%index < 0)
	{
		HUD_ToolActive.setVisible(0);
		HUD_ToolName.setText("");
		return;
	}
	HUD_ToolActive.setVisible(1);
	%x = 0;
	%y = 64 * %index;
	%w = 64;
	%h = 64;
	HUD_ToolActive.resize(%x, %y, %w, %h);
	HUD_ToolName.setText($ToolData[%index].uiName);
}

function scrollTools(%direction)
{
	if ($CurrScrollToolSlot $= "")
	{
		if (%direction > 0)
		{
			$CurrScrollToolSlot = -1;
		}
		else
		{
			$CurrScrollToolSlot = 1;
		}
	}
	for (%i = 0; %i < $HUD_NumToolSlots; %i++)
	{
		$CurrScrollToolSlot += %direction;
		if ($CurrScrollToolSlot < 0)
		{
			$CurrScrollToolSlot = $HUD_NumToolSlots - 1;
		}
		else if ($CurrScrollToolSlot >= $HUD_NumToolSlots)
		{
			$CurrScrollToolSlot = 0;
		}
		if ($ToolData[$CurrScrollToolSlot] > 0)
		{
			break;
		}
	}
	if ($ToolData[$CurrScrollToolSlot] > 0)
	{
		setActiveTool($CurrScrollToolSlot);
		commandToServer('UseTool', $CurrScrollToolSlot);
	}
	else
	{
		setActiveTool(-1);
	}
}

function setScrollMode(%newMode)
{
	if ($ScrollMode == %newMode)
	{
		return 0;
	}
	if ($ScrollMode == $SCROLLMODE_BRICKS)
	{
		setActiveInv(-1);
		HUD_BrickName.setText("");
		if ($pref::HUD::HideBrickBox)
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
	}
	else if ($ScrollMode == $SCROLLMODE_PAINT)
	{
		PlayGui.FadePaintRow($CurrPaintRow);
		HUD_PaintActive.setVisible(0);
		HUD_PaintName.setText("");
		if ($pref::HUD::HidePaintBox)
		{
			PlayGui.hidePaintBox(getWord(HUD_PaintBox.extent, 0) + 5, 10, 0);
		}
		if ($pref::HUD::showToolTips)
		{
			ToolTip_Paint.setVisible(1);
		}
		if ($InstantUse)
		{
			$InstantUse = 0;
		}
		else
		{
			commandToServer('UnUseTool');
		}
	}
	else if ($ScrollMode == $SCROLLMODE_TOOLS)
	{
		HUD_ToolActive.setVisible(0);
		HUD_ToolName.setText("");
		if ($pref::HUD::HideToolBox)
		{
			if ($pref::HUD::showToolTips)
			{
				PlayGui.hideToolBox($HUD_NumToolSlots * 64, 20, 0);
			}
			else
			{
				PlayGui.hideToolBox($HUD_NumToolSlots * 64 + 25, 20, 0);
			}
		}
		if ($pref::HUD::showToolTips)
		{
			ToolTip_Tools.setVisible(1);
		}
		if ($InstantUse)
		{
			$InstantUse = 0;
		}
		else
		{
			commandToServer('UnUseTool');
		}
	}
	$ScrollMode = %newMode;
	if ($ScrollMode == $SCROLLMODE_BRICKS)
	{
		if ($pref::HUD::HideBrickBox)
		{
			if ($pref::HUD::showToolTips)
			{
				PlayGui.hideBrickBox(-64, 10, 0);
			}
			else
			{
				PlayGui.hideBrickBox(-87, 10, 0);
			}
		}
	}
	else if ($ScrollMode == $SCROLLMODE_PAINT)
	{
		if ($pref::HUD::HidePaintBox)
		{
			PlayGui.hidePaintBox(-1 * getWord(HUD_PaintBox.extent, 0) - 5, 1, 0);
		}
		ToolTip_Paint.setVisible(0);
	}
	else if ($ScrollMode == $SCROLLMODE_TOOLS)
	{
		if ($pref::HUD::HideToolBox)
		{
			if ($pref::HUD::showToolTips)
			{
				PlayGui.hideToolBox(-$HUD_NumToolSlots * 64, 10, 0);
			}
			else
			{
				PlayGui.hideToolBox(-$HUD_NumToolSlots * 64 - 25, 10, 0);
			}
		}
		ToolTip_Tools.setVisible(0);
	}
	return 1;
}

function superRepeatBrickAway(%val)
{
	if (%val == $superBrickAway)
	{
		commandToServer('superShiftBrick', 1, 0, 0);
		schedule($BrickRepeatTime, 0, superRepeatBrickAway, %val);
	}
}

function superRepeatBrickTowards(%val)
{
	if (%val == $superBrickTowards)
	{
		commandToServer('superShiftBrick', -1, 0, 0);
		schedule($BrickRepeatTime, 0, superRepeatBrickTowards, %val);
	}
}

function superRepeatBrickLeft(%val)
{
	if (%val == $superBrickLeft)
	{
		commandToServer('superShiftBrick', 0, 1, 0);
		schedule($BrickRepeatTime, 0, superRepeatBrickLeft, %val);
	}
}

function superRepeatBrickRight(%val)
{
	if (%val == $superBrickRight)
	{
		commandToServer('superShiftBrick', 0, -1, 0);
		schedule($BrickRepeatTime, 0, superRepeatBrickRight, %val);
	}
}

function superRepeatBrickUp(%val)
{
	if (%val == $superBrickUp)
	{
		commandToServer('superShiftBrick', 0, 0, 1);
		schedule($BrickRepeatTime, 0, superRepeatBrickUp, %val);
	}
}

function superRepeatBrickDown(%val)
{
	if (%val == $superBrickDown)
	{
		commandToServer('superShiftBrick', 0, 0, -1);
		schedule($BrickRepeatTime, 0, superRepeatBrickDown, %val);
	}
}

function superShiftBrickAwayProxy(%val)
{
	if ($pref::Input::UseSuperShiftToggle && $pref::Input::UseSuperShiftSmartToggle)
	{
		shiftBrickAway(%val);
	}
	else
	{
		superShiftBrickAway(%val);
	}
}

function superShiftBrickTowardsProxy(%val)
{
	if ($pref::Input::UseSuperShiftToggle && $pref::Input::UseSuperShiftSmartToggle)
	{
		shiftBrickTowards(%val);
	}
	else
	{
		superShiftBrickTowards(%val);
	}
}

function superShiftBrickLeftProxy(%val)
{
	if ($pref::Input::UseSuperShiftToggle && $pref::Input::UseSuperShiftSmartToggle)
	{
		shiftBrickLeft(%val);
	}
	else
	{
		superShiftBrickLeft(%val);
	}
}

function superShiftBrickRightProxy(%val)
{
	if ($pref::Input::UseSuperShiftToggle && $pref::Input::UseSuperShiftSmartToggle)
	{
		shiftBrickRight(%val);
	}
	else
	{
		superShiftBrickRight(%val);
	}
}

function superShiftBrickUpProxy(%val)
{
	if ($pref::Input::UseSuperShiftToggle && $pref::Input::UseSuperShiftSmartToggle)
	{
		shiftBrickUp(%val);
	}
	else
	{
		superShiftBrickUp(%val);
	}
}

function superShiftBrickDownProxy(%val)
{
	if ($pref::Input::UseSuperShiftToggle && $pref::Input::UseSuperShiftSmartToggle)
	{
		shiftBrickDown(%val);
	}
	else
	{
		superShiftBrickDown(%val);
	}
}

function superShiftBrickAway(%val)
{
	$superBrickAway++;
	$superBrickAway = $superBrickAway % 1000;
	if (%val)
	{
		commandToServer('superShiftBrick', 1, 0, 0);
		schedule($BrickFirstRepeatTime, 0, superRepeatBrickAway, $superBrickAway);
	}
}

function superShiftBrickTowards(%val)
{
	$superBrickTowards++;
	$superBrickTowards = $superBrickTowards % 1000;
	if (%val)
	{
		commandToServer('superShiftBrick', -1, 0, 0);
		schedule($BrickFirstRepeatTime, 0, superRepeatBrickTowards, $superBrickTowards);
	}
}

function superShiftBrickLeft(%val)
{
	$superBrickLeft++;
	$superBrickLeft = $superBrickLeft % 1000;
	if (%val)
	{
		commandToServer('superShiftBrick', 0, 1, 0);
		schedule($BrickFirstRepeatTime, 0, superRepeatBrickLeft, $superBrickLeft);
	}
}

function superShiftBrickRight(%val)
{
	$superBrickRight++;
	$superBrickRight = $superBrickRight % 1000;
	if (%val)
	{
		commandToServer('superShiftBrick', 0, -1, 0);
		schedule($BrickFirstRepeatTime, 0, superRepeatBrickRight, $superBrickRight);
	}
}

function superShiftBrickUp(%val)
{
	$superBrickUp++;
	$superBrickUp = $superBrickUp % 1000;
	if (%val)
	{
		commandToServer('superShiftBrick', 0, 0, 1);
		schedule($BrickFirstRepeatTime, 0, superRepeatBrickUp, $superBrickUp);
	}
}

function superShiftBrickDown(%val)
{
	$superBrickDown++;
	$superBrickDown = $superBrickDown % 1000;
	if (%val)
	{
		commandToServer('superShiftBrick', 0, 0, -1);
		schedule($BrickFirstRepeatTime, 0, superRepeatBrickDown, $superBrickDown);
	}
}

function toggleSuperShift(%val)
{
	%minTime = 200;
	if ($pref::Input::UseSuperShiftToggle == 1)
	{
		if (%val)
		{
			$lastSuperShiftToggleTime = getSimTime();
			$SuperShift = !$SuperShift;
			HUD_SuperShift.setVisible($SuperShift);
			$brickAway++;
			$brickTowards++;
			$brickLeft++;
			$brickRight++;
			$brickUp++;
			$brickDown++;
			$brickThirdUp++;
			$brickThirdDown++;
			$brickPlant++;
			$superBrickAway++;
			$superBrickTowards++;
			$superBrickLeft++;
			$superBrickRight++;
			$superBrickUp++;
			$superBrickDown++;
		}
		else if ($pref::Input::UseSuperShiftSmartToggle)
		{
			%time = getSimTime() - $lastSuperShiftToggleTime;
			if (%time > %minTime)
			{
				$SuperShift = !$SuperShift;
				HUD_SuperShift.setVisible($SuperShift);
			}
		}
	}
}

function dropTool(%val)
{
	if (%val)
	{
		commandToServer('DropTool', $CurrScrollToolSlot);
	}
}

function undoBrick(%val)
{
	if (%val)
	{
		commandToServer('UndoBrick');
	}
}

function PageUpNewChatHud(%val)
{
	if (%val)
	{
		if (isObject($NewChatSO))
		{
			$NewChatSO.pageUp();
		}
	}
}

function PageDownNewChatHud(%val)
{
	if (%val)
	{
		if (isObject($NewChatSO))
		{
			$NewChatSO.pageDown();
		}
	}
}

function GlobalChat(%val)
{
	if (%val)
	{
		newMessageHud.open("SAY");
	}
}

function TeamChat(%val)
{
	if (%val)
	{
		newMessageHud.open("TEAM");
	}
}

