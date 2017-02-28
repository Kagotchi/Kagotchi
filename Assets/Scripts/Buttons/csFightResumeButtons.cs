using UnityEngine;
using System.Collections;

public class csFightResumeButtons : MonoBehaviour {

    private Canvas canvas;
    private csFightResumeSceneManager sceneManager;
	// Use this for initialization
	void Start () 
    {
        canvas = GameObject.FindObjectOfType<Canvas>();
        sceneManager = canvas.GetComponent<csFightResumeSceneManager>();
	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}

    public void OnClickShowFightDetails()
    {
        sceneManager.ShowCombatDetails();
    }

    public void OnClickHideFightDetails()
    {
        sceneManager.HideCombatDetails();
    }
}
