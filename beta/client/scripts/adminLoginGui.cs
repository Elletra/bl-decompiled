function sendAdminLogin()
{
	SAD(txtAdminPass.getValue());
}

function clientCmdAdminSuccess()
{
	$IamAdmin = 1;
	Canvas.popDialog(adminLoginGui);
	eval($AdminCallback);
}

function clientCmdAdminFailure()
{
	if ($sendAdminPass == 1)
	{
		MessageBoxOK("Login Failure", "Wrong Password");
	}
}

