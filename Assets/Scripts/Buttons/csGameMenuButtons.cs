using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class csGameMenuButtons : MonoBehaviour {

    private Canvas canvas;
    private csKitchenSceneManager sceneManager;
    private GameObject kagotchi;

    void Start()
    {
        canvas = GameObject.FindObjectOfType<Canvas>();
        sceneManager = canvas.GetComponent<csKitchenSceneManager>();
        kagotchi = GameObject.Find("Kagotchi");
    }

    public void OnClickHouse()
    {
        kagotchi.GetComponent<csKagotchi>().SetValues();
        sceneManager.SaveSceneData();
        SceneManager.LoadScene("House");
    }
}
