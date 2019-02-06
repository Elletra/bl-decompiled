$ItemTime = 10000;
$ItemDropTime = 1000;

$Item::RespawnTime = 4 * 1000;
$Item::PopTime = 10 * 1000;


function ItemData::create ( %data )
{
	%obj = new Item()
	{
		dataBlock = %data;
		static = 1;
		rotate = %data.rotate;
	};
	%obj.setSkinName (%data.skinName);

	return %obj;
}

function ItemData::onAdd ( %this, %obj )
{
	%obj.setSkinName (%this.skinName);

	if ( %this.image.doColorShift )
	{
		%obj.setNodeColor ("ALL", %this.image.colorShiftColor);
	}

	%obj.canPickup = 1;
}

function ItemData::onCollision ( %obj )
{
	// Your code here
}


function Item::fadeOut ( %obj )
{
	%obj.startFade (0, 0, 1);

	if ( %obj.getDataBlock().doColorShift )
	{
		%color = getWords (%obj.getDataBlock().colorShiftColor, 0, 2);
		%obj.setNodeColor ("ALL", %color  SPC 0.25);
	}
	else
	{
		%obj.setNodeColor ("ALL", "1 1 1 0.25");
	}

	%obj.canPickup = 0;
}

function Item::fadeIn ( %obj, %delay )
{
	if ( isEventPending(%obj.fadeInSchedule) )
	{
		cancel (%obj.fadeInSchedule);
	}

	if ( %delay > 0 )
	{
		%obj.fadeInSchedule = %obj.schedule (%delay, "fadeIn");
		return;
	}


	%obj.startFade (0, 0, 0);
	%obj.setNodeColor ("ALL", "1 1 1 0.25");

	if ( %obj.getDataBlock().image.doColorShift )
	{
		%obj.setNodeColor ("ALL", %obj.getDataBlock().image.colorShiftColor);
	}
	else
	{
		%obj.setNodeColor ("ALL", "1 1 1 1");
	}

	%obj.canPickup = 1;
}

function Item::respawn ( %obj )
{
	%obj.fadeOut();

	if ( isObject(%obj.spawnBrick) )
	{
		%obj.fadeIn (%obj.spawnBrick.itemRespawnTime);
	}
	else
	{
		%obj.fadeIn ($Game::Item::RespawnTime);
	}
}

function Item::schedulePop ( %obj )
{
	%oldQuotaObject = getCurrentQuotaObject();

	if ( isObject(%oldQuotaObject) )
	{
		clearCurrentQuotaObject();
	}

	%obj.schedule ($Game::Item::PopTime - 1000, "startFade", 1000, 0, 1);

	if ( %obj.getDataBlock().doColorShift )
	{
		%color = getWords (%obj.getDataBlock().colorShiftColor, 0, 2);

		%obj.schedule ($Game::Item::PopTime - 1000, "setNodeColor", "ALL", %color  SPC 0.5);
		%obj.schedule ($Game::Item::PopTime - 800,  "setNodeColor", "ALL", %color  SPC 0.4);
		%obj.schedule ($Game::Item::PopTime - 600,  "setNodeColor", "ALL", %color  SPC 0.3);
		%obj.schedule ($Game::Item::PopTime - 400,  "setNodeColor", "ALL", %color  SPC 0.2);
		%obj.schedule ($Game::Item::PopTime - 200,  "setNodeColor", "ALL", %color  SPC 0.1);
	}
	else
	{
		%obj.schedule ($Game::Item::PopTime - 1000, "setNodeColor", "ALL", "1 1 1 0.5");
		%obj.schedule ($Game::Item::PopTime - 800,  "setNodeColor", "ALL", "1 1 1 0.4");
		%obj.schedule ($Game::Item::PopTime - 600,  "setNodeColor", "ALL", "1 1 1 0.3");
		%obj.schedule ($Game::Item::PopTime - 400,  "setNodeColor", "ALL", "1 1 1 0.2");
		%obj.schedule ($Game::Item::PopTime - 200,  "setNodeColor", "ALL", "1 1 1 0.1");
	}

	%obj.schedule ($Game::Item::PopTime, "delete");

	if ( isObject(%oldQuotaObject) )
	{
		setCurrentQuotaObject (%oldQuotaObject);
	}
}


function ItemImage::onMount ( %this, %obj, %slot )
{
	// Your code here
}

function ItemImage::onUnMount ( %this, %obj, %slot )
{
	// Your code here
}
