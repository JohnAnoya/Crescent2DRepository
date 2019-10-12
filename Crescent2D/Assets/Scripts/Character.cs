﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Character : MonoBehaviour
{

 //-- PLAYER VARIABLES --// 
    private Animator anim;
    private Rigidbody2D rb;

    public Rigidbody2D DashEffect;
    public Rigidbody2D PowerupEffect; 

    public LayerMask isGroundLayer;
    public Transform groundCheck;

    private Vector2 movePlayerHorizontal;
    private Vector2 movePlayerVertical;

    float WalkSpeed;
    float JumpHeight;
    float groundCheckRadius;
    float FlingDirection;

    public int PowerUpCount; 

    public GameObject MainCamera;
    public GameObject CameraHolder;
    public GameObject Crescent; 

    public bool isGrounded;
    public bool isFacingRight;
    bool PlayerCanMove;
    bool HasKey;
    bool isCrouching; 
	//-- PLAYER VARIABLES --// 

	//-- PLAYER AUDIO --// 
	public AudioClip playerJumpSnd;

	public AudioClip playerLightAttackSnd;
	public AudioClip playerHurtSnd;
    //-- PLAYER AUDIO --//

    //-- CRESCENT VARIABLES --// 
    float NewPosition;
    const float Subtractor = 1.5f;

    public Rigidbody2D CrescentLight;

    bool CrescentCanLight;
    bool CrescentCanMove;
    bool CrescentCanFollow; 
    //-- CRESCENT VARIABLES --// 

    //-- CAMERA VARIABLES --// 
    private bool CameraFollowPlayer;
    private bool CameraFollowCrescent; 
    float camx, camy, camz = 0.0f;
    float camTransSpeed;
    float camStart;
    float camShakeSpeed;
    float ShakeDuration;
    float ShakeMagnitude; 
    //-- CAMERA VARIABLES --// 
    

    // Start is called before the first frame update
    void Start()
    {
        //-- SETTING PLAYER VARIABLES --//

        rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();

        PowerUpCount = 0; 

        WalkSpeed = 10.0f;
        JumpHeight = 13.0f;
        groundCheckRadius = 0.1f;

        isFacingRight = true;
        PlayerCanMove = true;

        HasKey = false;
        isCrouching = false;
        //-- SETTING PLAYER VARIABLES --//

        //-- SETTING CRESCENT VARIABLES --// 

        NewPosition = 1.0f; //Setting a default variable number to avoid errors with Mathf.PingPong 

        if (!Crescent)
        {
          Crescent = GameObject.Find("Crescent");
        }

        else if (Crescent)
        {
            NewPosition = Crescent.transform.position.y + 3.0f;
        }

        CrescentCanFollow = true; 
        CrescentCanLight = true;
        CrescentCanMove = false;
        //-- SETTING CRESCENT VARIABLES --// 


        //-- SETTING CAMERA VARIABLES --// 
        CameraFollowPlayer = true;
        CameraFollowCrescent = false; 
        camTransSpeed = 12.0f;
        camShakeSpeed = 3.0f; 
        camStart = Camera.main.orthographicSize;
        ShakeDuration = 0.4f;
        ShakeMagnitude = 0.6f; 
        //-- SETTING CAMERA VARIABLES --// 
    }

    // Update is called once per frame
    void Update()
    {
		float moveDirection = Input.GetAxis("Horizontal");

		isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, isGroundLayer);

        if (PlayerCanMove == true)
        {
            Vector3 horizontal = new Vector3(Input.GetAxis("Horizontal"), 0.0f);
            transform.position += horizontal * WalkSpeed * Time.deltaTime;
		}

        if (CrescentCanMove == true)
        {
          Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
          Crescent.gameObject.transform.position += movement * WalkSpeed * Time.deltaTime;
        }

		if (anim)
		{
			anim.SetFloat("Speed", Mathf.Abs(moveDirection));

			anim.SetBool("isGrounded", isGrounded);
		}

		if ((moveDirection < 0 && isFacingRight) || moveDirection > 0 && !isFacingRight)
		{
			flip();
		}

		if (Input.GetButtonDown("Jump") && isGrounded == true)
        {
            Debug.Log("Is Jumping lol");
            anim.Play("PlayerJump");
            rb.AddForce(new Vector2(0.0f, JumpHeight), ForceMode2D.Impulse);
			AudioManager.instance.alterPitchEffect(playerJumpSnd, playerJumpSnd);

		}

        //-- PLAYER ATTACKING IF STATEMENTS (BOTH KEYBOARD/CONTROLLER) --// 
        if (Input.GetButtonDown("Fire1") || Input.GetKey(KeyCode.Joystick1Button10))
        {
            gameObject.transform.GetChild(1).GetComponent<BoxCollider2D>().enabled = true;
            anim.SetBool("QuickAttack", true);
			
			StartCoroutine(DisableSwordCollider());
			AudioManager.instance.alterPitchEffect(playerLightAttackSnd, playerLightAttackSnd);
		}
        //-- PLAYER ATTACKING IF STATEMENTS (BOTH KEYBOARD/CONTROLLER) --// 


        //-- PLAYER MOVEMENT MECHANICS STATEMENTS (BOTH KEYBOARD/CONTROLLER) --// 
        if (Input.GetKeyDown(KeyCode.F) && isFacingRight && isCrouching == false && isGrounded == false || Input.GetButtonDown("Dash") && isFacingRight && isCrouching == false && isGrounded == false)
        {
            Rigidbody2D DashEffectSource = Instantiate(DashEffect, gameObject.transform.position, gameObject.transform.rotation);
            DashEffectSource.transform.parent = gameObject.transform;

            rb.velocity = new Vector2(10.5f, 0.0f);
            ShakeDuration = 0.4f;
            ShakeMagnitude = 0.12f; 
            CameraShake();
        }

        else if (Input.GetKeyDown(KeyCode.F) && !isFacingRight && isCrouching == false && isGrounded == false || Input.GetButtonDown("Dash") && !isFacingRight && isCrouching == false && isGrounded == false)
        {
            Rigidbody2D DashEffectSource = Instantiate(DashEffect, gameObject.transform.position, gameObject.transform.rotation);
            DashEffectSource.transform.parent = gameObject.transform;

            rb.velocity = new Vector2(-10.5f, 0.0f);
            ShakeDuration = 0.4f;
            ShakeMagnitude = 0.12f;
            CameraShake();
        }

        if (Input.GetKeyDown(KeyCode.R) && PowerUpCount == 3)
        {
            Debug.Log(PowerUpCount);
            Rigidbody2D PowerupEffectSource = Instantiate(PowerupEffect, gameObject.transform.position, gameObject.transform.rotation);
            PowerupEffectSource.transform.parent = gameObject.transform;
            anim.runtimeAnimatorController = Resources.Load("Animations/GunAnim") as RuntimeAnimatorController;

            ShakeDuration = 1.5f; 
            CameraShake();

            StartCoroutine(StopGunPowerUp());
        }
        //-- PLAYER MOVEMENT MECHANICS STATEMENTS (BOTH KEYBOARD/CONTROLLER) --// 


        //-- CRESCENT LIGHT TRANSFORM IF STATEMENTS (BOTH KEYBOARD/CONTROLLER) --// 
        if (Input.GetKey(KeyCode.V) && CrescentCanLight == true || Input.GetButtonDown("CrescentTransform") && CrescentCanLight == true)
        {
            CrescentCanLight = false;
            CrescentCanFollow = false;

            CameraFollowPlayer = false;
            CameraFollowCrescent = true; 

            PlayerCanMove = false; 

            Rigidbody2D LightSource = Instantiate(CrescentLight, Crescent.transform.position, Crescent.transform.rotation);
            LightSource.gameObject.transform.parent = GameObject.Find("Crescent").transform;

            ShakeDuration = 5.5f;
            StartCoroutine(CrescentLightMove());
            CameraShake();
        }

        else if (Input.GetKey(KeyCode.V) && CrescentCanLight == false || Input.GetButtonDown("CrescentTransform") && CrescentCanLight == false)
        {
            StartCoroutine(StopCrescentLight());
        }
        //-- CRESCENT LIGHT TRANSFORM IF STATEMENTS (BOTH KEYBOARD/CONTROLLER) --// 


        if (MainCamera)
        {
            CameraFollow();
            CameraScale();
        }

        else if (!MainCamera)
        {
            Debug.Log("No camera, setting the camera");
            MainCamera = GameObject.Find("Camera/Main Camera");
        }

        if (!CameraHolder)
        {
          CameraHolder = GameObject.Find("Camera");
        }

        if (!Crescent)
        {
          Crescent = GameObject.Find("Crescent");
        }

        else if (Crescent)
        {
          CrescentFollow();
        }

        if (Input.GetAxis("Horizontal") < 0)
        {
            isFacingRight = false;
        }

        else if (Input.GetAxis("Horizontal") > 0)
        {
            isFacingRight = true;  
        }

    }


     void OnCollisionEnter2D(Collision2D collision)
    {
        float distance = collision.gameObject.transform.position.x - gameObject.transform.position.x;

        if (collision.gameObject.tag.Contains("Enemy")) {

            PlayerCanMove = false;
            CameraShake();

            if (distance < 0)
            {
              FlingDirection = 15.0f;
            }

            else if (distance > 0)
            {
              FlingDirection = -15.0f;
            }

            StartCoroutine(Fling());

			AudioManager.instance.alterPitchEffect(playerHurtSnd, playerHurtSnd);
        }
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
       if (collision.gameObject.tag == "Key")
        {
            HasKey = true;
            Destroy(collision.gameObject);
        }

        else if (collision.gameObject.tag == "LockedDoor" && HasKey == true && SceneManager.GetActiveScene().buildIndex < 6)
        {
            SceneManager.LoadScene("Map1");
        }
    }

    // flips the sprite when player walks left
    void flip()
	{
		isFacingRight = !isFacingRight;
		Vector3 scaleFactor = transform.localScale;
		scaleFactor.x *= -1;
		transform.localScale = scaleFactor;
	}


    //-- CRESCENT FUNCTIONS --// 
    void CrescentFollow()
    {
        if (CrescentCanFollow == true)
        {
          Crescent.transform.position = new Vector3(gameObject.transform.position.x - 2.0f, gameObject.transform.position.y + 3.0f + (Mathf.PingPong(Time.time, 1.0f) - Subtractor * NewPosition), Crescent.transform.position.z);
        }
    }
    //-- CRESCENT FUNCTIONS --// 


    //-- CAMERA FUNCTIONS --// 

    void CameraScale()
    {
        if (Input.GetKey(KeyCode.E) && Camera.main.orthographicSize < 15.0f || Input.GetButton("RightBumper") && Camera.main.orthographicSize < 15.0f)
        {
            Vector3 Zoom = new Vector3(camx, camy + 2.25f, camz);
            MainCamera.transform.position += Zoom * Time.deltaTime;
            Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, camStart + 10.0f, camTransSpeed * Time.deltaTime);
            // *ANOTHER WAY OF TEMPERING WITH THE CAMERA*
        }

        if (Input.GetKey(KeyCode.Q) && Camera.main.orthographicSize > 2.14f || Input.GetButton("LeftBumper") && Camera.main.orthographicSize > 2.14f)
        {
            Vector3 Zoom = new Vector3(camx, camy - 2.25f, camz);
            MainCamera.transform.position += Zoom * Time.deltaTime;
            Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, camStart - 10.0f, camTransSpeed * Time.deltaTime);
            // *ANOTHER WAY OF TEMPERING WITH THE CAMERA*
        }
    }

    void CameraFollow()
    {
        if (CameraFollowPlayer == true)
        {
            MainCamera.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, MainCamera.transform.position.z);
        }

        else if (CameraFollowCrescent == true && Crescent)
        {
            MainCamera.transform.position = new Vector3(Crescent.transform.position.x, Crescent.transform.position.y, MainCamera.transform.position.z);
        }
    }

    public void CameraShake()
    {
        CameraHolder.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, MainCamera.transform.position.z);
        StartCoroutine(CameraShaking());
    }
    //-- CAMERA FUNCTIONS --// 


    //-- COROUTINES --// 
    IEnumerator Fling()
    {
        rb.AddForce(new Vector2(FlingDirection + gameObject.transform.position.x * Time.deltaTime, 10.5f), ForceMode2D.Impulse);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
        yield return new WaitForSeconds(0.25f);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
        yield return new WaitForSeconds(1.0f);
        PlayerCanMove = true;
    }

    IEnumerator DisableSwordCollider()
    {
        yield return new WaitForSeconds(0.1f);
        gameObject.transform.GetChild(1).GetComponent<BoxCollider2D>().enabled = false;
		anim.SetBool("QuickAttack", false);
    }

    IEnumerator CameraShaking()
    {
        Vector3 initialPos = MainCamera.transform.localPosition;
        Vector2 newPos; 

        float elapsedTime = 0.0f;

        while (elapsedTime < ShakeDuration)
        {
            newPos.x = Random.Range(-1.0f, 1.0f) * ShakeMagnitude;
            newPos.y = Random.Range(-1.0f, 1.0f) * ShakeMagnitude;

            MainCamera.transform.localPosition = new Vector3(newPos.x, newPos.y, initialPos.z);
            elapsedTime += Time.deltaTime;

            yield return null; 
        }

        ShakeDuration = 0.4f;
        ShakeMagnitude = 0.6f; 
    }

    IEnumerator CrescentLightMove()
    {
        yield return new WaitForSeconds(5.5f);
        CrescentCanMove = true;
    }

    IEnumerator StopCrescentLight()
    {
        if (CrescentCanMove == true)
        {
            yield return new WaitForSeconds(0.15f);

            CrescentCanLight = true;
            CrescentCanFollow = true;

            CameraFollowPlayer = true;
            CameraFollowCrescent = false;

            PlayerCanMove = true;
        }
    }

    IEnumerator StopGunPowerUp()
    {
        yield return new WaitForSeconds(30.0f);
        ShakeDuration = 0.8f;
        CameraShake();
        PowerUpCount = 0;
        anim.runtimeAnimatorController = Resources.Load("Animations/Player") as RuntimeAnimatorController;
    }
    //-- COROUTINES --// 

}
