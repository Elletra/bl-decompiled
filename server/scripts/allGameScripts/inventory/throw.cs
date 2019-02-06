function ShapeBase::throwObject ( %this, %obj )
{
	%throwForce = %this.throwForce;

	if ( !%throwForce )
	{
		%throwForce = 20;
	}

	%eye = %this.getEyeVector();
	%vec = VectorScale (%eye, %throwForce);

	%verticalForce = %throwForce / 2;

	%dot = VectorDot ("0 0 1", %eye);

	if ( %dot < 0 )
	{
		%dot = -%dot;
	}

	%vec = VectorAdd ( %vec, VectorScale("0 0 " @  %verticalForce, 1 - %dot) );
	%vec = VectorAdd ( %vec, %this.getVelocity() );

	%pos = getBoxCenter ( %this.getWorldBox() );

	%obj.setTransform (%pos);
	%obj.applyImpulse (%pos, %vec);
	%obj.setCollisionTimeout (%this);
}

function ShapeBase::throw ( %this, %data, %amount )
{
	if ( %this.getInventory(%data) > 0 )
	{
		%obj = %data.onThrow (%this, %amount);

		if ( %obj )
		{
			%this.throwObject (%obj);
			return true;
		}
	}

	return false;
}

function ShapeBaseData::onThrow ( %this, %user, %amount )
{
	return 0;
}


function Item::setThrower ( %this, %newThrower )
{
	%this.thrower = %newThrower;
}

function ItemData::onThrow ( %this, %user, %amount )
{
	if ( %amount $= "" )
	{
		%amount = 1;
	}

	if ( %this.maxInventory !$= "" )
	{
		if ( %amount > %this.maxInventory )
		{
			%amount = %this.maxInventory;
		}
	}

	if ( !%amount )
	{
		return 0;
	}


	%user.decInventory (%this, %amount);

	%obj = new Item()
	{
		dataBlock = %this;
		rotation = "0 0 1 " @ getRandom() * 360;
		count = %amount;
	};
	MissionGroup.add (%obj);

	%obj.schedulePop();
	return %obj;
}


function serverCmdDropTool ( %client, %position )
{
	%player = %client.Player;

	if ( !isObject(%player) )
	{
		return;
	}


	%item = %player.tool[%position];

	if ( isObject(%item) )
	{
		if ( %item.canDrop == 1 )
		{
			%zScale = getWord (%player.getScale(), 2);

			%muzzlepoint = VectorAdd (%player.getPosition(), "0 0" SPC  1.5 * %zScale);
			%muzzlevector = %player.getEyeVector();
			%muzzlepoint = VectorAdd (%muzzlepoint, %muzzlevector);

			%playerRot = rotFromTransform ( %player.getTransform() );

			%thrownItem = new Item ()
			{
				dataBlock = %item;
			};

			%thrownItem.setScale ( %player.getScale() );
			MissionCleanup.add (%thrownItem);

			%thrownItem.setTransform (%muzzlepoint  @ " " @  %playerRot);
			%thrownItem.setVelocity ( VectorScale(%muzzlevector, 20 * %zScale) );

			%thrownItem.schedulePop();

			%thrownItem.miniGame = %client.miniGame;
			%thrownItem.bl_id = %client.getBLID();
			%thrownItem.setCollisionTimeout (%player);

			if ( %item.className $= "Weapon" )
			{
				%player.weaponCount--;
			}


			%player.tool[%position] = 0;
			messageClient (%client, 'MsgItemPickup', '', %position, 0);

			if ( %player.getMountedImage(%item.image.mountPoint)  >  0 )
			{
				if ( %player.getMountedImage(%item.image.mountPoint).getId() == %item.image.getId() )
				{
					%player.unmountImage (%item.image.mountPoint);
				}
			}
		}
	}
}
