using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class csInventoryManager  
{

    private int invIdx = 0;

    public GameObject HierarchyParent { get; set; }

    public List<GameObject> Inventory { get; set; }

    public GameObject GetNextActiveInventoryItem<T>()
    {
        if (Inventory.Count > 0)
        {
            invIdx = 0;
            return Inventory[0] as GameObject;
        }
        else
            return null;
    }

    private int GetIndex(csInventoryButtonEnum action)
    {
        if (action == csInventoryButtonEnum.Next)
            invIdx++;
        else
            invIdx--;

        if (invIdx > Inventory.Count - 1)
            invIdx = Inventory.Count - 1;

        if (invIdx < 0)
            invIdx = 0;

        return invIdx;
    }

    public GameObject BrowseInventoryItem<T>(csInventoryButtonEnum action)
    {
        if (Inventory.Count > 0)
        {
            var currentItemGameObject = Inventory[invIdx] as GameObject;
            currentItemGameObject.SetActive(false);

            invIdx = GetIndex(action);

            var itemGameObject = Inventory[invIdx] as GameObject;
            if (itemGameObject != null)
            {
                itemGameObject.SetActive(true);
                if (itemGameObject.GetComponent<CanvasGroup>() == null)
                    itemGameObject.AddComponent<CanvasGroup>();
                itemGameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
                itemGameObject.GetComponent<CanvasGroup>().interactable = true;
                itemGameObject.transform.SetParent(HierarchyParent.transform, false);

                return itemGameObject.gameObject;
            }
            else
                return null;
        }
        else
            return null;
    }

    public void Remove(GameObject item)
    {
        Inventory.Remove(item);
    }
}
