using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class csPrefabSpawner : MonoBehaviour {

    public List<GameObject> prefabList;
    public float jitter = 0.25f;
    public AnimationCurve spawnCurve;
    public float curveLengthInSeconds = 30;

    private GameObject farmObject;
    private float nextSpawn = 0.0f;
    private float startTime = 0.0f;

	// Use this for initialization
	void Start () 
    {
        farmObject = GameObject.Find("FarmObstacles");

        if (Application.platform == RuntimePlatform.Android)
            curveLengthInSeconds *= 1.25f;

        
	}

    private GameObject GetRandomPrefab()
    {
        int startIdx = 0;
        int endIdx = prefabList.Count;
        int num = 0;

        var idx = Random.Range(startIdx, endIdx);
        var mainPrefab = (GameObject)Instantiate(prefabList[idx]);
        var grid = mainPrefab.GetComponent<GridLayoutGroup>();
        var spawn = mainPrefab.GetComponent<csSpawn>();
        if(grid != null)
        {
            var lootList = spawn.GetRandomLoot();
            foreach( var loot in lootList)
            {
                var csloot = loot.GetComponent<csLoot>();
                if (csloot != null)
                {
                    num = Random.Range(0, 101);

                    if (num <= csloot.Probability)
                        loot.transform.SetParent(grid.transform, false);
                    else
                        Destroy(loot);
                }
            }

        }
        return mainPrefab;
    }

	void FixedUpdate() 
    {
        float height = 0.0f;
	    if(Time.time > nextSpawn)
        {
            var obj = GetRandomPrefab();

            var hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), -Vector2.up, Mathf.Infinity);
            if (hit)
            {
                obj.transform.SetParent(farmObject.transform, false);

                if (obj.GetComponent<csSpawn>().IsGrounded)
                {
                    if (obj.GetComponent<Collider2D>() != null)
                        height = obj.GetComponent<Collider2D>().bounds.extents.y;

                    var posY = hit.point.y + height;
                    obj.transform.position = new Vector3(transform.position.x, posY, transform.position.z);
                }

                float curvepos = (Time.time - startTime) / curveLengthInSeconds;

                if (curvepos > 1.0f)
                {
                    curvepos = 1.0f;
                    startTime = Time.time;
                }

                nextSpawn = Time.time + spawnCurve.Evaluate(curvepos) + Random.Range(-jitter, jitter);
            }

            
        }
	}
}
