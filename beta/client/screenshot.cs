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
	if (%val)
	{
		%oldContent = Canvas.getContent();
		Canvas.setContent(noHudGui);
		$pref::interior::showdetailmaps = 0;
		while (1)
		{
			%fileName = "screenshot_" @ formatImageNumber($pref::screenshotNumber++);
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
		return;
	}
}

function doPanoramaScreenShot(%val)
{
	if (%val)
	{
		$pref::interior::showdetailmaps = 0;
		if ($pref::Video::screenShotFormat $= "JPEG")
		{
			panoramaScreenShot("screenshot_" @ formatImageNumber($pref::screenshotNumber++), "JPEG");
		}
		else if ($pref::Video::screenShotFormat $= "PNG")
		{
			panoramaScreenShot("screenshot_" @ formatImageNumber($pref::screenshotNumber++), "PNG");
		}
		else
		{
			panoramaScreenShot("screenshot_" @ formatImageNumber($pref::screenshotNumber++), "PNG");
		}
	}
}

