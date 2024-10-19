for (%file = findFirstFile("base/data/terrains/*propertymap.cs"); %file !$= ""; %file = findNextFile("base/data/terrains/*propertymap.cs"))
{
	exec(%file);
}
