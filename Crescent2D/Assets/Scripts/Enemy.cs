using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* LIST OF ENEMIES, ENEMY NAMES, AND LEVEL ASSOCIATION */
/*   {                               }   */

abstract public class Enemy : MonoBehaviour
{

    public GameObject Player;

    Rigidbody2D rb;
    Animator anim; 

    float speed;
    public float health;
    float distance;
    float height;
    float leftFollowRange;
    float rightFollowRange;
    float FlingDirection; 

    bool isFacingRight;
    bool EnemyFlipped;
    bool EnemyAttacking;
    bool EnemyJump; 

    Vector3 initialPos;

    public void StartEnemyScript()
    {
        distance = 0.0f;
        height = 0.0f; 
        leftFollowRange = 25.0f;
        rightFollowRange = -25.0f; 
        isFacingRight = true;
        EnemyFlipped = false;
        EnemyAttacking = false;
        EnemyJump = false; 

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        if (gameObject.tag == "Enemy19")
        {
            speed = 5.0f;
            health = 100.0f;
            initialPos.x = gameObject.transform.position.x;
            initialPos.y = gameObject.transform.position.y;
        }

       else if (gameObject.tag == "Enemy5")
        {
            speed = 10.0f;
            health = 75.0f;
            initialPos.x = gameObject.transform.position.x;
            initialPos.y = gameObject.transform.position.y; 
        }
    }

   public void UpdateEnemyScript()
    {
        if (!Player)
        {
           Player = GameObject.Find("Player");  
        }

        else

        distance =  gameObject.transform.position.x - Player.transform.position.x;
        height = Player.transform.position.y - gameObject.transform.position.y;

        if (distance > 0.0f && isFacingRight == true)
        {
            isFacingRight = false;
            FlipEnemy();
        }

        else if (distance < 0.0f && isFacingRight == false)
        {
           
            isFacingRight = true;
            FlipEnemy();
        }

        if (distance > 0.0f)
        {
          FlingDirection = 3.5f;
        }

        else if (distance < 0.0f)
        {
          FlingDirection = -3.5f;
        }


        if (!Player)
        {
            Debug.Log("There is no player set!");
        }

        else if (Player && gameObject.tag == "Enemy19" && distance < leftFollowRange && distance > 0.0f || Player && gameObject.tag == "Enemy19" && distance > rightFollowRange && distance < 0.0f)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, Player.transform.position, 1.0f * speed * Time.deltaTime);
            anim.SetBool("Walk", true);

            if (height > 0.0f && EnemyJump == false)
            {
                rb.AddForce(new Vector2(0.0f, 5.5f), ForceMode2D.Impulse);
                EnemyJump = true;
                StartCoroutine(EnemyCanJump());
            } 
        }

        else if (Player && gameObject.tag == "Enemy5" && distance < leftFollowRange && distance > 0.0f || Player && gameObject.tag == "Enemy5" && distance > rightFollowRange && distance < 0.0f)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, Player.transform.position, 1.0f * speed * Time.deltaTime);
            anim.SetBool("Walk", true);
        }

        else
        {
            anim.SetBool("Walk", false);
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, initialPos, 1.0f * speed * Time.deltaTime);
        }
    }



    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BasicSwordAttack")
        {
            health = health - Random.Range(5.0f, 20.0f);
            anim.SetTrigger("Hit");
            StartCoroutine(EnemyFling());
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            EnemyAttacking = true; 
            StartCoroutine(GroundAttack());
        }
    }

    void FlyingAttack()
    {

    }

    void FlipEnemy ()
    {
        // Keep a copy of 'localScale' because scale cannot be changed directly
        Vector3 scaleFactor = transform.localScale;

        // Change sign of scale in 'x'
        scaleFactor.x *= -1; // or - -scaleFactor.x

        // Assign updated value back to 'localScale'
        gameObject.transform.localScale = scaleFactor;
    }


    IEnumerator GroundAttack()
    {
        if (EnemyAttacking == true)
        {
           anim.SetBool("Attack", EnemyAttacking);
        }

        yield return new WaitForSeconds(1.0f);
        EnemyAttacking = false;
        anim.SetBool("Attack", EnemyAttacking);
    }

    IEnumerator EnemyFling()
    {
        rb.AddForce(new Vector2(FlingDirection, 1.5f), ForceMode2D.Impulse);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
        yield return new WaitForSeconds(0.25f);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
    }

    IEnumerator EnemyCanJump()
    {
        yield return new WaitForSeconds(1.0f);
        EnemyJump = false; 
    }
}


