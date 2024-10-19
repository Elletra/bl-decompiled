function clientCmdUpdatePrefs()
{
	commandToServer('updatePrefs', $pref::Player::Name, $pref::Player::Hat, $pref::Player::Accent, $pref::Player::Pack, $pref::Player::HatColor, $pref::Player::AccentColor, $pref::Player::PackColor, $pref::Player::TorsoColor, $pref::Player::HipColor, $pref::Player::LLegColor, $pref::Player::RLegColor, $pref::Player::LArmColor, $pref::Player::RArmColor, $pref::Player::LHandColor, $pref::Player::RHandColor, $pref::Player::DecalColor, $pref::Player::FaceColor);
}

function clientCmdTimeScale(%val)
{
	if (%val < 0.5 || %val > 2)
	{
		return;
	}
	$timeScale = %val;
}

