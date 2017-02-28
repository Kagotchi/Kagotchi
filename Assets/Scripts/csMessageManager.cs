using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class csMessageManager : MonoBehaviour 
{
    public GameObject messageUI;
    private CanvasGroup messageUICanvas;
    private ArrayList messageQueue = new ArrayList();
    private csMessage msg;

	// Use this for initialization
	void Start () 
    {
        messageUICanvas = messageUI.GetComponent<CanvasGroup>();
        messageUICanvas.alpha = 0;
	}

    
	// Update is called once per frame
	void Update () 
    {
        if (messageQueue.Count > 0)
        {
            msg = (csMessage)messageQueue[0];

            if (msg.Status == csMessageStatusEnum.Visible)
                StartCoroutine(Message(msg));

            if (msg.Status == csMessageStatusEnum.Hidden && messageUICanvas.alpha > 0)
                messageUICanvas.alpha -= Time.deltaTime;

            if (msg.Status == csMessageStatusEnum.Hidden && messageUICanvas.alpha <= 0)
            {
                messageQueue.RemoveAt(0);
            }

            if (msg.Status == csMessageStatusEnum.Visible && messageUICanvas.alpha <= 1)
                messageUICanvas.alpha += Time.deltaTime;
        }
	}

    private IEnumerator Message(csMessage message)
    {
        float r = 0;
        float g = 0;
        float b = 0;

        if (enabled)
        {
            var messageTxt = messageUI.GetComponentInChildren<Text>();
            messageTxt.text = message.Message;

            if (message.Type == csMessageTypeEnum.Success)
            {
                r = 21.0f / 255.0f;
                g = 182.0f / 255.0f;
                b = 42.0f / 255.0f;
            }
            else if (message.Type == csMessageTypeEnum.Failure)
            {
                r = 255.0f / 255.0f;
                g = 0;
                b = 0;
            }

            messageTxt.color = new Color(r, g, b);

            if (message.Timeout > 0.0f)
            {
                yield return new WaitForSeconds(message.Timeout);
                if (message.Status == csMessageStatusEnum.Visible)
                {
                    message.Status = csMessageStatusEnum.Hidden;
                }
            }
        }
        else
        {
            messageUICanvas.alpha = 0;
        }
        messageUICanvas.interactable = false;
        messageUICanvas.blocksRaycasts = false;
    }

    public void SetUIMessage(csMessage message)
    {
        if(message.Enable)
            message.Status = csMessageStatusEnum.Visible;
        
        messageQueue.Add(message);
    }
}
