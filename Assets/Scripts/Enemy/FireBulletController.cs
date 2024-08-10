using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBulletController : MonoBehaviour
{
    private Rigidbody2D rb;
    private float time = 0.0f;

    public void Initialize(float speed, float angle, float gravity = 0.0f)
    {
        rb = GetComponent<Rigidbody2D>();
        float angleRad = angle * Mathf.Deg2Rad;
        Vector2 velocity = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * speed;
        rb.velocity = velocity;
        rb.gravityScale = gravity;
    }

    void Update()
    {
        if (rb.velocity != Vector2.zero)
        {
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
        time += Time.deltaTime;
        if (time > 5.0f)
        {
            Destroy(gameObject);
        }
    }
}
