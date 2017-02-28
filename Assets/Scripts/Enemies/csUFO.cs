using UnityEngine;
using System.Collections;

public class csUFO : MonoBehaviour
{
    private float timer;
    private float speed;
    private Vector2 initialPosition;

    [SerializeField]
    private GameObject target;

    private GameObject bomb = null;
    private int ammo = 0;
    Sprite rockSprite;

    // Use this for initialization
    void Start ()
    {
        speed = 15;
        initialPosition = transform.position.normalized;
        rockSprite = GetComponent<SpriteRenderer>().sprite;
    }
	
	// Update is called once per frame
	void Update ()
    {
        var direction = target.transform.position - transform.position;
        direction.Normalize();
        direction.Set(direction.x,  Vector2.down.y * 100, direction.z);
        Movement(direction);
        Vector3 pos = Vector3.zero;
        pos.Set(transform.position.x, transform.position.y - rockSprite.bounds.max.y * 1.1f, transform.position.z);

        var hit = Physics2D.Raycast(pos, direction, Mathf.Infinity);
        if(hit)
        {
            Debug.DrawRay(pos, direction, Color.red);
            if (hit.rigidbody != null && hit.rigidbody.tag == "Kagotchi" && ammo < 1)
            {
                var itemObject = Resources.Load("Prefabs/Projectile/Bomb");
                if (itemObject != null)
                {
                    bomb = (GameObject)Instantiate(itemObject);
                    bomb.name = itemObject.name;
                    bomb.transform.position = pos;
                    ammo++;
                }
            }
        }
	}

    private bool Timer(float timeLimit)
    {
        timer = -Time.deltaTime;
        if (timer < timeLimit)
            return true;

        return false;
    }
    void Movement(Vector3 direction)
    {
        float factor = Time.deltaTime * speed;

        this.transform.Translate(direction.x * factor, 0, 0, Space.World);
    }
}
