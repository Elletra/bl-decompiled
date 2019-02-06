function AvatarColorCheck ( %color )
{
	%r = mFloor ( getWord(%color, 0) * 1000 ) / 1000;
	%g = mFloor ( getWord(%color, 1) * 1000 ) / 1000;
	%b = mFloor ( getWord(%color, 2) * 1000 ) / 1000;
	%a = 1;


	if ( %r <= 0 )
	{
		%r = 0;
	}

	if ( %g <= 0 )
	{
		%g = 0;
	}

	if ( %b <= 0 )
	{
		%b = 0;
	}

	if ( %a <= 0.2)
	{
		%a = 0.2;
	}

	if ( %r > 1 )
	{
		%r = 1;
	}

	if ( %g > 1 )
	{
		%g = 1;
	}

	if ( %b > 1 )
	{
		%b = 1;
	}

	if ( %a > 1 )
	{
		%a = 1;
	}

	return %r SPC %g SPC %b SPC %a;
}

function AvatarColorCheckT ( %color )
{
	%r = getWord (%color, 0);
	%g = getWord (%color, 1);
	%b = getWord (%color, 2);
	%a = getWord (%color, 3);


	if ( %r <= 0 )
	{
		%r = 0;
	}

	if ( %g <= 0 )
	{
		%g = 0;
	}

	if ( %b <= 0 )
	{
		%b = 0;
	}

	if ( %a <= 0.2)
	{
		%a = 0.2;
	}

	if ( %r > 1 )
	{
		%r = 1;
	}

	if ( %g > 1 )
	{
		%g = 1;
	}

	if ( %b > 1 )
	{
		%b = 1;
	}

	if ( %a > 1 )
	{
		%a = 1;
	}

	return %r SPC %g SPC %b SPC %a;
}


function GameConnection::applyBodyColors ( %client )
{
	%player = %client.Player;

	if ( !isObject(%player) )
	{
		return;
	}

	if ( fileName(%player.getDataBlock().shapeFile)  !$=  "m.dts" )
	{
		if ( fileName ( %player.getDataBlock().shapeFile )  $=  "horse.dts" )
		{
			%player.setNodeColor ("body", %client.chestColor);
			%player.setNodeColor ("head", "0 0 0 1");

			return;
		}
		else
		{
			%player.setNodeColor ("ALL", %client.chestColor);
			return;
		}
	}


	%accentList = $accentsAllowed[ $hat[%client.hat] ];
	%AccentName = getWord(%accentList, %client.accent);

	if ( %AccentName !$= "none"  &&  %AccentName !$= "" )
	{
		%player.setNodeColor (%AccentName, %client.accentColor);
	}

	%hatName = $hat[%client.hat];

	if ( %hatName !$= "none" )
	{
		%player.setNodeColor (%hatName, %client.hatColor);
	}

	%player.setNodeColor ("headSkin", %client.headColor);


	%packName = $pack[%client.pack];

	if ( %packName !$= "none" )
	{
		%player.setNodeColor (%packName, %client.packColor);
	}

	%secondPackName = $SecondPack[%client.secondPack];

	if ( %secondPackName !$= "none" )
	{
		%player.setNodeColor (%secondPackName, %client.secondPackColor);
	}


	%chestName = $Chest[%client.chest];

	if ( %chestName !$= "none" )
	{
		%player.setNodeColor (%chestName, %client.chestColor);
	}


	%hipName = $Hip[%client.hip];

	if ( %hipName !$= "none" )
	{
		%player.setNodeColor (%hipName, %client.hipColor);
	}


	%LLegName = $LLeg[%client.lleg];
	%RLegName = $RLeg[%client.rleg];

	if ( %hipName $= "SkirtHip" )
	{
		%player.setNodeColor ("SkirtTrimLeft", %client.llegColor);
		%player.setNodeColor ("SkirtTrimRight", %client.rlegColor);
	}
	else
	{
		if ( %LLegName !$= "none" )
		{
			%player.setNodeColor (%LLegName, %client.llegColor);
		}

		if ( %RLegName !$= "none" )
		{
			%player.setNodeColor (%RLegName, %client.rlegColor);
		}
	}


	%LArmName = $LArm[%client.larm];

	if ( %LArmName !$= "none" )
	{
		%player.setNodeColor (%LArmName, %client.larmColor);
	}

	%RArmName = $RArm[%client.rarm];

	if ( %RArmName !$= "none" )
	{
		%player.setNodeColor (%RArmName, %client.rarmColor);
	}


	%LHandName = $LHand[%client.lhand];

	if ( %LHandName !$= "none" )
	{
		%player.setNodeColor (%LHandName, %client.lhandColor);
	}

	%RHandName = $RHand[%client.rhand];

	if ( %RHandName !$= "none" )
	{
		%player.setNodeColor (%RHandName, %client.rhandColor);
	}


	%color = getColorIDTable (%client.currentColor);

	%player.setNodeColor (lski, %color);
	%player.setNodeColor (rski, %color);

	%player.setFaceName (%client.faceName);
	%player.setDecalName (%client.decalName);
}


function servercmdupdatebodycolors ( %client, %headColor, %hatColor, %accentColor, %packColor, %secondPackColor, %chestColor, %hipColor, %LLegColor, %RLegColor, %LArmColor, %RArmColor, %LHandColor, %RHandColor, %decalName, %faceName )
{
	%currTime = getSimTime();

	if ( %client.lastUpdateBodyColorsTime + 1000  >  %currTime )
	{
		return;
	}


	%client.lastUpdateBodyColorsTime = %currTime;

	%client.hatColor = AvatarColorCheck (%hatColor);
	%client.accentColor = AvatarColorCheckT (%accentColor);

	%client.packColor = AvatarColorCheck (%packColor);
	%client.secondPackColor = AvatarColorCheck (%secondPackColor);

	%client.headColor = AvatarColorCheck (%headColor);
	%client.chestColor = AvatarColorCheck (%chestColor);
	%client.hipColor = AvatarColorCheck (%hipColor);

	%client.llegColor = AvatarColorCheck (%LLegColor);
	%client.rlegColor = AvatarColorCheck (%RLegColor);

	%client.larmColor = AvatarColorCheck (%LArmColor);
	%client.rarmColor = AvatarColorCheck (%RArmColor);

	%client.lhandColor = AvatarColorCheck (%LHandColor);
	%client.rhandColor = AvatarColorCheck (%RHandColor);


	if ( getCharCount(%decalName, "/") > 0 )
	{
		%decalName = "AAA-None";
	}

	if ( getCharCount(%faceName, "/") > 0 )
	{
		%decalName = "smiley";
	}

	%client.decalName = %decalName;
	%client.faceName = %faceName;

	%client.applyBodyParts();
	%client.applyBodyColors();
}
