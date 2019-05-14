using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ai : MonoBehaviour {

    //components
    private Animator anim;
    private Rigidbody2D rb2d;
    private AudioSource sound;

    //stats
    public int health = 100;

    //for movement 
    public float speed;
    public float jumpHeight = 5;

    //checkers
    private bool canWalk = true;
    private bool jumping = false;
    private bool facingLeft = true;


    //sounds
    public AudioClip taunt1Sound;
    public AudioClip taunt2Sound;
    public AudioClip taunt3Sound;
    public AudioClip punchSound;
    public AudioClip kickSound;
    public AudioClip pokeSound;
    public AudioClip slashSound;
    public AudioClip jumpSound;
    public AudioClip landingSound;

    //postions
    private Transform me;
    private Transform player;

    // Use this for initialization
    void Start () {

        me = this.transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sound = GetComponent<AudioSource>();

        
    }


    //check collison
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            rb2d.isKinematic = true;
            rb2d.velocity = rb2d.velocity * 0;
        }
    }

    //check that collision has exited
    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            rb2d.isKinematic = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate () {

        //Debug.Log(Distance().ToString());

        checkDirection();

        //flip sprite depending on direction facing
        if (facingLeft)
            this.transform.localRotation = Quaternion.Euler(0, 180, 0);
        else
            this.transform.localRotation = Quaternion.Euler(0, 0, 0);

        //check health and determine whether to play defensive or offensive
        if(health > 30)
        {
            offensive();
        }
        else
        {
            defensive();
        }

    }

    //check if enemy should face left or right
    void checkDirection()
    {
        if(player.position.x < me.position.x)
        {
            facingLeft = true;
        }
        else
        {
            facingLeft = false;
        }

    }

    //return distance between player and self
    private float Distance()
    {
        return Vector2.Distance(me.position, player.position);
    }

    //play offensive
    void offensive()
    {
        //check distance to start walking towards player
        if(Distance() > 3.5 && player.position.y <= me.position.y)
        {
            anim.SetBool("isWalking", true);
            walk();
        }
        else
        {
            anim.SetBool("isWalking", false);
            rb2d.velocity = Vector2.zero; 

        }
    }

    //play defensive
    void defensive()
    {

    }

    //walk towards player
    void walk()
    {
        if(facingLeft)
        {
            rb2d.velocity = new Vector2(-speed, rb2d.velocity.y);
        }
        else
        {
            rb2d.velocity = new Vector2(speed, rb2d.velocity.y);
        }

    }

}
