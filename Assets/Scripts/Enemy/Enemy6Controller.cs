using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy6Controller : EnemyController
{
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private float bulletSpeed;

    private float attackDuration = 0.5f;
    private float stopDuration = 3.0f;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.Find("AudioSource").GetComponent<AudioManager>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        anim.SetTrigger("Attack");
    }

    // animation event から実行
    private void StartAttack()
    {
        StartCoroutine(AttackAndWait());
    }

    private IEnumerator AttackAndWait()
    {
        yield return new WaitForSeconds(attackDuration);
        Attack();
        yield return new WaitForSeconds(attackDuration);
        anim.SetTrigger("FinishAttack");
        yield return new WaitForSeconds(stopDuration);
        anim.SetTrigger("Attack");
    }

    protected override void Attack()
    {
        audioManager.PlaySE("NormalBullet");
        GameObject b1 = Instantiate(bullet, transform.position, transform.rotation);
        GameObject b2 = Instantiate(bullet, transform.position, transform.rotation);
        GameObject b3 = Instantiate(bullet, transform.position, transform.rotation);
        float angle = transform.localScale.x < 0 ? 0.0f : 180.0f;
        b1.GetComponent<BulletController>().Initialize(bulletSpeed, angle + 30.0f);
        b2.GetComponent<BulletController>().Initialize(bulletSpeed, angle);
        b3.GetComponent<BulletController>().Initialize(bulletSpeed, angle - 30.0f);
    }
}
