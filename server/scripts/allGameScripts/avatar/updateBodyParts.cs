function GameConnection::applyBodyParts ( %client )
{
	%player = %client.Player;

	if ( !isObject(%player) )
	{
		return;
	}

	if ( fileName(%player.getDataBlock().shapeFile) !$= "m.dts" )
	{
		return;
	}

	%client.validateAvatarPrefs();

	hideAllNodes (%player);


	%accentList = $accentsAllowed[ $hat[%client.hat] ];
	%AccentName = getWord (%accentList, %client.accent);

	if ( %AccentName !$= "none" )
	{
		%player.unHideNode (%AccentName);
	}


	%player.unHideNode ( $hat[%client.hat] );
	%player.unHideNode ( $Chest[%client.chest] );

	if ( $pack[%client.pack] !$= "none" )
	{
		%player.unHideNode ( $pack[%client.pack] );
	}

	if ( $SecondPack[%client.secondPack] !$= "none" )
	{
		%player.unHideNode ( $SecondPack[%client.secondPack] );
	}

	if ( $Hip[%client.hip] $= "SkirtHip" )
	{
		%player.unHideNode (skirtHip);
		%player.unHideNode (skirtTrimLeft);
		%player.unHideNode (skirtTrimRight);
	}
	else
	{
		%player.unHideNode ( $Hip[%client.hip] );
		%player.unHideNode ( $LLeg[%client.lleg] );
		%player.unHideNode ( $RLeg[%client.rleg] );
	}


	%player.unHideNode ( $LArm[%client.larm] );
	%player.unHideNode ( $RArm[%client.rarm] );
	%player.unHideNode ( $LHand[%client.lhand] );
	%player.unHideNode ( $RHand[%client.rhand] );


	if ( %client.pack == 0  &&  %client.secondPack == 0 )
	{
		%player.setHeadUp (0);
	}
	else
	{
		%player.setHeadUp (1);
	}


	%vehicle = %player.getObjectMount();

	if ( %vehicle  &&  isObject(skiVehicle) )
	{
		if ( %vehicle.getDataBlock().getId() == skiVehicle.getId() )
		{
			%player.unHideNode (lski);
			%player.unHideNode (rski);

			%color = getColorIDTable (%client.currentColor);

			%player.setNodeColor ("LSki", %color);
			%player.setNodeColor ("RSki", %color);
		}
	}
}


function servercmdupdatebodyparts ( %client, %hat, %accent, %pack, %secondPack, %chest, %hip, %LLeg, %RLeg, %LArm, %RArm, %LHand, %RHand )
{
	%currTime = getSimTime();

	if ( %client.lastUpdateBodyPartsTime + 1000  >  %currTime )
	{
		return;
	}


	%client.lastUpdateBodyPartsTime = %currTime;

	%client.hat = mFloor (%hat);
	%client.accent = mFloor (%accent);
	%client.pack = mFloor (%pack);
	%client.secondPack = mFloor (%secondPack);
	%client.chest = mFloor (%chest);
	%client.hip = mFloor (%hip);
	%client.lleg = mFloor (%LLeg);
	%client.rleg = mFloor (%RLeg);
	%client.larm = mFloor (%LArm);
	%client.rarm = mFloor (%RArm);
	%client.lhand = mFloor (%LHand);
	%client.rhand = mFloor (%RHand);


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

	%client.applyBodyParts();
	%client.applyBodyColors();
}
