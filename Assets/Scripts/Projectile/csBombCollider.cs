using UnityEngine;
using System.Collections;

public class csBombCollider : MonoBehaviour
{
    Object explosion;

    // Use this for initialization
    void Start()
    {
        explosion = Resources.Load("Prefabs/Explosions/Explosion 2");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Kagotchi")
        {
            var clone = (GameObject)Instantiate(explosion);
            clone.transform.position = gameObject.transform.position;
            clone.name = explosion.name;
            clone.SetActive(true);
            var animator = clone.GetComponent<Animator>();
            animator.enabled = true;
            var explosionTime = animator.GetCurrentAnimatorStateInfo(0).length;
            Destroy(clone, explosionTime);
            Destroy(gameObject);
        }
    }
}
