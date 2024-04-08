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
            // TODO: Remove item from inventory (using CommonInventoryID)
            InventoryItem? i = room.Viking?.InventoryItems.FirstOrDefault(x => x.Id == itemRequest.UserInventoryCommonID);
            if (i != null) {
                i.Quantity--;
                if (itemRequest.Item is null) {
                    itemRequest.Item = itemService.GetItem(i.ItemId);
                }
            }

            RoomItem roomItem = new RoomItem {
                RoomItemData = XmlUtil.SerializeXml<UserItemPosition>(itemRequest).Replace(" xsi:type=\"UserItemPositionSetRequest\"", "") // NOTE: No way to avoid this hack when we're serializing a child class into a base class
            };

            room.Items.Add(roomItem);
            ctx.SaveChanges();
            ids.Add(roomItem.Id);
            if (itemRequest.Item.ItemStates.Count > 0) {
                ItemState defaultState = itemRequest.Item.ItemStates.Find(x => x.Order == 1)!;
                UserItemState userDefaultState = new UserItemState {
                    CommonInventoryID = (int)itemRequest.UserInventoryCommonID!,
                    UserItemPositionID = roomItem.Id,
                    ItemID = (int)itemRequest.Item.ItemID,
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
            if (itemRequest.Item != null) itemPosition.Item = itemRequest.Item;
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
            data.ItemID = data.Item?.ItemID;
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

        AchievementReward[]? rewards;
        List<ItemStateCriteria> consumables;
        int nextStateID = GetNextStateID(pos, speedup, out rewards, out consumables);

        foreach (var consumable in consumables) {
            ItemStateCriteriaConsumable c = (ItemStateCriteriaConsumable)consumable;
            InventoryItem? invItem = item.Room.Viking?.InventoryItems.FirstOrDefault(x => x.ItemId == c.ItemID);
            if (invItem != null)
                invItem.Quantity = invItem.Quantity - c.Amount < 0 ? 0 : invItem.Quantity - c.Amount;
        }

        if (rewards != null) {
            response.Rewards = achievementService.ApplyAchievementRewards(item.Room.Viking, rewards);
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
            ItemID = pos.Item.ItemID,
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

    private int GetNextStateID(UserItemPosition pos, bool speedup, out AchievementReward[]? rewards, out List<ItemStateCriteria> consumables) {
        rewards = null;
        consumables = new List<ItemStateCriteria>();

        if (pos.UserItemState == null)
            return pos.Item.ItemStates.Find(x => x.Order == 1)!.ItemStateID;

        ItemState currState = pos.Item.ItemStates.Find(x => x.ItemStateID == pos.UserItemState.ItemStateID)!;
        rewards = currState.Rewards;
        consumables = currState.Rule.Criterias.FindAll(x => x.Type == ItemStateCriteriaType.ConsumableItem);

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
                return pos.Item.ItemStates.Find(x => x.Order == currState.Order + 1)!.ItemStateID;
            case StateTransition.InitialState:
                return pos.Item.ItemStates.Find(x => x.Order == 1)!.ItemStateID;
            case StateTransition.Deletion:
                return -1;

        }
    }
}
