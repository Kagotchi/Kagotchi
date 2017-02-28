using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class csDroppableFood : MonoBehaviour, IDropHandler
{
    public csKagotchi kagotchi;
    public Text txtIngredientsAmount;

    private Canvas canvas;
    private csKitchenSceneManager sceneManager;
    private GameObject foodSelectorUI;

	// Use this for initialization
	void Start () 
    {
        canvas = GameObject.FindObjectOfType<Canvas>();
        sceneManager = canvas.GetComponent<csKitchenSceneManager>();
        foodSelectorUI = GameObject.Find("FoodSelectorUI");
	}
	
    public void OnDrop(PointerEventData eventData)
    {
        var food = eventData.pointerDrag.gameObject.GetComponent<csIngredient>();
        if (food != null)
        {
            if (food.healthModifier != 0)
            {
                kagotchi.Health += food.healthModifier;
                kagotchi.healthSlider.value += food.healthModifier;
            }

            if (food.energyModifier != 0)
            {
                kagotchi.Energy += food.energyModifier;
                kagotchi.energySlider.value += food.energyModifier;
            }

            if (food.foodModifier != 0)
            {
                kagotchi.Food += food.foodModifier;
                kagotchi.foodSlider.value += food.foodModifier;
            }

            if (food.happinessModifier != 0)
            {
                kagotchi.Happiness += food.happinessModifier;
                kagotchi.happinessSlider.value += food.happinessModifier;
            }

            food.Amount--;
            txtIngredientsAmount.text = food.Amount.ToString();

            if (food.Amount > 0)
            {
                food.transform.position = food.StartPosition;
            }
            else
            {
                food.transform.position = food.StartPosition;
                food.gameObject.SetActive(false);
                sceneManager.RemoveFoodItem(food.gameObject);
                var newItem = sceneManager.GetNextActiveFoodItem();
                if (newItem != null)
                {
                    newItem.transform.SetParent(foodSelectorUI.transform, false);
                    newItem.GetComponent<CanvasGroup>().blocksRaycasts = true;
                    newItem.GetComponent<CanvasGroup>().interactable = true;
                    newItem.SetActive(true);
                }
                    
                else
                {
                    sceneManager.SetNoItemVisibility(true);
                }
            }
        }
    }
}
