using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class csRecipe : MonoBehaviour, csIInventoryItem
{
    public int amount;
    public List<GameObject> ingredients = new List<GameObject>();
    public List<GameObject> kitchenware = new List<GameObject>();
    public float time;
    public int servings;
    public Sprite recipeImage;
    public string recipeName;
    public float stepTime;
    public GameObject finalResul;

    public int Amount
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

    public int Steps { get; set; }

    public float Time
    {
        get
        {
            return time;
        }
        set
        {
            time = value;
        }
    }

    public float StepTime
    {
        get
        {
            return stepTime;
        }
        set
        {
            stepTime = value;
        }
    }

    public int StepInterval { get; set; }


    public Sprite RecipeImage 
    {
        get
        {
            return recipeImage;
        }
        set
        {
            recipeImage = value;
        }
    }

    public string RecipeName
    {
        get
        {
            return recipeName;
        }
        set
        {
            recipeName = value;
        }
    }

    public GameObject FinalResult
    {
        get
        {
            return finalResul;
        }
        set
        {
            finalResul = value;
        }
    }

    public csRecipeStateEnum State { get; set; }

	// Use this for initialization
	void Start () 
    {
        Steps = ingredients.Count;
        StepInterval = (int)Mathf.Round((Time - 10) / Steps);

        if (StepTime > StepInterval)
            StepTime -= 1;
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
