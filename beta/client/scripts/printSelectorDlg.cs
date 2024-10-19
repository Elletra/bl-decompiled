function clientCmdSetLetterPrintInfo(%start, %numLetters)
{
	$PSD_letterStart = %start;
	$PSD_numLetters = %numLetters;
}

function clientCmdOpenPrintSelectorDlg(%aspectRatio, %startPrint, %numPrints)
{
	if (PSD_window.scrollcount $= "")
	{
		PSD_window.scrollcount = 0;
	}
	if (!isObject("PSD_PrintScroller" @ %aspectRatio))
	{
		PSD_LoadPrints(%aspectRatio, %startPrint, %numPrints);
	}
	if (!isObject("PSD_PrintScrollerLetters"))
	{
		PSD_LoadPrints("Letters", $PSD_letterStart, $PSD_numLetters);
	}
	Canvas.pushDialog("printSelectorDlg");
	if ($PSD_LettersVisible)
	{
		PSD_PrintScrollerLetters.setVisible(1);
	}
	else
	{
		%cmdString = "PSD_PrintScroller" @ %aspectRatio @ ".setVisible(true);";
		eval(%cmdString);
	}
	$PSD_CurrentAR = %aspectRatio;
}

function printSelectorDlg::onSleep(%__unused)
{
	if (PSD_PrintScrollerLetters.visible == 1)
	{
		$PSD_LettersVisible = 1;
	}
	else
	{
		$PSD_LettersVisible = 0;
	}
	for (%i = 0; %i < PSD_window.scrollcount; %i++)
	{
		PSD_window.Scroll[%i].setVisible(0);
	}
}

function ClientCmdPSD_KillPrints()
{
	PSD_KillPrints();
}

function PSD_KillPrints()
{
	for (%i = 0; %i < PSD_window.scrollcount; %i++)
	{
		if (isObject(PSD_window.Scroll[%i]))
		{
			PSD_window.Scroll[%i].delete();
		}
	}
	PSD_window.scrollcount = 0;
}

function PSD_click(%number)
{
	commandToServer('setPrint', %number);
	Canvas.popDialog(printSelectorDlg);
}

function PSD_LettersTab()
{
	PSD_PrintScrollerLetters.setVisible(1);
	%cmdString = "PSD_PrintScroller" @ $PSD_CurrentAR @ ".setVisible(false);";
	eval(%cmdString);
}

function PSD_PrintsTab()
{
	PSD_PrintScrollerLetters.setVisible(0);
	%cmdString = "PSD_PrintScroller" @ $PSD_CurrentAR @ ".setVisible(true);";
	eval(%cmdString);
}

function PSD_LoadPrints(%aspectRatio, %startPrint, %numPrints)
{
	%scrollName = "PSD_PrintScroller" @ %aspectRatio;
	%scrollObj = new GuiScrollCtrl();
	PSD_window.add(%scrollObj);
	PSD_window.Scroll[PSD_window.scrollcount] = %scrollObj;
	PSD_window.scrollcount++;
	%scrollObj.setName(%scrollName);
	%scrollObj.hScrollBar = "alwaysOff";
	%scrollObj.vScrollBar = "alwaysOn";
	%scrollObj.setProfile(ColorScrollProfile);
	%scrollObj.resize(6, 42, 205, 392);
	%boxName = "PSD_PrintBox" @ %aspectRatio;
	%boxObj = new GuiControl();
	%scrollObj.add(%boxObj);
	%boxObj.setName(%boxName);
	%boxObj.setProfile(ColorScrollProfile);
	%boxObj.resize(0, 0, 64, 64);
	%Xsize = 64;
	%Ysize = 64;
	%numColumns = 3;
	for (%i = %startPrint; %i < %startPrint + %numPrints; %i++)
	{
		%rawFileName = getPrintTexture(%i);
		%fileName = strreplace(%rawFileName, "prints", "printIcons");
		%newPrint = new GuiBitmapCtrl();
		%boxObj.add(%newPrint);
		%newPrint.setBitmap(%fileName);
		%x = (%Xsize + 1) * %columnCount;
		%y = (%Ysize + 1) * %rowCount;
		%w = %Xsize;
		%h = %Ysize;
		%newPrint.resize(%x, %y, %w, %h);
		%newButton = new GuiBitmapButtonCtrl();
		%boxObj.add(%newButton);
		%newButton.setProfile(BlockButtonProfile);
		%newButton.setBitmap("base/client/ui/btnPrint");
		%newButton.setText(" ");
		%newButton.command = "PSD_click(" @ %i @ ");";
		%x = (%Xsize + 1) * %columnCount;
		%y = (%Ysize + 1) * %rowCount;
		%w = %Xsize;
		%h = %Ysize;
		%newButton.resize(%x, %y, %w, %h);
		%baseName = fileBase(%fileName);
		if (strlen(%baseName) == 1)
		{
			%newButton.accelerator = %baseName;
		}
		if (%baseName $= "-bang")
		{
			%newButton.accelerator = "shift 1";
		}
		if (%baseName $= "-at")
		{
			%newButton.accelerator = "shift 2";
		}
		if (%baseName $= "-pound")
		{
			%newButton.accelerator = "shift 3";
		}
		if (%baseName $= "-dollar")
		{
			%newButton.accelerator = "shift 4";
		}
		if (%baseName $= "-percent")
		{
			%newButton.accelerator = "shift 5";
		}
		if (%baseName $= "-caret")
		{
			%newButton.accelerator = "shift 6";
		}
		if (%baseName $= "-and")
		{
			%newButton.accelerator = "shift 7";
		}
		if (%baseName $= "-asterisk")
		{
			%newButton.accelerator = "shift 8";
		}
		if (%baseName $= "-minus")
		{
			%newButton.accelerator = "-";
		}
		if (%baseName $= "-equals")
		{
			%newButton.accelerator = "=";
		}
		if (%baseName $= "-plus")
		{
			%newButton.accelerator = "shift =";
		}
		if (%baseName $= "-period")
		{
			%newButton.accelerator = ".";
		}
		if (%baseName $= "-lessthan")
		{
			%newButton.accelerator = "shift ,";
		}
		if (%baseName $= "-greaterthan")
		{
			%newButton.accelerator = ">";
		}
		if (%baseName $= "-qmark")
		{
			%newButton.accelerator = "shift /";
		}
		if (%baseName $= "-apostrophe")
		{
			%newButton.accelerator = "'";
		}
		%x = 0;
		%y = 0;
		%w = (%Xsize + 1) * %numColumns;
		%h = (%Ysize + 1) * (%rowCount + 1);
		%boxObj.resize(%x, %y, %w, %h);
		%columnCount++;
		if (%columnCount >= %numColumns)
		{
			%rowCount++;
			%columnCount = 0;
		}
	}
	%scrollObj.setVisible(0);
}

