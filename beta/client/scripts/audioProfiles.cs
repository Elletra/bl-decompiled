$GuiAudioType = 1;
$SimAudioType = 2;
$MessageAudioType = 3;
new AudioDescription(AudioGui)
{
	volume = 1;
	isLooping = 0;
	is3D = 0;
	type = $GuiAudioType;
};
new AudioDescription(AudioMessage)
{
	volume = 1;
	isLooping = 0;
	is3D = 0;
	type = $MessageAudioType;
};
new AudioProfile(AudioButtonOver)
{
	fileName = "~/data/sound/buttonOver.wav";
	description = "AudioGui";
	preload = 1;
};
new AudioProfile(AudioError)
{
	fileName = "~/data/sound/error.wav";
	description = "AudioGui";
	preload = 1;
};
new AudioProfile(ItemPickup)
{
	fileName = "~/data/sound/error.wav";
	description = "AudioGui";
	preload = 1;
};
