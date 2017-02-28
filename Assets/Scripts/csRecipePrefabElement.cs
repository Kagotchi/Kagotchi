using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class csRecipePrefabElement : MonoBehaviour {

    [SerializeField]
    private Text txtName;
    [SerializeField]
    private Text txtAmount;
    [SerializeField]
    private Image image;
    [SerializeField]
    private Image imageAvail;
    [SerializeField]
    private Image imageNotAvail;

    private CanvasGroup AvailCanvas;
    private CanvasGroup NotAvailCanvas;

    public string Name { get; set; }
    public string Amount { get; set; }
    public string AmountMax { get; set; }
    public Image Image { get; set; }
    public bool Available { get; set; }

    void Awake()
    {
    }

	// Use this for initialization
	void Start () 
    {
        image.sprite = Image.sprite;
        txtName.text = Name;
        txtAmount.text = Amount +"/" + AmountMax;

        AvailCanvas = imageAvail.GetComponent<CanvasGroup>();
        NotAvailCanvas = imageNotAvail.GetComponent<CanvasGroup>();

        if(Available)
        {
            AvailCanvas.alpha = 1;
            NotAvailCanvas.alpha = 0;
        }
        else
        {
            AvailCanvas.alpha = 0;
            NotAvailCanvas.alpha = 1;
        }
	}
	

}
