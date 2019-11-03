using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCMonologueTrigger : MonoBehaviour
{
    public Monologue monologue;

    public GameObject MainMonologuePanel;
    public GameObject FindPlayer; 

    const float MaxLerpTimer = 3.0f;
    float OpenedMonologuePos;
    float ClosedMonologuePos;

    public bool MonologueIsOpen;
    bool isClosingMonologue; 

    void Awake()
    {
       MainMonologuePanel = GameObject.Find("UI/Monologue/MainPanel");
       FindPlayer = GameObject.Find("Player");
    }

    void Start()
    {
        OpenedMonologuePos = 65.0f;
        ClosedMonologuePos = -65.0f;
        MonologueIsOpen = false;
        isClosingMonologue = false; 
    }

    void LateUpdate()
    {
       float distancePlrAndNpc = gameObject.transform.position.x - FindPlayer.transform.position.x;

        if (distancePlrAndNpc > 10.0f || distancePlrAndNpc < -4.0f) 
        {
            if (isClosingMonologue == false)
            {
                isClosingMonologue = true;
                StartCoroutine(CloseMonologuePanel());
            }
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && Input.GetButtonDown("Submit") && MonologueIsOpen == false)
        {
            MonologueIsOpen = true;
            Destroy(GameObject.Find("MapUIPopUp"));
            InitiateMonologue();
            StartCoroutine(OpenMonologuePanel());
        }
    }

    public void InitiateMonologue()
    {
        FindObjectOfType<MonologueManager>().BeginMonologue(monologue);

    }

    IEnumerator OpenMonologuePanel()
    {
        MainMonologuePanel.SetActive(true);

        float elapsedTime = 0.0f;

        while (elapsedTime < MaxLerpTimer)
        {
            MainMonologuePanel.transform.position = new Vector3(MainMonologuePanel.transform.position.x, Mathf.Lerp(MainMonologuePanel.transform.position.y, OpenedMonologuePos, (elapsedTime / MaxLerpTimer)), 0.0f);
            elapsedTime += Time.deltaTime;

            yield return null;
        }
    }

    public IEnumerator CloseMonologuePanel()
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < MaxLerpTimer)
        {
            MainMonologuePanel.transform.position = new Vector3(MainMonologuePanel.transform.position.x, Mathf.Lerp(MainMonologuePanel.transform.position.y, ClosedMonologuePos, (elapsedTime / 0.5f)), 0.0f);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        MonologueIsOpen = false;
        isClosingMonologue = false;
        MainMonologuePanel.SetActive(false);
    }
}
