using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Character : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 movePlayerHorizontal;
    private Vector2 movePlayerVertical;
    public float WalkSpeed;
    public float JumpHeight;



	public bool isFacingRight;

	private Animator anim;
	

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
        WalkSpeed = 10.0f;
        JumpHeight = 10.0f;
		isFacingRight = true;
    }

    // Update is called once per frame
    void Update()
    {
		float moveDirection = Input.GetAxis("Horizontal");


		Vector3 horizontal = new Vector3(Input.GetAxis("Horizontal"), 0.0f);
        transform.position += horizontal * WalkSpeed * Time.deltaTime;

		if (anim)
		{
			anim.SetFloat("Speed", Mathf.Abs(moveDirection));
		}

		if ((moveDirection < 0 && isFacingRight) || moveDirection > 0 && !isFacingRight)
		{
			flip();
		}

		if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("Is Jumping lol");
            rb.AddForce(new Vector2(0.0f, JumpHeight), ForceMode2D.Impulse);
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

}
