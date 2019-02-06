function createUINameTable ()
{
	$UINameTableCreated = 1;
	%dbCount = getDataBlockGroupSize();

	for ( %i = 0;  %i < %dbCount;  %i++ )
	{
		%db = getDataBlock (%i);

		if ( %db.uiName !$= "" )
		{
			if ( %db.getClassName() $= "FxDTSBrickData" )
			{
				$uiNameTable[%db.uiName] = %db;
			}
			else if ( %db.getClassName() $= "FxLightData" )
			{
				if ( %db.uiName !$= "" )
				{
					$uiNameTable_Lights[%db.uiName] = %db;
				}
			}
			else if ( %db.getClassName() $= "ParticleEmitterData" )
			{
				if ( %db.uiName !$= "" )
				{
					$uiNameTable_Emitters[%db.uiName] = %db;
				}
			}
			else if ( %db.getClassName() $= "ItemData" )
			{
				if ( %db.uiName !$= "" )
				{
					$uiNameTable_Items[%db.uiName] = %db;
				}
			}
			else if ( %db.getClassName() $= "AudioProfile" )
			{
				if ( %db.uiName !$= "" )
				{
					if ( %db.getDescription().isLooping )
					{
						$uiNameTable_Music[%db.uiName] = %db;
					}
					else
					{
						$uiNameTable_Sounds[%db.uiName] = %db;
					}
				}
			}
			else if ( %db.getClassName() $= "PlayerData" )
			{
				if ( %db.uiName !$= ""  &&  %db.rideAble )
				{
					$uiNameTable_Vehicle[%db.uiName] = %db;
				}

				$uiNameTable_Player[%db.uiName] = %db;
			}
			else if ( %db.getClassName() $= "WheeledVehicleData" )
			{
				if ( %db.uiName !$= ""  &&  %db.rideAble )
				{
					$uiNameTable_Vehicle[%db.uiName] = %db;
				}
			}
			else if ( %db.getClassName() $= "FlyingVehicleData" )
			{
				if ( %db.uiName !$= ""  &&  %db.rideAble )
				{
					$uiNameTable_Vehicle[%db.uiName] = %db;
				}
			}
			else if ( %db.getClassName() $= "HoverVehicleData" )
			{
				if ( %db.uiName !$= ""  &&  %db.rideAble )
				{
					$uiNameTable_Vehicle[%db.uiName] = %db;
				}
			}
		}
	}
}
