using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingGreenSlime : Enemy
{

	public AudioClip flyingSlimeHitSnd;

    // Start is called before the first frame update
    void Start()
    {
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
