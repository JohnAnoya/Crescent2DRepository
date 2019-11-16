using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Character : MonoBehaviour
{

 //-- PLAYER VARIABLES --// 
    private Animator anim;
    private Rigidbody2D rb;

    public Rigidbody2D DashEffect;
    public Rigidbody2D PowerupEffect; 

    public LayerMask isGroundLayer;
    public Transform groundCheck;
    public Transform BulletSpawn;

    public Rigidbody2D Bullet; 

    public Image HealthBar; 

    private Vector2 movePlayerHorizontal;
    private Vector2 movePlayerVertical;

    float initialHealth;
    public float PlayerHealth; 
    float WalkSpeed;
    float JumpHeight;
    float groundCheckRadius;
    float FlingDirection;
    float projectileForce; 

    TextMeshProUGUI Title;
    TextMeshProUGUI Level;
    TextMeshProUGUI Info; 

    public Transform UIPopUp;

    public int PowerUpCount; 

    public GameObject MainCamera;
    public GameObject CameraHolder;
    public GameObject Crescent;
    public GameObject DeathPanel;

    public bool isDead;
    public bool isGrounded;
    public bool isFacingRight;
    public bool GunnerRunnerEnabled;
    bool PlayerCanMove;
    bool playerCanInteract;
    bool HasKey;
    bool isCrouching;
    bool UIOpen;
    bool CanShootAgain;  
	//-- PLAYER VARIABLES --// 

	//-- PLAYER AUDIO --// 
	public AudioClip playerJumpSnd;
	public AudioClip playerDashSnd;

	public AudioClip pickedUpItemSnd;

	public AudioClip playerLightAttackSnd;
	public AudioClip playerHurtSnd;

	public AudioClip playerLootSnd;

	private bool audioPlayed;

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

    void Awake()
    {
        DeathPanel = GameObject.Find("UI/Canvas/DeathPanelBorder");

        if (GameObject.Find("UI/Canvas/Info")) {
            Info = GameObject.Find("UI/Canvas/Info").GetComponent<TextMeshProUGUI>();
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f; 

        //-- SETTING PLAYER VARIABLES --//
        rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();

        PowerUpCount = 0;

        PlayerHealth = 150.0f;
        initialHealth = 150.0f; 
        WalkSpeed = 10.0f;
        JumpHeight = 13.0f;
        groundCheckRadius = 0.1f;
        projectileForce = 20.0f;

        isFacingRight = true;
        PlayerCanMove = true;

        HasKey = false;
        isCrouching = false;
        isDead = false;
        GunnerRunnerEnabled = false;
        CanShootAgain = true; 
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

		// reseting variable
		audioPlayed = false;
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

		if (anim && isDead == false)
		{
			anim.SetFloat("Speed", Mathf.Abs(moveDirection));

			anim.SetBool("isGrounded", isGrounded);
		}

		if ((moveDirection < 0 && isFacingRight) || moveDirection > 0 && !isFacingRight)
		{
			flip();
		}

		if (Input.GetButtonDown("Jump") && isGrounded == true && isDead == false)
        {
            Debug.Log("Is Jumping lol");
            anim.Play("PlayerJump");
            rb.AddForce(new Vector2(0.0f, JumpHeight), ForceMode2D.Impulse);
			AudioManager.instance.alterPitchEffect(playerJumpSnd, playerJumpSnd);
		}

        if (Input.GetButtonDown("Submit") && playerCanInteract == true && SceneManager.GetActiveScene().buildIndex < 6 && SceneManager.GetActiveScene().buildIndex != 5)
        {
            Debug.Log("Map1 should be loading....");
            SceneManager.LoadScene("Map1");
        }

        //-- PLAYER HEALTH IF STATEMENTS --// 

        if (HealthBar)
        {
            float calculateHealthBarAmount = PlayerHealth / initialHealth;
            HealthBar.fillAmount = calculateHealthBarAmount;

            if (PlayerHealth > 75.0f)
            {
                HealthBar.color = Color.green; 
            }

            else if (PlayerHealth < 75.0f)
            {
                HealthBar.color = Color.yellow;
            }

            if (PlayerHealth <= 0.0f)
            {
                isDead = true;
				if (isDead)
				FindObjectOfType<AudioManager>().DeathMusicPlay();
				
				DeathPanel.SetActive(true);
                PlayerCanMove = false;
                Time.timeScale = 0.0f; 
            }

            if (isDead && Input.GetButtonDown("Respawn"))
            {
                RespawnPlayer();
            }

            else if (isDead && Input.GetButtonDown("GoBackToMap"))
            {
                GoBackToMap();
            }
        }
        //-- PLAYER HEALTH IF STATEMENTS --// 



        //-- PLAYER ATTACKING IF STATEMENTS (BOTH KEYBOARD/CONTROLLER) --// 
        if (Input.GetButtonDown("Fire1") && isDead == false && CrescentCanMove == false && GunnerRunnerEnabled == false)
        {
            gameObject.transform.GetChild(1).GetComponent<BoxCollider2D>().enabled = true;
            anim.SetBool("QuickAttack", true);
			
			StartCoroutine(DisableSwordCollider());
			AudioManager.instance.alterPitchEffect(playerLightAttackSnd, playerLightAttackSnd);
		}

        else if (Input.GetButtonDown("Fire1") && isDead == false && CrescentCanMove == false && GunnerRunnerEnabled == true && CanShootAgain == true)
        {
            CanShootAgain = false; 
            anim.SetTrigger("Shoot");
            Rigidbody2D spawnBullet = Instantiate(Bullet, BulletSpawn.transform.position, BulletSpawn.transform.rotation);

            Physics2D.IgnoreCollision(GetComponent<Collider2D>(),
             spawnBullet.GetComponent<Collider2D>(), true);

            if (isFacingRight)
            {
                spawnBullet.GetComponent<Rigidbody2D>().AddForce(BulletSpawn.right * projectileForce, ForceMode2D.Impulse);           
            }

            else
            {
                spawnBullet.GetComponent<SpriteRenderer>().flipX = true;
                spawnBullet.GetComponent<Rigidbody2D>().AddForce(-BulletSpawn.right * projectileForce, ForceMode2D.Impulse);
            }

            StartCoroutine(ResetCanShootAgain());
        }
            //-- PLAYER ATTACKING IF STATEMENTS (BOTH KEYBOARD/CONTROLLER) --// 


            //-- PLAYER MOVEMENT MECHANICS STATEMENTS (BOTH KEYBOARD/CONTROLLER) --// 
            if (Input.GetKeyDown(KeyCode.F) && isFacingRight && isCrouching == false && isGrounded == false && isDead == false || Input.GetButtonDown("Dash") && isFacingRight && isCrouching == false && isGrounded == false && isDead == false)
        {
            Rigidbody2D DashEffectSource = Instantiate(DashEffect, gameObject.transform.position, gameObject.transform.rotation);
            DashEffectSource.transform.parent = gameObject.transform;

            rb.velocity = new Vector2(10.5f, 0.0f);

			AudioManager.instance.PlayAudioClip(playerDashSnd);

			ShakeDuration = 0.4f;
            ShakeMagnitude = 0.12f;

            CameraShake();
        }

        else if (Input.GetKeyDown(KeyCode.F) && !isFacingRight && isCrouching == false && isGrounded == false && isDead == false || Input.GetButtonDown("Dash") && !isFacingRight && isCrouching == false && isGrounded == false && isDead == false)
        {
            Rigidbody2D DashEffectSource = Instantiate(DashEffect, gameObject.transform.position, gameObject.transform.rotation);
            DashEffectSource.transform.parent = gameObject.transform;

            rb.velocity = new Vector2(-10.5f, 0.0f);

			AudioManager.instance.PlayAudioClip(playerDashSnd);

			ShakeDuration = 0.4f;
			ShakeMagnitude = 0.12f;

			CameraShake();
        }

        if (Input.GetKeyDown(KeyCode.R) && PowerUpCount >= 3 && isDead == false && GunnerRunnerEnabled == false || Input.GetButtonDown("Powerup") && PowerUpCount >= 3 && isDead == false && GunnerRunnerEnabled == false)
        {
            GunnerRunnerEnabled = true;
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
        if (Input.GetKey(KeyCode.V) && CrescentCanLight == true && isDead == false || Input.GetButtonDown("CrescentTransform") && CrescentCanLight == true && isDead == false)
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

        else if (Input.GetKey(KeyCode.V) && CrescentCanLight == false && isDead == false || Input.GetButtonDown("CrescentTransform") && CrescentCanLight == false && isDead == false)
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

        if (collision.gameObject.tag == "Enemy1")
        {
            float TakeDamage = Random.Range(5, 15);
            PlayerHealth = PlayerHealth - TakeDamage; 
        }

        else if (collision.gameObject.tag == "Enemy2")
        {
            float TakeDamage = Random.Range(15, 25);
            PlayerHealth = PlayerHealth - TakeDamage;
        }

        else if (collision.gameObject.tag == "Enemy3")
        {
            float TakeDamage = Random.Range(15, 30);
            PlayerHealth = PlayerHealth - TakeDamage;
        }

        else if (collision.gameObject.tag == "Enemy4")
        {
            float TakeDamage = Random.Range(20, 35);
            PlayerHealth = PlayerHealth - TakeDamage;
        }

        else if (collision.gameObject.tag == "LevelExit" && SceneManager.GetActiveScene().buildIndex < 6 && SceneManager.GetActiveScene().buildIndex != 5)
        {
            SceneManager.LoadScene("Map1");
        }

        else if (collision.gameObject.tag == "LevelExit" && SceneManager.GetActiveScene().buildIndex < 6 && SceneManager.GetActiveScene().buildIndex == 5)
        {
            SceneManager.LoadScene("End");
        }

        else if (collision.gameObject.tag == "HealthCollectible")
        {
            Destroy(collision.gameObject);

			AudioManager.instance.alterPitchEffect(playerLootSnd, playerLootSnd);

            if (PlayerHealth < initialHealth)
            {
                PlayerHealth += 5.0f; 
            }           
        }

        else if (collision.gameObject.tag == "Spikes")
        {
            PlayerHealth = 0.0f; 
        }
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
       if (collision.gameObject.tag == "Key")
        {
            HasKey = true;
			AudioManager.instance.PlayAudioClip(pickedUpItemSnd);
            Destroy(collision.gameObject);
        }

        else if (collision.gameObject.tag == "LockedDoor" && HasKey == true && SceneManager.GetActiveScene().buildIndex < 6)
        {
            SceneManager.LoadScene("Map1");
        }

        else if (collision.gameObject.tag == "LockedDoor" && HasKey == false && SceneManager.GetActiveScene().buildIndex < 6)
        {
            Info.text = "Door is locked (you need a key!)";
        }

        if (collision.gameObject.tag == "Exit")
        {
            collision.gameObject.GetComponent<SpriteRenderer>().color = new Color(255.0f, 0.0f, 0.0f);
        }

       if (collision.gameObject.tag == "Projectile")
        {
            PlayerCanMove = false;
            CameraShake();
            StartCoroutine(Fling());
            PlayerHealth = PlayerHealth - Random.Range(10.0f, 30.0f);        
        }

        if (collision.gameObject.tag == "Parry" && GameObject.Find("Player/Sword").GetComponent<BoxCollider2D>().enabled == false && SceneManager.GetActiveScene().buildIndex != 3)
        {
            PlayerCanMove = false;
            CameraShake();
            StartCoroutine(Fling());
            PlayerHealth = PlayerHealth - Random.Range(10.0f, 30.0f);
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Exit")
        {
            playerCanInteract = true; 
        }

        else if (collision.gameObject.tag == "Note" && UIOpen == false && GameObject.Find("NOTE") && GameObject.Find("NOTE").GetComponent<NPCMonologueTrigger>().MonologueIsOpen == false)
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
    }

    void OnTriggerExit2D(Collider2D collision) 
    {
      if(GameObject.Find("NOTE") && GameObject.Find("NOTE").GetComponent<NPCMonologueTrigger>().MonologueIsOpen == false)
        Destroy(GameObject.Find("MapUIPopUp"));
        UIOpen = false;

        if (collision.gameObject.tag == "Exit")
        {
            collision.gameObject.GetComponent<SpriteRenderer>().color = new Color(255.0f, 255.0f, 255.0f);
            playerCanInteract = false;
        }

        if (collision.gameObject.tag == "LockedDoor" && HasKey == false && SceneManager.GetActiveScene().buildIndex < 6)
        {
            Info.text = "";
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
          Crescent.transform.position = new Vector3(gameObject.transform.position.x - 3.5f, gameObject.transform.position.y + 3.0f + (Mathf.PingPong(Time.time, 1.0f) - Subtractor * NewPosition), Crescent.transform.position.z);
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


    //-- PLAYER RESPAWN FUCNTION --// 
    public void RespawnPlayer()
    {
        Time.timeScale = 1.0f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    //-- PLAYER RESPAWN FUNCTION --// 

    //-- GO BACK TO MAP FUNCTION --// 
    public void GoBackToMap()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Map1");
    }
	//-- GO BACK TO MAP FUNCTION --// 

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

            CrescentCanMove = false; 

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
        GunnerRunnerEnabled = false; 
    }

    IEnumerator ResetCanShootAgain()
    {
        yield return new WaitForSeconds(0.25f);
        CanShootAgain = true;
    }
    //-- COROUTINES --// 

}
