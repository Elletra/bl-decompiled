function clientCmdGameStart(%__unused)
{
	PlayerListGui.zeroScores();
}

function clientCmdGameEnd(%__unused)
{
	alxStopAll();
	EndGameGuiList.clear();
	for (%i = 0; %i < PlayerListGuiList.rowCount(); %i++)
	{
		%text = PlayerListGuiList.getRowText(%i);
		%id = PlayerListGuiList.getRowId(%i);
		EndGameGuiList.addRow(%id, %text);
	}
	EndGameGuiList.sortNumerical(1, 0);
	Canvas.setContent(EndGameGui);
}

