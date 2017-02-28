using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class csFightPlanButtons : MonoBehaviour {

    private Canvas canvas;
    private csFightPlanSceneManager sceneManager;
	// Use this for initialization
	void Start () 
    {
        canvas = GameObject.FindObjectOfType<Canvas>();
        sceneManager = canvas.GetComponent<csFightPlanSceneManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnNewTurnClick()
    {
        sceneManager.CreateNewTurn();
    }

    public void OnDeleteClick()
    {
        var button = EventSystem.current.currentSelectedGameObject;
        sceneManager.DeleteTurn(button);
        Destroy(button.transform.parent.gameObject);
    }

    public void OnBackClick()
    {
        SceneManager.LoadScene("Fight List");
    }

    public void OnFightClick()
    {
        sceneManager.Fight();
        SceneManager.LoadScene("Fight Resume");
    }
}
