// FIXME: This method is broken and doesn't work
// Decompiler broke @hatf0 pls fix

// Seriously, like, it doesn't work at all.  Don't use this for anything.


function fxDTSBrick::radiusImpulse ( %obj, %radius, %force, %verticalForce, %client )
{
	warn ("fxDTSBrick::radiusImpulse is broken and doesn't work!");  // FIXME: ADDED DEBUG LINE
	return;                                                          // FIXME: ADDED DEBUG LINE


	%pos = %obj.getPosition();
	%mask = $TypeMasks::PlayerObjectType | $TypeMasks::VehicleObjectType | 
	        $TypeMasks::CorpseObjectType | $TypeMasks::ItemObjectType;

	initContainerRadiusSearch (%pos, %radius, %mask);

	%mg = getMiniGameFromObject (%client);

	while ( ( %searchObj = containerSearchNext() ) != 0 )
	{
		%searchObj = getWord (%searchObj, 0);

		if ( isObject(%mg) )
		{
			if ( miniGameCanDamage(%client, %searchObj) != 0  &&  !$Server::LAN )
			{
				if ( %searchObj.client == %client  &&  isObject(%searchObj.spawnBrick) )
				{
					if ( %searchObj.spawnBrick.getGroup.bl_id != %client.getBLID() )
					{
						// ah yes
					}
					else
					{
						// right-o
					}
				}

				if ( %searchObj.getType() & $TypeMasks::PlayerObjectType )
				{
					%searchPos = %searchObj.getHackPosition();
				}
				else
				{
					%searchPos = %searchObj.getWorldBoxCenter();
				}


				%dist = VectorDist (%searchPos, %pos);
				%distanceFactor = 1 - %dist / %radius * %dist / %radius;

				if ( %distanceFactor <= 0 )
				{
					break;
				}
				else if ( %distanceFactor > 1 )
				{
					%distanceFactor = 1;
				}


				%impulseVec = VectorSub (%searchPos, %pos);
				%impulseVec = VectorNormalize (%impulseVec);
				%impulseVec = VectorScale (%impulseVec, %force * %distanceFactor);

				%searchObj.applyImpulse (%searchPos, %impulseVec);
				%impulseVec = VectorScale ("0 0 1", %verticalForce * %distanceFactor);
				%searchObj.applyImpulse (%pos, %impulseVec);

				if ( isObject(%client) )
				{
					%searchObj.lastPusher = %client;
					%searchObj.lastPushTime = getSimTime();
				}
			}
		}
	}
}


// =================
//  Register Events
// =================

registerOutputEvent ("fxDTSBrick", "radiusImpulse", "int 1 100 5" TAB "int -50000 50000 50" TAB 
	"int -50000 50000 10");
