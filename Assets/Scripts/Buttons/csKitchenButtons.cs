using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class csKitchenButtons : MonoBehaviour {

    private Canvas canvas;
    private csKitchenSceneManager sceneManager;
    private csDroppableRecipe droppableRecipe;
    public Text txtIngredientsAmount;

	// Use this for initialization
	void Start () {
        canvas = GameObject.FindObjectOfType<Canvas>();
        sceneManager = canvas.GetComponent<csKitchenSceneManager>();
        droppableRecipe = GameObject.FindObjectOfType<csDroppableRecipe>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnClickNextFood()
    {
        var item = sceneManager.FoodInventoryManager.BrowseInventoryItem<csIngredient>(csInventoryButtonEnum.Next);
        var ingredient = item.GetComponent<csIngredient>();
        if (ingredient != null)
            txtIngredientsAmount.text = ingredient.Amount.ToString();
    }

    public void OnClickPrevFood()
    {
        var item = sceneManager.FoodInventoryManager.BrowseInventoryItem<csIngredient>(csInventoryButtonEnum.Previous);
        var ingredient = item.GetComponent<csIngredient>();
        if (ingredient != null)
            txtIngredientsAmount.text = ingredient.Amount.ToString();
    }

    public void OnClickNextRecipe()
    {
        sceneManager.RecipeInventoryManager.BrowseInventoryItem<csRecipe>(csInventoryButtonEnum.Next);
    }

    public void OnClickPrevRecipe()
    {
        sceneManager.RecipeInventoryManager.BrowseInventoryItem<csRecipe>(csInventoryButtonEnum.Previous);
    }

    public void OnClickCloseRecipe()
    {
        sceneManager.CloseRecipe();

    }

    public void OnClickStartCooking()
    {
        droppableRecipe.StartCooking();
        sceneManager.CloseRecipe();
        sceneManager.HideRecipeSelector();
        sceneManager.ShowCuttingBoard();
    }

    public void OnClickEndCooking()
    {
        droppableRecipe.FinalizeRecipe();
    }

    public void OnClickGetIngredients()
    {
        sceneManager.SaveSceneData();
        SceneManager.LoadScene("IngredientsGame");
    }
}
