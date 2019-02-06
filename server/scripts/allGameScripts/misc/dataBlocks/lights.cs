datablock AudioProfile (lightOnSound)
{
	fileName = "~/data/sound/lightOn.wav";
	description = AudioClosest3d;
	preload = 1;
};

datablock AudioProfile (lightOffSound)
{
	fileName = "~/data/sound/lightOff.wav";
	description = AudioClosest3d;
	preload = 1;
};

datablock fxLightData (PlayerLight)
{
	uiName = "Player\'s Light";

	LightOn = 1;
	radius = 15;
	Brightness = 5;
	color = "1 1 1 1";
	FlareOn = 1;
	FlareTP = 1;
	FlareBitmap = "base/lighting/corona";
	FlareColor = "1 1 1";
	ConstantSizeOn = 1;
	ConstantSize = 1;
	NearSize = 3;
	FarSize = 0.5;
	NearDistance = 10;
	FarDistance = 30;
	FadeTime = 0.1;
	BlendMode = 0;
};

datablock fxLightData (PlayerGreenLight)
{
	uiName = "Player\'s Greenlight";

	LightOn = 1;
	radius = 15;
	Brightness = 5;
	color = "0 1 0 1";
	FlareOn = 1;
	FlareTP = 1;
	FlareBitmap = "base/lighting/corona";
	FlareColor = "1 1 1";
	ConstantSizeOn = 1;
	ConstantSize = 1;
	NearSize = 3;
	FarSize = 0.5;
	NearDistance = 10;
	FarDistance = 30;
	FadeTime = 0.1;
	BlendMode = 0;
};

function fxLight::onRemove ( %obj )
{
	// Your code here
}


function serverCmdLight ( %client )
{
	%player = %client.Player;

	if ( !isObject(%player) )
	{
		return;
	}

	if ( %player.getDamagePercent() >= 1 )
	{
		return;
	}

	if ( getSimTime() - %player.lastLightTime < 200 )
	{
		return;
	}

	%player.lastLightTime = getSimTime();


	if ( isObject(%player.light) )
	{
		%player.light.delete();
		%player.light = 0;

		ServerPlay3D ( lightOffSound, %player.getPosition() );
		%player.playAudio (0, lightOff);
	}
	else
	{
		%player.light = new fxLight()
		{
			dataBlock = PlayerLight;
		};
		MissionCleanup.add (%player.light);

		%player.light.setTransform ( %player.getTransform() );
		%player.light.attachToObject (%player);
		%player.light.Player = %player;


		ServerPlay3D ( lightOnSound, %player.getPosition() );
		%player.playAudio (0, LightOn);
	}
}

function serverCmdGreenLight ( %client, %checkValue )
{
	%player = %client.Player;

	if ( isObject(%player.light) )
	{
		%player.light.delete();
		%player.light = 0;
	}

	serverCmdLight (%client);

	if ( !isObject(%player) )
	{
		return;
	}


	if ( !$Server::LAN )
	{
		%expectedValue = getStringCRC (%client.getBLID()  @ "ly=ythot");  // y thot

		if ( %checkValue !$= %expectedValue )
		{
			messageClient (%client, '', "Click on the Steam link on the main menu first!");
			return;
		}
	}


	if ( isObject(%player.light) )
	{
		%player.light.setDatablock (PlayerGreenLight);
	}
}
