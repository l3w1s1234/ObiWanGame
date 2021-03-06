﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class control : MonoBehaviour 
{
	//for movement 
	public float speed;
	public float jumpHeight = 5;

	//stats
	private int health = 100;
    public int regen = 5;

	//keys
	public KeyCode left;
	public KeyCode right;
	public KeyCode taunt1;
	public KeyCode punch;
	public KeyCode kick;
	public KeyCode poke;
	public KeyCode slash;
	public KeyCode jump;

	//components
	private Animator anim;
	private Rigidbody2D rb2d;
	private AudioSource sound;
    public Slider healthBar;
    private FightState fs;

	//checkers
	private bool canWalk = true;
	private bool jumping = false;
	private bool facingRight = true;
    private bool attacking = false;
    private bool hit = false;

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


    //attack variables
    public Transform attackPos;
    public float attackRange;
    public LayerMask whatIsEnemy;
    public int damage;

    void Start () 
	{
        fs = GameObject.Find("Countdown").GetComponent<FightState>();
        rb2d = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		sound = GetComponent<AudioSource> ();
	}

    //for physiscs like jumping
    void FixedUpdate()
    {
        //check fight is active
        if (fs.fight)
        {

                //check if  can jumping
                if (Input.GetKey(jump) && jumping == false)
                {
                    anim.SetBool("isWalking", false);
                    anim.SetTrigger("jump");
                    sound.clip = jumpSound;
                    sound.Play();
                    rb2d.AddForce(Vector2.up * jumpHeight);
                    jumping = true;
                }
            

        }
    }


	// Update is called once per frame
	void Update () 
	{

       
        //keep health at 100
        if (health > 100)
            health = 100;

        //flip sprite depending on direction facing
        if (facingRight)
			this.transform.localRotation = Quaternion.Euler(0, 0, 0);
		else
			this.transform.localRotation = Quaternion.Euler(0, 180, 0);

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("idle"))
            canWalk = true;


            //check that player has landed
        if (jumping)
		    checkLanded ();

        //check that the fight is on before granting user control
        if(fs.fight)
        {

            setAnim();


            if (canWalk)
                move();

            //deal damage to any enemies being attacked
            if (attacking)
            {
                if (!hit)
                {
                    Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemy);

                    //check that an enemy has been hit
                    if (enemiesToDamage != null)
                        hit = true;
                    //damage enmies in damage radius
                    for (int i = 0; i < enemiesToDamage.Length; i++)
                    {
                        enemiesToDamage[i].GetComponent<ai>().takeDamage(damage);
                    }
                }
            }

            //if health is less than 0 AI wins
            if (health <= 0)
            {
                fs.hasLost();
                anim.enabled = false;
            }
        }

        //check that we have won to disable animation
        if (fs.win)
        {
            anim.enabled = false;
        }

    }

    //for test purposes/see hit area
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

    //take damage from AI
    public void takeDamage(int amount)
    {
        health -= amount;
        healthBar.value = health;
        Debug.Log("Damage Taken");
    }

    //used to reheal // called after taunt animation
    void reHeal()
    {
        health += regen;
        healthBar.value = health;
    }

    //check collison
    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log(other.gameObject.tag);

        if (other.gameObject.tag == "Enemy")
        { 
        }
    }

        //check keys for movement and move appropriatly
        public void move()
	{
		
		if (Input.GetKey (right)) {
			facingRight = true;
			rb2d.velocity = new Vector2 (speed, rb2d.velocity.y);
		} else if (Input.GetKey(left)) 
		{
			facingRight = false;
			rb2d.velocity = new Vector2 (-speed, rb2d.velocity.y);
		} 
		else if (!Input.anyKey ) 
		{
			rb2d.velocity = new Vector2 (0, rb2d.velocity.y);
		}

		
	}



	//check that player has landed
	public void checkLanded()
	{
		if (rb2d.velocity.y == 0 && jumping == true) 
		{
			
			swichClips (landingSound, taunt2Sound);

            
            Debug.Log ("has Landed");
			jumping = false;
            canWalk = true;
            anim.SetBool("falling", false);
		}
	}


	//set the objects animaiton
	public void setAnim()
	{

        //change animation if not moving or is moving
		if (!Input.anyKey) {
			anim.SetBool ("isWalking", false);
		} else if (Input.GetKey (left) && canWalk == true) {
			anim.SetBool ("isWalking", true);
		} else if (Input.GetKey (right) && canWalk == true) {
			anim.SetBool ("isWalking", true);
		}




		//check if player is falling
		if (rb2d.velocity.y < -0.5) {
			anim.SetBool ("falling", true);           
		} else if (rb2d.velocity.y == 0) {
			anim.SetBool ("falling", false);
		}

		//determine button press animations
		if (Input.GetKeyDown (taunt1)) {
			rb2d.velocity = new Vector2 (0, rb2d.velocity.y);

			sound.clip = taunt1Sound;
			sound.Play ();

			anim.SetTrigger ("taunt1");
			canWalk = false;
		}
        if (Input.GetKeyDown (punch)) 
		{
			rb2d.velocity = new Vector2 (0, rb2d.velocity.y);

			sound.clip = punchSound;
			sound.Play ();

			anim.SetTrigger ("punch");
			canWalk = false;
            attacking = true;
        }
        if (Input.GetKeyDown (kick)) 
		{
			rb2d.velocity = new Vector2 (0, rb2d.velocity.y);


			sound.clip = kickSound;
			sound.Play ();

			anim.SetTrigger ("kick");
			canWalk = false;
            attacking = true;
        }
        if (Input.GetKeyDown (poke)) 
		{
			rb2d.velocity = new Vector2 (0, rb2d.velocity.y);

			swichClips (pokeSound, slashSound);


			anim.SetTrigger ("poke");
			canWalk = false;
            attacking = true;
        }
        if (Input.GetKeyDown (slash)) 
		{
			rb2d.velocity = new Vector2 (0, rb2d.velocity.y);

			swichClips (taunt3Sound, slashSound);

			anim.SetTrigger ("slash");
			canWalk = false;
            attacking = true;
        }
			
	}

    //when attack animation has finished
    void onAttackFinish()
    {
        attacking = false;
        hit = false;
        canWalk = true;
    }

	//chooses a random clip out of 2
	private void swichClips(AudioClip clip1, AudioClip clip2)
	{ 
		int clip = Random.Range(0, 2);

		switch (clip) {
		case 0:
			sound.clip = clip1;
			sound.Play ();
			break;

		case 1:
			sound.clip = clip2;
			sound.Play ();
			break;
		}
	}
}
