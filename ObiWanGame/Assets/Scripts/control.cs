using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class control : MonoBehaviour 
{
	//for movement 
	public float speed;
	public float jumpHeight = 5;

	//stats
	public int health = 100;

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

	//checkers
	private bool canWalk = true;
	private bool jumping = false;
	private bool facingRight = true;

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


	void Start () 
	{
		rb2d = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		sound = GetComponent<AudioSource> ();
	}


	// Update is called once per frame
	void FixedUpdate () 
	{

       

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

		setAnim ();

       

        move();


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
		
		if (Input.GetKey (right) && canWalk == true) {
			facingRight = true;
			rb2d.velocity = new Vector2 (speed, rb2d.velocity.y);
		} else if (Input.GetKey(left) && canWalk == true) 
		{
			facingRight = false;
			rb2d.velocity = new Vector2 (-speed, rb2d.velocity.y);
		} 
		else if (!Input.anyKey ) 
		{
			rb2d.velocity = new Vector2 (0, rb2d.velocity.y);
		}

		//check if jumping
		if (Input.GetKey(jump) && jumping == false ) 
		{
			canWalk = false;
			rb2d.AddForce(Vector2.up * jumpHeight);
			jumping = true;
		}
	}



	//check that player has landed
	public void checkLanded()
	{
		if (rb2d.velocity.y == 0) 
		{
			
			swichClips (landingSound, taunt2Sound);

			Debug.Log ("has Landed");
			jumping = false;
            canWalk = true;
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


		//Jumping animation
		if (Input.GetKeyDown(jump) && jumping == false) {
			
			sound.clip = jumpSound;
			sound.Play ();

            anim.SetTrigger("jump");
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
		}
        if (Input.GetKeyDown (kick)) 
		{
			rb2d.velocity = new Vector2 (0, rb2d.velocity.y);


			sound.clip = kickSound;
			sound.Play ();

			anim.SetTrigger ("kick");
			canWalk = false;
		}
        if (Input.GetKeyDown (poke)) 
		{
			rb2d.velocity = new Vector2 (0, rb2d.velocity.y);

			swichClips (pokeSound, slashSound);


			anim.SetTrigger ("poke");
			canWalk = false;
		}
        if (Input.GetKeyDown (slash)) 
		{
			rb2d.velocity = new Vector2 (0, rb2d.velocity.y);

			swichClips (taunt3Sound, slashSound);

			anim.SetTrigger ("slash");
			canWalk = false;
		}
			
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
