using UnityEngine;
using System.Collections;

public class csKitchenware : MonoBehaviour, csIRecipeItem {

    [SerializeField]
    private string kitchenwareName;
    [SerializeField]
    private int amount;

	// Use this for initialization
	void Start () 
    {
        Amount = amount;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public string Name 
    {
        get
        {
            return kitchenwareName;
        }
        set 
        {
            kitchenwareName = value;
        } 
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

}
