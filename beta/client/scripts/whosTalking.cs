function WhoTalk_Init()
{
	if (isObject($WhoTalkSO))
	{
		$WhoTalkSO.delete();
	}
	$WhoTalkSO = new ScriptObject()
	{
		class = WhoTalkSO;
		textObj = chatWhosTalkingText.getId();
		count = 0;
	};
}

function WhoTalk_addID(%client)
{
	if (!isObject($WhoTalkSO))
	{
		WhoTalk_Init();
	}
	$WhoTalkSO.addID(%client);
}

function WhoTalk_removeID(%client)
{
	if (!isObject($WhoTalkSO))
	{
		return;
	}
	$WhoTalkSO.removeID(%client);
}

function WhoTalkSO::addID(%this, %client)
{
	if (%this.HasID(%client))
	{
		return;
	}
	%this.id[%this.count] = %client;
	%this.count++;
	%this.Display();
}

function WhoTalkSO::removeID(%this, %client)
{
	for (%i = 0; %i < %this.count; %i++)
	{
		if (%this.id[%i] == %client)
		{
			for (%j = %i + 1; %j < %this.count; %j++)
			{
				%this.id[%j - 1] = %this.id[%j];
			}
			%this.count--;
			%this.Display();
			return;
		}
	}
}

function WhoTalkSO::HasID(%this, %client)
{
	for (%i = 0; %i < %this.count; %i++)
	{
		if (%this.id[%i] == %client)
		{
			return 1;
		}
	}
	return 0;
}

function WhoTalkSO::Display(%this)
{
	%text = %this.textObj;
	if (isObject(%text))
	{
		%buff = "";
		for (%i = 0; %i < %this.count; %i++)
		{
			%buff = %buff @ " " @ lstAdminPlayerList.getRowTextById(%this.id[%i]);
		}
		%text.setText(%buff);
	}
	else
	{
		error("ERROR: WhoTalkSO::Display() - text object not found.");
	}
}

