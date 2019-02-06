$Max::Quota::Schedules = 1000;
$Max::Quota::Misc = 9999;
$Max::Quota::Projectile = 200;
$Max::Quota::Item = 1000;
$Max::Quota::Environment = 5000;
$Max::Quota::Player = 250;
$Max::Quota::Vehicle = 100;
$Min::Quota::Schedules = 10;
$Min::Quota::Misc = 10;
$Min::Quota::Projectile = 5;
$Min::Quota::Item = 5;
$Min::Quota::Environment = 20;
$Min::Quota::Player = 0;
$Min::Quota::Vehicle = 0;

$Default::Quota::Schedules = 50;
$Default::Quota::Misc = 100;
$Default::Quota::Projectile = 25;
$Default::Quota::Item = 25;
$Default::Quota::Environment = 100;
$Default::Quota::Player = 10;
$Default::Quota::Vehicle = 5;

$Max::QuotaLAN::Schedules = 1000;
$Max::QuotaLAN::Misc = 9999;
$Max::QuotaLAN::Projectile = 500;
$Max::QuotaLAN::Item = 1000;
$Max::QuotaLAN::Environment = 5000;
$Max::QuotaLAN::Player = 250;
$Max::QuotaLAN::Vehicle = 100;
$Min::QuotaLAN::Schedules = 10;
$Min::QuotaLAN::Misc = 10;
$Min::QuotaLAN::Projectile = 5;
$Min::QuotaLAN::Item = 5;
$Min::QuotaLAN::Environment = 20;
$Min::QuotaLAN::Player = 0;
$Min::QuotaLAN::Vehicle = 0;

$Default::QuotaLAN::Schedules = 50;
$Default::QuotaLAN::Misc = 100;
$Default::QuotaLAN::Projectile = 25;
$Default::QuotaLAN::Item = 25;
$Default::QuotaLAN::Environment = 100;
$Default::QuotaLAN::Player = 50;
$Default::QuotaLAN::Vehicle = 20;


function verifyQuotaNumber ( %val, %min, %max, %default )
{
	if ( strlen(%val) > 6 )
	{
		return %max;
	}

	if ( %val $= ""  ||  mFloor(%val) !$= %val )
	{
		return %default;
	}

	return mClamp (%val, %min, %max);
}


function getQuotaObjectFromBrick ( %brick )
{
	%brickGroup = %brick.getGroup();
	return getQuotaObjectFromBrickGroup (%brickGroup);
}

function getQuotaObjectFromClient ( %client )
{
	%brickGroup = %client.brickGroup;
	return getQuotaObjectFromBrickGroup (%brickGroup);
}

function getQuotaObjectFromBrickGroup ( %brickGroup )
{
	%quotaObject = %brickGroup.QuotaObject;

	if ( isObject(%quotaObject) )
	{
		return %quotaObject;
	}
	else
	{
		%oldQuotaObject = getCurrentQuotaObject();

		if ( %oldQuotaObject != 0 )
		{
			clearCurrentQuotaObject();
		}

		%val = verifyQuotaNumber ($Server::QuotaLAN::Schedules, $Min::QuotaLAN::Schedules, 
			$Max::QuotaLAN::Schedules, $Default::QuotaLAN::Schedules);

		$Server::QuotaLAN::Schedules = %val;

		%val = verifyQuotaNumber ($Server::QuotaLAN::Misc, $Min::QuotaLAN::Misc, 
			$Max::QuotaLAN::Misc, $Default::QuotaLAN::Misc);

		$Server::QuotaLAN::Misc = %val;

		%val = verifyQuotaNumber ($Server::QuotaLAN::Projectile, $Min::QuotaLAN::Projectile, 
			$Max::QuotaLAN::Projectile, $Default::QuotaLAN::Projectile);

		$Server::QuotaLAN::Projectile = %val;

		%val = verifyQuotaNumber($Server::QuotaLAN::Item, $Min::QuotaLAN::Item, 
			$Max::QuotaLAN::Item, $Default::QuotaLAN::Item);

		$Server::QuotaLAN::Item = %val;

		%val = verifyQuotaNumber($Server::QuotaLAN::Environment, $Min::QuotaLAN::Environment, 
			$Max::QuotaLAN::Environment, $Default::QuotaLAN::Environment);

		$Server::QuotaLAN::Environment = %val;

		%val = verifyQuotaNumber($Server::QuotaLAN::Player, $Min::QuotaLAN::Player, 
			$Max::QuotaLAN::Player, $Default::QuotaLAN::Player);

		$Server::QuotaLAN::Player = %val;

		%val = verifyQuotaNumber($Server::QuotaLAN::Vehicle, $Min::QuotaLAN::Vehicle, 
			$Max::QuotaLAN::Vehicle, $Default::QuotaLAN::Vehicle);

		$Server::QuotaLAN::Vehicle = %val;

		%val = verifyQuotaNumber($Server::Quota::Schedules, $Min::Quota::Schedules, 
			$Max::Quota::Schedules, $Default::Quota::Schedules);

		$Server::Quota::Schedules = %val;

		%val = verifyQuotaNumber($Server::Quota::Misc, $Min::Quota::Misc, $Max::Quota::Misc, 
			$Default::Quota::Misc);

		$Server::Quota::Misc = %val;

		%val = verifyQuotaNumber($Server::Quota::Projectile, $Min::Quota::Projectile, 
			$Max::Quota::Projectile, $Default::Quota::Projectile);

		$Server::Quota::Projectile = %val;

		%val = verifyQuotaNumber($Server::Quota::Item, $Min::Quota::Item, $Max::Quota::Item, 
			$Default::Quota::Item);

		$Server::Quota::Item = %val;

		%val = verifyQuotaNumber($Server::Quota::Environment, $Min::Quota::Environment, 
			$Max::Quota::Environment, $Default::Quota::Environment);

		$Server::Quota::Environment = %val;

		%val = verifyQuotaNumber($Server::Quota::Player, $Min::Quota::Player, 
			$Max::Quota::Player, $Default::Quota::Player);

		$Server::Quota::Player = %val;

		%val = verifyQuotaNumber($Server::Quota::Vehicle, $Min::Quota::Vehicle, 
			$Max::Quota::Vehicle, $Default::Quota::Vehicle);

		$Server::Quota::Vehicle = %val;


		if ( $Server::LAN )
		{
			%quotaObject = new QuotaObject ();

			%quotaObject.setAllocs_Schedules ($Server::QuotaLAN::Schedules, 5465489);
			%quotaObject.setAllocs_Misc ($Server::QuotaLAN::Misc, 5465489);
			%quotaObject.setAllocs_Projectile ($Server::QuotaLAN::Projectile, 5465489);
			%quotaObject.setAllocs_Item ($Server::QuotaLAN::Item, 5465489);
			%quotaObject.setAllocs_Environment ($Server::QuotaLAN::Environment, 5465489);
			%quotaObject.setAllocs_Player ($Server::QuotaLAN::Player, 5465489);
			%quotaObject.setAllocs_Vehicle ($Server::QuotaLAN::Vehicle, 5465489);
		}
		else
		{
			%quotaObject = new QuotaObject ();

			%quotaObject.setAllocs_Schedules ($Server::Quota::Schedules, 5465489);
			%quotaObject.setAllocs_Misc ($Server::Quota::Misc, 5465489);
			%quotaObject.setAllocs_Projectile ($Server::Quota::Projectile, 5465489);
			%quotaObject.setAllocs_Item ($Server::Quota::Item, 5465489);
			%quotaObject.setAllocs_Environment ($Server::Quota::Environment, 5465489);
			%quotaObject.setAllocs_Player ($Server::Quota::Player, 5465489);
			%quotaObject.setAllocs_Vehicle ($Server::Quota::Vehicle, 5465489);
		}

		QuotaGroup.add (%quotaObject);
		%brickGroup.QuotaObject = %quotaObject;

		if ( %oldQuotaObject != 0 )
		{
			setCurrentQuotaObject (%oldQuotaObject);
		}

		return %quotaObject;
	}
}
