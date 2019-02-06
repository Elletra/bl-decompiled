exec ("./buyBrick.cs");
exec ("./useInventory.cs");
exec ("./onUse.cs");


function serverCmdClearInventory ( %client )
{
	%player = %client.player;

	if ( !isObject(%player) )
	{
		return;
	}


	%maxItems = %player.getDataBlock().maxItems;

	for ( %i = 0;  %i < %maxItems;  %i++ )
	{
		%client.inventory[%i] = "";
	}
}
