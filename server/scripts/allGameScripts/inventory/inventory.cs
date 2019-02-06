function ShapeBase::maxInventory ( %this, %data )
{
	return %this.getDataBlock().maxInv[ %data.getName() ];
}

function ShapeBase::getInventory ( %this, %data )
{
	if ( !isObject(%data) )
	{
		error ("ERROR: ShapeBase::getInventory() - datablock " @  %data  @ " does not exist.");
		return 0;
	}

	return %this.inv[ %data.getName() ];
}

function ammo::onInventory ( %this, %obj, %amount )
{
	for ( %i = 0;  %i < 8;  %i++ )
	{
		%image = %obj.getMountedImage (%i);

		if ( %obj.getMountedImage(%i) > 0 )
		{
			if ( isObject(%image.ammo)  &&  %image.ammo.getId() == %this.getId() )
			{
				%obj.setImageAmmo (%i, %amount != 0);
			}
		}
	}
}

function ShapeBase::onInventory ( %this, %data, %value )
{
	// Your code here
}

function ShapeBaseData::onInventory ( %this, %user, %value )
{
	// Your code here
}

function Weapon::onInventory ( %this, %obj, %amount )
{
	%slot = %obj.getMountSlot (%this.image);

	if ( !%amount  &&  %obj.getMountSlot(%this.image) != -1 )
	{
		%obj.unmountImage (%slot);
	}
}


exec ("./modify.cs");
exec ("./item.cs");
exec ("./pickup.cs");
exec ("./use.cs");
exec ("./throw.cs");
