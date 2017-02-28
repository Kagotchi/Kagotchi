using UnityEngine;
using System.Collections;

public class csArmorSceneManager : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        Screen.orientation = ScreenOrientation.Landscape;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
}
