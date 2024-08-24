using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    [SerializeField]
    private GameObject enemy;
    private Camera mainCamera;
    private float bufferDistance = 0.5f;

    void Start()
    {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    void Update()
    {
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);
        if (viewportPosition.x > -bufferDistance && viewportPosition.x < 1 + bufferDistance &&
            viewportPosition.y > -bufferDistance && viewportPosition.y < 1 + bufferDistance)
        {
            Instantiate(enemy, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
