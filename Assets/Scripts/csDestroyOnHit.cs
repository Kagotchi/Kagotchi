using UnityEngine;
using System.Collections;

public class csDestroyOnHit : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other)
    {
        var spawn = other.gameObject.GetComponent<csSpawn>();
        if (spawn)
            spawn.FinalLoot.Clear();

        Destroy(other.gameObject);
    }
}
