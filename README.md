# HollowKnight QoL.Lite

This is a Hollow Knight quality of life mod specifically created to support runners intending to have a hitless run verified with Team Hitless.

The original source was forked from [Yusuf Bham's speedrunning-focused QoL mod](https://github.com/fifty-six/HollowKnight.QoL) and then adjusted by [greyfirey](https://github.com/greyfirey/QoLthLegal/) and then kyleanderror13 to meet Team Hitless rule requirements.

## Features

- Sell all to Lemm and Jinn and immediate nail upgrades from Nailsmith by speaking from the right side.
- Reduced dialogue box delay.
- Reduced menu fade in/out speed.
- Reduced time to transition between scenes.
- Removed charm equipping explanation on first time charm pick up.
- Ability to reduce the volume of double-damage hits.
- Instant stag appearance on stag station bell ring.
- Enables additional cutscenes to be skipped.
- Reduced time of specific cutscenes (abyss shriek collection, first time Godhome visit, Black Egg opening, post-King's Brand escape, Hall of Gods statues).
- Path of Pain can be done multiple times on one save.

## Changes from original QoL mod

- Menu speed improvements were changed from the previous implementation to use the in-code setting MENU_FADE_SPEED. This results in slower menu speeds compared to the original implementation, but avoids an issue where the menu UI could remain on screen until a quit out.
- The ability to skip dreamer sequences (Lurien, Monomon, Herrah) was removed as avoiding the platforming changes the gameplay experience of a hitless run. The option to "Allow Skipping Nonskippable" cutscenes has also been removed as this caused the game to soft-lock at the end of the dreamer sequence when it was not skipped in its entirety.
- Removed lever skips, fireball fixes, dashmaster fixes, patched bosses, Uumuu RNG manipulation, various speed skips, Salubra's Blessing removal, and hard fall removal.
- Removed dev features for stacktraces and Unity logging.
- Removed First Time Bosses, Absolute Radiance, Pure Vessel Roar and Soul Master Phase Transition Skip options.
- Fixed issue where selling rancid eggs to Jinn would always give 450 geo, rather than the expected 290-449 range.
- Removed ability to lock mouse cursor to game window.
- Removed .json copy of save files.