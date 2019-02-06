function makePadString ( %char, %num )
{
	for ( %i = 0;  %i < %num;  %i++ )
	{
		%ret = %ret @ %char;
	}

	return %ret;
}

function verifyBrickUINames ()
{
	%size = getDataBlockGroupSize();

	for ( %i = 0;  %i < %size;  %i++ )
	{
		%db = getDataBlock (%i);

		if ( %db.getClassName() $= "fxDTSBrickData" )
		{
			if ( %db.uiName $= "" )
			{
				error ("ERROR: Brick datablock " @  %db.getName()  @ " has no uiname");
			}
			else if ( %uiNamePresent[%db.uiName] )
			{
				if ( %db.category !$= ""  &&  %db.subCategory !$= "" )
				{
					error ("ERROR: Brick datablock " @  %db.getName()  @ "\" has the same uiname as " @  
						%uiNamePresent[%db.uiName].getName()  @ " (" @  %db.uiName  @ ") - removing.");

					%db.uiName = "";
				}
			}
			else if ( %db.category !$= ""  &&  %db.subCategory !$= "" )
			{
				%uiNamePresent[%db.uiName] = %db;
			}
		}
	}
}
