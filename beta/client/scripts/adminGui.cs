function admingui::kick()
{
	%victimId = lstAdminPlayerList.getSelectedId();
	commandToServer('kick', %victimId);
}

function admingui::ban()
{
	%victimId = lstAdminPlayerList.getSelectedId();
	commandToServer('ban', %victimId);
}

function admingui::changeMap()
{
}

function AdminGui_Wand()
{
	commandToServer('MagicWand');
	Canvas.popDialog(admingui);
	Canvas.popDialog(EscapeMenu);
}

function AdminGui_ClearBricks()
{
	MessageBoxYesNo("Clear Bricks?", "Are you sure you want to delete all bricks?", "commandToServer('ClearBricks');canvas.popDialog(AdminGui);");
}

