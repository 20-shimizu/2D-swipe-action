using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveBulletController : BulletController
{
    private float time = 0.0f;
    private StageManager stageManager;

    private float amplitude = 7.0f;
    private float frequency = 5.0f;
    private float speed;
    private Vector2 direction;
    private Vector2 perpendicular;
    private bool isReverse;

    public override void Initialize(float speed, float angle, bool reverse = false, float gravity = 0.0f)
    {
        rb = GetComponent<Rigidbody2D>();
        this.speed = speed;
        float angleRad = angle * Mathf.Deg2Rad;
        direction = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        perpendicular = new Vector2(-direction.y, direction.x);
        isReverse = reverse;
        rb.gravityScale = gravity;
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
    }
    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        Vector2 baseVelocity = direction * speed;
        Vector2 waveVelocity = perpendicular * amplitude * Mathf.Cos(frequency * time);
        if (isReverse) waveVelocity = perpendicular * amplitude * Mathf.Cos(frequency * time + Mathf.PI);
        rb.velocity = baseVelocity + waveVelocity;

        if (time > 5.0f || !stageManager.isOnGame)
        {
            Destroy(gameObject);
        }
    }
}
