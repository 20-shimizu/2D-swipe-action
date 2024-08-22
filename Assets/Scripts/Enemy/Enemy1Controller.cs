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

    private float time = 0.0f;
    private float yOffset = 0.0f;
    private float startY;
    private float amplitude = 1.0f;
    private float frequency = 0.2f;
    private float stopDuration = 1.0f;
    private bool isAttacking = false;
    private bool hasAttacked = false;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        shotPoint = transform.Find("ShotPoint").gameObject;
        startY = transform.position.y;
    }

    void FixedUpdate()
    {
        if (!isAttacking)
        {
            time += Time.fixedDeltaTime;

            yOffset = Mathf.Sin(2 * Mathf.PI * frequency * time) * amplitude;
            rb.position = new Vector2(rb.position.x, startY + yOffset);

            if (Mathf.Abs(Mathf.Cos(2 * Mathf.PI * frequency * time)) < 0.02f)
            {
                if (!hasAttacked)
                {
                    rb.velocity = Vector2.zero;
                    isAttacking = true;
                    StartCoroutine(AttackAndWait());
                }
            }
            else
            {
                hasAttacked = false;
            }
        }
    }

    private IEnumerator AttackAndWait()
    {
        Attack();
        yield return new WaitForSeconds(stopDuration);
        hasAttacked = true;
        isAttacking = false;
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
