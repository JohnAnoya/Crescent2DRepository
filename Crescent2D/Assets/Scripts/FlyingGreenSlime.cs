using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingGreenSlime : Enemy
{

	public AudioClip flyingSlimeHitSnd;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        speed = 10.0f;
        health = 75.0f;
        initialPos.x = gameObject.transform.position.x;
        initialPos.y = gameObject.transform.position.y;

        anim = GetComponent<Animator>();
        StartEnemyScript();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEnemyScript();

        if (health <= 0.0f)
        {
			AudioManager.instance.alterPitchEffect(flyingSlimeHitSnd, flyingSlimeHitSnd);
            Destroy(gameObject);
        }
    }
}
