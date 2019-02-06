function generatePrintCountTable ()
{
	for ( %i = 0;  %i < 10;  %i++ )
	{
		$PrintCountIdx[%i] = -1;
	}

	%i = 0;
	%texture = strlwr ( getPrintTexture(%i) );

	while ( %texture !$= "" )
	{
		%path = filePath (%texture);
		%name = fileName (%texture);

		if ( strstr(%path, "print_letters_default") != -1  &&  %name $= "0.png" )
		{
			$PrintCountIdx[0] = %i;
		}

		if ( strstr(%path, "print_letters_default") != -1  &&  %name $= "1.png" )
		{
			$PrintCountIdx[1] = %i;
		}

		if ( strstr(%path, "print_letters_default") != -1  &&  %name $= "2.png" )
		{
			$PrintCountIdx[2] = %i;
		}

		if ( strstr(%path, "print_letters_default") != -1  &&  %name $= "3.png" )
		{
			$PrintCountIdx[3] = %i;
		}

		if ( strstr(%path, "print_letters_default") != -1  &&  %name $= "4.png" )
		{
			$PrintCountIdx[4] = %i;
		}

		if ( strstr(%path, "print_letters_default") != -1  &&  %name $= "5.png" )
		{
			$PrintCountIdx[5] = %i;
		}

		if ( strstr(%path, "print_letters_default") != -1  &&  %name $= "6.png" )
		{
			$PrintCountIdx[6] = %i;
		}

		if ( strstr(%path, "print_letters_default") != -1  &&  %name $= "7.png" )
		{
			$PrintCountIdx[7] = %i;
		}

		if ( strstr(%path, "print_letters_default") != -1  &&  %name $= "8.png" )
		{
			$PrintCountIdx[8] = %i;
		}

		if ( strstr(%path, "print_letters_default") != -1  &&  %name $= "9.png" )
		{
			$PrintCountIdx[9] = %i;
		}

		%texture = strlwr ( getPrintTexture(%i + 1) );  // FIXME: This may need to just be `%i` rather than `%i + 1`
		%i++;
	}

	if ( $PrintCountIdx[%i] == -1 )
	{
		%i = 0;
		%texture = strlwr ( getPrintTexture(%i) );

		while ( %texture !$= "" )
		{
			%path = filePath (%texture);
			%name = fileName (%texture);

			if ( strstr(%path, "print_letters_") != -1  &&  %name $= "0.png" )
			{
				$PrintCountIdx[0] = %i;
			}

			if ( strstr(%path, "print_letters_") != -1  &&  %name $= "1.png" )
			{
				$PrintCountIdx[1] = %i;
			}

			if ( strstr(%path, "print_letters_") != -1  &&  %name $= "2.png" )
			{
				$PrintCountIdx[2] = %i;
			}

			if ( strstr(%path, "print_letters_") != -1  &&  %name $= "3.png" )
			{
				$PrintCountIdx[3] = %i;
			}

			if ( strstr(%path, "print_letters_") != -1  &&  %name $= "4.png" )
			{
				$PrintCountIdx[4] = %i;
			}

			if ( strstr(%path, "print_letters_") != -1  &&  %name $= "5.png" )
			{
				$PrintCountIdx[5] = %i;
			}

			if ( strstr(%path, "print_letters_") != -1  &&  %name $= "6.png" )
			{
				$PrintCountIdx[6] = %i;
			}

			if ( strstr(%path, "print_letters_") != -1  &&  %name $= "7.png" )
			{
				$PrintCountIdx[7] = %i;
			}

			if ( strstr(%path, "print_letters_") != -1  &&  %name $= "8.png" )
			{
				$PrintCountIdx[8] = %i;
			}

			if ( strstr(%path, "print_letters_") != -1  &&  %name $= "9.png" )
			{
				$PrintCountIdx[9] = %i;
			}

			%texture = strlwr ( getPrintTexture(%i + 1) );  // FIXME: Same deal here, too
			%i++;
		}
	}

	if ( $PrintCountIdx[%i] == -1 )
	{
		for ( %i = 0;  %i < 10;  %i++ )
		{
			$PrintCountIdx[%i] = 0;
		}
	}
}
