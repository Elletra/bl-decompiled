function MessageCallback(%dlg, %callback)
{
	Canvas.popDialog(%dlg);
	eval(%callback);
}

function MBSetText(%text, %frame, %msg)
{
	%ext = %text.getExtent();
	%text.setText("<just:center>" @ %msg);
	%text.forceReflow();
	%newExtent = %text.getExtent();
	%deltaY = getWord(%newExtent, 1) - getWord(%ext, 1);
	%windowPos = %frame.getPosition();
	%windowExt = %frame.getExtent();
	%frame.resize(getWord(%windowPos, 0), getWord(%windowPos, 1) - %deltaY / 2, getWord(%windowExt, 0), getWord(%windowExt, 1) + %deltaY);
}

function MessageBoxOK(%title, %message, %callback)
{
	MBOKFrame.setText(%title);
	Canvas.pushDialog(MessageBoxOKDlg);
	MBSetText(MBOKText, MBOKFrame, %message);
	MessageBoxOKDlg.callback = %callback;
}

function MessageBoxOKDlg::onSleep(%this)
{
	%this.callback = "";
}

function MessageBoxOKCancel(%title, %message, %callback, %cancelCallback)
{
	MBOKCancelFrame.setText(%title);
	Canvas.pushDialog(MessageBoxOKCancelDlg);
	MBSetText(MBOKCancelText, MBOKCancelFrame, %message);
	MessageBoxOKCancelDlg.callback = %callback;
	MessageBoxOKCancelDlg.cancelCallback = %cancelCallback;
}

function MessageBoxOKCancelDlg::onSleep(%this)
{
	%this.callback = "";
}

function MessageBoxYesNo(%title, %message, %yesCallback, %noCallback)
{
	MBYesNoFrame.setText(%title);
	Canvas.pushDialog(MessageBoxYesNoDlg);
	MBSetText(MBYesNoText, MBYesNoFrame, %message);
	MessageBoxYesNoDlg.yesCallBack = %yesCallback;
	MessageBoxYesNoDlg.noCallback = %noCallback;
}

function MessageBoxYesNoDlg::onSleep(%this)
{
	%this.yesCallBack = "";
	%this.noCallback = "";
}

function MessagePopup(%title, %message, %delay)
{
	MessagePopFrame.setText(%title);
	Canvas.pushDialog(MessagePopupDlg);
	MBSetText(MessagePopText, MessagePopFrame, %message);
	if (%delay !$= "")
	{
		schedule(%delay, 0, CloseMessagePopup);
	}
}

function CloseMessagePopup()
{
	Canvas.popDialog(MessagePopupDlg);
}

