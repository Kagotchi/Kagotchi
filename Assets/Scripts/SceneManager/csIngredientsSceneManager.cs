using UnityEngine;
using System.Collections;

public class csIngredientsSceneManager : MonoBehaviour, csISceneManager
{
    void Awake()
    {
        QualitySettings.vSyncCount = 0;  // VSync must be disabled
        Application.targetFrameRate = 30;
    }

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

    public void OnApplicationPause(bool paused)
    {

    }

    public void UpdateScenery(bool scroll, float direction)
    {
        throw new System.NotImplementedException();
    }

    public void SetNoItemVisibility(bool visible)
    {
        throw new System.NotImplementedException();
    }
}
