using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterBoardMovement : MonoBehaviour
{

    float WalkSpeed; 
    // Start is called before the first frame update
    void Start()
    {
        WalkSpeed = 12.0f; 
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f);
        transform.position += movement * WalkSpeed * Time.deltaTime;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
       if(collision.tag == "Level" && Input.GetButtonDown("Submit"))
        {
            Debug.Log("Player hit Enter");
            SceneManager.LoadScene(collision.gameObject.name);
        }

       else if (collision.tag == "Tutorial" && Input.GetButtonDown("Submit"))
        {
           SceneManager.LoadScene("Tutorial");
        }
    }
}
