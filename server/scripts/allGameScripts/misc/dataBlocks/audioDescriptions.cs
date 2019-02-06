datablock AudioDescription (AudioDefault3d)
{
	volume = 1;
	isLooping = false;
	is3D = true;
	ReferenceDistance = 20;
	maxDistance = 100;
	type = $SimAudioType;
};

datablock AudioDescription (AudioClose3d)
{
	volume = 1;
	isLooping = false;
	is3D = true;
	ReferenceDistance = 10;
	maxDistance = 60;
	type = $SimAudioType;
};

datablock AudioDescription (AudioClosest3d)
{
	volume = 1;
	isLooping = false;
	is3D = true;
	ReferenceDistance = 5;
	maxDistance = 30;
	type = $SimAudioType;
};

datablock AudioDescription (AudioDefaultLooping3d)
{
	volume = 1;
	isLooping = true;
	is3D = true;
	ReferenceDistance = 20;
	maxDistance = 100;
	type = $SimAudioType;
};

datablock AudioDescription (AudioCloseLooping3d)
{
	volume = 1;
	isLooping = true;
	is3D = true;
	ReferenceDistance = 10;
	maxDistance = 50;
	type = $SimAudioType;
};

datablock AudioDescription (AudioClosestLooping3d)
{
	volume = 1;
	isLooping = true;
	is3D = true;
	ReferenceDistance = 5;
	maxDistance = 30;
	type = $SimAudioType;
};

datablock AudioDescription (Audio2D)
{
	volume = 1;
	isLooping = false;
	is3D = false;
	type = $SimAudioType;
};

datablock AudioDescription (AudioLooping2D)
{
	volume = 1;
	isLooping = true;
	is3D = false;
	type = $SimAudioType;
};
