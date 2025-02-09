using sodoff.Schema;
using sodoff.Model;
using sodoff.Util;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using System.Collections.ObjectModel;

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

        public AchievementStoreSingleton() {
            ArrayOfUserRank allranks = XmlUtil.DeserializeXml<ArrayOfUserRank>(XmlUtil.ReadResourceXmlString("ranks.allranks_sod"));
            foreach (var pointType in Enum.GetValues<AchievementPointTypes>()) {
                ranks[pointType] = allranks.UserRank.Where(r => r.PointTypeID == pointType).ToArray();
            }

            AchievementsIdInfo[] allAchievementsIdInfo = XmlUtil.DeserializeXml<AchievementsIdInfo[]>(XmlUtil.ReadResourceXmlString("achievements.achievementid_sod"));
            foreach (var achievementsIdInfo in allAchievementsIdInfo) {
                achievementsRewardByID[achievementsIdInfo.AchievementID] = achievementsIdInfo.AchievementReward;
            }

            achievementsTasks[ClientVersion.Min_SoD] = new AchievementTasks("achievements.achievementtaskinfo_sod");
            achievementsTasks[ClientVersion.MaM] = new AchievementTasks("achievements.achievementtaskinfo_mam");
            achievementsTasks[ClientVersion.MB] = new AchievementTasks("achievements.achievementtaskinfo_mb");
            achievementsTasks[ClientVersion.EMD] = new AchievementTasks("achievements.achievementtaskinfo_emd");
            achievementsTasks[ClientVersion.SS] = new AchievementTasks("achievements.achievementtaskinfo_ss");
            achievementsTasks[ClientVersion.WoJS] = new AchievementTasks("achievements.achievementtaskinfo_wojs");

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
            gameVersion = GameVersionForTasks(gameVersion);
            if (achievementsTasks.ContainsKey(gameVersion))
                return achievementsTasks[gameVersion].achievementsGroupIdToTaskId;
            return new ReadOnlyDictionary<int, int>(new Dictionary<int, int>());
        }

        public List<AchievementTaskInfo>? GetAllAchievementTaskInfo(int taskID, uint gameVersion) {
            gameVersion = GameVersionForTasks(gameVersion);
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
                dragonXP += dragonTitanMinXP - dragonAdultMinXP;
            }
            return dragonXP;
        }

        private uint GameVersionForTasks(uint gameVersion) {
            // all SoD version of SoD using the same Tasks database
            if ((gameVersion & ClientVersion.Min_SoD) == 0xa0000000) return ClientVersion.Min_SoD;
            // all version of WoJS (including lands) using the same Tasks database
            if (gameVersion <= ClientVersion.Max_OldJS && (gameVersion & ClientVersion.WoJS) != 0) return ClientVersion.WoJS;
            return gameVersion;
        }
    }
}
