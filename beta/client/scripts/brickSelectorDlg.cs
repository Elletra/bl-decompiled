function BrickSelectorDlg::onWake(%this)
{
	$BSD_CurrClickData = -1;
	$BSD_CurrClickInv = -1;
	BSD_FavsHelper.setVisible(0);
	BSD_SetFavsButton.setText("Set Favs>");
}

function BrickSelectorDlg::onSleep(%this)
{
	if ($BSD_CurrClickData != -1)
	{
		$BSD_activeBitmap[$BSD_CurrClickData].setVisible(0);
	}
	if ($BSD_CurrClickInv != -1)
	{
		$BSD_InvActive[$BSD_CurrClickInv].setVisible(0);
	}
	$BSD_CurrClickData = -1;
	$BSD_CurrClickInv = -1;
}

function clientCmdBSD_Open()
{
	Canvas.pushDialog(BrickSelectorDlg);
}

function clientCmdBSD_LoadBricks()
{
	BSD_LoadBricks();
	BSD_LoadFavorites();
	$UINameTableCreated = 0;
	BSD_Window.pushToBack(BSD_FavsHelper);
	PlayGui.createInvHUD();
}

function BSD_LoadBricks()
{
	BSD_KillBricks();
	$BSD_numCategories = 0;
	%dbCount = getDataBlockGroupSize();
	for (%i = 0; %i < %dbCount; %i++)
	{
		%db = getDataBlock(%i);
		%dbClass = %db.getClassName();
		if (%dbClass $= "fxDTSBrickData")
		{
			%cat = %db.category;
			%subCat = %db.subCategory;
			BSD_addCategory(%cat);
			%subCatObj = BSD_addSubCategory(%cat, %subCat);
			%subCatObj.numBricks++;
		}
	}
	%newScrollBox = new GuiControl();
	BSD_Window.add(%newScrollBox);
	%x = 3;
	%y = 57;
	%w = 634;
	%h = 363;
	%newScrollBox.resize(%x, %y, %w, %h);
	%newScrollBox.setName("BSD_ScrollBox");
	%newTabBox = new GuiControl();
	BSD_Window.add(%newTabBox);
	%x = 3;
	%y = 30;
	%w = 634;
	%h = 25;
	%newTabBox.resize(%x, %y, %w, %h);
	%newTabBox.setName("BSD_TabBox");
	for (%i = 0; %i < $BSD_numCategories; %i++)
	{
		%newTab = new GuiBitmapButtonCtrl();
		BSD_TabBox.add(%newTab);
		%newTab.setProfile(BlockButtonProfile);
		%x = %i * 80;
		%y = 0;
		%w = 80;
		%h = 25;
		%newTab.resize(%x, %y, %w, %h);
		%newTab.setText($BSD_category[%i].name);
		%newTab.setBitmap("base/client/ui/tab1");
		%newTab.command = "BSD_ShowTab(" @ %i @ ");";
		%newScroll = new GuiScrollCtrl();
		BSD_ScrollBox.add(%newScroll);
		%newScroll.rowHeight = 64;
		%newScroll.hScrollBar = "alwaysOff";
		%newScroll.vScrollBar = "alwaysOn";
		%newScroll.setProfile(BlockScrollProfile);
		%newScroll.defaultLineHeight = 32;
		%x = 0;
		%y = 0;
		%w = 634;
		%h = 363;
		%newScroll.resize(%x, %y, %w, %h);
		if (%i > 0)
		{
			%newScroll.setVisible(0);
		}
		%newBox = new GuiControl();
		%newScroll.add(%newBox);
		%newBox.setProfile(ColorScrollProfile);
		%x = 0;
		%y = 0;
		%w = 0;
		%h = 0;
		%newBox.resize(%x, %y, %w, %h);
		$BSD_category[%i].tab = %newTab;
		$BSD_category[%i].Scroll = %newScroll;
		$BSD_category[%i].box = %newBox;
		BSD_createSubHeadings($BSD_category[%i]);
	}
	for (%i = 0; %i < %dbCount; %i++)
	{
		%db = getDataBlock(%i);
		%dbClass = %db.getClassName();
		if (%dbClass $= "fxDTSBrickData")
		{
			BSD_CreateBrickButton(%db);
		}
	}
	BSD_CreateInventoryButtons();
	BSD_Window.pushToBack(BSD_ClearBtn);
}

function BSD_DumpCategories()
{
	for (%i = 0; %i < $BSD_numCategories; %i++)
	{
		echo($BSD_category[%i].name);
		for (%j = 0; %j < $BSD_category[%i].numSubCategories; %j++)
		{
			echo("  ", $BSD_category[%i].subCategory[%j].name);
		}
	}
}

function BSD_KillBricks()
{
	%numCats = $BSD_numCategories;
	for (%i = 0; %i < %numCats; %i++)
	{
		$BSD_category[%i].delete();
	}
	$BSD_numCategories = 0;
	if (isObject(BSD_InvBox))
	{
		BSD_InvBox.delete();
	}
	if (isObject(BSD_TabBox))
	{
		BSD_TabBox.delete();
	}
	if (isObject(BSD_ScrollBox))
	{
		BSD_ScrollBox.delete();
	}
	for (%i = 0; %i < $BSD_NumInventorySlots; %i++)
	{
		$BSD_InvData[%i] = -1;
		$BSD_InvIcon[%i] = -1;
	}
}

function BSD_category::onRemove(%this)
{
	%this.tab.delete();
	%this.Scroll.delete();
	for (%j = 0; %j < %this.numSubCategories; %j++)
	{
		if (isObject(%this.subCategory[%j]))
		{
			%this.subCategory[%j].delete();
		}
	}
	$BSD_numCategories--;
}

function BSD_addCategory(%newcat)
{
	for (%i = 0; %i < $BSD_numCategories; %i++)
	{
		if ($BSD_category[%i].name $= %newcat)
		{
			return;
		}
	}
	$BSD_category[$BSD_numCategories] = new ScriptObject()
	{
		class = BSD_category;
		name = %newcat;
		numSubCategories = 0;
	};
	RootGroup.add($BSD_category[$BSD_numCategories]);
	$BSD_numCategories++;
}

function BSD_addSubCategory(%cat, %newSubCat)
{
	%catID = -1;
	for (%i = 0; %i < $BSD_numCategories; %i++)
	{
		if ($BSD_category[%i].name $= %cat)
		{
			%catID = %i;
			break;
		}
	}
	if (%catID == -1)
	{
		error("Error: BSD_addSubCategory - category \"", %cat, "\" not found.");
		return;
	}
	else
	{
		for (%i = 0; %i < $BSD_category[%catID].numSubCategories; %i++)
		{
			if ($BSD_category[%catID].subCategory[%i].name $= %newSubCat)
			{
				return $BSD_category[%catID].subCategory[%i];
			}
		}
		$BSD_category[%catID].subCategory[$BSD_category[%catID].numSubCategories] = new ScriptObject()
		{
			name = %newSubCat;
			numBrickButtons = 0;
		};
		RootGroup.add($BSD_category[%catID].subCategory[$BSD_category[%catID].numSubCategories]);
		$BSD_category[%catID].numSubCategories++;
		return $BSD_category[%catID].subCategory[$BSD_category[%catID].numSubCategories - 1];
	}
}

function BSD_findCategory(%catName)
{
	for (%i = 0; %i < $BSD_numCategories; %i++)
	{
		if ($BSD_category[%i].name $= %catName)
		{
			return $BSD_category[%i];
		}
	}
}

function BSD_findSubCategory(%catObj, %subCatName)
{
	for (%i = 0; %i < %catObj.numSubCategories; %i++)
	{
		if (%catObj.subCategory[%i].name $= %subCatName)
		{
			return %catObj.subCategory[%i];
		}
	}
}

function BSD_createSubHeadings(%cat)
{
	%box = %cat.box;
	for (%i = 0; %i < %cat.numSubCategories; %i++)
	{
		%subCatObj = %cat.subCategory[%i];
		%boxExtent = %box.getExtent();
		%boxMinExtent = %box.getMinExtent();
		%boxHeight = getWord(%boxExtent, 1) - getWord(%boxMinExtent, 1);
		%subCatObj.startHeight = %boxHeight;
		%newBar = new GuiBitmapCtrl();
		%box.add(%newBar);
		%x = 0 + 18;
		%y = %subCatObj.startHeight + 15;
		%w = 581;
		%h = 1;
		%newBar.resize(%x, %y, %w, %h);
		%newHeading = new GuiTextCtrl();
		%box.add(%newHeading);
		%newHeading.setProfile(BlockButtonProfile);
		%newHeading.setText(%subCatObj.name);
		%x = 0 + 18;
		%y = %subCatObj.startHeight - 2;
		%w = 100;
		%h = 18;
		%newHeading.resize(%x, %y, %w, %h);
		%numRows = mCeil(%subCatObj.numBricks / 6);
		%x = 0;
		%y = 0;
		%w = 634;
		%h = %boxHeight + 18 + %numRows * 96 + 5;
		%box.resize(%x, %y, %w, %h);
	}
}

function BSD_CreateBrickButton(%data)
{
	%catName = %data.category;
	%subCatName = %data.subCategory;
	%catObj = BSD_findCategory(%catName);
	%subCatObj = BSD_findSubCategory(%catObj, %subCatName);
	if (%catObj == 0 || %subCatObj == 0)
	{
		error("ERROR: BSD_CreateBrickButton - Couldnt find category objects");
		return;
	}
	%brickName = %data.uiName;
	%brickIcon = %data.iconName;
	if (%brickName $= "")
	{
		%brickName = "No Name";
	}
	if (!isFile(%brickIcon @ ".png"))
	{
		%brickIcon = "base/client/ui/brickIcons/unknown";
	}
	%box = %catObj.box;
	%x = (%subCatObj.numBrickButtons % 6) * 97 + 18;
	%y = mFloor(%subCatObj.numBrickButtons / 6) * 97 + %subCatObj.startHeight + 18;
	%subCatObj.numBrickButtons++;
	%newIconBG = new GuiBitmapCtrl();
	%box.add(%newIconBG);
	%newIconBG.resize(%x, %y, 96, 96);
	%newIconBG.setBitmap("base/client/ui/brickicons/brickiconbg");
	%newIconBG.setProfile(BlockDefaultProfile);
	%newIcon = new GuiBitmapCtrl();
	%box.add(%newIcon);
	%newIcon.resize(%x, %y, 96, 96);
	%newIcon.setBitmap(%brickIcon);
	%newIcon.setProfile(BlockDefaultProfile);
	%newActive = new GuiBitmapCtrl();
	%box.add(%newActive);
	%newActive.resize(%x, %y, 96, 96);
	%newActive.setBitmap("base/client/ui/brickicons/brickIconActive");
	%newActive.setProfile(BlockDefaultProfile);
	%newActive.setVisible(0);
	$BSD_activeBitmap[%data] = %newActive;
	%newIconButton = new GuiBitmapButtonCtrl();
	%box.add(%newIconButton);
	%newIconButton.resize(%x, %y, 96, 96);
	%newIconButton.setBitmap("base/client/ui/brickicons/brickIconBtn");
	%newIconButton.setProfile(BlockButtonProfile);
	%newIconButton.setText(" ");
	%newIconButton.command = "BSD_ClickIcon(" @ %data @ ");";
	%newIconButton.altCommand = "BSD_RightClickIcon(" @ %data @ ");";
	%newLabel = new GuiTextCtrl();
	%box.add(%newLabel);
	%w = 96;
	%h = 18;
	%x = %x;
	%y = (%y + 96) - %h;
	%newLabel.resize(%x, %y, %w, %h);
	%newLabel.setProfile(HUDBSDNameProfile);
	%newLabel.setText(%brickName);
}

function BSD_KillInventoryButtons()
{
	if (isObject(BSD_InvBox))
	{
		BSD_InvBox.delete();
	}
}

function BSD_CreateInventoryButtons()
{
	$BSD_NumInventorySlots = 10;
	%invWidth = 617;
	%buttonWidth = mFloor(%invWidth / 11) - 1;
	%newBox = new GuiBitmapCtrl();
	BSD_Window.add(%newBox);
	%x = 3;
	%y = (480 - %buttonWidth) - 4;
	%w = %invWidth - 58;
	%h = %buttonWidth;
	%newBox.resize(%x, %y, %w, %h);
	$BSD_InvBox = %newBox;
	%newBox.setName("BSD_InvBox");
	for (%i = 0; %i < $BSD_NumInventorySlots; %i++)
	{
		%x = %i * (%buttonWidth + 1);
		%y = 0;
		%w = %buttonWidth;
		%h = %buttonWidth;
		%newInvBG = new GuiBitmapCtrl();
		%newBox.add(%newInvBG);
		%newInvBG.setBitmap("base/client/ui/brickicons/brickiconbg");
		%newInvBG.resize(%x, %y, %w, %h);
		%newIcon = new GuiBitmapCtrl();
		%newBox.add(%newIcon);
		%newIcon.setProfile(HUDBitmapProfile);
		%newIcon.resize(%x, %y, %w, %h);
		$BSD_InvIcon[%i] = %newIcon;
		%newActive = new GuiBitmapCtrl();
		%newBox.add(%newActive);
		%newActive.setBitmap("base/client/ui/brickicons/brickiconActive");
		%newActive.setVisible(0);
		%newActive.resize(%x, %y, %w, %h);
		$BSD_InvActive[%i] = %newActive;
		%newInvButton = new GuiBitmapButtonCtrl();
		%newBox.add(%newInvButton);
		%newInvButton.setBitmap("base/client/ui/brickicons/brickIconBtn");
		%newInvButton.setProfile(BlockButtonProfile);
		%newInvButton.setText(" ");
		%newInvButton.resize(%x, %y, %w, %h);
		%newInvButton.command = "BSD_ClickInv(" @ %i @ ");";
	}
}

function BSD_ClickClear()
{
	for (%i = 0; %i < $BSD_NumInventorySlots; %i++)
	{
		$BSD_InvIcon[%i].setBitmap("");
		$BSD_InvData[%i] = -1;
	}
}

function BSD_ClickInv(%index)
{
	if ($BSD_CurrClickInv == %index)
	{
		$BSD_InvData[%index] = -1;
		$BSD_InvIcon[%index].setBitmap("");
		$BSD_InvActive[%index].setVisible(0);
		$BSD_CurrClickData = -1;
		$BSD_CurrClickInv = -1;
	}
	else if ($BSD_CurrClickInv != -1)
	{
		%tempData = $BSD_InvData[%index];
		$BSD_InvData[%index] = $BSD_InvData[$BSD_CurrClickInv];
		$BSD_InvData[$BSD_CurrClickInv] = %tempData;
		if (!isFile($BSD_InvData[%index].iconName @ ".png"))
		{
			$BSD_InvIcon[%index].setBitmap("base/client/ui/brickIcons/unknown");
		}
		else
		{
			$BSD_InvIcon[%index].setBitmap($BSD_InvData[%index].iconName);
		}
		if (!isFile($BSD_InvData[$BSD_CurrClickInv].iconName @ ".png"))
		{
			$BSD_InvIcon[$BSD_CurrClickInv].setBitmap("base/client/ui/brickIcons/unknown");
		}
		else
		{
			$BSD_InvIcon[$BSD_CurrClickInv].setBitmap($BSD_InvData[$BSD_CurrClickInv].iconName);
		}
		$BSD_InvActive[%index].setVisible(0);
		$BSD_InvActive[$BSD_CurrClickInv].setVisible(0);
		$BSD_CurrClickData = -1;
		$BSD_CurrClickInv = -1;
	}
	else if ($BSD_CurrClickData != -1)
	{
		$BSD_InvData[%index] = $BSD_CurrClickData;
		if (!isFile($BSD_InvData[%index].iconName @ ".png"))
		{
			$BSD_InvIcon[%index].setBitmap("base/client/ui/brickIcons/unknown");
		}
		else
		{
			$BSD_InvIcon[%index].setBitmap($BSD_InvData[%index].iconName);
		}
		$BSD_activeBitmap[$BSD_CurrClickData].setVisible(0);
		$BSD_CurrClickData = -1;
		$BSD_CurrClickInv = -1;
	}
	else
	{
		$BSD_InvActive[%index].setVisible(1);
		$BSD_CurrClickData = -1;
		$BSD_CurrClickInv = %index;
	}
}

function BSD_ClickIcon(%data)
{
	if ($BSD_CurrClickData == %data)
	{
		$BSD_activeBitmap[%data].setVisible(0);
		%openSlot = -1;
		for (%i = 0; %i < $BSD_NumInventorySlots; %i++)
		{
			if ($BSD_InvData[%i] <= 0)
			{
				%openSlot = %i;
				break;
			}
		}
		if (%openSlot != -1)
		{
			$BSD_InvData[%openSlot] = $BSD_CurrClickData;
			if (!isFile($BSD_InvData[%openSlot].iconName @ ".png"))
			{
				$BSD_InvIcon[%openSlot].setBitmap("base/client/ui/brickIcons/unknown");
			}
			else
			{
				$BSD_InvIcon[%openSlot].setBitmap($BSD_InvData[%openSlot].iconName);
			}
		}
		else if ($pref::Input::QueueBrickBuying)
		{
			for (%i = 0; %i < $BSD_NumInventorySlots - 1; %i++)
			{
				$BSD_InvData[%i] = $BSD_InvData[%i + 1];
				if (!isFile($BSD_InvData[%i].iconName @ ".png"))
				{
					$BSD_InvIcon[%i].setBitmap("base/client/ui/brickIcons/unknown");
				}
				else
				{
					$BSD_InvIcon[%i].setBitmap($BSD_InvData[%i].iconName);
				}
			}
			$BSD_InvData[$BSD_NumInventorySlots - 1] = $BSD_CurrClickData;
			if (!isFile($BSD_InvData[$BSD_NumInventorySlots - 1].iconName @ ".png"))
			{
				$BSD_InvIcon[$BSD_NumInventorySlots - 1].setBitmap("base/client/ui/brickIcons/unknown");
			}
			else
			{
				$BSD_InvIcon[$BSD_NumInventorySlots - 1].setBitmap($BSD_InvData[$BSD_NumInventorySlots - 1].iconName);
			}
		}
		$BSD_activeBitmap[$BSD_CurrClickData].setVisible(0);
		$BSD_CurrClickData = -1;
		$BSD_CurrClickInv = -1;
	}
	else
	{
		if ($BSD_CurrClickData != -1)
		{
			$BSD_activeBitmap[$BSD_CurrClickData].setVisible(0);
		}
		if ($BSD_CurrClickInv != -1)
		{
			$BSD_InvActive[$BSD_CurrClickInv].setVisible(0);
		}
		$BSD_activeBitmap[%data].setVisible(1);
		$BSD_CurrClickData = %data;
		$BSD_CurrClickInv = -1;
	}
}

function BSD_RightClickIcon(%data)
{
	Canvas.popDialog(BrickSelectorDlg);
	commandToServer('InstantUseBrick', %data);
	$InstantUse = 1;
	setScrollMode($SCROLLMODE_BRICKS);
	HUD_BrickName.setText(%data.uiName);
}

function BSD_ShowTab(%catID)
{
	for (%i = 0; %i < $BSD_numCategories; %i++)
	{
		if (%i == %catID)
		{
			$BSD_category[%i].Scroll.setVisible(1);
		}
		else
		{
			$BSD_category[%i].Scroll.setVisible(0);
		}
	}
}

function BSD_BuyBricks()
{
	commandToServer('ClearInventory');
	for (%i = 0; %i < $BSD_NumInventorySlots; %i++)
	{
		$HUD_BrickIcon[%i].setBitmap("");
		$InvData[%i] = -1;
		if ($BSD_InvData[%i] != -1)
		{
			commandToServer('BuyBrick', %i, $BSD_InvData[%i]);
		}
	}
	Canvas.popDialog(BrickSelectorDlg);
}

function BSD_SetFavs()
{
	BSD_FavsHelper.setVisible(!BSD_FavsHelper.visible);
	if (BSD_FavsHelper.visible == 0)
	{
		BSD_SetFavsButton.setText("Set Favs>");
	}
	else
	{
		BSD_SetFavsButton.setText(" Cancel ");
	}
}

function BSD_ClickFav(%idx)
{
	if (BSD_FavsHelper.visible == 1)
	{
		BSD_SaveFavorites(%idx);
		BSD_FavsHelper.setVisible(0);
		BSD_SetFavsButton.setText("Set Favs>");
	}
	else
	{
		BSD_BuyFavorites(%idx);
	}
}

function BSD_SaveFavorites(%idx)
{
	if ($FavoritesLoaded == 0)
	{
		BSD_LoadFavorites();
	}
	echo("saving brick faves to index ", %idx);
	for (%i = 0; %i < $BSD_NumInventorySlots; %i++)
	{
		$Favorite::Brick[%idx, %i] = $BSD_InvData[%i].uiName;
	}
	%fileName = "~/client/Favorites.cs";
	export("$Favorite::*", %fileName, 0);
}

function BSD_BuyFavorites(%idx)
{
	if ($FavoritesLoaded == 0)
	{
		BSD_LoadFavorites();
	}
	if ($UINameTableCreated == 0)
	{
		createUiNameTable();
	}
	for (%i = 0; %i < $BSD_NumInventorySlots; %i++)
	{
		$BSD_InvData[%i] = $uiNameTable[$Favorite::Brick[%idx, %i]];
		if (isObject($BSD_InvData[%i]))
		{
			if (!isFile($BSD_InvData[%i].iconName @ ".png"))
			{
				$BSD_InvIcon[%i].setBitmap("base/client/ui/brickIcons/unknown");
			}
			else
			{
				$BSD_InvIcon[%i].setBitmap($BSD_InvData[%i].iconName);
			}
		}
		else
		{
			$BSD_InvIcon[%i].setBitmap("");
		}
	}
}

function BSD_LoadFavorites()
{
	exec("~/client/Favorites.cs");
	$FavoritesLoaded = 1;
}

function listAllDataBlocks()
{
	%numDataBlocks = getDataBlockGroupSize();
	echo(%numDataBlocks, " DataBlocks");
	echo("--------------------------");
	for (%i = 0; %i < %numDataBlocks; %i++)
	{
		%db = getDataBlock(%i);
		%dbClass = %db.getClassName();
		echo(%db, " : ", %dbClass);
	}
	echo("--------------------------");
}

