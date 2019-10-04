using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingGreenSlime : Enemy
{
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
            Destroy(gameObject);
        }
    }
}
