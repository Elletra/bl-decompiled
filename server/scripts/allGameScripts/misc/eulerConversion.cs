function eulerToMatrix ( %euler )
{
	%euler = VectorScale (%euler, $pi / 180);
	%matrix = MatrixCreateFromEuler (%euler);

	%xvec = getWord (%matrix, 3);
	%yvec = getWord (%matrix, 4);
	%zvec = getWord (%matrix, 5);

	%ang = getWord (%matrix, 6);
	%ang = %ang * 180 / $pi;

	%rotationMatrix = %xvec  @ " " @  %yvec  @ " " @  %zvec  @ " " @  %ang;
	return %rotationMatrix;
}

function eulerRadToMatrix ( %euler )
{
	%matrix = MatrixCreateFromEuler (%euler);

	%xvec = getWord (%matrix, 3);
	%yvec = getWord (%matrix, 4);
	%zvec = getWord (%matrix, 5);

	%ang = getWord (%matrix, 6);

	%rotationMatrix = %xvec  @ " " @  %yvec  @ " " @  %zvec  @ " " @  %ang;
	return %rotationMatrix;
}

function eulerToQuat ( %euler )
{
	%euler = VectorScale (%euler, $pi / 180);
	%matrix = MatrixCreateFromEuler (%euler);

	%xvec = getWord (%matrix, 3);
	%yvec = getWord (%matrix, 4);
	%zvec = getWord (%matrix, 5);

	%ang = getWord (%matrix, 6);

	%quat = %xvec SPC %yvec SPC %zvec SPC %ang;
	return %quat;
}

function eulerToQuat_degrees ( %euler )
{
	%euler = VectorScale (%euler, $pi / 180);
	%matrix = MatrixCreateFromEuler (%euler);

	%xvec = getWord (%matrix, 3);
	%yvec = getWord (%matrix, 4);
	%zvec = getWord (%matrix, 5);

	%ang = getWord (%matrix, 6);
	%ang = %ang * 180 / $pi;

	%quat = %xvec SPC %yvec SPC %zvec SPC %ang;
	return %quat;
}
