function GameConnection::syncClock ( %client, %time )
{
	commandToClient (%client, 'syncClock', %time);
}

function GameConnection::incScore ( %client, %delta )
{
	%client.score += %delta;
	%client.setScore (%client.score);
}

function GameConnection::setScore ( %client, %val )
{
	%client.score = %val;
	%count = ClientGroup.getCount();

	for ( %i = 0;  %i < %count;  %i++ )
	{
		%cl = ClientGroup.getObject(%i);

		if ( %cl.playerListOpen )
		{
			secureCommandToClient ("zbR4HmJcSY8hdRhr", %cl, 'ClientScoreChanged', mFloor(%client.score), %client);
		}
	}
}
