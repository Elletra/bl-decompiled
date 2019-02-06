function serverCmdUndoBrick ( %client )
{
	%line = %client.undoStack.pop();
	%obj = getField (%line, 0);
	%player = %client.Player;


	if ( isObject(%obj) )
	{
		%action = getField (%line, 1);

		if ( isObject(%player) )
		{
			%player.playThread (3, undo);
		}

		if ( %action !$= "COLORGENERIC" )
		{
			if ( !%obj.getType() & $TypeMasks::FxBrickAlwaysObjectType )
			{
				error ("ERROR: ServerCmdUndoBrick(" @  %client  @ ") - " @  %obj  @ " is not a brick");
				return;
			}
		}


		if ( %action $= "PLANT" )
		{
			if ( %obj.getGroup() != %client.brickGroup )
			{
				return;
			}

			if ( %obj.willCauseChainKill() )
			{
				%obj.undoTrustCheck();
			}
			else
			{
				%obj.killBrick();
			}
		}
		else if ( %action $= "COLOR" )
		{
			if ( getTrustLevel(%obj, %client) < $TrustLevel::UndoPaint )
			{
				%client.sendTrustFailureMessage (%brickGroup);
				return;
			}


			%oldColor = getField (%line, 2);
			%obj.setColor (%oldColor);

			if ( isObject(%obj.Vehicle) )
			{
				if ( %obj.reColorVehicle )
				{
					%obj.colorVehicle();
				}
			}
		}
		else if ( %action $= "COLORFX" )
		{
			if ( getTrustLevel(%obj, %client) < $TrustLevel::UndoFXPaint )
			{
				%client.sendTrustFailureMessage (%brickGroup);
				return;
			}

			%oldColorFX = getField (%line, 2);
			%obj.setColorFX (%oldColorFX);
		}
		else if ( %action $= "SHAPEFX" )
		{
			if ( getTrustLevel(%obj, %client) < $TrustLevel::UndoFXPaint )
			{
				%client.sendTrustFailureMessage (%brickGroup);
				return;
			}

			%oldShapeFX = getField (%line, 2);
			%obj.setShapeFX (%oldShapeFX);
		}
		else if ( %action $= "PRINT" )
		{
			if ( getTrustLevel(%obj, %client) < $TrustLevel::UndoPrint )
			{
				%client.sendTrustFailureMessage (%brickGroup);
				return;
			}

			%oldPrintID = getField (%line, 2);
			%obj.setPrint (%oldPrintID);
		}
		else if ( %action $= "COLORGENERIC" )
		{
			%oldColor = getField (%line, 2);
			%obj.setNodeColor ("ALL", %oldColor);
			%obj.color = %oldColor;
		}
		else
		{
			error ("ERROR: ServerCmdUndoBrick() - unknown undo state " @  %line  @ "");
		}
	}
}
