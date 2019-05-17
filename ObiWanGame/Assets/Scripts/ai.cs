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
    private bool attacking = false;


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

        //check that ai hasnt attacked before doing anything else
        if(!attacking)
        {
            //flip sprite depending on direction facing
            checkDirection();
            if (facingLeft)
                this.transform.localRotation = Quaternion.Euler(0, 180, 0);
            else
                this.transform.localRotation = Quaternion.Euler(0, 0, 0);

            //check health and determine whether to play defensive or offensive
            if (health > 30)
            {
                offensive();
            }
            else
            {
                defensive();
            }
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
        //random move selection
        int attack = Random.Range(0, 4);

        //check distance to start walking towards player
        if(Distance() > 2 && player.position.y <= me.position.y)
        {
            if(!attacking)
            {
                anim.SetBool("isWalking", true);
                walk();
            } 
        }
        else
        {
            anim.SetBool("isWalking", false);
            rb2d.velocity = Vector2.zero;

            if(!attacking)
            {
                attacking = true;
                switch (attack)
                {
                    case 0: anim.SetTrigger("punch");break;
                    case 1: anim.SetTrigger("kick");  break;
                    case 2: anim.SetTrigger("poke"); break;
                    case 3: anim.SetTrigger("slash");  break;
                }
             
            }
        }
    }

    //play defensive
    void defensive()
    {

    }

    //jump
    void jump()
    {

    }

    //this is an animation event called once an animation has finished
   void OnCompleteAttackAnimation()
    {
        Debug.Log("stopped attacking");
        //check if anakin will jump away or run away
        int rnd = Random.Range(0, 1);
        rb2d.velocity = Vector2.zero;
        switch (rnd)
        {
            case 0:
                StartCoroutine(runAway(Random.Range(0.5f, 1.5f))); break;
            case 1:
                StartCoroutine(runAway(Random.Range(0.5f, 1.5f))); break;
        }

    }


        //walk opposite direction X amount of seconds
        IEnumerator runAway(float seconds)
    {
        Debug.Log("Running Away" + seconds.ToString());

        var time = 0f;
        rb2d.velocity = Vector2.zero;

        facingLeft = !facingLeft;

        //flip sprite depending on direction facing
        if (facingLeft)
            this.transform.localRotation = Quaternion.Euler(0, 180, 0);
        else
            this.transform.localRotation = Quaternion.Euler(0, 0, 0);

        //walk for X seconds here
        while (time < seconds)
        {
            anim.SetBool("isWalking", true);
            walk();
            time += Time.deltaTime;

            yield return null;
        }

        //turn off walking animation
        anim.SetBool("isWalking", false);
        attacking = false;
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
