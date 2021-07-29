using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using UnityEngine;

[System.Serializable]
public class ItemJson{
    public int index;
    public string name;
    public string desc;
    public string en_name;
    public string en_desc;
    public string image;
    public string type;
    public int count;
    public float weight;
    public string size;
    public int size_x;
    public int size_y;
}

[System.Serializable]
public class ItemTable {
    public int box_index;
    public int item_index;
    public int item_count;
}

public class PlayerInventory : MonoBehaviour {

    public List<ItemTable> all_inventory = new List<ItemTable>();
    public List<ItemJson> all_data = new List<ItemJson>();
    private int inventory_length;
    public int inventory_count = 25;

    void Start() {
        ItemData_Load();
        Inventory_Load();
    }

    void Update() {
        
    }

    [ContextMenu("Inventory_Create")]
    public void Inventory_Create() {
        inventory_length = inventory_count;
        JArray all_itemtable = new JArray();
        for(int i = 0; i < inventory_length; i++) {
            JObject itemtable = new JObject();
            itemtable.Add("box_index", i);
            itemtable.Add("item_index", 0);
            itemtable.Add("item_count", 0);
            all_itemtable.Add(itemtable);
        }
        
        File.WriteAllText(Application.dataPath + "/Project_R/Scripts/Database/Player/playerinventory.json", all_itemtable.ToString());
    }

    [ContextMenu("Inventory_Load")]
    public void Inventory_Load() {
        all_inventory = new List<ItemTable>();
        string text_data = File.ReadAllText(Application.dataPath + "/Project_R/Scripts/Database/Player/playerinventory.json");
        JArray json_data = JArray.Parse(text_data);
        for (int i = 0; i < json_data.Count; i++) {
            all_inventory.Add(new ItemTable() {
                box_index = int.Parse(json_data[i]["box_index"].ToString()),
                item_index = int.Parse(json_data[i]["item_index"].ToString()),
                item_count = int.Parse(json_data[i]["item_count"].ToString())
            });
        }
    }

    [ContextMenu("Inventory_Save")]
    public void Inventory_Save() {
        inventory_length = inventory_count;
        JArray all_itemtable = new JArray();
        for (int i = 0; i < inventory_length; i++) {
            JObject itemtable = new JObject();
            itemtable.Add("box_index", all_inventory[i].box_index);
            itemtable.Add("item_index", all_inventory[i].item_index);
            itemtable.Add("item_count", all_inventory[i].item_count);
            all_itemtable.Add(itemtable);
        }

        File.WriteAllText(Application.dataPath + "/Project_R/Scripts/Database/Player/playerinventory.json", all_itemtable.ToString());
    }

    [ContextMenu("ItemData_Load")]
    void ItemData_Load() {
        all_data = new List<ItemJson>();
        string text_data = File.ReadAllText(Application.dataPath + "/Project_R/Scripts/Database/Item/normal_item.json");
        JObject data = JObject.Parse(text_data);
        JArray json_data = (JArray)data["item"];
        for (int i = 0; i < json_data.Count; i++) {
            all_data.Add(new ItemJson() {
                index = int.Parse(json_data[i]["index"].ToString()),
                name = json_data[i]["name"].ToString(),
                desc = json_data[i]["desc"].ToString(),
                en_name = json_data[i]["en_name"].ToString(),
                en_desc = json_data[i]["en_desc"].ToString(),
                image = json_data[i]["image"].ToString(),
                type = json_data[i]["type"].ToString(),
                count = int.Parse(json_data[i]["count"].ToString()),
                weight = float.Parse(json_data[i]["weight"].ToString()),
                size = json_data[i]["image"].ToString(),
                size_x = 0,
                size_y = 0
            });
        }
    }

    public void Find_Empty_ItemSlot(int i_index, int i_count) {
        List<int> use_index = new List<int>();
        List<int> no_index = new List<int>();

        bool is_find = false;
        int find_index = 0;

        for (int i = 0; i < all_inventory.Count; i++) {
            if(all_inventory[i].item_count > 0) {
                use_index.Add(i);
            }
            if (all_inventory[i].item_count <= 0) {
                no_index.Add(i);
            }
        }

        for (int i = 0; i < all_inventory.Count; i++) {
            if (i_index == all_inventory[i].item_index) {
                is_find = true;
                find_index = i;
            }
        }

        if (is_find) {
            Add_Item(find_index, i_index, i_count);
        }

        if (!is_find) {
            Add_Item(no_index[0], i_index, i_count);
        }
    }

    public void Add_Item(int all_index, int item_index, int item_count) {
        all_inventory[all_index].item_index = item_index;
        all_inventory[all_index].item_count += item_count;
        string text = string.Format("[{0:}] x{1:0}À» Å‰µæÇÏ¿´½À´Ï´Ù.", all_data[all_index].name, item_count);
        Debug.Log(text);
    }
}
