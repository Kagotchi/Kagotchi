using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class csKagotchiCollider : MonoBehaviour {

    csKagotchiController controller;
    private Canvas canvas;
    private csKitchenSceneManager sceneManager;

	// Use this for initialization
	void Start () 
    {
       controller = GameObject.FindObjectOfType<csKagotchiController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            SceneManager.LoadScene("Kitchen");
        }
        else if (other.gameObject.tag == "Ground")
        {
            controller.JumpsLeft = 2;
            controller.OnGround = true;
        }
            
    }
}
