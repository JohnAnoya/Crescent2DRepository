using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Character : MonoBehaviour
{

 //-- PLAYER VARIABLES --// 
    private Animator anim;
    private Rigidbody2D rb;

    public LayerMask isGroundLayer;
    public Transform groundCheck;

    private Vector2 movePlayerHorizontal;
    private Vector2 movePlayerVertical;

    float WalkSpeed;
    float JumpHeight;
    float groundCheckRadius;
    float FlingDirection;

    public GameObject MainCamera;

    public bool isGrounded;
    public bool isFacingRight;
    bool PlayerCanMove;
    bool HasKey;
	//-- PLAYER VARIABLES --// 

	//-- PLAYER AUDIO --// 
	public AudioClip playerJumpSnd;

	public AudioClip playerLightAttackSnd;
	public AudioClip playerHurtSnd;
	//-- PLAYER AUDIO --//

	//-- CAMERA VARIABLES --// 
	private bool FollowPlayer;
    float camx, camy, camz = 0.0f;
    float camTransSpeed;
    float camStart;
 //-- CAMERA VARIABLES --// 


    // Start is called before the first frame update
    void Start()
    {
        //-- SETTING PLAYER VARIABLES --//
        rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
        WalkSpeed = 10.0f;
        JumpHeight = 13.0f;
		isFacingRight = true;
        PlayerCanMove = true;
        HasKey = false; 
		groundCheckRadius = 0.1f;
        //-- SETTING PLAYER VARIABLES --//


        //-- SETTING CAMERA VARIABLES --// 
        FollowPlayer = true;
        camTransSpeed = 12.0f;
        camStart = Camera.main.orthographicSize;
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

        if (MainCamera)
        {
            CameraFollow();
            CameraScale();
        }

        else if (!MainCamera)
        {
            Debug.Log("If no camera, then set the camera");
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
        if (FollowPlayer == true)
        {
            MainCamera.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, MainCamera.transform.position.z);
        }
    }

    void CameraShake()
    {

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
	//-- COROUTINES --// 

}
