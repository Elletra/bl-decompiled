function MJ_connect()
{
	cancelServerQuery();
	%ip = MJ_txtIP.getValue();
	%joinPass = MJ_txtJoinPass.getValue();
	echo("Attempting to connect to ", %ip);
	if (%ip)
	{
		$conn.delete();
		$conn = new GameConnection(ServerConnection);
		$conn.setConnectArgs($pref::Player::Name);
		$conn.setJoinPassword(%joinPass);
		$conn.connect(%ip);
	}
}

