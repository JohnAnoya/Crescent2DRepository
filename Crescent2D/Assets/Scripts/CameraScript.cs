using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    public GameObject Player;
    private bool FollowPlayer;
    float camx, camy, camz = 0.0f;
    float camTransSpeed;
    float camStart;

    // Start is called before the first frame update
    void Start()
    {
        FollowPlayer = true;
        camTransSpeed = 12.0f;
        camStart = Camera.main.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (Player && FollowPlayer == true)
        {
            this.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, this.transform.position.z);
        }

        if (Input.GetKey(KeyCode.E) && Camera.main.orthographicSize < 15.0f)
        {
            Vector3 Zoom = new Vector3(camx, camy + 2.25f, camz);
            transform.position += Zoom * Time.deltaTime;
            Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, camStart + 10.0f, camTransSpeed * Time.deltaTime);
            // *ANOTHER WAY OF TEMPERING WITH THE CAMERA*
        }

        if (Input.GetKey(KeyCode.Q) && Camera.main.orthographicSize > 2.14f)
        {
            Vector3 Zoom = new Vector3(camx, camy - 2.25f, camz);

            transform.position += Zoom * Time.deltaTime;

            Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, camStart - 10.0f, camTransSpeed * Time.deltaTime);
            // *ANOTHER WAY OF TEMPERING WITH THE CAMERA*
        }
    }
}
