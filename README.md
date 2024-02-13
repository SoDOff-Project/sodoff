# SoD-Off - School of Dragons, Offline

On 7th June, 2023, School of Dragons announced they were "sunsetting" the game, and turning the servers off on the 30th of June.
At that time, SoD-Off was born. Currently, it is an (almost) complete implementation of the API server for SoD (and some other JS games).
It allows you to run SoD offline as well as host private online servers.

We provide also MMO server implementation for SoD (https://github.com/SoDOff-Project/sodoff-mmo).
We recommend using it even in offline mode, as some aspects of the single-player game depend on the MMO connection.

## Discord
[![Discord Banner](https://discordapp.com/api/guilds/1124405524679643318/widget.png?style=banner2)](https://discord.gg/bqHtMRbhM3)

## Licence

SoD-Off is open source, distributed under [AGPL](LICENSE) license.
This license does not cover resources obtained from the game or from responses of original api distributed with the API server, especially: 
	`missions.xml`, `items.xml`, `allranks.xml` and `store.xml` files from `src/Resources` directory and files inside `src/assets` directory.


## Getting started

You need dotnet 6.0 SDK to build api server from sources. To do this (and start server) just run:

```
dotnet run --project src/sodoff.csproj
```

### Modify client

To play game you need to modify game client to use `http://localhost:5001/.com/DWADragonsUnity/` instead `http://media.jumpstart.com/DWADragonsUnity/` for getting main XML config file (`DWADragonsMain.xml`).
You can do this editing `DOMain_Data/resources.assets` in hex-editor and replace those URLs.

### Server configuration

Most configuration of server is set in `appsettings.json`. See `"// ..."` keys in this file for options description.

#### supported clients

Each version of supported client need own file `assets/DWADragonsUnity/{PLATFORM}/{VERSION}/DWADragonsMain.xml`.
By default (can be changed in `appsettings.json`) files for version 2.5.0 and newer will be automatically encrypted (in accordance with the client's expectations).

Sample file for `{PLATFORM} = WIN`, `{VERSION} = 3.31.0` is provided.
It assumes that the server address is `localhost:5000` (for api) and `localhost:5000` (for assets). When running a public server, the addresses must be adjusted.

#### asset server

Multiple options of asset server can be customized. Most important of them is `ProviderURL` indicating the source of assets downloading in `partial` mode.
By default it's set to archive.org. Please do not abuse this server (especially do not disable `UseCache` option and do not get rid of `asset-cache` dir content).

#### listening address/port

By default server listening on all IPv4 and IPv6 addresses on ports 5000 (api) and 5001 (assets).
This can be changed in `appsettings.json`, but it can also requires changes in `DWADragonsMain.xml` and in clients (on change assets server address)

### Server side modding

Server support server side modding like adding new items, adding them to store without modification server source.
For details see [src/mods/README-MODDING.md](src/mods/README-MODDING.md).


## Status

### What works

Almost everything:

- register/login
- create profile
- list profiles
- tutorial
- roaming in the open world
- inventory
- store
- missions
- hideouts
- farms
- minigames
- MMO (using sodoff-mmo)

### What doesn't work

- play as Guest
- friends
- clans
- in-game messaging system (Terrible Mail)


### Methods

#### Fully implemented
- AcceptMission
- AddBattleItems
- AuthenticateUser
- CreatePet
- DeleteAccountNotification
- DeleteProfile
- FuseItems
- GetAchievementsByUserID
- GetAllActivePetsByuserId
- GetAuthoritativeTime
- GetChildList
- GetCommonInventory (V2)
- GetDefaultNameSuggestion
- GetDetailedChildList
- GetGameData
- GetImage
- GetImageByUserId
- GetItem
- GetKeyValuePair
- GetKeyValuePairByUserID
- GetMMOServerInfoWithZone (uses resource xml as response)
- GetPetAchievementsByUserID
- GetSelectedRaisedPet
- GetStore
- GetUnselectedPetByTypes
- GetUserActiveMissionState
- GetUserCompletedMissionState
- GetUserInfoByApiToken
- GetUserMissionState
- GetUserProfile
- GetUserProfileByUserID
- GetUserRoomItemPositions
- GetUserUpcomingMissionState
- IsValidApiToken_V2
- LoginChild
- LoginParent
- PurchaseItems (V1)
- PurchaseItems (V2)
- RedeemMysteryBoxItems
- RegisterChild
- RegisterParent
- RerollUserItem
- SetAchievementAndGetReward
- SetAchievementByEntityIDs
- SetAvatar
- SetCommonInventory
- SetDisplayName (V2)
- SetDragonXP (used by account import tools)
- SetImage
- SetKeyValuePair
- SetKeyValuePairByUserID
- SetPlayerXP (used by account import tools)
- SetRaisedPet
- SetSelectedPet
- SetUserRoomItemPositions
- UseInventory

#### Implemented enough (probably)
- GetCommonInventory (V1 -  returns the viking's inventory if it is called with a viking; otherwise returns 8 viking slots)
- GetQuestions (doesn't return all questions, probably doesn't need to)
- GetRules (doesn't return any rules, probably doesn't need to)
- GetSubscriptionInfo (always returns member, with end date 10 years from now)
- SendRawGameData
- SetNextItemState
- SetTaskState (only the TaskCanBeDone status is supported; might contain a serious problem - see the MissionService class)
- SetUserAchievementAndGetReward (works like SetAchievementAndGetReward)
- SetUserRoom
- ValidateName

#### Partially implemented
- ApplyPayout (doesn't calculate rewards properly)
- ApplyRewards
- GetGameDataByGame (friend tab displays all players - friend filter is not yet implemented because friend lists are not implemented)
- GetGameDataByGameForDateRange (friend tab displays all players)
- GetTopAchievementPointUsers (ignores type [all, buddy, hall of fame, ...] and mode [overall, monthly, weekly] properties)
- GetUserAchievements (used by Magic & Mythies)
- GetUserRoomList (room categories are not implemented, but it's enough for SoD)
- ProcessRewardedItems (gives gems, but doesn't give gold, gold is not yet implemented)
- SellItems (gives gems, but doesn't give gold, gold is not yet implemented)
- SetUserAchievementTask (returns a real reward but still use task placeholder)

#### Currently static or stubbed
- GetAchievementTaskInfo (returns a static XML)
- GetActiveChallenges (returns an empty array)
- GetAllRanks (needs to be populated with what ranks the user has)
- GetAllRewardTypeMultiplier (returns a static XML)
- GetAllRewardTypeMultiplier (returns a static XML)
- GetAnnouncementsByUser (returns no announcements, but that might be sufficient)
- GetAverageRatingForRoom (return max rating)
- GetBuddyList (returns an emtpy array)
- GetProfileTagAll (returns an empty array - used by Magic & Mythies)
- GetRankAttributeData (returns a static XML)
- GetUserActivityByUserID (returns an empty array)
- GetUserGameCurrency (return 65536 gems and 65536 coins)
- GetUserMessageQueue (returns an emtpy array)
- SaveMessage (doesn't do anything and returns false)
- SendMessage (doesn't do anything and returns false)
