using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIButtonScript : MonoBehaviour
{
    public int currentselection;
    bool CanSelect;
    bool ButtonPressed;
    bool InCredits; 
    Button Play;
    Button Credits;
    Button Quit;

    GameObject PlayPanel;
    GameObject CreditPanel;

    ParticleSystem Particles;

	public AudioClip buttonClickSnd;
	public AudioClip buttonBackSnd;
	public AudioClip selectionMoveSnd;
	private bool audioPlayed;

	// Start is called before the first frame update
	void Start()
    {
        currentselection = 1;
        CanSelect = true;
        ButtonPressed = false;
        InCredits = false;
		audioPlayed = false;

        if (GameObject.Find("UI/Canvas/MainPanel/Buttons/Play")) {
            Play = GameObject.Find("UI/Canvas/MainPanel/Buttons/Play").GetComponent<Button>();
        }

        if(GameObject.Find("UI/Canvas/MainPanel/Buttons/Credits"))
        {
            Credits = GameObject.Find("UI/Canvas/MainPanel/Buttons/Credits").GetComponent<Button>();
        }

        if (GameObject.Find("UI/Canvas/MainPanel/Buttons/Quit"))
        {
            Quit = GameObject.Find("UI/Canvas/MainPanel/Buttons/Quit").GetComponent<Button>();
        }

        if (GameObject.Find("UI/Canvas/PlayPanel"))
        {
            PlayPanel = GameObject.Find("UI/Canvas/PlayPanel");
        }

        if (GameObject.Find("UI/Canvas/CreditsPanel"))
        {
            CreditPanel = GameObject.Find("UI/Canvas/CreditsPanel");
        }

        if(GameObject.Find("UI/Canvas/MainPanel/CrescentLogo/Particles"))
        {
            Particles = GameObject.Find("UI/Canvas/MainPanel/CrescentLogo/Particles").GetComponent<ParticleSystem>(); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        float moveDirection = Input.GetAxisRaw("Vertical");

		menuCursorMoveSounds(moveDirection, CanSelect);

		if (moveDirection == -1 && currentselection < 3 && CanSelect == true && ButtonPressed == false)
        {
            CanSelect = false;
            currentselection += 1;
            Debug.Log(currentselection);
			StartCoroutine(ResetCanSelect());
        }

        else if (moveDirection == 1 && currentselection > 1 && CanSelect == true && ButtonPressed == false && InCredits == false)
        {
            CanSelect = false;
            currentselection -= 1;
            Debug.Log(currentselection);
            StartCoroutine(ResetCanSelect());
        }

        if (currentselection == 1 && ButtonPressed == false)
        {
            Quit.animator.SetTrigger("Normal");
            Credits.animator.SetTrigger("Normal");
            Play.animator.SetTrigger("Highlighted");
        }

        else if (currentselection == 2 && ButtonPressed == false)
        {
            Quit.animator.SetTrigger("Normal");
            Play.animator.SetTrigger("Normal");
            Credits.animator.SetTrigger("Highlighted");
        }

        else if (currentselection == 3 && ButtonPressed == false)
        {
            Play.animator.SetTrigger("Normal");
            Credits.animator.SetTrigger("Normal");
            Quit.animator.SetTrigger("Highlighted");
        }

        if (currentselection == 1 && Input.GetButtonDown("Submit") && ButtonPressed == false && InCredits == false)
        {
            ButtonPressed = true;
            Play.animator.SetTrigger("Pressed");
            StartCoroutine(ResetButtonPress());

			AudioManager.instance.alterPitchEffect(buttonClickSnd, buttonClickSnd);

			PlayGame();
        }

        else if (currentselection == 2 && Input.GetButtonDown("Submit") && ButtonPressed == false)
        {
            ButtonPressed = true;
            InCredits = true;

			AudioManager.instance.alterPitchEffect(buttonClickSnd, buttonClickSnd);

			Credits.animator.SetTrigger("Pressed");
            StartCoroutine(ResetButtonPress());
            OpenCredits();

        }

        else if (currentselection == 3 && Input.GetButtonDown("Submit") && ButtonPressed == false && InCredits == false)
        {
            ButtonPressed = true;
            Quit.animator.SetTrigger("Pressed");

			AudioManager.instance.alterPitchEffect(buttonClickSnd, buttonClickSnd);

			StartCoroutine(ResetButtonPress());
            QuitGame(); 
        }

        if (InCredits == true && Input.GetKeyDown(KeyCode.JoystickButton2))
        {
            CloseCredits();
        }
    }

    public void QuitGame()
    {
        Application.Quit(); 
    }

    public void PlayGame()
    {
        StartCoroutine(StartGame());
        Particles.Stop();
    }

    public void OpenCredits()
    {
        CreditPanel.GetComponent<Animator>().SetTrigger("Slide");
        Particles.Stop();
    }

    public void CloseCredits()
    {
        CreditPanel.GetComponent<Animator>().SetTrigger("UnSlide");

		AudioManager.instance.alterPitchEffect(buttonBackSnd, buttonBackSnd);

		Particles.Play();
        InCredits = false;
    }
    
    IEnumerator StartGame()
    {
        PlayPanel.GetComponent<Animator>().SetTrigger("Slide");
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(1);
    }

    IEnumerator ResetButtonPress()
    {
        yield return new WaitForSeconds(0.1f);
        Play.animator.SetTrigger("Normal");
        Credits.animator.SetTrigger("Normal");
        Quit.animator.SetTrigger("Normal");
        ButtonPressed = false;
    }

        IEnumerator ResetCanSelect()
    {
        yield return new WaitForSeconds(0.2f);
        CanSelect = true;
    }

	void menuCursorMoveSounds(float moveDir, bool selectionState)
	{

		if (moveDir != 0 && !InCredits && !selectionState && !audioPlayed)
		{
			audioPlayed = true;
			AudioManager.instance.alterPitchEffect(selectionMoveSnd);
			audioPlayed = false;
		}
	}

}
