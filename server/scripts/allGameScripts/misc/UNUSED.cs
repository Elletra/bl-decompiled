// Functions that I'm pretty sure are unused
// I could be wrong though


function serverCmdClearColors ( %client )
{
	if ( !%client.isAdmin )
	{
		return;
	}

	if ( $LoadingBricks_Client !$= %client )
	{
		%name = $LoadingBricks_Client.getPlayerName();
		messageClient (%client, '', %name  @ " is loading bricks right now. Try again later.");
	}
}

// I guess this was replaced with serverCmdInitUploadHandshake ?? 

function serverCmdStartSaveFileUpload ( %client, %colorMethod )
{
	return;

	if ( !%client.isAdmin )
	{
		return;
	}

	if ( !isObject($LoadingBricks_Client) )
	{
		if ( $Pref::Server::AllowColorLoading )
		{
			$LoadingBricks_ColorMethod = %colorMethod;
		}
		else
		{
			$LoadingBricks_ColorMethod = 3;
		}

		$LoadingBricks_StartTime = getSimTime();
		$LoadingBricks_Client = %client;
		$LoadingBricks_Name = %client.getPlayerName();
		$LoadingBricks_FailureCount = 0;
		$LoadingBricks_BrickCount = 0;

		$LoadBrick_LineCount = 0;

		%name = $LoadingBricks_Client.getPlayerName();

		MessageAll ('', %name  @ "is uploading a save file.");
		commandToClient (%client, 'LoadBricksHandshake', 1, $Pref::Server::AllowColorLoading);


		if ( isObject($LoadBrick_FileObj) )
		{
			$LoadBrick_FileObj.delete();
		}

		$LoadBrick_FileObj = new FileObject();
		$LoadBrick_FileObj.openForWrite ("base/server/temp/temp.bls");

		$LoadingBricks_TimeoutSchedule = schedule (30 * 1000, 0, serverLoadBricks_Timeout);
		$LoadingBricks_colorCount = -1;

		for ( %i = 0;  %i < 64;  %i++ )
		{
			if ( getWord ( getColorIDTable(%i), 3 ) > 0.001 )
			{
				$LoadingBricks_colorCount++;
			}
		}

		if ( %colorMethod != 0 )
		{
			if ( %colorMethod == 1 )
			{
				$LoadingBricks_divCount = 0;

				for ( %i = 0;  %i < 16;  %i++ )
				{
					if ( getSprayCanDivisionSlot(%i) != 0 )
					{
						$LoadingBricks_divCount++;
					}
					else
					{
						break;
					}
				}
			}
			else if ( %colorMethod == 2 )
			{
				$LoadingBricks_colorCount = -1;
				$LoadingBricks_divCount = -1;
			}
		}
	}
	else
	{
		%name = $LoadingBricks_Client.getPlayerName();

		messageClient (%client, '', %name  @ "is loading bricks right now. Try again later.");
		commandToClient (%client, 'LoadBricksHandshake', 0);
	}
}

// I fixed it up anyway



// The update that never was...

function serverCmdSetHatTicket ( %client, %hatTicket )
{
	return;

	if ( strlen(%ticket) <= 0 )
	{
		%client.hatTicket = "";
		return;
	}

	if ( !checkInventoryTicket ( %hatTicket, getPublicKey() ) )
	{
		if ( getBuildString() !$= "Ship" )
		{
			echo ( "Bad hat ticket from client " @  %client  @ ", " @  %client.getPlayerName() );
		}

		%client.hatTicket = "";
		return;
	}
	else if ( getBuildString() !$= "Ship" )
	{
		echo ( "Valid hat ticket from client " @  %client  @ ", " @  %client.getPlayerName() );
	}

	%client.hatTicket = %hatTicket;
	commandToAll ('setClientHatTicket', %client, %client.hatTicket);
}

function serverCmdSetFocalPoint ( %client )
{
	%start = %client.getControlObject().getEyePoint();
	%vec   = %client.getControlObject().getEyeVector();
	%end   = VectorAdd ( %start, VectorScale(%vec, 100) );

	%mask = $TypeMasks::StaticObjectType | $TypeMasks::PlayerObjectType | $TypeMasks::FxBrickObjectType;

	%search = containerRayCast ( %start, %end, %mask, %client.getControlObject() );

	%point = getWords (%search, 1, 3);

	commandToClient (%client, 'SetFocalPoint', %point);
}

function getLongNumberString ( %val )
{
	%val = 1000003;
	echo ("  val = ",  %val % 10);


	%ret = "";
	%i = 0;

	while ( %i < 100 )
	{
		%ret = %val % 10 @ %ret;
		%val = mFloor (%val / 10);

		if ( %val > 0 )
		{
			%i++;
		}
	}

	return %ret;
}

function updateDemoBrickCount ( %val )
{
	if ( isObject(Demo_BrickCount) )
	{
		Demo_BrickCount.setText (%val);
	}
}
