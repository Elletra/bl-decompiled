function fxDTSBrick::setLight ( %obj, %data, %client )
{
	if ( isObject(%data) )
	{
		if ( isObject(%obj.light) )
		{
			if ( %obj.light.getDataBlock().getId() == %data.getId() )
			{
				return;
			}
			else
			{
				%obj.light.delete();
				%obj.light = 0;
			}
		}
	}
	else
	{
		if ( isObject(%obj.light) )
		{
			%obj.light.delete();
			%obj.light = 0;
		}

		return;
	}

	if ( %obj.isDead() )
	{
		return;
	}

	if ( %data.getClassName() !$= "fxLightData" )
	{
		return;
	}

	if ( %data.uiName $= "" )
	{
		return;
	}


	%light = new fxLight()
	{
		dataBlock = %data;
	};

	if ( !%light )
	{
		if ( $Server::LAN )
		{
			commandToClient (%client, 'centerPrint', "\c6You can\'t have more than " @  
				$Server::QuotaLAN::Environment  @ " lights/emitters!", 3);
		}
		else
		{
			commandToClient (%client, 'centerPrint', "\c6You can\'t have more than " @  
				$Server::Quota::Environment  @ " lights/emitters!", 3);
		}

		return;
	}

	MissionCleanup.add (%light);

	%light.brick = %obj;
	%obj.light = %light;

	%light.setTransform ( %obj.getTransform() );
	%light.attachToBrick (%obj);
	%light.setEnable (1);
}


// =================
//  Register Events
// =================

registerOutputEvent ("fxDTSBrick", "setLight", "dataBlock FxLightData");
