function fxDTSBrick::getPrintCount ( %obj )
{
	if ( $PrintCountIdx[0] $= "" )
	{
		generatePrintCountTable();
	}

	if ( %obj.printCount $= "" )
	{
		%texture = strlwr ( getPrintTexture ( %obj.getPrintID() ) );

		for ( %i = 0;  %i < 10;  %i++ )
		{
			if ( getPrintTexture ( $PrintCountIdx[%i] ) $= %texture )
			{
				%obj.printCount = %i;
			}
		}
	}

	%obj.printCount = mFloor (%obj.printCount);
	return %obj.printCount;
}

function fxDTSBrick::incrementPrintCount ( %obj, %amt, %client )
{
	%obj.getPrintCount();
	%amt = mClamp (%amt, 1, 9);

	%obj.printCount += %amt;

	if ( %obj.printCount > 9 )
	{
		%obj.printCount -= 10;
		%obj.onPrintCountOverFlow (%client);
	}

	%obj.setPrint ( $PrintCountIdx[%obj.printCount] );
}

function fxDTSBrick::decrementPrintCount ( %obj, %amt, %client )
{
	%obj.getPrintCount();
	%amt = mClamp (%amt, 1, 9);

	%obj.printCount -= %amt;

	if ( %obj.printCount < 0 )
	{
		%obj.printCount += 10;
		%obj.onPrintCountUnderFlow (%client);
	}

	%obj.setPrint ( $PrintCountIdx[%obj.printCount] );
}

function fxDTSBrick::setPrintCount ( %obj, %count, %client )
{
	%obj.getPrintCount();
	%obj.printCount = %count;

	%obj.setPrint ( $PrintCountIdx[%obj.printCount] );
}


// =================
//  Register Events
// =================

registerOutputEvent ("fxDTSBrick", "incrementPrintCount", "int 1 9 1");
registerOutputEvent ("fxDTSBrick", "decrementPrintCount", "int 1 9 1");
registerOutputEvent ("fxDTSBrick", "setPrintCount", "int 0 9 0");
