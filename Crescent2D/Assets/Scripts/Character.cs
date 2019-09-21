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

	private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
        WalkSpeed = 10.0f;
        JumpHeight = 10.0f; 
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 horizontal = new Vector3(Input.GetAxis("Horizontal"), 0.0f);
        transform.position += horizontal * WalkSpeed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Is Jumping lol");
            rb.AddForce(new Vector2(0.0f, JumpHeight), ForceMode2D.Impulse);
        }
     
    }



}
