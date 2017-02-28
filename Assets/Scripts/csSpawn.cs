using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class csSpawn : MonoBehaviour 
{
    [SerializeField]
    private List<GameObject> loot;

    [SerializeField]
    private int lootAmount;

    [SerializeField]
    private float spawnProbability;

    [SerializeField]
    private bool lootAfterDestroyed;

    [SerializeField]
    private bool grounded;

    [SerializeField]
    private GameObject parent;

    [SerializeField]
    private List<GameObject> children;

    [SerializeField]
    private float velocity;

    [SerializeField]
    private float durability;

    private GameObject parentUI;

    private List<GameObject> finalLoot = new List<GameObject>();

    public GameObject SpawnObject
    {
        get { return gameObject; }
    }
    
    public float SpawnProbability 
    { 
        get { return spawnProbability; }
        set { spawnProbability = value; }
    }

    public List<GameObject> Loot
    {
        get { return loot; }
        set { loot = value; }
    }

    public List<GameObject> FinalLoot
    {
        get { return finalLoot; }
        set { finalLoot = value; }
    }

    public bool IsGrounded
    {
        get { return grounded; }
        set { grounded = value; }
    }

    public GameObject Parent
    {
        get { return parent; }
        set { parent = value; }
    }

    public float Velocity
    {
        get { return velocity; }
        set { velocity = value; }
    }

	// Use this for initialization
	void Start () 
    {
        parentUI = GameObject.Find("FarmObstacles");

        if (children.Count > 0)
        {
            foreach(var child in children)
            {
                var prefab = (GameObject)Instantiate(child);
                prefab.transform.SetParent(parentUI.transform, false);
                prefab.GetComponent<csSpawn>().Parent = gameObject;
                prefab.SetActive(true);
                var collider = GetComponent<Collider2D>();
                var childCollider = prefab.GetComponent<Collider2D>();
                var posY = transform.position.y + collider.bounds.extents.y + childCollider.bounds.extents.y;
                prefab.transform.position = new Vector3(transform.position.x, posY, transform.position.z);
            }
        }
	}

	// Update is called once per frame
	void Update () 
    {
        if(parent == null)
            transform.position += Vector3.left * velocity * Time.deltaTime;
        else
        {
            transform.position += Vector3.left * parent.GetComponent<csSpawn>().Velocity * Time.deltaTime;
        }
            
	}

    public List<GameObject> GetRandomLoot()
    {
        for(var i = 0; i < lootAmount; i++)
        {
            int startIdx = 0;
            int endIdx = loot.Count;

            var idx = Random.Range(startIdx, endIdx);
            var clone = (GameObject)Instantiate(loot[idx]);
            clone.name = loot[idx].name;
            finalLoot.Add(clone);
        }

        return finalLoot;
    }
}
