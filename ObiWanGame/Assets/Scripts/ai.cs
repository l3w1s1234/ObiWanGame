using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ai : MonoBehaviour {

    //components
    private Animator anim;
    private Rigidbody2D rb2d;
    private AudioSource sound;



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
            rb2d.velocity = Vector2.zero;
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
    void Update () {


    }
}
