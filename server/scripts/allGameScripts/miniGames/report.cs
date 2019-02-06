function MiniGameSO::report ( %obj )
{
	echo ("");
	echo ("MiniGame: ", %obj.title);
	echo ( "  Color: ", $MiniGameColorName[%obj.colorIdx] );

	echo ("");

	echo ("  InviteOnly:", %obj.InviteOnly);
	echo ("  UseAllPlayersBricks:", %obj.UseAllPlayersBricks);
	echo ("  PlayersUseOwnBricks:", %obj.PlayersUseOwnBricks);
	echo ("  UseSpawnBricks:", %obj.UseSpawnBricks);

	echo ("");

	echo ("  Points_BreakBrick:", %obj.Points_BreakBrick);
	echo ("  Points_PlantBrick:", %obj.Points_PlantBrick);
	echo ("  Points_KillPlayer:", %obj.Points_KillPlayer);
	echo ("  Points_KillSelf:", %obj.Points_KillSelf);
	echo ("  Points_Die:", %obj.Points_Die);

	echo ("");

	echo ("  RespawnTime:", %obj.RespawnTime);
	echo ("  VehicleRespawnTime:", %obj.VehicleRespawnTime);

	echo ("");

	echo ("  JetLevel:", %obj.JetLevel);
	echo ("  FallingDamage:", %obj.FallingDamage);
	echo ("  WeaponDamage:", %obj.WeaponDamage);
	echo ("  VehicleDamage:", %obj.VehicleDamage);
	echo ("  BrickDamage:", %obj.BrickDamage);

	echo ("");

	echo ("  SelfDamage:", %obj.SelfDamage);
	echo ("  EnableWand:", %obj.EnableWand);
	echo ("  EnableBuilding:", %obj.EnableBuilding);

	echo ("");

	// FIXME: make it not hardcoded at 5
	for ( %i = 0;  %i < 5;  %i++ )
	{
		echo ("  StartEquip" @  %i  @ ": " @  %obj.startEquip[%i]  @ " | " @  %obj.startEquip[%i].uiName);
	}

	echo ("  ", %obj.numMembers, " Members:");
	echo ("  -------------------------------");

	for ( %i = 0;  %i < %obj.numMembers;  %i++ )
	{
		if ( %obj.member[%i] == %obj.owner )
		{
			echo ("    " @  %obj.member[%i]  @ ": " @  %obj.member[%i].getPlayerName(), " <--- Owner");
		}
		else
		{
			echo ( "    " @  %obj.member[%i]  @ ": " @  %obj.member[%i].getPlayerName() );
		}
	}

	echo ("  -------------------------------");
}
