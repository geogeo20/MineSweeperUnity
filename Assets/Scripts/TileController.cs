using UnityEngine;
using Random = UnityEngine.Random;

public enum TileState
{
    COVERED,
    UNCOVERED,
    MARKED
};

public enum TileType
{
    BOMB,
    NORMAL
};

public class TileController : MonoBehaviour
{
    [SerializeField]
    private Sprite[] sprites = null;
    [SerializeField]
    private Sprite bombSprite = null;
    [SerializeField]
    private Sprite markSprite = null;
    [SerializeField]
    private Sprite defaultSprite = null;

    private SpriteRenderer spriteComponent;
    private MasterController masterController;

    private TileState tileState = TileState.COVERED;
    private TileType tileType;
    private Vector2Int gridPosition;

    void Start()
    {
        masterController = MasterController.Instace;
        spriteComponent = GetComponent<SpriteRenderer>();
        gridPosition = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        RandomizeTile();
    }

    private void RandomizeTile()
    {
        if(Random.value < masterController.bombChance)
        {
            TileType = TileType.BOMB;
            masterController.BombCount++;
        }
        else
        {
            TileType = TileType.NORMAL;
        }
    }

    private void OnMouseOver()
    {
        if (TileState != TileState.UNCOVERED && Input.GetMouseButtonDown(1))
        {
            MarkTile();
        }
    }

    public void MarkTile()
    {
        if (TileState == TileState.COVERED)
        {
            spriteComponent.sprite = markSprite;
            tileState = TileState.MARKED;
            masterController.BombCount--;
        }
        else if(TileState == TileState.MARKED)
        {
            spriteComponent.sprite = defaultSprite;
            tileState = TileState.COVERED;
            masterController.BombCount++;
        }
    }

    public void LoadTexture()
    {
        if (TileType == TileType.BOMB)
        {
            spriteComponent.sprite = bombSprite;
        }
        else
        {
            spriteComponent.sprite = sprites[masterController.SetCount(gridPosition.x, gridPosition.y)];
            TileState = TileState.UNCOVERED;
        }
    }

    private void TouchMovement()
    {
        if (Input.touchCount <= 0 || Input.GetTouch(0).phase != TouchPhase.Began)
        {
            return;
        }

        if (TileType == TileType.BOMB)
        {
            masterController.GameOver();
        }
        else
        {
            int x = (int)transform.position.x;
            int y = (int)transform.position.y;
            LoadTexture();
            if (masterController.SetCount(x, y) == 0)
            {
                masterController.UncoverBlanks(x, y);
            }
        }
    }

    public void TouchDown()
    {
        if (TileType == TileType.BOMB && TileState != TileState.MARKED)
        {
            masterController.GameOver();
        }
        else if (TileState != TileState.MARKED)
        {
            if (masterController.SetCount(gridPosition.x, gridPosition.y) == 0)
            {
                masterController.UncoverBlanks(gridPosition.x, gridPosition.y);
            }
            else
            {
                LoadTexture();
            }
        }
    }


    private void OnMouseUp()
    {
        if (TileState != TileState.MARKED)
        {
            if (TileType == TileType.BOMB)
            {
                masterController.GameOver();
            }
            else
            {
                if (masterController.SetCount(gridPosition.x, gridPosition.y) == 0)
                {
                    masterController.UncoverBlanks(gridPosition.x, gridPosition.y);
                }
                else
                {
                    LoadTexture();
                }
            }
        }
    }

    #region Properties
    public TileState TileState
    {
        get
        {
            return tileState;
        }
        private set
        {
            tileState = value;
        }
    }

    public TileType TileType
    {
        get
        {
            return tileType;
        }
        private set
        {
            tileType = value;
        }
    }
    #endregion

}
