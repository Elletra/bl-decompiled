package snowBallPackage
{
	function Player::activateStuff ( %obj )
	{
		Parent::activateStuff (%obj);

		if ( strstr(strlwr(Rain.dropTexture), "snow") == -1 )
		{
			return;
		}

		if ( %obj.activateLevel >= 2 )
		{
			%type = $TypeMasks::FxBrickObjectType | $TypeMasks::TerrainObjectType;

			%pos = %obj.getEyePoint();
			%vec = VectorScale (%obj.getEyeVector(), 5);
			%end = VectorAdd (%pos, %vec);

			%brick = getWord (containerRayCast(%pos, %end, %type, %obj), 0);

			if ( !isObject(%brick)  ||  %brick.numEvents != 0 )
			{
				return;
			}

			if ( %brick.getType() & $TypeMasks::FxBrickObjectType )
			{
				%color = %brick.getColorID();
				%color = getColorIDTable (%color);
			}
			else
			{
				%color = getWord (%brick.color, 0) / 255 SPC getWord (%brick.color, 1) / 255 SPC 
					getWord(%brick.color, 2) / 255;
				
				if ( %brick.topTexture !$= "Add-Ons/Ground_Plate/plate.png"  &&  
					 %brick.topTexture !$= "Add-Ons/Ground_White/white.png")
				{
					return;
				}
			}

			if ( VectorDot(%color, "1 1 1") > 2.5  &&  !isObject ( %obj.getMountedImage(0) ) )
			{
				%obj.mountImage (blankaBallImage, 0);
			}
		}
	}
};
activatePackage (snowBallPackage);
