function fxDTSBrick::undoTrustCheck ( %obj )
{
	%obj.chainUndoTrustCheckDown (0);
}

function fxDTSBrick::chainUndoTrustCheckDown ( %obj, %idx )
{
	%ourBrickGroup = %obj.getGroup();
	%count = %obj.getNumDownBricks();

	for ( %i = 0;  %i < $FXBrick::ChainBatchSize;  %i++ )
	{
		if ( %idx >= %count )
		{
			%obj.ChainUndoTrustCheckUp();
			return;
		}

		%checkBrick = %obj.getDownBrick(%i);

		if ( getTrustLevel(%checkBrick, %obj) < $TrustLevel::UndoBrick )
		{
			%client = %obj.client;
			%checkBrickGroup = %checkBrick.getGroup();

			%client.sendTrustFailureMessage (%checkBrickGroup);

			return;
		}

		%idx++;
	}

	%obj.schedule ($FXBrick::ChainDelay, chainUndoTrustCheckDown, %idx);
}

function fxDTSBrick::chainUndoTrustCheckUp ( %obj, %idx )
{
	%ourBrickGroup = %obj.getGroup();
	%count = %obj.getNumUpBricks();

	for ( %i = 0;  %i < $FXBrick::ChainBatchSize;  %i++ )
	{
		if ( %idx >= %count )
		{
			%obj.killBrick();
			return;
		}

		%checkBrick = %obj.getUpBrick (%i);

		if ( getTrustLevel(%checkBrick, %obj) < $TrustLevel::UndoBrick )
		{
			%client = %obj.client;
			%checkBrickGroup = %checkBrick.getGroup();

			%client.sendTrustFailureMessage (%checkBrickGroup);

			return;
		}

		%idx++;
	}

	%obj.schedule ($FXBrick::ChainDelay, ChainUndoTrustCheckUp, %idx);
}
