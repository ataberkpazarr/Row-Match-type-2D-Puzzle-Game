using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LEVELS_sprite_controller : ChangeScenes
{
    bool released = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }



    void OnMouseDown()
    {
        released = true;
        LoadScene("_LevelScene_");
        //screenfader eklenebilir

    }

    void OnMouseUp()
    {
        if (released) // for checking if we have valid board object 
        {
            //LoadScene("Main");
        }

        //LoadScene("Main");

    }

    public void onClick_resetButton() // resetting all levels, making locked except first level and initializing 1000 highest score to the all of them 
    {

        PlayerPrefs.DeleteAll();
        for (int i =0; i<25;i++) 
        {

            string key = string.Concat(i.ToString(), "highest");
            PlayerPrefs.SetInt(key, 1000);
            Debug.Log(key);
            ScoreManager.Instance.UpdateHighest(1000);

        }
    }
}
