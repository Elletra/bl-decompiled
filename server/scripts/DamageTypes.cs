function AddDamageType(%name, %deathMessageSuicide, %deathMessageMurder, %vehicleDamageScale, %direct)
{
	%dmmString = getTaggedString(%deathMessageSuicide);
	%startPos = strpos(%dmmString, "<bitmap:");
	if (%startPos != -1)
	{
		%startPos += strlen("<bitmap:");
		%endPos = strpos(%dmmString, ">", %startPos);
		%bitmapName = getSubStr(%dmmString, %startPos, %endPos - %startPos) @ ".png";
		%bitmapName = ExpandFilename(%bitmapName);
		if (!isFile(%bitmapName))
		{
			warn("WARNING: AddDamageType() - " @ %name @ " file \"" @ %bitmapName @ "\" does not exist!");
			schedule(1000, 0, MessageAll, '', "AddDamageType() - " @ %name @ " file \"" @ %bitmapName @ "\" does not exist!");
			return;
		}
	}
	%dmmString = getTaggedString(%deathMessageMurder);
	%startPos = strpos(%dmmString, "<bitmap:");
	if (%startPos != -1)
	{
		%startPos += strlen("<bitmap:");
		%endPos = strpos(%dmmString, ">", %startPos);
		%bitmapName = getSubStr(%dmmString, %startPos, %endPos - %startPos) @ ".png";
		%bitmapName = ExpandFilename(%bitmapName);
		if (!isFile(%bitmapName))
		{
			warn("WARNING: AddDamageType() - " @ %name @ ": file \"" @ %bitmapName @ "\" does not exist!");
			schedule(1000, 0, MessageAll, '', "AddDamageType() - " @ %name @ " file \"" @ %bitmapName @ "\" does not exist!");
			return;
		}
	}
	eval("%exists = $DamageType::" @ %name @ ";");
	if (%exists > 0)
	{
		warn("Warning: DamageType \"" @ %name @ "\" already exists.");
		%idx = %exists;
	}
	else
	{
		$MaxDamageType++;
		%idx = $MaxDamageType;
		eval("$DamageType::" @ %name @ " = $maxDamageType;");
	}
	$DamageType_Array[%idx] = %name;
	$DeathMessage_Suicide[%idx] = %deathMessageSuicide;
	$DeathMessage_Murder[%idx] = %deathMessageMurder;
	$DamageType::SuicideBitmap[%idx] = "";
	%str = getTaggedString(%deathMessageSuicide);
	%posA = stripos(%str, "<bitmap:");
	if (%posA > -1)
	{
		%posB = stripos(%str, ">", %posA + 1);
		if (%posB > -1)
		{
			%start = %posA + strlen("<bitmap:");
			%len = %posB - %start;
			%filename = getSubStr(%str, %start, %len);
			if (isFile(%filename @ ".png"))
			{
				%filename = %filename @ ".png";
			}
			$DamageType::SuicideBitmap[%idx] = %filename;
		}
	}
	$DamageType::MurderBitmap[%idx] = "";
	%str = getTaggedString(%deathMessageMurder);
	%posA = stripos(%str, "<bitmap:");
	if (%posA > -1)
	{
		%posB = stripos(%str, ">", %posA + 1);
		if (%posB > -1)
		{
			%start = %posA + strlen("<bitmap:");
			%len = %posB - %start;
			%filename = getSubStr(%str, %start, %len);
			if (isFile(%filename @ ".png"))
			{
				%filename = %filename @ ".png";
			}
			$DamageType::MurderBitmap[%idx] = %filename;
		}
	}
	if (%vehicleDamageScale $= "")
	{
		%vehicleDamageScale = 1;
	}
	$Damage::VehicleDamageScale[%idx] = %vehicleDamageScale;
	if (%direct $= "")
	{
		%direct = 0;
	}
	$Damage::Direct[%idx] = %direct;
}

function initDefaultDamageTypes()
{
	$MaxDamageType = 0;
	AddDamageType("Default", '<bitmap:base/client/ui/ci/skull> %1', '%2 <bitmap:base/client/ui/ci/skull> %1!', 1, 0);
	AddDamageType("Suicide", '<bitmap:base/client/ui/ci/skull> %1', '%2 <bitmap:base/client/ui/ci/skull> %1', 1, 0);
	AddDamageType("Direct", '<bitmap:base/client/ui/ci/generic> %1', '%2 <bitmap:base/client/ui/ci/generic> %1', 1, 1);
	AddDamageType("Radius", '<bitmap:base/client/ui/ci/bomb> %1', '%2 <bitmap:base/client/ui/ci/splat> %1', 1, 0);
	AddDamageType("Impact", '<bitmap:base/client/ui/ci/splat> %1', '%2 <bitmap:base/client/ui/ci/splat> %1', 1, 0);
	AddDamageType("Fall", '<bitmap:base/client/ui/ci/crater> %1', '%2 <bitmap:base/client/ui/ci/crater> %1', 1, 0);
	AddDamageType("Vehicle", '<bitmap:base/client/ui/ci/car> %1', '%2 <bitmap:base/client/ui/ci/car> %1', 1, 1);
	AddDamageType("VehicleExplosion", '<bitmap:base/client/ui/CI/carExplosion> %1', '%2 <bitmap:base/client/ui/CI/carExplosion> %1', 1, 0);
	AddDamageType("Lava", '<bitmap:base/client/ui/ci/skull> %1', '%2 <bitmap:base/client/ui/ci/skull> %1!', 1, 0);
}

function dumpDamageTypes()
{
	for (%i = 1; %i <= $MaxDamageType; %i++)
	{
		echo($DamageType_Array[%i]);
		echo("  SuicideMsg: ", getTaggedString($DeathMessage_Suicide[%i]));
		echo("   MurderMsg: ", getTaggedString($DeathMessage_Murder[%i]));
	}
}

