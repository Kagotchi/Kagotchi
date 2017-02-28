using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class csLoot : MonoBehaviour {

    private BoxCollider2D boxCollider;

    [SerializeField]
    private int probability;

    public int Probability
    {
        get { return probability; }
        set { probability = value; }
    }

	// Use this for initialization
	void Start () 
    {
        if (gameObject.GetComponent<BoxCollider2D>() == null)
        {
            boxCollider = gameObject.AddComponent<BoxCollider2D>();
            boxCollider.isTrigger = true;
            boxCollider.size = gameObject.GetComponent<RectTransform>().sizeDelta;
        }
        if(gameObject.GetComponent<CanvasGroup>() == null)
        {
            var canvasGroup = gameObject.AddComponent<CanvasGroup>();
            gameObject.SetActive(true);
            canvasGroup.blocksRaycasts = true;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            List<csPrefabItem> ingredients;
            if (csGameController.control.TempInventoryManagers.TryGetValue(InventoryManagerCode.Ingredients, out ingredients))
            {
                ingredients = csGameController.control.SavePrefab(ingredients, gameObject.name);
            }
            if (csGameController.control.TempInventoryManagers.ContainsKey(InventoryManagerCode.Ingredients))
                csGameController.control.TempInventoryManagers[InventoryManagerCode.Ingredients] = ingredients;
            else
                csGameController.control.TempInventoryManagers.Add(InventoryManagerCode.Ingredients, ingredients);
            Destroy(gameObject);
        }
    }
}
