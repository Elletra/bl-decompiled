function changeMapGui::onWake(%this)
{
	if (changeMapList.rowCount() <= 0)
	{
		changeMapList.clear();
		commandToServer('getMapList');
	}
}

function changeMapGui::onSleep(%this)
{
}

function changeMapList::select(%this)
{
	%selectedID = %this.getSelectedId();
	%row = %this.getRowTextById(%selectedID);
	%name = getField(%row, 0);
	%picture = getField(%row, 1);
	%fileName = getField(%row, 2);
	%description = %this.description[%selectedID];
	changeMapPreview.setBitmap(%picture);
	changeMapName.setValue(%name);
	if (strlen(%description) > 1)
	{
		changeMapDescription.setValue(%description);
	}
	else
	{
		%description = getMissionDescription(%fileName);
		if (strlen(%description) > 1)
		{
			changeMapDescription.setText(%description);
			%this.description[%selectedID] = %description;
		}
	}
}

function changeMapButton::click(%this)
{
	%row = changeMapList.getRowTextById(changeMapList.getSelectedId());
	%fileName = getField(%row, 2);
	if (strlen(%fileName) > 1)
	{
		commandToServer('changeMap', %fileName);
	}
}

function clientCmdUpdateMapList(%entry)
{
	echo("client recieved map entry ", %entry);
	changeMapList.addRow(changeMapList.rowCount(), %entry);
	changeMapList.sort(0);
	changeMapList.setSelectedRow(0);
}

function clientCmdClearMapList()
{
	changeMapList.clear();
	for (%i = 0; changeMapList.description[%i] !$= ""; %i++)
	{
		changeMapList.description[%i] = "";
	}
}

