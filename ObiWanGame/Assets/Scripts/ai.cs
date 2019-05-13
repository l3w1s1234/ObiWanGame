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


    // Use this for initialization
    void Start () { 

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

        //flip sprite depending on direction facing
        if (facingLeft)
            this.transform.localRotation = Quaternion.Euler(0, 180, 0);
        else
            this.transform.localRotation = Quaternion.Euler(0, 0, 0);

    }
}
