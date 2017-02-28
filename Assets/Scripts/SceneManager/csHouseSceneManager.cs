using UnityEngine;
using System.Collections;

public class csHouseSceneManager : MonoBehaviour, csISceneManager 
{

    private GameObject kagotchiObj;
	// Use this for initialization
	void Start () 
    {
        kagotchiObj = GameObject.Find("Kagotchi");
        var kagotchi = kagotchiObj.GetComponent<csKagotchi>();
        kagotchi.Init();
        kagotchi.SetValues();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnApplicationPause(bool paused)
    {
    }

    public void UpdateScenery(bool scroll, float direction)
    {
    }

    public void SetNoItemVisibility(bool visible)
    {
    }
}
