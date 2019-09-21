using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    public GameObject Player;
    private bool FollowPlayer; 

    // Start is called before the first frame update
    void Start()
    {
        FollowPlayer = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Player && FollowPlayer == true)
        {
            this.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, this.transform.position.z);
        }

    }
}
