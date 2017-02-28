using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class csDroppableKitchenware : MonoBehaviour, IDropHandler
{
    private csIngredient food;
    private csDroppableRecipe droppableRecipe;
	// Use this for initialization
	void Start () {
        droppableRecipe = GameObject.FindObjectOfType<csDroppableRecipe>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnDrop(PointerEventData eventData)
    {
        food = eventData.pointerDrag.gameObject.GetComponent<csIngredient>();
        if(food != null)
        {
            droppableRecipe.SetStepAsSucceded();
            droppableRecipe.EnableCurrKitchenwareAnimation(false);
            Destroy(eventData.pointerDrag.gameObject);
        }
    }
}
