if (isObject(moveMap))
{
	moveMap.delete();
}
new ActionMap(moveMap);
function escapeFromGame()
{
	if ($Server::ServerType $= "SinglePlayer")
	{
		MessageBoxYesNo("Quit Mission", "Exit from this Mission?", "disconnect();", "");
	}
	else
	{
		MessageBoxYesNo("Disconnect", "Disconnect from the server?", "disconnect();", "");
	}
}

function quitGame()
{
	MessageBoxYesNo("Quit Game", "Quit to Desktop?", "quit();", "");
}

moveMap.bindCmd(keyboard, "escape", "", "escapeMenu.toggle();");
$movementSpeed = 1;
function setSpeed(%speed)
{
	if (%speed)
	{
		$movementSpeed = %speed;
	}
}

function moveleft(%val)
{
	$mvLeftAction = %val * $RunMultiplier;
}

function moveright(%val)
{
	$mvRightAction = %val * $RunMultiplier;
}

function moveforward(%val)
{
	$mvForwardAction = %val * $RunMultiplier;
}

function movebackward(%val)
{
	$mvBackwardAction = %val * $RunMultiplier;
}

function moveup(%val)
{
	$mvUpAction = %val;
}

function movedown(%val)
{
	$mvDownAction = %val;
}

function turnLeft(%val)
{
	$mvYawRightSpeed = %val ? $pref::Input::KeyboardTurnSpeed : "0";
}

function turnRight(%val)
{
	$mvYawLeftSpeed = %val ? $pref::Input::KeyboardTurnSpeed : "0";
}

function panUp(%val)
{
	$mvPitchDownSpeed = %val ? $pref::Input::KeyboardTurnSpeed : "0";
}

function panDown(%val)
{
	$mvPitchUpSpeed = %val ? $pref::Input::KeyboardTurnSpeed : "0";
}

function getMouseAdjustAmount(%val)
{
	return %val * ($cameraFov / 90) * 0.001;
}

function getMouseAdjustAmount(%val)
{
	return %val * ($cameraFov / 90) * 0.005;
}

function yaw(%val)
{
	%sens = $pref::Input::MouseSensitivity;
	$mvYaw += %sens * getMouseAdjustAmount(%val);
}

function pitch(%val)
{
	%sens = $pref::Input::MouseSensitivity;
	if ($pref::Input::MouseInvert)
	{
		$mvPitch -= %sens * getMouseAdjustAmount(%val);
	}
	else
	{
		$mvPitch += %sens * getMouseAdjustAmount(%val);
	}
}

function jump(%val)
{
	if ($pref::Input::noobjet)
	{
		$mvTriggerCount4++;
	}
	$mvTriggerCount2++;
}

$RunMultiplier = 1;
function walk(%val)
{
	if (%val)
	{
		$RunMultiplier = 0.4;
	}
	else
	{
		$RunMultiplier = 1;
	}
	if ($mvLeftAction)
	{
		$mvLeftAction = $RunMultiplier;
	}
	if ($mvRightAction)
	{
		$mvRightAction = $RunMultiplier;
	}
	if ($mvForwardAction)
	{
		$mvForwardAction = $RunMultiplier;
	}
	if ($mvBackwardAction)
	{
		$mvBackwardAction = $RunMultiplier;
	}
}

function crouch(%val)
{
	$mvTriggerCount3++;
}

function jet(%val)
{
	$mvTriggerCount4++;
}

moveMap.bind(keyboard, a, moveleft);
moveMap.bind(keyboard, d, moveright);
moveMap.bind(keyboard, w, moveforward);
moveMap.bind(keyboard, s, movebackward);
moveMap.bind(keyboard, space, jump);
moveMap.bind(mouse, xaxis, yaw);
moveMap.bind(mouse, yaxis, pitch);
function mouseFire(%val)
{
	$mvTriggerCount0++;
}

function altTrigger(%val)
{
	$mvTriggerCount1++;
}

moveMap.bind(mouse, button0, mouseFire);
if ($Pref::player::CurrentFOV $= "")
{
	$Pref::player::CurrentFOV = 45;
}
function setZoomFOV(%val)
{
	if (%val)
	{
		toggleZoomFOV();
	}
}

function toggleZoom(%val)
{
	if (%val)
	{
		$ZoomOn = 1;
		setFov($Pref::player::CurrentFOV);
	}
	else
	{
		$ZoomOn = 0;
		setFov($pref::Player::defaultFov);
	}
}

moveMap.bind(keyboard, r, setZoomFOV);
moveMap.bind(keyboard, e, toggleZoom);
function toggleFreeLook(%val)
{
	if (%val)
	{
		$mvFreeLook = 1;
	}
	else
	{
		$mvFreeLook = 0;
	}
}

function toggleFirstPerson(%val)
{
	if (%val)
	{
		$firstPerson = !$firstPerson;
	}
}

function toggleCamera(%val)
{
	if (%val)
	{
		commandToServer('ToggleCamera');
	}
}

moveMap.bind(keyboard, z, toggleFreeLook);
moveMap.bind(keyboard, tab, toggleFirstPerson);
moveMap.bind(keyboard, "alt c", toggleCamera);
moveMap.bindCmd(keyboard, "ctrl w", "commandToServer('playCel',\"wave\");", "");
moveMap.bindCmd(keyboard, "ctrl s", "commandToServer('playCel',\"salute\");", "");
moveMap.bindCmd(keyboard, "ctrl k", "commandToServer('suicide');", "");
moveMap.bindCmd(keyboard, "h", "commandToServer('use',\"HealthKit\");", "");
moveMap.bindCmd(keyboard, 1, "commandToServer('use',\"Rifle\");", "");
moveMap.bindCmd(keyboard, 2, "commandToServer('use',\"Crossbow\");", "");
function pageMessageHudUp(%val)
{
	if (%val)
	{
		pageUpMessageHud();
	}
}

function pageMessageHudDown(%val)
{
	if (%val)
	{
		pageDownMessageHud();
	}
}

function resizeMessageHud(%val)
{
	if (%val)
	{
		cycleMessageHudSize();
	}
}

moveMap.bind(keyboard, u, toggleMessageHud);
moveMap.bind(keyboard, "pageUp", pageMessageHudUp);
moveMap.bind(keyboard, "pageDown", pageMessageHudDown);
moveMap.bind(keyboard, "p", resizeMessageHud);
function startRecordingDemo(%val)
{
	if (%val)
	{
		startDemoRecord();
	}
}

function stopRecordingDemo(%val)
{
	if (%val)
	{
		stopDemoRecord();
	}
}

moveMap.bind(keyboard, F3, startRecordingDemo);
moveMap.bind(keyboard, F4, stopRecordingDemo);
function dropCameraAtPlayer(%val)
{
	if (%val)
	{
		commandToServer('dropCameraAtPlayer');
	}
}

function dropPlayerAtCamera(%val)
{
	if (%val)
	{
		commandToServer('DropPlayerAtCamera');
	}
}

moveMap.bind(keyboard, "F8", dropCameraAtPlayer);
moveMap.bind(keyboard, "F7", dropPlayerAtCamera);
$MFDebugRenderMode = 0;
function cycleDebugRenderMode(%val)
{
	if (!%val)
	{
		return;
	}
	if (getBuildString() $= "Debug")
	{
		if ($MFDebugRenderMode == 0)
		{
			$MFDebugRenderMode = 1;
			GLEnableOutline(1);
		}
		else if ($MFDebugRenderMode == 1)
		{
			$MFDebugRenderMode = 2;
			GLEnableOutline(0);
			setInteriorRenderMode(7);
			showInterior();
		}
		else if ($MFDebugRenderMode == 2)
		{
			$MFDebugRenderMode = 0;
			setInteriorRenderMode(0);
			GLEnableOutline(0);
			show();
		}
	}
	else
	{
		echo("Debug render modes only available when running a Debug build.");
	}
}

GlobalActionMap.bind(keyboard, "F9", cycleDebugRenderMode);
GlobalActionMap.bind(keyboard, "tilde", ToggleConsole);
GlobalActionMap.bindCmd(keyboard, "alt enter", "", "toggleFullScreen();");
GlobalActionMap.bindCmd(keyboard, "F1", "", "contextHelp();");
