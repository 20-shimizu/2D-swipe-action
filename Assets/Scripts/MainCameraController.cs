using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    [SerializeField]
    private bool isFollowPlayer;
    private GameObject player;
    private GoalItem goalItem;
    private Vector3 pos;
    private float cameraPosZ = -10.0f;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        FollowPlayer();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (isFollowPlayer)
            FollowPlayer();
        if (goalItem != null)
        {
            pos = goalItem.transform.position;
            pos.z = cameraPosZ;
            transform.position = pos;
        }
    }

    private void FollowPlayer()
    {
        pos = player.transform.position;
        pos.z = cameraPosZ;
        transform.position = pos;
    }

    public void SetGoalItem(GoalItem value)
    {
        goalItem = value;
    }
}
