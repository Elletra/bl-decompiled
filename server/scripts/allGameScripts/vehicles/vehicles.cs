datablock AudioProfile (fastImpactSound)
{
	fileName = "base/data/sound/fastimpact.WAV";
	description = AudioDefault3d;
	preload = 1;
};

datablock AudioProfile (slowImpactSound)
{
	fileName = "base/data/sound/slowimpact.wav";
	description = AudioDefault3d;
	preload = 1;
};


exec ("./wheeledVehicle/wheeledVehicle.cs");
exec ("./flyingVehicle.cs");

exec ("./liquid.cs");
exec ("./mountDismount.cs");

exec ("./nextSeat.cs");
exec ("./prevSeat.cs");

exec ("./onActivate.cs");

exec ("./vehicleBubble.cs");
exec ("./vehicleBurn.cs");
exec ("./vehicleExplosion.cs");
exec ("./finalExplosion.cs");
exec ("./vehicleFoam.cs");
exec ("./vehicleSplash.cs");
exec ("./vehicleTire.cs");

exec ("./miscellaneous.cs");
