$centerPrintActive = 0;
$bottomPrintActive = 0;
$CenterPrintSizes[1] = 20;
$CenterPrintSizes[2] = 36;
$CenterPrintSizes[3] = 56;
function clientCmdCenterPrint(%message, %time, %size)
{
	if ($centerPrintActive)
	{
		if (CenterPrintDlg.removePrint !$= "")
		{
			cancel(CenterPrintDlg.removePrint);
		}
	}
	else
	{
		CenterPrintDlg.visible = 1;
		$centerPrintActive = 1;
	}
	CenterPrintText.setText("<just:center>" @ %message);
	CenterPrintDlg.extent = firstWord(CenterPrintDlg.extent) @ " " @ $CenterPrintSizes[%size];
	if (%time > 0)
	{
		CenterPrintDlg.removePrint = schedule(%time * 1000, 0, "clientCmdClearCenterPrint");
	}
}

function clientCmdBottomPrint(%message, %time, %size)
{
	if ($bottomPrintActive)
	{
		if (BottomPrintDlg.removePrint !$= "")
		{
			cancel(BottomPrintDlg.removePrint);
		}
	}
	else
	{
		BottomPrintDlg.setVisible(1);
		$bottomPrintActive = 1;
	}
	BottomPrintText.setText("<just:center>" @ %message);
	BottomPrintDlg.extent = firstWord(BottomPrintDlg.extent) @ " " @ $CenterPrintSizes[%size];
	if (%time > 0)
	{
		BottomPrintDlg.removePrint = schedule(%time * 1000, 0, "clientCmdClearbottomPrint");
	}
}

function BottomPrintText::onResize(%this, %__unused, %__unused)
{
	%this.position = "0 0";
}

function CenterPrintText::onResize(%this, %__unused, %__unused)
{
	%this.position = "0 0";
}

function clientCmdClearCenterPrint()
{
	$centerPrintActive = 0;
	CenterPrintDlg.visible = 0;
	CenterPrintDlg.removePrint = "";
}

function clientCmdclearBottomPrint()
{
	$bottomPrintActive = 0;
	BottomPrintDlg.visible = 0;
	BottomPrintDlg.removePrint = "";
}

