using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class MasterController : Singleton<MasterController>
{
    [Range(0, 1)]
    public float bombChance;
    [SerializeField]
    private int width = 10;
    [SerializeField]
    private int height = 18;
    [SerializeField]
    private GameObject tilePrefab;
    [SerializeField]
    private BombCount bombCountScript;

    private int bombCount;
    private float timer;
    private bool gameStarted = false;
    private TileController[,] tiles;
    private float holdTime = 0.5f;
    private float acumTime = 0;
    private bool tapDone = false;

    void Start()
    {
        InitializeTiles();
    }

    private void InitializeTiles()
    {
        tiles = new TileController[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                tiles[i,j] = Instantiate(tilePrefab, new Vector3(i, j, 0), Quaternion.identity).GetComponent<TileController>();
            }
        }
    }

    void Update()
    {
        if(!gameStarted && Input.GetMouseButtonDown(0))
        {
            gameStarted = true;
        }

        if(gameStarted)
        {
            Timer += Time.deltaTime;
        }
        

        if (Input.GetKeyDown("r"))
            SceneManager.LoadScene("MineSweeper");


        // Android touch control
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
                    acumTime = 0;
                    
                    
                }
            }

            if (Input.GetTouch(0).phase == TouchPhase.Ended && acumTime > holdTime)
            {
                acumTime = 0;
                tapDone = false;
            }
        }


        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

    }

    private void UncoverMines()
    {
        foreach (TileController tile in tiles)
        {
            tile.LoadTexture();
        }
        gameStarted = false;
    }

    public void GameOver()
    {
        gameStarted = false;
        UncoverMines();
        Debug.Log("Game over");
    }

    public void UncoverBlanks(int x, int y)
    {
        if (tiles[x, y].TileState == TileState.UNCOVERED || tiles[x,y].TileType == TileType.BOMB)
        {
            return;
        }

        tiles[x,y].LoadTexture();

        if (SetCount(x, y) > 0)
            return;

        if( y < height - 1) UncoverBlanks(x,y+1);
        if( y != 0) UncoverBlanks(x, y-1);
        if( x < width - 1) UncoverBlanks(x+1, y);
        if( x != 0) UncoverBlanks(x-1, y);
        if( x < width - 1 && y < height - 1) UncoverBlanks(x+1,y+1);
        if( x < width - 1 && y != 0) UncoverBlanks(x+1,y-1);
        if( x != 0 && y != 0 ) UncoverBlanks(x-1, y-1);
        if( x != 0 && y < height - 1) UncoverBlanks(x-1,y+1);
    }

    public bool CheckIfTileIsBomb(int x, int y)
    {
        if(tiles[x,y].TileType == TileType.BOMB)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public int SetCount(int x, int y)
    {
        int count = 0;
        
        if( y < height - 1 ) if (CheckIfTileIsBomb(x, y + 1)) ++count; 
        if( x < width-1 && y < height - 1) if (CheckIfTileIsBomb(x + 1, y + 1)) ++count;
        if( x < width-1) if (CheckIfTileIsBomb(x + 1, y)) ++count;
        if( x < width - 1 && y != 0)if (CheckIfTileIsBomb(x + 1, y - 1)) ++count;
        if( y != 0) if (CheckIfTileIsBomb(x, y - 1)) ++count;
        if( x != 0 && y != 0 ) if (CheckIfTileIsBomb(x - 1, y - 1)) ++count;
        if( x != 0)if (CheckIfTileIsBomb(x - 1, y)) ++count;
        if (x != 0 && y < height - 1) if (CheckIfTileIsBomb(x - 1, y + 1)) ++count;

        return count;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    #region Properties
    public float Timer
    {
        get
        {
            return timer;
        }
        private set
        {
            timer = value;
        }
    }

    public int BombCount
    {
        get
        {
            return bombCount;
        }
        set
        {
            bombCount = value;
            bombCountScript.UpdateBombCount(BombCount);
        }
    }
    #endregion
}
