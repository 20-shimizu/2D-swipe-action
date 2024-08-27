using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2Controller : EnemyController
{
    private GameObject shotPoint;
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private float bulletSpeed;

    private float angle = 0.0f;
    private Vector2 posOffset = Vector2.zero;
    private Vector2 startPos;
    private float xAmplitude = 3.0f;
    private float yAmplitude = 1.0f;
    private float timeScale = 0.7f;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        shotPoint = transform.Find("ShotPoint").gameObject;
        InvokeRepeating("Attack", 0f, 5.0f);
        startPos = transform.position;
    }

    void FixedUpdate()
    {
        angle += Time.fixedDeltaTime * timeScale;
        posOffset.x = Mathf.Sin(angle) * xAmplitude;
        posOffset.y = Mathf.Sin(angle * 2.0f) * yAmplitude;
        transform.position = startPos + posOffset;
    }

    protected override void Attack()
    {
        GameObject b1 = Instantiate(bullet, shotPoint.transform.position, transform.rotation);
        GameObject b2 = Instantiate(bullet, shotPoint.transform.position, transform.rotation);
        GameObject b3 = Instantiate(bullet, shotPoint.transform.position, transform.rotation);
        float angle = transform.localScale.x < 0 ? 0.0f : 180.0f;
        b1.GetComponent<BulletController>().Initialize(bulletSpeed, angle + 30.0f);
        b2.GetComponent<BulletController>().Initialize(bulletSpeed, angle);
        b3.GetComponent<BulletController>().Initialize(bulletSpeed, angle - 30.0f);
        // anim.SetTrigger("Attack");
    }

    protected override void Die()
    {
        CancelInvoke();
        anim.SetTrigger("Die");
    }
}
