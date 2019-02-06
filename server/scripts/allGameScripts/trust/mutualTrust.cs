function SimGroup::addPotentialTrust ( %this, %bl_id, %level )
{
	%bl_id = mFloor (%bl_id);
	%level = mFloor (%level);

	%this.potentialTrust[%bl_id] = %level;

	%count = mFloor (%this.potentialTrustCount);

	for ( %i = 0;  %i < %count;  %i++ )
	{
		if ( %this.potentialTrustEntry[%i] == %bl_id )
		{
			return;
		}
	}

	%this.potentialTrustEntry[%count] = %bl_id;
	%this.potentialTrustCount++;
}

function setMutualBrickGroupTrust ( %bl_idA, %bl_idB, %level )
{
	%bl_idA = mFloor (%bl_idA);
	%bl_idB = mFloor (%bl_idB);

	%level = mFloor (%level);

	if ( %level < 0  ||  %level > 2 )
	{
		error ("ERROR: SetMutualBrickGroupTrust() - invalid trust level ", %level);
		return;
	}

	%brickGroupA = "BrickGroup_" @  %bl_idA;
	%brickGroupB = "BrickGroup_" @  %bl_idB;

	%brickGroupA.abandonedTime = 0;
	%brickGroupB.abandonedTime = 0;

	%brickGroupA.isPublicDomain = 0;
	%brickGroupB.isPublicDomain = 0;

	if ( !isObject(%brickGroupA)  ||  !isObject(%brickGroupB) )
	{
		return;
	}

	%brickGroupA.Trust[%bl_idB] = %level;
	%brickGroupB.Trust[%bl_idA] = %level;

	%brickGroupA.addPotentialTrust (%bl_idB, %level);
	%brickGroupB.addPotentialTrust (%bl_idA, %level);
}
