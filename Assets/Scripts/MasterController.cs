using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MasterController : MonoBehaviour
{

    public GameObject tile;

    private static int w = 10;
    private static int h = 18;

    private static int axes = 2;
    private static int directions = 10;

    private int[,] _dir = new int[axes, directions];


    public static TileController[,] tiles = new TileController[w,h];

    public static bool isGameOver = true;

    private float holdTime = 0.5f;
    private float acumTime = 0;

    private bool tapDone = false;

    // Start is called before the first frame update
    void Start()
    {


        for(float i=0 ; i< 10; i++)
            for(float j=0; j<18; j++)
                    Instantiate(tile, new Vector3(i,j,0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("r"))
            SceneManager.LoadScene("MineSweeper");



        
        if(Input.touchCount > 0)
        {

            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);

            

            acumTime += Input.GetTouch(0).deltaTime;

            if (acumTime >= holdTime && !tapDone)
            {
                if (hit.collider != null)
                {
                    Debug.Log(hit.collider.name);
                    hit.collider.gameObject.SendMessage("MakeQuestion");
                    
                    tapDone = true;
                }
            }
            if (Input.GetTouch(0).phase == TouchPhase.Ended && acumTime < holdTime && !tapDone)
            {
                if (hit.collider != null)
                {
                    Debug.Log(hit.collider.name);
                    hit.collider.gameObject.SendMessage("TouchDown");
                    
                    
                }
            }

            if (Input.GetTouch(0).phase == TouchPhase.Ended && acumTime > holdTime)
            {
                acumTime = 0;
                tapDone = false;
            }
                


        }


        /* if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);

            if (hit.collider != null)
            {
                Debug.Log(hit.collider.name);
                hit.collider.gameObject.SendMessage("TouchDown");
            }
        }*/
            





        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

    }

    public static void UncoverMines()
    {

        foreach (TileController tile in tiles)
        {
            int x = (int) tile.transform.position.x;
            int y = (int) tile.transform.position.x;


            if (tile.isMine)
                tile.LoadTexture();
            else
                tile.LoadTexture();
        }
    }

    public static void UncoverBlanks(int x, int y)
    {
        
        
        if (tiles[x, y].isUncovered || tiles[x,y].isMine )
            return;

        tiles[x,y].LoadTexture();

        if (SetCount(x, y) > 0)
            return;


        if( y < h-1) UncoverBlanks(x,y+1);
        if( y != 0) UncoverBlanks(x, y-1);
        if( x < w-1) UncoverBlanks(x+1, y);
        if( x != 0) UncoverBlanks(x-1, y);
        if( x < w-1 && y < h-1) UncoverBlanks(x+1,y+1);
        if( x < w-1 && y != 0) UncoverBlanks(x+1,y-1);
        if( x != 0 && y != 0 ) UncoverBlanks(x-1, y-1);
        if( x != 0 && y < h-1) UncoverBlanks(x-1,y+1);
    }
    
    
    
    public static void GameOver()
    {
        // isGameOver = false;
    }
    

    public static bool mineAt(int x, int y)
    {
        return tiles[x, y].isMine;
    }
    
    public static int SetCount(int x, int y)
    {
        int count = 0;
        
        if( y < h-1 ) if (mineAt(x, y + 1)) ++count; 
        if( x < w-1 && y < h-1) if (mineAt(x + 1, y + 1)) ++count;
        if( x < w-1) if (mineAt(x + 1, y)) ++count;
        if( x < w-1 && y != 0)if (mineAt(x + 1, y - 1)) ++count;
        if( y != 0) if (mineAt(x, y - 1)) ++count;
        if( x != 0 && y != 0 ) if (mineAt(x - 1, y - 1)) ++count;
        if( x != 0)if (mineAt(x - 1, y)) ++count;
        if( x != 0 && y < h-1) if (mineAt(x - 1, y + 1)) ++count;
        

        return count;
    }
    
}
