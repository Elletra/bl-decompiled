function Projectile::onAdd ( %this )
{
	%this.originPoint = %this.initialPosition;
}


exec ("./damage.cs");
exec ("./impulse.cs");
exec ("./onCollision.cs");
exec ("./onExplode.cs");
