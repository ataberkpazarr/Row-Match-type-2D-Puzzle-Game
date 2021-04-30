using System.Collections;
using System.Collections.Generic; // in order to use generic list 
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;


public class Board : MonoBehaviour
{
    //Lower left is 0,0 coordinate and upper right corner is max x,y
    public bool user_move_Enabled = true;
    public int width;
    public int height;
    public float swapTime = 0.5f;  // public in order to try and check which swapping time is best, by changing from unity manually    
    public int borderSize;

    // so with defining them as global, it is possible to assign them values from unity window 


    public GameObject tilePrefab; // the prefab that we will use to create one square the board. it will lay down one square and duplicate it with a couple of loops 
    
    public GameObject[] gamePiecePrefabs;  // for storing the prefabs of the game pieces so we will be able to reach needed colored game piece
    GamePiece[,] m_allGamePieces;
    // tiles does not move but gamepieces will move with using the help of tiles array indexing 

    Tile[,] m_allTiles; // C# sytax for composing two dimesional tile array 


   public Tile m_clickedTile; // this line and the below one is for handling the switching operation
    public Tile m_targetTile;



    public GameObject ConfettiX;
    public  int _movesLeft;

    
    void Start()
    {
        
        //this array for storing indexes of tiles 
        height = Button_Controller.grid_height;
        width = Button_Controller.grid_width;
        _movesLeft = Button_Controller.move_count;



        //tiles are different from the gamepieces, same indexes but never moves, so tiles are the things which give me opportunity to iterate over the board
        m_allTiles = new Tile[width, height];

        //and the below array is for storing the indexes of gamepieces, it was created above and now it is initialized
        m_allGamePieces = new GamePiece[width,height];

        ScoreManager.Instance.UpdateMovesCount(_movesLeft);
        SetupTiles();
        SetupCamera();
        SetupGamePieces(Button_Controller.grid);
       

        int curr_level = PlayerPrefs.GetInt("unlocked_level");
      
        string key = string.Concat(curr_level.ToString(), "highest");

        ScoreManager.Instance.UpdateHighest(PlayerPrefs.GetInt(key)); // initialize the highest score for the current level. it is being observed inside of the level

    }

   

    void SetupTiles() //creating tiles
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject tile = Instantiate(tilePrefab, new Vector3(i, j, 0), Quaternion.identity) as GameObject;

                tile.name = "Tile (" + i + "," + j + ")";

                m_allTiles[i, j] = tile.GetComponent<Tile>();

                tile.transform.parent = transform;
                // with this we have achieved to store indexes of all tiles so we can add some properties in future in other words tiles may have special properties
                m_allTiles[i, j].Init(i, j, this);


            }
        }
    }


    public void SetupGamePieces(char [] pieces) // setting up gamepieces with respected to desired order, the parameter char array is %100 equal to the .txt grid {r,g,r,b.....}

    {

       
        int k = 0;

        char[,] two_d_converted = new char[width, height];



        for (int i = 0; i < height; i++) //converting passed 1D char array to 2-D array with respected to its board positions x,y
        {
            
            for (int j = 0; j < width; j++)
            {
                two_d_converted[j,i] = pieces[k];
                k++;
            } 
        }

        for (int i = 0; i < width; i++) //creating desired coloured objects at the desired positions of the board
        {
            for (int j = 0; j < height; j++)
            {
                char ch = two_d_converted[i,j];
                // as for casting as a gameobject
                //instantiate for creating clone game piece prefabs
                //without as, instantiate does not return an efficient, fully usable object
                GameObject ga = null;
                if (ch == 'b')
                {
                    ga = Instantiate(gamePiecePrefabs[0], Vector3.zero, Quaternion.identity) as GameObject;
                }
                else if (ch =='g')
                { 
                    ga = Instantiate(gamePiecePrefabs[1], Vector3.zero, Quaternion.identity) as GameObject;

                }
                else if(ch =='r')
                {
                    ga = Instantiate(gamePiecePrefabs[2], Vector3.zero, Quaternion.identity) as GameObject;


                }
                else if(ch =='y')
                
                { 
                    ga = Instantiate(gamePiecePrefabs[3], Vector3.zero, Quaternion.identity) as GameObject;

                }
                

                if (ga != null)
                {
                    ga.GetComponent<GamePiece>().Init(this);
                    PlaceGamePiece(ga.GetComponent<GamePiece>(), i, j);
                    ga.transform.parent = transform;
                }


            }
        }

    }

    // in order to showing screen to player properly without affecting by the change of width-height dimensions
    void SetupCamera()
    {
        Camera.main.transform.position = new Vector3((float)(width - 1) / 2f, (float)(height - 1) / 2f, -10f); //position it in the (width-1/2 , height-1/2)

        float aspectRatio = (float)Screen.width / (float)Screen.height;

        float verticalSize = (float)height / 2f + (float)borderSize;

        float horizontalSize = ((float)width / 2f + (float)borderSize) / aspectRatio;

        Camera.main.orthographicSize = (verticalSize > horizontalSize) ? verticalSize : horizontalSize; // take the bigger value for setting the camera properly
        
        // the parameter where field of view is managed. orthographic size is the distance from center of the camera to the top bounddary. It is screen height/2 
        //size arttıkça camera da büyüyor

        //aspect ratio = width/height and we know that ortho size is height/2 then, aspect ratio is width/2orthosize
        //then width = 2orthosize x aspect ratio
        //border size is the distance from end of the board to the end of the camera view
        // so then, we are able to calculate orthosize by orthosize = (width/2+bordersize) / aspect ratio
        // verticalsize  and horizontal size are represents the half. since we start to create the board from center point of the camera view like the semidiameter analogy
        //they represent the half because we are setting the position of the center of the camera at the positiyon width-1/2 and height-1 / 2



    }




    //function for locating a gamepiece in the board
    //locates passed gamepiece to passed x,y coordinates
    public void PlaceGamePiece(GamePiece gamePiece, int x, int y)
    {
        if (gamePiece == null)
        {
            Debug.LogWarning("BOARD:  Invalid GamePiece!");
            return;
        }
        GamePiece targetGamePiece = m_allGamePieces[x,y];
        // transform used for repositioning the passed gamepiece object
        gamePiece.transform.position = new Vector3(x, y, 0);
        gamePiece.transform.rotation = Quaternion.identity; // we did not rotate the object
   
        if (IsWithinBounds(x, y) &&m_allGamePieces[x,y]==null) //put an element on the array if x and y is valid and on the board
        {
            m_allGamePieces[x, y] = gamePiece;
            gamePiece.SetCoord(x, y); // gamepiece object has public x,y coordinate pair and it is set when a new gamepiece is placed
            
        }

    }

    


    

   



    bool IsWithinBounds(int x, int y) //checks if the x and y exist in the current board positions
    {
        return (x >= 0 && x < width && y >= 0 && y < height);
    }

   
    


    public void ClickTile(Tile tile) 
    {
        if (m_clickedTile == null)  // if the m_clicked tile is null, then we will assign that the passed tile
        {
            m_clickedTile = tile; 
            
        }
    }

    public void DragToTile(Tile tile) //dragging operation
    {
        if (m_clickedTile != null && isNextTo(tile,m_clickedTile)) // if clicked tile is not null which means already clicked a tile for switching so we can set the target tile below 
        {
            m_targetTile = tile;
           
        }

        
    }

    public void ReleaseTile() // move operation starts when user releases tile by getting away his/her finger from mouse
    {
        if (m_clickedTile != null && m_targetTile != null) // if both are not null then we can switch them and then release
        {
            SwitchTiles(m_clickedTile, m_targetTile);
        }

        m_clickedTile = null;
        m_targetTile = null;
    }

   

    void SwitchTiles(Tile clickedTile, Tile targetTile)
    {
        StartCoroutine(SwitchTilesRoutine(clickedTile, targetTile));
    }

    bool isNextTo(GamePiece first, GamePiece second) // checks if gamepieces are adjacent
    {

        int x_index_first = first.xIndex;
        int y_index_first = first.yIndex;


        int x_index_second = second.xIndex;
        int y_index_second = second.yIndex;


        if ((x_index_first == x_index_second) && (y_index_first == y_index_second + 1))
            return true;
        else if ((x_index_first == x_index_second) && (y_index_first == y_index_second - 1))
            return true;
        else if ((x_index_first == x_index_second + 1) && (y_index_first == y_index_second))
            return true;
        else if ((x_index_first == x_index_second - 1) && (y_index_first == y_index_second))
            return true;

        return false;


    }

    IEnumerator SwitchTilesRoutine(Tile clickedTile, Tile targetTile)
    {   
        if(isNextTo(clickedTile,targetTile))
      { 
        //m_allgamepieces and m_alltiles arrays are same in terms of the indexes. m_alltiles are always remains same while m_allgamepieces changes move by move
        GamePiece clicked_game_piece = m_allGamePieces[clickedTile.xIndex, clickedTile.yIndex]; //took clicked game piece from all game pieces array with the help of clicked tile indexes 

        GamePiece target_game_piece = m_allGamePieces[targetTile.xIndex, targetTile.yIndex];

       //defensive programming, if pieces are not null, never matched and adjacent to their each
         if (target_game_piece != null && clicked_game_piece != null && isNextTo(clicked_game_piece,target_game_piece)&&(!clicked_game_piece.match_done)&&(!target_game_piece.match_done))
        {
                
                m_allGamePieces[targetTile.xIndex, targetTile.yIndex] = null; // since the positions of pieces will changed, needed re-assignings should be done
            clicked_game_piece.Move(targetTile.xIndex, targetTile.yIndex, swapTime);
                m_allGamePieces[clickedTile.xIndex, clickedTile.yIndex] = null;

                target_game_piece.Move(clickedTile.xIndex, clickedTile.yIndex, swapTime);

                _movesLeft = _movesLeft - 1; //move count decrease when game pieces switches

                ScoreManager.Instance.UpdateMovesCount(_movesLeft);

                yield return new WaitForSeconds(swapTime); 


                Check_If_New_Matches_Occured(); //check if a match occured after the switching pieces operation


                // saving highest points as 0highest,1highest,2highest.... 
                int curr_level = PlayerPrefs.GetInt("unlocked_level");
                string key = string.Concat(curr_level.ToString() ,"highest");
                

                if (_movesLeft == 0)
                {
                    if (PlayerPrefs.HasKey(key)) // if there exists a record of highest score for this specific level
                    {
                        if (ScoreManager.Instance.m_currentScore > PlayerPrefs.GetInt(key) ) //if the current score is higher than the old highest score, then next level will be unlocked
                        {
                           

                           
                            PlayerPrefs.SetInt("unlocked_level", curr_level+1); //unlock the next level
                      
                            PlayerPrefs.SetInt(key, ScoreManager.Instance.m_currentScore);
                          


                            SceneManager.LoadScene("StartScene");
                            //SpawnConfetti.Instance.SpawnTHEConfetti();  //animation  // this was working and I broke it somehow 



                        }

                        else // if the current score is not higher than oldest highest score, then next level will not be unlocked
                        {
                            
                            SceneManager.LoadScene("_LevelScene_");
                          
                        }
                    }
                    else // if this level is never played and there is no record of highest score
                    {
                        

                        PlayerPrefs.SetInt(key,ScoreManager.Instance.m_currentScore); //update the current score
                        PlayerPrefs.SetInt("unlocked_level", curr_level+1); //unlock the next level
                        
                        Debug.Log(key);
                        SceneManager.LoadScene("StartScene");
                       




                    }
                }

            }
        

     }
    }

 
    
    
    
    void Check_If_New_Matches_Occured() // This function controls all board if a new matches occured. 

    {
        

        List<GamePiece> aa =FindAllMatches();
        if(aa.Count>0)
        {
            StartCoroutine(Handle_Extra_Matches(aa, 0.5f));
           

        }
    }

    List<GamePiece> FindAllMatches()
    {
        List<GamePiece> combinedMatches = new List<GamePiece>();
        bool matches_found = false;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (!m_allGamePieces[i,j].match_done)
                {
                    
                    List<GamePiece> matches = FindMatches(i, j, new Vector2(1, 0));
                    if (matches !=null) // then we find a match in the current row so we are done with this row and lets quit 
                    {
                        combinedMatches = combinedMatches.Union(matches).ToList();
                        matches_found = true;
                        break;
                    }
                   


                }

            }
            if (matches_found)
            {
                break;
            }
        }
         return combinedMatches;
        
    }
    IEnumerator Handle_Extra_Matches(List<GamePiece> matched_Game_pieces,float ti)
    {
        Tile a_tile_index_of_occured_match = m_allTiles[matched_Game_pieces[0].xIndex, matched_Game_pieces[0].yIndex];
        

        HighlightMatchesAt(a_tile_index_of_occured_match.xIndex, a_tile_index_of_occured_match.yIndex);
        yield return new WaitForSeconds(0.2f);
        
      

        Check_If_New_Matches_Occured();

    }


    

   
    bool isNextTo(Tile first,Tile second) // check if a tile is adjacent to another
    {

        int x_index_first = first.xIndex;
        int y_index_first = first.yIndex;


        int x_index_second = second.xIndex;
        int y_index_second = second.yIndex;

        
        if ((x_index_first == x_index_second) && (y_index_first == y_index_second + 1))
            return true;
        else if ((x_index_first == x_index_second) && (y_index_first == y_index_second -1))
            return true;
        else if ((x_index_first == x_index_second+1) && (y_index_first == y_index_second))
            return true;
        else if ((x_index_first == x_index_second - 1) && (y_index_first == y_index_second))
            return true;

        return false;


    }

    // it was like List<GamePiece> FindMatches(int startX, int startY, Vector2 searchDirection, int minLength) 
    List<GamePiece> FindMatches(int startX, int startY, Vector2 searchDirection) 
    {
        // running list of matching pieces
        List<GamePiece> matches = new List<GamePiece>();

        // start searching from this GamePiece
        GamePiece startPiece = null;

        // if our Tile coordinate is within the Board, get the corresponding GamePiece
        if (IsWithinBounds(startX, startY))
        {
            startPiece = m_allGamePieces[startX, startY];
        }

        // our starting piece is the first element of our matches list
        if (startPiece != null)
        {
            matches.Add(startPiece);
        }
        // if the Tile is empty, return null
        else
        {
            return null;
        }

        // coordinates for next tile to search
        int nextX;
        int nextY;

        // we can set our maximum search to the width or height of the Board, whichever is greater
        int maxValue = (width > height) ? width : height;
        //int maxValue = width;

        // start searching Tile at (startX, startY); increment depending on how we set our searchDirection
        for (int i = 1; i < maxValue ; i++) //-1 vardı byrda
        {
            nextX = startX + (int)Mathf.Clamp(searchDirection.x, -1, 1) * i; // x will be 1 in first call and -1 in second call  
            nextY = startY + (int)Mathf.Clamp(searchDirection.y, -1, 1) * i; // y is always zero since the search direction y comes as 0

            // if we hit the edge of the board, stop searching
            if (!IsWithinBounds(nextX, nextY))
            {
                break;
            }

            // get the correspond GamePiece to the (nextX, nextY) coordinate
            GamePiece nextPiece = m_allGamePieces[nextX, nextY];

            // if the next GamePiece has a matching value and is not already in our list
            if (nextPiece.matchValue == startPiece.matchValue && !matches.Contains(nextPiece))
            {
                matches.Add(nextPiece);
            }
            // we encounter a GamePiece that does not have a matching value or is already in our list, stop searching
            else
            {
                break;
            }
        }
        int wi = width;
        // if our list of matching pieces is greater than our minimum to be considered a match, return it
        if (matches.Count >= width) 
        {
            return matches;
        }

        // we don't have the minimum number of matches, return null
        return null;

    }


    void HighlightTileOff(int x, int y) 
    {
        SpriteRenderer spriteRenderer = m_allTiles[x, y].GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0);
    }

    void HighlightTileOn(int x, int y, Color col) // highlighting the matches one by one
    {
        SpriteRenderer spriteRenderer = m_allTiles[x, y].GetComponent<SpriteRenderer>();
        spriteRenderer.color = col;

        
    }

    void HighlightMatchesAt(int x, int y) //highlight matches if exists at that row
    {
        HighlightTileOff(x, y);
        
        var horizontal_matches = FindMatches(x, y, new Vector2(1, 0));

        if (horizontal_matches.Count > 0) // if any width-length matched found then the list will be bigger than 0  
        {
            foreach (GamePiece piece in horizontal_matches)
            {
                HighlightTileOn(piece.xIndex, piece.yIndex, piece.GetComponent<SpriteRenderer>().color);
               

                if(!m_allGamePieces[piece.xIndex, piece.yIndex].match_done) // check if this is a new match or not
                { 
                    piece.handleScore(); 
                }
                m_allGamePieces[piece.xIndex, piece.yIndex].match_done = true;

            }
        }
    }

    
  

}
