function formatImageNumber(%number)
{
	if (%number < 10)
	{
		%number = "0" @ %number;
	}
	if (%number < 100)
	{
		%number = "0" @ %number;
	}
	if (%number < 1000)
	{
		%number = "0" @ %number;
	}
	if (%number < 10000)
	{
		%number = "0" @ %number;
	}
	return %number;
}

function recordMovie(%movieName, %fps)
{
	$timeAdvance = 1000 / %fps;
	$screenGrabThread = schedule($timeAdvance, 0, movieGrabScreen, %movieName, 0);
}

function movieGrabScreen(%movieName, %frameNumber)
{
	screenShot(%movieName @ formatImageNumber(%frameNumber) @ ".png");
	$screenGrabThread = schedule($timeAdvance, 0, movieGrabScreen, %movieName, %frameNumber + 1);
}

function stopMovie()
{
	$timeAdvance = 0;
	cancel($screenGrabThread);
}

$screenshotNumber = 0;
function doScreenShot(%val)
{
	if (!%val)
	{
		return;
	}
	%oldContent = Canvas.getContent();
	Canvas.setContent(noHudGui);
	while (1)
	{
		%fileName = "screenshots/Blockland_" @ formatImageNumber($pref::screenshotNumber++);
		if ($pref::Video::screenShotFormat $= "JPEG")
		{
			if (screenShot(%fileName @ ".jpg", "JPEG"))
			{
				break;
			}
		}
		else if (screenShot(%fileName @ ".png", "png"))
		{
			break;
		}
	}
	Canvas.setContent(%oldContent);
}

function doHudScreenShot(%val)
{
	if (!%val)
	{
		return;
	}
	while (1)
	{
		%fileName = "screenshots/Blockland_" @ formatImageNumber($pref::screenshotNumber++);
		if ($pref::Video::screenShotFormat $= "JPEG")
		{
			if (dofScreenShot(%fileName @ ".jpg", "JPEG"))
			{
				break;
			}
		}
		else if (dofScreenShot(%fileName @ ".png", "png"))
		{
			break;
		}
	}
}

function doDofScreenshot(%val)
{
	if (!%val)
	{
		return;
	}
	$lockBuffers = 0;
	%oldContent = Canvas.getContent();
	Canvas.setContent(noHudGui);
	%oldShowNames = NoHudGui_ShapeNameHud.isVisible();
	NoHudGui_ShapeNameHud.setVisible(0);
	if (!$dofDisableAutoFocus)
	{
		$dofNear = getFocusDistance() * 2;
	}
	while (1)
	{
		%fileName = "screenshots/Blockland_" @ formatImageNumber($pref::screenshotNumber++);
		if ($pref::Video::screenShotFormat $= "JPEG")
		{
			if (dofScreenShot(%fileName @ ".jpg", "JPEG"))
			{
				break;
			}
		}
		else if (dofScreenShot(%fileName @ ".png", "png"))
		{
			break;
		}
	}
	$lockBuffers = 0;
	NoHudGui_ShapeNameHud.setVisible(%oldShowNames);
	Canvas.setContent(%oldContent);
}

function doPanoramaScreenShot(%val)
{
	if (%val)
	{
		$pref::interior::showdetailmaps = 0;
		if ($pref::Video::screenShotFormat $= "JPEG")
		{
			panoramaScreenShot("screenshots/Blockland_" @ formatImageNumber($pref::screenshotNumber++), "JPEG");
		}
		else if ($pref::Video::screenShotFormat $= "PNG")
		{
			panoramaScreenShot("screenshots/Blockland_" @ formatImageNumber($pref::screenshotNumber++), "PNG");
		}
		else
		{
			panoramaScreenShot("screenshots/Blockland_" @ formatImageNumber($pref::screenshotNumber++), "PNG");
		}
	}
}

function dofPreview(%val)
{
	if (isEventPending($dofPreviewSchedule))
	{
		cancel($dofPreviewSchedule);
	}
	if (!%val)
	{
		$dofX = 0;
		$dofY = 0;
		return;
	}
	if (mAbs($dofX) != mAbs($dofScale))
	{
		$dofX = $dofScale;
	}
	$dofX *= -1;
	$dofPreviewSchedule = schedule(30, 0, dofPreview, 1);
}

