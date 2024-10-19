function clientCmdUseBrickControls()
{
	if (!$UsingBrickControls)
	{
		$BC_mouseX = moveMap.getCommand("mouse0", "xaxis");
		$BC_mouseY = moveMap.getCommand("mouse0", "yaxis");
		$BC_mouseZ = moveMap.getCommand("mouse0", "zaxis");
		$BC_mouseButton0 = moveMap.getCommand("mouse0", "button0");
		$BC_mouseButton1 = moveMap.getCommand("mouse0", "button1");
		$BC_mouseButton2 = moveMap.getCommand("mouse0", "button2");
		moveMap.bind(mouse0, "xaxis", mouseMoveBrickX);
		moveMap.bind(mouse0, "yaxis", mouseMoveBrickY);
		moveMap.bind(mouse0, "zaxis", mouseMoveBrickZ);
		moveMap.bind(mouse0, "button0", plantBrick);
		moveMap.bind(mouse0, "button1", RotateBrickCW);
		moveMap.bind(mouse0, "button2", RotateBrickCCW);
		$UsingBrickControls = 1;
	}
}

function clientCmdStopBrickControls()
{
	moveMap.bind(mouse0, "xaxis", $BC_mouseX);
	moveMap.bind(mouse0, "yaxis", $BC_mouseY);
	moveMap.bind(mouse0, "zaxis", $BC_mouseZ);
	moveMap.bind(mouse0, "button0", $BC_mouseButton0);
	moveMap.bind(mouse0, "button1", $BC_mouseButton1);
	moveMap.bind(mouse0, "button2", $BC_mouseButton2);
	$UsingBrickControls = 0;
}

function mouseMoveBrickX(%val)
{
	$mouseBrickShiftX += %val * 0.1;
	if ($mouseBrickShiftX >= 1)
	{
		commandToServer('ShiftBrick', 0, -1, 0);
		$mouseBrickShiftX = 0;
	}
	else if ($mouseBrickShiftX <= -1)
	{
		commandToServer('ShiftBrick', 0, 1, 0);
		$mouseBrickShiftX = 0;
	}
}

function mouseMoveBrickY(%val)
{
	$mouseBrickShiftY += %val * 0.1;
	if ($mouseBrickShiftY >= 1)
	{
		commandToServer('ShiftBrick', -1, 0, 0);
		$mouseBrickShiftY = 0;
	}
	else if ($mouseBrickShiftY <= -1)
	{
		commandToServer('ShiftBrick', 1, 0, 0);
		$mouseBrickShiftY = 0;
	}
}

function mouseMoveBrickZ(%val)
{
	commandToServer('ShiftBrick', 0, 0, %val / 120);
}

