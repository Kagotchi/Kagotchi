using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class csHouseButtons : MonoBehaviour {

	public void OnClickKitchen()
    {
        SceneManager.LoadScene("Kitchen");
    }

    public void OnClickBedroom()
    {
        SceneManager.LoadScene("Bedroom");
    }

    public void OnClickOutside()
    {
        SceneManager.LoadScene("Outside");
    }
}
