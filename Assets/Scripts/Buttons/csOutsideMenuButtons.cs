using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class csOutsideMenuButtons : MonoBehaviour {

    public void OnClickBack()
    {
        SceneManager.LoadScene("House");
    }

    public void OnClickFight()
    {
        SceneManager.LoadScene("Fight List");
    }
}
