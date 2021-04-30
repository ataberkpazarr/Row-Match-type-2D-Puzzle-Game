using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


[RequireComponent(typeof(Button))]
public class Button_Controller : MonoBehaviour
    
{
    public Button[] LevelButtons;
    public bool[] level_locked; 
    public Button mybutton;
    public Sprite blockA; // unlocked level sprite which used in unlocked level buttons
    public Sprite blockA_disable; //locked level sprite which used in locked level buttons
    private int counter = 0;


    public static int grid_width;
    public static int grid_height;
    public static int move_count;

    public static char[] grid;


    void Start()
    {
        
        
        level_locked[0] = true; //first level is unlocked
        LevelButtons[0].image.overrideSprite = blockA;
        
        for (int i =1;i<LevelButtons.Length;i++) // assigning gray pictures and making unclickable buttons except first button which represents first level
        {
            
            
            LevelButtons[i].image.overrideSprite = blockA_disable;
            LevelButtons[i].interactable = false;
        }

        

        if(PlayerPrefs.HasKey("unlocked_level")) // thanks to unlocked_level key, I am able to track which the information of which levels are locked and which are not. It is being updated by Board.cs class at every end of a level
        {

            int unlocked_level = PlayerPrefs.GetInt("unlocked_level");
            for (int i =0; i<unlocked_level+1;i++)
            {
                
                LevelButtons[i].image.overrideSprite = blockA;
                LevelButtons[i].interactable = true;
            }

            for (int k =unlocked_level+1;k<LevelButtons.Length;k++)
            {
                
                LevelButtons[k].image.overrideSprite = blockA_disable;
                LevelButtons[k].interactable = false;
            }

        }
        
        
    }

    

 
  

    public void setLevelParameters(int width, int height, int count_of_move, char [] pieces) //configuring the level parameters, the left hand side parameters at the below are static parameters
    {

        
            grid_width=width;
            grid_height=height;
            move_count=count_of_move;

            grid=pieces;
        

    }


    public void On_click_function() // when level button is clicked
    {

     

        string k = EventSystem.current.currentSelectedGameObject.name; //find the name of the button that clicked and configure the level respectively
        int a = int.Parse(k); //name of the button, for ease I named buttons as 1,2,3,4,5...
       

        
        if (a == 0) // level 1 RM_A1
        {
            char[] arr = { 'b', 'b', 'y', 'b', 'b', 'g', 'y', 'g', 'r', 'b', 'y', 'g', 'r', 'g', 'g', 'b', 'b', 'g', 'b', 'y', 'r', 'r', 'g', 'g', 'y', 'g', 'g', 'y', 'y', 'b', 'y', 'b', 'b', 'y', 'b' };
            setLevelParameters(5,7,20,arr); // width,height,move count, game pieces array
            PlayerPrefs.SetInt("unlocked_level", 0);

            

            SceneManager.LoadScene("Main");
           

        }
     
        else if (a == 1) // level 2 RM_A2
        {
            char[] arr = { 'r', 'r', 'b', 'b', 'y', 'b', 'r', 'r', 'y', 'g', 'y', 'y', 'g', 'b', 'r', 'b', 'r', 'y', 'r', 'y', 'y', 'g', 'y', 'r', 'y', 'b', 'g', 'r', 'b', 'r', 'g', 'g', 'g', 'y', 'g' };
            setLevelParameters(7, 5, 18, arr);

            PlayerPrefs.SetInt("unlocked_level", 1);

            SceneManager.LoadScene("Main");


        }

        else if (a == 2)// level 3 RM_A3
        {
            char[] arr = { 'g', 'r', 'y', 'r', 'r', 'g', 'r', 'g', 'y', 'y', 'b', 'b', 'y', 'g', 'r', 'r', 'y', 'g', 'b', 'g', 'y', 'g', 'r', 'y', 'r', 'y', 'g', 'b', 'g', 'y', 'g', 'y', 'r', 'g', 'g', 'y', 'b', 'g', 'b', 'r', 'g', 'b', 'r', 'b', 'g', 'g', 'y', 'g' };
            setLevelParameters(8, 6, 23, arr);

            PlayerPrefs.SetInt("unlocked_level", 2);

            SceneManager.LoadScene("Main");

        }

        else if (a == 3)// level 4 RM_A4
        {
            char[] arr = { 'r', 'r', 'y', 'r', 'r', 'r', 'r', 'b', 'b', 'y', 'g', 'y', 'g', 'y', 'g', 'y', 'b', 'b', 'g', 'g', 'b', 'b', 'b', 'b', 'r' };
            setLevelParameters(5, 5, 30, arr);

            PlayerPrefs.SetInt("unlocked_level", 3);

            SceneManager.LoadScene("Main");

        }
        else if (a == 4) // level 5 RM_A5
        {
            char[] arr = { 'g', 'r', 'y', 'r', 'y', 'y', 'r', 'r', 'g', 'y', 'y', 'r', 'y', 'g', 'g', 'g', 'y', 'g', 'r', 'y', 'b', 'b', 'b', 'g', 'g', 'b', 'g', 'g', 'b', 'g', 'g', 'y', 'r', 'b', 'g', 'b', 'r', 'b', 'b', 'y', 'r', 'y', 'g', 'g', 'b', 'g', 'b', 'b', 'y', 'r', 'b', 'b', 'g', 'g' };
            setLevelParameters(6, 9, 24, arr);

            PlayerPrefs.SetInt("unlocked_level", 4);

            SceneManager.LoadScene("Main");

        }

        else if (a == 5 ) //level 6 RM_A6
        {
            char[] arr = { 'r', 'g', 'r', 'g', 'r', 'b', 'r', 'b', 'r', 'y', 'y', 'r', 'y', 'y', 'g', 'b', 'y', 'y', 'y', 'b', 'b', 'y', 'g', 'g', 'b', 'r', 'b', 'r', 'y', 'r', 'g', 'r', 'r', 'b', 'g', 'b' };
            setLevelParameters(4, 9, 26, arr);
            PlayerPrefs.SetInt("unlocked_level", 5);

            SceneManager.LoadScene("Main");
            

        }

        else if (a == 6 ) //level 7 RM_A7
        {
            char[] arr ={ 'y', 'b', 'g', 'r', 'y', 'g', 'b', 'g', 'g', 'r', 'b', 'r', 'b', 'y', 'y', 'g', 'g', 'r', 'g', 'b', 'r', 'b', 'b', 'g', 'y', 'g', 'b', 'b', 'r', 'b', 'g', 'r', 'b', 'r', 'r', 'g', 'b', 'b', 'g', 'b', 'g', 'y', 'y', 'b', 'y', 'g', 'g', 'r', 'b', 'g', 'g', 'y', 'g', 'g', 'y', 'g', 'y', 'r', 'b', 'b', 'b', 'r', 'y' };
            setLevelParameters(7, 9, 23, arr);
            PlayerPrefs.SetInt("unlocked_level", 6);

            SceneManager.LoadScene("Main");

        }

        else if (a == 7 ) //level 8 RM_A8
        {
            char[] arr = { 'r', 'g', 'b', 'r', 'r', 'b', 'b', 'g', 'r', 'y', 'b', 'y', 'b', 'g', 'g', 'b', 'b', 'b', 'b', 'r', 'r', 'y', 'r', 'g', 'y', 'g', 'g', 'g', 'g', 'r', 'b', 'b', 'b', 'r', 'y', 'b', 'y', 'b', 'r', 'r' };
            setLevelParameters(5, 8, 30, arr);
            PlayerPrefs.SetInt("unlocked_level", 7);

            SceneManager.LoadScene("Main");

        }

        else if (a == 8 ) //level 9 RM_A9
        {
            char[] arr = { 'y', 'g', 'r', 'b', 'r', 'b', 'b', 'b', 'b', 'b', 'b', 'r', 'g', 'y', 'y', 'y', 'r', 'r', 'g', 'y', 'r', 'b', 'g', 'b', 'r', 'r', 'g', 'r', 'y', 'r' };
            setLevelParameters(6, 5, 19, arr);
            PlayerPrefs.SetInt("unlocked_level", 8);

            SceneManager.LoadScene("Main");

        }

        else if (a == 9 ) //Level 10 RM_A10
        {
            char[] arr = { 'g', 'b', 'y', 'y', 'b', 'g', 'r', 'r', 'g', 'g', 'b', 'g', 'y', 'b', 'g', 'y', 'g', 'b', 'y', 'r', 'b', 'r', 'y', 'g', 'b', 'g', 'r', 'b', 'g', 'g', 'y', 'b', 'g', 'g', 'y', 'y', 'b', 'g', 'y', 'g', 'y', 'r', 'y', 'b', 'b', 'g', 'b', 'g', 'r', 'r', 'g', 'b', 'b', 'y', 'y', 'g', 'g', 'y', 'r', 'r', 'g', 'y', 'b', 'r', 'g', 'y', 'g', 'y', 'g', 'r', 'r', 'b' };
            setLevelParameters(8, 9, 21, arr);
            PlayerPrefs.SetInt("unlocked_level", 9);

            SceneManager.LoadScene("Main");

        }

        else if (a == 10 ) //Level 11 RM_A11
        {
            char[] arr = { 'b', 'b', 'y', 'b', 'b', 'g', 'y', 'g', 'r', 'b', 'y', 'g', 'r', 'g', 'g', 'b', 'b', 'g', 'b', 'y', 'r', 'r', 'g', 'g', 'y', 'g', 'g', 'y', 'y', 'b', 'y', 'b', 'b', 'y', 'b' };
            setLevelParameters(5, 7, 28, arr);
            PlayerPrefs.SetInt("unlocked_level", 10);

            SceneManager.LoadScene("Main");

        }

        //If I add levels as it, then the new levels will be playable in the flow of the game 

   
    }

}
