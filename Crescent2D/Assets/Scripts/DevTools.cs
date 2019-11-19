using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DevTools : MonoBehaviour
{
	private bool devMode;

	[SerializeField]
	private int desiredLevelIndex;

	private void Start()
	{
		devMode = true;
		desiredLevelIndex = 0;
	}

	// Update is called once per frame
	void Update()
    {
		ResetGame();
    }

	private void ResetGame()
	{
		if (devMode && Input.GetKeyDown(KeyCode.O))
		{
			SceneManager.LoadScene(desiredLevelIndex);
		}
	}
}
