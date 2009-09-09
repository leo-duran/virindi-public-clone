Quick and Dirty Rank 6 Automatic Assembler
by Virindi Inquisitor of Thistledown
http://www.virindi.net
All files in this package (c)2006

PREREQUISITES:
-Your desktop must be set to 1280x1024 resolution

-AC must be set to run windowed at 1024x768

-You must have AC Dual client installed with at least two clients set up and updated

-AC Dual Client must be version 7.2.2 (or you have to change the switch to lines in the macros)

-Decal must be working. The verify.dll plugin and the ACTool companion plugin must both be active, with no other plugins

-The Decal bar must be in the default position (upper left, going vertically)

-The first 9 accounts in AC Dual Client must be the accounts you want to make the rank tree with. They must all have the correct server selected. All 9 accounts must be EMPTY on the selected server and have 7 slots (ToD accounts)

-AC Dual client must be started and in the center of the screen (its default position, don't move it)

-The file c:\t.txt must contain 63 names with which to create new chars. You can use the included namegen.exe program to make some names, then paste them into a text file and save as c:\t.txt.

-The file ranktree.txt must be in c:\

-If your computer opens AC slowly, increase the login delays in these lines at the top of the macros:
logindelay=40000
chardelay=30000

logindelay is the time from when you click the login button in ACDC until you are ingame, default 40 seconds
chardelay is the time it takes a character to login from the character selection screen, default 30 seconds

PROCEDURE:

-Run genchars.mac to create characters on the server.

-You might want to make sure that at least all of the chars were created.

-Run swearrankchars.mac to swear the rank tree together.

-Party.