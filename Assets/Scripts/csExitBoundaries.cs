using UnityEngine;
using System.Collections;

public class csExitBoundaries : MonoBehaviour
{
    [SerializeField]
    private CircleCollider2D projectile;
    private csProjectileDragging projectileDragging;

    void Awake()
    {
        projectileDragging = projectile.gameObject.GetComponent<csProjectileDragging>();
    }

    // Use this for initialization
    void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Projectile")
        {
            projectileDragging.Reset();
        }
    }
}
