using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePiece : MonoBehaviour
{
    bool m_isMoving = false; // for checking if objects are moving or not at that time
    public int xIndex;
    public int yIndex;
    public Board m_board;
    public bool match_done = false;

    public int score=20;

    public MatchValue matchValue; //since it is public, the match value is able to being changed by Unity manually, since it is public and all set according to their colours 



    public virtual void handleScore()
    {}

    public enum MatchValue
    {
        Yellow,
        Blue,
        Green,
        Red, 
        Wild
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move((int)transform.position.x + 1, (int)transform.position.y, 0.5f);

        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move((int)transform.position.x - 1, (int)transform.position.y, 0.5f);

        }
        */
    }

    public void Init(Board board)
    {

        m_board = board;

    }



    // the game pieces will move during the game so coordinates of it should be set from time to time
    public void SetCoord(int x, int y)
    {
        xIndex = x;
        yIndex = y;

    }

    //timetoMove for how long we want the action of the movement take
    public void Move(int destX, int destY, float timeToMove)
    {

        if (!m_isMoving &m_board.user_move_Enabled) // if objects are not moving at that time
        {
            m_board.user_move_Enabled = false;
            StartCoroutine(MoveRoutine(new Vector3(destX, destY, 0), timeToMove));
            //      System.Threading.Thread.Sleep(40);
            m_board.user_move_Enabled = true;

        }
    }


    IEnumerator MoveRoutine(Vector3 destination, float timeToMove)
    {
        Vector3 startPosition = transform.position; //starting position of our object before moving. This is need for vector3.Lerp

        bool reachedDestination = false;

        float elapsedTime = 0f;

        m_isMoving = true; // true because it will enter to the move loop one line later

        while (!reachedDestination)
        {
            // if we are about to reach our destination
            if (Vector3.Distance(transform.position, destination) < 0.01f)
            {
                reachedDestination = true; //since we reached the destination then we can make it true in order to end the loop

                if (m_board != null) // as long as the below placegamepiece method is being called, this routine will not end thus m_Board object will not let any changes on it  
                {
                    m_board.PlaceGamePiece(this, (int)destination.x, (int)destination.y); 

                }
            
                break;

            }


            //if we did not reached our destination
            // track the total running time
            elapsedTime += Time.deltaTime; //deltaTime is time in seconds that it took the last frame to run. we are pausing every frame with respected to deltaTime
            // so at the end it will equal to total time 

            // calculate the Lerp value
       
            float t = Mathf.Clamp(elapsedTime / timeToMove, 0f, 1f); //so with this, object will change its position stage by stage while time elapses
            t = t * t * (3 - 2 * t);

            // move the game piece
            // thanks to above float t calculation, our game piece will have small advances at every turn of the loop
            transform.position = Vector3.Lerp(startPosition, destination, t);

            // wait until next frame
            //yield return new WaitForSeconds(0.2f);
            yield return null;

        }

        m_isMoving = false; // false since the moving operation is done


    }


}
