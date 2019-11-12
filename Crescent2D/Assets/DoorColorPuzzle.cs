using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class DoorColorPuzzle : MonoBehaviour
{
    public string[] PuzzleColorsAnswer;
    public string[] PuzzleGuess;

    TextMeshProUGUI InfoText;

    void Awake()
    {
        InfoText = GameObject.Find("UI/Canvas/Info").GetComponent<TextMeshProUGUI>();  
    }

    // Start is called before the first frame update
    void Start()
    {
        PuzzleColorsAnswer = new string[3];

        PuzzleColorsAnswer[0] = "Color.blue";
        PuzzleColorsAnswer[1] = "Color.red";
        PuzzleColorsAnswer[2] = "Color.green";

        PuzzleGuess = new string[3];

        PuzzleGuess[0] = null;
        PuzzleGuess[1] = null;
        PuzzleGuess[2] = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (PuzzleGuess.SequenceEqual(PuzzleColorsAnswer))
        {
            Debug.Log("Player has solved the puzzle!");
            InfoText.text = "Congratulations, you have solved the puzzle!";
            InfoText.color = Color.green; 
            StartCoroutine(ResetText());
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
