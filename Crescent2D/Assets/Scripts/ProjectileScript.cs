using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public GameObject Player;
    float ProjectileSpeed; 
    // Start is called before the first frame update
    void Start()
    {
      if (!Player)
        {
            Player = GameObject.Find("Player");
        }

        ProjectileSpeed = 9.0f; 
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, new Vector3(Player.transform.position.x, Player.transform.position.y - 0.5f, Player.transform.position.z), 1.0f * ProjectileSpeed * Time.deltaTime);
        StartCoroutine(DeleteProjectile());
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Enemy2" && gameObject.tag == "Projectile")
        {
            Destroy(gameObject);
        }
    } 

    IEnumerator DeleteProjectile()
    {
        yield return new WaitForSeconds(3.5f);
        Destroy(gameObject);
    }
}
