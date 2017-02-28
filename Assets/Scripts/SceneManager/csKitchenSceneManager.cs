using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using Assets.Scripts;
using System;
public class csKitchenSceneManager : MonoBehaviour, csISceneManager
{
    public Text elapsedTime;
    public Text noItemFood;
    public Text noItemRecipe;
    public Text txtIngredientsAmount;

    private float _time = 0;
    private List<GameObject> foodInventory = new List<GameObject>();
    private List<GameObject> recipeInventory = new List<GameObject>();
    private csInventoryManager inventoryIngredientsManager = new csInventoryManager();
    private csInventoryManager inventoryRecipeManager = new csInventoryManager();
    private List<GameObject> kitchenwareInventory = new List<GameObject>();
    private int invIdx = 0;

    private CanvasGroup statusUICanvas;
    private CanvasGroup cookingUICanvas;
    private CanvasGroup cookingStatusUICanvas;
    private CanvasGroup foodSelectorUICanvas;
    private CanvasGroup recipeSelectorUICanvas;
    private CanvasGroup recipeUICanvas;

    private GameObject scenery;
    private GameObject kagotchi;
    private GameObject statusUI;
    private GameObject cookingStatusUI;
    private GameObject cookingUI;
    
    private GameObject recipeSelectorUI;
    private GameObject recipeUI;
    private GameObject pnlMouthTrigger;
    private GameObject btnStopCooking;

    

    private RectTransform sceneryRectTransform;
    private RectTransform kagotchyRectTransform;

    private float sceneryLeftLimitX = -153.0f;
    private float sceneryRightLimitX = -610.0f;
    private float speed = 1000.0f;
    private Vector2 kagotchiStartPosition;

    private bool isScrolling = false;
    private bool reachedRightLimit = false;
    private bool reachedLeftLimit = false;

    private csRectTransformData foodRectTransform;

    public csInventoryManager FoodInventoryManager { get; set; }

    public csInventoryManager RecipeInventoryManager { get; set; }

    public CanvasGroup RecipeUICanvasGroup { get; set; }

    public GameObject FoodSelectorUI { get; set; }

    public GameObject CurrentKitchenware { get; set; }

    public List<GameObject> KitchenwareInventory
    {
        get { return kitchenwareInventory; }
        set { kitchenwareInventory = value; }
    }


    public csRecipe CurrentRecipe { get; set; }
	// Use this for initialization
	void Start () 
    {
        Screen.orientation = ScreenOrientation.Portrait;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        if (!Application.runInBackground) 
            Application.runInBackground = true;

        scenery = GameObject.Find("Kitchen");
        kagotchi = GameObject.Find("Kagotchi");


        statusUI = GameObject.Find("StatusUI");
        statusUICanvas = statusUI.GetComponent<CanvasGroup>();

        cookingStatusUI = GameObject.Find("CookingStatusUI");
        cookingStatusUICanvas = cookingStatusUI.GetComponent<CanvasGroup>();

        cookingUI = GameObject.Find("CookingUI");
        cookingUICanvas = cookingUI.GetComponent<CanvasGroup>();

        FoodSelectorUI = GameObject.Find("FoodSelectorUI");
        foodSelectorUICanvas = FoodSelectorUI.GetComponent<CanvasGroup>();

        recipeSelectorUI = GameObject.Find("RecipeSelectorUI");
        recipeSelectorUICanvas = recipeSelectorUI.GetComponent<CanvasGroup>();

        recipeUI = GameObject.Find("RecipeUI");
        recipeUICanvas = recipeUI.GetComponent<CanvasGroup>();
        RecipeUICanvasGroup = recipeUICanvas;

        btnStopCooking = GameObject.Find("btnStopCooking");

        sceneryRectTransform = scenery.GetComponent<RectTransform>();
        kagotchyRectTransform = kagotchi.GetComponent<RectTransform>();

        kagotchiStartPosition = kagotchyRectTransform.anchoredPosition;

        foodRectTransform = new csRectTransformData()
        {
            AnchorMin = new Vector2(1, 1),
            AnchorMax = new Vector2(1, 1),
            Pivot = new Vector2(0.5f, 0.5f),
            AnchoredPosition = new Vector2(-400, -89),
            SizeDelta = new Vector2(120, 114)
        };

        //LoadInventoryItemInSelector(foodInventory, FoodSelectorUI, "Prefabs/Ingredients/Banana", foodRectTransform);
        //LoadInventoryItemInSelector(foodInventory, FoodSelectorUI, "Prefabs/Ingredients/HamburgerBread", foodRectTransform);
        //LoadInventoryItemInSelector(foodInventory, FoodSelectorUI, "Prefabs/Ingredients/Bacon", foodRectTransform);
        //LoadInventoryItemInSelector(foodInventory, FoodSelectorUI, "Prefabs/Ingredients/Cheese", foodRectTransform);
        //LoadInventoryItemInSelector(foodInventory, FoodSelectorUI, "Ingredients/Meat", foodRectTransform);

        inventoryIngredientsManager.Inventory = foodInventory;

        var recipeRectTransform = new csRectTransformData()
        {
            AnchorMin = new Vector2(0.5f, 0.5f),
            AnchorMax = new Vector2(0.5f, 0.5f),
            Pivot = new Vector2(0.5f, 0.5f),
            AnchoredPosition = new Vector2(7.629395e-06f, 1.239777e-05f),
            SizeDelta = new Vector2(110, 155)
        };


        recipeInventory.Add(LoadItemInSelector(recipeSelectorUI, "Prefabs/Recipes/Hamburger Recipe", recipeRectTransform));
        recipeInventory.Add(LoadItemInSelector(recipeSelectorUI, "Prefabs/Recipes/Salad Recipe", recipeRectTransform));
        recipeInventory.Add(LoadItemInSelector(recipeSelectorUI, "Prefabs/Recipes/Hot Dog Recipe", recipeRectTransform));

        inventoryRecipeManager.Inventory = recipeInventory;

        LoadItemInInventory(kitchenwareInventory, "Prefabs/Kitchenware/Grill");

        if (foodInventory.Count > 0)
            noItemFood.gameObject.SetActive(false);
        else
            noItemFood.gameObject.SetActive(true);

        if (recipeInventory.Count > 0)
            noItemRecipe.gameObject.SetActive(false);
        else
            noItemRecipe.gameObject.SetActive(true);

        
        if(foodInventory.Count > 0)
        {
            var food = foodInventory[invIdx] as GameObject;
            food.SetActive(true);
            food.GetComponent<CanvasGroup>().blocksRaycasts = true;
            food.transform.SetParent(FoodSelectorUI.transform, false);
        }

        var recipe = recipeInventory[invIdx] as GameObject;
        recipe.SetActive(true);
        recipe.GetComponent<CanvasGroup>().blocksRaycasts = true;
        recipe.transform.SetParent(recipeSelectorUI.transform, false);

        inventoryIngredientsManager.HierarchyParent = FoodSelectorUI;
        inventoryRecipeManager.HierarchyParent = recipeSelectorUI;

        FoodInventoryManager = inventoryIngredientsManager;
        RecipeInventoryManager = inventoryRecipeManager;

        pnlMouthTrigger = GameObject.Find("pnlMouthTrigger");

        LoadSceneData();
	}
	
	// Update is called once per frame
	void Update () 
    {
        _time += Time.deltaTime;
        elapsedTime.text = Mathf.RoundToInt(_time).ToString();
	}



    private GameObject LoadItemInSelector(GameObject selector, string itemPath, csRectTransformData rectTransform)
    {
        var itemObject = Resources.Load(itemPath);
        if (itemObject != null)
        {
            var clone = (GameObject)Instantiate(itemObject);
            clone.name = itemObject.name;
            clone.transform.SetParent(FoodSelectorUI.transform, false);
            clone.GetComponent<RectTransform>().anchorMin = rectTransform.AnchorMin;
            clone.GetComponent<RectTransform>().anchorMax = rectTransform.AnchorMax;
            clone.GetComponent<RectTransform>().pivot = rectTransform.Pivot;
            clone.GetComponent<RectTransform>().anchoredPosition = rectTransform.AnchoredPosition;
            clone.GetComponent<RectTransform>().sizeDelta = rectTransform.SizeDelta;
            clone.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
            clone.SetActive(false);

            if (clone.GetComponent<csDragHandler>() == null)
                clone.AddComponent<csDragHandler>();

            var animator = clone.GetComponent<Animator>();
            if (animator != null)
                animator.enabled = false;

            return clone;
        }
        return null;
    }

    public GameObject LoadItemInSelector(GameObject selector, GameObject itemObject, csRectTransformData rectTransform)
    {
        if (itemObject != null)
        {
            var clone = (GameObject)Instantiate(itemObject);
            clone.name = itemObject.name;
            clone.transform.SetParent(FoodSelectorUI.transform, false);
            clone.GetComponent<RectTransform>().anchorMin = rectTransform.AnchorMin;
            clone.GetComponent<RectTransform>().anchorMax = rectTransform.AnchorMax;
            clone.GetComponent<RectTransform>().pivot = rectTransform.Pivot;
            clone.GetComponent<RectTransform>().anchoredPosition = rectTransform.AnchoredPosition;
            clone.GetComponent<RectTransform>().sizeDelta = rectTransform.SizeDelta;
            clone.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
            clone.SetActive(false);

            if (clone.GetComponent<csDragHandler>() == null)
                clone.AddComponent<csDragHandler>();

            var animator = clone.GetComponent<Animator>();
            if (animator != null)
                animator.enabled = false;
            return clone;
        }
        return null;
    }

    private void LoadItemInInventory(List<GameObject> inventory, string itemPath)
    {
        var item = Resources.Load(itemPath);
        inventory.Add((GameObject)item);
    }

    public void UpdateScenery(bool scroll, float direction)
    {
        if (scroll)
            isScrolling = true;


        if (direction < 0 && isScrolling == true)
        {
            sceneryRectTransform.anchoredPosition += Vector2.left * speed * Time.deltaTime;
            if (sceneryRectTransform.anchoredPosition.x < sceneryRightLimitX)
            {
                sceneryRectTransform.anchoredPosition = new Vector2(sceneryRightLimitX, sceneryRectTransform.anchoredPosition.y);
                isScrolling = false;
            }
            
        }
        else if (direction > 0 && isScrolling == true)
        {
            sceneryRectTransform.anchoredPosition += Vector2.right * speed * Time.deltaTime;
            if (sceneryRectTransform.anchoredPosition.x > sceneryLeftLimitX)
            {
                sceneryRectTransform.anchoredPosition = new Vector2(sceneryLeftLimitX, sceneryRectTransform.anchoredPosition.y);
                isScrolling = false;
            }
        }

        if (sceneryRectTransform.anchoredPosition.x == sceneryRightLimitX && !reachedRightLimit)
        {
            statusUICanvas.alpha = 0;
            cookingStatusUICanvas.alpha = 1;
            cookingUICanvas.alpha = 0;

            foodSelectorUICanvas.alpha = 0;
            foodSelectorUICanvas.blocksRaycasts = false;
            foodSelectorUICanvas.interactable = false;

            if(CurrentRecipe == null || (CurrentRecipe != null && (CurrentRecipe.State == csRecipeStateEnum.None || CurrentRecipe.State == csRecipeStateEnum.Finished)))
                ShowRecipeSelector();

            if(CurrentRecipe != null && CurrentRecipe.State == csRecipeStateEnum.InProgress)
            {
                ShowCuttingBoard();
                ToggleKitchenware(true);
            }

            pnlMouthTrigger.GetComponent<CanvasGroup>().interactable = false;
            pnlMouthTrigger.GetComponent<CanvasGroup>().blocksRaycasts = false;

            kagotchyRectTransform.anchoredPosition += new Vector2(-160, 0);
            reachedRightLimit = true;
            reachedLeftLimit = false;

            kagotchi.GetComponent<csKagotchi>().SetValues();
        }

        if (sceneryRectTransform.anchoredPosition.x == sceneryLeftLimitX && !reachedLeftLimit)
        {
            statusUICanvas.alpha = 1;
            cookingStatusUICanvas.alpha = 0;
            cookingUICanvas.alpha = 0;

            foodSelectorUICanvas.alpha = 1;
            foodSelectorUICanvas.blocksRaycasts = true;
            foodSelectorUICanvas.interactable = true;

            HideRecipeSelector();

            pnlMouthTrigger.GetComponent<CanvasGroup>().interactable = true;
            pnlMouthTrigger.GetComponent<CanvasGroup>().blocksRaycasts = true;

            recipeUICanvas.alpha = 0;
            recipeUICanvas.blocksRaycasts = false;
            recipeUICanvas.interactable = false;

            kagotchyRectTransform.anchoredPosition = kagotchiStartPosition;
            reachedRightLimit = false;
            reachedLeftLimit = true;

            ToggleStopCookingButton(false);
            ToggleKitchenware(false);

            kagotchi.GetComponent<csKagotchi>().LoadValues();
            
        }
    }

    public void OnApplicationPause(bool paused)
    {
        if (paused)
        {
            // Game is paused, remember the time
        }
        else
        {
            // Game is unpaused, calculate the time passed since the game was paused and use this time to calculate build times of your buildings or how much money the player has gained in the meantime.
        }
    }

    public GameObject GetNextActiveFoodItem()
    {
        return inventoryIngredientsManager.GetNextActiveInventoryItem<csIngredient>();
    }

    public void RemoveFoodItem(GameObject item)
    {
        inventoryIngredientsManager.Remove(item);
    }

    public void SetNoItemVisibility(bool visible)
    {
        noItemFood.gameObject.SetActive(visible);
    }

    public void ShowRecipeSelector()
    {
        recipeSelectorUICanvas.alpha = 1;
        recipeSelectorUICanvas.blocksRaycasts = true;
        recipeSelectorUICanvas.interactable = true;
    }

    public void HideRecipeSelector()
    {
        recipeSelectorUICanvas.alpha = 0;
        recipeSelectorUICanvas.blocksRaycasts = false;
        recipeSelectorUICanvas.interactable = false;
    }

    public void CloseRecipe()
    {
        RecipeUICanvasGroup.alpha = 0;
        RecipeUICanvasGroup.blocksRaycasts = false;
        RecipeUICanvasGroup.interactable = false;
    }

    public void ShowCuttingBoard()
    {
        cookingUICanvas.alpha = 1;
        cookingUICanvas.blocksRaycasts = true;
        cookingUICanvas.interactable = true;
    }

    public void HideCuttingBoard()
    {
        cookingUICanvas.alpha = 0;
        cookingUICanvas.blocksRaycasts = false;
        cookingUICanvas.interactable = false;
    }

    public void ToggleStopCookingButton(bool visible)
    {
        ToggleGameObject(btnStopCooking, visible);
    }

    public void ToggleGameObject(GameObject gameObject, bool visible)
    {
        if (gameObject != null)
        {
            if (gameObject.GetComponent<CanvasGroup>() == null)
                gameObject.AddComponent<CanvasGroup>();

            if (visible)
            {
                gameObject.GetComponent<CanvasGroup>().alpha = 1;
                gameObject.GetComponent<CanvasGroup>().interactable = true;
                gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
            else
            {
                gameObject.GetComponent<CanvasGroup>().alpha = 0;
                gameObject.GetComponent<CanvasGroup>().interactable = false;
                gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
        }
    }

    public void ToggleKitchenware(bool visible)
    {
        ToggleGameObject(CurrentKitchenware, visible);
    }

    public void SaveSceneData()
    {
        csGameController.control.SaveInventoryManager(InventoryManagerCode.Ingredients, inventoryIngredientsManager);
        csGameController.control.SaveInventoryManager(InventoryManagerCode.Recipes, inventoryRecipeManager);
    }

    public void LoadSceneData()
    {
        List<csPrefabItem> ingredients;
        if(csGameController.control.TempInventoryManagers.TryGetValue(InventoryManagerCode.Ingredients, out ingredients))
        {
            foreach(var ingredient in ingredients)
            {
                var prefab = LoadItemInSelector(FoodSelectorUI, "Prefabs/Ingredients/" + ingredient.Name, foodRectTransform);
                if(prefab != null)
                {
                    prefab.GetComponent<csIngredient>().Amount = ingredient.Amount;
                    txtIngredientsAmount.text = ingredient.Amount.ToString();
                    foodInventory.Add(prefab);
                }
            }
            if (foodInventory.Count > 0)
            {
                var ingredient = foodInventory[invIdx] as GameObject;
                ingredient.SetActive(true);

                if (ingredient.GetComponent<CanvasGroup>() == null)
                    ingredient.AddComponent<CanvasGroup>();

                ingredient.GetComponent<CanvasGroup>().blocksRaycasts = true;
                ingredient.transform.SetParent(FoodSelectorUI.transform, false);
                noItemFood.gameObject.SetActive(false);
            }

            inventoryIngredientsManager.Inventory = foodInventory;
        }
    }
}
