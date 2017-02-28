using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Actor;

public class csDroppableRecipe : MonoBehaviour, IDropHandler
{

    private GameObject recipeUI;
    private GameObject ingredientsGrid;
    private GameObject checkIngredientsGrid;
    private GameObject kitchenwareGrid;
    private GameObject checkKitchenwareGrid;
    private GameObject recipeName;
    private GameObject kitchenwareWrapper;
    private GameObject boardIngredients;
    private GameObject kitchenwarePref;
    
    private GameObject btnKitchenware;
    private GameObject btnIngredients;
    private GameObject btnCooking;

    private CanvasGroup recipeUICanvas;

    private GameObject imgRecipe;
    private Object check;
    private Object notCheck;

    private csRecipe recipe;

    private Canvas canvas;
    private csKitchenSceneManager sceneManager;

    private csStep[] steps = null; 

    public Slider cookingTimeSlider;
    public Slider cookingStepsSlider;

    public Text txtCookingTime;
    public Text txtStepTime;
    public Text txtMaxSteps;
    public Text txtCurrentStep;
    public csKagotchi kagotchi;

    private csMessageManager message;

    private delegate bool ValidateRecipeItem(GameObject item);

    private bool IsMissingIngredients { get; set; }

    private bool IsMissingKitchenware { get; set; }

    private float MaxCooking { get; set; }

    private float CookingTime { get; set; }

    private int CookingSteps { get; set; }

    private float MaxStepTime { get; set; }

    private float StepTime { get; set; }

    private int CurrentStep { get; set; }

	// Use this for initialization
	void Start () 
    {
        ingredientsGrid = GameObject.Find("grdIngredients");
        checkIngredientsGrid = GameObject.Find("grdIngredientsCheck");
        kitchenwareGrid = GameObject.Find("grdKitchenware");
        checkKitchenwareGrid = GameObject.Find("grdKitchenwareCheck");
        kitchenwareWrapper = GameObject.Find("Kitchenware");
        boardIngredients = GameObject.Find("Board Ingredients");

        btnKitchenware = GameObject.Find("btnKitchenware");
        btnKitchenware.GetComponent<CanvasGroup>().alpha = 0;
        btnKitchenware.GetComponent<CanvasGroup>().interactable = false;
        btnKitchenware.GetComponent<CanvasGroup>().blocksRaycasts = false;

        btnIngredients = GameObject.Find("btnIngredients");
        btnIngredients.GetComponent<CanvasGroup>().alpha = 0;
        btnIngredients.GetComponent<CanvasGroup>().interactable = false;
        btnIngredients.GetComponent<CanvasGroup>().blocksRaycasts = false;

        btnCooking = GameObject.Find("btnCooking");
        btnCooking.GetComponent<CanvasGroup>().alpha = 0;
        btnCooking.GetComponent<CanvasGroup>().interactable = false;
        btnCooking.GetComponent<CanvasGroup>().blocksRaycasts = false;

        canvas = GameObject.FindObjectOfType<Canvas>();
        sceneManager = canvas.GetComponent<csKitchenSceneManager>();
        message = GameObject.FindObjectOfType<csMessageManager>();


        IsMissingIngredients = false;
        IsMissingKitchenware = false;

        MaxCooking = 100;
        MaxStepTime = 100;
        CurrentStep = 0;
	}
	
	// Update is called once per frame
	void Update () 
    {
	}

    private void LoadCheckNotCheckImage(GameObject grid, Object validationObject)
    {
        if (validationObject != null)
        {
            var gameObject = (GameObject)Instantiate(validationObject);
            gameObject.SetActive(true);
            gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            var image = gameObject.GetComponent<Image>();
            image.gameObject.AddComponent<CanvasGroup>();
            image.gameObject.GetComponent<CanvasGroup>().interactable = false;
            image.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
            image.transform.SetParent(grid.transform, false);
        }
    }

    private bool ValidateIngredient(GameObject ingredient)
    {
        var exists = sceneManager.FoodInventoryManager.Inventory.Cast<GameObject>().Any(x => x.name == ingredient.name);
        if (exists)
        {
            check = Resources.Load("UI/Check");
            LoadCheckNotCheckImage(checkIngredientsGrid, check);
        }
        else
        {
            notCheck = Resources.Load("UI/NotCheck");
            LoadCheckNotCheckImage(checkIngredientsGrid, notCheck);
            IsMissingIngredients = true;
        }
        return exists;
    }

    private bool ValidateKitchenware(GameObject kitchenware)
    {
        var exists = sceneManager.KitchenwareInventory.Cast<GameObject>().Any(x => x.name == kitchenware.name);
        if (exists)
        {
            check = Resources.Load("UI/Check");
            LoadCheckNotCheckImage(checkKitchenwareGrid, check);
        }
        else
        {
            notCheck = Resources.Load("UI/NotCheck");
            LoadCheckNotCheckImage(checkKitchenwareGrid, notCheck);
            IsMissingKitchenware = true;
        }
        return exists;
    }

    private void LoadRecipeItems(List<GameObject> items, string prefabPath, GameObject grid, ValidateRecipeItem Validate)
    {
        var i = 0;

        if (items.Count > 0)
        {
            foreach (var item in items)
            {
                var itemPref = Resources.Load(prefabPath + item.name);
                if (itemPref != null)
                {
                    var clone = (GameObject)Instantiate(itemPref);
                    clone.name = itemPref.name;
                    clone.SetActive(true);
                    clone.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    if(clone.GetComponent<csIngredient>() != null)
                    {
                        var step = new csStep()
                        {
                            Succeded = false,
                            Ingredient = clone
                        };
                        steps[i] = step;
                    }

                    if (clone.GetComponent<csKitchenware>() != null)
                    {
                        clone.GetComponent<Animator>().enabled = false;
                    }
                    
                    var image = clone.GetComponent<Image>();
                    var canvasGroup = image.gameObject.GetComponent<CanvasGroup>();
                    if (canvasGroup == null)
                        image.gameObject.AddComponent<CanvasGroup>();
                    image.gameObject.GetComponent<CanvasGroup>().interactable = false;
                    image.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
                    image.transform.SetParent(grid.transform, false);
                    
                }

                Validate(item);
                i++;
            }
        }
    }

    private void LoadKitchenWare()
    {
        var kitchenware = recipe.kitchenware;

        foreach(var item in kitchenware)
        {
            var clone = Instantiate(item);
            clone.name = item.name;
            kitchenwarePref = clone;
            clone.transform.SetParent(kitchenwareWrapper.transform, false);
            sceneManager.CurrentKitchenware = clone;
        }

    }

    private void ClearGrids()
    {
        if (ingredientsGrid.transform.childCount > 0)
        {
            for (var i = 0; i < ingredientsGrid.transform.childCount; i++)
            {
                Destroy(ingredientsGrid.transform.GetChild(i).gameObject);
            }
        }

        if (checkIngredientsGrid.transform.childCount > 0)
        {
            for (var i = 0; i < checkIngredientsGrid.transform.childCount; i++)
            {
                Destroy(checkIngredientsGrid.transform.GetChild(i).gameObject);
            }
        }

        if (kitchenwareGrid.transform.childCount > 0)
        {
            for (var i = 0; i < kitchenwareGrid.transform.childCount; i++)
            {
                Destroy(kitchenwareGrid.transform.GetChild(i).gameObject);
            }
        }

        if (checkKitchenwareGrid.transform.childCount > 0)
        {
            for (var i = 0; i < checkKitchenwareGrid.transform.childCount; i++)
            {
                Destroy(checkKitchenwareGrid.transform.GetChild(i).gameObject);
            }
        }

        if (kitchenwareWrapper.transform.childCount > 0)
        {
            for (var i = 0; i < kitchenwareWrapper.transform.childCount; i++)
            {
                Destroy(kitchenwareWrapper.transform.GetChild(i).gameObject);
            }
        }
        
    }

    public void OnDrop(PointerEventData eventData)
    {
        recipe = eventData.pointerDrag.gameObject.GetComponent<csRecipe>();

        if (recipe != null)
        {
            recipe.State = csRecipeStateEnum.None;

            sceneManager.CurrentRecipe = recipe;

            txtMaxSteps.text = recipe.Steps.ToString();
            txtCurrentStep.text = CurrentStep.ToString();

            steps = new csStep[recipe.Steps];

            recipeUI = GameObject.Find("RecipeUI");
            recipeUICanvas = recipeUI.GetComponent<CanvasGroup>();
            recipeUICanvas.alpha = 1;
            recipeUICanvas.blocksRaycasts = true;
            recipeUICanvas.interactable = true;

            imgRecipe = GameObject.Find("imgRecipe");
            imgRecipe.GetComponent<Image>().sprite = recipe.RecipeImage;

            recipeName = GameObject.Find("txtRecipeName");
            recipeName.GetComponent<Text>().text = recipe.RecipeName;

            ClearGrids();

            var ingredients = recipe.ingredients;
            LoadRecipeItems(ingredients, "Prefabs/Ingredients/", ingredientsGrid, ValidateIngredient);

            var kitchenware = recipe.kitchenware;
            LoadRecipeItems(kitchenware, "Prefabs/Kitchenware/", kitchenwareGrid, ValidateKitchenware);

            LoadKitchenWare();

            if(IsMissingKitchenware)
            {
                btnKitchenware.GetComponent<CanvasGroup>().alpha = 1;
                btnKitchenware.GetComponent<CanvasGroup>().interactable = true;
                btnKitchenware.GetComponent<CanvasGroup>().blocksRaycasts = true;
            }

            if(IsMissingIngredients)
            {
                btnIngredients.GetComponent<CanvasGroup>().alpha = 1;
                btnIngredients.GetComponent<CanvasGroup>().interactable = true;
                btnIngredients.GetComponent<CanvasGroup>().blocksRaycasts = true;
            }

            if (!IsMissingIngredients && !IsMissingKitchenware)
            {
                btnCooking.GetComponent<CanvasGroup>().alpha = 1;
                btnCooking.GetComponent<CanvasGroup>().interactable = true;
                btnCooking.GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
        }
    }

    public void StartCooking()
    {
        InvokeRepeating("CookingTimeManagment", 0, 1.0f);
        InvokeRepeating("CookingStepsManagment", 0, recipe.StepInterval);
        InvokeRepeating("CurrentStepManagment", 0, 1.0f);
        recipe.State = csRecipeStateEnum.InProgress;
    }

    private void CookingTimeManagment()
    {

        CookingTime += MaxCooking / sceneManager.CurrentRecipe.Time;

        if (CookingTime < 0)
            CookingTime = 0;

        if (CookingTime >= MaxCooking)
            CookingTime = MaxCooking;

        if (CookingTime >= MaxCooking && sceneManager.CurrentRecipe.State != csRecipeStateEnum.Finished)
        {
            recipe.State = csRecipeStateEnum.Finished;
            var msg = new csMessage()
            {
                Enable = true,
                Message = "YOU BURNED THE FOOD!",
                Status = csMessageStatusEnum.Visible,
                Timeout = 3.0f,
                Type = csMessageTypeEnum.Failure
            };
            message.SetUIMessage(msg);
            sceneManager.ToggleStopCookingButton(false);
            sceneManager.ShowRecipeSelector();
            Reset();
        }

        if (CookingTime >= 90 && sceneManager.CurrentRecipe.State != csRecipeStateEnum.Finished)
        {
            sceneManager.HideCuttingBoard();
            sceneManager.ToggleStopCookingButton(true);
        }

        txtCookingTime.text = Mathf.Round(CookingTime).ToString() + "%";
        cookingTimeSlider.value = CookingTime;
    }

    private void CookingStepsManagment()
    {
        if (CurrentStep < sceneManager.CurrentRecipe.Steps)
        {
            CurrentStep++;
            StepTime = 0;
            txtCurrentStep.text = CurrentStep.ToString();
            txtStepTime.text = "0%";
            cookingStepsSlider.value = 0;
            var step = steps[CurrentStep - 1];
            var clone = Instantiate(step.Ingredient);
            clone.name = step.Ingredient.name;
            clone.GetComponent<RectTransform>().sizeDelta = step.Ingredient.GetComponent<RectTransform>().sizeDelta;
            clone.transform.localScale = new Vector3(2, 2, 2);
            clone.GetComponent<CanvasGroup>().blocksRaycasts = true;
            clone.GetComponent<CanvasGroup>().interactable = true;
            clone.AddComponent<csDragHandler>();
            //Clear last ingredient
            if (boardIngredients.transform.childCount > 0)
            {
                for (var i = 0; i < boardIngredients.transform.childCount; i++)
                {
                    Destroy(boardIngredients.transform.GetChild(i).gameObject);
                }
            }

            clone.transform.SetParent(boardIngredients.transform, false);
            //center child in parent
            clone.transform.localPosition = Vector3.zero;
            EnableCurrKitchenwareAnimation(true);
        }
    }

    private void CurrentStepManagment()
    {
        if (CurrentStep <= sceneManager.CurrentRecipe.Steps)
        {
            StepTime += (MaxStepTime / sceneManager.CurrentRecipe.StepTime);

            if (StepTime >= MaxStepTime)
            {
                StepTime = MaxStepTime;
            }
            if (StepTime < 0)
                StepTime = 0;

            txtStepTime.text = StepTime.ToString() + "%";
            cookingStepsSlider.value += sceneManager.CurrentRecipe.StepTime;
        }
    }

    public void SetStepAsSucceded()
    {
        var step = steps[CurrentStep - 1];
        step.Succeded = true;

        var msg = new csMessage()
        {
            Enable = true,
            Message = "FASE " + CurrentStep + " COMPLETED!",
            Status = csMessageStatusEnum.Visible,
            Timeout = 3.0f,
            Type = csMessageTypeEnum.Success
        };
        message.SetUIMessage(msg);

        kagotchi.IncreaseSkill(csSkillEnum.Cooking);
    }

    

    public void EnableCurrKitchenwareAnimation(bool enable)
    {
        kitchenwarePref.GetComponent<Animator>().enabled = enable;
    }

    public void FinalizeRecipe()
    {
        var inventory = sceneManager.FoodInventoryManager.Inventory;
        var foodRectTransform = new csRectTransformData()
        {
            AnchorMin = new Vector2(1, 1),
            AnchorMax = new Vector2(1, 1),
            Pivot = new Vector2(0.5f, 0.5f),
            AnchoredPosition = new Vector2(-400, -89),
            SizeDelta = new Vector2(120, 114)
        };
        inventory.Add(sceneManager.LoadItemInSelector(sceneManager.FoodSelectorUI, recipe.FinalResult, foodRectTransform));

        var msg = new csMessage()
        {
            Enable = true,
            Message = "YOU CREATED THE RECIPE!!",
            Status = csMessageStatusEnum.None,
            Timeout = 3.0f,
            Type = csMessageTypeEnum.Success
        };
        message.SetUIMessage(msg);

        sceneManager.ToggleStopCookingButton(false);
        sceneManager.CurrentRecipe.State = csRecipeStateEnum.Finished;
        sceneManager.ShowRecipeSelector();

        Reset();
    }

    private void Reset()
    {
        CancelInvoke("CookingTimeManagment");
        CancelInvoke("CookingStepsManagment");
        CancelInvoke("CurrentStepManagment");
        CurrentStep = 0;
        txtCurrentStep.text = CurrentStep.ToString();
        txtStepTime.text = "0%";
        cookingStepsSlider.value = 0;
        Destroy(kitchenwarePref);
        txtCookingTime.text = "0%";
        cookingTimeSlider.value = 0;
        txtMaxSteps.text = "0";
        CookingTime = 0;
    }
}
