using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class csFightListButtons : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnClickFight()
    {
        var btnData = GetComponent<csButtonData>();
        if (btnData != null)
        {
            csGameController.control.CurrentBot = (csBot)btnData.Data;
            SceneManager.LoadScene("Fight Resume");
        }
        
    }
}
