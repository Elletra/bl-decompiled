function Player::setTempColor ( %player, %color, %time, %position, %projectileData )
{
	if ( isEventPending(%player.tempColorSchedule) )
	{
		cancel (%player.tempColorSchedule);
	}


	%playerZ = getWord (%player.getPosition(), 2);
	%projectileZ = getWord (%position, 2);
	%zDiff = %projectileZ - %playerZ;

	%client = %player.client;

	if ( isObject(%client)  &&  %position !$= ""  &&  fileBase(%player.getDataBlock().shapeFile) $= "m" )
	{
		if ( %zDiff < 0.63 )
		{
			%player.setNodeColor ( $LLeg[%client.lleg],  %color );
			%player.setNodeColor ( $RLeg[%client.rleg],  %color );
		}
		else if ( %zDiff < 1.04 )
		{
			%player.setNodeColor ( $Hip[%client.hip],  %color );
			%player.setNodeColor ( $LHand[%client.lhand],  %color );
			%player.setNodeColor ( $RHand[%client.rhand],  %color );
		}
		else if ( %zDiff < 1.72 )
		{
			%player.setNodeColor ( $Chest[%client.chest],  %color );
			%player.setNodeColor ( $LArm[%client.larm],  %color );
			%player.setNodeColor ( $RArm[%client.rarm],  %color );

			%player.setDecalName ("AAA-None");
		}
		else if ( %zDiff < 1.98 )
		{
			if ( %client.pack > 0 )
			{
				%player.setNodeColor ( $pack[%client.pack],  %color );
			}

			if ( %client.secondPack > 0 )
			{
				%player.setNodeColor ( $SecondPack[%client.secondPack],  %color );
			}
		}
		else if ( %zDiff < 2.35 )
		{
			%player.setNodeColor ("headskin", %color);
		}
		else
		{
			if ( %client.hat > 0 )
			{
				%player.setNodeColor ( $hat[%client.hat],  %color );
			}

			if ( %client.accent > 0 )
			{
				%player.setNodeColor ( $Accent[%client.accent],  %color );
			}
		}
	}
	else if ( %position !$= ""  &&  fileBase(%player.getDataBlock().shapeFile) $= "m" )
	{
		if ( %zDiff < 0.63 )
		{
			for ( %i = 0;  %i < $num["LLeg"];  %i++ )
			{
				%player.setNodeColor ( $LLeg[%i],  %color );
			}
			
			for ( %i = 0;  %i < $num["RLeg"];  %i++ )
			{
				%player.setNodeColor ( $RLeg[%i],  %color );
			}
		}
		else if ( %zDiff < 1.04 )
		{
			for ( %i = 0;  %i < $num["Hip"];  %i++ )
			{
				%player.setNodeColor ( $Hip[%i],  %color );
			}
			
			for ( %i = 0;  %i < $num["LHand"];  %i++ )
			{
				%player.setNodeColor ( $LHand[%i],  %color );
			}

			for ( %i = 0;  %i < $num["RHand"];  %i++ )
			{
				%player.setNodeColor ( $RHand[%i],  %color );
			}
		}
		else if ( %zDiff < 1.72 )
		{
			for ( %i = 0;  %i < $num["Chest"];  %i++ )
			{
				%player.setNodeColor ( $Chest[%i],  %color );
			}

			for ( %i = 0;  %i < $num["Larm"];  %i++ )
			{
				%player.setNodeColor ( $LArm[%i],  %color );
			}

			for ( %i = 0;  %i < $num["Rarm"];  %i++ )
			{
				%player.setNodeColor ( $RArm[%i],  %color );
			}

			%player.setDecalName ("AAA-None");
		}
		else if ( %zDiff < 1.98 )
		{
			for ( %i = 1;  %i < $num["Pack"];  %i++ )
			{
				%player.setNodeColor ( $pack[%i],  %color );
			}

			for ( %i = 1;  %i < $num["SecondPack"];  %i++ )
			{
				%player.setNodeColor ( $SecondPack[%i],  %color);
			}
		}
		else if (%zDiff < 2.35)
		{
			%player.setNodeColor ("headskin", %color);
		}
		else
		{
			for ( %i = 1;  %i < $num["Hat"];  %i++ )
			{
				%player.setNodeColor ( $hat[%i],  %color );
			}
			
			for ( %i = 1;  %i < $num["Accent"];  %i++ )
			{
				%player.setNodeColor ( $Accent[%i],  %color );
			}
		}
	}
	else
	{
		%player.setNodeColor ("ALL", %color);
		%player.setDecalName ("AAA-None");
	}


	if ( %time > 0 )
	{
		if ( isObject(%player.client) )
		{
			%player.tempColorSchedule = %player.schedule (%time, ClearTempColor, %projectileData);
		}
		else if ( isObject(%player.spawnBrick) )
		{
			%player.tempColorSchedule = %player.spawnBrick.schedule (%time, colorVehicle);
		}
	}
}

function Player::clearTempColor ( %player, %projectileData )
{
	if ( isEventPending(%player.tempColorSchedule) )
	{
		cancel (%player.tempColorSchedule);
	}


	if ( isObject(%projectileData) )
	{
		%p = new Projectile ()
		{
			dataBlock = %projectileData;
			initialPosition = %player.getHackPosition();
		};

		if ( isObject(%p) )
		{
			MissionCleanup.add (%p);
			%scale = VectorScale (%player.getScale(), 2);

			%p.setScale (%scale);
			%p.explode();
		}
	}

	if ( isObject(%player.client) )
	{
		%player.client.applyBodyColors();
	}
}
