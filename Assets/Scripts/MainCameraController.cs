using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    private GameObject player;
    Vector3 pos;
    float cameraPosZ = -10.0f;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        FollowPlayer();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        pos = player.transform.position;
        pos.z = cameraPosZ;
        transform.position = pos;
    }
}
