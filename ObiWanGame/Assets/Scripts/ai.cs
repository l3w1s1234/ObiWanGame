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
        if (other.gameObject.tag == "Player" && !jumping)
        {
            rb2d.isKinematic = true;
            rb2d.velocity = rb2d.velocity * 0;
        }

        if (other.gameObject.name == "floor" && jumping)
        {
            checkLanded();
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

        //check that ai is falling and change animation
        if (rb2d.velocity.y < -0.5)
        {
            anim.SetBool("isFalling", true);
        }
        else if (rb2d.velocity.y == 0)
        {
            anim.SetBool("isFalling", false);
        }

        //check that ai hasnt attacked before doing anything else
        if (!attacking)
        {
            //flip sprite depending on direction facing
            checkDirection();
            changeFacing();

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

    //chooses a random clip out of 2
    private void swichClips(AudioClip clip1, AudioClip clip2)
    {
        int clip = Random.Range(0, 2);

        switch (clip)
        {
            case 0:
                sound.clip = clip1;
                sound.Play();
                break;

            case 1:
                sound.clip = clip2;
                sound.Play();
                break;
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


    //check that player has landed
    public void checkLanded()
    {
        if (rb2d.velocity.y == 0 && jumping == true)
        {

            swichClips(landingSound, taunt2Sound);
            jumping = false;
            attacking = false;
        }
    }

    //play offensive
    void offensive()
    {
        //random move selection
        int attack = Random.Range(0, 4);

        //check distance to start walking towards player
        if(Distance() > 2 && player.position.y <= me.position.y)
        {
            if(!attacking && !jumping)
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
                    case 0: anim.SetTrigger("punch");
                        sound.clip = punchSound;
                        sound.Play(); break;
                    case 1: anim.SetTrigger("kick");
                        sound.clip = kickSound;
                        sound.Play(); break;
                    case 2: anim.SetTrigger("poke");
                        swichClips(pokeSound, slashSound); break;
                    case 3: anim.SetTrigger("slash");
                        swichClips(slashSound,taunt3Sound); break;
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
        changeFacing();

        jumping = true;
        walk();
        rb2d.AddForce(Vector2.up * jumpHeight);
        anim.SetTrigger("jump");
    }

    //this is an animation event called once an animation has finished
   void OnCompleteAttackAnimation()
    {
        Debug.Log("stopped attacking");
        //check if anakin will jump away or run away
        int rnd = Random.Range(0, 2);
        rb2d.velocity = Vector2.zero;
        switch (rnd)
        {
            case 0:
                StartCoroutine(runAway(Random.Range(0.5f, 1.5f))); break;
            case 1:
                facingLeft = !facingLeft;
                StartCoroutine(walkThenJump(0.1f));
                break;
        }

    }

    //flip sprite depending on direction facing
    void changeFacing()
    {
        if (facingLeft)
            this.transform.localRotation = Quaternion.Euler(0, 180, 0);
        else
            this.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

        //walk opposite direction X amount of seconds
        IEnumerator runAway(float seconds)
    {
        Debug.Log("Running Away" + seconds.ToString());

        var time = 0f;
        rb2d.velocity = Vector2.zero;

        facingLeft = !facingLeft;
        changeFacing();


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

    //walk towards directiuon that it is currently facing
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

    //walk for a set amount of seconds before jumping
    IEnumerator walkThenJump(float delay)
    {
        var time = 0f;
        while (time < delay)
        {
            walk();
            time += Time.deltaTime;
            yield return null;

        }
        jump();

    }

}
