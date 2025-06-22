using sodoff.Schema;
using sodoff.Model;
using sodoff.Util;
using sodoff.Configuration;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using System.Collections.ObjectModel;
using Microsoft.Extensions.Options;

namespace sodoff.Services {
    public class AchievementStoreSingleton {
        class AchievementTasks {
            public Dictionary<int, List<AchievementTaskInfo>> achievementsRewardByTask = new();
            public readonly ReadOnlyDictionary<int, int> achievementsGroupIdToTaskId;
            
            public AchievementTasks(string xmlFile) {
                AchievementTaskInfo[] allAchievementTaskInfo = XmlUtil.DeserializeXml<AchievementTaskInfo[]>(XmlUtil.ReadResourceXmlString(xmlFile));
                Dictionary<int, int> achievementsGroupIdToTaskIdTmp = new();
                foreach (var achievementTaskInfo in allAchievementTaskInfo) {
                    if (!achievementsRewardByTask.ContainsKey(achievementTaskInfo.TaskID)) {
                        achievementsRewardByTask[achievementTaskInfo.TaskID] = new();
                    }
                    achievementsRewardByTask[achievementTaskInfo.TaskID].Add(achievementTaskInfo);
                    achievementsGroupIdToTaskIdTmp[achievementTaskInfo.TaskGroupID] = achievementTaskInfo.TaskID;
                }
                achievementsGroupIdToTaskId = new(achievementsGroupIdToTaskIdTmp);
            }
        }

        Dictionary<AchievementPointTypes, UserRank[]> ranks = new();
        Dictionary<int, AchievementReward[]> achievementsRewardByID = new();
        Dictionary<int, List<AchievementTaskInfo>> achievementsRewardByTask = new();
        Dictionary<uint, AchievementTasks> achievementsTasks = new();

        int dragonAdultMinXP;
        int dragonTitanMinXP;

        public AchievementStoreSingleton(IOptions<ApiServerConfig> config) {
            ArrayOfUserRank allranks = XmlUtil.DeserializeXml<ArrayOfUserRank>(XmlUtil.ReadResourceXmlString("ranks.allranks_sod"));
            foreach (var pointType in Enum.GetValues<AchievementPointTypes>()) {
                ranks[pointType] = allranks.UserRank.Where(r => r.PointTypeID == pointType).ToArray();
            }

            AchievementsIdInfo[] allAchievementsIdInfo = XmlUtil.DeserializeXml<AchievementsIdInfo[]>(XmlUtil.ReadResourceXmlString("achievements.achievementid_sod"));
            foreach (var achievementsIdInfo in allAchievementsIdInfo) {
                achievementsRewardByID[achievementsIdInfo.AchievementID] = achievementsIdInfo.AchievementReward;
            }

            achievementsTasks[ClientVersion.Min_SoD] = new AchievementTasks("achievements.achievementtaskinfo_sod");
            if (config.Value.LoadNonSoDData) {
                achievementsTasks[ClientVersion.MaM] = new AchievementTasks("achievements.achievementtaskinfo_mam");
                achievementsTasks[ClientVersion.MB] = new AchievementTasks("achievements.achievementtaskinfo_mb");
                achievementsTasks[ClientVersion.EMD] = new AchievementTasks("achievements.achievementtaskinfo_emd");
                achievementsTasks[ClientVersion.SS] = new AchievementTasks("achievements.achievementtaskinfo_ss");
                achievementsTasks[ClientVersion.WoJS] = new AchievementTasks("achievements.achievementtaskinfo_wojs");
            }

            dragonAdultMinXP = ranks[AchievementPointTypes.DragonXP][10].Value;
            dragonTitanMinXP = ranks[AchievementPointTypes.DragonXP][20].Value;
        }
        
        public int GetRankFromXP(int? xpPoints, AchievementPointTypes type) {
            return ranks[type].Count(r => r.Value <= xpPoints);
        }

        public AchievementReward[]? GetAchievementRewardsById(int achievementID) {
            if (achievementsRewardByID.ContainsKey(achievementID)) {
                return achievementsRewardByID[achievementID];
            } else {
                return null;
            }
        }

        public ReadOnlyDictionary<int, int> GetAchievementsGroupIdToTaskId(uint gameVersion) {
            gameVersion = ClientVersion.GetGameID(gameVersion);
            if (achievementsTasks.ContainsKey(gameVersion))
                return achievementsTasks[gameVersion].achievementsGroupIdToTaskId;
            return new ReadOnlyDictionary<int, int>(new Dictionary<int, int>());
        }

        public List<AchievementTaskInfo>? GetAllAchievementTaskInfo(int taskID, uint gameVersion) {
            gameVersion = ClientVersion.GetGameID(gameVersion);
            if (achievementsTasks.ContainsKey(gameVersion) && achievementsTasks[gameVersion].achievementsRewardByTask.ContainsKey(taskID)) {
                return achievementsTasks[gameVersion].achievementsRewardByTask[taskID];
            }
            return null;
        }

        public AchievementTaskInfo? GetAchievementTaskInfo(int taskID, uint gameVersion, int points, int groupID = -1) {
            var achievementTasks = GetAchievementTaskInfoForNextLevel(taskID, gameVersion, points, groupID);
            if (achievementTasks != null)
                return achievementTasks;
            return GetAchievementTaskInfoForCurrentLevel(taskID, gameVersion, points, groupID);
        }

        public AchievementTaskInfo? GetAchievementTaskInfoForNextLevel(int taskID, uint gameVersion, int points, int groupID = -1) {
            var achievementTasks = GetAllAchievementTaskInfo(taskID, gameVersion)?.Where(x => x.PointValue > points && (groupID < 0 || x.TaskGroupID == groupID));
            if (achievementTasks != null && achievementTasks.Count() > 0)
                return achievementTasks.FirstOrDefault(x => x.PointValue == achievementTasks.Min(x => x.PointValue));
            return null;
        }

        public AchievementTaskInfo? GetAchievementTaskInfoForCurrentLevel(int taskID, uint gameVersion, int points, int groupID = -1) {
            var achievementTasks = GetAllAchievementTaskInfo(taskID, gameVersion)?.Where(x => x.PointValue <= points && (groupID < 0 || x.TaskGroupID == groupID));
            if (achievementTasks != null && achievementTasks.Count() > 0)
                return achievementTasks?.FirstOrDefault(x => x.PointValue == achievementTasks.Max(x => x.PointValue));
            return null;
        }

        public int GetUpdatedDragonXP(int dragonXP, int growthState) {
            if (growthState == 4 && dragonXP < dragonAdultMinXP) {
                // to adult via ticket -> add XP
                dragonXP += dragonAdultMinXP;
            } else if  (growthState == 5 && dragonXP < dragonTitanMinXP) {
                // adult to titan via ticket -> add XP
                if (dragonXP < dragonAdultMinXP)
                    dragonXP += dragonTitanMinXP;
                else
                    dragonXP += dragonTitanMinXP - dragonAdultMinXP;
            }
            return dragonXP;
        }
    }
}
