using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Collections.Generic;
public class DoorColorPuzzle : MonoBehaviour
{
    public string[] PuzzleColorsAnswer;
    public string[] PuzzleGuess; 

    TextMeshProUGUI InfoText;

    GameObject Door;
    GameObject[] CircleClue;
    

    Animator DoorAnim; 

    void Awake()
    {
        InfoText = GameObject.Find("UI/Canvas/Info").GetComponent<TextMeshProUGUI>();

        CircleClue = new GameObject[3]; 

        if (GameObject.Find("Gate/Frame"))
        {
            Door = GameObject.Find("Gate/Frame");
            DoorAnim = Door.GetComponent<Animator>(); 
        }

        if (GameObject.Find("Puzzle/Clues/Color1"))
        {
            CircleClue[0] = GameObject.Find("Puzzle/Clues/Color1"); 
        }

        if (GameObject.Find("Puzzle/Clues/Color2"))
        {
            CircleClue[1] = GameObject.Find("Puzzle/Clues/Color2");
        }

        if (GameObject.Find("Puzzle/Clues/Color3"))
        {
            CircleClue[2] = GameObject.Find("Puzzle/Clues/Color3");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
       PuzzleColorsAnswer = new string[3];
       RandomizeColors(); 

       PuzzleGuess = new string[3];

        PuzzleGuess[0] = null;
        PuzzleGuess[1] = null;
        PuzzleGuess[2] = null;
    }

    void RandomizeColors()
    {
        List<string> ColorsInList = new List<string>();
        List<string> ReversedAnswers = new List<string>(); 

        ColorsInList.Add("Color.red");
        ColorsInList.Add("Color.green");
        ColorsInList.Add("Color.blue");

        for (int i = 0; i < PuzzleColorsAnswer.Length; i++)
        {
            int currentSelection = Random.Range(0, ColorsInList.Count);
            PuzzleColorsAnswer[i] = ColorsInList[currentSelection].ToString();
            ColorsInList.RemoveAt(currentSelection);

            Debug.Log(PuzzleColorsAnswer[i]);
        }

        ReversedAnswers = PuzzleColorsAnswer.Reverse().ToList();
 
        for (int j = 2; j > -1; j--)
        {
            if (ReversedAnswers[j] == "Color.red")
            {
                CircleClue[j].GetComponent<SpriteRenderer>().color = Color.red;
            }

            else if (ReversedAnswers[j] == "Color.green")
            {
                CircleClue[j].GetComponent<SpriteRenderer>().color = Color.green;
            }

            else if (ReversedAnswers[j] == "Color.blue")
            {
                CircleClue[j].GetComponent<SpriteRenderer>().color = Color.blue;
            }
        }

        Debug.Log("Reversed Color answers are: " + ReversedAnswers[0]);
        Debug.Log("Reversed Color answers are: " + ReversedAnswers[1]);
        Debug.Log("Reversed Color answers are: " + ReversedAnswers[2]);
    }

    // Update is called once per frame
    void Update()
    {
        if (PuzzleGuess.SequenceEqual(PuzzleColorsAnswer))
        {
            Debug.Log("Player has solved the puzzle!");
            InfoText.text = "CONGRATULATIONS, you have solved the puzzle!";
            InfoText.color = Color.green; 
            StartCoroutine(ResetText());
            DoorAnim.SetBool("DoorIsOpen", true);
        }

        else
        {
            if (PuzzleGuess[0] != null && PuzzleGuess[1] != null && PuzzleGuess[2] != null)
            {
                InfoText.text = "Code INCORRECT, refer back to the clues!";
                InfoText.color = Color.red;
                StartCoroutine(ResetText());
            }
        }
    }

    IEnumerator ResetText()
    {
        yield return new WaitForSeconds(3.5f);
        InfoText.text = "";
    }

}
