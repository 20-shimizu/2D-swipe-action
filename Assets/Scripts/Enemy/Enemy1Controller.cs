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
    private float xOffset = 0.0f;
    private float startX;
    private float amplitude = 1.0f;
    private float frequency = 0.2f;
    private float stopDuration = 1.0f;
    private bool isAttacking = false;
    private bool hasAttacked = false;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.Find("AudioSource").GetComponent<AudioManager>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        shotPoint = transform.Find("ShotPoint").gameObject;
        startX = transform.position.x;
    }

    void FixedUpdate()
    {
        if (!isAttacking)
        {
            time += Time.fixedDeltaTime;

            xOffset = Mathf.Sin(2 * Mathf.PI * frequency * time) * amplitude;
            rb.position = new Vector2(startX + xOffset, rb.position.y);

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
        audioManager.PlaySE("NormalBullet");
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
