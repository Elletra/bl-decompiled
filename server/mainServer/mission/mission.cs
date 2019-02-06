exec ("./createMission.cs");
exec ("./endMission.cs");
exec ("./resetMission.cs");


function onMissionLoaded ()
{
	startGame();
}

function onMissionEnded ()
{
	endGame();
}

function onMissionReset ()
{
	// Your code here
}
