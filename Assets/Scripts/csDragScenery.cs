using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class csDragScenery : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 startPosition;
    private float direction;
    private bool scrollScenery = false;
    private csISceneManager sceneManager;
    private Canvas canvas;

	// Use this for initialization
	void Start () 
    {
        startPosition = new Vector3();
        canvas = GameObject.FindObjectOfType<Canvas>();
        sceneManager = canvas.GetComponent<csISceneManager>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        sceneManager.UpdateScenery(scrollScenery, direction);    
	}

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = Input.mousePosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        direction = Mathf.Sign(Input.mousePosition.x - startPosition.x);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        scrollScenery = true;
    }
}
