using UnityEngine;
using Random = UnityEngine.Random;

public class TileController : MonoBehaviour
{
    public Sprite[] sprites;
    public Sprite bombSprite;
    public Sprite markSprite;
    public Sprite defaultSprite;

    private SpriteRenderer spriteComponent;

    [HideInInspector]
    public bool isMine;
    [HideInInspector]
    public bool isUncovered = false;
    private bool isQuestion = false;

    void Start()
    {
        isMine = Random.value < 0.20;
        var position = transform.position;
        int x = (int) position.x;
        int y = (int) position.y;
        MasterController.tiles[x, y] = this;

        spriteComponent = GetComponent<SpriteRenderer>();
    }

    private void OnMouseOver()
    {
        if(!isUncovered && Input.GetMouseButtonDown(1))
        {
            MarkTile();
        }
    }

    public void MarkTile()
    {
        if (!isQuestion)
        {
            spriteComponent.sprite = markSprite;
        }
        else
        {
            spriteComponent.sprite = defaultSprite;
        }

        isQuestion = !isQuestion;
    }

    public void LoadTexture()
    {
        int x = (int) transform.position.x;
        int y = (int) transform.position.y;
        if(isMine)
        {
            spriteComponent.sprite = bombSprite;
        }
        else
        {
            spriteComponent.sprite = sprites[MasterController.SetCount(x,y)];
            isUncovered = true;
        }
    }

    public bool IsCovered()
    {
        return GetComponent<SpriteRenderer>().sprite.texture.name == "Default";
    }

    private void TouchMovement()
    {
        if (Input.touchCount <= 0 || Input.GetTouch(0).phase != TouchPhase.Began) return;
        if (isMine)
        {
            MasterController.UncoverMines();
            print("You lose!");
        }
        else
        {
            int x = (int) transform.position.x;
            int y = (int) transform.position.y;
            LoadTexture();
            if (MasterController.SetCount(x, y) == 0)
                MasterController.UncoverBlanks(x, y);
            print(MasterController.SetCount(x, y));
        }
    }

    public void TouchDown()
    {
        if (isMine && !isQuestion)
        {

            MasterController.UncoverMines();
            print("You lose!");
        }
        else if( isQuestion == false)
        {
            int x = (int)transform.position.x;
            int y = (int)transform.position.y;

            if (MasterController.SetCount(x, y) == 0)
            {
                print("Bagami-as pla in mata de methoda infecta");
                MasterController.UncoverBlanks(x, y);
            }
            else
                LoadTexture();
            print(MasterController.SetCount(x, y));
        }
    }

    
    private void OnMouseUp()
    {
        if(!isQuestion)
        {
            if (isMine)
            {
                MasterController.UncoverMines();
                Debug.Log("You lose!");
            }
            else
            {
                int x = (int)transform.position.x;
                int y = (int)transform.position.y;

                if (MasterController.SetCount(x, y) == 0)
                {
                    MasterController.UncoverBlanks(x, y);
                }
                else
                {
                    LoadTexture();
                }
            }
        }
    }
}
