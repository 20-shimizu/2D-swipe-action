using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    private Vector3 pos;
    private float cameraPosZ = -10.0f;
    private Vector3 desiredPos;

    private bool isDirectFollowing;

    void Start()
    {
    }

    void LateUpdate()
    {
        if (isDirectFollowing)
        {
            pos = desiredPos;
            pos.z = cameraPosZ;
            transform.position = pos;
        }
        else
        {
            pos += (desiredPos - transform.position) * 0.2f;
            pos.z = cameraPosZ;
            transform.position = pos;
        }
    }

    public void SetDesiredPos(Vector3 p, bool isDirect)
    {
        isDirectFollowing = isDirect;
        desiredPos = p;
    }
}
