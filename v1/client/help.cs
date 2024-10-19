function HelpDlg::onWake(%this)
{
	HelpFileList.entryCount = 0;
	HelpFileList.clear();
	for (%file = findFirstFile("*.hfl"); %file !$= ""; %file = findNextFile("*.hfl"))
	{
		HelpFileList.fileName[HelpFileList.entryCount] = %file;
		HelpFileList.addRow(HelpFileList.entryCount, fileBase(%file));
		HelpFileList.entryCount++;
	}
	HelpFileList.sortNumerical(0);
	HelpFileList.setSelectedRow(0);
}

function HelpFileList::onSelect(%this, %row)
{
	%fo = new FileObject();
	%fo.openForRead(%this.fileName[%row]);
	for (%text = ""; !%fo.isEOF(); %text = %text @ %fo.readLine() @ "\n")
	{
	}
	%fo.delete();
	HelpText.setText(%text);
}

function getHelp(%helpName)
{
	Canvas.pushDialog(HelpDlg);
	if (%helpName !$= "")
	{
		%index = HelpFileList.findTextIndex(%helpName);
		HelpFileList.setSelectedRow(%index);
	}
}

function contextHelp()
{
	for (%i = 0; %i < Canvas.getCount(); %i++)
	{
		if (Canvas.getObject(%i).getName() $= HelpDlg)
		{
			Canvas.popDialog(HelpDlg);
			return;
		}
	}
	%content = Canvas.getContent();
	%helpPage = %content.getHelpPage();
	getHelp(%helpPage);
}

function GuiControl::getHelpPage(%this)
{
	return %this.helpPage;
}

function GuiMLTextCtrl::onURL(%this, %url)
{
	gotoWebPage(%url);
}

