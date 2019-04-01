Ingame FMScout09

Ingame FMScout09 is a real-time scouting (plus editing) utility for Sports Interactive's Football Manager 2009. It allows users to search within the current 
game database to see certain information about a number of entities such as player and staff. The scout can query the database using a number 
of different arguments and can display in a view or in a profile page various kind of information about the current entity. The programming 
language used is C# .NET.

INSTRUCTIONS:
* Run Football Manager 2009 with Patch 9.1.0 or 9.2.0 patch installed (maybe 9.0.0 too).
* Run the scout or miniscout and click Load (game will be loaded when you have FM2009 up and running)
If all done properly, the fields should get enabled. You can reload as many times as you wish.
Don't try to run the scout from within the archive (zip). Make sure to extract the contents of the downloaded file first, otherwise it will trigger an error.

INSTRUCTIONS FOR MAC
I am not sure if this will work but you can try:
1. http://www.codeweavers.com/products/cxmac/
2. http://sourceforge.net/projects/darwine/

Credits:
Based on DrBernhard's Ingame Framework in SIGames forums. Thank you DrBernhard.
Framework has since then been improved by me and others.
Icons taken from www.iconshock.com and others created by me.
Color TabControl is created by Mick Doherty (www.dotnetrix.co.uk)

Thanks to all the testers!

REQUIREMENTS:
.NET Framework 3.5 SP1
Football Manager 9.1.0, 9.2.0 and 9.3.0 (maybe 9.0.0 too)

NOTE:
- To see all your own players leave sale value to zero, it will also search for players with sale value -1 (your own for example)
- The features that edit the game are absolutely at your own risk! Make sure you save your game before you attempt to edit anything!


05-04-2009
Immuner's FMScout09 v1.32

Changelist

Added Features
- Human Managers

Changes - bugfixes
- Updated framework assemblies
- Fixed windows in preferences not showing up properly due to vs.net's intelligence

14-03-2009
Immuner's FMScout09 v1.3

Changelist

Added Features
- Added Support for 9.3.0
- Added left right buttons in profile pages to quickly view profiles from your list like in FM
- Added tooltips on the quick column buttons and on miniScout
- Added shortlist column in players to indicate whether this player exists in the shortlist view
- Added staff ratings
- Added context menu when right clicking on a non empty data grid
- Added update functionality when searching to indicate the number of results so far
- Added a new theme
- Changed styling to improve the visual quality and readability
- Added filters in preferences to customize the wonderbuttons
- Added Regen No/Yes and Preferred Foot search filters
- Added EU search option and multiple nationalities display
- Added EU players color filtering
- Added Mental Traits Skills for players (in table, attributes and profile) in scout and miniScout
- Added filter for staff search for 1st team coach/youth coach/ coach at the same time

Changes - bugfixes
- When a game is not loaded, correct buttons are disabled to avoid crashing
- Reorganized layout for easier access on filters
- Removed MultiSearch
- Removed Player More Options
- Fixed editing for several player attributes
- Fixed bug in saving settings
- Fixed bugs in shortcut keys
- Fixed crash on team staff listing

13-01-2009
Immuner's FMScout09 v1.2

Changelist

Added Features
- Added support for FM 9.2.0
- Added a new theme based on FM colors
- Added Active Staff and Active Team buttons
- Added MiniScout

Changes - bugfixes
- Load button works properly (whether game is running or not)
- Load button also reloads a new game
- Removed Active Player node from tree since its the same as Player Profile
- Fixed a huge number of wrongly read information from ingame memory with the new assemblies
- Fixed regens
- Removed animation when clicking on the vertical splitter to hide/show the tree

22-12-2008
Immuner's FMScout09 v1.1 Beta

Changelist

Added Features
- Added shortlist view
- Added shortlist import from csv and slt and export to slt, csv and txt
- Added localization support (currently only english)
- Added theme visualization with save support
- Added save user preferences option
- Added treeview for easier navigation
- Added editor support for players and staff (team read information is still buggy) which can be enabled through the preferences
- Added bug report
- Added positional ratings
- Added shortcuts and toolbar with icons
- Added quick access keys (current search, clear current table, view current profile, column customization, etc.)

Changes - bugfixes
- Changed layout a lot
- Fixed display information on the tables (wages, values, etc)
- Fixed bug on last version where you couldnt search for some player information

09-12-2008
Immuner's FMScout09 v1.0 Beta

Changelist

Added Features
- Added exit button
- Added Icons
- Added colored display information on game date, currencies, etc.

Changes - bugfixes
- Search staff by nickname possible
- You can now search for players, staff with special characters in the name
- Changed layout a bit

07-12-2008
Immuner's FMScout09 v0.99 Beta

Changelist

Added Features
- Added Team search and Team Profile with finance and stadium display information
- Added WonderTeams button
- Added Team column customization
- Added Team Heal button
- Added List Team button (for clubs lists all club players, for national teams all country players)
- Added Region search

Changes - bugfixes
- Fixed bug where you could see players by searching with nickname
- Staff roles are shown properly now
- Loads of minor bugfixes

04-12-2008
Immuner's FMScout09 v0.98 Beta

Changelist

Added Features
- Added time taken for each search
- Added Goalkeeper skills for players and Chairman Skills for Staff
- Added color encoding for free players and staff (green), loan players (blue), co-contract players (red)
- Added multiple shortlist import support
- Added default width and height for all columns and rows
- Added Heal button for players
- Added Ownership checkbox list for players to search for ones loan or co-contract
- Added WonderStaff button          

Changes - bugfixes
- Set initial directory for import/export shortlist to the FM shortlist directory
- All player's teams are shown
- Hopefully no more date crashes with properly filtering all incorrectly read players out
- Loads of minor bugfixes
- Minor layout changes

03-12-2008
Immuner's FMScout09 v0.96 Beta

Changes - bugfixes
- All staff is being shown (updated framework)

03-12-2008
Immuner's FMScout09 v0.95 Beta

Changelist

Added Features
- Added column customization features (can't remove ID or Name from players and Name from staff) with default options. ID needed for the shortlist
- Added shortlist import / export
- Nickname is being displayed (if any) on the player profile page

Changes - bugfixes
- All contact dates have been fixed
- All player wages are displayed correctly
- Fixed bug where if you were sorting by a column and searching again, the first diplayed row would be the one that was displayed before
- Search by contract expiry on players is now enabled
- Disabled row auto-resize (have a tiny bit more height now) for efficiency

01-12-2008
Immuner's FMScout09 v0.9 Beta

Changelist

Added Features
- Added game date information to know where you are when you reload the game

Changes - bugfixes
- Fixed bug where players with no name where getting displayed
- Changed size in rows
- Fixed bug where selecting a row that is sorted would result in wrong entity profile
- Fixed big where own players could not be shown
- Changed layout a bit
- Fixed bug where some dates where displayed wrong

01-12-2008
Immuner's FMScout09 v0.8 Beta

Changelist

Added Features
- Added weight and height in preferences
- Added Active Player button
- Added profile pages for staff and player which are accessible by either clicking on the Active Player button or in a row
- All numbers have their assosiative symbol in the profile pages
- All attribute from to fields are synchronized. For example, the max field can never have lower value than the min.

Changes - bugfixes
- Changed place of attribute pages for easier access
- Enabled wage button in preferences

30-11-2008
Immuner's FMScout09 v0.7 Beta

Changelist

Added Features
- Search for contract expiry date
- Ability to search and display all possible player and staff attributes
- Added preferences option to select currency (pounds, euro or dollars)
- Added preferences option to select wage display (weekly, monthly or yearly)
- Added wonderkids button

Changes - bugfixes
- Fixed several bugs on the display of elements
- Changed data structures to speed up searching

27-11-2008
Immuner's FMScout09 v0.6 Beta

Changelist

Added Features
- Search for staff
- Search options for staff: Name, Nationality, Role, Age, CA, PA
- Clear search fields for staff

Changes - bugfixes
- All search fields are now disabled until loading the game
- Numeric fields now accept only numeric values
- Results View now sorts properly numbers and dates


25-11-2008
Immuner's FMScout09 v0.5 Beta

Features
- Search for players
- Search options for players: Name, Nationality, Club, Value, Sale Value, Age, CA, PA, Position
- Clear search fields for players