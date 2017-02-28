using UnityEngine;
using System.Collections;

public class csRecipeMenuButtons : MonoBehaviour 
{

    private Canvas canvas;
    private csRecipeMenuSceneManager sceneManager;
	// Use this for initialization
	void Start () 
    {
        canvas = GameObject.FindObjectOfType<Canvas>();
        sceneManager = canvas.GetComponent<csRecipeMenuSceneManager>();
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public void OnClickNextRecipe()
    {
        sceneManager.NextRecipe();
    }

    public void OnClickPreviousRecipe()
    {
        sceneManager.PreviousRecipe();
    }
}
