using UnityEngine;
using System.Collections;

public class csProjectileCollider : MonoBehaviour 
{
    Object explosion;
    Canvas canvas;

	// Use this for initialization
	void Start () {
        explosion = Resources.Load("Prefabs/Farm/Explosion");
        canvas = GameObject.FindObjectOfType<Canvas>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter2D(Collision2D other)
    {
        var lootList = other.gameObject.GetComponent<csSpawn>().GetRandomLoot();
        var cratePosition = other.gameObject.transform.position;
        Destroy(gameObject);
        if (other.gameObject.tag == "Destructible")
        {
            var clone = (GameObject)Instantiate(explosion);
            clone.transform.SetParent(canvas.transform);
            clone.transform.position = other.gameObject.transform.position;
            clone.name = explosion.name;
            clone.SetActive(true);
            Destroy(other.gameObject);
            var animator = clone.GetComponent<Animator>();
            animator.enabled = true;
            var explosionTime = animator.GetCurrentAnimatorStateInfo(0).length;
            Destroy(clone, explosionTime);
            foreach (var loot in lootList)
            {
                loot.transform.SetParent(canvas.transform, false);
                loot.GetComponent<RectTransform>().localScale = new Vector3(0.25f, 0.25f, 1);
                loot.transform.position = cratePosition;
                loot.AddComponent<csMoveLeft>();
                loot.AddComponent<csLoot>();
            }
        }
    }
}
