using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class csStartMenuButtons : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void NewClicked()
    {
        SceneManager.LoadScene("House");
    }
}
