using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrescentFollow : MonoBehaviour
{

    public GameObject Player;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Player)
        {
            this.transform.position = new Vector3(Player.transform.position.x - 2.0f, Player.transform.position.y + 1.0f, this.transform.position.z);
            StartCoroutine(CrescentBobbing());
        }
    }

    IEnumerator CrescentBobbing()
    {
        yield return new WaitForSeconds(4.0f);
        
        for (int i = 0; i < 5; i++)
        {
            this.transform.position = new Vector3(Player.transform.position.x, this.transform.position.y - i, this.transform.position.z);

            yield return new WaitForSeconds(1.3f);

            for (int j = 0; j < 5; j++)
            {
                this.transform.position = new Vector3(Player.transform.position.x, this.transform.position.y + j, this.transform.position.z);
            }
        }


    

    }
}
