using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    Animator anim;

    string color;
    public string[] GuessArray;

    Color defaultPuzzleColor;
    Color newColor;

    GameObject Circle1;
    GameObject Circle2;
    GameObject Circle3;

    bool stillOnButton; 

    void Awake()
    {
        if (GameObject.Find("Puzzle/Color1"))
        {
            Circle1 = GameObject.Find("Puzzle/Color1");
        }

        if (GameObject.Find("Puzzle/Color2"))
        {
            Circle2 = GameObject.Find("Puzzle/Color2");
        }

        if (GameObject.Find("Puzzle/Color3"))
        {
            Circle3 = GameObject.Find("Puzzle/Color3");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        defaultPuzzleColor = new Color(0.0f, 0.0f, 0.0f);

        GuessArray = GameObject.Find("Gate").GetComponent<DoorColorPuzzle>().PuzzleGuess;

        stillOnButton = false; 

        if (gameObject.tag == "RedButton")
        {
            color = "Color.red";
            newColor = gameObject.GetComponent<SpriteRenderer>().color;
        }
        
        else if (gameObject.tag == "GreenButton")
        {
            color = "Color.green";
            newColor = gameObject.GetComponent<SpriteRenderer>().color;
        }

        else if (gameObject.tag == "BlueButton")
        {
            color = "Color.blue";
            newColor = gameObject.GetComponent<SpriteRenderer>().color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GuessArray.Length > 0  && GuessArray[0] != null && GuessArray[1] != null && GuessArray[2] != null)
        {
            StartCoroutine(ResetColors());
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            float heightCheck = collision.gameObject.transform.position.y - gameObject.transform.position.y;

            if (heightCheck > 1.0f)
            {
                stillOnButton = true; 
                anim.SetBool("ButtonPress", true);
                StartCoroutine(ButtonPressCoroutine());        
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        anim.SetBool("ButtonPress", false);
        stillOnButton = false; 
    }

    IEnumerator ButtonPressCoroutine()
    {
        yield return new WaitForSeconds(0.45f);

        if (stillOnButton == true)
        {
            if (GuessArray[0] == null)
            {
                GuessArray[0] = color;
                Circle1.gameObject.GetComponent<SpriteRenderer>().color = newColor;
            }

            else if (color != GuessArray[0] && GuessArray[0] != null && GuessArray[1] == null)
            {
                GuessArray[1] = color;
                Circle2.gameObject.GetComponent<SpriteRenderer>().color = newColor;
            }

            else if (GuessArray[0] != null && color != GuessArray[1] && GuessArray[1] != null && GuessArray[2] == null)
            {
                GuessArray[2] = color;
                Circle3.gameObject.GetComponent<SpriteRenderer>().color = newColor;
            }

            else if (GuessArray[0] != null && GuessArray[1] != null && GuessArray[2] != null)
            {
                StartCoroutine(ResetColors());
            }
        }
    } 

    IEnumerator ResetColors()
    {
        yield return new WaitForSeconds(0.25f);

        for (int i = 0; i < 3; i++)
        {
            GuessArray[i] = null;
        }

        Circle1.gameObject.GetComponent<SpriteRenderer>().color = defaultPuzzleColor;
        Circle2.gameObject.GetComponent<SpriteRenderer>().color = defaultPuzzleColor;
        Circle3.gameObject.GetComponent<SpriteRenderer>().color = defaultPuzzleColor;
    }
}
