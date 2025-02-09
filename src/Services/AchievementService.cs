using sodoff.Schema;
using sodoff.Model;
using sodoff.Util;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace sodoff.Services {
    public class AchievementService {
        private AchievementStoreSingleton achievementStore;
        private InventoryService inventoryService;
        public readonly DBContext ctx;

        public AchievementService(AchievementStoreSingleton achievementStore, InventoryService inventoryService, DBContext ctx) {
            this.achievementStore = achievementStore;
            this.inventoryService = inventoryService;
            this.ctx = ctx;
        }

        public UserAchievementInfo CreateUserAchievementInfo(Guid userId, int? value, AchievementPointTypes type) {
            if (value is null)
                value = 0;
            return new UserAchievementInfo {
                UserID = userId,
                AchievementPointTotal = value,
                RankID = achievementStore.GetRankFromXP(value, type),
                PointTypeID = type
            };
        }

        public UserAchievementInfo CreateUserAchievementInfo(Viking viking, AchievementPointTypes type) {
            return CreateUserAchievementInfo(viking.Uid, viking.AchievementPoints.FirstOrDefault(a => a.Type == (int)type)?.Value, type);
        }

        public void DragonLevelUpOnAgeUp(Dragon dragon, RaisedPetGrowthState oldGrowthState, RaisedPetGrowthState newGrowthState) {
            if (oldGrowthState is null || newGrowthState.Order > oldGrowthState.Order) { // if dragon age up
                dragon.PetXP = achievementStore.GetUpdatedDragonXP(dragon.PetXP ?? 0, newGrowthState.Order);
            }
        }

        public void SetAchievementPoints(Viking viking, AchievementPointTypes type, int value) {
            if (type == AchievementPointTypes.DragonXP && viking.SelectedDragon != null) {
                viking.SelectedDragon.PetXP = value;
            } else if (type != null) {
                AchievementPoints xpPoints = viking.AchievementPoints.FirstOrDefault(a => a.Type == (int)type);
                if (xpPoints is null) {
                    xpPoints = new AchievementPoints {
                        Type = (int)type
                    };
                    viking.AchievementPoints.Add(xpPoints);
                }
                xpPoints.Value = value;
            }
        }

        public AchievementReward? AddDragonAchievementPoints(Dragon dragon, int? value) {
            dragon.PetXP = (dragon.PetXP ?? 0) + (value ?? 0);

            if (dragon.PetXP < 0) {
                dragon.PetXP = int.MaxValue;
                value = 0;
            }

            return new AchievementReward{
                // NOTE: RewardID and EntityTypeID are not used by client
                EntityID = dragon.EntityId,
                PointTypeID = AchievementPointTypes.DragonXP,
                Amount = value ?? 0
            };
        }

        public AchievementReward? AddAchievementPoints(Viking viking, AchievementPointTypes? type, int? value) {
            if (type == AchievementPointTypes.DragonXP && viking.SelectedDragon != null) {
                return AddDragonAchievementPoints(viking.SelectedDragon, value);
            } else if (type != null) {
                AchievementPoints xpPoints = viking.AchievementPoints.FirstOrDefault(a => a.Type == (int)type);
                if (xpPoints is null) {
                    xpPoints = new AchievementPoints {
                        Type = (int)type,
                        Value = 0
                    };
                    viking.AchievementPoints.Add(xpPoints);
                }
                
                int initialPoints = xpPoints.Value;
                xpPoints.Value += value ?? 0;
                
                if (value > 0 && initialPoints > xpPoints.Value) {
                    xpPoints.Value = int.MaxValue;
                    value = int.MaxValue - initialPoints;
                }
                if (xpPoints.Value < 0) {
                    xpPoints.Value = 0;
                    value = 0;
                }

                ctx.SaveChanges();

                return new AchievementReward{
                    EntityID = viking.Uid,
                    PointTypeID = type,
                    Amount = value
                };
            }
            return null;
        }

        public AchievementReward? ApplyAchievementReward(Viking viking, AchievementReward reward) {
            if (reward.PointTypeID == AchievementPointTypes.ItemReward) {
                inventoryService.AddItemToInventory(viking, reward.ItemID, (int)reward.Amount!);

                AchievementReward grantedReward = reward.Clone();
                grantedReward.EntityID = viking.Uid;
                return grantedReward;
            } else { // currencies, all types of player XP and dragon XP
                return AddAchievementPoints(viking, reward.PointTypeID, reward.Amount);
            }
        }

        public AchievementReward[] ApplyAchievementRewards(Viking viking, AchievementReward[] rewards, Guid[]? dragonsIDs = null) {
            if (rewards is null)
                return null;
            List<AchievementReward> grantedRewards = new List<AchievementReward>();
            foreach (var reward in rewards) {
                if (dragonsIDs != null && reward.PointTypeID == AchievementPointTypes.DragonXP) {
                    if (dragonsIDs.Length == 0)
                        continue;
                    double amountDouble = (reward.Amount ?? 0)/dragonsIDs.Length;
                    int amount = (int)Math.Ceiling(amountDouble);
                    foreach (Guid dragonID in dragonsIDs) {
                        Dragon dragon = viking.Dragons.FirstOrDefault(e => e.EntityId == dragonID);
                        grantedRewards.Add(
                            AddDragonAchievementPoints(dragon, amount)
                        );
                    }
                } else {
                    grantedRewards.Add(
                        ApplyAchievementReward(viking, reward)
                    );
                }
            }

            // TODO: check trophies, etc criteria and id need apply and add to results extra reward here

            return grantedRewards.ToArray();
        }

        public AchievementReward[] ApplyAchievementRewardsByID(Viking viking, int achievementID, Guid[]? dragonsIDs = null) {
            var rewards = achievementStore.GetAchievementRewardsById(achievementID);
            if (rewards != null) {
                return ApplyAchievementRewards(viking, rewards, dragonsIDs);
            } else {
                return new AchievementReward[0];
            }
        }

        public AchievementTaskSetResponse ApplyAchievementRewardsByTask(Viking viking, int taskID, uint gameVersion) {
            AchievementTaskState? achievementTaskState = viking.AchievementTaskStates.FirstOrDefault(x => x.TaskId == taskID);
            int pointValue = (achievementTaskState?.Points ?? 0);
            var achievementInfo = achievementStore.GetAchievementTaskInfo(taskID, gameVersion, pointValue);
            var lastLevelCompleted = false;

            if (achievementInfo == null) return new AchievementTaskSetResponse();

            if (pointValue < achievementInfo.PointValue) { // limit points stored value to max points value in achievement tasks
                pointValue += 1;
                lastLevelCompleted = true;
            }

            var rewards = (achievementInfo.Reproducible || pointValue == achievementInfo.PointValue)
                ? ApplyAchievementRewards(viking, achievementInfo.Rewards)
                : Array.Empty<AchievementReward>();

            if (achievementTaskState == null)
                viking.AchievementTaskStates.Add(new AchievementTaskState { TaskId = taskID, Points = pointValue });
            else
                achievementTaskState.Points = pointValue;

            ctx.SaveChanges();

            return new AchievementTaskSetResponse {
                Success = true,
                UserMessage = true, // TODO: placeholder
                AchievementName = achievementInfo.Name,
                Level = achievementInfo.Level,
                AchievementTaskGroupID = achievementInfo.TaskGroupID,
                LastLevelCompleted = lastLevelCompleted,
                AchievementInfoID = achievementInfo.InfoID,
                AchievementRewards = rewards
            };
        }

        public UserGameCurrency GetUserCurrency(Viking viking) {
            int? coins = viking.AchievementPoints.FirstOrDefault(x => x.Type == (int)AchievementPointTypes.GameCurrency)?.Value;
            int? gems = viking.AchievementPoints.FirstOrDefault(x => x.Type == (int)AchievementPointTypes.CashCurrency)?.Value;
            if (coins is null) {
                coins = 300;
                AddAchievementPoints(viking, AchievementPointTypes.GameCurrency, coins);
            }
            if (gems is null) {
                gems = 75;
                AddAchievementPoints(viking, AchievementPointTypes.CashCurrency, gems);
            }
            return new UserGameCurrency {
                CashCurrency = gems,
                GameCurrency = coins,
                UserGameCurrencyID = viking.Id,
                UserID = viking.Uid
            };
        }

        public UserAchievementInfoResponse GetTopAchievementUsers(UserAchievementInfoRequest request) {
            // TODO: Type and mode are currently ignored
            List<UserAchievementInfo> achievementInfo = new();
            var topAchievers = ctx.AchievementPoints.Where(x => x.Type == request.PointTypeID)
                .Select(e => new { e.Viking.Uid, e.Viking.Name, e.Value })
                .OrderByDescending(e => e.Value)
                .Skip((request.Page - 1) * request.Quantity)
                .Take(request.Quantity);

            foreach (var a in topAchievers) {
                achievementInfo.Add(new UserAchievementInfo {
                    UserID = a.Uid,
                    UserName = a.Name,
                    AchievementPointTotal = a.Value,
                    PointTypeID = (AchievementPointTypes)request.PointTypeID
                });
            }

            return new UserAchievementInfoResponse {
                AchievementInfo = achievementInfo.ToArray(),
                DateRange = new DateRange()
            };
        }

        public ArrayOfUserAchievementInfo GetTopAchievementBuddies(UserAchievementInfoRequest request) {
            // TODO: Type and mode are currently ignored
            List<UserAchievementInfo> achievementInfo = new();
            var topAchievers = ctx.AchievementPoints.Where(x => x.Type == request.PointTypeID)
                .Select(e => new { e.Viking.Uid, e.Viking.Name, e.Value })
                .OrderByDescending(e => e.Value)
                .Take(10); // Lets not sent 50,000 entries to the client please.

            foreach (var a in topAchievers) {
                achievementInfo.Add(new UserAchievementInfo {
                    UserID = a.Uid,
                    UserName = a.Name,
                    AchievementPointTotal = a.Value,
                    PointTypeID = (AchievementPointTypes)request.PointTypeID
                });
            }
            if (achievementInfo.Count<=0) { // If there are no points for this (liekly won't be until implemented), just add GC and everything might be fine.
                achievementInfo.Add(new UserAchievementInfo {
                    UserID = new Guid("ffffffff-0000-0000-0000-000000000001"),
                    UserName = "GC",
                    AchievementPointTotal = 69,
                    PointTypeID = (AchievementPointTypes)request.PointTypeID
                });
            }

            return new ArrayOfUserAchievementInfo {
                UserAchievementInfo = achievementInfo.ToArray()
            };
        }

        public UserAchievementTask[] GetUserAchievementTask(Viking viking, uint gameVersion) {
            List <UserAchievementTask> userAchievementTasks = new();

            foreach (var achievementsGroup in achievementStore.GetAchievementsGroupIdToTaskId(gameVersion)) {
                List<AchievementTaskReward> rewards = new();
                int nextLevel = 0;
                int points = viking.AchievementTaskStates.FirstOrDefault(x => x.TaskId == achievementsGroup.Value)?.Points ?? 0;
                var achievementInfo = achievementStore.GetAchievementTaskInfoForNextLevel(
                    achievementsGroup.Value, gameVersion, points, achievementsGroup.Key
                );

                if (achievementInfo != null) {
                    nextLevel = achievementInfo.Level;
                    if (achievementInfo.Rewards != null) {
                        foreach (var r in achievementInfo.Rewards) {
                            rewards.Add(new AchievementTaskReward {
                                RewardQuantity = r.Amount ?? 0,
                                PointTypeID = (int)(r.PointTypeID),
                                ItemID = r.ItemID,
                                RewardID = r.RewardID,
                                AchievementInfoID = achievementInfo.InfoID
                            });
                        }
                    }
                } else {
                    achievementInfo = achievementStore.GetAchievementTaskInfoForCurrentLevel(
                        achievementsGroup.Value, gameVersion, points, achievementsGroup.Key
                    );
                    nextLevel = achievementInfo.Level + 1;
                }

                userAchievementTasks.Add(new UserAchievementTask {
                    AchievementTaskGroupID = achievementsGroup.Key,
                    AchievedQuantity = Math.Min(points, achievementInfo.PointValue),
                    NextLevel = nextLevel,
                    QuantityRequired = Math.Max(0, achievementInfo.PointValue - points),
                    NextLevelAchievementRewards = rewards.ToArray()
                });
            }
            return userAchievementTasks.ToArray();
        }
    }
}
