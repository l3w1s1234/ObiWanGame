using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FightState : MonoBehaviour {

    public bool fight = false;
    public bool win = false;
    public bool lose = false;
    public bool[] clipPlayed = new bool[6];

    public AudioClip count;
    public Sprite[] sprite;

    private AudioSource audio;
    private Image image;

	// Use this for initialization
	void Start () {

        //initiate audio clips that havent been played
        for(int i = 0; i<clipPlayed.Length; i++)
        {
            clipPlayed[i] = false;
        }

        audio = GetComponent<AudioSource>();
        image = GetComponent<Image>();
        StartCoroutine(countdown());
    }
	

    IEnumerator countdown()
    {
        var time = 0f;
        while(time < 4.5f)
        {
            if (time <= 1f)
            {
                image.sprite = sprite[0];
                //playSound
                if(!clipPlayed[0])
                {
                    audio.clip = count;
                    audio.Play();
                    clipPlayed[0] = true;
                }
            }
            else if(time <=2f)
            {
                image.sprite = sprite[1];
                //playSound
                if (!clipPlayed[1])
                {
                    audio.clip = count;
                    audio.Play();
                    clipPlayed[1] = true;
                }
            }
            else if (time <= 3f)
            {
                image.sprite = sprite[2];
                //playSound
                if (!clipPlayed[2])
                {
                    audio.clip = count;
                    audio.Play();
                    clipPlayed[2] = true;
                }
            }
            else if (time <= 4f)
            {
                image.sprite = sprite[3];
                //playSound
                if (!clipPlayed[3])
                {
                    audio.clip = count;
                    audio.Play();
                    clipPlayed[3] = true;
                }
            }


            time += Time.deltaTime;
            yield return null;
        }

        image.enabled = !image.enabled;
        fight = true;
    }

    //called when AI is out of health
    public void hasWon()
    {
        win = true;
        fight = false;
        image.enabled = !image.enabled;
        image.sprite = sprite[4];
        StartCoroutine(quitFight());
    }

    //called when player is out of health
    public void hasLost()
    {
        lose = true;
        fight = false;
        image.enabled = !image.enabled;
        image.sprite = sprite[5];
        StartCoroutine(quitFight());
    }


	// Update is called once per frame
	void Update () {
            
	}

    //wait 4 seconds and load menu
    IEnumerator quitFight()
    {
        var time = 0f;

        while(time <= 4f)
        {
            time += Time.deltaTime;

            yield return null;
        }

        SceneManager.LoadScene("Menu");
        
    }
}
