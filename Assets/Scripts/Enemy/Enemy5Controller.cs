using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy5Controller : EnemyController
{
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private float bulletSpeed;

    private float angle = 0.0f;
    private float radius = 3.0f;
    private Vector2 centerPos;
    private Vector2 posOffset;
    private Vector3 scale;
    private float timeScale = 0.7f;

    private PlayerController playerCtrl;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        scale = transform.localScale;
        centerPos = transform.position;
        InvokeRepeating("Attack", 0f, 5.0f);

        playerCtrl = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void FixedUpdate()
    {
        angle += Time.fixedDeltaTime * timeScale;
        posOffset.x = radius * Mathf.Cos(angle);
        posOffset.y = radius * Mathf.Sin(angle);
        transform.position = centerPos + posOffset;

        if (playerCtrl.transform.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
        }
        else
        {
            transform.localScale = scale;
        }
    }

    protected override void Attack()
    {
        GameObject b = Instantiate(bullet, transform.position, transform.rotation);
        Vector2 direction = playerCtrl.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        b.GetComponent<BulletController>().Initialize(bulletSpeed, angle);
        b.GetComponent<SpriteRenderer>().color = new Color32(0, 100, 255, 255);
    }

    protected override void Die()
    {
        anim.SetTrigger("Die");
    }
}
