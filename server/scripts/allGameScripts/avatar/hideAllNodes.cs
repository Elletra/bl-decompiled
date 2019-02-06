function hideAllNodes ( %player )
{
	%player.hideNode ( lski );
	%player.hideNode ( rski );
	%player.hideNode ( skirtTrimLeft );
	%player.hideNode ( skirtTrimRight );


	for ( %i = 0;  %i < $num["Hat"];  %i++ )
	{
		if ( $hat[%i] !$= "None" )
		{
			%player.hideNode ( $hat[%i] );
		}
	}

	for ( %i = 0;  %i < $num["Accent"];  %i++ )
	{
		if ( $Accent[%i] !$= "None" )
		{
			%player.hideNode ( $Accent[%i] );
		}
	}


	for ( %i = 0;  %i < $num["Pack"];  %i++ )
	{
		if ( $pack[%i] !$= "None" )
		{
			%player.hideNode ( $pack[%i] );
		}
	}

	for ( %i = 0;  %i < $num["SecondPack"];  %i++ )
	{
		if ( $SecondPack[%i] !$= "None" )
		{
			%player.hideNode ( $SecondPack[%i] );
		}
	}


	for ( %i = 0;  %i < $num["Chest"];  %i++ )
	{
		if ( $Chest[%i] !$= "None" )
		{
			%player.hideNode ( $Chest[%i] );
		}
	}

	for ( %i = 0;  %i < $num["Hip"];  %i++ )
	{
		if ( $Hip[%i] !$= "None" )
		{
			%player.hideNode ( $Hip[%i] );
		}
	}


	for ( %i = 0;  %i < $num["RArm"];  %i++ )
	{
		if ( $RArm[%i] !$= "None" )
		{
			%player.hideNode ( $RArm[%i] );
		}
	}

	for ( %i = 0;  %i < $num["LArm"];  %i++ )
	{
		if ( $LArm[%i] !$= "None" )
		{
			%player.hideNode ( $LArm[%i] );
		}
	}


	for ( %i = 0;  %i < $num["RHand"];  %i++ )
	{
		if ( $RHand[%i] !$= "None" )
		{
			%player.hideNode ( $RHand[%i] );
		}
	}

	for ( %i = 0;  %i < $num["LHand"];  %i++ )
	{
		if ( $LHand[%i] !$= "None" )
		{
			%player.hideNode ( $LHand[%i] );
		}
	}


	for ( %i = 0;  %i < $num["RLeg"];  %i++ )
	{
		if ( $RLeg[%i] !$= "None" )
		{
			%player.hideNode ( $RLeg[%i] );
		}
	}

	for ( %i = 0;  %i < $num["LLeg"];  %i++ )
	{
		if ( $LLeg[%i] !$= "None" )
		{
			%player.hideNode ( $LLeg[%i] );
		}
	}
}
