using sodoff.Schema;
using sodoff.Util;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace sodoff.Services {
    public class ItemService {

        Dictionary<int, ItemData> items = new();
        int[] itemsRewardForDT;
        Random random = new Random();

        public ItemService(ModdingService moddingService) {
            ServerItemArray itemArray = XmlUtil.DeserializeXml<ServerItemArray>(XmlUtil.ReadResourceXmlString("items"));
            foreach (var item in itemArray.ItemDataArray) {
                items.Add(item.ItemID, item);
            }

            itemsRewardForDT = XmlUtil.DeserializeXml<int[]>(XmlUtil.ReadResourceXmlString("dtrewards"));

            moddingService.UpdateItems(ref items);
        }

        public ItemData GetItem(int id) {
            try {
                return items[id];
            } catch {
                return null;
            }
        }

        public ItemData GetDTReward(Gender gender) {
            int itemID = 12374;
            for (int i=0; i<8; ++i) {
                // TODO: better calculation of reward item - use difficulty of DT level, item rarity, tier, etc
                itemID = itemsRewardForDT[random.Next(0, itemsRewardForDT.Length)];
                if (CheckItemGender(items[itemID], gender))
                    return items[itemID];
            }
            return items[itemID];
        }

        public int GetItemQuantity(ItemDataRelationship itemData, int bulkSize = 1) {
            if (itemData.MaxQuantity is null || itemData.MaxQuantity < 2 || itemData.MaxQuantity == itemData.Quantity) {
                if (itemData.Quantity == 0)
                    return 1 * bulkSize;
                else
                    return itemData.Quantity * bulkSize;
            }
            return random.Next(1 * bulkSize, (int)itemData.MaxQuantity * bulkSize + 1);
        }

        public ItemDataRelationship OpenBox(ItemData boxItem, Gender gender) {
            var boxRewards = boxItem.Relationship.Where(e => e.Type == "Prize").ToArray();
            int totalWeight = boxRewards.Sum(e => e.Weight);
            if (totalWeight == 0) {
                return boxRewards[random.Next(0, boxRewards.Length)];
            }
            int cnt = 0;
            int win = random.Next(0, totalWeight);
            foreach (var reward in boxRewards) {
                cnt += reward.Weight;
                if (cnt > win && CheckItemGender(items[reward.ItemId], gender)) {
                    return reward;
                }
            }
            foreach (var reward in boxRewards) { // do again in case high `win` value and CheckItemGender fail
                if (CheckItemGender(items[reward.ItemId], gender)) {
                    return reward;
                }
            }
            return null;
        }

        public void OpenBox(int boxItemId, Gender gender, out int itemId, out int quantity) {
            var reward = OpenBox(items[boxItemId], gender);
            itemId = reward.ItemId;
            quantity = GetItemQuantity(reward);
        }

        public void CheckAndOpenBox(int boxItemId, Gender gender, out int itemId, out int quantity) {
            if (IsBoxItem(boxItemId)) {
                OpenBox(boxItemId, gender, out itemId, out quantity);
            } else {
                itemId = boxItemId;
                quantity = 1;
            }
        }

        public bool IsBoxItem(int itemId) {
            return items[itemId].Relationship?.FirstOrDefault(e => e.Type == "Prize") != null;
        }

        public bool IsBundleItem(int itemId) {
            return items[itemId].Relationship?.FirstOrDefault(e => e.Type == "Bundle") != null;
        }

        public bool IsGemBundle(int itemId, out int value) {
            value = 0;
            ItemAttribute? attribute = items[itemId].Attribute?.FirstOrDefault(e => e.Key == "VCashRedemptionValue");
            if (attribute != null && int.TryParse(attribute.Value, out int result))
                value = result;
            return attribute != null;
        }

        public bool IsCoinBundle(int itemId, out int value) {
            value = 0;
            ItemAttribute? attribute = items[itemId].Attribute?.FirstOrDefault(e => e.Key == "CoinRedemptionValue");
            if (attribute != null && int.TryParse(attribute.Value, out int result))
                value = result;
            return attribute != null;
        }

        public bool CheckItemGender(ItemData itemData, Gender gender) {
            string? itemGender = itemData.Attribute?.FirstOrDefault(e => e.Key == "Gender")?.Value;
            if (itemGender != null) {
                if (gender == Gender.Male && itemGender == "F")
                    return false;
                if (gender == Gender.Female && itemGender == "M")
                    return false;
            }
            return true;
        }

        public bool ItemHasCategory(ItemData itemData, int categoryId) {
            ItemDataCategory? category = itemData.Category?.FirstOrDefault(e => e.CategoryId == categoryId);
            return category != null;
        }

        public bool ItemHasCategory(ItemData itemData, int[] categoryIds) {
            ItemDataCategory? category = itemData.Category?.FirstOrDefault(e => categoryIds.Contains(e.CategoryId));
            return category != null;
        }

        public List<ItemStat> CreateItemStats(ItemPossibleStatsMap? possibleStats, int rarity, int itemTier) {
            List<ItemStat> newStat = new List<ItemStat>();
            int rMax = possibleStats.Stats.Sum(e => e.Probability);
            for (int i=0; i<rarity && rMax > 0; ++i) {
                int val = random.Next(0, rMax);
                int cnt = 0;
                foreach (var stat in possibleStats.Stats) {
                    if (newStat.FirstOrDefault(e => e.ItemStatID == stat.ItemStatsID) != null) {
                        // this type of stat already is in newStat ... is excluded from this draw
                        continue;
                    }

                    cnt += stat.Probability;
                    if (cnt > val) {
                        rMax -= stat.Probability; // do not include in the next draw

                        StatRangeMap rangeMap = stat.ItemStatsRangeMaps.FirstOrDefault(e => e.ItemTierID == itemTier);
                        newStat.Add(
                            new ItemStat {
                                ItemStatID = rangeMap.ItemStatsID,
                                Name = rangeMap.ItemStatsName,
                                Value = random.Next(rangeMap.StartRange, rangeMap.EndRange+1).ToString(),
                                DataType = DataTypeInfo.Int
                            }
                        );

                        break; // we found new stat for slot "i" ... goto i+1
                    }
                }
            }
            return newStat;
        }
    }
}
