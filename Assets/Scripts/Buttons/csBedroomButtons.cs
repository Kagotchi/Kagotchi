using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class csBedroomButtons : MonoBehaviour 
{
    private Canvas canvas;
    private csBedroomSceneManager sceneManager;
    private GameObject kagotchi;
    private bool on;
	// Use this for initialization
	void Start () 
    {
        canvas = GameObject.FindObjectOfType<Canvas>();
        sceneManager = canvas.GetComponent<csBedroomSceneManager>();
        kagotchi = GameObject.Find("Kagotchi");
        on = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnClickLamp()
    {
        if(on)
        {
            if (kagotchi.GetComponent<csKagotchi>().Energy > 20.0f)
                sceneManager.ShowSleepMsg(true);
            else
            {
                csGameController.control.Kagotchi.IsAwake = false;
                Camera.main.backgroundColor = Color.black;
            }
        }
        else
        {
            Camera.main.backgroundColor = Color.white;
            kagotchi.GetComponent<csKagotchi>().IsAwake = true;
        }
        on = !on;
    }

    public void OnClickOkSleepMsg()
    {
        kagotchi.GetComponent<csKagotchi>().IsAwake = false;
        sceneManager.ShowSleepMsg(false);
        Camera.main.backgroundColor = Color.black;
    }

    public void OnClickCancelSleepMsg()
    {
        sceneManager.ShowSleepMsg(false);
    }

    public void OnClickSewingMachine()
    {
        SceneManager.LoadScene("RecipesMenu");
    }
}
