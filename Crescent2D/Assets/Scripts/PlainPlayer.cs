using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlainPlayer : MonoBehaviour
{
    private Animator anim;

    TextMeshProUGUI Title;
    TextMeshProUGUI Level;

    public Transform UIPopUp;
    public Transform camera;

    float WalkSpeed;

    bool UIOpen;
    bool CameraFollowPlayer;
    public bool PlayerCanMove;
    bool isFacingRight;

    void Awake()
    {
        camera = Camera.main.transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
        WalkSpeed = 10.0f;
        anim = GetComponent<Animator>();
        CameraFollowPlayer = true;
        PlayerCanMove = true;
        isFacingRight = true;
    }

    // Update is called once per frame
    void Update()
    {
        float moveDirection = Input.GetAxis("Horizontal") + Input.GetAxis("Vertical");

        if (PlayerCanMove == true)
        {
            Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f);
            transform.position += movement * WalkSpeed * Time.deltaTime;
        }
       
        if (anim)
        {
          anim.SetFloat("Speed", Mathf.Abs(moveDirection));
        }

        if ((moveDirection < 0 && isFacingRight) || moveDirection > 0 && !isFacingRight)
        {
            isFacingRight = !isFacingRight;
            gameObject.GetComponent<SpriteRenderer>().flipX = !gameObject.GetComponent<SpriteRenderer>().flipX;
        }

            CameraFollow();
    }

    void CameraFollow()
    {
        if (CameraFollowPlayer == true)
        {
            camera.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, camera.transform.position.z);
        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Exit")
        {
            collision.gameObject.GetComponent<SpriteRenderer>().color = new Color(255.0f, 0.0f, 0.0f);
        }
    }


    void OnTriggerStay2D(Collider2D collision)
    {
        if (UIOpen == false)
        {
            UIOpen = true;

            var SpawnedUI = Instantiate(UIPopUp, new Vector3(gameObject.transform.position.x + 3.0f, gameObject.transform.position.y + 3.0f, UIPopUp.transform.position.z), gameObject.transform.rotation);
            SpawnedUI.parent = GameObject.Find("UI").transform;
            SpawnedUI.name = "MapUIPopUp";

            Title = GameObject.Find("UI/MapUIPopUp/Panel/Title").GetComponent<TextMeshProUGUI>();
            Level = GameObject.Find("UI/MapUIPopUp/Panel/Level").GetComponent<TextMeshProUGUI>();

            Level.text = collision.gameObject.name;
            Title.text = collision.gameObject.transform.Find("Title").GetComponent<Text>().text;
        }


        if (collision.gameObject.tag == "Exit" && Input.GetButtonDown("Submit") && SceneManager.GetActiveScene().buildIndex < 6)
        {
            Destroy(GameObject.Find("MapUIPopUp"));
            SceneManager.LoadScene("Map1");
        }

        else if (collision.tag == "Tutorial" && Input.GetButtonDown("Submit"))
        {
            Destroy(GameObject.Find("MapUIPopUp"));
            SceneManager.LoadScene("Tutorial");
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        Destroy(GameObject.Find("MapUIPopUp"));
        UIOpen = false;

        if (collision.gameObject.tag == "Exit")
        {
            collision.gameObject.GetComponent<SpriteRenderer>().color = new Color(255.0f, 255.0f, 255.0f);
        }
    }
}

