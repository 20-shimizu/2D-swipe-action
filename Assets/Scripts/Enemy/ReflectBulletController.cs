using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectBulletController : BulletController
{
    private float time = 0.0f;
    private StageManager stageManager;
    private float speed;
    private Vector2 direction;

    public override void Initialize(float speed, float angle, bool reverse = false, float gravity = 0.0f)
    {
        rb = GetComponent<Rigidbody2D>();
        float angleRad = angle * Mathf.Deg2Rad;
        Vector2 velocity = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * speed;
        rb.velocity = velocity;
        rb.gravityScale = gravity;
        this.speed = speed;
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
    }

    void Update()
    {
        direction = rb.velocity.normalized;
        if (rb.velocity != Vector2.zero)
        {
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
        time += Time.deltaTime;
        if (time > 5.0f || !stageManager.isOnGame)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            Vector2 normal = other.contacts[0].normal;
            Vector2 reflectDirection = Vector2.Reflect(direction, normal);
            rb.velocity = reflectDirection * speed;
        }
    }
}
