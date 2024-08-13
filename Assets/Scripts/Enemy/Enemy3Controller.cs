using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3Controller : EnemyController
{
    private GameObject shotPoint;
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private float bulletSpeed;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        shotPoint = transform.Find("ShotPoint").gameObject;
        InvokeRepeating("Attack", 0f, 1.0f);
    }

    protected override void Attack()
    {
        GameObject b1 = Instantiate(bullet, shotPoint.transform.position, transform.rotation);
        // GameObject b2 = Instantiate(bullet, shotPoint.transform.position, transform.rotation);
        float angle = transform.localScale.x > 0 ? 0.0f : 180.0f;
        b1.GetComponent<BulletController>().Initialize(bulletSpeed, angle, reverse: false);
        // b2.GetComponent<BulletController>().Initialize(bulletSpeed, angle, reverse: true);
        // anim.SetTrigger("Attack");
    }
}
