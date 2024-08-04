using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public float speed = 10f;
    public float angle = 0f;
    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        float angleRad = angle * Mathf.Deg2Rad;

        Vector2 velocity = new Vector2(Mathf.Cos(angleRad),Mathf.Sin(angleRad))* speed;

        rb.velocity = velocity;
    }

    void Update()
    {
        
    }
}
