using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class csBotProfileList : MonoBehaviour {

    [SerializeField]
    private Text txtLevel;
    [SerializeField]
    private Text txtName;
    [SerializeField]
    private Image avatar;

    public int Level { get; set; }
    public string Name { get; set; }
    public Image Avatar { get; set; }

	// Use this for initialization
	void Awake () 
    {
        Avatar = avatar;
	}

    void Start()
    {
        txtLevel.text = Level.ToString();
        txtName.text = Name;
    }
}
