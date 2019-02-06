function verifyOutputParameterList ( %class, %idx )
{
	%count = getFieldCount ( $OutputEvent_parameterList[%class, %idx] );
	%verifiedList = "";

	for ( %i = 0;  %i < %count;  %i++ )
	{
		%field = getField ($OutputEvent_parameterList[%class, %idx], %i);
		%type = getWord (%field, 0);

		if ( %type $= "int" )
		{
			%min = mFloor ( getWord(%field, 1) );
			%max = mFloor ( getWord(%field, 2) );
			%default = mFloor ( getWord(%field, 3) );

			if ( %min > %max )
			{
				%min = %max;

				error ( "WARNING: integer min > max on class:" SPC  %class  @ ", event:" SPC  
					$OutputEvent_Name[%class, %idx] );
			}

			if ( %default < %min )
			{
				%default = %min;

				error ( "WARNING: integer default < min on class:" SPC  %class  @ ", event:" SPC 
					$OutputEvent_Name[%class, %idx] );
			}

			if ( %default > %max )
			{
				%default = %max;

				error ( "WARNING: integer default > max on class:" SPC  %class  @ ", event:" SPC 
					$OutputEvent_Name[%class, %idx] );
			}

			%verifiedField = "int" SPC %min SPC %max SPC %default;
		}
		else if ( %type $= "intList" )
		{
			%width = mFloor ( getWord(%field, 1) );

			if ( %width <= 8 )
			{
				%width = 100;

				error ( "WARNING: integer list width <= 8 on class:" SPC  %class  @ ", event:" SPC 
					$OutputEvent_Name[%class, %idx] );
			}

			%verifiedField = "intList" SPC %width;
		}
		else if ( %type $= "float" )
		{
			%min = atof ( getWord(%field, 1) );
			%max = atof ( getWord(%field, 2) );
			%step = mAbs ( getWord(%field, 3) );
			%default = atof ( getWord(%field, 4) );

			if ( %min > %max )
			{
				%min = %max - 1;

				error ( "WARNING: float min > max on class:" SPC  %class  @ ", event:" SPC 
					$OutputEvent_Name[%class, %idx] );
			}

			if ( %step > %max - %min )
			{
				error ( "WARNING: float step(" @  %step  @ ") > range(" @  %max - %min  @ 
					") on class:" SPC  %class  @ ", event:" SPC  $OutputEvent_Name[%class, %idx] );

				%step = %max - %min;
			}

			if ( %default < %min )
			{
				%default = %min;

				error ( "WARNING: float default < min on class:" SPC  %class  SPC 
					", event:" SPC  $OutputEvent_Name[%class, %idx] );
			}

			if ( %default > %max )
			{
				%default = %max;

				error ( "WARNING: float default > max on class:" SPC  %class  SPC 
					", event:" SPC  $OutputEvent_Name[%class, %idx] );
			}

			%verifiedField = "float" SPC %min SPC %max SPC %step SPC %default;
		}
		else if ( %type $= "bool" )
		{
			%verifiedField = "bool";
		}
		else if ( %type $= "string" )
		{
			%maxLength = mFloor ( getWord(%field, 1) );
			%width = mFloor ( getWord(%field, 2) );

			if ( %maxLength <= 0 )
			{
				%maxLength = 1;

				error ( "WARNING: string maxLength < 1 on class:" SPC  %class  SPC 
					", event:" SPC  $OutputEvent_Name[%class, %idx] );
			}

			if ( %maxLength > 200 )
			{
				%maxLength = 200;

				error ( "WARNING: string maxLength > 200 on class:" SPC  %class  SPC 
					", event:" SPC  $OutputEvent_Name[%class, %idx] );
			}

			if ( %width <= 18 )
			{
				%width = 18;

				error ( "WARNING: string width < 18 on class:" SPC  %class  SPC 
					", event:" SPC  $OutputEvent_Name[%class, %idx] );
			}

			%verifiedField = "string" SPC %maxLength SPC %width;
		}
		else if ( %type $= "datablock" )
		{
			%dbClassName = getWord (%field, 1);
			%verifiedField = "datablock" SPC %dbClassName;
		}
		else if ( %type $= "vector" )
		{
			%verifiedField = "vector";
		}
		else if ( %type $= "list" )
		{
			%wordCount = getWordCount (%field);

			if ( (%wordCount - 1) % 2 != 0 )
			{
				error ( "WARNING: list has odd number of arguments on class:" SPC  %class  @ 
					", event:" SPC  $OutputEvent_Name[%class, %idx] );
			}

			if ( %wordCount == 1 )
			{
				error ( "WARNING: list has no arguments on class:" SPC  %class  @ 
					", event:" SPC $OutputEvent_Name[%class, %idx] );
			}

			%j = 0;
			%verifiedField = "list";

			while ( %j < %wordCount )
			{
				%j++;
				%text = getWord (%field, %j + 1);

				%j++;
				%id = getWord (%field, %j + 1);


				if ( %id != mFloor(%id) )
				{
					%id = mFloor (%id);

					error ( "WARNING: list has non-integer ID " @  %id  @ " on class:" SPC  %class  @ 
						", event:" SPC  $OutputEvent_Name[%class, %idx] );
				}

				%verifiedField = %verifiedField SPC %text SPC %id;
			}
		}
		else if ( %type $= "paintColor" )
		{
			%verifiedField = "paintColor";
		}
		else
		{
			error ( "WARNING: Unknown output parameter type " @  %type  @ " on class:" SPC  %class  @ 
				", event:" SPC $OutputEvent_Name[%class, %idx] );

			%verifiedField = %field;
		}

		if ( %i == 0 )
		{
			%verifiedList = %verifiedField;
		}
		else
		{
			%verifiedList = %verifiedList TAB %verifiedField;
		}
	}
}
