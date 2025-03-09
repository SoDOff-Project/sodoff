using sodoff.Schema;
using sodoff.Util;

using System.Xml.Serialization;

namespace sodoff.Services;

public class ModdingService {

    private Dictionary<int, ModItem> itemsToUpdate = new();
    private Dictionary<int, List<int>> itemsInStore = new();

    public ModdingService() {
        if (!Directory.Exists("mods/"))
            return;
        foreach (var dir in Directory.GetDirectories("mods/")) {
            string manifestFile = dir + "/manifest.xml";
            if (File.Exists(manifestFile)) {
                Console.WriteLine($"Load mod manifest from {manifestFile}");
                ModManifest modManifest = XmlUtil.DeserializeXml<ModManifest>(System.IO.File.ReadAllText(manifestFile));

                if (modManifest.items != null) {
                    foreach (ModItem item in modManifest.items) {
                        // get item id from mod-level info
                        int? itemID = item.ItemID;
                        
                        // process item data itemID value
                        if (itemID is null)
                            itemID = item.data?.ItemID;
                        else if (item.data != null)
                            item.data.ItemID = (int)itemID;
                        
                        // check for unset itemID
                        if (itemID is null) {
                            Console.WriteLine("Missing item id.");
                            System.Environment.Exit(1);
                        }
                        
                        try {
                            itemsToUpdate.Add((int)itemID, item);
                            if (item.stores != null && (item.action != ModAction.Remove)) {
                                foreach (int storeID in item.stores) {
                                    try {
                                        itemsInStore[storeID].Add((int)itemID);
                                    } catch (System.Collections.Generic.KeyNotFoundException) {
                                        itemsInStore.Add(storeID, new List<int> {(int)itemID});
                                    }
                                }
                            }
                        } catch (System.ArgumentException) {
                            Console.WriteLine($"Conflict for ItemID = {itemID} with previous modification.");
                            System.Environment.Exit(1);
                        }
                    }
                }
            } else {
                Console.WriteLine($"Skip mod directory {dir} (missing manifest file)");
            }
        }
    }

    public void UpdateItems(ref Dictionary<int, ItemData> items) {
        foreach (var item in itemsToUpdate) {
            if (item.Value.action == ModAction.Remove) {
                try {
                    items.Remove(item.Key);
                }  catch {
                    Console.WriteLine($"Can't remove item with ID = {item.Key} - not found");
                }
            } else if (item.Value.action == ModAction.Replace) {
                try {
                    items[item.Key] = item.Value.data;
                }  catch {
                    Console.WriteLine($"Can't replace item with ID = {item.Key} - not found");
                }
            } else /*if (item.Value.action == ModAction.Add || item.Value.action == ModAction.Default)*/ {
                try {
                    items.Add(item.Key, item.Value.data);
                }  catch {
                    Console.WriteLine($"Item with ID = {item.Key} is already defined in base item set.");
                    System.Environment.Exit(1);
                }
            }
        }
        itemsToUpdate.Clear();
    }
    public List<int> GetStoreItem(int storeID) {
        if (itemsInStore.ContainsKey(storeID)) {
            return itemsInStore[storeID];
        } else {
            return new List<int>();
        }
    }
}
