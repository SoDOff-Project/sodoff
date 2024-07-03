# SoDOff - School of Dragons, Offline

On 7th June 2023, School of Dragons announced they were "sunsetting" the game, and turning the servers off on the 30th of June.
At that time, SoDOff was born. Currently, it is an almost complete implementation of the School of Dragons API server (and some other JS games).
It allows you to run SoD offline as well as host private online servers.

We also provide MMO server implementation for SoD (https://github.com/SoDOff-Project/sodoff-mmo).
It is recommended to use the MMO server even when offline, as some aspects of the single player game depend on MMO.

## License

SoDOff is open source, distributed under the [AGPL](LICENSE) license.
This license does not cover resources obtained from the game or from responses of the original API distributed with the API server, especially:
	`missions.xml`, `items.xml`, `allranks.xml` and `store.xml` files from the `src/Resources` directory and files inside the `src/assets` directory.

## Getting started

To build the API server from sources, you'll need the dotnet 6.0 SDK. Simply run the following command to build and start the server:

```
dotnet run --project src/sodoff.csproj
```

### Modifying the Client

To play the game you need to modify the game client to use `http://localhost:5001/.com/conf/` instead of `http://media.jumpstart.com/DWADragonsUnity/`.
You can use *ClientPatcher* tool form https://github.com/SoDOff-Project/sodoff-tools to do this. This tool also change correct ApiKey and 3DesKey values.
You can also do this by manually editing `DOMain_Data/resources.assets` in a hex-editor and swapping the URLs (and keys if needed).

By default we use `conf` instead of `DWADragonsUnity` for XML configuration to allow the use of a longer server name/IP address (limitation of url length in resources.assets).
Provided sample `DWADragonsMain.xml` file still requires the use of the `DWADragonsUnity` directory for assets.

### Server Configuration

Most of the server configuration is stored in `appsettings.json`.  Check out the "// ..." keys in there for descriptions of different options.

#### Supported Clients

The server support multiple versions of School of Dragons and some other JS online games (like Magic & Mythies, Math Blaster and World of JumpStart).

For each supported SoD client version, there must be a corresponding file located at `assets/conf/{PLATFORM}/{VERSION}/DWADragonsMain.xml`.
By default (modifiable in appsettings.json), files for version 2.5.0 and newer will be automatically encrypted to meet the client's requirements.

A sample file is provided for `{PLATFORM} = WIN`, `{VERSION} = 3.31.0`.
It assumes that the server address are `localhost:5000` (API) and `localhost:5001` (assets).

#### Asset Server

Various settings for the asset server are customizable, with the key one being `ProviderURL`, which specifies the source for downloading assets in `partial` mode. By default, it's configured to use archive.org
Please do not abuse the archive.org server, do not disable the `UseCache` option and do not delete the contents of the `asset-cache` directory.
The [SoDOff-tools](https://github.com/SoDOff-Project/sodoff-tools) repository contains an *AssetsDownloader* tool to pre-download assets (this may be needed in case of a slow connection to `ProviderURL`).

#### Listening address/port

By default, the server listens on all IPv4 and IPv6 addresses on ports 5000 (API) and 5001 (assets).
You can tweak this in `appsettings.json`, but it might also mean adjusting `DWADragonsMain.xml` and updating clients to reflect the changes in the asset server address.

### Server Side Modding

The server supports server side modifications, which includes adding new items and putting them in the store without having to modify the server source code.
For more information, check out [src/mods/README-MODDING.md](src/mods/README-MODDING.md).

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
- GetGameDataByUser
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
