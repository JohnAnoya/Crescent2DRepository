using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 movePlayer; 
    public float WalkSpeed; 
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        WalkSpeed = 10.0f; 
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveVector = new Vector2(Input.GetAxis("Horizontal"), 0.0f);
        movePlayer = moveVector * WalkSpeed;
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movePlayer * Time.fixedDeltaTime);
    }
}
