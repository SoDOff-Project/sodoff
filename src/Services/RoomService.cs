using sodoff.Model;
using sodoff.Schema;
using sodoff.Util;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace sodoff.Services;
public class RoomService {

    private readonly DBContext ctx;

    private ItemService itemService;
    private AchievementService achievementService;
    private Random random = new Random();

    public RoomService(DBContext ctx, ItemService itemService, AchievementService achievementService) {
        this.ctx = ctx;
        this.itemService = itemService;
        this.achievementService = achievementService;
    }

    public void CreateRoom(Viking viking, string roomId) {
        viking.Rooms.Add(new Room { RoomId = roomId });
        ctx.SaveChanges();
    }

    public Tuple<int[], UserItemState[]> CreateItems(UserItemPositionSetRequest[] roomItemRequest, Room room) {
        List<int> ids = new();
        List<UserItemState> states = new();
        foreach (var itemRequest in roomItemRequest) {
            ItemData itemData = itemRequest.Item;

            // TODO: Remove item from inventory (using CommonInventoryID)
            InventoryItem? i = room.Viking?.InventoryItems.FirstOrDefault(x => x.Id == itemRequest.UserInventoryCommonID);
            if (i != null) {
                i.Quantity--;
                if (itemData is null) {
                    itemData = itemService.GetItem(i.ItemId);
                }
            }

            // do not store item definition in serialised xml in database (store item id instead)
            itemRequest.Item = null;
            itemRequest.ItemID = itemData.ItemID;

            RoomItem roomItem = new RoomItem {
                RoomItemData = XmlUtil.SerializeXml<UserItemPosition>(itemRequest).Replace(" xsi:type=\"UserItemPositionSetRequest\"", "") // NOTE: No way to avoid this hack when we're serializing a child class into a base class
            };

            room.Items.Add(roomItem);
            ctx.SaveChanges();
            ids.Add(roomItem.Id);
            if (itemData.ItemStates.Count > 0) {
                ItemState defaultState = itemData.ItemStates.Find(x => x.Order == 1)!;
                UserItemState userDefaultState = new UserItemState {
                    CommonInventoryID = (int)itemRequest.UserInventoryCommonID!,
                    UserItemPositionID = roomItem.Id,
                    ItemID = (int)itemData.ItemID,
                    ItemStateID = defaultState.ItemStateID,
                    StateChangeDate = new DateTime(DateTime.Now.Ticks)
                };
                states.Add(userDefaultState);
                itemRequest.UserItemState = userDefaultState;
                roomItem.RoomItemData = XmlUtil.SerializeXml<UserItemPosition>(itemRequest).Replace(" xsi:type=\"UserItemPositionSetRequest\"", "");
                ctx.SaveChanges();
            }
        }
        return new(ids.ToArray(), states.ToArray());
    }

    public UserItemState[] UpdateItems(UserItemPositionSetRequest[] roomItemRequest, Room room) {
        List<UserItemState> state = new();
        foreach (var itemRequest in roomItemRequest) {
            RoomItem? item = room.Items.FirstOrDefault(x => x.Id == itemRequest.UserItemPositionID);
            if (item is null) continue;

            UserItemPosition itemPosition = XmlUtil.DeserializeXml<UserItemPosition>(item.RoomItemData);

            if (itemRequest.Uses != null) itemPosition.Uses = itemRequest.Uses;
            itemPosition.InvLastModifiedDate = itemRequest.InvLastModifiedDate;
            if (itemRequest.UserItemState != null) itemPosition.UserItemState = itemRequest.UserItemState;
            if (itemRequest.UserItemAttributes != null) itemPosition.UserItemAttributes = itemRequest.UserItemAttributes;
            if (itemRequest.UserItemStat != null) itemPosition.UserItemStat = itemRequest.UserItemStat;
            if (itemRequest.Item != null) itemPosition.ItemID = itemRequest.Item.ItemID;
            if (itemRequest.PositionX != null) itemPosition.PositionX = itemRequest.PositionX;
            if (itemRequest.PositionY != null) itemPosition.PositionY = itemRequest.PositionY;
            if (itemRequest.PositionZ != null) itemPosition.PositionZ = itemRequest.PositionZ;
            if (itemRequest.RotationX != null) itemPosition.RotationX = itemRequest.RotationX;
            if (itemRequest.RotationY != null) itemPosition.RotationY = itemRequest.RotationY;
            if (itemRequest.RotationZ != null) itemPosition.RotationZ = itemRequest.RotationZ;
            if (itemRequest.ParentID != null) itemPosition.ParentID = itemRequest.ParentID;

            item.RoomItemData = XmlUtil.SerializeXml(itemPosition);
            if (itemPosition.UserItemState != null) state.Add(itemPosition.UserItemState);
        }

        ctx.SaveChanges();
        return state.ToArray();
    }

    public void DeleteItems(int[] itemIds, Room room) {
        for (int i = 0; i < itemIds.Length; i++) {
            RoomItem? ri = room.Items.FirstOrDefault(x => x.Id == itemIds[i]);
            if (ri is null) continue;
            UserItemPosition itemPosition = XmlUtil.DeserializeXml<UserItemPosition>(ri.RoomItemData);
            room.Items.Remove(ri);
            InventoryItem? invItem = room.Viking?.InventoryItems.FirstOrDefault(x => x.Id == itemPosition.UserInventoryCommonID);
            if (invItem != null) invItem.Quantity++;
        }
        ctx.SaveChanges();
    }

    public UserItemPositionList GetUserItemPositionList(Room room, uint gameVersion) {
        List<UserItemPosition> itemPosition = new();
        foreach (var item in room.Items) {
            UserItemPosition data = XmlUtil.DeserializeXml<UserItemPosition>(item.RoomItemData);
            data.UserItemPositionID = item.Id;
            if (data.ItemID is null)
                data.ItemID = data.Item?.ItemID; // for backward compatibility with database entries without set `data.ItemID`
            else
                data.Item = itemService.GetItem((int)data.ItemID);
            if (gameVersion < 0xa3a00a0a && data.Uses is null)
                data.Uses = -1;
            itemPosition.Add(data);
        }
        return new UserItemPositionList { UserItemPosition = itemPosition.ToArray() };
    }

    public SetNextItemStateResult NextItemState(RoomItem item, bool speedup) {
        SetNextItemStateResult response = new SetNextItemStateResult {
            Success = true,
            ErrorCode = ItemStateChangeError.Success
        };

        UserItemPosition pos = XmlUtil.DeserializeXml<UserItemPosition>(item.RoomItemData);
        if (pos.ItemID is null)
            pos.ItemID = pos.Item?.ItemID; // for backward compatibility with database entries without set `data.ItemID`

        AchievementReward[]? rewards;
        int? achievementID;
        List<ItemStateCriteria> consumables;
        int nextStateID = GetNextStateID(pos, speedup, out rewards, out achievementID, out consumables);

        foreach (var consumable in consumables) {
            ItemStateCriteriaConsumable c = (ItemStateCriteriaConsumable)consumable;
            InventoryItem? invItem = item.Room.Viking?.InventoryItems.FirstOrDefault(x => x.ItemId == c.ItemID);
            if (invItem != null)
                invItem.Quantity = invItem.Quantity - c.Amount < 0 ? 0 : invItem.Quantity - c.Amount;
        }

        if (rewards != null) {
            response.Rewards = achievementService.ApplyAchievementRewards(item.Room.Viking, rewards);
        }
        if (achievementID != null) {
            var newrewards = achievementService.ApplyAchievementRewardsByID(item.Room.Viking, (int)achievementID);
            if (response.Rewards is null)
                response.Rewards = newrewards;
            else
                response.Rewards = response.Rewards.Concat(newrewards).ToArray();
        }

        DateTime stateChange = new DateTime(DateTime.Now.Ticks);
        if (nextStateID == -1) {
            nextStateID = pos.UserItemState.ItemStateID;
            stateChange = pos.UserItemState.StateChangeDate;
            ctx.RoomItems.Remove(item);
            ctx.SaveChanges();
        }

        response.UserItemState = new UserItemState {
            CommonInventoryID = (int)pos.UserInventoryCommonID!,
            UserItemPositionID = item.Id,
            ItemID = (int)pos.ItemID,
            ItemStateID = nextStateID,
            StateChangeDate = stateChange
        };

        if (nextStateID != -1) {
            pos.UserItemState = response.UserItemState;
            item.RoomItemData = XmlUtil.SerializeXml<UserItemPosition>(pos);
            ctx.SaveChanges();
        }

        return response;
    }

    private int GetNextStateID(UserItemPosition pos, bool speedup, out AchievementReward[]? rewards, out int? achievementID, out List<ItemStateCriteria> consumables) {
        rewards = null;
        achievementID = null;
        consumables = new List<ItemStateCriteria>();
        var itemStates = itemService.GetItem((int)pos.ItemID).ItemStates;

        if (pos.UserItemState == null)
            return itemStates.Find(x => x.Order == 1)!.ItemStateID;

        ItemState currState = itemStates.Find(x => x.ItemStateID == pos.UserItemState.ItemStateID)!;
        rewards = currState.Rewards;
        consumables = currState.Rule.Criterias.FindAll(x => x.Type == ItemStateCriteriaType.ConsumableItem);

        // achievementID = currState.AchievementID; // TODO we should do this or not? some items use the same rewards in `currState.Rewards` and achievement definition, but some do not contain only achievementID (but then there is generally no definition for achievement)
        if (currState.Rule.CompletionAction.AchievementCompletion != null) {
            achievementID = currState.Rule.CompletionAction.AchievementCompletion[
                random.Next(0, currState.Rule.CompletionAction.AchievementCompletion.Length)
            ].AchievementID;
        }

        if (speedup)
            return ((ItemStateCriteriaSpeedUpItem)currState.Rule.Criterias.Find(x => x.Type == ItemStateCriteriaType.SpeedUpItem)!).EndStateID;

        ItemStateCriteriaExpiry? expiry = (ItemStateCriteriaExpiry?)currState.Rule.Criterias.Find(x => x.Type == ItemStateCriteriaType.StateExpiry);
        if (expiry != null) {
            DateTime start = pos.UserItemState.StateChangeDate;
            if (start.AddSeconds(expiry.Period) <= new DateTime(DateTime.Now.Ticks))
                return expiry.EndStateID; 
        }
        
        switch (currState.Rule.CompletionAction.Transition) {
            default:
                return itemStates.Find(x => x.Order == currState.Order + 1)!.ItemStateID;
            case StateTransition.InitialState:
                return itemStates.Find(x => x.Order == 1)!.ItemStateID;
            case StateTransition.Deletion:
                return -1;
        }
    }
}
