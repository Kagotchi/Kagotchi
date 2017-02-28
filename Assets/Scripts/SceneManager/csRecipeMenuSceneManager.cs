using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class csRecipeMenuSceneManager : MonoBehaviour {

    [SerializeField]
    private Image recipeImage;
    [SerializeField]
    private Text recipeName;
    [SerializeField]
    private List<GameObject> recipes;
    [SerializeField]
    private GameObject ingredientsGrid;
    [SerializeField]
    private GameObject toolsGrid;

    private csRecipe currentRecipe;
    private int recipeIdx;

	// Use this for initialization
	void Start () 
    {
        GenerateRecipeItems(0);
	}

    private void CreateListItem(List<GameObject> objectList, GameObject parent)
    {
        if (parent.transform.childCount > 0)
        {
            for (var i = 0; i < parent.transform.childCount; i++)
            {
                Destroy(parent.transform.GetChild(i).gameObject);
            }
        }
        
        foreach (var item in objectList)
        {
            var itemPref = Resources.Load("Prefabs/UI/IngredientListElement");
            if (itemPref != null)
            {
                var clone = (GameObject)Instantiate(itemPref);
                clone.name = itemPref.name;
                clone.SetActive(true);
                clone.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                var recipeElem = clone.GetComponent<csRecipePrefabElement>();
                var elem = item.GetComponent<csIRecipeItem>();
                recipeElem.AmountMax = elem.AmountMax.ToString();
                recipeElem.Name = elem.Name;
                  var image = item.GetComponent<Image>();
                recipeElem.Image = image;
                clone.transform.SetParent(parent.transform, false);
            }
        }
    }

    private void GenerateRecipeItems(int idx)
    {
        currentRecipe = recipes[idx].GetComponent<csRecipe>();
        recipeName.text = currentRecipe.RecipeName;
        recipeImage.sprite = currentRecipe.RecipeImage;
        CreateListItem(currentRecipe.ingredients, ingredientsGrid);
        CreateListItem(currentRecipe.kitchenware, toolsGrid);
    }

    public void NextRecipe()
    {
        recipeIdx++;
        GenerateRecipeItems(recipeIdx);
    }

    public void PreviousRecipe()
    {
        recipeIdx--;
        GenerateRecipeItems(recipeIdx);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
