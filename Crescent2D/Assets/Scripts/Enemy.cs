using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

/* LIST OF ENEMIES, ENEMY NAMES, AND LEVEL ASSOCIATION */
/*   {                               }   */

abstract public class Enemy : MonoBehaviour
{

    public GameObject Player;

    Rigidbody2D rb;
    Animator anim;

    public Transform HealthCollectible;
    public Transform projectileSpawnPoint;
    public Transform parryProjectile;
    public Transform regularProjectile;

    public float speed;
    public float health;
    public float initialHealth; 
    float distance;
    float height;
    float leftFollowRange;
    float rightFollowRange;
    float projectileEnemyRandomPos;
    float projectileEnemyAttackDecider; 
    float FlingDirection; 

    bool isFacingRight;
    bool EnemyFlipped;
    bool EnemyAttacking;
    bool EnemyJumpDebounce;
    bool ProjectileEnemyInAir;
    bool ProjectileEnemyCanChangePos;
    bool ProjectileEnemyCanAttack; 

    public Vector3 initialPos;

    public Image HealthBar; 

    public void StartEnemyScript()
    {
        distance = 0.0f;
        height = 0.0f; 
        leftFollowRange = 25.0f;
        rightFollowRange = -25.0f;
        projectileEnemyRandomPos = 10.0f; 
        isFacingRight = true;
        EnemyFlipped = false;
        EnemyAttacking = false;
        EnemyJumpDebounce = false;
        ProjectileEnemyInAir = true;
        ProjectileEnemyCanChangePos = true;
        ProjectileEnemyCanAttack = false; 

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
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
            Debug.Log("There is no player to follow!");
        }

        else if (Player && gameObject.tag.Contains("Enemy") && distance < leftFollowRange && distance > 0.0f || Player && gameObject.tag.Contains("Enemy") && distance > rightFollowRange && distance < 0.0f)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, Player.transform.position, 1.0f * speed * Time.deltaTime);
            anim.SetBool("Walk", true);

            if (height > 1.5f && EnemyJumpDebounce == false)
            {
                rb.AddForce(new Vector2(0.0f, 5.5f), ForceMode2D.Impulse);
                EnemyJumpDebounce = true;
                StartCoroutine(EnemyCanJump());
            }
        }

        else
        {
            anim.SetBool("Walk", false);
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, initialPos, 1.0f * speed * Time.deltaTime);
        }


        if (HealthBar)
        {
            HealthBar.fillAmount = health / initialHealth;
        }

        if (gameObject.tag == "Enemy1" && health <= 0.0f)
        {
           int HealthDropChance = Random.Range(0, 5);
           
           if (HealthDropChance > 0)
            {
                for (int i = 1; i <= HealthDropChance; i++)
                {
                   
                    Instantiate(HealthCollectible, gameObject.transform.position, gameObject.transform.rotation);
                }
            }
        }

        else if (gameObject.tag == "Enemy3" && health <= 0.0f || gameObject.tag == "Enemy4" && health <= 0.0f)
        {
            int HealthDropChance = Random.Range(0, 10);

            if (HealthDropChance > 0)
            {
                for (int i = 1; i <= HealthDropChance; i++)
                {
                    Instantiate(HealthCollectible, gameObject.transform.position, gameObject.transform.rotation);
                }
            }
        }
    }


    public void UpdateProjectileEnemyScript()
    { 
        if (!Player)
        {
            Player = GameObject.Find("Player");
        }

        else

        distance = gameObject.transform.position.x - Player.transform.position.x;
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
            Debug.Log("There is no player to follow!");
        }

       
        else if (Player && gameObject.tag.Contains("Enemy") && distance < leftFollowRange && distance > 0.0f && ProjectileEnemyInAir == true || Player && gameObject.tag.Contains("Enemy") && distance > rightFollowRange && distance < 0.0f && ProjectileEnemyInAir == true)
        {
          gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, new Vector3(Player.transform.position.x + projectileEnemyRandomPos, initialPos.y + 3.5f + (Mathf.PingPong(Time.time, 1.0f)), Player.transform.position.z), 1.0f * speed * Time.deltaTime);
          anim.SetBool("Walk", true);

            if (height > 1.5f && EnemyJumpDebounce == false)
            {
                rb.AddForce(new Vector2(0.0f, 5.5f), ForceMode2D.Impulse);
                EnemyJumpDebounce = true;
                StartCoroutine(EnemyCanJump());
            }

            if (ProjectileEnemyCanChangePos == true)
            {
                ProjectileEnemyCanChangePos = false; 
                StartCoroutine(ChangeProjectileEnemyPos());
            }
        }

        else
        {
            anim.SetBool("Walk", false);
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, initialPos, 1.0f * speed * Time.deltaTime);
        }

        if (ProjectileEnemyCanAttack == true && projectileEnemyAttackDecider <= 50.0f)
        {
            GroundAttack();
        }

        else if (ProjectileEnemyCanAttack == true && projectileEnemyAttackDecider > 50.0f && projectileEnemyAttackDecider < 85.0f)
        {
            NormalProjectileAttack(); 
        }

        else if (ProjectileEnemyCanAttack == true && projectileEnemyAttackDecider > 85.0f)
        {
            ParryProjectileAttack();
        }

        if (HealthBar)
        {
            HealthBar.fillAmount = health / initialHealth;
        }

        if (gameObject.tag == "Enemy2" && health <= 0.0f)
        {
            int HealthDropChance = Random.Range(0, 10);

            if (HealthDropChance > 0)
            {
                for (int i = 1; i <= HealthDropChance; i++)
                {
                    Instantiate(HealthCollectible, gameObject.transform.position, gameObject.transform.rotation);
                }
            }
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
            StartCoroutine(GroundAttackAnimation());
        }
        
        else if (collision.gameObject.tag == "Spikes")
        {
            health = 0.0f; 
        }
    }

    void NormalProjectileAttack()
    {
        if (ProjectileEnemyCanAttack == true)
        {
            ProjectileEnemyCanAttack = false;
            Instantiate(regularProjectile, gameObject.transform.position, gameObject.transform.rotation);
        }
    }

    void ParryProjectileAttack()
    {
        if (ProjectileEnemyCanAttack == true)
        {
            ProjectileEnemyCanAttack = false;
            var tempParryProjectile = Instantiate(parryProjectile, gameObject.transform.position, gameObject.transform.rotation);
            tempParryProjectile.parent = gameObject.transform;
        }
    }

    void GroundAttack()
    {
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, Player.transform.position, 1.0f * speed * Time.deltaTime);
        StartCoroutine(StopGroundAttack());
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


    IEnumerator GroundAttackAnimation()
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
        EnemyJumpDebounce = false; 
    }

    IEnumerator ChangeProjectileEnemyPos()
    {
        yield return new WaitForSeconds(3.5f);
        projectileEnemyRandomPos = Random.Range(-15.0f, 15.0f);
        projectileEnemyAttackDecider = Random.Range(0.0f, 100.0f);
        ProjectileEnemyCanAttack = true;

        yield return new WaitForSeconds(5.5f);
        projectileEnemyRandomPos = 10.0f;
        ProjectileEnemyCanChangePos = true;
    }

    IEnumerator StopGroundAttack()
    {
        ProjectileEnemyInAir = false;
        yield return new WaitForSeconds(1.35f);
        ProjectileEnemyInAir = true;
        ProjectileEnemyCanAttack = false; 
    }
}


