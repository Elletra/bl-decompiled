exec ("./updateBodyParts.cs");
exec ("./updateBodyColors.cs");
exec ("./loadAvatar.cs");
exec ("./hideAllNodes.cs");


function applyCharacterPrefs ( %client )
{
	if ( !isObject(%client.Player) )
	{
		return;
	}

	%client.applyBodyParts();
	%client.applyBodyColors();
}

function applyDefaultCharacterPrefs ( %player )
{
	if ( !isObject(%player) )
	{
		return;
	}

	if ( fileName ( %player.getDataBlock().shapeFile )  !$=  "m.dts" )
	{
		return;
	}

	hideAllNodes (%player);

	%player.unHideNode ( $Chest[0] );
	%player.unHideNode ( $Hip[0] );
	%player.unHideNode ( $LLeg[0] );
	%player.unHideNode ( $RLeg[0] );
	%player.unHideNode ( $LArm[0] );
	%player.unHideNode ( $RArm[0] );
	%player.unHideNode ( $LHand[0] );
	%player.unHideNode ( $RHand[0] );

	%player.setHeadUp (0);


	%mainColor = "";

	if ( isObject(%player.spawnBrick)  &&  isObject(%player.spawnBrick.vehicleSpawnMarker) )
	{
		if ( %player.spawnBrick.vehicleSpawnMarker.reColorVehicle )
		{
			%mainColor = getColorIDTable (%player.spawnBrick.colorID);
		}
	}

	if ( %mainColor $= "" )
	{
		%mainColor = %player.color;
	}

	if ( %mainColor $= "" )
	{
		%mainColor = "1 1 1 1";
	}

	%mainColor = getWords (%mainColor, 0, 2) SPC 1;


	%player.setNodeColor ("headSkin", "1 1 0 1");
	%player.setNodeColor ($Chest[0], %mainColor);
	%player.setNodeColor ($Hip[0], "0 0 1 1");
	%player.setNodeColor ($LLeg[0], "0.1 0.1 0.1 1");
	%player.setNodeColor ($RLeg[0], "0.1 0.1 0.1 1");
	%player.setNodeColor ($LArm[0], %mainColor);
	%player.setNodeColor ($RArm[0], %mainColor);
	%player.setNodeColor ($LHand[0], "1 1 0 1");
	%player.setNodeColor ($RHand[0], "1 1 0 1");
	%player.setNodeColor ("headSkin", "1 1 0 1");
}

function GameConnection::validateAvatarPrefs ( %client )
{
	%client.hat = mFloor (%client.hat);
	%client.accent = mFloor (%client.accent);

	%client.pack = mFloor (%client.pack);
	%client.secondPack = mFloor (%client.secondPack);

	%client.chest = mFloor (%client.chest);
	%client.hip = mFloor (%client.hip);

	%client.lleg = mFloor (%client.lleg);
	%client.rleg = mFloor (%client.rleg);

	%client.larm = mFloor (%client.larm);
	%client.rarm = mFloor (%client.rarm);

	%client.lhand = mFloor (%client.lhand);
	%client.rhand = mFloor (%client.rhand);


	if ( $Chest[%client.chest] $= "" )
	{
		%client.chest = 0;
	}

	if ( $Hip[%client.hip] $= "" )
	{
		%client.hip = 0;
	}

	if ( $LLeg[%client.lleg] $= "" )
	{
		%client.lleg = 0;
	}

	if ( $RLeg[%client.rleg] $= "" )
	{
		%client.rleg = 0;
	}

	if ( $LArm[%client.larm] $= "" )
	{
		%client.larm = 0;
	}

	if ( $RArm[%client.rarm] $= "" )
	{
		%client.rarm = 0;
	}

	if ( $LHand[%client.lhand] $= "" )
	{
		%client.lhand = 0;
	}

	if ( $RHand[%client.rhand] $= "" )
	{
		%client.rhand = 0;
	}


	%client.hatColor = AvatarColorCheck (%client.hatColor);
	%client.accentColor = AvatarColorCheckT (%client.accentColor);

	%client.packColor = AvatarColorCheck (%client.packColor);
	%client.secondPackColor = AvatarColorCheck (%client.secondPackColor);

	%client.headColor = AvatarColorCheck (%client.headColor);
	%client.chestColor = AvatarColorCheck (%client.chestColor);
	%client.hipColor = AvatarColorCheck (%client.hipColor);

	%client.llegColor = AvatarColorCheck (%client.llegColor);
	%client.rlegColor = AvatarColorCheck (%client.rlegColor);

	%client.larmColor = AvatarColorCheck (%client.larmColor);
	%client.rarmColor = AvatarColorCheck (%client.rarmColor);

	%client.lhandColor = AvatarColorCheck (%client.lhandColor);
	%client.rhandColor = AvatarColorCheck (%client.rhandColor);
}
