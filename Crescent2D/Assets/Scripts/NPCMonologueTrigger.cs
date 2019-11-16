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

    Animator MonologueAnim; 

    const float MaxLerpTimer = 3.0f;
    float OpenedMonologuePos;
    float ClosedMonologuePos;

    public bool MonologueIsOpen;
    bool isClosingMonologue;
    bool playerCanInteract;

    bool isAnimating; 

    void Awake()
    {
       MainMonologuePanel = GameObject.Find("UI/Monologue/MainPanel");
       FindPlayer = GameObject.Find("Player");
       MonologueAnim = MainMonologuePanel.GetComponent<Animator>();
    }

    void Start()
    {
        OpenedMonologuePos = 65.0f;
        ClosedMonologuePos = -65.0f;
        MonologueIsOpen = false;
        isClosingMonologue = false;
        isAnimating = false; 
    }

    void LateUpdate()
    {
        float distancePlrAndNpc = gameObject.transform.position.x - FindPlayer.transform.position.x;
        if (distancePlrAndNpc > 10.0f || distancePlrAndNpc < -4.0f)
        {
            if (isClosingMonologue == false && isAnimating == false)
            {
                isClosingMonologue = true;
                StartCoroutine(CloseMonologuePanel());
            }
        }
    }

    void Update()
    {
        if (playerCanInteract == true && Input.GetButtonDown("Submit"))
        {
            playerCanInteract = false;
            isAnimating = true; 
            MonologueIsOpen = true;
            Destroy(GameObject.Find("MapUIPopUp"));
            InitiateMonologue();
            StartCoroutine(OpenMonologuePanel());
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && MonologueIsOpen == false && playerCanInteract == false)
        {
            playerCanInteract = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        playerCanInteract = false; 
    }

    public void InitiateMonologue()
    {
        FindObjectOfType<MonologueManager>().BeginMonologue(monologue);
    }


    IEnumerator OpenMonologuePanel()
    {
        yield return new WaitForSeconds(0.15f);
        MainMonologuePanel.SetActive(true);
        MonologueAnim.SetBool("isOpen", MonologueIsOpen);

        yield return new WaitForSeconds(0.5f);
        isAnimating = false;
    }

    public IEnumerator CloseMonologuePanel()
    {
        yield return new WaitForSeconds(0.15f);
        MonologueIsOpen = false;
        isClosingMonologue = false;
        MonologueAnim.SetBool("isOpen", MonologueIsOpen);


        yield return new WaitForSeconds(1.0f);
        MainMonologuePanel.SetActive(false);
    }
}
