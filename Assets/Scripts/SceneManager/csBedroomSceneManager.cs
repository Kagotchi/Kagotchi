using UnityEngine;
using System.Collections;

public class csBedroomSceneManager : MonoBehaviour, csISceneManager 
{
    [SerializeField]
    private GameObject pnlSleepMsg;
    private CanvasGroup sleepMsgCanvas;
    private RectTransform sceneryRectTransform;
    private RectTransform kagotchyRectTransform;

    private float sceneryLeftLimitX = 931.0f;
    private float sceneryRightLimitX = -291.0f;
    private float speed = 1000.0f;
    private Vector2 kagotchiStartPosition;

    private bool isScrolling = false;
    private bool reachedRightLimit = false;
    private bool reachedLeftLimit = false;

    private GameObject scenery;
    private GameObject kagotchi;
	// Use this for initialization
	void Start () 
    {
        pnlSleepMsg = GameObject.Find("pnlSleepMsg");
        sleepMsgCanvas = pnlSleepMsg.GetComponent<CanvasGroup>();
        sleepMsgCanvas.alpha = 0;
        sleepMsgCanvas.interactable = false;
        sleepMsgCanvas.blocksRaycasts = false;

        scenery = GameObject.Find("Bedroom");
        kagotchi = GameObject.Find("Kagotchi");

        sceneryRectTransform = scenery.GetComponent<RectTransform>();
        kagotchyRectTransform = kagotchi.GetComponent<RectTransform>();

        kagotchiStartPosition = kagotchyRectTransform.anchoredPosition;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ShowSleepMsg(bool show)
    {
        if(show)
        {
            sleepMsgCanvas.alpha = 1;
            sleepMsgCanvas.interactable = true;
            sleepMsgCanvas.blocksRaycasts = true;
        }
        else
        {
            sleepMsgCanvas.alpha = 0;
            sleepMsgCanvas.interactable = false;
            sleepMsgCanvas.blocksRaycasts = false;
        }
    }

    public void OnApplicationPause(bool paused)
    {
        throw new System.NotImplementedException();
    }

    public void UpdateScenery(bool scroll, float direction)
    {
        if (scroll)
            isScrolling = true;


        if (direction < 0 && isScrolling == true)
        {
            sceneryRectTransform.anchoredPosition += Vector2.left * speed * Time.deltaTime;
            if (sceneryRectTransform.anchoredPosition.x < sceneryRightLimitX)
            {
                sceneryRectTransform.anchoredPosition = new Vector2(sceneryRightLimitX, sceneryRectTransform.anchoredPosition.y);
                isScrolling = false;
            }

        }
        else if (direction > 0 && isScrolling == true)
        {
            sceneryRectTransform.anchoredPosition += Vector2.right * speed * Time.deltaTime;
            if (sceneryRectTransform.anchoredPosition.x > sceneryLeftLimitX)
            {
                sceneryRectTransform.anchoredPosition = new Vector2(sceneryLeftLimitX, sceneryRectTransform.anchoredPosition.y);
                isScrolling = false;
            }
        }

        if (sceneryRectTransform.anchoredPosition.x == sceneryRightLimitX && !reachedRightLimit)
        {

            kagotchyRectTransform.anchoredPosition += new Vector2(-160, 0);
            reachedRightLimit = true;
            reachedLeftLimit = false;

            kagotchi.GetComponent<csKagotchi>().SetValues();
        }

        if (sceneryRectTransform.anchoredPosition.x == sceneryLeftLimitX && !reachedLeftLimit)
        {

            kagotchyRectTransform.anchoredPosition = kagotchiStartPosition;
            reachedRightLimit = false;
            reachedLeftLimit = true;



            kagotchi.GetComponent<csKagotchi>().LoadValues();

        }
    }

    public void SetNoItemVisibility(bool visible)
    {
        throw new System.NotImplementedException();
    }
}
