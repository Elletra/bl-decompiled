function setSprayCanColors ()
{
	%foundGameModeColorSet = 0;

	if ( $GameModeArg !$= "" )
	{
		%filename = filePath ($GameModeArg) @  "/colorset.txt";

		if ( isFile(%filename) )
		{
			%foundGameModeColorSet = 1;
		}
	}


	if ( !%foundGameModeColorSet )
	{
		%filename = "config/server/colorSet.txt";

		if ( !isFile(%filename) )
		{
			echo ("Colorset not found, creating default colorset file");

			%file = new FileObject();
			%file.openForWrite (%filename);

			%file.writeLine ("0.900 0.000 0.000 1.000");
			%file.writeLine ("0.900 0.900 0.000 1.000");
			%file.writeLine ("0.000 0.500 0.250 1.000");
			%file.writeLine ("0.200 0.000 0.800 1.000");
			%file.writeLine ("0.900 0.900 0.900 1.000");
			%file.writeLine ("0.750 0.750 0.750 1.000");
			%file.writeLine ("0.500 0.500 0.500 1.000");
			%file.writeLine ("0.200 0.200 0.200 1.000");
			%file.writeLine ("100 50 0 255");
			%file.writeLine ("DIV:Standard");

			%file.writeLine ("");

			%file.writeLine ("230 87 20 255");
			%file.writeLine ("191 46 123 255");
			%file.writeLine ("99 0 30 255");
			%file.writeLine ("34 69 69 255");
			%file.writeLine ("0 36 85 255");
			%file.writeLine ("27 117 196 255");
			%file.writeLine ("255 255 255 255");
			%file.writeLine ("20 20 20 255");
			%file.writeLine ("255 255 255 64");
			%file.writeLine ("DIV:Bold");

			%file.writeLine ("");

			%file.writeLine ("236 131 173 255");
			%file.writeLine ("255 154 108 255");
			%file.writeLine ("255 224 156 255");
			%file.writeLine ("244 224 200 255");
			%file.writeLine ("200 235 125 255");
			%file.writeLine ("138 178 141 255");
			%file.writeLine ("143 237 245 255");
			%file.writeLine ("178 169 231 255");
			%file.writeLine ("224 143 244 255");
			%file.writeLine ("DIV:Soft");

			%file.writeLine ("");

			%file.writeLine ("0.667 0.000 0.000 0.700");
			%file.writeLine ("1.000 0.500 0.000 0.700");
			%file.writeLine ("0.990 0.960 0.000 0.700");
			%file.writeLine ("0.000 0.471 0.196 0.700");
			%file.writeLine ("0.000 0.200 0.640 0.700");
			%file.writeLine ("152 41 100 178");
			%file.writeLine ("0.550 0.700 1.000 0.700");
			%file.writeLine ("0.850 0.850 0.850 0.700");
			%file.writeLine ("0.100 0.100 0.100 0.700");
			%file.writeLine ("DIV:Transparent");

			%file.close();
			%file.delete();
		}

		if ( !isFile(%filename) )
		{
			error ("ERROR: setSprayCanColors() - File " @  %filename  @ " not found and could not be created!");
			return 0;
		}
	}


	%file = new FileObject();
	%file.openForRead (%filename);

	%i = -1;
	%divCount = -1;

	while ( !%file.isEOF() )
	{
		%line = %file.readLine();

		if ( getSubStr(%line, 0, 4) $= "DIV:" )
		{
			%divName = getSubStr (%line, 4, strlen(%line) - 4);
			setSprayCanDivision (%divCount + 1, %i, %divName);
			%divCount++;
		}
		else if ( %line !$= ""  &&  %i < 63 )
		{
			%r = mAbs ( getWord(%line, 0) );
			%g = mAbs ( getWord(%line, 1) );
			%b = mAbs ( getWord(%line, 2) );
			%a = mAbs ( getWord(%line, 3) );

			if ( mFloor(%r) != %r  ||  mFloor(%g) != %g  ||  mFloor(%b) != %b  ||  
				 mFloor(%a) != %a  ||  ( %r <= 1  &&  %g <= 1  &&  %b <= 1  &&  %a <= 1 ) )
			{
				setSprayCanColor (%i + 1, %r SPC %g SPC %b SPC %a);
				%i++;
			}
			else
			{
				setSprayCanColorI (%i + 1, %r SPC %g SPC %b SPC %a);
				%i++;
			}
		}
	}

	%file.close();
	%file.delete();

	$maxSprayColors = %i;

	for ( %j = %divCount + 1;  %j < 16;  %j++ )
	{
		setSprayCanDivision (%j, 0, "");
	}

	for ( %j = %i + 1;  %j < 64;  %j++ )
	{
		setColorTable (%j, "1.0 0.0 1.0 0.0");
	}

	return 1;
}

function setSprayCanColorI ( %id, %color )
{
	%red   = getWord (%color, 0);
	%green = getWord (%color, 1);
	%blue  = getWord (%color, 2);
	%alpha = getWord (%color, 3);

	%red   = mClamp(%red, 0, 255);
	%green = mClamp(%green, 0, 255);
	%blue  = mClamp(%blue, 0, 255);
	%alpha = mClamp(%alpha, 1, 255);

	%red   = %red / 255;
	%green = %green / 255;
	%blue  = %blue / 255;
	%alpha = %alpha / 255;

	%floatColor = %red SPC %green SPC %blue SPC %alpha;
	setSprayCanColor (%id, %floatColor);
}

function setSprayCanColor ( %id, %color )
{
	%red   = getWord (%color, 0);
	%green = getWord (%color, 1);
	%blue  = getWord (%color, 2);
	%alpha = getWord (%color, 3);

	%red   = mClampF (%red, 0, 1);
	%green = mClampF (%green, 0, 1);
	%blue  = mClampF (%blue, 0, 1);
	%alpha = mClampF (%alpha, 1 / 255, 1);

	%imageAlpha = mClampF (%alpha, 10 / 255, 1);

	%color = %red SPC %green SPC %blue SPC %alpha;
	%imagecolor = %red SPC %green SPC %blue SPC %imageAlpha;

	setColorTable (%id, %color);

	%rgbColor = getWords (%color, 0, 2);
	%alphaVal = getWord (%color, 3);

	if ( %alphaVal > 0.99 )
	{
		%invalpha = 1;
	}
	else
	{
		%invalpha = 0;

		if ( %red < 8 / 255  &&  %green < 8 / 255  &&  %blue < 8 / 255 )
		{
			%rgbColor = 8 / 255 SPC 8 / 255 SPC 8 / 255;
		}
	}


	%dbName = "color" @  %id  @ "PaintExplosionParticle";
	%commandString = "datablock ParticleData(" @  %dbName  @ " : bluePaintExplosionParticle)" @ 
		"{ colors[0] = \""  @  %rgbColor  @ " 0.500\";" @ 
		"  colors[1] = \""  @  %rgbColor  @ " 0.000\";" @ 
		" useInvAlpha = " @  %invalpha  @ ";" @ 
	"};";

	eval (%commandString);


	%dbName = "color" @  %id  @ "PaintDropletParticle";
	%commandString = "datablock ParticleData(" @  %dbName  @ " : bluePaintDropletParticle)" @ 
		"{ colors[0] = \""  @  %rgbColor  @ " 0.500\";" @ 
		"  colors[1] = \""  @  %rgbColor  @ " 0.000\";" @ 
		" useInvAlpha = " @  %invalpha  @ ";" @ 
	"};";

	eval (%commandString);


	%dbName = "color" @  %id  @ "PaintExplosionEmitter";
	%particleDBName = "color" @  %id  @ "PaintExplosionParticle";

	%commandString = "datablock ParticleEmitterData(" @  %dbName  @ " : bluePaintExplosionEmitter)" @ 
		"{ particles = " @ 
		%particleDBName  @ ";" @ 
	"};";

	eval (%commandString);


	%dbName = "color" @  %id  @ "PaintDropletEmitter";
	%particleDBName = "color" @  %id  @ "PaintDropletParticle";

	%commandString = "datablock ParticleEmitterData(" @ %dbName @ " : bluePaintDropletEmitter)" @ 
		"{ particles = " @ %particleDBName @ ";" @ 
	"};";

	eval (%commandString);


	%dbName = "color" @  %id  @ "PaintExplosion";
	%emitter0Name = "color" @  %id  @ "PaintExplosionEmitter";
	%emitter1Name = "color" @  %id  @ "PaintDropletEmitter";

	%commandString = "datablock ExplosionData(" @  %dbName  @ " : bluePaintExplosion)" @ 
		"{ emitter[0] = " @  %emitter0Name  @ ";" @ 
		"  emitter[1] = " @  %emitter1Name  @ ";" @ 
	"};";

	eval (%commandString);


	%dbName = "color" @  %id  @ "PaintProjectile";
	%explosionDBName = "color" @  %id  @ "PaintExplosion";

	%commandString = "datablock ProjectileData(" @  %dbName  @ " : bluePaintProjectile)" @ 
		"{ explosion = " @  %explosionDBName  @ ";" @ 
		" colorID = " @  %id  @ ";" @ 
	"};";

	eval (%commandString);


	%dbName = "color" @ %id @ "SprayCanImage";
	%projectileDBName = "color" @ %id @ "PaintProjectile";
	%stateEmitterDBName = "color" @ %id @ "PaintEmitter";

	if ( %alphaVal > 0.99 )
	{
		%shapeFile = "base/data/shapes/spraycan.dts";
	}
	else
	{
		%shapeFile = "base/data/shapes/transspraycan.dts";
	}

	%commandString = "datablock ShapeBaseImageData(" @ %dbName @ " : blueSprayCanImage)" @ 
		"{ projectile = " @ %projectileDBName @ ";" @ 
		"  doColorShift = true;" @ 
		"  colorShiftColor = \"" @ %imagecolor @ "\";" @ 
		"  shapeFile = \"" @ %shapeFile @ "\";" @ 
	"};";

	eval (%commandString);
}
