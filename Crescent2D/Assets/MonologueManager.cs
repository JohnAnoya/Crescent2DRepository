using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MonologueManager : MonoBehaviour
{

    private Queue<string> MonologueSentences;

    TextMeshProUGUI MonologueNameText;
    TextMeshProUGUI MonologueText;

    void Awake()
    {
        MonologueNameText = GameObject.Find("UI/Monologue/MainPanel/Name").GetComponent<TextMeshProUGUI>();
        MonologueText = GameObject.Find("UI/Monologue/MainPanel/Info").GetComponent<TextMeshProUGUI>();
    }

    // Start is called before the first frame update
    void Start()
    {
        MonologueSentences = new Queue<string>();
    }

    void Update()
    {
       if (Input.GetButtonDown("NextSentence") && GameObject.Find("NPC").GetComponent<NPCMonologueTrigger>().MonologueIsOpen == true)
        {
            ShowNextSentence();
        }
    }

    public void BeginMonologue(Monologue monologue)
    {
        MonologueNameText.text = monologue.NPCName;

        MonologueSentences.Clear();

        foreach (string MonologueSentence in monologue.Sentences)
        {
            MonologueSentences.Enqueue(MonologueSentence);
        }

        ShowNextSentence(); 
    }

    public void ShowNextSentence()
    {
        if (MonologueSentences.Count == 0)
        {
            Debug.Log("No monologue to display!");
            StartCoroutine(GameObject.Find("NPC").GetComponent<NPCMonologueTrigger>().CloseMonologuePanel());
            return;
        }

        string Sentence = MonologueSentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentenceCharacters(Sentence));
    }

    IEnumerator TypeSentenceCharacters(string sentenceToType)
    {
        MonologueText.text = "";
        foreach (char characters in sentenceToType.ToCharArray())
        {
            MonologueText.text += characters;
            yield return new WaitForSeconds(0.03f);
        }
    }
}
