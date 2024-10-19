function initClient()
{
	echo("\n--------- Initializing Base: Client ---------");
	$Server::Dedicated = 0;
	$Client::GameTypeQuery = "Blockland";
	$Client::MissionTypeQuery = "Any";
	initBaseClient();
	initCanvas("Blockland");
	exec("./scripts/allClientScripts.cs");
	exec("./ui/PlayGui.gui");
	exec("./ui/newPlayerListGui.gui");
	exec("./ui/trustInviteGui.gui");
	exec("./ui/miniGameInviteGui.gui");
	exec("./ui/JoinMiniGameGui.gui");
	exec("./ui/CreateMiniGameGui.gui");
	exec("./ui/mainMenuGui.gui");
	exec("./ui/aboutDlg.gui");
	exec("./ui/startMissionGui.gui");
	exec("./ui/joinServerGui.gui");
	exec("./ui/joinServerPassGui.gui");
	exec("./ui/connectingGui.gui");
	exec("./ui/loadingGui.gui");
	exec("./ui/optionsDlg.gui");
	exec("./ui/remapDlg.gui");
	exec("./ui/manualJoin.gui");
	exec("./ui/filtersGui.gui");
	exec("./ui/changeMapGui.gui");
	exec("./ui/noHudGui.gui");
	exec("./ui/ServerConfigGui.gui");
	exec("./scripts/default.bind.cs");
	exec("~/config/client/config.cs");
	exec("./ui/adminGui.gui");
	exec("./ui/addBanGui.gui");
	exec("./ui/unBanGui.gui");
	exec("./ui/brickManGui.gui");
	exec("./ui/escapeMenu.gui");
	exec("./ui/adminLoginGui.gui");
	exec("./ui/printSelectorDlg.gui");
	exec("./ui/brickSelectorDlg.gui");
	exec("./ui/saveBricksGui.gui");
	exec("./ui/loadBricksGui.gui");
	exec("./ui/loadBricksColorGui.gui");
	exec("./ui/saveBricksWarningGui.gui");
	exec("./ui/AvatarGui.gui");
	exec("./ui/autoUpdateGui.gui");
	exec("./ui/newMessageHud.gui");
	exec("./ui/newChatHud.gui");
	exec("./ui/SelectNetworkGui.gui");
	exec("./ui/ProgressGui.gui");
	exec("./ui/fastLoadWarningGui.gui");
	exec("./ui/defaultControlsGui.gui");
	exec("./ui/savingGui.gui");
	exec("./ui/wrenchDlg.gui");
	exec("./ui/wrenchSoundDlg.gui");
	exec("./ui/wrenchVehicleSpawnDlg.gui");
	exec("./ui/keyGui.gui");
	exec("./ui/regNameGui.gui");
	exec("./ui/colorGui.gui");
	exec("./ui/colorSetGui.gui");
	exec("./ui/musicFilesGui.gui");
	exec("./ui/AddOnsGui.gui");
	exec("./ui/PortForwardInfoGui.gui");
	exec("./ui/demoBrickLimitGui.gui");
	JoinServerGui.lastQueryTime = 0;
	exec("./scripts/clientAddOns.cs");
	loadClientAddOns();
	setNetPort(0);
	setShadowDetailLevel($pref::shadows);
	setDefaultFov($pref::Player::defaultFov);
	setZoomSpeed($pref::Player::zoomSpeed);
	loadMainMenu();
	loadTrustList();
	updateTempBrickSettings();
	if ($JoinGameAddress !$= "")
	{
		connect($JoinGameAddress, "", $pref::Player::LANName, $pref::Player::NetName, $Pref::Player::ClanPrefix, $Pref::Player::ClanSuffix);
	}
}

function loadMainMenu()
{
	Canvas.setContent(MainMenuGui);
	Canvas.setCursor("DefaultCursor");
	if ($Pref::Net::ConnectionType > 0 && $Pref::Input::SelectedDefaults > 0)
	{
		if (!$Pref::DontUpdate)
		{
			$AU_AutoClose = 1;
			Canvas.pushDialog(AutoUpdateGui);
		}
	}
}

