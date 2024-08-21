using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Controller : EnemyController
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
        InvokeRepeating("Attack", 0f, 2.0f);
    }

    protected override void Attack()
    {
        GameObject b = Instantiate(bullet, shotPoint.transform.position, transform.rotation);
        float angle = transform.localScale.x > 0 ? 0.0f : 180.0f;
        b.GetComponent<BulletController>().Initialize(bulletSpeed, angle);
        anim.SetTrigger("Shot");
    }

    protected override void Die()
    {
        CancelInvoke();
        anim.SetTrigger("Die");
    }
}
