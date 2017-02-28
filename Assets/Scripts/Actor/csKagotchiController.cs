using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class csKagotchiController : MonoBehaviour {

    private Rigidbody2D rigidBody;
    public float upForce;
    private GameObject kagotchi;
    private GameObject powerEmitter;
    private Object power;
    private Animator animator;
    private Canvas canvas;

    [SerializeField]
    private float rateOfFire;

    private bool shoot;


    public int JumpsLeft{get; set;}

    public bool OnGround { get; set; }

    // Use this for initialization
    void Start()
    {
        kagotchi = GameObject.Find("Kagotchi");
        powerEmitter = GameObject.Find("Power Emitter");
        power = Resources.Load("Prefabs/Kagotchi/Power");
        canvas = GameObject.FindObjectOfType<Canvas>();
        rigidBody = kagotchi.GetComponent<Rigidbody2D>();
        animator = kagotchi.GetComponent<Animator>();
        animator.SetBool("isRunning", true);
        JumpsLeft = 2;

        if (Application.platform == RuntimePlatform.Android)
            upForce *= 1.25f;

    }

    // Update is called once per frame
    void Update()
    {
        if(shoot)
        {
            animator.SetTrigger("Shoot");
            GameObject bullet = (GameObject)Instantiate(power);
            bullet.transform.SetParent(canvas.transform, false);
            bullet.transform.position = powerEmitter.transform.position;
            bullet.SetActive(true);
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(200.0f,0);
            shoot = false;
        }
        
        animator.SetFloat("yVelocity", Mathf.Abs(rigidBody.velocity.y));
        
    }

    public void OnClickJump()
    {
        if (JumpsLeft > 0)
        {
            if (rigidBody.velocity.y < 0)
            {
                rigidBody.velocity = Vector2.zero;
            }

            if (JumpsLeft == 1)
                rigidBody.AddForce(transform.up * upForce * 0.75f, ForceMode2D.Force);
            else
                rigidBody.AddForce(transform.up * upForce, ForceMode2D.Force);

            JumpsLeft--;
        }
    }

    public void OnClickFire()
    {
        if (!shoot)
            shoot = true;
    }
}
