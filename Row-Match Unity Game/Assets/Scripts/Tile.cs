using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int xIndex;
    public int yIndex;
    

    Board m_board; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init(int x, int y, Board board)
    {

        xIndex = x;
        yIndex = y;
        m_board = board;


    }

    // Update is called once per frame
    /*
    void Update()
    {
        
    }
    */

    // we need a collider on the tile class in order for the mouse events to register
        void OnMouseDown()
        {
            if (m_board != null && m_board.user_move_Enabled) // for checking if we have valid board object 
            {
                m_board.ClickTile(this); // this tile object
            }

        }

    void OnMouseEnter()
    {
        if (m_board != null && m_board.user_move_Enabled)
        {
            m_board.DragToTile(this);
        }

    }

    void OnMouseUp()
    {
        if (m_board != null && m_board.user_move_Enabled )
        {
            m_board.ReleaseTile();
        }

    }
}
