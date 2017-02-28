using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class csIngredient : MonoBehaviour, csIInventoryItem, csIRecipeItem {


    public float healthModifier;
    public float energyModifier;
    public float foodModifier = 10;
    public float happinessModifier;
    public int amount;
    [SerializeField]
    private string ingredientName;


    public Vector3 StartPosition { get; set; }
    public Quaternion StartRotation { get; set; }

	// Use this for initialization
	void Start () {
        StartPosition = gameObject.transform.position;
        StartRotation = gameObject.transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public int AmountMax 
    {
        get
        {
            return amount;
        }
        set
        {
            amount = value;
        }
    }

    public int Amount { get; set; }

    public string Name 
    {
        get
        {
            return ingredientName;
        } 
        set
        {
            ingredientName = value;
        }
    }
}
