using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
	public SurfaceEffector2D se2d;

	public bool facingRight;

    // Start is called before the first frame update
    void Start()
    {
		se2d = GetComponent<SurfaceEffector2D>();

		se2d.speed = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			if (facingRight)
			{
				se2d.speed = 5.0f;
			}
			else
			{
				se2d.speed = -5.0f;
			}
		}
		else
		{
			se2d.speed = 0.0f;
		}
	}

}
