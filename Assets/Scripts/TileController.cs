using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using UnityEngineInternal;
using Random = UnityEngine.Random;

public class TileController : MonoBehaviour
{
    public Sprite[] sprites;
    public Sprite mine;
    public Sprite qustion;
    public Sprite defaultSprite;

    public int setCount;
    
    public bool isMine;

    public bool isUncovered = false;
    // Start is called before the first frame update


    private bool isQuestion = false;

    void Start()
    {
        isMine = Random.value < 0.20;
        var position = transform.position;
        int x = (int) position.x;
        int y = (int) position.y;
      

        MasterController.tiles[x, y] = this;

        

    }

    // Update is called once per frame
    void Update()
    {

        // TouchMovement();
        if (Input.GetMouseButton(1))
            MakeQuestion();
            
        
    }

    public void MakeQuestion()
    {
        if(!isUncovered)
        {
            if (isQuestion == false)

            {
                GetComponent<SpriteRenderer>().sprite = qustion;
                isQuestion = !isQuestion;
                Handheld.Vibrate();
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = defaultSprite;
                isQuestion = !isQuestion;
                Handheld.Vibrate();
            }
        }
        
            
       
    }

    public void LoadTexture()
    {
        int x = (int) transform.position.x;
        int y = (int) transform.position.y;
        if(isMine)
            GetComponent<SpriteRenderer>().sprite = mine;
        else
        {
            GetComponent<SpriteRenderer>().sprite = sprites[MasterController.SetCount(x,y)];
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
            MasterController.GameOver();
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
            MasterController.GameOver();
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

    /*
    private void OnMouseUp()
    {
        if (isMine)
        {

            MasterController.UncoverMines();
            MasterController.GameOver();
            print("You lose!");
        }
        else
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
    */




    private void OnMouseDown()
    {
        // if(MasterController.isGameOver)
        
    }
    
}
