using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EndingGame()); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator EndingGame()
    {
        yield return new WaitForSeconds(6.5f);
        SceneManager.LoadScene("TitleScreen");
    }
}
