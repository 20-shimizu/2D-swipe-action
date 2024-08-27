using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy4Controller : EnemyController
{
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private float bulletSpeed;

    private float stopDuration = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    protected override void Attack()
    {
        GameObject b1 = Instantiate(bullet, transform.position, transform.rotation);
        GameObject b2 = Instantiate(bullet, transform.position, transform.rotation);
        b1.GetComponent<BulletController>().Initialize(bulletSpeed, 60.0f, gravity: 1.0f);
        b2.GetComponent<BulletController>().Initialize(bulletSpeed, 120.0f, gravity: 1.0f);
        // anim.SetTrigger("Attack");
    }

    protected override void Die()
    {
        rb.velocity = Vector2.zero;
        anim.SetTrigger("Die");
    }

    private IEnumerator AttackAndJump()
    {
        yield return new WaitForSeconds(stopDuration);
        Attack();
        yield return new WaitForSeconds(stopDuration);
        rb.AddForce(new Vector2(0.0f, 500.0f));
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            anim.SetBool("Jump", false);
            StartCoroutine(AttackAndJump());
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            anim.SetBool("Jump", true);
        }
    }
}
