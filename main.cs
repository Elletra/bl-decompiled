function initCommon()
{
	setRandomSeed();
	exec("./client/canvas.cs");
	exec("./client/audio.cs");
}

function initBaseClient()
{
	exec("./client/message.cs");
	exec("./client/mission.cs");
	exec("./client/missionDownload.cs");
	exec("./client/actionMap.cs");
}

function initBaseServer()
{
	if (1)
	{
		exec("./server/mainServer.cs");
	}
	else
	{
		exec("./server/webCom.cs");
		exec("./server/authQuery.cs");
		exec("./server/audio.cs");
		exec("./server/server.cs");
		exec("./server/message.cs");
		exec("./server/commands.cs");
		exec("./server/missionInfo.cs");
		exec("./server/missionLoad.cs");
		exec("./server/missionDownload.cs");
		exec("./server/clientConnection.cs");
		exec("./server/game.cs");
	}
}

function onDatablockLimitExceeded()
{
	$datablockExceededCount++;
}

function onDatablocksDeleted()
{
	$datablockExceededCount = 0;
}

