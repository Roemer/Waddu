Version History:

xx.xx.xxxx - 0.7.0.0 (Alpha 7)
New: Packages

17.12.2008 - 0.6.5.0 (Alpha 6 Bugfix 5)
New: Option to set an Addon to updated (Right-Click on Addon)
Chg: Refresh Addon Display when an Addon was updated
Bug: Always use preferred Mapping if manually set (even after Restarting)
Bug: Fixed Error when Addon doesn't have a preferred Mapping

12.12.2008 - 0.6.4.0 (Alpha 6 Bugfix 4)
New: Simple ChangeLog Viewer
New: Local Last Updated
New: Added Caching when Remote Checking an Addon
Chg: Improved Dependencies
Bug: Fixed String Splitting

08.11.2008 - 0.6.3.0 (Alpha 6 Bugfix 3)
New: Error Dialog Form

08.11.2008 - 0.6.2.0 (Alpha 6 Bugfix 2)
Chg: Made Dialogs Modal to Main Form
Bug: NoLib Version Crash

08.11.2008 - 0.6.1.0 (Alpha 6 Bugfix)
Chg: Better About Box
Chg: Better Update Available Form (for Waddu Updates)
Bug: Crash on Curse, WoWInterface, WoWUI Page if something went wrong
Bug: MobMap Download (German only)

07.11.2008 - 0.6.0.0 (Alpha 6)
New: Check Content before Deleting
New: Version Checking for Waddu and Mapping File
New: Implement Ignored/Preferred Mappings in Settings
New: Option to always prefer an older NoLib Version before a newer one with Libs
Chg: Option to use Custom Mapping File
Chg: If NoLib is on, use curseforge/wowace before preferred or default Mapping
Bug: Ignore ignored Addons also when just Checking

05.11.2008 - 0.5.0.0 (Alpha 5)
New: Created Admin Form for fast Mapping Creation
New: Ignoring Addons (and saving between Sessions)
New: Added Loader for the Mappings File
Chg: Save Preferred (Best) Mappings between Sessions
Bug: Fixed Archive Content Showing Bug

01.11.2008 - 0.4.0.0 (Alpha 4)
New: Implement Site Priorities
New: NoLib Support
Chg: Use 7Zip for Extracting and check for wrong Packages (like xchar)

25.10.2008 - 0.3.0.0 (Alpha 3)
New: Function to collect missing Mappings
New: Save the Log
New: Allow to choose the Site when multiple Mappings exists
New: Add DirectLink Mapping Type (<Version>|<Date>|<DetailUrl>|<DownloadUrl>)
New: Added Last Updated Date to the Mappings
Chg: Using Regex for Parsing
Bug: Fixed Crash when default WoW Folder doesn't exist

20.10.2008 - 0.2.0.0 (Alpha 2)
Bug: Propably fixed the curse login with Special Chars
Bug: Fixed old Addon deleting
Bug: Fixed move to Trash on Vista64Bit
Bug: Catched some more Exceptions

17.10.2008 - 0.1.0.0 (Alpha 1)
New: Added the possibility to use Cookies

12.10.2008 - Pre-Alpha
New: Download from most Sites Work

05.10.2008 - Begin of Development

Task List:
- Zip Mapping File
- Add Changelog Viewing
- Local Change Date
- Option to ignore Alpha/Beta Releases
- Implement Packages
- Version Check (instead of update all)
- Cleanup Saved Variables (with Backup)
- Proxy Support
- Store Window and Column Sizes
- Add Local Version if missing (own File / TOC)
- Request gzipped HTML (content-Encoding)

Bugs:
- Catch protected (unreadable) Folders when deleting / extracting
