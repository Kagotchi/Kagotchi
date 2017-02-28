using UnityEngine;
using System.Collections;

public class csMoveLeft : MonoBehaviour {

    public float speed = 200.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.position += Vector3.left * speed  * Time.deltaTime;
	}
}
