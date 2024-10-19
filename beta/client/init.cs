function initClient()
{
	echo("\n--------- Initializing Base: Client ---------");
	$Server::Dedicated = 0;
	$Client::GameTypeQuery = "Blockland";
	$Client::MissionTypeQuery = "Any";
	initBaseClient();
	initCanvas("Blockland");
	exec("./scripts/audioProfiles.cs");
	exec("./ui/PlayGui.gui");
	exec("./ui/MainChatHud.gui");
	exec("./ui/MessageHud.gui");
	exec("./ui/playerList.gui");
	exec("./ui/mainMenuGui.gui");
	exec("./ui/aboutDlg.gui");
	exec("./ui/startMissionGui.gui");
	exec("./ui/joinServerGui.gui");
	exec("./ui/joinServerPassGui.gui");
	exec("./ui/endGameGui.gui");
	exec("./ui/loadingGui.gui");
	exec("./ui/optionsDlg.gui");
	exec("./ui/remapDlg.gui");
	exec("./ui/manualJoin.gui");
	exec("./ui/filtersGui.gui");
	exec("./ui/changeMapGui.gui");
	exec("./ui/noHudGui.gui");
	exec("./scripts/client.cs");
	exec("./scripts/missionDownload.cs");
	exec("./scripts/serverConnection.cs");
	exec("./scripts/playerList.cs");
	exec("./scripts/loadingGui.cs");
	exec("./scripts/optionsDlg.cs");
	exec("./scripts/chatHud.cs");
	exec("./scripts/messageHud.cs");
	exec("./scripts/playGui.cs");
	exec("./scripts/centerPrint.cs");
	exec("./scripts/game.cs");
	exec("./scripts/msgCallbacks.cs");
	exec("./scripts/startMissionGui.cs");
	exec("./scripts/clientCmd.cs");
	exec("./scripts/manualJoin.cs");
	exec("./scripts/joinServerGui.cs");
	exec("./scripts/joinServerPassGui.cs");
	exec("./scripts/filtersGui.cs");
	exec("./scripts/noHudGui.cs");
	exec("./scripts/changeMapGui.cs");
	exec("./scripts/default.bind.cs");
	exec("./config.cs");
	exec("./ui/adminGui.gui");
	exec("./scripts/adminGui.cs");
	exec("./ui/escapeMenu.gui");
	exec("./scripts/escapeMenu.cs");
	exec("./ui/adminLoginGui.gui");
	exec("./scripts/adminLoginGui.cs");
	exec("./ui/printSelectorDlg.gui");
	exec("./scripts/printSelectorDlg.cs");
	exec("./ui/brickSelectorDlg.gui");
	exec("./scripts/brickSelectorDlg.cs");
	exec("./scripts/brickControls.cs");
	exec("./ui/saveBricksGui.gui");
	exec("./ui/loadBricksGui.gui");
	exec("./ui/loadBricksColorGui.gui");
	exec("./scripts/saveBricks.cs");
	exec("./ui/AvatarGui.gui");
	exec("./scripts/AvatarGui.cs");
	exec("./ui/autoUpdateGui.gui");
	exec("./scripts/autoUpdate.cs");
	exec("./scripts/mainMenuGui.cs");
	exec("./scripts/whosTalking.cs");
	exec("./ui/newMessageHud.gui");
	exec("./scripts/newMessageHud.cs");
	exec("./ui/newChatHud.gui");
	exec("./scripts/newChatHud.cs");
	setNetPort(0);
	setShadowDetailLevel($pref::shadows);
	setDefaultFov($pref::Player::defaultFov);
	setZoomSpeed($pref::Player::zoomSpeed);
	loadMainMenu();
	updateTempBrickSettings();
	if ($JoinGameAddress !$= "")
	{
		connect($JoinGameAddress, "", $pref::Player::Name);
	}
}

function loadMainMenu()
{
	Canvas.setContent(MainMenuGui);
	Canvas.setCursor("DefaultCursor");
	if (!$Pref::DontUpdate)
	{
		$AU_AutoClose = 1;
		Canvas.pushDialog(AutoUpdateGui);
	}
}

