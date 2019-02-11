# Blockland's Default Scripts Decompiled

This is a _very_ WIP project I've been working on in secret for the past 9 months or so (off and on).  I've got most of them cleaned up pretty well, but **_@hatf0's PyDSO is far from perfect so not all of these functions will work properly_**.  _Please_ keep this in mind when using these.  Some functions just plain don't work at all because the decompiler fucked up so badly (e.g. `fxDTSBrick::radiusImpulse` and `ProjectileData::onExplode`).


## Reporting Bugs/Errors

If you find an issue with one of the scripts, _please_ make a new issue.  If you know how to fix it, make a pull request and I'll check it out.


## Common Errors

* `while` and `if` statements get mixed up a lot, creating infinite loops.
* `\c#` are usually off by 1 so `\c1` becomes `\c2`, etc. until `\c7` where it becomes `\cb`, `\c8` becomes `\cc`, and `\c9` becomes `\ce`; also `\c0` becomes `\c2\c1`.  It's weird.
* `if` statements get mixed up sometimes so that the logic gets flipped, creating weird empty code blocks like this:
```php
if (!isObject(%player))
{
}
else
{
	if (%cl.miniGame != %mg)
	{
	}
	else
	{
		%obj.setOrbitMode(%player, %obj.getTransform(), 0, 8, 8);
		break;
	}
}
```


## Parting Thoughts

A lot of these functions (specifically the auth functions) will require [a DLL](https://github.com/Electrk/PackageAnyFunction)/modified exe for them to be defined in non-DSO files.

Everything is cleaned up for the most part.  The only thing I haven't done is `allClientScripts` which I have no intention of doing any time soon.  `allGameScripts` took me almost two months and I was grinding on it for hours and hours every day—I don't have that kind of time anymore.

I'm hesitant to accept help on it because I have my own way of formatting/styling/fixing up scripts that I can't be bothered to explain right now.  I also don't really know if I can really be bothered to check out other people's pull requests and see if they're up to snuff.  I mostly just published this for people to reference.

I hope this will be useful to people in some way.  I think it can be extraordinarily helpful for people who want to slightly edit the default functionality, or at least see how certain functions work (provided that they've been decompiled/fixed properly, that is).

<sup>(hi badspot if ur reading this dont revoke me pls its 4 modders 2 ref thx)</sup>
