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
    Animator DoorAnim; 

    void Awake()
    {
        InfoText = GameObject.Find("UI/Canvas/Info").GetComponent<TextMeshProUGUI>();  

        if (GameObject.Find("Gate/Frame"))
        {
            Door = GameObject.Find("Gate/Frame");
            DoorAnim = Door.GetComponent<Animator>(); 
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        List<string> ColorsInList = new List<string>();
        ColorsInList.Add("Color.red");
        ColorsInList.Add("Color.green");
        ColorsInList.Add("Color.blue");

        PuzzleColorsAnswer = new string[3];

        for (int i = 0; i < PuzzleColorsAnswer.Length; i++)
        {
            int currentSelection = Random.Range(0, ColorsInList.Count);
            PuzzleColorsAnswer[i] = ColorsInList[currentSelection].ToString();
            ColorsInList.RemoveAt(currentSelection);

            Debug.Log(PuzzleColorsAnswer[i]);
        }

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
