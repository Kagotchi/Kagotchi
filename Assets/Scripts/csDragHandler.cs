using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class csDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public static GameObject targetItem;
    private Vector3 startPosition;
    public Transform startParent;

    public void OnBeginDrag(PointerEventData eventData)
    {
        targetItem = gameObject;
        startPosition = transform.position;
        //startParent = transform.parent;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        targetItem = null;
        if(startParent != transform.parent)
            transform.position = startPosition;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}