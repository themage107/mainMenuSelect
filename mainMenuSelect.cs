using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class mainMenuSelect : MonoBehaviour {

    int selPos;
    float selX = 3.75f;
    float selY;
    float selZ = 0;

    int minPos;
    int maxPos;

    bool canMove;
    public static bool stopMovement = true;

    int loadedBefore;
    int continueLevel;

    void Start()
    {
        //do something about no continue option available and general saved settings        
        setupPlayerPrefs();

        //set selection arrow position
        selectionArrowSetup();

        //selPos = 1;
        canMove = true;
    }

    void setupPlayerPrefs()
    {
        loadedBefore = PlayerPrefs.GetInt("gameLoadedBefore");

        if (loadedBefore != 1)
        {
            // game has now been loaded before
            PlayerPrefs.SetInt("gameLoadedBefore", 1);

            // no level to continue from
            PlayerPrefs.SetInt("continueLevel", 0);

            // reset all the notes to not viewed
            for (int i = 0; i < 77; i++)
            {
                string noteViewed = "noteViewed" + i.ToString();
                PlayerPrefs.SetInt(noteViewed, 0);                
            }

            for (int i = 0; i < 6; i++)
            {
                string keyFound = "keyFound" + i.ToString();
                PlayerPrefs.SetInt(keyFound, 0);
            }            

            PlayerPrefs.Save();

            minPos = 1;
            selPos = 1;
            selY = -0.35f;

            GameObject cont = GameObject.Find("Continue");
            cont.GetComponent<Text>().color = new Color(1,1,1, 0.15f);
        }
        else
        {            

            continueLevel = PlayerPrefs.GetInt("continueLevel");
            Debug.Log("This is the continue level: " + continueLevel);
            minPos = 0;
            selPos = 0;
            selY = 0.68f;

        }
    }

    void selectionArrowSetup()
    {
        this.transform.position = new Vector3(selX, selY, selZ);
        maxPos = 3;
    }

    // Update is called once per frame
    void Update () {
        if (!stopMovement)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && selPos > minPos && canMove || Input.GetAxis("VerticalJS") < -0.25 && selPos > minPos && canMove)
            {
                this.gameObject.GetComponent<Transform>().position = new Vector3(selX, this.gameObject.GetComponent<Transform>().position.y + 1, selZ);
                selPos--;
                canMove = false;                
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) && selPos < maxPos & canMove || Input.GetAxis("VerticalJS") > 0.25 && selPos < maxPos && canMove)
            {
                this.gameObject.GetComponent<Transform>().position = new Vector3(selX, this.gameObject.GetComponent<Transform>().position.y - 1, selZ);
                selPos++;
                canMove = false;                

            }

            //player didn't press any buttons so the menu thing doesn't freak out
            else if (!canMove && Input.GetAxis("VerticalJS") < 0.25 && Input.GetAxis("VerticalJS") > -0.25)
            {
                canMove = true;
            }

            if (Input.GetKeyDown(KeyCode.Space) && selPos == 0 || Input.GetKeyDown(KeyCode.Joystick1Button0) && selPos == 0)
            {
                SceneManager.LoadScene("level" + continueLevel);

                //we don't want the player to keep messing with stuff
                stopMovement = true;
            }

            if (Input.GetKeyDown(KeyCode.Space) && selPos == 1 || Input.GetKeyDown(KeyCode.Joystick1Button0) && selPos == 1)
            {
                //LoadIntro.advance = true;
                
                //we don't want the player to keep messing with stuff
                stopMovement = true;
            }

            if (Input.GetKeyDown(KeyCode.Space) && selPos == 3 || Input.GetKeyDown(KeyCode.Joystick1Button0) && selPos == 3)
            {
                Application.Quit();
                stopMovement = true;
            }

            
            if (Input.GetAxis("Cancel") > 0)
            {
                PlayerPrefs.SetInt("gameLoadedBefore", 0);
                PlayerPrefs.Save();
            }
            
        }

    }
}
