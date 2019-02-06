function Trigger::onAdd ( %trigger )
{
	%data = %trigger.getDataBlock();
	%data.onAdd (%trigger);
}

function TriggerData::onAdd ( %data, %trigger )
{
	// Your code here
}

function TriggerData::onEnterTrigger ( %this, %trigger, %obj )
{
	// Your code here
}

function TriggerData::onLeaveTrigger ( %this, %trigger, %obj )
{
	// Your code here
}

function TriggerData::onTickTrigger ( %this, %trigger, %obj )
{
	// Your code here
}
