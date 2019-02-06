function serverCmdSetPrint ( %client, %index )
{
	%printBrick = getWord (%client.printBrick, 0);

	if ( isObject(%printBrick) )
	{
		%ar = %printBrick.getDataBlock().printAspectRatio;

		if ( %index >= $printARStart[%ar]  &&  %index < $printARStart[%ar] + $printARNumPrints[%ar] )
		{
			if ( %printBrick.getPrintID() != %index )
			{
				%client.undoStack.push ( %printBrick  TAB "PRINT" TAB  %printBrick.getPrintID() );
				%printBrick.setPrint (%index);
			}
		}

		if ( %index >= $printARStart["Letters"]  &&  %index < $printARStart["Letters"] + $printARNumPrints["Letters"] )
		{
			if ( %printBrick.getPrintID() != %index )
			{
				%client.undoStack.push ( %printBrick  TAB "PRINT" TAB  %printBrick.getPrintID() );
				%printBrick.setPrint (%index);
			}
		}


		%client.lastPrint[%ar] = %index;
		%player = %client.Player;

		if ( isObject(%player) )
		{
			if ( isObject(%player.tempBrick) )
			{
				%data = %player.tempBrick.getDataBlock();
				%aspectRatio = %data.printAspectRatio;

				if ( %aspectRatio $= %ar )
				{
					%player.tempBrick.setPrint ( %client.lastPrint[%aspectRatio] );
				}
			}
		}
	}

	%client.printBrick = 0;
}
