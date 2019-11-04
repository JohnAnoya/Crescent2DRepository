using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class CharacterBoardMovement : MonoBehaviour
{

    TextMeshProUGUI Title;
    TextMeshProUGUI Level;

    public Transform UIPopUp; 

    float WalkSpeed;

    bool UIOpen;

    void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
        WalkSpeed = 12.0f;
        UIOpen = false; 
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f);
        transform.position += movement * WalkSpeed * Time.deltaTime;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (UIOpen == false)
        {
            UIOpen = true; 

            var SpawnedUI = Instantiate(UIPopUp, new Vector3(gameObject.transform.position.x + 7.0f, gameObject.transform.position.y + 7.0f, UIPopUp.transform.position.z), gameObject.transform.rotation);
            SpawnedUI.parent = GameObject.Find("UI").transform;
            SpawnedUI.name = "MapUIPopUp";

            Title = GameObject.Find("UI/MapUIPopUp/Panel/Title").GetComponent<TextMeshProUGUI>();
            Level = GameObject.Find("UI/MapUIPopUp/Panel/Level").GetComponent<TextMeshProUGUI>();

            Level.text = collision.gameObject.name;
            Title.text = collision.gameObject.transform.Find("Title").GetComponent<Text>().text;
        }


        if (collision.tag == "Level" && Input.GetButtonDown("Submit") || collision.tag == "Level" && Input.GetKey(KeyCode.Return))
        {
            Debug.Log("Player hit Enter");
            SceneManager.LoadScene(collision.gameObject.name);
        }

       else if (collision.tag == "Tutorial" && Input.GetButtonDown("Submit") || collision.tag == "Tutorial" && Input.GetKey(KeyCode.Return))
        {
           SceneManager.LoadScene("Tutorial");
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        Destroy(GameObject.Find("MapUIPopUp"));
        UIOpen = false; 
    }
}
