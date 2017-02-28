using UnityEngine;
using System.Collections;

public class csInvisibleObject : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.AddComponent<CanvasGroup>().alpha = 0;
    }
}
