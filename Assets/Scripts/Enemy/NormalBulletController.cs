using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBulletController : BulletController
{
    private float time = 0.0f;
    private StageManager stageManager;

    public override void Initialize(float speed, float angle, float gravity = 0.0f)
    {
        rb = GetComponent<Rigidbody2D>();
        float angleRad = angle * Mathf.Deg2Rad;
        Vector2 velocity = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * speed;
        rb.velocity = velocity;
        rb.gravityScale = gravity;
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
    }
    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > 5.0f || !stageManager.isOnGame)
        {
            Destroy(gameObject);
        }
    }
}
