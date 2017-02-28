using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum InventoryManagerCode
{
    Ingredients = 0,
    Recipes = 1
}

public class csGameController : MonoBehaviour {

    public static csGameController control;

    private Dictionary<InventoryManagerCode, List<csPrefabItem>> inventoryManagers = new Dictionary<InventoryManagerCode, List<csPrefabItem>>();

	// Use this for initialization
	void Awake()
    {
        if(control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = this;
        }
        else if (control != this)
        {
            Destroy(gameObject);
        }
    }

    public csKagotchi Kagotchi {get; set;}
    public System.DateTime LastTime { get; set; }
    public List<csFightPlanElement> FightPlan { get; set; }
    public csBot CurrentBot { get; set; }

    public Dictionary<InventoryManagerCode, List<csPrefabItem>> TempInventoryManagers 
    {
        get { return inventoryManagers; }
        set { inventoryManagers = value; }
    }

    public List<csPrefabItem> SavePrefab(List<csPrefabItem> prefabList, string prefabName)
    {
        csPrefabItem foundItem = prefabList.FirstOrDefault(i => i.Name == prefabName);
        if (foundItem != null)
        {
            prefabList.Remove(foundItem);
            foundItem.Amount += 1;
        }
        else
        {
            foundItem = new csPrefabItem();
            foundItem.Name = prefabName;
            foundItem.Amount = 1;
        }
        prefabList.Add(foundItem);
        return prefabList;
    }

    public void SaveInventoryManager(InventoryManagerCode inventoryCode, csInventoryManager inventoryManager)
    {
        List<csPrefabItem> prefabs;

        prefabs = new List<csPrefabItem>();

        foreach (var prefab in inventoryManager.Inventory)
        {
            prefabs = SavePrefab(prefabs, prefab.name);
        }

        if (TempInventoryManagers.ContainsKey(inventoryCode))
            TempInventoryManagers[inventoryCode] = prefabs;
        else
            TempInventoryManagers.Add(inventoryCode, prefabs);
    }
}
