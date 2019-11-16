using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteCollectibles : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DeleteRemainingCollectibles());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DeleteRemainingCollectibles()
    {
        yield return new WaitForSeconds(30.0f);
        Destroy(gameObject);
    }
}
