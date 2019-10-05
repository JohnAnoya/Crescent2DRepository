using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrescentFollow : MonoBehaviour
{

    public GameObject Player;
    float NewPosition;
    const float Subtractor = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        NewPosition = this.transform.position.y + 3.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Player)
        {
            this.transform.position = new Vector3(Player.transform.position.x - 2.0f, Player.transform.position.y + 3.0f + Mathf.PingPong(Time.time, NewPosition) - Subtractor * NewPosition, this.transform.position.z);
        }
    }
}