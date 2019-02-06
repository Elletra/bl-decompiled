function serverCmdUseSprayCan ( %client, %index )
{
	if ( isObject(%client.miniGame) )
	{
		if ( !%client.miniGame.EnablePainting )
		{
			return;
		}
	}

	%index = mFloor (%index);
	%player = %client.Player;
	%color = %index;

	if ( !isObject(%player) )
	{
		return;
	}


	if ( %client.isTalking )
	{
		serverCmdStopTalking (%client);
	}


	%image = nameToID ("color" @  %color  @ "SprayCanImage");

	if ( isObject(%image) )
	{
		%client.currentColor = %index;
		%player.updateArm (%image);
		%player.mountImage (%image, $RightHandSlot, 1, %image.skinName);
	}
	else
	{
		return;
	}


	if ( isObject(%player.tempBrick) )
	{
		%player.tempBrick.setColor (%index);
	}

	%player.currSprayCan = %index;
	%player.currFXCan = -1;
	%player.currTool = -1;

	return;


	%mountedImage = %player.getMountedImage ($RightHandSlot);

	if ( %mountedImage.Item $= "sprayCan"  &&  %player.currWeaponSlot == %invPosition )
	{
		%color++;

		if ( %color > $maxSprayColors )
		{
			%color = 0;
		}

		%image = nameToID ("color" @  %color  @ "SprayCanImage");
		%player.mountImage (%image, $RightHandSlot, 1, %image.skinName);

		%client.color = %color;
	}
	else
	{
		if ( %color !$= "" )
		{
			%image = nameToID ("color" @  %color  @ "SprayCanImage");
			%player.mountImage (%image, $RightHandSlot, 1, %image.skinName);
		}
		else
		{
			%image = nameToID ("color0SprayCanImage");
			%player.mountImage (%image, $RightHandSlot, 1, %image.skinName);
			%client.color = 0;
		}

		%player.currWeaponSlot = %invPosition;
	}

	return;

	%player = %client.Player;
	%color = %client.color;


	%mountedImage = %player.getMountedImage ($RightHandSlot);

	if ( %mountedImage.Item $= "sprayCan" )
	{
		if ( %mountedImage == nameToID("redSprayCanImage") )
		{
			%image = yellowSprayCanImage;
			%client.color = "yellow";
		}
		else if ( %mountedImage == nameToID("yellowSprayCanImage") )
		{
			%image = greenSprayCanImage;
			%client.color = "green";
		}
		else if ( %mountedImage == nameToID("greenSprayCanImage") )
		{
			%image = blueSprayCanImage;
			%client.color = "blue";
		}
		else if ( %mountedImage == nameToID("blueSprayCanImage") )
		{
			%image = whiteSprayCanImage;
			%client.color = "white";
		}
		else if ( %mountedImage == nameToID("whiteSprayCanImage") )
		{
			%image = graySprayCanImage;
			%client.color = "gray";
		}
		else if ( %mountedImage == nameToID("graySprayCanImage") )
		{
			%image = grayDarkSprayCanImage;
			%client.color = "grayDark";
		}
		else if ( %mountedImage == nameToID("grayDarkSprayCanImage") )
		{
			%image = blackSprayCanImage;
			%client.color = "black";
		}
		else if ( %mountedImage == nameToID("blackSprayCanImage") )
		{
			%image = redSprayCanImage;
			%client.color = "red";
		}
		else if ( %mountedImage == nameToID("brownSprayCanImage") )
		{
			%image = redSprayCanImage;
			%client.color = "red";
		}

		%player.mountImage (%image, $RightHandSlot, 1, %image.skinName);
	}
	else
	{
		if ( %color !$= "" )
		{
			%image = nameToID (%color  @ "SprayCanImage");
			%player.mountImage (%image, $RightHandSlot, 1, %image.skinName);
		}
		else
		{
			%image = redSprayCanImage;
			%player.mountImage (%image, $RightHandSlot, 1, %image.skinName);
		}

		messageClient (%client, 'MsgHilightInv', '', -1);
		%player.currWeaponSlot = -1;
	}
}

function serverCmdUseFXCan ( %client, %index )
{
	if ( isObject(%client.miniGame) )
	{
		if ( !%client.miniGame.EnablePainting )
		{
			return;
		}
	}


	%player = %client.Player;
	%index = mFloor (%index);

	if ( !isObject(%player) )
	{
		return;
	}

	if ( %client.isTalking )
	{
		serverCmdStopTalking (%client);
	}


	if ( %index == 0 )
	{
		%image = flatSprayCanImage;
	}
	else if ( %index == 1 )
	{
		%image = pearlSprayCanImage;
	}
	else if ( %index == 2 )
	{
		%image = chromeSprayCanImage;
	}
	else if ( %index == 3 )
	{
		%image = glowSprayCanImage;
	}
	else if ( %index == 4 )
	{
		%image = blinkSprayCanImage;
	}
	else if ( %index == 5 )
	{
		%image = swirlSprayCanImage;
	}
	else if ( %index == 6 )
	{
		%image = rainbowSprayCanImage;
	}
	else if ( %index == 7 )
	{
		%image = stableSprayCanImage;
	}
	else if ( %index == 8 )
	{
		%image = jelloSprayCanImage;
	}
	else
	{
		return;
	}

	%player.currSprayCan = -1;
	%player.currFXCan = %index;
	%player.currTool = -1;

	%player.updateArm (%image);
	%player.mountImage (%image, $RightHandSlot, 1);
}
