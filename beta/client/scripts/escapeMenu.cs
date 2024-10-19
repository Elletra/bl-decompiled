function EscapeMenu::toggle(%this)
{
	if (%this.isAwake())
	{
		Canvas.popDialog(%this);
	}
	else
	{
		Canvas.pushDialog(%this);
	}
}

function EscapeMenu::clickAdmin()
{
	if ($IamAdmin == 1 || isObject(ServerGroup))
	{
		Canvas.pushDialog("adminGui");
	}
	else
	{
		$AdminCallback = "canvas.pushDialog(admingui);";
		Canvas.pushDialog("adminLoginGui");
	}
}

function EscapeMenu::clickLoadBricks()
{
	if ($IamAdmin == 1 || isObject(ServerGroup))
	{
		Canvas.pushDialog("loadBricksGui");
	}
	else
	{
		$AdminCallback = "canvas.pushDialog(loadBricksGui);";
		Canvas.pushDialog("adminLoginGui");
	}
}

