using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunFollow : MonoBehaviour
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
            this.transform.position = new Vector3(Player.transform.position.x + 25.0f, this.transform.position.y, this.transform.position.z);
        }
    }
}
