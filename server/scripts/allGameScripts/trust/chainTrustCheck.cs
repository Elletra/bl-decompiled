$FXBrick::ChainBatchSize = 100;
$FXBrick::ChainDelay = 10;


function fxDTSBrick::chainTrustCheckDown ( %obj, %idx )
{
	%ourBrickGroup = %obj.getGroup();
	%count = %obj.getNumDownBricks();

	for ( %i = 0;  %i < $FXBrick::ChainBatchSize;  %i++ )
	{
		if ( %idx >= %count )
		{
			%obj.ChainTrustCheckUp();
			return;
		}

		%checkBrick = %obj.getDownBrick (%i);
		%brickGroup = %checkBrick.getGroup();

		if ( %brickGroup.bl_id == 888888 )
		{
			%obj.isBasePlate = 1;
		}
		else if ( getTrustLevel(%checkBrick, %obj) < $TrustLevel::BuildOn )
		{
			%client = %obj.client;
			%checkBrickGroup = %checkBrick.getGroup();

			%client.sendTrustFailureMessage (%checkBrickGroup);
			%obj.TrustCheckFailed();

			return;
		}

		%idx++;
	}

	%obj.schedule ($FXBrick::ChainDelay, chainTrustCheckDown, %idx);
}

function fxDTSBrick::chainTrustCheckUp ( %obj, %idx )
{
	%ourBrickGroup = %obj.getGroup();
	%count = %obj.getNumUpBricks();
	
	for ( %i = 0;  %i < $FXBrick::ChainBatchSize;  %i++ )
	{
		if ( %idx >= %count )
		{
			if ( %obj.getDataBlock().isWaterBrick )
			{
				%obj.chainTrustCheckVolume();
			}
			else
			{
				%obj.trustCheckFinished();
			}

			return;
		}


		%checkBrick = %obj.getUpBrick (%i);
		%brickGroup = %checkBrick.getGroup();

		if ( %brickGroup.bl_id == 888888 )
		{
			%obj.isBasePlate = 1;
		}
		else if ( getTrustLevel(%checkBrick, %obj) < $TrustLevel::BuildOn )
		{
			%client = %obj.client;
			%checkBrickGroup = %checkBrick.getGroup();

			%client.sendTrustFailureMessage (%checkBrickGroup);
			%obj.trustCheckFailed();

			return;
		}

		%idx++;
	}

	%obj.schedule ($FXBrick::ChainDelay, chainTrustCheckUp, %idx);
}

function fxDTSBrick::chainTrustCheckVolume ( %obj, %idx )
{
	%ourBrickGroup = %obj.getGroup();

	%client = %obj.client;

	%pos = %obj.getPosition();
	%data = %obj.getDataBlock();

	%size = %data.brickSizeX / 2 - 0.05 SPC %data.brickSizeY / 2 - 0.05 SPC %data.brickSizeZ / 2 - 0.05;

	%mask = $TypeMasks::FxBrickAlwaysObjectType;

	initContainerBoxSearch (%pos, %size, %mask);

	while ( ( %checkBrick = containerSearchNext() )  != 0 )
	{
		%brickGroup = %checkBrick.getGroup();

		if ( %brickGroup.bl_id == 888888 )
		{
			%obj.isBasePlate = 1;
		}
		else if ( getTrustLevel(%checkBrick, %obj) < $TrustLevel::BuildOn )
		{
			%checkBrickGroup = %checkBrick.getGroup();

			%client.sendTrustFailureMessage (%checkBrickGroup);
			%obj.trustCheckFailed();

			return;
		}
	}

	%obj.trustCheckFinished();
}
