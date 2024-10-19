function loadClientAddOns()
{
	%dir = "Add-Ons/client/*.cs";
	%fileCount = getFileCount(%dir);
	%fileName = findFirstFile(%dir);
	for (%i = 0; %i < %fileCount; %i++)
	{
		exec(%fileName);
		%fileName = findNextFile(%dir);
	}
}

