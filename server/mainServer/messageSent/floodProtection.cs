$SPAM_PROTECTION_PERIOD = 6000;
$SPAM_MESSAGE_THRESHOLD = 5;
$SPAM_PENALTY_PERIOD    = 5000;
$SPAM_MESSAGE           = '\c3FLOOD PROTECTION:\c0 You must wait another %1 seconds.';


function GameConnection::spamMessageTimeout ( %this )
{
	if ( %this.spamMessageCount > 0 )
	{
		%this.spamMessageCount--;
	}
}

function GameConnection::spamReset ( %this )
{
	%this.isSpamming = 0;
	%this.spamMessageCount = 0;
}

function spamAlert ( %client )
{
	if ( !%client.isSpamming  &&  %client.spamMessageCount >= $SPAM_MESSAGE_THRESHOLD )
	{
		%client.spamProtectStart = getSimTime();
		%client.isSpamming = 1;

		%client.schedule ( $SPAM_PENALTY_PERIOD * getTimeScale(),  spamReset );
	}

	if ( %client.isSpamming )
	{
		%wait = mCeil( ( $SPAM_PENALTY_PERIOD * getTimeScale() - getSimTime() - %client.spamProtectStart )  /  1000 );
		messageClient (%client, "", $SPAM_MESSAGE, %wait);

		return 1;
	}

	%client.spamMessageCount++;
	%client.schedule ( $SPAM_PROTECTION_PERIOD * getTimeScale(),  spamMessageTimeout );

	return 0;
}
