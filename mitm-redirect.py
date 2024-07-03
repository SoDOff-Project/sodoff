from mitmproxy import ctx
import mitmproxy.http

methods = [
  'GetRules',
  'LoginParent',
  'AuthenticateUser',
  'RegisterParent',
  'GetSubscriptionInfo',
  'GetUserInfoByApiToken',
  'IsValidApiToken_V2',
  'ValidateName',
  'GetDefaultNameSuggestion',
  'RegisterChild',
  'GetProfileByUserId',
  'LoginChild',
  'GetUserProfileByUserID',
  'GetKeyValuePair',
  'SetKeyValuePair',
  'GetKeyValuePairByUserID',
  'SetKeyValuePairByUserID',
  'GetUserProfile',
  'GetQuestions',
  'GetCommonInventory',
  'SetCommonInventory',
  'GetItem',
  'GetAuthoritativeTime',
  'SetAvatar',
  'GetPetAchievementsByUserID',
  'GetDetailedChildList',
  'GetStore',
  'GetAllRanks',
  'GetUserUpcomingMissionState',
  'GetUserActiveMissionState',
  'GetUserCompletedMissionState',
  'SetTaskState',
  'CreatePet',
  'SetRaisedPet',
  'SetSelectedPet',
  'GetAllActivePetsByuserId',
  'GetSelectedRaisedPet',
  'SetImage',
  'GetImage',
  'GetImageByUserId',
  'GetAchievementTaskInfo',
  'GetAllRewardTypeMultiplier',
  'GetBuddyList',
  'GetRankAttributeData',
  'GetUserMessageQueue',
  'SendMessage',
  'SaveMessage',
  'GetMMOServerInfoWithZone',
  'GetActiveChallenges',
  'GetAchievementsByUserID',
  'PurchaseItems',
  'AcceptMission',
  'GetUserMissionState',
  'SetAchievementAndGetReward',
  'SetUserAchievementTask',
  'GetAnnouncementsByUser',
  'GetUserRoomItemPositions',
  'SetUserRoomItemPositions',
  'GetAverageRatingForRoom',
  'GetUserRoomList',
  'GetUserActivityByUserID',
  'SetNextItemState',
  'SetUserRoom',
  'GetChildList',
  'GetUnselectedPetByTypes',
  'GetUserGameCurrency',
  'SetAchievementByEntityIDs',
  'UseInventory',
  'SellItems',
  'FuseItems',
  'RerollUserItem',
  'AddBattleItems',
  'ApplyRewards',
  'DeleteProfile',
  'DeleteAccountNotification',
  'SetUserAchievementAndGetReward',
  'ProcessRewardedItems',
  'ApplyPayout',
  'RedeemMysteryBoxItems',
  'SendRawGameData',
  'GetGameData',
  'GetGameDataByGame',
  'GetGameDataByUser',
  'GetGameDataByGameForDateRange',
  'GetTopAchievementPointUsers',
  'SetDisplayName',
]

def routable(path):
  for method in methods:
    if method in path:
      return True
  return False


class LocalRedirect:
  def __init__(self):
    print('Loaded redirect addon')

  def request(self, flow: mitmproxy.http.HTTPFlow):
    if ('api.sodoff.spirtix.com' in flow.request.pretty_host or 'api.jumpstart.com' in flow.request.pretty_host) and routable(flow.request.path):
      flow.request.host = "localhost"
      flow.request.scheme = 'http'
      flow.request.port = 5000

class RedirectMediaRequests:
  def __init__(self):
    print('Loaded media request redirector')

  def request(self, flow: mitmproxy.http.HTTPFlow):
    if "media.jumpstart.com" in flow.request.pretty_host:
      flow.request.host = "media.sodoff.spirtix.com"

addons = [
  LocalRedirect(),
  RedirectMediaRequests()
]
