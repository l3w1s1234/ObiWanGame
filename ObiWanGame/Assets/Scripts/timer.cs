using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timer : MonoBehaviour {

    private int time = 0;


    //components
    private Text timeText;
    private FightState fs;

    // Use this for initialization
    void Start () {
        fs = GameObject.Find("Countdown").GetComponent<FightState>();
        timeText = GetComponent<Text>();

        InvokeRepeating("counter", 0, 1.0f);
    }
	
	// Update is called once per frame
	void Update () {

       

	}

    void counter()
    {

        //start counter when fight starts

        if (fs.fight )
        {
            time += 1;

            timeText.text = time.ToString();

            if (timeText.text.Length == 1)
            {
                timeText.text = "00" + timeText.text;
            }


            if (timeText.text.Length == 2)
            {
                timeText.text = "0" + timeText.text;
            }

            if(timeText.text.Length > 3)
            {
                timeText.text = "999";
            }
        }
       

    }

}
